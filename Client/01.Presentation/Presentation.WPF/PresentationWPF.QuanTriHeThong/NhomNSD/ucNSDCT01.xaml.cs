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
using Presentation.Process.QuanTriHeThongServiceRef;
using Telerik.Windows.Controls;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

namespace PresentationWPF.QuanTriHeThong.NhomNSD
{
    /// <summary>
    /// Interaction logic for ucNSDCT01.xaml
    /// </summary>
    public partial class ucNSDCT01 : UserControl
    {
        #region Khai bao

        private int id = 0;
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

        public event EventHandler OnSavingCompleted;

        public string formCase = null;
        public HT_NSD obj;
        List<HT_NHNSD> dsNHNSD = new List<HT_NHNSD>();
        List<PhamViDto> dsPhamViPhongGD = new List<PhamViDto>();
        List<HT_TRUY_CAP> lstTruyCap = new List<HT_TRUY_CAP>();
        DataTable dt = new DataTable();
        DataTable dtPhongGD = new DataTable();
        QuanTriHeThongProcess qtht = new QuanTriHeThongProcess();
        public DatabaseConstant.Action luuAction;
        private bool chiXem = false;
        List<DataRow> lstPopup = new List<DataRow>();
        List<DataRow> lstPopupPhongGD = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        public void LayDuLieuTuPopupPhongGD(List<DataRow> lst)
        {
            lstPopupPhongGD = lst;
        }
        private bool isLoaded = false;
        private DataSet dsTruyCap;

        private string MAC = BusinessConstant.LOAI_DIA_CHI.MAC.layGiaTri();
        private string IP = BusinessConstant.LOAI_DIA_CHI.IP.layGiaTri();

        public bool ChiXem
        {
            get { return chiXem; }
            set { chiXem = value; }
        }

        List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhanLoai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceGioiTinh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTanSuat = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTinhTrang = new List<AutoCompleteEntry>();

        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        #endregion

        #region Khoi tao

        public ucNSDCT01()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.QuanTriHeThong;component/NhomNSD/ucNSDCT01.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindShortkey();
            KhoiTaoGridTruyCap();
            tabNSDCT.Focus();
            cmbPhanCap.Focus();
        }

