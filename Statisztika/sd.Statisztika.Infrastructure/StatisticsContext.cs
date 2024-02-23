using Microsoft.EntityFrameworkCore;
using sd.Statisztika.Domain;

namespace sd.Statisztika.Infrastructure;

public class StatisticsContext(DbContextOptions<StatisticsContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Statistic> Statistics { get; set; }
}
