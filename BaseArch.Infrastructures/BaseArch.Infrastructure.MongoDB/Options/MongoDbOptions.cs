namespace BaseArch.Infrastructure.MongoDB.Options
{
    /// <summary>
    /// MongoDb options
    /// </summary>
    public sealed class MongoDbOptions
    {
        /// <summary>
        /// Connection string 
        /// </summary>
        public required string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// Database name
        /// </summary>
        public required string DatabaseName { get; set; } = string.Empty;

        /// <summary>
        /// Enable auto transaction
        /// </summary>
        public bool AutoTransaction { get; set; }
    }
}
