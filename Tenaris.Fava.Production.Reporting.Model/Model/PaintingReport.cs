using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenaris.Fava.Production.Reporting.Model.DTO
{
    public class PaintingReport
    {
        public int? IdPainting { get; set; }
        public string BoxUdt { get; set; }
        public string ParentUdt { get; set; }
        public int ChildOrden { get; set; }
        public int ParentOrden { get; set; }
        public int HeatNumber { get; set; }
        public string HeatNumberCode { get; set; }
        public int LoadQuantity { get; set; }
        public int SendedQuantiry { get; set; }
        public string Storage { get; set; }
        public int NextSequence { get; set; }
        public string NextOperation { get; set; }
        public string NextOption { get; set; }
        public string LotId { get; set; }
        public string UdtType { get; set; }
        public string UdcType { get; set; }

        public int GoodCount { get; set; }
        public int ScrapCount { get; set; }
        public string IdUser { get; set; }
        public string IdHistory { get; set; }

        public DateTimeOffset InsDatetIme { get; set; }
        public DateTimeOffset UpdDatetIme { get; set; }
    }
}
