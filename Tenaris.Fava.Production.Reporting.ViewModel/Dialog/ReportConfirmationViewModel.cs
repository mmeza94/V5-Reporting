using Infrastructure.InteractionRequests;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Tenaris.Fava.Production.Reporting.Model.Business;
using Tenaris.Fava.Production.Reporting.Model.Data_Access;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.Model;
using Tenaris.Fava.Production.Reporting.ViewModel.Dialog;

namespace Tenaris.Fava.Production.Reporting.ViewModel
{

    public class ReportConfirmationViewModel : IPopupWindowActionAware, INotifyPropertyChanged
    {

        #region Constructor
        public ReportConfirmationViewModel()
        {

            TipoEnvio = new List<string>() { "Parcial", "Final", "Completo" };
            Destino = new List<string>() { "Chatarra", "Decisión de Ingeniería" };

            RejectionReportDetails = new ObservableCollection<RejectionReportDetail>();
            CounterTagN1 = Configurations.Instance.N1CounterTag;
            CounterTagN2 = Configurations.Instance.N2CounterTag;
            DestinoSelected = Destino.FirstOrDefault();
            UnlockVisibility = Visibility.Visible;
            LockVisibility = Visibility.Collapsed;

            ContadorVisibility = ConfigurationManager.AppSettings["isContadorVisible"] == "true" ? Visibility.Visible : Visibility.Collapsed;
            ExtremoEditable = Configurations.Instance.Machine.Contains("Forjadora") ? Visibility.Visible : Visibility.Collapsed;

            Trabajado = ConfigurationManager.AppSettings.Get("WorkedCheckBoxChecked") == "1";
            TrabajadoVisibility = ConfigurationManager.AppSettings.Get("VisibleWorkedCheckbox") == "1" ? Visibility.Visible : Visibility.Collapsed;

            ExtremosVisibility = ConfigurationManager.AppSettings.Get("VisibleExtremeRadioButton") == "1" ? Visibility.Visible : Visibility.Collapsed;

            lockCommandExecute();

        }
        #endregion

        #region Builder

        public ReportConfirmationViewModel SetGeneralPiece(GeneralPiece generalPiece)
        {
            CurrentGeneralPiece = generalPiece;
            Extremo2 = generalPiece.Extremo.Contains("2");
            Extremo1 = !Extremo2;
            this.PopulateLevel2Counters()
                .SetSendStatus()
                .PopulateRejectionCodeByMachineDescription()
                .GetPreviousCounters();
            RazonDescarteSelected = RazonDescarte.FirstOrDefault();
            return this;
        }

        public ReportConfirmationViewModel SetReportProductionDto(ReportProductionDto reportProductionDto)
        {
            CurrentReportProduction = reportProductionDto;
            return this;
        }

        public ReportConfirmationViewModel SetUser(string user)
        {
            this.User = user;
            return this;
        }

        public ReportConfirmationViewModel SetITLoadHelper(int firstTotalLoadedPieces)
        {
            this.ITLoadHelper = firstTotalLoadedPieces;
            return this;
        }

        #endregion

        #region Propiedades privadas
        private ObservableCollection<RejectionReportDetail> rejectionReportDetails;
        private string motivo;
        private bool trabajado;
        private bool extremo2;
        private bool extremo1;
        private ObservableCollection<RejectionCode> razonDescarte;
        private List<string> destino;
        private int cantidad;
        private List<string> tipoEnvio;
        private int cargadasTotal;
        private int cargadasActual;
        private int cargadasAnterior;
        private int reprocesosTotal;
        private int reprocesosActual;
        private int reprocesosAnterior;
        private int malasTotal;
        private int malasActual;
        private int malasAnterior;
        private int buenasTotal;
        private int buenasActual;
        private int buenasAnterior;
        private int atado;
        private int colada;
        private int orden;
        private Visibility unlockVisibility;
        private Visibility lockVisibility;
        private bool locked;
        private RejectionCode razonDescarteSelected;
        private string destinoSelected;
        private RejectionReportDetail rejectionReportDetailSelected;
        private int numDetalles;
        private GeneralPiece currentGeneralPiece;
        private ReportProductionDto currentReportProduction;
        private Visibility extremosVisibility;
        private Visibility trabajadoVisibility;
        private Visibility contadorVisibility;
        private Visibility extremoEditable;
        private bool isEditionEnabled;
        private ObservableCollection<RejectionReportDetail> dgRejections;
        private int iTLoadHelper;
        private string counterTagN1;
        private string counterTagN2;
        private int tbN1Counter;
        //private OplSubscription _oplSusbscription;
        private string tipoEnvioSelected;
        private string user;
        private string selectedSendType;
        private bool result;
        private string extremo;
        #endregion

