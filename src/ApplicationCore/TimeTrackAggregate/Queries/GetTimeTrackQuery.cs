using ApplicationCore.Interfaces;
using ApplicationCore.TimeTrackAggregate.Specifications;
using MediatR;

namespace ApplicationCore.TimeTrackAggregate.Queries;

public record GetTimeTrackQuery(DateOnly Date) : IRequest<TimeTrack?>
{

    public class Handler : IRequestHandler<GetTimeTrackQuery, TimeTrack?>
    {
        private readonly IReadRepository<TimeTrack> _readTimeTrackRepository;

        public Handler(IReadRepository<TimeTrack> readTimeTrackRepository)
        {
            _readTimeTrackRepository = readTimeTrackRepository;
        }

        public async Task<TimeTrack?> Handle(GetTimeTrackQuery request, CancellationToken cancellationToken)
        {
            var getTimeTrackByDateSpecification = new GetTimeTrackByDate(request.Date);
            var timeTrack = await _readTimeTrackRepository.GetBySpecAsync(getTimeTrackByDateSpecification, cancellationToken);

            return timeTrack;
        }
    }
}
