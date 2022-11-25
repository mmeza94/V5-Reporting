using Infrastructure.InteractionRequests;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace Tenaris.Fava.Production.Reporting.ViewModel.Dialog
{
    public class ShowMessage : IPopupWindowActionAware, INotifyPropertyChanged
    {
        public ShowMessage(string title, string message)
        {
            Title = title;
            Message = message;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private string title;
        private string message;                

        public string Message
        {
            get { return message; }
            set
            {
                if (value == message) return;
                message = value;
                OnPropertyChanged("Message");
            }
        }
        public string Title
        {
            get { return title; }
            set
            {
                if (value == title) return;
                title = value;
                OnPropertyChanged("Title");
            }
        }

        private ICommand acceptCommand;        

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

        private void acceptCommandExecute()
        {            
            GC.Collect();
            CloseWindow();
        }

        private bool acceptCommandCanExecute()
        {
            return true;
        }

        public Window HostWindow { get; set; }
        public Notification HostNotification { get; set; }



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
    }
}
