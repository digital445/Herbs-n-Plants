using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Services.Identity.Models;

namespace Services.Identity.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>

	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<ApplicationUser> ApplicationUsers { get; set; }
	}
}
