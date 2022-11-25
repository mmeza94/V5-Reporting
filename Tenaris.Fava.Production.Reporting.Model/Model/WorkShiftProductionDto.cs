using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenaris.Fava.Production.Reporting.Model.DTO
{
    public class WorkShiftProductionDto
    {
        public string Workshift { get; set; }

        public int OrderNumber { get; set; }

        public int HeatNumber { get; set; }
        public int GroupItemNumber { get; set; }
        public String Operation { get; set; }
        public int LoadedCount { get; set; }
        public int GoodCount { get; set; }
        public int ScrapCount { get; set; }
        public int ReworkedCount { get; set; }

    }
}
