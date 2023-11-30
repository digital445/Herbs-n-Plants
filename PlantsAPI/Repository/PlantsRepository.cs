using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Services.PlantsAPI.DbContexts;
using Services.PlantsAPI.Models;
using Services.PlantsAPI.Models.Dto;
using System.Linq.Expressions;

namespace Services.PlantsAPI.Repository
{
	public class PlantsRepository : IPlantsRepository
	{
		private readonly ApplicationDbContext _db;
		private readonly IMapper _mapper;

		public PlantsRepository(ApplicationDbContext context, IMapper mapper)
		{
			_db = context;
			_mapper = mapper;
		}

		public async Task<PlantDto> CreateUpdatePlant(PlantDto plantDto)
		{
			Plant plant = _mapper.Map<Plant>(plantDto);

			await ValidatePlantNames(plant.Names);

			Plant? dbPlant = await _db.Plants
				.AsNoTracking()
				.FirstOrDefaultAsync(pl => pl.PlantId == plant.PlantId);

			if (dbPlant == null) //plant does not exist in the database
			{
				_db.Plants.Add(plant);
			}
			else
			{
				var dbPlantNames = await _db.PlantNames.AsNoTracking().Where(pn => pn.PlantId == plant.PlantId).ToListAsync();
				_db.PlantNames.RemoveRange(dbPlantNames); //removes old names for this plant from db
				
				var dbImageLinks = await _db.ImageLink.AsNoTracking().Where(pn => pn.PlantId == plant.PlantId).ToListAsync();
				_db.ImageLink.RemoveRange(dbImageLinks); //removes old image links for this plant from db

				_db.Plants.Update(plant);
			}

			await _db.SaveChangesAsync();

			return _mapper.Map<Plant, PlantDto>(plant);

		}

		public async Task<bool> DeletePlant(int plantId)
		{
			try
			{
				Plant? plant = await _db.Plants.FirstOrDefaultAsync(plant => plant.PlantId == plantId);
				if (plant == null)
				{
					return false;
				}
				_db.Plants.Remove(plant);
				await _db.SaveChangesAsync();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<PlantDto> GetPlantById(int plantId)
		{
			Plant? plant = await _db.Plants
				.Where(pl => pl.PlantId == plantId)
				.Include(pl => pl.Names)
				.Include(pl => pl.ImageLinks)
				.FirstOrDefaultAsync();
			return _mapper.Map<PlantDto>(plant);
		}

		public async Task<PageResultDto> GetFilteredPage(FilterDto? filter, int page = 1, int pageSize = int.MaxValue)
		{
			IQueryable<Plant> filteredPlants = _db.Plants.Include(pl => pl.Names);

			if (filter != null)
			{

				Expression<Func<Plant, bool>> filterExpression = pl =>
					(string.IsNullOrWhiteSpace(filter.Name) || pl.Names.Any(pn => EF.Functions.ILike(pn.Name, $"%{filter.Name}%"))) &&
					(filter.FlowerColorCode == -1 || pl.FlowerColorCode == filter.FlowerColorCode) &&
					(filter.Poisonous == null || pl.Poisonous == filter.Poisonous) &&
					(filter.ForHerbalTea == null || pl.ForHerbalTea == filter.ForHerbalTea) &&
					(filter.PickingProhibited == null || pl.PickingProhibited == filter.PickingProhibited);

				filteredPlants = filteredPlants.Where(filterExpression);
			}

			int totalCount = await filteredPlants.CountAsync();
			int itemsToSkip = (page - 1) * pageSize;
			List<Plant> plantList = await filteredPlants
				.OrderBy(pl => pl.PlantId)
				.Skip(itemsToSkip)
				.Take(pageSize)
				.Include(pl => pl.ImageLinks)
				.ToListAsync();
			return new PageResultDto
			{
				Plants = _mapper.Map<List<PlantDto>>(plantList),
				TotalCount = totalCount
			};
		}

		public async Task<PageResultDto> GetPage(int page = 1, int pageSize = int.MaxValue)
		{
			int totalCount = await _db.Plants.CountAsync();
			int itemsToSkip = (page - 1) * pageSize;
			List<Plant> plantList = await _db.Plants
				.OrderBy(pl => pl.PlantId)
				.Skip(itemsToSkip)
				.Take(pageSize)
				.Include(pl => pl.Names)
				.Include(pl => pl.ImageLinks)
				.ToListAsync();
			return new PageResultDto
			{
				Plants = _mapper.Map<List<PlantDto>>(plantList),
				TotalCount = totalCount
			};
		}

		public async Task<IEnumerable<ColorDto>> GetPalette()
		{
			List<Color> palette = await _db.Palette.ToListAsync();
			return _mapper.Map<List<ColorDto>>(palette);
		}

		private async Task ValidatePlantNames(IEnumerable<PlantName>? plantNames)
		{
			//Verify that names exist and are not empty.
			if (plantNames == null || !plantNames.Any() || plantNames.Any(pn => string.IsNullOrWhiteSpace(pn?.Name)))
			{
				throw new Exception("There should be at least one name for the plant and no names should be empty!");
			}

			//Check duplicates among plantNames
			var hs = new HashSet<PlantName>(plantNames, new PlantName.CaseInsensitiveNameComparer());
			if (hs.Count < plantNames.Count())
			{
				throw new Exception("Duplicate names are not allowed!");
			}

			//Check duplicates between given plantNames and database plantNames
			var sharedPlantNames = await _db.PlantNames
				.Where(pn => plantNames.Any(pn2 => pn.Name == pn2.Name && pn.PlantId != pn2.PlantId)) //same names for different plants
				.ToListAsync();
			if (sharedPlantNames.Any())
			{
				var conflictingNames = sharedPlantNames.Select(pn => pn.Name);
				throw new Exception($"Names {string.Join(", ", conflictingNames)} are occupied by other plants!");
			}
		}
	}
}
