using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Services.PlantsAPI.DbContexts;
using Services.PlantsAPI.Models;
using Services.PlantsAPI.Models.Dto;

namespace Services.PlantsAPI.Repository
{
	public class Repository : IRepository
	{
		private readonly ApplicationDbContext _db;
		private readonly IMapper _mapper;

		public Repository(ApplicationDbContext context, IMapper mapper)
		{
			_db = context;
			_mapper = mapper;
		}

		public async Task<PlantDto> CreateUpdatePlant(PlantDto plantDto)
		{
			Plant plant = _mapper.Map<Plant>(plantDto);
			if (plant.PlantId > 0) //plant already exists in database
			{
				_db.Plants.Update(plant);
			}
			else
			{
				_db.Plants.Add(plant);
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
			Plant? plant = await _db.Plants.FirstOrDefaultAsync(plant => plantId ==  plant.PlantId);
			return _mapper.Map<PlantDto>(plant); //??? what if plant is null? what does mapper do in this case?
		}

		public async Task<IEnumerable<PlantDto>> GetPlants(PlantDto? filter)
		{
			Plant? mfilter = _mapper.Map<Plant?>(filter);
			List<Plant> plantList = await _db.Plants.Where(pl => pl.MatchesFilter(mfilter)).ToListAsync();
			return _mapper.Map<List<PlantDto>>(plantList);
		}
	}
}
