using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;	
using Tenaris.Fava.Production.Reporting.Model.DTO;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.Configuration;
using Tenaris.Fava.Production.Reporting.ViewModel.Dialog;
using Tenaris.Library.UI.Framework.ViewModel;
using Microsoft.Practices.Prism;
using Tenaris.Fava.Production.Reporting.ViewModel.Support;
using Tenaris.Fava.Production.Reporting.Model.Model;
using System.Text.RegularExpressions;
using Tenaris.Library.Log;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace Tenaris.Fava.Production.Reporting.ViewModel.View
{

    public class ProductionReportViewModel : ViewModelBase
    {

        #region Singleton
        static ProductionReportViewModel classInstance = null;
        static object classLock = new object();
        public static ProductionReportViewModel Instance
        {
            get
            {
                lock (classLock)
                {
                    if (classInstance == null)
                    {
                        classInstance = new ProductionReportViewModel();
                    }
                }
                return classInstance;
            }
        }
        #endregion

        #region Constructor
        public ProductionReportViewModel()
        {
            try
            {
                Configurations.Instance.GetConfigutation();
                UnlockControls();
                IsMaquinaInicial = Configurations.Instance.MaquinaInicialZona == "1";
                ForjadoraTextboxes = !Configurations.Instance.Machine.ToUpper().Equals("FORJADORA 0");
                ForjaColumnsVisibility = Configurations.Instance.Machine.ToUpper().Equals("FORJADORA 0")? Visibility.Visible: Visibility.Collapsed;
                CommonVisibility = ForjaColumnsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                visibilityDataGrid();
                Machine = Configurations.Instance.Machine;
                MachineVisibilidad = ConfigurationManager.AppSettings["isMachineVisible"].ToString() == "1" ? Visibility.Visible : Visibility.Collapsed;
                LabelMachine = string.IsNullOrEmpty(ConfigurationManager.AppSettings["LabelMachine"].ToString()) ? ConfigurationManager.AppSettings["Machine"].ToString() : ConfigurationManager.AppSettings["LabelMachine"].ToString();






            }
            catch (Exception ex)
            {
                Trace.Exception(ex);
            }
            
            
        }
        private void visibilityDataGrid()
        {
            if (Configurations.Instance.VersionApplication.Equals("V4"))
            {                
                IsVisibility = Visibility.Collapsed;
                IsVisibility2 = Visibility.Visible;
                IsVisibilityCantTot = Visibility.Visible;
            }
            else if (Configurations.Instance.Machine.Equals("Forjadora 0"))
            {
                IsVisibility = Visibility.Visible;
                IsVisibility2 = Visibility.Collapsed;
                IsVisibilityCantTot = Visibility.Collapsed;
            }
            else
            {                
                IsVisibility = Visibility.Collapsed;
                IsVisibility2 = Visibility.Collapsed;
                IsVisibilityCantTot = Visibility.Visible;
            }
        }

        #endregion

        #region private properties
        private int orden;
        private int colada;
        private int atado;
        private ObservableCollection<GeneralPiece> resultados;
        private ObservableCollection<ReportProductionHistory> historico;
        private string title;
        private GeneralPiece selected_bundle;
        private bool isLocked;
        private Visibility lock_Visibility;
        private Visibility unlock_Visibility;
        private bool forjadoraTextboxes;
        private bool isMaquinaInicial;
        private bool isCargarTPS;
        private string numResultados;
        private string numBuenas;
        private bool isReportButtonEnabled;
        private bool validationHasError;
        private string machine;
        private Visibility forjaColumnsVisibility;
        private Visibility commonVisibility;
        private Visibility machineVisibilidad;
        private Visibility isVisibility;
        private Visibility isVisibility2;
        private Visibility isVisibilityCantTot;
        private string extremo;
        private string labelMachine;

        private ObservableCollection<ReportProductionHistoryV1> historicoV1;
        #endregion

        #region private InteracionRequests<Notification>
        private InteractionRequest<Notification> reportConfirmationWindowRequest { get; set; }
        private InteractionRequest<Notification> indBoxReportConfirmationWindowRequest { get; set; }
        private InteractionRequest<Notification> showQuestionWindowRequest { get; set; }
        private InteractionRequest<Notification> showMessageWindowRequest { get; set; }
        private InteractionRequest<Notification> showErrorWindowRequest { get; set; }
        private InteractionRequest<Notification> showPaintingWindowRequest { get; set; }

        #endregion

        #region public properties



        public string LabelMachine
        {
            get { return labelMachine; }
            set
            {
                if (labelMachine == value) return;
                labelMachine = value;
                onPropertyChanged("LabelMachine");
            }
        }

        public string Extremo
        {
            get { return extremo; }
            set
            {
                if (value == extremo) return;
                extremo = value;
                onPropertyChanged("Extremo");
            }
        }

        public Visibility IsVisibility
        {
            get { return isVisibility; }
            set
            {
                if (value == isVisibility) return;
                isVisibility = value;
                onPropertyChanged("IsVisibility");
            }
        }

        public Visibility IsVisibility2
        {
            get { return isVisibility2; }
            set
            {
                if (value == isVisibility2) return;
                isVisibility2 = value;
                onPropertyChanged("IsVisibility2");
            }
        }

        public Visibility IsVisibilityCantTot
        {
            get { return isVisibilityCantTot; }
            set
            {
                if (value == isVisibilityCantTot) return;
                isVisibilityCantTot = value;
                onPropertyChanged("IsVisibilityCantTot");
            }
        }

        public string Machine
        {
            get { return machine; }
            set
            {
                if (value == machine) return;
                machine = value;
                onPropertyChanged("Machine");
            }
        }
        
        public Visibility MachineVisibilidad
        {
            get { return machineVisibilidad; }
            set
            {
                if (value == machineVisibilidad) return;
                machineVisibilidad = value;
                onPropertyChanged("MachineVisibilidad");
            }
        }

        public Visibility ForjaColumnsVisibility
        {
            get { return forjaColumnsVisibility; }
            set
            {
                if (value == forjaColumnsVisibility) return;
                forjaColumnsVisibility = value;
                onPropertyChanged("ForjaColumnsVisibility");
            }
        }
        public Visibility CommonVisibility
        {
            get { return commonVisibility; }
            set
            {
                if (value == commonVisibility) return;
                commonVisibility = value;
                onPropertyChanged("CommonVisibility");
            }
        }
        public bool ForjadoraTextboxes
        {
            get { return forjadoraTextboxes; }
            set
            {
                if (value == forjadoraTextboxes)
                    return;
                forjadoraTextboxes = value;
                onPropertyChanged("ForjadoraTextboxes");
            }
        }
        public bool ValidationHasError
        {
            get { return validationHasError; }
            set
            {
                if (value == validationHasError) return;
                validationHasError = value;
                onPropertyChanged("ValidationHasError");
            }
        }
        public GeneralPiece Selected_Bundle
        {
            get { return selected_bundle; }
            set
            {
                if (value == selected_bundle) return;
                selected_bundle = value;
                onPropertyChanged("Selected_Bundle");
            }
        }
        public int Orden
        {
            get { return orden; }
            set
            {
                if (orden == value) return;
                orden = value;
                onPropertyChanged("Orden");
            }
        }
        public int Colada
        {
            get { return colada; }
            set
            {
                if (colada == value) return;
                colada = value;
                onPropertyChanged("Colada");
            }
        }
        public int Atado
        {
            get { return atado; }

            set
            {
                if (atado == value) return;
                atado = value;
                onPropertyChanged("Atado");
            }
        }
        public ObservableCollection<GeneralPiece> Resultados
        {
            get { return resultados; }
            set
            {
                if (resultados == value) return;
                resultados = value;
                onPropertyChanged("Resultados");
            }
        }
        public ObservableCollection<ReportProductionHistory> Historico
        {
            get { return historico; }
            set
            {
                if (historico == value) return;
                historico = value;
                onPropertyChanged("Historico");
            }
        }
        public ObservableCollection<ReportProductionHistoryV1> HistoricoV1
        {
            get { return historicoV1; }
            set
            {
                if (historicoV1 == value) return;
                historicoV1 = value;
                onPropertyChanged("HistoricoV1");
            }
        }
        public string Title
        {
            get { return title; }
            set
            {
                if (title == value) return;
                title = value;
                onPropertyChanged("Title");
            }
        }
        public bool IsLocked
        {
            get { return isLocked; }
            set
            {
                if (isLocked == value) return;
                isLocked = value;
                onPropertyChanged("IsLocked");

            }
        }
        public Visibility Lock_Visibility
        {
            get { return lock_Visibility; }
            set
            {
                if (value == lock_Visibility) return;
                lock_Visibility = value;
                onPropertyChanged("Lock_Visibility");
            }
        }
        public Visibility Unlock_Visibility
        {
            get { return unlock_Visibility; }
            set
            {
                if (value == unlock_Visibility) return;
                unlock_Visibility = value;
                onPropertyChanged("Unlock_Visibility");
            }
        }
        public bool IsMaquinaInicial
        {
            get { return isMaquinaInicial; }
            set
            {
                if (value == isMaquinaInicial) return;
                isMaquinaInicial = value;
                onPropertyChanged("IsMaquinaInicial");
            }
        }
        public bool IsCargarTPS
        {
            get { return isCargarTPS; }
            set
            {
                if (value == isCargarTPS) return;
                isCargarTPS = value;
                onPropertyChanged("IsCargarTPS");
            }
        }
        public string NumResultados
        {
            get { return "Numero de resultados: " + numResultados; }
            set
            {
                if (value == numResultados) return;
                numResultados = value;
                onPropertyChanged("NumResultados");
            }
        }
        public string NumBuenas
        {
            get { return "Total Piezas Buenas: " + numBuenas; }
            set
            {
                if (value == numBuenas) return;
                numBuenas = value;
                onPropertyChanged("NumBuenas");
            }
        }
        public bool IsReportButtonEnabled
        {
            get { return isReportButtonEnabled; }
            set
            {
                if (value == isReportButtonEnabled) return;
                isReportButtonEnabled = value;
                onPropertyChanged("IsReportButtonEnabled");
            }
        }
        #endregion

        #region public InteractionRequests<Notification>
        public InteractionRequest<Notification> ReportConfirmationWindowRequest
        {
            get { return reportConfirmationWindowRequest ?? (reportConfirmationWindowRequest = new InteractionRequest<Notification>()); }         
        }
        public InteractionRequest<Notification> IndBoxReportConfirmationWindowRequest
        {
            get { return indBoxReportConfirmationWindowRequest ?? (indBoxReportConfirmationWindowRequest = new InteractionRequest<Notification>()); }
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
        public InteractionRequest<Notification> ShowPaintingWindowRequest
        {
            get { return showPaintingWindowRequest ?? (showPaintingWindowRequest = new InteractionRequest<Notification>()); }
        }

        #endregion

        #region Private Commands
        private ICommand searchCommand;
        private ICommand reportCommand;
        private ICommand toggleLockCommand;
        private ICommand selectionChangedCommand;
        #endregion

        #region Public Commands

        public ICommand SearchCommand
        {
            get
            {
                if (searchCommand == null)
                {
                    searchCommand = new RelayCommand(param =>this.searchCommandExecute(),param => this.ValidateParams(this));
                }
                return searchCommand;
            }
        }
        
        public ICommand ReportCommand
        {
            get
            {
                if (reportCommand == null)
                {
                    reportCommand = new RelayCommand(param => this.reportCommandExecute(), null);
                }
                return reportCommand;
            }
        }
        public ICommand ToggleLockCommand
        {
            get
            {
                if (toggleLockCommand == null)
                {
                    toggleLockCommand = new RelayCommand(param => this.toggleLockCommandExecute(), null);
                }
                return toggleLockCommand;
            }
        }
        public ICommand SelectionChangedCommand
        {
            get
            {
                if (selectionChangedCommand == null)
                {
                    selectionChangedCommand = new RelayCommand(param => this.selectionChangedCommandExecute(), null);
                }
                return selectionChangedCommand;
            }
        }

        #endregion

        #region Commands execute
        private void searchCommandExecute()
        {
            //ShowQuestion showQuestion = new ShowQuestion("titlw", "msg");
            //ShowQuestionWindowRequest.Raise(new Notification() {Content = showQuestion });
            //PaintingReportConfirmationViewModel viewModel = new PaintingReportConfirmationViewModel();
            //ShowPaintingWindowRequest.Raise(new Notification() {Content= viewModel });

            if (true)
            {
                try
                {
                    Trace.Debug("PopulateGeneralProduction Orden:{0},Colada:{1},Atado:{2}", Orden, Colada, Atado);
                    Resultados = ProductionReportSupport.PopulateGeneralProduction(Orden, Colada, Atado, Configurations.Instance.ConnectionString);
                    foreach (var item in Resultados)
                    {
                        if (item.SendStatus != Model.Enums.Enumerations.ProductionReportSendStatus.Final)
                            item.GoodCount -= item.ReworkedCount;
                    }
                    int sumaPiezas = Resultados.Sum(x => x.GoodCount);
                    NumResultados = Resultados.Count.ToString();
                    NumBuenas = sumaPiezas.ToString();
                    Selected_Bundle = Resultados.FirstOrDefault();


                    #region FilasHardcode
                    //PRUEBA DE POPULATE
                    //GeneralPiece prueba1 = new GeneralPiece()
                    //{
                    //    GroupItemNumber = 12345678,
                    //    BuenasReportadas = 0,
                    //    Cargados = 0,
                    //    Customer = "AAA",
                    //    Description = "Forjadora 0",
                    //    Extremo = "Extremo 1",
                    //    Sended = Model.Enums.Enumerations.AxlrBit.No,
                    //    SendStatus = Model.Enums.Enumerations.ProductionReportSendStatus.Parcial,
                    //    SendedString = "No",
                    //    LoadedCount = 10,
                    //    ReworkedCount = 20,
                    //    GoodCount = 30,
                    //    ScrapCount = 40,
                    //    LotNumberHTR = 50,
                    //    HeatNumber = 12345,
                    //    Location = "AQUI",
                    //    OrderNumber = 0001,
                    //    IdHistory = 11,
                    //    IdBatch = 23,
                    //    GroupItemType = "nose",
                    //    ReportSequence = 1



                    //};

                    //GeneralPiece prueba2 = new GeneralPiece()
                    //{
                    //    GroupItemNumber = 1222222,
                    //    BuenasReportadas = 0,
                    //    Cargados = 10,
                    //    Customer = "BBB",
                    //    Description = "Forjadora 0",
                    //    Extremo = "Extremo 1",
                    //    Sended = Model.Enums.Enumerations.AxlrBit.No,
                    //    SendStatus = Model.Enums.Enumerations.ProductionReportSendStatus.Parcial,
                    //    SendedString = "No",
                    //    LoadedCount = 10,
                    //    ReworkedCount = 20,
                    //    GoodCount = 30,
                    //    ScrapCount = 40,
                    //    LotNumberHTR = 50,
                    //    HeatNumber = 12345,
                    //    Location = "AQUI",
                    //    OrderNumber = 0001,
                    //    IdHistory = 11,
                    //    IdBatch = 23,
                    //    GroupItemType = "nose",
                    //    ReportSequence = 1

                    //};
                    //Resultados.Add(prueba1);
                    //Resultados.Add(prueba2);
                    //acabaPrueba
                    #endregion
                }
                catch (Exception ex)
                {
                    Trace.Exception(ex);
                    throw;
                }
            }

            else
            {
                //CODIGO DE PRUEBA
            }
        }
        private void reportCommandExecute()
        {


            if (Selected_Bundle != null)
            {

                GeneralPiece generalPiece = Selected_Bundle;


                

                //generalPiece.GoodCount += generalPiece.ReworkedCount;
                
                //Cambio de Extremo para Forja0/1
                //if(Configurations.Instance.Machine.Contains("Forjadora") && generalPiece.Extremo == "Extremo 1")
                //{
                //    generalPiece = ProductionReportSupport.ExtremoForja(ShowQuestionWindowRequest, generalPiece);
                //}
                //else if (Configurations.Instance.Machine.Contains("Forjadora") && generalPiece.Extremo == "Extremo 2")
                //{
                //    generalPiece = ProductionReportSupport.ExtremoForja(ShowQuestionWindowRequest, generalPiece);
                //}               

                if (ProductionReportSupport.Report(generalPiece, ReportConfirmationWindowRequest, IndBoxReportConfirmationWindowRequest, ShowErrorWindowRequest, ShowMessageWindowRequest, ShowQuestionWindowRequest, Historico ))
                {
                    ReportConfirmationSupport.rejectionReportDetails = new List<RejectionReportDetail>();
                    
                }
                Resultados = new ObservableCollection<GeneralPiece>();
                searchCommandExecute();
                selectionChangedCommandExecute();
            }
            else
            {
                ShowError showerror = new ShowError("Error", "Seleccione un registro para reportar");
                ShowErrorWindowRequest.Raise(new Notification() { Content=showerror});
               
            }


        }
        private void toggleLockCommandExecute()
        {
            if (IsLocked)
            {
                if (ProductionReportSupport.Login())
                {
                    UnlockControls();
                }
            }
            else
            {
                LockControls();
            }
        }
        private void selectionChangedCommandExecute()
        {
            try
            {
                if (Configurations.Instance.VersionApplication.Equals("V1"))
                {
                    if (Resultados.Count!=0 && Selected_Bundle ==null)
                    {
                        Selected_Bundle = Resultados.FirstOrDefault();
                    }
                    ObservableCollection<ReportProductionHistoryV1> prodHistory = ProductionReportSupport.dgReporteProduccion_SelectionChangedV1(Selected_Bundle);
                    Historico = BindingHistory(prodHistory);
                    //Esto reorganiza el history por la secuencia para V1
                    List<ReportProductionHistory> LISTAhist = new List<ReportProductionHistory>();
                    LISTAhist = Historico.OrderBy(x => x.MachineSequence).ToList();
                    ObservableCollection<ReportProductionHistory> tempHist = new ObservableCollection<ReportProductionHistory>(LISTAhist);
                    Historico = tempHist;





                    //COMENTARIO PARA FORJADORA 0
                    //if (Configurations.Instance.Machine == "Forjadora 0")
                    //{
                    //    bool flag = false;
                    //    GeneralPiece newSel = ProductionReportSupport.dgReporteProduccion_SelectionChangedForja0(Selected_Bundle);
                    //    Selected_Bundle = newSel;
                    //    onPropertyChanged("Resultados");
                    //    //var x = Resultados;
                    //    //Resultados = x;
                    //    //Resultados[2] = newSel;
                    //    //onPropertyChanged("Resultados");
                    //    //Resultados[Index] = newSel;
                    //    //Selected_Bundle = resultados[Index];

                    //}
                }
                else
                {
                    if (Resultados.Count != 0 && Selected_Bundle == null)
                    {
                        Selected_Bundle=Resultados.FirstOrDefault();
                    }
                    Historico = ProductionReportSupport.dgReporteProduccion_SelectionChanged(Selected_Bundle);
                    Historico.OrderBy(x => x.MachineSequence);
                    //Esto reorganiza el history por la secuencia
                    List<ReportProductionHistory> LISTAhist = new List<ReportProductionHistory>();
                    LISTAhist = Historico.OrderBy(x => x.MachineSequence).ToList();
                    ObservableCollection<ReportProductionHistory> tempHist = new ObservableCollection<ReportProductionHistory>(LISTAhist);
                    Historico = tempHist;
                }
            }
            catch (Exception ex)
            {

                Trace.Exception(ex);
            }
            
            
        }
        #endregion

        #region CommandsCanExecute
        private bool ValidateParams(object obj)
        {
            if (Colada>0 || Orden >0 ||Atado>0)
            {
                return true;
            }
            else 
            { 
               return false;
            }
        }
        #endregion

        #region Methods
        private ObservableCollection<ReportProductionHistory> BindingHistory(ObservableCollection<ReportProductionHistoryV1> prodHistory)
        {
            ObservableCollection<ReportProductionHistory> list = new ObservableCollection<ReportProductionHistory>();
            if (prodHistory != null)
            {
                foreach (ReportProductionHistoryV1 rph in prodHistory)
                {
                    ReportProductionHistory rph0 = new ReportProductionHistory();
                    rph0.IdHistory = rph.IdHistory;
                    rph0.Id = rph.Id;
                    rph0.IdOrder = rph.IdOrder;
                    rph0.HeatNumber = rph.HeatNumber;
                    rph0.GroupItemNumber = rph.GroupItemNumber;
                    rph0.SendStatus = rph.SendStatus;
                    rph0.TotalQuantity = rph.TotalQuantity;
                    rph0.GoodCount = rph.GoodCount;
                    rph0.ScrapCount = rph.ScrapCount;
                    rph0.ReworkedCount = rph.ReworkedCount;
                    rph0.IdMachine = rph.IdMachine;
                    rph0.LotNumberHtr = rph.LotNumberHtr;
                    rph0.InsDateTime = rph.InsDateTime;
                    rph0.InsertedBy = rph.InsertedBy;
                    rph0.UpdDateTime = rph.UpdDateTime;
                    rph0.UpdatedBy = rph.UpdatedBy;
                    rph0.MachineSequence = rph.MachineSequence;
                    rph0.MachineOption = rph.MachineOption;
                    rph0.MachineOperation = rph.MachineOperation;
                    rph0.Observation = rph.Observation;
                    rph0.GroupItemType = rph.SendStatus.ToString();
                    rph0.ChildOrder = null;
                    rph0.ChildGroupItemNumber = null;
                    rph0.ChildGroupItemType = String.Empty;
                    list.Add(rph0);
                }
            }

            return list;
        }
        private void LockControls()
        {
            Lock_Visibility = Visibility.Collapsed;
            Unlock_Visibility = Visibility.Visible;
            IsLocked = true;
            IsReportButtonEnabled = false;
            ProductionReportSupport.whoIsLogged = "";
        }
        private void UnlockControls()
        {
            Lock_Visibility = Visibility.Visible;
            Unlock_Visibility = Visibility.Collapsed;
            IsLocked = false;
            IsReportButtonEnabled = true;
        }
        #endregion


    }
}
