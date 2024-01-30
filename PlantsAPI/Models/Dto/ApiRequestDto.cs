using static Services.PlantsAPI.Enumerations;

namespace Services.PlantsAPI.Models.Dto
{
    public class ApiRequestDto
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; } = ""; //адрес, на который отсылается ApiRequest
        public object? Data { get; set; }
        public string AccessToken { get; set; } = "";
    }
}
