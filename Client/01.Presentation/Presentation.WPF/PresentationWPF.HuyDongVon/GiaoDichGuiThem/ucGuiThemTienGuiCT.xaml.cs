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
using PresentationWPF.HuyDongVon.Popup;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;


namespace PresentationWPF.HuyDongVon.GiaoDichGuiThem
{
    /// <summary>
    /// Interaction logic for ucGuiThemTienGui.xaml
    /// </summary>
    public partial class ucGuiThemTienGuiCT : UserControl
    {
        #region Khai bao

        private KIEM_SOAT _objKiemSoat = null;
        public KIEM_SOAT objKiemSoat
        {
            get { return _objKiemSoat; }
            set { _objKiemSoat = value; }
        }

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

        private string maGiaoDich;
        public string MaGiaoDich
        {
            get { return maGiaoDich; }
            set { maGiaoDich = value; }
        }

        private HDV_GUI_TIEN_THEO_SO obj;
        public HDV_GUI_TIEN_THEO_SO Obj
        {
            get { return obj; }
            set { obj = value; }
        }

        private string sTrangThaiNVu = "";

        private string maSanPham = "";

        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceGD_HinhThuc = new List<AutoCompleteEntry>();

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
        public ucGuiThemTienGuiCT()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            LoadCombobox();

            HideControl();

            txtSoTGui.Focus();
            
        }

