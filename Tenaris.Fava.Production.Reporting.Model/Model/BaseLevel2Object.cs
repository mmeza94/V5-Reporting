using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.Enums;

namespace Tenaris.Fava.Production.Reporting.Model.DTO
{
    public class BaseLevel2Object
    {
        public virtual DateTime InsDateTime { get; set; }

        public virtual Enumerations.AxlrBit Active { get; set; }

        public virtual DateTime? UpdDateTime { get; set; }
    }
}
