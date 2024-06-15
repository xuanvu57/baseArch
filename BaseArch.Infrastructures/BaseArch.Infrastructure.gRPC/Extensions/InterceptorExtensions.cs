using Grpc.Core;

namespace BaseArch.Infrastructure.gRPC.Extensions
{
    internal static class InterceptorExtensions
    {
        public static Metadata AddIfNonExistent(Metadata metadata, Metadata.Entry entry)
        {
            if (metadata.Get(entry.Key) == null) metadata.Add(entry);
            return metadata;
        }
    }
}
