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
using Utilities.Common;
using PresentationWPF.CustomControl;
using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using System.Data;
using PresentationWPF.KhachHang.KhachHang.Popup;
using System.Reflection;
using Telerik.Windows.Controls;
using Presentation.Process.KhachHangServiceRef;
using System.Collections;
using PresentationWPF.BaoCao.DungChung;

namespace PresentationWPF.KhachHang.KhachHang
{
    /// <summary>
    /// Interaction logic for ucKhachHangThanhVienBinhKhanh.xaml
    /// </summary>
    public partial class ucKhachHangThanhVienBinhKhanh : UserControl
    {
        #region Khai bao
        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        public event EventHandler OnSavingCompleted;

        public static string formCase = null;

        //Source cac combobox phan thong tin chung
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceGioiTinh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNoiCap = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLyDoRa = new List<AutoCompleteEntry>();

        //DataSet chứa thông tin của các thông tin liên quan tới khách hàng nhập thông qua popup, grid
        private DataSet dsSourceKHang = new DataSet();

        private string gioi_tinh = DatabaseConstant.DanhMuc.GIOI_TINH.getValue();
       
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        private int _idKhachHang = -1;
        public int idKhachHang
        {
            get { return _idKhachHang; }
            set { _idKhachHang = value; }
        }

        private string tthaiNvu = "";
        public string TthaiNvu
        {
            get { return tthaiNvu; }
            set { tthaiNvu = value; }
        }

        private string _maKhachHang = "";

        public string MaKhachHang
        {
            get { return _maKhachHang; }
            set { _maKhachHang = value; }
        }

        private DatabaseConstant.Function _function = DatabaseConstant.Function.KH_THANH_VIEN;
        public DatabaseConstant.Function function
        {
            get { return _function; }
        }

        // Thông tin phòng gd, khu vực, cụm, nhóm
        private string idPhongGD = "";
        private string idKhuVuc = "";
        private string idCum = "";
        private string idNhom = "";

        // Kiem tra radio button tren grid
        private bool rdoChecked = false;

        #endregion

        #region Khoi tao
        public ucKhachHangThanhVienBinhKhanh()
        {
            InitializeComponent();
            KhoiTaoChung();

            txtMaKhachHang.Focus();
            raddtNgayCNhat.Value = null;
            raddtNgayNhap.Value = null;
            raddtNgayCNhat.Value = null;
            raddtNgaySinh.Value = null;
            raddtNgayCap.Value = null;
            txtNguoiLap.Text = Presentation.Process.Common.ClientInformation.TenDangNhap;

            KhoiTaoDataTable();
            beforeAddNew();
        }

        public ucKhachHangThanhVienBinhKhanh(int id,string tthai,DatabaseConstant.Action action)
        {
            InitializeComponent();
            KhoiTaoChung();

            _idKhachHang = id;
            tthaiNvu = tthai;            

            txtMaKhachHang.Focus();
            raddtNgayCNhat.Value = null;
            txtNguoiLap.Text = Presentation.Process.Common.ClientInformation.TenDangNhap;
            SetFormData();
            beforeModifyFromList(action);
        }

        private void KhoiTaoChung()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KhachHang;component/KhachHang/BinhKhanh/ucKhachHangThanhVienBinhKhanh.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            raddtNgayHetHL.Value = null;
            stackLyDoRa.Visibility = Visibility.Hidden;
            cmbLyDoRaKhoiNhom.Visibility = Visibility.Hidden;
            chkKHHieuLuc.Checked += new RoutedEventHandler(chkKHHieuLuc_Checked);
            chkKHHieuLuc.Unchecked += new RoutedEventHandler(chkKHHieuLuc_Unchecked);
            BindShortkey();
            LoadCombobox();
            ResetForm();
        }

        /// <summary>
        /// Khoi tao datatable luu source cac popup
        /// </summary>
        private void KhoiTaoDataTable()
        {
            Presentation.Process.KhachHangProcess process = new Presentation.Process.KhachHangProcess();
            dsSourceKHang = process.getThongTinKhac("%");
        }
        #endregion

        #region Dang ky hot key, shortcut key

