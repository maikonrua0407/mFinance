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
using System.Data;
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using PresentationWPF.CustomControl;
using Presentation.Process;
using Presentation.Process.TinDungServiceRef;
using Presentation.Process.Common;
using Telerik.Windows.Controls;

namespace PresentationWPF.TinDung.VongVay
{
    /// <summary>
    /// Interaction logic for ucVongVayCT.xaml
    /// </summary>
    public partial class ucVongVayCT : UserControl
    {
        /// <summary>
        /// Khai bao
        /// </summary>
        #region Khai bao
        public event EventHandler OnSavingComleted = null;
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
        List<AutoCompleteEntry> lstHanMucGocVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstHanMucKyHan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiKyHan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstMaLoaiTien = new List<AutoCompleteEntry>();
        TD_VONG_VAY objVongVay;

        public TD_VONG_VAY ObjVongVay
        {
            get { return objVongVay; }
            set { objVongVay = value; }
        }
        int idVongVay=0;

        public int IdVongVay
        {
            get { return idVongVay; }
            set { idVongVay = value; }
        }
        string sHanMucGocVay, sHanMucKyHan;

        bool bSuaDuLieu = false;

        public bool BSuaDuLieu
        {
            get { return bSuaDuLieu; }
            set { bSuaDuLieu = value; }
        }
        DataTable dtVongVayVon;
        string sTrangThai="";
        string sKeyGocVay = "U.TinDung.ucVongVayCT.ColGocVay";
        string sKeyKyHanVay = "U.TinDung.ucVongVayCT.ColKyHan";
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucVongVayCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDung;component/VongVay/ucVongVayCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            KhoiTaoGiaTriComboBox();
            ClearForm();
        }

