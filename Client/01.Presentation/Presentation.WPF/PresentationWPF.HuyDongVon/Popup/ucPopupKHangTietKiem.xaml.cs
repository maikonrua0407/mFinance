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
using Presentation.Process.Common;
using Presentation.Process.ZAMainAppServiceRef;
using System.Data;
using PresentationWPF.CustomControl;
using Telerik.Windows.Controls;
using Utilities.Common;

namespace PresentationWPF.HuyDongVon.Popup
{
    /// <summary>
    /// Interaction logic for ucPopupKHangTietKiem.xaml
    /// </summary>
    public partial class ucPopupKHangTietKiem : UserControl
    {
        #region Khai bao
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand SearchCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        List<AutoCompleteEntry> lstSourceNganhKinhTe = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiHinhToChuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiKhachHang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceGioiTinh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDanToc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLHinhCongTac = new List<AutoCompleteEntry>();

        private DataTable dtSourceTree = new DataTable();

        private string _id = "";
        public string id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _ma = "";
        public string ma
        {
            get { return _ma; }
            set { _ma = value; }
        }

        private string _ten = "";
        public string ten
        {
            get { return _ten; }
            set { _ten = value; }
        }

        private List<DataRowView> _lstData = null;
        public List<DataRowView> lstData
        {
            get { return _lstData; }
        }

        private bool isMultiSelect = true;

        // khai báo 1 hàm delegate
        public delegate void LayDuLieu(List<DataRow> lst);
        // khai báo 1 kiểu hàm delegate
        public LayDuLieu DuLieuTraVe;

        private bool depSing = false;
        private string LoaiKhachHang = "";
        string maKhuVuc = "";
        string maCum = "";
        string maNhom = "";
        // Phân trang
        int StartRow = 1;
        int EndRow = ClientInformation.SoLuongBanGhi;
        int CurrentPagging = 1;
        int PaggingSize = ClientInformation.SoLuongBanGhi;

        #endregion

        #region Khoi tao
        public ucPopupKHangTietKiem()
        {
            InitializeComponent();
            txtTimKiemNhanh.Focus();
            //HeThong hethong = new HeThong();
            //hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KhachHang;component/KhachHang/ucKhachHangDS.xaml", ref Toolbar, ref mnuGrid);
            //foreach (var item in mnuGrid.Items)
            //{
            //    if (item is MenuItem)
            //        ((MenuItem)item).Click += tlbHotKey_Click;
            //}
            BindHotkey();
            //radpage.PageSize = (int)nudPageSize.Value;
            radpage.PageIndexChanging += new EventHandler<PageIndexChangingEventArgs>(radpage_PageIndexChanging);
            LoadDuLieu();
        }

