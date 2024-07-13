## Response
You are always able to structure your response as a standard format by

```
Responses.From<TResponse>(TResponse yourResponseData);
```

You are also able to add the Pagination model to the response

```
Responses.From<TResponse>(TResponse yourResponseData, PaginationResponseModel pagination);
```

Note:

> If some properties are null then they will be ignored in Json when the response return to client

## Correlation id

1. GuidCorrelationIdProvider

It provides a default correlation id provider to generate correlation id as Guid

You can customize your correlation id provider by implement interface `ICorrelationIdProvider`

```
public class YourCorrelationIdProvider : ICorrelationIdProvider
```

2. Register the correlation services

You use the extension method to add the necessary services

```
builder.Services.AddCorrelationIdServices<GuidCorrelationIdProvider>();
```

You are able to customize the header name of correlation id for Request and Response with `CorrelationIdOptions`
```
builder.Services.AddCorrelationIdServices<GuidCorrelationIdProvider>(opt =>
{
    opt.RequestHeader = "X-Correlation-Id";
    opt.ResponseHeader = "X-Correlation-Id";
});
```

3. CorrelationIdMiddleware

It requires you to add the `CorrelationIdMiddleware` to the pipeline

- This middleware will firstly check the Request's header to get the existing correlation id
- Then it will generate new correlation id by the registered provider and append to Request's header if the correlation id was not found
- Lastly, it will add the correlation id to the Response's header

```
var app = builder.Build();
app.UseCorrelationIdMiddleware();
```

***Note:***

>To make sure that the correlation id always exists during a request's lifetime, this middlware should be added at the top of the pipeline

4. Attach correlation id to Serilog LogContext

You need to add the `CustomizedSerilogLogContextMiddleware` to the pipeline

- This middleware will push the ***correlation id*** to Serilog LogContext at with default property: ___CorrelationId___

```
app.UseCorrelationIdMiddleware();
app.UseCustomizedSerilogLogContextMiddleware();
```

***Note***

>To make sure that the correlation id is always attached to the Serilog LogContext before writing log, this middleware should be added at the top of the pipeline right after the `CorrelationIdMiddleware`

## IExceptionHandler

1. BusinessExceptionHandler

It provides a default business exception handler to handle the excpetion from your business logic code

You can customize one or many business exception to handle your business logic by implement the interface `IExceptionHandler`

```
public class YourBusinessExceptionHandler: IExceptionHandler
```

2. Register the business exception handler

You use the extension method to add the necessary service

Currently, it supports maximum of 3 business exception handlers

```
services.AddBusinessExceptionHandler<BusinessExceptionHandler>();
```

```
services.AddBusinessExceptionHandler<BusinessExceptionHandler, YourBusinessExceptionHandler>();
```

These extension methods also add the `ProblemDetails` response to standardlize your response when problem occurs

```
services.AddProblemDetails();
```

***3. Prerequisites***

It requires to use Exception handler middleware to the pipeline

```
app.UseExceptionHandler();
```

## Global exception handler

You can add the global exception handler middleware to catch and log for any unhandlled exception in your application

```
app.UserGlobalExceptionHandlingMiddlewareRegistration();
```

The exception will be logged as the following template so that it can be filtered by ___UnhandleExceptionMessage___ field
```
logger.LogError(exception, "Unhandle exception occurred: {UnhandleExceptionMessage}", exception.Message);
```

## Logging model

This define the log's structure and it will be used in middlewares or interceptor for logging the `Request` and `Response` automatically

It also define the log message template for specific cases, such as:

- `HttpRequestResponseLoggingMiddleware`
- `HttpClientLoggingDelegatingHandler`
- `GrpcServiceLoggingInterceptor`
- `GrpcClientLoggingInterceptor`

## Repository and Unit of Work

Define the interfaces for `Base respository` and `Unit of work`

They will be implemented from the Repository and Unit of work in the infrastructure projects

## Message queue

You message to add one of our queue package, such as:

- BaseArch.Infrastructure.MassTransit

It build an automatic configuration to register and consume the event message which will be published or sent from your producer

Create your message that inherits from `BaseEventMessage`

```
public record YourMessage(string MessageData) : BaseEventMessage<Guid>(Guid.NewGuid());
```

Then you inject `IPublisher` or `ISender` to publish or send your message

Finally, you create the handlers to handle the message without creating any consumer

```
[DIService(DIServiceLifetime.Scoped)]
public class YourMessageHandler() : IEventMessageHandler<YourMessage>
```

### Customize your consumers

In some special cases, you would like to do a specific action or modify your consumer, you can create your own consumer instead of the auto consumers

First of all, your message is implemented from `IEventMessage`

```
public record UserCreatedCustomizeMessage(Guid MessageId) : IEventMessage<Guid>;
```

Then, your consumer is inherited from `DefaultConsumer`

- For example 1: you would like to create a batch consumer with MassTransit

```
public class YourBatchConsumer(IEventMessageHandler<YourMessage> messageHandler) : DefaultConsumer<Batch<YourMessage>>
```

- For example 2: you would like to create your consumer definition with MassTransit

```
public class YourCustomizedConsumerDefinition : ConsumerDefinition<YourCustomizedConsumer>
```
 
- For example 3: you simply would like to create "real" multi consumers (in the same host) to consume the same message (instead of calling multi handlers from one auto consumer)

```
To be updated
```

Reference
> https://masstransit.io/documentation/concepts/consumers#batch-consumers
> https://masstransit.io/documentation/concepts/consumers#definitions

## Encryption

You are able to use `IEncryptionProvider` interface to inject to your project

It also provide (some) class to implment the `IEncryptionProvider` interface, such as

- `AesEncryptionProvider` which implement a simple AES algorithm

Note: 
> You can use `IEnumerable<IEncryptionProvider>` then select your expected algorithm

## Identity

Define the interfaces for `SSO provider` and `Token provider`

`IIdentityUser` will define a standard for Identity user model