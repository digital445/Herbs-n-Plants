using System.ComponentModel.DataAnnotations;

namespace Services.PlantsAPI.Models
{
	public class Color
	{
		public Color()
		{
			ColorCode = -1;
			Name = string.Empty;
		}

		[Key]
		public int ColorCode { get; set; }
		[Required]
		public string Name { get; set; }
    }
}
