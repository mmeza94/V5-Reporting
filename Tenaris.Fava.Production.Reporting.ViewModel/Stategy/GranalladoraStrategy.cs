using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tenaris.Fava.Production.Reporting.Model.Business;
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
        public Dictionary<string, object> Filters { get; set; }
        public Dictionary<string, object> OutPuts { get; set; }
        #endregion

        #region Constructor

        public GranalladoraStrategy()
        {
            reportingProcess = new RPGeneral(this);
            Filters = Filter;
            OutPuts = OutPut;
        }

        #endregion

        #region Implements methods
        public IActions Search()
        {
            try
            {
                CurrentGeneralPieces = ProductionReport.GetProductionGeneral(Filters);

                if (CurrentGeneralPieces == null)
                    AddValues("Search", new ObservableCollection<GeneralPiece>());

                CurrentGeneralPieces = new ObservableCollection<GeneralPiece>(ProductionReport.ClassifyBySendStatus(CurrentGeneralPieces));

                AddValues("Search", CurrentGeneralPieces);

                return this;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Report(GeneralPiece currentDGRow)
        {
            var ReportPRoduction = currentDGRow.BuildReportProductionDTO();

            if (!reportingProcess.CanReport(currentDGRow, ReportPRoduction))
                return false;

            if (!reportingProcess.IsReportConfirmationAccepted(currentDGRow))
                return false;

            ReportProductionDto currentReportProductionDTO = reportingProcess.BuildReport()
                                                                             .ValidateReportStructure()
                                                                             .PrepareDtoForProductionReport();

            var response = Adapter.ReportProduction(WhoIsLogged, currentReportProductionDTO, currentReportProductionDTO.SelectedSendType,
                true, reportingProcess.dgRejectionReportDetails);


            reportingProcess.ShowITMessage(response);



            reportingProcess.CheckReportProductionForNextOperation(response);

            return false;
        }

        public ObservableCollection<ReportProductionHistory> dgReporteProduccion_SelectionChanged(GeneralPiece SelectedBundle)
        {
            if (SelectedBundle == null)
                return new ObservableCollection<ReportProductionHistory>();

            ObservableCollection<ReportProductionHistory> productionReportHistories =
               ProductionReportingBusiness.GetReportProductionHistoryByParamsTest(
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
