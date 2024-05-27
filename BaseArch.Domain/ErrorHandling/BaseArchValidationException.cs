namespace BaseArch.Domain.ErrorHandling
{
    /// <summary>
    /// Base Arch validation exception
    /// </summary>
    public class BaseArchValidationException : Exception
    {
        /// <summary>
        /// Error messages which are grouped by fields
        /// </summary>
        public IDictionary<string, string[]> Errors { get; init; }

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="errors">Intialized list of errors which are grouped by field</param>
        public BaseArchValidationException(IDictionary<string, string[]> errors)
        {
            Errors = errors;
        }

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="parameName">Name of invalid parameter</param>
        /// <param name="errorMessages">List of errors</param>
        public BaseArchValidationException(string parameName, string[] errorMessages)
        {
            Errors = new Dictionary<string, string[]>
            {
                { parameName, errorMessages }
            };
        }
    }
}
