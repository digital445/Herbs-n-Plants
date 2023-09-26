using Microsoft.AspNetCore.Identity;

namespace Services.Identity.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string Name { get; set; }
	}
}
