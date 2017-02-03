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
using System.Data;
using Presentation.Process;
using Presentation.Process.Common;
using PresentationWPF.BaoCao.DungChung;
using Telerik.Windows.Controls;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.TinDungServiceRef;
using PresentationWPF.TinDungTT.PopupNghiepVu;
using System.Collections;
using System.Reflection;

namespace PresentationWPF.TinDungTT.KUOC
{
    /// <summary>
    /// Interaction logic for ucLapKheUocDS.xaml
    /// </summary>
    public partial class ucLapKheUocDS : UserControl
    {

        /// <summary>
        /// Khai báo
        /// </summary>
        #region Khai báo
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

        string TThaiNVu = "";
        public DatabaseConstant.Action action;
        int idKheUoc;
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        public void LayDuLieuTuPopup(List<TDVM_KHE_UOC> lst)
        {
            lstDSachKUOCVM = lst;
        }
        TDVM_KHE_UOC_DSACH objKUOCVMDS = new TDVM_KHE_UOC_DSACH();

        List<TDVM_KHE_UOC> lstDSachKUOCVM = new List<TDVM_KHE_UOC>();
        List<AutoCompleteEntry> lstSourceHinhThucGoc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHinhThucLai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTGianVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum = new List<AutoCompleteEntry>();
        string sKiemTraLSuat = "CO";
        string sSoNgayBDau = "";
        #endregion

        /// <summary>
        /// Khởi tạo
        /// </summary>
        #region Khởi tạo
        public ucLapKheUocDS()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDungTT;component/KUOC/ucLapKheUocDS.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            InitEventHanler();
            ShowControl();
            KhoiTaoGiaTriChoComboBox();
            ClearForm();
        }

