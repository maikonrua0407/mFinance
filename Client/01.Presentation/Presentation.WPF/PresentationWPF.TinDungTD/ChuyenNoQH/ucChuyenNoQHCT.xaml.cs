using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Collections;
using System.Reflection;
using System.Data;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using PresentationWPF.CustomControl;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.TinDungTDServiceRef;
using Presentation.Process.QuanTriHeThongServiceRef;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;


namespace PresentationWPF.TinDungTD.ChuyenNoQH
{

    public partial class ucChuyenNoQHCT : UserControl
    {

        #region Khai bao
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private DatabaseConstant.Function function = DatabaseConstant.Function.TDTD_CHUYEN_NO;

        public event EventHandler OnSavingCompleted;

        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceThoiHanVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhuongThucGiaiNgan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLaiSuatTrongHan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLaiSuatQuaHan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHinhThucTraGoc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHinhThucTraLai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTanSuatTraGoc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTanSuatTraLai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCachTinhSoNgay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhuongThucDieuChinhNgayTraNo = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNhomNo = new List<AutoCompleteEntry>();
        TDTD_CHUYEN_NO_QHAN obj = null;
        List<TTIN_CHUYEN_NO_QHAN_CHI_TIET> lstChuyenHoan = null;
        private string sTrangThaiNVu = "";

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        private string cso_TINH_LAI = "";

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
        public ucChuyenNoQHCT()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            LoadCombobox();

            ResetForm();

            InitEventHandler();
        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDungTD;component/KheUoc/ucKheUocCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void InitEventHandler()
        {
            btnThemKUoc.Click += new RoutedEventHandler(btnThemKUoc_Click);
            btnTinhToan.Click += new RoutedEventHandler(btnTinhToan_Click);
            chkTrichDuPhong.Unchecked += new RoutedEventHandler(chkTrichDuPhong_Unchecked);
            chkTrichDuPhong.Checked += new RoutedEventHandler(chkTrichDuPhong_Checked);
        }


        public ucChuyenNoQHCT(KIEM_SOAT _objKiemSoat) : this()
        {
            obj.ID = _objKiemSoat.ID;
            obj.MA_GDICH = _objKiemSoat.SO_GIAO_DICH;
            action = _objKiemSoat.action;
            SetFormData();
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TINH_TOAN)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.T, ModifierKeys.Control | ModifierKeys.Shift);
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
            //OnHold();
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
            //BeforeApprove();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //BeforeRefuse();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //BeforeCancel();
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
                //OnHold();
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
                //OnHold();
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

        /// <summary>
        /// Hàm xử lý giải phóng lock dữ liệu cho form
        /// </summary>
        private void Release()
        {
            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            if (obj != null && obj.ID != null)
            {
                listLockId.Add(obj.ID);

                bool ret = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_CHUYEN_NO,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.SUA,
                    listLockId);
            }
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

        void btnTinhToan_Click(object sender, RoutedEventArgs e)
        {
            OnCalculator();
        }

        void chkTrichDuPhong_Unchecked(object sender, RoutedEventArgs e)
        {
            btnTinhToan.IsEnabled = false;
            if (!lstChuyenHoan.IsNullOrEmpty())
                lstChuyenHoan.ForEach(f => { f.DU_PHONG_PHAI_TRICH = f.DU_PHONG_DA_TRICH; f.CHENH_LENH = 0; });
        }


        void chkTrichDuPhong_Checked(object sender, RoutedEventArgs e)
        {
            btnTinhToan.IsEnabled = true;
        }

