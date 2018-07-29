using System;
using System.Linq.Expressions;

namespace Bubbio.Core.Helpers
{
    public static class ExpressionHelper<TFrom, TTo>
    {
        public static Expression<Func<TTo, bool>> Transform(Expression<Func<TFrom, bool>> predicate)
        {
            var parameter = Expression.Parameter(typeof(TTo));
            var body = new Visitor(parameter).Visit(predicate.Body);
            return Expression.Lambda<Func<TTo, bool>>(body ?? throw new InvalidOperationException(), parameter);
        }

        public static Expression<Func<TTo, T>> Transform<T>(Expression<Func<TFrom, T>> expr)
        {
            var parameter = Expression.Parameter(typeof(TTo));
            var body = new Visitor(parameter).Visit(expr.Body);
            return Expression.Lambda<Func<TTo, T>>(body ?? throw new InvalidOperationException(), parameter);
        }

        private class Visitor : ExpressionVisitor
        {
            private readonly ParameterExpression _parameter;

            public Visitor(ParameterExpression parameter)
            {
                _parameter = parameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return _parameter;
            }
        }
    }
}