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
using Telerik.Windows.Controls.GridView;

namespace PresentationWPF.KeToan.CauTruc
{
    /// <summary>
    /// Interaction logic for ucCauTrucCT.xaml
    /// </summary>
    public partial class ucCauTrucCT : UserControl
    {
         /// <summary>
        /// Khai bao
        /// </summary>
        #region Khai bao
        public static RoutedCommand ImportCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand CloneCommand = new RoutedCommand();
        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand CashStmtCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand PreviewCommand = new RoutedCommand();
        public static RoutedCommand ViewCommand = new RoutedCommand();
        public static RoutedCommand ExportCommand = new RoutedCommand();
        public static RoutedCommand SearchCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        // Source combobox
        List<AutoCompleteEntry> lstSourceLoaiCauTruc = new List<AutoCompleteEntry>();

        public event EventHandler OnSavingCompleted;

        private string tthaiNvu = "";
        public string TthaiNvu
        {
            get { return tthaiNvu; }
            set { tthaiNvu = value; }
        }

        private int _idCauTruc = -1;

        private DataTable dtCauTruc = new DataTable();

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        #endregion

        #region Khoi tao
        public ucCauTrucCT()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KeToan;component/TaiKhoan/ucCauTrucCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            raddtNgayApDung.Value = LDateTime.StringToDate(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            raddtNgayNhap.Value = null;
            raddtNgayCNhat.Value = null;
            BindShortkey();
            InitCombobox();
            KhoiTaoDataSource();
            beforeAddNew();
        }

        public ucCauTrucCT(int id, string tthai, DatabaseConstant.Action action)
        {
            InitializeComponent();
            _idCauTruc = id;
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KeToan;component/TaiKhoan/ucCauTrucCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }

            BindShortkey();
            InitCombobox();
            SetFormData();
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
                //Loại tài khoản
                lstDK.Clear();
                lstDK.Add(DatabaseConstant.DanhMuc.LOAI_CAU_TRUC_TK.getValue());
                auto.GenAutoComboBox(ref lstSourceLoaiCauTruc, ref cbbLoaiCauTruc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK);
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

        private void KhoiTaoDataSource()
        {
            try
            {
                KeToanProcess process = new KeToanProcess();
                dtCauTruc.Columns.Add("ID", typeof(int));
                dtCauTruc.Columns.Add("CHON", typeof(bool));
                dtCauTruc.Columns.Add("STT", typeof(int));
                dtCauTruc.Columns.Add("MA_TPHAN", typeof(string));
                dtCauTruc.Columns.Add("TEN_TPHAN", typeof(string));
                DataRow dr = dtCauTruc.NewRow();
                dr["STT"] = 1;
                dr["CHON"] = false;
                dtCauTruc.Rows.Add(dr);
                grThanhPhanCauTruc.ItemsSource = dtCauTruc.DefaultView;
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
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
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.H, ModifierKeys.Control);
                        key = new KeyBinding(HoldCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
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
            listLockId.Add(_idCauTruc);

            bool ret = process.UnlockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        private void SetEnabledAllControls(bool enable)
        {
            grbThongTin.IsEnabled = enable;
            grbThanhPhan.IsEnabled = enable;
        }

        #endregion              
 
        #region Xu ly nghiep vu
        private void beforeView()
        {
            SetFormData();
            SetEnabledAllControls(false);
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu);
        }

        private void beforeAddNew()
        {
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu);
            lblTrangThai.Content = "";
            raddtNgayNhap.Value = null;
            raddtNgayCNhat.Value = null;
            
        }

        private void beforeModifyFromList(DatabaseConstant.Action action)
        {
            if (action == DatabaseConstant.Action.XEM)
            {
                SetEnabledAllControls(false);
            }
            CommonFunction.RefreshButton(Toolbar, action, tthaiNvu);
        }

