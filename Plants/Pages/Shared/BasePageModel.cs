using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Plants.Pages.Shared
{
	public abstract class BasePageModel : PageModel
	{
		protected void SetResultMessages(bool wasSuccess, params string[] messages)
		{
			TempData["ResultMessages"] = messages;
			TempData["Success"] = wasSuccess;
		}
	}
}
