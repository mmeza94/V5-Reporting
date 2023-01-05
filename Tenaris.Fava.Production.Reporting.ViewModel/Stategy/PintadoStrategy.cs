using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Tenaris.Fava.Production.Reporting.Model.Adapter;
using Tenaris.Fava.Production.Reporting.Model.Business;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.Interfaces;
using Tenaris.Fava.Production.Reporting.Model.Model;
using Tenaris.Fava.Production.Reporting.Model.Support;
using Tenaris.Fava.Production.Reporting.ViewModel.Dialog;
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
        private ShowQuestion question;
        private int reportedPieces;
        private PaintingReportConfirmationViewModel reportConfirmation;
        private ITServiceAdapter serviceAdapter = new ITServiceAdapter();
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
            try
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
            }
            catch (Exception) { }
            return this;
        }

        public IActions Report()
        {
            try
            {
                if (Filters["type_action"].ToString().ToInteger() == 2)
                {
                    reporting();
                    return this;
                }
                loadReport();
            }
            catch (Exception) { }

            return this;
        }

        public ObservableCollection<ReportProductionHistory> dgReporteProduccion_SelectionChanged(GeneralPiece SelectedBundle)
        {
            throw new NotImplementedException();
        }

        private void reporting()
        {
            BoxLoad box = BoxLoadBuilder
                .ManipulateObject(Filters["selectedLoaded"])
                .Build();

            int totals = box.Cantidad.ToInteger() - reportedPieces;

            PaintingReport paintingReport = PaintingReportBuilder
                .Create()
                .ConvertByBoxLoad(box)
                .WithLoadQuantity(totals)
                .WithGoodCount(totals)
                .Build();

            reportConfirmation =
               new PaintingReportConfirmationViewModel(paintingReport, ProductionReportingBusiness.GetCurrentUser());

            if (paintingReport.LoadQuantity == 0)
            {
                question = new ShowQuestion("Mensaje de Carga", "La caja ya tiene reportado por Nivel 2 todas las piezas cargadas a Pintado, desea continuar con el envio? ");
                ShowQuestionRequests.Raise(new Notification() { Content = question });
            }

            if (paintingReport.LoadQuantity == 0 && !question.Result)
            {
                Search();
                return;
            }

            Request.Raise(new Notification() { Content = reportConfirmation });

            if (!reportConfirmation.Result)
            {
                Search();
                return;
            }

            question = new ShowQuestion("Confirmar Reporte",
                string.Format("Resumen de lo Reportado: \n\n Buenas:{0} \n" + " Malas:{1} \n Reprocesos:{2} \n Total:{3} \n\n ¿Desea reportar estas cantidades?"
                                            , reportConfirmation.BuenasActual
                                            , reportConfirmation.MalasActual
                                            , reportConfirmation.ReprocesosActual
                                            , reportConfirmation.CargadasActual));

            ShowQuestionRequests.Raise(new Notification() { Content = question });

            if (!question.Result)
            {
                Search();
                return;
            }



            int cantidadTotal = reportConfirmation.DisponiblesTPS - (reportConfirmation.CargadasAnterior == 0 ? 0 : reportConfirmation.CargadasAnterior);

            ShowMessage showMessage = new ShowMessage("Reporte de Producción",
                new ProductionReport()
                .ReportProductionForPainting(
                    PaintingReportBuilder
                        .ManipulatePaintingReport(paintingReport)
                        .WithLoadQuantity(cantidadTotal == 0 ? reportConfirmation.CargadasActual : cantidadTotal)
                        .WithGoodCount(reportConfirmation.BuenasActual)
                        .WithScrapCount(reportConfirmation.MalasActual)
                        .WithIdUser(reportConfirmation.UserReport)
                      .Build(),
                    Enumerations.ProductionReportSendStatus.Parcial,
                    true,
                    reportConfirmation.rejectionReportDetails));

            ShowMessageRequest.Raise(new Notification() { Content = showMessage });
            Search();
        }

        private void loadReport()
        {
            try
            {
                StockTPS stockTPS = StockTPSBuilder
                                .ManipulateObject(Filters["selectedTPS"])
                                .Build();

                int totals = stockTPS.Cantidad.ToInteger();

                PaintingReport paintingReport = PaintingReportBuilder
                    .Create()
                    .ConvertByStockTPS(stockTPS)
                    .WithLoadQuantity(totals)
                    .WithGoodCount(totals)
                    .Build();

                bool response = serviceAdapter.TPSLoadMaterialForPainting(paintingReport);
                string message = response ? "Caja Cargada correctamente" : "Error al cargar la caja";
                ShowMessageRequest.Raise(new Notification() { Content = new ShowMessage("Mensaje de Carga", message) });
            }
            catch (Exception) { }
            Search();
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
