using BaseArch.Application.CorrelationId;
using BaseArch.Application.CorrelationId.Interfaces;
using BaseArch.Infrastructure.gRPC.Extensions;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Options;

namespace BaseArch.Infrastructure.gRPC.Interceptors
{
    /// <summary>
    /// Interceptor to add correlation id into request
    /// </summary>
    /// <param name="correlationIdProvider"><see cref="ICorrelationIdProvider"/></param>
    /// <param name="options"><see cref="CorrelationIdOptions"/></param>
    public class GrpcClientCorrelationIdInterceptor(ICorrelationIdProvider correlationIdProvider, IOptions<CorrelationIdOptions> options) : Interceptor
    {
        /// <inheritdoc/>
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
                    InterceptorExtensions.AddIfNonExistent))
                );

            return continuation(request, newContext);
        }
    }
}
