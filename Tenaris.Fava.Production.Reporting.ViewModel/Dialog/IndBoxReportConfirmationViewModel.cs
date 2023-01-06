using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Library.UI.Framework.ViewModel;
using System.Collections.ObjectModel;
using Tenaris.Fava.Production.Reporting.ViewModel.Support;
using Tenaris.Fava.Production.Reporting.Model.Business;
using System.Windows.Input;
using Tenaris.Fava.Production.Reporting.Model.Adapter;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows;
using System.Collections;
using Tenaris.Fava.Production.Reporting.Model.Support;
using Infrastructure.InteractionRequests;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.Configuration;

namespace Tenaris.Fava.Production.Reporting.ViewModel.Dialog
{
    public class IndBoxReportConfirmationViewModel : IPopupWindowActionAware, INotifyPropertyChanged

    {
        #region propiedades privadas
        private int orden;
        private int colada;
        private int atado;
        private string opHija;
        private string cople;
        private string cabezal;
        private string centralizado;
        private int buenas;
        private int malas;
        private int reprocesos;
        private int disponibles;
        private int total;
        private ObservableCollection<ProductionBox> dgBoxes;
        private int scrapCountForRejection;
        private List<string> destination;
        private string destinationSelect;
        private string selectedBundleDestiny;
        private ObservableCollection<RejectionCode> rejectionCode;
        private RejectionCode selectedRejectionCode;
        private bool extremo1;
        private bool extremo2;
        private bool worked;
        private string changeReason;
        private string motivo;
        private int totalActualAtado;
        private int cantidad;
        private RejectionReportDetail selectedBundleRejection;
        private ObservableCollection<RejectionReportDetail> dgrejectionReportDetails;
        private string lblNoDetails;
        private string user;
        private ProductionBox selectedBox;
        private Visibility unlockVisibility;
        private Visibility lockVisibility;
        private Visibility extremeDiscardVisibility;
        private bool isEnableContador;
        private bool result;
        #endregion

        //Propiedades para recibir la informacion de la ventanDg

        #region "Propiedades publicas"

        public ReportProductionDto currentProductionReport;
        public GeneralPiece currentGeneralPiece;
        public string User
        {
            get { return user; }
            set
            {
                if (value == user) return;
                user = value;
                RaisePropertyChanged("User");
            }
        }

        public ProductionBox SelectedBox
        {
            get { return selectedBox; }
            set
            {
                if (value == selectedBox) return;
                selectedBox = value;
                RaisePropertyChanged("SelectedBox");
            }
        }

        public string LblNoDetails
        {
            get { return "No de detalles: " + lblNoDetails; }
            set
            {
                if (value == lblNoDetails) return;
                lblNoDetails = value;
                RaisePropertyChanged("LblNoDetails");
            }
        }
        public string SelectedBundleDestiny
        {
            get { return selectedBundleDestiny; }
            set
            {
                if (value == selectedBundleDestiny) return;
                selectedBundleDestiny = value;
                RaisePropertyChanged("SelectedBundleDestiny");
            }
        }
        public RejectionCode SelectedRejectionCode
        {
            get { return selectedRejectionCode; }
            set
            {
                if (value == selectedRejectionCode) return;
                selectedRejectionCode = value;
                RaisePropertyChanged("SelectedRejectionCode");
            }
        }
        public ObservableCollection<RejectionCode> RejectionCode
        {
            get { return rejectionCode; }
            set
            {
                if (value == rejectionCode) return;
                rejectionCode = value;
                RaisePropertyChanged("RejectionCode");
            }
        }
        public RejectionReportDetail SelectedBundleRejection
        {
            get { return selectedBundleRejection; }
            set
            {
                if (value == selectedBundleRejection) return;
                selectedBundleRejection = value;
                RaisePropertyChanged("SelectedBundleRejection");
            }
        }

        public int Cantidad
        {
            get { return cantidad; }
            set
            {
                if (value == cantidad) return;
                cantidad = value;
                RaisePropertyChanged("Cantidad");
            }
        }

