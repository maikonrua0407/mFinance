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
using System.Data;
using Presentation.Process.NhanSuServiceRef;
using Telerik.Windows.Data;
using System.Collections;
using System.Reflection;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls;
using Presentation.Process.QuanTriHeThongServiceRef;

namespace PresentationWPF.NhanSu.HopDong
{
    /// <summary>
    /// Interaction logic for ucPhuLucHopDong.xaml
    /// </summary>
    public partial class ucPhuLucHopDong : UserControl
    {
        #region Khai bao
        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SubmitCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        List<AutoCompleteEntry> lstSourcePhanHe = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKhaiBaoLichDonViTinh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiKhaiBaoLich = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHinhThucBacThang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKhaiBaoLichKyHan = new List<AutoCompleteEntry>();

        public static DatabaseConstant.Module Module = DatabaseConstant.Module.DMDC;
        public static DatabaseConstant.Function Function = DatabaseConstant.Function.DC_LAI_SUAT_CT;
        public static DatabaseConstant.Table Table = DatabaseConstant.Table.HT_LICH;
        public DatabaseConstant.Action Action;

        public event EventHandler OnSavingCompleted;

        private int id = 0;
        public string formCase = null;
        bool isLoaded = false;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string tthaiNvu = "";
        public string TthaiNvu
        {
            get { return tthaiNvu; }
            set { tthaiNvu = value; }
        }
        DataTable dtKhaiBaoLichCT = new DataTable();
        #endregion

        #region Khoi tao
        public ucPhuLucHopDong()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("PresentationWPF/NhanSu.;component/HopDong/ucPhuLucHopDong.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }

            //InitEventHandler();

            BindShortkey();

            //HideControl();

            // Refresh buttons
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_LAI_SUAT_CT);
        }
        #endregion

        #region Dang ky hot key, shortcut key

