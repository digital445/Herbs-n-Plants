namespace Plants.Models.Dto
{
	public class ResponseDto
	{
		public bool IsSuccess { get; set; } = true; //was the response successful?
        public object? Result { get; set; } //we don't know a result type, so we use object
		public int TotalCount { get; set; } = 0;
		public string DisplayMessage { get; set; } = ""; //if we want to return something special
		public List<string>? ErrorMessages { get; set; }
    }
}
