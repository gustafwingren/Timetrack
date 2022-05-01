using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.ProjectAggregate;
using ApplicationCore.TimeTrackAggregate;
using ApplicationCore.TimeTrackAggregate.Queries;
using Test.Shared;
using Xunit;
using static ApplicationCore.TimeTrackAggregate.Queries.GetTimeTrackQuery;

namespace ApplicationCore.UnitTests.TimeTrackAggregate.Queries;

public class GetTimeTrackQueryTests : UnitTestBase
{
    private readonly Handler _sut;

    public GetTimeTrackQueryTests()
    {
        _sut = new Handler(TimeTrackRepository as IReadRepository<TimeTrack>);
    }

    [Fact]
    public async Task Handle_WithValidModel_ShouldOnlyReturnProvidedDateTimeTrackItems()
    {
        // Arrange
        await SetupRepo();
        var request = new GetTimeTrackQuery(new DateOnly(2022, 4, 10));

        // Act
        var result = await _sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result!.Items);
        Assert.Equal(new TimeSpan(3, 0, 0), result?.TotalTime);
        Assert.Equal(2, result?.Items.Count);
        Assert.Collection(result.Items, i =>
        {
            Assert.Equal("Task1", i.Task);
        }, i =>
        {
            Assert.Equal("Task2", i.Task);
        });
    }

    private async Task SetupRepo()
    {
        var project = await ProjectRepository.AddAsync(new Project("TestProject", "TestProjectNumber", new DateTime(2022, 4, 9)));
        var timeTrack = new TimeTrack(new DateOnly(2022, 4, 9));
        timeTrack.AddItem(project.Id, "Task1", new TimeSpan(0, 30, 0));
        timeTrack.AddItem(project.Id, "Task2", new TimeSpan(1, 30, 0));
        timeTrack.AddItem(project.Id, "Task3", new TimeSpan(0, 15, 0));
        await TimeTrackRepository.AddAsync(timeTrack);

        var timeTrack2 = new TimeTrack(new DateOnly(2022, 4, 10));
        timeTrack2.AddItem(project.Id, "Task1", new TimeSpan(1, 30, 0));
        timeTrack2.AddItem(project.Id, "Task2", new TimeSpan(1, 30, 0));
        await TimeTrackRepository.AddAsync(timeTrack2);
    }
}