        /// <summary>
        /// Định nghĩa phím tắt
        /// </summary>
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
                        key = new KeyBinding(ucPhuLucHopDong.HoldCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(ucPhuLucHopDong.ModifyCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.Shift);
                        key = new KeyBinding(ucPhuLucHopDong.DeleteCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.A, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucPhuLucHopDong.ApproveCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucPhuLucHopDong.RefuseCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.C, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucPhuLucHopDong.CancelCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(ucPhuLucHopDong.HelpCommand, keyg);
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
            e.CanExecute = tlbHold.IsEnabled;
        }
        private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onHold();
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbSubmit.IsEnabled;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onSave();
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

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbApprove.IsEnabled;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onApprove();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbRefuse.IsEnabled;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onRefuse();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbCancel.IsEnabled;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onCancel();
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
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                onHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                beforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                beforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                beforeRefuse();
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
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                onHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                beforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                beforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                beforeRefuse();
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

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void onHelp()
        {
        }

        /// <summary>
        /// Đóng form
        /// </summary>
        private void onClose()
        {
            Release();
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        /// <summary>
        /// Sự kiện load cho form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isLoaded)
            {
                LoadCombobox();
                ResetForm();
                InitEventHandler();
                txtTenLS.Focus();
                if (Action == DatabaseConstant.Action.XEM)
                    beforeView();
                else if (Action == DatabaseConstant.Action.SUA)
                    beforeModifyFromDetail();
                else
                    beforeAddNew();
                HideControl();
                isLoaded = true;
            }
        }

        /// <summary>
        /// Sự kiện unload cho form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Release();
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
            listLockId.Add(id);

            bool ret = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_LAI_SUAT_CT,
                DatabaseConstant.Table.HT_LICH,
                DatabaseConstant.Action.SUA,
                listLockId);
            id = 0;
            isLoaded = false;
        }

        /// <summary>
        /// Khởi tạo các combobox
        /// </summary>
        private void LoadCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            cmbPhanHe.Items.Clear();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DANH_MUC_PHAN_HE.getValue());
            lstSourcePhanHe = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstSourcePhanHe, ref cmbPhanHe, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, DatabaseConstant.Module.TDVM.getValue());
            // Hiện tại chỉ thực hiện cho HDVO, TDVM
            auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHe, DatabaseConstant.Module.QTHT.getValue());
            auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHe, DatabaseConstant.Module.DMDC.getValue());
            auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHe, DatabaseConstant.Module.KHTV.getValue());
            auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHe, DatabaseConstant.Module.GDKT.getValue());
            auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHe, DatabaseConstant.Module.TDTT.getValue());
            auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHe, DatabaseConstant.Module.BHTH.getValue());
            auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHe, DatabaseConstant.Module.NSTL.getValue());
            auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHe, DatabaseConstant.Module.KTDL.getValue());

            cmbLoaiTien.Items.Clear();
            lstDieuKien = new List<string>();
            lstSourceLoaiTien = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TIENTE.getValue(), lstDieuKien, ClientInformation.MaDongNoiTe);

            cmbLSDonViTinh.Items.Clear();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.TAN_SUAT.getValue());
            lstSourceKhaiBaoLichDonViTinh = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstSourceKhaiBaoLichDonViTinh, ref cmbLSDonViTinh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, BusinessConstant.TAN_SUAT.NAM.layGiaTri());

        }

        /// <summary>
        /// InitEventHandler
        /// </summary>
        private void InitEventHandler()
        {
            
        }

        /// <summary>
        /// thiết lập hiển thị cho các control
        /// </summary>
        private void HideControl()
        {
            this.Cursor = Cursors.Wait;
            try
            {
                if (formCase == null)
                {
                    formCase = ClientInformation.FormCase;
                }
                if (!string.IsNullOrEmpty(formCase))
                {
                    HeThong hethong = new HeThong();
                    ArrayList arr = new ArrayList();
                    arr = hethong.SetVisibleControl("PresentationWPF.KhaiBaoLich.KhaiBaoLich.ucPhuLucHopDong", formCase);
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
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Đưa form về trạng thái mặc định
        /// </summary>
        private void ResetForm()
        {
            tthaiNvu = "";
            txtMaLS.Text = string.Empty;
            txtTenLS.Text = string.Empty;
            txtNgayLap.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri());
            txtNguoiCapNhat.Text = ClientInformation.TenDangNhap;
            lblTrangThai.Content = "";
            
            titemThongTinChung.Focus();
        }

        #endregion

        #region Xu ly nghiep vu

        /// <summary>
        /// Sự kiện load dữ liệu lên form
        /// </summary>
        private void SetFormData()
        {
            NhanSuProcess NhanSuProcess = new NhanSuProcess();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Đưa dữ liệu từ form vào object HT_LICH
        /// </summary>
        private void GetFormData(ref HT_LICH obj, ref List<HT_LICH> lst)
        {
            if (id != 0)
            {
                obj.ID = id;
            }
            

            return;
        }

        /// <summary>
        /// Kiểm tra các thông tin nhập vào trước khi lưu dữ liệu
        /// </summary>
        /// <returns></returns>
        private bool Validation()
        {
            
            return true;
        }

        /// <summary>
        /// Trước khi xem
        /// </summary>
        public void beforeView()
        {
            SetFormData();
            formCase = "XEM";
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_LAI_SUAT_CT);
        }

        /// <summary>
        /// Trước khi thêm mới
        /// </summary>
        private void beforeAddNew()
        {
            id = 0;
            SetFormData();
            formCase = "MANAGE";
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_LAI_SUAT_CT);
        }

        /// <summary>
        /// Trước khi sửa từ chi tiết
        /// </summary>
        private void beforeModifyFromDetail()
        {
            // Yêu cầu lock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool ret = process.LockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_LAI_SUAT_CT,
                DatabaseConstant.Table.HT_LICH,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Nếu lock thành công >> cho phép sửa
            if (ret)
            {
                SetFormData();
                formCase = "MANAGE";
                HideControl();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_LAI_SUAT_CT);
            }
            else
            {
                LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
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
                listLockId.Add(id);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_LAI_SUAT_CT,
                    DatabaseConstant.Table.HT_LICH,
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

        /// <summary>
        /// Trước khi duyệt
        /// </summary>
        private void beforeApprove()
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

                bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_LAI_SUAT_CT,
                    DatabaseConstant.Table.HT_LICH,
                    DatabaseConstant.Action.DUYET,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    // Gọi tới hàm duyệt dữ liệu
                    onApprove();
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

        /// <summary>
        /// Trước khi thoái duyệt
        /// </summary>
        private void beforeCancel()
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

                bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_LAI_SUAT_CT,
                    DatabaseConstant.Table.HT_LICH,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    // Gọi tới hàm thoái duyệt dữ liệu
                    onCancel();
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

        /// <summary>
        /// Trước khi từ chối
        /// </summary>
        private void beforeRefuse()
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

                bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_LAI_SUAT_CT,
                    DatabaseConstant.Table.HT_LICH,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    // Gọi tới hàm xóa dữ liệu
                    onRefuse();
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

        /// <summary>
        /// Lưu dữ liệu (hay trình duyệt)
        /// </summary>
        private void onSave()
        {
            string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            if (Validation())
            {
                NhanSuProcess NhanSuProcess = new NhanSuProcess();
                try
                {
                    // Dữ liệu truyền vào và dữ liệu trả về
                    HT_LICH obj = new HT_LICH();
                    List<HT_LICH> lst = new List<HT_LICH>();

                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (id == 0)
                    {
                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref lst);
                        obj.TTHAI_NVU = trangThai;

                        // Set các thông tin khác                        
                        obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                        obj.NGAY_NHAP = LDateTime.GetCurrentDate(ApplicationConstant.defaultDateTimeFormat);
                        List<HT_LICH> lstLS = new List<HT_LICH>();
                        lstLS.Add(obj);
                        List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                        
                        // xử lý

                        afterAddNew(lstLS.First());
                    }
                    // Nếu là lưu tạm khi sửa
                    // Hoặc lưu tạm khi sửa sau duyệt
                    // Hoặc sửa
                    else
                    {
                        // Lấy thông tin cũ
                        //obj = NhanSuProcess.GetKhaiBaoLichByID(id);

                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref lst);
                        obj.TTHAI_NVU = trangThai;
                        List<HT_LICH> lstLS = new List<HT_LICH>();
                        lstLS.Add(obj);

                        List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                        // xử lý
                        afterModify(lstLS.First());
                    }
                }
                catch (System.Exception ex)
                {
                    if (id > 0)
                    {
                        // Yêu cầu Unlock bản ghi cần sửa
                        UtilitiesProcess process = new UtilitiesProcess();
                        List<int> listLockId = new List<int>();
                        listLockId.Add(id);

                        bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                            DatabaseConstant.Function.DC_LAI_SUAT_CT,
                            DatabaseConstant.Table.HT_LICH,
                            DatabaseConstant.Action.SUA,
                            listLockId);
                    }
                    this.Cursor = Cursors.Arrow;
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                finally
                {
                    NhanSuProcess = null;
                }
            }
        }

        /// <summary>
        /// Lưu tạm dữ liệu
        /// </summary>
        private void onHold()
        {
            string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));

            if (Validation())
            {
                NhanSuProcess NhanSuProcess = new NhanSuProcess();
                try
                {
                    // Dữ liệu truyền vào và dữ liệu trả về
                    HT_LICH obj = new HT_LICH();
                    List<HT_LICH> lst = new List<HT_LICH>();

                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (id == 0)
                    {
                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref lst);
                        obj.TTHAI_NVU = trangThai;

                        // Set các thông tin khác                        
                        obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                        obj.NGAY_NHAP = LDateTime.GetCurrentDate(ApplicationConstant.defaultDateTimeFormat);
                        List<HT_LICH> lstLS = new List<HT_LICH>();
                        lstLS.Add(obj);
                        List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                        // xử lý
                        afterModify(lstLS.First());
                    }
                    // Nếu là lưu tạm khi sửa
                    // Hoặc lưu tạm khi sửa sau duyệt
                    // Hoặc sửa
                    else
                    {
                        // Lấy thông tin cũ
                        

                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref lst);
                        obj.TTHAI_NVU = trangThai;
                        List<HT_LICH> lstLS = new List<HT_LICH>();
                        lstLS.Add(obj);
                        List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                        // xử lý
                        afterModify(lstLS.First());
                    }
                }
                catch (System.Exception ex)
                {
                    if (id > 0)
                    {
                        // Yêu cầu Unlock bản ghi cần sửa
                        UtilitiesProcess process = new UtilitiesProcess();
                        List<int> listLockId = new List<int>();
                        listLockId.Add(id);

                        bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                            DatabaseConstant.Function.DC_LAI_SUAT_CT,
                            DatabaseConstant.Table.HT_LICH,
                            DatabaseConstant.Action.SUA,
                            listLockId);
                    }
                    this.Cursor = Cursors.Arrow;
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                finally
                {
                    NhanSuProcess = null;
                }
            }
        }

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        private void onDelete()
        {
            NhanSuProcess NhanSuProcess = new NhanSuProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = id;
                // Dữ liệu truyền vào và dữ liệu trả về
                HT_LICH obj = new HT_LICH();
                obj.ID = id;
                List<HT_LICH> lstLS = new List<HT_LICH>();
                lstLS.Add(obj);
                List<HT_LICH> lst = new List<HT_LICH>();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                // xử lý
                bool ret = false;

                afterDelete(ret);
            }
            catch (System.Exception ex)
            {
                if (id > 0)
                {
                    // Yêu cầu Unlock bản ghi cần sửa
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);

                    bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_LAI_SUAT_CT,
                        DatabaseConstant.Table.HT_LICH,
                        DatabaseConstant.Action.SUA,
                        listLockId);
                }
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Duyệt dữ liệu
        /// </summary>
        private void onApprove()
        {
            NhanSuProcess NhanSuProcess = new NhanSuProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = id;
                // Dữ liệu truyền vào và dữ liệu trả về
                HT_LICH obj = new HT_LICH();
                obj.ID = id;
                List<HT_LICH> lst = new List<HT_LICH>();
                GetFormData(ref obj, ref lst);
                List<HT_LICH> lstLS = new List<HT_LICH>();
                lstLS.Add(obj);
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                // xử lý
                bool ret = false;

                afterApprove(ret);
            }
            catch (System.Exception ex)
            {
                if (id > 0)
                {
                    // Yêu cầu Unlock bản ghi cần sửa
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);

                    bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_LAI_SUAT_CT,
                        DatabaseConstant.Table.HT_LICH,
                        DatabaseConstant.Action.SUA,
                        listLockId);
                }
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Thoái duyệt dữ liệu
        /// </summary>
        private void onCancel()
        {
            NhanSuProcess NhanSuProcess = new NhanSuProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = id;
                // Dữ liệu truyền vào và dữ liệu trả về
                HT_LICH obj = new HT_LICH();
                obj.ID = id;
                List<HT_LICH> lstLS = new List<HT_LICH>();
                lstLS.Add(obj);
                List<HT_LICH> lst = new List<HT_LICH>();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                // xử lý
                bool ret = false;

                afterCancel(ret);
            }
            catch (System.Exception ex)
            {
                if (id > 0)
                {
                    // Yêu cầu Unlock bản ghi cần sửa
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);

                    bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_LAI_SUAT_CT,
                        DatabaseConstant.Table.HT_LICH,
                        DatabaseConstant.Action.SUA,
                        listLockId);
                }
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Từ chối duyệt dữ liệu
        /// </summary>
        private void onRefuse()
        {
            NhanSuProcess NhanSuProcess = new NhanSuProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = id;
                // Dữ liệu truyền vào và dữ liệu trả về
                HT_LICH obj = new HT_LICH();
                obj.ID = id;
                List<HT_LICH> lstLS = new List<HT_LICH>();
                lstLS.Add(obj);
                List<HT_LICH> lst = new List<HT_LICH>();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                // xử lý
                bool ret = false;

                afterRefuse(ret);
            }
            catch (System.Exception ex)
            {
                if (id > 0)
                {
                    // Yêu cầu Unlock bản ghi cần sửa
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);

                    bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_LAI_SUAT_CT,
                        DatabaseConstant.Table.HT_LICH,
                        DatabaseConstant.Action.SUA,
                        listLockId);
                }
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Sau khi thêm mới
        /// </summary>
        /// <param name="ret"></param>
        private void afterAddNew(HT_LICH ret)
        {
            if (ret != null)
            {
                LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                if (cbMultiAdd.IsChecked == true)
                {
                    beforeAddNew();
                }
                else if (!DatabaseConstant.CLOSE_DETAIL_FORM)
                {
                    id = ret.ID;
                    TthaiNvu = ret.TTHAI_NVU;
                    txtMaLS.Text = ""; // set mã
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(ret.TTHAI_NVU);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(ret.TTHAI_BGHI);

                    formCase = "XEM";
                    HideControl();
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_LAI_SUAT_CT);

                    titemThongTinChung.Focus();
                }
                else
                {
                    onClose();
                }
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.ThemKhongThanhCong", LMessage.MessageBoxType.Error);
            }
        }

        /// <summary>
        /// Sau khi sửa
        /// </summary>
        /// <param name="ret"></param>
        private void afterModify(HT_LICH ret)
        {
            if (ret != null)
            {
                LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                id = ret.ID;
                TthaiNvu = ret.TTHAI_NVU;
                txtMaLS.Text = ""; // set mã
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(ret.TTHAI_NVU);
                txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(ret.TTHAI_BGHI);

                formCase = "XEM";
                HideControl();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_LAI_SUAT_CT);

                titemThongTinChung.Focus();
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.CapNhatKoThanhCong", LMessage.MessageBoxType.Error);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_LAI_SUAT_CT,
                DatabaseConstant.Table.HT_LICH,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Sau khi xóa
        /// </summary>
        /// <param name="ret"></param>
        private void afterDelete(bool ret)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);

                //SetEnabledAllControls(false);
                //SetEnabledRequiredControls(false);
                //CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_LAI_SUAT_CT);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);
            List<int> listUnlockId = new List<int>();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_LAI_SUAT_CT,
                DatabaseConstant.Table.HT_LICH,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Đóng cửa sổ chi tiết sau khi xóa
            onClose();
        }

        /// <summary>
        /// Sau khi duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void afterApprove(bool ret)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);

                TthaiNvu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);

                formCase = "XEM";
                HideControl();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_LAI_SUAT_CT);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.DuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_LAI_SUAT_CT,
                DatabaseConstant.Table.HT_LICH,
                DatabaseConstant.Action.DUYET,
                listLockId);
        }

        /// <summary>
        /// Sau khi thoái duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void afterCancel(bool ret)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);

                TthaiNvu = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);

                formCase = "XEM";
                HideControl();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_LAI_SUAT_CT);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.ThoaiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_LAI_SUAT_CT,
                DatabaseConstant.Table.HT_LICH,
                DatabaseConstant.Action.THOAI_DUYET,
                listLockId);
        }

        /// <summary>
        /// Sau khi từ chối
        /// </summary>
        /// <param name="ret"></param>
        private void afterRefuse(bool ret)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);

                TthaiNvu = BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);

                formCase = "XEM";
                HideControl();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_LAI_SUAT_CT);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.TuChoiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_LAI_SUAT_CT,
                DatabaseConstant.Table.HT_LICH,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                listLockId);
        }

        #endregion
    }
}
