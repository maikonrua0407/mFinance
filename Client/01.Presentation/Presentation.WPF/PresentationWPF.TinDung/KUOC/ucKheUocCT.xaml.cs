using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
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
using PresentationWPF.CustomControl;
using Presentation.Process.TinDungServiceRef;
using Presentation.Process.Common;
using Presentation.Process;
using Telerik.Windows.Controls;
using Presentation.Process.PopupServiceRef;
using System.Reflection;
using Telerik.Windows.Controls.GridView;
using PresentationWPF.BaoCao.DungChung;
using System.Collections;


namespace PresentationWPF.TinDung.KUOC
{
    /// <summary>
    /// Interaction logic for ucKheUocCT.xaml
    /// </summary>
    public partial class ucKheUocCT : UserControl
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
        private TDVM_KHE_UOC _TDVMKHEUOC = new TDVM_KHE_UOC();
        private TDVM_HDTD TDVMHDTD = new TDVM_HDTD();
        private List<AutoCompleteEntry> lstThoiHanVay = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstMucDichVayVon = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstLoaiNguonVon = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstDinhKyDanhGiaLS = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstHinhThucTraGoc = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstHinhThucTraLai = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstNguyenNhanTDoi = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstLoaiLSuat = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstHinhThucLapLich = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstDonViTinhLSuat = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstMaLoaiTien = new List<AutoCompleteEntry>();
        List<DataRow> lstPopup = new List<DataRow>();
        public event EventHandler OnSavingCompleted;
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        string TThaiNVu = "";
        decimal dLaiSuat = 0;
        int iDKheUoc = 0;
        List<TD_KHOACHVM_CT> lstKeHoach = new List<TD_KHOACHVM_CT>();
        List<TD_KHOACHVM> lstKeHoachDKien = new List<TD_KHOACHVM>();
        public DatabaseConstant.Action action;
        List<DANH_SACH_TSDB> lstTSDBVM = new List<DANH_SACH_TSDB>();
        string TThaiKUOC = BusinessConstant.TRANG_THAI_TAT_TOAN.CHUA_GIAI_NGAN.layGiaTri();
        public TDVM_KHE_UOC TDVMKHEUOC
        {
            get { return _TDVMKHEUOC; }
            set { _TDVMKHEUOC = value; }
        }
        private List<AutoCompleteEntry> lstNgayDauKy = new List<AutoCompleteEntry>();
        decimal sSoTienGiaiNgan = 0;
        string ngayGiaiNgan = "";
        string sKiemTraLSuat = "CO";
        string sSoNgayBDau = "";
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucKheUocCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDung;component/KUOC/ucKheUocCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            KhoiTaoGiaTriChoComboBox();
            InitEventHandler();
            ShowControl();
            ClearForm();
            //MA 20130322
            System.Linq.Expressions.Expression<Func<TD_KHOACHVM_CT, decimal?>> expressionKH = kh => (kh.KH_TRA_GOC.IsNullOrEmpty() && kh.KH_TRA_LAI.IsNullOrEmpty() && kh.KH_TRA_PHI.IsNullOrEmpty() ? null : (decimal?)kh.KH_TRA_GOC.GetValueOrDefault(0) + kh.KH_TRA_LAI.GetValueOrDefault(0) + kh.KH_TRA_PHI.GetValueOrDefault(0));
            GridViewExpressionColumn column = this.raddgrLichTraNo.Columns["Cong"] as GridViewExpressionColumn;
            column.Expression = expressionKH;
            GridViewExpressionColumn columnKH = this.raddgrTinhTrangTraNo.Columns["CongKH"] as GridViewExpressionColumn;
            columnKH.Expression = expressionKH;
            System.Linq.Expressions.Expression<Func<TD_KHOACHVM_CT, decimal?>> expressionTT = kh => (kh.TT_TRA_GOC.IsNullOrEmpty() && kh.TT_TRA_LAI.IsNullOrEmpty() && kh.TT_TRA_PHI.IsNullOrEmpty() ? null : (decimal?)kh.TT_TRA_GOC.GetValueOrDefault(0) + kh.TT_TRA_LAI.GetValueOrDefault(0) + kh.TT_TRA_PHI.GetValueOrDefault(0));
            GridViewExpressionColumn columnTT = this.raddgrTinhTrangTraNo.Columns["CongTT"] as GridViewExpressionColumn;
            columnTT.Expression = expressionTT;
            // Lần đầu không cho chọn nguyên nhân thay đổi
            cmbNguyenNhanThayDoi.IsEnabled = false;
        }
        void KhoiTaoGiaTriChoComboBox()
        {
            List<string> lstDieuKien = new List<string>();
            List<string> lstMaChon = new List<string>();
            string sMaTruyVan = "";
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DVI_TINH_KY_HAN.getValue());
            sMaTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            AutoComboBox auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstThoiHanVay, ref cmbThoiHanVay, sMaTruyVan, lstDieuKien, "THANG", null);
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.MUC_DICH_VAY_VON.getValue());
            auto.GenAutoComboBox(ref lstMucDichVayVon, ref cmbMaMucDichVay, sMaTruyVan, lstDieuKien);
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DVI_TINH_KY_HAN.getValue());
            auto.GenAutoComboBox(ref lstDinhKyDanhGiaLS, ref cmbDinhKyDanhGia, sMaTruyVan, lstDieuKien,"THANG");
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.HINH_THUC_TRA_GOC.getValue());
            lstMaChon.Add(BusinessConstant.HINH_THUC_TRA_GOC.DINH_KY.layGiaTri());
            lstMaChon.Add(BusinessConstant.HINH_THUC_TRA_GOC.TRA_SAU.layGiaTri());
            auto.GenAutoComboBox(ref lstHinhThucTraGoc, ref cmbDinhKyTraGoc, sMaTruyVan, lstDieuKien, BusinessConstant.HINH_THUC_TRA_GOC.DINH_KY.layGiaTri(), lstMaChon);
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.HINH_THUC_TRA_LAI.getValue());
            auto.GenAutoComboBox(ref lstHinhThucTraLai, ref cmbDinhKyTraLai, sMaTruyVan, lstDieuKien, BusinessConstant.HINH_THUC_TRA_GOC.DINH_KY.layGiaTri());
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGUYEN_NHAN_THAY_DOI_LTN.getValue());
            lstNguyenNhanTDoi.Add(new AutoCompleteEntry("", "", ""));
            auto.GenAutoComboBox(ref lstNguyenNhanTDoi, ref cmbNguyenNhanThayDoi, sMaTruyVan, lstDieuKien, "");
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.LOAI_LAI_SUAT.getValue());
            auto.GenAutoComboBox(ref lstLoaiLSuat, ref cmbLoaiLSuat, sMaTruyVan, lstDieuKien);
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.TAN_SUAT.getValue());
            auto.GenAutoComboBox(ref lstDonViTinhLSuat, ref cmbLSuatDViTinh, sMaTruyVan, lstDieuKien);
            sMaTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_TIENTE.getValue();
            auto.GenAutoComboBox(ref lstMaLoaiTien, ref cmbLoaiTienGN, sMaTruyVan, null, ClientInformation.MaDongNoiTe);
            sMaTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_NGUON_VON.getValue();
            lstDieuKien = new List<string>();
            auto.GenAutoComboBox(ref lstLoaiNguonVon, ref cmbLoaiNguonVon, sMaTruyVan, lstDieuKien);
        }
        void InitEventHandler()
        {
            txtLSBienDo.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(txtLSBienDo_ValueChanged);
            ucColNgayBDau.EditCellEnd += new EventHandler(ucColNgayBDau_EditCellEnd);
            ucColHinhThuc.EditCellEnd += new EventHandler(ucColHinhThuc_EditCellEnd);
            ucColLoaiHinh.EditCellEnd += new EventHandler(ucColLoaiHinh_EditCellEnd);
            teldtNgayPhatVon.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(teldtNgayPhatVon_ValueChanged);
            dtNgayBatDauTraNo.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(dtNgayBatDauTraNo_ValueChanged);
        }

        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.TinDung.KUOC.ucKheUocCT", "RibbonButton");
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
                        KeyGesture keyg = new KeyGesture(Key.I, ModifierKeys.Control);
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
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control|ModifierKeys.Shift);
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
                        KeyGesture keyg = new KeyGesture(Key.F, ModifierKeys.Control | ModifierKeys.Shift);
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
                ClearForm();
                SetEnabledAllControl(true);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                Modify();
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
            else if (strTinhNang.Equals("PreviewKheUoc"))
            {
                OnPreviewKheUoc();
            }
            else if (strTinhNang.Equals("PreviewNhanNo"))
            {
                OnPreviewKheUocNhanNo();
            }
            else if (strTinhNang.Equals("PreviewPhanKy"))
            {
                OnPreviewKheUocPhanKy();
            }
            else if (strTinhNang.Equals("PreviewBaoHiem"))
            {
                OnPreviewBaoHiem();
            }
            else if (strTinhNang.Equals("PreviewDanhGiaKH"))
            {
                //OnPreviewDanhGiaKH();
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
            // Truongnx
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAP_DU_LIEU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ClearForm();
                SetEnabledAllControl(true);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                Modify();
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
            else if (strTinhNang.Equals("PreviewKheUoc"))
            {
                OnPreviewKheUoc();
            }
            else if (strTinhNang.Equals("PreviewNhanNo"))
            {
                OnPreviewKheUocNhanNo();
            }
            else if (strTinhNang.Equals("PreviewPhanKy"))
            {
                OnPreviewKheUocPhanKy();
            }
            else if (strTinhNang.Equals("PreviewBaoHiem"))
            {
                OnPreviewBaoHiem();
            }
            else if (strTinhNang.Equals("PreviewDanhGiaKH"))
            {
                //OnPreviewDanhGiaKH();
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
                {
                    telControl.Value = dtpControl.SelectedDate;
                    telControl.Focus();
                }
                else
                    throw new System.NullReferenceException(LLanguage.SearchResourceByKey("M.TinDung.KheUoc.ucKheUocCT.KhongTimThayControl") + sbControl.ToString());
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void btnMaHDTD_Click(object sender, RoutedEventArgs e)
        {
            lstPopup.Clear();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
            lstDieuKien.Add("HDTDLAPKU");
            lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation("POPUP_DS_HDTD", lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            ucPopup popup = new ucPopup(true, simplePopupResponse);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
            win.Content = popup;
            win.ShowDialog();
            if (lstPopup.Count > 0)
            {
                TDVMHDTD.HDTD_VM.ID = Convert.ToInt32(lstPopup[0]["ID"]);
                txtSoHDTD.Text = TDVMHDTD.HDTD_VM.MA_HDTDVM = lstPopup[0]["MA_HDTDVM"].ToString();
                txtTenKhachHang.Text = lstPopup[0]["TEN_KHANG"].ToString();
                TDVMHDTD.HDTD_VM.MA_KHANG = lstPopup[0]["MA_KHANG"].ToString();
                TDVMHDTD.HDTD_VM.ID_KHANG = Convert.ToInt32(lstPopup[0]["ID_KHANG"]);
                TDVMHDTD.HDTD_VM.LSUAT_QHAN = Convert.ToDecimal(lstPopup[0]["LSUAT_QHAN"]);
                LayDanhSachNgayDauKy();
            }
        }

        private void btnMaSanPham_Click(object sender, RoutedEventArgs e)
        {
            lstPopup.Clear();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.MaDonVi);
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation("POPUP_DS_SANPHAM_TD", lstDieuKien);
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
                txtMaSanPham.Text = lstPopup[0]["MA_SAN_PHAM"].ToString();
                txtMaSanPham.Tag = Convert.ToInt32(lstPopup[0]["ID"]);
                txtMaLaiSuat.Text = lstPopup[0]["MA_LSUAT"].ToString();
            }
            LayChiTietSoTienGiaiNgan();
            LayDanhSachNgayDauKy();
            LayChiTietLaiSuat();
        }

        private void btnHopDong_Click(object sender, RoutedEventArgs e)
        {
            //lstPopup.Clear();
            //List<string> lstDieuKien = new List<string>();
            //lstDieuKien.Add("NULL");
            //PopupProcess popupProcess = new PopupProcess();
            //popupProcess.getPopupInformation("POPUP_DS_SANPHAM_TD", lstDieuKien);
            //SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            //ucPopup popup = new ucPopup(false, simplePopupResponse);
            //popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            //Window win = new Window();
            //win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
            //win.Content = popup;
            //win.ShowDialog();
            //if (lstPopup.Count > 0)
            //{
            //    txtMaSanPham.Text = lstPopup[0]["MA_SAN_PHAM"].ToString();
            //    txtMaSanPham.Tag = Convert.ToInt32(lstPopup[0]["ID"]);
            //    LayDanhSachNgayDauKy();
            //}
        }

        private void btnMaLaiSuat_Click(object sender, RoutedEventArgs e)
        {
            lstPopup.Clear();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add("TDVM");
            lstDieuKien.Add(ClientInformation.MaDonVi);
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation("POPUP_DS_LAISUAT", lstDieuKien);
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
                txtMaLaiSuat.Text = lstPopup[0][2].ToString();
                txtMaLaiSuat.Tag = lstPopup[0][1];
                lblLSuat.Content = lstPopup[0][3];
                LayChiTietLaiSuat();
            }
        }
        
        private void btnThemHDTC_Click(object sender, RoutedEventArgs e)
        {
            lstPopup.Clear();
            string lstTSDBID = "";
            foreach (DANH_SACH_TSDB tsdb in lstTSDBVM)
            {
                lstTSDBID += "," + tsdb.ID_TSDB.ToString();
            }
            if (lstTSDBID.Length > 0)
                lstTSDBID = lstTSDBID.Substring(1);
            else
                lstTSDBID = "0";
            List<string> lstDieuKien = new List<string>();
            string sMAKHANG = TDVMHDTD.HDTD_VM.ID_KHANG.IsNullOrEmpty() ? "0" : TDVMHDTD.HDTD_VM.ID_KHANG.ToString();
            lstDieuKien.Add(sMAKHANG);
            lstDieuKien.Add(lstTSDBID);
            lstPopup = new List<DataRow>();
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation("POPUP_DS_TSDB_KHEUOC", lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            ucPopup popup = new ucPopup(false, simplePopupResponse,true);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
            win.Content = popup;
            win.ShowDialog();
            
            if (lstPopup.Count > 0)
            {
                foreach (DataRow dr in lstPopup)
                {
                    DANH_SACH_TSDB objTSDB = new DANH_SACH_TSDB();
                    foreach (DataColumn col in dr.Table.Columns)
                    {
                        PropertyInfo property = objTSDB.GetType().GetProperty(col.ColumnName);
                        if (property != null)
                        {
                            if (property.PropertyType.Equals(typeof(int)))
                                property.SetValue(objTSDB, Convert.ToInt32(dr[col.ColumnName]), null);
                            else if (property.PropertyType.Equals(typeof(decimal)))
                                property.SetValue(objTSDB, Convert.ToDecimal(dr[col.ColumnName]), null);
                            else
                                property.SetValue(objTSDB, dr[col.ColumnName], null);
                        }
                    }
                    lstTSDBVM.Add(objTSDB);
                }
                raddgrTSDB.ItemsSource = null;
                raddgrTSDB.ItemsSource = lstTSDBVM;
            }
        }

        private void btnXoaHDTC_Click(object sender, RoutedEventArgs e)
        {
            if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            foreach (DANH_SACH_TSDB objTSDB in raddgrTSDB.SelectedItems)
            {
                lstTSDBVM.Remove(objTSDB);
            }
            raddgrTSDB.ItemsSource = null;
            raddgrTSDB.ItemsSource = lstTSDBVM;
        }

        void teldtNgayPhatVon_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            dtNgayBatDauTraNo.Value = teldtNgayPhatVon.Value.GetValueOrDefault().PlusDays(sSoNgayBDau.StringToInt32());
            dtNgayBatDauTraNoCum.Value = teldtNgayPhatVon.Value.GetValueOrDefault().PlusDays(sSoNgayBDau.StringToInt32());
        }

        void dtNgayBatDauTraNo_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            LayDanhSachNgayDauKy();
        }

        void ClearForm()
        {
            
            string sNgayTraGoc = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_SONGAY_TOITHIEU_TRAGOC,ClientInformation.MaDonVi);
            string sNgayTraLai = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_SONGAY_TOITHIEU_TRALAI, ClientInformation.MaDonVi);
            if (sNgayTraGoc.StringToInt32().CompareTo(sNgayTraLai.StringToInt32()) >= 0)
                sSoNgayBDau = sNgayTraGoc;
            else
                sSoNgayBDau = sNgayTraLai;
            sKiemTraLSuat = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_KTRA_LAI_SUAT_KUOC, ClientInformation.MaDonVi);
            if (!LObject.IsNullOrEmpty(sKiemTraLSuat) && sKiemTraLSuat.Equals(BusinessConstant.CoKhong.CO.layGiaTri()))
                lblEniLSuat.Visibility = Visibility.Visible;
            else
                lblEniLSuat.Visibility = Visibility.Collapsed;
            TDVMKHEUOC.KUOC_VM = new Presentation.Process.TinDungServiceRef.TD_KUOCVM();
            TDVMHDTD.HDTD_VM = new TD_HDTDVM();
            lstKeHoach = new List<TD_KHOACHVM_CT>();
            lstKeHoachDKien = new List<TD_KHOACHVM>();
            lstTSDBVM = new List<DANH_SACH_TSDB>();
            raddgrLichTraNo.ItemsSource = null;
            raddgrThongTinLapLich.ItemsSource = null;
            raddgrTinhTrangTraNo.ItemsSource = null;
            raddgrTSDB.ItemsSource = null;
            raddgrLichTraNo.ItemsSource = lstKeHoach;
            raddgrThongTinLapLich.ItemsSource = lstKeHoachDKien;
            raddgrTinhTrangTraNo.ItemsSource = lstKeHoach;
            raddgrTSDB.ItemsSource = lstTSDBVM;
            txtSoKheUoc.Text = "";
            txtSoHDTD.Text = "";
            teldtNgayLapKU.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            dtNgayBatDauTraNo.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat).PlusDays(sSoNgayBDau.StringToInt32());
            dtNgayBatDauTraNoCum.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat).PlusDays(sSoNgayBDau.StringToInt32());
            txtTenKhachHang.Text = "";
            txtMaLaiSuat.Text = "";
            txtMaSanPham.Text = "";
            txtMaSanPham.Tag = null;
            iDKheUoc = 0;
            txtHeSo.Value = 1;
            txtSoTienVay.Value = 0;
            txtThoiHanVay.Value = 0;
            teldtNgayPhatVon.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            txtHopDong.Text = "";
            txtBenChoVay.Text = "";
            teldtNgayVay.Value = null;
            txtThoiHan.Text = "";
            telnumSoTienGiaiNgan.Value = null;
            telnumSoTienVay.Value = null;
            txtMaLaiSuat.Text = "";
            lblLSuat.Content = LLanguage.SearchResourceByKey("U.TinDung.ucKheUocCT.TenLaiSuat");
            txtLSBienDo.Value = 0;
            txtDinhKyDanhGia.Value = null;
            txtLSuat.Value = null;
            TThaiNVu = "";
            txtNguoiCapNhat.Text = "";
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            txtTrangThai.Text = "";
            lblTrangThai.Content = "";
            teldtNgayCNhat.Value = null;
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            action = DatabaseConstant.Action.THEM;
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC);
            RefeshButton();
        }

        private void cmbLoaiLSuat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lstLoaiLSuat.ElementAt(cmbLoaiLSuat.SelectedIndex).KeywordStrings.First().Equals(BusinessConstant.LOAI_LAI_SUAT.CO_DINH.layGiaTri()))
            {
                txtDinhKyDanhGia.IsEnabled = false;
                cmbDinhKyDanhGia.IsEnabled = false;
            }
            else
            {
                txtDinhKyDanhGia.IsEnabled = true;
                cmbDinhKyDanhGia.IsEnabled = true;
            }
        }

        private void raddgrTSDB_CellValidating(object sender, GridViewCellValidatingEventArgs e)
        {
            if (e.Cell.Column.UniqueName.Equals("GTRI_DBAO_DTUONG"))
            {
                DANH_SACH_TSDB dstsdb = e.Row.Item as DANH_SACH_TSDB;
                if (dstsdb.GTRI_DBAO_DTUONG > dstsdb.GTRI_CON_DBAO)
                {
                    e.IsValid = false;
                    e.ErrorMessage = "Giá trị không phù hợp";
                }
            }
        }

        private void raddgrTSDB_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            DANH_SACH_TSDB objDSTSDB = e.Cell.ParentRow.Item as DANH_SACH_TSDB;
            if (e.Cell.Column.UniqueName.Equals("GTRI_TY_LE"))
                objDSTSDB.GTRI_DBAO_DTUONG = objDSTSDB.GTRI_CON_DBAO * objDSTSDB.GTRI_TY_LE / 100;
            else if (e.Cell.Column.UniqueName.Equals("GTRI_DBAO_DTUONG"))
                objDSTSDB.GTRI_TY_LE = objDSTSDB.GTRI_DBAO_DTUONG / objDSTSDB.GTRI_CON_DBAO * 100;
            lstTSDBVM[lstTSDBVM.IndexOf(objDSTSDB)] = objDSTSDB;
        }

        private void txtMaLaiSuat_LostFocus(object sender, RoutedEventArgs e)
        {
            LayChiTietLaiSuat();
        }

        private void txtMaLaiSuat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
                btnMaLaiSuat_Click(sender, null);
        }

        private void txtSoHDTD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
                btnMaHDTD_Click(sender, null);
        }

        private void txtMaSanPham_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
                btnMaSanPham_Click(sender, null);
        }

        private void SetEnabledAllControl(bool bBool)
        {
            if (TThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()) && action.Equals(DatabaseConstant.Action.SUA))
            {
                raddgrThongTinLapLich.IsReadOnly = false;
                tlbLapKeHoach.IsEnabled = true;
                raddgrTSDB.IsReadOnly = false;
                btnThemHDTC.IsEnabled = true;
                btnXoaHDTC.IsEnabled = true;
                cmbNguyenNhanThayDoi.IsEnabled = true;
            }
            else
            {
                grbThongTinChung.IsEnabled = bBool;
                grbNguonVonGiaiNgan.IsEnabled = bBool;
                grbLaiSuat.IsEnabled = bBool;
                grbHinhThuc.IsEnabled = bBool;
                raddgrThongTinLapLich.IsReadOnly = !bBool;
                raddgrLichTraNo.IsReadOnly = !bBool;
                cmbNguyenNhanThayDoi.IsEnabled = bBool;
                tlbLapKeHoach.IsEnabled = bBool;
                raddgrTSDB.IsReadOnly = !bBool;
                btnThemHDTC.IsEnabled = bBool;
                btnXoaHDTC.IsEnabled = bBool;
            }
        }

        void txtLSBienDo_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (txtLSBienDo.Value.IsNullOrEmpty())
                txtLSBienDo.Value = 0;
            txtLSuat.Value = (double)dLaiSuat + txtLSBienDo.Value.Value;
        }

        private void raddgrThongTinLapLich_BeginningEdit(object sender, GridViewBeginningEditRoutedEventArgs e)
        {
            if (!e.Cell.Column.UniqueName.IsNullOrEmptyOrSpace())
            {
                if (e.Cell.Column.UniqueName.Equals("NGAY_BDAU"))
                    ucColNgayBDau.GiaTri = LDateTime.DateToString(Convert.ToDateTime(e.Cell.Value),ApplicationConstant.defaultDateTimeFormat);
                    ucColNgayBDau.lstComboBox = lstNgayDauKy;
            }
        }

        private void tlbAddLich_Click(object sender, RoutedEventArgs e)
        {
            TD_KHOACHVM_CT objKHoachCT = new TD_KHOACHVM_CT();
            objKHoachCT.KY_THU = lstKeHoach.Count + 1;
            lstKeHoach.Add(objKHoachCT);
            raddgrLichTraNo.ItemsSource = null;
            raddgrLichTraNo.ItemsSource = lstKeHoach;
        }

        private void tlbDeleteLich_Click(object sender, RoutedEventArgs e)
        {
            TD_KHOACHVM_CT objKHoachCT = raddgrLichTraNo.SelectedItem as TD_KHOACHVM_CT;
            lstKeHoach.Remove(objKHoachCT);
            raddgrLichTraNo.ItemsSource = null;
            raddgrLichTraNo.ItemsSource = lstKeHoach;
        }

        private void tlbDelTTin_Click(object sender, RoutedEventArgs e)
        {
            TD_KHOACHVM objKHoach = raddgrThongTinLapLich.SelectedItem as TD_KHOACHVM;
            lstKeHoachDKien.Remove(objKHoach);
            raddgrThongTinLapLich.ItemsSource = null;
            raddgrThongTinLapLich.ItemsSource = lstKeHoachDKien;
        }

        private void tlbAddTTin_Click(object sender, RoutedEventArgs e)
        {
            if (LObject.IsNullOrEmpty(lstNgayDauKy) || lstNgayDauKy.Count < 1)
                LayDanhSachNgayDauKy();
            if (lstNgayDauKy.Count > 0)
            {
                TD_KHOACHVM objKHoachVM = new TD_KHOACHVM();
                objKHoachVM.LOAI_HINH_LAP_KH = "";
                objKHoachVM.MA_NNHAN_TDOI = lstNguyenNhanTDoi.ElementAt(cmbNguyenNhanThayDoi.SelectedIndex).KeywordStrings.First();
                objKHoachVM.MA_HTHUC = "";
                objKHoachVM.NGAY_BDAU = LDateTime.DateToString(teldtNgayPhatVon.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
                lstKeHoachDKien.Add(objKHoachVM);
            }
            raddgrThongTinLapLich.ItemsSource = null;
            raddgrThongTinLapLich.ItemsSource = lstKeHoachDKien;
        }

        void ucColLoaiHinh_EditCellEnd(object sender, EventArgs e)
        {
            Telerik.Windows.Controls.GridView.GridViewCell cellEdit = ucColLoaiHinh.cellEdit;
            TD_KHOACHVM objKHoachVM = cellEdit.ParentRow.Item as TD_KHOACHVM;
            string GiaTri = ucColLoaiHinh.GiaTri;
            lstKeHoachDKien.ElementAt(lstKeHoachDKien.IndexOf(objKHoachVM)).LOAI_HINH_LAP_KH = GiaTri;
            //lstKeHoachDKien.RemoveAll(d => LDateTime.CountDayBetweenDates(d.NGAY_BDAU, objKHoachVM.NGAY_BDAU, ApplicationConstant.defaultDateTimeFormat) > 0 && d.LOAI_HINH_LAP_KH == objKHoachVM.LOAI_HINH_LAP_KH);
        }

        void ucColHinhThuc_EditCellEnd(object sender, EventArgs e)
        {
            Telerik.Windows.Controls.GridView.GridViewCell cellEdit = ucColHinhThuc.cellEdit;
            TD_KHOACHVM objKHoachVM = cellEdit.ParentRow.Item as TD_KHOACHVM;
            string GiaTri = ucColHinhThuc.GiaTri;
            lstKeHoachDKien.ElementAt(lstKeHoachDKien.IndexOf(objKHoachVM)).MA_HTHUC = GiaTri;
            //lstKeHoachDKien.RemoveAll(d => LDateTime.CountDayBetweenDates(d.NGAY_BDAU, objKHoachVM.NGAY_BDAU, ApplicationConstant.defaultDateTimeFormat) > 0 && d.LOAI_HINH_LAP_KH == objKHoachVM.LOAI_HINH_LAP_KH);
        }

        void ucColNgayBDau_EditCellEnd(object sender, EventArgs e)
        {
            Telerik.Windows.Controls.GridView.GridViewCell cellEdit = ucColNgayBDau.cellEdit;
            TD_KHOACHVM objKHoachVM = cellEdit.ParentRow.Item as TD_KHOACHVM;
            string GiaTri = ucColNgayBDau.GiaTri;
            lstKeHoachDKien.ElementAt(lstKeHoachDKien.IndexOf(objKHoachVM)).NGAY_BDAU = GiaTri;
            //lstKeHoachDKien.RemoveAll(d => LDateTime.CountDayBetweenDates(d.NGAY_BDAU, objKHoachVM.NGAY_BDAU, ApplicationConstant.defaultDateTimeFormat) > 0 && d.LOAI_HINH_LAP_KH == objKHoachVM.LOAI_HINH_LAP_KH);
        }

        private void teldtNgayPhatVon_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ngayGiaiNgan.Equals(LDateTime.DateToString(teldtNgayPhatVon.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat)))
                return;
            LayDanhSachNgayDauKy();
        }
        //private void txtThoiHanVay_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        //{
        //    LayDanhSachNgayDauKy();
        //}

        private void cmbThoiHanVay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LayDanhSachNgayDauKy();
        }

        private void txtThoiHanVay_LostFocus(object sender, RoutedEventArgs e)
        {
            LayDanhSachNgayDauKy();
        }

        private void cmbDinhKyTraGoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InitDieuKienLapLich();
        }

        private void cmbDinhKyTraLai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InitDieuKienLapLich();
        }

        // Đang thực hiện.
        private void raddgrThongTinLapLich_CellValidating(object sender, GridViewCellValidatingEventArgs e)
        {
            TD_KHOACHVM objKHoach = e.Cell.ParentRow.Item as TD_KHOACHVM;
            string sHinhThucTraGoc = lstHinhThucTraGoc.ElementAt(cmbDinhKyTraGoc.SelectedIndex).KeywordStrings.First();
            string sHinhThucTraLai = lstHinhThucTraLai.ElementAt(cmbDinhKyTraLai.SelectedIndex).KeywordStrings.First();
            if (e.Cell.Column.UniqueName == "LOAI_HINH_LAP_KE_HOACH")
            {
                
            }
            else if (e.Cell.Column.UniqueName == "HINH_THUC_THANH_TOAN")
            {

            }
            else if (e.Cell.Column.UniqueName == "NGAY_BDAU")
            {

            }
            else if (e.Cell.Column.UniqueName == "SO_KY")
            {

            }
            else if (e.Cell.Column.UniqueName == "TAN_SUAT")
            {

            }
        }

        private void cmbNguyenNhanThayDoi_LostFocus(object sender, RoutedEventArgs e)
        {
            //if(cmbNguyenNhanThayDoi.SelectedIndex>=0)
            //    {
            //        string GiaTriNguyenNhan = lstNguyenNhanTDoi.ElementAt(cmbNguyenNhanThayDoi.SelectedIndex).KeywordStrings.FirstOrDefault();
            //        if (GiaTriNguyenNhan.Equals(BusinessConstant.NGUYEN_NHAN_THAY_DOI_LTN.CO_CAU_KY_HAN.layGiaTri()))
            //        {
            //            lblMaPhi.Visibility = Visibility.Visible;
            //            txtMaPhi.Visibility = Visibility.Visible;
            //            btnMaPhi.Visibility = Visibility.Visible;
            //        }
            //        else
            //        {
            //            lblMaPhi.Visibility = Visibility.Collapsed;
            //            txtMaPhi.Visibility = Visibility.Collapsed;
            //            btnMaPhi.Visibility = Visibility.Collapsed;
            //        }
            //    }
        }

        private void txtHeSo_LostFocus(object sender, RoutedEventArgs e)
        {
            txtSoTienVay.Value = (double)sSoTienGiaiNgan * txtHeSo.Value.GetValueOrDefault(1);
        }

        private void RefeshButton()
        {
            if (action.Equals(DatabaseConstant.Action.XEM) && TThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                tlbRefuse.IsEnabled = true;
            if (action.Equals(DatabaseConstant.Action.SUA) && TThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri()))
            {
                tlbHold.IsEnabled = true;
                tlbSubmit.IsEnabled = true;
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
                TDVMKHEUOC.KUOC_VM.ID = iDKheUoc;
                TDVMKHEUOC.KUOC_VM.ID_HDTDVM = TDVMHDTD.HDTD_VM.ID;
                TDVMKHEUOC.ID_KHANG = TDVMHDTD.HDTD_VM.ID_KHANG;
                TDVMKHEUOC.MA_KHANG = TDVMHDTD.HDTD_VM.MA_KHANG;
                TDVMKHEUOC.KUOC_VM.ID_KHANG = TDVMHDTD.HDTD_VM.ID_KHANG;
                TDVMKHEUOC.KUOC_VM.MA_KHANG = TDVMHDTD.HDTD_VM.MA_KHANG;
                TDVMKHEUOC.KUOC_VM.ID_DIABAN = TDVMHDTD.HDTD_VM.ID_DIABAN;
                TDVMKHEUOC.KUOC_VM.ID_NGUOI_QLY = TDVMHDTD.HDTD_VM.ID_NGUOI_QLY;
                TDVMKHEUOC.KUOC_VM.MA_NGUOI_QLY = TDVMHDTD.HDTD_VM.MA_NGUOI_QLY;
                TDVMKHEUOC.KUOC_VM.MA_KUOCVM = txtSoKheUoc.Text;
                TDVMKHEUOC.KUOC_VM.MA_HDTDVM = txtSoHDTD.Text;
                TDVMKHEUOC.KUOC_VM.MA_SAN_PHAM = txtMaSanPham.Text;
                TDVMKHEUOC.KUOC_VM.MA_LSUAT = txtMaLaiSuat.Text;
                TDVMKHEUOC.KUOC_VM.TGIAN_VAY = Convert.ToInt32(txtThoiHanVay.Value);
                TDVMKHEUOC.KUOC_VM.TGIAN_VAY_DVI_TINH = lstThoiHanVay.ElementAt(cmbThoiHanVay.SelectedIndex).KeywordStrings.First();
                TDVMKHEUOC.KUOC_VM.NGAY_LAP_KUOC = "";
                TDVMKHEUOC.KUOC_VM.TRGOC_HTHUC = lstHinhThucTraGoc.ElementAt(cmbDinhKyTraGoc.SelectedIndex).KeywordStrings.First();
                TDVMKHEUOC.KUOC_VM.TRLAI_HTHUC = lstHinhThucTraLai.ElementAt(cmbDinhKyTraLai.SelectedIndex).KeywordStrings.First();
                TDVMKHEUOC.KUOC_VM.TRGOC_SO_KY = 0;
                TDVMKHEUOC.KUOC_VM.TRLAI_SO_KY = 0;
                TDVMKHEUOC.KUOC_VM.TTHAI_KUOC = TThaiKUOC;
                if (!teldtNgayLapKU.Value.IsNullOrEmpty())
                    TDVMKHEUOC.KUOC_VM.NGAY_LAP_KUOC = LDateTime.DateToString((DateTime)teldtNgayLapKU.Value, ApplicationConstant.defaultDateTimeFormat);
                TDVMKHEUOC.KUOC_VM.NGAY_GIAI_NGAN = "";
                if (!teldtNgayPhatVon.Value.IsNullOrEmpty())
                    TDVMKHEUOC.KUOC_VM.NGAY_GIAI_NGAN = LDateTime.DateToString((DateTime)teldtNgayPhatVon.Value, ApplicationConstant.defaultDateTimeFormat);
                TDVMKHEUOC.KUOC_VM.SO_TIEN_GIAI_NGAN = 0;
                if (!dtNgayBatDauTraNo.Value.IsNullOrEmpty())
                {
                    TDVMKHEUOC.KUOC_VM.KHOACH_NGAY_LAP = LDateTime.DateToString((DateTime)dtNgayBatDauTraNo.Value, ApplicationConstant.defaultDateTimeFormat);
                    TDVMKHEUOC.NGAY_BD_TRA = LDateTime.DateToString((DateTime)dtNgayBatDauTraNo.Value, ApplicationConstant.defaultDateTimeFormat);
                }
                if (!dtNgayBatDauTraNoCum.Value.IsNullOrEmpty())
                {
                    TDVMKHEUOC.KUOC_VM.KHOACH_NGAY_LAP_CUM = LDateTime.DateToString((DateTime)dtNgayBatDauTraNoCum.Value, ApplicationConstant.defaultDateTimeFormat);
                }
                if (!txtSoTienVay.Value.IsNullOrEmpty())
                    TDVMKHEUOC.KUOC_VM.SO_TIEN_GIAI_NGAN = (decimal)txtSoTienVay.Value;
                TDVMKHEUOC.KUOC_VM.LAI_SUAT = txtLSuat.Value != null ? (decimal)txtLSuat.Value : 0;
                TDVMKHEUOC.KUOC_VM.LAI_SUAT_CLN = 0;
                TDVMKHEUOC.KUOC_VM.LAI_SUAT_QH = TDVMKHEUOC.KUOC_VM.LAI_SUAT * (TDVMHDTD.HDTD_VM.LSUAT_QHAN != null ? (decimal)TDVMHDTD.HDTD_VM.LSUAT_QHAN : 1);
                TDVMKHEUOC.KUOC_VM.TTHAI_BGHI = bghi.layGiaTri();
                TDVMKHEUOC.KUOC_VM.TTHAI_NVU = nghiepvu.layGiaTri();
                TDVMKHEUOC.KUOC_VM.LOAI_LSUAT = lstLoaiLSuat.ElementAt(cmbLoaiLSuat.SelectedIndex).KeywordStrings.First();
                TDVMKHEUOC.KUOC_VM.LSUAT_BIEN_DO = txtLSBienDo.Value != null ? (decimal)txtLSBienDo.Value : 0;
                TDVMKHEUOC.KUOC_VM.DGIA_SO_DKY = (int?)txtDinhKyDanhGia.Value;
                if (cmbDinhKyDanhGia.SelectedIndex >= 0)
                    TDVMKHEUOC.KUOC_VM.DGIA_DVI_TINH = lstDinhKyDanhGiaLS.ElementAt(cmbDinhKyDanhGia.SelectedIndex).KeywordStrings.First();
                if (cmbLoaiNguonVon.SelectedIndex >= 0)
                    TDVMKHEUOC.KUOC_VM.NV_LOAI_NVON = lstLoaiNguonVon.ElementAt(cmbLoaiNguonVon.SelectedIndex).KeywordStrings.First();
                TDVMKHEUOC.KUOC_VM.NV_MA_HOP_DONG = txtHopDong.Text;
                TDVMKHEUOC.KUOC_VM.NV_STIEN_VAY = (decimal?)telnumSoTienVay.Value;
                TDVMKHEUOC.KUOC_VM.NV_STIEN_GNGAN = (decimal?)telnumSoTienGiaiNgan.Value;
                TDVMKHEUOC.KUOC_VM.HE_SO = (decimal)txtHeSo.Value;
                if (cmbMaMucDichVay.SelectedIndex >= 0)
                    TDVMKHEUOC.KUOC_VM.MUC_DICH_VAY = lstMucDichVayVon.ElementAt(cmbMaMucDichVay.SelectedIndex).KeywordStrings.First();
                if (TDVMKHEUOC.KUOC_VM.ID.IsNullOrEmpty())
                {
                    TDVMKHEUOC.KUOC_VM.ID = 0;
                    TDVMKHEUOC.KUOC_VM.SO_DU = 0;
                    TDVMKHEUOC.KUOC_VM.NGAY_DAO_HAN = "";
                    TDVMKHEUOC.KUOC_VM.NGAY_GIA_HAN = "";
                    TDVMKHEUOC.KUOC_VM.NGAY_CHUYEN_QH = "";
                    TDVMKHEUOC.KUOC_VM.NHOM_NO_HIEN_TAI = "NHOM1";
                    TDVMKHEUOC.KUOC_VM.THU_LAI_DEN_NGAY = "";
                    TDVMKHEUOC.KUOC_VM.LAI_TREO = 0;
                    TDVMKHEUOC.KUOC_VM.SO_TAI_KHOAN = "";
                    TDVMKHEUOC.KUOC_VM.LAI_DU_THU = 0;
                    TDVMKHEUOC.KUOC_VM.LAI_PHAI_THU = 0;
                    TDVMKHEUOC.KUOC_VM.LAI_DA_THU = 0;
                    TDVMKHEUOC.KUOC_VM.GOC_DA_THU = 0;
                    TDVMKHEUOC.KUOC_VM.SO_TIEN_TLDP = 0;
                    TDVMKHEUOC.KUOC_VM.LAI_DA_XUAT_NB = 0;
                    TDVMKHEUOC.KUOC_VM.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    TDVMKHEUOC.KUOC_VM.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    TDVMKHEUOC.KUOC_VM.MA_DVI_QLY = ClientInformation.MaDonVi;
                    TDVMKHEUOC.KUOC_VM.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                }
                else
                {
                    TDVMKHEUOC.KUOC_VM.NGUOI_NHAP = txtNguoiLap.Text;
                    TDVMKHEUOC.KUOC_VM.NGAY_NHAP = LDateTime.DateToString((DateTime)teldtNgayNhap.Value, ApplicationConstant.defaultDateTimeFormat);
                    TDVMKHEUOC.KUOC_VM.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    TDVMKHEUOC.KUOC_VM.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                }
                TDVMKHEUOC.DSACH_TSDB = lstTSDBVM.ToArray();
                lstKeHoach = raddgrLichTraNo.ItemsSource as List<TD_KHOACHVM_CT>;
                lstKeHoachDKien = raddgrThongTinLapLich.ItemsSource as List<TD_KHOACHVM>;
                // Cập nhật nguyên nhân trả nợ cho điều kiện kế hoạch
                foreach (TD_KHOACHVM keHoach in lstKeHoachDKien)
                {
                    if (cmbNguyenNhanThayDoi.SelectedIndex >= 0)
                        keHoach.MA_NNHAN_TDOI = lstNguyenNhanTDoi.ElementAt(cmbNguyenNhanThayDoi.SelectedIndex).KeywordStrings.FirstOrDefault();
                    else
                        keHoach.MA_NNHAN_TDOI = null;
                }
                TDVMKHEUOC.DSACH_KHOACHVM = lstKeHoachDKien.ToArray();
                TDVMKHEUOC.DSACH_KHOACHVM_CTIET = lstKeHoach.ToArray();
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            
        }

        public void SetDataForm()
        {
            try
            {
                Dispatcher.CurrentDispatcher.DelayInvoke("SetDataForm", () =>
                {
                    DataSet ds = new TinDungProcess().getThongTinChiTietKUOCVMByID(TDVMKHEUOC.KUOC_VM.ID.ToString());

                    Dispatcher.CurrentDispatcher.DelayInvoke("SetDataTabThongTinChung", () =>
                    {
                        SetDataTabThongTinChung(ds);
                    }, TimeSpan.FromSeconds(0));

                    Dispatcher.CurrentDispatcher.DelayInvoke("SetDataTabLichTraNo", () =>
                    {
                        SetDataTabLichTraNo(ds);
                    }, TimeSpan.FromSeconds(0));

                    Dispatcher.CurrentDispatcher.DelayInvoke("SetDataTabTSDB", () =>
                    {
                        SetDataTabTSDB(ds);
                    }, TimeSpan.FromSeconds(0));


                    Dispatcher.CurrentDispatcher.DelayInvoke("SetDataTabThongTinKiemSoat", () =>
                    {
                        SetDataTabThongTinKiemSoat(ds);
                    }, TimeSpan.FromSeconds(0));
                }, TimeSpan.FromSeconds(0));

                //SetDataTabThongTinChung(ds);
                //SetDataTabLichTraNo(ds);
                //SetDataTabTSDB(ds);
                //SetDataTabThongTinKiemSoat(ds);

                
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void SetDataTabThongTinChung(DataSet ds)
        {
            try
            {
                DataTable dt = ds.Tables["TTIN_CTIET"];
                foreach (DataColumn columns in dt.Columns)
                {
                    PropertyInfo property = TDVMKHEUOC.KUOC_VM.GetType().GetProperty(columns.ColumnName);

                    if (property != null)
                    {
                        object GiaTri = null;
                        if (!((object)dt.Rows[0][columns.ColumnName]).Equals(DBNull.Value))
                            GiaTri = dt.Rows[0][columns.ColumnName];
                        property.SetValue(TDVMKHEUOC.KUOC_VM, GiaTri, null);
                    }
                }
                iDKheUoc = TDVMKHEUOC.KUOC_VM.ID;
                TThaiKUOC = TDVMKHEUOC.KUOC_VM.TTHAI_KUOC;
                TThaiNVu = TDVMKHEUOC.KUOC_VM.TTHAI_NVU;
                TDVMHDTD.HDTD_VM.ID = TDVMKHEUOC.KUOC_VM.ID_HDTDVM;
                TDVMHDTD.HDTD_VM.ID_KHANG = Convert.ToInt32(dt.Rows[0]["ID_KHANG"]);
                TDVMHDTD.HDTD_VM.MA_KHANG = dt.Rows[0]["MA_KHANG"].ToString();
                txtSoKheUoc.Text = TDVMKHEUOC.KUOC_VM.MA_KUOCVM;
                txtSoHDTD.Text = TDVMHDTD.HDTD_VM.MA_HDTDVM = TDVMKHEUOC.KUOC_VM.MA_HDTDVM;
                teldtNgayLapKU.Value = LDateTime.StringToDate(TDVMKHEUOC.KUOC_VM.NGAY_LAP_KUOC, ApplicationConstant.defaultDateTimeFormat);
                txtTenKhachHang.Text = dt.Rows[0]["TEN_KHANG"].ToString();
                txtMaSanPham.Text = TDVMKHEUOC.KUOC_VM.MA_SAN_PHAM;
                txtHeSo.Value = (double)TDVMKHEUOC.KUOC_VM.HE_SO;
                txtSoTienVay.Value = (double)TDVMKHEUOC.KUOC_VM.SO_TIEN_GIAI_NGAN;
                txtThoiHanVay.Value = (double)TDVMKHEUOC.KUOC_VM.TGIAN_VAY;
                cmbThoiHanVay.SelectedIndex = lstThoiHanVay.IndexOf(lstThoiHanVay.FirstOrDefault(e => e.KeywordStrings.First().Equals(TDVMKHEUOC.KUOC_VM.TGIAN_VAY_DVI_TINH)));
                cmbMaMucDichVay.SelectedIndex = lstMucDichVayVon.IndexOf(lstMucDichVayVon.FirstOrDefault(e => e.KeywordStrings.First().Equals(TDVMKHEUOC.KUOC_VM.MUC_DICH_VAY)));
                teldtNgayPhatVon.Value = LDateTime.StringToDate(TDVMKHEUOC.KUOC_VM.NGAY_GIAI_NGAN, ApplicationConstant.defaultDateTimeFormat);
                if(!LObject.IsNullOrEmpty(TDVMKHEUOC.KUOC_VM.KHOACH_NGAY_LAP))
                    dtNgayBatDauTraNo.Value = LDateTime.StringToDate(TDVMKHEUOC.KUOC_VM.KHOACH_NGAY_LAP, ApplicationConstant.defaultDateTimeFormat);
                if (!LObject.IsNullOrEmpty(TDVMKHEUOC.KUOC_VM.KHOACH_NGAY_LAP_CUM))
                    dtNgayBatDauTraNoCum.Value = LDateTime.StringToDate(TDVMKHEUOC.KUOC_VM.KHOACH_NGAY_LAP_CUM, ApplicationConstant.defaultDateTimeFormat);
                cmbLoaiNguonVon.SelectedIndex = lstLoaiNguonVon.IndexOf(lstLoaiNguonVon.FirstOrDefault(e => e.KeywordStrings.First().Equals(TDVMKHEUOC.KUOC_VM.NV_LOAI_NVON)));
                txtHopDong.Text = TDVMKHEUOC.KUOC_VM.NV_MA_HOP_DONG;
                txtMaLaiSuat.Text = TDVMKHEUOC.KUOC_VM.MA_LSUAT;
                cmbLoaiLSuat.SelectedIndex = lstLoaiLSuat.IndexOf(lstLoaiLSuat.FirstOrDefault(e => e.KeywordStrings.First().Equals(TDVMKHEUOC.KUOC_VM.LOAI_LSUAT)));
                txtLSBienDo.Value = (double)TDVMKHEUOC.KUOC_VM.LSUAT_BIEN_DO;
                txtDinhKyDanhGia.Value = (double?)TDVMKHEUOC.KUOC_VM.DGIA_SO_DKY;
                cmbDinhKyDanhGia.SelectedIndex = lstDinhKyDanhGiaLS.IndexOf(lstDinhKyDanhGiaLS.FirstOrDefault(e => e.KeywordStrings.First().Equals(TDVMKHEUOC.KUOC_VM.DGIA_DVI_TINH)));
                cmbDinhKyTraGoc.SelectedIndex = lstHinhThucTraGoc.IndexOf(lstHinhThucTraGoc.FirstOrDefault(e => e.KeywordStrings.First().Equals(TDVMKHEUOC.KUOC_VM.TRGOC_HTHUC)));
                cmbDinhKyTraLai.SelectedIndex = lstHinhThucTraLai.IndexOf(lstHinhThucTraLai.FirstOrDefault(e => e.KeywordStrings.First().Equals(TDVMKHEUOC.KUOC_VM.TRLAI_HTHUC)));
                decimal dLaiSuat = TDVMKHEUOC.KUOC_VM.LAI_SUAT;
                txtLSuat.Value = (double)dLaiSuat;
                LayDanhSachNgayDauKy();
                if (!action.Equals(DatabaseConstant.Action.SUA))
                {
                    CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC);
                    SetEnabledAllControl(false);
                    RefeshButton();
                }
                else
                {
                    if (TThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    {
                        TThaiNVu = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                        SetEnabledAllControl(false);
                        TThaiNVu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    }
                    Modify();
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void SetDataTabLichTraNo(DataSet ds)
        {
            try
            {
                lstKeHoachDKien.Clear();
                lstKeHoach.Clear();
                DataTable dt = ds.Tables["LICH_TRA_NO"];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        TD_KHOACHVM objKHoachVM = new TD_KHOACHVM();
                        foreach (DataColumn columns in dt.Columns)
                        {
                            PropertyInfo property = objKHoachVM.GetType().GetProperty(columns.ColumnName);

                            if (property != null)
                            {
                                object GiaTri = null;
                                if (!((object)dr[columns.ColumnName]).Equals(DBNull.Value))
                                    GiaTri = dr[columns.ColumnName];
                                property.SetValue(objKHoachVM, GiaTri, null);
                            }
                        }
                        lstKeHoachDKien.Add(objKHoachVM);
                    }
                    raddgrThongTinLapLich.ItemsSource = null;
                    raddgrThongTinLapLich.ItemsSource = lstKeHoachDKien;
                }
                dt = ds.Tables["LICH_TRA_NO_CT"];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        TD_KHOACHVM_CT objKHoachVMCT = new TD_KHOACHVM_CT();
                        foreach (DataColumn columns in dt.Columns)
                        {
                            PropertyInfo property = objKHoachVMCT.GetType().GetProperty(columns.ColumnName);

                            if (property != null)
                            {
                                object GiaTri = null;
                                if (!((object)dr[columns.ColumnName]).Equals(DBNull.Value))
                                    GiaTri = dr[columns.ColumnName];
                                property.SetValue(objKHoachVMCT, GiaTri, null);
                            }
                        }
                        lstKeHoach.Add(objKHoachVMCT);
                    }
                    raddgrLichTraNo.ItemsSource = null;
                    raddgrLichTraNo.ItemsSource = lstKeHoach;
                    raddgrTinhTrangTraNo.ItemsSource = null;
                    raddgrTinhTrangTraNo.ItemsSource = lstKeHoach;
                    grbLichTraNo.Visibility = Visibility.Collapsed;
                    raddgrLichTraNo.Visibility = Visibility.Collapsed;
                    expTinhTrangTraNo.IsExpanded = true;
                }
                //tlbLapKeHoach_Click(null, null);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void SetDataTabTSDB(DataSet ds)
        {
            try
            {
                DataTable dtTaiSanDB = ds.Tables["TSDB"];
                lstTSDBVM = new List<DANH_SACH_TSDB>();
                foreach (DataRow dr in dtTaiSanDB.Rows)
                {
                    DANH_SACH_TSDB objTSDB = new DANH_SACH_TSDB();
                    foreach (DataColumn dc in dr.Table.Columns)
                    {
                        PropertyInfo proper = objTSDB.GetType().GetProperty(dc.ColumnName);
                        if (proper != null)
                        {
                            proper.SetValue(objTSDB, dr[dc.ColumnName], null);
                        }
                    }
                    lstTSDBVM.Add(objTSDB);
                }
                raddgrTSDB.ItemsSource = lstTSDBVM;
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void SetDataTabPhi(DataSet ds)
        {
            
        }

        void SetDataTabThongTinKiemSoat(DataSet ds)
        {
            try
            {
                TThaiNVu = TDVMKHEUOC.KUOC_VM.TTHAI_NVU;
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
                txtNguoiLap.Text = TDVMKHEUOC.KUOC_VM.NGUOI_NHAP;
                txtNguoiCapNhat.Text = TDVMKHEUOC.KUOC_VM.NGUOI_CNHAT;
                teldtNgayNhap.Value = LDateTime.StringToDate(TDVMKHEUOC.KUOC_VM.NGAY_NHAP, ApplicationConstant.defaultDateTimeFormat);
                if (!TDVMKHEUOC.KUOC_VM.NGAY_CNHAT.IsNullOrEmptyOrSpace())
                    teldtNgayCNhat.Value = LDateTime.StringToDate(TDVMKHEUOC.KUOC_VM.NGAY_CNHAT, ApplicationConstant.defaultDateTimeFormat);
                txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(TDVMKHEUOC.KUOC_VM.TTHAI_BGHI);
                LayChiTietLaiSuat();
                txtLSuat.Value = Convert.ToDouble(ds.Tables["TTIN_CTIET"].Rows[0]["LAI_SUAT"]);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            
        }

        private void tlbLapKeHoach_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!LObject.IsNullOrEmpty(sender))
                {
                    if (!VadidateDieuKienKeHoach())
                        return;
                }
                GetDataForm(BusinessConstant.TrangThaiBanGhi.SU_DUNG, BusinessConstant.TrangThaiNghiepVu.DA_DUYET);
                _TDVMKHEUOC.KUOC_VM.TTHAI_NVU = TThaiNVu;
                int iret = new TinDungProcess().TinhToanLichTraNo(ref _TDVMKHEUOC);
                if (iret > 0)
                    lstKeHoach = _TDVMKHEUOC.DSACH_KHOACHVM_CTIET.ToList();
                if(LObject.IsNullOrEmpty(sender))
                {
                    raddgrTinhTrangTraNo.ItemsSource = null;
                    raddgrTinhTrangTraNo.ItemsSource = lstKeHoach;
                    grbLichTraNo.Visibility = Visibility.Collapsed;
                    raddgrLichTraNo.Visibility = Visibility.Collapsed;
                    expTinhTrangTraNo.IsExpanded = true;
                }
                else
                {
                    raddgrLichTraNo.ItemsSource = null;
                    raddgrLichTraNo.ItemsSource = lstKeHoach;
                    expTinhTrangTraNo.IsExpanded = false;
                    grbLichTraNo.Visibility = Visibility.Visible;
                    raddgrLichTraNo.Visibility = Visibility.Visible;
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void LayChiTietLaiSuat()
        {
            try
            {
                if (!txtMaLaiSuat.Text.IsNullOrEmptyOrSpace() & txtSoTienVay.Value != null & txtThoiHanVay.Value != null)
                {
                    txtMaLaiSuat.Tag = TDVMKHEUOC.KUOC_VM.MA_LSUAT = txtMaLaiSuat.Text;
                    TDVMKHEUOC.KUOC_VM.SO_TIEN_GIAI_NGAN = (decimal)txtSoTienVay.Value;
                    TDVMKHEUOC.KUOC_VM.TGIAN_VAY = (int)txtThoiHanVay.Value;
                    TDVMKHEUOC.KUOC_VM.TGIAN_VAY_DVI_TINH = lstThoiHanVay.ElementAt(cmbThoiHanVay.SelectedIndex).KeywordStrings.First();
                    int iret = new TinDungProcess().TinhToanLaiSuat(ref _TDVMKHEUOC);
                    if (iret == 1)
                    {
                        dLaiSuat = TDVMKHEUOC.KUOC_VM.LAI_SUAT;
                        txtLSuat.Value = (double)TDVMKHEUOC.KUOC_VM.LAI_SUAT + txtLSBienDo.Value.Value;
                        cmbLSuatDViTinh.SelectedIndex = lstDonViTinhLSuat.IndexOf(lstDonViTinhLSuat.FirstOrDefault(e => e.KeywordStrings.First().Equals(TDVMKHEUOC.LSUAT_VM.DVI_TINH)));
                        lblLSuat.Content = TDVMKHEUOC.LSUAT_VM.MO_TA;
                    }
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void LayChiTietSoTienGiaiNgan()
        {
            try
            {
                TDVMKHEUOC.KUOC_VM.MA_HDTDVM = txtSoHDTD.Text;
                TDVMKHEUOC.KUOC_VM.HE_SO = (decimal)txtHeSo.Value.Value;
                TDVMKHEUOC.KUOC_VM.MA_SAN_PHAM = txtMaSanPham.Text;
                TDVMKHEUOC.KUOC_VM.TGIAN_VAY = (int)txtThoiHanVay.Value.Value;
                TDVMKHEUOC.KUOC_VM.TGIAN_VAY_DVI_TINH = lstThoiHanVay.ElementAt(cmbThoiHanVay.SelectedIndex).KeywordStrings.First();
                int iret = new TinDungProcess().TinhToanSoTienGiaiNgan(ref _TDVMKHEUOC);
                int iThoiGianVay = TDVMKHEUOC.KUOC_VM.TGIAN_VAY;
                if (iret == 1)
                {
                    
                    sSoTienGiaiNgan = _TDVMKHEUOC.KUOC_VM.SO_TIEN_GIAI_NGAN;
                    txtSoTienVay.Value = (double)_TDVMKHEUOC.KUOC_VM.SO_TIEN_GIAI_NGAN;

                    if (!_TDVMKHEUOC.KUOC_VM.TGIAN_VAY_DVI_TINH.IsNullOrEmptyOrSpace())
                    {
                        cmbThoiHanVay.IsEnabled = false;
                        cmbThoiHanVay.SelectedIndex = lstThoiHanVay.IndexOf(lstThoiHanVay.FirstOrDefault(e => e.KeywordStrings.First().Equals(_TDVMKHEUOC.KUOC_VM.TGIAN_VAY_DVI_TINH)));
                    }
                }
                if (!LObject.IsNullOrEmpty(_TDVMKHEUOC.VONG_VAY))
                {
                    cmbThoiHanVay.IsEnabled = false;
                    if (_TDVMKHEUOC.VONG_VAY.TCHAT_GOC_VAY.Equals(BusinessConstant.TINH_CHAT_VONG_VAY.CO_DINH.layGiaTri()))
                        txtSoTienVay.IsEnabled = false;
                    else
                        txtSoTienVay.IsEnabled = true;
                    if (_TDVMKHEUOC.VONG_VAY.TCHAT_KY_HAN.Equals(BusinessConstant.TINH_CHAT_VONG_VAY.CO_DINH.layGiaTri()))
                    {
                        txtThoiHanVay.Value = (double)iThoiGianVay;
                        txtThoiHanVay.IsEnabled = false;
                    }
                    else
                        txtThoiHanVay.IsEnabled = true;
                }
                else
                {
                    txtSoTienVay.IsEnabled = true;
                    txtThoiHanVay.IsEnabled = true;
                    cmbThoiHanVay.IsEnabled = true;
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            
        }

        bool Validation()
        {
            bool bReturn = true;
            string sGiaTriTSo = "";
            try
            {
                if (teldtNgayLapKU.Value.IsNullOrEmpty())
                {
                    CommonFunction.ThongBaoTrong(lblNgayLapKU.Content.ToString());
                    teldtNgayLapKU.Focus();
                    bReturn = false;
                }
                else if (txtSoHDTD.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblSoHDTD.Content.ToString());
                    txtSoHDTD.Focus();
                    bReturn = false;
                }
                else if (txtMaSanPham.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblMaSanPham.Content.ToString());
                    txtMaSanPham.Focus();
                    bReturn = false;
                }
                else if (txtHeSo.Value.IsNullOrEmpty())
                {
                    CommonFunction.ThongBaoTrong(lblHeSo.Content.ToString());
                    txtHeSo.Focus();
                    bReturn = false;
                }
                else if (txtThoiHanVay.Value.IsNullOrEmpty())
                {
                    CommonFunction.ThongBaoTrong(lblThoiHanVay.Content.ToString());
                    txtThoiHanVay.Focus();
                    bReturn = false;
                }
                else if (teldtNgayPhatVon.Value.IsNullOrEmpty())
                {
                    CommonFunction.ThongBaoTrong(lblNgayPhatVon.Content.ToString());
                    teldtNgayPhatVon.Focus();
                    bReturn = false;
                }
                else if (dtNgayBatDauTraNoCum.Value.GetValueOrDefault() < dtNgayBatDauTraNo.Value.GetValueOrDefault())
                {
                    LMessage.ShowMessage("M.TinDung.ucKheUocCT.NgayBatDauTraNoCum", LMessage.MessageBoxType.Warning);
                    bReturn = false;
                }
                else if (LObject.IsNullOrEmpty(txtMaLaiSuat.Tag) || txtMaLaiSuat.Tag.ToString().IsNullOrEmptyOrSpace())
                {
                    sGiaTriTSo = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_KTRA_LAI_SUAT_KUOC, ClientInformation.MaDonVi);
                    if (!LObject.IsNullOrEmpty(sGiaTriTSo) && sGiaTriTSo.Equals(BusinessConstant.CoKhong.CO.layGiaTri()))
                    {
                        CommonFunction.ThongBaoTrong(lblLaiSuat.Content.ToString());
                        btnMaLaiSuat.Focus();
                        bReturn = false;
                    }
                }
                
                else if (LObject.IsNullOrEmpty(lstKeHoach) || lstKeHoach.Count < 1)
                {
                    if (!VadidateDieuKienKeHoach())
                        bReturn = false;
                    else
                    {
                        string sMessage = LLanguage.SearchResourceByKey("M.TinDung.ucKheUocCT.KeHoachKhongDuocTrong") + " " + LLanguage.SearchResourceByKey("M.TinDung.ucKheUocCT.HinhThucTraLai") + " " + cmbDinhKyTraLai.Text + " " + LLanguage.SearchResourceByKey("M.TinDung.ucKheUocCT.HinhThucTraGoc") + " " + cmbDinhKyTraGoc.Text + "?";
                        if (LMessage.ShowMessage(sMessage, LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                                bReturn = true;
                        else
                        {
                            titemLichTraNo.IsSelected = true;
                            tlbLapKeHoach.Focus();
                            bReturn = false;
                        }
                    }
                    
                }
                else if (!lstHinhThucTraLai.ElementAt(cmbDinhKyTraLai.SelectedIndex).KeywordStrings.FirstOrDefault().Equals(BusinessConstant.HINH_THUC_TRA_LAI.DAU_KY.layGiaTri()) && lstKeHoach.Where(e => e.KH_NGAY_TRA.Equals(LDateTime.DateToString(teldtNgayPhatVon.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat)) && e.KH_TRA_LAI_KHONG.Equals(BusinessConstant.CoKhong.CO.layGiaTri())).ToList().Count > 0)
                {
                    LMessage.ShowMessage("M.TinDung.ucKheUocCT.KeHoachKhongPhuHop", LMessage.MessageBoxType.Warning);
                    titemLichTraNo.IsSelected = true;
                    tlbLapKeHoach.Focus();
                    bReturn = false;
                }
                else if (lstHinhThucTraLai.ElementAt(cmbDinhKyTraLai.SelectedIndex).KeywordStrings.FirstOrDefault().Equals(BusinessConstant.HINH_THUC_TRA_LAI.DAU_KY.layGiaTri()) && lstKeHoach.Where(e => e.KH_NGAY_TRA.Equals(LDateTime.DateToString(teldtNgayPhatVon.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat)) && e.KH_TRA_LAI_KHONG.Equals(BusinessConstant.CoKhong.CO.layGiaTri())).ToList().Count < 1)
                {
                    LMessage.ShowMessage("M.TinDung.ucKheUocCT.KeHoachKhongPhuHop", LMessage.MessageBoxType.Warning);
                    titemLichTraNo.IsSelected = true;
                    tlbLapKeHoach.Focus();
                    bReturn = false;
                }
                else if (lstKeHoach.Sum(e=>e.KH_TRA_GOC).GetValueOrDefault(0)!=(decimal)txtSoTienVay.Value.GetValueOrDefault(0))
                {
                    LMessage.ShowMessage("M.TinDung.ucKheUocCT.KeHoachKhongPhuHop", LMessage.MessageBoxType.Warning);
                    titemLichTraNo.IsSelected = true;
                    tlbLapKeHoach.Focus();
                    bReturn = false;
                }
            }
            catch (System.Exception ex)
            {
                bReturn = false;
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return bReturn;
        }

        void Modify()
        {
            List<int> lstId = new List<int>();
            lstId.Add(iDKheUoc);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.SUA,
            lstId);
            action = DatabaseConstant.Action.SUA;
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC);
            RefeshButton();
            SetEnabledAllControl(true);
        }

        void BeforeSave(BusinessConstant.TrangThaiNghiepVu nghiepvu, BusinessConstant.TrangThaiBanGhi bghi)
        {
            Cursor = Cursors.Wait;
            txtSoHDTD.Focus();
            try
            {
                if (!nghiepvu.Equals(BusinessConstant.TrangThaiNghiepVu.LUU_TAM))
                {
                    if (!Validation())
                    {
                        Cursor = Cursors.Arrow;
                        return;
                    }
                }
                GetDataForm(bghi, nghiepvu);
                OnSave();
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
            
        }
        void OnSave()
        {
            try
            {
                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                int iret = 0;
                if (iDKheUoc == 0)
                    iret = new TinDungProcess().ThemMoiKheUocTinDungViMo(ref _TDVMKHEUOC, ref lstResponseDetail);
                else
                    iret = new TinDungProcess().SuaKheUocTinDungViMo(ref _TDVMKHEUOC, ref lstResponseDetail);
                AfterSave(lstResponseDetail, iret);
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }           
        }
        void AfterSave(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            try
            {
                CommonFunction.ThongBaoKetQua(lstResponseDetail);
                // Yêu cầu Unlock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDKheUoc);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.SUA,
                lstId);
                action = DatabaseConstant.Action.XEM;
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
                if (!cbMultiAdd.IsChecked.GetValueOrDefault(false))
                {
                    if (iret > 0)
                        SetInfomation();
                }
                else
                {
                    if (iret > 0)
                        ClearForm();
                }
                
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }  
        }

        void BeforeDelete()
        {
            Cursor = Cursors.Wait;
            try
            {
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDKheUoc);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.XOA,
                lstId);
                OnDelete();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDKheUoc);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.XOA,
                lstId);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally { Cursor = Cursors.Arrow; }
        }
        void OnDelete()
        {
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (iDKheUoc != 0)
                {
                    TDVMKHEUOC.KUOC_VM.ID = iDKheUoc;
                    TDVMKHEUOC.KUOC_VM.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    List<TD_KUOCVM> lstKUOC = new List<TD_KUOCVM>();
                    lstKUOC.Add(TDVMKHEUOC.KUOC_VM);
                    TDVMKHEUOC.DSACH_KUOC_VM = lstKUOC.ToArray();
                    iret = new TinDungProcess().XoaKheUocTinDungViMo(ref _TDVMKHEUOC, ref lstResponseDetail);
                }
            AfterDelete(lstResponseDetail, iret);
        }
        void AfterDelete(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(iDKheUoc);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.XOA,
            lstId);
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            Cursor = Cursors.Arrow;
            if (iret == 0) ;
            else
                CommonFunction.CloseUserControl(this);
        }

        void BeforeApprove()
        {
            try
            {
                Cursor = Cursors.Wait;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDKheUoc);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.DUYET,
                lstId);
                OnApprove();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDKheUoc);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.DUYET,
                lstId);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally { Cursor = Cursors.Arrow; }
        }
        void OnApprove()
        {
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (iDKheUoc != 0)
            {
                TDVMKHEUOC.KUOC_VM.ID = iDKheUoc;
                TDVMKHEUOC.KUOC_VM.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                List<TD_KUOCVM> lstKUOC = new List<TD_KUOCVM>();
                lstKUOC.Add(TDVMKHEUOC.KUOC_VM);
                TDVMKHEUOC.DSACH_KUOC_VM = lstKUOC.ToArray();
                iret = new TinDungProcess().DuyetKheUocTinDungViMo(ref _TDVMKHEUOC, ref lstResponseDetail);
            }
            AfterApprove(lstResponseDetail, iret);
        }
        void AfterApprove(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(iDKheUoc);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.DUYET,
            lstId);
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            SetInfomation();
            Cursor = Cursors.Arrow;
        }

        void BeforeRefuse()
        {
            try
            {
                Cursor = Cursors.Wait;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDKheUoc);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);
                OnRefuse();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDKheUoc);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally { Cursor = Cursors.Arrow; }
        }
        void OnRefuse()
        {
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (iDKheUoc != 0)
            {
                TDVMKHEUOC.KUOC_VM.ID = iDKheUoc;
                TDVMKHEUOC.KUOC_VM.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                List<TD_KUOCVM> lstKUOC = new List<TD_KUOCVM>();
                lstKUOC.Add(TDVMKHEUOC.KUOC_VM);
                TDVMKHEUOC.DSACH_KUOC_VM = lstKUOC.ToArray();
                iret = new TinDungProcess().TuChoiDuyetKheUocTinDungViMo(ref _TDVMKHEUOC, ref lstResponseDetail);
            }
            AfterRefuse(lstResponseDetail, iret);
        }
        void AfterRefuse(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(iDKheUoc);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.TU_CHOI_DUYET,
            lstId);
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            Cursor = Cursors.Arrow;
            SetInfomation();
        }

        void BeforeCancel()
        {
            try
            {
                Cursor = Cursors.Wait;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDKheUoc);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                OnCancel();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDKheUoc);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }
        void OnCancel()
        {
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (iDKheUoc != 0)
            {
                TDVMKHEUOC.KUOC_VM.ID = iDKheUoc;
                TDVMKHEUOC.KUOC_VM.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                List<TD_KUOCVM> lstKUOC = new List<TD_KUOCVM>();
                lstKUOC.Add(TDVMKHEUOC.KUOC_VM);
                TDVMKHEUOC.DSACH_KUOC_VM = lstKUOC.ToArray();
                iret = new TinDungProcess().ThoaiDuyetKheUocTinDungViMo(ref _TDVMKHEUOC, ref lstResponseDetail);
            }
            AfterCancel(lstResponseDetail, iret);
        }
        void AfterCancel(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(iDKheUoc);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.THOAI_DUYET,
            lstId);
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            Cursor = Cursors.Arrow;
            SetInfomation();
        }
        private void LayDanhSachNgayDauKy()
        {
            try
            {
                if (LObject.IsNullOrEmpty(TDVMHDTD.HDTD_VM) || TDVMHDTD.HDTD_VM.ID == 0 || txtMaSanPham.Text.IsNullOrEmptyOrSpace())
                    return;
                lstNgayDauKy = new List<AutoCompleteEntry>();
                TDVMKHEUOC.KUOC_VM.ID = 0;
                TDVMKHEUOC.KUOC_VM.ID_HDTDVM = TDVMHDTD.HDTD_VM.ID;
                TDVMKHEUOC.KUOC_VM.TGIAN_VAY = (int)txtThoiHanVay.Value.GetValueOrDefault(0);
                TDVMKHEUOC.KUOC_VM.TGIAN_VAY_DVI_TINH = lstThoiHanVay.ElementAt(cmbThoiHanVay.SelectedIndex).KeywordStrings.FirstOrDefault();
                TDVMKHEUOC.KUOC_VM.NGAY_GIAI_NGAN = LDateTime.DateToString(teldtNgayPhatVon.Value.GetValueOrDefault(LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat)), ApplicationConstant.defaultDateTimeFormat);
                TDVMKHEUOC.KUOC_VM.MA_SAN_PHAM = txtMaSanPham.Text;
                TDVMKHEUOC.DSACH_KHOACHVM = null;
                TDVMKHEUOC.DSACH_KHOACHVM_CTIET = null;
                TDVMKHEUOC.KUOC_VM.MA_DVI_QLY = ClientInformation.MaDonVi;
                TDVMKHEUOC.NGAY_BD_TRA = LDateTime.DateToString((DateTime)dtNgayBatDauTraNo.Value, ApplicationConstant.defaultDateTimeFormat);
                _TDVMKHEUOC.DSACH_NGAY_DKY = null;
                int iret = new TinDungProcess().TinhToanLichTraNo(ref _TDVMKHEUOC);
                if (LObject.IsNullOrEmpty(TDVMKHEUOC.DSACH_NGAY_DKY)) return;
                List<string> lstsNgayDauKy = TDVMKHEUOC.DSACH_NGAY_DKY.ToList();
                foreach (string sNgayDauKy in lstsNgayDauKy)
                {
                    lstNgayDauKy.Add(new AutoCompleteEntry(LDateTime.StringToDate(sNgayDauKy, ApplicationConstant.defaultDateTimeFormat).ToString("dd/MM/yyyy"), sNgayDauKy, sNgayDauKy));
                }
                InitDieuKienLapLich();
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            
        }

        private void InitDieuKienLapLich()
        {
            try
            {
                lstKeHoachDKien.Clear();
                decimal SoTienVay = (decimal)txtSoTienVay.Value.GetValueOrDefault(0);
                if (lstNgayDauKy.Count > 1)
                {
                    if (lstHinhThucTraGoc.ElementAt(cmbDinhKyTraGoc.SelectedIndex).KeywordStrings.FirstOrDefault().Equals(BusinessConstant.HINH_THUC_TRA_GOC.TRA_SAU.layGiaTri()))
                    {
                        TD_KHOACHVM objKHoachVM = new TD_KHOACHVM();
                        objKHoachVM.LOAI_HINH_LAP_KH = BusinessConstant.LOAI_HINH_LAP_KE_HOACH.GOC.layGiaTri();
                        objKHoachVM.MA_HTHUC = BusinessConstant.HINH_THUC_THANH_TOAN.CUOI_KY.layGiaTri();
                        objKHoachVM.NGAY_BDAU = lstNgayDauKy[1].KeywordStrings.FirstOrDefault();
                        objKHoachVM.SO_KY = objKHoachVM.TAN_SUAT = lstNgayDauKy.Count - 1;
                        objKHoachVM.SO_TIEN = SoTienVay;
                        lstKeHoachDKien.Add(objKHoachVM);
                    }
                    else
                    {
                        TD_KHOACHVM objKHoachVM = new TD_KHOACHVM();
                        int SoLamTron = Convert.ToInt32(new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_MUC_LAM_TRON_TD_TINH_LAI, ClientInformation.MaDonVi));
                        objKHoachVM.LOAI_HINH_LAP_KH = BusinessConstant.LOAI_HINH_LAP_KE_HOACH.GOC.layGiaTri();
                        objKHoachVM.MA_HTHUC = BusinessConstant.HINH_THUC_THANH_TOAN.DAU_KY.layGiaTri();
                        objKHoachVM.NGAY_BDAU = lstNgayDauKy[1].KeywordStrings.FirstOrDefault();
                        objKHoachVM.SO_KY = lstNgayDauKy.Count - 1;
                        objKHoachVM.TAN_SUAT = 1;
                        objKHoachVM.SO_TIEN = (SoTienVay / (lstNgayDauKy.Count - 1) / SoLamTron).Rounding(0) * SoLamTron;
                        lstKeHoachDKien.Add(objKHoachVM);
                    }
                    if (lstHinhThucTraLai.ElementAt(cmbDinhKyTraLai.SelectedIndex).KeywordStrings.FirstOrDefault().Equals(BusinessConstant.HINH_THUC_TRA_LAI.DINH_KY.layGiaTri()))
                    {
                        TD_KHOACHVM objKHoachVM = new TD_KHOACHVM();
                        objKHoachVM.LOAI_HINH_LAP_KH = BusinessConstant.LOAI_HINH_LAP_KE_HOACH.LAI.layGiaTri();
                        objKHoachVM.MA_HTHUC = BusinessConstant.HINH_THUC_THANH_TOAN.DAU_KY.layGiaTri();
                        objKHoachVM.NGAY_BDAU = lstNgayDauKy[1].KeywordStrings.FirstOrDefault();
                        objKHoachVM.SO_KY = lstNgayDauKy.Count - 1;
                        objKHoachVM.TAN_SUAT = 1;
                        lstKeHoachDKien.Add(objKHoachVM);
                    }
                    else if (lstHinhThucTraLai.ElementAt(cmbDinhKyTraLai.SelectedIndex).KeywordStrings.FirstOrDefault().Equals(BusinessConstant.HINH_THUC_TRA_LAI.CUOI_KY.layGiaTri()))
                    {
                        TD_KHOACHVM objKHoachVM = new TD_KHOACHVM();
                        objKHoachVM.LOAI_HINH_LAP_KH = BusinessConstant.LOAI_HINH_LAP_KE_HOACH.LAI.layGiaTri();
                        objKHoachVM.MA_HTHUC = BusinessConstant.HINH_THUC_THANH_TOAN.CUOI_KY.layGiaTri();
                        objKHoachVM.NGAY_BDAU = lstNgayDauKy[1].KeywordStrings.FirstOrDefault();
                        objKHoachVM.SO_KY = objKHoachVM.TAN_SUAT = lstNgayDauKy.Count - 1;
                        lstKeHoachDKien.Add(objKHoachVM);
                    }
                    else
                    {
                        TD_KHOACHVM objKHoachVM = new TD_KHOACHVM();
                        objKHoachVM.LOAI_HINH_LAP_KH = BusinessConstant.LOAI_HINH_LAP_KE_HOACH.LAI.layGiaTri();
                        objKHoachVM.MA_HTHUC = BusinessConstant.HINH_THUC_THANH_TOAN.DAU_KY.layGiaTri();
                        objKHoachVM.NGAY_BDAU = lstNgayDauKy[0].KeywordStrings.FirstOrDefault();
                        objKHoachVM.SO_KY = objKHoachVM.TAN_SUAT = 1;
                        lstKeHoachDKien.Add(objKHoachVM);
                    }
                    raddgrThongTinLapLich.ItemsSource = null;
                    raddgrThongTinLapLich.ItemsSource = lstKeHoachDKien;
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private bool VadidateDieuKienKeHoach()
        {
            List<ClientResponseDetail> lstClientResponseDetail = new List<ClientResponseDetail>();
            bool bReturn = true;
            try
            {
                int iTongSoKy = 1;
                int iKyTiepTheo = 1;
                int iTongSoKyTToan = lstNgayDauKy.Count;
                List<TD_KHOACHVM> lstDieuKienKH = null;
                string sLoaiHinhKeHoach = "";
                string sHinhThucGoc = lstHinhThucTraGoc.ElementAt(cmbDinhKyTraGoc.SelectedIndex).KeywordStrings.First();
                string sHinhThucLai = lstHinhThucTraLai.ElementAt(cmbDinhKyTraLai.SelectedIndex).KeywordStrings.First();
                // Check loại hình lập kế hoạch - gốc

                sLoaiHinhKeHoach = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.Goc");
                lstDieuKienKH = lstKeHoachDKien.Where(e => e.LOAI_HINH_LAP_KH.Equals(BusinessConstant.LOAI_HINH_LAP_KE_HOACH.GOC.layGiaTri())).ToList();
                if (!sHinhThucGoc.Equals(BusinessConstant.HINH_THUC_TRA_GOC.DINH_KY.layGiaTri()))
                {
                    if (lstDieuKienKH.Count > 1)
                    {
                        ClientResponseDetail oClientResponseDetail = new ClientResponseDetail();
                        oClientResponseDetail.Id = 0;
                        oClientResponseDetail.Object = "";
                        oClientResponseDetail.Operation = DatabaseConstant.Action.TINH_TOAN.layNgonNgu();
                        oClientResponseDetail.Result = LLanguage.SearchResourceByKey("M.DungChung.Result.KhongThanhCong");
                        oClientResponseDetail.Detail = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.LoaiHinhLapKHoach", new string[] { sLoaiHinhKeHoach });
                        lstClientResponseDetail.Add(oClientResponseDetail);
                        bReturn = false;
                    }
                    else
                    {
                        if (bReturn)
                        {
                            for (int i = 0; i < lstDieuKienKH.Count; i++)
                            {
                                int iTanSuat = lstDieuKienKH[i].TAN_SUAT.GetValueOrDefault(0);
                                int iSoKy = lstDieuKienKH[i].SO_KY.GetValueOrDefault(0);
                                int iSoDong = raddgrThongTinLapLich.Items.IndexOf(lstDieuKienKH[i]) + 1;
                                iKyTiepTheo = iTongSoKy;
                                iTongSoKy += iSoKy;
                                string NgayBatDau = lstDieuKienKH[i].NGAY_BDAU;
                                string NgayBatDauKy = lstDieuKienKH[i].NGAY_BDAU;
                                string HinhThucTToan = lstDieuKienKH[i].MA_HTHUC;
                                if (iKyTiepTheo < lstNgayDauKy.Count)
                                    NgayBatDauKy = lstNgayDauKy[iKyTiepTheo].KeywordStrings.First();
                                if (iSoKy % iTanSuat != 0)
                                {
                                    ClientResponseDetail oClientResponseDetail = new ClientResponseDetail();
                                    oClientResponseDetail.Id = 0;
                                    oClientResponseDetail.Object = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.DongDieuKien") + " " + iSoDong.ToString();
                                    oClientResponseDetail.Operation = DatabaseConstant.Action.TINH_TOAN.layNgonNgu();
                                    oClientResponseDetail.Result = LLanguage.SearchResourceByKey("M.DungChung.Result.KhongThanhCong");
                                    oClientResponseDetail.Detail = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.SoKyKhongChiaHetTanSuat");
                                    lstClientResponseDetail.Add(oClientResponseDetail);
                                    bReturn = false;
                                }
                                else if (iTongSoKy > iTongSoKyTToan)
                                {
                                    ClientResponseDetail oClientResponseDetail = new ClientResponseDetail();
                                    oClientResponseDetail.Id = 0;
                                    oClientResponseDetail.Object = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.DongDieuKien") + " " + iSoDong.ToString();
                                    oClientResponseDetail.Operation = DatabaseConstant.Action.TINH_TOAN.layNgonNgu();
                                    oClientResponseDetail.Result = LLanguage.SearchResourceByKey("M.DungChung.Result.KhongThanhCong");
                                    oClientResponseDetail.Detail = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.SoKy");
                                    lstClientResponseDetail.Add(oClientResponseDetail);
                                    bReturn = false;
                                }
                                else if (iTongSoKy > iTongSoKyTToan)
                                {
                                    ClientResponseDetail oClientResponseDetail = new ClientResponseDetail();
                                    oClientResponseDetail.Id = 0;
                                    oClientResponseDetail.Object = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.DongDieuKien") + " " + iSoDong.ToString();
                                    oClientResponseDetail.Operation = DatabaseConstant.Action.TINH_TOAN.layNgonNgu();
                                    oClientResponseDetail.Result = LLanguage.SearchResourceByKey("M.DungChung.Result.KhongThanhCong");
                                    oClientResponseDetail.Detail = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.SoKy");
                                    lstClientResponseDetail.Add(oClientResponseDetail);
                                    bReturn = false;
                                }
                                else if (NgayBatDauKy != NgayBatDau)
                                {
                                    ClientResponseDetail oClientResponseDetail = new ClientResponseDetail();
                                    oClientResponseDetail.Id = 0;
                                    oClientResponseDetail.Object = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.DongDieuKien") + " " + iSoDong.ToString();
                                    oClientResponseDetail.Operation = DatabaseConstant.Action.TINH_TOAN.layNgonNgu();
                                    oClientResponseDetail.Result = LLanguage.SearchResourceByKey("M.DungChung.Result.KhongThanhCong");
                                    oClientResponseDetail.Detail = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.NgayBatDau");
                                    lstClientResponseDetail.Add(oClientResponseDetail);
                                    bReturn = false;
                                }
                                else if (iTanSuat > 1 && HinhThucTToan.Equals(BusinessConstant.HINH_THUC_THANH_TOAN.DAU_KY))
                                {
                                    ClientResponseDetail oClientResponseDetail = new ClientResponseDetail();
                                    oClientResponseDetail.Id = 0;
                                    oClientResponseDetail.Object = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.DongDieuKien") + " " + iSoDong.ToString();
                                    oClientResponseDetail.Operation = DatabaseConstant.Action.TINH_TOAN.layNgonNgu();
                                    oClientResponseDetail.Result = LLanguage.SearchResourceByKey("M.DungChung.Result.KhongThanhCong");
                                    oClientResponseDetail.Detail = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.TanSuat");
                                    lstClientResponseDetail.Add(oClientResponseDetail);
                                    bReturn = false;
                                }
                            }
                            if (iTongSoKy != lstNgayDauKy.Count && bReturn)
                            {
                                ClientResponseDetail oClientResponseDetail = new ClientResponseDetail();
                                oClientResponseDetail.Id = 0;
                                oClientResponseDetail.Object = "";
                                oClientResponseDetail.Operation = DatabaseConstant.Action.TINH_TOAN.layNgonNgu();
                                oClientResponseDetail.Result = LLanguage.SearchResourceByKey("M.DungChung.Result.KhongThanhCong");
                                oClientResponseDetail.Detail = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.TongSoKy", new string[] { sLoaiHinhKeHoach });
                                lstClientResponseDetail.Add(oClientResponseDetail);
                                bReturn = false;
                            }
                        }
                    }
                }
                // Check loại hình lập kế hoạch - lãi
                iTongSoKy = 1;
                iKyTiepTheo = 1;
                sLoaiHinhKeHoach = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.Lai");
                lstDieuKienKH = lstKeHoachDKien.Where(e => e.LOAI_HINH_LAP_KH.Equals(BusinessConstant.LOAI_HINH_LAP_KE_HOACH.LAI.layGiaTri())).ToList();
                if (!sHinhThucLai.Equals(BusinessConstant.HINH_THUC_TRA_GOC.DINH_KY.layGiaTri()))
                {
                    if (lstDieuKienKH.Count > 1)
                    {
                        ClientResponseDetail oClientResponseDetail = new ClientResponseDetail();
                        oClientResponseDetail.Id = 0;
                        oClientResponseDetail.Object = "";
                        oClientResponseDetail.Operation = DatabaseConstant.Action.TINH_TOAN.layNgonNgu();
                        oClientResponseDetail.Result = LLanguage.SearchResourceByKey("M.DungChung.Result.KhongThanhCong");
                        oClientResponseDetail.Detail = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.LoaiHinhLapKHoach", new string[] { sLoaiHinhKeHoach });
                        lstClientResponseDetail.Add(oClientResponseDetail);
                        bReturn = false;
                    }
                }
                else
                {
                    if (bReturn)
                    {
                        for (int i = 0; i < lstDieuKienKH.Count; i++)
                        {
                            int iTanSuat = lstDieuKienKH[i].TAN_SUAT.GetValueOrDefault(0);
                            int iSoKy = lstDieuKienKH[i].SO_KY.GetValueOrDefault(0);
                            int iSoDong = raddgrThongTinLapLich.Items.IndexOf(lstDieuKienKH[i]) + 1;
                            iKyTiepTheo = iTongSoKy;
                            iTongSoKy += iSoKy;
                            string NgayBatDau = lstDieuKienKH[i].NGAY_BDAU;
                            string NgayBatDauKy = lstDieuKienKH[i].NGAY_BDAU;
                            string HinhThucTToan = lstDieuKienKH[i].MA_HTHUC;
                            if (iKyTiepTheo < lstNgayDauKy.Count)
                                NgayBatDauKy = lstNgayDauKy[iKyTiepTheo].KeywordStrings.First();
                            if (iSoKy % iTanSuat != 0)
                            {
                                ClientResponseDetail oClientResponseDetail = new ClientResponseDetail();
                                oClientResponseDetail.Id = 0;
                                oClientResponseDetail.Object = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.DongDieuKien") + " " + iSoDong.ToString();
                                oClientResponseDetail.Operation = DatabaseConstant.Action.TINH_TOAN.layNgonNgu();
                                oClientResponseDetail.Result = LLanguage.SearchResourceByKey("M.DungChung.Result.KhongThanhCong");
                                oClientResponseDetail.Detail = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.SoKyKhongChiaHetTanSuat");
                                lstClientResponseDetail.Add(oClientResponseDetail);
                                bReturn = false;
                            }
                            else if (iTongSoKy > iTongSoKyTToan)
                            {
                                ClientResponseDetail oClientResponseDetail = new ClientResponseDetail();
                                oClientResponseDetail.Id = 0;
                                oClientResponseDetail.Object = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.DongDieuKien") + " " + iSoDong.ToString();
                                oClientResponseDetail.Operation = DatabaseConstant.Action.TINH_TOAN.layNgonNgu();
                                oClientResponseDetail.Result = LLanguage.SearchResourceByKey("M.DungChung.Result.KhongThanhCong");
                                oClientResponseDetail.Detail = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.SoKy");
                                lstClientResponseDetail.Add(oClientResponseDetail);
                                bReturn = false;
                            }
                            else if (iTongSoKy > iTongSoKyTToan)
                            {
                                ClientResponseDetail oClientResponseDetail = new ClientResponseDetail();
                                oClientResponseDetail.Id = 0;
                                oClientResponseDetail.Object = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.DongDieuKien") + " " + iSoDong.ToString();
                                oClientResponseDetail.Operation = DatabaseConstant.Action.TINH_TOAN.layNgonNgu();
                                oClientResponseDetail.Result = LLanguage.SearchResourceByKey("M.DungChung.Result.KhongThanhCong");
                                oClientResponseDetail.Detail = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.SoKy");
                                lstClientResponseDetail.Add(oClientResponseDetail);
                                bReturn = false;
                            }
                            else if (NgayBatDauKy != NgayBatDau & !sHinhThucLai.Equals(BusinessConstant.HINH_THUC_TRA_LAI.DAU_KY.layGiaTri()))
                            {
                                ClientResponseDetail oClientResponseDetail = new ClientResponseDetail();
                                oClientResponseDetail.Id = 0;
                                oClientResponseDetail.Object = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.DongDieuKien") + " " + iSoDong.ToString();
                                oClientResponseDetail.Operation = DatabaseConstant.Action.TINH_TOAN.layNgonNgu();
                                oClientResponseDetail.Result = LLanguage.SearchResourceByKey("M.DungChung.Result.KhongThanhCong");
                                oClientResponseDetail.Detail = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.NgayBatDau");
                                lstClientResponseDetail.Add(oClientResponseDetail);
                                bReturn = false;
                            }
                            else if (iTanSuat > 1 && HinhThucTToan.Equals(BusinessConstant.HINH_THUC_THANH_TOAN.DAU_KY))
                            {
                                ClientResponseDetail oClientResponseDetail = new ClientResponseDetail();
                                oClientResponseDetail.Id = 0;
                                oClientResponseDetail.Object = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.DongDieuKien") + " " + iSoDong.ToString();
                                oClientResponseDetail.Operation = DatabaseConstant.Action.TINH_TOAN.layNgonNgu();
                                oClientResponseDetail.Result = LLanguage.SearchResourceByKey("M.DungChung.Result.KhongThanhCong");
                                oClientResponseDetail.Detail = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.TanSuat");
                                lstClientResponseDetail.Add(oClientResponseDetail);
                                bReturn = false;
                            }
                        }
                        if (iTongSoKy != lstNgayDauKy.Count && bReturn)
                        {
                            ClientResponseDetail oClientResponseDetail = new ClientResponseDetail();
                            oClientResponseDetail.Id = 0;
                            oClientResponseDetail.Object = "";
                            oClientResponseDetail.Operation = DatabaseConstant.Action.TINH_TOAN.layNgonNgu();
                            oClientResponseDetail.Result = LLanguage.SearchResourceByKey("M.DungChung.Result.KhongThanhCong");
                            oClientResponseDetail.Detail = LLanguage.SearchResourceByKey("U.TinDung.KUOC.ucKheUocCT.TongSoKy", new string[] { sLoaiHinhKeHoach });
                            lstClientResponseDetail.Add(oClientResponseDetail);
                            bReturn = false;
                        }
                    }
                }
                // Check loại hình lập kế hoạch - phí
                // Check loại hình lập kế hoạch - hoa hồng
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
                
            CommonFunction.ThongBaoKetQua(lstClientResponseDetail);
            return bReturn;
        }

        private void OnPreviewKheUoc()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(iDKheUoc))
            {
                LMessage.ShowMessage("M.TinDung.KheUoc.ucKheUocCT.KhongCoThongTinKheUoc", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {

                VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                Presentation.Process.BaoCaoServiceRef.DoiTuongBaoCao doiTuongBaoCao = new Presentation.Process.BaoCaoServiceRef.DoiTuongBaoCao();

                Presentation.Process.BaoCaoServiceRef.GIAO_DICH_BASE objGIAO_DICH_BASE = new Presentation.Process.BaoCaoServiceRef.GIAO_DICH_BASE();
                if (ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BINHKHANH.layGiaTri()))
                    objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.TDVM_KUOCVM_00;
                else
                    objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.TDVM_KUOCVM;
                objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC;

                Presentation.Process.BaoCaoServiceRef.TDVM_KUOCVM objTDVM_KUOCVM = new Presentation.Process.BaoCaoServiceRef.TDVM_KUOCVM();
                objTDVM_KUOCVM.SoKheUoc = TDVMKHEUOC.KUOC_VM.MA_KUOCVM;
                doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                List<Presentation.Process.BaoCaoServiceRef.TDVM_KUOCVM> lstTDVM_KUOCVM = new List<Presentation.Process.BaoCaoServiceRef.TDVM_KUOCVM>();
                lstTDVM_KUOCVM.Add(objTDVM_KUOCVM);
                doiTuongBaoCao.lstTDVM_KUOCVM = lstTDVM_KUOCVM.ToArray();
                doiTuongBaoCao.objTDVM_KUOCVM = objTDVM_KUOCVM;
                
                xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
            }

        }

        private void OnPreviewKheUocNhanNo()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(iDKheUoc))
            {
                LMessage.ShowMessage("M.TinDung.KheUoc.ucKheUocCT.KhongCoThongTinKheUoc", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {

                VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                Presentation.Process.BaoCaoServiceRef.DoiTuongBaoCao doiTuongBaoCao = new Presentation.Process.BaoCaoServiceRef.DoiTuongBaoCao();

                Presentation.Process.BaoCaoServiceRef.GIAO_DICH_BASE objGIAO_DICH_BASE = new Presentation.Process.BaoCaoServiceRef.GIAO_DICH_BASE();
                objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.TDVM_KUOCVM_NHAN_NO_00;
                objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC;

                Presentation.Process.BaoCaoServiceRef.TDVM_KUOCVM objTDVM_KUOCVM = new Presentation.Process.BaoCaoServiceRef.TDVM_KUOCVM();
                objTDVM_KUOCVM.SoKheUoc = TDVMKHEUOC.KUOC_VM.MA_KUOCVM;
                doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                List<Presentation.Process.BaoCaoServiceRef.TDVM_KUOCVM> lstTDVM_KUOCVM = new List<Presentation.Process.BaoCaoServiceRef.TDVM_KUOCVM>();
                lstTDVM_KUOCVM.Add(objTDVM_KUOCVM);
                doiTuongBaoCao.lstTDVM_KUOCVM = lstTDVM_KUOCVM.ToArray();
                doiTuongBaoCao.objTDVM_KUOCVM = objTDVM_KUOCVM;

                xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
            }

        }

        private void OnPreviewKheUocPhanKy()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(iDKheUoc))
            {
                LMessage.ShowMessage("M.TinDung.KheUoc.ucKheUocCT.KhongCoThongTinKheUoc", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {

                VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                Presentation.Process.BaoCaoServiceRef.DoiTuongBaoCao doiTuongBaoCao = new Presentation.Process.BaoCaoServiceRef.DoiTuongBaoCao();

                Presentation.Process.BaoCaoServiceRef.GIAO_DICH_BASE objGIAO_DICH_BASE = new Presentation.Process.BaoCaoServiceRef.GIAO_DICH_BASE();
                objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.TDVM_KUOCVM_PHAN_KY_00;
                objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC;

                Presentation.Process.BaoCaoServiceRef.TDVM_KUOCVM objTDVM_KUOCVM = new Presentation.Process.BaoCaoServiceRef.TDVM_KUOCVM();
                objTDVM_KUOCVM.SoKheUoc = TDVMKHEUOC.KUOC_VM.MA_KUOCVM;
                doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                List<Presentation.Process.BaoCaoServiceRef.TDVM_KUOCVM> lstTDVM_KUOCVM = new List<Presentation.Process.BaoCaoServiceRef.TDVM_KUOCVM>();
                lstTDVM_KUOCVM.Add(objTDVM_KUOCVM);
                doiTuongBaoCao.lstTDVM_KUOCVM = lstTDVM_KUOCVM.ToArray();
                doiTuongBaoCao.objTDVM_KUOCVM = objTDVM_KUOCVM;

                xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
            }

        }

        private void OnPreviewBaoHiem()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(iDKheUoc))
            {
                LMessage.ShowMessage("M.TinDung.KheUoc.ucKheUocCT.KhongCoThongTinKheUoc", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {

                VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                Presentation.Process.BaoCaoServiceRef.DoiTuongBaoCao doiTuongBaoCao = new Presentation.Process.BaoCaoServiceRef.DoiTuongBaoCao();

                Presentation.Process.BaoCaoServiceRef.GIAO_DICH_BASE objGIAO_DICH_BASE = new Presentation.Process.BaoCaoServiceRef.GIAO_DICH_BASE();
                objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.BHTH_PHIEU_YEU_CAU_BH;
                objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC;

                Presentation.Process.BaoCaoServiceRef.BHTH_PHIEU_YEU_CAU_BH objTDVM_KUOCVM = new Presentation.Process.BaoCaoServiceRef.BHTH_PHIEU_YEU_CAU_BH();
                objTDVM_KUOCVM.MaKheUoc = TDVMKHEUOC.KUOC_VM.MA_KUOCVM;
                objTDVM_KUOCVM.P_NgayBaoCao = ClientInformation.NgayLamViecHienTai;
                doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                doiTuongBaoCao.objBHTH_PHIEU_YEU_CAU_BH = objTDVM_KUOCVM;
                xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
            }

        }

        private void AfterOperation()
        {
            TThaiNVu = _TDVMKHEUOC.KUOC_VM.TTHAI_NVU;
        }

        private void SetInfomation()
        {
            if (!LObject.IsNullOrEmpty(TDVMKHEUOC.KUOC_VM))
            {
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TDVMKHEUOC.KUOC_VM.TTHAI_NVU);
                txtSoKheUoc.Text = TDVMKHEUOC.KUOC_VM.MA_KUOCVM;
                if (!LObject.IsNullOrEmpty(TDVMKHEUOC.KUOC_VM.NGAY_CNHAT))
                    teldtNgayCNhat.Value = LDateTime.StringToDate(TDVMKHEUOC.KUOC_VM.NGAY_CNHAT, ApplicationConstant.defaultDateTimeFormat);
                txtNguoiCapNhat.Text = TDVMKHEUOC.KUOC_VM.NGUOI_CNHAT;
                txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(TDVMKHEUOC.KUOC_VM.TTHAI_BGHI);
                TThaiNVu = TDVMKHEUOC.KUOC_VM.TTHAI_NVU;
                iDKheUoc = TDVMKHEUOC.KUOC_VM.ID;
                if (!LObject.IsNullOrEmpty(TDVMKHEUOC.DSACH_KHOACHVM_CTIET) && TDVMKHEUOC.DSACH_KHOACHVM_CTIET.Length>0)
                    lstKeHoach = TDVMKHEUOC.DSACH_KHOACHVM_CTIET.ToList();
                raddgrTinhTrangTraNo.ItemsSource = null;
                raddgrLichTraNo.ItemsSource = null;
                raddgrTinhTrangTraNo.ItemsSource = lstKeHoach;
                raddgrLichTraNo.ItemsSource = lstKeHoach;
                grbLichTraNo.Visibility = Visibility.Collapsed;
                raddgrLichTraNo.Visibility = Visibility.Collapsed;
                expTinhTrangTraNo.IsExpanded = true;
                action = DatabaseConstant.Action.XEM;
                SetEnabledAllControl(false);
                CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC);
                RefeshButton();
            }
            else
                SetDataForm();
        }
        #endregion

        

    }
}

