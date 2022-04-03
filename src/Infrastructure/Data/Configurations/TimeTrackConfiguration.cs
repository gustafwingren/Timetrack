using ApplicationCore.TimeTrackAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class TimeTrackConfiguration : IEntityTypeConfiguration<TimeTrack>
{
    public void Configure(EntityTypeBuilder<TimeTrack> builder)
    {
        var navigation = builder.Metadata.FindNavigation(nameof(TimeTrack.Items));
        navigation!.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(p => p.Date)
            .IsRequired();
    }
}
