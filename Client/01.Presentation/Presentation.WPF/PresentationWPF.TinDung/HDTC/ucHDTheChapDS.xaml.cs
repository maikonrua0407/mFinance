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
using Presentation.Process;
using Telerik.Windows.Controls;
using Presentation.Process.Common;

namespace PresentationWPF.TinDung.HDTC
{
    /// <summary>
    /// Interaction logic for ucHDTheChapDS.xaml
    /// </summary>
    public partial class ucHDTheChapDS : UserControl
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

        // Danh sách các item được chọn trong treeview
        private List<string> lstCheckedKhuVuc = new List<string>();
        private List<string> lstCheckedSanPham = new List<string>();
        private List<AutoCompleteEntry> lstLoaiGTo = new List<AutoCompleteEntry>();
        private DataTable dtTree;
        List<string> lstDonVi = new List<string>();
        delegate void LoadFormChiTiet();
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucHDTheChapDS()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.DanhMuc;component/HDTC/ucHDTheChapDS.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindHotkey();
            radPage.PageSize = (int)nudPageSize.Value;
            KhoiTaoGiaTriComboBox();
            BuildTree();
            InitEventHandler();
            SetNullForControl();
            LoadDuLieu();
        }

        void KhoiTaoGiaTriComboBox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.LOAI_GIAY_TO.getValue());
            lstLoaiGTo.Add(new AutoCompleteEntry("Tất cả", "", "0"));
            auto.GenAutoComboBox(ref lstLoaiGTo, ref cmbLoaiGiayTo, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
        }

        void InitEventHandler()
        {
            tvwChiNhanh.SelectionChanged += new SelectionChangedEventHandler(tvwChiNhanh_SelectionChanged);
            raddgrHopDongTheChapDS.MouseDoubleClick += new MouseButtonEventHandler(raddgrHopDongTheChapDS_MouseDoubleClick);
            this.Unloaded += new RoutedEventHandler(ucHDTheChapDS_Unloaded);
        }

        void ucHDTheChapDS_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        void BuildTree()
        {
            try
            {
                dtTree = new TaiSanDamBaoProcess().getDanhSachDonVi().Tables[0];
                RadTreeViewItem ItemRoot = new RadTreeViewItem();
                ItemRoot.Tag = "";
                ItemRoot.Header = "Danh mục đơn vị";
                ItemRoot.IsExpanded = true;
                ItemRoot.IsChecked = true;
                tvwChiNhanh.Items.Add(ItemRoot);
                BuildTree(ItemRoot);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
            }
        }

        private void BuildTree(RadTreeViewItem item)
        {
            foreach (DataRow row in dtTree.Rows)
            {
                if (row["ma_dvi_cha"].ToString().Equals(item.Tag))
                {
                    RadTreeViewItem subItem = new RadTreeViewItem();
                    subItem.Header = row["ten_gdich"].ToString();
                    subItem.Tag = row["ma_dvi"].ToString();
                    subItem.IsExpanded = true;
                    subItem.IsChecked = true;
                    item.Items.Add(subItem);
                    BuildTree(subItem);
                }
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
                beforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                beforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                beforeRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                beforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
                Xem();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
                LoadDuLieu();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
                XuatExcel();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
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
            {
                Them();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                Sua();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                beforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                beforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                beforeRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                beforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
                Xem();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
                LoadDuLieu();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
                XuatExcel();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }
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
            PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(raddgrHopDongTheChapDS, txtTimKiemNhanh.Text);
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
            if (raddgrHopDongTheChapDS != null && raddgrHopDongTheChapDS.DataContext != null)
            {
                DataTable dt = ((DataView)raddgrHopDongTheChapDS.DataContext).Table;
                if (dt != null)
                {
                    radPage.PageSize = (int)nudPageSize.Value;
                    raddgrHopDongTheChapDS.DataContext = dt.DefaultView;
                }
            }
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(raddgrHopDongTheChapDS);
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

        void tvwChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstDonVi = new List<string>();
            RadTreeViewItem item = (RadTreeViewItem)tvwChiNhanh.SelectedItem;
            LayChuoiDanhMucDonVi(item);
            LoadDuLieu();
        }

        private void LayChuoiDanhMucDonVi(RadTreeViewItem item)
        {
            lstDonVi.Add(item.Tag.ToString());
            for (int i = 0; i < item.Items.Count; i++)
            {
                RadTreeViewItem subItem = (RadTreeViewItem)item.Items[i];
                subItem.IsChecked = true;
                LayChuoiDanhMucDonVi(subItem);
            }
        }

        void SetNullForControl()
        {
            teldtNgayHopDongTu.Value = null;
            teldtNgayHopDongDen.Value = null;
            txtGiaTriTu.Value = null;
            txtGiaTriDen.Value = null;
        }

        void raddgrHopDongTheChapDS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Xem();
        }
        #endregion

        #region Xy ly nghiep vu
        void LoadDuLieu()
        {
            TaiSanDamBaoProcess tsdbProcess = new TaiSanDamBaoProcess();
            this.Cursor = Cursors.Wait;
            try
            {
                string slstDonVi = "NULL";
                if (lstDonVi.Count > 0)
                {
                    slstDonVi = "";
                    foreach (string sDonVi in lstDonVi)
                    {
                        slstDonVi += ",''" + sDonVi + "''";
                    }
                    slstDonVi = slstDonVi.Substring(1);
                }
                string TThaiNvu = ucTrangThaiNVu.GetItemsSelected();
                string SoHDTC = txtSoHDTD.Text;
                string NgayHDTu = teldtNgayHopDongTu.Value != null ? LDateTime.DateToString((DateTime)teldtNgayHopDongTu.Value, ApplicationConstant.defaultDateTimeFormat) : "";
                string NgayHDDen = teldtNgayHopDongDen.Value != null ? LDateTime.DateToString((DateTime)teldtNgayHopDongDen.Value, ApplicationConstant.defaultDateTimeFormat) : "";
                string MaTS = txtMaTaiSan.Text;
                string TenTS = txtTenTaiSan.Text;
                string TongGTriTu = txtGiaTriTu.Value != null ? txtGiaTriTu.Value.ToString() : "0";
                string TongGTriDen = txtGiaTriDen.Value != null ? txtGiaTriDen.Value.ToString() : "0";
                string MaKHang = txtMaKhachHang.Text;
                string TenKHang = txtTenKhachHang.Text;
                string LoaiGiayTo = lstLoaiGTo.ElementAt(cmbLoaiGiayTo.SelectedIndex).KeywordStrings.FirstOrDefault();
                string SoGiayTo = txtSoGiayTo.Text;
                string DienThoai = txtDienThoai.Text;
                string Email = txtEmail.Text;
                DataSet ds = tsdbProcess.getDanhSachHopDongTC(TThaiNvu, SoHDTC, NgayHDTu, NgayHDDen, MaTS, TenTS, TongGTriTu, TongGTriDen, MaKHang, TenKHang, LoaiGiayTo, SoGiayTo, DienThoai, Email,slstDonVi);
                if (ds != null)
                {
                    raddgrHopDongTheChapDS.ItemsSource = null;
                    raddgrHopDongTheChapDS.ItemsSource = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
            }
            finally
            {
                tsdbProcess = null;
                this.Cursor = Cursors.Arrow;
            }
        }
        void Them()
        {
            if (!tlbAdd.IsEnabled)
                return;
                ucHDTheChapCT objHDTCCT = new ucHDTheChapCT();
                objHDTCCT.OnSavingCompleted += new EventHandler(objHDTCCT_OnSavingCompleted);
                Window window = new Window();
                window.Content = objHDTCCT;
                window.ShowDialog();
        }
        void Xem()
        {
            if (!tlbView.IsEnabled)
                return;
            if (raddgrHopDongTheChapDS.SelectedItems.Count == 1)
            {
                DataRow dr = (DataRow)raddgrHopDongTheChapDS.SelectedItems[0];
                ucHDTheChapCT objHDTCCT = new ucHDTheChapCT();
                objHDTCCT.BSua = false;
                objHDTCCT.IdHDTC = Convert.ToInt32(dr["ID"]);
                objHDTCCT.OnSavingCompleted += new EventHandler(objHDTCCT_OnSavingCompleted);
                LoadFormChiTiet loadForm = new LoadFormChiTiet(objHDTCCT.SetDataForm);
                Window window = new Window();
                window.Content = objHDTCCT;
                loadForm();
                window.ShowDialog();
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
                return;
            }
        }
        void Sua()
        {
            if (!tlbView.IsEnabled)
                return;
            if (raddgrHopDongTheChapDS.SelectedItems.Count == 1)
            {
                DataRow dr = (DataRow)raddgrHopDongTheChapDS.SelectedItems[0];
                ucHDTheChapCT objHDTCCT = new ucHDTheChapCT();
                objHDTCCT.BSua = true;
                objHDTCCT.IdHDTC = Convert.ToInt32(dr["ID"]);
                objHDTCCT.OnSavingCompleted += new EventHandler(objHDTCCT_OnSavingCompleted);
                LoadFormChiTiet loadForm = new LoadFormChiTiet(objHDTCCT.SetDataForm);
                Window window = new Window();
                window.Content = objHDTCCT;
                loadForm();
                window.ShowDialog();
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
                return;
            }
        }

        void beforeDelete()
        {
            if (!tlbDelete.IsEnabled)
                return;
            List<int> lstId = new List<int>();
            if (raddgrHopDongTheChapDS.SelectedItems.Count > 0)
            {
                foreach (DataRow dr in raddgrHopDongTheChapDS.SelectedItems)
                {
                    lstId.Add(Convert.ToInt32(dr["ID"]));
                }
                onDelete(lstId);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
                return;
            }
        }

        void onDelete(List<int> lstId)
        {
            if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            // Yêu cầu look dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retLockData = process.UnlockData(DatabaseConstant.Module.TIN_DUNG_VI_MO,
                DatabaseConstant.Function.TD_DANH_SACH_HOP_DONG_THE_CHAP,
                DatabaseConstant.Table.TD_HDTC,
                DatabaseConstant.Action.XOA,
                lstId);
            //Bắt đầu xóa dữ liệu
            if (retLockData)
            {
                List<ClientResponseDetail> responseDetail = new List<ClientResponseDetail>();
                new TaiSanDamBaoProcess().XoaHopDongTheChap(lstId, ref responseDetail);
                afterDelete(lstId, responseDetail);
            }
            else
            {
                LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                return;
            }
        }

        void afterDelete(List<int> lstId,List<ClientResponseDetail> responseDetail)
        {
            
            CommonFunction.ThongBaoKetQua(responseDetail);
            LoadDuLieu();

            // Yêu cầu unlook dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retLockData = process.UnlockData(DatabaseConstant.Module.TIN_DUNG_VI_MO,
                DatabaseConstant.Function.TD_DANH_SACH_HOP_DONG_THE_CHAP,
                DatabaseConstant.Table.TD_HDTC,
                DatabaseConstant.Action.XOA,
                lstId);
        }

        void beforeApprove()
        {
            if (!tlbApprove.IsEnabled)
                return;
            List<int> lstId = new List<int>();
            if (raddgrHopDongTheChapDS.SelectedItems.Count > 0)
            {
                foreach (DataRow dr in raddgrHopDongTheChapDS.SelectedItems)
                {
                    lstId.Add(Convert.ToInt32(dr["ID"]));
                }
                onApprove(lstId);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
                return;
            }
        }

        void onApprove(List<int> lstId)
        {
            if (LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            // Yêu cầu look dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retLockData = process.UnlockData(DatabaseConstant.Module.TIN_DUNG_VI_MO,
                DatabaseConstant.Function.TD_DANH_SACH_HOP_DONG_THE_CHAP,
                DatabaseConstant.Table.TD_HDTC,
                DatabaseConstant.Action.DUYET,
                lstId);
            //Bắt đầu xóa dữ liệu
            if (retLockData)
            {
                List<ClientResponseDetail> responseDetail = new List<ClientResponseDetail>();
                new TaiSanDamBaoProcess().DuyetHopDongTheChap(lstId, ref responseDetail);
                afterApprove(lstId, responseDetail);
            }
            else
            {
                LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                return;
            }
        }

        void afterApprove(List<int> lstId, List<ClientResponseDetail> responseDetail)
        {
            
            CommonFunction.ThongBaoKetQua(responseDetail);
            LoadDuLieu();

            // Yêu cầu unlook dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retLockData = process.UnlockData(DatabaseConstant.Module.TIN_DUNG_VI_MO,
                DatabaseConstant.Function.TD_DANH_SACH_HOP_DONG_THE_CHAP,
                DatabaseConstant.Table.TD_HDTC,
                DatabaseConstant.Action.DUYET,
                lstId);
        }

        void beforeRefuse()
        {
            if (!tlbRefuse.IsEnabled)
                return;
            List<int> lstId = new List<int>();
            if (raddgrHopDongTheChapDS.SelectedItems.Count > 0)
            {
                foreach (DataRow dr in raddgrHopDongTheChapDS.SelectedItems)
                {
                    lstId.Add(Convert.ToInt32(dr["ID"]));
                }
                onRefuse(lstId);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
                return;
            }
        }

        void onRefuse(List<int> lstId)
        {
            if (LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            // Yêu cầu look dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retLockData = process.UnlockData(DatabaseConstant.Module.TIN_DUNG_VI_MO,
                DatabaseConstant.Function.TD_DANH_SACH_HOP_DONG_THE_CHAP,
                DatabaseConstant.Table.TD_HDTC,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);
            //Bắt đầu xóa dữ liệu
            if (retLockData)
            {
                List<ClientResponseDetail> responseDetail = new List<ClientResponseDetail>();
                new TaiSanDamBaoProcess().TuChoiHopDongTheChap(lstId, ref responseDetail);
                afterRefuse(lstId, responseDetail);
            }
            else
            {
                LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                return;
            }
        }

        void afterRefuse(List<int> lstId, List<ClientResponseDetail> responseDetail)
        {
            
            CommonFunction.ThongBaoKetQua(responseDetail);
            LoadDuLieu();

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retLockData = process.UnlockData(DatabaseConstant.Module.TIN_DUNG_VI_MO,
                DatabaseConstant.Function.TD_DANH_SACH_HOP_DONG_THE_CHAP,
                DatabaseConstant.Table.TD_HDTC,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);
        }

        void beforeCancel()
        {
            if (!tlbCancel.IsEnabled)
                return;
            List<int> lstId = new List<int>();
            if (raddgrHopDongTheChapDS.SelectedItems.Count > 0)
            {
                foreach (DataRow dr in raddgrHopDongTheChapDS.SelectedItems)
                {
                    lstId.Add(Convert.ToInt32(dr["ID"]));
                }
                onCancel(lstId);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
                return;
            }
        }

        void onCancel(List<int> lstId)
        {
            if (LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            // Yêu cầu look dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retLockData = process.UnlockData(DatabaseConstant.Module.TIN_DUNG_VI_MO,
                DatabaseConstant.Function.TD_DANH_SACH_HOP_DONG_THE_CHAP,
                DatabaseConstant.Table.TD_HDTC,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
            //Bắt đầu xóa dữ liệu
            if (retLockData)
            {
                List<ClientResponseDetail> responseDetail = new List<ClientResponseDetail>();
                new TaiSanDamBaoProcess().ThoaiDuyetHopDongTheChap(lstId, ref responseDetail);
                afterCancel(lstId, responseDetail);
            }
            else
            {
                LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                return;
            }
        }

        void afterCancel(List<int> lstId, List<ClientResponseDetail> responseDetail)
        {           
            
            CommonFunction.ThongBaoKetQua(responseDetail);
            LoadDuLieu();

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retLockData = process.UnlockData(DatabaseConstant.Module.TIN_DUNG_VI_MO,
                DatabaseConstant.Function.TD_DANH_SACH_HOP_DONG_THE_CHAP,
                DatabaseConstant.Table.TD_HDTC,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
        }

        void objHDTCCT_OnSavingCompleted(object sender, EventArgs e)
        {
            LoadDuLieu();
        }
        #endregion
    }
}
