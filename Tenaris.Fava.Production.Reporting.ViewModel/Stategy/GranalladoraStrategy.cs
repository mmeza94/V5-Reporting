using System;
using System.Collections.ObjectModel;
using System.Linq;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Interfaces;
using Tenaris.Fava.Production.Reporting.Model.Model;
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



            // ReportConfirmationViewModel reportConfirmation = new ReportConfirmationViewModel(
            //                        generalPieceDto, reportProductionDto, GetFirstPieceLoadedNumberForIT(generalPieceDto), true, WhoIsLogged);
            //Request.Raise(new Notification() { Content = reportConfirmation });


            //myReports.ReportProduction();
            //myReports.PrepareDtoForProductionReport();

            //ReportConfirmationValidation();
            //PrepareDtoForProductionReport();
            //ReportProduction();


            //if (reportConfirmation.Result)

            //    response = ReportExecute(showErrorMessageRequest, showMessageRequest, showQuestionRequest, reportConfirmation);




            return false;
        }

        #endregion

    }
}
