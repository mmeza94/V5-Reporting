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

        IPaintingReportBuilder WithIdPainting(int? value);
        IPaintingReportBuilder WithBoxUdt(string value);
        IPaintingReportBuilder WithParentUdt(string value);
        IPaintingReportBuilder WithChildOrden(int value);
        IPaintingReportBuilder WithParentOrden(int value);
        IPaintingReportBuilder WithHeatNumber(int value);
        IPaintingReportBuilder WithHeatNumberCode(string value);
        IPaintingReportBuilder WithLoadQuantity(int value);
        IPaintingReportBuilder WithSendedQuantiry(int value);
        IPaintingReportBuilder WithStorage(string value);
        IPaintingReportBuilder WithNextSequence(int value);
        IPaintingReportBuilder WithNextOperation(string value);
        IPaintingReportBuilder WithNextOption(string value);
        IPaintingReportBuilder WithLotId(string value);
        IPaintingReportBuilder WithUdtType(string value);
        IPaintingReportBuilder WithUdcType(string value);
        IPaintingReportBuilder WithGoodCount(int value);
        IPaintingReportBuilder WithScrapCount(int value);
        IPaintingReportBuilder WithIdUser(string value);
        IPaintingReportBuilder WithIdHistory(string value);
        IPaintingReportBuilder WithInsDatetIme(DateTimeOffset value);
        IPaintingReportBuilder WithUpdDatetIme(DateTimeOffset value);
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

        public IPaintingReportBuilder WithIdPainting(int? value)
        {
            paintingReport.IdPainting = value;
            return this;
        }

        public IPaintingReportBuilder WithBoxUdt(string value)
        {
            paintingReport.BoxUdt = value;
            return this;
        }

        public IPaintingReportBuilder WithParentUdt(string value)
        {
            paintingReport.ParentUdt = value;
            return this;
        }

        public IPaintingReportBuilder WithChildOrden(int value)
        {
            paintingReport.ChildOrden = value;
            return this;
        }

        public IPaintingReportBuilder WithParentOrden(int value)
        {
            paintingReport.ParentOrden = value;
            return this;
        }

        public IPaintingReportBuilder WithHeatNumber(int value)
        {
            paintingReport.HeatNumber = value;
            return this;
        }

        public IPaintingReportBuilder WithHeatNumberCode(string value)
        {
            paintingReport.HeatNumberCode = value;
            return this;
        }

        public IPaintingReportBuilder WithLoadQuantity(int value)
        {
            paintingReport.LoadQuantity = value;
            return this;
        }

        public IPaintingReportBuilder WithSendedQuantiry(int value)
        {
            paintingReport.SendedQuantiry = value;
            return this;
        }

        public IPaintingReportBuilder WithStorage(string value)
        {
            paintingReport.Storage = value;
            return this;
        }

        public IPaintingReportBuilder WithNextSequence(int value)
        {
            paintingReport.NextSequence = value;
            return this;
        }

        public IPaintingReportBuilder WithNextOperation(string value)
        {
            paintingReport.NextOperation = value;
            return this;
        }

        public IPaintingReportBuilder WithNextOption(string value)
        {
            paintingReport.NextOption = value;
            return this;
        }

        public IPaintingReportBuilder WithLotId(string value)
        {
            paintingReport.LotId = value;
            return this;
        }

        public IPaintingReportBuilder WithUdtType(string value)
        {
            paintingReport.UdtType = value;
            return this;
        }

        public IPaintingReportBuilder WithUdcType(string value)
        {
            paintingReport.UdcType = value;
            return this;
        }

        public IPaintingReportBuilder WithGoodCount(int value)
        {
            paintingReport.GoodCount = value;
            return this;
        }

        public IPaintingReportBuilder WithScrapCount(int value)
        {
            paintingReport.ScrapCount = value;
            return this;
        }

        public IPaintingReportBuilder WithIdUser(string value)
        {
            paintingReport.IdUser = value;
            return this;
        }

        public IPaintingReportBuilder WithIdHistory(string value)
        {
            paintingReport.IdHistory = value;
            return this;
        }

        public IPaintingReportBuilder WithInsDatetIme(DateTimeOffset value)
        {
            paintingReport.InsDatetIme = value;
            return this;
        }

        public IPaintingReportBuilder WithUpdDatetIme(DateTimeOffset value)
        {
            paintingReport.UpdDatetIme = value;
            return this;
        }

        public PaintingReport Build()
        {
            return paintingReport;
        }

    }

}
