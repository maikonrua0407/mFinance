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
using Presentation.Process;
using Utilities.Common;
using PresentationWPF.CustomControl;
using Telerik.Windows.Controls;
using Presentation.Process.Common;

namespace PresentationWPF.NhanSu.Lich
{
    /// <summary>
    /// Interaction logic for ucKhaiBaoLichDS.xaml
    /// </summary>
    public partial class ucKhaiBaoLichDS : UserControl
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

        // Danh sách các item được chọn trong treeview
        private List<string> lstChecked = new List<string>();
        private static bool isLoaded = false;
        private static DataTable dt;
        delegate void LoadDuLieuCT();

        private DatabaseConstant.Module Module = DatabaseConstant.Module.DMDC;
        private DatabaseConstant.Function Function = DatabaseConstant.Function.DC_LAI_SUAT_DS;

        List<AutoCompleteEntry> lstSourcePhanHe = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiNhanSu = new List<AutoCompleteEntry>();

        #endregion

        #region Khoi tao

        /// <summary>
        /// Khởi tạo danh sách
        /// </summary>
        public ucKhaiBaoLichDS()
        {
            InitializeComponent();            

            InitEventHandler();
            
            BindHotkey();

            ResetForm();

            LoadDuLieu();
        }

        /// <summary>
        /// InitEventHandler
        /// </summary>
        private void InitEventHandler()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.NhanSu;component/NhanSu/ucKhaiBaoLichDS.xaml", ref Toolbar, ref mnuGrid);

            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }

            txtTimKiemNhanh.GotFocus += txtTimKiemNhanh_GotFocus;
            txtTimKiemNhanh.LostFocus += txtTimKiemNhanh_LostFocus;
            txtTimKiemNhanh.KeyDown += new KeyEventHandler(txtTimKiemNhanh_KeyDown);
            grNhanSuDS.MouseDoubleClick += grNhanSuDS_MouseDoubleClick;
            trvDonVi.Checked += trvPhanHe_Checked;
            trvDonVi.Unchecked += trvPhanHe_Unchecked;
        }

        /// <summary>
        /// Đưa form về trạng thái mặc định
        /// </summary>
        private void ResetForm()
        {
            radPage.PageSize = (int)nudPageSize.Value;
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.W, ModifierKeys.Control);
                        key = new KeyBinding(ViewCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F, ModifierKeys.Control);
                        key = new KeyBinding(SearchCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F5, ModifierKeys.None);
                        key = new KeyBinding(ReloadCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.E, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ExportCommand, keyg);
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
            beforeAddNew();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbModify.IsEnabled;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeModify();
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
            beforeApprove();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbRefuse.IsEnabled;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeRefuse();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbCancel.IsEnabled;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeCancel();
        }

        private void ViewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbView.IsEnabled;
        }
        private void ViewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeView();
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
            Reload();
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
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
                beforeAddNew();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                beforeModify();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                beforeDelete();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                beforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                beforeRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                beforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
                beforeView();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
                txtTimKiemNhanh.RaiseEvent(new RoutedEventArgs(Button.GotFocusEvent));
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
                Reload();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
                XuatExcel();
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

        #region Xu ly giao dien

        /// <summary>
        /// Lấy lại dữ liệu
        /// </summary>
        private void Reload()
        {
            LoadDuLieu();
        }               

        /// <summary>
        /// Load lại dữ liệu khi có thay đổi từ form chi tiết
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void userControl_OnSavingCompleted(object sender, EventArgs e)
        {
            LoadDuLieu();
        }

        /// <summary>
        /// Load dữ liệu lên datagrid
        /// </summary>
        private void LoadDuLieu()
        {
            NhanSuProcess bus = new NhanSuProcess();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                // xử lý lấy dữ liệu
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                bus = null;
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Sự kiện tìm kiếm nhanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void txtTimKiemNhanh_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (txtTimKiemNhanh.Text == LLanguage.SearchResourceByKey("U.DungChung.TimKiemNhanh"))
        //    {
        //        return;
        //    }
        //    PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grNhanSuDS, txtTimKiemNhanh.Text);
        //}

        /// <summary>
        /// Sự kiện tìm kiếm nhanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTimKiemNhanh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grNhanSuDS, txtTimKiemNhanh.Text);
            }
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
                txtTimKiemNhanh.Focus();
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
        /// Sự kiện shortcutkey
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
                beforeAddNew();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                beforeModify();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                beforeDelete();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                beforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                beforeRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                beforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
                beforeView();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
                txtTimKiemNhanh.RaiseEvent(new RoutedEventArgs(Button.GotFocusEvent));
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
                Reload();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
                XuatExcel();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                onClose();
            }
        }

        /// <summary>
        /// Sự kiện thay đổi số bản ghi trên một page hiển thị
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudPageSize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (grNhanSuDS != null && grNhanSuDS.ItemsSource != null)
            {
                if (dt != null)
                {                    
                    radPage.PageSize = (int)nudPageSize.Value;
                    //grNhanSuDS.DataContext = dt;
                    grNhanSuDS.ItemsSource = dt;
                }
            }
        }

        /// <summary>
        /// Build tree
        /// </summary>
        /// <param name="item"></param>
        protected void BuildTree(RadTreeViewItem item)
        {
            foreach (AutoCompleteEntry entry in lstSourcePhanHe)
            {
                RadTreeViewItem subItem = new RadTreeViewItem();
                subItem.Header = entry.DisplayName;
                subItem.Tag = entry.KeywordStrings.First();
                subItem.Uid = entry.KeywordStrings.ElementAt(1);
                //subItem.Tag = row["id"].ToString();
                subItem.IsExpanded = true;
                subItem.IsChecked = false;
                item.Items.Add(subItem);
            }
        }

        /// <summary>
        /// checked tree
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trvPhanHe_Checked(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            FindData();
        }

        private void FindData()
        {
            try
            {
                List<string> lst = new List<string>();
                foreach (RadTreeViewItem item in trvDonVi.CheckedItems)
                {
                    if (!LString.IsNullOrEmptyOrSpace(item.Tag.ToString()))
                    {
                        lst.Add(item.Tag.ToString());
                    }
                }
                DataTable grdData = new DataTable("FIND_DATA");
                foreach (DataColumn col in dt.Columns)
                {
                    grdData.Columns.Add(col.ColumnName, col.DataType);
                }
                grNhanSuDS.ItemsSource = null;
                if (lst.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (lst.Contains(row["MA_PHAN_HE"].ToString()))
                        {
                            grdData.ImportRow(row);
                            grdData.Rows[grdData.Rows.Count - 1]["STT"] = grdData.Rows.Count;
                        }
                    }
                    grNhanSuDS.ItemsSource = grdData;
                    UpdateLayout();
                    lblSum.Content = grdData.Rows.Count;
                }
                else
                {
                    grNhanSuDS.ItemsSource = dt;
                    lblSum.Content = dt.Rows.Count;
                }
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Uncheck tree view vùng miền
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trvPhanHe_Unchecked(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            FindData();
        }   

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(grNhanSuDS);
            //PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel("GET_DS_NhanSu");
        }
        
        /// <summary>
        /// Xử lý sự kiện keydown trên form
        /// Bao gồm:
        /// Nhấn Escape để thoát form
        /// Nhấn Enter/Tab để focus vào control tiếp theo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Release();
                // Kiểm tra escape thoát form
                PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            }
            else
            {
                // Nhấn enter để chuyển focus tới control tiếp theo
                PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
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
        /// Đóng form
        /// </summary>
        private void onClose()
        {
            Release();
            CustomControl.CommonFunction.CloseUserControl(this);
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
        }
        #endregion

        #region Xu ly nghiep vu

        /// <summary>
        /// Su kien load form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucKhaiBaoLichDS_Loaded(object sender, RoutedEventArgs e)
        {
            txtTimKiemNhanh.Focus();
        }

        /// <summary>
        /// Sự kiện double click để sửa dữ liệu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grNhanSuDS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            beforeView();
        }

        /// <summary>
        /// Trước khi xem
        /// </summary>
        private void beforeView()
        {
            List<DataRow> listDataRow = getListSeletedDataRow();
            int id;

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
                    id = int.Parse(listDataRow.First()["id"].ToString());
                    onView(id);
                }
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.LoiChonDuLieu", LMessage.MessageBoxType.Warning);
                return;
            }
        }

        /// <summary>
        /// Trước khi thêm mới
        /// </summary>
        private void beforeAddNew()
        {
            onAddNew();
        }

        /// <summary>
        /// Trước khi sửa
        /// </summary>
        private void beforeModify()
        {
            List<DataRow> listDataRow = getListSeletedDataRow();
            
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
                    int id = int.Parse(listDataRow.First()["id"].ToString());
                    onModify(id);
                    
                }
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.LoiChonDuLieu", LMessage.MessageBoxType.Warning);
                return;
            }
        }

        /// <summary>
        /// Trước khi xóa
        /// </summary>
        private void beforeDelete()
        {
            List<DataRow> listDataRow = getListSeletedDataRow();

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
                    foreach (DataRow dr in listDataRow)
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

                        bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                            DatabaseConstant.Function.DC_LAI_SUAT_DS,
                            DatabaseConstant.Table.DC_LSUAT,
                            DatabaseConstant.Action.XOA,
                            listId);

                        // Nếu lock thành công >> cho phép xử lý
                        if (retLockData)
                        {
                            onDelete(listId);
                        }
                        else
                        {
                            LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                        }
                    }
                    else 
                    {
                        return;
                    }
                }
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.LoiChonDuLieu", LMessage.MessageBoxType.Warning);
                return;
            }
        }

        /// <summary>
        /// Trước khi duyệt
        /// </summary>
        private void beforeApprove()
        {
            List<DataRow> listDataRow = getListSeletedDataRow();

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
                    foreach (DataRow dr in listDataRow)
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

                        bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                            DatabaseConstant.Function.DC_LAI_SUAT_DS,
                            DatabaseConstant.Table.DC_LSUAT,
                            DatabaseConstant.Action.DUYET,
                            listId);

                        // Nếu lock thành công >> cho phép xử lý
                        if (retLockData)
                        {
                            onApprove(listId);
                        }
                        else
                        {
                            LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.LoiChonDuLieu", LMessage.MessageBoxType.Warning);
                return;
            }
        }

        /// <summary>
        /// Trước khi thoái duyệt
        /// </summary>
        private void beforeCancel()
        {
            List<DataRow> listDataRow = getListSeletedDataRow();

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
                    foreach (DataRow dr in listDataRow)
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

                        bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                            DatabaseConstant.Function.DC_LAI_SUAT_DS,
                            DatabaseConstant.Table.DC_LSUAT,
                            DatabaseConstant.Action.THOAI_DUYET,
                            listId);

                        // Nếu lock thành công >> cho phép xử lý
                        if (retLockData)
                        {
                            onCancel(listId);
                        }
                        else
                        {
                            LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.LoiChonDuLieu", LMessage.MessageBoxType.Warning);
                return;
            }
        }

        /// <summary>
        /// Trước khi từ chối
        /// </summary>
        private void beforeRefuse()
        {
            List<DataRow> listDataRow = getListSeletedDataRow();

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
                    foreach (DataRow dr in listDataRow)
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

                        bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                            DatabaseConstant.Function.DC_LAI_SUAT_DS,
                            DatabaseConstant.Table.DC_LSUAT,
                            DatabaseConstant.Action.TU_CHOI_DUYET,
                            listId);

                        // Nếu lock thành công >> cho phép xử lý
                        if (retLockData)
                        {
                            onRefuse(listId);
                        }
                        else
                        {
                            LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.LoiChonDuLieu", LMessage.MessageBoxType.Warning);
                return;
            }
        }

        /// <summary>
        /// Xem dữ liệu
        /// </summary>
        private void onView(int id)
        {
            ucKhaiBaoLichCT userControl = new ucKhaiBaoLichCT();
            userControl.OnSavingCompleted += new EventHandler(userControl_OnSavingCompleted);
            userControl.Id = id;
            userControl.Action = DatabaseConstant.Action.XEM;
            //LoadDuLieuCT dlgLoadDuLieuCT = new LoadDuLieuCT(userControl.beforeView);
            //dlgLoadDuLieuCT();

            string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.Function);
            Window frm = new Window();
            frm.Title = tittle;
            frm.Content = userControl;
            frm.ShowDialog();
        }

        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        private void onAddNew()
        {
            string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.Function);
            Window frm = new Window();
            frm.Title = tittle;
            ucKhaiBaoLichCT uc = new ucKhaiBaoLichCT();
            uc.OnSavingCompleted += new EventHandler(userControl_OnSavingCompleted);
            uc.Action = DatabaseConstant.Action.THEM;
            frm.Content = uc;
            frm.ShowDialog();
        }

        /// <summary>
        /// Sửa dữ liệu
        /// </summary>
        private void onModify(int id)
        {
            ucKhaiBaoLichCT userControl = new ucKhaiBaoLichCT();
            userControl.OnSavingCompleted += new EventHandler(userControl_OnSavingCompleted);
            userControl.Id = id;
            userControl.Action = DatabaseConstant.Action.SUA;
            //LoadDuLieuCT dlgLoadDuLieuCT = new LoadDuLieuCT(userControl.beforeModifyFromList);
            //dlgLoadDuLieuCT();

            string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.Function);
            Window frm = new Window();
            frm.Title = tittle;
            frm.Content = userControl;
            frm.ShowDialog();
        }

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        private void onDelete(List<int> listId)
        {
            NhanSuProcess NhanSuProcess = new NhanSuProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                // Get data

                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                // xử lý
                bool ret = false;

                afterDelete(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_LAI_SUAT_DS,
                    DatabaseConstant.Table.DC_LSUAT,
                    DatabaseConstant.Action.XOA,
                    listId);
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }       

        /// <summary>
        /// Duyệt dữ liệu
        /// </summary>
        private void onApprove(List<int> listId)
        {
            NhanSuProcess NhanSuProcess = new NhanSuProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                // lấy dữ liệu
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                // xử lý
                bool ret = false;

                afterApprove(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_LAI_SUAT_DS,
                    DatabaseConstant.Table.DC_LSUAT,
                    DatabaseConstant.Action.DUYET,
                    listId);

                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Thoái duyệt dữ liệu
        /// </summary>
        private void onCancel(List<int> listId)
        {
            NhanSuProcess NhanSuProcess = new NhanSuProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                // lấy dữ liệu
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                // xử lý
                bool ret = false;
                afterCancel(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_LAI_SUAT_DS,
                    DatabaseConstant.Table.DC_LSUAT,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listId);

                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Từ chối duyệt dữ liệu
        /// </summary>
        private void onRefuse(List<int> listId)
        {
            NhanSuProcess NhanSuProcess = new NhanSuProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                // lấy dữ liệu
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                // xử lý
                bool ret = false;
                afterRefuse(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_LAI_SUAT_DS,
                    DatabaseConstant.Table.DC_LSUAT,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listId);

                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Sau khi xem
        /// </summary>
        /// <param name="ret"></param>
        private void afterView()
        {
        }

        /// <summary>
        /// Sau khi thêm
        /// </summary>
        /// <param name="ret"></param>
        private void afterAddNew()
        {
            LoadDuLieu();
        }
        
        /// <summary>
        /// Sau khi xóa
        /// </summary>
        /// <param name="ret"></param>
        private void afterDelete(bool ret, 
            List<int> listId, 
            List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
                LoadDuLieu();
            }
            else
            {
                //LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                LoadDuLieu();
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_LAI_SUAT_DS,
                DatabaseConstant.Table.DC_LSUAT,
                DatabaseConstant.Action.XOA,
                listId);
        }

        /// <summary>
        /// Sau khi duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void afterApprove(bool ret,
            List<int> listId,
            List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);
                LoadDuLieu();
            }
            else
            {
                //LMessage.ShowMessage("M.DungChung.DuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                LoadDuLieu();
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_LAI_SUAT_DS,
                DatabaseConstant.Table.DC_LSUAT,
                DatabaseConstant.Action.DUYET,
                listId);
        }

        /// <summary>
        /// Sau khi thoái duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void afterCancel(bool ret,
            List<int> listId,
            List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);
                LoadDuLieu();
            }
            else
            {
                //LMessage.ShowMessage("M.DungChung.ThoaiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                LoadDuLieu();
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_LAI_SUAT_DS,
                DatabaseConstant.Table.DC_LSUAT,
                DatabaseConstant.Action.THOAI_DUYET,
                listId);
        }

        /// <summary>
        /// Sau khi từ chối
        /// </summary>
        /// <param name="ret"></param>
        private void afterRefuse(bool ret,
            List<int> listId,
            List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);
                LoadDuLieu();
            }
            else
            {
                //LMessage.ShowMessage("M.DungChung.TuChoiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                LoadDuLieu();
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_LAI_SUAT_DS,
                DatabaseConstant.Table.DC_LSUAT,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                listId);
        }

        /// <summary>
        /// Lấy danh sách id được chọn
        /// </summary>
        /// <returns></returns>
        private List<DataRow> getListSeletedDataRow()
        {
            List<DataRow> listDataRow = new List<DataRow>();
            if (grNhanSuDS.SelectedItems.Count <= 0)
            {
                return listDataRow;
            }
            else
            {
                for (int i = 0; i < grNhanSuDS.SelectedItems.Count; i++)
                {
                    DataRow dr = (DataRow)grNhanSuDS.SelectedItems[i];
                    listDataRow.Add(dr);
                }
                return listDataRow;
            }
        }        

        #endregion
    }
}
