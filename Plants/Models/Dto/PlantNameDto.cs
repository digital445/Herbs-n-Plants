using static Plants.Enumerations;

namespace Plants.Models.Dto
{
	public class PlantNameDto
	{
		public int PlantNameId { get; set; }
		public int PlantId { get; set; }
		public string? Name { get; set; }
		public Language Language { get; set; } = Language.Unknown;
	}
}
