using System;
using System.Threading.Tasks;
using ApplicationCore.TimeTrackAggregate.Commands.AddTimeTrackCommand;
using MediatR;
using NSubstitute;
using Timetrack.Commands;
using Xunit;
using static Timetrack.Commands.CreateTimeTrackCommand;

namespace Timetrack.UnitTests.Commands;

public class CreateTimeTrackCommandTests
{
    private readonly Handler _sut;
    private readonly IMediator _mediator;

    public CreateTimeTrackCommandTests()
    {
        _mediator = Substitute.For<IMediator>();
        _sut = new Handler(_mediator);
    }

    [Fact]
    public async Task InvokeAsync_WithValidData_ShouldInvokeMediatorSend()
    {
        // Arrange
        _sut.Description = "Description";
        _sut.Task = "Task1";
        _sut.ProjectNumber = "123";
        _sut.TimeSpent = new TimeSpan(0, 1, 0);
        
        // Act
        await _sut.InvokeAsync(null);
        
        // Assert
        await _mediator.Received(1).Send(Arg.Is<AddTimeTrack>(x => x.Description == "Description" && x.Task == "Task1" && x.ProjectNumber == "123" && x.TimeSpent == new TimeSpan(0, 1, 0)));
    }
}
