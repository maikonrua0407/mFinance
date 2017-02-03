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
using Presentation.Process.TruyVanServiceRef;
using Presentation.Process.DanhMucServiceRef;
using System.Collections;
using System.Reflection;
using Presentation.Process.Common;

namespace PresentationWPF.DanhMuc.DungChung
{
    /// <summary>
    /// Interaction logic for ucDanhMucDS.xaml
    /// </summary>
    public partial class ucDanhMucDS : UserControl
    {
        #region Khai bao

        DataTable dtMaster = new DataTable();
        DataTable dtDetail = new DataTable();
        static DataTable grdData = new DataTable();
        DM_DMUC_LOAI obj = new DM_DMUC_LOAI();
        static List<HeaderDto> lstHeader;
        static bool columnsWidthLoad = false;
        static bool treViewUpdate = false;

        List<AutoCompleteEntry> lstSourceNguon = new List<AutoCompleteEntry>();
        private string maNguon;

        public string MaNguon
        {
            get { return maNguon; }
            set { maNguon = value; }
        }

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
        #endregion

        #region Khoi tao

        public ucDanhMucDS()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.DanhMuc;component/DungChung/ucDanhMucDS.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindHotkey();
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            // Khởi tạo điều kiện gọi danh mục
            List<string> lstDieuKien = new List<string>();
            // Gán giá trị điều kiện
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.NGUON_TAO_DU_LIEU));
            auto.GenAutoComboBox(ref lstSourceNguon, ref cmbNguon, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

            radPage.PageSize = (int)nudPageSize.Value;
            LoadDuLieu();
            HideControl();
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
                LoadDuLieu();
            }
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
            Sua();
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
            Duyet();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TuChoi();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ThoaiDuyet();
        }

        private void ViewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ViewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Xem();
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
            LayLai();
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
        /// Sự kiện LoadForm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtTimKiemNhanh.TextChanged += txtTimKiemNhanh_TextChanged;
            txtTimKiemNhanh.GotFocus += txtTimKiemNhanh_GotFocus;
            txtTimKiemNhanh.LostFocus += txtTimKiemNhanh_LostFocus;
            txtTimKiemNhanh.Focus();
        }

        private void HideControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_SA.layGiaTri()) ||
                ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTTW.layGiaTri()))
                arr = hethong.SetVisibleControl("PresentationWPF.DanhMuc.DungChung.ucDanhMucDS", "MANAGE");
            else
                arr = hethong.SetVisibleControl("PresentationWPF.DanhMuc.DungChung.ucDanhMucDS", "SETVALUE");
            foreach (List<string> lst in arr)
            {
                object item = gridMain.FindName(lst.First());
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
            PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grDanhSach, txtTimKiemNhanh.Text);
            loadWidthColumn();
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
            loadWidthColumn();
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
            loadWidthColumn();
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
                onHelp();
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
            if (grDanhSach != null)
            {
                radPage.PageSize = (int)nudPageSize.Value;
                if (grdData.Rows.Count > 0)
                    grDanhSach.ItemsSource = grdData;
                lblTong.Content = grdData.Rows.Count;
                loadWidthColumn();
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
        public void LoadDuLieu()
        {
            var process = new TruyVanProcess();
            List<string> lstDkLoc = new List<string>();
            lstDkLoc.Add(ucTrangThaiNVu.GetItemsSelected());
            lstDkLoc.Add(ucTrangThaiSDung.GetItemsSelected());
            DanhSachResponse DanhSachResponse = process.getDanhSachInformation(DatabaseConstant.DanhSachTruyVan.DM_DUNGCHUNG.getValue(), lstDkLoc);
            if (DanhSachResponse.DataSetSource != null)
            {
                lstHeader = DanhSachResponse.ListHeader.ToList();
                dtMaster = new DataTable();
                grdData = new DataTable();
                foreach (HeaderDto item in lstHeader)
                {
                    grdData.Columns.Add(LLanguage.SearchResourceByKey(item.LanguageKey), typeof(string));
                }

                lstHeader = DanhSachResponse.ListHeader.ToList();

                dtMaster = DanhSachResponse.DataSetSource.Tables[0];
                dtDetail = DanhSachResponse.DataSetSource.Tables[1];
                while (tvwTree.Items.Count > 0)
                    tvwTree.Items.RemoveAt(0);
                UpdateLayout();
                RadTreeViewItem rootItem = new RadTreeViewItem();
                rootItem.Header = "List data (" + DanhSachResponse.DataSetSource.Tables[0].Rows.Count.ToString() + ")";
                rootItem.Tag = string.Empty;
                rootItem.IsExpanded = true;
                tvwTree.Items.Add(rootItem);
                BuildTree(rootItem);
                BuildGrid();
                loadWidthColumn();
                cmbNguon.KeyDown += cmb_KeyDown;
                tvwTree.SelectionChanged += tvwTree_SelectionChanged;
                tvwTree.MouseDoubleClick += tvwTree_MouseDoubleClick;
                grDanhSach.MouseDoubleClick += grDanhSach_MouseDoubleClick;
                nudPageSize.ValueChanged += nudPageSize_ValueChanged;
            }
        }

        protected void BuildTree(RadTreeViewItem item)
        {
            foreach (DataRow row in dtMaster.Rows)
            {
                if (row["NODE_PARENT"].ToString() == item.Tag.ToString())
                {
                    RadTreeViewItem subItem = new RadTreeViewItem();
                    subItem.Header = LLanguage.SearchResourceByKey(row["MA_NNGU_LOAI"].ToString());
                    subItem.Tag = row["NODE"].ToString();
                    subItem.IsExpanded = true;
                    item.Items.Add(subItem);
                    BuildTree(subItem);
                }
            }
        }

        private void BuildGrid()
        {
            List<string> lst = new List<string>();
            foreach (RadTreeViewItem item in tvwTree.SelectedItems)
            {
                if (item.Items.Count == 0)
                {
                    lst.Add(item.Tag.ToString());
                }
            }
            grdData = new DataTable();
            foreach (DataColumn col in dtDetail.Columns)
            {
                grdData.Columns.Add(col.ColumnName, typeof(string));
            }
            if (lst.Count > 0)
            {
                foreach (DataRow row in dtDetail.Rows)
                {
                    if (lst.Contains(row["KEY"].ToString()))
                    {
                        row["MA_NNGU_GTRI"] = LLanguage.SearchResourceByKey(row["MA_NNGU_GTRI"].ToString());
                        grdData.ImportRow(row);
                    }
                }
            }
            if (grdData.Rows.Count > 0)
                grDanhSach.ItemsSource = grdData;
            else
                grDanhSach.ItemsSource = null;
            lblTong.Content = grdData.Rows.Count;
        }

        private void tvwTree_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BuildGrid(); loadWidthColumn();
        }

        private void grDanhSach_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement originalSender = e.OriginalSource as FrameworkElement;
            if (originalSender != null)
            {
                Xem();
            }
        }

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
                    }
                    else
                        grDanhSach.Columns[idx].IsVisible = false;
                    idx = idx + 1;
                }
                columnsWidthLoad = true;
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var process = new DanhMucProcess();
                if (treViewUpdate)
                {
                    if (obj.ID > 0)
                    {
                        obj.MA_DMUC_LOAI = txtMa.Text;
                        obj.TEN_DMUC_LOAI = txtTen.Text;
                        obj.NGUON_TAO_DL = lstSourceNguon.ElementAt(cmbNguon.SelectedIndex).KeywordStrings.First();
                        process.SuaDungChungLoai(obj);
                        treViewUpdate = false;
                    }
                }
                else
                {
                    obj = new DM_DMUC_LOAI();
                    obj.MA_DMUC_LOAI = txtMa.Text;
                    obj.TEN_DMUC_LOAI = txtTen.Text;
                    obj.NGUON_TAO_DL = lstSourceNguon.ElementAt(cmbNguon.SelectedIndex).KeywordStrings.First();
                    process.ThemDungChungLoai(obj);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (obj.ID > 0)
                {
                    var process = new DanhMucProcess();
                    int[] id = new int[1];
                    List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();

                    id[0] = obj.ID;
                    process.XoaDungChungLoai(id.ToArray(), ref listResponseDetail);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void tvwTree_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var process = new DanhMucProcess();
                DM_DMUC_LOAI loai = new DM_DMUC_LOAI();
                loai.MA_DMUC_LOAI = ((RadTreeViewItem)tvwTree.SelectedItem).Tag.ToString();
                obj = process.layDungChungLoai(loai);
                txtMa.Text = obj.MA_DMUC_LOAI;
                txtTen.Text = obj.TEN_DMUC_LOAI;
                setMaNguonDL(obj.NGUON_TAO_DL);
                treViewUpdate = true;
                txtMa.Focus();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void cmb_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                ((RadComboBox)sender).IsDropDownOpen = true;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        public void setMaNguonDL(string maChon)
        {
            try
            {
                cmbNguon.SelectedIndex = lstSourceNguon.IndexOf(lstSourceNguon.FirstOrDefault(i => i.KeywordStrings.First().Equals(maChon)));
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        //private void btnLoadGrid_Click(object sender, RoutedEventArgs e)
        //{
        //    BuildGrid();
        //    loadWidthColumn();
        //}

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(grDanhSach);
        }

        #endregion

        #region Xu ly nghiep vu

        /// <summary>
        /// Xử lý sự kiện thêm
        /// </summary>
        private void Them()
        {
            try
            {
                ucDanhMucCT ct = new ucDanhMucCT();
                ct.LstChiTiet = null;
                ct.ChiXem = false;
                Window window = new Window();
                window.Content = ct;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Xử lý sự kiện thêm
        /// </summary>
        private void Xem()
        {
            if (grDanhSach.SelectedItems.Count == 1)
            {
                try
                {
                    List<DataRow> list = new List<DataRow>();
                    DataRow dr = (DataRow)grDanhSach.SelectedItems.First();
                    list.Add(dr);
                    ucDanhMucCT ct = new ucDanhMucCT();
                    ct.LstChiTiet = list;
                    ct.ChiXem = true;
                    Window window = new Window();
                    window.Content = ct;
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.ShowDialog();
                }
                catch (Exception ex)
                {
                    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                }
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
            }
        }

        /// <summary>
        /// Xử lý sự kiện sửa
        /// </summary>
        private void Sua()
        {
            if (grDanhSach.SelectedItems.Count == 1)
            {
                try
                {
                    List<DataRow> list = new List<DataRow>();
                    DataRow dr = (DataRow)grDanhSach.SelectedItems.First();
                    list.Add(dr);
                    ucDanhMucCT ct = new ucDanhMucCT();
                    ct.LstChiTiet = list;
                    ct.ChiXem = false;
                    Window window = new Window();
                    window.Content = ct;
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.ShowDialog();
                }
                catch (Exception ex)
                {
                    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                }
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
            }
        }

        /// <summary>
        /// Xử lý xóa dữ liệu
        /// </summary>
        private void Xoa()
        {
            {
                if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                {
                    DanhMucProcess danhmucProcess = new DanhMucProcess();
                    Mouse.OverrideCursor = Cursors.Wait;
                    try
                    {
                        List<int> lstID = new List<int>();
                        List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();

                        foreach (DataRow row in grDanhSach.SelectedItems)
                        {
                            if ((int)row[2] > 0)
                                lstID.Add((int)row[2]);
                        }

                        if (danhmucProcess.XoaDungChung(lstID.ToArray(), ref listResponseDetail))
                        {
                            LMessage.ShowMessage("M.DanhMuc.ucDonViDS.XoaThanhCong", LMessage.MessageBoxType.Information);
                        }
                        else
                        {
                            LMessage.ShowMessage("M.DanhMuc.ucDonViDS.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                        }
                        LoadDuLieu();
                    }
                    catch (System.Exception ex)
                    {
                        this.Cursor = Cursors.Arrow;
                        if (ex.GetType() == typeof(CustomException))
                        {
                            new frmThongBaoLoi(ex.Message, ex.InnerException).ShowDialog();
                        }
                        else if (ex.InnerException.GetType() == typeof(CustomException))
                        {
                            new frmThongBaoLoi(ex.InnerException.Message, ex.InnerException).ShowDialog();
                        }
                        else
                        {
                            new frmThongBaoLoi("M.DanhMuc.ucDonViDS.LoiXoaDuLieu", ex).ShowDialog();
                        }
                        LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                    }
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
        }

        /// <summary>
        /// Xử lý duyệt
        /// </summary>
        private void Duyet()
        {
            DanhMucProcess danhmucProcess = new DanhMucProcess();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<int> lstID = new List<int>();
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();

                foreach (DataRow row in grDanhSach.SelectedItems)
                {
                    if ((int)row[2] > 0)
                        lstID.Add((int)row[2]);
                }

                if (danhmucProcess.DuyetDungChung(lstID.ToArray(), ref listResponseDetail))
                {
                    LMessage.ShowMessage("M.DanhMuc.ucDonViDS.XoaThanhCong", LMessage.MessageBoxType.Information);
                }
                else
                {
                    LMessage.ShowMessage("M.DanhMuc.ucDonViDS.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                }
                LoadDuLieu();
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                if (ex.GetType() == typeof(CustomException))
                {
                    new frmThongBaoLoi(ex.Message, ex.InnerException).ShowDialog();
                }
                else if (ex.InnerException.GetType() == typeof(CustomException))
                {
                    new frmThongBaoLoi(ex.InnerException.Message, ex.InnerException).ShowDialog();
                }
                else
                {
                    new frmThongBaoLoi("M.DanhMuc.ucDonViDS.LoiXoaDuLieu", ex).ShowDialog();
                }
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void ThoaiDuyet()
        {
            DanhMucProcess danhmucProcess = new DanhMucProcess();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<int> lstID = new List<int>();
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();

                foreach (DataRow row in grDanhSach.SelectedItems)
                {
                    if ((int)row[2] > 0)
                        lstID.Add((int)row[2]);
                }

                if (danhmucProcess.ThoaiDuyetDungChung(lstID.ToArray(), ref listResponseDetail))
                {
                    LMessage.ShowMessage("M.DanhMuc.ucDonViDS.XoaThanhCong", LMessage.MessageBoxType.Information);
                }
                else
                {
                    LMessage.ShowMessage("M.DanhMuc.ucDonViDS.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                }
                LoadDuLieu();
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                if (ex.GetType() == typeof(CustomException))
                {
                    new frmThongBaoLoi(ex.Message, ex.InnerException).ShowDialog();
                }
                else if (ex.InnerException.GetType() == typeof(CustomException))
                {
                    new frmThongBaoLoi(ex.InnerException.Message, ex.InnerException).ShowDialog();
                }
                else
                {
                    new frmThongBaoLoi("M.DanhMuc.ucDonViDS.LoiXoaDuLieu", ex).ShowDialog();
                }
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void TuChoi()
        {
            DanhMucProcess danhmucProcess = new DanhMucProcess();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<int> lstID = new List<int>();
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                foreach (DataRow row in grDanhSach.SelectedItems)
                {
                    if ((int)row[2] > 0)
                        lstID.Add((int)row[2]);
                }

                if (danhmucProcess.TuChoiDungChung(lstID.ToArray(), ref listResponseDetail))
                {
                    LMessage.ShowMessage("M.DanhMuc.ucDonViDS.XoaThanhCong", LMessage.MessageBoxType.Information);
                }
                else
                {
                    LMessage.ShowMessage("M.DanhMuc.ucDonViDS.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                }
                LoadDuLieu();
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                if (ex.GetType() == typeof(CustomException))
                {
                    new frmThongBaoLoi(ex.Message, ex.InnerException).ShowDialog();
                }
                else if (ex.InnerException.GetType() == typeof(CustomException))
                {
                    new frmThongBaoLoi(ex.InnerException.Message, ex.InnerException).ShowDialog();
                }
                else
                {
                    new frmThongBaoLoi("M.DanhMuc.ucDonViDS.LoiXoaDuLieu", ex).ShowDialog();
                }
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        #endregion

    }
}
