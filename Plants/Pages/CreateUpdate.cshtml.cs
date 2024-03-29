using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Plants.Services.IServices;
using Plants.Models.Dto;
using Plants.Models.Dto.Imgur;
using static Plants.Enumerations;
using System.Collections.Concurrent;

namespace Plants.Pages
{
    public class CreateUpdateModel : BasePageModel
	{
        [BindProperty]
		public PlantDto Plant { get; set; }
		public string Referer = "/"; //url the request was got from

		public CreateUpdateModel(
			IImageStorageService imageService, 
			IPlantsService plantsService) : base(imageService, plantsService)
		{
			Plant = new PlantDto();
			Plant.Names.Add(new PlantNameDto());
		}
		public async Task<IActionResult> OnGet(int plantId = 0)
		{
			Referer = Request.Headers.Referer.FirstOrDefault() ?? "/";

			await RefreshPalette();

			if (plantId > 0) //Plant update is coming
			{
				var plantResponse = await _plantsService.GetAsync<ResponseDto>(plantId);
				if (plantResponse?.IsSuccess == true)
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

				var psResponse = await _plantsService.CreateUpdateAsync<ResponseDto>(Plant);
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
				var imageResponse = await _imageService.UploadImageAsync<ImgurResponseDto>(file);
				if (imageResponse != null && imageResponse.success && imageResponse.data != null)
				{
					ImageDataDto? imageData = JsonConvert.DeserializeObject<ImageDataDto>(Convert.ToString(imageResponse.data)!);
					if (imageData == null)
						return;
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
				string stringResult = Plant.PlantId > 0 ? "updated" : "created";
				SetResultMessages(true, $"Plant is successfully {stringResult}.");
			}
			else
			{
				SetResultMessages(false, "An exception has happened while requesting plantService.", response.ErrorMessages);
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
