using System.Collections.ObjectModel;
using Tenaris.Fava.Production.Reporting.Model.Adapter;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.ViewModel.Interfaces;
using Tenaris.Fava.Production.Reporting.ViewModel.Stategy;

namespace Tenaris.Fava.Production.Reporting.Model.Interfaces
{
    public interface IActions
    {
        ITServiceAdapter Adapter { get; set; }
        GeneralMachine GeneralMachine { get; }
        IReportingProcess reportingProcess { get; set; }

        ObservableCollection<GeneralPiece> Search(int Orden, int Colada, int Atado);
        bool Report(GeneralPiece currentDGRow);

        ObservableCollection<ReportProductionHistory> dgReporteProduccion_SelectionChanged(GeneralPiece SelectedBundle);

        



    }
}
