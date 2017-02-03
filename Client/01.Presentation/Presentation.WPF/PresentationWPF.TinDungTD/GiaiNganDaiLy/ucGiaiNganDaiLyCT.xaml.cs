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
using System.Windows.Threading;
using System.Data;
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using PresentationWPF.CustomControl;
using Presentation.Process.TinDungTDServiceRef;
using Telerik.Windows.Controls;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.Common;
using System.Reflection;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;
using System.Collections;
using Telerik.Windows.Data;

namespace PresentationWPF.TinDungTD.GiaiNganDaiLy
{
    /// <summary>
    /// Interaction logic for ucGiaiNganDaiLyCT.xaml
    /// </summary>
    public partial class ucGiaiNganDaiLyCT : UserControl
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

        private DatabaseConstant.Function function = DatabaseConstant.Function.TDTD_GIAI_NGAN_DAI_LY;
        string maDonViQLy = ClientInformation.MaDonVi;
        string maDonViGDich = ClientInformation.MaDonViGiaoDich;
        List<AutoCompleteEntry> lstSourceCanBo = new List<AutoCompleteEntry>();
        List<DataRow> lstPopup = new List<DataRow>();
        DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public event EventHandler OnSavingCompleted;
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        string TKhoanTUng = "";
        string NgayPhatVon = "";
        string NguonVon = "";
        int iDGDTamUng = 0;
        string TThaiNV = "";
        int iDGiaDich = 0;
        int iDCanBo = 0;
        double soTienTamUng = 0;
        private KIEM_SOAT _objKiemSoat = null;
        TDTD_GIAI_NGAN_DAI_LY objGiaiNganDaiLy = new TDTD_GIAI_NGAN_DAI_LY();
        List<THONG_TIN_KHE_UOC_GNGAN_DAI_LY> lstGiaiNganDaiLyKUOC = new List<THONG_TIN_KHE_UOC_GNGAN_DAI_LY>();
        bool isLoad = true;
        List<BIEU_PHI_CTIET_DTO> lstBieuPhi = null;
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucGiaiNganDaiLyCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDungTD;component/GiaiNganDaiLy/ucGiaiNganDaiLyCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            ClearForm();
            btnMaDaiLy.Focus();
            InitEventHanler();
            ShowControl();
        }

        public ucGiaiNganDaiLyCT(KIEM_SOAT objKiemSoat) : this()
        {
            _objKiemSoat = objKiemSoat;
            action = _objKiemSoat.action;
            SetDataForm();
        }

        void InitEventHanler()
        {
            var tongTT = new AggregateFunction<THONG_TIN_KHE_UOC_GNGAN_DAI_LY, decimal>
            {
                AggregationExpression = ku => ku.Sum(r => r.SO_TIEN_GNGAN - r.PHI_MO_SO),
                ResultFormatString = "{0:n0}",
            };
            GridViewExpressionColumn columnTT = this.raddgrDSachPhatVay.Columns["SO_TIEN_CHUYEN_KHOAN"] as GridViewExpressionColumn;
            columnTT.AggregateFunctions.Add(tongTT);

            btnMaDaiLy.Click += btnMaDaiLy_Click;
            btnThemCumNhom.Click += btnThemCumNhom_Click;
        }

        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.TinDungTD.GiaiNganDaiLy.ucGiaiNganDaiLyCT", "");
            foreach (List<string> lst in arr)
            {
                object item = grMain.FindName(lst.First());
                string strProperty = lst.ElementAt(1);
                PropertyInfo prty = item.GetType().GetProperty(strProperty);
                if (strProperty.Equals("Visibility"))
                {
                    if (lst.ElementAt(2).Equals("0"))
                        prty.SetValue(item, Visibility.Collapsed, null);
                    else if (lst.ElementAt(2).Equals("1"))
                        prty.SetValue(item, Visibility.Visible, null);
                    else
                        prty.SetValue(item, Visibility.Hidden, null);
                }
                else
                {
                    if (lst.ElementAt(2).Equals("0"))
                        prty.SetValue(item, false, null);
                    else
                        prty.SetValue(item, true, null);
                }
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
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                OnModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                BeforeSave(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET,BusinessConstant.TrangThaiBanGhi.SU_DUNG);
            }
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
            else if (strTinhNang.Equals("PreviewDanhSach"))
            {
                OnPreviewDanhSach();
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
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                OnModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                BeforeSave(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET, BusinessConstant.TrangThaiBanGhi.SU_DUNG);
            }
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
            else if (strTinhNang.Equals("PreviewDanhSach"))
            {
                OnPreviewDanhSach();
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

        void ClearForm()
        {
            txtSoPhieu.Text = "";
            TThaiNV = "";
            teldtNgayGD.Value = ClientInformation.NgayLamViecHienTai.StringToDate(ApplicationConstant.defaultDateTimeFormat);
            maDonViQLy = ClientInformation.MaDonVi;
            maDonViGDich = ClientInformation.MaDonViGiaoDich;
            txtDienGiai.Text = "";
            lblLabelTrangThai.Content = "";
            txtTrangThai.Text = "";
            teldtNgayCNhat.Value = null;
            teldtNgayNhap.Value = ClientInformation.NgayLamViecHienTai.StringToDate(ApplicationConstant.defaultDateTimeFormat);
            txtNguoiCapNhat.Text = "";
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            txtMaDaiLy.Text = "";
            txtSoTaiKhoan.Text = "";
            action = DatabaseConstant.Action.THEM;
            CommonFunction.RefreshButton(Toolbar, action, TThaiNV, mnuMain, function);
        }

        void SetEnabledAllControl(bool bBool)
        {
            txtDienGiai.IsEnabled = bBool;
            raddgrDSachPhatVay.IsReadOnly = !bBool;
            txtMaDaiLy.IsEnabled = bBool;
            btnMaDaiLy.IsEnabled = bBool;
        }

        
        void btnPhiMoSo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                THONG_TIN_KHE_UOC_GNGAN_DAI_LY objTamUngKUOC = raddgrDSachPhatVay.SelectedItem as THONG_TIN_KHE_UOC_GNGAN_DAI_LY;
                int idBieuPhi = 0;
                lstPopup.Clear();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonVi);
                lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
                lstDieuKien.Add("'GNDL03'");
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_BIEU_PHI_LOAI_GDICH", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(false, simplePopupResponse);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.Content = popup;
                win.ShowDialog();
                if (lstPopup.Count > 0)
                {
                    DataRow dr = lstPopup.FirstOrDefault();
                    idBieuPhi = Convert.ToInt32(dr["ID"]);
                    objTamUngKUOC.MA_PHI_MO_SO = dr["MA_BPHI"].ToString();
                    objTamUngKUOC.BIEU_PHI.MA_BPHI = dr["MA_BPHI"].ToString();
                    objTamUngKUOC.BIEU_PHI.ID_BPHI = idBieuPhi;
                    GetThongTinBieuPhiCTiet(ref objTamUngKUOC);
                    TinhPhiTraTruoc(ref objTamUngKUOC);
                    raddgrDSachPhatVay.CurrentItem = objTamUngKUOC;
                    raddgrDSachPhatVay.Rebind();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void btnThemCumNhom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add(teldtNgayGD.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat));
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_KUOC_GNGAN_DAILY_TIN_DUNG_TDUNG", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(false, simplePopupResponse, true);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (objGiaiNganDaiLy.IsNullOrEmpty())
                    objGiaiNganDaiLy = new TDTD_GIAI_NGAN_DAI_LY();
                if (lstGiaiNganDaiLyKUOC.IsNullOrEmpty())
                    lstGiaiNganDaiLyKUOC = new List<THONG_TIN_KHE_UOC_GNGAN_DAI_LY>();
                if (lstPopup.Count > 0)
                {
                    foreach (DataRow dr in lstPopup)
                    {
                        THONG_TIN_KHE_UOC_GNGAN_DAI_LY objKUOC = new THONG_TIN_KHE_UOC_GNGAN_DAI_LY();
                        objKUOC.LOAI_TIEN = dr["LOAI_TIEN"].ToString();
                        objKUOC.LOAI_TIEN_GN = dr["LOAI_TIEN"].ToString();
                        objKUOC.LAI_SUAT = Convert.ToDecimal(dr["LAI_SUAT"]);
                        objKUOC.LSUAT_DVT = dr["LSUAT_DVT"].ToString();
                        objKUOC.MA_KHANG = dr["MA_KHANG"].ToString();
                        objKUOC.NGAY_NHAN_NO = dr["NGAY_GIAI_NGAN"].ToString();
                        objKUOC.SO_KUOC = dr["SO_KUOC"].ToString();
                        objKUOC.SO_TIEN_GNGAN = Convert.ToDecimal(dr["SO_TIEN_PHAT_VAY"]);
                        objKUOC.SO_TIEN_CK = Convert.ToDecimal(dr["SO_TIEN_PHAT_VAY"]);
                        objKUOC.SO_TIEN_NHAN_NO = Convert.ToDecimal(dr["SO_TIEN_GNGAN"]);
                        objKUOC.TEN_KHANG = dr["TEN_KHANG"].ToString();
                        objKUOC.NV_LOAI_NVON = dr["NV_LOAI_NVON"].ToString();
                        objKUOC.MA_SAN_PHAM = dr["MA_SAN_PHAM"].ToString();
                        objKUOC.MA_KUOC = dr["MA_KUOC"].ToString();
                        objKUOC.SO_GTLQ_KHANG = dr["SO_GTLQ"].ToString();
                        objKUOC.NGAY_MUA_HANG = dr["NGAY_GIAI_NGAN"].ToString();
                        objKUOC.NGAY_DAO_HAN = dr["NGAY_DAO_HAN"].ToString();
                        objKUOC.DIA_CHI_KHANG = dr["DIA_CHI"].ToString();
                        objKUOC.MA_PHI_MO_SO = dr["MA_BPHI"].ToString();
                        objKUOC.PHI_MO_SO = Convert.ToDecimal(dr["SO_TIEN_PHI_MO_SO"]);
                        objKUOC.TGIAN_VAY = Convert.ToString(dr["TGIAN_VAY"]);
                        objKUOC.TGIAN_VAY_DVT = Convert.ToString(dr["TGIAN_VAY_DVT"]);

                        objKUOC.BIEU_PHI = new BIEU_PHI_DTO();
                        objKUOC.BIEU_PHI.HTHUC_BTHANG = dr["HTHUC_BTHANG"].ToString();
                        objKUOC.BIEU_PHI.ID_BPHI = Convert.ToInt32(dr["ID_BPHI"]);
                        objKUOC.BIEU_PHI.LOAI_BPHI = dr["LOAI_BPHI"].ToString();
                        objKUOC.BIEU_PHI.LOAI_TIEN = dr["MA_LOAI_TIEN"].ToString();
                        objKUOC.BIEU_PHI.MA_BPHI = dr["MA_BPHI"].ToString();
                        objKUOC.BIEU_PHI.NGAY_ADUNG = dr["NGAY_ADUNG"].ToString();
                        if (dr["NGAY_HHAN"] != DBNull.Value)
                            objKUOC.BIEU_PHI.NGAY_HHAN = dr["NGAY_HHAN"].ToString();
                        objKUOC.BIEU_PHI.TCHAT_BPHI = dr["TCHAT_BPHI"].ToString();
                        objKUOC.BIEU_PHI.TEN_BPHI = dr["TEN_BPHI"].ToString();
                        objKUOC.BIEU_PHI.TY_LE_VAT = Convert.ToDecimal(dr["TY_LE_VAT"]);
                        if (objKUOC.BIEU_PHI.ID_BPHI > 0)
                        {
                            GetThongTinBieuPhiCTiet(ref objKUOC);
                            objKUOC.BIEU_PHI.DSACH_BPHI_CT = lstBieuPhi.ToArray();
                        }
                        objKUOC.TTIN_SO_TK = new THONG_TIN_SO_TKIEM_GNGAN_DAI_LY();
                        lstGiaiNganDaiLyKUOC.Add(objKUOC);
                    }
                }
                raddgrDSachPhatVay.ItemsSource = lstGiaiNganDaiLyKUOC;
                raddgrDSachPhatVay.Rebind();
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void GetThongTinBieuPhiCTiet(ref THONG_TIN_KHE_UOC_GNGAN_DAI_LY objKUOC)
        {
            DataSet ds = new PhiProcess().GetPhiByID(objKUOC.BIEU_PHI.ID_BPHI);
            DataTable dt = ds.Tables[1];
            lstBieuPhi = new List<BIEU_PHI_CTIET_DTO>();
            foreach (DataRow dtr in dt.Rows)
            {
                BIEU_PHI_CTIET_DTO objBieuPhiCT = new BIEU_PHI_CTIET_DTO();
                objBieuPhiCT.ID_BPHI = objKUOC.BIEU_PHI.ID_BPHI;
                objBieuPhiCT.LOAI_BPHI = dtr["LOAI_BPHI"].ToString();
                objBieuPhiCT.MA_BPHI = dtr["MA_BPHI"].ToString();
                if (dtr["SO_TIEN"] != DBNull.Value)
                    objBieuPhiCT.SO_TIEN_TINH_PHI = Convert.ToDecimal(dtr["SO_TIEN"]);
                if (dtr["SO_TIEN_PHI"] != DBNull.Value)
                    objBieuPhiCT.SO_TIEN_PHI = Convert.ToDecimal(dtr["SO_TIEN_PHI"]);
                if (dtr["STIEN_PHI_TDA"] != DBNull.Value)
                    objBieuPhiCT.SO_TIEN_TDA = Convert.ToDecimal(dtr["STIEN_PHI_TDA"]);
                if (dtr["STIEN_PHI_TTHIEU"] != DBNull.Value)
                    objBieuPhiCT.SO_TIEN_TTHIEU = Convert.ToDecimal(dtr["STIEN_PHI_TTHIEU"]);
                if (dtr["TY_LE_PHI"] != DBNull.Value)
                    objBieuPhiCT.TY_LE_PHI = Convert.ToDecimal(dtr["TY_LE_PHI"]);
                if (dtr["TY_LE_VAT"] != DBNull.Value)
                    objBieuPhiCT.TY_LE_VAT = Convert.ToDecimal(dtr["TY_LE_VAT"]);
                lstBieuPhi.Add(objBieuPhiCT);

            }
            objKUOC.BIEU_PHI.DSACH_BPHI_CT = lstBieuPhi.ToArray();
        }

        private void TinhPhiTraTruoc(ref THONG_TIN_KHE_UOC_GNGAN_DAI_LY objKUOC)
        {
            decimal soDu = objKUOC.SO_TIEN_GNGAN;
            decimal soTienPhi = 0;
            decimal tyLe = 0;
            decimal soTien = 0;
            decimal soTienTThieu = 0;
            decimal soTienTDa = 0;
            lstBieuPhi = objKUOC.BIEU_PHI.DSACH_BPHI_CT.ToList();
            soTienTThieu = lstBieuPhi.FirstOrDefault().SO_TIEN_TTHIEU;
            soTienTDa = lstBieuPhi.FirstOrDefault().SO_TIEN_TDA;

            if (objKUOC.BIEU_PHI.TCHAT_BPHI.Equals(BusinessConstant.TCHAT_BPHI.DTH.layGiaTri()))
            {
                if (objKUOC.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.TY_LE.layGiaTri()))
                {
                    tyLe = lstBieuPhi.FirstOrDefault().TY_LE_PHI;
                }
                else if (objKUOC.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.SO_TIEN.layGiaTri()))
                {
                    soTien = lstBieuPhi.FirstOrDefault().SO_TIEN_PHI;
                }
                else if (objKUOC.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.STIEN_TLE.layGiaTri()))
                {
                    tyLe = lstBieuPhi.FirstOrDefault().TY_LE_PHI;
                    soTien = lstBieuPhi.FirstOrDefault().SO_TIEN_PHI;
                }
            }
            else
            {
            }
            if (objKUOC.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.TY_LE.layGiaTri()))
            {
                soTienPhi = soDu * (tyLe / 100);
            }
            else if (objKUOC.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.SO_TIEN.layGiaTri()))
            {
                soTienPhi = soTien;
            }
            else if (objKUOC.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.STIEN_TLE.layGiaTri()))
            {

            }
            if (soTienPhi < soTienTThieu)
                soTienPhi = soTienTThieu;
            if (soTienPhi > soTienTDa)
                soTienPhi = soTienTDa;
            objKUOC.PHI_MO_SO = soTienPhi;
            objKUOC.MA_PHI_MO_SO = objKUOC.BIEU_PHI.MA_BPHI;
        }

        private void btnMaDaiLy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lstPopup.Clear();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViQuanLy);
                lstDieuKien.Add("'DAILY'");
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_TCTD.getValue(), lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(false, simplePopupResponse);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.Content = popup;
                win.ShowDialog();
                if (lstPopup.Count > 0)
                {
                    txtMaDaiLy.Tag = Convert.ToInt32(lstPopup[0]["ID"]);
                    txtMaDaiLy.Text = lstPopup[0]["MA_TCTD"].ToString();
                    txtSoTaiKhoan.Text = lstPopup[0]["SO_TKHOAN"].ToString();
                    txtSoTaiKhoan.Tag = lstPopup[0]["MA_PLOAI"].ToString();
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        #endregion

        /// <summary>
        /// Xu ly nghiep vu
        /// </summary>
        #region Xu ly nghiep vu
        bool GetDataForm(BusinessConstant.TrangThaiNghiepVu nghiepvu, BusinessConstant.TrangThaiBanGhi sudung)
        {
            bool bBool = true;
            try
            {
                lstGiaiNganDaiLyKUOC = raddgrDSachPhatVay.ItemsSource as List<THONG_TIN_KHE_UOC_GNGAN_DAI_LY>;
                objGiaiNganDaiLy = new TDTD_GIAI_NGAN_DAI_LY();
                objGiaiNganDaiLy.DIEN_GIAI = txtDienGiai.Text;
                objGiaiNganDaiLy.MA_GDICH = txtSoPhieu.Text;
                objGiaiNganDaiLy.ID_GDICH = iDGiaDich;
                objGiaiNganDaiLy.LOAI_TIEN = ClientInformation.MaDongNoiTe;
                objGiaiNganDaiLy.HINH_THUC_TTOAN = BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN.layGiaTri();
                objGiaiNganDaiLy.MA_DVI_GD = ClientInformation.MaDonViGiaoDich;
                objGiaiNganDaiLy.MA_DVI_QLY = ClientInformation.MaDonVi;
                objGiaiNganDaiLy.MA_DAI_LY = txtMaDaiLy.Text;
                //objGiaiNganDaiLy.TEN_DAI_LY = txtMaDaiLy.Tag.ToString();
                objGiaiNganDaiLy.NGAY_PHAT_VON = teldtNgayGD.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                objGiaiNganDaiLy.NGAY_GDICH = teldtNgayGD.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                objGiaiNganDaiLy.NGUOI_LAP = txtNguoiLap.Text;
                objGiaiNganDaiLy.TEN_NGUOI_GDICH = ClientInformation.HoTen;
                objGiaiNganDaiLy.NGAY_LAP = LDateTime.DateToString(teldtNgayNhap.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
                objGiaiNganDaiLy.TTHAI_BGHI = sudung.layGiaTri();
                objGiaiNganDaiLy.TTHAI_NVU = nghiepvu.layGiaTri();
                objGiaiNganDaiLy.TAI_KHOAN = txtSoTaiKhoan.Tag.ToString();
                if (objGiaiNganDaiLy.ID_GDICH > 0)
                {
                    objGiaiNganDaiLy.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    objGiaiNganDaiLy.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }
                if (!LObject.IsNullOrEmpty(lstGiaiNganDaiLyKUOC))
                    objGiaiNganDaiLy.DSACH_KUOC = lstGiaiNganDaiLyKUOC.ToArray();
            }
            catch (Exception ex)
            {
                bBool = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return bBool;
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

        /// <summary>
        /// Xem báo cáo
        /// </summary>
        private void OnPreviewDanhSach()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(txtSoPhieu.Text))
            {
                LMessage.ShowMessage("Không có thông tin dữ liệu cần hiển thị", LMessage.MessageBoxType.Warning);

            }
            else
            {
                if (ClientInformation.Company.Equals("BANTAYVANG"))
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();

                    List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();
                    lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaGiaoDich", txtSoPhieu.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.HDVO_DANH_SACH_HOAN_TK);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
                else
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();

                    List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();
                    lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaGiaoDich", txtSoPhieu.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoDungChung(DatabaseConstant.DanhSachBaoCaoDungChung.HDVO_DANH_SACH_HOAN_TK);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        bool Validation()
        {
            if (txtMaDaiLy.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblMaDaiLy.Content.ToString());
                txtMaDaiLy.Focus();
                return false;
            }
            else if (txtSoTaiKhoan.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblSoTaiKhoan.Content.ToString());
                txtSoTaiKhoan.Focus();
                return false;
            }
            else if (txtSoTaiKhoan.Tag.IsNullOrEmpty())
            {
                CommonFunction.ThongBaoTrong(lblSoTaiKhoan.Content.ToString());
                txtSoTaiKhoan.Focus();
                return false;
            }
            else if (txtDienGiai.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblDienGiai.Content.ToString());
                txtDienGiai.Focus();
                return false;
            }
            return true;
        }

        void BeforeSave(BusinessConstant.TrangThaiNghiepVu nghiepvu, BusinessConstant.TrangThaiBanGhi bghi)
        {
            Cursor = Cursors.Wait;

            try
            {
                if (!nghiepvu.Equals(BusinessConstant.TrangThaiNghiepVu.LUU_TAM))
                {
                    if (!Validation())
                    {
                        Cursor = Cursors.Arrow;
                        return;
                    }
                }
                GetDataForm(nghiepvu, bghi);
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDTD,
                DatabaseConstant.Function.TDTD_GIAI_NGAN_DAI_LY,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                lstId);
                OnSave();
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                // Yêu cầu Unlock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                DatabaseConstant.Function.TDTD_GIAI_NGAN_DAI_LY,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                lstId);
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
                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                int iret = 0;
                if (objGiaiNganDaiLy.MA_GDICH.IsNullOrEmptyOrSpace())
                    iret = new TinDungTDProcess().GiaiNganDaiLy(DatabaseConstant.Action.THEM, ref objGiaiNganDaiLy, ref lstResponseDetail);
                else
                    iret = new TinDungTDProcess().GiaiNganDaiLy(DatabaseConstant.Action.SUA, ref objGiaiNganDaiLy, ref lstResponseDetail);
                AfterSave(lstResponseDetail, iret);
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
        void AfterSave(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            try
            {
                CommonFunction.ThongBaoKetQua(lstResponseDetail);
                // Yêu cầu Unlock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                DatabaseConstant.Function.TDTD_GIAI_NGAN_DAI_LY,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                lstId);
                if (!cbMultiAdd.IsChecked.GetValueOrDefault(false))
                {
                    if (iret > 0)
                    {
                        SetInfomation();
                    }
                }
                else
                {
                    if (iret > 0)
                        ClearForm();
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void BeforeDelete()
        {
            if (!tlbDelete.IsEnabled)
                return;
            if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            Cursor = Cursors.Wait;
            try
            {
                objGiaiNganDaiLy.MA_GDICH = txtSoPhieu.Text;
                objGiaiNganDaiLy.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                objGiaiNganDaiLy.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDTD,
                DatabaseConstant.Function.TDTD_GIAI_NGAN_DAI_LY,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.XOA,
                lstId);
                OnDelete();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                DatabaseConstant.Function.TDTD_GIAI_NGAN_DAI_LY,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.XOA,
                lstId);

                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally { Cursor = Cursors.Arrow; }
        }
        void OnDelete()
        {
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            iret = new TinDungTDProcess().GiaiNganDaiLy(DatabaseConstant.Action.XOA, ref objGiaiNganDaiLy, ref lstResponseDetail);
            AfterDelete(lstResponseDetail, iret);
        }
        void AfterDelete(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();

            Cursor = Cursors.Arrow;
            if (iret == 0) ;
            else
                CommonFunction.CloseUserControl(this);
        }

        void BeforeApprove()
        {
            if (!tlbApprove.IsEnabled)
                return;
            if (LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            try
            {
                Cursor = Cursors.Wait;
                objGiaiNganDaiLy.MA_GDICH = txtSoPhieu.Text;
                objGiaiNganDaiLy.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                objGiaiNganDaiLy.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDTD,
                DatabaseConstant.Function.TDTD_GIAI_NGAN_DAI_LY,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.DUYET,
                lstId);

                OnApprove();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                DatabaseConstant.Function.TDTD_GIAI_NGAN_DAI_LY,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.DUYET,
                lstId);

                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally { Cursor = Cursors.Arrow; }
        }
        void OnApprove()
        {
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            iret = new TinDungTDProcess().GiaiNganDaiLy(DatabaseConstant.Action.DUYET, ref objGiaiNganDaiLy, ref lstResponseDetail);
            AfterApprove(lstResponseDetail, iret);
        }
        void AfterApprove(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(iDGiaDich);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDTD,
            DatabaseConstant.Function.TDTD_GIAI_NGAN_DAI_LY,
            DatabaseConstant.Table.KT_GIAO_DICH,
            DatabaseConstant.Action.DUYET,
            lstId);

            SetInfomation();
            Cursor = Cursors.Arrow;
        }

        void BeforeRefuse()
        {
            if (!tlbRefuse.IsEnabled)
                return;
            if (LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            Cursor = Cursors.Wait;
            try
            {

                objGiaiNganDaiLy.MA_GDICH = txtSoPhieu.Text;
                objGiaiNganDaiLy.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                objGiaiNganDaiLy.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                Cursor = Cursors.Wait;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDTD,
                DatabaseConstant.Function.TDTD_GIAI_NGAN_DAI_LY,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);

                OnRefuse();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                DatabaseConstant.Function.TDTD_GIAI_NGAN_DAI_LY,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);

                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally { Cursor = Cursors.Arrow; }
        }
        void OnRefuse()
        {
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            iret = new TinDungTDProcess().GiaiNganDaiLy(DatabaseConstant.Action.TU_CHOI_DUYET, ref objGiaiNganDaiLy, ref lstResponseDetail);
            AfterRefuse(lstResponseDetail, iret);
        }
        void AfterRefuse(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(iDGiaDich);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDTD,
            DatabaseConstant.Function.TDTD_GIAI_NGAN_DAI_LY,
            DatabaseConstant.Table.KT_GIAO_DICH,
            DatabaseConstant.Action.TU_CHOI_DUYET,
            lstId);
            SetInfomation();
            Cursor = Cursors.Arrow;

        }

        void BeforeCancel()
        {
            if (!tlbCancel.IsEnabled)
                return;
            if (LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            Cursor = Cursors.Wait;
            try
            {

                objGiaiNganDaiLy.MA_GDICH = txtSoPhieu.Text;
                objGiaiNganDaiLy.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                objGiaiNganDaiLy.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                Cursor = Cursors.Wait;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDTD,
                DatabaseConstant.Function.TDTD_GIAI_NGAN_DAI_LY,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);

                OnCancel();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                DatabaseConstant.Function.TDTD_GIAI_NGAN_DAI_LY,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);

                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);

            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }
        void OnCancel()
        {
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            iret = new TinDungTDProcess().GiaiNganDaiLy(DatabaseConstant.Action.THOAI_DUYET, ref objGiaiNganDaiLy, ref lstResponseDetail);
            AfterCancel(lstResponseDetail, iret);
        }
        void AfterCancel(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(iDGiaDich);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDTD,
            DatabaseConstant.Function.TDTD_GIAI_NGAN_DAI_LY,
            DatabaseConstant.Table.KT_GIAO_DICH,
            DatabaseConstant.Action.THOAI_DUYET,
            lstId);
            SetInfomation();
            Cursor = Cursors.Arrow;
        }

        void SetInfomation()
        {
            TThaiNV = objGiaiNganDaiLy.TTHAI_NVU;
            iDGiaDich = objGiaiNganDaiLy.ID_GDICH;
            txtSoPhieu.Text = objGiaiNganDaiLy.MA_GDICH;
            txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(objGiaiNganDaiLy.TTHAI_BGHI);
            lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(objGiaiNganDaiLy.TTHAI_NVU);
            txtNguoiLap.Text = objGiaiNganDaiLy.NGUOI_LAP;
            txtNguoiCapNhat.Text = objGiaiNganDaiLy.NGUOI_CNHAT;
            teldtNgayNhap.Value = LDateTime.StringToDate(objGiaiNganDaiLy.NGAY_LAP, ApplicationConstant.defaultDateTimeFormat);
            if (!objGiaiNganDaiLy.NGAY_CNHAT.IsNullOrEmptyOrSpace())
                teldtNgayCNhat.Value = LDateTime.StringToDate(objGiaiNganDaiLy.NGAY_CNHAT, ApplicationConstant.defaultDateTimeFormat);
            SetEnabledAllControl(false);
            action = DatabaseConstant.Action.XEM;
            CommonFunction.RefreshButton(Toolbar, action, TThaiNV, mnuMain, function);
        }

        void OnModify()
        {
            SetEnabledAllControl(true);
            action = DatabaseConstant.Action.SUA;
            CommonFunction.RefreshButton(Toolbar, action, TThaiNV, mnuMain, function);
        }

        void SetDataForm()
        {
            try
            {
                TThaiNV = _objKiemSoat.TTHAI_NVU;
                iDGiaDich = _objKiemSoat.ID;
                txtSoPhieu.Text = _objKiemSoat.SO_GIAO_DICH;
                DataSet ds = new DataSet();
                DataTable dtPar = null;
                LDatatable.MakeParameterTable(ref dtPar);
                LDatatable.AddParameter(ref dtPar, "@SoGiaoDich", "String", _objKiemSoat.SO_GIAO_DICH);
                LDatatable.AddParameter(ref dtPar, "@IdGiaoDich", "String", _objKiemSoat.ID.ToString());
                ds = new TinDungTDProcess().GetThongTinGiaiNganDaiLy(dtPar);
                Dispatcher.CurrentDispatcher.DelayInvoke("SetDataForm", () =>
                {
                    SetTabThongTinChung(ds);

                    Dispatcher.CurrentDispatcher.DelayInvoke("SetTabThongKiemSoat", () =>
                    {
                        SetTabThongKiemSoat(ds);
                    }, TimeSpan.FromSeconds(0));

                }, TimeSpan.FromSeconds(0));
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void SetTabThongTinChung(DataSet ds)
        {
            try
            {

                DataTable dt = ds.Tables["TTIN_CHUNG"];
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    objGiaiNganDaiLy = new TDTD_GIAI_NGAN_DAI_LY();
                    TThaiNV = dr["TTHAI_NVU"].ToString();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNV);

                    objGiaiNganDaiLy.ID_GDICH = Convert.ToInt32(dr["ID_GDICH"]);
                    objGiaiNganDaiLy.MA_GDICH = Convert.ToString(dr["MA_GDICH"]);
                    objGiaiNganDaiLy.NGAY_PHAT_VON = dr["NGAY_PHAT_VON"].ToString();
                    objGiaiNganDaiLy.NGAY_GDICH = dr["NGAY_GIAO_DICH"].ToString();
                    objGiaiNganDaiLy.TTHAI_NVU = dr["TTHAI_NVU"].ToString();
                    objGiaiNganDaiLy.MA_DAI_LY = dr["MA_DAI_LY"].ToString();
                    objGiaiNganDaiLy.TAI_KHOAN = dr["TAI_KHOAN"].ToString();
                    txtDienGiai.Text = dr["DIEN_GIAI"].ToString();
                    txtSoPhieu.Text = objGiaiNganDaiLy.MA_GDICH;
                    teldtNgayGD.Value = LDateTime.StringToDate(objGiaiNganDaiLy.NGAY_GDICH, ApplicationConstant.defaultDateTimeFormat);
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(objGiaiNganDaiLy.TTHAI_NVU);
                    txtMaDaiLy.Tag = Convert.ToInt32(dr["ID_TCTD"]);
                    txtMaDaiLy.Text = dr["MA_DAI_LY"].ToString();
                    txtSoTaiKhoan.Text = dr["SO_TKHOAN"].ToString();
                    txtSoTaiKhoan.Tag = dr["TAI_KHOAN"].ToString();
                    CommonFunction.RefreshButton(Toolbar, action, TThaiNV, mnuMain, DatabaseConstant.Function.TDTD_GIAI_NGAN_DAI_LY);
                    if (action.Equals(DatabaseConstant.Action.SUA))
                        SetEnabledAllControl(true);
                    else
                        SetEnabledAllControl(false);
                }

                dt = ds.Tables["TTIN_KUOC"];
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    THONG_TIN_KHE_UOC_GNGAN_DAI_LY objKUOC = new THONG_TIN_KHE_UOC_GNGAN_DAI_LY();
                    objKUOC.LOAI_TIEN = dr["LOAI_TIEN"].ToString();
                    objKUOC.LOAI_TIEN_GN = dr["LOAI_TIEN"].ToString();
                    objKUOC.LAI_SUAT = Convert.ToDecimal(dr["LAI_SUAT"]);
                    objKUOC.LSUAT_DVT = dr["LSUAT_DVT"].ToString();
                    objKUOC.MA_KHANG = dr["MA_KHANG"].ToString();
                    objKUOC.NGAY_NHAN_NO = dr["NGAY_GIAI_NGAN"].ToString();
                    objKUOC.SO_KUOC = dr["SO_KUOC"].ToString();
                    objKUOC.SO_TIEN_GNGAN = Convert.ToDecimal(dr["SO_TIEN_PHAT_VAY"]);
                    objKUOC.SO_TIEN_CK = Convert.ToDecimal(dr["SO_TIEN_PHAT_VAY"]);
                    objKUOC.SO_TIEN_NHAN_NO = Convert.ToDecimal(dr["SO_TIEN_GNGAN"]);
                    objKUOC.TEN_KHANG = dr["TEN_KHANG"].ToString();
                    objKUOC.NV_LOAI_NVON = dr["NV_LOAI_NVON"].ToString();
                    objKUOC.MA_SAN_PHAM = dr["MA_SAN_PHAM"].ToString();
                    objKUOC.MA_KUOC = dr["MA_KUOC"].ToString();
                    objKUOC.SO_GTLQ_KHANG = dr["SO_GTLQ"].ToString();
                    objKUOC.NGAY_MUA_HANG = dr["NGAY_BAN_HANG"].ToString();
                    objKUOC.NGAY_DAO_HAN = dr["NGAY_DAO_HAN"].ToString();
                    objKUOC.DIA_CHI_KHANG = dr["DIA_CHI"].ToString();
                    objKUOC.MA_PHI_MO_SO = dr["MA_BPHI"].ToString();
                    objKUOC.PHI_MO_SO = Convert.ToDecimal(dr["SO_TIEN_PHI_MO_SO"]);
                    objKUOC.TGIAN_VAY = Convert.ToString(dr["TGIAN_VAY"]);
                    objKUOC.TGIAN_VAY_DVT = Convert.ToString(dr["TGIAN_VAY_DVT"]);

                    objKUOC.BIEU_PHI = new BIEU_PHI_DTO();
                    objKUOC.BIEU_PHI.HTHUC_BTHANG = dr["HTHUC_BTHANG"].ToString();
                    objKUOC.BIEU_PHI.ID_BPHI = Convert.ToInt32(dr["ID_BPHI"]);
                    objKUOC.BIEU_PHI.LOAI_BPHI = dr["LOAI_BPHI"].ToString();
                    objKUOC.BIEU_PHI.LOAI_TIEN = dr["MA_LOAI_TIEN"].ToString();
                    objKUOC.BIEU_PHI.MA_BPHI = dr["MA_BPHI"].ToString();
                    objKUOC.BIEU_PHI.NGAY_ADUNG = dr["NGAY_ADUNG"].ToString();
                    if (dr["NGAY_HHAN"] != DBNull.Value)
                        objKUOC.BIEU_PHI.NGAY_HHAN = dr["NGAY_HHAN"].ToString();
                    objKUOC.BIEU_PHI.TCHAT_BPHI = dr["TCHAT_BPHI"].ToString();
                    objKUOC.BIEU_PHI.TEN_BPHI = dr["TEN_BPHI"].ToString();
                    objKUOC.BIEU_PHI.TY_LE_VAT = Convert.ToDecimal(dr["TY_LE_VAT"]);
                    if (objKUOC.BIEU_PHI.ID_BPHI > 0)
                    {
                        GetThongTinBieuPhiCTiet(ref objKUOC);
                        objKUOC.BIEU_PHI.DSACH_BPHI_CT = lstBieuPhi.ToArray();
                    }
                    objKUOC.TTIN_SO_TK = new THONG_TIN_SO_TKIEM_GNGAN_DAI_LY();
                    lstGiaiNganDaiLyKUOC.Add(objKUOC);
                }
                raddgrDSachPhatVay.ItemsSource = lstGiaiNganDaiLyKUOC;
                raddgrDSachPhatVay.Rebind();
                dt = ds.Tables["TTIN_SOTK"];
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void SetTabThongKiemSoat(DataSet ds)
        {
            try
            {
                if (!LObject.IsNullOrEmpty(ds) && ds.Tables.Count > 1)
                {
                    DataTable dt = ds.Tables["TTIN_CHUNG"];
                    if (dt.Rows.Count > 0)
                    {
                        txtNguoiLap.Text = dt.Rows[0]["NGUOI_NHAP"].ToString();
                        teldtNgayNhap.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_NHAP"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                        txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(dt.Rows[0]["TTHAI_BGHI"].ToString());
                        txtNguoiCapNhat.Text = dt.Rows[0]["NGUOI_CNHAT"].ToString();
                        if (!dt.Rows[0]["NGAY_CNHAT"].ToString().IsNullOrEmptyOrSpace())
                            teldtNgayCNhat.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_CNHAT"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    }
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
