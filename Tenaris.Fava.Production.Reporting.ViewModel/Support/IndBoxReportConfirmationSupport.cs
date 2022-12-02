//using SimpleScadaCenexion;
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
using Tenaris.Fava.Production.Reporting.Model.Business;
using Tenaris.Fava.Production.Reporting.Model.Data_Access;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.Model;
using Tenaris.Fava.Production.Reporting.Model.Support;
using Tenaris.Fava.Production.Reporting.ViewModel.Dialog;
using Tenaris.Library.Log;
using Tenaris.Library.UI.Framework.ViewModel;

namespace Tenaris.Fava.Production.Reporting.ViewModel.Support
{
    public class IndBoxReportConfirmationSupport
    {
        



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

        public static List<RejectionCode> PopulateRejectionCodeByMachineDescription(string machineDescription)
        {
            try
            {


                var rejectionCode = new RejectionCodeFacade().
                        GetRejectionCodeByMachineDescription(machineDescription);



                return (List<RejectionCode>)rejectionCode;

            }
            catch (Exception ex)
            {

                Trace.Exception(ex);
                return null;
            }

        }

        public static bool BoxReportConfirmation(int buenas, int reprocesos, int total, int totalActualAtado, int malas,
            ProductionBox selectedBox, GeneralPiece currentGeneralPiece, ReportProductionDto currentProductionReport, int opHija, string changeReason,
            ObservableCollection<ProductionBox> dgBoxes, string user, InteractionRequest<Notification> questionRequest, InteractionRequest<Notification> messageRequest,InteractionRequest<Notification> errorMessageRequest)
        {
            ITServiceAdapter iTServiceAdapter = new ITServiceAdapter();
            try
            {
                if (selectedBox != null)
                {
                    int goodPieces = buenas;
                    int workedPieces = total;
                    int reworkedPieces = reprocesos;
                    int totalPieces = totalActualAtado;

                    currentGeneralPiece.LoadedCount = totalPieces;
                    currentProductionReport.CantidadTotal = totalPieces;

                    if (workedPieces > totalPieces)
                    {                       
                        ShowMessage showMessage = new ShowMessage("Reporte de Producción", string.Format("El Total de Piezas Procesadas ({0}) debe ser menor o igual al número de Piezas Disponibles en el Atado ({1})",
                            workedPieces, totalPieces));
                        messageRequest.Raise(new Notification() { Content = showMessage});
                        return false;
                    }

                    //if (workedPieces <= selectedBox.MissingPieces)
                    if (goodPieces <= selectedBox.MissingPieces)
                    {
                        List<ProductionBox> listUsed = dgBoxes.Where(l => l.LoadedPieces > 0).ToList();

                        //AGREGAR VALIDACION PARA QUE LA CAJA SELECIONADA SEA LA QUES ESTE MARACADA POR EL MODULO GUIA DE PRODUCCION

                        if (!isSelectBox(currentGeneralPiece.OrderNumber, selectedBox.Id))
                        {
                            string confirmBox2 = string.Format("La caja {0} no corresponde a la selecionada en Guia de Produccion, o no se ha marcado ninguna en la Guia de Produccion.\nSeleccione la caja Correspondiente\n \n¿Desea continuar aun asi?\n", selectedBox.Id);                            
                            ShowQuestion showQuestion = new ShowQuestion("Confirmar caja", confirmBox2);
                            questionRequest.Raise(new Notification() { Content = showQuestion });
                            if (!showQuestion.Result)
                            {
                                return false;
                            }

                        }

                        //LE INFORMA AL OPERADOR QUE HA DEJADO CAJAS PENDIENTES
                        if (listUsed.Count > 0 && !listUsed.Contains(selectedBox))
                        {
                            string confirmBox = string.Format("Ha seleccionado una caja diferente a las cajas que estan pendientes de llenar.\nEsta seguro que quiere seleccionar esta caja?\nCaja: {0}",
                                selectedBox.Id);
                            ShowQuestion showQuestion = new ShowQuestion("Confirmar caja", confirmBox);
                            questionRequest.Raise(new Notification() { Content = showQuestion });
                            //if (MessageBox.Show(confirmBox, "Confirmar Caja", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                            if (!showQuestion.Result)
                            {
                                return false;
                            }
                        }

                        var confirmMessage = string.Format("Resumen de lo Reportado: \n\n Buenas:{0} \n" +
                            " Malas:{1} \n Reprocesos:{2} \n Total:{3} \n Caja:{4} \n\n ¿Desea reportar estas cantidades?",
                            buenas, malas, reprocesos, total, selectedBox.Id);

                        //if (MessageBox.Show(confirmMessage, "Confirmar Reporte", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        ShowQuestion showQuestion2 = new ShowQuestion("Confirmar Reporte", confirmMessage);
                        questionRequest.Raise(new Notification() { Content = showQuestion2 });
                        if (showQuestion2.Result)
                        {
                            string errorMessage = string.Empty;
                            int idN2;
                            bool resultLoad;
                            ProductionBoxReport report = new ProductionBoxReport();

                            int discardPieces = rejectionReportDetails.Sum(d => d.ScrapCount);

                            //HARCODE
                            //bool bypass = false;
                            //bool result = true;
                            //if (bypass)
                            //{

                            //if (selectedBox.IdN2 == 0)
                            //{
                            //resultLoad = report.LoadProductionBox(out idN2, selectedBox.Id, selectedBox.ParentOrderNumber, selectedBox.OrderNumber, selectedBox.MachineId, selectedBox.OperationId,
                            //    selectedBox.Type, currentProductionReportDto.Secuencia, selectedBox.MaxPieces, selectedBox.LoadedPieces, selectedBox.MissingPieces, out errorMessage);
                            resultLoad = iTServiceAdapter.LoadProductionBox(out idN2, selectedBox.Id, selectedBox.ParentOrderNumber, selectedBox.OrderNumber, selectedBox.MachineId, selectedBox.OperationId,
                                selectedBox.Type, currentProductionReport.Secuencia, selectedBox.MissingPieces, currentProductionReport.TipoUDT, currentProductionReport.IdUDT.ToString(),
                                currentProductionReport.Colada, currentProductionReport.Lote, goodPieces, reworkedPieces, discardPieces, currentGeneralPiece.LoadedCount, changeReason,
                                user, currentProductionReport.DescripcionMaquina, currentProductionReport.Almacen, currentProductionReport.IdHistory, out errorMessage, 4);

                            //    selectedBox.IdN2 = idN2;
                            //}
                            //else
                            //{
                            //    resultLoad = true;
                            //}

                            //if (resultLoad)
                            //{
                            //int totalPieces = goodPieces + discardPieces;

                            bool result = iTServiceAdapter.ReportProductionBox(user, selectedBox.IdN2, currentProductionReport.TipoUDT, currentProductionReport.IdUDT.ToString(), currentProductionReport.Orden,
                                currentProductionReport.Colada, currentProductionReport.Lote, currentProductionReport.Secuencia, selectedBox.OperationId, selectedBox.MachineId, goodPieces, workedPieces,
                                reworkedPieces, currentProductionReport.CantidadTotal, currentProductionReport.ColadaSalida, opHija, selectedBox.Id, currentProductionReport.LoteSalida, changeReason,
                                selectedBox.Type, user, currentProductionReport.DescripcionMaquina, currentProductionReport.Almacen, currentProductionReport.IdHistory,
                                rejectionReportDetails.ToArray(), out errorMessage);
                            //}
                            //VERIFICAR SI LA CAJA QUE ACABAN DE REPORTAR YA SE ENCUENTRA COMPLETA CON LA QUERY DE IT
                            verificaBoxCompeteIT(currentGeneralPiece.OrderNumber, selectedBox.Id, questionRequest,  messageRequest, errorMessageRequest);

                            if (result)
                            {
                                ShowMessage message = new ShowMessage("Reporte de Producción", "Reporte de Producción en Caja realizado correctamente!");
                                messageRequest.Raise(new Notification() { Content = message});

                            }
                            else //Descargar la caja, de lo contrario se queda cargada en TPS y ya no le aparece al operador en la lista de cajas disponibles para seleccionar
                            {
                                string errorUnload;


                                bool unload = iTServiceAdapter.UnloadProductionBox(selectedBox.IdN2, selectedBox.Id, selectedBox.MachineId, selectedBox.OperationId, selectedBox.MissingPieces,
                                    currentProductionReport.Secuencia, selectedBox.Type, out errorUnload);
                                ShowError showError = new ShowError("Reporte de producción", "No se pudo realizar el Reporte de Producción: " + errorMessage);
                                errorMessageRequest.Raise(new Notification() { Content = showError });
                                
                            }
                            //}
                            //else
                            //{
                            //    MessageBox.Show("No se pudo realizar la Carga de Material: " + errorMessage, "Reporte de Producción", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //}
                        }
                    }
                    else
                    {
                        ShowMessage message = new ShowMessage("Reporte de Producción", string.Format("El Total de Piezas Buenas ({0}) debe ser menor o igual al número de Piezas Disponibles en la Caja Seleccionada ({1})",
                            goodPieces, selectedBox.MissingPieces));
                        messageRequest.Raise(new Notification() {Content = message });
                        return false;
                    }
                }
                else
                {
                    ShowMessage message = new ShowMessage("Reporte de Producción", "No ha seleccionado una Caja");
                    messageRequest.Raise(new Notification() { Content = message });
                    return false;
                }
            }
            catch (Exception ex)
            {
                ShowError showError = new ShowError("Reporte de Producción", "Error: " + ex.Message);
                errorMessageRequest.Raise(new Notification() {Content = showError });
            }
            return true;
        }







