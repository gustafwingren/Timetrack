using ApplicationCore.Interfaces;
using MediatR;

namespace ApplicationCore.ProjectAggregate.Queries.ListProjects;

public record ListProjectsQuery : IRequest<IEnumerable<Project>>
{
    public class Handler : IRequestHandler<ListProjectsQuery, IEnumerable<Project>>
    {
        private readonly IReadRepository<Project> _projectReadRepository;

        public Handler(IReadRepository<Project> projectReadRepository)
        {
            _projectReadRepository = projectReadRepository;
        }

        public async Task<IEnumerable<Project>> Handle(ListProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _projectReadRepository.ListAsync(cancellationToken);

            return projects;
        }
    }
}
