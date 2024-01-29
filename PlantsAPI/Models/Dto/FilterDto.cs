namespace Services.PlantsAPI.Models.Dto
{
	public class FilterDto
	{
		public FilterDto()
		{
			Name = "";
			FlowerColorCode = -1;
			Poisonous = null;
			ForHerbalTea = null;
			PickingProhibited = null;
		}

		public string Name { get; set; }
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
	}
}
