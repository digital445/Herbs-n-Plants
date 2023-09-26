using Microsoft.AspNetCore.Identity;
using Services.Identity.Data;
using Services.Identity.Models;
using System.Security.Claims;
using IdentityModel;

namespace Services.Identity.DbInitializer
{
	public class DbInitializer : IDbInitializer
	{
		private readonly ApplicationDbContext _db;
		private readonly UserManager<ApplicationUser> _usermanager;
		private readonly RoleManager<IdentityRole> _rolemanager;

		public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> usermanager, RoleManager<IdentityRole> rolemanager)
		{
			_db = db;
			_usermanager = usermanager;
			_rolemanager = rolemanager;
		}

		public void Initialize()
		{
			if (_rolemanager.FindByNameAsync(StaticDetails.Admin).Result == null) //role doesn't exist in the database
			{
				_rolemanager.CreateAsync(new IdentityRole(StaticDetails.Admin)).GetAwaiter().GetResult();
				_rolemanager.CreateAsync(new IdentityRole(StaticDetails.Customer)).GetAwaiter().GetResult();
			}
			else return;

			ApplicationUser adminUser = new()
			{
				//свойства базового IdentityUser
				UserName = "admin1@gmail.com",
				Email = "admin1@gmail.com",
				EmailConfirmed = true,
				PhoneNumber = "111111111111",
				//свойства ApplicationUser
				Name = "Ben Admin",
			};

			_usermanager.CreateAsync(adminUser, "Admin123*").GetAwaiter().GetResult(); //создает связку логин-пароль
																					   //.GetAwaiter().GetResult() позволяет дождаться выполнения асинхронного метода
			_usermanager.AddToRoleAsync(adminUser, StaticDetails.Admin).GetAwaiter().GetResult(); //включить adminUser в роль Admin

			_usermanager.AddClaimsAsync(adminUser, new Claim[]
			{
				new Claim(JwtClaimTypes.Name, adminUser.Name),
				new Claim(JwtClaimTypes.Role, StaticDetails.Admin)
			});


			ApplicationUser customerUser = new()
			{
				//свойства базового IdentityUser
				UserName = "customer1@gmail.com",
				Email = "customer1@gmail.com",
				EmailConfirmed = true,
				PhoneNumber = "111111111111",
				//свойства ApplicationUser
				Name = "Ben Customer"
			};

			_usermanager.CreateAsync(customerUser, "Customer123*").GetAwaiter().GetResult(); //создает связку логин-пароль
																							 //.GetAwaiter().GetResult() позволяет дождаться выполнения асинхронного метода
			_usermanager.AddToRoleAsync(customerUser, StaticDetails.Customer).GetAwaiter().GetResult(); //связать customerUser с ролью Customer

			_usermanager.AddClaimsAsync(customerUser, new Claim[]
			{
				new Claim(JwtClaimTypes.Name, customerUser.Name),
				new Claim(JwtClaimTypes.Role, StaticDetails.Customer)
			});
		}
	}
}