        public static List<RejectionReportDetail> rejectionReportDetails = new List<RejectionReportDetail>();



        public static ObservableCollection<RejectionReportDetail> btnAddRejectionDetail_Click(RejectionCode SelectedRejectionCode, int scrapCountForRejection, string selectedBundleDestiny,
            string motivo, bool worked, bool extremo1, ObservableCollection<RejectionReportDetail> dgrejectionReportDetails, out bool refresh)
        {

            short cantidad = (short)scrapCountForRejection;

            var rejectionReportDetail = new RejectionReportDetail();

            var TEMrejectionReportDetail = new RejectionReportDetail
            {
                RejectionCode = SelectedRejectionCode,
                ScrapCount = cantidad,
                Active = Enumerations.AxlrBit.Si,
                InsDateTime = DateTime.Now,
                Destino = selectedBundleDestiny,
                Observation = motivo,
                Trabajado = (worked) ? Enumerations.AxlrBit.Si : Enumerations.AxlrBit.No,
                Extremo = (extremo1) ? "Extremo 1" : "Extremo 2"/// Revisar
            };
            rejectionReportDetail = TEMrejectionReportDetail;


            bool alreadyAdded = false;

            foreach (RejectionReportDetail reportRejDet in dgrejectionReportDetails) //Posible cambio FUTURODWF (
            {
                if (reportRejDet.RejectionCode.Id == SelectedRejectionCode.Id && reportRejDet.Trabajado == (worked ? Enumerations.AxlrBit.Si : Enumerations.AxlrBit.No) &&
                    (reportRejDet.Extremo == (extremo1 ? "Extremo 1" : "Extremo 2")) || reportRejDet.Extremo == null)
                {
                    reportRejDet.ScrapCount += cantidad;
                    alreadyAdded = true;
                }
            }

            if (!alreadyAdded)
            {
                dgrejectionReportDetails.Add(rejectionReportDetail);
                refresh = true;
            }
            else
                refresh = false;

            rejectionReportDetails = dgrejectionReportDetails.ToList();
            return new ObservableCollection<RejectionReportDetail>(dgrejectionReportDetails);

        }

        




