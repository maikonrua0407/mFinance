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
using PresentationWPF.CustomControl;
using Telerik.Windows.Controls;
using System.Data;
using Utilities.Common;
using Presentation.Process.Common;
using System.Collections;
using System.Reflection;
using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.TaiSanServiceRef;
using Telerik.Windows.Controls.GridView;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;

namespace PresentationWPF.TaiSan.TaiSan
{
    /// <summary>
    /// Interaction logic for ucKhauHaoCT.xaml
    /// </summary>
    public partial class ucKhauHaoCT : UserControl
    {
        /// <summary>
        /// Khai bao
        /// </summary>
        #region Khai bao
        public static RoutedCommand AddCommand = new RoutedCommand();
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
        List<DataRow> lstPopup = new List<DataRow>();
        public event EventHandler OnSavingCompleted;
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        string TThaiNVu = "";
        public int idCT = 0;
        string maDonViSD = "";
        private bool isTinhToan;
        public bool IsTinhToan
        {
            get { return isTinhToan; }
            set { isTinhToan = value; }
        }
        DIEU_KIEN_TIM_KIEM_DTO dieuKien = new DIEU_KIEN_TIM_KIEM_DTO();
        KHAU_HAO_DTO obj = new KHAU_HAO_DTO();
        List<DANH_SACH_KHAU_HAO_DTO> danhSachDto = new List<DANH_SACH_KHAU_HAO_DTO>();
        List<AutoCompleteEntry> lstSourceNguoiQD = new List<AutoCompleteEntry>();

        public DatabaseConstant.Action Action;
        public DatabaseConstant.Function Function;
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucKhauHaoCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TaiSan;component/TaiSan/ucKhauHaoCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            KhoiTaoGiaTriChoComboBox();
            InitEventHandler();
            ShowControl();
            ClearForm();
            titemThongTinChung.Focus();
            // Lần đầu không cho chọn nguyên nhân thay đổi
        }

        public ucKhauHaoCT(string maThamChieu, DatabaseConstant.Action action)
        {
            if (!maThamChieu.IsNullOrEmptyOrSpace())
                idCT = maThamChieu.Split('.')[1].StringToInt32();
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TaiSan;component/TaiSan/ucKhauHaoCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            KhoiTaoGiaTriChoComboBox();
            InitEventHandler();
            ShowControl();
            ClearForm();
            Action = action;
            titemThongTinChung.Focus();
            // Lần đầu không cho chọn nguyên nhân thay đổi
        }

