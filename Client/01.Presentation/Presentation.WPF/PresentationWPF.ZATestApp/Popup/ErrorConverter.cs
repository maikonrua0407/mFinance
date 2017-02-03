using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Globalization;

namespace PresentationWPF.ZATestApp.Popup
{
    [ValueConversion(typeof(ReadOnlyObservableCollection<ValidationError>), typeof(string))]
    public class ErrorConverter : MarkupExtension, IValueConverter
    {
        private ErrorConverter _mySelf;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_mySelf == null)
            {
                _mySelf = new ErrorConverter();
            }
            return _mySelf;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ReadOnlyObservableCollection<ValidationError> errors =
                value as ReadOnlyObservableCollection<ValidationError>;

            StringBuilder errorMessage = new StringBuilder();
            if (null != errors)
            {
                foreach (var error in errors)
                {
                    errorMessage.AppendLine(error.ErrorContent.ToString());
                }

                return errorMessage;
            }

            return errorMessage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }  
}
