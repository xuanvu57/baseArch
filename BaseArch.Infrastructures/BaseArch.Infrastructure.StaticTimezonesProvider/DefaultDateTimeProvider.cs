using BaseArch.Domain.DependencyInjection;
using BaseArch.Domain.Timezones.Interfaces;

namespace BaseArch.Infrastructure.StaticTimezonesProvider
{
    /// <summary>
    /// Default date time provider us the static <see cref="DateTime"/>
    /// </summary>
    [DIService(DIServiceLifetime.Singleton)]
    public class DefaultDateTimeProvider : IDateTimeProvider
    {
        /// <inheritdoc/>
        public DateTime GetNow()
        {
            return DateTime.Now;
        }

        /// <inheritdoc/>
        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }

        /// <inheritdoc/>
        public DateOnly GetDateOnlyNow(bool utc)
        {
            return DateOnly.FromDateTime(utc ? GetUtcNow() : GetNow());
        }

        /// <inheritdoc/>
        public TimeOnly GetTimeOnlyNow(bool utc)
        {
            return TimeOnly.FromDateTime(utc ? GetUtcNow() : GetNow());
        }
    }
}
