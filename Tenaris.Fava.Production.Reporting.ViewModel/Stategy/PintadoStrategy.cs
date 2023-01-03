using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Tenaris.Fava.Production.Reporting.Model.Adapter;
using Tenaris.Fava.Production.Reporting.Model.Business;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Interfaces;
using Tenaris.Fava.Production.Reporting.Model.Model;
using Tenaris.Fava.Production.Reporting.Model.Support;
using Tenaris.Fava.Production.Reporting.ViewModel.Interfaces;
using Tenaris.Fava.Production.Reporting.ViewModel.Stategy.RProcess;
using Tenaris.Library.Framework.Utility.Conversion;

namespace Tenaris.Fava.Production.Reporting.ViewModel.Stategy
{
    public class PintadoStrategy : GeneralMachine, IActions
    {
        #region Properties
        public Dictionary<string, object> Filters { get; set; }
        public Dictionary<string, object> OutPuts { get; set; }
        public GeneralMachine GeneralMachine => this;
        public IReportingProcess reportingProcess { get; set; }
        private int reportedPieces;
        #endregion

        #region Constructor
        public PintadoStrategy()
        {
            reportingProcess = new RPGeneral(this);
            Filters = Filter;
            OutPuts = OutPut;
        }
        #endregion

        #region Methods

        public IActions Search()
        {
            ObservableCollection<BoxReport> BoxesForPainting = ProductionReportingBusiness.GetBoxesForPainting(Filters),
                filteredTable = BoxesForPainting.Where(Box => Box.MachineOperation.Equals("Mecanizado Extremo 2")).ToObservableCollection(),
                reported = BoxesForPainting.Where(Box => Box.MachineOperation.Equals("Pintado")).ToObservableCollection();
            ObservableCollection<StockTPS> stockTPs = new ObservableCollection<StockTPS>();
            ObservableCollection<BoxLoad> LoadPainting = ProductionReportingBusiness.GetLoadPainting(Filters);

            if (filteredTable.Count > 0)
                stockTPs = new ITServiceAdapter()
                    .GetAvailableStock(filteredTable.FirstOrDefault().OrdenHija, 11)
                    .Select($"IdUdt = '{int.Parse(Filters["@UdtBox"].ToString())}'")
                    .Select(stockTPS)
                    .ToObservableCollection();

            if (reported.Count > 0)
                reportedPieces = reported.Sum(Box => Box.PiezasBuenas.ToInteger());

            AddValues("ReportesDeCajaRef", BoxesForPainting)
                .AddValues("CajasCargadasRef", LoadPainting)
                .AddValues("StockParaTPSRef", stockTPs);

            return this;
        }

        public IActions Report()
        {
            PaintingReportBuilder
                .Create()
                .ConvertByBoxLoad((BoxLoad)Filters["selectedLoaded"]);
            Search();
            return this;
        }

        public ObservableCollection<ReportProductionHistory> dgReporteProduccion_SelectionChanged(GeneralPiece SelectedBundle)
        {
            throw new NotImplementedException();
        }

        private static StockTPS stockTPS(DataRow row)
        {
            return StockTPSBuilder
                .Create()
                .WithOrder(row[0].ToString())
                .WithColada(row[1].ToString())
                .WithCodigoColada(row[2].ToString())
                .WithTipoUdt(row[3].ToString())
                .WithIdUdt(row[4].ToString())
                .WithTipoUdc(row[5].ToString())
                .WithLote(row[6].ToString())
                .WithCantidad(row[7].ToString())
                .WithAlmacen(row[8].ToString())
                .WithExtremo(row[9].ToString())
                .WithSecuenciaSiguiente(row[10].ToString())
                .WithOperacionSiguiente(row[11].ToString())
                .WithOpcionSiguiente(row[12].ToString())
                .WithLot4(row[13].ToString())
                .WithLotId(row[14].ToString())
                .WithProductReportBox(row[15].ToString())
                .Build();
        }

        #endregion
    }
}
