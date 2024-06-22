using BaseArch.Domain.DependencyInjection;
using BaseArch.Domain.Timezones.Interfaces;

namespace BaseArch.Infrastructure.StaticTimezonesProvider
{
    [DIService(DIServiceLifetime.Singleton)]
    public class DefaultDateTimeProvider : IDateTimeProvider
    {
        public DateTime GetNow()
        {
            return DateTime.Now;
        }

        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
        public DateOnly GetDateOnlyNow(bool utc)
        {
            return DateOnly.FromDateTime(utc ? GetUtcNow() : GetNow());
        }

        public TimeOnly GetTimeOnlyNow(bool utc)
        {
            return TimeOnly.FromDateTime(utc ? GetUtcNow() : GetNow());
        }
    }
}
