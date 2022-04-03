using Ardalis.Specification;

namespace ApplicationCore.TimeTrackAggregate.Specifications;

public class GetTimeTrackByToday : Specification<TimeTrack>, ISingleResultSpecification
{
    public GetTimeTrackByToday(DateOnly dateOnly)
    {
        Query
            .Where(x => x.Date == dateOnly)
            .Include(x => x.Items);
    }
}
