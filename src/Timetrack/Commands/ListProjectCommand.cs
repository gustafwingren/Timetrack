using System.CommandLine;
using System.CommandLine.Invocation;
using ApplicationCore.ProjectAggregate.Queries.ListProjects;
using MediatR;
using Spectre.Console;

namespace Timetrack.Commands;

public class ListProjectCommand : Command
{
    public ListProjectCommand()
        : base("list", "List defined projects")
    {
    }

    public new class Handler : ICommandHandler
    {
        private readonly IMediator _mediator;

        public Handler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            var projects = await _mediator.Send(new ListProjectsQuery());

            var table = new Table
            {
                Title = new TableTitle($"Projects ({projects.Count()})"),
            };
            table.AddColumn("Id");
            table.AddColumn("ProjectNumber");
            table.AddColumn("ProjectName");

            foreach (var project in projects)
            {
                table.AddRow(project.Id.ToString(), project.Number.ToString(), project.Name);
            }

            var grid = new Grid().Alignment(Justify.Center);
            grid.AddColumn(new GridColumn());
            grid.AddRow(table);

            AnsiConsole.Write(new Panel(grid));

            return 0;
        }
    }
}
