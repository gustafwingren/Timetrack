using ApplicationCore.Common.Interfaces;
using ApplicationCore.Interfaces;
using ApplicationCore.ProjectAggregate;
using ApplicationCore.ProjectAggregate.Specifications;
using ApplicationCore.TimeTrackAggregate.Specifications;
using MediatR;

namespace ApplicationCore.TimeTrackAggregate.Commands.AddTimeTrack;

public record AddTimeTrack(string ProjectNumber, string Task, TimeSpan TimeSpent, string? Description = null) : IRequest<Guid>
{
    public class Handler : IRequestHandler<AddTimeTrack, Guid>
    {
        private readonly IDateTime _dateTime;
        private readonly IRepository<TimeTrack> _timeTrackRepository;
        private readonly IReadRepository<Project> _projectReadRepository;


        public Handler(IDateTime dateTime, IRepository<TimeTrack> timeTrackRepository, IReadRepository<Project> projectReadRepository)
        {
            _dateTime = dateTime;
            _timeTrackRepository = timeTrackRepository;
            _projectReadRepository = projectReadRepository;
        }

        public async Task<Guid> Handle(AddTimeTrack request, CancellationToken cancellationToken)
        {
            var getTimeTrackByTodaySpecification = new GetTimeTrackByDate(_dateTime.Today);
            var getProjectByNumber = new ProjectByNumber(request.ProjectNumber);
            var timeTrack =
                await _timeTrackRepository.GetBySpecAsync(getTimeTrackByTodaySpecification, cancellationToken);

            if (timeTrack is null)
            {
                timeTrack = new TimeTrack(_dateTime.Today);
                await _timeTrackRepository.AddAsync(timeTrack, cancellationToken);
            }

            var project = await _projectReadRepository.GetBySpecAsync(getProjectByNumber, cancellationToken);

            timeTrack.AddItem(project!.Id, request.Task, request.TimeSpent, request.Description);

            await _timeTrackRepository.UpdateAsync(timeTrack, cancellationToken);

            return timeTrack.Id;
        }
    }
}
