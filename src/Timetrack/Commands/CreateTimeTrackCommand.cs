using System.CommandLine;
using System.CommandLine.Invocation;
using ApplicationCore.TimeTrackAggregate.Commands.AddTimeTrackCommand;
using MediatR;
using Timetrack.Helpers;

namespace Timetrack.Commands;

public class CreateTimeTrackCommand : Command
{
    public CreateTimeTrackCommand()
        : base("add", "Add a new row for today")
    {
        AddArgument(new Argument<string>("ProjectNumber", "Project number"));
        AddArgument(new Argument<string>("Task", "Task"));
        AddArgument(new Argument<TimeSpan>("TimeSpent", ParseHelperMethods.ParseTimeSpan, false, "Time spent"));
        AddOption(new Option<string?>(new[] { "--description", "-d" }, "Description"));
    }
    
    public new class Handler: ICommandHandler
    {
        private readonly IMediator _mediator;

        public string ProjectNumber { get; set; } = string.Empty;

        public string Task { get; set; } = string.Empty;

        public TimeSpan TimeSpent { get; set; }

        public string? Description { get; set; }

        public Handler(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            await _mediator.Send(new AddTimeTrack(ProjectNumber, Task, TimeSpent, Description));
            return 0;
        }
    }
}
