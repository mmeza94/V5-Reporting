namespace Tenaris.Fava.Production.Reporting.Model.Model
{

    public class BoxLoad
    {

        public string IdLoadPainting { get; set; }
        public string Order { get; set; }
        public string Colada { get; set; }
        public string CodigoColada { get; set; }
        public string TipoUdt { get; set; }
        public string IdUdt { get; set; }
        public string TipoUdc { get; set; }
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
        public string Active { get; set; }

        public BoxLoad() { }

        public BoxLoad(string idLoadPainting,
            string order,
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
            string opcionSiguiente,
            string lot4,
            string lotId,
            string productBoxReport,
            string active)
        {
            this.IdLoadPainting = idLoadPainting;
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
            this.OpcionSiguiente = opcionSiguiente;
            this.Lot4 = lot4;
            this.LotId = lotId;
            this.ProductReportBox = productBoxReport;
            this.Active = active;
        }
    }

    public interface IBoxLoadBuilder
    {
        IBoxLoadBuilder WithIdLoadPainting(string value);
        IBoxLoadBuilder WithOrder(string value);
        IBoxLoadBuilder WithColada(string value);
        IBoxLoadBuilder WithCodigoColada(string value);
        IBoxLoadBuilder WithTipoUdt(string value);
        IBoxLoadBuilder WithIdUdt(string value);
        IBoxLoadBuilder WithTipoUdc(string value);
        IBoxLoadBuilder WithLote(string value);
        IBoxLoadBuilder WithCantidad(string value);
        IBoxLoadBuilder WithAlmacen(string value);
        IBoxLoadBuilder WithExtremo(string value);
        IBoxLoadBuilder WithSecuenciaSiguiente(string value);
        IBoxLoadBuilder WithOperacionSiguiente(string value);
        IBoxLoadBuilder WithOpcionSiguiente(string value);
        IBoxLoadBuilder WithLot4(string value);
        IBoxLoadBuilder WithLotId(string value);
        IBoxLoadBuilder WithProductReportBox(string value);
        IBoxLoadBuilder WithActive(string value);
        BoxLoad Build();
    }

    public class BoxLoadBuilder : IBoxLoadBuilder
    {
        private readonly BoxLoad boxLoad;

        #region Constructors
        private BoxLoadBuilder() { boxLoad = new BoxLoad(); }
        private BoxLoadBuilder(object obj) { boxLoad = (BoxLoad)obj; }
        #endregion

        #region Cases
        public static IBoxLoadBuilder Create()
        {
            return new BoxLoadBuilder();
        }

        public static IBoxLoadBuilder ManipulateObject(object obj)
        {
            return new BoxLoadBuilder(obj);
        }
        #endregion

        public IBoxLoadBuilder WithActive(string value)
        {
            throw new System.NotImplementedException();
        }

        public IBoxLoadBuilder WithAlmacen(string value)
        {
            throw new System.NotImplementedException();
        }

        public IBoxLoadBuilder WithCantidad(string value)
        {
            throw new System.NotImplementedException();
        }

        public IBoxLoadBuilder WithCodigoColada(string value)
        {
            throw new System.NotImplementedException();
        }

        public IBoxLoadBuilder WithColada(string value)
        {
            throw new System.NotImplementedException();
        }

        public IBoxLoadBuilder WithExtremo(string value)
        {
            throw new System.NotImplementedException();
        }

        public IBoxLoadBuilder WithIdLoadPainting(string value)
        {
            throw new System.NotImplementedException();
        }

        public IBoxLoadBuilder WithIdUdt(string value)
        {
            throw new System.NotImplementedException();
        }

        public IBoxLoadBuilder WithLot4(string value)
        {
            throw new System.NotImplementedException();
        }

        public IBoxLoadBuilder WithLote(string value)
        {
            throw new System.NotImplementedException();
        }

        public IBoxLoadBuilder WithLotId(string value)
        {
            throw new System.NotImplementedException();
        }

        public IBoxLoadBuilder WithOpcionSiguiente(string value)
        {
            throw new System.NotImplementedException();
        }

        public IBoxLoadBuilder WithOperacionSiguiente(string value)
        {
            throw new System.NotImplementedException();
        }

        public IBoxLoadBuilder WithOrder(string value)
        {
            throw new System.NotImplementedException();
        }

        public IBoxLoadBuilder WithProductReportBox(string value)
        {
            throw new System.NotImplementedException();
        }

        public IBoxLoadBuilder WithSecuenciaSiguiente(string value)
        {
            throw new System.NotImplementedException();
        }

        public IBoxLoadBuilder WithTipoUdc(string value)
        {
            throw new System.NotImplementedException();
        }

        public IBoxLoadBuilder WithTipoUdt(string value)
        {
            throw new System.NotImplementedException();
        }

        public BoxLoad Build()
        {
            return boxLoad;
        }

    }

}
