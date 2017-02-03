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
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using PresentationWPF.CustomControl;
using System.Data;
using Presentation.Process.LaiSuatServiceRef;
using Telerik.Windows.Data;
using System.Collections;
using System.Reflection;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls;

namespace PresentationWPF.LaiSuat.LaiSuat
{
    /// <summary>
    /// Interaction logic for ucLaiSuatCT.xaml
    /// </summary>
    public partial class ucLaiSuatCT : UserControl
    {
        #region Khai bao
        public static RoutedCommand AddCommand = new RoutedCommand();
        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();        
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        List<AutoCompleteEntry> lstSourcePhanHe = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLaiSuatDonViTinh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiLaiSuat = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHinhThucBacThang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLaiSuatKyHan = new List<AutoCompleteEntry>();

        public static DatabaseConstant.Module Module = DatabaseConstant.Module.DMDC;
        public static DatabaseConstant.Function Function = DatabaseConstant.Function.DC_LAI_SUAT_CT;
        public static DatabaseConstant.Table Table = DatabaseConstant.Table.DC_LSUAT;
        public DatabaseConstant.Action Action;

        public event EventHandler OnSavingCompleted;

        private int id = 0;
        public string formCase = null;
        bool isLoaded = false;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string tthaiNvu = "";
        public string TthaiNvu
        {
            get { return tthaiNvu; }
            set { tthaiNvu = value; }
        }
        DataTable dtLaiSuatCT = new DataTable();
        #endregion

        #region Khoi tao
        public ucLaiSuatCT()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.LaiSuat;component/LaiSuat/ucLaiSuatCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }

            //InitEventHandler();

            BindShortkey();

            //HideControl();

