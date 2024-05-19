# Why should gRPC at the Infrastructure layer?

- **Infrastructure Concern**: gRPC is a communication framework that handles communication with external systems or services.
- **Dependency Management**: This arrangement keeps your Application layer independent of external framewrok (gRPC)
- **Future changes**: It is easier to swap out or modify the communication mechanism without impacting the layer, such as Presentation layer

# How to register services

You is able to use the default extension methods to register and map your gRPC services automatically

```
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpcServices();

// other code ...

var app = builder.Build();
app.MapControllers();
app.AutoMapGprcServices();
```

***Note***

As default, the `HttpRequestResponseLoggingMiddleware` will ignore the request with Content-Type is ***application/grpc***

If you would like to log the request and response from grpc services, you can add the provided interceptor `GrpcRequestResponseLoggingInterceptor`

```
builder.Services.AddGrpcServices(option =>
{
    option.Interceptors.Add<GrpcRequestResponseLoggingInterceptor>();
});
```

You can add your interceptors at this time also

# Fundamental configurations

## 1. Service
For mapping gRPC services automatically, you are required to use `[GrpcService]` attribute

```
[GrpcService]
public class GreetingService(ILogger<GreetingService> logger) : Greeter.GreeterBase
{
}
```

## 2. Client
Its purpose is to make a call to the gRPC server so you try to keep it as simplest as posible

- Inherit from `BaseGrpcClient` class to leverage the provided methods
- Implement the interface from your Application layer with appropriate scope

```
[DIService(DIServiceLifetime.Scoped)]
public class GreetingClient(IConfiguration configuration) : BaseGrpcClient(configuration), IGreetingClient
{
}
```

### Tip
It is the best practice if you create your base client and set the configurations only once

Then your client classes are able to reuse these configurations without duplication and changes to mistake

## 3. AppSettings.json
You can define your gRPC server urls from appsetings[.Environment].json

Then you can provide key of the url to create the corresponding channel

### References
> https://grpc.io/docs/what-is-grpc/introduction/
>
>https://github.com/grpc/grpc/blob/master/src/csharp/BUILD-INTEGRATION.md