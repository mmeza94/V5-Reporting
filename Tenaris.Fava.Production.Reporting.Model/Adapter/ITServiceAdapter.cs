using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

using System.Configuration;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using System.Collections;

using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.NhAccess.Reporitories;
using Tenaris.Fava.Production.Reporting.ITConnection;
using log4net;
using Tenaris.Fava.Production.Reporting.Model.Support;
using Tenaris.Fava.Production.Reporting.ITConnection.ITService;
using System.Xml.Serialization;
using System.IO;
using Tenaris.Fava.Production.Reporting.Model.Business;
using System.Collections.ObjectModel;
using Tenaris.Library.Log;
using Tenaris.Fava.Production.Reporting.Model.Model;

namespace Tenaris.Fava.Production.Reporting.Model.Adapter
{
    public class ITServiceAdapter
    {
        public ITServ IT = new ITServ();
        public static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static int secuenciaReal = 1;

        public DataTable GetAvailableStock(string OP, string Operacion, string OpcionConsola, string IdProducto, string VersionProducto, ref string sMsjHost)
        {

            DataTable dt = new DataTable();
            try
            {

                //WebService ws = new WebService();
                string result = IT.GetAvaibleStockIT(OP, Operacion, OpcionConsola, IdProducto, VersionProducto);
                DataSet ds = new DataSet();
                ds.ReadXml(new System.IO.StringReader(result));

                dt.Columns.Add("Order");
                dt.Columns.Add("Colada");
                dt.Columns.Add("Codigo Colada");
                dt.Columns.Add("TipoUdt");
                dt.Columns.Add("IdUdt");
                dt.Columns.Add("TipoUdc");
                dt.Columns.Add("Lote");
                dt.Columns.Add("Cantidad");
                dt.Columns.Add("Almacen");
                dt.Columns.Add("Extremo");
                dt.Columns.Add("SecuenciaSiguiente");
                dt.Columns.Add("OperacionSiguiente");
                dt.Columns.Add("OpcionSiguiente");
                dt.Columns.Add("Lot4");
                dt.Columns.Add("LotId");
                dt.Columns.Add("ProductReportBox");
                if (ds.Tables.Contains("UdtDisponible"))
                {


                    foreach (DataRow item in ds.Tables["UdtDisponible"].Rows)
                    {
                        dt.Rows.Add(new object[] {
                        item["Op"].ToString(),
                        item["Colada"].ToString(),
                        item["CodigoColada"].ToString(),
                        item["TipoUdt"].ToString(),
                        item["IdUdt"].ToString(),
                        item["TipoUdc"].ToString(),
                        item["Lote"],
                        item["Cantidad"].ToString(),
                        item["Almacen"], GetSuckerRodOrientation(item),
                        item["SecuenciaSiguiente"],
                        item["OperacionSiguiente"],
                        item["OpcionSiguiente"], "",
                        item["Lote"],
                        item["ProductReportBox"]});
                    }

                }
                sMsjHost = String.Empty;

            }
            catch (Exception ex)
            {
                // throw ex;
            }
            return dt;

        }

        private string GetSuckerRodOrientation(DataRow item) //no usa ITComHost directamente
        {
            var extremo = "";
            if (item["OperacionSiguiente"].ToString().Contains("Extremo"))
            {
                extremo = item["OperacionSiguiente"].ToString().EndsWith("Extremo 1") ? "Extremo 1" : "Extremo 2";
            }

            return extremo;
        }




        //ITCONNFACADE no usa ITComHost directamente
        public DataTable GetAvailableItems(string op, int sequence) //usado en ItConnFacade
        {
            var itConn = new ITServiceAdapter();
            string resultMessage = "";

            var numeroOperacionsiguiente = sequence + 1;
            var option = ConfigurationManager.AppSettings.Get("Option_" + (numeroOperacionsiguiente).ToString());
            var operation = ConfigurationManager.AppSettings.Get("Operation_" + (numeroOperacionsiguiente).ToString());
            var availableItems = itConn.GetAvailableStock(op, operation, option, "", "", ref resultMessage);
            return availableItems;
        }


        public DataTable GetAvailableStock(string op, int sequence) //usado en form1 //suma +1 a option y operacion
        {
            var itConn = new ITServiceAdapter();
            string resultMessage = "";

            var option = ConfigurationManager.AppSettings.Get("Option_" + (sequence).ToString());
            var operation = ConfigurationManager.AppSettings.Get("Operation_" + (sequence).ToString());
            var availableItems = itConn.GetAvailableStock(op, operation, option, "", "", ref resultMessage);
            return availableItems;
        }

