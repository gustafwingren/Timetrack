using Ardalis.Specification;

namespace ApplicationCore.TimeTrackAggregate.Specifications;

public class GetTimeTrackByDate : Specification<TimeTrack>, ISingleResultSpecification
{
    public GetTimeTrackByDate(DateOnly dateOnly)
    {
        Query
            .Where(x => x.Date == dateOnly)
            .Include(x => x.Items).ThenInclude(x => x.Project);
    }
}
