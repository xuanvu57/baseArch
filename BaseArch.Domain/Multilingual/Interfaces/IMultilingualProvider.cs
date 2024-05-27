namespace BaseArch.Domain.Multilingual.Interfaces
{
    /// <summary>
    /// Multilingual provider interface
    /// </summary>
    public interface IMultilingualProvider
    {
        /// <summary>
        /// Get corresponding string by the string id in specific culture
        /// </summary>
        /// <param name="stringId">String id</param>
        /// <param name="values">Values to replace for placeholders</param>
        /// <returns></returns>
        Task<string> GetString(string stringId, params string[] values);
    }
}
