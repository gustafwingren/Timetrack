using System.CommandLine;
using System.CommandLine.Invocation;
using ApplicationCore.TimeTrackAggregate.Commands.AddTimeTrack;
using MediatR;

namespace Timetrack.Commands;

public class CreateTimeTrackCommand : Command
{
    public CreateTimeTrackCommand()
        : base("add", "Add a new row for today")
    {
        AddArgument(new Argument<string>("projectNumber", "Project number"));
        AddArgument(new Argument<string>("task", "Task"));
        AddArgument(new Argument<TimeSpan>("timeSpent", "Time spent"));
        AddOption(new Option<string?>(new[] { "--description", "-d" }, "Description"));
    }

    public new class Handler: ICommandHandler
    {
        private readonly IMediator _mediator;

        public string ProjectNumber { get; set; } = string.Empty;

        public string Task { get; set; } = string.Empty;

        public TimeSpan TotalTime { get; set; }

        public string? Description { get; set; }

        public Handler(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            await _mediator.Send(new AddTimeTrack(ProjectNumber, Task, TotalTime, Description));
            return 0;
        }
    }
}
