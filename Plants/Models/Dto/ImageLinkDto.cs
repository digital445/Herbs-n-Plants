using System.Diagnostics.CodeAnalysis;
using static Plants.StaticDetails;

namespace Plants.Models.Dto
{
	public class ImageLinkDto
	{
		public int ImageId { get; set; }
		public int? PlantId { get; set; }
		public string? ImageUrl { get; set; }
		public bool DeleteLater { get; set; } = false;
		public string? ImageServiceId { get; set; }
		public ViewType ViewType { get; set; }

		public class IdComparer : IEqualityComparer<ImageLinkDto>
		{
			public bool Equals(ImageLinkDto? x, ImageLinkDto? y)
			{
				return x != null && y != null && x.ImageId == y.ImageId;
			}

			public int GetHashCode([DisallowNull] ImageLinkDto obj)
			{
				return obj.ImageId;
			}
		}
	}
}
