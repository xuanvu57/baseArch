namespace BaseArch.Application.Repositories.Enums
{
    /// <summary>
    /// Database type enums
    /// </summary>
    public class DatabaseTypeEnums
    {
        /// <summary>
        /// Database types
        /// </summary>
        public enum DatabaseType
        {
            /// <summary>
            /// General database with Entity Framework
            /// </summary>
            GeneralEfDb,

            /// <summary>
            /// In-memory database (with Entity Framework)
            /// </summary>
            InMemoryDb,

            /// <summary>
            /// MySql database
            /// </summary>
            MySql,

            /// <summary>
            /// PostgreSql database
            /// </summary>
            PostgreSql,

            /// <summary>
            /// Microsoft Sql database
            /// </summary>
            MsSql,

            /// <summary>
            /// Oracle database
            /// </summary>
            Oracle,

            /// <summary>
            /// MongoDb databse
            /// </summary>
            MongoDb
        }
    }
}