        private void beforeModifyFromDetail()
        {
            // Yêu cầu lock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idCauTruc);

            bool ret = process.LockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Nếu lock thành công >> cho phép sửa
            if (ret)
            {
                SetEnabledAllControls(true);
                SetFormData();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu);
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
                listLockId.Add(_idCauTruc);
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

        private void beforeApprove()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_idCauTruc);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.GDKT,
                    DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                    DatabaseConstant.Table.KT_PLOAI,
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

        private void beforeCancel()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_idCauTruc);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.GDKT,
                    DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                    DatabaseConstant.Table.KT_PLOAI,
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
                listLockId.Add(_idCauTruc);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.GDKT,
                    DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                    DatabaseConstant.Table.KT_PLOAI,
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
            string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.TRINH_DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            if (Validation())
            {
                KeToanProcess process = new KeToanProcess();
                try
                {
                    Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN obj = new Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN();
                    List<Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN_CTIET> lstChiTiet = new List<Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN_CTIET>();
                    // Dữ liệu truyền vào và dữ liệu trả về
                    Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN ret = new Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN();
                    Mouse.OverrideCursor = Cursors.Wait;
                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (_idCauTruc == -1)
                    {
                        // Lấy dữ liệu từ form
                        //GetFormData(ref obj, ref lstChiTiet, trangThai);
                        //ret = process.ThemGDChuyenDiaBan(obj, lstChiTiet);
                        _idCauTruc = ret.ID;
                        tthaiNvu = trangThai;
                        Mouse.OverrideCursor = Cursors.Arrow;
                        afterAddNew(ret);
                    }
                    // Nếu là lưu tạm khi sửa
                    // Hoặc lưu tạm khi sửa sau duyệt
                    // Hoặc sửa
                    else
                    {
                        // Lấy dữ liệu từ form
                        //GetFormData(ref obj, ref lstChiTiet, trangThai);
                        //ret = process.SuaGDChuyenDiaBan(obj, lstChiTiet, DatabaseConstant.Action.SUA);
                        _idCauTruc = ret.ID;
                        tthaiNvu = trangThai;
                        Mouse.OverrideCursor = Cursors.Arrow;
                        afterModify(ret);
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
        /// Lưu tạm dữ liệu
        /// </summary>
        private void onHold()
        {
            string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            KeToanProcess process = new KeToanProcess();
            try
            {
                Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN obj = new Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN();
                List<Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN_CTIET> lstChiTiet = new List<Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN_CTIET>();
                // Dữ liệu truyền vào và dữ liệu trả về
                Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN ret = new Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN();
                Mouse.OverrideCursor = Cursors.Wait;
                // Nếu là lưu tạm hoặc thêm mới lần đầu
                if (_idCauTruc == -1)
                {
                    // Lấy dữ liệu từ form
                    //GetFormData(ref obj, ref lstChiTiet, trangThai);
                    //ret = process.ThemGDChuyenDiaBan(obj, lstChiTiet);
                    _idCauTruc = ret.ID;
                    tthaiNvu = trangThai;
                    Mouse.OverrideCursor = Cursors.Arrow;
                    afterAddNew(ret);
                }
                // Nếu là lưu tạm khi sửa
                // Hoặc lưu tạm khi sửa sau duyệt
                // Hoặc sửa
                else
                {
                    // Lấy dữ liệu từ form
                    //GetFormData(ref obj, ref lstChiTiet, trangThai);
                    //ret = process.SuaGDChuyenDiaBan(obj, lstChiTiet, DatabaseConstant.Action.SUA);
                    _idCauTruc = ret.ID;
                    tthaiNvu = trangThai;
                    Mouse.OverrideCursor = Cursors.Arrow;
                    afterModify(ret);
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
                process = null;
            }
        }

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        private void onDelete()
        {
            
                KeToanProcess process = new KeToanProcess();
                int[] arrayID = new int[0];
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    Array.Resize(ref arrayID, arrayID.Length + 1);
                    arrayID[arrayID.Length - 1] = _idCauTruc;

                    List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                    //ApplicationConstant.ResponseStatus ret = process.XoaGDChuyenDiaBan(arrayID, ref listClientResponseDetail);

                    //afterDelete(ret);
                }
                catch (System.Exception ex)
                {
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
            
                KeToanProcess process = new KeToanProcess();
                int[] arrayID = new int[0];
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    Array.Resize(ref arrayID, arrayID.Length + 1);
                    arrayID[arrayID.Length - 1] = _idCauTruc;

                    List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                    //ApplicationConstant.ResponseStatus ret = process.DuyetGDChuyenDiaBan(arrayID, ref listClientResponseDetail);
                    ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.THANH_CONG;
                    tthaiNvu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    afterApprove(ret);
                }
                catch (System.Exception ex)
                {
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
            
                KeToanProcess process = new KeToanProcess();
                int[] arrayID = new int[0];
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    Array.Resize(ref arrayID, arrayID.Length + 1);
                    arrayID[arrayID.Length - 1] = _idCauTruc;

                    List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                    //ApplicationConstant.ResponseStatus ret = process.ThoaiDuyetGDChuyenDiaBan(arrayID, ref listClientResponseDetail);
                    ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.THANH_CONG;
                    tthaiNvu = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                    afterCancel(ret);
                }
                catch (System.Exception ex)
                {
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
            
                KeToanProcess process = new KeToanProcess();
                int[] arrayID = new int[0];
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    Array.Resize(ref arrayID, arrayID.Length + 1);
                    arrayID[arrayID.Length - 1] = _idCauTruc;

                    List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                    //ApplicationConstant.ResponseStatus ret = process.TuChoiGDChuyenDiaBan(arrayID, ref listClientResponseDetail);
                    ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.THANH_CONG;

                    afterRefuse(ret, CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.TU_CHOI_DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu)));
                }
                catch (System.Exception ex)
                {
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
        private void afterAddNew(Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN ret)
        {
            //if (ret != null)
            //{
                
                LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);
                SetEnabledAllControls(false);
                //lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(ret.TTHAI_NVU);
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                //txtNguoiLap.Text = ret.NGUOI_NHAP;
                //raddtNgayNhap.Value = LDateTime.StringToDate(ret.NGAY_NHAP, "yyyyMMdd");
                //tthaiNvu = ret.TTHAI_NVU;

                if (cbMultiAdd.IsChecked == true)
                {
                    SetEnabledAllControls(true);
                    ResetForm();
                }
                else if (!DatabaseConstant.CLOSE_DETAIL_FORM)
                {
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu);
                }
                else
                {
                    onClose();
                }
            //}
            //else
            //{
            //    LMessage.ShowMessage("M.DungChung.ThemKhongThanhCong", LMessage.MessageBoxType.Error);
            //}
        }

        /// <summary>
        /// Sau khi sửa
        /// </summary>
        /// <param name="ret"></param>
        private void afterModify(Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN ret)
        {
            //if (ret != null)
            //{
                LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);
                //tthaiNvu = ret.TTHAI_NVU;
                SetEnabledAllControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu);
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                //lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(ret.TTHAI_NVU);
                //txtNguoiCapNhat.Text = ret.NGUOI_CNHAT;
                //raddtNgayCNhat.Value = LDateTime.StringToDate(ret.NGAY_CNHAT, "yyyyMMdd");
            //}
            //else
            //{
            //    LMessage.ShowMessage("M.DungChung.CapNhatKoThanhCong", LMessage.MessageBoxType.Warning);
            //}

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(ret.ID);

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
            //if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            //{
                LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
            //}
            //else
            //{
            //    LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
            //}

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idCauTruc);
            List<int> listUnlockId = new List<int>();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Đóng cửa sổ chi tiết sau khi xóa
            onClose();
        }

        /// <summary>
        /// Sau khi duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void afterApprove(ApplicationConstant.ResponseStatus ret)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);
                
                
                //TthaiNvu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);

                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu);
                SetEnabledAllControls(false);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.DuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idCauTruc);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.DUYET,
                listLockId);
        }

        /// <summary>
        /// Sau khi thoái duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void afterCancel(ApplicationConstant.ResponseStatus ret)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);

                tthaiNvu = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);

                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu);
                SetEnabledAllControls(false);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.ThoaiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idCauTruc);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.THOAI_DUYET,
                listLockId);
        }

        /// <summary>
        /// Sau khi từ chối
        /// </summary>
        /// <param name="ret"></param>
        private void afterRefuse(ApplicationConstant.ResponseStatus ret,string trangThai)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);
                tthaiNvu = trangThai;
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(trangThai);
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(trangThai);

                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu);
                SetEnabledAllControls(false);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.TuChoiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idCauTruc);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                listLockId);
        }

        /// <summary>
        /// Kiểm tra dữ liệu trước khi lưu
        /// </summary>
        /// <returns></returns>
        private bool Validation()
        {
            AutoComboBox au = new AutoComboBox();
            try
            {
                AutoCompleteEntry auLoaiCauTruc = au.getEntryByDisplayName(lstSourceLoaiCauTruc, ref cbbLoaiCauTruc);

                if (LString.IsNullOrEmptyOrSpace(txtMaCauTruc.Text))
                {
                    LMessage.ShowMessage("Mã cấu trúc tài khoản không được để trống", LMessage.MessageBoxType.Warning);
                    txtMaCauTruc.Focus();
                    return false;
                }
                else if (LString.IsNullOrEmptyOrSpace(txtTenCauTruc.Text))
                {
                    LMessage.ShowMessage("Tên cấu trúc tài khoản không được để trống", LMessage.MessageBoxType.Warning);
                    txtTenCauTruc.Focus();
                    return false;
                }
                else if (auLoaiCauTruc == null)
                {
                    LMessage.ShowMessage("Loại cấu trúc không được để trống", LMessage.MessageBoxType.Warning);
                    cbbLoaiCauTruc.Focus();
                    return false;
                }
                else if (raddtNgayApDung.Value == null)
                {
                    LMessage.ShowMessage("Ngày áp dụng không được để trống", LMessage.MessageBoxType.Warning);
                    raddtNgayApDung.Focus();
                    return false;
                }
                else if (grThanhPhanCauTruc.Items.Count == 0)
                {
                    LMessage.ShowMessage("Chưa có thành phần nào trong cấu trúc tài khoản", LMessage.MessageBoxType.Warning);
                    return false;
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

        private void GetFormData(string tthai)
        {
            AutoComboBox au = new AutoComboBox();
            
            
        }

        private void ResetForm()
        {
            txtMaCauTruc.Text = "";
            txtTenCauTruc.Text = "";
            cbbLoaiCauTruc.SelectedIndex = 0;
            raddtNgayApDung.Value = LDateTime.StringToDate(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            grThanhPhanCauTruc.Items.Clear();

            _idCauTruc = -1;
            lblTrangThai.Content = "";
            tthaiNvu = "";
            txtTrangThai.Text = "";
            txtNguoiLap.Text = "";
            txtNguoiCapNhat.Text = "";
            raddtNgayNhap.Value = null;
            raddtNgayCNhat.Value = null;
        }
        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtMaCauTruc.Focus();
        }

        private void ribbtnDelete_Click(object sender, RoutedEventArgs e)
        {
            for (int i = dtCauTruc.Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToBoolean(dtCauTruc.Rows[i]["CHON"]) == true)
                {
                    dtCauTruc.Rows.RemoveAt(i);
                }
            }
            for (int i = 0; i < dtCauTruc.Rows.Count; i++)
            {
                dtCauTruc.Rows[i]["STT"] = i + 1;
            }
            TaoMaCauTruc();
        }

        private void ribbtnUp_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dr = (DataRowView)grThanhPhanCauTruc.SelectedItem;
            if (dr != null)
            {
                int stt = Convert.ToInt32(dr["STT"]);
                if (stt != 1)
                {
                    dtCauTruc.Rows[stt - 1]["STT"] = stt - 1;
                    dtCauTruc.Rows[stt - 2]["STT"] = stt;
                }
                DataView dv = dtCauTruc.DefaultView;
                dv.Sort = "STT ASC";
                DataTable dt = dv.ToTable();
                dtCauTruc.Rows.Clear();
                dtCauTruc = dt.Copy();
                grThanhPhanCauTruc.ItemsSource = dtCauTruc.DefaultView;
                TaoMaCauTruc();
            }
        }

        private void ribbtnDown_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dr = (DataRowView)grThanhPhanCauTruc.SelectedItem;
            if (dr != null)
            {
                int stt = Convert.ToInt32(dr["STT"]);
                if (stt != grThanhPhanCauTruc.Items.Count -1)
                {
                    dtCauTruc.Rows[stt - 1]["STT"] = stt + 1;
                    dtCauTruc.Rows[stt]["STT"] = stt;
                }
                DataView dv = dtCauTruc.DefaultView;
                dv.Sort = "STT ASC";
                DataTable dt = dv.ToTable();
                dtCauTruc.Rows.Clear();
                dtCauTruc = dt.Copy();
                grThanhPhanCauTruc.ItemsSource = dtCauTruc.DefaultView;
                TaoMaCauTruc();
            }
        }

        private void chkThanhPhan_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dtCauTruc.Rows.Count;i++ )
            {
                dtCauTruc.Rows[i]["CHON"] = chkThanhPhan.IsChecked;
            }
        }

        private void grThanhPhanCauTruc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                Presentation.Process.KeToanProcess ketoanProcess = new Presentation.Process.KeToanProcess();
                try
                {
                    //Bat popup
                    var process = new PopupProcess();
                    
                    List<string> lstDieuKien = new List<string>();
                    lstDieuKien.Add(LayDSThanhPhan());

                    process.getPopupInformation("POPUP_TP_CAUTRUC",lstDieuKien);
                    SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;

                    ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                    popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                    Window win = new Window();
                    win.Title = "Danh sách thành phần cấu trúc tài khoản";
                    win.Content = popup;
                    win.ShowDialog();
                    if (lstPopup != null && lstPopup.Count > 0)
                    {
                        DataRow dr = lstPopup[0];
                        DataRowView drCurrent = (DataRowView)grThanhPhanCauTruc.SelectedItem;
                        if (LString.IsNullOrEmptyOrSpace(drCurrent["MA_TPHAN"].ToString()))
                        {
                            // Them dong moi
                            DataRow drNew = dtCauTruc.NewRow();
                            drNew["STT"] = dtCauTruc.Rows.Count + 1;
                            drNew["CHON"] = false;
                            dtCauTruc.Rows.Add(drNew);
                        }
                        drCurrent["ID"] = dr[1]; //ID thanh phan
                        drCurrent["MA_TPHAN"] = dr[2]; //Ma thanh phan
                        drCurrent["TEN_TPHAN"] = dr[3]; //Ten thanh phan

                        lstPopup.Clear();
                        TaoMaCauTruc();
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
        }

        private string LayDSThanhPhan()
        {
            string dieuKien = "";
            for (int i = 0; i < dtCauTruc.Rows.Count;i++ )
            {
                dieuKien += dtCauTruc.Rows[i]["ID"].ToString() + ",";
            }
            dieuKien = dieuKien.Substring(0, dieuKien.Length - 1);
            if (dieuKien == "")
            {
                dieuKien += "''";
            }
            else
            {
                dieuKien = dieuKien.Substring(0, dieuKien.Length - 1);
            }
            return dieuKien;
        }

        private void TaoMaCauTruc()
        {
            txtCauTrucTK.Text = "";
            for (int i = 0; i < dtCauTruc.Rows.Count - 1;i++)
            {
                txtCauTrucTK.Text += "[" + dtCauTruc.Rows[i]["MA_TPHAN"].ToString() + "]";
            }
        }

        private void SapXep()
        {

        }
    }
}
