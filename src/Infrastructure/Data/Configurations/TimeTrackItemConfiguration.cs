using ApplicationCore.TimeTrackAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class TimeTrackItemConfiguration : IEntityTypeConfiguration<TimeTrackItem>
{
    public void Configure(EntityTypeBuilder<TimeTrackItem> builder)
    {
        builder.Property(p => p.Task)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(p => p.TimeSpent)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(256);

        builder.HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId);
    }
}
