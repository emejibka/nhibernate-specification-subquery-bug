using System;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace TestApp
{
    [TestFixture]
    public class TestClass
    {
        private static Configuration _config;

        [Test]
        public void test()
        {
            var sessionFactory = CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                var schemaExport = new SchemaExport(_config);
                schemaExport.Create(false, true, session.Connection);


                Console.WriteLine("specification test".PadLeft(80, '-'));
                Console.WriteLine();
                var clients = session.Query<Client>().Where(x => x.IsAdult).ToList(); //ok
                Console.WriteLine("specification test passed".PadLeft(80, '-'));
                Console.WriteLine();

                var specification = new Specification<Client>(x => x.BirthDay < DateTime.Today);

                var subQuery = session.Query<Client>().Where(specification);

                Console.WriteLine("specification subquery with variable test".PadLeft(80, '-'));
                Console.WriteLine();
                var ok = session.Query<Client>()
                                .Where(x => subQuery.Any(c => x.Id == c.Id))
                                .ToList();
                Console.WriteLine("specification subquery with variable passed".PadLeft(80, '-'));
                Console.WriteLine();

                Console.WriteLine("specification subquery test".PadLeft(80, '-'));
                Console.WriteLine();
                var notOk = session.Query<Client>()
                                   .Where(x => session.Query<Client>().Where(specification).Any(c => x.Id == c.Id))
                                   .ToList();
                Console.WriteLine("specification subquery passed".PadLeft(80, '-'));
                Console.WriteLine();
            }
        }

        private static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                           .Database(SQLiteConfiguration.Standard.InMemory().ShowSql)
                           .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ClientMap>())
                           .ExposeConfiguration(config =>
                           {
                               config.LinqQueryProvider<CustomQueryProvider>();
                               _config = config;
                           })
                           .BuildSessionFactory();
        }
    }
}