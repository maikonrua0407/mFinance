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
    /// Interaction logic for ucGiamCT.xaml
    /// </summary>
    public partial class ucGiamCT : UserControl
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
        string maDonViSD = "";
        DIEU_KIEN_TIM_KIEM_DTO dieuKien = new DIEU_KIEN_TIM_KIEM_DTO();
        GIAM_DTO obj = new GIAM_DTO();
        List<DANH_SACH_GIAM_DTO> danhSachDto = new List<DANH_SACH_GIAM_DTO>();
        List<AutoCompleteEntry> lstSourceNguoiQD = new List<AutoCompleteEntry>();

        public DatabaseConstant.Action Action;
        public DatabaseConstant.Function Function;
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucGiamCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TaiSan;component/TaiSan/ucGiamCT.xaml", ref Toolbar, ref mnuMain);
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
            txtSoQD.Focus();
            // Lần đầu không cho chọn nguyên nhân thay đổi
        }

        public ucGiamCT(string maThamChieu, DatabaseConstant.Action action)
        {
            if (!maThamChieu.IsNullOrEmptyOrSpace())
                idCT = maThamChieu.Split('.')[1].StringToInt32();
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TaiSan;component/TaiSan/ucGiamCT.xaml", ref Toolbar, ref mnuMain);
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
            // Lần đầu không cho chọn nguyên nhân thay đổi
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
                lstDanhMucLoai.Add(DatabaseConstant.LOAI_DMUC_TSAN.NN_GIAM_TS);
                lstDanhMucLoai.Add(DatabaseConstant.LOAI_DMUC_TSAN.HT_THANH_TOAN);
                List<DMUC_TSAN_DTO> lstDanhMucDto = new List<DMUC_TSAN_DTO>();
                TaiSanProcess process = new TaiSanProcess();
                if (process.LayDanhMucTaiSanTheoLoai(ref lstDanhMucDto, lstDanhMucLoai))
                {
                    foreach (DMUC_TSAN_DTO item in lstDanhMucDto)
                    {
                        if (item.MaLoai == DatabaseConstant.LOAI_DMUC_TSAN.NN_GIAM_TS)
                        {
                            foreach (TS_DM_DMUC_GTRI itemDanhMuc in item.LstObj)
                            {
                                cmbNguyenNhan.Items.Add(new AutoCompleteEntry(itemDanhMuc.TEN_DMUC, itemDanhMuc.MA_DMUC, itemDanhMuc.MA_DMUC_LOAI));
                            }
                            cmbNguyenNhan.SelectedIndex = 0; continue;
                        }
                        if (item.MaLoai == DatabaseConstant.LOAI_DMUC_TSAN.HT_THANH_TOAN)
                        {
                            foreach (TS_DM_DMUC_GTRI itemDanhMuc in item.LstObj)
                            {
                                cmbHinhThucTT.Items.Add(new AutoCompleteEntry(itemDanhMuc.TEN_DMUC, itemDanhMuc.MA_DMUC, itemDanhMuc.MA_DMUC_LOAI));
                                cmbHinhThucTTPhi.Items.Add(new AutoCompleteEntry(itemDanhMuc.TEN_DMUC, itemDanhMuc.MA_DMUC, itemDanhMuc.MA_DMUC_LOAI));
                            }
                            cmbHinhThucTT.SelectedIndex = 0;
                            cmbHinhThucTTPhi.SelectedIndex = 0; continue;
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

            cmbHinhThucTT.SelectionChanged += cmbHinhThucTT_SelectionChanged;
            cmbHinhThucTTPhi.SelectionChanged += cmbHinhThucTTPhi_SelectionChanged;
        }

        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.TaiSan.TaiSan.ucGiamCT", "RibbonButton");
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
                    //else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
                    //{
                    //    KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift);
                    //    key = new KeyBinding(CashStmtCommand, keyg);
                    //    key.Gesture = keyg;
                    //}
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
                        KeyGesture keyg = new KeyGesture(Key.E, ModifierKeys.Control | ModifierKeys.Shift);
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
            Function = DatabaseConstant.Function.TS_GIAM;

            raddtNgayQD.Value = ClientInformation.NgayLamViecHienTai.StringToDate("yyyyMMdd");

            bool isChuyenKhoan = false;
            if (((AutoCompleteEntry)cmbHinhThucTT.SelectedItem).KeywordStrings.First().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN.layGiaTri()))
                isChuyenKhoan = true;
            txtTKTT.IsEnabled = isChuyenKhoan;
            btnTKTT.IsEnabled = isChuyenKhoan;
            isChuyenKhoan = false;
            if (((AutoCompleteEntry)cmbHinhThucTTPhi.SelectedItem).KeywordStrings.First().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN.layGiaTri()))
                isChuyenKhoan = true;
            txtTKTTPhi.IsEnabled = isChuyenKhoan;
            btnTKTTPhi.IsEnabled = isChuyenKhoan;
            //Hiển thị Form khi xem dữ liệu
            if (Action == DatabaseConstant.Action.XEM || Action == DatabaseConstant.Action.SUA)
            {
                if (obj.ObjGiam.IsNullOrEmpty()) obj.ObjGiam = new TS_GIAM();
                obj.ObjGiam.ID = idCT;
                SetDataForm();
                // Refresh buttons
                LLogging.WriteLog("Xem", LLogging.LogType.BUS, DateTime.Now.Subtract(DateTime.Now).TotalMilliseconds.ToString());
            }
            CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_GIAM);
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
                DatabaseConstant.Table.TS_GIAM,
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
            numGiaTriConLai.Value = 0;
            numKHLuyKe.Value = 0;
            numKHThangGiam.Value = 0;
            cmbNguyenNhan.SelectedIndex = 0;
            numSoTien.Value = 0;
            cmbHinhThucTT.SelectedIndex = 0;
            numChiPhi.Value = 0;
            txtTKTT.Text = "";
            txtTKTTPhi.Text = "";
            txtDienGiai.Text = "";


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
            CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_GIAM);
        }

        private void ResetData()
        {
            Action = DatabaseConstant.Action.THEM;
            idCT = 0;

            ClearForm();
        }

        private void cmbHinhThucTT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool isChuyenKhoan = false;
            if (((AutoCompleteEntry)cmbHinhThucTT.SelectedItem).KeywordStrings.First().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN.layGiaTri()))
                isChuyenKhoan = true;
            txtTKTT.IsEnabled = isChuyenKhoan;
            btnTKTT.IsEnabled = isChuyenKhoan;
        }

        private void cmbHinhThucTTPhi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool isChuyenKhoan = false;
            if (((AutoCompleteEntry)cmbHinhThucTTPhi.SelectedItem).KeywordStrings.First().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN.layGiaTri()))
                isChuyenKhoan = true;
            txtTKTTPhi.IsEnabled = isChuyenKhoan;
            btnTKTTPhi.IsEnabled = isChuyenKhoan;
        }

        private void SetEnabledAllControl(bool bBool)
        {
            txtSoQD.IsEnabled = bBool;
            raddtNgayQD.IsEnabled = bBool;
            dtpNgayQD.IsEnabled = bBool;
            cmbNguoiQD.IsEnabled = bBool;
            btnMaTS.IsEnabled = bBool;
            cmbNguyenNhan.IsEnabled = bBool;
            numSoTien.IsEnabled = bBool;
            numChiPhi.IsEnabled = bBool;
            cmbHinhThucTT.IsEnabled = bBool;
            cmbHinhThucTTPhi.IsEnabled = bBool;
            txtTKTT.IsEnabled = bBool;
            txtTKTTPhi.IsEnabled = bBool;
            txtDienGiai.IsEnabled = bBool;
        }

        private void raddtNgayQD_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            numGiaTriConLai.Value = 0;
            numKHLuyKe.Value = 0;
            numKHThangGiam.Value = 0;
            txtThoiGianConLai.Text = string.Empty;
            txtTenTS.Text = string.Empty;
            txtMaTS.Text = string.Empty;
            idTS = 0;
        }

        private void btnMaTS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (raddtNgayQD.Value.IsNullOrEmpty())
                {
                    LMessage.ShowMessage("Phải nhập ngày quyết định trước.", LMessage.MessageBoxType.Warning);
                    numGiaTriConLai.Value = 0;
                    numKHLuyKe.Value = 0;
                    txtThoiGianConLai.Text = string.Empty;
                    txtTenTS.Text = string.Empty;
                    txtMaTS.Text = string.Empty;
                    idTS = 0;
                    raddtNgayQD.Focus();
                    return;
                }
                else
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
                        //resetThongTinGiam();
                        DataRow r = lstPopup.First();
                        numGiaTriConLai.Value = r["GIA_TRI_CON_LAI"].ToString().StringToDouble();
                        numKHLuyKe.Value = r["KH_LUY_KE"].ToString().StringToDouble();
                        txtThoiGianConLai.Text = r["THOI_GIAN_CON_LAI"].ToString();
                        txtTenTS.Text = r["TEN_TAI_SAN"].ToString();
                        txtMaTS.Text = r["MA_TAI_SAN"].ToString();
                        idTS = Convert.ToInt32(r["ID"]);
                        maDonViSD = r["DVI_SDUNG"].ToString();
                        if (obj.IsNullOrEmpty())
                            obj = new GIAM_DTO();
                        if (obj.ObjGiam.IsNullOrEmpty())
                            obj.ObjGiam = new TS_GIAM();
                        obj.ObjGiam.ID_TAI_SAN = idTS;
                        obj.ObjGiam.NGAY_QUYET_DINH = ((DateTime)raddtNgayQD.Value).DateToString("yyyyMMdd");
                        List<int> lstID = new List<int>();
                        dieuKien = new DIEU_KIEN_TIM_KIEM_DTO();
                        danhSachDto = new List<DANH_SACH_GIAM_DTO>();
                        TaiSanProcess taiSanProcess = new TaiSanProcess();
                        List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                        if (taiSanProcess.GiamTaiSan(DatabaseConstant.Function.TS_GIAM, DatabaseConstant.Action.TINH_TOAN, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail))
                            numKHThangGiam.Value = Convert.ToDouble(obj.ObjGiam.KH_THANG_GIAM);
                        else
                        {
                            CommonFunction.ThongBaoKetQua(lstResponseDetail);
                            numGiaTriConLai.Value = 0;
                            numKHLuyKe.Value = 0;
                            numKHThangGiam.Value = 0;
                            txtThoiGianConLai.Text = string.Empty;
                            txtTenTS.Text = string.Empty;
                            txtMaTS.Text = string.Empty;
                            idTS = 0;
                        }
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

        private void TaiKhoanThanhToan_Click(object sender, RoutedEventArgs e)
        {
            string tk = TaiKhoanThanhToan();
            if (!tk.IsNullOrEmptyOrSpace())
                txtTKTT.Text = tk;
        }

        private void TaiKhoanThanhToan_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void TaiKhoanThanhToan_KeyDown(object sender, KeyEventArgs e)
        {
            string tk = TaiKhoanThanhToan();
            if (!tk.IsNullOrEmptyOrSpace())
                txtTKTT.Text = tk;
        }

        private void TaiKhoanThanhToanPhi_Click(object sender, RoutedEventArgs e)
        {
            string tk = TaiKhoanThanhToan();
            if (!tk.IsNullOrEmptyOrSpace())
                txtTKTTPhi.Text = tk;
        }

        private void TaiKhoanThanhToanPhi_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void TaiKhoanThanhToanPhi_KeyDown(object sender, KeyEventArgs e)
        {
            string tk = TaiKhoanThanhToan();
            if (!tk.IsNullOrEmptyOrSpace())
                txtTKTTPhi.Text = tk;
        }

        private string TaiKhoanThanhToan()
        {
            try
            {
                string tk = "";
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                var process = new PopupProcess();
                lstPopup.Clear();
                process.getPopupInformation("POPUP_TAI_KHOAN_NOI_BO", lstDieuKien);

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow row = lstPopup[0];
                    tk = row[2].ToString();
                }
                return tk;
            }
            catch (System.Exception ex)
            {
                return null;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void resetThongTinGiam()
        {
            txtSoQD.Text = string.Empty;
            cmbNguoiQD.SelectedIndex = 0;
            txtMaTS.Text = string.Empty;
            txtTenTS.Text = string.Empty;
            numSoTien.Value = 0;
            numChiPhi.Value = 0;
            numGiaTriConLai.Value = 0;
            cmbNguyenNhan.SelectedIndex = 0;
            cmbHinhThucTT.SelectedIndex = 0;
            cmbHinhThucTTPhi.SelectedIndex = 0;
            txtTKTT.Text = string.Empty;
            txtTKTTPhi.Text = string.Empty;
            txtDienGiai.Text = string.Empty;
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
                if (obj.ObjGiam.IsNullOrEmpty())
                {
                    obj = new GIAM_DTO();
                    obj.ObjGiam = new TS_GIAM();
                }
                obj.ObjGiam.DVI_SDUNG = maDonViSD;
                obj.ObjGiam.SO_QUYET_DINH = txtSoQD.Text;
                obj.ObjGiam.NGAY_QUYET_DINH = ((DateTime)raddtNgayQD.Value).DateToString("yyyyMMdd");
                obj.ObjGiam.NGUOI_QUYET_DINH = ((AutoCompleteEntry)cmbNguoiQD.SelectedItem).KeywordStrings.First();
                //obj.ObjGiam.CVU_NGUOI_QD
                //obj.ObjGiam.SO_GIAO_DICH = txtMaGD.Text;
                obj.ObjGiam.ID_TAI_SAN = idTS;
                obj.ObjGiam.MA_TAI_SAN = txtMaTS.Text;
                obj.ObjGiam.TEN_TAI_SAN = txtTenTS.Text;
                obj.ObjGiam.KH_LUY_KE = Convert.ToDecimal(numKHLuyKe.Value);
                obj.ObjGiam.KH_THANG_GIAM = Convert.ToDecimal(numKHThangGiam.Value);
                obj.ObjGiam.NGUYEN_NHAN_GIAM = ((AutoCompleteEntry)cmbNguyenNhan.SelectedItem).KeywordStrings.First();
                obj.ObjGiam.HTHUC_TTOAN = ((AutoCompleteEntry)cmbHinhThucTT.SelectedItem).KeywordStrings.First();
                obj.ObjGiam.TKHOAN_TTOAN = txtTKTT.Text;
                obj.ObjGiam.HTHUC_TTOAN_PHI = ((AutoCompleteEntry)cmbHinhThucTTPhi.SelectedItem).KeywordStrings.First();
                obj.ObjGiam.TKHOAN_TTOAN_PHI = txtTKTTPhi.Text;
                //obj.ObjGiam.NGUYEN_GIA_MOI=
                obj.ObjGiam.GIA_TRI_CON_LAI = Convert.ToDecimal(numGiaTriConLai.Value);
                obj.ObjGiam.THOI_GIAN_CON_LAI = Convert.ToInt32(txtThoiGianConLai.Text);
                obj.ObjGiam.SO_TIEN_TONG = Convert.ToDecimal(numSoTien.Value);
                obj.ObjGiam.CHI_PHI_TONG = Convert.ToDecimal(numChiPhi.Value);
                obj.ObjGiam.THOI_GIAN_CON_LAI = Convert.ToInt32(txtThoiGianConLai.Text);
                obj.ObjGiam.DIEN_GIAI = txtDienGiai.Text;
                if (idCT > 0)
                {
                    obj.ObjGiam.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    obj.ObjGiam.TTHAI_NVU = nghiepvu.layGiaTri();
                    obj.ObjGiam.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    obj.ObjGiam.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }
                else if (idCT == 0)
                {
                    obj.ObjGiam.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    obj.ObjGiam.TTHAI_NVU = nghiepvu.layGiaTri();
                    obj.ObjGiam.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    obj.ObjGiam.NGUOI_NHAP = ClientInformation.TenDangNhap;
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
                ref1 = process.GiamTaiSan(DatabaseConstant.Function.TS_GIAM, DatabaseConstant.Action.XEM, lstID, dieuKien, ref obj, ref danhSachDto, ref listClientResponseDetail);
                if (ref1)
                {
                    maDonViSD = obj.ObjGiam.DVI_SDUNG;
                    txtSoQD.Text = obj.ObjGiam.SO_QUYET_DINH;
                    raddtNgayQD.Value = obj.ObjGiam.NGAY_QUYET_DINH.StringToDate("yyyyMMdd");
                    foreach (var item in cmbNguoiQD.Items)
                    {
                        AutoCompleteEntry entry = (AutoCompleteEntry)item;
                        if (entry.KeywordStrings.First().Equals(obj.ObjGiam.NGUOI_QUYET_DINH))
                            cmbNguoiQD.SelectedItem = item;
                    }
                    //obj.ObjGiam.CVU_NGUOI_QD ;
                    //txtMaGD.Text = obj.ObjGiam.SO_GIAO_DICH;
                    idTS = obj.ObjGiam.ID_TAI_SAN;
                    txtMaTS.Text = obj.ObjGiam.MA_TAI_SAN;
                    txtTenTS.Text = obj.ObjGiam.TEN_TAI_SAN;
                    numKHLuyKe.Value = Convert.ToDouble(obj.ObjGiam.KH_LUY_KE);
                    numKHThangGiam.Value = Convert.ToDouble(obj.ObjGiam.KH_THANG_GIAM);
                    foreach (var item in cmbNguyenNhan.Items)
                    {
                        AutoCompleteEntry entry = (AutoCompleteEntry)item;
                        if (entry.KeywordStrings.First().Equals(obj.ObjGiam.NGUYEN_NHAN_GIAM))
                            cmbNguyenNhan.SelectedItem = item;
                    }
                    foreach (var item in cmbHinhThucTT.Items)
                    {
                        AutoCompleteEntry entry = (AutoCompleteEntry)item;
                        if (entry.KeywordStrings.First().Equals(obj.ObjGiam.HTHUC_TTOAN))
                            cmbHinhThucTT.SelectedItem = item;
                    }
                    foreach (var item in cmbHinhThucTTPhi.Items)
                    {
                        AutoCompleteEntry entry = (AutoCompleteEntry)item;
                        if (entry.KeywordStrings.First().Equals(obj.ObjGiam.HTHUC_TTOAN_PHI))
                            cmbHinhThucTTPhi.SelectedItem = item;
                    }
                    numGiaTriConLai.Value = Convert.ToDouble(obj.ObjGiam.GIA_TRI_CON_LAI);
                    txtThoiGianConLai.Text = obj.ObjGiam.THOI_GIAN_CON_LAI.ToString();
                    numSoTien.Value = Convert.ToDouble(obj.ObjGiam.SO_TIEN_TONG);
                    numChiPhi.Value = Convert.ToDouble(obj.ObjGiam.CHI_PHI_TONG);
                    if (obj.ObjGiam.HTHUC_TTOAN.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri()))
                    {
                        txtTKTT.IsEnabled = false;
                        btnTKTT.IsEnabled = false;
                    }
                    else
                    {
                        txtTKTT.IsEnabled = true;
                        btnTKTT.IsEnabled = true;
                    }
                    if (obj.ObjGiam.HTHUC_TTOAN_PHI.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri()))
                    {
                        txtTKTTPhi.IsEnabled = false;
                        btnTKTTPhi.IsEnabled = false;
                    }
                    else
                    {
                        txtTKTTPhi.IsEnabled = true;
                        btnTKTTPhi.IsEnabled = true;
                    }
                    txtTKTT.Text = obj.ObjGiam.TKHOAN_TTOAN;
                    txtTKTTPhi.Text = obj.ObjGiam.TKHOAN_TTOAN_PHI;
                    txtDienGiai.Text = obj.ObjGiam.DIEN_GIAI;

                    TThaiNVu = obj.ObjGiam.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(obj.ObjGiam.TTHAI_NVU);

                    #region Tab thông tin kiểm soát
                    txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(obj.ObjGiam.TTHAI_BGHI);
                    teldtNgayNhap.Value = LDateTime.StringToDate(obj.ObjGiam.NGAY_NHAP, "yyyyMMdd");
                    txtNguoiLap.Text = obj.ObjGiam.NGUOI_NHAP;
                    if (LDateTime.IsDate(obj.ObjGiam.NGAY_CNHAT, "yyyyMMdd") == true)
                        teldtNgayCNhat.Value = LDateTime.StringToDate(obj.ObjGiam.NGAY_CNHAT, "yyyyMMdd");
                    else
                        teldtNgayCNhat.Value = null;
                    txtNguoiCapNhat.Text = obj.ObjGiam.NGUOI_CNHAT;
                    #endregion

                    if (!Action.Equals(DatabaseConstant.Action.SUA))
                    {
                        CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_GIAM);
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
            string loaiGD = ((AutoCompleteEntry)cmbHinhThucTT.SelectedItem).KeywordStrings.First();
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
                else if (numSoTien.Value <= 0)
                {
                    LMessage.ShowMessage("Chưa nhập số tiền hoặc số tiền không hợp lệ", LMessage.MessageBoxType.Warning);
                    numSoTien.Focus();
                    return false;
                }
                else if (((AutoCompleteEntry)cmbHinhThucTT.SelectedItem).KeywordStrings.First().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN.layGiaTri()) && txtTKTT.Text.IsNullOrEmptyOrSpace())
                {
                    LMessage.ShowMessage("Chưa nhập tài khoản thanh toán", LMessage.MessageBoxType.Warning);
                    txtTKTT.Focus();
                    return false;
                }
                else if (numChiPhi.Value < 0)
                {
                    LMessage.ShowMessage("Số tiền phí không hợp lệ", LMessage.MessageBoxType.Warning);
                    numChiPhi.Focus();
                    return false;
                }
                else if (((AutoCompleteEntry)cmbHinhThucTTPhi.SelectedItem).KeywordStrings.First().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN.layGiaTri()) && txtTKTTPhi.Text.IsNullOrEmptyOrSpace())
                {
                    LMessage.ShowMessage("Chưa nhập tài khoản thanh toán phí", LMessage.MessageBoxType.Warning);
                    txtTKTTPhi.Focus();
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
            DatabaseConstant.Function.TS_GIAM,
            DatabaseConstant.Table.TS_GIAM,
            DatabaseConstant.Action.SUA,
            lstId);
            Action = DatabaseConstant.Action.SUA;
            CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_GIAM);
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
                danhSachDto = new List<DANH_SACH_GIAM_DTO>();
                TaiSanProcess process = new TaiSanProcess();
                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                int iret = 0;
                if (idCT == 0)
                {
                    obj.ObjGiam.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    obj.ObjGiam.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    obj.ObjGiam.MA_DVI_QLY = ClientInformation.MaDonVi;
                    obj.ObjGiam.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    if (!process.GiamTaiSan(DatabaseConstant.Function.TS_GIAM, DatabaseConstant.Action.THEM, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail))
                        iret = 1;
                }
                else
                {
                    obj.ObjGiam.NGUOI_CNHAT = ClientInformation.NgayLamViecHienTai;
                    obj.ObjGiam.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    obj.ObjGiam.ID = idCT;
                    if (!process.GiamTaiSan(DatabaseConstant.Function.TS_GIAM, DatabaseConstant.Action.SUA, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail))
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
                DatabaseConstant.Function.TS_GIAM,
                DatabaseConstant.Table.TS_GIAM,
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
                idCT = obj.ObjGiam.ID;
                TThaiNVu = obj.ObjGiam.TTHAI_NVU;
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
                if (cbMultiAdd.IsChecked == true)
                    ClearForm();
                else
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_GIAM);

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
                    DatabaseConstant.Function.TS_GIAM,
                    DatabaseConstant.Table.TS_GIAM,
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
                DatabaseConstant.Function.TS_GIAM,
                DatabaseConstant.Table.TS_GIAM,
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
                obj.ObjGiam.ID = idCT;
                if (!process.GiamTaiSan(DatabaseConstant.Function.TS_GIAM, DatabaseConstant.Action.XOA, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail))
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
            DatabaseConstant.Function.TS_GIAM,
            DatabaseConstant.Table.TS_GIAM,
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
                    DatabaseConstant.Function.TS_GIAM,
                    DatabaseConstant.Table.TS_GIAM,
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
                DatabaseConstant.Function.TS_GIAM,
                DatabaseConstant.Table.TS_GIAM,
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
                obj.ObjGiam.ID = idCT;
                if (!process.GiamTaiSan(DatabaseConstant.Function.TS_GIAM, DatabaseConstant.Action.DUYET, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail))
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
            DatabaseConstant.Function.TS_GIAM,
            DatabaseConstant.Table.TS_GIAM,
            DatabaseConstant.Action.DUYET,
            lstId);
            Action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            TThaiNVu = obj.ObjGiam.TTHAI_NVU;
            lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
            SetInfomation();
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
                    DatabaseConstant.Function.TS_GIAM,
                    DatabaseConstant.Table.TS_GIAM,
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
                DatabaseConstant.Function.TS_GIAM,
                DatabaseConstant.Table.TS_GIAM,
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
                obj.ObjGiam.ID = idCT;
                if (!process.GiamTaiSan(DatabaseConstant.Function.TS_GIAM, DatabaseConstant.Action.TU_CHOI_DUYET, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail))
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
            DatabaseConstant.Function.TS_GIAM,
            DatabaseConstant.Table.TS_GIAM,
            DatabaseConstant.Action.TU_CHOI_DUYET,
            lstId);
            Action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            TThaiNVu = obj.ObjGiam.TTHAI_NVU;
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
                DatabaseConstant.Function.TS_GIAM,
                DatabaseConstant.Table.TS_GIAM,
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
                DatabaseConstant.Function.TS_GIAM,
                DatabaseConstant.Table.TS_GIAM,
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
                obj.ObjGiam.ID = idCT;
                process.GiamTaiSan(DatabaseConstant.Function.TS_GIAM, DatabaseConstant.Action.THOAI_DUYET, lstID, dieuKien, ref obj, ref danhSachDto, ref lstResponseDetail);
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
            DatabaseConstant.Function.TS_GIAM,
            DatabaseConstant.Table.TS_GIAM,
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
                objBienDongDTO.Function = DatabaseConstant.Function.TS_GIAM;
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
                txtSoQD.Text = obj.ObjGiam.SO_QUYET_DINH;
                //txtMaGD.Text = obj.ObjGiam.SO_GIAO_DICH;
                Action = DatabaseConstant.Action.XEM;
                SetEnabledAllControl(false);
                CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_GIAM);
            }
            else
                SetDataForm();
        }

        #endregion
    }
}
