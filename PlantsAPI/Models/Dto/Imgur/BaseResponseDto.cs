namespace Services.PlantsAPI.Models.Dto.Imgur
{ 
	public abstract class BaseResponseDto : ResponseDto
    {
        public int status { get; set; }
        public bool success { get; set; }
    }
}
