using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Plants.Models.Dto;
using Plants.Pages.Shared;
using Plants.Services.IServices;

namespace Plants.Pages
{
	/// <summary>
	/// The PageModel for the Delete Page. Handles plant deletions.
	/// </summary>
    public class DeleteModel : BasePageModel
    {
        private readonly IPlantsService _plantsService;

		private const string psToken = "";

		public DeleteModel(IPlantsService plantsService)
		{
			_plantsService = plantsService;
		}

		public async Task<IActionResult> OnGet(int plantId = 0)
        {
			var response = await _plantsService.DeleteAsync<ResponseDto>(plantId, psToken);
			HandleDeleteResponse(response);

			return RedirectToPage("/ResultPage");
		}

		/// <summary>
		/// Checks the response result, sets result messages
		/// </summary>
		private void HandleDeleteResponse(ResponseDto? response)
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
					SetResultMessages(true, "Plant is successfully deleted.");
				}
				else
				{
					SetResultMessages(false, "Error deleting Plant from db");
				}
			}
			else
			{
				SetResultMessages(false, "An exception occured while requesting plantService.", response.ErrorMessages);
			}
		}
	}
}
