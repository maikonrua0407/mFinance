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
using Presentation.Process.Common;
using System.Data;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.TinDungServiceRef;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;
using System.Collections;
using System.Reflection;

namespace PresentationWPF.TinDung.HoanUng
{
    /// <summary>
    /// Interaction logic for ucHoanUngCT.xaml
    /// </summary>
    public partial class ucHoanUngCT : UserControl
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

        private DatabaseConstant.Function function = DatabaseConstant.Function.TDVM_HOAN_UNG;
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
        TDVM_HOAN_UNG objHoanUng = new TDVM_HOAN_UNG();
        List<TDVM_HOAN_UNG_KUOCVM> lstHoanUngKUOC = new List<TDVM_HOAN_UNG_KUOCVM>();
        List<TDVM_HOAN_UNG_SOTK> lstHoanUngSOTK = new List<TDVM_HOAN_UNG_SOTK>();
        bool isLoad = true;
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucHoanUngCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDung;component/HoanUng/ucHoanUngCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            ClearForm();
            cmbCanBoPhatVon.Focus();
            InitEventHanler();
            ShowControl();
        }

        public ucHoanUngCT(KIEM_SOAT objKiemSoat) : this()
        {
            _objKiemSoat = objKiemSoat;
            action = _objKiemSoat.action;
            SetDataForm();
        }

        void InitEventHanler()
        {
            btnSoPhieuTUng.Click += new RoutedEventHandler(btnSoPhieuTUng_Click);
            btnTinhLaiTD.Click += new RoutedEventHandler(btnTinhLaiTD_Click);
            cmbCanBoPhatVon.SelectionChanged += new SelectionChangedEventHandler(cmbCanBoPhatVon_SelectionChanged);
            raddgrDSachPhatVay.DataLoaded += new EventHandler<EventArgs>(raddgrDSachPhatVay_DataLoaded);
            raddgrDSRutTK.DataLoaded += new EventHandler<EventArgs>(raddgrDSRutTK_DataLoaded);
            txtSoPhieuTUng.KeyDown += txtSoPhieuTUng_KeyDown;
            raddgrDSachPhatVay.CellValidating += new EventHandler<GridViewCellValidatingEventArgs>(raddgrDSachPhatVay_CellValidating);
            raddgrDSRutTK.CellValidating += new EventHandler<GridViewCellValidatingEventArgs>(raddgrDSRutTK_CellValidating);
            raddgrDSachPhatVay.CellEditEnded += new EventHandler<GridViewCellEditEndedEventArgs>(raddgrDSachPhatVay_CellEditEnded);
            raddgrDSRutTK.CellEditEnded += new EventHandler<GridViewCellEditEndedEventArgs>(raddgrDSRutTK_CellEditEnded);
        }

        void KhoiTaoComboBoxCanBo()
        {
            AutoComboBox au = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
            lstSourceCanBo.Clear();
            cmbCanBoPhatVon.Items.Clear();
            lstSourceCanBo.Add(new AutoCompleteEntry("", "%", "0"));
            au.GenAutoComboBox(ref lstSourceCanBo, ref cmbCanBoPhatVon, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NHAN_SU.getValue(), lstDieuKien);
        }

        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.TinDung.HoanUng.ucHoanUngCT", "");
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
            txtSoPhieuTUng.Text = "";
            TThaiNV = "";
            teldtNgayGD.Value = ClientInformation.NgayLamViecHienTai.StringToDate(ApplicationConstant.defaultDateTimeFormat);
            maDonViQLy = ClientInformation.MaDonVi;
            maDonViGDich = ClientInformation.MaDonViGiaoDich;
            KhoiTaoComboBoxCanBo();
            telnumSoTienTUng.Value = 0;
            txtDienGiai.Text = "";
            lblLabelTrangThai.Content = "";
            txtTrangThai.Text = "";
            teldtNgayCNhat.Value = null;
            teldtNgayNhap.Value = ClientInformation.NgayLamViecHienTai.StringToDate(ApplicationConstant.defaultDateTimeFormat);
            txtNguoiCapNhat.Text = "";
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            action = DatabaseConstant.Action.THEM;
            CommonFunction.RefreshButton(Toolbar, action, TThaiNV, mnuMain, function);
        }

        void SetEnabledAllControl(bool bBool)
        {
            cmbCanBoPhatVon.IsEnabled = bBool;
            telnumSoTienTUng.IsEnabled = bBool;
            txtDienGiai.IsEnabled = bBool;
            raddgrDSachPhatVay.IsReadOnly = !bBool;
            raddgrDSRutTK.IsReadOnly = !bBool;
            txtSoPhieuTUng.IsEnabled = bBool;
            btnSoPhieuTUng.IsEnabled = bBool;
        }

        void btnSoPhieuTUng_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            try
            {
                AutoCompleteEntry auCanBo = lstSourceCanBo.ElementAt(cmbCanBoPhatVon.SelectedIndex);
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(maDonViGDich);
                lstDieuKien.Add(auCanBo.KeywordStrings.FirstOrDefault());
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_GDICH_TUNG.getValue(), lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Content = popup;
                win.ShowDialog();
                if (lstPopup.Count > 0)
                {
                    foreach (DataRow dr in lstPopup)
                    {
                        isLoad = false;
                        cmbCanBoPhatVon.SelectedIndex = lstSourceCanBo.IndexOf(lstSourceCanBo.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["MA_CAN_BO_TUNG"].ToString())));
                        objHoanUng.MA_CAN_BO_TUNG = dr["MA_CAN_BO_TUNG"].ToString();
                        objHoanUng.ID_GDICH_TUNG = Convert.ToInt32(dr["ID"]);
                        objHoanUng.SO_GDICH_TUNG = dr["MA_GDICH"].ToString();
                    }
                    List<ClientResponseDetail> lstClientResponseDetail = new List<ClientResponseDetail>();
                    int iRet = new TinDungProcess().HoanUngGiaiNgan(DatabaseConstant.Action.LOAD_DATA, ref objHoanUng, ref lstClientResponseDetail);
                    if (iRet > 0)
                    {
                        telnumSoTienTUng.Value = (double)objHoanUng.SO_TIEN_TUNG;
                        NguonVon = objHoanUng.NV_LOAI_NVON;
                        txtSoPhieuTUng.Text = objHoanUng.SO_GDICH_TUNG;
                        lstHoanUngKUOC = objHoanUng.DSACH_KUOC.ToList();
                        lstHoanUngSOTK = objHoanUng.DSACH_SOTK.ToList();
                        TKhoanTUng = objHoanUng.SO_TKHOAN_TUNG;
                        NgayPhatVon = objHoanUng.NGAY_PHAT_VON;
                        raddgrDSachPhatVay.ItemsSource = lstHoanUngKUOC;
                        raddgrDSRutTK.ItemsSource = lstHoanUngSOTK;
                        raddgrDSachPhatVay.Rebind();
                        raddgrDSRutTK.Rebind();
                    }
                    else
                        CommonFunction.ThongBaoKetQua(lstClientResponseDetail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void txtSoPhieuTUng_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnSoPhieuTUng_Click(null, null);
            }
        }

        void btnTinhLaiTD_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void cmbCanBoPhatVon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isLoad)
            {
                objHoanUng = new TDVM_HOAN_UNG();
                lstHoanUngKUOC = new List<TDVM_HOAN_UNG_KUOCVM>();
                lstHoanUngSOTK = new List<TDVM_HOAN_UNG_SOTK>();
                txtSoPhieuTUng.Text = "";
                telnumSoTienTUng.Value = null;
                raddgrDSachPhatVay.ItemsSource = lstHoanUngKUOC;
                raddgrDSRutTK.ItemsSource = lstHoanUngSOTK;
                raddgrDSachPhatVay.Rebind();
                raddgrDSRutTK.Rebind();
            }
            else
                isLoad = true;
        }

        void raddgrDSRutTK_DataLoaded(object sender, EventArgs e)
        {
            if (!ClientInformation.Company.Equals("PHUTHO"))
            {
                telTongTienDSRutTK.Value = (double)(lstHoanUngSOTK.Sum(f => f.SO_TIEN_GOC) + lstHoanUngSOTK.Sum(f => f.SO_TIEN_LAI));
                telnumSoTienTraKH.Value = (double)(lstHoanUngSOTK.Sum(f => f.SO_TIEN_GOC) + lstHoanUngSOTK.Sum(f => f.SO_TIEN_LAI) + lstHoanUngKUOC.Sum(f => f.SO_TIEN_GNGAN));
                telnumSoTienHoanUng.Value = Convert.ToDouble(Math.Max(Convert.ToDecimal(telnumSoTienTUng.Value.GetValueOrDefault()) - Convert.ToDecimal(telnumSoTienTraKH.Value.GetValueOrDefault()) + lstHoanUngKUOC.Sum(f => f.SO_TIEN_LAI), 0));
            }
            else
            {
                telTongTienDSRutTK.Value = (double)(lstHoanUngSOTK.Sum(f => f.SO_TIEN_GOC) + lstHoanUngSOTK.Sum(f => f.SO_TIEN_LAI));
                telnumSoTienTraKH.Value = (double)(lstHoanUngSOTK.Sum(f => f.SO_TIEN_GOC) + lstHoanUngSOTK.Sum(f => f.SO_TIEN_LAI) + lstHoanUngKUOC.Sum(f => f.SO_TIEN_GNGAN) - lstHoanUngKUOC.Sum(f => f.SO_TIEN_TKIEM));
                telnumSoTienHoanUng.Value = Convert.ToDouble(Math.Max(Convert.ToDecimal(telnumSoTienTUng.Value.GetValueOrDefault()) - Convert.ToDecimal(telnumSoTienTraKH.Value.GetValueOrDefault()) + lstHoanUngKUOC.Sum(f => f.SO_TIEN_LAI), 0));
            }
        }

        void raddgrDSachPhatVay_DataLoaded(object sender, EventArgs e)
        {
            if (!ClientInformation.Company.Equals("PHUTHO"))
            {
                telTongTienPhatVay.Value = (double)(lstHoanUngKUOC.Sum(f => f.SO_TIEN_GNGAN));
                telnumSoTienTraKH.Value = (double)(lstHoanUngSOTK.Sum(f => f.SO_TIEN_GOC) + lstHoanUngSOTK.Sum(f => f.SO_TIEN_LAI) + lstHoanUngKUOC.Sum(f => f.SO_TIEN_GNGAN));
                telnumSoTienHoanUng.Value = Convert.ToDouble(Math.Max(Convert.ToDecimal(telnumSoTienTUng.Value.GetValueOrDefault()) - Convert.ToDecimal(telnumSoTienTraKH.Value.GetValueOrDefault()) + lstHoanUngKUOC.Sum(f => f.SO_TIEN_LAI), 0));
            }
            else
            {
                telTongTienPhatVay.Value = (double)(lstHoanUngKUOC.Sum(f => f.SO_TIEN_GNGAN) - lstHoanUngKUOC.Sum(f => f.SO_TIEN_TKIEM));
                telnumSoTienTraKH.Value = (double)(lstHoanUngSOTK.Sum(f => f.SO_TIEN_GOC) + lstHoanUngSOTK.Sum(f => f.SO_TIEN_LAI) + lstHoanUngKUOC.Sum(f => f.SO_TIEN_GNGAN) - lstHoanUngKUOC.Sum(f => f.SO_TIEN_TKIEM));
                telnumSoTienHoanUng.Value = Convert.ToDouble(Math.Max(Convert.ToDecimal(telnumSoTienTUng.Value.GetValueOrDefault()) - Convert.ToDecimal(telnumSoTienTraKH.Value.GetValueOrDefault()) + lstHoanUngKUOC.Sum(f => f.SO_TIEN_LAI), 0));
            }
        }

        void raddgrDSRutTK_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            if (!ClientInformation.Company.Equals("PHUTHO"))
            {
                telTongTienDSRutTK.Value = (double)(lstHoanUngSOTK.Sum(f => f.SO_TIEN_GOC) + lstHoanUngSOTK.Sum(f => f.SO_TIEN_LAI));
                telnumSoTienTraKH.Value = (double)(lstHoanUngSOTK.Sum(f => f.SO_TIEN_GOC) + lstHoanUngSOTK.Sum(f => f.SO_TIEN_LAI) + lstHoanUngKUOC.Sum(f => f.SO_TIEN_GNGAN));
                telnumSoTienHoanUng.Value = Convert.ToDouble(Math.Max(Convert.ToDecimal(telnumSoTienTUng.Value.GetValueOrDefault()) - Convert.ToDecimal(telnumSoTienTraKH.Value.GetValueOrDefault()) + lstHoanUngKUOC.Sum(f => f.SO_TIEN_LAI), 0));
            }
            else
            {
                telTongTienDSRutTK.Value = (double)(lstHoanUngSOTK.Sum(f => f.SO_TIEN_GOC) + lstHoanUngSOTK.Sum(f => f.SO_TIEN_LAI));
                telnumSoTienTraKH.Value = (double)(lstHoanUngSOTK.Sum(f => f.SO_TIEN_GOC) + lstHoanUngSOTK.Sum(f => f.SO_TIEN_LAI) + lstHoanUngKUOC.Sum(f => f.SO_TIEN_GNGAN) - lstHoanUngKUOC.Sum(f => f.SO_TIEN_TKIEM));
                telnumSoTienHoanUng.Value = Convert.ToDouble(Math.Max(Convert.ToDecimal(telnumSoTienTUng.Value.GetValueOrDefault()) - Convert.ToDecimal(telnumSoTienTraKH.Value.GetValueOrDefault()) + lstHoanUngKUOC.Sum(f => f.SO_TIEN_LAI), 0));
            }
        }

        void raddgrDSachPhatVay_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            if (!ClientInformation.Company.Equals("PHUTHO"))
            {
                if (ClientInformation.Company.Equals("BIDV"))
                {
                    TDVM_HOAN_UNG_KUOCVM objKheUoc = e.Cell.ParentRow.Item as TDVM_HOAN_UNG_KUOCVM;
                    if (!LObject.IsNullOrEmpty(e.Cell.Column.UniqueName))
                    {
                        if (e.Cell.Column.UniqueName == "SO_TIEN_GNGAN")
                        {
                            if (Convert.ToDecimal(e.Cell.Value) == 0)
                            {
                                
                            }
                        }
                    }

                    telTongTienPhatVay.Value = (double)(lstHoanUngKUOC.Sum(f => f.SO_TIEN_GNGAN));
                    telnumSoTienTraKH.Value = (double)(lstHoanUngSOTK.Sum(f => f.SO_TIEN_GOC) + lstHoanUngSOTK.Sum(f => f.SO_TIEN_LAI) + lstHoanUngKUOC.Sum(f => f.SO_TIEN_GNGAN));
                    telnumSoTienHoanUng.Value = Convert.ToDouble(Math.Max(Convert.ToDecimal(telnumSoTienTUng.Value.GetValueOrDefault()) - Convert.ToDecimal(telnumSoTienTraKH.Value.GetValueOrDefault()) + lstHoanUngKUOC.Sum(f => f.SO_TIEN_LAI), 0));
                }
                else
                {
                    telTongTienPhatVay.Value = (double)(lstHoanUngKUOC.Sum(f => f.SO_TIEN_GNGAN));
                    telnumSoTienTraKH.Value = (double)(lstHoanUngSOTK.Sum(f => f.SO_TIEN_GOC) + lstHoanUngSOTK.Sum(f => f.SO_TIEN_LAI) + lstHoanUngKUOC.Sum(f => f.SO_TIEN_GNGAN));
                    telnumSoTienHoanUng.Value = Convert.ToDouble(Math.Max(Convert.ToDecimal(telnumSoTienTUng.Value.GetValueOrDefault()) - Convert.ToDecimal(telnumSoTienTraKH.Value.GetValueOrDefault()) + lstHoanUngKUOC.Sum(f => f.SO_TIEN_LAI), 0));
                }
            }
            
            else
            {
                telTongTienPhatVay.Value = (double)(lstHoanUngKUOC.Sum(f => f.SO_TIEN_GNGAN) - lstHoanUngKUOC.Sum(f => f.SO_TIEN_TKIEM));
                telnumSoTienTraKH.Value = (double)(lstHoanUngSOTK.Sum(f => f.SO_TIEN_GOC) + lstHoanUngSOTK.Sum(f => f.SO_TIEN_LAI) + lstHoanUngKUOC.Sum(f => f.SO_TIEN_GNGAN) - lstHoanUngKUOC.Sum(f => f.SO_TIEN_TKIEM));
                telnumSoTienHoanUng.Value = Convert.ToDouble(Math.Max(Convert.ToDecimal(telnumSoTienTUng.Value.GetValueOrDefault()) - Convert.ToDecimal(telnumSoTienTraKH.Value.GetValueOrDefault()) + lstHoanUngKUOC.Sum(f => f.SO_TIEN_LAI), 0));
            }
        }

        void raddgrDSRutTK_CellValidating(object sender, GridViewCellValidatingEventArgs e)
        {
            TDVM_HOAN_UNG_SOTK objSoTK = e.Cell.ParentRow.Item as TDVM_HOAN_UNG_SOTK;
            if (!LObject.IsNullOrEmpty(e.Cell.Column.UniqueName))
            {
                if (e.Cell.Column.UniqueName == "SO_TIEN_GOC")
                {
                    if (Convert.ToDecimal(e.NewValue) != 0 && Convert.ToDecimal(e.NewValue) != objSoTK.SO_TIEN_GOC_TUNG)
                    {
                        e.IsValid = false;
                        e.ErrorMessage = "Value must Equals 0 or " + objSoTK.SO_TIEN_GOC_TUNG.ToString();
                    }
                }
                else if (e.Cell.Column.UniqueName == "SO_TIEN_LAI")
                {
                    if (Convert.ToDecimal(e.NewValue) != 0 && Convert.ToDecimal(e.NewValue) != objSoTK.SO_TIEN_LAI_TUNG)
                    {
                        e.IsValid = false;
                        e.ErrorMessage = "Value must Equals 0 or " + objSoTK.SO_TIEN_LAI_TUNG.ToString();
                    }
                }
            }
        }

        void raddgrDSachPhatVay_CellValidating(object sender, GridViewCellValidatingEventArgs e)
        {
            TDVM_HOAN_UNG_KUOCVM objKheUoc = e.Cell.ParentRow.Item as TDVM_HOAN_UNG_KUOCVM;
            if (!LObject.IsNullOrEmpty(e.Cell.Column.UniqueName))
            {
                if (e.Cell.Column.UniqueName == "SO_TIEN_GNGAN")
                {
                    if (LString.IsNumeric(e.NewValue.ToString()))
                    {
                        if (Convert.ToDecimal(e.NewValue) != 0 && Convert.ToDecimal(e.NewValue) != objKheUoc.SO_TIEN_GNGAN_TUNG)
                        {
                            e.IsValid = false;
                            e.ErrorMessage = "Value must Equals 0 or " + objKheUoc.SO_TIEN_GNGAN_TUNG.ToString();
                        }

                        if (Convert.ToDecimal(e.NewValue) == 0)
                        {
                            objKheUoc.SO_TIEN_TKIEM = 0;
                            objKheUoc.PHI_MO_SO = 0;

                            raddgrDSachPhatVay.CurrentItem = objKheUoc;
                            //raddgrDSachPhatVay.CommitEdit();
                            //raddgrDSachPhatVay.CellEditEnded += raddgrDSachPhatVay_CellEditEnded;
                        }
                    }
                }
            }
        }

        void btnPhiMoSo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TDVM_HOAN_UNG_KUOCVM objTamUngKUOC = raddgrDSachPhatVay.SelectedItem as TDVM_HOAN_UNG_KUOCVM;
                int idBieuPhi = 0;
                lstPopup.Clear();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonVi);
                lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
                lstDieuKien.Add("'TU01','TU02','TU03','HU01','HU02','HU03'");
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
                    objTamUngKUOC.ID_PHI_MO_SO = idBieuPhi;
                    decimal soTienPhi = 0;
                    TinhPhiTraTruoc(objTamUngKUOC.SO_TIEN_TKIEM, idBieuPhi, out soTienPhi);
                    objTamUngKUOC.PHI_MO_SO = soTienPhi;
                    raddgrDSachPhatVay.CurrentItem = objTamUngKUOC;
                    raddgrDSachPhatVay.Rebind();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
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
                AutoCompleteEntry auCanBo = lstSourceCanBo.ElementAt(cmbCanBoPhatVon.SelectedIndex);
                lstHoanUngKUOC = raddgrDSachPhatVay.ItemsSource as List<TDVM_HOAN_UNG_KUOCVM>;
                lstHoanUngSOTK = raddgrDSRutTK.ItemsSource as List<TDVM_HOAN_UNG_SOTK>;
                objHoanUng = new TDVM_HOAN_UNG();
                objHoanUng.DIEN_GIAI = txtDienGiai.Text;
                objHoanUng.MA_GDICH = txtSoPhieu.Text;
                objHoanUng.ID_GDICH = iDGiaDich;
                objHoanUng.LOAI_TIEN = ClientInformation.MaDongNoiTe;
                objHoanUng.LOAI_TTOAN = BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri();
                objHoanUng.MA_CAN_BO_TUNG = auCanBo.KeywordStrings.FirstOrDefault();
                objHoanUng.MA_DVI_GD = ClientInformation.MaDonViGiaoDich;
                objHoanUng.MA_DVI_QLY = ClientInformation.MaDonVi;
                objHoanUng.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                objHoanUng.MA_LOAI_GDICH = DatabaseConstant.LoaiGiaoDich.TU01.layGiaTri();
                objHoanUng.NGAY_DL = teldtNgayGD.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                objHoanUng.NGAY_GD = teldtNgayGD.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                objHoanUng.NGUOI_NHAP = ClientInformation.TenDangNhap;
                if (!LObject.IsNullOrEmpty(lblPhongban.Content))
                    objHoanUng.PBAN_CAN_BO_TUNG = lblPhongban.Content.ToString();
                objHoanUng.SO_TKHOAN_TUNG = TKhoanTUng;
                objHoanUng.NGAY_PHAT_VON = NgayPhatVon;
                objHoanUng.SO_GDICH_TUNG = txtSoPhieuTUng.Text;
                objHoanUng.SO_TIEN_TUNG = (decimal)telnumSoTienTUng.Value.GetValueOrDefault();
                objHoanUng.SO_TIEN_PHAT_VAY = (decimal)telTongTienPhatVay.Value.GetValueOrDefault();
                objHoanUng.SO_TIEN_RUT_LAI = (decimal)telTongTienDSRutTK.Value.GetValueOrDefault();
                objHoanUng.SOCMND_CAN_BO_TUNG = "";
                objHoanUng.TEN_CAN_BO_TUNG = auCanBo.DisplayName;
                objHoanUng.TTHAI_BGHI = sudung.layGiaTri();
                objHoanUng.TTHAI_NVU = nghiepvu.layGiaTri();
                objHoanUng.NV_LOAI_NVON = NguonVon;
                if (iDGiaDich > 0)
                {
                    objHoanUng.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    objHoanUng.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }

                objHoanUng.NGAY_NHAP = LDateTime.DateToString(teldtNgayNhap.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
                objHoanUng.NGUOI_NHAP = txtNguoiLap.Text;
                if (!LObject.IsNullOrEmpty(lstHoanUngKUOC))
                    objHoanUng.DSACH_KUOC = lstHoanUngKUOC.ToArray();
                if (!LObject.IsNullOrEmpty(lstHoanUngSOTK))
                    objHoanUng.DSACH_SOTK = lstHoanUngSOTK.ToArray();
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
                LMessage.ShowMessage("M.DungChung.KhongCoThongTin", LMessage.MessageBoxType.Warning);

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
                LMessage.ShowMessage("M.DungChung.KhongCoThongTin", LMessage.MessageBoxType.Warning);

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
            if (cmbCanBoPhatVon.SelectedIndex < 0)
            {
                CommonFunction.ThongBaoTrong(lblCanBoTamUng.Content.ToString());
                cmbCanBoPhatVon.Focus();
                return false;
            }
            else if (txtSoPhieuTUng.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblSoGDTUng.Content.ToString());
                txtSoPhieuTUng.Focus();
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
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_HOAN_UNG,
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
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_HOAN_UNG,
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
                if (objHoanUng.MA_GDICH.IsNullOrEmptyOrSpace())
                    iret = new TinDungProcess().HoanUngGiaiNgan(DatabaseConstant.Action.THEM, ref objHoanUng, ref lstResponseDetail);
                else
                    iret = new TinDungProcess().HoanUngGiaiNgan(DatabaseConstant.Action.SUA, ref objHoanUng, ref lstResponseDetail);
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
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_HOAN_UNG,
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
                objHoanUng.MA_GDICH = txtSoPhieu.Text;
                objHoanUng.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                objHoanUng.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_HOAN_UNG,
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
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_TAM_UNG,
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
            iret = new TinDungProcess().HoanUngGiaiNgan(DatabaseConstant.Action.XOA, ref objHoanUng, ref lstResponseDetail);
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
                objHoanUng.MA_GDICH = txtSoPhieu.Text;
                objHoanUng.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                objHoanUng.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                objHoanUng.MA_DVI_QLY = ClientInformation.MaDonVi;
                objHoanUng.MA_DVI_GD = ClientInformation.MaDonViGiaoDich;
                objHoanUng.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_HOAN_UNG,
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
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_HOAN_UNG,
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
            iret = new TinDungProcess().HoanUngGiaiNgan(DatabaseConstant.Action.DUYET, ref objHoanUng, ref lstResponseDetail);
            AfterApprove(lstResponseDetail, iret);
        }
        void AfterApprove(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(iDGiaDich);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_HOAN_UNG,
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

                objHoanUng.MA_GDICH = txtSoPhieu.Text;
                objHoanUng.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                objHoanUng.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                Cursor = Cursors.Wait;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_HOAN_UNG,
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
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_HOAN_UNG,
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
            iret = new TinDungProcess().HoanUngGiaiNgan(DatabaseConstant.Action.TU_CHOI_DUYET, ref objHoanUng, ref lstResponseDetail);
            AfterRefuse(lstResponseDetail, iret);
        }
        void AfterRefuse(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(iDGiaDich);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_HOAN_UNG,
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

                objHoanUng.MA_GDICH = txtSoPhieu.Text;
                objHoanUng.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                objHoanUng.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                Cursor = Cursors.Wait;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_HOAN_UNG,
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
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_HOAN_UNG,
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
            iret = new TinDungProcess().HoanUngGiaiNgan(DatabaseConstant.Action.THOAI_DUYET, ref objHoanUng, ref lstResponseDetail);
            AfterCancel(lstResponseDetail, iret);
        }
        void AfterCancel(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(iDGiaDich);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_HOAN_UNG,
            DatabaseConstant.Table.KT_GIAO_DICH,
            DatabaseConstant.Action.THOAI_DUYET,
            lstId);
            SetInfomation();
            Cursor = Cursors.Arrow;
        }

        void SetInfomation()
        {
            TThaiNV = objHoanUng.TTHAI_NVU;
            iDGiaDich = objHoanUng.ID_GDICH;
            txtSoPhieu.Text = objHoanUng.MA_GDICH;
            txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(objHoanUng.TTHAI_BGHI);
            lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(objHoanUng.TTHAI_NVU);
            txtNguoiLap.Text = objHoanUng.NGUOI_NHAP;
            txtNguoiCapNhat.Text = objHoanUng.NGUOI_CNHAT;
            if (!objHoanUng.NGAY_NHAP.IsNullOrEmptyOrSpace())
                teldtNgayNhap.Value = LDateTime.StringToDate(objHoanUng.NGAY_NHAP, ApplicationConstant.defaultDateTimeFormat);
            if (!objHoanUng.NGAY_CNHAT.IsNullOrEmptyOrSpace())
                teldtNgayCNhat.Value = LDateTime.StringToDate(objHoanUng.NGAY_CNHAT, ApplicationConstant.defaultDateTimeFormat);
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
                isLoad = false;
                DataSet ds = new TinDungProcess().LayThongTinGiaoDichHoanUngGiaiNgan(_objKiemSoat.ID.ToString());
                if (!LObject.IsNullOrEmpty(ds) && ds.Tables.Count > 2)
                {
                    DataTable dt = ds.Tables["CHI_TIET"];
                    maDonViQLy = dt.Rows[0]["MA_DVI_QLY"].ToString();
                    maDonViGDich = dt.Rows[0]["MA_DVI"].ToString();
                    TThaiNV = dt.Rows[0]["TTHAI_NVU"].ToString();
                    txtSoPhieu.Text = dt.Rows[0]["MA_GDICH"].ToString();
                    iDGiaDich = Convert.ToInt32(dt.Rows[0]["ID_GDICH"]);
                    teldtNgayGD.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_GDICH"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    KhoiTaoComboBoxCanBo();
                    cmbCanBoPhatVon.SelectedIndex = lstSourceCanBo.IndexOf(lstSourceCanBo.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dt.Rows[0]["MA_CAN_BO_TUNG"].ToString())));
                    soTienTamUng = Convert.ToDouble(dt.Rows[0]["SO_TIEN_TUNG"].ToString());
                    telTongTienDSRutTK.Value = Convert.ToDouble(dt.Rows[0]["SO_TIEN_RUT_LAI"]);
                    telTongTienPhatVay.Value = Convert.ToDouble(dt.Rows[0]["SO_TIEN_PHAT_VAY"]);
                    txtDienGiai.Text = dt.Rows[0]["DIEN_GIAI"].ToString();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNV);
                    txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(dt.Rows[0]["TTHAI_BGHI"].ToString());
                    txtNguoiLap.Text = dt.Rows[0]["NGUOI_NHAP"].ToString();
                    txtNguoiCapNhat.Text = dt.Rows[0]["NGUOI_CNHAT"].ToString();
                    teldtNgayNhap.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_NHAP"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    TKhoanTUng = dt.Rows[0]["SO_TKHOAN_TUNG"].ToString();
                    NgayPhatVon = dt.Rows[0]["NGAY_PHAT_VON"].ToString();
                    txtSoPhieuTUng.Text = dt.Rows[0]["SO_GDICH_TUNG"].ToString();
                    if (dt.Rows[0]["NV_LOAI_NVON"] != DBNull.Value)
                        NguonVon = dt.Rows[0]["NV_LOAI_NVON"].ToString();
                    if (dt.Rows[0]["NGAY_CNHAT"] != DBNull.Value)
                        teldtNgayCNhat.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_CNHAT"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    dt = ds.Tables["KUOCVM"];
                    
                    TDVM_HOAN_UNG_KUOCVM objKUOC = null;
                    if (LObject.IsNullOrEmpty(lstHoanUngKUOC)) lstHoanUngKUOC = new List<TDVM_HOAN_UNG_KUOCVM>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        objKUOC = new TDVM_HOAN_UNG_KUOCVM();
                        objKUOC.DIA_CHI_KHANG = dr["DIA_CHI_KHANG"].ToString();
                        objKUOC.ID_KHE_UOC = Convert.ToInt32(dr["ID_KHE_UOC"]);
                        objKUOC.MA_GDICH = txtSoPhieu.Text;
                        objKUOC.MA_KHANG = dr["MA_KHANG"].ToString();
                        objKUOC.MA_KHE_UOC = dr["MA_KHE_UOC"].ToString();
                        objKUOC.MA_SAN_PHAM = dr["MA_SAN_PHAM"].ToString();
                        objKUOC.NGAY_DAO_HAN = dr["NGAY_DAO_HAN"].ToString();
                        objKUOC.NGAY_GD = dr["NGAY_GD"].ToString();
                        objKUOC.SO_GTLQ_KHANG = dr["SO_GTLQ_KHANG"].ToString();
                        objKUOC.SO_TIEN_GNGAN = Convert.ToDecimal(dr["SO_TIEN_GNGAN"]);
                        objKUOC.SO_TIEN_GNGAN_TUNG = Convert.ToDecimal(dr["SO_TIEN_GNGAN_TUNG"]);
                        objKUOC.SO_TIEN_LAI = Convert.ToDecimal(dr["SO_TIEN_LAI"]);
                        objKUOC.TEN_KHANG = dr["TEN_KHANG"].ToString();
                        objKUOC.TGIAN_VAY = Convert.ToInt32(dr["TGIAN_VAY"]);
                        objKUOC.SO_TIEN_QUY_TT = Convert.ToDecimal(dr["SO_TIEN_QUY_TT"]);
                        objKUOC.NV_LOAI_NVON = Convert.ToString(dr["NV_LOAI_NVON"]);
                        objKUOC.PLOAI_NO = dr["PLOAI_NO"].ToString();
                        objKUOC.SO_SO_TKIEM = dr["SO_SO_TKIEM"].ToString();
                        objKUOC.MA_SAN_PHAM_TKIEM = dr["MA_SAN_PHAM_TKIEM"].ToString();
                        objKUOC.SO_TIEN_TKIEM = Convert.ToDecimal(dr["SO_TIEN_TKIEM"]);
                        objKUOC.PHI_MO_SO = Convert.ToDecimal(dr["PHI_MO_SO"]);
                        objKUOC.MA_PHI_MO_SO = dr["MA_PHI_MO_SO"].ToString();
                        objKUOC.ID_PHI_MO_SO = Convert.ToInt32(dr["ID_PHI_MO_SO"]);
                        objKUOC.PHI_BAO_HIEM = Convert.ToDecimal(dr["PHI_BAO_HIEM"]);
                        objKUOC.MA_PHI_BAO_HIEM = dr["MA_PHI_BH"].ToString();
                        objKUOC.ID_PHI_BAO_HIEM = Convert.ToInt32(dr["ID_PHI_BH"]);
                        objKUOC.PHI_GIAI_NGAN = Convert.ToDecimal(dr["PHI_GIAI_NGAN"]);
                        objKUOC.MA_PHI_GIAI_NGAN = dr["MA_PHI_GIAI_NGAN"].ToString();
                        objKUOC.ID_PHI_GIAI_NGAN = Convert.ToInt32(dr["ID_PHI_GIAI_NGAN"]);
                        lstHoanUngKUOC.Add(objKUOC);
                    }
                    dt = ds.Tables["SOTKIEM"];
                    TDVM_HOAN_UNG_SOTK objSOTK = null;
                    if (LObject.IsNullOrEmpty(lstHoanUngSOTK)) lstHoanUngSOTK = new List<TDVM_HOAN_UNG_SOTK>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        objSOTK = new TDVM_HOAN_UNG_SOTK();
                        objSOTK.DIA_CHI_KHANG = dr["DIA_CHI_KHANG"].ToString();
                        objSOTK.GHI_CHU = dr["GHI_CHU"].ToString();
                        objSOTK.ID_SO_SO_TG = Convert.ToInt32(dr["ID_SO_SO_TG"]);
                        objSOTK.MA_GDICH = txtSoPhieu.Text;
                        objSOTK.MA_KHANG = dr["MA_KHANG"].ToString();
                        objSOTK.LAI_DU_CHI = Convert.ToDecimal(dr["LAI_DU_CHI"]);
                        objSOTK.LOAI_SO_TG = dr["LOAI_SO_TG"].ToString();
                        objSOTK.NGAY_GD = dr["NGAY_GD"].ToString();
                        objSOTK.SO_GTLQ_KHANG = dr["SO_GTLQ_KHANG"].ToString();
                        objSOTK.SO_DU_GOC = Convert.ToDecimal(dr["SO_DU_GOC"]);
                        objSOTK.TEN_KHANG = dr["TEN_KHANG"].ToString();
                        objSOTK.SO_SO_TG = dr["SO_SO_TG"].ToString();
                        objSOTK.SO_TIEN_GOC = Convert.ToDecimal(dr["SO_TIEN_GOC"]);
                        objSOTK.SO_TIEN_LAI = Convert.ToDecimal(dr["SO_TIEN_LAI"]);
                        objSOTK.SO_TIEN_GOC_TUNG = Convert.ToDecimal(dr["SO_TIEN_GOC_TUNG"]);
                        objSOTK.SO_TIEN_LAI_TUNG = Convert.ToDecimal(dr["SO_TIEN_LAI_TUNG"]);
                        objSOTK.MA_SAN_PHAM = dr["MA_SAN_PHAM"].ToString();
                        objSOTK.NV_LOAI_NVON = Convert.ToString(dr["NV_LOAI_NVON"]);
                        objSOTK.TIEN_LAI_TINH_DUOC = Convert.ToDecimal(dr["TIEN_LAI_TINH_DUOC"]);
                        
                        lstHoanUngSOTK.Add(objSOTK);
                    }
                    raddgrDSachPhatVay.ItemsSource = lstHoanUngKUOC;
                    raddgrDSRutTK.ItemsSource = lstHoanUngSOTK;
                    raddgrDSachPhatVay.Rebind();
                    raddgrDSRutTK.Rebind();
                    telnumSoTienTUng.Value = soTienTamUng;
                }
                if (action.Equals(DatabaseConstant.Action.XEM))
                    SetEnabledAllControl(false);
                else
                    SetEnabledAllControl(true);
                isLoad = true;
                CommonFunction.RefreshButton(Toolbar, action, TThaiNV, mnuMain, function);
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void GetThongTinBieuPhiCTiet(ref BIEU_PHI_DTO objBieuPhi)
        {
            DataSet ds = new PhiProcess().GetPhiByID(objBieuPhi.ID_BPHI);
            DataTable dt = ds.Tables[0];
            List<BIEU_PHI_CTIET_DTO> lstBieuPhi = new List<BIEU_PHI_CTIET_DTO>();
            objBieuPhi.HTHUC_BTHANG = dt.Rows[0]["HTHUC_BTHANG"].ToString();
            objBieuPhi.LOAI_BPHI = dt.Rows[0]["LOAI_BPHI"].ToString();
            objBieuPhi.LOAI_TIEN = dt.Rows[0]["MA_LOAI_TIEN"].ToString();
            objBieuPhi.MA_BPHI = dt.Rows[0]["MA_BPHI"].ToString();
            objBieuPhi.NGAY_ADUNG = dt.Rows[0]["NGAY_ADUNG"].ToString();
            objBieuPhi.NGAY_HHAN = dt.Rows[0]["NGAY_HHAN"].ToString();
            objBieuPhi.TCHAT_BPHI = dt.Rows[0]["TCHAT_BPHI"].ToString();
            objBieuPhi.TEN_BPHI = dt.Rows[0]["TEN_BPHI"].ToString();
            objBieuPhi.TY_LE_VAT = Convert.ToDecimal(dt.Rows[0]["TY_LE_VAT"]);

            dt = ds.Tables[1];
            foreach (DataRow dtr in dt.Rows)
            {
                BIEU_PHI_CTIET_DTO objBieuPhiCT = new BIEU_PHI_CTIET_DTO();
                objBieuPhiCT.ID_BPHI = objBieuPhi.ID_BPHI;
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
            objBieuPhi.DSACH_BPHI_CT = lstBieuPhi.ToArray();
        }

        private void TinhPhiTraTruoc(decimal soTienTinhPhi, int idBieuPhi, out decimal soTienPhi)
        {
            decimal soDu = 0;
            soTienPhi = 0;
            decimal tyLe = 0;
            decimal soTien = 0;
            decimal soTienTThieu = 0;
            decimal soTienTDa = 0;
            BIEU_PHI_DTO objBieuPhi = new BIEU_PHI_DTO();
            objBieuPhi.ID_BPHI = idBieuPhi;
            GetThongTinBieuPhiCTiet(ref objBieuPhi);
            List<BIEU_PHI_CTIET_DTO> lstBieuPhi = objBieuPhi.DSACH_BPHI_CT.ToList();
            soTienTThieu = lstBieuPhi.FirstOrDefault().SO_TIEN_TTHIEU;
            soTienTDa = lstBieuPhi.FirstOrDefault().SO_TIEN_TDA;

            if (objBieuPhi.TCHAT_BPHI.Equals(BusinessConstant.TCHAT_BPHI.DTH.layGiaTri()))
            {
                if (objBieuPhi.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.TY_LE.layGiaTri()))
                {
                    tyLe = lstBieuPhi.FirstOrDefault().TY_LE_PHI;
                }
                else if (objBieuPhi.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.SO_TIEN.layGiaTri()))
                {
                    soTien = lstBieuPhi.FirstOrDefault().SO_TIEN_PHI;
                }
                else if (objBieuPhi.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.STIEN_TLE.layGiaTri()))
                {
                    tyLe = lstBieuPhi.FirstOrDefault().TY_LE_PHI;
                    soTien = lstBieuPhi.FirstOrDefault().SO_TIEN_PHI;
                }
            }
            else
            {

            }
            if (objBieuPhi.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.TY_LE.layGiaTri()))
            {
                soTienPhi = soDu * (tyLe / 100);
            }
            else if (objBieuPhi.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.SO_TIEN.layGiaTri()))
            {
                soTienPhi = soTien;
            }
            else if (objBieuPhi.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.STIEN_TLE.layGiaTri()))
            {

            }
            if (soTienPhi < soTienTThieu)
                soTienPhi = soTienTThieu;
            if (soTienPhi > soTienTDa)
                soTienPhi = soTienTDa;

        }
        #endregion
    }
}
