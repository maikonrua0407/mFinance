using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
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
using Telerik.Windows.Controls;
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.ZAMainAppServiceRef;
using Presentation.Process.KhachHangServiceRef;
using Presentation.Process.BaoCaoServiceRef;
using PresentationWPF.CustomControl;

namespace PresentationWPF.KhachHang.KhachHang
{
    /// <summary>
    /// Interaction logic for ucKhachHangDS01.xaml
    /// </summary>
    public partial class ucKhachHangDS01 : UserControl
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

        private string gioi_tinh = DatabaseConstant.DanhMuc.GIOI_TINH.getValue();
        private string dan_toc = DatabaseConstant.DanhMuc.DAN_TOC.getValue();
        private string loai_hinh_cong_tac = "LOAI_HINH_CONG_TAC";

        List<AutoCompleteEntry> lstSourceNganhKinhTe = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiHinhToChuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiKhachHang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceGioiTinh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDanToc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLHinhCongTac = new List<AutoCompleteEntry>();

        private DataTable dtSourceTree = new DataTable();

        //Lưu danh sách các item được chọn trong treeview
        private List<string> lstChecked = new List<string>();

        // Phân trang
        int StartRow = 1;
        int EndRow = ClientInformation.SoLuongBanGhi;
        int CurrentPagging = 1;
        int PaggingSize = ClientInformation.SoLuongBanGhi;

        #endregion

        #region Khoi tao
        public ucKhachHangDS01()
        {
            InitializeComponent();
            txtTimKiemNhanh.Focus();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KhachHang;component/KhachHang/ucKhachHangDS01.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += tlbHotKey_Click;
            }
            BindHotkey();
            raddtTuNgayGiaNhap.Value = null;
            raddtDenNgayGiaNhap.Value = null;
            //radpage.PageSize = (int)nudPageSize.Value;
            radpage.PageIndexChanging += new EventHandler<PageIndexChangingEventArgs>(radpage_PageIndexChanging);
            //radpage.PageIndexChanged += new EventHandler<PageIndexChangedEventArgs>(radpage_PageIndexChanged);

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
            string strTinhNang = ""; // tlb.Name.Substring(3, tlb.Name.Length - 3);
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
        private void KhoiTaoCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDK = new List<string>();

            // Loại khách hàng
            lstDK.Add(DatabaseConstant.DanhMuc.LOAI_KHACH_HANG.getValue());
            auto.GenAutoComboBox(ref lstSourceNganhKinhTe, ref cmbLoaiKhachHang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK, null);

            //Load du lieu combobox GioiTinh
            lstDK.Clear();
            lstDK.Add(gioi_tinh);
            auto.GenAutoComboBox(ref lstSourceGioiTinh, ref cmbGioiTinh, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK, null);

            //Load du lieu combobox DanToc
            lstDK.Clear();
            lstDK.Add(dan_toc);
            auto.GenAutoComboBox(ref lstSourceDanToc, ref cmbDanToc, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK, null);

            //Combobox loại hình công tác
            lstDK.Clear();
            lstDK.Add(loai_hinh_cong_tac);
            auto.GenAutoComboBox(ref lstSourceLHinhCongTac, ref cmbLHCongTac, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK, null);

            // Ngành kinh tế
            lstDK.Clear();
            lstDK.Add(DatabaseConstant.DanhMuc.NGANH_KINH_TE.getValue());
            auto.GenAutoComboBox(ref lstSourceNganhKinhTe, ref cmbNganhKT, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK, null);

            // Loại hình tổ chức
            lstDK.Clear();
            lstDK.Add(DatabaseConstant.DanhMuc.LOAI_HINH_TO_CHUC.getValue());
            auto.GenAutoComboBox(ref lstSourceLoaiHinhToChuc, ref cmbLoaiHinhToChuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK, null);
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

