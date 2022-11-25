using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tenaris.Fava.Production.Reporting.View.Dialog
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();

            resetpass();
        }
        
        public void resetpass()
        {
            Password.Clear();
            Password.Password = String.Empty;
        }

        //private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        //{
        //    if (this.DataContext != null)
        //    {
        //        ((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password;
        //    }
        //}
    }
}
