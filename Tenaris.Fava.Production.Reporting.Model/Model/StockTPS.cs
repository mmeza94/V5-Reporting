namespace Tenaris.Fava.Production.Reporting.Model.Model
{

    public class StockTPS
    {

        public string Colada { get; set; }
        public string CodigoColada { get; set; }
        public string TipoUdt { get; set; }
        public string IdUdt { get; set; }
        public string Lote { get; set; }
        public string Cantidad { get; set; }
        public string Almacen { get; set; }
        public string Extremo { get; set; }
        public string SecuenciaSiguiente { get; set; }
        public string OperacionSiguiente { get; set; }
        public string OpcionSiguiente { get; set; }
        public string Lot4 { get; set; }
        public string LotId { get; set; }
        public string ProductReportBox { get; set; }
        public string Order { get; set; }
        public string TipoUdc { get; set; }

        public StockTPS() { }

        public StockTPS(string order,
            string colada,
            string codigoColada,
            string tipoUdt,
            string idUdt,
            string tipoUdc,
            string lote,
            string cantidad,
            string almacen,
            string extremo,
            string secuenciaSiguiente,
            string operacionSiguiente,
            string opcionSiguiente,
            string lot4,
            string lotId,
            string productBoxReport)
        {
            this.Order = order;
            this.Colada = colada;
            this.CodigoColada = codigoColada;
            this.TipoUdt = tipoUdt;
            this.IdUdt = idUdt;
            this.TipoUdc = tipoUdc;
            this.Lote = lote;
            this.Cantidad = cantidad;
            this.Almacen = almacen;
            this.Extremo = extremo;
            this.SecuenciaSiguiente = secuenciaSiguiente;
            this.OperacionSiguiente = operacionSiguiente;
            this.OpcionSiguiente = opcionSiguiente;
            this.Lot4 = lot4;
            this.LotId = lotId;
            this.ProductReportBox = productBoxReport;
        }
    }

    public interface IStockTPSBuilder
    {
        IStockTPSBuilder WithColada(string value);
        IStockTPSBuilder WithCodigoColada(string value);
        IStockTPSBuilder WithTipoUdt(string value);
        IStockTPSBuilder WithIdUdt(string value);
        IStockTPSBuilder WithLote(string value);
        IStockTPSBuilder WithCantidad(string value);
        IStockTPSBuilder WithAlmacen(string value);
        IStockTPSBuilder WithExtremo(string value);
        IStockTPSBuilder WithSecuenciaSiguiente(string value);
        IStockTPSBuilder WithOperacionSiguiente(string value);
        IStockTPSBuilder WithOpcionSiguiente(string value);
        IStockTPSBuilder WithLot4(string value);
        IStockTPSBuilder WithLotId(string value);
        IStockTPSBuilder WithProductReportBox(string value);
        IStockTPSBuilder WithOrder(string value);
        IStockTPSBuilder WithTipoUdc(string value);
        StockTPS Build();
    }

    public class StockTPSBuilder : IStockTPSBuilder
    {

        private readonly StockTPS stockTPS;

        private StockTPSBuilder() { stockTPS = new StockTPS(); }
        private StockTPSBuilder(object obj) { stockTPS = (StockTPS)obj; }

        public static IStockTPSBuilder Create()
        {
            return new StockTPSBuilder();
        }

        public static IStockTPSBuilder ManipulateObject(object obj)
        {
            return new StockTPSBuilder(obj);
        }

        public IStockTPSBuilder WithAlmacen(string value)
        {
            stockTPS.Almacen = value;
            return this;
        }

        public IStockTPSBuilder WithCantidad(string value)
        {
            stockTPS.Cantidad = value;
            return this;
        }

        public IStockTPSBuilder WithCodigoColada(string value)
        {
            stockTPS.CodigoColada = value;
            return this;
        }

        public IStockTPSBuilder WithColada(string value)
        {
            stockTPS.Colada = value;
            return this;
        }

        public IStockTPSBuilder WithExtremo(string value)
        {
            stockTPS.Extremo = value;
            return this;
        }

        public IStockTPSBuilder WithIdUdt(string value)
        {
            stockTPS.IdUdt = value;
            return this;
        }

        public IStockTPSBuilder WithLot4(string value)
        {
            stockTPS.Lot4 = value;
            return this;
        }

        public IStockTPSBuilder WithLote(string value)
        {
            stockTPS.Lote = value;
            return this;
        }

        public IStockTPSBuilder WithLotId(string value)
        {
            stockTPS.LotId = value;
            return this;
        }

        public IStockTPSBuilder WithOperacionSiguiente(string value)
        {
            stockTPS.OperacionSiguiente = value;
            return this;
        }

        public IStockTPSBuilder WithOrder(string value)
        {
            stockTPS.Order = value;
            return this;
        }

        public IStockTPSBuilder WithProductReportBox(string value)
        {
            stockTPS.ProductReportBox = value;
            return this;
        }

        public IStockTPSBuilder WithSecuenciaSiguiente(string value)
        {
            stockTPS.SecuenciaSiguiente = value;
            return this;
        }

        public IStockTPSBuilder WithTipoUdc(string value)
        {
            stockTPS.TipoUdc = value;
            return this;
        }

        public IStockTPSBuilder WithTipoUdt(string value)
        {
            stockTPS.TipoUdt = value;
            return this;
        }
        public IStockTPSBuilder WithOpcionSiguiente(string value)
        {
            stockTPS.OpcionSiguiente = value;
            return this;
        }

        public StockTPS Build()
        {
            return stockTPS;
        }


    }

}
