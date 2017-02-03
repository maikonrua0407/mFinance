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
using Presentation.Process.HuyDongVonServiceRef;
using Presentation.Process.NhanSuServiceRef;
using Presentation.Process.KhachHangServiceRef;
using Presentation.Process.BaoCaoServiceRef;
using PresentationWPF.CustomControl;
using PresentationWPF.NhanSu.Converts;

namespace PresentationWPF.NhanSu.ChuyenCongTac
{
    /// <summary>
    /// Interaction logic for ucChuyenCongTacCT.xaml
    /// </summary>
    public partial class ucChuyenCongTacCT : UserControl
    {
        #region Khai bao
        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private DatabaseConstant.Function function = DatabaseConstant.Function.NS_THUYEN_CHUYEN_CT;

        public event EventHandler OnSavingCompleted;

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int idHoSo = 0;
       
        private NS_TCHUYEN_CTAC obj;
        public NS_TCHUYEN_CTAC Obj
        {
            get { return obj; }
            set { obj = value; }
        }

        private string loaiThoiHan = BusinessConstant.LOAI_THOI_HAN_HDLD.CO_THOI_HAN.layGiaTri();

        private string sTrangThaiNVu = "";

        List<AutoCompleteEntry> lstSourceViTriCu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceViTriMoi = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongBanCu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongBanMoi = new List<AutoCompleteEntry>();        

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
        public ucChuyenCongTacCT()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            InitEventHandler();
        }        

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.NhanSu;component/ChuyenCongTac/ucChuyenCongTacCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void InitEventHandler()
        {
            chkThemNhieuLan.Checked += new RoutedEventHandler(chkThemNhieuLan_Checked);

            txtNhanVien.KeyDown += new KeyEventHandler(txtNhanVien_KeyDown);
            btnNhanVien.Click += new RoutedEventHandler(btnNhanVien_Click);
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

            bool ret = process.UnlockData(DatabaseConstant.Module.NSTL,
                DatabaseConstant.Function.NS_THUYEN_CHUYEN_CT,
                DatabaseConstant.Table.NS_TCHUYEN_CTAC,
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

        private void chkThemNhieuLan_Checked(object sender, RoutedEventArgs e)
        {
            action = DatabaseConstant.Action.THEM;
            id = 0;
            idHoSo = 0;            
            obj = null;            
            sTrangThaiNVu = "";

            ResetForm();
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
            txtTenNhanVien.Focus();

            chkThemNhieuLan.IsChecked = false;
        }        

        private void btnNhanVien_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lstPopup = null;
                var process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_HOSO_NVIEN.getValue(), lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow row = lstPopup[0];
                    if (!string.IsNullOrWhiteSpace(row[1].ToString()))
                        idHoSo = Convert.ToInt32(row[1].ToString());
                    if (!string.IsNullOrWhiteSpace(row[2].ToString()))
                        txtNhanVien.Text = row[2].ToString();
                    if (!string.IsNullOrWhiteSpace(row[3].ToString()))
                        txtTenNhanVien.Text = row[3].ToString();
                    if (!string.IsNullOrWhiteSpace(row[4].ToString()))
                    {
                        cmbChucVuCu.SelectedIndex = lstSourceViTriCu.IndexOf(lstSourceViTriCu.FirstOrDefault(i => i.KeywordStrings[1].Equals(row[4].ToString())));
                        cmbChucVuMoi.SelectedIndex = lstSourceViTriMoi.IndexOf(lstSourceViTriMoi.FirstOrDefault(i => i.KeywordStrings[1].Equals(row[4].ToString())));
                    }
                    if (!string.IsNullOrWhiteSpace(row[5].ToString()))
                    {
                        cmbPhongBanCu.SelectedIndex = lstSourcePhongBanCu.IndexOf(lstSourcePhongBanCu.FirstOrDefault(i => i.KeywordStrings[1].Equals(row[5].ToString())));
                        cmbPhongBanMoi.SelectedIndex = lstSourcePhongBanMoi.IndexOf(lstSourcePhongBanMoi.FirstOrDefault(i => i.KeywordStrings[1].Equals(row[5].ToString())));
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                CommonFunction.ThongBaoLoi(ex);
            }
        }

        private void txtNhanVien_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnNhanVien_Click(null, null);
            }
        }
        #endregion               

        #region Xử lý nghiệp vụ
        private void GetFormData(ref NS_TCHUYEN_CTAC obj, string sTrangThaiNVu)
        {
            try
            {
                obj = new NS_TCHUYEN_CTAC();
                obj.ID = id;
                obj.ID_HSO = idHoSo;
                obj.MA_TCHUYEN_CTAC = txtMa.Text;
                obj.NGAY_DLIEU = ClientInformation.NgayLamViecHienTai;
                obj.NGAY_HLUC = Convert.ToDateTime(raddtNgayChuyenCongTac.Value).ToString("yyyyMMdd");
                if(raddtNgayBanHanh.Value != null && !raddtNgayBanHanh.Text.Equals("__/__/____"))
                    obj.QDINH_NGAY = Convert.ToDateTime(raddtNgayBanHanh.Value).ToString("yyyyMMdd");
                obj.QDINH_NKY = txtNguoiBanHanh.Text;
                obj.QDINH_SO = txtSoQuyetDinh.Text;
                obj.ID_BO_PHAN_CU = Convert.ToInt32(lstSourcePhongBanCu[cmbPhongBanCu.SelectedIndex].KeywordStrings[1]);
                obj.ID_BO_PHAN_MOI = Convert.ToInt32(lstSourcePhongBanMoi[cmbPhongBanMoi.SelectedIndex].KeywordStrings[1]);
                obj.ID_CHUC_VU_CU = Convert.ToInt32(lstSourceViTriCu[cmbChucVuCu.SelectedIndex].KeywordStrings[1]);
                obj.ID_CHUC_VU_MOI = Convert.ToInt32(lstSourceViTriMoi[cmbChucVuMoi.SelectedIndex].KeywordStrings[1]);
                obj.GHI_CHU = txtDienGiai.Text;
                //Thông tin kiểm soát
                obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.TTHAI_NVU = sTrangThaiNVu;
                obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                obj.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
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
            NhanSuProcess processNhanSu = new NhanSuProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            obj = new NS_TCHUYEN_CTAC();
            NS_TEMP_TCHUYEN_CTAC objTempThuyenChuyenCongTac = null;            
            List<NS_DM_CHUC_VU> lstDMChucVu = null;
            List<NS_DM_DVI_CTAC> lstDMDonViCongTac = null;
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processNhanSu.ThuyenChuyenCongTac(DatabaseConstant.Action.LOAD, ref obj, ref objTempThuyenChuyenCongTac,ref lstDMChucVu, ref lstDMDonViCongTac, ref listClientResponseDetail);
                if (ret == true)
                {
                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    #region Dữ liệu lên combobox
                    lstSourceViTriCu = ConvertNhanSu.ToAutoCompleteEntry(lstDMChucVu);
                    lstSourceViTriMoi = ConvertNhanSu.ToAutoCompleteEntry(lstDMChucVu);
                    lstSourcePhongBanCu = ConvertNhanSu.ToAutoCompleteEntry(lstDMDonViCongTac);
                    lstSourcePhongBanMoi = ConvertNhanSu.ToAutoCompleteEntry(lstDMDonViCongTac);

                    AutoComboBox auto = new AutoComboBox();
                    auto.GenAutoComboBoxBySource(ref lstSourceViTriCu, ref cmbChucVuCu);
                    auto.GenAutoComboBoxBySource(ref lstSourceViTriMoi, ref cmbChucVuMoi);
                    auto.GenAutoComboBoxBySource(ref lstSourcePhongBanCu, ref cmbPhongBanCu);
                    auto.GenAutoComboBoxBySource(ref lstSourcePhongBanMoi, ref cmbPhongBanMoi);
                    #endregion                    

                    #region Thông tin chung
                    idHoSo = objTempThuyenChuyenCongTac.ID_NHAN_VIEN;
                    txtNhanVien.Text = objTempThuyenChuyenCongTac.MA_NHAN_VIEN;
                    txtTenNhanVien.Text = objTempThuyenChuyenCongTac.TEN_NHAN_VIEN;
                    raddtNgayChuyenCongTac.Value = LDateTime.StringToDate(obj.NGAY_HLUC, "yyyyMMdd");
                    #endregion

                    #region Thông tin quyết định
                    txtSoQuyetDinh.Text = obj.QDINH_SO;
                    txtNguoiBanHanh.Text = obj.QDINH_NKY;
                    if (LDateTime.IsDate(obj.QDINH_NGAY, "yyyyMMdd"))
                        raddtNgayCapNhat.Value = LDateTime.StringToDate(obj.QDINH_NGAY, "yyyyMMdd");
                    #endregion

                    #region Thông tin chuyển công tác
                    txtMa.Text = obj.MA_TCHUYEN_CTAC;
                    cmbChucVuCu.SelectedIndex = lstSourceViTriCu.IndexOf(lstSourceViTriCu.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.ID_CHUC_VU_CU.ToString())));
                    cmbChucVuMoi.SelectedIndex = lstSourceViTriMoi.IndexOf(lstSourceViTriMoi.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.ID_CHUC_VU_MOI.ToString())));
                    cmbPhongBanCu.SelectedIndex = lstSourcePhongBanCu.IndexOf(lstSourcePhongBanCu.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.ID_BO_PHAN_CU.ToString())));
                    cmbPhongBanMoi.SelectedIndex = lstSourcePhongBanMoi.IndexOf(lstSourcePhongBanMoi.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.ID_BO_PHAN_MOI.ToString())));
                    txtDienGiai.Text = obj.GHI_CHU;
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
                processNhanSu = null;
                listClientResponseDetail = null;                
                lstDMChucVu = null;
                lstDMDonViCongTac = null;
            }
        }

        private void LoadFormData()
        {
            NhanSuProcess processNhanSu = new NhanSuProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            obj = new NS_TCHUYEN_CTAC();
            NS_TEMP_TCHUYEN_CTAC objTempThuyenChuyenCongTac = null;
            List<NS_DM_CHUC_VU> lstDMChucVu = null;
            List<NS_DM_DVI_CTAC> lstDMDonViCongTac = null;            
            try
            {
                bool ret = false;
                ret = processNhanSu.ThuyenChuyenCongTac(DatabaseConstant.Action.LOAD, ref obj, ref objTempThuyenChuyenCongTac, ref lstDMChucVu, ref lstDMDonViCongTac, ref listClientResponseDetail);
                if (ret == true)
                {
                    lstSourceViTriCu = ConvertNhanSu.ToAutoCompleteEntry(lstDMChucVu);
                    lstSourceViTriMoi = ConvertNhanSu.ToAutoCompleteEntry(lstDMChucVu);
                    lstSourcePhongBanCu = ConvertNhanSu.ToAutoCompleteEntry(lstDMDonViCongTac);
                    lstSourcePhongBanMoi = ConvertNhanSu.ToAutoCompleteEntry(lstDMDonViCongTac);

                    AutoComboBox auto = new AutoComboBox();
                    auto.GenAutoComboBoxBySource(ref lstSourceViTriCu, ref cmbChucVuCu);
                    auto.GenAutoComboBoxBySource(ref lstSourceViTriMoi, ref cmbChucVuMoi);
                    auto.GenAutoComboBoxBySource(ref lstSourcePhongBanCu, ref cmbPhongBanCu);
                    auto.GenAutoComboBoxBySource(ref lstSourcePhongBanMoi, ref cmbPhongBanMoi);
                }
                else
                {
                    LMessage.ShowMessage("M_ResponseMessage_HanhDong_LayDuLieuKhongThanhCong", LMessage.MessageBoxType.Warning);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                processNhanSu = null;
                listClientResponseDetail = null;
                lstDMChucVu = null;
                lstDMDonViCongTac = null;
            }
        }

        private void ResetForm()
        {
            lblTrangThai.Content = "";

            #region Thông tin chung
            txtNhanVien.Text = "";
            txtTenNhanVien.Text = "";
            raddtNgayChuyenCongTac.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            #endregion

            #region Thông tin quyết đinh
            txtSoQuyetDinh.Text = "";
            txtNguoiBanHanh.Text = "";
            raddtNgayBanHanh.Value = null;
            #endregion

            #region Thông tin chuyển công tác
            txtMa.Text = "";
            cmbPhongBanCu.SelectedIndex = -1;
            cmbPhongBanMoi.SelectedIndex = -1;
            cmbChucVuCu.SelectedIndex = -1;
            cmbChucVuMoi.SelectedIndex = -1;
            txtDienGiai.Text = "";
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
                if (txtNhanVien.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaChon(lblNhanVien.Content.ToString());
                    txtNhanVien.Focus();
                    return false;
                }
                if (raddtNgayChuyenCongTac.Value == null || raddtNgayChuyenCongTac.Text.Equals("__/__/____"))
                {
                    CommonFunction.ThongBaoChuaNhap(lblNgayChuyenCongTac.Content.ToString());
                    raddtNgayChuyenCongTac.Focus();
                    return false;
                }
                if (cmbPhongBanMoi.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblPhongBanMoi.Content.ToString());
                    cmbPhongBanMoi.Focus();
                    return false;
                }
                if (cmbChucVuMoi.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblChucVuMoi.Content.ToString());
                    cmbChucVuMoi.Focus();
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
                #region Thông tin chung
                txtNhanVien.IsEnabled = true;
                btnNhanVien.IsEnabled = true;
                txtTenNhanVien.IsEnabled = false;
                raddtNgayChuyenCongTac.IsEnabled = true;
                dtpNgayChuyenCongTac.IsEnabled = true;
                #endregion

                #region Thông tin quyết đinh
                txtSoQuyetDinh.IsEnabled = true;
                txtNguoiBanHanh.IsEnabled = true;
                raddtNgayBanHanh.IsEnabled = true;
                dtpNgayBanHanh.IsEnabled = true;
                #endregion

                #region Thông tin chuyển công tác
                txtMa.IsEnabled = false;
                cmbPhongBanCu.IsEnabled = false;
                cmbPhongBanMoi.IsEnabled = true;
                cmbChucVuCu.IsEnabled = false;
                cmbChucVuMoi.IsEnabled = true;
                txtDienGiai.IsEnabled = true;
                #endregion

            }
            #endregion

            #region Sửa
            else if (action == DatabaseConstant.Action.SUA)
            {
                #region Thông tin chung
                txtNhanVien.IsEnabled = true;
                btnNhanVien.IsEnabled = true;
                txtTenNhanVien.IsEnabled = false;
                raddtNgayChuyenCongTac.IsEnabled = true;
                dtpNgayChuyenCongTac.IsEnabled = true;
                #endregion

                #region Thông tin quyết đinh
                txtSoQuyetDinh.IsEnabled = true;
                txtNguoiBanHanh.IsEnabled = true;
                raddtNgayBanHanh.IsEnabled = true;
                dtpNgayBanHanh.IsEnabled = true;
                #endregion

                #region Thông tin chuyển công tác
                txtMa.IsEnabled = false;
                cmbPhongBanCu.IsEnabled = false;
                cmbPhongBanMoi.IsEnabled = true;
                cmbChucVuCu.IsEnabled = false;
                cmbChucVuMoi.IsEnabled = true;
                txtDienGiai.IsEnabled = true;
                #endregion
            }
            #endregion

            #region Xem ~ Mặc định
            else
            {
                #region Thông tin chung
                txtNhanVien.IsEnabled = false;
                btnNhanVien.IsEnabled = false;
                txtTenNhanVien.IsEnabled = false;
                raddtNgayChuyenCongTac.IsEnabled = false;
                dtpNgayChuyenCongTac.IsEnabled = false;
                #endregion

                #region Thông tin quyết đinh
                txtSoQuyetDinh.IsEnabled = false;
                txtNguoiBanHanh.IsEnabled = false;
                raddtNgayBanHanh.IsEnabled = false;
                dtpNgayBanHanh.IsEnabled = false;
                #endregion

                #region Thông tin chuyển công tác
                txtMa.IsEnabled = false;
                cmbPhongBanCu.IsEnabled = false;
                cmbPhongBanMoi.IsEnabled = false;
                cmbChucVuCu.IsEnabled = false;
                cmbChucVuMoi.IsEnabled = false;
                txtDienGiai.IsEnabled = false;
                #endregion
            }
            #endregion
        }


        public void OnHold()
        {
            List<NS_PHU_CAP> lstPhuCap = null;
            try
            {
                if (!Validation()) return;

                string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));
                obj = new NS_TCHUYEN_CTAC();
                //GetFormData(ref obj, ref lstPhuCap, ref lstTrinhDoHocVan, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    //OnAddNew(obj, lstPhuCap, lstTrinhDoHocVan);
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    //OnModify(obj, lstPhuCap, lstTrinhDoHocVan);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                lstPhuCap = null;                
            }
        }

        public void OnSave()
        {                    
            try
            {
                if (!Validation()) return;

                string trangThai = "";
                trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));

                obj = new NS_TCHUYEN_CTAC();

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
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain,function);
        }

        public void BeforeViewFromList()
        {
            SetFormData();
            BeforeViewFromDetail();
        }


        public void BeforeAddNew()
        {
            LoadFormData();
            action = DatabaseConstant.Action.THEM;
            ResetForm();
            SetEnabledControls();            
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain,function);
        }

        public void OnAddNew(NS_TCHUYEN_CTAC obj)
        {
            
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                NhanSuProcess processNhanSu = new NhanSuProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processNhanSu.ThuyenChuyenCongTac(DatabaseConstant.Action.THEM, ref obj, ref listClientResponseDetail);
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

        public void AfterAddNew(bool ret, NS_TCHUYEN_CTAC obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                    id = obj.ID;
                    txtMa.Text = obj.MA_TCHUYEN_CTAC;
                    sTrangThaiNVu = obj.TTHAI_NVU;              
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);

                    BeforeViewFromDetail();
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

                bool ret = process.LockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_THUYEN_CHUYEN_CT,
                    DatabaseConstant.Table.NS_TCHUYEN_CTAC,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                if (ret)
                {
                    action = DatabaseConstant.Action.SUA;
                    SetEnabledControls();                    
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
            action = DatabaseConstant.Action.SUA;
            SetEnabledControls();            
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnModify(NS_TCHUYEN_CTAC obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                NhanSuProcess processNhanSu = new NhanSuProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processNhanSu.ThuyenChuyenCongTac(DatabaseConstant.Action.SUA,ref obj, ref listClientResponseDetail);
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

        public void AfterModify(bool ret, NS_TCHUYEN_CTAC obj, List<ClientResponseDetail> listClientResponseDetail)
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_THUYEN_CHUYEN_CT,
                    DatabaseConstant.Table.NS_TCHUYEN_CTAC,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.NSTL,
                        DatabaseConstant.Function.NS_THUYEN_CHUYEN_CT,
                        DatabaseConstant.Table.NS_TCHUYEN_CTAC,
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_THUYEN_CHUYEN_CT,
                    DatabaseConstant.Table.NS_TCHUYEN_CTAC,
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
            NhanSuProcess processNhanSu = new NhanSuProcess();
            try
            {
                bool ret = false;                
                obj.ID = id;
                ret = processNhanSu.ThuyenChuyenCongTac(action, ref obj, ref listClientResponseDetail);
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
                processNhanSu = null;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_THUYEN_CHUYEN_CT,
                    DatabaseConstant.Table.NS_TCHUYEN_CTAC,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.NSTL,
                        DatabaseConstant.Function.NS_THUYEN_CHUYEN_CT,
                        DatabaseConstant.Table.NS_TCHUYEN_CTAC,
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
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnApprove()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            NhanSuProcess processNhanSu = new NhanSuProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processNhanSu.ThuyenChuyenCongTac(action, ref obj, ref listClientResponseDetail);
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
                listClientResponseDetail = null;
                processNhanSu = null;
            }
        }

        public void AfterApprove(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
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
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_THUYEN_CHUYEN_CT,
                    DatabaseConstant.Table.NS_TCHUYEN_CTAC,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.NSTL,
                        DatabaseConstant.Function.NS_THUYEN_CHUYEN_CT,
                        DatabaseConstant.Table.NS_TCHUYEN_CTAC,
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
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnCancel()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            NhanSuProcess processNhanSu = new NhanSuProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processNhanSu.ThuyenChuyenCongTac(action, ref obj, ref listClientResponseDetail);
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
                listClientResponseDetail = null;
                processNhanSu = null;
            }
        }

        public void AfterCancel(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
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
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_THUYEN_CHUYEN_CT,
                    DatabaseConstant.Table.NS_TCHUYEN_CTAC,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.NSTL,
                        DatabaseConstant.Function.NS_THUYEN_CHUYEN_CT,
                        DatabaseConstant.Table.NS_TCHUYEN_CTAC,
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
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnRefuse()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            NhanSuProcess processNhanSu = new NhanSuProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processNhanSu.ThuyenChuyenCongTac(action, ref obj, ref listClientResponseDetail);
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
                listClientResponseDetail = null;
                processNhanSu = null;
            }
        }

        public void AfterRefuse(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
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
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_THUYEN_CHUYEN_CT,
                    DatabaseConstant.Table.NS_TCHUYEN_CTAC,
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
