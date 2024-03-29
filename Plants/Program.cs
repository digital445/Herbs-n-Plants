using Plants;
using Plants.Services;
using Plants.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

string IdentityServerBaseUrl = builder.Configuration["Services:IdentityServer:BaseUrl"] ?? "";

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(20);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});


builder.Services.AddAuthentication(options =>
{
	options.DefaultChallengeScheme = "oidc"; //DefaultChallengeScheme is used when an unauthenticated user must log in
	options.DefaultScheme = "Cookies"; //Subsequent requests to the client will include a cookie and be authenticated with the default Cookie scheme.
})
	.AddCookie("Cookies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(10))
	.AddOpenIdConnect("oidc", options =>
	{
		options.Authority = IdentityServerBaseUrl;
		options.GetClaimsFromUserInfoEndpoint = true;
		options.ClientId = "Plants.Web"; //got from Identity\StaticDetails
		options.ClientSecret = "secret"; //is simple for testing
		options.ResponseType = "code";

		options.TokenValidationParameters.NameClaimType = "name";
		options.TokenValidationParameters.RoleClaimType = "role";
		options.Scope.Add("ApiAccess");
		options.SaveTokens = true;
	});

builder.Services.AddHttpClient<IPlantsService, PlantsService>();
builder.Services.AddHttpClient<IImageStorageService, ImgurImageStorageService>();
builder.Services.AddScoped<IPlantsService, PlantsService>();
builder.Services.AddScoped<IImageStorageService, ImgurImageStorageService>();

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
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
