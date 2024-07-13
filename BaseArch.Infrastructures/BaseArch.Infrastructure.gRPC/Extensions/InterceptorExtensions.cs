using Grpc.Core;

namespace BaseArch.Infrastructure.gRPC.Extensions
{
    /// <summary>
    /// Extension methods for interceptor
    /// </summary>
    internal static class InterceptorExtensions
    {
        /// <summary>
        /// Add metadata key if it does not exist
        /// </summary>
        /// <param name="metadata"><see cref="Metadata"/></param>
        /// <param name="entry"><see cref="Metadata.Entry"/></param>
        /// <returns><see cref="Metadata"/></returns>
        public static Metadata AddIfNonExistent(Metadata metadata, Metadata.Entry entry)
        {
            if (metadata.Get(entry.Key) == null) metadata.Add(entry);
            return metadata;
        }
    }
}
