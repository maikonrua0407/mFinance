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
using Utilities.Common;
using PresentationWPF.CustomControl;
using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using System.Data;
using PresentationWPF.KhachHang.KhachHang.Popup;
using System.Reflection;
using Telerik.Windows.Controls;
using Presentation.Process.KhachHangServiceRef;
using System.Collections;
using PresentationWPF.BaoCao.DungChung;
using System.Text.RegularExpressions;

namespace PresentationWPF.KhachHang.KhachHang
{
    /// <summary>
    /// Interaction logic for ucKhachHangThanhVien.xaml
    /// </summary>
    public partial class ucKhachHangThanhVien : UserControl
    {
        #region Khai bao
        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        public event EventHandler OnSavingCompleted;

        public static string formCase = null;

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
        List<AutoCompleteEntry> lstSourceNguonVon = new List<AutoCompleteEntry>();        

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

        //DataSet chứa thông tin của các thông tin liên quan tới khách hàng nhập thông qua popup, grid
        private DataSet dsSourceKHang = new DataSet();

        private string gioi_tinh = DatabaseConstant.DanhMuc.GIOI_TINH.getValue();
        private string dan_toc = DatabaseConstant.DanhMuc.DAN_TOC.getValue();
        private string tinh_trang_hon_nhan = DatabaseConstant.DanhMuc.TINH_TRANG_HON_NHAN.getValue();
        private string trinh_do_hoc_van = DatabaseConstant.DanhMuc.TRINH_DO_HOC_VAN.getValue();
        private string vai_tro_trong_gd = DatabaseConstant.DanhMuc.VAI_TRO_TRONG_GD.getValue();
        private string loai_hinh_cong_tac = "LOAI_HINH_CONG_TAC";
        private string thoi_gian_cong_tac = DatabaseConstant.DanhMuc.THOI_GIAN_CONG_TAC.getValue();
        private string tinh_trang_cu_tru = DatabaseConstant.DanhMuc.TINH_TRANG_CU_TRU.getValue();
        private string noi_cu_tru = "NOI_CU_TRU";
        private string thoi_gian_cu_tru = "THOI_GIAN_CU_TRU";
        private string quan_huyen_HK, xa_phuong_HK, quan_huyen_hien_tai, xa_phuong_hien_tai;

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        private int _idKhachHang = -1;
        public int idKhachHang
        {
            get { return _idKhachHang; }
            set { _idKhachHang = value; }
        }

        private string tthaiNvu = "";
        public string TthaiNvu
        {
            get { return tthaiNvu; }
            set { tthaiNvu = value; }
        }

        private string _maKhachHang = "";

        public string MaKhachHang
        {
            get { return _maKhachHang; }
            set { _maKhachHang = value; }
        }

        private DatabaseConstant.Function _function = DatabaseConstant.Function.KH_THANH_VIEN;
        public DatabaseConstant.Function function
        {
            get { return _function; }
        }

        // Thông tin phòng gd, khu vực, cụm, nhóm
        private string idPhongGD = "";
        private string idKhuVuc = "";
        private string idCum = "";
        private string idNhom = "";
        private string ngayCongNhan = "";

        decimal soDuTietKiem = 0;
        

        // Kiem tra radio button tren grid
        private bool rdoChecked = false;
        private bool bHetHLuc = false;

        #endregion

        #region Khoi tao
        public ucKhachHangThanhVien()
        {
            InitializeComponent();
            KhoiTaoChung();

            txtMaKhachHang.Focus();
            raddtNgayHetHL.Value = null;
            raddtNgayCNhat.Value = null;
            raddtNgayNhap.Value = null;
            raddtNgayCNhat.Value = null;
            raddtNgaySinh.Value = null;
            raddtNgayCap.Value = null;
            raddtNgayThanhLap.Value = null;
            lblTenNhom.Content = "";
            raddtNgayTGiaTC.Value = LDateTime.StringToDate(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai,"yyyyMMdd");
            txtNguoiLap.Text = Presentation.Process.Common.ClientInformation.TenDangNhap;

            KhoiTaoDataTable();
            beforeAddNew();
            txtTenKhachHang.Focus();
        }

        public ucKhachHangThanhVien(int id,string tthai,DatabaseConstant.Action action)
        {
            InitializeComponent();
            _idKhachHang = id;
            tthaiNvu = tthai;
            KhoiTaoChung();

            txtMaKhachHang.Focus();
            raddtNgayHetHL.Value = null;
            raddtNgayCNhat.Value = null;
            lblTenNhom.Content = "";
            raddtNgayTGiaTC.Value = LDateTime.StringToDate(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = Presentation.Process.Common.ClientInformation.TenDangNhap;
            SetFormData();
            beforeModifyFromList(action);
            txtTenKhachHang.Focus();
        }

        private void KhoiTaoChung()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KhachHang;component/KhachHang/ucKhachHangThanhVien.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindShortkey();
            LoadCombobox();
            cmbLoaiKhachHang.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiKhachHang_SelectionChanged);
            cmbHKTinhTP.SelectionChanged += new SelectionChangedEventHandler(cmbHKTinhTP_SelectionChanged);
            cmbHKQuanHuyen.SelectionChanged += new SelectionChangedEventHandler(cmbHKQuanHuyen_SelectionChanged);
            cmbTinhTPHienTai.SelectionChanged += new SelectionChangedEventHandler(cmbTinhTPHienTai_SelectionChanged);
            cmbQuanHuyenHienTai.SelectionChanged += new SelectionChangedEventHandler(cmbQuanHuyenHienTai_SelectionChanged);
            
            cmbHKTinhTP.LostFocus += new RoutedEventHandler(cmbHKTinhTP_LostFocus);
            cmbHKQuanHuyen.LostFocus += new RoutedEventHandler(cmbHKQuanHuyen_LostFocus);
            cmbTinhTPHienTai.LostFocus += new RoutedEventHandler(cmbTinhTPHienTai_LostFocus);
            cmbQuanHuyenHienTai.LostFocus += new RoutedEventHandler(cmbQuanHuyenHienTai_LostFocus);
            cmbXaPhuongHienTai.LostFocus += new RoutedEventHandler(cmbXaPhuongHienTai_LostFocus);
            cmbHKXaPhuong.LostFocus += new RoutedEventHandler(cmbHKXaPhuong_LostFocus);
            txtNhomTVien.KeyDown += new KeyEventHandler(txtNhomTVien_KeyDown);
            stackLyDoRa.Visibility = Visibility.Hidden;
            cmbLyDoRaKhoiNhom.Visibility = Visibility.Hidden;
            chkKHHieuLuc.Checked += new RoutedEventHandler(chkKHHieuLuc_Checked);
            chkKHHieuLuc.Unchecked += new RoutedEventHandler(chkKHHieuLuc_Unchecked);
        }

