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
using Presentation.Process;
using Utilities.Common;
using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process.Common;
using PresentationWPF.CustomControl;
using Presentation.Process.TinDungTDServiceRef;
using PresentationWPF.TinDungTD.KheUoc;
using System.Data;
using Presentation.Process.PopupServiceRef;
using PresentationWPF.TinDungTD.ThuocTinh;
using Telerik.Windows.Controls;

namespace PresentationWPF.TinDungTD.KiemSoatRuiRo
{
    /// <summary>
    /// Interaction logic for ucKiemSoatRuiRo.xaml
    /// </summary>
    public partial class ucKiemSoatRuiRoCT : UserControl
    {
        #region Khai bao
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private string sTrangThaiNVu = "";

        private DatabaseConstant.Function function = DatabaseConstant.Function.TDTD_KIEM_SOAT_RR;

        public event EventHandler OnSavingCompleted;

        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand SubmitCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        private TD_KIEM_SOAT_RR _obj = new TD_KIEM_SOAT_RR();
        public TD_KIEM_SOAT_RR obj
        {
            get { return _obj; }
            set { _obj = value; }
        }

        private TD_KIEM_SOAT_RR_EXT _objExt = new TD_KIEM_SOAT_RR_EXT();
        public TD_KIEM_SOAT_RR_EXT objExt
        {
            get { return _objExt; }
            set { _objExt = value; }
        }

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        private string loaiDon;
        #endregion

        #region Khoi tao
        public ucKiemSoatRuiRoCT()
        {
            InitializeComponent();
            BindShortkey();
        }
        #endregion

