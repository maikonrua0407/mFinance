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
using Presentation.Process.Common;
using Utilities.Common;
using PresentationWPF.CustomControl;
using Telerik.Windows.Controls;
using Presentation.Process.DanhMucServiceRef;


namespace PresentationWPF.KeToan.DoiTuong
{
    /// <summary>
    /// Interaction logic for ucNNSDDS.xaml
    /// </summary>
    public partial class ucDoiTuongDS : UserControl
    {
        #region Khai bao

        bool isLoaded = false;
        List<AutoCompleteEntry> lstSourceLoaiDTuong = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();

        List<DM_DTUONG> dsDTUONG = new List<DM_DTUONG>();
        List<DM_LOAI_DTUONG> dsLOAIDTUONG = new List<DM_LOAI_DTUONG>();
        DataTable dt = new DataTable();
        DanhMucProcess DanhMucProcess = new DanhMucProcess();

        private string loaiDoiTuong = "DOI_TUONG";
        private string maDonVi = "%";

        public static RoutedCommand AddCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ResetpassCommand = new RoutedCommand();
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
        public ucDoiTuongDS()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KeToan;component/DoiTuong/ucDoiTuongDS.xaml", ref Toolbar, ref mnuGrid);
            updateContextMenu();
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                {
                    ((MenuItem)item).Click += btnShortcutKey_Click;
                }
            }
            BindHotkey();
            radPage.PageSize = (int)nudPageSize.Value;
            //loaiDoiTuong = "DOI_TUONG";
            loaiDoiTuong = "LOAI_DOI_TUONG";
            //tlbResetpass.IsEnabled = false;
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.RESET_PASS)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control);
                        key = new KeyBinding(ResetpassCommand, keyg);
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
            e.CanExecute = true;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //Luu(DatabaseConstant.Action.THEM);
            beforeAddNew();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //Luu(DatabaseConstant.Action.SUA);
            beforeModify();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //LuuTrangThai(DatabaseConstant.Action.XOA);
            beforeDelete();
        }

        //private void ResetpassCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        //{
        //    e.CanExecute = true;
        //}
        //private void ResetpassCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        //{
        //    //LuuTrangThai(DatabaseConstant.Action.XOA);
        //    beforeResetpass();
        //}

        private void ViewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ViewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //Luu(DatabaseConstant.Action.XEM);
            beforeView();
        }

        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //txtTimKiemNhanh.RaiseEvent(new RoutedEventArgs(Button.GotFocusEvent));
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
            //CustomControl.CommonFunction.CloseUserControl(this);
            onClose();
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
                //Luu(DatabaseConstant.Action.THEM);
                beforeAddNew();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                //Luu(DatabaseConstant.Action.SUA);
                beforeModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                //LuuTrangThai(DatabaseConstant.Action.XOA);
                beforeDelete();
            }
            //else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.RESET_PASS)))
            //{
            //    //LuuTrangThai(DatabaseConstant.Action.XOA);
            //    beforeResetpass();
            //}
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
                //Luu(DatabaseConstant.Action.XEM);
                beforeView();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            {
                //txtTimKiemNhanh.RaiseEvent(new RoutedEventArgs(Button.GotFocusEvent));
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
                //CustomControl.CommonFunction.CloseUserControl(this);
                onClose();
            }
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
            if (!isLoaded)
            {
                // Khởi tạo các sự kiện cho control
                //cmbDoiTuong.SelectionChanged += cmbDoiTuong_SelectionChanged;
                tvwTree.SelectionChanged += tvwTree_SelectionChanged;
                grDanhSach.MouseDoubleClick += grDanhSach_MouseDoubleClick;
                txtTimKiemNhanh.GotFocus += txtTimKiemNhanh_GotFocus;
                txtTimKiemNhanh.LostFocus += txtTimKiemNhanh_LostFocus;
                txtTimKiemNhanh.KeyDown += txtTimKiemNhanh_KeyDown;
                txtTimKiemNhanh.Focus();
                isLoaded = true;
            }
        }

        private void LoadCombobox()
        {
            try
            {
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();

                //lstDieuKien.Add("'" + DatabaseConstant.ToChucDonVi.HSO.getValue() + "', '" + DatabaseConstant.ToChucDonVi.CNH.getValue() + "'");
                //auto.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOLOAI.getValue(), lstDieuKien, ClientInformation.MaDonVi);
                //if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()) || ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()))
                //    cmbDonVi.IsEnabled = false;        

                lstDieuKien.Add(ClientInformation.TenDangNhap);
                lstDieuKien.Add(ClientInformation.MaDonVi);
                lstDieuKien.Add(DatabaseConstant.ToChucDonVi.CNH.getValue());
                auto.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVI_THEOPVI.getValue(), lstDieuKien, ClientInformation.MaDonVi);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        
        /// <summary>
        /// Sự kiện tìm kiếm nhanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTimKiemNhanh_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtTimKiemNhanh.Text == LLanguage.SearchResourceByKey("U.DungChung.TimKiemNhanh"))
            {
                return;
            }
            PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grDanhSach, txtTimKiemNhanh.Text);
            loadWidthColumnDoiTuong();
            
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

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                //Luu(DatabaseConstant.Action.THEM);
                beforeAddNew();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                //Luu(DatabaseConstant.Action.SUA);
                beforeModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                //LuuTrangThai(DatabaseConstant.Action.XOA);
                beforeDelete();
            }
            //else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.RESET_PASS)))
            //{
            //    //LuuTrangThai(DatabaseConstant.Action.XOA);
            //    beforeResetpass();
            //}
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                //LuuTrangThai(DatabaseConstant.Action.DUYET);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                //LuuTrangThai(DatabaseConstant.Action.TU_CHOI_DUYET);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                //LuuTrangThai(DatabaseConstant.Action.THOAI_DUYET);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
                //Luu(DatabaseConstant.Action.XEM);
                beforeView();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            {
                //txtTimKiemNhanh.RaiseEvent(new RoutedEventArgs(Button.GotFocusEvent));
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
                //CustomControl.CommonFunction.CloseUserControl(this);
                onClose();
            }
        }

        private void updateContextMenu()
        {
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                {
                    MenuItem menuItem = (MenuItem)item;
                    string name = menuItem.Name.Substring(3, menuItem.Name.Length - 3);
                    if (name.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.RESET_PASS)))
                    {
                        if (loaiDoiTuong.Equals(BusinessConstant.DM_Loai_DTuong.DOI_TUONG.layGiaTri()) || loaiDoiTuong == "")
                        {
                            menuItem.IsEnabled = true;
                        }
                        else if (loaiDoiTuong.Equals(BusinessConstant.DM_Loai_DTuong.LOAI_DOI_TUONG.layGiaTri()))
                        {
                            menuItem.IsEnabled = false;
                        }
                    }
                }
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
                if (dt.Rows.Count > 0)
                    grDanhSach.ItemsSource = dt.DefaultView;
                lblSumNNSD.Content = dt.Rows.Count;
                loadWidthColumnDoiTuong();
            }
        }

        private void cmbDonVi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                maDonVi = lstSourceDonVi.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.ElementAt(0);
                //if(maDonVi.Equals(DatabaseConstant.MA_HSO))   maDonVi = "%";
                BuildGridDoiTuong();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }

        }

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
        /// Lấy lại dữ liệu
        /// </summary>
        private void LayLai()
        {
            LoadDuLieu();
        }

        /// <summary>
        /// Load dữ liệu lên datagrid
        /// </summary>
        private void LoadDuLieu()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                LoadCombobox();

                AutoComboBox auto = new AutoComboBox();
                lstSourceLoaiDTuong = new List<AutoCompleteEntry>();
                // lấy dữ liệu đổ source cho combobox Loại đối tượng
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add("LOAI_DTUONG_DOI_TUONG");
                Telerik.Windows.Controls.RadComboBox cmb = new Telerik.Windows.Controls.RadComboBox();
                string tmp = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                auto.GenAutoComboBox(ref lstSourceLoaiDTuong, ref cmb, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

                while (tvwTree.Items.Count > 0)
                    tvwTree.Items.RemoveAt(0);
                UpdateLayout();
                RadTreeViewItem rootItem = new RadTreeViewItem();
                rootItem.Header = "Loại đối tượng và đối tượng";
                rootItem.Tag = "";
                rootItem.IsExpanded = true;
                tvwTree.Items.Add(rootItem);
                foreach (AutoCompleteEntry item in lstSourceLoaiDTuong)
                {
                    RadTreeViewItem node = new RadTreeViewItem();
                    node.Header = item.DisplayName;
                    node.Tag = item.KeywordStrings.First();
                    //if (lstSourceLoaiDTuong.IndexOf(item) == 0)
                    if (lstSourceLoaiDTuong.IndexOf(item) == 1)
                        node.IsSelected = true;
                    node.IsExpanded = true;
                    rootItem.Items.Add(node);
                }
                BuildGridDoiTuong();
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void BuildGridDoiTuong()
        {
            this.Cursor = Cursors.Wait;
            try
            {
                dsDTUONG = new List<DM_DTUONG>();
                dsLOAIDTUONG = new List<DM_LOAI_DTUONG>();
                //List<DM_DON_VI> listDonVi = new List<DM_DON_VI>();

                // lấy loại đối tượng từ combo
                // string loaiDoiTuong = lstSourceLoaiDTuong.ElementAt(cmbDoiTuong.SelectedIndex).KeywordStrings.First();
                // string loaiDoiTuong = "";

                if (tvwTree.SelectedItem != null)
                    if (((RadTreeViewItem)tvwTree.SelectedItem).Tag != null)
                        loaiDoiTuong = ((RadTreeViewItem)tvwTree.SelectedItem).Tag.ToString();

                //string maDonVi = ClientInformation.MaDonVi;
                // lấy dữ liệu sử dụng chung : DM_DTUONG, DM_LOAI_DTUONG
                //string tmp = "DOI_TUONG";
                //string tmp1 = "LOAI_DOI_TUONG";
                if (loaiDoiTuong.Equals(BusinessConstant.DM_Loai_DTuong.DOI_TUONG.layGiaTri()) || loaiDoiTuong == "")
                {

                    dsDTUONG = DanhMucProcess.getDanhSachDoiTuong(maDonVi);
                    //tlbResetpass.IsEnabled = true;
                }
                else if (loaiDoiTuong.Equals(BusinessConstant.DM_Loai_DTuong.LOAI_DOI_TUONG.layGiaTri()))
                {
                    dsLOAIDTUONG = DanhMucProcess.getDanhSachDoiTuongLoai(maDonVi);
                    //tlbResetpass.IsEnabled = false;
                }
                updateContextMenu();

                //// Nếu người dùng là CAP_SA hoặc CAP_QTTW thì lấy danh sách đơn vị
                //if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_SA.layGiaTri()) ||
                //    ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTTW.layGiaTri()))
                //{
                //    listDonVi = DanhMucProcess.layDanhSachDonVi();
                //}
                //// Còn lại lấy đơn vị của người dùng đang đăng nhập
                //else
                //{
                //    DM_DON_VI dv = new DM_DON_VI();
                //    dv.MA_DVI = ClientInformation.MaDonVi;
                //    dv.TEN_GDICH = ClientInformation.TenDonVi;

                //    listDonVi.Add(dv);
                //}

                // Tạo source thông tin đối tượng
                dt = new DataTable();
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add(LLanguage.SearchResourceByKey("U.DungChung.STT"), typeof(int));
                dt.Columns.Add(LLanguage.SearchResourceByKey("U.KeToan.DoiTuong.ucDoiTuongDS.Ma"), typeof(string));
                dt.Columns.Add(LLanguage.SearchResourceByKey("U.KeToan.DoiTuong.ucDoiTuongDS.Ten"), typeof(string));
                dt.Columns.Add(LLanguage.SearchResourceByKey("U.KeToan.DoiTuong.ucDoiTuongDS.MoTa"), typeof(string));
                if (loaiDoiTuong.Equals(BusinessConstant.DM_Loai_DTuong.DOI_TUONG.layGiaTri()) || loaiDoiTuong == "")
                {
                    dt.Columns.Add("Mã loại đối tượng", typeof(string));
                    dt.Columns.Add("Mã loại tham chiếu", typeof(string));
                }

                int stt = 0;
                // Lấy dữ liệu đổ vào source với loại đối tượng tương ứng
                if (loaiDoiTuong.Equals(BusinessConstant.DM_Loai_DTuong.DOI_TUONG.layGiaTri()) || loaiDoiTuong == "")
                {
                    foreach (var item in dsDTUONG)
                    {
                        DataRow r = dt.NewRow();
                        stt = stt + 1;
                        r[0] = item.ID;
                        r[1] = stt;
                        r[2] = item.MA_DTUONG;
                        r[3] = item.TEN_DTUONG;
                        r[4] = item.MO_TA;

                        r[5] = item.MA_LOAI_DTUONG;
                        r[6] = item.MA_LOAI_TCHIEU;
                        dt.Rows.Add(r);
                    }
                }
                else if (loaiDoiTuong.Equals(BusinessConstant.DM_Loai_DTuong.LOAI_DOI_TUONG.layGiaTri()))
                {
                    foreach (var item in dsLOAIDTUONG)
                    {
                        DataRow r = dt.NewRow();
                        stt = stt + 1;
                        r[0] = item.ID;
                        r[1] = stt;
                        r[2] = item.MA_LOAI_DTUONG;
                        r[3] = item.TEN_LOAI_DTUONG;
                        r[4] = item.MO_TA;
                        //r[4] = layTenDonViTheoDanhSach(item.MA_DVI_QLY, listDonVi);
                        //r[5] = item.MO_TA;
                        //r[6] = BusinessConstant.layNgonNguSuDung(item.TTHAI_BGHI);
                        dt.Rows.Add(r);
                    }
                }
                // đổ source lên lưới
                grDanhSach.ItemsSource = dt.DefaultView;
                lblSumNNSD.Content = dt.Rows.Count;
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void loadWidthColumnDoiTuong()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                grDanhSach.SelectedItems.Clear();
                for (int i = 0; i < grDanhSach.Columns.Count; i++)
                {
                    if (i == 1)
                        grDanhSach.Columns[i].IsVisible = false;
                    else if (i == 2)
                        grDanhSach.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(50, Telerik.Windows.Controls.GridViewLengthUnitType.Pixel);
                    else if (i == 3)
                        grDanhSach.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Star);
                    else if (i == 4)
                        grDanhSach.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(2, Telerik.Windows.Controls.GridViewLengthUnitType.Star);
                    else if (i == 5)
                        grDanhSach.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(2, Telerik.Windows.Controls.GridViewLengthUnitType.Star);
                    else if (i == 6)
                        grDanhSach.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(2, Telerik.Windows.Controls.GridViewLengthUnitType.Star);
                    else if (i == 7)
                        grDanhSach.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Star);
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }



        private void cmbDoiTuong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BuildGridDoiTuong();
            loadWidthColumnDoiTuong();
        }

        private void grDanhSach_Loaded(object sender, RoutedEventArgs e)
        {
            loadWidthColumnDoiTuong();
        }

        private void grDanhSach_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Luu(DatabaseConstant.Action.XEM);
            beforeView();
        }


        private void LuuTrangThai(DatabaseConstant.Action action)
        {
            DanhMucProcess process = new DanhMucProcess();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<int> lstID = new List<int>();
                foreach (var row in grDanhSach.SelectedItems)
                {
                    if (row is DataRow)
                    {
                        if ((int)((DataRow)row)[0] > 0)
                            lstID.Add((int)((DataRow)row)[0]);
                    }
                    else
                    {
                        if ((int)((DataRow)row)[0] > 0)
                            lstID.Add((int)((DataRow)row)[0]);
                    }
                }
                string loaiDoiTuong = ((RadTreeViewItem)tvwTree.SelectedItem).Tag.ToString();
                //if (loaiDoiTuong.Equals("DOI_TUONG"))
                //process.capNhatNSD(action, null, lstID, null);
                //else if (loaiDoiTuong.Equals("LOAI_DOI_TUONG"))
                //process.capNhatNHNSD(action, null, lstID, null);
                LMessage.ShowMessage("M.DungChung.LuuDuLieuThanhCong", LMessage.MessageBoxType.Information);
                BuildGridDoiTuong();
                loadWidthColumnDoiTuong();
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }


        private void tvwTree_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BuildGridDoiTuong();
            loadWidthColumnDoiTuong();
        }

        #endregion

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
                    string loaiDoiTuong = ((RadTreeViewItem)tvwTree.SelectedItem).Tag.ToString();
                    id = int.Parse(listDataRow.First()["id"].ToString());
                    onView(id, loaiDoiTuong);
                }
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.LoiChonDuLieu", LMessage.MessageBoxType.Warning);
                return;
            }
        }

        /// <summary>
        /// Trước khi thêm mới
        /// </summary>
        private void beforeAddNew()
        {
            onAddNew();
        }

        /// <summary>
        /// Trước khi sửa
        /// </summary>
        private void beforeModify()
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
                    string loaiDoiTuong = ((RadTreeViewItem)tvwTree.SelectedItem).Tag.ToString();
                    int id = int.Parse(listDataRow.First()["id"].ToString());

                    // Yêu cầu lock bản ghi cần xử lý
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);

                    bool ret = true;
                    if (loaiDoiTuong.Equals(BusinessConstant.DM_Loai_DTuong.DOI_TUONG.layGiaTri()) || loaiDoiTuong == "")
                    {
                        ret = process.LockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_DM_DTUONG,
                        DatabaseConstant.Table.DM_DTUONG,
                        DatabaseConstant.Action.SUA,
                        listLockId);
                    }
                    else if (loaiDoiTuong.Equals(BusinessConstant.DM_Loai_DTuong.LOAI_DOI_TUONG.layGiaTri()))
                    {
                        ret = process.LockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_DM_LOAI_DTUONG,
                        DatabaseConstant.Table.DM_LOAI_DTUONG,
                        DatabaseConstant.Action.SUA,
                        listLockId);
                    }



                    // Nếu lock thành công >> cho phép xử lý
                    if (ret)
                    {
                        onModify(id, loaiDoiTuong);
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
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

        /// <summary>
        /// Trước khi xóa
        /// </summary>
        private void beforeDelete()
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
                        string loaiDoiTuong = ((RadTreeViewItem)tvwTree.SelectedItem).Tag.ToString();

                        // Yêu cầu lock bản ghi cần xử lý
                        UtilitiesProcess process = new UtilitiesProcess();
                        List<int> listLockedId = new List<int>();

                        bool retLockData = true;
                        if (loaiDoiTuong.Equals(BusinessConstant.DM_Loai_DTuong.DOI_TUONG.layGiaTri()) || loaiDoiTuong == "")
                        {
                            retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                            DatabaseConstant.Function.DC_DM_DTUONG,
                            DatabaseConstant.Table.DM_DTUONG,
                            DatabaseConstant.Action.XOA,
                            listId);
                        }
                        else if (loaiDoiTuong.Equals(BusinessConstant.DM_Loai_DTuong.LOAI_DOI_TUONG.layGiaTri()))
                        {
                            retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                            DatabaseConstant.Function.DC_DM_LOAI_DTUONG,
                            DatabaseConstant.Table.DM_LOAI_DTUONG,
                            DatabaseConstant.Action.XOA,
                            listId);
                        }

                        // Nếu lock thành công >> cho phép xử lý
                        if (retLockData)
                        {
                            onDelete(listId, loaiDoiTuong);
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

        /// <summary>
        /// Xem dữ liệu
        /// </summary>
        private void onView(int id, string loaiDoiTuong)
        {
            if (loaiDoiTuong.Equals(BusinessConstant.DM_Loai_DTuong.DOI_TUONG.layGiaTri()) || loaiDoiTuong == "")
            {
                ucDoiTuongCT ct = new ucDoiTuongCT(maDonVi);
                ct.Action = DatabaseConstant.Action.XEM;
                ct.ID = id;
                //ct. = true;
                Window window = new Window();
                window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.DC_DM_DTUONG);
                window.Content = ct;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
                BuildGridDoiTuong();
                loadWidthColumnDoiTuong();
            }

            else if (loaiDoiTuong.Equals(BusinessConstant.DM_Loai_DTuong.LOAI_DOI_TUONG.layGiaTri()))
            {
                ucDoiTuongLoaiCT ct = new ucDoiTuongLoaiCT();
                ct.Action = DatabaseConstant.Action.XEM;
                ct.ID = id;
                //ct. = true;
                Window window = new Window();
                window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.DC_DM_LOAI_DTUONG);
                window.Content = ct;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
                BuildGridDoiTuong();
                loadWidthColumnDoiTuong();
            }
        }

        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        private void onAddNew()
        {
            string loaiDoiTuong = ((RadTreeViewItem)tvwTree.SelectedItem).Tag.ToString();
            if (loaiDoiTuong.Equals(BusinessConstant.DM_Loai_DTuong.DOI_TUONG.layGiaTri()) || loaiDoiTuong == "")
            {
                ucDoiTuongCT ct = new ucDoiTuongCT(ClientInformation.MaDonVi);
                ct.Action = DatabaseConstant.Action.THEM;
                //ct.ChiXem = true;
                Window window = new Window();
                window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.DC_DM_DTUONG);
                window.Content = ct;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
                BuildGridDoiTuong();
                loadWidthColumnDoiTuong();
            }

            else if (loaiDoiTuong.Equals(BusinessConstant.DM_Loai_DTuong.LOAI_DOI_TUONG.layGiaTri()))
            {
                ucDoiTuongLoaiCT ct = new ucDoiTuongLoaiCT();
                ct.Action = DatabaseConstant.Action.THEM;
                //ct.ChiXem = true;
                Window window = new Window();
                window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.DC_DM_LOAI_DTUONG);
                window.Content = ct;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
                BuildGridDoiTuong();
                loadWidthColumnDoiTuong();
            }
        }

        /// <summary>
        /// Sửa dữ liệu
        /// </summary>
        private void onModify(int id, string loaiDoiTuong)
        {
            if (loaiDoiTuong.Equals(BusinessConstant.DM_Loai_DTuong.DOI_TUONG.layGiaTri()) || loaiDoiTuong == "")
            {
                ucDoiTuongCT ct = new ucDoiTuongCT(maDonVi);
                ct.Action = DatabaseConstant.Action.SUA;
                ct.ID = id;
                //ct.ChiXem = true;
                Window window = new Window();
                window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.DC_DM_DTUONG);
                window.Content = ct;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
                BuildGridDoiTuong();
                loadWidthColumnDoiTuong();
            }

            else if (loaiDoiTuong.Equals(BusinessConstant.DM_Loai_DTuong.LOAI_DOI_TUONG.layGiaTri()))
            {
                ucDoiTuongLoaiCT ct = new ucDoiTuongLoaiCT();
                ct.Action = DatabaseConstant.Action.SUA;
                ct.ID = id;
                //ct.ChiXem = true;
                Window window = new Window();
                window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.DC_DM_LOAI_DTUONG);
                window.Content = ct;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
                BuildGridDoiTuong();
                loadWidthColumnDoiTuong();
            }
        }

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        private void onDelete(List<int> listId, string loaiDoiTuong)
        {
            DanhMucProcess processDanhMuc = new DanhMucProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = true;

                if (loaiDoiTuong.Equals(BusinessConstant.DM_Loai_DTuong.DOI_TUONG.layGiaTri()) || loaiDoiTuong == "")
                {
                    ret = processDanhMuc.XoaDoiTuong(listId.ToArray(), ref listClientResponseDetail);
                }
                else if (loaiDoiTuong.Equals(BusinessConstant.DM_Loai_DTuong.LOAI_DOI_TUONG.layGiaTri()))
                {
                    ret = processDanhMuc.XoaDoiTuongLoai(listId.ToArray(), ref listClientResponseDetail);
                }

                afterDelete(ret, listId, listClientResponseDetail, loaiDoiTuong);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess lockProcess = new UtilitiesProcess();

                bool retUnlockData = true;
                if (loaiDoiTuong.Equals(BusinessConstant.DM_Loai_DTuong.DOI_TUONG.layGiaTri()) || loaiDoiTuong == "")
                {
                    retUnlockData = lockProcess.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_DTUONG,
                    DatabaseConstant.Table.DM_DTUONG,
                    DatabaseConstant.Action.XOA,
                    listId);
                }
                else if (loaiDoiTuong.Equals(BusinessConstant.DM_Loai_DTuong.LOAI_DOI_TUONG.layGiaTri()))
                {
                    retUnlockData = lockProcess.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_LOAI_DTUONG,
                    DatabaseConstant.Table.DM_LOAI_DTUONG,
                    DatabaseConstant.Action.XOA,
                    listId);
                }

                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Sau khi thêm
        /// </summary>
        /// <param name="ret"></param>
        private void afterAddNew()
        {
        }

        /// <summary>
        /// Sau khi xóa
        /// </summary>
        /// <param name="ret"></param>
        private void afterDelete(bool ret,
            List<int> listId,
            List<ClientResponseDetail> listClientResponseDetail,
            string loaiDoiTuong)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
                this.loaiDoiTuong = loaiDoiTuong;
                //LoadDuLieu();
                BuildGridDoiTuong();
                loadWidthColumnDoiTuong();
            }
            else
            {
                //LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                this.loaiDoiTuong = loaiDoiTuong;
                //LoadDuLieu();
                BuildGridDoiTuong();
                loadWidthColumnDoiTuong();
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retUnlockData = true;
            if (loaiDoiTuong.Equals(BusinessConstant.DM_Loai_DTuong.DOI_TUONG.layGiaTri()) || loaiDoiTuong == "")
            {
                retUnlockData = process.LockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_DTUONG,
                DatabaseConstant.Table.DM_DTUONG,
                DatabaseConstant.Action.XOA,
                listId);
            }
            else if (loaiDoiTuong.Equals(BusinessConstant.DM_Loai_DTuong.LOAI_DOI_TUONG.layGiaTri()))
            {
                retUnlockData = process.LockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_LOAI_DTUONG,
                DatabaseConstant.Table.DM_LOAI_DTUONG,
                DatabaseConstant.Action.XOA,
                listId);
            }
        }

        //private void beforeResetpass()
        //{
        //    List<DataRowView> listDataRow = getListSeletedDataRow();

        //    if (listDataRow != null)
        //    {
        //        if (listDataRow.Count == 0)
        //        {
        //            LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
        //            return;
        //        }
        //        else if (listDataRow.Count > 1)
        //        {
        //            LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
        //            return;
        //        }
        //        else
        //        {
        //            string loaiDoiTuong = ((RadTreeViewItem)tvwTree.SelectedItem).Tag.ToString();
        //            int id = int.Parse(listDataRow.First()["id"].ToString());
        //            string userName = listDataRow.First()[2].ToString();
        //            string fullName = listDataRow.First()[3].ToString();

        //            bool ret = true;
        //            if (loaiDoiTuong.Equals("DOI_TUONG") || loaiDoiTuong == "")
        //            {
        //                // Đổi mật khẩu
        //                ucResetPassword ct = new ucResetPassword();
        //                Window window = new Window();
        //                window.ResizeMode = ResizeMode.NoResize;
        //                window.Height = 150;
        //                window.Width = 450;
        //                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        //                window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.HT_RESET_PASS);
        //                window.Content = ct;
        //                ct.Id = id;
        //                ct.UserName = userName;
        //                ct.FullName = fullName;
        //                ct.lblUser.Content = "(" + userName + ") " + fullName;
        //                window.ShowDialog();
        //            }
        //            else if (loaiDoiTuong.Equals("LOAI_DOI_TUONG"))
        //            {
        //                LMessage.ShowMessage("Chọn người sử dụng để thiết lập lại mật khẩu", LMessage.MessageBoxType.Warning);
        //                return;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        LMessage.ShowMessage("M.DungChung.LoiChonDuLieu", LMessage.MessageBoxType.Warning);
        //        return;
        //    }
        //}

        /// <summary>
        /// Lấy danh sách id được chọn
        /// </summary>
        /// <returns></returns>
        private List<DataRowView> getListSeletedDataRow()
        {
            List<DataRowView> listDataRow = new List<DataRowView>();
            if (grDanhSach.SelectedItems.Count <= 0)
            {
                return listDataRow;
            }
            else
            {
                for (int i = 0; i < grDanhSach.SelectedItems.Count; i++)
                {
                    DataRowView dr = (DataRowView)grDanhSach.SelectedItems[i];
                    listDataRow.Add(dr);
                }
                return listDataRow;
            }
        }


        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void onHelp()
        {
            PresentationWPF.CustomControl.CommonFunction.ShowHelp(this);
        }

        /// <summary>
        /// Đóng form
        /// </summary>
        private void onClose()
        {
            Release();
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(this);
        }

        /// <summary>
        /// Hàm xử lý giải phóng lock dữ liệu cho form
        /// </summary>
        private void Release()
        {
            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool ret = process.UnlockDataFromFunctionByUser(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_LOAI_DTUONG);

            ret = process.UnlockDataFromFunctionByUser(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_DTUONG);
        }

        /// <summary>
        /// Sự kiện unlod cho form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Release();

            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(this);
        }

    }
}
