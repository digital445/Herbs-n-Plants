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
			});
			return mappingConfig;
		}

	}
}
