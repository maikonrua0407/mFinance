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

namespace PresentationWPF.HuyDongVon.RutGoc
{
    /// <summary>
    /// Interaction logic for ucRutGocCT.xaml
    /// </summary>
    public partial class ucRutGocCT : UserControl
    {
        #region Khai bao
        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private DatabaseConstant.Function function = DatabaseConstant.Function.HDV_RUT_BOT_GOC;

        public event EventHandler OnSavingCompleted;

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private KIEM_SOAT _objKiemSoat = null;

        private HDV_RUT_GOC_MOT_PHAN obj;
        public HDV_RUT_GOC_MOT_PHAN Obj
        {
            get { return obj; }
            set { obj = value; }
        }

        private List<THONG_TIN_TIEN_LAI> lstThongTinLai = null;

        private string sTrangThaiNVu = "";

        private string maGiaoDich = "";

        private decimal tienLaiTinhDuoc = 0;

        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
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
        public ucRutGocCT()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            LoadCombobox();

            HideControl();

            txtSoTGui.Focus();
        }

        public ucRutGocCT(KIEM_SOAT obj)
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            LoadCombobox();

            _objKiemSoat = obj;

            action = _objKiemSoat.action;

            sTrangThaiNVu = _objKiemSoat.TTHAI_NVU;

            maGiaoDich = _objKiemSoat.SO_GIAO_DICH;

            this.obj = new HDV_RUT_GOC_MOT_PHAN();

            HideControl();

