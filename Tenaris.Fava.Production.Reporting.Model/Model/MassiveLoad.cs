using NHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using NHMA = NHibernate.Mapping.Attributes;

namespace Tenaris.Fava.Production.Reporting.Model.DTO
{
    [NHMA.Class(Table = "tpsMassiveLoad_t1", Schema = "dbo")]
    public class MassiveLoad
    {

        [NHMA.Id(0, Name = "Id", TypeType = typeof(Int32), Column = "idTpsMassiveLoad", Access = "property")]
        [NHMA.Generator(1, Class = "native")]
        public virtual int Id { get; set; }

        [NHMA.Property(Name = "IdOrder", Access = "property", Column = "OrderNumber", NotNull = true)]
        public virtual int IdOrder { get; set; }

        [NHMA.Property(Name = "HeatNumber", Access = "property", Column = "HeatNumber", NotNull = true)]
        public virtual int HeatNumber { get; set; }

        [NHMA.Property(Name = "GroupItemNumber", Access = "property", Column = "GroupItemNumber", NotNull = true)]
        public virtual int GroupItemNumber { get; set; }

        [NHMA.Property(Name = "LotNumberHTR", Access = "property", Column = "LotNumberHTR", NotNull = true)]
        public virtual int LotNumberHTR { get; set; }

        [NHMA.Property(Name = "SendStatus", Access = "property", Column = "SendStatus", NotNull = false)]
        public virtual Enumerations.ProductionReportSendStatus? SendStatus { get; set; }

        //[NHMA.Property(Name = "TotalQuantity", Access = "property", Column = "TotalQuantity", NotNull = true)]
        public virtual int TotalQuantity
        {
            get { return this.GoodCount + this.ScrapCount; }
        }

        [NHMA.Property(Name = "GoodCount", Access = "property", Column = "GoodCount", NotNull = true)]
        public virtual int GoodCount { get; set; }

        [NHMA.Property(Name = "ScrapCount", Access = "property", Column = "ScrapCount", NotNull = true)]
        public virtual int ScrapCount { get; set; }


        [NHMA.Property(Name = "MachineSequence", Access = "property", Column = "MachineSequence", NotNull = true)]
        public virtual int MachineSequence { get; set; }


        [NHMA.Property(Name = "MachineOperation", Access = "property", Column = "MachineOperation", NotNull = true)]
        public virtual string MachineOperation { get; set; }

        [NHMA.Property(Name = "Destiny", Access = "property", Column = "Destiny")]
        public virtual string Destiny { get; set; }

        [NHMA.Property(Name = "RejectionCode", Access = "property", Column = "RejectionCode", Length = 300)]
        public virtual string RejectionCode { get; set; }

        [NHMA.Property(Name = "ErrorMessage", Access = "property", Column = "ErrorMessage", Length = 300)]
        public virtual string ErrorMessage { get; set; }

        [NHMA.Property(Name = "RejectionDescription", Access = "property", Column = "RejectionDescription", Length = 300)]
        public virtual string RejectionDescription { get; set; }

        [NHMA.Property(Name = "WareHouse", Access = "property", Column = "WareHouse")]
        public virtual string WareHouse { get; set; }

        public virtual string Option { get; set; }
        public virtual string Operation { get; set; }

        [NHMA.Property(Name = "UdtSalida", Access = "property", Column = "UdtSalida")]
        public virtual int UdtSalida { get; set; }

        [NHMA.Property(Name = "CargaInicial", Access = "property", Column = "CargaInicial")]
        public virtual int CargaInicial { get; set; }

        [NHMA.Property(Name = "ReportSequence", Access = "property", Column = "ReportSequence")]
        public virtual int? ReportSequence { get; set; }
    }
}