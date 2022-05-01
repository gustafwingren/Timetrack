using System.CommandLine;
using System.CommandLine.Invocation;
using ApplicationCore.ProjectAggregate.Commands.AddProjectCommand;
using MediatR;

namespace Timetrack.Commands;

public class AddProjectCommand : Command
{
    public AddProjectCommand()
        : base("add", "Add a new project")
    {
        AddArgument(new Argument<string>("Name", "Name of the project"));
        AddArgument(new Argument<string>("Number", "Number of the project"));
    }

    public new class Handler : ICommandHandler
    {
        private readonly IMediator _mediator;

        public string Name { get; set; } = string.Empty;

        public string Number { get; set; } = string.Empty;

        public Handler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            await _mediator.Send(
                new AddProject(Name, Number));
            return 0;
        }
    }
}
