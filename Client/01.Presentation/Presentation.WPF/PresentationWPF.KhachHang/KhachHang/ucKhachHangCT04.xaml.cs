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
using Telerik.Windows.Controls;
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.KhachHangServiceRef;
using PresentationWPF.CustomControl;
using PresentationWPF.KhachHang.KhachHang.Popup;
using PresentationWPF.BaoCao.DungChung;
using System.Text.RegularExpressions;

namespace PresentationWPF.KhachHang.KhachHang
{
    /// <summary>
    /// Interaction logic for ucKhachHangCT04.xaml
    /// </summary>
    public partial class ucKhachHangCT04 : UserControl
    {
        #region Khai bao
        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private DatabaseConstant.Function function = DatabaseConstant.Function.KH_THANH_VIEN;

        public event EventHandler OnSavingCompleted;

        private string loaiKhachHang = BusinessConstant.LoaiKhachHang.TVIEN.layGiaTri();
        public string LoaiKhachHang
        {
            get { return loaiKhachHang; }
            set { loaiKhachHang = value; }
        }

        private int id = 0;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int idKhuVuc = 0;
        private int idCum = 0;
        private int idNhom = 0;

        private KH_KHANG_HSO obj;
        public KH_KHANG_HSO Obj
        {
            get { return obj; }
            set { obj = value; }
        }

        private string sTrangThaiNVu = "";
        
        private DataSet dsSource  = new DataSet();        

        private string vung_mien_HK, tinh_tp_HK, quan_huyen_HK, xa_phuong_HK, lang_todp_HK, vung_mien_hien_tai, tinh_tp_hien_tai, quan_huyen_hien_tai, xa_phuong_hien_tai, lang_todp_hien_tai;

        //Source cac combobox phan thong tin chung
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceGioiTinh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDanToc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNoiCap = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceQuocTich = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTTCuTru = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLyDoRa = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLHToChuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNganhKT = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiKH = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgheNghiep = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiEmail = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();

        //Source cac combobox phan thong tin khac
        List<AutoCompleteEntry> lstSourceHonNhan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHocVan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceVaiTroGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLHinhCongTac = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTGianCongTac = new List<AutoCompleteEntry>();

        //Source cac combobox phan ho khau
        List<AutoCompleteEntry> lstSourceHKVungMien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHKTinhTP = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHKXaPhuong = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHKQuanHuyen = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHKLangTodp = new List<AutoCompleteEntry>();

        //Source cac combobox phan dia chi hien tai
        List<AutoCompleteEntry> lstSourceVungMien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTinhTp = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceXaPhuong = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceQuanHuyen = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLangTodp = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiHinhCuTru = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceQuanHeVoiNguoiCuTru = new List<AutoCompleteEntry>();

        //Source cac combobox phan nha cua
        List<AutoCompleteEntry> lstSourceNoiCuTru = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceQuanHeCuTru = new List<AutoCompleteEntry>();

        //Source cac combobox du lieu hinh anh
        List<AutoCompleteEntry> lstSourceLoaiHinhAnh = new List<AutoCompleteEntry>();

        //Source cac combobox cong viec hien tai, tham chieu
        List<AutoCompleteEntry> lstSourceLoaiHinhCongTy = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceMoiQuanHeTChieu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNganhNgheKD = new List<AutoCompleteEntry>();

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();


        List<LoaiHinhAnh> lstLoaiHinhAnhDef = new List<LoaiHinhAnh>()
        {
            new LoaiHinhAnh(){ MaLoai = "HSPL", TenLoai = "Hồ sơ pháp lý", LangKey = "LoaiHinhAnh.HSPL"},
	        new LoaiHinhAnh(){ MaLoai = "HSKT", TenLoai = "Hồ sơ kinh tế", LangKey = "LoaiHinhAnh.HSKT"},
            new LoaiHinhAnh(){ MaLoai = "TCVM", TenLoai = "Quan hệ với tổ chức TCVM", LangKey = "LoaiHinhAnh.TCVM"}
        };
        List<LoaiHinhAnh> lstLoaiHinhAnhSel = new List<LoaiHinhAnh>();
        List<DoiTuongHinhAnh> lstDoiTuongHinhAnhDef = new List<DoiTuongHinhAnh>()
        {
            new DoiTuongHinhAnh(){ MaLoai = "HSPL", MaDoiTuong="CK", TenDoiTuong = "Chữ ký", LangKey = "DoiTuongHinhAnh.HSPL.CK"},
            new DoiTuongHinhAnh(){ MaLoai = "HSPL", MaDoiTuong="HA", TenDoiTuong = "Hình ảnh", LangKey = "DoiTuongHinhAnh.HSPL.HA"},
	        new DoiTuongHinhAnh(){ MaLoai = "HSPL", MaDoiTuong="CMTND", TenDoiTuong = "Chứng minh thư nhân dân", LangKey = "DoiTuongHinhAnh.HSPL.CMTND"},
            new DoiTuongHinhAnh(){ MaLoai = "HSPL", MaDoiTuong="KHAC", TenDoiTuong = "Khác", LangKey = "DoiTuongHinhAnh.HSPL.KHAC"},

	        new DoiTuongHinhAnh(){ MaLoai = "HSKT", MaDoiTuong="SODO", TenDoiTuong = "Sổ đỏ nhà đất", LangKey = "DoiTuongHinhAnh.HSKT.SODO"},
            new DoiTuongHinhAnh(){ MaLoai = "HSKT", MaDoiTuong="HDLD", TenDoiTuong = "Hợp đồng lao động", LangKey = "DoiTuongHinhAnh.HSKT.HDLD"},
            new DoiTuongHinhAnh(){ MaLoai = "HSKT", MaDoiTuong="SAOKE", TenDoiTuong = "Sao kê tài khoản", LangKey = "DoiTuongHinhAnh.HSKT.SAOKE"},
	        new DoiTuongHinhAnh(){ MaLoai = "HSKT", MaDoiTuong="KHAC", TenDoiTuong = "Khác", LangKey = "DoiTuongHinhAnh.HSKT.KHAC"},

            new DoiTuongHinhAnh(){ MaLoai = "TCVM", MaDoiTuong="SOTK", TenDoiTuong = "Sổ tiết kiệm", LangKey = "DoiTuongHinhAnh.TCVM.SOTK"},
            new DoiTuongHinhAnh(){ MaLoai = "TCVM", MaDoiTuong="HDTD", TenDoiTuong = "Hợp đồng tín dụng", LangKey = "DoiTuongHinhAnh.TCVM.HDTD"},
            new DoiTuongHinhAnh(){ MaLoai = "TCVM", MaDoiTuong="KHTV", TenDoiTuong = "Xác nhận thành viên", LangKey = "DoiTuongHinhAnh.TCVM.KHTV"},
            new DoiTuongHinhAnh(){ MaLoai = "TCVM", MaDoiTuong="KHAC", TenDoiTuong = "Khác", LangKey = "DoiTuongHinhAnh.TCVM.KHAC"}
        };
        List<DoiTuongHinhAnh> lstDoiTuongHinhAnhSel = new List<DoiTuongHinhAnh>();
        List<DuLieuHinhAnh> lstDuLieuHinhAnh = new List<DuLieuHinhAnh>();
        private byte[] imageData = null;
        private string imageName = "";
        private string imageFormat = "";
        private List<BS_FileObject> lstChuKyHinhAnh = new List<BS_FileObject>();
        private bool bHetHLuc = false;
        private string ngayCongNhan = "";
        #endregion

        #region Khoi tao
        public ucKhachHangCT04()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            LoadCombobox();