        #region Dang ky hotkey, shortcut key
        private void BindShortkey()
        {
            foreach (var child in Toolbar.Children)
            {
                if (child.GetType() == typeof(RibbonButton))
                {
                    RibbonButton tlb = (RibbonButton)child;
                    KeyBinding key = new KeyBinding();
                    string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
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

        private void SubmitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SubmitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
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
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetForm();
                SetEnabledControls(true);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
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

            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetForm();
                SetEnabledControls(true);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
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
                //OnPreview();
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

        #region Xu ly giao dien
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
            //if (_obj != null && _obj.objKuoc != null)
            //{
            //    listLockId.Add(_obj.objKuoc.ID);

            //    bool ret = process.UnlockData(DatabaseConstant.Module.TDTD,
            //        DatabaseConstant.Function.TDTD_KHE_UOC,
            //        DatabaseConstant.Table.TDTD_KUOC,
            //        DatabaseConstant.Action.SUA,
            //        listLockId);
            //}
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
        #endregion

        #region Xu ly nghiep vu
        private void GetFormData(string sTrangThaiNVu)
        {
            try
            {
                if (_obj == null)
                {
                    _obj = new TD_KIEM_SOAT_RR();
                }

                _obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                _obj.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                _obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                _obj.NGUOI_NHAP = txtNguoiLap.Text;
                _obj.NGAY_NHAP = Convert.ToDateTime(teldtNgayNhap.Value).ToString("yyyyMMdd");
                _obj.TTHAI_NVU = sTrangThaiNVu;
                if (action != DatabaseConstant.Action.THEM)
                {
                    _obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    _obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }

                _obj.MA_PHAN_HE = DatabaseConstant.Module.TDTD.getValue();
                _obj.DIEN_GIAI = txtDienGiai.Text.Trim();
                _obj.SO_KIEM_SOAT = txtSoKiemSoat.Text.Trim();
                _obj.SO_HDTD = txtSoHDTD.Text.Trim();
                _obj.SO_KUOC = txtSoKuoc.Text.Trim();
                _obj.NGAY_HOP_DONG = LDateTime.DateToString(teldtNgayHDTD.Value.Value, "yyyyMMdd");
                _obj.NGAY_KUOC = LDateTime.DateToString(teldtNgayKuoc.Value.Value, "yyyyMMdd");
                _obj.NGAY_KIEM_SOAT = LDateTime.DateToString(teldtNgayKiemSoat.Value.Value, "yyyyMMdd");
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            TinDungTDProcess processTinDungTD = new TinDungTDProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                int ret = 1;
                txtSoKiemSoat.Text = _obj.SO_KIEM_SOAT;
                teldtNgayKiemSoat.Value = LDateTime.StringToDate(_obj.NGAY_KIEM_SOAT, "yyyyMMdd");
                txtSoKuoc.Text = _obj.SO_KUOC;
                teldtNgayKuoc.Value = LDateTime.StringToDate(_obj.NGAY_KUOC, "yyyyMMdd");
                txtSoHDTD.Text = _obj.SO_HDTD;
                teldtNgayHDTD.Value = LDateTime.StringToDate(_obj.NGAY_HOP_DONG, "yyyyMMdd");

                txtKhachHang.Text = objExt.MA_KHANG + " - " + objExt.TEN_KHANG;
                if (objExt.NGAY_SINH != null)
                    teldtNgaySinh.Value = LDateTime.StringToDate(objExt.NGAY_SINH, "yyyyMMdd");
                txtCMND.Text = objExt.GTLQ_SO;
                if (objExt.GTLQ_NGAY_CAP != null)
                    teldtNgayCapCMND.Value = LDateTime.StringToDate(objExt.GTLQ_NGAY_CAP, "yyyyMMdd");

                if (ret > 0)
                {
                    sTrangThaiNVu = _obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    txtSoKiemSoat.Text = _obj.SO_KIEM_SOAT;
                    teldtNgayKiemSoat.Value = LDateTime.StringToDate(_obj.NGAY_KIEM_SOAT, "yyyyMMdd");
                    txtSoKuoc.Text = _obj.SO_KUOC;
                    teldtNgayKuoc.Value = LDateTime.StringToDate(_obj.NGAY_KUOC, "yyyyMMdd");
                    txtSoHDTD.Text = _obj.SO_HDTD;
                    teldtNgayHDTD.Value = LDateTime.StringToDate(_obj.NGAY_HOP_DONG, "yyyyMMdd");
                    txtDienGiai.Text = _obj.DIEN_GIAI;
                    sTrangThaiNVu = _obj.TTHAI_NVU;

                    txtKhachHang.Text = objExt.MA_KHANG + " - " + objExt.TEN_KHANG;
                    if (objExt.NGAY_SINH != null)
                        teldtNgaySinh.Value = LDateTime.StringToDate(objExt.NGAY_SINH, "yyyyMMdd");
                    txtCMND.Text = objExt.GTLQ_SO;
                    if (objExt.GTLQ_NGAY_CAP != null)
                        teldtNgayCapCMND.Value = LDateTime.StringToDate(objExt.GTLQ_NGAY_CAP, "yyyyMMdd");
                    DataTable dt = null;
                    LDatatable.MakeParameterTable(ref dt);
                    LDatatable.AddParameter(ref dt, "@INQ_MA_KIEM_SOAT_RR", "String", _obj.SO_KIEM_SOAT);
                    LDatatable.AddParameter(ref dt, "@INQ_ID_KIEM_SOAT_RR", "String", _obj.ID.ToString());
                    DataSet ds = new TinDungTDProcess().GetThongTinKiemSoatRuiRo(dt);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables["TTIN_MA_XAC_NHAN"] != null)
                    {
                        DataTable serverDataTable = ds.Tables["TTIN_MA_XAC_NHAN"];
                        raddgrDSRutTK.ItemsSource = serverDataTable.DefaultView;
                    }
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
            action = DatabaseConstant.Action.THEM;
            _obj = new TD_KIEM_SOAT_RR();
            txtSoKiemSoat.Text = "";
            teldtNgayKiemSoat.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai,"yyyyMMdd");
            txtSoKuoc.Text = "";
            teldtNgayKuoc.Value = null;
            txtSoHDTD.Text = "";
            teldtNgayHDTD.Value = null;
            txtDienGiai.Text = "";

            #region Thông tin kiểm soát
            txtTrangThai.Text = "";
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            teldtNgayCNhat.Value = null;
            txtNguoiCapNhat.Text = "";
            #endregion

            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, "", mnuMain, DatabaseConstant.Function.TDTD_KHE_UOC);

        }

        private bool Validation()
        {
            try
            {
                if (txtSoKuoc.Text.Trim().IsNullOrEmpty())
                {
                    CommonFunction.ThongBaoChuaNhap(lblSoKuoc.Content.ToString());
                    txtSoKuoc.Focus();
                    return false;
                }

                if (txtDienGiai.Text.IsNullOrEmptyOrSpace() && ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BIDV_BLF.ToString()))
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

        private void SetEnabledControls(bool isEnable)
        {
            grbThongTinChung.IsEnabled = isEnable;
            grbThongTinKiemSoat.IsEnabled = isEnable;
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
                List<TD_KIEM_SOAT_RR> lst = new List<TD_KIEM_SOAT_RR>();
                lst.Add(_obj);
                ret = processTinDungTD.KiemSoatRuiRo(DatabaseConstant.Action.THEM, ref lst, ref listClientResponseDetail);
                _obj = lst[0];
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
                        sTrangThaiNVu = _obj.TTHAI_NVU;
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        txtSoKiemSoat.Text = _obj.SO_KIEM_SOAT;
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
                listLockId.Add(_obj.ID);

                bool ret = process.LockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_KIEM_SOAT_RR,
                    DatabaseConstant.Table.TD_KIEM_SOAT_RR,
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
                List<TD_KIEM_SOAT_RR> lst = new List<TD_KIEM_SOAT_RR>();
                lst.Add(_obj);
                ret = processTinDungTD.KiemSoatRuiRo(DatabaseConstant.Action.SUA, ref lst, ref listClientResponseDetail);
                _obj = lst[0];
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

                    sTrangThaiNVu = _obj.TTHAI_NVU;
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
                listLockId.Add(_obj.ID);

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_KIEM_SOAT_RR,
                    DatabaseConstant.Table.TD_KIEM_SOAT_RR,
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
                    listLockId.Add(_obj.ID);

                    bool retLockData = process.LockData(DatabaseConstant.Module.TDTD,
                        DatabaseConstant.Function.TDTD_KIEM_SOAT_RR,
                        DatabaseConstant.Table.TD_KIEM_SOAT_RR,
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
                listLockId.Add(_obj.ID);

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_KIEM_SOAT_RR,
                    DatabaseConstant.Table.TD_KIEM_SOAT_RR,
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
                List<TD_KIEM_SOAT_RR> lst = new List<TD_KIEM_SOAT_RR>();
                lst.Add(_obj);
                ret = processTinDungTD.KiemSoatRuiRo(action, ref lst, ref listClientResponseDetail);
                _obj = lst[0];
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
                listLockId.Add(_obj.ID);

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_KIEM_SOAT_RR,
                    DatabaseConstant.Table.TD_KIEM_SOAT_RR,
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
                    listLockId.Add(_obj.ID);
                    bool retLockData = process.LockData(DatabaseConstant.Module.TDTD,
                        DatabaseConstant.Function.TDTD_KIEM_SOAT_RR,
                        DatabaseConstant.Table.TD_KIEM_SOAT_RR,
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
                List<TD_KIEM_SOAT_RR> lst = new List<TD_KIEM_SOAT_RR>();
                lst.Add(_obj);
                ret = processTinDungTD.KiemSoatRuiRo(action, ref lst, ref listClientResponseDetail);
                _obj = lst[0];
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
                listLockId.Add(_obj.ID);

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_KIEM_SOAT_RR,
                    DatabaseConstant.Table.TD_KIEM_SOAT_RR,
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
                    listLockId.Add(_obj.ID);

                    bool retLockData = process.LockData(DatabaseConstant.Module.TDTD,
                        DatabaseConstant.Function.TDTD_KIEM_SOAT_RR,
                        DatabaseConstant.Table.TD_KIEM_SOAT_RR,
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
                List<TD_KIEM_SOAT_RR> lst = new List<TD_KIEM_SOAT_RR>();
                lst.Add(_obj);
                ret = processTinDungTD.KiemSoatRuiRo(action, ref lst, ref listClientResponseDetail);
                _obj = lst[0];
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
                listLockId.Add(_obj.ID);

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_KIEM_SOAT_RR,
                    DatabaseConstant.Table.TD_KIEM_SOAT_RR,
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
                    listLockId.Add(_obj.ID);

                    bool retLockData = process.LockData(DatabaseConstant.Module.TDTD,
                        DatabaseConstant.Function.TDTD_KIEM_SOAT_RR,
                        DatabaseConstant.Table.TD_KIEM_SOAT_RR,
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
                List<TD_KIEM_SOAT_RR> lst = new List<TD_KIEM_SOAT_RR>();
                lst.Add(_obj);
                ret = processTinDungTD.KiemSoatRuiRo(action, ref lst, ref listClientResponseDetail);
                _obj = lst[0];
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
                listLockId.Add(_obj.ID);

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_KIEM_SOAT_RR,
                    DatabaseConstant.Table.TD_KIEM_SOAT_RR,
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

        private void txtSoKuoc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnSoKuoc_Click(null, null);
            }
        }

        private void btnSoKuoc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_KUOC_RUI_RO", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup.Count > 0)
                {
                    foreach (DataRow dr in lstPopup)
                    {
                        if (_obj == null)
                        {
                            _obj = new TD_KIEM_SOAT_RR();
                        }

                        _obj.ID_HDTD = Convert.ToInt32(dr["ID_HDTD"]);
                        _obj.MA_HDTD = dr["MA_HDTD"].ToString();
                        _obj.SO_HDTD = dr["SO_HDTD"].ToString();
                        _obj.NGAY_KUOC = dr["NGAY_GIAI_NGAN"].ToString();
                        
                        _obj.ID_KUOC = Convert.ToInt32(dr["ID_KUOC"]);
                        _obj.MA_KUOC = dr["MA_KUOC"].ToString();
                        _obj.SO_KUOC = dr["SO_KUOC"].ToString();
                        _obj.NGAY_HOP_DONG = dr["NGAY_HD"].ToString();

                        txtSoKuoc.Text = _obj.SO_KUOC;
                        teldtNgayKuoc.Value = LDateTime.StringToDate(_obj.NGAY_KUOC, "yyyyMMdd");
                        txtSoHDTD.Text = _obj.SO_HDTD;
                        teldtNgayHDTD.Value = LDateTime.StringToDate(_obj.NGAY_HOP_DONG, "yyyyMMdd");
                        loaiDon = dr["LOAI_DXVV"].ToString();
                        txtKhachHang.Text = dr["MA_KHANG"].ToString() + " - " + dr["TEN_KHANG"].ToString();
                        if (dr["DD_NGAY_SINH"] != null)
                            if (dr["DD_NGAY_SINH"].ToString() != null && !dr["DD_NGAY_SINH"].ToString().Equals(""))
                                teldtNgaySinh.Value = LDateTime.StringToDate(dr["DD_NGAY_SINH"].ToString(), "yyyyMMdd");
                        txtCMND.Text = dr["DD_GTLQ_SO"].ToString();
                        if (dr["DD_GTLQ_NGAY_CAP"] != null)
                            if (dr["DD_GTLQ_NGAY_CAP"].ToString() != null && !dr["DD_GTLQ_NGAY_CAP"].ToString().Equals(""))
                                teldtNgayCapCMND.Value = LDateTime.StringToDate(dr["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd");
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void btnViewKuoc_Click(object sender, RoutedEventArgs e)
        {
            Window win = new Window();
            ucKheUocCT uc = new ucKheUocCT();
            uc.obj = new TDTD_KHE_UOC();
            uc.obj.objKuoc = new THONG_TIN_KHE_UOC();
            uc.obj.objKuoc.ID = _obj.ID_KUOC;
            uc.obj.objKuoc.MA_KHE_UOC = _obj.MA_KUOC;
            uc.obj.objKuoc.SO_KHE_UOC = _obj.SO_KUOC;
        }

        private void btnViewHDTD_Click(object sender, RoutedEventArgs e)
        {
            ucHopDongTinDungCaNhanCT uc = new ucHopDongTinDungCaNhanCT();
        }

        private void ViewKuoc()
        {
            ucKheUocCT userControl = new ucKheUocCT();

            userControl.Action = DatabaseConstant.Action.XEM;
            userControl.obj = new TDTD_KHE_UOC();
            userControl.obj.objKuoc = new THONG_TIN_KHE_UOC();
            userControl.obj.objKuoc.ID = obj.ID_KUOC;

            Window window = new Window();
            window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.TDTD_KHE_UOC);
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Content = userControl;
            window.ShowDialog();
        }

        private void ViewHDTD()
        {
            ucHopDongTinDungCaNhanCT objHDTDThoaThuan = new ucHopDongTinDungCaNhanCT();
            objHDTDThoaThuan.action = DatabaseConstant.Action.XEM;
            objHDTDThoaThuan.SetDataForm(obj.MA_HDTD);
            string tittle = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN);
            Window window = new Window();
            window.Title = tittle;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Content = objHDTDThoaThuan;
            window.ShowDialog();
        }

        private void btnViewDoiTuong_Click(object sender, RoutedEventArgs e)
        {
            string doiTuong = ((RadComboBoxItem)cboDoiTuong.SelectedItem).Tag.ToString();
            if (doiTuong.Equals("HD"))
            {
                if (obj.MA_HDTD != null)
                    ViewHDTD();
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.KhongTonTaiDuLieu", LMessage.MessageBoxType.Information);
                    return;
                }
            }
            else if (doiTuong.Equals("KU"))
            {
                if (obj.ID_KUOC > 0)
                    ViewKuoc();
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.KhongTonTaiDuLieu", LMessage.MessageBoxType.Information);
                    return;
                }

            }
            else
            {
            }
        }

        private void txtSoKuoc_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtSoKuoc.Text.Trim().IsNullOrEmptyOrSpace())
                {
                    TinDungTDProcess process = new TinDungTDProcess();
                    DataTable dt = null;
                    LDatatable.AddParameter(ref dt, "@INP_SO_KUOC", "String", txtSoKuoc.Text.Trim());

                    DataSet ds = process.GetThongTinKheUoc(dt);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
                    {
                        if (_obj == null)
                        {
                            _obj = new TD_KIEM_SOAT_RR();
                        }

                        _obj.ID_HDTD = Convert.ToInt32(ds.Tables[0].Rows[0]["ID_HDTD"]);
                        _obj.MA_HDTD = ds.Tables[0].Rows[0]["MA_HDTD"].ToString();
                        _obj.SO_HDTD = ds.Tables[0].Rows[0]["SO_HDTD"].ToString();
                        _obj.NGAY_KUOC = ds.Tables[0].Rows[0]["NGAY_GIAI_NGAN"].ToString();

                        _obj.ID_KUOC = Convert.ToInt32(ds.Tables[0].Rows[0]["ID_KUOC"]);
                        _obj.MA_KUOC = ds.Tables[0].Rows[0]["MA_KUOC"].ToString();
                        _obj.SO_KUOC = ds.Tables[0].Rows[0]["SO_KUOC"].ToString();
                        _obj.NGAY_HOP_DONG = ds.Tables[0].Rows[0]["NGAY_HD"].ToString();

                        txtSoKuoc.Text = _obj.SO_KUOC;
                        teldtNgayKuoc.Value = LDateTime.StringToDate(_obj.NGAY_KUOC, "yyyyMMdd");
                        txtSoHDTD.Text = _obj.SO_HDTD;
                        teldtNgayHDTD.Value = LDateTime.StringToDate(_obj.NGAY_HOP_DONG, "yyyyMMdd");
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.KhongTonTaiDuLieu", LMessage.MessageBoxType.Warning);
                        _obj.ID_HDTD = 0;
                        _obj.MA_HDTD = "";
                        _obj.SO_HDTD = "";
                        _obj.NGAY_KUOC = "";

                        _obj.ID_KUOC = 0;
                        _obj.MA_KUOC = "";
                        _obj.SO_KUOC = "";
                        _obj.NGAY_HOP_DONG = "";

                        txtSoKuoc.Text = "";
                        teldtNgayKuoc.Value = null;
                        txtSoHDTD.Text = "";
                        teldtNgayHDTD.Value = null;
                    }
                }
                else
                {
                    _obj.ID_HDTD = 0;
                    _obj.MA_HDTD = "";
                    _obj.SO_HDTD = "";
                    _obj.NGAY_KUOC = "";

                    _obj.ID_KUOC = 0;
                    _obj.MA_KUOC = "";
                    _obj.SO_KUOC = "";
                    _obj.NGAY_HOP_DONG = "";

                    txtSoKuoc.Text = "";
                    teldtNgayKuoc.Value = null;
                    txtSoHDTD.Text = "";
                    teldtNgayHDTD.Value = null;
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
    }
}
