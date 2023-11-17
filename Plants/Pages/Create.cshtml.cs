using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Plants.Services.IServices;
using Plants.Models.Dto;

namespace Plants.Pages
{
	public class CreateModel : PageModel
	{
		public CreateModel(IImageService imageService, IPlantsService plantsService)
		{
			_imageService = imageService;
			_plantsService = plantsService;
		}
		private readonly IImageService _imageService;
		private readonly IPlantsService _plantsService;

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

		public void OnGet()
		{
		}
		public async Task<IActionResult> OnPost(List<IFormFile> files, PlantDto plant)
		{
			string token = "ef8ced08edc102e17d8fcb6abcab2b7342ea6b39";

			try
			{
				//Uploading images
				if (files != null && files.Any())
				{
					for (int i = 0; i < files.Count; i++)
					{
						var file = files[i];
						if (file != null)
						{
							var imageResponse = await _imageService.UploadImageAsync<ImgurResponseDto>(file, token);
							if (imageResponse != null && imageResponse.IsSuccess && imageResponse.success) //the imgur service success and the general API success are both kept in mind
							{
								var imageData = imageResponse.data;
								if (imageData != null)
								{
									plant.ImageLinks[i].ImageUrl = imageData.link;
									plant.ImageLinks[i].ImageServiceId = imageData.id;
								}
							}
						}
					}

					if (!plant.ImageLinks.Any(il => !string.IsNullOrWhiteSpace(il.ImageUrl)))
					{
						TempData["ResultMessages"] = "No valid imageUrls. Images upload is failed.";
						TempData["Success"] = false;
						return RedirectToPage("/ResultPage");
					}

					var psResponse = await _plantsService.CreateAsync<ResponseDto>(plant, "");

					if (psResponse == null)
					{
						TempData["ResultMessages"] = "No response from plantService";
						TempData["Success"] = false;
						return RedirectToPage("/ResultPage");
					}
					if (psResponse.IsSuccess)
					{
						TempData["ResultMessages"] = "Plant is successfully created.";
						TempData["Success"] = true;
					}
					else
					{
						psResponse.ErrorMessages?.Insert(0, "An exception has happened while requesting plantService.");
						TempData["ResultMessages"] = psResponse.ErrorMessages;
						TempData["Success"] = false;
					}
				}
				else
				{
					TempData["ResultMessages"] = "No images to upload.";
					TempData["Success"] = false;
				}
			}
			catch (Exception ex)
			{
				TempData["ResultMessages"] = new string[] { "An exception occured while creating the plant.", ex.Message, ex.InnerException?.Message ?? "" };
				TempData["Success"] = false;
			}

			return RedirectToPage("/ResultPage");
		}
	}
}
