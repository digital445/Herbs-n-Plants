using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Plants.Models.Dto;
using Plants.Pages.Shared;
using Plants.Services.IServices;

namespace Plants.Pages
{
	public class IndexModel : BasePageModel
	{
		private readonly IPlantsService _plantsService;
		private readonly ILogger<IndexModel> _logger;
		private List<ColorDto>? _palette;

		private const string psToken = "";
		private readonly int pageSize = 3;

		public int TotalCount { get; set; }
		public int TotalPages => (int)Math.Ceiling(TotalCount / (double)pageSize);
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
		public List<ColorDto>? Palette { get => _palette; }


		public IndexModel(ILogger<IndexModel> logger, IPlantsService plantsService)
		{
			_logger = logger;
			_plantsService = plantsService;
		}

		public async Task OnGet(bool reset = false)
		{
			await RefreshPalette();

			if (reset)
				ResetFilter();

			ResponseDto? response = Filter.IsApplied
				? await _plantsService.GetFilteredAsync<ResponseDto>(Filter, PageId, pageSize, psToken)
				: await _plantsService.GetPageAsync<ResponseDto>(PageId, pageSize, psToken);

			if (response?.IsSuccess == true)
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
			await RefreshPalette();
			SaveFilter();

			var response = await _plantsService.GetFilteredAsync<ResponseDto>(Filter, PageId, pageSize, psToken);
			if (response?.IsSuccess == true)
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
			var deleteResponse = await _plantsService.DeleteAsync<ResponseDto>(plantId, psToken);
			HandleDeleteResponse(deleteResponse, plantId);

			return RedirectToPage("/ResultPage");
		}

	#region Private

		/// <summary>
		///Sets the palette value by retrieving it from either the page session or the database.
		/// </summary>
		private async Task RefreshPalette()
		{
			if (_palette == null && HttpContext.Session.IsAvailable)
			{
				string? json = HttpContext.Session.GetString("palette");
				if (string.IsNullOrEmpty(json)) //if Session does not contain the palette
				{
					var paletteResponse = await _plantsService.GetPaletteAsync<ResponseDto>(psToken);
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
		}
		private void ResetFilter() => HttpContext.Session.Remove("filter"); //removes the filter from the page session
		private void SaveFilter() =>
			HttpContext.Session.SetString("filter", JsonConvert.SerializeObject(Filter)); //saves the filter to the page session state
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
	#endregion
	}
}