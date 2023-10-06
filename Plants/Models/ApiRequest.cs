using static Plants.StaticDetails;

namespace Plants.Models
{
	public class ApiRequest
	{
		public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; } = ""; //адрес, на который отсылается ApiRequest
        public object? Data { get; set; }
        public string AccessToken { get; set; } = "";
    }
}
