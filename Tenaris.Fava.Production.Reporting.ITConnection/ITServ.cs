using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace Tenaris.Fava.Production.Reporting.ITConnection
{
    public class ITServ
    {
        public string GetAvaibleStockIT(string OP, string Operacion, string OpcionConsola, string IdProducto, string VersionProducto)
        {
            ITService.TServiceClient Accesories = null;
            ITService.ErrorCollection errores = null;



            Accesories = new ITService.TServiceClient();
            //WebService ws = new WebService();
            string result = Accesories.GetAccesoriesAvailableStock(out errores,OP, Operacion, OpcionConsola, VersionProducto, IdProducto );

            return result;
        }




        //ProductionBoxReport
        public ITService.ProductionBox[] GetProductionBoxesIT(out ITService.ErrorCollection errors,int orderNumber, string machineId, string operationId)
        {


            ITService.TServiceClient client = new ITService.TServiceClient();
            ITService.ProductionBox[] availableBoxes = client.GetProductionBoxes(out errors,orderNumber, machineId, operationId);
            return availableBoxes;
        }


        public bool LoadProductionBoxIT(string boxId, string machineId, string operationId, int missingPieces, int sequenceProcess, string workUnit, out ITService.ErrorCollection errors)
        {

            ITService.TServiceClient client = new ITService.TServiceClient();
            bool result = client.LoadProductionBox(out errors,boxId, machineId, operationId, missingPieces, sequenceProcess, workUnit);
            return result;
        }

        public bool UnLoadProductionBoxIT(string boxId, string machineId, string operationId, int pieces, int sequenceProcess, string workUnit, out ITService.ErrorCollection errors)
        {
            ITService.TServiceClient client = new ITService.TServiceClient();
            bool result = client.UnloadProductionBox(out errors,boxId, machineId, operationId, pieces, sequenceProcess, workUnit);
            return result;
        }

        public bool ReportProductionBoxIT(out ITService.ErrorCollection errors,string user, string workUnitSource, string workUnitSourceId, int heat, int idLot, int sequenceProcess, string operationId, string machineId, decimal goodPieces,
            decimal workedPieces, decimal reworkedPieces, decimal totalPieces, int endHeat, string boxId, int endIdLot, string comments, string boxType, ITService.Descarte[] tpsDiscards)
        {

            ITService.TServiceClient client = new ITService.TServiceClient();

            bool result = client.ReportProductionBox(out errors,user, workUnitSource, workUnitSourceId, heat, idLot, sequenceProcess, operationId, machineId,
                    goodPieces, workedPieces, reworkedPieces, totalPieces, endHeat, boxId, endIdLot, comments, boxType, tpsDiscards);
            return result;
        }






        //ProductionReport
        public bool LoadMaterialIT(out ITService.ErrorCollection errores,int orderNumber, int heat, string tipoUDT, string idUDT, string almacen, string cantidad, string secuencia, string observaciones, string operacion, string opcionConsola, string lote )
        {

            ITService.TServiceClient fava = new ITService.TServiceClient();

            bool result = //true;
                     fava.LoadMaterial(
                   out errores,
                   orderNumber,
                   heat,
                   tipoUDT,
                   idUDT,
                   almacen,
                   //reportProductionDto.CantidadProcesadas.ToString(),
                   cantidad,
                   secuencia,
                   "observaciones",
                   operacion,
                   opcionConsola,
                   lote);
            return result;
        }


        public bool ReportProductionWProbesIT(string user,
                string TipoUDT,
                    int IdUDT,
                    int Colada,
                    int Lote,
                    int Aprietes,
                    int Secuencia,
                    string Operacion,
                    string Opcion,
                    decimal CantidadBuenas,
                    decimal CantidadProcesadas,
                    decimal CantidadReprocesadas,
                    decimal CantidadTotal,
                    int ColadaSalida,
                    int IdUDTSalida,
                    int LoteSalida,
                    string Observaciones,
                    string TipoUDTSalida,
                    ITService.Descarte[] Descartes,
                    ITService.ProbetaAsociada[] Probetas,
                    out ITService.ErrorCollection errores)
        {

            ITService.TServiceClient fava = new ITService.TServiceClient();
            bool result = fava.ReportProductionWProbes( out errores,user, TipoUDT, IdUDT, Colada, Lote, Aprietes, Secuencia, Operacion, Opcion,
                            CantidadBuenas, CantidadProcesadas, CantidadReprocesadas, CantidadTotal, ColadaSalida, IdUDTSalida, LoteSalida,
                            Observaciones, TipoUDTSalida, Descartes, Probetas);
            return result;
        }

        public bool ReportProductionIT(string user,
                    string TipoUDT,
                    int IdUDT,
                    int Colada,
                    int Lote,
                    int Aprietes,
                    int Secuencia,
                    string Operacion,
                    string Opcion,
                    decimal CantidadBuenas,
                    decimal CantidadProcesadas,
                    decimal CantidadReprocesadas,
                    decimal CantidadTotal,
                    int ColadaSalida,
                    int IdUDTSalida,
                    int LoteSalida,
                    string Observaciones,
                    string TipoUDTSalida,
                    ITService.Descarte[] Descartes,
                    out ITService.ErrorCollection errores)
        {

            // ITService.ErrorCollection errores = null;
            ITService.TServiceClient fava = new ITService.TServiceClient();
            bool result = fava.ReportProduction(out errores,user, TipoUDT, IdUDT, Colada, Lote, Aprietes, Secuencia, Operacion, Opcion,
                            CantidadBuenas, CantidadProcesadas, CantidadReprocesadas, CantidadTotal, ColadaSalida, IdUDTSalida, LoteSalida,
                            Observaciones, TipoUDTSalida, Descartes);
            return result;
        }


        public ITService.OPSpecContainer GetAccesoriesSpecificationIT(ITService.ProductKey prodkey)
        {
            ITService.OPSpecContainer prueba = null;
            ITService.TServiceClient fava = new ITService.TServiceClient();
            fava.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(300);
            // ITService.OPSpecContainer result = fava.GetAccesoriesSpecification(prodkey);
            var X = fava.GetAccesoriesSpecificationForRuta(prodkey);

            return prueba;
                     
        }










    }
}
