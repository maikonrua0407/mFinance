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
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using System.Data;
using Presentation.Process;
using PresentationWPF.CustomControl;
using Presentation.Process.BaoCaoServiceRef;
using System.Reflection;
using System.Collections;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Presentation.Process.Common;
//using PresentationWPF.HoTro.HDVO;
//using PresentationWPF.HoTro.TDVM;
using PresentationWPF.HoTro.GDKT;
using System.IO;

namespace PresentationWPF.HoTro.DungChung
{
    /// <summary>
    /// Interaction logic for ucDanhSach.xaml
    /// </summary>
    public partial class ucDanhSach : UserControl
    {
        #region Khai bao

        Application app = Application.Current;
        UserControl uc;

        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        HoTroProcess process = new HoTroProcess();
        bool isLoaded = false;
        public string formCase = null;
        private string dataQueryGet = "";
        private string dataQuerySet = "";
        List<HT_CNANG> lstBaoCaoTheoPhanHe = new List<HT_CNANG>();

        delegate void DieuKienBaoCao();

        HT_BAOCAO htBaoCao = null;
        List<HT_BAOCAO_TSO> lstHtBaoCaoTso = null;

        #endregion

        #region Khoi tao

        public ucDanhSach()
        {
            try
            {
                InitializeComponent();
                InitEventHandler();
                LoadDuLieu();
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// InitEventHandler
        /// </summary>
        private void InitEventHandler()
        {
            txtTimKiemNhanh.GotFocus += txtTimKiemNhanh_GotFocus;
            txtTimKiemNhanh.LostFocus += txtTimKiemNhanh_LostFocus;
            txtTimKiemNhanh.KeyDown += new KeyEventHandler(txtTimKiemNhanh_KeyDown);

        }
        #endregion

        #region Dang ky hot key

        private void BindHotkey()
        {
            foreach (var child in Toolbar.Children)
            {
                if (child.GetType() == typeof(RibbonButton))
                {
                    RibbonButton tlb = (RibbonButton)child;
                    KeyBinding key = new KeyBinding();
                    string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(HelpCommand, keyg);
                        key.Gesture = keyg;
                    }
                    if (key != null)
                        InputBindings.Add(key);
                }
            }
        }

        /// <summary>
        /// Sự kiện hot key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                ShowReport();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }

        #endregion

        #region Dang ky shortcut key

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ShowReport();
        }

        private void ExportCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ExportCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void HelpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onHelp();
        }

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void onHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }
        #endregion

        #region Xu ly nghiep vu

        private void ShowReport()
        {
            try
            {

            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        #endregion

        #region Xu ly giao dien

        private void LoadDuLieu()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                BuildGrid();

            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isLoaded)
            {
                formCase = ClientInformation.FormCase;

                // Khởi tạo các sự kiện cho controltxtTimKiemNhanh.KeyDown += txtTimKiemNhanh_KeyDown;
                txtTimKiemNhanh.GotFocus += txtTimKiemNhanh_GotFocus;
                txtTimKiemNhanh.LostFocus += txtTimKiemNhanh_LostFocus;
                grid.SelectionChanged += grid_SelectionChanged;
                grid.MouseDoubleClick += grid_MouseDoubleClick;
                isLoaded = true;
                txtTimKiemNhanh.Focus();
            }
        }

        /// <summary>
        /// Sự kiện tìm kiếm nhanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTimKiemNhanh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grid, txtTimKiemNhanh.Text);
            }
        }

        /// <summary>
        /// Sự kiện focus vào textbox tìm kiếm nhanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTimKiemNhanh_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtTimKiemNhanh.Text == LLanguage.SearchResourceByKey("U.DungChung.TimKiemNhanh"))
            {
                txtTimKiemNhanh.Text = "";
                txtTimKiemNhanh.Focus();
            }
        }

        /// <summary>
        /// Sự kiện rời focus khỏi textbox tìm kiếm nhanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTimKiemNhanh_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTimKiemNhanh.Text))
            {
                txtTimKiemNhanh.Text = LLanguage.SearchResourceByKey("U.DungChung.TimKiemNhanh");
            }
        }

        private void BuildGrid()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                string maPhanHeBaoCao = ClientInformation.FormCase;
                DataSet ds = process.GetDanhSachHoTro(ClientInformation.TenDangNhap,maPhanHeBaoCao);
                if (ds != null && ds.Tables.Count > 0)
                {
                    grid.ItemsSource = ds.Tables[0].DefaultView;
                }
                else
                {
                    grid.ItemsSource = null;
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void BuildFormDieuKien()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (grid.SelectedItems.Count > 0)
                {
                    // Lấy báo cáo được chọn từ grid
                    DataRowView dr = (DataRowView)grid.SelectedItem;                    
                    string maHoTro = dr["MA_HOTRO"].ToString();

                    // Lấy thông tin                  
                    DataSet ds = process.GetChiTietHoTro(maHoTro);

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        string uriDieuKien = "/" + ds.Tables[0].Rows[0]["NHOM_DIEUKIEN"].ToString() + ";component/" + ds.Tables[0].Rows[0]["FILE_DIEUKIEN"].ToString();
                        dataQueryGet = ds.Tables[0].Rows[0]["DATA_QUERY_GET"].ToString();
                        dataQuerySet = ds.Tables[0].Rows[0]["DATA_QUERY_SET"].ToString();

                        uc = (UserControl)System.Windows.Application.LoadComponent(new Uri(uriDieuKien, System.UriKind.RelativeOrAbsolute));
                    }

                    if (!LObject.IsNullOrEmpty(uc.GetType().GetProperty("dataQueryGet")))
                    {
                        uc.GetType().GetProperty("dataQueryGet").SetValue(uc, dataQueryGet, null);
                    }

                    frFormInput.Content = uc;
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void grid_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            BuildFormDieuKien();
        }

        private void grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BuildFormDieuKien();
        }

        #endregion

        void NavigationService_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            //((PageNavigation)e.Content).MessageFromCallingWindow = (string)e.ExtraData;
        }

        private void tlbProcess_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                // Lấy dữ liệu từ form điều kiện                                
                DatabaseConstant.Action action = DatabaseConstant.Action.IN;
                MethodInfo mi;
                if (uc != null)
                {
                    mi = uc.GetType().GetMethod("GetParameters");
                }
                else
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                    LMessage.ShowMessage("Chưa chọn vấn đề cần thao tác", LMessage.MessageBoxType.Error);
                    return;
                }

                object ret = mi.Invoke(uc, null);
                DataTable dt = null;
                if (ret != null)
                {
                    dt = (DataTable)ret;
                }
                else
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                    return;
                }

                DataSet ds = new HoTroProcess().ThucHien(dataQuerySet, dt);

                if (ds != null && ds.Tables.Count > 0)
                {
                    int i = ds.Tables.Count;
                    string kq = ds.Tables[i-1].Rows[0][0].ToString();
                    if (kq == "THANH_CONG")
                    {
                        LMessage.ShowMessage("Xử lý thành công", LMessage.MessageBoxType.Information);
                    }
                    else
                    {
                        CommonFunction.ThongBaoKetQua(ds.Tables[i-1]);
                    }
                }
                else
                {
                    LMessage.ShowMessage("Xử lý không thành công", LMessage.MessageBoxType.Warning);
                }

            }
            catch (System.Exception ex)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }

        }        
    }    
}