        /// <summary>
        /// Đưa form về trạng thái mặc định
        /// </summary>
        private void ResetForm()
        {
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
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
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

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onSave();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeModify();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeDelete();
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
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                onSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                beforeModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                beforeDelete();
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
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                onSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                beforeModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                beforeDelete();
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
            if (e.Key == Key.Escape)
            {
                Release();
                if (OnSavingCompleted != null)
                {
                    OnSavingCompleted(this, EventArgs.Empty);
                }
                PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            }
            else
            {
                CustomControl.CommonFunction.SelectNextControl(e);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            try
            {
                if (!isLoaded)
                {
                    if (luuAction == DatabaseConstant.Action.XEM)
                        formCase = "XEM";
                    else if (luuAction == DatabaseConstant.Action.SUA)
                        formCase = "SUA";
                    else if (luuAction == DatabaseConstant.Action.THEM)
                        formCase = "MANAGE";

                    string strTrangThai = string.Empty;
                    if (formCase == null)
                    {
                        formCase = ClientInformation.FormCase;
                    }
                    if (formCase.Equals("PROFILE"))
                        obj = qtht.layThongTinCaNhan();
                    if (obj != null)
                    {
                        id = obj.ID;
                        tthaiNvu = obj.TTHAI_NVU;

                        //lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(obj.TTHAI_NVU);
                        lblTrangThai.Content = BusinessConstant.layNgonNguTrangThaiNguoiDung(obj.TINH_TRANG);
                        txtTenDangNhap.Text = obj.MA_DANG_NHAP;                        
                        txtNhanVien.Text = obj.MA_HSO;
                        if (!txtNhanVien.Text.IsNullOrEmptyOrSpace())
                            chkLayThongTin.IsChecked = true;
                        txtTenGoi.Text = obj.TEN_GOI;
                        txtTenDem.Text = obj.TEN_HO_DEM;
                        txtEmail.Text = obj.EMAIL;
                        if (!string.IsNullOrEmpty(obj.NGAY_HIEU_LUC) && obj.NGAY_HIEU_LUC != "")
                            raddtNgayHieuLuc.Value = LDateTime.StringToDate(obj.NGAY_HIEU_LUC, "yyyyMMdd");
                        if (!string.IsNullOrEmpty(obj.NGAY_HET_HAN) && obj.NGAY_HET_HAN != "")
                            raddtNgayHetHan.Value = LDateTime.StringToDate(obj.NGAY_HET_HAN, "yyyyMMdd");
                        if (!obj.DIEN_THOAI.IsNullOrEmptyOrSpace())
                            txtDienThoai.Text = obj.DIEN_THOAI;
                        if (!obj.NGAY_SINH.IsNullOrEmptyOrSpace() && obj.NGAY_SINH.IsDate(ApplicationConstant.defaultDateTimeFormat))
                            raddtNgaySinh.Value = obj.NGAY_SINH.StringToDate(ApplicationConstant.defaultDateTimeFormat);
                        // khởi tạo combobox
                        AutoComboBox auto = new AutoComboBox();
                        // Khởi tạo điều kiện gọi danh mục
                        List<string> lstDieuKien = new List<string>();
                        // khởi tạo combobox
                        auto = new AutoComboBox();
                        lstDieuKien = new List<string>();
                        lstDieuKien.Add(DatabaseConstant.DanhMuc.PHAN_LOAI_NGUOI_SU_DUNG.getValue());
                        auto.GenAutoComboBox(ref lstSourcePhanLoai, ref cmbPhanCap, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, obj.PHAN_LOAI_NSD);

                        // khởi tạo combobox
                        auto = new AutoComboBox();
                        lstDieuKien = new List<string>();
                        lstDieuKien.Add(DatabaseConstant.DanhMuc.GIOI_TINH.getValue());
                        auto.GenAutoComboBox(ref lstSourceGioiTinh, ref cmbGioiTinh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, obj.GIOI_TINH);

                        // khởi tạo combobox
                        auto = new AutoComboBox();
                        lstDieuKien = new List<string>();
                        lstDieuKien.Add(DatabaseConstant.DanhMuc.TAN_SUAT.getValue());
                        auto.GenAutoComboBox(ref lstSourceTanSuat, ref cmbDoiMatKhau, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, obj.TGIAN_DOI_DVI_TINH);

                        // khởi tạo combobox
                        auto = new AutoComboBox();
                        lstDieuKien = new List<string>();
                        lstDieuKien.Add(DatabaseConstant.DanhMuc.TRANG_THAI_NGUOI_DUNG.getValue());
                        auto.GenAutoComboBox(ref lstSourceTinhTrang, ref cmbTinhTrang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, obj.TINH_TRANG);

                        // khởi tạo combobox
                        auto = new AutoComboBox();
                        //auto.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVI.getValue(), null, ClientInformation.MaDonVi);
                        lstDieuKien = new List<string>();
                        lstDieuKien.Add("'" + DatabaseConstant.ToChucDonVi.HSO.getValue() + "', '" + DatabaseConstant.ToChucDonVi.CNH.getValue() + "'");
                        auto.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOLOAI.getValue(), lstDieuKien, obj.MA_DVI_QLY);

                        if (obj.TGIAN_DOI_DVI_TINH != null &&
                            obj.TGIAN_DOI_DVI_TINH.Equals(BusinessConstant.TAN_SUAT.NA.layGiaTri()) &&
                            obj.TGIAN_DOI_MKHAU == 0)
                        {
                            // Ẩn control
                            lblDoiMatKhau.Visibility = Visibility.Hidden;
                            telnumDoiMatKhau.Visibility = Visibility.Hidden;
                            cmbDoiMatKhau.Visibility = Visibility.Hidden;

                            chkMatKhauKhongHetHan.IsChecked = true;
                        }
                        else
                        {
                            // Hiện control
                            lblDoiMatKhau.Visibility = Visibility.Visible;
                            telnumDoiMatKhau.Visibility = Visibility.Visible;
                            cmbDoiMatKhau.Visibility = Visibility.Visible;

                            telnumDoiMatKhau.Value = obj.TGIAN_DOI_MKHAU;
                            cmbDoiMatKhau.SelectedIndex = lstSourceTanSuat.IndexOf(lstSourceTanSuat.FirstOrDefault(item => item.KeywordStrings.First().Equals(obj.TGIAN_DOI_DVI_TINH))); ;

                            chkMatKhauKhongHetHan.IsChecked = false;
                        }

                        if (obj.TDOI_MKHAU.Equals(BusinessConstant.YeuCauDoiMatKhau.CHUA_THAY_DOI.layGiaTri()))
                        {
                            chkYeuCauDoiMatKhau.IsChecked = true;
                        }

                        if (obj.HAN_CHE_TRUY_CAP.Equals(BusinessConstant.CoKhong.CO.layGiaTri()))
                            chkHoatDong.IsChecked = true;
                        else
                            chkHoatDong.IsChecked = false;

                        txtTrangThai.Text = BusinessConstant.layNgonNguTrangThaiNguoiDung(obj.TINH_TRANG);
                        if (!string.IsNullOrEmpty(obj.NGAY_NHAP) && obj.NGAY_NHAP != "")
                            raddtNgayNhap.Value = LDateTime.StringToDate(obj.NGAY_NHAP, "yyyyMMdd");
                        else
                            raddtNgayNhap.Value = null;
                        txtNguoiLap.Text = obj.NGUOI_NHAP;
                        txtNguoiCapNhat.Text = obj.NGUOI_CNHAT;
                        if (!string.IsNullOrEmpty(obj.NGAY_CNHAT) && obj.NGAY_CNHAT != "")
                            raddtNgayCNhat.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, "yyyyMMdd");
                        else
                            raddtNgayCNhat.Value = null;

                        BuildGridDoiTuong();
                        loadWidthColumnDoiTuong();
                        BuildGridDoiTuongPhongGD();
                        loadWidthColumnDoiTuongPhongGD();
                        strTrangThai = obj.TTHAI_NVU;

                        // Y\Thông tin lấy từ nhân sự
                        if (obj.MA_HSO == null || obj.MA_HSO == "")
                        {
                            chkLayThongTin.IsChecked = false;
                        }
                        else
                        {
                            chkLayThongTin.IsChecked = true;
                        }
                        chkLayThongTin.IsEnabled = false;
                        txtNhanVien.IsEnabled = false;
                        btnNhanVien.IsEnabled = false;
                    }
                    else
                    {
                        //raddtNgayHieuLuc.Value = LDateTime.StringToDate((DateTime.Today).ToString("yyyyMMdd"), "yyyyMMdd");
                        raddtNgayHieuLuc.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                        // khởi tạo combobox
                        AutoComboBox auto = new AutoComboBox();
                        // Khởi tạo điều kiện gọi danh mục
                        List<string> lstDieuKien = new List<string>();
                        // khởi tạo combobox
                        auto = new AutoComboBox();
                        lstDieuKien = new List<string>();
                        lstDieuKien.Add(DatabaseConstant.DanhMuc.PHAN_LOAI_NGUOI_SU_DUNG.getValue());
                        auto.GenAutoComboBox(ref lstSourcePhanLoai, ref cmbPhanCap, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri());

                        // khởi tạo combobox
                        auto = new AutoComboBox();
                        lstDieuKien = new List<string>();
                        lstDieuKien.Add(DatabaseConstant.DanhMuc.GIOI_TINH.getValue());
                        auto.GenAutoComboBox(ref lstSourceGioiTinh, ref cmbGioiTinh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, "NAM");

                        // Nếu người dùng là quản trị/tác nghiệp tại đơn vị >> cập nhật list loại người dùng
                        // quản trị tại đơn vị chỉ quản lý người dùng tại đơn vị mình
                        // disable thông tin đơn vị
                        if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()) ||
                            ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()))
                        {
                            auto.removeEntry(ref lstSourcePhanLoai, ref cmbPhanCap, BusinessConstant.LoaiNguoiSuDung.CAP_SA.layGiaTri());
                            auto.removeEntry(ref lstSourcePhanLoai, ref cmbPhanCap, BusinessConstant.LoaiNguoiSuDung.CAP_QTTW.layGiaTri());
                            auto.removeEntry(ref lstSourcePhanLoai, ref cmbPhanCap, BusinessConstant.LoaiNguoiSuDung.CAP_NVTW.layGiaTri());

                            cmbDonVi.IsEnabled = false;
                        }
                        else if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTTW.layGiaTri()))
                        {
                            auto.removeEntry(ref lstSourcePhanLoai, ref cmbPhanCap, BusinessConstant.LoaiNguoiSuDung.CAP_SA.layGiaTri());
                        }
                        else if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_SA.layGiaTri()) && !formCase.Equals("PROFILE"))
                        {
                            auto.removeEntry(ref lstSourcePhanLoai, ref cmbPhanCap, BusinessConstant.LoaiNguoiSuDung.CAP_SA.layGiaTri());
                        }                        
                        cmbPhanCap.SelectedIndex = lstSourcePhanLoai.IndexOf(lstSourcePhanLoai.FirstOrDefault(i => i.KeywordStrings.First().Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri())));

                        // khởi tạo combobox
                        auto = new AutoComboBox();
                        lstDieuKien = new List<string>();
                        lstDieuKien.Add(DatabaseConstant.DanhMuc.TAN_SUAT.getValue());
                        auto.GenAutoComboBox(ref lstSourceTanSuat, ref cmbDoiMatKhau, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

                        // khởi tạo combobox
                        auto = new AutoComboBox();
                        lstDieuKien = new List<string>();
                        lstDieuKien.Add(DatabaseConstant.DanhMuc.TRANG_THAI_NGUOI_DUNG.getValue());
                        auto.GenAutoComboBox(ref lstSourceTinhTrang, ref cmbTinhTrang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

                        // khởi tạo combobox
                        auto = new AutoComboBox();
                        //auto.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVI.getValue(), null, ClientInformation.MaDonVi);
                        lstDieuKien = new List<string>();
                        lstDieuKien.Add("'" + DatabaseConstant.ToChucDonVi.HSO.getValue() + "', '" + DatabaseConstant.ToChucDonVi.CNH.getValue() + "'");
                        auto.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOLOAI.getValue(), lstDieuKien, ClientInformation.MaDonVi);

                        lblTrangThai.Content = string.Empty;
                        txtTrangThai.Text = string.Empty;
                        raddtNgayNhap.Value = null;
                        txtNguoiLap.Text = string.Empty;
                        raddtNgayCNhat.Value = null;
                        txtNguoiCapNhat.Text = string.Empty;
                        strTrangThai = string.Empty;

                        // Enable thông tin lấy từ nhân sự
                        chkLayThongTin.IsChecked = true;
                        txtNhanVien.IsEnabled = true;
                        btnNhanVien.IsEnabled = true;
                    }
                    CommonFunction.RefreshButton(Toolbar, luuAction, strTrangThai, mnuMain, DatabaseConstant.Function.HT_NSD);

                    //if (luuAction != DatabaseConstant.Action.THEM)
                    //    cbMultiAdd.Visibility = Visibility.Collapsed;

                    ((RadTabItem)tabNSDCT.Items[0]).IsSelected = true;
                    tabNSDCT.SelectionChanged += tabNSDCT_SelectionChanged;

                    //tlbSave.IsEnabled = true;

                    LoadGridTruyCap();

                    tabNSDCT.Focus();
                    cmbPhanCap.Focus();
                    isLoaded = true;
                }
                HideControl();

                // Nếu người dùng là quản trị tại đơn vị hoặc đang ở trạng thái xem >> disable thông tin đơn vị
                if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()) ||
                    ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()) ||
                    luuAction == DatabaseConstant.Action.XEM)
                {
                    cmbDonVi.IsEnabled = false;
                }
                
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void HideControl()
        {
            this.Cursor = Cursors.Wait;
            try
            {
                if (!string.IsNullOrEmpty(formCase))
                {
                    HeThong hethong = new HeThong();
                    ArrayList arr = new ArrayList();
                    arr = hethong.SetVisibleControl("PresentationWPF.QuanTriHeThong.NhomNSD.ucNSDCT01", formCase);
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
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void BuildGridDoiTuong()
        {
            this.Cursor = Cursors.Wait;
            try
            {
                dt = new DataTable();
                dsNHNSD = new List<HT_NHNSD>();
                dsNHNSD = qtht.layNhomTheoNSD(obj);
                // Tạo source thông tin đối tượng
                dt = new DataTable();
                dt.Columns.Add(LLanguage.SearchResourceByKey("U.DungChung.STT"), typeof(string));
                dt.Columns.Add("ID", typeof(string));
                dt.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.NhomNSD.ucNSDCT01.Ma"), typeof(string));
                dt.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.NhomNSD.ucNSDCT01.Ten"), typeof(string));
                int stt = 0;
                foreach (var item in dsNHNSD)
                {
                    DataRow r = dt.NewRow();
                    stt = stt + 1;
                    r[0] = stt;
                    r[1] = item.ID;
                    r[2] = item.MA_NHNSD;
                    r[3] = item.TEN_NHNSD;
                    dt.Rows.Add(r);
                }
                // đổ source lên lưới
                grDSDoiTuong.ItemsSource = dt;
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void BuildGridDoiTuongPhongGD()
        {
            this.Cursor = Cursors.Wait;
            try
            {
                dtPhongGD = new DataTable();
                dsPhamViPhongGD = new List<PhamViDto>();
                dsPhamViPhongGD = qtht.layPhamViPhongGDTheoNSD(obj, BusinessConstant.LoaiPhamViDuLieu.PHONG_GIAO_DICH.layMaPhamVi());
                // Tạo source thông tin đối tượng
                dtPhongGD = new DataTable();
                dtPhongGD.Columns.Add(LLanguage.SearchResourceByKey("U.DungChung.STT"), typeof(string));
                dtPhongGD.Columns.Add("ID", typeof(string));
                dtPhongGD.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.NhomNSD.ucNSDCT01.Ma"), typeof(string));
                dtPhongGD.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.NhomNSD.ucNSDCT01.Ten"), typeof(string));
                int stt = 0;
                foreach (var item in dsPhamViPhongGD)
                {
                    DataRow r = dtPhongGD.NewRow();
                    stt = stt + 1;
                    r[0] = stt;
                    r[1] = item.IdPvi;
                    r[2] = item.MaPvi;
                    r[3] = item.TenPvi;
                    dtPhongGD.Rows.Add(r);
                }
                // đổ source lên lưới
                grDSPhongGD.ItemsSource = dtPhongGD;
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void grDSDoiTuong_Loaded(object sender, RoutedEventArgs e)
        {
            loadWidthColumnDoiTuong();
        }

        private void loadWidthColumnDoiTuong()
        {
            this.Cursor = Cursors.Wait;
            try
            {
                for (int i = 0; i < grDSDoiTuong.Columns.Count; i++)
                {
                    if (i == 2)
                        grDSDoiTuong.Columns[i].IsVisible = false;
                    else if (i == 1)
                        grDSDoiTuong.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(40, Telerik.Windows.Controls.GridViewLengthUnitType.Pixel);
                    else if (i == 3)
                        grDSDoiTuong.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Auto);
                    else if (i == 4)
                        grDSDoiTuong.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Star);
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void grDSPhongGD_Loaded(object sender, RoutedEventArgs e)
        {
            loadWidthColumnDoiTuongPhongGD();
        }

        private void loadWidthColumnDoiTuongPhongGD()
        {
            this.Cursor = Cursors.Wait;
            try
            {
                for (int i = 0; i < grDSPhongGD.Columns.Count; i++)
                {
                    if (i == 2)
                        grDSPhongGD.Columns[i].IsVisible = false;
                    else if (i == 1)
                        grDSPhongGD.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(40, Telerik.Windows.Controls.GridViewLengthUnitType.Pixel);
                    else if (i == 3)
                        grDSPhongGD.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Auto);
                    else if (i == 4)
                        grDSPhongGD.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Star);
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void tabNSDCT_SelectionChanged(object sender, Telerik.Windows.Controls.RadSelectionChangedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            try
            {
                if (tabNSDCT.SelectedIndex == 0)
                {
                    if (tabNSDCT != null)
                    {
                        UpdateLayout();
                        //txtMaDonVi.Focus();
                        cmbDonVi.Focus();
                    }
                }
                else if (tabNSDCT.SelectedIndex == 3)
                {
                    if (txtTrangThai != null)
                    {
                        UpdateLayout();
                        txtTrangThai.Focus();
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private DataTable LayMacIP()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("NAME");
            dt.Rows.Add(MAC, MAC);
            dt.Rows.Add(IP, IP);
            return dt;
        }

        private void KhoiTaoGridTruyCap()
        {
            DataTable dtTruyCap = new DataTable();
            dtTruyCap.Columns.Add("STT", typeof(int));
            dtTruyCap.Columns.Add("DIA_CHI", typeof(string));
            dtTruyCap.Columns.Add("LOAI_DIA_CHI", typeof(string));
            dtTruyCap.Columns.Add("KICH_HOAT", typeof(bool));
            dsTruyCap = new DataSet();
            dsTruyCap.Tables.Add(dtTruyCap);
        }

        private void LoadGridTruyCap()
        {
            if (luuAction == DatabaseConstant.Action.SUA || luuAction == DatabaseConstant.Action.XEM || formCase.Equals("PROFILE"))
            {
                List<HT_TRUY_CAP> lstTruyCap = new List<HT_TRUY_CAP>();
                lstTruyCap = qtht.layTruyCapTheoNSD(obj);
                if (lstTruyCap != null && lstTruyCap.Count > 0)
                {
                    for (int i = 0; i < lstTruyCap.Count; i++)
                    {
                        DataRow dr = dsTruyCap.Tables[0].NewRow();
                        dr["STT"] = i + 1;
                        dr["DIA_CHI"] = lstTruyCap[i].DIA_CHI;
                        dr["LOAI_DIA_CHI"] = lstTruyCap[i].LOAI_DIA_CHI;

                        if (lstTruyCap[i].KICH_HOAT.Equals(BusinessConstant.CoKhong.CO.layGiaTri()))
                            dr["KICH_HOAT"] = true;
                        else
                            dr["KICH_HOAT"] = false;

                        dsTruyCap.Tables[0].Rows.Add(dr);
                    }
                }

            }

            if (dsTruyCap != null)
            {
                grdTruyCap.DataContext = dsTruyCap.Tables[0].DefaultView;
                ((GridViewComboBoxColumn)grdTruyCap.Columns[3]).ItemsSource = LayMacIP().DefaultView;
            }
        }

        private void btnAddTruyCap_Click(object sender, RoutedEventArgs e)
        {
            int stt = dsTruyCap.Tables[0].Rows.Count;
            dsTruyCap.Tables[0].Rows.Add(stt + 1, "", MAC, true);
            grdTruyCap.DataContext = dsTruyCap.Tables[0].DefaultView;
            grdTruyCap.Focus();
        }

        private void btnDeleteTruyCap_Click(object sender, RoutedEventArgs e)
        {
            List<DataRowView> lstSelected = new List<DataRowView>();
            for (int i = 0; i < grdTruyCap.SelectedItems.Count; i++)
            {
                DataRowView dr = (DataRowView)grdTruyCap.SelectedItems[i];
                if (!dr["STT"].ToString().IsNullOrEmptyOrSpace())
                    lstSelected.Add(dr);
            }

            if (lstSelected.Count == 0)
            {
                //LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                foreach (DataRowView dr in lstSelected)
                {
                    grdTruyCap.Items.Remove(dr);
                }

                for (int i = 0; i < grdTruyCap.Items.Count; i++)
                {

                    DataRowView dr = (DataRowView)grdTruyCap.Items[i];
                    if (!dr["STT"].ToString().IsNullOrEmptyOrSpace() && Convert.ToInt32(dr["STT"]) > 0)
                    {
                        if (!dr[2].ToString().IsNullOrEmptyOrSpace())
                        {
                            dr["STT"] = i + 1;
                            grdTruyCap.SelectedItem = grdTruyCap.Items[i];
                            grdTruyCap.CurrentItem = dr;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void grdTruyCap_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {
            //DataRowView dr = (DataRowView)grdTruyCap.CurrentItem;
            //string diaChi = dr["DIA_CHI"].ToString();
            //string loaiDiaChi = dr["LOAI_DIA_CHI"].ToString();

            //if (diaChi.IsNullOrEmptyOrSpace())
            //{
            //    //CommonFunction.ThongBaoChuaNhap(grdTruyCap.Columns[2].Header.ToString() + ":");
            //    return;
            //}
            //else if (!loaiDiaChi.Equals(MAC) && !loaiDiaChi.Equals(IP))
            //{
            //    //CommonFunction.ThongBaoChuaNhap(grdTruyCap.Columns[3].Header.ToString() + ":");
            //    return;
            //}

            //if (loaiDiaChi.Equals(MAC))
            //    if (LSecurity.IsMacAddress(diaChi) == false)
            //        LMessage.ShowMessage("Địa chỉ Mac không hợp lệ", LMessage.MessageBoxType.Warning);

            //if (loaiDiaChi.Equals(IP))
            //    if (LSecurity.IsIPv4Address(diaChi) == false)
            //        LMessage.ShowMessage("Địa chỉ Mac không hợp lệ", LMessage.MessageBoxType.Warning);

            //if (dr["STT"].ToString().IsNullOrEmptyOrSpace())
            //{
            //    int stt = dsTruyCap.Tables[0].Rows.Count;
            //    dsTruyCap.Tables[0].Rows[stt - 1]["STT"] = stt;
            //    dsTruyCap.Tables[0].Rows.Add(null, "", MAC, true);
            //    grdTruyCap.DataContext = dsTruyCap.Tables[0].DefaultView;
            //    grdTruyCap.Focus();
            //}

        }

        /// <summary>
        /// Sự kiện unlod cho form
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

        private void cmbDonVi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Reset thông tin nhóm quyền
            dt = new DataTable();
            dt.Columns.Add(LLanguage.SearchResourceByKey("U.DungChung.STT"), typeof(string));
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.NhomNSD.ucNSDCT01.Ma"), typeof(string));
            dt.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.NhomNSD.ucNSDCT01.Ten"), typeof(string));
            // đổ source lên lưới
            grDSDoiTuong.ItemsSource = dt;
            loadWidthColumnDoiTuong();

            // Reset thông tin phòng giao dịch
            dtPhongGD = new DataTable();
            dtPhongGD.Columns.Add(LLanguage.SearchResourceByKey("U.DungChung.STT"), typeof(string));
            dtPhongGD.Columns.Add("ID", typeof(string));
            dtPhongGD.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.NhomNSD.ucNSDCT01.Ma"), typeof(string));
            dtPhongGD.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.NhomNSD.ucNSDCT01.Ten"), typeof(string));
            // đổ source lên lưới
            grDSPhongGD.ItemsSource = dtPhongGD;
            loadWidthColumnDoiTuongPhongGD();
        }

        private void btnNhanVien_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lstPopup = null;
                var process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonVi);
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_HOSO_NVIEN_CTIET.getValue(), lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow row = lstPopup[0];

                    if (!string.IsNullOrWhiteSpace(row[2].ToString()))
                        txtNhanVien.Text = row[2].ToString();
                    if (!string.IsNullOrWhiteSpace(row[3].ToString()))
                    {
                        txtTenDangNhap.Text = TaoTenDangNhap(row[3].ToString());
                        txtTenDem.Text = LString.SplitFirstName(row[3].ToString());
                        txtTenGoi.Text = LString.SplitLastName(row[3].ToString());
                    }
                    if (!string.IsNullOrWhiteSpace(row[4].ToString()))
                        txtEmail.Text = row[4].ToString();
                    if (!string.IsNullOrWhiteSpace(row[5].ToString()))
                        txtDienThoai.Text = row[5].ToString();
                    if (!string.IsNullOrWhiteSpace(row[6].ToString()))
                        raddtNgaySinh.Value = LDateTime.StringToDate(row[6].ToString(), "yyyyMMdd");
                    if (!string.IsNullOrWhiteSpace(row[7].ToString()))
                        cmbGioiTinh.SelectedIndex = lstSourceGioiTinh.IndexOf(lstSourceGioiTinh.FirstOrDefault(i => i.KeywordStrings[0].Equals(row[7].ToString())));
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                CommonFunction.ThongBaoLoi(ex);
            }
        }

        private string TaoTenDangNhap(string hoTenDayDu)
        {
            bool flag = false;
            string shortName = LString.ToShortName(hoTenDayDu, "_");
            string tenDangNhap = shortName;

            List<HT_NSD> lstNSD = new QuanTriHeThongProcess().layAllNSD();

            for (int i = 0; i < 100; i++)
            {
                flag = false;

                if (i > 0)
                    tenDangNhap = shortName + i.ToString();

                foreach (HT_NSD item in lstNSD)
                {
                    if (item.MA_DANG_NHAP.Equals(tenDangNhap))
                    {
                        flag = true;
                        break;
                    }
                }

                if (flag == false)
                    break;
            }

            return tenDangNhap;
        }

        private void chkLayThongTin_Checked(object sender, RoutedEventArgs e)
        {
            bool chk = chkLayThongTin.IsChecked.Value;

            txtNhanVien.IsEnabled = true;
            btnNhanVien.IsEnabled = true;
        }

        private void chkLayThongTin_Unchecked(object sender, RoutedEventArgs e)
        {
            bool chk = chkLayThongTin.IsChecked.Value;

            txtNhanVien.IsEnabled = false;
            btnNhanVien.IsEnabled = false;
        }

        private void txtNhanVien_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnNhanVien_Click(null, null);
            }
        }

        private void chkMatKhauKhongHetHan_Checked(object sender, RoutedEventArgs e)
        {
            telnumDoiMatKhau.Value = 0;
            telnumDoiMatKhau.IsEnabled = false;
            cmbDoiMatKhau.SelectedIndex = lstSourceTanSuat.IndexOf(lstSourceTanSuat.FirstOrDefault(item => item.KeywordStrings.First().Equals(BusinessConstant.TAN_SUAT.NGAY.layGiaTri()))); ;
            cmbDoiMatKhau.IsEnabled = false;

            // Ẩn control
            lblDoiMatKhau.Visibility = Visibility.Hidden;
            telnumDoiMatKhau.Visibility = Visibility.Hidden;
            cmbDoiMatKhau.Visibility = Visibility.Hidden;
        }

        private void chkMatKhauKhongHetHan_Unchecked(object sender, RoutedEventArgs e)
        {
            telnumDoiMatKhau.Value = 0;
            telnumDoiMatKhau.IsEnabled = true;
            cmbDoiMatKhau.SelectedIndex = lstSourceTanSuat.IndexOf(lstSourceTanSuat.FirstOrDefault(item => item.KeywordStrings.First().Equals(BusinessConstant.TAN_SUAT.NGAY.layGiaTri()))); ;
            cmbDoiMatKhau.IsEnabled = true;

            // Hiện control
            lblDoiMatKhau.Visibility = Visibility.Visible;
            telnumDoiMatKhau.Visibility = Visibility.Visible;
            cmbDoiMatKhau.Visibility = Visibility.Visible;
        }

        private void cmbPhanCap_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = cmbPhanCap.Text;
            AutoComboBox auto = new AutoComboBox();
            AutoCompleteEntry entry = auto.checkDisplayName(lstSourcePhanLoai, ref cmbPhanCap);

            if (entry == null)
            {
                return;
            }
            else
            {
                dt = new DataTable();
                dt.Columns.Add(LLanguage.SearchResourceByKey("U.DungChung.STT"), typeof(string));
                dt.Columns.Add("ID", typeof(string));
                dt.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.NhomNSD.ucNSDCT01.Ma"), typeof(string));
                dt.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.NhomNSD.ucNSDCT01.Ten"), typeof(string));
                // đổ source lên lưới
                grDSDoiTuong.ItemsSource = dt;
                loadWidthColumnDoiTuong();

                string maLoaiNguoiDung = entry.KeywordStrings.First();

                if (maLoaiNguoiDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTTW.layGiaTri()) ||
                    maLoaiNguoiDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVTW.layGiaTri()))
                {
                    cmbDonVi.SelectedIndex = lstSourceDonVi.IndexOf(lstSourceDonVi.FirstOrDefault(item => item.KeywordStrings.First().Equals(DatabaseConstant.MA_HSO)));
                    cmbDonVi.IsEnabled = false;
                }
                else
                {
                    if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()))
                    {
                        cmbDonVi.IsEnabled = false;
                    }
                    else
                    {
                        cmbDonVi.IsEnabled = true;
                    }
                }
            }
        }
        #endregion

        #region Xu ly nghiep vu

        public void onSave()
        {
            this.Cursor = Cursors.Wait;
            try
            {
                if (Validate())
                {
                    QuanTriHeThongProcess process = new QuanTriHeThongProcess();

                    List<string> lstStrIdNHNSD = (from row in dt.AsEnumerable() select row.Field<string>("ID")).Distinct().ToList();
                    List<int> lstIdNHNSD = lstStrIdNHNSD.Select(i => i.StringToInt32()).ToList();

                    List<DataRow> lstDataRowPhongGD = (from row in dtPhongGD.AsEnumerable() select row).Distinct().ToList();
                    List<string> lstStrIdPhongGD = (from row in dtPhongGD.AsEnumerable() select row.Field<string>("ID")).Distinct().ToList();
                    List<int> lstIdPhongGD = lstStrIdPhongGD.Select(i => i.StringToInt32()).ToList();
                    List<string> lstStrMaPhongGD = (from row in dtPhongGD.AsEnumerable() select row[2].ToString()).Distinct().ToList();

                    // Build danh sách phạm vi dữ liệu
                    List<PhamViDto> lstPhamVi = new List<PhamViDto>();
                    foreach (DataRow item in lstDataRowPhongGD)
                    {
                        PhamViDto phamViDto = new PhamViDto();
                        phamViDto.IdLoaiPvi = BusinessConstant.LoaiPhamViDuLieu.PHONG_GIAO_DICH.layIdPhamVi();
                        phamViDto.MaLoaiPvi = BusinessConstant.LoaiPhamViDuLieu.PHONG_GIAO_DICH.layMaPhamVi();
                        phamViDto.IdPvi = Int32.Parse(item[1].ToString());
                        phamViDto.MaPvi = item[2].ToString();
                        phamViDto.TenPvi = item[3].ToString();

                        lstPhamVi.Add(phamViDto);
                    }

                    ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    string responseMessage = null;

                    // Nếu là thêm mới
                    if (id == 0)
                    {
                        getObject(DatabaseConstant.Action.THEM);
                        ret = process.ThemNSD(ref obj, lstIdNHNSD, lstPhamVi, lstTruyCap, ref responseMessage);
                        afterAddNew(ret, obj, responseMessage);
                    }
                    // Nếu là sửa
                    else
                    {
                        getObject(DatabaseConstant.Action.SUA);
                        ret = process.SuaNSD(ref obj, lstIdNHNSD, lstPhamVi, lstTruyCap, ref responseMessage);
                        afterModify(ret, obj, responseMessage);
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }        

        public void getObject(DatabaseConstant.Action action)
        {
            this.Cursor = Cursors.Wait;
            try
            {
                if (obj == null)
                    obj = new HT_NSD();

                obj.MA_DVI_QLY = lstSourceDonVi.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.First();
                obj.MA_DVI_TAO = ClientInformation.MaDonVi;
                obj.MA_DANG_NHAP = txtTenDangNhap.Text.ToUpper();
                obj.PHAN_LOAI_NSD = lstSourcePhanLoai.ElementAt(cmbPhanCap.SelectedIndex).KeywordStrings.First();

                obj.MA_NSD = txtTenDangNhap.Text.ToUpper();
                obj.MA_HSO = txtNhanVien.Text;
                obj.TEN_GOI = txtTenGoi.Text;
                obj.TEN_HO_DEM = txtTenDem.Text;
                obj.TEN_DAY_DU = obj.TEN_HO_DEM + " " + obj.TEN_GOI;
                obj.EMAIL = txtEmail.Text;
                obj.DIEN_THOAI = txtDienThoai.Text;

                if (raddtNgaySinh.Value is DateTime)
                    obj.NGAY_SINH = ((DateTime)raddtNgaySinh.Value).ToString("yyyyMMdd");
                //obj.GIOI_TINH = ((RadComboBoxItem)cmbGioiTinh.SelectedItem).Tag.ToString();
                obj.GIOI_TINH = lstSourceGioiTinh.ElementAt(cmbGioiTinh.SelectedIndex).KeywordStrings.First();
                obj.TINH_TRANG = lstSourceTinhTrang.ElementAt(cmbTinhTrang.SelectedIndex).KeywordStrings.First();

                if (!LString.IsNullOrEmptyOrSpace(pwbMatKhau.Password) && (obj.ID == 0))
                {
                    string txtMD5 = LSecurity.MD5Encrypt(pwbMatKhau.Password);
                    obj.MAT_KHAU = txtMD5;
                }
                if (raddtNgayHieuLuc.Value is DateTime)
                    obj.NGAY_HIEU_LUC = ((DateTime)raddtNgayHieuLuc.Value).ToString("yyyyMMdd");
                if (raddtNgayHetHan.Value is DateTime)
                    obj.NGAY_HET_HAN = ((DateTime)raddtNgayHetHan.Value).ToString("yyyyMMdd");

                if (chkMatKhauKhongHetHan.IsChecked == false)
                {
                    obj.TGIAN_DOI_DVI_TINH = lstSourceTanSuat.ElementAt(cmbDoiMatKhau.SelectedIndex).KeywordStrings.First();
                    obj.TGIAN_DOI_MKHAU = Convert.ToInt32(telnumDoiMatKhau.Value);
                }
                else
                {
                    obj.TGIAN_DOI_DVI_TINH = BusinessConstant.TAN_SUAT.NA.layGiaTri();
                    obj.TGIAN_DOI_MKHAU = 0;
                }

                if (chkYeuCauDoiMatKhau.IsChecked == false)
                {
                    obj.TDOI_MKHAU = BusinessConstant.YeuCauDoiMatKhau.DA_THAY_DOI.layGiaTri();
                }
                else
                {
                    obj.TDOI_MKHAU = BusinessConstant.YeuCauDoiMatKhau.CHUA_THAY_DOI.layGiaTri();
                }

                if (chkHoatDong.IsChecked == true)
                {
                    obj.HAN_CHE_TRUY_CAP = BusinessConstant.CoKhong.CO.layGiaTri();
                }
                else
                {
                    obj.HAN_CHE_TRUY_CAP = BusinessConstant.CoKhong.KHONG.layGiaTri();
                }

                //obj.NGAY_CNHAT = (DateTime.Today).ToString("yyyyMMdd");
                obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                if (obj.ID == 0)
                {
                    obj.NGAY_DOI_MKHAU = ((DateTime)raddtNgayHieuLuc.Value).ToString("yyyyMMdd");
                    //obj.NGAY_NHAP = (DateTime.Today).ToString("yyyyMMdd");
                    obj.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    obj.NGAY_TAO = obj.NGAY_NHAP;
                }
                obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;

                string strTTNV = string.Empty;
                if (obj.ID == 0)
                {
                    obj.NGUON_TAO_DL = DatabaseConstant.NguonTaoDuLieu.NSD.layGiaTri();
                    strTTNV = obj.TTHAI_NVU;
                }
                obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.TTHAI_NVU = CommonFunction.LayTrangThaiBanGhi(action, BusinessConstant.layTrangThaiNghiepVu(strTTNV));

                // Luôn là đã duyệt (???)
                obj.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();

                //Lấy thông tin hạn chế truy cập MAC or IP
                lstTruyCap = new List<HT_TRUY_CAP>();
                foreach (DataRow dr in dsTruyCap.Tables[0].Rows)
                {
                    if (!dr["STT"].ToString().IsNullOrEmptyOrSpace())
                    {
                        HT_TRUY_CAP objTruyCap = new HT_TRUY_CAP();
                        objTruyCap.ID_DTUONG = obj.ID;
                        objTruyCap.MA_DTUONG = obj.MA_NSD;
                        objTruyCap.LOAI_DTUONG = BusinessConstant.LoaiDoiTuong.NGUOI_SDUNG.layGiaTri();

                        if (Convert.ToBoolean(dr["KICH_HOAT"]) == true)
                            objTruyCap.KICH_HOAT = BusinessConstant.CoKhong.CO.layGiaTri();
                        else
                            objTruyCap.KICH_HOAT = BusinessConstant.CoKhong.KHONG.layGiaTri();

                        objTruyCap.DIA_CHI = dr["DIA_CHI"].ToString().ToUpper();
                        objTruyCap.LOAI_DIA_CHI = dr["LOAI_DIA_CHI"].ToString();

                        lstTruyCap.Add(objTruyCap);
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw new CustomException("M.ResponseMessage.Common.KhongThanhCong", ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lstPopup = new List<DataRow>();
                var process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                //lstDieuKien.Add("'" + txtMaDonVi.Text + "'");
                lstDieuKien.Add("'" + lstSourceDonVi.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.First() + "'");
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_HT_NHNSD.getValue(), lstDieuKien);

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, true);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Content = popup;
                win.ShowDialog();
                if (lstPopup != null)
                {
                    List<string> lstID = (from row in dt.AsEnumerable()
                                          select row.Field<string>("ID")).Distinct().ToList();
                    if (dt.Rows.Count == 0)
                    {
                        dt = new DataTable();
                        dt.Columns.Add(LLanguage.SearchResourceByKey("U.DungChung.STT"), typeof(string));
                        dt.Columns.Add("ID", typeof(string));
                        dt.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.NhomNSD.ucNSDCT01.Ma"), typeof(string));
                        dt.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.NhomNSD.ucNSDCT01.Ten"), typeof(string));
                    }
                    foreach (DataRow row in lstPopup)
                    {
                        if (lstID.Contains(row[1].ToString()) != true)
                        {
                            DataRow r = dt.NewRow();
                            r[0] = dt.Rows.Count + 1;
                            r[1] = row[1];
                            r[2] = row[2];
                            r[3] = row[3];
                            dt.Rows.Add(r);
                        }
                    }
                    grDSDoiTuong.ItemsSource = null;
                    grDSDoiTuong.ItemsSource = dt;
                    loadWidthColumnDoiTuong();
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            try
            {
                if (grDSDoiTuong.SelectedItems.Count > 0)
                {
                    List<DataRow> lstRowDel = new List<DataRow>();
                    foreach (var item in grDSDoiTuong.SelectedItems)
                    {
                        DataRow r = dt.AsEnumerable().FirstOrDefault(d => d.Field<string>("ID").Equals(((DataRow)item).Field<string>("ID")));
                        lstRowDel.Add(r);
                    }
                    foreach (DataRow item in lstRowDel)
                    {
                        for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        {
                            if (dt.Rows[i][1].Equals(item[1]))
                            {
                                dt.Rows.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    for (int i = dt.Rows.Count; i > 0; i--)
                    {
                        dt.Rows[i - 1][0] = i.ToString();
                    }
                    grDSDoiTuong.ItemsSource = null;
                    grDSDoiTuong.ItemsSource = dt;
                    loadWidthColumnDoiTuong();
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void btnAddPhongGD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lstPopupPhongGD = new List<DataRow>();
                var process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                //lstDieuKien.Add("'" + txtMaDonVi.Text + "'");
                lstDieuKien.Add(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri());
                lstDieuKien.Add(BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri());
                string loaiNSD = lstSourcePhanLoai.ElementAt(cmbPhanCap.SelectedIndex).KeywordStrings.First();

                // Kiểm tra nếu là loại NSD sa hoặc Quản trị TƯ hoặc Tác nghiệp TƯ thì hiển thị hết. Nếu không chỉ hiển thị ĐV con
                if (loaiNSD.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVTW.layGiaTri()) || loaiNSD.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTTW.layGiaTri()) || loaiNSD.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_SA.layGiaTri()))
                    lstDieuKien.Add("%");
                else
                    lstDieuKien.Add(lstSourceDonVi.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.First());
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DONVITHEOMACHA.getValue(), lstDieuKien);

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, true);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopupPhongGD);
                Window win = new Window();
                //win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.Title = "Danh sách phòng giao dịch";
                win.Content = popup;
                win.ShowDialog();
                if (lstPopupPhongGD != null)
                {
                    List<string> lstID = (from row in dtPhongGD.AsEnumerable()
                                          select row.Field<string>("ID")).Distinct().ToList();
                    if (dtPhongGD.Rows.Count == 0)
                    {
                        dtPhongGD = new DataTable();
                        dtPhongGD.Columns.Add(LLanguage.SearchResourceByKey("U.DungChung.STT"), typeof(string));
                        dtPhongGD.Columns.Add("ID", typeof(string));
                        dtPhongGD.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.NhomNSD.ucNSDCT01.Ma"), typeof(string));
                        dtPhongGD.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.NhomNSD.ucNSDCT01.Ten"), typeof(string));
                    }
                    foreach (DataRow row in lstPopupPhongGD)
                    {
                        if (lstID.Contains(row[1].ToString()) != true)
                        {
                            DataRow r = dtPhongGD.NewRow();
                            r[0] = dtPhongGD.Rows.Count + 1;
                            r[1] = row[1];
                            r[2] = row[2];
                            r[3] = row[3];
                            dtPhongGD.Rows.Add(r);
                        }
                    }
                    grDSPhongGD.ItemsSource = null;
                    grDSPhongGD.ItemsSource = dtPhongGD;
                    loadWidthColumnDoiTuongPhongGD();
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }

        }

        private void btnDeletePhongGD_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            try
            {
                if (grDSPhongGD.SelectedItems.Count > 0)
                {
                    List<DataRow> lstRowDel = new List<DataRow>();
                    foreach (var item in grDSPhongGD.SelectedItems)
                    {
                        DataRow r = dtPhongGD.AsEnumerable().FirstOrDefault(d => d.Field<string>("ID").Equals(((DataRow)item).Field<string>("ID")));
                        lstRowDel.Add(r);
                    }
                    foreach (DataRow item in lstRowDel)
                    {
                        for (int i = dtPhongGD.Rows.Count - 1; i >= 0; i--)
                        {
                            if (dtPhongGD.Rows[i][1].Equals(item[1]))
                            {
                                dtPhongGD.Rows.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    for (int i = dtPhongGD.Rows.Count; i > 0; i--)
                    {
                        dtPhongGD.Rows[i - 1][0] = i.ToString();
                    }
                    grDSPhongGD.ItemsSource = null;
                    grDSPhongGD.ItemsSource = dtPhongGD;
                    loadWidthColumnDoiTuongPhongGD();
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void btnPopupDonVi_Click(object sender, RoutedEventArgs e)
        {
            ShowPopupDonVi();
        }

        private void txtMaDonVi_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.F3)
            {
                ShowPopupDonVi();
            }
        }

        private void ShowPopupDonVi()
        {
            try
            {
                lstPopup = new List<DataRow>();
                var process = new PopupProcess();
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DM_DONVI.getValue());

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.Content = popup;
                win.ShowDialog();
                if (lstPopup != null)
                    if (lstPopup != null)
                    {
                        //if (txtMaDonVi.Text != lstPopup[0][2].ToString())
                        //{
                        //    // Reset source thông tin đối tượng
                        //    dt = new DataTable();
                        //    dt.Columns.Add("STT", typeof(string));
                        //    dt.Columns.Add("ID", typeof(string));
                        //    dt.Columns.Add("Mã", typeof(string));
                        //    dt.Columns.Add("Tên", typeof(string));
                        //    grDSDoiTuong.ItemsSource = dt;
                        //    txtMaDonVi.Text = lstPopup[0][2].ToString();
                        //    lblDonViQL.Content = lstPopup[0][3].ToString();
                        //}
                    }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private bool Validate()
        {
            if (txtTenDangNhap.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaNhap(lblTenDangNhap.Content.ToString());
                txtTenDangNhap.Focus();
                return false;
            }

            if (txtTenDem.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaNhap(lblTenDem.Content.ToString());
                txtTenDem.Focus();
                return false;
            }

            if (txtTenGoi.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaNhap(lblTenGoi.Content.ToString());
                txtTenGoi.Focus();
                return false;
            }

            if (cmbTinhTrang.SelectedIndex < 0)
            {
                CommonFunction.ThongBaoChuaChon(lblTinhTrang.Content.ToString());
                cmbTinhTrang.Focus();
                return false;
            }

            if (!LDateTime.IsDate(raddtNgayHieuLuc.Text, "dd/MM/yyyy"))
            {
                CommonFunction.ThongBaoChuaNhap(lblNgayHieuLuc.Content.ToString());
                raddtNgayHieuLuc.Focus();
                return false;
            }

            if (raddtNgaySinh.Text != "__/__/____")
            {
                if (!LDateTime.IsDate(raddtNgaySinh.Text, "dd/MM/yyyy"))
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.InvalidDateOfBirthFormat", LMessage.MessageBoxType.Warning);
                    raddtNgaySinh.Focus();
                    return false;
                }
                else
                {
                    string ngaySinh = Convert.ToDateTime(raddtNgaySinh.Value).ToString("yyyyMMdd");
                    if (ngaySinh.CompareTo(ClientInformation.NgayLamViecHienTai)>0)
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.InvalidDateOfBirthValue", LMessage.MessageBoxType.Warning);
                        raddtNgaySinh.Focus();
                        return false;
                    }
                }
            }

            if ((formCase.Equals("MANAGE") || (formCase.Equals("THEM")) || (formCase.Equals("SUA"))) && obj == null)
            {
                if (pwbMatKhau.Password.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaNhap(lblMatKhau.Content.ToString());
                    pwbMatKhau.Focus();
                    return false;
                }

                if (pwbNhapLaiMatKhau.Password.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaNhap(lblMatKhauReq.Content.ToString());
                    pwbNhapLaiMatKhau.Focus();
                    return false;
                }

                if (pwbMatKhau.Password != pwbNhapLaiMatKhau.Password)
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.InvalidRepasswordValue", LMessage.MessageBoxType.Warning);
                    pwbNhapLaiMatKhau.Focus();
                    return false;
                }

                // Kiểm tra độ dài mật khẩu khi thêm mới
                UtilitiesProcess utilitiesProcess = new UtilitiesProcess();
                Presentation.Process.UtilitiesServiceRef.HT_TSO htTso = utilitiesProcess.LayThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_DODAI_MATKHAU, ClientInformation.MaDonViQuanLy);
                if (htTso != null)
                {
                    string giaTriThamSo = htTso.GIA_TRI;
                    if (giaTriThamSo != null && giaTriThamSo != "")
                    {
                        if (pwbMatKhau.Password.Length < Int32.Parse(giaTriThamSo))
                        {
                            LMessage.ShowMessage("M.ResponseMessage.Common.InvalidPasswordLength", LMessage.MessageBoxType.Warning);
                            txtTenDangNhap.Focus();
                            return false;
                        }
                    }
                }
            }

            if (!chkMatKhauKhongHetHan.IsChecked == true && (Convert.ToInt32(telnumDoiMatKhau.Value) < 0))
            {
                LMessage.ShowMessage("M.ResponseMessage.Common.InvalidPasswordPeriodFormat", LMessage.MessageBoxType.Warning);
                chkMatKhauKhongHetHan.Focus();
                return false;
            }


            string regexPatternUserName = @"^[A-Za-z0-9_-]{3,15}$";
            if (Regex.IsMatch(txtTenDangNhap.Text, regexPatternUserName) == false)
            {
                LMessage.ShowMessage("M.ResponseMessage.Common.InvalidUserNameFormat", LMessage.MessageBoxType.Warning);
                txtTenDangNhap.Focus();
                return false;
            }

            string regexPatternEmail =
                        @"^([0-9a-zA-Z]" + //Start with a digit or alphabate
                        @"([\+\-_\.][0-9a-zA-Z]+)*" + // No continues or ending +-_. chars in email
                        @")+" +
                        @"@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,17})$";

            if (!LObject.IsNullOrEmpty(txtEmail.Text))
            {
                if (Regex.IsMatch(txtEmail.Text, regexPatternEmail) == false)
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.InvalidEmailFormat", LMessage.MessageBoxType.Warning);
                    txtEmail.Focus();
                    return false;
                }
            }

            //String cmnd = @"^[0-9]{9,11}$";

            string regexPatternPhoneNumber =
                        //@"^[0-9]{9,11}$"; // length 9 - 11
                        @"^[0-9]+$"; // any number
            if (!LObject.IsNullOrEmpty(txtDienThoai.Text))
            {
                if (Regex.IsMatch(txtDienThoai.Text, regexPatternPhoneNumber) == false)
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.InvalidPhoneFormat", LMessage.MessageBoxType.Warning);
                    txtDienThoai.Focus();
                    return false;
                }
            }

            if (dsTruyCap != null && dsTruyCap.Tables[0].Rows.Count > 0)
            {
                string message = "";
                string dsMAC = "";
                string dsIP = "";
                foreach (DataRow dr in dsTruyCap.Tables[0].Rows)
                {
                    if (dr["LOAI_DIA_CHI"].ToString().Equals(MAC) && !LSecurity.IsMacAddress(dr["DIA_CHI"].ToString()))
                        dsMAC = dsMAC + " " + dr["STT"].ToString();

                    else if (dr["LOAI_DIA_CHI"].ToString().Equals(IP) && !LSecurity.IsIPv4Address(dr["DIA_CHI"].ToString()))
                        dsIP = dsIP + " " + dr["STT"].ToString();
                }

                if (dsMAC.Length > 0)
                    message = message + "MAC address is not valid. Row number" + dsMAC + "\n";

                if (dsIP.Length > 0)
                    message = message + "IP address is not valid. Row number" + dsIP;

                if (message.Length > 0)
                {
                    LMessage.ShowMessage(message, LMessage.MessageBoxType.Warning);
                    grdTruyCap.Focus();
                    return false;
                }                
            }

            return true;
        }

        /// <summary>
        /// Trước khi sửa
        /// </summary>
        private void beforeModify()
        {
            // Yêu cầu lock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool ret = process.LockData(DatabaseConstant.Module.QTHT,
                DatabaseConstant.Function.HT_NSD,
                DatabaseConstant.Table.HT_NSD,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Nếu lock thành công >> cho phép sửa
            if (ret)
            {
                formCase = "SUA";
                HideControl();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu, mnuMain, DatabaseConstant.Function.HT_NSD);
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

                bool retLockData = process.LockData(DatabaseConstant.Module.QTHT,
                    DatabaseConstant.Function.HT_NSD,
                    DatabaseConstant.Table.HT_NSD,
                    DatabaseConstant.Action.XOA,
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
        /// Xóa dữ liệu
        /// </summary>
        private void onDelete()
        {
            QuanTriHeThongProcess process = new QuanTriHeThongProcess();
            int[] arrayID = new int[0];
            //Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = id;

                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = process.XoaListNSD(arrayID, ref listClientResponseDetail);

                afterDelete(ret, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            //Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Sau khi thêm mới
        /// </summary>
        /// <param name="ret"></param>
        private void afterAddNew(ApplicationConstant.ResponseStatus ret, HT_NSD obj, string responseMessage)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                if (cbMultiAdd.IsChecked == true)
                {
                    ResetForm();
                }
                else if (!DatabaseConstant.CLOSE_DETAIL_FORM)
                {
                    id = obj.ID;
                    TthaiNvu = obj.TTHAI_NVU;

                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(obj.TINH_TRANG);
                    txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(obj.TINH_TRANG);
                    raddtNgayNhap.Value = LDateTime.StringToDate(obj.NGAY_NHAP, "yyyyMMdd");
                    txtNguoiLap.Text = obj.NGUOI_NHAP;
                    raddtNgayCNhat.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, "yyyyMMdd");
                    txtNguoiCapNhat.Text = obj.NGUOI_CNHAT;
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.HT_NSD);

                    formCase = "XEM";
                    HideControl();
                }
                else
                {
                    onClose();
                }
            }
            else
            {
                LMessage.ShowMessage(responseMessage, LMessage.MessageBoxType.Error);
            }
        }

        /// <summary>
        /// Sau khi sửa
        /// </summary>
        /// <param name="ret"></param>
        private void afterModify(ApplicationConstant.ResponseStatus ret, HT_NSD obj, string responseMessage)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                id = obj.ID;
                TthaiNvu = obj.TTHAI_NVU;

                lblTrangThai.Content = BusinessConstant.layNgonNguTrangThaiNguoiDung(obj.TINH_TRANG);
                txtTrangThai.Text = BusinessConstant.layNgonNguTrangThaiNguoiDung(obj.TINH_TRANG);
                raddtNgayNhap.Value = LDateTime.StringToDate(obj.NGAY_NHAP, "yyyyMMdd");
                txtNguoiLap.Text = obj.NGUOI_NHAP;
                raddtNgayCNhat.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, "yyyyMMdd");
                txtNguoiCapNhat.Text = obj.NGUOI_CNHAT;
                if (formCase.Equals("PROFILE"))
                {
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu, mnuMain, DatabaseConstant.Function.HT_NSD);

                    formCase = "PROFILE";
                    HideControl();
                }
                else
                {
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.HT_NSD);

                    formCase = "XEM";
                    HideControl();
                }
            }
            else
            {
                LMessage.ShowMessage(responseMessage, LMessage.MessageBoxType.Error);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.QTHT,
                DatabaseConstant.Function.HT_NSD,
                DatabaseConstant.Table.HT_NSD,
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
            }
            else
            {
                //LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);
            List<int> listUnlockId = new List<int>();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.QTHT,
                DatabaseConstant.Function.HT_NSD,
                DatabaseConstant.Table.HT_NSD,
                DatabaseConstant.Action.XOA,
                listLockId);

            // Đóng cửa sổ chi tiết sau khi xóa thành công
            if (ret)
            {
                onClose();
            }
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
            listLockId.Add(id);

            bool ret = process.UnlockData(DatabaseConstant.Module.QTHT,
                DatabaseConstant.Function.HT_NSD,
                DatabaseConstant.Table.HT_NSD,
                DatabaseConstant.Action.SUA,
                listLockId);
        }     

        #endregion

    }
}
