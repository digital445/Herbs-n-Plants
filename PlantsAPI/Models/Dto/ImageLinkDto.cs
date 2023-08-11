﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Services.PlantsAPI.StaticDetails;

namespace Services.PlantsAPI.Models.Dto
{
	public class ImageLinkDto
	{
        public int ImageId { get; set; }
		public int PlantId { get; set; }
		public virtual PlantDto? Plant { get; set; }

		public string? ImageUrl { get; set; }
		public ViewType ViewType { get; set; }
	}
}