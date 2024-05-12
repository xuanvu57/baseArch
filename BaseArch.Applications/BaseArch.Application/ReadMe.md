
## Correlation id

1. GuidCorrelationIdProvider

It provides a default correlation id provider to generate correlation id as Guid

You can customize your correlation id provider by implement interface ___ICorrelationIdProvider___

```
public class YourCorrelationIdProvider : ICorrelationIdProvider
```

2. Register the correlation services

You use the extension method to add the necessary services

```
builder.Services.AddCorrelationIdServices<GuidCorrelationIdProvider>();
```

You are able to customize the header name of correlation id for Request and Response with ___CorrelationIdOptions___ 
```
builder.Services.AddCorrelationIdServices<GuidCorrelationIdProvider>(opt =>
{
    opt.RequestHeader = "X-Correlation-Id";
    opt.ResponseHeader = "X-Correlation-Id";
});
```

3. CorrelationIdMiddleware

It requires you to add the ***CorrelationIdMiddleware*** to the pipeline

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

You need to add the ***CustomizedSerilogLogContextMiddleware*** to the pipeline

- This middleware will push the ***correlation id*** to Serilog LogContext at with default property: ___CorrelationId___
```
app.UseCorrelationIdMiddleware();
app.UseCustomizedSerilogLogContextMiddleware();
```

***Note***
>To make sure that the correlation id is always attached to the Serilog LogContext before writing log, this middleware should be added at the top of the pipeline right after the ***CorrelationIdMiddleware***

## IExceptionHandler

1. BusinessExceptionHandler

It provides a default business exception handler to handle the excpetion from your business logic code

You can customize one or many business exception to handle your business logic by implement the interface ___IExceptionHandler___

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


These extension methods also add the ProblemDetails response to standardlize your response when problem occurs

```
services.AddProblemDetails();
```

***3. Prerequisites***

It requires to add the ExceptionHandler middle to the pipeline

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