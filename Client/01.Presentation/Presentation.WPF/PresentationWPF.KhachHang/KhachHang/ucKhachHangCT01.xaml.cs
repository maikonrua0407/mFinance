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

namespace PresentationWPF.KhachHang.KhachHang
{
    /// <summary>
    /// Interaction logic for ucKhachHangCT01.xaml
    /// </summary>
    public partial class ucKhachHangCT01 : UserControl
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

        private string quan_huyen_HK, xa_phuong_HK, quan_huyen_hien_tai, xa_phuong_hien_tai;

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

        //Source cac combobox phan thong tin khac
        List<AutoCompleteEntry> lstSourceHonNhan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHocVan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceVaiTroGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLHinhCongTac = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTGianCongTac = new List<AutoCompleteEntry>();

        //Source cac combobox phan ho khau
        List<AutoCompleteEntry> lstSourceHKTinhTP = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHKXaPhuong = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHKQuanHuyen = new List<AutoCompleteEntry>();

        //Source cac combobox phan dia chi hien tai
        List<AutoCompleteEntry> lstSourceTinhTp = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceXaPhuong = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceQuanHuyen = new List<AutoCompleteEntry>();

        //Source cac combobox phan nha cua
        List<AutoCompleteEntry> lstSourceNoiCuTru = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTGCuTru = new List<AutoCompleteEntry>();

