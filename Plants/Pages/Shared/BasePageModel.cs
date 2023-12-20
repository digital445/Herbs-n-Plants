using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Plants.Pages.Shared
{
	public abstract class BasePageModel : PageModel
	{
		protected void SetResultMessages(bool wasSuccess, string firstMessage, params string?[]? messages)
		{			
			TempData["ResultMessages"] = (messages == null) ? firstMessage : string.Join(Environment.NewLine, firstMessage, messages);
			TempData["Success"] = wasSuccess;
		}
	}
}
