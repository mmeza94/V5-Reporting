using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tenaris.Fava.Production.Reporting.Model.Adapter;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.Model;
using Tenaris.Fava.Production.Reporting.Model.Support;
using Tenaris.Fava.Production.Reporting.ViewModel.Dialog;
using Tenaris.Library.UI.Framework.ViewModel;

namespace Tenaris.Fava.Production.Reporting.ViewModel.Support
{
    public class ReportConfirmationSupport
    {
        GeneralPiece currentGeneralPieceDto;
        ReportProductionDto currentProductionReportDto;
        ReportProductionDto productionReportDto;
        public static IList rejectionReportDetails;
        private static string user;
        public bool canReportOnConfirm = false;
        public static bool ReportConfirmation(int tbGoodCountL2, int tbScrapCountL2, int tbReworkedCountL2, int tbLoadedCountL2, string extremo, int epProductionReport
            , IList rejectionReportDetails, ObservableCollection<RejectionReportDetail> dgRejectionReportDetails, ReportProductionDto currentProductionReportDto
            , int lbITLoadHelper, int tbTotalLoaded, string SelectedSendType, int tbPreviousLoaded, string User, InteractionRequest<Notification> ShowQuestionRequest, InteractionRequest<Notification> showMessageRequest, InteractionRequest<Notification> showErrorWindowRequest)
        {
            user = User;
            if (ValidationRules.ValidateCountersForProductionReport(tbGoodCountL2,
                tbScrapCountL2, tbReworkedCountL2, tbLoadedCountL2, epProductionReport,
                GetSendStatus(SelectedSendType)))
            {
                if (ValidationRules.ValidateRejectionReasons(tbScrapCountL2,
                    rejectionReportDetails, epProductionReport, dgRejectionReportDetails, showErrorWindowRequest))
                {
                    if(!Configurations.Instance.Machine.Contains("Forjadora"))
                    {
                        extremo = null;
                    }
                    var confirmMessage = string.Format("Resumen de lo Reportado: \n\n Buenas:{0} \n" +
                        " Malas:{1} \n Reprocesos:{2} \n Total:{3} \n {4} \n\n ¿Desea reportar estas cantidades?", tbGoodCountL2,
                        tbScrapCountL2, tbReworkedCountL2, tbLoadedCountL2, extremo);
                    //MessageBox.Show(confirmMessage, "Confirmar Reporte", MessageBoxButtons.OKCancel) == DialogResult.OK
                    ShowQuestion showQuestion = new ShowQuestion("Confirmar reporte", confirmMessage);
                    ShowQuestionRequest.Raise(new Notification() { Content = showQuestion });
                    if (showQuestion.Result)
                    {

                        currentProductionReportDto = PrepareDtoForProductionReport(currentProductionReportDto, lbITLoadHelper, tbPreviousLoaded,
           tbLoadedCountL2, tbGoodCountL2, tbScrapCountL2, tbReworkedCountL2, user);
                        ITServiceAdapter iTServiceAdapter = new ITServiceAdapter();



                        int versionInt = Convert.ToInt32(Configurations.Instance.VersionApplication.Replace("V", ""));
                        if (Configurations.Instance.Machine == "Granalladora" && Configurations.Instance.Secuencia == "8")
                        {
                            versionInt = 3;
                        }
                        var response = iTServiceAdapter.ReportProduction(user,
                            currentProductionReportDto, GetSendStatus(SelectedSendType),
                                true, rejectionReportDetails);
                        
                        ShowMessage showMessage = new ShowMessage("Reporte de Producción", response);
                        showMessageRequest.Raise(new Notification() { Content = showMessage });
                        if (!Configurations.Instance.Machine.ToUpper().Equals("CND"))
                        {
                            CheckReportProductionForNextOperation(response, currentProductionReportDto, lbITLoadHelper, tbTotalLoaded, showMessageRequest);
                        }
                    }

                }
            }

            return false;
        }

        public static void CheckReportProductionForNextOperation(string response, ReportProductionDto currentProductionReportDto, int lbITLoadHelper,
            int tbTotalLoaded, InteractionRequest<Notification> showMessageRequest)
        {
            if (response.StartsWith("Reporte Enviado Correctamente"))
            {
                var message = "";
                var numeroOperacionsiguiente = currentProductionReportDto.Secuencia + 1;
                var operation = ConfigurationManager.AppSettings.Get("Operation_" + (numeroOperacionsiguiente).ToString());
                var isAvailable = new ITConnFacade().IsGroupItemAvailableForNextOperation(currentProductionReportDto.IdUDT,
                    currentProductionReportDto.Colada, currentProductionReportDto.Orden, currentProductionReportDto.Secuencia);
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
                showMessageRequest.Raise(new Notification() { Content = showMessage });


            }
        }

