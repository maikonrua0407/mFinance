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


namespace PresentationWPF.TinDung.VongVay
{
    /// <summary>
    /// Interaction logic for ucVongVayDS.xaml
    /// </summary>
    public partial class ucVongVayDS : UserControl
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
        List<AutoCompleteEntry> lstHanMucGocVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstHanMucKyHan = new List<AutoCompleteEntry>();
        // Danh sách các item được chọn trong treeview
        private List<string> lstCheckedKhuVuc = new List<string>();
        private List<string> lstCheckedSanPham = new List<string>();
        delegate void LoadDuLieuCT();
        private DatabaseConstant.Module Module = DatabaseConstant.Module.TDVM;
        private DatabaseConstant.Function Function = DatabaseConstant.Function.TDVM_VONG_VAY;
        DataTable dtTreeDLy;
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucVongVayDS()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDung;component/VongVay/ucVongVayDS.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindHotkey();
            string Dislay = LLanguage.SearchResourceByKey("U.DungChung.TatCa");
            radPage.PageSize = (int)nudPageSize.Value;
            lstHanMucGocVay.Add(new AutoCompleteEntry(Dislay, "%", "%"));
            lstHanMucKyHan.Add(new AutoCompleteEntry(Dislay, "%", "%"));
            List<string> lstDieuKien = new List<string>();
            // Gán giá trị điều kiện
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.TINH_CHAT_VONG_VAY));
            KhoiTaoGiaTriComboBox(ref lstHanMucGocVay, ref cmbTinhChatGocVay, lstDieuKien);
            KhoiTaoGiaTriComboBox(ref lstHanMucKyHan, ref cmbTinhChatKyHan, lstDieuKien);
            teldtNgayLapDen.Value = null;
            teldtNgayLapTu.Value = null;
            BuildTreeKhuVuc();
            LoadDuLieu();
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
                if (row["LEVEL"].Equals(0))
                    BuildSubTreeKhuVuc(subItem, row, Convert.ToInt32(row["LEVEL"]) + 1);
            }
        }

        /// <summary>
        /// Khởi tạo các ComboBox
        /// </summary>
        /// <param name="lstAutoComplete"></param>
        /// <param name="cmbCommon"></param>
        /// <param name="lstDieuKien"></param>
        void KhoiTaoGiaTriComboBox(ref List<AutoCompleteEntry> lstAutoComplete, ref Telerik.Windows.Controls.RadComboBox cmbCommon, List<string> lstDieuKien)
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            // Gen ComboBox bằng việc gọi hàm
            auto.GenAutoComboBox(ref lstAutoComplete, ref cmbCommon, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
            if (cmbCommon.Items.Count > 0)
                cmbCommon.SelectedIndex = 0;
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
            LoadDuLieu();
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
                LoadDuLieu();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
                LoadDuLieu();
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
            PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(dgrVongVayDS, txtTimKiemNhanh.Text);
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
            if (dgrVongVayDS != null && dgrVongVayDS.ItemsSource != null)
            {
                DataView dt = (DataView)dgrVongVayDS.ItemsSource;
                if (dt != null)
                {
                    radPage.PageSize = (int)nudPageSize.Value;
                    dgrVongVayDS.ItemsSource = dt;
                }
            }
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(dgrVongVayDS);
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
                    throw new System.NullReferenceException(LLanguage.SearchResourceByKey("M.TinDung.VongVay.ucVongVayDS.KhongTimThayControl") + sbControl.ToString());
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Load dữ liệu
        /// </summary>
        private void LoadDuLieu() 
        {
            Cursor = Cursors.Wait;
            try
            {
                TinDungProcess tindungProcess = new TinDungProcess();
                string lstTrangThai = ucTrangThai.GetItemsSelected();
                string sMaVongVay = txtMaVongVay.Text;
                string sNgayLapTu = teldtNgayLapTu.Value != null ? LDateTime.DateToString((DateTime)teldtNgayLapTu.Value, ApplicationConstant.defaultDateTimeFormat) : "";
                string sNgayLapDen = teldtNgayLapDen.Value != null ? LDateTime.DateToString((DateTime)teldtNgayLapDen.Value, ApplicationConstant.defaultDateTimeFormat) : "";
                string sHanMucGocVay = lstHanMucGocVay.ElementAt(cmbTinhChatGocVay.SelectedIndex).KeywordStrings.First();
                string sHanMucKyHan = lstHanMucKyHan.ElementAt(cmbTinhChatKyHan.SelectedIndex).KeywordStrings.First();
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
                else if (((RadTreeViewItem)tvwKhuVuc.SelectedItem).Tag.ToString().Substring(0, 3).Equals("CNH"))
                    ListKVuc = ((RadTreeViewItem)tvwKhuVuc.SelectedItem).Tag.ToString();
                else if (((RadTreeViewItem)tvwKhuVuc.SelectedItem).Tag.ToString().Substring(0, 3).Equals("PGD"))                
                    ListKVuc = ((RadTreeViewItem)((RadTreeViewItem)tvwKhuVuc.SelectedItem).Parent).Tag.ToString();
                else if (((RadTreeViewItem)tvwKhuVuc.SelectedItem).Tag.ToString().Substring(0, 3).Equals("CUM"))
                    ListKVuc = ((RadTreeViewItem)((RadTreeViewItem)((RadTreeViewItem)tvwKhuVuc.SelectedItem).Parent).Parent).Tag.ToString();
                DataSet ds = tindungProcess.getDanhSachVongVonVay(lstTrangThai, sMaVongVay, sNgayLapTu, sNgayLapDen, sHanMucGocVay, sHanMucKyHan, "%", ListKVuc);
                if (ds != null & ds.Tables[0].Columns.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        dr["TEN_TTHAI_NVU"] = BusinessConstant.layNgonNguNghiepVu(dr["TTHAI_NVU"].ToString());

                    dgrVongVayDS.ItemsSource = null;
                    dgrVongVayDS.ItemsSource = ds.Tables[0].DefaultView;
                    if (!LObject.IsNullOrEmpty(dgrVongVayDS.SelectedItems))
                        dgrVongVayDS.SelectedItems.Clear();
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DanhMuc.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            Cursor = Cursors.Arrow;
        }

        private void Them()
        {
            if (tlbAdd.IsEnabled == false)
                return;
            Cursor = Cursors.Wait;
            ucVongVayCT usercontrol = new ucVongVayCT();
            usercontrol.OnSavingComleted += new EventHandler(usercontrol_OnSavingComleted);
            Window window = new Window();
            string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.Function);
            window.Title = tittle;
            window.Content = usercontrol;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
            Cursor = Cursors.Arrow;
        }

        void usercontrol_OnSavingComleted(object sender, EventArgs e)
        {
            LoadDuLieu();
        }

        private void Sua()
        {
            Cursor = Cursors.Wait;
            if (dgrVongVayDS.SelectedItems.Count == 1)
            {
                DataRow dr = ((DataRowView)dgrVongVayDS.SelectedItems[0]).Row;
                ucVongVayCT usercontrol = new ucVongVayCT();
                usercontrol.OnSavingComleted += new EventHandler(usercontrol_OnSavingComleted);
                usercontrol.IdVongVay = Convert.ToInt32(dr["ID"]);
                usercontrol.BSuaDuLieu = true;
                LoadDuLieuCT dlgLoadDuLieuCT = new LoadDuLieuCT(usercontrol.LoadDuLieu);
                dlgLoadDuLieuCT();
                Window window = new Window();
                string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.Function);
                window.Title = tittle;
                window.Content = usercontrol;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
            }
            else
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
            Cursor = Cursors.Arrow;
        }

        private void Xoa()
        {
            if (tlbDelete.IsEnabled == false)
                return;
            Cursor = Cursors.Wait;
            try
            {
                if (dgrVongVayDS.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        List<int> lstID = new List<int>();
                        foreach (DataRowView dr in dgrVongVayDS.SelectedItems)
                        {
                            lstID.Add(Convert.ToInt32(dr["ID"]));
                        }
                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_VONG_VAY,
                        DatabaseConstant.Table.TD_VONG_VAY,
                        DatabaseConstant.Action.XOA,
                        lstID);
                        List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
                        bool bResult = new TinDungProcess().XoaVongVayTinDung(lstID.ToArray(), ref ResponseDetail);
                        if (bResult)
                            LoadDuLieu();
                        CommonFunction.ThongBaoKetQua(ResponseDetail);
                        // Yêu cầu Unlock dữ liệu
                        retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_VONG_VAY,
                        DatabaseConstant.Table.TD_VONG_VAY,
                        DatabaseConstant.Action.XOA,
                        lstID);
                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DanhMuc.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// Xem chi tiết
        /// </summary>
        private void Xem()
        {
            if (tlbView.IsEnabled == false)
                return;
            Cursor = Cursors.Wait;
            if (dgrVongVayDS.SelectedItems.Count == 1)
            {
                DataRow dr = ((DataRowView)dgrVongVayDS.SelectedItems[0]).Row;
                ucVongVayCT usercontrol = new ucVongVayCT();
                usercontrol.OnSavingComleted += new EventHandler(usercontrol_OnSavingComleted);
                usercontrol.IdVongVay = Convert.ToInt32(dr["ID"]);
                usercontrol.BSuaDuLieu = false;
                LoadDuLieuCT dlgLoadDuLieuCT = new LoadDuLieuCT(usercontrol.LoadDuLieu);
                dlgLoadDuLieuCT();
                Window window = new Window();
                string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.Function);
                window.Title = tittle;
                window.Content = usercontrol;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
            }
            else
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
            Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// Duyệt chi tiết
        /// </summary>
        private void Duyet()
        {
            if (tlbApprove.IsEnabled == false)
                return;
            Cursor = Cursors.Wait;
            try
            {
                if (dgrVongVayDS.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        List<int> lstID = new List<int>();
                        // Yêu cầu lock dữ liệu
                        foreach (DataRowView dr in dgrVongVayDS.SelectedItems)
                        {
                                lstID.Add(Convert.ToInt32(dr["ID"]));
                        }
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_VONG_VAY,
                        DatabaseConstant.Table.TD_VONG_VAY,
                        DatabaseConstant.Action.DUYET,
                        lstID);
                        List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
                        bool bResult = new TinDungProcess().DuyetVongVayTinDung(lstID.ToArray(), ref ResponseDetail);
                        if (bResult)
                            LoadDuLieu();
                        CommonFunction.ThongBaoKetQua(ResponseDetail);
                        // Yêu cầu unlock dữ liệu
                        retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_VONG_VAY,
                        DatabaseConstant.Table.TD_VONG_VAY,
                        DatabaseConstant.Action.DUYET,
                        lstID);
                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DanhMuc.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// Thoái duyệt
        /// </summary>
        private void ThoaiDuyet()
        {
            if (tlbCancel.IsEnabled == false)
                return;
            Cursor = Cursors.Wait;
            try
            {
                if (dgrVongVayDS.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        List<int> lstID = new List<int>();
                        foreach (DataRowView dr in dgrVongVayDS.SelectedItems)
                        {
                                lstID.Add(Convert.ToInt32(dr["ID"]));
                        }
                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_VONG_VAY,
                        DatabaseConstant.Table.TD_VONG_VAY,
                        DatabaseConstant.Action.THOAI_DUYET,
                        lstID);
                        List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
                        bool bResult = new TinDungProcess().HuyDuyetVongVayTinDung(lstID.ToArray(), ref ResponseDetail);
                        if (bResult)
                            LoadDuLieu();
                        CommonFunction.ThongBaoKetQua(ResponseDetail);
                        // Yêu cầu Unlock dữ liệu
                        retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_VONG_VAY,
                        DatabaseConstant.Table.TD_VONG_VAY,
                        DatabaseConstant.Action.THOAI_DUYET,
                        lstID);
                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DanhMuc.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// Từ chối duyệt
        /// </summary>
        private void TuChoiDuyet()
        {
            if (tlbRefuse.IsEnabled == false)
                return;
            Cursor = Cursors.Wait;
            try
            {
                if (dgrVongVayDS.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {

                        List<int> lstID = new List<int>();
                        foreach (DataRowView dr in dgrVongVayDS.SelectedItems)
                        {
                                lstID.Add(Convert.ToInt32(dr["ID"]));
                        }
                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_VONG_VAY,
                        DatabaseConstant.Table.TD_VONG_VAY,
                        DatabaseConstant.Action.TU_CHOI_DUYET,
                        lstID);
                        List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
                        bool bResult = new TinDungProcess().TuChoiVongVayTinDung(lstID.ToArray(), ref ResponseDetail);
                        if (bResult)
                            LoadDuLieu();
                        CommonFunction.ThongBaoKetQua(ResponseDetail);
                        // Yêu cầu lock dữ liệu
                        retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                       DatabaseConstant.Function.TDVM_VONG_VAY,
                       DatabaseConstant.Table.TD_VONG_VAY,
                       DatabaseConstant.Action.TU_CHOI_DUYET,
                       lstID);
                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DanhMuc.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            Cursor = Cursors.Arrow;
        }

        private void dgrVongVayDS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Xem();
        }
        #endregion


    }
}
