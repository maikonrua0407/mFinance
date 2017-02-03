using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.HuyDongVonServiceRef;
using Presentation.Process.NhanSuServiceRef;
using Presentation.Process.KhachHangServiceRef;
using Presentation.Process.BaoCaoServiceRef;
using PresentationWPF.CustomControl;
using PresentationWPF.NhanSu.Converts;

namespace PresentationWPF.NhanSu.HoSo
{
    /// <summary>
    /// Interaction logic for ucHoSoNhanVienCT.xaml
    /// </summary>
    public partial class ucHoSoNhanVienCT : UserControl
    {
        #region Khai bao
        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private DatabaseConstant.Function function = DatabaseConstant.Function.NS_HO_SO_CT;

        public event EventHandler OnSavingCompleted;

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int idNguoiQuanLy = 0;

        private NS_HO_SO obj;
        public NS_HO_SO Obj
        {
            get { return obj; }
            set { obj = value; }
        }

        private string sTrangThaiNVu = "";

        List<AutoCompleteEntry> lstSourceLoaiHoSo = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceGioiTinh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTrinhDoHocVan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceChuyenNganhDaoTao = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongBan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceChucVu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHinhThucLamViec = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceQuocTich = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTonGiao = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDanToc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTinhTrangHonNhan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTinhThanhHienTai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceQuanHuyenHienTai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhuongXaHienTai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTinhThanhThuongChu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceQuanHuyenThuongChu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhuongXaThuongChu = new List<AutoCompleteEntry>();



        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand AddCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        #endregion

        #region Khoi tao
        public ucHoSoNhanVienCT()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            InitEventHandler();

            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);

