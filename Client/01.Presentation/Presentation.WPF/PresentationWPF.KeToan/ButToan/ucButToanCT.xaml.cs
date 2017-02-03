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
using System.ComponentModel;
namespace PresentationWPF.KeToan.ButToan
{
    /// <summary>
    /// Interaction logic for ucButToanCT.xaml
    /// </summary>
    public partial class ucButToanCT : UserControl
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
        List<AutoCompleteEntry> lstSourcePhanHe = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHinhThucGD = new List<AutoCompleteEntry>();

        public event EventHandler OnSavingCompleted;

        private string tthaiNvu = "";
        public string TthaiNvu
        {
            get { return tthaiNvu; }
            set { tthaiNvu = value; }
        }

        private int _idButToan = -1;

        private DataTable dtButToan = new DataTable();

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        #endregion

        #region Khoi tao
        public ucButToanCT()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KeToan;component/ButToan/ucCauTrucCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            
            raddtNgayNhap.Value = null;
            raddtNgayCNhat.Value = null;
            BindShortkey();
            InitCombobox();
            KhoiTaoDataSource();
            beforeAddNew();
        }

        public ucButToanCT(int id, string tthai, DatabaseConstant.Action action)
        {
            InitializeComponent();
            _idButToan = id;
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KeToan;component/ButToan/ucCauTrucCT.xaml", ref Toolbar, ref mnuMain);
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
                // Combobox phan he
                auto.GenAutoComboBox(ref lstSourcePhanHe, ref cmbMaPhanHe, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHANHE.getValue());

                //Combobox hinh thuc gd
                lstDK.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.HINH_THUC_GIAO_DICH_KT));
                auto.GenAutoComboBox(ref lstSourceHinhThucGD, ref cbbHinhThucGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK);
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
                dtButToan = new DataTable();
                dtButToan.Columns.Add("CHON", typeof(bool));
                dtButToan.Columns.Add("STT", typeof(int));
                dtButToan.Columns.Add("ID", typeof(int));
                dtButToan.Columns.Add("DINHKHOAN_MA", typeof(string));
                dtButToan.Columns.Add("DINHKHOAN_TEN", typeof(string));
                dtButToan.Columns.Add("MA_PHAN_LOAI", typeof(string));
                dtButToan.Columns.Add("TEN_PHAN_LOAI", typeof(string));
                dtButToan.Columns.Add("NHOM_DK", typeof(string));
                dtButToan.Columns.Add("CHUNGTU_MA", typeof(string));
                dtButToan.Columns.Add("CHUNGTU_TEN", typeof(string));

                raddgrDSButToan.ItemsSource = dtButToan.DefaultView;
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
            listLockId.Add(_idButToan);

            bool ret = process.UnlockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_PHAN_LOAI_CT,
                DatabaseConstant.Table.KT_PLOAI,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        private void SetEnabledAllControls(bool enable)
        {
            grbButToan.IsEnabled = enable;
            grbGiaoDich.IsEnabled = enable;
        }

        #endregion              
 
        #region Xu ly nghiep vu
        private void beforeView()
        {
            SetFormData();
            SetEnabledAllControls(false);
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_BUT_TOAN_CT);
        }

        private void beforeAddNew()
        {
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_BUT_TOAN_CT);
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
            CommonFunction.RefreshButton(Toolbar, action, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_BUT_TOAN_CT);
        }

        private void beforeModifyFromDetail()
        {
            // Yêu cầu lock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idButToan);

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
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_BUT_TOAN_CT);
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
                listLockId.Add(_idButToan);
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
                listLockId.Add(_idButToan);
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
                listLockId.Add(_idButToan);
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
                listLockId.Add(_idButToan);
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
                    if (_idButToan == -1)
                    {
                        // Lấy dữ liệu từ form
                        //GetFormData(ref obj, ref lstChiTiet, trangThai);
                        //ret = process.ThemGDChuyenDiaBan(obj, lstChiTiet);
                        _idButToan = ret.ID;
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
                        _idButToan = ret.ID;
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
                if (_idButToan == -1)
                {
                    // Lấy dữ liệu từ form
                    //GetFormData(ref obj, ref lstChiTiet, trangThai);
                    //ret = process.ThemGDChuyenDiaBan(obj, lstChiTiet);
                    _idButToan = ret.ID;
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
                    _idButToan = ret.ID;
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
                    arrayID[arrayID.Length - 1] = _idButToan;

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
                    arrayID[arrayID.Length - 1] = _idButToan;

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
                    arrayID[arrayID.Length - 1] = _idButToan;

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
                    arrayID[arrayID.Length - 1] = _idButToan;

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
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_BUT_TOAN_CT);
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
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_BUT_TOAN_CT);
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
            listLockId.Add(_idButToan);
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

                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_BUT_TOAN_CT);
                SetEnabledAllControls(false);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.DuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idButToan);

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

                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_BUT_TOAN_CT);
                SetEnabledAllControls(false);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.ThoaiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idButToan);

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

                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.KT_BUT_TOAN_CT);
                SetEnabledAllControls(false);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.TuChoiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idButToan);

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
            _idButToan = -1;
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
            txtMaGiaoDich.Focus();
        }

        private void btnCauTrucDienGiai_Click(object sender, RoutedEventArgs e)
        {
            //POPUP_DIEN_GIAI
            Presentation.Process.KeToanProcess ketoanProcess = new Presentation.Process.KeToanProcess();
            try
            {
                //Bat popup
                var process = new PopupProcess();

                List<string> lstDieuKien = new List<string>();

                //process.getPopupInformation("POPUP_TP_CAUTRUC", lstDieuKien);
                process.getPopupInformation("POPUP_DIEN_GIAI");
                SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey("U.KeToan.ButToan.ucButToanCT.DanhSachDienGiai");
                win.Content = popup;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow dr = lstPopup[0];
                    txtCauTrucDienGiai.Text = dr[2].ToString();
                    lblTenCauTrucDienGiai.Content = dr[3].ToString();
                    txtCauTrucDienGiai.Tag = dr[1].ToString();
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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            ucNhomDinhKhoan uc = new ucNhomDinhKhoan();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Title = LLanguage.SearchResourceByKey("U.KeToan.ButToan.ucButToanCT.ChiTietNhomDinhKhoan");
            window.Content = uc;
            window.ShowDialog();
            try
            {
                if (uc.insertValue == true)
                {
                    if (uc.dtSource != null)
                    {
                        if (this.raddgrDSButToan.GroupDescriptors.Count > 0)
                        {
                            this.raddgrDSButToan.GroupDescriptors.RemoveAt(0);
                        }
                        ImportRows(ref dtButToan, uc.dtSource);
                        this.raddgrDSButToan.GroupDescriptors.Add(new ColumnGroupDescriptor()
                            {
                                Column = this.raddgrDSButToan.Columns["NHOM_DK"],
                                SortDirection = ListSortDirection.Ascending
                            });
                    }
                    raddgrDSButToan.Rebind();
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                window = null;
            }
        }

        private void ImportRows(ref DataTable dtSource, DataTable dtAdd)
        {
            DataRow[] drRemove = dtSource.Select("CHON=1");
            if (drRemove.Length > 0)
            {
                foreach (DataRow dr in drRemove)
                {
                    dtSource.Rows.Remove(dr);
                }
            }

            foreach (DataRow dr in dtAdd.Rows)
            {
                dtSource.ImportRow(dr);
            }

            //Set lại số STT
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                dtSource.Rows[i]["STT"] = i + 1;
                dtSource.Rows[i]["CHON"] = false;
            }
        }

        private void chkDSButToan_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dtButToan.Rows.Count; i++)
            {
                dtButToan.Rows[i]["CHON"] = chkDSButToan.IsChecked;
            }
        }

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            ucNhomDinhKhoan uc = new ucNhomDinhKhoan();
            for (int i = 0; i < dtButToan.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dtButToan.Rows[i]["CHON"]) == true)
                {
                    uc.dtSource.ImportRow(dtButToan.Rows[i]);
                }
            }
            if (uc.dtSource.Rows.Count == 0)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                uc = null;
                return;
            }
            uc.idSinhMa = findMinId(dtButToan);
            window.Content = uc;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Title = "Chi tiết nhóm định khoản";
            window.ShowDialog();
            if (uc.dtSource != null)
            {
                DataTable dt = dtButToan;
                ImportRows(ref dt, uc.dtSource);
            }
            uc = null;
            raddgrDSButToan.Rebind();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int rowCount = dtButToan.Rows.Count;
            for (int i = rowCount - 1; i >= 0; i--)
            {
                if (Convert.ToBoolean(dtButToan.Rows[i]["CHON"]) == true)
                {
                    dtButToan.Rows.RemoveAt(i);

                }
            }
            for (int i = 0; i < dtButToan.Rows.Count; i++)
            {
                dtButToan.Rows[i]["STT"] = i + 1;
            }
        }

        /// <summary>
        /// Tìm id min trong datatable
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private int findMinId(DataTable dt)
        {
            DataRow[] dr = dt.Select("ID=MIN(ID)");
            if (dr.Length > 0)
            {
                return Convert.ToInt32(dr[0]["ID"]);
            }
            return 0;
        }
    }
}
