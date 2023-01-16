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

namespace Tenaris.Fava.Production.Reporting.ViewModel.Stategy
{
    public class CoplesStrategy : GeneralMachine, IActions
    {

        #region Properties
        private IFormatterPiece formatterPiece;
        public GeneralMachine GeneralMachine { get => this; }
        public IReportingProcess reportingProcess { get; set; }
        public Dictionary<string, object> Filters { get; set; }
        public Dictionary<string, object> OutPuts { get; set; }
        #endregion

        #region Constructor

        public CoplesStrategy()
        {
            reportingProcess = new RPGeneral(this);
            Filters = Filter;
            OutPuts = OutPut;
            formatterPiece = new ProcessorPieces.ProcessorByCoples();
            
        }
      

        #endregion

        #region Implements methods
        public IActions Search()
        {
            try
            {
                CurrentGeneralPieces = ProductionReportingBusiness
                    .GetProductionGeneral(Filters)
                    .FormatterPieces(formatterPiece);

                AddValues("Search", CurrentGeneralPieces);


                //AddValues("Search", CurrentGeneralPieces);
                
            }
            catch (Exception)
            {
               // throw;
            }
            return this;
        }

        public IActions Report()
        {
            GeneralPiece currentDGRow = (GeneralPiece)Filters["Selected_Bundle"];
            var ReportPRoduction = currentDGRow.BuildReportProductionDTO();

            if (!reportingProcess.CanReport(currentDGRow, ReportPRoduction))
                return this;

            if (!reportingProcess.IsReportConfirmationAccepted(currentDGRow))
                return this;

            try
                {
               
           

            ReportProductionDto currentReportProductionDTO = reportingProcess.BuildReport()
                                                                             .ValidateReportStructure()
                                                                             .PrepareDtoForProductionReport();
            

            var response = Adapter.ReportProduction(WhoIsLogged, currentReportProductionDTO, currentReportProductionDTO.SelectedSendType,
                true, reportingProcess.dgRejectionReportDetails);


            reportingProcess.ShowITMessage(response);



            reportingProcess.CheckReportProductionForNextOperation(response);

            }
            catch (NullReferenceException)
            {
               // TODO: REPLICAR TRY-CATCH eN LAS DEMAS STRATEGIES
            }

            return this;
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