        private void BindShortkey()
        {
            foreach (var child in Toolbar.Children)
            {
                if (child.GetType() == typeof(RibbonButton))
                {
                    RibbonButton tlb = (RibbonButton)child;
                    KeyBinding key = new KeyBinding();
                    string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.H, ModifierKeys.Control);
                        key = new KeyBinding(HoldCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(SaveCommand, keyg);
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(HelpCommand, keyg);
                    }
                    if (key != null)
                    {
                        InputBindings.Add(key);
                    }
                }
            }
        }

        private void HoldCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbHold.IsEnabled;
        }
        private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onHold();
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbSubmit.IsEnabled;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onSave();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbModify.IsEnabled;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeModifyFromDetail();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbDelete.IsEnabled;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeDelete();
        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbApprove.IsEnabled;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onApprove();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbRefuse.IsEnabled;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onRefuse();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbCancel.IsEnabled;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onCancel();
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
            onClose();
        }

        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            // Truongnx
            string strTinhNang = "";
            if (sender is RibbonButton)
            {
                RibbonButton tlb = (RibbonButton)sender;
                strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            }
            else if (sender is RibbonMenuItem)
            {
                RibbonMenuItem tlb = (RibbonMenuItem)sender;
                strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            }
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                onHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                onSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                beforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                beforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                beforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                beforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                beforeRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                onClose();
            }
            
        }

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                onHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                onSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                beforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                beforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                beforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                beforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                beforeRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                onClose();
            }
        }

        #endregion

        #region Xu ly Giao dien
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
        /// Sự kiện load 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_idKhachHang == -1)
                {
                    formCase = Presentation.Process.Common.ClientInformation.FormCase;
                    if (LString.IsNullOrEmptyOrSpace(formCase))
                    {
                        formCase = ApplicationConstant.LoaiKhachHang.THANH_VIEN.layGiaTri();
                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Khởi tạo các combobox
        /// </summary>
        private void LoadCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDK = new List<string>();

            //Load du lieu combobox GioiTinh
            lstDK.Add(gioi_tinh);
            auto.GenAutoComboBox(ref lstSourceGioiTinh, ref cmbGioiTinh, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK);

            //Load du lieu combobox NoiCap ~ TinhThanh
            auto.GenAutoComboBox(ref lstSourceNoiCap, ref cmbNoiCap, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_TINHTP), null);

            // Load du lieu Phong GD
            lstDK.Clear();
            lstDK.Add(ClientInformation.TenDangNhap);
            lstDK.Add(ClientInformation.MaDonViQuanLy);
            lstDK.Add(DatabaseConstant.ToChucDonVi.PGD.getValue());
            auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVI_THEOPVI.getValue(), lstDK, ClientInformation.MaDonViGiaoDich);
            cmbPhongGD.IsEnabled = false;

            //Load ly do ra khoi nhom
            lstDK.Clear();
            lstDK.Add(DatabaseConstant.DanhMuc.LY_DO_VAO_RA.getValue());
            auto.GenAutoComboBox(ref lstSourceLyDoRa, ref cmbLyDoRaKhoiNhom, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK, null);
        }

        /// <summary>
        /// Sự kiện click button hiển thị popup khách hàng chưa phải là thành viên
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMaKhachHang_Click(object sender, RoutedEventArgs e)
        {
            HienPopUpKhachHang();
        }

        /// <summary>
        /// Hiện popup khách hàng chưa phải là thành viên
        /// </summary>
        private void HienPopUpKhachHang()
        {
            Window window = new Window();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.RenderSize = new Size(1024, 768);
            ucPopupKhachHang uc = new ucPopupKhachHang(false);
            window.Title = LLanguage.SearchResourceByKey("U.POPUP.POPUP_DS_KHACHHANG");
            window.Content = uc;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
            if (!LString.IsNullOrEmptyOrSpace(uc.id))
            {
                _idKhachHang = Convert.ToInt32(uc.id);
                SetFormData();
            }
            uc = null;
        }

        /// <summary>
        /// Hiện popup khu vực/cụm/nhóm
        /// </summary>
        private void HienPopUpCum()
        {
            try
            {
                var process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_CUM.getValue(), lstDieuKien);

                SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(true, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Title = LLanguage.SearchResourceByKey("U.POPUP.POPUP_DM_CUM");
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow dr = lstPopup[0];
                    txtMaCum.Text = dr[2].ToString(); //Mã cụm
                    lblTenCum.Content = dr[3].ToString(); //Tên cụm
                    txtMaCum.Tag = dr[1].ToString(); //ID cụm
                    idCum = dr[1].ToString();
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Nhấn nút "F3" hiển để hiển thị popup khu vực/cụm/nhóm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNhomTVien_Click(object sender, RoutedEventArgs e)
        {
            HienPopUpCum();
        }

        /// <summary>
        /// Chuyển giá trị vào datatable source
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="dtAdd"></param>
        private void ImportRows(ref DataTable dtSource, DataTable dtAdd)
        {
            DataRow[] drRemove = dtSource.Select("CHON=1");
            if (drRemove.Length > 0)
            {
                foreach (DataRow dr in drRemove)
                {
                    dtSource.Rows.Remove(dr);
                }
            }

            foreach (DataRow dr in dtAdd.Rows)
            {
                dtSource.ImportRow(dr);
            }

            //Set lại số STT
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                dtSource.Rows[i]["STT"] = i + 1;
                dtSource.Rows[i]["CHON"] = false;
            }
        }

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void onHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }

        /// <summary>
        /// Đóng form
        /// </summary>
        private void onClose()
        {
            Release();
            if (OnSavingCompleted != null)
            {
                OnSavingCompleted(this, EventArgs.Empty);
            }
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        /// <summary>
        /// Hàm xử lý giải phóng lock dữ liệu cho form
        /// </summary>
        private void Release()
        {
            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idKhachHang);

            bool ret = process.UnlockData(DatabaseConstant.Module.KHTV,
                _function,
                DatabaseConstant.Table.KH_KHANG_HSO,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        void txtNhomTVien_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
                btnNhomTVien_Click(sender, null);
        }

        void chkKHHieuLuc_Unchecked(object sender, RoutedEventArgs e)
        {
            raddtNgayHetHL.Value = null;
            stackLyDoRa.Visibility = System.Windows.Visibility.Hidden;
            cmbLyDoRaKhoiNhom.Visibility = Visibility.Hidden;
        }

        void chkKHHieuLuc_Checked(object sender, RoutedEventArgs e)
        {
            raddtNgayHetHL.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            stackLyDoRa.Visibility = System.Windows.Visibility.Visible;
            cmbLyDoRaKhoiNhom.Visibility = Visibility.Visible;
        }

        #endregion

        #region Xu ly nghiep vu

        private void beforeView()
        {
            SetFormData();
            SetEnabledAllControls(false);
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KH_THANH_VIEN);
        }

        private void beforeAddNew()
        {
            //XuLyGiaoDien("CNHAN");
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KH_THANH_VIEN);
        }

        private void beforeModifyFromList(DatabaseConstant.Action action)
        {
            if (action == DatabaseConstant.Action.XEM)
            {
                SetEnabledAllControls(false);
            }
            CommonFunction.RefreshButton(Toolbar, action, tthaiNvu, mnuMain, DatabaseConstant.Function.KH_THANH_VIEN);
        }

        private void beforeModifyFromDetail()
        {
            // Yêu cầu lock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idKhachHang);

            bool ret = process.LockData(DatabaseConstant.Module.KHTV,
                _function,
                DatabaseConstant.Table.KH_KHANG_HSO,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Nếu lock thành công >> cho phép sửa
            if (ret)
            {
                SetFormData();
                SetEnabledAllControls(true);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu, mnuMain, DatabaseConstant.Function.KH_THANH_VIEN);
            }
            else
            {
                LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
            }
        }

        private void beforeDelete()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_idKhachHang);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.KHTV,
                    _function,
                    DatabaseConstant.Table.KH_KHANG_HSO,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    // Gọi tới hàm xóa dữ liệu
                    onDelete();
                    return;
                }
                // Nếu lock không thành công >> cảnh báo
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                    return;
                }
            }
            else
            {
                // Unlock dữ liệu
                // (Hiện tại không phải Unlock dữ liệu, vì chỉ được xóa khi chọn sửa - sẽ Unlock sau)
                return;
            }
        }

        private void beforeApprove()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_idKhachHang);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.KHTV,
                    _function,
                    DatabaseConstant.Table.KH_KHANG_HSO,
                    DatabaseConstant.Action.DUYET,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    // Gọi tới hàm duyệt dữ liệu
                    onApprove();
                    return;
                }
                // Nếu lock không thành công >> cảnh báo
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                    return;
                }
            }
            else
            {
                // Unlock dữ liệu
                // (Hiện tại không phải Unlock dữ liệu, vì chỉ được xóa khi chọn sửa - sẽ Unlock sau)
                return;
            }
        }

        private void beforeCancel()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_idKhachHang);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.KHTV,
                    _function,
                    DatabaseConstant.Table.KH_KHANG_HSO,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    // Gọi tới hàm thoái duyệt dữ liệu
                    onCancel();
                    return;
                }
                // Nếu lock không thành công >> cảnh báo
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                    return;
                }
            }
            else
            {
                // Unlock dữ liệu
                // (Hiện tại không phải Unlock dữ liệu, vì chỉ được xóa khi chọn sửa - sẽ Unlock sau)
                return;
            }
        }

        /// <summary>
        /// Trước khi từ chối
        /// </summary>
        private void beforeRefuse()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_idKhachHang);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.KHTV,
                    _function,
                    DatabaseConstant.Table.KH_KHANG_HSO,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    // Gọi tới hàm xóa dữ liệu
                    onRefuse();
                    return;
                }
                // Nếu lock không thành công >> cảnh báo
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                    return;
                }
            }
            else
            {
                // Unlock dữ liệu
                // (Hiện tại không phải Unlock dữ liệu, vì chỉ được xóa khi chọn sửa - sẽ Unlock sau)
                return;
            }
        }

        /// <summary>
        /// Lưu dữ liệu (hay trình duyệt)
        /// </summary>
        private void onSave()
        {
            string trangThai = tthaiNvu != "" ? tthaiNvu : CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.TRINH_DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            tabThongTinChung.Focus();
            if (Validation())
            {
                KhachHangProcess process = new KhachHangProcess();
                try
                {
                    // Dữ liệu truyền vào và dữ liệu trả về
                    KH_KHANG_HSO obj = new KH_KHANG_HSO();
                    List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                    int ret = -1;
                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (dsSourceKHang.Tables.Contains("VKH_LACNHOM_TRUONG"))
                        dsSourceKHang.Tables.Remove("VKH_LACNHOM_TRUONG");
                    DataTable dt = new DataTable("VKH_LACNHOM_TRUONG");
                    dt.Columns.Add("LA_CTRUONG", typeof(string));
                    dt.Columns.Add("LA_NTRUONG", typeof(string));
                    DataRow drv = dt.NewRow();
                    drv["LA_CTRUONG"] = "false";
                    drv["LA_NTRUONG"] = "false";
                    dsSourceKHang.Tables.Add(dt);
                    if (_idKhachHang == -1)
                    {
                        // Lấy dữ liệu từ form
                        obj = GetFormData(false, trangThai);
                        
                        dt.Rows.Add(drv);
                        ret = process.Them(obj, dsSourceKHang, ref _idKhachHang, ref lstResponseDetail);
                        tthaiNvu = obj.TTHAI_NVU;
                        CommonFunction.ThongBaoKetQua(lstResponseDetail);
                        afterAddNew(_idKhachHang, ret);
                    }
                    // Nếu là lưu tạm khi sửa
                    // Hoặc lưu tạm khi sửa sau duyệt
                    // Hoặc sửa
                    else
                    {
                        // Lấy dữ liệu từ form
                        obj = GetFormData(true, trangThai);
                        ret = process.Sua(obj, dsSourceKHang, ref lstResponseDetail);
                        tthaiNvu = obj.TTHAI_NVU;
                        CommonFunction.ThongBaoKetQua(lstResponseDetail);
                        afterModify(_idKhachHang, ret);
                    }
                }
                catch (System.Exception ex)
                {
                    this.Cursor = Cursors.Arrow;
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                finally
                {
                    process = null;
                }
            }
        }

        /// <summary>
        /// Lưu tạm dữ liệu
        /// </summary>
        private void onHold()
        {
            string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            KhachHangProcess process = new KhachHangProcess();
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            try
            {
                // Dữ liệu truyền vào và dữ liệu trả về
                KH_KHANG_HSO obj = new KH_KHANG_HSO();
                int ret = -1;

                // Nếu là lưu tạm hoặc thêm mới lần đầu
                if (_idKhachHang == -1)
                {
                    // Lấy dữ liệu từ form
                    obj = GetFormData(false, trangThai);
                    ret = process.Them(obj, dsSourceKHang, ref _idKhachHang, ref lstResponseDetail);
                    tthaiNvu = obj.TTHAI_NVU;
                    //CommonFunction.ThongBaoKetQua(lstResponseDetail);
                    afterAddNew(_idKhachHang, ret);
                }
                // Nếu là lưu tạm khi sửa
                // Hoặc lưu tạm khi sửa sau duyệt
                // Hoặc sửa
                else
                {
                    // Lấy dữ liệu từ form
                    obj = GetFormData(true, trangThai);
                    ret = process.Sua(obj, dsSourceKHang, ref lstResponseDetail);
                    tthaiNvu = obj.TTHAI_NVU;
                    CommonFunction.ThongBaoKetQua(lstResponseDetail);
                    afterModify(_idKhachHang, ret);
                }
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }
        }

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        private void onDelete()
        {
            KhachHangProcess process = new KhachHangProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = _idKhachHang;

                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                int ret = process.Xoa(arrayID, ref lstResponseDetail);
                CommonFunction.ThongBaoKetQua(lstResponseDetail);

                afterDelete(ret);
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Duyệt dữ liệu
        /// </summary>
        private void onApprove()
        {
            KhachHangProcess process = new KhachHangProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = _idKhachHang;
                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                int ret = process.Duyet(arrayID, ref lstResponseDetail);
                CommonFunction.ThongBaoKetQua(lstResponseDetail);
                afterApprove(ret);
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Thoái duyệt dữ liệu
        /// </summary>
        private void onCancel()
        {
            KhachHangProcess process = new KhachHangProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = _idKhachHang;

                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                int ret = process.ThoaiDuyet(arrayID, ref lstResponseDetail);
                CommonFunction.ThongBaoKetQua(lstResponseDetail);

                afterCancel(ret);
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Từ chối duyệt dữ liệu
        /// </summary>
        private void onRefuse()
        {
            KhachHangProcess process = new KhachHangProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = _idKhachHang;

                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                int ret = process.TuChoi(arrayID, ref lstResponseDetail);
                CommonFunction.ThongBaoKetQua(lstResponseDetail);

                afterRefuse(ret);
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Sau khi thêm mới
        /// </summary>
        /// <param name="ret"></param>
        private void afterAddNew(int id, int ret)
        {
            if (id>0)
            {

                SetEnabledAllControls(false);
                if (cbMultiAdd.IsChecked == true)
                {
                    SetEnabledAllControls(true);
                    ResetForm();
                }
                else if (!DatabaseConstant.CLOSE_DETAIL_FORM)
                {
                    _idKhachHang = id;
                    SetFormData();
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu,mnuMain,DatabaseConstant.Function.KH_THANH_VIEN);
                    txtTenKhachHang.Focus();
                }
                else
                {
                    onClose();
                }
            }
            else
            {
                //LMessage.ShowMessage("M.DungChung.ThemKhongThanhCong", LMessage.MessageBoxType.Error);
            }
        }

        /// <summary>
        /// Sau khi sửa
        /// </summary>
        /// <param name="ret"></param>
        private void afterModify(int id, int ret)
        {
            if (ret > 0)
            {
                _idKhachHang = id;
                SetEnabledAllControls(false);
                if (cbMultiAdd.IsChecked == true)
                {
                    SetEnabledAllControls(true);
                    ResetForm();
                }
                else if (!DatabaseConstant.CLOSE_DETAIL_FORM)
                {
                    _idKhachHang = id;
                    SetFormData();
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KH_THANH_VIEN);
                    txtTenKhachHang.Focus();
                }
                else
                {
                    onClose();
                }
            }
            else
            {
                //LMessage.ShowMessage("M.DungChung.CapNhatKoThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                _function,
                DatabaseConstant.Table.KH_KHANG_HSO,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Sau khi xóa
        /// </summary>
        /// <param name="ret"></param>
        private void afterDelete(int ret)
        {
            

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idKhachHang);
            List<int> listUnlockId = new List<int>();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                _function,
                DatabaseConstant.Table.KH_KHANG_HSO,
                DatabaseConstant.Action.XOA,
                listLockId);
            if (ret == 2)
            {
                // Đóng cửa sổ chi tiết sau khi xóa
                onClose();
            }
            else
            {

            }
            
        }

        /// <summary>
        /// Sau khi duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void afterApprove(int ret)
        {
            if (ret == 2)
            {
                TthaiNvu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);

                SetEnabledAllControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KH_THANH_VIEN);
            }
            else
            {
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idKhachHang);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                _function,
                DatabaseConstant.Table.KH_KHANG_HSO,
                DatabaseConstant.Action.DUYET,
                listLockId);
        }

        /// <summary>
        /// Sau khi thoái duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void afterCancel(int ret)
        {
            if (ret == 2)
            {
                TthaiNvu = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);

                SetEnabledAllControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KH_THANH_VIEN);
            }
            else
            {
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idKhachHang);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                _function,
                DatabaseConstant.Table.KH_KHANG_HSO,
                DatabaseConstant.Action.THOAI_DUYET,
                listLockId);
        }

        /// <summary>
        /// Sau khi từ chối
        /// </summary>
        /// <param name="ret"></param>
        private void afterRefuse(int ret)
        {
            if (ret == 2)
            {

                TthaiNvu = BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);

                SetEnabledAllControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KH_THANH_VIEN);
            }
            else
            {
                
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idKhachHang);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                _function,
                DatabaseConstant.Table.KH_KHANG_HSO,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                listLockId);
        }

        /// <summary>
        /// Lay thong tin khách hàng
        /// </summary>
        /// <returns></returns>
        private Presentation.Process.KhachHangServiceRef.KH_KHANG_HSO GetFormData(bool isUpdate, string status)
        {
            Presentation.Process.KhachHangServiceRef.KH_KHANG_HSO obj = new Presentation.Process.KhachHangServiceRef.KH_KHANG_HSO();
            this.Cursor = Cursors.Wait;
            try
            {
                #region Lay du lieu tu cac combobox
                AutoComboBox au = new AutoComboBox();
                AutoCompleteEntry auPhongGDHienTai = au.getEntryByDisplayName(lstSourcePhongGD, ref cmbPhongGD);
                AutoCompleteEntry auGioiTinh = au.getEntryByDisplayName(lstSourceGioiTinh, ref cmbGioiTinh);
                AutoCompleteEntry auNoiCap = au.getEntryByDisplayName(lstSourceNoiCap, ref cmbNoiCap);
                AutoCompleteEntry auLyDoRa = au.getEntryByDisplayName(lstSourceLyDoRa, ref cmbLyDoRaKhoiNhom);
                #endregion

                #region Tab thong tin chung
                if (isUpdate)
                {
                    obj.ID = _idKhachHang;
                }
                obj.MA_KHANG_LOAI = "TVIEN";
                if (isUpdate)
                {
                    obj.MA_KHANG = txtMaKhachHang.Text;
                    obj.MA_TVIEN = txtMaKhachHang.Text;
                }
                obj.TEN_KHANG = txtTenKhachHang.Text;
                obj.TEN_GDICH = obj.TEN_KHANG;
                if (raddtNgayTGiaTC.Value != null)
                {
                    obj.NGAY_THAM_GIA = LDateTime.DateToString(Convert.ToDateTime(raddtNgayTGiaTC.Value), "yyyyMMdd");
                }
                else
                {
                    obj.NGAY_THAM_GIA = "";
                }

                obj.NGAY_THANH_LAP = "";
                if (raddtNgayHetHL.Value != null)
                {
                    obj.NGAY_HET_HLUC = LDateTime.DateToString(Convert.ToDateTime(raddtNgayHetHL.Value), "yyyyMMdd");
                }
                if (auLyDoRa != null)
                {
                    obj.MA_LY_DO = auLyDoRa.KeywordStrings[0];
                }

                obj.ID_DON_VI = Convert.ToInt32(idPhongGD);
                if (!LString.IsNullOrEmptyOrSpace(idKhuVuc))
                {
                    obj.ID_KHU_VUC = Convert.ToInt32(idKhuVuc);
                }
                if (!LString.IsNullOrEmptyOrSpace(idCum))
                {
                    obj.ID_CUM = Convert.ToInt32(idCum);
                }
                if (!LObject.IsNullOrEmpty(txtMaNhom.Tag) && txtMaNhom.Tag.ToString().IsNumeric())
                {
                    obj.ID_NHOM = Convert.ToInt32(txtMaNhom.Tag);
                }
                if (raddtNgaySinh.Value != null)
                {
                    obj.DD_NGAY_SINH = LDateTime.DateToString(Convert.ToDateTime(raddtNgaySinh.Value), "yyyyMMdd");
                }

                if (formCase == "CNHAN" || formCase == "TVIEN")
                {
                    obj.DD_GTLQ_LOAI = ApplicationConstant.LoaiGiayTo.CHUNG_MINH_ND.layGiaTri();
                }
                else
                {
                    obj.DD_GTLQ_LOAI = ApplicationConstant.LoaiGiayTo.GP_DKKD.layGiaTri();
                }
                obj.DD_GTLQ_SO = txtSoCMND.Text.Trim();
                if (raddtNgayCap.Value != null)
                {
                    obj.DD_GTLQ_NGAY_CAP = LDateTime.DateToString(Convert.ToDateTime(raddtNgayCap.Value), "yyyyMMdd");
                }
                if (auNoiCap != null)
                {
                    obj.DD_GTLQ_NOI_CAP = auNoiCap.KeywordStrings[0];
                }
                if (auGioiTinh != null)
                {
                    obj.DD_GIOI_TINH = auGioiTinh.KeywordStrings[0];
                }
                obj.DD_MA_DAN_TOC = "";
                obj.DD_MA_QUOC_TICH = "";
                obj.MA_SO_THUE = "";
                obj.DD_TTRU_DIA_CHI = "";
                obj.DIA_CHI = txtDiaChi.Text;
                obj.SO_DTHOAI = txtSoCoDinh.Text;
                obj.SO_DDONG = txtSoDiDong.Text;
                obj.EMAIL = txtEmail.Text;

                obj.DD_HO_TEN = obj.TEN_KHANG;
                obj.DD_SO_DTHOAI = obj.SO_DTHOAI;
                obj.DD_SO_DDONG = obj.SO_DDONG;
                obj.DD_EMAIL = obj.EMAIL;
                obj.DDAN_HINH_ANH = "";
                obj.TTIN_KHAC = txtTTinKhac.Text;
                #endregion

                #region Thông tin kiểm soát
                obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.TTHAI_NVU = status;
                //obj.MA_DVI_TAO = Presentation.Process.Common.ClientInformation.MaDonVi;
                obj.MA_DVI_TAO = auPhongGDHienTai.KeywordStrings.ElementAt(0);
                obj.MA_DVI_QLY = Presentation.Process.Common.ClientInformation.MaDonVi;
                if (!isUpdate)
                {
                    obj.NGAY_NHAP = Presentation.Process.Common.ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_NHAP = Presentation.Process.Common.ClientInformation.TenDangNhap;
                }
                else
                {
                    obj.NGAY_NHAP = LDateTime.DateToString(Convert.ToDateTime(raddtNgayNhap.Value), "yyyyMMdd");
                    obj.NGUOI_NHAP = txtNguoiLap.Text;
                    obj.NGAY_CNHAT = Presentation.Process.Common.ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_CNHAT = Presentation.Process.Common.ClientInformation.TenDangNhap;
                }
                #endregion

                // Lay ma tinhtp cua don vi
                string maTinhTp = "02";
                obj.MA_TINHTP = maTinhTp;
                obj.NV_LOAI_NVON = "VON_TUCO";
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
            return obj;
        }

        /// <summary>
        /// Kiểm tra dữ liệu trước khi lưu
        /// </summary>
        /// <returns></returns>
        private bool Validation()
        {
            AutoComboBox au = new AutoComboBox();
            AutoCompleteEntry auLyDoHHL = au.getEntryByDisplayName(lstSourceLyDoRa, ref cmbLyDoRaKhoiNhom);
            if (chkKHHieuLuc.IsChecked.GetValueOrDefault())
            {
                if (!tthaiNvu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.TrangThaiNghiepVuKhongPhuHop", LMessage.MessageBoxType.Warning);
                    txtTenKhachHang.Focus();
                    return false;
                }
                else if (idKhachHang < 1)
                {
                    LMessage.ShowMessage("M_ResponseMessage_KhachHang_KhongTonTai", LMessage.MessageBoxType.Warning);
                    txtTenKhachHang.Focus();
                    return false;
                }
                if (raddtNgayHetHL.Value != null)
                {
                    if (raddtNgayHetHL.Value != null && raddtNgayHetHL.Value <= raddtNgayTGiaTC.Value)
                    {
                        LMessage.ShowMessage("M.KhachHang.ucKhachHangThanhVien.SaiNgayHetHan", LMessage.MessageBoxType.Warning);
                        raddtNgayHetHL.Focus();
                        return false;
                    }
                    else if (LObject.IsNullOrEmpty(auLyDoHHL) || cmbLyDoRaKhoiNhom.Text.IsNullOrEmptyOrSpace())
                    {
                        CommonFunction.ThongBaoTrong(lblLyDoRaNhom.Content.ToString());
                        cmbLyDoRaKhoiNhom.Focus();
                        return false;
                    }
                }
            }
            else
            {

                if (LString.IsNullOrEmptyOrSpace(txtTenKhachHang.Text))
                {
                    CommonFunction.ThongBaoTrong(lblTenKhachHang.Content.ToString());
                    txtTenKhachHang.Focus();
                    return false;
                }
                else if (LString.IsNullOrEmptyOrSpace(txtMaCum.Text))
                {
                    CommonFunction.ThongBaoTrong(lblCum.Content.ToString());
                    txtMaCum.Focus();
                    return false;
                }
                else if (raddtNgayTGiaTC.Value == null)
                {
                    CommonFunction.ThongBaoTrong(lblNgayThamGiaTC.Content.ToString());
                    raddtNgayTGiaTC.Focus();
                    return false;
                }
                else if (raddtNgaySinh.Value == null)
                {
                    CommonFunction.ThongBaoTrong(lblNgaySinh.Content.ToString());
                    raddtNgaySinh.Focus();
                    return false;
                }
                else if (LString.IsNullOrEmptyOrSpace(txtSoCMND.Text))
                {
                    CommonFunction.ThongBaoTrong(lblSoCMND.Content.ToString());
                    txtSoCMND.Focus();
                    return false;
                }
                //else if (new KhachHangProcess().CheckSoCMND(txtSoCMND.Text, txtMaKhachHang.Text) == 0)
                //{
                //    //CommonFunction.ThongBaoTrong(lblSoCMND.Content.ToString());
                //    CommonFunction.ThongBaoLoi("M.KhachHang.ucKhachHangThanhVien.TrungCMND");
                //    txtSoCMND.Focus();
                //    return false;
                //}
                else if (raddtNgayCap.Value == null)
                {
                    CommonFunction.ThongBaoTrong(lblNgayCap.Content.ToString());
                    raddtNgayCap.Focus();
                    return false;
                }
                else if (LString.IsNullOrEmptyOrSpace(cmbNoiCap.Text))
                {
                    CommonFunction.ThongBaoTrong(lblNoiCap.Content.ToString());
                    cmbNoiCap.Focus();
                    return false;
                }
                else if (LString.IsNullOrEmptyOrSpace(txtDiaChi.Text))
                {
                    CommonFunction.ThongBaoTrong(lblDiaChi.Content.ToString());
                    txtDiaChi.Focus();
                    return false;
                }
                if (!raddtNgaySinh.Value.IsNullOrEmpty())
                {
                    int minTuoi = 16;
                    UtilitiesProcess process = new UtilitiesProcess();
                    string _minTuoi = process.LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_KH_MIN_TUOI, null);
                    string _maxTuoi = process.LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_KH_MAX_TUOI, null);
                    if (!_minTuoi.IsNullOrEmpty() && !_minTuoi.IsNumeric()) minTuoi = _minTuoi.StringToInt32();
                    //if (!_maxTuoi.IsNullOrEmpty() && !_maxTuoi.IsNumeric()) maxTuoi = _maxTuoi.StringToInt32();
                    string ngay = "";
                    if (raddtNgaySinh.Value == null)
                    {
                        CommonFunction.ThongBaoTrong(lblNgaySinh.Content.ToString());
                        raddtNgaySinh.Focus();
                        return false;
                    }
                    else
                        ngay = Convert.ToDateTime(raddtNgaySinh.Value).DateToString("yyyyMMdd");


                    //if (ClientInformation.NgayLamViecHienTai.CompareTo((ngay.StringToDate("yyyyMMdd").AddYears(minTuoi)).DateToString("yyyyMMdd")) < 0
                    //    || ClientInformation.NgayLamViecHienTai.CompareTo((ngay.StringToDate("yyyyMMdd").AddYears(maxTuoi)).DateToString("yyyyMMdd")) > 0)
                    //{
                    //    CommonFunction.ThongBaoLoi("M.KhachHang.Popup.ucThongTinCoBanHoGD.LoiNgaySinh");
                    //    raddtNgaySinh.Focus();
                    //    return false;
                    //}
                    if (ClientInformation.NgayLamViecHienTai.CompareTo((ngay.StringToDate("yyyyMMdd").AddYears(minTuoi)).DateToString("yyyyMMdd")) < 0)
                    {
                        CommonFunction.ThongBaoLoi("M.KhachHang.Popup.ucThongTinCoBanHoGD.LoiNgaySinh");
                        raddtNgaySinh.Focus();
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion

        /// <summary>
        /// Lấy dữ liệu khách hàng theo id
        /// </summary>
        private void SetFormData()
        {
            Presentation.Process.KhachHangProcess process = new Presentation.Process.KhachHangProcess();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                dsSourceKHang = process.getThongTinKHTheoID(_idKhachHang);
                if (dsSourceKHang != null && dsSourceKHang.Tables.Count > 0)
                {
                    //Dữ liệu thông tin chung
                    DataRow dr = dsSourceKHang.Tables["KH_KHANG_HSO"].Rows[0];
                    if (dr["ID_DON_VI"] != null && !LString.IsNullOrEmptyOrSpace(dr["ID_DON_VI"].ToString()))
                    {
                        idPhongGD = dr["ID_DON_VI"].ToString();
                        cmbPhongGD.SelectedIndex = lstSourcePhongGD.FindIndex(f => f.KeywordStrings[0].Equals(dr["MA_DVI_TAO"]));
                    }
                    idKhuVuc = dr["ID_KHU_VUC"].ToString();
                    idKhuVuc = dr["ID_KHU_VUC"].ToString();

                    if (dr["ID_CUM"] != null && !LString.IsNullOrEmptyOrSpace(dr["ID_CUM"].ToString()))
                    {
                        txtMaCum.Tag = idCum = dr["ID_CUM"].ToString();
                        txtMaCum.Text = dr["MA_CUM"].ToString();
                        lblTenCum.Content = dr["TEN_CUM"].ToString();
                    }
                    if (dr["ID_NHOM"] != null && !LString.IsNullOrEmptyOrSpace(dr["ID_NHOM"].ToString()))
                    {
                        txtMaNhom.Tag = dr["ID_NHOM"].ToString();
                        txtMaNhom.Text = dr["MA_NHOM"].ToString();
                        lblTenNhom.Content = dr["TEN_NHOM"].ToString();
                    }
                    MaKhachHang = txtMaKhachHang.Text = dr["MA_KHANG"].ToString();
                    txtTenKhachHang.Text = dr["TEN_KHANG"].ToString();

                    if (!LString.IsNullOrEmptyOrSpace(dr["NGAY_THAM_GIA"].ToString()))
                    {
                        raddtNgayTGiaTC.Value = LDateTime.StringToDate(dr["NGAY_THAM_GIA"].ToString(), "yyyyMMdd");
                    }
                    else
                    {
                        raddtNgayTGiaTC.Value = null;
                    }

                    if (!LString.IsNullOrEmptyOrSpace(dr["NGAY_HET_HLUC"].ToString()))
                    {
                        chkKHHieuLuc.IsChecked = true;
                        raddtNgayHetHL.Value = LDateTime.StringToDate(dr["NGAY_HET_HLUC"].ToString(), "yyyyMMdd");
                    }
                    else
                    {
                        raddtNgayHetHL.Value = null;
                    }

                    if (dr["MA_LY_DO"] != null && !LString.IsNullOrEmptyOrSpace(dr["MA_LY_DO"].ToString()))
                    {
                        cmbLyDoRaKhoiNhom.SelectedIndex = lstSourceLyDoRa.IndexOf(lstSourceLyDoRa.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["MA_LY_DO"].ToString())));
                    }

                    if (!LString.IsNullOrEmptyOrSpace(dr["DD_NGAY_SINH"].ToString()))
                    {
                        raddtNgaySinh.Value = LDateTime.StringToDate(dr["DD_NGAY_SINH"].ToString(), "yyyyMMdd");
                    }
                    else
                    {
                        raddtNgaySinh.Value = null;
                    }

                    txtSoCMND.Text = dr["DD_GTLQ_SO"].ToString();

                    if (!LString.IsNullOrEmptyOrSpace(dr["DD_GTLQ_NGAY_CAP"].ToString()))
                    {
                        raddtNgayCap.Value = LDateTime.StringToDate(dr["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd");
                    }
                    else
                    {
                        raddtNgayCap.Value = null;
                    }

                    if (!LString.IsNullOrEmptyOrSpace(dr["DD_GTLQ_NOI_CAP"].ToString()))
                    {
                        cmbNoiCap.SelectedIndex = lstSourceNoiCap.IndexOf(lstSourceNoiCap.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["DD_GTLQ_NOI_CAP"].ToString())));
                    }
                    else
                    {
                        cmbNoiCap.SelectedIndex = -1;
                    }

                    if (!LString.IsNullOrEmptyOrSpace(dr["DD_GIOI_TINH"].ToString()))
                    {
                        cmbGioiTinh.SelectedIndex = lstSourceGioiTinh.IndexOf(lstSourceGioiTinh.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["DD_GIOI_TINH"].ToString())));
                    }
                    else
                    {
                        cmbGioiTinh.SelectedIndex = -1;
                    }

                    txtDiaChi.Text = dr["DIA_CHI"].ToString();
                    txtSoCoDinh.Text = dr["DD_SO_DTHOAI"].ToString();
                    txtSoDiDong.Text = dr["DD_SO_DDONG"].ToString();
                    txtEmail.Text = dr["DD_EMAIL"].ToString();
                    // Thông tin kiểm soát
                    txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(dsSourceKHang.Tables["KH_KHANG_HSO"].Rows[0]["TTHAI_NVU"].ToString());
                    lblTrangThai.Content = txtTrangThai.Text;
                    txtNguoiLap.Text = dsSourceKHang.Tables["KH_KHANG_HSO"].Rows[0]["NGUOI_NHAP"].ToString();
                    raddtNgayNhap.Value = LDateTime.StringToDate(dsSourceKHang.Tables["KH_KHANG_HSO"].Rows[0]["NGAY_NHAP"].ToString(), "yyyyMMdd");
                    if (dsSourceKHang.Tables["KH_KHANG_HSO"].Rows[0]["NGUOI_CNHAT"] != null && !LString.IsNullOrEmptyOrSpace(dsSourceKHang.Tables["KH_KHANG_HSO"].Rows[0]["NGUOI_CNHAT"].ToString()))
                    {
                        txtNguoiCapNhat.Text = dsSourceKHang.Tables["KH_KHANG_HSO"].Rows[0]["NGUOI_CNHAT"].ToString();
                    }
                    if (dsSourceKHang.Tables["KH_KHANG_HSO"].Rows[0]["NGAY_CNHAT"] != null && !LString.IsNullOrEmptyOrSpace(dsSourceKHang.Tables["KH_KHANG_HSO"].Rows[0]["NGAY_CNHAT"].ToString()))
                    {
                        raddtNgayCNhat.Value = LDateTime.StringToDate(dsSourceKHang.Tables["KH_KHANG_HSO"].Rows[0]["NGAY_CNHAT"].ToString(), "yyyyMMdd");
                    }
                    if (dsSourceKHang.Tables["KH_KHANG_HSO"].Rows[0]["TTIN_KHAC"] != DBNull.Value)
                        txtTTinKhac.Text = dsSourceKHang.Tables["KH_KHANG_HSO"].Rows[0]["TTIN_KHAC"].ToString();
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Reset form
        /// </summary>
        private void ResetForm()
        {
            // Tham so
            _idKhachHang = -1;
            //idKhuVuc = "";
            //idCum = "";

            // Thong tin chung
            txtMaKhachHang.Text = "";
            txtTenKhachHang.Focus();
            txtTenKhachHang.Text = "";
            //txtMaCum.Text = "";
            //txtMaCum.Tag = null;
            //lblTenCum.Content = "Tên cụm"; 
            raddtNgayHetHL.Value = null;
            cmbLyDoRaKhoiNhom.SelectedIndex = -1;
            txtSoCMND.Text = "";
            txtDiaChi.Text = "";
            raddtNgayTGiaTC.Value = LDateTime.StringToDate(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            lblTrangThai.Content = tthaiNvu = "";
            raddtNgaySinh.Value = null;
            raddtNgayCap.Value = null;
            raddtNgayNhap.Value = LDateTime.StringToDate(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            txtSoCoDinh.Text = "";
            txtSoDiDong.Text = "";
            txtEmail.Text = "";
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KH_THANH_VIEN);
        }

        private void SetEnabledAllControls(bool enable)
        {
            txtTenKhachHang.IsEnabled = enable;
            txtMaCum.IsEnabled = enable;
            raddtNgayTGiaTC.IsEnabled = enable;
            raddtNgaySinh.IsEnabled = enable;
            txtSoCMND.IsEnabled = enable;
            raddtNgayCap.IsEnabled = enable;
            cmbNoiCap.IsEnabled = enable;
            cmbGioiTinh.IsEnabled = enable;
            txtDiaChi.IsEnabled = enable;
            cmbLyDoRaKhoiNhom.IsEnabled = enable;
            txtSoCoDinh.IsEnabled = enable;
            txtSoDiDong.IsEnabled = enable;
            txtEmail.IsEnabled = enable;
            txtMaNhom.IsEnabled = enable;
            txtTTinKhac.IsEnabled = enable;
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Presentation.Process.DanhMucProcess processDanhMuc = new Presentation.Process.DanhMucProcess();
            if (cmbPhongGD.SelectedIndex >= 0)
            {
                if (lstSourcePhongGD.Select(i => i.DisplayName).Contains(cmbPhongGD.Text))
                {
                    idPhongGD = processDanhMuc.getDonViTheoMa(((AutoCompleteEntry)cmbPhongGD.SelectedValue).KeywordStrings[0]).Tables[0].Rows[0]["ID"].ToString();
                }
                else
                {
                    idPhongGD = ClientInformation.IdDonViGiaoDich.ToString();
                }
            }
            else
            {
                idPhongGD = ClientInformation.IdDonViGiaoDich.ToString();
            }
            idKhuVuc = processDanhMuc.getKhuVucByIdDonVi(idPhongGD.StringToInt32()).ID.ToString();
        }

        private void btnCum_Click(object sender, RoutedEventArgs e)
        {
            HienPopUpCum();
        }

        /// <summary>
        /// Nhấn nút "F3" hiển để hiển thị popup khu vực/cụm/nhóm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNhom_Click(object sender, RoutedEventArgs e)
        {
            HienPopUpNhom();
        }

        /// <summary>
        /// Hiện popup khu vực/cụm/nhóm
        /// </summary>
        private void HienPopUpNhom()
        {
            try
            {
                var process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DM_CUM.getValue(), lstDieuKien);

                SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(true, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey("U.POPUP.POPUP_DS_NHOM_KHACHHANG");
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow dr = lstPopup[0];
                    txtMaNhom.Tag = dr[1].ToString(); // id nhom
                    txtMaNhom.Text = dr[2].ToString(); // ma nhom
                    lblTenNhom.Content = dr[3].ToString(); // ten nhom

                    Presentation.Process.KhachHangProcess processKhachHang = new Presentation.Process.KhachHangProcess();
                    try
                    {
                        DataSet ds = processKhachHang.getThongTinCumTheoIDNhom(txtMaNhom.Tag.ToString());
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            lblTenCum.Content = ds.Tables[0].Rows[0]["ten_cum"].ToString();
                            txtMaCum.Text = ds.Tables[0].Rows[0]["ma_cum"].ToString();
                            txtMaCum.Tag = idCum = ds.Tables[0].Rows[0]["id"].ToString();
                            idPhongGD = ds.Tables[0].Rows[0]["id_dvi"].ToString();
                            idKhuVuc = ds.Tables[0].Rows[0]["id_kvuc"].ToString();
                        }
                        else
                        {
                            txtMaNhom.Tag = "";
                            txtMaNhom.Text = "";
                            lblTenNhom.Content = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        txtMaNhom.Tag = "";
                        txtMaNhom.Text = "";
                        lblTenNhom.Content = "";
                        LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
    }
}
