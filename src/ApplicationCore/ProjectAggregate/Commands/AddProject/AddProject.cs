using ApplicationCore.Common.Interfaces;
using ApplicationCore.Interfaces;
using MediatR;

namespace ApplicationCore.ProjectAggregate.Commands.AddProject;

public record AddProject(string Name, string Number) : IRequest<Guid>
{
    public class Handler : IRequestHandler<AddProject, Guid>
    {
        private readonly IRepository<Project> _projectReadRepository;
        private readonly IDateTime _dateTime;

        public Handler(IRepository<Project> projectReadRepository, IDateTime dateTime)
        {
            _projectReadRepository = projectReadRepository;
            _dateTime = dateTime;
        }

        public async Task<Guid> Handle(AddProject request, CancellationToken cancellationToken)
        {
            var newProject = new Project(request.Name, request.Number, _dateTime.Now);

            await _projectReadRepository.AddAsync(newProject, cancellationToken);

            return newProject.Id;
        }
    }
}

