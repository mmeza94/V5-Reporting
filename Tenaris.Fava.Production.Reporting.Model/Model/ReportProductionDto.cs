using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.Enums;

namespace Tenaris.Fava.Production.Reporting.Model.DTO
{
    public class ReportProductionDto
    {
        public int IdHistory { get; set; }
        public int Orden { get; set; }
        public string Almacen { get; set; }
        public string TipoUDT { get; set; }
        public int IdUDT { get; set; }
        public int Colada { get; set; }
        public int Lote { get; set; }
        public int Aprietes { get; set; }
        public int Secuencia { get; set; }
        public string Operacion { get; set; }
        public string Opcion { get; set; }
        public int CantidadBuenas { get; set; }
        public string DescripcionMaquina { get; set; }
        public int CantidadMalas { get; set; }
        public Enumerations.AxlrBit Enviado { get; set; }
        
        public int CantidadProcesadas
        {
            get { return CantidadBuenas + CantidadMalas; }
        }
        public int CantidadReprocesadas { get; set; }
        public int CantidadTotal { get; set; }
        public int ColadaSalida
        {

            get { return Colada; }
        }
        public int IdUDTSalida
        {
            get { return IdUDT; }
        }
        public int LoteSalida
        {
            get { return Lote; }
        }
        public string Observaciones { get; set; }
        public string TipoUDTSalida
        {
            get { return TipoUDT; }
        }
        public int IdBatch { get; set; }
        public string IdUser { get; set; }
    }
}

