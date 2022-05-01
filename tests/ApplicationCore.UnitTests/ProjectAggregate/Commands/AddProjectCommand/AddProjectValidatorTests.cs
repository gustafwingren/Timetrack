using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.ProjectAggregate;
using ApplicationCore.ProjectAggregate.Commands.AddProjectCommand;
using Test.Shared;
using Xunit;

namespace ApplicationCore.UnitTests.ProjectAggregate.Commands.AddProjectCommand;

public class AddProjectValidatorTest : UnitTestBase
{
    private readonly AddProjectValidator _sut;
    public AddProjectValidatorTest()
    {
        _sut = new AddProjectValidator(ProjectRepository as IReadRepository<Project>);
    }

    [Fact]
    public async Task Validate_WithValidModel_ShouldReturnNoErrors()
    {
        // Arrange
        var model = new AddProject("Test", "123456");

        // Act
        var result = await _sut.ValidateAsync(model);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null, "123456", "Name can not be empty.")]
    [InlineData("", "123456", "Name can not be empty.")]
    [InlineData(" ", "123456", "Name can not be empty.")]
    [InlineData("TestProject", null, "Number can not be empty.")]
    [InlineData("TestProject", "", "Number can not be empty.")]
    [InlineData("TestProject", " ", "Number can not be empty.")]
    [InlineData("TestProject", "TestNumber", "A project with this projectNumber exists.")]
    public async Task Validate_WithInvalidModel_ShouldReturnCorrectErrors(string projectName, string projectNumber,
        string expectedErrorMessage)
    {
        // Arrange
        await AddDefaultProject();
        var model = new AddProject(projectName, projectNumber);

        // Act
        var result = await _sut.ValidateAsync(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(expectedErrorMessage, result.Errors[0].ErrorMessage);
    }
}
