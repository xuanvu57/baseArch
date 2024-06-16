using BaseArch.Application.Identity.Interfaces;
using BaseArch.Infrastructure.gRPC.Extensions;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace BaseArch.Infrastructure.gRPC.Interceptors
{
    internal class GrpcClientAuthenticationInterceptor(ITokenProvider tokenProvider) : Interceptor
    {
        private const string AuthorizationMetadataKey = "Authorization";

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var newContext = AddAuthorizationToHeader(context);

            return continuation(request, newContext);
        }

        private ClientInterceptorContext<TRequest, TResponse> AddAuthorizationToHeader<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context)
            where TRequest : class
            where TResponse : class
        {
            if (context.Options.Headers is not null && context.Options.Headers.Any(x => x.Key == AuthorizationMetadataKey))
                return context;

            var token = tokenProvider.GetAccessToken();

            if (string.IsNullOrEmpty(token))
                return context;

            var metadata = new Metadata
            {
                { AuthorizationMetadataKey, $"{tokenProvider.DefaultScheme} {token}" }
            };

            var newContext = new ClientInterceptorContext<TRequest, TResponse>(
            context.Method,
            context.Host,
            context.Options.WithHeaders(
                (context.Options.Headers ?? []).Aggregate(
                    metadata,
                    InterceptorExtensions.AddIfNonExistent))
                );

            return newContext;
        }
    }
}