        public int Orden
        {
            get { return orden; }
            set
            {
                if (orden == value) return;
                orden = value;
                RaisePropertyChanged("Orden");
            }
        }
        public int Colada
        {
            get { return colada; }
            set
            {
                if (value == colada) return;
                colada = value;
                RaisePropertyChanged("Colada");
            }
        }
        public int Atado
        {
            get { return atado; }
            set
            {
                if (value == atado) return;
                atado = value;
                RaisePropertyChanged("Atado");
            }
        }
        public string OpHija
        {
            get { return opHija; }
            set
            {
                if (value == opHija) return;
                opHija = value;
                RaisePropertyChanged("OpHija");
            }
        }
        public string Cople
        {
            get { return cople; }
            set
            {
                if (value == cople) return;
                cople = value;
                RaisePropertyChanged("Cople");
            }
        }
        public string Cabezal
        {
            get { return cabezal; }
            set
            {
                if (value == cabezal) return;
                cabezal = value;
                RaisePropertyChanged("Cabezal");
            }
        }
        public string Centralizado
        {
            get { return centralizado; }
            set
            {
                if (value == centralizado) return;
                centralizado = value;
                RaisePropertyChanged("Centralizado");
            }
        }
        public int Buenas
        {
            get { return buenas; }
            set
            {
                if (value == buenas) return;
                buenas = value;
                Total = Buenas + Malas;
                RaisePropertyChanged("Buenas");
            }
        }
        public int Malas
        {
            get { return malas; }
            set
            {
                if (value == malas) return;
                malas = value;
                Total = Malas + Buenas;
                RaisePropertyChanged("Malas");
            }
        }
        public int Reprocesos
        {
            get { return reprocesos; }
            set
            {
                if (value == reprocesos) return;
                reprocesos = value;
                RaisePropertyChanged("Reprocesos");
            }
        }
        public int Disponibles
        {
            get { return disponibles; }
            set
            {
                if (value == disponibles) return;
                disponibles = value;
                RaisePropertyChanged("Disponibles");
            }
        }
        public int Total
        {
            get { return total; }
            set
            {
                if (value == total) return;
                total = value;
                RaisePropertyChanged("Total");
            }
        }
        public ObservableCollection<ProductionBox> DgBoxes
        {
            get { return dgBoxes; }
            set
            {
                if (value == dgBoxes) return;
                dgBoxes = value;
                RaisePropertyChanged("DgBoxes");
            }
        }
        public int ScrapCountForRejection
        {
            get { return scrapCountForRejection; }
            set
            {
                if (value == scrapCountForRejection) return;
                scrapCountForRejection = value;
                RaisePropertyChanged("ScrapCountForRejection");
            }
        }
        public List<string> Destination
        {
            get { return destination; }
            set
            {
                if (value == destination) return;
                destination = value;
                RaisePropertyChanged("Destination");
            }
        }
        public string DestinationSelected
        {
            get { return destinationSelect; }
            set
            {
                if (value == destinationSelect) return;
                destinationSelect = value;
                RaisePropertyChanged("DestinationSelected");
            }
        }
        public bool Extremo1
        {
            get { return extremo1; }
            set
            {
                if (value == extremo1) return;
                extremo1 = value;
                RaisePropertyChanged("Extremo1");
            }
        }
        public bool Extremo2
        {
            get { return extremo2; }
            set
            {
                if (value == extremo2) return;
                extremo2 = value;
                RaisePropertyChanged("Extremo2");
            }
        }
        public bool Worked
        {
            get { return worked; }
            set
            {
                if (value == worked) return;
                worked = value;
                RaisePropertyChanged("Worked");
            }
        }
        public string ChangeReason
        {
            get { return changeReason; }
            set
            {
                if (value == changeReason) return;
                changeReason = value;
                RaisePropertyChanged("ChangeReason");
            }
        }
        public string Motivo
        {
            get { return motivo; }
            set
            {
                if (value == motivo) return;
                motivo = value;
                RaisePropertyChanged("Motivo");
            }
        }
        public ObservableCollection<RejectionReportDetail> DgRejectionReportDetails
        {
            get { return dgrejectionReportDetails; }
            set
            {
                if (value == dgrejectionReportDetails) return;
                dgrejectionReportDetails = value;
                LblNoDetails = dgrejectionReportDetails.Count.ToString();
                RaisePropertyChanged("DgRejectionReportDetails");
            }
        }
        public int TotalActualAtado
        {
            get { return totalActualAtado; }
            set
            {
                if (value == totalActualAtado) return;
                totalActualAtado = value;
                RaisePropertyChanged("TotalActualAtado");
            }
        }

        public Visibility UnlockVisibility
        {
            get { return unlockVisibility; }
            set
            {
                if (value == unlockVisibility) return;
                unlockVisibility = value;
                RaisePropertyChanged("UnlockVisibility");

            }
        }

        public Visibility LockVisibility
        {
            get { return lockVisibility; }
            set
            {
                if (value == lockVisibility) return;
                lockVisibility = value;
                RaisePropertyChanged("LockVisibility");
            }
        }

        public Visibility ExtremeDiscardVisibility
        {
            get { return extremeDiscardVisibility; }
            set
            {
                if (value == extremeDiscardVisibility) return;
                extremeDiscardVisibility = value;
                RaisePropertyChanged("ExtremeDiscardVisibility");
            }
        }

