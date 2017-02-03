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
using Telerik.Windows.Controls;
using Presentation.Process.Common;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.TinDungServiceRef;
using System.Data;

namespace PresentationWPF.TinDung.LichTraNo
{
    /// <summary>
    /// Interaction logic for ucLichTraNoCT.xaml
    /// </summary>
    public partial class ucLichTraNoCT : UserControl
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
        List<AutoCompleteEntry> lstSourceBDauTra = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstNguyenNhanTDoi = new List<AutoCompleteEntry>();
        int idCum = 0;
        string maCum = "";
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        TDVM_LICH_THU_GOC_LAI objLichThuGocLai = new TDVM_LICH_THU_GOC_LAI();
        List<DANH_SACH_KHE_UOC_LAP_LICH> lstDanhSachKUOC = new List<DANH_SACH_KHE_UOC_LAP_LICH>();
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucLichTraNoCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDung;component/LichTraNo/ucLichTraNoCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            KhoiTaoGiaTriComboBox();
            dtpThangBDTra.SelectedDateChanged +=new EventHandler<SelectionChangedEventArgs>(dtpThangBDTra_SelectedDateChanged);
            btnCum.Click +=new RoutedEventHandler(btnCum_Click);
            tlbThem.Click +=new RoutedEventHandler(tlbThem_Click);
            tlbXoa.Click +=new RoutedEventHandler(tlbXoa_Click);
        }

        void KhoiTaoGiaTriComboBox()
        {
            List<string> lstMaChon = new List<string>();
            List<string> lstDieuKien = new List<string>();
            string sMaTruyVan = "";
            sMaTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            AutoComboBox auto = new AutoComboBox();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGUYEN_NHAN_THAY_DOI_LTN.getValue());
            lstNguyenNhanTDoi.Add(new AutoCompleteEntry("", "", ""));
            auto.GenAutoComboBox(ref lstNguyenNhanTDoi, ref cbbNguyenNhanThayDoi, sMaTruyVan, lstDieuKien);
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
            OnAction();
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
                //ClearForm();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                OnAction();
            }
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
                //ClearForm();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                OnAction();
            }
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

        void dtpThangBDTra_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboBoxBDauTraVon();
        }

        void LoadComboBoxBDauTraVon()
        {
            lstSourceBDauTra.Clear();
            cmbThangBDTra.Items.Clear();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(LDateTime.DateToString(teldtThangBDTra.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat));
            lstDieuKien.Add("THU_VON");
            lstDieuKien.Add(idCum.ToString());
            new AutoComboBox().GenAutoComboBox(ref lstSourceBDauTra, ref cmbThangBDTra, "COMBOBOX_DOT_THU_PHAT", lstDieuKien);
        }

        void  btnCum_Click(object sender, RoutedEventArgs e)
        {
 	        List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
            lstPopup = new List<DataRow>();
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation("POPUP_DS_CUM", lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            ucPopup popup = new ucPopup(false, simplePopupResponse, true);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.Content = popup;
            win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ShowDialog();
            if (lstPopup.Count > 0)
            {
                idCum = Convert.ToInt32(lstPopup[0]["ID"]);
                maCum = lstPopup[0]["MA_CUM"].ToString();
                lstDanhSachKUOC = new List<DANH_SACH_KHE_UOC_LAP_LICH>();
                raddgrLapKeHoach.ItemsSource = null;
                raddgrLapKeHoach.ItemsSource = lstDanhSachKUOC;
                txtCum.Text = maCum;
                LoadComboBoxBDauTraVon();
            }
        }

        void  tlbThem_Click(object sender, RoutedEventArgs e)
        {
 	        DataTable dt = null;
            DataSet ds = null;
            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@IdCum", "string", idCum.ToString());
            LDatatable.AddParameter(ref dt, "@MaCum", "string", maCum);
            LDatatable.AddParameter(ref dt, "@IDKhachHang", "string", "");
            if (!lstDanhSachKUOC.IsNullOrEmpty())
            {
                foreach (DANH_SACH_KHE_UOC_LAP_LICH objDSach in lstDanhSachKUOC)
                {
                    LDatatable.AddParameter(ref dt, "@IDKhachHang", "string", objDSach.MA_KHACH_HANG);
                }
            }
            else
                lstDanhSachKUOC = new List<DANH_SACH_KHE_UOC_LAP_LICH>();
            ds = new TruyVanProcess().TruyVanUDTT("_DC_LAP_LAI_LICH_THU_NO", dt);
            if (!ds.IsNullOrEmpty() && ds.Tables.Count>0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DANH_SACH_KHE_UOC_LAP_LICH objDSach = new DANH_SACH_KHE_UOC_LAP_LICH();
                    objDSach.ID_KUOCVM = Convert.ToInt32(dr["ID_KUOCVM"]);
                    objDSach.MA_KHACH_HANG = Convert.ToString(dr["MA_KHACH_HANG"]);
                    objDSach.MA_KUOCVM = Convert.ToString(dr["MA_KUOCVM"]);
                    objDSach.NGAY_BAT_DAU = Convert.ToString(dr["NGAY_BAT_DAU"]);
                    objDSach.NGAY_PHAT_VON = Convert.ToString(dr["NGAY_PHAT_VON"]);
                    objDSach.SO_DU = Convert.ToDecimal(dr["SO_DU"]);
                    objDSach.TEN_KHACH_HANG = Convert.ToString(dr["TEN_KHACH_HANG"]);
                    lstDanhSachKUOC.Add(objDSach);
                }
                raddgrLapKeHoach.ItemsSource = null;
                raddgrLapKeHoach.ItemsSource = lstDanhSachKUOC;
            }
        }

        void tlbXoa_Click(object sender, RoutedEventArgs e)
        {
            lstDanhSachKUOC = raddgrLapKeHoach.ItemsSource as List<DANH_SACH_KHE_UOC_LAP_LICH>;
            foreach (DANH_SACH_KHE_UOC_LAP_LICH objDanhSach in raddgrLapKeHoach.SelectedItems)
            {
                lstDanhSachKUOC.Remove(objDanhSach);
            }
            raddgrLapKeHoach.ItemsSource = null;
            raddgrLapKeHoach.ItemsSource = lstDanhSachKUOC;
        }
        #endregion

        /// <summary>
        /// Xu ly nghiep vu
        /// </summary>
        #region Xu ly nghiep vu
        private void OnAction()
        {
            Cursor = Cursors.Wait;
            int iret = 1;
            List<ClientResponseDetail> lstClientResponseDetail = new List<ClientResponseDetail>();
            try
            {
                if (!Validation())
                    return;
                if (objLichThuGocLai.IsNullOrEmpty())
                    objLichThuGocLai = new TDVM_LICH_THU_GOC_LAI();
                AutoCompleteEntry au = lstSourceBDauTra.ElementAt(cmbThangBDTra.SelectedIndex);
                AutoCompleteEntry auNN = lstNguyenNhanTDoi.ElementAt(cbbNguyenNhanThayDoi.SelectedIndex);
                objLichThuGocLai.DSACH_KUOCVM = lstDanhSachKUOC.ToArray();
                objLichThuGocLai.ID_CUM = idCum;
                objLichThuGocLai.MA_CUM = maCum;
                objLichThuGocLai.NGAY_BAT_DAU = au.KeywordStrings[2];
                objLichThuGocLai.NGUYEN_NHAN_TDOI = auNN.KeywordStrings.FirstOrDefault();
                iret = new TinDungProcess().LapLichThuGocLai(DatabaseConstant.Action.TINH_TOAN_LICH_TRA_NO, ref objLichThuGocLai, ref lstClientResponseDetail);
                CommonFunction.ThongBaoKetQua(lstClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        private bool Validation()
        {
            lstDanhSachKUOC = raddgrLapKeHoach.ItemsSource as List<DANH_SACH_KHE_UOC_LAP_LICH>;
            if (lstDanhSachKUOC.IsNullOrEmpty())
            {
                CommonFunction.ThongBaoTrong(grbDanhSach.Header.ToString() + ":");
                return false;
            }
            else if (cmbThangBDTra.SelectedIndex < 0)
            {
                CommonFunction.ThongBaoLoi("Chưa chọn ngày bắt đầu trả gốc lãi");
                return false;
            }
            return true;
        }
        #endregion
    }
}
