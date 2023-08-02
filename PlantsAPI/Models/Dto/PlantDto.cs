using System.Drawing;

namespace Services.PlantsAPI.Models.Dto
{
	public class PlantDto
	{
        public int PlantId { get; set; }
        public string? Name { get; set; }
        public Color FlowerColor { get; set; } = Color.Green; //main color of a flower
        public bool Poisonous { get; set; }
        public bool ForHerbalTea { get; set; }
        public bool PickingProhibited { get; set; } //if picking flowers is prohibited by law

		public List<ImageLinkDto>? ImageLinks { get; set; } //Links to Images representing this type of Plant
	}
}
