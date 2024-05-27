using BaseArch.Domain.BaseArchModels.Requests;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace BaseArch.Infrastructure.EFCore.Extensions
{
    /// <summary>
    /// The extension methods for IQueryable
    /// </summary>
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Create sort query for a property by ascending
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="source">Soure queryable</param>
        /// <param name="property">Property to sort</param>
        /// <returns><see cref="IOrderedQueryable"/></returns>
        public static IOrderedQueryable<TEntity> CustomizedOrderBy<TEntity>(this IQueryable<TEntity> source, string property)
        {
            return ApplyOrder(source, property, "OrderBy");
        }

        /// <summary>
        /// Create sort query for a property by Descending
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="source">Soure queryable</param>
        /// <param name="property">Property to sort</param>
        /// <returns><see cref="IOrderedQueryable"/></returns>
        public static IOrderedQueryable<TEntity> CustomizedOrderByDescending<TEntity>(this IQueryable<TEntity> source, string property)
        {
            return ApplyOrder(source, property, "OrderByDescending");
        }

        /// <summary>
        /// Create sort query for a property by ascending after sorting of another property
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="source">Soure ordered queryable</param>
        /// <param name="property">Property to sort</param>
        /// <returns><see cref="IOrderedQueryable"/></returns>
        public static IOrderedQueryable<TEntity> CustomizedThenBy<TEntity>(this IOrderedQueryable<TEntity> source, string property)
        {
            return ApplyOrder(source, property, "ThenBy");
        }

        /// <summary>
        /// Create sort query for a property by descending after sorting of another property
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="source">Soure ordered queryable</param>
        /// <param name="property">Property to sort</param>
        /// <returns><see cref="IOrderedQueryable"/></returns>
        public static IOrderedQueryable<TEntity> CustomizedThenByDescending<TEntity>(this IOrderedQueryable<TEntity> source, string property)
        {
            return ApplyOrder(source, property, "ThenByDescending");
        }

        /// <summary>
        /// Generate the function expression for filter (LIKE) with OR condition
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="source">Source queryable</param>
        /// <param name="properties">List of properties to filter</param>
        /// <param name="value">Value to search</param>
        /// <returns><see cref="IQueryable"/></returns>
        public static IQueryable<TEntity> GenerateORFilterExpression<TEntity>(this IQueryable<TEntity> source, IEnumerable<string> properties, object value)
        {
            Expression finalExpression = Expression.Empty();
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TEntity), "e");
            ConstantExpression valueExpression = Expression.Constant($"%{value}%");

            foreach (string propertyName in properties)
            {
                Expression nameProperty = Expression.Property(parameterExpression, propertyName);

                Expression e1 = Expression.Call(typeof(DbFunctionsExtensions), nameof(DbFunctionsExtensions.Like), null,
                    Expression.Constant(EF.Functions), nameProperty, valueExpression);

                finalExpression = Expression.OrElse(finalExpression, e1);
            }

            Expression<Func<TEntity, bool>> expression = Expression.Lambda<Func<TEntity, bool>>(finalExpression, parameterExpression);

            return source.Where(expression);
        }

        /// <summary>
        /// Generate the function expression for filter (LIKE) with AND condition
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="source">Source queryable</param>
        /// <param name="filters"><see cref="FilterQueryModel"/></param>
        /// <returns><see cref="IQueryable"/></returns>
        public static IQueryable<TEntity> GenerateANDFilterExpression<TEntity>(this IQueryable<TEntity> source, IEnumerable<FilterQueryModel> filters)
        {
            Expression finalExpression = Expression.Empty();
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TEntity), "e");

            foreach (FilterQueryModel filter in filters)
            {
                Expression nameProperty = Expression.Property(parameterExpression, filter.FieldName);
                ConstantExpression valueExpression = Expression.Constant($"%{filter.SearchText}%");

                Expression e1 = Expression.Call(typeof(DbFunctionsExtensions), nameof(DbFunctionsExtensions.Like), null,
                    Expression.Constant(EF.Functions), nameProperty, valueExpression);

                finalExpression = Expression.AndAlso(finalExpression, e1);
            }

            Expression<Func<TEntity, bool>> expression = Expression.Lambda<Func<TEntity, bool>>(finalExpression, parameterExpression);

            return source.Where(expression);
        }

        /// <summary>
        /// Generate the lambda expression for sorted query
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="source">Source queriable</param>
        /// <param name="property">Property to sort</param>
        /// <param name="methodName">Method name to express the lambda</param>
        /// <returns><see cref="IOrderedQueryable"/></returns>
        private static IOrderedQueryable<TEntity> ApplyOrder<TEntity>(IQueryable<TEntity> source, string property, string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(TEntity);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                PropertyInfo? pi = type.GetProperty(prop);
                if (pi != null)
                {
                    expr = Expression.Property(expr, pi);
                    type = pi.PropertyType;
                }
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(TEntity), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object? result = typeof(Queryable).GetMethods().Single(
                method => method.Name == methodName
                && method.IsGenericMethodDefinition
                && method.GetGenericArguments().Length == 2
                && method.GetParameters().Length == 2).MakeGenericMethod(typeof(TEntity), type).Invoke(null, new object[] { source, lambda });

            if (result == null)
                return source.Order();

            return (IOrderedQueryable<TEntity>)result;
        }
    }
}
