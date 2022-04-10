using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.ProjectAggregate;
using ApplicationCore.TimeTrackAggregate.Commands.AddTimeTrackCommand;
using NSubstitute;
using Test.Shared;
using Xunit;
using static ApplicationCore.TimeTrackAggregate.Commands.AddTimeTrackCommand.AddTimeTrack;

namespace ApplicationCore.UnitTests.TimeTrackAggregate.Commands.AddTimeTrackCommand;

public class AddTimeTrackTests : UnitTestBase
{
    private readonly Handler _sut;

    public AddTimeTrackTests()
    {
        _sut = new Handler(DateTime, TimeTrackRepository, ProjectRepository as IReadRepository<Project>);
    }

    [Fact]
    public async Task Handle_WithValidModel_ShouldAddTimeTrack()
    {
        // Arrange
        await AddDefaultProject();
        DateTime.Today.Returns(new DateOnly(2022, 4, 10));
        var request = new AddTimeTrack("TestNumber", "Task", new TimeSpan(0, 30, 0), "Description");

        // Act
        var result = await _sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        var timeTrack = await TimeTrackRepository.GetByIdAsync(result);
        var project = (await ProjectRepository.ListAsync(CancellationToken.None)).FirstOrDefault();
        Assert.NotNull(timeTrack);
        Assert.Equal(new DateOnly(2022, 4, 10), timeTrack!.Date);
        Assert.NotEmpty(timeTrack!.Items);
        Assert.Collection(timeTrack.Items,
            i =>
            {
                Assert.Equal(request.Description, i.Description);
                Assert.Equal(request.Task, i.Task);
                Assert.Equal(request.TimeSpent, i.TimeSpent);
                Assert.Equal(project?.Id, i.ProjectId);
            });
    }

    [Fact]
    public async Task Handle_WithValidModel_ShouldCalculateTotalTime()
    {
        // Arrange
        await AddDefaultProject();
        await AddDefaultTimeTrack();
        DateTime.Today.Returns(new DateOnly(2022, 4, 10));
        var request1 = new AddTimeTrack("TestNumber", "Task", new TimeSpan(1, 30, 0), "Description");
        var request2 = new AddTimeTrack("TestNumber", "Task", new TimeSpan(0, 30, 0), "Description");

        // Act
        var result1 = await _sut.Handle(request1, CancellationToken.None);
        var result2 = await _sut.Handle(request2, CancellationToken.None);

        // Assert
        Assert.Equal(result1, result2);
        var timeTrack = await TimeTrackRepository.GetByIdAsync(result2);
        Assert.NotNull(timeTrack);
        Assert.Equal(new TimeSpan(2, 0, 0), timeTrack?.TotalTime);
    }

    [Fact]
    public async Task Handle_WithValidModel_ShouldSetTimeTrackId()
    {
        // Arrange
        await AddDefaultProject();
        var defaultTimeTrack = await AddDefaultTimeTrack();
        DateTime.Today.Returns(new DateOnly(2022, 4, 10));
        var request = new AddTimeTrack("TestNumber", "Task", new TimeSpan(0, 30, 0), "Description");

        // Act
        var result = await _sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        var timeTrack = await TimeTrackRepository.GetByIdAsync(result);
        Assert.NotNull(timeTrack);
        Assert.Equal(defaultTimeTrack.Id, timeTrack!.Id);
    }
}
