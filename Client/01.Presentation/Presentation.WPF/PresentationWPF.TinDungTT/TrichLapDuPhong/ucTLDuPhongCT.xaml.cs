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
using Telerik.Windows.Controls;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.Common;
using System.Data;
using Presentation.Process.TinDungServiceRef;
using System.Reflection;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;
using Telerik.Windows.Controls.GridView;

namespace PresentationWPF.TinDungTT.TrichLapDuPhong
{
    /// <summary>
    /// Interaction logic for ucTLDuPhongCT.xaml
    /// </summary>
    public partial class ucTLDuPhongCT : UserControl
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
        string TThaiNVu = "";
        List<DataRow> lstPopupKU = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopupKU = lst;
        }
        DataTable dtKUOC = new DataTable();
        List<AutoCompleteEntry> lstTienTe = new List<AutoCompleteEntry>();
        TDVM_TRICH_LAP_DU_PHONG TDVMTRICHLAPDUPHONG = null;
        List<DANH_SACH_DU_PHONG_CHUNG> lstDuPhongChung = null;
        List<DANH_SACH_KHE_UOC_DU_PHONG> lstDuPhongCuThe = null;
        int iDGiaoDich = 0;
        string MaGiaoDich = "";
        DatabaseConstant.Action action;
        public event EventHandler OnSavingCompleted;
        private KIEM_SOAT _objKiemSoat;

        public Utilities.Common.KIEM_SOAT ObjKiemSoat
        {
            get { return _objKiemSoat; }
            set { _objKiemSoat = value; }
        }
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucTLDuPhongCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDungTT;component/TrichLapDuPhong/ucTLDuPhongCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            KhoiTaoComboBox();
            ClearForm();
            InitEvenHanler();
            
        }

        public ucTLDuPhongCT(KIEM_SOAT _KiemSoat) : this()
        {
            _objKiemSoat = _KiemSoat;
            TDVMTRICHLAPDUPHONG = new TDVM_TRICH_LAP_DU_PHONG();
            TDVMTRICHLAPDUPHONG.MA_GIAO_DICH = _objKiemSoat.SO_GIAO_DICH;
            action = _KiemSoat.action;
            SetDataForm();
            CommonFunction.RefreshButton(Toolbar, _KiemSoat.action, TThaiNVu, mnuMain);
        }
        private void InitEvenHanler()
        {
            raddgrTrichLapDuPhongChung.CellEditEnded += new EventHandler<GridViewCellEditEndedEventArgs>(raddgrTrichLapDuPhongChung_CellEditEnded);
            raddgrTrichLapDuPhong.CellEditEnded += new EventHandler<GridViewCellEditEndedEventArgs>(raddgrTrichLapDuPhong_CellEditEnded);
            chkDuPhongChung.Click += new RoutedEventHandler(chkDuPhongChung_Checked);
            chkDuPhongCuThe.Click += new RoutedEventHandler(chkDuPhongCuThe_Checked);
        }
   
        private void KhoiTaoComboBox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            
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
                        KeyGesture keyg = new KeyGesture(Key.I, ModifierKeys.Control);
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
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control|ModifierKeys.Shift);
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
                        KeyGesture keyg = new KeyGesture(Key.F, ModifierKeys.Control | ModifierKeys.Shift);
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
                action = DatabaseConstant.Action.SUA;
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
                BeforeRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                BeforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
                OnPreview();
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
            TDVMTRICHLAPDUPHONG = new TDVM_TRICH_LAP_DU_PHONG();
            lstDuPhongChung = new List<DANH_SACH_DU_PHONG_CHUNG>();
            lstDuPhongCuThe = new List<DANH_SACH_KHE_UOC_DU_PHONG>();
            raddgrTrichLapDuPhong.ItemsSource = null;
            raddgrTrichLapDuPhongChung.ItemsSource = null;
            raddgrTrichLapDuPhong.ItemsSource = lstDuPhongCuThe;
            raddgrTrichLapDuPhongChung.ItemsSource = lstDuPhongChung;
            iDGiaoDich = 0;
            MaGiaoDich = "";
            txtSoGiaoDich.Text = "";
            teldtNgayGiaoDich.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            teldtDenNgay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            txtDienGiai.Text = "";
            TThaiNVu = "";
            txtNguoiCapNhat.Text = "";
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            txtTrangThai.Text = "";
            lblTrangThai.Content = "";
            teldtNgayCNhat.Value = null;
            TinhToanDuPhongChung();
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, TThaiNVu,mnuMain);
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
                string oldValue="";
                DatePicker dtpControl = (DatePicker)sender;
                StringBuilder sbControl = new StringBuilder();
                sbControl.Append("teldt");
                sbControl.Append(dtpControl.Name.Substring(3));
                RadMaskedDateTimeInput telControl = (RadMaskedDateTimeInput)grMain.FindName(sbControl.ToString());
                if (telControl != null)
                {
                    oldValue=telControl.Text;
                    telControl.Value = dtpControl.SelectedDate;
                }
                else
                    throw new System.NullReferenceException("M.TinDungTT.TrichLapDuPhong.ucTLDuPhongCT.KhongTimThayControl" + sbControl.ToString());
                if (telControl.Name.Equals("teldtDenNgay") && !telControl.Text.Equals(oldValue))
                    clearKheUoc();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void tlbThemKUOC_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            try
            {
                string sidKheUoc = "";
                if (!LObject.IsNullOrEmpty(lstDuPhongCuThe))
                {
                    foreach (DANH_SACH_KHE_UOC_DU_PHONG dr in lstDuPhongCuThe)
                    {
                        sidKheUoc += "," + dr.ID_KHE_UOC.ToString();
                    }
                }
                
                if (sidKheUoc.Length > 0)
                    sidKheUoc = sidKheUoc.Substring(1);
                else
                    sidKheUoc = "0";
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(sidKheUoc);
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add("KUOCTRICHDP");
                lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
                lstDieuKien.Add("%");
                lstPopupKU = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_KHEUOC", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopupKheUocViMo popup = new ucPopupKheUocViMo(true, simplePopupResponse, true);
                popup.LayGiaTriListDataRow = new ucPopupKheUocViMo.LayListDataRow(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopupKU.Count > 0)
                {
                    List<DANH_SACH_KHE_UOC_DU_PHONG> lstPopupDS = new List<DANH_SACH_KHE_UOC_DU_PHONG>();
                    //lstDuPhongChung.Clear();
                    //lstDuPhongCuThe.Clear();
                    int iret = 0;
                    foreach (DataRow drv in lstPopupKU)
                    {
                        DANH_SACH_KHE_UOC_DU_PHONG objKUoc = new DANH_SACH_KHE_UOC_DU_PHONG();
                        foreach (DataColumn dtl in drv.Table.Columns)
                        {
                            PropertyInfo proper = objKUoc.GetType().GetProperty(dtl.ColumnName);
                            if (!LObject.IsNullOrEmpty(proper))
                            {
                                if (proper.PropertyType.Equals(typeof(int)))
                                    proper.SetValue(objKUoc, Convert.ToInt32(drv[dtl.ColumnName]), null);
                                else if (proper.PropertyType.Equals(typeof(decimal)))
                                    proper.SetValue(objKUoc, Convert.ToDecimal(drv[dtl.ColumnName]), null);
                                else
                                    proper.SetValue(objKUoc, drv[dtl.ColumnName].ToString(), null);
                            }
                        }
                        lstPopupDS.Add(objKUoc);
                    }
                    TDVMTRICHLAPDUPHONG.MA_DVI = ClientInformation.MaDonViGiaoDich;
                    if (!LObject.IsNullOrEmpty(TDVMTRICHLAPDUPHONG.DSACH_DPHONG_CHUNG))
                        TDVMTRICHLAPDUPHONG.DSACH_DPHONG_CHUNG = null;
                    TDVMTRICHLAPDUPHONG.DSACH_KHE_UOC = lstPopupDS.ToArray();
                    iret = new TinDungProcess().TinhToanTrichlapDuPhong(DatabaseConstant.Action.TINH_TOAN_TRICH_LAP_DU_PHONG_CU_THE, LDateTime.DateToString(teldtDenNgay.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat), ref TDVMTRICHLAPDUPHONG);
                    if (iret != 0)
                    {
                        lstDuPhongCuThe.AddRange(TDVMTRICHLAPDUPHONG.DSACH_KHE_UOC.ToList());
                        raddgrTrichLapDuPhong.ItemsSource = null;
                        raddgrTrichLapDuPhong.ItemsSource = lstDuPhongCuThe;
                    }
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
            raddgrTrichLapDuPhong.ItemsSource = null;
            raddgrTrichLapDuPhong.ItemsSource = lstDuPhongCuThe;
        }

        private void tlbXoaKUOC_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            try
            {
                if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                    return;
                foreach (DANH_SACH_KHE_UOC_DU_PHONG drv in raddgrTrichLapDuPhong.SelectedItems)
                {
                    lstDuPhongCuThe.Remove(drv);
                }
                raddgrTrichLapDuPhong.ItemsSource = null;
                raddgrTrichLapDuPhong.ItemsSource = lstDuPhongCuThe;
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        void raddgrTrichLapDuPhongChung_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            string UniqueName = e.Cell.Column.UniqueName;
            if(UniqueName.Equals("DU_PHONG_PHAI_TRICH"))
            {
                DANH_SACH_DU_PHONG_CHUNG objDuPhong = e.Cell.ParentRow.Item as DANH_SACH_DU_PHONG_CHUNG;
                decimal ChenhLech = objDuPhong.DU_PHONG_PHAI_TRICH - objDuPhong.DU_PHONG_DA_TRICH;
                lstDuPhongChung.ElementAt(lstDuPhongChung.IndexOf(objDuPhong)).CHENH_LECH = ChenhLech;
            }
        }

        void raddgrTrichLapDuPhong_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            string UniqueName = e.Cell.Column.UniqueName;
            if (UniqueName.Equals("DU_PHONG_PHAI_TRICH"))
            {
                DANH_SACH_KHE_UOC_DU_PHONG objDuPhong = e.Cell.ParentRow.Item as DANH_SACH_KHE_UOC_DU_PHONG;
                decimal ChenhLech = objDuPhong.DU_PHONG_PHAI_TRICH - objDuPhong.DU_PHONG_DA_TRICH;
                lstDuPhongCuThe.ElementAt(lstDuPhongCuThe.IndexOf(objDuPhong)).CHENH_LENH = ChenhLech;
            }
        }

        private void tlbCalReturn_Click(object sender, RoutedEventArgs e)
        {
            TinhToanDuPhongChung();
        }

        void chkDuPhongCuThe_Checked(object sender, RoutedEventArgs e)
        {
            if (chkDuPhongCuThe.IsChecked.GetValueOrDefault())
                grbDuPhongCuThe.Visibility = Visibility.Visible;
            else
                grbDuPhongCuThe.Visibility = Visibility.Collapsed;
        }

        void chkDuPhongChung_Checked(object sender, RoutedEventArgs e)
        {
            if (chkDuPhongChung.IsChecked.GetValueOrDefault())
                grbDuPhongChung.Visibility = Visibility.Visible;
            else
                grbDuPhongChung.Visibility = Visibility.Collapsed;
        }

        private void txtTKDuPhong_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                var txt = sender as TextBox;
                GridViewRow grrow = txt.ParentOfType<GridViewRow>();
                GetsTaiKhoan("DUPH02", grrow);
            }
        }

        private void btnTKDuPhong_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            GridViewRow grrow = btn.ParentOfType<GridViewRow>();
            GetsTaiKhoan("DUPH02", grrow);
        }

        private void txtTKChiPhi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                var txt = sender as TextBox;
                GridViewRow grrow = txt.ParentOfType<GridViewRow>();
                GetsTaiKhoan("CP_DPH_CHUNG", grrow);
            }
        }

        private void btnTKChiPhi_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            GridViewRow grrow = btn.ParentOfType<GridViewRow>();
            GetsTaiKhoan("CP_DPH_CHUNG#TNKHAC", grrow);
        }

        private void GetsTaiKhoan(string maKyHieu, GridViewRow grrow)
        {
            DANH_SACH_DU_PHONG_CHUNG drv = grrow.Item as DANH_SACH_DU_PHONG_CHUNG;
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
            lstDieuKien.Add(maKyHieu);
            lstDieuKien.Add(drv.NV_LOAI_NVON);
            lstPopupKU = new List<DataRow>();
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation("POPUP_TAIKHOANDUPHONG", lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            ucPopup popup = new ucPopup(false, simplePopupResponse, false);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Content = popup;
            win.ShowDialog();
            if (lstPopupKU.Count > 0)
            {
                if (maKyHieu.Equals("DUPH02"))
                {
                    drv.MA_SAN_PHAM = lstPopupKU[0]["SO_TAI_KHOAN"].ToString();
                    drv.DU_PHONG_DA_TRICH = Convert.ToDecimal(lstPopupKU[0]["SODU"]);
                    drv.CHENH_LECH = Convert.ToDecimal(drv.DU_PHONG_PHAI_TRICH) - Convert.ToDecimal(drv.DU_PHONG_DA_TRICH);
                }
                else
                    drv.TEN_SAN_PHAM = lstPopupKU[0]["SO_TAI_KHOAN"].ToString();
                raddgrTrichLapDuPhongChung.CurrentItem = drv;
            }
        }
        #endregion

        /// <summary>
        /// Xu ly nghiep vu
        /// </summary>
        #region Xu ly nghiep vu
        void TinhToanDuPhongChung()
        {
            int iret = 0;
            TDVMTRICHLAPDUPHONG.MA_DVI = ClientInformation.MaDonViGiaoDich;
            lstDuPhongChung.Clear();
            if (!LObject.IsNullOrEmpty(TDVMTRICHLAPDUPHONG.DSACH_KHE_UOC))
                TDVMTRICHLAPDUPHONG.DSACH_KHE_UOC = null;
            TDVMTRICHLAPDUPHONG.DSACH_DPHONG_CHUNG = lstDuPhongChung.ToArray();
            iret = new TinDungProcess().TinhToanTrichlapDuPhong(DatabaseConstant.Action.TINH_TOAN_TRICH_LAP_DU_PHONG_CHUNG, LDateTime.DateToString(teldtDenNgay.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat), ref TDVMTRICHLAPDUPHONG);
            if (iret != 0)
            {
                lstDuPhongChung = TDVMTRICHLAPDUPHONG.DSACH_DPHONG_CHUNG.ToList();
                raddgrTrichLapDuPhongChung.ItemsSource = null;
                raddgrTrichLapDuPhongChung.ItemsSource = lstDuPhongChung;
            }
            else
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
        }
        void GetDataForm(BusinessConstant.TrangThaiNghiepVu NghiepVu,BusinessConstant.TrangThaiBanGhi BanGhi)
        {
            if (LObject.IsNullOrEmpty(TDVMTRICHLAPDUPHONG)) TDVMTRICHLAPDUPHONG = new TDVM_TRICH_LAP_DU_PHONG();
            TDVMTRICHLAPDUPHONG.DU_PHONG_CHUNG = chkDuPhongChung.IsChecked.GetValueOrDefault(true) ? BusinessConstant.CoKhong.CO.layGiaTri() :
                BusinessConstant.CoKhong.KHONG.layGiaTri();
            TDVMTRICHLAPDUPHONG.DU_PHONG_CU_THE = chkDuPhongCuThe.IsChecked.GetValueOrDefault(true) ? BusinessConstant.CoKhong.CO.layGiaTri() :
                BusinessConstant.CoKhong.KHONG.layGiaTri();
            TDVMTRICHLAPDUPHONG.ID_GIAO_DICH = iDGiaoDich;
            TDVMTRICHLAPDUPHONG.LOAI_TIEN = ClientInformation.MaDongNoiTe;
            TDVMTRICHLAPDUPHONG.MA_DVI = ClientInformation.MaDonViGiaoDich;
            TDVMTRICHLAPDUPHONG.MA_GIAO_DICH = MaGiaoDich;
            TDVMTRICHLAPDUPHONG.NGAY_GIAO_DICH = LDateTime.DateToString(teldtNgayGiaoDich.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
            TDVMTRICHLAPDUPHONG.NGAY_TRICH_LAP = LDateTime.DateToString(teldtDenNgay.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
            TDVMTRICHLAPDUPHONG.LY_DO = TDVMTRICHLAPDUPHONG.DIEN_GIAI = txtDienGiai.Text;
            TDVMTRICHLAPDUPHONG.NGUOI_LAP = ClientInformation.TenDangNhap;
            TDVMTRICHLAPDUPHONG.NGAY_LAP = ClientInformation.NgayLamViecHienTai;
            TDVMTRICHLAPDUPHONG.TRANG_THAI_BAN_GHI = BanGhi.layGiaTri();
            TDVMTRICHLAPDUPHONG.TRANG_THAI_NGHIEP_VU = NghiepVu.layGiaTri();
            if (chkDuPhongChung.IsChecked.GetValueOrDefault())
            {
                lstDuPhongChung = (List<DANH_SACH_DU_PHONG_CHUNG>)raddgrTrichLapDuPhongChung.ItemsSource;
                TDVMTRICHLAPDUPHONG.DSACH_DPHONG_CHUNG = lstDuPhongChung.ToArray();
            }
            else
                TDVMTRICHLAPDUPHONG.DSACH_DPHONG_CHUNG = null;
            if (chkDuPhongCuThe.IsChecked.GetValueOrDefault())
            {
                lstDuPhongCuThe = (List<DANH_SACH_KHE_UOC_DU_PHONG>)raddgrTrichLapDuPhong.ItemsSource;
                TDVMTRICHLAPDUPHONG.DSACH_KHE_UOC = lstDuPhongCuThe.ToArray();
            }
            else
                TDVMTRICHLAPDUPHONG.DSACH_KHE_UOC = null;
            if (iDGiaoDich > 0)
            {
                TDVMTRICHLAPDUPHONG.NGUOI_LAP = txtNguoiLap.Text;
                TDVMTRICHLAPDUPHONG.NGAY_LAP = LDateTime.DateToString(teldtNgayNhap.Value.Value, ApplicationConstant.defaultDateTimeFormat);
                TDVMTRICHLAPDUPHONG.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                TDVMTRICHLAPDUPHONG.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
            }
        }

        void SetDataForm()
        {
            DataSet ds = new TinDungProcess().GetThongTinTrichLapDuPhong(TDVMTRICHLAPDUPHONG.MA_GIAO_DICH);
            SetThongTinChung(ds);
            SetThongTinKiemSoat(ds);
            OnModify();
            chkDuPhongChung_Checked(null, null);
            chkDuPhongCuThe_Checked(null, null);
        }

        void SetThongTinChung(DataSet ds)
        {
            if (LObject.IsNullOrEmpty(TDVMTRICHLAPDUPHONG)) TDVMTRICHLAPDUPHONG = new TDVM_TRICH_LAP_DU_PHONG();
            DataTable dt = null;
            if (!LObject.IsNullOrEmpty(ds) && ds.Tables.Count > 0)
            {
                dt = ds.Tables["TTIN_CTIET"];
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    foreach (DataColumn dtc in dr.Table.Columns)
                    {
                        PropertyInfo property = TDVMTRICHLAPDUPHONG.GetType().GetProperty(dtc.ColumnName);

                        if (property != null)
                        {
                            object GiaTri = null;
                            if (!((object)dt.Rows[0][dtc.ColumnName]).Equals(DBNull.Value))
                                GiaTri = dt.Rows[0][dtc.ColumnName];
                            property.SetValue(TDVMTRICHLAPDUPHONG, GiaTri, null);
                        }
                    }
                    iDGiaoDich = TDVMTRICHLAPDUPHONG.ID_GIAO_DICH;
                    MaGiaoDich = TDVMTRICHLAPDUPHONG.MA_GIAO_DICH;
                    txtSoGiaoDich.Text = MaGiaoDich;
                    txtDienGiai.Text = TDVMTRICHLAPDUPHONG.DIEN_GIAI;
                    teldtNgayGiaoDich.Value = LDateTime.StringToDate(TDVMTRICHLAPDUPHONG.NGAY_GIAO_DICH, ApplicationConstant.defaultDateTimeFormat);
                    teldtDenNgay.Value = LDateTime.StringToDate(TDVMTRICHLAPDUPHONG.NGAY_TRICH_LAP, ApplicationConstant.defaultDateTimeFormat);
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TDVMTRICHLAPDUPHONG.TRANG_THAI_NGHIEP_VU);
                    TThaiNVu = TDVMTRICHLAPDUPHONG.TRANG_THAI_NGHIEP_VU;
                    dt = ds.Tables["CHUNG"];
                    if (dt.Rows.Count > 0)
                    {
                        chkDuPhongChung.IsChecked = true;
                        if (LObject.IsNullOrEmpty(lstDuPhongChung)) lstDuPhongChung = new List<DANH_SACH_DU_PHONG_CHUNG>();
                        lstDuPhongChung.Clear();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DANH_SACH_DU_PHONG_CHUNG objDPhongChung = new DANH_SACH_DU_PHONG_CHUNG();
                            foreach (DataColumn dtc in dt.Columns)
                            {
                                PropertyInfo property = objDPhongChung.GetType().GetProperty(dtc.ColumnName);

                                if (property != null)
                                {
                                    object GiaTri = null;
                                    if (!((object)dt.Rows[i][dtc.ColumnName]).Equals(DBNull.Value))
                                        GiaTri = dt.Rows[i][dtc.ColumnName];
                                    if (property.PropertyType.Equals(typeof(int)))
                                        property.SetValue(objDPhongChung, Convert.ToInt32(GiaTri), null);
                                    else if (property.PropertyType.Equals(typeof(decimal)))
                                        property.SetValue(objDPhongChung, Convert.ToDecimal(GiaTri), null);
                                    else
                                        property.SetValue(objDPhongChung, GiaTri, null);
                                }
                            }
                            lstDuPhongChung.Add(objDPhongChung);
                        }
                        raddgrTrichLapDuPhongChung.ItemsSource = null;
                        raddgrTrichLapDuPhongChung.ItemsSource = lstDuPhongChung;
                    }
                    else
                        chkDuPhongChung.IsChecked = false;
                    dt = ds.Tables["CUTHE"];
                    if (dt.Rows.Count > 0)
                    {
                        chkDuPhongCuThe.IsChecked = true;
                        if (LObject.IsNullOrEmpty(lstDuPhongCuThe)) lstDuPhongCuThe = new List<DANH_SACH_KHE_UOC_DU_PHONG>();
                        lstDuPhongCuThe.Clear();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DANH_SACH_KHE_UOC_DU_PHONG objDPhongCThe = new DANH_SACH_KHE_UOC_DU_PHONG();
                            foreach (DataColumn dtc in dt.Columns)
                            {
                                PropertyInfo property = objDPhongCThe.GetType().GetProperty(dtc.ColumnName);

                                if (property != null)
                                {
                                    object GiaTri = null;
                                    if (!((object)dt.Rows[i][dtc.ColumnName]).Equals(DBNull.Value))
                                        GiaTri = dt.Rows[i][dtc.ColumnName];
                                    if (property.PropertyType.Equals(typeof(int)))
                                        property.SetValue(objDPhongCThe, Convert.ToInt32(GiaTri), null);
                                    else if (property.PropertyType.Equals(typeof(decimal)))
                                        property.SetValue(objDPhongCThe, Convert.ToDecimal(GiaTri), null);
                                    else
                                        property.SetValue(objDPhongCThe, GiaTri, null);
                                }
                            }
                            lstDuPhongCuThe.Add(objDPhongCThe);
                        }
                        raddgrTrichLapDuPhong.ItemsSource = null;
                        raddgrTrichLapDuPhong.ItemsSource = lstDuPhongCuThe;
                    }
                    else
                        chkDuPhongCuThe.IsChecked = false;
                }
            }
        }

        void SetThongTinKiemSoat(DataSet ds)
        {
            DataTable dt = ds.Tables["TTIN_CTIET"];
            if (dt != null && dt.Rows.Count > 0)
            {
                txtNguoiLap.Text = dt.Rows[0]["NGUOI_LAP"].ToString();
                txtNguoiCapNhat.Text = dt.Rows[0]["NGUOI_CAP_NHAT"].ToString();
                teldtNgayNhap.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_LAP"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                if (!dt.Rows[0]["NGAY_CAP_NHAT"].ToString().IsNullOrEmptyOrSpace())
                    teldtNgayCNhat.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_CAP_NHAT"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(dt.Rows[0]["TRANG_THAI_BAN_GHI"].ToString());
            }
        }

        void BeforeSave(BusinessConstant.TrangThaiNghiepVu nghiepvu, BusinessConstant.TrangThaiBanGhi banghi)
        {
            Cursor = Cursors.Wait;
            try
            {
                if (nghiepvu != BusinessConstant.TrangThaiNghiepVu.LUU_TAM)
                {
                    if (!Vadidation())
                        return;
                }
                GetDataForm(nghiepvu, banghi);
                OnSave();
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
            
        }

        void OnSave()
        {
            try
            {
                List<ClientResponseDetail> lstResponse = new List<ClientResponseDetail>();
                int iret = 0;
                if (iDGiaoDich == 0)
                    iret = new TinDungProcess().ThemMoiTrichLapTinDungViMo(ref TDVMTRICHLAPDUPHONG, ref lstResponse);
                else
                    iret = new TinDungProcess().SuaTrichLapTinDungViMo(ref TDVMTRICHLAPDUPHONG, ref lstResponse);
                AfterSave(lstResponse, iret);
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void AfterSave(List<ClientResponseDetail> lstResponse, int iret)
        {
            try
            {
                CommonFunction.ThongBaoKetQua(lstResponse);
                if (iret > 0)
                {
                    action = DatabaseConstant.Action.XEM;
                    List<int> lstId = new List<int>();
                    lstId.Add(TDVMTRICHLAPDUPHONG.ID_GIAO_DICH);
                    // Yêu cầu Unlock dữ liệu
                    UtilitiesProcess process = new UtilitiesProcess();
                    bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                    DatabaseConstant.Function.TDVM_TRICH_LAP_DU_PHONG,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.SUA,
                    lstId);
                    if (LObject.IsNullOrEmpty(_objKiemSoat)) _objKiemSoat = new KIEM_SOAT();
                    _objKiemSoat.ID = TDVMTRICHLAPDUPHONG.ID_GIAO_DICH;
                    _objKiemSoat.SO_GIAO_DICH = TDVMTRICHLAPDUPHONG.MA_GIAO_DICH;
                    MaGiaoDich = TDVMTRICHLAPDUPHONG.MA_GIAO_DICH;
                    iDGiaoDich = TDVMTRICHLAPDUPHONG.ID_GIAO_DICH;
                    SetDataForm();
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, TThaiNVu, mnuMain);
                    if (OnSavingCompleted != null)
                        OnSavingCompleted(null, EventArgs.Empty);
                    if ((bool)cbMultiAdd.IsChecked)
                        ClearForm();
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            
        }

        private void OnPreview()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(iDGiaoDich) && LObject.IsNullOrEmpty(MaGiaoDich))
            {
                LMessage.ShowMessage("M.TinDungTT.SanPham.ucDangKySanPhamCT.KhongCoThongTinSo", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();

                GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.GDKT_IN_GIAO_DICH;
                objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDVM_TRICH_LAP_DU_PHONG;

                GDKT_GIAO_DICH objGDKT_GIAO_DICH = new GDKT_GIAO_DICH();
                objGDKT_GIAO_DICH.MaGiaoDich = MaGiaoDich;

                doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                doiTuongBaoCao.objGDKT_GIAO_DICH = objGDKT_GIAO_DICH;
                xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
            }
        }

        bool Vadidation()
        {
            if (txtDienGiai.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblDienGiai.Content.ToString());
                txtDienGiai.Focus();
                return false;
            }
            else if (chkDuPhongCuThe.IsChecked.GetValueOrDefault(true) && (lstDuPhongCuThe.Count<1 || LObject.IsNullOrEmpty(raddgrTrichLapDuPhong.ItemsSource)))
            {
                CommonFunction.ThongBaoTrong(grbDuPhongCuThe.Header.ToString()+":");
                tlbThemKUOC.Focus();
                return false;
            }
            else if (chkDuPhongChung.IsChecked.GetValueOrDefault(true) && (lstDuPhongChung.Count < 1 || LObject.IsNullOrEmpty(raddgrTrichLapDuPhongChung.ItemsSource)))
            {
                CommonFunction.ThongBaoTrong(grbDuPhongChung.Header.ToString() + ":");
                tlbCalReturn.Focus();
                return false;
            }
            return true;
        }

        void BeforeDelete()
        {
            Cursor = Cursors.Wait;
            try
            {
                if (iDGiaoDich > 0)
                {
                    List<int> lstId = new List<int>();
                    lstId.Add(iDGiaoDich);
                    // Yêu cầu Unlock dữ liệu
                    UtilitiesProcess process = new UtilitiesProcess();
                    bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                    DatabaseConstant.Function.TDVM_TRICH_LAP_DU_PHONG,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.XOA,
                    lstId);
                    OnDelete();
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
            
        }

        void OnDelete()
        {
            try
            {
                if (iDGiaoDich > 0)
                {
                    TDVMTRICHLAPDUPHONG.ID_GIAO_DICH = iDGiaoDich;
                    TDVMTRICHLAPDUPHONG.MA_GIAO_DICH = MaGiaoDich;
                    TDVMTRICHLAPDUPHONG.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                    TDVMTRICHLAPDUPHONG.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                    List<ClientResponseDetail> lstClientDetail = new List<ClientResponseDetail>();
                    int iret = new TinDungProcess().XoaTrichLapTinDungViMo(ref TDVMTRICHLAPDUPHONG, ref lstClientDetail);
                    AfterDelete(lstClientDetail, iret);
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void AfterDelete(List<ClientResponseDetail> lstResponse, int iret)
        {
            try
            {
                CommonFunction.ThongBaoKetQua(lstResponse);
                if (iret > 0)
                {
                    List<int> lstId = new List<int>();
                    lstId.Add(TDVMTRICHLAPDUPHONG.ID_GIAO_DICH);
                    // Yêu cầu Unlock dữ liệu
                    UtilitiesProcess process = new UtilitiesProcess();
                    bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                    DatabaseConstant.Function.TDVM_TRICH_LAP_DU_PHONG,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.XOA,
                    lstId);
                    if (OnSavingCompleted != null)
                        OnSavingCompleted(null, EventArgs.Empty);
                    if (iret > 0)
                    {
                        CommonFunction.CloseUserControl(this);
                    }
                    SetDataForm();
                    if ((bool)cbMultiAdd.IsChecked)
                        ClearForm();
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void BeforeApprove()
        {
            Cursor = Cursors.Wait;
            try
            {
                if (iDGiaoDich > 0)
                {
                    List<int> lstId = new List<int>();
                    lstId.Add(iDGiaoDich);
                    // Yêu cầu Unlock dữ liệu
                    UtilitiesProcess process = new UtilitiesProcess();
                    bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                    DatabaseConstant.Function.TDVM_TRICH_LAP_DU_PHONG,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.DUYET,
                    lstId);
                    OnApprove();
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        void OnApprove()
        {
            try
            {
                if (iDGiaoDich > 0)
                {
                    TDVMTRICHLAPDUPHONG.ID_GIAO_DICH = iDGiaoDich;
                    TDVMTRICHLAPDUPHONG.MA_GIAO_DICH = MaGiaoDich;
                    TDVMTRICHLAPDUPHONG.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                    TDVMTRICHLAPDUPHONG.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                    List<ClientResponseDetail> lstClientDetail = new List<ClientResponseDetail>();
                    int iret = new TinDungProcess().DuyetTrichLapTinDungViMo(ref TDVMTRICHLAPDUPHONG, ref lstClientDetail);
                    AfterApprove(lstClientDetail, iret);
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void AfterApprove(List<ClientResponseDetail> lstResponse, int iret)
        {
            try
            {
                CommonFunction.ThongBaoKetQua(lstResponse);
                if (iret > 0)
                {
                    action = DatabaseConstant.Action.XEM;
                    List<int> lstId = new List<int>();
                    lstId.Add(TDVMTRICHLAPDUPHONG.ID_GIAO_DICH);
                    // Yêu cầu Unlock dữ liệu
                    UtilitiesProcess process = new UtilitiesProcess();
                    bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                    DatabaseConstant.Function.TDVM_TRICH_LAP_DU_PHONG,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.DUYET,
                    lstId);
                    SetDataForm();
                    CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain);
                    if (OnSavingCompleted != null)
                        OnSavingCompleted(null, EventArgs.Empty);
                    if ((bool)cbMultiAdd.IsChecked)
                        ClearForm();
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void BeforeRefuse()
        {
            Cursor = Cursors.Wait;
            try
            {
                if (iDGiaoDich > 0)
                {
                    List<int> lstId = new List<int>();
                    lstId.Add(iDGiaoDich);
                    // Yêu cầu Unlock dữ liệu
                    UtilitiesProcess process = new UtilitiesProcess();
                    bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                    DatabaseConstant.Function.TDVM_TRICH_LAP_DU_PHONG,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    lstId);
                    OnRefuse();
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        void OnRefuse()
        {
            try
            {
                if (iDGiaoDich > 0)
                {
                    TDVMTRICHLAPDUPHONG.ID_GIAO_DICH = iDGiaoDich;
                    TDVMTRICHLAPDUPHONG.MA_GIAO_DICH = MaGiaoDich;
                    TDVMTRICHLAPDUPHONG.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                    TDVMTRICHLAPDUPHONG.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                    List<ClientResponseDetail> lstClientDetail = new List<ClientResponseDetail>();
                    int iret = new TinDungProcess().TuChoiDuyetTrichLapTinDungViMo(ref TDVMTRICHLAPDUPHONG, ref lstClientDetail);
                    AfterRefuse(lstClientDetail, iret);
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void AfterRefuse(List<ClientResponseDetail> lstResponse, int iret)
        {
            try
            {
                CommonFunction.ThongBaoKetQua(lstResponse);
                if (iret > 0)
                {
                    action = DatabaseConstant.Action.XEM;
                    List<int> lstId = new List<int>();
                    lstId.Add(TDVMTRICHLAPDUPHONG.ID_GIAO_DICH);
                    // Yêu cầu Unlock dữ liệu
                    UtilitiesProcess process = new UtilitiesProcess();
                    bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                    DatabaseConstant.Function.TDVM_TRICH_LAP_DU_PHONG,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    lstId);
                    SetDataForm();
                    CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain);
                    if (OnSavingCompleted != null)
                        OnSavingCompleted(null, EventArgs.Empty);
                    if ((bool)cbMultiAdd.IsChecked)
                        ClearForm();
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void BeforeCancel()
        {
            Cursor = Cursors.Wait;
            try
            {
                if (iDGiaoDich > 0)
                {
                    List<int> lstId = new List<int>();
                    lstId.Add(iDGiaoDich);
                    // Yêu cầu Unlock dữ liệu
                    UtilitiesProcess process = new UtilitiesProcess();
                    bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                    DatabaseConstant.Function.TDVM_TRICH_LAP_DU_PHONG,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.THOAI_DUYET,
                    lstId);
                    OnCancel();
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        void OnCancel()
        {
            try
            {
                if (iDGiaoDich > 0)
                {
                    TDVMTRICHLAPDUPHONG.ID_GIAO_DICH = iDGiaoDich;
                    TDVMTRICHLAPDUPHONG.MA_GIAO_DICH = MaGiaoDich;
                    TDVMTRICHLAPDUPHONG.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                    TDVMTRICHLAPDUPHONG.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                    List<ClientResponseDetail> lstClientDetail = new List<ClientResponseDetail>();
                    int iret = new TinDungProcess().ThoaiDuyetTrichLapTinDungViMo(ref TDVMTRICHLAPDUPHONG, ref lstClientDetail);
                    AfterCancel(lstClientDetail, iret);
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void AfterCancel(List<ClientResponseDetail> lstResponse, int iret)
        {
            try
            {
                CommonFunction.ThongBaoKetQua(lstResponse);
                if (iret > 0)
                {
                    action = DatabaseConstant.Action.XEM;
                    List<int> lstId = new List<int>();
                    lstId.Add(TDVMTRICHLAPDUPHONG.ID_GIAO_DICH);
                    // Yêu cầu Unlock dữ liệu
                    UtilitiesProcess process = new UtilitiesProcess();
                    bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                    DatabaseConstant.Function.TDVM_TRICH_LAP_DU_PHONG,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.THOAI_DUYET,
                    lstId);
                    SetDataForm();
                    CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain);
                    if (OnSavingCompleted != null)
                        OnSavingCompleted(null, EventArgs.Empty);
                    if ((bool)cbMultiAdd.IsChecked)
                        ClearForm();
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void OnModify()
        {
            if (action.Equals(DatabaseConstant.Action.SUA))
                EnableAllControl(true);
            else
                EnableAllControl(false);
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu,mnuMain);
        }

        void EnableAllControl(bool bBool)
        {
            teldtDenNgay.IsEnabled = bBool;
            dtpDenNgay.IsEnabled = bBool;
            grbThongTinChung.IsEnabled = bBool;
            raddgrTrichLapDuPhong.IsReadOnly = !bBool;
            raddgrTrichLapDuPhongChung.IsReadOnly = !bBool;
            tlbCalReturn.IsEnabled = bBool;
            tlbThemKUOC.IsEnabled = bBool;
            tlbXoaKUOC.IsEnabled = bBool;
        }

        void ReleaseForm()
        {
            if (iDGiaoDich > 0)
            {
                List<int> lstId = new List<int>();
                lstId.Add(TDVMTRICHLAPDUPHONG.ID_GIAO_DICH);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_TRICH_LAP_DU_PHONG,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                lstId);
            }
        }

        #endregion

        private void teldtDenNgay_LostFocus(object sender, RoutedEventArgs e)
        {
            clearKheUoc();
        }

        private void clearKheUoc()
        {
            if (teldtDenNgay.Text.IsDate("dd/mm/yyyy"))
            {
                TinhToanDuPhongChung();
                raddgrTrichLapDuPhong.ItemsSource = null;
            }
        }
    }
}
