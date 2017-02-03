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
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.TinDungServiceRef;
using Telerik.Windows.Controls;
using System.Reflection;
using Telerik.Windows.Controls.GridView;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;
using System.Collections;


namespace PresentationWPF.TinDungTT.ChuyenQuaHan
{
    /// <summary>
    /// Interaction logic for ucChuyenQuaHanCT.xaml
    /// </summary>
    public partial class ucChuyenQuaHanCT : UserControl
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
        TDVM_CHUYEN_NO_QUA_HAN TDVMCNQH = new TDVM_CHUYEN_NO_QUA_HAN();
        List<DANH_SACH_KHE_UOC_CHUYEN_NO_QUA_HAN> lstDANHSACH = null;
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        List<int> lstPopupKU = new List<int>();
        public void LayDuLieuTuPopup(List<int> lst)
        {
            lstPopupKU = lst;
        }
        string TThaiNVu;
        int iDGiaoDich = 0;
        string mAGiaoDich = "";
        public event EventHandler OnSavingCompleted;
        KIEM_SOAT _objKiemSoat;
        DatabaseConstant.Action action;
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucChuyenQuaHanCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDungTT;component/ChuyenQuaHan/ucChuyenQuaHanCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            ClearForm();
            InitEventHanler();
            ShowControl();
        }

        public ucChuyenQuaHanCT(KIEM_SOAT objKiemSoat)
            : this()
        {
            _objKiemSoat = objKiemSoat;
            mAGiaoDich = _objKiemSoat.SO_GIAO_DICH;
            action = _objKiemSoat.action;
            SetDataForm();
        }
        private void InitEventHanler()
        {
            ucNhomNoMoi.EditCellEnd += new EventHandler(ucNhomNoMoi_EditCellEnd);
            ucNguyenNhanQH.EditCellEnd += new EventHandler(ucNguyenNhanQH_EditCellEnd);
        }

        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.TinDungTT.ChuyenQuaHan.ucChuyenQuaHanCT", "RibbonButton");
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
                Modify();
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
                    throw new System.NullReferenceException(LLanguage.SearchResourceByKey("M.TinDungTT.ChuyenQuaHan.ucChuyenQuaHanCT.KhongTimThayControl") + sbControl.ToString());
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void ClearForm()
        {
            TDVMCNQH = new TDVM_CHUYEN_NO_QUA_HAN();
            lstDANHSACH = new List<DANH_SACH_KHE_UOC_CHUYEN_NO_QUA_HAN>();
            txtSoPhieu.Text = "";
            teldtNgayGiaoDich.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            txtDienGiai.Text = "";
            TThaiNVu = "";
            txtNguoiCapNhat.Text = "";
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            txtTrangThai.Text = "";
            lblTrangThai.Content = "";
            teldtNgayCNhat.Value = null;
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            grdKheUocQuaHan.ItemsSource = lstDANHSACH;
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, TThaiNVu,mnuMain);
        }

        private void tlbAdddetail_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            try
            {
                string siDGiaoDich = "";
                List<ClientResponseDetail> lstClientResponse = new List<ClientResponseDetail>();
                foreach (DANH_SACH_KHE_UOC_CHUYEN_NO_QUA_HAN dr in lstDANHSACH)
                {
                    siDGiaoDich += "," + dr.ID_KHE_UOC.ToString();
                }
                if (siDGiaoDich.Length > 0)
                    siDGiaoDich = siDGiaoDich.Substring(1);
                else
                    siDGiaoDich = "0";
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(siDGiaoDich);
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add("KUOCQH");
                lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
                lstDieuKien.Add("%");
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_KHEUOC", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopupKheUocViMo popup = new ucPopupKheUocViMo(true, simplePopupResponse, true);
                popup.LayGiaTriListDataRow = new ucPopupKheUocViMo.LayListDataRow(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Content = popup;
                win.ShowDialog();
                if (lstPopup.Count > 0)
                {
                    List<DANH_SACH_KHE_UOC_CHUYEN_NO_QUA_HAN> lstKUOC = new List<DANH_SACH_KHE_UOC_CHUYEN_NO_QUA_HAN>();
                    foreach (DataRow drv in lstPopup)
                    {
                        DANH_SACH_KHE_UOC_CHUYEN_NO_QUA_HAN objKUOC = new DANH_SACH_KHE_UOC_CHUYEN_NO_QUA_HAN();
                        foreach (DataColumn dtl in drv.Table.Columns)
                        {
                            PropertyInfo proper = objKUOC.GetType().GetProperty(dtl.ColumnName);
                            if (!LObject.IsNullOrEmpty(proper))
                            {
                                if(proper.PropertyType.Equals(typeof(int)))
                                    proper.SetValue(objKUOC,Convert.ToInt32(drv[dtl.ColumnName]),null);
                                else if (proper.PropertyType.Equals(typeof(decimal)))
                                    proper.SetValue(objKUOC, Convert.ToDecimal(drv[dtl.ColumnName]), null);
                                else
                                    proper.SetValue(objKUOC, drv[dtl.ColumnName].ToString(), null);
                            }
                        }
                        lstKUOC.Add(objKUOC);
                    }
                    lstDANHSACH.AddRange(lstKUOC);
                    LoadGriDGiaoDich();
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

        private void tlbDeletedetail_Click(object sender, RoutedEventArgs e)
        {
            if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            Cursor = Cursors.Wait;
            try
            {
                foreach (DANH_SACH_KHE_UOC_CHUYEN_NO_QUA_HAN dr in grdKheUocQuaHan.SelectedItems)
                {
                    lstDANHSACH.Remove(dr);
                }
                grdKheUocQuaHan.ItemsSource = null;
                grdKheUocQuaHan.ItemsSource = lstDANHSACH;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void tlbCalDetail_Click(object sender, RoutedEventArgs e)
        {
            int iret = 0;
            Cursor = Cursors.Wait;
            try
            {
                if (!LObject.IsNullOrEmpty(grdKheUocQuaHan.RowInEditMode))
                    grdKheUocQuaHan.CommitEdit();
                TDVMCNQH.NGAY_GIAO_DICH = TDVMCNQH.NGAY_LAP = ClientInformation.NgayLamViecHienTai;
                lstDANHSACH = grdKheUocQuaHan.ItemsSource as List<DANH_SACH_KHE_UOC_CHUYEN_NO_QUA_HAN>;
                TDVMCNQH.DSACH_KHE_UOC = lstDANHSACH.ToArray();
                List<ClientResponseDetail> lstResponseDatail = new List<ClientResponseDetail>();
                iret = new TinDungProcess().TinhToanTrichLapDuPhong(ref TDVMCNQH, ref lstResponseDatail);
                CommonFunction.ThongBaoKetQua(lstResponseDatail);
                if (iret>0)
                {
                    
                    lstDANHSACH = TDVMCNQH.DSACH_KHE_UOC.ToList();
                    LoadGriDGiaoDich();
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        void ucNguyenNhanQH_EditCellEnd(object sender, EventArgs e)
        {
            DANH_SACH_KHE_UOC_CHUYEN_NO_QUA_HAN objKUOC = ucNguyenNhanQH.cellEdit.ParentRow.Item as DANH_SACH_KHE_UOC_CHUYEN_NO_QUA_HAN;
            lstDANHSACH[lstDANHSACH.IndexOf(objKUOC)].NGUYEN_NHAN_QUA_HAN = ucNguyenNhanQH.GiaTri;
        }

        void ucNhomNoMoi_EditCellEnd(object sender, EventArgs e)
        {
            DANH_SACH_KHE_UOC_CHUYEN_NO_QUA_HAN objKUOC = ucNhomNoMoi.cellEdit.ParentRow.Item as DANH_SACH_KHE_UOC_CHUYEN_NO_QUA_HAN;
            lstDANHSACH[lstDANHSACH.IndexOf(objKUOC)].NHOM_NO_MOI = ucNhomNoMoi.GiaTri;
        }

        private void grdKheUocQuaHan_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            DANH_SACH_KHE_UOC_CHUYEN_NO_QUA_HAN objKUOC = e.Cell.ParentRow.Item as DANH_SACH_KHE_UOC_CHUYEN_NO_QUA_HAN;
            lstDANHSACH.ElementAt(lstDANHSACH.IndexOf(objKUOC)).CHENH_LECH = objKUOC.DU_PHONG_PHAI_TRICH - objKUOC.DU_PHONG_DA_TRICH;
        }
        #endregion

        /// <summary>
        /// Xu ly nghiep vu
        /// </summary>
        #region Xu ly nghiep vu
        void LoadGriDGiaoDich()
        {
            grdKheUocQuaHan.ItemsSource = null;
            grdKheUocQuaHan.ItemsSource = lstDANHSACH;
        }

        bool Validation()
        {
            bool bReturn = true;
            try
            {
                if (txtDienGiai.Text.IsNullOrEmpty())
                {
                    CommonFunction.ThongBaoTrong(lblDienGiai.Content.ToString());
                    txtDienGiai.Focus();
                    bReturn = false;
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
            List<int> lstId = new List<int>();
            lstId.Add(iDGiaoDich);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_CHUYEN_NO_QUA_HAN,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.SUA,
            lstId);
            action = DatabaseConstant.Action.SUA;
            OnModify();
            //SetEnabledAllControl(true);
        }
        void GetDataForm(BusinessConstant.TrangThaiBanGhi bghi, BusinessConstant.TrangThaiNghiepVu nghiepvu)
        {

            try
            {
                TDVMCNQH.NGAY_GIAO_DICH = LDateTime.DateToString(teldtNgayGiaoDich.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
                TDVMCNQH.ID_GIAO_DICH = iDGiaoDich;
                TDVMCNQH.MA_GIAO_DICH = mAGiaoDich;
                TDVMCNQH.LOAI_TIEN = ClientInformation.MaDongNoiTe;
                TDVMCNQH.DIEN_GIAI = TDVMCNQH.LY_DO = txtDienGiai.Text;
                TDVMCNQH.MA_DVI = ClientInformation.MaDonViGiaoDich;
                TDVMCNQH.TRANG_THAI_BAN_GHI = bghi.layGiaTri();
                TDVMCNQH.TRANG_THAI_NGHIEP_VU = nghiepvu.layGiaTri();
                TDVMCNQH.NGUOI_LAP = ClientInformation.TenDangNhap;
                TDVMCNQH.NGAY_LAP = ClientInformation.NgayLamViecHienTai;
                TDVMCNQH.MA_GIAO_DICH = mAGiaoDich;
                if (iDGiaoDich>0)
                {
                    TDVMCNQH.NGUOI_LAP = txtNguoiLap.Text;
                    TDVMCNQH.NGAY_LAP = LDateTime.DateToString((DateTime)teldtNgayNhap.Value, ApplicationConstant.defaultDateTimeFormat);
                    TDVMCNQH.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                    TDVMCNQH.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                }
                lstDANHSACH = grdKheUocQuaHan.ItemsSource as List<DANH_SACH_KHE_UOC_CHUYEN_NO_QUA_HAN>;
                TDVMCNQH.DSACH_KHE_UOC = lstDANHSACH.ToArray();
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }

        }
        void SetDataForm()
        {
            Cursor = Cursors.Wait;
            try
            {
                DataSet ds = new TinDungProcess().GetThongTinChuyenNoQuaHan(mAGiaoDich);
                if (LObject.IsNullOrEmpty(ds) || ds.Tables.Count<1)
                    return;
                SetTabThongTinChung(ds);
                SetTabThongTinKiemSoat(ds);
                if (action.Equals(DatabaseConstant.Action.SUA))
                    Modify();
                else
                    OnModify();
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }
        void SetTabThongTinChung(DataSet ds)
        {
            try
            {
                DataTable dt = ds.Tables["TTIN_CTIET"];
                if (LObject.IsNullOrEmpty(TDVMCNQH)) TDVMCNQH = new TDVM_CHUYEN_NO_QUA_HAN();
                if (LObject.IsNullOrEmpty(dt) || dt.Rows.Count < 1)
                    return;
                TDVMCNQH.DIEN_GIAI = dt.Rows[0]["DIEN_GIAI"].ToString();
                iDGiaoDich = TDVMCNQH.ID_GIAO_DICH = Convert.ToInt32(dt.Rows[0]["ID_GIAO_DICH"]);
                TDVMCNQH.LOAI_TIEN = dt.Rows[0]["LOAI_TIEN"].ToString();
                TDVMCNQH.LOAI_TIEN = dt.Rows[0]["LOAI_TIEN"].ToString();
                TDVMCNQH.LY_DO = dt.Rows[0]["LY_DO"].ToString();
                TDVMCNQH.MA_DVI = dt.Rows[0]["MA_DVI"].ToString();
                TDVMCNQH.MA_DVI = dt.Rows[0]["MA_DVI"].ToString();
                TDVMCNQH.MA_GIAO_DICH = dt.Rows[0]["MA_GIAO_DICH"].ToString();
                TDVMCNQH.NGAY_GIAO_DICH = dt.Rows[0]["NGAY_GIAO_DICH"].ToString();
                TDVMCNQH.NGAY_LAP = dt.Rows[0]["NGAY_LAP"].ToString();
                TDVMCNQH.NGUOI_LAP = dt.Rows[0]["NGUOI_LAP"].ToString();
                TDVMCNQH.TRANG_THAI_BAN_GHI = dt.Rows[0]["TRANG_THAI_BAN_GHI"].ToString();
                TDVMCNQH.TRANG_THAI_NGHIEP_VU = dt.Rows[0]["TRANG_THAI_NGHIEP_VU"].ToString();
                TDVMCNQH.NGUOI_CAP_NHAT = dt.Rows[0]["NGUOI_CAP_NHAT"] != null ? dt.Rows[0]["NGUOI_CAP_NHAT"].ToString() : "";
                TDVMCNQH.NGAY_CAP_NHAT = dt.Rows[0]["NGAY_CAP_NHAT"] != null ? dt.Rows[0]["NGAY_CAP_NHAT"].ToString() : "";
                txtDienGiai.Text = TDVMCNQH.DIEN_GIAI;
                mAGiaoDich = txtSoPhieu.Text = TDVMCNQH.MA_GIAO_DICH;
                TThaiNVu = TDVMCNQH.TRANG_THAI_NGHIEP_VU;
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TDVMCNQH.TRANG_THAI_NGHIEP_VU);
                teldtNgayGiaoDich.Value = LDateTime.StringToDate(TDVMCNQH.NGAY_GIAO_DICH, ApplicationConstant.defaultDateTimeFormat);
                dt = ds.Tables["KHE_UOC"];
                lstDANHSACH = new List<DANH_SACH_KHE_UOC_CHUYEN_NO_QUA_HAN>();
                foreach (DataRow dr in dt.Rows)
                {
                    DANH_SACH_KHE_UOC_CHUYEN_NO_QUA_HAN objKUOC = new DANH_SACH_KHE_UOC_CHUYEN_NO_QUA_HAN();
                    foreach (DataColumn dcl in dt.Columns)
                    {
                        PropertyInfo proper = objKUOC.GetType().GetProperty(dcl.ColumnName);
                        if(!LObject.IsNullOrEmpty(proper))
                        {
                            if (proper.PropertyType.Equals(typeof(int)))
                                proper.SetValue(objKUOC, Convert.ToInt32(dr[dcl.ColumnName]),null);
                            else if (proper.PropertyType.Equals(typeof(decimal)))
                                proper.SetValue(objKUOC, Convert.ToDecimal(dr[dcl.ColumnName]), null);
                            else
                                proper.SetValue(objKUOC, dr[dcl.ColumnName].ToString(), null);
                        }
                    }
                    lstDANHSACH.Add(objKUOC);
                }
                grdKheUocQuaHan.ItemsSource = null;
                grdKheUocQuaHan.ItemsSource = lstDANHSACH;
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
            }

        }
        void SetTabThongTinKiemSoat(DataSet ds)
        {
            try
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
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
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
                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                int iret = 0;
                if (iDGiaoDich == 0)
                    iret = new TinDungProcess().ThemMoiGiaoDichChuyenNo(ref TDVMCNQH, ref lstResponseDetail);
                else
                    iret = new TinDungProcess().SuaGiaoDichChuyenNo(ref TDVMCNQH, ref lstResponseDetail);
                iDGiaoDich = TDVMCNQH.ID_GIAO_DICH;
                mAGiaoDich = TDVMCNQH.MA_GIAO_DICH;
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
                lstId.Add(iDGiaoDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHUYEN_NO_QUA_HAN,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.SUA,
                lstId);
                action = DatabaseConstant.Action.XEM;
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
                if (iret > 0)
                    SetDataForm();
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void OnPreview()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(iDGiaoDich) && LObject.IsNullOrEmpty(mAGiaoDich))
            {
                LMessage.ShowMessage("M.TinDungTT.ChuyenQuaHan.ucChuyenQuaHanCT.KhongCoThongTinSo", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();

                GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.GDKT_IN_GIAO_DICH;
                objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDVM_CHUYEN_NO_QUA_HAN;

                GDKT_GIAO_DICH objGDKT_GIAO_DICH = new GDKT_GIAO_DICH();
                objGDKT_GIAO_DICH.MaGiaoDich = mAGiaoDich;

                doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                doiTuongBaoCao.objGDKT_GIAO_DICH = objGDKT_GIAO_DICH;
                xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
            }
        }

        void BeforeDelete()
        {
            Cursor = Cursors.Wait;
            try
            {
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHUYEN_NO_QUA_HAN,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.XOA,
                lstId);
                OnDelete();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHUYEN_NO_QUA_HAN,
                DatabaseConstant.Table.TD_KUOCVM,
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
            if (iDGiaoDich != 0)
            {
                TDVMCNQH.ID_GIAO_DICH = iDGiaoDich;
                TDVMCNQH.MA_GIAO_DICH = mAGiaoDich;
                TDVMCNQH.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                TDVMCNQH.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                iret = new TinDungProcess().XoaGiaoDichChuyenNo(ref TDVMCNQH, ref lstResponseDetail);
            }
            AfterDelete(lstResponseDetail, iret);
        }
        void AfterDelete(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(iDGiaoDich);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_CHUYEN_NO_QUA_HAN,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.XOA,
            lstId);
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            Cursor = Cursors.Arrow;
            if (iret == 0)
                SetDataForm();
            else
                CommonFunction.CloseUserControl(this);
        }

        void BeforeApprove()
        {
            try
            {
                Cursor = Cursors.Wait;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHUYEN_NO_QUA_HAN,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.DUYET,
                lstId);
                OnApprove();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHUYEN_NO_QUA_HAN,
                DatabaseConstant.Table.TD_KUOCVM,
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
            if (iDGiaoDich != 0)
            {
                TDVMCNQH.ID_GIAO_DICH = iDGiaoDich;
                TDVMCNQH.MA_GIAO_DICH = mAGiaoDich;
                TDVMCNQH.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                TDVMCNQH.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                iret = new TinDungProcess().DuyetGiaoDichChuyenNo(ref TDVMCNQH, ref lstResponseDetail);
            }
            AfterApprove(lstResponseDetail, iret);
        }
        void AfterApprove(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(iDGiaoDich);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_CHUYEN_NO_QUA_HAN,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.DUYET,
            lstId);
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            SetDataForm();
            Cursor = Cursors.Arrow;
        }

        void BeforeRefuse()
        {
            try
            {
                Cursor = Cursors.Wait;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHUYEN_NO_QUA_HAN,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);
                OnRefuse();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHUYEN_NO_QUA_HAN,
                DatabaseConstant.Table.TD_KUOCVM,
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
            if (iDGiaoDich != 0)
            {
                TDVMCNQH.ID_GIAO_DICH = iDGiaoDich;
                TDVMCNQH.MA_GIAO_DICH = mAGiaoDich;
                TDVMCNQH.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                TDVMCNQH.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                iret = new TinDungProcess().TuChoiDuyetGiaoDichChuyenNo(ref TDVMCNQH, ref lstResponseDetail);
            }
            AfterRefuse(lstResponseDetail, iret);
        }
        void AfterRefuse(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(iDGiaoDich);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_CHUYEN_NO_QUA_HAN,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.TU_CHOI_DUYET,
            lstId);
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            Cursor = Cursors.Arrow;
            SetDataForm();
        }

        void BeforeCancel()
        {
            try
            {
                Cursor = Cursors.Wait;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHUYEN_NO_QUA_HAN,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                OnCancel();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHUYEN_NO_QUA_HAN,
                DatabaseConstant.Table.TD_KUOCVM,
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
            if (iDGiaoDich != 0)
            {
                TDVMCNQH.ID_GIAO_DICH = iDGiaoDich;
                TDVMCNQH.MA_GIAO_DICH = mAGiaoDich;
                TDVMCNQH.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                TDVMCNQH.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                iret = new TinDungProcess().ThoaiDuyetGiaoDichChuyenNo(ref TDVMCNQH, ref lstResponseDetail);
            }
            AfterCancel(lstResponseDetail, iret);
        }
        void AfterCancel(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(iDGiaoDich);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_CHUYEN_NO_QUA_HAN,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.THOAI_DUYET,
            lstId);
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            Cursor = Cursors.Arrow;
            SetDataForm();
        }

        void OnModify()
        {
            if (action.Equals(DatabaseConstant.Action.SUA))
            {
                tlbAdddetail.IsEnabled = true;
                tlbDeletedetail.IsEnabled = true;
                tlbCalDetail.IsEnabled = true;
                grdKheUocQuaHan.IsReadOnly = false;
                txtDienGiai.IsEnabled = true;
            }
            else
            {
                tlbAdddetail.IsEnabled = false;
                tlbDeletedetail.IsEnabled = false;
                tlbCalDetail.IsEnabled = false;
                grdKheUocQuaHan.IsReadOnly = true;
                txtDienGiai.IsEnabled = false;
            }
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_CHUYEN_NO_QUA_HAN);
        }
        #endregion

        
    }
}
