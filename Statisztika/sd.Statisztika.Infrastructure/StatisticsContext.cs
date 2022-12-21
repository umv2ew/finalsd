using Microsoft.EntityFrameworkCore;
using sd.Statisztika.Domain;

namespace sd.Statisztika.Infrastructure
{
    public class StatisticsContext : DbContext
    {
        public StatisticsContext(DbContextOptions<StatisticsContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Statistic> Statistics { get; set; }
    }
}