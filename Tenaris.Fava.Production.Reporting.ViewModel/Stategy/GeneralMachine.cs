using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Controls;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.Model;
using Tenaris.Fava.Production.Reporting.Model.Support;
using Tenaris.Fava.Production.Reporting.ViewModel.Dialog;
using Tenaris.Fava.Production.Reporting.ViewModel.Support;

namespace Tenaris.Fava.Production.Reporting.ViewModel.Stategy
{
    public abstract class GeneralMachine
    {

        public string WhoIsLogged { get; set; }
        public IList<GeneralPiece> currentGeneralPieces { get; set; }
        public InteractionRequest<Notification> Request { get; set; }
        public InteractionRequest<Notification> IndBoxReportConfirmationRequest { get; set; }
        public InteractionRequest<Notification> ShowErrorMessageRequest { get; set; }
        public InteractionRequest<Notification> ShowMessageRequest { get; set; }
        public InteractionRequest<Notification> ShowQuestionRequests { get; set; }

        public virtual ReportProductionDto GetCurrentGroupItemToReport(GeneralPiece currentDGRow)
        {
            ReportProductionDto reportProductionDto = null;

            if (currentDGRow != null)
            {
                var GeneralPiece = currentDGRow;
                reportProductionDto = new ReportProductionDto()
                {

                    TipoUDT = string.IsNullOrEmpty(GeneralPiece.GroupItemType) ? "Tarjeta de Linea" : GeneralPiece.GroupItemType,
                    IdBatch = GeneralPiece.IdBatch,
                    IdHistory = GeneralPiece.IdHistory,
                    Orden = GeneralPiece.OrderNumber,
                    Almacen = GeneralPiece.Location,
                    IdUDT = GeneralPiece.GroupItemNumber,
                    Colada = GeneralPiece.HeatNumber,
                    Lote = GeneralPiece.LotNumberHTR,
                    Aprietes = 0,
                    DescripcionMaquina = GeneralPiece.Description,
                    CantidadMalas = GeneralPiece.ScrapCount,
                    CantidadBuenas = GeneralPiece.GoodCount,
                    CantidadReprocesadas = GeneralPiece.ReworkedCount,
                    Enviado = GeneralPiece.Sended,
                    CantidadTotal = GeneralPiece.LoadedCount,
                    Secuencia = Convert.ToInt32(Configurations.Instance.Secuencia),
                    Operacion = Configurations.Instance.Operacion,
                    Opcion = Configurations.Instance.Opcion


                };


            }
            return reportProductionDto;
        }


        public bool Login()
        {

            if (!(ConfigurationManager.AppSettings["UserByPass"] == ""))
            {
                WhoIsLogged = ConfigurationManager.AppSettings["UserByPass"];
                return true;
            }

            WhoIsLogged = ProductionReport.GetCurrentUser();

            if (string.IsNullOrEmpty(WhoIsLogged))
            {
                ShowErrorMessageRequest.Raise(new Notification() { Content = new ShowError("Error", string.Format("No se pudo iniciar sesión en el sistema. Operación cancelada")) });
                return false;
            }
                
            return true;

        }



        public bool IsSended(ReportProductionDto reportDto)
        {
            bool bypass = ConfigurationManager.AppSettings["Bypass"] == "true";
            if (!(reportDto.Enviado == Enumerations.AxlrBit.No || bypass))
            {
                ShowError showError = new ShowError("Error", string.Format("Este reporte ya ha sido enviado. Operación cancelada"));
                ShowErrorMessageRequest.Raise(new Notification() { Content = showError });
                return false;
            }
            return true;
        }



        public void SetNotifications(InteractionRequest<Notification> request, InteractionRequest<Notification> IndBoxReportConfirmationRequest,
            InteractionRequest<Notification> showErrorMessageRequest, InteractionRequest<Notification> showMessageRequest,
            InteractionRequest<Notification> showQuestionRequests)
        {
            this.Request = request;
            this.IndBoxReportConfirmationRequest = IndBoxReportConfirmationRequest;
            this.ShowErrorMessageRequest = showErrorMessageRequest;
            this.ShowMessageRequest = showMessageRequest;
            this.ShowQuestionRequests = showQuestionRequests;
        }



        public bool IsReportSequenceValidated(IList<GeneralPiece> generalPieces, GeneralPiece currentGeneralPice)
        {
            if (!ValidationRules.ValidateReportSequence(generalPieces, currentGeneralPice))
            {
                ShowError showError = new ShowError("Error", string.Format("Debe reportar el parcial anterior. Operación cancelada"));
                ShowErrorMessageRequest.Raise(new Notification() { Content = showError });
                return false;
            }
            return true;
        }








    }
}
