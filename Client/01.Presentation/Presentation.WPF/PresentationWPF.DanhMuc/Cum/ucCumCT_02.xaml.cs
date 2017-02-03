using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
using Telerik.Windows.Controls;
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.DanhMucServiceRef;
using PresentationWPF.CustomControl;

namespace PresentationWPF.DanhMuc.Cum
{
    /// <summary>
    /// Interaction logic for ucCumCT_02.xaml
    /// </summary>
    public partial class ucCumCT_02 : UserControl
    {
        #region Khai bao
        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private DatabaseConstant.Function function = DatabaseConstant.Function.DC_DM_CUM;

        public event EventHandler OnSavingCompleted;

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int idCumTruong = 0;

        private DM_CUM obj;
        public DM_CUM Obj
        {
            get { return obj; }
            set { obj = value; }
        }

        private string sTrangThaiNVu = "";

        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGiaoDich = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKhuVuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCBQL = new List<AutoCompleteEntry>();

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand AddCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        #endregion

        #region Khoi tao
        public ucCumCT_02()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            LoadCombobox();

            InitEventHandler();

        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.DanhMuc;component/Cum/ucCumCT_02.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void InitEventHandler()
        {
            chkThemNhieuLan.Checked += new RoutedEventHandler(chkThemNhieuLan_Checked);

            cmbChiNhanh.SelectionChanged += new SelectionChangedEventHandler(cmbChiNhanh_SelectionChanged);
            cmbPhongGiaoDich.SelectionChanged += new SelectionChangedEventHandler(cmbPhongGiaoDich_SelectionChanged);

            btnCumTruong.Click += btnCumTruong_Click;
            txtCumTruong.KeyDown += txtCumTruong_KeyDown;
        }

        private void LoadCombobox()
        {

            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();

            //Chi nhánh
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);

            if (cmbChiNhanh.SelectedIndex >= 0)
            {
                string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                string idChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.ElementAt(1);
                cmbPhongGiaoDich.Items.Clear();
                lstDieuKien.Clear();
                lstSourcePhongGiaoDich.Clear();
                lstDieuKien.Add(maChiNhanh);
                auto.GenAutoComboBox(ref lstSourcePhongGiaoDich, ref cmbPhongGiaoDich, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOMACHA.getValue(), lstDieuKien, ClientInformation.MaDonViGiaoDich);
            }

            if (cmbPhongGiaoDich.SelectedIndex >= 0)
            {
                string maPhongGD = lstSourcePhongGiaoDich.ElementAt(cmbPhongGiaoDich.SelectedIndex).KeywordStrings.First();
                string idPhongGD = lstSourcePhongGiaoDich.ElementAt(cmbPhongGiaoDich.SelectedIndex).KeywordStrings.ElementAt(1);

                cmbKhuVuc.Items.Clear();
                lstDieuKien.Clear();
                lstSourceKhuVuc.Clear();
                lstDieuKien.Add(idPhongGD);
                auto.GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUC.getValue(), lstDieuKien);

