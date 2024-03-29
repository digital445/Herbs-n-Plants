﻿using Services.PlantsAPI.Models.Dto;

namespace Services.PlantsAPI.Repository
{
	public interface IPlantsRepository
	{
		public Task<PageResultDto> GetPage(int page = 1, int pageSize = int.MaxValue); //gets a page of plantDtos with a given amount of items on page,
																									   //default is one page with maximum items
		public Task<PageResultDto> GetFilteredPage(FilterDto? filter, int page = 1, int pageSize = int.MaxValue); //receives a plants page matching the filter criteria
		public Task<PlantDto> GetPlantById(int plantId);
		public Task<PlantDto> CreateUpdatePlant(PlantDto plantDto);
		public Task<bool> DeletePlant(int plantId);
		public Task<IEnumerable<ColorDto>> GetPalette();
		public Task<IEnumerable<ImageLinkDto>> GetOrphanedImageLinks();
		public Task DeleteOrphanedImageLinks(IEnumerable<int> ids);
	}
}
