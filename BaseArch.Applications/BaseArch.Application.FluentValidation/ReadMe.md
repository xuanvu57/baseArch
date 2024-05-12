
# Default configuration

## Auto validation

It will call your fluent validation automatically by default when the request model is created
```
services.AddFluentValidationAutoValidation()
```

## Disable the filter that returns an BadRequestObjectResult when ModelState is invalid

This configuration allow to throw the **BaseArchValidationException** through **IValidatorInterceptor**
```
services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
```


