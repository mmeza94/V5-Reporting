using System;
using System.Configuration;
using Tenaris.Fava.Production.Reporting.Model.Model;

namespace Tenaris.Fava.Production.Reporting.Model.DTO
{
    public class PaintingReport
    {
        public int? IdPainting { get; set; }
        public string BoxUdt { get; set; }
        public string ParentUdt { get; set; }
        public int ChildOrden { get; set; }
        public int ParentOrden { get; set; }
        public int HeatNumber { get; set; }
        public string HeatNumberCode { get; set; }
        public int LoadQuantity { get; set; }
        public int SendedQuantiry { get; set; }
        public string Storage { get; set; }
        public int NextSequence { get; set; }
        public string NextOperation { get; set; }
        public string NextOption { get; set; }
        public string LotId { get; set; }
        public string UdtType { get; set; }
        public string UdcType { get; set; }
        public int GoodCount { get; set; }
        public int ScrapCount { get; set; }
        public string IdUser { get; set; }
        public string IdHistory { get; set; }
        public DateTimeOffset InsDatetIme { get; set; }
        public DateTimeOffset UpdDatetIme { get; set; }

    }

    public interface IPaintingReportBuilder
    {
        IPaintingReportBuilder ConvertByBoxLoad(BoxLoad boxLoad);
        IPaintingReportBuilder ConvertByStockTPS(StockTPS stockTPS);
        PaintingReport Build();
    }

    public class PaintingReportBuilder : IPaintingReportBuilder
    {
        private readonly PaintingReport paintingReport;

        private PaintingReportBuilder()
        {
            paintingReport = new PaintingReport();
        }

        public static IPaintingReportBuilder Create()
        {
            return new PaintingReportBuilder();
        }

        public IPaintingReportBuilder ConvertByBoxLoad(BoxLoad boxLoad)
        {
            paintingReport.ChildOrden = Convert.ToInt32(boxLoad.Order);
            paintingReport.ParentOrden = Convert.ToInt32(boxLoad.Order);
            paintingReport.HeatNumber = Convert.ToInt32(boxLoad.Colada);
            paintingReport.HeatNumberCode = boxLoad.CodigoColada;
            paintingReport.UdtType = boxLoad.TipoUdt;
            paintingReport.BoxUdt = boxLoad.IdUdt;
            paintingReport.ParentUdt = boxLoad.IdUdt;
            paintingReport.UdcType = boxLoad.TipoUdc;
            paintingReport.LotId = boxLoad.Lote;
            paintingReport.LoadQuantity = 1;
            //Convert.ToInt32(currentDGRow.Cells["Cantidad"].Value.ToString());
            paintingReport.Storage = boxLoad.Almacen;
            paintingReport.NextSequence = Convert.ToInt32(boxLoad.SecuenciaSiguiente);
            paintingReport.NextOperation = ConfigurationManager.AppSettings["Operation_" + Configurations.Instance.Secuencia].ToString();//currentDGRow.Cells["OperacionSiguiente"].Value.ToString();
            paintingReport.NextOption = ConfigurationManager.AppSettings["Option_" + Configurations.Instance.Secuencia].ToString(); //currentDGRow.Cells["OpcionSiguiente"].Value.ToString()
            paintingReport.GoodCount = 1;
            return this;
        }

        public IPaintingReportBuilder ConvertByStockTPS(StockTPS stockTPS)
        {
            return this;
        }

        public PaintingReport Build()
        {
            return paintingReport;
        }

    }

}
