using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Plants.Models.Dto;
using Plants.Services.IServices;

namespace Plants.Pages
{
    /// <summary>
    /// This class extends PageModel class to provide common functionality for its inheritors
    /// </summary>
    public abstract class BasePageModel : PageModel
    {        
        private const string paletteSessionKey = "palette"; //the key to store palette in the page session

		protected readonly IImageStorageService _imageService;
		protected readonly IPlantsService _plantsService;

        private List<ColorDto> _palette = new();
        public List<ColorDto> Palette { get => _palette; }

		protected BasePageModel(
            IImageStorageService imageService, 
            IPlantsService plantsService)
		{
			_imageService = imageService;
			_plantsService = plantsService;
		}

		public static string NullBoolFilterToString(bool? b)
		{
			return b switch
			{
				true => "Yes",
				false => "No",
				_ => "Any"
			};
		}
		public static string NullBoolPlantToString(bool? b)
		{
			return b switch
			{
				true => "Yes",
				false => "No",
				_ => "Unknown"
			};
		}

		/// <summary>
		/// Gets the palette value by retrieving it from either the page session or the API.
		/// </summary>
		protected async Task RefreshPalette()
        {
            if (!_palette.Any() && HttpContext.Session.IsAvailable)
            {
                string? json = HttpContext.Session.GetString(paletteSessionKey);
                if (string.IsNullOrEmpty(json)) //if Session does not contain the palette
                {
                    var paletteResponse = await _plantsService.GetPaletteAsync<ResponseDto>();
                    if (paletteResponse?.IsSuccess == true)
                    {
                        json = Convert.ToString(paletteResponse.Result);
                        if (!string.IsNullOrEmpty(json))
                        {
                            HttpContext.Session.SetString(paletteSessionKey, json); //save palette to session
                        }
                    }
                }
                if (string.IsNullOrEmpty(json))
                    return;

                _palette = JsonConvert.DeserializeObject<List<ColorDto>>(json) ?? _palette;
            }
        }

        /// <summary>
        /// Saves the result of the page's action and relating messages to temporary storage before redirection
        /// </summary>
        /// <param name="wasSuccess"></param>
        /// <param name="messages"></param>
        protected void SetResultMessages(bool wasSuccess, params string[] messages)
        {
            TempData["ResultMessages"] = string.Join(Environment.NewLine, messages);
            TempData["Success"] = wasSuccess;
        }
        protected void SetResultMessages(bool wasSuccess, string firstMessage, IEnumerable<string> messages)
        {
            string messagesString = messages == null ? string.Empty : string.Join(Environment.NewLine, messages);

            TempData["ResultMessages"] = firstMessage + Environment.NewLine + messagesString;
            TempData["Success"] = wasSuccess;
        }

	}
}
