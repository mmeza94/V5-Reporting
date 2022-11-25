using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenaris.Fava.Production.Reporting.Model.Business
{
    public class IndBoxReportConfirmation
    {
        //GeneralPieceDto currentGeneralPieceDto;
        //ReportProductionDto currentProductionReportDto;
        ////ReportProductionDto productionReportDto;
        //List<RejectionReportDetail> rejectionReportDetails;
        //ProductionBox selectedBox;
        //private string user;
        //private int indiceTablaBox = -1;

        //public IndBoxReportConfirmation()
        //{
        //    InitializeComponent();
        //}

        //public IndBoxReportConfirmation(GeneralPieceDto generalPieceDto, ReportProductionDto productionReportDto, string user)
        //{
        //    InitializeComponent();
        //    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;

        //    this.currentGeneralPieceDto = generalPieceDto;
        //    this.currentProductionReportDto = productionReportDto;
        //    this.user = user;
        //    this.rejectionReportDetails = new List<RejectionReportDetail>();

        //    OPChildrens opHijaespecificacion = null;
        //    tbOrder.Text = this.currentGeneralPieceDto.OrderNumber.ToString();
        //    tbHeat.Text = this.currentGeneralPieceDto.HeatNumber.ToString();
        //    tbGroupItem.Text = this.currentGeneralPieceDto.GroupItemNumber.ToString();


        //    List<ProductionBox> listBoxes = new List<ProductionBox>();
        //    //string machineId = ConfigurationManager.AppSettings["Opcion"];
        //    //string operationId = ConfigurationManager.AppSettings["Operacion"];

        //    ProductionBoxReport business = new ProductionBoxReport();

        //    try
        //    {
        //        string errorMessage;

        //        listBoxes = business.GetProductionBoxes(this.currentGeneralPieceDto.OrderNumber, productionReportDto.Opcion, productionReportDto.Operacion, out errorMessage);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }

        //    //OCULTA LAS CAJAS QUE NO HAN SIDOI SELECCIONADAS EN GUIA PRODUCCION
        //    //listBoxes = fitroDeCajas(listBoxes, this.currentGeneralPieceDto.OrderNumber);

        //    if (listBoxes == null)
        //    {
        //        listBoxes = new List<ProductionBox>();
        //    }

        //    listBoxes = listBoxes.OrderBy(b => b.MissingPieces).ThenBy(b => b.Id).ToList();
        //    dgBoxes.AutoGenerateColumns = false;
        //    dgBoxes.DataSource = listBoxes;
        //    dgBoxes.SelectionChanged += new EventHandler(dgBoxes_SelectionChanged);
        //    PopulateRejectionCodeByMachineDescription();
        //    cbDestiny.SelectedIndex = 0;
        //    //Consultar OP HIJA
        //    opHijaespecificacion = Data.DataAccess.GetNextOpChildrenActive(this.currentGeneralPieceDto.OrderNumber);
        //    //identifica en el grid su pocicion para posterione mente marcar esa pieza para marcar
        //    if (listBoxes.Count > 0)
        //    {
        //        string idBoxSelect = Data.DataAccess.BoxSelect(this.currentGeneralPieceDto.OrderNumber);
        //        foreach (DataGridViewRow row in dgBoxes.Rows)
        //        {
        //            if (row.Cells[0].Value.ToString().Equals(idBoxSelect))
        //            {
        //                indiceTablaBox = row.Index;
        //            }
        //        }
        //    }

        //    txtOPHija.Text = opHijaespecificacion != null ? opHijaespecificacion.NumeroOrder.ToString() : string.Empty;
        //    txtCabezal.Text = opHijaespecificacion != null ? opHijaespecificacion.Cabezal : string.Empty;
        //    txtCentralizado.Text = opHijaespecificacion != null ? opHijaespecificacion.Centralizado : string.Empty;
        //    txtCople.Text = opHijaespecificacion != null ? opHijaespecificacion.Cople : string.Empty;

        //    txtBuenas.Text = currentGeneralPieceDto.GoodCount.ToString();
        //    txtMalas.Text = "0";
        //    txtReprocesos.Text = currentGeneralPieceDto.ReworkedCount.ToString();
        //    txtTotal.Text = txtBuenas.Text;
        //    tbScrapCountForRejection.Text = currentGeneralPieceDto.ScrapCount.ToString();

        //    int total1 = GetPreviousTotal("Extremo 1");
        //    int total2 = GetPreviousTotal("Extremo 2");
        //    int total = total1 - total2;

        //    if (total < 0)
        //    {
        //        total = 0;
        //    }

        //    currentGeneralPieceDto.LoadedCount = total;
        //    currentProductionReportDto.CantidadTotal = total;

        //    txtTotalActualAtado.Text = total.ToString();
        //}

        //void dgBoxes_SelectionChanged(object sender, EventArgs e)
        //{
        //    if (dgBoxes.SelectedRows.Count > 0)
        //    {
        //        selectedBox = dgBoxes.SelectedRows[0].DataBoundItem as ProductionBox;
        //    }
        //    else
        //    {
        //        selectedBox = null;
        //    }
        //}

        //private void PopulateRejectionCodeByMachineDescription()
        //{
        //    try
        //    {
        //        cbRejectionCode.DataSource = new RejectionCodeFacade().
        //            GetRejectionCodeByMachineDescription(currentGeneralPieceDto.Description);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //private void btnReport_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (selectedBox != null)
        //        {
        //            int goodPieces = Int32.Parse(txtBuenas.Text);
        //            int workedPieces = Int32.Parse(txtTotal.Text);
        //            int reworkedPieces = Int32.Parse(txtReprocesos.Text);
        //            int totalPieces = Int32.Parse(txtTotalActualAtado.Text);

        //            currentGeneralPieceDto.LoadedCount = totalPieces;
        //            currentProductionReportDto.CantidadTotal = totalPieces;

        //            if (workedPieces > totalPieces)
        //            {
        //                MessageBox.Show(string.Format("El Total de Piezas Procesadas ({0}) debe ser menor o igual al número de Piezas Disponibles en el Atado ({1})",
        //                    workedPieces, totalPieces), "Reporte de Producción", MessageBoxButtons.OK, MessageBoxIcon.Error);

        //                return;
        //            }

        //            //if (workedPieces <= selectedBox.MissingPieces)
        //            if (goodPieces <= selectedBox.MissingPieces)
        //            {
        //                List<ProductionBox> listUsed = (dgBoxes.DataSource as List<ProductionBox>).Where(l => l.LoadedPieces > 0).ToList();

        //                //AGREGAR VALIDACION PARA QUE LA CAJA SELECIONADA SEA LA QUES ESTE MARACADA POR EL MODULO GUIA DE PRODUCCION
        //                if (!isSelectBox(this.currentGeneralPieceDto.OrderNumber, selectedBox.Id))
        //                {
        //                    string confirmBox2 = string.Format("La caja {0} no corresponde a la selecionada en Guia de Produccion, o no se ha marcado ninguna en la Guia de Produccion.\nSeleccione la caja Correspondiente\n", selectedBox.Id);

        //                    if (MessageBox.Show(confirmBox2, "Confirmar Caja", MessageBoxButtons.OK) == DialogResult.OK)
        //                    {
        //                        return;
        //                    }

        //                }

        //                //LE INFORMA AL OPERADOR QUE HA DEJADO CAJAS PENDIENTES
        //                if (listUsed.Count > 0 && !listUsed.Contains(selectedBox))
        //                {
        //                    string confirmBox = string.Format("Ha seleccionado una caja diferente a las cajas que estan pendientes de llenar.\nEsta seguro que quiere seleccionar esta caja?\nCaja: {0}",
        //                        selectedBox.Id);

        //                    if (MessageBox.Show(confirmBox, "Confirmar Caja", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
        //                    {
        //                        return;
        //                    }
        //                }

        //                var confirmMessage = string.Format("Resumen de lo Reportado: \n\n Buenas:{0} \n" +
        //                    " Malas:{1} \n Reprocesos:{2} \n Total:{3} \n Caja:{4} \n\n ¿Desea reportar estas cantidades?",
        //                    txtBuenas.Text, txtMalas.Text, txtReprocesos.Text, txtTotal.Text, selectedBox.Id);

        //                if (MessageBox.Show(confirmMessage, "Confirmar Reporte", MessageBoxButtons.OKCancel) == DialogResult.OK)
        //                {
        //                    string errorMessage = string.Empty;
        //                    int idN2;
        //                    bool resultLoad;
        //                    ProductionBoxReport report = new ProductionBoxReport();

        //                    int discardPieces = rejectionReportDetails.Sum(d => d.ScrapCount);

        //                    //HARCODE
        //                    //bool bypass = false;
        //                    //bool result = true;
        //                    //if (bypass)
        //                    //{

        //                    //if (selectedBox.IdN2 == 0)
        //                    //{
        //                    //resultLoad = report.LoadProductionBox(out idN2, selectedBox.Id, selectedBox.ParentOrderNumber, selectedBox.OrderNumber, selectedBox.MachineId, selectedBox.OperationId,
        //                    //    selectedBox.Type, currentProductionReportDto.Secuencia, selectedBox.MaxPieces, selectedBox.LoadedPieces, selectedBox.MissingPieces, out errorMessage);
        //                    resultLoad = report.LoadProductionBox(out idN2, selectedBox.Id, selectedBox.ParentOrderNumber, selectedBox.OrderNumber, selectedBox.MachineId, selectedBox.OperationId,
        //                        selectedBox.Type, currentProductionReportDto.Secuencia, selectedBox.MissingPieces, currentProductionReportDto.TipoUDT, currentProductionReportDto.IdUDT.ToString(),
        //                        currentProductionReportDto.Colada, currentProductionReportDto.Lote, goodPieces, reworkedPieces, discardPieces, currentGeneralPieceDto.LoadedCount, tbChangeReason.Text,
        //                        user, currentProductionReportDto.DescripcionMaquina, currentProductionReportDto.Almacen, currentProductionReportDto.IdHistory, out errorMessage);

        //                    //    selectedBox.IdN2 = idN2;
        //                    //}
        //                    //else
        //                    //{
        //                    //    resultLoad = true;
        //                    //}

        //                    //if (resultLoad)
        //                    //{
        //                    //int totalPieces = goodPieces + discardPieces;

        //                    bool result = report.ReportProductionBox(selectedBox.IdN2, currentProductionReportDto.TipoUDT, currentProductionReportDto.IdUDT.ToString(), currentProductionReportDto.Orden,
        //                        currentProductionReportDto.Colada, currentProductionReportDto.Lote, currentProductionReportDto.Secuencia, selectedBox.OperationId, selectedBox.MachineId, goodPieces, workedPieces,
        //                        reworkedPieces, currentProductionReportDto.CantidadTotal, currentProductionReportDto.ColadaSalida, Int32.Parse(txtOPHija.Text), selectedBox.Id, currentProductionReportDto.LoteSalida, tbChangeReason.Text,
        //                        selectedBox.Type, user, currentProductionReportDto.DescripcionMaquina, currentProductionReportDto.Almacen, currentProductionReportDto.IdHistory,
        //                        rejectionReportDetails.ToArray(), out errorMessage);
        //                    //}
        //                    //VERIFICAR SI LA CAJA QUE ACABAN DE REPORTAR YA SE ENCUENTRA COMPLETA CON LA QUERY DE IT
        //                    this.verificaBoxCompeteIT(this.currentGeneralPieceDto.OrderNumber, selectedBox.Id);

        //                    if (result)
        //                    {
        //                        MessageBox.Show("Reporte de Producción en Caja realizado correctamente!", "Reporte de Producción", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //                        this.Close();
        //                    }
        //                    else //Descargar la caja, de lo contrario se queda cargada en TPS y ya no le aparece al operador en la lista de cajas disponibles para seleccionar
        //                    {
        //                        string errorUnload;

        //                        bool unload = report.UnloadProductionBox(selectedBox.IdN2, selectedBox.Id, selectedBox.MachineId, selectedBox.OperationId, selectedBox.MissingPieces,
        //                            currentProductionReportDto.Secuencia, selectedBox.Type, out errorUnload);

        //                        MessageBox.Show("No se pudo realizar el Reporte de Producción: " + errorMessage, "Reporte de Producción", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    }
        //                    //}
        //                    //else
        //                    //{
        //                    //    MessageBox.Show("No se pudo realizar la Carga de Material: " + errorMessage, "Reporte de Producción", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    //}
        //                }
        //            }
        //            else
        //            {
        //                MessageBox.Show(string.Format("El Total de Piezas Buenas ({0}) debe ser menor o igual al número de Piezas Disponibles en la Caja Seleccionada ({1})",
        //                    goodPieces, selectedBox.MissingPieces), "Reporte de Producción", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("No ha seleccionado una Caja", "Reporte de Producción", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error: " + ex.Message, "Reporte de Producción", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    Close();
        //}

        //private void btnAddRejectionDetail_Click(object sender, EventArgs e)
        //{
        //    if (ValidationRules.CheckForValidIntegers(new List<TextBox> { tbScrapCountForRejection }, epProductionReport, true))
        //    {
        //        short cantidad = short.Parse(tbScrapCountForRejection.Text);
        //        var rejectionCode = (RejectionCode)cbRejectionCode.SelectedItem;
        //        var rejectionReportDetail = new RejectionReportDetail();
        //        //if (radButExtreme1.Visible && radButExtreme2.Visible)
        //        //{
        //        var TEMrejectionReportDetail = new RejectionReportDetail
        //        {
        //            RejectionCode = rejectionCode,
        //            ScrapCount = cantidad,
        //            Active = Enumerations.AxlrBit.Yes,
        //            InsDateTime = DateTime.Now,
        //            Destino = cbDestiny.SelectedItem.ToString().Trim(),
        //            Observation = tbMotivo.Text,
        //            Trabajado = (chbWorked.Checked) ? Enumerations.AxlrBit.Yes : Enumerations.AxlrBit.No,
        //            Extremo = (radButExtreme1.Checked) ? Resources.Resources.ext1.Trim() : Resources.Resources.ext2.Trim()
        //        };
        //        rejectionReportDetail = TEMrejectionReportDetail;
        //        //}
        //        //else
        //        //{
        //        //    var TEMrejectionReportDetail = new RejectionReportDetail
        //        //    {
        //        //        RejectionCode = rejectionCode,
        //        //        ScrapCount = cantidad,
        //        //        Active = Enumerations.AxlrBit.Yes,
        //        //        InsDateTime = DateTime.Now,
        //        //        Destino = cbDestiny.SelectedItem.ToString().Trim(),
        //        //        Observation = tbMotivo.Text,
        //        //        Trabajado = (chbWorked.Checked) ? Enumerations.AxlrBit.Yes : Enumerations.AxlrBit.No
        //        //    };
        //        //    rejectionReportDetail = TEMrejectionReportDetail;
        //        //}

        //        bool alreadyAdded = false;

        //        foreach (RejectionReportDetail reportRejDet in rejectionReportDetails)
        //        {
        //            if (reportRejDet.RejectionCode.Id == rejectionCode.Id &&
        //                reportRejDet.Trabajado == ((chbWorked.Checked) ? Enumerations.AxlrBit.Yes : Enumerations.AxlrBit.No) &&
        //                (reportRejDet.Extremo == ((radButExtreme1.Checked) ? radButExtreme1.Text.Trim() : radButExtreme2.Text.Trim()) || reportRejDet.Extremo == null)
        //                )
        //            {
        //                reportRejDet.ScrapCount += cantidad;
        //                alreadyAdded = true;
        //            }
        //        }

        //        if (!alreadyAdded)
        //        {
        //            rejectionReportDetails.Add(rejectionReportDetail);
        //        }

        //        dgRejectionReportDetails.AutoGenerateColumns = false;
        //        dgRejectionReportDetails.DataSource = "";
        //        dgRejectionReportDetails.DataSource = rejectionReportDetails;

        //        var scrapCount = 0;

        //        foreach (RejectionReportDetail rrd in rejectionReportDetails)
        //        {
        //            scrapCount += rrd.ScrapCount;
        //        }

        //        txtMalas.Text = scrapCount.ToString();
        //        SumCounters();

        //        lblNoDetails.Text = string.Format("No. Detalles: {0}", scrapCount);

        //        tbScrapCountForRejection.Text = "0";
        //        cbDestiny.SelectedIndex = 0;
        //        cbRejectionCode.SelectedIndex = 0;
        //    }
        //}

        //private void btnRemoveRejectionDetail_Click(object sender, EventArgs e)
        //{
        //    if (dgRejectionReportDetails.CurrentRow != null)
        //    {
        //        var rejectionReportDetail = (RejectionReportDetail)dgRejectionReportDetails.CurrentRow.DataBoundItem;

        //        dgRejectionReportDetails.DataSource = "";
        //        rejectionReportDetails.Remove(rejectionReportDetail);
        //        dgRejectionReportDetails.DataSource = rejectionReportDetails;

        //        var scrapCount = 0;

        //        foreach (RejectionReportDetail rrd in rejectionReportDetails)
        //        {
        //            scrapCount += rrd.ScrapCount;
        //        }

        //        txtMalas.Text = scrapCount.ToString();
        //        SumCounters();

        //        lblNoDetails.Text = string.Format("No. Detalles: {0}", scrapCount);
        //    }
        //}

        //private void SumCounters()
        //{
        //    if (ValidationRules.CheckForValidIntegers(new List<TextBox>{txtBuenas,
        //      txtMalas}, epProductionReport, true))
        //    {
        //        txtTotal.Text = (int.Parse(txtBuenas.Text) + int.Parse(txtMalas.Text)).ToString();
        //    }
        //}

        //private void txtBuenas_KeyUp(object sender, KeyEventArgs e)
        //{
        //    SumCounters();
        //}

        //private void txtReprocesos_KeyUp(object sender, KeyEventArgs e)
        //{
        //    SumCounters();
        //}

        //private int GetPreviousTotal(string extremo)
        //{
        //    var description = "";
        //    int total = 0;

        //    if (currentGeneralPieceDto.Description.Contains("Forjadora"))
        //        description = "Forjado";
        //    else if (currentGeneralPieceDto.Description.Contains("Roscadora"))
        //        description = "Mecanizado";
        //    else
        //        description = ConfigurationManager.AppSettings["Operacion"].ToString();

        //    int machineSequence = new ReportProductionHistoryFacade().GetPreviousSequenceByOperation(description + " " + extremo) + 1;

        //    IList reportItems = new ReportProductionHistoryFacade().GetReportProductionHistoryByParams(currentGeneralPieceDto.OrderNumber, currentGeneralPieceDto.GroupItemNumber,
        //        currentGeneralPieceDto.HeatNumber, null, null, machineSequence);

        //    foreach (ReportProductionHistory rph in reportItems)
        //    {
        //        if (extremo == "Extremo 1")
        //        {
        //            total += rph.GoodCount;//Solo se toman en cuenta las buenas del extremo 1, ya que las malas no se pueden reportar en la segunda estacion (extremo 2)
        //        }
        //        else if (extremo == "Extremo 2")
        //        {
        //            total += (rph.GoodCount + rph.ScrapCount);
        //        }
        //    }

        //    return total;
        //}

        //private void chkEdit_CheckedChanged(object sender, EventArgs e)
        //{
        //    txtTotalActualAtado.ReadOnly = !txtTotalActualAtado.ReadOnly;
        //    txtTotalActualAtado.Focus();
        //}

        ///// <summary>
        ///// verifica con la consaluta de oracle de IT, si la caja se ha completado, de ser asi actualiza DB SQL
        ///// </summary>
        ///// <param name="opMother"></param>
        ///// <param name="idbox"></param>
        //private void verificaBoxCompeteIT(int opMother, string idbox)
        //{
        //    try
        //    {
        //        List<BoxProducctionIT> listBoXIT = DataAccessOracle.GetBoxesByWOMother(opMother);

        //        BoxProducctionIT box = listBoXIT.First(x => x.IdBox.Equals(idbox, StringComparison.OrdinalIgnoreCase));

        //        if (box.Status.Equals("C", StringComparison.OrdinalIgnoreCase))
        //        {
        //            //Cambia Status BOX
        //            Data.DataAccess.UpdBoxReportada(idbox);
        //            if (Convert.ToBoolean(ConfigurationManager.AppSettings["BloqueoActivo"]))
        //            {
        //                //Bloquea TAG Carga Tag para Bloqueo
        //                TagDefinicion tDef = new TagDefinicion();
        //                tDef.TagName = ConfigurationManager.AppSettings["TagName"].ToString();
        //                tDef.longitudTag = Convert.ToInt32(ConfigurationManager.AppSettings["LongitudTag"].ToString());
        //                tDef.posicionEscritura = Convert.ToInt32(ConfigurationManager.AppSettings["PosisionEscritura"].ToString());
        //                short bloqueoValorTEM = (short)Convert.ToUInt16(ConfigurationManager.AppSettings["ValorBloqueo"].ToString());
        //                short desbloqueoValorTEM = (short)Convert.ToUInt16(ConfigurationManager.AppSettings["ValorDesBloqueo"].ToString());
        //                tDef.maxIntConexionScada = Convert.ToInt32(ConfigurationManager.AppSettings["MaxIntConexionScada"].ToString());
        //                tDef.valorBloqueo = new short[tDef.longitudTag];
        //                tDef.valorBloqueo[tDef.posicionEscritura] = bloqueoValorTEM;
        //                tDef.valorDesBloqueo = new short[tDef.longitudTag];
        //                tDef.valorDesBloqueo[tDef.posicionEscritura] = desbloqueoValorTEM;

        //                ScadaAdquisidor sAdq = new ScadaAdquisidor(tDef);

        //                sAdq.inicializar();
        //                sAdq.activarOnliBlock();
        //                sAdq.bloquear();
        //                sAdq.desActivarOnliBlock();
        //                sAdq = null;
        //                tDef = null;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error: " + ex.Message, "Reporte de Producción", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        ///// <summary>
        ///// verifica si la caja ha sido selecionada en el la DB
        ///// </summary>
        ///// <param name="opMother"></param>
        ///// <param name="idBox"></param>
        ///// <returns>true si ha sido seleccionada, false si no</returns>
        //private bool isSelectBox(int opMother, string idBox)
        //{
        //    bool resul = Data.DataAccess.IsBoxSelect(opMother, idBox) == 1 ? true : false;
        //    return resul;
        //}

        ///// <summary>
        ///// filtra el listado de cajas para dejar ver solo la que se ha seleccionado por la guia de produccion
        ///// </summary>
        ///// <param name="lisBoxIT"></param>
        ///// <param name="orderMother"></param>
        ///// <returns></returns>
        //private List<ProductionBox> fitroDeCajas(List<ProductionBox> lisBoxIT, int orderMother)
        //{
        //    List<ProductionBox> lisCajas;
        //    try
        //    {
        //        string idBoxSelect = Data.DataAccess.BoxSelect(orderMother);
        //        lisCajas = lisBoxIT.FindAll(x => x.Id.Equals(idBoxSelect));
        //    }
        //    catch (Exception ex)
        //    {
        //        lisCajas = new List<ProductionBox>();
        //    }
        //    return lisCajas;
        //}

        ///// <summary>
        ///// manipuela el evento load de la paguina para marcar en el datagridViwer la casa selecionada sea la que esta indicada en la DB
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void IndBoxReportConfirmation_Load(object sender, EventArgs e)
        //{
        //    //cuando carge esta madre repintaremos el dgBoxes DataGridViwer del las Cajas
        //    if (indiceTablaBox != -1)
        //    {
        //        this.dgBoxes.Rows[indiceTablaBox].DefaultCellStyle.BackColor = Color.Yellow;
        //        this.dgBoxes.Rows[indiceTablaBox].DefaultCellStyle.ForeColor = Color.Red;
        //        this.dgBoxes.Rows[indiceTablaBox].Selected = true;
        //    }

        //}
    }
}
