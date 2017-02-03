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
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using PresentationWPF.CustomControl;
using Telerik.Windows.Controls;
using Presentation.Process;
using Presentation.Process.TinDungServiceRef;
using Presentation.Process.Common;

namespace PresentationWPF.TinDungTT.DieuChinhLaiSuat
{
    /// <summary>
    /// Interaction logic for ucDCLaiSuatDS.xaml
    /// </summary>
    public partial class ucDCLaiSuatDS : UserControl
    {
        #region Khai bao
        public static RoutedCommand AddCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand ViewCommand = new RoutedCommand();
        public static RoutedCommand SearchCommand = new RoutedCommand();
        public static RoutedCommand ReloadCommand = new RoutedCommand();
        public static RoutedCommand ExportCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        private TDVM_DIEU_CHINH_LAI_SUAT TDVMDCHINHLSUAT = null;
        // Danh sách các item được chọn trong treeview
        private DataTable dtTreeDLy;
        private List<string> lstCheckedKhuVuc = new List<string>();
        private List<string> lstCheckedSanPham = new List<string>();
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucDCLaiSuatDS()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDungTT;component/DieuChinhLaiSuat/ucDCLaiSuatDS.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindHotkey();
            radPage.PageSize = (int)nudPageSize.Value;
            ClearForm();
            BuildTreeKhuVuc();
            //LoadData();
        }

        void BuildTreeKhuVuc()
        {
            dtTreeDLy = new TinDungProcess().getTreeViewDieuChinhLSuat(ClientInformation.MaDonVi, ClientInformation.TenDangNhap).Tables[0];
            try
            {
                RadTreeViewItem Item = new RadTreeViewItem();
                //Item.Header = "Danh mục địa lý";
                //Item.IsExpanded = true;
                //Item.IsChecked = true;
                //tvwKhuVuc.Items.Add(Item);
                BuildSubTreeKhuVuc(Item, null, 0);
            }
            catch (Exception ex)
            { }
        }

        void BuildSubTreeKhuVuc(RadTreeViewItem Item, DataRow dr, int iLevel)
        {

            List<DataRow> lstDataRow = null;
            if (dr != null)
                lstDataRow = dtTreeDLy.Select("NGAY_GD_CHA='" + dr["MA_NGAY_GD"] + "' AND LEVEL=" + iLevel).ToList();
            else
                lstDataRow = dtTreeDLy.Select("NGAY_GD_CHA=''").ToList();
            foreach (DataRow row in lstDataRow)
            {
                RadTreeViewItem subItem = new RadTreeViewItem();
                subItem.Header = row["NGAY_GD"].ToString();
                subItem.Tag = row["MA_NGAY_GD"].ToString();
                //subItem.IsExpanded = true;
                //subItem.IsChecked = true;
                if (row["LEVEL"].Equals(0))
                    tvwKhuVuc.Items.Add(subItem);
                else
                    Item.Items.Add(subItem);
                BuildSubTreeKhuVuc(subItem, row, Convert.ToInt32(row["LEVEL"]) + 1);
            }
        } 
        #endregion

