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
using PresentationWPF.TaiSan.Control;
using System.Collections.ObjectModel;
using Presentation.Process.TruyVanServiceRef;
using PresentationWPF.BaoCao.DungChung;

namespace PresentationWPF.TaiSan.TaiSan
{
    /// <summary>
    /// Interaction logic for ucBanGiaoCT.xaml
    /// </summary>
    public partial class ucBanGiaoCT : UserControl
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
        List<TS_BAN_GIAO_CT_GIAO_NHAN> lstPopupNguoiBG = new List<TS_BAN_GIAO_CT_GIAO_NHAN>();
        public event EventHandler OnSavingCompleted;
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        List<AutoCompleteEntry> lstSourceDonViSD = new List<AutoCompleteEntry>();

        public void LayDuLieuPopupNguoiBG(List<TS_BAN_GIAO_CT_GIAO_NHAN> lst)
        {
            lstPopupNguoiBG = lst;
        }
        string TThaiNVu = "";
        public int idCT = 0;
        List<int> lstID = new List<int>();
        DIEU_KIEN_TIM_KIEM_DTO dieuKien = new DIEU_KIEN_TIM_KIEM_DTO();
        BAN_GIAO_DTO obj = new BAN_GIAO_DTO();
        List<DANH_SACH_BAN_GIAO_DTO> danhSachDto = new List<DANH_SACH_BAN_GIAO_DTO>();

