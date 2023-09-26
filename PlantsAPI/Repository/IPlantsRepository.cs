using Services.PlantsAPI.Models.Dto;

namespace Services.PlantsAPI.Repository
{
	public interface IPlantsRepository
	{
		public Task<IEnumerable<PlantDto>> GetFiltered(PlantDto? filter); //receives Plants matching the filter criteria
		public Task<IEnumerable<PlantDto>> GetPage(int page, int pageSize); //gets a page of plantDtos with a given amount of items on page
		public Task<IEnumerable<PlantDto>> GetAll(); //receives all Plants in db
		public Task<PlantDto> GetPlantById(int plantId);
		public Task<PlantDto> CreateUpdatePlant(PlantDto plantDto);
		public Task<bool> DeletePlant(int plantId);
	}
}