        #region Load Combobox
        private void LoadCombobox()
        {

            COMBOBOX_DTO combo = null;
            List<string> lstDieuKien = new List<string>();
            List<COMBOBOX_DTO> lstCombobox = new List<COMBOBOX_DTO>();
            AutoComboBox auCombo = new AutoComboBox();

            try
            {

            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
        #endregion

        #region Xử lý Popup

        void btnThemKUoc_Click(object sender, RoutedEventArgs e)
        {
            if (lstChuyenHoan.IsNullOrEmpty())
                return;
            lstPopup.Clear();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
            lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
            PopupProcess popupProcess = new PopupProcess();
            if(ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BIDV_BLF.ToString()))
                popupProcess.getPopupInformation("POPUP_DSACH_KUOC_CHUYEN_QUA_HAN_TDTD_LMF", lstDieuKien);
            else
                popupProcess.getPopupInformation("POPUP_DSACH_KUOC_CHUYEN_QUA_HAN_TDTD", lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            ucPopup popup = new ucPopup(false, simplePopupResponse, true);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
            win.Content = popup;
            win.ShowDialog();
            TTIN_CHUYEN_NO_QHAN_CHI_TIET objTTinCT = null;
            if (lstPopup.Count > 0)
            {
                List<int> lstID = lstChuyenHoan.Select(f => f.ID_KUOC).ToList();
                foreach (DataRow dr in lstPopup)
                {
                    if (lstID.Contains(Convert.ToInt32(dr["ID_KUOC"])))
                    {
                        continue;
                    }
                    objTTinCT = new TTIN_CHUYEN_NO_QHAN_CHI_TIET();
                    objTTinCT.CO_TRICH_DPHONG = BusinessConstant.CoKhong.KHONG.layGiaTri();
                    objTTinCT.DU_PHONG_DA_TRICH = Convert.ToDecimal(dr["SO_TIEN_TLDP"]);
                    objTTinCT.SO_DU_PHONG_NHOM_CU = Convert.ToDecimal(dr["DU_PHONG_NHOM_CU"]);
                    objTTinCT.DU_PHONG_PHAI_TRICH = Convert.ToDecimal(dr["SO_TIEN_TLDP"]);
                    objTTinCT.DU_THU = Convert.ToDecimal(dr["DU_THU"]);
                    objTTinCT.GOC_QH = Convert.ToDecimal(dr["GOC_QH"]);
                    objTTinCT.LAI_QH = Convert.ToDecimal(dr["LAI_QH"]);
                    objTTinCT.ID_KHANG = Convert.ToInt32(dr["ID_KHANG"]);
                    objTTinCT.ID_KUOC = Convert.ToInt32(dr["ID_KUOC"]);
                    objTTinCT.ID_DON_VI = Convert.ToInt32(dr["ID_DON_VI"]);
                    objTTinCT.NHOM_CU_DU_PHONG = Convert.ToString(dr["NHOM_NO_CU"]);
                    objTTinCT.MA_DVI = Convert.ToString(dr["MA_DVI"]);
                    objTTinCT.MA_KHANG = Convert.ToString(dr["MA_KHANG"]);
                    objTTinCT.MA_SAN_PHAM = Convert.ToString(dr["MA_SAN_PHAM"]);
                    objTTinCT.NGAY_DAO_HAN = Convert.ToString(dr["NGAY_DAO_HAN"]);
                    objTTinCT.NGAY_VAY = Convert.ToString(dr["NGAY_VAY"]);
                    objTTinCT.NHOM_NO_CU = Convert.ToString(dr["NHOM_NO_HIEN_TAI"]);
                    objTTinCT.NHOM_NO_MOI = Convert.ToString(dr["NHOM_NO_HIEN_TAI"]);
                    objTTinCT.NV_LOAI_NVON = Convert.ToString(dr["NV_LOAI_NVON"]);
                    //objTTinCT.PLOAI_NO = Convert.ToString(dr["PLOAI_NO"]);
                    objTTinCT.SO_DU = Convert.ToDecimal(dr["SO_DU"]);
                    objTTinCT.SO_KHE_UOC = Convert.ToString(dr["SO_KUOC"]);
                    objTTinCT.MA_KUOC = Convert.ToString(dr["MA_KUOC"]);
                    objTTinCT.SO_NGAY_QHAN = Convert.ToInt32(dr["SO_NGAY_QHAN"]);
                    objTTinCT.SO_TIEN_VAY = Convert.ToDecimal(dr["SO_TIEN_VAY"]);
                    objTTinCT.TEN_KHANG = Convert.ToString(dr["TEN_KHANG"]);
                    objTTinCT.THAN_VAY = Convert.ToInt32(dr["THAN_VAY"]);
                    objTTinCT.THAN_VAY_DVT = Convert.ToString(dr["THAN_VAY_DVT"]);
                    objTTinCT.SO_HDTD = Convert.ToString(dr["SO_HDTD"]);
                    objTTinCT.NGAY_QUA_HAN = Convert.ToString(dr["NGAY_QUA_HAN"]);
                    objTTinCT.SO_CMND = Convert.ToString(dr["SO_CMND"]);
                    objTTinCT.LOAI_TIEN = Convert.ToString(dr["LOAI_TIEN"]);
                    lstChuyenHoan.Add(objTTinCT);
                    raddgrDSKheUoc.Rebind();
                }
            }
        }
        #endregion
        #endregion

        #region Xử lý nghiệp vụ
        private void GetFormData(string sTrangThaiNVu)
        {
            try
            {
                string CoTrichDuPhong = BusinessConstant.CoKhong.KHONG.layGiaTri();
                if (obj.ID > 0)
                {
                    obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                }
                obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.TTHAI_NVU = sTrangThaiNVu;
                if (chkTrichDuPhong.IsChecked.GetValueOrDefault())
                {
                    CoTrichDuPhong = BusinessConstant.CoKhong.CO.layGiaTri();
                }
                lstChuyenHoan.ForEach(f => f.CO_TRICH_DPHONG = CoTrichDuPhong);
                obj.DSACH_CHUYEN_NO_QHAN = lstChuyenHoan.ToArray();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TinDungTDProcess processTinDungTD = new TinDungTDProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                int ret = 0;
                ret = processTinDungTD.ChuyenNoQuaHan(DatabaseConstant.Action.LOAD_DATA, ref obj, ref listClientResponseDetail);
                if (ret > 0)
                {
                    lstChuyenHoan = obj.DSACH_CHUYEN_NO_QHAN.ToList();
                    raddgrDSKheUoc.ItemsSource = lstChuyenHoan;
                    grMain.DataContext = obj;
                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    if (!obj.DSACH_CHUYEN_NO_QHAN.Where(f => f.CHENH_LENH != 0).IsNullOrEmpty() && obj.DSACH_CHUYEN_NO_QHAN.Where(f => f.CHENH_LENH != 0).Count() > 0)
                    {
                        chkTrichDuPhong.IsChecked = true;
                    }
                    else
                        chkTrichDuPhong.IsChecked = false;
                    CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, DatabaseConstant.Function.TDTD_THU_GOC_LAI);
                    if (action.Equals(DatabaseConstant.Action.SUA))
                        SetEnabledControls(true);
                    else
                        SetEnabledControls(false);
                    SetTabThongKiemSoat();
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
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        void SetTabThongKiemSoat()
        {
            try
            {
                if (!LObject.IsNullOrEmpty(obj))
                {
                    txtNguoiLap.Text = obj.NGUOI_NHAP;
                    teldtNgayNhap.Value = LDateTime.StringToDate(obj.NGAY_NHAP, ApplicationConstant.defaultDateTimeFormat);
                    txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    txtNguoiCapNhat.Text = obj.NGUOI_CNHAT;
                    if (!obj.NGAY_CNHAT.IsNullOrEmptyOrSpace())
                        teldtNgayCNhat.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, ApplicationConstant.defaultDateTimeFormat);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void ResetForm()
        {
            obj = new TDTD_CHUYEN_NO_QHAN();
            lstChuyenHoan = new List<TTIN_CHUYEN_NO_QHAN_CHI_TIET>();
            obj.NGAY_GDICH = ClientInformation.NgayLamViecHienTai;
            obj.MA_DVI_QLY = ClientInformation.MaDonVi;
            obj.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
            obj.LOAI_TIEN = ClientInformation.MaDongNoiTe;
            obj.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
            obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
            obj.TEN_NGUOI_GDICH = ClientInformation.HoTen;
            grMain.DataContext = obj;
            raddgrDSKheUoc.ItemsSource = lstChuyenHoan;
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            txtNguoiCapNhat.Text = "";
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            teldtNgayCNhat.Value = null;
            txtTrangThai.Text = "";
            sTrangThaiNVu = "";
            lblTrangThai.Content = "";
            chkTrichDuPhong.IsChecked = false;
            btnTinhToan.IsEnabled = false;
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, "", mnuMain, DatabaseConstant.Function.TDTD_CHUYEN_NO);

        }

        private bool Validation()
        {
            bool kq = true;
            List<ClientResponseDetail> listClientResponseDetail = null;
            ClientResponseDetail objClientResponseDetail = null;
            try
            {
                if (lstChuyenHoan.Count <= 0)
                {
                    CommonFunction.ThongBaoTrong(lblDanhSachTVien.Content.ToString());
                    btnThemKUoc.Focus();
                    return false;
                }
                else if (txtDienGiai.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblDienGiai.Content.ToString());
                    btnThemKUoc.Focus();
                    return false;
                }
                if (!lstChuyenHoan.Where(f => f.NHOM_NO_CU == f.NHOM_NO_MOI).IsNullOrEmpty() && lstChuyenHoan.Where(f => f.NHOM_NO_CU == f.NHOM_NO_MOI).Count() > 0)
                {
                    if (listClientResponseDetail.IsNullOrEmpty()) listClientResponseDetail = new List<ClientResponseDetail>();
                    foreach (TTIN_CHUYEN_NO_QHAN_CHI_TIET objChuyenNo in lstChuyenHoan.Where(f => f.NHOM_NO_CU == f.NHOM_NO_MOI))
                    {
                        objClientResponseDetail = new ClientResponseDetail();
                        objClientResponseDetail.Detail = LLanguage.SearchResourceByKey("M_ResponseMessage_NhomNo_BangNhomNoHienTai");
                        objClientResponseDetail.Id = objChuyenNo.ID_KUOC;
                        objClientResponseDetail.Object = objChuyenNo.MA_KUOC;
                        objClientResponseDetail.Operation = DatabaseConstant.Action.KIEM_TRA.layNgonNgu();
                        objClientResponseDetail.Result = ApplicationConstant.OperationStatus.Failed.layGiaTri();
                        listClientResponseDetail.Add(objClientResponseDetail);
                    }
                    
                    kq = false;
                }
                if (!lstChuyenHoan.Where(f => f.NHOM_NO_CU.Substring(f.NHOM_NO_CU.Length - 1).CompareTo(f.NHOM_NO_MOI.Substring(f.NHOM_NO_CU.Length - 1)) > 0).IsNullOrEmpty() && lstChuyenHoan.Where(f => f.NHOM_NO_CU.Substring(f.NHOM_NO_CU.Length - 1).CompareTo(f.NHOM_NO_MOI.Substring(f.NHOM_NO_CU.Length - 1)) > 0).Count() > 0)
                {
                    if (listClientResponseDetail.IsNullOrEmpty()) listClientResponseDetail = new List<ClientResponseDetail>();
                    foreach (TTIN_CHUYEN_NO_QHAN_CHI_TIET objChuyenNo in lstChuyenHoan.Where(f => f.NHOM_NO_CU.Substring(f.NHOM_NO_CU.Length - 1).CompareTo(f.NHOM_NO_MOI.Substring(f.NHOM_NO_CU.Length - 1)) >= 0))
                    {
                        objClientResponseDetail = new ClientResponseDetail();
                        objClientResponseDetail.Detail = LLanguage.SearchResourceByKey("M_ResponseMessage_NhomNo_NhoHonNhomNoHienTai");
                        objClientResponseDetail.Id = objChuyenNo.ID_KUOC;
                        objClientResponseDetail.Object = objChuyenNo.MA_KUOC;
                        objClientResponseDetail.Operation = DatabaseConstant.Action.KIEM_TRA.layNgonNgu();
                        objClientResponseDetail.Result = ApplicationConstant.OperationStatus.Failed.layGiaTri();
                        listClientResponseDetail.Add(objClientResponseDetail);
                    }
                    kq = false;
                }
                if (!kq)
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    raddgrDSKheUoc.Focus();
                    return kq;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                kq = false;
            }
            return kq;
        }

        private void SetEnabledControls(bool isEnable)
        {
            grbThongTinChung.IsEnabled = isEnable;
            btnThemKUoc.IsEnabled = isEnable;
            btnXoaKUoc.IsEnabled = isEnable;
            raddgrDSKheUoc.IsReadOnly = !isEnable;
            chkTrichDuPhong.IsEnabled = isEnable;
            if (isEnable && chkTrichDuPhong.IsChecked.GetValueOrDefault())
                btnTinhToan.IsEnabled = true;
            else
                btnTinhToan.IsEnabled = false;
        }

        public void OnHold()
        {
            try
            {
                if (!Validation()) return;

                string trangThai = "";
                if (sTrangThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                else
                    trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));

                GetFormData(trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew();
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify();
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

                string trangThai = "";
                if (sTrangThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                else
                    trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));

                GetFormData(trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew();
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify();
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
            SetEnabledControls(false);
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
            grMain.DataContext = obj;
            SetTabThongKiemSoat();
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
            SetEnabledControls(true);
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnAddNew()
        {

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TinDungTDProcess processTinDungTD = new TinDungTDProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                int ret = 0;

                ret = processTinDungTD.ChuyenNoQuaHan(DatabaseConstant.Action.THEM, ref obj, ref listClientResponseDetail);
                AfterAddNew(ret, listClientResponseDetail);
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

        public void AfterAddNew(int ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret > 0)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                    if (cbMultiAdd.IsChecked == true)
                    {
                        ResetForm();
                    }
                    else
                    {
                        sTrangThaiNVu = obj.TTHAI_NVU;
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
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
                listLockId.Add(obj.ID);

                bool ret = process.LockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_CHUYEN_NO,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                if (ret)
                {
                    action = DatabaseConstant.Action.SUA;
                    SetEnabledControls(true);
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
            SetEnabledControls(true);
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnModify()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TinDungTDProcess processTinDungTD = new TinDungTDProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                int ret = 0;

                ret = processTinDungTD.ChuyenNoQuaHan(DatabaseConstant.Action.SUA, ref obj, ref listClientResponseDetail);
                AfterModify(ret, listClientResponseDetail);
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

        public void AfterModify(int ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret > 0)
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

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
                listLockId.Add(obj.ID);

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_CHUYEN_NO,
                    DatabaseConstant.Table.KT_GIAO_DICH,
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
                    listLockId.Add(obj.ID);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.TDTD,
                        DatabaseConstant.Function.TDTD_CHUYEN_NO,
                        DatabaseConstant.Table.KT_GIAO_DICH,
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
                listLockId.Add(obj.ID);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_CHUYEN_NO,
                    DatabaseConstant.Table.KT_GIAO_DICH,
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
            TinDungTDProcess processTinDungTD = new TinDungTDProcess();
            try
            {
                int ret = 0;
                ret = processTinDungTD.ChuyenNoQuaHan(action, ref obj, ref listClientResponseDetail);
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
                processTinDungTD = null;
            }
        }

        public void AfterDelete(int ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret > 0)
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
                listLockId.Add(obj.ID);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_CHUYEN_NO,
                    DatabaseConstant.Table.KT_GIAO_DICH,
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
                    listLockId.Add(obj.ID);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.TDTD,
                        DatabaseConstant.Function.TDTD_CHUYEN_NO,
                        DatabaseConstant.Table.KT_GIAO_DICH,
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
            TinDungTDProcess processTinDungTD = new TinDungTDProcess();
            try
            {
                int ret = 0;
                ret = processTinDungTD.ChuyenNoQuaHan(action, ref obj, ref listClientResponseDetail);
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
                processTinDungTD = null;
            }
        }

        public void AfterApprove(int ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret > 0)
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
                listLockId.Add(obj.ID);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_CHUYEN_NO,
                    DatabaseConstant.Table.KT_GIAO_DICH,
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
                    listLockId.Add(obj.ID);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.TDTD,
                        DatabaseConstant.Function.TDTD_CHUYEN_NO,
                        DatabaseConstant.Table.KT_GIAO_DICH,
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
            TinDungTDProcess processTinDungTD = new TinDungTDProcess();
            try
            {
                int ret = 0;
                ret = processTinDungTD.ChuyenNoQuaHan(action, ref obj, ref listClientResponseDetail);
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
                processTinDungTD = null;
            }
        }

        public void AfterCancel(int ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret > 0)
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
                listLockId.Add(obj.ID);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_CHUYEN_NO,
                    DatabaseConstant.Table.KT_GIAO_DICH,
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
                    listLockId.Add(obj.ID);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.TDTD,
                        DatabaseConstant.Function.TDTD_CHUYEN_NO,
                        DatabaseConstant.Table.KT_GIAO_DICH,
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
            TinDungTDProcess processTinDungTD = new TinDungTDProcess();
            try
            {
                int ret = 0;
                ret = processTinDungTD.ChuyenNoQuaHan(action, ref obj, ref listClientResponseDetail);
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
                processTinDungTD = null;
            }
        }

        public void AfterRefuse(int ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret > 0)
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
                listLockId.Add(obj.ID);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_CHUYEN_NO,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void OnPreview()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(obj.MA_GDICH))
            {
                LMessage.ShowMessage("Không có thông tin cần xử lý", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();

                GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.GDKT_IN_GIAO_DICH;
                objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDTD_THU_GOC_LAI;

                GDKT_GIAO_DICH objGDKT_GIAO_DICH = new GDKT_GIAO_DICH();
                objGDKT_GIAO_DICH.MaGiaoDich = txtSoGiaoDich.Text;

                doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                doiTuongBaoCao.objGDKT_GIAO_DICH = objGDKT_GIAO_DICH;

                List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
                listThamSoBaoCao.Add(new ThamSoBaoCao("@SoPhieu", txtSoGiaoDich.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                string maBaoCao = "GDKT_GIAO_DICH";
                xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
            }
        }

        public void OnCalculator()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TinDungTDProcess processTinDungTD = new TinDungTDProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                int ret = 0;
                obj.DSACH_CHUYEN_NO_QHAN = lstChuyenHoan.ToArray();
                ret = processTinDungTD.ChuyenNoQuaHan(DatabaseConstant.Action.TINH_TOAN_TRICH_LAP_DU_PHONG_CU_THE, ref obj, ref listClientResponseDetail);
                if (ret > 0)
                {
                    lstChuyenHoan = obj.DSACH_CHUYEN_NO_QHAN.ToList();
                    raddgrDSKheUoc.ItemsSource = lstChuyenHoan;
                    grMain.DataContext = obj;
                    LMessage.ShowMessage("M.DungChung.Result.ThanhCong", LMessage.MessageBoxType.Information);
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
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }
        #endregion

    }
}
