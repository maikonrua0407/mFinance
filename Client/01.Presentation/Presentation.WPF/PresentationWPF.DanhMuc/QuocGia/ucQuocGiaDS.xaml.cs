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
using Presentation.Process.TruyVanServiceRef;
using System.Data;
using PresentationWPF.CustomControl;
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using Telerik.Windows.Controls;
using Presentation.Process;
using Presentation.Process.Common;

namespace PresentationWPF.DanhMuc.QuocGia
{
    /// <summary>
    /// Interaction logic for ucQuocGiaDS.xaml
    /// </summary>
    public partial class ucQuocGiaDS : UserControl
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
        ///
        delegate void LoadFormCT(bool bBool);
        //Lưu danh sách các item được chọn trong treeview
        private List<string> lstChecked = new List<string>();
        static List<HeaderDto> lstHeader;
        static bool columnsWidthLoad = false;
        DataTable dtMaster = new DataTable();
        DataTable dtDetail = new DataTable();
        #endregion

        #region Khoi tao

        public ucQuocGiaDS()
        {
            InitializeComponent();
            InitEventHandler();
            BindHotkey();
            LoadDuLieu();
            ResetForm();
        }

        /// <summary>
        /// InitEventHandler
        /// </summary>
        private void InitEventHandler()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.DanhMuc;component/QuocGia/ucQuocGiaDS.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }

            txtTimKiemNhanh.GotFocus += txtTimKiemNhanh_GotFocus;
            txtTimKiemNhanh.LostFocus += txtTimKiemNhanh_LostFocus;
            txtTimKiemNhanh.KeyDown += new KeyEventHandler(txtTimKiemNhanh_KeyDown);

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

        #region Dang ky hot key

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
                Them();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                Sua(true);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                Xoa();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
                Sua(false);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
                txtTimKiemNhanh.RaiseEvent(new RoutedEventArgs(Button.GotFocusEvent));
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
                LayLai();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
                XuatExcel();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }

        #endregion

        #region Dang ky shortcut key

        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Them();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Sua(true);
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Xoa();
        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ViewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ViewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Sua(false);
        }

        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            txtTimKiemNhanh.RaiseEvent(new RoutedEventArgs(Button.GotFocusEvent));
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
        /// Trợ giúp
        /// </summary>
        private void onHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }
        #endregion

        #region Xu ly giao dien

        /// <summary>
        /// Su kien Loaded danh sach tinh thanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grDanhSach_Loaded(object sender, RoutedEventArgs e)
        {
            loadWidthColumn();
        }

        /// <summary>
        /// Load lai du lieu khi co thay doi tu form chi tiet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void usercontrol_OnSavingComleted(object sender, EventArgs e)
        {
            LoadDuLieu();
        }

        /// <summary>
        /// Sự kiện LoadForm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtTimKiemNhanh.Focus();
        }

        /// <summary>
        /// Sự kiện tìm kiếm nhanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTimKiemNhanh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                if (txtTimKiemNhanh.Text == LLanguage.SearchResourceByKey("U.DungChung.TimKiemNhanh"))
                {
                    return;
                }
                PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grDanhSach, txtTimKiemNhanh.Text);
                loadWidthColumn();
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
        /// Xử lý sự kiện escape thoát form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                Them();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                Sua(true);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                Xoa();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
                Sua(true);
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
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }

        /// <summary>
        /// Sự kiện thay đổi số bản ghi trên một page hiển thị
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudPageSize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (grDanhSach != null && grDanhSach.ItemsSource != null)
            {
                if (dtDetail != null)
                {
                    radPage.PageSize = (int)nudPageSize.Value;
                    //grDanhSach.DataContext = dt.DefaultView;
                    grDanhSach.ItemsSource = dtDetail.DefaultView;
                    loadWidthColumn();
                }
            }
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(grDanhSach);
        }

        /// <summary>
        /// Fix columns datagrid
        /// </summary>
        private void loadWidthColumn()
        {
            if (grDanhSach.Items.Count > 0)
            {
                int idx = 1;
                foreach (HeaderDto item in lstHeader)
                {
                    double width = (double)item.WidthValue;
                    if (width > 0)
                    {
                        Telerik.Windows.Controls.GridViewLengthUnitType unit = new Telerik.Windows.Controls.GridViewLengthUnitType();
                        if (item.WidthUnit.Equals(ApplicationConstant.layGiaTri(ApplicationConstant.UnitWidth.Pixel)))
                            unit = Telerik.Windows.Controls.GridViewLengthUnitType.Pixel;
                        else if (item.WidthUnit.Equals(ApplicationConstant.layGiaTri(ApplicationConstant.UnitWidth.Star)))
                            unit = Telerik.Windows.Controls.GridViewLengthUnitType.Star;
                        else if (item.WidthUnit.Equals(ApplicationConstant.layGiaTri(ApplicationConstant.UnitWidth.Percent)))
                            unit = Telerik.Windows.Controls.GridViewLengthUnitType.Auto;
                        grDanhSach.Columns[idx].Width = new Telerik.Windows.Controls.GridViewLength(width, unit);
                        grDanhSach.Columns[idx].IsReadOnly = true;
                        grDanhSach.Columns[idx].Header = LLanguage.SearchResourceByKey(item.LanguageKey);
                    }
                    else
                        grDanhSach.Columns[idx].IsVisible = false;
                    idx = idx + 1;
                }
                columnsWidthLoad = true;
            }
        }

        /// <summary>
        /// Lấy lại dữ liệu
        /// </summary>
        private void LayLai()
        {
            LoadDuLieu();
        }

        /// <summary>
        /// Load dữ liệu lên datagrid
        /// </summary>
        private void LoadDuLieu()
        {
            var process = new TruyVanProcess();
            List<string> lstDkLoc = new List<string>();
            lstDkLoc.Add(ucTrangThaiNVu.GetItemsSelected());
            lstDkLoc.Add(ucTrangThaiSDung.GetItemsSelected());
            DanhSachResponse DanhSachResponse = process.getDanhSachInformation(DatabaseConstant.DanhSachTruyVan.DM_QUOCGIA.getValue(), lstDkLoc);
            if (DanhSachResponse.DataSetSource != null)
            {
                lstHeader = DanhSachResponse.ListHeader.ToList();

                //dtMaster = DanhSachResponse.DataSetSource.Tables[0];
                //dtDetail = DanhSachResponse.DataSetSource.Tables[1];
                dtMaster = null;
                dtDetail = DanhSachResponse.DataSetSource.Tables[0];

                //RadTreeViewItem rootItem = new RadTreeViewItem();
                //rootItem.Header = LLanguage.SearchResourceByKey(DanhSachResponse.Title);
                //rootItem.Tag = string.Empty;
                //rootItem.IsExpanded = true;
                //tvwTree.Items.Add(rootItem);
                //BuildTree(rootItem);
                BuildGrid();
                loadWidthColumn();
            }
        }

        /// <summary>
        /// Build cây thư mục vùng, miền, quốc gia
        /// </summary>
        /// <param name="item"></param>
        protected void BuildTree(RadTreeViewItem item)
        {
            foreach (DataRow row in dtMaster.Rows)
            {
                if (row["NODE_PARENT"].ToString() == item.Tag.ToString())
                {
                    RadTreeViewItem subItem = new RadTreeViewItem();
                    subItem.Header = row["NODE_NAME"].ToString();
                    subItem.Tag = row["NODE"].ToString();
                    subItem.IsExpanded = true;
                    subItem.IsChecked = true;
                    item.Items.Add(subItem);
                    BuildTree(subItem);
                }
            }
        }

        /// <summary>
        /// Build dữ liệu lên lưới
        /// </summary>
        private void BuildGrid()
        {
            //List<string> lst = new List<string>();
            //foreach (RadTreeViewItem item in tvwTree.CheckedItems)
            //{
            //    if (item.Items.Count == 0)
            //    {
            //        lst.Add(item.Tag.ToString());
            //    }
            //}
            DataTable grdData = new DataTable();
            foreach (DataColumn col in dtDetail.Columns)
            {
                grdData.Columns.Add(col.ColumnName, typeof(string));
            }
            if (dtDetail.Rows.Count > 0)
            {
                int stt = 0;
                foreach (DataRow row in dtDetail.Rows)
                {
                    stt = stt + 1;
                    row[0] = stt.ToString();
                    grdData.ImportRow(row);
                }
            }
            if (grdData.Rows.Count > 0)
            {
                grDanhSach.ItemsSource = grdData.DefaultView;
                grDanhSach.SelectedItems.Clear();
            }
            else
                grDanhSach.ItemsSource = null;
            lblTongSo.Content = grdData.Rows.Count;
        }

        /// <summary>
        /// DoubleClick trên lưới
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grDanhSach_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            return;
            FrameworkElement originalSender = e.OriginalSource as FrameworkElement;
            if (originalSender != null)
            {
                Sua(true);
            }
        }

        /// <summary>
        /// Check tree view Vùng Miền
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvwTree_Checked(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            BuildGrid();
            loadWidthColumn();
        }
        /// <summary>
        /// Uncheck tree view vùng miền
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvwTree_Unchecked(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            BuildGrid();
            loadWidthColumn();
        }

        #endregion

        #region Xu ly nghiep vu

        /// <summary>
        /// Xử lý sự kiện thêm
        /// </summary>
        private void Them()
        {
            ucQuocGiaCT usercontrol = new ucQuocGiaCT();
            usercontrol.OnSavingComleted += new EventHandler(usercontrol_OnSavingComleted);
            Window frm = new Window();
            frm.Content = usercontrol;
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frm.ShowDialog();
        }

        /// <summary>
        /// Xử lý sự kiện sửa
        /// </summary>
        private void Sua(bool bSua)
        {
            if (grDanhSach.SelectedItems.Count > 0 & grDanhSach.SelectedItems.Count < 2)
            {
                DataRow dr = (DataRow)grDanhSach.SelectedItems[0];
                ucQuocGiaCT usercontrol = new ucQuocGiaCT();
                usercontrol.LstChiTiet = dr;
                LoadFormCT dlgLoadFormCT = new LoadFormCT(usercontrol.LoadForm);
                dlgLoadFormCT(bSua);
                usercontrol.OnSavingComleted += new EventHandler(usercontrol_OnSavingComleted);
                Window frm = new Window();
                frm.Content = usercontrol;
                frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                frm.ShowDialog();
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
                return;
            }
        }

        /// <summary>
        /// Xử lý xóa dữ liệu
        /// </summary>
        private void Xoa()
        {
            DanhMucProcess danhmucProcess = new Presentation.Process.DanhMucProcess();
            try
            {
                if (grDanhSach.SelectedItems.Count > 0)
                {
                    int[] arrid = new int[grDanhSach.SelectedItems.Count];
                    List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();

                    for (int i = 0; i < grDanhSach.SelectedItems.Count; i++)
                    {
                        DataRow dtr = (DataRow)grDanhSach.SelectedItems[i];
                        arrid[i] = int.Parse(dtr[1].ToString());
                    }
                    if (danhmucProcess.XoaTinhTP(arrid.ToArray(), ref listResponseDetail))
                    {
                        LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
                        LoadDuLieu();
                    }
                    else
                    {
                        LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Error);
                    }
                }
                else
                {
                    LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
            }
            danhmucProcess = null;
        }

        #endregion
    }
}
