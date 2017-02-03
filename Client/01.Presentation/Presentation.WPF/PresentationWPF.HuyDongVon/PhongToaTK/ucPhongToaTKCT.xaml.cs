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
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.Common;
using PresentationWPF.CustomControl;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.HuyDongVonServiceRef;
using Presentation.Process.LaiSuatServiceRef;
using Presentation.Process.KhachHangServiceRef;

namespace PresentationWPF.HuyDongVon.PhongToaTK
{
    /// <summary>
    /// Interaction logic for ucPhongToaTKCT.xaml
    /// </summary>
    public partial class ucPhongToaTKCT : UserControl
    {
        #region Chua khai bao Constanst, Language
        /*
         * DatabaseConstant.DanhMuc.LOAI_GDICH_PTOA: PHONG_TOA, GIAI_TOA
         * DatabaseConstant.DanhMuc.PHUONG_THUC_PHONG_TOA: TBSD, MPSD
         * 
         * 
        */
        #endregion

        #region Khai bao
        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        public event EventHandler OnSavingCompleted;

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private KIEM_SOAT _objKiemSoat = null;

        private HDV_PHONG_TOA_SO_DU objPhongToa;
        public HDV_PHONG_TOA_SO_DU ObjPhongToa
        {
            get { return objPhongToa; }
            set { objPhongToa = value; }
        }

        private HDV_GIAI_TOA_SO_DU objGiaiToa;
        public HDV_GIAI_TOA_SO_DU ObjGiaiToa
        {
            get { return objGiaiToa; }
            set { objGiaiToa = value; }
        }

        private string sTrangThaiNVu = "";

        private string sLoaiGD = "";
        private string sPhuongThucPT = ""; 

        private DatabaseConstant.Function function = DatabaseConstant.Function.HDV_PHONG_TOA_SD;

        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceGiaoDich = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhuongThucPT = new List<AutoCompleteEntry>();   

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        /*Cờ đánh dấu trạng thái khi LoadForm: 
         * 0 là khi gọi từ Main chương trình lần đầu
         * 1 là khi thêm từ Form danh sách
         * 2 là khi sửa từ Form danh sách 
         * 3 là khi xem từ Form danh sách
         * -1 là Khi đã load Form tránh trường hợp load nhiều lần
        */
        private int flag = 0;
        public int Flag
        {
            get { return flag; }
            set { flag = value; }
        }
        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        #endregion

        #region Khoi tao
        public ucPhongToaTKCT()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            LoadCombobox();

            txtSoTGui.Focus();
        }

        public ucPhongToaTKCT(KIEM_SOAT obj)
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            LoadCombobox();

            _objKiemSoat = obj;

