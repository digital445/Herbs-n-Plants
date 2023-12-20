using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Plants.Pages.Shared
{
	public abstract class BasePageModel : PageModel
	{
		protected void SetResultMessages(bool wasSuccess, params string?[] messages)
		{
			TempData["ResultMessages"] = string.Join(Environment.NewLine, messages);
			TempData["Success"] = wasSuccess;
		}
		protected void SetResultMessages(bool wasSuccess, string firstMessage, List<string>? messages)
		{
			messages?.Insert(0, firstMessage);
			TempData["ResultMessages"] = (messages == null) ? firstMessage : string.Join(Environment.NewLine, messages);
			TempData["Success"] = wasSuccess;
		}
	}
}