        #region Private InteractionRequest

        private InteractionRequest<Notification> reportConfirmationWindowRequest { get; set; }

        private InteractionRequest<Notification> loginConfirmationWindowRequest { get; set; }

        private InteractionRequest<Notification> showQuestionWindowRequest { get; set; }
        private InteractionRequest<Notification> showMessageWindowRequest { get; set; }
        private InteractionRequest<Notification> showErrorWindowRequest { get; set; }
        private InteractionRequest<Notification> loginWindowRequest { get; set; }

        #endregion

        #region Public InteractionRequest
        public InteractionRequest<Notification> ReportConfirmationWindowRequest
        {
            get { return reportConfirmationWindowRequest ?? (reportConfirmationWindowRequest = new InteractionRequest<Notification>()); }
        }

        public InteractionRequest<Notification> LoginConfirmationWindowRequest
        {
            get { return loginConfirmationWindowRequest ?? (loginConfirmationWindowRequest = new InteractionRequest<Notification>()); }
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
        public InteractionRequest<Notification> LoginWindowRequest
        {
            get { return loginWindowRequest ?? (loginWindowRequest = new InteractionRequest<Notification>()); }

        }
        #endregion

        #region Public Properties
        public string Extremo
        {
            get { return extremo; }
            set
            {
                if (value == extremo) return;
                extremo = value;
                OnPropertyChanged("Extremo");
            }
        }


        public Visibility ContadorVisibility
        {
            get { return contadorVisibility; }
            set
            {
                if (value == contadorVisibility) return;
                contadorVisibility = value;
                OnPropertyChanged("ContadorVisibility");
            }
        }

        public Visibility ExtremoEditable
        {
            get { return extremoEditable; }
            set
            {
                if (value == extremoEditable) return;
                extremoEditable = value;
                OnPropertyChanged("ExtremoEditable");
            }
        }
        public bool Result
        {
            get { return result; }
            set
            {
                if (value == result) return;
                result = value;
                OnPropertyChanged("Result");
            }
        }

        public string SelectedSendType
        {
            get { return selectedSendType; }
            set
            {
                if (value == selectedSendType) return;
                selectedSendType = value;
                OnPropertyChanged("SelectedSendType");
            }
        }

        public string User
        {
            get { return user; }
            set
            {
                if (value == user) return;
                user = value;
                OnPropertyChanged("User");
            }
        }




        public string TipoEnvioSelected
        {
            get { return tipoEnvioSelected; }
            set
            {
                if (value == tipoEnvioSelected) return;
                tipoEnvioSelected = value;
                OnPropertyChanged("TipoEnvioSelected");
            }
        }

        //public OplSubscription _OplSusbcription
        //{
        //    get { return _oplSusbscription; }
        //    set
        //    {
        //        if (value == _oplSusbscription) return;
        //        _oplSusbscription = value;
        //        OnPropertyChanged("_OplSusbcription");
        //    }
        //}




        //Propiedades publicas
        public Visibility TrabajadoVisibility
        {
            get { return trabajadoVisibility; }
            set
            {
                if (value == trabajadoVisibility) return;
                trabajadoVisibility = value;
                OnPropertyChanged("TrabajadoVisibility");
            }
        }
        public Visibility ExtremosVisibility
        {
            get { return extremosVisibility; }
            set
            {
                if (value == extremosVisibility) return;
                extremosVisibility = value;
                OnPropertyChanged("ExtremosVisibility");
            }
        }
        public ReportProductionDto CurrentReportProduction
        {
            get { return currentReportProduction; }
            set
            {
                if (value == currentReportProduction) return;
                currentReportProduction = value;
                OnPropertyChanged("CurrentReportProduction");
            }
        }
        public GeneralPiece CurrentGeneralPiece
        {
            get { return currentGeneralPiece; }
            set
            {
                if (value == currentGeneralPiece) return;
                currentGeneralPiece = value;
                OnPropertyChanged("CurrentGeneralPiece");
            }
        }
        public int NumDetalles
        {
            get { return numDetalles; }
            set
            {
                if (value == numDetalles) return;
                numDetalles = value;
                OnPropertyChanged("NumDetalles");
            }
        }


        public RejectionReportDetail RejectionReportDetailSelected
        {
            get { return rejectionReportDetailSelected; }
            set
            {
                if (value == rejectionReportDetailSelected) return;
                rejectionReportDetailSelected = value;
                OnPropertyChanged("RejectionReportDetailSelected");
            }
        }
        public RejectionCode RazonDescarteSelected
        {
            get { return razonDescarteSelected; }
            set
            {
                if (value == razonDescarteSelected) return;
                razonDescarteSelected = value;
                OnPropertyChanged("RazonDescarteSelected");
            }
        }
        public string DestinoSelected
        {
            get { return destinoSelected; }
            set
            {
                if (value == destinoSelected) return;
                destinoSelected = value;
                OnPropertyChanged("DestinoSelected");
            }
        }
        public ObservableCollection<RejectionReportDetail> RejectionReportDetails
        {
            get { return rejectionReportDetails; }
            set
            {
                if (value == rejectionReportDetails) return;
                rejectionReportDetails = value;
                OnPropertyChanged("RejectionReportDetails");
            }
        }
        public string Motivo
        {
            get { return motivo; }
            set
            {
                if (value == motivo) return;
                motivo = value;
                OnPropertyChanged("Motivo");
            }
        }
        public bool Trabajado
        {
            get { return trabajado; }
            set
            {
                if (value == trabajado) return;
                trabajado = value;
                OnPropertyChanged("Trabajado");
            }
        }
        public bool Extremo2
        {
            get { return extremo2; }
            set
            {
                if (value == extremo2) return;
                extremo2 = value;
                OnPropertyChanged("Extremo2");
            }
        }
        public bool Extremo1
        {
            get { return extremo1; }
            set
            {
                if (value == extremo1) return;
                extremo1 = value;
                OnPropertyChanged("Extremo1");
            }
        }
        public ObservableCollection<RejectionCode> RazonDescarte
        {
            get { return razonDescarte; }
            set
            {
                if (value == razonDescarte) return;
                razonDescarte = value;
                OnPropertyChanged("RazonDescarte");
            }
        }
        public List<string> Destino
        {
            get { return destino; }
            set
            {
                if (value == destino) return;
                destino = value;
                OnPropertyChanged("Destino");
            }
        }
        public int Cantidad
        {
            get { return cantidad; }
            set
            {
                if (value == cantidad) return;
                cantidad = value;
                OnPropertyChanged("Cantidad");
            }
        }
        public List<string> TipoEnvio
        {
            get { return tipoEnvio; }
            set
            {
                if (value == tipoEnvio) return;
                tipoEnvio = value;
                OnPropertyChanged("TipoEnvio");
            }
        }
        public int CargadasTotal
        {
            get { return cargadasTotal; }
            set
            {
                if (value == cargadasTotal) return;
                cargadasTotal = value;
                OnPropertyChanged("CargadasTotal");
            }
        }
        public int CargadasActual
        {
            get { return cargadasActual; }
            set
            {
                if (value == cargadasActual) return;
                cargadasActual = value;
                CargadasTotal = CargadasAnterior + CargadasActual;
                OnPropertyChanged("CargadasActual");
            }
        }
        public int CargadasAnterior
        {
            get { return cargadasAnterior; }
            set
            {
                if (value == cargadasAnterior) return;
                cargadasAnterior = value;
                OnPropertyChanged("CargadasAnterior");
            }
        }
        public int ReprocesosTotal
        {
            get { return reprocesosTotal; }
            set
            {
                if (value == reprocesosTotal) return;
                reprocesosTotal = value;
                OnPropertyChanged("ReprocesosTotal");
            }
        }
        public int ReprocesosActual
        {
            get { return reprocesosActual; }
            set
            {
                if (value == reprocesosActual) return;
                reprocesosActual = value;
                OnPropertyChanged("ReprocesosActual");
                ReprocesosTotal = ReprocesosActual + ReprocesosAnterior;
            }
        }
        public int ReprocesosAnterior
        {
            get { return reprocesosAnterior; }
            set
            {
                if (value == reprocesosAnterior) return;
                reprocesosAnterior = value;
                OnPropertyChanged("ReprocesosAnterior");
            }
        }
        public int MalasTotal
        {
            get { return malasTotal; }
            set
            {
                if (malasTotal == value) return;
                malasTotal = value;
                OnPropertyChanged("MalasTotal");
            }
        }
        public int MalasActual
        {
            get { return malasActual; }
            set
            {
                if (malasActual == value) return;
                malasActual = value;
                OnPropertyChanged("MalasActual");
                CargadasActual = BuenasActual + MalasActual;
                MalasTotal = MalasActual + MalasAnterior;
            }
        }
        public int MalasAnterior
        {
            get { return malasAnterior; }
            set
            {
                if (malasAnterior == value) return;
                malasAnterior = value;
                OnPropertyChanged("MalasAnterior");
            }
        }
        public int BuenasTotal
        {
            get { return buenasTotal; }
            set
            {
                if (buenasTotal == value) return;
                buenasTotal = value;
                OnPropertyChanged("BuenasTotal");
            }
        }
        public int BuenasActual
        {
            get { return buenasActual; }
            set
            {
                if (buenasActual == value) return;
                buenasActual = value;
                OnPropertyChanged("BuenasActual");
                CargadasActual = BuenasActual + MalasActual;
                BuenasTotal = BuenasActual + BuenasAnterior;
            }
        }
        public int BuenasAnterior
        {
            get { return buenasAnterior; }
            set
            {
                if (buenasAnterior == value) return;
                buenasAnterior = value;
                OnPropertyChanged("BuenasAnterior");
            }
        }
        public int Atado
        {
            get { return atado; }
            set
            {
                if (atado == value) return;
                atado = value;
                OnPropertyChanged("Atado");
            }
        }
        public int Colada
        {
            get { return colada; }
            set
            {
                if (colada == value) return;
                colada = value;
                OnPropertyChanged("Colada");
            }
        }
        public int Orden
        {
            get { return orden; }
            set
            {
                if (orden == value) return;
                orden = value;
                OnPropertyChanged("Orden");
            }
        }
        public Visibility UnlockVisibility
        {
            get { return unlockVisibility; }
            set
            {
                if (unlockVisibility == value) return;
                unlockVisibility = value;
                OnPropertyChanged("UnlockVisibility");
            }
        }
        public Visibility LockVisibility
        {
            get { return lockVisibility; }
            set
            {
                if (lockVisibility == value) return;
                lockVisibility = value;
                OnPropertyChanged("LockVisibility");
            }
        }
        public bool Locked
        {
            get { return locked; }
            set
            {
                if (value == locked) return;
                locked = value;
                OnPropertyChanged("Locked");
            }
        }
        public bool IsEditionEnabled
        {
            get { return isEditionEnabled; }
            set
            {
                if (value == isEditionEnabled) return;
                isEditionEnabled = value;
                OnPropertyChanged("IsEditionEnabled");
            }
        }
        public ObservableCollection<RejectionReportDetail> DgRejections
        {
            get { return dgRejections; }
            set
            {
                if (value == dgRejections) return;
                dgRejections = value;
                OnPropertyChanged("DgRejections");
            }
        }
        public int ITLoadHelper
        {
            get { return iTLoadHelper; }
            set
            {
                if (value == iTLoadHelper) return;
                iTLoadHelper = value;
                OnPropertyChanged("ITLoadHelper");
            }
        }
        public string CounterTagN1
        {
            get { return counterTagN1; }
            set
            {
                if (value == counterTagN1) return;
                counterTagN1 = value;
                OnPropertyChanged("CounterTagN1");
            }
        }
        public string CounterTagN2
        {
            get { return counterTagN2; }
            set
            {
                if (value == counterTagN2) return;
                counterTagN2 = value;
                OnPropertyChanged("CounterTagN2");
            }
        }
        public int TbN1Counter
        {
            get { return tbN1Counter; }
            set
            {
                if (value == tbN1Counter) return;
                tbN1Counter = value;
                OnPropertyChanged("TbN1Counter");
            }
        }
        public Window HostWindow { get; set; }
        public Notification HostNotification { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public string Title => "Confirmacion de reporte de produccion";
        #endregion

        #region Private Comandos
        private ICommand unlockCommand;
        private ICommand lockCommand;
        private ICommand addRejectionCommand;
        private ICommand removeRejectionCommand;
        private ICommand acceptCommand;
        private ICommand cancelCommand;
        private ICommand extremoChangedCommand;
        #endregion

        #region Public Commands
        public ICommand CancelCommand2
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new RelayCommand(param => this.cancelCommandExecute(), param => this.cancelCommandCanExecute());
                }
                return cancelCommand;
            }

        }
        public ICommand ExtremoChangedCommand
        {
            get
            {
                if (extremoChangedCommand == null)
                {
                    extremoChangedCommand = new RelayCommand(param => this.extremoChangedCommandExecute(), null);
                }
                return extremoChangedCommand;
            }
        }
        public ICommand AcceptCommand2
        {
            get
            {
                if (acceptCommand == null)
                {
                    acceptCommand = new RelayCommand(param => this.acceptCommandExecute(), param => this.acceptCommandCanExecute());
                }
                return acceptCommand;
            }
        }
        public ICommand RemoveRejectionCommand
        {
            get
            {
                if (removeRejectionCommand == null)
                {
                    removeRejectionCommand = new RelayCommand(param => this.removeRejectionCommandExecute(), param => this.removeRejectionCommandCanExecute());
                }
                return removeRejectionCommand;
            }
        }
        public ICommand UnlockCommand
        {
            get
            {
                if (unlockCommand == null)
                {
                    unlockCommand = new RelayCommand(param => this.unlockCommandExecute(), null);
                }
                return unlockCommand;
            }
        }
        public ICommand LockCommand
        {
            get
            {
                if (lockCommand == null)
                {
                    lockCommand = new RelayCommand(param => this.lockCommandExecute(), null);
                }
                return lockCommand;
            }
        }
        public ICommand AddRejectionCommand
        {
            get
            {
                if (addRejectionCommand == null)
                {
                    addRejectionCommand = new RelayCommand(param => this.addRejectionCommandExecute(), param => this.AddRejectionCommandCanExecute());
                }
                return addRejectionCommand;
            }
        }
        #endregion

