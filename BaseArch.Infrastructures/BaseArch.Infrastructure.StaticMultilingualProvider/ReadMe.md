# Multilingual provider from static resource

1. AbstractMultilingualProvider

This is an abstract class to implement the default method

It uses `IStringLocalizer` to get a string from the .resx files

Your provider need to inherit this abstract class and provide corresponding `YourMessage` class

```
[DIService(DIServiceLifetime.Singleton)]
public class SampleMultilingualProvider(IStringLocalizer<YourMessages> localizer) : AbstractMultilingualProvider<YourMessages>(localizer), ISampleMultilingualProvider
```

***Note***

> - The `YourMessages` is only an empty class without any implementation
>	
> - And your `*.resx` files must follow this naming convention with `YourMessages` classs
>
> Example: YourMessage.en-US.resx, YourMessage.vi-VN-resx

2. Service registration

You need to register the necessary service to use the `IStringLocalizer` for your provider

```
services.AddStaticMultilingualProviders(["en-US", "vi-VN"]);
```

3. Middleware registration

You need to add `RequestLocalizationMiddlware` to automatically set culture information for request base on the Request's header

> Accept-Language

This middleware can be added with the extention method

```
app.UseStaticMultilingualProviders();
```

Or you can add direcly

```
app.UseRequestLocalization();
```