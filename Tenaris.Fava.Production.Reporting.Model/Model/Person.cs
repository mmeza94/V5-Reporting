using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using NHMA = NHibernate.Mapping.Attributes;

namespace Tenaris.Fava.Production.Reporting.Model.DTO
{
    [NHMA.Class(Table = "People", Schema = "Common", DiscriminatorValue = "P")]
    public class Person
    {
        [NHMA.Id(0, Name = "Id", TypeType = typeof(Int32), Column = "idPeople", Access = "property")]
        [NHMA.Generator(1, Class = "native")]
        public virtual int Id { get; set; }

        [NHMA.Property(Name = "Nombre", Access = "property", Column = "Nombre", NotNull = true, Length = 40)]
        public virtual string Nombre { get; set; }

        [NHMA.Property(Name = "ApellidoPaterno", Access = "property", Column = "ApellidoPaterno", NotNull = true, Length = 50)]
        public virtual string ApellidoPaterno { get; set; }

        [NHMA.Property(Name = "ApellidoMaterno", Access = "property", Column = "ApellidoMaterno", NotNull = true, Length = 50)]
        public virtual string ApellidoMaterno { get; set; }

        [NHMA.Property(Name = "InsertedBy", Access = "property", Column = "InsertedBy", NotNull = true)]
        public virtual string InsertedBy { get; set; }

        [NHMA.Property(Name = "UpdatedBy", Access = "property", Column = "UpdatedBy", NotNull = false)]
        public virtual string UpdatedBy { get; set; }

        [NHMA.Property(Name = "InsDateTime", Access = "property", Column = "InsDateTime", NotNull = true)]
        public virtual DateTime InsDateTime { get; set; }

        [NHMA.Property(Name = "UpdDateTime", Access = "property", Column = "UpdDateTime", NotNull = false)]
        public virtual DateTime? UpdDateTime { get; set; }

        [NHMA.Discriminator(Column = "PersonType", NotNull = true, Length = 2)]
        public virtual string PersonType { get; set; }

        [NHMA.Property(Name = "Active", Access = "property", Column = "Active", NotNull = true)]
        public virtual Enumerations.AxlrBit Active { get; set; }
    }
}