        void KhoiTaoGiaTriChoComboBox()
        {
            try
            {
                AutoComboBox auto = new AutoComboBox();
                auto.GenComboBox_Nam(ref cmbNam);
                auto = new AutoComboBox();
                auto.GenComboBox_Thang(ref cmbThang);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void InitEventHandler()
        {

        }

        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.TaiSan.TaiSan.ucKhauHaoCT", "RibbonButton");
            foreach (List<string> lst in arr)
            {
                object item = Toolbar.FindName(lst.First());
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
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.D, ModifierKeys.Control);
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(SubmitCommand, keyg);
                        key.Gesture = keyg;
                    }
                    //else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
                    //{
                    //    KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift);
                    //    key = new KeyBinding(CashStmtCommand, keyg);
                    //    key.Gesture = keyg;
                    //}
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift);
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

        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbAdd.IsEnabled;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ResetData();
            SetEnabledAllControl(true);
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbModify.IsEnabled;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Modify();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbDelete.IsEnabled;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeDelete();
        }

        private void CloneCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CloneCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Nhân bản dữ liệu");
        }

        //private void HoldCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        //{
        //    e.CanExecute = true;
        //}
        //private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        //{
        //    BeforeSave(BusinessConstant.TrangThaiNghiepVu.LUU_TAM, BusinessConstant.TrangThaiBanGhi.SU_DUNG);
        //}

        private void SubmitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbSubmit.IsEnabled;
        }
        private void SubmitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeSave(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET, BusinessConstant.TrangThaiBanGhi.SU_DUNG);
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbSubmit.IsEnabled;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeSave(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET, BusinessConstant.TrangThaiBanGhi.SU_DUNG);
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
            e.CanExecute = tlbApprove.IsEnabled;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeApprove();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeCancel();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbRefuse.IsEnabled;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeRefuse();
        }

        private void PreviewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void PreviewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnPreviewChungTu();
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

            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAP_DU_LIEU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetData();
                SetEnabledAllControl(true);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                Modify();
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
            else if (strTinhNang.Equals("PreviewChungTu"))
            {
                OnPreviewChungTu();
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
            // Truongnx
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAP_DU_LIEU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetData();
                SetEnabledAllControl(true);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                Modify();
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
            else if (strTinhNang.Equals("PreviewChungTu"))
            {
                OnPreviewChungTu();
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
                {
                    telControl.Value = dtpControl.SelectedDate;
                    telControl.Focus();
                }
                else
                    throw new System.NullReferenceException(LLanguage.SearchResourceByKey("M.TinDung.KheUoc.ucKheUocCT.KhongTimThayControl") + sbControl.ToString());
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Sự kiện load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Function = DatabaseConstant.Function.TS_KHAU_HAO;

            //Hiển thị Form khi xem dữ liệu
            if (Action == DatabaseConstant.Action.XEM || Action == DatabaseConstant.Action.SUA)
            {
                if (obj.ObjKhauHao.IsNullOrEmpty()) obj.ObjKhauHao = new TS_KHAU_HAO();
                obj.ObjKhauHao.ID = idCT;
                SetDataForm();
                // Refresh buttons
                LLogging.WriteLog("Xem", LLogging.LogType.BUS, DateTime.Now.Subtract(DateTime.Now).TotalMilliseconds.ToString());
            }
            CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_KHAU_HAO);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
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
            listLockId.Add(idCT);

            bool ret = process.UnlockData(DatabaseConstant.Module.QLTS,
                Function,
                DatabaseConstant.Table.TS_KHAU_HAO,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        void ClearForm()
        {

            lblTrangThai.Content = "";
            txtSoChungTu.Text = "";        
            grTaiSan.ItemsSource = null;
            grTaiSan.Rebind();
            isTinhToan = false;

            //Lay lai tai khoan mac dinh

            #region Thông tin kiểm soát
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            teldtNgayCNhat.Value = null;
            txtTrangThai.Text = "";
            txtTrangThai.Text = "";
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            txtNguoiCapNhat.Text = "";
            #endregion

            Action = DatabaseConstant.Action.THEM;
            CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_KHAU_HAO);
        }

        private void ResetData()
        {
            Action = DatabaseConstant.Action.THEM;
            idCT = 0;

            ClearForm();
        }

        private void SetEnabledAllControl(bool bBool)
        {
            txtSoChungTu.IsEnabled = false;
            cmbThang.IsEnabled = bBool;
            cmbNam.IsEnabled = bBool;
            tlbLoadTaiSan.IsEnabled = bBool;
            tlbCalcTaiSan.IsEnabled = bBool;
            grTaiSan.IsEnabled = bBool;
            //txtDienGiai.IsEnabled = bBool;
        }

        void resetThongTinKhauHao()
        {
            txtSoChungTu.Text = string.Empty;
            foreach (var item in cmbThang.Items)
            {
                AutoCompleteEntry entry = (AutoCompleteEntry)item;
                if (entry.DisplayName.Equals(Convert.ToInt32(ClientInformation.NgayLamViecHienTai.Substring(4, 2)).ToString()))
                    cmbThang.SelectedItem = item;
            }
            foreach (var item in cmbNam.Items)
            {
                AutoCompleteEntry entry = (AutoCompleteEntry)item;
                if (entry.DisplayName.Equals(Convert.ToInt32(ClientInformation.NgayLamViecHienTai.Substring(0, 4)).ToString()))
                    cmbNam.SelectedItem = item;
            }
            //txtDienGiai.Text = string.Empty;
        }

        #endregion

        /// <summary>
        /// Xu ly nghiep vu
        /// </summary>
        #region Xu ly nghiep vu

        void GetDataForm(BusinessConstant.TrangThaiBanGhi bghi, BusinessConstant.TrangThaiNghiepVu nghiepvu)
        {
            try
            {
                if (obj.ObjKhauHao.IsNullOrEmpty())
                {
                    obj = new KHAU_HAO_DTO();
                    obj.ObjKhauHao = new TS_KHAU_HAO();
                }
                obj.ObjKhauHao.DVI_SDUNG = maDonViSD;

                // Lấy thông tin
                string thang = ((AutoCompleteEntry)cmbThang.SelectedItem).KeywordStrings.First();
                if (thang.Length == 1) thang = "0" + thang;
                obj.ObjKhauHao.KY_KHAU_HAO = ((AutoCompleteEntry)cmbNam.SelectedItem).KeywordStrings.First() + thang;
                //obj.ObjKhauHao.SO_CHUNG_TU = txtSoChungTu.Text;
                obj.ObjKhauHao.NGAY_CHUNG_TU = (obj.ObjKhauHao.KY_KHAU_HAO+"01").StringToDate("yyyyMMdd").GetLastDateOfMonth().DateToString("yyyyMMdd");
                List<TS_KHAU_HAO_CT> lstChiTiet = new List<TS_KHAU_HAO_CT>();
                foreach (TS_KHAU_HAO_CT row in grTaiSan.Items)
                {
                    row.NGAY_CHUNG_TU = obj.ObjKhauHao.NGAY_CHUNG_TU;
                    lstChiTiet.Add(row);
                }
                obj.LstChiTiet = lstChiTiet.ToArray();

                if (idCT > 0)
                {
                    obj.ObjKhauHao.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    obj.ObjKhauHao.TTHAI_NVU = nghiepvu.layGiaTri();
                    obj.ObjKhauHao.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    obj.ObjKhauHao.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }
                else if (idCT == 0)
                {
                    obj.ObjKhauHao.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    obj.ObjKhauHao.TTHAI_NVU = nghiepvu.layGiaTri();
                    obj.ObjKhauHao.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    obj.ObjKhauHao.NGUOI_NHAP = ClientInformation.TenDangNhap;
                }
                foreach (var ct in obj.LstChiTiet)
                {
                    if (idCT > 0)
                    {
                        ct.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                        ct.TTHAI_NVU = nghiepvu.layGiaTri();
                        ct.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                        ct.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    }
                    else if (idCT == 0)
                    {
                        ct.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                        ct.TTHAI_NVU = nghiepvu.layGiaTri();
                        ct.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                        ct.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    }
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }

        }

        public void SetDataForm()
        {
            try
            {
                // Do something
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                TaiSanProcess process = new TaiSanProcess();
                List<int> lstID = new List<int>();
                bool ref1 = false;
                ref1 = process.KhauHaoTaiSan(DatabaseConstant.Function.TS_KHAU_HAO, DatabaseConstant.Action.XEM, lstID, dieuKien, ref obj, ref danhSachDto, ref listClientResponseDetail);
                if (ref1)
                {
                    isTinhToan = true;
                    maDonViSD = obj.ObjKhauHao.DVI_SDUNG;

                    TThaiNVu = obj.ObjKhauHao.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(obj.ObjKhauHao.TTHAI_NVU);
                    txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri());
                    txtSoChungTu.Text = obj.ObjKhauHao.SO_CHUNG_TU;
                    //txtDienGiai.Text=obj.ObjKhauHao.
                    int thang = obj.ObjKhauHao.KY_KHAU_HAO.Substring(4, 2).StringToInt32();
                    int nam = obj.ObjKhauHao.KY_KHAU_HAO.Substring(0, 4).StringToInt32();
                    foreach (var item in cmbThang.Items)
                    {
                        AutoCompleteEntry entry = (AutoCompleteEntry)item;
                        if (entry.KeywordStrings.First().StringToInt32() == thang)
                            cmbThang.SelectedItem = item;
                    }
                    foreach (var item in cmbNam.Items)
                    {
                        AutoCompleteEntry entry = (AutoCompleteEntry)item;
                        if (entry.KeywordStrings.First().StringToInt32() == nam)
                            cmbNam.SelectedItem = item;
                    }
                    grTaiSan.ItemsSource = obj.LstChiTiet;
                    grTaiSan.Rebind();

                    #region Tab thông tin kiểm soát
                    txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(obj.ObjKhauHao.TTHAI_BGHI);
                    teldtNgayNhap.Value = LDateTime.StringToDate(obj.ObjKhauHao.NGAY_NHAP, "yyyyMMdd");
                    txtNguoiLap.Text = obj.ObjKhauHao.NGUOI_NHAP;
                    if (LDateTime.IsDate(obj.ObjKhauHao.NGAY_CNHAT, "yyyyMMdd") == true)
                        teldtNgayCNhat.Value = LDateTime.StringToDate(obj.ObjKhauHao.NGAY_CNHAT, "yyyyMMdd");
                    else
                        teldtNgayCNhat.Value = null;
                    txtNguoiCapNhat.Text = obj.ObjKhauHao.NGUOI_CNHAT;
                    #endregion

                    if (!Action.Equals(DatabaseConstant.Action.SUA))
                    {
                        CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_KHAU_HAO);
                        SetEnabledAllControl(false);
                    }
                    else
                    {
                        if (TThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                        {
                            tlbModify.IsEnabled = false;
                            SetEnabledAllControl(false);
                        }
                        else
                            Modify();
                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void tlbLoadTaiSan_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            try
            {
                isTinhToan = false;
                List<int> lstID = new List<int>();
                dieuKien = new DIEU_KIEN_TIM_KIEM_DTO();
                danhSachDto = new List<DANH_SACH_KHAU_HAO_DTO>();
                TaiSanProcess process = new TaiSanProcess();
                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                if (obj.ObjKhauHao.IsNullOrEmpty()) obj.ObjKhauHao = new TS_KHAU_HAO();
                obj.ObjKhauHao.MA_DVI_QLY = ClientInformation.MaDonVi;
                obj.ObjKhauHao.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                obj.ObjKhauHao.NGAY_CHUNG_TU = ClientInformation.NgayLamViecHienTai;
                string thang = ((AutoCompleteEntry)cmbThang.SelectedItem).KeywordStrings.First();
                if (thang.Length == 1) thang = "0" + thang;
                obj.ObjKhauHao.KY_KHAU_HAO = ((AutoCompleteEntry)cmbNam.SelectedItem).KeywordStrings.First() + thang;
                process.KhauHaoTaiSan(DatabaseConstant.Function.TS_KHAU_HAO, DatabaseConstant.Action.LOAD, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail);
                grTaiSan.ItemsSource = obj.LstChiTiet;
                grTaiSan.Rebind();
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

        private void tlbCalcTaiSan_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            try
            {
                List<int> lstID = new List<int>();
                dieuKien = new DIEU_KIEN_TIM_KIEM_DTO();
                danhSachDto = new List<DANH_SACH_KHAU_HAO_DTO>();
                TaiSanProcess process = new TaiSanProcess();
                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                if (obj.ObjKhauHao.IsNullOrEmpty()) obj.ObjKhauHao = new TS_KHAU_HAO();
                obj.ObjKhauHao.MA_DVI_QLY = ClientInformation.MaDonVi;
                obj.ObjKhauHao.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                obj.ObjKhauHao.NGAY_CHUNG_TU = ClientInformation.NgayLamViecHienTai;
                string thang = ((AutoCompleteEntry)cmbThang.SelectedItem).KeywordStrings.First();
                if (thang.Length == 1) thang = "0" + thang;
                obj.ObjKhauHao.KY_KHAU_HAO = ((AutoCompleteEntry)cmbNam.SelectedItem).KeywordStrings.First() + thang;
                List<TS_KHAU_HAO_CT> lstChiTiet = new List<TS_KHAU_HAO_CT>();
                foreach (TS_KHAU_HAO_CT row in grTaiSan.Items)
                {
                    lstChiTiet.Add(row);
                }
                obj.LstChiTiet = lstChiTiet.ToArray();
                isTinhToan = process.KhauHaoTaiSan(DatabaseConstant.Function.TS_KHAU_HAO, DatabaseConstant.Action.TINH_TOAN, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail);
                if (isTinhToan == true)
                {
                    grTaiSan.ItemsSource = obj.LstChiTiet;
                    grTaiSan.Rebind();
                    if (obj.LstChiTiet.Count() <= 0)
                    {
                        LMessage.ShowMessage("Chưa có dữ liệu tính toán", LMessage.MessageBoxType.Warning);
                    }
                    else
                    {
                        LMessage.ShowMessage("Tính toán thành công", LMessage.MessageBoxType.Information);
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
        }

        bool Validation()
        {
            bool bReturn = true;
            try
            {
                //if (txtSoChungTu.Text.IsNullOrEmptyOrSpace())
                //{
                //    LMessage.ShowMessage("Chưa nhập số chứng từ", LMessage.MessageBoxType.Warning);
                //    txtSoChungTu.Focus();
                //    return false;
                //}

                if (obj.LstChiTiet.Count() <= 0)
                {
                    LMessage.ShowMessage("Lỗi thực hiện lấy dữ liệu", LMessage.MessageBoxType.Warning);
                    return false;
                }

                if (isTinhToan == false)
                {
                    LMessage.ShowMessage("Chưa thực hiện tính toán", LMessage.MessageBoxType.Warning);
                    tlbCalcTaiSan.Focus();
                    return false;
                }
                //else if (txtDienGiai.Text.IsNullOrEmptyOrSpace())
                //{
                //    LMessage.ShowMessage("Thiếu diễn giải", LMessage.MessageBoxType.Warning);
                //    txtDienGiai.Focus();
                //    return false;
                //}
            }
            catch (System.Exception ex)
            {
                bReturn = false;
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return bReturn;
        }

        void Modify()
        {
            if (TThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
            {
                tlbModify.IsEnabled = false;
                SetEnabledAllControl(false);
            }
            else
            {
                List<int> lstId = new List<int>();
                lstId.Add(idCT);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_KHAU_HAO,
                DatabaseConstant.Table.TS_KHAU_HAO,
                DatabaseConstant.Action.SUA,
                lstId);
                Action = DatabaseConstant.Action.SUA;
                CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_KHAU_HAO);
                SetEnabledAllControl(true);
            }
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
                GetDataForm(bghi, nghiepvu);
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
                List<int> lstID = new List<int>();
                dieuKien = new DIEU_KIEN_TIM_KIEM_DTO();
                danhSachDto = new List<DANH_SACH_KHAU_HAO_DTO>();
                TaiSanProcess process = new TaiSanProcess();
                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                int iret = 0;
                if (idCT == 0)
                {
                    obj.ObjKhauHao.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    obj.ObjKhauHao.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    obj.ObjKhauHao.MA_DVI_QLY = ClientInformation.MaDonVi;
                    obj.ObjKhauHao.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    if (!process.KhauHaoTaiSan(DatabaseConstant.Function.TS_KHAU_HAO, DatabaseConstant.Action.THEM, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail))
                        iret = 1;
                }
                else
                {
                    obj.ObjKhauHao.NGUOI_CNHAT = ClientInformation.NgayLamViecHienTai;
                    obj.ObjKhauHao.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    obj.ObjKhauHao.ID = idCT;
                    if (!process.KhauHaoTaiSan(DatabaseConstant.Function.TS_KHAU_HAO, DatabaseConstant.Action.SUA, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail))
                        iret = 1;
                }
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
                if (iret == 0)
                    LMessage.ShowMessage("M.DungChung.LuuDuLieuThanhCong", LMessage.MessageBoxType.Information);
                else
                    CommonFunction.ThongBaoKetQua(lstResponseDetail);
                // Yêu cầu Unlock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(idCT);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_KHAU_HAO,
                DatabaseConstant.Table.TS_KHAU_HAO,
                DatabaseConstant.Action.SUA,
                lstId);
                Action = DatabaseConstant.Action.XEM;
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
                if (!cbMultiAdd.IsChecked.GetValueOrDefault(false))
                {
                    SetInfomation();
                }
                else
                {
                    ClearForm();
                }
                idCT = obj.ObjKhauHao.ID;
                TThaiNVu = obj.ObjKhauHao.TTHAI_NVU;
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
                if (cbMultiAdd.IsChecked == true)
                    ClearForm();
                else
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_KHAU_HAO);

            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void BeforeDelete()
        {
            Cursor = Cursors.Wait;
            try
            {
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    // Yêu cầu Lock dữ liệu
                    List<int> lstId = new List<int>();
                    lstId.Add(idCT);
                    UtilitiesProcess process = new UtilitiesProcess();
                    bool retLockData = process.LockData(DatabaseConstant.Module.QLTS,
                    DatabaseConstant.Function.TS_KHAU_HAO,
                    DatabaseConstant.Table.TS_KHAU_HAO,
                    DatabaseConstant.Action.XOA,
                    lstId);
                    OnDelete();
                }
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(idCT);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_KHAU_HAO,
                DatabaseConstant.Table.TS_KHAU_HAO,
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
            if (idCT != 0)
            {
                List<int> lstID = new List<int>();
                TaiSanProcess process = new TaiSanProcess();
                obj.ObjKhauHao.ID = idCT;
                if (!process.KhauHaoTaiSan(DatabaseConstant.Function.TS_KHAU_HAO, DatabaseConstant.Action.XOA, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail))
                    iret = 1;
            }
            AfterDelete(lstResponseDetail, iret);
        }
        void AfterDelete(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            if (iret == 0)
                LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
            else
                CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(idCT);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
            DatabaseConstant.Function.TS_KHAU_HAO,
            DatabaseConstant.Table.TS_KHAU_HAO,
            DatabaseConstant.Action.XOA,
            lstId);
            Action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            Cursor = Cursors.Arrow;
            if (iret == 0) CommonFunction.CloseUserControl(this);
        }

        void BeforeApprove()
        {
            try
            {
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    Cursor = Cursors.Wait;
                    // Yêu cầu Lock dữ liệu
                    List<int> lstId = new List<int>();
                    lstId.Add(idCT);
                    UtilitiesProcess process = new UtilitiesProcess();
                    bool retLockData = process.LockData(DatabaseConstant.Module.QLTS,
                    DatabaseConstant.Function.TS_KHAU_HAO,
                    DatabaseConstant.Table.TS_KHAU_HAO,
                    DatabaseConstant.Action.DUYET,
                    lstId);
                    OnApprove();
                }
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(idCT);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_KHAU_HAO,
                DatabaseConstant.Table.TS_KHAU_HAO,
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
            if (idCT != 0)
            {
                List<int> lstID = new List<int>();
                TaiSanProcess process = new TaiSanProcess();
                obj.ObjKhauHao.ID = idCT;
                if (!process.KhauHaoTaiSan(DatabaseConstant.Function.TS_KHAU_HAO, DatabaseConstant.Action.DUYET, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail))
                    iret = 1;
            }
            AfterApprove(lstResponseDetail, iret);
        }
        void AfterApprove(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            if (iret == 0)
                LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);
            else
                CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(idCT);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
            DatabaseConstant.Function.TS_KHAU_HAO,
            DatabaseConstant.Table.TS_KHAU_HAO,
            DatabaseConstant.Action.DUYET,
            lstId);
            Action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            idCT = obj.ObjKhauHao.ID;
            TThaiNVu = obj.ObjKhauHao.TTHAI_NVU;
            lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_KHAU_HAO);
            Cursor = Cursors.Arrow;
        }

        void BeforeRefuse()
        {
            try
            {
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    Cursor = Cursors.Wait;
                    // Yêu cầu Lock dữ liệu
                    List<int> lstId = new List<int>();
                    lstId.Add(idCT);
                    UtilitiesProcess process = new UtilitiesProcess();
                    bool retLockData = process.LockData(DatabaseConstant.Module.QLTS,
                    DatabaseConstant.Function.TS_KHAU_HAO,
                    DatabaseConstant.Table.TS_KHAU_HAO,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    lstId);
                    OnRefuse();
                }
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(idCT);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_KHAU_HAO,
                DatabaseConstant.Table.TS_KHAU_HAO,
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
            if (idCT != 0)
            {
                List<int> lstID = new List<int>();
                TaiSanProcess process = new TaiSanProcess();
                obj.ObjKhauHao.ID = idCT;
                if (!process.KhauHaoTaiSan(DatabaseConstant.Function.TS_KHAU_HAO, DatabaseConstant.Action.TU_CHOI_DUYET, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail))
                    iret = 1;
            }
            AfterRefuse(lstResponseDetail, iret);
        }
        void AfterRefuse(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            if (iret == 0)
                LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);
            else
                CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(idCT);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
            DatabaseConstant.Function.TS_KHAU_HAO,
            DatabaseConstant.Table.TS_KHAU_HAO,
            DatabaseConstant.Action.TU_CHOI_DUYET,
            lstId);
            Action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            idCT = obj.ObjKhauHao.ID;
            TThaiNVu = obj.ObjKhauHao.TTHAI_NVU;
            lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_KHAU_HAO);
            Cursor = Cursors.Arrow;
        }

        void BeforeCancel()
        {
            try
            {
                Cursor = Cursors.Wait;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(idCT);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_KHAU_HAO,
                DatabaseConstant.Table.TS_KHAU_HAO,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                OnCancel();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(idCT);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_KHAU_HAO,
                DatabaseConstant.Table.TS_KHAU_HAO,
                DatabaseConstant.Action.THOAI_DUYET,
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
            if (idCT != 0)
            {
                List<int> lstID = new List<int>();
                TaiSanProcess process = new TaiSanProcess();
                obj.ObjKhauHao.ID = idCT;
                process.KhauHaoTaiSan(DatabaseConstant.Function.TS_KHAU_HAO, DatabaseConstant.Action.THOAI_DUYET, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail);
            }
            AfterCancel(lstResponseDetail, iret);
        }
        void AfterCancel(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(idCT);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
            DatabaseConstant.Function.TS_KHAU_HAO,
            DatabaseConstant.Table.TS_KHAU_HAO,
            DatabaseConstant.Action.THOAI_DUYET,
            lstId);
            Action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            idCT = obj.ObjKhauHao.ID;
            TThaiNVu = obj.ObjKhauHao.TTHAI_NVU;
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_KHAU_HAO);
            Cursor = Cursors.Arrow;
        }

        private void OnPreviewChungTu()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(idCT))
            {
                LMessage.ShowMessage("Không có chứng từ", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                // Lấy thông tin giao dịch theo biến động
                TaiSanProcess process = new TaiSanProcess();
                BIEN_DONG_DTO objBienDongDTO = new BIEN_DONG_DTO();
                KIEM_SOAT objKiemSoat = new KIEM_SOAT();
                objBienDongDTO.Function = DatabaseConstant.Function.TS_KHAU_HAO;
                objBienDongDTO.IdBienDong = idCT;

                bool ret = process.LayThongTinGiaoDich(ref objKiemSoat, objBienDongDTO);

                if (ret)
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                    DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();
                    DatabaseConstant.Function _function = DatabaseConstant.Function.KT_PHIEU_KE_TOAN;

                    GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                    objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.GDKT_IN_GIAO_DICH;
                    objGIAO_DICH_BASE.ChucNang = _function;

                    GDKT_GIAO_DICH objGDKT_GIAO_DICH = new GDKT_GIAO_DICH();
                    objGDKT_GIAO_DICH.MaGiaoDich = objKiemSoat.SO_GIAO_DICH;

                    doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                    doiTuongBaoCao.objGDKT_GIAO_DICH = objGDKT_GIAO_DICH;
                    xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
                }
                else
                {
                }
            }

        }

        private void SetInfomation()
        {
            if (!LObject.IsNullOrEmpty(obj))
            {
                //txtMaGD.Text = obj.ObjKhauHao.SO_GIAO_DICH;
                Action = DatabaseConstant.Action.XEM;
                SetEnabledAllControl(false);
                CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_KHAU_HAO);
            }
            else
                SetDataForm();
        }

        #endregion
    }
}
