using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHMA = NHibernate.Mapping.Attributes;


namespace Tenaris.Fava.Production.Reporting.Model.DTO
{
    [NHMA.Class(Table = "Machine", Schema = "Common")]
    public class Machine
    {

        [NHMA.Id(0, Name = "Id", TypeType = typeof(Int32), Column = "idMachine", Access = "property")]
        [NHMA.Generator(1, Class = "native")]
        public virtual int Id { get; set; }

        [NHMA.Property(Name = "Code", Access = "property", Column = "Code")]
        public virtual string Code { get; set; }

        [NHMA.Property(Name = "Name", Access = "property", Column = "Name")]
        public virtual string Name { get; set; }

        [NHMA.Property(Name = "Description", Access = "property", Column = "Description")]
        public virtual string Description { get; set; }

        [NHMA.Property(Name = "Active", Access = "property", Column = "Active")]
        public virtual bool Active { get; set; }
    }
}
