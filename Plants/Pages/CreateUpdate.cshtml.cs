using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Plants.Services.IServices;
using Plants.Models.Dto;
using Plants.Models.Dto.Imgur;
using static Plants.StaticDetails;
using Newtonsoft.Json.Linq;

namespace Plants.Pages
{
    public class CreateUpdateModel : PageModel
	{
		public CreateUpdateModel(IImageService imageService, IPlantsService plantsService)
		{
			_imageService = imageService;
			_plantsService = plantsService;
			Plant = new PlantDto();
			Plant.Names.Add(new PlantNameDto());
		}
		private readonly IImageService _imageService;
		private readonly IPlantsService _plantsService;


		[BindProperty]
        public PlantDto Plant { get; set; }
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

		const string Token = "ef8ced08edc102e17d8fcb6abcab2b7342ea6b39";

		public async Task OnGet(int plantId = 0)
		{
			if (plantId > 0) //Plant update is coming
			{
				var plantResponse = await _plantsService.GetAsync<ResponseDto>(plantId, "");
				if (plantResponse != null && plantResponse.IsSuccess)
				{
					Plant = JsonConvert.DeserializeObject<PlantDto>(Convert.ToString(plantResponse.Result)!) ?? Plant;
				}
				else
				{
					//!!!error processing
				}
			}
		}
		public async Task<IActionResult> OnPost(List<IFormFile> files, List<ViewType> viewTypes)
		{
			try
			{
				ValidatePlantNames();

				await UploadImageFiles(files, viewTypes);

				if (!PlantHasImages())
				{
					SetResultMessages(false, "Plant with no images cannot be created or updated.");
					return RedirectToPage("/ResultPage");
				}

				var psResponse = await _plantsService.CreateUpdateAsync<ResponseDto>(Plant, "");
				HandleResponse(psResponse);

				return RedirectToPage("/ResultPage");
			}
			catch (Exception ex)
			{
				SetResultMessages(false,
					"An exception occured while creating the Plant.",
					ex.Message, 
					ex.InnerException?.Message ?? "");
				return RedirectToPage("/ResultPage");
			}			
		}

		private void ValidatePlantNames()
		{
			var notEmptyNames = Plant.Names.Where(pn => !string.IsNullOrWhiteSpace(pn.Name)).ToList();
			if (!notEmptyNames.Any())
			{
				throw new Exception("At least one meaningful Name should be added!");
			}
			Plant.Names = notEmptyNames;
		}
		private async Task UploadImageFiles(List<IFormFile> files, List<ViewType> viewTypes)
		{
			if (files != null && files.Any())
			{
				for (int i = 0; i < files.Count; i++)
				{
					var file = files[i];
					if (file != null)
					{
						var imageResponse = await _imageService.UploadImageAsync<UploadResponseDto>(file, Token);
						if (imageResponse != null && imageResponse.IsSuccess && imageResponse.success) //the imgur service success and the general API success are both kept in mind
						{
							var imageData = imageResponse.data;
							if (imageData != null)
							{
								var imageLink = new ImageLinkDto
								{
									ImageUrl = imageData.link,
									ViewType = viewTypes[i],
									ImageServiceId = imageData.id
								};
								Plant.ImageLinks.Add(imageLink);
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// Check whether the plant has at least one image URL
		/// </summary>
		/// <returns></returns>
		private bool PlantHasImages() => 
			Plant.ImageLinks.Any(il => !string.IsNullOrWhiteSpace(il.ImageUrl));

		private void SetResultMessages(bool wasSuccess, params string[] messages)
		{
			TempData["ResultMessages"] = messages;
			TempData["Success"] = wasSuccess;
		}
		private void HandleResponse(ResponseDto? response)
		{
			if (response == null)
			{
				SetResultMessages(false, "No response from plantService");
			}
			else if (response.IsSuccess)
			{
				SetResultMessages(true, "Plant is successfully created.");
			}
			else
			{
				string firstMessage = "An exception has happened while requesting plantService.";
				if (response.ErrorMessages != null)
				{
					response.ErrorMessages.Insert(0, firstMessage);
					SetResultMessages(false, response.ErrorMessages.ToArray());
				}
				else
				{
					SetResultMessages(false, firstMessage);
				}
			}
		}
	}
}
