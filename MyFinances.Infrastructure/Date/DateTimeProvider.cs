using MyFinances.Application.Common.Interfaces;
using MyFinances.Application.Services.Interfaces;

namespace MyFinances.Application.Common;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}