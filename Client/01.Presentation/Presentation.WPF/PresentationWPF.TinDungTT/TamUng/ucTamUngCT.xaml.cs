﻿using System;
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
using Telerik.Windows.Controls;
using Presentation.Process.Common;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using System.Data;
using PresentationWPF.TinDungTT.PopupNghiepVu;
using Presentation.Process.TinDungServiceRef;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;

namespace PresentationWPF.TinDungTT.TamUng
{
    /// <summary>
    /// Interaction logic for ucTamUngCT.xaml
    /// </summary>
    public partial class ucTamUngCT : UserControl
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

        string maDonViQLy = ClientInformation.MaDonVi;
        string maDonViGDich = ClientInformation.MaDonViGiaoDich;
        List<AutoCompleteEntry> lstSourceCanBo = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNguonVon = new List<AutoCompleteEntry>();
        List<DataRow> lstPopup = new List<DataRow>();
        DatabaseConstant.Action action=DatabaseConstant.Action.THEM;
        public event EventHandler OnSavingCompleted;
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        List<int> lstPopupKU = new List<int>();
        public void LayDuLieuTuPopup(List<int> lst)
        {
            lstPopupKU = lst;
        }
        string TThaiNV = "";
        int iDGiaDich = 0;
        int iDCanBo = 0;
        double soTienTamUng = 0;
        private KIEM_SOAT _objKiemSoat = null;
        private DatabaseConstant.Function function = DatabaseConstant.Function.TDVM_TAM_UNG;
        TDVM_TAM_UNG objTamUng = new TDVM_TAM_UNG();
        List<TDVM_TAM_UNG_KUOCVM> lstTamUngKUOC = new List<TDVM_TAM_UNG_KUOCVM>();
        List<TDVM_TAM_UNG_SOTK> lstTamUngSOTK = new List<TDVM_TAM_UNG_SOTK>();
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucTamUngCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDungTT;component/TamUng/ucTamUngCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            InitEventHanler();
            ClearForm();
        }

        public ucTamUngCT(KIEM_SOAT objKiemSoat) : this()
        {
            _objKiemSoat = objKiemSoat;
            action = _objKiemSoat.action;
            SetDataForm();
        }

        void KhoiTaoComboBoxCanBo()
        {
            AutoComboBox au = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
            lstSourceCanBo.Clear();
            cmbCanBoPhatVon.Items.Clear();
            au.GenAutoComboBox(ref lstSourceCanBo, ref cmbCanBoPhatVon, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NHAN_SU.getValue(), lstDieuKien);
        }

        void InitEventHanler()
        {
            AutoComboBox au = new AutoComboBox();
            au.GenAutoComboBox(ref lstSourceNguonVon, ref cmbNguonVon, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NGUON_VON.getValue(), null);
            btnThemCumNhom.Click += new RoutedEventHandler(btnThemCumNhom_Click);
            btnThemDSRutTK.Click += new RoutedEventHandler(btnThemDSRutTK_Click);
            raddgrDSachPhatVay.DataLoaded += new EventHandler<EventArgs>(raddgrDSachPhatVay_DataLoaded);
            raddgrDSRutTK.DataLoaded += new EventHandler<EventArgs>(raddgrDSRutTK_DataLoaded);
            btnTKhoanTUng.Click += new RoutedEventHandler(btnTKhoanTUng_Click);
            btnTinhLaiTK.Click += new RoutedEventHandler(btnTinhLaiTK_Click);
            raddgrDSRutTK.CellEditEnded += new EventHandler<GridViewCellEditEndedEventArgs>(raddgrDSRutTK_CellEditEnded);
            dtpNgayPhatVon.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(dtpNgayPhatVon_SelectedDateChanged);
            cmbNguonVon.SelectionChanged += new SelectionChangedEventHandler(cmbNguonVon_SelectionChanged);
        }

        void cmbNguonVon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtSoTaiKhoan.Text = "";
            lstTamUngSOTK.Clear();
            raddgrDSRutTK.ItemsSource = lstTamUngSOTK;
            raddgrDSRutTK.Rebind();
            lstTamUngKUOC.Clear();
            raddgrDSachPhatVay.ItemsSource = lstTamUngKUOC;
            raddgrDSachPhatVay.Rebind();
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
                OnModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                BeforeSave(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET, BusinessConstant.TrangThaiBanGhi.SU_DUNG);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
            }
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
                OnPreview();
            }
            else if (strTinhNang.Equals("PreviewDanhSach"))
            {
                OnPreviewDanhSach();
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
                OnModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                BeforeSave(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET, BusinessConstant.TrangThaiBanGhi.SU_DUNG);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
            }
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
                OnPreview();
            }
            else if (strTinhNang.Equals("PreviewDanhSach"))
            {
                OnPreviewDanhSach();
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
            txtSoPhieu.Text = "";
            TThaiNV = "";
            teldtNgayGD.Value = ClientInformation.NgayLamViecHienTai.StringToDate(ApplicationConstant.defaultDateTimeFormat);
            teldtNgayPhatVon.Value = ClientInformation.NgayLamViecHienTai.StringToDate(ApplicationConstant.defaultDateTimeFormat);
            maDonViQLy = ClientInformation.MaDonVi;
            maDonViGDich = ClientInformation.MaDonViGiaoDich;
            KhoiTaoComboBoxCanBo();
            telnumSoTienTUng.Value = 0;
            txtDienGiai.Text = "";
            lblLabelTrangThai.Content = "";
            txtTrangThai.Text = "";
            teldtNgayCNhat.Value = null;
            teldtNgayNhap.Value = ClientInformation.NgayLamViecHienTai.StringToDate(ApplicationConstant.defaultDateTimeFormat);
            txtNguoiCapNhat.Text = "";
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            lstTamUngKUOC = new List<TDVM_TAM_UNG_KUOCVM>();
            lstTamUngSOTK = new List<TDVM_TAM_UNG_SOTK>();
            objTamUng = new TDVM_TAM_UNG();
            CommonFunction.RefreshButton(Toolbar, action, TThaiNV, mnuMain, function);
        }

        void SetEnabledAllControl(bool bBool)
        {
            cmbCanBoPhatVon.IsEnabled = bBool;
            telnumSoTienTUng.IsEnabled = bBool;
            txtDienGiai.IsEnabled = bBool;
            btnThemCumNhom.IsEnabled = bBool;
            btnXoaCumNhom.IsEnabled = bBool;
            btnThemDSRutTK.IsEnabled = bBool;
            btnXoaDSRutTK.IsEnabled = bBool;
            btnTinhLaiTK.IsEnabled = bBool;
            btnTKhoanTUng.IsEnabled = bBool;
            raddgrDSachPhatVay.IsReadOnly = !bBool;
            raddgrDSRutTK.IsReadOnly = !bBool;
            dtpNgayPhatVon.IsEnabled = bBool;
        }

        void btnThemDSRutTK_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            try
            {
                lstPopup.Clear();
                Window window = new Window();
                ucPopupSoTGui uc = new ucPopupSoTGui();
                uc.Function = function;
                uc.isMultiSelect = true;
                uc.DuLieuTraVe = new ucPopupSoTGui.LayDuLieu(LayDuLieuTuPopup);
                window.Title = "Danh sách sổ";
                window.Content = uc;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
                if (lstPopup != null)
                {
                    foreach (DataRow dr in lstPopup)
                    {
                        if (lstTamUngSOTK.Where(f => f.ID_SO_SO_TG == Convert.ToInt32(dr["ID"])).Count() > 0)
                            continue;
                        if (dr["NV_LOAI_NVON"] == DBNull.Value || lstTamUngSOTK.Where(f => f.NV_LOAI_NVON != dr["NV_LOAI_NVON"].ToString()).Count() > 0)
                            continue;
                        TDVM_TAM_UNG_SOTK objSOTK = new TDVM_TAM_UNG_SOTK();
                        objSOTK.ID_SO_SO_TG = Convert.ToInt32(dr["ID"]);
                        objSOTK.LOAI_SO_TG = dr["TEN_SAN_PHAM"].ToString();
                        objSOTK.MA_KHANG = dr["MA_KHANG"].ToString();
                        objSOTK.TEN_KHANG = dr["TEN_KHANG"].ToString();
                        objSOTK.SO_DU_GOC = Convert.ToDecimal(dr["SO_DU"]);
                        objSOTK.SO_SO_TG = dr["SO_SO_TG"].ToString();
                        objSOTK.DIA_CHI_KHANG = dr["DIA_CHI"].ToString();
                        objSOTK.SO_GTLQ_KHANG = dr["DD_GTLQ_SO"].ToString();
                        objSOTK.NV_LOAI_NVON = dr["NV_LOAI_NVON"].ToString();
                        lstTamUngSOTK.Add(objSOTK);
                    }
                }
                raddgrDSRutTK.ItemsSource = lstTamUngSOTK;
                raddgrDSRutTK.Rebind();
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Cursor = Cursors.Arrow;
        }

        void btnThemCumNhom_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            try
            {
                AutoCompleteEntry auNguonVon = lstSourceNguonVon.ElementAt(cmbNguonVon.SelectedIndex);
                string sidKheUoc = "";
                //foreach (DANH_SACH_KHE_UOC dr in lstKUOCGN)
                //{
                //    sidKheUoc += "," + dr.ID_KHE_UOC.ToString();
                //}
                if (sidKheUoc.Length > 0)
                    sidKheUoc = sidKheUoc.Substring(1);
                else
                    sidKheUoc = "0";
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(sidKheUoc);
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add("KUOCGN");
                lstDieuKien.Add(LDateTime.DateToString(teldtNgayPhatVon.Value.Value, ApplicationConstant.defaultDateTimeFormat));
                lstDieuKien.Add(auNguonVon.KeywordStrings.FirstOrDefault());
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_KHEUOC", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopupKheUocViMo popup = new ucPopupKheUocViMo(true, simplePopupResponse, true);
                popup.LayGiaTriListDataRow = new ucPopupKheUocViMo.LayListDataRow(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Content = popup;
                win.ShowDialog();
                if (lstPopup.Count > 0)
                {
                    foreach (DataRow dr in lstPopup)
                    {
                        if (lstTamUngKUOC.Where(f => f.ID_KHE_UOC == Convert.ToInt32(dr["ID"])).Count() > 0)
                            continue;
                        TDVM_TAM_UNG_KUOCVM objKUOCVM = new TDVM_TAM_UNG_KUOCVM();
                        objKUOCVM.DIA_CHI_KHANG = dr["DIA_CHI"].ToString();
                        objKUOCVM.ID_KHE_UOC = Convert.ToInt32(dr["ID"]);
                        objKUOCVM.MA_KHANG = dr["MA_KHANG"].ToString();
                        objKUOCVM.MA_KHE_UOC = dr["MA_KUOCVM"].ToString();
                        objKUOCVM.NGAY_DAO_HAN = dr["NGAY_DAO_HAN"].ToString();
                        objKUOCVM.SO_GTLQ_KHANG = dr["DD_GTLQ_SO"].ToString();
                        objKUOCVM.SO_TIEN_GNGAN = Convert.ToDecimal(dr["SO_TIEN_GIAI_NGAN"]);
                        objKUOCVM.TEN_KHANG = dr["TEN_KHANG"].ToString();
                        objKUOCVM.TGIAN_VAY = Convert.ToInt32(dr["TGIAN_VAY"]);
                        objKUOCVM.NV_LOAI_NVON = dr["NV_LOAI_NVON"].ToString();
                        lstTamUngKUOC.Add(objKUOCVM);
                    }
                    
                }
                raddgrDSachPhatVay.ItemsSource = lstTamUngKUOC;
                raddgrDSachPhatVay.Rebind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Cursor = Cursors.Arrow;
        }

        void raddgrDSRutTK_DataLoaded(object sender, EventArgs e)
        {
            telnumSoTienTUng.Value = (double)(lstTamUngKUOC.Sum(f => f.SO_TIEN_GNGAN) + lstTamUngSOTK.Sum(f => f.SO_TIEN_GOC) + lstTamUngSOTK.Sum(f => f.SO_TIEN_LAI));
            telTongTienDSRutTK.Value = (double)(lstTamUngSOTK.Sum(f => f.SO_TIEN_GOC) + lstTamUngSOTK.Sum(f => f.SO_TIEN_LAI));
            if (lstTamUngSOTK.Count > 0)
                btnTinhLaiTK.IsEnabled = true;
            else
                btnTinhLaiTK.IsEnabled = false;
        }

        void raddgrDSachPhatVay_DataLoaded(object sender, EventArgs e)
        {
            telnumSoTienTUng.Value = (double)(lstTamUngKUOC.Sum(f => f.SO_TIEN_GNGAN) + lstTamUngSOTK.Sum(f => f.SO_TIEN_GOC) + lstTamUngSOTK.Sum(f => f.SO_TIEN_LAI));
            telTongTienPhatVay.Value = (double)(lstTamUngKUOC.Sum(f => f.SO_TIEN_GNGAN));
        }

        void raddgrDSRutTK_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            telnumSoTienTUng.Value = (double)(lstTamUngKUOC.Sum(f => f.SO_TIEN_GNGAN) + lstTamUngSOTK.Sum(f => f.SO_TIEN_GOC) + lstTamUngSOTK.Sum(f => f.SO_TIEN_LAI));
            telTongTienDSRutTK.Value = (double)(lstTamUngSOTK.Sum(f => f.SO_TIEN_GOC) + lstTamUngSOTK.Sum(f => f.SO_TIEN_LAI));
            if (lstTamUngSOTK.Count > 0)
                btnTinhLaiTK.IsEnabled = true;
            else
                btnTinhLaiTK.IsEnabled = false;
        }


        void btnTKhoanTUng_Click(object sender, RoutedEventArgs e)
        {
            Presentation.Process.KeToanProcess ketoanProcess = new Presentation.Process.KeToanProcess();
            try
            {
                //Bat popup
                AutoCompleteEntry auNguonVon = lstSourceNguonVon.ElementAt(cmbNguonVon.SelectedIndex);
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add("%");
                lstDieuKien.Add("NOI_BANG");
                lstDieuKien.Add(Presentation.Process.Common.ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add(auNguonVon.KeywordStrings.FirstOrDefault());
                var process = new PopupProcess();

                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_TKHOAN_CTIET.getValue(), lstDieuKien);
                SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;

                lstPopup.Clear();
                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    txtSoTaiKhoan.Text = lstPopup[0][2].ToString();
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                ketoanProcess = null;
            }
        }

        void btnTinhLaiTK_Click(object sender, RoutedEventArgs e)
        {
            int iret = 0;
            TDVM_TAM_UNG obj = new TDVM_TAM_UNG();
            try
            {
                lstTamUngSOTK = raddgrDSRutTK.ItemsSource as List<TDVM_TAM_UNG_SOTK>;
                obj.NGAY_GD = LDateTime.DateToString(teldtNgayGD.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
                obj.NGAY_PHAT_VON = LDateTime.DateToString(teldtNgayPhatVon.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
                if (!LObject.IsNullOrEmpty(lstTamUngSOTK) && lstTamUngSOTK.Count > 0)
                {
                    List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                    obj.DSACH_SOTK = lstTamUngSOTK.ToArray();
                    iret = new TinDungProcess().TamUngGiaiNgan(DatabaseConstant.Action.TINH_TOAN_DU_CHI, ref obj, ref lstResponseDetail);
                    if (iret > 0)
                    {
                        lstTamUngSOTK = obj.DSACH_SOTK.ToList();
                        raddgrDSRutTK.ItemsSource = lstTamUngSOTK;
                        raddgrDSRutTK.Rebind();
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
            }
            finally
            {
                obj = null;
            }
        }

        void dtpNgayPhatVon_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (teldtNgayPhatVon.Value < teldtNgayGD.Value)
            //{
            //    teldtNgayPhatVon.Value = teldtNgayGD.Value;
            //}
            lstTamUngKUOC.Clear();
            lstTamUngSOTK.Clear();
            raddgrDSachPhatVay.ItemsSource = lstTamUngKUOC;
            raddgrDSRutTK.ItemsSource = lstTamUngSOTK;
            raddgrDSachPhatVay.Rebind();
            raddgrDSRutTK.Rebind();
        }
        #endregion

        /// <summary>
        /// Xu ly nghiep vu
        /// </summary>
        #region Xu ly nghiep vu
        bool GetDataForm(BusinessConstant.TrangThaiNghiepVu nghiepvu, BusinessConstant.TrangThaiBanGhi sudung)
        {
            bool bBool = true;
            try
            {
                AutoCompleteEntry auCanBo = lstSourceCanBo.ElementAt(cmbCanBoPhatVon.SelectedIndex);
                AutoCompleteEntry auNguonVon = lstSourceNguonVon.ElementAt(cmbNguonVon.SelectedIndex);
                lstTamUngKUOC = raddgrDSachPhatVay.ItemsSource as List<TDVM_TAM_UNG_KUOCVM>;
                lstTamUngSOTK = raddgrDSRutTK.ItemsSource as List<TDVM_TAM_UNG_SOTK>;
                objTamUng = new TDVM_TAM_UNG();
                objTamUng.DIEN_GIAI = txtDienGiai.Text;
                objTamUng.MA_GDICH = txtSoPhieu.Text;
                objTamUng.ID_GDICH = iDGiaDich;
                objTamUng.LOAI_TIEN = ClientInformation.MaDongNoiTe;
                objTamUng.LOAI_TTOAN = BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri();
                objTamUng.MA_CAN_BO_TUNG = auCanBo.KeywordStrings.FirstOrDefault();
                objTamUng.MA_DVI_GD = ClientInformation.MaDonViGiaoDich;
                objTamUng.MA_DVI_QLY = ClientInformation.MaDonVi;
                objTamUng.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                objTamUng.MA_LOAI_GDICH = DatabaseConstant.LoaiGiaoDich.TU01.layGiaTri();
                objTamUng.NGAY_DL = teldtNgayGD.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                objTamUng.NGAY_GD = teldtNgayGD.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                objTamUng.NGAY_PHAT_VON = teldtNgayPhatVon.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                objTamUng.NGUOI_NHAP = ClientInformation.TenDangNhap;
                objTamUng.PBAN_CAN_BO_TUNG = "";
                objTamUng.SO_TIEN_TUNG = (decimal)telnumSoTienTUng.Value.GetValueOrDefault();
                objTamUng.SO_TIEN_PHAT_VAY = (decimal)telTongTienPhatVay.Value.GetValueOrDefault();
                objTamUng.SO_TIEN_RUT_LAI = (decimal)telTongTienDSRutTK.Value.GetValueOrDefault();
                objTamUng.SOCMND_CAN_BO_TUNG = "";
                objTamUng.SO_TKHOAN_TUNG = txtSoTaiKhoan.Text;
                objTamUng.TEN_CAN_BO_TUNG = auCanBo.DisplayName;
                objTamUng.TTHAI_BGHI = sudung.layGiaTri();
                objTamUng.TTHAI_NVU = nghiepvu.layGiaTri();
                objTamUng.NV_LOAI_NVON = auNguonVon.KeywordStrings.FirstOrDefault();
                if (iDGiaDich > 0)
                {
                    objTamUng.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    objTamUng.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }

                objTamUng.NGAY_NHAP = LDateTime.DateToString(teldtNgayNhap.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
                objTamUng.NGUOI_NHAP = txtNguoiLap.Text;
                if (LObject.IsNullOrEmpty(lstTamUngKUOC))
                    lstTamUngKUOC = new List<TDVM_TAM_UNG_KUOCVM>();
                objTamUng.DSACH_KUOC = lstTamUngKUOC.ToArray();
                if (LObject.IsNullOrEmpty(lstTamUngSOTK))
                    lstTamUngSOTK = new List<TDVM_TAM_UNG_SOTK>();
                objTamUng.DSACH_SOTK = lstTamUngSOTK.ToArray();
            }
            catch (Exception ex)
            {
                bBool = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return bBool;
        }

        private void OnPreview()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(txtSoPhieu.Text))
            {
                LMessage.ShowMessage("Không có thông tin dữ liệu cần hiển thị", LMessage.MessageBoxType.Warning);

            }
            else
            {
                VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();

                GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.GDKT_IN_GIAO_DICH;
                DatabaseConstant.Function _function = DatabaseConstant.Function.KT_PHIEU_KE_TOAN;

                objGIAO_DICH_BASE.ChucNang = _function;

                GDKT_GIAO_DICH objGDKT_GIAO_DICH = new GDKT_GIAO_DICH();
                objGDKT_GIAO_DICH.MaGiaoDich = txtSoPhieu.Text;

                doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                doiTuongBaoCao.objGDKT_GIAO_DICH = objGDKT_GIAO_DICH;
                xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Xem báo cáo
        /// </summary>
        private void OnPreviewDanhSach()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(txtSoPhieu.Text))
            {
                LMessage.ShowMessage("Không có thông tin dữ liệu cần hiển thị", LMessage.MessageBoxType.Warning);

            }
            else
            {
                if (ClientInformation.Company.Equals("BANTAYVANG"))
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();

                    List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();
                    lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaGiaoDich", txtSoPhieu.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.HDVO_DANH_SACH_HOAN_TK);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
                else
                {
                    
                }
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }
         
        bool Validation()
        {
            if (txtSoTaiKhoan.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblTaiKhoanTamUng.Content.ToString());
                txtSoTaiKhoan.Focus();
                return false;
            }
            if (txtDienGiai.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblDienGiai.Content.ToString());
                txtDienGiai.Focus();
                return false;
            }
            if (LObject.IsNullOrEmpty(telnumSoTienTUng.Value) || telnumSoTienTUng.Value == 0)
            {
                CommonFunction.ThongBaoTrong(lblSoTienTamUng.Content.ToString());
                telnumSoTienTUng.Focus();
                return false;
            }
            if (cmbCanBoPhatVon.SelectedIndex < 0)
            {
                CommonFunction.ThongBaoTrong(lblCanBoPhatVon.Content.ToString());
                cmbCanBoPhatVon.Focus();
                return false;
            }
            return true;
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
                GetDataForm(nghiepvu,bghi);
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_TAM_UNG,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.SUA,
                lstId);
                OnSave();
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                // Yêu cầu Unlock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_TAM_UNG,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.SUA,
                lstId);
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
                if (objTamUng.MA_GDICH.IsNullOrEmptyOrSpace())
                    iret = new TinDungProcess().TamUngGiaiNgan(DatabaseConstant.Action.THEM, ref objTamUng, ref lstResponseDetail);
                else
                    iret = new TinDungProcess().TamUngGiaiNgan(DatabaseConstant.Action.SUA, ref objTamUng, ref lstResponseDetail);
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
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_TAM_UNG,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.SUA,
                lstId);
                if (!cbMultiAdd.IsChecked.GetValueOrDefault(false))
                {
                    if (iret > 0)
                    {
                        SetInfomation();
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
                objTamUng.MA_GDICH = txtSoPhieu.Text;
                objTamUng.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                objTamUng.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_TAM_UNG,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.XOA,
                lstId);
                OnDelete();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_TAM_UNG,
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
            iret = new TinDungProcess().TamUngGiaiNgan(DatabaseConstant.Action.XOA, ref objTamUng, ref lstResponseDetail);
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
                Cursor = Cursors.Wait;
                objTamUng.MA_GDICH = txtSoPhieu.Text;
                objTamUng.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                objTamUng.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_TAM_UNG,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.DUYET,
                lstId);

                OnApprove();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_TAM_UNG,
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
            iret = new TinDungProcess().TamUngGiaiNgan(DatabaseConstant.Action.DUYET, ref objTamUng, ref lstResponseDetail);
            AfterApprove(lstResponseDetail, iret);
        }
        void AfterApprove(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(iDGiaDich);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_TAM_UNG,
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
                
                objTamUng.MA_GDICH = txtSoPhieu.Text;
                objTamUng.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                objTamUng.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                Cursor = Cursors.Wait;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_TAM_UNG,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);

                OnRefuse();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_TAM_UNG,
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
            iret = new TinDungProcess().TamUngGiaiNgan(DatabaseConstant.Action.TU_CHOI_DUYET, ref objTamUng, ref lstResponseDetail);
            AfterRefuse(lstResponseDetail, iret);
        }
        void AfterRefuse(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(iDGiaDich);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_TAM_UNG,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.TU_CHOI_DUYET,
            lstId);
            SetInfomation();
            Cursor = Cursors.Arrow;

        }

        void BeforeCancel()
        {
            try
            {
                
                objTamUng.MA_GDICH = txtSoPhieu.Text;
                objTamUng.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                objTamUng.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                Cursor = Cursors.Wait;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_TAM_UNG,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);

                OnCancel();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_TAM_UNG,
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
            iret = new TinDungProcess().TamUngGiaiNgan(DatabaseConstant.Action.THOAI_DUYET, ref objTamUng, ref lstResponseDetail);
            AfterCancel(lstResponseDetail, iret);
        }
        void AfterCancel(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(iDGiaDich);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_TAM_UNG,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.THOAI_DUYET,
            lstId);
            SetInfomation();
            Cursor = Cursors.Arrow;
        }

        void SetInfomation()
        {
            TThaiNV = objTamUng.TTHAI_NVU;
            iDGiaDich = objTamUng.ID_GDICH;
            txtSoPhieu.Text = objTamUng.MA_GDICH;
            txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(objTamUng.TTHAI_BGHI);
            lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(objTamUng.TTHAI_NVU);
            txtNguoiLap.Text = objTamUng.NGUOI_NHAP;
            txtNguoiCapNhat.Text = objTamUng.NGUOI_CNHAT;
            teldtNgayNhap.Value = LDateTime.StringToDate(objTamUng.NGAY_NHAP, ApplicationConstant.defaultDateTimeFormat);
            if(!objTamUng.NGAY_CNHAT.IsNullOrEmptyOrSpace())
                teldtNgayCNhat.Value = LDateTime.StringToDate(objTamUng.NGAY_CNHAT, ApplicationConstant.defaultDateTimeFormat);
            SetEnabledAllControl(false);
            action = DatabaseConstant.Action.XEM;
            CommonFunction.RefreshButton(Toolbar, action, TThaiNV, mnuMain, function);
        }

        void OnModify()
        {
            SetEnabledAllControl(true);
            action = DatabaseConstant.Action.SUA;
            CommonFunction.RefreshButton(Toolbar, action, TThaiNV, mnuMain, function);
        }

        void SetDataForm()
        {
            try
            {
                DataSet ds = new TinDungProcess().LayThongTinGiaoDichTamUngGiaiNgan(_objKiemSoat.ID.ToString());
                if (!LObject.IsNullOrEmpty(ds) && ds.Tables.Count>2)
                {
                    DataTable dt = ds.Tables["CHI_TIET"];
                    maDonViQLy = dt.Rows[0]["MA_DVI_QLY"].ToString();
                    maDonViGDich = dt.Rows[0]["MA_DVI"].ToString();
                    iDGiaDich = Convert.ToInt32(dt.Rows[0]["ID_GDICH"]);
                    TThaiNV = dt.Rows[0]["TTHAI_NVU"].ToString();
                    txtSoPhieu.Text = dt.Rows[0]["MA_GDICH"].ToString();
                    teldtNgayGD.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_GDICH"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    KhoiTaoComboBoxCanBo();
                    cmbCanBoPhatVon.SelectedIndex = lstSourceCanBo.IndexOf(lstSourceCanBo.FirstOrDefault(f=>f.KeywordStrings.FirstOrDefault().Equals(dt.Rows[0]["MA_CAN_BO_TUNG"].ToString())));
                    cmbNguonVon.SelectedIndex = lstSourceNguonVon.IndexOf(lstSourceNguonVon.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dt.Rows[0]["NV_LOAI_NVON"].ToString())));
                    soTienTamUng = Convert.ToDouble(dt.Rows[0]["SO_TIEN_TUNG"].ToString());
                    telTongTienDSRutTK.Value = Convert.ToDouble(dt.Rows[0]["SO_TIEN_RUT_LAI"]);
                    telTongTienPhatVay.Value = Convert.ToDouble(dt.Rows[0]["SO_TIEN_PHAT_VAY"]);
                    txtSoTaiKhoan.Text = dt.Rows[0]["SO_TKHOAN_TUNG"].ToString();
                    txtDienGiai.Text = dt.Rows[0]["DIEN_GIAI"].ToString();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNV);
                    txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(dt.Rows[0]["TTHAI_BGHI"].ToString());
                    txtNguoiLap.Text = dt.Rows[0]["NGUOI_NHAP"].ToString();
                    txtNguoiCapNhat.Text = dt.Rows[0]["NGUOI_CNHAT"].ToString();
                    teldtNgayNhap.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_NHAP"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    teldtNgayPhatVon.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_PHAT_VON"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    if (dt.Rows[0]["NGAY_CNHAT"] != DBNull.Value)
                        teldtNgayCNhat.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_CNHAT"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    dt = ds.Tables["KUOCVM"];
                    
                    TDVM_TAM_UNG_KUOCVM objKUOC = null;
                    if (LObject.IsNullOrEmpty(lstTamUngKUOC)) lstTamUngKUOC = new List<TDVM_TAM_UNG_KUOCVM>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        objKUOC = new TDVM_TAM_UNG_KUOCVM();
                        objKUOC.DIA_CHI_KHANG = dr["DIA_CHI_KHANG"].ToString();
                        objKUOC.ID_KHE_UOC = Convert.ToInt32(dr["ID_KHE_UOC"]);
                        objKUOC.MA_GDICH = txtSoPhieu.Text;
                        objKUOC.MA_KHANG = dr["MA_KHANG"].ToString();
                        objKUOC.MA_KHE_UOC = dr["MA_KHE_UOC"].ToString();
                        objKUOC.NGAY_DAO_HAN = dr["NGAY_DAO_HAN"].ToString();
                        objKUOC.NGAY_GD = dr["NGAY_GD"].ToString();
                        objKUOC.SO_GTLQ_KHANG = dr["SO_GTLQ_KHANG"].ToString();
                        objKUOC.SO_TIEN_GNGAN = Convert.ToDecimal(dr["SO_TIEN_GNGAN"]);
                        objKUOC.TEN_KHANG = dr["TEN_KHANG"].ToString();
                        objKUOC.TGIAN_VAY = Convert.ToInt32(dr["TGIAN_VAY"]);
                        objKUOC.NV_LOAI_NVON = dr["NV_LOAI_NVON"].ToString();
                        lstTamUngKUOC.Add(objKUOC);
                    }
                    dt = ds.Tables["SOTKIEM"];
                    TDVM_TAM_UNG_SOTK objSOTK = null;
                    if (LObject.IsNullOrEmpty(lstTamUngSOTK)) lstTamUngSOTK = new List<TDVM_TAM_UNG_SOTK>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        objSOTK = new TDVM_TAM_UNG_SOTK();
                        objSOTK.DIA_CHI_KHANG = dr["DIA_CHI_KHANG"].ToString();
                        objSOTK.GHI_CHU = dr["GHI_CHU"].ToString();
                        objSOTK.ID_SO_SO_TG = Convert.ToInt32(dr["ID_SO_SO_TG"]);
                        objSOTK.MA_GDICH = txtSoPhieu.Text;
                        objSOTK.MA_KHANG = dr["MA_KHANG"].ToString();
                        objSOTK.LAI_DU_CHI = Convert.ToDecimal(dr["LAI_DU_CHI"]);
                        objSOTK.LOAI_SO_TG = dr["LOAI_SO_TG"].ToString();
                        objSOTK.NGAY_GD = dr["NGAY_GD"].ToString();
                        objSOTK.SO_GTLQ_KHANG = dr["SO_GTLQ_KHANG"].ToString();
                        objSOTK.SO_DU_GOC = Convert.ToDecimal(dr["SO_DU_GOC"]);
                        objSOTK.TEN_KHANG = dr["TEN_KHANG"].ToString();
                        objSOTK.SO_SO_TG = dr["SO_SO_TG"].ToString();
                        objSOTK.SO_TIEN_GOC = Convert.ToDecimal(dr["SO_TIEN_GOC"]);
                        objSOTK.SO_TIEN_LAI = Convert.ToDecimal(dr["SO_TIEN_LAI"]);
                        objSOTK.NV_LOAI_NVON = dr["NV_LOAI_NVON"].ToString();
                        lstTamUngSOTK.Add(objSOTK);
                    }
                    raddgrDSachPhatVay.ItemsSource = lstTamUngKUOC;
                    raddgrDSRutTK.ItemsSource = lstTamUngSOTK;
                    raddgrDSachPhatVay.Rebind();
                    raddgrDSRutTK.Rebind();
                    telnumSoTienTUng.Value = soTienTamUng;
                }
                if (action.Equals(DatabaseConstant.Action.XEM))
                    SetEnabledAllControl(false);
                else
                    SetEnabledAllControl(true);
                CommonFunction.RefreshButton(Toolbar, action, TThaiNV, mnuMain, function);
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
        #endregion
    }
}
