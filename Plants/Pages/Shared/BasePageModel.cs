using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Plants.Models.Dto;
using Plants.Services.IServices;

namespace Plants.Pages.Shared
{
	/// <summary>
	/// This class extends PageModel class to provide common functionality for its inheritors
	/// </summary>
	public abstract class BasePageModel : PageModel
	{
		private const string paletteSessionKey = "palette"; //the key to store palette in the page session
		private List<ColorDto> _palette = new();
        public List<ColorDto> Palette { get => _palette;}

        /// <summary>
        /// Gets the palette value by retrieving it from either the page session or the API.
        /// </summary>
        /// <param name="plantsService">Service to get the palette from if needed</param>
        /// <param name="psToken">Service token</param>
        protected async Task RefreshPalette(IPlantsService plantsService, string psToken)
		{
			if (!_palette.Any() && HttpContext.Session.IsAvailable)
			{
				string? json = HttpContext.Session.GetString(paletteSessionKey);
				if (string.IsNullOrEmpty(json)) //if Session does not contain the palette
				{
					var paletteResponse = await plantsService.GetPaletteAsync<ResponseDto>(psToken);
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

		protected void SetResultMessages(bool wasSuccess, params string?[] messages)
		{
			TempData["ResultMessages"] = string.Join(Environment.NewLine, messages);
			TempData["Success"] = wasSuccess;
		}
		protected void SetResultMessages(bool wasSuccess, string firstMessage, IEnumerable<string>? messages)
		{
			string messagesString = (messages == null) ? string.Empty : string.Join(Environment.NewLine, messages);
			
			TempData["ResultMessages"] = firstMessage + Environment.NewLine + messagesString;
			TempData["Success"] = wasSuccess;
		}
	}
}
