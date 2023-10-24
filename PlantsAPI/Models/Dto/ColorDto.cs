namespace Services.PlantsAPI.Models.Dto
{
	public class ColorDto
	{
		public ColorDto()
		{
			ColorCode = -1;
			Name = string.Empty;
		}

		public int ColorCode { get; set; }
		public string Name { get; set; }
    }
}
