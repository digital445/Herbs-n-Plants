using System.Drawing;

namespace Services.PlantsAPI.Models.Dto
{
	public class PlantDto
	{
		public int PlantId { get; set; } = -1;
		public List<PlantNameDto>? Names { get; set; }
		public int FlowerColorCode { get; set; } = -1; //the code of flower's color
		public bool? Poisonous { get; set; }
        public bool? ForHerbalTea { get; set; }
        public bool? PickingProhibited { get; set; } //if picking flowers is prohibited by law

		public List<ImageLinkDto>? ImageLinks { get; set; } //Links to Images representing this type of Plant
	}
}
