using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Services.PlantsAPI.StaticDetails;

namespace Services.PlantsAPI.Models
{
	public class ImageLink
	{
		[Key]
		public int ImageId { get; set; }
		public int PlantId { get; set; }
		[ForeignKey("PlantId")]
		public virtual Plant? Plant { get; set; }

		[Required]
		public string? ImageUrl { get; set; }
		[Required]
		[Column(TypeName = "integer")]
		public ViewType ViewType { get; set; }
	}
}
