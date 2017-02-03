using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
using System.Data;
using Microsoft.Windows.Controls.Ribbon;
using Telerik.Windows.Controls;
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.TaiSanDamBaoServiceRef;
using PresentationWPF.CustomControl;

namespace PresentationWPF.TinDungTD.TSDB
{
    /// <summary>
    /// Interaction logic for ucTaiSanDamBaoDS.xaml
    /// </summary>
    public partial class ucTaiSanDamBaoDS : UserControl
    {
        #region Khai bao
        private int flag = 0;
        private int currentPosition;
        private int currentPage;
        private int currentID;

        DataTable dtTreeDonVi = new DataTable();

        List<AutoCompleteEntry> lstSourceChucVu = new List<AutoCompleteEntry>();

        private DataTable dtSourceTree = new DataTable();

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

        //Lưu danh sách các item được chọn trong treeview
        private List<string> lstChecked = new List<string>();

        // Phân trang
        int StartRow = 1;
        int EndRow = ClientInformation.SoLuongBanGhi;
        int CurrentPagging = 1;
        int PaggingSize = ClientInformation.SoLuongBanGhi;
        #endregion

        #region Khoi tao
        public ucTaiSanDamBaoDS()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDungTD;component/TSDB/ucTaiSanDamBaoDS.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindHotkey();
            //radPage.PageSize = (int)nudPageSize.Value;
            radPage.PageIndexChanging += new EventHandler<PageIndexChangingEventArgs>(radPage_PageIndexChanging);
            //radPage.PageIndexChanged += new EventHandler<PageIndexChangedEventArgs>(radPage_PageIndexChanged);

            KhoiTaoCombobox();
            LoadTree();
            txtTimKiemNhanh.Focus();
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
                        KeyGesture keyg = new KeyGesture(Key.D, ModifierKeys.Control);
                        key = new KeyBinding(DeleteCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift);
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
            e.CanExecute = true;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeAddNew();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeModify();
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

