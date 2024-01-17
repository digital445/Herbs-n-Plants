using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Plants.Pages.Shared
{
	/// <summary>
	/// This class extends PageModel class to provide common functionality for its inheritors
	/// </summary>
	public abstract class BasePageModel : PageModel
	{
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
