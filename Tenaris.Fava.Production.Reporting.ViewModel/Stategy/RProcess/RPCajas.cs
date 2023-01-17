using LinFu.DynamicProxy;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using Tenaris.Fava.Production.Reporting.Model.Business;
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

        public IndBoxReportConfirmationViewModel indBoxReportConfirmation { get; set; }

        public ProductionBox SelectedBox { get; set; }
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

            indBoxReportConfirmation = new IndBoxReportConfirmationViewModel(currentDGRow, ReportProductionDto, GeneralMachine.WhoIsLogged);
            SelectedBox = indBoxReportConfirmation.SelectedBox;

            //GeneralMachine.Request.Raise(new Notification() { Content =  IndBoxReportConfirmation });
            GeneralMachine.IndBoxReportConfirmationRequest.Raise(new Notification() { Content = indBoxReportConfirmation });
            //GeneralMachine.Request.Raise(new Notification() { Content=prueba });
            // GC.Collect();
            return indBoxReportConfirmation.Result;
        }




        




        public IReportingProcess BuildReport()
        {
            ReportProductionDto = indBoxReportConfirmation.currentGeneralPiece.BuildReportProductionDTO();//Contruyo mi reporte a partir de mi current general piece(selected bundle)
            dgRejectionReportDetails = indBoxReportConfirmation.DgRejectionReportDetails;
            tbScrapCountL2 = indBoxReportConfirmation.Malas;
            tbReworkedCountL2 = indBoxReportConfirmation.Reprocesos;
            tbLoadedCountL2 = indBoxReportConfirmation.Total;
            tbGoodCountL2 = indBoxReportConfirmation.Buenas;
            //lbITLoadHelper = indBoxReportConfirmation.ITLoadHelper;
            //tbTotalLoaded = indBoxReportConfirmation.CargadasTotal;
            //tbPreviousLoaded = indBoxReportConfirmation.CargadasAnterior;
            //SelectedSendType = indBoxReportConfirmation.SelectedSendType;
            _User = GeneralMachine.WhoIsLogged;

            return this;
        }

        public IReportingProcess ValidateReportStructure()
        {
            if (!ValidateBox())
                return null;
            
            var confirmMessage = string.Format("Resumen de lo Reportado: \n\n Buenas:{0} \n" +
                        " Malas:{1} \n Reprocesos:{2} \n Total:{3} \n \n ¿Desea reportar estas cantidades?", tbGoodCountL2,
                        tbScrapCountL2, tbReworkedCountL2, tbLoadedCountL2);

            showQuestion = new ShowQuestion("Confirmar reporte", confirmMessage);
            GeneralMachine.ShowQuestionRequests.Raise(new Notification() { Content = showQuestion });

            return this;

        }


        public void ReportProductionIT()
        {
            string errorMessage = string.Empty;
            GeneralMachine.Adapter.LoadProductionBox(ReportProductionDto,SelectedBox,out errorMessage);
            //GeneralMachine.Adapter.ReportProductionBox();
        }




        private bool ValidateBox()
        {
            if (IsSelectedBoxNull())
                return false;
            if (ValidateWorkedPieces())
                return false;
            if (ValidateGoodPieces())
                return false;
            if (!IsBoxSelectedProductionGuide())
                return false;
            if (ValidatePendingBoxes())
                return false;
            return true;
        }




        private bool ValidatePendingBoxes()
        {
            List<ProductionBox> listUsed = indBoxReportConfirmation.DgBoxes.Where(l => l.LoadedPieces > 0).ToList();

            //LE INFORMA AL OPERADOR QUE HA DEJADO CAJAS PENDIENTES
            if (listUsed.Count > 0 && !listUsed.Contains(SelectedBox))
            {
                string confirmBox = string.Format("Ha seleccionado una caja diferente a las cajas que estan pendientes de llenar.\nEsta seguro que quiere seleccionar esta caja?\nCaja: {0}",
                    SelectedBox.Id);
                ShowQuestion showQuestion = new ShowQuestion("Confirmar caja", confirmBox);
                GeneralMachine.ShowQuestionRequests.Raise(new Notification() { Content = showQuestion });
                //if (MessageBox.Show(confirmBox, "Confirmar Caja", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                if (!showQuestion.Result)
                {
                    return true;
                }

            }
            return false;
        }



        private bool IsBoxSelectedProductionGuide()
        {

            bool result = (ProductionReportingBusiness.IsBoxSelect(indBoxReportConfirmation.currentGeneralPiece.OrderNumber, SelectedBox.Id)) == 1 ? true : false;
            if (!result)
            {
                string confirmBox2 = string.Format("La caja {0} no corresponde a la selecionada en Guia de Produccion, o no se ha marcado ninguna en la Guia de Produccion.\nSeleccione la caja Correspondiente\n \n¿Desea continuar aun asi?\n", SelectedBox.Id);
                ShowQuestion showQuestion = new ShowQuestion("Confirmar caja", confirmBox2);
                GeneralMachine.ShowQuestionRequests.Raise(new Notification() { Content = showQuestion });
                if (!showQuestion.Result)
                {
                    return false;
                }

            }
            return true;
        }



        private bool ValidateGoodPieces()
        {
            if (indBoxReportConfirmation.Buenas <= SelectedBox.MissingPieces)
            {
                ShowError message = new ShowError("Reporte de Producción", string.Format("El Total de Piezas Buenas ({0}) debe ser menor o igual al número de Piezas Disponibles en la Caja Seleccionada ({1})",
                            indBoxReportConfirmation.Buenas, SelectedBox.MissingPieces));
                GeneralMachine.ShowErrorMessageRequest.Raise(new Notification() { Content = message });
                return true;
            }
            return false;
        }



        private bool IsSelectedBoxNull()
        {
            if(SelectedBox is null)
            {
                ShowError message = new ShowError("Reporte de Producción", "No ha seleccionado una Caja");
                GeneralMachine.ShowErrorMessageRequest.Raise(new Notification() { Content = message });
                return true;
            }
            return false;
        }

        private bool ValidateWorkedPieces()
        {
            if (indBoxReportConfirmation.Total > indBoxReportConfirmation.TotalActualAtado)
            {
                ShowError message = new ShowError("Reporte de Producción", string.Format("El Total de Piezas Procesadas ({0}) debe ser menor o igual al número de Piezas Disponibles en el Atado ({1})",
                            indBoxReportConfirmation.Total, indBoxReportConfirmation.TotalActualAtado));
                GeneralMachine.ShowErrorMessageRequest.Raise(new Notification() { Content = message });
                return true;
            }
            return false;
        }



        public ReportProductionDto PrepareDtoForProductionReport()
        {
            if (!showQuestion.Result)
                return null;

            ReportProductionDto.CantidadBuenas = tbGoodCountL2;
            ReportProductionDto.CantidadMalas = tbScrapCountL2;
            ReportProductionDto.CantidadReprocesadas = tbReworkedCountL2;
            ReportProductionDto.CantidadTotal = indBoxReportConfirmation.currentGeneralPiece.LoadedCount;
            ReportProductionDto.Orden = SelectedBox.ParentOrderNumber;
            ReportProductionDto.IdUser = GeneralMachine.WhoIsLogged;
            ReportProductionDto.Operacion = SelectedBox.OperationId;
            ReportProductionDto.Opcion = SelectedBox.MachineId;
            ReportProductionDto.Observaciones = indBoxReportConfirmation.ChangeReason;






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
