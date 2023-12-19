using Newtonsoft.Json;
using Plants.Models.Dto;
using Plants.Models.Dto.Imgur;
using Plants.Services.IServices;
using System.Collections.Concurrent;

namespace Plants.Services
{
	public class ImageLinkCleanupService : BackgroundService
	{
		private const string Token = "ef8ced08edc102e17d8fcb6abcab2b7342ea6b39";
		private const string PsToken = "";

		private readonly IServiceScopeFactory _serviceScopeFactory;

		public ImageLinkCleanupService(IServiceScopeFactory serviceScopeFactory)
		{
			_serviceScopeFactory = serviceScopeFactory;
		}
		protected override async Task ExecuteAsync(CancellationToken stopToken)
		{
			while (!stopToken.IsCancellationRequested)
			{
				try
				{
					// Execute cleanup task
					await CleanupImageLinksAsync(stopToken);
					// Sleep for a specified interval (e.g., 24 hours)
					await Task.Delay(TimeSpan.FromHours(24), stopToken);
				}
				catch (Exception ex)
				{
					//!!! log exception
				}
			}
		}

		private async Task CleanupImageLinksAsync(CancellationToken stopToken)
		{
			using var scope = _serviceScopeFactory.CreateScope();
			var imageService = scope.ServiceProvider.GetRequiredService<IImageStorageService>();
			var plantsService = scope.ServiceProvider.GetRequiredService<IPlantsService>();

			var getResponse = await plantsService.GetOrphanedImageLinks<ResponseDto>(PsToken);
			if (getResponse == null || !getResponse.IsSuccess)
				return;

			IEnumerable<ImageLinkDto>? linksToDelete = JsonConvert.DeserializeObject<IEnumerable<ImageLinkDto>>(Convert.ToString(getResponse.Result)!);
			if (linksToDelete == null || !linksToDelete.Any())
				return;
			
			var linksDeleted = new ConcurrentBag<int>();

			await Parallel.ForEachAsync(linksToDelete, stopToken, //OperationCanceledException is thrown when token is canceled
				async (link, stopToken) => await DeleteFromExternalStorage(link, imageService, linksDeleted, stopToken));
			if (!linksDeleted.Any())
				return;

			var deleteResponse = await plantsService.DeleteOrphanedImageLinks<ResponseDto>(linksDeleted, PsToken);
		}

		private async ValueTask DeleteFromExternalStorage(ImageLinkDto link, IImageStorageService imageService, ConcurrentBag<int> linksDeleted, CancellationToken stopToken)
		{
			if (string.IsNullOrEmpty(link.ImageServiceId))
			{
				linksDeleted.Add(link.ImageId); //treat links with empty ImageServiceId as deleted
			}
			else
			{
				stopToken.ThrowIfCancellationRequested();
				var deleteImgResponse = await imageService.DeleteImageAsync<DeleteResponseDto>(link.ImageServiceId, Token);
				if (deleteImgResponse != null && deleteImgResponse.IsSuccess && deleteImgResponse.success)
					linksDeleted.Add(link.ImageId);
			}
		}
	}
}
