using System.Threading.Tasks;
using ApplicationCore.ProjectAggregate.Commands.AddProjectCommand;
using MediatR;
using NSubstitute;
using Xunit;
using static Timetrack.Commands.AddProjectCommand;

namespace Timetrack.UnitTests.Commands;

public class AddProjectCommandTests
{
    private readonly IMediator _mediator;
    private readonly Handler _sut;

    public AddProjectCommandTests()
    {
        _mediator = Substitute.For<IMediator>();
        _sut = new Handler(_mediator);
    }

    [Fact]
    public async Task InvokeAsync_WithValidData_ShouldInvokeMediatorSend()
    {
        // Arrange
        _sut.Name = "Test";
        _sut.Number = "123";
        
        // Act
        await _sut.InvokeAsync(null);

        // Assert
        await _mediator.Received(1).Send(Arg.Is<AddProject>(x => x.Name == "Test" && x.Number == "123"));
    }
}
