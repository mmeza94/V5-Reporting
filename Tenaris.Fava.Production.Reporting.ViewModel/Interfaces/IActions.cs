using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tenaris.Fava.Production.Reporting.Model.Adapter;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.ViewModel.Interfaces;
using Tenaris.Fava.Production.Reporting.ViewModel.Stategy;

namespace Tenaris.Fava.Production.Reporting.Model.Interfaces
{
    public interface IActions
    {
        Dictionary<string, object> Filters { get; set; }
        Dictionary<string, object> OutPuts{ get; set; }
        ITServiceAdapter Adapter { get; set; }
        GeneralMachine GeneralMachine { get; }
        IReportingProcess reportingProcess { get; set; }
        IActions Search();
        IActions Report();
        ObservableCollection<ReportProductionHistory> dgReporteProduccion_SelectionChanged(GeneralPiece SelectedBundle);
    }
}
