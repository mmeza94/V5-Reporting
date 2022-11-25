using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHMA = NHibernate.Mapping.Attributes;


namespace Tenaris.Fava.Production.Reporting.Model.DTO
{
    [NHMA.Subclass(Extends = "Person", ExtendsType = typeof(Person), DiscriminatorValue = "U")]
    public class User: Person
    {
        [NHMA.Property(Name = "UserName", Access = "property", Column = "UserName", NotNull = false, Length = 13)]
        public virtual string UserName { get; set; }

        [NHMA.Property(Name = "Password", Access = "property", Column = "Password", NotNull = false, Length = 13)]
        public virtual string Password { get; set; }

    }
}
