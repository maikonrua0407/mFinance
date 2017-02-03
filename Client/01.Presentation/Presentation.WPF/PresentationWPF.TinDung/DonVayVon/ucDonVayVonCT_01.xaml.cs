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
using System.Data;
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using PresentationWPF.CustomControl;
using Presentation.Process.TinDungServiceRef;
using Telerik.Windows.Controls;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.Common;
using System.Reflection;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;
using System.Collections;

namespace PresentationWPF.TinDung.DonVayVon
{
    /// <summary>
    /// Interaction logic for ucDonVayVonCT_01.xaml
    /// </summary>
    public partial class ucDonVayVonCT_01 : UserControl
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
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        int idKhachHang = 0;
        string maKhangHang = "";
        string tenKhachHang = "";
        
        string soGTLQ = "";
        string tenNhom = "";
        string TThaiNVu = "";
        public DatabaseConstant.Action action;
        public event EventHandler OnSavingCompleted;
        List<int> lstId = new List<int>();
        List<DXVVVM_MUC_DICH_VAY_VON> lstMucDichVayVon = new List<DXVVVM_MUC_DICH_VAY_VON>();
        List<DXVVVM_NGUON_TRA_NO> lstNguonTraNo = new List<DXVVVM_NGUON_TRA_NO>();
        List<DXVVVM_NGUON_TRA_NO> lstNguonTraNoThu = new List<DXVVVM_NGUON_TRA_NO>();
        List<DXVVVM_NGUON_TRA_NO> lstNguonTraNoChi = new List<DXVVVM_NGUON_TRA_NO>();
        List<CAP_PHE_DUYET> PheDuyet = new List<CAP_PHE_DUYET>();
        List<THONG_TIN_TKE_BLANH> lstThongTinBLanh = new List<THONG_TIN_TKE_BLANH>();
        public void LayDuLieuPheDuyet(List<CAP_PHE_DUYET> lst)
        {
            PheDuyet = lst;
        }
        CAP_PHE_DUYET objCapPheDuyet = new CAP_PHE_DUYET();
        DON_XIN_VAY_VON_VI_MO obj;

        List<AutoCompleteEntry> lstSourceMucDichVayVon = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiSanPham = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceSanPhamVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTHanVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTHanLaiSuat = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHinhThucTraGoc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHinhThucTraLai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTienTe = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTrangThaiCapTinDung = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceMoiQuanHe = new List<AutoCompleteEntry>();