        public static int GetPreviousTotal(string extremo, GeneralPiece currentGeneralPiece)
        {
            var description = "";
            int total = 0;

            if (currentGeneralPiece.Description.Contains("Forjadora"))
                description = "Forjado";
            else if (currentGeneralPiece.Description.Contains("Roscadora"))
                description = "Mecanizado";
            else
                description = Configurations.Instance.Operacion;

            int machineSequence = new ReportProductionHistoryFacade().GetPreviousSequenceByOperation(description + " " + extremo) + 1;

            IList reportItems = new ReportProductionHistoryFacade().GetReportProductionHistoryByParams(currentGeneralPiece.OrderNumber, currentGeneralPiece.GroupItemNumber,
                currentGeneralPiece.HeatNumber, null, null, machineSequence);

            foreach (ReportProductionHistory rph in reportItems)
            {
                if (extremo == "Extremo 1")
                {
                    total += rph.GoodCount;//Solo se toman en cuenta las buenas del extremo 1, ya que las malas no se pueden reportar en la segunda estacion (extremo 2)
                }
                else if (extremo == "Extremo 2")
                {
                    total += (rph.GoodCount + rph.ScrapCount);
                }
            }

            return total;
        }

        //private void chkEdit_CheckedChanged(object sender, EventArgs e)
        //{
        //    txtTotalActualAtado.ReadOnly = !txtTotalActualAtado.ReadOnly;
        //    txtTotalActualAtado.Focus();
        //}

