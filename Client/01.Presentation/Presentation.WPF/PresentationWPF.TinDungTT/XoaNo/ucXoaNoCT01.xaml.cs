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
using Presentation.Process.UtilitiesServiceRef;
using Presentation.Process.Common;
using Presentation.Process.TinDungServiceRef;
using Presentation.Process;
using Telerik.Windows.Controls;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;


namespace PresentationWPF.TinDungTT.XoaNo
{
    /// <summary>
    /// Interaction logic for ucXoaNoCT01.xaml
    /// </summary>
    public partial class ucXoaNoCT01 : UserControl
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
        List<XuLyNoQuaHan> lstXuLyNoQH = new List<XuLyNoQuaHan>();
        delegate void TruyenGiaTriChoPopup(List<XuLyNoQuaHan> lst);
        public void LayGiaTriPopup(List<XuLyNoQuaHan> lst)
        {
            lstXuLyNoQH = lst;
        }
        private List<DANH_SACH_KHE_UOC_XU_LY_NO> _lstDSKUocXLN;
        private List<DANH_SACH_KHE_UOC_XU_LY_NO> _lstKuocLuuXLN;
        string TThaiNVu = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
        decimal _decDuPhongChung = 0;
        TDVM_XY_LY_NO _objXuLyNo;
        private KIEM_SOAT _objKiemSoat = null;
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucXoaNoCT01()
        {
            InitialComponent();
        }

