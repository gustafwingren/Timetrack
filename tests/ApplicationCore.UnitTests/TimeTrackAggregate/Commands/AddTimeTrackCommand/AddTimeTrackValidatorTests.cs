using System;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.ProjectAggregate;
using ApplicationCore.TimeTrackAggregate.Commands.AddTimeTrackCommand;
using Test.Shared;
using Xunit;

namespace ApplicationCore.UnitTests.TimeTrackAggregate.Commands.AddTimeTrackCommand;

public class AddTimeTrackValidatorTests : UnitTestBase
{
    private readonly AddTimeTrackValidator _sut;

    public AddTimeTrackValidatorTests()
    {
        _sut = new AddTimeTrackValidator(ProjectRepository as IReadRepository<Project>);
    }


    [Fact]
    public async Task Validate_WithValidModel_ShouldReturnNoErrors()
    {
        // Arrange
        await AddDefaultProject();
        var model = new ApplicationCore.TimeTrackAggregate.Commands.AddTimeTrackCommand.AddTimeTrack("TestNumber", "Task",
            new TimeSpan(0, 0, 30), "Description");

        // Act
        var result = await _sut.ValidateAsync(model);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(null, "Task", "00:01:00", "Description", "ProjectNumber is required")]
    [InlineData("", "Task", "00:01:00", "Description", "ProjectNumber is required")]
    [InlineData(" ", "Task", "00:01:00", "Description", "ProjectNumber is required")]
    [InlineData("ProjectNotExists", "Task", "00:01:00", "Description", "Project must exist")]
    [InlineData("TestNumber", null, "00:01:00", "Description", "Task is required")]
    [InlineData("TestNumber", "", "00:01:00", "Description", "Task is required")]
    [InlineData("TestNumber", " ", "00:01:00", "Description", "Task is required")]
    [InlineData("TestNumber", "ThisIsAVeryLongMessageWithMoreThan256Characters123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890", "00:01:00", "Description", "Task must be less than 256 characters")]
    [InlineData("TestNumber", "TestTask", null, "Description", "TimeSpent is required")]
    [InlineData("TestNumber", "TestTask", "", "Description", "TimeSpent is required")]
    [InlineData("TestNumber", "TestTask", " ", "Description", "TimeSpent is required")]
    [InlineData("TestNumber", "TestTask", "00:01:00", "ThisIsAVeryLongMessageWithMoreThan256Characters123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890", "Description must be less than 256 characters")]
    public async Task Handle_WithInvalidModel_ShouldReturnErrors(string projectNumber, string task, string timeSpent,
        string description, string expectedErrorMessage)
    {
        // Arrange
        TimeSpan.TryParse(timeSpent, out var timeSpentTimeSpan);
        await AddDefaultProject();
        var model = new ApplicationCore.TimeTrackAggregate.Commands.AddTimeTrackCommand.AddTimeTrack(projectNumber, task,
            timeSpentTimeSpan, description);

         // Act
        var result = await _sut.ValidateAsync(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(expectedErrorMessage, result.Errors[0].ErrorMessage);
    }

}
