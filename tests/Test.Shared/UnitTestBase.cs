using ApplicationCore.Common.Interfaces;
using ApplicationCore.Interfaces;
using ApplicationCore.ProjectAggregate;
using ApplicationCore.TimeTrackAggregate;
using NSubstitute;

namespace Test.Shared;

public class UnitTestBase
{
    protected readonly IRepository<Project> ProjectRepository;
    protected readonly IRepository<TimeTrack> TimeTrackRepository;
    protected readonly IDateTime DateTime;

    protected UnitTestBase()
    {
        DateTime = Substitute.For<IDateTime>();
        ProjectRepository = new FakeRepository<Project>(new List<Project>());
        TimeTrackRepository = new FakeRepository<TimeTrack>(new List<TimeTrack>());
    }

    protected async Task AddDefaultProject()
    {
        var project = new Project("Test Project", "TestNumber", DateTime.Now);
        await ProjectRepository.AddAsync(project);
    }

    protected async Task<TimeTrack> AddDefaultTimeTrack()
    {
        var timeTrack = new TimeTrack(new DateOnly(2022, 4, 10));
        return await TimeTrackRepository.AddAsync(timeTrack);
    }
}
