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
using System.Data;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.KeToanServiceRef;

namespace PresentationWPF.KeToan.TaiKhoan
{
    /// <summary>
    /// Interaction logic for ucTaiKhoanCT.xaml
    /// </summary>
    public partial class ucTaiKhoanCT : UserControl
    {
        #region Khai bao
        public static RoutedCommand AddCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand MakeCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        // Source combobox
        List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTienTe = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTinhChatTK = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDoiTuong = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNguonVon = new List<AutoCompleteEntry>();

        public event EventHandler OnSavingCompleted;

        private DataRow drPhanLoai = null;

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        private string tthaiNvu = "";
        public string TthaiNvu
        {
            get { return tthaiNvu; }
            set { tthaiNvu = value; }
        }

        private int _idTaiKhoan = -1;

        private string dauPhanCachTK = ".";

        private bool daPhatSinhGD = false;
        private string tthaiDongMoTK = "MO";

        #endregion

        #region Khoi tao
        public ucTaiKhoanCT()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KhachHang;component/ChuyenDiaBan/ucPhanLoaiCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }

            raddtNgaySoLieu.Value = LDateTime.StringToDate(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtMaPhanLoai.KeyDown += new KeyEventHandler(txtMaPhanLoai_KeyDown);
            BindShortkey();
            InitCombobox();
            ResetForm();
        }

        
        public ucTaiKhoanCT(int id, string tthai, DatabaseConstant.Action action) : this()
        {
            //SetFormData();
            _idTaiKhoan = id;
            tthaiNvu = tthai;
            beforeModifyFromList(action);
        }

