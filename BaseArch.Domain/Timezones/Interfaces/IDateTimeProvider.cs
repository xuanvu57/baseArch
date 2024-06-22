namespace BaseArch.Domain.Timezones.Interfaces
{
    /// <summary>
    /// Date and time provider
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Get current time as UTC time
        /// </summary>
        /// <returns><see cref="DateTime"/></returns>
        DateTime GetUtcNow();

        /// <summary>
        /// Get current time as the system time
        /// </summary>
        /// <returns><see cref="DateTime"/></returns>
        DateTime GetNow();

        /// <summary>
        /// Get current date only
        /// </summary>
        /// <param name="utc">It should be based on UTC or not</param>
        /// <returns><see cref="DateOnly"/></returns>
        DateOnly GetDateOnlyNow(bool utc);

        /// <summary>
        /// Get current time only in day
        /// </summary>
        /// <param name="utc">It should be based on UTC or not</param>
        /// <returns><see cref="TimeOnly"/></returns>
        TimeOnly GetTimeOnlyNow(bool utc);
    }
}
