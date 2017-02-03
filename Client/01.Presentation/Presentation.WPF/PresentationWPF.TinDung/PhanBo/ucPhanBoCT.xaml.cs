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
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using Presentation.Process;
using Presentation.Process.TinDungServiceRef;
using PresentationWPF.TinDung;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;

namespace PresentationWPF.TinDung.PhanBo
{
    /// <summary>
    /// Interaction logic for ucPhanBoCT.xaml
    /// </summary>
    public partial class ucPhanBoCT : UserControl
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
        private DataTable dtPhanBo = new DataTable();
        string _TThaiNVu = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
        List<DANH_SACH_KHE_UOC_PHAN_BO_LAI> _lstKUocPhanBo;
        List<int> lstPopupKU = new List<int>();
        public void LayDuLieuTuPopup(List<int> lst)
        {
            lstPopupKU = lst;
        }
        int _idGiaoDich = 0;
        private TDVM_PHAN_BO_LAI_VAY _objPhanBoLaiVay = null;
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        private KIEM_SOAT _objKiemSoat = null;
        public KIEM_SOAT objKIemSoat
        {
            get { return _objKiemSoat; }
            set { _objKiemSoat = value; }
        }
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucPhanBoCT()
        {
            InitComponent();
            KhoiTaoBangGiaTri();
            ClearForm();
            InitEventHandler();
        }

