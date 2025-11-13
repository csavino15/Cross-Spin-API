using Infrastructure.Clock;
using Microsoft.Extensions.Options;
using Quartz;

namespace Infrastructure.Jobs.ResetLeaderboard;

internal sealed class ResetLeaderboardJobSetup(IOptions<TimeZoneOptions> jobOptions) : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var tz = TimeZoneInfo.FindSystemTimeZoneById(jobOptions.Value!.TimeZoneId);
        ArgumentNullException.ThrowIfNull(tz);

        const string jobName = nameof(ResetLeaderboardJob);

        options.AddJob<ResetLeaderboardJob>(configure => configure.WithIdentity(jobName))
            .AddTrigger(configure => configure.ForJob(jobName)
                .WithSchedule(DailyTimeIntervalScheduleBuilder.Create()
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))
                .WithIntervalInHours(24)
                .OnEveryDay()
                .InTimeZone(tz))
            .Build());
    }
}
