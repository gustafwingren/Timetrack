using System.Threading.Tasks;
using ApplicationCore.ProjectAggregate.Commands.AddProjectCommand;
using MediatR;
using NSubstitute;
using Xunit;
using static Timetrack.Commands.AddProjectCommand;

namespace Timetrack.UnitTests.Commands;

public class AddProjectCommand
{
    private readonly IMediator _mediator;
    private readonly Handler _sut;

    public AddProjectCommand()
    {
        _mediator = Substitute.For<IMediator>();
        _sut = new Handler(_mediator);
    }

    [Fact]
    public async Task InvokeAsync_WithValidData_ShouldInvokeMediatorSend()
    {
        // Act
        await _sut.InvokeAsync(null);

        // Assert
        await _mediator.Received(1).Send(Arg.Any<AddProject>());
    }
}
