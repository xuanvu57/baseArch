
## Support mapping for both Class and Record

***Problem***: the default `ForMember()` method will not work and we get the error "___No available constructor___"

***Reason***: because `Record` (and `Class` with primary constructor) does not use parameterless constructor

This library support an extension method to allow map

```
mappingExpression.ForMember()
```

Reference
> https://christosmonogios.com/2023/04/06/Map-One-C-Sharp-Record-To-Another-With-AutoMapper/


