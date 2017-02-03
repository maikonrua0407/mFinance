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
using System.Windows.Shapes;
using System.IO;
using Presentation.Process.Common;
using Utilities.Common;
using System.Windows.Xps.Packaging;
using System.IO.Packaging;
using Telerik.Windows.Controls;
using System.Net;
using Presentation.Process.SupportServiceRef;
using Presentation.Process;

namespace PresentationWPF.CustomControl
{
    /// <summary>
    /// Interaction logic for WindowHelp.xaml
    /// </summary>
    public partial class WindowHelp01 : Window
    {
        private string filePath = "";
        private string title = "";
        private string webserver = "";
        private int port = 1001;
        private RadTreeViewItem parentItem = null;
        private HT_MENU objMenu;
        private string ngonNgu = "";

        public WindowHelp01()
        {
            InitializeComponent();
            CommonFunction.setIcon(this);
        }

        public WindowHelp01(int idChucNang)
            : this()
        {
            ngonNgu = ClientInformation.NgonNgu;
            BuildTreeRoot(idChucNang);
            //trvMenu.SelectionChanged += new SelectionChangedEventHandler(trvMenu_SelectionChanged);
        }

        void trvMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RadTreeViewItem item = ((RadTreeViewItem)(((RadTreeView)sender).SelectedItem));
            if (item != null)
            {
                filePath = @"/help/" + item.Uid + "_" + ngonNgu + ".html";
                title = item.Header.ToString();
                LoadBrowser();
            }
        }

        public void LoadBrowser()
        {        
            this.Title = "mFinance - " + title;

            webserver = "localhost";
            port = 1002;
            UriBuilder uriBuilder = new UriBuilder("http", webserver, port, filePath);

            Uri urlCheck = new Uri(uriBuilder.ToString());
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlCheck);
            request.Timeout = 15000;
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                wbSample.Navigate(uriBuilder.ToString());
            }
            catch (Exception)
            {
                LMessage.ShowMessage("M.DungChung.ThongBao.TaiLieuDangDuocPhatTrien", LMessage.MessageBoxType.Information);
                wbSample.NavigateToString("Updating...");
            }
        }  

        private void BuildTreeRoot(int idChucNang)
        {
            RadTreeViewItem treeviewChild = null;
            foreach (Presentation.Process.ZAMainAppServiceRef.ChucNangDto chucnang in ClientInformation.ListChucNang.Where(f => f.IDChucNangCha.IsNullOrEmpty()))
            {
                getMenu(chucnang.IDChucNang);
                treeviewChild = new RadTreeViewItem();
                treeviewChild.Tag = objMenu.ID;
                treeviewChild.Header = LLanguage.SearchResourceByKey(objMenu.MA_NNGU);
                treeviewChild.Uid = objMenu.MA_MENU;
                trvMenu.Items.Add(treeviewChild);
                parentItem = treeviewChild;
                if (idChucNang == objMenu.ID)
                    trvMenu.SelectedItem = treeviewChild;
                BuildTree(treeviewChild, idChucNang);
            }
        }

        private void BuildTree(RadTreeViewItem treeview, int idChucNang)
        {
            RadTreeViewItem treeviewChild = null;
            foreach (Presentation.Process.ZAMainAppServiceRef.ChucNangDto chucnang in ClientInformation.ListChucNang.Where(f => f.IDChucNangCha.Equals(treeview.Tag)))
            {
                getMenu(chucnang.IDChucNang);
                treeviewChild = new RadTreeViewItem();
                treeviewChild.Tag = objMenu.ID;
                treeviewChild.Header = LLanguage.SearchResourceByKey(objMenu.MA_NNGU);
                treeviewChild.Uid = objMenu.MA_MENU;
                treeview.Items.Add(treeviewChild);
                if (idChucNang == objMenu.ID)
                {
                    trvMenu.SelectedItem = treeviewChild;
                    parentItem.ExpandAll();
                }
                BuildTree(treeviewChild, idChucNang);
            }
        }

        private void getMenu(int id)
        {
            try
            {
                SupportProcess supportProcess = new SupportProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                HT_MENU obj= new HT_MENU();
                bool ret = false;

                obj.ID = id;

                ret = supportProcess.GetMenu(DatabaseConstant.Action.LOAD, ref obj, ref listClientResponseDetail);
                if (ret == true)
                {
                    objMenu = obj;
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }
    }
}
