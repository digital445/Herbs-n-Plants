namespace Services.PlantsAPI.Models.Dto
{
	public class PlantDto
	{
		public int PlantId { get; set; } = -1;
		public List<PlantNameDto>? Names { get; set; } = new();
		/// <summary>
		/// the code of flower's color
		/// </summary>
		public int FlowerColorCode { get; set; }
		public bool? Poisonous { get; set; }
		public bool? ForHerbalTea { get; set; }
		/// <summary>
		/// picking flowers is prohibited by law
		/// </summary>
		public bool? PickingProhibited { get; set; }

		public List<ImageLinkDto>? ImageLinks { get; set; } //Links to Images representing this type of Plant
	}
}
