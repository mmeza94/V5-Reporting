using Infrastructure.InteractionRequests;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.ViewModel.Support;
using Tenaris.Library.UI.Framework.ViewModel;

namespace Tenaris.Fava.Production.Reporting.ViewModel.Dialog
{
    public class PaintingReportConfirmationViewModel : IPopupWindowActionAware, INotifyPropertyChanged

    {

        public string Title => "Painting Report Confirmation";

        //VARIABLES PRIVADAS
        public PaintingReport currentProductionReportDto;
        //ReportProductionDto ReportProductionDtoToSend;
        public ObservableCollection<RejectionReportDetail> rejectionReportDetails;


        private int orden;
        private int colada;
        private int atados;
        private int disponiblesTPS;
        private int buenasAnterior;
        private int malasAnterior;
        private int reprocesosAnterior;
        private int cargadasAnterior;
        private int buenasActual;
        private int malasActual;
        private int reprocesosActual;
        private int cargadasActual;
        private int buenasTotal;
        private int malasTotal;
        private int reprocesosTotal;
        private int cargadasTotal;
        private List<string> tipoEnvio;
        private string tipoEnvioSelected;
        private string userReport;
        private string motivo;
        private Visibility lockVisibility;
        private Visibility unlockVisibility;
        private bool isEnabled;

        //PROPIEDADES PUBLICAS
        #region PropiedadesPublicas

        public Visibility UnlockVisibility
        {
            get { return unlockVisibility; }
            set
            {
                if (value == unlockVisibility) return;
                unlockVisibility = value;
                OnPropertyChanged("UnlockVisibility");

            }
        }

