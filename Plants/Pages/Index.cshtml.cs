using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Plants.Models.Dto;
using Plants.Services.IServices;

namespace Plants.Pages
{
    /// <summary>
    /// The PageModel for the Index Page, which displays requested plants
    /// </summary>
    public class IndexModel : BasePageModel
	{
		private readonly ILogger<IndexModel> _logger;

		private const string filterSessionKey = "filter"; //the key to store the filter in the page session
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
					string? json = HttpContext.Session.GetString(filterSessionKey);
					_filter = string.IsNullOrEmpty(json) ? null : JsonConvert.DeserializeObject<FilterDto>(json);
				}
				return _filter ??= new FilterDto();
			}
			set { _filter = value; }
		}

		public IndexModel(
			ILogger<IndexModel> logger,
			IImageStorageService imageService,
			IPlantsService plantsService) : base(imageService, plantsService)
		{
			_logger = logger;
		}

		public async Task OnGet(bool reset = false)
		{
			await RefreshPalette();

			if (reset)
				ResetFilter();

			ResponseDto? response = Filter.IsApplied
				? await _plantsService.GetFilteredAsync<ResponseDto>(Filter, PageId, pageSize)
				: await _plantsService.GetPageAsync<ResponseDto>(PageId, pageSize);

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
		/// <summary>
		/// Processes the POST request which contains the filter conditions
		/// </summary>
		public async Task OnPost()
		{
			await RefreshPalette();

			SaveFilter();

			var response = await _plantsService.GetFilteredAsync<ResponseDto>(Filter, PageId, pageSize);
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

	#region Private
		/// <summary>
		/// Removes the filter from the page session
		/// </summary>
		private void ResetFilter() => HttpContext.Session.Remove(filterSessionKey);
		/// <summary>
		/// Saves the filter to the page session state
		/// </summary>
		private void SaveFilter() =>
			HttpContext.Session.SetString(filterSessionKey, JsonConvert.SerializeObject(Filter));
	#endregion
	}
}