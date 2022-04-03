using System.Reflection;
using ApplicationCore.ProjectAggregate;
using ApplicationCore.TimeTrackAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbC : DbContext
{
    public DbSet<TimeTrack> TimeTrackLists => Set<TimeTrack>();

    public DbSet<TimeTrackItem> TimeTrackItems => Set<TimeTrackItem>();

    public DbSet<Project> Projects => Set<Project>();

    public AppDbC(DbContextOptions<AppDbC> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
