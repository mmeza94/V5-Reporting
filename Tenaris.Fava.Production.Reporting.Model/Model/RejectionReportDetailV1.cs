using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using NHMA = NHibernate.Mapping.Attributes;

namespace Tenaris.Fava.Production.Reporting.Model.DTO
{
    [NHMA.Class(Table = "RejectionReportDetail", Schema = "Production")]
    public class RejectionReportDetailV1 : BaseLevel2Object
    {


        [NHMA.Id(0, Name = "Id", TypeType = typeof(Int32), Column = "idRejectionReportDetail", Access = "property")]
        [NHMA.Generator(1, Class = "native")]
        public virtual int Id { get; set; }

        //[NHMA.Property(Name = "ReportProductionHistory", Access = "property", Column = "idReportProductionHistory")]
        [NHMA.ManyToOne(2, Name = "ReportProductionHistory", Access = "property", Column = "idReportProductionHistory",
        Class = "ReportProductionHistory", ClassType = typeof(ReportProductionHistory),
        ForeignKey = "FK_RejectionReportDetail_ReportProductionHistory", NotNull = true)]
        public virtual ReportProductionHistoryV1 ReportProductionHistory { get; set; }

        [NHMA.Property(Name = "ScrapCount", Access = "property", Column = "ScrapCount")]
        public virtual Int16 ScrapCount { get; set; }

        //[NHMA.Property(Name = "RejectionCode", Access = "property", Column = "idRejectionCode")]
        [NHMA.ManyToOne(3, Name = "RejectionCode", Access = "property", Column = "idRejectionCode",
        Class = "RejectionCode", ClassType = typeof(RejectionCode),
        ForeignKey = "FK_RejectionReportDetail_RejectionCode", NotNull = true)]
        public virtual RejectionCode RejectionCode { get; set; }

        [NHMA.Property(Name = "Observation", Access = "property", Column = "Observation", NotNull = false)]
        public virtual string Observation { get; set; }

        [NHMA.Property(Name = "Destino", Access = "property", Column = "Destino", NotNull = false)]
        public virtual string Destino { get; set; }

        [NHMA.Property(Name = "Trabajado", Access = "property", Column = "Trabajado")]
        public virtual Enumerations.AxlrBit Trabajado { get; set; }

        [NHMA.Property(Name = "Extremo", Access = "property", Column = "Extremo")]
        public virtual string Extremo { get; set; }

        [NHMA.Property(Name = "InsDateTime", Access = "property", Column = "InsDateTime")]
        public virtual DateTime InsDateTime { get; set; }

        [NHMA.Property(Name = "Active", Access = "property", Column = "Active")]
        public virtual Enumerations.AxlrBit Active { get; set; }

        public virtual string RejectionCodeDescription
        {
            get { return this.RejectionCode.Description; }
            set { this.RejectionCode.Description = value; }
        }

    }
}
