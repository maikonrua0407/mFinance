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
using Presentation.Process.Common;
using System.Reflection;

namespace PresentationWPF.DanhMuc.KhuVuc
{
    /// <summary>
    /// Interaction logic for ucKhuVucDS.xaml
    /// </summary>
    public partial class ucKhuVucDS : UserControl
    {
        #region Khai bao

        private DatabaseConstant.Module Module = DatabaseConstant.Module.DMDC;
        private DatabaseConstant.Function Function = DatabaseConstant.Function.DC_DM_KHU_VUC;
        private DatabaseConstant.Table Table = DatabaseConstant.Table.DM_KHU_VUC;

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
        public int Id;
        public string Ma;
        public string Ten;
        DataTable dtMaster = new DataTable();
        DataTable dtDetail = new DataTable();
        static List<HeaderDto> lstHeader;
        static bool columnsWidthLoad = false;
        private string parent = "";
        delegate void LoadDuLieuCT(int iD);
        public int idDonVi;
        public string maDonVi;
        private DatabaseConstant.Function function = DatabaseConstant.Function.DC_DM_KHU_VUC;
        #endregion

        #region Khoi tao

        public ucKhuVucDS()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.DanhMuc;component/KhuVuc/ucKhuVucDS.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindHotkey();
            radPage.PageSize = (int)nudPageSize.Value;
            LoadDuLieu();
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
                    if(key !=null)
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
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
                beforeView();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
                tvwTree_ItemClick(null, null);
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
            PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grKhuVucDS, txtTimKiemNhanh.Text);
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
        /// Hàm xử lý giải phóng lock dữ liệu cho form
        /// </summary>
        private void Release()
        {
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
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
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
            if (grKhuVucDS != null && grKhuVucDS.DataContext != null) 
            {
                DataTable dt = ((DataView)grKhuVucDS.DataContext).Table;
                if (dt != null)
                {
                    radPage.PageSize = (int)nudPageSize.Value;
                    grKhuVucDS.DataContext = dt.DefaultView;
                }
            }
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(grKhuVucDS);            
        }