        #region Commands Execute
        private void extremoChangedCommandExecute()
        {
            GetPreviousCounters();
        }

        private bool cancelCommandCanExecute()
        {
            return true;
        }

        private void cancelCommandExecute()
        {
            Result = false;
            CloseWindow();
        }


        private void acceptCommandExecute()
        {
            Extremo = Extremo1 ? "Extremo 1" : "Extremo 2";
            CurrentGeneralPiece.Extremo = Extremo;

            Result = true;
            GC.Collect();
            CloseWindow();
        }

        private bool acceptCommandCanExecute()
        {
            return true;
        }

        //Comandos publicos
        private bool removeRejectionCommandCanExecute()
        {
            if (RejectionReportDetails.Count != 0)
                return true;
            return false;
        }

        private bool AddRejectionCommandCanExecute()
        {
            if (Cantidad > 0 && DestinoSelected != null && RazonDescarteSelected != null)
                return true;
            return false;
        }

        //Ejecución de comandos
        private void removeRejectionCommandExecute()
        {
            if (RejectionReportDetails != null || RejectionReportDetails.Count > 0)
                RejectionReportDetails.Clear();
        }

        private void unlockCommandExecute()
        {
            LoginViewModel login = new LoginViewModel();

            LoginWindowRequest.Raise(new Notification { Content = login });

            if (!login.Result)
                return;

            if (!ProductionReportingBusiness.LoginUser(login.Username, login.Password))
                return;

            Locked = false;
            IsEditionEnabled = true;
            LockVisibility = Visibility.Visible;
            UnlockVisibility = Visibility.Collapsed;

            login.Password = "";
            login = null;
        }
        private void lockCommandExecute()
        {

            UnlockVisibility = Visibility.Visible;
            LockVisibility = Visibility.Collapsed;
            Locked = true;
            IsEditionEnabled = false;
        }