            txtSoTGui.Focus();
        }
        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.HuyDongVon;component/PhongToaTK/ucPhongToaTKCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void LoadCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();

            auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TIENTE.getValue());
            cmbLoaiTien.SelectedIndex = 0;

            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.LOAI_GDICH_PTOA));
            auto.GenAutoComboBox(ref lstSourceGiaoDich, ref cmbGiaoDich, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, "PHONG_TOA");            
            sLoaiGD = "PHONG_TOA";
            function = DatabaseConstant.Function.HDV_PHONG_TOA_SD;

            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.PHUONG_THUC_PHONG_TOA));
            auto.GenAutoComboBox(ref lstSourcePhuongThucPT, ref cmbPhuongThucPT, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien,"TBSD");
            sPhuongThucPT = "TBSD";
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(SaveCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(ModifyCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.Shift);
                        key = new KeyBinding(DeleteCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.A, ModifierKeys.Control | ModifierKeys.Shift);
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
            OnHold();
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnSave();
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
            BeforeApprove();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeRefuse();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeCancel();
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
                OnHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                OnSave();
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
                OnHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                OnSave();
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
            BeforeAddNew();
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

            bool ret = process.UnlockData(DatabaseConstant.Module.HDVO,
                DatabaseConstant.Function.HDV_TAT_TOAN,
                DatabaseConstant.Table.BL_TIEN_GUI,
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

        private void SetEnabledAllControls(bool enable)
        {
            txtSoTGui.IsEnabled = enable;
            btnSoTGui.IsEnabled = enable;
            cmbGiaoDich.IsEnabled = enable;
            if (enable == true)
            {
                if (sLoaiGD.Equals("PHONG_TOA"))
                {
                    raddtPTTuNgay.IsEnabled = true;
                    dtpPTTuNgay.IsEnabled = true;
                    raddtPTDenNgay.IsEnabled = true;
                    dtpPTDenNgay.IsEnabled = true;
                    cmbPhuongThucPT.IsEnabled = true;
                    if (sPhuongThucPT.Equals("TBSD"))
                    {
                        numSoTienPT.IsEnabled = false;
                    }
                    else
                    {
                        numSoTienPT.IsEnabled = true;
                    }
                }
                else if (sLoaiGD.Equals("GIAI_TOA"))
                {
                    raddtPTTuNgay.IsEnabled = false;
                    dtpPTTuNgay.IsEnabled = false;
                    raddtPTDenNgay.IsEnabled = true;
                    dtpPTDenNgay.IsEnabled = true;
                    cmbPhuongThucPT.IsEnabled = false;
                    numSoTienPT.IsEnabled = false;
                }
            }
            else
            {
                raddtPTTuNgay.IsEnabled = false;
                dtpPTTuNgay.IsEnabled = false;
                raddtPTDenNgay.IsEnabled = false;
                dtpPTDenNgay.IsEnabled = false;
                cmbPhuongThucPT.IsEnabled = false;
                numSoTienPT.IsEnabled = false;
            }

            txtDienGiai.IsEnabled = enable;

        }

        private void txtSoTGui_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnSoTGui_Click(null, null);
            }
        }

        private void txtSoTGui_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtSoTGui.Text.IsNullOrEmptyOrSpace())
                {
                    HuyDongVonProcess processHDV = new HuyDongVonProcess();

                    DataTable dt = null;

                    LDatatable.MakeParameterTable(ref dt);
                    LDatatable.AddParameter(ref dt, "@SO_TGUI", "STRING", txtSoTGui.Text);
                    LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", ClientInformation.MaDonViGiaoDich);
                    DataSet ds = processHDV.GetThongTinSoTGuiTheoMa(dt);
                    
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];

                        #region Hiển thị thông tin sổ
                        lblIDSoTGui.Content = dr["ID"].ToString();
                        txtSoTGui.Text = dr["SO_SO_TG"].ToString();
                        numSoDu.Value = Convert.ToDouble(dr["SO_TIEN"]);
                        cmbLoaiTien.SelectedIndex = lstSourceLoaiTien.IndexOf(lstSourceLoaiTien.FirstOrDefault(i => i.KeywordStrings.First().Equals(dr["MA_LOAI_TIEN"].ToString())));
                        numLaiSuat.Value = Convert.ToDouble(dr["LAI_SUAT"]);
                        if (LDateTime.IsDate(dr["NGAY_MO_SO"].ToString(), "yyyyMMdd"))
                            raddtNgayMo.Value = LDateTime.StringToDate(dr["NGAY_MO_SO"].ToString(), "yyyyMMdd");
                        else
                            raddtNgayMo.Value = null;

                        if (LDateTime.IsDate(dr["NGAY_DEN_HAN"].ToString(), "yyyyMMdd"))
                            raddtNgayDH.Value = LDateTime.StringToDate(dr["NGAY_DEN_HAN"].ToString(), "yyyyMMdd");
                        else
                            raddtNgayDH.Value = null;
                        #endregion

                        #region Hiển thị thông tin khách hàng
                        lblIDKhachHang.Content = dr["ID_KHANG"].ToString();
                        txtMaKH.Text = dr["MA_KHANG"].ToString();
                        txtTenKH.Text = dr["TEN_KHANG"].ToString();
                        txtDiaChi.Text = dr["DIA_CHI"].ToString();
                        txtSDT.Text = dr["SO_DTHOAI"].ToString();

                        if (dr["DD_GTLQ_LOAI"].ToString().Equals("CHUNG_MINH_ND"))
                        {
                            txtSoCMT.Text = dr["DD_GTLQ_SO"].ToString();
                            if (LDateTime.IsDate(dr["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd"))
                                raddtNgayCap.Value = LDateTime.StringToDate(dr["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd");
                            else
                                raddtNgayCap.Value = null;
                            txtNoiCap.Text = dr["DD_GTLQ_NOI_CAP"].ToString();
                        }
                        #endregion

                        #region Xử lý khác
                        numSoTienPT.Value = Convert.ToDouble(dr["SO_TIEN"]);
                        #endregion
                    }
                    else
                    {
                        lblIDSoTGui.Content = "0";
                        txtSoTGui.Text = "";
                        numSoDu.Value = null;
                        cmbLoaiTien.Text = "";
                        numLaiSuat.Value = null;
                        raddtNgayMo.Value = null;
                        raddtNgayDH.Value = null;

                        lblIDKhachHang.Content = "0";
                        txtMaKH.Text = "";
                        txtTenKH.Text = "";
                        txtDiaChi.Text = "";
                        txtSDT.Text = "";
                        txtSoCMT.Text = "";
                        raddtNgayCap.Value = null;
                        txtNoiCap.Text = "";

                    }
                }
                else
                {
                    lblIDSoTGui.Content = "0";
                    txtSoTGui.Text = "";
                    numSoDu.Value = null;
                    numLaiSuat.Value = null;
                    cmbLoaiTien.Text = "";
                    raddtNgayMo.Value = null;
                    raddtNgayDH.Value = null;

                    lblIDKhachHang.Content = "0";
                    txtMaKH.Text = "";
                    txtTenKH.Text = "";
                    txtDiaChi.Text = "";
                    txtSDT.Text = "";
                    txtSoCMT.Text = "";
                    raddtNgayCap.Value = null;
                    txtNoiCap.Text = "";

                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void btnSoTGui_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //list điều kiện theo từng loại sổ
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonVi);

                var process = new PopupProcess();
                process.getPopupInformation("POPUP_DS_SO_TGUI_HDV", lstDieuKien);

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    HuyDongVonProcess processHDV = new HuyDongVonProcess();
                    DataRow row = lstPopup[0];
                    int idSoTGui = Convert.ToInt32(row[1]);
                    DataSet ds = processHDV.GetThongTinQTrongSoTGui(idSoTGui);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];

                        #region Hiển thị thông tin sổ
                        lblIDSoTGui.Content = dr["ID"].ToString();
                        txtSoTGui.Text = dr["SO_SO_TG"].ToString();
                        numSoDu.Value = Convert.ToDouble(dr["SO_TIEN"]);                        
                        cmbLoaiTien.SelectedIndex = lstSourceLoaiTien.IndexOf(lstSourceLoaiTien.FirstOrDefault(i => i.KeywordStrings.First().Equals(dr["MA_LOAI_TIEN"].ToString())));
                        numLaiSuat.Value = Convert.ToDouble(dr["LAI_SUAT"]);
                        if (LDateTime.IsDate(dr["NGAY_MO_SO"].ToString(), "yyyyMMdd"))
                            raddtNgayMo.Value = LDateTime.StringToDate(dr["NGAY_MO_SO"].ToString(), "yyyyMMdd");
                        else
                            raddtNgayMo.Value = null;

                        if (LDateTime.IsDate(dr["NGAY_DEN_HAN"].ToString(), "yyyyMMdd"))
                            raddtNgayDH.Value = LDateTime.StringToDate(dr["NGAY_DEN_HAN"].ToString(), "yyyyMMdd");
                        else
                            raddtNgayDH.Value = null;
                        #endregion

                        #region Hiển thị thông tin khách hàng
                        lblIDKhachHang.Content = dr["ID_KHANG"].ToString();
                        txtMaKH.Text = dr["MA_KHANG"].ToString();
                        txtTenKH.Text = dr["TEN_KHANG"].ToString();
                        txtDiaChi.Text = dr["DIA_CHI"].ToString();
                        txtSDT.Text = dr["SO_DTHOAI"].ToString();

                        if (dr["DD_GTLQ_LOAI"].ToString().Equals("CHUNG_MINH_ND"))
                        {
                            txtSoCMT.Text = dr["DD_GTLQ_SO"].ToString();
                            if (LDateTime.IsDate(dr["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd"))
                                raddtNgayCap.Value = LDateTime.StringToDate(dr["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd");
                            else
                                raddtNgayCap.Value = null;
                            txtNoiCap.Text = dr["DD_GTLQ_NOI_CAP"].ToString();
                        }
                        #endregion

                        #region Xử lý khác
                        numSoTienPT.Value = Convert.ToDouble(dr["SO_TIEN"]); 
                        #endregion
                    }

                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void RadComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                sLoaiGD = lstSourceGiaoDich.ElementAt(cmbGiaoDich.SelectedIndex).KeywordStrings.ElementAt(0);
                if (sLoaiGD.Equals("PHONG_TOA"))
                {
                    function = DatabaseConstant.Function.HDV_PHONG_TOA_SD;
                    raddtPTTuNgay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                    raddtPTTuNgay.IsEnabled = true;
                    dtpPTTuNgay.IsEnabled = true;
                    cmbPhuongThucPT.IsEnabled = true;
                    numSoTienPT.IsEnabled = true;
                }
                else if (sLoaiGD.Equals("GIAI_TOA"))
                {
                    function = DatabaseConstant.Function.HDV_GIAI_TOA_SD;
                    raddtPTTuNgay.Value = null;
                    raddtPTTuNgay.IsEnabled = false;
                    dtpPTTuNgay.IsEnabled = false;
                    cmbPhuongThucPT.IsEnabled = false;
                    numSoTienPT.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            
        }

        private void cmbPhuongThucPT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sPhuongThucPT = lstSourcePhuongThucPT.ElementAt(cmbPhuongThucPT.SelectedIndex).KeywordStrings.ElementAt(0);
            if (sPhuongThucPT.Equals("TBSD"))
            {
                numSoTienPT.IsEnabled = false;
            }
            else if (sPhuongThucPT.Equals("MPSD"))
            {
                numSoTienPT.IsEnabled = true;
            }
        }    
        #endregion      

        #region Xử lý nghiệp vụ
        private void GetFormData(ref HDV_PHONG_TOA_SO_DU objPhongToa,ref HDV_GIAI_TOA_SO_DU objGiaiToa, string sTrangThaiNVu)
        {
            try
            {
                if (sLoaiGD.Equals("PHONG_TOA"))
                {
                    //Thông tin tài khoản
                    objPhongToa.MA_GDICH = txtSoGD.Text;
                    objPhongToa.NGAY_GDICH = ClientInformation.NgayLamViecHienTai;
                    objPhongToa.SO_TAI_KHOAN = txtSoTGui.Text;
                    objPhongToa.SO_DU = Convert.ToDecimal(numSoDu.Value);
                    objPhongToa.LOAI_TIEN = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.ElementAt(0);
                    objPhongToa.LAI_SUAT = Convert.ToDecimal(numLaiSuat.Value);
                    objPhongToa.NGAY_MO_SO = Convert.ToDateTime(raddtNgayMo.Value).ToString("yyyyMMdd");
                    objPhongToa.NGAY_DAO_HAN = Convert.ToDateTime(raddtNgayDH.Value).ToString("yyyyMMdd");

                    //Thông tin khách hàng
                    objPhongToa.MA_KHACH_HANG = txtMaKH.Text;
                    objPhongToa.TEN_KHACH_HANG = txtTenKH.Text;
                    objPhongToa.DIA_CHI = txtDiaChi.Text;
                    objPhongToa.SO_CMND = txtSoCMT.Text;
                    objPhongToa.NGAY_CAP = Convert.ToDateTime(raddtNgayCap.Value).ToString("yyyyMMdd");
                    objPhongToa.NOI_CAP = txtNoiCap.Text;
                    objPhongToa.SO_DIEN_THOAI = txtSDT.Text;

                    //Thông tin phong tỏa số dư
                    objPhongToa.PHONG_TOA_TU_NGAY = Convert.ToDateTime(raddtPTTuNgay.Value).ToString("yyyyMMdd");
                    objPhongToa.PHONG_TOA_DEN_NGAY = Convert.ToDateTime(raddtPTDenNgay.Value).ToString("yyyyMMdd");
                    objPhongToa.PHUONG_THUC_PHONG_TOA = sPhuongThucPT;
                    objPhongToa.DIEN_GIAI = txtDienGiai.Text;

                    //Thông tin kiểm soát
                    objPhongToa.TRANG_THAI_NGHIEP_VU = sTrangThaiNVu;
                    objPhongToa.TRANG_THAI_BAN_GHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    objPhongToa.NGAY_LAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                    objPhongToa.NGUOI_LAP = txtNguoiLap.Text;
                    if (action != DatabaseConstant.Action.THEM)
                    {
                        objPhongToa.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                        objPhongToa.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                    }
                    
                }
                else if (sLoaiGD.Equals("GIAI_TOA"))
                {
                    //Thông tin tài khoản
                    objGiaiToa.MA_GDICH = txtSoGD.Text;
                    objGiaiToa.NGAY_GDICH = ClientInformation.NgayLamViecHienTai;
                    objGiaiToa.SO_TAI_KHOAN = txtSoTGui.Text;
                    objGiaiToa.SO_DU = Convert.ToDecimal(numSoDu.Value);
                    objGiaiToa.LOAI_TIEN = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.ElementAt(0);
                    objGiaiToa.LAI_SUAT = Convert.ToDecimal(numLaiSuat.Value);
                    objGiaiToa.NGAY_MO_SO = Convert.ToDateTime(raddtNgayMo.Value).ToString("yyyyMMdd");
                    objGiaiToa.NGAY_DAO_HAN = Convert.ToDateTime(raddtNgayDH.Value).ToString("yyyyMMdd");

                    //Thông tin khách hàng
                    objGiaiToa.MA_KHACH_HANG = txtMaKH.Text;
                    objGiaiToa.TEN_KHACH_HANG = txtTenKH.Text;
                    objGiaiToa.DIA_CHI = txtDiaChi.Text;
                    objGiaiToa.SO_CMND = txtSoCMT.Text;
                    objGiaiToa.NGAY_CAP = Convert.ToDateTime(raddtNgayCap.Value).ToString("yyyyMMdd");
                    objGiaiToa.NOI_CAP = txtNoiCap.Text;
                    objGiaiToa.SO_DIEN_THOAI = txtSDT.Text;

                    //Thông tin phong tỏa số dư
                    objGiaiToa.PHONG_TOA_TU_NGAY = Convert.ToDateTime(raddtPTTuNgay.Value).ToString("yyyyMMdd");
                    objGiaiToa.PHONG_TOA_DEN_NGAY = Convert.ToDateTime(raddtPTDenNgay.Value).ToString("yyyyMMdd");
                    objGiaiToa.PHUONG_THUC_PHONG_TOA = sPhuongThucPT;
                    objGiaiToa.DIEN_GIAI = txtDienGiai.Text;

                    //Thông tin kiểm soát
                    objGiaiToa.TRANG_THAI_NGHIEP_VU = sTrangThaiNVu;
                    objGiaiToa.TRANG_THAI_BAN_GHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    objGiaiToa.NGAY_LAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                    objGiaiToa.NGUOI_LAP = txtNguoiLap.Text;
                    if (action != DatabaseConstant.Action.THEM)
                    {
                        objGiaiToa.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                        objGiaiToa.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                    }
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

        }

        private bool Validation()
        {
            try
            {
                if (txtSoTGui.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaChon(lblSoTaiKhoan.Content.ToString());
                    txtSoTGui.Focus();
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


        public void OnHold()
        {
            try
            {
                if (!Validation()) return;

                string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));

                objPhongToa = new HDV_PHONG_TOA_SO_DU();
                objGiaiToa = new HDV_GIAI_TOA_SO_DU();
                GetFormData(ref objPhongToa,ref objGiaiToa, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(objPhongToa, objGiaiToa);
                    
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(objPhongToa, objGiaiToa);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnSave()
        {
            try
            {
                if (!Validation()) return;

                string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));

                objPhongToa = new HDV_PHONG_TOA_SO_DU();
                objGiaiToa = new HDV_GIAI_TOA_SO_DU();
                GetFormData(ref objPhongToa, ref objGiaiToa, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(objPhongToa, objGiaiToa);

                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(objPhongToa, objGiaiToa);
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
            SetEnabledAllControls(false);
            action = DatabaseConstant.Action.XEM;
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain,function);
        }

        public void BeforeViewFromList()
        {
            SetFormData();
            BeforeViewFromDetail();
        }


        public void BeforeAddNew()
        {
            //Thiết lập các thông tin mặc định
            cmbLoaiTien.Text = "";
            numSoDu.Value = null;
            numLaiSuat.Value = null;
            raddtNgayMo.Value = null;
            raddtNgayDH.Value = null;
            raddtNgayCap.Value = null;
            raddtPTTuNgay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            

            txtTrangThaiBanGhi.Text = "";
            raddtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            raddtNgayCapNhat.Value = null;
            txtNguoiCapNhat.Text = "";

            SetEnabledAllControls(true);
            action = DatabaseConstant.Action.THEM;
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain,function);
        }

        public void OnAddNew(HDV_PHONG_TOA_SO_DU objPhongToa, HDV_GIAI_TOA_SO_DU objGiaiToa)
        {
            try
            {
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                if (sLoaiGD.Equals("PHONG_TOA"))
                {
                    ret = processHDV.PhongToa(DatabaseConstant.Action.THEM, ref objPhongToa, ref listClientResponseDetail);                    
                }
                else if (sLoaiGD.Equals("GIAI_TOA"))
                {
                    ret = processHDV.GiaiToa(DatabaseConstant.Action.THEM, ref objGiaiToa, ref listClientResponseDetail);                    
                }

                AfterAddNew(ret, objPhongToa, objGiaiToa, listClientResponseDetail); 
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public void AfterAddNew(bool ret, HDV_PHONG_TOA_SO_DU objPhongToa, HDV_GIAI_TOA_SO_DU objGiaiToa, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                    if (sLoaiGD.Equals("PHONG_TOA"))
                    {                        
                        sTrangThaiNVu = objPhongToa.TRANG_THAI_NGHIEP_VU;
                        txtSoGD.Text = objPhongToa.MA_GDICH;
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(objPhongToa.TRANG_THAI_BAN_GHI);
                        raddtNgayLap.Value = LDateTime.StringToDate(objPhongToa.NGAY_LAP, "yyyyMMdd");
                        txtNguoiLap.Text = objPhongToa.NGUOI_LAP;
                        raddtNgayCapNhat.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                        txtNguoiCapNhat.Text = ClientInformation.TenDangNhap;
                    }
                    else if (sLoaiGD.Equals("GIAI_TOA"))
                    {
                        sTrangThaiNVu = objGiaiToa.TRANG_THAI_NGHIEP_VU;
                        txtSoGD.Text = objGiaiToa.MA_GDICH;
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(objGiaiToa.TRANG_THAI_BAN_GHI);
                        raddtNgayLap.Value = LDateTime.StringToDate(objGiaiToa.NGAY_LAP, "yyyyMMdd");
                        txtNguoiLap.Text = objGiaiToa.NGUOI_LAP;
                        raddtNgayCapNhat.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                        txtNguoiCapNhat.Text = ClientInformation.TenDangNhap;
                    }
                    

                    BeforeViewFromDetail();
                }
                else
                {
                    LMessage.ShowMessage("M.DungChung.ThemKhongThanhCong", LMessage.MessageBoxType.Error);
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

                
                bool ret = process.LockData(DatabaseConstant.Module.HDVO,
                    function,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.SUA,
                    listLockId);                

                if (ret)
                {
                    SetEnabledAllControls(true);
                    action = DatabaseConstant.Action.SUA;
                    CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain,function);
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
            SetEnabledAllControls(true);
            action = DatabaseConstant.Action.SUA;
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain,function);
        }

        public void OnModify(HDV_PHONG_TOA_SO_DU objPhongToa, HDV_GIAI_TOA_SO_DU objGiaiToa)
        {
            try
            {
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                if (sLoaiGD.Equals("PHONG_TOA"))
                {
                    ret = processHDV.PhongToa(DatabaseConstant.Action.SUA, ref objPhongToa, ref listClientResponseDetail);
                }
                else if (sLoaiGD.Equals("GIAI_TOA"))
                {
                    ret = processHDV.GiaiToa(DatabaseConstant.Action.SUA, ref objGiaiToa, ref listClientResponseDetail);
                }
                AfterModify(ret, objPhongToa, objGiaiToa, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public void AfterModify(bool ret, HDV_PHONG_TOA_SO_DU objPhongToa, HDV_GIAI_TOA_SO_DU objGiaiToa, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                    if (sLoaiGD.Equals("PHONG_TOA"))
                    {
                        sTrangThaiNVu = objPhongToa.TRANG_THAI_NGHIEP_VU;
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(objPhongToa.TRANG_THAI_BAN_GHI);
                        raddtNgayLap.Value = LDateTime.StringToDate(objPhongToa.NGAY_LAP, "yyyyMMdd");
                        txtNguoiLap.Text = objPhongToa.NGUOI_LAP;
                        raddtNgayCapNhat.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                        txtNguoiCapNhat.Text = ClientInformation.TenDangNhap;
                    }
                    else if (sLoaiGD.Equals("GIAI_TOA"))
                    {
                        sTrangThaiNVu = objGiaiToa.TRANG_THAI_NGHIEP_VU;
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(objGiaiToa.TRANG_THAI_BAN_GHI);
                        raddtNgayLap.Value = LDateTime.StringToDate(objGiaiToa.NGAY_LAP, "yyyyMMdd");
                        txtNguoiLap.Text = objGiaiToa.NGUOI_LAP;
                        raddtNgayCapNhat.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                        txtNguoiCapNhat.Text = ClientInformation.TenDangNhap;
                    }

                    BeforeViewFromDetail();
                }
                else
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatKoThanhCong", LMessage.MessageBoxType.Error);
                }

                // Yêu cầu Unlock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    function,
                    DatabaseConstant.Table.BL_TIEN_GUI,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        function,
                        DatabaseConstant.Table.BL_TIEN_GUI,
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_TAT_TOAN,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnDelete()
        {
            try
            {
                bool ret = false;
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                HuyDongVonProcess processHDV = new HuyDongVonProcess();

                if (sLoaiGD.Equals("PHONG_TOA"))
                {
                    ret = processHDV.PhongToa(DatabaseConstant.Action.XOA, ref objPhongToa, ref listClientResponseDetail);
                }
                else if (sLoaiGD.Equals("GIAI_TOA"))
                {
                    ret = processHDV.GiaiToa(DatabaseConstant.Action.XOA, ref objGiaiToa, ref listClientResponseDetail);
                }
                
                if (ret)
                {
                    AfterDelete(ret, listClientResponseDetail);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    function,
                    DatabaseConstant.Table.BL_TIEN_GUI,
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


        public void BeforeApprove()
        {
            try
            {
                // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        function,
                        DatabaseConstant.Table.BL_TIEN_GUI,
                        DatabaseConstant.Action.DUYET,
                        listLockId);

                    if (retLockData)
                    {
                        action = DatabaseConstant.Action.DUYET;
                        OnApprove();
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_TAT_TOAN,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.DUYET,
                    listLockId);

                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnApprove()
        {
            try
            {
                bool ret = false;
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                if (sLoaiGD.Equals("PHONG_TOA"))
                {
                    ret = processHDV.PhongToa(DatabaseConstant.Action.DUYET, ref objPhongToa, ref listClientResponseDetail);
                }
                else if (sLoaiGD.Equals("GIAI_TOA"))
                {
                    ret = processHDV.GiaiToa(DatabaseConstant.Action.DUYET, ref objGiaiToa, ref listClientResponseDetail);
                }

                if (ret)
                {
                    AfterApprove(ret, listClientResponseDetail);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public void AfterApprove(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    //LMessage.ShowMessage("M.DungChung.DuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    function,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.DUYET,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        public void BeforeCancel()
        {
            try
            {
                // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        function,
                        DatabaseConstant.Table.BL_TIEN_GUI,
                        DatabaseConstant.Action.THOAI_DUYET,
                        listLockId);

                    if (retLockData)
                    {
                        action = DatabaseConstant.Action.THOAI_DUYET;
                        OnCancel();
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_TAT_TOAN,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listLockId);

                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnCancel()
        {
            try
            {
                bool ret = false;
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                if (sLoaiGD.Equals("PHONG_TOA"))
                {
                    ret = processHDV.PhongToa(DatabaseConstant.Action.THOAI_DUYET, ref objPhongToa, ref listClientResponseDetail);
                }
                else if (sLoaiGD.Equals("GIAI_TOA"))
                {
                    ret = processHDV.GiaiToa(DatabaseConstant.Action.THOAI_DUYET, ref objGiaiToa, ref listClientResponseDetail);
                }

                if (ret)
                {
                    AfterCancel(ret, listClientResponseDetail);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public void AfterCancel(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    //LMessage.ShowMessage("M.DungChung.ThoaiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    function,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listLockId);

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        public void BeforeRefuse()
        {
            try
            {
                // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        function,
                        DatabaseConstant.Table.BL_TIEN_GUI,
                        DatabaseConstant.Action.TU_CHOI_DUYET,
                        listLockId);

                    if (retLockData)
                    {
                        action = DatabaseConstant.Action.TU_CHOI_DUYET;
                        OnRefuse();
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_TAT_TOAN,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);

                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnRefuse()
        {
            try
            {
                bool ret = false;
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                if (sLoaiGD.Equals("PHONG_TOA"))
                {
                    ret = processHDV.PhongToa(DatabaseConstant.Action.TU_CHOI_DUYET, ref objPhongToa, ref listClientResponseDetail);
                }
                else if (sLoaiGD.Equals("GIAI_TOA"))
                {
                    ret = processHDV.GiaiToa(DatabaseConstant.Action.TU_CHOI_DUYET, ref objGiaiToa, ref listClientResponseDetail);
                }

                if (ret)
                {
                    AfterRefuse(ret, listClientResponseDetail);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public void AfterRefuse(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    //LMessage.ShowMessage("M.DungChung.TuChoiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    function,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);
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
