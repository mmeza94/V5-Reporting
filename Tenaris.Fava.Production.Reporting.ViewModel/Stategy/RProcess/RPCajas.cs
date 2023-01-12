using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.Model;
using Tenaris.Fava.Production.Reporting.ViewModel.Dialog;
using Tenaris.Fava.Production.Reporting.ViewModel.Interfaces;
using Tenaris.Fava.Production.Reporting.ViewModel.Support;

namespace Tenaris.Fava.Production.Reporting.ViewModel.Stategy.RProcess
{
    public class RPCajas: IReportingProcess
    {

        private int lbITLoadHelper, tbTotalLoaded, tbPreviousLoaded;
        private string SelectedSendType, _User;
        public ObservableCollection<RejectionReportDetail> dgRejectionReportDetails { get; set; }
        private ReportProductionDto ReportProductionDto { get; set; }
        private ReportConfirmationViewModel reportConfirmation { get; set; }

        private IndBoxReportConfirmationViewModel indBoxReportConfirmation { get; set; }
        private GeneralMachine GeneralMachine { get; set; }
        public int tbScrapCountL2 { get; set; }
        public int tbReworkedCountL2 { get; set; }
        public int tbLoadedCountL2 { get; set; }
        public int tbGoodCountL2 { get; set; }
        public ShowQuestion showQuestion { get; set; }

        public RPCajas(GeneralMachine generalMachine)
        {
            this.GeneralMachine = generalMachine;
        }


        public bool CanReport(GeneralPiece currentDGRow, ReportProductionDto reportProductionDto)
        {
            bool response = true;

            ReportProductionDto = reportProductionDto;

            var maquina = Configurations.Instance.Machine;

            if (!GeneralMachine.Login())
                return response;

            if (!GeneralMachine.IsSended(ReportProductionDto))
                return response;

            if (!GeneralMachine.IsReportSequenceValidated(GeneralMachine.CurrentGeneralPieces, currentDGRow))
                return response;

            return response;
        }

        public bool IsReportConfirmationAccepted(GeneralPiece currentDGRow)
        {

            var IndBoxReportConfirmation = new IndBoxReportConfirmationViewModel(currentDGRow, ReportProductionDto, GeneralMachine.WhoIsLogged);
            //GeneralMachine.Request.Raise(new Notification() { Content =  IndBoxReportConfirmation });
            GeneralMachine.IndBoxReportConfirmationRequest.Raise(new Notification() { Content = IndBoxReportConfirmation });
            //GeneralMachine.Request.Raise(new Notification() { Content=prueba });
            // GC.Collect();
            return IndBoxReportConfirmation.Result;
        }

        private bool IsExtremo1(GeneralPiece currentDGRow)
        {
            return currentDGRow.Extremo.Contains("1");
        }


        public IReportingProcess BuildReport()
        {
            ReportProductionDto = reportConfirmation.CurrentReportProduction;
            ReportProductionDto.IdUDT = reportConfirmation.Atado;
            ReportProductionDto.Orden = reportConfirmation.Orden;
            ReportProductionDto.Colada = reportConfirmation.Colada;
            dgRejectionReportDetails = reportConfirmation.RejectionReportDetails;
            tbScrapCountL2 = reportConfirmation.MalasActual;
            tbReworkedCountL2 = reportConfirmation.ReprocesosActual;
            tbLoadedCountL2 = reportConfirmation.CargadasActual;
            tbGoodCountL2 = reportConfirmation.BuenasActual;
            lbITLoadHelper = reportConfirmation.ITLoadHelper;
            tbTotalLoaded = reportConfirmation.CargadasTotal;
            tbPreviousLoaded = reportConfirmation.CargadasAnterior;
            SelectedSendType = reportConfirmation.SelectedSendType;
            _User = GeneralMachine.WhoIsLogged;

            return this;
        }

        public IReportingProcess ValidateReportStructure()
        {
            if (!ValidationRules.ValidateRejectionReasons(tbScrapCountL2, dgRejectionReportDetails))
            {
                ShowError showError = new ShowError("Error", "La cantidad de Detalles de Rechazos no coincide con la cantidad de Piezas Malas");
                GeneralMachine.ShowErrorMessageRequest.Raise(new Notification() { Content = showError });
                return null;
            }


            var confirmMessage = string.Format("Resumen de lo Reportado: \n\n Buenas:{0} \n" +
                        " Malas:{1} \n Reprocesos:{2} \n Total:{3} \n \n ¿Desea reportar estas cantidades?", tbGoodCountL2,
                        tbScrapCountL2, tbReworkedCountL2, tbLoadedCountL2);

            showQuestion = new ShowQuestion("Confirmar reporte", confirmMessage);
            GeneralMachine.ShowQuestionRequests.Raise(new Notification() { Content = showQuestion });
            return this;
        }

        public ReportProductionDto PrepareDtoForProductionReport()
        {
            if (!showQuestion.Result)
                return null;
            //IsCND();
            ReportProductionDto.CantidadMalas = tbScrapCountL2;
            ReportProductionDto.CantidadBuenas = tbGoodCountL2;
            ReportProductionDto.CantidadReprocesadas = tbReworkedCountL2;
            ReportProductionDto.IdUser = GeneralMachine.WhoIsLogged;

            GetSendStatus();

            return ReportProductionDto;


        }



        private void GetSendStatus()
        {
            Enumerations.ProductionReportSendStatus sendStatus = Enumerations.ProductionReportSendStatus.Completo;

            switch (SelectedSendType)
            {
                case "Parcial":
                    sendStatus = Enumerations.ProductionReportSendStatus.Parcial;
                    break;
                case "Final":
                    sendStatus = Enumerations.ProductionReportSendStatus.Final;
                    break;
                default:
                    break;


            }

            ReportProductionDto.SelectedSendType = sendStatus;


        }

        public void ShowITMessage(string reponse)
        {
            ShowMessage showMessage = new ShowMessage("Reporte de Producción", reponse);
            GeneralMachine.ShowMessageRequest.Raise(new Notification() { Content = showMessage });
        }

        public void CheckReportProductionForNextOperation(string reponse)
        {
            if (reponse.StartsWith("Reporte Enviado Correctamente"))
            {
                var message = "";
                var numeroOperacionsiguiente = ReportProductionDto.Secuencia + 1;
                var operation = ConfigurationManager.AppSettings.Get("Operation_" + (numeroOperacionsiguiente).ToString());
                var isAvailable = GeneralMachine.Adapter.IsGroupItemAvailableForNextOperation(ReportProductionDto.IdUDT,
                    ReportProductionDto.Colada, ReportProductionDto.Orden, ReportProductionDto.Secuencia);
                if (isAvailable)
                {
                    message = string.Format("Disponible para la siguiente\n operación:{0}",
                       operation);
                }
                else
                {
                    var firstItReport = lbITLoadHelper;
                    var totalLoaded = tbTotalLoaded;
                    var piecesForReport = firstItReport - totalLoaded;
                    message = string.Format("El atado NO se encuentra disponible\npara la siguiente operación:{0}.\n" +
                    "Es posible que falten {1} piezas por reportar",
                       operation, piecesForReport.ToString());
                }
                ShowMessage showMessage = new ShowMessage("Reporte de Disponibilidad", message);
                GeneralMachine.ShowMessageRequest.Raise(new Notification() { Content = showMessage });
            }
        }
    }
}
