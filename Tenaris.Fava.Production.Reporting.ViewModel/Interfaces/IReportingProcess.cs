using Tenaris.Fava.Production.Reporting.Model.DTO;

namespace Tenaris.Fava.Production.Reporting.ViewModel.Interfaces
{
    public interface IReportingProcess
    {
        bool CanReport(GeneralPiece currentDGRow);
        bool IsReportConfirmationAccepted(GeneralPiece currentDGRow);
        void BuildReport();
        void ValidateReportStructure();
        void PrepareDtoForProductionReport();
    }
}
