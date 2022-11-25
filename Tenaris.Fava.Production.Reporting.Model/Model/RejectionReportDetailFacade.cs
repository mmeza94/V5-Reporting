using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.NhAccess.Reporitories;

namespace Tenaris.Fava.Production.Reporting.Model.Support
{
    internal class RejectionReportDetailFacade
    {
        public IList GetRejectionReportDetailsByReportProductionHistory(ReportProductionHistory reportProductionHist)
        {
            try
            {
                return new RejectionReportDetailRepository().GetListByPropertyValue
                    (typeof(RejectionReportDetail), "ReportProductionHistory", reportProductionHist);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IList GetRejectionReportDetailsByReportProductionHistory(Int32 idReportProductionHist)
        {
            try
            {
                var reportProductionHist = (ReportProductionHistory)new RejectionReportDetailRepository().
                    Get(typeof(ReportProductionHistory), idReportProductionHist);
                return GetRejectionReportDetailsByReportProductionHistory(reportProductionHist);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetRejectionDetailReport(DateTime fechaIni, DateTime fechaFin, Enumerations.Zone zone)
        {
            return new RejectionReportDetailRepository().GetRejectionDetailReport(fechaIni, fechaFin, zone);
        }
    }
}