        /// <summary>
        /// Khoi tao combobox
        /// </summary>
        private void InitCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDK = new List<string>();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                //Combobox donvi
                auto.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVI.getValue(), null, Presentation.Process.Common.ClientInformation.MaDonViGiaoDich);

                // Combobox loại tiền
                auto.GenAutoComboBox(ref lstSourceTienTe, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TIENTE.getValue(), null, Presentation.Process.Common.ClientInformation.MaDongNoiTe);

                // Combobox tính chất tài khoản
                auto.GenAutoComboBox(ref lstSourceTinhChatTK, ref cmbTinhChatTK, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TINH_CHAT_TK.getValue(), null);

                // Combobox doi tuong
                lstDK.Add(ClientInformation.MaDonVi);
                auto.GenAutoComboBox(ref lstSourceDoiTuong, ref cmbDoiTuong, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TK_LOAI_DTUONG.getValue(), lstDK);

                // Combobox nguon von
                auto.GenAutoComboBox(ref lstSourceNguonVon, ref cmbNguonVon, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NGUON_VON.getValue(), null);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
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
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
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

        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbAdd.IsEnabled;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ResetForm();
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbSave.IsEnabled;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onSave();
        }

        private void MakeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbMake.IsEnabled;
        }
        private void MakeCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onMake();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbModify.IsEnabled;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeModifyFromDetail();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbDelete.IsEnabled;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeDelete();
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
            onClose();
        }

        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);

            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetForm();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                onSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                beforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                beforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TAO_DU_LIEU)))
            {
                onMake();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                onClose();
            }
        }

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetForm();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                onSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TAO_DU_LIEU)))
            {
                onMake();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                beforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                beforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                onClose();
            }
        }

        #endregion 

        #region Xu ly Giao dien

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Release();
            if (OnSavingCompleted != null)
            {
                OnSavingCompleted(this, EventArgs.Empty);
            }
            CustomControl.CommonFunction.CloseUserControl(this);
        }

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
        /// Trợ giúp
        /// </summary>
        private void onHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }

        /// <summary>
        /// Đóng form
        /// </summary>
        private void onClose()
        {
            Release();
            if (OnSavingCompleted != null)
            {
                OnSavingCompleted(this, EventArgs.Empty);
            }
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        /// <summary>
        /// Hàm xử lý giải phóng lock dữ liệu cho form
        /// </summary>
        private void Release()
        {
            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idTaiKhoan);

            bool ret = process.UnlockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        private void SetEnabledAllControls(bool enable)
        {
            txtMaPhanLoai.IsEnabled = enable;
            btnMaPhanLoai.IsEnabled = enable;
            cmbLoaiTien.IsEnabled = enable;
            txtSoTaiKhoan.IsEnabled = enable;
            txtTenTaiKhoan.IsEnabled = enable;
            raddtNgaySoLieu.IsEnabled = enable;
            dtpNgaySoLieu.IsEnabled = enable;
            cmbTinhChatTK.IsEnabled = enable;
            numSoDu.IsEnabled = enable;
            cmbDoiTuong.IsEnabled = enable;
            cmbNguonVon.IsEnabled = enable;
            if (!LObject.IsNullOrEmpty(raddgrDoiTuongTK.ItemsSource) && raddgrDoiTuongTK.Items.Count > 0)
                cmbDoiTuong.IsEnabled = false;
        }

        void txtMaPhanLoai_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
                btnMaPhanLoai_Click(sender, null);
        }

        #endregion              
 
        #region Xu ly nghiep vu
        private void beforeView()
        {
            SetFormData();
            SetEnabledAllControls(false);
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu,mnuMain,DatabaseConstant.Function.KT_TAI_KHOAN_CT);
            tlbMake.IsEnabled = true;
        }

        private void beforeAddNew()
        {
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu,mnuMain,DatabaseConstant.Function.KT_TAI_KHOAN_CT);
            tlbMake.IsEnabled = true;
            lblTrangThai.Content = "";
            raddtNgayNhap.Value = null;
            raddtNgayCNhat.Value = null;
        }

        private void beforeModifyFromList(DatabaseConstant.Action action)
        {
            SetFormData();
            if (action == DatabaseConstant.Action.XEM)
            {
                SetEnabledAllControls(false);
            }
            if (action == DatabaseConstant.Action.SUA)
            {
                if (daPhatSinhGD)
                {
                    SetEnabledAllControls(false);
                    txtTenTaiKhoan.IsEnabled = true;
                    dtpNgaySoLieu.IsEnabled = true;
                }
                else
                {
                    SetEnabledAllControls(false);
                    txtTenTaiKhoan.IsEnabled = true;
                    raddtNgaySoLieu.IsEnabled = true;
                    dtpNgaySoLieu.IsEnabled = true;
                    cmbTinhChatTK.IsEnabled = true;
                    numSoDu.IsEnabled = true;
                    cmbDoiTuong.IsEnabled = true;
                }
            }
            CommonFunction.RefreshButton(Toolbar, action, tthaiNvu,mnuMain,DatabaseConstant.Function.KT_TAI_KHOAN_CT);
            tlbMake.IsEnabled = false;
        }

        private void beforeModifyFromDetail()
        {
            // Yêu cầu lock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idTaiKhoan);

            bool ret = process.LockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Nếu lock thành công >> cho phép sửa
            if (ret)
            {
                SetFormData(); 
                if (daPhatSinhGD)
                {
                    SetEnabledAllControls(false);
                    txtTenTaiKhoan.IsEnabled = true;
                }
                else
                {
                    SetEnabledAllControls(false);
                    txtTenTaiKhoan.IsEnabled = true;
                    raddtNgaySoLieu.IsEnabled = true;
                    dtpNgaySoLieu.IsEnabled = true;
                    cmbTinhChatTK.IsEnabled = true;
                    numSoDu.IsEnabled = true;
                    cmbDoiTuong.IsEnabled = true;
                }
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu,mnuMain,DatabaseConstant.Function.KT_TAI_KHOAN_CT);
                tlbMake.IsEnabled = false;
            }
            else
            {
                LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
            }
        }

        private void beforeDelete()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_idTaiKhoan);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.GDKT,
                    DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                    DatabaseConstant.Table.KT_PLOAI,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    // Gọi tới hàm xóa dữ liệu
                    onDelete();
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

        private void onMake()
        {
            if (Validation(false))
            {
                KeToanProcess process = new KeToanProcess();
                try
                {
                    Presentation.Process.KeToanServiceRef.KT_TKHOAN objTkhoan = new Presentation.Process.KeToanServiceRef.KT_TKHOAN();
                    List<Presentation.Process.KeToanServiceRef.DC_CTRUC_CTIET> lstCtruc = new List<Presentation.Process.KeToanServiceRef.DC_CTRUC_CTIET>();
                    List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                    // Lay gia tri
                    objTkhoan = GetTaiKhoan("");
                    lstCtruc = GetCauTrucTK();

                    ApplicationConstant.ResponseStatus ret = process.TaiKhoanChiTiet(DatabaseConstant.Action.TAO_DU_LIEU, ref objTkhoan,ref listClientResponseDetail, lstCtruc);
                    afterMake(ret, objTkhoan);
                }
                catch (System.Exception ex)
                {
                    this.Cursor = Cursors.Arrow;
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                finally
                {
                    process = null;
                }
            }
        }

        /// <summary>
        /// Lưu dữ liệu (hay trình duyệt)
        /// </summary>
        private void onSave()
        {
            string trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
            if (Validation(true))
            {
                KeToanProcess process = new KeToanProcess();
                try
                {
                    Presentation.Process.KeToanServiceRef.KT_TKHOAN obj = new Presentation.Process.KeToanServiceRef.KT_TKHOAN();
                    ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                    // Dữ liệu truyền vào và dữ liệu trả về
                    
                    Mouse.OverrideCursor = Cursors.Wait;
                    obj = GetTaiKhoan(trangThai);
                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (_idTaiKhoan == -1)
                    {
                        // Lấy dữ liệu từ form
                        ret = process.TaiKhoanChiTiet(DatabaseConstant.Action.THEM, ref obj, ref listResponseDetail);
                       
                        Mouse.OverrideCursor = Cursors.Arrow;
                        afterAddNew(ret, obj, listResponseDetail);
                    }
                    // Nếu là lưu tạm khi sửa
                    // Hoặc lưu tạm khi sửa sau duyệt
                    // Hoặc sửa
                    else
                    {
                        // Lấy dữ liệu từ form
                        ret = process.TaiKhoanChiTiet(DatabaseConstant.Action.SUA, ref obj, ref listResponseDetail);
                        
                        Mouse.OverrideCursor = Cursors.Arrow;
                        afterModify(ret, obj, listResponseDetail);
                    }
                }
                catch (System.Exception ex)
                {
                    this.Cursor = Cursors.Arrow;
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                finally
                {
                    process = null;
                }
            }
        }

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        private void onDelete()
        {
            string trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();

            KeToanProcess process = new KeToanProcess();
            try
            {
                Presentation.Process.KeToanServiceRef.KT_TKHOAN obj = new Presentation.Process.KeToanServiceRef.KT_TKHOAN();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                // Dữ liệu truyền vào và dữ liệu trả về

                Mouse.OverrideCursor = Cursors.Wait;
                obj = GetTaiKhoan(trangThai);
                ret = process.TaiKhoanChiTiet(DatabaseConstant.Action.XOA, ref obj, ref listResponseDetail);

                Mouse.OverrideCursor = Cursors.Arrow;
                afterDelete(ret);

            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }
        }

        /// <summary>
        /// Sau khi thêm mới
        /// </summary>
        /// <param name="ret"></param>
        private void afterAddNew(ApplicationConstant.ResponseStatus ret, Presentation.Process.KeToanServiceRef.KT_TKHOAN obj, List<ClientResponseDetail> listResponseDetail)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                
                LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);
                SetEnabledAllControls(false);
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(obj.TTHAI_NVU);
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                txtNguoiLap.Text = obj.NGUOI_NHAP;
                raddtNgayNhap.Value = LDateTime.StringToDate(obj.NGAY_NHAP, "yyyyMMdd");
                tthaiNvu = obj.TTHAI_NVU;

                if (cbMultiAdd.IsChecked == true)
                {
                    SetEnabledAllControls(true);
                    ResetForm();
                }
                else if (!DatabaseConstant.CLOSE_DETAIL_FORM)
                {
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_TAI_KHOAN_CT);
                }
                else
                {
                    onClose();
                }
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listResponseDetail);
            }
        }

        /// <summary>
        /// Sau khi sửa
        /// </summary>
        /// <param name="ret"></param>
        private void afterModify(ApplicationConstant.ResponseStatus ret, Presentation.Process.KeToanServiceRef.KT_TKHOAN obj, List<ClientResponseDetail> listResponseDetail)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);
                tthaiNvu = obj.TTHAI_NVU;
                SetEnabledAllControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_TAI_KHOAN_CT);
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(obj.TTHAI_NVU);
                txtNguoiCapNhat.Text = obj.NGUOI_CNHAT;
                raddtNgayCNhat.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, "yyyyMMdd");
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listResponseDetail);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(obj.ID);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Sau khi xóa
        /// </summary>
        /// <param name="ret"></param>
        private void afterDelete(ApplicationConstant.ResponseStatus ret)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idTaiKhoan);
            List<int> listUnlockId = new List<int>();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Đóng cửa sổ chi tiết sau khi xóa
            onClose();
        }

        private void afterMake(ApplicationConstant.ResponseStatus status, KT_TKHOAN obj)
        {
            if (status == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
            {
                LMessage.ShowMessage("M.KeToan.TaiKhoan.ucTaiKhoanCT.LoiTaoSoTaiKhoan", LMessage.MessageBoxType.Error);
            }
            else
            {
                txtSoTaiKhoan.Text = obj.SO_TAI_KHOAN;
            }
        }

        /// <summary>
        /// Kiểm tra dữ liệu trước khi lưu
        /// </summary>
        /// <returns></returns>
        private bool Validation(bool isSave)
        {
            AutoComboBox au = new AutoComboBox();
            try
            {
                AutoCompleteEntry auChiNhanh = (AutoCompleteEntry)cmbDonVi.SelectedValue;
                AutoCompleteEntry auLoaiTien = au.getEntryByDisplayName(lstSourceTienTe, ref cmbLoaiTien);
                AutoCompleteEntry auTinhChat = au.getEntryByDisplayName(lstSourceTinhChatTK, ref cmbTinhChatTK);
                AutoCompleteEntry auDoiTuong = au.getEntryByDisplayName(lstSourceDoiTuong, ref cmbDoiTuong);
                AutoCompleteEntry auNguonVon = au.getEntryByDisplayName(lstSourceNguonVon, ref cmbNguonVon);
                if (auChiNhanh == null)
                {
                    CommonFunction.ThongBaoTrong(lblDonVi.Content.ToString());
                    cmbDonVi.Focus();
                    return false;
                }
                if (auNguonVon == null)
                {
                    CommonFunction.ThongBaoTrong(lblNguonVon.Content.ToString());
                    cmbNguonVon.Focus();
                    return false;
                }
                else if (LString.IsNullOrEmptyOrSpace(txtMaPhanLoai.Text))
                {
                    CommonFunction.ThongBaoTrong(lblMaPhanLoai.Content.ToString());
                    txtMaPhanLoai.Focus();
                    return false;
                }
                else if (raddgrThanhPhanCauTruc.ItemsSource == null || raddgrThanhPhanCauTruc.Items.Count == 0)
                {
                    LMessage.ShowMessage("M.KeToan.TaiKhoan.ucTaiKhoanCT.LoiChuaCoCauTrucTK", LMessage.MessageBoxType.Warning);
                    return false;
                }
                if (isSave)
                {
                    if (LString.IsNullOrEmptyOrSpace(txtSoTaiKhoan.Text))
                    {
                        CommonFunction.ThongBaoTrong(lblSoTaiKhoan.Content.ToString());
                        return false;
                    }
                    else if (LString.IsNullOrEmptyOrSpace(txtTenTaiKhoan.Text))
                    {
                        CommonFunction.ThongBaoTrong(lblTenTaiKhoan.Content.ToString());
                        txtTenTaiKhoan.Focus();
                        return false;
                    }
                    else if (raddtNgaySoLieu.Value == null)
                    {
                        CommonFunction.ThongBaoTrong(lblNgaySoLieu.Content.ToString());
                        raddtNgaySoLieu.Focus();
                        return false;
                    }
                    else if (auTinhChat == null)
                    {
                        CommonFunction.ThongBaoTrong(lblTinhChat.Content.ToString());
                        cmbTinhChatTK.Focus();
                        return false;
                    }
                    else if (auDoiTuong == null)
                    {
                        CommonFunction.ThongBaoTrong(lblDoiTuong.Content.ToString());
                        cmbDoiTuong.Focus();
                        return false;
                    }
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                au = null;
            }
            return true;
        }

        private void SetFormData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            KeToanProcess process = new KeToanProcess();
            AutoComboBox au = new AutoComboBox();
            try
            {
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@IdTaiKhoan","INT",_idTaiKhoan.ToString());
                DataSet ds = process.getDanhSachTaiKhoanChiTiet(dt, "TTIN_CTIET");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    txtMaPhanLoai.Text = ds.Tables[0].Rows[0]["MA_PLOAI"].ToString();
                    txtMaPhanLoai.Tag = ds.Tables[0].Rows[0]["ID_PLOAI"].ToString();
                    txtSoTaiKhoan.Text = ds.Tables[0].Rows[0]["SO_TAI_KHOAN"].ToString();
                    txtTenTaiKhoan.Text = ds.Tables[0].Rows[0]["TEN_TAI_KHOAN"].ToString();
                    raddtNgaySoLieu.Value = LDateTime.StringToDate(ds.Tables[0].Rows[0]["NGAY_GDICH"].ToString(), "yyyyMMdd");
                    numSoDu.Value = Convert.ToDouble(ds.Tables[0].Rows[0]["SODU"]);
                    lblTenPhanLoai.Content = ds.Tables[0].Rows[0]["TEN_PLOAI"].ToString();
                    string maKHangNBo = ds.Tables[0].Rows[0]["MA_KHANG_NBO"].ToString();
                    
                    // truonglq gán thêm thông tin để phục vụ hàm GetData
                    PopupProcess Popupprocess = new PopupProcess();
                    List<string> lstDK = new List<string>();
                    lstDK.Add(maKHangNBo);
                    lstDK.Add(ClientInformation.MaDonVi);
                    Popupprocess.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_PLOAI_TAO_TK.getValue(), lstDK);
                    SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;
                    foreach (DataRow dr in simplePopupResponse.DataSetSource.Tables[0].Rows)
                    {
                        if (dr["U.Popup.Code"].ToString().Equals(txtMaPhanLoai.Text))
                            drPhanLoai = dr;
                    }

                    LayDanhSachCauTrucTK(maKHangNBo);

                    cmbDonVi.SelectedIndex = lstSourceDonVi.IndexOf(lstSourceDonVi.FirstOrDefault(f => f.KeywordStrings.First().Equals(ds.Tables[0].Rows[0]["MA_DVI"].ToString())));
                    cmbDoiTuong.SelectedIndex = lstSourceDoiTuong.IndexOf(lstSourceDoiTuong.FirstOrDefault(f => f.KeywordStrings.First().Equals(ds.Tables[0].Rows[0]["LOAI_DTUONG"].ToString())));
                    cmbLoaiTien.SelectedIndex = lstSourceTienTe.IndexOf(lstSourceTienTe.FirstOrDefault(f => f.KeywordStrings.First().Equals(ds.Tables[0].Rows[0]["MA_LOAI_TIEN"].ToString())));
                    cmbTinhChatTK.SelectedIndex = lstSourceTinhChatTK.IndexOf(lstSourceTinhChatTK.FirstOrDefault(f => f.KeywordStrings.First().Equals(ds.Tables[0].Rows[0]["MA_LSDU"].ToString())));
                    if (ds.Tables[0].Rows[0]["NV_LOAI_NVON"] != DBNull.Value)
                        cmbNguonVon.SelectedIndex = lstSourceNguonVon.IndexOf(lstSourceNguonVon.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(ds.Tables[0].Rows[0]["NV_LOAI_NVON"].ToString())));

                    tthaiNvu = ds.Tables[0].Rows[0]["TTHAI_NVU"].ToString();
                    txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                    lblTrangThai.Content = txtTrangThai.Text;
                    txtNguoiLap.Text = ds.Tables[0].Rows[0]["NGUOI_NHAP"].ToString();
                    raddtNgayNhap.Value = LDateTime.StringToDate(ds.Tables[0].Rows[0]["NGAY_NHAP"].ToString(), "yyyyMMdd");
                    if (ds.Tables[0].Rows[0]["NGUOI_CNHAT"] != null && !LString.IsNullOrEmptyOrSpace(ds.Tables[0].Rows[0]["NGUOI_CNHAT"].ToString()))
                    {
                        txtNguoiCapNhat.Text = ds.Tables[0].Rows[0]["NGUOI_CNHAT"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["NGAY_CNHAT"] != null && !LString.IsNullOrEmptyOrSpace(ds.Tables[0].Rows[0]["NGAY_CNHAT"].ToString()))
                    {
                        raddtNgayCNhat.Value = LDateTime.StringToDate(ds.Tables[0].Rows[0]["NGAY_CNHAT"].ToString(), "yyyyMMdd");
                    }
                    if (ds.Tables[0].Rows[0]["PHAT_SINH"].ToString().StringToInt32() > 0)
                        daPhatSinhGD = true;
                    else
                        daPhatSinhGD = false;
                    tthaiDongMoTK = ds.Tables[0].Rows[0]["TTHAI_DOMO"].ToString();
                }
                else
                {
                    
                }
                ds = process.getDanhSachTaiKhoanChiTiet(dt, "DOI_TUONG");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    raddgrDoiTuongTK.ItemsSource = ds.Tables[0].DefaultView;
                }
                else
                {

                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private Presentation.Process.KeToanServiceRef.KT_TKHOAN GetTaiKhoan(string tthai)
        {
            AutoComboBox au = new AutoComboBox();
            Presentation.Process.KeToanServiceRef.KT_TKHOAN obj = new Presentation.Process.KeToanServiceRef.KT_TKHOAN();

            AutoCompleteEntry auChiNhanh = (AutoCompleteEntry) cmbDonVi.SelectedValue;
            AutoCompleteEntry auLoaiTien = au.getEntryByDisplayName(lstSourceTienTe, ref cmbLoaiTien);
            AutoCompleteEntry auTinhChat = au.getEntryByDisplayName(lstSourceTinhChatTK, ref cmbTinhChatTK);
            AutoCompleteEntry auDoiTuong = au.getEntryByDisplayName(lstSourceDoiTuong, ref cmbDoiTuong);
            AutoCompleteEntry auNguonVon = au.getEntryByDisplayName(lstSourceNguonVon, ref cmbNguonVon);
            if (auDoiTuong != null)
            {
                //obj.ID_DTUONG = Convert.ToInt32(auDoiTuong.KeywordStrings[1]);
                obj.LOAI_DTUONG = auDoiTuong.KeywordStrings[0];
            }

            obj.ID_PLOAI = Convert.ToInt32(txtMaPhanLoai.Tag);
            obj.ID_DVI = Convert.ToInt32(auChiNhanh.KeywordStrings[1]);
            obj.MA_PLOAI = txtMaPhanLoai.Text;
            obj.MA_DVI = auChiNhanh.KeywordStrings[0];
            obj.SO_TAI_KHOAN = txtSoTaiKhoan.Text;
            obj.NGAY_GDICH = Convert.ToDateTime(raddtNgaySoLieu.Value).ToString("yyyyMMdd");
            obj.TEN_TAI_KHOAN = txtTenTaiKhoan.Text;
            obj.MA_NOI_NGOAI = drPhanLoai["MA_NOI_NGOAI"].ToString();
            obj.MA_KHANG_NBO = drPhanLoai["MA_KHANG_NBO"].ToString();
            obj.MA_TNHAP_CPHI = drPhanLoai["MA_TNHAP_CPHI"].ToString();
            obj.MA_TCHAT_CNO = drPhanLoai["MA_TCHAT_CNO"].ToString();
            obj.TTHAI_DOMO = tthaiDongMoTK;
            obj.MA_LOAI_TIEN = auLoaiTien.KeywordStrings[0];
            obj.TY_GIA = 1;
            obj.SODU = Convert.ToDecimal(numSoDu.Value);
            obj.SODU_KDUNG = Convert.ToDecimal(numSoDu.Value);
            obj.SODU_TTE = Convert.ToDecimal(numSoDu.Value);
            obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
            obj.TTHAI_NVU = tthai;
            obj.MA_DVI_QLY = ClientInformation.MaDonVi;
            obj.MA_DVI_TAO = auChiNhanh.KeywordStrings[0];
            obj.MA_LSDU = auTinhChat.KeywordStrings[0];
            obj.ID_LSDU = Convert.ToInt32(auTinhChat.KeywordStrings[1]);
            obj.NV_LOAI_NVON = auNguonVon.KeywordStrings.FirstOrDefault();
            //obj.LOAI_DTUONG = "";

            if (_idTaiKhoan == -1)
            {
                obj.NGUOI_NHAP = Presentation.Process.Common.ClientInformation.TenDangNhap;
                obj.NGAY_NHAP = Presentation.Process.Common.ClientInformation.NgayLamViecHienTai;
                obj.TTHAI_DOMO = "MO";
            }
            else
            {
                obj.ID = _idTaiKhoan;
                obj.NGUOI_NHAP = txtNguoiLap.Text;
                obj.NGAY_NHAP = LDateTime.DateToString(Convert.ToDateTime(raddtNgayNhap.Value), "yyyyMMdd");
                obj.NGUOI_CNHAT = Presentation.Process.Common.ClientInformation.TenDangNhap;
                obj.NGAY_CNHAT = Presentation.Process.Common.ClientInformation.NgayLamViecHienTai;
            }

            return obj;
        }

        private List<Presentation.Process.KeToanServiceRef.DC_CTRUC_CTIET> GetCauTrucTK()
        {
            List<Presentation.Process.KeToanServiceRef.DC_CTRUC_CTIET> lstCTruc = new List<Presentation.Process.KeToanServiceRef.DC_CTRUC_CTIET>();
            for (int i = 0; i < raddgrThanhPhanCauTruc.Items.Count;i++ )
            {
                DataRowView drv = (DataRowView)raddgrThanhPhanCauTruc.Items[i];
                Presentation.Process.KeToanServiceRef.DC_CTRUC_CTIET obj = new Presentation.Process.KeToanServiceRef.DC_CTRUC_CTIET();
                obj.ID = Convert.ToInt32(drv["ID"]);
                obj.ID_CTRUC = Convert.ToInt32(drv["ID_CTRUC"]);
                obj.MA_CTRUC = drv["MA_CTRUC"].ToString();
                obj.MA_TPHAN = drv["MA_TPHAN"].ToString();
                obj.SO_TT = Convert.ToInt32(drv["STT"]);
                obj.TEN_TPHAN = drv["TEN_TPHAN"].ToString();
                obj.DO_DAI = Convert.ToInt32(drv["DO_DAI"]);
                obj.LOAI_GTRI = drv["LOAI_GTRI"].ToString();
                obj.KIEU_DU_LIEU = drv["KIEU_DU_LIEU"].ToString();
                obj.DS_TSO = drv["DS_TSO"].ToString();
                obj.DS_TSO_KIEU_DLIEU = drv["DS_TSO_KIEU_DLIEU"].ToString();
                obj.GIA_TRI = drv["GIA_TRI"].ToString();
                //obj.TTHAI_BGHI = drv["TTHAI_BGHI"].ToString();
                //obj.TTHAI_NVU = drv["TTHAI_NVU"].ToString();
                //obj.MA_DVI_QLY = drv["MA_DVI_QLY"].ToString();
                //obj.MA_DVI_TAO = drv["MA_DVI_TAO"].ToString();
                //obj.NGAY_NHAP = drv["NGAY_NHAP"].ToString();
                //obj.NGUOI_NHAP = drv["NGUOI_NHAP"].ToString();
                //obj.NGAY_CNHAT = drv["NGAY_CNHAT"].ToString();
                //obj.NGUOI_CNHAT = drv["NGUOI_CNHAT"].ToString();

                lstCTruc.Add(obj);
            }
            return lstCTruc;
        }

        private void ResetForm()
        {
            txtMaPhanLoai.Text = "";
            txtMaPhanLoai_LostFocus(txtMaPhanLoai, null);
            txtSoTaiKhoan.Text = "";
            cmbDoiTuong.SelectedIndex = 0;
            cmbTinhChatTK.SelectedIndex = 0;
            cmbLoaiTien.SelectedIndex = lstSourceTienTe.IndexOf(lstSourceTienTe.FirstOrDefault(f=>f.KeywordStrings.FirstOrDefault().Equals(ClientInformation.MaDongNoiTe)));
            raddgrThanhPhanCauTruc.ItemsSource = null;
            _idTaiKhoan = -1;
            lblTrangThai.Content = "";
            tthaiNvu = "";
            txtTrangThai.Text = "";
            txtNguoiLap.Text = "";
            txtNguoiCapNhat.Text = "";
            txtTenTaiKhoan.Text = "";
            raddtNgayNhap.Value = null;
            raddtNgayCNhat.Value = null;
            SetEnabledAllControls(true);
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_TAI_KHOAN_CT);
            tlbMake.IsEnabled = true;
        }
        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtMaPhanLoai.Focus();
        }

        private void btnMaPhanLoai_Click(object sender, RoutedEventArgs e)
        {
            Presentation.Process.KeToanProcess ketoanProcess = new Presentation.Process.KeToanProcess();
            try
            {
                //Bat popup
                PopupProcess process = new PopupProcess();

                List<string> lstDK = new List<string>();
                lstDK.Add("%");
                lstDK.Add(ClientInformation.MaDonVi);
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_PLOAI_TAO_TK.getValue(),lstDK);

                SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = simplePopupResponse.PopupTitle;
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow dr = lstPopup[0];
                    drPhanLoai = dr;
                    SetFormTheoMaPhanLoai(true);
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                ketoanProcess = null;
            }
        }

        private void LayDanhSachCauTrucTK(string ma_khang_nbo)
        {
            string maCtruc = "";
            switch (ma_khang_nbo)
            {
                case "NOI_BO":
                    maCtruc = "TAI_KHOAN_NOI_BO";
                    break;

                case "KHACH_HANG":
                    maCtruc = "TAI_KHOAN_CHI_TIET";
                    break;
            }
            KeToanProcess process = new KeToanProcess();
            AutoComboBox auto = new AutoComboBox();
            DataSet ds = process.getDanhSachCauTrucTaiKhoan(maCtruc);
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    switch (dr["MA_TPHAN"].ToString())
                    {
                        case "MA_PHAN_LOAI":
                            dr["GIA_TRI"] = txtMaPhanLoai.Text;
                            break;
                        case "MA_DON_VI":
                            dr["GIA_TRI"] = ClientInformation.MaDonViGiaoDich;
                            break;
                        case "DAU_PHAN_CACH":
                            dr["GIA_TRI"] = dauPhanCachTK;
                            break;
                    }
                }
                raddgrThanhPhanCauTruc.ItemsSource = ds.Tables[0].DefaultView;
            }
        }

        private void btnChon_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //var parent = (sender as Button).ParentOfType<Telerik.Windows.Controls.GridView.GridViewRow>();
                //DataRowView dr = parent.Item as DataRowView;
                //DataRow drPopup = null;
                //switch (dr["MA_TPHAN"].ToString())
                //{
                //    case "MA_PHAN_LOAI":
                //        List<string> lstDK = new List<string>();
                //        lstDK.Add("%");
                //        drPopup = HienThiPopup(DatabaseConstant.DanhSachTruyVan.POPUP_PLOAI_TAO_TK.getValue(), "Danh sách mã phân loại",lstDK);
                //        if (drPopup != null)
                //        {
                //            dr["GIA_TRI"] = drPopup[2];
                //        }
                //        break;
                //}
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private DataRow HienThiPopup(string tenPopup,string title, List<string> lstDieuKien = null)
        {
            PopupProcess process = new PopupProcess();
            DataRow dr = null;
            try
            {
                //Bat popup
                process.getPopupInformation(tenPopup,lstDieuKien);

                SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = title;
                win.Content = popup;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    dr = lstPopup[0];
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }
            return dr;
        }

        private void SetFormTheoMaPhanLoai(bool isFormPopup)
        {
            if (!isFormPopup)
            {
                txtMaPhanLoai.Tag = drPhanLoai["ID"].ToString();
                txtMaPhanLoai.Text = drPhanLoai["MA_PLOAI"].ToString();
                lblTenPhanLoai.Content = drPhanLoai["TEN_PLOAI"].ToString();
            }
            else
            {
                txtMaPhanLoai.Tag = drPhanLoai[1].ToString();
                txtMaPhanLoai.Text = drPhanLoai[2].ToString();
                lblTenPhanLoai.Content = drPhanLoai[3].ToString();
            }
            // Cấu trúc tài khoản
            LayDanhSachCauTrucTK(drPhanLoai["MA_KHANG_NBO"].ToString());
            txtTenTaiKhoan.Text = lblTenPhanLoai.Content.ToString();
            cmbTinhChatTK.SelectedIndex = lstSourceTinhChatTK.IndexOf(lstSourceTinhChatTK.FirstOrDefault(f => f.KeywordStrings.First().Equals(drPhanLoai["MA_TCHAT_GOC"].ToString())));
        }

        private void txtMaPhanLoai_LostFocus(object sender, RoutedEventArgs e)
        {
            if (drPhanLoai == null)
            {
                if (!LString.IsNullOrEmptyOrSpace(txtMaPhanLoai.Text))
                {
                    KeToanProcess process = new KeToanProcess();
                    DataTable dt = process.getThongTinMaPhanLoaiTheoMa(txtMaPhanLoai.Text);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (LString.IsNullOrEmptyOrSpace(dt.Rows[0]["MA_KHANG_NBO"].ToString()))
                        {
                            LMessage.ShowMessage("M.KeToan.TaiKhoan.ucTaiKhoanCT.LoiMaPhanLoaiKhongPhuHop", LMessage.MessageBoxType.Warning);

                            drPhanLoai = null;
                            txtMaPhanLoai.Text = "";
                            lblTenPhanLoai.Content = "";
                            txtMaPhanLoai.Tag = "";
                            txtMaPhanLoai.Focus();
                        }
                        else
                        {
                            drPhanLoai = dt.Rows[0];
                            SetFormTheoMaPhanLoai(false);
                        }
                    }
                    else
                    {
                        LMessage.ShowMessage("M.KeToan.PhanLoai.ucPhanLoaiCT.LoiKhongTonTaiMaPhanLoai", LMessage.MessageBoxType.Warning);
                        drPhanLoai = null;
                        txtMaPhanLoai.Text = "";
                        lblTenPhanLoai.Content = "";
                        txtMaPhanLoai.Tag = "";
                        txtMaPhanLoai.Focus();
                    }
                }
            }
            else
            {
                if (LString.IsNullOrEmptyOrSpace(txtMaPhanLoai.Text))
                {
                    drPhanLoai = null;
                    txtMaPhanLoai.Text = "";
                    lblTenPhanLoai.Content = "";
                    txtMaPhanLoai.Tag = "";
                }
                else
                {
                    if (drPhanLoai[2].ToString() != txtMaPhanLoai.Text)
                    {
                        if (!LString.IsNullOrEmptyOrSpace(txtMaPhanLoai.Text))
                        {
                            KeToanProcess process = new KeToanProcess();
                            DataTable dt = process.getThongTinMaPhanLoaiTheoMa(txtMaPhanLoai.Text);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                if (LString.IsNullOrEmptyOrSpace(dt.Rows[0]["MA_KHANG_NBO"].ToString()))
                                {
                                    LMessage.ShowMessage("M.KeToan.TaiKhoan.ucTaiKhoanCT.LoiMaPhanLoaiKhongPhuHop", LMessage.MessageBoxType.Warning);

                                    drPhanLoai = null;
                                    txtMaPhanLoai.Text = "";
                                    lblTenPhanLoai.Content = "";
                                    txtMaPhanLoai.Tag = "";
                                    txtMaPhanLoai.Focus();
                                }
                                else
                                {
                                    drPhanLoai = dt.Rows[0];
                                    SetFormTheoMaPhanLoai(false);
                                }
                            }
                            else
                            {
                                LMessage.ShowMessage("M.KeToan.PhanLoai.ucPhanLoaiCT.LoiKhongTonTaiMaPhanLoai", LMessage.MessageBoxType.Warning);
                                drPhanLoai = null;
                                txtMaPhanLoai.Text = "";
                                lblTenPhanLoai.Content = "";
                                txtMaPhanLoai.Tag = "";
                                txtMaPhanLoai.Focus();
                            }
                        }
                    }
                }
            }
        }
    }
}