        /// <summary>
        /// verifica con la consaluta de oracle de IT, si la caja se ha completado, de ser asi actualiza DB SQL
        /// </summary>
        /// <param name="opMother"></param>
        /// <param name="idbox"></param>
        private static void verificaBoxCompeteIT(int opMother, string idbox, InteractionRequest<Notification> questionRequest, InteractionRequest<Notification> messageRequest, InteractionRequest<Notification> errorMessageRequest)
        {
            try
            {
                List<BoxProductionIT> listBoXIT = DataAccessOracle.GetBoxesByWOMother(opMother);


                //PRUEBADWF ORACLE BYPASS
                BoxProductionIT box;

                if (listBoXIT.Count > 0)
                {
                    box = listBoXIT.First(x => x.IdBox.Equals(idbox, StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    box = new BoxProductionIT();
                    box.Status = "C";
                }
                

                

                if (box.Status.Equals("C", StringComparison.OrdinalIgnoreCase))
                {
                    //Cambia Status BOX
                    DataAccessSQL dataAccessSQL = new DataAccessSQL();
                    dataAccessSQL.UpdBoxReportada(idbox);
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["BloqueoActivo"]))
                    {
                        //Bloquea TAG Carga Tag para Bloqueo
                        //TagDefinicion tDef = new TagDefinicion();
                        //tDef.TagName = ConfigurationManager.AppSettings["TagName"].ToString();
                        //tDef.longitudTag = Convert.ToInt32(ConfigurationManager.AppSettings["LongitudTag"].ToString());
                        //tDef.posicionEscritura = Convert.ToInt32(ConfigurationManager.AppSettings["PosisionEscritura"].ToString());
                        //short bloqueoValorTEM = (short)Convert.ToUInt16(ConfigurationManager.AppSettings["ValorBloqueo"].ToString());
                        //short desbloqueoValorTEM = (short)Convert.ToUInt16(ConfigurationManager.AppSettings["ValorDesBloqueo"].ToString());
                        //tDef.maxIntConexionScada = Convert.ToInt32(ConfigurationManager.AppSettings["MaxIntConexionScada"].ToString());
                        //tDef.valorBloqueo = new short[tDef.longitudTag];
                        //tDef.valorBloqueo[tDef.posicionEscritura] = bloqueoValorTEM;
                        //tDef.valorDesBloqueo = new short[tDef.longitudTag];
                        //tDef.valorDesBloqueo[tDef.posicionEscritura] = desbloqueoValorTEM;

                        //ScadaAdquisidor sAdq = new ScadaAdquisidor(tDef);

                        //sAdq.inicializar();
                        //sAdq.activarOnliBlock();
                        //sAdq.bloquear();
                        //sAdq.desActivarOnliBlock();
                        //sAdq = null;
                        //tDef = null;
                    }

                }
            }
            catch (Exception ex)
            {
                ShowError showError = new ShowError("Reporte de Produccion","Error: "+ ex.Message);
                errorMessageRequest.Raise(new Notification() {Content = showError });
                
            }
        }

        /// <summary>
        /// verifica si la caja ha sido selecionada en el la DB
        /// </summary>
        /// <param name="opMother"></param>
        /// <param name="idBox"></param>
        /// <returns>true si ha sido seleccionada, false si no</returns>
        private static bool isSelectBox(int opMother, string idBox)
        {
            bool resul = ProductionReportingBusiness.IsBoxSelect(opMother, idBox) == 1 ? true : false;
            return resul;
        }

        /// <summary>
        /// filtra el listado de cajas para dejar ver solo la que se ha seleccionado por la guia de produccion
        /// </summary>
        /// <param name="lisBoxIT"></param>
        /// <param name="orderMother"></param>
        /// <returns></returns>
        private List<ProductionBox> fitroDeCajas(List<ProductionBox> lisBoxIT, int orderMother)
        {
            List<ProductionBox> lisCajas;
            try
            {
                string idBoxSelect = ProductionReportingBusiness.BoxSelect(orderMother);
                lisCajas = lisBoxIT.FindAll(x => x.Id.Equals(idBoxSelect));
            }
            catch (Exception ex)
            {
                lisCajas = new List<ProductionBox>();
            }
            return lisCajas;
        }


    }
}
