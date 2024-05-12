
# Write a structured log

## Log with property, it will create the log's property
```
record RequestLog(string Method, string Path);

var request = RequestLog("POST", "api/Users/Create");
logger.LogInformation("Request from client is {RequestPath}", request.Path);

// It will create the log and allow to filter with property
// fields.RequestPath = "api/Users/Create"
```

***Note:***
> **Do not** log with the string interpolation because it will not create the property is unable to filter your logs
```
// Do not
logger.LogInformation($"Request from client is {Request.Path}");
```

## Log with object, it will create log's fields as the Class or Record's properties
```
record RequestLog(string Method, string Path);

var request = RequestLog("POST", "api/Users/Create");
logger.LogInformation("Request from client is {@HttpRequest}", Request);

// It will create the log and allow to filter with the fields
// fields.HttpRequest.Method = "POST"
// fields.HttpRequest.Path = "api/Users/Create"
```

### Reference
> https://github.com/serilog/serilog/wiki/Structured-Data

# Sensitive data

When you add the ***HttpRequestResponseLoggingMiddleware*** to log data from HttpRequest and HttpResponse, it may contain the sensitive data and you need to mask, remove or tokenize them within your logs

It provide a default destructing policy to masked your sensitive data during destructing objects

You can also implement your own destructing policy to adapt your business
```
public class YourSensitiveDataDestructuringPolicy : IDestructuringPolicy
```

***Note***
> - The destructing policy will only be executed with
```
// It is ok for masking
logger.LogInformation("Request from client is {@HttpRequest}", Request);

// It will not be masked
logger.LogInformation("Request from client is {RequestMethod}", Request.Method);
```

> - The destructing policy only masks for properties from destructing objects
> - It is not available for the [Simple, Scalar Values](https://github.com/serilog/serilog/wiki/Structured-Data)
> - It is not available for the Enrichers, such as: LogContext, Environment

### To register the default destructing policy

```
builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
    loggerConfiguration.Destructure.With(new SensitiveDataDestructuringPolicy(opt =>
    {
        opt.MaskValue = "xxxx";
        opt.Keywords = ["property"];
    }));
});
```

***Note***

### Reference
> https://betterstack.com/community/guides/logging/sensitive-data/
> https://dev.to/auvansangit/prevent-sensitive-data-exposure-in-log-with-serilog-1pk7
> https://github.com/dotnet/extensions/tree/main/src/Libraries/Microsoft.Extensions.Compliance.Redaction
> https://github.com/serilog-contrib/Serilog.Enrichers.Sensitive