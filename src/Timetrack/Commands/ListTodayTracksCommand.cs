using System.CommandLine;
using System.CommandLine.Invocation;
using ApplicationCore.Common.Interfaces;
using ApplicationCore.TimeTrackAggregate.Queries;
using MediatR;
using Spectre.Console;

namespace Timetrack.Commands;

public class ListTodayTracksCommand : Command
{
    public ListTodayTracksCommand() : base("today", "List today's tracked time")
    {
    }

    public new class Handler : ICommandHandler
    {
        private readonly IMediator _mediatr;
        private readonly IDateTime _dateTime;

        public Handler(IMediator mediatr, IDateTime dateTime)
        {
            _mediatr = mediatr;
            _dateTime = dateTime;
        }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            var timeTrack = await _mediatr.Send(new GetTimeTrackQuery(_dateTime.Today), context.GetCancellationToken());

            if (timeTrack is null)
            {
                var panel = new Panel("No time tracked today");
                panel.Header("TimeTrack",Justify.Center);
                panel.Border = BoxBorder.Rounded;

                AnsiConsole.Write(panel);
                return 0;
            }

            var table = new Table
            {
                Title = new TableTitle("Today's tracked time"),
            };
            table.AddColumn("Project Name");
            table.AddColumn("Project Number");
            table.AddColumn("Task");
            table.AddColumn("Description");
            table.AddColumn("TimeSpent");

            foreach (var timeTrackItem in timeTrack.Items)
            {
                table.AddRow(timeTrackItem.Project!.Name, timeTrackItem.Project!.Number, timeTrackItem.Task, timeTrackItem.Description ?? string.Empty, timeTrackItem.TimeSpent.ToString());
            }

            table.Columns[4].Footer(timeTrack.TotalTime.ToString());

            var grid = new Grid().Alignment(Justify.Center);
            grid.AddColumn(new GridColumn());
            grid.AddRow(table);

            AnsiConsole.Write(new Panel(grid));
            return 0;
        }
    }
}
