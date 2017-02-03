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
using PresentationWPF.CustomControl;
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using System.Data;
using Telerik.Windows.Controls;
using Presentation.Process;
using Presentation.Process.Common;

namespace PresentationWPF.KhachHang.ChuyenDiaBan
{
    /// <summary>
    /// Interaction logic for ucChuyenDiaBanDS.xaml
    /// </summary>
    public partial class ucChuyenDiaBanDS : UserControl
    {
        #region Khai bao
        public static RoutedCommand AddCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand ViewCommand = new RoutedCommand();
        public static RoutedCommand SearchCommand = new RoutedCommand();
        public static RoutedCommand ReloadCommand = new RoutedCommand();
        public static RoutedCommand ExportCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        List<AutoCompleteEntry> lstSourceLoaiGiayTo = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLyDoRa = new List<AutoCompleteEntry>();

        private DataTable dtSourceTree = new DataTable();

        //Lưu danh sách các item được chọn trong treeview
        private List<string> lstChecked = new List<string>();
        #endregion

        #region Khoi tao
        public ucChuyenDiaBanDS()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KhachHang;component/ChuyenDiaBan/ucChuyenDiaBanDS.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += tlbHotKey_Click;
            }
            BindHotkey();
            radpage.PageSize = (int)nudPageSize.Value;
            KhoiTaoCombobox();
            LoadDuLieu();
        }
        #endregion

        #region Dang ky hot key, shortcut key
        private void BindHotkey()
        {
            foreach (var child in Toolbar.Children)
            {
                if (child.GetType() == typeof(RibbonButton))
                {
                    RibbonButton tlb = (RibbonButton)child;
                    KeyBinding key = new KeyBinding();
                    string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.N, ModifierKeys.Control);
                        key = new KeyBinding(AddCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(ModifyCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.Shift);
                        key = new KeyBinding(DeleteCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.A, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ApproveCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(RefuseCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.C, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(CancelCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.W, ModifierKeys.Control);
                        key = new KeyBinding(ViewCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F, ModifierKeys.Control);
                        key = new KeyBinding(SearchCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F5, ModifierKeys.None);
                        key = new KeyBinding(ReloadCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.E, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ExportCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(HelpCommand, keyg);
                        key.Gesture = keyg;
                    }
                    if (key != null)
                        InputBindings.Add(key);
                }
            }
        }

        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbAdd.IsEnabled;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Them();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbModify.IsEnabled;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Sua();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbDelete.IsEnabled;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Xoa();
        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbApprove.IsEnabled;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Duyet();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbRefuse.IsEnabled;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TuChoi();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbCancel.IsEnabled;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ThoaiDuyet();
        }

        private void ViewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbView.IsEnabled;
        }
        private void ViewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Xem();
        }

        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbSearch.IsEnabled;
        }
        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            txtTimKiemNhanh.RaiseEvent(new RoutedEventArgs(Button.GotFocusEvent));
        }

        private void ReloadCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbReload.IsEnabled;
        }
        private void ReloadCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TimKiem();
        }

        private void ExportCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbExport.IsEnabled;
        }
        private void ExportCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            XuatExcel();
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

        /// <summary>
        /// Sự kiện hot key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            //RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = "";

            if (sender is RibbonButton)
                strTinhNang = ((RibbonButton)sender).Name.Substring(3, ((RibbonButton)sender).Name.Length - 3);
            else
                strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);


            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                Them();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                Sua();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                Xoa();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                Duyet();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                TuChoi();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                ThoaiDuyet();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
                Xem();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            {
                TimKiem();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
            {
                TimKiem();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
            {
                XuatExcel();
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

        /// <summary>
        /// Sự kiện double click trên data grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grKhachHangDS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Xem();
        }
        #endregion

        #region Xu ly giao dien
        /// <summary>
        /// Sự kiện tìm kiếm nhanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTimKiemNhanh_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtTimKiemNhanh.Text == LLanguage.SearchResourceByKey("U.DungChung.TimKiemNhanh"))
            {
                return;
            }
            PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grKhachHangDS, txtTimKiemNhanh.Text);
        }

        /// <summary>
        /// Sự kiện focus vào textbox tìm kiếm nhanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTimKiemNhanh_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtTimKiemNhanh.Text == LLanguage.SearchResourceByKey("U.DungChung.TimKiemNhanh"))
            {
                txtTimKiemNhanh.Text = "";
            }
        }

        /// <summary>
        /// Sự kiện rời focus khỏi textbox tìm kiếm nhanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTimKiemNhanh_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTimKiemNhanh.Text))
            {
                txtTimKiemNhanh.Text = LLanguage.SearchResourceByKey("U.DungChung.TimKiemNhanh");
            }
        }

        /// <summary>
        /// Xử lý sự kiện escape thoát form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        /// <summary>
        /// Sự kiện thay đổi số bản ghi trên một page hiển thị
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudPageSize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (grKhachHangDS != null && grKhachHangDS.ItemsSource != null)
            {
                DataTable dt = null;
                if (grKhachHangDS.ItemsSource is DataView)
                {
                    dt = ((DataView)grKhachHangDS.ItemsSource).Table;
                }
                else
                {
                    dt = grKhachHangDS.ItemsSource as DataTable;
                }

                if (dt != null)
                {
                    radpage.PageSize = (int)nudPageSize.Value;
                    grKhachHangDS.ItemsSource = dt.DefaultView;
                }
            }
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(grKhachHangDS);
        }

        /// <summary>
        /// Khởi tạo các datasource cho combobx
        /// </summary>
        private void KhoiTaoCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDK = new List<string>();

           // Loai giay to
            lstDK.Add(DatabaseConstant.DanhMuc.LOAI_GIAY_TO.getValue());
            auto.GenAutoComboBox(ref lstSourceLoaiGiayTo, ref cmbLoaiGiayTo, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK);

            //Load ly do ra khoi nhom
            lstDK.Clear();
            lstDK.Add(DatabaseConstant.DanhMuc.LY_DO_VAO_RA.getValue());
            auto.GenAutoComboBox(ref lstSourceLyDoRa, ref cmbLyDoRaKhoiNhom, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK, null);
        }

        /// <summary>
        /// Sự kiện thêm nodes chưa có vào tree khi mở rộng parent nodes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvwKhachHangDS_LoadOnDemand(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            RadTreeViewItem item = e.OriginalSource as RadTreeViewItem;
            bool check = tvwKhachHangDS.CheckedItems.Contains(item);
            if (!BuildTreeCungCap(item))
            {
                BuildTreeKhacCap(item);
            }
            item.IsLoadOnDemandEnabled = false;
            //item.IsChecked = check;
        }

        /// <summary>
        /// Add các nodes khác bảng csdl với node truyền vào
        /// </summary>
        /// <param name="itemRoot">Node cha</param>
        private void BuildTreeKhacCap(RadTreeViewItem itemRoot)
        {
            string[] parent = itemRoot.Tag.ToString().Split('/');
            string[] level = itemRoot.Tag.ToString().Split('#');
            string condition = "";
            if (level[0] == "0")
            {
                condition = "path like '" + (Convert.ToInt32(level[0]) + 1) + "#" + parent[parent.Length - 1].Substring(0, parent[parent.Length - 1].Length - 4) + "/%'";
            }
            else
            {
                condition = "path like '" + (Convert.ToInt32(level[0]) + 1) + "#" + level[1] + "/%'";
            }
            DataRow[] drChild = dtSourceTree.Select(condition).OrderBy(row => row[2]).ToArray();
            //DataRow[] drChild = dtSourceTree.Select(condition);
            foreach (DataRow dr in drChild)
            {
                RadTreeViewItem item = new RadTreeViewItem();
                item.Header = dr["ten"];
                item.Tag = dr["path"];
                itemRoot.Items.Add(item);
                dtSourceTree.Rows.Remove(dr);
            }
        }

        /// <summary>
        /// Add các nodes cùng bảng csdl với node truyền vào
        /// </summary>
        /// <param name="itemRoot">Node cha</param>
        /// <returns></returns>
        private bool BuildTreeCungCap(RadTreeViewItem itemRoot)
        {
            bool kq = true;
            string[] parent = itemRoot.Tag.ToString().Split('/');
            string[] level = itemRoot.Tag.ToString().Split('#');
            if (level[0] == "0")
            {
                DataRow[] drChild = dtSourceTree.Select("path like '" + level[0] + "#" + parent[parent.Length - 1].Substring(0, parent[parent.Length - 1].Length - 4) + "/%'");
                if (drChild.Length > 0)
                {
                    foreach (DataRow dr in drChild)
                    {
                        RadTreeViewItem item = new RadTreeViewItem();
                        item.Header = dr["ten"];
                        item.Tag = dr["path"];
                        itemRoot.Items.Add(item);
                        dtSourceTree.Rows.Remove(dr);
                    }
                    kq = true;
                }
                else
                {
                    kq = false;
                }
            }
            else
            {
                kq = false;
            }
            return kq;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < grKhachHangDS.Items.Count; i++)
            {
                DataRowView dr = (DataRowView)grKhachHangDS.Items[i];
                dr["CHON"] = chkKhachHangDS.IsChecked;
            }
        }
        #endregion

        #region Xu ly nghiep vu
        /// <summary>
        /// Xử lý sự kiện thêm
        /// </summary>
        private void Them()
        {
            Window window = new Window();
            ucChuyenDiaBanCT uc = new ucChuyenDiaBanCT();
            uc.TthaiNvu = "";
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.KH_CHUYEN_DIA_BAN);
            window.Content = uc;
            uc.OnSavingCompleted += new EventHandler(OnSavingCompleted);
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }

        /// <summary>
        /// Xử lý sự kiện sửa
        /// </summary>
        private void Sua()
        {
            try
            {
                DataRowView dr=null;
                for (int i = 0; i < grKhachHangDS.Items.Count; i++)
                {
                    if (Convert.ToBoolean(((DataRowView)grKhachHangDS.Items[i])["CHON"]) == true)
                    {
                        dr = (DataRowView)grKhachHangDS.Items[i];
                        break;
                    }
                }
                if (!LObject.IsNullOrEmpty(dr))
                {
                    Window window = new Window();
                    ucChuyenDiaBanCT uc = new ucChuyenDiaBanCT(Convert.ToInt32(dr["ID"]), dr["TTHAI_NVU"].ToString(), DatabaseConstant.Action.SUA);
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.KH_CHUYEN_DIA_BAN);
                    window.Content = uc;
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.ShowDialog();
                }
                
            }
            catch (System.Exception ex)
            {

            }
        }

        /// <summary>
        /// Xử lý sự kiện sửa
        /// </summary>
        private void Xem()
        {
            try
            {
                DataRowView dr=null;
                for (int i = 0; i < grKhachHangDS.Items.Count; i++)
                {
                    if (Convert.ToBoolean(((DataRowView)grKhachHangDS.Items[i])["CHON"]) == true)
                    {
                        dr = (DataRowView)grKhachHangDS.Items[i];
                        break;
                    }
                }
                if (!LObject.IsNullOrEmpty(dr))
                {
                    Window window = new Window();
                    ucChuyenDiaBanCT uc = new ucChuyenDiaBanCT(Convert.ToInt32(dr["ID"]), dr["TTHAI_NVU"].ToString(), DatabaseConstant.Action.XEM);
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.KH_CHUYEN_DIA_BAN);
                    window.Content = uc;
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.ShowDialog();
                }
                
            }
            catch (System.Exception ex)
            {
            	
            }
            
        }

        /// <summary>
        /// Xử lý xóa dữ liệu
        /// </summary>
        private void Xoa()
        {
            if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    KhachHangProcess process = new KhachHangProcess();
                    int[] arrayID = new int[0];
                    Mouse.OverrideCursor = Cursors.Wait;
                    try
                    {
                        for (int i = 0; i < grKhachHangDS.Items.Count; i++)
                        {
                            DataRowView dr = (DataRowView)grKhachHangDS.Items[i];
                            if (Convert.ToBoolean(dr["CHON"]) == true)
                            {
                                Array.Resize(ref arrayID, arrayID.Length + 1);
                                arrayID[arrayID.Length - 1] = Convert.ToInt32(dr["ID"]);
                            }
                        }

                        List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                        ApplicationConstant.ResponseStatus ret = process.XoaGDChuyenDiaBan(arrayID, ref listClientResponseDetail);

                        if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
                        {
                            LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
                        }
                        else
                        {
                            CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                        }
                        TimKiem();
                    }
                    catch (System.Exception ex)
                    {
                        this.Cursor = Cursors.Arrow;
                        CommonFunction.ThongBaoLoi(ex);
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                    }
                }
                catch (System.Exception ex)
                {
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                finally
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
        }

        private void Duyet()
        {
            if (LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    KhachHangProcess process = new KhachHangProcess();
                    int[] arrayID = new int[0];
                    Mouse.OverrideCursor = Cursors.Wait;
                    try
                    {
                        for (int i = 0; i < grKhachHangDS.Items.Count; i++)
                        {
                            DataRowView dr = (DataRowView)grKhachHangDS.Items[i];
                            if (Convert.ToBoolean(dr["CHON"]) == true)
                            {
                                Array.Resize(ref arrayID, arrayID.Length + 1);
                                arrayID[arrayID.Length - 1] = Convert.ToInt32(dr["ID"]);
                            }
                        }
                        if (arrayID.Length > 0)
                        {
                            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                            ApplicationConstant.ResponseStatus ret = process.DuyetGDChuyenDiaBan(arrayID, ref listClientResponseDetail);

                            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
                            {
                                LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);
                            }
                            else
                            {
                                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                            }
                            TimKiem();
                        }
                        else
                        {
                            LMessage.ShowMessage("Chưa có bản ghi nào được chọn", LMessage.MessageBoxType.Warning);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        this.Cursor = Cursors.Arrow;
                        CommonFunction.ThongBaoLoi(ex);
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                    }

                }
                catch (System.Exception ex)
                {
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                finally
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
        }

        private void TuChoi()
        {
            if (LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    KhachHangProcess process = new KhachHangProcess();
                    int[] arrayID = new int[0];
                    Mouse.OverrideCursor = Cursors.Wait;
                    try
                    {
                        for (int i = 0; i < grKhachHangDS.Items.Count; i++)
                        {
                            DataRowView dr = (DataRowView)grKhachHangDS.Items[i];
                            if (Convert.ToBoolean(dr["CHON"]) == true)
                            {
                                Array.Resize(ref arrayID, arrayID.Length + 1);
                                arrayID[arrayID.Length - 1] = Convert.ToInt32(dr["ID"]);
                            }
                        }

                        if (arrayID.Length > 0)
                        {
                            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                            ApplicationConstant.ResponseStatus ret = process.TuChoiGDChuyenDiaBan(arrayID, ref listClientResponseDetail);

                            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
                            {
                                LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);
                            }
                            else
                            {
                                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                            }
                            TimKiem();
                        }
                        else
                        {
                            LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        this.Cursor = Cursors.Arrow;
                        CommonFunction.ThongBaoLoi(ex);
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                    }

                }
                catch (System.Exception ex)
                {
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                finally
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
        }

        private void ThoaiDuyet()
        {
            if (LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    KhachHangProcess process = new KhachHangProcess();
                    int[] arrayID = new int[0];
                    Mouse.OverrideCursor = Cursors.Wait;
                    try
                    {
                        for (int i = 0; i < grKhachHangDS.Items.Count; i++)
                        {
                            DataRowView dr = (DataRowView)grKhachHangDS.Items[i];
                            if (Convert.ToBoolean(dr["CHON"]) == true)
                            {
                                Array.Resize(ref arrayID, arrayID.Length + 1);
                                arrayID[arrayID.Length - 1] = Convert.ToInt32(dr["ID"]);
                            }
                        }

                        if (arrayID.Length > 0)
                        {
                            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                            ApplicationConstant.ResponseStatus ret = process.ThoaiDuyetGDChuyenDiaBan(arrayID, ref listClientResponseDetail);

                            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
                            {
                                LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);
                            }
                            else
                            {
                                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                            }
                            TimKiem();
                        }
                        else
                        {
                            LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        this.Cursor = Cursors.Arrow;
                        CommonFunction.ThongBaoLoi(ex);
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                    }

                }
                catch (System.Exception ex)
                {
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                finally
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
        }

        /// <summary>
        /// Tìm kiếm nâng cao
        /// </summary>
        private void TimKiem()
        {
            AutoComboBox au = new AutoComboBox();
            AutoCompleteEntry auLyDo = au.getEntryByDisplayName(lstSourceLyDoRa,ref cmbLyDoRaKhoiNhom);
            AutoCompleteEntry auLoaiGiayTo = au.getEntryByDisplayName(lstSourceLoaiGiayTo,ref cmbLoaiGiayTo);

            Mouse.OverrideCursor = Cursors.Wait;
            DataTable dt = null;
            LDatatable.MakeParameterTable(ref dt);
            // Lay du lieu treeview cho tim kiem
            string ma_don_vi = "-1";
            string ma_khu_vuc = "-1";
            string ma_cum = "-1";
            string ma_nhom = "-1";

            if (tvwKhachHangDS.SelectedItem != null)
            {
                RadTreeViewItem item = tvwKhachHangDS.SelectedItem as RadTreeViewItem;
                string level = item.Tag.ToString().Split('#')[0];
                string[] path = item.Tag.ToString().Split('#')[1].Split('/');
                string type = item.Tag.ToString().Split('#')[2];
                if (type == "DVI")
                {
                    ma_don_vi = path[path.Length - 1];
                }
                else
                {
                    ma_don_vi = path[0];
                    //ma_khu_vuc = path[1];
                    //ma_cum = path[1];
                    if (type == "KVUC")
                    {
                        ma_khu_vuc = path[1];
                    }
                    if (type == "CUM")
                    {
                        if (path.Length > 2)
                            ma_cum = path[2];
                        else
                            ma_cum = path[1];
                    }
                    if (type == "NHOM")
                    {
                        if (path.Length > 3)
                            ma_nhom = path[3];
                        else
                            ma_nhom = path[2];
                    }
                }
            }

            if (ucTrangThaiNVu.GetItemsSelected() == "NULL")
            {
                LDatatable.AddParameter(ref dt, "@TrangThaiNghiepVu", "STRING", "");
            }
            else
            {
                LDatatable.AddParameter(ref dt, "@TrangThaiNghiepVu", "STRING", ucTrangThaiNVu.GetItemsSelected());
            }

            LDatatable.AddParameter(ref dt, "@IdDonVi", "INT", ma_don_vi);
            LDatatable.AddParameter(ref dt, "@IdKhuVuc", "INT", ma_khu_vuc);
            LDatatable.AddParameter(ref dt, "@IdCum", "INT", ma_cum);
            LDatatable.AddParameter(ref dt, "@IdNhom", "INT", ma_nhom);
            LDatatable.AddParameter(ref dt, "@SoGD", "STRING", txtSoGD.Text);

            if (raddtTuNgayGD.Value != null)
            {
                LDatatable.AddParameter(ref dt, "@NgayGDTu", "STRING", txtMaKH.Text);
            }
            else
            {
                LDatatable.AddParameter(ref dt, "@NgayGDTu", "STRING", "");
            }

            if (raddtDenNgayGD.Value != null)
            {
                LDatatable.AddParameter(ref dt, "@NgayGDDen", "STRING", txtMaKH.Text);
            }
            else
            {
                LDatatable.AddParameter(ref dt, "@NgayGDDen", "STRING", "");
            }

            if (auLyDo != null)
            {
                LDatatable.AddParameter(ref dt, "@LyDo", "STRING", auLyDo.KeywordStrings[0]);
            }
            else
            {
                LDatatable.AddParameter(ref dt, "@LyDo", "STRING", "");
            }

            LDatatable.AddParameter(ref dt, "@MaKhang", "STRING", txtMaKH.Text);
            LDatatable.AddParameter(ref dt, "@TenKhang", "STRING", txtTenKH.Text);

            if (auLoaiGiayTo != null)
            {
                LDatatable.AddParameter(ref dt, "@LoaiGTO", "STRING", txtMaKH.Text);
            }
            else
            {
                LDatatable.AddParameter(ref dt, "@LoaiGTO", "STRING", "");
            }

            LDatatable.AddParameter(ref dt, "@SoGTO", "STRING", txtSoGiayTo.Text);
            LDatatable.AddParameter(ref dt, "@Email", "STRING", txtEmail.Text);
            LDatatable.AddParameter(ref dt, "@SoDThoai", "STRING", txtDienThoai.Text);
            LDatatable.AddParameter(ref dt, "@TenDangNhap", "STRING", ClientInformation.TenDangNhap);
            LDatatable.AddParameter(ref dt, "@DonViQLy", "STRING", ClientInformation.MaDonViQuanLy);

            // Tim kiem
            Presentation.Process.KhachHangProcess process = new Presentation.Process.KhachHangProcess();
            DataSet ds = process.getChuyenDiaBanDS(dt);
            if (ds != null && ds.Tables.Count > 0)
            {
                grKhachHangDS.ItemsSource = ds.Tables[0].DefaultView;
                if (!LObject.IsNullOrEmpty(grKhachHangDS.SelectedItems))
                    grKhachHangDS.SelectedItems.Clear();
                lblSumDonVi.Content = ds.Tables[0].Rows.Count.ToString();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Load dữ liệu lên form
        /// </summary>
        private void LoadDuLieu()
        {
            try
            {
                Presentation.Process.KhachHangProcess process = new Presentation.Process.KhachHangProcess();
                //DataSet ds = process.getTreeView(Presentation.Process.Common.ClientInformation.IdDonVi);
                DataSet ds = process.getTreeView(ClientInformation.MaDonViQuanLy,ClientInformation.TenDangNhap);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dtSourceTree = ds.Tables[0];
                    if (dtSourceTree != null && dtSourceTree.Rows.Count > 0)
                    {
                        DataRow drFirst = dtSourceTree.Rows[0];
                        string cond = "convert(path,System.String) like '0#" + drFirst["idparent"].ToString() + @"/%'";
                        DataRow[] drRoot = dtSourceTree.Select(cond, "id");
                        foreach (DataRow dr in drRoot)
                        {
                            RadTreeViewItem item = new RadTreeViewItem();
                            item.Header = dr["ten"];
                            item.Tag = dr["path"];
                            tvwKhachHangDS.Items.Add(item);
                            tvwKhachHangDS.SelectedItem = item;
                            dtSourceTree.Rows.Remove(dr);
                            BuildTreeCungCap(item);
                            item.IsExpanded = true;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void OnSavingCompleted(object sender, EventArgs e)
        {
            try
            {
                TimKiem();
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtTimKiemNhanh.Focus();
        }
    }
}
