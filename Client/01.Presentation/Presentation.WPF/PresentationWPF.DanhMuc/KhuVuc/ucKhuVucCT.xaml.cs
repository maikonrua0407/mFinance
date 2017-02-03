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
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using PresentationWPF.CustomControl;
using Presentation.Process.DanhMucServiceRef;
using System.Data;

namespace PresentationWPF.DanhMuc.KhuVuc
{
    /// <summary>
    /// Interaction logic for ucKhuVucCT.xaml
    /// </summary>
    public partial class ucKhuVucCT : UserControl
    {
        #region Khai bao
        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        List<AutoCompleteEntry> lstSourcePGD = new List<AutoCompleteEntry>();
        string tthaiNVu;
        int idKhuVuc;
        public DatabaseConstant.Action action = DatabaseConstant.Action.XEM;
        public EventHandler OnSavingCompleted;
        #endregion

        #region Khoi tao
        public ucKhuVucCT()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.DanhMuc;component/KhuVuc/ucKhuVucCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindShortkey();
            KhoiTaoComboBox();
            ResetForm();
        }

        private void KhoiTaoComboBox()
        {
            List<string> lstDieuKien = new List<string>();
            string sTruyVan = "";
            AutoComboBox auto = new AutoComboBox();
            sTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue();
            lstDieuKien.Add(ClientInformation.MaDonVi);
            auto.GenAutoComboBox(ref lstSourcePGD, ref cmbDonVi, sTruyVan, lstDieuKien, ClientInformation.MaDonViGiaoDich);
        }
        #endregion