                cmbCBQL.Items.Clear();
                lstDieuKien.Clear();
                lstSourceCBQL.Clear();
                lstDieuKien.Add(maPhongGD);
                auto.GenAutoComboBox(ref lstSourceCBQL, ref cmbCBQL, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NSD.getValue(), lstDieuKien);                
            }

        }
        #endregion

        #region Dang ky hot key, shortcut key

        private void BindShortkey()
        {
            foreach (var child in Toolbar.Children)
            {
                if (child.GetType() == typeof(RibbonButton))
                {
                    RibbonButton tlb = (RibbonButton)child;
                    KeyBinding key = new KeyBinding();
                    string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.H, ModifierKeys.Control);
                        key = new KeyBinding(HoldCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(SaveCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.N, ModifierKeys.Control);
                        key = new KeyBinding(AddCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(ModifyCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.D, ModifierKeys.Control);
                        key = new KeyBinding(DeleteCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ApproveCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(RefuseCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.C, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(CancelCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(HelpCommand, keyg);
                    }
                    if (key != null)
                    {
                        InputBindings.Add(key);
                    }
                }
            }
        }

        private void HoldCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnSave();
        }

        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeAddNew();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeModifyFromDetail();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeDelete();
        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void HelpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnHelp();
        }

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnClose();
        }

        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                BeforeAddNew();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                BeforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
            }
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                OnHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                OnClose();
            }
        }

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                BeforeAddNew();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                BeforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
            }
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                OnHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                OnClose();
            }
        }

        #endregion

        #region Xu ly Giao dien
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Hiển thị Form khi thêm mới dữ liệu
            if (action == DatabaseConstant.Action.THEM)
            {
                BeforeAddNew();
            }

            //Hiển thị Form khi sửa dữ liệu
            else if (action == DatabaseConstant.Action.SUA)
            {
                BeforeModifyFromList();
            }

            //Hiển thị Form khi xem dữ liệu
            else if (action == DatabaseConstant.Action.XEM)
            {
                BeforeViewFromList();
            }
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

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        /// <summary>
        /// Hàm xử lý giải phóng lock dữ liệu cho form
        /// </summary>
        private void Release()
        {
            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool ret = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_CUM,
                DatabaseConstant.Table.DM_CUM,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void OnHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }

        /// <summary>
        /// Đóng form
        /// </summary>
        private void OnClose()
        {
            Release();
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        /// <summary>
        /// Sự kiện khi thay đổi giá trị của combobox cmbDonVi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbChiNhanh.SelectedIndex >= 0)
            {
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();

                string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                string idChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.ElementAt(1);
                cmbPhongGiaoDich.Items.Clear();
                lstDieuKien.Clear();
                lstSourcePhongGiaoDich.Clear();
                lstDieuKien.Add(maChiNhanh);
                auto.GenAutoComboBox(ref lstSourcePhongGiaoDich, ref cmbPhongGiaoDich, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOMACHA.getValue(), lstDieuKien);
            }
            else
            {
                cmbPhongGiaoDich.Items.Clear();
                lstSourcePhongGiaoDich.Clear();

                cmbCBQL.Items.Clear();
                lstSourceCBQL.Clear();
            }
        }

        private void cmbPhongGiaoDich_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPhongGiaoDich.SelectedIndex >= 0)
            {
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();

                string maPhongGD = lstSourcePhongGiaoDich.ElementAt(cmbPhongGiaoDich.SelectedIndex).KeywordStrings.First();
                string idPhongGD = lstSourcePhongGiaoDich.ElementAt(cmbPhongGiaoDich.SelectedIndex).KeywordStrings.ElementAt(1);

                cmbKhuVuc.Items.Clear();
                lstDieuKien.Clear();
                lstSourceKhuVuc.Clear();
                lstDieuKien.Add(idPhongGD);
                auto.GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUC.getValue(), lstDieuKien);

                cmbCBQL.Items.Clear();
                lstDieuKien.Clear();
                lstSourceCBQL.Clear();
                lstDieuKien.Add(maPhongGD);
                auto.GenAutoComboBox(ref lstSourceCBQL, ref cmbCBQL, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NSD.getValue(), lstDieuKien);
            }
            else
            {
                cmbCBQL.Items.Clear();
                lstSourceCBQL.Clear();
            }
        }

        private void chkThemNhieuLan_Checked(object sender, RoutedEventArgs e)
        {
            //action = DatabaseConstant.Action.THEM;
            //ResetForm();
            //SetEnabledControls();
            //CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
            //txtTenNhanVien.Focus();

            //chkThemNhieuLan.IsChecked = false;
        }

        private void btnCumTruong_Click(object sender, RoutedEventArgs e)
        {
            ShowPopupKhachHang();
        }

        private void txtCumTruong_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnCumTruong_Click(null, null);
            }
        }

        private void ShowPopupKhachHang()
        {
            if (Convert.ToInt32(lstSourcePhongGiaoDich.ElementAt(cmbPhongGiaoDich.SelectedIndex).KeywordStrings.ElementAt(1)) > 0)
            {
                if (chkCumTruongLaKHTV.IsChecked == true)
                {
                    if (!txtMaCum.Text.IsNullOrEmptyOrSpace())
                    {
                        List<string> lstDieuKien = new List<string>();
                        lstDieuKien.Add(lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.ElementAt(0));
                        lstDieuKien.Add(txtMaCum.Text);
                        var process = new PopupProcess();
                        process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_KHACHHANG.getValue(), lstDieuKien);

                        SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                        ucPopup popup = new ucPopup(true, simplePopupResponse, true);
                        popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                        Window win = new Window();
                        win.Content = popup;
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.ShowDialog();
                        if (lstPopup != null && lstPopup.Count > 0)
                        {
                            DataRow row = lstPopup[0];
                            idCumTruong = Convert.ToInt32(row[1].ToString());
                            txtCumTruong.Text = row[2].ToString();
                            txtTenCumTruong.Text = row[3].ToString();
                        }
                    }
                    else
                    {
                        LMessage.ShowMessage("Chưa có thông tin khách hàng thành viên", LMessage.MessageBoxType.Warning);
                    }
                }
                else
                {
                    List<string> lstDieuKien = new List<string>();
                    lstDieuKien.Add(lstSourcePhongGiaoDich.ElementAt(cmbPhongGiaoDich.SelectedIndex).KeywordStrings.ElementAt(0));
                    lstDieuKien.Add(BusinessConstant.LOAI_HO_SO.CONG_TAC_VIEN.layGiaTri());
                    lstDieuKien.Add("(0)");
                    var process = new PopupProcess();
                    process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_HOSO_NHAN_SU_CTV.getValue(), lstDieuKien);

                    SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                    ucPopup popup = new ucPopup(false, simplePopupResponse, true);
                    popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                    Window win = new Window();
                    win.Content = popup;
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    win.ShowDialog();
                    if (lstPopup != null && lstPopup.Count > 0)
                    {
                        DataRow row = lstPopup[0];
                        idCumTruong = Convert.ToInt32(row[1].ToString());
                        txtCumTruong.Text = row[2].ToString();
                        txtTenCumTruong.Text = row[3].ToString();
                    }
                }               
            }
            else
                LMessage.ShowMessage("Chưa chọn phòng giao dịch", LMessage.MessageBoxType.Warning);
        }

        private void chkCumTruongLaKHTV_Checked(object sender, RoutedEventArgs e)
        {
            idCumTruong = 0;
            txtCumTruong.Text = "";
            txtTenCumTruong.Text = "";
        }

        private void chkCumTruongLaKHTV_Unchecked(object sender, RoutedEventArgs e)
        {
            idCumTruong = 0;
            txtCumTruong.Text = "";
            txtTenCumTruong.Text = "";
        }

        #endregion

        #region Xử lý nghiệp vụ
        private void GetFormData(ref DM_CUM obj, string sTrangThaiNVu)
        {
            try
            {
                obj = new DM_CUM();

                obj.ID = id;
                obj.ID_DVI = Convert.ToInt32(lstSourcePhongGiaoDich.ElementAt(cmbPhongGiaoDich.SelectedIndex).KeywordStrings.ElementAt(1));
                obj.ID_KVUC = Convert.ToInt32(lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings.ElementAt(1));
                obj.MA_CUM = txtMaCum.Text;
                obj.MA_KVUC = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings.First();
                obj.MA_DVI = lstSourcePhongGiaoDich.ElementAt(cmbPhongGiaoDich.SelectedIndex).KeywordStrings.First();
                if (raddtNgayThanhLap.Value != null)
                    obj.NGAY_TLAP = Convert.ToDateTime(raddtNgayThanhLap.Value).ToString("yyyyMMdd");
                obj.TEN_CUM = txtTenCum.Text;
                obj.TEN_TAT = txtTenTat.Text;
                obj.ID_CBO_QLY = Convert.ToInt32(lstSourceCBQL.ElementAt(cmbCBQL.SelectedIndex).KeywordStrings.ElementAt(1));
                obj.MA_CBO_QLY = lstSourceCBQL.ElementAt(cmbCBQL.SelectedIndex).KeywordStrings.First();                
                obj.ID_CUM_TRUONG = idCumTruong;
                obj.MA_CUM_TRUONG = txtCumTruong.Text;
                if (chkCumTruongLaKHTV.IsChecked == true)
                    obj.LOAI_CUM_TRUONG = "KHTV";
                else
                    obj.LOAI_CUM_TRUONG = "CONG_TAC_VIEN";

                //Thông tin kiểm soát
                obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.TTHAI_NVU = sTrangThaiNVu;
                obj.MA_DVI_QLY = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                obj.MA_DVI_TAO = obj.MA_DVI;
                obj.NGAY_NHAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                obj.NGUOI_NHAP = txtNguoiLap.Text;
                if (action != DatabaseConstant.Action.THEM)
                {
                    obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            DanhMucProcess processDanhMuc = new DanhMucProcess();

            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            obj = new DM_CUM();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                bool ret = false;
                obj.ID = id;
                DM_TEMP_CUM objTempCum = new DM_TEMP_CUM();
                ret = processDanhMuc.Cum02(DatabaseConstant.Action.LOAD, ref obj, ref objTempCum, ref listClientResponseDetail);
                if (ret == true)
                {
                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    #region Thông tin chung
                    cmbChiNhanh.SelectedIndex = lstSourceChiNhanh.IndexOf(lstSourceChiNhanh.FirstOrDefault(e => e.KeywordStrings.First().Equals(obj.MA_DVI_QLY)));
                    cmbPhongGiaoDich.SelectedIndex = lstSourcePhongGiaoDich.IndexOf(lstSourcePhongGiaoDich.FirstOrDefault(e => e.KeywordStrings.First().Equals(obj.MA_DVI)));
                    cmbPhongGiaoDich_SelectionChanged(null, null);
                    cmbKhuVuc.SelectedIndex = lstSourceKhuVuc.IndexOf(lstSourceKhuVuc.FirstOrDefault(e => e.KeywordStrings.First().Equals(obj.MA_KVUC)));
                    txtMaCum.Text = obj.MA_CUM;
                    txtTenCum.Text = obj.TEN_CUM;
                    txtTenTat.Text = obj.TEN_TAT;
                    if (LDateTime.IsDate(obj.NGAY_TLAP, "yyyyMMdd"))
                        raddtNgayThanhLap.Value = LDateTime.StringToDate(obj.NGAY_TLAP, "yyyyMMdd");
                    cmbCBQL.SelectedIndex = lstSourceCBQL.IndexOf(lstSourceCBQL.FirstOrDefault(e => e.KeywordStrings.First().Equals(obj.MA_CBO_QLY)));
                    if (obj.LOAI_CUM_TRUONG.Equals("KHTV"))
                        chkCumTruongLaKHTV.IsChecked = true;
                    else
                        chkCumTruongLaKHTV.IsChecked = false;
                    txtCumTruong.Text = obj.MA_CUM_TRUONG;
                    if (objTempCum != null)
                        txtTenCumTruong.Text = objTempCum.TEN_CUM_TRUONG;                    
                    #endregion

                    #region Thông tin kiểm soát
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    raddtNgayLap.Value = LDateTime.StringToDate(obj.NGAY_NHAP, "yyyyMMdd");
                    txtNguoiLap.Text = obj.NGUOI_NHAP;
                    if (LDateTime.IsDate(obj.NGAY_CNHAT, "yyyyMMdd") == true)
                        raddtNgayCapNhat.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, "yyyyMMdd");
                    txtNguoiCapNhat.Text = obj.NGUOI_CNHAT;
                    #endregion

                }
                else
                {
                    LMessage.ShowMessage("M_ResponseMessage_HanhDong_LayDuLieuKhongThanhCong", LMessage.MessageBoxType.Warning);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M_ResponseMessage_HanhDong_LayDuLieuKhongThanhCong", LMessage.MessageBoxType.Warning);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void ResetForm()
        {
            sTrangThaiNVu = "";
            lblTrangThai.Content = "";
            id = 0;
            idCumTruong = 0;
            obj = null;

            #region Thông tin chung
            txtMaCum.Text = "";
            txtTenCum.Text = "";
            txtTenTat.Text = "";
            raddtNgayThanhLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            chkCumTruongLaKHTV.IsChecked = true;
            txtCumTruong.Text = "";
            txtTenCumTruong.Text = "";
            #endregion

            #region Thông tin kiểm soát
            txtTrangThaiBanGhi.Text = "";
            raddtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            raddtNgayCapNhat.Value = null;
            txtNguoiCapNhat.Text = "";
            #endregion

        }

        private bool Validation()
        {
            try
            {
                if (cmbChiNhanh.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblChiNhanh.Content.ToString());
                    cmbChiNhanh.Focus();
                    return false;
                }

                if (cmbPhongGiaoDich.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblPhongGiaoDich.Content.ToString());
                    cmbPhongGiaoDich.Focus();
                    return false;
                }

                if (txtTenCum.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaChon(lblTenCum.Content.ToString());
                    txtTenCum.Focus();
                    return false;
                }

                if (txtTenTat.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaChon(lblTenTat.Content.ToString());
                    txtTenTat.Focus();
                    return false;
                }

                if (cmbCBQL.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblCBQL.Content.ToString());
                    cmbCBQL.Focus();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                return false;
            }
        }

        private void SetEnabledControls()
        {
            #region Thêm
            if (action == DatabaseConstant.Action.THEM)
            {
                cmbChiNhanh.IsEnabled = false;
                cmbPhongGiaoDich.IsEnabled = false;
                cmbKhuVuc.IsEnabled = true;
                txtMaCum.IsEnabled = false;
                txtTenCum.IsEnabled = true;
                txtTenTat.IsEnabled = true;
                raddtNgayThanhLap.IsEnabled = true;
                dtpNgayThanhLap.IsEnabled = true;
                cmbCBQL.IsEnabled = true;
                chkCumTruongLaKHTV.IsEnabled = true;
                txtCumTruong.IsEnabled = true;
                btnCumTruong.IsEnabled = true;
                txtTenCumTruong.IsEnabled = true;

            }
            #endregion

            #region Sửa
            else if (action == DatabaseConstant.Action.SUA)
            {
                cmbChiNhanh.IsEnabled = false;
                cmbPhongGiaoDich.IsEnabled = false;
                cmbKhuVuc.IsEnabled = false;
                txtMaCum.IsEnabled = false;
                txtTenCum.IsEnabled = true;
                txtTenTat.IsEnabled = true;
                raddtNgayThanhLap.IsEnabled = true;
                dtpNgayThanhLap.IsEnabled = true;
                cmbCBQL.IsEnabled = true;
                chkCumTruongLaKHTV.IsEnabled = true;
                txtCumTruong.IsEnabled = true;
                btnCumTruong.IsEnabled = true;
                txtTenCumTruong.IsEnabled = true;

            }
            #endregion

            #region Xem ~ Mặc định
            else
            {
                cmbChiNhanh.IsEnabled = false;
                cmbPhongGiaoDich.IsEnabled = false;
                cmbKhuVuc.IsEnabled = false;
                txtMaCum.IsEnabled = false;
                txtTenCum.IsEnabled = false;
                txtTenTat.IsEnabled = false;
                raddtNgayThanhLap.IsEnabled = false;
                dtpNgayThanhLap.IsEnabled = false;
                cmbCBQL.IsEnabled = false;
                chkCumTruongLaKHTV.IsEnabled = false;
                txtCumTruong.IsEnabled = false;
                btnCumTruong.IsEnabled = false;
                txtTenCumTruong.IsEnabled = false;
            }
            #endregion
        }


        public void OnSave()
        {
            try
            {
                if (!Validation()) return;

                string trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();

                obj = new DM_CUM();

                GetFormData(ref obj, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(obj);
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(obj);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }

        }

        public void BeforeViewFromDetail()
        {
            action = DatabaseConstant.Action.XEM;
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void BeforeViewFromList()
        {
            SetFormData();
            BeforeViewFromDetail();
        }


        public void BeforeAddNew()
        {
            action = DatabaseConstant.Action.THEM;
            ResetForm();
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
            txtTenCum.Focus();
        }

        public void OnAddNew(DM_CUM obj)
        {

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                DanhMucProcess processDanhMuc = new DanhMucProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                DM_TEMP_CUM objTempCum = new DM_TEMP_CUM();
                ret = processDanhMuc.Cum02(DatabaseConstant.Action.THEM, ref obj, ref objTempCum, ref listClientResponseDetail);
                AfterAddNew(ret, obj, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public void AfterAddNew(bool ret, DM_CUM obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                    if (chkThemNhieuLan.IsChecked == true)
                    {
                        BeforeAddNew();
                    }
                    else
                    {
                        id = obj.ID;
                        txtMaCum.Text = obj.MA_CUM;

                        sTrangThaiNVu = obj.TTHAI_NVU;
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);

                        BeforeViewFromDetail();
                    }
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        public void BeforeModifyFromDetail()
        {
            try
            {
                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);

                bool ret = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_CUM,
                    DatabaseConstant.Table.DM_CUM,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                if (ret)
                {
                    action = DatabaseConstant.Action.SUA;
                    SetEnabledControls();
                    CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
                }
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void BeforeModifyFromList()
        {
            SetFormData();
            action = DatabaseConstant.Action.SUA;
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnModify(DM_CUM obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                DanhMucProcess processDanhMuc = new DanhMucProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                DM_TEMP_CUM objTempCum = new DM_TEMP_CUM();
                ret = processDanhMuc.Cum02(DatabaseConstant.Action.SUA, ref obj, ref objTempCum, ref listClientResponseDetail);
                AfterModify(ret, obj, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public void AfterModify(bool ret, DM_CUM obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    raddtNgayCapNhat.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                    txtNguoiCapNhat.Text = ClientInformation.TenDangNhap;

                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    //LMessage.ShowMessage("M.DungChung.CapNhatKoThanhCong", LMessage.MessageBoxType.Error);
                }

                // Yêu cầu Unlock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_CUM,
                    DatabaseConstant.Table.DM_CUM,
                    DatabaseConstant.Action.SUA,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        public void BeforeDelete()
        {
            try
            {
                // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_DM_CUM,
                        DatabaseConstant.Table.DM_CUM,
                        DatabaseConstant.Action.XOA,
                        listLockId);

                    if (retLockData)
                    {
                        action = DatabaseConstant.Action.XOA;
                        OnDelete();
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_CUM,
                    DatabaseConstant.Table.DM_CUM,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnDelete()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            DanhMucProcess processDanhMuc = new DanhMucProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                DM_TEMP_CUM objTempCum = new DM_TEMP_CUM();
                ret = processDanhMuc.Cum02(action, ref obj, ref objTempCum, ref listClientResponseDetail);
                AfterDelete(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                listClientResponseDetail = null;
                processDanhMuc = null;
            }
        }

        public void AfterDelete(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    //LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_CUM,
                    DatabaseConstant.Table.DM_CUM,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                // Đóng cửa sổ chi tiết sau khi xóa
                OnClose();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        #endregion

        
    }
}
