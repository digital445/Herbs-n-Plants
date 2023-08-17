using Services.PlantsAPI.Models.Dto;

namespace Services.PlantsAPI.Repository
{
	public interface IRepository
	{
		public Task<IEnumerable<PlantDto>> GetPlants(PlantDto filter); //receives all Plants corresponding the filter criteria
		public Task<PlantDto> GetPlantById(int plantId);
		public Task<PlantDto> CreateUpdatePlant(PlantDto plantDto);
		public Task<bool> DeletePlant(int plantId);
	}
}
