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
using Utilities.Common;
using System.Windows.Media.Animation;
using PresentationWPF.CustomControl;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.Common;
using PresentationWPF.ZATestApp.Popup;
using System.Collections;
using System.Reflection;

namespace PresentationWPF.ZATestApp
{
    /// <summary>
    /// Interaction logic for ucDemo.xaml
    /// </summary>
    public partial class ucDemo : UserControl
    {
        int numberOfPage = 0, currentPage = 1, totalPage = 0;
        double _marqueeTimeInSeconds = 2;
        static DataTable dt;
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        public ucDemo()
        {
            InitializeComponent();
            LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "CCC");
            dt = new DataTable();
            System.Net.IPAddress localIP = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()).FirstOrDefault(e => e.AddressFamily.ToString().Equals("InterNetwork"));
            lbIP.Content = "Your IP address is " + localIP.ToString();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Code", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Note", typeof(string));
            for (int i = 0; i < 100; i++)
            {
                dt.NewRow();
                dt.Rows.Add(new object[] { i, "PC" + (10-i).ToString(), "This is Item " + i.ToString(), "" });
            }
            totalPage = dt.Rows.Count;
            if (lbPageSize.SelectedItem != null)
                numberOfPage = Convert.ToInt32(((ListBoxItem)lbPageSize.SelectedItem).Content);
            else
                numberOfPage = 5;
            BindData();
        }
        private void BindData()
        {
            int maxPage = 0;
            if (totalPage % numberOfPage > 0)
                maxPage = totalPage / numberOfPage + 1;
            else
                maxPage = totalPage / numberOfPage;
            lvDemo.DataContext = dt.AsEnumerable().Skip(numberOfPage * (currentPage - 1)).Take(numberOfPage).CopyToDataTable().DefaultView;
            if (currentPage == 1)
            {
                rbtnFirst.IsEnabled = false;
                rbtnPrevious.IsEnabled = false;
            }
            else
            {
                rbtnFirst.IsEnabled = true;
                rbtnPrevious.IsEnabled = true;
            }
            if (currentPage < maxPage || ((currentPage == maxPage) && ((totalPage % numberOfPage) > 0)))
            {
                rbtnNext.IsEnabled = true;
                rbtnLast.IsEnabled = true;
            }
            else
            {
                rbtnNext.IsEnabled = false;
                rbtnLast.IsEnabled = false;
            }
            txtCurrentPage.Text = currentPage + "/" + maxPage;
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem obj = (MenuItem)sender;
            if (obj.Header.Equals("Ascending"))
            {
                dt.DefaultView.Sort = "ID";
                lvDemo.DataContext = dt.DefaultView;
            }
            if (obj.Header.Equals("Descending"))
            {
                dt.DefaultView.Sort = "Code";
                lvDemo.DataContext = dt.DefaultView;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new UserControl1();
        }

        private void rbtnNext_Click(object sender, RoutedEventArgs e)
        {
            currentPage = currentPage + 1;
            BindData();
        }

        private void rbtnLast_Click(object sender, RoutedEventArgs e)
        {
            if (totalPage % numberOfPage > 0)
                currentPage = totalPage / numberOfPage + 1;
            else
                currentPage = totalPage / numberOfPage;
            BindData();
        }

        private void rbtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            currentPage = currentPage - 1;
            BindData();
        }

        private void rbtnFirst_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            BindData();
        }

        private void lbPageSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            numberOfPage = Convert.ToInt32(((ListBoxItem)lbPageSize.SelectedItem).Content);
            BindData();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new UserControl2();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            this.radBusyMain.IsEnabled = true;
            this.radBusyMain.IsBusy = true;
            this.Content = new UserControl3();
            this.radBusyMain.IsBusy = false;
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {

        }

        /*
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            ucPopup popup = new ucPopup(0, 1, false);
            Window win = new Window();
            win.Content = popup;
            win.ShowDialog();
        }
        */ 

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            var process = new PopupProcess();
            //process.getPopupInformation("POPUP_DS_DONVI");
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add("'SDU','KDU'");
            lstDieuKien.Add("1");
            process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DM_DIABAN.getValue(), lstDieuKien);

            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

            ucPopup popup = new ucPopup(false, simplePopupResponse, true);
            Window win = new Window();
            win.Content = popup;
            win.ShowDialog();
            int ID = popup.ID;
            // ???
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            var process = new PopupProcess();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add("NULL");
            lstDieuKien.Add("NULL");
            process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_KHACHHANG.getValue(),lstDieuKien);

            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

            ucPopup popup = new ucPopup(true, simplePopupResponse, true);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.Content = popup;
            win.ShowDialog();
        }

        private void tlbDonViPopup_Click(object sender, RoutedEventArgs e)
        {
            // Khởi tạo các giá trị cho popup
            //bool multiSelect = false;
            //string popupQueryCode = "GET_DS_DONVI_POPUP";
            //List<String> inputParameterValue = null;            
            //bool reloadRowFromServer = false;
            //string rowQueryCode = "GET_DS_DONVI_POPUP_ROW";
            //List<String> outputControlRelation = new List<string>{"txtMaDonVi", "lblTenDonVi"};
            //string targetMainForm = "";

            // Khởi tạo popup
            //ucPopup popup = new ucPopup(multiSelect, popupQueryCode, inputParameterValue, reloadRowFromServer, rowQueryCode, outputControlRelation);
            //popup.viewPopup();
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            //Window win = new Window();
            //win.Content = new ucDemoComboBox();
            //win.ShowDialog();
        }

        private void btnSample_Click(object sender, RoutedEventArgs e)
        {
            Window win = new Window();
            win.Content = new ucPopupDemo();
            win.ShowDialog();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetVisible setVisible = new SetVisible();
            ArrayList arr = setVisible.SetVisibleControl("PresentationWPF.ZATestApp.ucDemo", "Loai2");
            foreach (List<string> lst in arr)
            {
                object item = gridMain.FindName(lst.First());
                PropertyInfo prty = item.GetType().GetProperty("Visibility");
                if (lst.ElementAt(1).Equals("1"))
                    prty.SetValue(item, Visibility.Visible, null);
                else
                    prty.SetValue(item, Visibility.Hidden, null);
            }
        }
    }
}
