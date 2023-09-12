using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

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
		public string Name { get; set; } = "";

		public class CaseInsensitiveNameComparer : IEqualityComparer<PlantName?>
		{
			public bool Equals(PlantName? pn1, PlantName? pn2)
			{

				return pn1 != null && pn2 != null && string.Equals(pn1.Name, pn2.Name, StringComparison.OrdinalIgnoreCase);
			}

			public int GetHashCode([DisallowNull] PlantName? pn)
			{
				return pn.Name.ToLowerInvariant().GetHashCode();
			}
		}
	}
}