        private void numTuoi_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(numTuoiTu.Value) > Convert.ToInt32(numTuoiTu.Value))
            {
                LMessage.ShowMessage("M.KhachHang.KhachHang.ucKhachHangDS01.TuoiTuLonHonDen", LMessage.MessageBoxType.Warning);
                numTuoiDen.Focus();
            }
        }
        //private void chkKhachHangDS_Click(object sender, RoutedEventArgs e)
        //{
        //    for (int i = 0; i < grKhachHangDS.Items.Count; i++)
        //    {
        //        DataRowView dr = (DataRowView)grKhachHangDS.Items[i];
        //        dr["CHON"] = chkKhachHangDS.IsChecked;
        //    }
        //}



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
        #endregion

        #region Xu ly nghiep vu
        /// <summary>
        /// Xử lý sự kiện thêm
        /// </summary>
        private void Them()
        {
            Window window = new Window();
            ucKhachHangCT01 uc = new ucKhachHangCT01();
            uc.LoaiKhachHang = BusinessConstant.LoaiKhachHang.TVIEN.layGiaTri();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.KH_THANH_VIEN);

            window.Content = uc;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }

        /// <summary>
        /// Xử lý sự kiện sửa
        /// </summary>
        private void Sua()
        {
            DataRowView dr = (DataRowView)grKhachHangDS.SelectedItem;
            if (dr == null)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                Window window = new Window();
                //ucKhachHangCT01 uc = new ucKhachHangCT01(Convert.ToInt32(dr["ID"]), dr["TTHAI_NVU"].ToString(), DatabaseConstant.Action.SUA);
                ucKhachHangCT01 uc = new ucKhachHangCT01();
                uc.ID = Convert.ToInt32(dr["ID"]);
                uc.Action = DatabaseConstant.Action.SUA;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.KH_THANH_VIEN);
                window.Content = uc;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
            }
        }

        /// <summary>
        /// Xử lý sự kiện sửa
        /// </summary>
        private void Xem()
        {
            DataRowView dr = (DataRowView)grKhachHangDS.SelectedItem;
            if (dr == null)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
            }
            else
            {
                Window window = new Window();
                ucKhachHangCT01 uc = new ucKhachHangCT01();
                uc.ID = Convert.ToInt32(dr["ID"]);
                uc.Action = DatabaseConstant.Action.XEM;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.KH_THANH_VIEN);
                window.Content = uc;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
            }
        }

        /// <summary>
        /// Xử lý xóa dữ liệu
        /// </summary>
        private void Xoa()
        {
            KhachHangProcess process = new KhachHangProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                foreach (DataRowView dr in grKhachHangDS.SelectedItems)
                {
                    Array.Resize(ref arrayID, arrayID.Length + 1);
                    arrayID[arrayID.Length - 1] = Convert.ToInt32(dr["ID"]);
                }

                List<KH_KHANG_HSO> lstKhachHang = new List<KH_KHANG_HSO>();
                KH_KHANG_HSO obj = null;
                for (int i = 0; i < arrayID.Length; i++)
                {
                    obj = new KH_KHANG_HSO();
                    obj.ID = arrayID[i];
                    lstKhachHang.Add(obj);
                }

                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                bool ret = process.DanhSachKhachHang01(DatabaseConstant.Action.XOA, ref lstKhachHang, ref lstResponseDetail);
                if (ret == true)
                {
                    LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
                    TimKiem();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(lstResponseDetail);
                    TimKiem();
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
                Mouse.OverrideCursor = Cursors.Arrow;
            }            
        }

        private void Duyet()
        {
            KhachHangProcess process = new KhachHangProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                foreach (DataRowView dr in grKhachHangDS.SelectedItems)
                {
                    Array.Resize(ref arrayID, arrayID.Length + 1);
                    arrayID[arrayID.Length - 1] = Convert.ToInt32(dr["ID"]);
                }

                List<KH_KHANG_HSO> lstKhachHang = new List<KH_KHANG_HSO>();
                KH_KHANG_HSO obj = null;
                for (int i = 0; i < arrayID.Length; i++)
                {
                    obj = new KH_KHANG_HSO();
                    obj.ID = arrayID[i];
                    lstKhachHang.Add(obj);
                }

                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                bool ret = process.DanhSachKhachHang01(DatabaseConstant.Action.DUYET, ref lstKhachHang, ref lstResponseDetail);
                if (ret == true)
                {
                    //LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);
                    CommonFunction.ThongBaoKetQua(lstResponseDetail);
                    TimKiem();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(lstResponseDetail);
                    TimKiem();
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
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void TuChoi()
        {
            KhachHangProcess process = new KhachHangProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                foreach (DataRowView dr in grKhachHangDS.SelectedItems)
                {
                    Array.Resize(ref arrayID, arrayID.Length + 1);
                    arrayID[arrayID.Length - 1] = Convert.ToInt32(dr["ID"]);
                }

                List<KH_KHANG_HSO> lstKhachHang = new List<KH_KHANG_HSO>();
                KH_KHANG_HSO obj = null;
                for (int i = 0; i < arrayID.Length; i++)
                {
                    obj = new KH_KHANG_HSO();
                    obj.ID = arrayID[i];
                    lstKhachHang.Add(obj);
                }

                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                bool ret = process.DanhSachKhachHang01(DatabaseConstant.Action.TU_CHOI_DUYET, ref lstKhachHang, ref lstResponseDetail);
                if (ret == true)
                {
                    LMessage.ShowMessage("M.DungChung.TuChoiThanhCong", LMessage.MessageBoxType.Information);
                    TimKiem();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(lstResponseDetail);
                    TimKiem();
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
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void ThoaiDuyet()
        {
            KhachHangProcess process = new KhachHangProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                foreach (DataRowView dr in grKhachHangDS.SelectedItems)
                {
                    Array.Resize(ref arrayID, arrayID.Length + 1);
                    arrayID[arrayID.Length - 1] = Convert.ToInt32(dr["ID"]);
                }

                List<KH_KHANG_HSO> lstKhachHang = new List<KH_KHANG_HSO>();
                KH_KHANG_HSO obj = null;
                for (int i = 0; i < arrayID.Length; i++)
                {
                    obj = new KH_KHANG_HSO();
                    obj.ID = arrayID[i];
                    lstKhachHang.Add(obj);
                }

                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                bool ret = process.DanhSachKhachHang01(DatabaseConstant.Action.THOAI_DUYET, ref lstKhachHang, ref lstResponseDetail);
                if (ret == true)
                {
                    LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);
                    TimKiem();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(lstResponseDetail);
                    TimKiem();
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
                Mouse.OverrideCursor = Cursors.Arrow;
            }
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
            string het_hieu_luc = cmbLoaiTThaiKHang.SelectedValue.ToString();
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

            // Thong tin ngay gia nhap
            if (raddtTuNgayGiaNhap.Value != null)
            {
                LDatatable.AddParameter(ref dt, "@NgayGiaNhapTu", "STRING", Convert.ToDateTime(raddtTuNgayGiaNhap.Value).ToString("yyyyMMdd"));
            }
            else
            {
                LDatatable.AddParameter(ref dt, "@NgayGiaNhapTu", "STRING", "");
            }

            if (raddtDenNgayGiaNhap.Value != null)
            {
                LDatatable.AddParameter(ref dt, "@NgayGiaNhapDen", "STRING", Convert.ToDateTime(raddtDenNgayGiaNhap.Value).ToString("yyyyMMdd"));
            }
            else
            {
                LDatatable.AddParameter(ref dt, "@NgayGiaNhapDen", "STRING", "");
            }

            // Lay du lieu tu cac combobox
            AutoComboBox au = new AutoComboBox();
            AutoCompleteEntry nganhKT = au.getEntryByDisplayName(lstSourceNganhKinhTe, ref cmbNganhKT);
            AutoCompleteEntry loaihinhToChuc = au.getEntryByDisplayName(lstSourceLoaiHinhToChuc, ref cmbLoaiHinhToChuc);
            AutoCompleteEntry loaiKhachHang = au.getEntryByDisplayName(lstSourceLoaiKhachHang, ref cmbLoaiKhachHang);
            if (nganhKT != null)
            {
                LDatatable.AddParameter(ref dt, "@NganhKT", "STRING", nganhKT.KeywordStrings[0]);
            }
            else
            {
                LDatatable.AddParameter(ref dt, "@NganhKT", "STRING", "");
            }

            if (loaihinhToChuc != null)
            {
                LDatatable.AddParameter(ref dt, "@LoaiHinhToChuc", "STRING", loaihinhToChuc.KeywordStrings[0]);
            }
            else
            {
                LDatatable.AddParameter(ref dt, "@LoaiHinhToChuc", "STRING", "");
            }

            if (loaiKhachHang != null)
            {
                LDatatable.AddParameter(ref dt, "@LoaiKhachHang", "STRING", loaiKhachHang.KeywordStrings[0]);
            }
            else
            {
                LDatatable.AddParameter(ref dt, "@LoaiKhachHang", "STRING", "");
            }

            // Them du lieu vao tim kiem
            if (ucTrangThaiNVu.GetItemsSelected() == "NULL")
            {
                LDatatable.AddParameter(ref dt, "@TrangThaiNghiepVu", "STRING", "");
            }
            else
            {
                LDatatable.AddParameter(ref dt, "@TrangThaiNghiepVu", "STRING", ucTrangThaiNVu.GetItemsSelected());
            }

            LDatatable.AddParameter(ref dt, "@MaKhachHang", "STRING", txtMaKH.Text);
            LDatatable.AddParameter(ref dt, "@TenKhachHang", "STRING", txtTenKH.Text);
            LDatatable.AddParameter(ref dt, "@DkienMPA", "STRING", "");
            if (cmbDanToc.SelectedIndex >= 0)
                LDatatable.AddParameter(ref dt, "@DanToc", "STRING", lstSourceDanToc.ElementAt(cmbDanToc.SelectedIndex).KeywordStrings.First());
            else
                LDatatable.AddParameter(ref dt, "@DanToc", "STRING", "");
            if (cmbGioiTinh.SelectedIndex >= 0)
                LDatatable.AddParameter(ref dt, "@GioiTinh", "STRING", lstSourceGioiTinh.ElementAt(cmbGioiTinh.SelectedIndex).KeywordStrings.First());
            else
                LDatatable.AddParameter(ref dt, "@GioiTinh", "STRING", "");
            if (cmbLHCongTac.SelectedIndex >= 0)
                LDatatable.AddParameter(ref dt, "@NgheNghiep", "STRING", lstSourceLHinhCongTac.ElementAt(cmbLHCongTac.SelectedIndex).KeywordStrings.First());
            else
                LDatatable.AddParameter(ref dt, "@NgheNghiep", "STRING", "");
            LDatatable.AddParameter(ref dt, "@TuoiTu", "STRING", Convert.ToInt32(numTuoiTu.Value).ToString());
            LDatatable.AddParameter(ref dt, "@TuoiDen", "STRING", Convert.ToInt32(numTuoiDen.Value).ToString());
            LDatatable.AddParameter(ref dt, "@NgayHienTai", "STRING", ClientInformation.NgayLamViecHienTai);
            LDatatable.AddParameter(ref dt, "@SoCMND", "STRING", txtCMND.Text);
            LDatatable.AddParameter(ref dt, "@SoDkyKDoanh", "STRING", txtSoDKKD.Text);
            LDatatable.AddParameter(ref dt, "@TenDangNhap", "STRING", ClientInformation.TenDangNhap);
            LDatatable.AddParameter(ref dt, "@DonViQLy", "STRING", ClientInformation.MaDonViQuanLy);
            LDatatable.AddParameter(ref dt, "@DonVi", "INT", ma_don_vi);
            LDatatable.AddParameter(ref dt, "@KhuVuc", "INT", ma_khu_vuc);
            LDatatable.AddParameter(ref dt, "@Cum", "INT", ma_cum);
            LDatatable.AddParameter(ref dt, "@Nhom", "INT", ma_nhom);
            LDatatable.AddParameter(ref dt, "@HetHieuLuc", "STRING", het_hieu_luc);
            LDatatable.AddParameter(ref dt, "@StartRow", "INT", StartRow.ToString());
            LDatatable.AddParameter(ref dt, "@EndRow", "INT", EndRow.ToString());

            // Tim kiem
            Presentation.Process.KhachHangProcess process = new Presentation.Process.KhachHangProcess();
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
                
                Presentation.Process.KhachHangProcess process = new Presentation.Process.KhachHangProcess();
                //DataSet ds = process.getTreeView(Presentation.Process.Common.ClientInformation.IdDonVi);
                //DataSet ds = process.getTreeView(1);

                // Nếu người dùng là NVDV, QTDV thì chỉ lấy thông tin đơn vị mình
                // Nếu người dùng là NVTW,... thì lấy thông tin toàn hệ thống
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

                DataSet ds = process.getTreeView(ClientInformation.MaDonViQuanLy, ClientInformation.TenDangNhap);
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
            string userName = ClientInformation.TenDangNhap;
            string het_hieu_luc = cmbLoaiTThaiKHang.SelectedValue.ToString();
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

            // Thong tin ngay gia nhap
            if (raddtTuNgayGiaNhap.Value != null)
            {
                LDatatable.AddParameter(ref dt, "@NgayGiaNhapTu", "STRING", Convert.ToDateTime(raddtTuNgayGiaNhap.Value).ToString("yyyyMMdd"));
            }
            else
            {
                LDatatable.AddParameter(ref dt, "@NgayGiaNhapTu", "STRING", "");
            }

            if (raddtDenNgayGiaNhap.Value != null)
            {
                LDatatable.AddParameter(ref dt, "@NgayGiaNhapDen", "STRING", Convert.ToDateTime(raddtDenNgayGiaNhap.Value).ToString("yyyyMMdd"));
            }
            else
            {
                LDatatable.AddParameter(ref dt, "@NgayGiaNhapDen", "STRING", "");
            }

            // Lay du lieu tu cac combobox
            AutoComboBox au = new AutoComboBox();
            AutoCompleteEntry nganhKT = au.getEntryByDisplayName(lstSourceNganhKinhTe, ref cmbNganhKT);
            AutoCompleteEntry loaihinhToChuc = au.getEntryByDisplayName(lstSourceLoaiHinhToChuc, ref cmbLoaiHinhToChuc);
            AutoCompleteEntry loaiKhachHang = au.getEntryByDisplayName(lstSourceLoaiKhachHang, ref cmbLoaiKhachHang);
            if (nganhKT != null)
            {
                LDatatable.AddParameter(ref dt, "@NganhKT", "STRING", nganhKT.KeywordStrings[0]);
            }
            else
            {
                LDatatable.AddParameter(ref dt, "@NganhKT", "STRING", "");
            }

            if (loaihinhToChuc != null)
            {
                LDatatable.AddParameter(ref dt, "@LoaiHinhToChuc", "STRING", loaihinhToChuc.KeywordStrings[0]);
            }
            else
            {
                LDatatable.AddParameter(ref dt, "@LoaiHinhToChuc", "STRING", "");
            }

            if (loaiKhachHang != null)
            {
                LDatatable.AddParameter(ref dt, "@LoaiKhachHang", "STRING", loaiKhachHang.KeywordStrings[0]);
            }
            else
            {
                LDatatable.AddParameter(ref dt, "@LoaiKhachHang", "STRING", "");
            }

            // Them du lieu vao tim kiem
            if (ucTrangThaiNVu.GetItemsSelected() == "NULL")
            {
                LDatatable.AddParameter(ref dt, "@TrangThaiNghiepVu", "STRING", "");
            }
            else
            {
                LDatatable.AddParameter(ref dt, "@TrangThaiNghiepVu", "STRING", ucTrangThaiNVu.GetItemsSelected());
            }
            LDatatable.AddParameter(ref dt, "@MaKhachHang", "STRING", txtMaKH.Text);
            LDatatable.AddParameter(ref dt, "@TenKhachHang", "STRING", txtTenKH.Text);
            LDatatable.AddParameter(ref dt, "@DkienMPA", "STRING", "");
            if (cmbDanToc.SelectedIndex >= 0)
                LDatatable.AddParameter(ref dt, "@DanToc", "STRING", lstSourceDanToc.ElementAt(cmbDanToc.SelectedIndex).KeywordStrings.First());
            else
                LDatatable.AddParameter(ref dt, "@DanToc", "STRING", "");
            if (cmbGioiTinh.SelectedIndex >= 0)
                LDatatable.AddParameter(ref dt, "@GioiTinh", "STRING", lstSourceGioiTinh.ElementAt(cmbGioiTinh.SelectedIndex).KeywordStrings.First());
            else
                LDatatable.AddParameter(ref dt, "@GioiTinh", "STRING", "");
            if (cmbLHCongTac.SelectedIndex >= 0)
                LDatatable.AddParameter(ref dt, "@NgheNghiep", "STRING", lstSourceLHinhCongTac.ElementAt(cmbLHCongTac.SelectedIndex).KeywordStrings.First());
            else
                LDatatable.AddParameter(ref dt, "@NgheNghiep", "STRING", "");
            LDatatable.AddParameter(ref dt, "@TuoiTu", "STRING", Convert.ToInt32(numTuoiTu.Value).ToString());
            LDatatable.AddParameter(ref dt, "@TuoiDen", "STRING", Convert.ToInt32(numTuoiDen.Value).ToString());
            LDatatable.AddParameter(ref dt, "@NgayHienTai", "STRING", ClientInformation.NgayLamViecHienTai);
            LDatatable.AddParameter(ref dt, "@SoCMND", "STRING", txtCMND.Text);
            LDatatable.AddParameter(ref dt, "@SoDkyKDoanh", "STRING", txtSoDKKD.Text);
            LDatatable.AddParameter(ref dt, "@TenDangNhap", "STRING", ClientInformation.TenDangNhap);
            LDatatable.AddParameter(ref dt, "@DonViQLy", "STRING", ClientInformation.MaDonViQuanLy);
            LDatatable.AddParameter(ref dt, "@DonVi", "INT", ma_don_vi);
            LDatatable.AddParameter(ref dt, "@KhuVuc", "INT", ma_khu_vuc);
            LDatatable.AddParameter(ref dt, "@Cum", "INT", ma_cum);
            LDatatable.AddParameter(ref dt, "@Nhom", "INT", ma_nhom);
            LDatatable.AddParameter(ref dt, "@HetHieuLuc", "STRING", het_hieu_luc);
            LDatatable.AddParameter(ref dt, "@StartRow", "INT", StartRow.ToString());
            LDatatable.AddParameter(ref dt, "@EndRow", "INT", EndRow.ToString());

            // Tim kiem
            Presentation.Process.KhachHangProcess process = new Presentation.Process.KhachHangProcess();
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

                //grKhachHangDS.ItemsSource = radpage.PagedSource;
                grKhachHangDS.ItemsSource = null;
                grKhachHangDS.ItemsSource = clientDataTable.DefaultView;
                if (!LObject.IsNullOrEmpty(grKhachHangDS.SelectedItem))
                    grKhachHangDS.SelectedItems.Clear();
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            DataTable dt = null;
            LDatatable.MakeParameterTable(ref dt);

            // Lay du lieu treeview cho tim kiem
            string ma_don_vi = "-1";
            string ma_khu_vuc = "-1";
            string ma_cum = "-1";
            string ma_nhom = "-1";
            string userName = ClientInformation.TenDangNhap;
            string het_hieu_luc = cmbLoaiTThaiKHang.SelectedValue.ToString();
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

            // Thong tin ngay gia nhap
            if (raddtTuNgayGiaNhap.Value != null)
            {
                LDatatable.AddParameter(ref dt, "@NgayGiaNhapTu", "STRING", Convert.ToDateTime(raddtTuNgayGiaNhap.Value).ToString("yyyyMMdd"));
            }
            else
            {
                LDatatable.AddParameter(ref dt, "@NgayGiaNhapTu", "STRING", "");
            }

            if (raddtDenNgayGiaNhap.Value != null)
            {
                LDatatable.AddParameter(ref dt, "@NgayGiaNhapDen", "STRING", Convert.ToDateTime(raddtDenNgayGiaNhap.Value).ToString("yyyyMMdd"));
            }
            else
            {
                LDatatable.AddParameter(ref dt, "@NgayGiaNhapDen", "STRING", "");
            }

            // Lay du lieu tu cac combobox
            AutoComboBox au = new AutoComboBox();
            AutoCompleteEntry nganhKT = au.getEntryByDisplayName(lstSourceNganhKinhTe, ref cmbNganhKT);
            AutoCompleteEntry loaihinhToChuc = au.getEntryByDisplayName(lstSourceLoaiHinhToChuc, ref cmbLoaiHinhToChuc);
            AutoCompleteEntry loaiKhachHang = au.getEntryByDisplayName(lstSourceLoaiKhachHang, ref cmbLoaiKhachHang);
            if (nganhKT != null)
            {
                LDatatable.AddParameter(ref dt, "@NganhKT", "STRING", nganhKT.KeywordStrings[0]);
            }
            else
            {
                LDatatable.AddParameter(ref dt, "@NganhKT", "STRING", "");
            }

            if (loaihinhToChuc != null)
            {
                LDatatable.AddParameter(ref dt, "@LoaiHinhToChuc", "STRING", loaihinhToChuc.KeywordStrings[0]);
            }
            else
            {
                LDatatable.AddParameter(ref dt, "@LoaiHinhToChuc", "STRING", "");
            }

            if (loaiKhachHang != null)
            {
                LDatatable.AddParameter(ref dt, "@LoaiKhachHang", "STRING", loaiKhachHang.KeywordStrings[0]);
            }
            else
            {
                LDatatable.AddParameter(ref dt, "@LoaiKhachHang", "STRING", "");
            }

            // Them du lieu vao tim kiem
            if (ucTrangThaiNVu.GetItemsSelected() == "NULL")
            {
                LDatatable.AddParameter(ref dt, "@TrangThaiNghiepVu", "STRING", "");
            }
            else
            {
                LDatatable.AddParameter(ref dt, "@TrangThaiNghiepVu", "STRING", ucTrangThaiNVu.GetItemsSelected());
            }
            LDatatable.AddParameter(ref dt, "@MaKhachHang", "STRING", txtMaKH.Text);
            LDatatable.AddParameter(ref dt, "@TenKhachHang", "STRING", txtTenKH.Text);
            LDatatable.AddParameter(ref dt, "@DkienMPA", "STRING", "");
            if (cmbDanToc.SelectedIndex >= 0)
                LDatatable.AddParameter(ref dt, "@DanToc", "STRING", lstSourceDanToc.ElementAt(cmbDanToc.SelectedIndex).KeywordStrings.First());
            else
                LDatatable.AddParameter(ref dt, "@DanToc", "STRING", "");
            if (cmbGioiTinh.SelectedIndex >= 0)
                LDatatable.AddParameter(ref dt, "@GioiTinh", "STRING", lstSourceGioiTinh.ElementAt(cmbGioiTinh.SelectedIndex).KeywordStrings.First());
            else
                LDatatable.AddParameter(ref dt, "@GioiTinh", "STRING", "");
            if (cmbLHCongTac.SelectedIndex >= 0)
                LDatatable.AddParameter(ref dt, "@NgheNghiep", "STRING", lstSourceLHinhCongTac.ElementAt(cmbLHCongTac.SelectedIndex).KeywordStrings.First());
            else
                LDatatable.AddParameter(ref dt, "@NgheNghiep", "STRING", "");
            LDatatable.AddParameter(ref dt, "@TuoiTu", "STRING", Convert.ToInt32(numTuoiTu.Value).ToString());
            LDatatable.AddParameter(ref dt, "@TuoiDen", "STRING", Convert.ToInt32(numTuoiDen.Value).ToString());
            LDatatable.AddParameter(ref dt, "@NgayHienTai", "STRING", ClientInformation.NgayLamViecHienTai);
            LDatatable.AddParameter(ref dt, "@SoCMND", "STRING", txtCMND.Text);
            LDatatable.AddParameter(ref dt, "@SoDkyKDoanh", "STRING", txtSoDKKD.Text);
            LDatatable.AddParameter(ref dt, "@TenDangNhap", "STRING", ClientInformation.TenDangNhap);
            LDatatable.AddParameter(ref dt, "@DonViQLy", "STRING", ClientInformation.MaDonViQuanLy);
            LDatatable.AddParameter(ref dt, "@DonVi", "INT", ma_don_vi);
            LDatatable.AddParameter(ref dt, "@KhuVuc", "INT", ma_khu_vuc);
            LDatatable.AddParameter(ref dt, "@Cum", "INT", ma_cum);
            LDatatable.AddParameter(ref dt, "@Nhom", "INT", ma_nhom);
            LDatatable.AddParameter(ref dt, "@HetHieuLuc", "STRING", het_hieu_luc);
            LDatatable.AddParameter(ref dt, "@StartRow", "INT", "0");
            LDatatable.AddParameter(ref dt, "@EndRow", "INT", "1000000000");

            //Xuat Excel
            ApplicationConstant.ResponseStatus retStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            Presentation.Process.BaoCaoServiceRef.FileBase fileResponse = new Presentation.Process.BaoCaoServiceRef.FileBase();
            List<Presentation.Process.BaoCaoServiceRef.FileBase> lstFileResponse = new List<Presentation.Process.BaoCaoServiceRef.FileBase>();
            string responseMessage = null;

            Presentation.Process.BaoCaoProcess process = new Presentation.Process.BaoCaoProcess();
            retStatus = process.XuatExcel(ref fileResponse, ref responseMessage, dt, DatabaseConstant.DanhSachXuatExcel.KHTV_DANH_SACH_KH);
            if (retStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                string fileReport = "";
                string folderReport = ClientInformation.TempDir;

                fileReport = folderReport + "\\" + fileResponse.FileName + "." + "xls";

                LFile.WriteFileFromByteArray(fileResponse.FileData, fileReport);
                System.Diagnostics.Process.Start(fileReport);
            }
            else
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                LMessage.ShowMessage(responseMessage, LMessage.MessageBoxType.Error);
                return;
            }

            Mouse.OverrideCursor = Cursors.Arrow;
        }
        #endregion

    }
}