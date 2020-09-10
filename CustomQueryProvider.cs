using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Linq;

namespace TestApp
{
    //https://stackoverflow.com/questions/56134101/custom-linqtohqlgeneratorsregistry-invalidcastexception-unable-cast-antlr-r
    public class CustomQueryProvider : DefaultQueryProvider, IQueryProviderWithOptions
    {
        public CustomQueryProvider(ISessionImplementor session) : base(session) { }
        public CustomQueryProvider(ISessionImplementor session, object collection) : base(session, collection) { }

        protected override NhLinqExpression PrepareQuery(Expression expression, out IQuery query) => base.PrepareQuery(expression.ApplySpecifications(), out query);

        // Hacks for correctly supporting IQueryProviderWithOptions
        IQueryProvider IQueryProviderWithOptions.WithOptions(Action<NhQueryableOptions> setOptions)
        {
            if (setOptions == null)
                throw new ArgumentNullException(nameof(setOptions));

            var options = (NhQueryableOptions)_options.GetValue(this);
            var newOptions = options != null ? (NhQueryableOptions)_cloneOptions.Invoke(options, null) : new NhQueryableOptions();
            setOptions(newOptions);
            var clone = (CustomQueryProvider)this.MemberwiseClone();
            _options.SetValue(clone, newOptions);
            return clone;
        }

        public override IQueryable CreateQuery(Expression expression)
        {
            return base.CreateQuery(expression.ApplySpecifications());
        }

        public override IQueryable<T> CreateQuery<T>(Expression expression)
        {
            return base.CreateQuery<T>(expression.ApplySpecifications());
        }

        private static readonly FieldInfo _options = typeof(DefaultQueryProvider).GetField("_options", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly MethodInfo _cloneOptions = typeof(NhQueryableOptions).GetMethod("Clone", BindingFlags.NonPublic | BindingFlags.Instance);
    }
}