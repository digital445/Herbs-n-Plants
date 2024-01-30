using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using static Services.PlantsAPI.Enumerations;

namespace Services.PlantsAPI.Models
{
	[Index(nameof(Name), IsUnique = true)]
	public class PlantName
	{
		[Key]
		public int PlantNameId { get; set; }
		public int PlantId { get; set; }
		[ForeignKey(nameof(PlantId))]
		public virtual Plant? Plant { get; set; }

		[Required]
		public string Name { get; set; } = "";
		[Required]
		[Column(TypeName = "integer")]
		public Language Language { get; set; }
	}
}
