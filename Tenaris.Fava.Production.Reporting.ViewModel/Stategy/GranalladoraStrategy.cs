using System;
using System.Collections.ObjectModel;
using System.Linq;
using Tenaris.Fava.Production.Reporting.Model.Adapter;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Interfaces;
using Tenaris.Fava.Production.Reporting.Model.Support;
using Tenaris.Fava.Production.Reporting.ViewModel.Interfaces;
using Tenaris.Fava.Production.Reporting.ViewModel.Stategy;
using Tenaris.Fava.Production.Reporting.ViewModel.Stategy.RProcess;

namespace Tenaris.Fava.Production.Reporting.Model.Stategy
{
    public class GranalladoraStrategy : GeneralMachine, IActions
    {
        #region Properties
        public GeneralMachine GeneralMachine { get => this; }
        public IReportingProcess reportingProcess { get; set; }
        public ITServiceAdapter Adapter { get; set; }

        #endregion

        #region Constructor

        public GranalladoraStrategy()
        {
            Adapter = new ITServiceAdapter();
            reportingProcess = new RPGeneral(this);
        }

        #endregion

        #region Implements methods
        public ObservableCollection<GeneralPiece> Search(int Orden, int Colada, int Atado)
        {
            try
            {
                var generalPieces = ProductionReport.GetProductionGeneral(Orden, Colada, Atado);
                if (generalPieces == null)
                    return null;
                currentGeneralPieces = ProductionReport.ClassifyBySendStatus(generalPieces).ToList();

                return new ObservableCollection<GeneralPiece>(currentGeneralPieces);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Report(GeneralPiece currentDGRow)
        {

            if (!reportingProcess.CanReport(currentDGRow))
                return false;

            if (!reportingProcess.IsReportConfirmationAccepted(currentDGRow))
                return false;

            ReportProductionDto currentReportProductionDTO = reportingProcess.BuildReport().ValidateReportStructure().PrepareDtoForProductionReport();

            Adapter.ReportProduction(WhoIsLogged, currentReportProductionDTO,
                currentReportProductionDTO.SelectedSendType, true,
                reportingProcess.dgRejectionReportDetails);

            return false;
        }

        #endregion

    }
}
