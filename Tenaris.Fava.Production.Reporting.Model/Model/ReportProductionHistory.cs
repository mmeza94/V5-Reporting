using System;
using Tenaris.Fava.Production.Reporting.Model.Enums;


namespace Tenaris.Fava.Production.Reporting.Model.DTO
{

    public class ReportProductionHistory
    {

        public int Id { get; set; }


        public int IdHistory { get; set; }


        public int IdOrder { get; set; }


        public int HeatNumber { get; set; }


        public int GroupItemNumber { get; set; }


        public Enumerations.ProductionReportSendStatus SendStatus { get; set; }


        public int TotalQuantity { get; set; }


        public int GoodCount { get; set; }


        public int ScrapCount { get; set; }



        public int ReworkedCount { get; set; }


        public int IdMachine { get; set; }


        public int LotNumberHtr { get; set; }


        public DateTime InsDateTime { get; set; }


        public string InsertedBy { get; set; }


        public string UpdDateTime { get; set; }


        public string UpdatedBy { get; set; }


        public int MachineSequence { get; set; }


        public string MachineOption { get; set; }


        public string MachineOperation { get; set; }


        public string Observation { get; set; }


        public string GroupItemType { get; set; }


        public int? ChildOrder { get; set; }

        public int? ChildGroupItemNumber { get; set; }

        public string ChildGroupItemType { get; set; }
    }
}
