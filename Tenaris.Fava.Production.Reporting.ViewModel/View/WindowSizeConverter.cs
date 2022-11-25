using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Tenaris.Fava.Production.Reporting.ViewModel.View
{
    public class WindowSizeConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //double size = System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
            double size = System.Convert.ToDouble(value) - System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
            var x = size.ToString();
            return x;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;

    }
}