        private void InitComponent()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDung;component/PhanBo/ucPhanBoCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
        }

        private void InitEventHandler()
        {
            dtpNgayPhanBo.SelectedDateChanged += dtpNgayPhanBo_SelectedDateChanged;
        }

        public ucPhanBoCT(KIEM_SOAT objKSoat)
        {
            _objKiemSoat = objKSoat;
            InitComponent();
            if (_objKiemSoat != null)
            {
                action = _objKiemSoat.action;
                _TThaiNVu = _objKiemSoat.TTHAI_NVU;
                SetEnableControl();
                LoadData();
            }
            InitEventHandler();
        }

        void KhoiTaoBangGiaTri()
        {
            dtPhanBo.Columns.Add("ID", typeof(int));
            dtPhanBo.Columns.Add("MA_KUOC", typeof(string));
            dtPhanBo.Columns.Add("TEN_KHANG", typeof(string));
            dtPhanBo.Columns.Add("SO_TIEN_VAY", typeof(decimal));
            dtPhanBo.Columns.Add("SO_DU", typeof(decimal));
            dtPhanBo.Columns.Add("NGAY_VAY", typeof(string));
            dtPhanBo.Columns.Add("NGAY_TRA_GNHAT", typeof(string));
            dtPhanBo.Columns.Add("NGAY_PBO", typeof(int));
            dtPhanBo.Columns.Add("DT_PBO", typeof(decimal));
            dtPhanBo.Columns.Add("SO_NGAY", typeof(int));
            dtPhanBo.Columns.Add("DT_CPBO", typeof(decimal));
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
                        KeyGesture keyg = new KeyGesture(Key.V, ModifierKeys.Control | ModifierKeys.Shift);
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
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift);
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
                        KeyGesture keyg = new KeyGesture(Key.W, ModifierKeys.Control | ModifierKeys.Shift);
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                BeforeModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                BeforeSave();
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                BeforeModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                BeforeSave();
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

        void ClearForm()
        {
            txtSoPhieu.Text = "";
            dtpNgayPhanBo.SelectedDate = ClientInformation.NgayLamViecHienTai.StringToDate("yyyyMMdd");
            dtpNgayGiaoDich.SelectedDate = ClientInformation.NgayLamViecHienTai.StringToDate("yyyyMMdd");
            teldtNgayGiaoDich.Value = ClientInformation.NgayLamViecHienTai.StringToDate("yyyyMMdd");
            teldtPhanBoDenNgay.Value = ClientInformation.NgayLamViecHienTai.StringToDate("yyyyMMdd");
            _TThaiNVu = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
            txtNguoiCapNhat.Text = ClientInformation.TenDangNhap;
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            txtTrangThai.Text = "";
            lblTrangThai.Content = "";
            teldtNgayCNhat.Value = null;
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, _TThaiNVu);
            _objPhanBoLaiVay = null;
            _lstKUocPhanBo = null;
            _idGiaoDich = 0;
            raddgrPhanBoDS.ItemsSource = null;
        }

        private void HienPopupKheUoc()
        {
            try
            {
                decimal decSoTienLai = 0;
                List<TD_KHOACHVM_CT> lstKeHoach = null;
                TD_KHOACHVM_CT objKeHoach = null;
                string sNgayGanNhat = "";
                List<string> lstDieuKien = new List<string>();
                TDVM_KHE_UOC objKUocVM = new TDVM_KHE_UOC();
                KH_KHANG_HSO objKhachHang = new KH_KHANG_HSO();
                TinDungProcess tdProcess = new TinDungProcess();
                DANH_SACH_KHE_UOC_PHAN_BO_LAI objKuocPBo = null;
                string sidKheUoc = "";
                int idKUoc = 0;
                int iRet = 1;
                if (_lstKUocPhanBo == null) _lstKUocPhanBo = new List<DANH_SACH_KHE_UOC_PHAN_BO_LAI>();
                if (_lstKUocPhanBo != null && _lstKUocPhanBo.Count > 0)
                {
                    for (int i = 0; i < _lstKUocPhanBo.Count; i++)
                    {
                        sidKheUoc += "," + _lstKUocPhanBo[i].ID_KHE_UOC.ToString();
                    }
                }
                if (sidKheUoc.Length > 0)
                    sidKheUoc = sidKheUoc.Substring(1);
                else
                    sidKheUoc = "0";
                lstDieuKien.Add(sidKheUoc);
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add("KUOC_PBO_DTHU");
                lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
                lstDieuKien.Add("%");
                PopupProcess process = new PopupProcess();
                process.getPopupInformation("POPUP_DS_KHEUOC", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopupKheUocViMo popup = new ucPopupKheUocViMo(true, simplePopupResponse, true);
                popup.LayGiaTriListID = new ucPopupKheUocViMo.LayListID(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Content = popup;
                win.ShowDialog();
                if (lstPopupKU != null && lstPopupKU.Count > 0)
                {
                    for (int i = 0; i < lstPopupKU.Count; i++)
                    {
                        idKUoc = lstPopupKU[i];
                        lstKeHoach = new List<TD_KHOACHVM_CT>();
                        objKeHoach = new TD_KHOACHVM_CT();
                        objKuocPBo = new DANH_SACH_KHE_UOC_PHAN_BO_LAI();
                        objKUocVM = new TDVM_KHE_UOC();
                        objKhachHang = new KH_KHANG_HSO();

                        objKUocVM.KUOC_VM = new TD_KUOCVM();
                        objKUocVM.KUOC_VM.ID = idKUoc;
                        iRet = tdProcess.GetKUocById(ref objKUocVM);
                        if (iRet == 1)
                        {
                            iRet = tdProcess.GetKHangByKuoc(ref objKhachHang, objKUocVM);
                            if (iRet == 1)
                            {
                                lstKeHoach = objKUocVM.DSACH_KHOACHVM_CTIET.ToList();
                                objKuocPBo.HINH_THUC_TRA_LAI = objKUocVM.KUOC_VM.TRLAI_HTHUC;
                                objKuocPBo.ID_DON_VI = ClientInformation.IdDonViGiaoDich;
                                objKuocPBo.ID_KHACH_HANG = objKhachHang.ID;
                                objKuocPBo.ID_KHE_UOC = objKUocVM.KUOC_VM.ID;
                                objKuocPBo.MA_DON_VI = ClientInformation.MaDonViGiaoDich;
                                objKuocPBo.MA_KHACH_HANG = objKhachHang.MA_KHANG;
                                objKuocPBo.MA_KHE_UOC = objKUocVM.KUOC_VM.MA_KUOCVM;
                                objKuocPBo.MA_SAN_PHAM = objKUocVM.KUOC_VM.MA_SAN_PHAM;
                                objKuocPBo.NGAY_PHAN_BO_GAN_NHAT = objKUocVM.KUOC_VM.PHAN_BO_DEN_NGAY;
                                objKuocPBo.NGAY_VAY = objKUocVM.KUOC_VM.NGAY_GIAI_NGAN;
                                objKuocPBo.SO_TIEN_VAY = objKUocVM.KUOC_VM.SO_TIEN_GIAI_NGAN;
                                objKuocPBo.TEN_DON_VI = ClientInformation.TenDonViGiaoDich;
                                objKuocPBo.TEN_KHACH_HANG = objKhachHang.TEN_KHANG;
                                objKuocPBo.TONG_TIEN_PHAN_BO = objKUocVM.KUOC_VM.LAI_TREO;
                                if (lstKeHoach != null && lstKeHoach.Count > 0)
                                {
                                    //objKuocPBo.NGAY_TRA_LAI_GAN_NHAT = objKUocVM.KUOC_VM.THU_LAI_DEN_NGAY;
                                    objKuocPBo.NGAY_TRA_LAI_GAN_NHAT = lstKeHoach.Max(e => e.TT_NGAY_TRA);
                                    sNgayGanNhat = lstKeHoach.Where(e => e.KH_NGAY_TRA.CompareTo(ClientInformation.NgayLamViecHienTai) <= 0).Select(e => e.KH_NGAY_TRA).Max();
                                    objKeHoach = lstKeHoach.FirstOrDefault(e => e.KH_NGAY_TRA.Equals(sNgayGanNhat));
                                }
                                if (objKUocVM.KUOC_VM.TRLAI_HTHUC.Equals(BusinessConstant.HINH_THUC_TRA_LAI.DINH_KY.layGiaTri()))
                                {
                                    decSoTienLai = Convert.ToDecimal(objKeHoach.KH_TRA_LAI);
                                    if (decSoTienLai <= objKUocVM.KUOC_VM.LAI_TREO)
                                    {
                                        objKuocPBo.SO_TIEN_PHAN_BO_KY_NAY = decSoTienLai;
                                    }
                                    else
                                    {
                                        objKuocPBo.SO_TIEN_PHAN_BO_KY_NAY = objKUocVM.KUOC_VM.LAI_TREO;
                                    }
                                }
                                else if (objKUocVM.KUOC_VM.TRLAI_HTHUC.Equals(BusinessConstant.HINH_THUC_TRA_LAI.DAU_KY.layGiaTri()))
                                {
                                    //chua rõ số tiền trả lãi đầu kỳ được lưu vào đâu nên tạm thời để = lãi đã thu
                                    TD_KHOACHVM_CT kehoach = null;
                                    if (!lstKeHoach.IsNullOrEmpty() && lstKeHoach.Count > 0)
                                        kehoach = lstKeHoach.First(e => e.MA_KUOCVM.Equals(objKUocVM.KUOC_VM.MA_KUOCVM) && e.KY_THU == 0);
                                    if(!kehoach.IsNullOrEmpty())
                                        decSoTienLai = kehoach.TT_TRA_LAI.Value / objKUocVM.KUOC_VM.TGIAN_VAY;
                                    //decSoTienLai = objKUocVM.KUOC_VM.LAI_TREO / objKUocVM.KUOC_VM.TGIAN_VAY;
                                    if (objKUocVM.KUOC_VM.LAI_TREO <= decSoTienLai)
                                    {
                                        objKuocPBo.SO_TIEN_PHAN_BO_KY_NAY = objKUocVM.KUOC_VM.LAI_TREO;
                                    }
                                    else
                                    {
                                        objKuocPBo.SO_TIEN_PHAN_BO_KY_NAY = decSoTienLai;
                                    }
                                }
                                objKuocPBo.SO_TIEN_CON_PHAI_PHAN_BO = objKuocPBo.TONG_TIEN_PHAN_BO - objKuocPBo.SO_TIEN_PHAN_BO_KY_NAY;
                                _lstKUocPhanBo.Add(objKuocPBo);
                            }
                        }
                    }
                }
                LoadGridKheUoc();
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void tlbThemKUOC_Click(object sender, RoutedEventArgs e)
        {
            HienPopupKheUoc();
        }

        private void tlbXoaKUOC_Click(object sender, RoutedEventArgs e)
        {
            if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            foreach (DANH_SACH_KHE_UOC_PHAN_BO_LAI objPhanBo in raddgrPhanBoDS.SelectedItems)
            {
                _lstKUocPhanBo.Remove(objPhanBo);
            }
            LoadGridKheUoc();
        }

        private void raddgrPhanBoDS_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {
            //DANH_SACH_KHE_UOC_PHAN_BO_LAI objKUocPBo = (DANH_SACH_KHE_UOC_PHAN_BO_LAI)raddgrPhanBoDS.SelectedItem;
            DANH_SACH_KHE_UOC_PHAN_BO_LAI objKUocPBo = (DANH_SACH_KHE_UOC_PHAN_BO_LAI)e.Row.Item;

            if (objKUocPBo.TONG_TIEN_PHAN_BO < objKUocPBo.SO_TIEN_PHAN_BO_KY_NAY)
            {
                objKUocPBo.SO_TIEN_PHAN_BO_KY_NAY = objKUocPBo.TONG_TIEN_PHAN_BO;
                objKUocPBo.SO_TIEN_CON_PHAI_PHAN_BO = objKUocPBo.TONG_TIEN_PHAN_BO - objKUocPBo.SO_TIEN_PHAN_BO_KY_NAY;
            }
            else
            {
                objKUocPBo.SO_TIEN_CON_PHAI_PHAN_BO = objKUocPBo.TONG_TIEN_PHAN_BO - objKUocPBo.SO_TIEN_PHAN_BO_KY_NAY;
            }

            for (int i = 0; i < _lstKUocPhanBo.Count; i++)
            {
                if (_lstKUocPhanBo[i].ID_KHE_UOC == objKUocPBo.ID_KHE_UOC)
                {
                    _lstKUocPhanBo[i] = objKUocPBo;
                    break;
                }
            }
            LoadGridKheUoc();
        }

        private void teldtPhanBoDenNgay_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (!teldtPhanBoDenNgay.Text.IsDate("dd/MM/yyyy"))
            {
                LMessage.ShowMessage("Ngày phân bổ không hợp lệ.", LMessage.MessageBoxType.Warning);
                return;
            }

            else if (Convert.ToDateTime(teldtPhanBoDenNgay.Value) < Convert.ToDateTime(teldtNgayGiaoDich.Value))
            {
                LMessage.ShowMessage("M.TinDung.PhanBo.ucPhanBoCT.NgayPhanBoLonHonNgayGDich", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                _lstKUocPhanBo = new List<DANH_SACH_KHE_UOC_PHAN_BO_LAI>();
                LoadGridKheUoc();
            }
        }

        private void dtpNgayPhanBo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

            if (dtpNgayPhanBo.SelectedDate.Value < Convert.ToDateTime(teldtNgayGiaoDich.Value))
            {
                LMessage.ShowMessage("M.TinDung.PhanBo.ucPhanBoCT.NgayPhanBoLonHonNgayGDich", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                _lstKUocPhanBo = new List<DANH_SACH_KHE_UOC_PHAN_BO_LAI>();
                LoadGridKheUoc();
            }
        }

        #endregion


        /// <summary>
        /// Xu ly nghiep vu
        /// </summary>
        #region Xu ly nghiep vu
        void LoadGridKheUoc()
        {
            raddgrPhanBoDS.ItemsSource = _lstKUocPhanBo;
            raddgrPhanBoDS.Rebind();
        }

        private void LoadData()
        {
            try
            {
                DataSet dsGDich = new DataSet();
                TinDungProcess tdProcess = new TinDungProcess();
                dsGDich = tdProcess.LayThongTinGiaoDichPhanBoDoanhThu(_objKiemSoat.SO_GIAO_DICH);
                DataTable dtKUoc = new DataTable();
                DataTable dtPBDT = new DataTable();
                _lstKUocPhanBo = new List<DANH_SACH_KHE_UOC_PHAN_BO_LAI>();
                _objPhanBoLaiVay = new TDVM_PHAN_BO_LAI_VAY();
                DANH_SACH_KHE_UOC_PHAN_BO_LAI objKUoc = null;
                if (dsGDich != null && dsGDich.Tables.Count >= 2)
                {
                    dtKUoc = dsGDich.Tables[1];
                    dtPBDT = dsGDich.Tables[0];

                    #region Lay thong tin giao dich
                    _objPhanBoLaiVay.DIEN_GIAI = dtPBDT.Rows[0]["DIEN_GIAI"].ToString();
                    _objPhanBoLaiVay.ID_GIAO_DICH = Convert.ToInt32(dtPBDT.Rows[0]["ID_GDICH"]);
                    _objPhanBoLaiVay.LOAI_TIEN = dtPBDT.Rows[0]["LOAI_TIEN"].ToString();
                    _objPhanBoLaiVay.LY_DO = dtPBDT.Rows[0]["LY_DO"].ToString();
                    _objPhanBoLaiVay.MA_DVI = dtPBDT.Rows[0]["MA_DVI_TAO"].ToString();
                    _objPhanBoLaiVay.MA_DVI_GD = dtPBDT.Rows[0]["MA_DVI_TAO"].ToString();
                    _objPhanBoLaiVay.MA_GIAO_DICH = dtPBDT.Rows[0]["MA_GDICH"].ToString();
                    _objPhanBoLaiVay.NGAY_CAP_NHAT = dtPBDT.Rows[0]["NGAY_CNHAT"].ToString();
                    _objPhanBoLaiVay.NGAY_GIAO_DICH = dtPBDT.Rows[0]["NGAY_DL"].ToString();
                    _objPhanBoLaiVay.NGAY_LAP = dtPBDT.Rows[0]["NGAY_NHAP"].ToString();
                    _objPhanBoLaiVay.NGUOI_CAP_NHAT = dtPBDT.Rows[0]["NGUOI_CNHAT"].ToString();
                    _objPhanBoLaiVay.NGUOI_LAP = dtPBDT.Rows[0]["NGUOI_NHAP"].ToString();
                    _objPhanBoLaiVay.PHAN_BO_DEN_NGAY = dtPBDT.Rows[0]["NGAY_PHAN_BO"].ToString();
                    _objPhanBoLaiVay.TRANG_THAI_BAN_GHI = dtPBDT.Rows[0]["TTHAI_BGHI"].ToString();
                    _objPhanBoLaiVay.TRANG_THAI_NGHIEP_VU = dtPBDT.Rows[0]["TTHAI_NVU"].ToString();
                    #endregion

                    #region Lay thong tin khe uoc phan bo doanh thu

                    for (int i = 0; i < dtKUoc.Rows.Count; i++)
                    {
                        objKUoc = new DANH_SACH_KHE_UOC_PHAN_BO_LAI();
                        objKUoc.HINH_THUC_TRA_LAI = dtKUoc.Rows[i]["HINH_THUC_TRA_LAI"].ToString();
                        objKUoc.ID_DON_VI = Convert.ToInt32(dtKUoc.Rows[i]["ID_DON_VI"]);
                        objKUoc.ID_KHACH_HANG = Convert.ToInt32(dtKUoc.Rows[i]["ID_KHACH_HANG"]);
                        objKUoc.ID_KHE_UOC = Convert.ToInt32(dtKUoc.Rows[i]["ID_KHE_UOC"]);
                        objKUoc.MA_DON_VI = dtKUoc.Rows[i]["MA_DON_VI"].ToString();
                        objKUoc.MA_KHACH_HANG = dtKUoc.Rows[i]["MA_KHACH_HANG"].ToString();
                        objKUoc.MA_KHE_UOC = dtKUoc.Rows[i]["MA_KHE_UOC"].ToString();
                        objKUoc.MA_SAN_PHAM = dtKUoc.Rows[i]["MA_SAN_PHAM"].ToString();
                        objKUoc.NGAY_PHAN_BO_GAN_NHAT = dtKUoc.Rows[i]["NGAY_PHAN_BO_GAN_NHAT"].ToString();
                        objKUoc.NGAY_TRA_LAI_GAN_NHAT = dtKUoc.Rows[i]["NGAY_TRA_LAI_GAN_NHAT"].ToString();
                        objKUoc.NGAY_VAY = dtKUoc.Rows[i]["NGAY_VAY"].ToString();
                        objKUoc.SO_TIEN_CON_PHAI_PHAN_BO = dtKUoc.Rows[i]["SO_TIEN_CON_PHAI_PHAN_BO"].ToString().Replace(".00", "").StringToDecimal();
                        objKUoc.SO_TIEN_PHAN_BO_KY_NAY = dtKUoc.Rows[i]["SO_TIEN_PHAN_BO_KY_NAY"].ToString().Replace(".00", "").StringToDecimal();
                        objKUoc.SO_TIEN_VAY = dtKUoc.Rows[i]["SO_TIEN_VAY"].ToString().Replace(".00", "").StringToDecimal();
                        objKUoc.TEN_DON_VI = dtKUoc.Rows[i]["TEN_DON_VI"].ToString();
                        objKUoc.TEN_KHACH_HANG = dtKUoc.Rows[i]["TEN_KHACH_HANG"].ToString();
                        objKUoc.TONG_TIEN_PHAN_BO = dtKUoc.Rows[i]["TONG_TIEN_PHAN_BO"].ToString().Replace(".00", "").StringToDecimal();
                        _lstKUocPhanBo.Add(objKUoc);
                    }
                    _objPhanBoLaiVay.DSACH_KHE_UOC = _lstKUocPhanBo.ToArray();
                    #endregion

                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(_objPhanBoLaiVay.TRANG_THAI_NGHIEP_VU);
                    LoadGridKheUoc();
                    dtpNgayPhanBo.SelectedDate = _objPhanBoLaiVay.PHAN_BO_DEN_NGAY.StringToDate("yyyyMMdd");
                    dtpNgayGiaoDich.SelectedDate = _objPhanBoLaiVay.NGAY_GIAO_DICH.StringToDate("yyyyMMdd");
                    teldtPhanBoDenNgay.Value = _objPhanBoLaiVay.PHAN_BO_DEN_NGAY.StringToDate("yyyyMMdd");
                    teldtNgayGiaoDich.Value = _objPhanBoLaiVay.NGAY_GIAO_DICH.StringToDate("yyyyMMdd");
                    txtSoPhieu.Text = _objPhanBoLaiVay.MA_GIAO_DICH;
                    teldtNgayCNhat.Value = LDateTime.StringToDate(_objPhanBoLaiVay.NGAY_CAP_NHAT, ApplicationConstant.defaultDateTimeFormat);
                    teldtNgayNhap.Value = LDateTime.StringToDate(_objPhanBoLaiVay.NGAY_LAP, ApplicationConstant.defaultDateTimeFormat);
                    txtNguoiCapNhat.Text = _objPhanBoLaiVay.NGUOI_CAP_NHAT;
                    txtNguoiLap.Text = _objPhanBoLaiVay.NGUOI_LAP;
                    txtTrangThai.Text = _objPhanBoLaiVay.TRANG_THAI_NGHIEP_VU;
                    txtDienGiai.Text = _objPhanBoLaiVay.DIEN_GIAI;
                    _idGiaoDich = _objKiemSoat.ID;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void OnPreview()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(txtSoPhieu.Text))
            {
                LMessage.ShowMessage("Không có thông tin dữ liệu cần hiển thị", LMessage.MessageBoxType.Warning);

            }
            else
            {
                VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();

                GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.GDKT_IN_GIAO_DICH;
                DatabaseConstant.Function _function = DatabaseConstant.Function.KT_PHIEU_KE_TOAN;

                objGIAO_DICH_BASE.ChucNang = _function;

                GDKT_GIAO_DICH objGDKT_GIAO_DICH = new GDKT_GIAO_DICH();
                objGDKT_GIAO_DICH.MaGiaoDich = txtSoPhieu.Text;

                doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                doiTuongBaoCao.objGDKT_GIAO_DICH = objGDKT_GIAO_DICH;
                xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private bool Validation()
        {
            if (_lstKUocPhanBo == null || _lstKUocPhanBo.Count == 0)
            {
                LMessage.ShowMessage("M.TinDung.XoaNo.ucXoaNoCT01.LoiKhongCoKheUoc", LMessage.MessageBoxType.Warning);
                return false;
            }

            else if (teldtPhanBoDenNgay.Value.IsNullOrEmpty() || !teldtPhanBoDenNgay.Text.IsDate("dd/MM/yyyy"))
            {
                LMessage.ShowMessage("Ngày phân bổ không hợp lệ.", LMessage.MessageBoxType.Warning);
                return false;
            }

            else if (Convert.ToDateTime(teldtPhanBoDenNgay.Value) < Convert.ToDateTime(teldtNgayGiaoDich.Value))
            {
                LMessage.ShowMessage("M.TinDung.PhanBo.ucPhanBoCT.NgayPhanBoLonHonNgayGDich", LMessage.MessageBoxType.Warning);
                return false;
            }
            else if (txtDienGiai.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblDienGiai.Content.ToString());
                return false;
            }

            return true;
        }

        private void SetEnableControl()
        {
            CommonFunction.RefreshButton(Toolbar, action, _TThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_PHAN_BO_LAI_VAY);
            if (action == DatabaseConstant.Action.XEM)
            {
                dtpNgayPhanBo.IsEnabled = false;
                dtpNgayGiaoDich.IsEnabled = false;
                teldtNgayGiaoDich.IsEnabled = false;
                teldtNgayCNhat.IsEnabled = false;
                teldtNgayNhap.IsEnabled = false;
                teldtPhanBoDenNgay.IsEnabled = false;
                tlbThemKUOC.IsEnabled = false;
                tlbXoaKUOC.IsEnabled = false;
                raddgrPhanBoDS.IsEnabled = false;
            }
            else if (action == DatabaseConstant.Action.THEM)
            {
                dtpNgayPhanBo.IsEnabled = true;
                dtpNgayGiaoDich.IsEnabled = true;
                teldtNgayGiaoDich.IsEnabled = true;
                teldtNgayCNhat.IsEnabled = true;
                teldtNgayNhap.IsEnabled = true;
                teldtPhanBoDenNgay.IsEnabled = true;
                tlbThemKUOC.IsEnabled = true;
                tlbXoaKUOC.IsEnabled = true;
                raddgrPhanBoDS.IsEnabled = true;
                ClearForm();
            }
            else if (action == DatabaseConstant.Action.SUA)
            {
                dtpNgayPhanBo.IsEnabled = true;
                dtpNgayGiaoDich.IsEnabled = true;
                teldtNgayGiaoDich.IsEnabled = true;
                teldtNgayCNhat.IsEnabled = true;
                teldtNgayNhap.IsEnabled = true;
                teldtPhanBoDenNgay.IsEnabled = true;
                tlbThemKUOC.IsEnabled = true;
                tlbXoaKUOC.IsEnabled = true;
                raddgrPhanBoDS.IsEnabled = true;
            }
        }

        private int GetValuesOnForm(ref TDVM_PHAN_BO_LAI_VAY objPBoLai)
        {
            int iRet = 1;
            try
            {
                if (objPBoLai == null)
                {
                    objPBoLai = new TDVM_PHAN_BO_LAI_VAY();
                }
                objPBoLai.ID_GIAO_DICH = _idGiaoDich;
                objPBoLai.MA_GIAO_DICH = txtSoPhieu.Text.Trim();
                objPBoLai.DIEN_GIAI = txtDienGiai.Text;
                foreach (var item in _lstKUocPhanBo)
                {
                    item.SO_TIEN_CON_PHAI_PHAN_BO = LNumber.Rounding(item.SO_TIEN_CON_PHAI_PHAN_BO, 0, LNumber.RoundingType.Down);
                    item.SO_TIEN_PHAN_BO_KY_NAY = LNumber.Rounding(item.SO_TIEN_PHAN_BO_KY_NAY, 0, LNumber.RoundingType.Down);
                    item.SO_TIEN_VAY = LNumber.Rounding(item.SO_TIEN_VAY, 0, LNumber.RoundingType.Down);
                    item.TONG_TIEN_PHAN_BO = LNumber.Rounding(item.TONG_TIEN_PHAN_BO, 0, LNumber.RoundingType.Down);
                }
                objPBoLai.DSACH_KHE_UOC = _lstKUocPhanBo.ToArray();
                objPBoLai.LOAI_TIEN = ClientInformation.MaDongNoiTe;
                objPBoLai.LY_DO = "";
                objPBoLai.MA_DVI = ClientInformation.MaDonVi;
                objPBoLai.MA_DVI_GD = ClientInformation.MaDonViGiaoDich;
                objPBoLai.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                objPBoLai.NGAY_GIAO_DICH = ClientInformation.NgayLamViecHienTai;
                objPBoLai.NGAY_LAP = ClientInformation.NgayLamViecHienTai;
                objPBoLai.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                objPBoLai.NGUOI_LAP = ClientInformation.TenDangNhap;
                objPBoLai.PHAN_BO_DEN_NGAY = Convert.ToDateTime(teldtPhanBoDenNgay.Value).ToString("yyyyMMdd");
                objPBoLai.TRANG_THAI_BAN_GHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                objPBoLai.TRANG_THAI_NGHIEP_VU = _TThaiNVu;
            }
            catch (Exception ex)
            {
                iRet = 0;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return iRet;
        }

        private void BeforeSave()
        {
            try
            {
                if (!Validation()) return;
                int iRet = GetValuesOnForm(ref _objPhanBoLaiVay);
                if (iRet == 0) return;
                OnSave(ref _objPhanBoLaiVay);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void OnSave(ref TDVM_PHAN_BO_LAI_VAY objPBoDT)
        {
            try
            {
                List<ClientResponseDetail> lstClienReponseDetail = new List<ClientResponseDetail>();
                TinDungProcess tdProcess = new TinDungProcess();
                int iRet = 1;
                iRet = tdProcess.PhanBoDoanhThu(ref _objPhanBoLaiVay, ref lstClienReponseDetail, action);
                AfterSave(iRet, lstClienReponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void AfterSave(int iRet, List<ClientResponseDetail> lstClientResponseDetail)
        {
            try
            {
                if (iRet == 1)
                {
                    LMessage.ShowMessage("M.DungChung.LuuDuLieuThanhCong", LMessage.MessageBoxType.Information);
                    action = DatabaseConstant.Action.XEM;
                    txtSoPhieu.Text = _objPhanBoLaiVay.MA_GIAO_DICH;
                    _idGiaoDich = _objPhanBoLaiVay.ID_GIAO_DICH;
                    _TThaiNVu = _objPhanBoLaiVay.TRANG_THAI_NGHIEP_VU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(_TThaiNVu);
                    SetEnableControl();
                    CommonFunction.RefreshButton(Toolbar, action, _TThaiNVu);
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(lstClientResponseDetail);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void BeforeModify()
        {
            try
            {
                action = DatabaseConstant.Action.SUA;
                _TThaiNVu = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                SetEnableControl();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void BeforeDelete()
        {
            try
            {
                if (_idGiaoDich > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) == MessageBoxResult.No) return;
                    action = DatabaseConstant.Action.XOA;
                    List<int> lstID = new List<int>();
                    lstID.Add(_idGiaoDich);
                    UtilitiesProcess utiProcess = new UtilitiesProcess();
                    utiProcess.LockData(DatabaseConstant.Module.TDVM,
                                        DatabaseConstant.Function.TDVM_PHAN_BO_LAI_VAY,
                                        DatabaseConstant.Table.KT_GIAO_DICH,
                                        action,
                                        lstID);
                    OnDelete();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void OnDelete()
        {
            try
            {
                List<ClientResponseDetail> lstClienResponseDetail = new List<ClientResponseDetail>();
                TinDungProcess tdProcess = new TinDungProcess();
                int iRet = tdProcess.PhanBoDoanhThu(ref _objPhanBoLaiVay, ref lstClienResponseDetail, action);
                AfterDelete(iRet, lstClienResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void AfterDelete(int iRet, List<ClientResponseDetail> lstClientResponseDetail)
        {
            try
            {
                if (iRet == 1)
                {
                    List<int> lstID = new List<int>();
                    lstID.Add(_idGiaoDich);
                    UtilitiesProcess utiProcess = new UtilitiesProcess();
                    LMessage.ShowMessage("M_ResponseMessage_GIAODICH_XoaThanhCong", LMessage.MessageBoxType.Information);
                    utiProcess.UnlockData(DatabaseConstant.Module.TDVM,
                                    DatabaseConstant.Function.TDVM_PHAN_BO_LAI_VAY,
                                    DatabaseConstant.Table.KT_GIAO_DICH,
                                    action,
                                    lstID);
                    _TThaiNVu = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                    action = DatabaseConstant.Action.THEM;
                    ClearForm();
                    SetEnableControl();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(lstClientResponseDetail);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void BeforeApprove()
        {
            try
            {
                if (_idGiaoDich > 0)
                {
                    UtilitiesProcess utiProcess = new UtilitiesProcess();
                    action = DatabaseConstant.Action.DUYET;
                    List<int> lstID = new List<int>();
                    lstID.Add(_idGiaoDich);
                    utiProcess.LockData(DatabaseConstant.Module.TDVM,
                                        DatabaseConstant.Function.TDVM_PHAN_BO_LAI_VAY,
                                        DatabaseConstant.Table.KT_GIAO_DICH,
                                        action,
                                        lstID);
                    OnApprove();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void OnApprove()
        {
            try
            {
                TinDungProcess tdProcess = new TinDungProcess();
                List<ClientResponseDetail> lstClientResponseDetail = new List<ClientResponseDetail>();
                int iRet = tdProcess.PhanBoDoanhThu(ref _objPhanBoLaiVay, ref lstClientResponseDetail, action);
                AfterApprove(iRet, lstClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void AfterApprove(int iRet, List<ClientResponseDetail> lstClientResponseDetail)
        {
            try
            {
                UtilitiesProcess utiProcess = new UtilitiesProcess();
                List<int> lstID = new List<int>();
                lstID.Add(_idGiaoDich);
                if (iRet == 1)
                {
                    LMessage.ShowMessage("M_ResponseMessage_GIAODICH_DuyetThanhCong", LMessage.MessageBoxType.Information);
                    utiProcess.UnlockData(DatabaseConstant.Module.TDVM,
                                        DatabaseConstant.Function.TDVM_PHAN_BO_LAI_VAY,
                                        DatabaseConstant.Table.KT_GIAO_DICH,
                                        action,
                                        lstID);

                    _TThaiNVu = _objPhanBoLaiVay.TRANG_THAI_NGHIEP_VU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(_TThaiNVu);
                    SetEnableControl();
                    CommonFunction.RefreshButton(Toolbar, action, _TThaiNVu);
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(lstClientResponseDetail);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void BeforeRefuse()
        {
            try
            {
                List<int> lstID = new List<int>();
                UtilitiesProcess utiProcess = new UtilitiesProcess();
                if (_idGiaoDich > 0)
                {
                    lstID.Add(_idGiaoDich);
                    action = DatabaseConstant.Action.TU_CHOI_DUYET;
                    utiProcess.LockData(DatabaseConstant.Module.TDVM,
                                        DatabaseConstant.Function.TDVM_PHAN_BO_LAI_VAY,
                                        DatabaseConstant.Table.KT_GIAO_DICH,
                                        action,
                                        lstID);
                    OnRefuse();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void OnRefuse()
        {
            try
            {
                TinDungProcess tdProcess = new TinDungProcess();
                List<ClientResponseDetail> lstClientResponseDetail = new List<ClientResponseDetail>();
                int iRet = tdProcess.PhanBoDoanhThu(ref _objPhanBoLaiVay, ref lstClientResponseDetail, action);
                AfterRefuse(iRet, lstClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void AfterRefuse(int iRet, List<ClientResponseDetail> lstClientResponseDetail)
        {
            try
            {
                List<int> lstID = new List<int>();
                UtilitiesProcess utiProcess = new UtilitiesProcess();
                lstID.Add(_idGiaoDich);
                if (iRet == 1)
                {
                    LMessage.ShowMessage("M_ResponseMessage_GIAODICH_TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);
                    utiProcess.UnlockData(DatabaseConstant.Module.TDVM,
                                        DatabaseConstant.Function.TDVM_PHAN_BO_LAI_VAY,
                                        DatabaseConstant.Table.KT_GIAO_DICH,
                                        action,
                                        lstID);
                    _TThaiNVu = _objPhanBoLaiVay.TRANG_THAI_NGHIEP_VU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(_TThaiNVu);
                    SetEnableControl();
                    CommonFunction.RefreshButton(Toolbar, action, _TThaiNVu);
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(lstClientResponseDetail);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void BeforeCancel()
        {
            try
            {
                List<int> lstID = new List<int>();
                UtilitiesProcess utiProcess = new UtilitiesProcess();
                if (_idGiaoDich > 0)
                {
                    lstID.Add(_idGiaoDich);
                    action = DatabaseConstant.Action.THOAI_DUYET;
                    utiProcess.LockData(DatabaseConstant.Module.TDVM,
                                        DatabaseConstant.Function.TDVM_PHAN_BO_LAI_VAY,
                                        DatabaseConstant.Table.KT_GIAO_DICH,
                                        action,
                                        lstID);
                    OnCancel();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void OnCancel()
        {
            try
            {
                TinDungProcess tdProcess = new TinDungProcess();
                List<ClientResponseDetail> lstClientReponseDetail = new List<ClientResponseDetail>();
                int iRet = tdProcess.PhanBoDoanhThu(ref _objPhanBoLaiVay, ref lstClientReponseDetail, action);
                AfterCancel(iRet, lstClientReponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void AfterCancel(int iRet, List<ClientResponseDetail> lstClientResponseDetail)
        {
            try
            {
                List<int> lstID = new List<int>();
                UtilitiesProcess utiProcess = new UtilitiesProcess();
                if (iRet == 1)
                {
                    LMessage.ShowMessage("M_ResponseMessage_GIAODICH_ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);
                    lstID.Add(_idGiaoDich);
                    utiProcess.UnlockData(DatabaseConstant.Module.TDVM,
                                        DatabaseConstant.Function.TDVM_PHAN_BO_LAI_VAY,
                                        DatabaseConstant.Table.KT_GIAO_DICH,
                                        action,
                                        lstID);

                    _TThaiNVu = _objPhanBoLaiVay.TRANG_THAI_NGHIEP_VU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(_TThaiNVu);
                    SetEnableControl();
                    CommonFunction.RefreshButton(Toolbar, action, _TThaiNVu);
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(lstClientResponseDetail);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
        #endregion
    }
}
