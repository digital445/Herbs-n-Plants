using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;

namespace Plants.Pages
{
	public class ResultPageModel : PageModel
	{
		public StringValues ResultMessages;
		public string Referer = "/"; //url the request was got from
		public bool Success;
		public void OnGet()
		{
			Referer = Request.Headers.Referer.FirstOrDefault() ?? "/";
			ResultMessages = TempData["ResultMessages"] switch
			{
				string message => new StringValues(message),
				IEnumerable<string> messages => new StringValues(messages.ToArray()),
				_ => "" //null and others
			};

			Success = (bool?)TempData["Success"] ?? false;
		}
	}
}
