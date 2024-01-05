namespace Plants.Models.Dto
{
	public class ResponseDto
	{
		public bool IsSuccess { get; set; } = true; //was the response successful?
        public object? Result { get; set; } //we don't know a result type, so we use object
		public List<string>? ErrorMessages { get; set; }
    }
}
