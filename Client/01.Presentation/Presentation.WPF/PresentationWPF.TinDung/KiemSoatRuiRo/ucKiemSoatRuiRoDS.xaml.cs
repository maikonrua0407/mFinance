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
using Utilities.Common;
using Presentation.Process.Common;
using Microsoft.Windows.Controls.Ribbon;
using System.Data;
using Telerik.Windows.Controls;
using PresentationWPF.CustomControl;
using Presentation.Process;
using Presentation.Process.TinDungServiceRef;
using PresentationWPF.TinDung.ThuocTinh;

namespace PresentationWPF.TinDung.KiemSoatRuiRo
{
    /// <summary>
    /// Interaction logic for ucKiemSoatRuiRoDS.xaml
    /// </summary>
    public partial class ucKiemSoatRuiRoDS : UserControl
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

        private DatabaseConstant.Module Module = DatabaseConstant.Module.TDVM;
        private DatabaseConstant.Function Function = DatabaseConstant.Function.TDVM_KIEM_SOAT_RR;

        // Phân trang
        int StartRow = 1;
        int EndRow = ClientInformation.SoLuongBanGhi;
        int CurrentPagging = 1;
        int PaggingSize = ClientInformation.SoLuongBanGhi;
        string maGDichKU = "";
        string maGDichHD = "";
        DataSet dsRaw = new DataSet();
        DataTable dtTreeDLy;
        #endregion

