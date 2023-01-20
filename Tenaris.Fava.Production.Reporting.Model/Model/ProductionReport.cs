using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using Tenaris.Fava.Production.Reporting.ITConnection.ITService;
using Tenaris.Fava.Production.Reporting.Model.Adapter;
using Tenaris.Fava.Production.Reporting.Model.Business;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.Model;
using Tenaris.Fava.Production.Reporting.Model.NhAccess.Reporitories;
using Tenaris.Library.Framework;
using Tenaris.Library.Framework.Utility.Conversion;

namespace Tenaris.Fava.Production.Reporting.Model.Support
{
    public class ProductionReport
    {
        public static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);




        #region METHOS PRIVATE

        public string ReportProductionForPainting(
            PaintingReport reportProductionDto,
            Enumerations.ProductionReportSendStatus sendStatus,
            bool loadMaterial,
            IList rejectionReportDetails)
        {
            var respuesta = "";
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>() 
                {
                    { "@Order", reportProductionDto.ChildOrden},
                    { "@GroupItemNumber", Convert.ToInt32(reportProductionDto.BoxUdt)},
                    { "@HeatNumber", reportProductionDto.HeatNumber },
                    { "@idHistory", null},
                    { "@SendStatus", null },
                    { "@MachineSequence", 20 },
                };


                IList reportItems = ProductionReportingBusiness.GetReportProductionHistoryByParamsTest(param);

                int MaxIdHistory = 0;
                foreach (ReportProductionHistory item in reportItems)
                {
                    if (item.IdHistory > MaxIdHistory)
                    {
                        MaxIdHistory = item.IdHistory;
                    }
                }
                ReportProductionDto reportProductionDtoToSend = new ReportProductionDto
                {

                    TipoUDT = reportProductionDto.UdtType,
                    IdUDT = Convert.ToInt32(reportProductionDto.BoxUdt),
                    Colada = reportProductionDto.HeatNumber,
                    Lote = Convert.ToInt32(reportProductionDto.LotId),
                    Aprietes = 0,
                    Secuencia = reportProductionDto.NextSequence,
                    Operacion = reportProductionDto.NextOperation,
                    Opcion = reportProductionDto.NextOption,
                    CantidadBuenas = reportProductionDto.GoodCount,
                    CantidadMalas = reportProductionDto.ScrapCount,
                    CantidadReprocesadas = 0,
                    CantidadTotal = reportProductionDto.LoadQuantity,
                    IdHistory = MaxIdHistory,
                    DescripcionMaquina = "Pintado",
                    Orden = reportProductionDto.ChildOrden,
                    IdUser = reportProductionDto.IdUser,
                    Observaciones = ""
                };
                respuesta = new ITServiceAdapter().TPSReportProduction(reportProductionDto.IdUser, reportProductionDtoToSend, sendStatus, rejectionReportDetails);
            }
            catch (Exception ex)
            {
                respuesta = ex.Message.ToString();
            }
            return respuesta;
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
                log.Error(string.Format("Petición: {0}", ITServiceAdapter.GetXmlSerialization(reportProductionDto)));

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




