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
using Presentation.Process;
using Presentation.Process.HanMucServiceRef;

namespace PresentationWPF.HanMuc.HanMucKhachHang
{
    /// <summary>
    /// Interaction logic for ucHanMucDS.xaml
    /// </summary>
    public partial class ucHanMucDS : UserControl
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

        private DatabaseConstant.Function _function = DatabaseConstant.Function.HM_TONG;

        #endregion

        #region Khoi tao
        public ucHanMucDS()
        {
            InitializeComponent();
            txtTimKiemNhanh.Focus();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.HanMuc;component/HanMucKhachHang/ucHanMucDS.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += tlbHotKey_Click;
            }
            BindHotkey();
            //radpage.PageSize = (int)nudPageSize.Value;
            radpage.PageIndexChanging += new EventHandler<PageIndexChangingEventArgs>(radpage_PageIndexChanging);
            //radpage.PageIndexChanged += new EventHandler<PageIndexChangedEventArgs>(radpage_PageIndexChanged);

            KhoiTaoCombobox();
            KhoiTaoTreeHanMuc();
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

        }

        private void KhoiTaoTreeHanMuc()
        {
            dtSourceTree = new HanMucProcess().getTreeViewHanMucTong(ClientInformation.MaDonViQuanLy, ClientInformation.TenDangNhap).Tables[0];
            try
            {
                RadTreeViewItem Item = new RadTreeViewItem();
                BuildSubTreeKhuVuc(Item, null, 0);
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void BuildSubTreeKhuVuc(RadTreeViewItem Item, DataRow dr, int iLevel)
        {

            List<DataRow> lstDataRow = null;
            if (dr != null)
                lstDataRow = dtSourceTree.Select("MA_DVI_CHA='" + dr["MA_DVI"] + "' AND LEVEL=" + iLevel).ToList();
            else
                lstDataRow = dtSourceTree.Select("LEVEL=0").ToList();
            foreach (DataRow row in lstDataRow)
            {
                RadTreeViewItem subItem = new RadTreeViewItem();
                subItem.Header = row["TEN_GDICH"].ToString();
                subItem.Tag = row["MA_DVI"].ToString();
                if(iLevel == 0 || row["ID"].ToString().Equals(ClientInformation.IdDonViGiaoDich.ToString()) || row["ID"].ToString().Equals(ClientInformation.IdDonVi.ToString()))
                {
                    subItem.IsExpanded = true;
                }
                subItem.IsChecked = true;
                if (row["LEVEL"].Equals(0))
                    tvwKhachHangDS.Items.Add(subItem);
                else
                    Item.Items.Add(subItem);
                BuildSubTreeKhuVuc(subItem, row, Convert.ToInt32(row["LEVEL"]) + 1);
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

        #endregion

        #region Xu ly nghiep vu
        /// <summary>
        /// Xử lý sự kiện thêm
        /// </summary>
        private void Them()
        {
            Window window = new Window();
            ucHanMucTong uc = new ucHanMucTong();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.HM_TONG);
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
                KIEM_SOAT obj = new KIEM_SOAT();
                obj.action = DatabaseConstant.Action.SUA;
                obj.ID = Convert.ToInt32(dr["ID"]);
                ucHanMucTong uc = new ucHanMucTong(obj);
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.HM_TONG);
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
                KIEM_SOAT obj = new KIEM_SOAT();
                obj.action = DatabaseConstant.Action.XEM;
                obj.ID = Convert.ToInt32(dr["ID"]);
                ucHanMucTong uc = new ucHanMucTong(obj);
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.HM_TONG);
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
            try
            {
                HanMucProcess process = new HanMucProcess();
                int[] arrayID = new int[0];
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    foreach (DataRowView dr in grKhachHangDS.SelectedItems)
                    {
                        Array.Resize(ref arrayID, arrayID.Length + 1);
                        arrayID[arrayID.Length - 1] = Convert.ToInt32(dr["ID"]);
                    }
                    HM_HMUC_TONG obj = new HM_HMUC_TONG();
                    obj.lstID = arrayID;
                    List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                    ApplicationConstant.ResponseStatus ret = process.HanMucTong(_function,DatabaseConstant.Action.XOA, ref obj, ref lstResponseDetail);
                    CommonFunction.ThongBaoKetQua(lstResponseDetail);
                    TimKiem();
                }
                catch (System.Exception ex)
                {
                    this.Cursor = Cursors.Arrow;
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void Duyet()
        {
            try
            {
                HanMucProcess process = new HanMucProcess();
                int[] arrayID = new int[0];
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    foreach (DataRowView dr in grKhachHangDS.SelectedItems)
                    {
                        Array.Resize(ref arrayID, arrayID.Length + 1);
                        arrayID[arrayID.Length - 1] = Convert.ToInt32(dr["ID"]);
                    }
                    HM_HMUC_TONG obj = new HM_HMUC_TONG();
                    obj.lstID = arrayID;
                    List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                    ApplicationConstant.ResponseStatus ret = process.HanMucTong(_function,DatabaseConstant.Action.DUYET, ref obj, ref lstResponseDetail);
                    CommonFunction.ThongBaoKetQua(lstResponseDetail);
                    TimKiem();
                }
                catch (System.Exception ex)
                {
                    this.Cursor = Cursors.Arrow;
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void TuChoi()
        {
            try
            {
                HanMucProcess process = new HanMucProcess();
                int[] arrayID = new int[0];
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    foreach (DataRowView dr in grKhachHangDS.SelectedItems)
                    {
                        Array.Resize(ref arrayID, arrayID.Length + 1);
                        arrayID[arrayID.Length - 1] = Convert.ToInt32(dr["ID"]);
                    }
                    HM_HMUC_TONG obj = new HM_HMUC_TONG();
                    obj.lstID = arrayID;
                    List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                    ApplicationConstant.ResponseStatus ret = process.HanMucTong(_function,DatabaseConstant.Action.TU_CHOI_DUYET, ref obj, ref lstResponseDetail);
                    CommonFunction.ThongBaoKetQua(lstResponseDetail);
                    TimKiem();
                }
                catch (System.Exception ex)
                {
                    this.Cursor = Cursors.Arrow;
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void ThoaiDuyet()
        {
            try
            {
                HanMucProcess process = new HanMucProcess();
                int[] arrayID = new int[0];
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    foreach (DataRowView dr in grKhachHangDS.SelectedItems)
                    {
                        Array.Resize(ref arrayID, arrayID.Length + 1);
                        arrayID[arrayID.Length - 1] = Convert.ToInt32(dr["ID"]);
                    }
                    HM_HMUC_TONG obj = new HM_HMUC_TONG();
                    obj.lstID = arrayID;
                    List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                    ApplicationConstant.ResponseStatus ret = process.HanMucTong(_function,DatabaseConstant.Action.THOAI_DUYET, ref obj, ref lstResponseDetail);
                    CommonFunction.ThongBaoKetQua(lstResponseDetail);
                    TimKiem();
                }
                catch (System.Exception ex)
                {
                    this.Cursor = Cursors.Arrow;
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            catch (System.Exception ex)
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
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);

                // Load dữ liệu lên grid
                Presentation.Process.HanMucProcess processHanMuc = new HanMucProcess();
                if (ucTrangThaiNVu.GetItemsSelected() == "NULL")
                {
                    LDatatable.AddParameter(ref dt, "@MA_TRANG_THAI_NGHIEP_VU", "STRING", "");
                }
                else
                {
                    LDatatable.AddParameter(ref dt, "@MA_TRANG_THAI_NGHIEP_VU", "STRING", ucTrangThaiNVu.GetItemsSelected());
                }

                LDatatable.AddParameter(ref dt, "@MA_HAN_MUC", "STRING", txtMaHanMuc.Text);
                LDatatable.AddParameter(ref dt, "@HMUC_PDUYET_TU", "DECIMAL", telHMPheDuyetTu.Value.Value.ToString());
                LDatatable.AddParameter(ref dt, "@HMUC_PDUYET_DEN", "DECIMAL", telHMPheDuyetDen.Value.Value.ToString());
                LDatatable.AddParameter(ref dt, "@HMUC_KDUNG_TU", "DECIMAL", telHMKhaDungTu.Value.Value.ToString());
                LDatatable.AddParameter(ref dt, "@HMUC_KDUNG_DEN", "DECIMAL", telHMKhaDungDen.Value.Value.ToString());
                LDatatable.AddParameter(ref dt, "@MA_KHANG", "STRING", txtMaKhachHang.Text.Trim());
                LDatatable.AddParameter(ref dt, "@TEN_KHANG", "STRING", txtTenKhachHang.Text.Trim());
                LDatatable.AddParameter(ref dt, "@TEN_DANG_NHAP", "STRING", ClientInformation.TenDangNhap);
                LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", ClientInformation.MaDonVi);
                LDatatable.AddParameter(ref dt, "@START_ROW", "INT", "0");
                LDatatable.AddParameter(ref dt, "@END_ROW", "INT", "0");

                Presentation.Process.HanMucProcess process = new Presentation.Process.HanMucProcess();
                DataSet ds = process.GetDanhSachHanMuc(dt);
                if (ds != null && ds.Tables.Count > 0)
                {
                    grKhachHangDS.ItemsSource = ds.Tables[0].DefaultView;
                }
                else
                {
                    grKhachHangDS.ItemsSource = null;
                }
                

                // Phân trang
                int StartRow = 1;
                int EndRow = ClientInformation.SoLuongBanGhi;
                int CurrentPagging = 1;
                int PaggingSize = ClientInformation.SoLuongBanGhi;
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Tìm kiếm phân trang
        /// </summary>
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