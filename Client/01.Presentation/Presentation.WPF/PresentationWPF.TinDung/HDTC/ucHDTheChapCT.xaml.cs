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
using Presentation.Process.PopupServiceRef;
using Presentation.Process.Common;
using Presentation.Process.TaiSanDamBaoServiceRef;
using Telerik.Windows.Controls;

namespace PresentationWPF.TinDung.HDTC
{
    /// <summary>
    /// Interaction logic for ucHDTheChapCT.xaml
    /// </summary>
    public partial class ucHDTheChapCT : UserControl
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
        public static RoutedCommand SaveCommand = new RoutedCommand();
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
        private int idHDTC = 0;

        public int IdHDTC
        {
            get { return idHDTC; }
            set { idHDTC = value; }
        }
        private List<AutoCompleteEntry> lstLoaiHD = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstNoiCap = new List<AutoCompleteEntry>();
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        int idKhachHang=0;
        string maKhachHang="";
        private string tthaiNvu = "";
        public string TthaiNvu
        {
            get { return tthaiNvu; }
            set { tthaiNvu = value; }
        }
        private bool bSua = false;

        public bool BSua
        {
            get { return bSua; }
            set { bSua = value; }
        }

        public event EventHandler OnSavingCompleted;
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucHDTheChapCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDung;component/HDTC/ucHDTheChapCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            KhoiTaoGiaTriComboBox();
            InitEventHandler();
        }

        void KhoiTaoGiaTriComboBox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.HINH_THUC_THE_CHAP.getValue());
            auto.GenAutoComboBox(ref lstLoaiHD, ref cmbLoaiHD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
            lstNoiCap.Add(new AutoCompleteEntry("","","0"));
            auto.GenAutoComboBox(ref lstNoiCap, ref cmbNoiCap, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TINHTP.getValue());
        }

        private void InitEventHandler()
        {
            txtMaKhachHang.LostFocus += new RoutedEventHandler(txtMaKhachHang_LostFocus);
            this.Unloaded += new RoutedEventHandler(ucHDTheChapCT_Unloaded);
        }

        void ucHDTheChapCT_Unloaded(object sender, RoutedEventArgs e)
        {
            Release();
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
                        key = new KeyBinding(SaveCommand, keyg);
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
        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
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
            MessageBox.Show("Trợ giúp");
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                beforeModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                beforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            {
 
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                beforeSave(BusinessConstant.TrangThaiBanGhi.SU_DUNG, BusinessConstant.TrangThaiNghiepVu.LUU_TAM);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                beforeSave(BusinessConstant.TrangThaiBanGhi.SU_DUNG, BusinessConstant.TrangThaiNghiepVu.CHO_DUYET);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
            { }
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
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                Release();
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                beforeModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                beforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                beforeSave(BusinessConstant.TrangThaiBanGhi.SU_DUNG, BusinessConstant.TrangThaiNghiepVu.LUU_TAM);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                beforeSave(BusinessConstant.TrangThaiBanGhi.SU_DUNG, BusinessConstant.TrangThaiNghiepVu.CHO_DUYET);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
            { }
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
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                Release();
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }
        #endregion

        /// <summary>
        /// Xu ly giao dien
        /// </summary>
        #region Xu ly giao dien

        /// <summary>
        /// Sự kiện nhấn key trên form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
                Release();
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

        /// <summary>
        /// Hiện popup khách hàng
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMaKhachHang_Click(object sender, RoutedEventArgs e)
        {
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add("NULL");
            lstDieuKien.Add("NULL");
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_KHACHHANG.getValue(), lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            ucPopup popup = new ucPopup(true, simplePopupResponse);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.Content = popup;
            win.ShowDialog();
            if (lstPopup.Count > 0)
            {
                idKhachHang = Convert.ToInt32(lstPopup[0]["ID"]);
                getThongTinKhachHang(idKhachHang, "");
            }
        }

        void ResetForm()
        {
            idKhachHang=0;
            maKhachHang="";
            txtMoTa.Text = "";
            teldtNgayHopDong.Value = LDateTime.GetCurrentDate();
            cmbLoaiHD.SelectedIndex = 0;
            txtSoHD.Text = "";
            txtMaKhachHang.Text = "";
            txtTenKhachHang.Text = "";
            txtCMT.Text = "";
            teldtNgayCap.Value = null;
            cmbNoiCap.SelectedIndex = 0;
            txtDiaChi.Text = "";
            raddgrTSDamBaoDS.ItemsSource = null;
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, "");
        }

        void txtMaKhachHang_LostFocus(object sender, RoutedEventArgs e)
        {
            if (maKhachHang.Equals(txtMaKhachHang.Text))
                return;
            if (!getThongTinKhachHang(0, txtMaKhachHang.Text))
            {
                txtMaKhachHang.Text = "";
                txtTenKhachHang.Text = "";
                txtCMT.Text = "";
                teldtNgayCap.Value = null;
                cmbNoiCap.SelectedIndex = 0;
                txtDiaChi.Text = "";
            }
        }

        #endregion

        /// <summary>
        /// Xu ly nghiep vu
        /// </summary>
        #region Xu ly nghiep vu
        /// <summary>
        /// Kiểm tra dữ liệu
        /// </summary>
        bool Vadidation()
        {
            bool bResult = true;
            if (txtMaKhachHang.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblMaKhachHang.Content.ToString());
                txtMaKhachHang.Focus();
                bResult = false;
            }
            else if (teldtNgayHopDong.Value.IsNullOrEmpty())
            {
                CommonFunction.ThongBaoTrong(lblNgayHopDong.Content.ToString());
                teldtNgayHopDong.Focus();
                bResult = false;
            }
            else if (raddgrTSDamBaoDS.SelectedItems.Count < 1)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                raddgrTSDamBaoDS.Focus();
                bResult = false;
            }
            return bResult;
        }

        /// <summary>
        /// Lấy thông tin khách hàng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="maKHang"></param>
        /// <returns></returns>
        bool getThongTinKhachHang(int id, string maKHang)
        {
            bool bResutl = true;
            try
            {
                DataSet ds = new KhachHangProcess().getThongTinCoBanKHTheoMa(id, maKHang, 0);
                if (ds.Tables[0].Rows.Count == 1)
                {
                    idKhachHang = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]);
                    txtMaKhachHang.Text = maKhachHang = ds.Tables[0].Rows[0]["MA_KHANG"].ToString();
                    txtTenKhachHang.Text = ds.Tables[0].Rows[0]["TEN_KHANG"].ToString();
                    txtCMT.Text = ds.Tables[0].Rows[0]["DD_GTLQ_SO"].ToString();
                    if (ds.Tables[0].Rows[0]["DD_GTLQ_NGAY_CAP"].ToString().IsDate(ApplicationConstant.defaultDateTimeFormat))
                        teldtNgayCap.Value = LDateTime.StringToDate(ds.Tables[0].Rows[0]["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd");
                    else
                        teldtNgayCap.Value = null;
                    if (!ds.Tables[0].Rows[0]["DD_GTLQ_NOI_CAP"].ToString().IsNullOrEmptyOrSpace())
                        cmbNoiCap.SelectedIndex = lstNoiCap.IndexOf(lstNoiCap.FirstOrDefault(i => i.KeywordStrings.First().Equals(ds.Tables[0].Rows[0]["DD_GTLQ_NOI_CAP"])));
                    else
                        cmbNoiCap.SelectedIndex = 0;
                    txtDiaChi.Text = ds.Tables[0].Rows[0]["DIA_CHI"].ToString();
                    getDanhSachTaiSanDamBao(idKhachHang, 0);
                }
                else
                {
                    if (!maKhachHang.Equals(txtMaKhachHang.Text))
                    {
                        idKhachHang = 0;
                        raddgrTSDamBaoDS.ItemsSource = null;
                        bResutl = false;
                    }
                }

            }
            catch (Exception ex)
            {
                bResutl = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
            }
            return bResutl;
        }

        /// <summary>
        /// Lấy thông tin tài sản đảm bảo
        /// </summary>
        /// <param name="idKHang"></param>
        /// <param name="idHDTC"></param>
        void getDanhSachTaiSanDamBao(int idKHang, int idHDTC)
        {
            TaiSanDamBaoProcess tsdbProcess = new TaiSanDamBaoProcess();
            try
            {
                DataSet dsDSTSDB = tsdbProcess.getThongTinTSDBCTbyKhachHangorHopDong(idKHang.ToString(), idHDTC.ToString());
                if (dsDSTSDB.Tables.Count > 0)
                {
                    raddgrTSDamBaoDS.ItemsSource = null;
                    raddgrTSDamBaoDS.ItemsSource = dsDSTSDB.Tables[0];
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
            }
        }

        public void SetDataForm()
        {
            try
            {
                DataSet ds = new TaiSanDamBaoProcess().getThongTinHDTC(idHDTC.ToString());
                if (ds != null)
                {
                    SetTabThongTinChung(ds);
                    SetTabThongTinKiemSoat(ds);
                    afterLoadForm();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
            }
        }

        private void SetTabThongTinChung(DataSet ds)
        {
            try
            {
                DataTable dataTable = ds.Tables["INQ.CT.TD_HDTC.TTIN_CTIET"];
                DataTable dataTSDB = ds.Tables["INQ.CT.TD_HDTC.TSDB"];
                raddgrTSDamBaoDS.ItemsSource = null;
                raddgrTSDamBaoDS.ItemsSource = dataTSDB;
                txtSoHD.Text = dataTable.Rows[0]["MA_HDTC"].ToString();
                teldtNgayHopDong.Value = LDateTime.StringToDate(dataTable.Rows[0]["NGAY_HDTC"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                cmbLoaiHD.SelectedIndex = lstLoaiHD.IndexOf(lstLoaiHD.FirstOrDefault(i => i.KeywordStrings.First().Equals(dataTable.Rows[0]["LOAI_HDTC"].ToString())));
                txtMaKhachHang.Text = maKhachHang = dataTable.Rows[0]["MA_KHANG"].ToString();
                idKhachHang = Convert.ToInt32(dataTable.Rows[0]["ID_KHANG"].ToString());
                txtTenKhachHang.Text = dataTable.Rows[0]["TEN_KHANG"].ToString();
                txtDiaChi.Text = dataTable.Rows[0]["DIA_CHI"].ToString();
                txtCMT.Text = dataTable.Rows[0]["DD_GTLQ_SO"].ToString();
                if (LDateTime.IsDate(dataTable.Rows[0]["DD_GTLQ_SO"].ToString(), ApplicationConstant.defaultDateTimeFormat))
                    teldtNgayCap.Value = LDateTime.StringToDate(dataTable.Rows[0]["DD_GTLQ_SO"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                cmbNoiCap.SelectedIndex = lstNoiCap.IndexOf(lstNoiCap.FirstOrDefault(i => i.KeywordStrings.First().Equals(dataTable.Rows[0]["DD_GTLQ_NOI_CAP"].ToString())));
                txtMoTa.Text = dataTable.Rows[0]["MO_TA"].ToString();
                lblTrangThai.Content = BusinessConstant.layNgonNgu(dataTable.Rows[0]["tthai_nvu"].ToString());
                tthaiNvu = dataTable.Rows[0]["tthai_nvu"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetTabThongTinKiemSoat(DataSet ds)
        {
            try
            {
                DataTable dataTable = ds.Tables["INQ.CT.TD_HDTC.TTIN_CTIET"];
                txtNguoiLap.Text = dataTable.Rows[0]["NGUOI_NHAP"].ToString();
                txtNguoiCapNhat.Text = dataTable.Rows[0]["NGUOI_CNHAT"].ToString();
                txtTrangThai.Text = BusinessConstant.layNgonNgu(dataTable.Rows[0]["TTHAI_BGHI"].ToString());
                if (LDateTime.IsDate(dataTable.Rows[0]["NGAY_NHAP"].ToString(), ApplicationConstant.defaultDateTimeFormat))
                    teldtNgayNhap.Value = LDateTime.StringToDate(dataTable.Rows[0]["NGAY_NHAP"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                if (LDateTime.IsDate(dataTable.Rows[0]["NGAY_CNHAT"].ToString(), ApplicationConstant.defaultDateTimeFormat))
                    teldtNgayCNhat.Value = LDateTime.StringToDate(dataTable.Rows[0]["NGAY_CNHAT"].ToString(), ApplicationConstant.defaultDateTimeFormat);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void afterLoadForm()
        {
            if (bSua)
            {
                beforeModify();
            }
            else
            {
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, TthaiNvu);
                SetEnabledAllControls(bSua);
            }
        }

        void GetDataForm(ref TD_HDTC objHDTC,ref List<int> lstIDTSDB, BusinessConstant.TrangThaiBanGhi bghi, BusinessConstant.TrangThaiNghiepVu nghiepvu)
        {
            objHDTC.ID_KHANG = idKhachHang;
            objHDTC.MA_KHANG = maKhachHang;
            objHDTC.MO_TA = txtMoTa.Text;
            objHDTC.NGAY_HDTC = LDateTime.DateToString((DateTime)teldtNgayHopDong.Value, ApplicationConstant.defaultDateTimeFormat);
            objHDTC.LOAI_HDTC = lstLoaiHD.ElementAt(cmbLoaiHD.SelectedIndex).KeywordStrings.First();
            decimal dTongGiaTri=0;
            foreach (DataRow dr in raddgrTSDamBaoDS.SelectedItems)
            {
                dTongGiaTri += Convert.ToDecimal(dr["GTRI_TOI_DA"]);
                lstIDTSDB.Add(Convert.ToInt32(dr["ID"]));
            }
            objHDTC.TONG_GIA_TRI = dTongGiaTri;
            objHDTC.TTHAI_BGHI = BusinessConstant.layGiaTri(bghi);
            objHDTC.TTHAI_NVU = BusinessConstant.layGiaTri(nghiepvu);
            objHDTC.MA_DVI_QLY = ClientInformation.MaDonVi;
            objHDTC.MA_DVI_TAO = ClientInformation.MaDonVi;
            objHDTC.NGAY_NHAP = LDateTime.DateToString(LDateTime.GetCurrentDate(), ApplicationConstant.defaultDateTimeFormat);
            objHDTC.NGUOI_NHAP = ClientInformation.TenDangNhap;
            if (idHDTC > 0)
            {
                objHDTC.NGAY_CNHAT = LDateTime.DateToString(LDateTime.GetCurrentDate(), ApplicationConstant.defaultDateTimeFormat);
                objHDTC.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                objHDTC.ID = idHDTC;
                objHDTC.MA_HDTC = txtSoHD.Text;
                objHDTC.NGAY_NHAP = LDateTime.DateToString((DateTime)teldtNgayNhap.Value, ApplicationConstant.defaultDateTimeFormat);
                objHDTC.NGUOI_NHAP = txtNguoiLap.Text;
            }
        }
        
        void beforeSave(BusinessConstant.TrangThaiBanGhi banghi, BusinessConstant.TrangThaiNghiepVu nghiepvu)
        {
            if (!nghiepvu.Equals(BusinessConstant.TrangThaiNghiepVu.LUU_TAM))
            {
                if (!Vadidation())
                    return;
            }
            TD_HDTC objHDTC = new TD_HDTC();
            List<int> lstIDTSDB = new List<int>();
            GetDataForm(ref objHDTC, ref lstIDTSDB, banghi, nghiepvu);
            onSave(ref objHDTC, lstIDTSDB);
        }

        void onSave(ref TD_HDTC objHDTC, List<int> lstIDTSDB)
        {
            try
            {
                ApplicationConstant.ResponseStatus aResult;
                if (idHDTC == 0)
                    aResult = new TaiSanDamBaoProcess().ThemHopDongTheChap(ref objHDTC, lstIDTSDB);
                else
                    aResult = new TaiSanDamBaoProcess().SuaHopDongTheChap(ref objHDTC, lstIDTSDB);
                if (aResult.Equals(ApplicationConstant.ResponseStatus.THANH_CONG))
                {
                    if (idHDTC == 0)
                        afterAddNew(objHDTC);
                    else
                        afterModify(objHDTC);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
        
        void afterAddNew(TD_HDTC objHDTC)
        {
            if (objHDTC != null)
            {
                LMessage.ShowMessage("M.ResponseMessage.Common.ThemThanhCong", LMessage.MessageBoxType.Information);
                idHDTC = objHDTC.ID;
                TthaiNvu = objHDTC.TTHAI_NVU;
                lblTrangThai.Content = BusinessConstant.layNgonNgu(objHDTC.TTHAI_NVU);
                txtTrangThai.Text = BusinessConstant.layNgonNgu(objHDTC.TTHAI_BGHI);
                txtSoHD.Text = objHDTC.MA_HDTC;
                SetEnabledAllControls(false);
                titemThongTinChung.Focus();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, TthaiNvu);
                if (cbMultiAdd.IsChecked == true)
                {
                        ResetForm();
                }
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
            }
            else
            {
                LMessage.ShowMessage("M.ResponseMessage.Common.ThemKhongThanhCong", LMessage.MessageBoxType.Error);
            }
        }

        void beforeModify()
        {
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(idHDTC);
            bool bLocked = false;
            bLocked = process.LockData(DatabaseConstant.Module.TIN_DUNG_VI_MO,
                DatabaseConstant.Function.TD_CHI_TIET_HOP_DONG_THE_CHAP,
                DatabaseConstant.Table.TD_HDTC,
                DatabaseConstant.Action.SUA,
                listLockId);
            if (bLocked)
            {
                bSua = true;
                SetEnabledAllControls(bSua);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, TthaiNvu);
            }
        }

        void afterModify(TD_HDTC objHDTC)
        {
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(idHDTC);
            bool bLocked = false;
            bLocked = process.UnlockData(DatabaseConstant.Module.TIN_DUNG_VI_MO,
                DatabaseConstant.Function.TD_CHI_TIET_HOP_DONG_THE_CHAP,
                DatabaseConstant.Table.TD_HDTC,
                DatabaseConstant.Action.SUA,
                listLockId);
            if (objHDTC != null & bLocked)
            {
                LMessage.ShowMessage("M.ResponseMessage.Common.SuaThanhCong", LMessage.MessageBoxType.Information);
                TthaiNvu = objHDTC.TTHAI_NVU;
                lblTrangThai.Content = BusinessConstant.layNgonNgu(objHDTC.TTHAI_NVU);
                txtTrangThai.Text = BusinessConstant.layNgonNgu(objHDTC.TTHAI_BGHI);
                txtSoHD.Text = objHDTC.MA_HDTC;
                SetEnabledAllControls(false);
                titemThongTinChung.Focus();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, TthaiNvu);
                if (cbMultiAdd.IsChecked == true)
                {
                    ResetForm();
                }
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
            }
            else
            {
                LMessage.ShowMessage("M.ResponseMessage.Common.SuaKhongThanhCong", LMessage.MessageBoxType.Error);
            }
        }

        void beforeDelete()
        {
            if (!tlbDelete.IsEnabled)
                return;
            if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            List<int> lstId = new List<int>();
            lstId.Add(idHDTC);
            // Yêu cầu lock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.LockData(DatabaseConstant.Module.TIN_DUNG_VI_MO,
                    DatabaseConstant.Function.TD_CHI_TIET_HOP_DONG_THE_CHAP,
                    DatabaseConstant.Table.TD_HDTC,
                    DatabaseConstant.Action.XOA,
                    lstId);
             //Bắt đầu xóa dữ liệu
             if (retLockData)
                    onDelete(lstId);
        }

        void onDelete(List<int> lstId)
        {
            List<ClientResponseDetail> responseDetail = new List<ClientResponseDetail>();
            new TaiSanDamBaoProcess().XoaHopDongTheChap(lstId, ref responseDetail);
            afterDelete(responseDetail, lstId);
        }

        void afterDelete(List<ClientResponseDetail> responseDetail, List<int> lstId)
        {
            CommonFunction.ThongBaoKetQua(responseDetail);
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TIN_DUNG_VI_MO,
                DatabaseConstant.Function.TD_CHI_TIET_HOP_DONG_THE_CHAP,
                DatabaseConstant.Table.TD_HDTC,
                DatabaseConstant.Action.XOA,
                lstId);
            CommonFunction.CloseUserControl(this);
        }

        void beforeApprove()
        {
            if (!tlbApprove.IsEnabled)
                return;
            if (LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            List<int> lstId = new List<int>();
            lstId.Add(idHDTC);
                // Yêu cầu lock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retLockData = process.LockData(DatabaseConstant.Module.TIN_DUNG_VI_MO,
                    DatabaseConstant.Function.TD_CHI_TIET_HOP_DONG_THE_CHAP,
                    DatabaseConstant.Table.TD_HDTC,
                    DatabaseConstant.Action.DUYET,
                    lstId);
                //Bắt đầu duyệt dữ liệu
                if (retLockData)
                    onApprove(lstId);
        }

        void onApprove(List<int> lstId)
        {
            List<ClientResponseDetail> responseDetail = new List<ClientResponseDetail>();
            new TaiSanDamBaoProcess().DuyetHopDongTheChap(lstId, ref responseDetail);
            afterApprove(responseDetail, lstId);
        }

        void afterApprove(List<ClientResponseDetail> responseDetail, List<int> lstId)
        {
            // Yêu cầu lock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            CommonFunction.ThongBaoKetQua(responseDetail);
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TIN_DUNG_VI_MO,
                DatabaseConstant.Function.TD_CHI_TIET_HOP_DONG_THE_CHAP,
                DatabaseConstant.Table.TD_HDTC,
                DatabaseConstant.Action.DUYET,
                lstId);
            bSua = false;
            SetDataForm();
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu);
        }

        void beforeRefuse()
        {
            if (!tlbRefuse.IsEnabled)
                return;
            if (LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            List<int> lstId = new List<int>();
            lstId.Add(idHDTC);
                // Yêu cầu lock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retLockData = process.LockData(DatabaseConstant.Module.TIN_DUNG_VI_MO,
                    DatabaseConstant.Function.TD_CHI_TIET_HOP_DONG_THE_CHAP,
                    DatabaseConstant.Table.TD_HDTC,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    lstId);
                //Bắt đầu thoái duyệt dữ liệu
                if (retLockData)
                    onRefuse(lstId);
        }

        void onRefuse(List<int> lstId)
        {
            List<ClientResponseDetail> responseDetail = new List<ClientResponseDetail>();
            new TaiSanDamBaoProcess().TuChoiHopDongTheChap(lstId, ref responseDetail);
            afterRefuse(responseDetail, lstId);
        }

        void afterRefuse(List<ClientResponseDetail> responseDetail, List<int> lstId)
        {
            CommonFunction.ThongBaoKetQua(responseDetail);
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TIN_DUNG_VI_MO,
                DatabaseConstant.Function.TD_CHI_TIET_HOP_DONG_THE_CHAP,
                DatabaseConstant.Table.TD_HDTC,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);
            
            bSua = false;
            SetDataForm();
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu);
        }

        void beforeCancel()
        {
            if (!tlbCancel.IsEnabled)
                return;
            if (LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            List<int> lstId = new List<int>();
            lstId.Add(idHDTC);
                // Yêu cầu lock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TIN_DUNG_VI_MO,
                    DatabaseConstant.Function.TD_CHI_TIET_HOP_DONG_THE_CHAP,
                    DatabaseConstant.Table.TD_HDTC,
                    DatabaseConstant.Action.THOAI_DUYET,
                    lstId);
                //Bắt đầu từ chối duyệt dữ liệu
                if (retLockData)
                    onCancel(lstId);
            
        }
        void onCancel(List<int> lstId)
        {
            List<ClientResponseDetail> responseDetail = new List<ClientResponseDetail>();
            new TaiSanDamBaoProcess().ThoaiDuyetHopDongTheChap(lstId, ref responseDetail);
            afterCancel(responseDetail, lstId);
        }

        void afterCancel(List<ClientResponseDetail> responseDetail, List<int> lstId)
        {
            CommonFunction.ThongBaoKetQua(responseDetail);
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retLockData = process.UnlockData(DatabaseConstant.Module.TIN_DUNG_VI_MO,
                DatabaseConstant.Function.TD_CHI_TIET_HOP_DONG_THE_CHAP,
                DatabaseConstant.Table.TD_HDTC,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
            bSua = false;
            SetDataForm();
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu);
        }

        private void SetEnabledAllControls(bool enable)
        {
            grbThongTinChung.IsEnabled = enable;
            grbVongVonDS.IsEnabled = enable;
        }
        private void Release()
        {
            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(idHDTC);

            bool ret = process.UnlockData(DatabaseConstant.Module.TIN_DUNG_VI_MO,
                DatabaseConstant.Function.TD_CHI_TIET_HOP_DONG_THE_CHAP,
                DatabaseConstant.Table.TD_HDTC,
                DatabaseConstant.Action.SUA,
                listLockId);
        }
        #endregion
    }
}