        public bool IsEnableContador
        {
            get { return isEnableContador; }
            set 
            {
                if (value == isEnableContador) return;
                isEnableContador = value;
                RaisePropertyChanged("IsEnableContador");
            }
        }

        public bool Result
        {
            get { return result; }
            set
            {
                if (value == result) return;
                result = value;
                RaisePropertyChanged("Result");
            }
        }

        #endregion

        private InteractionRequest<Notification> reportConfirmationWindowRequest { get; set; }
        private InteractionRequest<Notification> indBoxReportConfirmationWindowRequest { get; set; }
        private InteractionRequest<Notification> showQuestionWindowRequest { get; set; }
        private InteractionRequest<Notification> showMessageWindowRequest { get; set; }
        private InteractionRequest<Notification> showErrorWindowRequest { get; set; }

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

        #endregion


        #region "Constructor


        public IndBoxReportConfirmationViewModel()
        {
            Extremo1 = false;
            Destination = new List<string>() { "Chatarra", "Decisión de Ingeniería" };
            DestinationSelected = Destination.FirstOrDefault();

        }

        public IndBoxReportConfirmationViewModel(GeneralPiece generalPieceDto, ReportProductionDto productionReportDto, string user) : this()
        {
            
            this.currentGeneralPiece = generalPieceDto;
            this.currentProductionReport = productionReportDto;
            this.user = user;
            DgRejectionReportDetails = new ObservableCollection<RejectionReportDetail>();
            PopulateRejectionCodeByMachineDescription();

            UnlockVisibility = Visibility.Visible;
            LockVisibility = Visibility.Collapsed;


            //Llenando textboxes
            Orden = this.currentGeneralPiece.OrderNumber;
            Colada = this.currentGeneralPiece.HeatNumber;
            Atado = this.currentGeneralPiece.GroupItemNumber;

            OPChildrens opHijaespecificacion = null;


            List<ProductionBox> listBoxes = new List<ProductionBox>();
            //string machineId = ConfigurationManager.AppSettings["Opcion"];
            //string operationId = ConfigurationManager.AppSettings["Operacion"];

            ExtremeDiscardVisibility = ConfigurationManager.AppSettings["VisibleExtremeRadioButton"] == "1" ? Visibility.Visible : Visibility.Collapsed;


            ITServiceAdapter itAdapter = new ITServiceAdapter();

            IndBoxReportConfirmationSupport.rejectionReportDetails = new List<RejectionReportDetail>();

            try
            {
                string errorMessage;

                listBoxes = itAdapter.GetProductionBoxes(
                    this.currentGeneralPiece.OrderNumber, 
                    currentProductionReport.Opcion, 
                    currentProductionReport.Operacion, 
                    out errorMessage);
            }
            catch (Exception ex)
            {
                //agregar modal
                ShowError showError = new ShowError("Error", ex.Message);
                showErrorWindowRequest.Raise(new Notification() { Content = showError});
            }


            if (listBoxes == null)
            {
                listBoxes = new List<ProductionBox>();
            }

            listBoxes = listBoxes.OrderBy(b => b.MissingPieces).ThenBy(b => b.Id).ToList();
            DgBoxes = DgBoxes ?? new ObservableCollection<ProductionBox>();

            foreach (ProductionBox box in listBoxes)
            {
                DgBoxes.Add(box);
            }



            ////Consultar OP HIJA
            opHijaespecificacion = ProductionReportingBusiness.GetNextOpChildrenActive(this.currentGeneralPiece.OrderNumber);


            ////identifica en el grid su pocicion para posterione mente marcar esa pieza para marcar
            if (listBoxes.Count > 0)
            {
                string idBoxSelect = ProductionReportingBusiness.BoxSelect(this.currentGeneralPiece.OrderNumber);
                int idActiveBox = ProductionReportingBusiness.GetActiveBox();

                SelectedBox = DgBoxes.FirstOrDefault(x=>x.Id.Equals(idActiveBox.ToString()));


            }

            OpHija = opHijaespecificacion != null ? opHijaespecificacion.NumeroOrder.ToString() : string.Empty;
            Cabezal = opHijaespecificacion != null ? opHijaespecificacion.Cabezal : string.Empty;
            Centralizado = opHijaespecificacion != null ? opHijaespecificacion.Centralizado : string.Empty;
            Cople = opHijaespecificacion != null ? opHijaespecificacion.Cople : string.Empty;

            Buenas = currentGeneralPiece.GoodCount;
            Malas = 0;
            Reprocesos = currentGeneralPiece.ReworkedCount;
            Total = Buenas;
            ScrapCountForRejection = currentGeneralPiece.ScrapCount;

            int total1 = IndBoxReportConfirmationSupport.GetPreviousTotal("Extremo 1", currentGeneralPiece);
            int total2 = IndBoxReportConfirmationSupport.GetPreviousTotal("Extremo 2", currentGeneralPiece);
            int total = total1 - total2;

            if (total < 0)
            {
                total = 0;
            }

            currentGeneralPiece.LoadedCount = total;
            currentProductionReport.CantidadTotal = total;

            TotalActualAtado = total;
        }


