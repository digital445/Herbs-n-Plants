using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.PlantsAPI;
using Services.PlantsAPI.DbContexts;
using Services.PlantsAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IPlantsRepository, PlantsRepository>();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAuthentication("Bearer")
	.AddJwtBearer("Bearer", options =>
	{
		options.Authority = builder.Configuration.GetValue<string>("IdentityServer:Authority"); //gets an identity server URL
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateAudience = false
		};
	});
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("ApiScope", policy =>
	{
		policy.RequireAuthenticatedUser();
		policy.RequireClaim("scope", "ApiAccess");
	});
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo { Title = "Services.PlantsAPI", Version = "v1" });
	options.EnableAnnotations();
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = @"Enter 'Bearer' [space] and your token", //description displayed when someone enters a bearer token
		Name = "Authorization",
		In = ParameterLocation.Header, //a place to display
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});

	options.AddSecurityRequirement(new OpenApiSecurityRequirement {
		{new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				},
				Scheme = "oauth2",
				Name = "Bearer",
				In = ParameterLocation.Header
			},
		new List<string>()}
	});
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
