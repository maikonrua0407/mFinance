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
using PresentationWPF.CustomControl;
using Telerik;
using Telerik.Windows.Controls;
using System.Data;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.Common;
using Presentation.Process.TinDungServiceRef;
using System.Reflection;

namespace PresentationWPF.TinDung.DieuChinhLaiSuat
{
    /// <summary>
    /// Interaction logic for ucDCLaiSuatCT.xaml
    /// </summary>
    public partial class ucDCLaiSuatCT : UserControl
    {
        /// <summary>
        /// Khai bao
        /// </summary>
        #region Khai bao
        public static RoutedCommand ImportCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand CloneCommand = new RoutedCommand();
        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SubmitCommand = new RoutedCommand();
        public static RoutedCommand CashStmtCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand PreviewCommand = new RoutedCommand();
        public static RoutedCommand ViewCommand = new RoutedCommand();
        public static RoutedCommand ExportCommand = new RoutedCommand();
        public static RoutedCommand SearchCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        private TDVM_DIEU_CHINH_LAI_SUAT TDVMDCHINHLSUAT = null;
        private List<AutoCompleteEntry> lstLoaiNguonVon = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiLSuatSP = new List<AutoCompleteEntry>();
        List<DataRow> lstPopup;
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        private List<DANH_SACH_KHE_UOC_THAY_DOI_LAI_SUAT> lstDanhSachKUOC = null;
        int iDGiaoDich = 0;
        private DataTable dtNhom;
        public int IDGiaoDich
        {
            get { return iDGiaoDich; }
            set { iDGiaoDich = value; }
        }
        public DatabaseConstant.Action action;
        string TThaiNV = "";
        public event EventHandler OnSavingCompleted;
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucDCLaiSuatCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDung;component/DieuChinhLaiSuat/ucDCLaiSuatCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            BindDataComboBox();
            KhoiTaoDataTable();
            ClearForm();
        }
        