        /// <summary>
        /// Khởi tạo các ComboBox
        /// </summary>
        /// <param name="lstAutoComplete"></param>
        /// <param name="cmbCommon"></param>
        /// <param name="lstDieuKien"></param>
        void KhoiTaoGiaTriComboBox()
        {
            // khởi tạo combobox
            string maTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            // Gán giá trị điều kiện
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.TINH_CHAT_VONG_VAY));
            // Gen ComboBox bằng việc gọi hàm
            auto.GenAutoComboBox(ref lstHanMucGocVay, ref cmbTCGocVay, maTruyVan, lstDieuKien);
            auto.GenAutoComboBox(ref lstHanMucKyHan, ref cmbTCKyHan, maTruyVan, lstDieuKien);
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.KY_HAN_DVI_TINH));
            auto.GenAutoComboBox(ref lstLoaiKyHan, ref cmbLoaiKyHan, maTruyVan, lstDieuKien, BusinessConstant.KY_HAN_DVI_TINH.THANG.layGiaTri());
            // Gen ComboBox bằng việc gọi hàm
            maTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_TIENTE.getValue();
            auto.GenAutoComboBox(ref lstMaLoaiTien, ref cmbGocVay, maTruyVan, null, ClientInformation.MaDongNoiTe);
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
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAP_DU_LIEU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ClearForm();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                Sua();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                Xoa();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                LuuDuLieu(BusinessConstant.TrangThaiBanGhi.SU_DUNG, BusinessConstant.TrangThaiNghiepVu.LUU_TAM, DatabaseConstant.Action.LUU_TAM);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                LuuDuLieu(BusinessConstant.TrangThaiBanGhi.SU_DUNG, BusinessConstant.TrangThaiNghiepVu.CHO_DUYET, DatabaseConstant.Action.TRINH_DUYET);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                Duyet();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                TuChoiDuyet();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                ThoaiDuyet();
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
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAP_DU_LIEU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ClearForm();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                Sua();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                Xoa();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                LuuDuLieu(BusinessConstant.TrangThaiBanGhi.SU_DUNG, BusinessConstant.TrangThaiNghiepVu.LUU_TAM, DatabaseConstant.Action.LUU_TAM);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                LuuDuLieu(BusinessConstant.TrangThaiBanGhi.SU_DUNG, BusinessConstant.TrangThaiNghiepVu.CHO_DUYET, DatabaseConstant.Action.TRINH_DUYET);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                Duyet();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                TuChoiDuyet();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                ThoaiDuyet();
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
                    throw new System.NullReferenceException(LLanguage.SearchResourceByKey("M.TinDung.VongVay.ucVongVayDS.KhongTimThayControl") + sbControl.ToString());
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Cấu hình lưới khi load xong dữ liệu lên lưới
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void raddgrVongVonDS_Loaded(object sender, RoutedEventArgs e)
        {
            //foreach (DataRow dt in raddgrVongVonDS.Items)
            //{
            //    raddgrVongVonDS.SelectedItems.Add(dt);
            //}
            loadWidthColumn();
            if (dtVongVayVon != null && dtVongVayVon.Rows.Count > 0)
            {
                if (dtVongVayVon.Rows[dtVongVayVon.Rows.Count - 1][5].ToString() == ">=")
                {
                    LockerControl(false);
                }
                else
                    LockerControl(true);
            }
        }

        /// <summary>
        /// Cấu hình độ rộng lưới
        /// </summary>
        private void loadWidthColumn()
        {
            if (raddgrVongVonDS.Columns.Count > 2)
            {
                raddgrVongVonDS.Columns[2].Width = new Telerik.Windows.Controls.GridViewLength(1, GridViewLengthUnitType.Star);
                raddgrVongVonDS.Columns[2].Header = LLanguage.SearchResourceByKey("U.TinDung.ucVongVayCT.ColVong");
                raddgrVongVonDS.Columns[3].Width = new Telerik.Windows.Controls.GridViewLength(1, GridViewLengthUnitType.Star);
                raddgrVongVonDS.Columns[3].Header = LLanguage.SearchResourceByKey(sKeyGocVay);
                raddgrVongVonDS.Columns[4].Width = new Telerik.Windows.Controls.GridViewLength(1, GridViewLengthUnitType.Star);
                raddgrVongVonDS.Columns[4].Header = LLanguage.SearchResourceByKey(sKeyKyHanVay);
            }
            
        }

        /// <summary>
        /// Khóa control điều khiển vòng vay chi tiết
        /// </summary>
        /// <param name="bBool"></param>
        void LockerControl(bool bBool)
        {
            string sToanTu = "=";
            if(dtVongVayVon.Rows.Count>0)
                sToanTu = dtVongVayVon.Rows[dtVongVayVon.Rows.Count - 1][5].ToString();
            if (sToanTu != "=")
                cmbSoSanh.IsEnabled = false;
            else
                cmbSoSanh.IsEnabled = true;
            txtSoVong.IsEnabled = bBool;
            txtGocVay.IsEnabled = bBool;
            //txtKyHan.IsEnabled = bBool;
        }

        /// <summary>
        /// ComboBox Han Muc Goc Vay Selection Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTCGocVay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTCGocVay.SelectedIndex >= 0)
            {
                sHanMucGocVay = lstHanMucGocVay.ElementAt(cmbTCGocVay.SelectedIndex).KeywordStrings.First();
                if (cmbTCGocVay.SelectedIndex == 0)
                    sKeyGocVay = "U.TinDung.ucVongVayCT.ColGocVay";
                else
                    sKeyGocVay = "U.TinDung.ucVongVayCT.ColGocVayTu";
                loadWidthColumn();
            }
        }

        /// <summary>
        /// ComboBox Han Muc Ky Han Selection Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTCKyHan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTCKyHan.SelectedIndex >= 0)
            {
                sHanMucKyHan = lstHanMucKyHan.ElementAt(cmbTCKyHan.SelectedIndex).KeywordStrings.First();
                if (cmbTCKyHan.SelectedIndex == 0)
                    sKeyKyHanVay = "U.TinDung.ucVongVayCT.ColKyHan";
                else
                    sKeyKyHanVay = "U.TinDung.ucVongVayCT.ColKyHanTu";
                loadWidthColumn();
            }
            
        }

        /// <summary>
        /// Lock Control điều khiển nhập thông tin chung vòng vay vốn
        /// </summary>
        /// <param name="bBool"></param>
        void LockControl(bool bBool)
        {
            teldtNgayLap.IsEnabled = bBool;
            txtTenVongVay.IsEnabled = bBool;
            teldtNgayHieuLuc.IsEnabled = bBool;
            teldtNgayHetHan.IsEnabled = bBool;
            cmbTCGocVay.IsEnabled = bBool;
            cmbTCKyHan.IsEnabled = bBool;
            cmbLoaiKyHan.IsEnabled = bBool;
            grbVongVayVon.IsEnabled = bBool;
        }


        private void tlbAddVongVon_Click(object sender, RoutedEventArgs e)
        {
            int iViTri = dtVongVayVon.Rows.Count - 1;
            if (iViTri < 0)
                return;
            string sToanTu = dtVongVayVon.Rows[iViTri][5].ToString();
            if(!LObject.IsNullOrEmpty(sToanTu) && sToanTu != ">=")
            {
                txtSoVong.Value = dtVongVayVon.Rows.Count + 1;
                txtGocVay.Value = 0;
                txtKyHan.Value = 1;
            }
        }
        #endregion

        /// <summary>
        /// Xu ly nghiep vu
        /// </summary>
        #region Xu ly nghiep vu

        
        void LuuDuLieu(BusinessConstant.TrangThaiBanGhi banghi, BusinessConstant.TrangThaiNghiepVu nghiepvu, DatabaseConstant.Action action)
        {
            if (nghiepvu != BusinessConstant.TrangThaiNghiepVu.LUU_TAM)
            {
                if (!Vadidate())
                    return;
            }
            int iResult = 0;
            TinDungProcess tinhdungProcess = new TinDungProcess();
            try
            {
                objVongVay = new TD_VONG_VAY();
                List<TD_VONG_VAY_CTIET> lstVongVay = new List<TD_VONG_VAY_CTIET>();
                LayDuLieu(ref objVongVay, ref lstVongVay, banghi, nghiepvu);
                if (idVongVay == 0)
                {
                    iResult = tinhdungProcess.LuuVongVayTinDung(objVongVay, lstVongVay);
                }
                else
                {
                    iResult = tinhdungProcess.SuaVongVayTinDung(objVongVay, lstVongVay);
                }
                if (iResult > 0)
                {
                    LMessage.ShowMessage("M.DungChung.LuuDuLieuThanhCong", LMessage.MessageBoxType.Information);
                    List<int> lstId = new List<int>();
                    lstId.Add(idVongVay);
                    // Yêu cầu unlock dữ liệu
                    UtilitiesProcess process = new UtilitiesProcess();
                    bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                    DatabaseConstant.Function.TDVM_VONG_VAY,
                    DatabaseConstant.Table.TD_VONG_VAY,
                    DatabaseConstant.Action.SUA,
                    lstId);
                    idVongVay = iResult;
                    LoadDuLieu();
                    if (OnSavingComleted != null)
                        OnSavingComleted(null, EventArgs.Empty);
                    if (cbMultiAdd.IsChecked == true)
                    {
                        ClearForm();
                        idVongVay = 0;
                        objVongVay = null;
                    }
                    else
                        CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, sTrangThai,mnuMain,DatabaseConstant.Function.TDVM_VONG_VAY);
                    loadWidthColumn();
                }
                else
                    LMessage.ShowMessage("M.DungChung.LoiLuuDuLieu", LMessage.MessageBoxType.Error);
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLuuDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            
        }

        void ClearForm()
        {
            dtVongVayVon = new DataTable();
            dtVongVayVon.Columns.Add("SO_VONG", typeof(string));
            dtVongVayVon.Columns.Add("SO_GOC", typeof(decimal));
            dtVongVayVon.Columns.Add("KY_HAN", typeof(string));
            dtVongVayVon.Columns.Add("SOKYHAN", typeof(int));
            dtVongVayVon.Columns.Add("KYHAN", typeof(string));
            dtVongVayVon.Columns.Add("TOANTU", typeof(string));
            dtVongVayVon.Columns.Add("TEN_VONG", typeof(string));
            teldtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai,ApplicationConstant.defaultDateTimeFormat);
            teldtNgayHieuLuc.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            teldtNgayHetHan.Value = null;
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            teldtNgayCNhat.Value = null;
            raddgrVongVonDS.ItemsSource = null;
            raddgrVongVonDS.ItemsSource = dtVongVayVon;
            cmbSoSanh.SelectedIndex = 0;
            cmbTCKyHan.SelectedIndex = 0;
            cmbTCGocVay.SelectedIndex = 0;
            txtMaNhomVong.Text = "";
            txtTenVongVay.Text = "";
            txtSoVong.Value = 1;
            txtGocVay.Value = null;
            txtKyHan.Value = 1;
            LockControl(true);
            LockerControl(true);
            loadWidthColumn();
            sTrangThai = "";
            txtTrangThai.Text = "";
            lblTrangThai.Content = "";
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, "", mnuMain, DatabaseConstant.Function.TDVM_VONG_VAY);
        }

        void LayDuLieu(ref TD_VONG_VAY obj, ref List<TD_VONG_VAY_CTIET> lstVongVay, BusinessConstant.TrangThaiBanGhi banghi, BusinessConstant.TrangThaiNghiepVu nghiepvu)
        {
            obj.TEN_VONG_VAY = txtTenVongVay.Text;
            obj.NGAY_LAP = "";
            if(teldtNgayLap.Value!=null)
            obj.NGAY_LAP = LDateTime.DateToString((DateTime)teldtNgayLap.Value, ApplicationConstant.defaultDateTimeFormat);
            obj.NGAY_HIEU_LUC = "";
            if (teldtNgayHieuLuc.Value != null)
            obj.NGAY_HIEU_LUC = LDateTime.DateToString((DateTime)teldtNgayHieuLuc.Value, ApplicationConstant.defaultDateTimeFormat);
            obj.NGAY_HET_HLUC = "";
            if (teldtNgayHetHan.Value != null)
            obj.NGAY_HET_HLUC = LDateTime.DateToString((DateTime)teldtNgayHetHan.Value, ApplicationConstant.defaultDateTimeFormat);
            obj.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
            obj.TCHAT_GOC_VAY = sHanMucGocVay;
            obj.TCHAT_KY_HAN = sHanMucKyHan;
            obj.TOAN_TU = cmbSoSanh.Text;
            obj.KY_HAN_DVI_TINH = lstLoaiKyHan.ElementAt(cmbLoaiKyHan.SelectedIndex).KeywordStrings.First();
            obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
            obj.TTHAI_BGHI = BusinessConstant.layGiaTri(banghi);
            obj.TTHAI_NVU = BusinessConstant.layGiaTri(nghiepvu);
            obj.NGAY_CNHAT = "";
            obj.NGUOI_CNHAT = "";
            if (idVongVay > 0)
            {
                obj.ID = idVongVay;
                obj.MA_VONG_VAY = txtMaNhomVong.Text;
                obj.NGUOI_NHAP = txtNguoiLap.Text;
                obj.NGAY_NHAP = LDateTime.DateToString((DateTime)teldtNgayNhap.Value, ApplicationConstant.defaultDateTimeFormat);
                obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
            }
            else
            {
                obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                obj.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
            }
            foreach (DataRow dr in dtVongVayVon.Rows)
            {
                TD_VONG_VAY_CTIET objCT = new TD_VONG_VAY_CTIET();
                objCT.TOAN_TU = dr[5].ToString();
                objCT.SO_THU_TU = dr[0].ToString();
                objCT.SO_TIEN = decimal.Parse(dr[1].ToString());
                objCT.KY_HAN = int.Parse(dr[3].ToString());
                objCT.KY_HAN_DVI_TINH = lstLoaiKyHan.ElementAt(cmbLoaiKyHan.SelectedIndex).KeywordStrings.First();
                objCT.MA_DVI_QLY = ClientInformation.MaDonVi;
                objCT.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                objCT.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                objCT.NGUOI_NHAP = ClientInformation.TenDangNhap;
                objCT.TTHAI_BGHI = BusinessConstant.layGiaTri(banghi);
                objCT.TTHAI_NVU = BusinessConstant.layGiaTri(nghiepvu);
                lstVongVay.Add(objCT);
            }
            
        }
        /// <summary>
        /// Vadidate dữ liệu trước khi lưu
        /// </summary>
        /// <returns></returns>
        bool Vadidate()
        {
            if (teldtNgayLap.Value == null)
            {
                CommonFunction.ThongBaoTrong(lblNgayLapVongVong.Content.ToString());
                teldtNgayLap.Focus();
                return false;
            }
            else if (teldtNgayHieuLuc.Value == null)
            {
                CommonFunction.ThongBaoTrong(lblNgayHieuLucVongVong.Content.ToString());
                teldtNgayHieuLuc.Focus();
                return false;
            }
            else if (teldtNgayHetHan.Value <= teldtNgayHieuLuc.Value)
            {
                LMessage.ShowMessage("M.DungChung.ThongBao.NgayHetHanNhoNgayHieuLuc",LMessage.MessageBoxType.Warning);
                teldtNgayHetHan.Focus();
                return false;
            }
            else if (txtTenVongVay.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblTenVongVay.Content.ToString());
                txtTenVongVay.Focus();
                return false;
            }
            else if (cmbTCGocVay.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblHanMucGocVay.Content.ToString());
                cmbTCGocVay.Focus();
                return false;
            }
            else if (cmbTCKyHan.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblHanMucKyHan.Content.ToString());
                cmbTCKyHan.Focus();
                return false;
            }
            else if (dtVongVayVon.Rows.Count < 1)
            {
                LMessage.ShowMessage("M.TinDung.ucVongVayCT.DanhSachTrong", LMessage.MessageBoxType.Warning);
                txtSoVong.Focus();
                return false;
            }
            else if (dtVongVayVon.Rows[dtVongVayVon.Rows.Count - 1]["TOANTU"].Equals("="))
            {
                LMessage.ShowMessage("M.TinDung.ucVongVayCT.VongVayCuoi", new string[] { dtVongVayVon.Rows[dtVongVayVon.Rows.Count - 1]["SO_VONG"].ToString() }, LMessage.MessageBoxType.Warning);
                txtSoVong.Focus();
                return false;
            }
            else
                return true;
        }
        /// <summary>
        /// Sự kiện lưu vòng vay chi tiết vào lưới danh sách
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbSubmitVongVon_Click(object sender, RoutedEventArgs e)
        {
            int iViTri = -1;
            if (!ValidationDataVongCT())
                return;
            try
            {
                string sSoVong = txtSoVong.Value.ToString();
                string[] arrSoVong = new string[1];
                arrSoVong[0] = sSoVong;
                if (dtVongVayVon.Rows.Count > 0)
                {
                    string sToanTu = dtVongVayVon.Rows[dtVongVayVon.Rows.Count - 1][5].ToString();
                    for (int i = 0; i < dtVongVayVon.Rows.Count; i++)
                    {
                        if (dtVongVayVon.Rows[i][0].ToString().Contains(sSoVong))
                        {
                            iViTri = i;
                            break;
                        }
                    }
                    if (iViTri > -1)
                    {
                        dtVongVayVon.Rows[iViTri][0] = txtSoVong.Value;
                        dtVongVayVon.Rows[iViTri][1] = txtGocVay.Value;
                        dtVongVayVon.Rows[iViTri][2] = txtKyHan.Value.ToString();
                        dtVongVayVon.Rows[iViTri][3] = txtKyHan.Value;
                        dtVongVayVon.Rows[iViTri][4] = "";
                        dtVongVayVon.Rows[iViTri][5] = cmbSoSanh.Text;
                        if (cmbSoSanh.Text.Equals("="))
                            dtVongVayVon.Rows[iViTri][6] = sSoVong;
                        else
                            dtVongVayVon.Rows[iViTri][6] = LLanguage.SearchResourceByKey("U.TinDung.ucVongVayCT.TuVong", arrSoVong);
                    }
                    else
                    {
                        if (dtVongVayVon.Rows.Count + 1 == int.Parse(txtSoVong.Value.ToString()))
                        {
                            DataRow dr = dtVongVayVon.NewRow();
                            dr[0] = txtSoVong.Value;
                            dr[1] = txtGocVay.Value;
                            dr[2] = txtKyHan.Value.ToString();
                            dr[3] = txtKyHan.Value;
                            dr[4] = "";
                            dr[5] = cmbSoSanh.Text;
                            if (cmbSoSanh.Text.Equals("="))
                                dr[6] = sSoVong;
                            else
                                dr[6] = LLanguage.SearchResourceByKey("U.TinDung.ucVongVayCT.TuVong", arrSoVong);
                            dtVongVayVon.Rows.Add(dr);
                        }
                    }
                }
                else
                {
                    DataRow dr = dtVongVayVon.NewRow();
                    dr[0] = 1;
                    dr[1] = txtGocVay.Value;
                    dr[2] = txtKyHan.Value.ToString();
                    dr[3] = txtKyHan.Value;
                    dr[4] = "";
                    dr[5] = cmbSoSanh.Text;
                    if (cmbSoSanh.Text.Equals("="))
                        dr[6] = txtSoVong.Value;
                    else
                        dr[6] = LLanguage.SearchResourceByKey("U.TinDung.ucVongVayCT.TuVong", arrSoVong);
                    dtVongVayVon.Rows.Add(dr);
                }
                
                if (dtVongVayVon != null && dtVongVayVon.Rows.Count > 0)
                {
                    iViTri = -1;
                    for (int i = 0; i < dtVongVayVon.Rows.Count; i++)
                    {
                        if (dtVongVayVon.Rows[i][5].Equals(">="))
                        {
                            iViTri = i;
                            break;
                        }
                    }
                    if (iViTri > -1)
                    {
                        int dem = dtVongVayVon.Rows.Count;
                        for (int i = iViTri + 1; i < dem; i++ )
                        {
                            dtVongVayVon.Rows.RemoveAt(iViTri + 1);
                        }
                    }
                    if (dtVongVayVon.Rows[dtVongVayVon.Rows.Count - 1][5].ToString() == ">=")
                    {
                        LockerControl(false);
                    }
                    else
                        LockerControl(true);
                }
                raddgrVongVonDS.ItemsSource = null;
                raddgrVongVonDS.ItemsSource = dtVongVayVon;
                loadWidthColumn();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M_ResponseMessage_DungChung_LoiKhongXacDinh", LMessage.MessageBoxType.Error);
            }
        }

        /// <summary>
        /// Validate dữ liệu vòng vay chi tiết
        /// </summary>
        /// <returns></returns>
        bool ValidationDataVongCT()
        {
            if (txtSoVong.Value == null || txtSoVong.Value == 0)
            {
                txtSoVong.Focus();
                return false;
            }
            else if (txtGocVay.Value == null || txtGocVay.Value == 0)
            {
                txtGocVay.Focus();
                return false;
            }
            else if (txtKyHan.Value == null || txtKyHan.Value == 0)
            {
                txtKyHan.Focus();
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Load dữ liệu lên control Vòng vay CT
        /// </summary>
        void LoadDataControl()
        {
            if (raddgrVongVonDS.SelectedItems.Count > 0)
            {
                DataRow dr = (DataRow)raddgrVongVonDS.SelectedItems[0];
                txtSoVong.Value = double.Parse(dr[0].ToString());
                txtGocVay.Value = double.Parse(dr[1].ToString());
                txtKyHan.Value = double.Parse(dr[3].ToString());
                //cmbLoaiKyHan.SelectedIndex = lstLoaiKyHan.IndexOf(lstLoaiKyHan.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dr[4])));
                cmbSoSanh.Text = dr[5].ToString();
                LockerControl(true);
                if (cmbSoSanh.Text == ">=")
                    cmbSoSanh.IsEnabled = true;
            }
        }

        /// <summary>
        /// Sự kiện xóa dữ liệu trên lưới vòng vay chi tiết
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbDeleteVongVon_Click(object sender, RoutedEventArgs e)
        {
            if (raddgrVongVonDS.SelectedItems.Count > 0)
            {
                foreach (DataRow dr in raddgrVongVonDS.SelectedItems)
                {
                    dtVongVayVon.Rows.Remove(dr);
                }
                for (int i = 0; i < dtVongVayVon.Rows.Count; i++)
                {
                    dtVongVayVon.Rows[i][0] = (i + 1);
                }
                raddgrVongVonDS.ItemsSource = null;
                raddgrVongVonDS.ItemsSource = dtVongVayVon;
                loadWidthColumn();
                string sToanTu = dtVongVayVon.Rows[dtVongVayVon.Rows.Count - 1][5].ToString();
                if (sToanTu != "=")
                {
                    LockerControl(false);
                }
            }
            
        }

        private void Sua()
        {
            // Yêu cầu lock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(idVongVay);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_VONG_VAY,
            DatabaseConstant.Table.TD_VONG_VAY,
            DatabaseConstant.Action.SUA,
            lstId);
            if (sTrangThai.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
            {
            } 
            else
            {
                LockControl(true);
            }
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, sTrangThai, mnuMain, DatabaseConstant.Function.TDVM_VONG_VAY);
        }

        /// <summary>
        /// Load du lieu
        /// </summary>
        public void LoadDuLieu()
        {
            try 
            {
                if (idVongVay > 0)
                {
                    DataSet ds = new TinDungProcess().getVongVonVayByID(idVongVay.ToString());
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        sTrangThai = ds.Tables[0].Rows[0]["TTHAI_NVU"].ToString();
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThai);
                        
                        txtMaNhomVong.Text = ds.Tables[0].Rows[0]["MA_VONG_VAY"].ToString();
                        if (!ds.Tables[0].Rows[0]["NGAY_LAP"].ToString().IsNullOrEmpty())
                        teldtNgayLap.Value = LDateTime.StringToDate(ds.Tables[0].Rows[0]["NGAY_LAP"].ToString(),ApplicationConstant.defaultDateTimeFormat);
                        if (!ds.Tables[0].Rows[0]["NGAY_HIEU_LUC"].ToString().IsNullOrEmpty())
                        teldtNgayHieuLuc.Value = LDateTime.StringToDate(ds.Tables[0].Rows[0]["NGAY_HIEU_LUC"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                        if (!ds.Tables[0].Rows[0]["NGAY_HET_HLUC"].ToString().IsNullOrEmpty())
                        teldtNgayHetHan.Value = LDateTime.StringToDate(ds.Tables[0].Rows[0]["NGAY_HET_HLUC"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                        txtTenVongVay.Text = ds.Tables[0].Rows[0]["TEN_VONG_VAY"].ToString();
                        cmbTCGocVay.SelectedIndex = lstHanMucGocVay.IndexOf(lstHanMucGocVay.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(ds.Tables[0].Rows[0]["TCHAT_GOC_VAY"].ToString())));
                        cmbTCKyHan.SelectedIndex = lstHanMucKyHan.IndexOf(lstHanMucKyHan.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(ds.Tables[0].Rows[0]["TCHAT_KY_HAN"].ToString())));
                        cmbLoaiKyHan.SelectedIndex = lstLoaiKyHan.IndexOf(lstLoaiKyHan.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(ds.Tables[0].Rows[0]["KY_HAN_DVI_TINH"].ToString())));
                        txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(ds.Tables[0].Rows[0]["TTHAI_BGHI"].ToString());
                        if (!ds.Tables[0].Rows[0]["NGAY_NHAP"].ToString().IsNullOrEmpty())
                        teldtNgayNhap.Value = LDateTime.StringToDate(ds.Tables[0].Rows[0]["NGAY_NHAP"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                        txtNguoiLap.Text = ds.Tables[0].Rows[0]["NGUOI_NHAP"].ToString();
                        if (!ds.Tables[0].Rows[0]["NGAY_CNHAT"].ToString().IsNullOrEmpty())
                            teldtNgayCNhat.Value = LDateTime.StringToDate(ds.Tables[0].Rows[0]["NGAY_CNHAT"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                        txtNguoiCapNhat.Text = ds.Tables[0].Rows[0]["NGUOI_CNHAT"].ToString();
                        dtVongVayVon.Rows.Clear();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string[] arrVongVay = new string[1];
                            arrVongVay[0] = ds.Tables[0].Rows[i]["SO_THU_TU"].ToString();
                            if (!ds.Tables[0].Rows[i]["SO_THU_TU"].ToString().IsNullOrEmpty())
                            {
                                DataRow dr = dtVongVayVon.NewRow();
                                dr[0] = ds.Tables[0].Rows[i]["SO_THU_TU"];
                                dr[1] = ds.Tables[0].Rows[i]["SO_TIEN"];
                                dr[2] = ds.Tables[0].Rows[i]["KY_HAN_GHEP"];
                                dr[3] = ds.Tables[0].Rows[i]["KY_HAN"];
                                dr[4] = ds.Tables[0].Rows[i]["KY_HAN_DVI_TINH"];
                                dr[5] = ds.Tables[0].Rows[i]["TOAN_TU"];
                                if (ds.Tables[0].Rows[i]["TOAN_TU"].Equals("="))
                                    dr[6] = arrVongVay[0];
                                else
                                    dr[6] = LLanguage.SearchResourceByKey("U.TinDung.ucVongVayCT.TuVong", arrVongVay);
                                dtVongVayVon.Rows.Add(dr);
                            }
                        }
                        raddgrVongVonDS.ItemsSource = null;
                        raddgrVongVonDS.ItemsSource = dtVongVayVon;
                    }
                    if (BSuaDuLieu)
                        Sua();
                    else
                    {
                        LockControl(false);
                        CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, sTrangThai, mnuMain, DatabaseConstant.Function.TDVM_VONG_VAY);
                    }
                }
            }
            catch (Exception ex) 
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        private void Xoa()
        {
            if (tlbDelete.IsEnabled == false)
                return;
            if (idVongVay > 0)
            {
                try
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        // Yêu cầu lock dữ liệu
                        List<int> lstId = new List<int>();
                        lstId.Add(idVongVay);
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_VONG_VAY,
                        DatabaseConstant.Table.TD_VONG_VAY,
                        DatabaseConstant.Action.XOA,
                        lstId);
                        if (!retLockData)
                            return;
                        TinDungProcess tindungProcess = new TinDungProcess();
                        int[] arrayiD = new int[] { idVongVay };
                        List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
                        bool bResult = tindungProcess.XoaVongVayTinDung(arrayiD,ref ResponseDetail);
                        CommonFunction.ThongBaoKetQua(ResponseDetail);
                        if (bResult)
                        {   
                            if (OnSavingComleted != null)
                                OnSavingComleted(null, EventArgs.Empty);
                            CommonFunction.CloseUserControl(this);
                        }
                        else
                        {

                        }
                        retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_VONG_VAY,
                        DatabaseConstant.Table.TD_VONG_VAY,
                        DatabaseConstant.Action.XOA,
                        lstId);
                    }
                }
                catch (Exception ex)
                {
                    LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                }
            }
            
        }

        /// <summary>
        /// Duyệt chi tiết
        /// </summary>
        private void Duyet()
        {
            if (tlbApprove.IsEnabled == false)
                return;
            List<int> lstID = new List<int>();
            lstID.Add(idVongVay);
            bool retLockData;
            // Yêu cầu lock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            try
            {
                if (idVongVay > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_VONG_VAY,
                        DatabaseConstant.Table.TD_VONG_VAY,
                        DatabaseConstant.Action.DUYET,
                        lstID);
                        if (!retLockData)
                            return;
                        List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
                        bool bResult = new TinDungProcess().DuyetVongVayTinDung(lstID.ToArray(), ref ResponseDetail);
                        if (bResult)
                        {
                            CommonFunction.ThongBaoKetQua(ResponseDetail);
                            LoadDuLieu();
                            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, sTrangThai, mnuMain, DatabaseConstant.Function.TDVM_VONG_VAY);
                            if (OnSavingComleted != null)
                                OnSavingComleted(null, EventArgs.Empty);
                            retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                           DatabaseConstant.Function.TDVM_VONG_VAY,
                           DatabaseConstant.Table.TD_VONG_VAY,
                           DatabaseConstant.Action.DUYET,
                           lstID);
                        }
                        else
                            LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);


                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                // Yêu cầu unlock dữ liệu
                retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_VONG_VAY,
                DatabaseConstant.Table.TD_VONG_VAY,
                DatabaseConstant.Action.DUYET,
                lstID);
            }
        }

        /// <summary>
        /// Thoái duyệt
        /// </summary>
        private void ThoaiDuyet()
        {
            if (tlbCancel.IsEnabled == false)
                return;
            List<int> lstID = new List<int>();
            lstID.Add(idVongVay);
            bool retLockData;
            // Yêu cầu lock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            try
            {
                if (idVongVay > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {

                        retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_VONG_VAY,
                        DatabaseConstant.Table.TD_VONG_VAY,
                        DatabaseConstant.Action.THOAI_DUYET,
                        lstID);
                        if (!retLockData)
                            return;
                        List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
                        bool bResult = new TinDungProcess().HuyDuyetVongVayTinDung(lstID.ToArray(), ref ResponseDetail);
                        if (bResult)
                        {
                            CommonFunction.ThongBaoKetQua(ResponseDetail);
                            LoadDuLieu();
                            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, sTrangThai, mnuMain, DatabaseConstant.Function.TDVM_VONG_VAY);
                            if (OnSavingComleted != null)
                                OnSavingComleted(null, EventArgs.Empty);
                            retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_VONG_VAY,
                        DatabaseConstant.Table.TD_VONG_VAY,
                        DatabaseConstant.Action.THOAI_DUYET,
                        lstID);
                        }
                        else
                            LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);

                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                // Yêu cầu unlock dữ liệu
                retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_VONG_VAY,
                DatabaseConstant.Table.TD_VONG_VAY,
                DatabaseConstant.Action.THOAI_DUYET,
                lstID);
            }
        }

        /// <summary>
        /// Từ chối duyệt
        /// </summary>
        private void TuChoiDuyet()
        {
            if (tlbRefuse.IsEnabled == false)
                return;
            List<int> lstID = new List<int>();
            lstID.Add(idVongVay);
            bool retLockData;
            // Yêu cầu lock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            try
            {
                if (idVongVay > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {

                        retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_VONG_VAY,
                        DatabaseConstant.Table.TD_VONG_VAY,
                        DatabaseConstant.Action.TU_CHOI_DUYET,
                        lstID);
                        if (!retLockData)
                            return;
                        List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
                        bool bResult = new TinDungProcess().TuChoiVongVayTinDung(lstID.ToArray(), ref ResponseDetail);
                        if (bResult)
                        {
                            CommonFunction.ThongBaoKetQua(ResponseDetail);
                            LoadDuLieu();
                            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, sTrangThai, mnuMain, DatabaseConstant.Function.TDVM_VONG_VAY);
                            if (OnSavingComleted != null)
                                OnSavingComleted(null, EventArgs.Empty);
                            retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_VONG_VAY,
                        DatabaseConstant.Table.TD_VONG_VAY,
                        DatabaseConstant.Action.TU_CHOI_DUYET,
                        lstID);
                        }
                        else
                            LMessage.ShowMessage("M.DungChung.LoiKhongXacDinh", LMessage.MessageBoxType.Error);

                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                // Yêu cầu unlock dữ liệu
                retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_VONG_VAY,
                DatabaseConstant.Table.TD_VONG_VAY,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstID);
            }
        }

        /// <summary>
        /// Selection Changed Vòng Vốn Vay
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void raddgrVongVonDS_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (dtVongVayVon.Rows.Count < 1)
                return;
            FrameworkElement originalSender = e.OriginalSource as FrameworkElement;
            if (originalSender != null)
            {
                LoadDataControl();
            }
        }
        #endregion

    }
}