            // Refresh buttons
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_LAI_SUAT_CT);
        }
        #endregion

        #region Dang ky hot key, shortcut key

        /// <summary>
        /// Định nghĩa phím tắt
        /// </summary>
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
                        key = new KeyBinding(ucLaiSuatCT.HoldCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(SaveCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(ucLaiSuatCT.ModifyCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.Shift);
                        key = new KeyBinding(ucLaiSuatCT.DeleteCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.A, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucLaiSuatCT.ApproveCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucLaiSuatCT.RefuseCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.C, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucLaiSuatCT.CancelCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(ucLaiSuatCT.HelpCommand, keyg);
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
            beforeAddNew();
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
            e.CanExecute = tlbSave.IsEnabled;
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
                beforeAddNew();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                onHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
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
                beforeAddNew();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                onHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
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

        /// <summary>
        /// Sự kiện ấn key trên form
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
        /// Sự kiện load cho form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isLoaded)
            {
                LoadCombobox();
                ResetForm();
                InitEventHandler();
                txtTenLS.Focus();
                if (Action == DatabaseConstant.Action.XEM)
                    beforeView();
                else if (Action == DatabaseConstant.Action.SUA)
                    beforeModifyFromDetail();
                else
                    beforeAddNew();
                HideControl();
                HienThiLaiSuatBacThang();
                isLoaded = true;
            }
        }

        /// <summary>
        /// Sự kiện unload cho form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            listLockId.Add(id);

            bool ret = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_LAI_SUAT_CT,
                DatabaseConstant.Table.DC_LSUAT,
                DatabaseConstant.Action.SUA,
                listLockId);
            id = 0;
            isLoaded = false;
        }

        /// <summary>
        /// Khởi tạo các combobox
        /// </summary>
        private void LoadCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            cmbPhanHe.Items.Clear();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DANH_MUC_PHAN_HE.getValue());
            lstSourcePhanHe = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstSourcePhanHe, ref cmbPhanHe, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, DatabaseConstant.Module.TDVM.getValue());
            // Hiện tại chỉ thực hiện cho HDVO, TDVM
            auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHe, DatabaseConstant.Module.QTHT.getValue());
            auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHe, DatabaseConstant.Module.DMDC.getValue());
            auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHe, DatabaseConstant.Module.KHTV.getValue());
            auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHe, DatabaseConstant.Module.GDKT.getValue());
            auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHe, DatabaseConstant.Module.QLTS.getValue());
            auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHe, DatabaseConstant.Module.BHTH.getValue());
            auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHe, DatabaseConstant.Module.NSTL.getValue());
            auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHe, DatabaseConstant.Module.KTDL.getValue());

            cmbLoaiTien.Items.Clear();
            lstDieuKien = new List<string>();
            lstSourceLoaiTien = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TIENTE.getValue(), lstDieuKien, ClientInformation.MaDongNoiTe);

            cmbLSDonViTinh.Items.Clear();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.TAN_SUAT.getValue());
            lstSourceLaiSuatDonViTinh = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstSourceLaiSuatDonViTinh, ref cmbLSDonViTinh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, BusinessConstant.TAN_SUAT.NAM.layGiaTri());

            cmbLSLoai.Items.Clear();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.PPHAP_TINH_LSUAT.getValue());
            lstSourceLoaiLaiSuat = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstSourceLoaiLaiSuat, ref cmbLSLoai, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, BusinessConstant.PPHAP_TINH_LSUAT.DTH.layGiaTri());

            cmbLSLoaiBacThang.Items.Clear();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.HTHUC_BTHANG.getValue());
            lstSourceHinhThucBacThang = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstSourceHinhThucBacThang, ref cmbLSLoaiBacThang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

            cmbKHDonViTinh.Items.Clear();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.TAN_SUAT.getValue());
            lstSourceLaiSuatKyHan = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstSourceLaiSuatKyHan, ref cmbKHDonViTinh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, BusinessConstant.TAN_SUAT.THANG.layGiaTri());
        }

        /// <summary>
        /// InitEventHandler
        /// </summary>
        private void InitEventHandler()
        {
            cmbLSLoai.SelectionChanged += cmbLSLoai_SelectionChanged;
            cmbLSLoaiBacThang.SelectionChanged += cmbLSLoaiBacThang_SelectionChanged;
            cmbPhanHe.KeyDown += cmbPhanHe_KeyDown;
            //cmbLoaiTien.KeyDown += cmbLoaiTien_KeyDown;
            cmbLSDonViTinh.KeyDown += cmbLSDonViTinh_KeyDown;
            cmbLSLoai.KeyDown += cmbLSLoai_KeyDown;
            cmbLSLoaiBacThang.KeyDown += cmbLSLoaiBacThang_KeyDown;
            cmbKHDonViTinh.KeyDown += cmbKHDonViTinh_KeyDown;
            //raddgrLSBacThangDS.CellValidating += raddgrLSBacThangDS_CellValidating;
        }

        /// <summary>
        /// thiết lập hiển thị cho các control
        /// </summary>
        private void HideControl()
        {
            this.Cursor = Cursors.Wait;
            try
            {
                if (formCase == null)
                {
                    formCase = ClientInformation.FormCase;
                }
                if (!string.IsNullOrEmpty(formCase))
                {
                    HeThong hethong = new HeThong();
                    ArrayList arr = new ArrayList();
                    arr = hethong.SetVisibleControl("PresentationWPF.LaiSuat.LaiSuat.ucLaiSuatCT", formCase);
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

                if(tthaiNvu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    cmbLSLoai.IsEnabled = false;
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void SetEnableControl()
        {

        }

        /// <summary>
        /// Đưa form về trạng thái mặc định
        /// </summary>
        private void ResetForm()
        {
            tthaiNvu = "";
            lblTrangThai.Content = "";
            txtMaLS.Text = string.Empty;
            txtTenLS.Text = string.Empty;
            raddtNgayHL.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            raddtNgayHetHL.Value = null;
            radnumLaiSuat.Value = 0;
            txtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            txtNgayCapNhat.Value = null;
            txtNguoiCapNhat.Text = "";
            txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri());
            txtNguoiCapNhat.Text = ClientInformation.TenDangNhap;                       
            raddgrLSBacThangDS.Items.Clear();
            raddgrLSLaiSuatDS.Items.Clear();            
            titemThongTinChung.Focus();
        }

        private void HienThiLaiSuatBacThang()
        {
            string loaiLaiSuat = lstSourceLoaiLaiSuat.ElementAt(cmbLSLoai.SelectedIndex).KeywordStrings.First();
            if (loaiLaiSuat.Equals(BusinessConstant.PPHAP_TINH_LSUAT.BTH.layGiaTri()))
            {
                cmbLSLoaiBacThang.IsEnabled = cmbLSLoai.IsEnabled;
                if (lstSourceHinhThucBacThang.ElementAt(cmbLSLoaiBacThang.SelectedIndex).KeywordStrings.First().Equals(BusinessConstant.HTHUC_BTHANG.SO_TIEN.layGiaTri()))
                    cmbKHDonViTinh.IsEnabled = false;
                else
                    cmbKHDonViTinh.IsEnabled = cmbLSLoai.IsEnabled;
                grbLSBacThang.Visibility = Visibility.Visible;
                radnumLaiSuat.Visibility = Visibility.Collapsed;
                lblLaiSuat.Visibility = Visibility.Collapsed;
            }
            else
            {
                cmbLSLoaiBacThang.IsEnabled = false;
                cmbKHDonViTinh.IsEnabled = false;
                grbLSBacThang.Visibility = Visibility.Collapsed;
                radnumLaiSuat.Visibility = Visibility.Visible;
                lblLaiSuat.Visibility = Visibility.Visible;
            }
        }

        private void cmbLSLoai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HienThiLaiSuatBacThang();
            raddgrLSBacThangDSColumn();
        }

        private void cmbLSLoaiBacThang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstSourceHinhThucBacThang.ElementAt(cmbLSLoaiBacThang.SelectedIndex).KeywordStrings.First().Equals(BusinessConstant.HTHUC_BTHANG.SO_TIEN.layGiaTri()))
                cmbKHDonViTinh.IsEnabled = false;
            else
                cmbKHDonViTinh.IsEnabled = cmbLSLoai.IsEnabled;
            raddgrLSBacThangDSColumn();
        }

        private void cmbPhanHe_LostFocus(object sender, RoutedEventArgs e)
        {
            if (lstSourcePhanHe.Select(i => i.DisplayName).Contains(cmbPhanHe.Text) != true)
                cmbPhanHe.SelectedIndex = 0;
        }

        //private void cmbLoaiTien_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    if (lstSourceLoaiTien.Select(i => i.DisplayName).Contains(cmbLoaiTien.Text) != true)
        //        cmbLoaiTien.SelectedIndex = 0;
        //}

        private void cmbLSDonViTinh_LostFocus(object sender, RoutedEventArgs e)
        {
            if (lstSourceLaiSuatDonViTinh.Select(i => i.DisplayName).Contains(cmbLSDonViTinh.Text) != true)
                cmbLSDonViTinh.SelectedIndex = 0;
        }

        private void cmbLSLoai_LostFocus(object sender, RoutedEventArgs e)
        {
            if (lstSourceLoaiLaiSuat.Select(i => i.DisplayName).Contains(cmbLSLoai.Text) != true)
                cmbLSLoai.SelectedIndex = 0;
        }

        private void cmbLSLoaiBacThang_LostFocus(object sender, RoutedEventArgs e)
        {
            if (lstSourceHinhThucBacThang.Select(i => i.DisplayName).Contains(cmbLSLoaiBacThang.Text) != true)
                cmbLSLoaiBacThang.SelectedIndex = 0;
        }

        private void cmbKHDonViTinh_LostFocus(object sender, RoutedEventArgs e)
        {
            if (lstSourceLaiSuatKyHan.Select(i => i.DisplayName).Contains(cmbKHDonViTinh.Text) != true)
                cmbKHDonViTinh.SelectedIndex = 0;
        }

        private void cmbPhanHe_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                if (lstSourcePhanHe.Select(i => i.DisplayName).Contains(cmbPhanHe.Text) != true)
                    cmbPhanHe.SelectedIndex = 0;
            }
            cmbPhanHe.IsDropDownOpen = true;
        }

        //private void cmbLoaiTien_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
        //    {
        //        if (lstSourceLoaiTien.Select(i => i.DisplayName).Contains(cmbLoaiTien.Text) != true)
        //            cmbLoaiTien.SelectedIndex = 0;
        //    }
        //    cmbLoaiTien.IsDropDownOpen = true;
        //}

        private void cmbLSDonViTinh_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                if (lstSourceLaiSuatDonViTinh.Select(i => i.DisplayName).Contains(cmbLSDonViTinh.Text) != true)
                    cmbLSDonViTinh.SelectedIndex = 0;
            }
            cmbLSDonViTinh.IsDropDownOpen = true;
        }

        private void cmbLSLoai_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                if (lstSourceLoaiLaiSuat.Select(i => i.DisplayName).Contains(cmbLSLoai.Text) != true)
                    cmbLSLoai.SelectedIndex = 0;
            }
            cmbLSLoai.IsDropDownOpen = true;
        }

        private void cmbLSLoaiBacThang_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                if (lstSourceHinhThucBacThang.Select(i => i.DisplayName).Contains(cmbLSLoaiBacThang.Text) != true)
                    cmbLSLoaiBacThang.SelectedIndex = 0;
            }
            cmbLSLoaiBacThang.IsDropDownOpen = true;
        }

        private void cmbKHDonViTinh_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                if (lstSourceLaiSuatKyHan.Select(i => i.DisplayName).Contains(cmbKHDonViTinh.Text) != true)
                    cmbKHDonViTinh.SelectedIndex = 0;
            }
            cmbKHDonViTinh.IsDropDownOpen = true;
        }

        private void raddgrLSBacThangDS_Loaded(object sender, RoutedEventArgs e)
        {
            raddgrLSBacThangDSColumn();
        }

        private void raddgrLSBacThangDSColumn()
        {
            raddgrLSBacThangDS.SelectedItems.Clear();
            raddgrLSBacThangDS.Columns[1].Width = new Telerik.Windows.Controls.GridViewLength(50, Telerik.Windows.Controls.GridViewLengthUnitType.Pixel);
            raddgrLSBacThangDS.Columns[2].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Star);
            raddgrLSBacThangDS.Columns[3].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Star);
            raddgrLSBacThangDS.Columns[4].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Star);
            if (lstSourceHinhThucBacThang.ElementAt(cmbLSLoaiBacThang.SelectedIndex).KeywordStrings.First().Equals(BusinessConstant.HTHUC_BTHANG.KY_HAN.layGiaTri()))
            {
                raddgrLSBacThangDS.Columns[2].IsVisible = true;
                raddgrLSBacThangDS.Columns[3].IsVisible = false;
            }
            else if (lstSourceHinhThucBacThang.ElementAt(cmbLSLoaiBacThang.SelectedIndex).KeywordStrings.First().Equals(BusinessConstant.HTHUC_BTHANG.SO_TIEN.layGiaTri()))
            {
                raddgrLSBacThangDS.Columns[2].IsVisible = false;
                raddgrLSBacThangDS.Columns[3].IsVisible = true;
            }
            else
            {
                raddgrLSBacThangDS.Columns[3].IsVisible = true;
                raddgrLSBacThangDS.Columns[2].IsVisible = true;
            }

        }

        private void radGridNumLaiSuat_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                ThemChiTiet();
            }
        }

        #endregion

        #region Xu ly nghiep vu

        /// <summary>
        /// Sự kiện load dữ liệu lên form
        /// </summary>
        private void SetFormData()
        {
            LaiSuatProcess laisuatProcess = new LaiSuatProcess();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                dtLaiSuatCT = new DataTable();
                dtLaiSuatCT.Columns.Add("STT", typeof(string));
                dtLaiSuatCT.Columns.Add("KY_HAN", typeof(string));
                dtLaiSuatCT.Columns.Add("SO_TIEN", typeof(string));
                dtLaiSuatCT.Columns.Add("LAI_SUAT", typeof(string));
                dtLaiSuatCT.NewRow();
                dtLaiSuatCT.Rows.Add(1, 0, 0, 0);
                raddgrLSBacThangDS.ItemsSource = dtLaiSuatCT.DefaultView;

                if (id != 0)
                {
                    //Sự kiện load dữ liệu
                    DataSet dsLaiSuat = laisuatProcess.GetLaiSuatByID(id);
                    DataTable dtLaiSuat = dsLaiSuat.Tables[0];
                    DataTable dtLaiSuatLS = dsLaiSuat.Tables[2]; //= laisuatProcess.GetLaiSuatLichSuByID(id);
                    if (dsLaiSuat != null && dsLaiSuat.Tables.Count > 0 && dtLaiSuat.Rows.Count > 0)
                    {
                        // ID ,MA_LSUAT,NGAY_ADUNG,NGAY_HHAN,MO_TA,MA_LOAI_TIEN,MA_PHAN_HE,PPHAP_TINH_LSUAT,HTHUC_BTHANG,DVI_TINH,TTHAI_BGHI,TTHAI_NVU,MA_DVI_QLY,MA_DVI_TAO,NGAY_NHAP,NGUOI_NHAP,NGAY_CNHAT,NGUOI_CNHAT
                        // Chi tiết 
                        // STT,KY_HAN,SO_TIEN,LAI_SUAT
                        // Chi tiết lịch sử 
                        // STT,NGAY_ADUNG,NGAY_HHAN,KY_HAN,KY_HAN_DVI_TINH,LAI_SUAT
                        cmbPhanHe.SelectedIndex = lstSourcePhanHe.IndexOf(lstSourcePhanHe.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dtLaiSuat.Rows[0]["MA_PHAN_HE"].ToString())));
                        cmbLoaiTien.SelectedIndex = lstSourceLoaiTien.IndexOf(lstSourceLoaiTien.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dtLaiSuat.Rows[0]["MA_LOAI_TIEN"].ToString())));
                        cmbLSDonViTinh.SelectedIndex = lstSourceLaiSuatDonViTinh.IndexOf(lstSourceLaiSuatDonViTinh.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dtLaiSuat.Rows[0]["DVI_TINH"].ToString())));
                        cmbLSLoai.SelectedIndex = lstSourceLoaiLaiSuat.IndexOf(lstSourceLoaiLaiSuat.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dtLaiSuat.Rows[0]["PPHAP_TINH_LSUAT"].ToString())));

                        txtMaLS.Text = dtLaiSuat.Rows[0]["MA_LSUAT"].ToString();
                        txtTenLS.Text = dtLaiSuat.Rows[0]["MO_TA"].ToString();
                        raddtNgayHL.Value = LDateTime.StringToDate(dtLaiSuat.Rows[0]["NGAY_ADUNG"].ToString(), "yyyyMMdd");
                        if (dtLaiSuat.Rows[0]["NGAY_HHAN"].ToString().Length == 8)
                            raddtNgayHetHL.Value = LDateTime.StringToDate(dtLaiSuat.Rows[0]["NGAY_HHAN"].ToString(), "yyyyMMdd");

                        dtLaiSuatCT = new DataTable();
                        dtLaiSuatCT = dsLaiSuat.Tables[1];

                        if (dtLaiSuatCT.Rows.Count > 0)
                        {
                            if (dtLaiSuat.Rows[0]["PPHAP_TINH_LSUAT"].ToString().Equals(BusinessConstant.PPHAP_TINH_LSUAT.BTH.layGiaTri()))
                            {
                                cmbLSLoaiBacThang.SelectedIndex = lstSourceHinhThucBacThang.IndexOf(lstSourceHinhThucBacThang.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dtLaiSuat.Rows[0]["HTHUC_BTHANG"].ToString())));
                                cmbKHDonViTinh.SelectedIndex = lstSourceLaiSuatKyHan.IndexOf(lstSourceLaiSuatKyHan.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dtLaiSuatCT.Rows[0]["KY_HAN_DVI_TINH"].ToString())));

                                raddgrLSBacThangDS.ItemsSource = null;
                                raddgrLSBacThangDS.ItemsSource = dtLaiSuatCT.DefaultView;
                                raddgrLSBacThangDSColumn();
                            }
                            else
                                radnumLaiSuat.Value = Convert.ToDouble(dtLaiSuatCT.Rows[0]["LAI_SUAT"].ToString());
                        }

                        raddgrLSLaiSuatDS.ItemsSource = dtLaiSuatLS.DefaultView;

                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(dtLaiSuat.Rows[0]["TTHAI_BGHI"].ToString());
                        txtNguoiLap.Text = dtLaiSuat.Rows[0]["NGUOI_NHAP"].ToString();
                        txtNguoiCapNhat.Text = dtLaiSuat.Rows[0]["NGUOI_CNHAT"].ToString();
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(dtLaiSuat.Rows[0]["TTHAI_NVU"].ToString());
                        TthaiNvu = dtLaiSuat.Rows[0]["TTHAI_NVU"].ToString();
                        DateTime dtNgayLap = LDateTime.StringToDate(dtLaiSuat.Rows[0]["NGAY_NHAP"].ToString(), "yyyyMMdd");
                        txtNgayLap.Text = dtNgayLap.ToString("dd/MM/yyyy");
                        if (!string.IsNullOrEmpty(dtLaiSuat.Rows[0]["NGAY_CNHAT"].ToString()))
                        {
                            DateTime dtNgayDuyet = LDateTime.StringToDate(dtLaiSuat.Rows[0]["NGAY_CNHAT"].ToString(), "yyyyMMdd");
                            txtNgayCapNhat.Text = dtNgayDuyet.ToString("dd/MM/yyyy");
                        }
                    }
                }
                else
                {
                    ResetForm();
                    cmbPhanHe.SelectedIndex = lstSourcePhanHe.IndexOf(lstSourcePhanHe.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(DatabaseConstant.Module.TDVM.getValue())));
                    //cmbLoaiTien.SelectedIndex = 0;
                    cmbLSDonViTinh.SelectedIndex = lstSourceLaiSuatDonViTinh.IndexOf(lstSourceLaiSuatDonViTinh.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.TAN_SUAT.NAM.layGiaTri())));
                    cmbLSLoai.SelectedIndex = lstSourceLoaiLaiSuat.IndexOf(lstSourceLoaiLaiSuat.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.PPHAP_TINH_LSUAT.DTH.layGiaTri())));
                    cmbKHDonViTinh.SelectedIndex = lstSourceLaiSuatKyHan.IndexOf(lstSourceLaiSuatKyHan.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.TAN_SUAT.THANG.layGiaTri())));
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Đưa dữ liệu từ form vào object DC_LSUAT
        /// </summary>
        private void GetFormData(ref DC_LSUAT obj, ref List<DC_LSUAT_CTIET> lst)
        {
            if (id != 0)
            {
                obj.ID = id;
            }
            obj.MA_LSUAT = txtMaLS.Text;

            obj.MO_TA = txtTenLS.Text;
            obj.MA_PHAN_HE = lstSourcePhanHe.ElementAt(cmbPhanHe.SelectedIndex).KeywordStrings.First();
            obj.MA_LOAI_TIEN = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.First();
            obj.DVI_TINH = lstSourceLaiSuatDonViTinh.ElementAt(cmbLSDonViTinh.SelectedIndex).KeywordStrings.First();
            obj.PPHAP_TINH_LSUAT = lstSourceLoaiLaiSuat.ElementAt(cmbLSLoai.SelectedIndex).KeywordStrings.First();
            if (cmbLSLoaiBacThang.IsEnabled)
                obj.HTHUC_BTHANG = lstSourceHinhThucBacThang.ElementAt(cmbLSLoaiBacThang.SelectedIndex).KeywordStrings.First();
            obj.NGAY_ADUNG = ((DateTime)raddtNgayHL.Value).ToString("yyyyMMdd");
            if (raddtNgayHetHL.Value is DateTime)
                obj.NGAY_HHAN = ((DateTime)raddtNgayHetHL.Value).ToString("yyyyMMdd");

            obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
            obj.TTHAI_NVU = tthaiNvu;
            obj.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
            obj.MA_DVI_QLY = ClientInformation.MaDonVi;

            obj.NGUOI_NHAP = txtNguoiLap.Text;
            obj.NGAY_NHAP = ((DateTime)txtNgayLap.Value).ToString("yyyyMMdd");
            obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
            obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;

            if (cmbLSLoaiBacThang.IsEnabled)
            {
                foreach (DataRow row in dtLaiSuatCT.Rows)
                {
                    DC_LSUAT_CTIET objCT = new DC_LSUAT_CTIET();
                    objCT.LOAI_LSUAT = obj.PPHAP_TINH_LSUAT;
                    objCT.ID_LSUAT = obj.ID;
                    objCT.KY_HAN = Convert.ToInt32(row["KY_HAN"].ToString());
                    objCT.KY_HAN_DVI_TINH = lstSourceLaiSuatKyHan.ElementAt(cmbKHDonViTinh.SelectedIndex).KeywordStrings.First();
                    objCT.LAI_SUAT = Convert.ToDecimal(row["LAI_SUAT"].ToString());
                    objCT.LSUAT_TTHUAN = objCT.LAI_SUAT;
                    objCT.MA_DVI_QLY = obj.MA_DVI_QLY;
                    objCT.MA_DVI_TAO = obj.MA_DVI_TAO;
                    objCT.MA_LSUAT = obj.MA_LSUAT;
                    objCT.NGAY_CNHAT = obj.NGAY_CNHAT;
                    objCT.NGAY_NHAP = obj.NGAY_CNHAT;
                    objCT.NGUOI_CNHAT = obj.NGUOI_CNHAT;
                    objCT.NGUOI_NHAP = obj.NGUOI_CNHAT;
                    objCT.SO_TIEN = Convert.ToDecimal(row["SO_TIEN"].ToString());
                    objCT.TTHAI_BGHI = obj.TTHAI_BGHI;
                    objCT.TTHAI_NVU = obj.TTHAI_NVU;
                    lst.Add(objCT);
                }
            }
            else
            {
                DC_LSUAT_CTIET objCT = new DC_LSUAT_CTIET();
                objCT.LOAI_LSUAT = obj.PPHAP_TINH_LSUAT;
                objCT.ID_LSUAT = obj.ID;
                objCT.LAI_SUAT = (decimal)radnumLaiSuat.Value;
                objCT.LSUAT_TTHUAN = objCT.LAI_SUAT;
                objCT.MA_DVI_QLY = obj.MA_DVI_QLY;
                objCT.MA_DVI_TAO = obj.MA_DVI_TAO;
                objCT.NGAY_CNHAT = obj.NGAY_CNHAT;
                objCT.NGAY_NHAP = obj.NGAY_CNHAT;
                objCT.NGUOI_CNHAT = obj.NGUOI_CNHAT;
                objCT.NGUOI_NHAP = obj.NGUOI_CNHAT;
                objCT.TTHAI_BGHI = obj.TTHAI_BGHI;
                objCT.TTHAI_NVU = obj.TTHAI_NVU;
                lst.Add(objCT);
            }

            return;
        }

        /// <summary>
        /// Kiểm tra các thông tin nhập vào trước khi lưu dữ liệu
        /// </summary>
        /// <returns></returns>
        private bool Validation()
        {
            dtLaiSuatCT = ((DataView)raddgrLSBacThangDS.ItemsSource).Table;
            if (dtLaiSuatCT != null && dtLaiSuatCT.Rows.Count > 0)
            {
                for (int i = dtLaiSuatCT.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow row = dtLaiSuatCT.Rows[i];
                    if ((row[1].ToString().Equals("0") || LString.IsNullOrEmptyOrSpace(row[1].ToString())) && (row[2].ToString().Equals("0") || LString.IsNullOrEmptyOrSpace(row[2].ToString())) && (row[3].ToString().Equals("0") || LString.IsNullOrEmptyOrSpace(row[3].ToString())))
                        dtLaiSuatCT.Rows.RemoveAt(i);
                }
            }

            if (LString.IsNullOrEmptyOrSpace(txtTenLS.Text.Trim()))
            {
                //LMessage.ShowMessage("M.DanhMuc.ucLaiSuatCT.ThieuTenLaiSuat", LMessage.MessageBoxType.Warning);
                CommonFunction.ThongBaoChuaNhap(lblTenLaiSuat.Content.ToString());
                txtTenLS.Focus();
                return false;
            }

            else if (!(raddtNgayHL.Value is DateTime))
            {
                //LMessage.ShowMessage("M.DanhMuc.ucLaiSuatCT.NgayHieuLuc", LMessage.MessageBoxType.Warning);
                CommonFunction.ThongBaoChuaNhap(lblNgayHieuLuc.Content.ToString());
                raddtNgayHL.Focus();
                return false;
            }

            //CongLC: Quan niệm lãi suất bằng 0 vẫn được coi là hợp lệ
            //else if (cmbLSLoaiBacThang.IsEnabled == false && (radnumLaiSuat.Value == 0 || LString.IsNullOrEmptyOrSpace(radnumLaiSuat.Text)))
            //{
            //    LMessage.ShowMessage("M.DanhMuc.ucLaiSuatCT.LaiSuat", LMessage.MessageBoxType.Warning);
            //    radnumLaiSuat.Focus();
            //    return false;
            //}

            string phuongPhapTinh = lstSourceLoaiLaiSuat.ElementAt(cmbLSLoai.SelectedIndex).KeywordStrings.First();
            if (phuongPhapTinh.Equals(BusinessConstant.PPHAP_TINH_LSUAT.BTH.layGiaTri()) && dtLaiSuatCT.Rows.Count == 0)
            {
                LMessage.ShowMessage("M.DanhMuc.ucLaiSuatCT.LaiSuatCT", LMessage.MessageBoxType.Warning);
                raddgrLSBacThangDS.Focus();
                return false;
            }
            else
            {
                if (!ValidateLaiSuatChiTiet())
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Trước khi xem
        /// </summary>
        public void beforeView()
        {
            SetFormData();
            formCase = "XEM";
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_LAI_SUAT_CT);
        }

        /// <summary>
        /// Trước khi thêm mới
        /// </summary>
        private void beforeAddNew()
        {
            id = 0;
            SetFormData();
            formCase = "MANAGE";
            HideControl();
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_LAI_SUAT_CT);
        }

        /// <summary>
        /// Trước khi sửa từ chi tiết
        /// </summary>
        private void beforeModifyFromDetail()
        {
            // Yêu cầu lock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool ret = process.LockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_LAI_SUAT_CT,
                DatabaseConstant.Table.DC_LSUAT,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Nếu lock thành công >> cho phép sửa
            if (ret)
            {
                SetFormData();
                formCase = "MANAGE";
                HideControl();
                HienThiLaiSuatBacThang();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_LAI_SUAT_CT);
            }
            else
            {
                LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
            }
        }

        /// <summary>
        /// Trước khi xóa
        /// </summary>
        private void beforeDelete()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_LAI_SUAT_CT,
                    DatabaseConstant.Table.DC_LSUAT,
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

        /// <summary>
        /// Trước khi duyệt
        /// </summary>
        private void beforeApprove()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_LAI_SUAT_CT,
                    DatabaseConstant.Table.DC_LSUAT,
                    DatabaseConstant.Action.DUYET,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
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

        /// <summary>
        /// Trước khi thoái duyệt
        /// </summary>
        private void beforeCancel()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_LAI_SUAT_CT,
                    DatabaseConstant.Table.DC_LSUAT,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
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
                listLockId.Add(id);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_LAI_SUAT_CT,
                    DatabaseConstant.Table.DC_LSUAT,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
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
            tthaiNvu = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            if (Validation())
            {
                LaiSuatProcess laisuatProcess = new LaiSuatProcess();
                try
                {
                    // Dữ liệu truyền vào và dữ liệu trả về
                    DC_LSUAT obj = new DC_LSUAT();
                    List<DC_LSUAT_CTIET> lst = new List<DC_LSUAT_CTIET>();

                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (id == 0)
                    {
                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref lst);                        
                                        
                        List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                        bool ret = laisuatProcess.LaiSuat(DatabaseConstant.Action.THEM, ref obj, ref lst, ref listClientResponseDetail);

                        afterAddNew(ret, obj, listClientResponseDetail);
                    }
                    // Nếu là lưu tạm khi sửa
                    // Hoặc lưu tạm khi sửa sau duyệt
                    // Hoặc sửa
                    else
                    {
                        // Lấy thông tin cũ
                        //obj = laisuatProcess.GetLaiSuatByID(id);

                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref lst);                                          

                        List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                        bool ret = laisuatProcess.LaiSuat(DatabaseConstant.Action.SUA, ref obj, ref lst, ref listClientResponseDetail);
                        afterModify(ret, obj, listClientResponseDetail);
                    }
                }
                catch (System.Exception ex)
                {
                    if (id > 0)
                    {
                        // Yêu cầu Unlock bản ghi cần sửa
                        UtilitiesProcess process = new UtilitiesProcess();
                        List<int> listLockId = new List<int>();
                        listLockId.Add(id);

                        bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                            DatabaseConstant.Function.DC_LAI_SUAT_CT,
                            DatabaseConstant.Table.DC_LSUAT,
                            DatabaseConstant.Action.SUA,
                            listLockId);
                    }
                    this.Cursor = Cursors.Arrow;
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                finally
                {
                    laisuatProcess = null;
                }
            }
        }

        /// <summary>
        /// Lưu tạm dữ liệu
        /// </summary>
        private void onHold()
        {
            string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));

            if (Validation())
            {
                LaiSuatProcess laisuatProcess = new LaiSuatProcess();
                try
                {
                    // Dữ liệu truyền vào và dữ liệu trả về
                    DC_LSUAT obj = new DC_LSUAT();
                    List<DC_LSUAT_CTIET> lst = new List<DC_LSUAT_CTIET>();

                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (id == 0)
                    {
                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref lst);
                        obj.TTHAI_NVU = trangThai;

                        // Set các thông tin khác                        
                        obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                        obj.NGAY_NHAP = LDateTime.GetCurrentDate(ApplicationConstant.defaultDateTimeFormat);
                        List<DC_LSUAT> lstLS = new List<DC_LSUAT>();
                        lstLS.Add(obj);
                        List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                        bool ret = laisuatProcess.LaiSuat( DatabaseConstant.Action.THEM, ref obj, ref lst, ref listClientResponseDetail);

                        afterModify(ret, lstLS.First(), listClientResponseDetail);
                    }
                    // Nếu là lưu tạm khi sửa
                    // Hoặc lưu tạm khi sửa sau duyệt
                    // Hoặc sửa
                    else
                    {
                        // Lấy thông tin cũ
                        obj = laisuatProcess.LayThongTinLaiSuat(id);

                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref lst);
                        obj.TTHAI_NVU = trangThai;
                        List<DC_LSUAT> lstLS = new List<DC_LSUAT>();
                        lstLS.Add(obj);
                        List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                        bool ret = laisuatProcess.LaiSuat(DatabaseConstant.Action.SUA, ref obj, ref lst, ref listClientResponseDetail);
                        afterModify(ret, lstLS.First(), listClientResponseDetail);
                    }
                }
                catch (System.Exception ex)
                {
                    if (id > 0)
                    {
                        // Yêu cầu Unlock bản ghi cần sửa
                        UtilitiesProcess process = new UtilitiesProcess();
                        List<int> listLockId = new List<int>();
                        listLockId.Add(id);

                        bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                            DatabaseConstant.Function.DC_LAI_SUAT_CT,
                            DatabaseConstant.Table.DC_LSUAT,
                            DatabaseConstant.Action.SUA,
                            listLockId);
                    }
                    this.Cursor = Cursors.Arrow;
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                finally
                {
                    laisuatProcess = null;
                }
            }
        }

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        private void onDelete()
        {
            LaiSuatProcess laisuatProcess = new LaiSuatProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = id;
                // Dữ liệu truyền vào và dữ liệu trả về
                DC_LSUAT obj = new DC_LSUAT();
                obj.ID = id;
                obj.MA_LSUAT = txtMaLS.Text;
                List<DC_LSUAT> lstLS = new List<DC_LSUAT>();
                lstLS.Add(obj);
                List<DC_LSUAT_CTIET> lst = new List<DC_LSUAT_CTIET>();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                bool ret = laisuatProcess.LaiSuat(DatabaseConstant.Action.XOA, ref obj, ref lst, ref listClientResponseDetail);

                afterDelete(ret, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                if (id > 0)
                {
                    // Yêu cầu Unlock bản ghi cần sửa
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);

                    bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_LAI_SUAT_CT,
                        DatabaseConstant.Table.DC_LSUAT,
                        DatabaseConstant.Action.SUA,
                        listLockId);
                }
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Duyệt dữ liệu
        /// </summary>
        private void onApprove()
        {
            LaiSuatProcess laisuatProcess = new LaiSuatProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = id;
                // Dữ liệu truyền vào và dữ liệu trả về
                DC_LSUAT obj = new DC_LSUAT();
                obj.ID = id;
                obj.MA_LSUAT = txtMaLS.Text;
                List<DC_LSUAT_CTIET> lst = new List<DC_LSUAT_CTIET>();
                GetFormData(ref obj, ref lst);
                List<DC_LSUAT> lstLS = new List<DC_LSUAT>();
                lstLS.Add(obj);
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                bool ret = laisuatProcess.LaiSuat(DatabaseConstant.Action.DUYET, ref obj, ref lst, ref listClientResponseDetail);

                afterApprove(ret, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                if (id > 0)
                {
                    // Yêu cầu Unlock bản ghi cần sửa
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);

                    bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_LAI_SUAT_CT,
                        DatabaseConstant.Table.DC_LSUAT,
                        DatabaseConstant.Action.SUA,
                        listLockId);
                }
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Thoái duyệt dữ liệu
        /// </summary>
        private void onCancel()
        {
            LaiSuatProcess laisuatProcess = new LaiSuatProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = id;
                // Dữ liệu truyền vào và dữ liệu trả về
                DC_LSUAT obj = new DC_LSUAT();
                obj.ID = id;
                obj.MA_LSUAT = txtMaLS.Text;
                List<DC_LSUAT> lstLS = new List<DC_LSUAT>();
                lstLS.Add(obj);
                List<DC_LSUAT_CTIET> lst = new List<DC_LSUAT_CTIET>();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                bool ret = laisuatProcess.LaiSuat(DatabaseConstant.Action.THOAI_DUYET, ref obj, ref lst, ref listClientResponseDetail);

                afterCancel(ret, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                if (id > 0)
                {
                    // Yêu cầu Unlock bản ghi cần sửa
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);

                    bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_LAI_SUAT_CT,
                        DatabaseConstant.Table.DC_LSUAT,
                        DatabaseConstant.Action.SUA,
                        listLockId);
                }
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Từ chối duyệt dữ liệu
        /// </summary>
        private void onRefuse()
        {
            LaiSuatProcess laisuatProcess = new LaiSuatProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = id;
                // Dữ liệu truyền vào và dữ liệu trả về
                DC_LSUAT obj = new DC_LSUAT();
                obj.ID = id;
                obj.MA_LSUAT = txtMaLS.Text;
                List<DC_LSUAT> lstLS = new List<DC_LSUAT>();
                lstLS.Add(obj);
                List<DC_LSUAT_CTIET> lst = new List<DC_LSUAT_CTIET>();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                bool ret = laisuatProcess.LaiSuat(DatabaseConstant.Action.TU_CHOI_DUYET, ref obj, ref lst, ref listClientResponseDetail);

                afterRefuse(ret, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                if (id > 0)
                {
                    // Yêu cầu Unlock bản ghi cần sửa
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);

                    bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_LAI_SUAT_CT,
                        DatabaseConstant.Table.DC_LSUAT,
                        DatabaseConstant.Action.SUA,
                        listLockId);
                }
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Sau khi thêm mới
        /// </summary>
        /// <param name="ret"></param>
        private void afterAddNew(bool ret, DC_LSUAT obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret == true)
            {
                LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                if (cbMultiAdd.IsChecked == true)
                {
                    beforeAddNew();
                }
                else if (!DatabaseConstant.CLOSE_DETAIL_FORM)
                {
                    id = obj.ID;
                    TthaiNvu = obj.TTHAI_NVU;
                    txtMaLS.Text = obj.MA_LSUAT;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(obj.TTHAI_NVU);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);

                    formCase = "XEM";
                    HideControl();
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_LAI_SUAT_CT);

                    titemThongTinChung.Focus();
                }
                else
                {
                    onClose();
                }
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                //LMessage.ShowMessage("M.DungChung.ThemKhongThanhCong", LMessage.MessageBoxType.Error);
            }
        }

        /// <summary>
        /// Sau khi sửa
        /// </summary>
        /// <param name="ret"></param>
        private void afterModify(bool ret, DC_LSUAT obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret == true)
            {
                LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                id = obj.ID;
                TthaiNvu = obj.TTHAI_NVU;
                txtMaLS.Text = obj.MA_LSUAT;
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(obj.TTHAI_NVU);
                txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);

                formCase = "XEM";
                HideControl();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_LAI_SUAT_CT);

                titemThongTinChung.Focus();
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                //LMessage.ShowMessage("M.DungChung.CapNhatKoThanhCong", LMessage.MessageBoxType.Error);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_LAI_SUAT_CT,
                DatabaseConstant.Table.DC_LSUAT,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Sau khi xóa
        /// </summary>
        /// <param name="ret"></param>
        private void afterDelete(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);

                //SetEnabledAllControls(false);
                //SetEnabledRequiredControls(false);
                //CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_LAI_SUAT_CT);
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                //LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);
            List<int> listUnlockId = new List<int>();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_LAI_SUAT_CT,
                DatabaseConstant.Table.DC_LSUAT,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Đóng cửa sổ chi tiết sau khi xóa
            onClose();
        }

        /// <summary>
        /// Sau khi duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void afterApprove(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);

                TthaiNvu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);

                formCase = "XEM";
                HideControl();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_LAI_SUAT_CT);
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                //LMessage.ShowMessage("M.DungChung.DuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_LAI_SUAT_CT,
                DatabaseConstant.Table.DC_LSUAT,
                DatabaseConstant.Action.DUYET,
                listLockId);
        }

        /// <summary>
        /// Sau khi thoái duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void afterCancel(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);

                TthaiNvu = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);

                formCase = "XEM";
                HideControl();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_LAI_SUAT_CT);
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                //LMessage.ShowMessage("M.DungChung.ThoaiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_LAI_SUAT_CT,
                DatabaseConstant.Table.DC_LSUAT,
                DatabaseConstant.Action.THOAI_DUYET,
                listLockId);
        }

        /// <summary>
        /// Sau khi từ chối
        /// </summary>
        /// <param name="ret"></param>
        private void afterRefuse(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);

                TthaiNvu = BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);

                formCase = "XEM";
                HideControl();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_LAI_SUAT_CT);
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                //LMessage.ShowMessage("M.DungChung.TuChoiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_LAI_SUAT_CT,
                DatabaseConstant.Table.DC_LSUAT,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                listLockId);
        }

        private void tlbAddLSBacThang_Click(object sender, RoutedEventArgs e)
        {
            ThemChiTiet();
        }

        private void ThemChiTiet()
        {
            raddgrLSBacThangDS.ItemsSource = null;
            if (dtLaiSuatCT.Rows.Count > 0)
            {
                for (int i = dtLaiSuatCT.Rows.Count; i > 0; i--)
                {
                    DataRow row = dtLaiSuatCT.Rows[i - 1];
                    if ((row["SO_TIEN"].ToString().Equals("0") || LString.IsNullOrEmptyOrSpace(row["SO_TIEN"].ToString())) && (row["KY_HAN"].ToString().Equals("0") || LString.IsNullOrEmptyOrSpace(row["KY_HAN"].ToString())) && (row["LAI_SUAT"].ToString().Equals("0") || LString.IsNullOrEmptyOrSpace(row["LAI_SUAT"].ToString())))
                        dtLaiSuatCT.Rows.RemoveAt(i - 1);
                }
                for (int i = dtLaiSuatCT.Rows.Count; i > 0; i--)
                {
                    dtLaiSuatCT.Rows[i - 1][0] = i.ToString();
                }
            }
            dtLaiSuatCT.NewRow();
            dtLaiSuatCT.Rows.Add((dtLaiSuatCT.Rows.Count + 1), 0, 0, 0);
            raddgrLSBacThangDS.ItemsSource = dtLaiSuatCT.DefaultView;
        }

        private void tlbDeleteLSBacThang_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            try
            {
                if (raddgrLSBacThangDS.SelectedItems.Count > 0)
                {
                    List<DataRow> lstRowDel = new List<DataRow>();
                    foreach (DataRowView item in raddgrLSBacThangDS.SelectedItems)
                    {
                        DataRow r = dtLaiSuatCT.AsEnumerable().FirstOrDefault(d => Convert.ToDouble(d[1].ToString()) == Convert.ToDouble((item.Row[1])) && Convert.ToDouble(d[2].ToString()) == Convert.ToDouble((item.Row[2])) && Convert.ToDouble(d[3].ToString()) == Convert.ToDouble((item.Row[3])));
                        lstRowDel.Add(r);
                    }
                    foreach (DataRow item in lstRowDel)
                    {
                        for (int i = dtLaiSuatCT.Rows.Count; i > 0; i--)
                        {
                            if (Convert.ToDouble(dtLaiSuatCT.Rows[i - 1][1]) == Convert.ToDouble((item[1])) && Convert.ToDouble(dtLaiSuatCT.Rows[i - 1][2]) == Convert.ToDouble((item[2])) && Convert.ToDouble(dtLaiSuatCT.Rows[i - 1][3]) == Convert.ToDouble((item[3])))
                            {
                                dtLaiSuatCT.Rows.RemoveAt(i - 1);
                                break;
                            }
                        }
                    }
                    for (int i = dtLaiSuatCT.Rows.Count; i > 0; i--)
                    {
                        dtLaiSuatCT.Rows[i - 1][0] = i.ToString();
                    }
                    raddgrLSBacThangDS.ItemsSource = dtLaiSuatCT.DefaultView;
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private bool ValidateLaiSuatChiTiet()
        {
            string phuongPhapTinh = lstSourceLoaiLaiSuat.ElementAt(cmbLSLoai.SelectedIndex).KeywordStrings.First();
            if (phuongPhapTinh.Equals(BusinessConstant.PPHAP_TINH_LSUAT.BTH.layGiaTri()))
            {
                foreach (DataRow row in dtLaiSuatCT.Rows)
                {
                    int idxRow = dtLaiSuatCT.Rows.IndexOf(row);
                    // Loại hình bậc thang theo Kỳ hạn
                    if (lstSourceHinhThucBacThang.ElementAt(cmbLSLoaiBacThang.SelectedIndex).KeywordStrings.First().Equals(BusinessConstant.HTHUC_BTHANG.KY_HAN.layGiaTri()))
                    {
                        if (idxRow == 0)
                        {
                            if (!row["KY_HAN"].ToString().Replace(".00", "").Equals("0"))
                            {
                                LMessage.ShowMessage("Kỳ hạn đầu tiên phải bằng 0", LMessage.MessageBoxType.Warning);
                                raddgrLSBacThangDS.Focus();
                                return false;
                            }
                        }
                        else
                        {
                            if (!(Convert.ToInt32(row["KY_HAN"].ToString().Replace(".00", "")) > Convert.ToInt32(dtLaiSuatCT.Rows[idxRow - 1]["KY_HAN"].ToString().Replace(".00", ""))))
                            {
                                LMessage.ShowMessage("Kỳ hạn " + (idxRow + 1) + " phải lớn hơn kỳ hạn " + idxRow, LMessage.MessageBoxType.Warning);
                                raddgrLSBacThangDS.Focus();
                                return false;
                            }
                        }
                    }
                    // Loại hình bậc thang theo Số tiền
                    else if (lstSourceHinhThucBacThang.ElementAt(cmbLSLoaiBacThang.SelectedIndex).KeywordStrings.First().Equals(BusinessConstant.HTHUC_BTHANG.SO_TIEN.layGiaTri()))
                    {
                        if (idxRow == 0)
                        {
                            if (!row["SO_TIEN"].ToString().Replace(".00", "").Equals("0"))
                            {
                                LMessage.ShowMessage("Số tiền đầu tiên phải bằng 0", LMessage.MessageBoxType.Warning);
                                raddgrLSBacThangDS.Focus();
                                return false;
                            }
                        }
                        else
                        {
                            if (!(Convert.ToInt32(row["SO_TIEN"].ToString().Replace(".00", "")) > Convert.ToInt32(dtLaiSuatCT.Rows[idxRow - 1]["SO_TIEN"].ToString().Replace(".00", ""))))
                            {
                                LMessage.ShowMessage("Số tiền " + (idxRow + 1) + " phải lớn hơn số tiền " + idxRow, LMessage.MessageBoxType.Warning);
                                raddgrLSBacThangDS.Focus();
                                return false;
                            }
                        }
                    }
                    // Loại hình bậc thang theo Kỳ hạn và số tiền
                    else
                    {
                        if (idxRow == 0)
                        {
                            if (!row["KY_HAN"].ToString().Replace(".00", "").Equals("0"))
                            {
                                LMessage.ShowMessage("Kỳ hạn đầu tiên phải bằng 0", LMessage.MessageBoxType.Warning);
                                raddgrLSBacThangDS.Focus();
                                return false;
                            }
                            else if (!row["SO_TIEN"].ToString().Replace(".00", "").Equals("0"))
                            {
                                LMessage.ShowMessage("Số tiền đầu tiên phải bằng 0", LMessage.MessageBoxType.Warning);
                                raddgrLSBacThangDS.Focus();
                                return false;
                            }
                        }
                        else
                        {
                            int hieuKyHan = (Convert.ToInt32(row["KY_HAN"].ToString().Replace(".00", "")) - Convert.ToInt32(dtLaiSuatCT.Rows[idxRow - 1]["KY_HAN"].ToString().Replace(".00", "")));
                            int hieuSoTien = (Convert.ToInt32(row["SO_TIEN"].ToString().Replace(".00", "")) - Convert.ToInt32(dtLaiSuatCT.Rows[idxRow - 1]["SO_TIEN"].ToString().Replace(".00", "")));
                            if (!(hieuKyHan >= 0))
                            {
                                LMessage.ShowMessage("Kỳ hạn " + (idxRow + 1) + " phải lớn hơn hoặc bằng kỳ hạn " + idxRow, LMessage.MessageBoxType.Warning);
                                raddgrLSBacThangDS.Focus();
                                return false;
                            }
                            else if (hieuKyHan == 0 && !(hieuSoTien > 0))
                            {
                                LMessage.ShowMessage("Số tiền " + (idxRow + 1) + " phải lớn hơn số tiền " + idxRow, LMessage.MessageBoxType.Warning);
                                raddgrLSBacThangDS.Focus();
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        #endregion
    }
}
