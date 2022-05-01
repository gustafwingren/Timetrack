using ApplicationCore.Common.Interfaces;
using ApplicationCore.Interfaces;
using MediatR;

namespace ApplicationCore.ProjectAggregate.Commands.AddProjectCommand;

public record AddProject(string Name, string Number) : IRequest<Guid>
{
    public class Handler : IRequestHandler<AddProject, Guid>
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IDateTime _dateTime;

        public Handler(IRepository<Project> projectRepository, IDateTime dateTime)
        {
            _projectRepository = projectRepository;
            _dateTime = dateTime;
        }

        public async Task<Guid> Handle(AddProject request, CancellationToken cancellationToken)
        {
            var newProject = new Project(request.Name, request.Number, _dateTime.Now);

            var project = await _projectRepository.AddAsync(newProject, cancellationToken);

            return project.Id;
        }
    }
}

