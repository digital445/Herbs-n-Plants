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

		///Images
		public string? GeneralViewImgUrl { get; set; } //view of the whole plant
		public string? FlowerImgUrl { get; set; }
		public string? BudImgUrl { get; set; }
		public string? FruitImgUrl { get; set; }
		public string? LeafImgUrl { get; set; }
		public string? StemImgUrl { get; set; } //view of the stem/branch with leaves and flowers on it
	}
}
