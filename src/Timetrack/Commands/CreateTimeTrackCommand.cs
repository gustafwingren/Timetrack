using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using ApplicationCore.TimeTrackAggregate.Commands.AddTimeTrack;
using MediatR;

namespace Timetrack.Commands;

public class CreateTimeTrackCommand : Command
{
    public CreateTimeTrackCommand()
        : base("add", "Add a new row for today")
    {
        AddArgument(new Argument<string>("ProjectNumber", "Project number"));
        AddArgument(new Argument<string>("Task", "Task"));
        AddArgument(new Argument<TimeSpan>("TimeSpent", ParseTimeSpan, false, "Time spent"));
        AddOption(new Option<string?>(new[] { "--description", "-d" }, "Description"));
    }

    private static TimeSpan ParseTimeSpan(ArgumentResult result)
    {
        return TimeSpan.Parse(result.Tokens.Single().Value);
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
