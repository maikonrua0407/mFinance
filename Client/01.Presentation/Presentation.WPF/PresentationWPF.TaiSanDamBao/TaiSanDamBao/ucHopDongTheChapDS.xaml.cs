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
using PresentationWPF.CustomControl;
using Presentation.Process.TaiSanDamBaoServiceRef;

namespace PresentationWPF.TaiSanDamBao.TaiSanDamBao
{
    /// <summary>
    /// Interaction logic for ucHopDongTheChapDS.xaml
    /// </summary>
    public partial class ucHopDongTheChapDS : UserControl
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
        private DatabaseConstant.Function Function = DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP;
        private DataTable dtSourceTree = new DataTable();
        private TDVM_HOP_DONG_TCHAP objHDTC = null;
        List<int> lstId;
        //Lưu danh sách các item được chọn trong treeview
        private List<string> lstChecked = new List<string>();

        // Phân trang
        int StartRow = 1;
        int EndRow = ClientInformation.SoLuongBanGhi;
        int CurrentPagging = 1;
        int PaggingSize = ClientInformation.SoLuongBanGhi;
        private DataTable dtTreeDLy;
        #endregion

        #region Khoi tao
        public ucHopDongTheChapDS()
        {
            InitializeComponent();
            txtTimKiemNhanh.Focus();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KhachHang;component/KhachHang/ucHopDongTheChapDS.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += tlbHotKey_Click;
            }
            BindHotkey();
            //raddtTuNgayGiaNhap.Value = null;
            //raddtDenNgayGiaNhap.Value = null;
            ////radpage.PageSize = (int)nudPageSize.Value;
            radPage.PageIndexChanging += new EventHandler<PageIndexChangingEventArgs>(radPage_PageIndexChanging);
            grDSTaiSan.MouseDoubleClick += new MouseButtonEventHandler(grDSTaiSan_MouseDoubleClick);
            ////radpage.PageIndexChanged += new EventHandler<PageIndexChangedEventArgs>(radpage_PageIndexChanged);

