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
using System.Data;
using Presentation.Process;
using Utilities.Common;
using PresentationWPF.CustomControl;
using Telerik.Windows.Controls;
using Presentation.Process.Common;

namespace PresentationWPF.TinDung.SanPham
{
    /// <summary>
    /// Interaction logic for ucDangKySanPhamDS_01.xaml
    /// </summary>
    public partial class ucDangKySanPhamDS_01 : UserControl
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

        private string maDonVi = "%";

        // Danh sách các item được chọn trong treeview
        private List<string> lstCheckedKhuVuc = new List<string>();
        private List<string> lstCheckedSanPham = new List<string>();

        List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiSanPham = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstNguonVon = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstPhuongThucTinhLai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiLaiSuat = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstHieuLuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstMucDichSuDung = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstPhuongThucVay = new List<AutoCompleteEntry>();
        //
        private delegate void LoadDuLieuCT(bool bSua);
        DatabaseConstant.Function function = DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM;
        DatabaseConstant.Module module = DatabaseConstant.Module.TDVM;
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucDangKySanPhamDS_01()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDung;component/SanPham/ucDangKySanPhamDS_01.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindHotkey();
            nudPageSize.Value = ClientInformation.SoLuongBanGhi;
            radPage.PageSize = (int)nudPageSize.Value;
            KhoiTaoControl();
            BuildTreePhuongThucVay();
            LoadDuLieu();
        }
        void KhoiTaoControl()
        {
            try
            {
            string Dislay = LLanguage.SearchResourceByKey("U.DungChung.TatCa");
            teldtNgayADungDen.Value = null;
            teldtNgayADungTu.Value = null;
            teldtNgayHetHanDen.Value = null;
            teldtNgayHetHanTu.Value = null;
            List<string> lstDieuKien = new List<string>();
            string maTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
 
            // Gán giá trị điều kiện
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.PHUONG_THUC_TINH_LAI));
            lstPhuongThucTinhLai.Add(new AutoCompleteEntry(Dislay, "%", "%"));
            KhoiTaoGiaTriComboBox(ref lstPhuongThucTinhLai, ref cmbPhuongThucTinhLai, maTruyVan, lstDieuKien);

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.MUC_DICH_VAY_VON));
            lstMucDichSuDung.Add(new AutoCompleteEntry(Dislay,"%","%"));
            KhoiTaoGiaTriComboBox(ref lstMucDichSuDung, ref cmbMucDich, maTruyVan, lstDieuKien);

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.LOAI_SAN_PHAM_TDUNG));
            lstLoaiSanPham.Add(new AutoCompleteEntry(Dislay, "%", "%"));
            KhoiTaoGiaTriComboBox(ref lstLoaiSanPham, ref cmbLoaiSanPham, maTruyVan, lstDieuKien);

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.TRANG_THAI_BAN_GHI));
            lstHieuLuc.Add(new AutoCompleteEntry(Dislay, "%", "%"));
            KhoiTaoGiaTriComboBox(ref lstHieuLuc, ref cmbTinhTrang, maTruyVan, lstDieuKien);

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.NGUON_VON_VAY));
            lstNguonVon.Add(new AutoCompleteEntry(Dislay, "%", "%"));
            KhoiTaoGiaTriComboBox(ref lstNguonVon, ref cmbLoaiVay, maTruyVan, lstDieuKien);

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.LOAI_LAI_SUAT));
            lstLoaiLaiSuat.Add(new AutoCompleteEntry(Dislay, "%", "%"));
            KhoiTaoGiaTriComboBox(ref lstLoaiLaiSuat, ref cmbLoaiLaiSuat, maTruyVan, lstDieuKien);

            AutoComboBox auto = new AutoComboBox();
            lstDieuKien = new List<string>();
            lstDieuKien.Add("'" + DatabaseConstant.ToChucDonVi.HSO.getValue() + "', '" + DatabaseConstant.ToChucDonVi.CNH.getValue() + "'");
            auto.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOLOAI.getValue(), lstDieuKien, ClientInformation.MaDonVi);
            if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()) || ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()))
                cmbDonVi.IsEnabled = false;
            cmbDonVi.SelectionChanged += new SelectionChangedEventHandler(cmbDonVi_SelectionChanged);
            
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Khởi tạo lấy Items cho ComboBox 
        /// </summary>
        /// <param name="lstAutoComplete"></param>
        /// <param name="cmbControl"></param>
        /// <param name="sMaTruyVan"></param>
        /// <param name="lstDKien"></param>
        /// <param name="bSelectChanged"></param>
        void KhoiTaoGiaTriComboBox(ref List<AutoCompleteEntry> lstAutoComplete, ref RadComboBox cmbControl, string sMaTruyVan, List<string> lstDKien = null)
        {
            AutoComboBox autoComboBox = new AutoComboBox();
            autoComboBox.GenAutoComboBox(ref lstAutoComplete, ref cmbControl, sMaTruyVan, lstDKien);
        }

        void BuildTreePhuongThucVay()
        {
            RadComboBox cmbPhuongThucVay = new RadComboBox();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.PHUONG_THUC_VAY.getValue());
            new AutoComboBox().GenAutoComboBox(ref lstPhuongThucVay, ref cmbPhuongThucVay, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
            try
            {
                RadTreeViewItem Item = new RadTreeViewItem();
                Item.Header = LLanguage.SearchResourceByKey("U.DMUC_LOAI.PHUONG_THUC_VAY");
                Item.IsExpanded = true;
                Item.IsChecked = true;
                Item.Tag = "";
                for (int i = 0; i < lstPhuongThucVay.Count;i++ )
                {
                    RadTreeViewItem ItemSub = new RadTreeViewItem();
                    ItemSub.Header = lstPhuongThucVay[i].DisplayName;
                    ItemSub.Tag = lstPhuongThucVay[i].KeywordStrings[0];
                    ItemSub.IsExpanded = true;
                    ItemSub.IsChecked = true;
                    Item.Items.Add(ItemSub);
                }
                tvwLoaiVay.Items.Add(Item);
            }
            catch (Exception ex)
            { }
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
            e.CanExecute = tlbAdd.IsEnabled;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Them();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbModify.IsEnabled;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Sua();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbDelete.IsEnabled;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Xoa();
        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbApprove.IsEnabled;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Duyet();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbRefuse.IsEnabled;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ThoaiDuyet();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbCancel.IsEnabled;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TuChoi();
        }

        private void ViewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbView.IsEnabled;
        }
        private void ViewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Xem();
        }

        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbSearch.IsEnabled;
        }
        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LoadDuLieu();
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
            e.CanExecute = tlbExport.IsEnabled;
        }
        private void ExportCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            XuatExcel();
        }

        private void HelpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbHelp.IsEnabled;
        }
        private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onHelp();
        }

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbClose.IsEnabled;
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
                TuChoi();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                ThoaiDuyet();
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
                TuChoi();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                ThoaiDuyet();
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
            PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grdDangKySanPhamDS, txtTimKiemNhanh.Text);
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
            if (grdDangKySanPhamDS != null && grdDangKySanPhamDS.ItemsSource != null)
            {
                DataRowView dt = ((DataRowView)grdDangKySanPhamDS.ItemsSource);
                if (dt != null)
                {
                    radPage.PageSize = (int)nudPageSize.Value;
                    grdDangKySanPhamDS.ItemsSource = dt;
                }
            }
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(grdDangKySanPhamDS);
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

        /// <summary>
        /// Sự kiện double click gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdDangKySanPhamDS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Xem();
        }

        private void cmbDonVi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if(!LObject.IsNullOrEmpty(lstSourceDonVi) && cmbDonVi.SelectedIndex > -1)
                {
                    maDonVi = lstSourceDonVi.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (maDonVi.Equals(DatabaseConstant.MA_HSO)) maDonVi = "%";
                    LoadDuLieu();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }

        }

        /// <summary>
        /// Load lại dữ liệu khi khi thêm, sửa, xóa chi tiết
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ucSanPhamCT_OnSavingComleted(object sender, EventArgs e)
        {
            LoadDuLieu();
        }
        #endregion

        #region Xu ly nghiep vu

        private void LoadDuLieu()
        {
            Cursor = Cursors.Wait;
            try
            {
                TinDungProcess tindungProcess = new TinDungProcess();
                maDonVi = lstSourceDonVi.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.ElementAt(0);
                string MaTTNVu = ucCheckBoxList.GetItemsSelected();
                string MaLoaiSP = lstLoaiSanPham.ElementAt(cmbLoaiSanPham.SelectedIndex).KeywordStrings.FirstOrDefault();
                string MaTinhTrangHLuc = lstHieuLuc.ElementAt(cmbTinhTrang.SelectedIndex).KeywordStrings.FirstOrDefault();
                string MaMucDichVay = lstMucDichSuDung.ElementAt(cmbMucDich.SelectedIndex).KeywordStrings.FirstOrDefault();
                string MaNguonVon = lstNguonVon.ElementAt(cmbLoaiVay.SelectedIndex).KeywordStrings.FirstOrDefault();
                string MaPhuongThucTinhLai = lstPhuongThucTinhLai.ElementAt(cmbPhuongThucTinhLai.SelectedIndex).KeywordStrings.FirstOrDefault();
                string MaLoaiLSuat = lstLoaiLaiSuat.ElementAt(cmbLoaiLaiSuat.SelectedIndex).KeywordStrings.FirstOrDefault();
                string MaSanPham = txtMaSanPham.Text;
                string TenSanPham = txtTenSanPham.Text;
                string NgayADungTu = "";
                if (teldtNgayADungTu.Value != null)
                    NgayADungTu = LDateTime.DateToString((DateTime)teldtNgayADungTu.Value, ApplicationConstant.defaultDateTimeFormat);
                string NgayADungDen = "";
                if (teldtNgayADungDen.Value != null)
                    NgayADungDen = LDateTime.DateToString((DateTime)teldtNgayADungDen.Value, ApplicationConstant.defaultDateTimeFormat);
                string NgayHetHanTu = "";
                if (teldtNgayHetHanTu.Value != null)
                    NgayHetHanTu = LDateTime.DateToString((DateTime)teldtNgayHetHanTu.Value, ApplicationConstant.defaultDateTimeFormat);
                string NgayHetHanDen = "";
                if (teldtNgayHetHanDen.Value != null)
                    NgayHetHanDen = LDateTime.DateToString((DateTime)teldtNgayHetHanDen.Value, ApplicationConstant.defaultDateTimeFormat);
                string ListPThucVay = "";
                foreach (RadTreeViewItem item in tvwLoaiVay.CheckedItems)
                {
                    ListPThucVay += ",''" + item.Tag.ToString() + "''";
                }
                if (ListPThucVay.Length > 0)
                    ListPThucVay = "(" + ListPThucVay.Substring(1) + ")";
                string UserName = "";
                grdDangKySanPhamDS.ItemsSource = tindungProcess.getDanhSachSanPhamTinDung(MaTTNVu, MaLoaiSP, MaTinhTrangHLuc, MaMucDichVay, MaNguonVon, MaPhuongThucTinhLai, MaLoaiLSuat, MaSanPham, TenSanPham, NgayADungTu, NgayADungDen, NgayHetHanTu, NgayHetHanDen,ListPThucVay, UserName, maDonVi).Tables["DANH_SACH"].DefaultView;
                if (!LObject.IsNullOrEmpty(grdDangKySanPhamDS.SelectedItems))
                    grdDangKySanPhamDS.SelectedItems.Clear();
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }
        /// <summary>
        /// Xem dữ liệu
        /// </summary>
        private void Sua()
        {
            if (tlbModify.IsEnabled == false)
                return;
            if (grdDangKySanPhamDS.SelectedItems.Count == 1)
            {
                DataRow dr = ((DataRowView)grdDangKySanPhamDS.SelectedItems[0]).Row;
                ucDangKySanPhamCT_01 ucSanPhamCT = new ucDangKySanPhamCT_01();
                ucSanPhamCT.IdSanPham = Convert.ToInt32(dr["ID"]);
                ucSanPhamCT.OnSavingComleted += new EventHandler(ucSanPhamCT_OnSavingComleted);
                LoadDuLieuCT loadDuLieu = new LoadDuLieuCT(ucSanPhamCT.LoadDuLieuCT);
                loadDuLieu(true);
                Window frm = new Window();
                frm.Title = DatabaseConstant.layNgonNguTieuDeForm(this.function);
                frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                frm.Content = ucSanPhamCT;
                frm.ShowDialog();
            }
            else
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
        }

        /// <summary>
        /// Sửa dữ liệu
        /// </summary>
        private void Xem()
        {
            if (tlbView.IsEnabled == false)
                return;
            if (grdDangKySanPhamDS.SelectedItems.Count == 1)
            {
                DataRow dr = ((DataRowView)grdDangKySanPhamDS.SelectedItems[0]).Row;
                ucDangKySanPhamCT_01 ucSanPhamCT = new ucDangKySanPhamCT_01();
                ucSanPhamCT.IdSanPham = Convert.ToInt32(dr["ID"]);
                ucSanPhamCT.OnSavingComleted +=new EventHandler(ucSanPhamCT_OnSavingComleted);
                LoadDuLieuCT loadDuLieu = new LoadDuLieuCT(ucSanPhamCT.LoadDuLieuCT);
                loadDuLieu(false);
                Window frm = new Window();
                frm.Title = DatabaseConstant.layNgonNguTieuDeForm(this.function);
                frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                frm.Content = ucSanPhamCT;
                frm.ShowDialog();
            }
            else
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
        }

        /// <summary>
        /// Thêm
        /// </summary>
        private void Them()
        {
            if (tlbAdd.IsEnabled == false)
                return;
            ucDangKySanPhamCT_01 ucSanPhamCT = new ucDangKySanPhamCT_01();
            ucSanPhamCT.OnSavingComleted += new EventHandler(ucSanPhamCT_OnSavingComleted);
            ucSanPhamCT.IdSanPham = 0;
            Window frm = new Window();
            frm.Title = DatabaseConstant.layNgonNguTieuDeForm(this.function);
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frm.Content = ucSanPhamCT;
            frm.ShowDialog();
        }

        /// <summary>
        /// Xóa
        /// </summary>
        private void Xoa()
        {
            if (tlbDelete.IsEnabled == false)
                return;
            try
            {
                if (grdDangKySanPhamDS.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        List<int> lstID = new List<int>();
                        foreach (DataRowView dr in grdDangKySanPhamDS.SelectedItems)
                        {
                            lstID.Add(Convert.ToInt32(dr["ID"]));
                        }
                        List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                        DatabaseConstant.Table.TD_SAN_PHAM,
                        DatabaseConstant.Action.TU_CHOI_DUYET,
                        lstID);
                        bool bResult = new TinDungProcess().XoaSanPhamTinDung(lstID,ref ResponseDetail);
                        if (bResult)
                        {
                            CommonFunction.ThongBaoKetQua(ResponseDetail);
                            // Yeu cau Unlook du lieu
                            retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                        DatabaseConstant.Table.TD_SAN_PHAM,
                        DatabaseConstant.Action.XOA,
                        lstID);
                            LoadDuLieu();
                        }
                        else
                        {
                            LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DanhMuc.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Duyệt
        /// </summary>
        private void Duyet()
        {
            if (!tlbApprove.IsEnabled)
                return;
            try
            {
                if (grdDangKySanPhamDS.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        List<int> lstID = new List<int>();
                        foreach (DataRowView dr in grdDangKySanPhamDS.SelectedItems)
                        {
                                lstID.Add(Convert.ToInt32(dr["ID"]));
                        }
                        List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                        DatabaseConstant.Table.TD_SAN_PHAM,
                        DatabaseConstant.Action.DUYET,
                        lstID);
                        bool bResult = new TinDungProcess().DuyetSanPhamTinDung(lstID,ref ResponseDetail);
                        if (bResult)
                        {
                            CommonFunction.ThongBaoKetQua(ResponseDetail);
                            // Yeu cau Unlook du lieu
                            retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                        DatabaseConstant.Table.TD_SAN_PHAM,
                        DatabaseConstant.Action.DUYET,
                        lstID);
                            LoadDuLieu();
                        }
                        else
                        {
                            LMessage.ShowMessage("M.DungChung.DuyetKhongThanhCong", LMessage.MessageBoxType.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DanhMuc.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Thoái duyệt
        /// </summary>
        private void ThoaiDuyet()
        {
            if (!tlbCancel.IsEnabled)
                return;
            try
            {
                if (grdDangKySanPhamDS.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        List<int> lstID = new List<int>();
                        foreach (DataRowView dr in grdDangKySanPhamDS.SelectedItems)
                        {
                                lstID.Add(Convert.ToInt32(dr["ID"]));
                        }
                        List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                        DatabaseConstant.Table.TD_SAN_PHAM,
                        DatabaseConstant.Action.THOAI_DUYET,
                        lstID);
                        bool bResult = new TinDungProcess().HuyDuyetSanPhamTinDung(lstID,ref ResponseDetail);
                        if (bResult)
                        {
                            CommonFunction.ThongBaoKetQua(ResponseDetail);
                            // Yeu cau Unlook du lieu
                            retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                        DatabaseConstant.Table.TD_SAN_PHAM,
                        DatabaseConstant.Action.THOAI_DUYET,
                        lstID);
                            LoadDuLieu();
                        }
                        else
                        {
                            LMessage.ShowMessage("M.DungChung.DuyetKhongThanhCong", LMessage.MessageBoxType.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DanhMuc.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Từ chối duyệt
        /// </summary>
        private void TuChoi()
        {
            if (!tlbRefuse.IsEnabled)
                return;
            try
            {
                if (grdDangKySanPhamDS.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        List<int> lstID = new List<int>();
                        foreach (DataRowView dr in grdDangKySanPhamDS.SelectedItems)
                        {
                                lstID.Add(Convert.ToInt32(dr["ID"]));
                        }
                        List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                        DatabaseConstant.Table.TD_SAN_PHAM,
                        DatabaseConstant.Action.TU_CHOI_DUYET,
                        lstID);
                        bool bResult = new TinDungProcess().TuChoiSanPhamTinDung(lstID,ref ResponseDetail);
                        if (bResult)
                        {
                            CommonFunction.ThongBaoKetQua(ResponseDetail);
                            // Yeu cau Unlook du lieu
                            retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                        DatabaseConstant.Table.TD_SAN_PHAM,
                        DatabaseConstant.Action.TU_CHOI_DUYET,
                        lstID);
                            LoadDuLieu();
                        }
                        else
                        {
                            LMessage.ShowMessage("M.DungChung.DuyetKhongThanhCong", LMessage.MessageBoxType.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DanhMuc.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
        #endregion

    }
}
