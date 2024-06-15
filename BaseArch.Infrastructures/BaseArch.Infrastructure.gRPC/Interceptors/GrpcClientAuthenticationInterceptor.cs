using BaseArch.Infrastructure.gRPC.Extensions;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;

namespace BaseArch.Infrastructure.gRPC.Interceptors
{
    internal class GrpcClientAuthenticationInterceptor(IHttpContextAccessor httpContextAccessor) : Interceptor
    {
        private const string DefaultScheme = "Bearer";

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            if (httpContextAccessor.HttpContext is null)
                return continuation(request, context);

            var authorizationValue = httpContextAccessor.HttpContext.Request.Headers.Authorization.FirstOrDefault() ?? "";
            if (!authorizationValue.StartsWith(DefaultScheme))
            {
                return continuation(request, context);
            }

            var token = authorizationValue.Replace($"{DefaultScheme} ", "");
            var metadata = new Metadata
            {
                { "Authorization", $"Bearer {token}" }
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
