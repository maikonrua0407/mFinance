using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
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
using Presentation.Process.PopupServiceRef;
using Presentation.Process.Common;
using Presentation.Process;
using Presentation.Process.TinDungServiceRef;
using Telerik.Windows.Controls;
using System.Reflection;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;
using Telerik.Windows.Controls.GridView;
using System.Collections;

namespace PresentationWPF.TinDungTT.GiaiNgan
{
    /// <summary>
    /// Interaction logic for ucGiaiNganCT.xaml
    /// </summary>
    public partial class ucGiaiNganCT : UserControl
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
        List<DataRow> lstPopup = new List<DataRow>();
        DatabaseConstant.Action action;
        public event EventHandler OnSavingCompleted;
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        List<int> lstPopupKU = new List<int>();
        public void LayDuLieuTuPopup(List<int> lst)
        {
            lstPopupKU = lst;
        }
        string TThaiNV = "";
        int iDGiaDich = 0;
        int iDCanBo = 0;
        public TDVM_GIAI_NGAN TDVMGIAINGAN = null;
        List<DANH_SACH_KHE_UOC> lstKUOCGN = null;
        private KIEM_SOAT _objKiemSoat = null;
        string sMaGiaoDich = "";
        
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucGiaiNganCT(KIEM_SOAT objKiemSoat) : this ()
        {
            _objKiemSoat = objKiemSoat;
            action = objKiemSoat.action;
            iDGiaDich = _objKiemSoat.ID;
            sMaGiaoDich = _objKiemSoat.SO_GIAO_DICH;
            LoadDataForm();
        }
        public ucGiaiNganCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDungTT;component/GiaiNgan/ucGiaiNganCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            ShowControl();
            ClearForm();
        }

        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.TinDungTT.GiaiNgan.ucGiaiNganCT", "raddgrTUngCT");
            foreach (List<string> lst in arr)
            {
                object item = raddgrTUngCT.Columns[lst.First()];
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
            arr = hethong.SetVisibleControl("PresentationWPF.TinDungTT.GiaiNgan.ucGiaiNganCT", "RibbonButton");
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
                        key = new KeyBinding(SubmitCommand, keyg);
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
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
            }
            else if (strTinhNang.Equals("PreviewChungTu"))
            {
                OnPreview();
            }
            else if (strTinhNang.Equals("PreviewBaoHiem"))
            {
                OnPreviewBH();
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
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
            }
            else if (strTinhNang.Equals("tlbPreviewChungTu"))
            {
                OnPreview();
            }
            else if (strTinhNang.Equals("tlbPreviewBaoHiem"))
            {
                OnPreviewBH();
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

        private void tlbDetailAdd_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            try
            {
                string sidKheUoc = "";
                foreach (DANH_SACH_KHE_UOC dr in lstKUOCGN)
                {
                    sidKheUoc += "," + dr.ID_KHE_UOC.ToString();
                }
                if (sidKheUoc.Length > 0)
                    sidKheUoc = sidKheUoc.Substring(1);
                else
                    sidKheUoc = "0";
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(sidKheUoc);
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add("KUOCGN");
                lstDieuKien.Add(LDateTime.DateToString(teldtNgayGiaoDich.Value.Value, ApplicationConstant.defaultDateTimeFormat));
                lstDieuKien.Add("%");
                lstPopupKU = new List<int>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_KHEUOC", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopupKheUocViMo popup = new ucPopupKheUocViMo(true, simplePopupResponse, true);
                popup.LayGiaTriListID = new ucPopupKheUocViMo.LayListID(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Content = popup;
                win.ShowDialog();
                if (lstPopupKU.Count > 0)
                {
                    string sID = "";
                    foreach (int drv in lstPopupKU)
                    {
                        sID += "," + drv.ToString();
                    }
                    sID = "(" + sID.Substring(1) + ")";
                    DataTable dt = new TinDungProcess().GetPOPUPKUOCVM(sID).Tables["KUOC_GIAINGAN"];
                    List<DANH_SACH_KHE_UOC> lstDSach = new List<DANH_SACH_KHE_UOC>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        DANH_SACH_KHE_UOC objDanhSachKUOC = new DANH_SACH_KHE_UOC();
                        objDanhSachKUOC.CO_BH = "true";
                        foreach (DataColumn dtl in dr.Table.Columns)
                        {
                            PropertyInfo property = objDanhSachKUOC.GetType().GetProperty(dtl.ColumnName);
                            if (property != null)
                            {
                                property.SetValue(objDanhSachKUOC, dr[dtl.ColumnName], null);
                            }
                        }
                        lstDSach.Add(objDanhSachKUOC);
                    }
                    
                    TDVMGIAINGAN.DSACH_KHE_UOC = lstDSach.ToArray();
                    int iret = new TinDungProcess().TinhLaiPhaiThu(ref TDVMGIAINGAN);
                    if (iret > 0)
                    {
                        foreach (DANH_SACH_KHE_UOC objDSach in TDVMGIAINGAN.DSACH_KHE_UOC)
                        {
                            lstKUOCGN.Add(objDSach);
                        }
                    }
                    LoadDataGridKheUoc();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Cursor = Cursors.Arrow;
        }

        private void tlbDetailDelete_Click(object sender, RoutedEventArgs e)
        {
            if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            lstKUOCGN = raddgrTUngCT.ItemsSource as List<DANH_SACH_KHE_UOC>;
            foreach (DANH_SACH_KHE_UOC objDSKUoc in raddgrTUngCT.SelectedItems)
            {
                lstKUOCGN.Remove(objDSKUoc);
            }
            LoadDataGridKheUoc();
        }

        private void btnCanBoNhanTUng_Click(object sender, RoutedEventArgs e)
        {
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.MaDonVi);
            lstPopup = new List<DataRow>();
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation("POPUP_DS_CAN_BO_TUNG", lstDieuKien);
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
                txtCanBoNhanTUng.Tag = lstPopup[0][2].ToString();
                txtCanBoNhanTUng.Text = lstPopup[0][2].ToString();
                lblTenCanBo.Content = lstPopup[0][3].ToString();
                txtBoPhan.Text = lstPopup[0][4].ToString();
            }
        }

        void ClearForm()
        {
            TThaiNV = "";
            TDVMGIAINGAN = new TDVM_GIAI_NGAN();
            lstKUOCGN = new List<DANH_SACH_KHE_UOC>();
            iDGiaDich = 0;
            iDCanBo = 0;
            txtSoPhieu.Text = "";
            teldtNgayGiaoDich.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            //teldtNgayCapPhatVon.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            txtDienGiai.Text = "";
            lblTrangThai.Content = TThaiNV;
            txtCanBoNhanTUng.Text = "";
            txtSoCMND.Text = "";
            txtBoPhan.Text = "";
            lblTenCanBo.Content = LLanguage.SearchResourceByKey("U.TinDungTT.GiaiNgan.ucGiaiNganCT.TenCanBo");
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            teldtNgayCNhat.Value = null;
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            txtNguoiCapNhat.Text = "";
            raddgrTUngCT.ItemsSource = lstKUOCGN;
            action = DatabaseConstant.Action.THEM;
            CommonFunction.RefreshButton(Toolbar, action, TThaiNV);
        }

        private void MaPhi_EditCellEnd(object sender, EventArgs e)
        {
            Telerik.Windows.Controls.GridView.GridViewCell cellEdit = MaPhi.cellEdit;
            DANH_SACH_KHE_UOC drv = (DANH_SACH_KHE_UOC)cellEdit.ParentRow.Item;
            int indexofRows = lstKUOCGN.IndexOf(drv);
            string GiaTri = MaPhi.GiaTri;
            lstKUOCGN[indexofRows].MA_PHI = GiaTri;
            TDVMGIAINGAN.DSACH_KHE_UOC = new Presentation.Process.TinDungServiceRef.DANH_SACH_KHE_UOC[1];
            TDVMGIAINGAN.DSACH_KHE_UOC[0] = new Presentation.Process.TinDungServiceRef.DANH_SACH_KHE_UOC();
            TDVMGIAINGAN.DSACH_KHE_UOC[0].MA_PHI = GiaTri;
            TDVMGIAINGAN.DSACH_KHE_UOC[0].SO_TIEN_PHAT_VON = drv.SO_TIEN_PHAT_VON;
            decimal PhiGiaiNgan = 0;
            new TinDungProcess().GetTienPhi(TDVMGIAINGAN, ref PhiGiaiNgan);
            lstKUOCGN[indexofRows].SO_TIEN_PHI = PhiGiaiNgan;
        }

        private void TaiKhoanPhi_EditCellEnd(object sender, EventArgs e)
        {
            Telerik.Windows.Controls.GridView.GridViewCell cellEdit = MaPhi.cellEdit;
            DANH_SACH_KHE_UOC drv = (DANH_SACH_KHE_UOC)cellEdit.ParentRow.Item;
            int indexofRows = lstKUOCGN.IndexOf(drv);
            string GiaTri = MaPhi.GiaTri;
            lstKUOCGN[indexofRows].TAI_KHOAN_PHI = GiaTri;
        }

        //private void teldtNgayCapPhatVon_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        //{
        //    if (teldtNgayCapPhatVon.Value.Value.CompareTo(teldtNgayGiaoDich.Value.Value) > 0)
        //        teldtNgayCapPhatVon.Value = teldtNgayGiaoDich.Value;
        //    raddgrTUngCT.ItemsSource = new List<DANH_SACH_KHE_UOC>();
        //}

        private void chkGiaiNgan_Checked(object sender, RoutedEventArgs e)
        {
            lstKUOCGN = raddgrTUngCT.ItemsSource as List<DANH_SACH_KHE_UOC>;
            lstKUOCGN.ForEach(q => { q.CO_BH = "true"; q.SO_TIEN_BH = q.SO_TIEN_BH_DP; });
            LoadDataGridKheUoc();
        }

        private void chkGiaiNgan_Click(object sender, RoutedEventArgs e)
        {
            string check = chkGiaiNgan.IsChecked.GetValueOrDefault().ToString();
            lstKUOCGN = raddgrTUngCT.ItemsSource as List<DANH_SACH_KHE_UOC>;
            if (Convert.ToBoolean(check))
                lstKUOCGN.ForEach(q => { q.CO_BH = check; q.SO_TIEN_BH = q.SO_TIEN_BH_DP; });
            else
                lstKUOCGN.ForEach(q => { q.CO_BH = check; q.SO_TIEN_BH = 0; });
            LoadDataGridKheUoc();
        }
        private void ckhBaoHiem_Checked(object sender, RoutedEventArgs e)
        {
            var ckh = sender as CheckBox;
            GridViewRow grrow = ckh.ParentOfType<GridViewRow>();
            DANH_SACH_KHE_UOC objKUOC = grrow.Item as DANH_SACH_KHE_UOC;
            objKUOC.CO_BH = "true";
            objKUOC.SO_TIEN_BH = objKUOC.SO_TIEN_BH_DP;
            raddgrTUngCT.CurrentItem = objKUOC;
        }


        private void chkBaoHiem_Click(object sender, RoutedEventArgs e)
        {
            var ckh = sender as CheckBox;
            GridViewRow grrow = ckh.ParentOfType<GridViewRow>();
            DANH_SACH_KHE_UOC objKUOC = grrow.Item as DANH_SACH_KHE_UOC;
            objKUOC.CO_BH = ckh.IsChecked.GetValueOrDefault().ToString();
            if (ckh.IsChecked.GetValueOrDefault())
                objKUOC.SO_TIEN_BH = objKUOC.SO_TIEN_BH_DP;
            else
                objKUOC.SO_TIEN_BH = 0;
            raddgrTUngCT.CurrentItem = objKUOC;   
        }

        private void ckhBaoHiem_Unchecked(object sender, RoutedEventArgs e)
        {
            var ckh = sender as CheckBox;
            GridViewRow grrow = ckh.ParentOfType<GridViewRow>();
            DANH_SACH_KHE_UOC objKUOC = grrow.Item as DANH_SACH_KHE_UOC;
            objKUOC.CO_BH = "false";
            objKUOC.SO_TIEN_BH = 0;
            raddgrTUngCT.CurrentItem = objKUOC;
            chkGiaiNgan.IsChecked = false;
        }

        private void chkGiaiNgan_Unchecked(object sender, RoutedEventArgs e)
        {
            lstKUOCGN = raddgrTUngCT.ItemsSource as List<DANH_SACH_KHE_UOC>;
            lstKUOCGN.ForEach(q => { q.CO_BH = "false"; q.SO_TIEN_BH = 0; });
            LoadDataGridKheUoc();
        }


        private void raddgrTUngCT_Loaded(object sender, RoutedEventArgs e)
        {
            lstKUOCGN = raddgrTUngCT.ItemsSource as List<DANH_SACH_KHE_UOC>;
            if (lstKUOCGN.Count(q => q.CO_BH == "false") > 0)
                chkGiaiNgan.IsChecked = false;
            else
                chkGiaiNgan.IsChecked = true;
        }

        #endregion

        /// <summary>
        /// Xu ly nghiep vu
        /// </summary>
        #region Xu ly nghiep vu
        void LoadDataGridKheUoc()
        {
            raddgrTUngCT.ItemsSource = null;
            txtTongTien.Value = (double)lstKUOCGN.Sum(e => e.SO_TIEN_PHAT_VON);
            raddgrTUngCT.ItemsSource = lstKUOCGN;
        }
        void GetDataForm(BusinessConstant.TrangThaiNghiepVu nghiepvu, BusinessConstant.TrangThaiBanGhi banghi)
        {
            TDVMGIAINGAN.DIA_CHI = "";
            TDVMGIAINGAN.DIEN_GIAI = txtDienGiai.Text;
            TDVMGIAINGAN.MA_GIAO_DICH = txtSoPhieu.Text;
            TDVMGIAINGAN.LOAI_TIEN = ClientInformation.MaDongNoiTe;
            TDVMGIAINGAN.MA_BO_PHAN = !txtBoPhan.Tag.IsNullOrEmpty() ? txtBoPhan.Tag.ToString() : "";
            TDVMGIAINGAN.MA_CAN_BO = txtCanBoNhanTUng.Text;
            TDVMGIAINGAN.MA_DVI = ClientInformation.MaDonViGiaoDich;
            TDVMGIAINGAN.NGAY_LAP = ClientInformation.NgayLamViecHienTai;
            TDVMGIAINGAN.NGUOI_LAP = ClientInformation.TenDangNhap;
            TDVMGIAINGAN.SO_CMND = txtSoCMND.Text;
            TDVMGIAINGAN.TEN_BO_PHAN = txtBoPhan.Text;
            TDVMGIAINGAN.TEN_CAN_BO = lblTenCanBo.Content.ToString();
            TDVMGIAINGAN.TONG_TIEN_GIAI_NGAN = !txtTongTien.Value.IsNullOrEmpty() ? (decimal)txtTongTien.Value : 0;
            TDVMGIAINGAN.TRANG_THAI_BAN_GHI = banghi.layGiaTri();
            TDVMGIAINGAN.TRANG_THAI_NGHIEP_VU = nghiepvu.layGiaTri();
            TDVMGIAINGAN.NGUOI_LAP = ClientInformation.TenDangNhap;
            TDVMGIAINGAN.NGAY_LAP = ClientInformation.NgayLamViecHienTai;
            lstKUOCGN = (List<DANH_SACH_KHE_UOC>)raddgrTUngCT.ItemsSource;
            TDVMGIAINGAN.DSACH_KHE_UOC = lstKUOCGN.ToArray();
            TDVMGIAINGAN.NGAY_PHAT_VON = ClientInformation.NgayLamViecHienTai;
            if (iDGiaDich > 0)
            {
                TDVMGIAINGAN.NGAY_PHAT_VON = LDateTime.DateToString(teldtNgayGiaoDich.Value.Value, ApplicationConstant.defaultDateTimeFormat);
                TDVMGIAINGAN.ID_GIAO_DICH = iDGiaDich;
                TDVMGIAINGAN.NGUOI_LAP = txtNguoiLap.Text;
                TDVMGIAINGAN.NGAY_LAP = LDateTime.DateToString(teldtNgayNhap.Value.Value,ApplicationConstant.defaultDateTimeFormat);
                TDVMGIAINGAN.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                TDVMGIAINGAN.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
            }
        }
        void BeforeSave(BusinessConstant.TrangThaiNghiepVu nghiepvu, BusinessConstant.TrangThaiBanGhi banghi)
        {
            if (nghiepvu != BusinessConstant.TrangThaiNghiepVu.LUU_TAM)
            {
                if (!Vadidation())
                    return;
            }
            GetDataForm(nghiepvu, banghi);
            OnSave();
        }
        void OnSave()
        {
            List<ClientResponseDetail> lstResponse = new List<ClientResponseDetail>();
            int iret = 0;
            if (iDGiaDich==0)
                iret = new TinDungProcess().ThemMoiGiaoDichGiaiNgan(ref TDVMGIAINGAN, ref lstResponse);
            else
                iret = new TinDungProcess().SuaGiaoDichGiaiNgan(ref TDVMGIAINGAN, ref lstResponse);
            AfterSave(lstResponse, iret);
        }
        void AfterSave(List<ClientResponseDetail> lstResponse, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponse);
            if (iret > 0)
            {
                action = DatabaseConstant.Action.XEM;
                List<int> lstId = new List<int>();
                lstId.Add(TDVMGIAINGAN.ID_GIAO_DICH);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_GIAI_NGAN,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                lstId);
                LoadDataForm();
                CommonFunction.RefreshButton(Toolbar, action, TThaiNV);
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
                if ((bool)cbMultiAdd.IsChecked)
                    ClearForm();
            }
        }
        bool Vadidation()
        {
            if (teldtNgayGiaoDich.Value.IsNullOrEmpty())
            {
                CommonFunction.ThongBaoTrong(lblNgayGiaoDich.Content.ToString());
                teldtNgayGiaoDich.Focus();
                return false;
            }
            //else if (teldtNgayCapPhatVon.Value.IsNullOrEmpty())
            //{
            //    CommonFunction.ThongBaoTrong(lblNgayPhatVon.Content.ToString());
            //    teldtNgayCapPhatVon.Focus();
            //    return false;
            //}
            else if (txtDienGiai.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblDienGiai.Content.ToString());
                txtDienGiai.Focus();
                return false;
            }
            else if (txtCanBoNhanTUng.Tag.IsNullOrEmpty())
            {
                CommonFunction.ThongBaoTrong(lblCanBoPhatVon.Content.ToString());
                txtCanBoNhanTUng.Focus();
                return false;
            }
            else if (txtTongTien.Value.IsNullOrEmpty())
            {
                CommonFunction.ThongBaoTrong(lblTongTienGiaiNgan.Content.ToString());
                txtTongTien.Focus();
                return false;
            }
            return true;
        }

        void LoadDataForm()
        {
            DataSet ds = null;
            if (!LObject.IsNullOrEmpty(_objKiemSoat)) ds = new TinDungProcess().GetThongTinChiTietGDichGiaiNgan(_objKiemSoat.SO_GIAO_DICH);
            else ds = new TinDungProcess().GetThongTinChiTietGDichGiaiNgan(TDVMGIAINGAN.MA_GIAO_DICH);
            if(ds!=null && ds.Tables.Count>0)
            {
                LoadTabThongTinChung(ds);
                LoadTabKiemSoat(ds);
                OnModify();
            }
        }
        void LoadTabThongTinChung(DataSet ds)
            {
                try
                {
                    DataTable dt = ds.Tables["TTIN_CTIET"];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        txtSoPhieu.Text = dt.Rows[0]["MA_GDICH"].ToString();
                        txtDienGiai.Text = dt.Rows[0]["DIEN_GIAI"].ToString();
                        //teldtNgayCapPhatVon.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_PHAT_VON"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                        teldtNgayGiaoDich.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_GIAO_DICH"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                        txtCanBoNhanTUng.Tag = txtCanBoNhanTUng.Text = dt.Rows[0]["MA_CBQL"].ToString();
                        lblTenCanBo.Content = dt.Rows[0]["MA_BPHAN"].ToString();
                        txtBoPhan.Text = dt.Rows[0]["TEN_BPHAN"].ToString();
                        txtSoCMND.Text = dt.Rows[0]["SO_CMND"].ToString();
                        txtTongTien.Value = Convert.ToDouble(dt.Rows[0]["TONG_TIEN_GNGAN"]);
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(dt.Rows[0]["TTHAI_NVU"].ToString());
                        TThaiNV = dt.Rows[0]["TTHAI_NVU"].ToString();
                        iDGiaDich = Convert.ToInt32(dt.Rows[0]["ID"]);
                        sMaGiaoDich = dt.Rows[0]["MA_GDICH"].ToString();
                    }
                    dt = ds.Tables["KHE_UOC"];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        lstKUOCGN = new List<DANH_SACH_KHE_UOC>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            DANH_SACH_KHE_UOC objDSKUoc = new DANH_SACH_KHE_UOC();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                PropertyInfo proper = objDSKUoc.GetType().GetProperty(dc.ColumnName);
                                if (proper != null)
                                    proper.SetValue(objDSKUoc, dr[dc.ColumnName], null);
                            }
                            lstKUOCGN.Add(objDSKUoc);
                        }
                        LoadDataGridKheUoc();
                    }
                }
                catch (System.Exception ex)
                {
                    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                }
                finally
                {
                }
            }
        void LoadTabKiemSoat(DataSet ds)
        {
            DataTable dt = ds.Tables["TTIN_CTIET"];
            if(dt!=null && dt.Rows.Count>0)
            {
                txtNguoiLap.Text = dt.Rows[0]["NGUOI_NHAP"].ToString();
                txtNguoiCapNhat.Text = dt.Rows[0]["NGUOI_CNHAT"].ToString();
                teldtNgayNhap.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_NHAP"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                if (!dt.Rows[0]["NGAY_CNHAT"].ToString().IsNullOrEmptyOrSpace())
                teldtNgayCNhat.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_CNHAT"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(dt.Rows[0]["TTHAI_BGHI"].ToString());
            }
        }

        void BeforeDelete()
        {
            if (iDGiaDich > 0)
            {
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_GIAI_NGAN,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.XOA,
                lstId);
                OnDelete();
            }
        }

        void OnDelete()
        {
            if (iDGiaDich > 0)
            {
                TDVMGIAINGAN.ID_GIAO_DICH = iDGiaDich;
                TDVMGIAINGAN.MA_GIAO_DICH = sMaGiaoDich;
                TDVMGIAINGAN.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                TDVMGIAINGAN.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                List<ClientResponseDetail> lstClientDetail = new List<ClientResponseDetail>();
                int iret = new TinDungProcess().XoaGiaoDichGiaiNgan(ref TDVMGIAINGAN, ref lstClientDetail);
                AfterDelete(lstClientDetail, iret);
            }
        }

        void AfterDelete(List<ClientResponseDetail> lstResponse, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponse);
            if (iret > 0)
            {
                List<int> lstId = new List<int>();
                lstId.Add(TDVMGIAINGAN.ID_GIAO_DICH);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_GIAI_NGAN,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.XOA,
                lstId);
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
                if (iret>0)
                {
                    CommonFunction.CloseUserControl(this);
                }
                LoadDataForm();
                if ((bool)cbMultiAdd.IsChecked)
                    ClearForm();
            }
        }

        void BeforeApprove()
        {
            if (iDGiaDich>0)
            {
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_GIAI_NGAN,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.DUYET,
                lstId);
                OnApprove();
            }
        }

        private void OnPreview()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(iDGiaDich) || LObject.IsNullOrEmpty(sMaGiaoDich))
            {
                LMessage.ShowMessage("M.TinDungTT.GiaiNgan.ucGiaiNganCT.KhongCoThongTinSo", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();

                GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.GDKT_IN_GIAO_DICH;
                objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDVM_GIAI_NGAN;

                GDKT_GIAO_DICH objGDKT_GIAO_DICH = new GDKT_GIAO_DICH();
                objGDKT_GIAO_DICH.MaGiaoDich = sMaGiaoDich;

                doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                doiTuongBaoCao.objGDKT_GIAO_DICH = objGDKT_GIAO_DICH;
                xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
            }
        }

        private void OnPreviewBH()
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                // Cảnh báo khi không có dữ liệu
                if (LObject.IsNullOrEmpty(iDGiaDich) || LObject.IsNullOrEmpty(sMaGiaoDich))
                {
                    LMessage.ShowMessage("M.TinDungTT.GiaiNgan.ucGiaiNganCT.KhongCoThongTinSo", LMessage.MessageBoxType.Warning);
                    return;
                }
                else
                {
                    BaoCaoProcess xemBaoCao = new BaoCaoProcess();
                    HT_BAOCAO htBaoCao = new HT_BAOCAO();
                    List<HT_BAOCAO_TSO> lstHtBaoCaoTso = new List<HT_BAOCAO_TSO>();
                    xemBaoCao.LayThongTinBaoCao(DatabaseConstant.DanhSachBaoCaoTheoDinhKy.BHTH_DSACH_BAO_VE_VON_VAY.layIdBaoCao(), DatabaseConstant.DanhSachBaoCaoTheoDinhKy.BHTH_DSACH_BAO_VE_VON_VAY.layMaBaoCao(), ref htBaoCao, ref lstHtBaoCaoTso);
                    List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
                    listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.PDF.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayPhatVon", LDateTime.DateToString(teldtNgayGiaoDich.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat), ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayBaoCao", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("P_NguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("@P_MaDonVi", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.GUIPARAM.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGiaoDich", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("@MaCum", "%", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("@MaGiaoDich", sMaGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("@TuNgay", LDateTime.DateToString(teldtNgayGiaoDich.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("@DenNgay", LDateTime.DateToString(teldtNgayGiaoDich.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("@MaLoaiTien", "VND", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    // Chuẩn bị điều kiện cho báo cáo
                    if (listThamSoBaoCao != null && listThamSoBaoCao.Count > 0)
                    {
                        foreach (HT_BAOCAO_TSO htBaoCaoTso in lstHtBaoCaoTso)
                        {
                            foreach (ThamSoBaoCao thamSoBaoCao in listThamSoBaoCao)
                            {
                                if (htBaoCaoTso.MA_TSO.Equals(thamSoBaoCao.MaThamSo) &&
                                    htBaoCaoTso.LOAI_TSO.Equals(thamSoBaoCao.LoaiThamSo))
                                {
                                    htBaoCaoTso.GTRI_TSO = thamSoBaoCao.GiaTriThamSo;
                                    break;
                                }
                            }
                        }
                    }
                    ApplicationConstant.ResponseStatus retStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    FileBase fileResponse = new FileBase();
                    string responseMessage = null;

                    retStatus = xemBaoCao.LayDuLieu(htBaoCao, lstHtBaoCaoTso, ref fileResponse, ref responseMessage);
                    if (retStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                    {
                        string fileReport = ClientInformation.TempDir + "\\" + fileResponse.FileName + "." + fileResponse.FileFormat;
                        LFile.WriteFileFromByteArray(fileResponse.FileData, fileReport);

                        // show file
                        string folderReport = ClientInformation.TempDir;
                        if (fileResponse.FileFormat == "rar")
                        {
                            LZip.UnZipFiles(fileReport, folderReport, "ng-mFina", false);
                            string format = "";
                            string loaiThamSo = ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri();
                            format = listThamSoBaoCao.Where(item => item.LoaiThamSo.Equals(loaiThamSo)).FirstOrDefault().GiaTriThamSo;
                            string originalFormat = "";
                            if (format.Equals(ApplicationConstant.LoaiDinhDangBaoCao.PDF.layGiaTri()))
                            {
                                originalFormat = "pdf";
                            }
                            else if (format.Equals(ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri()))
                            {
                                originalFormat = "xls";
                            }
                            else if (format.Equals(ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri()))
                            {
                                originalFormat = "doc";
                            }
                            else
                            {
                                originalFormat = "pdf";
                            }
                            string originalFileReport = ClientInformation.TempDir + "\\" + fileResponse.FileName + "." + originalFormat;
                            //Stream stream = LFile.ConvertByteArrayToStream(fileResponse.FileData);
                            Mouse.OverrideCursor = Cursors.Arrow;
                            System.Diagnostics.Process.Start(originalFileReport);
                        }
                        else
                        {
                            //Stream stream = LFile.ConvertByteArrayToStream(fileResponse.FileData);
                            Mouse.OverrideCursor = Cursors.Arrow;
                            System.Diagnostics.Process.Start(fileReport);
                        }
                    }
                    else
                    {
                        Mouse.OverrideCursor = Cursors.Arrow;
                        LMessage.ShowMessage(responseMessage, LMessage.MessageBoxType.Error);
                        return;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            
        }

        void OnApprove()
        {
            if (iDGiaDich > 0)
            {
                TDVMGIAINGAN.ID_GIAO_DICH = iDGiaDich;
                TDVMGIAINGAN.MA_GIAO_DICH = sMaGiaoDich;
                TDVMGIAINGAN.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                TDVMGIAINGAN.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                List<ClientResponseDetail> lstClientDetail = new List<ClientResponseDetail>();
                int iret = new TinDungProcess().DuyetGiaoDichGiaiNgan(ref TDVMGIAINGAN, ref lstClientDetail);
                AfterApprove(lstClientDetail, iret);
            }
        }

        void AfterApprove(List<ClientResponseDetail> lstResponse, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponse);
            if (iret > 0)
            {
                action = DatabaseConstant.Action.XEM;
                List<int> lstId = new List<int>();
                lstId.Add(TDVMGIAINGAN.ID_GIAO_DICH);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_GIAI_NGAN,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.DUYET,
                lstId);
                LoadDataForm();
                CommonFunction.RefreshButton(Toolbar, action, TThaiNV);
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
                if ((bool)cbMultiAdd.IsChecked)
                    ClearForm();
            }
        }

        void BeforeRefuse()
        {
            if (iDGiaDich > 0)
            {
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_GIAI_NGAN,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);
                OnRefuse();
            }
        }

        void OnRefuse()
        {
            if (iDGiaDich > 0)
            {
                TDVMGIAINGAN.ID_GIAO_DICH = iDGiaDich;
                TDVMGIAINGAN.MA_GIAO_DICH = sMaGiaoDich;
                TDVMGIAINGAN.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                TDVMGIAINGAN.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                List<ClientResponseDetail> lstClientDetail = new List<ClientResponseDetail>();
                int iret = new TinDungProcess().TuChoiDuyetGiaoDichGiaiNgan(ref TDVMGIAINGAN, ref lstClientDetail);
                AfterRefuse(lstClientDetail, iret);
            }
        }

        void AfterRefuse(List<ClientResponseDetail> lstResponse, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponse);
            if (iret > 0)
            {
                action = DatabaseConstant.Action.XEM;
                List<int> lstId = new List<int>();
                lstId.Add(TDVMGIAINGAN.ID_GIAO_DICH);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_GIAI_NGAN,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);
                LoadDataForm();
                CommonFunction.RefreshButton(Toolbar, action, TThaiNV);
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
                if ((bool)cbMultiAdd.IsChecked)
                    ClearForm();
            }
        }

        void BeforeCancel()
        {
            if (iDGiaDich > 0)
            {
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_GIAI_NGAN,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                OnCancel();
            }
        }

        void OnCancel()
        {
            if (iDGiaDich > 0)
            {
                TDVMGIAINGAN.ID_GIAO_DICH = iDGiaDich;
                TDVMGIAINGAN.MA_GIAO_DICH = sMaGiaoDich;
                TDVMGIAINGAN.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                TDVMGIAINGAN.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                List<ClientResponseDetail> lstClientDetail = new List<ClientResponseDetail>();
                int iret = new TinDungProcess().ThoaiDuyetGiaoDichGiaiNgan(ref TDVMGIAINGAN, ref lstClientDetail);
                AfterCancel(lstClientDetail, iret);
            }
        }

        void AfterCancel(List<ClientResponseDetail> lstResponse, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponse);
            if (iret > 0)
            {
                action = DatabaseConstant.Action.XEM;
                List<int> lstId = new List<int>();
                lstId.Add(TDVMGIAINGAN.ID_GIAO_DICH);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_GIAI_NGAN,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                LoadDataForm();
                CommonFunction.RefreshButton(Toolbar, action, TThaiNV);
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
                if ((bool)cbMultiAdd.IsChecked)
                    ClearForm();
            }
        }

        void OnModify()
        {
            if (action.Equals(DatabaseConstant.Action.SUA))
                EnableAllControl(true);
            else
                EnableAllControl(false);
            CommonFunction.RefreshButton(Toolbar, action, TThaiNV);
        }

        void EnableAllControl(bool bBool)
        {
            grbThongTinChung.IsEnabled = bBool;
            grbThongTinTamUng.IsEnabled = bBool;
            raddgrTUngCT.IsReadOnly = !bBool;
            tlbDetailAdd.IsEnabled = bBool;
            tlbDetailDelete.IsEnabled = bBool;
        }

        void ReleaseForm()
        {
            if (iDGiaDich>0)
            {
                List<int> lstId = new List<int>();
                lstId.Add(TDVMGIAINGAN.ID_GIAO_DICH);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_GIAI_NGAN,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                lstId);
            }
        }

        #endregion    

    }
}
