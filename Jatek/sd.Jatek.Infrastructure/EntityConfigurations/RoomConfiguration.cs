using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sd.Jatek.Domain;

namespace sd.Jatek.Infrastructure.EntityConfigurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.RoomId);
            builder.Property(x => x.Rounds);
            builder.Property(x => x.RightGuess);
            builder.Property(x => x.Started);
        }
    }
}