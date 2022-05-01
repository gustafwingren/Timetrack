using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.ProjectAggregate;
using ApplicationCore.ProjectAggregate.Queries.ListProjects;
using MediatR;
using NSubstitute;
using SQLitePCL;
using Xunit;
using static Timetrack.Commands.ListProjectCommand;

namespace Timetrack.UnitTests.Commands;

public class ListProjectCommandTests
{
    private readonly Handler _sut;
    private readonly IMediator _mediator;

    public ListProjectCommandTests()
    {
        _mediator = Substitute.For<IMediator>();
        _sut = new Handler(_mediator);
    }

    [Fact]
    public async Task InvokeAsync_WithValidData_ShouldInvokeMediatorSend()
    {
        // Act
        await _sut.InvokeAsync(default);
        
        // Assert
        await _mediator.Received(1).Send(Arg.Any<ListProjectsQuery>());
    }

    [Fact]
    public async Task InvokeAsync_WithAddedProject_MediatorShouldReturnProjects()
    {
        // Arrange
        _mediator.Send(Arg.Any<ListProjectsQuery>()).Returns(new List<Project>()
        {
            new("Test", "123", new DateTime(2022, 5, 1)),
        });
        
        // Act
        var result = await _sut.InvokeAsync(default);
        
        // Assert
        Assert.Equal(0, result);
    }
}