        /// <summary>
        /// Khoi tao datatable luu source cac popup
        /// </summary>
        private void KhoiTaoDataTable()
        {
            Presentation.Process.KhachHangProcess process = new Presentation.Process.KhachHangProcess();
            dsSourceKHang = process.getThongTinKhac("%");

            //Binding dữ liệu
            grTTCoBanHoGD.ItemsSource = dsSourceKHang.Tables["VKH_GIA_DINH"].DefaultView;
            grGiayTo.ItemsSource = dsSourceKHang.Tables["VKH_GTO_LQUAN"].DefaultView;
            grKhoanThuNhap.ItemsSource = dsSourceKHang.Tables["VKH_THU_NHAP"].DefaultView;
            grChiPhi.ItemsSource = dsSourceKHang.Tables["VKH_CHI_PHI"].DefaultView;
            grTaiSan.ItemsSource = dsSourceKHang.Tables["VKH_TAI_SAN"];
            grNTKe.ItemsSource = dsSourceKHang.Tables["VKH_NGUOI_TKE"].DefaultView;
            grKHLQuan.ItemsSource = dsSourceKHang.Tables["VKH_KHANG_LQUAN"].DefaultView;
            grTKTCTDKhac.ItemsSource = dsSourceKHang.Tables["VKH_TKHOAN_NHANG"].DefaultView;
            grDLieuHAnh.ItemsSource = dsSourceKHang.Tables["VKH_CKY_HANH"].DefaultView;
            grNguoiDaiDien.ItemsSource = dsSourceKHang.Tables["VKH_NGUOI_DDIEN"].DefaultView;
            //grXepHangTD.ItemsSource = dsSourceKHang.Tables["VKH_XHANG_TDUNG"].DefaultView;
            //grXepHangNgheo.ItemsSource = dsSourceKHang.Tables["VKH_XHANG_NGHEO"].DefaultView;
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
            else if (strTinhNang.Equals("PreviewSurvey"))
            {
                OnPreviewKhaoSatKH();
            }
            else if (strTinhNang.Equals("PreviewReview"))
            {
                OnPreviewDanhGiaKH();
            }
            else if (strTinhNang.Equals("PreviewRank"))
            {
                OnPreviewXepHangTinDung();
            } 
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                onClose();
            }
            
        }

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
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
        /// Sự kiện load 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_idKhachHang == -1)
                {
                    formCase = Presentation.Process.Common.ClientInformation.FormCase;
                    if (LString.IsNullOrEmptyOrSpace(formCase))
                    {
                        formCase = ApplicationConstant.LoaiKhachHang.THANH_VIEN.layGiaTri();
                    }
                    cmbLoaiKhachHang.SelectedIndex = lstSourceLoaiKH.IndexOf(lstSourceLoaiKH.FirstOrDefault(f => f.KeywordStrings.First().Equals(formCase)));
                    XuLyGiaoDien(formCase);
                }
                
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Khởi tạo các combobox
        /// </summary>
        private void LoadCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDK = new List<string>();

            #region Thong tin chung
            // Load du lieu Loai Khach Hang
            lstDK.Add(DatabaseConstant.DanhMuc.LOAI_KHACH_HANG.getValue());
            auto.GenAutoComboBox(ref lstSourceLoaiKH, ref cmbLoaiKhachHang, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK);

            auto.removeEntry(ref lstSourceLoaiKH, ref cmbLoaiKhachHang, BusinessConstant.LoaiKhachHang.TCTD.layGiaTri());
            auto.removeEntry(ref lstSourceLoaiKH, ref cmbLoaiKhachHang, BusinessConstant.LoaiKhachHang.VANG_LAI.layGiaTri());

            //Load du lieu combobox GioiTinh
            lstDK.Clear();
            lstDK.Add(gioi_tinh);
            auto.GenAutoComboBox(ref lstSourceGioiTinh, ref cmbGioiTinh, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK);

            //Load du lieu combobox DanToc
            lstDK.Clear();
            lstDK.Add(dan_toc);
            auto.GenAutoComboBox(ref lstSourceDanToc, ref cmbDanToc, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK);

            //Load du lieu combobx QuocTich ~ QuocGia
            auto.GenAutoComboBox(ref lstSourceQuocTich, ref cmbQuocTich, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_QUOCGIA), null, ClientInformation.MaQuocGiaBanDia);

            //Load du lieu combobox NoiCap ~ TinhThanh
            auto.GenAutoComboBox(ref lstSourceNoiCap, ref cmbNoiCap, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_TINHTP), null);

            //Load du lieu combobox TinhTrangCuTru
            lstDK.Clear();
            lstDK.Add(tinh_trang_cu_tru);
            auto.GenAutoComboBox(ref lstSourceTTCuTru, ref cmbTinhTrangCuTru, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK);

            //Load ly do ra khoi nhom
            lstDK.Clear();
            lstDK.Add(DatabaseConstant.DanhMuc.LY_DO_VAO_RA.getValue());
            auto.GenAutoComboBox(ref lstSourceLyDoRa, ref cmbLyDoRaKhoiNhom, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK, null);

            //Load loai hinh to chuc
            lstDK.Clear();
            lstDK.Add(DatabaseConstant.DanhMuc.LOAI_HINH_TO_CHUC.getValue());
            auto.GenAutoComboBox(ref lstSourceLHToChuc, ref cmbLoaiHinhTC, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK,null);

            // Load nganh kinh te
            lstDK.Clear();
            lstDK.Add(DatabaseConstant.DanhMuc.NGANH_KINH_TE.getValue());
            auto.GenAutoComboBox(ref lstSourceNganhKT, ref cmbNganhKinhTe, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK,null);

            //Nguồn vốn
            lstDK.Clear();
            lstDK.Add(ClientInformation.MaDonViGiaoDich);
            auto.GenAutoComboBox(ref lstSourceNguonVon, ref cmbNguonVon, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_NGUON_VON_DVI), lstDK);           
            #endregion

            #region Thong tin khac
            //Load dữ liệu combobox tình trạng hôn nhân
            lstDK.Clear();
            lstDK.Add(tinh_trang_hon_nhan);
            auto.GenAutoComboBox(ref lstSourceHonNhan, ref cmbHonNhan, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK, null);

            //Combobox Trình độ học vấn
            lstDK.Clear();
            lstDK.Add(trinh_do_hoc_van);
            auto.GenAutoComboBox(ref lstSourceHocVan, ref cmbHocVan, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK, null);

            //Combobox vai trò trong gia đình
            lstDK.Clear();
            lstDK.Add(vai_tro_trong_gd);
            auto.GenAutoComboBox(ref lstSourceVaiTroGD, ref cmbVaiTroGD, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK, null);

            //Combobox loại hình công tác
            lstDK.Clear();
            lstDK.Add(loai_hinh_cong_tac);
            auto.GenAutoComboBox(ref lstSourceLHinhCongTac, ref cmbLHCongTac, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK, null);

            //Combobox thời gian công tác
            lstDK.Clear();
            lstDK.Add(thoi_gian_cong_tac);
            auto.GenAutoComboBox(ref lstSourceTGianCongTac, ref cmbTGCongTac, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK, null);
            #endregion

            #region Ho Khau
            //Load du lieu combobox tinh tp
            auto.GenAutoComboBox(ref lstSourceHKTinhTP, ref cmbHKTinhTP, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_TINHTP), null);
            cmbHKTinhTP.SelectedIndex = lstSourceHKTinhTP.IndexOf(lstSourceHKTinhTP.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TinhTPDonViGiaoDich)));
            #endregion

            #region Dia chi hien tai
            //Load du lieu combobox dia chi hien tai - tinh tp
            auto.GenAutoComboBox(ref lstSourceTinhTp, ref cmbTinhTPHienTai, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_TINHTP), null);
            cmbTinhTPHienTai.SelectedIndex = lstSourceHKTinhTP.IndexOf(lstSourceHKTinhTP.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TinhTPDonViGiaoDich)));
            #endregion

            #region Nha o hien tai

            //Load du lieu combobox NoiCuTru
            lstDK.Clear();
            lstDK.Add(noi_cu_tru);
            auto.GenAutoComboBox(ref lstSourceNoiCuTru, ref cmbNoiCuTru, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK, null);

            //Load du lieu combobox ThoiGianCuTru
            lstDK.Clear();
            lstDK.Add(thoi_gian_cu_tru);
            auto.GenAutoComboBox(ref lstSourceTGCuTru, ref cmbTGianCuTru, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK, null);
            #endregion

            #region Loai hinh anh
            lstDK.Clear();
            lstDK.Add(DatabaseConstant.DanhMuc.LOAI_HINH_ANH.getValue());
            auto.GenAutoComboBox(ref lstSourceLoaiHinhAnh, ref cmbLoaiHinhAnh, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK);
            #endregion

            #region
            // Load du lieu Phong GD
            lstDK.Clear();
            lstDK.Add(ClientInformation.TenDangNhap);
            lstDK.Add(ClientInformation.MaDonViQuanLy);
            lstDK.Add(DatabaseConstant.ToChucDonVi.PGD.getValue());
            auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVI_THEOPVI.getValue(), lstDK, ClientInformation.MaDonViGiaoDich);
            cmbPhongGD.IsEnabled = false;
            //cmbPhongGD
            #endregion
        }

        /// <summary>
        /// Sự kiện thay đổi giá trị lựa chọn combobox tỉnh thành phố - hộ khẩu 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbHKTinhTP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /// <summary>
        /// Sự kiện thay đổi giá trị lựa chọn combobox quận huyện - hộ khẩu 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbHKQuanHuyen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        /// <summary>
        /// Sự kiện thay đổi giá trị lựa chọn combobox tỉnh thành phố - địa chỉ hiện tại
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTinhTPHienTai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        /// <summary>
        /// Sự kiện thay đổi giá trị lựa chọn combobox quận huyện - địa chỉ hiện tại
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbQuanHuyenHienTai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        /// <summary>
        /// Sự kiện chọn loại khác hàng
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbLoaiKhachHang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoComboBox au = new AutoComboBox();
            string maLoaiKH = au.getEntryByDisplayName(lstSourceLoaiKH, ref cmbLoaiKhachHang).KeywordStrings[0];
            XuLyGiaoDien(maLoaiKH);
            CommonFunction.SetWindowTitle(this, _function);
        }

        /// <summary>
        /// Sự kiện click button hiển thị popup khách hàng chưa phải là thành viên
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMaKhachHang_Click(object sender, RoutedEventArgs e)
        {
            HienPopUpKhachHang();
        }

        /// <summary>
        /// Hiện popup khách hàng chưa phải là thành viên
        /// </summary>
        private void HienPopUpKhachHang()
        {
            Window window = new Window();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.RenderSize = new Size(1024, 768);
            ucPopupKhachHang uc = new ucPopupKhachHang(false);
            window.Title = LLanguage.SearchResourceByKey("U.POPUP.POPUP_DS_KHACHHANG");
            window.Content = uc;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
            if (!LString.IsNullOrEmptyOrSpace(uc.id))
            {
                _idKhachHang = Convert.ToInt32(uc.id);
                SetFormData();
            }
            uc = null;
        }

        /// <summary>
        /// Hiện popup khu vực/cụm/nhóm
        /// </summary>
        private void HienPopUpNhom()
        {
            try
            {
                var process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DM_CUM.getValue(), lstDieuKien);

                SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(true, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey("U.POPUP.POPUP_DS_NHOM_KHACHHANG");
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow dr = lstPopup[0];
                    txtNhomTVien.Tag = dr[1].ToString(); // id nhom
                    txtNhomTVien.Text = dr[2].ToString(); // ma nhom
                    lblTenNhom.Content = dr[3].ToString(); // ten nhom

                    Presentation.Process.KhachHangProcess processKhachHang = new Presentation.Process.KhachHangProcess();
                    try
                    {
                        DataSet ds = processKhachHang.getThongTinCumTheoIDNhom(txtNhomTVien.Tag.ToString());
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            txtCumTVien.Text = ds.Tables[0].Rows[0]["ten_cum"].ToString();
                            txtCumTVien.Tag = ds.Tables[0].Rows[0]["ma_cum"].ToString();

                            idPhongGD = ds.Tables[0].Rows[0]["id_dvi"].ToString();
                            idKhuVuc = ds.Tables[0].Rows[0]["id_kvuc"].ToString();
                            idCum = ds.Tables[0].Rows[0]["id"].ToString();
                        }
                        else
                        {
                            txtNhomTVien.Tag = "";
                            txtNhomTVien.Text = "";
                            lblTenNhom.Content = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        txtNhomTVien.Tag = "";
                        txtNhomTVien.Text = "";
                        lblTenNhom.Content = "";
                        LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Nhấn nút "F3" hiển để hiển thị popup khu vực/cụm/nhóm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNhomTVien_Click(object sender, RoutedEventArgs e)
        {
            HienPopUpNhom();
        }

        /// <summary>
        /// Chuyển giá trị vào datatable source
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="dtAdd"></param>
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
            listLockId.Add(_idKhachHang);

            bool ret = process.UnlockData(DatabaseConstant.Module.KHTV,
                _function,
                DatabaseConstant.Table.KH_KHANG_HSO,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        private void cmbQuanHuyenHienTai_LostFocus(object sender, RoutedEventArgs e)
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
            }
            else
            {
                cmbXaPhuongHienTai.Items.Clear();
                lstSourceXaPhuong.Clear();
            }
        }

        private void cmbTinhTPHienTai_LostFocus(object sender, RoutedEventArgs e)
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
            }
            else
            {
                cmbQuanHuyenHienTai.Items.Clear();
                lstSourceQuanHuyen.Clear();
                cmbXaPhuongHienTai.Items.Clear();
                lstSourceXaPhuong.Clear();
            }
        }

        private void cmbHKQuanHuyen_LostFocus(object sender, RoutedEventArgs e)
        {
            if (cmbHKTinhTP.SelectedIndex >= 0 && cmbHKQuanHuyen.SelectedIndex >= 0)
            {
                if (lstSourceHKTinhTP.Select(i => i.DisplayName).Contains(cmbHKTinhTP.Text) && lstSourceHKQuanHuyen.Select(i => i.DisplayName).Contains(cmbHKQuanHuyen.Text))
                {
                    quan_huyen_HK = cmbHKQuanHuyen.Text;
                    AutoComboBox auto = new AutoComboBox();
                    List<string> lstDieuKien = new List<string>();
                    lstDieuKien.Add(lstSourceHKTinhTP.ElementAt(cmbHKTinhTP.SelectedIndex).KeywordStrings.ElementAt(1));
                    lstDieuKien.Add(lstSourceHKQuanHuyen.ElementAt(cmbHKQuanHuyen.SelectedIndex).KeywordStrings.ElementAt(1));
                    lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBan.XA_PHUONG.getValue());
                    cmbHKXaPhuong.Items.Clear();
                    lstSourceHKXaPhuong.Clear();
                    auto.GenAutoComboBox(ref lstSourceHKXaPhuong, ref cmbHKXaPhuong, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DIABAN), lstDieuKien);
                    if (lstSourceHKXaPhuong.Select(i => i.DisplayName).Contains(xa_phuong_HK))
                        cmbHKXaPhuong.Text = xa_phuong_HK;
                    else
                        cmbHKXaPhuong.Text = "";
                }
                else
                {
                    cmbHKXaPhuong.Text = "";
                }
            }
            else
            {
                cmbHKXaPhuong.Items.Clear();
                lstSourceHKXaPhuong.Clear();
            }
        }

        private void cmbHKTinhTP_LostFocus(object sender, RoutedEventArgs e)
        {
            if (cmbHKTinhTP.SelectedIndex >= 0)
            {
                if (lstSourceHKTinhTP.Select(i => i.DisplayName).Contains(cmbHKTinhTP.Text))
                {
                    AutoComboBox auto = new AutoComboBox();
                    //cmbHKTinhTP.SelectedIndex = lstSourceHKTinhTP.IndexOf(lstSourceHKTinhTP.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TinhTPDonViGiaoDich)));
                    List<string> lstDieuKien = new List<string>();
                    lstDieuKien.Add(lstSourceHKTinhTP.ElementAt(cmbHKTinhTP.SelectedIndex).KeywordStrings.ElementAt(1));
                    lstDieuKien.Add(null);
                    lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBan.HUYEN_QUAN.getValue());
                    cmbHKQuanHuyen.Items.Clear();
                    lstSourceHKQuanHuyen.Clear();
                    auto.GenAutoComboBox(ref lstSourceHKQuanHuyen, ref cmbHKQuanHuyen, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DIABAN), lstDieuKien);
                    if (lstSourceHKQuanHuyen.Select(i => i.DisplayName).Contains(quan_huyen_HK))
                        cmbHKQuanHuyen.Text = quan_huyen_HK;
                    else
                        cmbHKQuanHuyen.Text = "";
                }
                else
                {
                    cmbHKQuanHuyen.Text = "";
                }
            }
            else
            {
                cmbHKQuanHuyen.Items.Clear();
                lstSourceHKQuanHuyen.Clear();
                cmbHKXaPhuong.Items.Clear();
                lstSourceHKXaPhuong.Clear();
            }
        }

        private void cmbHKXaPhuong_LostFocus(object sender, RoutedEventArgs e)
        {
            xa_phuong_HK = cmbHKXaPhuong.Text;
        }

        private void cmbXaPhuongHienTai_LostFocus(object sender, RoutedEventArgs e)
        {
            xa_phuong_hien_tai = cmbXaPhuongHienTai.Text;
        }

        void txtNhomTVien_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
                btnNhomTVien_Click(sender, null);
        }

        void chkKHHieuLuc_Unchecked(object sender, RoutedEventArgs e)
        {
            raddtNgayHetHL.Value = null;
            stackLyDoRa.Visibility = System.Windows.Visibility.Hidden;
            cmbLyDoRaKhoiNhom.Visibility = Visibility.Hidden;
        }

        void chkKHHieuLuc_Checked(object sender, RoutedEventArgs e)
        {
            raddtNgayHetHL.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            if (lstSourceLoaiKH.ElementAt(cmbLoaiKhachHang.SelectedIndex).KeywordStrings.ElementAt(0) != ApplicationConstant.LoaiKhachHang.DOANH_NGHIEP.layGiaTri())
            {
                stackLyDoRa.Visibility = System.Windows.Visibility.Visible;
                cmbLyDoRaKhoiNhom.Visibility = Visibility.Visible;
            }
        }
        #endregion

        #region Xu ly nghiep vu

        private void beforeView()
        {
            SetFormData();
            SetEnabledAllControls(false);
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KH_THANH_VIEN);
        }

        private void beforeAddNew()
        {
            //XuLyGiaoDien("CNHAN");
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KH_THANH_VIEN);
        }

        private void beforeModifyFromList(DatabaseConstant.Action action)
        {
            if (action == DatabaseConstant.Action.XEM)
            {
                SetEnabledAllControls(false);
            }
            else
            {
                SetEnabledAllControls(true);
            }
            CommonFunction.RefreshButton(Toolbar, action, tthaiNvu, mnuMain, DatabaseConstant.Function.KH_THANH_VIEN);
        }

        private void beforeModifyFromDetail()
        {
            // Yêu cầu lock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idKhachHang);

            bool ret = process.LockData(DatabaseConstant.Module.KHTV,
                _function,
                DatabaseConstant.Table.KH_KHANG_HSO,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Nếu lock thành công >> cho phép sửa
            if (ret)
            {
                SetFormData();
                SetEnabledAllControls(true);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu, mnuMain, DatabaseConstant.Function.KH_THANH_VIEN);
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
                listLockId.Add(_idKhachHang);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.KHTV,
                    _function,
                    DatabaseConstant.Table.KH_KHANG_HSO,
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
                listLockId.Add(_idKhachHang);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.KHTV,
                    _function,
                    DatabaseConstant.Table.KH_KHANG_HSO,
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

        private void beforeCancel()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_idKhachHang);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.KHTV,
                    _function,
                    DatabaseConstant.Table.KH_KHANG_HSO,
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
                listLockId.Add(_idKhachHang);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.KHTV,
                    _function,
                    DatabaseConstant.Table.KH_KHANG_HSO,
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
            string trangThai = tthaiNvu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()) ? tthaiNvu : CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.TRINH_DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            tabThongTinChung.Focus();
            if (Validation())
            {
                KhachHangProcess process = new KhachHangProcess();
                try
                {
                    // Dữ liệu truyền vào và dữ liệu trả về
                    KH_KHANG_HSO obj = new KH_KHANG_HSO();
                    List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                    int ret = -1;
                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (_idKhachHang == -1)
                    {
                        // Lấy dữ liệu từ form
                        obj = GetFormData(false, trangThai);
                        LayThongTinThuocTinh(false);
                        ret = process.Them(obj, dsSourceKHang, ref _idKhachHang, ref lstResponseDetail);
                        tthaiNvu = obj.TTHAI_NVU;
                        CommonFunction.ThongBaoKetQua(lstResponseDetail);
                        afterAddNew(_idKhachHang);
                    }
                    // Nếu là lưu tạm khi sửa
                    // Hoặc lưu tạm khi sửa sau duyệt
                    // Hoặc sửa
                    else
                    {
                        // Lấy dữ liệu từ form
                        obj = GetFormData(true, trangThai);
                        LayThongTinThuocTinh(true);
                        ret = process.Sua(obj, dsSourceKHang, ref lstResponseDetail);
                        tthaiNvu = obj.TTHAI_NVU;
                        CommonFunction.ThongBaoKetQua(lstResponseDetail);
                        afterModify(obj.ID, ret);
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
            KhachHangProcess process = new KhachHangProcess();
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            try
            {
                // Dữ liệu truyền vào và dữ liệu trả về
                KH_KHANG_HSO obj = new KH_KHANG_HSO();
                int ret = -1;

                // Nếu là lưu tạm hoặc thêm mới lần đầu
                if (_idKhachHang == -1)
                {
                    // Lấy dữ liệu từ form
                    obj = GetFormData(false, trangThai);
                    LayThongTinThuocTinh(false);

                    ret = process.Them(obj, dsSourceKHang, ref _idKhachHang, ref lstResponseDetail);
                    tthaiNvu = obj.TTHAI_NVU;
                    //CommonFunction.ThongBaoKetQua(lstResponseDetail);
                    afterAddNew(_idKhachHang);
                }
                // Nếu là lưu tạm khi sửa
                // Hoặc lưu tạm khi sửa sau duyệt
                // Hoặc sửa
                else
                {
                    // Lấy dữ liệu từ form
                    obj = GetFormData(true, trangThai);
                    LayThongTinThuocTinh(true);

                    ret = process.Sua(obj, dsSourceKHang, ref lstResponseDetail);
                    tthaiNvu = obj.TTHAI_NVU;
                    CommonFunction.ThongBaoKetQua(lstResponseDetail);
                    afterModify(obj.ID,ret);
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
            KhachHangProcess process = new KhachHangProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = _idKhachHang;

                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                int ret = process.Xoa(arrayID, ref lstResponseDetail);
                CommonFunction.ThongBaoKetQua(lstResponseDetail);

                afterDelete(ret);
            }
            catch (System.Exception ex)
            {
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
            KhachHangProcess process = new KhachHangProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = _idKhachHang;
                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                int ret = process.Duyet(arrayID, ref lstResponseDetail);
                CommonFunction.ThongBaoKetQua(lstResponseDetail);
                afterApprove(ret);
            }
            catch (System.Exception ex)
            {
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
            KhachHangProcess process = new KhachHangProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = _idKhachHang;

                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                int ret = process.ThoaiDuyet(arrayID, ref lstResponseDetail);
                CommonFunction.ThongBaoKetQua(lstResponseDetail);

                afterCancel(ret);
            }
            catch (System.Exception ex)
            {
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
            KhachHangProcess process = new KhachHangProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = _idKhachHang;

                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                int ret = process.TuChoi(arrayID, ref lstResponseDetail);
                CommonFunction.ThongBaoKetQua(lstResponseDetail);

                afterRefuse(ret);
            }
            catch (System.Exception ex)
            {
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
        private void afterAddNew(int id)
        {
            if (id > 0)
            {
                

                SetEnabledAllControls(false);
                if (cbMultiAdd.IsChecked == true)
                {
                    SetEnabledAllControls(true);
                    ResetForm();
                }
                else if (!DatabaseConstant.CLOSE_DETAIL_FORM)
                {
                    _idKhachHang = id;
                    SetFormData();
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu,mnuMain,DatabaseConstant.Function.KH_THANH_VIEN);
                    txtTenKhachHang.Focus();
                }
                else
                {
                    onClose();
                }
            }
            else
            {
            }
        }

        /// <summary>
        /// Sau khi sửa
        /// </summary>
        /// <param name="ret"></param>
        private void afterModify(int id,int ret)
        {
            if (ret > 0)
            {
                _idKhachHang = id;
                SetFormData();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KH_THANH_VIEN);
                SetEnabledAllControls(false);

                txtTenKhachHang.Focus();
            }
            else
            {
                //LMessage.ShowMessage("M.DungChung.CapNhatKoThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                _function,
                DatabaseConstant.Table.KH_KHANG_HSO,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Sau khi xóa
        /// </summary>
        /// <param name="ret"></param>
        private void afterDelete(int ret)
        {
            

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idKhachHang);
            List<int> listUnlockId = new List<int>();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                _function,
                DatabaseConstant.Table.KH_KHANG_HSO,
                DatabaseConstant.Action.SUA,
                listLockId);
            if (ret == 2)
            {
                // Đóng cửa sổ chi tiết sau khi xóa
                onClose();
            }
            else
            {

            }
            
        }

        /// <summary>
        /// Sau khi duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void afterApprove(int ret)
        {
            if (ret == 2)
            {
                TthaiNvu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);

                SetEnabledAllControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KH_THANH_VIEN);
            }
            else
            {
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idKhachHang);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                _function,
                DatabaseConstant.Table.KH_KHANG_HSO,
                DatabaseConstant.Action.DUYET,
                listLockId);
        }

        /// <summary>
        /// Sau khi thoái duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void afterCancel(int ret)
        {
            if (ret == 2)
            {
                TthaiNvu = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);

                SetEnabledAllControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KH_THANH_VIEN);
            }
            else
            {
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idKhachHang);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                _function,
                DatabaseConstant.Table.KH_KHANG_HSO,
                DatabaseConstant.Action.THOAI_DUYET,
                listLockId);
        }

        /// <summary>
        /// Sau khi từ chối
        /// </summary>
        /// <param name="ret"></param>
        private void afterRefuse(int ret)
        {
            if (ret == 2)
            {

                TthaiNvu = BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);

                SetEnabledAllControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KH_THANH_VIEN);
            }
            else
            {
                
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idKhachHang);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                _function,
                DatabaseConstant.Table.KH_KHANG_HSO,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                listLockId);
        }

        /// <summary>
        /// Lay thong tin khách hàng
        /// </summary>
        /// <returns></returns>
        private Presentation.Process.KhachHangServiceRef.KH_KHANG_HSO GetFormData(bool isUpdate, string status)
        {
            Presentation.Process.KhachHangServiceRef.KH_KHANG_HSO obj = new Presentation.Process.KhachHangServiceRef.KH_KHANG_HSO();
            AutoComboBox au = new AutoComboBox();
            #region Lay du lieu tu cac combobox
            AutoCompleteEntry auPhongGDHienTai = au.getEntryByDisplayName(lstSourcePhongGD, ref cmbPhongGD);
            AutoCompleteEntry auTinhTpHienTai = au.getEntryByDisplayName(lstSourceTinhTp, ref cmbTinhTPHienTai);
            AutoCompleteEntry auQuanHuyenHienTai = au.getEntryByDisplayName(lstSourceQuanHuyen, ref cmbQuanHuyenHienTai);
            AutoCompleteEntry auXaPhuongHienTai = au.getEntryByDisplayName(lstSourceXaPhuong, ref cmbXaPhuongHienTai);
            AutoCompleteEntry auTTCuTru = au.getEntryByDisplayName(lstSourceTTCuTru, ref cmbTinhTrangCuTru);
            AutoCompleteEntry auGioiTinh = au.getEntryByDisplayName(lstSourceGioiTinh, ref cmbGioiTinh);
            AutoCompleteEntry auNoiCap = au.getEntryByDisplayName(lstSourceNoiCap, ref cmbNoiCap);
            AutoCompleteEntry auTinhTpHoKhau = au.getEntryByDisplayName(lstSourceHKTinhTP, ref cmbHKTinhTP);
            AutoCompleteEntry auQuanHuyenHoKhau = au.getEntryByDisplayName(lstSourceHKQuanHuyen, ref cmbHKQuanHuyen);
            AutoCompleteEntry auXaPhuongHoKhau = au.getEntryByDisplayName(lstSourceHKXaPhuong, ref cmbHKXaPhuong);
            AutoCompleteEntry auDanToc = au.getEntryByDisplayName(lstSourceDanToc, ref cmbDanToc);
            AutoCompleteEntry auQuocTich = au.getEntryByDisplayName(lstSourceQuocTich, ref cmbQuocTich);
            AutoCompleteEntry auTTrangHonNhan = au.getEntryByDisplayName(lstSourceHonNhan, ref cmbHonNhan);
            AutoCompleteEntry auVtroGiaDinh = au.getEntryByDisplayName(lstSourceVaiTroGD, ref cmbVaiTroGD);
            AutoCompleteEntry auHocVan = au.getEntryByDisplayName(lstSourceHocVan, ref cmbHocVan);
            AutoCompleteEntry auLHinhCTac = au.getEntryByDisplayName(lstSourceLHinhCongTac, ref cmbLHCongTac);
            AutoCompleteEntry auTGianCTac = au.getEntryByDisplayName(lstSourceTGianCongTac, ref cmbTGCongTac);
            AutoCompleteEntry auLyDoRa = au.getEntryByDisplayName(lstSourceLyDoRa, ref cmbLyDoRaKhoiNhom);
            AutoCompleteEntry auLoaiHinhTC = au.getEntryByDisplayName(lstSourceLHToChuc, ref cmbLoaiHinhTC);
            AutoCompleteEntry auNganhKT = au.getEntryByDisplayName(lstSourceNganhKT, ref cmbNganhKinhTe);
            AutoCompleteEntry auLoaiKH = au.getEntryByDisplayName(lstSourceLoaiKH, ref cmbLoaiKhachHang);
            #endregion

            #region Tab thong tin chung
            if (isUpdate)
            {
                obj.ID = _idKhachHang;
            }
            obj.MA_KHANG_LOAI = auLoaiKH.KeywordStrings[0];
            if (isUpdate)
            {
                obj.MA_KHANG = txtMaKhachHang.Text;
                obj.MA_TVIEN = txtMaKhachHang.Text;
            }
            obj.TEN_KHANG = txtTenKhachHang.Text;
            obj.TEN_GDICH = obj.TEN_KHANG;
            if (raddtNgayTGiaTC.Value != null)
            {
                obj.NGAY_THAM_GIA = LDateTime.DateToString(Convert.ToDateTime(raddtNgayTGiaTC.Value), "yyyyMMdd");
            }
            else
            {
                obj.NGAY_THAM_GIA = "";
            }
            if (raddtNgayThanhLap.Value != null)
            {
                obj.NGAY_THANH_LAP = LDateTime.DateToString(Convert.ToDateTime(raddtNgayThanhLap.Value), "yyyyMMdd");
            }
            else
            {
                obj.NGAY_THANH_LAP = "";
            }
            if (raddtNgayHetHL.Value != null)
            {
                obj.NGAY_HET_HLUC = LDateTime.DateToString(Convert.ToDateTime(raddtNgayHetHL.Value), "yyyyMMdd");
            }
            if (auLyDoRa != null)
            {
                obj.MA_LY_DO = auLyDoRa.KeywordStrings[0];
            }

            //obj.ID_DON_VI = Presentation.Process.Common.ClientInformation.IdDonVi;
            // Lấy id phòng giao dịch là id_donvi
            //if (lstSourceLoaiKH.ElementAt(cmbLoaiKhachHang.SelectedIndex).KeywordStrings.First().Equals(BusinessConstant.LoaiKhachHang.CNHAN.layGiaTri()))
            //    obj.ID_DON_VI = Convert.ToInt32(auPhongGDHienTai.KeywordStrings.ElementAt(1));
            //else
            //    obj.ID_DON_VI = Convert.ToInt32(idPhongGD);
            obj.ID_DON_VI = Convert.ToInt32(idPhongGD);
            if (!LString.IsNullOrEmptyOrSpace(idKhuVuc))
            {
                obj.ID_KHU_VUC = Convert.ToInt32(idKhuVuc);
            }
            if (!LString.IsNullOrEmptyOrSpace(idCum))
            {
                obj.ID_CUM = Convert.ToInt32(idCum);
            }
            if (txtNhomTVien.Tag != null && !LString.IsNullOrEmptyOrSpace(txtNhomTVien.Tag.ToString()))
            {
                obj.ID_NHOM = Convert.ToInt32(txtNhomTVien.Tag);
            }
            if (raddtNgaySinh.Value != null)
            {
                obj.DD_NGAY_SINH = LDateTime.DateToString(Convert.ToDateTime(raddtNgaySinh.Value), "yyyyMMdd");
            }
            if (auLyDoRa != null)
            {
                obj.MA_LY_DO = auLyDoRa.KeywordStrings[0];
            }

            if (formCase == "CNHAN" || formCase == "TVIEN")
            {
                obj.DD_GTLQ_LOAI = ApplicationConstant.LoaiGiayTo.CHUNG_MINH_ND.layGiaTri();
            }
            else
            {
                obj.DD_GTLQ_LOAI = ApplicationConstant.LoaiGiayTo.GP_DKKD.layGiaTri();
            }
            obj.DD_GTLQ_SO = txtSoCMND.Text.Trim();
            if (raddtNgayCap.Value != null)
            {
                obj.DD_GTLQ_NGAY_CAP = LDateTime.DateToString(Convert.ToDateTime(raddtNgayCap.Value), "yyyyMMdd");
            }
            if (auNoiCap != null)
            {
                obj.DD_GTLQ_NOI_CAP = auNoiCap.KeywordStrings[0];
            }
            if (auGioiTinh != null)
            {
                obj.DD_GIOI_TINH = auGioiTinh.KeywordStrings[0];
            }
            if (auDanToc != null)
            {
                obj.DD_MA_DAN_TOC = auDanToc.KeywordStrings[0];
            }
            else
            {
                obj.DD_MA_DAN_TOC = "";
            }
            if (auQuocTich != null)
            {
                obj.DD_MA_QUOC_TICH = auQuocTich.KeywordStrings[0];
            }
            else
            {
                obj.DD_MA_QUOC_TICH = "";
            }
            obj.MA_SO_THUE = txtMaSoThue.Text.Trim();
            if (auTTCuTru != null)
            {
                obj.MA_TTRANG_CUTRU = auTTCuTru.KeywordStrings[0];
            }

            if (cmbNguonVon.SelectedIndex >= 0)
                obj.NV_LOAI_NVON = lstSourceNguonVon.ElementAt(cmbNguonVon.SelectedIndex).KeywordStrings.ElementAt(0);
            obj.NGAY_CONG_NHAN = obj.NGAY_THAM_GIA;
            #endregion

            #region Thong tin tai san doanh nghiep
            obj.TONG_TAI_SAN = (decimal?)txtTongTS.Value;
            obj.VON_DIEU_LE = (decimal?)txtVonDieuLe.Value;
            if (auLoaiKH.KeywordStrings[0].Equals(ApplicationConstant.LoaiKhachHang.DOANH_NGHIEP.layGiaTri()) && !LObject.IsNullOrEmpty(dsSourceKHang.Tables["VKH_NGUOI_DDIEN"]) && dsSourceKHang.Tables["VKH_NGUOI_DDIEN"].Rows.Count > 0)
            {
                obj.DD_HO_TEN = dsSourceKHang.Tables["VKH_NGUOI_DDIEN"].Rows[0]["DDIEN_HO_TEN"].ToString();
            }
            #endregion

            #region Ho khau thuong tru
            obj.DD_TTRU_DIA_CHI = txtDiaChi.Text.Trim();
            if (auTinhTpHoKhau != null)
            {
                obj.DD_TTRU_MA_TINHTP = auTinhTpHoKhau.KeywordStrings[0];
            }
            if (auQuanHuyenHoKhau != null)
            {
                obj.DD_TTRU_MA_QUAN = auQuanHuyenHoKhau.KeywordStrings[0];
            }
            if (auXaPhuongHoKhau != null)
            {
                obj.DD_TTRU_MA_PHUONG = auXaPhuongHoKhau.KeywordStrings[0];
            }
            #endregion

            #region Dia chi hien tai
            obj.DIA_CHI = txtDiaChiHienTai.Text.Trim();
            if (auTinhTpHienTai != null)
            {
                obj.MA_TINHTP = auTinhTpHienTai.KeywordStrings[0];
            }
            if (auQuanHuyenHienTai != null)
            {
                obj.MA_QUAN = auQuanHuyenHienTai.KeywordStrings[0];
            }
            if (auXaPhuongHienTai != null)
            {
                obj.MA_PHUONG = auXaPhuongHienTai.KeywordStrings[0];
            }
            #endregion

            #region Thong tin khac
            obj.SO_DTHOAI = txtSoCoDinh.Text.Trim();
            obj.SO_DDONG = txtSoDiDong.Text.Trim();
            obj.EMAIL = txtEmail.Text.Trim();

            obj.DD_HO_TEN = obj.TEN_KHANG;
            obj.DD_SO_DTHOAI = obj.SO_DTHOAI;
            obj.DD_SO_DDONG = obj.SO_DDONG;
            obj.DD_EMAIL = obj.EMAIL;
            obj.DDAN_HINH_ANH = "";

            if (auTTrangHonNhan != null)
            {
                obj.DD_MA_TTRANG_HNHAN = auTTrangHonNhan.KeywordStrings[0];
            }
            if (auVtroGiaDinh != null)
            {
                obj.DD_MA_VTRO_GDINH = auVtroGiaDinh.KeywordStrings[0];
            }
            if (auHocVan != null)
            {
                obj.DD_MA_TDO_HVAN = auHocVan.KeywordStrings[0];
            }
            if (auLHinhCTac != null)
            {
                obj.DD_MA_LHINH_CTAC = auLHinhCTac.KeywordStrings[0];
            }
            if (auTGianCTac != null)
            {
                obj.DD_MA_TGIAN_CTAC = auTGianCTac.KeywordStrings[0];
            }
            if (chkMPA.IsChecked==true)
                obj.MA_MPA = BusinessConstant.CoKhong.CO.layGiaTri();
            else
                obj.MA_MPA = BusinessConstant.CoKhong.KHONG.layGiaTri();
            #endregion

            obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
            obj.TTHAI_NVU = status;
            //obj.MA_DVI_TAO = Presentation.Process.Common.ClientInformation.MaDonVi;
            obj.MA_DVI_TAO = auPhongGDHienTai.KeywordStrings.ElementAt(0);
            obj.MA_DVI_QLY = Presentation.Process.Common.ClientInformation.MaDonVi;
            if (!isUpdate)
            {
                obj.NGAY_NHAP = Presentation.Process.Common.ClientInformation.NgayLamViecHienTai;
                obj.NGUOI_NHAP = Presentation.Process.Common.ClientInformation.TenDangNhap;
                
            }
            else
            {
                obj.NGAY_NHAP = LDateTime.DateToString(Convert.ToDateTime(raddtNgayNhap.Value), "yyyyMMdd");
                obj.NGUOI_NHAP = txtNguoiLap.Text;
                obj.NGAY_CNHAT = Presentation.Process.Common.ClientInformation.NgayLamViecHienTai;
                obj.NGUOI_CNHAT = Presentation.Process.Common.ClientInformation.TenDangNhap;
                if (bHetHLuc && !chkKHHieuLuc.IsChecked.Value)
                    obj.NGAY_CONG_NHAN = ClientInformation.NgayLamViecHienTai;
                else
                    obj.NGAY_CONG_NHAN = ngayCongNhan;
            }
            if (auLoaiHinhTC != null)
            {
                obj.MA_LOAI_HINH_KT = auLoaiHinhTC.KeywordStrings[0];
            }
            if (auNganhKT != null)
            {
                obj.MA_NGANH_KT = auNganhKT.KeywordStrings[0];
            }
            return obj;
        }

        /// <summary>
        /// Lấy dữ liệu thuộc tính
        /// </summary>
        /// <param name="isUpdate"></param>
        private void LayThongTinThuocTinh(bool isUpdate)
        {
            AutoComboBox au = new AutoComboBox();
            AutoCompleteEntry auNoiCuTru = au.getEntryByDisplayName(lstSourceNoiCuTru, ref cmbNoiCuTru);
            AutoCompleteEntry auThoiGianCuTru = au.getEntryByDisplayName(lstSourceTGCuTru, ref cmbTGianCuTru);

            DataRow drNhaO;
            DataRow drPTien_Dlai;
            DataRow drPtien_TTin;
            if (dsSourceKHang.Tables["VKH_NHA_O"].Rows.Count == 0)
            {
                drNhaO = dsSourceKHang.Tables["VKH_NHA_O"].NewRow();
            }
            else
            {
                drNhaO = dsSourceKHang.Tables["VKH_NHA_O"].Rows[0];
            }

            if (dsSourceKHang.Tables["VKH_PTIEN_DLAI"].Rows.Count == 0)
            {
                drPTien_Dlai = dsSourceKHang.Tables["VKH_PTIEN_DLAI"].NewRow();
            }
            else
            {
                drPTien_Dlai = dsSourceKHang.Tables["VKH_PTIEN_DLAI"].Rows[0];
            }

            if (dsSourceKHang.Tables["VKH_PTIEN_TTIN"].Rows.Count == 0)
            {
                drPtien_TTin = dsSourceKHang.Tables["VKH_PTIEN_TTIN"].NewRow();
            }
            else
            {
                drPtien_TTin = dsSourceKHang.Tables["VKH_PTIEN_TTIN"].Rows[0];
            }

            #region Thong tin nha o
            // Thong tin cu tru
            if (auNoiCuTru == null)
            {
                drNhaO["NO_NOI_CU_TRU"] = "";
            }
            else
            {
                drNhaO["NO_NOI_CU_TRU"] = auNoiCuTru.KeywordStrings[0];
            }

            if (auThoiGianCuTru == null)
            {
                drNhaO["NO_THOI_GIAN_CU_TRU"] = "";
            }
            else
            {
                drNhaO["NO_THOI_GIAN_CU_TRU"] = auThoiGianCuTru.KeywordStrings[0];
            }

            // Kich co nha
            if (rdoKCoNhaNho.IsChecked == true)
            {
                drNhaO["NO_KICH_CO_NHA"] = "NHO";
            }
            else if (rdoKCoNhaTB.IsChecked == true)
            {
                drNhaO["NO_KICH_CO_NHA"] = "TRUNG_BINH";
            }
            else if (rdoKCoNhaTo.IsChecked == true)
            {
                drNhaO["NO_KICH_CO_NHA"] = "TO";
            }
            else
            {
                drNhaO["NO_KICH_CO_NHA"] = "KHAC";
            }

            // Tuong nha
            if (rdoTNhaDat.IsChecked == true)
            {
                drNhaO["NO_TUONG_NHA"] = "DAT_COT_EP";
            }
            else if (rdoKTNhaVNua.IsChecked == true)
            {
                drNhaO["NO_TUONG_NHA"] = "TRE_NUA";
            }
            else if (rdoKTNGach.IsChecked == true)
            {
                drNhaO["NO_TUONG_NHA"] = "GACH_GO";
            }
            else if (rdoTuongNhaKhac.IsChecked == true)
            {
                drNhaO["NO_TUONG_NHA"] = "KHAC";
            }

            // Chat lop mai nha
            if (rdoCLopTranh.IsChecked == true)
            {
                drNhaO["NO_CHAT_LOP_MAI"] = "TRANH_LA";
            }
            else if (rdoCLopTon.IsChecked == true)
            {
                drNhaO["NO_CHAT_LOP_MAI"] = "TON";
            }
            else if (rdoCLopMai.IsChecked == true)
            {
                drNhaO["NO_CHAT_LOP_MAI"] = "MAI_BANG";
            }
            else if (rdoCLopMaiKhac.IsChecked == true)
            {
                drNhaO["NO_CHAT_LOP_MAI"] = "KHAC";
            }

            // Muc do ben chac
            if (rdoMDoCap4.IsChecked == true)
            {
                drNhaO["NO_MUC_DO_BEN"] = "NHA_CAP_4";
            }
            else if (rdoMDoCap3.IsChecked == true)
            {
                drNhaO["NO_MUC_DO_BEN"] = "NHA_CAP_3";
            }
            else if (rdoMDoKienCo.IsChecked == true)
            {
                drNhaO["NO_MUC_DO_BEN"] = "NHA_KIEN_CO";
            }
            #endregion

            #region Phuong tien di lai
            if (rdoPTienCongCong.IsChecked == true)
            {
                drPTien_Dlai["PPDL_LOAI_PTIEN"] = "PT_CONG_CONG";
            }
            else if (rdoPTienXeMay.IsChecked == true)
            {
                drPTien_Dlai["PPDL_LOAI_PTIEN"] = "XE_HAI_BANH";
            }
            else if (rdoPTienOto.IsChecked == true)
            {
                drPTien_Dlai["PPDL_LOAI_PTIEN"] = "OTO";
            }
            else
            {
                drPTien_Dlai["PPDL_LOAI_PTIEN"] = "KHAC";
            }
            #endregion

            #region Phuong tien thong tin
            if (rdoPTTTCo.IsChecked == true)
            {
                drPtien_TTin["PPTT_SDUNG_DTHOAI"] = "CO_DIEN_THOAI";
            }
            else if (rdoPTTTKhong.IsChecked == true)
            {
                drPtien_TTin["PPTT_SDUNG_DTHOAI"] = "KHONG_CO_DIEN_THOAI";
            }
            if (dsSourceKHang.Tables["VKH_NHA_O"].Rows.Count==0)
                dsSourceKHang.Tables["VKH_NHA_O"].Rows.Add(drNhaO);
            if (dsSourceKHang.Tables["VKH_PTIEN_DLAI"].Rows.Count == 0)
                dsSourceKHang.Tables["VKH_PTIEN_DLAI"].Rows.Add(drPTien_Dlai);
            if (dsSourceKHang.Tables["VKH_PTIEN_TTIN"].Rows.Count == 0)
                dsSourceKHang.Tables["VKH_PTIEN_TTIN"].Rows.Add(drPtien_TTin);
            #endregion
        }

        /// <summary>
        /// Kiểm tra dữ liệu trước khi lưu
        /// </summary>
        /// <returns></returns>
        private bool Validation()
        {
            
            //int n = ClientInformation.NgayLamViecHienTai.CompareTo((ngay.StringToDate("yyyyMMdd").AddYears(16)).DateToString("yyyyMMdd"));
            AutoComboBox au = new AutoComboBox();
            AutoCompleteEntry auLoaiKH = au.getEntryByDisplayName(lstSourceLoaiKH, ref cmbLoaiKhachHang);
            AutoCompleteEntry auLoaiToChuc = au.getEntryByDisplayName(lstSourceLHToChuc, ref cmbLoaiHinhTC);
            AutoCompleteEntry auNganhKT = au.getEntryByDisplayName(lstSourceNganhKT,ref cmbNganhKinhTe);
            AutoCompleteEntry auTinhHienTai = au.getEntryByDisplayName(lstSourceTinhTp, ref cmbTinhTPHienTai);
            AutoCompleteEntry auQuanHienTai = au.getEntryByDisplayName(lstSourceQuanHuyen, ref cmbQuanHuyenHienTai);
            AutoCompleteEntry auXaHienTai = au.getEntryByDisplayName(lstSourceXaPhuong, ref cmbXaPhuongHienTai);

            AutoCompleteEntry auHonNhan = au.getEntryByDisplayName(lstSourceHonNhan, ref cmbHonNhan);
            AutoCompleteEntry auHocVan = au.getEntryByDisplayName(lstSourceHocVan, ref cmbHocVan);
            AutoCompleteEntry auVaiTroGD = au.getEntryByDisplayName(lstSourceVaiTroGD, ref cmbVaiTroGD);
            AutoCompleteEntry auLHinhCongTac = au.getEntryByDisplayName(lstSourceLHinhCongTac, ref cmbLHCongTac);
            AutoCompleteEntry auTGianCongTac = au.getEntryByDisplayName(lstSourceTGianCongTac, ref cmbTGCongTac);
            AutoCompleteEntry auLyDoHHL = au.getEntryByDisplayName(lstSourceLyDoRa, ref cmbLyDoRaKhoiNhom);
            if (chkKHHieuLuc.IsChecked.GetValueOrDefault())
            {
                if(!tthaiNvu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.TrangThaiNghiepVuKhongPhuHop", LMessage.MessageBoxType.Warning);
                    txtTenKhachHang.Focus();
                    return false;
                }
                else if (idKhachHang<1)
                {
                    LMessage.ShowMessage("M_ResponseMessage_KhachHang_KhongTonTai", LMessage.MessageBoxType.Warning);
                    txtTenKhachHang.Focus();
                    return false;
                }
                if (auLoaiKH.KeywordStrings[0] == ApplicationConstant.LoaiKhachHang.THANH_VIEN.layGiaTri())
                {
                    if (raddtNgayHetHL.Value != null)
                    {
                        if (raddtNgayHetHL.Value != null && raddtNgayHetHL.Value <= raddtNgayTGiaTC.Value)
                        {
                            LMessage.ShowMessage("M.KhachHang.ucKhachHangThanhVien.SaiNgayHetHan", LMessage.MessageBoxType.Warning);
                            raddtNgayHetHL.Focus();
                            return false;
                        }
                        else if (LObject.IsNullOrEmpty(auLyDoHHL) || cmbLyDoRaKhoiNhom.Text.IsNullOrEmptyOrSpace())
                        {
                            CommonFunction.ThongBaoTrong(lblLyDoRaNhom.Content.ToString());
                            cmbLyDoRaKhoiNhom.Focus();
                            return false;
                        }
                    }
                    
                }
            }
            else
            {
                if (LString.IsNullOrEmptyOrSpace(txtTenKhachHang.Text))
                {
                    CommonFunction.ThongBaoTrong(lblTenKhachHang.Content.ToString());
                    txtTenKhachHang.Focus();
                    return false;
                }
                else if (raddtNgayTGiaTC.Value == null)
                {
                    CommonFunction.ThongBaoTrong(lblNgayThamGiaTC.Content.ToString());
                    raddtNgayTGiaTC.Focus();
                    return false;
                }

                if (auLoaiKH.KeywordStrings[0] == ApplicationConstant.LoaiKhachHang.TCTD.layGiaTri() || auLoaiKH.KeywordStrings[0] == ApplicationConstant.LoaiKhachHang.DOANH_NGHIEP.layGiaTri())
                {
                    if (raddtNgayThanhLap.Value == null)
                    {
                        CommonFunction.ThongBaoTrong(lblNgayThanhLap.Content.ToString());
                        raddtNgayThanhLap.Focus();
                        return false;
                    }
                }

                if (auLoaiKH.KeywordStrings[0] == ApplicationConstant.LoaiKhachHang.THANH_VIEN.layGiaTri())
                {
                    if (LString.IsNullOrEmptyOrSpace(txtNhomTVien.Text))
                    {
                        CommonFunction.ThongBaoTrong(lblNhomThanhVien.Content.ToString());
                        txtNhomTVien.Focus();
                        return false;
                    }
                }

                int minTuoi = 16;
                //int maxTuoi = 64;
                UtilitiesProcess process = new UtilitiesProcess();
                if (!auLoaiKH.KeywordStrings[0].Equals(ApplicationConstant.LoaiKhachHang.DOANH_NGHIEP.layGiaTri()))
                {
                    string _minTuoi = process.LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_KH_MIN_TUOI, null);
                    string _maxTuoi = process.LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_KH_MAX_TUOI, null);
                    if (!_minTuoi.IsNullOrEmpty() && !_minTuoi.IsNumeric()) minTuoi = _minTuoi.StringToInt32();
                    //if (!_maxTuoi.IsNullOrEmpty() && !_maxTuoi.IsNumeric()) maxTuoi = _maxTuoi.StringToInt32();
                    string ngay = "";
                    if (raddtNgaySinh.Value == null)
                    {
                        CommonFunction.ThongBaoTrong(lblNgaySinh.Content.ToString());
                        raddtNgaySinh.Focus();
                        return false;
                    }
                    else
                        ngay = Convert.ToDateTime(raddtNgaySinh.Value).DateToString("yyyyMMdd");


                    //if (ClientInformation.NgayLamViecHienTai.CompareTo((ngay.StringToDate("yyyyMMdd").AddYears(minTuoi)).DateToString("yyyyMMdd")) < 0
                    //    || ClientInformation.NgayLamViecHienTai.CompareTo((ngay.StringToDate("yyyyMMdd").AddYears(maxTuoi)).DateToString("yyyyMMdd")) > 0)
                    //{
                    //    CommonFunction.ThongBaoLoi("M.KhachHang.Popup.ucThongTinCoBanHoGD.LoiNgaySinh");
                    //    raddtNgaySinh.Focus();
                    //    return false;
                    //}
                    if (ClientInformation.NgayLamViecHienTai.CompareTo((ngay.StringToDate("yyyyMMdd").AddYears(minTuoi)).DateToString("yyyyMMdd")) < 0)
                    {
                        CommonFunction.ThongBaoLoi("M.KhachHang.Popup.ucThongTinCoBanHoGD.LoiNgaySinh");
                        raddtNgaySinh.Focus();
                        return false;
                    }
                }
                // Kiểm tra số chứng minh nhân dân của khách hàng hoặc người đại diện
                if (LString.IsNullOrEmptyOrSpace(txtSoCMND.Text))
                {
                    CommonFunction.ThongBaoTrong(lblSoCMND.Content.ToString());
                    txtSoCMND.Focus();
                    return false;
                }
                else if (!Regex.IsMatch(txtSoCMND.Text, @"^\d+$"))
                {
                    LMessage.ShowMessage(ApplicationConstant.NghiepVuResponseMessage.M_ResponseMessage_KhachHang_SoCMNDKhongHopLe.layGiaTri(), LMessage.MessageBoxType.Error);
                    txtSoCMND.Focus();
                    return false;
                }
                else if (!txtSoCMND.Text.Length.Equals(9) && !txtSoCMND.Text.Length.Equals(12))
                {
                    LMessage.ShowMessage(ApplicationConstant.NghiepVuResponseMessage.M_ResponseMessage_KhachHang_SoCMNDKhongHopLe.layGiaTri(), LMessage.MessageBoxType.Error);
                    txtSoCMND.Focus();
                    return false;
                }
                //else if (new KhachHangProcess().CheckSoCMND(txtSoCMND.Text, txtMaKhachHang.Text) == 0)
                //{
                //    //CommonFunction.ThongBaoTrong(lblSoCMND.Content.ToString());
                //    CommonFunction.ThongBaoLoi("M.KhachHang.ucKhachHangThanhVien.TrungCMND");
                //    txtSoCMND.Focus();
                //    return false;
                //}
                if (raddtNgayCap.Value == null)
                {
                    CommonFunction.ThongBaoTrong(lblNgayCap.Content.ToString());
                    raddtNgayCap.Focus();
                    return false;
                }
                else if (LString.IsNullOrEmptyOrSpace(cmbNoiCap.Text))
                {
                    CommonFunction.ThongBaoTrong(lblNoiCap.Content.ToString());
                    cmbNoiCap.Focus();
                    return false;
                }


                //--------------------------------------------------------------------------------------------------
                if (auLoaiKH.KeywordStrings[0] == ApplicationConstant.LoaiKhachHang.THANH_VIEN.layGiaTri())
                {
                    if (LString.IsNullOrEmptyOrSpace(cmbDanToc.Text))
                    {
                        CommonFunction.ThongBaoTrong(lblDanToc.Content.ToString());
                        cmbDanToc.Focus();
                        return false;
                    }
                    else if (LString.IsNullOrEmptyOrSpace(cmbQuocTich.Text))
                    {
                        CommonFunction.ThongBaoTrong(lblQuocTich.Content.ToString());
                        cmbQuocTich.Focus();
                        return false;
                    }
                    else if (LString.IsNullOrEmptyOrSpace(txtDiaChi.Text))
                    {
                        LMessage.ShowMessage("M.KhachHang.ucKhachHangThanhVien.ThieuDiaChiHoKhau", LMessage.MessageBoxType.Warning);
                        txtDiaChi.Focus();
                        return false;
                    }
                    else if (LString.IsNullOrEmptyOrSpace(txtDiaChiHienTai.Text))
                    {
                        LMessage.ShowMessage("M.KhachHang.ucKhachHangThanhVien.ThieuDiaChiHienTai", LMessage.MessageBoxType.Warning);
                        txtDiaChiHienTai.Focus();
                        return false;
                    }
                    else if (auHonNhan == null)
                    {
                        CommonFunction.ThongBaoTrong(lblHonNhan.Content.ToString());
                        cmbHonNhan.Focus();
                        return false;
                    }
                    else if (auHocVan == null)
                    {
                        CommonFunction.ThongBaoTrong(lblHocVan.Content.ToString());
                        cmbHocVan.Focus();
                        return false;
                    }
                    else if (auVaiTroGD == null)
                    {
                        CommonFunction.ThongBaoTrong(lblVaiTroGD.Content.ToString());
                        cmbVaiTroGD.Focus();
                        return false;
                    }
                    else if (auLHinhCongTac == null)
                    {
                        CommonFunction.ThongBaoTrong(lblLHinhCongTac.Content.ToString());
                        cmbLHCongTac.Focus();
                        return false;
                    }
                    else if (auTGianCongTac == null)
                    {
                        CommonFunction.ThongBaoTrong(lblTGianCongTac.Content.ToString());
                        cmbTGCongTac.Focus();
                        return false;
                    }
                }
                else if (auLoaiKH.KeywordStrings[0] == ApplicationConstant.LoaiKhachHang.CA_NHAN.layGiaTri())
                {
                    if (LString.IsNullOrEmptyOrSpace(cmbQuocTich.Text))
                    {
                        CommonFunction.ThongBaoTrong(lblQuocTich.Content.ToString());
                        cmbQuocTich.Focus();
                        return false;
                    }
                    else if (LString.IsNullOrEmptyOrSpace(txtDiaChiHienTai.Text))
                    {
                        CommonFunction.ThongBaoTrong(lblDiaChiHienTai.Content.ToString());
                        txtDiaChiHienTai.Focus();
                        return false;
                    }
                    //else if (auTinhHienTai == null)
                    //{
                    //    CommonFunction.ThongBaoTrong(lblTinhHienTai.Content.ToString());
                    //    cmbTinhTPHienTai.Focus();
                    //    return false;
                    //}
                    //else if (auQuanHienTai == null)
                    //{
                    //    CommonFunction.ThongBaoTrong(lblQuanHienTai.Content.ToString());
                    //    cmbQuanHuyenHienTai.Focus();
                    //    return false;
                    //}
                    //else if (auXaHienTai == null)
                    //{
                    //    CommonFunction.ThongBaoTrong(lblXaHienTai.Content.ToString());
                    //    cmbXaPhuongHienTai.Focus();
                    //    return false;
                    //}
                }
                else if (auLoaiKH.KeywordStrings[0] == ApplicationConstant.LoaiKhachHang.TCTD.layGiaTri() || auLoaiKH.KeywordStrings[0] == ApplicationConstant.LoaiKhachHang.DOANH_NGHIEP.layGiaTri())
                {
                    if (auLoaiToChuc == null)
                    {
                        CommonFunction.ThongBaoTrong(lblLoaiHinhTC.Content.ToString());
                        cmbLoaiHinhTC.Focus();
                        return false;
                    }
                    else if (auNganhKT == null)
                    {
                        CommonFunction.ThongBaoTrong(lblNganhKT.Content.ToString());
                        cmbNganhKinhTe.Focus();
                        return false;
                    }
                    else if (txtTongTS.Value == null || txtTongTS.Value == 0)
                    {
                        CommonFunction.ThongBaoTrong(lblTongTaiSan.Content.ToString());
                        txtTongTS.Focus();
                        return false;
                    }
                    else if (txtVonDieuLe.Value == null || txtVonDieuLe.Value == 0)
                    {
                        CommonFunction.ThongBaoTrong(lblVonDieuLe.Content.ToString());
                        txtVonDieuLe.Focus();
                        return false;
                    }
                    else if (LString.IsNullOrEmptyOrSpace(txtDiaChiHienTai.Text))
                    {
                        CommonFunction.ThongBaoTrong(lblDiaChiHienTai.Content.ToString());
                        txtDiaChiHienTai.Focus();
                        return false;
                    }
                    //else if (auTinhHienTai == null)
                    //{
                    //    CommonFunction.ThongBaoTrong(lblTinhHienTai.Content.ToString());
                    //    cmbTinhTPHienTai.Focus();
                    //    return false;
                    //}
                    //else if (auQuanHienTai == null)
                    //{
                    //    CommonFunction.ThongBaoTrong(lblQuanHienTai.Content.ToString());
                    //    cmbQuanHuyenHienTai.Focus();
                    //    return false;
                    //}
                    //else if (auXaHienTai == null)
                    //{
                    //    CommonFunction.ThongBaoTrong(lblXaHienTai.Content.ToString());
                    //    cmbXaPhuongHienTai.Focus();
                    //    return false;
                    //}
                }
            }
            

            return true;
        }
        #endregion

        #region Popup

        #region Popup ThongTinCoBanHoGD
        /// <summary>
        /// Them thong tin co ban ho gia dinh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbAddThongTin_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            ucThongTinCoBanHoGD uc = new ucThongTinCoBanHoGD();
            window.Content = uc;
            uc.idSinhMa = findMinId(dsSourceKHang.Tables["VKH_GIA_DINH"]);
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Title = LLanguage.SearchResourceByKey("U.POPUP.POPUP_THONG_TIN_HO_GIA_DINH");
            window.RenderSize = new Size(1024, 768);
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
            DataTable dtGiaDinh = dsSourceKHang.Tables["VKH_GIA_DINH"];
            if (uc.dtSource != null)
            {
                ImportRows(ref dtGiaDinh, uc.dtSource);
            }
            uc = null;
            grTTCoBanHoGD.Rebind();
        }

        /// <summary>
        /// Sua thong tin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbModifyThongTin_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            ucThongTinCoBanHoGD uc = new ucThongTinCoBanHoGD();
            for (int i = 0; i < dsSourceKHang.Tables["VKH_GIA_DINH"].Rows.Count; i++)
            {
                if (Convert.ToBoolean(dsSourceKHang.Tables["VKH_GIA_DINH"].Rows[i]["CHON"]) == true)
                {
                    uc.dtSource.ImportRow(dsSourceKHang.Tables["VKH_GIA_DINH"].Rows[i]);
                }
            }
            if (uc.dtSource.Rows.Count == 0)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                uc = null;
                return;
            }
            uc.idSinhMa = findMinId(dsSourceKHang.Tables["VKH_GIA_DINH"]);
            window.Content = uc;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.RenderSize = new Size(1024, 768);
            window.Title = LLanguage.SearchResourceByKey("U.POPUP.POPUP_THONG_TIN_HO_GIA_DINH");
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
            if (uc.dtSource != null)
            {
                DataTable dt = dsSourceKHang.Tables["VKH_GIA_DINH"];
                ImportRows(ref dt, uc.dtSource);
            }
            uc = null;
            grTTCoBanHoGD.Rebind();
        }

        /// <summary>
        /// Xoa thong tin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbDeleteThongTin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int rowCount = dsSourceKHang.Tables["VKH_GIA_DINH"].Rows.Count;
                for (int i = rowCount - 1; i >= 0; i--)
                {
                    if (Convert.ToBoolean(dsSourceKHang.Tables["VKH_GIA_DINH"].Rows[i]["CHON"]) == true)
                    {
                        dsSourceKHang.Tables["VKH_GIA_DINH"].Rows.RemoveAt(i);
                    }
                }
                for (int i = 0; i < dsSourceKHang.Tables["VKH_GIA_DINH"].Rows.Count; i++)
                {
                    dsSourceKHang.Tables["VKH_GIA_DINH"].Rows[i]["STT"] = i + 1;
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Khách hàng liên quan
        /// <summary>
        /// Thêm mới khách hàng liên quan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbAddKH_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            ucKhachHangLienQuan uc = new ucKhachHangLienQuan();
            window.Content = uc;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.RenderSize = new Size(1024, 768);
            window.Title = LLanguage.SearchResourceByKey("U.POPUP.POPUP_KH_LIEN_QUAN");
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
            DataTable dtKHLienQuan = dsSourceKHang.Tables["VKH_KHANG_LQUAN"];
            if (uc.dtSource != null)
            {
                ImportRows(ref dtKHLienQuan, uc.dtSource);
            }
            uc = null;
            grKHLQuan.Rebind();
        }

        /// <summary>
        /// Sửa thông tin khách hàng liên quan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbModifyKH_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            ucThongTinCoBanHoGD uc = new ucThongTinCoBanHoGD();
            for (int i = 0; i < dsSourceKHang.Tables["VKH_KHANG_LQUAN"].Rows.Count; i++)
            {
                if (Convert.ToBoolean(dsSourceKHang.Tables["VKH_KHANG_LQUAN"].Rows[i]["CHON"]) == true)
                {
                    uc.dtSource.ImportRow(dsSourceKHang.Tables["VKH_KHANG_LQUAN"].Rows[i]);
                }
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
            window.Title = LLanguage.SearchResourceByKey("U.POPUP.POPUP_KH_LIEN_QUAN");
            window.ShowDialog();
            if (uc.dtSource != null)
            {
                DataTable dt = dsSourceKHang.Tables["VKH_KHANG_LQUAN"];
                ImportRows(ref dt, uc.dtSource);
            }
            uc = null;
            grTTCoBanHoGD.Rebind();
        }

        /// <summary>
        /// Xóa khách hàng liên quan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbDeleteKH_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int rowCount = dsSourceKHang.Tables["VKH_KHANG_LQUAN"].Rows.Count;
                for (int i = rowCount - 1; i >= 0; i--)
                {
                    if (Convert.ToBoolean(dsSourceKHang.Tables["VKH_KHANG_LQUAN"].Rows[i]["CHON"]) == true)
                    {
                        dsSourceKHang.Tables["VKH_KHANG_LQUAN"].Rows.RemoveAt(i);
                    }
                }
                for (int i = 0; i < dsSourceKHang.Tables["VKH_KHANG_LQUAN"].Rows.Count; i++)
                {
                    dsSourceKHang.Tables["VKH_KHANG_LQUAN"].Rows[i]["STT"] = i + 1;
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(CustomException))
                {
                    new frmThongBaoLoi(ex.Message, ex.InnerException).ShowDialog();
                }
                else if (ex.InnerException.GetType() == typeof(CustomException))
                {
                    new frmThongBaoLoi(ex.InnerException.Message, ex.InnerException).ShowDialog();
                }
                else
                {
                    new frmThongBaoLoi("M.DungChung.LoiXoaDuLieu", ex).ShowDialog();
                }
            }
        }
        #endregion

        #region Giấy tờ liên quan
        /// <summary>
        /// Thêm mới giấy tờ liên quan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbAddGiayTo_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            ucGiayToLienQuan uc = new ucGiayToLienQuan();
            window.Content = uc;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.RenderSize = new Size(1024, 768);
            window.Title = LLanguage.SearchResourceByKey("U.POPUP.POPUP_GIAYTO_LIEN_QUAN");
            window.ShowDialog();
            DataTable dtGiaDinh = dsSourceKHang.Tables["VKH_GTO_LQUAN"];
            if (uc.dtSource != null)
            {
                ImportRows(ref dtGiaDinh, uc.dtSource);
            }
            uc = null;
            grGiayTo.Rebind();
        }

        /// <summary>
        /// Sửa thông tin giấy tờ liên quan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbModifyGiayTo_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            ucGiayToLienQuan uc = new ucGiayToLienQuan();
            for (int i = 0; i < dsSourceKHang.Tables["VKH_GTO_LQUAN"].Rows.Count; i++)
            {
                if (Convert.ToBoolean(dsSourceKHang.Tables["VKH_GTO_LQUAN"].Rows[i]["CHON"]) == true)
                {
                    uc.dtSource.ImportRow(dsSourceKHang.Tables["VKH_GTO_LQUAN"].Rows[i]);
                }
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
            window.Title = LLanguage.SearchResourceByKey("U.POPUP.POPUP_GIAYTO_LIEN_QUAN");
            window.ShowDialog();
            if (uc.dtSource != null)
            {
                DataTable dt = dsSourceKHang.Tables["VKH_GTO_LQUAN"];
                ImportRows(ref dt, uc.dtSource);
            }
            uc = null;
            grGiayTo.Rebind();
        }

        /// <summary>
        /// Xóa giấy tờ liên quan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbDeleteGiayTo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int rowCount = dsSourceKHang.Tables["VKH_GTO_LQUAN"].Rows.Count;
                for (int i = rowCount - 1; i >= 0; i--)
                {
                    if (Convert.ToBoolean(dsSourceKHang.Tables["VKH_GTO_LQUAN"].Rows[i]["CHON"]) == true)
                    {
                        dsSourceKHang.Tables["VKH_GTO_LQUAN"].Rows.RemoveAt(i);
                    }
                }
                for (int i = 0; i < dsSourceKHang.Tables["VKH_GTO_LQUAN"].Rows.Count; i++)
                {
                    dsSourceKHang.Tables["VKH_GTO_LQUAN"].Rows[i]["STT"] = i + 1;
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Tài khoản TCTD khác
        private void tlbAddTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            ucTaiKhoanTCTDKhac uc = new ucTaiKhoanTCTDKhac();
            window.Content = uc;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.RenderSize = new Size(1024, 768);
            window.Title = LLanguage.SearchResourceByKey("U.POPUP.POPUP_TAIKHOAN_TCTD_KHAC");
            window.ShowDialog();
            DataTable dtGiaDinh = dsSourceKHang.Tables["VKH_TKHOAN_NHANG"];
            if (uc.dtSource != null)
            {
                ImportRows(ref dtGiaDinh, uc.dtSource);
            }
            uc = null;
            grGiayTo.Rebind();
        }

        private void tlbModifyTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            ucTaiKhoanTCTDKhac uc = new ucTaiKhoanTCTDKhac();
            for (int i = 0; i < dsSourceKHang.Tables["VKH_TKHOAN_NHANG"].Rows.Count; i++)
            {
                if (Convert.ToBoolean(dsSourceKHang.Tables["VKH_TKHOAN_NHANG"].Rows[i]["CHON"]) == true)
                {
                    uc.dtSource.ImportRow(dsSourceKHang.Tables["VKH_TKHOAN_NHANG"].Rows[i]);
                }
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
            window.Title = LLanguage.SearchResourceByKey("U.POPUP.POPUP_TAIKHOAN_TCTD_KHAC");
            window.ShowDialog();
            if (uc.dtSource != null)
            {
                DataTable dt = dsSourceKHang.Tables["VKH_TKHOAN_NHANG"];
                ImportRows(ref dt, uc.dtSource);
            }
            uc = null;
            grGiayTo.Rebind();
        }

        private void tlbDeleteTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int rowCount = dsSourceKHang.Tables["VKH_TKHOAN_NHANG"].Rows.Count;
                for (int i = rowCount - 1; i >= 0; i--)
                {
                    if (Convert.ToBoolean(dsSourceKHang.Tables["VKH_TKHOAN_NHANG"].Rows[i]["CHON"]) == true)
                    {
                        dsSourceKHang.Tables["VKH_TKHOAN_NHANG"].Rows.RemoveAt(i);
                    }
                }
                for (int i = 0; i < dsSourceKHang.Tables["VKH_TKHOAN_NHANG"].Rows.Count; i++)
                {
                    dsSourceKHang.Tables["VKH_TKHOAN_NHANG"].Rows[i]["STT"] = i + 1;
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Dữ liệu hình ảnh
        /// <summary>
        /// Double click chon anh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    }
                }
                catch (Exception ex)
                {
                    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                }
            }
        }

        /// <summary>
        /// Xử lý load image ở client theo đường dẫn truyền vào
        /// </summary>
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

        /// <summary>
        /// Xử lý load image ở server theo tên file truyền vào
        /// </summary>
        /// <param name="imageName"></param>
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

        /// <summary>
        /// Reset image về image mặc định
        /// </summary>
        private void ResetImage(Image img)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri("pack://application:,,,/Utilities.Common;component/Images/Other/picture.png");
            logo.EndInit();
            img.Source = logo;
            img.Tag = "";
        }

        private void ResetDuLieuHinhAnh()
        {
            ResetImage(imgAvatar);
            txtMaHinhAnh.Text = "";
            txtChuKyKH.Text = ""; 
        }

        /// <summary>
        /// Sự kiện thêm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbAddHinhAnh_Click(object sender, RoutedEventArgs e)
        {
            if (ValidationChuKy())
            {
                AutoComboBox au = new AutoComboBox();
                AutoCompleteEntry loai_hinh_anh = au.getEntryByDisplayName(lstSourceLoaiHinhAnh, ref cmbLoaiHinhAnh);
                dsSourceKHang.Tables["VKH_CKY_HANH"].Rows.Add(false, grDLieuHAnh.Items.Count + 1, -1, -1, loai_hinh_anh.KeywordStrings[0], "", txtChuKyKH.Text, chkHieuLuc.IsChecked, imgAvatar.Tag.ToString(), false, cmbLoaiHinhAnh.Text, "CLIENT");
            }
        }

        /// <summary>
        /// Sự kiện sửa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbModifyHinhAnh_Click(object sender, RoutedEventArgs e)
        {
            if (ValidationChuKy())
            {
                DataRowView dr = (DataRowView)grDLieuHAnh.SelectedItem;
                dr["CKHA_LOAI"] = "";
                dr["CKHA_LOAI_TEXT"] = cmbLoaiHinhAnh.Text;
                dr["CKHA_DTUONG"] = txtChuKyKH.Text;
                if (dr["CKHA_DUONG_DAN"] != imgAvatar.Tag.ToString())
                {
                    dr["CKHA_VI_TRI"] = "CLIENT";
                }
                dr["CKHA_DUONG_DAN"] = imgAvatar.Tag.ToString();
            }
        }

        /// <summary>
        /// Sự kiện xóa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbDeleteHinhAnh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = dsSourceKHang.Tables["VKH_CKY_HANH"].Rows.Count - 1; i >= 0; i--)
                {
                    if (Convert.ToBoolean(dsSourceKHang.Tables["VKH_CKY_HANH"].Rows[i]["CHON"]) == true)
                    {
                        if (Convert.ToBoolean(dsSourceKHang.Tables["VKH_CKY_HANH"].Rows[i]["CKHA_HIEN_THI_HS"]) == true)
                        {
                            ResetImage(imgKhachHang);
                        }
                        dsSourceKHang.Tables["VKH_CKY_HANH"].Rows.RemoveAt(i);
                    }
                }

                for (int i = 0; i < dsSourceKHang.Tables["VKH_CKY_HANH"].Rows.Count; i++)
                {
                    dsSourceKHang.Tables["VKH_CKY_HANH"].Rows[i]["STT"] = i + 1;
                }
            }
            catch (System.Exception ex)
            {

            }
        }

        /// <summary>
        /// Kiểm tra dữ liệu
        /// </summary>
        /// <returns></returns>
        private bool ValidationChuKy()
        {
            AutoComboBox au = new AutoComboBox();
            AutoCompleteEntry loai_hinh_anh = au.getEntryByDisplayName(lstSourceLoaiHinhAnh, ref cmbLoaiHinhAnh);
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri("pack://application:,,,/Utilities.Common;component/Images/Other/picture.png");
            logo.EndInit();

            if (loai_hinh_anh == null)
            {
                CommonFunction.ThongBaoTrong(lblLoaiHinhAnh.Content.ToString());
                cmbLoaiHinhAnh.Focus();
                return false;
            }
            else if (LString.IsNullOrEmptyOrSpace(txtChuKyKH.Text))
            {
                CommonFunction.ThongBaoTrong(lblChuKyHinhAnh.Content.ToString());
                txtChuKyKH.Focus();
                return false;
            }
            else if (imgAvatar.Source == null || imgAvatar.Tag == null || LString.IsNullOrEmptyOrSpace(imgAvatar.Tag.ToString()))
            {
                LMessage.ShowMessage("M.KhachHang.ucKhachHangThanhVien.PhaiChonAnhTruocKhiLuu", LMessage.MessageBoxType.Warning);
                return false;
            }
            return true;
        }

        private void grDLieuHAnh_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (grDLieuHAnh.SelectedItem != null)
            {
                DataRowView dr = (DataRowView)grDLieuHAnh.SelectedItem;
                txtMaHinhAnh.Text = dr["CKHA_MA"].ToString();
                cmbLoaiHinhAnh.SelectedIndex = lstSourceLoaiHinhAnh.IndexOf(lstSourceLoaiHinhAnh.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["CKHA_LOAI"].ToString())));
                txtChuKyKH.Text = dr["CKHA_DTUONG"].ToString();
                chkHieuLuc.IsChecked = Convert.ToBoolean(dr["CKHA_HIEU_LUC"]);
                if (dr["CKHA_VI_TRI"].ToString() == "CLIENT")
                {
                    LoadImageInClient(dr["CKHA_DUONG_DAN"].ToString(), imgAvatar);
                }
                else
                {
                    LoadImageInServer(dr["CKHA_DUONG_DAN"].ToString(), imgAvatar);
                }
            }
        }

        /// <summary>
        /// Sự kiện check all
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dsSourceKHang.Tables["VKH_CKY_HANH"].Rows.Count; i++)
            {
                dsSourceKHang.Tables["VKH_CKY_HANH"].Rows[i]["CHON"] = chkAll.IsChecked;
            }
        }

        /// <summary>
        /// Check anh hien thi ho so
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            Telerik.Windows.Controls.GridView.GridViewRow parentRow = radioButton.ParentOfType<Telerik.Windows.Controls.GridView.GridViewRow>();
            if (parentRow != null)
            {
                parentRow.IsSelected = true;
            }
            DataRowView dr = (DataRowView)grDLieuHAnh.SelectedItem;
            if (dr["CKHA_VI_TRI"].ToString().Equals("CLIENT"))
            {
                LoadImageInClient(dr["CKHA_DUONG_DAN"].ToString(), imgAvatar);
                imgKhachHang.Source = imgAvatar.Source;
            }
            else
            {
                LoadImageInServer(dr["CKHA_DUONG_DAN"].ToString(), imgAvatar);
                imgKhachHang.Source = imgAvatar.Source;
            }
        }
        #endregion

        #region Người đại diện
        /// <summary>
        /// Thêm mới giấy tờ liên quan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbAddNguoiDaiDien_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            ucNguoiDaiDien uc = new ucNguoiDaiDien();
            window.Content = uc;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.RenderSize = new Size(1024, 768);
            window.Title = LLanguage.SearchResourceByKey("U.POPUP.POPUP_NGUOI_DAI_DIEN");
            window.ShowDialog();
            DataTable dtGiaDinh = dsSourceKHang.Tables["VKH_NGUOI_DDIEN"];
            if (uc.dtSource != null)
            {
                ImportRows(ref dtGiaDinh, uc.dtSource);
            }
            uc = null;
            grNguoiDaiDien.Rebind();
        }

        /// <summary>
        /// Sửa thông tin người đại diện
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbModifyNguoiDaiDien_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            ucNguoiDaiDien uc = new ucNguoiDaiDien();
            for (int i = 0; i < dsSourceKHang.Tables["VKH_NGUOI_DDIEN"].Rows.Count; i++)
            {
                if (Convert.ToBoolean(dsSourceKHang.Tables["VKH_NGUOI_DDIEN"].Rows[i]["CHON"]) == true)
                {
                    uc.dtSource.ImportRow(dsSourceKHang.Tables["VKH_NGUOI_DDIEN"].Rows[i]);
                }
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
                DataTable dt = dsSourceKHang.Tables["VKH_NGUOI_DDIEN"];
                ImportRows(ref dt, uc.dtSource);
            }
            uc = null;
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
                int rowCount = dsSourceKHang.Tables["VKH_NGUOI_DDIEN"].Rows.Count;
                for (int i = rowCount - 1; i >= 0; i--)
                {
                    if (Convert.ToBoolean(dsSourceKHang.Tables["VKH_NGUOI_DDIEN"].Rows[i]["CHON"]) == true)
                    {
                        dsSourceKHang.Tables["VKH_NGUOI_DDIEN"].Rows.RemoveAt(i);
                    }
                }
                for (int i = 0; i < dsSourceKHang.Tables["VKH_NGUOI_DDIEN"].Rows.Count; i++)
                {
                    dsSourceKHang.Tables["VKH_NGUOI_DDIEN"].Rows[i]["STT"] = i + 1;
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
        #endregion

        /// <summary>
        /// Lấy dữ liệu khách hàng theo id
        /// </summary>
        private void SetFormData()
        {
            Presentation.Process.KhachHangProcess process = new Presentation.Process.KhachHangProcess();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                dsSourceKHang = process.getThongTinKHTheoID(_idKhachHang);
                if (dsSourceKHang != null && dsSourceKHang.Tables.Count > 0)
                {
                    //Dữ liệu thông tin chung
                    DataRow dr = dsSourceKHang.Tables["KH_KHANG_HSO"].Rows[0];
                    if (dr["ID_DON_VI"] != null && !LString.IsNullOrEmptyOrSpace(dr["ID_DON_VI"].ToString()))
                    {
                        idPhongGD = dr["ID_DON_VI"].ToString();
                        cmbPhongGD.SelectedIndex = lstSourcePhongGD.FindIndex(f => f.KeywordStrings[0].Equals(dr["MA_DVI_TAO"]));
                    }
                    idKhuVuc = dr["ID_KHU_VUC"].ToString();
                    if (dr["ID_CUM"] != null && !LString.IsNullOrEmptyOrSpace(dr["ID_CUM"].ToString()))
                    {
                        idCum = dr["ID_CUM"].ToString();
                        txtCumTVien.Tag = dr["MA_CUM"].ToString();
                        txtCumTVien.Text = dr["TEN_CUM"].ToString();
                    }
                    if (dr["ID_NHOM"] != null && !LString.IsNullOrEmptyOrSpace(dr["ID_NHOM"].ToString()))
                    {
                        txtNhomTVien.Tag = dr["ID_NHOM"].ToString();
                        txtNhomTVien.Text = dr["MA_NHOM"].ToString();
                        lblTenNhom.Content = dr["TEN_NHOM"].ToString();
                    }

                    MaKhachHang = txtMaKhachHang.Text = dr["MA_KHANG"].ToString();
                    txtTenKhachHang.Text = dr["TEN_KHANG"].ToString();
                    cmbLoaiKhachHang.SelectedIndex = lstSourceLoaiKH.IndexOf(lstSourceLoaiKH.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["MA_KHANG_LOAI"].ToString())));

                    if (!LString.IsNullOrEmptyOrSpace(dr["NGAY_THAM_GIA"].ToString()))
                    {
                        raddtNgayTGiaTC.Value = LDateTime.StringToDate(dr["NGAY_THAM_GIA"].ToString(), "yyyyMMdd");
                        //dtpNgayThamGiaTC.SelectedDate = LDateTime.StringToDate(dr["NGAY_THAM_GIA"].ToString(), "yyyyMMdd");
                    }
                    else
                    {
                        raddtNgayTGiaTC.Value = null;
                    }

                    if (!LString.IsNullOrEmptyOrSpace(dr["NGAY_THANH_LAP"].ToString()))
                    {
                        raddtNgayThanhLap.Value = LDateTime.StringToDate(dr["NGAY_THANH_LAP"].ToString(), "yyyyMMdd");
                    }
                    else
                    {
                        raddtNgayThanhLap.Value = null;
                    }

                    if (!LString.IsNullOrEmptyOrSpace(dr["NGAY_HET_HLUC"].ToString()))
                    {
                        chkKHHieuLuc.IsChecked = true;
                        bHetHLuc = true;
                        raddtNgayHetHL.Value = LDateTime.StringToDate(dr["NGAY_HET_HLUC"].ToString(), "yyyyMMdd");
                    }
                    else
                    {
                        raddtNgayHetHL.Value = null;
                    }

                    if (dr["MA_LY_DO"] != null && !LString.IsNullOrEmptyOrSpace(dr["MA_LY_DO"].ToString()))
                    {
                        cmbLyDoRaKhoiNhom.SelectedIndex = lstSourceLyDoRa.IndexOf(lstSourceLyDoRa.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["MA_LY_DO"].ToString())));
                    }

                    txtSoTheTVien.Text = dr["MA_TVIEN"].ToString();
                    ngayCongNhan = dr["NGAY_CONG_NHAN"].ToString();
                    if (!LString.IsNullOrEmptyOrSpace(dr["DD_NGAY_SINH"].ToString()))
                    {
                        raddtNgaySinh.Value = LDateTime.StringToDate(dr["DD_NGAY_SINH"].ToString(), "yyyyMMdd");
                    }
                    else
                    {
                        raddtNgaySinh.Value = null;
                    }

                    txtSoCMND.Text = dr["DD_GTLQ_SO"].ToString();

                    if (!LString.IsNullOrEmptyOrSpace(dr["DD_GTLQ_NGAY_CAP"].ToString()))
                    {
                        raddtNgayCap.Value = LDateTime.StringToDate(dr["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd");
                    }
                    else
                    {
                        raddtNgayCap.Value = null;
                    }

                    if (!LString.IsNullOrEmptyOrSpace(dr["DD_GTLQ_NOI_CAP"].ToString()))
                    {
                        cmbNoiCap.SelectedIndex = lstSourceNoiCap.IndexOf(lstSourceNoiCap.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["DD_GTLQ_NOI_CAP"].ToString())));
                    }
                    else
                    {
                        cmbNoiCap.SelectedIndex = -1;
                    }

                    if (!LString.IsNullOrEmptyOrSpace(dr["DD_GIOI_TINH"].ToString()))
                    {
                        cmbGioiTinh.SelectedIndex = lstSourceGioiTinh.IndexOf(lstSourceGioiTinh.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["DD_GIOI_TINH"].ToString())));
                    }
                    else
                    {
                        cmbGioiTinh.SelectedIndex = -1;
                    }

                    if (!LString.IsNullOrEmptyOrSpace(dr["DD_MA_DAN_TOC"].ToString()))
                    {
                        cmbDanToc.SelectedIndex = lstSourceDanToc.IndexOf(lstSourceDanToc.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["DD_MA_DAN_TOC"].ToString())));
                    }
                    else
                    {
                        cmbDanToc.SelectedIndex = -1;
                    }

                    if (!LString.IsNullOrEmptyOrSpace(dr["DD_MA_QUOC_TICH"].ToString()))
                    {
                        cmbQuocTich.SelectedIndex = lstSourceQuocTich.IndexOf(lstSourceQuocTich.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["DD_MA_QUOC_TICH"].ToString())));
                    }
                    else
                    {
                        cmbQuocTich.SelectedIndex = -1;
                    }
                    if (!LString.IsNullOrEmptyOrSpace(dr["MA_TTRANG_CUTRU"].ToString()))
                    {
                        cmbTinhTrangCuTru.SelectedIndex = lstSourceTTCuTru.IndexOf(lstSourceTTCuTru.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["MA_TTRANG_CUTRU"].ToString())));
                    }
                    txtMaSoThue.Text = dr["MA_SO_THUE"].ToString();

                    //Nguon von duyet so tiet kiem
                    if (!LString.IsNullOrEmptyOrSpace(dr["NV_LOAI_NVON"].ToString()))
                    {
                        cmbNguonVon.SelectedIndex = lstSourceNguonVon.IndexOf(lstSourceNguonVon.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["NV_LOAI_NVON"].ToString())));
                    }

                    //Loai hinh to chuc
                    //if(!LString.IsNullOrEmptyOrSpace(dr[""].ToString()))
                    //{
                    //    cmbTinhTrangCuTru.SelectedIndex = lstSourceTTCuTru.IndexOf(lstSourceTTCuTru.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["MA_TTRANG_CUTRU"].ToString())));
                    //}

                    if (!LString.IsNullOrEmptyOrSpace(dr["MA_LOAI_HINH_KT"].ToString()))
                    {
                        cmbLoaiHinhTC.SelectedIndex = lstSourceLHToChuc.IndexOf(lstSourceLHToChuc.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["MA_LOAI_HINH_KT"].ToString())));
                    }

                    if (!LString.IsNullOrEmptyOrSpace(dr["MA_NGANH_KT"].ToString()))
                    {
                        cmbNganhKinhTe.SelectedIndex = lstSourceNganhKT.IndexOf(lstSourceNganhKT.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["MA_NGANH_KT"].ToString())));
                    }

                    // Thong tin to chuc
                    if (!LString.IsNullOrEmptyOrSpace(dr["TONG_TAI_SAN"].ToString()))
                    {
                        txtTongTS.Value = Convert.ToDouble(dr["TONG_TAI_SAN"]);
                    }
                    if (!LString.IsNullOrEmptyOrSpace(dr["VON_DIEU_LE"].ToString()))
                    {
                        txtVonDieuLe.Value = Convert.ToDouble(dr["VON_DIEU_LE"]);
                    }

                    // Hộ khẩu thường trú
                    txtDiaChi.Text = dr["DD_TTRU_DIA_CHI"].ToString();
                    if (!LString.IsNullOrEmptyOrSpace(dr["DD_TTRU_MA_TINHTP"].ToString()))
                    {
                        cmbHKTinhTP.SelectedIndex = lstSourceHKTinhTP.IndexOf(lstSourceHKTinhTP.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["DD_TTRU_MA_TINHTP"].ToString())));
                    }
                    if (!LString.IsNullOrEmptyOrSpace(dr["DD_TTRU_MA_QUAN"].ToString()))
                    {
                        cmbHKQuanHuyen.SelectedIndex = lstSourceHKQuanHuyen.IndexOf(lstSourceHKQuanHuyen.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["DD_TTRU_MA_QUAN"].ToString())));
                    }
                    if (!LString.IsNullOrEmptyOrSpace(dr["DD_TTRU_MA_PHUONG"].ToString()))
                    {
                        cmbHKXaPhuong.SelectedIndex = lstSourceHKXaPhuong.IndexOf(lstSourceHKXaPhuong.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["DD_TTRU_MA_PHUONG"].ToString())));
                    }

                    // Địa chỉ hiện tại
                    txtDiaChiHienTai.Text = dr["DIA_CHI"].ToString();
                    if (!LString.IsNullOrEmptyOrSpace(dr["MA_TINHTP"].ToString()))
                    {
                        cmbTinhTPHienTai.SelectedIndex = lstSourceTinhTp.IndexOf(lstSourceTinhTp.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["MA_TINHTP"].ToString())));
                    }
                    if (!LString.IsNullOrEmptyOrSpace(dr["MA_QUAN"].ToString()))
                    {
                        cmbQuanHuyenHienTai.SelectedIndex = lstSourceQuanHuyen.IndexOf(lstSourceQuanHuyen.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["MA_QUAN"].ToString())));
                    }
                    if (!LString.IsNullOrEmptyOrSpace(dr["MA_PHUONG"].ToString()))
                    {
                        cmbXaPhuongHienTai.SelectedIndex = lstSourceXaPhuong.IndexOf(lstSourceXaPhuong.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["MA_PHUONG"].ToString())));
                    }

                    // SDT và Email
                    txtSoCoDinh.Text = dr["SO_DTHOAI"].ToString();
                    txtSoDiDong.Text = dr["SO_DDONG"].ToString();
                    txtEmail.Text = dr["EMAIL"].ToString();

                    //Thong tin khac
                    if (!LString.IsNullOrEmptyOrSpace(dr["DD_MA_TTRANG_HNHAN"].ToString()))
                    {
                        cmbHonNhan.SelectedIndex = lstSourceHonNhan.IndexOf(lstSourceHonNhan.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["DD_MA_TTRANG_HNHAN"].ToString())));
                    }
                    if (!LString.IsNullOrEmptyOrSpace(dr["DD_MA_TDO_HVAN"].ToString()))
                    {
                        cmbHocVan.SelectedIndex = lstSourceHocVan.IndexOf(lstSourceHocVan.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["DD_MA_TDO_HVAN"].ToString())));
                    }
                    if (!LString.IsNullOrEmptyOrSpace(dr["DD_MA_VTRO_GDINH"].ToString()))
                    {
                        cmbVaiTroGD.SelectedIndex = lstSourceVaiTroGD.IndexOf(lstSourceVaiTroGD.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["DD_MA_VTRO_GDINH"].ToString())));
                    }
                    if (!LString.IsNullOrEmptyOrSpace(dr["DD_MA_LHINH_CTAC"].ToString()))
                    {
                        cmbLHCongTac.SelectedIndex = lstSourceLHinhCongTac.IndexOf(lstSourceLHinhCongTac.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["DD_MA_LHINH_CTAC"].ToString())));
                    }
                    if (!LString.IsNullOrEmptyOrSpace(dr["DD_MA_TGIAN_CTAC"].ToString()))
                    {
                        cmbTGCongTac.SelectedIndex = lstSourceTGianCongTac.IndexOf(lstSourceTGianCongTac.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["DD_MA_TGIAN_CTAC"].ToString())));
                    }
                    if (!LString.IsNullOrEmptyOrSpace(dr["DDAN_HINH_ANH"].ToString()))
                    {
                        LoadImageInServer(dr["DDAN_HINH_ANH"].ToString(), imgKhachHang);
                    }

                    // Thông tin kiểm soát
                    txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(dsSourceKHang.Tables["KH_KHANG_HSO"].Rows[0]["TTHAI_NVU"].ToString());
                    lblTrangThai.Content = txtTrangThai.Text;
                    txtNguoiLap.Text = dsSourceKHang.Tables["KH_KHANG_HSO"].Rows[0]["NGUOI_NHAP"].ToString();
                    raddtNgayNhap.Value = LDateTime.StringToDate(dsSourceKHang.Tables["KH_KHANG_HSO"].Rows[0]["NGAY_NHAP"].ToString(), "yyyyMMdd");
                    if (dsSourceKHang.Tables["KH_KHANG_HSO"].Rows[0]["NGUOI_CNHAT"] != null && !LString.IsNullOrEmptyOrSpace(dsSourceKHang.Tables["KH_KHANG_HSO"].Rows[0]["NGUOI_CNHAT"].ToString()))
                    {
                        txtNguoiCapNhat.Text = dsSourceKHang.Tables["KH_KHANG_HSO"].Rows[0]["NGUOI_CNHAT"].ToString();
                    }
                    if (dsSourceKHang.Tables["KH_KHANG_HSO"].Rows[0]["NGAY_CNHAT"] != null && !LString.IsNullOrEmptyOrSpace(dsSourceKHang.Tables["KH_KHANG_HSO"].Rows[0]["NGAY_CNHAT"].ToString()))
                    {
                        raddtNgayCNhat.Value = LDateTime.StringToDate(dsSourceKHang.Tables["KH_KHANG_HSO"].Rows[0]["NGAY_CNHAT"].ToString(), "yyyyMMdd");
                    }

                    // Nhà cửa và phương tiện đi lại
                    if (dsSourceKHang.Tables["VKH_NHA_O"] != null && dsSourceKHang.Tables["VKH_NHA_O"].Rows.Count > 0)
                    {
                        if (!LString.IsNullOrEmptyOrSpace(dsSourceKHang.Tables["VKH_NHA_O"].Rows[0]["NO_NOI_CU_TRU"].ToString()))
                        {
                            cmbNoiCuTru.SelectedIndex = lstSourceNoiCuTru.IndexOf(lstSourceNoiCuTru.FirstOrDefault(f => f.KeywordStrings.First().Equals(dsSourceKHang.Tables["VKH_NHA_O"].Rows[0]["NO_NOI_CU_TRU"].ToString())));
                        }
                        if (!LString.IsNullOrEmptyOrSpace(dsSourceKHang.Tables["VKH_NHA_O"].Rows[0]["NO_THOI_GIAN_CU_TRU"].ToString()))
                        {
                            cmbTGianCuTru.SelectedIndex = lstSourceTGCuTru.IndexOf(lstSourceTGCuTru.FirstOrDefault(f => f.KeywordStrings.First().Equals(dsSourceKHang.Tables["VKH_NHA_O"].Rows[0]["NO_THOI_GIAN_CU_TRU"].ToString())));
                        }

                        // Kích cỡ nhà
                        if (dsSourceKHang.Tables["VKH_NHA_O"].Rows[0]["NO_KICH_CO_NHA"] != null && !LString.IsNullOrEmptyOrSpace(dsSourceKHang.Tables["VKH_NHA_O"].Rows[0]["NO_KICH_CO_NHA"].ToString()))
                        {
                            switch (dsSourceKHang.Tables["VKH_NHA_O"].Rows[0]["NO_KICH_CO_NHA"].ToString())
                            {
                                case "NHO":
                                    rdoKCoNhaNho.IsChecked = true;
                                    break;
                                case "TRUNG_BINH":
                                    rdoKCoNhaTB.IsChecked = true;
                                    break;
                                case "TO":
                                    rdoKCoNhaTo.IsChecked = true;
                                    break;
                                case "KHAC":
                                    rdoKichCoNhaKhac.IsChecked = true;
                                    break;
                            }
                        }

                        // Tường nhà
                        if (dsSourceKHang.Tables["VKH_NHA_O"].Rows[0]["NO_TUONG_NHA"] != null && !LString.IsNullOrEmptyOrSpace(dsSourceKHang.Tables["VKH_NHA_O"].Rows[0]["NO_TUONG_NHA"].ToString()))
                        {
                            switch (dsSourceKHang.Tables["VKH_NHA_O"].Rows[0]["NO_TUONG_NHA"].ToString())
                            {
                                case "DAT_COT_EP":
                                    rdoTNhaDat.IsChecked = true;
                                    break;
                                case "TRE_NUA":
                                    rdoKTNhaVNua.IsChecked = true;
                                    break;
                                case "GACH_GO":
                                    rdoKTNGach.IsChecked = true;
                                    break;
                                case "KHAC":
                                    rdoTuongNhaKhac.IsChecked = true;
                                    break;
                            }
                        }

                        // Chất lợp mái nhà
                        if (dsSourceKHang.Tables["VKH_NHA_O"].Rows[0]["NO_CHAT_LOP_MAI"] != null && !LString.IsNullOrEmptyOrSpace(dsSourceKHang.Tables["VKH_NHA_O"].Rows[0]["NO_CHAT_LOP_MAI"].ToString()))
                        {
                            switch (dsSourceKHang.Tables["VKH_NHA_O"].Rows[0]["NO_CHAT_LOP_MAI"].ToString())
                            {
                                case "TRANH_LA":
                                    rdoCLopTranh.IsChecked = true;
                                    break;
                                case "TON":
                                    rdoCLopTon.IsChecked = true;
                                    break;
                                case "MAI_BANG":
                                    rdoCLopMai.IsChecked = true;
                                    break;
                                case "KHAC":
                                    rdoCLopMaiKhac.IsChecked = true;
                                    break;
                            }
                        }

                        // Mức độ bền chắc
                        if (dsSourceKHang.Tables["VKH_NHA_O"].Rows[0]["NO_MUC_DO_BEN"] != null && !LString.IsNullOrEmptyOrSpace(dsSourceKHang.Tables["VKH_NHA_O"].Rows[0]["NO_MUC_DO_BEN"].ToString()))
                        {
                            switch (dsSourceKHang.Tables["VKH_NHA_O"].Rows[0]["NO_MUC_DO_BEN"].ToString())
                            {
                                case "NHA_CAP_4":
                                    rdoMDoCap4.IsChecked = true;
                                    break;
                                case "NHA_CAP_3":
                                    rdoMDoCap3.IsChecked = true;
                                    break;
                                case "NHA_KIEN_CO":
                                    rdoMDoKienCo.IsChecked = true;
                                    break;
                            }
                        }
                    }

                    // Phương tiện đi lại
                    if (dsSourceKHang.Tables["VKH_PTIEN_DLAI"] != null && dsSourceKHang.Tables["VKH_PTIEN_DLAI"].Rows.Count > 0)
                    {
                        switch (dsSourceKHang.Tables["VKH_PTIEN_DLAI"].Rows[0]["PPDL_LOAI_PTIEN"].ToString())
                        {
                            case "PT_CONG_CONG":
                                rdoPTienCongCong.IsChecked = true;
                                break;
                            case "XE_HAI_BANH":
                                rdoPTienXeMay.IsChecked = true;
                                break;
                            case "OTO":
                                rdoPTienOto.IsChecked = true;
                                break;
                            case "KHAC":
                                rdoPTienKhac.IsChecked = true;
                                break;
                        }
                    }

                    // Phương tiện thông tin
                    if (dsSourceKHang.Tables["VKH_PTIEN_TTIN"] != null && dsSourceKHang.Tables["VKH_PTIEN_TTIN"].Rows.Count > 0)
                    {
                        if (dsSourceKHang.Tables["VKH_PTIEN_TTIN"].Rows[0]["PPTT_SDUNG_DTHOAI"] == "CO_DIEN_THOAI")
                        {
                            rdoPTTTCo.IsChecked = true;
                        }
                        else
                        {
                            rdoPTTTKhong.IsChecked = true;
                        }
                    }

                    //Binding dữ liệu khác
                    grTTCoBanHoGD.ItemsSource = dsSourceKHang.Tables["VKH_GIA_DINH"].DefaultView;
                    grGiayTo.ItemsSource = dsSourceKHang.Tables["VKH_GTO_LQUAN"].DefaultView;
                    grNguoiDaiDien.ItemsSource = dsSourceKHang.Tables["VKH_NGUOI_DDIEN"].DefaultView;
                    if (dr["MA_KHANG_LOAI"].ToString().Equals(BusinessConstant.LoaiKhachHang.TVIEN.layGiaTri()))
                    {
                        grKhoanThuNhap.ItemsSource = null;
                        grChiPhi.ItemsSource = null;
                        grKhoanThuNhap.ItemsSource = dsSourceKHang.Tables["VKH_THU_NHAP"].DefaultView;
                        grChiPhi.ItemsSource = dsSourceKHang.Tables["VKH_CHI_PHI"].DefaultView;
                    }
                    grTaiSan.ItemsSource = dsSourceKHang.Tables["VKH_TAI_SAN"];
                    grNTKe.ItemsSource = dsSourceKHang.Tables["VKH_NGUOI_TKE"].DefaultView;
                    grKHLQuan.ItemsSource = dsSourceKHang.Tables["VKH_KHANG_LQUAN"].DefaultView;
                    grTKTCTDKhac.ItemsSource = dsSourceKHang.Tables["VKH_TKHOAN_NHANG"].DefaultView;
                    grDLieuHAnh.ItemsSource = dsSourceKHang.Tables["VKH_CKY_HANH"].DefaultView;
                    //grXepHangTD.ItemsSource = dsSourceKHang.Tables["VKH_XHANG_TDUNG"].DefaultView;
                    //grXepHangNgheo.ItemsSource = dsSourceKHang.Tables["VKH_XHANG_NGHEO"].DefaultView;

                    //Hien thi
                    XuLyGiaoDien(dr["MA_KHANG_LOAI"].ToString());

                    TinhSoDuTietKiem(dr["MA_KHANG_LOAI"].ToString());
                }
            }
            catch (System.Exception ex)
            {
                if (ex.GetType() == typeof(CustomException))
                {
                    new frmThongBaoLoi(ex.Message, ex.InnerException).ShowDialog();
                }
                else if (ex.InnerException.GetType() == typeof(CustomException))
                {
                    new frmThongBaoLoi(ex.InnerException.Message, ex.InnerException).ShowDialog();
                }
                else
                {
                    new frmThongBaoLoi("M.DungChung.LoiLoadDuLieu", ex).ShowDialog();
                }
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Tìm id min trong datatable
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private int findMinId(DataTable dt)
        {
            DataRow[] dr = dt.Select("ID=MIN(ID)");
            if (dr.Length > 0)
            {
                return Convert.ToInt32(dr[0]["ID"]);
            }
            return 0;
        }

        /// <summary>
        /// Reset form
        /// </summary>
        private void ResetForm()
        {
            // Tham so
            _idKhachHang = -1;
            idKhuVuc = "";
            idCum = "";
            KhoiTaoDataTable();
            // Thong tin chung
            txtMaKhachHang.Text = "";
            txtTenKhachHang.Text = "";
            raddtNgayTGiaTC.Value = LDateTime.StringToDate(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai,"yyyyMMdd");
            raddtNgayHetHL.Value = null;
            cmbLyDoRaKhoiNhom.SelectedIndex = -1;
            txtNhomTVien.Text = "";
            txtNhomTVien.Tag = null;
            txtCumTVien.Text = "";
            txtCumTVien.Tag = null;
            lblTrangThai.Content = tthaiNvu = "";
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KH_THANH_VIEN);
        }

        /// <summary>
        /// Xử lý giao diện
        /// </summary>
        /// <param name="loaiKH"></param>
        private void XuLyGiaoDien(string loaiKH)
        {
            if (loaiKH == ApplicationConstant.LoaiKhachHang.CA_NHAN.layGiaTri())
            {
                _function = DatabaseConstant.Function.KH_CA_NHAN;
                // An
                lblPhongGD.Visibility = Visibility.Visible;
                cmbPhongGD.Visibility = Visibility.Visible;
                gridThongTinChung.RowDefinitions[9].Height = new GridLength(0);
                gridThongTinChung.RowDefinitions[10].Height = new GridLength(0);
                raddtNgayThanhLap.IsEnabled = false;
                dtpThanhLap.IsEnabled = false;

                gridThongTinChung.RowDefinitions[13].Height = new GridLength(0);
                gridThongTinChung.RowDefinitions[14].Height = new GridLength(0);
                btnNhomTVien.IsEnabled = false;


                gridThongTinChung.RowDefinitions[25].Height = new GridLength(0);
                gridThongTinChung.RowDefinitions[26].Height = new GridLength(0);
                cmbLoaiHinhTC.IsEnabled = false;
                cmbNganhKinhTe.IsEnabled = false;

                stackNgaySinh.SetValue(Grid.ColumnProperty, 0);
                raddtNgaySinh.SetValue(Grid.ColumnProperty, 1);
                dtpNgaySinh.SetValue(Grid.ColumnProperty, 1);
                grbGiayToLQ.SetValue(Grid.RowProperty, 0);
                chkMPA.SetValue(Grid.RowProperty, 2);

                tabGiayToLQ.Header = LLanguage.SearchResourceByKey("U.KhachHang.KhachHang.ucKhachHangThanhVien.TabGiayToLienQuan");

                grbGiaDinh.Visibility = Visibility.Collapsed;
                lblSoTheTVien.Visibility = Visibility.Hidden;
                txtSoTheTVien.Visibility = Visibility.Hidden;

                expTaiSanDoanhNghiep.Visibility = Visibility.Collapsed;
                expHoKhau.Visibility = Visibility.Collapsed;
                expTTinKhac.Visibility = Visibility.Collapsed;
                tabNguoiDaiDien.Visibility = Visibility.Collapsed;
                tabThuNhap.Visibility = Visibility.Collapsed;
                tabNhaCua.Visibility = Visibility.Collapsed;
                tabLichSu.Visibility = Visibility.Collapsed;
                chkMPA.Visibility = Visibility.Collapsed;

                // Hien
                gridThongTinChung.RowDefinitions[15].Height = new GridLength(6);
                gridThongTinChung.RowDefinitions[16].Height = new GridLength(1, GridUnitType.Star);

                gridThongTinChung.RowDefinitions[21].Height = new GridLength(6);
                gridThongTinChung.RowDefinitions[22].Height = new GridLength(1, GridUnitType.Star);


                lblSoCMND.Content = LLanguage.SearchResourceByKey("U.KhachHang.KhachHang.ucKhachHangThanhVien.SoCMND:");

                stackNgaySinh.Visibility = Visibility.Visible;
                raddtNgaySinh.Visibility = Visibility.Visible;
                dtpNgaySinh.Visibility = Visibility.Visible;
                lblGioiTinh.Visibility = Visibility.Visible;
                cmbGioiTinh.Visibility = Visibility.Visible;
                //lblRedQuanHienTai.Visibility = Visibility.Visible;
                //lblRedTinhHienTai.Visibility = Visibility.Visible;
                //lblRedXaHienTai.Visibility = Visibility.Visible;

                raddtNgaySinh.IsEnabled = true;
                dtpNgaySinh.IsEnabled = true;
                cmbDanToc.IsEnabled = true;
                cmbQuocTich.IsEnabled = true;
            }
            else if (loaiKH == ApplicationConstant.LoaiKhachHang.THANH_VIEN.layGiaTri())
            {
                _function = DatabaseConstant.Function.KH_THANH_VIEN;
                // An
                lblPhongGD.Visibility = Visibility.Collapsed;
                cmbPhongGD.Visibility = Visibility.Collapsed;
                gridThongTinChung.RowDefinitions[9].Height = new GridLength(0);
                gridThongTinChung.RowDefinitions[10].Height = new GridLength(0);
                raddtNgayThanhLap.IsEnabled = false;
                dtpThanhLap.IsEnabled = false;

                gridThongTinChung.RowDefinitions[25].Height = new GridLength(0);
                gridThongTinChung.RowDefinitions[26].Height = new GridLength(0);
                cmbLoaiHinhTC.IsEnabled = false;
                cmbNganhKinhTe.IsEnabled = false;

                expTaiSanDoanhNghiep.Visibility = Visibility.Collapsed;
                tabNguoiDaiDien.Visibility = Visibility.Collapsed;
                //lblRedQuanHienTai.Visibility = Visibility.Hidden;
                //lblRedTinhHienTai.Visibility = Visibility.Hidden;
                //lblRedXaHienTai.Visibility = Visibility.Hidden;

                // Hien
                gridThongTinChung.RowDefinitions[13].Height = new GridLength(6);
                gridThongTinChung.RowDefinitions[14].Height = new GridLength(1, GridUnitType.Star);
                gridThongTinChung.RowDefinitions[21].Height = new GridLength(6);
                gridThongTinChung.RowDefinitions[22].Height = new GridLength(1, GridUnitType.Star);
                gridThongTinChung.RowDefinitions[15].Height = new GridLength(6);
                gridThongTinChung.RowDefinitions[16].Height = new GridLength(1, GridUnitType.Star);
                tabGiayToLQ.Header = LLanguage.SearchResourceByKey("U.KhachHang.KhachHang.ucKhachHangThanhVien.TabGiaDinhVaGiayToLienQuan");
                lblSoCMND.Content = LLanguage.SearchResourceByKey("U.KhachHang.KhachHang.ucKhachHangThanhVien.SoCMND:");
                stackNgaySinh.Visibility = Visibility.Visible;
                raddtNgaySinh.Visibility = Visibility.Visible;
                dtpNgaySinh.Visibility = Visibility.Visible;
                grbGiaDinh.Visibility = Visibility.Visible;
                grbGiayToLQ.SetValue(Grid.RowProperty, 2);
                chkMPA.SetValue(Grid.RowProperty, 4);
                chkMPA.Visibility = Visibility.Visible;
                tabThuNhap.Visibility = Visibility.Visible;
                tabNhaCua.Visibility = Visibility.Visible;
                tabLichSu.Visibility = Visibility.Visible;
                expHoKhau.Visibility = Visibility.Visible;
                expTTinKhac.Visibility = Visibility.Visible;
                lblSoTheTVien.Visibility = Visibility.Visible;
                txtSoTheTVien.Visibility = Visibility.Visible;
                stackNgaySinh.SetValue(Grid.ColumnProperty, 3);
                raddtNgaySinh.SetValue(Grid.ColumnProperty, 4);
                dtpNgaySinh.SetValue(Grid.ColumnProperty, 4);

                btnNhomTVien.IsEnabled = true;
                raddtNgaySinh.IsEnabled = true;
                dtpNgaySinh.IsEnabled = true;
                cmbDanToc.IsEnabled = true;
                cmbQuocTich.IsEnabled = true;
                       
            }
            else
            {
                _function = DatabaseConstant.Function.KH_TO_CHUC;
                // An
                btnMaKhachHang.Visibility = Visibility.Collapsed;
                lblPhongGD.Visibility = Visibility.Collapsed;
                cmbPhongGD.Visibility = Visibility.Collapsed;
                lblGioiTinh.Visibility = Visibility.Collapsed;
                cmbGioiTinh.Visibility = Visibility.Collapsed;
                lblTinhTrangCuChu.Visibility = Visibility.Collapsed;
                cmbTinhTrangCuTru.Visibility = Visibility.Collapsed;

                gridThongTinChung.RowDefinitions[13].Height = new GridLength(0);
                gridThongTinChung.RowDefinitions[14].Height = new GridLength(0);
                btnNhomTVien.IsEnabled = false;

                gridThongTinChung.RowDefinitions[15].Height = new GridLength(0);
                gridThongTinChung.RowDefinitions[16].Height = new GridLength(0);
                raddtNgaySinh.IsEnabled = false;
                dtpNgaySinh.IsEnabled = false;

                gridThongTinChung.RowDefinitions[21].Height = new GridLength(0);
                gridThongTinChung.RowDefinitions[22].Height = new GridLength(0);
                cmbDanToc.IsEnabled = false;
                cmbQuocTich.IsEnabled = false;

                lblSoCMND.Content = LLanguage.SearchResourceByKey("U.KhachHang.KhachHang.ucKhachHangThanhVien.SoDKKD:");
                expHoKhau.Visibility = Visibility.Collapsed;
                expTTinKhac.Visibility = Visibility.Collapsed;
                tabGiayToLQ.Header = LLanguage.SearchResourceByKey("U.KhachHang.KhachHang.ucKhachHangThanhVien.TabGiayToLienQuan");
                grbGiaDinh.Visibility = Visibility.Collapsed;
                grbGiayToLQ.SetValue(Grid.RowProperty, 0);
                chkMPA.Visibility = Visibility.Collapsed;
                tabThuNhap.Visibility = Visibility.Collapsed;
                tabNhaCua.Visibility = Visibility.Collapsed;
                tabLichSu.Visibility = Visibility.Collapsed;
                stackNgaySinh.Visibility = Visibility.Hidden;
                raddtNgaySinh.Visibility = Visibility.Hidden;
                dtpNgaySinh.Visibility = Visibility.Hidden;

                // Hien
                expTaiSanDoanhNghiep.Visibility = Visibility.Visible;
                tabNguoiDaiDien.Visibility = Visibility.Visible;
                //lblRedQuanHienTai.Visibility = Visibility.Visible;
                //lblRedTinhHienTai.Visibility = Visibility.Visible;
                //lblRedXaHienTai.Visibility = Visibility.Visible;
                gridThongTinChung.RowDefinitions[9].Height = new GridLength(6);
                gridThongTinChung.RowDefinitions[10].Height = new GridLength(1, GridUnitType.Star);
                gridThongTinChung.RowDefinitions[25].Height = new GridLength(6);
                gridThongTinChung.RowDefinitions[26].Height = new GridLength(1, GridUnitType.Star);

                cmbLoaiHinhTC.IsEnabled = true;
                cmbNganhKinhTe.IsEnabled = true;
                raddtNgayThanhLap.IsEnabled = true;
                dtpThanhLap.IsEnabled = true;
            }
        }

        private void TinhSoDuTietKiem(string loaiKH)
        {
            try
            {
                if (loaiKH == ApplicationConstant.LoaiKhachHang.THANH_VIEN.layGiaTri())
                {
                    DataTable dt = null;

                    LDatatable.MakeParameterTable(ref dt);
                    LDatatable.AddParameter(ref dt, "@MA_KHANG", "STRING", txtMaKhachHang.Text);
                    LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", ClientInformation.MaDonViGiaoDich);
                    DataSet ds = new HuyDongVonProcess().GetThongTinSoTGuiTheoMaKH(dt);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        soDuTietKiem = Convert.ToDecimal(ds.Tables[0].Rows[0]["SO_TIEN"]);
                    }
                }
                
            }
            catch (Exception ex)
            {
                LLogging.WriteLog("", LLogging.LogType.BUS, ex);
            }        
        }

        private void chkThongTinCoBanGDinh_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dsSourceKHang.Tables["VKH_GIA_DINH"].Rows.Count; i++)
            {
                dsSourceKHang.Tables["VKH_GIA_DINH"].Rows[i]["CHON"] = chkThongTinCoBanGDinh.IsChecked;
            }
        }

        private void chkGiayToLQuan_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dsSourceKHang.Tables["VKH_GTO_LQUAN"].Rows.Count; i++)
            {
                dsSourceKHang.Tables["VKH_GTO_LQUAN"].Rows[i]["CHON"] = chkGiayToLQuan.IsChecked;
            }
        }

        private void chkNguoiTKe_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dsSourceKHang.Tables["VKH_NGUOI_TKE"].Rows.Count; i++)
            {
                dsSourceKHang.Tables["VKH_NGUOI_TKE"].Rows[i]["CHON"] = chkNguoiTKe.IsChecked;
            }
        }

        private void chkKhachHangLQuan_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dsSourceKHang.Tables["VKH_KHANG_LQUAN"].Rows.Count; i++)
            {
                dsSourceKHang.Tables["VKH_KHANG_LQUAN"].Rows[i]["CHON"] = chkKhachHangLQuan.IsChecked;
            }
        }

        private void chkTaiKhoanAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dsSourceKHang.Tables["VKH_TKHOAN_NHANG"].Rows.Count; i++)
            {
                dsSourceKHang.Tables["VKH_TKHOAN_NHANG"].Rows[i]["CHON"] = chkTaiKhoanAll.IsChecked;
            }
        }

        private void SetEnabledAllControls(bool enable)
        {
            //expander ThongTinChung
            cmbLoaiKhachHang.IsEnabled = enable;
            btnMaKhachHang.IsEnabled = enable;
            txtTenKhachHang.IsEnabled = enable;
            raddtNgayTGiaTC.IsEnabled = enable;
            dtpNgayThamGiaTC.IsEnabled = enable;
            raddtNgayThanhLap.IsEnabled = enable;
            dtpThanhLap.IsEnabled = enable;
            raddtNgayHetHL.IsEnabled = enable;
            dtpNgayHetHL.IsEnabled = enable;
            cmbLyDoRaKhoiNhom.IsEnabled = enable;
            btnNhomTVien.IsEnabled = enable;
            raddtNgaySinh.IsEnabled = enable;
            dtpNgaySinh.IsEnabled = enable;
            txtSoCMND.IsEnabled = enable;
            raddtNgayCap.IsEnabled = enable;
            dtpNgayCap.IsEnabled = enable;
            cmbNoiCap.IsEnabled = enable;
            cmbGioiTinh.IsEnabled = enable;
            cmbQuocTich.IsEnabled = enable;
            cmbDanToc.IsEnabled = enable;
            cmbTinhTrangCuTru.IsEnabled = enable;
            txtMaSoThue.IsEnabled = enable;
            cmbLoaiHinhTC.IsEnabled = enable;
            cmbNganhKinhTe.IsEnabled = enable;
            chkKHHieuLuc.IsEnabled = enable;
            txtNhomTVien.IsEnabled = enable;
            
            // expander ThongTinTaiSanDoanhNghiep
            txtTongTS.IsEnabled = enable;
            cmbTienTeTongTS.IsEnabled = enable;
            txtVonDieuLe.IsEnabled = enable;
            cmbTienTeVonDL.IsEnabled = enable;

            // expander HoKhauThuongTru
            txtDiaChi.IsEnabled = enable;
            cmbHKTinhTP.IsEnabled = enable;
            cmbHKQuanHuyen.IsEnabled = enable;
            cmbHKXaPhuong.IsEnabled = enable;

            // expander DiaChiHienTai
            txtDiaChiHienTai.IsEnabled = enable;
            cmbTinhTPHienTai.IsEnabled = enable;
            cmbQuanHuyenHienTai.IsEnabled = enable;
            cmbXaPhuongHienTai.IsEnabled = enable;

            // expander SDT va Email
            txtSoCoDinh.IsEnabled = enable;
            txtSoDiDong.IsEnabled = enable;
            txtEmail.IsEnabled = enable;

            // expander ThongTinKhac
            cmbHonNhan.IsEnabled = enable;
            cmbHocVan.IsEnabled = enable;
            cmbVaiTroGD.IsEnabled = enable;
            cmbLHCongTac.IsEnabled = enable;
            cmbTGCongTac.IsEnabled = enable;

            // Nguoi dai dien
            grbNguoiDaiDien.IsEnabled = enable;

            // Gia dinh va giay to lien quan
            grbGiaDinh.IsEnabled = enable;
            grbGiayToLQ.IsEnabled = enable;
            chkMPA.IsEnabled = enable;

            // Thu nhap, chi phi va tai san
            grbThuNhap.IsEnabled = enable;
            grbChiPhi.IsEnabled = enable;
            grbTaiSan.IsEnabled = enable;

            // Nha o va phuong tien
            grbNhaO.IsEnabled = enable;
            grbPhuongTienThongTin.IsEnabled = enable;
            grbPhuongTinDiLai.IsEnabled = enable;

            grbKhachHangLienQuan.IsEnabled = enable;
            grbTaiKhoan.IsEnabled = enable;
            grbHinhAnh.IsEnabled = enable;


            //Xử lý khác
            if (enable == true)
            {
                if (soDuTietKiem > 0)
                {
                    chkKHHieuLuc.IsEnabled = false;
                }
                else
                {
                    chkKHHieuLuc.IsEnabled = true;
                }
            }
        }

        private void uccmbGiaTriTS_EditCellEnd(object sender, EventArgs e)
        {
            try
            {
                Telerik.Windows.Controls.GridView.GridViewCell cellEdit = uccmbGiaTriTS.cellEdit;
                DataRow drv = (DataRow)cellEdit.ParentRow.Item;
                int indexofRows = dsSourceKHang.Tables["VKH_TAI_SAN"].Rows.IndexOf(drv);
                string NameColumn = uccmbGiaTriTS.cellEdit.Column.UniqueName;
                string GiaTri = uccmbGiaTriTS.GiaTri;
                dsSourceKHang.Tables["VKH_TAI_SAN"].Rows[indexofRows][NameColumn] = GiaTri;
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void OnPreviewDanhGiaKH()
        {
            Cursor = Cursors.Wait;
            try
            {
                // Cảnh báo khi không có dữ liệu
                if (LObject.IsNullOrEmpty(idKhachHang))
                {
                    LMessage.ShowMessage("M.KhachHang.ucKhachHangThanhVien.KhongCoThongTinKheUoc", LMessage.MessageBoxType.Warning);
                    return;
                }
                else
                {

                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                    Presentation.Process.BaoCaoServiceRef.DoiTuongBaoCao doiTuongBaoCao = new Presentation.Process.BaoCaoServiceRef.DoiTuongBaoCao();

                    Presentation.Process.BaoCaoServiceRef.GIAO_DICH_BASE objGIAO_DICH_BASE = new Presentation.Process.BaoCaoServiceRef.GIAO_DICH_BASE();
                    objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.KHTV_PHIEU_DANHGIA;
                    objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.KH_THANH_VIEN;

                    Presentation.Process.BaoCaoServiceRef.KHTV_PHIEU_DANHGIA objKHTV_PHIEU_DANHGIA = new Presentation.Process.BaoCaoServiceRef.KHTV_PHIEU_DANHGIA();
                    objKHTV_PHIEU_DANHGIA.MA_KHACH_HANG = MaKhachHang;
                    objKHTV_PHIEU_DANHGIA.ID_KHANG = idKhachHang;

                    doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                    doiTuongBaoCao.objKHTV_PHIEU_DANHGIA = objKHTV_PHIEU_DANHGIA;
                    xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
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

        private void OnPreviewKhaoSatKH()
        {
            Cursor = Cursors.Wait;
            try
            {
                // Cảnh báo khi không có dữ liệu
                if (LObject.IsNullOrEmpty(idKhachHang))
                {
                    LMessage.ShowMessage("M.KhachHang.ucKhachHangThanhVien.KhongCoThongTinKheUoc", LMessage.MessageBoxType.Warning);
                    return;
                }
                else
                {

                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                    Presentation.Process.BaoCaoServiceRef.DoiTuongBaoCao doiTuongBaoCao = new Presentation.Process.BaoCaoServiceRef.DoiTuongBaoCao();

                    Presentation.Process.BaoCaoServiceRef.GIAO_DICH_BASE objGIAO_DICH_BASE = new Presentation.Process.BaoCaoServiceRef.GIAO_DICH_BASE();
                    objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.KHTV_PHIEU_KHAOSAT;
                    objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.KH_THANH_VIEN;

                    Presentation.Process.BaoCaoServiceRef.KHTV_PHIEU_KHAOSAT objKHTV_PHIEU_KHAOSAT = new Presentation.Process.BaoCaoServiceRef.KHTV_PHIEU_KHAOSAT();
                    objKHTV_PHIEU_KHAOSAT.MA_KHACH_HANG = MaKhachHang;
                    objKHTV_PHIEU_KHAOSAT.ID_KHANG = idKhachHang;

                    doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                    doiTuongBaoCao.objKHTV_PHIEU_KHAOSAT = objKHTV_PHIEU_KHAOSAT;
                    xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
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

        private void OnPreviewXepHangTinDung()
        {
            Cursor = Cursors.Wait;
            try
            {
                // Cảnh báo khi không có dữ liệu
                if (LObject.IsNullOrEmpty(idKhachHang))
                {
                    LMessage.ShowMessage("M.KhachHang.ucKhachHangThanhVien.KhongCoThongTinKhachHang", LMessage.MessageBoxType.Warning);
                    return;
                }
                else
                {

                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                    Presentation.Process.BaoCaoServiceRef.DoiTuongBaoCao doiTuongBaoCao = new Presentation.Process.BaoCaoServiceRef.DoiTuongBaoCao();

                    Presentation.Process.BaoCaoServiceRef.GIAO_DICH_BASE objGIAO_DICH_BASE = new Presentation.Process.BaoCaoServiceRef.GIAO_DICH_BASE();
                    objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.KHTV_PHIEU_XEPHANG;
                    objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.KH_THANH_VIEN;

                    Presentation.Process.BaoCaoServiceRef.KHTV_PHIEU_XEPHANG objKHTV_PHIEU_XEPHANG = new Presentation.Process.BaoCaoServiceRef.KHTV_PHIEU_XEPHANG();
                    objKHTV_PHIEU_XEPHANG.MA_KHACH_HANG = MaKhachHang;
                    objKHTV_PHIEU_XEPHANG.NGAY_BAO_CAO = ClientInformation.NgayLamViecHienTai;

                    doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                    doiTuongBaoCao.objKHTV_PHIEU_XEPHANG = objKHTV_PHIEU_XEPHANG;
                    xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
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

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Presentation.Process.DanhMucProcess processDanhMuc = new Presentation.Process.DanhMucProcess();
            if (cmbPhongGD.SelectedIndex >= 0)
            {
                if (lstSourcePhongGD.Select(i => i.DisplayName).Contains(cmbPhongGD.Text))
                {
                    idPhongGD = processDanhMuc.getDonViTheoMa(((AutoCompleteEntry)cmbPhongGD.SelectedValue).KeywordStrings[0]).Tables[0].Rows[0]["ID"].ToString();
                }
                else
                {
                    idPhongGD = ClientInformation.IdDonViGiaoDich.ToString();
                }
            }
            else
            {
                idPhongGD = ClientInformation.IdDonViGiaoDich.ToString();
            }
            //idKhuVuc = processDanhMuc.getKhuVucByIdDonVi(idPhongGD.StringToInt32()).ID.ToString();
        }
    }
}
