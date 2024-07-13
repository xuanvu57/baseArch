using BaseArch.Application.Identity.Interfaces;
using BaseArch.Infrastructure.gRPC.Extensions;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace BaseArch.Infrastructure.gRPC.Interceptors
{
    /// <summary>
    /// Interceptor to add authenticated token into request
    /// </summary>
    /// <param name="tokenProvider"></param>
    internal class GrpcClientAuthenticationInterceptor(ITokenProvider tokenProvider) : Interceptor
    {
        /// <summary>
        /// Authentication metadata key
        /// </summary>
        private const string _authorizationMetadataKey = "Authorization";

        /// <inheritdoc/>
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var newContext = AddAuthorizationToHeader(context);

            return continuation(request, newContext);
        }

        /// <summary>
        /// Add authorization token to header
        /// </summary>
        /// <typeparam name="TRequest">Type of request</typeparam>
        /// <typeparam name="TResponse">Type of response</typeparam>
        /// <param name="context"><see cref="ClientInterceptorContext{TRequest, TResponse}"/></param>
        /// <returns><see cref="ClientInterceptorContext{TRequest, TResponse}"/></returns>
        private ClientInterceptorContext<TRequest, TResponse> AddAuthorizationToHeader<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context)
            where TRequest : class
            where TResponse : class
        {
            if (context.Options.Headers is not null && context.Options.Headers.Any(x => x.Key == _authorizationMetadataKey))
                return context;

            var token = tokenProvider.GetAccessToken();

            if (string.IsNullOrEmpty(token))
                return context;

            var metadata = new Metadata
            {
                { _authorizationMetadataKey, $"{tokenProvider.DefaultScheme} {token}" }
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
