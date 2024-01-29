using AutoMapper;
using Services.PlantsAPI.Models;
using Services.PlantsAPI.Models.Dto;

namespace Services.PlantsAPI
{
	public class MappingConfig
	{
		public static MapperConfiguration RegisterMaps()
		{
			var mappingConfig = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<PlantDto, Plant>();
				cfg.CreateMap<Plant, PlantDto>();
				cfg.CreateMap<PlantNameDto, PlantName>();
				cfg.CreateMap<PlantName, PlantNameDto>();
				cfg.CreateMap<ImageLinkDto, ImageLink>();
				cfg.CreateMap<ImageLink, ImageLinkDto>();
				cfg.CreateMap<ColorDto, Color>();
				cfg.CreateMap<Color, ColorDto>();
			});
			return mappingConfig;
		}

	}
}
