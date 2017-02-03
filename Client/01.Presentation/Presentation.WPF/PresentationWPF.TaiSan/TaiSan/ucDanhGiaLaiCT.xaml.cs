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
using PresentationWPF.CustomControl;
using Telerik.Windows.Controls;
using System.Data;
using Utilities.Common;
using Presentation.Process.Common;
using System.Collections;
using System.Reflection;
using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.TaiSanServiceRef;
using Telerik.Windows.Controls.GridView;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;

namespace PresentationWPF.TaiSan.TaiSan
{
    /// <summary>
    /// Interaction logic for ucDanhGiaLaiCT.xaml
    /// </summary>
    public partial class ucDanhGiaLaiCT : UserControl
    {
        /// <summary>
        /// Khai bao
        /// </summary>
        #region Khai bao
        public static RoutedCommand AddCommand = new RoutedCommand();
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
        public event EventHandler OnSavingCompleted;
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        string TThaiNVu = "";
        public int idCT = 0;
        int idTS = 0;
        int thoiGianKH = 0;
        string maDonViSD = "";
        DIEU_KIEN_TIM_KIEM_DTO dieuKien = new DIEU_KIEN_TIM_KIEM_DTO();
        DANH_GIA_DTO obj = new DANH_GIA_DTO();
        List<DANH_SACH_DANH_GIA_DTO> danhSachDto = new List<DANH_SACH_DANH_GIA_DTO>();
        List<AutoCompleteEntry> lstSourceNguoiQD = new List<AutoCompleteEntry>();

        public DatabaseConstant.Action Action;
        public DatabaseConstant.Function Function;
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucDanhGiaLaiCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TaiSan;component/TaiSan/ucDanhGiaLaiCT.xaml", ref Toolbar, ref mnuMain);
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
            titemThongTinChung.Focus();
            // Lần đầu không cho chọn nguyên nhân thay đổi
        }

        public ucDanhGiaLaiCT(string maThamChieu, DatabaseConstant.Action action)
        {
            if (!maThamChieu.IsNullOrEmptyOrSpace())
                idCT = maThamChieu.Split('.')[1].StringToInt32();
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TaiSan;component/TaiSan/ucDanhGiaLaiCT.xaml", ref Toolbar, ref mnuMain);
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
            Action = action;
            titemThongTinChung.Focus();
        }

