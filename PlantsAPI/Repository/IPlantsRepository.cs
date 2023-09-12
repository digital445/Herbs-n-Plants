using Services.PlantsAPI.Models.Dto;

namespace Services.PlantsAPI.Repository
{
	public interface IPlantsRepository
	{
		public Task<IEnumerable<PlantDto>> GetFiltered(PlantDto? filter); //receives all Plants corresponding the filter criteria
		public Task<IEnumerable<PlantDto>> GetAll(); //receives all Plants in db
		public Task<PlantDto> GetPlantById(int plantId);
		public Task<PlantDto> CreateUpdatePlant(PlantDto plantDto);
		public Task<bool> DeletePlant(int plantId);
	}
}
