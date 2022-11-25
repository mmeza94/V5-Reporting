using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.Enums;

namespace Tenaris.Fava.Production.Reporting.Model.DTO
{
    public class GeneralPiece
    {


        public int IdHistory { get; set; }
        public int OrderNumber { get; set; }
        public string Customer { get; set; }
        public int HeatNumber { get; set; }
        public int GroupItemNumber { get; set; }
        public int LotNumberHTR { get; set; }
        public int LoadedCount { get; set; }
        public int GoodCount { get; set; }
        public int ScrapCount { get; set; }
        public int WarnedCount { get; set; }
        public int ReworkedCount { get; set; }
        public int PendingCount { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public Enumerations.ProductionReportSendStatus SendStatus { get; set; }
        public int IdBatch { get; set; }
        public DateTimeOffset InsDateTime { get; set; }
        public Enumerations.AxlrBit Sended { get; set; }
        public int ReportSequence { get; set; }
        public string Extremo { get; set; }
        public string ReportBox { get; set; }
        public string GroupItemType { get; set; }

        public string SendedString { get; set; }

        // campos forja 0

        public int Cargados { get; set; }

        public int TotalReportado
        {
            get;
            set;
        }

        public int BuenasReportadas { get; set; }

        public int MalasReportadas { get; set; }

        public int PendientesPorReportar { get; set; }

        public string ShiftDate { get; set; }
        public string ShiftNumber { get; set; }
    }
}
