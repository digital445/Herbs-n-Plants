namespace Services.PlantsAPI.Models.Dto
{
	public class PlantNameDto
	{
        public int PlantNameId { get; set; }
		public int PlantId { get; set; }
		//public virtual PlantDto? Plant { get; set; }
		public string? Name { get; set; }	
	}
}