        public Visibility LockVisibility
        {
            get { return lockVisibility; }
            set
            {
                if (value == lockVisibility) return;
                lockVisibility = value;
                OnPropertyChanged("LockVisibility");
            }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (value == isEnabled) return;
                isEnabled = value;
                OnPropertyChanged("IsEnabled");

            }
        }
        public bool Result { get; set; }
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
        public int Atados
        {
            get { return atados; }
            set
            {
                if (atados == value) return;
                atados = value;
                OnPropertyChanged("Atados");
            }
        }
        public int DisponiblesTPS
        {
            get { return disponiblesTPS; }
            set
            {
                if (disponiblesTPS == value) return;
                disponiblesTPS = value;
                OnPropertyChanged("DisponiblesTPS");
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
        public int ReprocesosAnterior
        {
            get { return reprocesosAnterior; }
            set
            {
                if (reprocesosAnterior == value) return;
                reprocesosAnterior = value;
                OnPropertyChanged("ReprocesosAnterior");
            }
        }
        public int CargadasAnterior
        {
            get { return cargadasAnterior; }
            set
            {
                if (cargadasAnterior == value) return;
                cargadasAnterior = value;
                OnPropertyChanged("CargadasAnterior");
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
                BuenasTotal = buenasAnterior + BuenasActual;
                CargadasActual = BuenasActual + MalasActual;
                
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
                MalasTotal = MalasAnterior + MalasActual;
                CargadasActual = BuenasActual + MalasActual;
                
            }
        }
        public int ReprocesosActual
        {
            get { return reprocesosActual; }
            set
            {
                if (reprocesosActual == value) return;
                reprocesosActual = value;
                OnPropertyChanged("ReprocesosActual");
                ReprocesosTotal = ReprocesosAnterior + ReprocesosActual;
               
            }
        }
        public int CargadasActual
        {
            get { return cargadasActual; }
            set
            {
                if (cargadasActual == value) return;
                cargadasActual = value;
                OnPropertyChanged("CargadasActual");
                CargadasTotal = CargadasAnterior + CargadasActual;
                
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
        public int ReprocesosTotal
        {
            get { return reprocesosTotal; }
            set
            {
                if (reprocesosTotal == value) return;
                reprocesosTotal = value;
                OnPropertyChanged("ReprocesosTotal");
            }
        }
        public int CargadasTotal
        {
            get { return cargadasTotal; }
            set
            {
                if (cargadasTotal == value) return;
                cargadasTotal = value;
                OnPropertyChanged("CargadasTotal");
            }
        }
        public List<string> TipoEnvio
        {
            get { return tipoEnvio; }
            set
            {
                if (tipoEnvio == value) return;
                tipoEnvio = value;
                OnPropertyChanged("TipoEnvio");
            }
        }
        public string TipoEnvioSelected
        {
            get { return tipoEnvioSelected; }
            set
            {
                if (tipoEnvioSelected == value) return;
                tipoEnvioSelected = value;
                OnPropertyChanged("TipoEnvioSelected");
            }
        }
        public string UserReport
        {
            get { return userReport; }
            set
            {
                if (userReport == value) return;
                userReport = value;
                OnPropertyChanged("UserReport");
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
        #endregion





        //METODO CONSTRUCTOR
        public PaintingReportConfirmationViewModel()
        {
        }
        public PaintingReportConfirmationViewModel(PaintingReport selectedReportPainting, string user)
        {

            currentProductionReportDto = selectedReportPainting;
            rejectionReportDetails = new ObservableCollection<RejectionReportDetail>();
            PopulateLevel2Counters();
            UserReport = user;
            var x = UserReport;
            TipoEnvio = new List<string>() { "Parcial", "Final", "Completo" };
            //TipoEnvio = (List<string>)Enum.GetValues(typeof(Enumerations.ProductionReportSendStatus)).Cast<Enumerations.ProductionReportSendStatus>();

            UnlockVisibility = Visibility.Visible;
            LockVisibility = Visibility.Collapsed;

            TipoEnvioSelected = TipoEnvio.FirstOrDefault();
  

        }


        //COMMANDOS PRIVADOS
        private ICommand editCommand;
        private ICommand reportCommand;
        private ICommand cancelCommand;
        private ICommand acceptCommand;
        private ICommand unlockCommand;
        private ICommand lockCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        //COMANDOS PUBLICOS
        #region Command Public

        public ICommand UnlockCommand
        {
            get
            {
                if (unlockCommand == null)
                {
                    unlockCommand = new RelayCommand(param => this.UnlockCommandExecute(), null);
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
                    lockCommand = new RelayCommand(param => this.LockCommandExecute(), null);
                }
                return lockCommand;
            }
        }
        public ICommand EditCommand
        {
            get
            {
                if (editCommand == null)
                {
                    editCommand = new RelayCommand(param => this.EditCommandExecute(), null);
                }
                return editCommand;
            }
        }

        public ICommand ReportCommand
        {
            get
            {
                if (reportCommand == null)
                {
                    reportCommand = new RelayCommand(param => this.ReportCommandExecute(), null);
                }
                return reportCommand;
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new RelayCommand(param => this.CancelCommandExecute(), null);
                }
                return cancelCommand;
            }
        }
        public ICommand AcceptCommand
        {
            get
            {
                if(acceptCommand == null)
                {
                    acceptCommand = new RelayCommand(param => this.AcceptCommandExecute(), null);                    
                }
                return acceptCommand;
            }
        }

        public Window HostWindow { get; set; }
        public Notification HostNotification { get; set; }
        #endregion



        //COMANDOS EXECUTE
        #region CommandExecute()

        private void UnlockCommandExecute()
        {
            UnlockVisibility = Visibility.Collapsed;
            LockVisibility = Visibility.Visible;

            IsEnabled = true;


        }
        private void LockCommandExecute()
        {
            UnlockVisibility = Visibility.Visible;
            LockVisibility = Visibility.Collapsed;

            IsEnabled = false;

        }
        private void EditCommandExecute()
        {
            //ShowHideOperatorCounters();
        }

        private void ReportCommandExecute()
        {
            //PaintingReportConfirmationSupport.Report(DisponiblesTPS, CargadasAnterior, BuenasActual, MalasActual, ReprocesosActual,
            //                                         CargadasActual, currentProductionReportDto, rejectionReportDetails, UserReport);
        }

        private void CancelCommandExecute()
        {
            GC.Collect();
            Result = false;
            CloseWindow();
        }
        private void AcceptCommandExecute()
        {
            GC.Collect();
            Result = true;
            CloseWindow();
        }

        public void CloseWindows(object param)
        {
            if (this.HostWindow != null)
            {
                this.HostWindow.Close();
            }
        }
        private void CloseWindow()
        {
            CloseWindows(this);
        }

        #endregion

















        private void SumCounters()
        {
            //if (ValidationRules.CheckForValidIntegers(new List<TextBox>{tbGoodCountL2,
            //  tbScrapCountL2, tbReworkedCountL2, tbLoadedCountL2}, epProductionReport, true))
            //{

            CargadasActual = BuenasActual + MalasActual;
            BuenasTotal = BuenasActual + ((BuenasAnterior == 0) ? 0 : MalasActual);
            MalasTotal = MalasActual + ((MalasAnterior == 0) ? 0 : MalasActual);
            ReprocesosActual = ReprocesosActual + ((ReprocesosAnterior == 0) ? 0 : MalasActual);
            CargadasTotal = CargadasActual + ((CargadasAnterior == 0) ? 0 : MalasActual);

            //}
        }





        private void PopulateLevel2Counters()
        {
            CargadasActual = currentProductionReportDto.LoadQuantity;
            BuenasActual = currentProductionReportDto.GoodCount;
            MalasActual = currentProductionReportDto.ScrapCount;
            ReprocesosActual = 0;//this.currentProductionReportDto.ReworkedCount.ToString();
            Atados = Convert.ToInt32(currentProductionReportDto.BoxUdt);
            Colada = currentProductionReportDto.HeatNumber;
            Orden = currentProductionReportDto.ChildOrden;
            DisponiblesTPS = currentProductionReportDto.LoadQuantity;
            // //SetSendStatus();

        }











        //VALIDACIONES
        private bool canReportCommand()
        {

            return true;
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
        private void VerifyPropertyName(string propertyName)
        {
            var type = GetType();
            if (type.GetProperty(propertyName) == null)
            {
                throw new ArgumentException("Property not found", propertyName);
            }
        }

        //

        public bool OnAccept()
        {
            return true;
        }

        public void OnCancel()
        {

        }

        public bool AcceptCanExecute()
        {
            return true;
        }

        public bool CancelCanExecute()
        {
            return true;
        }
    }
}
