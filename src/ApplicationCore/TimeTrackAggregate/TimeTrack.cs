using ApplicationCore.Interfaces;

namespace ApplicationCore.TimeTrackAggregate;

public class TimeTrack: BaseEntity, IAggregateRoot
{
    public DateOnly Date { get; private set; }
    private readonly List<TimeTrackItem> _items = new();
    public IReadOnlyCollection<TimeTrackItem> Items => _items.AsReadOnly();

    public TimeSpan TotalTime => _items.Aggregate(TimeSpan.Zero, (subtotal, item) => subtotal.Add(item.TimeSpent));

    public TimeTrack(DateOnly date)
    {
        Date = date;
    }

    public void AddItem(Guid projectId, string task, TimeSpan timeSpent, string? description = null)
    {
        if (!Items.Any(i => i.ProjectId == projectId && string.Equals(i.Task, task, StringComparison.InvariantCultureIgnoreCase)))
        {
            _items.Add(new TimeTrackItem(projectId, task, timeSpent, description));
            return;
        }

        var existingItem = Items.FirstOrDefault(i => i.ProjectId == projectId && string.Equals(i.Task, task, StringComparison.InvariantCultureIgnoreCase));
        existingItem!.AddTimeSpent(timeSpent);
    }
}
