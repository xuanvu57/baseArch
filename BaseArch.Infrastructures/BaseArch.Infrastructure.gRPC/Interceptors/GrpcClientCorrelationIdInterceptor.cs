using BaseArch.Application.CorrelationId;
using BaseArch.Application.CorrelationId.Interfaces;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Options;

namespace BaseArch.Infrastructure.gRPC.Interceptors
{
    public class GrpcClientCorrelationIdInterceptor(ICorrelationIdProvider correlationIdProvider, IOptions<CorrelationIdOptions> options) : Interceptor
    {
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var correlationId = correlationIdProvider.Get();

            var metadata = new Metadata
                {
                    { options.Value.RequestHeader, correlationId }
                };

            var newContext = new ClientInterceptorContext<TRequest, TResponse>(
            context.Method,
            context.Host,
            context.Options.WithHeaders(
                (context.Options.Headers ?? []).Aggregate(
                    metadata,
                    AddIfNonExistent))
                );

            return continuation(request, newContext);
        }

        private static Metadata AddIfNonExistent(Metadata metadata, Metadata.Entry entry)
        {
            if (metadata.Get(entry.Key) == null) metadata.Add(entry);
            return metadata;
        }
    }
}
