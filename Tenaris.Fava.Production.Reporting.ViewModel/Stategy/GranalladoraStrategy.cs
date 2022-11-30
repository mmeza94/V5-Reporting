using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Tenaris.Fava.Production.Reporting.Model.Adapter;
using Tenaris.Fava.Production.Reporting.Model.Data_Access;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Interfaces;
using Tenaris.Fava.Production.Reporting.Model.Support;
using Tenaris.Fava.Production.Reporting.ViewModel.Dialog;
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
        

        #endregion

        #region Constructor

        public GranalladoraStrategy()
        {
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
                    return new ObservableCollection<GeneralPiece>();
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

            ReportProductionDto currentReportProductionDTO = reportingProcess.BuildReport()
                                                                             .ValidateReportStructure()
                                                                             .PrepareDtoForProductionReport();

            var response  = Adapter.ReportProduction(WhoIsLogged, currentReportProductionDTO, currentReportProductionDTO.SelectedSendType,
                true,reportingProcess.dgRejectionReportDetails);


            reportingProcess.ShowITMessage(response);

  

            reportingProcess.CheckReportProductionForNextOperation(response);

            return false;
        }

        public ObservableCollection<ReportProductionHistory> dgReporteProduccion_SelectionChanged(GeneralPiece SelectedBundle)
        {
            if (SelectedBundle == null)
                return new ObservableCollection<ReportProductionHistory>();

            ObservableCollection<ReportProductionHistory> productionReportHistories =
                DataAccessSQL.Instance.GetReportProductionHistoryByParamsTest(
                new Dictionary<string, object>
            {
                        { "@Order", SelectedBundle.OrderNumber },
                        { "@GroupItemNumber", SelectedBundle.GroupItemNumber },
                        { "@HeatNumber", SelectedBundle.HeatNumber },
                        { "@idHistory", 0 },
                        { "@SendStatus", 0 },
                        { "@MachineSequence", 0 }
            });

            //productionReportHistories;

            return productionReportHistories;
        }

        #endregion

    }
}
