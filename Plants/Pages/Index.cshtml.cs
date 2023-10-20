using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Plants.Models.Dto;
using Plants.Services.IServices;

namespace Plants.Pages
{
	public class IndexModel : PageModel
	{
		private readonly int pageSize = 3;
		public int TotalPages => (int)Math.Ceiling(TotalCount / (double)pageSize);
		public int TotalCount { get; set; }
		public IEnumerable<PlantDto>? RequestedPlants { get; set; }
		[BindProperty(SupportsGet = true)]
		public int PageId { get; set; } = 1;
		public bool HasPreviousPage => PageId > 1;
		public bool HasNextPage => PageId < TotalPages;
		public bool FilterIsApplied => 
			Filter != null && (
				!string.IsNullOrWhiteSpace(Filter.Name) || 
				Filter.FlowerColorCode != -1 || Filter.Poisonous != null || Filter.ForHerbalTea != null || Filter.PickingProhibited != null
			);
		[BindProperty]
		public FilterDto Filter { get; set; }

		public List<int> ColorSet { get; set; } = new()
		{
			0xFFFFFF, //white
			0xFF0000, //red
			0xFFA500, //orange
			0xFFFF00, //yellow
			0x00BFFF, //skyblue
			0x0000FF, //blue
			0x8F00FF, //violet 0x800080
			0xE35B89, //pink
			0x890835, //burgundy
			0x000000  //black
			//0xFFFFFF, //white
			//0xFF0000, //red
			//0xFFA500, //orange
			//0xFFFF00, //yellow
			//0x00BFFF, //skyblue
			//0x0000FF, //blue
			//0x8F00FF, //violet 0x800080
			//0xE35B89, //pink
			//0x890835, //burgundy
			//0x000000  //black
		};

		public List<int> ColorWheelColors = new()
		{
			0x000000,
			0x8D6391, //aconitum
			0xFF0000,
			0xFF8000,
			0xFFFF00,
			0x80FF00,
			0x00FF00,
			0x00FF80,
			0x00FFFF,
			0x0080FF,
			0x0000FF,
			0x8000FF,
			0xFF00FF,
			0xFF0080,
			0xFFFFFF
		};





		private readonly IPlantsService _plantsService;
		private readonly ILogger<IndexModel> _logger;


		public IndexModel(ILogger<IndexModel> logger, IPlantsService plantsService)
		{
			_logger = logger;
			_plantsService = plantsService;
			
			Filter= new FilterDto();
		}

		public async Task OnGet()
		{
			var response = await _plantsService.GetPageAsync<ResponseDto>(PageId, pageSize, "");
			if (response != null && response.IsSuccess)
			{
				RequestedPlants = JsonConvert.DeserializeObject<IEnumerable<PlantDto>>(Convert.ToString(response.Result)!);
				TotalCount = response.TotalCount;
			}
		}

		public async Task OnPost()
		{
			// Filter передаётся в Request.Form
			var response = await _plantsService.GetFilteredAsync<ResponseDto>(Filter, PageId, pageSize, "");
			if (response != null && response.IsSuccess)
			{
				RequestedPlants = JsonConvert.DeserializeObject<IEnumerable<PlantDto>>(Convert.ToString(response.Result)!);
				TotalCount = response.TotalCount;
			}

		}

		internal string NullBoolToString(bool? b)
		{
			return b switch
			{
				true => "Yes",
				false => "No",
				_ => "Unknown"
			};
		}
	}
}