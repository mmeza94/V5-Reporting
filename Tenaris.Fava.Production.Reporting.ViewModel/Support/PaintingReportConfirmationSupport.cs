using log4net;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.Support;
using Tenaris.Fava.Production.Reporting.ViewModel.Dialog;

namespace Tenaris.Fava.Production.Reporting.ViewModel.Support
{
    public class PaintingReportConfirmationSupport
    {
        public static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //private void ShowHideOperatorCounters()
        //{
        //    //this.tbLoadedCount.Visible = chBEdit.Checked;
        //    this.tbGoodCountL2.Enabled = chBEdit.Checked;
        //    //this.tbScrapCountL2.Enabled = chBEdit.Checked;
        //    //this.tbReworkedCountL2.Enabled = chBEdit.Checked;
        //    //this.cbSendType.Enabled = chBEdit.Checked;
        //    this.tbLoadedCountL2.Enabled = chBEdit.Checked;
        //    //this.tbChangeReason.Visible = chBEdit.Checked;
        //    //this.tbChangeReason.Text = "";
        //    //this.lblChangeReason.Visible = chBEdit.Checked;
        //    this.lbITLoadHelper.Enabled = chBEdit.Checked;
        //    PopulateLevel2Counters();
        //}


        static PaintingReport currentProductionReportDto;
        public static void Report(int DisponiblesTPS, int CargadasAnterior, int BuenasActual, int MalasActual,
            int ReprocesosActual, int CargadasActual, PaintingReport currentProductionReportDto2, 
            ObservableCollection<RejectionReportDetail> rejectionReportDetails, string UserReport,
            InteractionRequest<Notification> QuestionRequest, InteractionRequest<Notification> MessageRequest, InteractionRequest<Notification> ErroRequest)
        {
            currentProductionReportDto = currentProductionReportDto2;

            try
            {
                var confirmMessage = string.Format("Resumen de lo Reportado: \n\n Buenas:{0} \n" + " Malas:{1} \n Reprocesos:{2} \n Total:{3} \n\n ¿Desea reportar estas cantidades?"
                                            , BuenasActual, MalasActual, ReprocesosActual, CargadasActual);
                ShowQuestion showQuestion = new ShowQuestion("Confirmar Reporte", confirmMessage);
                QuestionRequest.Raise(new Notification() {Content = showQuestion });
                if (showQuestion.Result)
                {
                    ////if (chBEdit.Checked)
                    currentProductionReportDto2=PrepareDtoForProductionReport(currentProductionReportDto2, DisponiblesTPS, CargadasAnterior, BuenasActual, MalasActual, CargadasActual, UserReport);
                    var response = new ProductionReport().ReportProductionForPainting(currentProductionReportDto, Enumerations.ProductionReportSendStatus.Parcial, true, rejectionReportDetails);
                    ShowMessage showMessage = new ShowMessage("Reporte de Producción", response);
                    MessageRequest.Raise(new Notification() {Content = showMessage });

                    ////CheckReportProductionForNextOperation(response);
                    //this.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error("btnReport_Click", ex);
            }
        }


        public static PaintingReport PrepareDtoForProductionReport(PaintingReport currentReportProduction,int DisponiblesTPS, int CargadasAnterior, int BuenasActual, int MalasActual, int CargadasActual, string UserReport)
        {
            //currentProductionReportDto.CantidadTotal = int.Parse(tbLoadedCountL2.Text);// Corrección para prevenir cantidad a cargar por parciales y reporte a IT
            var cantidadTotal = DisponiblesTPS - ((CargadasAnterior == 0) ? 0 : CargadasAnterior);
            currentProductionReportDto.LoadQuantity = (cantidadTotal <= 0) ? CargadasActual : cantidadTotal;
            currentProductionReportDto.GoodCount = BuenasActual;
            currentProductionReportDto.ScrapCount = MalasActual;
            //currentProductionReportDto.CantidadReprocesadas = int.Parse(tbReworkedCountL2.Text);
            currentProductionReportDto.IdUser = UserReport;

            //ReportProductionDtoToSend = new ReportProductionDto();
            //ReportProductionDtoToSend.CantidadTotal = currentProductionReportDto.LoadQuantity;
            //ReportProductionDtoToSend.CantidadBuenas = currentProductionReportDto.GoodCount;
            //ReportProductionDtoToSend.CantidadMalas = currentProductionReportDto.ScrapCount;
            //ReportProductionDtoToSend.CantidadReprocesadas = 0;
            //ReportProductionDtoToSend.IdUser = currentProductionReportDto.IdUser;
            return currentReportProduction;

        }

    }
}
