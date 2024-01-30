using static Plants.Enumerations;

namespace Plants.Models.Dto
{
    public class ApiRequestDto
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; } = "";
        public object? Data { get; set; }
        public string AccessToken { get; set; } = "";
    }
}
