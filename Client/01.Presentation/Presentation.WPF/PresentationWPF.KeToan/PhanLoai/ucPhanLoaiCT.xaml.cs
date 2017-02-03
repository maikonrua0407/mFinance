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
using System.Data;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;

namespace PresentationWPF.KeToan.PhanLoai
{
    /// <summary>
    /// Interaction logic for ucPhanLoaiCT.xaml
    /// </summary>
    public partial class ucPhanLoaiCT : UserControl
    {
        /// <summary>
        /// Khai bao
        /// </summary>
        #region Khai bao
        public static RoutedCommand AddCommand = new RoutedCommand();
        public static RoutedCommand ImportCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand CloneCommand = new RoutedCommand();
        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
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

        // Source combobox
        List<AutoCompleteEntry> lstSourceLoaiTK = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiKHNB = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceThuNhap = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTinhChatTK = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKyHieu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTChatGoc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTChatBTru = new List<AutoCompleteEntry>();
        List<TTHAI_LY_DO> lstLyDo = new List<TTHAI_LY_DO>();
        public void LayDuLieuLyDo(List<TTHAI_LY_DO> lst)
        {
            lstLyDo = lst;
        }

        public event EventHandler OnSavingCompleted;

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        private string tthaiNvu = "";
        public string TthaiNvu
        {
            get { return tthaiNvu; }
            set { tthaiNvu = value; }
        }

        private int _idPhanLoai = -1;
        private int _idKyHieuPLoai = -1;

        private bool? updateTNCP = null;
        #endregion

