using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sd.Jatek.Domain;

namespace sd.Jatek.Infrastructure.EntityConfigurations
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.RoomId);
            builder.Property(x => x.PlayerId);
            builder.Property(x => x.PlayerName);
            builder.Property(x => x.PlayerRole);
            builder.Property(x => x.Points);
            builder.Property(x => x.Place);
        }
    }
}