        private void addRejectionCommandExecute()
        {
            var rejection = btnAddRejectionDetail_Click(Cantidad, RazonDescarteSelected, DestinoSelected, Motivo,
                            Trabajado, Extremo1);

            if (RejectionReportDetails.FirstOrDefault(x => x.RejectionCode.Equals(rejection.RejectionCode)) != null)
            {
                RejectionReportDetails.FirstOrDefault(x => x.RejectionCode.Equals(rejection.RejectionCode)).ScrapCount += rejection.ScrapCount;
                var dummie = new RejectionReportDetail();
                RejectionReportDetails.Add(dummie);
                RejectionReportDetails.Remove(dummie);
                var z = RejectionReportDetails;
                OnPropertyChanged("RejectionReportDetails");
                RejectionReportDetails = new ObservableCollection<RejectionReportDetail>();
                RejectionReportDetails = z;
            }
            else
            {
                RejectionReportDetails.Add(rejection);
                OnPropertyChanged("RejectionReportDetails");
            }
            Cantidad = 0;
            DestinoSelected = Destino.FirstOrDefault();
            RazonDescarteSelected = RazonDescarte.FirstOrDefault();
            Motivo = "";



        }

        public bool AcceptCanExecute()
        {
            if (BuenasActual + MalasActual != CargadasActual && ReprocesosActual <= CargadasActual)
                return false;
            else
            {
                if (RejectionReportDetails.Count > 0 || MalasActual > 0)
                {
                    if (RejectionReportDetails.Sum(x => x.ScrapCount) == MalasActual)
                    {
                        return true;
                    }
                    return false;
                }
                return true;
            }
        }