        private void ViewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ViewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeView();
        }

        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TimKiem();
        }

        private void ReloadCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ReloadCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ExportCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
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
            OnHelp();
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
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                BeforeAddNew();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                BeforeModify();
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
                BeforeView();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            {
                TimKiem();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
            {
                LayLai();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
            {
                XuatExcel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                OnHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                BeforeAddNew();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                BeforeModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                BeforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                BeforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                BeforeRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
                BeforeView();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            {
                txtTimKiemNhanh.RaiseEvent(new RoutedEventArgs(Button.GotFocusEvent));
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
            {
                LayLai();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
            {
                XuatExcel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                OnHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void OnHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
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
            PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grDSTaiSan, txtTimKiemNhanh.Text);
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
            if (grDSTaiSan != null && grDSTaiSan.ItemsSource != null)
            {
                DataTable dt = null;
                if (grDSTaiSan.ItemsSource is DataView)
                {
                    dt = ((DataView)grDSTaiSan.ItemsSource).Table;
                }
                else
                {
                    dt = grDSTaiSan.ItemsSource as DataTable;
                }

                if (dt != null)
                {
                    radPage.PageSize = (int)nudPageSize.Value;
                    grDSTaiSan.ItemsSource = dt.DefaultView;
                }
            }
        }

        private void grTaiSan_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement originalSender = e.OriginalSource as FrameworkElement;
            if (originalSender != null)
            {
                BeforeView();
            }
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(grDSTaiSan);
        }

        /// <summary>
        /// Khởi tạo các datasource cho combobx
        /// </summary>
        private void KhoiTaoCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDK = new List<string>();

        }

        /// <summary>
        /// Load dữ liệu lên tree
        /// </summary>
        private void LoadTree()
        {
            DanhMucProcess danhMucProcess = new DanhMucProcess();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@USER_NAME", "String", ClientInformation.TenDangNhap);
            LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "String", ClientInformation.MaDonViQuanLy);
            DataSet ds = danhMucProcess.getTreeCum_01(dt);
            if (ds != null)
            {
                //lstHeader = DanhSachResponse.ListHeader.ToList();

                dtSourceTree = ds.Tables[0];

                DataRow drRoot = dtSourceTree.Rows[0];

                RadTreeViewItem rootItem = new RadTreeViewItem();
                rootItem.Header = drRoot["NODE_NAME"].ToString();
                rootItem.Tag = drRoot["NODE"].ToString();
                rootItem.Uid = drRoot["NODE_TYPE"].ToString();
                //rootItem.IsExpanded = true;
                tvwTaiSan.Items.Add(rootItem);
                BuildTree(rootItem);
            }
        }

        protected void BuildTree(RadTreeViewItem item)
        {
            List<DataRow> lstDataRow = null;
            lstDataRow = dtSourceTree.Select().OrderBy(row => row[2]).ToList();

            foreach (DataRow row in lstDataRow)
            {
                if (row["NODE_PARENT"].ToString() == item.Tag.ToString())
                {
                    RadTreeViewItem subItem = new RadTreeViewItem();
                    subItem.Header = row["NODE_NAME"].ToString();
                    subItem.Tag = row["NODE"].ToString();
                    subItem.Uid = row["NODE_TYPE"].ToString();
                    //subItem.IsExpanded = true;
                    item.Items.Add(subItem);
                    BuildTree(subItem);
                }
            }
        }

        private void txtTimKiemNhanh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txtTimKiemNhanh.Text == LLanguage.SearchResourceByKey("U.DungChung.TimKiemNhanh"))
                {
                    return;
                }
                Mouse.OverrideCursor = Cursors.Wait;
                PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grDSTaiSan, txtTimKiemNhanh.Text);
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void radPage_PageIndexChanging(object sender, PageIndexChangingEventArgs e)
        {
            if (e.NewPageIndex < radPage.PageCount)
            {
                CurrentPagging = e.NewPageIndex + 1;
                StartRow = (CurrentPagging - 1) * PaggingSize + 1;
                EndRow = StartRow + PaggingSize;
                //radPage = new RadDataPager();
                TimKiemPhanTrang();
            }
        }

        #endregion

        #region Xử lý nghiệp vụ
        /// <summary>
        /// Trước khi thêm mới
        /// </summary>
        private void BeforeAddNew()
        {
            OnAddNew();
        }

        /// <summary>
        /// Thêm
        /// </summary>
        private void OnAddNew()
        {
            Window window = new Window();
            ucTaiSanDamBaoCT userControl = new ucTaiSanDamBaoCT();

            userControl.Action = DatabaseConstant.Action.THEM;

            window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.TDTD_TSDB_CT);
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Content = userControl;
            window.ShowDialog();
            TimKiem();
        }


        /// <summary>
        /// Trước khi sửa
        /// </summary>
        private void BeforeModify()
        {
            try
            {
                List<DataRowView> listDataRow = getListSeletedDataRow();

                if (listDataRow != null)
                {
                    if (listDataRow.Count == 0)
                    {
                        LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    else if (listDataRow.Count > 1)
                    {
                        LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    else
                    {
                        currentPage = grDSTaiSan.Items.PageIndex;
                        currentPosition = grDSTaiSan.Items.CurrentPosition;
                        currentID = int.Parse(listDataRow.First()["ID"].ToString());

                        // Không cho phép sửa sau duyệt
                        if (listDataRow.First()["tthai_nvu"].ToString().Equals(BusinessConstant.layNgonNguNghiepVu(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri())))
                        {
                            LMessage.ShowMessage("M.DungChung.DaDuyetKhongDuocSua", LMessage.MessageBoxType.Warning);
                            return;
                        }
                        OnModify(currentID);
                    }
                }
                else
                {
                    LMessage.ShowMessage("M.DungChung.LoiChonDuLieu", LMessage.MessageBoxType.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Sửa
        /// </summary>
        /// <param name="id"></param>
        private void OnModify(int id)
        {
            try
            {
                ucTaiSanDamBaoCT userControl = new ucTaiSanDamBaoCT();

                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);

                bool ret = process.LockData(DatabaseConstant.Module.TSDB,
                    DatabaseConstant.Function.TDTD_TSDB_DS,
                    DatabaseConstant.Table.TDTD_TSDB,
                    DatabaseConstant.Action.SUA,
                    listLockId);
                if (ret)
                {

                    userControl.Action = DatabaseConstant.Action.SUA;
                    userControl.ID = id;

                    Window window = new Window();
                    window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.TDTD_TSDB_CT);
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.Content = userControl;
                    window.ShowDialog();
                }
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                }
                TimKiem();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }


        /// <summary>
        /// Trước khi xem
        /// </summary>
        private void BeforeView()
        {
            try
            {
                List<DataRowView> listDataRow = getListSeletedDataRow();

                if (listDataRow != null)
                {
                    if (listDataRow.Count == 0)
                    {
                        LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    else if (listDataRow.Count > 1)
                    {
                        LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    else
                    {
                        currentPage = grDSTaiSan.Items.PageIndex;
                        currentPosition = grDSTaiSan.Items.CurrentPosition;
                        currentID = int.Parse(listDataRow.First()["ID"].ToString());
                        OnView(currentID);
                    }
                }
                else
                {
                    LMessage.ShowMessage("M.DungChung.LoiChonDuLieu", LMessage.MessageBoxType.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Xem
        /// </summary>
        /// <param name="id"></param>
        private void OnView(int id)
        {
            try
            {
                ucTaiSanDamBaoCT userControl = new ucTaiSanDamBaoCT();

                userControl.Action = DatabaseConstant.Action.XEM;
                userControl.ID = id;

                Window window = new Window();
                window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.TDTD_TSDB_CT);
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Content = userControl;
                window.ShowDialog();
                TimKiem();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }


        /// <summary>
        /// Trước khi xóa
        /// </summary>
        private void BeforeDelete()
        {
            try
            {
                List<DataRowView> listDataRow = getListSeletedDataRow();

                if (listDataRow != null)
                {
                    if (listDataRow.Count == 0)
                    {
                        LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    else
                    {
                        // Lấy danh sách dữ liệu cần xử lý
                        List<int> listId = new List<int>();
                        foreach (DataRowView dr in listDataRow)
                        {
                            int id = int.Parse(dr["id"].ToString());
                            listId.Add(id);
                        }

                        // Cảnh báo người dùng
                        MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);
                        if (ret == MessageBoxResult.Yes)
                        {
                            // Yêu cầu lock bản ghi cần xử lý
                            UtilitiesProcess process = new UtilitiesProcess();
                            List<int> listLockedId = new List<int>();

                            bool retLockData = process.LockData(DatabaseConstant.Module.TSDB,
                                DatabaseConstant.Function.TDTD_TSDB_DS,
                                DatabaseConstant.Table.TDTD_TSDB,
                                DatabaseConstant.Action.XOA,
                                listId);

                            // Nếu lock thành công >> cho phép xử lý
                            if (retLockData)
                            {
                                OnDelete(listId);
                            }
                            else
                            {
                                LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                            }

                        }
                    }
                }
                else
                {
                    LMessage.ShowMessage("M.DungChung.LoiChonDuLieu", LMessage.MessageBoxType.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="listId"></param>
        private void OnDelete(List<int> listId)
        {
            TaiSanDamBaoProcess processTaiSanDamBao = new TaiSanDamBaoProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                List<TDTD_TAI_SAN_DAM_BAO> lstTSDB = new List<TDTD_TAI_SAN_DAM_BAO>();

                TDTD_TAI_SAN_DAM_BAO obj = null;
                foreach (int id in listId)
                {
                    obj = new TDTD_TAI_SAN_DAM_BAO();
                    obj.ID = id;
                    lstTSDB.Add(obj);
                }
                bool ret = processTaiSanDamBao.DanhSachTaiSanDamBaoTDTD(DatabaseConstant.Action.XOA, ref lstTSDB, ref listClientResponseDetail);

                AfterDelete(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TSDB,
                    DatabaseConstant.Function.TDTD_TSDB_DS,
                    DatabaseConstant.Table.TDTD_TSDB,
                    DatabaseConstant.Action.XOA,
                    listId);
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Sau khi xóa
        /// </summary>
        /// <param name="ret"></param>
        private void AfterDelete(bool ret, List<int> listId, List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
                TimKiem();
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                TimKiem();
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TSDB,
                DatabaseConstant.Function.TDTD_TSDB_DS,
                DatabaseConstant.Table.TDTD_TSDB,
                DatabaseConstant.Action.XOA,
                listId);
        }

        /// <summary>
        /// Trước khi duyệt
        /// </summary>
        private void BeforeApprove()
        {
            try
            {
                List<DataRowView> listDataRow = getListSeletedDataRow();

                if (listDataRow != null)
                {
                    if (listDataRow.Count == 0)
                    {
                        LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    else
                    {
                        // Lấy danh sách dữ liệu cần xử lý
                        List<int> listId = new List<int>();
                        foreach (DataRowView dr in listDataRow)
                        {
                            int id = int.Parse(dr["id"].ToString());
                            listId.Add(id);
                        }

                        // Cảnh báo người dùng
                        MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question);
                        if (ret == MessageBoxResult.Yes)
                        {
                            // Yêu cầu lock bản ghi cần xử lý
                            UtilitiesProcess process = new UtilitiesProcess();
                            List<int> listLockedId = new List<int>();

                            bool retLockData = process.LockData(DatabaseConstant.Module.TSDB,
                                DatabaseConstant.Function.TDTD_TSDB_DS,
                                DatabaseConstant.Table.TDTD_TSDB,
                                DatabaseConstant.Action.DUYET,
                                listId);

                            // Nếu lock thành công >> cho phép xử lý
                            if (retLockData)
                            {
                                OnApprove(listId);
                            }
                            else
                            {
                                LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                            }

                        }
                    }
                }
                else
                {
                    LMessage.ShowMessage("M.DungChung.LoiChonDuLieu", LMessage.MessageBoxType.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Duyệt
        /// </summary>
        /// <param name="listId"></param>
        private void OnApprove(List<int> listId)
        {
            TaiSanDamBaoProcess processTaiSanDamBao = new TaiSanDamBaoProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                List<TDTD_TAI_SAN_DAM_BAO> lstTSDB = new List<TDTD_TAI_SAN_DAM_BAO>();

                TDTD_TAI_SAN_DAM_BAO obj = null;
                foreach (int id in listId)
                {
                    obj = new TDTD_TAI_SAN_DAM_BAO();
                    obj.ID = id;
                    lstTSDB.Add(obj);
                }
                bool ret = processTaiSanDamBao.DanhSachTaiSanDamBaoTDTD(DatabaseConstant.Action.DUYET, ref lstTSDB, ref listClientResponseDetail);

                AfterApprove(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TSDB,
                    DatabaseConstant.Function.TDTD_TSDB_DS,
                    DatabaseConstant.Table.TDTD_TSDB,
                    DatabaseConstant.Action.XOA,
                    listId);
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Sau khi duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void AfterApprove(bool ret, List<int> listId, List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);
                TimKiem();
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                TimKiem();
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TSDB,
                DatabaseConstant.Function.TDTD_TSDB_DS,
                DatabaseConstant.Table.TDTD_TSDB,
                DatabaseConstant.Action.DUYET,
                listId);
        }


        /// <summary>
        /// Trước khi thoái duyệt
        /// </summary>
        private void BeforeCancel()
        {
            try
            {
                List<DataRowView> listDataRow = getListSeletedDataRow();

                if (listDataRow != null)
                {
                    if (listDataRow.Count == 0)
                    {
                        LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    else
                    {
                        // Lấy danh sách dữ liệu cần xử lý
                        List<int> listId = new List<int>();
                        foreach (DataRowView dr in listDataRow)
                        {
                            int id = int.Parse(dr["id"].ToString());
                            listId.Add(id);
                        }

                        // Cảnh báo người dùng
                        MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question);
                        if (ret == MessageBoxResult.Yes)
                        {
                            // Yêu cầu lock bản ghi cần xử lý
                            UtilitiesProcess process = new UtilitiesProcess();
                            List<int> listLockedId = new List<int>();

                            bool retLockData = process.LockData(DatabaseConstant.Module.TSDB,
                                DatabaseConstant.Function.TDTD_TSDB_DS,
                                DatabaseConstant.Table.TDTD_TSDB,
                                DatabaseConstant.Action.THOAI_DUYET,
                                listId);

                            // Nếu lock thành công >> cho phép xử lý
                            if (retLockData)
                            {
                                OnCancel(listId);
                            }
                            else
                            {
                                LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                            }

                        }
                    }
                }
                else
                {
                    LMessage.ShowMessage("M.DungChung.LoiChonDuLieu", LMessage.MessageBoxType.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Thoái duyệt
        /// </summary>
        /// <param name="listId"></param>
        private void OnCancel(List<int> listId)
        {
            TaiSanDamBaoProcess processTaiSanDamBao = new TaiSanDamBaoProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                List<TDTD_TAI_SAN_DAM_BAO> lstTSDB = new List<TDTD_TAI_SAN_DAM_BAO>();

                TDTD_TAI_SAN_DAM_BAO obj = null;
                foreach (int id in listId)
                {
                    obj = new TDTD_TAI_SAN_DAM_BAO();
                    obj.ID = id;
                    obj.OBJ_TDTD_TSDB = new TDTD_TSDB();
                    obj.OBJ_TDTD_TSDB.ID = id;
                    lstTSDB.Add(obj);
                }
                bool ret = processTaiSanDamBao.DanhSachTaiSanDamBaoTDTD(DatabaseConstant.Action.THOAI_DUYET, ref lstTSDB, ref listClientResponseDetail);

                AfterCancel(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TSDB,
                    DatabaseConstant.Function.TDTD_TSDB_DS,
                    DatabaseConstant.Table.TDTD_TSDB,
                    DatabaseConstant.Action.XOA,
                    listId);
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Sau khi thoái duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void AfterCancel(bool ret, List<int> listId, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);
                    TimKiem();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    TimKiem();
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TSDB,
                    DatabaseConstant.Function.TDTD_TSDB_DS,
                    DatabaseConstant.Table.TDTD_TSDB,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listId);
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }


        /// <summary>
        /// Trước khi từ chối
        /// </summary>
        private void BeforeRefuse()
        {
            try
            {
                List<DataRowView> listDataRow = getListSeletedDataRow();

                if (listDataRow != null)
                {
                    if (listDataRow.Count == 0)
                    {
                        LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    else
                    {
                        // Lấy danh sách dữ liệu cần xử lý
                        List<int> listId = new List<int>();
                        foreach (DataRowView dr in listDataRow)
                        {
                            int id = int.Parse(dr["id"].ToString());
                            listId.Add(id);
                        }

                        // Cảnh báo người dùng
                        MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question);
                        if (ret == MessageBoxResult.Yes)
                        {
                            // Yêu cầu lock bản ghi cần xử lý
                            UtilitiesProcess process = new UtilitiesProcess();
                            List<int> listLockedId = new List<int>();

                            bool retLockData = process.LockData(DatabaseConstant.Module.TSDB,
                                DatabaseConstant.Function.TDTD_TSDB_DS,
                                DatabaseConstant.Table.TDTD_TSDB,
                                DatabaseConstant.Action.TU_CHOI_DUYET,
                                listId);

                            // Nếu lock thành công >> cho phép xử lý
                            if (retLockData)
                            {
                                OnRefuse(listId);
                            }
                            else
                            {
                                LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                            }

                        }
                    }
                }
                else
                {
                    LMessage.ShowMessage("M.DungChung.LoiChonDuLieu", LMessage.MessageBoxType.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Từ chối
        /// </summary>
        /// <param name="listId"></param>
        private void OnRefuse(List<int> listId)
        {
            TaiSanDamBaoProcess processTaiSanDamBao = new TaiSanDamBaoProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                List<TDTD_TAI_SAN_DAM_BAO> lstTSDB = new List<TDTD_TAI_SAN_DAM_BAO>();

                TDTD_TAI_SAN_DAM_BAO obj = null;
                foreach (int id in listId)
                {
                    obj = new TDTD_TAI_SAN_DAM_BAO();
                    obj.ID = id;
                    obj.OBJ_TDTD_TSDB = new TDTD_TSDB();
                    obj.OBJ_TDTD_TSDB.ID = id;
                    lstTSDB.Add(obj);
                }
                bool ret = processTaiSanDamBao.DanhSachTaiSanDamBaoTDTD(DatabaseConstant.Action.TU_CHOI_DUYET, ref lstTSDB, ref listClientResponseDetail);

                AfterRefuse(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TSDB,
                    DatabaseConstant.Function.TDTD_TSDB_DS,
                    DatabaseConstant.Table.TDTD_TSDB,
                    DatabaseConstant.Action.XOA,
                    listId);
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Sau khi từ chối duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void AfterRefuse(bool ret, List<int> listId, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);
                    TimKiem();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    TimKiem();
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TSDB,
                    DatabaseConstant.Function.TDTD_TSDB_DS,
                    DatabaseConstant.Table.TDTD_TSDB,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listId);
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }


        /// <summary>
        /// Tìm kiếm nâng cao
        /// </summary>
        private void TimKiem()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                string sMaTrangThaiNVu = ucTrangThaiNVu.GetItemsSelected();
                if (sMaTrangThaiNVu.Equals("NULL")) sMaTrangThaiNVu = "(''CDU'',''DDU'',''TCD'',''THD'')";
                #region Điều kiện tìm kiếm
                RadTreeViewItem item = tvwTaiSan.SelectedItem as RadTreeViewItem;
                if (item == null)
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                    return;
                }
                string loaiDonVi = item.Uid.ToString();
                string maDonVi = item.Tag.ToString();
                #endregion

                DanhMucProcess danhMucProcess = new DanhMucProcess();

                DataTable dt = null;

                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@INP_MA_TRANG_THAI_NGHIEP_VU", "string", sMaTrangThaiNVu);
                LDatatable.AddParameter(ref dt, "@INP_MA_TSDB", "string", txtMaTS.Text);
                LDatatable.AddParameter(ref dt, "@INP_TEN_TSDB", "string", txtTenTS.Text);
                LDatatable.AddParameter(ref dt, "@INP_TEN_CHU_TS", "string", txtTenKH.Text);
                LDatatable.AddParameter(ref dt, "@INP_USER", "string", ClientInformation.TenDangNhap);
                LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "string", ClientInformation.MaDonViQuanLy);
                LDatatable.AddParameter(ref dt, "@MA_NNGU", "string", ClientInformation.NgonNgu);
                LDatatable.AddParameter(ref dt, "@StartRow", "string", StartRow.ToString());
                LDatatable.AddParameter(ref dt, "@EndRow", "string", EndRow.ToString());

                DataSet ds = new TaiSanDamBaoProcess().GetDanhSachTaiSanDamBaoTDTD(dt);
                if (ds != null && ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ds.Tables[0].Rows[i]["TTHAI_NVU"] = BusinessConstant.layNgonNguNghiepVu(ds.Tables[0].Rows[i]["TTHAI_NVU"].ToString());
                    }

                    grDSTaiSan.DataContext = ds.Tables[0].DefaultView;                    
                }
                else
                {
                    grDSTaiSan.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Lấy lại dữ liệu
        /// </summary>
        private void LayLai()
        {
            TimKiem();
        }

        /// <summary>
        /// Lấy danh sách id được chọn
        /// </summary>
        /// <returns></returns>
        private List<DataRowView> getListSeletedDataRow()
        {
            try
            {
                List<DataRowView> listDataRow = new List<DataRowView>();

                if (grDSTaiSan.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < grDSTaiSan.SelectedItems.Count; i++)
                    {
                        DataRowView dr = (DataRowView)grDSTaiSan.SelectedItems[i];
                        listDataRow.Add(dr);
                    }
                }
                return listDataRow;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                return null;
            }

        }

        private void TimKiemPhanTrang()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            DataTable dt = null;
            LDatatable.MakeParameterTable(ref dt);


            Mouse.OverrideCursor = Cursors.Arrow;
        }
        #endregion
    }
}

