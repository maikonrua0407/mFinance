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
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using PresentationWPF.CustomControl;
using Telerik.Windows.Controls;
using Presentation.Process.Common;
using System.Data;
using Presentation.Process.TinDungServiceRef;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using System.Collections;
using System.Reflection;
using PresentationWPF.BaoCao.DungChung;

namespace PresentationWPF.TinDung.HDTD
{
    /// <summary>
    /// Interaction logic for ucHDTDTheoNhomCT.xaml
    /// </summary>
    public partial class ucHDTDTheoNhomCT : UserControl
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

        //Khai báo tham số
        public event EventHandler OnSavingCompleted;
        int idHDTDVM=0;
        string maHDTDVM="",tthaiNVu="", maNhom = "", maCum = "", maKhuVuc = "";
        public DatabaseConstant.Action action = DatabaseConstant.Action.XEM;
        public DatabaseConstant.Function function = DatabaseConstant.Function.TDVM_HOP_DONG_NHOM;
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        HOP_DONG_TIN_DUNG_VI_MO_NHOM obj = null;
        List<HOP_DONG_TIN_DUNG_VI_MO> lstHDTDVM = null;
        List<int> lstIDXoa = new List<int>();
        List<int> lstId = new List<int>();
        List<CAP_PHE_DUYET> PheDuyet = new List<CAP_PHE_DUYET>();
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucHDTDTheoNhomCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDung;component/HDTD/ucHDTDTheoNhomCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            ResetForm();
            EventHanlerInit();
            ShowControl();
        }

        public ucHDTDTheoNhomCT(OBJ_INPUT objInput)
            : this()
        {
            idHDTDVM = objInput.ID;
            maHDTDVM = objInput.MA;
            action = objInput.ACTION;
            function = objInput.FUNCTION;
            tthaiNVu = objInput.TTHAI_NVU;
        }

        private void EventHanlerInit()
        {
            btnMaCanBoQLy.Click += new RoutedEventHandler(btnMaCanBoQLy_Click);
            btnNhomKhachHang.Click += new RoutedEventHandler(btnNhomKhachHang_Click);
            btnThemTVien.Click += new RoutedEventHandler(btnThemTVien_Click);
            btnXoaTVien.Click += new RoutedEventHandler(btnXoaTVien_Click);
            raddgrDSKhachHang.SelectionChanged += RaddgrDSKhachHang_SelectionChanged;
        }

        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.TinDung.HDTD.ucHDTDTheoNhomCT", "RibbonButton");
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
            arr = hethong.SetVisibleControl("PresentationWPF.TinDung.HDTD.ucHDTDTheoNhomCT", "raddgrDSThanhVien");
            foreach (List<string> lst in arr)
            {
                object item = raddgrDSThanhVien.FindName(lst.First());
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

            if (ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BIDV.layGiaTri()))
                grvColNgayDaoHan.IsVisible = false;
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
                ResetForm();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
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
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            { }
            else if (strTinhNang.Equals("Print"))
            {
                OnPrint();
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
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAP_DU_LIEU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
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
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            { }
            else if (strTinhNang.Equals("Print"))
            {
                OnPrint();
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
                    telControl.Value = dtpControl.SelectedDate;
                else
                    throw new System.NullReferenceException("Không tìm thấy control " + sbControl.ToString());
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void btnThemTVien_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HOP_DONG_TIN_DUNG_VI_MO objHopDong = null;
                if (txtNhomKhachHang.Tag.IsNullOrEmpty() || txtNhomKhachHang.Tag.ToString().IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblMaNhom.Content.ToString());
                    txtNhomKhachHang.Focus();
                    return;
                }
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(txtNhomKhachHang.Tag.ToString());
                lstDieuKien.Add(teldtNgayLapHD.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat));
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_DON_XIN_VAY_VON_DIA_BAN_NGAY_GD", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(false, simplePopupResponse, true);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup.Count > 0)
                {
                    foreach (DataRow dr in lstPopup)
                    {
                        if (!lstHDTDVM.FirstOrDefault(f => f.ID_DXVVVM.Equals(Convert.ToInt32(dr["ID"]))).IsNullOrEmpty())
                            break;
                        objHopDong = new HOP_DONG_TIN_DUNG_VI_MO();
                        if (dr["HE_SO"] != DBNull.Value)
                            objHopDong.HE_SO = Convert.ToDecimal(dr["HE_SO"]);
                        if (dr["ID_DIABAN"] != DBNull.Value)
                            objHopDong.ID_DIABAN = Convert.ToInt32(dr["ID_DIABAN"]);
                        if (dr["ID"] != DBNull.Value)
                            objHopDong.ID_DXVVVM = Convert.ToInt32(dr["ID"]);
                        if (dr["ID_KHANG"] != DBNull.Value)
                            objHopDong.ID_KHANG = Convert.ToInt32(dr["ID_KHANG"]);
                        if (dr["ID_NGUOI_DTN"] != DBNull.Value)
                            objHopDong.ID_NGUOI_DTN = Convert.ToInt32(dr["ID_NGUOI_DTN"]);
                        if (dr["ID_NGUOI_QLY"] != DBNull.Value)
                            objHopDong.ID_NGUOI_QLY = Convert.ToInt32(dr["ID_NGUOI_QLY"]);
                        if (dr["ID_SAN_PHAM"] != DBNull.Value)
                            objHopDong.ID_SAN_PHAM = Convert.ToInt32(dr["ID_SAN_PHAM"]);
                        if (dr["KHOACH_HTHUC_LAP"] != DBNull.Value)
                            objHopDong.KHOACH_HTHUC_LAP = dr["KHOACH_HTHUC_LAP"].ToString();
                        if (dr["KHOACH_NGAY_LAP"] != DBNull.Value)
                            objHopDong.KHOACH_NGAY_LAP = dr["KHOACH_NGAY_LAP"].ToString();
                        if (dr["LSUAT_BDO"] != DBNull.Value)
                            objHopDong.LSUAT_BDO = Convert.ToDecimal(dr["LSUAT_BDO"]);
                        if (dr["LSUAT_CCAU"] != DBNull.Value)
                            objHopDong.LSUAT_CCAU = Convert.ToDecimal(dr["LSUAT_CCAU"]);
                        if (dr["LSUAT_CTRA"] != DBNull.Value)
                            objHopDong.LSUAT_CTRA = Convert.ToDecimal(dr["LSUAT_CTRA"]);
                        if (dr["LSUAT_LOAI"] != DBNull.Value)
                            objHopDong.LSUAT_LOAI = dr["LSUAT_LOAI"].ToString();
                        if (dr["LSUAT_MA"] != DBNull.Value)
                            objHopDong.LSUAT_MA = dr["LSUAT_MA"].ToString();
                        if (dr["LSUAT_QHAN"] != DBNull.Value)
                            objHopDong.LSUAT_QHAN = Convert.ToDecimal(dr["LSUAT_QHAN"]);
                        if (dr["LSUAT_TSUAT"] != DBNull.Value)
                            objHopDong.LSUAT_TSUAT = Convert.ToInt32(dr["LSUAT_TSUAT"]);
                        if (dr["LSUAT_TSUAT_DVI_TINH"] != DBNull.Value)
                            objHopDong.LSUAT_TSUAT_DVI_TINH = dr["LSUAT_TSUAT_DVI_TINH"].ToString();
                        if (dr["LSUAT_VAY"] != DBNull.Value)
                            objHopDong.LSUAT_VAY = Convert.ToDecimal(dr["LSUAT_VAY"]);
                        if (dr["MA_DIABAN"] != DBNull.Value)
                            objHopDong.MA_DIABAN = dr["MA_DIABAN"].ToString();
                        if (dr["MA_DXVVVM"] != DBNull.Value)
                            objHopDong.MA_DXVVVM = dr["MA_DXVVVM"].ToString();
                        if (dr["MA_HMUC"] != DBNull.Value)
                            objHopDong.MA_HMUC = dr["MA_HMUC"].ToString();
                        if (dr["MA_KHANG"] != DBNull.Value)
                            objHopDong.MA_KHANG = dr["MA_KHANG"].ToString();
                        if (dr["MA_NGUOI_DTN"] != DBNull.Value)
                            objHopDong.MA_NGUOI_DTN = dr["MA_NGUOI_DTN"].ToString();
                        if (dr["MA_NGUOI_QLY"] != DBNull.Value)
                            objHopDong.MA_NGUOI_QLY = dr["MA_NGUOI_QLY"].ToString();
                        if (dr["MA_SAN_PHAM"] != DBNull.Value)
                            objHopDong.MA_SAN_PHAM = dr["MA_SAN_PHAM"].ToString();
                        if (dr["MUC_DICH_VAY"] != DBNull.Value)
                            objHopDong.MUC_DICH_VAY = dr["MUC_DICH_VAY"].ToString();
                        if (dr["NGANH_KINH_TE"] != DBNull.Value)
                            objHopDong.NGANH_KINH_TE = dr["NGANH_KINH_TE"].ToString();
                        if (dr["NGAY_CHUYEN_QH"] != DBNull.Value)
                            objHopDong.NGAY_CHUYEN_QH = dr["NGAY_CHUYEN_QH"].ToString();
                        if (dr["NGAY_DAO_HAN"] != DBNull.Value)
                            objHopDong.NGAY_DAO_HAN = dr["NGAY_DAO_HAN"].ToString();
                        if (dr["NGAY_GIA_HAN"] != DBNull.Value)
                            objHopDong.NGAY_GIA_HAN = dr["NGAY_GIA_HAN"].ToString();
                        if (dr["NGAY_HD"] != DBNull.Value)
                            objHopDong.NGAY_HD = dr["NGAY_HD"].ToString();
                        if (dr["PHI_MO_HD"] != DBNull.Value)
                            objHopDong.PHI_MO_HD = Convert.ToDecimal(dr["PHI_MO_HD"]);
                        if (dr["PHUONG_THUC_VAY"] != DBNull.Value)
                            objHopDong.PHUONG_THUC_VAY = dr["PHUONG_THUC_VAY"].ToString();
                        objHopDong.SO_GDICH = "";
                        if (dr["SO_TIEN_CAN"] != DBNull.Value)
                            objHopDong.SO_TIEN_CAN = Convert.ToDecimal(dr["SO_TIEN_CAN"]);
                        if (dr["SO_TIEN_GOC_MOI_KY"] != DBNull.Value)
                            objHopDong.SO_TIEN_GOC_MOI_KY = Convert.ToDecimal(dr["SO_TIEN_GOC_MOI_KY"]);
                        if (dr["SO_TIEN_LAI_MOI_KY"] != DBNull.Value)
                            objHopDong.SO_TIEN_LAI_MOI_KY = Convert.ToDecimal(dr["SO_TIEN_LAI_MOI_KY"]);
                        if (dr["SO_TIEN_MOI_KY"] != DBNull.Value)
                            objHopDong.SO_TIEN_MOI_KY = Convert.ToDecimal(dr["SO_TIEN_MOI_KY"]);
                        if (dr["SO_TIEN_TKBB"] != DBNull.Value)
                            objHopDong.SO_TIEN_TKBB = Convert.ToDecimal(dr["SO_TIEN_TKBB"]);
                        if (dr["SO_TIEN_TU_CO"] != DBNull.Value)
                            objHopDong.SO_TIEN_TU_CO = Convert.ToDecimal(dr["SO_TIEN_TU_CO"]);
                        if (dr["SO_TIEN_VAY"] != DBNull.Value)
                            objHopDong.SO_TIEN_VAY = Convert.ToDecimal(dr["SO_TIEN_VAY"]);
                        if (dr["SO_TIEN_XIN_VAY"] != DBNull.Value)
                            objHopDong.SO_TIEN_XIN_VAY = Convert.ToDecimal(dr["SO_TIEN_XIN_VAY"]);
                        if (dr["SO_TKHOAN_NHAN_NO"] != DBNull.Value)
                            objHopDong.SO_TKHOAN_NHAN_NO = dr["SO_TKHOAN_NHAN_NO"].ToString();
                        if (dr["TGIAN_VAY"] != DBNull.Value)
                            objHopDong.TGIAN_VAY = Convert.ToInt32(dr["TGIAN_VAY"]);
                        if (dr["TGIAN_VAY_DVI_TINH"] != DBNull.Value)
                            objHopDong.TGIAN_VAY_DVI_TINH = dr["TGIAN_VAY_DVI_TINH"].ToString();
                        if (dr["TEN_KHANG"] != DBNull.Value)
                            objHopDong.TEN_KHANG = dr["TEN_KHANG"].ToString();
                        if (dr["DD_GTLQ_SO"] != DBNull.Value)
                            objHopDong.SO_GTO_LQUAN = dr["DD_GTLQ_SO"].ToString();
                        if (dr["TEN_SAN_PHAM"] != DBNull.Value)
                            objHopDong.TEN_SAN_PHAM = dr["TEN_SAN_PHAM"].ToString();
                        if (dr["SP_MUC_DICH_VAY"] != DBNull.Value)
                            objHopDong.SP_MUC_DICH_VAY = dr["SP_MUC_DICH_VAY"].ToString();
                        if (dr["NGUOI_TKE"] != DBNull.Value)
                            objHopDong.TEN_NTK = dr["NGUOI_TKE"].ToString();
                        //if (dr["MA_CUM"] != DBNull.Value)
                        //    obj.MA_CUM = dr["MA_CUM"].ToString();
                        //if (dr["MA_DIABAN"] != DBNull.Value)
                        //    obj.MA_NHOM = dr["MA_DIABAN"].ToString();
                        //if (dr["MA_KVUC"] != DBNull.Value)
                        //    obj.MA_KVUC = dr["MA_KVUC"].ToString();
                        lstHDTDVM.Add(objHopDong);
                    }
                    raddgrDSThanhVien.ItemsSource = null;
                    raddgrDSThanhVien.ItemsSource = lstHDTDVM;
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        void btnNhomKhachHang_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DM_CUM", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(true, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup.Count > 0)
                {
                    foreach (DataRow dr in lstPopup)
                    {
                        txtNhomKhachHang.Text = dr["MA_NHOM"].ToString();
                        txtNhomKhachHang.Tag = Convert.ToInt32(dr["ID"]);
                        lblTenNhom.Content = dr["TEN_NHOM"].ToString();
                        txtMaCanBoQLy.Text = dr["MA_CBO_QLY"].ToString();
                        txtMaCanBoQLy.Tag = Convert.ToInt32(dr["ID_CBO_QLY"]);
                        lblTenCanBoQLy.Content = dr["TEN_CBO_QLY"].ToString();
                        txtTenCum.Text = dr["TEN_CUM"].ToString();
                        txtTenKhuVuc.Text = dr["TEN_KVUC"].ToString();
                        txtTenCum.Tag = dr["MA_CUM"].ToString();
                        txtTenKhuVuc.Tag = dr["MA_KVUC"].ToString();
                        obj.ID_NGUOI_QLY = Convert.ToInt32(dr["ID_CBO_QLY"]);
                        obj.MA_NGUOI_QLY = dr["MA_CBO_QLY"].ToString();
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        void btnMaCanBoQLy_Click(object sender, RoutedEventArgs e)
        {
            
        }

        void btnXoaTVien_Click(object sender, RoutedEventArgs e)
        {
            CAP_PHE_DUYET objPheDuyet = null;
            foreach (HOP_DONG_TIN_DUNG_VI_MO objHopDong in raddgrDSThanhVien.SelectedItems)
            {
                if (objHopDong.ID > 0)
                {
                    objPheDuyet = new CAP_PHE_DUYET();
                    objPheDuyet.ID_TCHIEU = objHopDong.ID;
                    objPheDuyet.MA_TCHIEU = objHopDong.MA_HDTDVM;
                    PheDuyet.Add(objPheDuyet);
                }
            }
        }

        private void RaddgrDSKhachHang_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            try
            {
                HOP_DONG_TIN_DUNG_VI_MO obj = raddgrDSKhachHang.SelectedItem as HOP_DONG_TIN_DUNG_VI_MO;
                DataView drv = raddgrDSKheUoc.ItemsSource as DataView;
                drv.RowFilter = "MA_HDTDVM =" + obj.MA_HDTDVM;
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

        private void ResetForm()
        {
            maHDTDVM = "";
            tthaiNVu = "";
            txtMaHDTD.Text = "";
            teldtNgayLapHD.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            txtSoHDTD.Text = "";
            txtMaCanBoQLy.Text = "";
            txtMaCanBoQLy.Tag = "";
            txtNhomKhachHang.Text = "";
            txtNhomKhachHang.Tag = "";
            txtTenCum.Text = "";
            txtTenKhuVuc.Text = "";
            lblTenNhom.Content = "";
            lblTenCanBoQLy.Content = "";
            raddgrDSThanhVien.ItemsSource = null;
            raddgrDSKhachHang.ItemsSource = null;
            raddgrDSKheUoc.ItemsSource = null;
            SetEnabledAllControls(true);
            action = DatabaseConstant.Action.THEM;
            obj = new HOP_DONG_TIN_DUNG_VI_MO_NHOM();
            lstHDTDVM = new List<HOP_DONG_TIN_DUNG_VI_MO>();
            raddgrDSThanhVien.ItemsSource = lstHDTDVM;
            PheDuyet = new List<CAP_PHE_DUYET>();
            CommonFunction.RefreshButton(Toolbar, action, tthaiNVu, mnuMain, function);
        }

        private void SetEnabledAllControls(bool enable)
        {
            teldtNgayLapHD.IsEnabled = enable;
            txtMaCanBoQLy.IsEnabled = enable;
            txtNhomKhachHang.IsEnabled = enable;
            btnNhomKhachHang.IsEnabled = enable;
            btnThemTVien.IsEnabled = enable;
            btnXoaTVien.IsEnabled = enable;
            raddgrDSThanhVien.IsReadOnly = !enable;
            raddgrDSKhachHang.IsReadOnly = !enable;
            raddgrDSKheUoc.IsReadOnly = !enable;
        }

        private void GetDataForm(BusinessConstant.TrangThaiBanGhi bghi, BusinessConstant.TrangThaiNghiepVu nghiepvu)
        {
            obj.ID_NGUOI_QLY = Convert.ToInt32(txtMaCanBoQLy.Tag);
            obj.MA_HDTDVM = txtMaHDTD.Text;
            obj.SO_HDTDVM = txtSoHDTD.Text;
            obj.NGAY_HD = teldtNgayLapHD.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
            obj.MA_NHOM = txtNhomKhachHang.Text;
            obj.MA_CUM = txtTenCum.Tag.ToString();
            obj.MA_KVUC = txtTenKhuVuc.Tag.ToString();
            lstHDTDVM.ForEach(f => { f.MA_DVI_QLY = ClientInformation.MaDonVi; f.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich; f.TTHAI_BGHI = bghi.layGiaTri(); f.TTHAI_NVU = nghiepvu.layGiaTri(); f.MA_NGUOI_QLY = txtMaCanBoQLy.Text; });
            if (idHDTDVM > 0)
            {
                lstHDTDVM.ForEach(f => { f.NGUOI_CNHAT = ClientInformation.TenDangNhap; f.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai; });
                lstHDTDVM.Where(f => f.ID == 0).ToList().ForEach(f => { f.NGUOI_NHAP = ClientInformation.TenDangNhap; f.NGAY_NHAP = ClientInformation.NgayLamViecHienTai; });
                //obj.DSACH_PHE_DUYET = null;
            }
            else
            {
                lstHDTDVM.ForEach(f => { f.NGUOI_NHAP = ClientInformation.TenDangNhap; f.NGAY_NHAP = ClientInformation.NgayLamViecHienTai; });
            }
            obj.DSACH_HOP_DONG_TDVM = lstHDTDVM.ToArray();
            obj.DSACH_PHE_DUYET = PheDuyet.ToArray();
        }

        public void SetDataForm(string sSoGiaoDich)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dtPar = null;
                LDatatable.MakeParameterTable(ref dtPar);
                LDatatable.AddParameter(ref dtPar, "@SoGiaoDich", "String", sSoGiaoDich);
                ds = new TinDungProcess().GetThongTinHopDongTinDungViMoTheoNhom(dtPar);
                PheDuyet = new List<CAP_PHE_DUYET>();
                Dispatcher.CurrentDispatcher.DelayInvoke("SetDataForm", () =>
                {
                    SetTabThongTinChung(ds);
                    SetTabDanhSachKheUoc(ds);
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
                    obj = new HOP_DONG_TIN_DUNG_VI_MO_NHOM();
                    lstHDTDVM = new List<HOP_DONG_TIN_DUNG_VI_MO>();
                    HOP_DONG_TIN_DUNG_VI_MO objHDTDVM = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        objHDTDVM = new HOP_DONG_TIN_DUNG_VI_MO();
                        if (dr["CAP_LNHIEM"] != DBNull.Value)
                            objHDTDVM.CAP_LNHIEM = dr["CAP_LNHIEM"].ToString();
                        if (dr["CAP_LNHIEM_LSUAT"] != DBNull.Value)
                            objHDTDVM.CAP_LNHIEM_LSUAT = Convert.ToDecimal(dr["CAP_LNHIEM_LSUAT"]);
                        if (dr["HE_SO"] != DBNull.Value)
                            objHDTDVM.HE_SO = Convert.ToDecimal(dr["HE_SO"]);
                        if (dr["ID"] != DBNull.Value)
                            objHDTDVM.ID = idHDTDVM = Convert.ToInt32(dr["ID"]);
                        if (dr["ID_DIABAN"] != DBNull.Value)
                            objHDTDVM.ID_DIABAN = Convert.ToInt32(dr["ID_DIABAN"]);
                        if (dr["ID_DXVVVM"] != DBNull.Value)
                            objHDTDVM.ID_DXVVVM = Convert.ToInt32(dr["ID_DXVVVM"]);
                        if (dr["ID_KHANG"] != DBNull.Value)
                            objHDTDVM.ID_KHANG = Convert.ToInt32(dr["ID_KHANG"]);
                        if (dr["ID_NGUOI_DTN"] != DBNull.Value)
                            objHDTDVM.ID_NGUOI_DTN = Convert.ToInt32(dr["ID_NGUOI_DTN"]);
                        if (dr["ID_NGUOI_QLY"] != DBNull.Value)
                            objHDTDVM.ID_NGUOI_QLY = obj.ID_NGUOI_QLY = Convert.ToInt32(dr["ID_NGUOI_QLY"]);
                        if (dr["ID_SAN_PHAM"] != DBNull.Value)
                            objHDTDVM.ID_SAN_PHAM = Convert.ToInt32(dr["ID_SAN_PHAM"]);
                        if (dr["KHOACH_HTHUC_LAP"] != DBNull.Value)
                            objHDTDVM.KHOACH_HTHUC_LAP = dr["KHOACH_HTHUC_LAP"].ToString();
                        if (dr["KHOACH_NGAY_LAP"] != DBNull.Value)
                            objHDTDVM.KHOACH_NGAY_LAP = dr["KHOACH_NGAY_LAP"].ToString();
                        if (dr["LSUAT_BDO"] != DBNull.Value)
                            objHDTDVM.LSUAT_BDO = Convert.ToDecimal(dr["LSUAT_BDO"]);
                        if (dr["LSUAT_CCAU"] != DBNull.Value)
                            objHDTDVM.LSUAT_CCAU = Convert.ToDecimal(dr["LSUAT_CCAU"]);
                        if (dr["LSUAT_CTRA"] != DBNull.Value)
                            objHDTDVM.LSUAT_CTRA = Convert.ToDecimal(dr["LSUAT_CTRA"]);
                        if (dr["LSUAT_LOAI"] != DBNull.Value)
                            objHDTDVM.LSUAT_LOAI = dr["LSUAT_LOAI"].ToString();
                        if (dr["LSUAT_MA"] != DBNull.Value)
                            objHDTDVM.LSUAT_MA = dr["LSUAT_MA"].ToString();
                        if (dr["LSUAT_QHAN"] != DBNull.Value)
                            objHDTDVM.LSUAT_QHAN = Convert.ToDecimal(dr["LSUAT_QHAN"]);
                        if (dr["LSUAT_TSUAT"] != DBNull.Value)
                            objHDTDVM.LSUAT_TSUAT = Convert.ToInt32(dr["LSUAT_TSUAT"]);
                        if (dr["LSUAT_TSUAT_DVI_TINH"] != DBNull.Value)
                            objHDTDVM.LSUAT_TSUAT_DVI_TINH = dr["LSUAT_TSUAT_DVI_TINH"].ToString();
                        if (dr["LSUAT_VAY"] != DBNull.Value)
                            objHDTDVM.LSUAT_VAY = Convert.ToDecimal(dr["LSUAT_VAY"]);
                        if (dr["MA_DIABAN"] != DBNull.Value)
                            objHDTDVM.MA_DIABAN = dr["MA_DIABAN"].ToString();
                        if (dr["MA_DVI_QLY"] != DBNull.Value)
                            objHDTDVM.MA_DVI_QLY = dr["MA_DVI_QLY"].ToString();
                        if (dr["MA_DVI_TAO"] != DBNull.Value)
                            objHDTDVM.MA_DVI_TAO = dr["MA_DVI_TAO"].ToString();
                        if (dr["MA_DXVVVM"] != DBNull.Value)
                            objHDTDVM.MA_DXVVVM = dr["MA_DXVVVM"].ToString();
                        if (dr["MA_HDTDVM"] != DBNull.Value)
                            objHDTDVM.MA_HDTDVM = dr["MA_HDTDVM"].ToString();
                        if (dr["MA_HMUC"] != DBNull.Value)
                            objHDTDVM.MA_HMUC = dr["MA_HMUC"].ToString();
                        if (dr["MA_KHANG"] != DBNull.Value)
                            objHDTDVM.MA_KHANG = dr["MA_KHANG"].ToString();
                        if (dr["MA_NGUOI_DTN"] != DBNull.Value)
                            objHDTDVM.MA_NGUOI_DTN = dr["MA_NGUOI_DTN"].ToString();
                        if (dr["MA_NGUOI_QLY"] != DBNull.Value)
                            objHDTDVM.MA_NGUOI_QLY = obj.MA_NGUOI_QLY = dr["MA_NGUOI_QLY"].ToString();
                        if (dr["MA_SAN_PHAM"] != DBNull.Value)
                            objHDTDVM.MA_SAN_PHAM = dr["MA_SAN_PHAM"].ToString();
                        if (dr["MUC_DICH_VAY"] != DBNull.Value)
                            objHDTDVM.MUC_DICH_VAY = dr["MUC_DICH_VAY"].ToString();
                        if (dr["NGANH_KINH_TE"] != DBNull.Value)
                            objHDTDVM.NGANH_KINH_TE = dr["NGANH_KINH_TE"].ToString();
                        if (dr["NGAY_CHUYEN_QH"] != DBNull.Value)
                            objHDTDVM.NGAY_CHUYEN_QH = dr["NGAY_CHUYEN_QH"].ToString();
                        if (dr["NGAY_CNHAT"] != DBNull.Value)
                            objHDTDVM.NGAY_CNHAT = dr["NGAY_CNHAT"].ToString();
                        if (dr["NGAY_DAO_HAN"] != DBNull.Value)
                            objHDTDVM.NGAY_DAO_HAN = dr["NGAY_DAO_HAN"].ToString();
                        if (dr["NGAY_GIA_HAN"] != DBNull.Value)
                            objHDTDVM.NGAY_GIA_HAN = dr["NGAY_GIA_HAN"].ToString();
                        if (dr["NGAY_HD"] != DBNull.Value)
                            objHDTDVM.NGAY_HD = obj.NGAY_HD = dr["NGAY_HD"].ToString();
                        if (dr["NGAY_NHAP"] != DBNull.Value)
                            objHDTDVM.NGAY_NHAP = dr["NGAY_NHAP"].ToString();
                        if (dr["NGUOI_CNHAT"] != DBNull.Value)
                            objHDTDVM.NGUOI_CNHAT = dr["NGUOI_CNHAT"].ToString();
                        if (dr["NGUOI_NHAP"] != DBNull.Value)
                            objHDTDVM.NGUOI_NHAP = dr["NGUOI_NHAP"].ToString();
                        if (dr["PHI_MO_HD"] != DBNull.Value)
                            objHDTDVM.PHI_MO_HD = Convert.ToDecimal(dr["PHI_MO_HD"]);
                        if (dr["PHUONG_THUC_VAY"] != DBNull.Value)
                            objHDTDVM.PHUONG_THUC_VAY = dr["PHUONG_THUC_VAY"].ToString();
                        if (dr["SO_GDICH"] != DBNull.Value)
                            objHDTDVM.SO_GDICH = obj.MA_HDTDVM = maHDTDVM = dr["SO_GDICH"].ToString();
                        if (dr["SO_HDTDVM"] != DBNull.Value)
                            objHDTDVM.SO_HDTDVM = obj.SO_HDTDVM = dr["SO_HDTDVM"].ToString();
                        if (dr["SO_TIEN_CAN"] != DBNull.Value)
                            objHDTDVM.SO_TIEN_CAN = Convert.ToDecimal(dr["SO_TIEN_CAN"]);
                        if (dr["SO_TIEN_DA_GNGAN"] != DBNull.Value)
                            objHDTDVM.SO_TIEN_DA_GNGAN = Convert.ToDecimal(dr["SO_TIEN_DA_GNGAN"]);
                        if (dr["SO_TIEN_GOC_MOI_KY"] != DBNull.Value)
                            objHDTDVM.SO_TIEN_GOC_MOI_KY = Convert.ToDecimal(dr["SO_TIEN_GOC_MOI_KY"]);
                        if (dr["SO_TIEN_LAI_MOI_KY"] != DBNull.Value)
                            objHDTDVM.SO_TIEN_LAI_MOI_KY = Convert.ToDecimal(dr["SO_TIEN_LAI_MOI_KY"]);
                        if (dr["SO_TIEN_MOI_KY"] != DBNull.Value)
                            objHDTDVM.SO_TIEN_MOI_KY = Convert.ToDecimal(dr["SO_TIEN_MOI_KY"]);
                        if (dr["SO_TIEN_TKBB"] != DBNull.Value)
                            objHDTDVM.SO_TIEN_TKBB = Convert.ToDecimal(dr["SO_TIEN_TKBB"]);
                        if (dr["SO_TIEN_TU_CO"] != DBNull.Value)
                            objHDTDVM.SO_TIEN_TU_CO = Convert.ToDecimal(dr["SO_TIEN_TU_CO"]);
                        if (dr["SO_TIEN_VAY"] != DBNull.Value)
                            objHDTDVM.SO_TIEN_VAY = Convert.ToDecimal(dr["SO_TIEN_VAY"]);
                        if (dr["SO_TIEN_XIN_VAY"] != DBNull.Value)
                            objHDTDVM.SO_TIEN_XIN_VAY = Convert.ToDecimal(dr["SO_TIEN_XIN_VAY"]);
                        if (dr["SO_TKHOAN_NHAN_NO"] != DBNull.Value)
                            objHDTDVM.SO_TKHOAN_NHAN_NO = dr["SO_TKHOAN_NHAN_NO"].ToString();
                        if (dr["TGIAN_VAY"] != DBNull.Value)
                            objHDTDVM.TGIAN_VAY = Convert.ToInt32(dr["TGIAN_VAY"]);
                        if (dr["TGIAN_VAY_DVI_TINH"] != DBNull.Value)
                            objHDTDVM.TGIAN_VAY_DVI_TINH = dr["TGIAN_VAY_DVI_TINH"].ToString();
                        if (dr["TRGOC_DVI_TINH"] != DBNull.Value)
                            objHDTDVM.TRGOC_DVI_TINH = dr["TRGOC_DVI_TINH"].ToString();
                        if (dr["TRGOC_HTHUC"] != DBNull.Value)
                            objHDTDVM.TRGOC_HTHUC = dr["TRGOC_HTHUC"].ToString();
                        if (dr["TRGOC_SO_KY"] != DBNull.Value)
                            objHDTDVM.TRGOC_SO_KY = Convert.ToInt32(dr["TRGOC_SO_KY"]);
                        if (dr["TRGOC_SO_TKHOAN"] != DBNull.Value)
                            objHDTDVM.TRGOC_SO_TKHOAN = dr["TRGOC_SO_TKHOAN"].ToString();
                        if (dr["TRLAI_DVI_TINH"] != DBNull.Value)
                            objHDTDVM.TRLAI_DVI_TINH = dr["TRLAI_DVI_TINH"].ToString();
                        if (dr["TRLAI_HTHUC"] != DBNull.Value)
                            objHDTDVM.TRLAI_HTHUC = dr["TRLAI_HTHUC"].ToString();
                        if (dr["TRLAI_SO_KY"] != DBNull.Value)
                            objHDTDVM.TRLAI_SO_KY = Convert.ToInt32(dr["TRLAI_SO_KY"]);
                        if (dr["TRLAI_SO_TKHOAN"] != DBNull.Value)
                            objHDTDVM.TRLAI_SO_TKHOAN = dr["TRLAI_SO_TKHOAN"].ToString();
                        if (dr["TTHAI_BGHI"] != DBNull.Value)
                            objHDTDVM.TTHAI_BGHI = dr["TTHAI_BGHI"].ToString();
                        if (dr["TTHAI_GIAI_NGAN"] != DBNull.Value)
                            objHDTDVM.TTHAI_GIAI_NGAN = dr["TTHAI_GIAI_NGAN"].ToString();
                        if (dr["TTHAI_LY_DO"] != DBNull.Value)
                            objHDTDVM.TTHAI_LY_DO = dr["TTHAI_LY_DO"].ToString();
                        if (dr["TTHAI_NVU"] != DBNull.Value)
                            objHDTDVM.TTHAI_NVU = tthaiNVu = dr["TTHAI_NVU"].ToString();
                        if (dr["TEN_KHANG"] != DBNull.Value)
                            objHDTDVM.TEN_KHANG = dr["TEN_KHANG"].ToString();
                        if (dr["DD_GTLQ_SO"] != DBNull.Value)
                            objHDTDVM.SO_GTO_LQUAN = dr["DD_GTLQ_SO"].ToString();
                        if (dr["TEN_SAN_PHAM"] != DBNull.Value)
                            objHDTDVM.TEN_SAN_PHAM = dr["TEN_SAN_PHAM"].ToString();
                        if (dr["MA_DIABAN"] != DBNull.Value)
                            txtNhomKhachHang.Text = dr["MA_DIABAN"].ToString();
                        if (dr["ID_DIABAN"] != DBNull.Value)
                            txtNhomKhachHang.Tag = Convert.ToInt32(dr["ID_DIABAN"]);
                        if (dr["TEN_DIABAN"] != DBNull.Value)
                            lblTenNhom.Content = dr["TEN_DIABAN"].ToString();
                        if (dr["TEN_NGUOI_QLY"] != DBNull.Value)
                            lblTenCanBoQLy.Content = dr["TEN_NGUOI_QLY"].ToString();
                        if (dr["TEN_CUM"] != DBNull.Value)
                            txtTenCum.Text = dr["TEN_CUM"].ToString();
                        if (dr["TEN_KVUC"] != DBNull.Value)
                            txtTenKhuVuc.Text = dr["TEN_KVUC"].ToString();
                        if (dr["SP_MUC_DICH_VAY"] != DBNull.Value)
                            objHDTDVM.SP_MUC_DICH_VAY = dr["SP_MUC_DICH_VAY"].ToString();
                        if (dr["MA_CUM"] != DBNull.Value)
                            obj.MA_CUM = dr["MA_CUM"].ToString();
                        if (dr["MA_DIABAN"] != DBNull.Value)
                            obj.MA_NHOM = dr["MA_DIABAN"].ToString();
                        if (dr["MA_KVUC"] != DBNull.Value)
                            obj.MA_KVUC = dr["MA_KVUC"].ToString();
                        if (dr["NGUOI_TKE"] != DBNull.Value)
                            objHDTDVM.TEN_NTK = dr["NGUOI_TKE"].ToString();
                        lstHDTDVM.Add(objHDTDVM);
                    }
                    txtMaHDTD.Text = obj.MA_HDTDVM;
                    txtSoHDTD.Text = obj.SO_HDTDVM;
                    txtMaCanBoQLy.Text = obj.MA_NGUOI_QLY;
                    txtMaCanBoQLy.Tag = obj.ID_NGUOI_QLY;
                    txtTenCum.Tag = obj.MA_CUM;
                    txtTenKhuVuc.Tag = obj.MA_KVUC;

                    teldtNgayLapHD.Value = obj.NGAY_HD.StringToDate(ApplicationConstant.defaultDateTimeFormat);
                    raddgrDSThanhVien.ItemsSource = null;
                    raddgrDSThanhVien.ItemsSource = lstHDTDVM;
                    raddgrDSKhachHang.ItemsSource = null;
                    raddgrDSKhachHang.ItemsSource = lstHDTDVM;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tthaiNVu);
                    CommonFunction.RefreshButton(Toolbar, action, tthaiNVu, mnuMain, DatabaseConstant.Function.TDVM_HOP_DONG_NHOM);
                    if (action.Equals(DatabaseConstant.Action.SUA))
                        SetEnabledAllControls(true);
                    else
                        SetEnabledAllControls(false);
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

        void SetTabDanhSachKheUoc(DataSet ds)
        {
            try
            {
                DataTable drv = ds.Tables["DSACH_KUOCVM"];

                if (drv != null)
                {
                    foreach (DataRow row in drv.Rows)
                    {
                        string maNgoNguMucDich = row["MUC_DICH_VAY_MA_NNGU"] != null ? row["MUC_DICH_VAY_MA_NNGU"].ToString() : "";
                        string tenMucDich = LLanguage.SearchResourceByKey(maNgoNguMucDich);

                        row["MUC_DICH_VAY_TEN"] = tenMucDich;
                        row.EndEdit();
                        drv.AcceptChanges();
                    }
                }

                raddgrDSKheUoc.ItemsSource = drv.DefaultView;
                raddgrDSKheUoc.Rebind();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void OnModify()
        {
            UtilitiesProcess process = new UtilitiesProcess();
            lstId.Add(idHDTDVM);
            bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_HOP_DONG_NHOM,
            DatabaseConstant.Table.TD_HDTDVM,
            action,
            lstId);
            SetEnabledAllControls(true);
            action = DatabaseConstant.Action.SUA;
            CommonFunction.RefreshButton(Toolbar, action, tthaiNVu, mnuMain, DatabaseConstant.Function.TDVM_HOP_DONG_NHOM);
        }

        bool Validation()
        {
            bool bReturn = true;
            if (teldtNgayLapHD.Value.IsNullOrEmpty())
            {
                teldtNgayLapHD.Focus();
                CommonFunction.ThongBaoTrong(lblNgayLapHDong.Content.ToString());
                return false;
            }
            if (txtNhomKhachHang.Tag.IsNullOrEmpty() || txtNhomKhachHang.Tag.ToString().IsNullOrEmptyOrSpace())
            {
                txtNhomKhachHang.Focus();
                CommonFunction.ThongBaoTrong(lblMaNhom.Content.ToString());
                return false;
            }
            if (lstHDTDVM.IsNullOrEmpty() || lstHDTDVM.Count < 1)
            {
                txtNhomKhachHang.Focus();
                CommonFunction.ThongBaoTrong(lblDanhSachTVien.Content.ToString());
                return false;
            }
            return bReturn;
        }
        void BeforeSave(BusinessConstant.TrangThaiNghiepVu trangthai, BusinessConstant.TrangThaiBanGhi banghi)
        {
            try
            {
                Cursor = Cursors.Wait;
                if (!trangthai.Equals(BusinessConstant.TrangThaiNghiepVu.LUU_TAM))
                {
                    if (!Validation())
                        return;
                }
                GetDataForm(banghi, trangthai);
                OnSave();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        void OnSave()
        {

            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (txtMaHDTD.Text == "")
                iret = new TinDungProcess().HopDongTinDungViMoTheoNhom(DatabaseConstant.Action.THEM, ref obj, ref lstResponseDetail);
            else
                iret = new TinDungProcess().HopDongTinDungViMoTheoNhom(DatabaseConstant.Action.SUA, ref obj, ref lstResponseDetail);
            AfterSave(lstResponseDetail, iret);
        }

        void AfterSave(List<ClientResponseDetail> lstResponseDetail, int iret)
        {

            if (iret > 0)
            {
                action = DatabaseConstant.Action.XEM;
                SetDataForm(obj.MA_HDTDVM);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_HOP_DONG_NHOM,
                DatabaseConstant.Table.TD_HDTDVM,
                DatabaseConstant.Action.SUA,
                lstId);
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
                if ((bool)cbMultiAdd.IsChecked)
                    ResetForm();
            }
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
        }

        void AfterDelete(string sSoGiaoDich, List<ClientResponseDetail> ResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(ResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_HOP_DONG_NHOM,
            DatabaseConstant.Table.TD_HDTDVM,
            DatabaseConstant.Action.XOA,
            lstId);
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            if (iret > 0)
                ResetForm();
        }

        void OnDelete()
        {
            int iret = 0;
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            obj.DSACH_PHE_DUYET = PheDuyet.ToArray();
            iret = new TinDungProcess().HopDongTinDungViMoTheoNhom(DatabaseConstant.Action.XOA, ref obj, ref ResponseDetail);
            AfterDelete(txtMaHDTD.Text, ResponseDetail, iret);
        }

        void BeforeDelete()
        {
            if (tlbDelete.IsEnabled == false)
                return;
            try
            {
                Cursor = Cursors.Wait;
                if (!txtMaHDTD.Text.IsNullOrEmptyOrSpace())
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        PheDuyet = new List<CAP_PHE_DUYET>();
                        foreach (HOP_DONG_TIN_DUNG_VI_MO objHDTDVM in lstHDTDVM)
                        {
                            CAP_PHE_DUYET objCapPheDuyet = new CAP_PHE_DUYET();
                            objCapPheDuyet.ACTION = DatabaseConstant.Action.XOA.getValue();
                            objCapPheDuyet.HAN_MUC_PHE_DUYET = 0;
                            objCapPheDuyet.ID_TCHIEU = objHDTDVM.ID;
                            objCapPheDuyet.MA_TCHIEU = objHDTDVM.MA_HDTDVM;
                            PheDuyet.Add(objCapPheDuyet);
                        }
                        lstId = new List<int>();
                        lstId.Add(idHDTDVM);
                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_HOP_DONG_NHOM,
                        DatabaseConstant.Table.TD_HDTDVM,
                        DatabaseConstant.Action.XOA,
                        lstId);
                        OnDelete();
                    }
                }
            }
            catch (Exception ex)
            {
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_HOP_DONG_NHOM,
                DatabaseConstant.Table.TD_HDTDVM,
                DatabaseConstant.Action.XOA,
                lstId);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        void AfterApprove(string sSoGiaoDich, List<ClientResponseDetail> ResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(ResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_HOP_DONG_NHOM,
            DatabaseConstant.Table.TD_HDTDVM,
            DatabaseConstant.Action.DUYET,
            lstId);
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            SetDataForm(sSoGiaoDich);
        }

        void OnApprove()
        {
            int iret = 0;
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            obj.DSACH_PHE_DUYET = PheDuyet.ToArray();
            iret = new TinDungProcess().HopDongTinDungViMoTheoNhom(DatabaseConstant.Action.DUYET, ref obj, ref ResponseDetail);
            AfterApprove(txtMaHDTD.Text, ResponseDetail);
        }

        void BeforeApprove()
        {
            if (!tlbApprove.IsEnabled)
                return;
            try
            {
                if (!txtMaHDTD.Text.IsNullOrEmptyOrSpace())
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        PheDuyet = new List<CAP_PHE_DUYET>();
                        foreach (HOP_DONG_TIN_DUNG_VI_MO objHDTDVM in lstHDTDVM)
                        {
                            CAP_PHE_DUYET objCapPheDuyet = new CAP_PHE_DUYET();
                            objCapPheDuyet.ACTION = DatabaseConstant.Action.DUYET.getValue();
                            objCapPheDuyet.HAN_MUC_PHE_DUYET = 0;
                            objCapPheDuyet.ID_TCHIEU = objHDTDVM.ID;
                            objCapPheDuyet.MA_TCHIEU = objHDTDVM.MA_HDTDVM;
                            PheDuyet.Add(objCapPheDuyet);
                        }
                        lstId = new List<int>();
                        lstId.Add(idHDTDVM);
                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_HOP_DONG_NHOM,
                        DatabaseConstant.Table.TD_HDTDVM,
                        DatabaseConstant.Action.DUYET,
                        lstId);
                        OnApprove();
                    }
                }
            }
            catch (Exception ex)
            {
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_HOP_DONG_NHOM,
                DatabaseConstant.Table.TD_HDTDVM,
                DatabaseConstant.Action.DUYET,
                lstId);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void AfterRefuse(string sSoGiaoDich, List<ClientResponseDetail> ResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(ResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_HOP_DONG_NHOM,
            DatabaseConstant.Table.TD_HDTDVM,
            DatabaseConstant.Action.TU_CHOI_DUYET,
            lstId);
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            SetDataForm(sSoGiaoDich);
        }

        void OnRefuse()
        {
            int iret = 0;
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            obj.DSACH_PHE_DUYET = PheDuyet.ToArray();
            iret = new TinDungProcess().HopDongTinDungViMoTheoNhom(DatabaseConstant.Action.TU_CHOI_DUYET, ref obj, ref ResponseDetail);
            AfterRefuse(txtMaHDTD.Text, ResponseDetail);
        }

        void BeforeRefuse()
        {
            if (!tlbRefuse.IsEnabled)
                return;
            try
            {
                if (!txtMaHDTD.Text.IsNullOrEmptyOrSpace())
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        PheDuyet = new List<CAP_PHE_DUYET>();
                        foreach (HOP_DONG_TIN_DUNG_VI_MO objHDTDVM in lstHDTDVM)
                        {
                            CAP_PHE_DUYET objCapPheDuyet = new CAP_PHE_DUYET();
                            objCapPheDuyet.ACTION = DatabaseConstant.Action.TU_CHOI_DUYET.getValue();
                            objCapPheDuyet.HAN_MUC_PHE_DUYET = 0;
                            objCapPheDuyet.ID_TCHIEU = objHDTDVM.ID;
                            objCapPheDuyet.MA_TCHIEU = objHDTDVM.MA_HDTDVM;
                            PheDuyet.Add(objCapPheDuyet);
                        }
                        lstId = new List<int>();
                        lstId.Add(idHDTDVM);
                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_HOP_DONG_NHOM,
                        DatabaseConstant.Table.TD_HDTDVM,
                        DatabaseConstant.Action.TU_CHOI_DUYET,
                        lstId);
                        OnRefuse();
                    }
                }
            }
            catch (Exception ex)
            {
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_HOP_DONG_NHOM,
                DatabaseConstant.Table.TD_HDTDVM,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }


        void AfterCancel(string sSoGiaoDich, List<ClientResponseDetail> ResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(ResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_HOP_DONG_NHOM,
            DatabaseConstant.Table.TD_HDTDVM,
            DatabaseConstant.Action.THOAI_DUYET,
            lstId);
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            SetDataForm(sSoGiaoDich);
        }

        void OnCancel()
        {
            int iret = 0;
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            obj.DSACH_PHE_DUYET = PheDuyet.ToArray();
            iret = new TinDungProcess().HopDongTinDungViMoTheoNhom(DatabaseConstant.Action.THOAI_DUYET, ref obj, ref ResponseDetail);
            AfterCancel(txtMaHDTD.Text, ResponseDetail);
        }

        void BeforeCancel()
        {
            if (!tlbCancel.IsEnabled)
                return;
            try
            {
                if (!txtMaHDTD.Text.IsNullOrEmptyOrSpace())
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        PheDuyet = new List<CAP_PHE_DUYET>();
                        foreach (HOP_DONG_TIN_DUNG_VI_MO objHDTDVM in lstHDTDVM)
                        {
                            CAP_PHE_DUYET objCapPheDuyet = new CAP_PHE_DUYET();
                            objCapPheDuyet.ACTION = DatabaseConstant.Action.THOAI_DUYET.getValue();
                            objCapPheDuyet.HAN_MUC_PHE_DUYET = 0;
                            objCapPheDuyet.ID_TCHIEU = objHDTDVM.ID;
                            objCapPheDuyet.MA_TCHIEU = objHDTDVM.MA_HDTDVM;
                            PheDuyet.Add(objCapPheDuyet);
                        }
                        lstId = new List<int>();
                        lstId.Add(idHDTDVM);
                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_HOP_DONG_NHOM,
                        DatabaseConstant.Table.TD_HDTDVM,
                        DatabaseConstant.Action.THOAI_DUYET,
                        lstId);
                        OnCancel();
                    }
                }
            }
            catch (Exception ex)
            {
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_HOP_DONG_NHOM,
                DatabaseConstant.Table.TD_HDTDVM,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        public void OnPrint()
        {
            //if (LObject.IsNullOrEmpty(txtNhomKhachHang.Text) && LObject.IsNullOrEmpty(teldtNgayLapHD.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat)))
            //{
            //    LMessage.ShowMessage("M.DungChung.KhongCoThongTin", LMessage.MessageBoxType.Warning);
            //    return;
            //}
            if (LObject.IsNullOrEmpty(txtNhomKhachHang.Text))
            {
                LMessage.ShowMessage("M.DungChung.KhongCoThongTin", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                lstThamSo.Add(new ThamSoBaoCao("@MaNhom", txtNhomKhachHang.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
               
                lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

                lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", LDateTime.StringToDate(teldtNgayLapHD.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat), "yyyyMMdd").ToLongDateString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

                lstThamSo.Add(new ThamSoBaoCao("@SoHopDong", txtSoHDTD.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

                string maBaoCao = DatabaseConstant.LayMaBaoCaoBIDV(DatabaseConstant.DanhSachBaoCaoBIDV.TDVM_HOP_DONG_VAY_VON);
                xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
            }
        }
        #endregion
    }
}