            tlbHelp.Focus();
        }
        
        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.HuyDongVon;component/RutGoc/ucRutGocCT.xaml", ref Toolbar, ref mnuMain);
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
                arr = hethong.SetVisibleControl("PresentationWPF.HuyDongVon.RutGoc.ucRutGocCT", "");
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
                DatabaseConstant.Function.HDV_RUT_BOT_GOC,
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
            //Thông tin sổ tiền gửi
            txtSoTGui.IsEnabled = enable;
            btnSoTGui.IsEnabled = enable;

            //Thông tin Rút gốc
            numSoTienRutGoc.IsEnabled = enable;

            //Thông tin rút lãi            
            numSoThang.IsEnabled = enable;
            numLaiSuatThang.IsEnabled = enable;
            numTienLaiThang.IsEnabled = enable;
            numSoNgay.IsEnabled = enable;
            numLaiSuatNgay.IsEnabled = enable;
            numTienLaiNgay.IsEnabled = enable;
            chkRutLai.IsEnabled = enable;

            // Giao dịch
            cmbGD_HinhThuc.IsEnabled = enable;
            if (enable == true)
            {
                string sHinhThucGD = lstSourceGD_HinhThuc.ElementAt(cmbGD_HinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                if (sHinhThucGD.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri()))
                {
                    numGD_SoTienTM.IsEnabled = false;
                    numGD_SoTienCK.IsEnabled = false;
                    txtGD_TaiKhoanKH.IsEnabled = false;
                    btnGD_TaiKhoanKH.IsEnabled = false;                    
                }
                else if (sHinhThucGD.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN.layGiaTri()))
                {
                    numGD_SoTienTM.IsEnabled = false;
                    numGD_SoTienCK.IsEnabled = false;
                    txtGD_TaiKhoanKH.IsEnabled = true;
                    btnGD_TaiKhoanKH.IsEnabled = true;                    
                }
                else if (sHinhThucGD.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TMAT_CKHOAN.layGiaTri()))
                {
                    numGD_SoTienTM.IsEnabled = false;
                    numGD_SoTienCK.IsEnabled = true;
                    txtGD_TaiKhoanKH.IsEnabled = true;
                    btnGD_TaiKhoanKH.IsEnabled = true;                 
                }
            }
            else
            {
                numGD_TongTien.IsEnabled = enable;
                numGD_SoTienTM.IsEnabled = enable;
                numGD_SoTienCK.IsEnabled = enable;
                txtGD_TaiKhoanKH.IsEnabled = enable;                
                btnGD_TaiKhoanKH.IsEnabled = enable;                
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
                        if (chkRutLai.IsChecked == true)
                        {
                            TinhToan();
                        }
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

                        lblIDKhachHang.Content= "0";
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
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                lstPopup.Clear();
                Window window = new Window();
                ucPopupSoTGui uc = new ucPopupSoTGui();
                uc.Function = function;
                uc.DuLieuTraVe = new ucPopupSoTGui.LayDuLieu(LayDuLieuTuPopup);
                window.Title = LLanguage.SearchResourceByKey("MENU.TOOLTIP.5215_MO_SO_DS");
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
                        if (chkRutLai.IsChecked == true)
                        {
                            TinhToan();
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

        private void chkRutLai_Checked(object sender, RoutedEventArgs e)
        {
            if (!txtSoTGui.ToString().IsNullOrEmpty())
                TinhToan();
        }

        private void chkRutLai_Unchecked(object sender, RoutedEventArgs e)
        {
            numSoThang.Value = 0;
            numLaiSuatThang.Value = 0;
            numTienLaiThang.Value = 0;
            numSoNgay.Value = 0;
            numLaiSuatNgay.Value = 0;
            numTienLaiNgay.Value = 0;
            numTongRutLai.Value = 0;
            numLaiDaDC.Value = 0;

            numGD_TongTien.Value = numSoTienRutGoc.Value + numTongRutLai.Value;
            numGD_TongTien_LostFocus(null, null);

            lstThongTinLai = null;
        }

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

                    numGD_SoTienTM.Value = numGD_TongTien.Value;
                    numGD_SoTienCK.Value = 0;
                }
                else if (sHinhThucGD.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN.layGiaTri()))
                {
                    numGD_SoTienTM.IsEnabled = false;
                    numGD_SoTienCK.IsEnabled = false;
                    txtGD_TaiKhoanKH.IsEnabled = true;
                    btnGD_TaiKhoanKH.IsEnabled = true;                    

                    numGD_SoTienTM.Value = 0;
                    numGD_SoTienCK.Value = numGD_TongTien.Value;
                }
                else if (sHinhThucGD.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TMAT_CKHOAN.layGiaTri()))
                {
                    numGD_SoTienTM.IsEnabled = false;
                    numGD_SoTienCK.IsEnabled = true;
                    txtGD_TaiKhoanKH.IsEnabled = true;
                    btnGD_TaiKhoanKH.IsEnabled = true;                    

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
                lstDieuKien.Add(lblIDKhachHang.Content.ToString());
                lstDieuKien.Add("KHACH_HANG");

                var process = new PopupProcess();
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_TAI_KHOAN.getValue(), lstDieuKien);

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

        private void numSoTienRutGoc_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtSoTGui.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaChon(lblSoTGui.Content.ToString());
                numSoTienRutGoc.Value = 0;
                txtSoTGui.Focus();
                return;
            }

            if (numSoTienRutGoc.Value > numSoDu.Value)
            {
                string soTien = LLanguage.SearchResourceByKey("U.DungChung.SoTienRG");
                string soTienTong = LLanguage.SearchResourceByKey("U.DungChung.SoDu");
                LMessage.ShowMessage(soTien + " (" + numSoTienRutGoc.Text + ") > " + soTienTong + " (" + numSoDu.Text + ")", LMessage.MessageBoxType.Warning);
            }

            numGD_TongTien.Value = numSoTienRutGoc.Value + numTongRutLai.Value;
            numGD_TongTien_LostFocus(null, null);
        }

        private void numTienLaiThang_LostFocus(object sender, RoutedEventArgs e)
        {
            numTongRutLai.Value = numTienLaiThang.Value + numTienLaiNgay.Value;
            numGD_TongTien.Value = numSoTienRutGoc.Value + numTongRutLai.Value;
            numGD_TongTien_LostFocus(null, null);
        }

        private void numTienLaiNgay_LostFocus(object sender, RoutedEventArgs e)
        {
            numTongRutLai.Value = numTienLaiThang.Value + numTienLaiNgay.Value;
            numGD_TongTien.Value = numSoTienRutGoc.Value + numTongRutLai.Value;
            numGD_TongTien_LostFocus(null, null);
        }

        private void numTongRutLai_LostFocus(object sender, RoutedEventArgs e)
        {            
            numGD_TongTien.Value = numSoTienRutGoc.Value + numTongRutLai.Value;
            numGD_TongTien_LostFocus(null, null);
        }

        #endregion              

        #region Xử lý nghiệp vụ
        private void GetFormData(ref HDV_RUT_GOC_MOT_PHAN obj, string sTrangThaiNVu)
        {
            try
            {
                //Thông tin sổ               
                obj.MA_GDICH = txtSoGD.Text;
                obj.NGAY_GDICH = ClientInformation.NgayLamViecHienTai;
                obj.MA_DVI = ClientInformation.MaDonViGiaoDich;
                obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                obj.SO_SO_TG = txtSoTGui.Text;
                obj.SO_DU = Convert.ToDecimal(numSoDu.Value);
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
                obj.HINH_THUC_GIAO_DICH = lstSourceGD_HinhThuc.ElementAt(cmbGD_HinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                obj.TONG_TIEN_GIAO_DICH = Convert.ToDecimal(numGD_TongTien.Value);
                obj.SO_TIEN_MAT = Convert.ToDecimal(numGD_SoTienTM.Value);
                obj.SO_TIEN_CHUYEN_KHOAN = Convert.ToDecimal(numGD_SoTienCK.Value);
                obj.TAI_KHOAN_KHACH_HANG = txtGD_TaiKhoanKH.Text;                
                obj.DIEN_GIAI = txtDienGiai.Text;

                //Thông tin rút gốc
                obj.SO_TIEN_RUT_GOC = Convert.ToDecimal(numSoTienRutGoc.Value);

                //Thông tin rút lãi
                obj.THUC_HIEN_RUT_LAI = (chkRutLai.IsChecked == true ? BusinessConstant.CoKhong.CO.layGiaTri() : BusinessConstant.CoKhong.KHONG.layGiaTri());
                obj.THONG_TIN_TINH_LAI = (radSoDuThucTe.IsChecked == true ? "SoDuThucTe" : "SoTienRutGoc");
                obj.SO_THANG = Convert.ToInt32(numSoThang.Value);
                obj.LAI_SUAT_THANG = Convert.ToDecimal(numLaiSuatThang.Value);
                obj.TIEN_LAI_THANG = Convert.ToDecimal(numTienLaiThang.Value);
                obj.SO_NGAY = Convert.ToInt32(numSoNgay.Value);
                obj.LAI_SUAT_NGAY = Convert.ToDecimal(numLaiSuatNgay.Value);
                obj.TIEN_LAI_NGAY = Convert.ToDecimal(numTienLaiNgay.Value);
                obj.TONG_TIEN_RUT_LAI = Convert.ToDecimal(numTongRutLai.Value);
                obj.LAI_DA_DU_CHI = Convert.ToDecimal(numLaiDaDC.Value);
                obj.TIEN_LAI_TINH_DUOC = tienLaiTinhDuoc;

                if (lstThongTinLai != null)
                    obj.LIST_THONG_TIN_LAI = lstThongTinLai.ToArray();

                

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
            HuyDongVonProcess processHDV = new HuyDongVonProcess();
            try
            {
                DataSet ds = processHDV.GetThongTinRutGoc(ClientInformation.MaDonViGiaoDich, maGiaoDich);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

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

                    #region Thông tin rút gốc
                    numSoTienRutGoc.Value = Convert.ToDouble(dr["RGOC_SO_TIEN"]); //Tạm thời để bằng tiền GD do database thiếu trường
                    #endregion

                    #region Thông tin rút lãi
                    if (dr["RUT_LAI"].ToString().Equals(BusinessConstant.CoKhong.CO.layGiaTri()))
                    {
                        chkRutLai.IsChecked = true;

                        if (dr["RLAI_LOAI_TLAI"].ToString().Equals("SoDuThucTe")) radSoDuThucTe.IsChecked = true;
                        else radSoTienRutGoc.IsChecked = true;
                        numSoThang.Value = Convert.ToDouble(dr["RLAI_THANG_SO"]);
                        numLaiSuatThang.Value = Convert.ToDouble(dr["RLAI_THANG_LSUAT"]);
                        numTienLaiThang.Value = Convert.ToDouble(dr["RLAI_THANG_SOTIEN"]);
                        numSoNgay.Value = Convert.ToDouble(dr["RLAI_NGAY_SO"]);
                        numLaiSuatNgay.Value = Convert.ToDouble(dr["RLAI_NGAY_LSUAT"]);
                        numTienLaiNgay.Value = Convert.ToDouble(dr["RLAI_NGAY_SOTIEN"]);
                        numTongRutLai.Value = Convert.ToDouble(dr["RLAI_TONG_TIEN"]);
                        numLaiDaDC.Value = Convert.ToDouble(dr["RLAI_DU_CHI"]);
                        tienLaiTinhDuoc = Convert.ToDecimal(dr["TIEN_LAI_TINH_DUOC"]);
                    }
                    #endregion

                    #region Thông tin giao dịch
                    cmbGD_HinhThuc.SelectedIndex = lstSourceGD_HinhThuc.IndexOf(lstSourceGD_HinhThuc.FirstOrDefault(i => i.KeywordStrings.First().Equals(dr["GDICH_HTHUC"].ToString())));
                    numGD_TongTien.Value = Convert.ToDouble(dr["GDICH_TIEN_MAT"]); //Tạm thời để bằng tiền GD do database thiếu trường
                    numGD_SoTienTM.Value = Convert.ToDouble(dr["GDICH_TIEN_MAT"]);
                    numGD_SoTienCK.Value = Convert.ToDouble(dr["GDICH_TIEN_CKHOAN"]);
                    txtGD_TaiKhoanKH.Text = dr["GDICH_TKHOAN_KHANG"].ToString();
                    lblGD_TaiKhoanKH.Content = dr["TEN_TAI_KHOAN"].ToString();                    
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

            //Thông tin rút gốc, lãi
            numSoTienRutGoc.Value = 0;
            numSoThang.Value = 0;
            numLaiSuatThang.Value = 0;
            numTienLaiThang.Value = 0;
            numSoNgay.Value = 0;
            numLaiSuatNgay.Value = 0;
            numTienLaiNgay.Value = 0;
            numTongRutLai.Value = 0;
            numLaiDaDC.Value = 0;

            //Thông tin giao dịch
            cmbGD_HinhThuc.SelectedIndex = lstSourceGD_HinhThuc.IndexOf(lstSourceGD_HinhThuc.FirstOrDefault(e => e.KeywordStrings.First().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri())));
            numGD_TongTien.Value = 0;
            numGD_SoTienTM.Value = 0;
            numGD_SoTienCK.Value = 0;
            txtGD_TaiKhoanKH.Text = "";          
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
                else if (numSoTienRutGoc.Text.IsNullOrEmptyOrSpace() || numSoTienRutGoc.Value == 0)
                {
                    CommonFunction.ThongBaoChuaNhap(lblSoTienRutGoc.Content.ToString());
                    numSoTienRutGoc.Focus();
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
            }
        }

        private bool KiemTraHopLeSoDuKhiRutGoc()
        {
            HuyDongVonProcess processHDV = new HuyDongVonProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            bool ret = false;

            obj = new HDV_RUT_GOC_MOT_PHAN();
            GetFormData(ref obj, sTrangThaiNVu);
            ret = processHDV.RutBotGoc(DatabaseConstant.Action.KIEM_TRA, ref obj, ref listClientResponseDetail);

            if (ret == false)
            {
                LMessage.ShowMessage("M_ResponseMessage_HuyDongVon_SoTienRutGocKhongHopLe", LMessage.MessageBoxType.Warning);

                if (LMessage.ShowMessage("M.DungChung.TiepTucThucHien", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    return true;
                else
                    return false;
            }
            else
            {
                return true;
            }
        }


        public void OnHold()
        {
            try
            {
                if (!Validation()) return;

                if (!KiemTraHopLeSoDuKhiRutGoc()) return;

                string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));
                obj = new HDV_RUT_GOC_MOT_PHAN();
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

                if (!KiemTraHopLeSoDuKhiRutGoc()) return;

                string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));
                obj = new HDV_RUT_GOC_MOT_PHAN();
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
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain,function);
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
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain,function);
        }

        public void OnAddNew(HDV_RUT_GOC_MOT_PHAN obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processHDV.RutBotGoc(DatabaseConstant.Action.THEM, ref obj, ref listClientResponseDetail);
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

        public void AfterAddNew(bool ret, HDV_RUT_GOC_MOT_PHAN obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = obj.TRANG_THAI_NGHIEP_VU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    maGiaoDich = obj.MA_GDICH;
                    txtSoGD.Text = obj.MA_GDICH;
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
                    DatabaseConstant.Function.HDV_RUT_BOT_GOC,
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
            SetFormData();
            if (sTrangThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                SetEnabledAllControls(false);
            else
                SetEnabledAllControls(true);
            action = DatabaseConstant.Action.SUA;
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain,function);
        }

        public void OnModify(HDV_RUT_GOC_MOT_PHAN obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processHDV.RutBotGoc(DatabaseConstant.Action.SUA, ref obj, ref listClientResponseDetail);
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

        public void AfterModify(bool ret, HDV_RUT_GOC_MOT_PHAN obj, List<ClientResponseDetail> listClientResponseDetail)
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
                    DatabaseConstant.Function.HDV_RUT_BOT_GOC,
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
                        DatabaseConstant.Function.HDV_RUT_BOT_GOC,
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
                    DatabaseConstant.Function.HDV_RUT_BOT_GOC,
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
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                obj.MA_GDICH = txtSoGD.Text;
                ret = processHDV.RutBotGoc(action, ref obj, ref listClientResponseDetail);
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

        public void AfterDelete(bool ret,List<ClientResponseDetail> listClientResponseDetail)
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
                    DatabaseConstant.Function.HDV_RUT_BOT_GOC,
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
                    action = DatabaseConstant.Action.DUYET;
                    OnApprove();
                    /*
                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        DatabaseConstant.Function.HDV_RUT_BOT_GOC,
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
                    }*/
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
                    DatabaseConstant.Function.HDV_RUT_BOT_GOC,
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
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                obj.MA_GDICH = txtSoGD.Text;
                ret = processHDV.RutBotGoc(action, ref obj, ref listClientResponseDetail);
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

        public void AfterApprove(bool ret,List<ClientResponseDetail> listClientResponseDetail)
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
                    DatabaseConstant.Function.HDV_RUT_BOT_GOC,
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
                        DatabaseConstant.Function.HDV_RUT_BOT_GOC,
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
                    DatabaseConstant.Function.HDV_RUT_BOT_GOC,
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
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                obj.MA_GDICH = txtSoGD.Text;
                ret = processHDV.RutBotGoc(action, ref obj, ref listClientResponseDetail);
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

        public void AfterCancel(bool ret,List<ClientResponseDetail> listClientResponseDetail)
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
                    DatabaseConstant.Function.HDV_RUT_BOT_GOC,
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
                        DatabaseConstant.Function.HDV_RUT_BOT_GOC,
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
                    DatabaseConstant.Function.HDV_RUT_BOT_GOC,
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
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                obj.MA_GDICH = txtSoGD.Text;
                ret = processHDV.RutBotGoc(action, ref obj, ref listClientResponseDetail);
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
                    DatabaseConstant.Function.HDV_RUT_BOT_GOC,
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

        private void TinhToan()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                obj = new HDV_RUT_GOC_MOT_PHAN();
                GetFormData(ref obj, sTrangThaiNVu);
                obj.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processHDV.RutBotGoc(DatabaseConstant.Action.TINH_TOAN, ref obj, ref listClientResponseDetail);
                AfterTinhToan(ret, obj, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public void AfterTinhToan(bool ret, HDV_RUT_GOC_MOT_PHAN obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    numSoThang.Value = Convert.ToDouble(obj.SO_THANG);
                    numLaiSuatThang.Value = Convert.ToDouble(obj.LAI_SUAT_THANG);
                    numTienLaiThang.Value = Convert.ToDouble(obj.TIEN_LAI_THANG);

                    numSoNgay.Value = Convert.ToDouble(obj.SO_NGAY);
                    numLaiSuatNgay.Value = Convert.ToDouble(obj.LAI_SUAT_NGAY);
                    numTienLaiNgay.Value = Convert.ToDouble(obj.TIEN_LAI_NGAY);

                    tienLaiTinhDuoc = obj.TIEN_LAI_TINH_DUOC;
                    numTongRutLai.Value = Convert.ToDouble(obj.TONG_TIEN_RUT_LAI);
                    numLaiDaDC.Value = Convert.ToDouble(obj.LAI_DA_DU_CHI);

                    numGD_TongTien.Value = numSoTienRutGoc.Value + numTongRutLai.Value;

                    numGD_TongTien_LostFocus(null, null);

                    lstThongTinLai = obj.LIST_THONG_TIN_LAI.ToList();
                }
                else
                {
                    //LMessage.ShowMessage(listClientResponseDetail[0].Detail, LMessage.MessageBoxType.Warning);
                    tienLaiTinhDuoc = 0;
                    numSoThang.Value = 0;
                    numLaiSuatThang.Value = 0;
                    numTienLaiThang.Value = 0;
                    numSoNgay.Value = 0;
                    numLaiSuatNgay.Value = 0;
                    numTienLaiNgay.Value = 0;
                    numTongRutLai.Value = 0;
                    numLaiDaDC.Value = 0;

                    numGD_TongTien.Value = numSoTienRutGoc.Value + numTongRutLai.Value;

                    numGD_TongTien_LostFocus(null, null);

                    lstThongTinLai = null;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
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
                    objGIAO_DICH_BASE.ChucNang = function;

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
            sTrangThaiNVu = "";

            BeforeAddNew();

            cbMultiAdd.IsChecked = false;
            txtSoTGui.Focus();
        }        
    }
}
