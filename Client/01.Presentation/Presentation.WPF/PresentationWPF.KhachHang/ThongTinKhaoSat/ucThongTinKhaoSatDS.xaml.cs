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
using Utilities.Common;
using PresentationWPF.CustomControl;
using Presentation.Process;
using Telerik.Windows.Controls;
using Presentation.Process.Common;
using Presentation.Process.TinDungTDServiceRef;
using Presentation.Process.KhachHangServiceRef;

namespace PresentationWPF.KhachHang.ThongTinKhaoSat
{
    /// <summary>
    /// Interaction logic for ucThongTinKhaoSatDS.xaml
    /// </summary>
    public partial class ucThongTinKhaoSatDS : UserControl
    {
        #region Khai bao

        private int currentPosition;
        private int currentPage;
        private int currentID;

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
        private List<string> lstCheckedKhuVuc = new List<string>();
        private List<string> lstCheckedSanPham = new List<string>();
        private DatabaseConstant.Module Module = DatabaseConstant.Module.KHTV;
        private DatabaseConstant.Function Function = DatabaseConstant.Function.KH_THONG_TIN_KHAO_SAT_DS;        
        DataTable dtTreeDLy;
        private string loaiDXVV = "";

        // Phân trang
        int StartRow = 1;
        int EndRow = ClientInformation.SoLuongBanGhi;
        int CurrentPagging = 1;
        int PaggingSize = ClientInformation.SoLuongBanGhi;
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucThongTinKhaoSatDS()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KhachHang;component/ThongTinKhaoSat/ucThongTinKhaoSatDS.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindHotkey();
            //radPage.PageSize = (int)nudPageSize.Value;
            nudPageSize.Value = ClientInformation.SoLuongBanGhi;
            radPage.PageSize = (int)nudPageSize.Value;
            radPage.PageIndexChanging += new EventHandler<PageIndexChangingEventArgs>(radPage_PageIndexChanging);
            loaiDXVV = ClientInformation.FormCase;
            KhoiTaoComboBox();
            BuildTreeKhuVuc();
            ClearForm();
            //LoadData();
        }
        void BuildTreeKhuVuc()
        {
            dtTreeDLy = new TinDungProcess().getDanhSachDonVi(ClientInformation.MaDonViQuanLy,ClientInformation.TenDangNhap).Tables[0];
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
                lstDataRow = dtTreeDLy.Select("MA_DVI_CHA='" + dr["MA_DVI"] + "' AND LEVEL=" + iLevel).OrderBy(row => row[4]).ToList();
                //lstDataRow = dtTreeDLy.Select("MA_DVI_CHA='" + dr["MA_DVI"] + "' AND LEVEL=" + iLevel).ToList();
            else
                lstDataRow = dtTreeDLy.Select("LEVEL=0").OrderBy(row => row[4]).ToList();
                //lstDataRow = dtTreeDLy.Select("LEVEL=0").ToList();
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
        void KhoiTaoComboBox()
        {
            
        }

        void KhoiTaoGiaTriComboBox(ref List<AutoCompleteEntry> lstAutoComplete, string maTruyVan, RadComboBox cmbCommon, List<string> lstDieuKien)
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            // Gen ComboBox bằng việc gọi hàm
            auto.GenAutoComboBox(ref lstAutoComplete, ref cmbCommon, maTruyVan, lstDieuKien);
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
                LoadData();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
            {
                LoadData();
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
                LoadData();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
            {
                LoadData();
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
        /// Trợ giúp
        /// </summary>
        private void onHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }

        #endregion

        /// <summary>
        /// Xu ly giao dien
        /// </summary>
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
            PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grDanhSach, txtTimKiemNhanh.Text);
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
            if (grDanhSach != null && grDanhSach.ItemsSource != null)
            {
                DataView dt = ((DataView)grDanhSach.ItemsSource);
                if (dt != null)
                {
                    radPage.PageSize = (int)nudPageSize.Value;
                    grDanhSach.DataContext = dt;
                }
            }
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(grDanhSach);
            //PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel("GET_DS_DONVI");
        }

