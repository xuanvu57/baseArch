using BaseArch.Application.Models.Requests;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BaseArch.Infrastructure.EFCore.Extensions
{
    /// <summary>
    /// The extension methods for IQueryable
    /// </summary>
    internal static class IQueryableExtensions
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
            Expression? finalExpression = null;
            var parameterExpression = Expression.Parameter(typeof(TEntity), "e");
            var valueExpression = Expression.Constant($"%{value}%");

            foreach (var propertyName in properties)
            {
                var nameProperty = Expression.Property(parameterExpression, propertyName);

                var executedExpression = Expression.Call(typeof(DbFunctionsExtensions), nameof(DbFunctionsExtensions.Like), null,
                    Expression.Constant(EF.Functions), nameProperty, valueExpression);

                finalExpression = finalExpression == null ? executedExpression : Expression.OrElse(finalExpression, executedExpression);
            }

            var expression = Expression.Lambda<Func<TEntity, bool>>(finalExpression!, parameterExpression);

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
            Expression? finalExpression = null;
            var parameterExpression = Expression.Parameter(typeof(TEntity), "e");

            foreach (var filter in filters)
            {
                var nameProperty = Expression.Property(parameterExpression, filter.FieldName);
                var valueExpression = Expression.Constant($"%{filter.SearchText}%");

                var executedExpression = Expression.Call(typeof(DbFunctionsExtensions), nameof(DbFunctionsExtensions.Like), null,
                    Expression.Constant(EF.Functions), nameProperty, valueExpression);

                finalExpression = finalExpression == null ? executedExpression : Expression.AndAlso(finalExpression, executedExpression);
            }

            var expression = Expression.Lambda<Func<TEntity, bool>>(finalExpression!, parameterExpression);

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
            var props = property.Split('.');
            var type = typeof(TEntity);
            var arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (var prop in props)
            {
                var pi = type.GetProperty(prop);
                if (pi != null)
                {
                    expr = Expression.Property(expr, pi);
                    type = pi.PropertyType;
                }
            }
            var delegateType = typeof(Func<,>).MakeGenericType(typeof(TEntity), type);
            var lambda = Expression.Lambda(delegateType, expr, arg);

            var result = typeof(Queryable).GetMethods().Single(
                method => method.Name == methodName
                && method.IsGenericMethodDefinition
                && method.GetGenericArguments().Length == 2
                && method.GetParameters().Length == 2).MakeGenericMethod(typeof(TEntity), type).Invoke(null, [source, lambda]);

            if (result == null)
                return source.Order();

            return (IOrderedQueryable<TEntity>)result;
        }
    }
}
