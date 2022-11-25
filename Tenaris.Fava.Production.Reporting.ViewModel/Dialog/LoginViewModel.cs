using Infrastructure.InteractionRequests;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Tenaris.Fava.Production.Reporting.ViewModel.Support;
using Tenaris.Library.UI.Framework.ViewModel;

namespace Tenaris.Fava.Production.Reporting.ViewModel.Dialog
{

    public class LoginViewModel : IPopupWindowActionAware, INotifyPropertyChanged
    {

        static LoginViewModel classInstance = null;
        static object classLock = new object();


        public static LoginViewModel Instance
        {
            get
            {
                lock (classLock)
                {
                    if (classInstance == null)
                    {
                        classInstance = new LoginViewModel();
                    }
                }
                return classInstance;
            }
        }

        #region Constructor
        public LoginViewModel()
        {
            Username = string.Empty;
        }
        #endregion

        #region private properties
        private string _username;
        private string _password;
        private bool result;
        #endregion

        #region public properties
        public string Username
        {
            get { return _username; }
            set
            {
                if(_username == value) return;
                _username = value;
                OnPropertyChanged("Username");
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                if(_password == value) return;
                _password = value;
                OnPropertyChanged("Password");
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
        #endregion

        #region events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        
        #region private Commands
        private ICommand acceptCommand;
        private ICommand cancelCommand;
        #endregion

        #region public Commands

        public ICommand CancelCommand2
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new RelayCommand(param => this.cancelCommandExecute(param), param => this.cancelCommandCanExecute());
                }
                return cancelCommand;
            }

        }
        public ICommand AcceptCommand2
        {
            get
            {
                if (acceptCommand == null)
                {
                    acceptCommand = new RelayCommand(param => this.acceptCommandExecute(param), param => this.acceptCommandCanExecute());
                }
                return acceptCommand;
            }
        }

        #endregion

        #region CanExecute
        private bool cancelCommandCanExecute()
        {
            return true;
        }
        private bool acceptCommandCanExecute()
        {
            return true;
        }
        #endregion

        #region Commands Execution 
        
        private void cancelCommandExecute(object param)
        {
            Password = ((PasswordBox)param).Password;
            Result = false;
            ((PasswordBox)param).Password = String.Empty;
            GC.Collect();
            CloseWindow();
        }

        private void acceptCommandExecute(object param)
        {
            Password = ((PasswordBox)param).Password;
            Result = true;
            ((PasswordBox)param).Password = String.Empty;
            GC.Collect();
            CloseWindow();
        }

        #endregion


        #region Anonymous Methods
        public Window HostWindow { get; set; }
        public Notification HostNotification { get ; set ; }
        #endregion

        #region methods

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
