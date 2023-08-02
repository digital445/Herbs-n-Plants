using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Services.PlantsAPI.Models
{
	public class Plant
	{
        [Key]
        public int PlantId { get; set; }
        public List<PlantName>? Names { get; set; }

        public int FlowerColorCode { get; set; } = Color.Green.ToArgb(); //the code of flower's color
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
