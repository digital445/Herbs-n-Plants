using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Numerics;

namespace Services.PlantsAPI.Models
{
	public class Plant
	{
        [Key]
        public int PlantId { get; set; }
        public List<PlantName>? Names { get; set; } = new();

        public int FlowerColorCode { get; set; } = -1; //the code of flower's color
        public bool? Poisonous { get; set; }
        public bool? ForHerbalTea { get; set; }
        public bool? PickingProhibited { get; set; } //if picking flowers is prohibited by law

        public List<ImageLink>? ImageLinks { get; set; } //Links to Images representing this type of Plant
    }
}
