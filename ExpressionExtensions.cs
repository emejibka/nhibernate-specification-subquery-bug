using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace TestApp
{
    public static class ExpressionExtensions
    {
        private static readonly ConcurrentDictionary<string, object> Cache
            = new ConcurrentDictionary<string, object>();

        public static Func<TIn, TOut> AsFunc<TIn, TOut>(this Expression<Func<TIn, TOut>> expr)
            => CompiledExpressions<TIn, TOut>.AsFunc(expr);

        public static bool Is<T>(this T entity, Expression<Func<T, bool>> expr)
            => AsFunc(expr).Invoke(entity);

        public static Expression<Func<TDestination, TReturn>> From<TSource, TDestination, TReturn>(
            this Expression<Func<TSource, TReturn>> source, Expression<Func<TDestination, TSource>> mapFrom)
            => Expression.Lambda<Func<TDestination, TReturn>>(
                Expression.Invoke(source, mapFrom.Body), mapFrom.Parameters);
    }
}