        public ucPopupKHangTietKiem(bool isMulti, bool depSingle = false, string LoaiKHang = null)
        {
            InitializeComponent();
            txtTimKiemNhanh.Focus();
            BindHotkey();
            radpage.PageIndexChanging += new EventHandler<PageIndexChangingEventArgs>(radpage_PageIndexChanging);
            this.grKhachHangDS.Columns[1].IsVisible = isMulti;
            depSing = depSingle;
            LoaiKhachHang = LoaiKHang;
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

                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(SaveCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F, ModifierKeys.Control);
                        key = new KeyBinding(SearchCommand, keyg);
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

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbSelect.IsEnabled;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Luu();
        }

        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbSearch.IsEnabled;
        }
        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            txtTimKiemNhanh.RaiseEvent(new RoutedEventArgs(Button.GotFocusEvent));
        }

        private void HelpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

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
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                Luu();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            {
                TimKiem();
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
        /// Sự kiện double click trên data grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grKhachHangDS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            tlbSelect_Click(null, null);
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
        /// Khởi tạo các datasource cho combobx
        /// </summary>

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
                DataRow[] drChild = dtSourceTree.Select("path like '" + level[0] + "#" + parent[parent.Length - 1].Substring(0, parent[parent.Length - 1].Length - 4) + "/%'").OrderBy(row => row[2]).ToArray();
                if (drChild.Length > 0)
                {
                    foreach (DataRow dr in drChild)
                    {
                        if (depSing)
                        {
                            if (dr["id"].Equals(ClientInformation.IdDonViGiaoDich))
                            {
                                RadTreeViewItem item = new RadTreeViewItem();
                                item.Header = dr["ten"];
                                item.Tag = dr["path"];
                                itemRoot.Items.Add(item);
                            }
                        }
                        else
                        {
                            RadTreeViewItem item = new RadTreeViewItem();
                            item.Header = dr["ten"];
                            item.Tag = dr["path"];
                            itemRoot.Items.Add(item);
                        }
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

        private void tlbSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grKhachHangDS.SelectedItems.Count == 0)
                {
                    LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                    return;
                }
                if (grKhachHangDS.SelectedItems.Count > 0)
                {
                    if (isMultiSelect == false && grKhachHangDS.SelectedItems.Count > 1)
                    {
                        LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
                        return;
                    }

                    if (DuLieuTraVe != null)
                    {
                        List<DataRow> list = new List<DataRow>();
                        foreach (DataRowView dr in grKhachHangDS.SelectedItems)
                        {
                            list.Add(dr.Row);
                        }
                        DuLieuTraVe(list);
                    }
                }

                if (this.Parent is Window)
                    ((Window)this.Parent).Close();
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
        #endregion

        #region Xu ly nghiep vu
        /// <summary>
        /// 
        /// </summary>
        private void Luu()
        {
            if (_lstData == null)
                _lstData = new List<DataRowView>();

            if (grKhachHangDS.Columns[1].IsVisible != true)
            {
                if (grKhachHangDS.SelectedItem != null)
                {
                    DataRowView dr = (DataRowView)grKhachHangDS.SelectedItem;
                    _id = dr["ID"].ToString();
                    _ma = dr["MA_KHANG"].ToString();
                    _ten = dr["TEN_KHANG"].ToString();

                    _lstData.Add(dr);
                }
            }
            else
            {
                _lstData.Clear();
                for (int i = 0; i < grKhachHangDS.Items.Count; i++)
                {
                    DataRowView item = (DataRowView)grKhachHangDS.Items[i];
                    if (Convert.ToBoolean(item["CHON"]) == true)
                    {
                        _lstData.Add(item);
                    }
                }
            }

            CustomControl.CommonFunction.CloseUserControl(this);
        }

        /// <summary>
        /// Tìm kiếm nâng cao
        /// </summary>
        private void TimKiem()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            DataTable dt = null;
            LDatatable.MakeParameterTable(ref dt);

            // Phân trang
            int StartRow = 1;
            int EndRow = ClientInformation.SoLuongBanGhi;
            int CurrentPagging = 1;
            int PaggingSize = ClientInformation.SoLuongBanGhi;

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

            // Them du lieu vao tim kiem
            LDatatable.AddParameter(ref dt, "@TenDangNhap", "STRING", "");
            LDatatable.AddParameter(ref dt, "@DonVi", "INT", ma_don_vi);
            LDatatable.AddParameter(ref dt, "@KhuVuc", "INT", ma_khu_vuc);
            LDatatable.AddParameter(ref dt, "@Cum", "INT", ma_cum);
            LDatatable.AddParameter(ref dt, "@Nhom", "INT", ma_nhom);
            LDatatable.AddParameter(ref dt, "@StartRow", "INT", StartRow.ToString());
            LDatatable.AddParameter(ref dt, "@EndRow", "INT", EndRow.ToString());

            // Tim kiem
            Presentation.Process.HuyDongVonProcess process = new Presentation.Process.HuyDongVonProcess();
            DataSet ds = process.getKetQuaTimKiemNangCao(dt);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable serverDataTable = ds.Tables[0];
                int totalRecord = Int32.Parse(ds.Tables[1].Rows[0][0].ToString());
                DataTable clientDataTable = CommonFunction.BuildClientDataTable(serverDataTable, CurrentPagging, PaggingSize, totalRecord);

                //grKhachHangDS.ItemsSource = ds.Tables[0].DefaultView;
                //lblSumKhachHang.Content = ds.Tables[0].Rows.Count.ToString();
                lblSumKhachHang.Content = totalRecord;
                //radpage.Source = ds.Tables[0].DefaultView;
                //radpage = new RadDataPager();

                //radpage.Source = clientDataTable.DefaultView;
                //radpage.PageSize = PaggingSize;
                //radpage.PageIndex = CurrentPagging - 1;
                //radpage.ItemCount = totalRecord;

                //grKhachHangDS.ItemsSource = radpage.PagedSource;
                grKhachHangDS.ItemsSource = null;
                grKhachHangDS.ItemsSource = clientDataTable.DefaultView;
                if (!LObject.IsNullOrEmpty(grKhachHangDS.SelectedItem))
                    grKhachHangDS.SelectedItems.Clear();
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
                Presentation.Process.HuyDongVonProcess process = new Presentation.Process.HuyDongVonProcess();
                //DataSet ds = process.getTreeView(Presentation.Process.Common.ClientInformation.IdDonVi);

                // Nếu người dùng là NVDV, QTDV thì chỉ lấy thông tin đơn vị mình
                // Nếu người dùng là NVTW,... thì lấy thông tin toàn hệ thống
                // (Truongnx: popup chỉ lấy thông tin khách hàng thuộc đơn vị mình)
                int idDonVi = 0;
                idDonVi = ClientInformation.IdDonVi;
                //if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()) ||
                //    ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()))
                //{
                //    idDonVi = ClientInformation.IdDonVi;
                //}
                //else
                //{
                //    idDonVi = DatabaseConstant.ID_TOCHUC;
                //}
                DataSet ds = process.getTreeView(idDonVi);
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
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
        #endregion

        private void txtTimKiemNhanh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txtTimKiemNhanh.Text == LLanguage.SearchResourceByKey("U.DungChung.TimKiemNhanh"))
                {
                    return;
                }
                Mouse.OverrideCursor = Cursors.Wait;
                PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grKhachHangDS, txtTimKiemNhanh.Text);
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void radpage_PageIndexChanging(object sender, PageIndexChangingEventArgs e)
        {
            if (e.NewPageIndex < radpage.PageCount)
            {
                CurrentPagging = e.NewPageIndex + 1;
                StartRow = (CurrentPagging - 1) * PaggingSize + 1;
                EndRow = StartRow + PaggingSize;
                //radpage = new RadDataPager();
                TimKiemPhanTrang();
            }
        }

        //private void radpage_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        //{
        //    if (e.NewPageIndex < radpage.PageCount)
        //    {
        //        CurrentPagging = e.NewPageIndex + 1;
        //        StartRow = (CurrentPagging - 1) * PaggingSize + 1;
        //        EndRow = StartRow + PaggingSize;
        //        //radpage = new RadDataPager();
        //        TimKiemPhanTrang();
        //    }
        //}

        /// <summary>
        /// Tìm kiếm phân trang
        /// </summary>
        private void TimKiemPhanTrang()
        {
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
            LDatatable.AddParameter(ref dt, "@TenDangNhap", "STRING", ClientInformation.TenDangNhap);
            LDatatable.AddParameter(ref dt, "@DonViQLy", "STRING", ClientInformation.MaDonViQuanLy);
            LDatatable.AddParameter(ref dt, "@DonVi", "INT", ma_don_vi);
            LDatatable.AddParameter(ref dt, "@KhuVuc", "INT", ma_khu_vuc);
            LDatatable.AddParameter(ref dt, "@Cum", "INT", ma_cum);
            LDatatable.AddParameter(ref dt, "@Nhom", "INT", ma_nhom);
            LDatatable.AddParameter(ref dt, "@StartRow", "INT", StartRow.ToString());
            LDatatable.AddParameter(ref dt, "@EndRow", "INT", EndRow.ToString());

            // Tim kiem
            Presentation.Process.HuyDongVonProcess process = new Presentation.Process.HuyDongVonProcess();
            DataSet ds = process.getKetQuaTimKiemNangCao(dt);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable serverDataTable = ds.Tables[0];
                int totalRecord = Int32.Parse(ds.Tables[1].Rows[0][0].ToString());
                DataTable clientDataTable = CommonFunction.BuildClientDataTable(serverDataTable, CurrentPagging, PaggingSize, totalRecord);

                lblSumKhachHang.Content = totalRecord;
                grKhachHangDS.ItemsSource = null;
                grKhachHangDS.ItemsSource = clientDataTable.DefaultView;
                if (!LObject.IsNullOrEmpty(grKhachHangDS.SelectedItem))
                    grKhachHangDS.SelectedItems.Clear();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

    }
}