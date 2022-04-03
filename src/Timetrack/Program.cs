using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using ApplicationCore;
using Infrastructure;
using Microsoft.Extensions.Hosting;
using Serilog;
using Timetrack;
using Timetrack.Commands;

var runner = BuildCommandLine()
    .UseHost(_ => CreateHostBuilder(args), (builder) => builder
        .UseEnvironment("CLI")
        .UseSerilog()
        .ConfigureServices((hostContext, services) =>
        {
            services.AddSerilog();
            var configuration = hostContext.Configuration;
            services.AddCli();
            services.AddApplication();
            services.AddInfrastructure(configuration);
        })
        .UseCommandHandler<CreateTimeTrackCommand, CreateTimeTrackCommand.Handler>()
        .UseCommandHandler<AddProjectCommand, AddProjectCommand.Handler>()
        .UseCommandHandler<ListProjectCommand, ListProjectCommand.Handler>())
    .UseDefaults()
    .Build();

runner.Invoke(args);

static CommandLineBuilder BuildCommandLine()
{
    var root = new RootCommand();
    root.AddCommand(BuildTodoListCommands());
    root.AddCommand(BuildProjectCommands());
    root.AddGlobalOption(new Option<bool>("--silent", "Disables diagnostics output"));
    // root.Handler = CommandHandler.Create(() => root.Invoke("-h"));

    return new CommandLineBuilder(root);

    static Command BuildTodoListCommands()
    {
        var timeTrack = new Command("time", "Track time management")
        {
            new CreateTimeTrackCommand(),
        };
        return timeTrack;
    }

    static Command BuildProjectCommands()
    {
        var project = new Command("project", "Project management")
        {
            new AddProjectCommand(),
            new ListProjectCommand(),
        };
        return project;
    }
}

static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args);
