using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.ComponentModel;
using PresentationWPF.CustomControl;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.Common;

namespace PresentationWPF.ZATestApp.Popup
{
    /// <summary>
    /// Interaction logic for ucPopupDemo.xaml
    /// </summary>
    public partial class ucPopupDemo : UserControl, INotifyPropertyChanged
    {   
        List<string> tinhThanhList = null;

        List<DataRow> lstPopup = new List<DataRow>();

        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        public ucPopupDemo()
        {
            InitializeComponent();
            tinhThanhList = new List<string> { "Hanoi", "Hue", "Saigon" };
            InitTinhThanhComboboxValue();
            
        }

        private void InitTinhThanhComboboxValue()
        {
            cboTinhThanh.ItemsSource = tinhThanhList;
        }

        private void rbtnDonVi_Click(object sender, RoutedEventArgs e)
        {
            var process = new PopupProcess();
            process.getPopupInformation("POPUP_DS_");

            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

            ucPopup popup = new ucPopup(true, simplePopupResponse, true);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.Content = popup;
            win.ShowDialog();
        }

        private void rbtnTinhThanh_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rbtnQuanHuyen_Click(object sender, RoutedEventArgs e)
        {

        }        

        private void rbtnPhuongXa_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cboTinhThanh_KeyDown(object sender, KeyEventArgs e)
        {
            tinhThanhList = new List<string> { "Hanoi", "Hue", "Saigon", "A", "B" };
            InitTinhThanhComboboxValue();
        }

        private void rbtnTest_Click(object sender, RoutedEventArgs e)
        {

        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnNotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        public string myProperty;

        public string MyProperty
        {
            get { return myProperty; }
            set
            {
                if (value == myProperty)
                    return;

                myProperty = value;
                OnNotifyPropertyChanged("myProperty");
            }
        }
    }
}