        public bool CancelCanExecute()
        {
            return true;
        }

        #endregion

        #region Public Methods
        public void CloseWindows(object param)
        {
            if (this.HostWindow != null)
            {
                this.HostWindow.Close();
            }
        }

        #endregion

        #region Private Methods
        private void CloseWindow()
        {
            CloseWindows(this);
        }
        private ReportConfirmationViewModel SetSendStatus()
        {
            switch (CurrentGeneralPiece.SendStatus)
            {
                case Model.Enums.Enumerations.ProductionReportSendStatus.Parcial: SelectedSendType = "Parcial"; break;
                case Model.Enums.Enumerations.ProductionReportSendStatus.Final: SelectedSendType = "Final"; break;
                default: SelectedSendType = "Completo"; break;
            }
            return this;
        }

        private ReportConfirmationViewModel GetPreviousCounters()
        {


            ObservableCollection<int> items = ProductionReportingBusiness.GetPreviousCountersByMachineTest(
                new Dictionary<string, object>
                {
                        { "@GroupItemNumber", CurrentGeneralPiece.GroupItemNumber },
                        { "@MachineSequence", Configurations.Instance.Secuencia },
                        { "@Operation", GetMachineDescription() }
                });

            BuenasAnterior = items[0];
            MalasAnterior = items[1];
            ReprocesosAnterior = items[2];

            CargadasAnterior = BuenasAnterior + MalasAnterior;
            BuenasTotal = BuenasAnterior + BuenasActual;
            MalasTotal = MalasAnterior + MalasActual;
            CargadasTotal = CargadasAnterior + CargadasActual;
            ReprocesosTotal = ReprocesosAnterior + ReprocesosActual;
            return this;
        }