        #region Dang ky hot key

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
                        key = new KeyBinding(DeleteCommand, keyg);
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

        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                AfterSave(BusinessConstant.TrangThaiNghiepVu.LUU_TAM, BusinessConstant.TrangThaiSuDung.SU_DUNG);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                AfterSave(BusinessConstant.TrangThaiNghiepVu.DA_DUYET, BusinessConstant.TrangThaiSuDung.SU_DUNG);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                OnModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                beforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }

        #endregion

        #region Dang ky shortcut key

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
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbModify.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeDelete();
        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbApprove.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbRefuse.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbCancel.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
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

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                beforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
            }
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

        #region Xu ly Giao dien
        /// <summary>
        /// Sự kiện ấn key trên form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        private void ResetForm()
        {
            lblTrangThai.Content = tthaiNVu = "";
            txtMaKhuVuc.Text = "";
            txtTenKhuVuc.Text = "";
            txtTenTat.Text = "";
            cmbDonVi.SelectedIndex = lstSourcePGD.IndexOf(lstSourcePGD.FirstOrDefault(e => e.KeywordStrings.FirstOrDefault().Equals(ClientInformation.MaDonViGiaoDich)));
            txtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            txtNgayDuyet.Value = null;
            txtNguoiDuyet.Text = "";
            txtTrangThaiBanGhi.Text = "";
            txtTenKhuVuc.Focus();
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNVu, mnuMain, DatabaseConstant.Function.DC_DM_KHU_VUC);
        }

        /// <summary>
        /// SetEnabledAllControls
        /// </summary>
        /// <param name="enable"></param>
        private void SetEnabledAllControls(bool enable)
        {
            //cmbChiNhanh.IsEnabled = enable;
            //cmbPhongGD.IsEnabled = enable;
            cmbDonVi.IsEnabled = enable;
            txtTenKhuVuc.IsEnabled = enable;
            txtTenTat.IsEnabled = enable;
        }

        /// <summary>
        /// SetEnabledRequiredControls
        /// </summary>
        /// <param name="enable"></param>
        private void SetEnabledRequiredControls(bool enable)
        {
            
        }
        #endregion    
        
        #region Xy ly nghiep vu
        bool Validation()
        {
            if (txtTenKhuVuc.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblTenKhuVuc.Content.ToString());
                txtTenKhuVuc.Focus();
                return false;
            }
            else if (txtTenTat.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblTenKhuVuc.Content.ToString());
                txtTenTat.Focus();
                return false;
            }
            else if (cmbDonVi.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblDonVi.Content.ToString());
                cmbDonVi.Focus();
                return false;
            }
            return true;
        }

        private DM_KHU_VUC GetDataForm(BusinessConstant.TrangThaiNghiepVu nghiepvu, BusinessConstant.TrangThaiSuDung sudung)
        {
            DM_KHU_VUC obj = new DM_KHU_VUC();
            DataSet dsDVi = new DanhMucProcess().getDonViTheoMa(lstSourcePGD.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings[0]);
            obj.ID = idKhuVuc;
            obj.ID_DVI = Convert.ToInt32(dsDVi.Tables[0].Rows[0]["ID"]);
            obj.MA_DVI = lstSourcePGD.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.FirstOrDefault();
            obj.MA_DVI_QLY = ClientInformation.MaDonVi;
            obj.MA_DVI_TAO = lstSourcePGD.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.FirstOrDefault();
            obj.MA_KVUC = txtMaKhuVuc.Text;
            obj.TEN_KVUC = txtTenKhuVuc.Text;
            obj.TEN_TAT = txtTenTat.Text;
            obj.TTHAI_NVU = nghiepvu.layGiaTri();
            obj.TTHAI_BGHI = sudung.layGiaTri();
            obj.NGAY_CNHAT = idKhuVuc > 0 ? ClientInformation.NgayLamViecHienTai : "";
            obj.NGUOI_CNHAT = idKhuVuc > 0 ? ClientInformation.TenDangNhap : "";
            obj.NGAY_NHAP = idKhuVuc == 0 ? ClientInformation.NgayLamViecHienTai : LDateTime.DateToString(txtNgayLap.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
            obj.NGUOI_NHAP = idKhuVuc == 0 ? ClientInformation.TenDangNhap : txtNguoiLap.Text;
            return obj;
        }

        private void AfterSave(BusinessConstant.TrangThaiNghiepVu nghiepvu, BusinessConstant.TrangThaiSuDung sudung)
        {
            try
            {
                Cursor = Cursors.Wait;
                if (!Validation() && nghiepvu.Equals(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET))
                return;
                DM_KHU_VUC obj = GetDataForm(nghiepvu, sudung);

                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(idKhuVuc);
                bool retLockData = true;
                if(idKhuVuc>0)
                retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_KHU_VUC,
                    DatabaseConstant.Table.DM_KHU_VUC,
                    DatabaseConstant.Action.SUA,
                    listLockId);
                if (retLockData)
                    OnSave(obj);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
                // Unlock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(idKhuVuc);
                bool retLockData = true;
                if (idKhuVuc > 0)
                    retLockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_DM_KHU_VUC,
                        DatabaseConstant.Table.DM_KHU_VUC,
                        DatabaseConstant.Action.SUA,
                        listLockId);
            }
        }

        private void OnSave(DM_KHU_VUC obj)
        {
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                if (idKhuVuc == 0)
                    obj = new DanhMucProcess().ThemKhuVuc(obj, ref listClientResponseDetail);
                else
                    obj = new DanhMucProcess().SuaKhuVuc(obj, ref listClientResponseDetail);
                BeforSave(obj, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void BeforSave(DM_KHU_VUC obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if(obj.ID>0)
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    SetThongTin(obj);
                    if (cbMultiAdd.IsChecked.GetValueOrDefault())
                        ResetForm();
                }
                else
                {
                    LMessage.ShowMessage("M.DungChung.LoiLuuDuLieu", LMessage.MessageBoxType.Error);
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {

            }
        }

        /// <summary>
        /// Trước khi xóa
        /// </summary>
        private void beforeDelete()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(idKhuVuc);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_KHU_VUC,
                    DatabaseConstant.Table.DM_KHU_VUC,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    // Gọi tới hàm xóa dữ liệu
                    OnDelete(listLockId);
                    return;
                }
                // Nếu lock không thành công >> cảnh báo
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                    return;
                }
            }
            else
            {
                // Unlock dữ liệu
                // (Hiện tại không phải Unlock dữ liệu, vì chỉ được xóa khi chọn sửa - sẽ Unlock sau)
                return;
            }
        }

        private void OnDelete(List<int> lstID)
        {
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool bKetQua = new DanhMucProcess().XoaKhuVuc(lstID.ToArray(), ref listClientResponseDetail);
                AfterDelete(bKetQua, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void AfterDelete(bool bKetQua, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (bKetQua)
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    CommonFunction.CloseUserControl(this);
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
            }
        }

        private void AfterApprove()
        {
            try
            {
                Cursor = Cursors.Wait;
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(idKhuVuc);
                bool retLockData = true;
                if (idKhuVuc > 0)
                    retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_DM_KHU_VUC,
                        DatabaseConstant.Table.DM_KHU_VUC,
                        DatabaseConstant.Action.DUYET,
                        listLockId);
                OnApprove(listLockId);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(idKhuVuc);
                bool retLockData = true;
                if (idKhuVuc > 0)
                    retLockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_DM_KHU_VUC,
                        DatabaseConstant.Table.DM_KHU_VUC,
                        DatabaseConstant.Action.DUYET,
                        listLockId);
                Cursor = Cursors.Arrow;
            }
        }

        private void OnApprove(List<int> lstID)
        {
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool bKetQua = new DanhMucProcess().DuyetKhuVuc(lstID.ToArray(), ref listClientResponseDetail);
                BeforApprove(bKetQua, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void BeforApprove(bool bKetQua, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (bKetQua)
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    SetDataForm(idKhuVuc);
                    if (cbMultiAdd.IsChecked.GetValueOrDefault())
                        ResetForm();
                }
                else
                {
                    LMessage.ShowMessage("M.DungChung.DuyetKhongThanhCong", LMessage.MessageBoxType.Error);
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
            }
        }

        private void AfterRefuse()
        {
            try
            {
                Cursor = Cursors.Wait;
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(idKhuVuc);
                bool retLockData = true;
                if (idKhuVuc > 0)
                    retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_DM_KHU_VUC,
                        DatabaseConstant.Table.DM_KHU_VUC,
                        DatabaseConstant.Action.TU_CHOI_DUYET,
                        listLockId);
                OnRefuse(listLockId);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(idKhuVuc);
                bool retLockData = true;
                if (idKhuVuc > 0)
                    retLockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_DM_KHU_VUC,
                        DatabaseConstant.Table.DM_KHU_VUC,
                        DatabaseConstant.Action.TU_CHOI_DUYET,
                        listLockId);
                Cursor = Cursors.Arrow;
            }
        }

        private void OnRefuse(List<int> lstID)
        {
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool bKetQua = new DanhMucProcess().TuChoiKhuVuc(lstID.ToArray(), ref listClientResponseDetail);
                BeforRefuse(bKetQua, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void BeforRefuse(bool bKetQua, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (bKetQua)
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    SetDataForm(idKhuVuc);
                    if (cbMultiAdd.IsChecked.GetValueOrDefault())
                        ResetForm();
                }
                else
                {
                    LMessage.ShowMessage("M.DungChung.DuyetKhongThanhCong", LMessage.MessageBoxType.Error);
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
            }
        }

        private void AfterCancel()
        {
            try
            {
                Cursor = Cursors.Wait;
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(idKhuVuc);
                bool retLockData = true;
                if (idKhuVuc > 0)
                    retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_DM_KHU_VUC,
                        DatabaseConstant.Table.DM_KHU_VUC,
                        DatabaseConstant.Action.THOAI_DUYET,
                        listLockId);
                OnCancel(listLockId);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(idKhuVuc);
                bool retLockData = true;
                if (idKhuVuc > 0)
                    retLockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_DM_KHU_VUC,
                        DatabaseConstant.Table.DM_KHU_VUC,
                        DatabaseConstant.Action.THOAI_DUYET,
                        listLockId);
                Cursor = Cursors.Arrow;
            }
        }

        private void OnCancel(List<int> lstID)
        {
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool bKetQua = new DanhMucProcess().ThoaiDuyetKhuVuc(lstID.ToArray(), ref listClientResponseDetail);
                BeforCancel(bKetQua, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void BeforCancel(bool bKetQua, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (bKetQua)
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    SetDataForm(idKhuVuc);
                    if (cbMultiAdd.IsChecked.GetValueOrDefault())
                        ResetForm();
                }
                else
                {
                    LMessage.ShowMessage("M.DungChung.DuyetKhongThanhCong", LMessage.MessageBoxType.Error);
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
            }
        }

        private void SetThongTin(DM_KHU_VUC obj)
        {
            try
            {
                idKhuVuc = obj.ID;
                tthaiNVu = obj.TTHAI_NVU;
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tthaiNVu);
                txtMaKhuVuc.Text = obj.MA_KVUC;
                txtNguoiDuyet.Text = obj.NGUOI_CNHAT;
                if (!obj.NGAY_CNHAT.IsNullOrEmptyOrSpace())
                    txtNgayDuyet.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, ApplicationConstant.defaultDateTimeFormat);
                txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                action = DatabaseConstant.Action.XEM;
                CommonFunction.RefreshButton(Toolbar, action, tthaiNVu, mnuMain, DatabaseConstant.Function.DC_DM_KHU_VUC);
                SetEnabledAllControls(false);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
            }
            
        }

        public void SetDataForm(int id)
        {
            try
            {
                idKhuVuc = id;
                DM_KHU_VUC obj = new DanhMucProcess().getKhuVucById(idKhuVuc);
                if(!LObject.IsNullOrEmpty(obj))
                {
                    tthaiNVu = obj.TTHAI_NVU;
                    txtMaKhuVuc.Text = obj.MA_KVUC;
                    txtTenKhuVuc.Text = obj.TEN_KVUC;
                    txtTenTat.Text = obj.TEN_TAT;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tthaiNVu);
                    cmbDonVi.SelectedIndex = lstSourcePGD.IndexOf(lstSourcePGD.FirstOrDefault(e => e.KeywordStrings.First().Equals(obj.MA_DVI)));
                    txtNguoiLap.Text = obj.NGUOI_NHAP;
                    txtNgayLap.Value = LDateTime.StringToDate(obj.NGAY_NHAP, ApplicationConstant.defaultDateTimeFormat);
                    txtNguoiDuyet.Text = obj.NGUOI_CNHAT;
                    if(!obj.NGAY_CNHAT.IsNullOrEmptyOrSpace())
                        txtNgayDuyet.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, ApplicationConstant.defaultDateTimeFormat);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    if (action.Equals(DatabaseConstant.Action.SUA))
                        SetEnabledAllControls(true);
                    else
                        SetEnabledAllControls(false);
                    CommonFunction.RefreshButton(Toolbar, action, tthaiNVu, mnuMain, DatabaseConstant.Function.DC_DM_KHU_VUC);
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void OnModify()
        {
            SetEnabledAllControls(true);
            action = DatabaseConstant.Action.SUA;
            CommonFunction.RefreshButton(Toolbar, action, tthaiNVu, mnuMain, DatabaseConstant.Function.DC_DM_KHU_VUC);
        }
        #endregion
    }
}