        void KhoiTaoGiaTriChoComboBox()
        {
            try
            {
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();
                //Load combobox cán bộ quản lý - Tab thông tin chung - Group thông tin khác
                lstDieuKien.Clear();
                lstDieuKien.Add("%");
                lstDieuKien.Add(BusinessConstant.LOAI_HO_SO.THOI_VIEC.layGiaTri());
                lstDieuKien.Add(ClientInformation.MaDonVi);
                auto.GenAutoComboBox(ref lstSourceNguoiQD, ref cmbNguoiQD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NGUOI_BAN_GIAO.getValue(), lstDieuKien);
                cmbNguoiQD.SelectedIndex = 0;

                List<DatabaseConstant.LOAI_DMUC_TSAN> lstDanhMucLoai = new List<DatabaseConstant.LOAI_DMUC_TSAN>();
                lstDanhMucLoai.Add(DatabaseConstant.LOAI_DMUC_TSAN.NN_DANH_GIA_LAI);
                lstDanhMucLoai.Add(DatabaseConstant.LOAI_DMUC_TSAN.LOAI_DANH_GIA);
                List<DMUC_TSAN_DTO> lstDanhMucDto = new List<DMUC_TSAN_DTO>();
                TaiSanProcess process = new TaiSanProcess();
                if (process.LayDanhMucTaiSanTheoLoai(ref lstDanhMucDto, lstDanhMucLoai))
                {
                    foreach (DMUC_TSAN_DTO item in lstDanhMucDto)
                    {
                        if (item.MaLoai == DatabaseConstant.LOAI_DMUC_TSAN.NN_DANH_GIA_LAI)
                        {
                            foreach (TS_DM_DMUC_GTRI itemDanhMuc in item.LstObj)
                            {
                                cmbNguyenNhan.Items.Add(new AutoCompleteEntry(itemDanhMuc.TEN_DMUC, itemDanhMuc.MA_DMUC, itemDanhMuc.MA_DMUC_LOAI));
                            }
                            cmbNguyenNhan.SelectedIndex = 0; continue;
                        }
                        if (item.MaLoai == DatabaseConstant.LOAI_DMUC_TSAN.LOAI_DANH_GIA)
                        {
                            foreach (TS_DM_DMUC_GTRI itemDanhMuc in item.LstObj)
                            {
                                cmbLoaiDG.Items.Add(new AutoCompleteEntry(itemDanhMuc.TEN_DMUC, itemDanhMuc.MA_DMUC, itemDanhMuc.MA_DMUC_LOAI));
                            }
                            cmbLoaiDG.SelectedIndex = 0; continue;
                        }
                    }
                }
                else
                    LMessage.ShowMessage(LLanguage.SearchResourceByKey(ApplicationConstant.NghiepVuResponseMessage.M_ResponseMessage_DungChung_LoiKhongXacDinh.layGiaTri()), LMessage.MessageBoxType.Error);

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void InitEventHandler()
        {
            txtMaTS.KeyDown += new KeyEventHandler(txtMaTS_KeyDown);
            btnMaTS.Click += new RoutedEventHandler(btnMaTS_Click);

            cmbNguyenNhan.SelectionChanged += cmbNguyenNhan_SelectionChanged;
            cmbLoaiDG.SelectionChanged += cmbLoaiDG_SelectionChanged;
        }

        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.TaiSan.TaiSan.ucTaiSanCT", "RibbonButton");
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
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.N, ModifierKeys.Control);
                        key = new KeyBinding(AddCommand, keyg);
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
                        KeyGesture keyg = new KeyGesture(Key.D, ModifierKeys.Control);
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
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
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift);
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

        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbAdd.IsEnabled;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ResetData();
            SetEnabledAllControl(true);
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbModify.IsEnabled;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Modify();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbDelete.IsEnabled;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeDelete();
        }

        private void CloneCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CloneCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Nhân bản dữ liệu");
        }

        //private void HoldCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        //{
        //    e.CanExecute = true;
        //}
        //private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        //{
        //    BeforeSave(BusinessConstant.TrangThaiNghiepVu.LUU_TAM, BusinessConstant.TrangThaiBanGhi.SU_DUNG);
        //}

        private void SubmitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbSubmit.IsEnabled;
        }
        private void SubmitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeSave(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET, BusinessConstant.TrangThaiBanGhi.SU_DUNG);
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbSubmit.IsEnabled;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeSave(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET, BusinessConstant.TrangThaiBanGhi.SU_DUNG);
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
            e.CanExecute = tlbApprove.IsEnabled;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeApprove();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeCancel();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbRefuse.IsEnabled;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeRefuse();
        }

        private void PreviewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void PreviewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnPreviewChungTu();
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
                ResetData();
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
            else if (strTinhNang.Equals("PreviewChungTu"))
            {
                OnPreviewChungTu();
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
                ResetData();
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
            else if (strTinhNang.Equals("PreviewChungTu"))
            {
                OnPreviewChungTu();
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

        /// <summary>
        /// Sự kiện load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Function = DatabaseConstant.Function.TS_DANH_GIA;

            raddtNgayQD.Value = ClientInformation.NgayLamViecHienTai.StringToDate("yyyyMMdd");

            //Hiển thị Form khi xem dữ liệu
            if (Action == DatabaseConstant.Action.XEM || Action == DatabaseConstant.Action.SUA)
            {
                if (obj.ObjDanhGia.IsNullOrEmpty()) obj.ObjDanhGia = new TS_DANH_GIA();
                obj.ObjDanhGia.ID = idCT;
                SetDataForm();
                // Refresh buttons
                LLogging.WriteLog("Xem", LLogging.LogType.BUS, DateTime.Now.Subtract(DateTime.Now).TotalMilliseconds.ToString());
            }
            CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_DANH_GIA);
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

        /// <summary>
        /// Hàm xử lý giải phóng lock dữ liệu cho form
        /// </summary>
        private void Release()
        {
            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(idCT);

            bool ret = process.UnlockData(DatabaseConstant.Module.QLTS,
                Function,
                DatabaseConstant.Table.TS_DANH_GIA,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        void ClearForm()
        {
            lblTrangThai.Content = "";
            txtSoQD.Text = "";
            cmbNguoiQD.SelectedIndex = 0;
            raddtNgayQD.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtMaTS.Text = "";
            txtTenTS.Text = "";
            txtThoiGianConLai.Text = "";
            numNguyenGia.Value = 0;
            numGiaTriConLai.Value = 0;
            cmbNguyenNhan.SelectedIndex = 0;
            cmbLoaiDG.SelectedIndex = 0;
            numNguyenGiaMoi.Value = 0;
            numKhauHaoMoi.Value = 0;
            numGiaTriConLaiMoi.Value = 0;
            numThoiGianMoi.Value = 0;
            numChenhLechGT.Value = 0;
            numChenhLechKH.Value = 0;
            numChenhLechNG.Value = 0;
            numChenhLechTG.Value = 0;
            txtDienGiai.Text = "";
            grdPhuKien.ItemsSource = null;
            grdPhuKien.Rebind();

            //Lay lai tai khoan mac dinh

            #region Thông tin kiểm soát
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            teldtNgayCNhat.Value = null;
            txtTrangThai.Text = "";
            txtTrangThai.Text = "";
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            txtNguoiCapNhat.Text = "";
            #endregion

            Action = DatabaseConstant.Action.THEM;
            CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_DANH_GIA);
        }

        private void ResetData()
        {
            Action = DatabaseConstant.Action.THEM;
            idCT = 0;

            ClearForm();
        }

        private void cmbNguyenNhan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetPhuKien(false);
        }

        private void cmbLoaiDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TinhChenhLech();
        }

        private void num_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            //if (numNguyenGia.Value > numNguyenGiaMoi.Value && ((AutoCompleteEntry)cmbLoaiDG.SelectedItem).KeywordStrings.First().Equals(BusinessConstant.LOAI_DANH_GIA_TAI_SAN.TANG.layGiaTri()))
            //{
            //    LMessage.ShowMessage("Nguyên giá mới không được nhỏ hơn nguyên giá trong đánh giá tăng tài sản.", LMessage.MessageBoxType.Warning);
            //    numNguyenGiaMoi.Focus();
            //}
            //else if (numNguyenGia.Value < numNguyenGiaMoi.Value && ((AutoCompleteEntry)cmbLoaiDG.SelectedItem).KeywordStrings.First().Equals(BusinessConstant.LOAI_DANH_GIA_TAI_SAN.GIAM.layGiaTri()))
            //{
            //    LMessage.ShowMessage("Nguyên giá mới không được lớn hơn nguyên giá trong đánh giá giảm tài sản.", LMessage.MessageBoxType.Warning);
            //    numNguyenGiaMoi.Focus();
            //}
            //else
            TinhChenhLech();
        }

        private void SetEnabledAllControl(bool bBool)
        {
            txtSoQD.IsEnabled = bBool;
            raddtNgayQD.IsEnabled = bBool;
            dtpNgayQD.IsEnabled = bBool;
            cmbNguoiQD.IsEnabled = bBool;
            btnMaTS.IsEnabled = bBool;
            cmbNguyenNhan.IsEnabled = bBool;
            numNguyenGiaMoi.IsEnabled = bBool;
            numKhauHaoMoi.IsEnabled = bBool;
            cmbLoaiDG.IsEnabled = bBool;
            numGiaTriConLaiMoi.IsEnabled = bBool;
            numThoiGianMoi.IsEnabled = bBool;
            txtDienGiai.IsEnabled = bBool;
            SetPhuKien(bBool);
        }

        void SetPhuKien(bool bBool)
        {
            if (((AutoCompleteEntry)cmbNguyenNhan.SelectedItem).KeywordStrings.First().Equals(BusinessConstant.NGUYEN_NHAN_DANH_GIA_TAI_SAN.THAO_DO_1_PHAN.layGiaTri()))
                bBool = true;
            btnAddPhuKien.IsEnabled = bBool;
            btnCommitPhuKien.IsEnabled = bBool;
            btnCancelPhuKien.IsEnabled = bBool;
            btnDeletePhuKien.IsEnabled = bBool;
        }

        void TinhChenhLech()
        {
            if (!numNguyenGiaMoi.Value.IsNullOrEmpty())
                numChenhLechNG.Value = Math.Abs((numNguyenGia.Value - numNguyenGiaMoi.Value).Value);
            if (!numKhauHaoMoi.Value.IsNullOrEmpty())
                numChenhLechKH.Value = Math.Abs((numKhauHaoLuyKe.Value - numKhauHaoMoi.Value).Value);
            if (!numThoiGianMoi.Value.IsNullOrEmpty())
                numChenhLechTG.Value = Math.Abs((txtThoiGianConLai.Text.StringToDouble() - numThoiGianMoi.Value).Value);
            numGiaTriConLaiMoi.Value = Math.Abs((numNguyenGiaMoi.Value - numKhauHaoMoi.Value).Value);
            numChenhLechGT.Value = Math.Abs((numGiaTriConLaiMoi.Value - numGiaTriConLai.Value).Value);
        }

        private void btnMaTS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                var process = new PopupProcess();
                lstPopup.Clear();
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_TAI_SAN.getValue(), lstDieuKien);

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow r = lstPopup.First();
                    numNguyenGia.Value = r["TONG_NGUYEN_GIA"].ToString().StringToDouble();
                    numNguyenGiaMoi.Value = r["TONG_NGUYEN_GIA"].ToString().StringToDouble();
                    numKhauHaoLuyKe.Value = r["KH_LUY_KE"].ToString().StringToDouble();
                    numKhauHaoMoi.Value = r["KH_LUY_KE"].ToString().StringToDouble();
                    txtThoiGianConLai.Text = r["THOI_GIAN_CON_LAI"].ToString();
                    numThoiGianMoi.Value = r["THOI_GIAN_CON_LAI"].ToString().StringToDouble();
                    numGiaTriConLai.Value = r["GIA_TRI_CON_LAI"].ToString().StringToDouble();
                    numGiaTriConLaiMoi.Value = r["GIA_TRI_CON_LAI"].ToString().StringToDouble();
                    txtTenTS.Text = r["TEN_TAI_SAN"].ToString();
                    txtMaTS.Text = r["MA_TAI_SAN"].ToString();
                    numChenhLechNG.Value = Math.Abs((numNguyenGia.Value - numNguyenGiaMoi.Value).Value);
                    numChenhLechKH.Value = Math.Abs((numKhauHaoLuyKe.Value - numKhauHaoMoi.Value).Value);
                    numChenhLechTG.Value = Math.Abs((txtThoiGianConLai.Text.StringToDouble() - numThoiGianMoi.Value).Value);
                    idTS = Convert.ToInt32(r["ID"]);
                    thoiGianKH = r["THOI_GIAN_KH"].ToString().StringToInt32();
                    maDonViSD = r["DVI_SDUNG"].ToString();

                    TS_TAI_SAN_DTO objTSDto = new TS_TAI_SAN_DTO();
                    objTSDto.TS_TAI_SAN = new TS_TAI_SAN();
                    objTSDto.TS_TAI_SAN.ID = idTS;
                    List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                    TaiSanProcess processTS = new TaiSanProcess();
                    List<int> lstID = new List<int>();
                    if (processTS.ThongTinTaiSan(DatabaseConstant.Function.TS_TAI_SAN, DatabaseConstant.Action.XEM, lstID, ref objTSDto, ref listClientResponseDetail))
                    {
                        grdPhuKien.ItemsSource = objTSDto.Lst_TS_PHU_KIEN.ToList();
                        grdPhuKien.Rebind();
                    }
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void txtMaTS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnMaTS_Click(null, null);
            }
        }

        void resetThongTinNangCap()
        {
            txtSoQD.Text = string.Empty;
            cmbNguoiQD.SelectedIndex = 0;
            txtMaTS.Text = string.Empty;
            txtTenTS.Text = string.Empty;
            numNguyenGia.Value = 0;
            numKhauHaoLuyKe.Value = 0;
            cmbNguyenNhan.SelectedIndex = 0;
            numChenhLechNG.Value = 0;
            numNguyenGiaMoi.Value = 0;
            numChenhLechKH.Value = 0;
            numKhauHaoMoi.Value = 0;
            cmbLoaiDG.SelectedIndex = 0;
            numGiaTriConLaiMoi.Value = 0;
            numChenhLechGT.Value = 0;
            numThoiGianMoi.Value = 0;
            numChenhLechTG.Value = 0;
            txtDienGiai.Text = string.Empty;
        }

        private void ucDonViTinh_EditCellEnd(object sender, EventArgs e)
        {
            TS_DANH_GIA_PK objPhuKien = ucDonViTinh.cellEdit.ParentRow.Item as TS_DANH_GIA_PK;
            objPhuKien.DON_VI_TINH = ucDonViTinh.GiaTri;
            obj.LstPhuKien.ElementAt(obj.LstPhuKien.ToList().IndexOf(objPhuKien)).DON_VI_TINH = ucDonViTinh.GiaTri;
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
                if (obj.ObjDanhGia.IsNullOrEmpty())
                {
                    obj = new DANH_GIA_DTO();
                    obj.ObjDanhGia = new TS_DANH_GIA();
                }
                obj.ObjDanhGia.DVI_SDUNG = maDonViSD;
                obj.ObjDanhGia.SO_QUYET_DINH = txtSoQD.Text;
                obj.ObjDanhGia.NGAY_QUYET_DINH = ((DateTime)raddtNgayQD.Value).DateToString("yyyyMMdd");
                obj.ObjDanhGia.NGUOI_QUYET_DINH = ((AutoCompleteEntry)cmbNguoiQD.SelectedItem).KeywordStrings.First();
                //obj.ObjDanhGia.CVU_NGUOI_QD
                //obj.ObjDanhGia.SO_GIAO_DICH = txtMaGD.Text;
                obj.ObjDanhGia.ID_TAI_SAN = idTS;
                obj.ObjDanhGia.MA_TAI_SAN = txtMaTS.Text;
                obj.ObjDanhGia.TEN_TAI_SAN = txtTenTS.Text;
                obj.ObjDanhGia.NGUYEN_NHAN_DG = ((AutoCompleteEntry)cmbNguyenNhan.SelectedItem).KeywordStrings.First();
                obj.ObjDanhGia.LOAI_DG = ((AutoCompleteEntry)cmbLoaiDG.SelectedItem).KeywordStrings.First();
                obj.ObjDanhGia.NGUYEN_GIA = Convert.ToDecimal(numNguyenGia.Value);
                obj.ObjDanhGia.CHENH_LECH_NG = Convert.ToDecimal(numChenhLechNG.Value);
                obj.ObjDanhGia.THOI_GIAN_KH = thoiGianKH;
                obj.ObjDanhGia.KHAU_HAO_LUY_KE = Convert.ToDecimal(numKhauHaoLuyKe.Value);
                obj.ObjDanhGia.CHENH_LECH_KH = Convert.ToDecimal(numChenhLechKH.Value);
                //obj.ObjDanhGia.NGUYEN_GIA_MOI=
                obj.ObjDanhGia.GIA_TRI_CON_LAI = Convert.ToDecimal(numGiaTriConLai.Value);
                obj.ObjDanhGia.CHENH_LECH_GTRI = Convert.ToDecimal(numChenhLechGT.Value);
                //obj.ObjDanhGia.SO_KY_DA_KHAU_HAO
                obj.ObjDanhGia.THOI_GIAN_CON_LAI = Convert.ToInt32(txtThoiGianConLai.Text);
                obj.ObjDanhGia.CHENH_LECH_TGIAN = Convert.ToInt32(numChenhLechTG.Value);
                obj.ObjDanhGia.DIEN_GIAI = txtDienGiai.Text;

                List<TS_DANH_GIA_PK> lstPhuKien = new List<TS_DANH_GIA_PK>();
                foreach (TS_DANH_GIA_PK row in grdPhuKien.Items)
                {
                    lstPhuKien.Add(row);
                }
                obj.LstPhuKien = lstPhuKien.ToArray();

                if (idCT > 0)
                {
                    obj.ObjDanhGia.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    obj.ObjDanhGia.TTHAI_NVU = nghiepvu.layGiaTri();
                    obj.ObjDanhGia.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    obj.ObjDanhGia.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }
                else if (idCT == 0)
                {
                    obj.ObjDanhGia.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    obj.ObjDanhGia.TTHAI_NVU = nghiepvu.layGiaTri();
                    obj.ObjDanhGia.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    obj.ObjDanhGia.NGUOI_NHAP = ClientInformation.TenDangNhap;
                }
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
                // Do something
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                TaiSanProcess process = new TaiSanProcess();
                List<int> lstID = new List<int>();
                bool ref1 = false;
                ref1 = process.DanhGiaTaiSan(DatabaseConstant.Function.TS_DANH_GIA, DatabaseConstant.Action.XEM, lstID, dieuKien, ref obj, ref danhSachDto, ref listClientResponseDetail);
                if (ref1)
                {
                    maDonViSD = obj.ObjDanhGia.DVI_SDUNG;
                    txtSoQD.Text = obj.ObjDanhGia.SO_QUYET_DINH;
                    raddtNgayQD.Value = obj.ObjDanhGia.NGAY_QUYET_DINH.StringToDate("yyyyMMdd");
                    foreach (var item in cmbNguoiQD.Items)
                    {
                        AutoCompleteEntry entry = (AutoCompleteEntry)item;
                        if (entry.KeywordStrings.First().Equals(obj.ObjDanhGia.NGUOI_QUYET_DINH))
                        { cmbNguoiQD.SelectedItem = item; break; }
                    }
                    //obj.ObjDanhGia.CVU_NGUOI_QD ;
                    //txtMaGD.Text = obj.ObjDanhGia.SO_GIAO_DICH;
                    idTS = obj.ObjDanhGia.ID_TAI_SAN;
                    txtMaTS.Text = obj.ObjDanhGia.MA_TAI_SAN;
                    txtTenTS.Text = obj.ObjDanhGia.TEN_TAI_SAN;
                    foreach (var item in cmbNguyenNhan.Items)
                    {
                        AutoCompleteEntry entry = (AutoCompleteEntry)item;
                        if (entry.KeywordStrings.First().Equals(obj.ObjDanhGia.NGUYEN_NHAN_DG))
                        { cmbNguyenNhan.SelectedItem = item; break; }
                    }
                    foreach (var item in cmbLoaiDG.Items)
                    {
                        AutoCompleteEntry entry = (AutoCompleteEntry)item;
                        if (entry.KeywordStrings.First().Equals(obj.ObjDanhGia.LOAI_DG))
                        { cmbLoaiDG.SelectedItem = item; break; }
                    }

                    thoiGianKH = obj.ObjDanhGia.THOI_GIAN_KH.Value;
                    numNguyenGia.Value = Convert.ToDouble(obj.ObjDanhGia.NGUYEN_GIA);
                    numChenhLechNG.Value = Convert.ToDouble(obj.ObjDanhGia.CHENH_LECH_NG);
                    numKhauHaoLuyKe.Value = Convert.ToDouble(obj.ObjDanhGia.KHAU_HAO_LUY_KE);
                    numChenhLechKH.Value = Convert.ToDouble(obj.ObjDanhGia.CHENH_LECH_KH);
                    numGiaTriConLai.Value = Convert.ToDouble(obj.ObjDanhGia.GIA_TRI_CON_LAI);
                    numChenhLechGT.Value = Convert.ToDouble(obj.ObjDanhGia.CHENH_LECH_GTRI);
                    txtThoiGianConLai.Text = obj.ObjDanhGia.THOI_GIAN_CON_LAI.ToString();
                    numChenhLechTG.Value = obj.ObjDanhGia.CHENH_LECH_TGIAN;
                    if (obj.ObjDanhGia.LOAI_DG.Equals(BusinessConstant.LOAI_DANH_GIA_TAI_SAN.TANG.layGiaTri()))
                    {
                        numNguyenGiaMoi.Value = numNguyenGia.Value + numChenhLechNG.Value;
                        numKhauHaoMoi.Value = numKhauHaoLuyKe.Value + numChenhLechKH.Value;
                        numGiaTriConLaiMoi.Value = numGiaTriConLai.Value + numChenhLechGT.Value;
                        numThoiGianMoi.Value = txtThoiGianConLai.Text.StringToDouble() + numChenhLechTG.Value;
                    }
                    else
                    {
                        numNguyenGiaMoi.Value = numNguyenGia.Value - numChenhLechNG.Value;
                        numKhauHaoMoi.Value = numKhauHaoLuyKe.Value - numChenhLechKH.Value;
                        numGiaTriConLaiMoi.Value = numGiaTriConLai.Value - numChenhLechGT.Value;
                        numThoiGianMoi.Value = txtThoiGianConLai.Text.StringToDouble() - numChenhLechTG.Value;
                    }

                    txtDienGiai.Text = obj.ObjDanhGia.DIEN_GIAI;

                    grdPhuKien.ItemsSource = obj.LstPhuKien;
                    SetPhuKien(false);

                    TThaiNVu = obj.ObjDanhGia.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(obj.ObjDanhGia.TTHAI_NVU);

                    #region Tab thông tin kiểm soát
                    txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(obj.ObjDanhGia.TTHAI_BGHI);
                    teldtNgayNhap.Value = LDateTime.StringToDate(obj.ObjDanhGia.NGAY_NHAP, "yyyyMMdd");
                    txtNguoiLap.Text = obj.ObjDanhGia.NGUOI_NHAP;
                    if (LDateTime.IsDate(obj.ObjDanhGia.NGAY_CNHAT, "yyyyMMdd") == true)
                        teldtNgayCNhat.Value = LDateTime.StringToDate(obj.ObjDanhGia.NGAY_CNHAT, "yyyyMMdd");
                    else
                        teldtNgayCNhat.Value = null;
                    txtNguoiCapNhat.Text = obj.ObjDanhGia.NGUOI_CNHAT;
                    #endregion

                    if (!Action.Equals(DatabaseConstant.Action.SUA))
                    {
                        CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_DANH_GIA);
                        SetEnabledAllControl(false);
                    }
                    else
                    {
                        if (TThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                        {
                            TThaiNVu = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                            SetEnabledAllControl(false);
                        }
                        else
                            Modify();
                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        bool Validation()
        {
            bool bReturn = true;
            string loaiGD = ((AutoCompleteEntry)cmbLoaiDG.SelectedItem).KeywordStrings.First();
            try
            {
                if (txtSoQD.Text.IsNullOrEmpty())
                {
                    LMessage.ShowMessage("Thiếu số quyết định", LMessage.MessageBoxType.Warning);
                    txtSoQD.Focus();
                    return false;
                }
                else if (raddtNgayQD.Value.IsNullOrEmpty())
                {
                    LMessage.ShowMessage("Thiếu ngày quyết định", LMessage.MessageBoxType.Warning);
                    raddtNgayQD.Focus();
                    return false;
                }
                else if (cmbNguoiQD.Text.IsNullOrEmptyOrSpace())
                {
                    LMessage.ShowMessage("Thiếu người quyết định", LMessage.MessageBoxType.Warning);
                    cmbNguoiQD.Focus();
                    return false;
                }
                else if (txtMaTS.Text.IsNullOrEmptyOrSpace())
                {
                    LMessage.ShowMessage("Chưa chọn tài sản", LMessage.MessageBoxType.Warning);
                    txtMaTS.Focus();
                    return false;
                }
                else if (numNguyenGia.Value == numNguyenGiaMoi.Value && numKhauHaoLuyKe.Value == numKhauHaoMoi.Value && txtThoiGianConLai.Text.StringToDouble() == (numThoiGianMoi.Value))
                {
                    LMessage.ShowMessage("Thông tin giao dịch không có gì thay đổi", LMessage.MessageBoxType.Warning);
                    numNguyenGiaMoi.Focus();
                    return false;
                }
                else if (numNguyenGia.Value < numKhauHaoMoi.Value)
                {
                    LMessage.ShowMessage("Khấu hao mới không được lớn hơn " + numNguyenGia.Value, LMessage.MessageBoxType.Warning);
                    numKhauHaoMoi.Focus();
                    return false;
                }
                else if (loaiGD.Equals(BusinessConstant.LOAI_DANH_GIA_TAI_SAN.TANG.layGiaTri()) && numNguyenGia.Value > numNguyenGiaMoi.Value)
                {
                    LMessage.ShowMessage("Nguyên giá mới không được nhỏ hơn " + numNguyenGia.Value, LMessage.MessageBoxType.Warning);
                    numNguyenGiaMoi.Focus();
                    return false;
                }
                else if (loaiGD.Equals(BusinessConstant.LOAI_DANH_GIA_TAI_SAN.TANG.layGiaTri()) && numKhauHaoLuyKe.Value > numKhauHaoMoi.Value)
                {
                    LMessage.ShowMessage("Khấu hao lũy kế mới không được nhỏ hơn " + numKhauHaoLuyKe.Value, LMessage.MessageBoxType.Warning);
                    numKhauHaoMoi.Focus();
                    return false;
                }
                else if (loaiGD.Equals(BusinessConstant.LOAI_DANH_GIA_TAI_SAN.TANG.layGiaTri()) && txtThoiGianConLai.Text.StringToDouble() > numThoiGianMoi.Value)
                {
                    LMessage.ShowMessage("Thời gian còn lại mới không được nhỏ hơn " + txtThoiGianConLai.Text, LMessage.MessageBoxType.Warning);
                    numThoiGianMoi.Focus();
                    return false;
                }
                else if (loaiGD.Equals(BusinessConstant.LOAI_DANH_GIA_TAI_SAN.GIAM.layGiaTri()) && numNguyenGia.Value < numNguyenGiaMoi.Value)
                {
                    LMessage.ShowMessage("Nguyên giá mới không được lớn hơn " + numNguyenGia.Value, LMessage.MessageBoxType.Warning);
                    numNguyenGiaMoi.Focus();
                    return false;
                }
                else if (loaiGD.Equals(BusinessConstant.LOAI_DANH_GIA_TAI_SAN.GIAM.layGiaTri()) && numKhauHaoLuyKe.Value < numKhauHaoMoi.Value)
                {
                    LMessage.ShowMessage("Khấu hao lũy kế mới không được lớn hơn " + numKhauHaoLuyKe.Value, LMessage.MessageBoxType.Warning);
                    numKhauHaoMoi.Focus();
                    return false;
                }
                else if (loaiGD.Equals(BusinessConstant.LOAI_DANH_GIA_TAI_SAN.GIAM.layGiaTri()) && txtThoiGianConLai.Text.StringToDouble() < numThoiGianMoi.Value)
                {
                    LMessage.ShowMessage("Thời gian còn lại mới không được lớn hơn " + txtThoiGianConLai.Text, LMessage.MessageBoxType.Warning);
                    numThoiGianMoi.Focus();
                    return false;
                }
                else if (txtDienGiai.Text.IsNullOrEmptyOrSpace())
                {
                    LMessage.ShowMessage("Thiếu diễn giải", LMessage.MessageBoxType.Warning);
                    txtDienGiai.Focus();
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
            List<int> lstId = new List<int>();
            lstId.Add(idCT);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.LockData(DatabaseConstant.Module.QLTS,
            DatabaseConstant.Function.TS_DANH_GIA,
            DatabaseConstant.Table.TS_DANH_GIA,
            DatabaseConstant.Action.SUA,
            lstId);
            Action = DatabaseConstant.Action.SUA;
            CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_DANH_GIA);
            SetEnabledAllControl(true);
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
                List<int> lstID = new List<int>();
                dieuKien = new DIEU_KIEN_TIM_KIEM_DTO();
                danhSachDto = new List<DANH_SACH_DANH_GIA_DTO>();
                TaiSanProcess process = new TaiSanProcess();
                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                int iret = 0;
                if (idCT == 0)
                {
                    obj.ObjDanhGia.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    obj.ObjDanhGia.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    obj.ObjDanhGia.MA_DVI_QLY = ClientInformation.MaDonVi;
                    obj.ObjDanhGia.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    if (!process.DanhGiaTaiSan(DatabaseConstant.Function.TS_DANH_GIA, DatabaseConstant.Action.THEM, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail))
                        iret = 1;
                }
                else
                {
                    obj.ObjDanhGia.NGUOI_CNHAT = ClientInformation.NgayLamViecHienTai;
                    obj.ObjDanhGia.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    obj.ObjDanhGia.ID = idCT;
                    if (!process.DanhGiaTaiSan(DatabaseConstant.Function.TS_DANH_GIA, DatabaseConstant.Action.SUA, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail))
                        iret = 1;
                }
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
                if (iret == 0)
                    LMessage.ShowMessage("M.DungChung.LuuDuLieuThanhCong", LMessage.MessageBoxType.Information);
                else
                    CommonFunction.ThongBaoKetQua(lstResponseDetail);
                // Yêu cầu Unlock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(idCT);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_DANH_GIA,
                DatabaseConstant.Table.TS_DANH_GIA,
                DatabaseConstant.Action.SUA,
                lstId);
                Action = DatabaseConstant.Action.XEM;
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
                if (!cbMultiAdd.IsChecked.GetValueOrDefault(false))
                {
                    SetInfomation();
                }
                else
                {
                    ClearForm();
                }
                idCT = obj.ObjDanhGia.ID;
                TThaiNVu = obj.ObjDanhGia.TTHAI_NVU;
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
                if (cbMultiAdd.IsChecked == true)
                    ClearForm();
                else
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_DANH_GIA);

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
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);
                if (ret == MessageBoxResult.Yes)
                {
                    // Yêu cầu Lock dữ liệu
                    List<int> lstId = new List<int>();
                    lstId.Add(idCT);
                    UtilitiesProcess process = new UtilitiesProcess();
                    bool retLockData = process.LockData(DatabaseConstant.Module.QLTS,
                    DatabaseConstant.Function.TS_DANH_GIA,
                    DatabaseConstant.Table.TS_DANH_GIA,
                    DatabaseConstant.Action.XOA,
                    lstId);
                    OnDelete();
                }
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(idCT);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_DANH_GIA,
                DatabaseConstant.Table.TS_DANH_GIA,
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
            if (idCT != 0)
            {
                List<int> lstID = new List<int>();
                TaiSanProcess process = new TaiSanProcess();
                obj.ObjDanhGia.ID = idCT;
                if (!process.DanhGiaTaiSan(DatabaseConstant.Function.TS_DANH_GIA, DatabaseConstant.Action.XOA, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail))
                    iret = 1;
            }
            AfterDelete(lstResponseDetail, iret);
        }
        void AfterDelete(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            if (iret == 0)
                LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
            else
                CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(idCT);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
            DatabaseConstant.Function.TS_DANH_GIA,
            DatabaseConstant.Table.TS_DANH_GIA,
            DatabaseConstant.Action.XOA,
            lstId);
            Action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            Cursor = Cursors.Arrow;
            if (iret == 0) CommonFunction.CloseUserControl(this);
        }

        void BeforeApprove()
        {
            try
            {
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question);
                if (ret == MessageBoxResult.Yes)
                {
                    Cursor = Cursors.Wait;
                    // Yêu cầu Lock dữ liệu
                    List<int> lstId = new List<int>();
                    lstId.Add(idCT);
                    UtilitiesProcess process = new UtilitiesProcess();
                    bool retLockData = process.LockData(DatabaseConstant.Module.QLTS,
                    DatabaseConstant.Function.TS_DANH_GIA,
                    DatabaseConstant.Table.TS_DANH_GIA,
                    DatabaseConstant.Action.DUYET,
                    lstId);
                    OnApprove();
                }
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(idCT);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_DANH_GIA,
                DatabaseConstant.Table.TS_DANH_GIA,
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
            if (idCT != 0)
            {
                List<int> lstID = new List<int>();
                TaiSanProcess process = new TaiSanProcess();
                obj.ObjDanhGia.ID = idCT;
                if (!process.DanhGiaTaiSan(DatabaseConstant.Function.TS_DANH_GIA, DatabaseConstant.Action.DUYET, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail))
                    iret = 1;
            }
            AfterApprove(lstResponseDetail, iret);
        }
        void AfterApprove(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            if (iret == 0)
                LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);
            else
                CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(idCT);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
            DatabaseConstant.Function.TS_DANH_GIA,
            DatabaseConstant.Table.TS_DANH_GIA,
            DatabaseConstant.Action.DUYET,
            lstId);
            Action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            idCT = obj.ObjDanhGia.ID;
            TThaiNVu = obj.ObjDanhGia.TTHAI_NVU;
            lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_NANG_CAP);
            Cursor = Cursors.Arrow;
        }

        void BeforeRefuse()
        {
            try
            {
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question);
                if (ret == MessageBoxResult.Yes)
                {
                    Cursor = Cursors.Wait;
                    // Yêu cầu Lock dữ liệu
                    List<int> lstId = new List<int>();
                    lstId.Add(idCT);
                    UtilitiesProcess process = new UtilitiesProcess();
                    bool retLockData = process.LockData(DatabaseConstant.Module.QLTS,
                    DatabaseConstant.Function.TS_DANH_GIA,
                    DatabaseConstant.Table.TS_DANH_GIA,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    lstId);
                    OnRefuse();
                }
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(idCT);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_DANH_GIA,
                DatabaseConstant.Table.TS_DANH_GIA,
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
            if (idCT != 0)
            {
                List<int> lstID = new List<int>();
                TaiSanProcess process = new TaiSanProcess();
                obj.ObjDanhGia.ID = idCT;
                if (!process.DanhGiaTaiSan(DatabaseConstant.Function.TS_DANH_GIA, DatabaseConstant.Action.TU_CHOI_DUYET, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail))
                    iret = 1;
            }
            AfterRefuse(lstResponseDetail, iret);
        }
        void AfterRefuse(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(idCT);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
            DatabaseConstant.Function.TS_DANH_GIA,
            DatabaseConstant.Table.TS_DANH_GIA,
            DatabaseConstant.Action.TU_CHOI_DUYET,
            lstId);
            Action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            TThaiNVu = obj.ObjDanhGia.TTHAI_NVU;
            lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
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
                lstId.Add(idCT);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_DANH_GIA,
                DatabaseConstant.Table.TS_DANH_GIA,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                OnCancel();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(idCT);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_DANH_GIA,
                DatabaseConstant.Table.TS_DANH_GIA,
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
            if (idCT != 0)
            {
                List<int> lstID = new List<int>();
                TaiSanProcess process = new TaiSanProcess();
                obj.ObjDanhGia.ID = idCT;
                process.DanhGiaTaiSan(DatabaseConstant.Function.TS_DANH_GIA, DatabaseConstant.Action.THOAI_DUYET, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail);
            }
            AfterCancel(lstResponseDetail, iret);
        }
        void AfterCancel(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(idCT);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
            DatabaseConstant.Function.TS_DANH_GIA,
            DatabaseConstant.Table.TS_DANH_GIA,
            DatabaseConstant.Action.THOAI_DUYET,
            lstId);
            Action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            Cursor = Cursors.Arrow;
            SetInfomation();
        }

        private void OnPreviewChungTu()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(idCT))
            {
                LMessage.ShowMessage("Không có chứng từ", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                // Lấy thông tin giao dịch theo biến động
                TaiSanProcess process = new TaiSanProcess();
                BIEN_DONG_DTO objBienDongDTO = new BIEN_DONG_DTO();
                KIEM_SOAT objKiemSoat = new KIEM_SOAT();
                objBienDongDTO.Function = DatabaseConstant.Function.TS_DANH_GIA;
                objBienDongDTO.IdBienDong = idCT;

                bool ret = process.LayThongTinGiaoDich(ref objKiemSoat, objBienDongDTO);

                if (ret)
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                    DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();
                    DatabaseConstant.Function _function = DatabaseConstant.Function.KT_PHIEU_KE_TOAN;

                    GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                    objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.GDKT_IN_GIAO_DICH;
                    objGIAO_DICH_BASE.ChucNang = _function;

                    GDKT_GIAO_DICH objGDKT_GIAO_DICH = new GDKT_GIAO_DICH();
                    objGDKT_GIAO_DICH.MaGiaoDich = objKiemSoat.SO_GIAO_DICH;

                    doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                    doiTuongBaoCao.objGDKT_GIAO_DICH = objGDKT_GIAO_DICH;
                    xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
                }
                else
                {
                }
            }

        }

        private void SetInfomation()
        {
            if (!LObject.IsNullOrEmpty(obj))
            {
                txtSoQD.Text = obj.ObjDanhGia.SO_QUYET_DINH;
                //txtMaGD.Text = obj.ObjDanhGia.SO_GIAO_DICH;
                Action = DatabaseConstant.Action.XEM;
                SetEnabledAllControl(false);
                CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_DANH_GIA);
            }
            else
                SetDataForm();
        }

        #endregion
    }
}
