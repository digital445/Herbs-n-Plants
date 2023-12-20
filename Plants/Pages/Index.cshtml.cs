using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Plants.Models.Dto;
using Plants.Models.Dto.Imgur;
using Plants.Pages.Shared;
using Plants.Services.IServices;

namespace Plants.Pages
{
	public class IndexModel : BasePageModel
	{
		private readonly int pageSize = 3;
		public int TotalPages => (int)Math.Ceiling(TotalCount / (double)pageSize);
		public int TotalCount { get; set; }
		public IEnumerable<PlantDto>? RequestedPlants { get; set; }
		public bool NotAuthorized { get; set; } = false;
		[BindProperty(SupportsGet = true)]
		public int PageId { get; set; } = 1;
		public bool HasPreviousPage => PageId > 1;
		public bool HasNextPage => PageId < TotalPages;
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
		private readonly IImageStorageService _imageService;
		private readonly ILogger<IndexModel> _logger;


		public IndexModel(ILogger<IndexModel> logger, IPlantsService plantsService, IImageStorageService imageService)
		{
			_logger = logger;
			_plantsService = plantsService;
			_imageService = imageService;
		}


		public async Task OnGet(bool reset = false)
		{
			ResponseDto? response;
			if (reset)
			{
				HttpContext.Session.Remove("filter"); //removes the filter from the page session
			}
			if (Filter.IsApplied)
			{
				response = await _plantsService.GetFilteredAsync<ResponseDto>(Filter, PageId, pageSize, "");
			}
			else
			{
				response = await _plantsService.GetPageAsync<ResponseDto>(PageId, pageSize, "");
			}

			if (response != null && response.IsSuccess)
			{
				var result = JsonConvert.DeserializeObject<PageResultDto>(Convert.ToString(response.Result)!);
				if (result != null)
				{
					RequestedPlants = result.Plants;
					TotalCount = result.TotalCount;
				}
			}
		}

		public async Task OnPost()
		{
			HttpContext.Session.SetString("filter", JsonConvert.SerializeObject(Filter)); //saves the filter to the page session state

			var response = await _plantsService.GetFilteredAsync<ResponseDto>(Filter, PageId, pageSize, "");
			if (response != null && response.IsSuccess)
			{
				var result = JsonConvert.DeserializeObject<PageResultDto>(Convert.ToString(response.Result)!);
				if (result != null)
				{
					RequestedPlants = result.Plants;
					TotalCount = result.TotalCount;
				}
			}
		}
		public async Task<IActionResult> OnPostDelete(int plantId)
		{
			string token = "ef8ced08edc102e17d8fcb6abcab2b7342ea6b39";

			var deleteResponse = await _plantsService.DeleteAsync<ResponseDto>(plantId, token);
			HandleDeleteResponse(deleteResponse, plantId);

			return RedirectToPage("/ResultPage");
		}

		private void HandleDeleteResponse(ResponseDto? response, int plantId)
		{
			if (response == null)
			{
				SetResultMessages(false, "No response from `plantService`.");
			}
			else if (response.IsSuccess)
			{
				bool responseResult = (bool)(response.Result ?? false);
				if (responseResult)
				{
					SetResultMessages(true, $"Plant {plantId} is successfully deleted.");
				}
				else
				{
					SetResultMessages(false, $"Error deleting Plant {plantId} from db");
				}
			}
			else
			{
				SetResultMessages(false, "An exception occured while requesting plantService.", response.ErrorMessages);
			}
		}
	}
}