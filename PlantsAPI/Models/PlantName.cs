using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.PlantsAPI.Models
{
	public class PlantName
	{
		[Key]
        public int PlantNameId { get; set; }
		public int PlantId { get; set; }
		[ForeignKey("PlantId")]
		public virtual Plant? Plant { get; set; }

		[Required]
		public string? Name { get; set; }	
	}
}