        private string GetMachineDescription()
        {

            if (CurrentGeneralPiece.Description.Contains("Forja"))
                return "Forjado";

            else if (CurrentGeneralPiece.Description == "Roscadora")
                return "Mecanizado";

            return Configurations.Instance.Operacion;
        }


        private ReportConfirmationViewModel PopulateRejectionCodeByMachineDescription()
        {
            try
            {
                RazonDescarte = new ObservableCollection<RejectionCode>(DataAccessSQL.
                    Instance.GetRejectionCodeByMachineDescriptionTestV5(new Dictionary<string, object>
                {{
                    "@MachineDescription",CurrentGeneralPiece.Description
                } }));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return this;
        }

        private ReportConfirmationViewModel PopulateLevel2Counters()
        {
            CargadasActual = CurrentGeneralPiece.LoadedCount;
            BuenasActual = CurrentGeneralPiece.GoodCount;
            MalasActual = CurrentGeneralPiece.ScrapCount;
            ReprocesosActual = CurrentGeneralPiece.ReworkedCount;
            Atado = CurrentGeneralPiece.GroupItemNumber;
            Colada = CurrentGeneralPiece.HeatNumber;
            Orden = CurrentGeneralPiece.OrderNumber;
            return this;
        }
        private void VerifyPropertyName(string propertyName)
        {
            var type = GetType();
            if (type.GetProperty(propertyName) == null)
            {
                throw new ArgumentException("Property not found", propertyName);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private RejectionReportDetail btnAddRejectionDetail_Click(int tbScrapCountForRejection, RejectionCode SelectedRejectionCode, string DestinationSelectedItem,
            string motivo, bool worked, bool extremo1)
        {
            if (RejectionReportDetails == null)
                RejectionReportDetails = new ObservableCollection<RejectionReportDetail>();


            short cantidad = (short)tbScrapCountForRejection;

            var rejectionCode = SelectedRejectionCode;
            var rejectionReportDetail = new RejectionReportDetail();
            if (Configurations.Instance.Secuencia == "8")
            {
                var TEMrejectionReportDetail = new RejectionReportDetail
                {
                    RejectionCode = rejectionCode,
                    ScrapCount = cantidad,
                    Active = Enumerations.AxlrBit.Si,
                    InsDateTime = DateTime.Now,
                    Destino = DestinationSelectedItem,
                    Observation = motivo,
                    Trabajado = (worked) ? Enumerations.AxlrBit.Si : Enumerations.AxlrBit.No,
                    Extremo = extremo1 ? "Extremo 1" : "Extremo 2"
                };
                rejectionReportDetail = TEMrejectionReportDetail;
            }
            else
            {
                var TEMrejectionReportDetail = new RejectionReportDetail
                {
                    RejectionCode = rejectionCode,
                    ScrapCount = cantidad,
                    Active = Enumerations.AxlrBit.Si,
                    InsDateTime = DateTime.Now,
                    Destino = DestinationSelectedItem,
                    Observation = motivo,
                    Trabajado = worked ? Enumerations.AxlrBit.Si : Enumerations.AxlrBit.No
                };
                rejectionReportDetail = TEMrejectionReportDetail;
            }


            return rejectionReportDetail;

        }

        #endregion

    }
}
