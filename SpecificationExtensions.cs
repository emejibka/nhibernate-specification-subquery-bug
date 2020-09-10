using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TestApp
{
    public static class SpecificationExtensions
    {
        public static Expression ApplySpecifications(this Expression source) => new SpecificationsProcessor().Visit(source);

        class SpecificationsProcessor : ExpressionVisitor
        {
            protected override Expression VisitMember(MemberExpression node)
            {
                if (node.Expression != null && node.Member is PropertyInfo property)
                {
                    if (property.GetCustomAttributes(typeof(SpecificationAttribute), false).SingleOrDefault() is SpecificationAttribute info)
                    {
                        var type = property.DeclaringType;
                        var specificationMemberInfo = type.GetFields(BindingFlags.Static | BindingFlags.Public)
                                                          .Single(x => x.Name == info.FieldName);
                        var specification = (BaseSpecification)specificationMemberInfo.GetValue(null);
                        var specificationExpression = (LambdaExpression)specification.ToExpression();
                        var expression = specificationExpression.Body.ReplaceParameter(specificationExpression.Parameters.Single(), Visit(node.Expression));
                        return Visit(expression);
                    }
                }
                return base.VisitMember(node);
            }
        }

        public static Expression ReplaceParameter(this Expression source, ParameterExpression from, Expression to)
            => new ParameterReplacer { From = from, To = to }.Visit(source);

        class ParameterReplacer : ExpressionVisitor
        {
            public ParameterExpression From;
            public Expression To;
            protected override Expression VisitParameter(ParameterExpression node) => node == From ? To : base.VisitParameter(node);
        }
    }
}