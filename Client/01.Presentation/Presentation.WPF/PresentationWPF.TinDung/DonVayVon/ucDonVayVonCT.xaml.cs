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
    /// Interaction logic for ucDonVayVonCT.xaml
    /// </summary>
    public partial class ucDonVayVonCT : UserControl
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
        private Presentation.Process.TinDungServiceRef.TDVM_HDTD objHDTDVM = new Presentation.Process.TinDungServiceRef.TDVM_HDTD();
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
        List<DON_XIN_VAY_KHOACH_CT> lstDonVayKHoach = new List<DON_XIN_VAY_KHOACH_CT>();
        List<DON_XIN_VAY_KHOACH_CT> lstDonVayThuNhap = new List<DON_XIN_VAY_KHOACH_CT>();
        List<DON_XIN_VAY_KHOACH_CT> lstDonVayChiPhi = new List<DON_XIN_VAY_KHOACH_CT>();

        List<AutoCompleteEntry> lstSourceMucDichVayVon = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiSanPham = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceSanPhamVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTHanVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHinhThucTraGoc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHinhThucTraLai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHanMuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNguonVon = new List<AutoCompleteEntry>();
        private int idDiaBan = 0;
        private string maVongVay = "";
        private int heSo = 0;
        private int idHDTDVM = 0;
        private string maDonVi;
        string soTienHanMuc = "0";
        string soTienGoc = "0";
        string soTienLai = "0";
        string capPheDuyet = "";
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucDonVayVonCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDung;component/DonVayVon/ucDonVayVonCT.xaml", ref Toolbar, ref mnuMain);
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

            if (!ClientInformation.Company.Equals("PHUTHO"))
            {
                cmbNguonVon.Visibility = Visibility.Collapsed;
                lbNguonVon.Visibility = Visibility.Collapsed;
            }
            else
            {
                lbTKBB.Visibility = Visibility.Collapsed;
                telSoTienTKBBMoiKy.Visibility = Visibility.Collapsed;
            }
        }

        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.TinDung.DonVayVon.ucDonVayVonCT", "raddgrDSThanhVien");
            foreach (List<string> lst in arr)
            {
                object item = raddgrDSThanhVien.Columns[lst.First()];
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
            arr = hethong.SetVisibleControl("PresentationWPF.TinDung.DonVayVon.ucDonVayVonCT", "raddgrDSThuNhap");
            foreach (List<string> lst in arr)
            {
                object item = raddgrDSThuNhap.Columns[lst.First()];
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
            arr = hethong.SetVisibleControl("PresentationWPF.TinDung.DonVayVon.ucDonVayVonCT", "RibbonButton");
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

        void InitEventHandler()
        {
            txtMaKHang.KeyDown += new KeyEventHandler(txtMaKHang_KeyDown);
            raddgrDSThanhVien.CellEditEnded += new EventHandler<GridViewCellEditEndedEventArgs>(raddgrDSThanhVien_CellEditEnded);
            raddgrDSThanhVien.CellValidating += new EventHandler<GridViewCellValidatingEventArgs>(raddgrDSThanhVien_CellValidating);
            raddgrDSThanhVien.RowValidating += new EventHandler<GridViewRowValidatingEventArgs>(raddgrDSThanhVien_RowValidating);
            raddgrDSThuNhap.CellEditEnded += new EventHandler<GridViewCellEditEndedEventArgs>(raddgrDSThuNhap_CellEditEnded);
            raddgrDSThuNhap.CellValidating += new EventHandler<GridViewCellValidatingEventArgs>(raddgrDSThuNhap_CellValidating);
            raddgrDSThuNhap.RowValidating += new EventHandler<GridViewRowValidatingEventArgs>(raddgrDSThuNhap_RowValidating);
            this.Unloaded += new RoutedEventHandler(ucDonVayVonCT_Unloaded);
            cmbLoaiSanPham.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiSanPham_SelectionChanged);
            cmbSanPham.SelectionChanged += new SelectionChangedEventHandler(cmbSanPham_SelectionChanged);
            btnMaKHang.Click += new RoutedEventHandler(btnMaKHang_Click);
            cmbHanMuc.SelectionChanged += new SelectionChangedEventHandler(cmbHanMuc_SelectionChanged);
        }

        void ucDonVayVonCT_Unloaded(object sender, RoutedEventArgs e)
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
            if (LObject.IsNullOrEmpty(lstDonVayKHoach)) lstDonVayKHoach = new List<DON_XIN_VAY_KHOACH_CT>();
            raddgrDSThanhVien.ItemsSource = lstDonVayKHoach;
        }

        void KhoiTaoComboBox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.MUC_DICH_VAY_VON.getValue());
            auto.GenAutoComboBox(ref lstSourceMucDichVayVon, ref cmbMucDichVay, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.LOAI_SAN_PHAM_TDUNG.getValue());
            auto.GenAutoComboBox(ref lstSourceLoaiSanPham, ref cmbLoaiSanPham, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
            maDonVi = ClientInformation.MaDonVi;
            LoadComboBoxSanPham();
            cmbSanPham.SelectedIndex = -1;
            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DVI_TINH_KY_HAN.getValue());
            auto.GenAutoComboBox(ref lstSourceTHanVay, ref cmbThoiHanVay, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.HINH_THUC_TRA_GOC.getValue());
            auto.GenAutoComboBox(ref lstSourceHinhThucTraGoc, ref cmbHinhThucTraGoc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, BusinessConstant.HINH_THUC_TRA_GOC.DINH_KY.layGiaTri());
            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.HINH_THUC_TRA_LAI.getValue());
            auto.GenAutoComboBox(ref lstSourceHinhThucTraLai, ref cmbHinhThucTraLai, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, BusinessConstant.HINH_THUC_TRA_GOC.DINH_KY.layGiaTri());
            lstDieuKien.Clear();
            maDonVi = ClientInformation.MaDonVi;
            LoadComboBoxNguonVon();
            //cmbNguonVon.SelectedIndex = -1;
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
            else if (strTinhNang.Equals("PreviewDonVayVonKhac"))
            {
                OnPreviewDonVayVonKhac();
            }
            else if (strTinhNang.Equals("PreviewLichTraDan"))
            {
                OnPreviewLichTraDan();
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
            else if (strTinhNang.Equals("PreviewDonVayVonKhac"))
            {
                OnPreviewDonVayVonKhac();
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

        void cmbSanPham_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSanPham.Items.Count > 0)
            {
                maVongVay = lstSourceSanPhamVay.ElementAt(cmbSanPham.SelectedIndex).KeywordStrings[2];
                LoadComboBoxHanMuc();
            }
            else
            {
                maVongVay = "";
                cmbHanMuc.Items.Clear();
            }
        }

        void cmbLoaiSanPham_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbLoaiSanPham.SelectedIndex > -1)
            {
                LoadComboBoxSanPham();
            }
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
                if (!ClientInformation.Company.Equals("PHUTHO"))
                {
                    popupProcess.getPopupInformation("POPUP_DS_KHACHHANG_TDVM", lstDieuKien);
                }
                else
                    popupProcess.getPopupInformation("POPUP_DS_KHACHHANG_TDVM_PHUTHO", lstDieuKien);
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

        private void txtMaKHang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
                btnMaKHang_Click(sender, null);
        }

        void cmbHanMuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbHanMuc.SelectedIndex > -1)
            {
                AutoCompleteEntry au = lstSourceHanMuc.ElementAt(cmbHanMuc.SelectedIndex);
                telThoiGianVay.Value = Convert.ToDouble(au.KeywordStrings[2]);
                cmbThoiHanVay.SelectedIndex = lstSourceTHanVay.IndexOf(lstSourceTHanVay.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(au.KeywordStrings[3])));
                telSoTienHoanTra.Value = Convert.ToDouble(au.KeywordStrings[4]);
                telSoTienTKBBMoiKy.Value = Convert.ToDouble(au.KeywordStrings[7]);
            }
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

        void LoadComboBoxSanPham()
        {
            AutoCompleteEntry auLoaiSanPham = lstSourceLoaiSanPham.ElementAt(cmbLoaiSanPham.SelectedIndex);
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add("''"+maDonVi+"''");
            lstDieuKien.Add(auLoaiSanPham.KeywordStrings.FirstOrDefault());
            lstDieuKien.Add(LDateTime.DateToString(teldtNgayLapHD.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat));
            lstDieuKien.Add(idDiaBan.ToString());
            lstSourceSanPhamVay.Clear();
            cmbSanPham.Items.Clear();
            new AutoComboBox().GenAutoComboBox(ref lstSourceSanPhamVay, ref cmbSanPham, "COMBOBOX_SAN_PHAM_TD", lstDieuKien);
        }

        void LoadComboBoxNguonVon()
        {
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.MaDonVi);
            lstSourceNguonVon.Clear();
            cmbNguonVon.Items.Clear();
            new AutoComboBox().GenAutoComboBox(ref lstSourceNguonVon, ref cmbNguonVon, "COMBOBOX_HD_NGUON_VON", lstDieuKien);
        }

        void LoadComboBoxHanMuc()
        {
            if (soTienHanMuc.IsNullOrEmptyOrSpace())
                soTienHanMuc = "0";
            if (soTienGoc.IsNullOrEmptyOrSpace())
                soTienGoc = "0";
            if (soTienLai.IsNullOrEmptyOrSpace())
                soTienLai = "0";
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.MaDonVi);
            lstDieuKien.Add(maVongVay);
            lstDieuKien.Add(soTienHanMuc);
            lstDieuKien.Add(heSo.ToString());
            lstDieuKien.Add(soTienGoc);
            lstDieuKien.Add(soTienLai);
            lstSourceHanMuc.Clear();
            cmbHanMuc.Items.Clear();
            new AutoComboBox().GenAutoComboBox(ref lstSourceHanMuc, ref cmbHanMuc, "COMBOBOX_HMUC_VONG_VAY_TD", lstDieuKien);
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
                if (!ClientInformation.Company.Equals("PHUTHO"))
                {
                    popupProcess.getPopupInformation("POPUP_DS_KHACHHANG_TDVM", lstDieuKien);
                }
                else
                    popupProcess.getPopupInformation("POPUP_DS_KHACHHANG_TDVM_PHUTHO", lstDieuKien);
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
        #endregion

        /// <summary>
        /// Xu ly nghiep vu
        /// </summary>
        #region Xu ly nghiep vu
        void GetDataForm(BusinessConstant.TrangThaiBanGhi bghi, BusinessConstant.TrangThaiNghiepVu nghiepvu)
        {
            try
            {
                AutoCompleteEntry auSanPham = lstSourceSanPhamVay.ElementAt(cmbSanPham.SelectedIndex);
                AutoCompleteEntry auKyHan = lstSourceTHanVay.ElementAt(cmbThoiHanVay.SelectedIndex);
                AutoCompleteEntry auMucDichVay = lstSourceMucDichVayVon.ElementAt(cmbMucDichVay.SelectedIndex);
                AutoCompleteEntry auSoTinXinVay = lstSourceHanMuc.ElementAt(cmbHanMuc.SelectedIndex);
                AutoCompleteEntry auHinhThucTraGoc = lstSourceHinhThucTraGoc.ElementAt(cmbHinhThucTraGoc.SelectedIndex);
                AutoCompleteEntry auHinhThucTraLai = lstSourceHinhThucTraLai.ElementAt(cmbHinhThucTraLai.SelectedIndex);
                if(LObject.IsNullOrEmpty(objHDTDVM)) objHDTDVM = new Presentation.Process.TinDungServiceRef.TDVM_HDTD();
                if(LObject.IsNullOrEmpty(objHDTDVM.HDTD_VM)) objHDTDVM.HDTD_VM = new TD_HDTDVM();
                lstDonVayChiPhi = raddgrDSThanhVien.ItemsSource as List<DON_XIN_VAY_KHOACH_CT>;
                lstDonVayThuNhap = raddgrDSThuNhap.ItemsSource as List<DON_XIN_VAY_KHOACH_CT>;
                if (!LObject.IsNullOrEmpty(lstDonVayChiPhi))
                    lstDonVayChiPhi.ForEach(f => f.LOAI_KHOACH = "CHI_PHI");
                else
                    lstDonVayChiPhi = new List<DON_XIN_VAY_KHOACH_CT>();
                if (!LObject.IsNullOrEmpty(lstDonVayThuNhap))
                    lstDonVayThuNhap.ForEach(f => f.LOAI_KHOACH = "THU_NHAP");
                else
                    lstDonVayThuNhap = new List<DON_XIN_VAY_KHOACH_CT>();
                lstDonVayKHoach = new List<DON_XIN_VAY_KHOACH_CT>();
                lstDonVayKHoach.AddRange(lstDonVayChiPhi);
                lstDonVayKHoach.AddRange(lstDonVayThuNhap);
                objHDTDVM.HDTD_VM.CAP_LNHIEM = "";
                objHDTDVM.HDTD_VM.HE_SO = heSo;
                objHDTDVM.HDTD_VM.ID = idHDTDVM;
                objHDTDVM.HDTD_VM.ID_DIABAN = idDiaBan;
                objHDTDVM.HDTD_VM.ID_KHANG = idKhachHang;
                objHDTDVM.HDTD_VM.ID_SAN_PHAM = Convert.ToInt32(auSanPham.KeywordStrings[1]);
                objHDTDVM.HDTD_VM.KHOACH_HTHUC_LAP = "";
                objHDTDVM.HDTD_VM.KHOACH_NGAY_LAP = "";
                objHDTDVM.HDTD_VM.LSUAT_MA = auSanPham.KeywordStrings[3];
                objHDTDVM.HDTD_VM.LSUAT_TSUAT_DVI_TINH = auKyHan.KeywordStrings.FirstOrDefault();
                objHDTDVM.HDTD_VM.MA_HDTDVM = txtSoGiaoDich.Text;
                objHDTDVM.HDTD_VM.MA_KHANG = txtMaKHang.Text;
                objHDTDVM.HDTD_VM.MA_SAN_PHAM = auSanPham.KeywordStrings.FirstOrDefault();
                objHDTDVM.HDTD_VM.MUC_DICH_VAY = auMucDichVay.KeywordStrings.FirstOrDefault();
                objHDTDVM.HDTD_VM.NGANH_KINH_TE = "";
                objHDTDVM.HDTD_VM.NGAY_HD = LDateTime.DateToString(teldtNgayLapHD.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
                if (!LObject.IsNullOrEmpty(txtSoGiaoDich.Tag))
                    objHDTDVM.HDTD_VM.SO_GDICH = txtSoGiaoDich.Tag.ToString();
                objHDTDVM.HDTD_VM.SO_TIEN_CAN = Convert.ToDecimal(telnumVonCanCo.Value.GetValueOrDefault());
                objHDTDVM.HDTD_VM.SO_TIEN_TU_CO = Convert.ToDecimal(telnumVonTuCo.Value.GetValueOrDefault());
                objHDTDVM.HDTD_VM.SO_TIEN_XIN_VAY = Convert.ToDecimal(auSoTinXinVay.KeywordStrings[0]);
                objHDTDVM.HDTD_VM.SO_TIEN_GOC_MOI_KY = Convert.ToDecimal(auSoTinXinVay.KeywordStrings[5]);
                objHDTDVM.HDTD_VM.SO_TIEN_LAI_MOI_KY = Convert.ToDecimal(auSoTinXinVay.KeywordStrings[6]);
                objHDTDVM.HDTD_VM.SO_TIEN_MOI_KY = objHDTDVM.HDTD_VM.SO_TIEN_GOC_MOI_KY + objHDTDVM.HDTD_VM.SO_TIEN_LAI_MOI_KY;
                objHDTDVM.HDTD_VM.TGIAN_VAY = Convert.ToInt32(telThoiGianVay.Value.GetValueOrDefault());
                objHDTDVM.HDTD_VM.TGIAN_VAY_DVI_TINH = auKyHan.KeywordStrings[0];
                objHDTDVM.HDTD_VM.TRGOC_DVI_TINH = auKyHan.KeywordStrings[0];
                objHDTDVM.HDTD_VM.TRGOC_HTHUC = auHinhThucTraGoc.KeywordStrings[0];
                objHDTDVM.HDTD_VM.TRGOC_SO_KY = objHDTDVM.HDTD_VM.TGIAN_VAY;
                objHDTDVM.HDTD_VM.TRLAI_DVI_TINH = auHinhThucTraLai.KeywordStrings[0];
                objHDTDVM.HDTD_VM.TRLAI_HTHUC = auHinhThucTraLai.KeywordStrings[0];
                if (ClientInformation.Company.Equals("PHUTHO"))
                {
                    AutoCompleteEntry auNguonVon = lstSourceNguonVon.ElementAt(cmbNguonVon.SelectedIndex);
                    objHDTDVM.HDTD_VM.NV_MA_HOP_DONG = auNguonVon.KeywordStrings[0];
                    objHDTDVM.HDTD_VM.NV_LOAI_NVON = auNguonVon.KeywordStrings[3];
                }
                objHDTDVM.HDTD_VM.TRLAI_SO_KY = objHDTDVM.HDTD_VM.TGIAN_VAY;
                objHDTDVM.DSACH_DON_XIN_VAY_CT = lstDonVayKHoach.ToArray();
                objHDTDVM.HDTD_VM.TTHAI_NVU = nghiepvu.layGiaTri();
                objHDTDVM.HDTD_VM.TTHAI_BGHI = bghi.layGiaTri();
                objHDTDVM.HDTD_VM.SO_TIEN_VAY = objHDTDVM.HDTD_VM.SO_TIEN_XIN_VAY;
                objHDTDVM.HDTD_VM.SO_TIEN_TKBB = Convert.ToDecimal(telSoTienTKBBMoiKy.Value.GetValueOrDefault());
                if (objHDTDVM.HDTD_VM.ID > 0)
                {
                    objHDTDVM.HDTD_VM.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    objHDTDVM.HDTD_VM.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                }
                else
                {
                    objHDTDVM.HDTD_VM.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    objHDTDVM.HDTD_VM.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    objHDTDVM.HDTD_VM.MA_DVI_QLY = ClientInformation.MaDonVi;
                    objHDTDVM.HDTD_VM.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    objHDTDVM.HDTD_VM.TTHAI_GIAI_NGAN = "GIAI_NGAN";
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
                Dispatcher.CurrentDispatcher.DelayInvoke("SetDataForm", () =>
                    {
                        ds = new TinDungProcess().GetThongTinDonXinVayVonTinDung(dtPar);
                        Dispatcher.CurrentDispatcher.DelayInvoke("SetTabThongTinChung", () =>
                        {
                            SetTabThongTinChung(ds);
                        }, TimeSpan.FromSeconds(0));

                        Dispatcher.CurrentDispatcher.DelayInvoke("SetKeHoachSuDungVonVay", () =>
                        {
                            SetKeHoachSuDungVonVay(ds);
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
                objHDTDVM = new Presentation.Process.TinDungServiceRef.TDVM_HDTD();
                objHDTDVM.HDTD_VM = new TD_HDTDVM();
                if (!LObject.IsNullOrEmpty(ds) && ds.Tables.Count > 1)
                {
                    DataTable dt = ds.Tables["TTIN_CTIET"];
                    if (dt.Rows.Count > 0)
                    {
                        maDonVi = dt.Rows[0]["MA_DVI_QLY"].ToString();
                        txtSoGiaoDich.Text = objHDTDVM.HDTD_VM.MA_HDTDVM = dt.Rows[0]["MA_HDTDVM"].ToString();
                        txtSoGiaoDich.Tag = objHDTDVM.HDTD_VM.SO_GDICH = objHDTDVM.SO_GIAO_DICH = dt.Rows[0]["SO_GDICH"].ToString();
                        idHDTDVM = objHDTDVM.HDTD_VM.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                        teldtNgayLapHD.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_HD"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                        objHDTDVM.HDTD_VM.NGAY_HD = dt.Rows[0]["NGAY_HD"].ToString();
                        txtMaKHang.Text = maKhangHang = objHDTDVM.HDTD_VM.MA_KHANG = dt.Rows[0]["MA_KHANG"].ToString();
                        txtMaKHang.Tag = idKhachHang = objHDTDVM.HDTD_VM.ID_KHANG = Convert.ToInt32(dt.Rows[0]["ID_KHANG"]);
                        txtTenKHang.Text = dt.Rows[0]["TEN_KHANG"].ToString();
                        txtSoCMND.Text = dt.Rows[0]["DD_GTLQ_SO"].ToString();
                        txtThonAp.Text = dt.Rows[0]["TEN_CUM"].ToString();
                        txtPhuongXa.Text = dt.Rows[0]["TEN_KVUC"].ToString();
                        txtQuanHuyen.Text = dt.Rows[0]["TEN_HUYEN"].ToString();
                        cmbMucDichVay.SelectedIndex = lstSourceMucDichVayVon.IndexOf(lstSourceMucDichVayVon.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dt.Rows[0]["MUC_DICH_VAY"].ToString())));
                        objHDTDVM.HDTD_VM.MUC_DICH_VAY = dt.Rows[0]["MUC_DICH_VAY"].ToString();
                        telnumVonCanCo.Value = Convert.ToDouble(dt.Rows[0]["SO_TIEN_CAN"]);
                        objHDTDVM.HDTD_VM.SO_TIEN_CAN = Convert.ToDecimal(dt.Rows[0]["SO_TIEN_CAN"]);
                        telnumVonTuCo.Value = Convert.ToDouble(dt.Rows[0]["SO_TIEN_TU_CO"]);
                        objHDTDVM.HDTD_VM.SO_TIEN_TU_CO = Convert.ToDecimal(dt.Rows[0]["SO_TIEN_TU_CO"]);
                        objHDTDVM.HDTD_VM.SO_TIEN_XIN_VAY = Convert.ToDecimal(dt.Rows[0]["SO_TIEN_XIN_VAY"]);
                        objHDTDVM.HDTD_VM.SO_TIEN_GOC_MOI_KY = Convert.ToDecimal(dt.Rows[0]["SO_TIEN_GOC_MOI_KY"]);
                        objHDTDVM.HDTD_VM.SO_TIEN_LAI_MOI_KY = Convert.ToDecimal(dt.Rows[0]["SO_TIEN_LAI_MOI_KY"]);
                        idDiaBan = Convert.ToInt32(dt.Rows[0]["ID_DIABAN"]);
                        telSoTienHoanTra.Value = (double)(objHDTDVM.HDTD_VM.SO_TIEN_GOC_MOI_KY.GetValueOrDefault() + objHDTDVM.HDTD_VM.SO_TIEN_LAI_MOI_KY.GetValueOrDefault());
                        cmbHinhThucTraGoc.SelectedIndex = lstSourceHinhThucTraGoc.IndexOf(lstSourceHinhThucTraGoc.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dt.Rows[0]["TRGOC_HTHUC"].ToString())));
                        cmbHinhThucTraLai.SelectedIndex = lstSourceHinhThucTraLai.IndexOf(lstSourceHinhThucTraLai.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dt.Rows[0]["TRLAI_HTHUC"].ToString())));
                        cmbLoaiSanPham.SelectedIndex = lstSourceLoaiSanPham.IndexOf(lstSourceLoaiSanPham.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dt.Rows[0]["LOAI_SAN_PHAM"].ToString())));
                        cmbSanPham.SelectedIndex = lstSourceSanPhamVay.IndexOf(lstSourceSanPhamVay.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dt.Rows[0]["MA_SAN_PHAM"].ToString())));
                        if (ClientInformation.Company.Equals("PHUTHO"))
                        {
                            cmbNguonVon.SelectedIndex = lstSourceNguonVon.IndexOf(lstSourceNguonVon.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dt.Rows[0]["NV_MA_HOP_DONG"].ToString())));
                        }
                        soTienHanMuc = Convert.ToDecimal(dt.Rows[0]["SO_TIEN_XIN_VAY"]).ToString("#");
                        soTienGoc = Convert.ToDecimal(dt.Rows[0]["SO_TIEN_GOC_MOI_KY"]).ToString("#");
                        soTienLai = Convert.ToDecimal(dt.Rows[0]["SO_TIEN_LAI_MOI_KY"]).ToString("#");
                        heSo = Convert.ToInt32(dt.Rows[0]["HE_SO"]);
                        LoadComboBoxHanMuc();
                        cmbHanMuc.SelectedIndex = lstSourceHanMuc.IndexOf(lstSourceHanMuc.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(soTienHanMuc)));
                        telThoiGianVay.Value = Convert.ToDouble(dt.Rows[0]["TGIAN_VAY"]);
                        cmbThoiHanVay.SelectedIndex = lstSourceTHanVay.IndexOf(lstSourceTHanVay.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dt.Rows[0]["TGIAN_VAY_DVI_TINH"].ToString())));
                        maDonVi = dt.Rows[0]["MA_DVI_QLY"].ToString();
                        telSoTienTKBBMoiKy.Value = Convert.ToDouble(dt.Rows[0]["SO_TIEN_TKBB"]);
                        objHDTDVM.HDTD_VM.MA_DVI_QLY = dt.Rows[0]["MA_DVI_QLY"].ToString();
                        objHDTDVM.HDTD_VM.MA_DVI_TAO = dt.Rows[0]["MA_DVI_TAO"].ToString();
                        objHDTDVM.HDTD_VM.NGAY_NHAP = dt.Rows[0]["NGAY_NHAP"].ToString();
                        objHDTDVM.HDTD_VM.NGUOI_NHAP = dt.Rows[0]["NGUOI_NHAP"].ToString();
                        TThaiNVu = dt.Rows[0]["TTHAI_NVU"].ToString();
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
                        lblTrangThaiCapDuyet.Content = dt.Rows[0]["TTHAI_NVU_CAP_DUYET"].ToString();
                    }
                    if (action == DatabaseConstant.Action.SUA)
                        beforeModify();
                    else
                        SetEnabledAllControls(false);
                    CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON);
                    if (dt.Rows[0]["TTHAI_NVU_CAP_DUYET"].ToString().Contains("DDU"))
                        tlbSubmit.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void SetKeHoachSuDungVonVay(DataSet ds)
        {
            try
            {
                lstDonVayKHoach = new List<DON_XIN_VAY_KHOACH_CT>();
                if (!LObject.IsNullOrEmpty(ds) && ds.Tables.Count > 1)
                {
                    DataTable dt = ds.Tables["KHOACH_SDUNG"];
                    foreach (DataRow dr in dt.Rows)
                    {
                        DON_XIN_VAY_KHOACH_CT objKHoach = new DON_XIN_VAY_KHOACH_CT();
                        objKHoach.ID = Convert.ToInt32(dr["ID"]);
                        objKHoach.ID_HDTDVM = idHDTDVM;
                        objKHoach.MA_DOI_TUONG = "VTD_DON_XIN_VAY_KHOACH_CT";
                        objKHoach.MA_DVI_TAO = objHDTDVM.HDTD_VM.MA_DVI_TAO;
                        objKHoach.MA_HDTDVM = objHDTDVM.HDTD_VM.MA_HDTDVM;
                        objKHoach.NGAY_DL = objHDTDVM.HDTD_VM.NGAY_HD;
                        objKHoach.NGAY_NHAP = objHDTDVM.HDTD_VM.NGAY_NHAP;
                        objKHoach.NGUOI_NHAP = objHDTDVM.HDTD_VM.NGUOI_NHAP;
                        objKHoach.TEN_BANG = "TD_HDTDVM";
                        objKHoach.TTHAI_BGHI = objHDTDVM.HDTD_VM.TTHAI_BGHI;
                        objKHoach.TTHAI_NVU = objHDTDVM.HDTD_VM.TTHAI_NVU;
                        objKHoach.VON_TU_CO_KY_HAN = dr["VON_TU_CO_KY_HAN"].ToString();
                        if (dr["VON_TU_CO_SO_TIEN"] != DBNull.Value)
                            objKHoach.VON_TU_CO_SO_TIEN = Convert.ToDecimal(dr["VON_TU_CO_SO_TIEN"]);
                        objKHoach.VON_XIN_VAY_KY_HAN = dr["VON_XIN_VAY_KY_HAN"].ToString();
                        if (dr["VON_XIN_VAY_SO_TIEN"] != DBNull.Value)
                            objKHoach.VON_XIN_VAY_SO_TIEN = Convert.ToDecimal(dr["VON_XIN_VAY_SO_TIEN"]);
                        objKHoach.LOAI_KHOACH = dr["LOAI_KHOACH"].ToString();
                        objKHoach.TEN_KHOACH = dr["TEN_KHOACH"].ToString();
                        lstDonVayKHoach.Add(objKHoach);
                    }
                    lstDonVayChiPhi = lstDonVayKHoach.Where(f => f.LOAI_KHOACH.Equals("CHI_PHI")).ToList();
                    lstDonVayThuNhap = lstDonVayKHoach.Where(f => f.LOAI_KHOACH.Equals("THU_NHAP")).ToList();
                }

                raddgrDSThanhVien.ItemsSource = lstDonVayChiPhi;
                raddgrDSThuNhap.ItemsSource = lstDonVayThuNhap;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void SetTabThongKiemSoat(DataSet ds)
        {
            try
            {
                if (!LObject.IsNullOrEmpty(ds) && ds.Tables.Count > 1)
                {
                    DataTable dt = ds.Tables["TTIN_CTIET"];
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
                throw ex;
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
            
            lstDonVayKHoach = new List<DON_XIN_VAY_KHOACH_CT>();
            lstDonVayChiPhi = new List<DON_XIN_VAY_KHOACH_CT>();
            lstDonVayThuNhap = new List<DON_XIN_VAY_KHOACH_CT>();
            raddgrDSThanhVien.ItemsSource = lstDonVayChiPhi;
            raddgrDSThuNhap.ItemsSource = lstDonVayThuNhap;
            txtSoGiaoDich.Text = "";
            teldtNgayLapHD.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            lblTrangThai.Content = "";
            txtNguoiLap.Text = "";
            txtNguoiCapNhat.Text = "";
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            teldtNgayCNhat.Value = null;
            txtTrangThai.Text = TThaiNVu = "";
            txtMaKHang.Text = maKhangHang = "";
            lblTrangThaiCapDuyet.Content = "";
            txtMaKHang.Tag = idKhachHang = 0;
            txtTenKHang.Text = "";
            txtSoCMND.Text = "";
            txtThonAp.Text = "";
            txtPhuongXa.Text = "";
            txtQuanHuyen.Text = "";
            telnumVonCanCo.Value = 0;
            telnumVonTuCo.Value = 0;
            telThoiGianVay.Value = 0;
            telSoTienHoanTra.Value = 0;
            cmbSanPham.Items.Clear();
            telThoiGianVay.Value = 0;
            cmbHanMuc.Items.Clear();
            heSo = 0;
            soTienHanMuc = "0";
            cmbLoaiSanPham.SelectedIndex = -1;
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, TThaiNVu,mnuMain,DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON);
        }

        void beforeModify()
        {
            action = DatabaseConstant.Action.SUA;
            SetEnabledAllControls(true);
            OnModify();
        }

        private void SetEnabledAllControls(bool enable)
        {
            grbThongTinChung.IsEnabled = enable;
            btnAddTaiKhoan.IsEnabled = enable;
            btnCommitTaiKhoan.IsEnabled = enable;
            btnCancelTaiKhoan.IsEnabled = enable;
            btnDeleteTaiKhoan.IsEnabled = enable;
            raddgrDSThanhVien.IsReadOnly = !enable;
            raddgrDSThuNhap.IsReadOnly = !enable;
        }

        private void Release()
        {
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
            DatabaseConstant.Table.TD_HDTDVM,
            DatabaseConstant.Action.SUA,
            lstId);
        }

        void OnModify()
        {
            UtilitiesProcess process = new UtilitiesProcess();
            lstId.Add(idHDTDVM);
            bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
            DatabaseConstant.Table.TD_HDTDVM,
            action,
            lstId);
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu,mnuMain,DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON);
        }

        bool Validation()
        {
            bool bReturn = true;
            if (teldtNgayLapHD.Value.IsNullOrEmpty())
            {
                CommonFunction.ThongBaoTrong(lblNgayLapHD.Content.ToString());
                teldtNgayLapHD.Focus();
                bReturn = false;
            }
            else if (txtMaKHang.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblMaKhachHang.Content.ToString());
                txtMaKHang.Focus();
                bReturn = false;
            }
            else if (cmbMucDichVay.SelectedIndex < 0)
            {
                CommonFunction.ThongBaoTrong(lblMucDichVay.Content.ToString());
                cmbMucDichVay.Focus();
                bReturn = false;
            }
            else if (telnumVonCanCo.Value.IsNullOrEmpty())
            {
                CommonFunction.ThongBaoTrong(lblVonPhaiCo.Content.ToString());
                telnumVonCanCo.Focus();
                bReturn = false;
            }
            else if (telnumVonCanCo.Value.IsNullOrEmpty())
            {
                CommonFunction.ThongBaoTrong(lblVonPhaiCo.Content.ToString());
                telnumVonCanCo.Focus();
                bReturn = false;
            }
            //else if (telnumVonCanCo.Value.IsNullOrEmpty())
            //{
            //    CommonFunction.ThongBaoTrong(lblVonPhaiCo.Content.ToString());
            //    telnumVonCanCo.Focus();
            //    bReturn = false;
            //}
            //else if (telnumVonTuCo.Value.IsNullOrEmpty())
            //{
            //    CommonFunction.ThongBaoTrong(lblVonTuCo.Content.ToString());
            //    telnumVonTuCo.Focus();
            //    bReturn = false;
            //}
            else if (cmbLoaiSanPham.SelectedIndex < 0)
            {
                CommonFunction.ThongBaoTrong(lblLoaiSanPham.Content.ToString());
                cmbLoaiSanPham.Focus();
                bReturn = false;
            }
            else if (cmbSanPham.SelectedIndex < 0)
            {
                CommonFunction.ThongBaoTrong(lblSanPhamVay.Content.ToString());
                cmbSanPham.Focus();
                bReturn = false;
            }
            else if (cmbHanMuc.SelectedIndex < 0)
            {
                CommonFunction.ThongBaoTrong(lblHanMuc.Content.ToString());
                cmbHanMuc.Focus();
                bReturn = false;
            }
            return bReturn;
        }
        void BeforeSave(BusinessConstant.TrangThaiNghiepVu trangthai, BusinessConstant.TrangThaiBanGhi banghi)
        {
            if (!trangthai.Equals(BusinessConstant.TrangThaiNghiepVu.LUU_TAM))
            {
                if (!Validation())
                    return;
            }
            GetDataForm(banghi, trangthai);
            OnSave();
        }

        void OnSave()
        {

            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (txtSoGiaoDich.Text == "")
                iret = new TinDungProcess().DonXinVayVon(DatabaseConstant.Action.THEM,ref objHDTDVM, ref lstResponseDetail);
            else
                iret = new TinDungProcess().DonXinVayVon(DatabaseConstant.Action.SUA, ref objHDTDVM, ref lstResponseDetail);
            AfterSave(lstResponseDetail, iret);
        }

        void AfterSave(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            if (iret > 0)
            {
                action = DatabaseConstant.Action.XEM;
                SetDataForm(objHDTDVM.HDTD_VM.MA_HDTDVM);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
                DatabaseConstant.Table.TD_HDTDVM,
                DatabaseConstant.Action.SUA,
                lstId);
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
                if ((bool)cbMultiAdd.IsChecked)
                    ResetForm();
            }
        }

        void AfterDelete(string sSoGiaoDich, List<ClientResponseDetail> ResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(ResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
            DatabaseConstant.Table.TD_HDTDVM,
            DatabaseConstant.Action.XOA,
            lstId);
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            SetDataForm(sSoGiaoDich);
        }

        void OnDelete()
        {
            int iret = 0;
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            iret = new TinDungProcess().DonXinVayVon(DatabaseConstant.Action.XOA, ref objHDTDVM, ref ResponseDetail);
            AfterDelete(txtSoGiaoDich.Text, ResponseDetail);
            CommonFunction.CloseUserControl(this);
        }

        void BeforeDelete()
        {
            if (tlbDelete.IsEnabled == false)
                return;
            try
            {
                if (!txtSoGiaoDich.Text.IsNullOrEmptyOrSpace())
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        List<TD_HDTDVM> lstHDTDVM = new List<TD_HDTDVM>();
                        lstHDTDVM.Add(objHDTDVM.HDTD_VM);
                        objHDTDVM.DSACH_HDTD_VM = lstHDTDVM.ToArray();
                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
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
                DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
                DatabaseConstant.Table.TD_HDTDVM,
                DatabaseConstant.Action.XOA,
                lstId);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void AfterApprove(string sSoGiaoDich, List<ClientResponseDetail> ResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(ResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
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
            iret = new TinDungProcess().DonXinVayVon(DatabaseConstant.Action.DUYET, ref objHDTDVM, ref ResponseDetail);
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
                        List<TD_HDTDVM> lstHDTDVM = new List<TD_HDTDVM>();
                        lstHDTDVM.Add(objHDTDVM.HDTD_VM);
                        objHDTDVM.DSACH_HDTD_VM = lstHDTDVM.ToArray();
                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
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
                DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
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
            DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
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
            iret = new TinDungProcess().DonXinVayVon(DatabaseConstant.Action.TU_CHOI_DUYET, ref objHDTDVM, ref ResponseDetail);
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
                        List<TD_HDTDVM> lstHDTDVM = new List<TD_HDTDVM>();
                        lstHDTDVM.Add(objHDTDVM.HDTD_VM);
                        objHDTDVM.DSACH_HDTD_VM = lstHDTDVM.ToArray();
                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
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
                DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
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
            DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
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
            iret = new TinDungProcess().DonXinVayVon(DatabaseConstant.Action.THOAI_DUYET, ref objHDTDVM, ref ResponseDetail);
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
                        List<TD_HDTDVM> lstHDTDVM = new List<TD_HDTDVM>();
                        lstHDTDVM.Add(objHDTDVM.HDTD_VM);
                        objHDTDVM.DSACH_HDTD_VM = lstHDTDVM.ToArray();
                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
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
                DatabaseConstant.Function.TDVM_DON_XIN_VAY_VON,
                DatabaseConstant.Table.TD_HDTDVM,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void OnPrint()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(objHDTDVM.SO_GIAO_DICH))
            {
                LMessage.ShowMessage("Không có thông tin hợp đồng cần xử lý", LMessage.MessageBoxType.Warning);
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
                        AutoCompleteEntry auHanMuc = lstSourceHanMuc.ElementAt(cmbHanMuc.SelectedIndex);
                        sotienvay = auHanMuc.KeywordStrings[0];
                        mahopdong = txtSoGiaoDich.Text;
                        AutoCompleteEntry auSanPham = lstSourceSanPhamVay.ElementAt(cmbSanPham.SelectedIndex);
                        masanpham = auSanPham.KeywordStrings[0];
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
            if (LObject.IsNullOrEmpty(objHDTDVM.SO_GIAO_DICH))
            {
                LMessage.ShowMessage("Không có thông tin cần xử lý", LMessage.MessageBoxType.Warning);
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
                        AutoCompleteEntry auHanMuc = lstSourceHanMuc.ElementAt(cmbHanMuc.SelectedIndex);
                        sotienvay = auHanMuc.KeywordStrings[0];
                        mahopdong = txtSoGiaoDich.Text;
                        AutoCompleteEntry auSanPham = lstSourceSanPhamVay.ElementAt(cmbSanPham.SelectedIndex);
                        masanpham = auSanPham.KeywordStrings[0];
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
                        AutoCompleteEntry auHanMuc = lstSourceHanMuc.ElementAt(cmbHanMuc.SelectedIndex);
                        sotienvay = auHanMuc.KeywordStrings[0];
                        mahopdong = txtSoGiaoDich.Text;
                        AutoCompleteEntry auSanPham = lstSourceSanPhamVay.ElementAt(cmbSanPham.SelectedIndex);
                        masanpham = auSanPham.KeywordStrings[0];
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
                        AutoCompleteEntry auHanMuc = lstSourceHanMuc.ElementAt(cmbHanMuc.SelectedIndex);
                        sotienvay = auHanMuc.KeywordStrings[0];
                        mahopdong = txtSoGiaoDich.Text;
                        AutoCompleteEntry auSanPham = lstSourceSanPhamVay.ElementAt(cmbSanPham.SelectedIndex);
                        masanpham = auSanPham.KeywordStrings[0];
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
                else if (ClientInformation.Company.Equals("QUANGBINH"))
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
                        AutoCompleteEntry auHanMuc = lstSourceHanMuc.ElementAt(cmbHanMuc.SelectedIndex);
                        sotienvay = auHanMuc.KeywordStrings[0];
                        mahopdong = txtSoGiaoDich.Text;
                        AutoCompleteEntry auSanPham = lstSourceSanPhamVay.ElementAt(cmbSanPham.SelectedIndex);
                        masanpham = auSanPham.KeywordStrings[0];
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
                    lstThamSo.Add(new ThamSoBaoCao("@MA_DON_V", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

                    lstThamSo.Add(new ThamSoBaoCao("P_TEN_KHACH_HANG", tenkhachhang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_TEN_THUONG_GOI", tenkhachhang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_GIOI_TINH", gioitinh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NGAY_SINH", ngaysinh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_DIA_CHI", diachi, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_SO_CMND", socmnd, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NGAY_CAP", ngaycap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NOI_CAP", noicap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoQuangBinh(DatabaseConstant.DanhSachBaoCaoQuangBinh.TDVM_DON_XIN_VAY_VON_TRA_DAN_VA_BO_SUNG);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
                else
                {
                }
            }
        }

        private void OnPreviewDonVayVonKhac()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(objHDTDVM.SO_GIAO_DICH))
            {
                LMessage.ShowMessage("Không có thông tin cần xử lý", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                if (ClientInformation.Company.Equals("QUANGBINH"))
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
                        AutoCompleteEntry auHanMuc = lstSourceHanMuc.ElementAt(cmbHanMuc.SelectedIndex);
                        sotienvay = auHanMuc.KeywordStrings[0];
                        mahopdong = txtSoGiaoDich.Text;
                        AutoCompleteEntry auSanPham = lstSourceSanPhamVay.ElementAt(cmbSanPham.SelectedIndex);
                        masanpham = auSanPham.KeywordStrings[0];
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
                    lstThamSo.Add(new ThamSoBaoCao("@MA_DON_V", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

                    lstThamSo.Add(new ThamSoBaoCao("P_TEN_KHACH_HANG", tenkhachhang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_TEN_THUONG_GOI", tenkhachhang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_GIOI_TINH", gioitinh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NGAY_SINH", ngaysinh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_DIA_CHI", diachi, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_SO_CMND", socmnd, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NGAY_CAP", ngaycap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NOI_CAP", noicap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoQuangBinh(DatabaseConstant.DanhSachBaoCaoQuangBinh.TDVM_DON_XIN_VAY_VON_VAY_KHAC);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
                else
                {
                }
            }
        }

        private void OnPreviewLichTraDan()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(objHDTDVM.SO_GIAO_DICH))
            {
                LMessage.ShowMessage("Không có thông tin cần xử lý", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                if (ClientInformation.Company.Equals("PHUTHO"))
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
                        AutoCompleteEntry auHanMuc = lstSourceHanMuc.ElementAt(cmbHanMuc.SelectedIndex);
                        sotienvay = auHanMuc.KeywordStrings[0];
                        mahopdong = txtSoGiaoDich.Text;
                        AutoCompleteEntry auSanPham = lstSourceSanPhamVay.ElementAt(cmbSanPham.SelectedIndex);
                        masanpham = auSanPham.KeywordStrings[0];
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
                    lstThamSo.Add(new ThamSoBaoCao("@MA_DON_V", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

                    lstThamSo.Add(new ThamSoBaoCao("P_TEN_KHACH_HANG", tenkhachhang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_TEN_THUONG_GOI", tenkhachhang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_GIOI_TINH", gioitinh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NGAY_SINH", ngaysinh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_DIA_CHI", diachi, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_SO_CMND", socmnd, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NGAY_CAP", ngaycap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NOI_CAP", noicap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoPhuTho(DatabaseConstant.DanhSachBaoCaoPhuTho.TDVM_LICH_TRA_NO);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
                else
                {
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
            if (raddgrDSThanhVien.SelectedItems.Count <= 0)
            {
                return listDataRow;
            }
            else
            {
                for (int i = 0; i < raddgrDSThanhVien.SelectedItems.Count; i++)
                {
                    DataRow dr = (DataRow)raddgrDSThanhVien.SelectedItems[i];
                    listDataRow.Add(dr);
                }
                return listDataRow;
            }
        }
        #endregion
    }
}
