using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Plants.Models.Dto;
using Plants.Services.IServices;

namespace Plants.Pages
{
	public class IndexModel : PageModel
	{
		private int _pageIndex = 1;
		private int _pageSize = 20;

		private readonly IPlantsService _plantsService;
		private readonly ILogger<IndexModel> _logger;

		//private readonly 

		public IndexModel(ILogger<IndexModel> logger, IPlantsService plantsService)
		{
			_logger = logger;
			_plantsService = plantsService;
		}

		public async Task OnGet()
		{
			var list = new List<PlantDto>();
			PlantDto plantDto = new PlantDto()
			{
				Names = new List<PlantNameDto> { new PlantNameDto {Name = "ts" } },
				FlowerColorCode = -1,
				ForHerbalTea = null,
				PickingProhibited = null,
				Poisonous = null
			};
			var response = await _plantsService.GetFilteredAsync<ResponseDto>(plantDto, "");
			if (response != null && response.IsSuccess)
			{
				list = JsonConvert.DeserializeObject<List<PlantDto>>(Convert.ToString(response.Result)!);
			}
		}
	}
}