        #region Khoi tao
        public ucKiemSoatRuiRoDS()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDung;component/KiemSoatRuiRo/ucKiemSoatRuiRoDS.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindHotkey();
            BuildTreeKhuVuc();
            teldtNgayKiemSoatTu.Value = null;
            teldtNgayKiemSoatDen.Value = null;
            radPage.PageIndexChanging += new EventHandler<PageIndexChangingEventArgs>(radPage_PageIndexChanging);
        }
        #endregion

        /// <summary>
        /// Dang ky hot key, shortcut key
        /// </summary>
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
                    //else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
                    //{
                    //    KeyGesture keyg = new KeyGesture(Key.Escape, ModifierKeys.None);
                    //    key = new KeyBinding(ucDonViDS.HelpCommand, keyg);
                    //}

                    if (key != null)
                    {
                        InputBindings.Add(key);
                    }
                }
            }
        }

        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ThemMoi();
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
            TuChoiDuyet();
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
            LoadData();
        }

        private void ReloadCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ReloadCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LoadData();
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
                ThemMoi();
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
                TuChoiDuyet();
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
                LoadData();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
            {
                LoadData(); 
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
        /// Xử lý sự kiện keydown trên form
        /// Bao gồm:
        /// Nhấn Escape để thoát form
        /// Nhấn Enter/Tab để focus vào control tiếp theo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            // Kiểm tra escape thoát form
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);

            // Nhấn enter để chuyển focus tới control tiếp theo
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
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
            {
                ThemMoi();
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
                TuChoiDuyet();
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
                LoadData();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
            {
                LoadData();
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
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(raddgrHDTDDS);
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
            PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(raddgrHDTDDS, txtTimKiemNhanh.Text);
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
        /// Sự kiện thay đổi số bản ghi trên một page hiển thị
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudPageSize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (raddgrHDTDDS != null && raddgrHDTDDS.ItemsSource != null)
            {
                DataView dt = ((DataView)raddgrHDTDDS.ItemsSource);
                if (dt != null)
                {
                    radPage.PageSize = (int)nudPageSize.Value;
                    raddgrHDTDDS.DataContext = dt;
                }
            }
        }

        void BuildTreeKhuVuc()
        {
            dtTreeDLy = new TinDungProcess().getDanhSachDonVi(ClientInformation.MaDonViQuanLy, ClientInformation.TenDangNhap).Tables[0];
            try
            {
                RadTreeViewItem Item = new RadTreeViewItem();
                //Item.Header = "Danh mục địa lý";
                //Item.IsExpanded = true;
                //Item.IsChecked = true;
                //tvwKhuVuc.Items.Add(Item);
                BuildSubTreeKhuVuc(Item, null, 0);
            }
            catch (Exception ex)
            { }
        }

        void BuildSubTreeKhuVuc(RadTreeViewItem Item, DataRow dr, int iLevel)
        {

            List<DataRow> lstDataRow = null;
            if (dr != null)
                lstDataRow = dtTreeDLy.Select("MA_DVI_CHA='" + dr["MA_DVI"] + "' AND LEVEL=" + iLevel).ToList();
            else
                lstDataRow = dtTreeDLy.Select("LEVEL=0").ToList();
            foreach (DataRow row in lstDataRow)
            {
                RadTreeViewItem subItem = new RadTreeViewItem();
                subItem.Header = row["TEN_GDICH"].ToString();
                subItem.Tag = row["MA_DVI"].ToString();
                //subItem.IsExpanded = true;
                subItem.IsChecked = true;
                if (row["LEVEL"].Equals(0))
                    tvwKhuVuc.Items.Add(subItem);
                else
                    Item.Items.Add(subItem);
                BuildSubTreeKhuVuc(subItem, row, Convert.ToInt32(row["LEVEL"]) + 1);
            }
        }

        private void raddgrHDTDDS_LoadingRowDetails(object sender, Telerik.Windows.Controls.GridView.GridViewRowDetailsEventArgs e)
        {
            DataRowView dtrv = e.Row.Item as DataRowView;
            DataTable dtDetail = dsRaw.Tables[0].Clone();
            foreach (DataRow dr in dsRaw.Tables[0].Select("MA_KIEM_SOAT ='" + dtrv["MA_KIEM_SOAT"].ToString() + "'"))
            {
                dtDetail.Rows.Add(dr.ItemArray);
            }
            RadGridView radGridViewDetail = e.DetailsElement.FindName("radDetailHDTD") as RadGridView;
            radGridViewDetail.ItemsSource = dtDetail.DefaultView;
            radGridViewDetail.Rebind();
        }

        private void radPage_PageIndexChanging(object sender, PageIndexChangingEventArgs e)
        {
            radPage.PageIndexChanging -= radPage_PageIndexChanging;
            if (e.NewPageIndex < radPage.PageCount)
            {
                CurrentPagging = e.NewPageIndex + 1;
                StartRow = (CurrentPagging - 1) * PaggingSize + 1;
                EndRow = StartRow + PaggingSize;
                //radpage = new RadDataPager();
                LoadDataPhanTrang();
                //radPage.PageIndex = CurrentPagging;
            }
            radPage.PageIndexChanging += radPage_PageIndexChanging;
        }
        #endregion

        #region Xử lý nghiệp vụ
        void LoadData()
        {
            Cursor = Cursors.Wait;
            //AutoCompleteEntry auLoaiSanPham = lstSourceLoaiSPham.ElementAt(cmbLoaiSanPham.SelectedIndex);

            DataTable dt = null;
            LDatatable.MakeParameterTable(ref dt);
            string TThaiNVu = ucTrangThaiNVu.GetItemsSelected();
            string ngayKiemSoatTu = "";
            string ngayKiemSoatDen = "";

            if (tvwKhuVuc.SelectedItem == null)
                tvwKhuVuc.SelectedItem = tvwKhuVuc.Items[0];

            string ListKVuc = "";
            if (((RadTreeViewItem)tvwKhuVuc.SelectedItem).Tag.ToString().Substring(0, 3).Equals("DVI"))
            {
                RadTreeViewItem itemDVI = (RadTreeViewItem)tvwKhuVuc.SelectedItem;
                foreach (RadTreeViewItem item in itemDVI.Items)
                {
                    if (item.Tag.ToString().Substring(0, 3).Equals("CNH"))
                        ListKVuc += ",''" + item.Tag.ToString() + "''";
                }
                ListKVuc = ListKVuc.Substring(3);
                ListKVuc = ListKVuc.Substring(0, ListKVuc.Length - 2);
            }
            else
                ListKVuc = ((RadTreeViewItem)tvwKhuVuc.SelectedItem).Tag.ToString();

            // Phân trang
            int StartRow = 1;
            int EndRow = ClientInformation.SoLuongBanGhi;
            int CurrentPagging = 1;
            int PaggingSize = ClientInformation.SoLuongBanGhi;

            if (teldtNgayKiemSoatTu.Value != null)
            {
                ngayKiemSoatTu = teldtNgayKiemSoatTu.Value.Value.ToString("yyyyMMdd");
            }

            if (teldtNgayKiemSoatDen.Value != null)
            {
                ngayKiemSoatDen = teldtNgayKiemSoatDen.Value.Value.ToString("yyyyMMdd");
            }

            TThaiNVu = TThaiNVu.Replace(@"''", @"'");

            LDatatable.AddParameter(ref dt, "@INP_MA_TRANG_THAI_NGHIEP_VU", "String", TThaiNVu);
            LDatatable.AddParameter(ref dt, "@INP_MA_PHIEU_KIEM_SOAT", "String", txtSoPhieuKiemSoat.Text.Trim());
            LDatatable.AddParameter(ref dt, "@INP_NGAY_KIEM_SOAT_TU", "String", ngayKiemSoatTu);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_KIEM_SOAT_DEN", "String", ngayKiemSoatDen);
            LDatatable.AddParameter(ref dt, "@INP_SO_KHE_UOC", "String", txtSoKheUoc.Text.Trim());
            LDatatable.AddParameter(ref dt, "@INP_SO_HDTD", "String", txtSoHDTD.Text.Trim());
            LDatatable.AddParameter(ref dt, "@INP_MA_KHACH_HANG", "String", txtMaKhachHang.Text.Trim());
            LDatatable.AddParameter(ref dt, "@INP_TEN_KHACH_HANG", "String", txtTenKhachHang.Text.Trim());
            LDatatable.AddParameter(ref dt, "@INP_KHUVUC", "String", ListKVuc);
            LDatatable.AddParameter(ref dt, "@INP_USERNAME", "String", ClientInformation.TenDangNhap);
            LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "String", ClientInformation.MaDonViQuanLy);
            LDatatable.AddParameter(ref dt, "@INP_START_ROW", "String", StartRow.ToString());
            LDatatable.AddParameter(ref dt, "@INP_END_ROW", "String", EndRow.ToString());

            DataSet ds = new TinDungProcess().GetDanhSachKiemSoatRuiRo(dt);
            dsRaw = ds;
            if (ds != null && ds.Tables.Count > 0)
            {
                dsRaw = ds;
                foreach (DataRow dr in ds.Tables[0].Rows)
                    dr["TTHAI_NVU_TEN"] = BusinessConstant.layNgonNguNghiepVu(dr["TTHAI_NVU_TEN"].ToString());
                DataTable tableRaw = ds.Tables[0].DefaultView.ToTable(true, "MA_KIEM_SOAT");
                DataTable serverDataTable = ds.Tables[0].Clone();
                foreach (DataRow dtr in tableRaw.Rows)
                {
                    DataRow dtrone = ds.Tables[0].Select("MA_KIEM_SOAT='" + dtr["MA_KIEM_SOAT"] + "'").FirstOrDefault();
                    if (!dtrone.IsNullOrEmpty())
                    {
                        serverDataTable.Rows.Add(dtrone.ItemArray);
                    }

                }
                int totalRecord = Int32.Parse(ds.Tables[1].Rows[0][0].ToString());
                DataTable clientDataTable = CommonFunction.BuildClientDataTable(serverDataTable, CurrentPagging, PaggingSize, totalRecord);

                //raddgrHDTDDS.ItemsSource = ds.Tables["DANH_SACH"].DefaultView;
                raddgrHDTDDS.ItemsSource = clientDataTable.DefaultView;
                if (!LObject.IsNullOrEmpty(raddgrHDTDDS.SelectedItems))
                    raddgrHDTDDS.SelectedItems.Clear();
                lblSumKhachHang.Content = totalRecord.ToString();
            }
            Cursor = Cursors.Arrow;
        }

        void LoadDataPhanTrang()
        {
            Cursor = Cursors.Wait;
            //AutoCompleteEntry auLoaiSanPham = lstSourceLoaiSPham.ElementAt(cmbLoaiSanPham.SelectedIndex);

            DataTable dt = null;
            LDatatable.MakeParameterTable(ref dt);
            string TThaiNVu = ucTrangThaiNVu.GetItemsSelected();
            string ngayKiemSoatTu = "";
            string ngayKiemSoatDen = "";

            if (tvwKhuVuc.SelectedItem == null)
                tvwKhuVuc.SelectedItem = tvwKhuVuc.Items[0];

            string ListKVuc = "";
            if (((RadTreeViewItem)tvwKhuVuc.SelectedItem).Tag.ToString().Substring(0, 3).Equals("DVI"))
            {
                RadTreeViewItem itemDVI = (RadTreeViewItem)tvwKhuVuc.SelectedItem;
                foreach (RadTreeViewItem item in itemDVI.Items)
                {
                    if (item.Tag.ToString().Substring(0, 3).Equals("CNH"))
                        ListKVuc += ",''" + item.Tag.ToString() + "''";
                }
                ListKVuc = ListKVuc.Substring(3);
                ListKVuc = ListKVuc.Substring(0, ListKVuc.Length - 2);
            }
            else
                ListKVuc = ((RadTreeViewItem)tvwKhuVuc.SelectedItem).Tag.ToString();

            // Phân trang
            int CurrentPagging = 1;
            int PaggingSize = ClientInformation.SoLuongBanGhi;

            if (teldtNgayKiemSoatTu.Value != null)
            {
                ngayKiemSoatTu = teldtNgayKiemSoatTu.Value.Value.ToString("yyyyMMdd");
            }

            if (teldtNgayKiemSoatDen.Value != null)
            {
                ngayKiemSoatDen = teldtNgayKiemSoatDen.Value.Value.ToString("yyyyMMdd");
            }

            TThaiNVu = TThaiNVu.Replace(@"''", @"'");

            LDatatable.AddParameter(ref dt, "@INP_MA_TRANG_THAI_NGHIEP_VU", "String", TThaiNVu);
            LDatatable.AddParameter(ref dt, "@INP_MA_PHIEU_KIEM_SOAT", "String", txtSoPhieuKiemSoat.Text.Trim());
            LDatatable.AddParameter(ref dt, "@INP_NGAY_KIEM_SOAT_TU", "String", ngayKiemSoatTu);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_KIEM_SOAT_DEN", "String", ngayKiemSoatDen);
            LDatatable.AddParameter(ref dt, "@INP_SO_KHE_UOC", "String", txtSoKheUoc.Text.Trim());
            LDatatable.AddParameter(ref dt, "@INP_SO_HDTD", "String", txtSoHDTD.Text.Trim());
            LDatatable.AddParameter(ref dt, "@INP_MA_KHACH_HANG", "String", txtMaKhachHang.Text.Trim());
            LDatatable.AddParameter(ref dt, "@INP_TEN_KHACH_HANG", "String", txtTenKhachHang.Text.Trim());
            LDatatable.AddParameter(ref dt, "@INP_KHUVUC", "String", ListKVuc);
            LDatatable.AddParameter(ref dt, "@INP_USERNAME", "String", ClientInformation.TenDangNhap);
            LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "String", ClientInformation.MaDonViQuanLy);
            LDatatable.AddParameter(ref dt, "@INP_START_ROW", "String", StartRow.ToString());
            LDatatable.AddParameter(ref dt, "@INP_END_ROW", "String", EndRow.ToString());

            DataSet ds = new TinDungProcess().GetDanhSachKiemSoatRuiRo(dt);
            dsRaw = ds;
            if (ds != null && ds.Tables.Count > 0)
            {
                dsRaw = ds;
                foreach (DataRow dr in ds.Tables[0].Rows)
                    dr["TTHAI_NVU_TEN"] = BusinessConstant.layNgonNguNghiepVu(dr["TTHAI_NVU_TEN"].ToString());
                DataTable tableRaw = ds.Tables[0].DefaultView.ToTable(true, "MA_KIEM_SOAT");
                DataTable serverDataTable = ds.Tables[0].Clone();
                foreach (DataRow dtr in tableRaw.Rows)
                {
                    DataRow dtrone = ds.Tables[0].Select("MA_KIEM_SOAT='" + dtr["MA_KIEM_SOAT"] + "'").FirstOrDefault();
                    if (!dtrone.IsNullOrEmpty())
                    {
                        serverDataTable.Rows.Add(dtrone.ItemArray);
                    }

                }
                int totalRecord = Int32.Parse(ds.Tables[1].Rows[0][0].ToString());
                DataTable clientDataTable = CommonFunction.BuildClientDataTable(serverDataTable, CurrentPagging, PaggingSize, totalRecord);

                //raddgrHDTDDS.ItemsSource = ds.Tables["DANH_SACH"].DefaultView;
                raddgrHDTDDS.ItemsSource = clientDataTable.DefaultView;
                if (!LObject.IsNullOrEmpty(raddgrHDTDDS.SelectedItems))
                    raddgrHDTDDS.SelectedItems.Clear();
                lblSumKhachHang.Content = totalRecord.ToString();
            }
            Cursor = Cursors.Arrow;
        }

        void ThemMoi()
        {
            if (!tlbAdd.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            ucKiemSoatRuiRoCT objHDTDThoaThuan = new ucKiemSoatRuiRoCT();
            objHDTDThoaThuan.Action = DatabaseConstant.Action.THEM;
            objHDTDThoaThuan.OnSavingCompleted += new EventHandler(objHDTDThoaThuan_OnSavingCompleted);
            string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.Function);
            Window window = new Window();
            window.Title = tittle;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Content = objHDTDThoaThuan;
            window.ShowDialog();
            Cursor = Cursors.Arrow;
        }

        void Sua()
        {
            if (!tlbModify.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            if (raddgrHDTDDS.SelectedItems.Count == 1)
            {
                DataRowView dr = (DataRowView)raddgrHDTDDS.SelectedItem;
                Cursor = Cursors.Wait;
                List<TD_KIEM_SOAT_RR> lst = new List<TD_KIEM_SOAT_RR>();
                lst = LayGiaTriObject();
                ucKiemSoatRuiRoCT objHDTDThoaThuan = new ucKiemSoatRuiRoCT(lst, maGDichKU,maGDichHD);
                objHDTDThoaThuan.obj = new TD_KIEM_SOAT_RR();
                objHDTDThoaThuan.obj = lst[0];

                objHDTDThoaThuan.objExt = new ThuocTinh.TD_KIEM_SOAT_RR_EXT();
                objHDTDThoaThuan.objExt = LayGiaTriObjectExt();

                objHDTDThoaThuan.Action = DatabaseConstant.Action.SUA;
                objHDTDThoaThuan.OnSavingCompleted += new EventHandler(objHDTDThoaThuan_OnSavingCompleted);
                string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.Function);
                Window window = new Window();
                window.Title = tittle;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Content = objHDTDThoaThuan;
                window.ShowDialog();
                Cursor = Cursors.Arrow;
            }
            else if (raddgrHDTDDS.SelectedItems.Count > 1)
            {
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
            }
            Cursor = Cursors.Arrow;
        }

        void Xem()
        {
            if (!tlbView.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            if (raddgrHDTDDS.SelectedItems.Count == 1)
            {
                DataRowView dr = (DataRowView)raddgrHDTDDS.SelectedItem;
                Cursor = Cursors.Wait;
                List<TD_KIEM_SOAT_RR> lst = new List<TD_KIEM_SOAT_RR>();
                lst = LayGiaTriObject();
                ucKiemSoatRuiRoCT objHDTDThoaThuan = new ucKiemSoatRuiRoCT(lst, maGDichKU, maGDichHD);
                objHDTDThoaThuan.obj = new TD_KIEM_SOAT_RR();
                objHDTDThoaThuan.obj = lst[0];

                objHDTDThoaThuan.objExt = new ThuocTinh.TD_KIEM_SOAT_RR_EXT();
                objHDTDThoaThuan.objExt = LayGiaTriObjectExt();

                objHDTDThoaThuan.Action = DatabaseConstant.Action.XEM;
                objHDTDThoaThuan.OnSavingCompleted += new EventHandler(objHDTDThoaThuan_OnSavingCompleted);
                string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.Function);
                Window window = new Window();
                window.Title = tittle;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Content = objHDTDThoaThuan;
                window.ShowDialog();
                Cursor = Cursors.Arrow;
            }
            else if (raddgrHDTDDS.SelectedItems.Count > 1)
            {
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
            }
            Cursor = Cursors.Arrow;
        }

        private TD_KIEM_SOAT_RR_EXT LayGiaTriObjectExt()
        {
            TD_KIEM_SOAT_RR_EXT obj = new TD_KIEM_SOAT_RR_EXT();
            DataRowView dr = (DataRowView)raddgrHDTDDS.SelectedItem;
            obj.obj = LayGiaTriObject()[0];
            obj.MA_KHANG = dr["MA_KHANG"].ToString();
            obj.TEN_KHANG = dr["TEN_KHANG"].ToString();
            obj.NGAY_SINH = dr["DD_NGAY_SINH"].ToString();
            obj.GTLQ_SO = dr["DD_GTLQ_SO"].ToString();
            obj.GTLQ_NGAY_CAP = dr["DD_GTLQ_NGAY_CAP"].ToString();
            return obj;
        }

        private List<TD_KIEM_SOAT_RR> LayGiaTriObject()
        {
            List<TD_KIEM_SOAT_RR> lst = new List<TD_KIEM_SOAT_RR>();
            
            DataRowView drv = (DataRowView)raddgrHDTDDS.SelectedItem;
            foreach (DataRow dr in dsRaw.Tables["DANH_SACH"].Select("MA_KIEM_SOAT='" + drv["MA_KIEM_SOAT"].ToString() + "'"))
            {
                TD_KIEM_SOAT_RR obj = new TD_KIEM_SOAT_RR();
                obj.ID = Convert.ToInt32(dr["ID"]);
                obj.MA_KIEM_SOAT = dr["MA_KIEM_SOAT"].ToString();
                obj.SO_KIEM_SOAT = dr["SO_KIEM_SOAT"].ToString();
                obj.NGAY_KIEM_SOAT = dr["NGAY_KIEM_SOAT"].ToString();
                obj.ID_KUOC = Convert.ToInt32(dr["ID_KUOC"]);
                obj.SO_KUOC = dr["SO_KUOC"].ToString();
                obj.MA_KUOC = dr["MA_KUOC"].ToString();
                obj.NGAY_KUOC = dr["NGAY_KUOC"].ToString();
                obj.ID_HDTD = Convert.ToInt32(dr["ID_HDTD"]);
                obj.SO_HDTD = dr["SO_HDTD"].ToString();
                obj.MA_HDTD = dr["MA_HDTD"].ToString();
                obj.NGAY_HOP_DONG = dr["NGAY_HOP_DONG"].ToString();
                obj.DIEN_GIAI = dr["DIEN_GIAI"].ToString();
                obj.MA_PHAN_HE = DatabaseConstant.Module.TDVM.getValue();
                obj.TTHAI_BGHI = dr["TTHAI_BGHI"].ToString();
                obj.TTHAI_NVU = dr["TTHAI_NVU"].ToString();
                obj.MA_DVI_QLY = dr["MA_DVI_QLY"].ToString();
                obj.MA_DVI_TAO = dr["MA_DVI_TAO"].ToString();
                obj.NGAY_NHAP = dr["NGAY_NHAP"].ToString();
                obj.NGUOI_NHAP = dr["NGUOI_NHAP"].ToString();
                obj.NGAY_CNHAT = dr["NGAY_CNHAT"].ToString();
                obj.NGUOI_CNHAT = dr["NGUOI_CNHAT"].ToString();
                maGDichHD = dr["SO_GDICH_HD"].ToString();
                maGDichKU = dr["SO_GDICH_KU"].ToString();
                lst.Add(obj);
            }
            return lst;
        }

        private List<TD_KIEM_SOAT_RR> LayListGiaTriObject()
        {
            List<TD_KIEM_SOAT_RR> lst = new List<TD_KIEM_SOAT_RR>();
            foreach (object itm in raddgrHDTDDS.SelectedItems)
            {
                TD_KIEM_SOAT_RR obj = new TD_KIEM_SOAT_RR();
                DataRowView dr = (DataRowView)itm;
                obj.ID = Convert.ToInt32(dr["ID"]);
                obj.MA_KIEM_SOAT = dr["MA_KIEM_SOAT"].ToString();
                obj.SO_KIEM_SOAT = dr["SO_KIEM_SOAT"].ToString();
                obj.NGAY_KIEM_SOAT = dr["NGAY_KIEM_SOAT"].ToString();
                obj.ID_KUOC = Convert.ToInt32(dr["ID_KUOC"]);
                obj.SO_KUOC = dr["SO_KUOC"].ToString();
                obj.MA_KUOC = dr["MA_KUOC"].ToString();
                obj.NGAY_KUOC = dr["NGAY_KUOC"].ToString();
                obj.ID_HDTD = Convert.ToInt32(dr["ID_HDTD"]);
                obj.SO_HDTD = dr["SO_HDTD"].ToString();
                obj.MA_HDTD = dr["MA_HDTD"].ToString();
                obj.NGAY_HOP_DONG = dr["NGAY_HOP_DONG"].ToString();
                obj.DIEN_GIAI = dr["DIEN_GIAI"].ToString();
                obj.MA_PHAN_HE = DatabaseConstant.Module.TDVM.getValue();
                obj.TTHAI_BGHI = dr["TTHAI_BGHI"].ToString();
                obj.TTHAI_NVU = dr["TTHAI_NVU"].ToString();
                obj.MA_DVI_QLY = dr["MA_DVI_QLY"].ToString();
                obj.MA_DVI_TAO = dr["MA_DVI_TAO"].ToString();
                obj.NGAY_NHAP = dr["NGAY_NHAP"].ToString();
                obj.NGUOI_NHAP = dr["NGUOI_NHAP"].ToString();
                obj.NGAY_CNHAT = dr["NGAY_CNHAT"].ToString();
                obj.NGUOI_CNHAT = dr["NGUOI_CNHAT"].ToString();
                lst.Add(obj);
            }
            return lst;
        }

        void Xoa()
        {
            if (!tlbDelete.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            List<int> lstID = new List<int>();
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = false;
            try
            {
                if (raddgrHDTDDS.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        TinDungProcess tindungProcess = new TinDungProcess();
                        List<ClientResponseDetail> ClientResponseDetail = new List<Presentation.Process.Common.ClientResponseDetail>();
                        List<TD_KIEM_SOAT_RR> lstKiemSoat = new List<TD_KIEM_SOAT_RR>();

                        lstKiemSoat = LayListGiaTriObject();
                        lstID = lstKiemSoat.Select(e => e.ID).ToList();

                        // Yêu cầu lock dữ liệu
                        retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                                                        DatabaseConstant.Function.TDVM_KIEM_SOAT_RR,
                                                        DatabaseConstant.Table.TD_KIEM_SOAT_RR,
                                                        DatabaseConstant.Action.XOA,
                                                        lstID);

                        int iret = tindungProcess.KiemSoatRuiRo(DatabaseConstant.Action.XOA, ref lstKiemSoat, ref ClientResponseDetail);
                        if (iret > 0)
                        {
                            LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);   
                        }
                        else
                        {
                            CommonFunction.ThongBaoKetQua(ClientResponseDetail);
                        }
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                // Yêu cầu Unlock dữ liệu
                retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                                                        DatabaseConstant.Function.TDVM_KIEM_SOAT_RR,
                                                        DatabaseConstant.Table.TD_KIEM_SOAT_RR,
                                                        DatabaseConstant.Action.XOA,
                                                        lstID);
                Cursor = Cursors.Arrow;
            }
        }

        void Duyet()
        {
            if (!tlbApprove.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            List<int> lstID = new List<int>();
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = false;
            try
            {
                if (raddgrHDTDDS.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        TinDungProcess tindungProcess = new TinDungProcess();
                        List<ClientResponseDetail> ClientResponseDetail = new List<Presentation.Process.Common.ClientResponseDetail>();
                        List<TD_KIEM_SOAT_RR> lstKiemSoat = new List<TD_KIEM_SOAT_RR>();

                        lstKiemSoat = LayListGiaTriObject();
                        lstID = lstKiemSoat.Select(e => e.ID).ToList();

                        // Yêu cầu lock dữ liệu
                        retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                                                        DatabaseConstant.Function.TDVM_KIEM_SOAT_RR,
                                                        DatabaseConstant.Table.TD_KIEM_SOAT_RR,
                                                        DatabaseConstant.Action.DUYET,
                                                        lstID);

                        int iret = tindungProcess.KiemSoatRuiRo(DatabaseConstant.Action.DUYET, ref lstKiemSoat, ref ClientResponseDetail);
                        if (iret > 0)
                        {
                            LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);
                        }
                        else
                        {
                            CommonFunction.ThongBaoKetQua(ClientResponseDetail);
                        }
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                // Yêu cầu Unlock dữ liệu
                retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                                                        DatabaseConstant.Function.TDVM_KIEM_SOAT_RR,
                                                        DatabaseConstant.Table.TD_KIEM_SOAT_RR,
                                                        DatabaseConstant.Action.DUYET,
                                                        lstID);
                Cursor = Cursors.Arrow;
            }
        }

        void ThoaiDuyet()
        {
            if (!tlbCancel.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            List<int> lstID = new List<int>();
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = false;
            try
            {
                if (raddgrHDTDDS.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        TinDungProcess tindungProcess = new TinDungProcess();
                        List<ClientResponseDetail> ClientResponseDetail = new List<Presentation.Process.Common.ClientResponseDetail>();
                        List<TD_KIEM_SOAT_RR> lstKiemSoat = new List<TD_KIEM_SOAT_RR>();

                        lstKiemSoat = LayListGiaTriObject();
                        lstID = lstKiemSoat.Select(e => e.ID).ToList();

                        // Yêu cầu lock dữ liệu
                        retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                                                        DatabaseConstant.Function.TDVM_KIEM_SOAT_RR,
                                                        DatabaseConstant.Table.TD_KIEM_SOAT_RR,
                                                        DatabaseConstant.Action.THOAI_DUYET,
                                                        lstID);

                        int iret = tindungProcess.KiemSoatRuiRo(DatabaseConstant.Action.THOAI_DUYET, ref lstKiemSoat, ref ClientResponseDetail);
                        if (iret > 0)
                        {
                            LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);
                        }
                        else
                        {
                            CommonFunction.ThongBaoKetQua(ClientResponseDetail);
                        }
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                // Yêu cầu Unlock dữ liệu
                retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                                                        DatabaseConstant.Function.TDVM_KIEM_SOAT_RR,
                                                        DatabaseConstant.Table.TD_KIEM_SOAT_RR,
                                                        DatabaseConstant.Action.THOAI_DUYET,
                                                        lstID);
                Cursor = Cursors.Arrow;
            }
        }

        void TuChoiDuyet()
        {
            if (!tlbRefuse.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            List<int> lstID = new List<int>();
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = false;
            try
            {
                if (raddgrHDTDDS.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        TinDungProcess tindungProcess = new TinDungProcess();
                        List<ClientResponseDetail> ClientResponseDetail = new List<Presentation.Process.Common.ClientResponseDetail>();
                        List<TD_KIEM_SOAT_RR> lstKiemSoat = new List<TD_KIEM_SOAT_RR>();

                        lstKiemSoat = LayListGiaTriObject();
                        lstID = lstKiemSoat.Select(e => e.ID).ToList();

                        // Yêu cầu lock dữ liệu
                        retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                                                        DatabaseConstant.Function.TDVM_KIEM_SOAT_RR,
                                                        DatabaseConstant.Table.TD_KIEM_SOAT_RR,
                                                        DatabaseConstant.Action.TU_CHOI_DUYET,
                                                        lstID);

                        int iret = tindungProcess.KiemSoatRuiRo(DatabaseConstant.Action.TU_CHOI_DUYET, ref lstKiemSoat, ref ClientResponseDetail);
                        if (iret > 0)
                        {
                            LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);
                        }
                        else
                        {
                            CommonFunction.ThongBaoKetQua(ClientResponseDetail);
                        }
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                // Yêu cầu Unlock dữ liệu
                retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                                                        DatabaseConstant.Function.TDVM_KIEM_SOAT_RR,
                                                        DatabaseConstant.Table.TD_KIEM_SOAT_RR,
                                                        DatabaseConstant.Action.TU_CHOI_DUYET,
                                                        lstID);
                Cursor = Cursors.Arrow;
            }
        }

        void objHDTDThoaThuan_OnSavingCompleted(object sender, EventArgs e)
        {
            LoadData();
        }
        #endregion

        
    }
}
