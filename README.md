# baseArch
Base Architecture and Sample projects

### Application
- Tracing log with `Correlation id` and `Guid correlation id provider` build-in
- Mapping objects with `AutoMapper` library
- Auto validation and customized validation with `FluentValidation` library

### Infrastructure
- Auto scan and inject dependancies with `Scrutor` library
- Apply repository and Unit of work with `Entity Framework` library
- Communicate between modules with `HttpClient` and `gRPC` library
- Send or publish message via queue by `MassTransit` library with `InMemoryQueye` and `RabbitMQ`
- Structure log with `Serilog` library
- Multiligual by static resource with `StaticMultiligualProvider` build-in

### Presentation
- Add the RestApi with `Swagger`, `CorrelationIdMiddleware`, `HttpRequestResponseLoggingMiddleware`
