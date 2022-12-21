using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sd.Statisztika.Domain;

namespace sd.Statisztika.Infrastructure
{
    public class StatisticsConfiguration : IEntityTypeConfiguration<Statistic>
    {
        public void Configure(EntityTypeBuilder<Statistic> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.PlayedGames);
            builder.Property(x => x.Points);
            builder.Property(x => x.NumberOfWins);
            builder.Property(x => x.UserId);
        }
    }
}