        public bool IsGroupItemAvailableForNextOperation(int groupItemNumber, int heatNumber, int order, int sequence) //Usado en ReportConfirmation
        {
            bool result = false;
            try
            {
                DataTable availableItems = GetAvailableItems(order.ToString(), sequence);
                var items = from c in availableItems.AsEnumerable()
                            select new
                            {
                                GroupItemNumber = c.Field<string>("IdUdt"),
                                HeatNumber = c.Field<string>("Colada"),
                                OrderNumber = c.Field<string>("Order")
                            };
                var ocurrences = items.Where(c => c.GroupItemNumber == groupItemNumber.ToString()
                    && c.HeatNumber == heatNumber.ToString()).Count();
                result = ocurrences > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }





        //PRODUCTIONbOXrEPORT
        public List<DTO.ProductionBox> GetProductionBoxes(int orderNumber, string machineId, string operationId, out string errorMessage) //usado en IndBoxReportConfirmation
        {
            List<DTO.ProductionBox> listBoxes = null;
            ErrorCollection errors = null;


            Tenaris.Fava.Production.Reporting.ITConnection.ITService.ProductionBox[] availableBoxes = IT.GetProductionBoxesIT(out errors, orderNumber, machineId, operationId);

            if ((availableBoxes == null || availableBoxes.Count() == 0) && errors.List.Count > 0)
            {
                errorMessage = this.GetErrorMessage(errors);
            }
            else
            {
                listBoxes = new List<DTO.ProductionBox>();

                foreach (var box in availableBoxes)
                {
                    listBoxes.Add(new DTO.ProductionBox(box.Id, box.ParentOrderNumber, box.OrderNumber, box.MachineId, box.OperationId, box.Type, box.ProcessSequence, box.MaxPieces, box.LoadedPieces, box.MissingPieces));
                }

                errorMessage = string.Empty;
            }

            return listBoxes;
        }



        public bool LoadProductionBox(out int idN2, string boxId, int parentOrderNumber, int orderNumber, string machineId, string operationId, string workUnit, int sequenceProcess, //usado en IndBoxReportConfirmation
            int missingPieces, string workUnitSource, string workUnitSourceId, int heatNumber, int idLot, decimal goodPieces, decimal reworkedPieces, decimal discardPieces,
            decimal totalPieces, string comments, string idUser, string machineDescription, string store, int idHistory, out string errorMessage, int version)
        {
            Tenaris.Fava.Production.Reporting.ITConnection.ITService.ErrorCollection errors; //PRUEBADWF

            //----- Carga de Tarjeta de Linea -----//
            ProductionReport report = new ProductionReport();

            ReportProductionDto reportProductionDto = new ReportProductionDto
            {
                CantidadBuenas = Convert.ToInt32(goodPieces),
                IdUDT = Int32.Parse(workUnitSourceId),
                TipoUDT = workUnitSource,
                Colada = heatNumber,
                IdHistory = idHistory,
                DescripcionMaquina = machineDescription,
                Orden = parentOrderNumber,
                IdUser = idUser,
                CantidadTotal = Convert.ToInt32(totalPieces),
                Lote = idLot,
                CantidadReprocesadas = Convert.ToInt32(reworkedPieces),
                CantidadMalas = Convert.ToInt32(discardPieces),
                Secuencia = sequenceProcess,
                Operacion = operationId,
                Opcion = machineId,
                Observaciones = comments,
                Almacen = store
            };

            bool loaded = TPSLoadMaterial(reportProductionDto, version);

            //----- Carga de Caja -----//

            bool result = IT.LoadProductionBoxIT(boxId, machineId, operationId, missingPieces, sequenceProcess, workUnit, out errors);

            if (result)
            {
                errorMessage = string.Empty;
            }
            else
            {
                errorMessage = this.GetErrorMessage(errors);
            }


            idN2 = 0;
            return result;
        }

        public bool UnloadProductionBox(int idN2, string boxId, string machineId, string operationId, int pieces, int sequenceProcess, string workUnit, out string errorMessage) //usado en IndBoxReportConfirmation
        {
            Tenaris.Fava.Production.Reporting.ITConnection.ITService.ErrorCollection errors; //PRUEBADWF


            bool result = IT.UnLoadProductionBoxIT(boxId, machineId, operationId, pieces, sequenceProcess, workUnit, out errors);

            if (result)
            {
                errorMessage = string.Empty;
            }
            else
            {
                errorMessage = this.GetErrorMessage(errors);
            }

            return result;
        }

        public bool ReportProductionBox(string user, int idN2, string workUnitSource, string workUnitSourceId, int order, int heat, int idLot, int sequenceProcess, string operationId, string machineId, //usado en IndBoxReportConfirmation
            decimal goodPieces, decimal workedPieces, decimal reworkedPieces, decimal totalPieces, int endHeat, int childOrder, string boxId, int endIdLot,
            string comments, string boxType, string idUser, string machineDescription, string store, int idHistory, RejectionReportDetail[] discards, out string errorMessage)
        {
            Trace.Message("----------------------ITServiceAdapter----------------------");
            Trace.Message($"USER recived in Adapter: {user}");
            ErrorCollection errors = null;
            Descarte[] tpsDiscards = new Descarte[discards.Length];

            int i = 0;

            foreach (RejectionReportDetail discard in discards)
            {
                Descarte tpsDiscard = new Descarte();
                tpsDiscard.Cantidad = discard.ScrapCount.ToString();
                tpsDiscard.Destino = discard.Destino;
                tpsDiscard.Motivo = discard.RejectionCode.Code;
                tpsDiscard.Tipo = (discard.Trabajado == Enumerations.AxlrBit.Si) ? "1" : "0"; ;

                tpsDiscards[i++] = tpsDiscard;
            }

            Enumerations.ProductionReportSendStatus sendStatus = Enumerations.ProductionReportSendStatus.Completo;


            bool result = IT.ReportProductionBoxIT(out errors, user, workUnitSource, workUnitSourceId, heat, idLot, sequenceProcess, operationId, machineId,
                    goodPieces, workedPieces, reworkedPieces, totalPieces, endHeat, boxId, endIdLot, comments, boxType, tpsDiscards);

            if (result)
            {
                //----- Inserción de registro histórico de envío de producción -----//
                ProductionReport report = new ProductionReport();

                var reportProductionHistory = new ReportProductionHistory
                {
                    GoodCount = Convert.ToInt32(goodPieces),
                    GroupItemNumber = Convert.ToInt32(workUnitSourceId),
                    HeatNumber = heat,
                    IdHistory = idHistory,
                    IdMachine = (new CommonMachineRepository().GetMachineByDescription(machineDescription)).Id,
                    IdOrder = order,
                    InsDateTime = DateTime.Now,
                    InsertedBy = idUser,
                    TotalQuantity = Convert.ToInt32(workedPieces),
                    LotNumberHtr = idLot,
                    ReworkedCount = Convert.ToInt32(reworkedPieces),
                    ScrapCount = discards.Sum(d => d.ScrapCount),
                    SendStatus = sendStatus,
                    MachineSequence = sequenceProcess,
                    MachineOperation = operationId,
                    MachineOption = machineId,
                    Observation = comments,
                    GroupItemType = workUnitSource,
                    ChildOrder = childOrder,
                    ChildGroupItemNumber = Int32.Parse(boxId),
                    ChildGroupItemType = boxType
                };

                if (discards.Count() > 0)
                {
                    foreach (RejectionReportDetail rrd in discards)
                    {
                        rrd.ReportProductionHistory = reportProductionHistory;
                    }

                    new ReportProductionHistoryRepository().Save(reportProductionHistory, discards);
                }
                else
                {
                    new ReportProductionHistoryRepository().Save(reportProductionHistory);
                }

                errorMessage = string.Empty;
            }
            else
            {
                errorMessage = this.GetErrorMessage(errors);
            }

            return result;
        }



        private string GetErrorMessage(ErrorCollection errors) //usado en ProductionBoxReport NO USA ITSERV DIRECTAMENTE
        {
            StringBuilder msgError = new StringBuilder();

            foreach (var error in errors.List)
            {
                msgError.AppendLine(error.Value.Description);
            }

            return msgError.ToString();
        }



        public void GetAccesoriesSpecificationIT(int op, int colada)
        {
            Tenaris.Fava.Production.Reporting.ITConnection.ITService.ProductKey prodkey = new ProductKey();
            prodkey.Order = op.ToString();
            prodkey.Heat = colada.ToString();
            var result = IT.GetAccesoriesSpecificationIT(prodkey);

            string stop = "";

        }










        #region ProductionReport
        //PRODUCTIONrEPORT

        public bool TPSLoadMaterial(ReportProductionDto reportProductionDto) //usada en Business/ProductionReport -> WIN/ReportConfirmation y cajas
        {
            Tenaris.Fava.Production.Reporting.ITConnection.ITService.ErrorCollection errores = null;
            bool result = false;

            var programmedPieces =  reportProductionDto.CantidadTotal;
            //new GroupItemProgramFacade().
            //    GetProgrammedPieces(reportProductionDto);
            result = //true;
                 IT.LoadMaterialIT(
                     out errores,
               reportProductionDto.Orden,
               reportProductionDto.Colada,
               reportProductionDto.TipoUDT,
               reportProductionDto.IdUDT.ToString(),
               reportProductionDto.Almacen,
               //reportProductionDto.CantidadProcesadas.ToString(),
               programmedPieces.ToString(),
               reportProductionDto.Secuencia.ToString(),
               "observaciones",
               reportProductionDto.Operacion,
               reportProductionDto.Opcion,
               reportProductionDto.Lote.ToString()
               );
            var respuesta = new StringBuilder();
            if (errores != null)
            {
                foreach (var item in errores.List)
                {
                    respuesta.Append("---- Error Carga de Material----");
                    respuesta.Append("\nCodigo: ");
                    respuesta.Append(item.Value.Code);
                    respuesta.Append("\nDescripcion: ");
                    respuesta.Append("\n" + item.Value.Description);
                }
                log.Error(respuesta.ToString());
                log.Error(string.Format("Petición: {0}", GetXmlSerialization(reportProductionDto)));

            }
            return true;
        }



        public bool TPSLoadMaterial(ReportProductionDto reportProductionDto, int Version) //usada en Business/ProductionReport -> WIN/ReportConfirmation y cajas
        {
            Tenaris.Fava.Production.Reporting.ITConnection.ITService.ErrorCollection errores = null;
            bool result = false;
            try
            {
                var programmedPieces = 0;
                int.TryParse(Configurations.Instance.Secuencia, out int secuenciaHard);
                /*V1: 11->5*/
                if (Version == 1 && secuenciaHard > 7 )
                {
                    //HardCode :( más allá de la forja, en donde no hay parciales

                    if (Configurations.Instance.MaquinaInicialZona == "0")
                    {
                        if (Configurations.Instance.Machine == "Roscadora" &&
                            Configurations.Instance.Extremo == "Extremo 2")
                            programmedPieces = new ReportProductionHistoryFacade().
                               GetLastMachineGoodPieces(reportProductionDto.Orden,
                               reportProductionDto.Colada,
                               reportProductionDto.IdUDT,
                               reportProductionDto.DescripcionMaquina, "Extremo 2");
                        else
                            programmedPieces = new ReportProductionHistoryFacade().
                                   GetLastMachineGoodPieces(reportProductionDto.Orden,
                                   reportProductionDto.Colada,
                                   reportProductionDto.IdUDT,
                                   reportProductionDto.DescripcionMaquina, "");
                    }
                    else
                        programmedPieces = new GroupItemProgramFacade().GetProgrammedPieces(reportProductionDto);

                }
                else if (secuenciaHard > 11)//HardCode :( más allá de la forja, en donde no hay parciales
                {
                    if (Configurations.Instance.MaquinaInicialZona == "0")
                    {
                        if (Configurations.Instance.Machine == "Roscadora" &&
                            Configurations.Instance.Extremo == "Extremo 2")
                            programmedPieces = new ReportProductionHistoryFacade().
                               GetLastMachineGoodPieces(reportProductionDto.Orden,
                               reportProductionDto.Colada,
                               reportProductionDto.IdUDT,
                               reportProductionDto.DescripcionMaquina, "Extremo 2");
                        else
                            programmedPieces =
                                new ReportProductionHistoryFacade().
                                   GetLastMachineGoodPieces(reportProductionDto.Orden,
                                   reportProductionDto.Colada,
                                   reportProductionDto.IdUDT,
                                   reportProductionDto.DescripcionMaquina, "");
                    }
                    else
                        programmedPieces = new GroupItemProgramFacade().GetProgrammedPieces(reportProductionDto);
                }



                programmedPieces = (programmedPieces == 0) ? reportProductionDto.CantidadTotal : programmedPieces;
                //new GroupItemProgramFacade().
                //    GetProgrammedPieces(reportProductionDto);
                result = //true;
                     IT.LoadMaterialIT(
                         out errores,
                   reportProductionDto.Orden,
                   reportProductionDto.Colada,
                   reportProductionDto.TipoUDT,
                   reportProductionDto.IdUDT.ToString(),
                   reportProductionDto.Almacen,
                   //reportProductionDto.CantidadProcesadas.ToString(),
                   programmedPieces.ToString(),
                   reportProductionDto.Secuencia.ToString(),
                   "observaciones",
                   reportProductionDto.Operacion,
                   reportProductionDto.Opcion,
                   reportProductionDto.Lote.ToString()
                   );
                var respuesta = new StringBuilder();
                if (errores != null)
                {
                    foreach (var item in errores.List)
                    {
                        respuesta.Append("---- Error Carga de Material----");
                        respuesta.Append("\nCodigo: ");
                        respuesta.Append(item.Value.Code);
                        respuesta.Append("\nDescripcion: ");
                        respuesta.Append("\n" + item.Value.Description);
                    }
                    log.Error(respuesta.ToString());
                    log.Error(string.Format("Petición: {0}", GetXmlSerialization(reportProductionDto)));

                }
            }
            catch (Exception ex)
            {


                log.Error(ex.Message.ToString());
                log.Error(string.Format("Petición: {0}", GetXmlSerialization(reportProductionDto)));
                throw ex;
            }
            return result;
        }


        //V3:
        public bool TPSLoadMaterialForPainting(PaintingReport reportProductionDto)
        {

            Tenaris.Fava.Production.Reporting.ITConnection.ITService.ErrorCollection errores = null;
            bool result = false;
            try
            {

                var programmedPieces = 0;
                programmedPieces = (programmedPieces == 0) ? reportProductionDto.LoadQuantity : programmedPieces;
                //new GroupItemProgramFacade().
                //    GetProgrammedPieces(reportProductionDto);
                result = IT.LoadMaterialIT(out errores, reportProductionDto.ChildOrden, reportProductionDto.HeatNumber, reportProductionDto.UdtType, reportProductionDto.BoxUdt.ToString(),
                                            reportProductionDto.Storage, programmedPieces.ToString(), reportProductionDto.NextSequence.ToString(), "observaciones",
                                            reportProductionDto.NextOperation, reportProductionDto.NextOption, reportProductionDto.LotId.ToString());

                var respuesta = new StringBuilder();
                if (errores != null)
                {
                    foreach (var item in errores.List)
                    {
                        respuesta.Append("---- Error Carga de Material----");
                        respuesta.Append("\nCodigo: ");
                        respuesta.Append(item.Value.Code);
                        respuesta.Append("\nDescripcion: ");
                        respuesta.Append("\n" + item.Value.Description);
                    }
                    log.Error(respuesta.ToString());
                    log.Error(string.Format("Petición: {0}", GetXmlSerialization(reportProductionDto)));
                }

                if (result)
                {
                    int registry = 0;
                    try
                    {
                        registry = ProductionReportingBusiness.InsLoadPintado(reportProductionDto);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                log.Error(string.Format("Petición: {0}", GetXmlSerialization(reportProductionDto)));
                throw ex;
            }
            return result;
        }



        private bool TPSReportOnRevenido(string user, ReportProductionDto reportProductionDto, ref ErrorCollection errores, //usada en Business/ProductionReport -> WIN/ReportConfirmation
             IList rejectionReportDetails, int Version)
        {
            //Tenaris.Fava.Production.Reporting.ITConnection.ITService.ErrorCollection errores = null;
            bool result = false;
            try
            {

                result = IT.ReportProductionWProbesIT(user,
                                             reportProductionDto.TipoUDT,
                                             reportProductionDto.IdUDT,
                                             reportProductionDto.Colada,
                                             reportProductionDto.Lote,
                                             reportProductionDto.Aprietes,
                                             reportProductionDto.Secuencia,
                                             reportProductionDto.Operacion,
                                             reportProductionDto.Opcion,
                                             reportProductionDto.CantidadBuenas,
                                             reportProductionDto.CantidadProcesadas,
                                             reportProductionDto.CantidadReprocesadas,
                                             reportProductionDto.CantidadTotal,
                                             reportProductionDto.ColadaSalida,
                                             reportProductionDto.IdUDTSalida,
                                             reportProductionDto.LoteSalida,
                                             "Observaciones",
                                             reportProductionDto.TipoUDTSalida,
                                             GetTPSDescartes(reportProductionDto, rejectionReportDetails, Version),
                                             GetTPSProbetas(reportProductionDto),
                                             out errores);
            }
            catch (Exception ex)
            { throw ex; }
            return result;
        }


        private ProbetaAsociada[] GetTPSProbetas(ReportProductionDto reportProductionDto)
        {
            DataTable tblProbetas = new SampleCuttingFacade().GetCuttingNumbers(reportProductionDto.Orden, reportProductionDto.Colada, reportProductionDto.IdUDT);
            ProbetaAsociada[] probetas = new ProbetaAsociada[tblProbetas.Rows.Count];

            int cont = 0;
            foreach (DataRow row in tblProbetas.Rows)
            {
                ProbetaAsociada probetaAsociada = new ProbetaAsociada() { Id = int.Parse(row["CuttingNumber"].ToString()) };
                probetas[cont] = probetaAsociada;
                cont++;
            }
            return probetas;
        }


        private bool TPSReportNotOnRevenido(string user, ReportProductionDto reportProductionDto, ref ErrorCollection errores, 
             IList rejectionReportDetails)
        {
            bool result = false;
            try
            {

                var programmedPieces = reportProductionDto.CantidadTotal;

                reportProductionDto.CantidadTotal = programmedPieces;
                result =
                    IT.ReportProductionIT(user,
                                           reportProductionDto.TipoUDT,
                                           reportProductionDto.IdUDT,
                                           reportProductionDto.Colada,
                                           reportProductionDto.Lote,
                                           reportProductionDto.Aprietes,
                                           reportProductionDto.Secuencia,
                                           reportProductionDto.Operacion,
                                           reportProductionDto.Opcion,
                                           reportProductionDto.CantidadBuenas,
                                           reportProductionDto.CantidadProcesadas,
                                           reportProductionDto.CantidadReprocesadas,
                                           //programmedPieces,
                                           reportProductionDto.CantidadTotal,
                                           reportProductionDto.ColadaSalida,
                                           reportProductionDto.IdUDTSalida,
                                           reportProductionDto.LoteSalida,
                                           "Observaciones",
                                           reportProductionDto.TipoUDTSalida,
                                           GetTPSDescartes(reportProductionDto, rejectionReportDetails)
                                           , out errores);
            }
            catch (Exception ex)
            { throw ex; }
            return result;
        }





        private bool TPSReportNotOnRevenido(string user, ReportProductionDto reportProductionDto, ref ErrorCollection errores, //usada en Business/ProductionReport -> WIN/ReportConfirmation
             IList rejectionReportDetails, int Version)
        {
            bool result = false;
            try
            {
                var programmedPieces = 0;
                //if (reportProductionDto.Secuencia > 8)//HardCode :( más allá de la forja, en donde no hay parciales
                //{
                //    if (ConfigurationManager.AppSettings["MaquinaInicialZona"].ToString() == "0")
                //    {
                //        if (ConfigurationManager.AppSettings["Machine"].ToString() == "Roscadora" &&
                //            ConfigurationManager.AppSettings["Extremo"].ToString() == "Extremo 2")
                //            programmedPieces = new ReportProductionHistoryFacade().
                //               GetLastMachineGoodPieces(reportProductionDto.Orden,
                //               reportProductionDto.Colada,
                //               reportProductionDto.IdUDT,
                //               reportProductionDto.DescripcionMaquina, "Extremo 2");
                //        else
                //            programmedPieces =
                //        new ReportProductionHistoryFacade().
                //           GetLastMachineGoodPieces(reportProductionDto.Orden,
                //           reportProductionDto.Colada,
                //           reportProductionDto.IdUDT,
                //           reportProductionDto.DescripcionMaquina, "");
                //    }
                //    //else
                //    //    programmedPieces = new GroupItemProgramFacade().GetProgrammedPieces(reportProductionDto);
                //}

                programmedPieces = (programmedPieces == 0) ? reportProductionDto.CantidadTotal : programmedPieces;

                reportProductionDto.CantidadTotal = programmedPieces;
                result =
                    IT.ReportProductionIT(user,
                                           reportProductionDto.TipoUDT,
                                           reportProductionDto.IdUDT,
                                           reportProductionDto.Colada,
                                           reportProductionDto.Lote,
                                           reportProductionDto.Aprietes,
                                           reportProductionDto.Secuencia,
                                           reportProductionDto.Operacion,
                                           reportProductionDto.Opcion,
                                           reportProductionDto.CantidadBuenas,
                                           reportProductionDto.CantidadProcesadas,
                                           reportProductionDto.CantidadReprocesadas,
                                           //programmedPieces,
                                           reportProductionDto.CantidadTotal,
                                           reportProductionDto.ColadaSalida,
                                           reportProductionDto.IdUDTSalida,
                                           reportProductionDto.LoteSalida,
                                           "Observaciones",
                                           reportProductionDto.TipoUDTSalida,
                                           GetTPSDescartes(reportProductionDto, rejectionReportDetails, Version)
                                           , out errores);
            }
            catch (Exception ex)
            { throw ex; }
            return result;
        }

        public string TPSReportProduction(string user, ReportProductionDto reportProductionDto,  //usada en Business/ProductionReport -> WIN/ReportConfirmation //NO USA DIRECTAMENTE ITSERVICE
           Enumerations.ProductionReportSendStatus sendStatus, IList rejectionReportDetails)
        {
            Trace.Message("----------------------ITServiceAdapter----------------------");
            Trace.Message($"USER recived in Adapter: {user}");
            Tenaris.Fava.Production.Reporting.ITConnection.ITService.ErrorCollection errores = null;

            Tenaris.Fava.Production.Reporting.ITConnection.ITService.Probeta[] samples = null;
            var respuesta = new StringBuilder();

            try
            {
                bool sendIT = false;
               
                sendIT = TPSReportNotOnRevenido(user, reportProductionDto, ref errores, rejectionReportDetails);

                if (sendIT)
                {
                    //Guardado en BD
                    new ReportProductionHistoryFacade().SaveReportProductionHistory(reportProductionDto, sendStatus, rejectionReportDetails);
                }

                respuesta = ProcessTPSErrors(reportProductionDto, errores, samples);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                log.Error(string.Format("Petición: {0}", GetXmlSerialization(reportProductionDto)));
                throw ex;
            }
            return respuesta.ToString();
        }




        public string TPSReportProduction(string user, ReportProductionDto reportProductionDto,  //usada en Business/ProductionReport -> WIN/ReportConfirmation //NO USA DIRECTAMENTE ITSERVICE
            Enumerations.ProductionReportSendStatus sendStatus, IList rejectionReportDetails, int Version)
        {
            Trace.Message("----------------------ITServiceAdapter----------------------");
            Trace.Message($"USER recived in Adapter: {user}");
            Tenaris.Fava.Production.Reporting.ITConnection.ITService.ErrorCollection errores = null;

            Tenaris.Fava.Production.Reporting.ITConnection.ITService.Probeta[] samples = null;
            var respuesta = new StringBuilder();

            try
            {
                bool sendIT = false;
                //Solo para horno de revenido
                if (Configurations.Instance.Secuencia == "7") //reportProductionDto.Secuencia == 7
                {
                    #region Forma Adecuada cuando las probetas sean generadas  desde N2
                    Tenaris.Fava.Production.Reporting.ITConnection.ITService.ProbetaAsociada[] probetas = GetTPSProbetas(reportProductionDto);
                    if (probetas.Where(p => p.Id == 0).ToList().Count == 0)
                        sendIT = TPSReportOnRevenido(user, reportProductionDto, ref errores, rejectionReportDetails, Version);
                    else
                    {
                        errores = new Tenaris.Fava.Production.Reporting.ITConnection.ITService.ErrorCollection();
                        errores.List.Add("Probeteo", new Tenaris.Fava.Production.Reporting.ITConnection.ITService.ErrorElement() { Code = "Probeteo", Description = "Faltan Números de Probeta" });
                    }
                    #endregion

                }
                else
                    sendIT = TPSReportNotOnRevenido(user, reportProductionDto, ref errores, rejectionReportDetails, Version);

                if (sendIT)
                {
                    if (Configurations.Instance.Secuencia == "7" || Configurations.Instance.Secuencia == "6")
                        new ReportProductionHistoryFacade().SaveReportProductionHistoryForRevenido(reportProductionDto, sendStatus, rejectionReportDetails);
                    else
                        //Grabamos N2
                        new ReportProductionHistoryFacade().SaveReportProductionHistory(reportProductionDto, sendStatus, rejectionReportDetails);
                }

                respuesta = ProcessTPSErrors(reportProductionDto, errores, samples);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                log.Error(string.Format("Petición: {0}", GetXmlSerialization(reportProductionDto)));
                throw ex;
            }
            return respuesta.ToString();
        }



        private StringBuilder ProcessTPSErrors(ReportProductionDto reportProductionDto, //NO USA DIRECRAMENTE ITSERVICE
            ErrorCollection errores, Probeta[] samples)
        {
            StringBuilder respuesta = new StringBuilder();
            if (errores != null)
            {
                foreach (var item in errores.List)
                {
                    respuesta.Append("---- Error Reporte de Producción----");
                    respuesta.Append("\nCodigo: ");
                    respuesta.Append(item.Value.Code);
                    respuesta.Append("\nDescripcion: ");
                    respuesta.Append("\n" + item.Value.Description);
                }
                log.Error(respuesta.ToString());
                log.Error(string.Format("Petición: {0}", GetXmlSerialization(reportProductionDto)));

            }
            else
            {
                log.Info(string.Format("Reporte Enviado Correctamente -Atado: {0}, Orden: {1}, Colada: {2}",
                    reportProductionDto.IdUDT.ToString(),
                    reportProductionDto.Orden.ToString(),
                    reportProductionDto.Colada.ToString()));
                respuesta.Append("Reporte Enviado Correctamente\n");
                if (samples != null)
                {
                    respuesta.Append("*** Números de Probetas ***\n");
                    foreach (Probeta probeta in samples)
                        respuesta.Append(
                            string.Format("Probeta: {0} \n", probeta.Id)
                            );
                }
            }
            return respuesta;
        }


        public static string GetXmlSerialization(Object item)
        {
            XmlSerializer x = new XmlSerializer(item.GetType());
            StringWriter stringWriter = new StringWriter();
            x.Serialize(stringWriter, item);
            return stringWriter.ToString();
        }

        private Descarte[] GetTPSDescartesV1(ReportProductionDto reportProductionDto, IList rejectionReportDetails)
        {

            Descarte[] descartes = new Descarte[rejectionReportDetails == null ? 0 : rejectionReportDetails.Count];
            int cont = 0;
            if (descartes.Length > 0)
            {
                foreach (RejectionReportDetail rej in rejectionReportDetails)
                {
                    Descarte descarte = new Descarte();
                    descarte.Destino = rej.Destino;
                    descarte.Motivo = rej.RejectionCode.Code;//"VVT";
                    descarte.Tipo = (rej.Trabajado == Enumerations.AxlrBit.Si) ? "1" : "0";
                    descarte.Cantidad = rej.ScrapCount.ToString();
                    descartes[cont] = descarte;
                    cont++;
                }
            }

            return descartes;
        }


        private Descarte[] GetTPSDescartes(ReportProductionDto reportProductionDto, IList rejectionReportDetails)
        {
            
                return GetTPSDescartesV1(reportProductionDto, rejectionReportDetails);
                
            
        }





        private Descarte[] GetTPSDescartes(ReportProductionDto reportProductionDto, IList rejectionReportDetails, int Version)
        {
            if (Version == 1)
            {
                return GetTPSDescartesV1(reportProductionDto, rejectionReportDetails);

            }
            else
            {


                IList TEMrejectionReportDetails = GroupByRejectionCode(rejectionReportDetails);
                Descarte[] descartes = new Descarte[TEMrejectionReportDetails.Count];
                int cont = 0;
                foreach (RejectionReportDetail rej in TEMrejectionReportDetails)
                {
                    Descarte descarte = new Descarte();
                    descarte.Destino = rej.Destino;
                    descarte.Motivo = rej.RejectionCode.Code;//"VVT";
                    descarte.Tipo = (rej.Trabajado == Enumerations.AxlrBit.Si) ? "1" : "0";
                    descarte.Cantidad = rej.ScrapCount.ToString();
                    descartes[cont] = descarte;
                    cont++;
                }

                return descartes;
            }
        }

        private IList GroupByRejectionCode(IList rejectionReportDetails)
        {
            IList<RejectionReportDetail> TEMRejectionReportDetail = (IList<RejectionReportDetail>)rejectionReportDetails;

            if (TEMRejectionReportDetail.Count > 1 && TEMRejectionReportDetail.First().Extremo != null)
            {
                return TEMRejectionReportDetail.GroupBy(m => m.RejectionCode.Code).Select
                                              (n => new RejectionReportDetail
                                              {
                                                  Destino = n.First().Destino,
                                                  Observation = n.First().Observation,
                                                  ScrapCount = (short)n.Sum(o => o.ScrapCount),
                                                  Trabajado = n.First().Trabajado,
                                                  Extremo = n.First().Extremo,
                                                  RejectionCode = n.First().RejectionCode,
                                                  Active = n.First().Active,
                                                  Id = n.First().Id,
                                                  InsDateTime = n.First().InsDateTime,
                                                  ReportProductionHistory = n.First().ReportProductionHistory,
                                                  UpdDateTime = n.First().UpdDateTime
                                              }).ToList();
            }
            else
            {
                return rejectionReportDetails;
            }
        }

        #endregion

        
      



      



   
       




        
       

        //PUBLICOS
        public string ReportProduction(string user, ReportProductionDto reportProductionDto, Enumerations.ProductionReportSendStatus sendStatus
              , bool loadMaterial, IList rejectionReportDetails)
        {

            //PRUEBADWF OBTENER NUEVA SECUENCIA PARA ENVIO A TPS (BARRAS DE PESO)
            DataTable availableItems = GetAvailableStock(reportProductionDto.Orden.ToString(), (reportProductionDto.Secuencia));
            //int.TryParse(availableItems.Select("SecuenciaSiguiente > 0").FirstOrDefault().ToString(), out secuenciaReal);

           // int.TryParse(availableItems.Rows[0]["SecuenciaSiguiente"].ToString(), out secuenciaReal);
            var items = from c in availableItems.AsEnumerable()
                        select new
                        {
                            GroupItemNumber = c.Field<string>("IdUdt"),
                            secuenciaRealx = c.Field<string>("SecuenciaSiguiente")
                        };
            reportProductionDto.Secuencia = Convert.ToInt32(items.FirstOrDefault()?.secuenciaRealx) == 0 ? reportProductionDto.Secuencia : Convert.ToInt32(items.FirstOrDefault()?.secuenciaRealx);

            //PRUEBADWF END
           



            var respuesta = "";
            try
            {
                if (loadMaterial)
                    TPSLoadMaterial(reportProductionDto);

                respuesta = TPSReportProduction(user, reportProductionDto, sendStatus, rejectionReportDetails);

            }
            catch (Exception ex)
            {
                respuesta = ex.Message.ToString();
            }
            return respuesta;
        }
    }
}
