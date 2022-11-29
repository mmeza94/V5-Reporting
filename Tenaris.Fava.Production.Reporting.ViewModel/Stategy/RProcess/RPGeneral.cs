using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.Model;
using Tenaris.Fava.Production.Reporting.ViewModel.Dialog;
using Tenaris.Fava.Production.Reporting.ViewModel.Interfaces;
using Tenaris.Fava.Production.Reporting.ViewModel.Support;

namespace Tenaris.Fava.Production.Reporting.ViewModel.Stategy.RProcess
{
    public class RPGeneral : IReportingProcess
    {
        private int lbITLoadHelper, tbTotalLoaded, tbPreviousLoaded;
        private string SelectedSendType, _User;
        public ObservableCollection<RejectionReportDetail> dgRejectionReportDetails { get; set; }
        private ReportProductionDto ReportProductionDto { get; set; }
        private ReportConfirmationViewModel reportConfirmation { get; set; }
        private GeneralMachine GeneralMachine { get; set; }
        public int tbScrapCountL2 { get; set; }
        public int tbReworkedCountL2 { get; set; }
        public int tbLoadedCountL2 { get; set; }
        public int tbGoodCountL2 { get; set; }
        public ShowQuestion showQuestion { get; set; }

        public  RPGeneral(GeneralMachine generalMachine)
        {
            this.GeneralMachine = generalMachine;
        }

        public bool CanReport(GeneralPiece currentDGRow)
        {
            bool response = true;
            var generalPieceDto = currentDGRow;
            ReportProductionDto = generalPieceDto.BuildReportProductionDTO();
            var maquina = Configurations.Instance.Machine;

            if (!GeneralMachine.Login())
                return response;

            if (!GeneralMachine.IsSended(ReportProductionDto))
                return response;

            if (!GeneralMachine.IsReportSequenceValidated(GeneralMachine.currentGeneralPieces, generalPieceDto))
                return response;

            return response;
        }

        public bool IsReportConfirmationAccepted(GeneralPiece currentDGRow)
        {
            reportConfirmation = new ReportConfirmationViewModel(
                                    currentDGRow, ReportProductionDto, GeneralMachine.GetFirstPieceLoadedNumberForIT(currentDGRow), true, GeneralMachine.WhoIsLogged);
            GeneralMachine.Request.Raise(new Notification() { Content = reportConfirmation });

            return reportConfirmation.Result;
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
            if(!ValidationRules.ValidateRejectionReasons(tbScrapCountL2, dgRejectionReportDetails))
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
            IsCND();
            ReportProductionDto.CantidadMalas = tbScrapCountL2;
            ReportProductionDto.CantidadBuenas = tbGoodCountL2;
            ReportProductionDto.CantidadReprocesadas = tbReworkedCountL2;
            ReportProductionDto.IdUser = GeneralMachine.WhoIsLogged;

            GetSendStatus();

            return ReportProductionDto;

            
        }


        private void IsCND()
        {
            if (Configurations.Instance.Machine.ToUpper().Equals("CND"))
            {
                ReportProductionDto.CantidadTotal =  lbITLoadHelper;
                return;
            }
            ReportProductionDto.CantidadTotal = (lbITLoadHelper - tbPreviousLoaded == 0) ? tbLoadedCountL2 : lbITLoadHelper - tbPreviousLoaded;

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


        #region Metodos temporales
        #endregion




    }
}