            cmbLoaiHoSo.Focus();
            
        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.NhanSu;component/HoSo/ucHoSoNhanVienCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void InitEventHandler()
        {
            chkThemNhieuLan.Checked += new RoutedEventHandler(chkThemNhieuLan_Checked);

            btnNguoiQuanLy.Click += new RoutedEventHandler(btnNguoiQuanLy_Click);
            txtNguoiQuanLy.KeyDown += new KeyEventHandler(txtNguoiQuanLy_KeyDown);

            cmbLoaiHoSo.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiHoSo_SelectionChanged);
            cmbTinhThanhHienTai.SelectionChanged += new SelectionChangedEventHandler(cmbTinhThanhHienTai_SelectionChanged);
            cmbQuanHuyenHienTai.SelectionChanged += new SelectionChangedEventHandler(cmbQuanHuyenHienTai_SelectionChanged);
            cmbTinhThanhThuongChu.SelectionChanged += new SelectionChangedEventHandler(cmbTinhThanhThuongChu_SelectionChanged);
            cmbQuanHuyenThuongChu.SelectionChanged += new SelectionChangedEventHandler(cmbQuanHuyenThuongChu_SelectionChanged);

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
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.N, ModifierKeys.Control);
                        key = new KeyBinding(AddCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(ModifyCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.D, ModifierKeys.Control);
                        key = new KeyBinding(DeleteCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift);
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

        private void HoldCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnHold();
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnSave();
        }

        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ResetData();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeModifyFromDetail();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeDelete();
        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeApprove();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeRefuse();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeCancel();
        }

        private void HelpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnHelp();
        }

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnClose();
        }

        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                OnHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetData();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                BeforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
            }
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                OnHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                OnClose();
            }
        }

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                OnHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetData();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                BeforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
            }
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                OnHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                OnClose();
            }
        }

        #endregion

        #region Xu ly Giao dien
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Hiển thị Form khi thêm mới dữ liệu
            if (action == DatabaseConstant.Action.THEM)
            {
                BeforeAddNew();
            }

            //Hiển thị Form khi sửa dữ liệu
            else if (action == DatabaseConstant.Action.SUA)
            {
                BeforeModifyFromList();
            }

            //Hiển thị Form khi xem dữ liệu
            else if (action == DatabaseConstant.Action.XEM)
            {
                BeforeViewFromList();
            }
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

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        private void HideControl()
        {
            try
            {
                HeThong hethong = new HeThong();
                ArrayList arr = new ArrayList();
                arr = hethong.SetVisibleControl("PresentationWPF.HuyDongVon.TatToan.ucTatToanCT", "");
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
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
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
            listLockId.Add(id);

            bool ret = process.UnlockData(DatabaseConstant.Module.NSTL,
                DatabaseConstant.Function.NS_HO_SO_CT,
                DatabaseConstant.Table.NS_HO_SO,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void OnHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }

        /// <summary>
        /// Đóng form
        /// </summary>
        private void OnClose()
        {
            Release();
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        private void chkThemNhieuLan_Checked(object sender, RoutedEventArgs e)
        {
            //action = DatabaseConstant.Action.THEM;
            //id = 0;
            //obj = null;
            //sTrangThaiNVu = "";

            //ResetForm();
            //SetEnabledControls();
            //CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
            //txtTenNhanVien.Focus();

            //chkThemNhieuLan.IsChecked = false;
        }        

        private void txtNguoiQuanLy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnNguoiQuanLy_Click(null, null);
            }
        }

        private void btnNguoiQuanLy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_HOSO_NVIEN.getValue(), lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow row = lstPopup[0];
                    if (!string.IsNullOrWhiteSpace(row[1].ToString()))
                        idNguoiQuanLy = Convert.ToInt32(row[1].ToString());
                    if (!string.IsNullOrWhiteSpace(row[2].ToString()))
                        txtNguoiQuanLy.Text = row[2].ToString();
                    if (!string.IsNullOrWhiteSpace(row[3].ToString()))
                        txtTenNguoiQuanLy.Text = row[3].ToString();

                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                CommonFunction.ThongBaoLoi(ex);
            }
        }

        private void cmbLoaiHoSo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                lstSourceChucVu.Clear();
                cmbChucVu.Items.Clear();

                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();

                string loaiHoSo = lstSourceLoaiHoSo.ElementAt(cmbLoaiHoSo.SelectedIndex).KeywordStrings.ElementAt(0);
                lstDieuKien.Add(loaiHoSo);
                auto.GenAutoComboBox(ref lstSourceChucVu, ref cmbChucVu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NS_CHUC_VU.getValue(), lstDieuKien);
                if(loaiHoSo.Equals(BusinessConstant.LOAI_HO_SO.CONG_TAC_VIEN.layGiaTri()) && ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BINHKHANH.layGiaTri()))
                {
                    grbHocVan.Visibility = System.Windows.Visibility.Collapsed;
                    grbViTriVLam.Visibility = System.Windows.Visibility.Collapsed;
                    lblMaNhanVien.Content = LLanguage.SearchResourceByKey("U.NhanSu.HoSo.ucHoSoNhanVienCT.MaNhanVien");
                    lblTenNhanVien.Content = LLanguage.SearchResourceByKey("U.NhanSu.HoSo.ucHoSoNhanVienCT.TenNhanVien");
                    lblNgayVaoDonVi.Content = LLanguage.SearchResourceByKey("U.NhanSu.HoSo.ucHoSoNhanVienCT.NgayVaoDonVi");
                }
                else if (loaiHoSo.Equals(BusinessConstant.LOAI_HO_SO.CONG_TAC_VIEN.layGiaTri()) && ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.MOM.layGiaTri()))
                {
                    grbHocVan.Visibility = System.Windows.Visibility.Visible;
                    grbViTriVLam.Visibility = System.Windows.Visibility.Visible;
                    lblMaNhanVien.Content = LLanguage.SearchResourceByKey("U.NhanSu.HoSo.ucHoSoNhanVienCT.MaCTV");
                    lblTenNhanVien.Content = LLanguage.SearchResourceByKey("U.NhanSu.HoSo.ucHoSoNhanVienCT.TenCTV");
                    lblNgayVaoDonVi.Content = LLanguage.SearchResourceByKey("U.NhanSu.HoSo.ucHoSoNhanVienCT.NgayCongTac");
                }
                else
                {
                    grbHocVan.Visibility = System.Windows.Visibility.Visible;
                    grbViTriVLam.Visibility = System.Windows.Visibility.Visible;
                    lblMaNhanVien.Content = LLanguage.SearchResourceByKey("U.NhanSu.HoSo.ucHoSoNhanVienCT.MaNhanVien");
                    lblTenNhanVien.Content = LLanguage.SearchResourceByKey("U.NhanSu.HoSo.ucHoSoNhanVienCT.TenNhanVien");
                    lblNgayVaoDonVi.Content = LLanguage.SearchResourceByKey("U.NhanSu.HoSo.ucHoSoNhanVienCT.NgayVaoDonVi");
                }                
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void cmbTinhThanhHienTai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                lstSourceQuanHuyenHienTai.Clear();
                cmbQuanHuyenHienTai.Items.Clear();
                if (cmbTinhThanhHienTai.SelectedIndex >= 0)
                {
                    AutoComboBox auto = new AutoComboBox();
                    List<string> lstDieuKien = new List<string>();

                    string idTinhTP = lstSourceTinhThanhHienTai.ElementAt(cmbTinhThanhHienTai.SelectedIndex).KeywordStrings.ElementAt(1);
                    lstDieuKien.Add(idTinhTP);
                    auto.GenAutoComboBox(ref lstSourceQuanHuyenHienTai, ref cmbQuanHuyenHienTai, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NS_QUAN_HUYEN.getValue(), lstDieuKien);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void cmbQuanHuyenHienTai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                lstSourcePhuongXaHienTai.Clear();
                cmbPhuongXaHienTai.Items.Clear();
                if (cmbQuanHuyenHienTai.SelectedIndex >= 0)
                {
                    AutoComboBox auto = new AutoComboBox();
                    List<string> lstDieuKien = new List<string>();

                    string idTinhTP = lstSourceQuanHuyenHienTai.ElementAt(cmbQuanHuyenHienTai.SelectedIndex).KeywordStrings.ElementAt(1);
                    lstDieuKien.Add(idTinhTP);
                    auto.GenAutoComboBox(ref lstSourcePhuongXaHienTai, ref cmbPhuongXaHienTai, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NS_PHUONG_XA.getValue(), lstDieuKien);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void cmbTinhThanhThuongChu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                lstSourceQuanHuyenThuongChu.Clear();
                cmbQuanHuyenThuongChu.Items.Clear();
                if (cmbTinhThanhThuongChu.SelectedIndex >= 0)
                {
                    AutoComboBox auto = new AutoComboBox();
                    List<string> lstDieuKien = new List<string>();

                    string idTinhTP = lstSourceTinhThanhThuongChu.ElementAt(cmbTinhThanhThuongChu.SelectedIndex).KeywordStrings.ElementAt(1);
                    lstDieuKien.Add(idTinhTP);
                    auto.GenAutoComboBox(ref lstSourceQuanHuyenThuongChu, ref cmbQuanHuyenThuongChu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NS_QUAN_HUYEN.getValue(), lstDieuKien);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void cmbQuanHuyenThuongChu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                lstSourcePhuongXaThuongChu.Clear();
                cmbPhuongXaThuongChu.Items.Clear();
                if (cmbQuanHuyenThuongChu.SelectedIndex >= 0)
                {
                    AutoComboBox auto = new AutoComboBox();
                    List<string> lstDieuKien = new List<string>();

                    string idTinhTP = lstSourceQuanHuyenThuongChu.ElementAt(cmbQuanHuyenThuongChu.SelectedIndex).KeywordStrings.ElementAt(1);
                    lstDieuKien.Add(idTinhTP);
                    auto.GenAutoComboBox(ref lstSourcePhuongXaThuongChu, ref cmbPhuongXaThuongChu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NS_PHUONG_XA.getValue(), lstDieuKien);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void ucQuanHeGD_EditCellEnd(object sender, EventArgs e)
        {
            NS_HO_SO_QHE_GDINH obj = ucQuanHeGD.cellEdit.ParentRow.Item as NS_HO_SO_QHE_GDINH;
            obj.ID_QHE_GDINH = ucQuanHeGD.GiaTri.StringToInt32();
            List<NS_HO_SO_QHE_GDINH> lstQuanHeGiaDinh = grdGiaDinh.ItemsSource as List<NS_HO_SO_QHE_GDINH>;
            lstQuanHeGiaDinh.ElementAt(lstQuanHeGiaDinh.IndexOf(obj)).ID_QHE_GDINH = ucQuanHeGD.GiaTri.StringToInt32();
            grdGiaDinh.ItemsSource = lstQuanHeGiaDinh;
            grdGiaDinh.Rebind();
        }

        #endregion

        #region Xử lý nghiệp vụ
        private void GetFormData(ref NS_HO_SO obj, ref List<NS_HO_SO_QHE_GDINH> lstQuanHeGiaDinh, ref List<NS_HO_SO_TDO_HVAN> lstTrinhDoHocVan, string sTrangThaiNVu)
        {
            try
            {
                obj = new NS_HO_SO();
                lstQuanHeGiaDinh = new List<NS_HO_SO_QHE_GDINH>();
                lstTrinhDoHocVan = new List<NS_HO_SO_TDO_HVAN>();

                #region NS_HO_SO
                obj.ID = id;
                obj.MA_HSO = txtMaNhanVien.Text;
                obj.TEN_HSO = txtTenNhanVien.Text;
                obj.NGAY_HSO = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd").ToString("yyyyMMdd");
                obj.NGAY_BDAU = Convert.ToDateTime(raddtNgayVaoDonVi.Value).ToString("yyyyMMdd");
                obj.HO_TEN = txtTenNhanVien.Text;
                obj.TEN_GOI_KHAC = txtTenNhanVien.Text;
                obj.MA_LOAI_HSO = lstSourceLoaiHoSo[cmbLoaiHoSo.SelectedIndex].KeywordStrings[0];
                obj.ID_GIOI_TINH = Convert.ToInt32(lstSourceGioiTinh[cmbGioiTinh.SelectedIndex].KeywordStrings[1]);
                obj.NGAY_SINH = Convert.ToDateTime(raddtNgaySinh.Value).ToString("yyyyMMdd");
                obj.NOI_SINH = txtNoiSinh.Text;
                obj.NGUYEN_QUAN = txtNoiSinh.Text;
                //obj.DDAN_ANH_HSO =
                obj.ID_BO_PHAN = Convert.ToInt32(lstSourcePhongBan[cmbPhongBan.SelectedIndex].KeywordStrings[1]);
                obj.ID_CHUC_VU = Convert.ToInt32(lstSourceChucVu[cmbChucVu.SelectedIndex].KeywordStrings[1]);
                obj.ID_CBQL_TTIEP = idNguoiQuanLy;
                obj.ID_HTHUC_LVIEC = Convert.ToInt32(lstSourceHinhThucLamViec[cmbHinhThucLamViec.SelectedIndex].KeywordStrings[1]);
                //obj.ID_NQUAN_TINH = 
                obj.ID_QUOC_TICH = Convert.ToInt32(lstSourceQuocTich[cmbQuocTich.SelectedIndex].KeywordStrings[1]);
                obj.ID_TON_GIAO = Convert.ToInt32(lstSourceTonGiao[cmbTonGiao.SelectedIndex].KeywordStrings[1]);
                obj.ID_DAN_TOC = Convert.ToInt32(lstSourceDanToc[cmbDanToc.SelectedIndex].KeywordStrings[1]);
                obj.ID_TTRANG_HNHAN = Convert.ToInt32(lstSourceTinhTrangHonNhan[cmbTrinhTrangHonNhan.SelectedIndex].KeywordStrings[1]);
                //obj.ID_LOAI_GTO = null;
                obj.CMND_SO = txtSoCMND.Text;
                obj.CMND_NGAY_CAP = Convert.ToDateTime(raddtNgayCap.Value).ToString("yyyyMMdd");
                obj.CMND_NGAY_HHAN = Convert.ToDateTime(raddtNgayHetHan.Value).ToString("yyyyMMdd");
                obj.CMND_NOI_CAP = txtNoiCap.Text;
                obj.HO_CHIEU_SO = null;
                obj.HO_CHIEU_NGAY_CAP = null;
                obj.HO_CHIEU_NGAY_HHAN = null;
                obj.HO_CHIEU_NOI_CAP = null;
                obj.ID_TPHAN_GDINH = null;
                obj.ID_TPHAN_BTHAN = null;
                obj.LH_HKHAU_DCHI = txtDiaChiHienTai.Text;
                if (cmbQuanHuyenHienTai.SelectedIndex >= 0)
                    obj.LH_HKHAU_ID_QHUYEN = Convert.ToInt32(lstSourceQuanHuyenHienTai[cmbQuanHuyenHienTai.SelectedIndex].KeywordStrings[1]);
                if (cmbPhuongXaHienTai.SelectedIndex >= 0)
                    obj.LH_HKHAU_ID_PXA = Convert.ToInt32(lstSourcePhuongXaHienTai[cmbPhuongXaHienTai.SelectedIndex].KeywordStrings[1]);
                if (cmbTinhThanhHienTai.SelectedIndex >= 0)
                    obj.LH_HKHAU_ID_TINHTP = Convert.ToInt32(lstSourceTinhThanhHienTai[cmbTinhThanhHienTai.SelectedIndex].KeywordStrings[1]);
                obj.LH_HKHAU_ID_QGIA = null;
                obj.LH_HNAY_DCHI = txtDiaChiThuongChu.Text;
                if (cmbQuanHuyenThuongChu.SelectedIndex >= 0)
                    obj.LH_HNAY_ID_QHUYEN = Convert.ToInt32(lstSourceQuanHuyenThuongChu[cmbQuanHuyenThuongChu.SelectedIndex].KeywordStrings[1]);
                if (cmbPhuongXaThuongChu.SelectedIndex >= 0)
                    obj.LH_HNAY_ID_PXA = Convert.ToInt32(lstSourcePhuongXaThuongChu[cmbPhuongXaThuongChu.SelectedIndex].KeywordStrings[1]);
                if (cmbTinhThanhThuongChu.SelectedIndex >= 0)
                    obj.LH_HNAY_ID_TINHTP = Convert.ToInt32(lstSourceTinhThanhThuongChu[cmbTinhThanhThuongChu.SelectedIndex].KeywordStrings[1]);
                obj.LH_HNAY_ID_QGIA = null;
                obj.LH_DT_DDONG = null;
                obj.LH_DT_CQUAN = null;
                obj.LH_DT_NRIENG = txtSoDienThoai.Text;
                obj.LH_DT_KHAC = null;
                obj.LH_EMAIL_CQUAN = null;
                obj.LH_EMAIL_CNHAN = txtEmail.Text;
                obj.LH_GMAIL = null;
                obj.LH_YAHOO = null;
                obj.LH_SKYPE = null;
                obj.LH_MSN = null;
                obj.LH_HOTMAIL = null;
                obj.LH_GTALK = null;
                obj.LH_KHAN_HOTEN = null;
                obj.LH_KHAN_ID_QHE = null;
                obj.LH_KHAN_DCHI = null;
                obj.LH_KHAN_EMAIL = null;
                obj.LH_KHAN_DDONG = null;
                obj.LH_KHAN_NRIENG = null;
                obj.MST_CNHAN = null;
                obj.MST_CNHAN_NCAP = null;
                obj.SO_TKHOAN = null;
                obj.SO_TKHOAN_NHANG = null;
                obj.THE_BHYT = null;
                obj.THE_BHYT_NOI_CAP = null;
                obj.THE_BHYT_NGAY_CAP = null;
                obj.THE_BHYT_NGAY_HHAN = null;
                obj.THE_BHYT_ID_BKCB = null;
                obj.SO_BHXH = null;
                obj.SO_BHXH_NOI_CAP = null;
                obj.SO_BHXH_NGAY_CAP = null;
                obj.SO_BHXH_NGAY_HHAN = null;
                obj.SO_BHXH_TY_LE = null;
                obj.SO_BHXH_ID_TINHTP = null;

                //Thông tin kiểm soát
                obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.TTHAI_NVU = sTrangThaiNVu;
                obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                obj.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                obj.NGAY_NHAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                obj.NGUOI_NHAP = txtNguoiLap.Text;
                if (action != DatabaseConstant.Action.THEM)
                {
                    obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }
                //obj.TTIN_KHAC = txtTTinKhac.Text;
                #endregion

                #region NS_HO_SO_QHE_GDINH
                lstQuanHeGiaDinh = grdGiaDinh.ItemsSource as List<NS_HO_SO_QHE_GDINH>;
                #endregion

                #region NS_HO_SO_TDO_HVAN
                //Hiện tại chỉ có 1 bản ghi
                NS_HO_SO_TDO_HVAN objTrinhDoHocVan = new NS_HO_SO_TDO_HVAN();
                objTrinhDoHocVan.ID = 0;
                objTrinhDoHocVan.ID_HSO = obj.ID;
                objTrinhDoHocVan.NGAY_HLUC = numNamTotNghiep.Text;
                objTrinhDoHocVan.NGAY_BDAU = obj.NGAY_BDAU;
                objTrinhDoHocVan.NGAY_KTHUC = null;
                objTrinhDoHocVan.ID_TDO_HVAN = Convert.ToInt32(lstSourceTrinhDoHocVan[cmbTrinhDoChuyenMon.SelectedIndex].KeywordStrings[1]);
                objTrinhDoHocVan.ID_HTHUC_DTAO = null;
                objTrinhDoHocVan.ID_TRUONG_DTAO = null;
                objTrinhDoHocVan.ID_KHOA_DTAO = null;
                objTrinhDoHocVan.ID_CNGANH_DTAO = Convert.ToInt32(lstSourceChuyenNganhDaoTao[cmbChuyenNganh.SelectedIndex].KeywordStrings[1]);
                objTrinhDoHocVan.ID_BANG_CAP = null;
                objTrinhDoHocVan.ID_NGHE_NGHIEP = null;
                objTrinhDoHocVan.TTHAI_HVAN = BusinessConstant.TRANG_THAI_MAC_DINH.MAC_DINH.layGiaTri();
                objTrinhDoHocVan.TTHAI_BGHI = obj.TTHAI_BGHI;
                objTrinhDoHocVan.TTHAI_NVU = obj.TTHAI_NVU;
                objTrinhDoHocVan.MA_DVI_QLY = obj.MA_DVI_QLY;
                objTrinhDoHocVan.MA_DVI_TAO = obj.MA_DVI_TAO;
                objTrinhDoHocVan.NGAY_NHAP = obj.NGAY_NHAP;
                objTrinhDoHocVan.NGUOI_NHAP = obj.NGUOI_NHAP;
                objTrinhDoHocVan.NGAY_CNHAT = obj.NGAY_CNHAT;
                objTrinhDoHocVan.NGUOI_CNHAT = obj.NGUOI_CNHAT;

                lstTrinhDoHocVan.Add(objTrinhDoHocVan);
                #endregion
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            NhanSuProcess processNhanSu = new NhanSuProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            NS_TEMP_HO_SO objTempHoSo = null;
            List<NS_HSO_DU_AN_DTO> lstDuAn = null;
            List<NS_HO_SO_QHE_GDINH> lstQuanHeGiaDinh = null;
            List<NS_HO_SO_TDO_HVAN> lstTrinhDoHVan = null;
            List<NS_DM_LOAI_HSO> lstLoaiHoSo = null;
            List<NS_DM_GIOI_TINH> lstGioiTinh = null;
            List<NS_DM_TDO_HVAN> lstTrinhDoHocVan = null;
            List<NS_DM_CNGANH_DTAO> lstChuyenNganhDaoTao = null;
            List<NS_DM_DVI_CTAC> lstDonViCongTac = null;
            List<NS_DM_CHUC_VU> lstChucVu = null;
            List<NS_DM_HTHUC_LVIEC> lstHinhThucLamViec = null;
            List<NS_DM_QUOC_TICH> lstQuocTich = null;
            List<NS_DM_TON_GIAO> lstTonGiao = null;
            List<NS_DM_DAN_TOC> lstDanToc = null;
            List<NS_DM_TTRANG_HNHAN> lstTinhTrangHonNhan = null;
            List<NS_DM_TINH_TP> lstTinhThanhPho = null;
            List<NS_DM_QUAN_HUYEN> lstQuanHuyen = null;
            List<NS_DM_PHUONG_XA> lstPhuongXa = null;
            try
            {
                bool ret = false;
                obj = new NS_HO_SO();
                obj.ID = id;
                ret = processNhanSu.HoSo(DatabaseConstant.Action.LOAD_DATA, ref obj, ref objTempHoSo, ref lstDuAn, ref lstQuanHeGiaDinh, ref lstTrinhDoHVan, ref lstLoaiHoSo, ref lstGioiTinh, ref lstTrinhDoHocVan, ref lstChuyenNganhDaoTao, ref lstDonViCongTac, ref lstChucVu, ref lstHinhThucLamViec, ref lstQuocTich, ref lstTonGiao, ref lstDanToc, ref lstTinhTrangHonNhan, ref lstTinhThanhPho, ref lstQuanHuyen, ref lstPhuongXa, ref listClientResponseDetail);
                if (ret == true)
                {

                    sTrangThaiNVu = obj.TTHAI_NVU;

                    #region Dữ liệu lên combobox
                    lstSourceLoaiHoSo = ConvertNhanSu.ToAutoCompleteEntry(lstLoaiHoSo);
                    lstSourceGioiTinh = ConvertNhanSu.ToAutoCompleteEntry(lstGioiTinh);
                    lstSourceTrinhDoHocVan = ConvertNhanSu.ToAutoCompleteEntry(lstTrinhDoHocVan);
                    lstSourceChuyenNganhDaoTao = ConvertNhanSu.ToAutoCompleteEntry(lstChuyenNganhDaoTao);
                    lstSourcePhongBan = ConvertNhanSu.ToAutoCompleteEntry(lstDonViCongTac);
                    lstSourceChucVu = ConvertNhanSu.ToAutoCompleteEntry(lstChucVu);
                    lstSourceHinhThucLamViec = ConvertNhanSu.ToAutoCompleteEntry(lstHinhThucLamViec);
                    lstSourceQuocTich = ConvertNhanSu.ToAutoCompleteEntry(lstQuocTich);
                    lstSourceTonGiao = ConvertNhanSu.ToAutoCompleteEntry(lstTonGiao);
                    lstSourceDanToc = ConvertNhanSu.ToAutoCompleteEntry(lstDanToc);
                    lstSourceTinhTrangHonNhan = ConvertNhanSu.ToAutoCompleteEntry(lstTinhTrangHonNhan);
                    lstSourceTinhThanhHienTai = ConvertNhanSu.ToAutoCompleteEntry(lstTinhThanhPho);
                    lstSourceTinhThanhThuongChu = ConvertNhanSu.ToAutoCompleteEntry(lstTinhThanhPho);
                    lstSourceQuanHuyenHienTai = ConvertNhanSu.ToAutoCompleteEntry(lstQuanHuyen);
                    lstSourceQuanHuyenThuongChu = ConvertNhanSu.ToAutoCompleteEntry(lstQuanHuyen);
                    lstSourcePhuongXaHienTai = ConvertNhanSu.ToAutoCompleteEntry(lstPhuongXa);
                    lstSourcePhuongXaThuongChu = ConvertNhanSu.ToAutoCompleteEntry(lstPhuongXa);

                    AutoComboBox auto = new AutoComboBox();
                    auto.GenAutoComboBoxBySource(ref lstSourceLoaiHoSo, ref cmbLoaiHoSo);
                    auto.GenAutoComboBoxBySource(ref lstSourceGioiTinh, ref cmbGioiTinh);
                    auto.GenAutoComboBoxBySource(ref lstSourceTrinhDoHocVan, ref cmbTrinhDoChuyenMon);
                    auto.GenAutoComboBoxBySource(ref lstSourceChuyenNganhDaoTao, ref cmbChuyenNganh);
                    auto.GenAutoComboBoxBySource(ref lstSourcePhongBan, ref cmbPhongBan);
                    auto.GenAutoComboBoxBySource(ref lstSourceChucVu, ref cmbChucVu);
                    auto.GenAutoComboBoxBySource(ref lstSourceHinhThucLamViec, ref cmbHinhThucLamViec);
                    auto.GenAutoComboBoxBySource(ref lstSourceQuocTich, ref cmbQuocTich);
                    auto.GenAutoComboBoxBySource(ref lstSourceTonGiao, ref cmbTonGiao);
                    auto.GenAutoComboBoxBySource(ref lstSourceDanToc, ref cmbDanToc);
                    auto.GenAutoComboBoxBySource(ref lstSourceTinhTrangHonNhan, ref cmbTrinhTrangHonNhan);
                    auto.GenAutoComboBoxBySource(ref lstSourceTinhThanhHienTai, ref cmbTinhThanhHienTai);
                    auto.GenAutoComboBoxBySource(ref lstSourceTinhThanhThuongChu, ref cmbTinhThanhThuongChu);
                    auto.GenAutoComboBoxBySource(ref lstSourceQuanHuyenHienTai, ref cmbQuanHuyenHienTai);
                    auto.GenAutoComboBoxBySource(ref lstSourceQuanHuyenThuongChu, ref cmbQuanHuyenThuongChu);
                    auto.GenAutoComboBoxBySource(ref lstSourcePhuongXaHienTai, ref cmbPhuongXaHienTai);
                    auto.GenAutoComboBoxBySource(ref lstSourcePhuongXaThuongChu, ref cmbPhuongXaThuongChu);
                    #endregion

                    #region Thông tin chung
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    cmbLoaiHoSo.SelectedIndex = lstSourceLoaiHoSo.IndexOf(lstSourceLoaiHoSo.FirstOrDefault(i => i.KeywordStrings[0].Equals(obj.MA_LOAI_HSO)));
                    txtMaNhanVien.Text = obj.MA_HSO;
                    if (LDateTime.IsDate(obj.NGAY_BDAU, "yyyyMMdd"))
                        raddtNgayVaoDonVi.Value = LDateTime.StringToDate(obj.NGAY_BDAU, "yyyyMMdd");
                    txtTenNhanVien.Text = obj.TEN_HSO;
                    cmbGioiTinh.SelectedIndex = lstSourceGioiTinh.IndexOf(lstSourceGioiTinh.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.ID_GIOI_TINH.ToString())));
                    txtNoiSinh.Text = obj.NOI_SINH;
                    if (LDateTime.IsDate(obj.NGAY_SINH, "yyyyMMdd"))
                        raddtNgaySinh.Value = LDateTime.StringToDate(obj.NGAY_SINH, "yyyyMMdd");
                    txtSoCMND.Text = obj.CMND_SO;
                    txtNoiCap.Text = obj.CMND_NOI_CAP;
                    if (LDateTime.IsDate(obj.CMND_NGAY_CAP, "yyyyMMdd"))
                        raddtNgayCap.Value = LDateTime.StringToDate(obj.CMND_NGAY_CAP, "yyyyMMdd");
                    if (LDateTime.IsDate(obj.CMND_NGAY_HHAN, "yyyyMMdd"))
                        raddtNgayHetHan.Value = LDateTime.StringToDate(obj.CMND_NGAY_HHAN, "yyyyMMdd");
                    //if(!obj.TTIN_KHAC.IsNullOrEmptyOrSpace())
                    //    txtTTinKhac.Text = obj.TTIN_KHAC;
                    #endregion

                    #region Thông tin dự án
                    if (lstDuAn.Count > 0)
                    {
                        grdTTinDuAn.ItemsSource = lstDuAn;
                        grdTTinDuAn.Rebind();
                    }
                    #endregion

                    #region Thông tin trình độ học vấn
                    if (lstTrinhDoHVan.Count > 0)
                    {
                        NS_HO_SO_TDO_HVAN objTrinhDoHocVan = lstTrinhDoHVan[0];
                        cmbTrinhDoChuyenMon.SelectedIndex = lstSourceTrinhDoHocVan.IndexOf(lstSourceTrinhDoHocVan.FirstOrDefault(i => i.KeywordStrings[1].Equals(objTrinhDoHocVan.ID_TDO_HVAN.ToString())));
                        cmbChuyenNganh.SelectedIndex = lstSourceChuyenNganhDaoTao.IndexOf(lstSourceChuyenNganhDaoTao.FirstOrDefault(i => i.KeywordStrings[1].Equals(objTrinhDoHocVan.ID_CNGANH_DTAO.ToString())));
                        numNamTotNghiep.Text = objTrinhDoHocVan.NGAY_HLUC;
                    }
                    #endregion

                    #region Vị trí việc làm
                    cmbPhongBan.SelectedIndex = lstSourcePhongBan.IndexOf(lstSourcePhongBan.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.ID_BO_PHAN.ToString())));
                    cmbChucVu.SelectedIndex = lstSourceChucVu.IndexOf(lstSourceChucVu.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.ID_CHUC_VU.ToString())));
                    cmbHinhThucLamViec.SelectedIndex = lstSourceHinhThucLamViec.IndexOf(lstSourceHinhThucLamViec.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.ID_HTHUC_LVIEC.ToString())));
                    if (objTempHoSo != null)
                    {
                        idNguoiQuanLy = objTempHoSo.ID_NGUOI_QLY;
                        txtNguoiQuanLy.Text = objTempHoSo.MA_NGUOI_QLY;
                        txtTenNguoiQuanLy.Text = objTempHoSo.TEN_NGUOI_QLY;
                    }
                    #endregion

                    #region Thông tin khác
                    cmbQuocTich.SelectedIndex = lstSourceQuocTich.IndexOf(lstSourceQuocTich.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.ID_QUOC_TICH.ToString())));
                    cmbTonGiao.SelectedIndex = lstSourceTonGiao.IndexOf(lstSourceTonGiao.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.ID_TON_GIAO.ToString())));
                    cmbDanToc.SelectedIndex = lstSourceDanToc.IndexOf(lstSourceDanToc.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.ID_DAN_TOC.ToString())));
                    cmbTrinhTrangHonNhan.SelectedIndex = lstSourceTinhTrangHonNhan.IndexOf(lstSourceTinhTrangHonNhan.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.ID_TTRANG_HNHAN.ToString())));
                    #endregion

                    #region Địa chỉ hiện tại
                    txtDiaChiHienTai.Text = obj.LH_HKHAU_DCHI;
                    cmbTinhThanhHienTai.SelectedIndex = lstSourceTinhThanhHienTai.IndexOf(lstSourceTinhThanhHienTai.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.LH_HKHAU_ID_TINHTP.ToString())));
                    if (obj.LH_HKHAU_ID_QHUYEN != null && obj.LH_HKHAU_ID_QHUYEN > 0)
                        cmbQuanHuyenHienTai.SelectedIndex = lstSourceQuanHuyenHienTai.IndexOf(lstSourceQuanHuyenHienTai.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.LH_HKHAU_ID_QHUYEN.ToString())));
                    if (obj.LH_HKHAU_ID_PXA != null && obj.LH_HKHAU_ID_PXA > 0)
                        cmbPhuongXaHienTai.SelectedIndex = lstSourcePhuongXaHienTai.IndexOf(lstSourcePhuongXaHienTai.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.LH_HKHAU_ID_PXA.ToString())));
                    #endregion

                    #region Hộ khẩu thường chú
                    txtDiaChiThuongChu.Text = obj.LH_HNAY_DCHI;
                    cmbTinhThanhThuongChu.SelectedIndex = lstSourceTinhThanhThuongChu.IndexOf(lstSourceTinhThanhThuongChu.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.LH_HNAY_ID_TINHTP.ToString())));
                    if (obj.LH_HNAY_ID_QHUYEN != null && obj.LH_HNAY_ID_QHUYEN > 0)
                        cmbQuanHuyenThuongChu.SelectedIndex = lstSourceQuanHuyenThuongChu.IndexOf(lstSourceQuanHuyenThuongChu.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.LH_HNAY_ID_QHUYEN.ToString())));
                    if (obj.LH_HNAY_ID_PXA != null && obj.LH_HNAY_ID_PXA > 0)
                        cmbPhuongXaThuongChu.SelectedIndex = lstSourcePhuongXaThuongChu.IndexOf(lstSourcePhuongXaThuongChu.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.LH_HNAY_ID_PXA.ToString())));
                    #endregion

                    #region Thông tin liên hệ
                    txtSoDienThoai.Text = obj.LH_DT_NRIENG;
                    txtEmail.Text = obj.LH_EMAIL_CNHAN;
                    #endregion

                    #region Thông tin gia đình
                    grdGiaDinh.ItemsSource = lstQuanHeGiaDinh;
                    grdGiaDinh.Rebind();
                    #endregion

                    #region Thông tin kiểm soát
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    raddtNgayLap.Value = LDateTime.StringToDate(obj.NGAY_NHAP, "yyyyMMdd");
                    txtNguoiLap.Text = obj.NGUOI_NHAP;
                    if (LDateTime.IsDate(obj.NGAY_CNHAT, "yyyyMMdd") == true)
                        raddtNgayCapNhat.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, "yyyyMMdd");
                    txtNguoiCapNhat.Text = obj.NGUOI_CNHAT;
                    #endregion

                }
                else
                {
                    LMessage.ShowMessage("M_ResponseMessage_HanhDong_LayDuLieuKhongThanhCong", LMessage.MessageBoxType.Warning);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                processNhanSu = null;
                listClientResponseDetail = null;
                lstTrinhDoHocVan = null;
                lstChuyenNganhDaoTao = null;
                lstLoaiHoSo = null;
                lstGioiTinh = null;
                lstDonViCongTac = null;
                lstChucVu = null;
                lstHinhThucLamViec = null;
                lstQuocTich = null;
                lstTonGiao = null;
                lstDanToc = null;
                lstTinhTrangHonNhan = null;
                lstTinhThanhPho = null;
            }
        }

        private void LoadFormData()
        {
            NhanSuProcess processNhanSu = new NhanSuProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            NS_TEMP_HO_SO objTempHoSo = null;
            List<NS_HSO_DU_AN_DTO> lstDuAn = null;
            List<NS_HO_SO_QHE_GDINH> lstQuanHeGiaDinh = null;
            List<NS_HO_SO_TDO_HVAN> lstTrinhDoHVan = null;
            List<NS_DM_LOAI_HSO> lstLoaiHoSo = null;
            List<NS_DM_GIOI_TINH> lstGioiTinh = null;
            List<NS_DM_TDO_HVAN> lstTrinhDoHocVan = null;
            List<NS_DM_CNGANH_DTAO> lstChuyenNganhDaoTao = null;
            List<NS_DM_DVI_CTAC> lstDonViCongTac = null;
            List<NS_DM_CHUC_VU> lstChucVu = null;
            List<NS_DM_HTHUC_LVIEC> lstHinhThucLamViec = null;
            List<NS_DM_QUOC_TICH> lstQuocTich = null;
            List<NS_DM_TON_GIAO> lstTonGiao = null;
            List<NS_DM_DAN_TOC> lstDanToc = null;
            List<NS_DM_TTRANG_HNHAN> lstTinhTrangHonNhan = null;
            List<NS_DM_TINH_TP> lstTinhThanhPho = null;
            List<NS_DM_QUAN_HUYEN> lstQuanHuyen = null;
            List<NS_DM_PHUONG_XA> lstPhuongXa = null;
            try
            {
                bool ret = false;
                ret = processNhanSu.HoSo(DatabaseConstant.Action.LOAD, ref obj, ref objTempHoSo, ref lstDuAn, ref lstQuanHeGiaDinh, ref lstTrinhDoHVan, ref lstLoaiHoSo, ref lstGioiTinh, ref lstTrinhDoHocVan, ref lstChuyenNganhDaoTao, ref lstDonViCongTac, ref lstChucVu, ref lstHinhThucLamViec, ref lstQuocTich, ref lstTonGiao, ref lstDanToc, ref lstTinhTrangHonNhan, ref lstTinhThanhPho, ref lstQuanHuyen, ref lstPhuongXa, ref listClientResponseDetail);
                if (ret == true)
                {
                    lstSourceLoaiHoSo = ConvertNhanSu.ToAutoCompleteEntry(lstLoaiHoSo);
                    lstSourceGioiTinh = ConvertNhanSu.ToAutoCompleteEntry(lstGioiTinh);
                    lstSourceTrinhDoHocVan = ConvertNhanSu.ToAutoCompleteEntry(lstTrinhDoHocVan);
                    lstSourceChuyenNganhDaoTao = ConvertNhanSu.ToAutoCompleteEntry(lstChuyenNganhDaoTao);
                    lstSourcePhongBan = ConvertNhanSu.ToAutoCompleteEntry(lstDonViCongTac);
                    lstSourceChucVu = ConvertNhanSu.ToAutoCompleteEntry(lstChucVu);
                    lstSourceHinhThucLamViec = ConvertNhanSu.ToAutoCompleteEntry(lstHinhThucLamViec);
                    lstSourceQuocTich = ConvertNhanSu.ToAutoCompleteEntry(lstQuocTich);
                    lstSourceTonGiao = ConvertNhanSu.ToAutoCompleteEntry(lstTonGiao);
                    lstSourceDanToc = ConvertNhanSu.ToAutoCompleteEntry(lstDanToc);
                    lstSourceTinhTrangHonNhan = ConvertNhanSu.ToAutoCompleteEntry(lstTinhTrangHonNhan);
                    lstSourceTinhThanhHienTai = ConvertNhanSu.ToAutoCompleteEntry(lstTinhThanhPho);
                    lstSourceTinhThanhThuongChu = ConvertNhanSu.ToAutoCompleteEntry(lstTinhThanhPho);

                    AutoComboBox auto = new AutoComboBox();
                    auto.GenAutoComboBoxBySource(ref lstSourceLoaiHoSo, ref cmbLoaiHoSo);
                    auto.GenAutoComboBoxBySource(ref lstSourceGioiTinh, ref cmbGioiTinh);
                    auto.GenAutoComboBoxBySource(ref lstSourceTrinhDoHocVan, ref cmbTrinhDoChuyenMon);
                    auto.GenAutoComboBoxBySource(ref lstSourceChuyenNganhDaoTao, ref cmbChuyenNganh);
                    auto.GenAutoComboBoxBySource(ref lstSourcePhongBan, ref cmbPhongBan);
                    auto.GenAutoComboBoxBySource(ref lstSourceChucVu, ref cmbChucVu);
                    auto.GenAutoComboBoxBySource(ref lstSourceHinhThucLamViec, ref cmbHinhThucLamViec);
                    auto.GenAutoComboBoxBySource(ref lstSourceQuocTich, ref cmbQuocTich);
                    auto.GenAutoComboBoxBySource(ref lstSourceTonGiao, ref cmbTonGiao);
                    auto.GenAutoComboBoxBySource(ref lstSourceDanToc, ref cmbDanToc);
                    auto.GenAutoComboBoxBySource(ref lstSourceTinhTrangHonNhan, ref cmbTrinhTrangHonNhan);
                    auto.GenAutoComboBoxBySource(ref lstSourceTinhThanhHienTai, ref cmbTinhThanhHienTai);
                    auto.GenAutoComboBoxBySource(ref lstSourceTinhThanhThuongChu, ref cmbTinhThanhThuongChu);

                }
                else
                {
                    LMessage.ShowMessage("M_ResponseMessage_HanhDong_LayDuLieuKhongThanhCong", LMessage.MessageBoxType.Warning);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                processNhanSu = null;
                listClientResponseDetail = null;
                lstTrinhDoHocVan = null;
                lstChuyenNganhDaoTao = null;
                lstLoaiHoSo = null;
                lstGioiTinh = null;
                lstDonViCongTac = null;
                lstChucVu = null;
                lstHinhThucLamViec = null;
                lstQuocTich = null;
                lstTonGiao = null;
                lstDanToc = null;
                lstTinhTrangHonNhan = null;
                lstTinhThanhPho = null;
            }
        }

        private void ResetData()
        {
            action = DatabaseConstant.Action.THEM;
            id = 0;
            obj = null;
            sTrangThaiNVu = "";

            ResetForm();
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
            cmbLoaiHoSo.Focus();

            chkThemNhieuLan.IsChecked = false;
        }

        private void ResetForm()
        {
            #region Thông tin chung
            lblTrangThai.Content = "";
            cmbLoaiHoSo.SelectedIndex = lstSourceLoaiHoSo.IndexOf(lstSourceLoaiHoSo.FirstOrDefault(i => i.KeywordStrings[0].Equals(BusinessConstant.LOAI_HO_SO.TUYEN_DUNG.layGiaTri())));
            txtMaNhanVien.Text = "";
            raddtNgayVaoDonVi.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtTenNhanVien.Text = "";
            cmbGioiTinh.SelectedIndex = 0;
            txtNoiSinh.Text = "";
            raddtNgaySinh.Value = null;
            txtSoCMND.Text = "";
            txtNoiCap.Text = "";
            raddtNgayCap.Value = null;
            raddtNgayHetHan.Value = null;
            #endregion

            #region Thông tin trình độ học vấn
            cmbTrinhDoChuyenMon.SelectedIndex = 0;
            cmbChuyenNganh.SelectedIndex = 0;
            numNamTotNghiep.Text = "";
            #endregion

            #region Vị trí việc làm
            cmbPhongBan.SelectedIndex = 0;
            cmbChucVu.SelectedIndex = 0;
            txtNguoiQuanLy.Text = "";
            txtTenNguoiQuanLy.Text = "";
            cmbHinhThucLamViec.SelectedIndex = 0;
            #endregion

            #region Thông tin khác
            cmbQuocTich.SelectedIndex = 0;
            cmbTonGiao.SelectedIndex = 0;
            cmbDanToc.SelectedIndex = 0;
            cmbTrinhTrangHonNhan.SelectedIndex = 0;
            #endregion

            #region Địa chỉ hiện tại
            txtDiaChiHienTai.Text = "";
            cmbTinhThanhHienTai.SelectedIndex = -1;
            cmbQuanHuyenHienTai.SelectedIndex = -1;
            cmbPhuongXaHienTai.SelectedIndex = -1;
            #endregion

            #region Hộ khẩu thường chú
            txtDiaChiThuongChu.Text = "";
            cmbTinhThanhThuongChu.SelectedIndex = -1;
            cmbQuanHuyenThuongChu.SelectedIndex = -1;
            cmbPhuongXaThuongChu.SelectedIndex = -1;
            #endregion

            #region Thông tin liên hệ
            txtSoDienThoai.Text = "";
            txtEmail.Text = "";
            #endregion

            #region Thông tin kiểm soát
            txtTrangThaiBanGhi.Text = "";
            raddtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            raddtNgayCapNhat.Value = null;
            txtNguoiCapNhat.Text = "";
            #endregion

            #region Focus
            cmbLoaiHoSo.Focus();
            #endregion

        }

        private bool Validation()
        {
            try
            {

                #region Thông tin chung
                if (cmbLoaiHoSo.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblLoaiHoSo.Content.ToString());
                    cmbLoaiHoSo.Focus();
                    return false;
                }
                if (raddtNgayVaoDonVi.Value == null || raddtNgayVaoDonVi.Text.Equals("__/__/____"))
                {
                    CommonFunction.ThongBaoChuaNhap(lblNgayVaoDonVi.Content.ToString());
                    raddtNgayVaoDonVi.Focus();
                    return false;
                }
                if (txtTenNhanVien.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaNhap(lblTenNhanVien.Content.ToString());
                    txtTenNhanVien.Focus();
                    return false;
                }
                if (cmbGioiTinh.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblGioiTinh.Content.ToString());
                    cmbGioiTinh.Focus();
                    return false;
                }
                if (txtNoiSinh.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaNhap(lblNoiSinh.Content.ToString());
                    txtNoiSinh.Focus();
                    return false;
                }
                if (raddtNgaySinh.Value == null || raddtNgaySinh.Text.Equals("__/__/____"))
                {
                    CommonFunction.ThongBaoChuaNhap(lblNgaySinh.Content.ToString());
                    raddtNgaySinh.Focus();
                    return false;
                }
                if (Convert.ToDateTime(raddtNgaySinh.Value).ToString("yyyyMMdd").CompareTo(ClientInformation.NgayLamViecHienTai) >= 0)
                {
                    LMessage.ShowMessage("Ngày sinh không hợp lệ", LMessage.MessageBoxType.Warning);
                    raddtNgaySinh.Focus();
                    return false;
                }
                if (txtSoCMND.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaNhap(lblSoCMND.Content.ToString());
                    txtSoCMND.Focus();
                    return false;
                }
                if (txtNoiCap.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaNhap(lblNoiCap.Content.ToString());
                    txtNoiCap.Focus();
                    return false;
                }
                if (raddtNgayCap.Value == null || raddtNgayCap.Text.Equals("__/__/____"))
                {
                    CommonFunction.ThongBaoChuaNhap(lblNgayCap.Content.ToString());
                    raddtNgayCap.Focus();
                    return false;
                }
                if (Convert.ToDateTime(raddtNgayCap.Value).ToString("yyyyMMdd").CompareTo(ClientInformation.NgayLamViecHienTai) >= 0)
                {
                    LMessage.ShowMessage("Ngày cấp không hợp lệ", LMessage.MessageBoxType.Warning);
                    raddtNgayCap.Focus();
                    return false;
                }
                #endregion

                #region Trình độ học vấn
                if (grbHocVan.Visibility == System.Windows.Visibility.Visible)
                {
                    if (cmbTrinhDoChuyenMon.SelectedIndex < 0)
                    {
                        CommonFunction.ThongBaoChuaChon(lblTrinhDoChuyenMon.Content.ToString());
                        cmbTrinhDoChuyenMon.Focus();
                        return false;
                    }
                    if (cmbChuyenNganh.SelectedIndex < 0)
                    {
                        CommonFunction.ThongBaoChuaChon(lblChuyenNganh.Content.ToString());
                        cmbChuyenNganh.Focus();
                        return false;
                    }
                }
                #endregion

                #region Vị trí việc làm
                if (grbViTriVLam.Visibility == System.Windows.Visibility.Visible)
                {
                    if (cmbPhongBan.SelectedIndex < 0)
                    {
                        CommonFunction.ThongBaoChuaChon(lblPhongBan.Content.ToString());
                        cmbPhongBan.Focus();
                        return false;
                    }
                    if (cmbChucVu.SelectedIndex < 0)
                    {
                        CommonFunction.ThongBaoChuaChon(lblChucVu.Content.ToString());
                        cmbChucVu.Focus();
                        return false;
                    }
                    //if (txtNguoiQuanLy.Text.IsNullOrEmptyOrSpace())
                    //{
                    //    CommonFunction.ThongBaoChuaNhap(lblNguoiQuanLyTrucTiep.Content.ToString());
                    //    txtNguoiQuanLy.Focus();
                    //    return false;
                    //}
                    if (cmbHinhThucLamViec.SelectedIndex < 0)
                    {
                        CommonFunction.ThongBaoChuaChon(lblHinhThucLamViec.Content.ToString());
                        cmbHinhThucLamViec.Focus();
                        return false;
                    }
                }
                #endregion

                #region Thông tin khác
                if (cmbQuocTich.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblQuocTich.Content.ToString());
                    cmbQuocTich.Focus();
                    return false;
                }
                #endregion

                #region Địa chỉ hiện tại
                if (txtDiaChiHienTai.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaNhap(lblDiaChiHienTai.Content.ToString());
                    tbiThongTinLienHe.IsSelected = true;
                    txtDiaChiHienTai.Focus();
                    return false;
                }
                if (cmbTinhThanhHienTai.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblTinhThanhHienTai.Content.ToString());
                    tbiThongTinLienHe.IsSelected = true;
                    cmbTinhThanhHienTai.Focus();
                    return false;
                }
                #endregion

                #region Hộ khẩu thường chú
                if (txtDiaChiThuongChu.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaNhap(lblDiaChiThuongChu.Content.ToString());
                    txtDiaChiThuongChu.Focus();
                    tbiThongTinLienHe.IsSelected = true;
                    return false;
                }
                if (cmbTinhThanhThuongChu.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblTinhThanhThuongChu.Content.ToString());
                    tbiThongTinLienHe.IsSelected = true;
                    cmbTinhThanhThuongChu.Focus();
                    return false;
                }

                if (!LObject.IsNullOrEmpty(txtEmail.Text))
                {
                    if (!CommonFunction.IsValidFormat(ApplicationConstant.FormatType.EMAIL,
                        txtEmail.Text))
                    {
                        txtEmail.Focus();
                        return false;
                    }
                }

                if (!LObject.IsNullOrEmpty(txtSoDienThoai.Text))
                {
                    if (!CommonFunction.IsValidFormat(ApplicationConstant.FormatType.PHONE,
                        txtSoDienThoai.Text))
                    {
                        txtSoDienThoai.Focus();
                        return false;
                    }
                }
                #endregion

                #region NS_HO_SO_QHE_GDINH
                List<NS_HO_SO_QHE_GDINH> lstQuanHeGiaDinh = grdGiaDinh.ItemsSource as List<NS_HO_SO_QHE_GDINH>;
                if (!lstQuanHeGiaDinh.IsNullOrEmpty())
                {
                    foreach (var item in lstQuanHeGiaDinh)
                    {
                        if (item.TEN_TVIEN.IsNullOrEmptyOrSpace() || !item.NGAY_SINH.IsDate("yyyyMMdd") || item.ID_QHE_GDINH.IsNullOrEmpty())
                        {
                            LMessage.ShowMessage("Invalid family information.", LMessage.MessageBoxType.Warning);
                            return false;
                        }
                    }
                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                return false;
            }
        }

        private void SetEnabledControls()
        {
            #region Thêm
            if (action == DatabaseConstant.Action.THEM)
            {
                //Thông tin chung
                cmbLoaiHoSo.IsEnabled = true;
                txtMaNhanVien.IsEnabled = false;
                raddtNgayVaoDonVi.IsEnabled = true;
                txtTenNhanVien.IsEnabled = true;
                cmbGioiTinh.IsEnabled = true;
                txtNoiSinh.IsEnabled = true;
                raddtNgaySinh.IsEnabled = true;
                dtpNgaySinh.IsEnabled = true;
                txtSoCMND.IsEnabled = true;
                txtNoiCap.IsEnabled = true;
                raddtNgayCap.IsEnabled = true;
                dtpNgayCap.IsEnabled = true;
                raddtNgayHetHan.IsEnabled = true;

                //Thông tin trình độ học vấn
                dtpNgayHetHan.IsEnabled = true;
                cmbTrinhDoChuyenMon.IsEnabled = true;
                cmbChuyenNganh.IsEnabled = true;
                numNamTotNghiep.IsEnabled = true;

                //Vị trí việc làm
                cmbPhongBan.IsEnabled = true;
                cmbChucVu.IsEnabled = true;
                txtNguoiQuanLy.IsEnabled = true;
                btnNguoiQuanLy.IsEnabled = true;
                txtTenNguoiQuanLy.IsEnabled = true;
                cmbHinhThucLamViec.IsEnabled = true;

                //Thông tin khác
                cmbQuocTich.IsEnabled = true;
                cmbTonGiao.IsEnabled = true;
                cmbDanToc.IsEnabled = true;
                cmbTrinhTrangHonNhan.IsEnabled = true;

                //Địa chỉ hiện tại
                txtDiaChiHienTai.IsEnabled = true;
                cmbTinhThanhHienTai.IsEnabled = true;
                cmbQuanHuyenHienTai.IsEnabled = true;
                cmbPhuongXaHienTai.IsEnabled = true;
                txtDiaChiThuongChu.IsEnabled = true;
                cmbTinhThanhThuongChu.IsEnabled = true;
                cmbQuanHuyenThuongChu.IsEnabled = true;
                cmbPhuongXaThuongChu.IsEnabled = true;

                //Hộ khẩu thường chú
                txtSoDienThoai.IsEnabled = true;
                txtEmail.IsEnabled = true;

                btnAddGiaDinh.IsEnabled = true;
                btnDeleteGiaDinh.IsEnabled = true;
                grdGiaDinh.IsEnabled = true;
                txtTTinKhac.IsEnabled = true;
            }
            #endregion

            #region Sửa
            else if (action == DatabaseConstant.Action.SUA)
            {
                //Thông tin chung          
                cmbLoaiHoSo.IsEnabled = true;
                txtMaNhanVien.IsEnabled = false;
                raddtNgayVaoDonVi.IsEnabled = true;
                txtTenNhanVien.IsEnabled = true;
                cmbGioiTinh.IsEnabled = true;
                txtNoiSinh.IsEnabled = true;
                raddtNgaySinh.IsEnabled = true;
                dtpNgaySinh.IsEnabled = true;
                txtSoCMND.IsEnabled = true;
                txtNoiCap.IsEnabled = true;
                raddtNgayCap.IsEnabled = true;
                dtpNgayCap.IsEnabled = true;
                raddtNgayHetHan.IsEnabled = true;

                //Thông tin trình độ học vấn
                dtpNgayHetHan.IsEnabled = true;
                cmbTrinhDoChuyenMon.IsEnabled = true;
                cmbChuyenNganh.IsEnabled = true;
                numNamTotNghiep.IsEnabled = true;

                //Vị trí việc làm
                cmbPhongBan.IsEnabled = false;
                cmbChucVu.IsEnabled = false;
                txtNguoiQuanLy.IsEnabled = true;
                btnNguoiQuanLy.IsEnabled = true;
                txtTenNguoiQuanLy.IsEnabled = true;
                cmbHinhThucLamViec.IsEnabled = true;

                //Thông tin khác
                cmbQuocTich.IsEnabled = true;
                cmbTonGiao.IsEnabled = true;
                cmbDanToc.IsEnabled = true;
                cmbTrinhTrangHonNhan.IsEnabled = true;

                //Địa chỉ hiện tại
                txtDiaChiHienTai.IsEnabled = true;
                cmbTinhThanhHienTai.IsEnabled = true;
                cmbQuanHuyenHienTai.IsEnabled = true;
                cmbPhuongXaHienTai.IsEnabled = true;
                txtDiaChiThuongChu.IsEnabled = true;
                cmbTinhThanhThuongChu.IsEnabled = true;
                cmbQuanHuyenThuongChu.IsEnabled = true;
                cmbPhuongXaThuongChu.IsEnabled = true;

                //Hộ khẩu thường chú
                txtSoDienThoai.IsEnabled = true;
                txtEmail.IsEnabled = true;

                btnAddGiaDinh.IsEnabled = true;
                btnDeleteGiaDinh.IsEnabled = true;
                grdGiaDinh.IsEnabled = true;
                txtTTinKhac.IsEnabled = true;
                #region Ngoại lệ
                if (sTrangThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                {
                    cmbLoaiHoSo.IsEnabled = false;
                    cmbPhongBan.IsEnabled = false;
                    cmbChucVu.IsEnabled = false;
                }
                #endregion

            }
            #endregion

            #region Xem ~ Mặc định
            else
            {
                //Thông tin chung
                cmbLoaiHoSo.IsEnabled = false;
                txtMaNhanVien.IsEnabled = false;
                raddtNgayVaoDonVi.IsEnabled = false;
                txtTenNhanVien.IsEnabled = false;
                cmbGioiTinh.IsEnabled = false;
                txtNoiSinh.IsEnabled = false;
                raddtNgaySinh.IsEnabled = false;
                dtpNgaySinh.IsEnabled = false;
                txtSoCMND.IsEnabled = false;
                txtNoiCap.IsEnabled = false;
                raddtNgayCap.IsEnabled = false;
                dtpNgayCap.IsEnabled = false;
                raddtNgayHetHan.IsEnabled = false;

                //Thông tin trình độ học vấn
                dtpNgayHetHan.IsEnabled = false;
                cmbTrinhDoChuyenMon.IsEnabled = false;
                cmbChuyenNganh.IsEnabled = false;
                numNamTotNghiep.IsEnabled = false;

                //Vị trí việc làm
                cmbPhongBan.IsEnabled = false;
                cmbChucVu.IsEnabled = false;
                txtNguoiQuanLy.IsEnabled = false;
                btnNguoiQuanLy.IsEnabled = false;
                txtTenNguoiQuanLy.IsEnabled = false;
                cmbHinhThucLamViec.IsEnabled = false;

                //Thông tin khác
                cmbQuocTich.IsEnabled = false;
                cmbTonGiao.IsEnabled = false;
                cmbDanToc.IsEnabled = false;
                cmbTrinhTrangHonNhan.IsEnabled = false;
                txtTTinKhac.IsEnabled = false;
                //Địa chỉ hiện tại
                txtDiaChiHienTai.IsEnabled = false;
                cmbTinhThanhHienTai.IsEnabled = false;
                cmbQuanHuyenHienTai.IsEnabled = false;
                cmbPhuongXaHienTai.IsEnabled = false;
                txtDiaChiThuongChu.IsEnabled = false;
                cmbTinhThanhThuongChu.IsEnabled = false;
                cmbQuanHuyenThuongChu.IsEnabled = false;
                cmbPhuongXaThuongChu.IsEnabled = false;

                //Hộ khẩu thường chú
                txtSoDienThoai.IsEnabled = false;
                txtEmail.IsEnabled = false;

                btnAddGiaDinh.IsEnabled = false;
                btnDeleteGiaDinh.IsEnabled = false;
                grdGiaDinh.IsEnabled = false;

                #region Ngoại lệ

                #endregion

            }
            #endregion
        }


        public void OnHold()
        {
            List<NS_HO_SO_QHE_GDINH> lstQuanHeGiaDinh = null;
            List<NS_HO_SO_TDO_HVAN> lstTrinhDoHocVan = null;
            try
            {
                if (!Validation()) return;

                string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));
                obj = new NS_HO_SO();
                GetFormData(ref obj, ref lstQuanHeGiaDinh, ref lstTrinhDoHocVan, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(obj, lstQuanHeGiaDinh, lstTrinhDoHocVan);
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(obj, lstQuanHeGiaDinh, lstTrinhDoHocVan);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                lstQuanHeGiaDinh = null;
                lstTrinhDoHocVan = null;
            }
        }

        public void OnSave()
        {
            List<NS_HO_SO_QHE_GDINH> lstQuanHeGiaDinh = null;
            List<NS_HO_SO_TDO_HVAN> lstTrinhDoHocVan = null;
            try
            {
                if (!Validation()) return;

                string trangThai = "";
                if (sTrangThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                else
                    trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));

                obj = new NS_HO_SO();

                GetFormData(ref obj, ref lstQuanHeGiaDinh, ref lstTrinhDoHocVan, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(obj, lstQuanHeGiaDinh, lstTrinhDoHocVan);
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(obj, lstQuanHeGiaDinh, lstTrinhDoHocVan);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                lstQuanHeGiaDinh = null;
                lstTrinhDoHocVan = null;
            }
        }

        public void BeforeViewFromDetail()
        {
            action = DatabaseConstant.Action.XEM;
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void BeforeViewFromList()
        {
            SetFormData();
            BeforeViewFromDetail();
        }


        public void BeforeAddNew()
        {
            LoadFormData();
            action = DatabaseConstant.Action.THEM;
            ResetForm();
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);

        }

        public void OnAddNew(NS_HO_SO obj, List<NS_HO_SO_QHE_GDINH> lstQuanHeGiaDinh, List<NS_HO_SO_TDO_HVAN> lstTrinhDoHocVan)
        {

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                NhanSuProcess processNhanSu = new NhanSuProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processNhanSu.HoSo(DatabaseConstant.Action.THEM, ref obj, lstQuanHeGiaDinh, lstTrinhDoHocVan, ref listClientResponseDetail);
                AfterAddNew(ret, obj, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public void AfterAddNew(bool ret, NS_HO_SO obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                    if (chkThemNhieuLan.IsChecked == true)
                    {
                        ResetData();
                    }
                    else
                    {
                        id = obj.ID;
                        txtMaNhanVien.Text = obj.MA_HSO;
                        sTrangThaiNVu = obj.TTHAI_NVU;
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);

                        BeforeViewFromDetail();
                    }
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        public void BeforeModifyFromDetail()
        {
            try
            {
                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);

                bool ret = process.LockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_HO_SO_CT,
                    DatabaseConstant.Table.NS_HO_SO,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                if (ret)
                {
                    action = DatabaseConstant.Action.SUA;
                    SetEnabledControls();
                    CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
                }
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void BeforeModifyFromList()
        {
            SetFormData();
            action = DatabaseConstant.Action.SUA;
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnModify(NS_HO_SO obj, List<NS_HO_SO_QHE_GDINH> lstQuanHeGiaDinh, List<NS_HO_SO_TDO_HVAN> lstTrinhDoHocVan)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                NhanSuProcess processNhanSu = new NhanSuProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processNhanSu.HoSo(DatabaseConstant.Action.SUA, ref obj, lstQuanHeGiaDinh, lstTrinhDoHocVan, ref listClientResponseDetail);
                AfterModify(ret, obj, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public void AfterModify(bool ret, NS_HO_SO obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    raddtNgayCapNhat.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                    txtNguoiCapNhat.Text = ClientInformation.TenDangNhap;

                    BeforeViewFromDetail();
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_HO_SO_CT,
                    DatabaseConstant.Table.NS_HO_SO,
                    DatabaseConstant.Action.SUA,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        public void BeforeDelete()
        {
            try
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.NSTL,
                        DatabaseConstant.Function.NS_HO_SO_CT,
                        DatabaseConstant.Table.NS_HO_SO,
                        DatabaseConstant.Action.XOA,
                        listLockId);

                    if (retLockData)
                    {
                        action = DatabaseConstant.Action.XOA;
                        OnDelete();
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_HO_SO_CT,
                    DatabaseConstant.Table.NS_HO_SO,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnDelete()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            NhanSuProcess processNhanSu = new NhanSuProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processNhanSu.HoSo(action, ref obj, null, null, ref listClientResponseDetail);
                AfterDelete(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                listClientResponseDetail = null;
                processNhanSu = null;
            }
        }

        public void AfterDelete(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_HO_SO_CT,
                    DatabaseConstant.Table.NS_HO_SO,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                // Đóng cửa sổ chi tiết sau khi xóa
                OnClose();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        public void BeforeApprove()
        {
            try
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.NSTL,
                        DatabaseConstant.Function.NS_HO_SO_CT,
                        DatabaseConstant.Table.NS_HO_SO,
                        DatabaseConstant.Action.DUYET,
                        listLockId);

                    if (retLockData)
                    {
                        action = DatabaseConstant.Action.DUYET;
                        OnApprove();
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnApprove()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            NhanSuProcess processNhanSu = new NhanSuProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processNhanSu.HoSo(action, ref obj, null, null, ref listClientResponseDetail);
                AfterApprove(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                listClientResponseDetail = null;
                processNhanSu = null;
            }
        }

        public void AfterApprove(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_HO_SO_CT,
                    DatabaseConstant.Table.NS_HO_SO,
                    DatabaseConstant.Action.DUYET,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        public void BeforeCancel()
        {
            try
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.NSTL,
                        DatabaseConstant.Function.NS_HO_SO_CT,
                        DatabaseConstant.Table.NS_HO_SO,
                        DatabaseConstant.Action.THOAI_DUYET,
                        listLockId);

                    if (retLockData)
                    {
                        action = DatabaseConstant.Action.THOAI_DUYET;
                        OnCancel();
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnCancel()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            NhanSuProcess processNhanSu = new NhanSuProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processNhanSu.HoSo(action, ref obj, null, null, ref listClientResponseDetail);
                AfterCancel(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                listClientResponseDetail = null;
                processNhanSu = null;
            }
        }

        public void AfterCancel(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_HO_SO_CT,
                    DatabaseConstant.Table.NS_HO_SO,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listLockId);

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }


        public void BeforeRefuse()
        {
            try
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.NSTL,
                        DatabaseConstant.Function.NS_HO_SO_CT,
                        DatabaseConstant.Table.NS_HO_SO,
                        DatabaseConstant.Action.TU_CHOI_DUYET,
                        listLockId);

                    if (retLockData)
                    {
                        action = DatabaseConstant.Action.TU_CHOI_DUYET;
                        OnRefuse();
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnRefuse()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            NhanSuProcess processNhanSu = new NhanSuProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processNhanSu.HoSo(action, ref obj, null, null, ref listClientResponseDetail);
                AfterRefuse(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                listClientResponseDetail = null;
                processNhanSu = null;
            }
        }

        public void AfterRefuse(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_HO_SO_CT,
                    DatabaseConstant.Table.NS_HO_SO,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        #endregion

    }
}
