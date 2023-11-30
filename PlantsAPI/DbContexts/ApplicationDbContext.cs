
using Microsoft.EntityFrameworkCore;
using Services.PlantsAPI.Models;

namespace Services.PlantsAPI.DbContexts
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<Plant> Plants { get; set; }
		public DbSet<PlantName> PlantNames { get; set; }
		public DbSet<ImageLink> ImageLink { get; set; }
		public DbSet<Color> Palette { get; set; }
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
			//!!!debug
			//Database.EnsureDeleted();
			//Database.EnsureCreated();

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Plant>().HasData(
				new Plant
				{

					PlantId = 1,
					//Names = { aconit, borets },
					FlowerColorCode = 6490276,
					Poisonous = true,
					ForHerbalTea = false,
					PickingProhibited = false
				},
				new Plant
				{
					PlantId = 2,
					//Names = { dushitsa },
					FlowerColorCode = 14298317,
					Poisonous = false,
					ForHerbalTea = true,
					PickingProhibited = false
				});

			modelBuilder.Entity<PlantName>().HasData(
				 new PlantName()
				 {
					 PlantNameId = 1,
					 PlantId = 1,
					 Name = "Aconit"
				 },
				 new PlantName()
				 {
					 PlantNameId = 2,
					 PlantId = 1,
					 Name = "Boretskyyyyy"
				 },
				 new PlantName()
				 {
					 PlantNameId = 3,
					 PlantId = 2,
					 Name = "Dushitsa"
				 });
		}

	}
}
