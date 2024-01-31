using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Identity;
using Services.Identity.Data;
using Services.Identity.DbInitializer;
using Services.Identity.Models;

var builder = WebApplication.CreateBuilder(args);

#if DEBUG
string postgreSQLConnectionString = Environment.GetEnvironmentVariable("PostgreSQLConnectionString") ?? "";
#else
string postgreSQLConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
#endif

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
	optionsBuilder.UseNpgsql(postgreSQLConnectionString));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddRazorPages();
builder.Services.AddIdentityServer(options =>
{
	options.Events.RaiseErrorEvents = true;
	options.Events.RaiseInformationEvents = true;
	options.Events.RaiseSuccessEvents = true;
	options.Events.RaiseFailureEvents = true;
	options.EmitStaticAudienceClaim = true; //useful in development process
}).AddInMemoryIdentityResources(StaticDetails.IdentityResources)
	.AddInMemoryApiScopes(StaticDetails.ApiScopes)
	.AddInMemoryClients(StaticDetails.Clients)
	.AddAspNetIdentity<ApplicationUser>()
	.AddDeveloperSigningCredential(); //should be changed in production

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
SeedDatabase();
app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();
app.MapRazorPages().RequireAuthorization();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabase()
{
	using (var scope = app.Services.CreateScope())
	{
		var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
		dbInitializer.Initialize();
	}
}