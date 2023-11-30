namespace Services.PlantsAPI.Models.Dto
{
	public class PageResultDto
	{
		public PageResultDto()
		{
			Plants = new List<PlantDto>();
			TotalCount = 0;
		}

		public IEnumerable<PlantDto> Plants { get; set; }
        public int TotalCount { get; set; }
    }
}
