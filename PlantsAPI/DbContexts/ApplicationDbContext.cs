
using Microsoft.EntityFrameworkCore;
using Services.PlantsAPI.Models;

namespace Services.PlantsAPI.DbContexts
{
	public class ApplicationDbContext : DbContext
	{
        public DbSet<Plant> Plants{ get; set; }
        public DbSet<PlantName> PlantNames{ get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
    }
}
