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
using Telerik.Windows.Controls;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.TinDungServiceRef;

namespace PresentationWPF.TinDungTT.KUOC
{
    /// <summary>
    /// Interaction logic for ucKheUocDS_01.xaml
    /// </summary>
    public partial class ucKheUocDS_01 : UserControl
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
        private DataTable  dtTreeDLy;
        private List<AutoCompleteEntry> lstLoaiGiayTo = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstDonViTinhThoiHan = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstSanPhamTinDungTT = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstNhomNo = new List<AutoCompleteEntry>();
        private DatabaseConstant.Module Module = DatabaseConstant.Module.TDVM;
        private DatabaseConstant.Function Function = DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01;
        List<int> lstId = new List<int>();
        TDVM_KHE_UOC_DSACH _KUOCVM = new TDVM_KHE_UOC_DSACH();

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
        public ucKheUocDS_01()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDungTT;component/KUOC/ucKheUocDS_01.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindHotkey();
            radPage.PageSize = PaggingSize;
            radPage.PageIndexChanging += new EventHandler<PageIndexChangingEventArgs>(radPage_PageIndexChanging);
            BuildTreeKhuVuc();
            KhoiTaoComboBox();
            ClearForm();
            InitEventHanler();
            //LoadDuLieu();
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
                BuildSubTreeKhuVuc(Item,null,0);
            }
            catch (Exception ex)
            { }
        }

        void BuildSubTreeKhuVuc(RadTreeViewItem Item,DataRow dr,int iLevel)
            {

                List<DataRow> lstDataRow=null;
                if (dr != null)
                    lstDataRow = dtTreeDLy.Select("MA_DVI_CHA='" + dr["MA_DVI"] + "' AND LEVEL=" + iLevel).ToList();
                else
                    lstDataRow = dtTreeDLy.Select("MA_DVI_CHA=''").ToList();
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
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add("''" + ClientInformation.MaDonVi + "''");
                lstDieuKien.Add("0");
                lstDieuKien.Add("0");
                lstDieuKien.Add("0");
                string Dislay = LLanguage.SearchResourceByKey("U.DungChung.TatCa");
                lstSanPhamTinDungTT.Add(new AutoCompleteEntry(Dislay, "%", "0"));
                KhoiTaoGiaTriComboBox(ref lstSanPhamTinDungTT, "COMBOBOX_SAN_PHAM_TD", cmbSanPhamTinDungTT, lstDieuKien);
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.NHOM_NO.getValue());
                lstNhomNo.Add(new AutoCompleteEntry(Dislay, "%", "0"));
                KhoiTaoGiaTriComboBox(ref lstNhomNo, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), cmbNhomNo, lstDieuKien);
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.DVI_TINH_KY_HAN));
                lstDonViTinhThoiHan.Add(new AutoCompleteEntry(Dislay, "%", "0"));
                KhoiTaoGiaTriComboBox(ref lstDonViTinhThoiHan, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), cmbThoiHanVayTu, lstDieuKien);
                KhoiTaoGiaTriComboBox(ref lstDonViTinhThoiHan, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), cmbThoiHanVayDen, lstDieuKien);
            }

        void KhoiTaoGiaTriComboBox(ref List<AutoCompleteEntry> lstAutoComplete, string maTruyVan, RadComboBox cmbCommon, List<string> lstDieuKien)
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            // Gen ComboBox bằng việc gọi hàm
            auto.GenAutoComboBox(ref lstAutoComplete, ref cmbCommon, maTruyVan, lstDieuKien);
        }

        void InitEventHanler()
        {
            tvwKhuVuc.SelectionChanged += new SelectionChangedEventHandler(tvwKhuVuc_SelectionChanged);
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
            
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
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
                Add();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                Modify();
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
                View();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
                LoadDuLieu();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
            { }
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
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
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
                txtTimKiemNhanh.RaiseEvent(new RoutedEventArgs(Button.GotFocusEvent));
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
            { }
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
            PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grdKheUocDS, txtTimKiemNhanh.Text);
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
            if (grdKheUocDS != null && grdKheUocDS.ItemsSource != null)
            {
                DataView dt = ((DataView)grdKheUocDS.ItemsSource);
                if (dt != null)
                {
                    radPage.PageSize = (int)nudPageSize.Value;
                    grdKheUocDS.DataContext = dt;
                }
            }
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(grdKheUocDS);
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

        private void grdKheUocDS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            View();
        }

        void ClearForm()
        {
            teldtNgayDaoHanHDDen.Value = null;
            teldtNgayDaoHanHDTu.Value = null;
            teldtNgayLapHDDen.Value = null;
            teldtNgayLapHDTu.Value = null;
            txtSoDuTu.Value = null;
            txtSoDuDen.Value = null;
            txtSoTienVayTu.Value = null;
            txtSoTienVayDen.Value = null;
            txtThoiHanVayTu.Value = null;
            txtThoiHanVayDen.Value = null;
            txtLaiSuatTu.Value = null;
            txtLaiSuatDen.Value = null;
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

        void tvwKhuVuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RadTreeViewItem treeItemRoot = (RadTreeViewItem)tvwKhuVuc.SelectedItem;
            string strLstDonVi = "";
            if (treeItemRoot.Tag.ToString().Substring(0, 3).Equals("DVI"))
            {
                foreach (RadTreeViewItem treeItem in treeItemRoot.Items)
                {
                    strLstDonVi += ",''" + treeItem.Tag.ToString() + "''";
                }

            }
            else if (treeItemRoot.Tag.ToString().Substring(0, 3).Equals("CNH"))
                strLstDonVi = ",''" + treeItemRoot.Tag.ToString() + "''";
            else if (treeItemRoot.Tag.ToString().Substring(0, 3).Equals("PGD"))
            {
                RadTreeViewItem treeItem = (RadTreeViewItem)treeItemRoot.Parent;
                strLstDonVi = ",''" + treeItem.Tag.ToString() + "''";
            }
            else if (treeItemRoot.Tag.ToString().Substring(0, 3).Equals("KVU"))
            {
                RadTreeViewItem treeItem = (RadTreeViewItem)((RadTreeViewItem)treeItemRoot.Parent).Parent;
                strLstDonVi = ",''" + treeItem.Tag.ToString() + "''";
            }
            else if (treeItemRoot.Tag.ToString().Substring(0, 3).Equals("CUM"))
            {
                RadTreeViewItem treeItem = (RadTreeViewItem)treeItemRoot.Parent;
                if (treeItem.Tag.ToString().Contains("KVU"))
                    treeItem = (RadTreeViewItem)((RadTreeViewItem)treeItem.Parent).Parent;
                else
                    treeItem = (RadTreeViewItem)treeItem.Parent;
                strLstDonVi = ",''" + treeItem.Tag.ToString() + "''";
            }
            else if (treeItemRoot.Tag.ToString().Substring(0, 3).Equals("NHM"))
            {
                RadTreeViewItem treeItem = (RadTreeViewItem)((RadTreeViewItem)treeItemRoot.Parent).Parent;
                if (treeItem.Tag.ToString().Contains("KVU"))
                    treeItem = (RadTreeViewItem)((RadTreeViewItem)treeItem.Parent).Parent;
                else
                    treeItem = (RadTreeViewItem)treeItem.Parent;
                strLstDonVi = ",''" + treeItem.Tag.ToString() + "''";
            }
            strLstDonVi = strLstDonVi.Substring(1);
            strLstDonVi = strLstDonVi.Replace("CNH", "");
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(strLstDonVi);
            lstDieuKien.Add("0");
            lstDieuKien.Add("0");
            lstDieuKien.Add("0");
            string Dislay = LLanguage.SearchResourceByKey("U.DungChung.TatCa");
            lstSanPhamTinDungTT.Clear();
            cmbSanPhamTinDungTT.Items.Clear();
            lstSanPhamTinDungTT.Add(new AutoCompleteEntry(Dislay, "%", "0"));
            KhoiTaoGiaTriComboBox(ref lstSanPhamTinDungTT, "COMBOBOX_SAN_PHAM_TD", cmbSanPhamTinDungTT, lstDieuKien);
        }
        #endregion

        #region Xy ly nghiep vu
        void LoadDuLieu()
        {
            try
            {
                string sMaTrangThaiNVu = ucTrangThaiNVu.GetItemsSelected();
                string NgayNhanNoTu = teldtNgayLapHDTu.Value != null ? LDateTime.DateToString(teldtNgayLapHDTu.Value.Value, ApplicationConstant.defaultDateTimeFormat) : "";
                string NgayNhanNoDen = teldtNgayLapHDDen.Value != null ? LDateTime.DateToString(teldtNgayLapHDDen.Value.Value, ApplicationConstant.defaultDateTimeFormat) : "";
                string NgayDaoHanTu = teldtNgayDaoHanHDTu.Value != null ? LDateTime.DateToString(teldtNgayDaoHanHDTu.Value.Value, ApplicationConstant.defaultDateTimeFormat) : "";
                string NgayDaoHanDen = teldtNgayDaoHanHDDen.Value != null ? LDateTime.DateToString(teldtNgayDaoHanHDDen.Value.Value, ApplicationConstant.defaultDateTimeFormat) : "";
                string SoTienGNTu = txtSoTienVayTu.Value != null ? txtSoTienVayTu.Value.ToString() : "";
                string SoTienGNDen = txtSoTienVayDen.Value != null ? txtSoTienVayDen.Value.ToString() : "";
                string SoDuTu = txtSoDuTu.Value != null ? txtSoDuTu.Value.ToString() : "";
                string SoDuDen = txtSoDuDen.Value != null ? txtSoDuDen.Value.ToString() : "";
                string ThoiHanTu = txtThoiHanVayTu.Value != null ? txtThoiHanVayTu.Value.ToString() : "";
                string ThoiHanDen = txtThoiHanVayDen.Value != null ? txtThoiHanVayDen.Value.ToString() : "";
                string ThoiHanDViTu = lstDonViTinhThoiHan.ElementAt(cmbThoiHanVayTu.SelectedIndex).KeywordStrings.First();
                string ThoiHanDViDen = lstDonViTinhThoiHan.ElementAt(cmbThoiHanVayDen.SelectedIndex).KeywordStrings.First();
                string LaiSuatTu = txtLaiSuatTu.Value != null ? txtLaiSuatTu.Value.ToString() : "";
                string LaiSuatDen = txtLaiSuatDen.Value != null ? txtLaiSuatDen.Value.ToString() : "";
                string LoaiGiayTo = "%";
                string MaSanPham = "";
                if (cmbSanPhamTinDungTT.SelectedIndex > -1) MaSanPham = lstSanPhamTinDungTT.ElementAt(cmbSanPhamTinDungTT.SelectedIndex).KeywordStrings.First();
                string SoGiayTo = lstNhomNo.ElementAt(cmbNhomNo.SelectedIndex).KeywordStrings.First();
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

                DataSet ds = new TinDungProcess().getDanhSachKUOCVM(sMaTrangThaiNVu, txtSoHDTD.Text, txtSoKheUoc.Text, NgayNhanNoTu, NgayNhanNoDen, NgayDaoHanTu, NgayDaoHanDen, SoTienGNTu, SoTienGNDen, SoDuTu, SoDuDen, ThoiHanTu, ThoiHanDen, ThoiHanDViTu, ThoiHanDViDen, LaiSuatTu, LaiSuatDen, txtMaKhachHang.Text, txtTenKhachHang.Text, LoaiGiayTo, SoGiayTo, txtDienThoai.Text, txtEmail.Text, MaSanPham, ListKVuc, ClientInformation.TenDangNhap, ClientInformation.MaDonViQuanLy, StartRow.ToString(), EndRow.ToString());

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable serverDataTable = ds.Tables[0];
                    int totalRecord = Int32.Parse(ds.Tables[1].Rows[0][0].ToString());
                    decimal totalSum = Decimal.Parse(ds.Tables[2].Rows[0][0].ToString());
                    DataTable clientDataTable = CommonFunction.BuildClientDataTable(serverDataTable, CurrentPagging, PaggingSize, totalRecord);

                    //grdKheUocDS.ItemsSource = ds.Tables["DANH_SACH"].DefaultView;
                    grdKheUocDS.ItemsSource = clientDataTable.DefaultView;
                    grdKheUocDS.SelectedItems.Clear();
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
                string NgayNhanNoTu = teldtNgayLapHDTu.Value != null ? LDateTime.DateToString(teldtNgayLapHDTu.Value.Value, ApplicationConstant.defaultDateTimeFormat) : "";
                string NgayNhanNoDen = teldtNgayLapHDDen.Value != null ? LDateTime.DateToString(teldtNgayLapHDDen.Value.Value, ApplicationConstant.defaultDateTimeFormat) : "";
                string NgayDaoHanTu = teldtNgayDaoHanHDTu.Value != null ? LDateTime.DateToString(teldtNgayDaoHanHDTu.Value.Value, ApplicationConstant.defaultDateTimeFormat) : "";
                string NgayDaoHanDen = teldtNgayDaoHanHDDen.Value != null ? LDateTime.DateToString(teldtNgayDaoHanHDDen.Value.Value, ApplicationConstant.defaultDateTimeFormat) : "";
                string SoTienGNTu = txtSoTienVayTu.Value != null ? txtSoTienVayTu.Value.ToString() : "";
                string SoTienGNDen = txtSoTienVayDen.Value != null ? txtSoTienVayDen.Value.ToString() : "";
                string SoDuTu = txtSoDuTu.Value != null ? txtSoDuTu.Value.ToString() : "";
                string SoDuDen = txtSoDuDen.Value != null ? txtSoDuDen.Value.ToString() : "";
                string ThoiHanTu = txtThoiHanVayTu.Value != null ? txtThoiHanVayTu.Value.ToString() : "";
                string ThoiHanDen = txtThoiHanVayDen.Value != null ? txtThoiHanVayDen.Value.ToString() : "";
                string ThoiHanDViTu = lstDonViTinhThoiHan.ElementAt(cmbThoiHanVayTu.SelectedIndex).KeywordStrings.First();
                string ThoiHanDViDen = lstDonViTinhThoiHan.ElementAt(cmbThoiHanVayDen.SelectedIndex).KeywordStrings.First();
                string LaiSuatTu = txtLaiSuatTu.Value != null ? txtLaiSuatTu.Value.ToString() : "";
                string LaiSuatDen = txtLaiSuatDen.Value != null ? txtLaiSuatDen.Value.ToString() : "";
                string LoaiGiayTo = "%";
                string MaSanPham = "";
                if (cmbSanPhamTinDungTT.SelectedIndex > -1) MaSanPham = lstSanPhamTinDungTT.ElementAt(cmbSanPhamTinDungTT.SelectedIndex).KeywordStrings.First();
                string SoGiayTo = lstNhomNo.ElementAt(cmbNhomNo.SelectedIndex).KeywordStrings.First();
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

                DataSet ds = new TinDungProcess().getDanhSachKUOCVM(sMaTrangThaiNVu, txtSoHDTD.Text, txtSoKheUoc.Text, NgayNhanNoTu, NgayNhanNoDen, NgayDaoHanTu, NgayDaoHanDen, SoTienGNTu, SoTienGNDen, SoDuTu, SoDuDen, ThoiHanTu, ThoiHanDen, ThoiHanDViTu, ThoiHanDViDen, LaiSuatTu, LaiSuatDen, txtMaKhachHang.Text, txtTenKhachHang.Text, LoaiGiayTo, SoGiayTo, txtDienThoai.Text, txtEmail.Text, MaSanPham, ListKVuc, ClientInformation.TenDangNhap, ClientInformation.MaDonViQuanLy, StartRow.ToString(), EndRow.ToString());

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable serverDataTable = ds.Tables[0];
                    int totalRecord = Int32.Parse(ds.Tables[1].Rows[0][0].ToString());
                    decimal totalSum = Decimal.Parse(ds.Tables[2].Rows[0][0].ToString());
                    DataTable clientDataTable = CommonFunction.BuildClientDataTable(serverDataTable, CurrentPagging, PaggingSize, totalRecord);

                    //grdKheUocDS.ItemsSource = ds.Tables["DANH_SACH"].DefaultView;
                    grdKheUocDS.ItemsSource = clientDataTable.DefaultView;
                    grdKheUocDS.SelectedItems.Clear();
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
        void Add()
            {
                if (!tlbAdd.IsEnabled)
                    return;
                Cursor = Cursors.Wait;
                ucLapKheUocDS_01 objKheUocCT = new ucLapKheUocDS_01();
                objKheUocCT.OnSavingCompleted += new EventHandler(objKheUocCT_OnSavingCompleted);
                string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.Function);
                Window window = new Window();
                window.Title = tittle;
                window.Content = objKheUocCT;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
                Cursor = Cursors.Arrow;
            }
        void Modify()
        {
            if (!tlbModify.IsEnabled)
                return;

            Cursor = Cursors.Wait;
            if (grdKheUocDS.SelectedItems.Count == 1)
            {
                DataRowView dr = (DataRowView)grdKheUocDS.SelectedItems[0];
                ucLapKheUocDS_01 objKheUocCT = new ucLapKheUocDS_01();
                objKheUocCT.action = DatabaseConstant.Action.SUA;
                objKheUocCT.objKUOCVMDS = new Presentation.Process.TinDungServiceRef.TDVM_KHE_UOC_DSACH();
                objKheUocCT.objKUOCVMDS.MA_GDICH = dr["MA_GDICH"].ToString();
                objKheUocCT.OnSavingCompleted += new EventHandler(objKheUocCT_OnSavingCompleted);
                objKheUocCT.SetDataForm();
                string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.Function);
                Window window = new Window();
                window.Title = tittle;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Content = objKheUocCT;
                window.ShowDialog();
            }
            else if (grdKheUocDS.SelectedItems.Count > 1)
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
            Cursor = Cursors.Arrow;
        }

        void View()
        {
            if (!tlbView.IsEnabled)
                return;

            Cursor = Cursors.Wait;
            if (grdKheUocDS.SelectedItems.Count == 1)
            {
                DataRowView dr = (DataRowView)grdKheUocDS.SelectedItems[0];
                ucLapKheUocDS_01 objKheUocCT = new ucLapKheUocDS_01();
                objKheUocCT.action = DatabaseConstant.Action.XEM;
                objKheUocCT.objKUOCVMDS = new Presentation.Process.TinDungServiceRef.TDVM_KHE_UOC_DSACH();
                objKheUocCT.objKUOCVMDS.MA_GDICH = dr["MA_GDICH"].ToString();
                objKheUocCT.OnSavingCompleted += new EventHandler(objKheUocCT_OnSavingCompleted);
                objKheUocCT.SetDataForm();
                string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.Function);
                Window window = new Window();
                window.Title = tittle;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Content = objKheUocCT;
                window.ShowDialog();
            }
            else if (grdKheUocDS.SelectedItems.Count > 1)
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
            Cursor = Cursors.Arrow;
        }
        void objKheUocCT_OnSavingCompleted(object sender, EventArgs e)
        {
            LoadDuLieu();
        }

        void BeforeDelete()
        {
            try
            {
                if (!tlbDelete.IsEnabled)
                    return;
                Cursor = Cursors.Wait;
                lstId = new List<int>();
                if (grdKheUocDS.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        List<TDVM_KHE_UOC> lstKUOCVM = new List<TDVM_KHE_UOC>();
                        foreach (DataRowView dr in grdKheUocDS.SelectedItems)
                        {
                            lstId.Add(Convert.ToInt32(dr["ID"]));
                            TDVM_KHE_UOC objKUOCVM = new TDVM_KHE_UOC();
                            objKUOCVM.KUOC_VM = new TD_KUOCVM();
                            objKUOCVM.KUOC_VM.ID = Convert.ToInt32(dr["ID"]);
                            objKUOCVM.KUOC_VM.MA_GDICH = dr["MA_GDICH"].ToString();
                            objKUOCVM.KUOC_VM.MA_KUOCVM = dr["MA_KUOCVM"].ToString();
                            lstKUOCVM.Add(objKUOCVM);
                        }
                        _KUOCVM.DSACH_KHE_UOC = lstKUOCVM.ToArray();
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01,
                        DatabaseConstant.Table.TD_KUOCVM,
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
                DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.XOA,
                lstId);
                Cursor = Cursors.Arrow;
            }

        }
        void OnDelete()
        {
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            iret = new TinDungProcess().LapKheUocDanhSach_01(DatabaseConstant.Action.XOA, ref _KUOCVM, ref lstResponseDetail);
            AfterDelete(lstResponseDetail, iret);
        }
        void AfterDelete(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.XOA,
            lstId);
            LoadDuLieu();
            Cursor = Cursors.Arrow;
        }

        void BeforeApprove()
        {
            try
            {
                if (!tlbApprove.IsEnabled)
                    return;
                Cursor = Cursors.Wait;
                lstId = new List<int>();
                if (grdKheUocDS.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        List<TDVM_KHE_UOC> lstKUOCVM = new List<TDVM_KHE_UOC>();
                        foreach (DataRowView dr in grdKheUocDS.SelectedItems)
                        {
                            lstId.Add(Convert.ToInt32(dr["ID"]));
                            TDVM_KHE_UOC objKUOCVM = new TDVM_KHE_UOC();
                            objKUOCVM.KUOC_VM = new TD_KUOCVM();
                            objKUOCVM.KUOC_VM.ID = Convert.ToInt32(dr["ID"]);
                            objKUOCVM.KUOC_VM.MA_GDICH = dr["MA_GDICH"].ToString();
                            objKUOCVM.KUOC_VM.MA_KUOCVM = dr["MA_KUOCVM"].ToString();
                            lstKUOCVM.Add(objKUOCVM);
                        }
                        _KUOCVM.DSACH_KHE_UOC = lstKUOCVM.ToArray();
                        // Yêu cầu Lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01,
                        DatabaseConstant.Table.TD_KUOCVM,
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
                DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.DUYET,
                lstId);
                Cursor = Cursors.Arrow;
            }

        }
        void OnApprove()
        {
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            iret = new TinDungProcess().LapKheUocDanhSach_01(DatabaseConstant.Action.DUYET,ref _KUOCVM, ref lstResponseDetail);
            AfterApprove(lstResponseDetail, iret);
        }
        void AfterApprove(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.DUYET,
            lstId);
            LoadDuLieu();
            Cursor = Cursors.Arrow;
        }

        void BeforeRefuse()
        {
            try
            {
                if (!tlbRefuse.IsEnabled)
                    return;
                Cursor = Cursors.Wait;
                lstId = new List<int>();
                if (grdKheUocDS.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        List<TDVM_KHE_UOC> lstKUOCVM = new List<TDVM_KHE_UOC>();
                        foreach (DataRowView dr in grdKheUocDS.SelectedItems)
                        {
                            lstId.Add(Convert.ToInt32(dr["ID"]));
                            TDVM_KHE_UOC objKUOCVM = new TDVM_KHE_UOC();
                            objKUOCVM.KUOC_VM = new TD_KUOCVM();
                            objKUOCVM.KUOC_VM.ID = Convert.ToInt32(dr["ID"]);
                            objKUOCVM.KUOC_VM.MA_GDICH = dr["MA_GDICH"].ToString();
                            objKUOCVM.KUOC_VM.MA_KUOCVM = dr["MA_KUOCVM"].ToString();
                            lstKUOCVM.Add(objKUOCVM);
                        }
                        _KUOCVM.DSACH_KHE_UOC = lstKUOCVM.ToArray();
                        // Yêu cầu Lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01,
                        DatabaseConstant.Table.TD_KUOCVM,
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
                DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);
                Cursor = Cursors.Arrow;
            }

        }
        void OnRefuse()
        {
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            iret = new TinDungProcess().LapKheUocDanhSach_01(DatabaseConstant.Action.TU_CHOI_DUYET,ref _KUOCVM, ref lstResponseDetail);
            AfterRefuse(lstResponseDetail, iret);
        }
        void AfterRefuse(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.TU_CHOI_DUYET,
            lstId);
            LoadDuLieu();
            Cursor = Cursors.Arrow;
        }

        void BeforeCancel()
        {
            try
            {
                if (!tlbCancel.IsEnabled)
                    return;
                Cursor = Cursors.Wait;
                lstId = new List<int>();
                if (grdKheUocDS.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        List<TDVM_KHE_UOC> lstKUOCVM = new List<TDVM_KHE_UOC>();
                        foreach (DataRowView dr in grdKheUocDS.SelectedItems)
                        {
                            lstId.Add(Convert.ToInt32(dr["ID"]));
                            TDVM_KHE_UOC objKUOCVM = new TDVM_KHE_UOC();
                            objKUOCVM.KUOC_VM = new TD_KUOCVM();
                            objKUOCVM.KUOC_VM.ID = Convert.ToInt32(dr["ID"]);
                            objKUOCVM.KUOC_VM.MA_GDICH = dr["MA_GDICH"].ToString();
                            objKUOCVM.KUOC_VM.MA_KUOCVM = dr["MA_KUOCVM"].ToString();
                            lstKUOCVM.Add(objKUOCVM);
                        }
                        _KUOCVM.DSACH_KHE_UOC = lstKUOCVM.ToArray();
                        // Yêu cầu Lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01,
                        DatabaseConstant.Table.TD_KUOCVM,
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
                DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                Cursor = Cursors.Arrow;
            }

        }
        void OnCancel()
        {
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            iret = new TinDungProcess().LapKheUocDanhSach_01(DatabaseConstant.Action.THOAI_DUYET,ref _KUOCVM, ref lstResponseDetail);
            AfterCancel(lstResponseDetail, iret);
        }
        void AfterCancel(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.THOAI_DUYET,
            lstId);
            LoadDuLieu();
            Cursor = Cursors.Arrow;
        }
        #endregion

        
    }
}
