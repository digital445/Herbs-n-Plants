using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Services.PlantsAPI.DbContexts;
using Services.PlantsAPI.Models;
using Services.PlantsAPI.Models.Dto;
using System.Data;
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

			ValidatePlantNames(plant.Names);

			Plant? existingPlant = await _db.Plants
				.Include(pl => pl.Names)
				.Include(pl => pl.ImageLinks)
				.AsNoTracking()
				.FirstOrDefaultAsync(pl => pl.PlantId == plant.PlantId);

			if (existingPlant == null) //plant does not exist in the database
			{
				_db.Plants.Add(plant);
			}
			else
			{
				//delete old PlantNames for existingPlant
				existingPlant.Names.ForEach(pn => pn.Plant = null); //cut ties with the parent plant to prevent tracking all related entities
				_db.PlantNames.RemoveRange(existingPlant.Names);

				//unbind obsolete imageLinks from their plant, so that links become orphaned and will be deleted later
				var linksToUnbind = existingPlant.ImageLinks
									.Except(plant.ImageLinks, new ImageLink.IdComparer()).ToList();
				linksToUnbind.ForEach(il => il.UnBindPlant());	
				_db.ImageLink.UpdateRange(linksToUnbind);

				//update the plant in db with new values including Names and ImageLinks
				_db.Plants.Update(plant);
			}

			await _db.SaveChangesAsync();

			return _mapper.Map<Plant, PlantDto>(plant);
		}

		public async Task<bool> DeletePlant(int plantId)
		{
			try
			{
				Plant? plant = await _db.Plants
					.Include(pl => pl.Names)
					.Include(pl => pl.ImageLinks)
					.FirstOrDefaultAsync(plant => plant.PlantId == plantId);
				if (plant == null)
				{
					return false;
				}

				_db.Plants.Remove(plant); //deletes Plant and related Names, doesn't delete ImageLinks, but unbinds it from the Plant (ImageId is nullable)
				
				await _db.SaveChangesAsync();
				return true;
			}
			catch (Exception)
			{
				//TODO: log exception
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

		public async Task<IEnumerable<ImageLinkDto>> GetOrphanedImageLinks()
		{
			List<ImageLink> orphanedImageLinks = await _db.ImageLink.Where(il => il.PlantId == null).ToListAsync();
			return _mapper.Map<List<ImageLinkDto>>(orphanedImageLinks);
		}

		public async Task DeleteOrphanedImageLinks(IEnumerable<int> ids)
		{
			var linksToDelete = await _db.ImageLink.Where(il => ids.Contains(il.ImageId)).ToListAsync();
			_db.ImageLink.RemoveRange(linksToDelete);
			await _db.SaveChangesAsync();
		}

		private void ValidatePlantNames(List<PlantName>? plantNames)
		{
			//Verify that names exist and are not empty.
			if (plantNames == null || !plantNames.Any() || plantNames.Any(pn => string.IsNullOrWhiteSpace(pn?.Name)))
			{
				throw new Exception("There should be at least one name for the plant and no names should be empty!");
			}

			//Check duplicates among plantNames
			var lowerNames = plantNames.Select(pn => pn.Name.ToLowerInvariant());
			var hs = new HashSet<string>();

			foreach (var name in lowerNames)
			{
				if (!hs.Add(name))
				{
					throw new Exception("Duplicate names are not allowed!");
				}
			}
		}
	}
}