        /// <summary>
        /// Sự kiện chọn ngày của DatetimePicker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DatePicker dtpControl = (DatePicker)sender;
                StringBuilder sbControl = new StringBuilder();
                sbControl.Append("teldt");
                sbControl.Append(dtpControl.Name.Substring(3));
                RadMaskedDateTimeInput telControl = (RadMaskedDateTimeInput)grMain.FindName(sbControl.ToString());
                if (telControl != null)
                    telControl.Value = dtpControl.SelectedDate;
                else
                    throw new System.NullReferenceException("Không tìm thấy control " + sbControl.ToString());
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void grDanhSach_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BeforeView();
        }

        private void ClearForm()
        {
        }

        private void radPage_PageIndexChanging(object sender, PageIndexChangingEventArgs e)
        {
            if (e.NewPageIndex < radPage.PageCount)
            {
                CurrentPagging = e.NewPageIndex + 1;
                StartRow = (CurrentPagging - 1) * PaggingSize + 1;
                EndRow = StartRow + PaggingSize;
                //radpage = new RadDataPager();
                LoadDataPhanTrang();
            }
        }
        #endregion

        #region Xử lý nghiệp vụ
        void LoadData()
        {
            Cursor = Cursors.Wait;
            string TThaiNVu = ucTrangThaiNVu.GetItemsSelected();
            if (tvwKhuVuc.SelectedItem == null)
                tvwKhuVuc.SelectedItem = tvwKhuVuc.Items[0];

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

            DataTable dt = null;
            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_TRANG_THAI_NGHIEP_VU", "string", TThaiNVu);
            LDatatable.AddParameter(ref dt, "@MA_DXVV", "string", txtSoDXVV.Text.Trim());
            LDatatable.AddParameter(ref dt, "@MA_KHANG", "string", txtMaKhachHang.Text.Trim());
            LDatatable.AddParameter(ref dt, "@TEN_KHANG", "string", txtTenKhachHang.Text.Trim());
            LDatatable.AddParameter(ref dt, "@INP_KHUVUC", "string", ListKVuc);
            LDatatable.AddParameter(ref dt, "@INP_LOAIDXVV", "string", loaiDXVV);
            LDatatable.AddParameter(ref dt, "@INP_USERNAME", "string", ClientInformation.TenDangNhap);
            LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "string", ClientInformation.MaDonViQuanLy);
            LDatatable.AddParameter(ref dt, "@START", "String", StartRow.ToString());
            LDatatable.AddParameter(ref dt, "@END", "String", EndRow.ToString());
            DataSet ds = new KhachHangProcess().DanhSachThongTinKhaoSat(dt);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable serverDataTable = ds.Tables[0];
                foreach (DataRow dr in serverDataTable.Rows)
                    dr["TEN_TTHAI_NVU"] = BusinessConstant.layNgonNguNghiepVu(dr["TTHAI_NVU"].ToString());

                int totalRecord = Int32.Parse(ds.Tables[1].Rows[0][0].ToString());
                DataTable clientDataTable = CommonFunction.BuildClientDataTable(serverDataTable, CurrentPagging, PaggingSize, totalRecord);

                //grDanhSach.ItemsSource = ds.Tables["DANH_SACH"].DefaultView;
                grDanhSach.ItemsSource = clientDataTable.DefaultView;
                if (!LObject.IsNullOrEmpty(grDanhSach.SelectedItems))
                    grDanhSach.SelectedItems.Clear();
                lblSumKhachHang.Content = totalRecord.ToString();
            }
            Cursor = Cursors.Arrow;
        }

        void LoadDataPhanTrang()
        {
            Cursor = Cursors.Wait;
            string TThaiNVu = ucTrangThaiNVu.GetItemsSelected();
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

            DataTable dt = null;
            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_TRANG_THAI_NGHIEP_VU", "string", TThaiNVu);
            LDatatable.AddParameter(ref dt, "@MA_DXVV", "string", txtSoDXVV.Text.Trim());
            LDatatable.AddParameter(ref dt, "@MA_KHANG", "string", txtMaKhachHang.Text.Trim());
            LDatatable.AddParameter(ref dt, "@TEN_KHANG", "string", txtTenKhachHang.Text.Trim());
            LDatatable.AddParameter(ref dt, "@INP_KHUVUC", "string", ListKVuc);
            LDatatable.AddParameter(ref dt, "@INP_LOAIDXVV", "string", loaiDXVV);
            LDatatable.AddParameter(ref dt, "@INP_USERNAME", "string", ClientInformation.TenDangNhap);
            LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "string", ClientInformation.MaDonViQuanLy);
            LDatatable.AddParameter(ref dt, "@START", "String", StartRow.ToString());
            LDatatable.AddParameter(ref dt, "@END", "String", EndRow.ToString());
            DataSet ds = new KhachHangProcess().DanhSachThongTinKhaoSat(dt);
            int PaggingSize = ClientInformation.SoLuongBanGhi;
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable serverDataTable = ds.Tables[0];

                foreach (DataRow dr in serverDataTable.Rows)
                    dr["TEN_TTHAI_NVU"] = BusinessConstant.layNgonNguNghiepVu(dr["TTHAI_NVU"].ToString());

                int totalRecord = Int32.Parse(ds.Tables[1].Rows[0][0].ToString());
                DataTable clientDataTable = CommonFunction.BuildClientDataTable(serverDataTable, CurrentPagging, PaggingSize, totalRecord);

                //grDanhSach.ItemsSource = ds.Tables["DANH_SACH"].DefaultView;
                grDanhSach.ItemsSource = clientDataTable.DefaultView;
                if (!LObject.IsNullOrEmpty(grDanhSach.SelectedItems))
                    grDanhSach.SelectedItems.Clear();
                lblSumKhachHang.Content = totalRecord.ToString();
            }
            Cursor = Cursors.Arrow;
        }

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
            ucThongTinKhaoSatCT userControl = new ucThongTinKhaoSatCT();

            userControl.Action = DatabaseConstant.Action.THEM;
            userControl.OnSavingCompleted += new EventHandler(userControl_OnSavingCompleted);
            if (ClientInformation.FormCase.Equals("KSTD"))
                window.Title = LLanguage.SearchResourceByKey("MENU.TDTD_PHIEU_KS_CT");
            if (ClientInformation.FormCase.Equals("KSVM"))
                window.Title = LLanguage.SearchResourceByKey("MENU.TDVM_PHIEU_KS_CT");
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Content = userControl;
            window.ShowDialog();
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
                        currentPage = grDanhSach.Items.PageIndex;
                        currentPosition = grDanhSach.Items.CurrentPosition;
                        currentID = int.Parse(listDataRow.First()["ID"].ToString());

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
                ucThongTinKhaoSatCT userControl = new ucThongTinKhaoSatCT();

                userControl.OnSavingCompleted += new EventHandler(userControl_OnSavingCompleted);
                userControl.Action = DatabaseConstant.Action.SUA;
                userControl.ID = id;

                Window window = new Window();
                if (ClientInformation.FormCase.Equals("KSTD"))
                    window.Title = LLanguage.SearchResourceByKey("MENU.TDTD_PHIEU_KS_CT");
                if (ClientInformation.FormCase.Equals("KSVM"))
                    window.Title = LLanguage.SearchResourceByKey("MENU.TDVM_PHIEU_KS_CT");
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Content = userControl;
                window.ShowDialog();
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
                        currentPage = grDanhSach.Items.PageIndex;
                        currentPosition = grDanhSach.Items.CurrentPosition;
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
                ucThongTinKhaoSatCT userControl = new ucThongTinKhaoSatCT();

                userControl.OnSavingCompleted += new EventHandler(userControl_OnSavingCompleted);
                userControl.Action = DatabaseConstant.Action.XEM;
                userControl.ID = id;

                Window window = new Window();
                //window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.KH_THONG_TIN_KHAO_SAT_CT);
                if (ClientInformation.FormCase.Equals("KSTD"))
                    window.Title = LLanguage.SearchResourceByKey("MENU.TDTD_PHIEU_KS_CT");
                if (ClientInformation.FormCase.Equals("KSVM"))
                    window.Title = LLanguage.SearchResourceByKey("MENU.TDVM_PHIEU_KS_CT");
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Content = userControl;
                window.ShowDialog();
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
                            OnDelete(listId);

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
            KhachHangProcess ProcessKhachHang = new KhachHangProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                List<KH_THONG_TIN_KHAO_SAT> lst = new List<KH_THONG_TIN_KHAO_SAT>();

                KH_THONG_TIN_KHAO_SAT obj = null;
                foreach (int id in listId)
                {
                    obj = new KH_THONG_TIN_KHAO_SAT();
                    obj.ID = id;
                    lst.Add(obj);
                }
                bool ret = ProcessKhachHang.DanhSachThongTinKhaoSat(DatabaseConstant.Action.XOA, ref lst, ref listClientResponseDetail);

                AfterDelete(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
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
                LoadData();
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                LoadData();
            }

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
                            OnApprove(listId);

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
            KhachHangProcess ProcessKhachHang = new KhachHangProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                List<KH_THONG_TIN_KHAO_SAT> lst = new List<KH_THONG_TIN_KHAO_SAT>();

                KH_THONG_TIN_KHAO_SAT obj = null;
                foreach (int id in listId)
                {
                    obj = new KH_THONG_TIN_KHAO_SAT();
                    obj.ID = id;
                    lst.Add(obj);
                }
                bool ret = ProcessKhachHang.DanhSachThongTinKhaoSat(DatabaseConstant.Action.DUYET, ref lst, ref listClientResponseDetail);

                AfterApprove(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {              
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
                LoadData();
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                LoadData();
            }

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
                            OnCancel(listId);

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
            KhachHangProcess ProcessKhachHang = new KhachHangProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                List<KH_THONG_TIN_KHAO_SAT> lst = new List<KH_THONG_TIN_KHAO_SAT>();

                KH_THONG_TIN_KHAO_SAT obj = null;
                foreach (int id in listId)
                {
                    obj = new KH_THONG_TIN_KHAO_SAT();
                    obj.ID = id;
                    lst.Add(obj);
                }
                bool ret = ProcessKhachHang.DanhSachThongTinKhaoSat(DatabaseConstant.Action.THOAI_DUYET, ref lst, ref listClientResponseDetail);

                AfterCancel(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
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
                    LoadData();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    LoadData();
                }

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
                            OnRefuse(listId);

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
            KhachHangProcess ProcessKhachHang = new KhachHangProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                List<KH_THONG_TIN_KHAO_SAT> lst = new List<KH_THONG_TIN_KHAO_SAT>();

                KH_THONG_TIN_KHAO_SAT obj = null;
                foreach (int id in listId)
                {
                    obj = new KH_THONG_TIN_KHAO_SAT();
                    obj.ID = id;
                    lst.Add(obj);
                }
                bool ret = ProcessKhachHang.DanhSachThongTinKhaoSat(DatabaseConstant.Action.TU_CHOI_DUYET, ref lst, ref listClientResponseDetail);

                AfterRefuse(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
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
                    LoadData();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    LoadData();
                }

            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
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

                if (grDanhSach.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < grDanhSach.SelectedItems.Count; i++)
                    {
                        DataRowView dr = (DataRowView)grDanhSach.SelectedItems[i];
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

        void userControl_OnSavingCompleted(object sender, EventArgs e)
        {
            LoadData();            
            CommonFunction.GoToPosition(currentID, ref grDanhSach, radPage, nudPageSize);
        }
        #endregion
    }
}
