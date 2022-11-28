using log4net;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.Adapter;
using Tenaris.Fava.Production.Reporting.Model.Business;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.Model;

using Tenaris.Fava.Production.Reporting.Model.Support;
using Tenaris.Fava.Production.Reporting.ViewModel.Dialog;
using Tenaris.Library.Log;
using Tenaris.Library.UI.Framework.ViewModel;
using Tenaris.Library.UI.Framework.Language;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
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
            if (currentDGRow == null)
                return null;

            return new ReportProductionDto()
            {
                TipoUDT = string.IsNullOrEmpty(currentDGRow.GroupItemType) ? "Tarjeta de Linea" : currentDGRow.GroupItemType,
                IdBatch = currentDGRow.IdBatch,
                IdHistory = currentDGRow.IdHistory,
                Orden = currentDGRow.OrderNumber,
                Almacen = currentDGRow.Location,
                IdUDT = currentDGRow.GroupItemNumber,
                Colada = currentDGRow.HeatNumber,
                Lote = currentDGRow.LotNumberHTR,
                Aprietes = 0,
                DescripcionMaquina = currentDGRow.Description,
                CantidadMalas = currentDGRow.ScrapCount,
                CantidadBuenas = currentDGRow.GoodCount,
                CantidadReprocesadas = currentDGRow.ReworkedCount,
                Enviado = currentDGRow.Sended,
                CantidadTotal = currentDGRow.LoadedCount,
                Secuencia = Convert.ToInt32(Configurations.Instance.Secuencia),
                Operacion = Configurations.Instance.Operacion,
                Opcion = Configurations.Instance.Opcion
            };

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
            if (reportDto == null)
                return false;

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


        public int GetFirstPieceLoadedNumberForIT(GeneralPiece GeneralPiece)
        {
            int FirstReportedLoadedCount = currentGeneralPieces.Where(c =>(c.GroupItemNumber == GeneralPiece.GroupItemNumber)
                                           && (c.ReportSequence == 1) && (c.Extremo == GeneralPiece.Extremo)).First().LoadedCount;

            return FirstReportedLoadedCount;
        }






    }
}