            KhoiTaoCombobox();
            BuildTreeKhuVuc();
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
            BeforeDelete();
        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbApprove.IsEnabled;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeApprove();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbRefuse.IsEnabled;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeRefuse();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbCancel.IsEnabled;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeCancel();
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
            //TimKiem();
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
                Xem();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            {
                LoadDuLieu();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
            {
                //TimKiem();
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

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
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
            //auto.GenAutoComboBox(ref lstSourceNganhKinhTe, ref cmbLoaiKhachHang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK, null);

            //Load du lieu combobox GioiTinh
            lstDK.Clear();
            lstDK.Add(gioi_tinh);
            //auto.GenAutoComboBox(ref lstSourceGioiTinh, ref cmbGioiTinh, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK, null);

            //Load du lieu combobox DanToc
            lstDK.Clear();
            lstDK.Add(dan_toc);
            //auto.GenAutoComboBox(ref lstSourceDanToc, ref cmbDanToc, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK, null);

            //Combobox loại hình công tác
            lstDK.Clear();
            lstDK.Add(loai_hinh_cong_tac);
            //auto.GenAutoComboBox(ref lstSourceLHinhCongTac, ref cmbLHCongTac, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK, null);

            // Ngành kinh tế
            lstDK.Clear();
            lstDK.Add(DatabaseConstant.DanhMuc.NGANH_KINH_TE.getValue());
            //auto.GenAutoComboBox(ref lstSourceNganhKinhTe, ref cmbNganhKT, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK, null);

            // Loại hình tổ chức
            lstDK.Clear();
            lstDK.Add(DatabaseConstant.DanhMuc.LOAI_HINH_TO_CHUC.getValue());
            //auto.GenAutoComboBox(ref lstSourceLoaiHinhToChuc, ref cmbLoaiHinhToChuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK, null);
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


        void grDSTaiSan_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Xem();
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
            DataRow[] drChild = dtSourceTree.Select(condition).OrderBy(row => row[0]).ToArray();
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
                DataRow[] drChild = dtSourceTree.Select("path like '" + level[0] + "#" + parent[parent.Length - 1].Substring(0, parent[parent.Length - 1].Length - 4) + "/%'").OrderBy(row => row[0]).ToArray();

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

        private void radPage_PageIndexChanging(object sender, PageIndexChangingEventArgs e)
        {
            if (e.NewPageIndex < radPage.PageCount)
            {
                CurrentPagging = e.NewPageIndex + 1;
                StartRow = (CurrentPagging - 1) * PaggingSize + 1;
                EndRow = StartRow + PaggingSize;
                //radpage = new RadDataPager();
                LoadDuLieuPhanTrang();
            }
        }

        #endregion

        #region Xu ly nghiep vu
        /// <summary>
        /// Xử lý sự kiện thêm
        /// </summary>
        private void Them()
        {
            if (!tlbAdd.IsEnabled)
                return;
            string formCase = cmbKieuHopDong.SelectedValue.ToString();
            ucHopDongTheChapCT ucHopDong = new ucHopDongTheChapCT(DatabaseConstant.Action.THEM, formCase, 0, "");
            string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.Function);
            Window window = new Window();
            window.Title = tittle;
            window.Content = ucHopDong;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
            Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// Xử lý sự kiện sửa
        /// </summary>
        private void Sua()
        {
            if (!tlbModify.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            if (grDSTaiSan.SelectedItems.Count == 1)
            {
                DataRowView dr = (DataRowView)grDSTaiSan.SelectedItems[0];
                string formCase = dr["LOAI_HD"].ToString();
                int idHDTC = Convert.ToInt32(dr["ID"]);
                string maHDTC = dr["MA_HDTC"].ToString();
                ucHopDongTheChapCT objKheUocCT = new ucHopDongTheChapCT(DatabaseConstant.Action.SUA, formCase, idHDTC, maHDTC);
                string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.Function);
                Window window = new Window();
                window.Title = tittle;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Content = objKheUocCT;
                window.ShowDialog();
            }
            else if (grDSTaiSan.SelectedItems.Count > 1)
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
            Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// Xử lý sự kiện sửa
        /// </summary>
        private void Xem()
        {
            if (!tlbModify.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            if (grDSTaiSan.SelectedItems.Count == 1)
            {
                DataRowView dr = (DataRowView)grDSTaiSan.SelectedItems[0];
                string formCase = dr["LOAI_HD"].ToString();
                int idHDTC = Convert.ToInt32(dr["ID"]);
                string maHDTC = dr["MA_HDTC"].ToString();
                ucHopDongTheChapCT objKheUocCT = new ucHopDongTheChapCT(DatabaseConstant.Action.XEM, formCase, idHDTC, maHDTC);
                string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.Function);
                Window window = new Window();
                window.Title = tittle;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Content = objKheUocCT;
                window.ShowDialog();
            }
            else if (grDSTaiSan.SelectedItems.Count > 1)
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
            Cursor = Cursors.Arrow;
        }

        void BeforeDelete()
        {
            try
            {
                if (!tlbDelete.IsEnabled)
                    return;
                Cursor = Cursors.Wait;
                lstId = new List<int>();
                if (grDSTaiSan.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        if (LObject.IsNullOrEmpty(objHDTC)) objHDTC = new TDVM_HOP_DONG_TCHAP();
                        foreach (DataRowView dr in grDSTaiSan.SelectedItems)
                        {
                            lstId.Add(Convert.ToInt32(dr["ID"]));
                        }
                        objHDTC.DSACH_ID_XOA = lstId.ToArray();
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP,
                        DatabaseConstant.Table.TD_HDTC,
                        DatabaseConstant.Action.XOA,
                        lstId);
                        OnDelete();
                    }
                }

            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP,
                DatabaseConstant.Table.TD_HDTC,
                DatabaseConstant.Action.XOA,
                lstId);
                Cursor = Cursors.Arrow;
            }

        }
        void OnDelete()
        {
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (new TaiSanDamBaoProcess().HopDongTheChap(DatabaseConstant.Action.XOA, ref objHDTC, ref lstResponseDetail))
                iret = 1;
            AfterDelete(lstResponseDetail, iret);
        }
        void AfterDelete(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP,
            DatabaseConstant.Table.TD_HDTC,
            DatabaseConstant.Action.XOA,
            lstId);
            LoadDuLieu();
            Cursor = Cursors.Arrow;
        }

        void BeforeApprove()
        {
            try
            {
                if (!tlbDelete.IsEnabled)
                    return;
                Cursor = Cursors.Wait;
                lstId = new List<int>();
                if (grDSTaiSan.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        if (LObject.IsNullOrEmpty(objHDTC)) objHDTC = new TDVM_HOP_DONG_TCHAP();
                        foreach (DataRowView dr in grDSTaiSan.SelectedItems)
                        {
                            lstId.Add(Convert.ToInt32(dr["ID"]));
                        }
                        objHDTC.DSACH_ID_XOA = lstId.ToArray();
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP,
                        DatabaseConstant.Table.TD_HDTC,
                        DatabaseConstant.Action.DUYET,
                        lstId);
                        OnApprove();
                    }
                }

            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP,
                DatabaseConstant.Table.TD_HDTC,
                DatabaseConstant.Action.DUYET,
                lstId);
                Cursor = Cursors.Arrow;
            }

        }
        void OnApprove()
        {
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (new TaiSanDamBaoProcess().HopDongTheChap(DatabaseConstant.Action.DUYET, ref objHDTC, ref lstResponseDetail))
                iret = 1;
            AfterApprove(lstResponseDetail, iret);
        }
        void AfterApprove(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP,
            DatabaseConstant.Table.TD_HDTC,
            DatabaseConstant.Action.DUYET,
            lstId);
            LoadDuLieu();
            Cursor = Cursors.Arrow;
        }

        void BeforeRefuse()
        {
            try
            {
                if (!tlbDelete.IsEnabled)
                    return;
                Cursor = Cursors.Wait;
                lstId = new List<int>();
                if (grDSTaiSan.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        if (LObject.IsNullOrEmpty(objHDTC)) objHDTC = new TDVM_HOP_DONG_TCHAP();
                        foreach (DataRowView dr in grDSTaiSan.SelectedItems)
                        {
                            lstId.Add(Convert.ToInt32(dr["ID"]));
                        }
                        objHDTC.DSACH_ID_XOA = lstId.ToArray();
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP,
                        DatabaseConstant.Table.TD_HDTC,
                        DatabaseConstant.Action.TU_CHOI_DUYET,
                        lstId);
                        OnRefuse();
                    }
                }

            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP,
                DatabaseConstant.Table.TD_HDTC,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);
                Cursor = Cursors.Arrow;
            }

        }
        void OnRefuse()
        {
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (new TaiSanDamBaoProcess().HopDongTheChap(DatabaseConstant.Action.TU_CHOI_DUYET, ref objHDTC, ref lstResponseDetail))
                iret = 1;
            AfterRefuse(lstResponseDetail, iret);
        }
        void AfterRefuse(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP,
            DatabaseConstant.Table.TD_HDTC,
            DatabaseConstant.Action.TU_CHOI_DUYET,
            lstId);
            LoadDuLieu();
            Cursor = Cursors.Arrow;
        }

        void BeforeCancel()
        {
            try
            {
                if (!tlbDelete.IsEnabled)
                    return;
                Cursor = Cursors.Wait;
                lstId = new List<int>();
                if (grDSTaiSan.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        if (LObject.IsNullOrEmpty(objHDTC)) objHDTC = new TDVM_HOP_DONG_TCHAP();
                        foreach (DataRowView dr in grDSTaiSan.SelectedItems)
                        {
                            lstId.Add(Convert.ToInt32(dr["ID"]));
                        }
                        objHDTC.DSACH_ID_XOA = lstId.ToArray();
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP,
                        DatabaseConstant.Table.TD_HDTC,
                        DatabaseConstant.Action.THOAI_DUYET,
                        lstId);
                        OnCancel();
                    }
                }

            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP,
                DatabaseConstant.Table.TD_HDTC,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                Cursor = Cursors.Arrow;
            }

        }
        void OnCancel()
        {
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (new TaiSanDamBaoProcess().HopDongTheChap(DatabaseConstant.Action.THOAI_DUYET, ref objHDTC, ref lstResponseDetail))
                iret = 1;
            AfterCancel(lstResponseDetail, iret);
        }
        void AfterCancel(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP,
            DatabaseConstant.Table.TD_HDTC,
            DatabaseConstant.Action.THOAI_DUYET,
            lstId);
            LoadDuLieu();
            Cursor = Cursors.Arrow;
        }

        void LoadDuLieu()
        {
            try
            {
                string sMaTrangThaiNVu = ucTrangThaiNVu.GetItemsSelected();
                if (tvwKhachHangDS.SelectedItem == null)
                    tvwKhachHangDS.SelectedItem = tvwKhachHangDS.Items[0];
                string ListKVuc = "";
                if (((RadTreeViewItem)tvwKhachHangDS.SelectedItem).Tag.ToString().Substring(0, 3).Equals("DVI"))
                {
                    RadTreeViewItem itemDVI = (RadTreeViewItem)tvwKhachHangDS.SelectedItem;
                    foreach (RadTreeViewItem item in itemDVI.Items)
                    {
                        if (item.Tag.ToString().Substring(0, 3).Equals("CNH"))
                            ListKVuc += ",''" + item.Tag.ToString() + "''";
                    }
                    ListKVuc = ListKVuc.Substring(3);
                    ListKVuc = ListKVuc.Substring(0, ListKVuc.Length - 2);
                }
                else
                    ListKVuc = ((RadTreeViewItem)tvwKhachHangDS.SelectedItem).Tag.ToString();

                // Phân trang
                int StartRow = 1;
                int EndRow = ClientInformation.SoLuongBanGhi;
                int CurrentPagging = 1;
                int PaggingSize = ClientInformation.SoLuongBanGhi;
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                //@INP_MA_TRANG_THAI_NGHIEP_VU#@INP_SO_HDTC#@INP_NGAY_HD_TU#@INP_NGAY_HD_DEN#@INP_MA_TSDB#@INP_TEN_TSDB#@INP_TONG_GTRI_TU#@INP_TONG_GTRI_DEN#@INP_MA_KHANG#@INP_TEN_KHANG#@INP_MA_GTO#@INP_SO_GTO#@INP_DIEN_THOAI#@INP_EMAIL
                string ngayHDTu = "";
                string ngayHDDen = "";
                if (!raddtTuNgayGiaNhap.Value.IsNullOrEmpty())
                    ngayHDTu = raddtTuNgayGiaNhap.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                if (!raddtDenNgayGiaNhap.Value.IsNullOrEmpty())
                    ngayHDDen = raddtDenNgayGiaNhap.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                string ngayHHLucTu = "";
                string ngayHHLucDen = "";
                if (!raddtTuNgayHHLuc.Value.IsNullOrEmpty())
                    ngayHHLucTu = raddtTuNgayHHLuc.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                if (!raddtDenNgayHHLuc.Value.IsNullOrEmpty())
                    ngayHHLucDen = raddtDenNgayHHLuc.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                LDatatable.AddParameter(ref dt, "@INP_MA_TRANG_THAI_NGHIEP_VU", "string", sMaTrangThaiNVu);
                LDatatable.AddParameter(ref dt, "@INP_NGAY_HD_TU", "string", ngayHDTu);
                LDatatable.AddParameter(ref dt, "@INP_NGAY_HD_DEN", "string", ngayHDDen);
                LDatatable.AddParameter(ref dt, "@INP_NGAY_HHLUC_TU", "string", ngayHHLucTu);
                LDatatable.AddParameter(ref dt, "@INP_NGAY_HHLUC_DEN", "string", ngayHHLucDen);
                LDatatable.AddParameter(ref dt, "@INP_SO_HDTC", "string", txtSoHD.Text);
                LDatatable.AddParameter(ref dt, "@INP_MA_KHANG", "string", txtMaKH.Text);
                LDatatable.AddParameter(ref dt, "@INP_TEN_KHANG", "string", txtTenKH.Text);
                LDatatable.AddParameter(ref dt, "@INP_KIEU_HDONG", "string", cmbKieuHopDong.SelectedValue.ToString());
                LDatatable.AddParameter(ref dt, "@INP_KHUVUC", "string", ListKVuc);
                LDatatable.AddParameter(ref dt, "@INP_USER", "string", ClientInformation.TenDangNhap);
                LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "string", ClientInformation.MaDonViQuanLy);
                LDatatable.AddParameter(ref dt, "@StartRow", "string", StartRow.ToString());
                LDatatable.AddParameter(ref dt, "@EndRow", "string", EndRow.ToString());
                DataSet ds = new TaiSanDamBaoProcess().GetDanhSachHopDongTheChap(dt);

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable serverDataTable = ds.Tables[0];
                    int totalRecord = Int32.Parse(ds.Tables[1].Rows[0][0].ToString());
                    decimal totalSum = Decimal.Parse(ds.Tables[2].Rows[0][0].ToString());
                    DataTable clientDataTable = CommonFunction.BuildClientDataTable(serverDataTable, CurrentPagging, PaggingSize, totalRecord);

                    //grdKheUocDS.ItemsSource = ds.Tables["DANH_SACH"].DefaultView;
                    grDSTaiSan.ItemsSource = clientDataTable.DefaultView;
                    grDSTaiSan.SelectedItems.Clear();
                    lblSumKhachHang.Content = totalRecord.ToString();
                    lblSumDuNo.Content = totalSum.ToString("N0");
                }

            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        void LoadDuLieuPhanTrang()
        {
            try
            {
                string sMaTrangThaiNVu = ucTrangThaiNVu.GetItemsSelected();
                if (tvwKhachHangDS.SelectedItem == null)
                    tvwKhachHangDS.SelectedItem = tvwKhachHangDS.Items[0];
                string ListKVuc = "";
                if (((RadTreeViewItem)tvwKhachHangDS.SelectedItem).Tag.ToString().Substring(0, 3).Equals("DVI"))
                {
                    RadTreeViewItem itemDVI = (RadTreeViewItem)tvwKhachHangDS.SelectedItem;
                    foreach (RadTreeViewItem item in itemDVI.Items)
                    {
                        if (item.Tag.ToString().Substring(0, 3).Equals("CNH"))
                            ListKVuc += ",''" + item.Tag.ToString() + "''";
                    }
                    ListKVuc = ListKVuc.Substring(3);
                    ListKVuc = ListKVuc.Substring(0, ListKVuc.Length - 2);
                }
                else
                    ListKVuc = ((RadTreeViewItem)tvwKhachHangDS.SelectedItem).Tag.ToString();

                // Phân trang
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                string ngayHDTu = "";
                string ngayHDDen = "";
                if (!raddtTuNgayGiaNhap.Value.IsNullOrEmpty())
                    ngayHDTu = raddtTuNgayGiaNhap.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                if (!raddtDenNgayGiaNhap.Value.IsNullOrEmpty())
                    ngayHDDen = raddtDenNgayGiaNhap.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                LDatatable.AddParameter(ref dt, "@INP_MA_TRANG_THAI_NGHIEP_VU", "string", sMaTrangThaiNVu);
                LDatatable.AddParameter(ref dt, "@INP_NGAY_HD_TU", "string", ngayHDTu);
                LDatatable.AddParameter(ref dt, "@INP_NGAY_HD_DEN", "string", ngayHDDen);
                LDatatable.AddParameter(ref dt, "@INP_SO_HDTC", "string", txtSoHD.Text);
                LDatatable.AddParameter(ref dt, "@INP_MA_KHANG", "string", txtMaKH.Text);
                LDatatable.AddParameter(ref dt, "@INP_TEN_KHANG", "string", txtTenKH.Text);
                LDatatable.AddParameter(ref dt, "@INP_KIEU_HDONG", "string", cmbKieuHopDong.SelectedValue.ToString());
                LDatatable.AddParameter(ref dt, "@INP_KHUVUC", "string", ListKVuc);
                LDatatable.AddParameter(ref dt, "@INP_USER", "string", ClientInformation.TenDangNhap);
                LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "string", ClientInformation.MaDonViQuanLy);
                LDatatable.AddParameter(ref dt, "@StartRow", "string", StartRow.ToString());
                LDatatable.AddParameter(ref dt, "@EndRow", "string", EndRow.ToString());
                DataSet ds = new TaiSanDamBaoProcess().GetDanhSachHopDongTheChap(dt);

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable serverDataTable = ds.Tables[0];
                    int totalRecord = Int32.Parse(ds.Tables[1].Rows[0][0].ToString());
                    decimal totalSum = Decimal.Parse(ds.Tables[2].Rows[0][0].ToString());
                    DataTable clientDataTable = CommonFunction.BuildClientDataTable(serverDataTable, CurrentPagging, PaggingSize, totalRecord);

                    //grdKheUocDS.ItemsSource = ds.Tables["DANH_SACH"].DefaultView;
                    grDSTaiSan.ItemsSource = clientDataTable.DefaultView;
                    grDSTaiSan.SelectedItems.Clear();
                    lblSumKhachHang.Content = totalRecord.ToString();
                    lblSumDuNo.Content = totalSum.ToString("N0");
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Tìm kiếm nâng cao
        /// </summary>

        /// <summary>
        /// Load dữ liệu lên form
        /// </summary>
        void BuildTreeKhuVuc()
        {
            dtTreeDLy = new TinDungProcess().getDanhSachDonVi(ClientInformation.MaDonViQuanLy, ClientInformation.TenDangNhap).Tables[0];
            try
            {
                RadTreeViewItem Item = new RadTreeViewItem();
                //Item.Header = "Danh mục địa lý";
                //Item.IsExpanded = true;
                //Item.IsChecked = true;
                //tvwKhachHangDS.Items.Add(Item);
                BuildSubTreeKhuVuc(Item, null, 0);
            }
            catch (Exception ex)
            { }
        }

        void BuildSubTreeKhuVuc(RadTreeViewItem Item, DataRow dr, int iLevel)
        {

            List<DataRow> lstDataRow = null;
            if (dr != null)
                lstDataRow = dtTreeDLy.Select("MA_DVI_CHA='" + dr["MA_DVI"] + "' AND LEVEL=" + iLevel).OrderBy(row => row[4]).ToList();
            //lstDataRow = dtTreeDLy.Select("MA_DVI_CHA='" + dr["MA_DVI"] + "' AND LEVEL=" + iLevel).ToList();
            else
                lstDataRow = dtTreeDLy.Select("MA_DVI_CHA=''").OrderBy(row => row[4]).ToList();
            //lstDataRow = dtTreeDLy.Select("MA_DVI_CHA=''").ToList();
            foreach (DataRow row in lstDataRow)
            {
                RadTreeViewItem subItem = new RadTreeViewItem();
                subItem.Header = row["TEN_GDICH"].ToString();
                subItem.Tag = row["MA_DVI"].ToString();
                //subItem.IsExpanded = true;
                subItem.IsChecked = true;
                if (row["LEVEL"].Equals(0))
                    tvwKhachHangDS.Items.Add(subItem);
                else
                    Item.Items.Add(subItem);
                BuildSubTreeKhuVuc(subItem, row, Convert.ToInt32(row["LEVEL"]) + 1);
            }
        }

        #endregion

        private void nudPageSize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {

        }
    }
}