        /// <summary>
        /// Dang ky hot key, shortcut key
        /// </summary>
        #region Dang ky hot key, shortcut key
        private void BindHotkey()
        {
            foreach (var child in Toolbar.Children)
            {
                if (child.GetType() == typeof(RibbonButton))
                {
                    RibbonButton tlb = (RibbonButton)child;
                    KeyBinding key = new KeyBinding();
                    string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.N, ModifierKeys.Control);
                        key = new KeyBinding(AddCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(ModifyCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.Shift);
                        key = new KeyBinding(DeleteCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.A, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ApproveCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(RefuseCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.C, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(CancelCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.W, ModifierKeys.Control);
                        key = new KeyBinding(ViewCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F, ModifierKeys.Control);
                        key = new KeyBinding(SearchCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F5, ModifierKeys.None);
                        key = new KeyBinding(ReloadCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.E, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ExportCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(HelpCommand, keyg);
                    }
                    //else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
                    //{
                    //    KeyGesture keyg = new KeyGesture(Key.Escape, ModifierKeys.None);
                    //    key = new KeyBinding(ucDonViDS.HelpCommand, keyg);
                    //}

                    if (key != null)
                    {
                        InputBindings.Add(key);
                    }
                }
            }
        }

        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ViewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ViewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            txtTimKiemNhanh.RaiseEvent(new RoutedEventArgs(Button.GotFocusEvent));
        }

        private void ReloadCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ReloadCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ExportCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ExportCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            XuatExcel();
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
        /// Sự kiện hot key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                Them();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                Sua();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                Xoa();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                Duyet();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
                Xem();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
                LoadData();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
                XuatExcel();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }
        /// <summary>
        /// Xử lý sự kiện keydown trên form
        /// Bao gồm:
        /// Nhấn Escape để thoát form
        /// Nhấn Enter/Tab để focus vào control tiếp theo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            // Kiểm tra escape thoát form
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);

            // Nhấn enter để chuyển focus tới control tiếp theo
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        /// <summary>
        /// Sự kiện shortcutkey
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
                txtTimKiemNhanh.RaiseEvent(new RoutedEventArgs(Button.GotFocusEvent));
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
                XuatExcel();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void onHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }
        #endregion

        /// <summary>
        /// Xu ly giao dien
        /// </summary>
        #region Xu ly giao dien
        /// <summary>
        /// Sự kiện tìm kiếm nhanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTimKiemNhanh_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtTimKiemNhanh.Text == LLanguage.SearchResourceByKey("U.DungChung.TimKiemNhanh"))
            {
                return;
            }
            PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grdDieuChinhLS, txtTimKiemNhanh.Text);
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

        /// <summary>
        /// Sự kiện thay đổi số bản ghi trên một page hiển thị
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudPageSize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (grdDieuChinhLS != null && grdDieuChinhLS.DataContext != null)
            {
                DataTable dt = ((DataView)grdDieuChinhLS.DataContext).Table;
                if (dt != null)
                {
                    radPage.PageSize = (int)nudPageSize.Value;
                    grdDieuChinhLS.DataContext = dt.DefaultView;
                }
            }
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(grdDieuChinhLS);
            //PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel("GET_DS_DONVI");
        }

        /// <summary>
        /// Sự kiện chọn ngày của DatetimePicker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DatePicker dtpControl = (DatePicker)sender;
                StringBuilder sbControl = new StringBuilder();
                sbControl.Append("teldt");
                sbControl.Append(dtpControl.Name.Substring(3));
                RadMaskedDateTimeInput telControl = (RadMaskedDateTimeInput)grMain.FindName(sbControl.ToString());
                if (telControl != null)
                    telControl.Value = dtpControl.SelectedDate;
                else
                    throw new System.NullReferenceException("Không tìm thấy control " + sbControl.ToString());
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void ClearForm()
        {
            teldtNgayDaoHanDen.Value = null;
            teldtNgayDaoHanTu.Value = null;
            teldtNgayDieuChinhDen.Value = null;
            teldtNgayDieuChinhTu.Value = null;
            teldtNgayNhanNoDen.Value = null;
            teldtNgayNhanNoTu.Value = null;
            TDVMDCHINHLSUAT = new TDVM_DIEU_CHINH_LAI_SUAT();
        }
        #endregion

        #region Xy ly nghiep vu
        void LoadData()
        {
            try
            {
                string MaTrangThai = ucTrangThaiNVu.GetItemsSelected();
                string SoGiaoDich = txtSoGiaoDich.Text;
                string NgayDieuChinhTu = teldtNgayDieuChinhTu.Value != null ? LDateTime.DateToString(teldtNgayDieuChinhTu.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat) : "";
                string NgayDieuChinhDen = teldtNgayDieuChinhDen.Value != null ? LDateTime.DateToString(teldtNgayDieuChinhDen.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat) : "";
                string MaNguonVon = "";
                string TenNguonVon = "";
                string MaHDTD = "";
                string MaKUOC = "";
                string NgayNhanNoTu = teldtNgayNhanNoTu.Value != null ? LDateTime.DateToString(teldtNgayNhanNoTu.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat) : "";
                string NgayNhanNoDen = teldtNgayNhanNoDen.Value != null ? LDateTime.DateToString(teldtNgayNhanNoDen.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat) : "";
                string NgayDaoHanTu = teldtNgayDaoHanTu.Value != null ? LDateTime.DateToString(teldtNgayDaoHanTu.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat) : "";
                string NgayDaoHanDen = teldtNgayDaoHanDen.Value != null ? LDateTime.DateToString(teldtNgayDaoHanDen.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat) : "";
                string SoTienGNganTu = txtSoTienVayTu.Value.GetValueOrDefault(0).ToString();
                string SoTienGNganDen = txtSoTienVayDen.Value.GetValueOrDefault(0).ToString();
                string SoDuTu = txtSoDuTu.Value.GetValueOrDefault(0).ToString();
                string SoDuDen = txtSoDuDen.Value.GetValueOrDefault(0).ToString();
                string ThoiHanVayTu = txtThoiHanVayTu.Value.GetValueOrDefault(0).ToString();
                string ThoiHanVayDen = txtThoiHanVayDen.Value.GetValueOrDefault(0).ToString();
                string ThoiHanVayDViTu = "";
                string ThoiHanVayDViDen = "";
                string LSuatTu = txtLaiSuatTu.Value.GetValueOrDefault(0).ToString();
                string LSuatDen = txtLaiSuatTu.Value.GetValueOrDefault(0).ToString();
                string MaKhachHang = txtMaKhachHang.Text;
                string TenKhachHang = txtTenKhachHang.Text;
                string LoaiGiayTo = "";
                string SoGiayTo = txtSoGiayTo.Text;
                string DienThoai = txtDienThoai.Text;
                string Email = txtEmail.Text;
                string MaSanPham = ClientInformation.MaDonViGiaoDich;
                if (LObject.IsNullOrEmpty(tvwKhuVuc.SelectedItem)) tvwKhuVuc.SelectedItem = tvwKhuVuc.Items[0];
                string ListKVuc = ((RadTreeViewItem)tvwKhuVuc.SelectedItem).Tag.ToString();
                DataSet ds = new TinDungProcess().GetDanhSachDieuChinhLSuat(MaTrangThai, SoGiaoDich, NgayDieuChinhTu, NgayDieuChinhDen, MaNguonVon, TenNguonVon, MaHDTD, MaKUOC, NgayNhanNoTu, NgayNhanNoDen, NgayDaoHanTu, NgayDaoHanDen, SoTienGNganTu, SoTienGNganDen, SoDuTu, SoDuDen, ThoiHanVayTu, ThoiHanVayDen, ThoiHanVayDViTu, ThoiHanVayDViDen, LSuatTu, LSuatDen, MaKhachHang, TenKhachHang, LoaiGiayTo, SoGiayTo, DienThoai, Email, MaSanPham, ListKVuc);
                if (ds != null)
                    grdDieuChinhLS.ItemsSource = ds.Tables["DANH_SACH"].DefaultView;
                if (!LObject.IsNullOrEmpty(grdDieuChinhLS.SelectedItems))
                    grdDieuChinhLS.SelectedItems.Clear();
            }
            catch (System.Exception ex)
            {
            	
            }
        }
        private void grdDieuChinhLS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Xem();
        }

        private void Them()
        {
            if (!tlbView.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            if (grdDieuChinhLS.SelectedItems.Count == 1)
            {
                DataRowView drv = grdDieuChinhLS.SelectedItem as DataRowView;
                ucDCLaiSuatCT ucLaiSuat = new ucDCLaiSuatCT();
                ucLaiSuat.action = DatabaseConstant.Action.THEM;
                string tittle = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT);
                Window win = new Window();
                win.Width = 1024;
                win.Height = 700;
                win.Content = ucLaiSuat;
                win.Title = tittle;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
            }
            else if (grdDieuChinhLS.SelectedItems.Count > 1)
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
            Cursor = Cursors.Arrow;
        }

        private void Xem()
        {
            if (!tlbView.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            if (grdDieuChinhLS.SelectedItems.Count == 1)
            {
                DataRowView drv = grdDieuChinhLS.SelectedItem as DataRowView;
                ucDCLaiSuatCT ucLaiSuat = new ucDCLaiSuatCT();
                ucLaiSuat.action = DatabaseConstant.Action.XEM;
                ucLaiSuat.IDGiaoDich = Convert.ToInt32(drv["ID"]);
                ucLaiSuat.LoadDataForm();
                string tittle = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT);
                Window win = new Window();
                win.Width = 1024;
                win.Height = 700;
                win.Content = ucLaiSuat;
                win.Title = tittle;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
            }
            else if (grdDieuChinhLS.SelectedItems.Count > 1)
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
            Cursor = Cursors.Arrow;
        }

        private void Sua()
        {
            if (!tlbView.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            if (grdDieuChinhLS.SelectedItems.Count == 1)
            {
                DataRowView drv = grdDieuChinhLS.SelectedItem as DataRowView;
                ucDCLaiSuatCT ucLaiSuat = new ucDCLaiSuatCT();
                ucLaiSuat.action = DatabaseConstant.Action.SUA;
                ucLaiSuat.IDGiaoDich = Convert.ToInt32(drv["ID"]);
                ucLaiSuat.LoadDataForm();
                string tittle = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT);
                Window win = new Window();
                win.Width = 1024;
                win.Height = 700;
                win.Content = ucLaiSuat;
                win.Title = tittle;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
            }
            else if (grdDieuChinhLS.SelectedItems.Count > 1)
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
            Cursor = Cursors.Arrow;
        }

        void Duyet()
        {
            if (!tlbApprove.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            List<int> lstID = new List<int>();
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = false;
            try
            {
                if (grdDieuChinhLS.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        List<int> lstId = new List<int>();
                        foreach (DataRowView drv in grdDieuChinhLS.SelectedItems)
                        {
                            lstId.Add(Convert.ToInt32(drv["ID"]));
                        }
                        TDVMDCHINHLSUAT.LST_ID_GIAO_DICH = lstId.ToArray();
                        TDVMDCHINHLSUAT.NGAY_GIAO_DICH = ClientInformation.NgayLamViecHienTai;
                        // Yêu cầu Unlock dữ liệu
                        retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT,
                        DatabaseConstant.Table.TD_TDOI_LSUAT,
                        DatabaseConstant.Action.DUYET,
                        lstId);
                        List<ClientResponseDetail> lstResponse = new List<ClientResponseDetail>();
                        new TinDungProcess().DuyetDieuChinhLSuatTinDung(ref TDVMDCHINHLSUAT, ref lstResponse);
                        CommonFunction.ThongBaoKetQua(lstResponse);
                        retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT,
                        DatabaseConstant.Table.TD_TDOI_LSUAT,
                        DatabaseConstant.Action.DUYET,
                        lstId);
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DanhMuc.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                // Yêu cầu Unlock dữ liệu
                retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG,
                DatabaseConstant.Table.TD_HDTDVM,
                DatabaseConstant.Action.DUYET,
                lstID);
                Cursor = Cursors.Arrow;
            }
        }

        void Xoa()
        {
            if (!tlbApprove.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            List<int> lstID = new List<int>();
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = false;
            try
            {
                if (grdDieuChinhLS.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        List<int> lstId = new List<int>();
                        foreach (DataRowView drv in grdDieuChinhLS.SelectedItems)
                        {
                            lstId.Add(Convert.ToInt32(drv["ID"]));
                        }
                        TDVMDCHINHLSUAT.LST_ID_GIAO_DICH = lstId.ToArray();
                        TDVMDCHINHLSUAT.NGAY_GIAO_DICH = ClientInformation.NgayLamViecHienTai;
                        // Yêu cầu Unlock dữ liệu
                        retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT,
                        DatabaseConstant.Table.TD_TDOI_LSUAT,
                        DatabaseConstant.Action.XOA,
                        lstId);
                        List<ClientResponseDetail> lstResponse = new List<ClientResponseDetail>();
                        new TinDungProcess().XoaDieuChinhLSuatTinDung(ref TDVMDCHINHLSUAT, ref lstResponse);
                        CommonFunction.ThongBaoKetQua(lstResponse);
                        retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT,
                        DatabaseConstant.Table.TD_TDOI_LSUAT,
                        DatabaseConstant.Action.XOA,
                        lstId);
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DanhMuc.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                // Yêu cầu Unlock dữ liệu
                retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG,
                DatabaseConstant.Table.TD_HDTDVM,
                DatabaseConstant.Action.DUYET,
                lstID);
                Cursor = Cursors.Arrow;
            }
        }
        #endregion

        
    }
}
