using Microsoft.EntityFrameworkCore;

namespace WeatherStats.Kent.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherData>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<WeatherData>()
                .HasOne(p => p.WeatherStation)
                    .WithMany(b => b.WeatherData);
            modelBuilder.Entity<WeatherStation>()
                .HasKey(x => x.Id);
        }

        public DbSet<WeatherData> WeatherData { get; set; }
        public DbSet<WeatherStation> WeatherStation { get; set; }
    }
}