        public static Enumerations.ProductionReportSendStatus GetSendStatus(string SelectedSendType)
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
                default: break;


            }
            return sendStatus;
        }



        public static ReportProductionDto PrepareDtoForProductionReport(ReportProductionDto currentProductionReportDto, int lbITLoadHelper, int tbPreviousLoaded,
            int tbLoadedCountL2, int tbGoodCountL2, int tbScrapCountL2, int tbReworkedCountL2, string user)
        {
            if (Configurations.Instance.Machine.ToUpper().Equals("CND"))
            {
                currentProductionReportDto.CantidadTotal = lbITLoadHelper;

            }
            else
            {
                //currentProductionReportDto.CantidadTotal = int.Parse(tbLoadedCountL2.Text);// Corrección para prevenir cantidad a cargar por parciales y reporte a IT
                currentProductionReportDto.CantidadTotal =
                    (lbITLoadHelper - tbPreviousLoaded == 0) ?
                    tbLoadedCountL2 :
                    lbITLoadHelper - tbPreviousLoaded;
            }
            currentProductionReportDto.CantidadMalas = tbScrapCountL2;
            currentProductionReportDto.CantidadBuenas = tbGoodCountL2;
            currentProductionReportDto.CantidadReprocesadas = tbReworkedCountL2;
            currentProductionReportDto.IdUser = user;
            return currentProductionReportDto;

        }




        public static RejectionReportDetail btnAddRejectionDetail_Click(int tbScrapCountForRejection, RejectionCode SelectedRejectionCode, string DestinationSelectedItem,
            string motivo, bool worked, bool extremo1)
        {
            if (rejectionReportDetails == null)
                rejectionReportDetails = new List<RejectionReportDetail>();


            short cantidad = (short)tbScrapCountForRejection;
            var rejectionCode = SelectedRejectionCode;
            var rejectionReportDetail = new RejectionReportDetail();
            if (Configurations.Instance.Secuencia == "8")
            {
                var TEMrejectionReportDetail = new RejectionReportDetail
                {
                    RejectionCode = rejectionCode,
                    ScrapCount = cantidad,
                    Active = Enumerations.AxlrBit.Si,
                    InsDateTime = DateTime.Now,
                    Destino = DestinationSelectedItem,
                    Observation = motivo,
                    Trabajado = (worked) ? Enumerations.AxlrBit.Si : Enumerations.AxlrBit.No,
                    //Extremo = (extremo1) ? Resources.Resources.ext1.Trim() : Resources.Resources.ext2.Trim()
                    Extremo = extremo1 ? "Extremo 1" : "Extremo 2"
                };
                rejectionReportDetail = TEMrejectionReportDetail;
            }
            else
            {
                var TEMrejectionReportDetail = new RejectionReportDetail
                {
                    RejectionCode = rejectionCode,
                    ScrapCount = cantidad,
                    Active = Enumerations.AxlrBit.Si,
                    InsDateTime = DateTime.Now,
                    Destino = DestinationSelectedItem,
                    Observation = motivo,
                    Trabajado = worked ? Enumerations.AxlrBit.Si : Enumerations.AxlrBit.No
                };
                rejectionReportDetail = TEMrejectionReportDetail;
            }

            //bool alreadyAdded = false;
            ////foreach (RejectionReportDetail reportRejDet in rejectionReportDetails)
            ////{
            ////    if (reportRejDet.RejectionCode.Id == rejectionCode.Id &&
            ////        reportRejDet.Trabajado == ((worked) ? Enumerations.AxlrBit.Yes : Enumerations.AxlrBit.No) &&
            ////        (reportRejDet.Extremo == ((extremo1) ? "Extremo 1" : "Extremo 2") || reportRejDet.Extremo == null)
            ////        )
            ////    {
            ////        reportRejDet.ScrapCount += cantidad;
            ////        alreadyAdded = true;
            ////    }
            ////}
            //if (!alreadyAdded)
            //    rejectionReportDetails.Add(rejectionReportDetail);
            //var scrapCount = 0;
            //foreach (RejectionReportDetail rrd in rejectionReportDetails)
            //    scrapCount += rrd.ScrapCount;
            //lblNoDetails.Text = string.Format("No. Detalles: {0}", scrapCount);
            return rejectionReportDetail;

        }

    }
}
