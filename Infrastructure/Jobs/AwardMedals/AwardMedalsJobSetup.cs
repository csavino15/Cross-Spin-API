using Infrastructure.Clock;
using Microsoft.Extensions.Options;
using Quartz;

namespace Infrastructure.Jobs.AwardMedals;

internal sealed class AwardMedalsJobSetup(IOptions<TimeZoneOptions> jobOptions) : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var tz = TimeZoneInfo.FindSystemTimeZoneById(jobOptions.Value!.TimeZoneId);
        ArgumentNullException.ThrowIfNull(tz);
        const string jobName = nameof(AwardMedalsJob);
        options.AddJob<AwardMedalsJob>(configure => configure.WithIdentity(jobName))
            .AddTrigger(configure => configure.ForJob(jobName)
                .WithSchedule(DailyTimeIntervalScheduleBuilder.Create()
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(23, 59))
                .WithIntervalInHours(24)
                .OnEveryDay()
                .InTimeZone(tz))
            .Build());
    }
}