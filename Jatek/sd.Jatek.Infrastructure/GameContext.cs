using Microsoft.EntityFrameworkCore;
using sd.Jatek.Domain;
using sd.Jatek.Infrastructure.EntityConfigurations;

namespace sd.Jatek.Infrastructure;

public class GameContext(DbContextOptions<GameContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RoomConfiguration());
        modelBuilder.ApplyConfiguration(new PlayerConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Room> Rooms { get; set; }
    public DbSet<Player> Players { get; set; }
}
