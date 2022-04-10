using System;
using Xunit;

namespace Infrastructure.UnitTests.Services;

public class DateTimeService
{
    private readonly Infrastructure.Services.DateTimeService _sut;
    public DateTimeService()
    {
        _sut = new Infrastructure.Services.DateTimeService();
    }

    [Fact]
    public void Now_ShouldReturnDateTimeNow()
    {
        var expected = DateTime.Now;

        // Act
        var now = _sut.Now;

        // Assert
        Assert.Equal(expected.ToLongDateString(), now.ToLongDateString());
    }

    [Fact]
    public void Today_ShouldReturnDatetimeToday()
    {
        // Act
        var today = _sut.Today;

        // Assert
        Assert.Equal(DateOnly.FromDateTime(DateTime.Today), today);
    }
}
