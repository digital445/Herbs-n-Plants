using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Services.PlantsAPI.Models
{
	public class Plant
	{
        [Key]
        public int PlantId { get; set; }
        [Required] //denotes NOT NULLABLE in DB
		public string? Name { get; set; }
        public Color MainColor { get; set; } = Color.Green; //main color of a flower
        public bool Poisonous { get; set; }
        public bool ForHerbalTea { get; set; }
        public bool PickingProhibited { get; set; } //if picking flowers is prohibited by law
    }
}