        public ucGuiThemTienGuiCT(KIEM_SOAT objKS)
        {
            InitializeComponent();

            DuyetQuyenTinhNang();            

            BindShortkey();

            LoadCombobox();

            _objKiemSoat = objKS;

            action = _objKiemSoat.action;

            sTrangThaiNVu = _objKiemSoat.TTHAI_NVU;            

            this.obj = new HDV_GUI_TIEN_THEO_SO();

            HideControl();

            tlbHelp.Focus();
        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.HuyDongVon;component/GiaoDichGuiThem/ucGuiThemTienGuiCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        /// <summary>
        /// Load combobox
        /// </summary>
        private void LoadCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();

            auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TIENTE.getValue());            
       
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.HINH_THUC_GIAO_DICH));
            auto.GenAutoComboBox(ref lstSourceGD_HinhThuc, ref cmbGD_HinhThuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien,BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri());            
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
                OnPreview();
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

        private void HideControl()
        {
            try
            {
                HeThong hethong = new HeThong();
                ArrayList arr = new ArrayList();
                arr = hethong.SetVisibleControl("PresentationWPF.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT", "");
                foreach (List<string> lst in arr)
                {
                    object item = grMain.FindName(lst.First());
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
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }

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
                DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
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

            #region Giao dịch
            cmbGD_HinhThuc.IsEnabled = enable;
            numGD_TongTien.IsEnabled = enable;
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
            #endregion

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

                        lblIDKhachHang.Content = "";
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

                    lblIDKhachHang.Content = "";
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
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                lstPopup.Clear();
                Window window = new Window();
                ucPopupSoTGui uc = new ucPopupSoTGui();
                uc.Function = DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO;
                uc.DuLieuTraVe = new ucPopupSoTGui.LayDuLieu(LayDuLieuTuPopup);
                window.Title = LLanguage.SearchResourceByKey("U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.DanhSachSo");
                window.Content = uc;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
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
                        maSanPham = dr["MA_SAN_PHAM"].ToString();
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
                    }

                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        #region Giao dịch

        private void cmbGD_HinhThuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbGD_HinhThuc.SelectedIndex < 0) return;

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
            if (cmbGD_HinhThuc.SelectedIndex >= 0)
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
        }

        private void numGD_SoTienTM_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbGD_HinhThuc.SelectedIndex >= 0)
                {
                    string sHinhThucGD = lstSourceGD_HinhThuc.ElementAt(cmbGD_HinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (sHinhThucGD.Equals(BusinessConstant.layGiaTri(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN)))
                    {
                        numGD_SoTienCK.Value = numGD_SoTienTM.Value;
                    }
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
                if (cmbGD_HinhThuc.SelectedIndex >= 0)
                {
                    string sHinhThucGD = lstSourceGD_HinhThuc.ElementAt(cmbGD_HinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (sHinhThucGD.Equals(BusinessConstant.layGiaTri(BusinessConstant.HINH_THUC_GIAO_DICH.TMAT_CKHOAN)))
                    {
                        if (numGD_SoTienCK.Value > numGD_TongTien.Value)
                        {
                            string soTienCK = LLanguage.SearchResourceByKey("U.DungChung.SoTienCK");
                            string soTienTong = LLanguage.SearchResourceByKey("U.DungChung.SoTienTong");
                            LMessage.ShowMessage(soTienCK + " (" + numGD_SoTienCK.Text + ") > " + soTienTong + " (" + numGD_TongTien.Text + ")", LMessage.MessageBoxType.Warning);
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
                //@MA_DVI#@MA_DTUONG#@MA_KHANG_NBO
                lstDieuKien.Add(ClientInformation.MaDonVi);
                lstDieuKien.Add(txtSoTGui.Text);
                lstDieuKien.Add(BusinessConstant.TinhChatLoaiKhangNBo.KHACH_HANG.layGiaTri());

                var process = new PopupProcess();
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_TKHOAN_GTHEM_TIEN.getValue(), lstDieuKien);

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
                //@MA_DVI#@MA_DTUONG#@MA_KHANG_NBO
                lstDieuKien.Add(ClientInformation.MaDonVi);
                lstDieuKien.Add("MD");
                lstDieuKien.Add(BusinessConstant.TinhChatLoaiKhangNBo.NOI_BO.layGiaTri());

                var process = new PopupProcess();
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_TKHOAN_GTHEM_TIEN.getValue(), lstDieuKien);

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
        private void GetFormData(ref HDV_GUI_TIEN_THEO_SO obj, string sTrangThaiNVu)
        {
            try
            {
                //Thông tin sổ               
                obj.MA_GDICH = txtSoGD.Text;
                obj.SO_SO_TG = txtSoTGui.Text;
                obj.SO_DU = Convert.ToDecimal(numSoDu.Value);
                obj.MA_SAN_PHAM = maSanPham;
                obj.LOAI_TIEN = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.ElementAt(0);
                obj.LAI_SUAT = Convert.ToDecimal(numLaiSuat.Value);
                obj.NGAY_MO_SO = Convert.ToDateTime(raddtNgayMo.Value).ToString("yyyyMMdd");
                obj.NGAY_DAO_HAN = Convert.ToDateTime(raddtNgayDH.Value).ToString("yyyyMMdd");

                //Thông tin khách hàng
                obj.MA_KHACH_HANG = txtMaKH.Text;
                obj.TEN_KHACH_HANG = txtTenKH.Text;
                obj.DIA_CHI = txtDiaChi.Text;
                obj.SO_CMND = txtSoCMT.Text;
                obj.NGAY_CAP = Convert.ToDateTime(raddtNgayCap.Value).ToString("yyyyMMdd");
                obj.NOI_CAP = txtNoiCap.Text;
                obj.SO_DIEN_THOAI = txtSDT.Text;

                //Thông tin giao dịch
                obj.MA_DVI = ClientInformation.MaDonViGiaoDich;
                obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                obj.NGAY_GDICH = ClientInformation.NgayLamViecHienTai;
                obj.HINH_THUC_GIAO_DICH = lstSourceGD_HinhThuc.ElementAt(cmbGD_HinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                obj.TONG_TIEN_GIAO_DICH = Convert.ToDecimal(numGD_TongTien.Value);
                obj.SO_TIEN_MAT = Convert.ToDecimal(numGD_SoTienTM.Value);
                obj.SO_TIEN_CHUYEN_KHOAN = Convert.ToDecimal(numGD_SoTienCK.Value);
                obj.TAI_KHOAN_KHACH_HANG = txtGD_TaiKhoanKH.Text;
                obj.TAI_KHOAN_NOI_BO = txtGD_TaiKhoanNB.Text;
                obj.DIEN_GIAI = txtDienGiai.Text;

                //Thông tin kiểm soát
                obj.TRANG_THAI_NGHIEP_VU = sTrangThaiNVu;
                obj.TRANG_THAI_BAN_GHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.NGAY_LAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                obj.NGUOI_LAP = txtNguoiLap.Text;
                if (action != DatabaseConstant.Action.THEM)
                {
                    obj.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
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
            try
            {
                if (_objKiemSoat != null && !_objKiemSoat.SO_GIAO_DICH.IsNullOrEmptyOrSpace())
                {

                    HuyDongVonProcess processHDV = new HuyDongVonProcess();
                    DataSet ds = processHDV.GetThongTinGuiThemTienTheoTungSo(ClientInformation.MaDonViGiaoDich, _objKiemSoat.SO_GIAO_DICH);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        MaGiaoDich = dr["MA_GDICH"].ToString();
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                        #region Thông tin sổ tiền gửi
                        txtSoGD.Text = dr["MA_GDICH"].ToString();
                        txtSoTGui.Text = dr["SO_SO_TG"].ToString();
                        numSoDu.Value = Convert.ToDouble(dr["SO_TIEN"]);
                        cmbLoaiTien.SelectedIndex = lstSourceLoaiTien.IndexOf(lstSourceLoaiTien.FirstOrDefault(i => i.KeywordStrings.First().Equals(dr["MA_LOAI_TIEN"].ToString())));
                        numLaiSuat.Value = Convert.ToDouble(dr["LAI_SUAT"]);
                        if (LDateTime.IsDate(dr["NGAY_MO_SO"].ToString(), "yyyyMMdd") == true)
                        {
                            raddtNgayMo.Value = LDateTime.StringToDate(dr["NGAY_MO_SO"].ToString(), "yyyyMMdd");
                        }
                        else
                        {
                            raddtNgayMo.Value = null;
                        }
                        if (LDateTime.IsDate(dr["NGAY_DEN_HAN"].ToString(), "yyyyMMdd") == true)
                        {
                            raddtNgayDH.Value = LDateTime.StringToDate(dr["NGAY_DEN_HAN"].ToString(), "yyyyMMdd");
                        }
                        else
                        {
                            raddtNgayDH.Value = null;
                        }
                        #endregion

                        #region Thông tin khách hàng
                        txtMaKH.Text = dr["MA_KHANG"].ToString();
                        txtTenKH.Text = dr["TEN_KHANG"].ToString();
                        txtDiaChi.Text = dr["DIA_CHI"].ToString();
                        if (dr["DD_GTLQ_LOAI"].ToString().Equals("CHUNG_MINH_ND"))
                        {
                            txtSoCMT.Text = dr["DD_GTLQ_SO"].ToString();
                            txtNoiCap.Text = dr["DD_GTLQ_NOI_CAP"].ToString();
                            if (LDateTime.IsDate(dr["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd") == true)
                                raddtNgayCap.Value = LDateTime.StringToDate(dr["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd");
                        }
                        txtSDT.Text = dr["SO_DTHOAI"].ToString();
                        #endregion

                        #region Thông tin giao dịch
                        cmbGD_HinhThuc.SelectedIndex = lstSourceGD_HinhThuc.IndexOf(lstSourceGD_HinhThuc.FirstOrDefault(i => i.KeywordStrings.First().Equals(dr["GDICH_HTHUC"].ToString())));
                        numGD_TongTien.Value = Convert.ToDouble(dr["GDICH_TIEN_MAT"]);
                        numGD_SoTienTM.Value = Convert.ToDouble(dr["GDICH_TIEN_MAT"]);
                        numGD_SoTienCK.Value = Convert.ToDouble(dr["GDICH_TIEN_CKHOAN"]);
                        txtGD_TaiKhoanKH.Text = dr["TKHOAN_KHANG"].ToString();
                        lblGD_TaiKhoanKH.Content = dr["TEN_TKHOAN_KHANG"].ToString();
                        txtGD_TaiKhoanNB.Text = dr["TKHOAN_NBO"].ToString();
                        lblGD_TaiKhoanNB.Content = dr["TEN_TKHOAN_NBO"].ToString();
                        txtDienGiai.Text = dr["DIEN_GIAI"].ToString();
                        #endregion

                        #region Tab thông tin kiểm soát
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(dr["TTHAI_BGHI"].ToString());
                        raddtNgayLap.Value = LDateTime.StringToDate(dr["NGAY_NHAP"].ToString(), "yyyyMMdd");
                        txtNguoiLap.Text = dr["NGUOI_NHAP"].ToString();
                        if (LDateTime.IsDate(dr["NGAY_CNHAT"].ToString(), "yyyyMMdd") == true)
                            raddtNgayCapNhat.Value = LDateTime.StringToDate(dr["NGAY_CNHAT"].ToString(), "yyyyMMdd");
                        else
                            raddtNgayCapNhat.Value = null;
                        txtNguoiCapNhat.Text = dr["NGUOI_CNHAT"].ToString();
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void ResetForm()
        {            
            //Thông tin sổ
            lblTrangThai.Content = "";
            txtSoGD.Text = "";
            txtSoTGui.Text = "";
            numSoDu.Value = null;
            cmbLoaiTien.SelectedIndex = -1;
            numLaiSuat.Value = null;
            raddtNgayMo.Value = null;
            raddtNgayDH.Value = null;
            raddtNgayCap.Value = null;

            //Thông tin khách hàng
            txtMaKH.Text = "";
            txtTenKH.Text = "";
            txtDiaChi.Text = "";
            txtSoCMT.Text = "";
            raddtNgayCap.Value = null;
            txtNoiCap.Text = "";
            txtSDT.Text = "";

            //Thông tin giao dịch
            cmbGD_HinhThuc.SelectedIndex = lstSourceGD_HinhThuc.IndexOf(lstSourceGD_HinhThuc.FirstOrDefault(e => e.KeywordStrings.First().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri())));
            numGD_TongTien.Value = 0;
            numGD_SoTienTM.Value = 0;
            numGD_SoTienCK.Value = 0;
            txtGD_TaiKhoanKH.Text = "";
            txtGD_TaiKhoanNB.Text = "";
            txtDienGiai.Text = "";

            //Thông tin kiểm soát
            txtTrangThaiBanGhi.Text = "";
            raddtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            raddtNgayCapNhat.Value = null;
            txtNguoiCapNhat.Text = "";            
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
                else if (cmbGD_HinhThuc.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaChon(lblHinhThucGD.Content.ToString());
                    cmbGD_HinhThuc.Focus();
                    return false;
                }
                else if (numGD_TongTien.Text.IsNullOrEmptyOrSpace() || numGD_TongTien.Value == 0)
                {
                    CommonFunction.ThongBaoChuaNhap(lblTongTienGD.Content.ToString());
                    numGD_TongTien.Focus();
                    return false;
                }
                else if (txtDienGiai.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaNhap(lblDienGiai.Content.ToString());
                    txtDienGiai.Focus();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {                
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                return false;
                throw ex;
            }
        }


        public void OnHold()
        {
            try
            {
                if (!Validation()) return;

                string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));                
                obj = new HDV_GUI_TIEN_THEO_SO();
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

        public void OnSave()
        {
            try
            {
                if (!Validation()) return;

                string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));
                obj = new HDV_GUI_TIEN_THEO_SO();
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
            SetEnabledAllControls(false);
            action = DatabaseConstant.Action.XEM;
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO);
            tlbPreview.IsEnabled = true;
        }

        public void BeforeViewFromList()
        {
            SetFormData();
            BeforeViewFromDetail();
        }


        public void BeforeAddNew()
        {
            ResetForm();                        
            SetEnabledAllControls(true);            
            action = DatabaseConstant.Action.THEM;
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO);
        }

        public void OnAddNew(HDV_GUI_TIEN_THEO_SO obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processHDV.GuiThemTheoTungSo(DatabaseConstant.Action.THEM, ref obj, ref listClientResponseDetail);
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

        public void AfterAddNew(bool ret, HDV_GUI_TIEN_THEO_SO obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = obj.TRANG_THAI_NGHIEP_VU;
                    maGiaoDich = obj.MA_GDICH;
                    txtSoGD.Text = obj.MA_GDICH;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TRANG_THAI_BAN_GHI);
                    raddtNgayLap.Value = LDateTime.StringToDate(obj.NGAY_LAP, "yyyyMMdd");
                    txtNguoiLap.Text = obj.NGUOI_LAP;

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
                    DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                if (ret)
                {
                    SetEnabledAllControls(true);                           
                    action = DatabaseConstant.Action.SUA;
                    CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO);
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
            //BeforeModifyFromDetail();
            if(sTrangThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                SetEnabledAllControls(false);
            else
                SetEnabledAllControls(true);            
            action = DatabaseConstant.Action.SUA;
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO);
        }

        public void OnModify(HDV_GUI_TIEN_THEO_SO obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processHDV.GuiThemTheoTungSo(DatabaseConstant.Action.SUA, ref obj, ref listClientResponseDetail);
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

        public void AfterModify(bool ret, HDV_GUI_TIEN_THEO_SO obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = obj.TRANG_THAI_NGHIEP_VU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TRANG_THAI_BAN_GHI);
                    raddtNgayLap.Value = LDateTime.StringToDate(obj.NGAY_LAP, "yyyyMMdd");
                    txtNguoiLap.Text = obj.NGUOI_LAP;
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

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
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
                        DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
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
                    DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnDelete()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                bool ret = false;
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                obj.MA_GDICH = txtSoGD.Text;
                ret = processHDV.GuiThemTheoTungSo(action, ref obj, ref listClientResponseDetail);
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
                    DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                // Đóng cửa sổ chi tiết sau khi xóa
                OnClose();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
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
                        DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
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
                    DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.DUYET,
                    listLockId);

                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnApprove()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                bool ret = false;
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                obj.MA_GDICH = txtSoGD.Text;
                ret = processHDV.GuiThemTheoTungSo(action, ref obj, ref listClientResponseDetail);
                AfterApprove(ret, listClientResponseDetail);
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

        public void AfterApprove(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    numSoDu.Value = Convert.ToDouble(obj.SO_DU);

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
                    DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.DUYET,
                    listLockId);                
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
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
                        DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
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
                    DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listLockId);

                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnCancel()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                bool ret = false;
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                obj.MA_GDICH = txtSoGD.Text;
                ret = processHDV.GuiThemTheoTungSo(action, ref obj, ref listClientResponseDetail);
                AfterCancel(ret, listClientResponseDetail);
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
                    DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listLockId);
                
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
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
                        DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
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
                    DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);

                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnRefuse()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                bool ret = false;
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                obj.MA_GDICH = txtSoGD.Text;
                ret = processHDV.GuiThemTheoTungSo(action, ref obj, ref listClientResponseDetail);
                AfterRefuse(ret, listClientResponseDetail);
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
                    DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void OnPreview()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                // Cảnh báo khi không có dữ liệu
                if (LObject.IsNullOrEmpty(id) && LObject.IsNullOrEmpty(maGiaoDich))
                {
                    LMessage.ShowMessage("M.DungChung.KhongCoThongTin", LMessage.MessageBoxType.Warning);
                    return;
                }
                else
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                    DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();

                    GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                    objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.GDKT_IN_GIAO_DICH;
                    objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO;

                    GDKT_GIAO_DICH objGDKT_GIAO_DICH = new GDKT_GIAO_DICH();
                    objGDKT_GIAO_DICH.MaGiaoDich = maGiaoDich;

                    doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                    doiTuongBaoCao.objGDKT_GIAO_DICH = objGDKT_GIAO_DICH;
                    xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            
        }

        #endregion        

        private void cbMultiAdd_Checked(object sender, RoutedEventArgs e)
        {
            //reset biến
            obj = null;
            _objKiemSoat = null;
            id = 0;
            maGiaoDich = "";
            maSanPham = "";
            sTrangThaiNVu = "";    
        
            BeforeAddNew();

            cbMultiAdd.IsChecked = false;
            txtSoTGui.Focus();
        }

    }
}
