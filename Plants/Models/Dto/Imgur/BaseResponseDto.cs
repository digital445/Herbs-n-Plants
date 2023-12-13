namespace Plants.Models.Dto.Imgur
{
    public abstract class BaseResponseDto : ResponseDto
    {
        public int status { get; set; }
        public bool success { get; set; }
        public object? data { get; set; }
    }
}
