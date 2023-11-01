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
		private FilterDto? _filter;
		[BindProperty]
		public FilterDto Filter
		{
			get
			{
				if (_filter == null && HttpContext.Session.IsAvailable)
				{
					string? json = HttpContext.Session.GetString("filter");
					_filter = string.IsNullOrEmpty(json) ? null : JsonConvert.DeserializeObject<FilterDto>(json);
				}
				return _filter ??= new FilterDto();
			}
			set { _filter = value; }
		}

		private List<ColorDto>? _palette;

		/// <summary>
		///gets the Palette either from the Session or from the PlantService
		/// </summary>
		public List<ColorDto>? Palette
		{
			get
			{
				if (_palette == null && HttpContext.Session.IsAvailable)
				{
					string? json = HttpContext.Session.GetString("palette");
					if (string.IsNullOrEmpty(json)) //if Session does not contain the palette
					{
						var paletteResponse = _plantsService.GetPaletteAsync<ResponseDto>("").Result; //??? Is DeadLock possible?
						if (paletteResponse != null && paletteResponse.IsSuccess)
						{
							json = Convert.ToString(paletteResponse.Result);
							if (!string.IsNullOrEmpty(json))
							{
								HttpContext.Session.SetString("palette", json); //save palette to session
							}
						}

					}
					_palette = string.IsNullOrEmpty(json) ? null : JsonConvert.DeserializeObject<IEnumerable<ColorDto>>(json)?.ToList();
				}
				return _palette;
			}
		}

		private readonly IPlantsService _plantsService;
		private readonly ILogger<IndexModel> _logger;


		public IndexModel(ILogger<IndexModel> logger, IPlantsService plantsService)
		{
			_logger = logger;
			_plantsService = plantsService;
		}


		public async Task OnGet(bool reset = false)
		{
			ResponseDto? response;
			if (reset)
			{
				HttpContext.Session.Remove("filter"); //removes the filter from the page session
			}
			if (FilterIsApplied)
			{
				response = await _plantsService.GetFilteredAsync<ResponseDto>(Filter, PageId, pageSize, "");
			}
			else
			{
				response = await _plantsService.GetPageAsync<ResponseDto>(PageId, pageSize, "");
			}

			if (response != null && response.IsSuccess)
			{
				RequestedPlants = JsonConvert.DeserializeObject<IEnumerable<PlantDto>>(Convert.ToString(response.Result)!);
				TotalCount = response.TotalCount;
			}
		}

		public async Task OnPost()
		{
			HttpContext.Session.SetString("filter", JsonConvert.SerializeObject(Filter)); //saves the filter to the page session state

			var response = await _plantsService.GetFilteredAsync<ResponseDto>(Filter, PageId, pageSize, "");
			if (response != null && response.IsSuccess)
			{
				RequestedPlants = JsonConvert.DeserializeObject<IEnumerable<PlantDto>>(Convert.ToString(response.Result)!);
				TotalCount = response.TotalCount;
			}
		}
	}
}