        private void grDanhSach_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement originalSender = e.OriginalSource as FrameworkElement;
            if (originalSender != null)
            {
                beforeView();
            }
        }

        private void grDanhSach_Loaded(object sender, RoutedEventArgs e)
        {
            loadWidthColumn();
        }
        #endregion

        #region Xu ly nghiep vu

        /// <summary>
        /// Xử lý sự kiện thêm
        /// </summary>
        private void Them()
        {
            UserControl userControl = null;
            string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.function);
            Window frm = new Window();
            frm.Title = tittle;
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ApplicationConstant.DonViSuDung company = ApplicationConstant.layDonViSuDung(ClientInformation.Company);
            switch (company)
            {
                case ApplicationConstant.DonViSuDung.BANTAYVANG:
                    userControl = new ucKhuVucCT_01();
                    break;
                default:
                    userControl = new ucKhuVucCT();
                    break;
            }
            FieldInfo fieldAction = userControl.GetType().GetField("action");
            fieldAction.SetValue(userControl, DatabaseConstant.Action.THEM);
            //MethodInfo mi = userControl.GetType().GetMethod("SetDataForm");
            //object[] para = new object[1] { id };
            //mi.Invoke(userControl, para);
            //LoadDuLieuCT dlgLoadDuLieuCT = new LoadDuLieuCT(userControl.beforeViewFromList);
            //dlgLoadDuLieuCT();
            frm.Content = userControl;
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frm.ShowDialog();
        }

        /// <summary>
        /// Xử lý sự kiện sửa
        /// </summary>
        private void Sua()
        {
            beforeModify();
        }

        /// <summary>
        /// Xử lý xóa dữ liệu
        /// </summary>
        private void Xoa()
        {
            Cursor = Cursors.Wait;
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
                        List<int> lstid = new List<int>();
                        foreach (DataRowView drv in listDataRow)
                        {
                            lstid.Add(Convert.ToInt32(drv["ID"]));
                        }

                        // Cảnh báo người dùng
                        MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);
                        if (ret == MessageBoxResult.Yes)
                        {
                            // Yêu cầu lock bản ghi cần xử lý
                            UtilitiesProcess process = new UtilitiesProcess();
                            List<int> listLockedId = new List<int>();

                            bool retLockData = process.LockData(this.Module,
                                this.Function,
                                this.Table,
                                DatabaseConstant.Action.XOA,
                                lstid);

                            // Nếu lock thành công >> cho phép xử lý
                            if (retLockData)
                            {
                                OnDelete(lstid);
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
            catch (System.Exception ex)
            {
            	
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
            
        }

        private void OnDelete(List<int> lstid)
        {
            try
            {
                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                bool bKetQua = new DanhMucProcess().XoaKhuVuc(lstid.ToArray(), ref lstResponseDetail);
                AfterDelete(lstResponseDetail);
            }
            catch (System.Exception ex)
            {
            	
            }
            
        }

        private void AfterDelete(List<ClientResponseDetail> lstResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            tvwTree_ItemClick(null, null);
        }

        /// <summary>
        /// Lấy lại dữ liệu
        /// </summary>
        private void LayLai()
        {
            LoadDuLieu();
        }

        /// <summary>
        /// Load dữ liệu lên tree
        /// </summary>
        private void LoadDuLieu()
        {
            var process = new TruyVanProcess();
            List<string> lstDkLoc = new List<string>();
            lstDkLoc.Add(ucTrangThaiNVu.GetItemsSelected());
            lstDkLoc.Add(ucTrangThaiSDung.GetItemsSelected());
            if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()) ||
                ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()))
            {
                lstDkLoc.Add(ClientInformation.IdDonVi.ToString());
            }
            else
            {
                lstDkLoc.Add(DatabaseConstant.ID_TOCHUC.ToString());
            }
            lstDkLoc.Add(ClientInformation.TenDangNhap);
            lstDkLoc.Add(ClientInformation.MaDonViQuanLy);

            DanhSachResponse DanhSachResponse = process.getDanhSachInformation(DatabaseConstant.DanhSachTruyVan.DM_CUM_TREE.getValue(), lstDkLoc);
            if (DanhSachResponse.DataSetSource != null)
            {
                //lstHeader = DanhSachResponse.ListHeader.ToList();

                dtMaster = DanhSachResponse.DataSetSource.Tables[0];
                DataRow drRoot = dtMaster.Rows[0];

                RadTreeViewItem rootItem = new RadTreeViewItem();
                rootItem.Header = drRoot["NODE_NAME"].ToString();
                rootItem.Tag = drRoot["NODE"].ToString();
                rootItem.Uid = drRoot["NODE_TYPE"].ToString();
                //rootItem.IsExpanded = true;
                tvwKhuVuc.Items.Add(rootItem);
                BuildTree(rootItem);
            }
        }

        protected void BuildTree(RadTreeViewItem item)
        {
            List<DataRow> lstDataRow = null;
            lstDataRow = dtMaster.Select().OrderBy(row => row[2]).ToList();

            foreach (DataRow row in lstDataRow)
            {
                if (row["NODE_TYPE"].ToString() != "KHUVUC")
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
        }

        private void tvwTree_Checked(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            BuildGrid("");
            loadWidthColumn();
        }

        private void BuildGrid(string maDonVi)
        {
            Cursor = Cursors.Wait;
            try
            {
                var process = new TruyVanProcess();
                List<string> lstDkLoc = new List<string>();
                lstDkLoc.Add(ucTrangThaiNVu.GetItemsSelected());
                lstDkLoc.Add(ucTrangThaiSDung.GetItemsSelected());
                lstDkLoc.Add(maDonVi);
                DanhSachResponse DanhSachResponse = process.getDanhSachInformation(DatabaseConstant.DanhSachTruyVan.DM_KHUVUC.getValue(), lstDkLoc);
                if (DanhSachResponse.DataSetSource != null)
                {
                    lstHeader = DanhSachResponse.ListHeader.ToList();

                    //dtMaster = DanhSachResponse.DataSetSource.Tables[0];
                    dtDetail = DanhSachResponse.DataSetSource.Tables[0];
                }

                List<string> lst = new List<string>();
                lst.Add(maDonVi);
                DataTable grdData = new DataTable();

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
                            grdData.ImportRow(row);
                        }
                    }
                }

                if (grdData.Rows.Count > 0)
                    grKhuVucDS.ItemsSource = grdData.DefaultView;
                else
                    grKhuVucDS.ItemsSource = null;
                lblTongSo.Content = grdData.Rows.Count;
            }
            catch (System.Exception ex)
            {
            	
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
            
        }

        private void tvwTree_Unchecked(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            BuildGrid("");
            loadWidthColumn();
        }

        private void loadWidthColumn()
        {
            grKhuVucDS.SelectedItems.Clear();
            if (grKhuVucDS.Items.Count > 0)
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
                        grKhuVucDS.Columns[idx].Width = new Telerik.Windows.Controls.GridViewLength(width, unit);
                        grKhuVucDS.Columns[idx].IsReadOnly = true;
                        grKhuVucDS.Columns[idx].Header = LLanguage.SearchResourceByKey(item.LanguageKey);
                    }
                    else
                        grKhuVucDS.Columns[idx].IsVisible = false;
                    idx = idx + 1;
                }
                columnsWidthLoad = true;
            }
        }

        private void tvwTree_ItemClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            RadTreeViewItem item = tvwKhuVuc.SelectedItem as RadTreeViewItem;
            string loaiDonVi = item.Uid.ToString();
            string maDonVi = item.Tag.ToString();
            parent = maDonVi;

            this.maDonVi = maDonVi;

            if (maDonVi != null && (loaiDonVi.Equals(DatabaseConstant.ToChucDonVi.PGD.getValue()) || loaiDonVi.Equals(DatabaseConstant.ToChucDonVi.VPGD.getValue())))
            {
                BuildGrid(maDonVi);
                loadWidthColumn();
            }
        }

        /// <summary>
        /// Key down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvwTree_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RadTreeViewItem item = tvwKhuVuc.SelectedItem as RadTreeViewItem;
                string loaiDonVi = item.Uid.ToString();
                string maDonVi = item.Tag.ToString();
                parent = maDonVi;

                this.maDonVi = maDonVi;

                if (maDonVi != null && (loaiDonVi.Equals(DatabaseConstant.ToChucDonVi.PGD.getValue()) || loaiDonVi.Equals(DatabaseConstant.ToChucDonVi.VPGD.getValue())))
                {
                    BuildGrid(maDonVi);
                    loadWidthColumn();
                }
            }
        }

        /// <summary>
        /// Trước khi xem
        /// </summary>
        private void beforeView()
        {
            List<DataRowView> listDataRow = getListSeletedDataRow();
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
        /// Xem dữ liệu
        /// </summary>
        private void onView(int id)
        {
            UserControl userControl = null;
            string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.function);
            Window frm = new Window();
            frm.Title = tittle;
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ApplicationConstant.DonViSuDung company = ApplicationConstant.layDonViSuDung(ClientInformation.Company);
            switch (company)
            {
                case ApplicationConstant.DonViSuDung.BANTAYVANG:
                    userControl = new ucKhuVucCT_01();
                break;
                default:
                    userControl = new ucKhuVucCT();
                break;
            }
            FieldInfo fieldAction = userControl.GetType().GetField("action");
            fieldAction.SetValue(userControl, DatabaseConstant.Action.XEM);
            MethodInfo mi = userControl.GetType().GetMethod("SetDataForm");
            object[] para = new object[1] { id };
            mi.Invoke(userControl, para);
            //LoadDuLieuCT dlgLoadDuLieuCT = new LoadDuLieuCT(userControl.beforeViewFromList);
            //dlgLoadDuLieuCT();
            frm.Content = userControl;
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frm.ShowDialog();
        }

        /// <summary>
        /// Trước khi xem
        /// </summary>
        private void beforeModify()
        {
            List<DataRowView> listDataRow = getListSeletedDataRow();
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
        /// Xem dữ liệu
        /// </summary>
        private void onModify(int id)
        {
            UserControl userControl = null;
            string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.function);
            Window frm = new Window();
            frm.Title = tittle;
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ApplicationConstant.DonViSuDung company = ApplicationConstant.layDonViSuDung(ClientInformation.Company);
            switch (company)
            {
                case ApplicationConstant.DonViSuDung.BANTAYVANG:
                    userControl = new ucKhuVucCT_01();
                    break;
                default:
                    userControl = new ucKhuVucCT();
                    break;
            }
            FieldInfo fieldAction = userControl.GetType().GetField("action");
            fieldAction.SetValue(userControl, DatabaseConstant.Action.SUA);
            MethodInfo mi = userControl.GetType().GetMethod("SetDataForm");
            object[] para = new object[1] { id };
            mi.Invoke(userControl, para);
            //LoadDuLieuCT dlgLoadDuLieuCT = new LoadDuLieuCT(userControl.beforeViewFromList);
            //dlgLoadDuLieuCT();
            frm.Content = userControl;
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frm.ShowDialog();
        }

        private List<DataRowView> getListSeletedDataRow()
        {
            List<DataRowView> listDataRow = new List<DataRowView>();
            if (grKhuVucDS.SelectedItems.Count <= 0)
            {
                return listDataRow;
            }
            else
            {
                for (int i = 0; i < grKhuVucDS.SelectedItems.Count; i++)
                {
                    DataRowView dr = (DataRowView)grKhuVucDS.SelectedItems[i];
                    listDataRow.Add(dr);
                }
                return listDataRow;
            }
        }

        void userControl_OnSavingCompleted(object sender, EventArgs e)
        {
            if (maDonVi != null)
            {
                BuildGrid(maDonVi);
                loadWidthColumn();
            }
        }
        #endregion

    }
}