        void KhoiTaoGiaTriChoComboBox()
        {
            List<string> lstDieuKien = new List<string>();
            List<string> lstMaChon = new List<string>();
            string sMaTruyVan = "";
            AutoComboBox auto = new AutoComboBox();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.HINH_THUC_TRA_GOC.getValue());
            sMaTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            lstMaChon.Add(BusinessConstant.HINH_THUC_TRA_GOC.DINH_KY.layGiaTri());
            lstMaChon.Add(BusinessConstant.HINH_THUC_TRA_GOC.TRA_SAU.layGiaTri());
            auto.GenAutoComboBox(ref lstSourceHinhThucGoc, ref cmbDinhKyTraGoc, sMaTruyVan, lstDieuKien, BusinessConstant.HINH_THUC_TRA_GOC.DINH_KY.layGiaTri(), lstMaChon);
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.HINH_THUC_TRA_LAI.getValue());
            auto.GenAutoComboBox(ref lstSourceHinhThucLai, ref cmbDinhKyTraLai, sMaTruyVan, lstDieuKien, BusinessConstant.HINH_THUC_TRA_GOC.DINH_KY.layGiaTri());
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DVI_TINH_KY_HAN.getValue());
        }

        void InitEventHanler()
        {
            
            btnSanPham.Click += new RoutedEventHandler(btnSanPham_Click);
            btnLaiSuat.Click += new RoutedEventHandler(btnLaiSuat_Click);
            tlbDetailAdd.Click += new RoutedEventHandler(tlbDetailAdd_Click);
            tlbDetailDelete.Click += new RoutedEventHandler(tlbDetailDelete_Click);
            txtSanPham.LostFocus += new RoutedEventHandler(txtSanPham_LostFocus);
            cmbDinhKyTraGoc.SelectionChanged += new SelectionChangedEventHandler(cmbDinhKyTraGoc_SelectionChanged);
            cmbDinhKyTraLai.SelectionChanged += new SelectionChangedEventHandler(cmbDinhKyTraLai_SelectionChanged);
            tlbLapKeHoach.Click +=new RoutedEventHandler(tlbLapKeHoach_Click);
            tlbXemChiTiet.Click += new RoutedEventHandler(tlbXemChiTiet_Click);
            raddgrTUngCT.CellEditEnded += new EventHandler<GridViewCellEditEndedEventArgs>(raddgrTUngCT_CellEditEnded);
            raddgrTUngCT.CellValidating += new EventHandler<GridViewCellValidatingEventArgs>(raddgrTUngCT_CellValidating);
            cmbNguonVonVay.EditCellEnd += new EventHandler(cmbNguonVonVay_EditCellEnd);
            cmbMucDichVay.EditCellEnd += new EventHandler(cmbMucDichVay_EditCellEnd);
            cmbDViTinhTGian.EditCellEnd += new EventHandler(cmbDViTinhTGian_EditCellEnd);
            teldtNgayPhatVon.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(teldtNgayPhatVon_ValueChanged);
            System.Linq.Expressions.Expression<Func<TDVM_KHE_UOC, decimal?>> expressionKHGoc = kh => (kh.DSACH_KHOACHVM_CTIET.Sum(ds => ds.KH_TRA_GOC.GetValueOrDefault(0)));
            GridViewExpressionColumn column = this.raddgrLichTraNo.Columns["TongTienGoc"] as GridViewExpressionColumn;
            column.Expression = expressionKHGoc;
            System.Linq.Expressions.Expression<Func<TDVM_KHE_UOC, decimal?>> expressionKHLai = kh => (kh.DSACH_KHOACHVM_CTIET.Sum(ds => ds.KH_TRA_LAI.GetValueOrDefault(0)));
            column = this.raddgrLichTraNo.Columns["TongTienLai"] as GridViewExpressionColumn;
            column.Expression = expressionKHLai;
            raddgrTUngCT.KeyboardCommandProvider = new CustomCommandProvider(this.raddgrTUngCT);
        }

        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.TinDungTT.KUOC.ucLapKheUocDS", "RibbonButton");
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
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift);
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
            else if (strTinhNang.Equals("PreviewDanhGiaKH"))
            {
                //OnPreviewDanhGiaKH();
            }
            else if (strTinhNang.Equals("PreviewNhanNo"))
            {
                OnPreviewKheUocNhanNo();
            }
            else if (strTinhNang.Equals("PreviewPhanKy"))
            {
                OnPreviewKheUocPhanKy();
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
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            // Truongnx
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAP_DU_LIEU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ClearForm();
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
            else if (strTinhNang.Equals("PreviewDanhGiaKH"))
            {
                //OnPreviewDanhGiaKH();
            }
            else if (strTinhNang.Equals("PreviewNhanNo"))
            {
                OnPreviewKheUocNhanNo();
            }
            else if (strTinhNang.Equals("PreviewPhanKy"))
            {
                OnPreviewKheUocPhanKy();
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
                    telControl.Value = dtpControl.SelectedDate;
                else
                    throw new System.NullReferenceException("Không tìm thấy control " + sbControl.ToString());
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void ClearForm()
        {
            
            string sNgayTraGoc = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_SONGAY_TOITHIEU_TRAGOC, ClientInformation.MaDonVi);
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
            lblTrangThai.Content = TThaiNVu = "";
            idKheUoc = 0;
            txtSanPham.Text = "";
            txtSanPham.Tag = null;
            txtLaiSuat.Text = "";
            lstDSachKUOCVM = new List<TDVM_KHE_UOC>();
            objKUOCVMDS = new TDVM_KHE_UOC_DSACH();
            raddgrTUngCT.ItemsSource = null;
            raddgrLichTraNo.ItemsSource = null;
            teldtNgayPhatVon.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            dtNgayBatDauTraNo.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat).PlusDays(sSoNgayBDau.StringToInt32());
            dtNgayBatDauTraNoCum.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat).PlusDays(sSoNgayBDau.StringToInt32());
            txtTrangThai.Text = "";
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            teldtNgayCNhat.Value = null;
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            txtNguoiCapNhat.Text = "";
            btnLaiSuat.IsEnabled = true;
            btnSanPham.IsEnabled = true;
            action = DatabaseConstant.Action.THEM;
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC);
        }

        private void SetEnabledAllControl(bool bBool)
        {
            if (TThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()) && action.Equals(DatabaseConstant.Action.SUA))
            {
                
            }
            else
            {
                
            }
        }

        void teldtNgayPhatVon_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            dtNgayBatDauTraNo.Value = teldtNgayPhatVon.Value.GetValueOrDefault().PlusDays(sSoNgayBDau.StringToInt32());
        }

        void tlbDetailDelete_Click(object sender, RoutedEventArgs e)
        {
            if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            foreach(TDVM_KHE_UOC obj in raddgrTUngCT.SelectedItems)
            {
                lstDSachKUOCVM.Remove(obj);
            }
            LoadGridViewDSKheUoc();
        }

        void tlbDetailAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txtSanPham.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblSanPham.Content.ToString());
                btnSanPham.Focus();
                return;
            }
            if (txtLaiSuat.Text.IsNullOrEmptyOrSpace() && sKiemTraLSuat.Equals(BusinessConstant.CoKhong.CO.layGiaTri()))
            {
                CommonFunction.ThongBaoTrong(lblMaLSuat.Content.ToString());
                btnLaiSuat.Focus();
                return;
            }
            lstPopup.Clear();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
            lstDieuKien.Add("HDTDLAPKU");
            lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation("POPUP_DS_HDTD", lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            ucPopup popup = new ucPopup(true, simplePopupResponse, true);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
            win.Content = popup;
            win.ShowDialog();
            List<TDVM_KHE_UOC> lstDSachKUOC = new List<TDVM_KHE_UOC>();
            List<ClientResponseDetail> lstClientResponseDetail = new List<ClientResponseDetail>();
            if (lstPopup.Count > 0)
            {
                lstDSachKUOCVM = raddgrTUngCT.ItemsSource as List<TDVM_KHE_UOC>;
                foreach(DataRow dr in lstPopup)
                {
                    TDVM_KHE_UOC objKUOC = new TDVM_KHE_UOC();
                    objKUOC.KUOC_VM = new TD_KUOCVM();
                    objKUOC.KUOC_VM.ID_HDTDVM = Convert.ToInt32(dr["ID"]);
                    objKUOC.KUOC_VM.MA_HDTDVM = dr["MA_HDTDVM"].ToString();
                    objKUOC.KUOC_VM.MA_DVI_QLY = ClientInformation.MaDonVi;
                    objKUOC.KUOC_VM.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    objKUOC.KUOC_VM.NGAY_LAP_KUOC = ClientInformation.NgayLamViecHienTai;
                    objKUOC.KUOC_VM.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    objKUOC.KUOC_VM.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    lstDSachKUOC.Add(objKUOC);
                }
                objKUOCVMDS.DSACH_KHE_UOC = lstDSachKUOC.ToArray();
                if (new TinDungProcess().DanhSachKheUocViMo(ref objKUOCVMDS, DatabaseConstant.Action.LAY_LAI, ref lstClientResponseDetail) > 0)
                {
                    if (LObject.IsNullOrEmpty(lstDSachKUOCVM)) lstDSachKUOCVM = new List<TDVM_KHE_UOC>();
                    lstDSachKUOC = objKUOCVMDS.DSACH_KHE_UOC.ToList();
                    if (lstDSachKUOC[0].VONG_VAY.TCHAT_GOC_VAY.Equals(BusinessConstant.TINH_CHAT_VONG_VAY.THAY_DOI.layGiaTri()))
                        lstDSachKUOC.ForEach(f => f.KUOC_VM.SO_TIEN_GIAI_NGAN = 0);
                    lstDSachKUOCVM.AddRange(lstDSachKUOC);
                }
                LoadGridViewDSKheUoc();
            }
        }

        void btnLaiSuat_Click(object sender, RoutedEventArgs e)
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
                txtLaiSuat.Text = objKUOCVMDS.MA_LAI_SUAT = lstPopup[0][2].ToString();
                txtLaiSuat.Tag = lstPopup[0][1];
                lblTenLaiSuat.Content = lstPopup[0][3];
            }
        }

        void btnSanPham_Click(object sender, RoutedEventArgs e)
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
                txtSanPham.Text = objKUOCVMDS.MA_SAN_PHAM = lstPopup[0]["MA_SAN_PHAM"].ToString();
                txtSanPham.Tag = Convert.ToInt32(lstPopup[0]["ID"]);
                lblTenSanPham.Content = lstPopup[0]["TEN_SAN_PHAM"].ToString();
                txtLaiSuat.Text = objKUOCVMDS.MA_LAI_SUAT = lstPopup[0]["MA_LSUAT"].ToString();
                lblTenLaiSuat.Content = lstPopup[0]["TEN_LSUAT"].ToString();
            }
        }

        void txtSanPham_LostFocus(object sender, RoutedEventArgs e)
        {
            
        }

        void cmbDinhKyTraLai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoCompleteEntry au = lstSourceHinhThucLai.ElementAt(cmbDinhKyTraLai.SelectedIndex);
            objKUOCVMDS.HINH_THUC_TRA_LAI = au.KeywordStrings[0];
        }

        void cmbDinhKyTraGoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoCompleteEntry au = lstSourceHinhThucGoc.ElementAt(cmbDinhKyTraGoc.SelectedIndex);
            objKUOCVMDS.HINH_THUC_TRA_GOC = au.KeywordStrings[0];
        }

        void tlbXemChiTiet_Click(object sender, RoutedEventArgs e)
        {
            TDVM_KHE_UOC objKUOC = raddgrLichTraNo.SelectedItem as TDVM_KHE_UOC;
            ucPopopKeHoachCT popup = new ucPopopKeHoachCT(ref lstDSachKUOCVM, objKUOC,objKUOCVMDS);
            Window win = new Window();
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Title = "Kế hoạch chi tiết";
            win.Content = popup;
            popup.LayGiaTri = new ucPopopKeHoachCT.LayGiaTriLapLich(LayDuLieuTuPopup);
            win.ShowDialog();
            LoadGridViewDSKheUoc();
        }

        void LockControl()
        {
            if(raddgrTUngCT.Items.Count>0)
            {
                btnSanPham.IsEnabled = false;
                btnLaiSuat.IsEnabled = false;
                ColumnsComboBoxv1 columnTGianVi = this.raddgrTUngCT.Columns["TGIAN_DVI_TINH"] as ColumnsComboBoxv1;
                GridViewDataColumn columnTGian = this.raddgrTUngCT.Columns["TGIAN_VAY"] as GridViewDataColumn;
                GridViewDataColumn columnSoTien = this.raddgrTUngCT.Columns["SO_TIEN_GIAI_NGAN"] as GridViewDataColumn;
                if (!LObject.IsNullOrEmpty(lstDSachKUOCVM[0].VONG_VAY))
                {
                    columnTGianVi.IsReadOnly = true;
                    if (lstDSachKUOCVM[0].VONG_VAY.TCHAT_KY_HAN.Equals(BusinessConstant.TINH_CHAT_VONG_VAY.CO_DINH.layGiaTri()))
                        columnTGian.IsReadOnly = true;
                    else
                        columnTGian.IsReadOnly = false;
                    if (lstDSachKUOCVM[0].VONG_VAY.TCHAT_GOC_VAY.Equals(BusinessConstant.TINH_CHAT_VONG_VAY.CO_DINH.layGiaTri()))
                        columnSoTien.IsReadOnly = true;
                    else
                        columnSoTien.IsReadOnly = false;
                }
                else
                {
                    columnTGianVi.IsReadOnly = false;
                    columnSoTien.IsReadOnly = false;
                    columnTGian.IsReadOnly = false;
                }
            }
            else
            {
                btnSanPham.IsEnabled = true;
                btnLaiSuat.IsEnabled = true;
                raddgrLichTraNo.ItemsSource = null;
            }
        }

        void raddgrTUngCT_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            if(!e.Cell.Column.UniqueName.IsNullOrEmptyOrSpace() && e.Cell.Column.UniqueName.Equals("SO_TIEN_GIAI_NGAN") && Convert.ToDecimal(e.OldData)!=Convert.ToDecimal(e.NewData))
            {
                TDVM_KHE_UOC objKheUocVm = e.Cell.ParentRow.Item as TDVM_KHE_UOC;
                int index = lstDSachKUOCVM.IndexOf(objKheUocVm);
                objKheUocVm.DSACH_KHOACHVM_CTIET = (new List<TD_KHOACHVM_CT>()).ToArray();
                lstDSachKUOCVM[index] = objKheUocVm;
            }
        }

        void raddgrTUngCT_CellValidating(object sender, GridViewCellValidatingEventArgs e)
        {

        }


        void cmbMucDichVay_EditCellEnd(object sender, EventArgs e)
        {
            TDVM_KHE_UOC objKUOCVM = cmbMucDichVay.cellEdit.ParentRow.Item as TDVM_KHE_UOC;
            int id = lstDSachKUOCVM.IndexOf(objKUOCVM);
            lstDSachKUOCVM[lstDSachKUOCVM.IndexOf(objKUOCVM)].KUOC_VM.MUC_DICH_VAY = cmbMucDichVay.GiaTri;
        }

        void cmbNguonVonVay_EditCellEnd(object sender, EventArgs e)
        {
            TDVM_KHE_UOC objKUOCVM = cmbNguonVonVay.cellEdit.ParentRow.Item as TDVM_KHE_UOC;
            lstDSachKUOCVM[lstDSachKUOCVM.IndexOf(objKUOCVM)].KUOC_VM.NV_LOAI_NVON = cmbNguonVonVay.GiaTri;
        }

        void cmbDViTinhTGian_EditCellEnd(object sender, EventArgs e)
        {
            TDVM_KHE_UOC objKUOCVM = cmbNguonVonVay.cellEdit.ParentRow.Item as TDVM_KHE_UOC;
            lstDSachKUOCVM[lstDSachKUOCVM.IndexOf(objKUOCVM)].KUOC_VM.TGIAN_VAY_DVI_TINH = cmbNguonVonVay.GiaTri;
        }

        void LockControl(bool bBool)
        {
            grbThongTinChung.IsEnabled = bBool;
            grbDanhSachKUoc.IsEnabled = bBool;
            grbKeHoach.IsEnabled = bBool;
            tlbDetailAdd.IsEnabled = bBool;
            raddgrTUngCT.IsReadOnly = !bBool;
        }
        #endregion

        /// <summary>
        /// Xu ly nghiep vu
        /// </summary>
        /// 
        #region Xu ly nghiep vu

        void GetDataForm(BusinessConstant.TrangThaiBanGhi bghi, BusinessConstant.TrangThaiNghiepVu nghiepvu)
        {

            try
            {
                string TrangThaiKUOC = BusinessConstant.TRANG_THAI_TAT_TOAN.CHUA_TAT_TOAN.layGiaTri();
                string TrangTraiBanGhi = bghi.layGiaTri();
                string TrangTraiNghiepVu = nghiepvu.layGiaTri();
                string NgayGiaiNgan = LDateTime.DateToString(teldtNgayPhatVon.Value.GetValueOrDefault(),ApplicationConstant.defaultDateTimeFormat);
                string NgayLapKUOC = ClientInformation.NgayLamViecHienTai;
                string LoaiLSuat = "";
                if(rdbCoDinh.IsChecked.GetValueOrDefault())
                    LoaiLSuat = BusinessConstant.LOAI_LAI_SUAT.CO_DINH.layGiaTri();
                if(rdbThaNoi.IsChecked.GetValueOrDefault())
                    LoaiLSuat = BusinessConstant.LOAI_LAI_SUAT.THA_NOI.layGiaTri();
                AutoCompleteEntry auHinhThucGoc = lstSourceHinhThucGoc.ElementAt(cmbDinhKyTraGoc.SelectedIndex);
                AutoCompleteEntry auHinhThucLai = lstSourceHinhThucLai.ElementAt(cmbDinhKyTraLai.SelectedIndex);
                objKUOCVMDS.DSACH_KHE_UOC = lstDSachKUOCVM.ToArray();
                objKUOCVMDS.HINH_THUC_TRA_GOC = auHinhThucGoc.KeywordStrings[0];
                objKUOCVMDS.HINH_THUC_TRA_LAI = auHinhThucLai.KeywordStrings[0];
                lstDSachKUOCVM = raddgrTUngCT.ItemsSource as List<TDVM_KHE_UOC>;
                lstDSachKUOCVM.ForEach(e => { e.KUOC_VM.TTHAI_KUOC = TrangThaiKUOC; e.KUOC_VM.TTHAI_BGHI = TrangTraiBanGhi;
                e.KUOC_VM.TTHAI_NVU = TrangTraiNghiepVu; e.KUOC_VM.NGAY_GIAI_NGAN = NgayGiaiNgan; e.KUOC_VM.SO_TAI_KHOAN = "";
                e.KUOC_VM.LOAI_LSUAT = LoaiLSuat; e.KUOC_VM.LAI_SUAT_CLN = 0; e.KUOC_VM.LAI_SUAT_QH = e.KUOC_VM.LAI_SUAT * e.LSUAT_QH;
                e.KUOC_VM.SO_DU = !e.KUOC_VM.SO_DU.IsNullOrEmpty() ? e.KUOC_VM.SO_DU : 0; e.KUOC_VM.NHOM_NO_HIEN_TAI = !e.KUOC_VM.NHOM_NO_HIEN_TAI.IsNullOrEmptyOrSpace() ?
                e.KUOC_VM.NHOM_NO_HIEN_TAI : BusinessConstant.NHOM_NO.NHOM1.layGiaTri(); e.KUOC_VM.LAI_TREO = !e.KUOC_VM.LAI_TREO.IsNullOrEmpty() ? e.KUOC_VM.LAI_TREO : 0;
                e.KUOC_VM.LAI_DU_THU = !e.KUOC_VM.LAI_DU_THU.IsNullOrEmpty() ? e.KUOC_VM.LAI_DU_THU : 0; e.KUOC_VM.LAI_DU_THU_NBANG = !e.KUOC_VM.LAI_DU_THU_NBANG.IsNullOrEmpty() ? e.KUOC_VM.LAI_DU_THU_NBANG : 0;
                e.KUOC_VM.LAI_PHAI_THU = !e.KUOC_VM.LAI_PHAI_THU.IsNullOrEmpty() ? e.KUOC_VM.LAI_PHAI_THU : 0; e.KUOC_VM.LAI_DA_THU = !e.KUOC_VM.LAI_DA_THU.IsNullOrEmpty() ? e.KUOC_VM.LAI_DA_THU : 0;
                e.KUOC_VM.GOC_DA_THU = !e.KUOC_VM.GOC_DA_THU.IsNullOrEmpty() ? e.KUOC_VM.GOC_DA_THU : 0; e.KUOC_VM.SO_TIEN_TLDP = !e.KUOC_VM.SO_TIEN_TLDP.IsNullOrEmpty() ? e.KUOC_VM.SO_TIEN_TLDP : 0;
                e.KUOC_VM.LAI_DA_XUAT_NB = !e.KUOC_VM.LAI_DA_XUAT_NB.IsNullOrEmpty() ? e.KUOC_VM.LAI_DA_XUAT_NB : 0;
                e.KUOC_VM.KHOACH_NGAY_LAP = LDateTime.DateToString((DateTime)dtNgayBatDauTraNo.Value, ApplicationConstant.defaultDateTimeFormat);
                e.KUOC_VM.KHOACH_NGAY_LAP_CUM = LDateTime.DateToString((DateTime)dtNgayBatDauTraNoCum.Value, ApplicationConstant.defaultDateTimeFormat);
                e.NGAY_BD_TRA = LDateTime.DateToString((DateTime)dtNgayBatDauTraNo.Value, ApplicationConstant.defaultDateTimeFormat);

                });
                objKUOCVMDS.NGAY_GIAI_NGAN = NgayGiaiNgan;
                objKUOCVMDS.DSACH_KHE_UOC = lstDSachKUOCVM.ToArray();
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
                DataSet ds=null;
                SetDataTabThongTinChung(ds);
                SetDataTabLichTraNo(ds);
                SetDataTabTSDB(ds);
                SetDataTabThongTinKiemSoat(ds);
                if (!action.Equals(DatabaseConstant.Action.SUA))
                {
                    CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC);
                    SetEnabledAllControl(false);
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
                
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }

        }

        private void tlbLapKeHoach_Click(object sender, RoutedEventArgs e)
        {
            List<ClientResponseDetail> lstClientResponseDetail = new List<ClientResponseDetail>();
            try
            {
                if (!LObject.IsNullOrEmpty(sender))
                {
                    if (!VadidateDieuKienKeHoach())
                        return;
                    AutoCompleteEntry auHinhThucGoc = lstSourceHinhThucGoc.ElementAt(cmbDinhKyTraGoc.SelectedIndex);
                    AutoCompleteEntry auHinhThucLai = lstSourceHinhThucLai.ElementAt(cmbDinhKyTraLai.SelectedIndex);
                    lstDSachKUOCVM.ForEach(f => f.NGAY_BD_TRA = LDateTime.DateToString(dtNgayBatDauTraNo.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat));
                    objKUOCVMDS.DSACH_KHE_UOC = lstDSachKUOCVM.ToArray();
                    objKUOCVMDS.HINH_THUC_TRA_GOC = auHinhThucGoc.KeywordStrings[0];
                    objKUOCVMDS.HINH_THUC_TRA_LAI = auHinhThucLai.KeywordStrings[0];
                    objKUOCVMDS.NGAY_GIAI_NGAN = LDateTime.DateToString(teldtNgayPhatVon.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
                    int iret = new TinDungProcess().DanhSachKheUocViMo(ref objKUOCVMDS, DatabaseConstant.Action.TINH_TOAN_LICH_TRA_NO, ref lstClientResponseDetail);
                    lstDSachKUOCVM = objKUOCVMDS.DSACH_KHE_UOC.ToList();
                    LoadGridViewDSKheUoc();
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
            
            try
            {
                if(txtSanPham.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblSanPham.Content.ToString());
                    btnSanPham.Focus();
                    bReturn = false;
                }
                else if (lstDSachKUOCVM.Count<1)
                {
                    CommonFunction.ThongBaoTrong(lblDSachKheUoc.Content.ToString());
                    tlbDetailAdd.Focus();
                    bReturn = false;
                }
                else if (lstDSachKUOCVM.Where(e=>e.KUOC_VM.NV_LOAI_NVON.IsNullOrEmptyOrSpace()).Count()>0)
                {
                    CommonFunction.ThongBaoTrong("Nguồn vốn cho vay:");
                    bReturn = false;
                }
                else if (lstDSachKUOCVM.Where(e => e.KUOC_VM.MUC_DICH_VAY.IsNullOrEmptyOrSpace()).Count() > 0)
                {
                    CommonFunction.ThongBaoTrong("Mục đích vay:");
                    bReturn = false;
                }
                else if (lstDSachKUOCVM.Where(e => e.KUOC_VM.SO_TIEN_GIAI_NGAN<=0).Count() > 0)
                {
                    CommonFunction.ThongBaoTrong("Số tiền giải ngân:");
                    bReturn = false;
                }
                else if (dtNgayBatDauTraNoCum.Value.GetValueOrDefault() < dtNgayBatDauTraNo.Value.GetValueOrDefault())
                {
                    LMessage.ShowMessage("Ngày bắt đầu trả nợ cụm không hợp lệ", LMessage.MessageBoxType.Warning);
                    bReturn = false;
                }
                else if (lstDSachKUOCVM.Where(e=>e.DSACH_KHOACHVM == null || e.DSACH_KHOACHVM_CTIET == null).ToList().Count > 0)
                {
                    string sMessage = LLanguage.SearchResourceByKey("M.TinDungTT.ucKheUocCT.KeHoachKhongDuocTrong") + " " + LLanguage.SearchResourceByKey("M.TinDungTT.ucKheUocCT.HinhThucTraLai") + " " + cmbDinhKyTraLai.Text + " " + LLanguage.SearchResourceByKey("M.TinDungTT.ucKheUocCT.HinhThucTraGoc") + " " + cmbDinhKyTraGoc.Text + "?";
                    if (LMessage.ShowMessage(sMessage, LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                        bReturn = true;
                    else
                    {
                        tlbLapKeHoach.Focus();
                        bReturn = false;
                    }
                }
                else if (txtLaiSuat.Text.IsNullOrEmptyOrSpace())
                {
                    
                    if (!LObject.IsNullOrEmpty(sKiemTraLSuat) && sKiemTraLSuat.Equals(BusinessConstant.CoKhong.CO.layGiaTri()))
                    {
                        CommonFunction.ThongBaoTrong(lblMaLSuat.Content.ToString());
                        btnLaiSuat.Focus();
                        bReturn = false;
                    }
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
            LockControl(true);
            action = DatabaseConstant.Action.SUA;
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC);
        }

        void BeforeSave(BusinessConstant.TrangThaiNghiepVu nghiepvu, BusinessConstant.TrangThaiBanGhi bghi)
        {
            Cursor = Cursors.Wait;
            
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
                if (idKheUoc == 0)
                    iret = new TinDungProcess().DanhSachKheUocViMo(ref objKUOCVMDS, DatabaseConstant.Action.THEM, ref lstResponseDetail);
                else
                    iret = new TinDungProcess().DanhSachKheUocViMo(ref objKUOCVMDS, DatabaseConstant.Action.SUA, ref lstResponseDetail);
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
                if (!cbMultiAdd.IsChecked.GetValueOrDefault(false))
                {
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
                if (idKheUoc == 0)
                    return;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId = lstDSachKUOCVM.Select(f => f.KUOC_VM.ID).ToList();
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
                lstId = lstDSachKUOCVM.Select(f => f.KUOC_VM.ID).ToList();
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
            iret = iret = new TinDungProcess().DanhSachKheUocViMo(ref objKUOCVMDS, DatabaseConstant.Action.XOA, ref lstResponseDetail);
            AfterDelete(lstResponseDetail, iret);
        }
        void AfterDelete(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            
            Cursor = Cursors.Arrow;
            if (iret == 0) ;
            else
                CommonFunction.CloseUserControl(this);
        }

        void BeforeApprove()
        {
            try
            {
                if (idKheUoc == 0)
                    return;
                Cursor = Cursors.Wait;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId = lstDSachKUOCVM.Select(f => f.KUOC_VM.ID).ToList();
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
                lstId = lstDSachKUOCVM.Select(f => f.KUOC_VM.ID).ToList();
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
            iret = iret = new TinDungProcess().DanhSachKheUocViMo(ref objKUOCVMDS, DatabaseConstant.Action.DUYET, ref lstResponseDetail);
            AfterApprove(lstResponseDetail, iret);
        }
        void AfterApprove(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId = lstDSachKUOCVM.Select(f => f.KUOC_VM.ID).ToList();
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.DUYET,
            lstId);
            
            SetInfomation();
            Cursor = Cursors.Arrow;
        }

        void BeforeRefuse()
        {
            try
            {
                if (idKheUoc == 0)
                    return;
                Cursor = Cursors.Wait;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId = lstDSachKUOCVM.Select(f => f.KUOC_VM.ID).ToList();
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
                lstId = lstDSachKUOCVM.Select(f => f.KUOC_VM.ID).ToList();
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
            iret = iret = new TinDungProcess().DanhSachKheUocViMo(ref objKUOCVMDS, DatabaseConstant.Action.TU_CHOI_DUYET, ref lstResponseDetail);
            AfterRefuse(lstResponseDetail, iret);
        }
        void AfterRefuse(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId = lstDSachKUOCVM.Select(f => f.KUOC_VM.ID).ToList();
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.TU_CHOI_DUYET,
            lstId);
            
            Cursor = Cursors.Arrow;
            SetInfomation();
        }

        void BeforeCancel()
        {
            try
            {
                if (idKheUoc == 0)
                    return;
                Cursor = Cursors.Wait;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId = lstDSachKUOCVM.Select(f => f.KUOC_VM.ID).ToList();
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);
                
                OnCancel();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId = lstDSachKUOCVM.Select(f => f.KUOC_VM.ID).ToList();
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.TU_CHOI_DUYET,
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
            iret = iret = new TinDungProcess().DanhSachKheUocViMo(ref objKUOCVMDS, DatabaseConstant.Action.THOAI_DUYET, ref lstResponseDetail);
            AfterCancel(lstResponseDetail, iret);
        }
        void AfterCancel(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId = lstDSachKUOCVM.Select(f => f.KUOC_VM.ID).ToList();
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.TU_CHOI_DUYET,
            lstId);
            
            Cursor = Cursors.Arrow;
            SetInfomation();
        }

        private bool VadidateDieuKienKeHoach()
        {
            List<ClientResponseDetail> lstClientResponseDetail = new List<ClientResponseDetail>();
            bool bReturn = true;
            try
            {
                
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
            Presentation.Process.BaoCaoServiceRef.DoiTuongBaoCao doiTuongBaoCao = new Presentation.Process.BaoCaoServiceRef.DoiTuongBaoCao();
            List<Presentation.Process.BaoCaoServiceRef.TDVM_KUOCVM> lstTDVM_KUOCVM = new List<Presentation.Process.BaoCaoServiceRef.TDVM_KUOCVM>();
            try
            {
                // Cảnh báo khi không có dữ liệu
                if (LObject.IsNullOrEmpty(idKheUoc))
                {
                    LMessage.ShowMessage("M.TinDungTT.KheUoc.ucKheUocCT.KhongCoThongTinKheUoc", LMessage.MessageBoxType.Warning);
                    return;
                }
                else
                {
                    foreach (TDVM_KHE_UOC kheUoc in raddgrTUngCT.SelectedItems)
                    {
                        Presentation.Process.BaoCaoServiceRef.TDVM_KUOCVM objTDVM_KUOCVM = new Presentation.Process.BaoCaoServiceRef.TDVM_KUOCVM();
                        objTDVM_KUOCVM.SoKheUoc = kheUoc.KUOC_VM.MA_KUOCVM;
                        lstTDVM_KUOCVM.Add(objTDVM_KUOCVM);
                        doiTuongBaoCao.objTDVM_KUOCVM = objTDVM_KUOCVM;
                    }
                    if (lstTDVM_KUOCVM.Count > 0)
                    {
                        VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                        Presentation.Process.BaoCaoServiceRef.GIAO_DICH_BASE objGIAO_DICH_BASE = new Presentation.Process.BaoCaoServiceRef.GIAO_DICH_BASE();
                        if (ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BINHKHANH.layGiaTri()))
                            objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.TDVM_KUOCVM_00;
                        else
                            objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.TDVM_KUOCVM;
                        objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC;
                        doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                        doiTuongBaoCao.lstTDVM_KUOCVM = lstTDVM_KUOCVM.ToArray();
                        xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
                    }
                    else
                    {
                        LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                    }
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
            }
            finally
            {
                doiTuongBaoCao = null;
                lstTDVM_KUOCVM = null;
            }

        }

        private void OnPreviewKheUocNhanNo()
        {
            // Cảnh báo khi không có dữ liệu
            Presentation.Process.BaoCaoServiceRef.DoiTuongBaoCao doiTuongBaoCao = new Presentation.Process.BaoCaoServiceRef.DoiTuongBaoCao();
            List<Presentation.Process.BaoCaoServiceRef.TDVM_KUOCVM> lstTDVM_KUOCVM = new List<Presentation.Process.BaoCaoServiceRef.TDVM_KUOCVM>();
            try
            {
                // Cảnh báo khi không có dữ liệu
                if (LObject.IsNullOrEmpty(idKheUoc))
                {
                    LMessage.ShowMessage("M.TinDungTT.KheUoc.ucKheUocCT.KhongCoThongTinKheUoc", LMessage.MessageBoxType.Warning);
                    return;
                }
                else
                {
                    foreach (TDVM_KHE_UOC kheUoc in raddgrTUngCT.SelectedItems)
                    {
                        Presentation.Process.BaoCaoServiceRef.TDVM_KUOCVM objTDVM_KUOCVM = new Presentation.Process.BaoCaoServiceRef.TDVM_KUOCVM();
                        objTDVM_KUOCVM.SoKheUoc = kheUoc.KUOC_VM.MA_KUOCVM;
                        lstTDVM_KUOCVM.Add(objTDVM_KUOCVM);
                        doiTuongBaoCao.objTDVM_KUOCVM = objTDVM_KUOCVM;
                    }
                    if (lstTDVM_KUOCVM.Count > 0)
                    {
                        VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                        Presentation.Process.BaoCaoServiceRef.GIAO_DICH_BASE objGIAO_DICH_BASE = new Presentation.Process.BaoCaoServiceRef.GIAO_DICH_BASE();
                        objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.TDVM_KUOCVM_NHAN_NO_00;
                        objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC;
                        doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                        doiTuongBaoCao.lstTDVM_KUOCVM = lstTDVM_KUOCVM.ToArray();
                        xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
                    }
                    else
                    {
                        LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                    }
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
            }
            finally
            {
                doiTuongBaoCao = null;
                lstTDVM_KUOCVM = null;
            }
        }

        private void OnPreviewKheUocPhanKy()
        {
            // Cảnh báo khi không có dữ liệu
            Presentation.Process.BaoCaoServiceRef.DoiTuongBaoCao doiTuongBaoCao = new Presentation.Process.BaoCaoServiceRef.DoiTuongBaoCao();
            List<Presentation.Process.BaoCaoServiceRef.TDVM_KUOCVM> lstTDVM_KUOCVM = new List<Presentation.Process.BaoCaoServiceRef.TDVM_KUOCVM>();
            try
            {
                // Cảnh báo khi không có dữ liệu
                if (LObject.IsNullOrEmpty(idKheUoc))
                {
                    LMessage.ShowMessage("M.TinDungTT.KheUoc.ucKheUocCT.KhongCoThongTinKheUoc", LMessage.MessageBoxType.Warning);
                    return;
                }
                else
                {
                    foreach (TDVM_KHE_UOC kheUoc in raddgrTUngCT.SelectedItems)
                    {
                        Presentation.Process.BaoCaoServiceRef.TDVM_KUOCVM objTDVM_KUOCVM = new Presentation.Process.BaoCaoServiceRef.TDVM_KUOCVM();
                        objTDVM_KUOCVM.SoKheUoc = kheUoc.KUOC_VM.MA_KUOCVM;
                        lstTDVM_KUOCVM.Add(objTDVM_KUOCVM);
                        doiTuongBaoCao.objTDVM_KUOCVM = objTDVM_KUOCVM;
                    }
                    if (lstTDVM_KUOCVM.Count > 0)
                    {
                        VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                        Presentation.Process.BaoCaoServiceRef.GIAO_DICH_BASE objGIAO_DICH_BASE = new Presentation.Process.BaoCaoServiceRef.GIAO_DICH_BASE();
                        objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.TDVM_KUOCVM_PHAN_KY_00;
                        objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC;
                        doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                        doiTuongBaoCao.lstTDVM_KUOCVM = lstTDVM_KUOCVM.ToArray();
                        xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
                    }
                    else
                    {
                        LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                    }
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
            }
            finally
            {
                doiTuongBaoCao = null;
                lstTDVM_KUOCVM = null;
            }
        }

        private void AfterOperation()
        {
            
        }

        private void SetInfomation()
        {
            lstDSachKUOCVM = objKUOCVMDS.DSACH_KHE_UOC.Where(e=>e.KUOC_VM.ID>0).ToList();
            objKUOCVMDS.DSACH_KHE_UOC = lstDSachKUOCVM.ToArray();
            if (lstDSachKUOCVM.Count>0)
            {
                idKheUoc = lstDSachKUOCVM[0].KUOC_VM.ID;
                TThaiNVu = lstDSachKUOCVM[0].KUOC_VM.TTHAI_NVU;
                LockControl();
            }
            else
            {
                idKheUoc = 0;
                TThaiNVu = "";
            }
            lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
            LoadGridViewDSKheUoc();
            action = DatabaseConstant.Action.XEM;
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC);
            tlbDetailAdd.IsEnabled = false;
            tlbDetailDelete.IsEnabled = false;
        }

        private void LoadGridViewDSKheUoc()
        {
            raddgrTUngCT.ItemsSource = lstDSachKUOCVM;
            raddgrLichTraNo.ItemsSource = lstDSachKUOCVM;
            raddgrLichTraNo.Rebind();
            raddgrTUngCT.Rebind();
            LockControl();
        }
        #endregion
    }
}
