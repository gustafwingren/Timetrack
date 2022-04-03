using ApplicationCore.Common.Interfaces;

namespace Infrastructure.Services;

public class DateTimeService: IDateTime
{
    public DateTime Now => DateTime.Now;

    public DateOnly Today => DateOnly.FromDateTime(DateTime.Today);
}
