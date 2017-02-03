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
    /// Interaction logic for ucLapKheUocDS_01.xaml
    /// </summary>
    public partial class ucLapKheUocDS_01 : UserControl
    {
        bool isLoad = false;
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
        int idDonVi;
        string nguonVon = "";
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        public TDVM_KHE_UOC_DSACH objKUOCVMDS = new TDVM_KHE_UOC_DSACH();
        public void LayThongTinLapLich(List<TDVM_KHE_UOC> _lstKUOCVMDS)
        {
            lstDSachKUOCVM = _lstKUOCVMDS;
        }
        bool GenDKien = true;
        public event EventHandler OnSavingCompleted;

        List<TDVM_KHE_UOC> lstDSachKUOCVM = new List<TDVM_KHE_UOC>();
        List<AutoCompleteEntry> lstSourceHinhThucGoc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHinhThucLai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTGianVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKhuVuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNhom = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceGiaoVon = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceBDauTra = new List<AutoCompleteEntry>();
        string sKiemTraLSuat = "CO";
        string sSoNgayBDau = "";
        List<int> lstID = new List<int>();
        int idKhuVuc = 0;
        int idCum = 0;
        int idNhom = 0;
        #endregion

        /// <summary>
        /// Khởi tạo
        /// </summary>
        #region Khởi tạo
        public ucLapKheUocDS_01()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDungTT;component/KUOC/ucLapKheUocDS_01.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            ShowControl();
            KhoiTaoGiaTriChoComboBox();
            InitEventHanler();
            ClearForm();

            if (ClientInformation.Company.Equals("BANTAYVANG"))
            {
                tlbPreviewKheUoc.Visibility = Visibility.Collapsed;
                tlbPreviewDanhGiaKH.Visibility = Visibility.Collapsed;
                tlbPreviewBaoHiem.Visibility = Visibility.Collapsed;
                tlbPreviewNhanNo.Visibility = Visibility.Collapsed;
                tlbPreviewPhanKy.Visibility = Visibility.Collapsed;

                tlbPreviewHopDongVayVon.Visibility = Visibility.Visible;
                tlbPreviewPhuLucHopDong.Visibility = Visibility.Visible;
            }
            else if (ClientInformation.Company.Equals("M7MFI"))
            {
                tlbPreviewKheUoc.Visibility = Visibility.Visible;
                tlbPreviewDanhGiaKH.Visibility = Visibility.Visible;
                tlbPreviewBaoHiem.Visibility = Visibility.Visible;
                tlbPreviewNhanNo.Visibility = Visibility.Visible;
                tlbPreviewPhanKy.Visibility = Visibility.Visible;

                tlbPreviewHopDongVayVon.Visibility = Visibility.Collapsed;
                tlbPreviewPhuLucHopDong.Visibility = Visibility.Collapsed;
            }
            else
            {
                tlbPreviewKheUoc.Visibility = Visibility.Collapsed;
                tlbPreviewDanhGiaKH.Visibility = Visibility.Collapsed;
                tlbPreviewBaoHiem.Visibility = Visibility.Collapsed;
                tlbPreviewNhanNo.Visibility = Visibility.Collapsed;
                tlbPreviewPhanKy.Visibility = Visibility.Collapsed;

                tlbPreviewHopDongVayVon.Visibility = Visibility.Visible;
                tlbPreviewPhuLucHopDong.Visibility = Visibility.Visible;
            }
            nguonVon = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_NGUONVON_MACDINH, ClientInformation.MaDonVi);
        }

        void KhoiTaoGiaTriChoComboBox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.HINH_THUC_TRA_GOC.getValue());
            auto.GenAutoComboBox(ref lstSourceHinhThucGoc, ref cmbHinhThucTraGoc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien,BusinessConstant.HINH_THUC_TRA_GOC.DINH_KY.layGiaTri());
            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.HINH_THUC_TRA_LAI.getValue());
            auto.GenAutoComboBox(ref lstSourceHinhThucLai, ref cmbHinhThucTraLai, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, BusinessConstant.HINH_THUC_TRA_LAI.DINH_KY.layGiaTri());
            idDonVi = ClientInformation.IdDonViGiaoDich;
            LoadComboBoxKhuVuc();
            LoadComboBoxCum();
            LoadComboBoxNhom();
        }

        void InitEventHanler()
        {
            cmbKhuVuc.SelectionChanged += new SelectionChangedEventHandler(cmbKhuVuc_SelectionChanged);
            cmbCum.SelectionChanged += new SelectionChangedEventHandler(cmbCum_SelectionChanged);
            cmbNhom.SelectionChanged += new SelectionChangedEventHandler(cmbNhom_SelectionChanged);
            //teldtThangGiaoVon.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(teldtThangGiaoVon_ValueChanged);
            //teldtThangBDTra.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(teldtThangBDTra_ValueChanged);
            dtpThangGiaoVon.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(dtpThangGiaoVon_SelectedDateChanged);
            dtpThangBDTra.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(dtpThangBDTra_SelectedDateChanged);
            tlbDetailAdd.Click +=new RoutedEventHandler(tlbDetailAdd_Click);
            tlbDetailDelete.Click +=new RoutedEventHandler(tlbDetailDelete_Click);
            tlbLapKeHoach.Click +=new RoutedEventHandler(tlbLapKeHoach_Click);
            tlbXemChiTiet.Click +=new RoutedEventHandler(tlbXemChiTiet_Click);
            cmbHinhThucTraGoc.SelectionChanged += new SelectionChangedEventHandler(cmbHinhThucTraGoc_SelectionChanged);
            cmbHinhThucTraLai.SelectionChanged += new SelectionChangedEventHandler(cmbHinhThucTraLai_SelectionChanged);
            cmbThangBDTra.SelectionChanged += new SelectionChangedEventHandler(cmbThangBDTra_SelectionChanged);
            cmbDotGiaoVon.SelectionChanged += new SelectionChangedEventHandler(cmbDotGiaoVon_SelectionChanged);
            System.Linq.Expressions.Expression<Func<TDVM_KHE_UOC, decimal?>> expressionKHGoc = kh => (kh.DSACH_KHOACHVM_CTIET.Sum(ds => ds.KH_TRA_GOC.GetValueOrDefault(0)));
            GridViewExpressionColumn column = this.raddgrLichTraNo.Columns["TongTienGoc"] as GridViewExpressionColumn;
            column.Expression = expressionKHGoc;
            System.Linq.Expressions.Expression<Func<TDVM_KHE_UOC, decimal?>> expressionKHLai = kh => (kh.DSACH_KHOACHVM_CTIET.Sum(ds => ds.KH_TRA_LAI.GetValueOrDefault(0)));
            column = this.raddgrLichTraNo.Columns["TongTienLai"] as GridViewExpressionColumn;
            column.Expression = expressionKHLai;
            ucNguonVon.EditCellEnd += new EventHandler(ucNguonVon_EditCellEnd);
        }

        void ucNguonVon_EditCellEnd(object sender, EventArgs e)
        {
            TDVM_KHE_UOC objKUOC = ucNguonVon.cellEdit.ParentRow.Item as TDVM_KHE_UOC;
            lstDSachKUOCVM[lstDSachKUOCVM.IndexOf(objKUOC)].KUOC_VM.NV_LOAI_NVON = ucNguonVon.GiaTri;
        }

        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.TinDungTT.KUOC.ucLapKheUocDS_01", "RibbonButton");
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
            else if (strTinhNang.Equals("PreviewHopDongVayVon"))
            {
                OnPreviewHopDongVayVon();
            }
            else if (strTinhNang.Equals("PreviewPhuLucHopDong"))
            {
                OnPreviewPhuLucHopDong();
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
            teldtNgayLapKU.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            teldtThangGiaoVon.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            teldtThangBDTra.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            teldtNgayBDTra.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            LoadComboBoxDotPhatVon();
            LoadComboBoxBDauTraVon();
            TThaiNVu = "";
            action = DatabaseConstant.Action.THEM;
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_CHI_TIET_KHE_UOC);
        }

        private void SetEnabledAllControl(bool bBool)
        {
            if (TThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()) && action.Equals(DatabaseConstant.Action.SUA))
            {

            }
            else
            {
                grbThongTinChung.IsEnabled = bBool;
                tlbDetailAdd.IsEnabled = bBool;
                tlbDetailDelete.IsEnabled = bBool;
            }
        }

        void teldtNgayPhatVon_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            
        }

        void tlbDetailDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (TDVM_KHE_UOC obj in raddgrTUngCT.SelectedItems)
                {
                    lstID.Add(obj.KUOC_VM.ID);
                    raddgrTUngCT.Items.Remove(obj);
                }
            }
            catch (Exception ex)
            { 
            }
        }

        void tlbDetailAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbNhom.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoTrong(lblNhom.Content.ToString());
                    cmbNhom.Focus();
                    return;
                }
                lstDSachKUOCVM = raddgrTUngCT.ItemsSource as List<TDVM_KHE_UOC>;
                if (LObject.IsNullOrEmpty(lstDSachKUOCVM)) lstDSachKUOCVM = new List<TDVM_KHE_UOC>();
                string lstIDHDTDVM = "";
                foreach (TDVM_KHE_UOC objTDVM_KHE_UOC in lstDSachKUOCVM)
                {
                    lstIDHDTDVM += "," + objTDVM_KHE_UOC.KUOC_VM.ID_HDTDVM.ToString();
                }
                if (lstIDHDTDVM.Length > 0)
                    lstIDHDTDVM = "(" + lstIDHDTDVM.Substring(1) + ")";
                else
                    lstIDHDTDVM = "(0)";
                AutoCompleteEntry auNhom = lstSourceNhom.ElementAt(cmbNhom.SelectedIndex);
                lstPopup.Clear();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add("HDTDLAPKU");
                lstDieuKien.Add(auNhom.KeywordStrings[1]);
                lstDieuKien.Add(auNhom.KeywordStrings[3]);
                lstDieuKien.Add(lstIDHDTDVM);
                lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_DON_XIN_VAY_VON_THEO_NHOM", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(false, simplePopupResponse, true);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.Content = popup;
                win.ShowDialog();
                List<ClientResponseDetail> lstClientResponseDetail = new List<ClientResponseDetail>();
                if (lstPopup.Count > 0)
                {
                    
                    foreach (DataRow dr in lstPopup)
                    {
                        TDVM_KHE_UOC objTDVM_KHE_UOC = new TDVM_KHE_UOC();
                        objTDVM_KHE_UOC.ID_CUM = idCum;
                        objTDVM_KHE_UOC.ID_KHANG = Convert.ToInt32(dr["ID_KHANG"]);
                        objTDVM_KHE_UOC.MA_KHANG = dr["MA_KHANG"].ToString();
                        objTDVM_KHE_UOC.NGAY_BD_TRA = teldtNgayBDTra.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                        objTDVM_KHE_UOC.SAN_PHAM = new TD_SAN_PHAM();
                        objTDVM_KHE_UOC.SAN_PHAM.MA_SAN_PHAM = dr["MA_SAN_PHAM"].ToString();
                        objTDVM_KHE_UOC.SAN_PHAM.TEN_SAN_PHAM = dr["TEN_SAN_PHAM"].ToString();
                        objTDVM_KHE_UOC.TEN_KHANG = dr["TEN_KHANG"].ToString();
                        objTDVM_KHE_UOC.KUOC_VM = new TD_KUOCVM();
                        objTDVM_KHE_UOC.KUOC_VM.MA_SAN_PHAM = dr["MA_SAN_PHAM"].ToString();
                        objTDVM_KHE_UOC.KUOC_VM.NV_LOAI_NVON = nguonVon;
                        objTDVM_KHE_UOC.KUOC_VM.MA_LSUAT = dr["MA_LSUAT"].ToString();
                        objTDVM_KHE_UOC.KUOC_VM.ID_HDTDVM = Convert.ToInt32(dr["ID_HDTDVM"]);
                        objTDVM_KHE_UOC.KUOC_VM.MA_HDTDVM = dr["MA_HDTDVM"].ToString();
                        objTDVM_KHE_UOC.KUOC_VM.MA_KUOCVM = "";
                        objTDVM_KHE_UOC.KUOC_VM.HE_SO = Convert.ToInt32(dr["HE_SO"]);
                        objTDVM_KHE_UOC.KUOC_VM.TGIAN_VAY = Convert.ToInt32(dr["TGIAN_VAY"]);
                        objTDVM_KHE_UOC.KUOC_VM.TGIAN_VAY_DVI_TINH = dr["TGIAN_VAY_DVI_TINH"].ToString();
                        objTDVM_KHE_UOC.KUOC_VM.LAI_SUAT = Convert.ToDecimal(dr["LAI_SUAT"]);
                        objTDVM_KHE_UOC.KUOC_VM.SO_TIEN_GIAI_NGAN = Convert.ToDecimal(dr["SO_TIEN_VAY"]);
                        objTDVM_KHE_UOC.SO_TIEN_XIN_VAY = Convert.ToDecimal(dr["SO_TIEN_XIN_VAY"]);
                        objTDVM_KHE_UOC.KUOC_VM.MUC_DICH_VAY = dr["MUC_DICH_VAY"].ToString();
                        objTDVM_KHE_UOC.DD_GTLQ_SO = dr["DD_GTLQ_SO"].ToString();
                        objTDVM_KHE_UOC.KUOC_VM.MA_DVI_QLY = ClientInformation.MaDonVi;
                        objTDVM_KHE_UOC.KUOC_VM.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                        lstDSachKUOCVM.Add(objTDVM_KHE_UOC);
                    }
                    LoadGridViewDSKheUoc();
                }
            }
            catch (Exception ex)
            {
            }
        }

        void btnLaiSuat_Click(object sender, RoutedEventArgs e)
        {
            
        }

        void btnSanPham_Click(object sender, RoutedEventArgs e)
        {
            
        }

        void txtSanPham_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        void cmbDinhKyTraLai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        void cmbDinhKyTraGoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        void tlbXemChiTiet_Click(object sender, RoutedEventArgs e)
        {
            TDVM_KHE_UOC objKUOC = raddgrLichTraNo.SelectedItem as TDVM_KHE_UOC;
            int index = lstDSachKUOCVM.IndexOf(objKUOC);
            if (Validation())
            {
                GetDataForm(BusinessConstant.TrangThaiBanGhi.SU_DUNG, BusinessConstant.TrangThaiNghiepVu.DEFAULT);
                if(GenDKien)
                    lstDSachKUOCVM.ForEach(f => { f.DSACH_KHOACHVM_CTIET = new List<TD_KHOACHVM_CT>().ToArray(); f.DSACH_KHOACHVM = new List<TD_KHOACHVM>().ToArray(); }); ;
                objKUOCVMDS.DSACH_KHE_UOC = lstDSachKUOCVM.ToArray();
                List<ClientResponseDetail> lstresponseDetail = new List<ClientResponseDetail>();
                int iret = new TinDungProcess().LapKheUocDanhSach_01(DatabaseConstant.Action.TINH_TOAN_LICH_TRA_NO, ref objKUOCVMDS, ref lstresponseDetail);
                if (iret > 0)
                {
                    
                    LoadGridViewDSKheUoc();
                    if (index >= 0)
                        objKUOC = lstDSachKUOCVM.ElementAt(index);
                    ucPopopKeHoachCT popup = new ucPopopKeHoachCT(ref lstDSachKUOCVM, objKUOC, objKUOCVMDS);
                    popup.LayGiaTri = new ucPopopKeHoachCT.LayGiaTriLapLich(LayThongTinLapLich);
                    objKUOCVMDS.DSACH_KHE_UOC = lstDSachKUOCVM.ToArray();
                    Window win = new Window();
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    win.Title = "Kế hoạch chi tiết";
                    win.Content = popup;
                    win.ShowDialog();
                    LoadGridViewDSKheUoc();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(lstresponseDetail);
                }
            }
            
        }

        void LockControl()
        {
            
        }

        void raddgrTUngCT_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {

        }

        void raddgrTUngCT_CellValidating(object sender, GridViewCellValidatingEventArgs e)
        {

        }

        void cmbMucDichVay_EditCellEnd(object sender, EventArgs e)
        {
            
        }

        void cmbNguonVonVay_EditCellEnd(object sender, EventArgs e)
        {
            
        }

        void cmbDViTinhTGian_EditCellEnd(object sender, EventArgs e)
        {
            
        }

        void LockControl(bool bBool)
        {
            
        }

        void teldtThangBDTra_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            LoadComboBoxBDauTraVon();
        }

        void teldtThangGiaoVon_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            LoadComboBoxDotPhatVon();
            //LoadComboBoxBDauTraVon();
        }

        void dtpThangBDTra_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboBoxBDauTraVon();
        }

        void dtpThangGiaoVon_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboBoxDotPhatVon();
        }

        void cmbCum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboBoxNhom();
            LoadComboBoxDotPhatVon();
            LoadComboBoxBDauTraVon();
        }

        void cmbKhuVuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboBoxCum();
        }

        void cmbNhom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbNhom.Items.Count <= 0 || cmbNhom.SelectedIndex < 0)
                return;
            AutoCompleteEntry auNhom = lstSourceNhom.ElementAt(cmbNhom.SelectedIndex);
            idNhom = auNhom.KeywordStrings[1].StringToInt32();
            lstDSachKUOCVM = new List<TDVM_KHE_UOC>();
            raddgrTUngCT.ItemsSource = lstDSachKUOCVM;
            raddgrTUngCT.Rebind();
        }

        void cmbThangBDTra_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbThangBDTra.Items.Count <= 0 || cmbThangBDTra.SelectedIndex < 0)
                return;
            AutoCompleteEntry au = lstSourceBDauTra.ElementAt(cmbThangBDTra.SelectedIndex);
            teldtNgayBDTra.Value = LDateTime.StringToDate(au.KeywordStrings[2], ApplicationConstant.defaultDateTimeFormat);
            dtNgayBatDauTraNo.Value = LDateTime.StringToDate(au.KeywordStrings[2], ApplicationConstant.defaultDateTimeFormat);
            dtNgayBatDauTraNoCum.Value = LDateTime.StringToDate(au.KeywordStrings[2], ApplicationConstant.defaultDateTimeFormat);
            GenDKien = true;
        }
        
        void cmbHinhThucTraLai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GenDKien = true;
        }

        void cmbHinhThucTraGoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GenDKien = true;
        }

        void LoadComboBoxKhuVuc()
        {
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(idDonVi.ToString());
            lstSourceKhuVuc.Clear();
            cmbKhuVuc.Items.Clear();
            new AutoComboBox().GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, "COMBOBOX_KHUVUC", lstDieuKien,idKhuVuc.ToString());
        }

        void LoadComboBoxCum()
        {
            lstSourceCum.Clear();
            cmbCum.Items.Clear();
            if (cmbKhuVuc.Items.Count <= 0 || cmbKhuVuc.SelectedIndex < 0)
                return;
            AutoCompleteEntry auKhuVuc = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex);
            idKhuVuc = auKhuVuc.KeywordStrings[1].StringToInt32();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(idDonVi.ToString());
            lstDieuKien.Add(auKhuVuc.KeywordStrings[1]);
            new AutoComboBox().GenAutoComboBox(ref lstSourceCum, ref cmbCum, "COMBOBOX_CUM", lstDieuKien,idCum.ToString());
        }

        void LoadComboBoxNhom()
        {
            lstSourceNhom.Clear();
            cmbNhom.Items.Clear();
            if (cmbKhuVuc.Items.Count <= 0 || cmbKhuVuc.SelectedIndex < 0 || cmbCum.Items.Count <= 0 || cmbCum.SelectedIndex < 0)
                return;
            AutoCompleteEntry auKhuVuc = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex);
            AutoCompleteEntry auCum = lstSourceCum.ElementAt(cmbCum.SelectedIndex);
            idCum = auCum.KeywordStrings[1].StringToInt32();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(idDonVi.ToString());
            lstDieuKien.Add(auKhuVuc.KeywordStrings[1]);
            lstDieuKien.Add(auCum.KeywordStrings[1]);
            new AutoComboBox().GenAutoComboBox(ref lstSourceNhom, ref cmbNhom, "COMBOBOX_NHOM", lstDieuKien,idNhom.ToString());
        }

        void LoadComboBoxDotPhatVon()
        {
            if (isLoad)
                return;
            lstSourceGiaoVon.Clear();
            cmbDotGiaoVon.Items.Clear();
            if (cmbCum.Items.Count <= 0 || cmbCum.SelectedIndex < 0)
                return;
            AutoCompleteEntry auCum = lstSourceCum.ElementAt(cmbCum.SelectedIndex);
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(LDateTime.DateToString(teldtThangGiaoVon.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat));
            lstDieuKien.Add("PHAT_VON");
            lstDieuKien.Add(auCum.KeywordStrings[1]);
            new AutoComboBox().GenAutoComboBox(ref lstSourceGiaoVon, ref cmbDotGiaoVon, "COMBOBOX_DOT_THU_PHAT", lstDieuKien);
            AutoCompleteEntry au = null;
            if (cmbDotGiaoVon.SelectedIndex > -1)
                au = lstSourceGiaoVon.ElementAt(cmbDotGiaoVon.SelectedIndex);
            if (!LObject.IsNullOrEmpty(au))
                teldtThangBDTra.Value = LDateTime.StringToDate(au.KeywordStrings[2], ApplicationConstant.defaultDateTimeFormat).AddMonths(1).AddDays(1);
            else
                LMessage.ShowMessage(LLanguage.SearchResourceByKey("M.DungChung.ThongBao.ChuaKhaiBaoLichPhatVon",new string[1]{auCum.DisplayName}), LMessage.MessageBoxType.Warning);
        }

        void LoadComboBoxBDauTraVon()
        {
            if (isLoad)
                return;
            lstSourceBDauTra.Clear();
            cmbThangBDTra.Items.Clear();
            if (cmbCum.Items.Count <= 0 || cmbCum.SelectedIndex < 0)
                return;
            AutoCompleteEntry auCum = lstSourceCum.ElementAt(cmbCum.SelectedIndex);
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(LDateTime.DateToString(teldtThangBDTra.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat));
            lstDieuKien.Add("THU_VON");
            lstDieuKien.Add(auCum.KeywordStrings[1]);
            new AutoComboBox().GenAutoComboBox(ref lstSourceBDauTra, ref cmbThangBDTra, "COMBOBOX_DOT_THU_PHAT", lstDieuKien);
        }

        void cmbDotGiaoVon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDotGiaoVon.SelectedIndex < 0)
                return;
            AutoCompleteEntry au = lstSourceGiaoVon.ElementAt(cmbDotGiaoVon.SelectedIndex);
            if (au != null)
                teldtThangBDTra.Value = LDateTime.StringToDate(au.KeywordStrings[2], ApplicationConstant.defaultDateTimeFormat).AddMonths(1).AddDays(1);
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
                AutoCompleteEntry auHinhThucGoc = lstSourceHinhThucGoc.ElementAt(cmbHinhThucTraGoc.SelectedIndex);
                AutoCompleteEntry auHinhThucLai = lstSourceHinhThucLai.ElementAt(cmbHinhThucTraLai.SelectedIndex);
                AutoCompleteEntry auNgayGiaoVon = lstSourceGiaoVon.ElementAt(cmbDotGiaoVon.SelectedIndex);
                lstDSachKUOCVM = raddgrTUngCT.ItemsSource as List<TDVM_KHE_UOC>;
                if (LObject.IsNullOrEmpty(objKUOCVMDS)) objKUOCVMDS = new TDVM_KHE_UOC_DSACH();
                objKUOCVMDS.ID_CUM = idCum;
                objKUOCVMDS.HINH_THUC_TRA_GOC = auHinhThucGoc.KeywordStrings.FirstOrDefault();
                objKUOCVMDS.HINH_THUC_TRA_LAI = auHinhThucLai.KeywordStrings.FirstOrDefault();
                objKUOCVMDS.ID_KHU_VUC = idKhuVuc;
                objKUOCVMDS.ID_NHOM = idNhom;
                objKUOCVMDS.MA_GDICH = txtSoGiaoDich.Text;
                objKUOCVMDS.NGAY_GIAI_NGAN = auNgayGiaoVon.KeywordStrings[2];
                objKUOCVMDS.NGAY_LAP_KUOC = LDateTime.DateToString(teldtNgayLapKU.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
                objKUOCVMDS.DSACH_ID_XOA = lstID.ToArray();
                lstDSachKUOCVM.ForEach(f => { f.KUOC_VM.NGAY_GIAI_NGAN = objKUOCVMDS.NGAY_GIAI_NGAN; f.KUOC_VM.NGAY_LAP_KUOC = objKUOCVMDS.NGAY_LAP_KUOC;
                f.KUOC_VM.TRGOC_HTHUC = objKUOCVMDS.HINH_THUC_TRA_GOC; f.KUOC_VM.TRLAI_HTHUC = objKUOCVMDS.HINH_THUC_TRA_LAI;
                f.KUOC_VM.TRGOC_SO_KY = f.KUOC_VM.TGIAN_VAY; f.KUOC_VM.TRLAI_SO_KY = f.KUOC_VM.TGIAN_VAY;
                f.NGAY_BD_TRA = teldtNgayBDTra.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                f.KUOC_VM.KHOACH_NGAY_LAP = teldtNgayBDTra.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                f.KUOC_VM.KHOACH_NGAY_LAP_CUM = teldtNgayBDTra.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                f.KUOC_VM.MA_GDICH = objKUOCVMDS.MA_GDICH; f.KUOC_VM.TTHAI_BGHI = bghi.layGiaTri(); f.KUOC_VM.TTHAI_NVU = nghiepvu.layGiaTri();
                f.KUOC_VM.SO_TAI_KHOAN = ""; f.KUOC_VM.LOAI_LSUAT = BusinessConstant.LOAI_LAI_SUAT.CO_DINH.layGiaTri();
                f.KUOC_VM.LSUAT_BIEN_DO = 0; f.KUOC_VM.LAI_SUAT_CLN = 0; f.KUOC_VM.LAI_SUAT_QH = (f.KUOC_VM.LAI_SUAT * (decimal)1.5);
                f.KUOC_VM.SO_DU = 0; f.KUOC_VM.NHOM_NO_HIEN_TAI = BusinessConstant.NHOM_NO.NHOM1.layGiaTri(); f.KUOC_VM.LAI_TREO = 0;
                f.KUOC_VM.LAI_DU_THU = 0; f.KUOC_VM.LAI_DU_THU_NBANG = 0; f.KUOC_VM.LAI_PHAI_THU = 0; f.KUOC_VM.LAI_DA_THU = 0;
                f.KUOC_VM.GOC_DA_THU = 0; f.KUOC_VM.SO_TIEN_TLDP = 0; f.KUOC_VM.LAI_DA_XUAT_NB = 0;
                f.KUOC_VM.MA_DVI_QLY = ClientInformation.MaDonVi; f.KUOC_VM.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                f.KUOC_VM.NGAY_NHAP = ClientInformation.NgayLamViecHienTai; f.KUOC_VM.NGUOI_NHAP = ClientInformation.TenDangNhap;
                f.KUOC_VM.TTHAI_KUOC = BusinessConstant.TRANG_THAI_TAT_TOAN.CHUA_TAT_TOAN.layGiaTri();
                });
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
                txtSoGiaoDich.Text = objKUOCVMDS.MA_GDICH;
                Dispatcher.CurrentDispatcher.DelayInvoke("SetDataForm", () =>
                {
                    DataSet ds = new TinDungProcess().getThongTinChiTietKUOCVMDSByGDich(objKUOCVMDS.MA_GDICH);
                    if (!LObject.IsNullOrEmpty(ds) && ds.Tables.Count > 2)
                    {
                        Dispatcher.CurrentDispatcher.DelayInvoke("SetDataTabThongTinChung", () =>
                        {
                            SetDataTabThongTinChung(ds);
                            SetDataTabLichTraNo(ds);
                            SetDataTabTSDB(ds);
                            SetDataTabPhi(ds);
                            LoadGridViewDSKheUoc();
                        }, TimeSpan.FromSeconds(0));

                        Dispatcher.CurrentDispatcher.DelayInvoke("SetDataTabThongTinKiemSoat", () =>
                        {
                            SetDataTabThongTinKiemSoat(ds);
                        }, TimeSpan.FromSeconds(0));
                        //SetDataTabThongTinChung(ds);
                        //SetDataTabLichTraNo(ds);
                        //SetDataTabTSDB(ds);
                        //SetDataTabPhi(ds);
                        //SetDataTabThongTinKiemSoat(ds);
                        
                    }
                }, TimeSpan.FromSeconds(0));
                
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
                isLoad = true;
                DataTable dt = ds.Tables["KUOC"];
                lstDSachKUOCVM = new List<TDVM_KHE_UOC>();
                idCum = Convert.ToInt32(ds.Tables["KUOC"].Rows[0]["ID_CUM"]);
                idKhuVuc = Convert.ToInt32(ds.Tables["KUOC"].Rows[0]["ID_KVUC"]);
                idNhom = Convert.ToInt32(ds.Tables["KUOC"].Rows[0]["ID_NHOM"]);
                idDonVi = Convert.ToInt32(ds.Tables["KUOC"].Rows[0]["ID_DON_VI"]);
                LoadComboBoxKhuVuc();
                cmbKhuVuc.SelectedIndex = lstSourceKhuVuc.IndexOf(lstSourceKhuVuc.FirstOrDefault(f => f.KeywordStrings[1].Equals(idKhuVuc.ToString())));
                cmbCum.SelectedIndex = lstSourceCum.IndexOf(lstSourceCum.FirstOrDefault(f => f.KeywordStrings[1].Equals(idCum.ToString())));
                cmbNhom.SelectedIndex = lstSourceNhom.IndexOf(lstSourceNhom.FirstOrDefault(f => f.KeywordStrings[1].Equals(idNhom.ToString())));
                foreach (DataRow dr in dt.Rows)
                {
                    TDVM_KHE_UOC dtoTDKUOCVM = new TDVM_KHE_UOC();
                    
                    objKUOCVMDS.MA_GDICH = dr["MA_GDICH"].ToString();
                    dtoTDKUOCVM.ID_CUM = idCum;
                    dtoTDKUOCVM.ID_KHANG = Convert.ToInt32(dr["ID_KHANG"]);
                    dtoTDKUOCVM.MA_KHANG = dr["MA_KHANG"].ToString();
                    dtoTDKUOCVM.NGAY_BD_TRA = dr["KHOACH_NGAY_LAP"].ToString();
                    dtoTDKUOCVM.SAN_PHAM = new TD_SAN_PHAM();
                    dtoTDKUOCVM.SAN_PHAM.MA_SAN_PHAM = dr["MA_SAN_PHAM"].ToString();
                    dtoTDKUOCVM.SAN_PHAM.TEN_SAN_PHAM = dr["TEN_SAN_PHAM"].ToString();
                    dtoTDKUOCVM.SO_TIEN_XIN_VAY = Convert.ToDecimal(dr["SO_TIEN_XIN_VAY"]);
                    dtoTDKUOCVM.TEN_KHANG = dr["TEN_KHANG"].ToString();
                    dtoTDKUOCVM.DD_GTLQ_SO = dr["DD_GTLQ_SO"].ToString();
                    dtoTDKUOCVM.KUOC_VM = new TD_KUOCVM();
                    dtoTDKUOCVM.KUOC_VM.ID = Convert.ToInt32(dr["ID"]);
                    dtoTDKUOCVM.KUOC_VM.MA_SAN_PHAM = dr["MA_SAN_PHAM"].ToString();
                    dtoTDKUOCVM.KUOC_VM.ID_HDTDVM = Convert.ToInt32(dr["ID_HDTDVM"]);
                    dtoTDKUOCVM.KUOC_VM.MA_HDTDVM = dr["MA_HDTDVM"].ToString();
                    dtoTDKUOCVM.KUOC_VM.MA_KUOCVM = dr["MA_KUOCVM"].ToString(); ;
                    dtoTDKUOCVM.KUOC_VM.HE_SO = Convert.ToInt32(dr["HE_SO"]);
                    dtoTDKUOCVM.KUOC_VM.TGIAN_VAY = Convert.ToInt32(dr["TGIAN_VAY"]);
                    dtoTDKUOCVM.KUOC_VM.TGIAN_VAY_DVI_TINH = dr["TGIAN_VAY_DVI_TINH"].ToString();
                    dtoTDKUOCVM.KUOC_VM.SO_TIEN_GIAI_NGAN = Convert.ToDecimal(dr["SO_TIEN_GIAI_NGAN"]);
                    dtoTDKUOCVM.KUOC_VM.NGAY_DAO_HAN = dr["NGAY_DAO_HAN"].ToString();
                    dtoTDKUOCVM.KUOC_VM.MUC_DICH_VAY = dr["MUC_DICH_VAY"].ToString();
                    dtoTDKUOCVM.KUOC_VM.TRGOC_HTHUC = dr["TRGOC_HTHUC"].ToString();
                    dtoTDKUOCVM.KUOC_VM.TRLAI_HTHUC = dr["TRLAI_HTHUC"].ToString();
                    dtoTDKUOCVM.KUOC_VM.NGAY_GIAI_NGAN = dr["NGAY_GIAI_NGAN"].ToString();
                    dtoTDKUOCVM.KUOC_VM.KHOACH_NGAY_LAP = dr["KHOACH_NGAY_LAP"].ToString();
                    dtoTDKUOCVM.KUOC_VM.NV_LOAI_NVON = dr["NV_LOAI_NVON"].ToString();
                    dtoTDKUOCVM.KUOC_VM.TTHAI_BGHI = dr["TTHAI_BGHI"].ToString();
                    dtoTDKUOCVM.KUOC_VM.TTHAI_KUOC = dr["TTHAI_KUOC"].ToString();
                    dtoTDKUOCVM.KUOC_VM.TTHAI_NVU = TThaiNVu = dr["TTHAI_NVU"].ToString();
                    dtoTDKUOCVM.KUOC_VM.MA_DVI_QLY = dr["MA_DVI_QLY"].ToString();
                    dtoTDKUOCVM.KUOC_VM.MA_DVI_TAO = dr["MA_DVI_TAO"].ToString();
                    dtoTDKUOCVM.KUOC_VM.NGAY_CNHAT = dr["NGAY_CNHAT"].ToString();
                    dtoTDKUOCVM.KUOC_VM.NGAY_NHAP = dr["NGAY_NHAP"].ToString();
                    dtoTDKUOCVM.KUOC_VM.NGUOI_CNHAT = dr["NGUOI_CNHAT"].ToString();
                    dtoTDKUOCVM.KUOC_VM.NGUOI_NHAP = dr["NGUOI_NHAP"].ToString();
                    lstDSachKUOCVM.Add(dtoTDKUOCVM);
                }
                cmbHinhThucTraGoc.SelectedIndex = lstSourceHinhThucGoc.IndexOf(lstSourceHinhThucGoc.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(lstDSachKUOCVM[0].KUOC_VM.TRGOC_HTHUC)));
                cmbHinhThucTraLai.SelectedIndex = lstSourceHinhThucLai.IndexOf(lstSourceHinhThucLai.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(lstDSachKUOCVM[0].KUOC_VM.TRLAI_HTHUC)));
                teldtThangGiaoVon.Value = LDateTime.StringToDate(lstDSachKUOCVM[0].KUOC_VM.NGAY_GIAI_NGAN, ApplicationConstant.defaultDateTimeFormat);
                teldtNgayBDTra.Value = LDateTime.StringToDate(lstDSachKUOCVM[0].NGAY_BD_TRA, ApplicationConstant.defaultDateTimeFormat);
                teldtThangBDTra.Value = LDateTime.StringToDate(lstDSachKUOCVM[0].NGAY_BD_TRA, ApplicationConstant.defaultDateTimeFormat);
                dtNgayBatDauTraNo.Value = LDateTime.StringToDate(lstDSachKUOCVM[0].NGAY_BD_TRA, ApplicationConstant.defaultDateTimeFormat);
                dtNgayBatDauTraNoCum.Value = LDateTime.StringToDate(lstDSachKUOCVM[0].NGAY_BD_TRA, ApplicationConstant.defaultDateTimeFormat);
                isLoad = false;
                LoadComboBoxDotPhatVon();
                LoadComboBoxBDauTraVon();
                teldtNgayBDTra.Value = LDateTime.StringToDate(lstDSachKUOCVM[0].NGAY_BD_TRA, ApplicationConstant.defaultDateTimeFormat);
                cmbDotGiaoVon.SelectedIndex = lstSourceGiaoVon.IndexOf(lstSourceGiaoVon.FirstOrDefault(f => f.KeywordStrings[2].Equals(lstDSachKUOCVM[0].KUOC_VM.NGAY_GIAI_NGAN)));
                cmbThangBDTra.SelectedIndex = lstSourceBDauTra.IndexOf(lstSourceBDauTra.FirstOrDefault(f => f.KeywordStrings[2].Equals(lstDSachKUOCVM[0].KUOC_VM.KHOACH_NGAY_LAP)));
                GenDKien = false;
                objKUOCVMDS.DSACH_KHE_UOC = lstDSachKUOCVM.ToArray();
                txtSoGiaoDich.Text = objKUOCVMDS.MA_GDICH;
                if (action.Equals(DatabaseConstant.Action.SUA))
                {
                    SetEnabledAllControl(true);
                    CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01);
                }
                else
                {
                    SetEnabledAllControl(false);
                    CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01);
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
                foreach (TDVM_KHE_UOC objTDVM_KHE_UOC in lstDSachKUOCVM)
                {
                    DataTable dt = ds.Tables["KHOACHVM"];
                    string maKUOCVM = objTDVM_KHE_UOC.KUOC_VM.MA_KUOCVM;
                    DataRow[] arrDr = dt.Select("MA_KUOCVM ='" + maKUOCVM + "'");
                    List<TD_KHOACHVM> lstKHOACH = new List<TD_KHOACHVM>();
                    foreach (DataRow dr in arrDr)
                    {
                        TD_KHOACHVM objTD_KHOACHVM = new TD_KHOACHVM();
                        objTD_KHOACHVM.ID = Convert.ToInt32(dr["ID"]);
                        objTD_KHOACHVM.ID_KUOCVM = Convert.ToInt32(dr["ID_KUOCVM"]);
                        objTD_KHOACHVM.LOAI_HINH_LAP_KH = dr["LOAI_HINH_LAP_KH"].ToString();
                        objTD_KHOACHVM.MA_BPHI = dr["MA_BPHI"].ToString();
                        objTD_KHOACHVM.MA_DVI_QLY = dr["MA_DVI_QLY"].ToString();
                        objTD_KHOACHVM.MA_DVI_TAO = dr["MA_DVI_TAO"].ToString();
                        objTD_KHOACHVM.MA_HTHUC = dr["MA_HTHUC"].ToString();
                        objTD_KHOACHVM.MA_KUOCVM = dr["MA_KUOCVM"].ToString();
                        objTD_KHOACHVM.MA_NNHAN_TDOI = dr["MA_NNHAN_TDOI"].ToString();
                        objTD_KHOACHVM.NGAY_BDAU = dr["NGAY_BDAU"].ToString();
                        objTD_KHOACHVM.NGAY_CNHAT = dr["NGAY_CNHAT"].ToString();
                        objTD_KHOACHVM.NGAY_NHAP = dr["NGAY_NHAP"].ToString();
                        objTD_KHOACHVM.NGUOI_CNHAT = dr["NGUOI_CNHAT"].ToString();
                        objTD_KHOACHVM.NGUOI_NHAP = dr["NGUOI_NHAP"].ToString();
                        objTD_KHOACHVM.SO_KY = Convert.ToInt32(dr["SO_KY"]);
                        objTD_KHOACHVM.SO_TIEN = Convert.ToDecimal(dr["SO_TIEN"]);
                        objTD_KHOACHVM.TAN_SUAT = Convert.ToInt32(dr["TAN_SUAT"]);
                        objTD_KHOACHVM.TTHAI_BGHI = dr["TTHAI_BGHI"].ToString();
                        objTD_KHOACHVM.TTHAI_NVU = dr["TTHAI_NVU"].ToString();
                        lstKHOACH.Add(objTD_KHOACHVM);
                    }
                    objTDVM_KHE_UOC.DSACH_KHOACHVM = lstKHOACH.ToArray();

                    dt = ds.Tables["KHOACHVM_CTIET"];
                    arrDr = dt.Select("MA_KUOCVM ='" + maKUOCVM + "'");
                    List<TD_KHOACHVM_CT> lstKHOACHCT = new List<TD_KHOACHVM_CT>();
                    foreach (DataRow dr in arrDr)
                    {
                        TD_KHOACHVM_CT objTD_KHOACHVM = new TD_KHOACHVM_CT();
                        objTD_KHOACHVM.ID = Convert.ToInt32(dr["ID"]);
                        objTD_KHOACHVM.ID_KUOCVM = Convert.ToInt32(dr["ID_KUOCVM"]);
                        if (dr["ID_KHOACHVM"] != DBNull.Value)
                            objTD_KHOACHVM.ID_KHOACHVM = Convert.ToInt32(dr["ID_KHOACHVM"]);
                        if (dr["CT_CTRA_GOC"] != DBNull.Value)
                            objTD_KHOACHVM.CT_CTRA_GOC = Convert.ToDecimal(dr["CT_CTRA_GOC"]);
                        if (dr["CT_CTRA_LAI"] != DBNull.Value)
                        objTD_KHOACHVM.CT_CTRA_LAI = Convert.ToDecimal(dr["CT_CTRA_LAI"]);
                        if (dr["CT_CTRA_LPHAT"] != DBNull.Value)
                        objTD_KHOACHVM.CT_CTRA_LPHAT = Convert.ToDecimal(dr["CT_CTRA_LPHAT"]);
                        if (dr["CT_CTRA_PHI"] != DBNull.Value)
                        objTD_KHOACHVM.CT_CTRA_PHI = Convert.ToDecimal(dr["CT_CTRA_PHI"]);
                        if (dr["CT_NGAY_CTRA"] != DBNull.Value)
                        objTD_KHOACHVM.CT_NGAY_CTRA = dr["CT_NGAY_CTRA"].ToString();
                        if (dr["DU_GOC"] != DBNull.Value)
                        objTD_KHOACHVM.DU_GOC = Convert.ToDecimal(dr["DU_GOC"]);
                        if (dr["DU_LAI"] != DBNull.Value)
                        objTD_KHOACHVM.DU_LAI = Convert.ToDecimal(dr["DU_LAI"]);
                        if (dr["KH_LAI_PHAT"] != DBNull.Value)
                        objTD_KHOACHVM.KH_LAI_PHAT = Convert.ToDecimal(dr["KH_LAI_PHAT"]);
                        objTD_KHOACHVM.KH_LAI_PHAT_KHONG = dr["KH_LAI_PHAT_KHONG"].ToString();
                        if (dr["KH_NGAY_TRA"] != DBNull.Value)
                        objTD_KHOACHVM.KH_NGAY_TRA = dr["KH_NGAY_TRA"].ToString();
                        if (dr["KH_TRA_GOC"] != DBNull.Value)
                        objTD_KHOACHVM.KH_TRA_GOC = Convert.ToDecimal(dr["KH_TRA_GOC"]);
                        objTD_KHOACHVM.KH_TRA_GOC_KHONG = dr["KH_TRA_GOC_KHONG"].ToString();
                        if (dr["KH_TRA_LAI"] != DBNull.Value)
                        objTD_KHOACHVM.KH_TRA_LAI = Convert.ToDecimal(dr["KH_TRA_LAI"]);
                        objTD_KHOACHVM.KH_TRA_LAI_KHONG = dr["KH_TRA_LAI_KHONG"].ToString();
                        if (dr["KH_TRA_PHI"] != DBNull.Value)
                        objTD_KHOACHVM.KH_TRA_PHI = Convert.ToDecimal(dr["KH_TRA_PHI"]);
                        objTD_KHOACHVM.KH_TRA_PHI_KHONG = dr["KH_TRA_PHI_KHONG"].ToString();
                        if (dr["KY_THU"] != DBNull.Value)
                        objTD_KHOACHVM.KY_THU = Convert.ToInt32(dr["KY_THU"]);
                        if (dr["LAI_SUAT"] != DBNull.Value)
                        objTD_KHOACHVM.LAI_SUAT = Convert.ToDecimal(dr["LAI_SUAT"]);
                        objTD_KHOACHVM.LOAI_TTIN = dr["LOAI_TTIN"].ToString();
                        objTD_KHOACHVM.MA_DVI_QLY = dr["MA_DVI_QLY"].ToString();
                        objTD_KHOACHVM.MA_DVI_TAO = dr["MA_DVI_TAO"].ToString();
                        objTD_KHOACHVM.NGAY_CNHAT = dr["NGAY_CNHAT"].ToString();
                        objTD_KHOACHVM.NGAY_NHAP = dr["NGAY_NHAP"].ToString();
                        objTD_KHOACHVM.NGUOI_CNHAT = dr["NGUOI_CNHAT"].ToString();
                        objTD_KHOACHVM.NGUOI_NHAP = dr["NGUOI_NHAP"].ToString();
                        objTD_KHOACHVM.TT_NGAY_TRA = dr["TT_NGAY_TRA"].ToString();
                        objTD_KHOACHVM.TR_NGAY_TRA = dr["TR_NGAY_TRA"].ToString();
                        if (dr["TR_TRA_GOC"] != DBNull.Value)
                        objTD_KHOACHVM.TR_TRA_GOC = Convert.ToDecimal(dr["TR_TRA_GOC"]);
                        if (dr["TR_TRA_LAI"] != DBNull.Value)
                        objTD_KHOACHVM.TR_TRA_LAI = Convert.ToDecimal(dr["TR_TRA_LAI"]);
                        if (dr["TR_TRA_PHI"] != DBNull.Value)
                        objTD_KHOACHVM.TR_TRA_PHI = Convert.ToDecimal(dr["TR_TRA_PHI"]);
                        if (dr["TT_LAI_PHAT"] != DBNull.Value)
                        objTD_KHOACHVM.TT_LAI_PHAT = Convert.ToDecimal(dr["TT_LAI_PHAT"]);
                        objTD_KHOACHVM.TT_LAI_PHAT_KHONG = dr["TT_LAI_PHAT_KHONG"].ToString();
                        if (dr["TT_TRA_GOC"] != DBNull.Value)
                        objTD_KHOACHVM.TT_TRA_GOC = Convert.ToDecimal(dr["TT_TRA_GOC"]);
                        objTD_KHOACHVM.TT_TRA_GOC_KHONG = dr["TT_TRA_GOC_KHONG"].ToString();
                        if (dr["TT_TRA_LAI"] != DBNull.Value)
                        objTD_KHOACHVM.TT_TRA_LAI = Convert.ToDecimal(dr["TT_TRA_LAI"]);
                        objTD_KHOACHVM.TT_TRA_LAI_KHONG = dr["TT_TRA_LAI_KHONG"].ToString();
                        if (dr["TT_TRA_PHI"] != DBNull.Value)
                        objTD_KHOACHVM.TT_TRA_PHI = Convert.ToDecimal(dr["TT_TRA_PHI"]);
                        objTD_KHOACHVM.TT_TRA_PHI_KHONG = dr["TT_TRA_PHI_KHONG"].ToString();
                        objTD_KHOACHVM.TTHAI_KHOACH = dr["TTHAI_KHOACH"].ToString();
                        objTD_KHOACHVM.TTHAI_BGHI = dr["TTHAI_BGHI"].ToString();
                        objTD_KHOACHVM.TTHAI_NVU = dr["TTHAI_NVU"].ToString();
                        lstKHOACHCT.Add(objTD_KHOACHVM);
                    }
                    objTDVM_KHE_UOC.DSACH_KHOACHVM_CTIET = lstKHOACHCT.ToArray();
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
                txtNguoiCapNhat.Text = objKUOCVMDS.DSACH_KHE_UOC[0].KUOC_VM.NGUOI_CNHAT;
                txtNguoiLap.Text = objKUOCVMDS.DSACH_KHE_UOC[0].KUOC_VM.NGUOI_NHAP;
                txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(objKUOCVMDS.DSACH_KHE_UOC[0].KUOC_VM.TTHAI_BGHI);
                if (!LObject.IsNullOrEmpty(objKUOCVMDS.DSACH_KHE_UOC[0].KUOC_VM.NGAY_CNHAT)) teldtNgayCNhat.Value = objKUOCVMDS.DSACH_KHE_UOC[0].KUOC_VM.NGAY_CNHAT.StringToDate(ApplicationConstant.defaultDateTimeFormat);
                teldtNgayNhap.Value = objKUOCVMDS.DSACH_KHE_UOC[0].KUOC_VM.NGAY_NHAP.StringToDate(ApplicationConstant.defaultDateTimeFormat);
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(objKUOCVMDS.DSACH_KHE_UOC[0].KUOC_VM.TTHAI_NVU);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }

        }

        private void tlbLapKeHoach_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                GetDataForm(BusinessConstant.TrangThaiBanGhi.SU_DUNG, BusinessConstant.TrangThaiNghiepVu.DEFAULT);
                if (GenDKien)
                    lstDSachKUOCVM.ForEach(f => { f.DSACH_KHOACHVM_CTIET = new List<TD_KHOACHVM_CT>().ToArray(); f.DSACH_KHOACHVM = new List<TD_KHOACHVM>().ToArray(); });
                lstDSachKUOCVM.ForEach(f => { f.DSACH_KHOACHVM_CTIET = new List<TD_KHOACHVM_CT>().ToArray(); f.DSACH_KHOACHVM = new List<TD_KHOACHVM>().ToArray(); });
                objKUOCVMDS.DSACH_KHE_UOC = lstDSachKUOCVM.ToArray();
                List<ClientResponseDetail> lstresponseDetail = new List<ClientResponseDetail>();
                int iret = new TinDungProcess().LapKheUocDanhSach_01(DatabaseConstant.Action.TINH_TOAN_LICH_TRA_NO, ref objKUOCVMDS, ref lstresponseDetail);
                if (iret > 0)
                {
                    GenDKien=false;
                    lstDSachKUOCVM = objKUOCVMDS.DSACH_KHE_UOC.ToList();
                    LoadGridViewDSKheUoc();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(lstresponseDetail);
                }
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
                if (teldtNgayLapKU.Value.IsNullOrEmpty())
                {
                    CommonFunction.ThongBaoTrong(lblNgayLap.Content.ToString());
                    teldtNgayLapKU.Focus();
                    return false;
                }
                if (cmbKhuVuc.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoTrong(lblXa.Content.ToString());
                    cmbKhuVuc.Focus();
                    return false;
                }

                if (cmbCum.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoTrong(lblAp.Content.ToString());
                    cmbCum.Focus();
                    return false;
                }

                if (cmbNhom.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoTrong(lblNhom.Content.ToString());
                    cmbNhom.Focus();
                    return false;
                }

                if (LObject.IsNullOrEmpty(teldtThangGiaoVon.Value))
                {
                    CommonFunction.ThongBaoTrong(lblThangVayVon.Content.ToString());
                    teldtThangGiaoVon.Focus();
                    return false;
                }

                if (cmbDotGiaoVon.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoTrong(lblDotGiaoVon.Content.ToString());
                    cmbDotGiaoVon.Focus();
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
                
                if (LObject.IsNullOrEmpty(teldtThangBDTra.Value))
                {
                    CommonFunction.ThongBaoTrong(lblThangBDauTra.Content.ToString());
                    teldtThangBDTra.Focus();
                    return false;
                }

                if (cmbThangBDTra.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoTrong(lblDotBDauTra.Content.ToString());
                    cmbThangBDTra.Focus();
                    return false;
                }

                if (LObject.IsNullOrEmpty(lstDSachKUOCVM) || lstDSachKUOCVM.Count < 1)
                {
                    CommonFunction.ThongBaoTrong(lblDSachKheUoc.Content.ToString());
                    tlbDetailAdd.Focus();
                    return false;
                }

                if (lstDSachKUOCVM.Where(f => f.KUOC_VM.NV_LOAI_NVON == null).Count() > 0)
                {
                    LMessage.ShowMessage("M.TinDungTT.ucLapKheUocDS_01.ChuaChonNguonVon", LMessage.MessageBoxType.Warning);
                    raddgrTUngCT.Focus();
                    return false;
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
            SetEnabledAllControl(true);
            action = DatabaseConstant.Action.SUA;
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01);
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
                if (objKUOCVMDS.MA_GDICH.IsNullOrEmptyOrSpace())
                    iret = new TinDungProcess().LapKheUocDanhSach_01(DatabaseConstant.Action.THEM,ref objKUOCVMDS, ref lstResponseDetail);
                else
                    iret = new TinDungProcess().LapKheUocDanhSach_01(DatabaseConstant.Action.SUA, ref objKUOCVMDS, ref lstResponseDetail);
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
                    if (iret > 0)
                    {
                        SetInfomation();
                        LockControl();
                    }
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
                lstDSachKUOCVM = raddgrTUngCT.ItemsSource as List<TDVM_KHE_UOC>;
                objKUOCVMDS.MA_GDICH = txtSoGiaoDich.Text;
                if (lstDSachKUOCVM.Count < 1)
                    return;
                lstDSachKUOCVM.ForEach(f => f.KUOC_VM.MA_GDICH = txtSoGiaoDich.Text);
                objKUOCVMDS.DSACH_KHE_UOC = lstDSachKUOCVM.ToArray();
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId = lstDSachKUOCVM.Select(f => f.KUOC_VM.ID).ToList();
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01,
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
                DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01,
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
            iret = new TinDungProcess().LapKheUocDanhSach_01(DatabaseConstant.Action.XOA, ref objKUOCVMDS, ref lstResponseDetail);
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
                lstDSachKUOCVM = raddgrTUngCT.ItemsSource as List<TDVM_KHE_UOC>;
                objKUOCVMDS.MA_GDICH = txtSoGiaoDich.Text;
                if (lstDSachKUOCVM.Count < 1)
                    return;
                lstDSachKUOCVM.ForEach(f => f.KUOC_VM.MA_GDICH = txtSoGiaoDich.Text);
                objKUOCVMDS.DSACH_KHE_UOC = lstDSachKUOCVM.ToArray();
                Cursor = Cursors.Wait;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId = lstDSachKUOCVM.Select(f => f.KUOC_VM.ID).ToList();
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01,
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
                DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01,
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
            iret = new TinDungProcess().LapKheUocDanhSach_01(DatabaseConstant.Action.DUYET, ref objKUOCVMDS, ref lstResponseDetail);
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
            DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01,
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
                lstDSachKUOCVM = raddgrTUngCT.ItemsSource as List<TDVM_KHE_UOC>;
                objKUOCVMDS.MA_GDICH = txtSoGiaoDich.Text;
                if (lstDSachKUOCVM.Count < 1)
                    return;
                lstDSachKUOCVM.ForEach(f => f.KUOC_VM.MA_GDICH = txtSoGiaoDich.Text);
                objKUOCVMDS.DSACH_KHE_UOC = lstDSachKUOCVM.ToArray();
                Cursor = Cursors.Wait;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId = lstDSachKUOCVM.Select(f => f.KUOC_VM.ID).ToList();
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01,
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
                DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01,
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
            iret = new TinDungProcess().LapKheUocDanhSach_01(DatabaseConstant.Action.TU_CHOI_DUYET, ref objKUOCVMDS, ref lstResponseDetail);
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
            DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01,
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
                lstDSachKUOCVM = raddgrTUngCT.ItemsSource as List<TDVM_KHE_UOC>;
                objKUOCVMDS.MA_GDICH = txtSoGiaoDich.Text;
                if (lstDSachKUOCVM.Count < 1)
                    return;
                lstDSachKUOCVM.ForEach(f => f.KUOC_VM.MA_GDICH = txtSoGiaoDich.Text);
                objKUOCVMDS.DSACH_KHE_UOC = lstDSachKUOCVM.ToArray();
                Cursor = Cursors.Wait;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId = lstDSachKUOCVM.Select(f => f.KUOC_VM.ID).ToList();
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.THOAI_DUYET,
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
                DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01,
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
            iret = new TinDungProcess().LapKheUocDanhSach_01(DatabaseConstant.Action.THOAI_DUYET, ref objKUOCVMDS, ref lstResponseDetail);
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
            DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.THOAI_DUYET,
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
                        objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01;
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
                        objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01;
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
                        objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01;
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

        private void OnPreviewHopDongVayVon()
        {
            try
            {
                // Cảnh báo khi không có dữ liệu
                if (LObject.IsNullOrEmpty(txtSoGiaoDich.Text))
                {
                    LMessage.ShowMessage("M.TinDungTT.KheUoc.ucKheUocCT.KhongCoThongTinKheUoc", LMessage.MessageBoxType.Warning);
                    return;
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

                        lstThamSo.Add(new ThamSoBaoCao("@MaPhongGD", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("@MaGiaoDich", txtSoGiaoDich.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                        string maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.TDVM_HOP_DONG_VAY_VON);
                        xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                    }
                    else if (ClientInformation.Company.Equals("BENTRE"))
                    {
                        VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();

                        List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                        lstThamSo.Add(new ThamSoBaoCao("@MaPhongGD", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("@MaGiaoDich", txtSoGiaoDich.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                        string maBaoCao = DatabaseConstant.LayMaBaoCaoBenTre(DatabaseConstant.DanhSachBaoCaoBenTre.TDVM_HOP_DONG_VAY_VON_NHOM_BAO_LANH);
                        xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                    }
                    else
                    {
                        VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();

                        List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                        lstThamSo.Add(new ThamSoBaoCao("@MaPhongGD", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("@MaGiaoDich", txtSoGiaoDich.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                        string maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.TDVM_HOP_DONG_VAY_VON);
                        xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
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
            }
        }

        private void OnPreviewPhuLucHopDong()
        {
            try
            {
                // Cảnh báo khi không có dữ liệu
                if (LObject.IsNullOrEmpty(txtSoGiaoDich.Text))
                {
                    LMessage.ShowMessage("M.TinDungTT.KheUoc.ucKheUocCT.KhongCoThongTinKheUoc", LMessage.MessageBoxType.Warning);
                    return;
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

                        lstThamSo.Add(new ThamSoBaoCao("@MaGiaoDich", txtSoGiaoDich.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("@NgayGiaoVon", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("@MaPhongGD", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("P_NGAY_GIAO_VON", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                        string maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.TDVM_PHU_LUC_HOP_DONG);
                        xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                    }
                    else
                    {
                        VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();

                        List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                        lstThamSo.Add(new ThamSoBaoCao("@MaGiaoDich", txtSoGiaoDich.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("@NgayGiaoVon", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("@MaPhongGD", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("P_NGAY_GIAO_VON", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                        string maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.TDVM_PHU_LUC_HOP_DONG);
                        xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
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
            }
        }

        private void AfterOperation()
        {

        }

        private void SetInfomation()
        {
            lstDSachKUOCVM = objKUOCVMDS.DSACH_KHE_UOC.Where(e => e.KUOC_VM.ID > 0).ToList();
            objKUOCVMDS.DSACH_KHE_UOC = lstDSachKUOCVM.ToArray();
            txtSoGiaoDich.Text = objKUOCVMDS.MA_GDICH;
            if (lstDSachKUOCVM.Count > 0)
            {
                idKheUoc = lstDSachKUOCVM[0].KUOC_VM.ID;
                TThaiNVu = lstDSachKUOCVM[0].KUOC_VM.TTHAI_NVU;
            }
            else
            {
                idKheUoc = 0;
                TThaiNVu = "";
            }
            lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
            LoadGridViewDSKheUoc();
            action = DatabaseConstant.Action.XEM;
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01);
            tlbDetailAdd.IsEnabled = false;
            tlbDetailDelete.IsEnabled = false;
            SetEnabledAllControl(false);
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
