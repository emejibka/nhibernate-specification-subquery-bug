using FluentNHibernate.Mapping;

namespace TestApp
{
    public class ClientMap : ClassMap<Client>
    {
        public ClientMap()
        {
            Table("CLIENT");

            Id(s => s.Id);
            Map(s => s.Name);
            Map(x => x.Sex);
            Map(x => x.BirthDay);
        }
    }
}