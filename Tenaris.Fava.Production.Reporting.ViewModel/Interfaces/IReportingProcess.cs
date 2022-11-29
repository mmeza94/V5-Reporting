using Iesi.Collections;
using System.Collections.ObjectModel;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.ViewModel.Dialog;

namespace Tenaris.Fava.Production.Reporting.ViewModel.Interfaces
{
    public interface IReportingProcess
    {
        ObservableCollection<RejectionReportDetail> dgRejectionReportDetails { get; set; }
        ShowQuestion showQuestion { get; set; }
        int tbScrapCountL2 { get; set; }
        int tbReworkedCountL2 { get; set; }
        int tbLoadedCountL2 { get; set; }
        int tbGoodCountL2 { get; set; }
        bool CanReport(GeneralPiece currentDGRow);
        bool IsReportConfirmationAccepted(GeneralPiece currentDGRow);
        IReportingProcess BuildReport();
        IReportingProcess ValidateReportStructure();
        ReportProductionDto PrepareDtoForProductionReport();
    }
}
