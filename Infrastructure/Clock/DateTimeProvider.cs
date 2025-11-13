using Application.Abstractions.Clock;
using Microsoft.Extensions.Options;

namespace Infrastructure.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    private readonly string _timeZoneId;

    public DateTimeProvider(IOptions<TimeZoneOptions> options)
    {
        _timeZoneId = options.Value.TimeZoneId;
    }

    public DateTime UtcNow => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, GetGameTimeZone);
    public TimeZoneInfo GetGameTimeZone => TimeZoneInfo.FindSystemTimeZoneById(_timeZoneId);
}
