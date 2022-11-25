using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenaris.Fava.Production.Reporting.Model.Business
{
    public class PaintingReportConfirmation
    {
        //ReportPaintingDto currentProductionReportDto;
        //ReportProductionDto ReportProductionDtoToSend;
        //IList rejectionReportDetails;
        //public static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //public PaintingReportConfirmation()
        //{
        //    InitializeComponent();
        //}

        //public PaintingReportConfirmation(ReportPaintingDto selectedReportPainting, string user)
        //{
        //    InitializeComponent();
        //    this.currentProductionReportDto = selectedReportPainting;
        //    rejectionReportDetails = new List<RejectionReportDetail>();
        //    PopulateLevel2Counters();
        //    this.UserReport.Text = user;
        //}

        //private void PopulateLevel2Counters()
        //{
        //    this.tbLoadedCountL2.Text = this.currentProductionReportDto.LoadQuantity.ToString();
        //    this.tbGoodCountL2.Text = this.currentProductionReportDto.GoodCount.ToString();
        //    this.tbScrapCountL2.Text = this.currentProductionReportDto.ScrapCount.ToString();
        //    this.tbReworkedCountL2.Text = "0";//this.currentProductionReportDto.ReworkedCount.ToString();
        //    this.tbGroupItem.Text = currentProductionReportDto.BoxUdt.ToString();
        //    this.tbHeat.Text = currentProductionReportDto.HeatNumber.ToString();
        //    this.tbOrder.Text = currentProductionReportDto.ChildOrden.ToString();
        //    this.lbITLoadHelper.Text = this.currentProductionReportDto.LoadQuantity.ToString();
        //    //SetSendStatus();
        //}

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

        //private void chBEdit_CheckedChanged(object sender, EventArgs e)
        //{
        //    ShowHideOperatorCounters();
        //}


        //#region KeyPress
        //private void tbGoodCountL2_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == (char)13)
        //    {
        //        SumCounters();
        //        tbScrapCountL2.Focus();
        //    }
        //}

        //private void tbScrapCountL2_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == (char)13)
        //    {
        //        SumCounters();
        //        tbReworkedCountL2.Focus();
        //    }
        //}

        //private void tbReworkedCountL2_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == (char)13)
        //    {
        //        SumCounters();
        //        tbLoadedCountL2.Focus();
        //    }
        //}


        //private void SumCounters()
        //{
        //    if (ValidationRules.CheckForValidIntegers(new List<TextBox>{tbGoodCountL2,
        //      tbScrapCountL2, tbReworkedCountL2, tbLoadedCountL2}, epProductionReport, true))
        //    {
        //        //if (GetSendStatus() == Enumerations.ProductionReportSendStatus.Complete)

        //        //else
        //        tbLoadedCountL2.Text = (int.Parse(tbGoodCountL2.Text) + int.Parse(string.IsNullOrEmpty(tbScrapCountL2.Text) ? "0" : tbScrapCountL2.Text)).ToString();
        //        tbTotalGood.Text = (int.Parse(tbGoodCountL2.Text) + int.Parse(string.IsNullOrEmpty(tbPreviousGood.Text) ? "0" : tbScrapCountL2.Text)).ToString();
        //        tbTotalScrap.Text = (int.Parse(tbScrapCountL2.Text) + int.Parse(string.IsNullOrEmpty(tbPreviousScrap.Text) ? "0" : tbScrapCountL2.Text)).ToString();
        //        tbTotalReworked.Text = (int.Parse(tbReworkedCountL2.Text) + int.Parse(string.IsNullOrEmpty(tbPreviousReworked.Text) ? "0" : tbScrapCountL2.Text)).ToString();
        //        tbTotalLoaded.Text = (int.Parse(tbLoadedCountL2.Text) + int.Parse(string.IsNullOrEmpty(tbPreviousLoaded.Text) ? "0" : tbScrapCountL2.Text)).ToString();

        //    }
        //}
        //#endregion

        //private void btnReport_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (ValidationRules.ValidateCountersForProductionReport(tbGoodCountL2, tbScrapCountL2, tbReworkedCountL2, tbLoadedCountL2, epProductionReport, Enumerations.ProductionReportSendStatus.Partial))
        //        {
        //            //if (ValidationRules.ValidateRejectionReasons(int.Parse(tbScrapCountL2.Text), rejectionReportDetails, epProductionReport, dgRejectionReportDetails))
        //            //{
        //            var confirmMessage = string.Format("Resumen de lo Reportado: \n\n Buenas:{0} \n" + " Malas:{1} \n Reprocesos:{2} \n Total:{3} \n\n ¿Desea reportar estas cantidades?"
        //                                                , tbGoodCountL2.Text, tbScrapCountL2.Text, tbReworkedCountL2.Text, tbLoadedCountL2.Text);
        //            if (MessageBox.Show(confirmMessage, "Confirmar Reporte", MessageBoxButtons.OKCancel) == DialogResult.OK)
        //            {
        //                //if (chBEdit.Checked)
        //                PrepareDtoForProductionReport();
        //                var response = new ProductionReport().ReportProductionForPainting(currentProductionReportDto, Enumerations.ProductionReportSendStatus.Partial, true, rejectionReportDetails);
        //                MessageBox.Show(response, "Reporte de Producción");

        //                //CheckReportProductionForNextOperation(response);
        //                this.Close();
        //            }

        //            //}
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("btnReport_Click", ex);
        //    }
        //}


        //private void PrepareDtoForProductionReport()
        //{
        //    //currentProductionReportDto.CantidadTotal = int.Parse(tbLoadedCountL2.Text);// Corrección para prevenir cantidad a cargar por parciales y reporte a IT
        //    var cantidadTotal = int.Parse(lbITLoadHelper.Text) - int.Parse(string.IsNullOrEmpty(tbPreviousLoaded.Text) ? "0" : tbPreviousLoaded.Text);
        //    currentProductionReportDto.LoadQuantity = (cantidadTotal <= 0) ? int.Parse(tbLoadedCountL2.Text) : cantidadTotal;
        //    currentProductionReportDto.GoodCount = int.Parse(tbGoodCountL2.Text);
        //    currentProductionReportDto.ScrapCount = int.Parse(tbScrapCountL2.Text);
        //    //currentProductionReportDto.CantidadReprocesadas = int.Parse(tbReworkedCountL2.Text);
        //    currentProductionReportDto.IdUser = this.UserReport.Text;

        //    //ReportProductionDtoToSend = new ReportProductionDto();
        //    //ReportProductionDtoToSend.CantidadTotal = currentProductionReportDto.LoadQuantity;
        //    //ReportProductionDtoToSend.CantidadBuenas = currentProductionReportDto.GoodCount;
        //    //ReportProductionDtoToSend.CantidadMalas = currentProductionReportDto.ScrapCount;
        //    //ReportProductionDtoToSend.CantidadReprocesadas = 0;
        //    //ReportProductionDtoToSend.IdUser = currentProductionReportDto.IdUser;


        //}
    }
}
