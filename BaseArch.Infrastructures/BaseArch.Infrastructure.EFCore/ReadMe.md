
# BaseRepository

This is the generic repository, which is implemented from `IBaseRepository`, to implement the common methods for any repository

> The common methods was defined in `IBaseRepository`

It require 3 types

- `TEntity`: this is type of the target entity
- `TKey`: this is the type of key property in the target entity
- `TUserKey`: this is the type of user id, it's used for implement the feature of permission

Your repository can inherit from this generic repository as the following example

```
public record UserEntity(string FullName): BaseEntity<Guid, Guid>;

public interface IUserRepository: IBaseRepository<UserEntity, Guid>
{
}

public sealed class UserRepository(SampleDBContext dbContext) : BaseRepository<UserEntity, Guid, Guid>(dbContext), IUserRepository
{
}
```

# UnitOfWork

It provide a abstract class of unit of work, which is implemented from `IUnitOfWork`, to manage the repositories

It handles the creating repository with repository pool to ensure there is no duplicated repositories

> The common methods was defined in `IUnitOfWork`

You can create and access to your repository as the following examples

```
class CreateUserService(IUnitOfWork unitOfWork) : ICreateUserService
{
    // You have your defined UserRepository to implement the specific methods
    private readonly IUserRepository userRepository = unitOfWork.GetRepository<IUserRepository>();

    // You only use common methods and you don't need to define your repository
    private readonly IBaseRepository<UserEntity, Guid> genericUserRepository = unitOfWork.GetVirtualRepository<UserEntity, Guid, Guid>();
}
```

It also handle to make sure that all repositories will be updated into database within a transaction

```
unitOfWork.SaveChangesAndCommit()
```

# IQueryable extension

It provides some extension methods to be more flexible when work with your model directly

1. Order

```
IQueryable queryable =  dbContext.Set<TEntity>().AsQueryable();

queryable.CustomizedOrderBy("Name");
// wil be the same to 
// queryable.OrderBy(e => e.Name);

queryable.CustomizedOrderByDescending("Name");
// wil be the same to 
// queryable.OrderByDescending(e => e.Name);

queryable.CustomizedOrderBy("Name").CustomizedThenBy("Age");
// wil be the same to 
// queryable.OrderBy(e => e.Name).ThenBy(e => e.Age);

queryable.CustomizedOrderBy("Name").CustomizedThenByDescending("Age");
// wil be the same to 
// queryable.OrderBy(e => e.Name).ThenByDescending(e => e.Age);
```

2. Where

```
IQueryable queryable =  dbContext.Set<TEntity>().AsQueryable();

queryable.GenerateORFilterExpression(["FirstName", "LastName"], "rain");
// wil be the same to
// queryable.Where(e => EF.Functions.Like(e.FirstName, "rain") OR EF.Functions.Like(e.LastName, "rain"));

var filter = new List<FilterQueryModel>() 
{
    new FilterQueryModel("rain", "FullName"),
    new FilterQueryModel("VietNam", "Nation")
};
queryable.GenerateANDFilterExpression(filter);
// wil be the same to
// queryable.Where(e => EF.Functions.Like(e.FullName, "rain") AND EF.Functions.Like(e.Nation, "VietNam"));
```