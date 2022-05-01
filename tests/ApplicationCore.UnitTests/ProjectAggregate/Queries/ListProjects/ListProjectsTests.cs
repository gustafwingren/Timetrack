using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.ProjectAggregate;
using ApplicationCore.ProjectAggregate.Queries.ListProjects;
using Test.Shared;
using Xunit;
using static ApplicationCore.ProjectAggregate.Queries.ListProjects.ListProjectsQuery;

namespace ApplicationCore.UnitTests.ProjectAggregate.Queries.ListProjects;

public class ListProjectsTests : UnitTestBase
{
    private readonly Handler _sut;

    public ListProjectsTests()
    {
        _sut = new Handler(ProjectRepository as IReadRepository<Project>);
    }

    [Fact]
    public async Task Handle_WithValidData_ShouldReturnAllProjects()
    {
        // Arrange
        await AddDefaultProject();
        var request = new ListProjectsQuery();

        // Act
        var result = await _sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task Handle_WithNoProject_ShouldReturnEmptyList()
    {
        // Arrange
        var request = new ListProjectsQuery();

        // Act
        var result = await _sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
}
