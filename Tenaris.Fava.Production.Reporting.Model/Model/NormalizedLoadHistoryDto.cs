using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenaris.Fava.Production.Reporting.Model.DTO
{
    public class NormalizedLoadHistoryDto
    {
        public int Identificador { get; set; }
        public int OrderNumber { get; set; }
        public int HeatNumber { get; set; }
        public int GroupItemNumber { get; set; }
        public DateTime InsDateTime { get; set; }
    }
}