        void BindDataComboBox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            // ComboBox phạm vi điều chỉnh
            lstDieuKien.Add(DatabaseConstant.DanhMuc.LOAI_LAI_SUAT.getValue());
            auto.GenAutoComboBox(ref lstLoaiLSuatSP, ref cmbLoaiLSuatSPham, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
            lstDieuKien.Clear();
            // ComboBox phạm vi điều chỉnh
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGUON_VON_VAY.getValue());
            lstLoaiNguonVon.Add(new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
            auto.GenAutoComboBox(ref lstLoaiNguonVon, ref cmbNguonVon, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

        }

        void KhoiTaoDataTable()
        {
            if (LObject.IsNullOrEmpty(dtNhom)) dtNhom = new DataTable();
            dtNhom.Columns.Add("ID_CUM", typeof(int));
            dtNhom.Columns.Add("ID_NHOM", typeof(int));
            dtNhom.Columns.Add("MA_CUM", typeof(string));
            dtNhom.Columns.Add("MA_NHOM", typeof(string));
            dtNhom.Columns.Add("TEN_CUM", typeof(string));
            dtNhom.Columns.Add("TEN_NHOM", typeof(string));
        }
        #endregion

        /// <summary>
        /// Dang ky hot key, shortcut key
        /// </summary>
        #region Dang ky hot key, shortcut key
        /// <summary>
        /// Binding HotKey
        /// </summary>
        private void BindHotkey()
        {
            foreach (var child in Toolbar.Children)
            {
                if (child.GetType() == typeof(RibbonButton))
                {
                    RibbonButton tlb = (RibbonButton)child;
                    KeyBinding key = new KeyBinding();
                    string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAP_DU_LIEU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.I, ModifierKeys.Shift);
                        key = new KeyBinding(ImportCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(ModifyCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.Shift);
                        key = new KeyBinding(DeleteCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.V, ModifierKeys.Control|ModifierKeys.Shift);
                        key = new KeyBinding(CloneCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.H, ModifierKeys.Control);
                        key = new KeyBinding(HoldCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(SubmitCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Shift);
                        key = new KeyBinding(CashStmtCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.A, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ApproveCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(RefuseCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.C, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(CancelCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.W,ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(PreviewCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.W, ModifierKeys.Control);
                        key = new KeyBinding(ViewCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.E, ModifierKeys.Shift);
                        key = new KeyBinding(ExportCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(SearchCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(HelpCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Escape, ModifierKeys.None);
                        key = new KeyBinding(CloseCommand, keyg);
                        key.Gesture = keyg;
                    }

                    InputBindings.Add(key);
                }
            }
        }
        private void ImportCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ImportCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Nhập dữ liệu");
        }
        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Sửa dữ liệu");
        }
        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Xóa dữ liệu");
        }
        private void CloneCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CloneCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Nhân bản dữ liệu");
        }
        private void HoldCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Lưu tạm dữ liệu");
        }
        private void SubmitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SubmitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Lưu dữ liệu");
        }
        private void CashStmtCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CashStmtCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Bảng kê tiền mặt");
        }
        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Duyệt dữ liệu");
        }
        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Hủy duyệt dữ liệu");
        }
        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Từ chối dữ liệu");
        }
        private void PreviewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void PreviewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Xem trước dữ liệu");
        }
        private void ViewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ViewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Xem dữ liệu");
        }
        private void ExportCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ExportCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Xuất dữ liệu");
        }
        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Tìm kiếm dữ liệu");
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
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAP_DU_LIEU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ClearForm();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                OnModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                BeforeSave(BusinessConstant.TrangThaiNghiepVu.LUU_TAM, BusinessConstant.TrangThaiBanGhi.SU_DUNG);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                BeforeSave(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET, BusinessConstant.TrangThaiBanGhi.SU_DUNG);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                BeforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
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

        /// <summary>
        /// Sự kiện shortcutkey
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAP_DU_LIEU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ClearForm();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
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
        void ClearForm()
        {
            action = DatabaseConstant.Action.THEM;
            TThaiNV = "";
            txtMaSanPham.Text = "";
            txtMaSanPham.Tag = null;
            txtMaLaiSuatSP.Text = "";
            txtMaLaiSuatSP.Tag = null;
            numBienDoSP.Value = null;
            numTSuatMoiSP.Value = null;
            txtSoKU.Tag = 0;
            txtHDTD.Tag = null;
            txtSoKU.Text = "";
            txtHDTD.Text = "";
            grdDanhSachKhachHang.ItemsSource = dtNhom;
            TDVMDCHINHLSUAT = new TDVM_DIEU_CHINH_LAI_SUAT();
            lstDanhSachKUOC = new List<DANH_SACH_KHE_UOC_THAY_DOI_LAI_SUAT>();
            grdDieuChinhLSSanPham.ItemsSource = lstDanhSachKUOC;
            CommonFunction.RefreshButton(Toolbar, action, TThaiNV, mnuMain, DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT);
        }
        /// <summary>
        /// Sự kiện nhấn key trên form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
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

        private void btnMaSanPham_Click(object sender, RoutedEventArgs e)
        {
            if (LObject.IsNullOrEmpty(lstPopup)) lstPopup = new List<DataRow>();
            lstPopup.Clear();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.MaDonVi);
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation("POPUP_DS_SANPHAM_TD", lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            ucPopup popup = new ucPopup(false, simplePopupResponse);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.Content = popup;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ShowDialog();
            if (lstPopup.Count > 0)
            {
                txtMaSanPham.Text = lstPopup[0][2].ToString();
                txtMaSanPham.Tag = Convert.ToInt32(lstPopup[0][1]);
                lblTenSanPham.Content = lstPopup[0][3].ToString();
            }
        }

        private void btnMaLaiSuatSP_Click(object sender, RoutedEventArgs e)
        {
            if (LObject.IsNullOrEmpty(lstPopup)) lstPopup = new List<DataRow>();
            lstPopup.Clear();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add("TDVM");
            lstDieuKien.Add(ClientInformation.MaDonVi);
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation("POPUP_DS_LAISUAT", lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            ucPopup popup = new ucPopup(false, simplePopupResponse);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.Content = popup;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ShowDialog();
            if (lstPopup.Count > 0)
            {
                txtMaLaiSuatSP.Text = lstPopup[0][2].ToString();
                txtMaLaiSuatSP.Tag = lstPopup[0][1];
                lblTenLSuatSPham.Content = lstPopup[0][3];
            }
        }

        private void btnTinhToanLSuat_Click(object sender, RoutedEventArgs e)
        {
            if (LObject.IsNullOrEmpty(TDVMDCHINHLSUAT)) TDVMDCHINHLSUAT = new TDVM_DIEU_CHINH_LAI_SUAT();
            TDVMDCHINHLSUAT.MA_LAI_SUAT = txtMaLaiSuatSP.Text;
            TDVMDCHINHLSUAT.MA_SAN_PHAM = txtMaSanPham.Text;
            TDVMDCHINHLSUAT.MA_KHE_UOC = txtSoKU.Text;
            TDVMDCHINHLSUAT.MA_NGUON_VON = lstLoaiNguonVon.ElementAt(cmbNguonVon.SelectedIndex).KeywordStrings.FirstOrDefault();
            TDVMDCHINHLSUAT.MA_HDTD = txtHDTD.Text;
            string ListPhamVi = "";
            DataTable dtNhom = grdDanhSachKhachHang.ItemsSource as DataTable;
            foreach (DataRow dr in dtNhom.Rows)
            {
                ListPhamVi += "," + dr["ID_NHOM"].ToString();
            }
            if (ListPhamVi.Length > 0)
                ListPhamVi = "(" + ListPhamVi.Substring(1, ListPhamVi.Length - 1) + ")";
            TDVMDCHINHLSUAT.PHAM_VI_THAY_DOI = ListPhamVi;
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            new TinDungProcess().TinhToanDieuChinhLSuatTinDung(ref TDVMDCHINHLSUAT, ref lstResponseDetail);
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            if (LObject.IsNullOrEmpty(TDVMDCHINHLSUAT.DSACH_KHE_UOC))
                return;
            lstDanhSachKUOC = TDVMDCHINHLSUAT.DSACH_KHE_UOC.ToList();
            LoadDataGridData();
            expandThongTinTimKiem.IsExpanded = false;
            expendDSKheUoc.IsExpanded = true;
            
        }
        #endregion

        /// <summary>
        /// Xu ly nghiep vu
        /// </summary>
        #region Xu ly nghiep vu
        void LoadDataGridData()
        {
            grdDieuChinhLSSanPham.ItemsSource = null;
            grdDieuChinhLSSanPham.ItemsSource = lstDanhSachKUOC;
            //titemLSSanPham.IsSelected = true;
        }

        bool VaditionData()
        {
            return true;
        }

        bool GetDataForm(BusinessConstant.TrangThaiNghiepVu nghiepvu,BusinessConstant.TrangThaiBanGhi banghi)
        {
            lstDanhSachKUOC.Clear();
            if (LObject.IsNullOrEmpty(TDVMDCHINHLSUAT)) TDVMDCHINHLSUAT = new TDVM_DIEU_CHINH_LAI_SUAT();
            if (grdDieuChinhLSSanPham.SelectedItems.Count == 0)
                return false;
            foreach(DANH_SACH_KHE_UOC_THAY_DOI_LAI_SUAT objKUOC in grdDieuChinhLSSanPham.SelectedItems)
            {
                lstDanhSachKUOC.Add(objKUOC);
            }
            TDVMDCHINHLSUAT.DSACH_KHE_UOC = lstDanhSachKUOC.ToArray();
            TDVMDCHINHLSUAT.BIEN_BO = (decimal)numBienDoSP.Value.GetValueOrDefault(0);
            TDVMDCHINHLSUAT.LOAI_LAI_SUAT = lstLoaiLSuatSP.ElementAt(cmbLoaiLSuatSPham.SelectedIndex).KeywordStrings.First();
            TDVMDCHINHLSUAT.NGAY_GIAO_DICH = ClientInformation.NgayLamViecHienTai;
            TDVMDCHINHLSUAT.TAN_SUAT = (int)numTSuatMoiSP.Value.GetValueOrDefault(0);
            TDVMDCHINHLSUAT.NGUOI_LAP = ClientInformation.TenDangNhap;
            TDVMDCHINHLSUAT.NGAY_LAP = ClientInformation.NgayLamViecHienTai;
            TDVMDCHINHLSUAT.MA_GIAO_DICH = txtSoGiaoDich.Text;
            TDVMDCHINHLSUAT.TRANG_THAI_BAN_GHI = banghi.layGiaTri();
            TDVMDCHINHLSUAT.TRANG_THAI_NGHIEP_VU = nghiepvu.layGiaTri();
            if(iDGiaoDich>0)
            {
                TDVMDCHINHLSUAT.ID_GIAO_DICH = iDGiaoDich;
                TDVMDCHINHLSUAT.NGUOI_LAP = txtNguoiLap.Text;
                TDVMDCHINHLSUAT.NGAY_LAP = LDateTime.DateToString(teldtNgayNhap.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
                TDVMDCHINHLSUAT.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                TDVMDCHINHLSUAT.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
            }
            return true;
        }
        public void LoadDataForm()
        {
            DataSet ds = new TinDungProcess().getThongTinChiTietGiaoDichLSuatByID(iDGiaoDich.ToString());
            if(ds!=null)
            {
                LoadTabThongTinChung(ds);
                LoadTabKiemSoat(ds);
            }
        }

        void LoadTabThongTinChung(DataSet ds)
        {
            DataTable dt = ds.Tables["KHE_UOC"];
            lstDanhSachKUOC.Clear();
            if(dt !=null && dt.Rows.Count>0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DANH_SACH_KHE_UOC_THAY_DOI_LAI_SUAT objDSKUoc = new DANH_SACH_KHE_UOC_THAY_DOI_LAI_SUAT();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        PropertyInfo proper = objDSKUoc.GetType().GetProperty(dc.ColumnName);
                        if (proper != null)
                        {
                            if (proper.PropertyType == typeof(decimal))
                                proper.SetValue(objDSKUoc, Convert.ToDecimal(dr[dc.ColumnName]), null);
                            else if (proper.PropertyType == typeof(int))
                                proper.SetValue(objDSKUoc, Convert.ToInt32(dr[dc.ColumnName]), null);
                            else if (proper.PropertyType == typeof(string))
                                proper.SetValue(objDSKUoc, Convert.ToString(dr[dc.ColumnName]), null);
                        }

                    }
                    lstDanhSachKUOC.Add(objDSKUoc);
                }
                LoadDataGridData();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(dt.Rows[0]["TTHAI_NVU"].ToString());
                TThaiNV = dt.Rows[0]["TTHAI_NVU"].ToString();
                txtSoGiaoDich.Text = dt.Rows[0]["MA_TDOI_LSUAT"].ToString();
                iDGiaoDich = Convert.ToInt32(dt.Rows[0]["ID"]);
                teldtNgayGD.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_TDOI"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                CommonFunction.RefreshButton(Toolbar, action, TThaiNV, mnuMain, DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT);
            }
        }

        void LoadTabKiemSoat(DataSet ds)
        {
            DataTable dt = ds.Tables["KHE_UOC"];
            if (dt != null && dt.Rows.Count > 0)
            {
                txtNguoiLap.Text = dt.Rows[0]["NGUOI_NHAP"].ToString();
                txtNguoiCapNhat.Text = dt.Rows[0]["NGUOI_CNHAT"].ToString();
                teldtNgayNhap.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_NHAP"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                if (!dt.Rows[0]["NGAY_CNHAT"].ToString().IsNullOrEmptyOrSpace())
                    teldtNgayCNhat.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_CNHAT"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(dt.Rows[0]["TTHAI_BGHI"].ToString());
            }
        }

        void BeforeSave(BusinessConstant.TrangThaiNghiepVu nghiepvu, BusinessConstant.TrangThaiBanGhi bghi)
        {
            if (!nghiepvu.Equals(BusinessConstant.TrangThaiNghiepVu.LUU_TAM))
            {
                if (!VaditionData())
                    return;
            }
            List<int> lstId = new List<int>();
            lstId.Add(iDGiaoDich);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT,
            DatabaseConstant.Table.TD_TDOI_LSUAT,
            DatabaseConstant.Action.SUA,
            lstId);
            if (GetDataForm(nghiepvu, bghi))
                OnSave();
        }

        void OnSave()
        {
            List<ClientResponseDetail> lstResponse = new List<ClientResponseDetail>();
            int iret = 0;
            if (iDGiaoDich == 0)
                iret = new TinDungProcess().ThemDieuChinhLSuatTinDung(ref TDVMDCHINHLSUAT, ref lstResponse);
            else
                iret = new TinDungProcess().SuaDieuChinhLSuatTinDung(ref TDVMDCHINHLSUAT, ref lstResponse);
            AfterSave(lstResponse, iret);
        }

        void AfterSave(List<ClientResponseDetail> lstResponse, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponse);
            if (iret > 0)
            {
                action = DatabaseConstant.Action.XEM;
                List<int> lstId = new List<int>();
                lstId.Add(TDVMDCHINHLSUAT.ID_GIAO_DICH);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT,
                DatabaseConstant.Table.TD_TDOI_LSUAT,
                DatabaseConstant.Action.SUA,
                lstId);
                iDGiaoDich = TDVMDCHINHLSUAT.ID_GIAO_DICH;
                
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
                if ((bool)cbMultiAdd.IsChecked)
                    ClearForm();
                else
                    AfterAction();
            }
        }

        void BeforeDelete()
        {
            if (iDGiaoDich > 0)
            {
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT,
                DatabaseConstant.Table.TD_TDOI_LSUAT,
                DatabaseConstant.Action.XOA,
                lstId);
                OnDelete();
            }
        }

        void OnDelete()
        {
            if (iDGiaoDich > 0)
            {
                TDVMDCHINHLSUAT.ID_GIAO_DICH = iDGiaoDich;
                TDVMDCHINHLSUAT.MA_GIAO_DICH = txtSoGiaoDich.Text;
                TDVMDCHINHLSUAT.LST_ID_GIAO_DICH = new int[1] { iDGiaoDich };
                TDVMDCHINHLSUAT.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                TDVMDCHINHLSUAT.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                List<ClientResponseDetail> lstClientDetail = new List<ClientResponseDetail>();
                int iret = new TinDungProcess().XoaDieuChinhLSuatTinDung(ref TDVMDCHINHLSUAT, ref lstClientDetail);
                AfterDelete(lstClientDetail, iret);
            }
        }

        void AfterDelete(List<ClientResponseDetail> lstResponse, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponse);
            if (iret > 0)
            {
                List<int> lstId = new List<int>();
                lstId.Add(TDVMDCHINHLSUAT.ID_GIAO_DICH);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT,
                DatabaseConstant.Table.TD_TDOI_LSUAT,
                DatabaseConstant.Action.XOA,
                lstId);
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
                if (iret > 0)
                {
                    CommonFunction.CloseUserControl(this);
                }
                if ((bool)cbMultiAdd.IsChecked)
                    ClearForm();
            }
        }

        void BeforeApprove()
        {
            if (iDGiaoDich > 0)
            {
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT,
                DatabaseConstant.Table.TD_TDOI_LSUAT,
                DatabaseConstant.Action.DUYET,
                lstId);
                OnApprove();
            }
        }

        void OnApprove()
        {
            if (iDGiaoDich > 0)
            {
                TDVMDCHINHLSUAT.ID_GIAO_DICH = iDGiaoDich;
                TDVMDCHINHLSUAT.MA_GIAO_DICH = txtSoGiaoDich.Text;
                TDVMDCHINHLSUAT.LST_ID_GIAO_DICH = new int[1] { iDGiaoDich };
                TDVMDCHINHLSUAT.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                TDVMDCHINHLSUAT.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                TDVMDCHINHLSUAT.NGAY_GIAO_DICH = ClientInformation.NgayLamViecHienTai;
                List<ClientResponseDetail> lstClientDetail = new List<ClientResponseDetail>();
                int iret = new TinDungProcess().DuyetDieuChinhLSuatTinDung(ref TDVMDCHINHLSUAT, ref lstClientDetail);
                AfterApprove(lstClientDetail, iret);
            }
        }

        void AfterApprove(List<ClientResponseDetail> lstResponse, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponse);
            if (iret > 0)
            {
                action = DatabaseConstant.Action.XEM;
                List<int> lstId = new List<int>();
                lstId.Add(TDVMDCHINHLSUAT.ID_GIAO_DICH);
                TDVMDCHINHLSUAT.LST_ID_GIAO_DICH = lstId.ToArray();
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT,
                DatabaseConstant.Table.TD_TDOI_LSUAT,
                DatabaseConstant.Action.DUYET,
                lstId);
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
                if ((bool)cbMultiAdd.IsChecked)
                    ClearForm();
                else
                    AfterAction();
            }
        }

        void BeforeRefuse()
        {
            if (iDGiaoDich > 0)
            {
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT,
                DatabaseConstant.Table.TD_TDOI_LSUAT,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);
                OnRefuse();
            }
        }

        void OnRefuse()
        {
            if (iDGiaoDich > 0)
            {
                TDVMDCHINHLSUAT.ID_GIAO_DICH = iDGiaoDich;
                TDVMDCHINHLSUAT.MA_GIAO_DICH = txtSoGiaoDich.Text;
                TDVMDCHINHLSUAT.LST_ID_GIAO_DICH = new int[1] { iDGiaoDich };
                TDVMDCHINHLSUAT.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                TDVMDCHINHLSUAT.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                List<ClientResponseDetail> lstClientDetail = new List<ClientResponseDetail>();
                int iret = new TinDungProcess().TuChoiDieuChinhLSuatTinDung(ref TDVMDCHINHLSUAT, ref lstClientDetail);
                AfterRefuse(lstClientDetail, iret);
            }
        }

        void AfterRefuse(List<ClientResponseDetail> lstResponse, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponse);
            if (iret > 0)
            {
                action = DatabaseConstant.Action.XEM;
                List<int> lstId = new List<int>();
                lstId.Add(TDVMDCHINHLSUAT.ID_GIAO_DICH);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT,
                DatabaseConstant.Table.TD_TDOI_LSUAT,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
                if ((bool)cbMultiAdd.IsChecked)
                    ClearForm();
                else
                    AfterAction();
            }
        }

        void BeforeCancel()
        {
            if (iDGiaoDich > 0)
            {
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT,
                DatabaseConstant.Table.TD_TDOI_LSUAT,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                OnCancel();
            }
        }

        void OnCancel()
        {
            if (iDGiaoDich > 0)
            {
                TDVMDCHINHLSUAT.ID_GIAO_DICH = iDGiaoDich;
                TDVMDCHINHLSUAT.MA_GIAO_DICH = txtSoGiaoDich.Text;
                TDVMDCHINHLSUAT.LST_ID_GIAO_DICH = new int[1] { iDGiaoDich };
                TDVMDCHINHLSUAT.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                TDVMDCHINHLSUAT.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                List<ClientResponseDetail> lstClientDetail = new List<ClientResponseDetail>();
                int iret = new TinDungProcess().ThoaiDuyetDieuChinhLSuatTinDung(ref TDVMDCHINHLSUAT, ref lstClientDetail);
                AfterCancel(lstClientDetail, iret);
            }
        }

        void AfterCancel(List<ClientResponseDetail> lstResponse, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponse);
            if (iret > 0)
            {
                action = DatabaseConstant.Action.XEM;
                List<int> lstId = new List<int>();
                lstId.Add(TDVMDCHINHLSUAT.ID_GIAO_DICH);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT,
                DatabaseConstant.Table.TD_TDOI_LSUAT,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                LoadDataForm();
                CommonFunction.RefreshButton(Toolbar, action, TThaiNV, mnuMain, DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT);
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
                if ((bool)cbMultiAdd.IsChecked)
                    ClearForm();
                else
                    AfterAction();
            }
        }

        void OnModify()
        {
            if (action.Equals(DatabaseConstant.Action.SUA))
                EnableAllControl(true);
            else
                EnableAllControl(false);
            CommonFunction.RefreshButton(Toolbar, action, TThaiNV, mnuMain);
        }

        void EnableAllControl(bool bBool)
        {
            grbThongTinChung.IsEnabled = bBool;
            grbThongTinDieuChinh.IsEnabled = bBool;
            grbThongTinLSuat.IsEnabled = bBool;
            grdDieuChinhLSSanPham.IsReadOnly = !bBool;
        }

        void ReleaseForm()
        {
            if (iDGiaoDich > 0)
            {
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT,
                DatabaseConstant.Table.TD_TDOI_LSUAT,
                DatabaseConstant.Action.SUA,
                lstId);
            }
        }

        void AfterAction()
        {
            TThaiNV = TDVMDCHINHLSUAT.TRANG_THAI_NGHIEP_VU;
            lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNV);
            txtSoGiaoDich.Text = TDVMDCHINHLSUAT.MA_GIAO_DICH;
            txtNguoiCapNhat.Text = TDVMDCHINHLSUAT.NGUOI_CAP_NHAT;
            txtNguoiLap.Text = TDVMDCHINHLSUAT.NGUOI_LAP;
            teldtNgayNhap.Value = LDateTime.StringToDate(TDVMDCHINHLSUAT.NGAY_LAP, ApplicationConstant.defaultDateTimeFormat);
            txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(TDVMDCHINHLSUAT.TRANG_THAI_BAN_GHI);
            if(!TDVMDCHINHLSUAT.NGAY_CAP_NHAT.IsNullOrEmptyOrSpace())
                teldtNgayCNhat.Value = LDateTime.StringToDate(TDVMDCHINHLSUAT.NGAY_CAP_NHAT,ApplicationConstant.defaultDateTimeFormat);
            iDGiaoDich = TDVMDCHINHLSUAT.ID_GIAO_DICH;
            action = DatabaseConstant.Action.XEM;
            CommonFunction.RefreshButton(Toolbar, action, TThaiNV, mnuMain, DatabaseConstant.Function.TDVM_DIEU_CHINH_LAI_SUAT);
        }
        #endregion

        private void expendDSKheUoc_Expanded(object sender, RoutedEventArgs e)
        {
            expandThongTinTimKiem.IsExpanded = false;
        }

        private void expandThongTinTimKiem_Expanded(object sender, RoutedEventArgs e)
        {
            expendDSKheUoc.IsExpanded = false;
        }

        private void btnThemKH_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DM_CUM.getValue(), lstDieuKien);

                SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(true, simplePopupResponse, true);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = "Danh sách nhóm khách hàng";
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    foreach(DataRow dr in lstPopup)
                    {
                        DataRow drNhom = dtNhom.NewRow();
                        drNhom["ID_NHOM"] = dr[1].ToString();
                        drNhom["MA_NHOM"] = dr[2].ToString(); // ma nhom
                        drNhom["TEN_NHOM"] = dr[3].ToString(); // ten nhom
                        drNhom["ID_CUM"] = dr[5].ToString();
                        drNhom["MA_CUM"] = dr[6].ToString(); // ma nhom
                        drNhom["TEN_CUM"] = dr[7].ToString(); // ten nhom
                        dtNhom.Rows.Add(drNhom);
                    }
                    grdDanhSachKhachHang.ItemsSource = null;
                    grdDanhSachKhachHang.ItemsSource = dtNhom;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void btnXoaKH_Click(object sender, RoutedEventArgs e)
        {
            foreach (DataRow drNhom in grdDanhSachKhachHang.SelectedItems)
            {
                dtNhom.Rows.Remove(drNhom);
            }
            grdDanhSachKhachHang.ItemsSource = null;
            grdDanhSachKhachHang.ItemsSource = dtNhom;
        }

        private void btnHDTD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lstPopup = new List<DataRow>();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add("HDTDLAPKU");
                lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_HDTD", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(true, simplePopupResponse);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.Content = popup;
                win.ShowDialog();
                if (lstPopup.Count > 0)
                {
                    txtHDTD.Tag = lstPopup[0]["ID"];
                    txtHDTD.Text = lstPopup[0]["MA_HDTDVM"].ToString();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        private void btnSoKU_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add("0");
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add("KUOCTDLSUAT");
                lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
                lstDieuKien.Add("%");
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_KHEUOC", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopupKheUocViMo popup = new ucPopupKheUocViMo(true, simplePopupResponse, false);
                popup.LayGiaTriListDataRow = new ucPopupKheUocViMo.LayListDataRow(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Content = popup;
                win.ShowDialog();
                if (lstPopup.Count > 0)
                {
                    txtSoKU.Tag = lstPopup[0]["ID_KHE_UOC"];
                    txtSoKU.Text = lstPopup[0]["MA_KHE_UOC"].ToString();
                    lblTenKhachHang.Content = lstPopup[0]["TEN_KHACH_HANG"].ToString();
                }
            }
            catch (System.Exception ex)
            {
            	
            }
            finally
            {
            }
        }
    }
}