        #endregion

    }



    public interface IFormatterPiece
    {
        IList<GeneralPiece> FormatterPiece(int order,
            int heat,
            int groupItem,
            IList<GeneralPiece> generalPieces,
            string description,
            string extremo);
    }

    public static class CollectionsUtil
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            return new ObservableCollection<T>(source);
        }
        public static ObservableCollection<T> ToObservableCollection<T>(this IList<T> source)
        {
            return new ObservableCollection<T>(source);
        }
    }

    public static class FormatterByPieces
    {
        public static ObservableCollection<GeneralPiece> FormatterPieces(
            this IEnumerable<GeneralPiece> source,
            IFormatterPiece formatterPiece)
        {
            if (source == null || formatterPiece == null)
                throw new ArgumentNullException();

            var generalPiecesClassified = new List<GeneralPiece>();
            int order = 0;
            int heat = 0;
            int groupItem = 0;
            string description = "";
            string extreme = "";
            source.OrderByDescending(Piece => Piece.InsDateTime).ForEach(Piece =>
            {
                if (order != Piece.OrderNumber || heat != Piece.HeatNumber ||
                        groupItem != Piece.GroupItemNumber || description != Piece.Description
                        || extreme != Piece.Extremo)
                {
                    order = Piece.OrderNumber;
                    heat = Piece.HeatNumber;
                    groupItem = Piece.GroupItemNumber;
                    description = Piece.Description;
                    extreme = Piece.Extremo;
                    if (!generalPiecesClassified.Exists(x => (x.OrderNumber == order)
                                                          && (x.HeatNumber == heat)
                                                          && (x.GroupItemNumber == groupItem)
                                                          && (x.Description == description)
                                                          && x.Extremo == extreme))
                        generalPiecesClassified.AddRange(formatterPiece.FormatterPiece(order, heat, groupItem, source.ToList(), description, extreme));
                }
            });
            return generalPiecesClassified.ToObservableCollection();
        }

        public static ObservableCollection<GeneralPiece> FormatterPieces(
            this IList<GeneralPiece> source,
           IFormatterPiece formatterPiece)
        {
            if (source == null)
                return source.ToObservableCollection();

            if (formatterPiece == null)
                throw new ArgumentNullException();

            var generalPiecesClassified = new List<GeneralPiece>();
            int order = 0;
            int heat = 0;
            int groupItem = 0;
            string description = "";
            string extreme = "";
            source.OrderByDescending(Piece => Piece.InsDateTime).ForEach(Piece =>
            {
                if (order != Piece.OrderNumber || heat != Piece.HeatNumber ||
                        groupItem != Piece.GroupItemNumber || description != Piece.Description
                        || extreme != Piece.Extremo)
                {
                    order = Piece.OrderNumber;
                    heat = Piece.HeatNumber;
                    groupItem = Piece.GroupItemNumber;
                    description = Piece.Description;
                    extreme = Piece.Extremo;
                    if (!generalPiecesClassified.Exists(x => (x.OrderNumber == order)
                                                          && (x.HeatNumber == heat)
                                                          && (x.GroupItemNumber == groupItem)
                                                          && (x.Description == description)
                                                          && x.Extremo == extreme))
                        generalPiecesClassified.AddRange(formatterPiece.FormatterPiece(order, heat, groupItem, source.ToList(), description, extreme));
                }
            });
            return generalPiecesClassified.ToObservableCollection();
        }
    }

    public class ProcessorPieces
    {

        private ProcessorPieces() { }

        public class ProcessorByForjas : IFormatterPiece
        {
            public IList<GeneralPiece> FormatterPiece(int order,
                int heat,
                int groupItem,
                IList<GeneralPiece> generalPieces,
                string description,
                string extremo)
            {
                var somePieces = generalPieces
                .Where(x => (x.OrderNumber == order)
            && (x.HeatNumber == heat) && (x.GroupItemNumber == groupItem)
            && (x.Description == description) && (x.Extremo == extremo))
                .OrderBy(x => x.InsDateTime)
                .ToList();

                //GeneralPiece Firstitem = somePieces.FirstOrDefault(),
                //   LastItem = somePieces.LastOrDefault();

                if (Configurations.Instance.Machine != "Forjadora 0")
                {

                    int FoundLoadedCount = ProductionReportingBusiness.GetLastMachineGoodPieces(somePieces[0].GroupItemNumber, Configurations.Instance.Secuencia.ToInteger() - 1);
                    somePieces[0].LoadedCount = FoundLoadedCount == 0 ? somePieces[0].LoadedCount : FoundLoadedCount;

                }


                if (somePieces.Count > 1)
                {
                    somePieces[somePieces.Count - 1].SendStatus =
                           (somePieces[somePieces.Count - 1].GoodCount + somePieces[somePieces.Count - 1].ScrapCount >= somePieces[somePieces.Count - 1].LoadedCount)
                           ? Enumerations.ProductionReportSendStatus.Final
                           : Enumerations.ProductionReportSendStatus.Parcial;


                    somePieces[somePieces.Count - 1].ReportSequence = (short)somePieces.Count;
                    for (int i = 0; i < somePieces.Count - 1; i++)
                    {
                        somePieces[i].ScrapCount = 0;
                        somePieces[i].ReportSequence = (short)(i + 1);
                        somePieces[i].SendStatus = Enumerations.ProductionReportSendStatus.Parcial;
                        if (i > 0)
                            somePieces[i].LoadedCount = somePieces[i - 1].LoadedCount - (somePieces[i - 1].GoodCount + somePieces[i - 1].ScrapCount);
                    }
                }
                else
                {
                    //somePieces[0].LoadedCount = somePieces[0].LoadedCount - somePieces[0].ReworkedCount; // Editamos la cantidad para que nos de las piezas programadas
                    somePieces[0].SendStatus = (somePieces[0].GoodCount + somePieces[0].ScrapCount >= somePieces[0].LoadedCount) ?
                        Enumerations.ProductionReportSendStatus.Completo : Enumerations.ProductionReportSendStatus.Parcial;
                    somePieces[0].ReportSequence = 1;
                }











                return somePieces;
            }
        }

        public class ProcessorByGranalladora : IFormatterPiece
        {
            public IList<GeneralPiece> FormatterPiece(int order,
                int heat,
                int groupItem,
                IList<GeneralPiece> generalPieces,
                string description,
                string extremo)
            {
                var somePieces = generalPieces
                .Where(x => (x.OrderNumber == order)
            && (x.HeatNumber == heat) && (x.GroupItemNumber == groupItem)
            && (x.Description == description) && (x.Extremo == extremo))
                .OrderBy(x => x.InsDateTime)
                .ToList();



                //GeneralPiece Firstitem = somePieces.FirstOrDefault(),
                //             LastItem = somePieces.LastOrDefault();



                if (somePieces.Count > 1)
                {
                    somePieces[somePieces.Count - 1].SendStatus =
                            (somePieces[somePieces.Count - 1].GoodCount + somePieces[somePieces.Count - 1].ScrapCount >= somePieces[somePieces.Count - 1].LoadedCount)
                            ? Enumerations.ProductionReportSendStatus.Final
                            : Enumerations.ProductionReportSendStatus.Parcial;


                    somePieces[somePieces.Count - 1].ReportSequence = (short)somePieces.Count;
                    for (int i = 0; i < somePieces.Count - 1; i++)
                    {
                        somePieces[i].ScrapCount = 0;
                        somePieces[i].ReportSequence = (short)(i + 1);
                        somePieces[i].SendStatus = Enumerations.ProductionReportSendStatus.Parcial;
                        if (i > 0)
                            somePieces[i].LoadedCount = somePieces[i - 1].LoadedCount - (somePieces[i - 1].GoodCount + somePieces[i - 1].ScrapCount);
                    }
                }
                else
                {
                    somePieces[0].SendStatus = (somePieces[0].GoodCount + somePieces[0].ScrapCount >= somePieces[0].LoadedCount)
                        ? Enumerations.ProductionReportSendStatus.Completo
                      : Enumerations.ProductionReportSendStatus.Parcial;
                }




                return somePieces;
            }
        }

        public class ProcessorByCoples
        : IFormatterPiece
        {
            public IList<GeneralPiece> FormatterPiece(int order,
                int heat,
                int groupItem,
                IList<GeneralPiece> generalPieces,
                string description,
                string extremo)
            {
                var somePieces = generalPieces
                .Where(x => (x.OrderNumber == order)
            && (x.HeatNumber == heat) && (x.GroupItemNumber == groupItem)
            && (x.Description == description) && (x.Extremo == extremo))
                .OrderBy(x => x.InsDateTime)
                .ToList();

                if (somePieces.Count > 1)
                {
                    
                     for (int i = 0; i < somePieces.Count; i++)
                        {
                            somePieces[i].ScrapCount = 0;
                            somePieces[i].ReportSequence = (short)(i + 1);
                            somePieces[i].SendStatus = Enumerations.ProductionReportSendStatus.Parcial;
                            if (i > 0)
                                somePieces[i].LoadedCount = somePieces[i - 1].LoadedCount - (somePieces[i - 1].GoodCount + somePieces[i - 1].ScrapCount);
                        }

                        somePieces[somePieces.Count - 1].SendStatus =
                                                (somePieces[somePieces.Count - 1].GoodCount + somePieces[somePieces.Count - 1].ScrapCount >= somePieces[somePieces.Count - 1].LoadedCount) ?
                                                Enumerations.ProductionReportSendStatus.Final : Enumerations.ProductionReportSendStatus.Parcial;
                        somePieces[somePieces.Count - 1].ReportSequence = (short)somePieces.Count;
                    }
             
                else
                {
                    somePieces[0].SendStatus = (somePieces[0].GoodCount + somePieces[0].ScrapCount >= somePieces[0].LoadedCount)
                        ? Enumerations.ProductionReportSendStatus.Completo
                      : Enumerations.ProductionReportSendStatus.Parcial;
                }




                return somePieces;
            }
        }
    }

}
