using System;
using System.Linq.Expressions;

namespace TestApp
{
    public class Specification<T> : BaseSpecification
    {
        public static bool operator false(Specification<T> specification) => false;

        public static bool operator true(Specification<T> specification) => false;

        public static Specification<T> operator &(Specification<T> spec1, Specification<T> spec2)
            => new Specification<T>(spec1._expression.And(spec2._expression));

        public static Specification<T> operator |(Specification<T> spec1, Specification<T> spec2)
            => new Specification<T>(spec1._expression.Or(spec2._expression));

        public static Specification<T> operator !(Specification<T> specification)
            => new Specification<T>(specification._expression.Not());

        public static implicit operator Expression<Func<T, bool>>(Specification<T> specification)
            => specification._expression;

        public static implicit operator Expression(Specification<T> specification)
            => specification._expression;

        public static implicit operator Specification<T>(Expression<Func<T, bool>> expression)
            => new Specification<T>(expression);

        private readonly Expression<Func<T, bool>> _expression;

        public Specification(Expression<Func<T, bool>> expression)
        {
            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        public bool IsSatisfiedBy(T obj) => _expression.AsFunc()(obj);

        public Specification<TParent> From<TParent>(Expression<Func<TParent, T>> mapFrom)
            => _expression.From(mapFrom);

        public override Expression ToExpression()
        {
            return _expression;
        }
    }
}