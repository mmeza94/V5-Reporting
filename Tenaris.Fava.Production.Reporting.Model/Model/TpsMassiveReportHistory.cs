using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using NHMA = NHibernate.Mapping.Attributes;


namespace Tenaris.Fava.Production.Reporting.Model.DTO
{
    [NHMA.Class(Table = "TpsMassiveReportHistory", Schema = "Production")]
    public class TpsMassiveReportHistory
    {

        [NHMA.Id(0, Name = "Id", TypeType = typeof(Int32), Column = "idTpsMassiveReportHistory", Access = "property")]
        [NHMA.Generator(1, Class = "native")]
        public virtual int Id { get; set; }


        [NHMA.Property(Name = "IdOrder", Access = "property", Column = "OrderNumber", NotNull = true)]
        public virtual int IdOrder { get; set; }

        [NHMA.Property(Name = "HeatNumber", Access = "property", Column = "HeatNumber", NotNull = true)]
        public virtual int HeatNumber { get; set; }

        [NHMA.Property(Name = "GroupItemNumber", Access = "property", Column = "GroupItemNumber", NotNull = true)]
        public virtual int GroupItemNumber { get; set; }

        [NHMA.Property(Name = "SendStatus", Access = "property", Column = "SendStatus", NotNull = true)]
        public virtual Enumerations.ProductionReportSendStatus SendStatus { get; set; }

        [NHMA.Property(Name = "TotalQuantity", Access = "property", Column = "TotalQuantity", NotNull = true)]
        public virtual int TotalQuantity { get; set; }

        [NHMA.Property(Name = "GoodCount", Access = "property", Column = "GoodCount", NotNull = true)]
        public virtual int GoodCount { get; set; }


        [NHMA.Property(Name = "ScrapCount", Access = "property", Column = "ScrapCount", NotNull = true)]
        public virtual int ScrapCount { get; set; }



        [NHMA.Property(Name = "ReworkedCount", Access = "property", Column = "ReworkedCount", NotNull = true)]
        public virtual int ReworkedCount { get; set; }

        //[NHMA.Property(Name = "IdMachine", Access = "property", Column = "IdMachine", NotNull = true)]
        //public virtual int IdMachine { get; set; }

        [NHMA.Property(Name = "LotNumberHtr", Access = "property", Column = "LotNumberHtr", NotNull = true)]
        public virtual int LotNumberHtr { get; set; }

        [NHMA.Property(Name = "InsDateTime", Access = "property", Column = "InsDateTime", NotNull = true)]
        public virtual DateTime InsDateTime { get; set; }

        [NHMA.Property(Name = "InsertedBy", Access = "property", Column = "InsertedBy", NotNull = true)]
        public virtual string InsertedBy { get; set; }

        [NHMA.Property(Name = "UpdDateTime", Access = "property", Column = "UpdDateTime")]
        public virtual string UpdDateTime { get; set; }

        [NHMA.Property(Name = "UpdatedBy", Access = "property", Column = "UpdatedBy")]
        public virtual string UpdatedBy { get; set; }

        [NHMA.Property(Name = "MachineSequence", Access = "property", Column = "MachineSequence", NotNull = true)]
        public virtual int MachineSequence { get; set; }

        [NHMA.Property(Name = "MachineOption", Access = "property", Column = "MachineOption", NotNull = true)]
        public virtual string MachineOption { get; set; }

        [NHMA.Property(Name = "MachineOperation", Access = "property", Column = "MachineOperation", NotNull = true)]
        public virtual string MachineOperation { get; set; }

        [NHMA.Property(Name = "Observation", Access = "property", Column = "Observation", NotNull = false)]
        public virtual string Observation { get; set; }

        [NHMA.Property(Name = "Destiny", Access = "property", Column = "Destiny")]
        public virtual string Destiny { get; set; }

        [NHMA.Property(Name = "RejectionCode", Access = "property", Column = "RejectionCode", Length = 300)]
        public virtual string RejectionCode { get; set; }

        [NHMA.Property(Name = "ReportSequence", Access = "property", Column = "ReportSequence", Length = 300)]
        public virtual int? ReportSequence { get; set; }
    }
}
