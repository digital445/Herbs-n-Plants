using System.Net.Sockets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddAuthentication(options =>
{
	options.DefaultChallengeScheme = "oidc"; //DefaultChallengeScheme is used when an unauthenticated user must log in
	options.DefaultScheme = "Cookies"; //Subsequent requests to the client will include a cookie and be authenticated with the default Cookie scheme.
})
	.AddCookie("Cookies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(10))
	.AddOpenIdConnect("oidc", options =>
	//options should correspond those in Identity.StaticDetails class
	{
		options.Authority = builder.Configuration["ServiceUrls:IdentityServer"];
		options.GetClaimsFromUserInfoEndpoint = true;
		options.ClientId = "Plants.Web"; //got from Identity\StaticDetails
		options.ClientSecret = "secret"; //is simple for testing
		options.ResponseType = "code";

		options.TokenValidationParameters.NameClaimType = "name";
		options.TokenValidationParameters.RoleClaimType = "role";
		options.Scope.Add("ApiAccess");
		options.SaveTokens = true;
	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