        private void InitialComponent()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDungTT;component/XoaNo/ucXoaNoCT01.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            ClearForm();
            LaySoDuDuPhongChung();
        }

        public ucXoaNoCT01(KIEM_SOAT objKSoat)
        {
            InitialComponent();
            try
            {
                _objKiemSoat = objKSoat;
                if (_objKiemSoat != null)
                {
                    _objXuLyNo = new TDVM_XY_LY_NO();
                    action = _objKiemSoat.action;
                    TThaiNVu = _objKiemSoat.TTHAI_NVU;
                    txtSoPhieu.Text = _objKiemSoat.SO_GIAO_DICH;
                    _objXuLyNo.ID_GIAO_DICH = _objKiemSoat.ID;
                    _objXuLyNo.MA_GIAO_DICH = _objKiemSoat.SO_GIAO_DICH;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(objKSoat.TTHAI_NVU);
                    LoadForm();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                CommonFunction.ThongBaoLoi(ex);
            }
        }

        private void LaySoDuDuPhongChung()
        {
            try
            {
                KeToanProcess process = new KeToanProcess();
                Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI objKHPL = new Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI();
                Presentation.Process.KeToanServiceRef.KT_TKHOAN_SDU objTKhoanSDU = new Presentation.Process.KeToanServiceRef.KT_TKHOAN_SDU();
                objTKhoanSDU.NGAY_DL = ClientInformation.NgayLamViecHienTai;
                objTKhoanSDU.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                objKHPL.MA_KY_HIEU = BusinessConstant.MA_KY_HIEU.DUPH02.layGiaTri();
                process.LaySoDuTheoKyHieuPLoai(ref _decDuPhongChung, objKHPL, objTKhoanSDU);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
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

            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAP_DU_LIEU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ClearForm();
                SetEnableControl();
                TThaiNVu = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                action = DatabaseConstant.Action.THEM;
            }
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {

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
            else if (strTinhNang.Equals("PreviewChungTu"))
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
            {
                BeforeDelete();
            }
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
                BeforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
            }
            else if (strTinhNang.Equals("PreviewChungTu"))
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

        public void LayDSKuocXoaNo(List<DANH_SACH_KHE_UOC_XU_LY_NO> lstDSKUoc)
        {
            _lstDSKUocXLN = lstDSKUoc;
            _lstKuocLuuXLN = new List<DANH_SACH_KHE_UOC_XU_LY_NO>();
            _lstKuocLuuXLN = _lstDSKUocXLN;
            LoadGridViewData();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ucXuLyNoKheUoc01 objKheUoc = new ucXuLyNoKheUoc01();
            objKheUoc.decDuPhongChung = _decDuPhongChung;
            objKheUoc.listKUocXLN = _lstDSKUocXLN;
            objKheUoc.GetListXoaNo = new ucXuLyNoKheUoc01.LayDSXoaNo(LayDSKuocXoaNo);
            Window window = new Window();
            window.Title = "Chi tiết xử lý nợ khế ước";
            window.Content = objKheUoc;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grdKheUoc.SelectedItems.Count == 0)
                    return;
                DANH_SACH_KHE_UOC_XU_LY_NO objXuLyNoQH = (DANH_SACH_KHE_UOC_XU_LY_NO)grdKheUoc.SelectedItems[0];
                ucXuLyNoKheUoc01 objKheUoc = new ucXuLyNoKheUoc01();
                objKheUoc.decDuPhongChung = _decDuPhongChung;
                objKheUoc.listKUocXLN = _lstDSKUocXLN;
                objKheUoc.objKUocXLN = objXuLyNoQH;
                objKheUoc.TrangThai = "EDIT";
                objKheUoc.GetListXoaNo = new ucXuLyNoKheUoc01.LayDSXoaNo(LayDSKuocXoaNo);
                Window window = new Window();
                window.Title = "Chi tiết xử lý nợ khế ước";
                window.Content = objKheUoc;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            if (grdKheUoc.SelectedItems.Count == 0)
                return;
            foreach (DANH_SACH_KHE_UOC_XU_LY_NO objXuLy in grdKheUoc.SelectedItems)
            {
                _lstDSKUocXLN.Remove(objXuLy);
            }
            LoadGridViewData();
        }

        void ClearForm()
        {
            txtSoPhieu.Text = "";
            teldtNgayXoaNo.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            txtDienGiai.Text = "";
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, TThaiNVu);
            _lstDSKUocXLN = null;
            _lstKuocLuuXLN = null;
            grdKheUoc.ItemsSource = null;
        }
        #endregion

        /// <summary>
        /// Xu ly nghiep vu
        /// </summary>
        #region Xu ly nghiep vu
        void LoadGridViewData()
        {
            grdKheUoc.ItemsSource = _lstDSKUocXLN;
            grdKheUoc.Rebind();
        }

        private void LoadForm()
        {
            try
            {
                if (_objKiemSoat != null)
                {
                    _objXuLyNo = new TDVM_XY_LY_NO();
                    _lstKuocLuuXLN = new List<DANH_SACH_KHE_UOC_XU_LY_NO>();
                    _lstDSKUocXLN = new List<DANH_SACH_KHE_UOC_XU_LY_NO>();
                    List<DANH_SACH_TSDB> lstTSDB = new List<DANH_SACH_TSDB>();
                    DANH_SACH_TSDB objTSDB = new DANH_SACH_TSDB();
                    DANH_SACH_KHE_UOC_XU_LY_NO objKUCLN = new DANH_SACH_KHE_UOC_XU_LY_NO();
                    DataSet dsKetQua = new DataSet();
                    DataTable dtTTinGDich = new DataTable();
                    DataTable dtTTinKUoc = new DataTable();
                    DataTable dtTTinTSan = new DataTable();
                    TinDungProcess tdProcess = new TinDungProcess();
                    dsKetQua = tdProcess.LayTTinGDichXLN(_objKiemSoat.SO_GIAO_DICH);
                    if (dsKetQua != null && dsKetQua.Tables.Count >= 3)
                    {
                        dtTTinGDich = dsKetQua.Tables[0];
                        dtTTinKUoc = dsKetQua.Tables[1];
                        dtTTinTSan = dsKetQua.Tables[2];

                        #region Lay thong tin giao dich
                        _objXuLyNo.DIEN_GIAI = dtTTinGDich.Rows[0]["DIEN_GIAI"].ToString();
                        _objXuLyNo.ID_GIAO_DICH = _objKiemSoat.ID;
                        _objXuLyNo.LOAI_TIEN = dtTTinGDich.Rows[0]["LOAI_TIEN"].ToString();
                        _objXuLyNo.LY_DO = dtTTinGDich.Rows[0]["LY_DO"].ToString();
                        _objXuLyNo.MA_DVI = _objXuLyNo.MA_DVI_GD = dtTTinGDich.Rows[0]["MA_DVI_TAO"].ToString();
                        _objXuLyNo.MA_GIAO_DICH = _objKiemSoat.SO_GIAO_DICH;
                        _objXuLyNo.NGAY_CAP_NHAT = _objXuLyNo.NGAY_GIAO_DICH = _objXuLyNo.NGAY_LAP = dtTTinGDich.Rows[0]["NGAY_DL"].ToString();
                        _objXuLyNo.NGUOI_CAP_NHAT = _objXuLyNo.NGUOI_LAP = dtTTinGDich.Rows[0]["NGUOI_NHAP"].ToString();
                        #endregion

                        #region Lay thong tin khe uoc xu ly no
                        for (int i = 0; i < dtTTinKUoc.Rows.Count; i++)
                        {
                            objKUCLN = new DANH_SACH_KHE_UOC_XU_LY_NO();
                            objKUCLN.CHI_PHI = Convert.ToDecimal(dtTTinKUoc.Rows[i]["chi_phi"]);
                            objKUCLN.DU_NO_GOC = Convert.ToDecimal(dtTTinKUoc.Rows[i]["du_no_goc"]);
                            objKUCLN.DU_NO_LAI = Convert.ToDecimal(dtTTinKUoc.Rows[i]["du_no_lai"]);
                            objKUCLN.DU_PHONG_CHUNG = Convert.ToDecimal(dtTTinKUoc.Rows[i]["du_phong_chung"]);
                            objKUCLN.DU_PHONG_CU_THE = Convert.ToDecimal(dtTTinKUoc.Rows[i]["du_phong_cu_the"]);
                            objKUCLN.DU_PHONG_CU_THE_TRUOC_XLY = Convert.ToDecimal(dtTTinKUoc.Rows[i]["du_phong_cu_the_truoc_xly"]);
                            objKUCLN.GOC_DUOC_XU_LY = Convert.ToDecimal(dtTTinKUoc.Rows[i]["goc_duoc_xu_ly"]);
                            objKUCLN.GTRI_TAI_SAN = Convert.ToDecimal(dtTTinKUoc.Rows[i]["gtri_tai_san"]);
                            objKUCLN.ID_DON_VI = Convert.ToInt32(dtTTinKUoc.Rows[i]["id_don_vi"]);
                            objKUCLN.ID_KHACH_HANG = Convert.ToInt32(dtTTinKUoc.Rows[i]["id_khach_hang"]);
                            objKUCLN.ID_KHE_UOC = Convert.ToInt32(dtTTinKUoc.Rows[i]["ID_KUOC"]);
                            objKUCLN.LAI_DUOC_XU_LY = Convert.ToDecimal(dtTTinKUoc.Rows[i]["lai_duoc_xu_ly"]);
                            objKUCLN.MA_DON_VI = dtTTinKUoc.Rows[i]["ma_don_vi"].ToString();
                            objKUCLN.MA_KHACH_HANG = dtTTinKUoc.Rows[i]["ma_khach_hang"].ToString();
                            objKUCLN.MA_KHE_UOC = dtTTinKUoc.Rows[i]["ma_khe_uoc"].ToString();
                            objKUCLN.MA_SAN_PHAM = dtTTinKUoc.Rows[i]["ma_san_pham"].ToString();
                            objKUCLN.NGAY_VAY = dtTTinKUoc.Rows[i]["ngay_vay"].ToString();
                            objKUCLN.SO_TIEN_VAY = Convert.ToDecimal(dtTTinKUoc.Rows[i]["so_tien_vay"]);
                            objKUCLN.TEN_DON_VI = dtTTinKUoc.Rows[i]["TEN_GDICH"].ToString();
                            objKUCLN.TEN_KHACH_HANG = dtTTinKUoc.Rows[i]["ten_khach_hang"].ToString();
                            objKUCLN.THOI_HAN_VAY = dtTTinKUoc.Rows[i]["thoi_han_vay"].ToString();
                            objKUCLN.XUAT_GOC_NGOAI_BANG = dtTTinKUoc.Rows[i]["xuat_goc_ngoai_bang"].ToString();
                            objKUCLN.XUAT_LAI_NGOAI_BANG = dtTTinKUoc.Rows[i]["xuat_lai_ngoai_bang"].ToString();
                            objKUCLN.LOAI_XU_LY_NO = dtTTinKUoc.Rows[i]["LOAI_XU_LY_NO"].ToString();

                            #region lay tai san dam bao cua khe uoc
                            for (int j = 0; j < dtTTinTSan.Rows.Count; i++)
                            {
                                if (dtTTinTSan.Rows[j]["ma_khe_uoc"].Equals(objKUCLN.MA_KHE_UOC))
                                {
                                    objTSDB = new DANH_SACH_TSDB();
                                    objTSDB.GTRI_CON_DBAO = Convert.ToDecimal(dtTTinTSan.Rows[j]["gtri_con_dbao"]);
                                    objTSDB.GTRI_DAM_BAO = Convert.ToDecimal(dtTTinTSan.Rows[j]["gtri_dam_bao"]);
                                    objTSDB.GTRI_DBAO_DTUONG = Convert.ToDecimal(dtTTinTSan.Rows[j]["gtri_dam_bao"]);
                                    objTSDB.GTRI_DINH_GIA = Convert.ToDecimal(dtTTinTSan.Rows[j]["gtri_dinh_gia"]);
                                    objTSDB.GTRI_TSDB_XLN = Convert.ToDecimal(dtTTinTSan.Rows[j]["gtri_tsdb_xln"]);
                                    objTSDB.GTRI_TY_LE = Convert.ToDecimal(dtTTinTSan.Rows[j]["gtri_ty_le"]);
                                    objTSDB.ID_TSDB = Convert.ToInt32(dtTTinTSan.Rows[j]["id_tsdb"]);
                                    objTSDB.MA_HDTC = dtTTinTSan.Rows[j]["ma_hdtc"].ToString();
                                    objTSDB.MA_LOAI_TSDB = dtTTinTSan.Rows[j]["ma_loai_tsdb"].ToString();
                                    objTSDB.MA_THAM_CHIEU = dtTTinTSan.Rows[j]["ma_tham_chieu"].ToString();
                                    objTSDB.MA_TSDB = dtTTinTSan.Rows[j]["ma_tsdb"].ToString();
                                    objTSDB.NGAY_DINH_GIA = dtTTinTSan.Rows[j]["ngay_dinh_gia"].ToString();
                                    objTSDB.NGAY_HET_HLUC = dtTTinTSan.Rows[j]["ngay_het_hluc"].ToString();
                                    objTSDB.TEN_TAI_SAN = dtTTinTSan.Rows[j]["ten_tai_san"].ToString();
                                    lstTSDB.Add(objTSDB);
                                }
                            }
                            #endregion
                            objKUCLN.lstDSTaiSanDB = lstTSDB.ToArray(); ;
                            _lstKuocLuuXLN.Add(objKUCLN);
                            lstTSDB = new List<DANH_SACH_TSDB>();
                        }
                        _lstDSKUocXLN = _lstKuocLuuXLN;
                        #endregion

                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(_objXuLyNo.TRANG_THAI_NGHIEP_VU);

                        _objXuLyNo.DSACH_KHE_UOC = _lstDSKUocXLN.ToArray();
                        txtDienGiai.Text = _objXuLyNo.DIEN_GIAI;
                        txtSoPhieu.Text = _objKiemSoat.SO_GIAO_DICH;
                        teldtNgayXoaNo.Value = LDateTime.StringToDate(_objXuLyNo.NGAY_GIAO_DICH, "yyyyMMdd");
                        LoadGridViewData();
                    }
                    SetEnableControl();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                CommonFunction.ThongBaoLoi(ex);
            }
        }

        /// <summary>
        /// Lay thong tin tren form
        /// </summary>
        /// <returns></returns>
        private int GetValueOnForm(string sTThaiNVu)
        {
            int iRet = 1;
            try
            {
                if (_objXuLyNo == null)
                {
                    _objXuLyNo = new TDVM_XY_LY_NO();
                    _objXuLyNo.ID_GIAO_DICH = 0;
                }
                _objXuLyNo.DIEN_GIAI = txtDienGiai.Text;
                _objXuLyNo.DSACH_KHE_UOC = _lstKuocLuuXLN.ToArray();
                _objXuLyNo.LOAI_TIEN = ClientInformation.MaDongNoiTe;
                _objXuLyNo.LY_DO = "";
                _objXuLyNo.MA_DVI = ClientInformation.MaDonViGiaoDich;
                _objXuLyNo.MA_DVI_GD = ClientInformation.MaDonViGiaoDich;
                _objXuLyNo.MA_GIAO_DICH = txtSoPhieu.Text.Trim();
                _objXuLyNo.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                _objXuLyNo.NGAY_GIAO_DICH = Convert.ToDateTime(teldtNgayXoaNo.Value).ToString("yyyyMMdd");
                _objXuLyNo.NGAY_LAP = ClientInformation.NgayLamViecHienTai;
                _objXuLyNo.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                _objXuLyNo.NGUOI_LAP = ClientInformation.TenDangNhap;
                _objXuLyNo.TRANG_THAI_BAN_GHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                _objXuLyNo.TRANG_THAI_NGHIEP_VU = sTThaiNVu;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                iRet = 0;
            }
            return iRet;
        }

        private int LayDSKheUocXuLyNo(ref List<DANH_SACH_KHE_UOC_XU_LY_NO> lstKUXLN)
        {
            int kq = 1;
            _lstKuocLuuXLN = new List<DANH_SACH_KHE_UOC_XU_LY_NO>();
            try
            {
                foreach (DANH_SACH_KHE_UOC_XU_LY_NO obj in grdKheUoc.SelectedItems)
                {
                    _lstKuocLuuXLN.Add(obj);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                kq = 0;
            }
            return kq;
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
            bool bRet = true;
            if (grdKheUoc.Items.Count == 0)
            {
                LMessage.ShowMessage("M.TinDungTT.XoaNo.ucXoaNoCT01.LoiKhongCoKheUoc", LMessage.MessageBoxType.Warning);
                bRet = false;
            }
            else if (txtDienGiai.Text.Trim() == "")
            {
                CommonFunction.ThongBaoTrong(lblDienGiai.Content.ToString());
                bRet = false;
            }
            return bRet;
        }

        private void BeforeSave()
        {
            if (!Validation()) return;
            int iRet = 1;
            iRet = GetValueOnForm(TThaiNVu);
            if (iRet == 1) onSave();
        }

        private void onSave()
        {
            List<ClientResponseDetail> lstClientResponseDetail = new List<ClientResponseDetail>();
            int iRet = 1;
            TinDungProcess tdprocess = new TinDungProcess();
            if (_objXuLyNo.ID_GIAO_DICH == 0) iRet = tdprocess.XuLyNo(ref _objXuLyNo, ref lstClientResponseDetail, DatabaseConstant.Action.THEM);
            else iRet = tdprocess.XuLyNo(ref _objXuLyNo, ref lstClientResponseDetail, DatabaseConstant.Action.SUA);

            AfterSave(iRet, lstClientResponseDetail);
        }

        private void SetEnableControl()
        {
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_XU_LY_NO);
            if (action == DatabaseConstant.Action.XEM)
            {
                btnAdd.IsEnabled = false;
                btnDelete.IsEnabled = false;
                btnModify.IsEnabled = false;
                grdKheUoc.IsEnabled = false;
                txtDienGiai.IsEnabled = false;
                dtpNgayXoaNo.IsEnabled = false;
            }
            else if (action == DatabaseConstant.Action.SUA)
            {
                btnAdd.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnModify.IsEnabled = true;
                grdKheUoc.IsEnabled = true;
                txtDienGiai.IsEnabled = true;
                dtpNgayXoaNo.IsEnabled = true;
            }
            else if (action == DatabaseConstant.Action.THEM)
            {
                btnAdd.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnModify.IsEnabled = true;
                grdKheUoc.IsEnabled = true;
                txtDienGiai.IsEnabled = true;
                dtpNgayXoaNo.IsEnabled = true;
                ClearForm();
            }
        }

        private void AfterSave(int iRet, List<ClientResponseDetail> lstResponseDetail)
        {
            if (iRet == 1)
            {
                LMessage.ShowMessage("M.DungChung.LuuDuLieuThanhCong", LMessage.MessageBoxType.Information);
                action = DatabaseConstant.Action.XEM;
                //Yeu cau lock ban ghi
                List<int> listID = new List<int>();
                for (int i = 0; i < _lstKuocLuuXLN.Count; i++) listID.Add(_lstKuocLuuXLN[i].ID_KHE_UOC);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                                                    DatabaseConstant.Function.TDVM_XU_LY_NO,
                                                    DatabaseConstant.Table.KT_GIAO_DICH,
                                                    action,
                                                    listID);
                if (retLockData)
                {
                    txtSoPhieu.Text = _objXuLyNo.MA_GIAO_DICH;
                    TThaiNVu = _objXuLyNo.TRANG_THAI_NGHIEP_VU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
                    SetEnableControl();
                    CommonFunction.RefreshButton(Toolbar, action, TThaiNVu);
                }
            }
            else
            {
                CommonFunction.ThongBaoKetQua(lstResponseDetail);
            }
        }

        private void BeforeModify()
        {
            action = DatabaseConstant.Action.SUA;
            SetEnableControl();
            TThaiNVu = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();

            //yeu cau unlock ban ghi
            List<int> listID = new List<int>();
            UtilitiesProcess utiProcess = new UtilitiesProcess();
            listID.Add(_objXuLyNo.ID_GIAO_DICH);
            utiProcess.UnlockData(DatabaseConstant.Module.TDVM,
                                DatabaseConstant.Function.TDVM_XU_LY_NO,
                                DatabaseConstant.Table.KT_GIAO_DICH,
                                action,
                                listID);
        }

        private void BeforeDelete()
        {
            UtilitiesProcess utiProcess = new UtilitiesProcess();
            List<int> listID = new List<int>();
            try
            {
                action = DatabaseConstant.Action.XOA;
                if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) == MessageBoxResult.No) return;
                //yeu cau lock ban ghi                
                listID.Add(_objXuLyNo.ID_GIAO_DICH);
                bool bLockData = utiProcess.LockData(DatabaseConstant.Module.TDVM,
                                    DatabaseConstant.Function.TDVM_XU_LY_NO,
                                    DatabaseConstant.Table.KT_GIAO_DICH,
                                    action,
                                    listID);
                if (bLockData)
                {
                    OnDelete();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                CommonFunction.ThongBaoLoi(ex);
                utiProcess.UnlockData(DatabaseConstant.Module.TDVM,
                                    DatabaseConstant.Function.TDVM_XU_LY_NO,
                                    DatabaseConstant.Table.KT_GIAO_DICH,
                                    action,
                                    listID);
            }
        }

        private void OnDelete()
        {
            TinDungProcess tdprocess = new TinDungProcess();
            List<ClientResponseDetail> lstClientResponseDetail = new List<ClientResponseDetail>();
            List<int> listID = new List<int>();
            UtilitiesProcess utiProcess = new UtilitiesProcess();
            try
            {
                int iRet = 1;
                listID.Add(_objXuLyNo.ID_GIAO_DICH);
                iRet = tdprocess.XuLyNo(ref _objXuLyNo, ref lstClientResponseDetail, action);
                AfterDelete(iRet, lstClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                utiProcess.UnlockData(DatabaseConstant.Module.TDVM,
                                    DatabaseConstant.Function.TDVM_XU_LY_NO,
                                    DatabaseConstant.Table.KT_GIAO_DICH,
                                    action,
                                    listID);
            }
        }

        private void AfterDelete(int iRet, List<ClientResponseDetail> lstClientResponseDetail)
        {
            UtilitiesProcess utiProcess = new UtilitiesProcess();
            List<int> listID = new List<int>();
            listID.Add(_objXuLyNo.ID_GIAO_DICH);
            if (iRet == 1)
            {
                LMessage.ShowMessage("M_ResponseMessage_GIAODICH_XoaThanhCong", LMessage.MessageBoxType.Information);
                action = DatabaseConstant.Action.THEM;
                TThaiNVu = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
                ClearForm();
                SetEnableControl();
                CommonFunction.RefreshButton(Toolbar, action, TThaiNVu);
            }
            else
            {
                CommonFunction.ThongBaoKetQua(lstClientResponseDetail);
            }
            utiProcess.UnlockData(DatabaseConstant.Module.TDVM,
                    DatabaseConstant.Function.TDVM_XU_LY_NO,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    action,
                    listID);
        }

        private void BeforeApprove()
        {
            UtilitiesProcess utiProcess = new UtilitiesProcess();
            action = DatabaseConstant.Action.DUYET;
            List<int> listID = new List<int>();
            try
            {
                if (_objXuLyNo != null && _objXuLyNo.ID_GIAO_DICH > 0)
                {
                    listID.Add(_objXuLyNo.ID_GIAO_DICH);
                    bool bLockdata = utiProcess.LockData(DatabaseConstant.Module.TDVM,
                                        DatabaseConstant.Function.TDVM_XU_LY_NO,
                                        DatabaseConstant.Table.KT_GIAO_DICH,
                                        action,
                                        listID);
                    if (bLockdata)
                    {
                        OnApprove();
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                CommonFunction.ThongBaoLoi(ex);
            }
            finally
            {
                utiProcess.UnlockData(DatabaseConstant.Module.TDVM,
                    DatabaseConstant.Function.TDVM_XU_LY_NO,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    action,
                    listID);
            }
        }

        private void OnApprove()
        {
            UtilitiesProcess utiProcess = new UtilitiesProcess();
            TinDungProcess tdProcess = new TinDungProcess();
            List<int> listID = new List<int>();
            List<ClientResponseDetail> lstClientResponseDetail = new List<ClientResponseDetail>();
            try
            {
                listID.Add(_objXuLyNo.ID_GIAO_DICH);
                int iRet = tdProcess.XuLyNo(ref _objXuLyNo, ref lstClientResponseDetail, action);
                AfterApprove(iRet, lstClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                CommonFunction.ThongBaoLoi(ex);
            }
            finally
            {
                utiProcess.UnlockData(DatabaseConstant.Module.TDVM,
                                    DatabaseConstant.Function.TDVM_XU_LY_NO,
                                    DatabaseConstant.Table.KT_GIAO_DICH,
                                    action,
                                    listID);
            }
        }

        private void AfterApprove(int iRet, List<ClientResponseDetail> lstClientResponseDetail)
        {
            UtilitiesProcess utiProcess = new UtilitiesProcess();
            List<int> listID = new List<int>();
            listID.Add(_objXuLyNo.ID_GIAO_DICH);
            if (iRet == 1)
            {
                LMessage.ShowMessage("M_ResponseMessage_GIAODICH_DuyetThanhCong", LMessage.MessageBoxType.Information);
                action = DatabaseConstant.Action.XEM;
                TThaiNVu = _objXuLyNo.TRANG_THAI_NGHIEP_VU;
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
                SetEnableControl();
                CommonFunction.RefreshButton(Toolbar, action, TThaiNVu);
            }
            else
            {
                CommonFunction.ThongBaoKetQua(lstClientResponseDetail);
            }

            utiProcess.UnlockData(DatabaseConstant.Module.TDVM,
                    DatabaseConstant.Function.TDVM_XU_LY_NO,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    action,
                    listID);
        }


        private void BeforeRefuse()
        {
            if (_objXuLyNo.ID_GIAO_DICH > 0)
            {
                action = DatabaseConstant.Action.TU_CHOI_DUYET;
                List<int> lstId = new List<int>();
                lstId.Add(_objXuLyNo.ID_GIAO_DICH);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                                                DatabaseConstant.Function.TDVM_XU_LY_NO,
                                                DatabaseConstant.Table.KT_GIAO_DICH,
                                                action,
                                                lstId);
                if (retLockData)
                    OnRefuse();
            }
        }

        private void OnRefuse()
        {
            UtilitiesProcess utiProcess = new UtilitiesProcess();
            List<ClientResponseDetail> lstClientResponseDetail = new List<ClientResponseDetail>();
            TinDungProcess tdProcess = new TinDungProcess();
            int iRet = tdProcess.XuLyNo(ref _objXuLyNo, ref lstClientResponseDetail, action);
            AfterRefuse(iRet, lstClientResponseDetail);
        }

        private void AfterRefuse(int iRet, List<ClientResponseDetail> lstClienResponseDetail)
        {
            UtilitiesProcess utiProcess = new UtilitiesProcess();
            List<int> listID = new List<int>();
            listID.Add(_objXuLyNo.ID_GIAO_DICH);
            if (iRet == 1)
            {
                LMessage.ShowMessage("TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);
                action = DatabaseConstant.Action.XEM;
                TThaiNVu = _objXuLyNo.TRANG_THAI_NGHIEP_VU;
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
                SetEnableControl();
                CommonFunction.RefreshButton(Toolbar, action, TThaiNVu);
            }
            else
            {
                CommonFunction.ThongBaoKetQua(lstClienResponseDetail);
            }

            utiProcess.UnlockData(DatabaseConstant.Module.TDVM,
                                    DatabaseConstant.Function.TDVM_XU_LY_NO,
                                    DatabaseConstant.Table.KT_GIAO_DICH,
                                    action,
                                    listID);
        }

        private void BeforeCancel()
        {
            if (_objXuLyNo.ID_GIAO_DICH > 0)
            {
                action = DatabaseConstant.Action.THOAI_DUYET;
                List<int> lstId = new List<int>();
                lstId.Add(_objXuLyNo.ID_GIAO_DICH);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                                                DatabaseConstant.Function.TDVM_XU_LY_NO,
                                                DatabaseConstant.Table.KT_GIAO_DICH,
                                                action,
                                                lstId);
                if (retLockData)
                    OnCancel();
            }
        }
        private void OnCancel()
        {
            TinDungProcess tdProcess = new TinDungProcess();
            int iRet = 1;
            List<ClientResponseDetail> lstClienResponseDetail = new List<ClientResponseDetail>();
            iRet = tdProcess.XuLyNo(ref _objXuLyNo, ref lstClienResponseDetail, action);
            AfterCancel(iRet, lstClienResponseDetail);
        }
        private void AfterCancel(int iRet, List<ClientResponseDetail> lstClientResponseDetail)
        {
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> lstID = new List<int>();
            lstID.Add(_objXuLyNo.ID_GIAO_DICH);
            if (iRet == 1)
            {
                LMessage.ShowMessage("M_ResponseMessage_GIAODICH_ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);
                action = DatabaseConstant.Action.XEM;
                TThaiNVu = _objXuLyNo.TRANG_THAI_NGHIEP_VU;
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
                SetEnableControl();
                CommonFunction.RefreshButton(Toolbar, action, TThaiNVu);
            }
            else
            {
                CommonFunction.ThongBaoKetQua(lstClientResponseDetail);
            }

            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                                DatabaseConstant.Function.TDVM_XU_LY_NO,
                                DatabaseConstant.Table.KT_GIAO_DICH,
                                action,
                                lstID);
        }
        #endregion
    }
}
