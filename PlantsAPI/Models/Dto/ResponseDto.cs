namespace Services.PlantsAPI.Models.Dto
{
	public class ResponseDto
	{
		public bool IsSuccess { get; set; } = true;
		public object Result { get; set; } = new object();
		public List<string>? ErrorMessages { get; set; }
	}
}
