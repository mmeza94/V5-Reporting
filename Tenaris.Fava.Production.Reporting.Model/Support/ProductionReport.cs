using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using log4net;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using System.Configuration;
using Tenaris.Fava.Production.Reporting.Model.Data_Access;
using Tenaris.Fava.Production.Reporting.Model.NhAccess.Reporitories;
using Tenaris.Fava.Production.Reporting.ITConnection.ITService;
using Tenaris.Fava.Production.Reporting.Model.Support;
using System.Collections.ObjectModel;
using Tenaris.Fava.Production.Reporting.Model.Business;
using Tenaris.Fava.Production.Reporting.Model.Adapter;

namespace Tenaris.Fava.Production.Reporting.Model.Support
{
    public class ProductionReport
    {
        public static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region PUBLIC METHOS
        public ObservableCollection<GeneralPiece> GetProductionGeneral(string Orden, string Colada, string Atado)
        {
            var generalPieces = new ObservableCollection<GeneralPiece>();
            try
            {

                Dictionary<string, object> listparams = new Dictionary<string, object>();
                listparams.Add("@Orden", Orden);
                listparams.Add("@Colada", Colada);
                listparams.Add("@Atado", Atado);

                var productionGeneralTable = ProductionReportingBusiness.GetProductionGeneral(listparams);

                generalPieces = productionGeneralTable;


                //foreach (DataRow row in productionGeneralTable.Rows)
                //{
                //    generalPieces.Add(
                //        new GeneralPiece
                //        {
                //            IdHistory = Convert.ToInt32(row["idHistory"].ToString()),
                //            OrderNumber = Convert.ToInt32(row["OrderNumber"].ToString()),
                //            Customer = row["Customer"].ToString(),
                //            HeatNumber = String.IsNullOrEmpty(row["HeatNumber"].ToString()) ? 0 : Convert.ToInt32(row["HeatNumber"].ToString()),
                //            GroupItemNumber = String.IsNullOrEmpty(row["GroupItemNumber"].ToString()) ? 0 : Convert.ToInt32(row["GroupItemNumber"].ToString()),
                //            LotNumberHTR = String.IsNullOrEmpty(row["LotNumberHTR"].ToString()) ? 0 : Convert.ToInt32(row["LotNumberHTR"].ToString()),
                //            LoadedCount = String.IsNullOrEmpty(row["LoadedCount"].ToString()) ? 0 : Convert.ToInt32(row["LoadedCount"].ToString()),
                //            GoodCount = String.IsNullOrEmpty(row["GoodCount"].ToString()) ? 0 : Convert.ToInt32(row["GoodCount"].ToString()),
                //            ScrapCount = String.IsNullOrEmpty(row["ScrapCount"].ToString()) ? 0 : Convert.ToInt32(row["ScrapCount"].ToString()),
                //            WarnedCount = String.IsNullOrEmpty(row["WarnedCount"].ToString()) ? 0 : Convert.ToInt32(row["WarnedCount"].ToString()),
                //            ReworkedCount = String.IsNullOrEmpty(row["ReworkedCount"].ToString()) ? 0 : Convert.ToInt32(row["ReworkedCount"].ToString()),
                //            Description = row["Description"].ToString(),
                //            Location = row["Location"].ToString(),
                //            IdBatch = String.IsNullOrEmpty(row["idBatch"].ToString()) ? 0 : Convert.ToInt32(row["idBatch"].ToString()),
                //            InsDateTime = Convert.ToDateTime(row["InsDateTime"].ToString()),
                //            Extremo = row["Extremo"].ToString(),
                //            ReportBox = productionGeneralTable.Columns.Contains("ReportBox") ? row["ReportBox"].ToString() : null,
                //            GroupItemType = productionGeneralTable.Columns.Contains("GroupItemType") ? row["GroupItemType"].ToString() : null,
                //        });
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return generalPieces;

        }

        public ObservableCollection<GeneralPiece> GetReportProductionHistory(int? orderNumber, int? groupItemNumber, int? heatNumber, int? idMachine, int? machineSequence, int? idHistory)
        {
            var resul = new ObservableCollection<GeneralPiece>();
            try
            {

                Dictionary<string, object> listparams = new Dictionary<string, object>();
                listparams.Add("@orderNumber", orderNumber);
                listparams.Add("@groupItemNumber", groupItemNumber);
                listparams.Add("@heatNumber", heatNumber);
                listparams.Add("@idMachine", idMachine);
                listparams.Add("@machineSequence", machineSequence);
                listparams.Add("@idHistory", idHistory);
         

                var table = ProductionReportingBusiness.GetReportProductionHistory(listparams);

                resul = table;
                //foreach (DataRow item in table.Rows)
                //{
                //    resul.Add(new ReportProductionHistory
                //    {
                //        Id = Convert.ToInt32(item["idReportProductionHistory"]),
                //        IdHistory = Convert.ToInt32(item["idHistory"]),
                //        IdOrder = Convert.ToInt32(item["OrderNumber"]),
                //        HeatNumber = Convert.ToInt32(item["HeatNumber"]),
                //        GroupItemNumber = Convert.ToInt32(item["GroupItemNumber"]),
                //        SendStatus = this.CastEnumProductionReportSendStatus(Convert.ToInt32(item["SendStatus"])),
                //        TotalQuantity = Convert.ToInt32(item["TotalQuantity"]),
                //        GoodCount = Convert.ToInt32(item["GoodCount"]),
                //        ScrapCount = Convert.ToInt32(item["ScrapCount"]),
                //        ReworkedCount = Convert.ToInt32(item["ReworkedCount"]),
                //        IdMachine = Convert.ToInt32(item["IdMachine"]),
                //        LotNumberHtr = Convert.ToInt32(item["LotNumberHtr"]),
                //        InsDateTime = Convert.ToDateTime(item["InsDateTime"]),
                //        InsertedBy = item["InsertedBy"].ToString(),
                //        MachineSequence = Convert.ToInt32(item["MachineSequence"]),
                //        MachineOption = item["MachineOption"].ToString(),
                //        MachineOperation = item["MachineOperation"].ToString(),
                //        Observation = item["Observation"].ToString()
                //    });
                //}
            }
            catch (Exception ex) { }
            return resul;
        }

        public int GetLastMachineGoodPieces(int order, int heat, int groupItem, string description, string extreme)
        {
            int resul = 0;
            int machineSequence = 0;
            if (extreme != string.Empty)
                machineSequence = ProductionReportingBusiness.GetPreviousSequenceByOperation(description + " " + extreme);
            else
                machineSequence = ProductionReportingBusiness.GetPreviousSequence(description);

            Dictionary<string, object> listparams = new Dictionary<string, object>();
            listparams.Add("@order", order);
            listparams.Add("@groupItem", groupItem);
            listparams.Add("@heat", heat);
            listparams.Add("@machineSequence", machineSequence);

            var table = ProductionReportingBusiness.GetReportProductionHistory(listparams);

            resul = table.Sum(x => x.GoodCount);

            //foreach (var item in table)
            //{
            //    resul += Convert.ToInt32(item.GoodCount);
            //}
       
            return resul;
        }

        public IList<GeneralPiece> GetSendStatusForGeneralPieces(IList<GeneralPiece> generalPieces)
        {
            var reportProductionHistoryFacade = new ReportProductionHistoryFacade();

            if (ConfigurationManager.AppSettings["Machine"] != "Pintado")
            {
                foreach (GeneralPiece generalpiece in generalPieces)
                {
                    var reportProductionHistory = reportProductionHistoryFacade.GetReportProductionHistoryByIdHistory(generalpiece.IdHistory, generalpiece.OrderNumber, generalpiece.HeatNumber, generalpiece.GroupItemNumber);
                    generalpiece.Sended = (reportProductionHistory != null) ? Enumerations.AxlrBit.Yes : Enumerations.AxlrBit.No;
                }
            }
            else
            {
                foreach (GeneralPiece generalpiece in generalPieces)
                {
                    var reportProductionHistory = reportProductionHistoryFacade.GetReportProductionHistoryByParams(generalpiece.OrderNumber, generalpiece.GroupItemNumber, generalpiece.HeatNumber, null, null, null);
                    //generalpiece.Sended = (reportProductionHistory.Count > 0) ?
                    generalpiece.Sended = (reportProductionHistory.Cast<ReportProductionHistory>().Count(p => p.MachineOperation == "Pintado") > 0) ? Enumerations.AxlrBit.Yes : Enumerations.AxlrBit.No;
                }
            }

            return generalPieces;
        }

        public bool IsForjadoraAndForgeModeIsOnline(int groupItemNumber)
        {
            var result = false;

            if (ConfigurationManager.AppSettings["MaquinaInicialZona"].ToString() == "1" && ConfigurationManager.AppSettings["Machine"].ToString() == "Forjadora") // Solo entra si está ubicado en la forja 1. 
            {
                var forgeMode = new ProductionReportFacade().GetCurrentForgeMode(groupItemNumber);
                result = (forgeMode == Enumerations.ForgeMode.OneEnd);
            }

            return result;
        }

        public IList<GeneralPiece> ClassifyBySendStatus(IList<GeneralPiece> generalPieces)
        {
            var reportProductionHistoryFacade = new ReportProductionHistoryFacade();
            var generalPiecesClassified = new List<GeneralPiece>();

            int order = 0;
            int heat = 0;
            int groupItem = 0;
            string description = "";
            string extreme = "";
            try
            {
                var orderedGeneralPieces = generalPieces.OrderByDescending(x => x.InsDateTime).ToList();
                for (int i = 0; i < orderedGeneralPieces.Count; i++)
                {
                    if (order != orderedGeneralPieces[i].OrderNumber || heat != orderedGeneralPieces[i].HeatNumber || groupItem != orderedGeneralPieces[i].GroupItemNumber || description != orderedGeneralPieces[i].Description || extreme != orderedGeneralPieces[i].Extremo)
                    {
                        order = orderedGeneralPieces[i].OrderNumber;
                        heat = orderedGeneralPieces[i].HeatNumber;
                        groupItem = orderedGeneralPieces[i].GroupItemNumber;
                        description = orderedGeneralPieces[i].Description;
                        extreme = orderedGeneralPieces[i].Extremo;

                        if (!generalPiecesClassified.Exists(x => (x.OrderNumber == order)
                        && (x.HeatNumber == heat)
                        && (x.GroupItemNumber == groupItem) && (x.Description == description) && x.Extremo == extreme))
                            generalPiecesClassified.AddRange(GetSomePieces(order, heat, groupItem,
                                orderedGeneralPieces, description, extreme));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return generalPiecesClassified;
        }

        public string ReportProduction(ReportProductionDto reportProductionDto, Enumerations.ProductionReportSendStatus sendStatus,
            bool loadMaterial, IList rejectionReportDetails)
        {
            ITServiceAdapter iTService = new ITServiceAdapter();
            var respuesta = "";
            try
            {
                if (loadMaterial)
                    iTService.TPSLoadMaterial(reportProductionDto);
                respuesta = TPSReportProduction(reportProductionDto, sendStatus, rejectionReportDetails);
            }
            catch (Exception ex)
            {
                respuesta = ex.Message.ToString();
            }
            return respuesta;
        }



        public string ReportProductionForPainting(PaintingReport reportProductionDto, Enumerations.ProductionReportSendStatus sendStatus, bool loadMaterial, IList rejectionReportDetails)
        {
            var respuesta = "";
            try
            {
                //if (loadMaterial)
                //    TPSLoadMaterialForPainting(reportProductionDto);


                //ReportProductionDto reportProductionDtoToSend = new ReportProductionDto();

                //reportProductionDtoToSend = new ReportProductionDto();


                IList reportItems = new ReportProductionHistoryRepository().GetReportProductionHistoryByParams(reportProductionDto.ChildOrden, Convert.ToInt32(reportProductionDto.BoxUdt), reportProductionDto.HeatNumber, null, null, 20);

                //IEnumerable<ReportProductionHistory> range = reportItems;


                int MaxIdHistory = 0;


                foreach (ReportProductionHistory item in reportItems)
                {
                    if (item.IdHistory > MaxIdHistory)
                    {
                        MaxIdHistory = item.IdHistory;
                    }
                }

                //reportItems.AsQueryable().Fin


                ReportProductionDto reportProductionDtoToSend = new ReportProductionDto
                {

                    TipoUDT = reportProductionDto.UdtType,
                    IdUDT = Convert.ToInt32(reportProductionDto.BoxUdt),
                    Colada = reportProductionDto.HeatNumber,
                    Lote = Convert.ToInt32(reportProductionDto.LotId),
                    Aprietes = 0,
                    Secuencia = reportProductionDto.NextSequence,
                    //Se modifica para que no tome la operación y la opción basada en la secuencia, por que la secuencia no es un id de los procesos ejemplo pintado e Inyectado
                    //Operacion = ConfigurationManager.AppSettings["Operation_" + reportProductionDto.NextSequence.ToString()].ToString(),
                    //Opcion = ConfigurationManager.AppSettings["Option_" + reportProductionDto.NextSequence.ToString()].ToString(),
                    Operacion = reportProductionDto.NextOperation,
                    Opcion = reportProductionDto.NextOption,
                    CantidadBuenas = reportProductionDto.GoodCount,
                    CantidadMalas = reportProductionDto.ScrapCount,
                    //CantidadProcesadas = reportProductionDto.GoodCount;
                    CantidadReprocesadas = 0,
                    CantidadTotal = reportProductionDto.LoadQuantity,
                    //ColadaSalida = reportProductionDto.HeatNumber,
                    //IdUDTSalida = Convert.ToInt32(reportProductionDto.BoxUdt),
                    //LoteSalida = Convert.ToInt32(reportProductionDto.LotId),
                    //                            "Observaciones",
                    //                            reportProductionDto.TipoUDTSalida,
                    IdHistory = MaxIdHistory,
                    DescripcionMaquina = "Pintado",
                    Orden = reportProductionDto.ChildOrden,
                    IdUser = reportProductionDto.IdUser,
                    Observaciones = ""

                    //GoodCount = reportProductionDto.LoadQuantity,
                    //GroupItemNumber = Convert.ToInt32(reportProductionDto.BoxUdt),
                    //HeatNumber = reportProductionDto.HeatNumber,
                    //IdHistory = MaxIdHistory,
                    //IdMachine = (new CommonMachineRepository().GetMachineByDescription("Pintado")).Id,
                    //IdOrder = reportProductionDto.ChildOrden,
                    //InsDateTime = DateTime.Now,
                    //InsertedBy = reportProductionDto.IdUser,
                    //TotalQuantity = reportProductionDto.LoadQuantity,
                    //LotNumberHtr = Convert.ToInt32(reportProductionDto.LotId),
                    //ReworkedCount = 0,//reportProductionDto.CantidadReprocesadas,
                    //ScrapCount = reportProductionDto.ScrapCount,
                    //SendStatus = sendStatus,
                    //MachineSequence = reportProductionDto.NextSequence,
                    //MachineOperation = ConfigurationManager.AppSettings["Operation_" + reportProductionDto.NextSequence.ToString()].ToString(),
                    //MachineOption = ConfigurationManager.AppSettings["Option_" + reportProductionDto.NextSequence.ToString()].ToString(),
                    //Observation = "",
                    //GroupItemType = reportProductionDto.UdtType,
                    //ChildOrder = reportProductionDto.ChildOrden,
                    //ChildGroupItemNumber = Convert.ToInt32(reportProductionDto.BoxUdt),
                    //ChildGroupItemType = reportProductionDto.UdtType

                    //LoadedCount = reportProductionDto.l

                };


                //reportProductionDtoToSend.CantidadTotal = reportProductionDto.LoadQuantity;
                //reportProductionDtoToSend.CantidadBuenas = reportProductionDto.GoodCount;
                //reportProductionDtoToSend.CantidadMalas = reportProductionDto.ScrapCount;
                //reportProductionDtoToSend.CantidadReprocesadas = 0;
                //reportProductionDtoToSend.IdUser = reportProductionDto.IdUser;
                //reportProductionDtoToSend.Secuencia = reportProductionDto.NextSequence;

                //reportProductionDtoToSend.Orden = reportProductionDto.ChildOrden;
                //reportProductionDtoToSend.Colada = reportProductionDto.HeatNumber;
                //reportProductionDtoToSend.IdUDT = Convert.ToInt32(reportProductionDto.BoxUdt);
                //reportProductionDtoToSend.DescripcionMaquina = "Pintado";

                respuesta = TPSReportProduction(reportProductionDtoToSend, sendStatus, rejectionReportDetails);
            }
            catch (Exception ex)
            {
                respuesta = ex.Message.ToString();
            }
            return respuesta;
        }

        #endregion

        #region METHOS TO TPS
        


       

        /// <summary>
        /// Método para reportar en revenido
        /// </summary>
        /// <param name="reportProductionDto"></param>
        /// <param name="fava"></param>
        /// <param name="errores"></param>
        /// <returns></returns>
        

        /// <summary>
        /// Método temporal para mostrar las probetas en Revenido, provenientes de IT
        /// ya no se ocupa
        /// </summary>
        /// <param name="reportProductionDto"></param>
        /// <param name="fava"></param>
        /// <param name="errores"></param>
        /// <returns></returns>
        /// 
       

        /// <summary>
        /// Reporte en máquina distinta a revenido
        /// </summary>
        /// <param name="reportProductionDto"></param>
        /// <param name="fava"></param>
        /// <param name="errores"></param>
        /// <returns></returns>
       

        /// <summary>
        /// Reporte de Producción en TPS
        /// </summary>
        /// <param name="reportProductionDto"></param>
        /// <param name="sendStatus"></param>
        /// <param name="rejectionReportDetails"></param>
        /// <returns></returns>
        private string TPSReportProduction(ReportProductionDto reportProductionDto,
            Enumerations.ProductionReportSendStatus sendStatus, IList rejectionReportDetails)
        {
            Tenaris.Fava.Production.Reporting.ITConnection.ITService.TServiceClient fava = new Tenaris.Fava.Production.Reporting.ITConnection.ITService.TServiceClient();
            ErrorCollection errores = null;
            Probeta[] samples = null;
            var respuesta = new StringBuilder();
            ITServiceAdapter iTService =  new ITServiceAdapter();
            try
            {
                bool sendIT = false;
                //Solo para horno de revenido
                if (reportProductionDto.Secuencia == 7)
                {
                    #region Forma Adecuada cuando las probetas sean generadas  desde N2
                    ProbetaAsociada[] probetas = GetTPSProbetas(reportProductionDto);
                    if (probetas.Where(p => p.Id == 0).ToList().Count == 0)
                        sendIT = iTService.TPSReportOnRevenido(reportProductionDto, fava, ref errores, rejectionReportDetails);
                    else
                    {
                        errores = new ErrorCollection();
                        errores.List.Add("Probeteo", new ErrorElement() { Code = "Probeteo", Description = "Faltan Números de Probeta" });
                    }
                    #endregion
                }
                else
                    sendIT = iTService.TPSReportNotOnRevenido(reportProductionDto, fava, ref errores, rejectionReportDetails);

                if (sendIT)
                {
                    if (reportProductionDto.Secuencia == 7 || reportProductionDto.Secuencia == 6)
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
                log.Error(string.Format("Petición: {0}", iTService.GetXmlSerialization(reportProductionDto)));
                throw ex;
            }
            return respuesta.ToString();
        }
        #endregion

        #region METHOS PRIVATE
        private IList<GeneralPiece> GetSomePieces(int order, int heat, int groupItem, IList<GeneralPiece> generalPieces, string description, string extreme)
        {
            var somePieces = generalPieces.Where(x => (x.OrderNumber == order) && (x.HeatNumber == heat) &&
            (x.GroupItemNumber == groupItem) && (x.Description == description) && (x.Extremo == extreme)).
            OrderBy(x => x.InsDateTime).ToList();
            try
            {
                //Configuración llave para saber si es la máquina inicial en una zona
                if (ConfigurationManager.AppSettings["MaquinaInicialZonaFiltre"].ToString() == "0" || (ConfigurationManager.AppSettings["Machine"].ToString().Contains("Roscadora") && ConfigurationManager.AppSettings["ExtremoFiltre"].ToString() == "Extremo 2"))
                {
                    if (ConfigurationManager.AppSettings["Machine"].ToString() == "Pintado")
                    {
                        //el valor de LoadedCount no se modifica
                    }
                    else
                    {
                        //somePieces[0].LoadedCount = new ReportProductionHistoryFacade().GetLastMachineGoodPieces(somePieces[0].OrderNumber, somePieces[0].HeatNumber, somePieces[0].GroupItemNumber, somePieces[0].Description, somePieces[0].Extremo);
                        somePieces[0].LoadedCount = this.GetLastMachineGoodPieces(somePieces[0].OrderNumber, somePieces[0].HeatNumber, somePieces[0].GroupItemNumber, somePieces[0].Description, somePieces[0].Extremo);
                    }
                }
                else
                {
                    somePieces[0].LoadedCount = new GroupItemProgramFacade().GetProgrammedPieces(somePieces[0].IdBatch);
                }

                if (somePieces.Count > 1)
                {
                    //somePieces[somePieces.Count - 1].SendStatus = Enumerations.ProductionReportSendStatus.Final;
                    somePieces[somePieces.Count - 1].SendStatus =
                        (somePieces[somePieces.Count - 1].GoodCount + somePieces[somePieces.Count - 1].ScrapCount >= somePieces[somePieces.Count - 1].LoadedCount) ? Enumerations.ProductionReportSendStatus.Final : Enumerations.ProductionReportSendStatus.Partial;
                    somePieces[somePieces.Count - 1].ReportSequence = (short)somePieces.Count;
                    for (int i = 0; i < somePieces.Count - 1; i++)
                    {
                        somePieces[i].ScrapCount = 0;
                        somePieces[i].ReportSequence = (short)(i + 1);
                        somePieces[i].SendStatus = Enumerations.ProductionReportSendStatus.Partial;
                        if (i > 0)
                            somePieces[i].LoadedCount = somePieces[i - 1].LoadedCount - (somePieces[i - 1].GoodCount + somePieces[i - 1].ScrapCount);
                    }
                }
                else
                {
                    //somePieces[0].LoadedCount = somePieces[0].LoadedCount - somePieces[0].ReworkedCount; // Editamos la cantidad para que nos de las piezas programadas
                    somePieces[0].SendStatus = (somePieces[0].GoodCount + somePieces[0].ScrapCount >= somePieces[0].LoadedCount) ?
                        Enumerations.ProductionReportSendStatus.Complete : Enumerations.ProductionReportSendStatus.Partial;
                    somePieces[0].ReportSequence = 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return somePieces;
        }

        //private Descarte[] GetTPSDescartes(ReportProductionDto reportProductionDto)
        //{
        //    Descarte[] descartes = new Descarte[1];
        //    if (reportProductionDto.CantidadMalas > 0)
        //    {
        //        Descarte descarte = new Descarte();
        //        descarte.Destino = "Decisión de Ingeniería";
        //        descarte.Motivo = "VVT";
        //        descarte.Tipo = "1";
        //        descarte.Cantidad = Convert.ToString(reportProductionDto.CantidadMalas);
        //        descartes[0] = descarte;
        //    }
        //    return descartes;
        //}

        private Descarte[] GetTPSDescartes(ReportProductionDto reportProductionDto, IList rejectionReportDetails)
        {
            IList TEMrejectionReportDetails = GroupByRejectionCode(rejectionReportDetails);
            Descarte[] descartes = new Descarte[TEMrejectionReportDetails.Count];
            int cont = 0;
            foreach (RejectionReportDetail rej in TEMrejectionReportDetails)
            {
                Descarte descarte = new Descarte();
                descarte.Destino = rej.Destino;
                descarte.Motivo = rej.RejectionCode.Code;//"VVT";
                descarte.Tipo = (rej.Trabajado == Enumerations.AxlrBit.Yes) ? "1" : "0";
                descarte.Cantidad = rej.ScrapCount.ToString();
                descartes[cont] = descarte;
                cont++;
            }
            return descartes;
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

        private ProbetaAsociada[] GetTPSProbetas(ReportProductionDto reportProductionDto)
        {
            DataTable tblProbetas = new SampleCuttingFacade().GetCuttingNumbers(
                reportProductionDto.Orden, reportProductionDto.Colada, reportProductionDto.IdUDT);
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

        private StringBuilder ProcessTPSErrors(ReportProductionDto reportProductionDto,
            ErrorCollection errores, Probeta[] samples)
        {
            ITServiceAdapter iTService = new ITServiceAdapter();
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
                log.Error(string.Format("Petición: {0}", iTService.GetXmlSerialization(reportProductionDto)));

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
                        respuesta.Append(string.Format("Probeta: {0} \n", probeta.Id));
                }
            }
            return respuesta;
        }

       

        private Enumerations.ProductionReportSendStatus CastEnumProductionReportSendStatus(int value)
        {
            switch (value)
            {
                case 0:
                    return Enumerations.ProductionReportSendStatus.ForSend;
                case 1:
                    return Enumerations.ProductionReportSendStatus.Partial;
                case 2:
                    return Enumerations.ProductionReportSendStatus.Final;
                case 3:
                    return Enumerations.ProductionReportSendStatus.Complete;
                default:
                    return Enumerations.ProductionReportSendStatus.Complete;
            }
        }
        #endregion



        //public DataTable GetBoxesForPainting(int udtBox)
        //{
        //    return DataAccess.GetBoxesForPainting(udtBox);
        //}

        //public int InsLoadPintado(PaintingReport reportProductionDto)
        //{
        //    return DataAccess.InsLoadPintado(reportProductionDto);
        //}
        //public DataTable GetLoadPainting(int? childOrder, int? heatNumber, string boxUdt)
        //{
        //    return DataAccess.GetLoadPainting(childOrder, heatNumber, boxUdt);
        //}
    }
}
