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
			Plant? dbPlant = await _db.Plants.AsNoTracking().FirstOrDefaultAsync(pl => pl.PlantId == plant.PlantId);

			var plantNames = plant.Names ?? throw new Exception("There should be at least one name for the plant!");

			var hs = new HashSet<PlantName>(plantNames, new PlantName.CaseInsensitiveNameComparer());
			if (hs.Count < plantNames.Count)
			{
				throw new Exception("Duplicate names are not allowed!");
			}

			plant.Names = null;

			if (dbPlant == null) //plant does not exist in the database
			{
				var names = plantNames.Select(pl => pl.Name);
				var dbNames = _db.PlantNames.Select(pn => pn.Name);

				foreach (string name in names)
				{
					if (await dbNames.ContainsAsync(name))
					{
						throw new Exception($"Name {name} is occupied by another plant!");
					}
				}

				_db.Plants.Add(plant);
				await _db.SaveChangesAsync(); //here a new plant receives an Id
				plantNames.ForEach(pn => pn.PlantId = plant.PlantId);
			}
			else
			{
				_db.Plants.Update(plant);
				var dbPlantNames = _db.PlantNames.Where(pn => pn.PlantId == plant.PlantId).AsEnumerable();
				_db.PlantNames.RemoveRange(dbPlantNames); //removes old names for this plant from db
			}

			_db.PlantNames.AddRange(plantNames);
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
			Plant? plant = await _db.Plants.Include(pl => pl.Names).FirstOrDefaultAsync(plant => plantId ==  plant.PlantId);
			return _mapper.Map<PlantDto>(plant);
		}

		public async Task<IEnumerable<PlantDto>> GetFiltered(PlantDto? conditions)
		{
			Expression<Func<Plant, bool>> basicFilterExpression = pl =>
					conditions == null ||
					((conditions.FlowerColorCode == -1 || pl.FlowerColorCode == conditions.FlowerColorCode) &&
					(conditions.Poisonous == null || pl.Poisonous == conditions.Poisonous) &&
					(conditions.ForHerbalTea == null || pl.ForHerbalTea == conditions.ForHerbalTea) &&
					(conditions.PickingProhibited == null || pl.PickingProhibited == conditions.PickingProhibited));

			Expression<Func<Plant, bool>> nameFilterExpression = pl =>
					conditions == null || conditions.Names == null || conditions.Names.Count == 0 || string.IsNullOrWhiteSpace(conditions.Names.First().Name) ||
					pl.Names!.Any(pn => pn.Name.ToLower() == conditions.Names.First().Name!.ToLower()); //case insensitive comparison, EF does not support translation of StringComparison.OrdinalIgnoreCase

			var filteredPlants = _db.Plants.Where(basicFilterExpression)
											.Include(pl => pl.Names)
											.Where(nameFilterExpression); 
			List<Plant> plantList = await filteredPlants.ToListAsync();
			return _mapper.Map<List<PlantDto>>(plantList);
		}

		public async Task<IEnumerable<PlantDto>> GetAll()
		{
			List<Plant> plantList = await _db.Plants.Include(pl => pl.Names).ToListAsync();
			return _mapper.Map<List<PlantDto>>(plantList);
		}
	}
}
