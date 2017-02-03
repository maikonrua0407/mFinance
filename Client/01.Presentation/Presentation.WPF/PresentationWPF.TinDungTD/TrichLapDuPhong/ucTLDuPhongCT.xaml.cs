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
using System.Reflection;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;
using Telerik.Windows.Controls.GridView;
using Presentation.Process.TinDungTDServiceRef;

namespace PresentationWPF.TinDungTD.TrichLapDuPhong
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
        TDTD_TRICH_LAP_DU_PHONG TDVMTRICHLAPDUPHONG = null;
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
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDung;component/TrichLapDuPhong/ucTLDuPhongCT.xaml", ref Toolbar, ref mnuMain);
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
            TDVMTRICHLAPDUPHONG.MA_GIAO_DICH = _objKiemSoat.SO_GIAO_DICH;
            TDVMTRICHLAPDUPHONG.ID_GIAO_DICH = _objKiemSoat.ID;
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
            btnThemKUoc.Click += new RoutedEventHandler(btnThemKUoc_Click);
            btnTinhToan.Click += new RoutedEventHandler(btnTinhToan_Click);
            tlbCalReturn.Click +=new RoutedEventHandler(tlbCalReturn_Click);
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
            TDVMTRICHLAPDUPHONG = new TDTD_TRICH_LAP_DU_PHONG();
            lstDuPhongChung = new List<DANH_SACH_DU_PHONG_CHUNG>();
            lstDuPhongCuThe = new List<DANH_SACH_KHE_UOC_DU_PHONG>();
            raddgrTrichLapDuPhong.ItemsSource = lstDuPhongCuThe;
            raddgrTrichLapDuPhongChung.ItemsSource = lstDuPhongChung;
            TDVMTRICHLAPDUPHONG.NGAY_GIAO_DICH = ClientInformation.NgayLamViecHienTai;
            TDVMTRICHLAPDUPHONG.MA_DVI_QLY = ClientInformation.MaDonVi;
            TDVMTRICHLAPDUPHONG.MA_DVI_GD = ClientInformation.MaDonViGiaoDich;
            TDVMTRICHLAPDUPHONG.MA_DVI = ClientInformation.MaDonViGiaoDich;
            TDVMTRICHLAPDUPHONG.LOAI_TIEN = ClientInformation.MaDongNoiTe;
            TDVMTRICHLAPDUPHONG.NGAY_LAP = ClientInformation.NgayLamViecHienTai;
            TDVMTRICHLAPDUPHONG.NGUOI_LAP = ClientInformation.TenDangNhap;
            TDVMTRICHLAPDUPHONG.TEN_NGUOI_GDICH = ClientInformation.HoTen;
            TDVMTRICHLAPDUPHONG.NGAY_TRICH_LAP = ClientInformation.NgayLamViecHienTai;
            TDVMTRICHLAPDUPHONG.DU_PHONG_CHUNG = BusinessConstant.CoKhong.KHONG.layGiaTri();
            TDVMTRICHLAPDUPHONG.DU_PHONG_CU_THE = BusinessConstant.CoKhong.CO.layGiaTri();
            grbDuPhongChung.Visibility = System.Windows.Visibility.Collapsed;
            chkDuPhongChung.Visibility = System.Windows.Visibility.Collapsed;
            chkDuPhongCuThe.Visibility = System.Windows.Visibility.Collapsed;
            grMain.DataContext = TDVMTRICHLAPDUPHONG;
            TThaiNVu = "";
            txtNguoiCapNhat.Text = "";
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            txtTrangThai.Text = "";
            lblTrangThai.Content = "";
            teldtNgayCNhat.Value = null;
            OnCalculator();
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, TThaiNVu,mnuMain,DatabaseConstant.Function.TDTD_TRICH_LAP_DP);
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
                    throw new System.NullReferenceException("M.TinDung.TrichLapDuPhong.ucTLDuPhongCT.KhongTimThayControl" + sbControl.ToString());
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
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
            if (OnCalculator())
                LMessage.ShowMessage("M.DungChung.Result.ThanhCong", LMessage.MessageBoxType.Information);
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


        void btnTinhToan_Click(object sender, RoutedEventArgs e)
        {
            OnCalculatorDetail();
        }

        void btnThemKUoc_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            try
            {
                if (lstDuPhongCuThe.IsNullOrEmpty())
                    return;
                lstPopupKU.Clear();
                string ngayDuThu = teldtDenNgay.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add(ngayDuThu);
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DSACH_KUOC_TRICH_LAP_TDTD", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(false, simplePopupResponse, true);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.Content = popup;
                win.ShowDialog();
                DANH_SACH_KHE_UOC_DU_PHONG objTTinCT = null;
                if (lstPopupKU.Count > 0)
                {
                    List<int> lstID = lstDuPhongCuThe.Select(f => f.ID_KHE_UOC).ToList();
                    foreach (DataRow dr in lstPopupKU)
                    {
                        if (lstID.Contains(Convert.ToInt32(dr["ID_KUOC"])))
                        {
                            continue;
                        }
                        objTTinCT = new DANH_SACH_KHE_UOC_DU_PHONG();
                        objTTinCT.CHENH_LENH = 0;
                        objTTinCT.DU_PHONG_DA_TRICH = Convert.ToDecimal(dr["SO_TIEN_TLDP"]);
                        objTTinCT.DU_PHONG_NHOM_CU = Convert.ToDecimal(dr["DU_PHONG_NHOM_CU"]);
                        objTTinCT.DU_PHONG_PHAI_TRICH = Convert.ToDecimal(dr["DU_PHONG_PHAI_TRICH"]);
                        objTTinCT.ID_DON_VI = Convert.ToInt32(dr["ID_DON_VI"]);
                        objTTinCT.ID_KHACH_HANG = Convert.ToInt32(dr["ID_KHANG"]);
                        objTTinCT.ID_KHE_UOC = Convert.ToInt32(dr["ID_KUOC"]);
                        objTTinCT.ID_SAN_PHAM = Convert.ToInt32(dr["ID_SAN_PHAM"]);
                        objTTinCT.LAI_SUAT = Convert.ToDecimal(dr["LAI_SUAT"]);
                        objTTinCT.LOAI_TIEN = dr["LOAI_TIEN"].ToString();
                        objTTinCT.MA_DON_VI = dr["MA_DVI_TAO"].ToString();
                        objTTinCT.MA_KHACH_HANG = dr["MA_KHANG"].ToString();
                        objTTinCT.MA_KHE_UOC = dr["MA_KUOC"].ToString();
                        objTTinCT.MA_SAN_PHAM = dr["MA_SAN_PHAM"].ToString();
                        objTTinCT.NGAY_VAY = dr["NGAY_GIAI_NGAN"].ToString();
                        objTTinCT.NHOM_NO = dr["NHOM_NO_HIEN_TAI"].ToString();
                        objTTinCT.NHOM_NO_CU = dr["NHOM_NO_CU"].ToString();
                        objTTinCT.NV_LOAI_NVON = dr["NV_LOAI_NVON"].ToString();
                        objTTinCT.SO_DU = Convert.ToDecimal(dr["SO_DU"]);
                        objTTinCT.SO_KHE_UOC = dr["SO_KUOC"].ToString();
                        objTTinCT.SO_TIEN_VAY = Convert.ToDecimal(dr["SO_TIEN_GIAI_NGAN"]);
                        objTTinCT.TEN_KHACH_HANG = dr["TEN_KHANG"].ToString();
                        lstDuPhongCuThe.Add(objTTinCT);
                        raddgrTrichLapDuPhong.Rebind();
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
   
        #endregion

        /// <summary>
        /// Xu ly nghiep vu
        /// </summary>
        #region Xu ly nghiep vu
        void GetDataForm(BusinessConstant.TrangThaiNghiepVu NghiepVu)
        {
            if (TDVMTRICHLAPDUPHONG.ID_GIAO_DICH > 0)
            {
                TDVMTRICHLAPDUPHONG.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                TDVMTRICHLAPDUPHONG.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
            }
            TDVMTRICHLAPDUPHONG.TRANG_THAI_BAN_GHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
            TDVMTRICHLAPDUPHONG.TRANG_THAI_NGHIEP_VU = NghiepVu.layGiaTri();
            TDVMTRICHLAPDUPHONG.DSACH_DPHONG_CHUNG = lstDuPhongChung.ToArray();
            TDVMTRICHLAPDUPHONG.DSACH_KHE_UOC = lstDuPhongCuThe.ToArray();
        }

        void SetDataForm()
        {
            TinDungTDProcess processTinDungTD = new TinDungTDProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                int ret = 0;
                ret = processTinDungTD.TrichLapDuPhong(DatabaseConstant.Action.LOAD_DATA, ref TDVMTRICHLAPDUPHONG, ref listClientResponseDetail);
                if (ret > 0)
                {
                    lstDuPhongChung = TDVMTRICHLAPDUPHONG.DSACH_DPHONG_CHUNG.ToList();
                    lstDuPhongCuThe = TDVMTRICHLAPDUPHONG.DSACH_KHE_UOC.ToList();
                    raddgrTrichLapDuPhongChung.ItemsSource = lstDuPhongChung;
                    raddgrTrichLapDuPhong.ItemsSource = lstDuPhongCuThe;
                    grMain.DataContext = TDVMTRICHLAPDUPHONG;
                    TThaiNVu = TDVMTRICHLAPDUPHONG.TRANG_THAI_NGHIEP_VU;
                    lblLabelTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
                    CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDTD_THU_GOC_LAI);
                    if (action.Equals(DatabaseConstant.Action.SUA))
                        EnableAllControl(true);
                    else
                        EnableAllControl(false);
                    SetTabThongKiemSoat();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M_ResponseMessage_HanhDong_LayDuLieuKhongThanhCong", LMessage.MessageBoxType.Warning);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        void SetTabThongKiemSoat()
        {
            try
            {
                if (!LObject.IsNullOrEmpty(TDVMTRICHLAPDUPHONG))
                {
                    txtNguoiLap.Text = TDVMTRICHLAPDUPHONG.NGUOI_LAP;
                    teldtNgayNhap.Value = LDateTime.StringToDate(TDVMTRICHLAPDUPHONG.NGAY_LAP, ApplicationConstant.defaultDateTimeFormat);
                    txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(TDVMTRICHLAPDUPHONG.TRANG_THAI_BAN_GHI);
                    txtNguoiCapNhat.Text = TDVMTRICHLAPDUPHONG.NGUOI_CAP_NHAT;
                    if (!TDVMTRICHLAPDUPHONG.NGAY_CAP_NHAT.IsNullOrEmptyOrSpace())
                        teldtNgayCNhat.Value = LDateTime.StringToDate(TDVMTRICHLAPDUPHONG.NGAY_CAP_NHAT, ApplicationConstant.defaultDateTimeFormat);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
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
                GetDataForm(nghiepvu);
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
                if (TDVMTRICHLAPDUPHONG.ID_GIAO_DICH == 0)
                    iret = new TinDungTDProcess().TrichLapDuPhong(DatabaseConstant.Action.THEM, ref TDVMTRICHLAPDUPHONG, ref lstResponse);
                else
                    iret = new TinDungTDProcess().TrichLapDuPhong(DatabaseConstant.Action.SUA, ref TDVMTRICHLAPDUPHONG, ref lstResponse);
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
                    DatabaseConstant.Function.TDTD_TRICH_LAP_DP,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.SUA,
                    lstId);
                    BeforeViewFromDetail();
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
            Mouse.OverrideCursor = Cursors.Wait;
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(txtSoGiaoDich.Text))
            {
                LMessage.ShowMessage("Không có thông tin dữ liệu cần hiển thị", LMessage.MessageBoxType.Warning);

            }
            else
            {

                VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();

                GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.GDKT_IN_GIAO_DICH;
                objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDTD_TRICH_LAP_DP;

                GDKT_GIAO_DICH objGDKT_GIAO_DICH = new GDKT_GIAO_DICH();
                objGDKT_GIAO_DICH.MaGiaoDich = txtSoGiaoDich.Text;

                doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                doiTuongBaoCao.objGDKT_GIAO_DICH = objGDKT_GIAO_DICH;

                List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
                listThamSoBaoCao.Add(new ThamSoBaoCao("@SoPhieu", txtSoGiaoDich.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                string maBaoCao = "GDKT_GIAO_DICH";
                xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);

            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        bool Vadidation()
        {
            bool kq = true;
            List<ClientResponseDetail> listClientResponseDetail = null;
            ClientResponseDetail objClientResponseDetail = null;

            if (txtDienGiai.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblDienGiai.Content.ToString());
                txtDienGiai.Focus();
                return false;
            }
            else if (chkDuPhongCuThe.IsChecked.GetValueOrDefault(true) && (lstDuPhongCuThe.Count<1 || LObject.IsNullOrEmpty(raddgrTrichLapDuPhong.ItemsSource)))
            {
                CommonFunction.ThongBaoTrong(grbDuPhongCuThe.Header.ToString()+":");
                btnThemKUoc.Focus();
                return false;
            }
            else if (chkDuPhongChung.IsChecked.GetValueOrDefault(true) && (lstDuPhongChung.Count < 1 || LObject.IsNullOrEmpty(raddgrTrichLapDuPhongChung.ItemsSource)))
            {
                CommonFunction.ThongBaoTrong(grbDuPhongChung.Header.ToString() + ":");
                tlbCalReturn.Focus();
                return false;
            }
            if (chkDuPhongCuThe.IsChecked.GetValueOrDefault(true))
            {
                if (!lstDuPhongCuThe.Where(f => f.CHENH_LENH == 0).IsNullOrEmpty() && lstDuPhongCuThe.Where(f => f.CHENH_LENH == 0).Count() > 0)
                {
                    if (listClientResponseDetail.IsNullOrEmpty()) listClientResponseDetail = new List<ClientResponseDetail>();
                    foreach (DANH_SACH_KHE_UOC_DU_PHONG objChuyenNo in lstDuPhongCuThe.Where(f => f.CHENH_LENH == 0))
                    {
                        objClientResponseDetail = new ClientResponseDetail();
                        objClientResponseDetail.Detail = LLanguage.SearchResourceByKey("M_ResponseMessage_KUOC_ChuaTinhTrichLapDuPhong");
                        objClientResponseDetail.Id = objChuyenNo.ID_KHE_UOC;
                        objClientResponseDetail.Object = objChuyenNo.MA_KHE_UOC;
                        objClientResponseDetail.Operation = DatabaseConstant.Action.KIEM_TRA.layNgonNgu();
                        objClientResponseDetail.Result = ApplicationConstant.OperationStatus.Failed.layGiaTri();
                        listClientResponseDetail.Add(objClientResponseDetail);
                    }

                    kq = false;
                    if (!kq)
                    {
                        CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                        raddgrTrichLapDuPhong.Focus();
                        return kq;
                    }
                }
            }
            if (chkDuPhongChung.IsChecked.GetValueOrDefault(true))
            {
                if (!lstDuPhongChung.Where(f => f.CHENH_LECH == 0).IsNullOrEmpty() && lstDuPhongChung.Where(f => f.CHENH_LECH == 0).Count() > 0)
                {
                    CommonFunction.ThongBaoTrong(LLanguage.SearchResourceByKey("M_ResponseMessage_KUOC_ChuaTinhTrichLapDuPhongChung") + ":");
                    raddgrTrichLapDuPhongChung.Focus();
                    return false;
                }
            }

            if (lstDuPhongChung.IsNullOrEmpty()) lstDuPhongChung = new List<DANH_SACH_DU_PHONG_CHUNG>();
            if (lstDuPhongCuThe.IsNullOrEmpty()) lstDuPhongCuThe = new List<DANH_SACH_KHE_UOC_DU_PHONG>();

            if (lstDuPhongChung.Sum(f => Math.Abs(f.CHENH_LECH)) + lstDuPhongCuThe.Sum(f => Math.Abs(f.CHENH_LENH)) == 0)
            {
                CommonFunction.ThongBaoTrong(LLanguage.SearchResourceByKey("M_ResponseMessage_TLDP_KhongCoThongTinTrichLap") + ":");
                txtDienGiai.Focus();
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
                    DatabaseConstant.Function.TDTD_TRICH_LAP_DP,
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
                    int iret = new TinDungTDProcess().TrichLapDuPhong(DatabaseConstant.Action.XOA,ref TDVMTRICHLAPDUPHONG, ref lstClientDetail);
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
                    DatabaseConstant.Function.TDTD_TRICH_LAP_DP,
                    DatabaseConstant.Table.KT_GIAO_DICH,
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
                    DatabaseConstant.Function.TDTD_TRICH_LAP_DP,
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
                    int iret = new TinDungTDProcess().TrichLapDuPhong(DatabaseConstant.Action.DUYET,ref TDVMTRICHLAPDUPHONG, ref lstClientDetail);
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
                    DatabaseConstant.Function.TDTD_TRICH_LAP_DP,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.DUYET,
                    lstId);
                    BeforeViewFromDetail();
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
                    DatabaseConstant.Function.TDTD_TRICH_LAP_DP,
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
                    int iret = new TinDungTDProcess().TrichLapDuPhong(DatabaseConstant.Action.TU_CHOI_DUYET,ref TDVMTRICHLAPDUPHONG, ref lstClientDetail);
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
                    DatabaseConstant.Function.TDTD_TRICH_LAP_DP,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    lstId);
                    BeforeViewFromDetail();
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
                    DatabaseConstant.Function.TDTD_TRICH_LAP_DP,
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
                    int iret = new TinDungTDProcess().TrichLapDuPhong(DatabaseConstant.Action.THOAI_DUYET,ref TDVMTRICHLAPDUPHONG, ref lstClientDetail);
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
                    DatabaseConstant.Function.TDTD_TRICH_LAP_DP,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.THOAI_DUYET,
                    lstId);
                    BeforeViewFromDetail();
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
            btnThemKUoc.IsEnabled = bBool;
            btnTinhToan.IsEnabled = bBool;
            btnXoaKUoc.IsEnabled = bBool;
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
                DatabaseConstant.Function.TDTD_TRICH_LAP_DP,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                lstId);
            }
        }

        public void BeforeViewFromDetail()
        {
            action = DatabaseConstant.Action.XEM;
            TThaiNVu = TDVMTRICHLAPDUPHONG.TRANG_THAI_NGHIEP_VU;
            iDGiaoDich = TDVMTRICHLAPDUPHONG.ID_GIAO_DICH;
            EnableAllControl(false);
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDTD_TRICH_LAP_DP);
            TThaiNVu = TDVMTRICHLAPDUPHONG.TRANG_THAI_NGHIEP_VU;
            lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
            grMain.DataContext = TDVMTRICHLAPDUPHONG;
            SetTabThongKiemSoat();
        }

        public bool OnCalculator()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TinDungTDProcess processTinDungTD = new TinDungTDProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                int ret = 0;
                TDVMTRICHLAPDUPHONG.DSACH_DPHONG_CHUNG = lstDuPhongChung.ToArray();
                ret = processTinDungTD.TrichLapDuPhong(DatabaseConstant.Action.TINH_TOAN_TRICH_LAP_DU_PHONG_CHUNG, ref TDVMTRICHLAPDUPHONG, ref listClientResponseDetail);
                if (ret > 0)
                {
                    lstDuPhongChung = TDVMTRICHLAPDUPHONG.DSACH_DPHONG_CHUNG.ToList();
                    raddgrTrichLapDuPhongChung.ItemsSource = lstDuPhongChung;
                    grMain.DataContext = TDVMTRICHLAPDUPHONG;
                    return true;
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                return false;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public void OnCalculatorDetail()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TinDungTDProcess processTinDungTD = new TinDungTDProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                int ret = 0;
                TDVMTRICHLAPDUPHONG.DSACH_KHE_UOC = lstDuPhongCuThe.ToArray();
                ret = processTinDungTD.TrichLapDuPhong(DatabaseConstant.Action.TINH_TOAN_TRICH_LAP_DU_PHONG_CU_THE, ref TDVMTRICHLAPDUPHONG, ref listClientResponseDetail);
                if (ret > 0)
                {
                    lstDuPhongCuThe = TDVMTRICHLAPDUPHONG.DSACH_KHE_UOC.ToList();
                    raddgrTrichLapDuPhong.ItemsSource = lstDuPhongCuThe;
                    grMain.DataContext = TDVMTRICHLAPDUPHONG;
                    LMessage.ShowMessage("M.DungChung.Result.ThanhCong", LMessage.MessageBoxType.Information);
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
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }
        #endregion
    }
}