        public IndBoxReportConfirmationViewModel GetOpSpecification()
        {
            OPChildrens opHijaespecificacion = null;
            opHijaespecificacion = ProductionReportingBusiness.GetNextOpChildrenActive(this.currentGeneralPiece.OrderNumber);

            OpHija = opHijaespecificacion != null ? opHijaespecificacion.NumeroOrder.ToString() : string.Empty;
            Cabezal = opHijaespecificacion != null ? opHijaespecificacion.Cabezal : string.Empty;
            Centralizado = opHijaespecificacion != null ? opHijaespecificacion.Centralizado : string.Empty;
            Cople = opHijaespecificacion != null ? opHijaespecificacion.Cople : string.Empty;


            return this;

        }

        private void PopulateRejectionCodeByMachineDescription()
        {
            try
            {

                IList rejectioncode = new RejectionCodeFacade().GetRejectionCodeByMachineDescription(currentGeneralPiece.Description);
                RejectionCode = new ObservableCollection<RejectionCode>();
                foreach (var item in rejectioncode)
                {
                    RejectionCode.Add((RejectionCode)item);
                }

                SelectedRejectionCode = RejectionCode.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        #region "Commands"

        private ICommand addCommand;
        private ICommand removeCommand;
        private ICommand unlockCommand;
        private ICommand lockCommand;
        private ICommand acceptCommand;
        private ICommand cancelCommand;
        #endregion

        #region Comandos publicos
        public ICommand AddCommand
        {
            get
            {
                if (addCommand == null)
                {
                    addCommand = new RelayCommand(param => this.addCommandExecute(), null);
                }
                return addCommand;
            }
        }

        public ICommand RemoveCommand
        {
            get
            {
                if (removeCommand == null)
                {
                    removeCommand = new RelayCommand(param => this.removeCommandExecute(), null);
                }
                return removeCommand;
            }
        }

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
        public ICommand AcceptCommand
        {
            get
            {
                if(acceptCommand == null)
                {
                    acceptCommand = new RelayCommand(parm => this.acceptCommandExecute(), null);
                }
                return acceptCommand;
            }

        }

        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new RelayCommand(parm => this.cancelCommandExecute(), null);
                }
                return cancelCommand;
            }

        }

        

        public string Title => throw new NotImplementedException();

        public Window HostWindow { get; set; }
        public Notification HostNotification { get; set; }

        #endregion

        #region Commands Execute
        private void removeCommandExecute()
        {
            if (DgRejectionReportDetails != null || DgRejectionReportDetails.Count > 0)
            {
                DgRejectionReportDetails.Clear();
                DgRejectionReportDetails = new ObservableCollection<RejectionReportDetail>();


            }
            IndBoxReportConfirmationSupport.rejectionReportDetails = new List<RejectionReportDetail>();

        }
        public void addCommandExecute()
        {
            bool refresh;
            DgRejectionReportDetails = IndBoxReportConfirmationSupport.btnAddRejectionDetail_Click(SelectedRejectionCode, Cantidad, DestinationSelected,
                 Motivo, Worked, Extremo1, DgRejectionReportDetails, out refresh);
            if (refresh)
            {
                Cantidad = 0;
                DestinationSelected = Destination.FirstOrDefault();
                ScrapCountForRejection = 0;
                Motivo = "";
                Worked = false;
            }
        }
        private void UnlockCommandExecute()
        {
            UnlockVisibility = Visibility.Collapsed;
            LockVisibility = Visibility.Visible;

            IsEnableContador = true;

        }
        private void LockCommandExecute()
        {
            UnlockVisibility = Visibility.Visible;
            LockVisibility = Visibility.Collapsed;

            IsEnableContador = false;
        }

        private void acceptCommandExecute()
        {
            var extremo = Extremo1 == true ? "Extremo 1" : "Extremo 2";
            Result = true;
            GC.Collect();
            CloseWindow();
        }

        private void cancelCommandExecute()
        {
            Result = false;
            GC.Collect();
            CloseWindow();
        }

        #endregion

        #region methods


        public bool AcceptCanExecute()
        {
            return true;
        }

        public bool CancelCanExecute()
        {
            return true;
        }
        private void VerifyPropertyName(string propertyName)
        {
            var type = GetType();
            if (type.GetProperty(propertyName) == null)
            {
                throw new ArgumentException("Property not found", propertyName);
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
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
    }
}
