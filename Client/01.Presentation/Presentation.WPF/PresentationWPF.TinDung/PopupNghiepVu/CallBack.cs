using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace PresentationWPF.TinDung.PopupNghiepVu
{
    public class CalTienNopTK : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double total = 0;
            total = Math.Max(0, System.Convert.ToDouble(values[0]) + System.Convert.ToDouble(values[1]) - System.Convert.ToDouble(values[2]) - (System.Convert.ToDouble(values[3]) + System.Convert.ToDouble(values[4]) + System.Convert.ToDouble(values[5]) + System.Convert.ToDouble(values[6])));
            return total;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class CalTraTruoc : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double total = 0;
            total = Math.Max(0, System.Convert.ToDouble(values[0]) + System.Convert.ToDouble(values[1]) - System.Convert.ToDouble(values[2]) - (System.Convert.ToDouble(values[3]) + System.Convert.ToDouble(values[4]) + System.Convert.ToDouble(values[5]) + System.Convert.ToDouble(values[6])));
            return total;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
