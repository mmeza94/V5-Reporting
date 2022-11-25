using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenaris.Fava.Production.Reporting.Model.Model
{
    public class BoxLoad
    {
        private string idLoadPainting;
        private string order;
        private string colada;
        private string codigoColada;
        private string tipoUdt;
        private string idUdt;
        private string tipoUdc;
        private string lote;
        private string cantidad;
        private string almacen;
        private string extremo;
        private string secuenciaSiguiente;
        private string operacionSiguiente;
        private string opcionSiguiente;
        private string lot4;
        private string lotId;
        private string productReportBox;
        private string active;

        public string IdLoadPainting { get => idLoadPainting; set => idLoadPainting = value; }
        public string Order { get => order; set => order = value; }
        public string Colada { get => colada; set => colada = value; }
        public string CodigoColada { get => codigoColada; set => codigoColada = value; }
        public string TipoUdt { get => tipoUdt; set => tipoUdt = value; }
        public string IdUdt { get => idUdt; set => idUdt = value; }
        public string TipoUdc { get => tipoUdc; set => tipoUdc = value; }
        public string Lote { get => lote; set => lote = value; }
        public string Cantidad { get => cantidad; set => cantidad = value; }
        public string Almacen { get => almacen; set => almacen = value; }
        public string Extremo { get => extremo; set => extremo = value; }
        public string SecuenciaSiguiente { get => secuenciaSiguiente; set => secuenciaSiguiente = value; }
        public string OperacionSiguiente { get => operacionSiguiente; set => operacionSiguiente = value; }
        public string OpcionSiguiente { get => opcionSiguiente; set => opcionSiguiente = value; }
        public string Lot4 { get => lot4; set => lot4 = value; }
        public string LotId { get => lotId; set => lotId = value; }
        public string ProductReportBox { get => productReportBox; set => productReportBox = value; }
        public string Active { get => active; set => active = value; }





        //METODO CONSTRUCTOR
        public BoxLoad()
        {

        }



        public BoxLoad(string idLoadPainting, string order, string colada, string codigoColada, string tipoUdt, string idUdt,
                            string tipoUdc, string lote, string cantidad, string almacen, string extremo, string secuenciaSiguiente,
                            string opcionSiguiente, string lot4, string lotId, string productBoxReport, string active)
        {
            this.idLoadPainting = idLoadPainting;
            this.order = order;
            this.colada = colada;
            this.codigoColada = codigoColada;
            this.tipoUdt = tipoUdt;
            this.idUdt = idUdt;
            this.tipoUdc = tipoUdc;
            this.lote = lote;
            this.cantidad = cantidad;
            this.almacen = almacen;
            this.extremo = extremo;
            this.secuenciaSiguiente = secuenciaSiguiente;
            this.opcionSiguiente = opcionSiguiente;
            this.lot4 = lot4;
            this.lotId = lotId;
            this.productReportBox = productBoxReport;
            this.active = active;

        }
    }
}
