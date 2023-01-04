using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
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
            if (Filters["type_action"].ToString().ToInteger() == 2)
            {
                reporting();
                return this;
            }
            loadReport();
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
               new PaintingReportConfirmationViewModel(paintingReport, "TestUser");

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

            //PaintingReportConfirmationSupport.Report(reportConfirmation.DisponiblesTPS,
            //                                         reportConfirmation.CargadasAnterior,
            //                                         reportConfirmation.BuenasActual,
            //                                         reportConfirmation.MalasActual,
            //                                         reportConfirmation.ReprocesosActual,
            //                                         reportConfirmation.CargadasActual,
            //                                         reportConfirmation.currentProductionReportDto,
            //                                         reportConfirmation.rejectionReportDetails,
            //                                         reportConfirmation.UserReport,
            //                                         ShowQuestionRequests, ShowMessageRequest,
            //                                         ShowErrorMessageRequest);

            question = new ShowQuestion("Confirmar Reporte",
                string.Format("Resumen de lo Reportado: \n\n Buenas:{0} \n" + " Malas:{1} \n Reprocesos:{2} \n Total:{3} \n\n ¿Desea reportar estas cantidades?"
                                            , reportConfirmation.BuenasActual
                                            , reportConfirmation.MalasActual
                                            , reportConfirmation.ReprocesosActual
                                            , reportConfirmation.CargadasActual));

            ShowQuestionRequests.Raise(new Notification() { Content = question });



        }

        private void loadReport()
        {

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
