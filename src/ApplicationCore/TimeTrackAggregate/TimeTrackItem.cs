using ApplicationCore.ProjectAggregate;

namespace ApplicationCore.TimeTrackAggregate;

public class TimeTrackItem : BaseEntity
{
    public Guid ProjectId { get; private set; }

    public Project? Project { get; private set; }

    public string Task { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public TimeSpan TimeSpent { get; private set; }

    public TimeTrackItem(Guid projectId, string task, TimeSpan timeSpent, string? description = null)
    {
        ProjectId = projectId;
        Task = task;
        Description = description;

        SetTimeSpent(timeSpent);
    }

    public void AddTimeSpent(TimeSpan timeSpent)
    {
        TimeSpent += timeSpent;
    }

    public void SetTimeSpent(TimeSpan timeSpent)
    {
        TimeSpent = timeSpent;
    }
}
