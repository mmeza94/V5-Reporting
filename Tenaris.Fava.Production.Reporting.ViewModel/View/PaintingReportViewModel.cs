using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.ViewModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Tenaris.Fava.Production.Reporting.Model.Interfaces;
using Tenaris.Fava.Production.Reporting.Model.Model;
using Tenaris.Fava.Production.Reporting.ViewModel.Reflection;
using Tenaris.Fava.Production.Reporting.ViewModel.Support;

namespace Tenaris.Fava.Production.Reporting.ViewModel.View
{
    public class PaintingReportViewModel : NotificationObject
    {

        private readonly IActions Actions = ReflectionStrategy.MyStrategy;

        #region Private Properties
        private ObservableCollection<StockTPS> stockParaTPS;
        private ObservableCollection<BoxLoad> cajasCargadas;
        private ObservableCollection<BoxReport> reportesDeCaja;
        private int cajon;
        private StockTPS selectedTPS;
        private BoxLoad selectedLoaded;
        #endregion

        #region Private Interactivity
        private InteractionRequest<Notification> paintReportConfirmationWindowRequest { get; set; }
        private InteractionRequest<Notification> showQuestionWindowRequest { get; set; }
        private InteractionRequest<Notification> showMessageWindowRequest { get; set; }
        private InteractionRequest<Notification> showErrorWindowRequest { get; set; }
        #endregion

        #region Public Interactivity
        public InteractionRequest<Notification> PaintReportConfirmationWindowRequest
        {
            get { return paintReportConfirmationWindowRequest ?? (paintReportConfirmationWindowRequest = new InteractionRequest<Notification>()); }
        }
        public InteractionRequest<Notification> ShowQuestionWindowRequest
        {
            get { return showQuestionWindowRequest ?? (showQuestionWindowRequest = new InteractionRequest<Notification>()); }
        }
        public InteractionRequest<Notification> ShowMessageWindowRequest
        {
            get { return showMessageWindowRequest ?? (showMessageWindowRequest = new InteractionRequest<Notification>()); }
        }
        public InteractionRequest<Notification> ShowErrorWindowRequest
        {
            get { return showErrorWindowRequest ?? (showErrorWindowRequest = new InteractionRequest<Notification>()); }
        }
        #endregion


        #region Public Properties
        public BoxLoad SelectedLoaded
        {
            get { return selectedLoaded; }
            set
            {
                if (value == selectedLoaded) return;
                selectedLoaded = value;
                RaisePropertyChanged(() => SelectedLoaded);
            }
        }
        public StockTPS SelectedTPS
        {
            get { return selectedTPS; }
            set
            {
                if (value == selectedTPS) return;
                selectedTPS = value;
                RaisePropertyChanged(() => SelectedTPS);
            }
        }
        public int Cajon
        {
            get { return cajon; }
            set
            {
                if (cajon == value) return;
                cajon = value;
                RaisePropertyChanged(() => Cajon);
            }
        }
        public ObservableCollection<BoxReport> ReportesDeCaja
        {
            get { return reportesDeCaja; }
            set
            {
                if (reportesDeCaja == value) return;
                reportesDeCaja = value;
                RaisePropertyChanged(() => ReportesDeCaja);
            }
        }
        public ObservableCollection<BoxLoad> CajasCargadas
        {
            get { return cajasCargadas; }
            set
            {
                if (cajasCargadas == value) return;
                cajasCargadas = value;
                RaisePropertyChanged(() => CajasCargadas);
            }
        }
        public ObservableCollection<StockTPS> StockParaTPS
        {
            get { return stockParaTPS; }
            set
            {
                if (stockParaTPS == value) return;
                stockParaTPS = value;
                RaisePropertyChanged(() => StockParaTPS);
            }
        }
        #endregion

        #region Private Commands
        private ICommand searchCommand;
        private ICommand loadCommand;
        private ICommand reportCommand;
        #endregion

        #region public commands
        public ICommand SearchCommand
        {
            get
            {
                if (searchCommand == null)
                {
                    searchCommand = new RelayCommand(param => this.searchCommandExecute(), null);
                }
                return searchCommand;
            }
        }
        public ICommand LoadCommand
        {
            get
            {
                if (loadCommand == null)
                {
                    loadCommand = new RelayCommand(param => this.loadCommandExecute(), param => canLoadCommand()); // 
                }
                return loadCommand;
            }
        }
        public ICommand ReportCommand
        {
            get
            {
                if (reportCommand == null)
                {
                    reportCommand = new RelayCommand(param => this.ReportCommandExecute(), param => canReportCommand()); // param => canReportCommand()
                }
                return reportCommand;
            }
        }
        #endregion

        #region CommandsExecute
        private void searchCommandExecute()
        {
            Actions.GeneralMachine
                .AddFilter("@UdtBox", cajon);

            StockParaTPS = (ObservableCollection<StockTPS>)Actions.Search().OutPuts["StockParaTPSRef"];
            CajasCargadas = (ObservableCollection<BoxLoad>)Actions.OutPuts["CajasCargadasRef"];
            ReportesDeCaja = (ObservableCollection<BoxReport>)Actions.OutPuts["ReportesDeCajaRef"];

            SelectedTPS = StockParaTPS.FirstOrDefault();
            SelectedLoaded = CajasCargadas.FirstOrDefault();

        }
        private void loadCommandExecute()
        {
            PaintingReportSupport.btnLoad_Click(cajon,
                out ObservableCollection<StockTPS> StockParaTPSRef,
                out ObservableCollection<BoxLoad> CajasCargadasRef,
                out ObservableCollection<BoxReport> ReportesDeCajaRef,
                selectedTPS,
                showMessageWindowRequest);
            StockParaTPS = StockParaTPSRef;
            CajasCargadas = CajasCargadasRef;
            ReportesDeCaja = ReportesDeCajaRef;
        }
        private void ReportCommandExecute()
        {
            //PaintingReportBusiness.Instance.Report(SelectedLoaded);
            //searchCommandExecute();


            Actions.GeneralMachine
                .AddFilter("cajonSelected", cajon)
                .AddFilter("selectedLoaded", selectedLoaded);

            Actions.Report();

            PaintingReportSupport.btnReportPintado_Click(
                cajon,
                out ObservableCollection<StockTPS> StockParaTPSRef,
                out ObservableCollection<BoxLoad> CajasCargadasRef,
                out ObservableCollection<BoxReport> ReportesDeCajaRef,
                selectedLoaded,
                PaintReportConfirmationWindowRequest,
                ShowErrorWindowRequest,
                ShowMessageWindowRequest,
                ShowQuestionWindowRequest);
            StockParaTPS = StockParaTPSRef;
            CajasCargadas = CajasCargadasRef;
            ReportesDeCaja = ReportesDeCajaRef;
        }
        #endregion

        #region Canexecute
        private bool canLoadCommand()
        {
            if (SelectedTPS == null)
            {
                //MessageBox.Show("Debe de seleccionar un registro del grid de \"STOCK PARA PINTADO TPS\"");
                return false;
            }
            else { return true; }
        }

        private bool canReportCommand()
        {

            if (SelectedLoaded == null)
            {
                //MessageBox.Show("Debe de seleccionar un registro del grid de \"CAJAS CARGADAS A NIVEL 2\"");
                return false;
            }
            else { return true; }
        }
        #endregion
    }
}
