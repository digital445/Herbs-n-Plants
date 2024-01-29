namespace Plants.Models.Dto.Imgur
{
    public class ImgurResponseDto : ResponseDto
    {
        public int status { get; set; }

		//map Imgur properties to the base class properties
		public bool success { get => base.IsSuccess; set => base.IsSuccess = value; }
		public object? data { get => base.Result; set => base.Result = value; }
	}
}
