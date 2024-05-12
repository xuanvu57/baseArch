using AutoMapper;
using System.Linq.Expressions;
using System.Reflection;

namespace BaseArch.Application.AutoMapper.Extensions
{
    /// <summary>
    /// Extension methods for AutoMapper library
    /// </summary>
    public static class AutoMapperExtensions
    {
        /// <summary>
        /// Extension for <see cref="IMappingExpression"/> to allow mapping both Record and Class
        /// </summary>
        /// <typeparam name="TSource">Type of source object</typeparam>
        /// <typeparam name="TDestination">Type of destination object</typeparam>
        /// <typeparam name="TResult">Type of result</typeparam>
        /// <typeparam name="TMember">Type of member</typeparam>
        /// <param name="mappingExpression"><see cref="IMappingExpression"/></param>
        /// <param name="destinationExpression">Expression for destination</param>
        /// <param name="sourceExpression">Expression for source</param>
        /// <returns></returns>
        public static IMappingExpression<TSource, TDestination> ForMember<TSource, TDestination, TResult, TMember>(
            this IMappingExpression<TSource, TDestination> mappingExpression,
            Expression<Func<TDestination, TMember>> destinationExpression,
            Expression<Func<TSource, TResult>> sourceExpression)
        {
            var memberInfo = GetMemberInfo(destinationExpression);
            var memberName = memberInfo.Name;
            return mappingExpression
                .ForMember(destinationExpression, opt => opt.MapFrom(sourceExpression))
                .ForCtorParam(memberName, opt => opt.MapFrom(sourceExpression));
        }

        /// <summary>
        /// Get member information by expression
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <typeparam name="TMember">Type of member</typeparam>
        /// <param name="lambda"></param>
        /// <returns></returns>
        private static PropertyInfo GetMemberInfo<T, TMember>(Expression<Func<T, TMember>> lambda)
        {
            var memberExpression = lambda.Body is UnaryExpression expression
                  ? (MemberExpression)expression.Operand
                  : (MemberExpression)lambda.Body;

            return (PropertyInfo)memberExpression.Member;

        }
    }
}
