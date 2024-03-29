﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using static Services.PlantsAPI.Enumerations;

namespace Services.PlantsAPI.Models
{
	public class ImageLink
	{
		[Key]
		public int ImageId { get; set; }
		public int? PlantId { get; set; }
		[ForeignKey(nameof(PlantId))]
		public virtual Plant? Plant { get; set; }

		[Required]
		public string? ImageUrl { get; set; }
		public string? ImageServiceId { get; set; }
		[Required]
		[Column(TypeName = "integer")]
		public ViewType ViewType { get; set; }

		public void UnBindPlant()
		{
			Plant = null;
			PlantId = null;
		}

		public class IdComparer : IEqualityComparer<ImageLink>
		{
			public bool Equals(ImageLink? x, ImageLink? y)
			{
				return x != null && y != null && x.ImageId == y.ImageId;
			}

			public int GetHashCode([DisallowNull] ImageLink obj)
			{
				return obj.ImageId;
			}
		}

	}
}
