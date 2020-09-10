using System;

namespace TestApp
{
    public class Client
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Sex { get; set; }
        public virtual DateTime BirthDay { get; set; }

        public static readonly Specification<Client> IsMaleSpecification = new Specification<Client>(x => x.Sex == "М");

        [Specification(nameof(IsMaleSpecification))]
        public virtual bool IsMale => IsMaleSpecification.IsSatisfiedBy(this);

        public static readonly Specification<Client> IsFemaleSpecification = new Specification<Client>(x => x.Sex == "Ж");

        [Specification(nameof(IsFemaleSpecification))] //в атрибут необходимо передать наименование поля, в котором находится спецификация
        public virtual bool IsFemale => IsFemaleSpecification.IsSatisfiedBy(this);

        public static readonly Specification<Client> IsAdultSpecification = new Specification<Client>(x => x.BirthDay <= DateTime.Today.AddYears(-18));

        [Specification(nameof(IsAdultSpecification))]
        public virtual bool IsAdult => IsAdultSpecification.IsSatisfiedBy(this);
    }
}