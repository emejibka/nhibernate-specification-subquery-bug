using System.Linq.Expressions;

namespace TestApp
{
    public abstract class BaseSpecification
    {
        public abstract Expression ToExpression();
    }
}