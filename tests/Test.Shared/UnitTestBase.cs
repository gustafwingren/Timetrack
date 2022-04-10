using ApplicationCore.Common.Interfaces;
using ApplicationCore.Interfaces;
using ApplicationCore.ProjectAggregate;
using NSubstitute;

namespace Test.Shared;

public class UnitTestBase
{
    protected readonly IRepository<Project> ProjectRepository;
    protected readonly IDateTime DateTime;

    protected UnitTestBase()
    {
        DateTime = Substitute.For<IDateTime>();
        ProjectRepository = new FakeRepository<Project>(new List<Project>());
    }

    public async Task AddDefaultProject()
    {
        var project = new Project("Test Project", "TestNumber", DateTime.Now);
        await ProjectRepository.AddAsync(project);
    }
}
