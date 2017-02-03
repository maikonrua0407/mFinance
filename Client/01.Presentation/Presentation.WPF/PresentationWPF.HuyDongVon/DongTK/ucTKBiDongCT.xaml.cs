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

namespace PresentationWPF.HuyDongVon.DongTK
{
    /// <summary>
    /// Interaction logic for ucTKBiDongCT.xaml
    /// </summary>
    public partial class ucTKBiDongCT : UserControl
    {

        #region Chua khai bao Constanst, Language
        /*
         * DatabaseConstant.DanhMuc.LOAI_GD_DONG_MO_TK: DONG_TAI_KHOAN, MO_TAI_KHOAN       
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

        private KIEM_SOAT _objKiemSoat;

        private HDV_DONG_TAI_KHOAN objDongTaiKhoan;
        public HDV_DONG_TAI_KHOAN ObjDongTaiKhoan
        {
            get { return objDongTaiKhoan; }
            set { objDongTaiKhoan = value; }
        }

        private HDV_MO_LAI_TAI_KHOAN objMoTaiKhoan;
        public HDV_MO_LAI_TAI_KHOAN ObjMoTaiKhoan
        {
            get { return objMoTaiKhoan; }
            set { objMoTaiKhoan = value; }
        }

        private string sTrangThaiNVu = "";

        private string sLoaiGD = "";        

        private DatabaseConstant.Function function = DatabaseConstant.Function.HDV_PHONG_TOA_SD;

        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceGiaoDich = new List<AutoCompleteEntry>();        
        List<AutoCompleteEntry> lstSourceGD_HinhThuc = new List<AutoCompleteEntry>();

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
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
        public ucTKBiDongCT()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            LoadCombobox();

            txtSoTGui.Focus();
        }

        public ucTKBiDongCT(KIEM_SOAT obj)
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
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.HuyDongVon;component/DongTK/ucTKBiDongCT.xaml", ref Toolbar, ref mnuMain);
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
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.HINH_THUC_GIAO_DICH));
            auto.GenAutoComboBox(ref lstSourceGD_HinhThuc, ref cmbGD_HinhThuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
            cmbGD_HinhThuc.SelectedIndex = 0;

            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.LOAI_GD_DONG_MO_TK));
            auto.GenAutoComboBox(ref lstSourceGiaoDich, ref cmbGiaoDich, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, "DONG_TAI_KHOAN");
            sLoaiGD = "DONG_TAI_KHOAN";
            function = DatabaseConstant.Function.HDV_DONG_TK;

            
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

            cmbGiaoDich.IsEnabled = enable;
            txtSoTGui.IsEnabled = enable;
            btnSoTGui.IsEnabled = enable;            

            if (sLoaiGD.Equals("DONG_TAI_KHOAN"))
            {
                //Thông tin đóng tài khoản
                raddtNgayDongTK.IsEnabled = enable;
                dtpNgayDongTK.IsEnabled = enable;
                txtLyDo.IsEnabled = enable;

                //Thông tin giao dịch
                cmbGD_HinhThuc.IsEnabled = enable;               
                txtDienGiai.IsEnabled = enable;
                if (enable == true)
                {
                    string sHinhThucGD = lstSourceGD_HinhThuc.ElementAt(cmbGD_HinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (sHinhThucGD.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri()))
                    {
                        numGD_SoTienTM.IsEnabled = false;
                        numGD_SoTienCK.IsEnabled = false;
                        txtGD_TaiKhoanKH.IsEnabled = false;
                        btnGD_TaiKhoanKH.IsEnabled = false;
                        txtGD_TaiKhoanNB.IsEnabled = false;
                        btnGD_TaiKhoanNB.IsEnabled = false;
                    }
                    else if (sHinhThucGD.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN.layGiaTri()))
                    {
                        numGD_SoTienTM.IsEnabled = false;
                        numGD_SoTienCK.IsEnabled = false;
                        txtGD_TaiKhoanKH.IsEnabled = true;
                        btnGD_TaiKhoanKH.IsEnabled = true;
                        txtGD_TaiKhoanNB.IsEnabled = true;
                        btnGD_TaiKhoanNB.IsEnabled = true;
                    }
                    else if (sHinhThucGD.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TMAT_CKHOAN.layGiaTri()))
                    {
                        numGD_SoTienTM.IsEnabled = false;
                        numGD_SoTienCK.IsEnabled = true;
                        txtGD_TaiKhoanKH.IsEnabled = true;
                        btnGD_TaiKhoanKH.IsEnabled = true;
                        txtGD_TaiKhoanNB.IsEnabled = true;
                        btnGD_TaiKhoanNB.IsEnabled = true;
                    }
                }
                else
                {
                    numGD_SoTienTM.IsEnabled = enable;
                    numGD_SoTienCK.IsEnabled = enable;
                    txtGD_TaiKhoanKH.IsEnabled = enable;
                    txtGD_TaiKhoanNB.IsEnabled = enable;
                    btnGD_TaiKhoanKH.IsEnabled = enable;
                    btnGD_TaiKhoanNB.IsEnabled = enable;
                }
            }
            else if (sLoaiGD.Equals("MO_TAI_KHOAN"))
            {                
                //Thông tin đóng tài khoản
                raddtNgayDongTK.IsEnabled = false;
                dtpNgayDongTK.IsEnabled = false;
                txtLyDo.IsEnabled = false;

                cmbGD_HinhThuc.IsEnabled = false;              
                numGD_SoTienTM.IsEnabled = false;
                numGD_SoTienCK.IsEnabled = false;
                txtGD_TaiKhoanKH.IsEnabled = false;
                btnGD_TaiKhoanKH.IsEnabled = false;
                txtGD_TaiKhoanNB.IsEnabled = false;
                btnGD_TaiKhoanNB.IsEnabled = false;
                txtDienGiai.IsEnabled = false;
            }
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
                        cmbGiaoDich.IsEnabled = false;
                        numGD_TongTien.Value = numSoDu.Value + numTienLai.Value;
                        numGD_TongTien_LostFocus(null, null);
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
                    cmbLoaiTien.Text = "";
                    numLaiSuat.Value = null;
                    raddtNgayMo.Value = null;                    

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
                        cmbGiaoDich.IsEnabled = false;
                        numGD_TongTien.Value = numSoDu.Value + numTienLai.Value;
                        numGD_TongTien_LostFocus(null, null);
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

        private void cmbGiaoDich_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                sLoaiGD = lstSourceGiaoDich.ElementAt(cmbGiaoDich.SelectedIndex).KeywordStrings.ElementAt(0);
                if (sLoaiGD.Equals("DONG_TAI_KHOAN"))
                {
                    function = DatabaseConstant.Function.HDV_PHONG_TOA_SD;
                    raddtNgayDongTK.IsEnabled = true;
                    dtpNgayDongTK.IsEnabled = true;
                    txtLyDo.IsEnabled = true;

                    cmbGD_HinhThuc.IsEnabled = true;
                    txtDienGiai.IsEnabled = true;

                    //if (lstSourceGD_HinhThuc == null) return;

                    string sHinhThucGD = lstSourceGD_HinhThuc.ElementAt(cmbGD_HinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (sHinhThucGD.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri()))
                    {
                        numGD_SoTienTM.IsEnabled = false;
                        numGD_SoTienCK.IsEnabled = false;
                        txtGD_TaiKhoanKH.IsEnabled = false;
                        btnGD_TaiKhoanKH.IsEnabled = false;
                        txtGD_TaiKhoanNB.IsEnabled = false;
                        btnGD_TaiKhoanNB.IsEnabled = false;

                    }
                    else if (sHinhThucGD.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN.layGiaTri()))
                    {
                        numGD_SoTienTM.IsEnabled = false;
                        numGD_SoTienCK.IsEnabled = false;
                        txtGD_TaiKhoanKH.IsEnabled = true;
                        btnGD_TaiKhoanKH.IsEnabled = true;
                        txtGD_TaiKhoanNB.IsEnabled = true;
                        btnGD_TaiKhoanNB.IsEnabled = true;

                    }
                    else if (sHinhThucGD.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TMAT_CKHOAN.layGiaTri()))
                    {
                        numGD_SoTienTM.IsEnabled = false;
                        numGD_SoTienCK.IsEnabled = true;
                        txtGD_TaiKhoanKH.IsEnabled = true;
                        btnGD_TaiKhoanKH.IsEnabled = true;
                        txtGD_TaiKhoanNB.IsEnabled = true;
                        btnGD_TaiKhoanNB.IsEnabled = true;
                    }
                }
                else if (sLoaiGD.Equals("MO_TAI_KHOAN"))
                {
                    function = DatabaseConstant.Function.HDV_GIAI_TOA_SD;
                    raddtNgayDongTK.IsEnabled = false;
                    dtpNgayDongTK.IsEnabled = false;
                    txtLyDo.IsEnabled = false;

                    cmbGD_HinhThuc.IsEnabled = false;
                    numGD_SoTienTM.IsEnabled = false;
                    numGD_SoTienCK.IsEnabled = false;
                    txtGD_TaiKhoanKH.IsEnabled = false;
                    btnGD_TaiKhoanKH.IsEnabled = false;
                    txtGD_TaiKhoanNB.IsEnabled = false;
                    btnGD_TaiKhoanNB.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        #region Giao dịch
        private void cmbGD_HinhThuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string sHinhThucGD = lstSourceGD_HinhThuc.ElementAt(cmbGD_HinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                if (sHinhThucGD.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri()))
                {
                    numGD_SoTienTM.IsEnabled = false;
                    numGD_SoTienCK.IsEnabled = false;
                    txtGD_TaiKhoanKH.IsEnabled = false;
                    btnGD_TaiKhoanKH.IsEnabled = false;
                    txtGD_TaiKhoanNB.IsEnabled = false;
                    btnGD_TaiKhoanNB.IsEnabled = false;

                    numGD_SoTienTM.Value = numGD_TongTien.Value;
                    numGD_SoTienCK.Value = 0;
                }
                else if (sHinhThucGD.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN.layGiaTri()))
                {
                    numGD_SoTienTM.IsEnabled = false;
                    numGD_SoTienCK.IsEnabled = false;
                    txtGD_TaiKhoanKH.IsEnabled = true;
                    btnGD_TaiKhoanKH.IsEnabled = true;
                    txtGD_TaiKhoanNB.IsEnabled = true;
                    btnGD_TaiKhoanNB.IsEnabled = true;

                    numGD_SoTienTM.Value = 0;
                    numGD_SoTienCK.Value = numGD_TongTien.Value;
                }
                else if (sHinhThucGD.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TMAT_CKHOAN.layGiaTri()))
                {
                    numGD_SoTienTM.IsEnabled = false;
                    numGD_SoTienCK.IsEnabled = true;
                    txtGD_TaiKhoanKH.IsEnabled = true;
                    btnGD_TaiKhoanKH.IsEnabled = true;
                    txtGD_TaiKhoanNB.IsEnabled = true;
                    btnGD_TaiKhoanNB.IsEnabled = true;

                    numGD_SoTienCK.Value = 0;
                    numGD_SoTienTM.Value = numGD_TongTien.Value - numGD_SoTienCK.Value;
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void numGD_TongTien_LostFocus(object sender, RoutedEventArgs e)
        {
            string sHinhThucGD = lstSourceGD_HinhThuc.ElementAt(cmbGD_HinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
            if (sHinhThucGD.Equals(BusinessConstant.layGiaTri(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN)))
            {
                numGD_SoTienCK.Value = numGD_TongTien.Value;
                numGD_SoTienTM.Value = 0;
            }
            else
            {
                numGD_SoTienTM.Value = numGD_TongTien.Value;
                numGD_SoTienCK.Value = 0;
            }
        }

        private void numGD_SoTienTM_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                string sHinhThucGD = lstSourceGD_HinhThuc.ElementAt(cmbGD_HinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                if (sHinhThucGD.Equals(BusinessConstant.layGiaTri(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN)))
                {
                    numGD_SoTienCK.Value = numGD_SoTienTM.Value;
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void numGD_SoTienCK_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                string sHinhThucGD = lstSourceGD_HinhThuc.ElementAt(cmbGD_HinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                if (sHinhThucGD.Equals(BusinessConstant.layGiaTri(BusinessConstant.HINH_THUC_GIAO_DICH.TMAT_CKHOAN)))
                {
                    if (numGD_SoTienCK.Value > numGD_TongTien.Value)
                    {
                        LMessage.ShowMessage("Số tiền chuyển khoản (" + numGD_SoTienCK.Text + ") > số tiền giao dịch (" + numGD_TongTien.Text + ")", LMessage.MessageBoxType.Warning);
                        numGD_SoTienTM.Value = numGD_TongTien.Value;
                        numGD_SoTienCK.Value = 0;
                        numGD_SoTienCK.Focus();

                    }
                    else
                    {
                        numGD_SoTienTM.Value = numGD_TongTien.Value - numGD_SoTienCK.Value;
                    }

                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void txtGD_TaiKhoanKH_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnGD_TaiKhoanKH_Click(null, null);
            }
        }

        private void txtGD_TaiKhoanKH_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void btnGD_TaiKhoanKH_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtSoTGui.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaChon(lblSoTGui.Content.ToString());
                    txtSoTGui.Focus();
                    return;
                }

                lstPopup = null;
                //list điều kiện theo từng loại sổ
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(lblIDKhachHang.Content.ToString());
                lstDieuKien.Add("KHACH_HANG");

                var process = new PopupProcess();
                process.getPopupInformation("POPUP_DS_TAI_KHOAN", lstDieuKien);

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null)
                {
                    DataRow row = lstPopup[0];
                    txtGD_TaiKhoanKH.Text = row[2].ToString();
                    lblGD_TaiKhoanKH.Content = row[3].ToString();
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void txtGD_TaiKhoanNB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnGD_TaiKhoanNB_Click(null, null);
            }
        }

        private void txtGD_TaiKhoanNB_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void btnGD_TaiKhoanNB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtSoTGui.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaChon(lblSoTGui.Content.ToString());
                    txtSoTGui.Focus();
                    return;
                }

                lstPopup = null;

                //list điều kiện theo từng loại sổ
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(lblIDKhachHang.Content.ToString());
                lstDieuKien.Add("NGUOI_SU_DUNG");

                var process = new PopupProcess();
                process.getPopupInformation("POPUP_DS_TAI_KHOAN", lstDieuKien);

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null)
                {
                    DataRow row = lstPopup[0];
                    txtGD_TaiKhoanNB.Text = row[2].ToString();
                    lblGD_TaiKhoanNB.Content = row[3].ToString();
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
        #endregion

        #endregion

        #region Xử lý nghiệp vụ
        private void GetFormData(ref HDV_DONG_TAI_KHOAN objDongTaiKhoan, ref HDV_MO_LAI_TAI_KHOAN objMoTaiKhoan, string sTrangThaiNVu)
        {
            try
            {
                if (sLoaiGD.Equals("DONG_TAI_KHOAN"))
                {
                    //Thông tin tài khoản
                    objDongTaiKhoan.MA_GDICH = txtSoGD.Text;
                    objDongTaiKhoan.NGAY_GDICH = ClientInformation.NgayLamViecHienTai;
                    objDongTaiKhoan.SO_TAI_KHOAN = txtSoTGui.Text;
                    objDongTaiKhoan.SO_DU = Convert.ToDecimal(numSoDu.Value);
                    objDongTaiKhoan.LOAI_TIEN = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.ElementAt(0);
                    objDongTaiKhoan.LAI_SUAT = Convert.ToDecimal(numLaiSuat.Value);
                    objDongTaiKhoan.NGAY_MO_SO = Convert.ToDateTime(raddtNgayMo.Value).ToString("yyyyMMdd");
                    objDongTaiKhoan.TIEN_LAI = Convert.ToDecimal(numTienLai.Value);

                    //Thông tin khách hàng
                    objDongTaiKhoan.MA_KHACH_HANG = txtMaKH.Text;
                    objDongTaiKhoan.TEN_KHACH_HANG = txtTenKH.Text;
                    objDongTaiKhoan.DIA_CHI = txtDiaChi.Text;
                    objDongTaiKhoan.SO_CMND = txtSoCMT.Text;
                    objDongTaiKhoan.NGAY_CAP = Convert.ToDateTime(raddtNgayCap.Value).ToString("yyyyMMdd");
                    objDongTaiKhoan.NOI_CAP = txtNoiCap.Text;
                    objDongTaiKhoan.SO_DIEN_THOAI = txtSDT.Text;

                    //Thông tin đóng tài khoản
                    objDongTaiKhoan.NGAY_DONG_TAI_KHOAN = Convert.ToDateTime(raddtNgayDongTK.Value).ToString("yyyyMMdd");
                    objDongTaiKhoan.LY_DO = txtLyDo.Text;

                    //Thông tin giao dịch
                    objDongTaiKhoan.HINH_THUC_GIAO_DICH = lstSourceGD_HinhThuc.ElementAt(cmbGD_HinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                    objDongTaiKhoan.TONG_TIEN_GOC_LAI = Convert.ToDecimal(numGD_TongTien.Value);
                    objDongTaiKhoan.SO_TIEN_MAT = Convert.ToDecimal(numGD_SoTienTM.Value);
                    objDongTaiKhoan.SO_TIEN_CHUYEN_KHOAN = Convert.ToDecimal(numGD_SoTienCK.Value);
                    objDongTaiKhoan.TAI_KHOAN_KHACH_HANG = txtGD_TaiKhoanKH.Text;
                    objDongTaiKhoan.TAI_KHOAN_NOI_BO = txtGD_TaiKhoanNB.Text;
                    objDongTaiKhoan.DIEN_GIAI = txtDienGiai.Text;

                    //Thông tin kiểm soát
                    objDongTaiKhoan.TRANG_THAI_NGHIEP_VU = sTrangThaiNVu;
                    objDongTaiKhoan.TRANG_THAI_BAN_GHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    objDongTaiKhoan.NGAY_LAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                    objDongTaiKhoan.NGUOI_LAP = txtNguoiLap.Text;
                    if (action != DatabaseConstant.Action.THEM)
                    {
                        objDongTaiKhoan.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                        objDongTaiKhoan.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                    }
                }
                else if (sLoaiGD.Equals("MO_TAI_KHOAN"))
                {
                    //Thông tin tài khoản
                    objMoTaiKhoan.MA_GDICH = txtSoGD.Text;
                    objMoTaiKhoan.NGAY_GDICH = ClientInformation.NgayLamViecHienTai;
                    objMoTaiKhoan.SO_TAI_KHOAN = txtSoTGui.Text;
                    objMoTaiKhoan.SO_DU = Convert.ToDecimal(numSoDu.Value);
                    objMoTaiKhoan.LOAI_TIEN = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.ElementAt(0);
                    objMoTaiKhoan.LAI_SUAT = Convert.ToDecimal(numLaiSuat.Value);
                    objMoTaiKhoan.NGAY_MO_SO = Convert.ToDateTime(raddtNgayMo.Value).ToString("yyyyMMdd");
                    objMoTaiKhoan.TIEN_LAI = Convert.ToDecimal(numTienLai.Value);

                    //Thông tin khách hàng
                    objMoTaiKhoan.MA_KHACH_HANG = txtMaKH.Text;
                    objMoTaiKhoan.TEN_KHACH_HANG = txtTenKH.Text;
                    objMoTaiKhoan.DIA_CHI = txtDiaChi.Text;
                    objMoTaiKhoan.SO_CMND = txtSoCMT.Text;
                    objMoTaiKhoan.NGAY_CAP = Convert.ToDateTime(raddtNgayCap.Value).ToString("yyyyMMdd");
                    objMoTaiKhoan.NOI_CAP = txtNoiCap.Text;
                    objMoTaiKhoan.SO_DIEN_THOAI = txtSDT.Text;

                    //Thông tin đóng tài khoản
                    objMoTaiKhoan.NGAY_MO_LAI_TAI_KHOAN = Convert.ToDateTime(raddtNgayDongTK.Value).ToString("yyyyMMdd");                    

                    //Thông tin giao dịch
                    //objMoTaiKhoan.HINH_THUC_GIAO_DICH = lstSourceGD_HinhThuc.ElementAt(cmbGD_HinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                    //objMoTaiKhoan.TONG_TIEN_GOC_LAI = Convert.ToDecimal(numGD_TongTien.Value);
                    //objMoTaiKhoan.SO_TIEN_MAT = Convert.ToDecimal(numGD_SoTienTM.Value);
                    //objMoTaiKhoan.SO_TIEN_CHUYEN_KHOAN = Convert.ToDecimal(numGD_SoTienCK.Value);
                    //objMoTaiKhoan.TAI_KHOAN_KHACH_HANG = txtGD_TaiKhoanKH.Text;
                    //objMoTaiKhoan.TAI_KHOAN_NOI_BO = txtGD_TaiKhoanNB.Text;
                    objMoTaiKhoan.DIEN_GIAI = txtDienGiai.Text;

                    //Thông tin kiểm soát
                    objMoTaiKhoan.TRANG_THAI_NGHIEP_VU = sTrangThaiNVu;
                    objMoTaiKhoan.TRANG_THAI_BAN_GHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    objMoTaiKhoan.NGAY_LAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                    objMoTaiKhoan.NGUOI_LAP = txtNguoiLap.Text;
                    if (action != DatabaseConstant.Action.THEM)
                    {
                        objMoTaiKhoan.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                        objMoTaiKhoan.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
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
                    CommonFunction.ThongBaoChuaChon(lblSoTGui.Content.ToString());
                    txtSoTGui.Focus();
                    return false;
                }
                else if (raddtNgayDongTK.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaNhap(lblNgayDongTK.Content.ToString());
                    raddtNgayDongTK.Focus();
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

                objDongTaiKhoan = new HDV_DONG_TAI_KHOAN();
                objMoTaiKhoan = new HDV_MO_LAI_TAI_KHOAN();
                GetFormData(ref objDongTaiKhoan, ref objMoTaiKhoan, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(objDongTaiKhoan, objMoTaiKhoan);

                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(objDongTaiKhoan, objMoTaiKhoan);
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

                objDongTaiKhoan = new HDV_DONG_TAI_KHOAN();
                objMoTaiKhoan = new HDV_MO_LAI_TAI_KHOAN();
                GetFormData(ref objDongTaiKhoan, ref objMoTaiKhoan, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(objDongTaiKhoan, objMoTaiKhoan);

                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(objDongTaiKhoan, objMoTaiKhoan);
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
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
            tlbPreview.IsEnabled = true;
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
            raddtNgayCap.Value = null;

            txtTrangThaiBanGhi.Text = "";
            raddtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            raddtNgayCapNhat.Value = null;
            txtNguoiCapNhat.Text = "";

            SetEnabledAllControls(true);
            action = DatabaseConstant.Action.THEM;
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnAddNew(HDV_DONG_TAI_KHOAN objDongTaiKhoan, HDV_MO_LAI_TAI_KHOAN objMoTaiKhoan)
        {
            try
            {
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                if (sLoaiGD.Equals("DONG_TAI_KHOAN"))
                {
                    ret = processHDV.DongTaiKhoan(DatabaseConstant.Action.THEM, ref objDongTaiKhoan, ref listClientResponseDetail);
                }
                else if (sLoaiGD.Equals("MO_TAI_KHOAN"))
                {
                    ret = processHDV.MoTaiKhoan(DatabaseConstant.Action.THEM, ref objMoTaiKhoan, ref listClientResponseDetail);
                }

                AfterAddNew(ret, objDongTaiKhoan, objMoTaiKhoan, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public void AfterAddNew(bool ret, HDV_DONG_TAI_KHOAN objDongTaiKhoan, HDV_MO_LAI_TAI_KHOAN objMoTaiKhoan, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                    if (sLoaiGD.Equals("DONG_TAI_KHOAN"))
                    {
                        sTrangThaiNVu = objDongTaiKhoan.TRANG_THAI_NGHIEP_VU;
                        txtSoGD.Text = objDongTaiKhoan.MA_GDICH;
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(objDongTaiKhoan.TRANG_THAI_BAN_GHI);
                        raddtNgayLap.Value = LDateTime.StringToDate(objDongTaiKhoan.NGAY_LAP, "yyyyMMdd");
                        txtNguoiLap.Text = objDongTaiKhoan.NGUOI_LAP;
                        raddtNgayCapNhat.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                        txtNguoiCapNhat.Text = ClientInformation.TenDangNhap;
                    }
                    else if (sLoaiGD.Equals("MO_TAI_KHOAN"))
                    {
                        sTrangThaiNVu = objMoTaiKhoan.TRANG_THAI_NGHIEP_VU;
                        txtSoGD.Text = objMoTaiKhoan.MA_GDICH;
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(objMoTaiKhoan.TRANG_THAI_BAN_GHI);
                        raddtNgayLap.Value = LDateTime.StringToDate(objMoTaiKhoan.NGAY_LAP, "yyyyMMdd");
                        txtNguoiLap.Text = objMoTaiKhoan.NGUOI_LAP;
                        raddtNgayCapNhat.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                        txtNguoiCapNhat.Text = ClientInformation.TenDangNhap;
                    }


                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    //LMessage.ShowMessage("M.DungChung.ThemKhongThanhCong", LMessage.MessageBoxType.Error);
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
            SetEnabledAllControls(true);
            action = DatabaseConstant.Action.SUA;
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnModify(HDV_DONG_TAI_KHOAN objDongTaiKhoan, HDV_MO_LAI_TAI_KHOAN objMoTaiKhoan)
        {
            try
            {
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                if (sLoaiGD.Equals("DONG_TAI_KHOAN"))
                {
                    ret = processHDV.DongTaiKhoan(DatabaseConstant.Action.SUA, ref objDongTaiKhoan, ref listClientResponseDetail);
                }
                else if (sLoaiGD.Equals("MO_TAI_KHOAN"))
                {
                    ret = processHDV.MoTaiKhoan(DatabaseConstant.Action.SUA, ref objMoTaiKhoan, ref listClientResponseDetail);
                }
                AfterModify(ret, objDongTaiKhoan, objMoTaiKhoan, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public void AfterModify(bool ret, HDV_DONG_TAI_KHOAN objDongTaiKhoan, HDV_MO_LAI_TAI_KHOAN objMoTaiKhoan, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                    if (sLoaiGD.Equals("DONG_TAI_KHOAN"))
                    {
                        sTrangThaiNVu = objDongTaiKhoan.TRANG_THAI_NGHIEP_VU;
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(objDongTaiKhoan.TRANG_THAI_BAN_GHI);
                        raddtNgayLap.Value = LDateTime.StringToDate(objDongTaiKhoan.NGAY_LAP, "yyyyMMdd");
                        txtNguoiLap.Text = objDongTaiKhoan.NGUOI_LAP;
                        raddtNgayCapNhat.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                        txtNguoiCapNhat.Text = ClientInformation.TenDangNhap;
                    }
                    else if (sLoaiGD.Equals("MO_TAI_KHOAN"))
                    {
                        sTrangThaiNVu = objMoTaiKhoan.TRANG_THAI_NGHIEP_VU;
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(objMoTaiKhoan.TRANG_THAI_BAN_GHI);
                        raddtNgayLap.Value = LDateTime.StringToDate(objMoTaiKhoan.NGAY_LAP, "yyyyMMdd");
                        txtNguoiLap.Text = objMoTaiKhoan.NGUOI_LAP;
                        raddtNgayCapNhat.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                        txtNguoiCapNhat.Text = ClientInformation.TenDangNhap;
                    }

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
                    function,
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

                if (sLoaiGD.Equals("DONG_TAI_KHOAN"))
                {
                    ret = processHDV.DongTaiKhoan(DatabaseConstant.Action.XOA, ref objDongTaiKhoan, ref listClientResponseDetail);
                }
                else if (sLoaiGD.Equals("MO_TAI_KHOAN"))
                {
                    ret = processHDV.MoTaiKhoan(DatabaseConstant.Action.XOA, ref objMoTaiKhoan, ref listClientResponseDetail);
                }

                AfterDelete(ret, listClientResponseDetail);
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
                    function,
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
                if (sLoaiGD.Equals("DONG_TAI_KHOAN"))
                {
                    ret = processHDV.DongTaiKhoan(DatabaseConstant.Action.DUYET, ref objDongTaiKhoan, ref listClientResponseDetail);
                }
                else if (sLoaiGD.Equals("MO_TAI_KHOAN"))
                {
                    ret = processHDV.MoTaiKhoan(DatabaseConstant.Action.DUYET, ref objMoTaiKhoan, ref listClientResponseDetail);
                }

                AfterApprove(ret, listClientResponseDetail);
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
                    function,
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
                if (sLoaiGD.Equals("DONG_TAI_KHOAN"))
                {
                    ret = processHDV.DongTaiKhoan(DatabaseConstant.Action.THOAI_DUYET, ref objDongTaiKhoan, ref listClientResponseDetail);
                }
                else if (sLoaiGD.Equals("MO_TAI_KHOAN"))
                {
                    ret = processHDV.MoTaiKhoan(DatabaseConstant.Action.THOAI_DUYET, ref objMoTaiKhoan, ref listClientResponseDetail);
                }

                AfterCancel(ret, listClientResponseDetail);
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
                    function,
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
                if (sLoaiGD.Equals("DONG_TAI_KHOAN"))
                {
                    ret = processHDV.DongTaiKhoan(DatabaseConstant.Action.TU_CHOI_DUYET, ref objDongTaiKhoan, ref listClientResponseDetail);
                }
                else if (sLoaiGD.Equals("MO_TAI_KHOAN"))
                {
                    ret = processHDV.MoTaiKhoan(DatabaseConstant.Action.TU_CHOI_DUYET, ref objMoTaiKhoan, ref listClientResponseDetail);
                }

                AfterRefuse(ret, listClientResponseDetail);
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
                    LMessage.ShowMessage("M.DungChung.TuChoiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
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
