using Microsoft.EntityFrameworkCore;
using MyWebAPI.Models;

namespace MyWebAPI.Context
{
    public class DrugDbContext : DbContext
    {
        public DrugDbContext(DbContextOptions<DrugDbContext> options) : base(options)
        {
        }
        public DbSet<Drug> Drugs { get; set; }

        public DbSet<Prescription> Prescriptions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            base.OnConfiguring(dbContextOptionsBuilder);
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot config = builder.Build();
            var conString = config.GetConnectionString("MyConnection");
            dbContextOptionsBuilder.UseSqlServer(conString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
