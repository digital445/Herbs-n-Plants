using static Plants.StaticDetails;

namespace Plants.Models.Dto
{
	public class ImageLinkDto
	{
		public int ImageId { get; set; }
		public int PlantId { get; set; }
		public string? ImageUrl { get; set; }
		public string? ImageServiceId { get; set; }
		public ViewType ViewType { get; set; }
	}
}
