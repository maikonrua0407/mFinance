using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace PresentationWPF.CustomControl
{
    public class ucAlertDetailForm
    {
        public void CreateAlert(ref StackPanel spAlert, List<string> lstAlert)
        {
            foreach (string strAlert in lstAlert)
            {
                Label lbAlert = new Label();
                lbAlert.Content = (lstAlert.IndexOf(strAlert) + 1) + ". " + strAlert;
                lbAlert.Foreground = new SolidColorBrush(Colors.Red);
                lbAlert.Margin = new Thickness(120, 3, 0, 0);
                spAlert.Children.Add(lbAlert);
            }
        }
    }
}
