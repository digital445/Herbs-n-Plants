using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Plants.Services.IServices;
using Plants.Models.Dto;
using Plants.Models.Dto.Imgur;
using static Plants.StaticDetails;
using Plants.Pages.Shared;
using System.Collections.Concurrent;

namespace Plants.Pages
{
	public class CreateUpdateModel : BasePageModel
	{
		public CreateUpdateModel(IImageStorageService imageService, IPlantsService plantsService)
		{
			_imageService = imageService;
			_plantsService = plantsService;
			Plant = new PlantDto();
			Plant.Names.Add(new PlantNameDto());
		}
		private readonly IImageStorageService _imageService;
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

		private const string Token = "ef8ced08edc102e17d8fcb6abcab2b7342ea6b39";

		public async Task<IActionResult> OnGet(int plantId = 0)
		{
			if (plantId > 0) //Plant update is coming
			{
				var plantResponse = await _plantsService.GetAsync<ResponseDto>(plantId, "");
				if (plantResponse != null && plantResponse.IsSuccess)
					Plant = JsonConvert.DeserializeObject<PlantDto>(Convert.ToString(plantResponse.Result)!) ?? Plant;
				else
				{
					SetResultMessages(false, "No plant data was received from server");
					return RedirectToPage("/ResultPage");
				}
			}
			return Page();
		}
		public async Task<IActionResult> OnPost(List<IFormFile> files, List<ViewType> viewTypes)
		{
			try
			{
				ValidatePlant(files.Count);

				await UploadImageFiles(files, viewTypes);
				HandleUpload();

				var psResponse = await _plantsService.CreateUpdateAsync<ResponseDto>(Plant, "");
				HandleResponse(psResponse);

				return RedirectToPage("/ResultPage");
			}
			catch (Exception ex)
			{
				SetResultMessages(false,
					"An exception occured while creating the Plant.",
					ex.Message,
					ex.InnerException?.Message);
				return RedirectToPage("/ResultPage");
			}
		}
#region Private
		private void ValidatePlant(int newImagesNumber)
		{
			//plant names validation
			var notEmptyNames = Plant.Names.Where(pn => !string.IsNullOrWhiteSpace(pn.Name)).ToList();
			if (!notEmptyNames.Any())
			{
				throw new Exception("At least one meaningful Name should be added!");
			}
			Plant.Names = notEmptyNames;

			//number of images validation
			if (newImagesNumber + Plant.ImageLinks.Count == 0)
			{
				throw new Exception("Plant with no images cannot be created or updated!");
			}
		}
		private async Task UploadImageFiles(List<IFormFile> files, List<ViewType> viewTypes)
		{
			if (files == null || !files.Any())
				return;

			var merged = files.Zip(viewTypes); //merge files with corresponding viewTypes List<(IFormFile, ViewType)>
			var imageLinks = new ConcurrentBag<ImageLinkDto>();
			
			await Parallel.ForEachAsync(merged, async (item, _) =>
			{
				var (file, viewType) = item; 
				if (file == null)
					return;
				var imageResponse = await _imageService.UploadImageAsync<UploadResponseDto>(file, Token);
				if (imageResponse != null && imageResponse.IsSuccess && imageResponse.success && imageResponse.data != null) //the imgur service success and the general API success are both kept in mind
				{
					ImageData imageData = imageResponse.data;
					var imageLink = new ImageLinkDto
					{
						ImageUrl = imageData.link,
						ViewType = viewType,
						ImageServiceId = imageData.id
					};
					imageLinks.Add(imageLink);
				}
			});

			Plant.ImageLinks.AddRange(imageLinks);
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
				SetResultMessages(false, "An exception has happened while requesting plantService.", response.ErrorMessages?.ToArray());
			}
		}
		private bool PlantHasImages() =>
			Plant.ImageLinks.Any(il => !string.IsNullOrWhiteSpace(il.ImageUrl));
		private void HandleUpload()
		{
			if (!PlantHasImages())
				throw new Exception("Plant with no images cannot be created or updated.");
		}

#endregion
	}
}