        #region Khoi tao
        public ucPhanLoaiCT()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KeToan;component/PhanLoai/ucPhanLoaiCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindShortkey();
            cmbLoaiTK.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiTK_SelectionChanged);
            cmbLoaiKHNBo.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiKHNBo_SelectionChanged);
            cmbTinhChatTK.SelectionChanged += new SelectionChangedEventHandler(cmbTinhChatTK_SelectionChanged);
            raddtTuNgayApDung.Value = LDateTime.StringToDate(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            lblTenPLTKCapTren.Content = "";
            txtMaPLTKCapTren.KeyDown += new KeyEventHandler(txtMaPLTKCapTren_KeyDown);
            InitCombobox();
            txtMaPLTKCapTren.Focus();
            ResetForm();
        }

        public ucPhanLoaiCT(int id, string tthai, DatabaseConstant.Action action) : this()
        {
            _idPhanLoai = id;
            tthaiNvu = tthai;
            SetFormData();
            beforeModifyFromList(action);
        }

        //private void KhoiTaoChung()
        //{
        //    InitializeComponent();
        //    HeThong hethong = new HeThong();
        //    hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KeToan;component/PhanLoai/ucPhanLoaiCT.xaml", ref Toolbar, ref mnuMain);
        //    foreach (var item in mnuMain.Items)
        //    {
        //        if (item is MenuItem)
        //            ((MenuItem)item).Click += btnShortcutKey_Click;
        //    }
        //    BindShortkey();
        //    cmbLoaiTK.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiTK_SelectionChanged);
        //    cmbLoaiKHNBo.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiKHNBo_SelectionChanged);
        //    cmbTinhChatTK.SelectionChanged += new SelectionChangedEventHandler(cmbTinhChatTK_SelectionChanged);
        //    raddtTuNgayApDung.Value = LDateTime.StringToDate(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
        //    lblTenPLTKCapTren.Content = "";
        //    txtMaPLTKCapTren.KeyDown += new KeyEventHandler(txtMaPLTKCapTren_KeyDown);
        //    InitCombobox();
        //    txtMaPLTKCapTren.Focus();
        //}



        /// <summary>
        /// Khoi tao combobox
        /// </summary>
        private void InitCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDK = new List<string>();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                //Loại tài khoản
                lstDK.Clear();
                lstDK.Add(DatabaseConstant.DanhMuc.LOAI_TAI_KHOAN_PVI.getValue());
                auto.GenAutoComboBox(ref lstSourceLoaiTK, ref cmbLoaiTK, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK,"NOI_BANG");

                //Loại khách hàng/ nội bộ
                lstDK.Clear();
                lstDK.Add(DatabaseConstant.DanhMuc.LOAI_TAI_KHOAN_DTUONG.getValue());
                auto.GenAutoComboBox(ref lstSourceLoaiKHNB, ref cmbLoaiKHNBo, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK,"NOI_BO");

                //Loại khách hàng/ nội bộ
                lstDK.Clear();
                lstDK.Add(DatabaseConstant.DanhMuc.CO_KHONG.getValue());
                auto.GenAutoComboBox(ref lstSourceTChatBTru, ref cmbTinhChatBTru, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK, "CO");

                //Loại thu nhập/ chi phí
                lstDK.Clear();
                lstDK.Add(DatabaseConstant.DanhMuc.LOAI_TAI_KHOAN_MDSD.getValue());
                auto.GenAutoComboBox(ref lstSourceThuNhap, ref cmbLoaiTNCP, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK,"KHAC");

                //Tính chất tài khoản
                auto.GenAutoComboBox(ref lstSourceTinhChatTK, ref cmbTinhChatTK, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TINH_CHAT_TK.getValue(),null);

                List<string> lstChon = new List<string>();
                lstChon.Add("NO");
                lstChon.Add("CO");
                //Tính chất tài khoản
                auto.GenAutoComboBox(ref lstSourceTChatGoc, ref cmbTinhChatGocTK, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TINH_CHAT_TK.getValue(), null, "NO", lstChon);

                //Tính chất tài khoản
                auto.GenAutoComboBox(ref lstSourceKyHieu, ref cmbKyHieu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KY_HIEU.getValue(), null);
                cmbKyHieu.SelectedIndex = -1;
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        #endregion

        #region Dang ky hot key, shortcut key

        private void BindShortkey()
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.H, ModifierKeys.Control);
                        key = new KeyBinding(HoldCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(SaveCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(ModifyCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.Shift);
                        key = new KeyBinding(DeleteCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.A, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ApproveCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(RefuseCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.C, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(CancelCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(HelpCommand, keyg);
                    }
                    if (key != null)
                    {
                        InputBindings.Add(key);
                    }
                }
            }
        }

        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbAdd.IsEnabled;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ResetForm();
        }

        private void HoldCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbHold.IsEnabled;
        }
        private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onHold();
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbSubmit.IsEnabled;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onSave();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbModify.IsEnabled;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeModifyFromDetail();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbDelete.IsEnabled;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeDelete();
        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbApprove.IsEnabled;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onApprove();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbRefuse.IsEnabled;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onRefuse();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbCancel.IsEnabled;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onCancel();
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
            onClose();
        }

        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetForm();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                onHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                onSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                beforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                beforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                beforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                beforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                beforeRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                onClose();
            }
        }

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetForm();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                onHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                onSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                beforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                beforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                beforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                beforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                beforeRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                onClose();
            }
        }

        #endregion 

        #region Xu ly Giao dien

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
        /// Trợ giúp
        /// </summary>
        private void onHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }

        /// <summary>
        /// Đóng form
        /// </summary>
        private void onClose()
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
            listLockId.Add(_idPhanLoai);

            bool ret = process.UnlockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        private void cmbLoaiTK_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoComboBox au = new AutoComboBox();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                AutoCompleteEntry auLoaiTK = au.getEntryByDisplayName(lstSourceLoaiTK, ref cmbLoaiTK);
                if (auLoaiTK != null)
                {
                    if (auLoaiTK.KeywordStrings[0] == ApplicationConstant.LoaiTaiKhoan.NOI_BANG.layGiaTri())
                    {
                        cmbLoaiKHNBo.IsEnabled = true;
                        cmbLoaiKHNBo.SelectedIndex = 0;
                        lblRedLoaiKHNBo.Visibility = Visibility.Visible;
                        cmbLoaiKHNBo.SelectedIndex = lstSourceLoaiKHNB.IndexOf(lstSourceLoaiKHNB.FirstOrDefault(f => f.KeywordStrings.First().Equals(ApplicationConstant.LoaiTaiKhoanTheoDoiTuong.NOI_BO.layGiaTri())));
                    }
                    else
                    {
                        cmbLoaiKHNBo.IsEnabled = false;
                        cmbLoaiKHNBo.SelectedIndex = -1;
                        lblRedLoaiKHNBo.Visibility = Visibility.Hidden;

                        cmbLoaiTNCP.IsEnabled = false;
                        cmbLoaiTNCP.SelectedIndex = -1;
                        lblRedLoaiTNChiPhi.Visibility = Visibility.Hidden;
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                au = null;
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void cmbLoaiKHNBo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoComboBox au = new AutoComboBox();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                AutoCompleteEntry auLoaiKHNBo = au.getEntryByDisplayName(lstSourceLoaiKHNB, ref cmbLoaiKHNBo);
                if (auLoaiKHNBo != null)
                {
                    if (auLoaiKHNBo.KeywordStrings[0] == ApplicationConstant.LoaiTaiKhoanTheoDoiTuong.KHACH_HANG.layGiaTri())
                    {
                        cmbLoaiTNCP.IsEnabled = false;
                        cmbLoaiTNCP.SelectedIndex = -1;
                        lblRedLoaiTNChiPhi.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        if (updateTNCP == null)
                        {
                            cmbLoaiTNCP.IsEnabled = true;
                            cmbLoaiTNCP.SelectedIndex = 0;
                            lblRedLoaiTNChiPhi.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            if (cmbLoaiTNCP.Tag == null)
                            {
                                cmbLoaiTNCP.SelectedIndex = 0;
                            }
                            else
                            {
                                cmbLoaiTNCP.SelectedIndex = Convert.ToInt32(cmbLoaiTNCP.Tag);
                            }

                            if (updateTNCP == true)
                            {
                                cmbLoaiTNCP.IsEnabled = true;
                                lblRedLoaiTNChiPhi.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                cmbLoaiTNCP.IsEnabled = false;
                                lblRedLoaiTNChiPhi.Visibility = Visibility.Visible;
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                au = null;
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void SetEnabledAllControls(bool enable)
        {
            grbThongTin.IsEnabled = enable;
        }


        void cmbTinhChatTK_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoCompleteEntry au = lstSourceTinhChatTK.ElementAt(cmbTinhChatTK.SelectedIndex);
            if (au.KeywordStrings.FirstOrDefault().Equals("LT"))
            {
                stpTinhChatGocTK.Visibility = System.Windows.Visibility.Visible;
                cmbTinhChatGocTK.Visibility = System.Windows.Visibility.Visible;

                stpTinhChatBTru.Visibility = System.Windows.Visibility.Visible;
                cmbTinhChatBTru.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                stpTinhChatGocTK.Visibility = System.Windows.Visibility.Collapsed;
                cmbTinhChatGocTK.Visibility = System.Windows.Visibility.Collapsed;

                stpTinhChatBTru.Visibility = System.Windows.Visibility.Collapsed;
                cmbTinhChatBTru.Visibility = System.Windows.Visibility.Collapsed;
                cmbTinhChatGocTK.SelectedIndex = lstSourceTChatGoc.IndexOf(lstSourceTChatGoc.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(au.KeywordStrings.FirstOrDefault())));
            }
        }
        
        void txtMaPLTKCapTren_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
                btnMaPLTKCapTren_Click(sender, null);
        }
        #endregion              
 
        #region Xu ly nghiep vu
        private void beforeView()
        {
            SetFormData();
            SetEnabledAllControls(false);
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu,mnuMain,DatabaseConstant.Function.KT_PHAN_LOAI_CT);
        }

        private void beforeAddNew()
        {
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_PHAN_LOAI_CT);
            lblTrangThai.Content = "";
            raddtNgayNhap.Value = null;
            raddtNgayCNhat.Value = null;
            lblTenPLTKCapTren.Content = "";
        }

        private void beforeModifyFromList(DatabaseConstant.Action action)
        {
            if (action == DatabaseConstant.Action.XEM)
            {
                SetEnabledAllControls(false);
            }
            CommonFunction.RefreshButton(Toolbar, action, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_PHAN_LOAI_CT);
        }

        private void beforeModifyFromDetail()
        {
            // Yêu cầu lock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idPhanLoai);

            bool ret = process.LockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Nếu lock thành công >> cho phép sửa
            if (ret)
            {
                SetEnabledAllControls(true);
                SetFormData();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_PHAN_LOAI_CT);
            }
            else
            {
                LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
            }
        }

        private void beforeDelete()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_idPhanLoai);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.GDKT,
                    DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                    DatabaseConstant.Table.KT_PLOAI,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    // Gọi tới hàm xóa dữ liệu
                    onDelete();
                    return;
                }
                // Nếu lock không thành công >> cảnh báo
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                    return;
                }
            }
            else
            {
                // Unlock dữ liệu
                // (Hiện tại không phải Unlock dữ liệu, vì chỉ được xóa khi chọn sửa - sẽ Unlock sau)
                return;
            }
        }

        private void beforeApprove()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_idPhanLoai);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.GDKT,
                    DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                    DatabaseConstant.Table.KT_PLOAI,
                    DatabaseConstant.Action.DUYET,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    TTHAI_LY_DO objTThai = new TTHAI_LY_DO();
                    objTThai.ID = _idPhanLoai;
                    objTThai.MA = txtMaPLTK.Text;
                    objTThai.TEN = txtTenPLTK.Text;
                    lstLyDo.Add(objTThai);
                    ucLyDo lydo = new ucLyDo(lstLyDo);
                    lydo.DuLieuTraVe = new ucLyDo.LayDuLieu(LayDuLieuLyDo);
                    Window win = new Window();
                    //win.Title = "Danh sách mã phân loại tài khoản";
                    win.Content = lydo;
                    win.Title = LLanguage.SearchResourceByKey("U.KeToan.KiemSoat.frmLyDo.LyDo") + "-" + LLanguage.SearchResourceByKey("U.DungChung.Button.Duyet");
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    win.ShowDialog();
                    // Gọi tới hàm duyệt dữ liệu
                    onApprove();
                    return;
                }
                // Nếu lock không thành công >> cảnh báo
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                    return;
                }
            }
            else
            {
                // Unlock dữ liệu
                // (Hiện tại không phải Unlock dữ liệu, vì chỉ được xóa khi chọn sửa - sẽ Unlock sau)
                return;
            }
        }

        private void beforeCancel()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_idPhanLoai);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.GDKT,
                    DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                    DatabaseConstant.Table.KT_PLOAI,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    TTHAI_LY_DO objTThai = new TTHAI_LY_DO();
                    objTThai.ID = _idPhanLoai;
                    objTThai.MA = txtMaPLTK.Text;
                    objTThai.TEN = txtTenPLTK.Text;
                    lstLyDo.Add(objTThai);
                    ucLyDo lydo = new ucLyDo(lstLyDo);
                    lydo.DuLieuTraVe = new ucLyDo.LayDuLieu(LayDuLieuLyDo);
                    Window win = new Window();
                    //win.Title = "Danh sách mã phân loại tài khoản";
                    win.Content = lydo;
                    win.Title = LLanguage.SearchResourceByKey("U.KeToan.KiemSoat.frmLyDo.LyDo") + "-" + LLanguage.SearchResourceByKey("U.DungChung.Button.ThoaiDuyet");
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    win.ShowDialog();
                    // Gọi tới hàm thoái duyệt dữ liệu
                    onCancel();
                    return;
                }
                // Nếu lock không thành công >> cảnh báo
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                    return;
                }
            }
            else
            {
                // Unlock dữ liệu
                // (Hiện tại không phải Unlock dữ liệu, vì chỉ được xóa khi chọn sửa - sẽ Unlock sau)
                return;
            }
        }

        /// <summary>
        /// Trước khi từ chối
        /// </summary>
        private void beforeRefuse()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_idPhanLoai);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.GDKT,
                    DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                    DatabaseConstant.Table.KT_PLOAI,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    TTHAI_LY_DO objTThai = new TTHAI_LY_DO();
                    objTThai.ID = _idPhanLoai;
                    objTThai.MA = txtMaPLTK.Text;
                    objTThai.TEN = txtTenPLTK.Text;
                    lstLyDo.Add(objTThai);
                    ucLyDo lydo = new ucLyDo(lstLyDo);
                    lydo.DuLieuTraVe = new ucLyDo.LayDuLieu(LayDuLieuLyDo);
                    Window win = new Window();
                    //win.Title = "Danh sách mã phân loại tài khoản";
                    win.Content = lydo;
                    win.Title = LLanguage.SearchResourceByKey("U.KeToan.KiemSoat.frmLyDo.LyDo") + "-" + LLanguage.SearchResourceByKey("U.DungChung.Button.TuChoi");
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    win.ShowDialog();
                    // Gọi tới hàm xóa dữ liệu
                    onRefuse();
                    return;
                }
                // Nếu lock không thành công >> cảnh báo
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                    return;
                }
            }
            else
            {
                // Unlock dữ liệu
                // (Hiện tại không phải Unlock dữ liệu, vì chỉ được xóa khi chọn sửa - sẽ Unlock sau)
                return;
            }
        }

        /// <summary>
        /// Lưu dữ liệu (hay trình duyệt)
        /// </summary>
        private void onSave()
        {
            string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.TRINH_DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            if (Validation())
            {
                KeToanProcess process = new KeToanProcess();
                try
                {
                    Presentation.Process.KeToanServiceRef.KT_PLOAI objPLoai = new Presentation.Process.KeToanServiceRef.KT_PLOAI();
                    Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI objKHPLoai = new Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI();
                    // Dữ liệu truyền vào và dữ liệu trả về
                    ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                    GetFormData(ref objPLoai, ref objKHPLoai, trangThai);
                    Mouse.OverrideCursor = Cursors.Wait;
                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (_idPhanLoai == -1)
                    {
                        // Lấy dữ liệu từ form
                        ret = process.PhanLoaiChiTiet(DatabaseConstant.Action.THEM, ref objPLoai, ref objKHPLoai, ref listResponseDetail);
                        Mouse.OverrideCursor = Cursors.Arrow;
                        afterAddNew(ret, objPLoai, objKHPLoai, listResponseDetail);
                    }
                    // Nếu là lưu tạm khi sửa
                    // Hoặc lưu tạm khi sửa sau duyệt
                    // Hoặc sửa
                    else
                    {
                        // Lấy dữ liệu từ form
                        ret = process.PhanLoaiChiTiet(DatabaseConstant.Action.SUA, ref objPLoai, ref objKHPLoai, ref listResponseDetail);
                        Mouse.OverrideCursor = Cursors.Arrow;
                        afterModify(ret, objPLoai, listResponseDetail);
                    }
                }
                catch (System.Exception ex)
                {
                    this.Cursor = Cursors.Arrow;
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                finally
                {
                    process = null;
                }
            }
        }

        /// <summary>
        /// Lưu tạm dữ liệu
        /// </summary>
        private void onHold()
        {
            string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            KeToanProcess process = new KeToanProcess();
            try
            {
                Presentation.Process.KeToanServiceRef.KT_PLOAI objPLoai = new Presentation.Process.KeToanServiceRef.KT_PLOAI();
                Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI objKHPLoai = new Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI();
                // Dữ liệu truyền vào và dữ liệu trả về
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                GetFormData(ref objPLoai, ref objKHPLoai, trangThai);
                Mouse.OverrideCursor = Cursors.Wait;
                // Nếu là lưu tạm hoặc thêm mới lần đầu
                if (_idPhanLoai == -1)
                {
                    // Lấy dữ liệu từ form
                    ret = process.PhanLoaiChiTiet(DatabaseConstant.Action.THEM, ref objPLoai, ref objKHPLoai, ref listResponseDetail);
                    Mouse.OverrideCursor = Cursors.Arrow;
                    afterAddNew(ret, objPLoai, objKHPLoai, listResponseDetail);
                }
                // Nếu là lưu tạm khi sửa
                // Hoặc lưu tạm khi sửa sau duyệt
                // Hoặc sửa
                else
                {
                    // Lấy dữ liệu từ form
                    ret = process.PhanLoaiChiTiet(DatabaseConstant.Action.SUA, ref objPLoai, ref objKHPLoai, ref listResponseDetail);
                    Mouse.OverrideCursor = Cursors.Arrow;
                    afterModify(ret, objPLoai, listResponseDetail);
                }
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }
        }

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        private void onDelete()
        {
            string trangThai = "";
            KeToanProcess process = new KeToanProcess();
            try
            {
                Presentation.Process.KeToanServiceRef.KT_PLOAI objPLoai = new Presentation.Process.KeToanServiceRef.KT_PLOAI();
                Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI objKHPLoai = new Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI();
                // Dữ liệu truyền vào và dữ liệu trả về
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                GetFormData(ref objPLoai, ref objKHPLoai, trangThai);
                Mouse.OverrideCursor = Cursors.Wait;

                ret = process.PhanLoaiChiTiet(DatabaseConstant.Action.XOA, ref objPLoai, ref objKHPLoai, ref listResponseDetail);
                Mouse.OverrideCursor = Cursors.Arrow;
                afterDelete(ret, listResponseDetail);
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }
        }

        /// <summary>
        /// Duyệt dữ liệu
        /// </summary>
        private void onApprove()
        {
            string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            KeToanProcess process = new KeToanProcess();
            try
            {
                Presentation.Process.KeToanServiceRef.KT_PLOAI objPLoai = new Presentation.Process.KeToanServiceRef.KT_PLOAI();
                Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI objKHPLoai = new Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI();
                // Dữ liệu truyền vào và dữ liệu trả về
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                GetFormData(ref objPLoai, ref objKHPLoai, trangThai);
                Mouse.OverrideCursor = Cursors.Wait;

                ret = process.PhanLoaiChiTiet(DatabaseConstant.Action.DUYET, ref objPLoai, ref objKHPLoai, ref listResponseDetail);
                Mouse.OverrideCursor = Cursors.Arrow;
                afterApprove(ret, objPLoai, listResponseDetail);
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }
        }

        /// <summary>
        /// Thoái duyệt dữ liệu
        /// </summary>
        private void onCancel()
        {
            string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.THOAI_DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            KeToanProcess process = new KeToanProcess();
            try
            {
                Presentation.Process.KeToanServiceRef.KT_PLOAI objPLoai = new Presentation.Process.KeToanServiceRef.KT_PLOAI();
                Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI objKHPLoai = new Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI();
                // Dữ liệu truyền vào và dữ liệu trả về
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                GetFormData(ref objPLoai, ref objKHPLoai, trangThai);
                Mouse.OverrideCursor = Cursors.Wait;

                ret = process.PhanLoaiChiTiet(DatabaseConstant.Action.THOAI_DUYET, ref objPLoai, ref objKHPLoai, ref listResponseDetail);
                Mouse.OverrideCursor = Cursors.Arrow;
                afterCancel(ret, objPLoai, listResponseDetail);

            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }
        }

        /// <summary>
        /// Từ chối duyệt dữ liệu
        /// </summary>
        private void onRefuse()
        {
            string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.TU_CHOI_DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            KeToanProcess process = new KeToanProcess();
            try
            {
                Presentation.Process.KeToanServiceRef.KT_PLOAI objPLoai = new Presentation.Process.KeToanServiceRef.KT_PLOAI();
                Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI objKHPLoai = new Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI();
                // Dữ liệu truyền vào và dữ liệu trả về
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                GetFormData(ref objPLoai, ref objKHPLoai, trangThai);
                Mouse.OverrideCursor = Cursors.Wait;

                ret = process.PhanLoaiChiTiet(DatabaseConstant.Action.TU_CHOI_DUYET, ref objPLoai, ref objKHPLoai, ref listResponseDetail);
                Mouse.OverrideCursor = Cursors.Arrow;
                afterRefuse(ret, objPLoai, listResponseDetail);

            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }
        }

        /// <summary>
        /// Sau khi thêm mới
        /// </summary>
        /// <param name="ret"></param>
        private void afterAddNew(ApplicationConstant.ResponseStatus kq, Presentation.Process.KeToanServiceRef.KT_PLOAI objPLoai, Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI objKHPLoai, List<ClientResponseDetail> listResponseDetail)
        {
            if (kq == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);
                SetEnabledAllControls(false);
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(objPLoai.TTHAI_NVU);
                txtNguoiLap.Text = objPLoai.NGUOI_NHAP;
                raddtNgayNhap.Value = LDateTime.StringToDate(objPLoai.NGAY_NHAP, "yyyyMMdd");
                txtTrangThai.Text = lblTrangThai.Content.ToString();
                tthaiNvu = objPLoai.TTHAI_NVU;
                _idPhanLoai = objPLoai.ID;
                _idKyHieuPLoai = objKHPLoai.ID;

                if (cbMultiAdd.IsChecked == true)
                {
                    SetEnabledAllControls(true);
                    ResetForm();
                }
                else if (!DatabaseConstant.CLOSE_DETAIL_FORM)
                {
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_PHAN_LOAI_CT);
                }
                else
                {
                    onClose();
                }
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listResponseDetail);
            }
        }

        /// <summary>
        /// Sau khi sửa
        /// </summary>
        /// <param name="ret"></param>
        private void afterModify(ApplicationConstant.ResponseStatus kq, Presentation.Process.KeToanServiceRef.KT_PLOAI ret, List<ClientResponseDetail> listResponseDetail)
        {
            if (kq == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);
                tthaiNvu = ret.TTHAI_NVU;
                SetEnabledAllControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_PHAN_LOAI_CT);
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                txtNguoiCapNhat.Text = ret.NGUOI_CNHAT;
                raddtNgayCNhat.Value = LDateTime.StringToDate(ret.NGAY_CNHAT, "yyyyMMdd");
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listResponseDetail);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(ret.ID);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Sau khi xóa
        /// </summary>
        /// <param name="ret"></param>
        private void afterDelete(ApplicationConstant.ResponseStatus kq, List<ClientResponseDetail> listResponseDetail)
        {
            if (kq == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listResponseDetail);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idPhanLoai);
            List<int> listUnlockId = new List<int>();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Đóng cửa sổ chi tiết sau khi xóa
            onClose();
        }

        /// <summary>
        /// Sau khi duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void afterApprove(ApplicationConstant.ResponseStatus kq, Presentation.Process.KeToanServiceRef.KT_PLOAI ret, List<ClientResponseDetail> listResponseDetail)
        {
            if (kq == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);
                tthaiNvu = ret.TTHAI_NVU;
                SetEnabledAllControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_PHAN_LOAI_CT);
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                txtNguoiCapNhat.Text = ret.NGUOI_CNHAT;
                raddtNgayCNhat.Value = LDateTime.StringToDate(ret.NGAY_CNHAT, "yyyyMMdd");
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listResponseDetail);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(ret.ID);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Sau khi thoái duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void afterCancel(ApplicationConstant.ResponseStatus kq, Presentation.Process.KeToanServiceRef.KT_PLOAI ret, List<ClientResponseDetail> listResponseDetail)
        {
            if (kq == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);
                tthaiNvu = ret.TTHAI_NVU;
                SetEnabledAllControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_PHAN_LOAI_CT);
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                txtNguoiCapNhat.Text = ret.NGUOI_CNHAT;
                raddtNgayCNhat.Value = LDateTime.StringToDate(ret.NGAY_CNHAT, "yyyyMMdd");
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listResponseDetail);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(ret.ID);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Sau khi từ chối
        /// </summary>
        /// <param name="ret"></param>
        private void afterRefuse(ApplicationConstant.ResponseStatus kq, Presentation.Process.KeToanServiceRef.KT_PLOAI ret, List<ClientResponseDetail> listResponseDetail)
        {
            if (kq == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);
                tthaiNvu = ret.TTHAI_NVU;
                SetEnabledAllControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_PHAN_LOAI_CT);
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                txtNguoiCapNhat.Text = ret.NGUOI_CNHAT;
                raddtNgayCNhat.Value = LDateTime.StringToDate(ret.NGAY_CNHAT, "yyyyMMdd");
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listResponseDetail);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(ret.ID);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Kiểm tra dữ liệu trước khi lưu
        /// </summary>
        /// <returns></returns>
        private bool Validation()
        {
            AutoComboBox au = new AutoComboBox();
            try
            {
                AutoCompleteEntry auTinhChat = au.getEntryByDisplayName(lstSourceTinhChatTK, ref cmbTinhChatTK);
                AutoCompleteEntry auKyHieu = au.getEntryByDisplayName(lstSourceKyHieu, ref cmbKyHieu);

                if (LString.IsNullOrEmptyOrSpace(txtMaPLTK.Text))
                {
                    LMessage.ShowMessage("M.KeToan.PhanLoai.ucPhanLoaiCT.LoiMaPhanLoaiTrong", LMessage.MessageBoxType.Warning);
                    txtMaPLTK.Focus();
                    return false;
                }
                else if (auKyHieu == null)
                {
                    LMessage.ShowMessage("M.KeToan.PhanLoai.ucPhanLoaiCT.LoiNhomTaiKhoanTrong", LMessage.MessageBoxType.Warning);
                    cmbKyHieu.Focus();
                    return false;
                }
                else if (LString.IsNullOrEmptyOrSpace(txtTenPLTK.Text))
                {
                    LMessage.ShowMessage("M.KeToan.PhanLoai.ucPhanLoaiCT.LoiTenPhanLoaiTKTrong", LMessage.MessageBoxType.Warning);
                    txtTenPLTK.Focus();
                    return false;
                }
                else if (raddtTuNgayApDung.Value == null)
                {
                    LMessage.ShowMessage("M.KeToan.PhanLoai.ucPhanLoaiCT.LoiNgayApDungTrong", LMessage.MessageBoxType.Warning);
                    raddtTuNgayApDung.Focus();
                    return false;
                }
                else if (auTinhChat == null)
                {
                    LMessage.ShowMessage("M.KeToan.PhanLoai.ucPhanLoaiCT.LoiTinhChatTaiKhoanTrong", LMessage.MessageBoxType.Warning);
                    cmbTinhChatTK.Focus();
                    return false;
                }
                else if (_idPhanLoai == -1 && Convert.ToDateTime(raddtTuNgayApDung.Value).ToString("yyyyMMdd").CompareTo(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai) < 0)
                {
                    LMessage.ShowMessage("M.KeToan.PhanLoai.ucPhanLoaiCT.LoiNgayApDungNhoHonNgayHienTai", LMessage.MessageBoxType.Warning);
                    raddtTuNgayApDung.Focus();
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                au = null;
            }
            return true;
        }

        private void SetFormData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            KeToanProcess process = new KeToanProcess();
            AutoComboBox au = new AutoComboBox();
            try
            {
                DataTable dt = process.getThongTinMaPhanLoaiTheoID(_idPhanLoai);
                if (dt != null && dt.Rows.Count > 0)
                {

                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);

                    //Thông tin tài khoản
                    txtMaPLTKCapTren.Text = dt.Rows[0]["MA_PLOAI_CHA"].ToString();
                    txtMaPLTKCapTren_LostFocus(null, null);
                    txtMaPLTK.Text = dt.Rows[0]["MA_PLOAI"].ToString();
                    txtTenPLTK.Text = dt.Rows[0]["TEN_PLOAI"].ToString();
                    if (dt.Rows[0]["ID_KHPL"] != DBNull.Value)
                    {
                        _idKyHieuPLoai = Convert.ToInt32(dt.Rows[0]["ID_KHPL"]);
                    }
                    else
                    {
                        _idKyHieuPLoai = -1;
                    }
                    cmbKyHieu.SelectedIndex = lstSourceKyHieu.IndexOf(lstSourceKyHieu.FirstOrDefault(e => e.KeywordStrings.First().Equals(dt.Rows[0]["MA_KY_HIEU"].ToString())));
                    raddtTuNgayApDung.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_ADUNG"].ToString(), "yyyyMMdd");
                    cmbTinhChatTK.SelectedIndex = lstSourceTinhChatTK.IndexOf(lstSourceTinhChatTK.FirstOrDefault(e => e.KeywordStrings.First().Equals(dt.Rows[0]["MA_NHOM_PLOAI"].ToString())));
                    cmbTinhChatGocTK.SelectedIndex = lstSourceTChatGoc.IndexOf(lstSourceTChatGoc.FirstOrDefault(e => e.KeywordStrings.First().Equals(dt.Rows[0]["MA_TCHAT_GOC"].ToString())));
                    string tinhChatBTru = BusinessConstant.CoKhong.CO.layGiaTri();
                    if (dt.Rows[0]["MA_TCHAT_LTINH"].ToString().Equals("KOBT"))
                        tinhChatBTru = BusinessConstant.CoKhong.CO.layGiaTri();
                    if (dt.Rows[0]["MA_TCHAT_LTINH"] != DBNull.Value && dt.Rows[0]["MA_TCHAT_LTINH"].ToString().IsNullOrEmptyOrSpace())
                        cmbTinhChatBTru.SelectedIndex = lstSourceTChatBTru.IndexOf(lstSourceTChatGoc.FirstOrDefault(e => e.KeywordStrings.First().Equals(tinhChatBTru)));
                    if (dt.Rows[0]["MA_TCHAT_CNO"].ToString().Equals("CO"))
                    {
                        chkTheoDoiCongNo.IsChecked = true;
                    }
                    else
                    {
                        chkTheoDoiCongNo.IsChecked = false;
                    }

                    if (dt.Rows[0]["MA_NHOM_PLOAI"] != null && !LString.IsNullOrEmptyOrSpace(dt.Rows[0]["MA_NHOM_PLOAI"].ToString()))
                    {
                        cmbTinhChatTK.SelectedIndex = lstSourceTinhChatTK.IndexOf(lstSourceTinhChatTK.FirstOrDefault(e => e.KeywordStrings.First().Equals(dt.Rows[0]["MA_NHOM_PLOAI"].ToString())));
                    }

                    //Thông tin kiểm soát
                    txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(dt.Rows[0]["TTHAI_BGHI"].ToString());
                    raddtNgayNhap.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_NHAP"].ToString(), "yyyyMMdd");
                    txtNguoiLap.Text = dt.Rows[0]["NGUOI_NHAP"].ToString();
                    if (LDateTime.IsDate(dt.Rows[0]["NGAY_CNHAT"].ToString(), "yyyyMMdd") == true)
                        raddtNgayCNhat.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_CNHAT"].ToString(), "yyyyMMdd");
                    else
                        raddtNgayCNhat.Value = null;
                    txtNguoiCapNhat.Text = dt.Rows[0]["NGUOI_CNHAT"].ToString();
                    
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void GetFormData(ref Presentation.Process.KeToanServiceRef.KT_PLOAI objPLoai, ref Presentation.Process.KeToanServiceRef.KT_KY_HIEU_PLOAI objKHPLoai, string tthai)
        {
            AutoComboBox au = new AutoComboBox();
            AutoCompleteEntry auLoaiTK = au.getEntryByDisplayName(lstSourceLoaiTK, ref cmbLoaiTK);
            AutoCompleteEntry auLoaiKHNB = au.getEntryByDisplayName(lstSourceLoaiKHNB, ref cmbLoaiKHNBo);
            AutoCompleteEntry auThuNhap = au.getEntryByDisplayName(lstSourceThuNhap, ref cmbLoaiTNCP);
            AutoCompleteEntry auTinhChat = au.getEntryByDisplayName(lstSourceTinhChatTK,ref cmbTinhChatTK);
            AutoCompleteEntry auKyHieu = au.getEntryByDisplayName(lstSourceKyHieu, ref cmbKyHieu);
            AutoCompleteEntry auTinhChatGoc = au.getEntryByDisplayName(lstSourceTChatGoc, ref cmbTinhChatGocTK);
            AutoCompleteEntry auTinhChatBTru = au.getEntryByDisplayName(lstSourceTChatBTru, ref cmbTinhChatBTru);

            #region KT_PLOAI
            if (_idPhanLoai != -1)
            {
                objPLoai.ID = _idPhanLoai;
            }

            if (!LString.IsNullOrEmptyOrSpace(txtMaPLTKCapTren.Text))
            {
                objPLoai.ID_PLOAI_CHA = Convert.ToInt32(txtMaPLTKCapTren.Tag);
                objPLoai.MA_PLOAI_CHA = txtMaPLTKCapTren.Text;
            }
            else 
            {
                objPLoai.MA_PLOAI_CHA = "";
            }
            objPLoai.MA_PLOAI = txtMaPLTK.Text;
            objPLoai.ID_NHOM_PLOAI = Convert.ToInt32(auTinhChat.KeywordStrings[1]);
            objPLoai.MA_NHOM_PLOAI = auTinhChat.KeywordStrings[0];
            objPLoai.TEN_PLOAI = txtTenPLTK.Text;
            objPLoai.NGAY_ADUNG = LDateTime.DateToString(Convert.ToDateTime(raddtTuNgayApDung.Value), "yyyyMMdd");
            objPLoai.MA_NOI_NGOAI = auLoaiTK.KeywordStrings[0];
            objPLoai.MA_TCHAT_GOC = auTinhChatGoc.KeywordStrings[0];
            objPLoai.MA_TCHAT_LTINH = "";
            if(auTinhChat.KeywordStrings[0].Equals("LT"))
            {
                if(auTinhChatBTru.KeywordStrings[0].Equals(BusinessConstant.CoKhong.CO.layGiaTri()))
                    objPLoai.MA_TCHAT_LTINH = "COBT";
                else
                    objPLoai.MA_TCHAT_LTINH = "KOBT";
            }
            if (auLoaiKHNB != null)
            {
                objPLoai.MA_KHANG_NBO = auLoaiKHNB.KeywordStrings[0];
            }
            if (auThuNhap != null)
            {
                objPLoai.MA_TNHAP_CPHI = auThuNhap.KeywordStrings[0];
            }
            if (chkTheoDoiCongNo.IsChecked == true)
            {
                objPLoai.MA_TCHAT_CNO = "CO";
            }
            else
            {
                objPLoai.MA_TCHAT_CNO = "KHONG";
            }
            
            objPLoai.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
            objPLoai.TTHAI_NVU = tthai;
            objPLoai.MA_DVI_QLY = Presentation.Process.Common.ClientInformation.MaDonVi;
            objPLoai.MA_DVI_TAO = Presentation.Process.Common.ClientInformation.MaDonViGiaoDich;
            if (_idPhanLoai == -1)
            {
                objPLoai.NGAY_NHAP = Presentation.Process.Common.ClientInformation.NgayLamViecHienTai;
                objPLoai.NGUOI_NHAP = Presentation.Process.Common.ClientInformation.TenDangNhap;
            }
            else
            {
                objPLoai.NGAY_NHAP = Convert.ToDateTime(raddtNgayNhap.Value).ToString("yyyyMMdd");
                objPLoai.NGUOI_NHAP = txtNguoiLap.Text;
                objPLoai.NGAY_CNHAT = Presentation.Process.Common.ClientInformation.NgayLamViecHienTai;
                objPLoai.NGUOI_CNHAT = Presentation.Process.Common.ClientInformation.TenDangNhap;
            }
            #endregion

            #region KT_KY_HIEU_PLOAI
            if (_idKyHieuPLoai != -1)
                objKHPLoai.ID = _idKyHieuPLoai;
            objKHPLoai.ID_KY_HIEU = Convert.ToInt32(auKyHieu.KeywordStrings[1]);
            objKHPLoai.ID_PLOAI = objPLoai.ID;
            objKHPLoai.MA_KY_HIEU = auKyHieu.KeywordStrings[0];
            objKHPLoai.MA_PLOAI = objPLoai.MA_PLOAI;
            objKHPLoai.TTHAI_BGHI = objPLoai.TTHAI_BGHI;
            objKHPLoai.TTHAI_NVU = objPLoai.TTHAI_NVU;
            objKHPLoai.MA_DVI_QLY = objPLoai.MA_DVI_QLY;
            objKHPLoai.MA_DVI_TAO = objPLoai.MA_DVI_TAO;
            objKHPLoai.NGAY_NHAP = objPLoai.NGAY_NHAP;
            objKHPLoai.NGUOI_NHAP = objPLoai.NGUOI_NHAP;
            objKHPLoai.NGAY_CNHAT = objPLoai.NGAY_CNHAT;
            objKHPLoai.NGUOI_CNHAT = objPLoai.NGUOI_CNHAT;
            #endregion
        }

        private void ResetForm()
        {
            txtMaPLTKCapTren.Text = "";
            txtMaPLTKCapTren.Tag = "";
            lblTenPLTKCapTren.Content = "";
            txtMaPLTK.Text = "";
            txtTenPLTK.Text = "";
            
            chkTheoDoiCongNo.IsEnabled = false;
            chkTheoDoiCongNo.IsChecked = true;

            raddtTuNgayApDung.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd"); ;

            _idPhanLoai = -1;
            _idKyHieuPLoai = -1;
            lblTrangThai.Content = "";
            tthaiNvu = "";
            txtTrangThai.Text = "";
            txtNguoiLap.Text = "";
            txtNguoiCapNhat.Text = "";
            raddtNgayNhap.Value = null;
            raddtNgayCNhat.Value = null;
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_PHAN_LOAI_CT);
            SetEnabledAllControls(true);
        }

        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtMaPLTKCapTren.Focus();
        }

        private bool KiemTraTonTaiTKChiTiet(string maPhanLoai)
        {
            Presentation.Process.KeToanProcess ketoanProcess = new Presentation.Process.KeToanProcess();
            DataSet ds = ketoanProcess.GetTaiKhoanByMaPLoai(maPhanLoai, ClientInformation.MaDonVi);
            if (ds != null && ds.Tables[0].Rows.Count == 0)
                return true;
            else
                return false;
        }

        private void btnMaPLTKCapTren_Click(object sender, RoutedEventArgs e)
        {
            Presentation.Process.KeToanProcess ketoanProcess = new Presentation.Process.KeToanProcess();
            try
            {
                //Bat popup
                var process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonVi);
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_KT_PLOAI.getValue(), lstDieuKien);                

                SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                //win.Title = "Danh sách mã phân loại tài khoản";
                win.Content = popup;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {                    
                    DataRow dr = lstPopup[0];
                    if (KiemTraTonTaiTKChiTiet(dr[2].ToString()) == false)
                    {
                        LMessage.ShowMessage("M.KeToan.PhanLoai.ucPhanLoaiCT.LoiTonTaiTKCT", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    txtMaPLTKCapTren.Tag = dr[1].ToString();
                    txtMaPLTKCapTren.Text = dr[2].ToString();
                    lblTenPLTKCapTren.Content = dr[3].ToString();
                    //Tao ma phan loai tai khoan
                    txtMaPLTK.Text = ketoanProcess.getMaPhanLoaiGoiY(txtMaPLTKCapTren.Text);
                    txtTenPLTK.Text = dr[3].ToString();
                    if (dr[5] != null && !LString.IsNullOrEmptyOrSpace(dr[5].ToString()))
                    {
                        cmbLoaiTK.SelectedIndex = lstSourceLoaiTK.IndexOf(lstSourceLoaiTK.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr[5].ToString())));
                        cmbLoaiTK.IsEnabled = false;
                    }

                    if (dr[6] != null && !LString.IsNullOrEmptyOrSpace(dr[6].ToString()))
                    {
                        cmbLoaiKHNBo.SelectedIndex = lstSourceLoaiKHNB.IndexOf(lstSourceLoaiKHNB.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr[6].ToString())));
                        //cmbLoaiKHNBo.IsEnabled = false;
                    }

                    AutoComboBox auto = new AutoComboBox();
                    AutoCompleteEntry auKHNBo = auto.getEntryByDisplayName(lstSourceLoaiKHNB, ref cmbLoaiKHNBo);
                    if (auKHNBo.KeywordStrings[0] == ApplicationConstant.LoaiTaiKhoanTheoDoiTuong.NOI_BO.layGiaTri())
                    {
                        if (dr[7] != null && !LString.IsNullOrEmptyOrSpace(dr[7].ToString()))
                        {
                            cmbLoaiTNCP.SelectedIndex = lstSourceThuNhap.IndexOf(lstSourceThuNhap.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr[7].ToString())));
                            cmbLoaiTNCP.Tag = cmbLoaiTNCP.SelectedIndex;
                            cmbLoaiTNCP.IsEnabled = false;
                            updateTNCP = false;
                        }
                    }
                    else
                    {
                        cmbLoaiTNCP.IsEnabled = false;
                        cmbLoaiTNCP.SelectedIndex = -1;
                        lblRedLoaiTNChiPhi.Visibility = Visibility.Hidden;
                        updateTNCP = false;
                        cmbLoaiTNCP.Tag = lstSourceThuNhap.IndexOf(lstSourceThuNhap.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr[7].ToString())));
                    }

                    if (!dr[8].ToString().IsNullOrEmptyOrSpace())
                    {
                        cmbKyHieu.SelectedIndex = lstSourceKyHieu.IndexOf(lstSourceKyHieu.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr[8].ToString())));
                    }
                    else
                    {
                        cmbKyHieu.SelectedIndex = -1;
                    }

                    if (!dr[10].ToString().IsNullOrEmptyOrSpace())
                    {
                        cmbTinhChatTK.SelectedIndex = lstSourceTinhChatTK.IndexOf(lstSourceTinhChatTK.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr[10].ToString())));
                    }
                    else
                    {
                        cmbTinhChatTK.SelectedIndex = -1;
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {

            }
        }

        private void txtMaPLTKCapTren_LostFocus(object sender, RoutedEventArgs e)
        {
            Presentation.Process.KeToanProcess ketoanProcess = new Presentation.Process.KeToanProcess();
            try
            {
                if (!LString.IsNullOrEmptyOrSpace(txtMaPLTKCapTren.Text))
                {
                    DataTable dt = ketoanProcess.getThongTinMaPhanLoaiTheoMa(txtMaPLTKCapTren.Text);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (KiemTraTonTaiTKChiTiet(dt.Rows[0]["MA_PLOAI"].ToString()) == false)
                        {
                            LMessage.ShowMessage("M.KeToan.PhanLoai.ucPhanLoaiCT.LoiTonTaiTKCT", LMessage.MessageBoxType.Warning);
                            return;
                        }
                        txtMaPLTKCapTren.Tag = dt.AsEnumerable().FirstOrDefault(f=>f.Field<string>("MA_DVI_QLY").Equals(ClientInformation.MaDonVi)).Field<int>("ID").ToString();
                        txtMaPLTKCapTren.Text = dt.AsEnumerable().FirstOrDefault(f => f.Field<string>("MA_DVI_QLY").Equals(ClientInformation.MaDonVi)).Field<string>("MA_PLOAI").ToString();
                        lblTenPLTKCapTren.Content = dt.AsEnumerable().FirstOrDefault(f => f.Field<string>("MA_DVI_QLY").Equals(ClientInformation.MaDonVi)).Field<string>("TEN_PLOAI").ToString();
                        //Kiem tra
                        txtMaPLTK.Text = ketoanProcess.getMaPhanLoaiGoiY(txtMaPLTKCapTren.Text);
                        txtTenPLTK.Text = dt.Rows[0]["TEN_PLOAI"].ToString();
                        if (!dt.Rows[0]["MA_KY_HIEU"].ToString().IsNullOrEmptyOrSpace())
                        {
                            cmbKyHieu.SelectedIndex = lstSourceKyHieu.IndexOf(lstSourceKyHieu.FirstOrDefault(f => f.KeywordStrings.First().Equals(dt.Rows[0]["MA_KY_HIEU"].ToString())));
                        }
                        else
                        {
                            cmbKyHieu.SelectedIndex = -1;
                        }
                        if (dt.Rows[0]["MA_NOI_NGOAI"] != null && !LString.IsNullOrEmptyOrSpace(dt.Rows[0]["MA_NOI_NGOAI"].ToString()))
                        {
                            cmbLoaiTK.SelectedIndex = lstSourceLoaiTK.IndexOf(lstSourceLoaiTK.FirstOrDefault(f => f.KeywordStrings.First().Equals(dt.Rows[0]["MA_NOI_NGOAI"].ToString())));
                            cmbLoaiTK.IsEnabled = false;
                        }

                        if (dt.Rows[0]["MA_KHANG_NBO"] != null && !LString.IsNullOrEmptyOrSpace(dt.Rows[0]["MA_KHANG_NBO"].ToString()))
                        {
                            cmbLoaiKHNBo.SelectedIndex = lstSourceLoaiKHNB.IndexOf(lstSourceLoaiKHNB.FirstOrDefault(f => f.KeywordStrings.First().Equals(dt.Rows[0]["MA_KHANG_NBO"].ToString())));
                            //cmbLoaiKHNBo.IsEnabled = false;
                        }

                        AutoComboBox auto = new AutoComboBox();
                        AutoCompleteEntry auKHNBo = auto.getEntryByDisplayName(lstSourceLoaiKHNB, ref cmbLoaiKHNBo);
                        if (auKHNBo.KeywordStrings[0] == ApplicationConstant.LoaiTaiKhoanTheoDoiTuong.NOI_BO.layGiaTri())
                        {
                            if (dt.Rows[0]["MA_TNHAP_CPHI"] != null && !LString.IsNullOrEmptyOrSpace(dt.Rows[0]["MA_TNHAP_CPHI"].ToString()))
                            {
                                cmbLoaiTNCP.SelectedIndex = lstSourceThuNhap.IndexOf(lstSourceThuNhap.FirstOrDefault(f => f.KeywordStrings.First().Equals(dt.Rows[0]["MA_TNHAP_CPHI"].ToString())));
                                cmbLoaiTNCP.Tag = cmbLoaiTNCP.SelectedIndex;
                                cmbLoaiTNCP.IsEnabled = false;
                                updateTNCP = false;
                            }
                        }
                        else
                        {
                            cmbLoaiTNCP.IsEnabled = false;
                            cmbLoaiTNCP.SelectedIndex = -1;
                            lblRedLoaiTNChiPhi.Visibility = Visibility.Hidden;
                            updateTNCP = false;
                            cmbLoaiTNCP.Tag = lstSourceThuNhap.IndexOf(lstSourceThuNhap.FirstOrDefault(f => f.KeywordStrings.First().Equals(dt.Rows[0]["MA_TNHAP_CPHI"].ToString())));
                        }
                    }
                    else
                    {
                        LMessage.ShowMessage("Không tồn tại mã phân loại tài khoản này", LMessage.MessageBoxType.Warning);
                        txtMaPLTKCapTren.Tag = "";
                        txtMaPLTKCapTren.Text = "";
                        lblTenPLTKCapTren.Content = "";
                        txtMaPLTK.Text = "";
                        updateTNCP = true;
                    }
                }
                else
                {
                    txtMaPLTKCapTren.Tag = "";
                    txtMaPLTKCapTren.Text = "";
                    lblTenPLTKCapTren.Content = "";
                    txtMaPLTK.Text = "";

                    cmbLoaiTK.SelectedIndex = lstSourceLoaiTK.IndexOf(lstSourceLoaiTK.FirstOrDefault(f => f.KeywordStrings.First().Equals("NOI_BANG")));
                    cmbLoaiTK.IsEnabled = true;


                    cmbLoaiKHNBo.SelectedIndex = lstSourceLoaiKHNB.IndexOf(lstSourceLoaiKHNB.FirstOrDefault(f => f.KeywordStrings.First().Equals("NOI_BO")));
                    cmbLoaiKHNBo.IsEnabled = true;


                    cmbLoaiTNCP.SelectedIndex = lstSourceThuNhap.IndexOf(lstSourceThuNhap.FirstOrDefault(f => f.KeywordStrings.First().Equals("KHAC")));
                    cmbLoaiTNCP.Tag = null;
                    cmbLoaiTNCP.IsEnabled = true;
                    updateTNCP = true;
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {

            }
        }
    }
}
