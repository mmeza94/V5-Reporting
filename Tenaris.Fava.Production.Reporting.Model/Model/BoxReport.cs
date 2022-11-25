using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenaris.Fava.Production.Reporting.Model.Model
{
    public class BoxReport
    {
        private string caja;
        private string colada;
        private string ordenHija;
        private string ordenOrigen;
        private string tipoUdt;
        private string maquinaAnterior;
        private string machineOperation;
        private string cantidadTotal;
        private string piezasBuenas;
        private string piezasMalas;
        private string idMachine;

        public string Caja { get => caja; set => caja = value; }
        public string Colada { get => colada; set => colada = value; }
        public string OrdenHija { get => ordenHija; set => ordenHija = value; }
        public string OrdenOrigen { get => ordenOrigen; set => ordenOrigen = value; }
        public string TipoUdt { get => tipoUdt; set => tipoUdt = value; }
        public string MaquinaAnterior { get => maquinaAnterior; set => maquinaAnterior = value; }
        public string MachineOperation { get => machineOperation; set => machineOperation = value; }
        public string CantidadTotal { get => cantidadTotal; set => cantidadTotal = value; }
        public string PiezasBuenas { get => piezasBuenas; set => piezasBuenas = value; }
        public string PiezasMalas { get => piezasMalas; set => piezasMalas = value; }
        public string IdMachine { get => idMachine; set => idMachine = value; }


        //METODO CONSTRUCTOR

        public BoxReport()
        {

        }
        public BoxReport(string caja, string colada, string ordenHija, string tipoUdt, string maquinaAnterior,
                        string machineOperation, string cantidadTotal, string piezasBuenas, string piezasMalas, string idMachine)
        {
            this.caja = caja;
            this.colada = colada;
            this.ordenHija = ordenHija;
            this.tipoUdt = tipoUdt;
            this.maquinaAnterior = maquinaAnterior;
            this.machineOperation = machineOperation;
            this.cantidadTotal = cantidadTotal;
            this.piezasBuenas = piezasBuenas;
            this.piezasMalas = piezasMalas;
            this.idMachine = idMachine;
        }
    }
}
