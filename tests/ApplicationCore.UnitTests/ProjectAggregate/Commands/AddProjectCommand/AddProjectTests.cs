using System;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using Test.Shared;
using Xunit;
using ApplicationCore.ProjectAggregate.Commands.AddProjectCommand;
using static ApplicationCore.ProjectAggregate.Commands.AddProjectCommand.AddProject;

namespace ApplicationCore.UnitTests.ProjectAggregate.Commands.AddProjectCommand;

public class AddProjectTests : UnitTestBase
{
    private readonly Handler _sut;

    public AddProjectTests()
    {
        _sut = new Handler(ProjectRepository,
            DateTime);
    }

    [Fact]
    public async Task Handle_WithValidModel_ShouldAddProject()
    {
        // Arrange
        DateTime.Now.Returns(new DateTime(2022, 4, 10));
        var request = new AddProject("New Project", "New Project Number");

        // Act
        var resultGuid = await _sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, resultGuid);
        var project = await ProjectRepository.GetByIdAsync(resultGuid);
        Assert.NotNull(project);
        Assert.Equal(request.Name, project?.Name);
        Assert.Equal(request.Number, project?.Number);
        Assert.Equal(new DateTime(2022, 4, 10), project?.CreatedDate);
    }
}
