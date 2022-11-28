using System.Collections.ObjectModel;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.ViewModel.Interfaces;
using Tenaris.Fava.Production.Reporting.ViewModel.Stategy;

namespace Tenaris.Fava.Production.Reporting.Model.Interfaces
{
    public interface IActions
    {
        GeneralMachine GeneralMachine { get; }
        IReportingProcess reportingProcess { get; set; }

        ObservableCollection<GeneralPiece> Search(int Orden, int Colada, int Atado);
        bool Report(GeneralPiece currentDGRow);


        //void GetMachineInformation(ref ReportProductionDto reportProductDto, GeneralPiece currentDGRow);



    }
}