        private int idDiaBan = 0;
        private string maVongVay = "";
        private int heSo = 0;
        private int idHDTDVM = 0;
        private string maDonVi;
        string soTienHanMuc = "0";
        string soTienGoc = "0";
        string soTienLai = "0";
        string capPheDuyet = "";
        string loaiTien = ClientInformation.MaDongNoiTe;
        string hinhThucGoc = BusinessConstant.TINH_CHAT_VONG_VAY.THAY_DOI.layGiaTri();
        string hinhThucKyHan = BusinessConstant.TINH_CHAT_VONG_VAY.THAY_DOI.layGiaTri();
        string maLaiSuat = "";
        string mucDichVay = "";
        string canCuTinhQH = "CONG_TY_LE";
        decimal tyleLaiQuaHan = 0;

        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucDonVayVonCT_01()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDung;component/DonVayVon/ucDonVayVonCT_01.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            KhoiTaoComboBox();
            ShowControl();
            InitEventHandler();
            TaoBangDuLieu();
            ResetForm();
            txtMaKHang.Focus();
        }

        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.TinDung.DonVayVon.ucDonVayVonCT_01", "raddgrDSThuNhap");
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

            arr = hethong.SetVisibleControl("PresentationWPF.TinDung.DonVayVon.ucDonVayVonCT_01", "");
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

        void InitEventHandler()
        {
            txtMaKHang.KeyDown += new KeyEventHandler(txtMaKHang_KeyDown);
            this.Unloaded += new RoutedEventHandler(ucDonVayVonCT_01_Unloaded);
            cmbLoaiSanPham.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiSanPham_SelectionChanged);
            btnMaKHang.Click += new RoutedEventHandler(btnMaKHang_Click);
            btnMaSanPham.Click += new RoutedEventHandler(btnMaSanPham_Click);
            btnMaMucDich.Click += new RoutedEventHandler(btnMaMucDich_Click);
            btnNguoiThuaKe.Click += new RoutedEventHandler(btnNguoiThuaKe_Click);
            btnNguoiBaoLanh.Click += new RoutedEventHandler(btnNguoiBaoLanh_Click);
            raddgrChiPhi.CellValidating += new EventHandler<GridViewCellValidatingEventArgs>(raddgrChiPhi_CellValidating);
            raddgrThuNhap.CellValidating += new EventHandler<GridViewCellValidatingEventArgs>(raddgrThuNhap_CellValidating);
            raddgrMucDich.CellValidating += new EventHandler<GridViewCellValidatingEventArgs>(raddgrMucDich_CellValidating);
            raddgrChiPhi.DataLoaded += new EventHandler<EventArgs>(raddgrChiPhi_DataLoaded);
            raddgrThuNhap.DataLoaded += new EventHandler<EventArgs>(raddgrThuNhap_DataLoaded);
            raddgrThuNhap.CellEditEnded += raddgrThuNhap_CellEditEnded;
            raddgrChiPhi.CellEditEnded += raddgrChiPhi_CellEditEnded;
            chkTaoSoTietKiem.Checked += new RoutedEventHandler(chkTaoSoTietKiem_Checked);
            chkTaoSoTietKiem.Unchecked += new RoutedEventHandler(chkTaoSoTietKiem_Unchecked);
            telSoTienVay.LostFocus += new RoutedEventHandler(telSoTienVay_LostFocus);
        }

        void ucDonVayVonCT_01_Unloaded(object sender, RoutedEventArgs e)
        {
            Release();
        }

        void TaoBangDuLieu()
        {
            capPheDuyet = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_ON_CHECK_MATRIX_APPROVE, ClientInformation.MaDonVi);
            if (capPheDuyet.IsNullOrEmptyOrSpace())
                capPheDuyet = BusinessConstant.CoKhong.KHONG.layGiaTri();
            if(capPheDuyet.Equals(BusinessConstant.CoKhong.KHONG.layGiaTri()))
                spnCapDuyet.Visibility = Visibility.Collapsed;
            else
                spnCapDuyet.Visibility = Visibility.Visible;
            objCapPheDuyet.FUNCTION = DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON.getValue();
            objCapPheDuyet.USER_NAME = ClientInformation.TenDangNhap;
        }

        void KhoiTaoComboBox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.LOAI_SAN_PHAM_TDUNG.getValue());
            auto.GenAutoComboBox(ref lstSourceLoaiSanPham, ref cmbLoaiSanPham, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
            maDonVi = ClientInformation.MaDonVi;
            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DVI_TINH_KY_HAN.getValue());
            auto.GenAutoComboBox(ref lstSourceTHanVay, ref cmbThoiHanVay, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DVI_TINH_LSUAT.getValue());
            auto.GenAutoComboBox(ref lstSourceTHanLaiSuat, ref cmbThoiHanLSuat, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
            lstDieuKien.Clear();
            auto.GenAutoComboBox(ref lstSourceTienTe, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TIENTE.getValue(), lstDieuKien, ClientInformation.MaDongNoiTe);
            auto.GenAutoComboBox(ref lstSourceTienTe, ref cmbLoaiTienTraHangThang, null, null, ClientInformation.MaDongNoiTe);
            auto.GenAutoComboBox(ref lstSourceTienTe, ref cmbLoaiTienThuNhap, null, null, ClientInformation.MaDongNoiTe);
            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.HINH_THUC_TRA_GOC.getValue());
            auto.GenAutoComboBox(ref lstSourceHinhThucTraGoc, ref cmbHinhThucTraGoc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, BusinessConstant.HINH_THUC_TRA_GOC.DINH_KY.layGiaTri());
            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.HINH_THUC_TRA_LAI.getValue());
            auto.GenAutoComboBox(ref lstSourceHinhThucTraLai, ref cmbHinhThucTraLai, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, BusinessConstant.HINH_THUC_TRA_GOC.DINH_KY.layGiaTri());
            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.TRANG_THAI_CAP_TIN_DUNG.getValue());
            auto.GenAutoComboBox(ref lstSourceTrangThaiCapTinDung, ref cmbTThaiCapTinDung, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.MOI_QUAN_HE.getValue());
            auto.GenAutoComboBox(ref lstSourceMoiQuanHe, ref txtQuanHeNguoiThuaKe, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
            auto.GenAutoComboBox(ref lstSourceMoiQuanHe, ref txtQuanHeNguoiBaoLanh, null, lstDieuKien);
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
                        KeyGesture keyg = new KeyGesture(Key.V, ModifierKeys.Control | ModifierKeys.Shift);
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
                        KeyGesture keyg = new KeyGesture(Key.W, ModifierKeys.Control | ModifierKeys.Shift);
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
            txtSoGiaoDich.Focus();

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
                SetEnabledAllControls(true);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                beforeModify();
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
                BeforeRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                BeforeCancel();
            }
            else if (strTinhNang.Equals("Print"))
            {
                OnPrint();
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
            else if (strTinhNang.Equals("PreviewDonVayVon"))
            {
                OnPreviewDonVayVon();
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetForm();
                SetEnabledAllControls(true);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
            }
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
            else if (strTinhNang.Equals("PreviewDonVayVon"))
            {
                OnPreviewDonVayVon();
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

        void raddgrDSThanhVien_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            
        }

        void cmbLoaiSanPham_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtMaSanPham.Text = "";
            txtMaSanPham.Tag = 0;

        }

        void btnMaKHang_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> lstDieuKien = new List<string>();
                string lstIDKH = "";
                if (!txtMaKHang.Text.IsNullOrEmptyOrSpace())
                    lstIDKH = "(" + txtMaKHang.Tag.ToString() + ")";
                else
                    lstIDKH = "(0)";
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add(lstIDKH);
                lstDieuKien.Add("NULL");
                lstDieuKien.Add(DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON.getValue());
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_KHACHHANG_TDVM", lstDieuKien);
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
                        txtMaKHang.Text = maKhangHang = dr["MA_KHANG"].ToString();
                        txtMaKHang.Tag = idKhachHang = Convert.ToInt32(dr["ID"]);
                        txtTenKHang.Text = dr["TEN_KHANG"].ToString();
                        txtThonAp.Text = dr["TEN_CUM"].ToString();
                        idDiaBan = Convert.ToInt32(dr["ID_NHOM"]);
                        txtMaNhom.Text = dr["MA_NHOM"].ToString();
                        txtTenNhomTruong.Text = dr["TEN_NHOM_TRUONG"].ToString();
                        txtPhuongXa.Text = dr["TEN_KVUC"].ToString();
                        txtQuanHuyen.Text = dr["TEN_HUYEN"].ToString();
                        txtSoCMND.Text = dr["DD_GTLQ_SO"].ToString();
                        heSo = Convert.ToInt32(dr["HE_SO_VVON"]);
                        if (obj.IsNullOrEmpty())
                            obj = new DON_XIN_VAY_VON_VI_MO();
                        if (obj.OBJ_DON_XIN_VAY.IsNullOrEmpty())
                            obj.OBJ_DON_XIN_VAY = new DON_XIN_VAY_VON_VI_MO_CTIET();
                        obj.OBJ_DON_XIN_VAY.ID_KHANG = idKhachHang;
                        obj.OBJ_DON_XIN_VAY.MA_KHANG = txtMaKHang.Text;
                        obj.OBJ_DON_XIN_VAY.HE_SO = heSo;
                        obj.OBJ_DON_XIN_VAY.ID_DIA_BAN = idDiaBan;
                        obj.OBJ_DON_XIN_VAY.MA_DIA_BAN = txtMaNhom.Text;
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        void btnNguoiThuaKe_Click(object sender, RoutedEventArgs e)
        {
            if (txtMaKHang.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblMaKhachHang.Content.ToString());
                return;
            }
            try
            {
                List<string> lstDieuKien = new List<string>();
                string lstIDKH = "";
                if (!txtMaKHang.Text.IsNullOrEmptyOrSpace())
                    lstIDKH = "(" + txtMaKHang.Tag.ToString() + ")";
                else
                    lstIDKH = "(0)";
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add(idKhachHang.ToString());
                lstDieuKien.Add(maKhangHang);
                lstDieuKien.Add(lstIDKH);
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_NGUOI_THUA_KE_TDVM", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
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
                        txtNguoiThuaKe.Text = dr["MA_NGUOI_DTN"].ToString();
                        txtNguoiThuaKe.Tag = Convert.ToInt32(dr["ID"]);
                        txtTenNguoiThuaKe.Text = dr["TEN_KHANG"].ToString();
                        txtTenBoNguoiThuaKe.Text = dr["TEN_BO"].ToString();
                        txtSoCMNDNguoiThuaKe.Text = dr["GTLQ_SO"].ToString();
                        if (dr["NGAY_CAP"] != DBNull.Value && !dr["NGAY_CAP"].ToString().IsNullOrEmptyOrSpace() && dr["NGAY_CAP"].ToString().IsDate(ApplicationConstant.defaultDateTimeFormat))
                            txtNgayCapCMNDNguoiThuaKe.Value = LDateTime.StringToDate(dr["NGAY_CAP"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                        else
                            txtNgayCapCMNDNguoiThuaKe.Text = null;
                        txtNoiCapCMNDNguoiThuaKe.Text = dr["NOI_CAP"].ToString();
                        txtDiaChiNguoiThuaKe.Text = dr["DIA_CHI"].ToString();
                        txtDienThoaiNguoiThuaKe.Text = dr["SO_DIEN_THOAI"].ToString();
                        txtDiDongNguoiThuaKe.Text = dr["SO_DI_DONG"].ToString();
                        txtQuanHeNguoiThuaKe.SelectedIndex = lstSourceMoiQuanHe.IndexOf(lstSourceMoiQuanHe.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["MOI_QUAN_HE"].ToString())));
                        obj.OBJ_DON_XIN_VAY.ID_NGUOI_DTN = Convert.ToInt32(dr["ID"]);
                        obj.OBJ_DON_XIN_VAY.MA_NGUOI_DTN = dr["MA_NGUOI_DTN"].ToString();
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        void btnNguoiBaoLanh_Click(object sender, RoutedEventArgs e)
        {
            if (txtMaKHang.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblMaKhachHang.Content.ToString());
                return;
            }
            try
            {
                List<string> lstDieuKien = new List<string>();
                string lstIDKH = "";
                if (!txtMaKHang.Text.IsNullOrEmptyOrSpace())
                    lstIDKH = "(" + txtMaKHang.Tag.ToString() + ")";
                else
                    lstIDKH = "(0)";
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add(idKhachHang.ToString());
                lstDieuKien.Add(maKhangHang);
                lstDieuKien.Add(lstIDKH);
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_NGUOI_BAO_LANH_TDVM", lstDieuKien);
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
                        txtNguoiBaoLanh.Text = dr["MA_NGUOI_DTN"].ToString();
                        txtNguoiBaoLanh.Tag = Convert.ToInt32(dr["ID"]);
                        txtTenNguoiBaoLanh.Text = dr["TEN_KHANG"].ToString();
                        txtTenBoNguoiBaoLanh.Text = dr["TEN_BO"].ToString();
                        txtSoCMNDNguoiBaoLanh.Text = dr["GTLQ_SO"].ToString();
                        if (dr["NGAY_CAP"] != DBNull.Value && !dr["NGAY_CAP"].ToString().IsNullOrEmptyOrSpace() && dr["NGAY_CAP"].ToString().IsDate(ApplicationConstant.defaultDateTimeFormat))
                            txtNgayCapCMNDNguoiBaoLanh.Value = LDateTime.StringToDate(dr["NGAY_CAP"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                        else
                            txtNgayCapCMNDNguoiBaoLanh.Value = null;
                        txtNoiCapCMNDNguoiBaoLanh.Text = dr["NOI_CAP"].ToString();
                        txtDiaChiNguoiBaoLanh.Text = dr["DIA_CHI"].ToString();
                        txtDienThoaiNguoiBaoLanh.Text = dr["SO_DIEN_THOAI"].ToString();
                        txtDiDongNguoiBaoLanh.Text = dr["SO_DI_DONG"].ToString();
                        if (dr["MOI_QUAN_HE"] != DBNull.Value && !dr["MOI_QUAN_HE"].IsNullOrEmpty())
                            txtQuanHeNguoiBaoLanh.SelectedIndex = lstSourceMoiQuanHe.IndexOf(lstSourceMoiQuanHe.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["MOI_QUAN_HE"].ToString())));
                        obj.OBJ_DON_XIN_VAY.ID_NGUOI_DTN = Convert.ToInt32(dr["ID"]);
                        obj.OBJ_DON_XIN_VAY.MA_NGUOI_DTN = dr["MA_NGUOI_DTN"].ToString();
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        void btnMaSanPham_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> lstDieuKien = new List<string>();
                string lstIDKH = "";
                if (!txtMaSanPham.Tag.IsNullOrEmpty())
                    lstIDKH = "(" + txtMaSanPham.Tag.ToString() + ")";
                else
                    lstIDKH = "(0)";
                AutoCompleteEntry au = lstSourceLoaiSanPham.ElementAt(cmbLoaiSanPham.SelectedIndex);
                lstDieuKien.Add(ClientInformation.MaDonVi);
                lstDieuKien.Add(lstIDKH);
                lstDieuKien.Add(au.KeywordStrings.FirstOrDefault());
                lstDieuKien.Add(heSo.ToString());
                lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_SAN_PHAM_TDVM", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
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
                        txtMaSanPham.Text = dr["MA_SAN_PHAM"].ToString();
                        txtMaSanPham.Tag = Convert.ToInt32(dr["ID_SAN_PHAM"]);
                        lblTenSanPham.Content = dr["TEN_SAN_PHAM"].ToString();
                        if (dr["KY_HAN_VAY"] != DBNull.Value && dr["KY_HAN_VAY"].ToString().IsNumeric())
                            telThoiGianVay.Value = Convert.ToDouble(dr["KY_HAN_VAY"]);
                        if (dr["KY_HAN_DVT"] != DBNull.Value && !dr["KY_HAN_DVT"].ToString().IsNullOrEmptyOrSpace())
                        {
                            cmbThoiHanVay.SelectedIndex = lstSourceTHanVay.IndexOf(lstSourceTHanVay.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["KY_HAN_DVT"].ToString())));
                            cmbThoiHanVay.IsEnabled = false;
                        }
                        else
                            cmbThoiHanVay.IsEnabled = true;
                        if (dr["LAI_SUAT"] != DBNull.Value && dr["LAI_SUAT"].ToString().IsNumeric())
                        {
                            telLaiSuat.Value = Convert.ToDouble(dr["LAI_SUAT"]);
                        }

                        if (dr["LAI_SUAT_DVT"] != DBNull.Value && !dr["LAI_SUAT_DVT"].ToString().IsNullOrEmptyOrSpace())
                        {
                            cmbThoiHanLSuat.SelectedIndex = lstSourceTHanLaiSuat.IndexOf(lstSourceTHanLaiSuat.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["LAI_SUAT_DVT"].ToString())));
                            cmbThoiHanLSuat.IsEnabled = false;
                            if (!obj.IsNullOrEmpty())
                            {
                                if (!obj.OBJ_DON_XIN_VAY.IsNullOrEmpty())
                                {
                                    obj.OBJ_DON_XIN_VAY.DVT_LAI_SUAT = dr["LAI_SUAT_DVT"].ToString();
                                    obj.OBJ_DON_XIN_VAY.DVT_LAI_SUAT_QH = dr["LAI_SUAT_DVT"].ToString();
                                }
                            }
                        }
                        else
                            cmbThoiHanLSuat.IsEnabled = true;
                        if (dr["SO_TIEN_VAY"] != DBNull.Value && dr["SO_TIEN_VAY"].ToString().IsNumeric())
                            telSoTienVay.Value = Convert.ToDouble(dr["SO_TIEN_VAY"]);
                        if (dr["LOAI_TIEN"] != DBNull.Value && !dr["LOAI_TIEN"].ToString().IsNullOrEmptyOrSpace())
                        {
                            cmbLoaiTien.SelectedIndex = lstSourceTienTe.IndexOf(lstSourceTienTe.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["LOAI_TIEN"].ToString())));
                            cmbLoaiTienTraHangThang.SelectedIndex = lstSourceTienTe.IndexOf(lstSourceTienTe.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["LOAI_TIEN"].ToString())));
                            loaiTien = dr["LOAI_TIEN"].ToString();
                        }
                        if (dr["TCHAT_GOC_VAY"] != DBNull.Value && !dr["TCHAT_GOC_VAY"].ToString().IsNullOrEmptyOrSpace() && dr["TCHAT_GOC_VAY"].ToString().Equals(BusinessConstant.TINH_CHAT_VONG_VAY.CO_DINH.layGiaTri()))
                            telSoTienVay.IsReadOnly = true;
                        else
                            telSoTienVay.IsReadOnly = false;
                        if (dr["TCHAT_KY_HAN"] != DBNull.Value && !dr["TCHAT_KY_HAN"].ToString().IsNullOrEmptyOrSpace() && dr["TCHAT_KY_HAN"].ToString().Equals(BusinessConstant.TINH_CHAT_VONG_VAY.CO_DINH.layGiaTri()))
                            telThoiGianVay.IsReadOnly = true;
                        else
                            telThoiGianVay.IsReadOnly = false;
                        if (dr["MUC_DICH_VAY"] != DBNull.Value)
                            mucDichVay = Convert.ToString(dr["MUC_DICH_VAY"]);
                        if (dr["MUC_DICH_VAY"] != DBNull.Value)
                            mucDichVay = Convert.ToString(dr["MUC_DICH_VAY"]);
                        if (dr["CAN_CU_TINH_LAI_QH"] != DBNull.Value)
                            canCuTinhQH = Convert.ToString(dr["CAN_CU_TINH_LAI_QH"]);
                        if (dr["TY_LE_TINH_LAI_QH"] != DBNull.Value)
                            tyleLaiQuaHan = Convert.ToDecimal(dr["TY_LE_TINH_LAI_QH"]);
                        if (dr["MA_LSUAT"] != DBNull.Value)
                            if (!obj.IsNullOrEmpty())
                            {
                                if (!obj.OBJ_DON_XIN_VAY.IsNullOrEmpty())
                                {
                                    obj.OBJ_DON_XIN_VAY.MA_LSUAT = Convert.ToString(dr["MA_LSUAT"]);
                                }
                            }
                        if (!obj.IsNullOrEmpty())
                        {
                            if (!obj.OBJ_DON_XIN_VAY.IsNullOrEmpty())
                            {
                                obj.OBJ_DON_XIN_VAY.MA_SAN_PHAM = txtMaSanPham.Text;
                                obj.OBJ_DON_XIN_VAY.ID_SAN_PHAM = Convert.ToInt32(dr["ID_SAN_PHAM"]);
                            }
                        }
                        
                        if (dr["LOAI_LSUAT"] != DBNull.Value && !dr["LOAI_LSUAT"].ToString().IsNullOrEmptyOrSpace())
                            if (!obj.IsNullOrEmpty())
                            {
                                if (!obj.OBJ_DON_XIN_VAY.IsNullOrEmpty())
                                {
                                    obj.OBJ_DON_XIN_VAY.LOAI_LAI_SUAT = dr["LOAI_LSUAT"].ToString();
                                }
                            }
                        
                        telSoTienVay_LostFocus(null, null);
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        void btnMaMucDich_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> lstDieuKien = new List<string>();
                if (mucDichVay.IsNullOrEmptyOrSpace())
                    mucDichVay = "";

                if (txtMaSanPham.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblSanPhamVay.Content.ToString());
                    return;
                }

                lstDieuKien.Add(mucDichVay);
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_MUC_DICH_VAY_VON", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
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
                        txtMaMucDich.Text = dr["MA_DMUC"].ToString();
                        lblTenMucDich.Content = dr["MA_NNGU"].ToString();
                        if (obj.IsNullOrEmpty())
                            obj = new DON_XIN_VAY_VON_VI_MO();
                        if (obj.OBJ_DON_XIN_VAY.IsNullOrEmpty())
                            obj.OBJ_DON_XIN_VAY = new DON_XIN_VAY_VON_VI_MO_CTIET();
                        obj.OBJ_DON_XIN_VAY.MA_MUC_DICH_VAY = txtMaMucDich.Text;
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
        private void txtMaKHang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
                btnMaKHang_Click(sender, null);
        }

        void raddgrDSThanhVien_RowValidating(object sender, GridViewRowValidatingEventArgs e)
        {
            DON_XIN_VAY_KHOACH_CT objDonVayVon = e.Row.Item as DON_XIN_VAY_KHOACH_CT;
            if (objDonVayVon.TEN_KHOACH.IsNullOrEmpty())
            {
                e.IsValid = false;
            }
        }

        void raddgrDSThanhVien_CellValidating(object sender, GridViewCellValidatingEventArgs e)
        {
            if (LObject.IsNullOrEmpty(e.NewValue) && (e.Cell.Column.UniqueName == "VON_XIN_VAY_SO_TIEN" || e.Cell.Column.UniqueName == "VON_XIN_VAY_KY_HAN"))
            {
                e.IsValid = false;
                e.ErrorMessage = "Value not null";
            }
            else if (e.NewValue.ToString().IsNumeric())
            {
                decimal dNumber = Convert.ToDecimal(e.NewValue);
                if (e.Cell.Column.UniqueName == "VON_TU_CO_SO_TIEN")
                {
                    if (dNumber < 0)
                    {
                        e.IsValid = false;
                        e.ErrorMessage = "Value must than 0";
                    }
                }
                else if (e.Cell.Column.UniqueName == "VON_XIN_VAY_SO_TIEN")
                {
                    if (dNumber < 0)
                    {
                        e.IsValid = false;
                        e.ErrorMessage = "Value must than 0";
                    }
                }
            }
        }

        void raddgrDSThuNhap_RowValidating(object sender, GridViewRowValidatingEventArgs e)
        {
            DON_XIN_VAY_KHOACH_CT objDonVayVon = e.Row.Item as DON_XIN_VAY_KHOACH_CT;
            if (objDonVayVon.TEN_KHOACH.IsNullOrEmpty())
            {
                e.IsValid = false;
            }
        }

        void raddgrDSThuNhap_CellValidating(object sender, GridViewCellValidatingEventArgs e)
        {
            if (LObject.IsNullOrEmpty(e.NewValue))
            {
                e.IsValid = false;
                e.ErrorMessage = "Value not null";
            }
            else if (e.NewValue.ToString().IsNumeric())
            {
                decimal dNumber = Convert.ToDecimal(e.NewValue);
                if (e.Cell.Column.UniqueName == "VON_TU_CO_SO_TIEN")
                {
                    if (dNumber < 0)
                    {
                        e.IsValid = false;
                        e.ErrorMessage = "Value must than 0";
                    }
                }
                else if (e.Cell.Column.UniqueName == "VON_XIN_VAY_SO_TIEN")
                {
                    if (dNumber < 0)
                    {
                        e.IsValid = false;
                        e.ErrorMessage = "Value must than 0";
                    }
                }
            }
        }

        void raddgrDSThuNhap_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            
        }

        void LoadPopUpKhachHang()
        {
            try
            {
                List<string> lstDieuKien = new List<string>();
                string lstIDKH = "";
                if (!txtMaKHang.Text.IsNullOrEmptyOrSpace())
                    lstIDKH = "(" + txtMaKHang.Tag.ToString() + ")";
                else
                    lstIDKH = "(0)";
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add(lstIDKH);
                lstDieuKien.Add("NULL");
                lstDieuKien.Add(DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON.getValue());
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_KHACHHANG_TDVM", lstDieuKien);
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
                        txtMaKHang.Text = dr["MA_KHANG"].ToString();
                        txtMaKHang.Tag = idKhachHang = Convert.ToInt32(dr["ID"]);
                        txtTenKHang.Text = dr["TEN_KHANG"].ToString();
                        txtThonAp.Text = dr["TEN_CUM"].ToString();
                        idDiaBan = Convert.ToInt32(dr["ID_NHOM"]);
                        txtPhuongXa.Text = dr["TEN_KVUC"].ToString();
                        txtQuanHuyen.Text = dr["TEN_HUYEN"].ToString();
                        txtSoCMND.Text = dr["DD_GTLQ_SO"].ToString();
                        heSo = Convert.ToInt32(dr["HE_SO_VVON"]);
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }

        }

        void raddgrMucDich_CellValidating(object sender, GridViewCellValidatingEventArgs e)
        {
            if (e.Cell.Column.UniqueName.Equals("SO_TIEN"))
            {
                if (e.NewValue.ToString().IsNumeric())
                {
                    decimal dNumber = Convert.ToDecimal(e.NewValue);
                    if (dNumber < 0)
                    {
                        e.IsValid = false;
                        e.ErrorMessage = "Value must than 0";
                    }
                }
                else
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Value must numeric";
                }
            }
        }

        void raddgrThuNhap_CellValidating(object sender, GridViewCellValidatingEventArgs e)
        {
            if (e.Cell.Column.UniqueName.Equals("SO_TIEN"))
            {
                if (e.NewValue.ToString().IsNumeric())
                {
                    decimal dNumber = Convert.ToDecimal(e.NewValue);
                    if (dNumber < 0)
                    {
                        e.IsValid = false;
                        e.ErrorMessage = "Value must than 0";
                    }
                }
                else
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Value must numeric";
                }
            }
        }

        void raddgrChiPhi_CellValidating(object sender, GridViewCellValidatingEventArgs e)
        {
            if (e.Cell.Column.UniqueName.Equals("SO_TIEN"))
            {
                if (e.NewValue.ToString().IsNumeric())
                {
                    decimal dNumber = Convert.ToDecimal(e.NewValue);
                    if (dNumber < 0)
                    {
                        e.IsValid = false;
                        e.ErrorMessage = "Value must than 0";
                    }
                }
                else
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Value must numeric";
                }
            }
        }

        void raddgrThuNhap_DataLoaded(object sender, EventArgs e)
        {
            lstNguonTraNoThu.Where(f => f.LOAI_TIEN.IsNullOrEmptyOrSpace()).ToList().ForEach(f => { f.LOAI_TIEN = loaiTien; f.TY_GIA = 1; });
            telSoTienTraNoHangThang.Value = Convert.ToDouble(lstNguonTraNoThu.Sum(f => f.SO_TIEN * f.TY_GIA) - lstNguonTraNoChi.Sum(f => f.SO_TIEN * f.TY_GIA));
        }

        void raddgrChiPhi_DataLoaded(object sender, EventArgs e)
        {
            lstNguonTraNoChi.Where(f => f.LOAI_TIEN.IsNullOrEmptyOrSpace()).ToList().ForEach(f => { f.LOAI_TIEN = loaiTien; f.TY_GIA = 1; });
            telSoTienTraNoHangThang.Value = Convert.ToDouble(lstNguonTraNoThu.Sum(f => f.SO_TIEN * f.TY_GIA) - lstNguonTraNoChi.Sum(f => f.SO_TIEN * f.TY_GIA));
        }

        void chkTaoSoTietKiem_Unchecked(object sender, RoutedEventArgs e)
        {
            telSoTienTKBB.IsEnabled = true;
            TinhSoTienTKiem();
        }

        void chkTaoSoTietKiem_Checked(object sender, RoutedEventArgs e)
        {
            telSoTienTKBB.IsEnabled = false;
        }

        void raddgrThuNhap_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            raddgrThuNhap.CellEditEnded -= raddgrThuNhap_CellEditEnded;
            lstNguonTraNoThu.Where(f => f.LOAI_TIEN.IsNullOrEmptyOrSpace()).ToList().ForEach(f => { f.LOAI_TIEN = loaiTien; f.TY_GIA = 1; });
            telSoTienTraNoHangThang.Value = Convert.ToDouble(lstNguonTraNoThu.Sum(f => f.SO_TIEN * f.TY_GIA) - lstNguonTraNoChi.Sum(f => f.SO_TIEN * f.TY_GIA));
            raddgrThuNhap.CellEditEnded += raddgrThuNhap_CellEditEnded;
        }

        void raddgrChiPhi_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            raddgrChiPhi.CellEditEnded -= raddgrChiPhi_CellEditEnded;
            lstNguonTraNoChi.Where(f => f.LOAI_TIEN.IsNullOrEmptyOrSpace()).ToList().ForEach(f => { f.LOAI_TIEN = loaiTien; f.TY_GIA = 1; });
            telSoTienTraNoHangThang.Value = Convert.ToDouble(lstNguonTraNoThu.Sum(f => f.SO_TIEN * f.TY_GIA) - lstNguonTraNoChi.Sum(f => f.SO_TIEN * f.TY_GIA));
            raddgrChiPhi.CellEditEnded += raddgrChiPhi_CellEditEnded;
        }

        void TinhSoTienTKiem()
        {
            try
            {
                decimal soTienKQuy = 0;

                string cachTinh = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_CACH_TINH_SO_TIEN_MO_SO_TK, ClientInformation.MaDonVi);
                if (cachTinh == "TY_LE")
                {
                    string tyLe = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_TY_LE_TINH_SO_TIEN_MO_SO_TK, ClientInformation.MaDonVi);
                    if (!tyLe.IsNullOrEmptyOrSpace())
                        soTienKQuy = (decimal)telSoTienVay.Value * Convert.ToDecimal(tyLe) / 100;
                }
                else
                {
                    string tuyetDoi = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_SO_TIEN_MO_SO_TK_TUYET_DOI, ClientInformation.MaDonVi);
                    if (!tuyetDoi.IsNullOrEmptyOrSpace())
                        soTienKQuy = Convert.ToDecimal(tuyetDoi);
                }

                telSoTienTKBB.Value = Convert.ToDouble(soTienKQuy);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void telSoTienVay_LostFocus(object sender, RoutedEventArgs e)
        {
            TinhSoTienTKiem();
        }
        #endregion

        /// <summary>
        /// Xu ly nghiep vu
        /// </summary>
        #region Xu ly nghiep vu
        void GetDataForm(BusinessConstant.TrangThaiBanGhi bghi, BusinessConstant.TrangThaiNghiepVu nghiepvu)
        {
            try
            {
                AutoCompleteEntry auHinhThucTraGoc = lstSourceHinhThucTraGoc.ElementAt(cmbHinhThucTraGoc.SelectedIndex);
                AutoCompleteEntry auHinhThucTraLai = lstSourceHinhThucTraLai.ElementAt(cmbHinhThucTraLai.SelectedIndex);
                AutoCompleteEntry auThoiHanVay = lstSourceTHanVay.ElementAt(cmbThoiHanVay.SelectedIndex);
                AutoCompleteEntry auThoiHanLSuat = lstSourceTHanLaiSuat.ElementAt(cmbThoiHanLSuat.SelectedIndex);
                AutoCompleteEntry auLoaiTien = lstSourceTienTe.ElementAt(cmbLoaiTien.SelectedIndex);
                AutoCompleteEntry auLoaiTienSXKD = lstSourceTienTe.ElementAt(cmbLoaiTienThuNhap.SelectedIndex);
                AutoCompleteEntry auQuanHeThuaKe = null;
                AutoCompleteEntry auQuanHeBaoLanh = null;
                
                obj.OBJ_DON_XIN_VAY.HINH_THUC_TRA_GOC = auHinhThucTraGoc.KeywordStrings.FirstOrDefault();
                obj.OBJ_DON_XIN_VAY.HINH_THUC_TRA_LAI = auHinhThucTraLai.KeywordStrings.FirstOrDefault();
                obj.OBJ_DON_XIN_VAY.DVT_THOI_GIAN_VAY = auThoiHanVay.KeywordStrings.FirstOrDefault();
                obj.OBJ_DON_XIN_VAY.DVT_TAN_SUAT = auThoiHanLSuat.KeywordStrings.FirstOrDefault();
                obj.OBJ_DON_XIN_VAY.MA_DXVVVM = txtSoGiaoDich.Text;
                obj.OBJ_DON_XIN_VAY.NGAY_LAP = LDateTime.DateToString(teldtNgayLapHD.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
                obj.OBJ_DON_XIN_VAY.THOI_GIAN_VAY = Convert.ToInt32(telThoiGianVay.Value.GetValueOrDefault());
                obj.OBJ_DON_XIN_VAY.LAI_SUAT_VAY = Convert.ToDecimal(telLaiSuat.Value.GetValueOrDefault());
                if (canCuTinhQH.Equals("CONG_TI_LE"))
                    obj.OBJ_DON_XIN_VAY.LAI_SUAT_QUA_HAN = obj.OBJ_DON_XIN_VAY.LAI_SUAT_VAY + tyleLaiQuaHan;
                else
                {
                    if(tyleLaiQuaHan==0)
                    {
                        tyleLaiQuaHan = Convert.ToDecimal(new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_TYLE_LAINO_QUAHAN_TOIDA, ClientInformation.MaDonVi));
                    }
                    obj.OBJ_DON_XIN_VAY.LAI_SUAT_QUA_HAN = obj.OBJ_DON_XIN_VAY.LAI_SUAT_VAY * tyleLaiQuaHan / 100;
                }
                obj.OBJ_DON_XIN_VAY.SO_TIEN_XIN_VAY = Convert.ToDecimal(telSoTienVay.Value.GetValueOrDefault());
                obj.OBJ_DON_XIN_VAY.SO_TIEN_VAY = Convert.ToDecimal(telnumHanMucPheDuyet.Value.GetValueOrDefault());
                if (chkTaoSoTietKiem.IsChecked.GetValueOrDefault())
                    obj.OBJ_DON_XIN_VAY.SO_TIEN_TKBB = null;
                else
                    obj.OBJ_DON_XIN_VAY.SO_TIEN_TKBB = Convert.ToDecimal(telSoTienTKBB.Value.GetValueOrDefault());

                lstMucDichVayVon = raddgrMucDich.ItemsSource as List<DXVVVM_MUC_DICH_VAY_VON>;
                lstNguonTraNoThu = raddgrThuNhap.ItemsSource as List<DXVVVM_NGUON_TRA_NO>;
                lstNguonTraNoChi = raddgrChiPhi.ItemsSource as List<DXVVVM_NGUON_TRA_NO>;
                lstNguonTraNo = new List<DXVVVM_NGUON_TRA_NO>();
                lstThongTinBLanh = new List<THONG_TIN_TKE_BLANH>();
                lstNguonTraNoChi.ForEach(f => f.LOAI_THU_NHAP_CHI_PHI = "CHI_PHI");
                lstNguonTraNoThu.ForEach(f => f.LOAI_THU_NHAP_CHI_PHI = "THU_NHAP");
                lstNguonTraNo.AddRange(lstNguonTraNoChi);
                lstNguonTraNo.AddRange(lstNguonTraNoThu);
                DXVVVM_NGUON_TRA_NO objNguonTraNo = new DXVVVM_NGUON_TRA_NO();
                objNguonTraNo.ID_DXVVVM = obj.OBJ_DON_XIN_VAY.ID;
                objNguonTraNo.LOAI_THU_NHAP_CHI_PHI = "THU_NHAP_TU_SXKD";
                objNguonTraNo.LOAI_TIEN = auLoaiTienSXKD.KeywordStrings.FirstOrDefault();
                objNguonTraNo.MA_DXVVVM = obj.OBJ_DON_XIN_VAY.MA_DXVVVM;
                objNguonTraNo.SO_TIEN = Convert.ToDecimal(telNguonTuSXKD.Value.GetValueOrDefault());
                objNguonTraNo.TEN_THU_NHAP_CHI_PHI = "THU_NHAP_TU_SXKD";
                objNguonTraNo.TY_GIA = 1;
                lstNguonTraNo.Add(objNguonTraNo);
                if (!txtTenNguoiThuaKe.Text.IsNullOrEmptyOrSpace())
                {
                    THONG_TIN_TKE_BLANH objTTinTKe = new THONG_TIN_TKE_BLANH();
                    objTTinTKe.DIA_CHI = txtDiaChiNguoiThuaKe.Text;
                    objTTinTKe.GTLQ_SO = txtSoCMNDNguoiThuaKe.Text;
                    objTTinTKe.LOAI_TKE = "THUA_KE";
                    objTTinTKe.MA_DXVV = txtSoGiaoDich.Text;
                    objTTinTKe.MA_KHANG = txtNguoiThuaKe.Text;
                    objTTinTKe.ID_KHANG = Convert.ToInt32(txtNguoiThuaKe.Tag);
                    if (!auQuanHeThuaKe.IsNullOrEmpty())
                        objTTinTKe.MOI_QUAN_HE = auQuanHeThuaKe.KeywordStrings.FirstOrDefault();
                    if (!LObject.IsNullOrEmpty(txtNgayCapCMNDNguoiThuaKe.Value))
                        objTTinTKe.NGAY_CAP = txtNgayCapCMNDNguoiThuaKe.Value.Value.ToString(ApplicationConstant.defaultDateTimeFormat);
                    else
                        objTTinTKe.NGAY_CAP = "";
                    objTTinTKe.NOI_CAP = txtNoiCapCMNDNguoiThuaKe.Text;
                    objTTinTKe.SO_DI_DONG = txtDiDongNguoiThuaKe.Text;
                    objTTinTKe.SO_DIEN_THOAI = txtDienThoaiNguoiThuaKe.Text;
                    objTTinTKe.TEN_BO = txtTenBoNguoiThuaKe.Text;
                    objTTinTKe.TEN_NGUOI_DTN = txtTenNguoiThuaKe.Text;

                    lstThongTinBLanh.Add(objTTinTKe);
                }

                if (!txtTenNguoiBaoLanh.Text.IsNullOrEmptyOrSpace())
                {
                    THONG_TIN_TKE_BLANH objTTinBLanh = new THONG_TIN_TKE_BLANH();
                    objTTinBLanh.DIA_CHI = txtDiaChiNguoiBaoLanh.Text;
                    objTTinBLanh.GTLQ_SO = txtSoCMNDNguoiBaoLanh.Text;
                    objTTinBLanh.LOAI_TKE = "BAO_LANH";
                    objTTinBLanh.MA_DXVV = txtSoGiaoDich.Text;
                    if (!txtNguoiBaoLanh.Text.Trim().IsNullOrEmpty())
                    {
                        objTTinBLanh.MA_KHANG = txtNguoiBaoLanh.Text;
                        objTTinBLanh.ID_KHANG = Convert.ToInt32(txtNguoiBaoLanh.Tag);
                    }
                    if (!auQuanHeBaoLanh.IsNullOrEmpty())
                        objTTinBLanh.MOI_QUAN_HE = auQuanHeBaoLanh.KeywordStrings.FirstOrDefault();
                    if (!LObject.IsNullOrEmpty(txtNgayCapCMNDNguoiBaoLanh.Value))
                        objTTinBLanh.NGAY_CAP = txtNgayCapCMNDNguoiBaoLanh.Value.Value.ToString(ApplicationConstant.defaultDateTimeFormat);
                    else
                        objTTinBLanh.NGAY_CAP = "";
                    objTTinBLanh.NOI_CAP = txtNoiCapCMNDNguoiBaoLanh.Text;
                    objTTinBLanh.SO_DI_DONG = txtDiDongNguoiBaoLanh.Text;
                    objTTinBLanh.SO_DIEN_THOAI = txtDienThoaiNguoiBaoLanh.Text;
                    objTTinBLanh.TEN_BO = txtTenBoNguoiBaoLanh.Text;
                    objTTinBLanh.TEN_NGUOI_DTN = txtTenNguoiBaoLanh.Text;

                    lstThongTinBLanh.Add(objTTinBLanh);
                }

                obj.DSACH_MUC_DICH_VAY = lstMucDichVayVon.ToArray();
                obj.DSACH_NGUON_TRA_NO = lstNguonTraNo.ToArray();
                obj.DSACH_NGUOI_TKE_BLANH = lstThongTinBLanh.ToArray();
                
                
                if (idHDTDVM == 0)
                {
                    obj.OBJ_DON_XIN_VAY.TTHAI_BGHI = bghi.layGiaTri();
                    obj.OBJ_DON_XIN_VAY.TTHAI_NVU = nghiepvu.layGiaTri();
                    obj.OBJ_DON_XIN_VAY.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    obj.OBJ_DON_XIN_VAY.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    obj.OBJ_DON_XIN_VAY.MA_DVI_QLY = ClientInformation.MaDonVi;
                    obj.OBJ_DON_XIN_VAY.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                }
                else
                {
                    obj.OBJ_DON_XIN_VAY.TTHAI_BGHI = bghi.layGiaTri();
                    obj.OBJ_DON_XIN_VAY.TTHAI_NVU = nghiepvu.layGiaTri();
                    obj.OBJ_DON_XIN_VAY.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    obj.OBJ_DON_XIN_VAY.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public void SetDataForm(string sSoGiaoDich)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dtPar = null;
                LDatatable.MakeParameterTable(ref dtPar);
                LDatatable.AddParameter(ref dtPar, "@SoGiaoDich", "String", sSoGiaoDich);
                ds = new TinDungProcess().GetThongTinDonXinVayVonTinDungViMo(dtPar);
                Dispatcher.CurrentDispatcher.DelayInvoke("SetDataForm", () =>
                    {
                        SetTabThongTinChung(ds);
                        Dispatcher.CurrentDispatcher.DelayInvoke("SetKeHoachSuDungVonVay", () =>
                        {
                            SetKeHoachSuDungVonVay(ds);
                        }, TimeSpan.FromSeconds(0));

                        Dispatcher.CurrentDispatcher.DelayInvoke("SetTabThongTinBaoLanhTKe", () =>
                        {
                            SetTabThongTinBaoLanhTKe(ds);
                        }, TimeSpan.FromSeconds(0));

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
                    DataRow dr = dt.Rows[0];
                    obj = new DON_XIN_VAY_VON_VI_MO();
                    DON_XIN_VAY_VON_VI_MO_CTIET objDXVVDTO = new DON_XIN_VAY_VON_VI_MO_CTIET();
                    objDXVVDTO.ID = idHDTDVM = Convert.ToInt32(dr["ID"]);
                    if (dr["CAP_LNHIEM"] != DBNull.Value)
                        objDXVVDTO.CAP_LIEN_NHIEM = dr["CAP_LNHIEM"].ToString();
                    if (dr["CAP_LNHIEM_LSUAT"] != DBNull.Value)
                        objDXVVDTO.LAI_SUAT_CAP_LIEN_NHIEM = Convert.ToDecimal(dr["CAP_LNHIEM_LSUAT"]);
                    if (dr["HE_SO"] != DBNull.Value)
                        objDXVVDTO.HE_SO = Convert.ToInt32(dr["HE_SO"]);
                    if (dr["ID_DIABAN"] != DBNull.Value)
                        objDXVVDTO.ID_DIA_BAN = Convert.ToInt32(dr["ID_DIABAN"]);
                    if (dr["ID_KHANG"] != DBNull.Value)
                        objDXVVDTO.ID_KHANG = idKhachHang = Convert.ToInt32(dr["ID_KHANG"]);
                    if (dr["ID_NGUOI_DTN"] != DBNull.Value)
                        objDXVVDTO.ID_NGUOI_DTN = Convert.ToInt32(dr["ID_NGUOI_DTN"]);
                    if (dr["ID_NGUOI_QLY"] != DBNull.Value)
                        objDXVVDTO.ID_NGUOI_QLY = Convert.ToInt32(dr["ID_NGUOI_QLY"]);
                    if (dr["ID_SAN_PHAM"] != DBNull.Value)
                        objDXVVDTO.ID_SAN_PHAM = Convert.ToInt32(dr["ID_SAN_PHAM"]);
                    if (dr["KHOACH_HTHUC_LAP"] != DBNull.Value)
                        objDXVVDTO.KHOACH_HTHUC_LAP = Convert.ToString(dr["KHOACH_HTHUC_LAP"]);
                    if (dr["KHOACH_NGAY_LAP"] != DBNull.Value)
                        objDXVVDTO.KHOACH_NGAY_LAP = Convert.ToString(dr["KHOACH_NGAY_LAP"]);
                    if (dr["LSUAT_BDO"] != DBNull.Value)
                        objDXVVDTO.BIEN_DO_LAI_SUAT = Convert.ToDecimal(dr["LSUAT_BDO"]);
                    if (dr["LSUAT_CCAU"] != DBNull.Value)
                        objDXVVDTO.LAI_SUAT_CO_CAU = Convert.ToDecimal(dr["LSUAT_CCAU"]);
                    if (dr["LSUAT_CTRA"] != DBNull.Value)
                        objDXVVDTO.LAI_SUAT_CHAM_TRA = Convert.ToDecimal(dr["LSUAT_CTRA"]);
                    if (dr["LSUAT_LOAI"] != DBNull.Value)
                        objDXVVDTO.LOAI_LAI_SUAT = Convert.ToString(dr["LSUAT_LOAI"]);
                    if (dr["LSUAT_MA"] != DBNull.Value)
                        objDXVVDTO.MA_LSUAT = maLaiSuat = Convert.ToString(dr["LSUAT_MA"]);
                    if (dr["LSUAT_QHAN"] != DBNull.Value)
                        objDXVVDTO.LAI_SUAT_QUA_HAN = Convert.ToDecimal(dr["LSUAT_QHAN"]);
                    if (dr["LSUAT_TSUAT"] != DBNull.Value)
                        objDXVVDTO.TAN_SUAT_LAI_SUAT = Convert.ToInt32(dr["LSUAT_TSUAT"]);
                    if (dr["LSUAT_TSUAT_DVI_TINH"] != DBNull.Value)
                        objDXVVDTO.DVT_TAN_SUAT = Convert.ToString(dr["LSUAT_TSUAT_DVI_TINH"]);
                    if (dr["LSUAT_VAY"] != DBNull.Value)
                        objDXVVDTO.LAI_SUAT_VAY = Convert.ToDecimal(dr["LSUAT_VAY"]);
                    if (dr["MA_DIABAN"] != DBNull.Value)
                        objDXVVDTO.MA_DIA_BAN = Convert.ToString(dr["MA_DIABAN"]);
                    if (dr["MA_DVI_QLY"] != DBNull.Value)
                        objDXVVDTO.MA_DVI_QLY = Convert.ToString(dr["MA_DVI_QLY"]);
                    if (dr["MA_DVI_TAO"] != DBNull.Value)
                        objDXVVDTO.MA_DVI_TAO = Convert.ToString(dr["MA_DVI_TAO"]);
                    if (dr["MA_DXVVVM"] != DBNull.Value)
                        objDXVVDTO.MA_DXVVVM = Convert.ToString(dr["MA_DXVVVM"]);
                    if (dr["MA_HMUC"] != DBNull.Value)
                        objDXVVDTO.MA_HMUC = Convert.ToString(dr["MA_HMUC"]);
                    if (dr["MA_KHANG"] != DBNull.Value)
                        objDXVVDTO.MA_KHANG = maKhangHang = Convert.ToString(dr["MA_KHANG"]);
                    if (dr["MA_NGUOI_DTN"] != DBNull.Value)
                        objDXVVDTO.MA_NGUOI_DTN = Convert.ToString(dr["MA_NGUOI_DTN"]);
                    if (dr["MA_NGUOI_QLY"] != DBNull.Value)
                        objDXVVDTO.MA_NGUOI_QLY = Convert.ToString(dr["MA_NGUOI_QLY"]);
                    if (dr["MA_SAN_PHAM"] != DBNull.Value)
                        objDXVVDTO.MA_SAN_PHAM = Convert.ToString(dr["MA_SAN_PHAM"]);
                    if (dr["MUC_DICH_VAY"] != DBNull.Value)
                        objDXVVDTO.MA_MUC_DICH_VAY = Convert.ToString(dr["MUC_DICH_VAY"]);
                    if (dr["NGANH_KINH_TE"] != DBNull.Value)
                        objDXVVDTO.MA_NGANH_KTE = Convert.ToString(dr["NGANH_KINH_TE"]);
                    if (dr["NGAY_CHUYEN_QH"] != DBNull.Value)
                        objDXVVDTO.NGAY_CHUYEN_QUA_HAN = Convert.ToString(dr["NGAY_CHUYEN_QH"]);
                    if (dr["NGAY_CNHAT"] != DBNull.Value)
                        objDXVVDTO.NGAY_CNHAT = Convert.ToString(dr["NGAY_CNHAT"]);
                    if (dr["NGAY_DAO_HAN"] != DBNull.Value)
                        objDXVVDTO.NGAY_DAO_HAN = Convert.ToString(dr["NGAY_DAO_HAN"]);
                    if (dr["NGAY_GIA_HAN"] != DBNull.Value)
                        objDXVVDTO.NGAY_GIA_HAN = Convert.ToString(dr["NGAY_GIA_HAN"]);
                    if (dr["NGAY_HD"] != DBNull.Value)
                        objDXVVDTO.NGAY_LAP = Convert.ToString(dr["NGAY_HD"]);
                    if (dr["NGAY_NHAP"] != DBNull.Value)
                        objDXVVDTO.NGAY_NHAP = Convert.ToString(dr["NGAY_NHAP"]);
                    if (dr["NGUOI_CNHAT"] != DBNull.Value)
                        objDXVVDTO.NGUOI_CNHAT = Convert.ToString(dr["NGUOI_CNHAT"]);
                    if (dr["NGUOI_NHAP"] != DBNull.Value)
                        objDXVVDTO.NGUOI_NHAP = Convert.ToString(dr["NGUOI_NHAP"]);
                    if (dr["PHI_MO_HD"] != DBNull.Value)
                        objDXVVDTO.PHI_MO_HOP_DONG = Convert.ToDecimal(dr["PHI_MO_HD"]);
                    if (dr["PHUONG_THUC_VAY"] != DBNull.Value)
                        objDXVVDTO.MA_PTHUC_VAY = Convert.ToString(dr["PHUONG_THUC_VAY"]);
                    if (dr["SO_GDICH"] != DBNull.Value)
                        objDXVVDTO.SO_GDICH = Convert.ToString(dr["SO_GDICH"]);
                    if (dr["SO_TIEN_CAN"] != DBNull.Value)
                        objDXVVDTO.SO_TIEN_CAN = Convert.ToDecimal(dr["SO_TIEN_CAN"]);
                    if (dr["SO_TIEN_GOC_MOI_KY"] != DBNull.Value)
                        objDXVVDTO.SO_TIEN_GOC_MOI_KY = Convert.ToDecimal(dr["SO_TIEN_GOC_MOI_KY"]);
                    if (dr["SO_TIEN_LAI_MOI_KY"] != DBNull.Value)
                        objDXVVDTO.SO_TIEN_LAI_MOI_KY = Convert.ToDecimal(dr["SO_TIEN_LAI_MOI_KY"]);
                    if (dr["SO_TIEN_MOI_KY"] != DBNull.Value)
                        objDXVVDTO.SO_TIEN_MOI_KY = Convert.ToDecimal(dr["SO_TIEN_MOI_KY"]);
                    if (dr["SO_TIEN_TKBB"] != DBNull.Value)
                        objDXVVDTO.SO_TIEN_TKBB = Convert.ToDecimal(dr["SO_TIEN_TKBB"]);
                    if (dr["SO_TIEN_TU_CO"] != DBNull.Value)
                        objDXVVDTO.SO_TIEN_CO = Convert.ToDecimal(dr["SO_TIEN_TU_CO"]);
                    if (dr["SO_TIEN_VAY"] != DBNull.Value)
                        objDXVVDTO.SO_TIEN_VAY = Convert.ToDecimal(dr["SO_TIEN_VAY"]);
                    if (dr["SO_TIEN_XIN_VAY"] != DBNull.Value)
                        objDXVVDTO.SO_TIEN_XIN_VAY = Convert.ToDecimal(dr["SO_TIEN_XIN_VAY"]);
                    if (dr["SO_TKHOAN_NHAN_NO"] != DBNull.Value)
                        objDXVVDTO.TAI_KHOAN_NHAN_NO = Convert.ToString(dr["SO_TKHOAN_NHAN_NO"]);
                    if (dr["TGIAN_VAY"] != DBNull.Value)
                        objDXVVDTO.THOI_GIAN_VAY = Convert.ToInt32(dr["TGIAN_VAY"]);
                    if (dr["TGIAN_VAY_DVI_TINH"] != DBNull.Value)
                        objDXVVDTO.DVT_THOI_GIAN_VAY = Convert.ToString(dr["TGIAN_VAY_DVI_TINH"]);
                    if (dr["TRGOC_DVI_TINH"] != DBNull.Value)
                        objDXVVDTO.DVT_SO_KY_TRA_GOC = Convert.ToString(dr["TRGOC_DVI_TINH"]);
                    if (dr["TRGOC_HTHUC"] != DBNull.Value)
                        objDXVVDTO.HINH_THUC_TRA_GOC = Convert.ToString(dr["TRGOC_HTHUC"]);
                    if (dr["TRGOC_SO_KY"] != DBNull.Value)
                        objDXVVDTO.SO_KY_TRA_GOC = Convert.ToInt32(dr["TRGOC_SO_KY"]);
                    if (dr["TRGOC_SO_TKHOAN"] != DBNull.Value)
                        objDXVVDTO.TAI_KHOAN_TRA_GOC = Convert.ToString(dr["TRGOC_SO_TKHOAN"]);
                    if (dr["TRLAI_DVI_TINH"] != DBNull.Value)
                        objDXVVDTO.DVT_SO_KY_TRA_LAI = Convert.ToString(dr["TRLAI_DVI_TINH"]);
                    if (dr["TRLAI_HTHUC"] != DBNull.Value)
                        objDXVVDTO.HINH_THUC_TRA_LAI = Convert.ToString(dr["TRLAI_HTHUC"]);
                    if (dr["TRLAI_SO_KY"] != DBNull.Value)
                        objDXVVDTO.SO_KY_TRA_LAI = Convert.ToInt32(dr["TRLAI_SO_KY"]);
                    if (dr["TRLAI_SO_TKHOAN"] != DBNull.Value)
                        objDXVVDTO.TAI_KHOAN_TRA_LAI = Convert.ToString(dr["TRLAI_SO_TKHOAN"]);
                    if (dr["TTHAI_BGHI"] != DBNull.Value)
                        objDXVVDTO.TTHAI_BGHI = Convert.ToString(dr["TTHAI_BGHI"]);
                    if (dr["TTHAI_GIAI_NGAN"] != DBNull.Value)
                        objDXVVDTO.TTHAI_GIAI_NGAN = Convert.ToString(dr["TTHAI_GIAI_NGAN"]);
                    if (dr["TTHAI_LY_DO"] != DBNull.Value)
                        objDXVVDTO.TTHAI_LY_DO = Convert.ToString(dr["TTHAI_LY_DO"]);
                    if (dr["TTHAI_NVU"] != DBNull.Value)
                        objDXVVDTO.TTHAI_NVU = Convert.ToString(dr["TTHAI_NVU"]);
                    if (dr["MA_VONG_VAY"] != DBNull.Value)
                        maVongVay = Convert.ToString(dr["MA_VONG_VAY"]);
                    if (dr["TCHAT_GOC_VAY"] != DBNull.Value)
                        hinhThucGoc = Convert.ToString(dr["TCHAT_GOC_VAY"]);
                    if (dr["TCHAT_KY_HAN"] != DBNull.Value)
                        hinhThucKyHan = Convert.ToString(dr["TCHAT_KY_HAN"]);
                    if (dr["TTHAI_CAP_PDUYET"] != DBNull.Value)
                        TThaiNVu = Convert.ToString(dr["TTHAI_CAP_PDUYET"]);
                    if (dr["CAN_CU_TINH_LAI_QH"] != DBNull.Value)
                        canCuTinhQH = Convert.ToString(dr["CAN_CU_TINH_LAI_QH"]);
                    if (dr["TY_LE_TINH_LAI_QH"] != DBNull.Value)
                        tyleLaiQuaHan = Convert.ToDecimal(dr["TY_LE_TINH_LAI_QH"]);

                    txtSoGiaoDich.Text = objDXVVDTO.MA_DXVVVM;
                    teldtNgayLapHD.Value = LDateTime.StringToDate(objDXVVDTO.NGAY_LAP, ApplicationConstant.defaultDateTimeFormat);
                    txtMaKHang.Text = objDXVVDTO.MA_KHANG;
                    txtMaKHang.Tag = Convert.ToInt32(dr["ID_KHANG"]);
                    txtTenKHang.Text = dr["TEN_KHANG"].ToString();
                    txtMaNhom.Text = objDXVVDTO.MA_DIA_BAN;
                    txtMaNhom.Tag = objDXVVDTO.ID_DIA_BAN;
                    txtTenNhomTruong.Text = dr["TEN_NHOM_TRUONG"].ToString();
                    txtSoCMND.Text = dr["DD_SO_GTLQ"].ToString();
                    txtThonAp.Text = dr["TEN_CUM"].ToString();
                    txtPhuongXa.Text = dr["TEN_KVUC"].ToString();
                    txtQuanHuyen.Text = dr["TEN_QUAN_HUYEN"].ToString();
                    
                    cmbLoaiSanPham.SelectedIndex = lstSourceLoaiSanPham.IndexOf(lstSourceLoaiSanPham.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["MA_LOAI_SAN_PHAM"].ToString())));
                    txtMaSanPham.Text = objDXVVDTO.MA_SAN_PHAM;
                    txtMaSanPham.Tag = objDXVVDTO.ID_SAN_PHAM;
                    lblTenSanPham.Content = dr["TEN_SAN_PHAM"].ToString();
                    telThoiGianVay.Value = objDXVVDTO.THOI_GIAN_VAY;
                    cmbThoiHanVay.SelectedIndex = lstSourceTHanVay.IndexOf(lstSourceTHanVay.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objDXVVDTO.DVT_THOI_GIAN_VAY)));
                    telLaiSuat.Value = Convert.ToDouble(objDXVVDTO.LAI_SUAT_VAY);
                    cmbThoiHanLSuat.SelectedIndex = lstSourceTHanLaiSuat.IndexOf(lstSourceTHanLaiSuat.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objDXVVDTO.DVT_TAN_SUAT)));
                    telSoTienVay.Value = Convert.ToDouble(objDXVVDTO.SO_TIEN_XIN_VAY);
                    cmbLoaiTien.SelectedIndex = lstSourceTienTe.IndexOf(lstSourceTienTe.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["MA_LOAI_TIEN"].ToString())));
                    cmbHinhThucTraGoc.SelectedIndex = lstSourceHinhThucTraGoc.IndexOf(lstSourceHinhThucTraGoc.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objDXVVDTO.HINH_THUC_TRA_GOC)));
                    cmbHinhThucTraLai.SelectedIndex = lstSourceHinhThucTraLai.IndexOf(lstSourceHinhThucTraLai.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objDXVVDTO.HINH_THUC_TRA_LAI)));

                    if (objDXVVDTO.SO_TIEN_TKBB.IsNullOrEmpty())
                    {
                        chkTaoSoTietKiem.IsChecked = true;
                        telSoTienTKBB.Value = 0;
                    }
                    else
                    {
                        chkTaoSoTietKiem.IsChecked = false;
                        telSoTienTKBB.Value = Convert.ToDouble(objDXVVDTO.SO_TIEN_TKBB);
                    }
                    txtMaMucDich.Text = objDXVVDTO.MA_MUC_DICH_VAY;
                    lblTenMucDich.Content = LLanguage.SearchResourceByKey(dr["TEN_MUC_DICH_VAY"].ToString());
                    cmbTThaiCapTinDung.SelectedIndex = lstSourceTrangThaiCapTinDung.IndexOf(lstSourceTrangThaiCapTinDung.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(TThaiNVu)));
                    telnumHanMucPheDuyet.Value = Convert.ToDouble(objDXVVDTO.SO_TIEN_VAY);
                    txtLyDoTuChoi.Text = objDXVVDTO.TTHAI_LY_DO;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(dr["TTHAI_NVU"].ToString());
                    dt = ds.Tables["DXVV_MUC_DICH_VAY_CT"];
                    lstMucDichVayVon = new List<DXVVVM_MUC_DICH_VAY_VON>();
                    foreach (DataRow drv in dt.Rows)
                    {
                        DXVVVM_MUC_DICH_VAY_VON objMucDich = new DXVVVM_MUC_DICH_VAY_VON();
                        objMucDich.ID_DXVVVM = objDXVVDTO.ID;
                        objMucDich.MA_DXVVVM = objDXVVDTO.MA_DXVVVM;
                        objMucDich.MA_MUC_DICH_VAY_VON = drv["MA_MDICH_VAY"].ToString();
                        objMucDich.SO_TIEN = Convert.ToDecimal(drv["SO_TIEN"]);
                        objMucDich.TEN_MUC_DICH_VAY_VON = "";
                        lstMucDichVayVon.Add(objMucDich);
                    }
                    raddgrMucDich.ItemsSource = null;
                    raddgrMucDich.ItemsSource = lstMucDichVayVon;
                    obj.OBJ_DON_XIN_VAY = objDXVVDTO;
                    CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON);
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

        void SetKeHoachSuDungVonVay(DataSet ds)
        {
            try
            {
                DataTable dt = ds.Tables["NGUON_TRA_NO"];
                lstNguonTraNoChi = new List<DXVVVM_NGUON_TRA_NO>();
                lstNguonTraNoThu = new List<DXVVVM_NGUON_TRA_NO>();
                DXVVVM_NGUON_TRA_NO objNguonTraNo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    objNguonTraNo = new DXVVVM_NGUON_TRA_NO();
                    objNguonTraNo.ID_DXVVVM = obj.OBJ_DON_XIN_VAY.ID;
                    objNguonTraNo.MA_DXVVVM = obj.OBJ_DON_XIN_VAY.MA_DXVVVM;
                    objNguonTraNo.TEN_THU_NHAP_CHI_PHI = dr["TEN_TNHAP_CPHI"].ToString();
                    objNguonTraNo.TY_GIA = Convert.ToDecimal(dr["TY_GIA"]);
                    objNguonTraNo.SO_TIEN = Convert.ToDecimal(dr["SO_TIEN"]);
                    objNguonTraNo.LOAI_TIEN = dr["LOAI_TIEN"].ToString();
                    if (dr["LOAI_TNHAP_CPHI"].ToString().Equals("THU_NHAP"))
                        lstNguonTraNoThu.Add(objNguonTraNo);
                    else if (dr["LOAI_TNHAP_CPHI"].ToString().Equals("CHI_PHI"))
                        lstNguonTraNoChi.Add(objNguonTraNo);
                    else if (dr["LOAI_TNHAP_CPHI"].ToString().Equals("THU_NHAP_TU_SXKD"))
                    {
                        cmbLoaiTienThuNhap.SelectedIndex = lstSourceTienTe.IndexOf(lstSourceTienTe.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objNguonTraNo.LOAI_TIEN)));
                        telNguonTuSXKD.Value = Convert.ToDouble(objNguonTraNo.SO_TIEN);
                    }
                }
                raddgrThuNhap.ItemsSource = null;
                raddgrThuNhap.ItemsSource = lstNguonTraNoThu;
                raddgrChiPhi.ItemsSource = null;
                raddgrChiPhi.ItemsSource = lstNguonTraNoChi;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void SetTabThongTinBaoLanhTKe(DataSet ds)
        {
            try
            {
                DataTable dt = ds.Tables["TTIN_THUAKE"];
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    txtNguoiThuaKe.Text = dr["MA_KHANG"].ToString();
                    txtNguoiThuaKe.Tag = Convert.ToInt32(dr["ID_KHANG"]);
                    if (dr["TEN_NGUOI_DTN"] != DBNull.Value)
                        txtTenNguoiThuaKe.Text = dr["TEN_NGUOI_DTN"].ToString();
                    if (dr["TEN_BO"] != DBNull.Value)
                        txtTenBoNguoiThuaKe.Text = dr["TEN_BO"].ToString();
                    if (dr["GTLQ_SO"] != DBNull.Value)
                        txtSoCMNDNguoiThuaKe.Text = dr["GTLQ_SO"].ToString();
                    if (dr["NGAY_CAP"] != DBNull.Value && !dr["NGAY_CAP"].ToString().IsNullOrEmptyOrSpace() && dr["NGAY_CAP"].ToString().IsDate(ApplicationConstant.defaultDateTimeFormat))
                        txtNgayCapCMNDNguoiThuaKe.Value = LDateTime.StringToDate(dr["NGAY_CAP"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    if (dr["NOI_CAP"] != DBNull.Value)
                        txtNoiCapCMNDNguoiThuaKe.Text = dr["NOI_CAP"].ToString();
                    if (dr["DIA_CHI"] != DBNull.Value)
                        txtDiaChiNguoiThuaKe.Text = dr["DIA_CHI"].ToString();
                    if (dr["SO_DIEN_THOAI"] != DBNull.Value)
                        txtDienThoaiNguoiThuaKe.Text = dr["SO_DIEN_THOAI"].ToString();
                    if (dr["SO_DI_DONG"] != DBNull.Value)
                        txtDiDongNguoiThuaKe.Text = dr["SO_DI_DONG"].ToString();
                    if (dr["MOI_QUAN_HE"] != DBNull.Value)
                        txtQuanHeNguoiThuaKe.SelectedIndex = lstSourceMoiQuanHe.IndexOf(lstSourceMoiQuanHe.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["MOI_QUAN_HE"].ToString())));
                }

                dt = ds.Tables["TTIN_BLANH"];
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    if (dr["MA_KHANG"] != DBNull.Value && !dr["MA_KHANG"].ToString().IsNullOrEmptyOrSpace())
                    {
                        txtNguoiBaoLanh.Text = dr["MA_KHANG"].ToString();
                        txtNguoiBaoLanh.Tag = Convert.ToInt32(dr["ID_KHANG"]);
                    }
                    if (dr["TEN_NGUOI_DTN"] != DBNull.Value)
                        txtTenNguoiBaoLanh.Text = dr["TEN_NGUOI_DTN"].ToString();
                    if (dr["TEN_BO"] != DBNull.Value)
                        txtTenBoNguoiBaoLanh.Text = dr["TEN_BO"].ToString();
                    if (dr["GTLQ_SO"] != DBNull.Value)
                        txtSoCMNDNguoiBaoLanh.Text = dr["GTLQ_SO"].ToString();
                    if (dr["NGAY_CAP"] != DBNull.Value && !dr["NGAY_CAP"].ToString().IsNullOrEmptyOrSpace() && dr["NGAY_CAP"].ToString().IsDate(ApplicationConstant.defaultDateTimeFormat))
                        txtNgayCapCMNDNguoiBaoLanh.Value = LDateTime.StringToDate(dr["NGAY_CAP"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    if (dr["NOI_CAP"] != DBNull.Value)
                        txtNoiCapCMNDNguoiBaoLanh.Text = dr["NOI_CAP"].ToString();
                    if (dr["DIA_CHI"] != DBNull.Value)
                        txtDiaChiNguoiBaoLanh.Text = dr["DIA_CHI"].ToString();
                    if (dr["SO_DIEN_THOAI"] != DBNull.Value)
                        txtDienThoaiNguoiBaoLanh.Text = dr["SO_DIEN_THOAI"].ToString();
                    if (dr["SO_DI_DONG"] != DBNull.Value)
                        txtDiDongNguoiBaoLanh.Text = dr["SO_DI_DONG"].ToString();
                    if (dr["MOI_QUAN_HE"] != DBNull.Value)
                        txtQuanHeNguoiBaoLanh.SelectedIndex = lstSourceMoiQuanHe.IndexOf(lstSourceMoiQuanHe.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["MOI_QUAN_HE"].ToString())));
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

        bool getThongTinKhachHang(int id, string maKHang)
        {
            bool bResutl = true;
            try
            {
                DataSet ds = new KhachHangProcess().getThongTinCoBanKHTheoMa(id, maKHang, 0);
                if (ds.Tables[0].Rows.Count == 1)
                {
                    idKhachHang = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]);
                    maKhangHang = ds.Tables[0].Rows[0]["MA_KHANG"].ToString();
                    tenKhachHang = ds.Tables[0].Rows[0]["TEN_KHANG"].ToString();
                    soGTLQ = ds.Tables[0].Rows[0]["DD_GTLQ_SO"].ToString();
                    tenNhom = ds.Tables[0].Rows[0]["TEN_CUM"].ToString() + "-" + ds.Tables[0].Rows[0]["TEN_NHOM"].ToString();
                }
            }
            catch (Exception ex)
            {
                bResutl = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
            }
            return bResutl;
        }

        void ResetForm()
        {
            obj = null;
            txtSoGiaoDich.Text = "";
            teldtNgayLapHD.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            txtMaKHang.Text = "";
            txtMaKHang.Tag = "";
            txtTenKHang.Text = "";
            txtMaNhom.Text = "";
            txtTenNhomTruong.Text = "";
            txtSoCMND.Text = "";
            txtThonAp.Text = "";
            txtPhuongXa.Text = "";
            txtQuanHuyen.Text = "";
            cmbLoaiSanPham.SelectedIndex = 0;
            txtMaSanPham.Text = "";
            txtMaSanPham.Tag = "0";
            lblTenSanPham.Content = LLanguage.SearchResourceByKey("U.TinDung.ucDonVayVonCT_01.TenSanPham");
            telThoiGianVay.Value = 0;
            cmbThoiHanVay.SelectedIndex = 0;
            telLaiSuat.Value = 0;
            cmbThoiHanLSuat.SelectedIndex = 0;
            telSoTienVay.Value = 0;
            cmbLoaiTien.SelectedIndex = lstSourceTienTe.IndexOf(lstSourceTienTe.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(ClientInformation.MaDongNoiTe)));
            chkTaoSoTietKiem.IsChecked = false;
            telSoTienTKBB.Value = 0;
            txtMaMucDich.Text = "";
            txtMaMucDich.Tag = "";
            lblTenMucDich.Content = LLanguage.SearchResourceByKey("U.TinDung.ucDonVayVonCT_01.TenMucDichSuDungVon");
            lstMucDichVayVon = new List<DXVVVM_MUC_DICH_VAY_VON>();
            raddgrMucDich.ItemsSource = lstMucDichVayVon;
            cmbTThaiCapTinDung.SelectedIndex = 0;
            telnumHanMucPheDuyet.Value = 0;
            txtLyDoTuChoi.Text = "";
            lstNguonTraNoChi = new List<DXVVVM_NGUON_TRA_NO>();
            lstNguonTraNoThu = new List<DXVVVM_NGUON_TRA_NO>();
            lstNguonTraNo = new List<DXVVVM_NGUON_TRA_NO>();
            DXVVVM_NGUON_TRA_NO objNguonTraNo = new DXVVVM_NGUON_TRA_NO();
            objNguonTraNo.LOAI_THU_NHAP_CHI_PHI = "THU_NHAP";
            objNguonTraNo.TEN_THU_NHAP_CHI_PHI = LLanguage.SearchResourceByKey("U.TinDung.ucDonVayVonCT_01.TongThuNhap");
            lstNguonTraNoThu.Add(objNguonTraNo);
            objNguonTraNo = new DXVVVM_NGUON_TRA_NO();
            objNguonTraNo.LOAI_THU_NHAP_CHI_PHI = "CHI_PHI";
            objNguonTraNo.TEN_THU_NHAP_CHI_PHI = LLanguage.SearchResourceByKey("U.TinDung.ucDonVayVonCT_01.TongChiPhi");
            lstNguonTraNoChi.Add(objNguonTraNo);
            raddgrChiPhi.ItemsSource = lstNguonTraNoChi;
            raddgrThuNhap.ItemsSource = lstNguonTraNoThu;
            telSoTienTraNoHangThang.Value = 0;
            txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(BusinessConstant.TrangThaiSuDung.SU_DUNG.layGiaTri());
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            txtNguoiCapNhat.Text = "";
            teldtNgayCNhat.Value = null;
            cmbThoiHanVay.IsEnabled = true;
            telSoTienVay.IsReadOnly = false;
            telThoiGianVay.IsReadOnly = false;
            idDiaBan = 0;
            idHDTDVM = 0;
            maKhangHang = "";
            maLaiSuat = "";
            maDonVi = ClientInformation.MaDonViGiaoDich;
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, "", mnuMain, DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON);
            
        }

        void beforeModify()
        {
            action = DatabaseConstant.Action.SUA;
            SetEnabledAllControls(true);
            OnModify();
        }

        private void SetEnabledAllControls(bool enable)
        {
            teldtNgayLapHD.IsEnabled = enable;
            txtMaKHang.IsEnabled = enable;
            cmbLoaiSanPham.IsEnabled = enable;
            txtMaSanPham.IsEnabled = enable;
            telThoiGianVay.IsEnabled = enable;
            cmbThoiHanVay.IsEnabled = enable;
            telLaiSuat.IsEnabled = enable;
            cmbThoiHanLSuat.IsEnabled = enable;
            telSoTienVay.IsEnabled = enable;
            cmbHinhThucTraGoc.IsEnabled = enable;
            cmbHinhThucTraLai.IsEnabled = enable;
            btnMaKHang.IsEnabled = enable;
            btnMaSanPham.IsEnabled = enable;
            txtMaMucDich.IsEnabled = enable;
            btnAddMucDich.IsEnabled = enable;
            btnCommitMucDich.IsEnabled = enable;
            btnCancelMucDich.IsEnabled = enable;
            btnDeleteMucDich.IsEnabled = enable;
            raddgrMucDich.IsReadOnly = !enable;
            btnAddThuNhap.IsEnabled = enable;
            btnCommitThuNhap.IsEnabled = enable;
            btnCancelThuNhap.IsEnabled = enable;
            btnDeleteThuNhap.IsEnabled = enable;
            raddgrThuNhap.IsReadOnly = !enable;
            btnAddChiPhi.IsEnabled = enable;
            btnCommitChiPhi.IsEnabled = enable;
            btnCancelChiPhi.IsEnabled = enable;
            btnDeleteChiPhi.IsEnabled = enable;
            raddgrChiPhi.IsReadOnly = !enable;
            txtNguoiThuaKe.IsEnabled = enable;
            btnNguoiThuaKe.IsEnabled = enable;
            txtNguoiBaoLanh.IsEnabled = enable;
            btnNguoiBaoLanh.IsEnabled = enable;
            txtTenNguoiBaoLanh.IsEnabled = enable;
            txtTenBoNguoiBaoLanh.IsEnabled = enable;
            txtSoCMNDNguoiBaoLanh.IsEnabled = enable;
            txtNgayCapCMNDNguoiBaoLanh.IsEnabled = enable;
            txtNoiCapCMNDNguoiBaoLanh.IsEnabled = enable;
            txtDiaChiNguoiBaoLanh.IsEnabled = enable;
            txtDienThoaiNguoiBaoLanh.IsEnabled = enable;
            txtDiDongNguoiBaoLanh.IsEnabled = enable;
            txtQuanHeNguoiBaoLanh.IsEnabled = enable;
            telNguonTuSXKD.IsEnabled = enable;
            telSoTienTKBB.IsEnabled = enable;
            chkTaoSoTietKiem.IsEnabled = enable;
            if (enable)
            {
                if (maVongVay.IsNullOrEmptyOrSpace())
                    cmbThoiHanVay.IsEnabled = enable;
                else
                    cmbThoiHanVay.IsEnabled = false;
                if (maLaiSuat.IsNullOrEmptyOrSpace())
                    cmbThoiHanLSuat.IsEnabled = enable;
                else
                    cmbThoiHanLSuat.IsEnabled = true;
                if (hinhThucGoc.Equals(BusinessConstant.TINH_CHAT_VONG_VAY.CO_DINH.layGiaTri()))
                {
                    telSoTienVay.IsEnabled = false;
                }
                else
                {
                    telSoTienVay.IsEnabled = enable;
                }
                if (hinhThucKyHan.Equals(BusinessConstant.TINH_CHAT_VONG_VAY.CO_DINH.layGiaTri()))
                {
                    telThoiGianVay.IsEnabled = false;
                    cmbThoiHanVay.IsEnabled = false;
                }
                else
                {
                    telThoiGianVay.IsEnabled = enable;
                    cmbThoiHanVay.IsEnabled = enable;
                }
            }
        }

        private void Release()
        {
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
            DatabaseConstant.Table.TD_DXVVVM,
            DatabaseConstant.Action.SUA,
            lstId);
        }

        void OnModify()
        {
            UtilitiesProcess process = new UtilitiesProcess();
            lstId.Add(idHDTDVM);
            bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
            DatabaseConstant.Table.TD_DXVVVM,
            action,
            lstId);
            SetEnabledAllControls(true);
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu,mnuMain,DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON);
        }

        bool Validation()
        {
            bool bReturn = true;
            if (teldtNgayLapHD.Value.IsNullOrEmpty())
            {
                CommonFunction.ThongBaoTrong(lblNgayLapHD.Content.ToString());
                teldtNgayLapHD.Focus();
                return false;
            }
            if (txtMaKHang.Tag.ToString().IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblMaKhachHang.Content.ToString());
                txtMaKHang.Focus();
                return false;
            }
            if (cmbLoaiSanPham.SelectedIndex < 0)
            {
                CommonFunction.ThongBaoTrong(lblLoaiSanPham.Content.ToString());
                cmbLoaiSanPham.Focus();
                return false;
            }
            if (txtMaSanPham.Tag.ToString().IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblSanPhamVay.Content.ToString());
                txtMaSanPham.Focus();
                return false;
            }
            if (telThoiGianVay.Value.IsNullOrEmpty() || telThoiGianVay.Value.GetValueOrDefault()==0)
            {
                CommonFunction.ThongBaoTrong(lblThoiGianVay.Content.ToString());
                telThoiGianVay.Focus();
                return false;
            }
            if (cmbThoiHanVay.SelectedIndex < 0)
            {
                CommonFunction.ThongBaoTrong(lblThoiGianVay.Content.ToString());
                cmbThoiHanVay.Focus();
                return false;
            }
            if (telLaiSuat.Value.IsNullOrEmpty())
            {
                CommonFunction.ThongBaoTrong(lblLaiSuat.Content.ToString());
                telLaiSuat.Focus();
                return false;
            }
            if (cmbThoiHanLSuat.SelectedIndex < 0)
            {
                CommonFunction.ThongBaoTrong(lblLaiSuat.Content.ToString());
                cmbThoiHanLSuat.Focus();
                return false;
            }
            if (telSoTienVay.Value.IsNullOrEmpty() || telSoTienVay.Value.GetValueOrDefault() == 0)
            {
                CommonFunction.ThongBaoTrong(lblSoTienVay.Content.ToString());
                telSoTienVay.Focus();
                return false;
            }
            if (cmbHinhThucTraGoc.SelectedIndex < 0)
            {
                CommonFunction.ThongBaoTrong(lblHinhThucTraGoc.Content.ToString());
                cmbHinhThucTraGoc.Focus();
                return false;
            }
            if (cmbHinhThucTraLai.SelectedIndex < 0)
            {
                CommonFunction.ThongBaoTrong(lblHinhThucTraLai.Content.ToString());
                cmbHinhThucTraLai.Focus();
                return false;
            }
            if (txtMaMucDich.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblMucDichVay.Content.ToString());
                txtMaMucDich.Focus();
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
            if (txtSoGiaoDich.Text == "")
                iret = new TinDungProcess().DonXinVayVonTinDungViMo(DatabaseConstant.Action.THEM,ref obj, ref lstResponseDetail);
            else
                iret = new TinDungProcess().DonXinVayVonTinDungViMo(DatabaseConstant.Action.SUA, ref obj, ref lstResponseDetail);
            AfterSave(lstResponseDetail, iret);
        }

        void AfterSave(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            
            if (iret > 0)
            {
                action = DatabaseConstant.Action.XEM;
                SetInfomation();
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
                DatabaseConstant.Table.TD_DXVVVM,
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
            DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
            DatabaseConstant.Table.TD_DXVVVM,
            DatabaseConstant.Action.XOA,
            lstId);
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            if (iret < 1)
                SetInfomation();
            else
                ResetForm();
        }

        void OnDelete()
        {
            int iret = 0;
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            obj.DSACH_PHE_DUYET = PheDuyet.ToArray();
            iret = new TinDungProcess().DonXinVayVonTinDungViMo(DatabaseConstant.Action.XOA, ref obj, ref ResponseDetail);
            AfterDelete(txtSoGiaoDich.Text, ResponseDetail,iret);
        }

        void BeforeDelete()
        {
            if (tlbDelete.IsEnabled == false)
                return;
            try
            {
                Cursor = Cursors.Wait;
                if (!txtSoGiaoDich.Text.IsNullOrEmptyOrSpace())
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        PheDuyet = new List<CAP_PHE_DUYET>();
                        objCapPheDuyet.ACTION = DatabaseConstant.Action.XOA.getValue();
                        objCapPheDuyet.HAN_MUC_PHE_DUYET = Convert.ToDecimal(telSoTienVay.Value.GetValueOrDefault());
                        objCapPheDuyet.ID_TCHIEU = idHDTDVM;
                        objCapPheDuyet.MA_TCHIEU = txtSoGiaoDich.Text;
                        PheDuyet.Add(objCapPheDuyet);
                        lstId = new List<int>();
                        lstId.Add(idHDTDVM);
                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
                        DatabaseConstant.Table.TD_DXVVVM,
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
                DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
                DatabaseConstant.Table.TD_DXVVVM,
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
            DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
            DatabaseConstant.Table.TD_DXVVVM,
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
            iret = new TinDungProcess().DonXinVayVonTinDungViMo(DatabaseConstant.Action.DUYET, ref obj, ref ResponseDetail);
            AfterApprove(txtSoGiaoDich.Text, ResponseDetail);
        }

        void BeforeApprove()
        {
            if (!tlbApprove.IsEnabled)
                return;
            try
            {
                if (!txtSoGiaoDich.Text.IsNullOrEmptyOrSpace())
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        PheDuyet = new List<CAP_PHE_DUYET>();
                        objCapPheDuyet.TRANG_THAI = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                        objCapPheDuyet.ACTION = DatabaseConstant.Action.DUYET.getValue();
                        objCapPheDuyet.HAN_MUC_PHE_DUYET = objCapPheDuyet.MUC_PHE_DUYET = Convert.ToDecimal(telSoTienVay.Value.GetValueOrDefault());
                        objCapPheDuyet.MA_TCHIEU = obj.OBJ_DON_XIN_VAY.MA_DXVVVM;
                        objCapPheDuyet.MO_TA = txtMaKHang.Text + " : " + txtTenKHang.Text;
                        objCapPheDuyet.ID_TCHIEU = obj.OBJ_DON_XIN_VAY.ID;
                        objCapPheDuyet.MA_CAP_PHE_DUYET = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                        objCapPheDuyet.LOAI_TIEN = loaiTien;
                        PheDuyet.Add(objCapPheDuyet);
                        ucLyDoDS objLyDo = new ucLyDoDS(PheDuyet);
                        objLyDo.DuLieuTraVe = new ucLyDoDS.ReturnResutl(LayDuLieuPheDuyet);
                        Window window = new Window();
                        window.Content = objLyDo;
                        window.Title = LLanguage.SearchResourceByKey("U.DungChung.CapPheDuyet");
                        window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        window.ShowDialog();

                        lstId = new List<int>();
                        lstId.Add(idHDTDVM);
                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
                        DatabaseConstant.Table.TD_DXVVVM,
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
                DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
                DatabaseConstant.Table.TD_DXVVVM,
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
            DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
            DatabaseConstant.Table.TD_DXVVVM,
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
            iret = new TinDungProcess().DonXinVayVonTinDungViMo(DatabaseConstant.Action.TU_CHOI_DUYET, ref obj, ref ResponseDetail);
            AfterRefuse(txtSoGiaoDich.Text, ResponseDetail);
        }

        void BeforeRefuse()
        {
            if (!tlbRefuse.IsEnabled)
                return;
            try
            {
                if (!txtSoGiaoDich.Text.IsNullOrEmptyOrSpace())
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        PheDuyet = new List<CAP_PHE_DUYET>();
                        objCapPheDuyet.TRANG_THAI = BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri();
                        objCapPheDuyet.ACTION = DatabaseConstant.Action.TU_CHOI_DUYET.getValue();
                        objCapPheDuyet.HAN_MUC_PHE_DUYET = objCapPheDuyet.MUC_PHE_DUYET = 0;
                        objCapPheDuyet.MA_TCHIEU = obj.OBJ_DON_XIN_VAY.MA_DXVVVM;
                        objCapPheDuyet.MO_TA = txtMaKHang.Text + " : " + txtTenKHang.Text;
                        objCapPheDuyet.ID_TCHIEU = obj.OBJ_DON_XIN_VAY.ID;
                        objCapPheDuyet.MA_CAP_PHE_DUYET = BusinessConstant.TrangThaiNghiepVu.TU_CHOI_CAP_TIN_DUNG.layGiaTri();
                        objCapPheDuyet.LOAI_TIEN = loaiTien;
                        PheDuyet.Add(objCapPheDuyet);
                        ucLyDoDS objLyDo = new ucLyDoDS(PheDuyet);
                        objLyDo.DuLieuTraVe = new ucLyDoDS.ReturnResutl(LayDuLieuPheDuyet);
                        Window window = new Window();
                        window.Content = objLyDo;
                        window.Title = LLanguage.SearchResourceByKey("U.DungChung.CapPheDuyet");
                        window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        window.ShowDialog();
                        lstId = new List<int>();
                        lstId.Add(idHDTDVM);
                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
                        DatabaseConstant.Table.TD_DXVVVM,
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
                DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
                DatabaseConstant.Table.TD_DXVVVM,
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
            DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
            DatabaseConstant.Table.TD_DXVVVM,
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
            iret = new TinDungProcess().DonXinVayVonTinDungViMo(DatabaseConstant.Action.THOAI_DUYET, ref obj, ref ResponseDetail);
            AfterCancel(txtSoGiaoDich.Text, ResponseDetail);
        }

        void BeforeCancel()
        {
            if (!tlbCancel.IsEnabled)
                return;
            try
            {
                if (!txtSoGiaoDich.Text.IsNullOrEmptyOrSpace())
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        PheDuyet = new List<CAP_PHE_DUYET>();
                        objCapPheDuyet.TRANG_THAI = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                        objCapPheDuyet.ACTION = DatabaseConstant.Action.THOAI_DUYET.getValue();
                        objCapPheDuyet.HAN_MUC_PHE_DUYET = objCapPheDuyet.MUC_PHE_DUYET = Convert.ToDecimal(telSoTienVay.Value.GetValueOrDefault());
                        objCapPheDuyet.MA_TCHIEU = obj.OBJ_DON_XIN_VAY.MA_DXVVVM;
                        objCapPheDuyet.MO_TA = txtMaKHang.Text + " : " + txtTenKHang.Text;
                        objCapPheDuyet.ID_TCHIEU = obj.OBJ_DON_XIN_VAY.ID;
                        objCapPheDuyet.MA_CAP_PHE_DUYET = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                        objCapPheDuyet.LOAI_TIEN = loaiTien;
                        PheDuyet.Add(objCapPheDuyet);
                        ucLyDoDS objLyDo = new ucLyDoDS(PheDuyet);
                        objLyDo.DuLieuTraVe = new ucLyDoDS.ReturnResutl(LayDuLieuPheDuyet);
                        Window window = new Window();
                        window.Content = objLyDo;
                        window.Title = LLanguage.SearchResourceByKey("U.DungChung.CapPheDuyet");
                        window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        window.ShowDialog();
                        lstId = new List<int>();
                        lstId.Add(idHDTDVM);
                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
                        DatabaseConstant.Table.TD_DXVVVM,
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
                DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
                DatabaseConstant.Table.TD_DXVVVM,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void OnPrint()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(obj.OBJ_DON_XIN_VAY.MA_DXVVVM))
            {
                LMessage.ShowMessage("M.DungChung.KhongCoThongTin", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                if (ClientInformation.Company.Equals("M7MFI") || ClientInformation.Company.Equals("BINHKHANH"))
                {
                    // Cảnh báo phải lựa chọn hợp đồng
                    List<string> lstMaHDTD = new List<string>();
                    List<DataRow> listDataRow = getListSeletedDataRow();
                    if (listDataRow.Count == 0)
                    {
                        LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    else
                    {
                        foreach (DataRow dr in listDataRow)
                        {
                            lstMaHDTD.Add(dr["MA_HDTDVM"].ToString());
                        }
                    }

                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                    DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();

                    GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                    objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.TDVM_HDTD;
                    objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON;

                    List<Presentation.Process.BaoCaoServiceRef.TDVM_HDTD> listTDVM_HDTD = new List<Presentation.Process.BaoCaoServiceRef.TDVM_HDTD>();
                    foreach (string maHDTDVM in lstMaHDTD)
                    {
                        Presentation.Process.BaoCaoServiceRef.TDVM_HDTD objGDKT_GIAO_DICH = new Presentation.Process.BaoCaoServiceRef.TDVM_HDTD();
                        objGDKT_GIAO_DICH.SoHopDong = maHDTDVM;
                        listTDVM_HDTD.Add(objGDKT_GIAO_DICH);
                    }


                    doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                    doiTuongBaoCao.listTDVM_HDTD = listTDVM_HDTD.ToArray();
                    xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
                }
                else if (ClientInformation.Company.Equals("BANTAYVANG"))
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                    List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                    string idkhachhang = "";
                    string makhachhang = "";
                    string tenkhachhang = "";
                    string gioitinh = "";
                    string ngaysinh = "";
                    string diachi = "";
                    string socmnd = "";
                    string ngaycap = "";
                    string noicap = "";

                    string sotienvay = "";
                    string mahopdong = "";
                    string ngaybaocao = "";
                    string masanpham = "";

                    KhachHangProcess khProcess = new KhachHangProcess();
                    DataSet dsKhachHang = new DataSet();
                    dsKhachHang = khProcess.getThongTinKHTheoID(idKhachHang);
                    DataTable dtKhangHSo = null;
                    bool IsCheckedMauMoi = false;
                    if (IsCheckedMauMoi == false)
                    {
                        if (dsKhachHang != null && dsKhachHang.Tables.Count > 0)
                        {
                            dtKhangHSo = dsKhachHang.Tables[0];
                            makhachhang = dtKhangHSo.Rows[0]["ma_khang"].ToString(); ;
                            tenkhachhang = dtKhangHSo.Rows[0]["ten_khang"].ToString();
                            diachi = dtKhangHSo.Rows[0]["dia_chi"].ToString();
                            gioitinh = dtKhangHSo.Rows[0]["DD_GIOI_TINH"].ToString();
                            ngaysinh = dtKhangHSo.Rows[0]["DD_NGAY_SINH"].ToString();
                            socmnd = dtKhangHSo.Rows[0]["DD_GTLQ_SO"].ToString();
                            ngaycap = dtKhangHSo.Rows[0]["DD_GTLQ_NGAY_CAP"].ToString();
                            noicap = dtKhangHSo.Rows[0]["DD_GTLQ_NOI_CAP"].ToString();
                        }
                        
                        idkhachhang = idKhachHang.ToString();
                        sotienvay = "0";
                        mahopdong = txtSoGiaoDich.Text;
                        masanpham = txtMaSanPham.Text;
                    }
                    else
                    {
                        makhachhang = "";
                        idkhachhang = "";
                        sotienvay = "0";
                        mahopdong = "";
                        masanpham = "";
                        tenkhachhang = "";
                        diachi = "";
                        gioitinh = "";
                        ngaysinh = "";
                        socmnd = "";
                        ngaycap = "";
                        noicap = "";
                    }
                    ngaybaocao = ClientInformation.NgayLamViecHienTai;

                    lstThamSo.Add(new ThamSoBaoCao("@MaKhachHang", makhachhang, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@IdKhachHang", idkhachhang, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@SoTienVay", sotienvay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaHopDongTD", mahopdong, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaSanPhamTD", masanpham, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MA_DON_V", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

                    lstThamSo.Add(new ThamSoBaoCao("P_TEN_KHACH_HANG", tenkhachhang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_TEN_THUONG_GOI", tenkhachhang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_GIOI_TINH", gioitinh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NGAY_SINH", ngaysinh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_DIA_CHI", diachi, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_SO_CMND", socmnd, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NGAY_CAP", ngaycap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NOI_CAP", noicap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.TDVM_DON_VAY_VON);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
                else
                {
                }
            }
        }

        private void OnPreviewDonVayVon()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(obj))
            {
                LMessage.ShowMessage("M.DungChung.KhongCoThongTin", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                if (LObject.IsNullOrEmpty(obj.OBJ_DON_XIN_VAY))
                {
                    LMessage.ShowMessage("M.DungChung.KhongCoThongTin", LMessage.MessageBoxType.Warning);
                    return;
                }

                if (LObject.IsNullOrEmpty(obj.OBJ_DON_XIN_VAY.MA_DXVVVM))
                {
                    LMessage.ShowMessage("M.DungChung.KhongCoThongTin", LMessage.MessageBoxType.Warning);
                    return;
                }

                if (ClientInformation.Company.Equals("M7MFI") || ClientInformation.Company.Equals("BINHKHANH"))
                {
                    // Cảnh báo phải lựa chọn hợp đồng
                    List<string> lstMaHDTD = new List<string>();
                    List<DataRow> listDataRow = getListSeletedDataRow();
                    if (listDataRow.Count == 0)
                    {
                        LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    else
                    {
                        foreach (DataRow dr in listDataRow)
                        {
                            lstMaHDTD.Add(dr["MA_HDTDVM"].ToString());
                        }
                    }

                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                    DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();

                    GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                    objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.TDVM_HDTD;
                    objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON;

                    List<Presentation.Process.BaoCaoServiceRef.TDVM_HDTD> listTDVM_HDTD = new List<Presentation.Process.BaoCaoServiceRef.TDVM_HDTD>();
                    foreach (string maHDTDVM in lstMaHDTD)
                    {
                        Presentation.Process.BaoCaoServiceRef.TDVM_HDTD objGDKT_GIAO_DICH = new Presentation.Process.BaoCaoServiceRef.TDVM_HDTD();
                        objGDKT_GIAO_DICH.SoHopDong = maHDTDVM;
                        listTDVM_HDTD.Add(objGDKT_GIAO_DICH);
                    }


                    doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                    doiTuongBaoCao.listTDVM_HDTD = listTDVM_HDTD.ToArray();
                    xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
                }
                else if (ClientInformation.Company.Equals("BANTAYVANG") || ClientInformation.Company.Equals("HOCVIENNGANHANG"))
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                    List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                    string idkhachhang = "";
                    string makhachhang = "";
                    string tenkhachhang = "";
                    string gioitinh = "";
                    string ngaysinh = "";
                    string diachi = "";
                    string socmnd = "";
                    string ngaycap = "";
                    string noicap = "";

                    string sotienvay = "";
                    string mahopdong = "";
                    string ngaybaocao = "";
                    string masanpham = "";

                    KhachHangProcess khProcess = new KhachHangProcess();
                    DataSet dsKhachHang = new DataSet();
                    dsKhachHang = khProcess.getThongTinKHTheoID(idKhachHang);
                    DataTable dtKhangHSo = null;
                    bool IsCheckedMauMoi = false;
                    if (IsCheckedMauMoi == false)
                    {
                        if (dsKhachHang != null && dsKhachHang.Tables.Count > 0)
                        {
                            dtKhangHSo = dsKhachHang.Tables[0];
                            makhachhang = dtKhangHSo.Rows[0]["ma_khang"].ToString(); ;
                            tenkhachhang = dtKhangHSo.Rows[0]["ten_khang"].ToString();
                            diachi = dtKhangHSo.Rows[0]["dia_chi"].ToString();
                            gioitinh = dtKhangHSo.Rows[0]["DD_GIOI_TINH"].ToString();
                            ngaysinh = dtKhangHSo.Rows[0]["DD_NGAY_SINH"].ToString();
                            socmnd = dtKhangHSo.Rows[0]["DD_GTLQ_SO"].ToString();
                            ngaycap = dtKhangHSo.Rows[0]["DD_GTLQ_NGAY_CAP"].ToString();
                            noicap = dtKhangHSo.Rows[0]["DD_GTLQ_NOI_CAP"].ToString();
                        }

                        idkhachhang = idKhachHang.ToString();
                        sotienvay = "0";
                        mahopdong = txtSoGiaoDich.Text;
                        masanpham = txtMaSanPham.Text;
                    }
                    else
                    {
                        makhachhang = "";
                        idkhachhang = "";
                        sotienvay = "0";
                        mahopdong = "";
                        masanpham = "";
                        tenkhachhang = "";
                        diachi = "";
                        gioitinh = "";
                        ngaysinh = "";
                        socmnd = "";
                        ngaycap = "";
                        noicap = "";
                    }
                    ngaybaocao = ClientInformation.NgayLamViecHienTai;

                    lstThamSo.Add(new ThamSoBaoCao("@MaKhachHang", makhachhang, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@IdKhachHang", idkhachhang, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@SoTienVay", sotienvay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaHopDongTD", mahopdong, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaSanPhamTD", masanpham, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MA_DON_V", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

                    lstThamSo.Add(new ThamSoBaoCao("P_TEN_KHACH_HANG", tenkhachhang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_TEN_THUONG_GOI", tenkhachhang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_GIOI_TINH", gioitinh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NGAY_SINH", ngaysinh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_DIA_CHI", diachi, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_SO_CMND", socmnd, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NGAY_CAP", ngaycap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NOI_CAP", noicap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.TDVM_DON_VAY_VON);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
                else if (ClientInformation.Company.Equals("PHUTHO"))
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                    List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                    string idkhachhang = "";
                    string makhachhang = "";
                    string tenkhachhang = "";
                    string gioitinh = "";
                    string ngaysinh = "";
                    string diachi = "";
                    string socmnd = "";
                    string ngaycap = "";
                    string noicap = "";

                    string sotienvay = "";
                    string mahopdong = "";
                    string ngaybaocao = "";
                    string masanpham = "";

                    KhachHangProcess khProcess = new KhachHangProcess();
                    DataSet dsKhachHang = new DataSet();
                    dsKhachHang = khProcess.getThongTinKHTheoID(idKhachHang);
                    DataTable dtKhangHSo = null;
                    bool IsCheckedMauMoi = false;
                    if (IsCheckedMauMoi == false)
                    {
                        if (dsKhachHang != null && dsKhachHang.Tables.Count > 0)
                        {
                            dtKhangHSo = dsKhachHang.Tables[0];
                            makhachhang = dtKhangHSo.Rows[0]["ma_khang"].ToString(); ;
                            tenkhachhang = dtKhangHSo.Rows[0]["ten_khang"].ToString();
                            diachi = dtKhangHSo.Rows[0]["dia_chi"].ToString();
                            gioitinh = dtKhangHSo.Rows[0]["DD_GIOI_TINH"].ToString();
                            ngaysinh = dtKhangHSo.Rows[0]["DD_NGAY_SINH"].ToString();
                            socmnd = dtKhangHSo.Rows[0]["DD_GTLQ_SO"].ToString();
                            ngaycap = dtKhangHSo.Rows[0]["DD_GTLQ_NGAY_CAP"].ToString();
                            noicap = dtKhangHSo.Rows[0]["DD_GTLQ_NOI_CAP"].ToString();
                        }

                        idkhachhang = idKhachHang.ToString();
                        sotienvay = "0";
                        mahopdong = txtSoGiaoDich.Text;
                        masanpham = txtMaSanPham.Text;
                    }
                    else
                    {
                        makhachhang = "";
                        idkhachhang = "";
                        sotienvay = "0";
                        mahopdong = "";
                        masanpham = "";
                        tenkhachhang = "";
                        diachi = "";
                        gioitinh = "";
                        ngaysinh = "";
                        socmnd = "";
                        ngaycap = "";
                        noicap = "";
                    }
                    ngaybaocao = ClientInformation.NgayLamViecHienTai;

                    lstThamSo.Add(new ThamSoBaoCao("@MaKhachHang", makhachhang, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@IdKhachHang", idkhachhang, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@SoTienVay", sotienvay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@SoDonXinVayVon", mahopdong, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaSanPhamTD", masanpham, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaPhongGD", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

                    lstThamSo.Add(new ThamSoBaoCao("P_TEN_KHACH_HANG", tenkhachhang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_TEN_THUONG_GOI", tenkhachhang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_GIOI_TINH", gioitinh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NGAY_SINH", ngaysinh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_DIA_CHI", diachi, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_SO_CMND", socmnd, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NGAY_CAP", ngaycap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NOI_CAP", noicap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoPhuTho(DatabaseConstant.DanhSachBaoCaoPhuTho.TDVM_DON_VAY_VON);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
                else if (ClientInformation.Company.Equals("BENTRE"))
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                    List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                    string idkhachhang = "";
                    string makhachhang = "";
                    string tenkhachhang = "";
                    string gioitinh = "";
                    string ngaysinh = "";
                    string diachi = "";
                    string socmnd = "";
                    string ngaycap = "";
                    string noicap = "";

                    string sotienvay = "";
                    string mahopdong = "";
                    string ngaybaocao = "";
                    string masanpham = "";

                    KhachHangProcess khProcess = new KhachHangProcess();
                    DataSet dsKhachHang = new DataSet();
                    dsKhachHang = khProcess.getThongTinKHTheoID(idKhachHang);
                    DataTable dtKhangHSo = null;
                    bool IsCheckedMauMoi = false;
                    if (IsCheckedMauMoi == false)
                    {
                        if (dsKhachHang != null && dsKhachHang.Tables.Count > 0)
                        {
                            dtKhangHSo = dsKhachHang.Tables[0];
                            makhachhang = dtKhangHSo.Rows[0]["ma_khang"].ToString(); ;
                            tenkhachhang = dtKhangHSo.Rows[0]["ten_khang"].ToString();
                            diachi = dtKhangHSo.Rows[0]["dia_chi"].ToString();
                            gioitinh = dtKhangHSo.Rows[0]["DD_GIOI_TINH"].ToString();
                            ngaysinh = dtKhangHSo.Rows[0]["DD_NGAY_SINH"].ToString();
                            socmnd = dtKhangHSo.Rows[0]["DD_GTLQ_SO"].ToString();
                            ngaycap = dtKhangHSo.Rows[0]["DD_GTLQ_NGAY_CAP"].ToString();
                            noicap = dtKhangHSo.Rows[0]["DD_GTLQ_NOI_CAP"].ToString();
                        }

                        idkhachhang = idKhachHang.ToString();
                        sotienvay = "0";
                        mahopdong = txtSoGiaoDich.Text;
                        masanpham = txtMaSanPham.Text;
                    }
                    else
                    {
                        makhachhang = "";
                        idkhachhang = "";
                        sotienvay = "0";
                        mahopdong = "";
                        masanpham = "";
                        tenkhachhang = "";
                        diachi = "";
                        gioitinh = "";
                        ngaysinh = "";
                        socmnd = "";
                        ngaycap = "";
                        noicap = "";
                    }
                    ngaybaocao = ClientInformation.NgayLamViecHienTai;

                    lstThamSo.Add(new ThamSoBaoCao("@MaKhachHang", makhachhang, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@IdKhachHang", idkhachhang, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@SoTienVay", sotienvay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaHopDongTD", mahopdong, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaSanPhamTD", masanpham, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MA_DON_V", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

                    lstThamSo.Add(new ThamSoBaoCao("P_TEN_KHACH_HANG", tenkhachhang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_TEN_THUONG_GOI", tenkhachhang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_GIOI_TINH", gioitinh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NGAY_SINH", ngaysinh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_DIA_CHI", diachi, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_SO_CMND", socmnd, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NGAY_CAP", ngaycap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NOI_CAP", noicap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoBenTre(DatabaseConstant.DanhSachBaoCaoBenTre.TDVM_DON_XIN_VAY_VON_NHOM_BAO_LANH);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
                else
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                    List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                    string idkhachhang = "";
                    string makhachhang = "";
                    string tenkhachhang = "";
                    string gioitinh = "";
                    string ngaysinh = "";
                    string diachi = "";
                    string socmnd = "";
                    string ngaycap = "";
                    string noicap = "";

                    string sotienvay = "";
                    string mahopdong = "";
                    string ngaybaocao = "";
                    string masanpham = "";

                    KhachHangProcess khProcess = new KhachHangProcess();
                    DataSet dsKhachHang = new DataSet();
                    dsKhachHang = khProcess.getThongTinKHTheoID(idKhachHang);
                    DataTable dtKhangHSo = null;
                    bool IsCheckedMauMoi = false;
                    if (IsCheckedMauMoi == false)
                    {
                        if (dsKhachHang != null && dsKhachHang.Tables.Count > 0)
                        {
                            dtKhangHSo = dsKhachHang.Tables[0];
                            makhachhang = dtKhangHSo.Rows[0]["ma_khang"].ToString(); ;
                            tenkhachhang = dtKhangHSo.Rows[0]["ten_khang"].ToString();
                            diachi = dtKhangHSo.Rows[0]["dia_chi"].ToString();
                            gioitinh = dtKhangHSo.Rows[0]["DD_GIOI_TINH"].ToString();
                            ngaysinh = dtKhangHSo.Rows[0]["DD_NGAY_SINH"].ToString();
                            socmnd = dtKhangHSo.Rows[0]["DD_GTLQ_SO"].ToString();
                            ngaycap = dtKhangHSo.Rows[0]["DD_GTLQ_NGAY_CAP"].ToString();
                            noicap = dtKhangHSo.Rows[0]["DD_GTLQ_NOI_CAP"].ToString();
                        }

                        idkhachhang = idKhachHang.ToString();
                        sotienvay = "0";
                        mahopdong = txtSoGiaoDich.Text;
                        masanpham = txtMaSanPham.Text;
                    }
                    else
                    {
                        makhachhang = "";
                        idkhachhang = "";
                        sotienvay = "0";
                        mahopdong = "";
                        masanpham = "";
                        tenkhachhang = "";
                        diachi = "";
                        gioitinh = "";
                        ngaysinh = "";
                        socmnd = "";
                        ngaycap = "";
                        noicap = "";
                    }
                    ngaybaocao = ClientInformation.NgayLamViecHienTai;

                    lstThamSo.Add(new ThamSoBaoCao("@MaKhachHang", makhachhang, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@IdKhachHang", idkhachhang, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@SoTienVay", sotienvay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaHopDongTD", mahopdong, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaSanPhamTD", masanpham, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaPhongGD", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

                    lstThamSo.Add(new ThamSoBaoCao("P_TEN_KHACH_HANG", tenkhachhang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_TEN_THUONG_GOI", tenkhachhang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_GIOI_TINH", gioitinh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NGAY_SINH", ngaysinh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_DIA_CHI", diachi, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_SO_CMND", socmnd, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NGAY_CAP", ngaycap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NOI_CAP", noicap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoPhuTho(DatabaseConstant.DanhSachBaoCaoPhuTho.TDVM_DON_VAY_VON);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
            }
        }

        /// <summary>
        /// Lấy danh sách được chọn
        /// </summary>
        /// <returns></returns>
        private List<DataRow> getListSeletedDataRow()
        {
            List<DataRow> listDataRow = new List<DataRow>();
            return listDataRow;
        }

        private void SetInfomation()
        {
            try
            {
                idHDTDVM = obj.OBJ_DON_XIN_VAY.ID;
                TThaiNVu = obj.OBJ_DON_XIN_VAY.TTHAI_NVU;
                txtSoGiaoDich.Text = obj.OBJ_DON_XIN_VAY.MA_DXVVVM;
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(obj.OBJ_DON_XIN_VAY.TTHAI_NVU);
                cmbTThaiCapTinDung.SelectedIndex = lstSourceTrangThaiCapTinDung.IndexOf(lstSourceTrangThaiCapTinDung.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(obj.OBJ_DON_XIN_VAY.TTHAI_NVU)));
                telnumHanMucPheDuyet.Value = Convert.ToDouble(obj.OBJ_DON_XIN_VAY.SO_TIEN_VAY);
                action = DatabaseConstant.Action.XEM;
                CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON);
                SetEnabledAllControls(false);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
        #endregion
    }
}