        //Source cac combobox du lieu hinh anh
        List<AutoCompleteEntry> lstSourceLoaiHinhAnh = new List<AutoCompleteEntry>();

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
            new LoaiHinhAnh(){ MaLoai = "HSPL", TenLoai = "Hồ sơ pháp lý"},
	        new LoaiHinhAnh(){ MaLoai = "HSKT", TenLoai = "Hồ sơ kinh tế"},
            new LoaiHinhAnh(){ MaLoai = "TCVM", TenLoai = "Quan hệ với tổ chức TCVM"}
        };
        List<LoaiHinhAnh> lstLoaiHinhAnhSel = new List<LoaiHinhAnh>();
        List<DoiTuongHinhAnh> lstDoiTuongHinhAnhDef = new List<DoiTuongHinhAnh>()
        {
            new DoiTuongHinhAnh(){ MaLoai = "HSPL", MaDoiTuong="CK", TenDoiTuong = "Chữ ký"},
            new DoiTuongHinhAnh(){ MaLoai = "HSPL", MaDoiTuong="HA", TenDoiTuong = "Hình ảnh"},
	        new DoiTuongHinhAnh(){ MaLoai = "HSPL", MaDoiTuong="CMTND", TenDoiTuong = "Chứng minh thư nhân dân"},
            new DoiTuongHinhAnh(){ MaLoai = "HSPL", MaDoiTuong="KHAC", TenDoiTuong = "Khác"},

	        new DoiTuongHinhAnh(){ MaLoai = "HSKT", MaDoiTuong="SODO", TenDoiTuong = "Sổ đỏ nhà đất"},
            new DoiTuongHinhAnh(){ MaLoai = "HSKT", MaDoiTuong="HDLD", TenDoiTuong = "Hợp đồng lao động"},
            new DoiTuongHinhAnh(){ MaLoai = "HSKT", MaDoiTuong="SAOKE", TenDoiTuong = "Sao kê tài khoản"},
	        new DoiTuongHinhAnh(){ MaLoai = "HSKT", MaDoiTuong="KHAC", TenDoiTuong = "Khác"},

            new DoiTuongHinhAnh(){ MaLoai = "TCVM", MaDoiTuong="SOTK", TenDoiTuong = "Sổ tiết kiệm"},
            new DoiTuongHinhAnh(){ MaLoai = "TCVM", MaDoiTuong="HDTD", TenDoiTuong = "Hợp đồng tín dụng"},
            new DoiTuongHinhAnh(){ MaLoai = "TCVM", MaDoiTuong="KHTV", TenDoiTuong = "Xác nhận thành viên"},
            new DoiTuongHinhAnh(){ MaLoai = "TCVM", MaDoiTuong="KHAC", TenDoiTuong = "Khác"}
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
        public ucKhachHangCT01()
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
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KhachHang;component/KhachHang/ucKhachHangCT01.xaml", ref Toolbar, ref mnuMain);
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
            lstDieuKien.Add(DatabaseConstant.DanhMuc.LY_DO_VAO_RA.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbLyDoRaKhoiNhom;
            combo.lstSource = lstSourceLyDoRa;
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

            //Tỉnh thành - hộ khẩu
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_TINHTP.getValue();
            combo.combobox = cmbTinhTP;
            combo.lstSource = lstSourceHKTinhTP;
            lstCombobox.Add(combo);

            //Tỉnh thành - Hiện tại
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_TINHTP.getValue();
            combo.combobox = cmbTinhTPHienTai;
            combo.lstSource = lstSourceTinhTp;
            lstCombobox.Add(combo);

            //Loại hình ảnh
            //lstDieuKien = new List<string>();
            //lstDieuKien.Add(DatabaseConstant.DanhMuc.LOAI_HINH_ANH.getValue());
            //combo = new COMBOBOX_DTO();
            //combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            //combo.combobox = cmbLoaiHinhAnh;
            //combo.lstSource = lstSourceLoaiHinhAnh;
            //combo.lstDieuKien = lstDieuKien;
            //lstCombobox.Add(combo);           

            AutoComboBox auto = new AutoComboBox();
            auto.GenAutoComboBoxTheoList(ref lstCombobox);

            auto.removeEntry(ref lstSourceLoaiKH, ref cmbLoaiKhachHang, BusinessConstant.LoaiKhachHang.TCTD.layGiaTri());
            auto.removeEntry(ref lstSourceLoaiKH, ref cmbLoaiKhachHang, BusinessConstant.LoaiKhachHang.VANG_LAI.layGiaTri());


            //Combobox tính chất sử dụng đất - Grid Sử dụng đất
            DataSet ds = new DanhMucProcess().GetDanhMucGTri("TINH_CHAT_SDUNG_DAT");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                ((GridViewComboBoxColumn)grCongCuSuDungDat.Columns["TINH_CHAT"]).ItemsSource = ds.Tables[0].DefaultView;
            }
            else
            {
                ((GridViewComboBoxColumn)grCongCuSuDungDat.Columns["TINH_CHAT"]).ItemsSource = null;
            }

            // Khoi tao combobox cho Chu ky - Hinh anh
            lstLoaiHinhAnhSel = lstLoaiHinhAnhDef;
            cmbLoaiHSHA.ItemsSource = lstLoaiHinhAnhSel;
            cmbLoaiHSHA.DisplayMemberPath = "TenLoai";
            cmbLoaiHSHA.SelectedItem = lstLoaiHinhAnhSel.ElementAt(0);
            cmbLoaiHSHA.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiHSHA_SelectionChanged);

            lstDoiTuongHinhAnhSel = new List<DoiTuongHinhAnh>();
            string MaLoai = ((LoaiHinhAnh)cmbLoaiHSHA.SelectedItem).MaLoai;
            lstDoiTuongHinhAnhSel = lstDoiTuongHinhAnhDef.Where(item => item.MaLoai == MaLoai).ToList();
            cmbDoiTuongHSHA.ItemsSource = lstDoiTuongHinhAnhSel;
            cmbDoiTuongHSHA.DisplayMemberPath = "TenDoiTuong";
            cmbDoiTuongHSHA.SelectedItem = lstDoiTuongHinhAnhSel.ElementAt(0);
        }

        private void cmbLoaiHSHA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbLoaiHSHA.SelectedIndex >= 0)
            {
                lstLoaiHinhAnhSel = new List<LoaiHinhAnh>();
                string MaLoai = ((LoaiHinhAnh)cmbLoaiHSHA.SelectedItem).MaLoai;
                lstDoiTuongHinhAnhSel = lstDoiTuongHinhAnhDef.Where(item => item.MaLoai == MaLoai).ToList();
            }
            else
            {
                lstLoaiHinhAnhSel = new List<LoaiHinhAnh>();
            }

            cmbDoiTuongHSHA.ItemsSource = lstDoiTuongHinhAnhSel;
            cmbDoiTuongHSHA.DisplayMemberPath = "TenDoiTuong";
            cmbDoiTuongHSHA.SelectedItem = lstDoiTuongHinhAnhSel.ElementAt(0);
        }

        private void InitEventHandler()
        {
            chkThemNhieuLan.Checked += new RoutedEventHandler(chkThemNhieuLan_Checked);

            cmbLoaiKhachHang.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiKhachHang_SelectionChanged);

            chkKhachHangHetHL.Checked += new RoutedEventHandler(chkKhachHangHetHL_Checked);
            chkKhachHangHetHL.Unchecked += new RoutedEventHandler(chkKhachHangHetHL_Unchecked);

            cmbTinhTP.SelectionChanged += new SelectionChangedEventHandler(cmbTinhTP_SelectionChanged);
            cmbQuanHuyen.SelectionChanged += new SelectionChangedEventHandler(cmbQuanHuyen_SelectionChanged);
            cmbXaPhuong.SelectionChanged += new SelectionChangedEventHandler(cmbXaPhuong_SelectionChanged);

            cmbTinhTPHienTai.SelectionChanged += new SelectionChangedEventHandler(cmbTinhTPHienTai_SelectionChanged);
            cmbQuanHuyenHienTai.SelectionChanged += new SelectionChangedEventHandler(cmbQuanHuyenHienTai_SelectionChanged);
            cmbXaPhuongHienTai.SelectionChanged += new SelectionChangedEventHandler(cmbXaPhuongHienTai_SelectionChanged);

            cmbTinhTP.KeyDown += new KeyEventHandler(cmbTinhTP_KeyDown);
            cmbQuanHuyen.KeyDown += new KeyEventHandler(cmbQuanHuyen_KeyDown);
            cmbXaPhuong.KeyDown += new KeyEventHandler(cmbXaPhuong_KeyDown);

            cmbTinhTPHienTai.KeyDown += new KeyEventHandler(cmbTinhTPHienTai_KeyDown);
            cmbQuanHuyenHienTai.KeyDown += new KeyEventHandler(cmbQuanHuyenHienTai_KeyDown);
            cmbXaPhuongHienTai.KeyDown += new KeyEventHandler(cmbXaPhuongHienTai_KeyDown);

            grThuNhapTrongTrot.CellEditEnded += new EventHandler<GridViewCellEditEndedEventArgs>(grThuNhapTrongTrot_CellEditEnded);
            grThuNhapChanNuoi.CellEditEnded += new EventHandler<GridViewCellEditEndedEventArgs>(grThuNhapChanNuoi_CellEditEnded);
            grChiPhi.CellEditEnded += new EventHandler<GridViewCellEditEndedEventArgs>(grChiPhi_CellEditEnded);
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

            if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.TVIEN.layGiaTri()))
            {
                lblStarLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Hidden;
                cmbLyDoRaKhoiNhom.IsEnabled = false;
            }                                    
        }

        private void chkKhachHangHetHL_Checked(object sender, RoutedEventArgs e)
        {
            raddtNgayHetHL.IsEnabled = true;
            dtpNgayHetHL.IsEnabled = true;
            raddtNgayHetHL.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            lblStarNgayHetHL.Visibility = System.Windows.Visibility.Visible;

            if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.TVIEN.layGiaTri()))
            {                                
                lblStarLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Visible;
                cmbLyDoRaKhoiNhom.IsEnabled = true;
            }
        }

        private void cmbTinhTP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTinhTP.SelectedIndex >= 0)
            {
                if (lstSourceHKTinhTP.Select(i => i.DisplayName).Contains(cmbTinhTP.Text))
                {
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
                    cmbXaPhuong.Items.Clear();
                    lstSourceHKXaPhuong.Clear();
                    auto.GenAutoComboBox(ref lstSourceHKXaPhuong, ref cmbXaPhuong, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DIABAN), lstDieuKien);
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
            if (cmbXaPhuong.SelectedIndex >= 0)
            {
                xa_phuong_HK = cmbXaPhuong.Text;
                TaoDiaChi();
            }
        }        

        private void cmbTinhTPHienTai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTinhTPHienTai.SelectedIndex >= 0)
            {
                if (lstSourceTinhTp.Select(i => i.DisplayName).Contains(cmbTinhTPHienTai.Text))
                {
                    AutoComboBox auto = new AutoComboBox();
                    //cmbTinhTPHienTai.SelectedIndex = lstSourceHKTinhTP.IndexOf(lstSourceHKTinhTP.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TinhTPDonViGiaoDich)));
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
                    cmbXaPhuongHienTai.Items.Clear();
                    lstSourceXaPhuong.Clear();
                    auto.GenAutoComboBox(ref lstSourceXaPhuong, ref cmbXaPhuongHienTai, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DIABAN), lstDieuKien);
                    if (lstSourceHKXaPhuong.Select(i => i.DisplayName).Contains(xa_phuong_hien_tai))
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
            if (cmbXaPhuongHienTai.SelectedIndex >= 0)
            {
                xa_phuong_hien_tai = cmbXaPhuongHienTai.Text;
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

        private void TaoDiaChi()
        {
            txtDiaChi.Text = "";
            txtDiaChi.Text = cmbXaPhuong.Text;
            if (txtDiaChi.Text.IsNullOrEmptyOrSpace())
                txtDiaChi.Text = cmbQuanHuyen.Text;
            else
                txtDiaChi.Text = txtDiaChi.Text + " - " + cmbQuanHuyen.Text;

            if (txtDiaChi.Text.IsNullOrEmptyOrSpace())
                txtDiaChi.Text = cmbTinhTP.Text;
            else
                txtDiaChi.Text = txtDiaChi.Text + " - " + cmbTinhTP.Text;
        }

        private void TaoDiaChiHienTai()
        {
            txtDiaChiHienTai.Text = "";
            txtDiaChiHienTai.Text = cmbXaPhuongHienTai.Text;
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
            ucGiaDinhNguoiThuaKe uc = new ucGiaDinhNguoiThuaKe();
            uc.Action = DatabaseConstant.Action.THEM;
            uc.DuLieuTraVe = new ucGiaDinhNguoiThuaKe.LayDuLieu(AddToGridNguoiThuaKe);
            window.Content = uc;
            window.Title = LLanguage.SearchResourceByKey("U.KhachHang.KhachHang.Popup.ucGiaDinhNguoiThuaKe.Tittle");
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();            
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
            if (grNguoiThuaKe.SelectedItems.Count > 1)
            {
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                Window window = new Window();
                ucGiaDinhNguoiThuaKe uc = new ucGiaDinhNguoiThuaKe();
                DataRowView dr = (DataRowView)grNguoiThuaKe.SelectedItem;
                int i = Convert.ToInt32(dr["STT"]);
                uc.drSource = dsSource.Tables["VKH_GDINH_NGUOI_TKE"].Rows[i-1];
                uc.Action = DatabaseConstant.Action.SUA;
                uc.DuLieuTraVe = new ucGiaDinhNguoiThuaKe.LayDuLieu(EditToGridNguoiThuaKe);
                window.Content = uc;
                window.Title = LLanguage.SearchResourceByKey("U.KhachHang.KhachHang.Popup.ucGiaDinhNguoiThuaKe.Tittle");
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;                                
                window.ShowDialog();
            }
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

        private bool ValidationHSHA(string action)
        {
            if (cmbLoaiHSHA.SelectedIndex < 0)
            {
                LMessage.ShowMessage("Chưa chọn loại hình ảnh", LMessage.MessageBoxType.Warning);
                cmbLoaiHSHA.Focus();
                return false;
            }

            if (cmbDoiTuongHSHA.SelectedIndex < 0)
            {
                LMessage.ShowMessage("Chưa chọn đối tượng hình ảnh", LMessage.MessageBoxType.Warning);
                cmbDoiTuongHSHA.Focus();
                return false;
            }

            if (txtMaHSHA.Text.IsNullOrEmptyOrSpace())
            {
                LMessage.ShowMessage("Chưa nhập tên hình ảnh", LMessage.MessageBoxType.Warning);
                txtMaHSHA.Focus();
                return false;
            }

            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri("pack://application:,,,/Utilities.Common;component/Images/Other/picture.png");
            logo.EndInit();

            if (imgAvatar.Source == null || imgAvatar.Tag == null || LString.IsNullOrEmptyOrSpace(imgAvatar.Tag.ToString()))
            {
                LMessage.ShowMessage("Chưa chọn hình ảnh", LMessage.MessageBoxType.Warning);
                return false;
            }

            if (action.Equals("THEM"))
            {
                if (lstDuLieuHinhAnh.Count > 0)
                {
                    string tenHinhAnh = txtMaHSHA.Text;
                    if (lstDuLieuHinhAnh.Where(item => item.TenHinhAnh.Equals(tenHinhAnh)).ToList().Count() > 0)
                    {
                        LMessage.ShowMessage("Đã tồn tại tên hình ảnh " + tenHinhAnh, LMessage.MessageBoxType.Warning);
                        txtMaHSHA.Focus();
                        return false;
                    }
                }
            }
            else if (action.Equals("SUA"))
            {
                if (lstDuLieuHinhAnh.Count > 0)
                {
                    string tenHinhAnh = txtMaHSHA.Text;
                    List<string> tenHinhAnhDangChon = new List<string>();
                    foreach (DuLieuHinhAnh dlha in grDLieuHAnh.SelectedItems)
                    {
                        tenHinhAnhDangChon.Add(dlha.TenHinhAnh);
                    }
                    if (lstDuLieuHinhAnh.Where(item => item.TenHinhAnh.Equals(tenHinhAnh) && !tenHinhAnhDangChon.Contains(item.TenHinhAnh)).ToList().Count() > 0)
                    {
                        LMessage.ShowMessage("Đã tồn tại tên hình ảnh " + tenHinhAnh, LMessage.MessageBoxType.Warning);
                        txtMaHSHA.Focus();
                        return false;
                    }
                }
            }
            return true;
        }

        private void tlbAddHinhAnh_Click(object sender, RoutedEventArgs e)
        {
            if (ValidationHSHA("THEM"))
            {
                string MaLoai = ((LoaiHinhAnh)cmbLoaiHSHA.SelectedItem).MaLoai;
                string TenLoai = ((LoaiHinhAnh)cmbLoaiHSHA.SelectedItem).TenLoai;
                string MaDoiTuong = ((DoiTuongHinhAnh)cmbDoiTuongHSHA.SelectedItem).MaDoiTuong;
                string TenDoiTuong = ((DoiTuongHinhAnh)cmbDoiTuongHSHA.SelectedItem).TenDoiTuong;
                string TenHinhAnh = txtMaHSHA.Text;

                int count = grDLieuHAnh.Items != null ? grDLieuHAnh.Items.Count + 1 : 1;
                //dsSource.Tables["VKH_CKY_HANH"].Rows.Add(count, -1, -1, MaLoai, MaDoiTuong, TenHinhAnh, false, count.ToString(), false);

                DuLieuHinhAnh duLieuHinhAnh = new DuLieuHinhAnh();
                duLieuHinhAnh.ID = -1;
                duLieuHinhAnh.STT = count;
                duLieuHinhAnh.MaLoai = MaLoai;
                duLieuHinhAnh.TenLoai = TenLoai;
                duLieuHinhAnh.MaDoiTuong = MaDoiTuong; 
                duLieuHinhAnh.TenDoiTuong = TenDoiTuong;
                duLieuHinhAnh.MaHinhAnh = count.ToString();
                duLieuHinhAnh.TenHinhAnh = TenHinhAnh;
                duLieuHinhAnh.HieuLuc = false;
                duLieuHinhAnh.HienThiHS = false;
                duLieuHinhAnh.CHON = false;
                duLieuHinhAnh.Data = imageData;
                duLieuHinhAnh.ImageName = count.ToString();
                duLieuHinhAnh.ImageFormat = imageFormat;

                lstDuLieuHinhAnh.Add(duLieuHinhAnh);

                grDLieuHAnh.ItemsSource = null;
                grDLieuHAnh.ItemsSource = lstDuLieuHinhAnh;
            }
        }

        private void tlbModifyHinhAnh_Click(object sender, RoutedEventArgs e)
        {
            if (ValidationHSHA("SUA") && grDLieuHAnh.Items.Count > 0)
            {
                string MaLoai = ((LoaiHinhAnh)cmbLoaiHSHA.SelectedItem).MaLoai;
                string TenLoai = ((LoaiHinhAnh)cmbLoaiHSHA.SelectedItem).TenLoai;
                string MaDoiTuong = ((DoiTuongHinhAnh)cmbDoiTuongHSHA.SelectedItem).MaDoiTuong;
                string TenDoiTuong = ((DoiTuongHinhAnh)cmbDoiTuongHSHA.SelectedItem).TenDoiTuong;
                string TenHinhAnh = txtMaHSHA.Text;

                //int count = grDLieuHAnh.Items != null ? grDLieuHAnh.Items.Count + 1 : 1;
                //dsSource.Tables["VKH_CKY_HANH"].Rows.Add(count, -1, -1, MaLoai, MaDoiTuong, TenHinhAnh, false, count.ToString(), false);
                lstDuLieuHinhAnh = grDLieuHAnh.ItemsSource as List<DuLieuHinhAnh>;

                DuLieuHinhAnh duLieuHinhAnh = grDLieuHAnh.SelectedItem as DuLieuHinhAnh;
                int index = lstDuLieuHinhAnh.IndexOf(duLieuHinhAnh);

                duLieuHinhAnh.ID = -1;
                //duLieuHinhAnh.STT = count;
                duLieuHinhAnh.MaLoai = MaLoai;
                duLieuHinhAnh.TenLoai = TenLoai;
                duLieuHinhAnh.MaDoiTuong = MaDoiTuong;
                duLieuHinhAnh.TenDoiTuong = TenDoiTuong;
                //duLieuHinhAnh.MaHinhAnh = count.ToString();
                duLieuHinhAnh.TenHinhAnh = TenHinhAnh;
                duLieuHinhAnh.HieuLuc = false;
                duLieuHinhAnh.HienThiHS = false;
                duLieuHinhAnh.CHON = false;
                duLieuHinhAnh.Data = imageData;
                //duLieuHinhAnh.ImageName = count.ToString();
                duLieuHinhAnh.ImageFormat = imageFormat;

                lstDuLieuHinhAnh[index] = duLieuHinhAnh;

                grDLieuHAnh.ItemsSource = null;
                grDLieuHAnh.ItemsSource = lstDuLieuHinhAnh;
            }
        }

        private void tlbDeleteHinhAnh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grDLieuHAnh.SelectedItems.Count > 0)
                {
                    //List<DuLieuHinhAnh> lstRowDel = new List<DuLieuHinhAnh>();
                    //foreach (var item in grDLieuHAnh.SelectedItems)
                    //{
                    //    string tenHinhAnh = ((DataRow)item).Field<string>("TenHinhAnh");
                    //    DuLieuHinhAnh r = lstDuLieuHinhAnh.FirstOrDefault(d => d.TenHinhAnh.Equals(tenHinhAnh));
                    //    lstRowDel.Add(r);
                    //}

                    //foreach (DuLieuHinhAnh item in lstRowDel)
                    //{
                    //    for (int i = lstDuLieuHinhAnh.Count - 1; i >= 0; i--)
                    //    {
                    //        if ((lstDuLieuHinhAnh.ElementAt(i).TenHinhAnh).Equals(item.TenHinhAnh))
                    //        {
                    //            lstDuLieuHinhAnh.RemoveAt(i);
                    //            break;
                    //        }
                    //    }
                    //}
                    //for (int i = lstDuLieuHinhAnh.Count; i > 0; i--)
                    //{
                    //    lstDuLieuHinhAnh.ElementAt(i).STT = i;
                    //}
                    foreach (DuLieuHinhAnh dlha in grDLieuHAnh.SelectedItems)
                    {
                        lstDuLieuHinhAnh.Remove(dlha);
                    }
                    grDLieuHAnh.ItemsSource = null;
                    grDLieuHAnh.ItemsSource = lstDuLieuHinhAnh;
                }
            }
            catch (System.Exception ex)
            {

            }
        }

        private void grDLieuHAnh_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (grDLieuHAnh.SelectedItems.Count > 0)
            {
                List<string> key = new List<string>();
                foreach (DuLieuHinhAnh dlha in grDLieuHAnh.SelectedItems)
                {
                    key.Add(dlha.TenHinhAnh);
                }
                string keySelected = key.FirstOrDefault();
                DuLieuHinhAnh duLieuHinhAnhSelected = lstDuLieuHinhAnh.Where(item => item.TenHinhAnh.Equals(keySelected)).FirstOrDefault();
                
                lstLoaiHinhAnhSel = lstLoaiHinhAnhDef;
                cmbLoaiHSHA.ItemsSource = lstLoaiHinhAnhSel;
                LoaiHinhAnh loaiHA = lstLoaiHinhAnhSel.Where(item => item.MaLoai == duLieuHinhAnhSelected.MaLoai).FirstOrDefault();
                cmbLoaiHSHA.DisplayMemberPath = "TenLoai";
                cmbLoaiHSHA.SelectedItem = loaiHA;

                lstDoiTuongHinhAnhSel = new List<DoiTuongHinhAnh>();
                string MaLoai = duLieuHinhAnhSelected.MaLoai;
                lstDoiTuongHinhAnhSel = lstDoiTuongHinhAnhDef.Where(item => item.MaLoai == MaLoai).ToList();
                cmbDoiTuongHSHA.ItemsSource = lstDoiTuongHinhAnhSel;
                DoiTuongHinhAnh doiTuongHA = lstDoiTuongHinhAnhSel.Where(item => item.MaDoiTuong == duLieuHinhAnhSelected.MaDoiTuong).FirstOrDefault();
                cmbDoiTuongHSHA.DisplayMemberPath = "TenDoiTuong";
                cmbDoiTuongHSHA.SelectedItem = doiTuongHA;

                txtMaHSHA.Text = duLieuHinhAnhSelected.TenHinhAnh;

                byte[] source = duLieuHinhAnhSelected.Data;
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage = LImage.LoadImageFromByteArray(source);
                if (myBitmapImage != null)
                {
                    imgAvatar.Source = myBitmapImage;
                    imgAvatar.Tag = duLieuHinhAnhSelected.TenHinhAnh;

                    imageData = source;
                    imageName = duLieuHinhAnhSelected.ImageName;
                    imageFormat = duLieuHinhAnhSelected.ImageFormat;
                }
            }
        }

        private void imgAvatar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                try
                {
                    dlg.FileName = "Document";
                    dlg.DefaultExt = ".jpg";
                    dlg.Filter = "Image (.jpg)|*.jpg";

                    // Show open file dialog box
                    Nullable<bool> result = dlg.ShowDialog();

                    if (result == true)
                    {
                        imgAvatar.Tag = dlg.FileName;
                        LoadImageInClient(dlg.FileName, imgAvatar);

                        imageData = LImage.GetByteArrayFromImage(dlg.FileName);
                        string[] str = @dlg.FileName.Split('\\');
                        string imageFullName = str[str.Length - 1];
                        string[] strFormat = imageFullName.Split('.');                        
                        imageFormat = strFormat[strFormat.Length - 1];
                        imageName = "";
                    }
                }
                catch (Exception ex)
                {
                    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                }
            }
        }

        private void LoadImageInClient(string path, Image img)
        {
            // Tạo image source
            BitmapImage myBitmapImage = new BitmapImage();

            // Set image vào image box
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(path);
            myBitmapImage.DecodePixelWidth = (int)brdAvatar.ActualWidth;
            myBitmapImage.DecodePixelHeight = (int)brdAvatar.ActualHeight;
            myBitmapImage.EndInit();
            img.Source = myBitmapImage;            
        }

        private void LoadImageInServer(string imageName, Image img)
        {
            Presentation.Process.DanhMucProcess process = new Presentation.Process.DanhMucProcess();
            byte[] source = process.LayAnhTuSever(DatabaseConstant.Table.KH_KHANG_HSO.getValue() + "\\" + imageName);
            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage = LImage.LoadImageFromByteArray(source);
            if (myBitmapImage != null)
            {
                img.Source = myBitmapImage;
            }
        }

        private void mniXoaAnh_Click(object sender, RoutedEventArgs e)
        {
            ResetImage();
        }

        private void mniXemAnh_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ResetImage()
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri("pack://application:,,,/Utilities.Common;component/Images/Other/picture.png");
            logo.EndInit();
            imgAvatar.Source = logo;
            imgAvatar.Tag = "ResetImage";
        }

        private void ResetImage(Image img)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri("pack://application:,,,/Utilities.Common;component/Images/Other/picture.png");
            logo.EndInit();
            img.Source = logo;
            img.Tag = "";
        }

        private void ResetHSHA()
        {
            ResetImage(imgAvatar);
            txtMaHSHA.Text = "";
        }

        private void chkAll_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void grThuNhapTrongTrot_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Name.Equals("DOANH_THU") || e.Cell.Column.Name.Equals("CHI_PHI") || e.Cell.Column.Name.Equals("SO_THANG_1_LAN"))
                {
                    DataRowView dr = (DataRowView)grThuNhapTrongTrot.CurrentItem;
                    decimal thang = Convert.ToDecimal(dr["SO_THANG_1_LAN"]);
                    if (thang > 0)
                    {
                        dr["BINH_QUAN_THANG"] = (Convert.ToDecimal(dr["DOANH_THU"]) - Convert.ToDecimal(dr["CHI_PHI"])) / thang;

                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void grThuNhapChanNuoi_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Name.Equals("DOANH_THU1") || e.Cell.Column.Name.Equals("CHI_PHI1") || e.Cell.Column.Name.Equals("SO_THANG_1_LAN1"))
                {
                    DataRowView dr = (DataRowView)grThuNhapChanNuoi.CurrentItem;
                    decimal thang = Convert.ToDecimal(dr["SO_THANG_1_LAN"]);
                    if (thang > 0)
                    {
                        dr["BINH_QUAN_THANG"] = (Convert.ToDecimal(dr["DOANH_THU"]) - Convert.ToDecimal(dr["CHI_PHI"])) / thang;

                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void grChiPhi_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Name.Equals("TONG_CHI_PHI") || e.Cell.Column.Name.Equals("SO_THANG"))
                {
                    DataRowView dr = (DataRowView)grChiPhi.CurrentItem;
                    decimal thang = Convert.ToDecimal(dr["SO_THANG"]);
                    if (thang > 0)
                    {
                        dr["BINH_QUAN_THANG2"] = Convert.ToDecimal(dr["TONG_CHI_PHI"]) / thang;

                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
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
                    if (raddtNgayCongNhan.Value != null)
                        obj.NGAY_THAM_GIA = Convert.ToDateTime(raddtNgayCongNhan.Value).ToString("yyyyMMdd");
                    if (raddtNgayHetHL.Value != null)
                        obj.NGAY_HET_HLUC = Convert.ToDateTime(raddtNgayHetHL.Value).ToString("yyyyMMdd");
                    if(cmbLyDoRaKhoiNhom.SelectedIndex>=0)
                        obj.MA_LY_DO = lstSourceLyDoRa.ElementAt(cmbLyDoRaKhoiNhom.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (raddtNgaySinh.Value != null)
                        obj.DD_NGAY_SINH = Convert.ToDateTime(raddtNgaySinh.Value).ToString("yyyyMMdd");
                    if (cmbGioiTinh.SelectedIndex >= 0)
                        obj.DD_GIOI_TINH = lstSourceGioiTinh.ElementAt(cmbGioiTinh.SelectedIndex).KeywordStrings.ElementAt(0);
                    obj.DD_GTLQ_LOAI = BusinessConstant.LOAI_GIAY_TO.CHUNG_MINH_ND.layGiaTri();
                    obj.DD_GTLQ_SO = txtSoCMND.Text;                    
                    obj.DD_GTLQ_NGAY_CAP = Convert.ToDateTime(raddtNgayCap.Value).ToString("yyyyMMdd");
                    obj.DD_GTLQ_NOI_CAP = txtNoiCap.Text;
                    if(cmbDanToc.SelectedIndex >= 0)
                        obj.DD_MA_DAN_TOC = lstSourceDanToc.ElementAt(cmbDanToc.SelectedIndex).KeywordStrings.ElementAt(0);
                    obj.SO_HO_KHAU = txtSoHoKhau.Text;
                    if(cmbTinhTrangHonNhan.SelectedIndex >= 0)
                        obj.DD_MA_TTRANG_HNHAN = lstSourceHonNhan.ElementAt(cmbTinhTrangHonNhan.SelectedIndex).KeywordStrings.ElementAt(0);
                    obj.NGAY_CONG_NHAN = obj.NGAY_THAM_GIA;
                    #endregion

                    #region Hộ khẩu thường chú
                    obj.DD_TTRU_DIA_CHI = txtDiaChi.Text.Trim();
                    if(cmbTinhTP.SelectedIndex>=0)
                        obj.DD_TTRU_MA_TINHTP = lstSourceHKTinhTP.ElementAt(cmbTinhTP.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbQuanHuyen.SelectedIndex >= 0)
                        obj.DD_TTRU_MA_QUAN = lstSourceHKQuanHuyen.ElementAt(cmbQuanHuyen.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbXaPhuong.SelectedIndex >= 0)
                        obj.DD_TTRU_MA_PHUONG = lstSourceHKXaPhuong.ElementAt(cmbXaPhuong.SelectedIndex).KeywordStrings.ElementAt(0);
                    #endregion

                    #region Địa chỉ hiện tại
                    obj.DIA_CHI = txtDiaChiHienTai.Text.Trim();
                    if (cmbTinhTPHienTai.SelectedIndex >= 0)
                        obj.MA_TINHTP = lstSourceTinhTp.ElementAt(cmbTinhTPHienTai.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbQuanHuyenHienTai.SelectedIndex >= 0)
                        obj.MA_QUAN = lstSourceQuanHuyen.ElementAt(cmbQuanHuyenHienTai.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbXaPhuongHienTai.SelectedIndex >= 0)
                        obj.MA_PHUONG = lstSourceXaPhuong.ElementAt(cmbXaPhuongHienTai.SelectedIndex).KeywordStrings.ElementAt(0);
                    #endregion

                    #region Số điện thoại email
                    obj.SO_DTHOAI = txtSoCoDinh.Text;
                    obj.SO_DDONG = txtSoDiDong.Text;
                    obj.EMAIL = txtEmail.Text;
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

                    #region DataSet

                    #region Xếp loại hộ nghèo chính quyền
                    if (radNgheo.IsChecked == true)
                    {
                        dsSource.Tables["VKH_HO_NGHEO"].Rows[0]["GIA_TRI"] = BusinessConstant.LOAI_HO_NGHEO.NGHEO.layGiaTri();                        
                    }
                    else if (radCanNgheo.IsChecked == true)
                    {
                        dsSource.Tables["VKH_HO_NGHEO"].Rows[0]["GIA_TRI"] = BusinessConstant.LOAI_HO_NGHEO.CAN_NGHEO.layGiaTri();
                    }
                    else
                    {
                        dsSource.Tables["VKH_HO_NGHEO"].Rows[0]["GIA_TRI"] = BusinessConstant.LOAI_HO_NGHEO.KHONG.layGiaTri();
                    }
                    #endregion

                    #region Nhà ở
                    //Mái nhà
                    if (radNgoi_MaiNha.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[0]["MA"] = BusinessConstant.NHA_O_LOAI.MAI_NHA.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[0]["GIA_TRI"] = BusinessConstant.NHA_O_MAI_NHA.NGOI.layGiaTri();
                    }
                    else if (radTon_MaiNha.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[0]["MA"] = BusinessConstant.NHA_O_LOAI.MAI_NHA.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[0]["GIA_TRI"] = BusinessConstant.NHA_O_MAI_NHA.TON.layGiaTri();
                    }
                    else if (radXiMang_MaiNha.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[0]["MA"] = BusinessConstant.NHA_O_LOAI.MAI_NHA.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[0]["GIA_TRI"] = BusinessConstant.NHA_O_MAI_NHA.XI_MANG.layGiaTri();
                    }
                    else if (radTranhLa_MaiNha.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[0]["MA"] = BusinessConstant.NHA_O_LOAI.MAI_NHA.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[0]["GIA_TRI"] = BusinessConstant.NHA_O_MAI_NHA.TRANH_LA.layGiaTri();
                    }
                    else if (radKhac_MaiNha.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[0]["MA"] = BusinessConstant.NHA_O_LOAI.MAI_NHA.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[0]["GIA_TRI"] = BusinessConstant.NHA_O_MAI_NHA.KHAC.layGiaTri();
                    }

                    //Tường
                    if (radXay_Tuong.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[1]["MA"] = BusinessConstant.NHA_O_LOAI.TUONG.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[1]["GIA_TRI"] = BusinessConstant.NHA_O_TUONG.XAY.layGiaTri();
                    }
                    else if (radTon_Tuong.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[1]["MA"] = BusinessConstant.NHA_O_LOAI.TUONG.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[1]["GIA_TRI"] = BusinessConstant.NHA_O_TUONG.TON.layGiaTri();
                    }
                    else if (radVan_Tuong.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[1]["MA"] = BusinessConstant.NHA_O_LOAI.TUONG.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[1]["GIA_TRI"] = BusinessConstant.NHA_O_TUONG.VAN.layGiaTri();
                    }
                    else if (radLa_Tuong.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[1]["MA"] = BusinessConstant.NHA_O_LOAI.TUONG.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[1]["GIA_TRI"] = BusinessConstant.NHA_O_TUONG.LA.layGiaTri();
                    }
                    else if (radKhac_Tuong.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[1]["MA"] = BusinessConstant.NHA_O_LOAI.TUONG.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[1]["GIA_TRI"] = BusinessConstant.NHA_O_TUONG.KHAC.layGiaTri();
                    }

                    //Nền
                    if (radGachMen_Nen.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[2]["MA"] = BusinessConstant.NHA_O_LOAI.NEN.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[2]["GIA_TRI"] = BusinessConstant.NHA_O_NEN.GACH_MEN.layGiaTri();
                    }
                    else if (radGachTau_Nen.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[2]["MA"] = BusinessConstant.NHA_O_LOAI.NEN.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[2]["GIA_TRI"] = BusinessConstant.NHA_O_NEN.GACH_TAU.layGiaTri();
                    }
                    else if (radXiMang_Nen.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[2]["MA"] = BusinessConstant.NHA_O_LOAI.NEN.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[2]["GIA_TRI"] = BusinessConstant.NHA_O_NEN.XI_MANG.layGiaTri();
                    }
                    else if (radDat_Nen.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[2]["MA"] = BusinessConstant.NHA_O_LOAI.NEN.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[2]["GIA_TRI"] = BusinessConstant.NHA_O_NEN.DAT.layGiaTri();
                    }
                    else if (radKhac_nen.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[2]["MA"] = BusinessConstant.NHA_O_LOAI.NEN.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[2]["GIA_TRI"] = BusinessConstant.NHA_O_NEN.KHAC.layGiaTri();
                    }

                    //Sân
                    if (radGach_San.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[3]["MA"] = BusinessConstant.NHA_O_LOAI.SAN.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[3]["GIA_TRI"] = BusinessConstant.NHA_O_SAN.GACH.layGiaTri();
                    }
                    else if (radXiMang_San.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[3]["MA"] = BusinessConstant.NHA_O_LOAI.SAN.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[3]["GIA_TRI"] = BusinessConstant.NHA_O_SAN.XI_MANG.layGiaTri();
                    }
                    else if (radDat_San.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[3]["MA"] = BusinessConstant.NHA_O_LOAI.SAN.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[3]["GIA_TRI"] = BusinessConstant.NHA_O_SAN.DAT.layGiaTri();
                    }
                    else if (radCat_San.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[3]["MA"] = BusinessConstant.NHA_O_LOAI.SAN.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[3]["GIA_TRI"] = BusinessConstant.NHA_O_SAN.CAT.layGiaTri();
                    }
                    else if (radKhac_San.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[3]["MA"] = BusinessConstant.NHA_O_LOAI.SAN.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[3]["GIA_TRI"] = BusinessConstant.NHA_O_SAN.KHAC.layGiaTri();
                    }


                    //Nước
                    if (radNuocMay_Nuoc.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[4]["MA"] = BusinessConstant.NHA_O_LOAI.NUOC.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[4]["GIA_TRI"] = BusinessConstant.NHA_O_NUOC.NUOC_MAY.layGiaTri();
                    }
                    else if (radCayNuoc_Nuoc.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[4]["MA"] = BusinessConstant.NHA_O_LOAI.NUOC.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[4]["GIA_TRI"] = BusinessConstant.NHA_O_NUOC.CAY_NUOC.layGiaTri();
                    }
                    else if (radBe_Nuoc.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[4]["MA"] = BusinessConstant.NHA_O_LOAI.NUOC.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[4]["GIA_TRI"] = BusinessConstant.NHA_O_NUOC.BE_LU_VAI.layGiaTri();
                    }

                    //Ánh sáng
                    if (radDien_AnhSang.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[5]["MA"] = BusinessConstant.NHA_O_LOAI.ANH_SANG.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[5]["GIA_TRI"] = BusinessConstant.NHA_O_ANH_SANG.DIEN.layGiaTri();
                    }
                    else if (radDau_AnhSang.IsChecked == true)
                    {
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[5]["MA"] = BusinessConstant.NHA_O_LOAI.ANH_SANG.layGiaTri();
                        dsSource.Tables["VKH_NHA_O_CTIET"].Rows[5]["GIA_TRI"] = BusinessConstant.NHA_O_ANH_SANG.DAU.layGiaTri();
                    }
                    #endregion

                    #region Tổng tài sản
                    dsSource.Tables["VKH_TONG_TAI_SAN"].Rows[0]["GIA_TRI"] = numTongTaiSan.Value;
                    #endregion

                    #endregion
                }
                #endregion

                #region Khách hàng cá nhân
                else if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.CNHAN.layGiaTri()))
                {
                    #region Thông tin chung
                    obj.ID = id;
                    obj.ID_DON_VI = ClientInformation.IdDonViGiaoDich;
                    obj.MA_KHANG_LOAI = loaiKhachHang;
                    obj.MA_KHANG = txtMaKhachHang.Text;                    
                    obj.TEN_KHANG = txtTenKhachHang.Text;
                    obj.TEN_GDICH = obj.TEN_KHANG;
                    if (raddtNgayCongNhan.Value != null)
                        obj.NGAY_THAM_GIA = Convert.ToDateTime(raddtNgayCongNhan.Value).ToString("yyyyMMdd");
                    if (raddtNgayHetHL.Value != null)
                        obj.NGAY_HET_HLUC = Convert.ToDateTime(raddtNgayHetHL.Value).ToString("yyyyMMdd");
                    if (cmbLyDoRaKhoiNhom.SelectedIndex >= 0)
                        obj.MA_LY_DO = lstSourceLyDoRa.ElementAt(cmbLyDoRaKhoiNhom.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (raddtNgaySinh.Value != null)
                        obj.DD_NGAY_SINH = Convert.ToDateTime(raddtNgaySinh.Value).ToString("yyyyMMdd");
                    if (cmbGioiTinh.SelectedIndex >= 0)
                        obj.DD_GIOI_TINH = lstSourceGioiTinh.ElementAt(cmbGioiTinh.SelectedIndex).KeywordStrings.ElementAt(0);
                    obj.DD_GTLQ_LOAI = BusinessConstant.LOAI_GIAY_TO.CHUNG_MINH_ND.layGiaTri();
                    obj.DD_GTLQ_SO = txtSoCMND.Text;
                    obj.DD_GTLQ_NGAY_CAP = Convert.ToDateTime(raddtNgayCap.Value).ToString("yyyyMMdd");
                    obj.DD_GTLQ_NOI_CAP = txtNoiCap.Text;
                    if (cmbDanToc.SelectedIndex >= 0)
                        obj.DD_MA_DAN_TOC = lstSourceDanToc.ElementAt(cmbDanToc.SelectedIndex).KeywordStrings.ElementAt(0);
                    obj.SO_HO_KHAU = txtSoHoKhau.Text;
                    if (cmbTinhTrangHonNhan.SelectedIndex >= 0)
                        obj.DD_MA_TTRANG_HNHAN = lstSourceHonNhan.ElementAt(cmbTinhTrangHonNhan.SelectedIndex).KeywordStrings.ElementAt(0);
                    obj.NGAY_CONG_NHAN = obj.NGAY_THAM_GIA;
                    #endregion                   

                    #region Địa chỉ hiện tại
                    obj.DIA_CHI = txtDiaChiHienTai.Text.Trim();
                    if (cmbTinhTPHienTai.SelectedIndex >= 0)
                        obj.MA_TINHTP = lstSourceTinhTp.ElementAt(cmbTinhTPHienTai.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbQuanHuyenHienTai.SelectedIndex >= 0)
                        obj.MA_QUAN = lstSourceQuanHuyen.ElementAt(cmbQuanHuyenHienTai.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbXaPhuongHienTai.SelectedIndex >= 0)
                        obj.MA_PHUONG = lstSourceXaPhuong.ElementAt(cmbXaPhuongHienTai.SelectedIndex).KeywordStrings.ElementAt(0);
                    #endregion

                    #region Số điện thoại email
                    obj.SO_DTHOAI = txtSoCoDinh.Text;
                    obj.SO_DDONG = txtSoDiDong.Text;
                    obj.EMAIL = txtEmail.Text;
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
                    if (raddtNgayCongNhan.Value != null)
                        obj.NGAY_THAM_GIA = Convert.ToDateTime(raddtNgayCongNhan.Value).ToString("yyyyMMdd");
                    if (raddtNgayThanhLapTC.Value != null)
                        obj.NGAY_THANH_LAP = Convert.ToDateTime(raddtNgayThanhLapTC.Value).ToString("yyyyMMdd");
                    if (raddtNgayHetHL.Value != null)
                        obj.NGAY_HET_HLUC = Convert.ToDateTime(raddtNgayHetHL.Value).ToString("yyyyMMdd");                                                            
                    obj.DD_GTLQ_LOAI = BusinessConstant.LOAI_GIAY_TO.GP_DKKD.layGiaTri();
                    obj.DD_GTLQ_SO = txtSoCMND.Text;
                    obj.DD_GTLQ_NGAY_CAP = Convert.ToDateTime(raddtNgayCap.Value).ToString("yyyyMMdd");
                    obj.DD_GTLQ_NOI_CAP = txtNoiCap.Text;                    
                    obj.DD_MA_TTRANG_HNHAN = null;
                    if(cmbNganhKinhTe.SelectedIndex >= 0)
                        obj.MA_NGANH_KT = lstSourceNganhKT.ElementAt(cmbNganhKinhTe.SelectedIndex).KeywordStrings.ElementAt(0);
                    obj.NGAY_CONG_NHAN = obj.NGAY_THAM_GIA;
                    #endregion

                    #region Tài sản doanh nghiệp
                    obj.TONG_TAI_SAN = (decimal?)numGiaTriTaiSan.Value;
                    obj.VON_DIEU_LE = (decimal?)numVonDieuLe.Value;
                    #endregion

                    #region Địa chỉ hiện tại
                    obj.DIA_CHI = txtDiaChiHienTai.Text.Trim();
                    if (cmbTinhTPHienTai.SelectedIndex >= 0)
                        obj.MA_TINHTP = lstSourceTinhTp.ElementAt(cmbTinhTPHienTai.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbQuanHuyenHienTai.SelectedIndex >= 0)
                        obj.MA_QUAN = lstSourceQuanHuyen.ElementAt(cmbQuanHuyenHienTai.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (cmbXaPhuongHienTai.SelectedIndex >= 0)
                        obj.MA_PHUONG = lstSourceXaPhuong.ElementAt(cmbXaPhuongHienTai.SelectedIndex).KeywordStrings.ElementAt(0);
                    #endregion

                    #region Số điện thoại email
                    obj.SO_DTHOAI = txtSoCoDinh.Text;
                    obj.SO_DDONG = txtSoDiDong.Text;
                    obj.EMAIL = txtEmail.Text;
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

                #region Chu ky hinh anh
                if (lstDuLieuHinhAnh.Count > 0)
                {
                    lstChuKyHinhAnh = new List<BS_FileObject>();
                    dsSource.Tables["VKH_CKY_HANH"].Rows.Clear();
                    foreach (DuLieuHinhAnh item in lstDuLieuHinhAnh)
                    {
                        int STT = item.STT;
                        int ID = -1;
                        int ID_KHANG = -1;
                        string CKHA_LOAI = item.MaLoai;
                        string CKHA_DTUONG = item.MaDoiTuong;
                        string CKHA_MA = item.TenHinhAnh;                        
                        bool CKHA_HIEU_LUC = false;
                        string CKHA_DUONG_DAN = item.STT.ToString() + "." + item.ImageFormat;
                        bool CKHA_HIEN_THI_HS = false;
                        
                        dsSource.Tables["VKH_CKY_HANH"].Rows.Add(STT, ID, ID_KHANG, CKHA_LOAI, CKHA_DTUONG, CKHA_MA, CKHA_HIEU_LUC, CKHA_DUONG_DAN, CKHA_HIEN_THI_HS);

                        BS_FileObject img = new BS_FileObject();
                        img.FileName = item.ImageName;
                        img.FileFormat = item.ImageFormat;
                        img.FileData = item.Data;

                        lstChuKyHinhAnh.Add(img);
                    }
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
                ret = processKhachHang.KhachHang01(DatabaseConstant.Action.LOAD, ref obj, ref dsSource, ref lstChuKyHinhAnh, ref listClientResponseDetail);
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
                        if(LDateTime.IsDate(obj.NGAY_THAM_GIA,"yyyyMMdd"))
                            raddtNgayCongNhan.Value = LDateTime.StringToDate(obj.NGAY_THAM_GIA,"yyyyMMdd");                        
                        if (LDateTime.IsDate(obj.NGAY_HET_HLUC, "yyyyMMdd"))
                            raddtNgayHetHL.Value = LDateTime.StringToDate(obj.NGAY_HET_HLUC, "yyyyMMdd");
                        if (LDateTime.IsDate(obj.NGAY_HET_HLUC, "yyyyMMdd"))
                        {
                            chkKhachHangHetHL.IsChecked = true;
                            bHetHLuc = true;
                            cmbLyDoRaKhoiNhom.SelectedIndex = lstSourceLyDoRa.IndexOf(lstSourceLyDoRa.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_LY_DO)));
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
                        ngayCongNhan = obj.NGAY_CONG_NHAN;
                        #endregion

                        #region Thông tin tài sản của doanh nghiệp
                        numGiaTriTaiSan.Value = null;
                        numVonDieuLe.Value = null;
                        #endregion

                        #region Hộ khẩu thường chú
                        cmbTinhTP.SelectedIndex = lstSourceHKTinhTP.IndexOf(lstSourceHKTinhTP.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.DD_TTRU_MA_TINHTP)));
                        cmbQuanHuyen.SelectedIndex = lstSourceHKQuanHuyen.IndexOf(lstSourceHKQuanHuyen.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.DD_TTRU_MA_QUAN)));
                        cmbXaPhuong.SelectedIndex = lstSourceHKXaPhuong.IndexOf(lstSourceHKXaPhuong.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.DD_TTRU_MA_PHUONG)));
                        txtDiaChi.Text = obj.DD_TTRU_DIA_CHI;
                        #endregion

                        #region Địa chỉ hiện tại
                        cmbTinhTPHienTai.SelectedIndex = lstSourceTinhTp.IndexOf(lstSourceTinhTp.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_TINHTP)));
                        cmbQuanHuyenHienTai.SelectedIndex = lstSourceQuanHuyen.IndexOf(lstSourceQuanHuyen.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_QUAN)));
                        cmbXaPhuongHienTai.SelectedIndex = lstSourceXaPhuong.IndexOf(lstSourceXaPhuong.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_PHUONG)));
                        txtDiaChiHienTai.Text = obj.DIA_CHI;
                        #endregion

                        #region Số điện thoại email
                        txtSoCoDinh.Text = obj.SO_DTHOAI;
                        txtSoDiDong.Text = obj.SO_DDONG;
                        txtEmail.Text = obj.EMAIL;
                        #endregion

                        #region Xếp loại hộ nghèo của chính quyền
                        if (dsSource.Tables["VKH_HO_NGHEO"].Rows.Count > 0)
                        {
                            giaTri = dsSource.Tables["VKH_HO_NGHEO"].Rows[0]["GIA_TRI"].ToString();
                            if (giaTri.Equals(BusinessConstant.LOAI_HO_NGHEO.NGHEO.layGiaTri()))
                                radNgheo.IsChecked = true;
                            else if (giaTri.Equals(BusinessConstant.LOAI_HO_NGHEO.CAN_NGHEO.layGiaTri()))
                                radCanNgheo.IsChecked = true;
                            else if (giaTri.Equals(BusinessConstant.LOAI_HO_NGHEO.KHONG.layGiaTri()))
                                radKhong.IsChecked = true;
                        }
                        #endregion

                        #region Nhà ở
                        if (dsSource.Tables["VKH_NHA_O_CTIET"].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsSource.Tables["VKH_NHA_O_CTIET"].Rows)
                            {
                                loai = dr["MA"].ToString();
                                giaTri = dr["GIA_TRI"].ToString();

                                //Mái nhà
                                if (loai.Equals(BusinessConstant.NHA_O_LOAI.MAI_NHA.layGiaTri()))
                                {
                                    if (giaTri.Equals(BusinessConstant.NHA_O_MAI_NHA.NGOI.layGiaTri()))
                                        radNgoi_MaiNha.IsChecked = true;

                                    else if (giaTri.Equals(BusinessConstant.NHA_O_MAI_NHA.TON.layGiaTri()))
                                        radTon_MaiNha.IsChecked = true;

                                    else if (giaTri.Equals(BusinessConstant.NHA_O_MAI_NHA.XI_MANG.layGiaTri()))
                                        radXiMang_MaiNha.IsChecked = true;

                                    else if (giaTri.Equals(BusinessConstant.NHA_O_MAI_NHA.TRANH_LA.layGiaTri()))
                                        radTranhLa_MaiNha.IsChecked = true;

                                    else if (giaTri.Equals(BusinessConstant.NHA_O_MAI_NHA.KHAC.layGiaTri()))
                                        radKhac_MaiNha.IsChecked = true;

                                }

                                //Tường
                                else if (loai.Equals(BusinessConstant.NHA_O_LOAI.TUONG.layGiaTri()))
                                {
                                    if (giaTri.Equals(BusinessConstant.NHA_O_TUONG.XAY.layGiaTri()))
                                        radXay_Tuong.IsChecked = true;

                                    else if (giaTri.Equals(BusinessConstant.NHA_O_TUONG.TON.layGiaTri()))
                                        radTon_Tuong.IsChecked = true;

                                    else if (giaTri.Equals(BusinessConstant.NHA_O_TUONG.VAN.layGiaTri()))
                                        radVan_Tuong.IsChecked = true;

                                    else if (giaTri.Equals(BusinessConstant.NHA_O_TUONG.LA.layGiaTri()))
                                        radLa_Tuong.IsChecked = true;

                                    else if (giaTri.Equals(BusinessConstant.NHA_O_TUONG.KHAC.layGiaTri()))
                                        radKhac_Tuong.IsChecked = true;
                                }

                                //Nền
                                else if (loai.Equals(BusinessConstant.NHA_O_LOAI.NEN.layGiaTri()))
                                {
                                    if (giaTri.Equals(BusinessConstant.NHA_O_NEN.GACH_MEN.layGiaTri()))
                                        radGachMen_Nen.IsChecked = true;

                                    else if (giaTri.Equals(BusinessConstant.NHA_O_NEN.GACH_TAU.layGiaTri()))
                                        radGachTau_Nen.IsChecked = true;

                                    else if (giaTri.Equals(BusinessConstant.NHA_O_NEN.XI_MANG.layGiaTri()))
                                        radXiMang_Nen.IsChecked = true;

                                    else if (giaTri.Equals(BusinessConstant.NHA_O_NEN.DAT.layGiaTri()))
                                        radDat_Nen.IsChecked = true;

                                    else if (giaTri.Equals(BusinessConstant.NHA_O_NEN.KHAC.layGiaTri()))
                                        radKhac_nen.IsChecked = true;
                                }

                                //Sân
                                else if (loai.Equals(BusinessConstant.NHA_O_LOAI.SAN.layGiaTri()))
                                {
                                    if (giaTri.Equals(BusinessConstant.NHA_O_SAN.GACH.layGiaTri()))
                                        radGach_San.IsChecked = true;

                                    else if (giaTri.Equals(BusinessConstant.NHA_O_SAN.XI_MANG.layGiaTri()))
                                        radXiMang_San.IsChecked = true;

                                    else if (giaTri.Equals(BusinessConstant.NHA_O_SAN.DAT.layGiaTri()))
                                        radDat_San.IsChecked = true;

                                    else if (giaTri.Equals(BusinessConstant.NHA_O_SAN.CAT.layGiaTri()))
                                        radCat_San.IsChecked = true;

                                    else if (giaTri.Equals(BusinessConstant.NHA_O_SAN.KHAC.layGiaTri()))
                                        radKhac_San.IsChecked = true;
                                }

                                //Nước
                                else if (loai.Equals(BusinessConstant.NHA_O_LOAI.NUOC.layGiaTri()))
                                {
                                    if (giaTri.Equals(BusinessConstant.NHA_O_NUOC.NUOC_MAY.layGiaTri()))
                                        radNuocMay_Nuoc.IsChecked = true;

                                    else if (giaTri.Equals(BusinessConstant.NHA_O_NUOC.CAY_NUOC.layGiaTri()))
                                        radCayNuoc_Nuoc.IsChecked = true;

                                    else if (giaTri.Equals(BusinessConstant.NHA_O_NUOC.BE_LU_VAI.layGiaTri()))
                                        radBe_Nuoc.IsChecked = true;
                                }

                                //Ánh sáng
                                else if (loai.Equals(BusinessConstant.NHA_O_LOAI.ANH_SANG.layGiaTri()))
                                {
                                    if (giaTri.Equals(BusinessConstant.NHA_O_ANH_SANG.DIEN.layGiaTri()))
                                        radDien_AnhSang.IsChecked = true;

                                    else if (giaTri.Equals(BusinessConstant.NHA_O_ANH_SANG.DAU.layGiaTri()))
                                        radDau_AnhSang.IsChecked = true;
                                }

                            }
                        }
                        
                        #endregion

                        #region Tổng tài sản sở hữu
                        if (dsSource.Tables["VKH_TONG_TAI_SAN"].Rows.Count > 0)
                        {
                            giaTri = dsSource.Tables["VKH_TONG_TAI_SAN"].Rows[0]["GIA_TRI"].ToString();
                            numTongTaiSan.Value = Convert.ToDouble(giaTri);
                        }
                        #endregion

                        #region Grid
                        grNguoiThuaKe.DataContext = dsSource.Tables["VKH_GDINH_NGUOI_TKE"].DefaultView;
                        grCongCuSuDungDat.DataContext = dsSource.Tables["VKH_SU_DUNG_DAT"].DefaultView;
                        grTrangThietBi.DataContext = dsSource.Tables["VKH_TRANG_THIET_BI"].DefaultView;
                        grCongCuSanXuat.DataContext = dsSource.Tables["VKH_CONG_CU_SX"].DefaultView;
                        grThuNhapTrongTrot.DataContext = dsSource.Tables["VKH_TNHAP_TRONG_TROT"].DefaultView;
                        grThuNhapChanNuoi.DataContext = dsSource.Tables["VKH_TNHAP_CHAN_NUOI"].DefaultView;
                        grThuNhapTienLuong.DataContext = dsSource.Tables["VKH_TNHAP_TIEN_LUONG"].DefaultView;
                        grThuNhapKhac.DataContext = dsSource.Tables["VKH_TNHAP_KHAC"].DefaultView;
                        grChiPhi.DataContext = dsSource.Tables["VKH_CPHI"].DefaultView;
                        grTinhHinhTinDung.DataContext = dsSource.Tables["VKH_TINH_HINH_TDUNG"].DefaultView;
                        //grDLieuHAnh.DataContext = dsSource.Tables["VKH_CKY_HANH"].DefaultView;
                        #endregion

                        lstDuLieuHinhAnh = new List<DuLieuHinhAnh>();
                        int ii=1;
                        foreach (DataRow item in dsSource.Tables["VKH_CKY_HANH"].Rows)
                        {
                            DuLieuHinhAnh duLieuHinhAnh = new DuLieuHinhAnh();
                            duLieuHinhAnh.ID = -1;
                            duLieuHinhAnh.STT = ii;
                            duLieuHinhAnh.MaLoai = item["CKHA_LOAI"].ToString();
                            duLieuHinhAnh.TenLoai = lstLoaiHinhAnhDef.Where(e => e.MaLoai == duLieuHinhAnh.MaLoai).Select(e=>e.TenLoai).FirstOrDefault();
                            duLieuHinhAnh.MaDoiTuong = item["CKHA_DTUONG"].ToString();
                            duLieuHinhAnh.TenDoiTuong = lstDoiTuongHinhAnhDef.Where(e => e.MaLoai == duLieuHinhAnh.MaLoai && e.MaDoiTuong == duLieuHinhAnh.MaDoiTuong).Select(e=>e.TenDoiTuong).FirstOrDefault();
                            duLieuHinhAnh.MaHinhAnh = item["CKHA_MA"].ToString();
                            duLieuHinhAnh.TenHinhAnh = item["CKHA_MA"].ToString();
                            duLieuHinhAnh.HieuLuc = false;
                            duLieuHinhAnh.HienThiHS = false;
                            duLieuHinhAnh.CHON = false;
                            imageData = lstChuKyHinhAnh.Where(f => f.FileName + ".jpg" == item["CKHA_DUONG_DAN"].ToString()).Select(f=>f.FileData).FirstOrDefault();
                            duLieuHinhAnh.Data = imageData;
                            //duLieuHinhAnh.ImageName = item["CKHA_DUONG_DAN"].ToString();
                            duLieuHinhAnh.ImageName = ii.ToString();
                            duLieuHinhAnh.ImageFormat = "jpg";

                            lstDuLieuHinhAnh.Add(duLieuHinhAnh);
                            ii++;
                        }
                        grDLieuHAnh.ItemsSource = null;
                        grDLieuHAnh.ItemsSource = lstDuLieuHinhAnh;

                        //foreach (BS_FileObject item in lstChuKyHinhAnh)
                        //{
                            
                        //}

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
                        txtMaKhachHang.Text = obj.MA_KHANG;
                        txtTenKhachHang.Text = obj.TEN_KHANG;
                        if (LDateTime.IsDate(obj.NGAY_THAM_GIA, "yyyyMMdd"))
                            raddtNgayCongNhan.Value = LDateTime.StringToDate(obj.NGAY_THAM_GIA, "yyyyMMdd");
                        if (LDateTime.IsDate(obj.NGAY_HET_HLUC, "yyyyMMdd"))
                        {
                            chkKhachHangHetHL.IsChecked = true;
                            bHetHLuc = true;
                            raddtNgayHetHL.Value = LDateTime.StringToDate(obj.NGAY_HET_HLUC, "yyyyMMdd");
                        }
                        cmbLyDoRaKhoiNhom.SelectedIndex = -1;
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
                        ngayCongNhan = obj.NGAY_CONG_NHAN;
                        #endregion

                        #region Thông tin tài sản của doanh nghiệp
                        numGiaTriTaiSan.Value = null;
                        numVonDieuLe.Value = null;
                        #endregion

                        #region Hộ khẩu thường chú
                        cmbTinhTP.SelectedIndex = 0;
                        cmbQuanHuyen.SelectedIndex = -1;
                        cmbXaPhuong.SelectedIndex = -1;
                        txtDiaChi.Text = "";
                        #endregion

                        #region Địa chỉ hiện tại
                        cmbTinhTPHienTai.SelectedIndex = lstSourceTinhTp.IndexOf(lstSourceTinhTp.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_TINHTP)));
                        cmbQuanHuyenHienTai.SelectedIndex = lstSourceQuanHuyen.IndexOf(lstSourceQuanHuyen.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_QUAN)));
                        cmbXaPhuongHienTai.SelectedIndex = lstSourceXaPhuong.IndexOf(lstSourceXaPhuong.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_PHUONG)));
                        txtDiaChiHienTai.Text = obj.DIA_CHI;
                        #endregion

                        #region Số điện thoại email
                        txtSoCoDinh.Text = obj.SO_DTHOAI;
                        txtSoDiDong.Text = obj.SO_DDONG;
                        txtEmail.Text = obj.EMAIL;
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
                        if (LDateTime.IsDate(obj.NGAY_THAM_GIA, "yyyyMMdd"))
                            raddtNgayCongNhan.Value = LDateTime.StringToDate(obj.NGAY_THAM_GIA, "yyyyMMdd");
                        if (LDateTime.IsDate(obj.NGAY_HET_HLUC, "yyyyMMdd"))
                        {
                            chkKhachHangHetHL.IsChecked = true;
                            raddtNgayHetHL.Value = LDateTime.StringToDate(obj.NGAY_HET_HLUC, "yyyyMMdd");
                        }
                        cmbLyDoRaKhoiNhom.SelectedIndex = -1;
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
                        #endregion

                        #region Thông tin tài sản của doanh nghiệp
                        numGiaTriTaiSan.Value = Convert.ToDouble(obj.TONG_TAI_SAN);
                        numVonDieuLe.Value = Convert.ToDouble(obj.VON_DIEU_LE);
                        #endregion

                        #region Hộ khẩu thường chú
                        cmbTinhTP.SelectedIndex = 0;
                        cmbQuanHuyen.SelectedIndex = -1;
                        cmbXaPhuong.SelectedIndex = -1;
                        txtDiaChi.Text = "";
                        #endregion

                        #region Địa chỉ hiện tại
                        cmbTinhTPHienTai.SelectedIndex = lstSourceTinhTp.IndexOf(lstSourceTinhTp.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_TINHTP)));
                        cmbQuanHuyenHienTai.SelectedIndex = lstSourceQuanHuyen.IndexOf(lstSourceQuanHuyen.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_QUAN)));
                        cmbXaPhuongHienTai.SelectedIndex = lstSourceXaPhuong.IndexOf(lstSourceXaPhuong.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.MA_PHUONG)));
                        txtDiaChiHienTai.Text = obj.DIA_CHI;
                        #endregion

                        #region Số điện thoại email
                        txtSoCoDinh.Text = obj.SO_DTHOAI;
                        txtSoDiDong.Text = obj.SO_DDONG;
                        txtEmail.Text = obj.EMAIL;
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
            dsSource = processKhachHang.getViewKhachHang(id);
            if (dsSource != null && dsSource.Tables.Count > 0)
            {
                grCongCuSuDungDat.DataContext = dsSource.Tables["VKH_SU_DUNG_DAT"].DefaultView;
                grTrangThietBi.DataContext = dsSource.Tables["VKH_TRANG_THIET_BI"].DefaultView;
                grCongCuSanXuat.DataContext = dsSource.Tables["VKH_CONG_CU_SX"].DefaultView;
                grThuNhapTrongTrot.DataContext = dsSource.Tables["VKH_TNHAP_TRONG_TROT"].DefaultView;
                grThuNhapChanNuoi.DataContext = dsSource.Tables["VKH_TNHAP_CHAN_NUOI"].DefaultView;
                grThuNhapTienLuong.DataContext = dsSource.Tables["VKH_TNHAP_TIEN_LUONG"].DefaultView;
                grThuNhapKhac.DataContext = dsSource.Tables["VKH_TNHAP_KHAC"].DefaultView;
                grChiPhi.DataContext = dsSource.Tables["VKH_CPHI"].DefaultView;
                grTinhHinhTinDung.DataContext = dsSource.Tables["VKH_TINH_HINH_TDUNG"].DefaultView;
                //grDLieuHAnh.DataContext = dsSource.Tables["VKH_CKY_HANH"].DefaultView;
            }
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
                lblNgayCongNhan.Content = "Ngày công nhận:";
                raddtNgayCongNhan.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                chkKhachHangHetHL.IsChecked = false;
                raddtNgayThanhLapTC.Value = null;
                raddtNgayHetHL.Value = null;
                cmbLyDoRaKhoiNhom.SelectedIndex = -1;
                raddtNgaySinh.Value = null;
                cmbGioiTinh.SelectedIndex = 0;
                lblSoCMND.Content = "Số CMND:";
                txtSoCMND.Text = "";
                raddtNgayCap.Value = null;
                txtNoiCap.Text = "";
                cmbDanToc.SelectedIndex = 0;
                txtSoHoKhau.Text = "";
                cmbTinhTrangHonNhan.SelectedIndex = 0;
                cmbLoaiHinhToChuc.SelectedIndex = -1;
                cmbNganhKinhTe.SelectedIndex = -1;
                #endregion

                #region Thông tin tài sản của doanh nghiệp
                numGiaTriTaiSan.Value = 0;
                numVonDieuLe.Value = 0;
                #endregion

                #region Hộ khẩu thường chú
                //cmbTinhTP.SelectedIndex = 0;
                cmbTinhTP.SelectedIndex = lstSourceHKTinhTP.IndexOf(lstSourceHKTinhTP.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TinhTPDonViGiaoDich)));
                cmbQuanHuyen.SelectedIndex = -1;
                cmbXaPhuong.SelectedIndex = -1;
                #endregion

                #region Địa chỉ hiện tại
                //cmbTinhTPHienTai.SelectedIndex = 0;
                cmbTinhTPHienTai.SelectedIndex = lstSourceHKTinhTP.IndexOf(lstSourceHKTinhTP.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TinhTPDonViGiaoDich)));
                cmbQuanHuyenHienTai.SelectedIndex = -1;
                cmbXaPhuongHienTai.SelectedIndex = -1;
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
                raddtNgayCongNhan.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                chkKhachHangHetHL.IsChecked = false;
                raddtNgayThanhLapTC.Value = null;
                raddtNgayHetHL.Value = null;
                cmbLyDoRaKhoiNhom.SelectedIndex = -1;
                raddtNgaySinh.Value = null;
                cmbGioiTinh.SelectedIndex = 0;
                txtSoCMND.Text = "";
                raddtNgayCap.Value = null;
                txtNoiCap.Text = "";
                cmbDanToc.SelectedIndex = 0;
                txtSoHoKhau.Text = "";
                cmbTinhTrangHonNhan.SelectedIndex = 0;
                cmbLoaiHinhToChuc.SelectedIndex = -1;
                cmbNganhKinhTe.SelectedIndex = -1;
                #endregion

                #region Thông tin tài sản của doanh nghiệp
                numGiaTriTaiSan.Value = 0;
                numVonDieuLe.Value = 0;
                #endregion

                #region Hộ khẩu thường chú
                //cmbTinhTP.SelectedIndex = 0;
                cmbTinhTP.SelectedIndex = lstSourceHKTinhTP.IndexOf(lstSourceHKTinhTP.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TinhTPDonViGiaoDich)));
                cmbQuanHuyen.SelectedIndex = -1;
                cmbXaPhuong.SelectedIndex = -1;
                #endregion

                #region Địa chỉ hiện tại
                //cmbTinhTPHienTai.SelectedIndex = 0;
                cmbTinhTPHienTai.SelectedIndex = lstSourceHKTinhTP.IndexOf(lstSourceHKTinhTP.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TinhTPDonViGiaoDich)));
                cmbQuanHuyenHienTai.SelectedIndex = -1;
                cmbXaPhuongHienTai.SelectedIndex = -1;
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

            #region Khách hàng tổ chức
            else if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.DNGHIEP.layGiaTri()))
            {
                CommonFunction.SetWindowTitle(this, DatabaseConstant.Function.KH_TO_CHUC);

                #region Thông tin chung
                cmbLoaiKhachHang.SelectedIndex = lstSourceLoaiKH.IndexOf(lstSourceLoaiKH.FirstOrDefault(i => i.KeywordStrings.First().Equals(loaiKhachHang)));
                txtMaKhachHang.Text = "";
                txtTenKhachHang.Text = "";
                lblNgayCongNhan.Content = "Ngày tham gia TC";
                raddtNgayCongNhan.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                chkKhachHangHetHL.IsChecked = false;
                raddtNgayThanhLapTC.Value = null;
                raddtNgayHetHL.Value = null;
                cmbLyDoRaKhoiNhom.SelectedIndex = -1;
                raddtNgaySinh.Value = null;
                cmbGioiTinh.SelectedIndex = 0;
                lblSoCMND.Content = "Số ĐKKD";
                txtSoCMND.Text = "";
                raddtNgayCap.Value = null;
                txtNoiCap.Text = "";
                cmbDanToc.SelectedIndex = 0;
                txtSoHoKhau.Text = "";
                cmbTinhTrangHonNhan.SelectedIndex = 0;
                cmbLoaiHinhToChuc.SelectedIndex = -1;
                cmbNganhKinhTe.SelectedIndex = -1;
                #endregion

                #region Thông tin tài sản của doanh nghiệp
                numGiaTriTaiSan.Value = 0;
                numVonDieuLe.Value = 0;
                #endregion

                #region Hộ khẩu thường chú
                //cmbTinhTP.SelectedIndex = 0;
                cmbTinhTP.SelectedIndex = lstSourceHKTinhTP.IndexOf(lstSourceHKTinhTP.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TinhTPDonViGiaoDich)));
                cmbQuanHuyen.SelectedIndex = -1;
                cmbXaPhuong.SelectedIndex = -1;
                #endregion

                #region Địa chỉ hiện tại
                //cmbTinhTPHienTai.SelectedIndex = 0;
                cmbTinhTPHienTai.SelectedIndex = lstSourceHKTinhTP.IndexOf(lstSourceHKTinhTP.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TinhTPDonViGiaoDich)));
                cmbQuanHuyenHienTai.SelectedIndex = -1;
                cmbXaPhuongHienTai.SelectedIndex = -1;
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
                    if (raddtNgayCongNhan.Value == null || !LDateTime.IsDate(raddtNgayCongNhan.Text, "dd/MM/yyyy"))
                    {
                        CommonFunction.ThongBaoChuaNhap(lblNgayCongNhan.Content.ToString());
                        raddtNgayCongNhan.Focus();
                        return false;
                    }
                    if (raddtNgaySinh.Value == null || !LDateTime.IsDate(raddtNgaySinh.Text,"dd/MM/yyyy"))
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
                            LMessage.ShowMessage("Ngày sinh không hợp lệ", LMessage.MessageBoxType.Warning);
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
                    if (raddtNgayCap.Value == null || raddtNgayCap.Text.Equals("__/__/____"))
                    {
                        CommonFunction.ThongBaoChuaNhap(lblNgayCap.Content.ToString());
                        raddtNgayCap.Focus();
                        return false;
                    }
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
                        if (cmbLyDoRaKhoiNhom.SelectedIndex < 0)
                        {
                            CommonFunction.ThongBaoChuaChon(lblLyDoRaKhoiNhom.Content.ToString());
                            cmbLyDoRaKhoiNhom.Focus();
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
                    if (raddtNgaySinh.Value == null || raddtNgaySinh.Text.Equals("__/__/____"))
                    {
                        CommonFunction.ThongBaoChuaNhap(lblNgaySinh.Content.ToString());
                        raddtNgaySinh.Focus();
                        return false;
                    }
                    if (raddtNgaySinh.Value == null || !LDateTime.IsDate(raddtNgaySinh.Text, "dd/MM/yyyy"))
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

                        DateTime ngayLapViecHienTai = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai,"yyyyMMdd");
                        DateTime ngaySinh = Convert.ToDateTime(raddtNgaySinh.Value);
                        DateTime ngaySinh1 = Convert.ToDateTime(raddtNgaySinh.Value);

                        if (ngaySinh.AddYears(canDuoi) > ngayLapViecHienTai || ngaySinh1.AddYears(canTren) < ngayLapViecHienTai)
                        {
                            LMessage.ShowMessage("Ngày sinh không hợp lệ", LMessage.MessageBoxType.Warning);
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
                    if (raddtNgayCap.Value == null || raddtNgayCap.Text.Equals("__/__/____"))
                    {
                        CommonFunction.ThongBaoChuaNhap(lblNgayCap.Content.ToString());
                        raddtNgayCap.Focus();
                        return false;
                    }
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

                    #region Địa chỉ hiện tại
                    if (txtDiaChiHienTai.Text.IsNullOrEmptyOrSpace())
                    {
                        CommonFunction.ThongBaoChuaNhap(lblDiaChiHienTai.Content.ToString());
                        tbiThongTinLienHe.IsSelected = true;
                        txtDiaChiHienTai.Focus();
                        return false;
                    }
                    #endregion
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
                tbiDieuKienKinhTe.Visibility = System.Windows.Visibility.Visible;
                tbiThuNhapChiPhi.Visibility = System.Windows.Visibility.Visible;
                tbiTinhHinhTinDung.Visibility = System.Windows.Visibility.Visible;
                tbiThongTinVonVay.Visibility = System.Windows.Visibility.Visible;
                tbiThongTinNguoiDaiDien.Visibility = System.Windows.Visibility.Collapsed;
                tbiChuKyHinhAnh.Visibility = System.Windows.Visibility.Visible;
                tbiKiemSoat.Visibility = System.Windows.Visibility.Visible;

                //Group
                grbTaiSanDoanhNghiep.Visibility = System.Windows.Visibility.Collapsed;
                grbHoKhauThuongChu.Visibility = System.Windows.Visibility.Visible;


                lblTempNgayThanhLapTC.Visibility = System.Windows.Visibility.Collapsed;
                lblNgayThanhLapTC.Visibility = System.Windows.Visibility.Collapsed;
                raddtNgayThanhLapTC.Visibility = System.Windows.Visibility.Collapsed;
                dtpNgayThanhLapTC.Visibility = System.Windows.Visibility.Collapsed;

                lblLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Visible;
                lblStarLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Visible;
                cmbLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Visible;

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

                lblTempLoaiHinhToChuc.Visibility = System.Windows.Visibility.Collapsed;
                lblLoaiHinhToChuc.Visibility = System.Windows.Visibility.Collapsed;
                lblStarLoaiHinhToChuc.Visibility = System.Windows.Visibility.Collapsed;
                cmbLoaiHinhToChuc.Visibility = System.Windows.Visibility.Collapsed;
                lblNganhKinhTe.Visibility = System.Windows.Visibility.Collapsed;
                lblStarNganhKinhTe.Visibility = System.Windows.Visibility.Collapsed;
                cmbNganhKinhTe.Visibility = System.Windows.Visibility.Collapsed;


                //Khác
                if (chkKhachHangHetHL.IsChecked == false)
                {
                    lblStarNgayHetHL.Visibility = System.Windows.Visibility.Collapsed;
                    lblStarLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    lblStarNgayHetHL.Visibility = System.Windows.Visibility.Visible;
                    lblStarLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Visible;
                }


            }
            #endregion

            #region Khách hàng cá nhân
            else if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.CNHAN.layGiaTri()))
            {
                //Tab
                tbiThongTinChung.Visibility = System.Windows.Visibility.Visible;
                tbiThongTinLienHe.Visibility = System.Windows.Visibility.Visible;
                tbiNguoiThuaKe.Visibility = System.Windows.Visibility.Collapsed;
                tbiDieuKienKinhTe.Visibility = System.Windows.Visibility.Collapsed;
                tbiThuNhapChiPhi.Visibility = System.Windows.Visibility.Collapsed;
                tbiTinhHinhTinDung.Visibility = System.Windows.Visibility.Collapsed;
                tbiThongTinVonVay.Visibility = System.Windows.Visibility.Collapsed;
                tbiThongTinNguoiDaiDien.Visibility = System.Windows.Visibility.Collapsed;
                tbiChuKyHinhAnh.Visibility = System.Windows.Visibility.Visible;
                tbiKiemSoat.Visibility = System.Windows.Visibility.Visible;

                //Group
                grbTaiSanDoanhNghiep.Visibility = System.Windows.Visibility.Collapsed;
                grbHoKhauThuongChu.Visibility = System.Windows.Visibility.Collapsed;


                lblTempNgayThanhLapTC.Visibility = System.Windows.Visibility.Collapsed;
                lblNgayThanhLapTC.Visibility = System.Windows.Visibility.Collapsed;
                raddtNgayThanhLapTC.Visibility = System.Windows.Visibility.Collapsed;
                dtpNgayThanhLapTC.Visibility = System.Windows.Visibility.Collapsed;

                lblLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Collapsed;
                lblStarLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Collapsed;
                cmbLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Collapsed;

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

                lblTempLoaiHinhToChuc.Visibility = System.Windows.Visibility.Collapsed;
                lblLoaiHinhToChuc.Visibility = System.Windows.Visibility.Collapsed;
                lblStarLoaiHinhToChuc.Visibility = System.Windows.Visibility.Collapsed;
                cmbLoaiHinhToChuc.Visibility = System.Windows.Visibility.Collapsed;
                lblNganhKinhTe.Visibility = System.Windows.Visibility.Collapsed;
                lblStarNganhKinhTe.Visibility = System.Windows.Visibility.Collapsed;
                cmbNganhKinhTe.Visibility = System.Windows.Visibility.Collapsed;


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

            #region Khách hàng tổ chức
            else if (loaiKhachHang.Equals(BusinessConstant.LoaiKhachHang.DNGHIEP.layGiaTri()))
            {
                //Tab
                tbiThongTinChung.Visibility = System.Windows.Visibility.Visible;
                tbiThongTinLienHe.Visibility = System.Windows.Visibility.Visible;
                tbiNguoiThuaKe.Visibility = System.Windows.Visibility.Collapsed;
                tbiDieuKienKinhTe.Visibility = System.Windows.Visibility.Collapsed;
                tbiThuNhapChiPhi.Visibility = System.Windows.Visibility.Collapsed;
                tbiTinhHinhTinDung.Visibility = System.Windows.Visibility.Collapsed;
                tbiThongTinVonVay.Visibility = System.Windows.Visibility.Collapsed;
                tbiThongTinNguoiDaiDien.Visibility = System.Windows.Visibility.Visible;
                tbiChuKyHinhAnh.Visibility = System.Windows.Visibility.Visible;
                tbiKiemSoat.Visibility = System.Windows.Visibility.Visible;

                //Group
                grbTaiSanDoanhNghiep.Visibility = System.Windows.Visibility.Visible;
                grbHoKhauThuongChu.Visibility = System.Windows.Visibility.Collapsed;

                lblTempNgayThanhLapTC.Visibility = System.Windows.Visibility.Visible;
                lblNgayThanhLapTC.Visibility = System.Windows.Visibility.Visible;
                raddtNgayThanhLapTC.Visibility = System.Windows.Visibility.Visible;
                dtpNgayThanhLapTC.Visibility = System.Windows.Visibility.Visible;

                lblLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Collapsed;
                lblStarLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Collapsed;
                cmbLyDoRaKhoiNhom.Visibility = System.Windows.Visibility.Collapsed;

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

                lblTempLoaiHinhToChuc.Visibility = System.Windows.Visibility.Visible;
                lblLoaiHinhToChuc.Visibility = System.Windows.Visibility.Visible;
                lblStarLoaiHinhToChuc.Visibility = System.Windows.Visibility.Visible;
                cmbLoaiHinhToChuc.Visibility = System.Windows.Visibility.Visible;
                lblNganhKinhTe.Visibility = System.Windows.Visibility.Visible;
                lblStarNganhKinhTe.Visibility = System.Windows.Visibility.Visible;
                cmbNganhKinhTe.Visibility = System.Windows.Visibility.Visible;


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
                raddtNgayCongNhan.IsEnabled = true;
                dtpNgayCongNhan.IsEnabled = true;
                chkKhachHangHetHL.IsEnabled = true;
                raddtNgayThanhLapTC.IsEnabled = true;
                dtpNgayThanhLapTC.IsEnabled = true;
                raddtNgayHetHL.IsEnabled = true;
                dtpNgayHetHL.IsEnabled = true;
                cmbLyDoRaKhoiNhom.IsEnabled = true;
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

                //Khác
                if (chkKhachHangHetHL.IsChecked == true)
                {
                    raddtNgayHetHL.IsEnabled = true;
                    cmbLyDoRaKhoiNhom.IsEnabled = true;
                }
                else
                {
                    raddtNgayHetHL.IsEnabled = false;
                    cmbLyDoRaKhoiNhom.IsEnabled = false;
                }
                #endregion

                #region Tài sản doanh nghiệp
                numGiaTriTaiSan.IsEnabled = true;
                numVonDieuLe.IsEnabled = true;
                #endregion

                #region Hộ khẩu thường chú
                cmbTinhTP.IsEnabled = true;
                cmbQuanHuyen.IsEnabled = true;
                cmbXaPhuong.IsEnabled = true;
                txtDiaChi.IsEnabled = true;
                #endregion

                #region Địa chỉ hiện tại
                cmbTinhTPHienTai.IsEnabled = true;
                cmbQuanHuyenHienTai.IsEnabled = true;
                cmbXaPhuongHienTai.IsEnabled = true;
                txtDiaChiHienTai.IsEnabled = true;
                #endregion

                #region SĐT và email
                txtSoCoDinh.IsEnabled = true;
                txtSoDiDong.IsEnabled = true;
                txtEmail.IsEnabled = true;
                #endregion

                #region Thông tin gia đình và người thừa kế
                btnAddNguoiThuaKe.IsEnabled = true;
                btnModifyNguoiThuaKe.IsEnabled = true;
                btnDeleteNguoiThuaKe.IsEnabled = true;                
                #endregion

                #region Xếp loại hộ nghèo của chính quyền
                radNgheo.IsEnabled = true;
                radCanNgheo.IsEnabled = true;
                radKhong.IsEnabled = true;
                #endregion

                #region Nhà ở
                //Mái nhà
                radNgoi_MaiNha.IsEnabled = true;
                radTon_MaiNha.IsEnabled = true;
                radXiMang_MaiNha.IsEnabled = true;
                radTranhLa_MaiNha.IsEnabled = true;
                radKhac_MaiNha.IsEnabled = true;

                //Tường
                radXay_Tuong.IsEnabled = true;
                radTon_Tuong.IsEnabled = true;
                radVan_Tuong.IsEnabled = true;
                radLa_Tuong.IsEnabled = true;
                radKhac_Tuong.IsEnabled = true;

                //Nền
                radGachMen_Nen.IsEnabled = true;
                radGachTau_Nen.IsEnabled = true;
                radXiMang_Nen.IsEnabled = true;
                radDat_Nen.IsEnabled = true;
                radKhac_nen.IsEnabled = true;

                //Sân
                radGach_San.IsEnabled = true;
                radXiMang_San.IsEnabled = true;
                radDat_San.IsEnabled = true;
                radCat_San.IsEnabled = true;
                radKhac_San.IsEnabled = true;

                //Nước
                radNuocMay_Nuoc.IsEnabled = true;
                radCayNuoc_Nuoc.IsEnabled = true;
                radBe_Nuoc.IsEnabled = true;

                //Ánh sáng
                radDien_AnhSang.IsEnabled = true;
                radDau_AnhSang.IsEnabled = true;
                #endregion

                #region Tổng tài sản
                numTongTaiSan.IsEnabled = true;
                #endregion

                #region Grid
                grCongCuSuDungDat.IsEnabled = true;
                grTrangThietBi.IsEnabled = true;
                grCongCuSanXuat.IsEnabled = true;
                grThuNhapTrongTrot.IsEnabled = true;
                grThuNhapChanNuoi.IsEnabled = true;
                grThuNhapTienLuong.IsEnabled = true;
                grThuNhapKhac.IsEnabled = true;
                grChiPhi.IsEnabled = true;
                grTinhHinhTinDung.IsEnabled = true;                
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
                raddtNgayCongNhan.IsEnabled = true;
                dtpNgayCongNhan.IsEnabled = true;
                chkKhachHangHetHL.IsEnabled = true;
                raddtNgayThanhLapTC.IsEnabled = true;
                dtpNgayThanhLapTC.IsEnabled = true;
                raddtNgayHetHL.IsEnabled = true;
                dtpNgayHetHL.IsEnabled = true;
                cmbLyDoRaKhoiNhom.IsEnabled = true;
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

                //Khác
                if (chkKhachHangHetHL.IsChecked == true)
                {
                    raddtNgayHetHL.IsEnabled = true;
                    cmbLyDoRaKhoiNhom.IsEnabled = true;
                }
                else
                {
                    raddtNgayHetHL.IsEnabled = false;
                    cmbLyDoRaKhoiNhom.IsEnabled = false;
                }

                #endregion

                #region Tài sản doanh nghiệp
                numGiaTriTaiSan.IsEnabled = true;
                numVonDieuLe.IsEnabled = true;
                #endregion

                #region Hộ khẩu thường chú
                cmbTinhTP.IsEnabled = true;
                cmbQuanHuyen.IsEnabled = true;
                cmbXaPhuong.IsEnabled = true;
                txtDiaChi.IsEnabled = true;
                #endregion

                #region Địa chỉ hiện tại
                cmbTinhTPHienTai.IsEnabled = true;
                cmbQuanHuyenHienTai.IsEnabled = true;
                cmbXaPhuongHienTai.IsEnabled = true;
                txtDiaChiHienTai.IsEnabled = true;
                #endregion

                #region SĐT và email
                txtSoCoDinh.IsEnabled = true;
                txtSoDiDong.IsEnabled = true;
                txtEmail.IsEnabled = true;
                #endregion

                #region Thông tin gia đình và người thừa kế
                btnAddNguoiThuaKe.IsEnabled = true;
                btnModifyNguoiThuaKe.IsEnabled = true;
                btnDeleteNguoiThuaKe.IsEnabled = true;
                #endregion

                #region Xếp loại hộ nghèo của chính quyền
                radNgheo.IsEnabled = true;
                radCanNgheo.IsEnabled = true;
                radKhong.IsEnabled = true;
                #endregion

                #region Nhà ở
                //Mái nhà
                radNgoi_MaiNha.IsEnabled = true;
                radTon_MaiNha.IsEnabled = true;
                radXiMang_MaiNha.IsEnabled = true;
                radTranhLa_MaiNha.IsEnabled = true;
                radKhac_MaiNha.IsEnabled = true;

                //Tường
                radXay_Tuong.IsEnabled = true;
                radTon_Tuong.IsEnabled = true;
                radVan_Tuong.IsEnabled = true;
                radLa_Tuong.IsEnabled = true;
                radKhac_Tuong.IsEnabled = true;

                //Nền
                radGachMen_Nen.IsEnabled = true;
                radGachTau_Nen.IsEnabled = true;
                radXiMang_Nen.IsEnabled = true;
                radDat_Nen.IsEnabled = true;
                radKhac_nen.IsEnabled = true;

                //Sân
                radGach_San.IsEnabled = true;
                radXiMang_San.IsEnabled = true;
                radDat_San.IsEnabled = true;
                radCat_San.IsEnabled = true;
                radKhac_San.IsEnabled = true;

                //Nước
                radNuocMay_Nuoc.IsEnabled = true;
                radCayNuoc_Nuoc.IsEnabled = true;
                radBe_Nuoc.IsEnabled = true;

                //Ánh sáng
                radDien_AnhSang.IsEnabled = true;
                radDau_AnhSang.IsEnabled = true;
                #endregion

                #region Tổng tài sản
                numTongTaiSan.IsEnabled = true;
                #endregion

                #region Grid
                grCongCuSuDungDat.IsEnabled = true;
                grTrangThietBi.IsEnabled = true;
                grCongCuSanXuat.IsEnabled = true;
                grThuNhapTrongTrot.IsEnabled = true;
                grThuNhapChanNuoi.IsEnabled = true;
                grThuNhapTienLuong.IsEnabled = true;
                grThuNhapKhac.IsEnabled = true;
                grChiPhi.IsEnabled = true;
                grTinhHinhTinDung.IsEnabled = true;
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
                raddtNgayCongNhan.IsEnabled = false;
                dtpNgayCongNhan.IsEnabled = false;
                chkKhachHangHetHL.IsEnabled = false;
                raddtNgayThanhLapTC.IsEnabled = false;
                dtpNgayThanhLapTC.IsEnabled = false;
                raddtNgayHetHL.IsEnabled = false;
                dtpNgayHetHL.IsEnabled = false;
                cmbLyDoRaKhoiNhom.IsEnabled = false;
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
                #endregion

                #region Tài sản doanh nghiệp
                numGiaTriTaiSan.IsEnabled = false;
                numVonDieuLe.IsEnabled = false;
                #endregion

                #region Hộ khẩu thường chú
                cmbTinhTP.IsEnabled = false;
                cmbQuanHuyen.IsEnabled = false;
                cmbXaPhuong.IsEnabled = false;
                txtDiaChi.IsEnabled = false;
                #endregion

                #region Địa chỉ hiện tại
                cmbTinhTPHienTai.IsEnabled = false;
                cmbQuanHuyenHienTai.IsEnabled = false;
                cmbXaPhuongHienTai.IsEnabled = false;
                txtDiaChiHienTai.IsEnabled = false;
                #endregion

                #region SĐT và email
                txtSoCoDinh.IsEnabled = false;
                txtSoDiDong.IsEnabled = false;
                txtEmail.IsEnabled = false;
                #endregion

                #region Thông tin gia đình và người thừa kế
                btnAddNguoiThuaKe.IsEnabled = false;
                btnModifyNguoiThuaKe.IsEnabled = false;
                btnDeleteNguoiThuaKe.IsEnabled = false;
                #endregion

                #region Xếp loại hộ nghèo của chính quyền
                radNgheo.IsEnabled = false;
                radCanNgheo.IsEnabled = false;
                radKhong.IsEnabled = false;
                #endregion

                #region Nhà ở
                //Mái nhà
                radNgoi_MaiNha.IsEnabled = false;
                radTon_MaiNha.IsEnabled = false;
                radXiMang_MaiNha.IsEnabled = false;
                radTranhLa_MaiNha.IsEnabled = false;
                radKhac_MaiNha.IsEnabled = false;

                //Tường
                radXay_Tuong.IsEnabled = false;
                radTon_Tuong.IsEnabled = false;
                radVan_Tuong.IsEnabled = false;
                radLa_Tuong.IsEnabled = false;
                radKhac_Tuong.IsEnabled = false;

                //Nền
                radGachMen_Nen.IsEnabled = false;
                radGachTau_Nen.IsEnabled = false;
                radXiMang_Nen.IsEnabled = false;
                radDat_Nen.IsEnabled = false;
                radKhac_nen.IsEnabled = false;

                //Sân
                radGach_San.IsEnabled = false;
                radXiMang_San.IsEnabled = false;
                radDat_San.IsEnabled = false;
                radCat_San.IsEnabled = false;
                radKhac_San.IsEnabled = false;

                //Nước
                radNuocMay_Nuoc.IsEnabled = false;
                radCayNuoc_Nuoc.IsEnabled = false;
                radBe_Nuoc.IsEnabled = false;

                //Ánh sáng
                radDien_AnhSang.IsEnabled = false;
                radDau_AnhSang.IsEnabled = false;
                #endregion

                #region Tổng tài sản
                numTongTaiSan.IsEnabled = false;
                #endregion

                #region Grid
                grCongCuSuDungDat.IsEnabled = false;
                grTrangThietBi.IsEnabled = false;
                grCongCuSanXuat.IsEnabled = false;
                grThuNhapTrongTrot.IsEnabled = false;
                grThuNhapChanNuoi.IsEnabled = false;
                grThuNhapTienLuong.IsEnabled = false;
                grThuNhapKhac.IsEnabled = false;
                grChiPhi.IsEnabled = false;
                grTinhHinhTinDung.IsEnabled = false;
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

                ret = processKhachHang.KhachHang01(DatabaseConstant.Action.THEM, ref obj, ref dsSource, ref lstChuKyHinhAnh, ref listClientResponseDetail);
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
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

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

                ret = processKhachHang.KhachHang01(DatabaseConstant.Action.SUA, ref obj, ref dsSource, ref lstChuKyHinhAnh, ref listClientResponseDetail);
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
                ret = processKhachHang.KhachHang01(action, ref obj, ref dsSource, ref lstChuKyHinhAnh, ref listClientResponseDetail);
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
                ret = processKhachHang.KhachHang01(action, ref obj, ref dsSource, ref lstChuKyHinhAnh, ref listClientResponseDetail);
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
                ret = processKhachHang.KhachHang01(action, ref obj, ref dsSource, ref lstChuKyHinhAnh, ref listClientResponseDetail);
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
                ret = processKhachHang.KhachHang01(action, ref obj, ref dsSource, ref lstChuKyHinhAnh, ref listClientResponseDetail);
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