        public DatabaseConstant.Action Action;
        public DatabaseConstant.Function Function;
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucBanGiaoCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TaiSan;component/TaiSan/ucBanGiaoCT.xaml", ref Toolbar, ref mnuMain);
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
            cmbHinhThucBG.Focus();
            // Lần đầu không cho chọn nguyên nhân thay đổi
        }

        void KhoiTaoGiaTriChoComboBox()
        {
            try
            {
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();
                //Load combobox đơn bị sử dụng
                lstDieuKien.Clear();
                lstDieuKien.Add(ClientInformation.MaDonVi);
                auto.GenAutoComboBox(ref lstSourceDonViSD, ref cmbDonViSD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DVI_SDUNG.getValue(), lstDieuKien);
                cmbDonViSD.SelectedIndex = 0;

                List<DatabaseConstant.LOAI_DMUC_TSAN> lstDanhMucLoai = new List<DatabaseConstant.LOAI_DMUC_TSAN>();
                lstDanhMucLoai.Add(DatabaseConstant.LOAI_DMUC_TSAN.HT_BAN_GIAO);
                List<DMUC_TSAN_DTO> lstDanhMucDto = new List<DMUC_TSAN_DTO>();
                TaiSanProcess process = new TaiSanProcess();
                if (process.LayDanhMucTaiSanTheoLoai(ref lstDanhMucDto, lstDanhMucLoai))
                {
                    foreach (DMUC_TSAN_DTO item in lstDanhMucDto)
                    {
                        if (item.MaLoai == DatabaseConstant.LOAI_DMUC_TSAN.HT_BAN_GIAO)
                        {
                            foreach (TS_DM_DMUC_GTRI itemDanhMuc in item.LstObj)
                            {
                                cmbHinhThucBG.Items.Add(new AutoCompleteEntry(itemDanhMuc.TEN_DMUC, itemDanhMuc.MA_DMUC, itemDanhMuc.MA_DMUC_LOAI));
                            }
                            cmbHinhThucBG.SelectedIndex = 0; continue;
                        }
                    }
                }
                else
                    LMessage.ShowMessage(LLanguage.SearchResourceByKey(ApplicationConstant.NghiepVuResponseMessage.M_ResponseMessage_DungChung_LoiKhongXacDinh.layGiaTri()), LMessage.MessageBoxType.Error);

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void InitEventHandler()
        {
            cmbDonViSD.SelectionChanged += cmbDonViSD_SelectionChanged;
        }

        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.TaiSan.TaiSan.ucBanGiaoCT", "RibbonButton");
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
            else if (strTinhNang.Equals("PreviewBienBan"))
            {
                OnPreviewBienBan();
            }
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
            else if (strTinhNang.Equals("PreviewBienBan"))
            {
                OnPreviewBienBan();
            }
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
            ucDoiTuongSD.SPhongBan = ((AutoCompleteEntry)cmbDonViSD.SelectedItem).KeywordStrings[1];

            raddtNgaySD.Value = ClientInformation.NgayLamViecHienTai.StringToDate("yyyyMMdd");
            Function = DatabaseConstant.Function.TS_BAN_GIAO;

            //Hiển thị Form khi xem dữ liệu
            if (Action == DatabaseConstant.Action.XEM || Action == DatabaseConstant.Action.SUA)
            {
                SetDataForm();
                // Refresh buttons
                LLogging.WriteLog("Xem", LLogging.LogType.BUS, DateTime.Now.Subtract(DateTime.Now).TotalMilliseconds.ToString());
            }
            CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_BAN_GIAO);
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

        private void tlbAddNSD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popupNguoiGiaoNhan popup = new popupNguoiGiaoNhan();
                popup.DuLieuTraVe = new popupNguoiGiaoNhan.LayDuLieu(LayDuLieuPopupNguoiBG);
                Window win = new Window();
                win.Width = 600;
                win.Height = 300;
                win.Content = popup;
                win.Title = LLanguage.SearchResourceByKey("U.TaiSan.DungChung.ThongTinDaiDien");
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopupNguoiBG != null && lstPopupNguoiBG.Count > 0)
                {
                    List<TS_BAN_GIAO_CT_GIAO_NHAN> curentSource = (List<TS_BAN_GIAO_CT_GIAO_NHAN>)grNSD.ItemsSource;
                    if (curentSource.IsNullOrEmpty()) curentSource = new List<TS_BAN_GIAO_CT_GIAO_NHAN>();
                    foreach (TS_BAN_GIAO_CT_GIAO_NHAN bg in lstPopupNguoiBG)
                    {
                        if (!curentSource.Select(h => h.MA_HSO).Contains(bg.MA_HSO) && !curentSource.Select(h => h.DAI_DIEN).Contains(bg.DAI_DIEN))
                            curentSource.Add(bg);
                        else
                            LMessage.ShowMessage("Thông tin người giao nhận không hợp lệ hoặc đã được sử dụng.", LMessage.MessageBoxType.Warning);
                    }
                    grNSD.ItemsSource = curentSource;
                    grNSD.Rebind();
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void tlbAddTaiSan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                var process = new PopupProcess();
                lstPopup.Clear();
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_TAI_SAN.getValue(), lstDieuKien);

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, true);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataTable currentSource = (DataTable)grTaiSan.ItemsSource;
                    if (currentSource.IsNullOrEmpty())
                    {
                        currentSource = new DataTable();
                        currentSource.Columns.Add("ID", typeof(string));
                        currentSource.Columns.Add("MA_TAI_SAN", typeof(string));
                        currentSource.Columns.Add("TEN_TAI_SAN", typeof(string));
                        currentSource.Columns.Add("NGUON_GOC", typeof(string));
                        currentSource.Columns.Add("TEN_NGUON_GOC", typeof(string));
                        currentSource.Columns.Add("DOI_TUONG", typeof(string));
                        currentSource.Columns.Add("TONG_NGUYEN_GIA", typeof(string));
                        currentSource.Columns.Add("MA_TINH_TRANG", typeof(string));
                    }
                    foreach (DataRow dr in lstPopup)
                    {
                        if (!currentSource.AsEnumerable().Select(t => t.Field<string>("MA_TAI_SAN")).Contains(dr["MA_TAI_SAN"].ToString()))
                        {
                            currentSource.NewRow();
                            currentSource.Rows.Add(dr["ID"].ToString(), dr["MA_TAI_SAN"].ToString(), dr["TEN_TAI_SAN"].ToString(), dr["MA_NGUON_GOC"].ToString(), dr["NGUON_GOC"].ToString(), dr["DOI_TUONG"].ToString(), dr["TONG_NGUYEN_GIA"].ToString(), dr["MA_TINH_TRANG"].ToString());
                        }
                    }
                    grTaiSan.ItemsSource = currentSource;
                    grTaiSan.Rebind();
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void tlbDeleteNSD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grNSD.SelectedItems.Count > 0)
                {
                    List<TS_BAN_GIAO_CT_GIAO_NHAN> curentSource = (List<TS_BAN_GIAO_CT_GIAO_NHAN>)grNSD.ItemsSource;
                    foreach (TS_BAN_GIAO_CT_GIAO_NHAN bg in grNSD.SelectedItems)
                    {
                        curentSource.Remove(bg);
                    }
                    grNSD.ItemsSource = curentSource;
                    grNSD.Rebind();
                }
                else
                {
                    LMessage.ShowMessage("Chưa chọn người đại diện để xóa", LMessage.MessageBoxType.Warning);
                    return;
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void tlbDeleteTaiSan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grTaiSan.SelectedItems.Count > 0)
                {
                    DataTable currentSource = new DataTable();
                    currentSource = new DataTable();
                    currentSource.Columns.Add("ID", typeof(string));
                    currentSource.Columns.Add("MA_TAI_SAN", typeof(string));
                    currentSource.Columns.Add("TEN_TAI_SAN", typeof(string));
                    currentSource.Columns.Add("NGUON_GOC", typeof(string));
                    currentSource.Columns.Add("TEN_NGUON_GOC", typeof(string));
                    currentSource.Columns.Add("DOI_TUONG", typeof(string));
                    currentSource.Columns.Add("TONG_NGUYEN_GIA", typeof(string));
                    currentSource.Columns.Add("MA_TINH_TRANG", typeof(string));
                    foreach (DataRow dr in grTaiSan.Items)
                    {
                        if (!grTaiSan.SelectedItems.Contains(dr))
                        {
                            DataRow row = (DataRow)dr;
                            currentSource.ImportRow(row);
                        }
                    }
                    grTaiSan.ItemsSource = currentSource;
                    grNSD.Rebind();
                }
                else
                {
                    LMessage.ShowMessage("Chưa chọn tài sản để xóa", LMessage.MessageBoxType.Warning);
                    return;
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
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
                DatabaseConstant.Table.TS_BAN_GIAO,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        void ClearForm()
        {
            lblTrangThai.Content = "";
            txtDiaDiemBG.Text = "";
            cmbDonViSD.SelectedIndex = 0;
            cmbHinhThucBG.SelectedIndex = 0;
            txtSoBienBan.Text = "";
            raddtNgaySD.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            grNSD.ItemsSource = null;
            grNSD.Rebind();
            grTaiSan.ItemsSource = null;
            grTaiSan.Rebind();

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
            CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_BAN_GIAO);
            RefreshButton();
        }

        private void ResetData()
        {
            Action = DatabaseConstant.Action.THEM;
            idCT = 0;

            ClearForm();
        }

        private void SetEnabledAllControl(bool bBool)
        {
            cmbDonViSD.IsEnabled = bBool;
            cmbHinhThucBG.IsEnabled = bBool;
            raddtNgaySD.IsEnabled = bBool;
            dtpNgaySD.IsEnabled = bBool;
            txtDiaDiemBG.IsEnabled = bBool;

            tlbAddNSD.IsEnabled = bBool;
            tlbDeleteNSD.IsEnabled = bBool;
            grNSD.IsEnabled = bBool;
            tlbAddTaiSan.IsEnabled = bBool;
            tlbDeleteTaiSan.IsEnabled = bBool;
            grTaiSan.IsEnabled = bBool;
        }

        private void RefreshButton()
        {
            if (Action.Equals(DatabaseConstant.Action.XEM) && TThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
            {
                tlbAddNSD.IsEnabled = false;
                tlbDeleteNSD.IsEnabled = false;
                grNSD.IsEnabled = false;
                tlbAddTaiSan.IsEnabled = false;
                tlbDeleteTaiSan.IsEnabled = false;
                grTaiSan.IsEnabled = false;
            }
            if (Action.Equals(DatabaseConstant.Action.SUA))
            {
                tlbAddNSD.IsEnabled = true;
                tlbDeleteNSD.IsEnabled = true;
                grNSD.IsEnabled = true;
                tlbAddTaiSan.IsEnabled = true;
                tlbDeleteTaiSan.IsEnabled = true;
                grTaiSan.IsEnabled = true;
            }
        }

        private void cmbDonViSD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ucDoiTuongSD.SPhongBan = ((AutoCompleteEntry)cmbDonViSD.SelectedItem).KeywordStrings[1];
            if (grTaiSan.Items.Count > 0)
            {
                if (obj.ObjBanGiao.DVI_SDUNG.IsNullOrEmptyOrSpace() || !obj.ObjBanGiao.DVI_SDUNG.Equals(((AutoCompleteEntry)cmbDonViSD.SelectedItem).KeywordStrings[1]))
                {
                    DataTable currentSource = (DataTable)grTaiSan.ItemsSource;
                    foreach (DataRow row in currentSource.Rows)
                        row["DOI_TUONG"] = string.Empty;
                    grTaiSan.ItemsSource = currentSource;
                    grTaiSan.Rebind();
                }
            }
        }

        private void ucDoiTuongSD_EditCellEnd(object sender, EventArgs e)
        {
            DataRow r = ucDoiTuongSD.cellEdit.ParentRow.Item as DataRow;
            DataTable currentSource = (DataTable)grTaiSan.ItemsSource;
            foreach (DataRow row in currentSource.Rows)
            {
                if (row["MA_TAI_SAN"].ToString().Equals(r["MA_TAI_SAN"].ToString()))
                    row["DOI_TUONG"] = ucDoiTuongSD.GiaTri;
            }
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
                if (obj.ObjBanGiao.IsNullOrEmpty())
                    obj.ObjBanGiao = new TS_BAN_GIAO();
                List<TS_BAN_GIAO_CT> lstBanGiaoCT = new List<TS_BAN_GIAO_CT>();
                List<TS_BAN_GIAO_CT_GIAO_NHAN> lstGiaoNhan = new List<TS_BAN_GIAO_CT_GIAO_NHAN>();
                obj.ObjBanGiao.SO_BIEN_BAN = txtSoBienBan.Text;
                obj.ObjBanGiao.HTHUC_BGIAO = ((AutoCompleteEntry)cmbHinhThucBG.SelectedItem).KeywordStrings.First();
                obj.ObjBanGiao.DIA_DIEM_GIAO_NHAN = txtDiaDiemBG.Text;
                obj.ObjBanGiao.DVI_SDUNG = ((AutoCompleteEntry)cmbDonViSD.SelectedItem).KeywordStrings[0];
                obj.ObjBanGiao.NGAY_BIEN_BAN = ((DateTime)raddtNgaySD.Value).DateToString("yyyyMMdd");
                if (idCT > 0)
                {
                    obj.ObjBanGiao.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    obj.ObjBanGiao.TTHAI_NVU = nghiepvu.layGiaTri();
                    obj.ObjBanGiao.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    obj.ObjBanGiao.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }
                else if (idCT == 0)
                {
                    obj.ObjBanGiao.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    obj.ObjBanGiao.TTHAI_NVU = nghiepvu.layGiaTri();
                    obj.ObjBanGiao.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    obj.ObjBanGiao.NGUOI_NHAP = ClientInformation.TenDangNhap;
                }
                lstGiaoNhan = new List<TS_BAN_GIAO_CT_GIAO_NHAN>();
                foreach (TS_BAN_GIAO_CT_GIAO_NHAN row in grNSD.Items)
                {
                    lstGiaoNhan.Add(row);
                }
                lstBanGiaoCT = new List<TS_BAN_GIAO_CT>();
                foreach (DataRow row in grTaiSan.Items)
                {
                    TS_BAN_GIAO_CT objChiTiet = new TS_BAN_GIAO_CT();
                    objChiTiet.ID_TAI_SAN = Convert.ToInt32(row["ID"]);
                    objChiTiet.DOI_TUONG = row["DOI_TUONG"].ToString();
                    objChiTiet.NGUYEN_GIA = Convert.ToDecimal(row["TONG_NGUYEN_GIA"]);
                    objChiTiet.HINH_THUC = row["MA_TINH_TRANG"].ToString();
                    lstBanGiaoCT.Add(objChiTiet);
                }
                obj.LstChiTiet = lstBanGiaoCT.ToArray();
                obj.LstGiaoNhan = lstGiaoNhan.ToArray();
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
                lstID = new List<int>();
                dieuKien = new DIEU_KIEN_TIM_KIEM_DTO();
                obj = new BAN_GIAO_DTO();
                danhSachDto = new List<DANH_SACH_BAN_GIAO_DTO>();
                bool ref1 = false;
                obj.ObjBanGiao = new TS_BAN_GIAO();
                obj.ObjBanGiao.ID = idCT;
                ref1 = process.BanGiaoTaiSan(DatabaseConstant.Function.TS_BAN_GIAO, DatabaseConstant.Action.XEM, lstID, dieuKien, ref obj, ref danhSachDto, ref listClientResponseDetail);
                if (ref1)
                {
                    foreach (var item in cmbHinhThucBG.Items)
                    {
                        AutoCompleteEntry entry = (AutoCompleteEntry)item;
                        if (entry.KeywordStrings.First().Equals(obj.ObjBanGiao.HTHUC_BGIAO))
                            cmbHinhThucBG.SelectedItem = item;
                    }
                    foreach (var item in cmbDonViSD.Items)
                    {
                        AutoCompleteEntry entry = (AutoCompleteEntry)item;
                        if (entry.KeywordStrings[1].Equals(obj.ObjBanGiao.DVI_SDUNG))
                            cmbDonViSD.SelectedItem = item;
                    }

                    ucDoiTuongSD.SPhongBan = ((AutoCompleteEntry)cmbDonViSD.SelectedItem).KeywordStrings[1];

                    txtSoBienBan.Text = obj.ObjBanGiao.SO_BIEN_BAN;
                    txtDiaDiemBG.Text = obj.ObjBanGiao.DIA_DIEM_GIAO_NHAN;
                    raddtNgaySD.Value = obj.ObjBanGiao.NGAY_BIEN_BAN.StringToDate("yyyyMMdd");
                    grNSD.ItemsSource = obj.LstGiaoNhan.ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID", typeof(string));
                    dt.Columns.Add("MA_TAI_SAN", typeof(string));
                    dt.Columns.Add("TEN_TAI_SAN", typeof(string));
                    dt.Columns.Add("NGUON_GOC", typeof(string));
                    dt.Columns.Add("TEN_NGUON_GOC", typeof(string));
                    dt.Columns.Add("DOI_TUONG", typeof(string));
                    dt.Columns.Add("TONG_NGUYEN_GIA", typeof(string));
                    dt.Columns.Add("MA_TINH_TRANG", typeof(string));
                    foreach (TS_BAN_GIAO_CT item in obj.LstChiTiet)
                    {
                        DataRow r = dt.NewRow();
                        TS_TAI_SAN taiSan = obj.LstTaiSan.FirstOrDefault(e => e.ID == item.ID_TAI_SAN);
                        r[0] = item.ID_TAI_SAN.ToString();
                        if (!taiSan.IsNullOrEmpty())
                        {
                            r[1] = taiSan.MA_TAI_SAN;
                            r[2] = taiSan.TEN_TAI_SAN;
                            r[3] = taiSan.NGUON_TAI_SAN;
                            if (taiSan.NGUON_TAI_SAN.Equals("01")) r[4] = "Mua sắm";
                            else if (taiSan.NGUON_TAI_SAN.Equals("02")) r[4] = "Nhận viện trợ";
                            else if (taiSan.NGUON_TAI_SAN.Equals("03")) r[4] = "Biếu tặng";
                            else if (taiSan.NGUON_TAI_SAN.Equals("04")) r[4] = "Nguồn gốc khác;";
                        }
                        r[5] = item.DOI_TUONG;
                        r[6] = item.NGUYEN_GIA;
                        r[7] = item.HINH_THUC;
                        dt.Rows.Add(r);
                    }
                    grTaiSan.ItemsSource = dt;
                    //((GridViewComboBoxColumn)this.grTaiSan.Columns[6]).ItemsSource = DoiTuongItems;
                    TThaiNVu = obj.ObjBanGiao.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);

                    #region Tab thông tin kiểm soát
                    txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(obj.ObjBanGiao.TTHAI_BGHI);
                    teldtNgayNhap.Value = LDateTime.StringToDate(obj.ObjBanGiao.NGAY_NHAP, "yyyyMMdd");
                    txtNguoiLap.Text = obj.ObjBanGiao.NGUOI_NHAP;
                    if (LDateTime.IsDate(obj.ObjBanGiao.NGAY_CNHAT, "yyyyMMdd") == true)
                        teldtNgayCNhat.Value = LDateTime.StringToDate(obj.ObjBanGiao.NGAY_CNHAT, "yyyyMMdd");
                    else
                        teldtNgayCNhat.Value = null;
                    txtNguoiCapNhat.Text = obj.ObjBanGiao.NGUOI_CNHAT;
                    #endregion

                    if (!Action.Equals(DatabaseConstant.Action.SUA))
                    {
                        SetEnabledAllControl(false);

                        if (!Action.Equals(DatabaseConstant.Action.SUA))
                        {
                            CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_BAN_GIAO);
                            RefreshButton();
                        }
                        else
                        {
                            if (TThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                            {
                                tlbModify.IsEnabled = false;
                                SetEnabledAllControl(true);
                                Modify();
                            }
                        }
                    }
 
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void SetDataTabThongTinChung(DataSet ds)
        {
            try
            {

            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void SetDataTabThongTinKiemSoat(DataSet ds)
        {
            try
            {

            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }

        }

        bool Validation()
        {
            bool bReturn = true;
            try
            {
                if (raddtNgaySD.Value.IsNullOrEmpty())
                {
                    LMessage.ShowMessage("Thiếu ngày bàn giao", LMessage.MessageBoxType.Warning);
                    raddtNgaySD.Focus();
                    return false;
                }
                else if (txtDiaDiemBG.Text.IsNullOrEmptyOrSpace())
                {
                    LMessage.ShowMessage("Thiếu địa điểm bàn giao", LMessage.MessageBoxType.Warning);
                    txtDiaDiemBG.Focus();
                    return false;
                }
                else if (grNSD.Items.Count < 2)
                {
                    LMessage.ShowMessage("Thiếu thông tin người giao nhận", LMessage.MessageBoxType.Warning);
                    return false;
                }
                else if (grTaiSan.Items.Count == 0)
                {
                    LMessage.ShowMessage("Thiếu thông tin tài sản", LMessage.MessageBoxType.Warning);
                    return false;
                }
                else if (((AutoCompleteEntry)cmbHinhThucBG.SelectedItem).KeywordStrings.First().Equals(BusinessConstant.HINH_THUC_BAN_GIAO_TS.CHINH_THUC.layGiaTri()))
                {
                    DataTable currentSource = (DataTable)grTaiSan.ItemsSource;
                    foreach (DataRow row in currentSource.Rows)
                    {
                        if (row["MA_TINH_TRANG"].ToString().Equals(BusinessConstant.HINH_THUC_NHAP_TS.DO_DANG.layGiaTri()))
                        {
                            LMessage.ShowMessage("Tài sản " + row["MA_TAI_SAN"] + " đang nhập dở dang, không thể bàn giao chính thức.", LMessage.MessageBoxType.Warning);
                            return false;
                        }
                        //else if (row["DOI_TUONG"].ToString().IsNullOrEmptyOrSpace())
                        //{
                        //    LMessage.ShowMessage("Tài sản " + row["MA_TAI_SAN"] + " chưa chọn đối tượng sử dụng.", LMessage.MessageBoxType.Warning);
                        //    return false;
                        //}
                    }
                }
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
                SetEnabledAllControl(true);
            }
            else
            {
                List<int> lstId = new List<int>();
                lstId.Add(idCT);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_BAN_GIAO,
                DatabaseConstant.Table.TS_BAN_GIAO,
                DatabaseConstant.Action.SUA,
                lstId);
                Action = DatabaseConstant.Action.SUA;
                CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_BAN_GIAO);
                RefreshButton();
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
                lstID = new List<int>();
                dieuKien = new DIEU_KIEN_TIM_KIEM_DTO();
                danhSachDto = new List<DANH_SACH_BAN_GIAO_DTO>();

                TaiSanProcess process = new TaiSanProcess();
                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                int iret = 0;
                if (idCT == 0)
                {
                    obj.ObjBanGiao.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    obj.ObjBanGiao.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    obj.ObjBanGiao.MA_DVI_QLY = ClientInformation.MaDonVi;
                    obj.ObjBanGiao.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    if (!process.BanGiaoTaiSan(DatabaseConstant.Function.TS_BAN_GIAO, DatabaseConstant.Action.THEM, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail))
                        iret = 1;
                }
                else
                {
                    obj.ObjBanGiao.NGUOI_CNHAT = ClientInformation.NgayLamViecHienTai;
                    obj.ObjBanGiao.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    obj.ObjBanGiao.ID = idCT;
                    if (!process.BanGiaoTaiSan(DatabaseConstant.Function.TS_BAN_GIAO, DatabaseConstant.Action.SUA, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail))
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
                if (!obj.ObjBanGiao.IsNullOrEmpty())
                {
                    idCT = obj.ObjBanGiao.ID;
                    txtSoBienBan.Text = obj.ObjBanGiao.SO_BIEN_BAN;
                }
                // Yêu cầu Unlock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(idCT);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_BAN_GIAO,
                DatabaseConstant.Table.TS_BAN_GIAO,
                DatabaseConstant.Action.SUA,
                lstId);
                Action = DatabaseConstant.Action.XEM;
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
                idCT = obj.ObjBanGiao.ID;
                TThaiNVu = obj.ObjBanGiao.TTHAI_NVU;
                if (cbMultiAdd.IsChecked == true)
                    ClearForm();
                else
                {
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_BAN_GIAO);
                    SetEnabledAllControl(false);
                }

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
                    DatabaseConstant.Function.TS_BAN_GIAO,
                    DatabaseConstant.Table.TS_BAN_GIAO,
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
                DatabaseConstant.Function.TS_BAN_GIAO,
                DatabaseConstant.Table.TS_BAN_GIAO,
                DatabaseConstant.Action.XOA,
                lstId);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally { Cursor = Cursors.Arrow; }
        }
        void OnDelete()
        {
            lstID = new List<int>();
            dieuKien = new DIEU_KIEN_TIM_KIEM_DTO();
            danhSachDto = new List<DANH_SACH_BAN_GIAO_DTO>();

            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (idCT != 0)
            {
                TaiSanProcess process = new TaiSanProcess();
                obj.ObjBanGiao.ID = idCT;
                if (!process.BanGiaoTaiSan(DatabaseConstant.Function.TS_BAN_GIAO, DatabaseConstant.Action.XOA, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail))
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
            DatabaseConstant.Function.TS_BAN_GIAO,
            DatabaseConstant.Table.TS_BAN_GIAO,
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
                    DatabaseConstant.Function.TS_BAN_GIAO,
                    DatabaseConstant.Table.TS_BAN_GIAO,
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
                DatabaseConstant.Function.TS_BAN_GIAO,
                DatabaseConstant.Table.TS_BAN_GIAO,
                DatabaseConstant.Action.DUYET,
                lstId);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally { Cursor = Cursors.Arrow; }
        }
        void OnApprove()
        {
            lstID = new List<int>();
            dieuKien = new DIEU_KIEN_TIM_KIEM_DTO();
            danhSachDto = new List<DANH_SACH_BAN_GIAO_DTO>();

            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (idCT != 0)
            {
                TaiSanProcess process = new TaiSanProcess();
                obj.ObjBanGiao.ID = idCT;
                if (!process.BanGiaoTaiSan(DatabaseConstant.Function.TS_BAN_GIAO, DatabaseConstant.Action.DUYET, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail))
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
            DatabaseConstant.Function.TS_BAN_GIAO,
            DatabaseConstant.Table.TS_BAN_GIAO,
            DatabaseConstant.Action.DUYET,
            lstId);
            Action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            idCT = obj.ObjBanGiao.ID;
            TThaiNVu = obj.ObjBanGiao.TTHAI_NVU;
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_BAN_GIAO);
            SetEnabledAllControl(false);
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
                    DatabaseConstant.Function.TS_BAN_GIAO,
                    DatabaseConstant.Table.TS_BAN_GIAO,
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
                DatabaseConstant.Function.TS_BAN_GIAO,
                DatabaseConstant.Table.TS_BAN_GIAO,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally { Cursor = Cursors.Arrow; }
        }
        void OnRefuse()
        {
            lstID = new List<int>();
            dieuKien = new DIEU_KIEN_TIM_KIEM_DTO();
            danhSachDto = new List<DANH_SACH_BAN_GIAO_DTO>();

            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (idCT != 0)
            {
                TaiSanProcess process = new TaiSanProcess();
                obj.ObjBanGiao.ID = idCT;
                if (!process.BanGiaoTaiSan(DatabaseConstant.Function.TS_BAN_GIAO, DatabaseConstant.Action.TU_CHOI_DUYET, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail))
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
            DatabaseConstant.Function.TS_BAN_GIAO,
            DatabaseConstant.Table.TS_BAN_GIAO,
            DatabaseConstant.Action.TU_CHOI_DUYET,
            lstId);
            Action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            idCT = obj.ObjBanGiao.ID;
            TThaiNVu = obj.ObjBanGiao.TTHAI_NVU;
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_BAN_GIAO);
            SetEnabledAllControl(false);
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
                DatabaseConstant.Function.TS_BAN_GIAO,
                DatabaseConstant.Table.TS_BAN_GIAO,
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
                DatabaseConstant.Function.TS_BAN_GIAO,
                DatabaseConstant.Table.TS_BAN_GIAO,
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
            lstID = new List<int>();
            dieuKien = new DIEU_KIEN_TIM_KIEM_DTO();
            danhSachDto = new List<DANH_SACH_BAN_GIAO_DTO>();

            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (idCT != 0)
            {
                TaiSanProcess process = new TaiSanProcess();
                obj.ObjBanGiao.ID = idCT;
                process.BanGiaoTaiSan(DatabaseConstant.Function.TS_BAN_GIAO, DatabaseConstant.Action.THOAI_DUYET, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail);
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
            DatabaseConstant.Function.TS_BAN_GIAO,
            DatabaseConstant.Table.TS_BAN_GIAO,
            DatabaseConstant.Action.THOAI_DUYET,
            lstId);
            Action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            Cursor = Cursors.Arrow;
            SetInfomation();
        }

        private void OnPreviewChungTu()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(idCT))
            {
                LMessage.ShowMessage("M.TinDung.KheUoc.ucKheUocCT.KhongCoThongTinKheUoc", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {

            }

        }

        private void OnPreviewBienBan()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(txtSoBienBan.Text))
            {
                LMessage.ShowMessage("Không có thông tin dữ liệu cần hiển thị", LMessage.MessageBoxType.Warning);

            }
            else
            {
                if (ClientInformation.Company.Equals("M7MFI") || ClientInformation.Company.Equals("BINHKHANH"))
                {

                }
                else if (ClientInformation.Company.Equals("BANTAYVANG"))
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();

                    List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                    lstThamSo.Add(new ThamSoBaoCao("@SoBienBan", txtSoBienBan.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.QLTS_BIEN_BAN_BAN_GIAO_TSCD);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
                else
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();

                    List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                    lstThamSo.Add(new ThamSoBaoCao("@SoBienBan", txtSoBienBan.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.QLTS_BIEN_BAN_BAN_GIAO_TSCD);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void AfterOperation()
        {

        }

        private void SetInfomation()
        {
            //if (!LObject.IsNullOrEmpty(objTSDto))
            if (!obj.ObjBanGiao.IsNullOrEmpty())
            {
                Action = DatabaseConstant.Action.XEM;
                SetEnabledAllControl(false);
                CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_BAN_GIAO);
                RefreshButton();
            }
            else
                SetDataForm();
        }

        #endregion
    }
}