            InitEventHandler();
        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KhachHang;component/KhachHang/ucKhachHangCT04.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void LoadCombobox()
        {
            COMBOBOX_DTO combo = null;
            List<string> lstDieuKien = null;
            List<COMBOBOX_DTO> lstCombobox = new List<COMBOBOX_DTO>();

            //Loại khách hàng
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.LOAI_KHACH_HANG.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbLoaiKhachHang;
            combo.lstSource = lstSourceLoaiKH;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Lý do ra khỏi nhóm
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.LOAI_EMAIL.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbEmail;
            combo.lstSource = lstSourceLoaiEmail;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Giới tính
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.GIOI_TINH.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbGioiTinh;
            combo.lstSource = lstSourceGioiTinh;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Dân tộc
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DAN_TOC.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbDanToc;
            combo.lstSource = lstSourceDanToc;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Tình trạng hôn nhân
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.TINH_TRANG_HON_NHAN.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbTinhTrangHonNhan;
            combo.lstSource = lstSourceHonNhan;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Loại hình tổ chức
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.LOAI_HINH_TO_CHUC.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbLoaiHinhToChuc;
            combo.lstSource = lstSourceLHToChuc;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Ngành kinh tế
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGANH_KINH_TE.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbNganhKinhTe;
            combo.lstSource = lstSourceNganhKT;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Nghe nghiep
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGHE_NGHIEP.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbNgheNghiep;
            combo.lstSource = lstSourceNgheNghiep;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Moi quan he tham chieu
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.MOI_QUAN_HE.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbQuanHeThamChieu;
            combo.lstSource = lstSourceMoiQuanHeTChieu;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Loai hinh cu tru
            lstDieuKien = new List<string>();
            lstDieuKien.Add("LOAI_HINH_CU_TRU");
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbLoaiHinhCuTru;
            combo.lstSource = lstSourceLoaiHinhCuTru;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Quan he voi nguoi cu tru cung
            lstDieuKien = new List<string>();
            lstDieuKien.Add("MOI_QUAN_HE_VOI_NGUOI_CTRU");
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbOVoiAi;
            combo.lstSource = lstSourceQuanHeCuTru;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Moi quan he tham chieu
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.LOAI_CONG_TY.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbLoaiHinhCongTy;
            combo.lstSource = lstSourceLoaiHinhCongTy;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Vung - Ho khau
            //combo = new COMBOBOX_DTO();
            //combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_VUNG_ALL.getValue();
            //combo.combobox = cmbVungMien;
            //combo.lstSource = lstSourceHKVungMien;
            //lstCombobox.Add(combo);

            //Tỉnh thành - hộ khẩu
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_TINHTP.getValue();
            combo.combobox = cmbTinhTP;
            combo.lstSource = lstSourceHKTinhTP;
            lstCombobox.Add(combo);

            //Vung - Hien tai
            //combo = new COMBOBOX_DTO();
            //combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_VUNG_ALL.getValue();
            //combo.combobox = cmbVungMienHienTai;
            //combo.lstSource = lstSourceVungMien;
            //lstCombobox.Add(combo);

            //Tỉnh thành - Hiện tại
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_TINHTP.getValue();
            combo.combobox = cmbTinhTPHienTai;
            combo.lstSource = lstSourceTinhTp;
            lstCombobox.Add(combo);

            //Ngành nghề kinh doanh
            lstDieuKien = new List<string>();
            lstDieuKien.Add("NGANH_NGHE_KINH_DOANH");
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbNganhNghe;
            combo.lstSource = lstSourceNganhNgheKD;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Loại tiền
            lstDieuKien = new List<string>();
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_TIENTE.getValue();
            combo.combobox = cmbLuongCoBan;
            combo.lstSource = lstSourceLoaiTien;
            combo.lstDieuKien = lstDieuKien;
            combo.maChon = ClientInformation.MaDongNoiTe;
            lstCombobox.Add(combo);

            AutoComboBox auto = new AutoComboBox();
            auto.GenAutoComboBoxTheoList(ref lstCombobox);
            auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbThuNhapKhac, null, null, ClientInformation.MaDongNoiTe);
            auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbTongThuNhap, null, null, ClientInformation.MaDongNoiTe);

        }

        private void InitEventHandler()
        {
            chkThemNhieuLan.Checked += new RoutedEventHandler(chkThemNhieuLan_Checked);

            cmbLoaiKhachHang.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiKhachHang_SelectionChanged);

            chkKhachHangHetHL.Checked += new RoutedEventHandler(chkKhachHangHetHL_Checked);
            chkKhachHangHetHL.Unchecked += new RoutedEventHandler(chkKhachHangHetHL_Unchecked);

            btnCheck.Click += new RoutedEventHandler(btnCheck_Click);
            btnCheckHoKhau.Click += new RoutedEventHandler(btnCheckSoHoKhau_Click);

            //cmbVungMien.SelectionChanged += new SelectionChangedEventHandler(cmbVungMien_SelectionChanged);
            cmbTinhTP.SelectionChanged += new SelectionChangedEventHandler(cmbTinhTP_SelectionChanged);
            cmbQuanHuyen.SelectionChanged += new SelectionChangedEventHandler(cmbQuanHuyen_SelectionChanged);
            cmbXaPhuong.SelectionChanged += new SelectionChangedEventHandler(cmbXaPhuong_SelectionChanged);
            cmbLangTodp.SelectionChanged += new SelectionChangedEventHandler(cmbLangTodp_SelectionChanged);

            //cmbVungMienHienTai.SelectionChanged += new SelectionChangedEventHandler(cmbVungMienHienTai_SelectionChanged);
            cmbTinhTPHienTai.SelectionChanged += new SelectionChangedEventHandler(cmbTinhTPHienTai_SelectionChanged);
            cmbQuanHuyenHienTai.SelectionChanged += new SelectionChangedEventHandler(cmbQuanHuyenHienTai_SelectionChanged);
            cmbXaPhuongHienTai.SelectionChanged += new SelectionChangedEventHandler(cmbXaPhuongHienTai_SelectionChanged);
            cmbLangTodpHienTai.SelectionChanged += new SelectionChangedEventHandler(cmbLangTodpHienTai_SelectionChanged);

            cmbTinhTP.KeyDown += new KeyEventHandler(cmbTinhTP_KeyDown);
            cmbQuanHuyen.KeyDown += new KeyEventHandler(cmbQuanHuyen_KeyDown);
            cmbXaPhuong.KeyDown += new KeyEventHandler(cmbXaPhuong_KeyDown);

            cmbTinhTPHienTai.KeyDown += new KeyEventHandler(cmbTinhTPHienTai_KeyDown);
            cmbQuanHuyenHienTai.KeyDown += new KeyEventHandler(cmbQuanHuyenHienTai_KeyDown);
            cmbXaPhuongHienTai.KeyDown += new KeyEventHandler(cmbXaPhuongHienTai_KeyDown);
            //txtSoCoDinh.KeyDown += new KeyEventHandler(txtSoCoDinh_KeyDown);
            //txtSoDiDong.KeyDown += new KeyEventHandler(txtSoDiDong_KeyDown);
            btnMaGoiNho.Click += btnMaGoiNho_Click;
            txtLuongCoBan.ValueChanged += txtLuongCoBan_ValueChanged;
            txtThuNhapKhac.ValueChanged += txtThuNhapKhac_ValueChanged;

            radioNongThonThuongChu.Checked += new RoutedEventHandler(radioNongThonThuongChu_Checked);
            radioThanhThiThuongChu.Checked += new RoutedEventHandler(radioThanhThiThuongChu_Checked);
            radioNongThonHienTai.Checked += new RoutedEventHandler(radioNongThonHienTai_Checked);
            radioThanhThiHienTai.Checked += new RoutedEventHandler(radioThanhThiHienTai_Checked);
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

            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                OnHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                OnSave();
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
            else if (strTinhNang.Equals("PreviewHoSo"))
            {
                OnPreviewHoSo();
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
            else if (strTinhNang.Equals("PreviewHoSo"))
            {
                OnPreviewHoSo();
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
            if (id <= 0 && !Presentation.Process.Common.ClientInformation.FormCase.IsNullOrEmptyOrSpace())
                loaiKhachHang = Presentation.Process.Common.ClientInformation.FormCase;

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

        /// <summary>
        /// Hàm xử lý giải phóng lock dữ liệu cho form
        /// </summary>
        private void Release()
        {
            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool ret = process.UnlockData(DatabaseConstant.Module.KHTV,
                DatabaseConstant.Function.KH_THANH_VIEN,
                DatabaseConstant.Table.KH_KHANG_HSO,
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
            action = DatabaseConstant.Action.THEM;
            id = 0;
            obj = null;
            sTrangThaiNVu = "";

            ResetForm();
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);

            chkThemNhieuLan.IsChecked = false;
        }

        private void cmbLoaiKhachHang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbLoaiKhachHang.SelectedIndex >= 0)
            {
                loaiKhachHang = lstSourceLoaiKH.ElementAt(cmbLoaiKhachHang.SelectedIndex).KeywordStrings.ElementAt(0);

                SetVisibledControl();
                SetEnabledControls();
                ResetForm();

            }
        }

        private void chkKhachHangHetHL_Unchecked(object sender, RoutedEventArgs e)
        {
            raddtNgayHetHL.Value = null;
            raddtNgayHetHL.IsEnabled = false;
            dtpNgayHetHL.IsEnabled = false;
            lblStarNgayHetHL.Visibility = System.Windows.Visibility.Hidden;

            //if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.TVIEN.layGiaTri()))
            //{
            //    lblStarLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Hidden;
            //    cmbLyDoRaKhoiNhom.IsEnabled = false;
            //}                                    
        }

        private void chkKhachHangHetHL_Checked(object sender, RoutedEventArgs e)
        {
            raddtNgayHetHL.IsEnabled = true;
            dtpNgayHetHL.IsEnabled = true;
            raddtNgayHetHL.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            lblStarNgayHetHL.Visibility = System.Windows.Visibility.Visible;

            //if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.TVIEN.layGiaTri()))
            //{                                
            //    lblStarLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Visible;
            //    cmbLyDoRaKhoiNhom.IsEnabled = true;
            //}
        }

        /*
        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.TVIEN.layGiaTri()) ||
                    loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.CNHAN.layGiaTri()))
                {
                    if (txtSoCMND.IsNullOrEmpty())
                    {
                        CommonFunction.ThongBaoChuaNhap(lblSoCMND.Content.ToString());
                        txtSoCMND.Focus();
                        return;
                    }

                    if (!LObject.IsNullOrEmpty(txtSoCMND.Text))
                    {
                        if (!CommonFunction.IsValidFormat(ApplicationConstant.FormatType.CMND,
                            txtSoCMND.Text))
                        {
                            txtSoCMND.Focus();
                            return;
                        }
                    }

                    Mouse.OverrideCursor = Cursors.Wait;

                    KhachHangProcess processKhachHang = new KhachHangProcess();
                    List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                    bool ret = false;

                    obj = new KH_KHANG_HSO();
                    obj.MA_KHANG = txtMaKhachHang.Text;
                    obj.DD_GTLQ_SO = txtSoCMND.Text.Trim();

                    if (txtSoCMND.Text.IsNullOrEmptyOrSpace())
                    {
                        //CommonFunction.ThongBaoChuaNhap(lblSoCMND.Content.ToString());
                        return;
                    }

                    ret = processKhachHang.KhachHang04(DatabaseConstant.Action.KIEM_TRA, ref obj, ref dsSource, ref lstChuKyHinhAnh, ref listClientResponseDetail);

                    if (ret)
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.NotExistCMND", LMessage.MessageBoxType.Information);
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.ExistCMND", LMessage.MessageBoxType.Warning);
                        //CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    }
                }
                else if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.DNGHIEP.layGiaTri()))
                {
                    if (txtSoCMND.IsNullOrEmpty())
                    {
                        CommonFunction.ThongBaoChuaNhap(lblSoCMND.Content.ToString());
                        txtSoCMND.Focus();
                        return;
                    }

                    Mouse.OverrideCursor = Cursors.Wait;

                    KhachHangProcess processKhachHang = new KhachHangProcess();
                    List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                    bool ret = false;

                    obj = new KH_KHANG_HSO();
                    obj.MA_KHANG = txtMaKhachHang.Text;
                    obj.DD_GTLQ_SO = txtSoCMND.Text.Trim();

                    if (txtSoCMND.Text.IsNullOrEmptyOrSpace())
                    {
                        //CommonFunction.ThongBaoChuaNhap(lblSoCMND.Content.ToString());
                        return;
                    }

                    ret = processKhachHang.KhachHang04(DatabaseConstant.Action.KIEM_TRA, ref obj, ref dsSource, ref lstChuKyHinhAnh, ref listClientResponseDetail);

                    if (ret)
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.NotExistDKKD", LMessage.MessageBoxType.Information);
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.ExistDKKD", LMessage.MessageBoxType.Warning);
                        //CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    }
                }
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
        */

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UtilitiesProcess process = new UtilitiesProcess();
                string objType = "KHACH_HANG";
                ApplicationConstant.UtilitesResponseMessage resMessage = ApplicationConstant.UtilitesResponseMessage.M_ResponseMessage_Utilities_ExistCMND;
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.TVIEN.layGiaTri()) ||
                    loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.CNHAN.layGiaTri()))
                {
                    if (txtSoCMND.IsNullOrEmpty())
                    {
                        CommonFunction.ThongBaoChuaNhap(lblSoCMND.Content.ToString());
                        txtSoCMND.Focus();
                        return;
                    }

                    if (!LObject.IsNullOrEmpty(txtSoCMND.Text))
                    {
                        if (!CommonFunction.IsValidFormat(ApplicationConstant.FormatType.CMND,
                            txtSoCMND.Text))
                        {
                            txtSoCMND.Focus();
                            return;
                        }
                    }

                    Mouse.OverrideCursor = Cursors.Wait; 

                    if (txtSoCMND.Text.IsNullOrEmptyOrSpace())
                    {
                        return;
                    }

                    ret = process.Utilites(DatabaseConstant.Action.KIEM_TRA, ApplicationConstant.FormatType.CMND, txtSoCMND.Text, objType, ref resMessage, ref listClientResponseDetail);

                    if (ret)
                    {
                        //LMessage.ShowMessage("M.ResponseMessage.Common.NotExistCMND", LMessage.MessageBoxType.Information);
                        LMessage.ShowMessage(ApplicationConstant.layGiaTri(resMessage), LMessage.MessageBoxType.Information);
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.NoResponse", LMessage.MessageBoxType.Warning);
                    }
                }
                else if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.DNGHIEP.layGiaTri()))
                {
                    if (txtSoCMND.IsNullOrEmpty())
                    {
                        CommonFunction.ThongBaoChuaNhap(lblSoCMND.Content.ToString());
                        txtSoCMND.Focus();
                        return;
                    }

                    Mouse.OverrideCursor = Cursors.Wait;

                    if (txtSoCMND.Text.IsNullOrEmptyOrSpace())
                    {
                        return;
                    }

                    ret = process.Utilites(DatabaseConstant.Action.KIEM_TRA, ApplicationConstant.FormatType.DKKD, txtSoCMND.Text, objType, ref resMessage, ref listClientResponseDetail);

                    if (ret)
                    {
                        //LMessage.ShowMessage("M.ResponseMessage.Common.NotExistDKKD", LMessage.MessageBoxType.Information);
                        LMessage.ShowMessage(ApplicationConstant.layGiaTri(resMessage), LMessage.MessageBoxType.Information);
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.NoResponse", LMessage.MessageBoxType.Warning);
                    }
                }
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

        private void btnCheckSoHoKhau_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UtilitiesProcess process = new UtilitiesProcess();
                string objType = "KHACH_HANG";
                ApplicationConstant.UtilitesResponseMessage resMessage = ApplicationConstant.UtilitesResponseMessage.M_ResponseMessage_Utilities_ExistSoHoKhau;
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.TVIEN.layGiaTri()) ||
                    loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.CNHAN.layGiaTri()))
                {
                    if (txtSoHoKhau.IsNullOrEmpty())
                    {
                        CommonFunction.ThongBaoChuaNhap(lblSoHoKhau.Content.ToString());
                        txtSoHoKhau.Focus();
                        return;
                    }

                    //if (!LObject.IsNullOrEmpty(txtSoHoKhau.Text))
                    //{
                    //    if (!CommonFunction.IsValidFormat(ApplicationConstant.FormatType.HOKHAU,
                    //        txtSoHoKhau.Text))
                    //    {
                    //        txtSoHoKhau.Focus();
                    //        return;
                    //    }
                    //}

                    Mouse.OverrideCursor = Cursors.Wait;

                    if (txtSoHoKhau.Text.IsNullOrEmptyOrSpace())
                    {
                        return;
                    }

                    ret = process.Utilites(DatabaseConstant.Action.KIEM_TRA, ApplicationConstant.FormatType.HOKHAU, txtSoHoKhau.Text, objType, ref resMessage, ref listClientResponseDetail);

                    if (ret)
                    {
                        //LMessage.ShowMessage("M.ResponseMessage.Common.NotExistSoHoKhau", LMessage.MessageBoxType.Information);
                        LMessage.ShowMessage(ApplicationConstant.layGiaTri(resMessage), LMessage.MessageBoxType.Information);
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.NoResponse", LMessage.MessageBoxType.Warning);
                    }
                }
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

        private void radioNongThonThuongChu_Checked(object sender, RoutedEventArgs e)
        {
            lstSourceHKXaPhuong.Clear();
            cmbXaPhuong.Items.Clear();

            if (cmbTinhTP.SelectedIndex >= 0 && cmbQuanHuyen.SelectedIndex >= 0)
            {
                if (lstSourceHKTinhTP.Select(i => i.DisplayName).Contains(cmbTinhTP.Text) && lstSourceHKQuanHuyen.Select(i => i.DisplayName).Contains(cmbQuanHuyen.Text))
                {
                    quan_huyen_HK = cmbQuanHuyen.Text;
                    AutoComboBox auto = new AutoComboBox();
                    List<string> lstDieuKien = new List<string>();
                    lstDieuKien.Add(lstSourceHKTinhTP.ElementAt(cmbTinhTP.SelectedIndex).KeywordStrings.ElementAt(1));
                    lstDieuKien.Add(lstSourceHKQuanHuyen.ElementAt(cmbQuanHuyen.SelectedIndex).KeywordStrings.ElementAt(1));
                    lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBan.XA_PHUONG.getValue());
                    if (radioNongThonThuongChu.IsChecked == true)
                        lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBanChiTiet.XA.getValue());
                    else if (radioThanhThiThuongChu.IsChecked == true)
                        lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBanChiTiet.PHUONG.getValue());
                    cmbXaPhuong.Items.Clear();
                    lstSourceHKXaPhuong.Clear();
                    auto.GenAutoComboBox(ref lstSourceHKXaPhuong, ref cmbXaPhuong, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DIABAN_CTIET), lstDieuKien);
                    if (lstSourceHKXaPhuong.Select(i => i.DisplayName).Contains(xa_phuong_HK))
                        cmbXaPhuong.Text = xa_phuong_HK;
                    else
                        cmbXaPhuong.Text = "";
                }
                else
                {
                    cmbXaPhuong.Text = "";
                }

                TaoDiaChi();
            }
            else
            {
                cmbXaPhuong.Items.Clear();
                lstSourceHKXaPhuong.Clear();
            }
        }

        private void radioThanhThiThuongChu_Checked(object sender, RoutedEventArgs e)
        {
            lstSourceHKXaPhuong.Clear();
            cmbXaPhuong.Items.Clear();

            if (cmbTinhTP.SelectedIndex >= 0 && cmbQuanHuyen.SelectedIndex >= 0)
            {
                if (lstSourceHKTinhTP.Select(i => i.DisplayName).Contains(cmbTinhTP.Text) && lstSourceHKQuanHuyen.Select(i => i.DisplayName).Contains(cmbQuanHuyen.Text))
                {
                    quan_huyen_HK = cmbQuanHuyen.Text;
                    AutoComboBox auto = new AutoComboBox();
                    List<string> lstDieuKien = new List<string>();
                    lstDieuKien.Add(lstSourceHKTinhTP.ElementAt(cmbTinhTP.SelectedIndex).KeywordStrings.ElementAt(1));
                    lstDieuKien.Add(lstSourceHKQuanHuyen.ElementAt(cmbQuanHuyen.SelectedIndex).KeywordStrings.ElementAt(1));
                    lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBan.XA_PHUONG.getValue());
                    if (radioNongThonThuongChu.IsChecked == true)
                        lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBanChiTiet.XA.getValue());
                    else if (radioThanhThiThuongChu.IsChecked == true)
                        lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBanChiTiet.PHUONG.getValue());
                    cmbXaPhuong.Items.Clear();
                    lstSourceHKXaPhuong.Clear();
                    auto.GenAutoComboBox(ref lstSourceHKXaPhuong, ref cmbXaPhuong, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DIABAN_CTIET), lstDieuKien);
                    if (lstSourceHKXaPhuong.Select(i => i.DisplayName).Contains(xa_phuong_HK))
                        cmbXaPhuong.Text = xa_phuong_HK;
                    else
                        cmbXaPhuong.Text = "";
                }
                else
                {
                    cmbXaPhuong.Text = "";
                }

                TaoDiaChi();
            }
            else
            {
                cmbXaPhuong.Items.Clear();
                lstSourceHKXaPhuong.Clear();
            }
        }

        private void radioNongThonHienTai_Checked(object sender, RoutedEventArgs e)
        {
            lstSourceXaPhuong.Clear();
            cmbXaPhuongHienTai.Items.Clear();

            if (cmbTinhTPHienTai.SelectedIndex >= 0 && cmbQuanHuyenHienTai.SelectedIndex >= 0)
            {
                if (lstSourceTinhTp.Select(i => i.DisplayName).Contains(cmbTinhTPHienTai.Text) && lstSourceQuanHuyen.Select(i => i.DisplayName).Contains(cmbQuanHuyenHienTai.Text))
                {
                    quan_huyen_hien_tai = cmbQuanHuyenHienTai.Text;
                    AutoComboBox auto = new AutoComboBox();
                    List<string> lstDieuKien = new List<string>();
                    lstDieuKien.Add(lstSourceTinhTp.ElementAt(cmbTinhTPHienTai.SelectedIndex).KeywordStrings.ElementAt(1));
                    lstDieuKien.Add(lstSourceQuanHuyen.ElementAt(cmbQuanHuyenHienTai.SelectedIndex).KeywordStrings.ElementAt(1));
                    lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBan.XA_PHUONG.getValue());
                    if (radioNongThonHienTai.IsChecked == true)
                        lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBanChiTiet.XA.getValue());
                    else if (radioThanhThiHienTai.IsChecked == true)
                        lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBanChiTiet.PHUONG.getValue());
                    cmbXaPhuongHienTai.Items.Clear();
                    lstSourceXaPhuong.Clear();
                    auto.GenAutoComboBox(ref lstSourceXaPhuong, ref cmbXaPhuongHienTai, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DIABAN_CTIET), lstDieuKien);
                    if (lstSourceXaPhuong.Select(i => i.DisplayName).Contains(xa_phuong_hien_tai))
                        cmbXaPhuongHienTai.Text = xa_phuong_hien_tai;
                    else
                        cmbXaPhuongHienTai.Text = "";
                }
                else
                {
                    cmbXaPhuongHienTai.Text = "";
                }
                TaoDiaChiHienTai();
            }
            else
            {
                cmbXaPhuongHienTai.Items.Clear();
                lstSourceXaPhuong.Clear();
            }
        }

        private void radioThanhThiHienTai_Checked(object sender, RoutedEventArgs e)
        {
            lstSourceXaPhuong.Clear();
            cmbXaPhuongHienTai.Items.Clear();

            if (cmbTinhTPHienTai.SelectedIndex >= 0 && cmbQuanHuyenHienTai.SelectedIndex >= 0)
            {
                if (lstSourceTinhTp.Select(i => i.DisplayName).Contains(cmbTinhTPHienTai.Text) && lstSourceQuanHuyen.Select(i => i.DisplayName).Contains(cmbQuanHuyenHienTai.Text))
                {
                    quan_huyen_hien_tai = cmbQuanHuyenHienTai.Text;
                    AutoComboBox auto = new AutoComboBox();
                    List<string> lstDieuKien = new List<string>();
                    lstDieuKien.Add(lstSourceTinhTp.ElementAt(cmbTinhTPHienTai.SelectedIndex).KeywordStrings.ElementAt(1));
                    lstDieuKien.Add(lstSourceQuanHuyen.ElementAt(cmbQuanHuyenHienTai.SelectedIndex).KeywordStrings.ElementAt(1));
                    lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBan.XA_PHUONG.getValue());
                    if (radioNongThonHienTai.IsChecked == true)
                        lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBanChiTiet.XA.getValue());
                    else if (radioThanhThiHienTai.IsChecked == true)
                        lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBanChiTiet.PHUONG.getValue());
                    cmbXaPhuongHienTai.Items.Clear();
                    lstSourceXaPhuong.Clear();
                    auto.GenAutoComboBox(ref lstSourceXaPhuong, ref cmbXaPhuongHienTai, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DIABAN_CTIET), lstDieuKien);
                    if (lstSourceXaPhuong.Select(i => i.DisplayName).Contains(xa_phuong_hien_tai))
                        cmbXaPhuongHienTai.Text = xa_phuong_hien_tai;
                    else
                        cmbXaPhuongHienTai.Text = "";
                }
                else
                {
                    cmbXaPhuongHienTai.Text = "";
                }
                TaoDiaChiHienTai();
            }
            else
            {
                cmbXaPhuongHienTai.Items.Clear();
                lstSourceXaPhuong.Clear();
            }
        }

        private void cmbTinhTP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTinhTP.SelectedIndex >= 0)
            {
                if (lstSourceHKTinhTP.Select(i => i.DisplayName).Contains(cmbTinhTP.Text))
                {
                    tinh_tp_HK = cmbTinhTP.Text;
                    AutoComboBox auto = new AutoComboBox();
                    //cmbTinhTP.SelectedIndex = lstSourceHKTinhTP.IndexOf(lstSourceHKTinhTP.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TinhTPDonViGiaoDich)));
                    List<string> lstDieuKien = new List<string>();
                    lstDieuKien.Add(lstSourceHKTinhTP.ElementAt(cmbTinhTP.SelectedIndex).KeywordStrings.ElementAt(1));
                    lstDieuKien.Add(null);
                    lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBan.HUYEN_QUAN.getValue());
                    cmbQuanHuyen.Items.Clear();
                    lstSourceHKQuanHuyen.Clear();
                    auto.GenAutoComboBox(ref lstSourceHKQuanHuyen, ref cmbQuanHuyen, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DIABAN), lstDieuKien);
                    if (lstSourceHKQuanHuyen.Select(i => i.DisplayName).Contains(quan_huyen_HK))
                        cmbQuanHuyen.Text = quan_huyen_HK;
                    else
                        cmbQuanHuyen.Text = "";
                }
                else
                {
                    cmbQuanHuyen.Text = "";
                }

                TaoDiaChi();
            }
            else
            {
                cmbQuanHuyen.Items.Clear();
                lstSourceHKQuanHuyen.Clear();
                cmbXaPhuong.Items.Clear();
                lstSourceHKXaPhuong.Clear();
            }
        }

        private void cmbQuanHuyen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTinhTP.SelectedIndex >= 0 && cmbQuanHuyen.SelectedIndex >= 0)
            {
                if (lstSourceHKTinhTP.Select(i => i.DisplayName).Contains(cmbTinhTP.Text) && lstSourceHKQuanHuyen.Select(i => i.DisplayName).Contains(cmbQuanHuyen.Text))
                {
                    quan_huyen_HK = cmbQuanHuyen.Text;
                    AutoComboBox auto = new AutoComboBox();
                    List<string> lstDieuKien = new List<string>();
                    lstDieuKien.Add(lstSourceHKTinhTP.ElementAt(cmbTinhTP.SelectedIndex).KeywordStrings.ElementAt(1));
                    lstDieuKien.Add(lstSourceHKQuanHuyen.ElementAt(cmbQuanHuyen.SelectedIndex).KeywordStrings.ElementAt(1));
                    lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBan.XA_PHUONG.getValue());
                    if (radioNongThonThuongChu.IsChecked == true)
                        lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBanChiTiet.XA.getValue());
                    else if (radioThanhThiThuongChu.IsChecked == true)
                        lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBanChiTiet.PHUONG.getValue());
                    cmbXaPhuong.Items.Clear();
                    lstSourceHKXaPhuong.Clear();
                    auto.GenAutoComboBox(ref lstSourceHKXaPhuong, ref cmbXaPhuong, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DIABAN_CTIET), lstDieuKien);
                    if (lstSourceHKXaPhuong.Select(i => i.DisplayName).Contains(xa_phuong_HK))
                        cmbXaPhuong.Text = xa_phuong_HK;
                    else
                        cmbXaPhuong.Text = "";
                }
                else
                {
                    cmbXaPhuong.Text = "";
                }

                TaoDiaChi();
            }
            else
            {
                cmbXaPhuong.Items.Clear();
                lstSourceHKXaPhuong.Clear();
            }
        }

        private void cmbXaPhuong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTinhTP.SelectedIndex >= 0 && cmbQuanHuyen.SelectedIndex >= 0 && cmbXaPhuong.SelectedIndex >= 0)
            {
                if (lstSourceHKTinhTP.Select(i => i.DisplayName).Contains(cmbTinhTP.Text) && 
                    lstSourceHKQuanHuyen.Select(i => i.DisplayName).Contains(cmbQuanHuyen.Text) &&
                    lstSourceHKXaPhuong.Select(i => i.DisplayName).Contains(cmbXaPhuong.Text))
                {
                    xa_phuong_HK = cmbXaPhuong.Text;
                    AutoComboBox auto = new AutoComboBox();
                    List<string> lstDieuKien = new List<string>();
                    lstDieuKien.Add(lstSourceHKTinhTP.ElementAt(cmbTinhTP.SelectedIndex).KeywordStrings.ElementAt(1));
                    lstDieuKien.Add(lstSourceHKXaPhuong.ElementAt(cmbXaPhuong.SelectedIndex).KeywordStrings.ElementAt(1));
                    lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBan.LANG_TODP.getValue());
                    cmbLangTodp.Items.Clear();
                    lstSourceHKLangTodp.Clear();
                    auto.GenAutoComboBox(ref lstSourceHKLangTodp, ref cmbLangTodp, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DIABAN), lstDieuKien);
                    if (lstSourceHKLangTodp.Select(i => i.DisplayName).Contains(lang_todp_HK))
                        cmbLangTodp.Text = lang_todp_HK;
                    else
                        cmbLangTodp.Text = "";
                }
                else
                {
                    cmbLangTodp.Text = "";
                }

                TaoDiaChi();
            }
            else
            {
                cmbLangTodp.Items.Clear();
                lstSourceHKLangTodp.Clear();
            }
        }

        //private void cmbXaPhuong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{            
        //    if (cmbXaPhuong.SelectedIndex >= 0)
        //    {
        //        xa_phuong_HK = cmbXaPhuong.Text;
        //        TaoDiaChi();
        //    }
        //}

        private void cmbLangTodp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbLangTodp.SelectedIndex >= 0)
            {
                lang_todp_HK = cmbLangTodp.Text;
                TaoDiaChi();
            }
        }

        private void cmbTinhTPHienTai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTinhTPHienTai.SelectedIndex >= 0)
            {
                if (lstSourceTinhTp.Select(i => i.DisplayName).Contains(cmbTinhTPHienTai.Text))
                {
                    tinh_tp_hien_tai = cmbTinhTPHienTai.Text;
                    AutoComboBox auto = new AutoComboBox();
                    //cmbTinhTPHienTai.SelectedIndex = lstSourceTinhTp.IndexOf(lstSourceTinhTp.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TinhTPDonViGiaoDich)));
                    List<string> lstDieuKien = new List<string>();
                    lstDieuKien.Add(lstSourceTinhTp.ElementAt(cmbTinhTPHienTai.SelectedIndex).KeywordStrings.ElementAt(1));
                    lstDieuKien.Add(null);
                    lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBan.HUYEN_QUAN.getValue());
                    cmbQuanHuyenHienTai.Items.Clear();
                    lstSourceQuanHuyen.Clear();
                    auto.GenAutoComboBox(ref lstSourceQuanHuyen, ref cmbQuanHuyenHienTai, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DIABAN), lstDieuKien);
                    if (lstSourceQuanHuyen.Select(i => i.DisplayName).Contains(quan_huyen_hien_tai))
                        cmbQuanHuyenHienTai.Text = quan_huyen_hien_tai;
                    else
                        cmbQuanHuyenHienTai.Text = "";
                }
                else
                {
                    cmbQuanHuyenHienTai.Text = "";
                }
                TaoDiaChiHienTai();
            }
            else
            {
                cmbQuanHuyenHienTai.Items.Clear();
                lstSourceQuanHuyen.Clear();
                cmbXaPhuongHienTai.Items.Clear();
                lstSourceXaPhuong.Clear();
            }
        }

        private void cmbQuanHuyenHienTai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTinhTPHienTai.SelectedIndex >= 0 && cmbQuanHuyenHienTai.SelectedIndex >= 0)
            {
                if (lstSourceTinhTp.Select(i => i.DisplayName).Contains(cmbTinhTPHienTai.Text) && lstSourceQuanHuyen.Select(i => i.DisplayName).Contains(cmbQuanHuyenHienTai.Text))
                {
                    quan_huyen_hien_tai = cmbQuanHuyenHienTai.Text;
                    AutoComboBox auto = new AutoComboBox();
                    List<string> lstDieuKien = new List<string>();
                    lstDieuKien.Add(lstSourceTinhTp.ElementAt(cmbTinhTPHienTai.SelectedIndex).KeywordStrings.ElementAt(1));
                    lstDieuKien.Add(lstSourceQuanHuyen.ElementAt(cmbQuanHuyenHienTai.SelectedIndex).KeywordStrings.ElementAt(1));
                    lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBan.XA_PHUONG.getValue());
                    if (radioNongThonHienTai.IsChecked == true)
                        lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBanChiTiet.XA.getValue());
                    else if (radioThanhThiHienTai.IsChecked == true)
                        lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBanChiTiet.PHUONG.getValue());
                    cmbXaPhuongHienTai.Items.Clear();
                    lstSourceXaPhuong.Clear();
                    auto.GenAutoComboBox(ref lstSourceXaPhuong, ref cmbXaPhuongHienTai, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DIABAN_CTIET), lstDieuKien);
                    if (lstSourceXaPhuong.Select(i => i.DisplayName).Contains(xa_phuong_hien_tai))
                        cmbXaPhuongHienTai.Text = xa_phuong_hien_tai;
                    else
                        cmbXaPhuongHienTai.Text = "";
                }
                else
                {
                    cmbXaPhuongHienTai.Text = "";
                } 
                TaoDiaChiHienTai();
            }
            else
            {
                cmbXaPhuongHienTai.Items.Clear();
                lstSourceXaPhuong.Clear();
            }
        }

        private void cmbXaPhuongHienTai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTinhTPHienTai.SelectedIndex >= 0 && cmbQuanHuyenHienTai.SelectedIndex >= 0 && cmbXaPhuongHienTai.SelectedIndex >= 0)
            {
                if (lstSourceTinhTp.Select(i => i.DisplayName).Contains(cmbTinhTPHienTai.Text) &&
                    lstSourceQuanHuyen.Select(i => i.DisplayName).Contains(cmbQuanHuyenHienTai.Text) &&
                    lstSourceXaPhuong.Select(i => i.DisplayName).Contains(cmbXaPhuongHienTai.Text))
                {
                    xa_phuong_hien_tai = cmbXaPhuongHienTai.Text;
                    AutoComboBox auto = new AutoComboBox();
                    List<string> lstDieuKien = new List<string>();
                    lstDieuKien.Add(lstSourceTinhTp.ElementAt(cmbTinhTPHienTai.SelectedIndex).KeywordStrings.ElementAt(1));
                    lstDieuKien.Add(lstSourceXaPhuong.ElementAt(cmbXaPhuongHienTai.SelectedIndex).KeywordStrings.ElementAt(1));
                    lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBan.LANG_TODP.getValue());
                    cmbLangTodpHienTai.Items.Clear();
                    lstSourceLangTodp.Clear();
                    auto.GenAutoComboBox(ref lstSourceLangTodp, ref cmbLangTodpHienTai, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DIABAN), lstDieuKien);
                    if (lstSourceLangTodp.Select(i => i.DisplayName).Contains(lang_todp_hien_tai))
                        cmbLangTodpHienTai.Text = lang_todp_hien_tai;
                    else
                        cmbLangTodpHienTai.Text = "";
                }
                else
                {
                    cmbLangTodpHienTai.Text = "";
                }

                TaoDiaChiHienTai();
            }
            else
            {
                cmbLangTodpHienTai.Items.Clear();
                lstSourceLangTodp.Clear();
            }
        }

        //private void cmbXaPhuongHienTai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (cmbXaPhuongHienTai.SelectedIndex >= 0)
        //    {
        //        xa_phuong_hien_tai = cmbXaPhuongHienTai.Text;
        //        TaoDiaChiHienTai();
        //    }
        //}

        private void cmbLangTodpHienTai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbLangTodpHienTai.SelectedIndex >= 0)
            {
                lang_todp_hien_tai = cmbLangTodpHienTai.Text;
                TaoDiaChiHienTai();
            }
        }

        private void cmbTinhTP_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                cmbTinhTP_SelectionChanged(null, null);
            }
        }

        private void cmbQuanHuyen_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                cmbQuanHuyen_SelectionChanged(null, null);
            }
        }

        private void cmbXaPhuong_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                cmbXaPhuong_SelectionChanged(null, null);
            }
        }

        private void cmbTinhTPHienTai_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                cmbTinhTPHienTai_SelectionChanged(null, null);
            }
        }

        private void cmbQuanHuyenHienTai_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                cmbQuanHuyenHienTai_SelectionChanged(null, null);
            }
        }

        private void cmbXaPhuongHienTai_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                cmbXaPhuongHienTai_SelectionChanged(null, null);
            }
        }

        private void txtSoDiDong_KeyDown(object sender, KeyEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtSoDiDong.Text, "[^0-9]"))
            {
                if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
                {
                    txtSoDiDong.Clear();
                }   
            }
        }

        private void txtSoCoDinh_KeyDown(object sender, KeyEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtSoCoDinh.Text, "[^0-9]"))
            {
                if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
                {
                    txtSoCoDinh.Clear();
                } 
            }
        }

        private void TaoDiaChi()
        {
            txtDiaChi.Text = "";
            txtDiaChi.Text = cmbLangTodp.Text;

            if (txtDiaChi.Text.IsNullOrEmptyOrSpace())
                txtDiaChi.Text = cmbXaPhuong.Text;
            else
                txtDiaChi.Text = txtDiaChi.Text + " - " + cmbXaPhuong.Text;

            if (txtDiaChi.Text.IsNullOrEmptyOrSpace())
                txtDiaChi.Text = cmbQuanHuyen.Text;
            else
                txtDiaChi.Text = txtDiaChi.Text + " - " + cmbQuanHuyen.Text;

            if (txtDiaChi.Text.IsNullOrEmptyOrSpace())
                txtDiaChi.Text = cmbTinhTP.Text;
            else
                txtDiaChi.Text = txtDiaChi.Text + " - " + cmbTinhTP.Text;

            // BIDV-MM: Ho khau thuong tru free text
            txtDiaChi.Text = "";
        }

        private void TaoDiaChiHienTai()
        {
            txtDiaChiHienTai.Text = "";
            txtDiaChiHienTai.Text = cmbLangTodpHienTai.Text;

            if (txtDiaChiHienTai.Text.IsNullOrEmptyOrSpace())
                txtDiaChiHienTai.Text = cmbXaPhuongHienTai.Text;
            else
                txtDiaChiHienTai.Text = txtDiaChiHienTai.Text + " - " + cmbXaPhuongHienTai.Text;

            if (txtDiaChiHienTai.Text.IsNullOrEmptyOrSpace())
                txtDiaChiHienTai.Text = cmbQuanHuyenHienTai.Text;
            else
                txtDiaChiHienTai.Text = txtDiaChiHienTai.Text + " - " + cmbQuanHuyenHienTai.Text;

            if (txtDiaChiHienTai.Text.IsNullOrEmptyOrSpace())
                txtDiaChiHienTai.Text = cmbTinhTPHienTai.Text;
            else
                txtDiaChiHienTai.Text = txtDiaChiHienTai.Text + " - " + cmbTinhTPHienTai.Text;
        }        

        private void btnAddNguoiThuaKe_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            ucGiaDinhNguoiThuaKe04 uc = new ucGiaDinhNguoiThuaKe04();
            uc.Action = DatabaseConstant.Action.THEM;
            uc.DuLieuTraVe = new ucGiaDinhNguoiThuaKe04.LayDuLieu(AddToGridNguoiThuaKe);
            window.Content = uc;
            window.Title = LLanguage.SearchResourceByKey("U.KhachHang.KhachHang.Popup.ucGiaDinhNguoiThuaKe.Tittle");
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();            
        }

        private void btnMaGoiNho_Click(object sender, RoutedEventArgs e)
        {
            lstPopup.Clear();
            List<string> lstDieuKien = new List<string>();
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation("POPUP_DS_DM_CONG_TY", lstDieuKien);
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
                txtMaGoiNho.Tag = Convert.ToInt32(lstPopup[0]["ID"]);
                txtMaGoiNho.Text = Convert.ToString(lstPopup[0]["MA_GOI_NHO"]);
                txtTenCongTy.Text = Convert.ToString(lstPopup[0]["TEN_CONG_TY"]);
                txtChiNhanh.Text = Convert.ToString(lstPopup[0]["TEN_CHI_NHANH"]);
                txtDiaChiCongTy.Text = Convert.ToString(lstPopup[0]["DIA_CHI"]);
                cmbLoaiHinhCongTy.SelectedItem = lstSourceLoaiHinhCongTy.IndexOf(lstSourceLoaiHinhCongTy.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(lstPopup[0]["DIA_CHI"].ToString())));
            }
        }
        public void AddToGridNguoiThuaKe(DataRow dr)
        {
            dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows.Add(dr.ItemArray);

            for (int i = 0; i < dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows.Count; i++)
            {
                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["STT"] = i + 1;
            }

            grNguoiThuaKe.DataContext = dsSource.Tables["VKH_GDINH_NGUOI_TKE"].DefaultView;
        }

        private void btnModifyNguoiThuaKe_Click(object sender, RoutedEventArgs e)
        {
            if (grNguoiThuaKe == null)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                return;
            }

            DataRowView drx = (DataRowView)grNguoiThuaKe.SelectedItem;
            if (drx == null)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                return;
            }

            if (grNguoiThuaKe.SelectedItems.Count > 1)
            {
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                Window window = new Window();
                ucGiaDinhNguoiThuaKe04 uc = new ucGiaDinhNguoiThuaKe04();
                DataRowView dr = (DataRowView)grNguoiThuaKe.SelectedItem;
                int i = Convert.ToInt32(dr["STT"]);
                uc.drSource = dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i-1];
                uc.Action = DatabaseConstant.Action.SUA;
                uc.DuLieuTraVe = new ucGiaDinhNguoiThuaKe04.LayDuLieu(EditToGridNguoiThuaKe);
                window.Content = uc;
                window.Title = LLanguage.SearchResourceByKey("U.KhachHang.KhachHang.Popup.ucGiaDinhNguoiThuaKe.Tittle");
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;                                
                window.ShowDialog();
            }
        }

        private void btnViewNguoiThuaKe_Click(object sender, RoutedEventArgs e)
        {
            if (grNguoiThuaKe == null)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                return;
            }

            DataRowView drx = (DataRowView)grNguoiThuaKe.SelectedItem;
            if (drx == null)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                return;
            }

            if (grNguoiThuaKe.SelectedItems.Count > 1)
            {
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                Window window = new Window();
                ucGiaDinhNguoiThuaKe04 uc = new ucGiaDinhNguoiThuaKe04();
                DataRowView dr = (DataRowView)grNguoiThuaKe.SelectedItem;
                int i = Convert.ToInt32(dr["STT"]);
                uc.drSource = dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i - 1];
                uc.Action = DatabaseConstant.Action.XEM;
                uc.DuLieuTraVe = new ucGiaDinhNguoiThuaKe04.LayDuLieu(EditToGridNguoiThuaKe);
                window.Content = uc;
                window.Title = LLanguage.SearchResourceByKey("U.KhachHang.KhachHang.Popup.ucGiaDinhNguoiThuaKe.Tittle");
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
            }
        }

        private void grNguoiThuaKe_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Window window = new Window();
            ucGiaDinhNguoiThuaKe04 uc = new ucGiaDinhNguoiThuaKe04();
            DataRowView dr = (DataRowView)grNguoiThuaKe.SelectedItem;
            int i = Convert.ToInt32(dr["STT"]);
            uc.drSource = dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i - 1];
            uc.Action = DatabaseConstant.Action.XEM;
            uc.DuLieuTraVe = new ucGiaDinhNguoiThuaKe04.LayDuLieu(EditToGridNguoiThuaKe);
            window.Content = uc;
            window.Title = LLanguage.SearchResourceByKey("U.KhachHang.KhachHang.Popup.ucGiaDinhNguoiThuaKe.Tittle");
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }

        public void EditToGridNguoiThuaKe(DataRow dr)
        {
            int i = Convert.ToInt32(dr["STT"]) - 1;

            if (i >= 0)
            {                
                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["VAI_TRO"] = dr["VAI_TRO"];
                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["HO_TEN"] = dr["HO_TEN"];
                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["NGAY_SINH"] = dr["NGAY_SINH"];
                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["GIOI_TINH"] = dr["GIOI_TINH"];
                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["DAN_TOC"] = dr["DAN_TOC"];
                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["QH_VOI_TVIEN"] = dr["QH_VOI_TVIEN"];
                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["TDO_HVAN"] = dr["TDO_HVAN"];
                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["NGHE_NGHIEP"] = dr["NGHE_NGHIEP"];
                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["SUC_KHOE"] = dr["SUC_KHOE"];
                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["SO_CMND"] = dr["SO_CMND"];
                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["NGAY_CAP"] = dr["NGAY_CAP"];
                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["NOI_CAP"] = dr["NOI_CAP"];
                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["CUNG_HKHAU_TVIEN"] = dr["CUNG_HKHAU_TVIEN"];
                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["SO_HKHAU"] = dr["SO_HKHAU"];
                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["SDT"] = dr["SDT"];
                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["DIA_CHI"] = dr["DIA_CHI"];
                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["GHI_CHU"] = dr["GHI_CHU"];
                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["TEN_BO"] = dr["TEN_BO"];

                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["TEN_BAN_DIA"] = dr["TEN_BAN_DIA"];
                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["EMAIL"] = dr["EMAIL"];
                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["DIA_CHI_NOI_CAP"] = dr["DIA_CHI_NOI_CAP"];

            }

            for (int j = 0; j < dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows.Count; j++)
            {
                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[j]["STT"] = j + 1;
            }

            grNguoiThuaKe.DataContext = dsSource.Tables["VKH_GDINH_NGUOI_TKE"].DefaultView;
        }

        private void btnDeleteNguoiThuaKe_Click(object sender, RoutedEventArgs e)
        {
            List<int> lstSTT = new List<int>();
            for (int i = 0; i < grNguoiThuaKe.SelectedItems.Count; i++)
            {
                DataRowView dr = (DataRowView)grNguoiThuaKe.SelectedItems[i];
                lstSTT.Add(Convert.ToInt32(dr["STT"]));
            }
            lstSTT.SortByDesc();
            foreach (int stt in lstSTT)
                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows.RemoveAt(stt - 1);

            for (int i = 0; i < dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows.Count; i++)
            {
                dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["STT"] = i + 1;
            }

            grNguoiThuaKe.DataContext = dsSource.Tables["VKH_GDINH_NGUOI_TKE"].DefaultView;
        }       
        
        private void chkAll_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ImportRows(ref DataTable dtSource, DataTable dtAdd)
        {
            DataRow[] drRemove = dtSource.Select("CHON=1");
            if (drRemove.Length > 0)
            {
                foreach (DataRow dr in drRemove)
                {
                    dtSource.Rows.Remove(dr);
                }
            }

            foreach (DataRow dr in dtAdd.Rows)
            {
                dtSource.ImportRow(dr);
            }

            //Set lại số STT
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                dtSource.Rows[i]["STT"] = i + 1;
                dtSource.Rows[i]["CHON"] = false;
            }
        }

        private void tlbAddNguoiDaiDien_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            ucNguoiDaiDien uc = new ucNguoiDaiDien();
            window.Content = uc;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.RenderSize = new Size(1024, 768);
            window.Title = LLanguage.SearchResourceByKey("U.POPUP.POPUP_NGUOI_DAI_DIEN");
            window.ShowDialog();
            DataTable dtGiaDinh = dsSource.Tables["VKH_NGUOI_DDIEN"];
            if (uc.dtSource != null)
            {
                ImportRows(ref dtGiaDinh, uc.dtSource);                
            }
            uc = null;
            grNguoiDaiDien.DataContext = dsSource.Tables["VKH_NGUOI_DDIEN"].DefaultView;
            grNguoiDaiDien.Rebind();
        }

        /// <summary>
        /// Sửa thông tin người đại diện
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbModifyNguoiDaiDien_Click(object sender, RoutedEventArgs e)
        {
            if (grNguoiDaiDien == null)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                return;
            }

            DataRowView dr = (DataRowView)grNguoiDaiDien.SelectedItem;
            if (dr == null)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                return;
            }

            Window window = new Window();
            ucNguoiDaiDien uc = new ucNguoiDaiDien();

            foreach(DataRowView item in grNguoiDaiDien.SelectedItems)
            {
                int i = Convert.ToInt32(item["STT"]);
                uc.dtSource.ImportRow(dsSource.Tables["VKH_NGUOI_DDIEN"].Rows[i-1]);
            }
            if (uc.dtSource.Rows.Count == 0)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                uc = null;
                return;
            }
            window.Content = uc;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.RenderSize = new Size(1024, 768);
            window.Title = LLanguage.SearchResourceByKey("U.POPUP.POPUP_NGUOI_DAI_DIEN");
            window.ShowDialog();
            if (uc.dtSource != null)
            {
                DataTable dt = dsSource.Tables["VKH_NGUOI_DDIEN"];
                ImportRows(ref dt, uc.dtSource);
            }
            uc = null;
            grNguoiDaiDien.DataContext = dsSource.Tables["VKH_NGUOI_DDIEN"].DefaultView;
            grNguoiDaiDien.Rebind();
        }

        /// <summary>
        /// Xóa người đại diện
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbDeleteNguoiDaiDien_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<int> lstSTT = new List<int>();
                for (int i = 0; i < grNguoiDaiDien.SelectedItems.Count; i++)
                {
                    DataRowView dr = (DataRowView)grNguoiDaiDien.SelectedItems[i];
                    lstSTT.Add(Convert.ToInt32(dr["STT"]));
                }
                lstSTT.SortByDesc();
                foreach (int stt in lstSTT)
                    dsSource.Tables["VKH_NGUOI_DDIEN"].Rows.RemoveAt(stt - 1);

                for (int i = 0; i < dsSource.Tables["VKH_NGUOI_DDIEN"].Rows.Count; i++)
                {
                    dsSource.Tables["VKH_NGUOI_DDIEN"].Rows[i]["STT"] = i + 1;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void txtThuNhapKhac_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            txtTongThuNhap.Value = txtThuNhapKhac.Value.GetValueOrDefault() + txtLuongCoBan.Value.GetValueOrDefault();
        }

        private void txtLuongCoBan_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            txtTongThuNhap.Value = txtThuNhapKhac.Value.GetValueOrDefault() + txtLuongCoBan.Value.GetValueOrDefault();
        }

        #endregion

        #region Xử lý nghiệp vụ
        private void GetFormData(ref KH_KHANG_HSO obj, string sTrangThaiNVu)
        {
            try
            {
                obj = new KH_KHANG_HSO();

                #region Khách hàng thành viên
                if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.TVIEN.layGiaTri()))
                {
                    #region Thông tin chung
                    obj.ID = id;
                    obj.ID_DON_VI = ClientInformation.IdDonViGiaoDich;
                    obj.ID_KHU_VUC = idKhuVuc;
                    obj.ID_CUM = idCum;
                    obj.ID_NHOM = idNhom;
                    obj.MA_KHANG_LOAI = loaiKhachHang;
                    obj.MA_KHANG = txtMaKhachHang.Text;
                    obj.MA_TVIEN = obj.MA_KHANG;
                    obj.TEN_KHANG = txtTenKhachHang.Text;
                    obj.TEN_GDICH = obj.TEN_KHANG;
                    obj.TEN_BAN_DIA = txtTenBanDia.Text;
                    if (raddtNgayCongNhan.Value != null)
                        obj.NGAY_THAM_GIA = Convert.ToDateTime(raddtNgayCongNhan.Value).ToString("yyyyMMdd");
                    if (raddtNgayHetHL.Value != null)
                        obj.NGAY_HET_HLUC = Convert.ToDateTime(raddtNgayHetHL.Value).ToString("yyyyMMdd");
                    //if(cmbLyDoRaKhoiNhom.SelectedIndex>=0)
                    //    obj.MA_LY_DO = lstSourceLyDoRa.ElementAt(cmbLyDoRaKhoiNhom.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (raddtNgaySinh.Value != null)
                        obj.DD_NGAY_SINH = Convert.ToDateTime(raddtNgaySinh.Value).ToString("yyyyMMdd");
                    if (cmbGioiTinh.SelectedIndex >= 0)
                        obj.DD_GIOI_TINH = lstSourceGioiTinh.ElementAt(cmbGioiTinh.SelectedIndex).KeywordStrings.ElementAt(0);
                    obj.DD_GTLQ_LOAI = BusinessConstant.LOAI_GIAY_TO.CHUNG_MINH_ND.layGiaTri();
                    obj.DD_GTLQ_SO = txtSoCMND.Text;
                    if (raddtNgayCap.Value != null)
                        obj.DD_GTLQ_NGAY_CAP = Convert.ToDateTime(raddtNgayCap.Value).ToString("yyyyMMdd");
                    obj.DD_GTLQ_NOI_CAP = txtNoiCap.Text;
                    if(cmbDanToc.SelectedIndex >= 0)
                        obj.DD_MA_DAN_TOC = lstSourceDanToc.ElementAt(cmbDanToc.SelectedIndex).KeywordStrings.ElementAt(0);
                    obj.SO_HO_KHAU = txtSoHoKhau.Text;
                    if(cmbTinhTrangHonNhan.SelectedIndex >= 0)
                        obj.DD_MA_TTRANG_HNHAN = lstSourceHonNhan.ElementAt(cmbTinhTrangHonNhan.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbNgheNghiep.SelectedIndex >= 0)
                        obj.MA_NGHE_NGHIEP = lstSourceNgheNghiep.ElementAt(cmbNgheNghiep.SelectedIndex).KeywordStrings.ElementAt(0);
                    obj.NGAY_CONG_NHAN = obj.NGAY_THAM_GIA;
                    obj.TEN_BO = txtTenBo.Text;
                    #endregion

                    #region Hộ khẩu thường chú
                    if (radioNongThonThuongChu.IsChecked == true)
                        obj.MA_TTHI_NTHON = "NONG_THON";
                    else if (radioThanhThiThuongChu.IsChecked == true)
                        obj.MA_TTHI_NTHON = "THANH_THI";

                    obj.DD_TTRU_DIA_CHI = txtDiaChi.Text.Trim();
                    if (cmbVungMien.SelectedIndex >= 0)
                        obj.DD_TTRU_MA_VUNG = lstSourceHKVungMien.ElementAt(cmbVungMien.SelectedIndex).KeywordStrings.ElementAt(0);
                    if(cmbTinhTP.SelectedIndex>=0)
                        obj.DD_TTRU_MA_TINHTP = lstSourceHKTinhTP.ElementAt(cmbTinhTP.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbQuanHuyen.SelectedIndex >= 0)
                        obj.DD_TTRU_MA_QUAN = lstSourceHKQuanHuyen.ElementAt(cmbQuanHuyen.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbXaPhuong.SelectedIndex >= 0)
                        obj.DD_TTRU_MA_PHUONG = lstSourceHKXaPhuong.ElementAt(cmbXaPhuong.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbLangTodp.SelectedIndex >= 0)
                        obj.DD_TTRU_MA_TODP = lstSourceHKLangTodp.ElementAt(cmbLangTodp.SelectedIndex).KeywordStrings.ElementAt(0);
                    #endregion

                    #region Địa chỉ hiện tại
                    if (radioNongThonHienTai.IsChecked == true)
                        obj.MA_TTHI_NTHON_HTAI = "NONG_THON";
                    else if (radioThanhThiHienTai.IsChecked == true)
                        obj.MA_TTHI_NTHON_HTAI = "THANH_THI";

                    obj.DIA_CHI = txtDiaChiHienTai.Text.Trim();
                    if (cmbVungMienHienTai.SelectedIndex >= 0)
                        obj.MA_VUNG = lstSourceVungMien.ElementAt(cmbVungMienHienTai.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbTinhTPHienTai.SelectedIndex >= 0)
                        obj.MA_TINHTP = lstSourceTinhTp.ElementAt(cmbTinhTPHienTai.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbQuanHuyenHienTai.SelectedIndex >= 0)
                        obj.MA_QUAN = lstSourceQuanHuyen.ElementAt(cmbQuanHuyenHienTai.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbXaPhuongHienTai.SelectedIndex >= 0)
                        obj.MA_PHUONG = lstSourceXaPhuong.ElementAt(cmbXaPhuongHienTai.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbLangTodpHienTai.SelectedIndex >= 0)
                        obj.MA_TODP = lstSourceLangTodp.ElementAt(cmbLangTodpHienTai.SelectedIndex).KeywordStrings.ElementAt(0);
                    #region Thông tin cư trú
                    if (!dsSource.Tables["VKH_THONG_TIN_CU_TRU"].IsNullOrEmpty())
                    {
                        DataRow dr = dsSource.Tables["VKH_THONG_TIN_CU_TRU"].NewRow();
                        if(cmbLoaiHinhCuTru.SelectedIndex>-1)
                        {
                            AutoCompleteEntry au = lstSourceLoaiHinhCuTru.ElementAt(cmbLoaiHinhCuTru.SelectedIndex);
                            if (!au.IsNullOrEmpty())
                                dr["LOAI_HINH_CTRU"] = au.KeywordStrings.FirstOrDefault();
                        }
                        dr["MO_TA_LHINH_CTRU"] = txtLoaiHinhCuTru.Text;
                        string quanHeCuTru = "";
                        if (cmbOVoiAi.SelectedIndex >= 0)
                        {
                            AutoCompleteEntry au = lstSourceQuanHeCuTru.ElementAt(cmbOVoiAi.SelectedIndex);
                            if (!au.IsNullOrEmpty())
                                quanHeCuTru = au.KeywordStrings.FirstOrDefault();
                        }
                        quanHeCuTru += "#" + txtOVoiAi.Text;
                        dr["THANH_PHAN"] = quanHeCuTru;
                        dr["TGIAN_O_NAM"] = txtSoNamCuTru.Value.GetValueOrDefault();
                        dr["TGIAN_O_THANG"] = txtSoThangCuTru.Value.GetValueOrDefault();

                        dsSource.Tables["VKH_THONG_TIN_CU_TRU"].Rows.Clear();
                        dsSource.Tables["VKH_THONG_TIN_CU_TRU"].Rows.Add(dr);
                    }

                    #endregion
                    #endregion

                    #region Số điện thoại email
                    obj.SO_DTHOAI = txtSoCoDinh.Text;
                    obj.SO_DDONG = txtSoDiDong.Text;
                    obj.EMAIL = txtEmail.Text;
                    if (cmbEmail.SelectedIndex >= 0)
                    {
                        AutoCompleteEntry auEmail = lstSourceLoaiEmail.ElementAt(cmbEmail.SelectedIndex);
                        if (!auEmail.IsNullOrEmpty())
                            obj.EMAIL_TYPE = auEmail.KeywordStrings.FirstOrDefault();
                    }
                    #endregion

                    #region Công việc hiện tại
                    if (!dsSource.Tables["VKH_CONG_VIEC_HIEN_TAI"].IsNullOrEmpty())
                    {
                        DataRow dr = dsSource.Tables["VKH_CONG_VIEC_HIEN_TAI"].NewRow();
                        dr["MA_GOI_NHO"] = txtMaGoiNho.Text;
                        dr["ID_CONG_TY"] = Convert.ToInt32(txtMaGoiNho.Tag);
                        if (chkLuuThongTin.IsChecked.GetValueOrDefault())
                            dr["LUU_THONG_TIN"] = BusinessConstant.CoKhong.CO.layGiaTri();
                        else
                            dr["LUU_THONG_TIN"] = BusinessConstant.CoKhong.KHONG.layGiaTri();
                        dr["TEN_CONG_TY"] = txtTenCongTy.Text;
                        dr["TEN_CHI_NHANH"] = txtChiNhanh.Text;
                        dr["DIA_CHI"] = txtDiaChiCongTy.Text;
                        dr["SO_DIEN_THOAI"] = txtSoDienThoai.Text;
                        dr["MAY_LE"] = txtNhanhSo.Text;
                        string tgianlhe = "";
                        if (!telLienHeSangTu.Value.IsNullOrEmpty())
                            tgianlhe = telLienHeSangTu.Value.GetValueOrDefault().ToString("yyyyMMddHHmm");
                        tgianlhe += "#";
                        if (!telLienHeSangDen.Value.IsNullOrEmpty())
                            tgianlhe += telLienHeSangDen.Value.GetValueOrDefault().ToString("yyyyMMddHHmm");
                        dr["TGIAN_LHE_SANG"] = tgianlhe;
                        tgianlhe = "";
                        if (!telLienHeChieuTu.Value.IsNullOrEmpty())
                            tgianlhe = telLienHeChieuTu.Value.GetValueOrDefault().ToString("yyyyMMddHHmm");
                        tgianlhe += "#";
                        if (!telLienHeChieuDen.Value.IsNullOrEmpty())
                            tgianlhe += telLienHeChieuDen.Value.GetValueOrDefault().ToString("yyyyMMddHHmm");
                        dr["TGIAN_LHE_CHIEU"] = tgianlhe;
                        dr["PHONG_BAN"] = txtPhongBan.Text;
                        dr["VI_TRI_HIEN_TAI"] = txtViTriHienTai.Text;
                        tgianlhe = "";
                        if (!txtSoNamCongTac.Value.IsNullOrEmpty())
                            tgianlhe += txtSoNamCongTac.Value.GetValueOrDefault().ToString();
                        tgianlhe += "#";
                        if (!txtSoThangCongTac.Value.IsNullOrEmpty())
                            tgianlhe += txtSoThangCongTac.Value.GetValueOrDefault().ToString();
                        dr["TGIAN_CONGTAC"] = tgianlhe;
                        if (cmbLoaiHinhCongTy.SelectedIndex >= 0)
                        {
                            AutoCompleteEntry au = lstSourceLoaiHinhCongTy.ElementAt(cmbLoaiHinhCongTy.SelectedIndex);
                            if (!au.IsNullOrEmpty())
                                dr["LOAI_HINH_CONG_TY"] = au.KeywordStrings.FirstOrDefault();
                        }
                        if (cmbNganhNghe.SelectedIndex >= 0)
                        {
                            AutoCompleteEntry au = lstSourceNganhNgheKD.ElementAt(cmbNganhNghe.SelectedIndex);
                            if (!au.IsNullOrEmpty())
                                dr["NNGHE_KDOANH"] = au.KeywordStrings.FirstOrDefault();
                        }

                        dr["LUONG_CO_BAN"] = txtLuongCoBan.Value.GetValueOrDefault().ToString();
                        dr["THU_NHAP_KHAC"] = txtThuNhapKhac.Value.GetValueOrDefault().ToString();
                        if (!teldtNgayNhanLuong.Value.IsNullOrEmpty())
                            dr["NGAY_NHAN_LUONG"] = teldtNgayNhanLuong.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                        dsSource.Tables["VKH_CONG_VIEC_HIEN_TAI"].Rows.Clear();
                        dsSource.Tables["VKH_CONG_VIEC_HIEN_TAI"].Rows.Add(dr);
                    }

                    #endregion

                    #region Công việc người tham chiếu
                    if (!dsSource.Tables["VKH_CONG_VIEC_NGUOI_THAM_CHIEU"].IsNullOrEmpty())
                    {
                        DataRow dr = dsSource.Tables["VKH_CONG_VIEC_NGUOI_THAM_CHIEU"].NewRow();
                        dr["HO_TEN"] = txtHoTenThamChieu.Text;
                        if (cmbQuanHeThamChieu.SelectedIndex >= 0)
                        {
                            AutoCompleteEntry au = lstSourceMoiQuanHeTChieu.ElementAt(cmbQuanHeThamChieu.SelectedIndex);
                            if (!au.IsNullOrEmpty())
                                dr["MOI_QUAN_HE"] = au.KeywordStrings.FirstOrDefault();
                        }
                        dr["MO_TA_MOI_QHE"] = txtMoTaQuanHe.Text;
                        dr["SO_DI_DONG"] = txtSoDiDongTChieu.Text;
                        dr["SO_DIEN_THOAI"] = txtSoDienThoaiTChieu.Text;
                        dr["DIA_CHI"] = txtDiaChiHienTaiTChieu.Text;
                        dsSource.Tables["VKH_CONG_VIEC_NGUOI_THAM_CHIEU"].Rows.Clear();
                        dsSource.Tables["VKH_CONG_VIEC_NGUOI_THAM_CHIEU"].Rows.Add(dr);
                    }

                    #endregion

                    #region Thông tin kiểm soát
                    obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    obj.TTHAI_NVU = sTrangThaiNVu;
                    obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                    obj.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    obj.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    if (action != DatabaseConstant.Action.THEM)
                    {
                        obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                        obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                        if (bHetHLuc && !chkKhachHangHetHL.IsChecked.Value)
                            obj.NGAY_CONG_NHAN = ClientInformation.NgayLamViecHienTai;
                        else
                            obj.NGAY_CONG_NHAN = ngayCongNhan;
                    }
                    #endregion                    
                }
                #endregion

                #region Khách hàng cá nhân
                else if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.CNHAN.layGiaTri()))
                {
                    #region Thông tin chung
                    obj.ID = id;
                    obj.ID_DON_VI = ClientInformation.IdDonViGiaoDich;
                    obj.ID_KHU_VUC = idKhuVuc;
                    obj.ID_CUM = idCum;
                    obj.ID_NHOM = idNhom;
                    obj.MA_KHANG_LOAI = loaiKhachHang;
                    obj.MA_KHANG = txtMaKhachHang.Text;
                    obj.MA_TVIEN = obj.MA_KHANG;
                    obj.TEN_KHANG = txtTenKhachHang.Text;
                    obj.TEN_GDICH = obj.TEN_KHANG;
                    obj.TEN_BAN_DIA = txtTenBanDia.Text;
                    if (raddtNgayCongNhan.Value != null)
                        obj.NGAY_THAM_GIA = Convert.ToDateTime(raddtNgayCongNhan.Value).ToString("yyyyMMdd");
                    if (raddtNgayHetHL.Value != null)
                        obj.NGAY_HET_HLUC = Convert.ToDateTime(raddtNgayHetHL.Value).ToString("yyyyMMdd");
                    //if(cmbLyDoRaKhoiNhom.SelectedIndex>=0)
                    //    obj.MA_LY_DO = lstSourceLyDoRa.ElementAt(cmbLyDoRaKhoiNhom.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (raddtNgaySinh.Value != null)
                        obj.DD_NGAY_SINH = Convert.ToDateTime(raddtNgaySinh.Value).ToString("yyyyMMdd");
                    if (cmbGioiTinh.SelectedIndex >= 0)
                        obj.DD_GIOI_TINH = lstSourceGioiTinh.ElementAt(cmbGioiTinh.SelectedIndex).KeywordStrings.ElementAt(0);
                    obj.DD_GTLQ_LOAI = BusinessConstant.LOAI_GIAY_TO.CHUNG_MINH_ND.layGiaTri();
                    obj.DD_GTLQ_SO = txtSoCMND.Text;
                    if (raddtNgayCap.Value != null)
                        obj.DD_GTLQ_NGAY_CAP = Convert.ToDateTime(raddtNgayCap.Value).ToString("yyyyMMdd");
                    obj.DD_GTLQ_NOI_CAP = txtNoiCap.Text;
                    if (cmbDanToc.SelectedIndex >= 0)
                        obj.DD_MA_DAN_TOC = lstSourceDanToc.ElementAt(cmbDanToc.SelectedIndex).KeywordStrings.ElementAt(0);
                    obj.SO_HO_KHAU = txtSoHoKhau.Text;
                    if (cmbTinhTrangHonNhan.SelectedIndex >= 0)
                        obj.DD_MA_TTRANG_HNHAN = lstSourceHonNhan.ElementAt(cmbTinhTrangHonNhan.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbNgheNghiep.SelectedIndex >= 0)
                        obj.MA_NGHE_NGHIEP = lstSourceNgheNghiep.ElementAt(cmbNgheNghiep.SelectedIndex).KeywordStrings.ElementAt(0);
                    obj.NGAY_CONG_NHAN = obj.NGAY_THAM_GIA;
                    obj.TEN_BO = txtTenBo.Text;
                    #endregion

                    #region Hộ khẩu thường chú
                    if (radioNongThonThuongChu.IsChecked == true)
                        obj.MA_TTHI_NTHON = "NONG_THON";
                    else if (radioThanhThiThuongChu.IsChecked == true)
                        obj.MA_TTHI_NTHON = "THANH_THI";

                    obj.DD_TTRU_DIA_CHI = txtDiaChi.Text.Trim();
                    if (cmbVungMien.SelectedIndex >= 0)
                        obj.DD_TTRU_MA_VUNG = lstSourceHKVungMien.ElementAt(cmbVungMien.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbTinhTP.SelectedIndex >= 0)
                        obj.DD_TTRU_MA_TINHTP = lstSourceHKTinhTP.ElementAt(cmbTinhTP.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbQuanHuyen.SelectedIndex >= 0)
                        obj.DD_TTRU_MA_QUAN = lstSourceHKQuanHuyen.ElementAt(cmbQuanHuyen.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbXaPhuong.SelectedIndex >= 0)
                        obj.DD_TTRU_MA_PHUONG = lstSourceHKXaPhuong.ElementAt(cmbXaPhuong.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbLangTodp.SelectedIndex >= 0)
                        obj.DD_TTRU_MA_TODP = lstSourceHKLangTodp.ElementAt(cmbLangTodp.SelectedIndex).KeywordStrings.ElementAt(0);
                    #endregion

                    #region Địa chỉ hiện tại
                    if (radioNongThonHienTai.IsChecked == true)
                        obj.MA_TTHI_NTHON_HTAI = "NONG_THON";
                    else if (radioThanhThiHienTai.IsChecked == true)
                        obj.MA_TTHI_NTHON_HTAI = "THANH_THI";

                    obj.DIA_CHI = txtDiaChiHienTai.Text.Trim();
                    if (cmbVungMienHienTai.SelectedIndex >= 0)
                        obj.MA_VUNG = lstSourceVungMien.ElementAt(cmbVungMienHienTai.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbTinhTPHienTai.SelectedIndex >= 0)
                        obj.MA_TINHTP = lstSourceTinhTp.ElementAt(cmbTinhTPHienTai.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbQuanHuyenHienTai.SelectedIndex >= 0)
                        obj.MA_QUAN = lstSourceQuanHuyen.ElementAt(cmbQuanHuyenHienTai.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbXaPhuongHienTai.SelectedIndex >= 0)
                        obj.MA_PHUONG = lstSourceXaPhuong.ElementAt(cmbXaPhuongHienTai.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbLangTodpHienTai.SelectedIndex >= 0)
                        obj.MA_TODP = lstSourceLangTodp.ElementAt(cmbLangTodpHienTai.SelectedIndex).KeywordStrings.ElementAt(0);

                    #region Thông tin cư trú
                    if (!dsSource.Tables["VKH_THONG_TIN_CU_TRU"].IsNullOrEmpty())
                    {
                        DataRow dr = dsSource.Tables["VKH_THONG_TIN_CU_TRU"].NewRow();
                        if (cmbLoaiHinhCuTru.SelectedIndex > -1)
                        {
                            AutoCompleteEntry au = lstSourceLoaiHinhCuTru.ElementAt(cmbLoaiHinhCuTru.SelectedIndex);
                            if (!au.IsNullOrEmpty())
                                dr["LOAI_HINH_CTRU"] = au.KeywordStrings.FirstOrDefault();
                        }
                        dr["MO_TA_LHINH_CTRU"] = txtLoaiHinhCuTru.Text;
                        string quanHeCuTru = "";
                        if (cmbOVoiAi.SelectedIndex >= 0)
                        {
                            AutoCompleteEntry au = lstSourceQuanHeCuTru.ElementAt(cmbOVoiAi.SelectedIndex);
                            if (!au.IsNullOrEmpty())
                                quanHeCuTru = au.KeywordStrings.FirstOrDefault();
                        }
                        quanHeCuTru += "#" + txtOVoiAi.Text;
                        dr["THANH_PHAN"] = quanHeCuTru;
                        dr["TGIAN_O_NAM"] = txtSoNamCuTru.Value.GetValueOrDefault();
                        dr["TGIAN_O_THANG"] = txtSoThangCuTru.Value.GetValueOrDefault();

                        dsSource.Tables["VKH_THONG_TIN_CU_TRU"].Rows.Clear();
                        dsSource.Tables["VKH_THONG_TIN_CU_TRU"].Rows.Add(dr);
                    }

                    #endregion
                    #endregion

                    #region Số điện thoại email
                    obj.SO_DTHOAI = txtSoCoDinh.Text;
                    obj.SO_DDONG = txtSoDiDong.Text;
                    obj.EMAIL = txtEmail.Text;
                    if (cmbEmail.SelectedIndex >= 0)
                    {
                        AutoCompleteEntry auEmail = lstSourceLoaiEmail.ElementAt(cmbEmail.SelectedIndex);
                        if (!auEmail.IsNullOrEmpty())
                            obj.EMAIL_TYPE = auEmail.KeywordStrings.FirstOrDefault();
                    }
                    
                    #endregion

                    #region Công việc hiện tại
                    if (!dsSource.Tables["VKH_CONG_VIEC_HIEN_TAI"].IsNullOrEmpty())
                    {
                        DataRow dr = dsSource.Tables["VKH_CONG_VIEC_HIEN_TAI"].NewRow();
                        dr["MA_GOI_NHO"] = txtMaGoiNho.Text;
                        dr["ID_CONG_TY"] = Convert.ToInt32(txtMaGoiNho.Tag);
                        if (chkLuuThongTin.IsChecked.GetValueOrDefault())
                            dr["LUU_THONG_TIN"] = BusinessConstant.CoKhong.CO.layGiaTri();
                        else
                            dr["LUU_THONG_TIN"] = BusinessConstant.CoKhong.KHONG.layGiaTri();
                        dr["TEN_CONG_TY"] = txtTenCongTy.Text;
                        dr["TEN_CHI_NHANH"] = txtChiNhanh.Text;
                        dr["DIA_CHI"] = txtDiaChiCongTy.Text;
                        dr["SO_DIEN_THOAI"] = txtSoDienThoai.Text;
                        dr["MAY_LE"] = txtNhanhSo.Text;
                        string tgianlhe = "";
                        if (!telLienHeSangTu.Value.IsNullOrEmpty())
                            tgianlhe = telLienHeSangTu.Value.GetValueOrDefault().ToString("yyyyMMddHHmm");
                        tgianlhe += "#";
                        if (!telLienHeSangDen.Value.IsNullOrEmpty())
                            tgianlhe += telLienHeSangDen.Value.GetValueOrDefault().ToString("yyyyMMddHHmm");
                        dr["TGIAN_LHE_SANG"] = tgianlhe;
                        tgianlhe = "";
                        if (!telLienHeChieuTu.Value.IsNullOrEmpty())
                            tgianlhe = telLienHeChieuTu.Value.GetValueOrDefault().ToString("yyyyMMddHHmm");
                        tgianlhe += "#";
                        if (!telLienHeChieuDen.Value.IsNullOrEmpty())
                            tgianlhe += telLienHeChieuDen.Value.GetValueOrDefault().ToString("yyyyMMddHHmm");
                        dr["TGIAN_LHE_CHIEU"] = tgianlhe;
                        dr["PHONG_BAN"] = txtPhongBan.Text;
                        dr["VI_TRI_HIEN_TAI"] = txtViTriHienTai.Text;
                        tgianlhe = "";
                        if (!txtSoNamCongTac.Value.IsNullOrEmpty())
                            tgianlhe += txtSoNamCongTac.Value.GetValueOrDefault().ToString();
                        tgianlhe += "#";
                        if (!txtSoThangCongTac.Value.IsNullOrEmpty())
                            tgianlhe += txtSoThangCongTac.Value.GetValueOrDefault().ToString();
                        dr["TGIAN_CONGTAC"] = tgianlhe;
                        if (cmbLoaiHinhCongTy.SelectedIndex >= 0)
                        {
                            AutoCompleteEntry au = lstSourceLoaiHinhCongTy.ElementAt(cmbLoaiHinhCongTy.SelectedIndex);
                            if (!au.IsNullOrEmpty())
                                dr["LOAI_HINH_CONG_TY"] = au.KeywordStrings.FirstOrDefault();
                        }
                        if (cmbNganhNghe.SelectedIndex >= 0)
                        {
                            AutoCompleteEntry au = lstSourceNganhNgheKD.ElementAt(cmbNganhNghe.SelectedIndex);
                            if (!au.IsNullOrEmpty())
                                dr["NNGHE_KDOANH"] = au.KeywordStrings.FirstOrDefault();
                        }
                        dr["LUONG_CO_BAN"] = txtLuongCoBan.Value.GetValueOrDefault().ToString();
                        dr["THU_NHAP_KHAC"] = txtThuNhapKhac.Value.GetValueOrDefault().ToString();
                        if (!teldtNgayNhanLuong.Value.IsNullOrEmpty())
                            dr["NGAY_NHAN_LUONG"] = teldtNgayNhanLuong.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                        dsSource.Tables["VKH_CONG_VIEC_HIEN_TAI"].Rows.Clear();
                        dsSource.Tables["VKH_CONG_VIEC_HIEN_TAI"].Rows.Add(dr);
                    }

                    #endregion

                    #region Công việc người tham chiếu
                    if (!dsSource.Tables["VKH_CONG_VIEC_NGUOI_THAM_CHIEU"].IsNullOrEmpty())
                    {
                        DataRow dr = dsSource.Tables["VKH_CONG_VIEC_NGUOI_THAM_CHIEU"].NewRow();
                        dr["HO_TEN"] = txtHoTenThamChieu.Text;
                        if (cmbQuanHeThamChieu.SelectedIndex >= 0)
                        {
                            AutoCompleteEntry au = lstSourceMoiQuanHeTChieu.ElementAt(cmbQuanHeThamChieu.SelectedIndex);
                            if (!au.IsNullOrEmpty())
                                dr["MOI_QUAN_HE"] = au.KeywordStrings.FirstOrDefault();
                        }
                        
                        dr["MO_TA_MOI_QHE"] = txtMoTaQuanHe.Text;
                        dr["SO_DI_DONG"] = txtSoDiDongTChieu.Text;
                        dr["SO_DIEN_THOAI"] = txtSoDienThoaiTChieu.Text;
                        dr["DIA_CHI"] = txtDiaChiHienTaiTChieu.Text;
                        dsSource.Tables["VKH_CONG_VIEC_NGUOI_THAM_CHIEU"].Rows.Clear();
                        dsSource.Tables["VKH_CONG_VIEC_NGUOI_THAM_CHIEU"].Rows.Add(dr);
                    }

                    #endregion

                    #region Thông tin kiểm soát
                    obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    obj.TTHAI_NVU = sTrangThaiNVu;
                    obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                    obj.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    obj.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    if (action != DatabaseConstant.Action.THEM)
                    {
                        obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                        obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                        if (bHetHLuc && !chkKhachHangHetHL.IsChecked.Value)
                            obj.NGAY_CONG_NHAN = ClientInformation.NgayLamViecHienTai;
                        else
                            obj.NGAY_CONG_NHAN = ngayCongNhan;
                    }
                    #endregion                    
                }
                #endregion

                #region Khách hàng tổ chức
                else if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.DNGHIEP.layGiaTri()))
                {
                    #region Thông tin chung
                    obj.ID = id;
                    obj.ID_DON_VI = ClientInformation.IdDonViGiaoDich;
                    obj.MA_KHANG_LOAI = loaiKhachHang;
                    obj.MA_KHANG = txtMaKhachHang.Text;
                    obj.TEN_KHANG = txtTenKhachHang.Text;
                    obj.TEN_GDICH = obj.TEN_KHANG;
                    obj.TEN_BAN_DIA = txtTenBanDia.Text;
                    if (raddtNgayCongNhan.Value != null)
                        obj.NGAY_THAM_GIA = Convert.ToDateTime(raddtNgayCongNhan.Value).ToString("yyyyMMdd");
                    if (raddtNgayThanhLapTC.Value != null)
                        obj.NGAY_THANH_LAP = Convert.ToDateTime(raddtNgayThanhLapTC.Value).ToString("yyyyMMdd");
                    if (raddtNgayHetHL.Value != null)
                        obj.NGAY_HET_HLUC = Convert.ToDateTime(raddtNgayHetHL.Value).ToString("yyyyMMdd");                                                            
                    obj.DD_GTLQ_LOAI = BusinessConstant.LOAI_GIAY_TO.GP_DKKD.layGiaTri();
                    obj.DD_GTLQ_SO = txtSoCMND.Text;
                    if (raddtNgayCap.Value != null)
                        obj.DD_GTLQ_NGAY_CAP = Convert.ToDateTime(raddtNgayCap.Value).ToString("yyyyMMdd");
                    obj.DD_GTLQ_NOI_CAP = txtNoiCap.Text;                    
                    obj.DD_MA_TTRANG_HNHAN = null;
                    if(cmbNganhKinhTe.SelectedIndex >= 0)
                        obj.MA_NGANH_KT = lstSourceNganhKT.ElementAt(cmbNganhKinhTe.SelectedIndex).KeywordStrings.ElementAt(0);
                    obj.MA_NGHE_NGHIEP = null;
                    obj.NGAY_CONG_NHAN = obj.NGAY_THAM_GIA;
                    obj.TEN_BO = txtTenBo.Text;
                    #endregion

                    #region Tài sản doanh nghiệp
                    obj.TONG_TAI_SAN = (decimal?)numGiaTriTaiSan.Value;
                    obj.VON_DIEU_LE = (decimal?)numVonDieuLe.Value;
                    #endregion

                    #region Địa chỉ hiện tại
                    if (radioNongThonHienTai.IsChecked == true)
                        obj.MA_TTHI_NTHON_HTAI = "NONG_THON";
                    else if (radioThanhThiHienTai.IsChecked == true)
                        obj.MA_TTHI_NTHON_HTAI = "THANH_THI";

                    obj.DIA_CHI = txtDiaChiHienTai.Text.Trim();
                    if (cmbVungMienHienTai.SelectedIndex >= 0)
                        obj.MA_VUNG = lstSourceVungMien.ElementAt(cmbVungMienHienTai.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbTinhTPHienTai.SelectedIndex >= 0)
                        obj.MA_TINHTP = lstSourceTinhTp.ElementAt(cmbTinhTPHienTai.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbQuanHuyenHienTai.SelectedIndex >= 0)
                        obj.MA_QUAN = lstSourceQuanHuyen.ElementAt(cmbQuanHuyenHienTai.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbXaPhuongHienTai.SelectedIndex >= 0)
                        obj.MA_PHUONG = lstSourceXaPhuong.ElementAt(cmbXaPhuongHienTai.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbLangTodp.SelectedIndex >= 0)
                        obj.MA_TODP = lstSourceLangTodp.ElementAt(cmbLangTodp.SelectedIndex).KeywordStrings.ElementAt(0);
                    #endregion

                    #region Số điện thoại email
                    obj.SO_DTHOAI = txtSoCoDinh.Text;
                    obj.SO_DDONG = txtSoDiDong.Text;
                    obj.EMAIL = txtEmail.Text;
                    if (cmbEmail.SelectedIndex > 0)
                    {
                        AutoCompleteEntry au = lstSourceLoaiEmail.ElementAt(cmbEmail.SelectedIndex);
                        if (!au.IsNullOrEmpty())
                            obj.EMAIL_TYPE = au.KeywordStrings.FirstOrDefault();
                    }
                    #endregion

                    #region Thông tin kiểm soát
                    obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    obj.TTHAI_NVU = sTrangThaiNVu;
                    obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                    obj.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    obj.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    if (action != DatabaseConstant.Action.THEM)
                    {
                        obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                        obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                        if (bHetHLuc && !chkKhachHangHetHL.IsChecked.Value)
                            obj.NGAY_CONG_NHAN = ClientInformation.NgayLamViecHienTai;
                        else
                            obj.NGAY_CONG_NHAN = ngayCongNhan;
                    }
                    #endregion
                }
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
            KhachHangProcess processKhachHang = new KhachHangProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            obj = new KH_KHANG_HSO();
            try
            {
                string loai = "";
                string giaTri = "";                
                bool ret = false;
                obj.ID = id;
                ret = processKhachHang.KhachHang04(DatabaseConstant.Action.LOAD, ref obj, ref dsSource, ref lstChuKyHinhAnh, ref listClientResponseDetail);
                if (ret == true)
                {
                    loaiKhachHang = obj.MA_KHANG_LOAI;
                    cmbLoaiKhachHang.SelectedIndex = lstSourceLoaiKH.IndexOf(lstSourceLoaiKH.FirstOrDefault(i => i.KeywordStrings.First().Equals(loaiKhachHang)));
                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    #region Khách hàng thành viên
                    if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.TVIEN.layGiaTri()))
                    {
                        CommonFunction.SetWindowTitle(this, DatabaseConstant.Function.KH_THANH_VIEN);

                        #region Thông tin chung
                        idKhuVuc = (int)obj.ID_KHU_VUC;
                        idCum = (int)obj.ID_CUM;
                        idNhom = (int)obj.ID_NHOM;
                        txtMaKhachHang.Text = obj.MA_KHANG;
                        txtTenKhachHang.Text = obj.TEN_KHANG;
                        txtTenBanDia.Text = obj.TEN_BAN_DIA;
                        if(LDateTime.IsDate(obj.NGAY_THAM_GIA,"yyyyMMdd"))
                            raddtNgayCongNhan.Value = LDateTime.StringToDate(obj.NGAY_THAM_GIA,"yyyyMMdd");                        
                        if (LDateTime.IsDate(obj.NGAY_HET_HLUC, "yyyyMMdd"))
                            raddtNgayHetHL.Value = LDateTime.StringToDate(obj.NGAY_HET_HLUC, "yyyyMMdd");
                        if (!obj.NGAY_HET_HLUC.IsNullOrEmptyOrSpace())
                        {
                            chkKhachHangHetHL.IsChecked = true;
                            //cmbLyDoRaKhoiNhom.SelectedIndex = lstSourceLyDoRa.IndexOf(lstSourceLyDoRa.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_LY_DO)));
                        }
                        raddtNgayThanhLapTC.Value = null;
                        if (LDateTime.IsDate(obj.DD_NGAY_SINH, "yyyyMMdd"))
                            raddtNgaySinh.Value = LDateTime.StringToDate(obj.DD_NGAY_SINH, "yyyyMMdd");
                        cmbGioiTinh.SelectedIndex = lstSourceGioiTinh.IndexOf(lstSourceGioiTinh.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.DD_GIOI_TINH)));
                        txtSoCMND.Text = obj.DD_GTLQ_SO;
                        if (LDateTime.IsDate(obj.DD_GTLQ_NGAY_CAP, "yyyyMMdd"))
                            raddtNgayCap.Value = LDateTime.StringToDate(obj.DD_GTLQ_NGAY_CAP, "yyyyMMdd");
                        txtNoiCap.Text = obj.DD_GTLQ_NOI_CAP;
                        cmbDanToc.SelectedIndex = lstSourceDanToc.IndexOf(lstSourceDanToc.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.DD_MA_DAN_TOC)));
                        txtSoHoKhau.Text = obj.SO_HO_KHAU;
                        cmbTinhTrangHonNhan.SelectedIndex = lstSourceHonNhan.IndexOf(lstSourceHonNhan.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.DD_MA_TTRANG_HNHAN)));
                        cmbLoaiHinhToChuc.SelectedIndex = -1;
                        cmbNganhKinhTe.SelectedIndex = -1;
                        cmbNgheNghiep.SelectedIndex = lstSourceNgheNghiep.IndexOf(lstSourceNgheNghiep.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_NGHE_NGHIEP)));
                        ngayCongNhan = obj.NGAY_CONG_NHAN;
                        txtTenBo.Text = obj.TEN_BO;
                        #endregion

                        #region Thông tin tài sản của doanh nghiệp
                        numGiaTriTaiSan.Value = null;
                        numVonDieuLe.Value = null;
                        #endregion

                        #region Hộ khẩu thường chú
                        
                        if (obj.MA_TTHI_NTHON.Equals("THANH_THI"))
                            radioThanhThiThuongChu.IsChecked = true;                        
                        else
                            radioNongThonThuongChu.IsChecked = true;

                        cmbVungMien.SelectedIndex = lstSourceHKVungMien.IndexOf(lstSourceHKVungMien.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.DD_TTRU_MA_VUNG)));
                        cmbTinhTP.SelectedIndex = lstSourceHKTinhTP.IndexOf(lstSourceHKTinhTP.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.DD_TTRU_MA_TINHTP)));
                        cmbQuanHuyen.SelectedIndex = lstSourceHKQuanHuyen.IndexOf(lstSourceHKQuanHuyen.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.DD_TTRU_MA_QUAN)));
                        cmbXaPhuong.SelectedIndex = lstSourceHKXaPhuong.IndexOf(lstSourceHKXaPhuong.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.DD_TTRU_MA_PHUONG)));
                        cmbLangTodp.SelectedIndex = lstSourceHKLangTodp.IndexOf(lstSourceHKLangTodp.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.DD_TTRU_MA_TODP)));
                        txtDiaChi.Text = obj.DD_TTRU_DIA_CHI;
                        #endregion

                        #region Địa chỉ hiện tại
                        if (obj.MA_TTHI_NTHON_HTAI.Equals("THANH_THI"))
                            radioThanhThiHienTai.IsChecked = true;
                        else
                            radioNongThonHienTai.IsChecked = true;

                        cmbVungMienHienTai.SelectedIndex = lstSourceVungMien.IndexOf(lstSourceVungMien.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_VUNG)));
                        cmbTinhTPHienTai.SelectedIndex = lstSourceTinhTp.IndexOf(lstSourceTinhTp.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_TINHTP)));
                        cmbQuanHuyenHienTai.SelectedIndex = lstSourceQuanHuyen.IndexOf(lstSourceQuanHuyen.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_QUAN)));
                        cmbXaPhuongHienTai.SelectedIndex = lstSourceXaPhuong.IndexOf(lstSourceXaPhuong.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_PHUONG)));
                        cmbLangTodpHienTai.SelectedIndex = lstSourceLangTodp.IndexOf(lstSourceLangTodp.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_TODP)));
                        txtDiaChiHienTai.Text = obj.DIA_CHI;

                        #region Thông tin cư trú
                        if (!dsSource.Tables["VKH_THONG_TIN_CU_TRU"].IsNullOrEmpty())
                        {
                            foreach (DataRow dr in dsSource.Tables["VKH_THONG_TIN_CU_TRU"].Rows)
                            {
                                if(dr["LOAI_HINH_CTRU"] != DBNull.Value && !dr["LOAI_HINH_CTRU"].IsNullOrEmpty())
                                {
                                    cmbLoaiHinhCuTru.SelectedIndex = lstSourceLoaiHinhCuTru.IndexOf(lstSourceLoaiHinhCuTru.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["LOAI_HINH_CTRU"].ToString())));
                                }
                                txtLoaiHinhCuTru.Text = dr["MO_TA_LHINH_CTRU"].ToString();
                                if (dr["THANH_PHAN"] != DBNull.Value && !dr["THANH_PHAN"].IsNullOrEmpty())
                                {
                                    string[] quanHeCuTru = dr["THANH_PHAN"].ToString().Split('#');
                                    cmbOVoiAi.SelectedIndex = lstSourceQuanHeCuTru.IndexOf(lstSourceQuanHeCuTru.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(quanHeCuTru[0])));
                                    if (quanHeCuTru.Count() > 1)
                                        txtOVoiAi.Text = quanHeCuTru[1];
                                }
                                if (dr["TGIAN_O_NAM"] != DBNull.Value && !dr["TGIAN_O_NAM"].IsNullOrEmpty() && dr["TGIAN_O_NAM"].ToString().IsNumeric())
                                {
                                    txtSoNamCuTru.Value = Convert.ToDouble(dr["TGIAN_O_NAM"]);
                                }
                                if (dr["TGIAN_O_THANG"] != DBNull.Value && !dr["TGIAN_O_THANG"].IsNullOrEmpty() && dr["TGIAN_O_THANG"].ToString().IsNumeric())
                                {
                                    txtSoThangCuTru.Value = Convert.ToDouble(dr["TGIAN_O_THANG"]);
                                }
                            }
                        }

                        #endregion

                        #endregion

                        #region Số điện thoại email
                        txtSoCoDinh.Text = obj.SO_DTHOAI;
                        txtSoDiDong.Text = obj.SO_DDONG;
                        txtEmail.Text = obj.EMAIL;
                        if (!obj.EMAIL_TYPE.IsNullOrEmptyOrSpace())
                            cmbEmail.SelectedIndex = lstSourceLoaiEmail.IndexOf(lstSourceLoaiEmail.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(obj.EMAIL_TYPE)));
                        #endregion

                        #region Công việc hiện tại
                        if (!dsSource.Tables["VKH_CONG_VIEC_HIEN_TAI"].IsNullOrEmpty() && dsSource.Tables["VKH_CONG_VIEC_HIEN_TAI"].Rows.Count > 0)
                        {
                            DataRow dr = dsSource.Tables["VKH_CONG_VIEC_HIEN_TAI"].Rows[0];
                            txtMaGoiNho.Text = dr["MA_GOI_NHO"].ToString();
                            txtMaGoiNho.Tag = Convert.ToInt32(dr["ID_CONG_TY"]);
                            if (dr["LUU_THONG_TIN"].ToString().Equals(BusinessConstant.CoKhong.CO.layGiaTri()))
                                chkLuuThongTin.IsChecked = true;
                            txtTenCongTy.Text = dr["TEN_CONG_TY"].ToString();
                            txtChiNhanh.Text = dr["TEN_CHI_NHANH"].ToString();
                            txtDiaChiCongTy.Text = dr["DIA_CHI"].ToString();
                            txtSoDienThoai.Text = dr["SO_DIEN_THOAI"].ToString();
                            txtNhanhSo.Text = dr["MAY_LE"].ToString();
                            string tgianlhe = dr["TGIAN_LHE_SANG"].ToString();
                            string[] arrtgianlHe = tgianlhe.Split('#');
                            if(arrtgianlHe.Count()>1)
                            {
                                if (arrtgianlHe[0].IsDate("yyyyMMddHHmm"))
                                    telLienHeSangTu.Value = arrtgianlHe[0].StringToDate("yyyyMMddHHmm");
                                if (arrtgianlHe[1].IsDate("yyyyMMddHHmm"))
                                    telLienHeSangDen.Value = arrtgianlHe[1].StringToDate("yyyyMMddHHmm");
                            }
                            tgianlhe = dr["TGIAN_LHE_CHIEU"].ToString();
                            arrtgianlHe = tgianlhe.Split('#');
                            if (arrtgianlHe.Count() > 1)
                            {
                                if (arrtgianlHe[0].IsDate("yyyyMMddHHmm"))
                                    telLienHeChieuTu.Value = arrtgianlHe[0].StringToDate("yyyyMMddHHmm");
                                if (arrtgianlHe[1].IsDate("yyyyMMddHHmm"))
                                    telLienHeChieuDen.Value = arrtgianlHe[1].StringToDate("yyyyMMddHHmm");
                            }

                            txtPhongBan.Text = dr["PHONG_BAN"].ToString();
                            txtViTriHienTai.Text = dr["VI_TRI_HIEN_TAI"].ToString();
                            tgianlhe = dr["TGIAN_CONGTAC"].ToString();
                            arrtgianlHe = tgianlhe.Split('#');
                            if (arrtgianlHe.Count() > 1)
                            {
                                if (arrtgianlHe[0].IsNumeric())
                                    txtSoNamCongTac.Value = arrtgianlHe[0].StringToInt32();
                                if (arrtgianlHe[1].IsNumeric())
                                    txtSoThangCongTac.Value = arrtgianlHe[1].StringToInt32();
                            }
                            if (!txtSoNamCongTac.Value.IsNullOrEmpty())
                                tgianlhe += txtSoNamCongTac.Value.GetValueOrDefault().ToString();
                            if(!dr["LOAI_HINH_CONG_TY"].IsNullOrEmpty())
                            {
                                cmbLoaiHinhCongTy.SelectedIndex = lstSourceLoaiHinhCongTy.IndexOf(lstSourceLoaiHinhCongTy.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["LOAI_HINH_CONG_TY"].ToString())));
                            }
                            if (!dr["NNGHE_KDOANH"].IsNullOrEmpty())
                            {
                                cmbNganhNghe.SelectedIndex = lstSourceNganhNgheKD.IndexOf(lstSourceNganhNgheKD.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["NNGHE_KDOANH"].ToString())));
                            }
                            txtLuongCoBan.Value = Convert.ToDouble(dr["LUONG_CO_BAN"]);
                            txtThuNhapKhac.Value = Convert.ToDouble(dr["THU_NHAP_KHAC"]);
                            txtTongThuNhap.Value = txtLuongCoBan.Value + txtThuNhapKhac.Value;
                            if (dr["NGAY_NHAN_LUONG"].ToString().IsDate(ApplicationConstant.defaultDateTimeFormat))
                                teldtNgayNhanLuong.Value = dr["NGAY_NHAN_LUONG"].ToString().StringToDate(ApplicationConstant.defaultDateTimeFormat);
                            
                        }

                        #endregion

                        #region Công việc người tham chiếu
                        if (!dsSource.Tables["VKH_CONG_VIEC_NGUOI_THAM_CHIEU"].IsNullOrEmpty() && dsSource.Tables["VKH_CONG_VIEC_NGUOI_THAM_CHIEU"].Rows.Count > 0)
                        {
                            DataRow dr = dsSource.Tables["VKH_CONG_VIEC_NGUOI_THAM_CHIEU"].Rows[0];
                            txtHoTenThamChieu.Text = dr["HO_TEN"].ToString();
                            if (dr["MOI_QUAN_HE"] != DBNull.Value)
                                cmbQuanHeThamChieu.SelectedIndex = lstSourceMoiQuanHeTChieu.IndexOf(lstSourceMoiQuanHeTChieu.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["MOI_QUAN_HE"].ToString())));
                            txtMoTaQuanHe.Text = dr["MO_TA_MOI_QHE"].ToString();
                            txtSoDiDongTChieu.Text = dr["SO_DI_DONG"].ToString();
                            txtSoDienThoaiTChieu.Text = dr["SO_DIEN_THOAI"].ToString();
                            txtDiaChiHienTaiTChieu.Text = dr["DIA_CHI"].ToString();
                        }

                        #endregion

                        #region Grid
                        // Cap nhat view nay, lay ngon ngu

                        if (dsSource.Tables["VKH_GDINH_NGUOI_TKE"] != null)
                        {
                            if (dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows.Count; i++)
                                {
                                    if (dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["TEN_VAI_TRO"] != null)
                                        dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["TEN_VAI_TRO"] = LLanguage.SearchResourceByKey(dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["TEN_VAI_TRO"].ToString());

                                    if (dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["GIOI_TINH1"] != null)
                                        dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["GIOI_TINH1"] = LLanguage.SearchResourceByKey(dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["GIOI_TINH1"].ToString());

                                    if (dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["QH_VOI_TVIEN1"] != null)
                                        dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["QH_VOI_TVIEN1"] = LLanguage.SearchResourceByKey(dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["QH_VOI_TVIEN1"].ToString());

                                    if (dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["TDO_HVAN1"] != null)
                                        dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["TDO_HVAN1"] = LLanguage.SearchResourceByKey(dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["TDO_HVAN1"].ToString());

                                    if (dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["NGHE_NGHIEP1"] != null)
                                        dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["NGHE_NGHIEP1"] = LLanguage.SearchResourceByKey(dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["NGHE_NGHIEP1"].ToString());

                                    if (dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["QUAN_HE_KT1"] != null)
                                        dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["QUAN_HE_KT1"] = LLanguage.SearchResourceByKey(dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["QUAN_HE_KT1"].ToString());
                                }
                            }
                            grNguoiThuaKe.DataContext = dsSource.Tables["VKH_GDINH_NGUOI_TKE"].DefaultView;
                        }

                        grNguoiThuaKe.DataContext = dsSource.Tables["VKH_GDINH_NGUOI_TKE"].DefaultView;
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
                    #endregion

                    #region Khách hàng cá nhân
                    else if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.CNHAN.layGiaTri()))
                    {
                        CommonFunction.SetWindowTitle(this, DatabaseConstant.Function.KH_CA_NHAN);

                        #region Thông tin chung
                        idKhuVuc = (int)obj.ID_KHU_VUC;
                        idCum = (int)obj.ID_CUM;
                        idNhom = (int)obj.ID_NHOM;
                        txtMaKhachHang.Text = obj.MA_KHANG;
                        txtTenKhachHang.Text = obj.TEN_KHANG;
                        txtTenBanDia.Text = obj.TEN_BAN_DIA;
                        if (LDateTime.IsDate(obj.NGAY_THAM_GIA, "yyyyMMdd"))
                            raddtNgayCongNhan.Value = LDateTime.StringToDate(obj.NGAY_THAM_GIA, "yyyyMMdd");
                        if (LDateTime.IsDate(obj.NGAY_HET_HLUC, "yyyyMMdd"))
                            raddtNgayHetHL.Value = LDateTime.StringToDate(obj.NGAY_HET_HLUC, "yyyyMMdd");
                        if (!obj.NGAY_HET_HLUC.IsNullOrEmptyOrSpace())
                        {
                            chkKhachHangHetHL.IsChecked = true;
                            //cmbLyDoRaKhoiNhom.SelectedIndex = lstSourceLyDoRa.IndexOf(lstSourceLyDoRa.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_LY_DO)));
                        }
                        raddtNgayThanhLapTC.Value = null;
                        if (LDateTime.IsDate(obj.DD_NGAY_SINH, "yyyyMMdd"))
                            raddtNgaySinh.Value = LDateTime.StringToDate(obj.DD_NGAY_SINH, "yyyyMMdd");
                        cmbGioiTinh.SelectedIndex = lstSourceGioiTinh.IndexOf(lstSourceGioiTinh.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.DD_GIOI_TINH)));
                        txtSoCMND.Text = obj.DD_GTLQ_SO;
                        if (LDateTime.IsDate(obj.DD_GTLQ_NGAY_CAP, "yyyyMMdd"))
                            raddtNgayCap.Value = LDateTime.StringToDate(obj.DD_GTLQ_NGAY_CAP, "yyyyMMdd");
                        txtNoiCap.Text = obj.DD_GTLQ_NOI_CAP;
                        cmbDanToc.SelectedIndex = lstSourceDanToc.IndexOf(lstSourceDanToc.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.DD_MA_DAN_TOC)));
                        txtSoHoKhau.Text = obj.SO_HO_KHAU;
                        cmbTinhTrangHonNhan.SelectedIndex = lstSourceHonNhan.IndexOf(lstSourceHonNhan.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.DD_MA_TTRANG_HNHAN)));
                        cmbLoaiHinhToChuc.SelectedIndex = -1;
                        cmbNganhKinhTe.SelectedIndex = -1;
                        cmbNgheNghiep.SelectedIndex = lstSourceNgheNghiep.IndexOf(lstSourceNgheNghiep.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_NGHE_NGHIEP)));
                        ngayCongNhan = obj.NGAY_CONG_NHAN;
                        txtTenBo.Text = obj.TEN_BO;
                        #endregion

                        #region Thông tin tài sản của doanh nghiệp
                        numGiaTriTaiSan.Value = null;
                        numVonDieuLe.Value = null;
                        #endregion

                        #region Hộ khẩu thường chú

                        if (obj.MA_TTHI_NTHON.Equals("THANH_THI"))
                            radioThanhThiThuongChu.IsChecked = true;
                        else
                            radioNongThonThuongChu.IsChecked = true;

                        cmbVungMien.SelectedIndex = lstSourceHKVungMien.IndexOf(lstSourceHKVungMien.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.DD_TTRU_MA_VUNG)));
                        cmbTinhTP.SelectedIndex = lstSourceHKTinhTP.IndexOf(lstSourceHKTinhTP.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.DD_TTRU_MA_TINHTP)));
                        cmbQuanHuyen.SelectedIndex = lstSourceHKQuanHuyen.IndexOf(lstSourceHKQuanHuyen.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.DD_TTRU_MA_QUAN)));
                        cmbXaPhuong.SelectedIndex = lstSourceHKXaPhuong.IndexOf(lstSourceHKXaPhuong.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.DD_TTRU_MA_PHUONG)));
                        cmbLangTodp.SelectedIndex = lstSourceHKLangTodp.IndexOf(lstSourceHKLangTodp.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.DD_TTRU_MA_TODP)));
                        txtDiaChi.Text = obj.DD_TTRU_DIA_CHI;
                        #endregion

                        #region Địa chỉ hiện tại
                        if (obj.MA_TTHI_NTHON_HTAI.Equals("THANH_THI"))
                            radioThanhThiHienTai.IsChecked = true;
                        else
                            radioNongThonHienTai.IsChecked = true;

                        cmbVungMienHienTai.SelectedIndex = lstSourceVungMien.IndexOf(lstSourceVungMien.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_VUNG)));
                        cmbTinhTPHienTai.SelectedIndex = lstSourceTinhTp.IndexOf(lstSourceTinhTp.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_TINHTP)));
                        cmbQuanHuyenHienTai.SelectedIndex = lstSourceQuanHuyen.IndexOf(lstSourceQuanHuyen.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_QUAN)));
                        cmbXaPhuongHienTai.SelectedIndex = lstSourceXaPhuong.IndexOf(lstSourceXaPhuong.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_PHUONG)));
                        cmbLangTodpHienTai.SelectedIndex = lstSourceLangTodp.IndexOf(lstSourceLangTodp.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_TODP)));
                        txtDiaChiHienTai.Text = obj.DIA_CHI;
                        
                        #region Thông tin cư trú
                        if (!dsSource.Tables["VKH_THONG_TIN_CU_TRU"].IsNullOrEmpty())
                        {
                            foreach (DataRow dr in dsSource.Tables["VKH_THONG_TIN_CU_TRU"].Rows)
                            {
                                if (dr["LOAI_HINH_CTRU"] != DBNull.Value && !dr["LOAI_HINH_CTRU"].IsNullOrEmpty())
                                {
                                    cmbLoaiHinhCuTru.SelectedIndex = lstSourceLoaiHinhCuTru.IndexOf(lstSourceLoaiHinhCuTru.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["LOAI_HINH_CTRU"].ToString())));
                                }
                                txtLoaiHinhCuTru.Text = dr["MO_TA_LHINH_CTRU"].ToString();
                                if (dr["THANH_PHAN"] != DBNull.Value && !dr["THANH_PHAN"].IsNullOrEmpty())
                                {
                                    string[] quanHeCuTru = dr["THANH_PHAN"].ToString().Split('#');
                                    cmbOVoiAi.SelectedIndex = lstSourceQuanHeCuTru.IndexOf(lstSourceQuanHeCuTru.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(quanHeCuTru[0])));
                                    if (quanHeCuTru.Count() > 1)
                                        txtOVoiAi.Text = quanHeCuTru[1];
                                }
                                if (dr["TGIAN_O_NAM"] != DBNull.Value && !dr["TGIAN_O_NAM"].IsNullOrEmpty() && dr["TGIAN_O_NAM"].ToString().IsNumeric())
                                {
                                    txtSoNamCuTru.Value = Convert.ToDouble(dr["TGIAN_O_NAM"]);
                                }
                                if (dr["TGIAN_O_THANG"] != DBNull.Value && !dr["TGIAN_O_THANG"].IsNullOrEmpty() && dr["TGIAN_O_THANG"].ToString().IsNumeric())
                                {
                                    txtSoThangCuTru.Value = Convert.ToDouble(dr["TGIAN_O_THANG"]);
                                }
                            }
                        }
                        #endregion
                        #endregion

                        #region Số điện thoại email
                        txtSoCoDinh.Text = obj.SO_DTHOAI;
                        txtSoDiDong.Text = obj.SO_DDONG;
                        txtEmail.Text = obj.EMAIL;
                        if (!obj.EMAIL_TYPE.IsNullOrEmptyOrSpace())
                            cmbEmail.SelectedIndex = lstSourceLoaiEmail.IndexOf(lstSourceLoaiEmail.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(obj.EMAIL_TYPE)));
                        #endregion

                        #region Công việc hiện tại
                        if (!dsSource.Tables["VKH_CONG_VIEC_HIEN_TAI"].IsNullOrEmpty() && dsSource.Tables["VKH_CONG_VIEC_HIEN_TAI"].Rows.Count > 0)
                        {
                            DataRow dr = dsSource.Tables["VKH_CONG_VIEC_HIEN_TAI"].Rows[0];
                            txtMaGoiNho.Text = dr["MA_GOI_NHO"].ToString();
                            txtMaGoiNho.Tag = Convert.ToInt32(dr["ID_CONG_TY"]);
                            if (dr["LUU_THONG_TIN"].ToString().Equals(BusinessConstant.CoKhong.CO.layGiaTri()))
                                chkLuuThongTin.IsChecked = true;
                            txtTenCongTy.Text = dr["TEN_CONG_TY"].ToString();
                            txtChiNhanh.Text = dr["TEN_CHI_NHANH"].ToString();
                            txtDiaChiCongTy.Text = dr["DIA_CHI"].ToString();
                            txtSoDienThoai.Text = dr["SO_DIEN_THOAI"].ToString();
                            txtNhanhSo.Text = dr["MAY_LE"].ToString();
                            string tgianlhe = dr["TGIAN_LHE_SANG"].ToString();
                            string[] arrtgianlHe = tgianlhe.Split('#');
                            if (arrtgianlHe.Count() > 1)
                            {
                                if (arrtgianlHe[0].IsDate("yyyyMMddHHmm"))
                                    telLienHeSangTu.Value = arrtgianlHe[0].StringToDate("yyyyMMddHHmm");
                                if (arrtgianlHe[1].IsDate("yyyyMMddHHmm"))
                                    telLienHeSangDen.Value = arrtgianlHe[1].StringToDate("yyyyMMddHHmm");
                            }
                            tgianlhe = dr["TGIAN_LHE_CHIEU"].ToString();
                            arrtgianlHe = tgianlhe.Split('#');
                            if (arrtgianlHe.Count() > 1)
                            {
                                if (arrtgianlHe[0].IsDate("yyyyMMddHHmm"))
                                    telLienHeChieuTu.Value = arrtgianlHe[0].StringToDate("yyyyMMddHHmm");
                                if (arrtgianlHe[1].IsDate("yyyyMMddHHmm"))
                                    telLienHeChieuDen.Value = arrtgianlHe[1].StringToDate("yyyyMMddHHmm");
                            }

                            txtPhongBan.Text = dr["PHONG_BAN"].ToString();
                            txtViTriHienTai.Text = dr["VI_TRI_HIEN_TAI"].ToString();
                            tgianlhe = dr["TGIAN_CONGTAC"].ToString();
                            arrtgianlHe = tgianlhe.Split('#');
                            if (arrtgianlHe.Count() > 1)
                            {
                                if (arrtgianlHe[0].IsNumeric())
                                    txtSoNamCongTac.Value = arrtgianlHe[0].StringToInt32();
                                if (arrtgianlHe[1].IsNumeric())
                                    txtSoThangCongTac.Value = arrtgianlHe[1].StringToInt32();
                            }
                            if (!txtSoNamCongTac.Value.IsNullOrEmpty())
                                tgianlhe += txtSoNamCongTac.Value.GetValueOrDefault().ToString();
                            if (!dr["LOAI_HINH_CONG_TY"].IsNullOrEmpty())
                            {
                                cmbLoaiHinhCongTy.SelectedIndex = lstSourceLoaiHinhCongTy.IndexOf(lstSourceLoaiHinhCongTy.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["LOAI_HINH_CONG_TY"].ToString())));
                            }
                            if (!dr["NNGHE_KDOANH"].IsNullOrEmpty())
                            {
                                cmbNganhNghe.SelectedIndex = lstSourceNganhNgheKD.IndexOf(lstSourceNganhNgheKD.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["NNGHE_KDOANH"].ToString())));
                            }
                            txtLuongCoBan.Value = Convert.ToDouble(dr["LUONG_CO_BAN"]);
                            txtThuNhapKhac.Value = Convert.ToDouble(dr["THU_NHAP_KHAC"]);
                            txtTongThuNhap.Value = txtLuongCoBan.Value + txtThuNhapKhac.Value;
                            if (dr["NGAY_NHAN_LUONG"].ToString().IsDate(ApplicationConstant.defaultDateTimeFormat))
                                teldtNgayNhanLuong.Value = dr["NGAY_NHAN_LUONG"].ToString().StringToDate(ApplicationConstant.defaultDateTimeFormat);

                        }

                        #endregion

                        #region Công việc người tham chiếu
                        if (!dsSource.Tables["VKH_CONG_VIEC_NGUOI_THAM_CHIEU"].IsNullOrEmpty() && dsSource.Tables["VKH_CONG_VIEC_NGUOI_THAM_CHIEU"].Rows.Count > 0)
                        {
                            DataRow dr = dsSource.Tables["VKH_CONG_VIEC_NGUOI_THAM_CHIEU"].Rows[0];
                            txtHoTenThamChieu.Text = dr["HO_TEN"].ToString();
                            if (dr["MOI_QUAN_HE"] != DBNull.Value)
                                cmbQuanHeThamChieu.SelectedIndex = lstSourceMoiQuanHeTChieu.IndexOf(lstSourceMoiQuanHeTChieu.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["MOI_QUAN_HE"].ToString())));
                            txtMoTaQuanHe.Text = dr["MO_TA_MOI_QHE"].ToString();
                            txtSoDiDongTChieu.Text = dr["SO_DI_DONG"].ToString();
                            txtSoDienThoaiTChieu.Text = dr["SO_DIEN_THOAI"].ToString();
                            txtDiaChiHienTaiTChieu.Text = dr["DIA_CHI"].ToString();
                        }

                        #endregion

                        #region Grid
                        if (dsSource.Tables["VKH_GDINH_NGUOI_TKE"] != null)
                        {
                            if (dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows.Count; i++)
                                {
                                    if (dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["TEN_VAI_TRO"] != null)
                                        dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["TEN_VAI_TRO"] = LLanguage.SearchResourceByKey(dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["TEN_VAI_TRO"].ToString());

                                    if (dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["GIOI_TINH1"] != null)
                                        dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["GIOI_TINH1"] = LLanguage.SearchResourceByKey(dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["GIOI_TINH1"].ToString());

                                    if (dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["QH_VOI_TVIEN1"] != null)
                                        dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["QH_VOI_TVIEN1"] = LLanguage.SearchResourceByKey(dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["QH_VOI_TVIEN1"].ToString());

                                    if (dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["TDO_HVAN1"] != null)
                                        dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["TDO_HVAN1"] = LLanguage.SearchResourceByKey(dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["TDO_HVAN1"].ToString());

                                    if (dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["NGHE_NGHIEP1"] != null)
                                        dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["NGHE_NGHIEP1"] = LLanguage.SearchResourceByKey(dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["NGHE_NGHIEP1"].ToString());

                                    if (dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["QUAN_HE_KT1"] != null)
                                        dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["QUAN_HE_KT1"] = LLanguage.SearchResourceByKey(dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i]["QUAN_HE_KT1"].ToString());
                                }
                            }
                            grNguoiThuaKe.DataContext = dsSource.Tables["VKH_GDINH_NGUOI_TKE"].DefaultView;
                        }

                        grNguoiThuaKe.DataContext = dsSource.Tables["VKH_GDINH_NGUOI_TKE"].DefaultView;
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
                    #endregion

                    #region Khách hàng tổ chức
                    else if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.DNGHIEP.layGiaTri()))
                    {
                        CommonFunction.SetWindowTitle(this, DatabaseConstant.Function.KH_TO_CHUC);

                        #region Thông tin chung
                        txtMaKhachHang.Text = obj.MA_KHANG;
                        txtTenKhachHang.Text = obj.TEN_KHANG;
                        txtTenBanDia.Text = obj.TEN_BAN_DIA;
                        if (LDateTime.IsDate(obj.NGAY_THAM_GIA, "yyyyMMdd"))
                            raddtNgayCongNhan.Value = LDateTime.StringToDate(obj.NGAY_THAM_GIA, "yyyyMMdd");
                        if (LDateTime.IsDate(obj.NGAY_HET_HLUC, "yyyyMMdd"))
                        {
                            chkKhachHangHetHL.IsChecked = true;
                            raddtNgayHetHL.Value = LDateTime.StringToDate(obj.NGAY_HET_HLUC, "yyyyMMdd");
                        }
                        //cmbLyDoRaKhoiNhom.SelectedIndex = -1;
                        if (LDateTime.IsDate(obj.NGAY_THANH_LAP, "yyyyMMdd"))
                            raddtNgayThanhLapTC.Value = LDateTime.StringToDate(obj.NGAY_THANH_LAP, "yyyyMMdd");                        
                        raddtNgaySinh.Value = null;                        
                        txtSoCMND.Text = obj.DD_GTLQ_SO;
                        if (LDateTime.IsDate(obj.DD_GTLQ_NGAY_CAP, "yyyyMMdd"))
                            raddtNgayCap.Value = LDateTime.StringToDate(obj.DD_GTLQ_NGAY_CAP, "yyyyMMdd");
                        txtNoiCap.Text = obj.DD_GTLQ_NOI_CAP;
                        cmbDanToc.SelectedIndex = -1;
                        txtSoHoKhau.Text = obj.SO_HO_KHAU;
                        cmbTinhTrangHonNhan.SelectedIndex = -1;
                        cmbLoaiHinhToChuc.SelectedIndex = -1;
                        cmbNganhKinhTe.SelectedIndex = lstSourceNganhKT.IndexOf(lstSourceNganhKT.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_NGANH_KT)));
                        ngayCongNhan = obj.NGAY_CONG_NHAN;
                        txtTenBo.Text = obj.TEN_BO;
                        #endregion

                        #region Thông tin tài sản của doanh nghiệp
                        numGiaTriTaiSan.Value = Convert.ToDouble(obj.TONG_TAI_SAN);
                        numVonDieuLe.Value = Convert.ToDouble(obj.VON_DIEU_LE);
                        #endregion

                        #region Hộ khẩu thường chú
                        if (obj.MA_TTHI_NTHON_HTAI.Equals("THANH_THI"))
                            radioThanhThiHienTai.IsChecked = true;
                        else
                            radioNongThonHienTai.IsChecked = true;

                        cmbTinhTP.SelectedIndex = 0;
                        cmbQuanHuyen.SelectedIndex = -1;
                        cmbXaPhuong.SelectedIndex = -1;
                        txtDiaChi.Text = "";
                        #endregion

                        #region Địa chỉ hiện tại
                        if (obj.MA_TTHI_NTHON_HTAI.Equals("THANH_THI"))
                            radioThanhThiHienTai.IsChecked = true;
                        else
                            radioNongThonHienTai.IsChecked = true;

                        cmbVungMienHienTai.SelectedIndex = lstSourceVungMien.IndexOf(lstSourceVungMien.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_VUNG)));
                        cmbTinhTPHienTai.SelectedIndex = lstSourceTinhTp.IndexOf(lstSourceTinhTp.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_TINHTP)));
                        cmbQuanHuyenHienTai.SelectedIndex = lstSourceQuanHuyen.IndexOf(lstSourceQuanHuyen.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_QUAN)));
                        cmbXaPhuongHienTai.SelectedIndex = lstSourceXaPhuong.IndexOf(lstSourceXaPhuong.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_PHUONG)));
                        cmbLangTodpHienTai.SelectedIndex = lstSourceLangTodp.IndexOf(lstSourceLangTodp.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_TODP)));
                        txtDiaChiHienTai.Text = obj.DIA_CHI;
                        #endregion

                        #region Số điện thoại email
                        txtSoCoDinh.Text = obj.SO_DTHOAI;
                        txtSoDiDong.Text = obj.SO_DDONG;
                        txtEmail.Text = obj.EMAIL;
                        if (!obj.EMAIL_TYPE.IsNullOrEmptyOrSpace())
                            cmbEmail.SelectedIndex = lstSourceLoaiEmail.IndexOf(lstSourceLoaiEmail.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(obj.EMAIL_TYPE)));
                        #endregion

                        #region Grid
                        grNguoiDaiDien.DataContext = dsSource.Tables["VKH_NGUOI_DDIEN"].DefaultView;
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
                    #endregion

                    #region Dữ liệu hình ảnh
                    lstDuLieuHinhAnh = new List<DuLieuHinhAnh>();
                    int ii = 1;
                    foreach (DataRow item in dsSource.Tables["VKH_CKY_HANH"].Rows)
                    {
                        DuLieuHinhAnh duLieuHinhAnh = new DuLieuHinhAnh();
                        duLieuHinhAnh.ID = -1;
                        duLieuHinhAnh.STT = ii;
                        duLieuHinhAnh.MaLoai = item["CKHA_LOAI"].ToString();
                        duLieuHinhAnh.TenLoai = lstLoaiHinhAnhDef.Where(e => e.MaLoai == duLieuHinhAnh.MaLoai).Select(e => e.TenLoai).FirstOrDefault();
                        duLieuHinhAnh.MaDoiTuong = item["CKHA_DTUONG"].ToString();
                        duLieuHinhAnh.TenDoiTuong = lstDoiTuongHinhAnhDef.Where(e => e.MaLoai == duLieuHinhAnh.MaLoai && e.MaDoiTuong == duLieuHinhAnh.MaDoiTuong).Select(e => e.TenDoiTuong).FirstOrDefault();
                        duLieuHinhAnh.MaHinhAnh = item["CKHA_MA"].ToString();
                        duLieuHinhAnh.TenHinhAnh = item["CKHA_MA"].ToString();
                        duLieuHinhAnh.HieuLuc = false;
                        duLieuHinhAnh.HienThiHS = false;
                        duLieuHinhAnh.CHON = false;
                        duLieuHinhAnh.NgayDuLieu = item["NGAY_DL"] != null ? item["NGAY_DL"].ToString() : "";
                        duLieuHinhAnh.NgayHieuLuc = item["CKHA_NGAY_HIEU_LUC"] != null ? item["CKHA_NGAY_HIEU_LUC"].ToString() : "";
                        imageData = lstChuKyHinhAnh.Where(f => f.FileName + ".jpg" == item["CKHA_DUONG_DAN"].ToString()).Select(f => f.FileData).FirstOrDefault();
                        duLieuHinhAnh.Data = imageData;
                        //duLieuHinhAnh.ImageName = item["CKHA_DUONG_DAN"].ToString();
                        duLieuHinhAnh.ImageName = ii.ToString();
                        duLieuHinhAnh.ImageFormat = "jpg";

                        lstDuLieuHinhAnh.Add(duLieuHinhAnh);
                        ii++;
                    }

                    if (lstDuLieuHinhAnh != null && lstDuLieuHinhAnh.Count > 0)
                    {
                        slideImage.lstDuLieuHinhAnh = lstDuLieuHinhAnh;
                        slideImage.SetImage();
                    }
                    #endregion
                }
                else
                {
                    LMessage.ShowMessage("M_ResponseMessage_HanhDong_LayDuLieuKhongThanhCong", LMessage.MessageBoxType.Warning);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M_ResponseMessage_HanhDong_LayDuLieuKhongThanhCong", LMessage.MessageBoxType.Warning);
            }
            finally
            {
                processKhachHang = null;
                listClientResponseDetail = null;
            }
        }

        private void SetGridData()
        {
            KhachHangProcess processKhachHang = new KhachHangProcess();
            dsSource = processKhachHang.getViewKhachHang04(id);           
        }

        private void ResetForm()
        {
            lblTrangThai.Content = "";

            #region Khách hàng thành viên
            if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.TVIEN.layGiaTri()))
            {
                CommonFunction.SetWindowTitle(this, DatabaseConstant.Function.KH_THANH_VIEN);

                #region Thông tin chung
                cmbLoaiKhachHang.SelectedIndex = lstSourceLoaiKH.IndexOf(lstSourceLoaiKH.FirstOrDefault(i => i.KeywordStrings.First().Equals(loaiKhachHang)));
                txtMaKhachHang.Text = "";
                txtTenKhachHang.Text = "";
                txtTenBanDia.Text = "";
                lblNgayCongNhan.Content = LLanguage.SearchResourceByKey("U.KhachHang.KhachHang.ucKhachHangCT01.NgayCongNhan");
                raddtNgayCongNhan.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                chkKhachHangHetHL.IsChecked = false;
                raddtNgayThanhLapTC.Value = null;
                raddtNgayHetHL.Value = null;
                //cmbLyDoRaKhoiNhom.SelectedIndex = -1;
                raddtNgaySinh.Value = null;
                cmbGioiTinh.SelectedIndex = 0;
                lblSoCMND.Content = LLanguage.SearchResourceByKey("U.KhachHang.KhachHang.ucKhachHangCT01.SoCMND");
                txtSoCMND.Text = "";
                raddtNgayCap.Value = null;
                txtNoiCap.Text = "";
                cmbDanToc.SelectedIndex = 0;
                txtSoHoKhau.Text = "";
                cmbTinhTrangHonNhan.SelectedIndex = 0;
                cmbLoaiHinhToChuc.SelectedIndex = -1;
                cmbNganhKinhTe.SelectedIndex = -1;
                cmbNgheNghiep.SelectedIndex = 0;
                #endregion

                #region Thông tin tài sản của doanh nghiệp
                numGiaTriTaiSan.Value = 0;
                numVonDieuLe.Value = 0;
                #endregion

                #region Hộ khẩu thường chú
                radioNongThonThuongChu.IsChecked = true;
                //cmbTinhTP.SelectedIndex = 0;
                cmbTinhTP.SelectedIndex = lstSourceHKTinhTP.IndexOf(lstSourceHKTinhTP.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TinhTPDonViGiaoDich)));
                cmbQuanHuyen.SelectedIndex = -1;
                cmbXaPhuong.SelectedIndex = -1;
                cmbLangTodp.SelectedIndex = -1;
                #endregion

                #region Địa chỉ hiện tại
                radioNongThonHienTai.IsChecked = true;
                //cmbTinhTPHienTai.SelectedIndex = 0;
                cmbTinhTPHienTai.SelectedIndex = lstSourceHKTinhTP.IndexOf(lstSourceHKTinhTP.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TinhTPDonViGiaoDich)));
                cmbQuanHuyenHienTai.SelectedIndex = -1;
                cmbXaPhuongHienTai.SelectedIndex = -1;
                cmbLangTodpHienTai.SelectedIndex = -1;
                #endregion

                #region Số điện thoại email
                txtSoCoDinh.Text = "";
                txtSoDiDong.Text = "";
                txtEmail.Text = "";
                #endregion

                #region Thông tin gia đình và người thừa kế
                grNguoiThuaKe.ContextMenu = null;
                #endregion

                #region Thông tin kiểm soát
                txtTrangThaiBanGhi.Text = "";
                raddtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                txtNguoiLap.Text = ClientInformation.TenDangNhap;
                raddtNgayCapNhat.Value = null;
                txtNguoiCapNhat.Text = "";
                #endregion
            }
            #endregion

            #region Khách hàng cá nhân
            else if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.CNHAN.layGiaTri()))
            {
                CommonFunction.SetWindowTitle(this, DatabaseConstant.Function.KH_CA_NHAN);

                #region Thông tin chung
                cmbLoaiKhachHang.SelectedIndex = lstSourceLoaiKH.IndexOf(lstSourceLoaiKH.FirstOrDefault(i => i.KeywordStrings.First().Equals(loaiKhachHang)));
                txtMaKhachHang.Text = "";
                txtTenKhachHang.Text = "";
                txtTenBanDia.Text = "";
                lblNgayCongNhan.Content = LLanguage.SearchResourceByKey("U.KhachHang.KhachHang.ucKhachHangCT01.NgayCongNhan");
                raddtNgayCongNhan.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                chkKhachHangHetHL.IsChecked = false;
                raddtNgayThanhLapTC.Value = null;
                raddtNgayHetHL.Value = null;
                //cmbLyDoRaKhoiNhom.SelectedIndex = -1;
                raddtNgaySinh.Value = null;
                cmbGioiTinh.SelectedIndex = 0;
                lblSoCMND.Content = LLanguage.SearchResourceByKey("U.KhachHang.KhachHang.ucKhachHangCT01.SoCMND");
                txtSoCMND.Text = "";
                raddtNgayCap.Value = null;
                txtNoiCap.Text = "";
                cmbDanToc.SelectedIndex = 0;
                txtSoHoKhau.Text = "";
                cmbTinhTrangHonNhan.SelectedIndex = 0;
                cmbLoaiHinhToChuc.SelectedIndex = -1;
                cmbNganhKinhTe.SelectedIndex = -1;
                cmbNgheNghiep.SelectedIndex = 0;
                #endregion

                #region Thông tin tài sản của doanh nghiệp
                numGiaTriTaiSan.Value = 0;
                numVonDieuLe.Value = 0;
                #endregion

                #region Hộ khẩu thường chú
                radioNongThonThuongChu.IsChecked = true;
                //cmbTinhTP.SelectedIndex = 0;
                cmbTinhTP.SelectedIndex = lstSourceHKTinhTP.IndexOf(lstSourceHKTinhTP.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TinhTPDonViGiaoDich)));
                cmbQuanHuyen.SelectedIndex = -1;
                cmbXaPhuong.SelectedIndex = -1;
                cmbLangTodp.SelectedIndex = -1;
                #endregion

                #region Địa chỉ hiện tại
                radioNongThonHienTai.IsChecked = true;
                //cmbTinhTPHienTai.SelectedIndex = 0;
                cmbTinhTPHienTai.SelectedIndex = lstSourceHKTinhTP.IndexOf(lstSourceHKTinhTP.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TinhTPDonViGiaoDich)));
                cmbQuanHuyenHienTai.SelectedIndex = -1;
                cmbXaPhuongHienTai.SelectedIndex = -1;
                cmbLangTodpHienTai.SelectedIndex = -1;
                #endregion

                #region Số điện thoại email
                txtSoCoDinh.Text = "";
                txtSoDiDong.Text = "";
                txtEmail.Text = "";
                #endregion

                #region Thông tin gia đình và người thừa kế
                grNguoiThuaKe.ContextMenu = null;
                #endregion

                #region Thông tin kiểm soát
                txtTrangThaiBanGhi.Text = "";
                raddtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                txtNguoiLap.Text = ClientInformation.TenDangNhap;
                raddtNgayCapNhat.Value = null;
                txtNguoiCapNhat.Text = "";
                #endregion
            }
            #endregion

            #region Khách hàng tổ chức
            else if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.DNGHIEP.layGiaTri()))
            {
                CommonFunction.SetWindowTitle(this, DatabaseConstant.Function.KH_TO_CHUC);

                #region Thông tin chung
                cmbLoaiKhachHang.SelectedIndex = lstSourceLoaiKH.IndexOf(lstSourceLoaiKH.FirstOrDefault(i => i.KeywordStrings.First().Equals(loaiKhachHang)));
                txtMaKhachHang.Text = "";
                txtTenKhachHang.Text = "";
                txtTenBanDia.Text = "";
                lblNgayCongNhan.Content = LLanguage.SearchResourceByKey("U.KhachHang.KhachHang.ucKhachHangThanhVien.NgayThamGiaTC:");
                raddtNgayCongNhan.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                chkKhachHangHetHL.IsChecked = false;
                raddtNgayThanhLapTC.Value = null;
                raddtNgayHetHL.Value = null;
                //cmbLyDoRaKhoiNhom.SelectedIndex = -1;
                raddtNgaySinh.Value = null;
                cmbGioiTinh.SelectedIndex = 0;
                lblSoCMND.Content = LLanguage.SearchResourceByKey("U.KhachHang.KhachHang.ucKhachHangDS01.SoDKKD");
                txtSoCMND.Text = "";
                raddtNgayCap.Value = null;
                txtNoiCap.Text = "";
                cmbDanToc.SelectedIndex = 0;
                txtSoHoKhau.Text = "";
                cmbTinhTrangHonNhan.SelectedIndex = 0;
                cmbLoaiHinhToChuc.SelectedIndex = -1;
                cmbNganhKinhTe.SelectedIndex = -1;
                cmbNgheNghiep.SelectedIndex = 0;
                #endregion

                #region Thông tin tài sản của doanh nghiệp
                numGiaTriTaiSan.Value = 0;
                numVonDieuLe.Value = 0;
                #endregion

                #region Hộ khẩu thường chú
                radioNongThonThuongChu.IsChecked = true;
                //cmbTinhTP.SelectedIndex = 0;
                cmbTinhTP.SelectedIndex = lstSourceHKTinhTP.IndexOf(lstSourceHKTinhTP.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TinhTPDonViGiaoDich)));
                cmbQuanHuyen.SelectedIndex = -1;
                cmbXaPhuong.SelectedIndex = -1;
                cmbLangTodp.SelectedIndex = -1;
                #endregion

                #region Địa chỉ hiện tại
                radioNongThonHienTai.IsChecked = true;
                //cmbTinhTPHienTai.SelectedIndex = 0;
                cmbTinhTPHienTai.SelectedIndex = lstSourceHKTinhTP.IndexOf(lstSourceHKTinhTP.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TinhTPDonViGiaoDich)));
                cmbQuanHuyenHienTai.SelectedIndex = -1;
                cmbXaPhuongHienTai.SelectedIndex = -1;
                cmbLangTodpHienTai.SelectedIndex = -1;
                #endregion

                #region Số điện thoại email
                txtSoCoDinh.Text = "";
                txtSoDiDong.Text = "";
                txtEmail.Text = "";
                #endregion

                #region Thông tin kiểm soát
                txtTrangThaiBanGhi.Text = "";
                raddtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                txtNguoiLap.Text = ClientInformation.TenDangNhap;
                raddtNgayCapNhat.Value = null;
                txtNguoiCapNhat.Text = "";
                #endregion

                #region Thông tin kiểm soát
                txtTrangThaiBanGhi.Text = "";
                raddtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                txtNguoiLap.Text = ClientInformation.TenDangNhap;
                raddtNgayCapNhat.Value = null;
                txtNguoiCapNhat.Text = "";
                #endregion
            }
            #endregion            

        }

        private void OnPreviewHoSo()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(txtMaKhachHang.Text))
            {
                LMessage.ShowMessage("M.ResponseMessage.Common.KhongTonTaiDuLieu", LMessage.MessageBoxType.Warning);

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

                    lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaKhachHang", txtMaKhachHang.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@IdKhachHang", id.ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.KHTV_HO_SO);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
                else if(ClientInformation.Company.Equals("HOCVIENNGANHANG"))
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();

                    List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                    lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaKhachHang", txtMaKhachHang.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@IdKhachHang", id.ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoHVNH(DatabaseConstant.DanhSachBaoCaoHVNH.KHTV_HO_SO_KHACH_HANG_THAM_DINH_THONG_TIN);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
                else
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();

                    List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                    lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaKhachHang", txtMaKhachHang.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@IdKhachHang", id.ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.KHTV_HO_SO);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private bool Validation()
        {
            try
            {
                #region Khách hàng thành viên
                if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.TVIEN.layGiaTri()))
                {
                    #region Thông tin chung
                    if (txtTenKhachHang.Text.IsNullOrEmptyOrSpace())
                    {
                        CommonFunction.ThongBaoChuaNhap(lblTenKhachHang.Content.ToString());
                        txtTenKhachHang.Focus();
                        return false;
                    }
                    if (raddtNgayCongNhan.Value == null)
                    {
                        CommonFunction.ThongBaoChuaNhap(lblNgayCongNhan.Content.ToString());
                        raddtNgayCongNhan.Focus();
                        return false;
                    }
                    if (raddtNgaySinh.Value == null)
                    {
                        CommonFunction.ThongBaoChuaNhap(lblNgaySinh.Content.ToString());
                        raddtNgaySinh.Focus();
                        return false;
                    }
                    else
                    {
                        int canTren = 60;
                        int canDuoi = 18;
                        string canTrenTS = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_KH_MAX_TUOI, ClientInformation.MaDonVi);
                        string canDuoiTS = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_KH_MIN_TUOI, ClientInformation.MaDonVi);

                        if (canTrenTS != "")
                            canTren = Convert.ToInt32(canTrenTS);

                        if (canDuoiTS != "")
                            canDuoi = Convert.ToInt32(canDuoiTS);

                        DateTime ngayLapViecHienTai = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                        DateTime ngaySinh = Convert.ToDateTime(raddtNgaySinh.Value);
                        DateTime ngaySinh1 = Convert.ToDateTime(raddtNgaySinh.Value);
                        
                        if(ngaySinh.AddYears(canDuoi) > ngayLapViecHienTai || ngaySinh1.AddYears(canTren) < ngayLapViecHienTai)
                        {
                            LMessage.ShowMessage("Invalid value for date of birth (age min: " + canDuoiTS + ", age max: " + canTrenTS + ")", LMessage.MessageBoxType.Warning);
                            raddtNgaySinh.Focus();
                            return false;
                        }
                    }

                    if (txtSoCMND.Text.IsNullOrEmptyOrSpace())
                    {
                        CommonFunction.ThongBaoChuaNhap(lblSoCMND.Content.ToString());
                        txtSoCMND.Focus();
                        return false;
                    }

                    if (!LObject.IsNullOrEmpty(txtSoCMND.Text))
                    {
                        if (!CommonFunction.IsValidFormat(ApplicationConstant.FormatType.CMND,
                            txtSoCMND.Text))
                        {
                            txtSoCMND.Focus();
                            return false;
                        }
                    }

                    //if (raddtNgayCap.Value == null || raddtNgayCap.Text.Equals("__/__/____"))
                    //{
                    //    CommonFunction.ThongBaoChuaNhap(lblNgayCap.Content.ToString());
                    //    raddtNgayCap.Focus();
                    //    return false;
                    //}
                    if (txtNoiCap.Text.IsNullOrEmptyOrSpace())
                    {
                        CommonFunction.ThongBaoChuaNhap(lblNoiCap.Content.ToString());
                        txtNoiCap.Focus();
                        return false;
                    }
                    if (cmbDanToc.SelectedIndex < 0)
                    {
                        CommonFunction.ThongBaoChuaChon(lblDanDoc.Content.ToString());
                        cmbDanToc.Focus();
                        return false;
                    }


                    //Khác
                    if (chkKhachHangHetHL.IsChecked == true)
                    {
                        if (raddtNgayHetHL.Value == null || raddtNgayHetHL.Text.Equals("__/__/____"))
                        {
                            CommonFunction.ThongBaoChuaNhap(lblNgayHetHL.Content.ToString());
                            raddtNgayHetHL.Focus();
                            return false;
                        }
                    }
                    #endregion

                    #region Hộ khẩu thường chú
                    if (txtDiaChi.Text.IsNullOrEmptyOrSpace())
                    {
                        CommonFunction.ThongBaoChuaNhap(lblDiaChi.Content.ToString());
                        tbiThongTinLienHe.IsSelected = true;
                        txtDiaChi.Focus();
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
                    #endregion

                    if (!LObject.IsNullOrEmpty(txtEmail.Text))
                    {
                        if (!CommonFunction.IsValidFormat(ApplicationConstant.FormatType.EMAIL,
                            txtEmail.Text))
                        {
                            txtEmail.Focus();
                            return false;
                        }
                    }

                    if (!LObject.IsNullOrEmpty(txtSoDiDong.Text))
                    {
                        if (!CommonFunction.IsValidFormat(ApplicationConstant.FormatType.PHONE,
                            txtSoDiDong.Text))
                        {
                            txtSoDiDong.Focus();
                            return false;
                        }
                    }

                    if (!LObject.IsNullOrEmpty(txtSoCoDinh.Text))
                    {
                        if (!CommonFunction.IsValidFormat(ApplicationConstant.FormatType.PHONE,
                            txtSoCoDinh.Text))
                        {
                            txtSoCoDinh.Focus();
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

                    if (!LObject.IsNullOrEmpty(txtNhanhSo.Text))
                    {
                        if (!CommonFunction.IsValidFormat(ApplicationConstant.FormatType.PHONE,
                            txtNhanhSo.Text))
                        {
                            txtNhanhSo.Focus();
                            return false;
                        }
                    }

                    if (!LObject.IsNullOrEmpty(txtSoDiDongTChieu.Text))
                    {
                        if (!CommonFunction.IsValidFormat(ApplicationConstant.FormatType.PHONE,
                            txtSoDiDongTChieu.Text))
                        {
                            txtSoDiDongTChieu.Focus();
                            return false;
                        }
                    }

                    if (!LObject.IsNullOrEmpty(txtSoDienThoaiTChieu.Text))
                    {
                        if (!CommonFunction.IsValidFormat(ApplicationConstant.FormatType.PHONE,
                            txtSoDienThoaiTChieu.Text))
                        {
                            txtSoDienThoaiTChieu.Focus();
                            return false;
                        }
                    }

                    foreach (DataRow dr in dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows)
                    {
                        if (dr["SO_CMND"] != null && dr["SO_CMND"].ToString() == txtSoCMND.Text)
                        {
                            LMessage.ShowMessage("M_ResponseMessage_KhachHang_SoCMNDKhongHopLe", LMessage.MessageBoxType.Warning);
                            txtSoCMND.Focus();
                            return false;
                        }

                    }
                }

                #endregion

                #region Khách hàng cá nhân
                else if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.CNHAN.layGiaTri()))
                {
                    #region Thông tin chung
                    if (txtTenKhachHang.Text.IsNullOrEmptyOrSpace())
                    {
                        CommonFunction.ThongBaoChuaNhap(lblTenKhachHang.Content.ToString());
                        txtTenKhachHang.Focus();
                        return false;
                    }
                    if (raddtNgayCongNhan.Value == null)
                    {
                        CommonFunction.ThongBaoChuaNhap(lblNgayCongNhan.Content.ToString());
                        raddtNgayCongNhan.Focus();
                        return false;
                    }
                    if (raddtNgaySinh.Value == null)
                    {
                        CommonFunction.ThongBaoChuaNhap(lblNgaySinh.Content.ToString());
                        raddtNgaySinh.Focus();
                        return false;
                    }
                    else
                    {
                        int canTren = 60;
                        int canDuoi = 18;
                        string canTrenTS = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_KH_MAX_TUOI, ClientInformation.MaDonVi);
                        string canDuoiTS = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_KH_MIN_TUOI, ClientInformation.MaDonVi);

                        if (canTrenTS != "")
                            canTren = Convert.ToInt32(canTrenTS);

                        if (canDuoiTS != "")
                            canDuoi = Convert.ToInt32(canDuoiTS);

                        DateTime ngayLapViecHienTai = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                        DateTime ngaySinh = Convert.ToDateTime(raddtNgaySinh.Value);
                        DateTime ngaySinh1 = Convert.ToDateTime(raddtNgaySinh.Value);

                        if (ngaySinh.AddYears(canDuoi) > ngayLapViecHienTai || ngaySinh1.AddYears(canTren) < ngayLapViecHienTai)
                        {
                            LMessage.ShowMessage("M.ResponseMessage.Common.InvalidDateOfBirthValue", LMessage.MessageBoxType.Warning);
                            raddtNgaySinh.Focus();
                            return false;
                        }
                    }

                    if (txtSoCMND.Text.IsNullOrEmptyOrSpace())
                    {
                        CommonFunction.ThongBaoChuaNhap(lblSoCMND.Content.ToString());
                        txtSoCMND.Focus();
                        return false;
                    }

                    if (!LObject.IsNullOrEmpty(txtSoCMND.Text))
                    {
                        if (!CommonFunction.IsValidFormat(ApplicationConstant.FormatType.CMND,
                            txtSoCMND.Text))
                        {
                            txtSoCMND.Focus();
                            return false;
                        }
                    }

                    //if (raddtNgayCap.Value == null || raddtNgayCap.Text.Equals("__/__/____"))
                    //{
                    //    CommonFunction.ThongBaoChuaNhap(lblNgayCap.Content.ToString());
                    //    raddtNgayCap.Focus();
                    //    return false;
                    //}
                    if (txtNoiCap.Text.IsNullOrEmptyOrSpace())
                    {
                        CommonFunction.ThongBaoChuaNhap(lblNoiCap.Content.ToString());
                        txtNoiCap.Focus();
                        return false;
                    }
                    if (cmbDanToc.SelectedIndex < 0)
                    {
                        CommonFunction.ThongBaoChuaChon(lblDanDoc.Content.ToString());
                        cmbDanToc.Focus();
                        return false;
                    }


                    //Khác
                    if (chkKhachHangHetHL.IsChecked == true)
                    {
                        if (raddtNgayHetHL.Value == null || raddtNgayHetHL.Text.Equals("__/__/____"))
                        {
                            CommonFunction.ThongBaoChuaNhap(lblNgayHetHL.Content.ToString());
                            raddtNgayHetHL.Focus();
                            return false;
                        }
                    }
                    #endregion

                    #region Hộ khẩu thường chú
                    if (txtDiaChi.Text.IsNullOrEmptyOrSpace())
                    {
                        CommonFunction.ThongBaoChuaNhap(lblDiaChi.Content.ToString());
                        tbiThongTinLienHe.IsSelected = true;
                        txtDiaChi.Focus();
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
                    #endregion

                    if (!LObject.IsNullOrEmpty(txtEmail.Text))
                    {
                        if (!CommonFunction.IsValidFormat(ApplicationConstant.FormatType.EMAIL,
                            txtEmail.Text))
                        {
                            txtEmail.Focus();
                            return false;
                        }
                    }

                    if (!LObject.IsNullOrEmpty(txtSoDiDong.Text))
                    {
                        if (!CommonFunction.IsValidFormat(ApplicationConstant.FormatType.PHONE,
                            txtSoDiDong.Text))
                        {
                            txtSoDiDong.Focus();
                            return false;
                        }
                    }

                    if (!LObject.IsNullOrEmpty(txtSoCoDinh.Text))
                    {
                        if (!CommonFunction.IsValidFormat(ApplicationConstant.FormatType.PHONE,
                            txtSoCoDinh.Text))
                        {
                            txtSoCoDinh.Focus();
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

                    if (!LObject.IsNullOrEmpty(txtNhanhSo.Text))
                    {
                        if (!CommonFunction.IsValidFormat(ApplicationConstant.FormatType.PHONE,
                            txtNhanhSo.Text))
                        {
                            txtNhanhSo.Focus();
                            return false;
                        }
                    }

                    if (!LObject.IsNullOrEmpty(txtSoDiDongTChieu.Text))
                    {
                        if (!CommonFunction.IsValidFormat(ApplicationConstant.FormatType.PHONE,
                            txtSoDiDongTChieu.Text))
                        {
                            txtSoDiDongTChieu.Focus();
                            return false;
                        }
                    }

                    if (!LObject.IsNullOrEmpty(txtSoDienThoaiTChieu.Text))
                    {
                        if (!CommonFunction.IsValidFormat(ApplicationConstant.FormatType.PHONE,
                            txtSoDienThoaiTChieu.Text))
                        {
                            txtSoDienThoaiTChieu.Focus();
                            return false;
                        }
                    }

                    foreach (DataRow dr in dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows)
                    {
                        if (dr["SO_CMND"] != null && dr["SO_CMND"].ToString() == txtSoCMND.Text)
                        {
                            LMessage.ShowMessage("M_ResponseMessage_KhachHang_SoCMNDKhongHopLe", LMessage.MessageBoxType.Warning);
                            txtSoCMND.Focus();
                            return false;
                        }

                    }
                }
                #endregion

                #region Khách hàng Tổ chức
                else if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.DNGHIEP.layGiaTri()))
                {
                    #region Thông tin chung
                    if (txtTenKhachHang.Text.IsNullOrEmptyOrSpace())
                    {
                        CommonFunction.ThongBaoChuaNhap(lblTenKhachHang.Content.ToString());
                        txtTenKhachHang.Focus();
                        return false;
                    }
                    if (cmbLoaiHinhToChuc.SelectedIndex < 0)
                    {
                        CommonFunction.ThongBaoChuaChon(lblLoaiHinhToChuc.Content.ToString());
                        cmbLoaiHinhToChuc.Focus();
                        return false;
                    }
                    if (cmbNganhKinhTe.SelectedIndex < 0)
                    {
                        CommonFunction.ThongBaoChuaChon(lblNganhKinhTe.Content.ToString());
                        cmbNganhKinhTe.Focus();
                        return false;
                    }

                    //Khác
                    if (chkKhachHangHetHL.IsChecked == true)
                    {
                        if (raddtNgayHetHL.Value == null || raddtNgayHetHL.Text.Equals("__/__/____"))
                        {
                            CommonFunction.ThongBaoChuaNhap(lblNgayHetHL.Content.ToString());
                            raddtNgayHetHL.Focus();
                            return false;
                        }
                    }

                    #endregion

                    #region Tài sản doanh nghiệp
                    if (numGiaTriTaiSan.Value == null || numGiaTriTaiSan.Value == 0)
                    {
                        CommonFunction.ThongBaoChuaNhap(lblGiaTriTaiSan.Content.ToString());
                        numGiaTriTaiSan.Focus();
                        return false;
                    }
                    if (numVonDieuLe.Value == null || numVonDieuLe.Value == 0)
                    {
                        CommonFunction.ThongBaoChuaNhap(lblVonDieuLe.Content.ToString());
                        numVonDieuLe.Focus();
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
                    #endregion    
                    

                    if (!LObject.IsNullOrEmpty(txtEmail.Text))
                    {
                        if (!CommonFunction.IsValidFormat(ApplicationConstant.FormatType.EMAIL,
                            txtEmail.Text))
                        {
                            txtEmail.Focus();
                            return false;
                        }
                    }

                    if (!LObject.IsNullOrEmpty(txtSoDiDong.Text))
                    {
                        if (!CommonFunction.IsValidFormat(ApplicationConstant.FormatType.PHONE,
                            txtSoDiDong.Text))
                        {
                            txtSoDiDong.Focus();
                            return false;
                        }
                    }

                    if (!LObject.IsNullOrEmpty(txtSoCoDinh.Text))
                    {
                        if (!CommonFunction.IsValidFormat(ApplicationConstant.FormatType.PHONE,
                            txtSoCoDinh.Text))
                        {
                            txtSoCoDinh.Focus();
                            return false;
                        }
                    }
                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);                
                return false;
            }
        }

        private void SetVisibledControl()
        {
            #region Khách hàng thành viên
            if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.TVIEN.layGiaTri()))
            {
                //Tab
                tbiThongTinChung.Visibility = System.Windows.Visibility.Visible;
                tbiThongTinLienHe.Visibility = System.Windows.Visibility.Visible;
                tbiNguoiThuaKe.Visibility = System.Windows.Visibility.Visible;
                tbiNguoiDaiDien.Visibility = System.Windows.Visibility.Collapsed;
                tbiKiemSoat.Visibility = System.Windows.Visibility.Visible;
                tbiCongViecHienTai.Visibility = System.Windows.Visibility.Visible;

                //Group
                grbTaiSanDoanhNghiep.Visibility = System.Windows.Visibility.Collapsed;
                grbHoKhauThuongChu.Visibility = System.Windows.Visibility.Visible;


                lblTempNgayThanhLapTC.Visibility = System.Windows.Visibility.Collapsed;
                lblNgayThanhLapTC.Visibility = System.Windows.Visibility.Collapsed;
                raddtNgayThanhLapTC.Visibility = System.Windows.Visibility.Collapsed;
                dtpNgayThanhLapTC.Visibility = System.Windows.Visibility.Collapsed;

                //lblLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Visible;
                //lblStarLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Visible;
                //cmbLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Visible;

                lblTempNgaySinh.Visibility = System.Windows.Visibility.Visible;
                lblNgaySinh.Visibility = System.Windows.Visibility.Visible;
                lblStarNgaySinh.Visibility = System.Windows.Visibility.Visible;
                raddtNgaySinh.Visibility = System.Windows.Visibility.Visible;
                dtpNgaySinh.Visibility = System.Windows.Visibility.Visible;

                lblGioiTinh.Visibility = System.Windows.Visibility.Visible;
                cmbGioiTinh.Visibility = System.Windows.Visibility.Visible;

                lblDanDoc.Visibility = System.Windows.Visibility.Visible;
                lblStarDanToc.Visibility = System.Windows.Visibility.Visible;
                cmbDanToc.Visibility = System.Windows.Visibility.Visible;

                lblTempSoHoKhau.Visibility = System.Windows.Visibility.Visible;
                lblSoHoKhau.Visibility = System.Windows.Visibility.Visible;
                //lblStarSoHoKhau.Visibility = System.Windows.Visibility.Visible;
                txtSoHoKhau.Visibility = System.Windows.Visibility.Visible;

                lblTinhTrangHonNhan.Visibility = System.Windows.Visibility.Visible;
                //lblStarTinhTrangHonNhan.Visibility = System.Windows.Visibility.Visible;
                cmbTinhTrangHonNhan.Visibility = System.Windows.Visibility.Visible;
                lblNgheNghiep.Visibility = System.Windows.Visibility.Visible;
                cmbNgheNghiep.Visibility = System.Windows.Visibility.Visible;

                lblTempLoaiHinhToChuc.Visibility = System.Windows.Visibility.Collapsed;
                lblLoaiHinhToChuc.Visibility = System.Windows.Visibility.Collapsed;
                lblStarLoaiHinhToChuc.Visibility = System.Windows.Visibility.Collapsed;
                cmbLoaiHinhToChuc.Visibility = System.Windows.Visibility.Collapsed;
                lblNganhKinhTe.Visibility = System.Windows.Visibility.Collapsed;
                lblStarNganhKinhTe.Visibility = System.Windows.Visibility.Collapsed;
                cmbNganhKinhTe.Visibility = System.Windows.Visibility.Collapsed;
                txtTenBo.Visibility = System.Windows.Visibility.Visible;
                sptTenBo.Visibility = System.Windows.Visibility.Visible;

                //Khác
                if (chkKhachHangHetHL.IsChecked == false)
                {
                    lblStarNgayHetHL.Visibility = System.Windows.Visibility.Collapsed;
                    //lblStarLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    lblStarNgayHetHL.Visibility = System.Windows.Visibility.Visible;
                    //lblStarLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Visible;
                }


            }
            #endregion

            #region Khách hàng cá nhân
            else if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.CNHAN.layGiaTri()))
            {
                //Tab
                tbiThongTinChung.Visibility = System.Windows.Visibility.Visible;
                tbiThongTinLienHe.Visibility = System.Windows.Visibility.Visible;
                tbiNguoiThuaKe.Visibility = System.Windows.Visibility.Visible;
                tbiNguoiDaiDien.Visibility = System.Windows.Visibility.Collapsed;
                tbiKiemSoat.Visibility = System.Windows.Visibility.Visible;
                tbiCongViecHienTai.Visibility = System.Windows.Visibility.Visible;

                //Group
                grbTaiSanDoanhNghiep.Visibility = System.Windows.Visibility.Collapsed;
                grbHoKhauThuongChu.Visibility = System.Windows.Visibility.Visible;


                lblTempNgayThanhLapTC.Visibility = System.Windows.Visibility.Collapsed;
                lblNgayThanhLapTC.Visibility = System.Windows.Visibility.Collapsed;
                raddtNgayThanhLapTC.Visibility = System.Windows.Visibility.Collapsed;
                dtpNgayThanhLapTC.Visibility = System.Windows.Visibility.Collapsed;

                //lblLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Visible;
                //lblStarLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Visible;
                //cmbLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Visible;

                lblTempNgaySinh.Visibility = System.Windows.Visibility.Visible;
                lblNgaySinh.Visibility = System.Windows.Visibility.Visible;
                lblStarNgaySinh.Visibility = System.Windows.Visibility.Visible;
                raddtNgaySinh.Visibility = System.Windows.Visibility.Visible;
                dtpNgaySinh.Visibility = System.Windows.Visibility.Visible;

                lblGioiTinh.Visibility = System.Windows.Visibility.Visible;
                cmbGioiTinh.Visibility = System.Windows.Visibility.Visible;

                lblDanDoc.Visibility = System.Windows.Visibility.Visible;
                lblStarDanToc.Visibility = System.Windows.Visibility.Visible;
                cmbDanToc.Visibility = System.Windows.Visibility.Visible;

                lblTempSoHoKhau.Visibility = System.Windows.Visibility.Visible;
                lblSoHoKhau.Visibility = System.Windows.Visibility.Visible;
                //lblStarSoHoKhau.Visibility = System.Windows.Visibility.Visible;
                txtSoHoKhau.Visibility = System.Windows.Visibility.Visible;

                lblTinhTrangHonNhan.Visibility = System.Windows.Visibility.Visible;
                //lblStarTinhTrangHonNhan.Visibility = System.Windows.Visibility.Visible;
                cmbTinhTrangHonNhan.Visibility = System.Windows.Visibility.Visible;
                lblNgheNghiep.Visibility = System.Windows.Visibility.Visible;
                cmbNgheNghiep.Visibility = System.Windows.Visibility.Visible;

                lblTempLoaiHinhToChuc.Visibility = System.Windows.Visibility.Collapsed;
                lblLoaiHinhToChuc.Visibility = System.Windows.Visibility.Collapsed;
                lblStarLoaiHinhToChuc.Visibility = System.Windows.Visibility.Collapsed;
                cmbLoaiHinhToChuc.Visibility = System.Windows.Visibility.Collapsed;
                lblNganhKinhTe.Visibility = System.Windows.Visibility.Collapsed;
                lblStarNganhKinhTe.Visibility = System.Windows.Visibility.Collapsed;
                cmbNganhKinhTe.Visibility = System.Windows.Visibility.Collapsed;
                txtTenBo.Visibility = System.Windows.Visibility.Visible;
                sptTenBo.Visibility = System.Windows.Visibility.Visible;

                //Khác
                if (chkKhachHangHetHL.IsChecked == false)
                {
                    lblStarNgayHetHL.Visibility = System.Windows.Visibility.Collapsed;
                    //lblStarLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    lblStarNgayHetHL.Visibility = System.Windows.Visibility.Visible;
                    //lblStarLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Visible;
                }
            }
            #endregion

            #region Khách hàng tổ chức
            else if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.DNGHIEP.layGiaTri()))
            {
                //Tab
                tbiThongTinChung.Visibility = System.Windows.Visibility.Visible;
                tbiThongTinLienHe.Visibility = System.Windows.Visibility.Visible;
                tbiNguoiThuaKe.Visibility = System.Windows.Visibility.Collapsed;
                tbiNguoiDaiDien.Visibility = System.Windows.Visibility.Visible;
                tbiKiemSoat.Visibility = System.Windows.Visibility.Visible;
                tbiCongViecHienTai.Visibility = System.Windows.Visibility.Visible;

                //Group
                grbTaiSanDoanhNghiep.Visibility = System.Windows.Visibility.Visible;
                grbHoKhauThuongChu.Visibility = System.Windows.Visibility.Collapsed;

                lblTempNgayThanhLapTC.Visibility = System.Windows.Visibility.Visible;
                lblNgayThanhLapTC.Visibility = System.Windows.Visibility.Visible;
                raddtNgayThanhLapTC.Visibility = System.Windows.Visibility.Visible;
                dtpNgayThanhLapTC.Visibility = System.Windows.Visibility.Visible;

                //lblLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Collapsed;
                //lblStarLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Collapsed;
                //cmbLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Collapsed;

                lblTempNgaySinh.Visibility = System.Windows.Visibility.Collapsed;
                lblNgaySinh.Visibility = System.Windows.Visibility.Collapsed;
                lblStarNgaySinh.Visibility = System.Windows.Visibility.Collapsed;
                raddtNgaySinh.Visibility = System.Windows.Visibility.Collapsed;
                dtpNgaySinh.Visibility = System.Windows.Visibility.Collapsed;

                lblGioiTinh.Visibility = System.Windows.Visibility.Collapsed;
                cmbGioiTinh.Visibility = System.Windows.Visibility.Collapsed;

                lblDanDoc.Visibility = System.Windows.Visibility.Collapsed;
                lblStarDanToc.Visibility = System.Windows.Visibility.Collapsed;
                cmbDanToc.Visibility = System.Windows.Visibility.Collapsed;

                lblTempSoHoKhau.Visibility = System.Windows.Visibility.Collapsed;
                lblSoHoKhau.Visibility = System.Windows.Visibility.Collapsed;
                //lblStarSoHoKhau.Visibility = System.Windows.Visibility.Collapsed;
                txtSoHoKhau.Visibility = System.Windows.Visibility.Collapsed;

                lblTinhTrangHonNhan.Visibility = System.Windows.Visibility.Collapsed;
                //lblStarTinhTrangHonNhan.Visibility = System.Windows.Visibility.Collapsed;
                cmbTinhTrangHonNhan.Visibility = System.Windows.Visibility.Collapsed;
                lblNgheNghiep.Visibility = System.Windows.Visibility.Collapsed;
                cmbNgheNghiep.Visibility = System.Windows.Visibility.Collapsed;

                lblTempLoaiHinhToChuc.Visibility = System.Windows.Visibility.Visible;
                lblLoaiHinhToChuc.Visibility = System.Windows.Visibility.Visible;
                lblStarLoaiHinhToChuc.Visibility = System.Windows.Visibility.Visible;
                cmbLoaiHinhToChuc.Visibility = System.Windows.Visibility.Visible;
                lblNganhKinhTe.Visibility = System.Windows.Visibility.Visible;
                lblStarNganhKinhTe.Visibility = System.Windows.Visibility.Visible;
                cmbNganhKinhTe.Visibility = System.Windows.Visibility.Visible;
                txtTenBo.Visibility = System.Windows.Visibility.Collapsed;
                sptTenBo.Visibility = System.Windows.Visibility.Collapsed;

                //Khác
                if (chkKhachHangHetHL.IsChecked == false)
                {
                    lblStarNgayHetHL.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    lblStarNgayHetHL.Visibility = System.Windows.Visibility.Visible;
                }
            }
            #endregion
        }

        private void SetEnabledControls()
        {
            #region Thêm
            if (action == DatabaseConstant.Action.THEM)
            {
                #region Thông tin chung
                cmbLoaiKhachHang.IsEnabled = true;
                txtMaKhachHang.IsEnabled = false;
                txtTenKhachHang.IsEnabled = true;
                txtTenBanDia.IsEnabled = true;
                raddtNgayCongNhan.IsEnabled = true;
                dtpNgayCongNhan.IsEnabled = true;
                chkKhachHangHetHL.IsEnabled = true;
                raddtNgayThanhLapTC.IsEnabled = true;
                dtpNgayThanhLapTC.IsEnabled = true;
                raddtNgayHetHL.IsEnabled = true;
                dtpNgayHetHL.IsEnabled = true;
                //cmbLyDoRaKhoiNhom.IsEnabled = true;
                raddtNgaySinh.IsEnabled = true;
                dtpNgaySinh.IsEnabled = true;
                cmbGioiTinh.IsEnabled = true;
                txtSoCMND.IsEnabled = true;
                raddtNgayCap.IsEnabled = true;
                dtpNgayCap.IsEnabled = true;
                txtNoiCap.IsEnabled = true;
                cmbDanToc.IsEnabled = true;
                txtSoHoKhau.IsEnabled = true;
                cmbTinhTrangHonNhan.IsEnabled = true;
                cmbLoaiHinhToChuc.IsEnabled = true;
                cmbNganhKinhTe.IsEnabled = true;
                cmbNgheNghiep.IsEnabled = true;
                btnCheck.IsEnabled = true;
                btnCheckHoKhau.IsEnabled = true;
                txtTenBo.IsEnabled = true;
                //Khác
                if (chkKhachHangHetHL.IsChecked == true)
                {
                    raddtNgayHetHL.IsEnabled = true;
                    //cmbLyDoRaKhoiNhom.IsEnabled = true;
                }
                else
                {
                    raddtNgayHetHL.IsEnabled = false;
                    //cmbLyDoRaKhoiNhom.IsEnabled = false;
                }
                #endregion

                #region Tài sản doanh nghiệp
                numGiaTriTaiSan.IsEnabled = true;
                numVonDieuLe.IsEnabled = true;
                #endregion

                #region Hộ khẩu thường chú
                radioNongThonThuongChu.IsEnabled = true;
                radioThanhThiThuongChu.IsEnabled = true;
                cmbTinhTP.IsEnabled = true;
                cmbQuanHuyen.IsEnabled = true;
                cmbXaPhuong.IsEnabled = true;
                cmbLangTodp.IsEnabled = true;
                txtDiaChi.IsEnabled = true;
                #endregion

                #region Địa chỉ hiện tại
                radioNongThonHienTai.IsEnabled = true;
                radioThanhThiHienTai.IsEnabled = true;
                cmbXaPhuongHienTai.IsEnabled = true;
                cmbLangTodpHienTai.IsEnabled = true;
                txtDiaChiHienTai.IsEnabled = true;
                cmbLoaiHinhCuTru.IsEnabled = true;
                txtLoaiHinhCuTru.IsEnabled = true;
                cmbOVoiAi.IsEnabled = true;
                txtOVoiAi.IsEnabled = true;
                txtSoNamCuTru.IsEnabled = true;
                txtSoThangCuTru.IsEnabled = true;
                #endregion

                #region SĐT và email
                txtSoCoDinh.IsEnabled = true;
                txtSoDiDong.IsEnabled = true;
                txtEmail.IsEnabled = true;
                cmbEmail.IsEnabled = true;
                #endregion

                #region Thông tin gia đình và người thừa kế
                btnAddNguoiThuaKe.IsEnabled = true;
                btnModifyNguoiThuaKe.IsEnabled = true;
                btnDeleteNguoiThuaKe.IsEnabled = true;
                
                //grNguoiThuaKe.IsEnabled = true;

                grNguoiDaiDien.IsEnabled = true;
                grbCongViecHienTai.IsEnabled = true;
                grbNguoiThamChieu.IsEnabled = true;
                #endregion

            }
            #endregion

            #region Sửa
            else if (action == DatabaseConstant.Action.SUA)
            {
                #region Thông tin chung
                cmbLoaiKhachHang.IsEnabled = false;
                txtMaKhachHang.IsEnabled = false;
                txtTenKhachHang.IsEnabled = true;
                txtTenBanDia.IsEnabled = true;
                raddtNgayCongNhan.IsEnabled = true;
                dtpNgayCongNhan.IsEnabled = true;
                chkKhachHangHetHL.IsEnabled = true;
                raddtNgayThanhLapTC.IsEnabled = true;
                dtpNgayThanhLapTC.IsEnabled = true;
                raddtNgayHetHL.IsEnabled = true;
                dtpNgayHetHL.IsEnabled = true;
                //cmbLyDoRaKhoiNhom.IsEnabled = true;
                raddtNgaySinh.IsEnabled = true;
                dtpNgaySinh.IsEnabled = true;
                cmbGioiTinh.IsEnabled = true;
                txtSoCMND.IsEnabled = true;
                raddtNgayCap.IsEnabled = true;
                dtpNgayCap.IsEnabled = true;
                txtNoiCap.IsEnabled = true;
                cmbDanToc.IsEnabled = true;
                txtSoHoKhau.IsEnabled = true;
                cmbTinhTrangHonNhan.IsEnabled = true;
                cmbLoaiHinhToChuc.IsEnabled = true;
                cmbNganhKinhTe.IsEnabled = true;
                cmbNgheNghiep.IsEnabled = true;
                btnCheck.IsEnabled = true;
                btnCheckHoKhau.IsEnabled = true;
                txtTenBo.IsEnabled = true;
                //Khác
                if (chkKhachHangHetHL.IsChecked == true)
                {
                    raddtNgayHetHL.IsEnabled = true;
                    //cmbLyDoRaKhoiNhom.IsEnabled = true;
                }
                else
                {
                    raddtNgayHetHL.IsEnabled = false;
                    //cmbLyDoRaKhoiNhom.IsEnabled = false;
                }

                #endregion

                #region Tài sản doanh nghiệp
                numGiaTriTaiSan.IsEnabled = true;
                numVonDieuLe.IsEnabled = true;
                #endregion

                #region Hộ khẩu thường chú
                radioNongThonThuongChu.IsEnabled = true;
                radioThanhThiThuongChu.IsEnabled = true;

                cmbTinhTP.IsEnabled = true;
                cmbQuanHuyen.IsEnabled = true;
                cmbXaPhuong.IsEnabled = true;
                cmbLangTodp.IsEnabled = true;
                txtDiaChi.IsEnabled = true;
                #endregion

                #region Địa chỉ hiện tại
                radioNongThonHienTai.IsEnabled = true;
                radioThanhThiHienTai.IsEnabled = true;

                cmbTinhTPHienTai.IsEnabled = true;
                cmbQuanHuyenHienTai.IsEnabled = true;
                cmbXaPhuongHienTai.IsEnabled = true;
                cmbLangTodpHienTai.IsEnabled = true;
                txtDiaChiHienTai.IsEnabled = true;
                cmbLoaiHinhCuTru.IsEnabled = true;
                txtLoaiHinhCuTru.IsEnabled = true;
                cmbOVoiAi.IsEnabled = true;
                txtOVoiAi.IsEnabled = true;
                txtSoNamCuTru.IsEnabled = true;
                txtSoThangCuTru.IsEnabled = true;
                #endregion

                #region SĐT và email
                txtSoCoDinh.IsEnabled = true;
                txtSoDiDong.IsEnabled = true;
                txtEmail.IsEnabled = true;
                cmbEmail.IsEnabled = true;
                #endregion

                #region Thông tin gia đình và người thừa kế
                btnAddNguoiThuaKe.IsEnabled = true;
                btnModifyNguoiThuaKe.IsEnabled = true;
                btnDeleteNguoiThuaKe.IsEnabled = true;

                //grNguoiThuaKe.IsEnabled = true;

                grNguoiDaiDien.IsEnabled = true;
                grbCongViecHienTai.IsEnabled = true;
                grbNguoiThamChieu.IsEnabled = true;
                #endregion                
            }
            #endregion

            #region Xem ~ Mặc định
            else
            {
                #region Thông tin chung
                cmbLoaiKhachHang.IsEnabled = false;
                txtMaKhachHang.IsEnabled = false;
                txtTenKhachHang.IsEnabled = false;
                txtTenBanDia.IsEnabled = false;
                raddtNgayCongNhan.IsEnabled = false;
                dtpNgayCongNhan.IsEnabled = false;
                chkKhachHangHetHL.IsEnabled = false;
                raddtNgayThanhLapTC.IsEnabled = false;
                dtpNgayThanhLapTC.IsEnabled = false;
                raddtNgayHetHL.IsEnabled = false;
                dtpNgayHetHL.IsEnabled = false;
                //cmbLyDoRaKhoiNhom.IsEnabled = false;
                raddtNgaySinh.IsEnabled = false;
                dtpNgaySinh.IsEnabled = false;
                cmbGioiTinh.IsEnabled = false;
                txtSoCMND.IsEnabled = false;
                raddtNgayCap.IsEnabled = false;
                dtpNgayCap.IsEnabled = false;
                txtNoiCap.IsEnabled = false;
                cmbDanToc.IsEnabled = false;
                txtSoHoKhau.IsEnabled = false;
                cmbTinhTrangHonNhan.IsEnabled = false;
                cmbLoaiHinhToChuc.IsEnabled = false;
                cmbNganhKinhTe.IsEnabled = false;
                cmbNgheNghiep.IsEnabled = false;
                btnCheck.IsEnabled = false;
                btnCheckHoKhau.IsEnabled = false;
                txtTenBo.IsEnabled = false;
                #endregion

                #region Tài sản doanh nghiệp
                numGiaTriTaiSan.IsEnabled = false;
                numVonDieuLe.IsEnabled = false;
                #endregion

                #region Hộ khẩu thường chú
                radioNongThonThuongChu.IsEnabled = false;
                radioThanhThiThuongChu.IsEnabled = false;
                cmbTinhTP.IsEnabled = false;
                cmbQuanHuyen.IsEnabled = false;
                cmbXaPhuong.IsEnabled = false;
                cmbLangTodp.IsEnabled = false;
                txtDiaChi.IsEnabled = false;
                #endregion

                #region Địa chỉ hiện tại
                radioNongThonHienTai.IsEnabled = false;
                radioThanhThiHienTai.IsEnabled = false;
                cmbTinhTPHienTai.IsEnabled = false;
                cmbQuanHuyenHienTai.IsEnabled = false;
                cmbXaPhuongHienTai.IsEnabled = false;
                cmbLangTodpHienTai.IsEnabled = false;
                txtDiaChiHienTai.IsEnabled = false;
                cmbLoaiHinhCuTru.IsEnabled = false;
                txtLoaiHinhCuTru.IsEnabled = false;
                cmbOVoiAi.IsEnabled = false;
                txtOVoiAi.IsEnabled = false;
                txtSoNamCuTru.IsEnabled = false;
                txtSoThangCuTru.IsEnabled = false;

                #endregion

                #region SĐT và email
                txtSoCoDinh.IsEnabled = false;
                txtSoDiDong.IsEnabled = false;
                txtEmail.IsEnabled = false;
                cmbEmail.IsEnabled = false;
                #endregion

                #region Thông tin gia đình và người thừa kế
                btnAddNguoiThuaKe.IsEnabled = false;
                btnModifyNguoiThuaKe.IsEnabled = false;
                btnDeleteNguoiThuaKe.IsEnabled = false;

                //grNguoiThuaKe.IsEnabled = false;

                grNguoiDaiDien.IsEnabled = false;
                grbCongViecHienTai.IsEnabled = false;
                grbNguoiThamChieu.IsEnabled = false;
                #endregion                
            }
            #endregion
        }


        public void OnHold()
        {

        }

        public void OnSave()
        {
            try
            {
                if (!Validation()) return;

                string trangThai = "";
                trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));
                if(trangThai.Equals(BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri()))
                    trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();

                obj = new KH_KHANG_HSO();

                GetFormData(ref obj, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(obj);
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(obj);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
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
            SetVisibledControl();
            BeforeViewFromDetail();
        }


        public void BeforeAddNew()
        {
            action = DatabaseConstant.Action.THEM;
            SetGridData();
            ResetForm();            
            SetVisibledControl();
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnAddNew(KH_KHANG_HSO obj)
        {

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                KhachHangProcess processKhachHang = new KhachHangProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processKhachHang.KhachHang04(DatabaseConstant.Action.THEM, ref obj, ref dsSource, ref lstChuKyHinhAnh, ref listClientResponseDetail);
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

        public void AfterAddNew(bool ret, KH_KHANG_HSO obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);

                    id = obj.ID;
                    txtMaKhachHang.Text = obj.MA_KHANG;
                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);

                    BeforeViewFromDetail();
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

                bool ret = process.LockData(DatabaseConstant.Module.KHTV,
                    DatabaseConstant.Function.KH_THANH_VIEN,
                    DatabaseConstant.Table.KH_KHANG_HSO,
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
            action = DatabaseConstant.Action.SUA;
            SetFormData();
            SetVisibledControl();
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnModify(KH_KHANG_HSO obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                KhachHangProcess processKhachHang = new KhachHangProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processKhachHang.KhachHang04(DatabaseConstant.Action.SUA, ref obj, ref dsSource, ref lstChuKyHinhAnh, ref listClientResponseDetail);
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

        public void AfterModify(bool ret, KH_KHANG_HSO obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);

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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                    DatabaseConstant.Function.KH_THANH_VIEN,
                    DatabaseConstant.Table.KH_KHANG_HSO,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.KHTV,
                        DatabaseConstant.Function.KH_THANH_VIEN,
                        DatabaseConstant.Table.KH_KHANG_HSO,
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                    DatabaseConstant.Function.KH_THANH_VIEN,
                    DatabaseConstant.Table.KH_KHANG_HSO,
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
            KhachHangProcess processKhachHang = new KhachHangProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processKhachHang.KhachHang04(action, ref obj, ref dsSource, ref lstChuKyHinhAnh, ref listClientResponseDetail);
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
                processKhachHang = null;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                    DatabaseConstant.Function.KH_THANH_VIEN,
                    DatabaseConstant.Table.KH_KHANG_HSO,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.KHTV,
                        DatabaseConstant.Function.KH_THANH_VIEN,
                        DatabaseConstant.Table.KH_KHANG_HSO,
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
            KhachHangProcess processKhachHang = new KhachHangProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processKhachHang.KhachHang04(action, ref obj, ref dsSource, ref lstChuKyHinhAnh, ref listClientResponseDetail);
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
                processKhachHang = null;
            }
        }

        public void AfterApprove(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret)
                {
                    //LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);

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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                    DatabaseConstant.Function.KH_THANH_VIEN,
                    DatabaseConstant.Table.KH_KHANG_HSO,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.KHTV,
                        DatabaseConstant.Function.KH_THANH_VIEN,
                        DatabaseConstant.Table.KH_KHANG_HSO,
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
            KhachHangProcess processKhachHang = new KhachHangProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processKhachHang.KhachHang04(action, ref obj, ref dsSource, ref lstChuKyHinhAnh, ref listClientResponseDetail);
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
                processKhachHang = null;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                    DatabaseConstant.Function.KH_THANH_VIEN,
                    DatabaseConstant.Table.KH_KHANG_HSO,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.KHTV,
                        DatabaseConstant.Function.KH_THANH_VIEN,
                        DatabaseConstant.Table.KH_KHANG_HSO,
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
            KhachHangProcess processKhachHang = new KhachHangProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processKhachHang.KhachHang04(action, ref obj, ref dsSource, ref lstChuKyHinhAnh, ref listClientResponseDetail);
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
                processKhachHang = null;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                    DatabaseConstant.Function.KH_THANH_VIEN,
                    DatabaseConstant.Table.KH_KHANG_HSO,
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
