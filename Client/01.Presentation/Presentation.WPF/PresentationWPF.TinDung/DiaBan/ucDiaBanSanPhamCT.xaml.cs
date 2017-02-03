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
using Presentation.Process.TinDungServiceRef;
using Presentation.Process.Common;
using Presentation.Process;
using Telerik.Windows.Controls;
using Presentation.Process.PopupServiceRef;
using System.Reflection;
using Telerik.Windows.Controls.GridView;
using PresentationWPF.BaoCao.DungChung;
using System.Collections;

namespace PresentationWPF.TinDung.DiaBan
{
    /// <summary>
    /// Interaction logic for ucDiaBanSanPhamCT.xaml
    /// </summary>
    public partial class ucDiaBanSanPhamCT : UserControl
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

        private string _namDL;
        DataTable dtTreeDLy;
        SAN_PHAM_DBAN obj = new SAN_PHAM_DBAN();
        List<SAN_PHAM_DBAN_CTIET> lstDanhSachSanpham = new List<SAN_PHAM_DBAN_CTIET>();
        List<AutoCompleteEntry> lstSourceSanPham = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiSanPham = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<int> lstIDXoa = new List<int>();
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        string MaDiaBan = "";
        int idDiaBan = 0;
        Presentation.Process.DanhMucServiceRef.DM_CUM objCum = null;
        #endregion

        #region Khoi tao
        public ucDiaBanSanPhamCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDung;component/DiaBan/ucDiaBanSanPhamCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            
            InitEvenHanler();
            radDanhSachLichPhat.ItemsSource = lstDanhSachSanpham;
            List<string> lstDieuKien = new List<string>();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.LOAI_SAN_PHAM_TDUNG.getValue());
            new AutoComboBox().GenAutoComboBox(ref lstSourceLoaiSanPham, ref cmbLoaiSanPham, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
            lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.TenDangNhap);
            lstDieuKien.Add(ClientInformation.TenDonViQuanLy);
            lstDieuKien.Add(DatabaseConstant.ToChucDonVi.CNH.getValue());
            new AutoComboBox().GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVI_THEOPVI.getValue(), lstDieuKien, ClientInformation.MaDonVi);
            BuildTreeKhuVuc();
            obj.CHIEU_DU_LIEU = "LOAI_SAN_PHAM";
            LoadComboxBoxSanPham();
            LayDuLieu();
        }

        public void InitEvenHanler()
        {
            //radDanhSachLichPhat.CellEditEnded += radDanhSachLichPhat_CellEditEnded;
            radDanhSachLichPhat.BeginningEdit += radDanhSachLichPhat_BeginningEdit;
            cmbSanPham.SelectionChanged += new SelectionChangedEventHandler(cmbSanPham_SelectionChanged);
            tvwKhuVuc.SelectionChanged += new SelectionChangedEventHandler(tvwKhuVuc_SelectionChanged);
            btnAddTaiKhoan.Click += new RoutedEventHandler(btnAddTaiKhoan_Click);
            rdoDiaBan.Checked += new RoutedEventHandler(rdoDiaBan_Checked);
            rdoSanPham.Checked += new RoutedEventHandler(rdoSanPham_Checked);
            rdoLoaiSanPham.Checked += new RoutedEventHandler(rdoLoaiSanPham_Checked);
            cmbLoaiSanPham.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiSanPham_SelectionChanged);
            cmbChiNhanh.SelectionChanged += new SelectionChangedEventHandler(cmbChiNhanh_SelectionChanged);
            radDanhSachLichPhat.Deleted += new EventHandler<GridViewDeletedEventArgs>(radDanhSachLichPhat_Deleted);
        }

        private void FormatData()
        {

        }

        void BuildTreeKhuVuc()
        {
            dtTreeDLy = new TinDungProcess().GetTreeViewCum(ClientInformation.MaDonViQuanLy, ClientInformation.TenDangNhap).Tables[0];
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
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {

            }
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
            {
                LayDuLieu();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
            { }
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
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                OnSave();
            }
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
            {
                LayDuLieu();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
            { }
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

        #region Xy ly giao dien
        //void radDanhSachLichPhat_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        //{
        //    CUM_LICH objCumLich = e.Cell.ParentRow.Item as CUM_LICH;
        //    Grid gridparent = e.EditingElement as Grid;
        //    int index = lstPhatVon.IndexOf(objCumLich);
        //    lstPhatVon[index] = objCumLich;
        //}

        void radDanhSachLichThu_BeginningEdit(object sender, GridViewBeginningEditRoutedEventArgs e)
        {
            if (!LObject.IsNullOrEmpty(e.Cell.Column.UniqueName) & e.Cell.Column.UniqueName.Contains("THANG"))
            {
                ColumnsDateTimeMulti col = e.Cell.Column as ColumnsDateTimeMulti;
                if (col.DisplayDate < ClientInformation.NgayLamViecHienTai.StringToDate(ApplicationConstant.defaultDateTimeFormat))
                    e.Cancel = true;
            }
        }

        void radDanhSachLichPhat_BeginningEdit(object sender, GridViewBeginningEditRoutedEventArgs e)
        {
            if (!LObject.IsNullOrEmpty(e.Cell.Column.UniqueName) & e.Cell.Column.UniqueName.Contains("THANG"))
            {
                ColumnsDateTimeMulti col = e.Cell.Column as ColumnsDateTimeMulti;
                if (col.DisplayDate < ClientInformation.NgayLamViecHienTai.StringToDate(ApplicationConstant.defaultDateTimeFormat))
                    e.Cancel = true;
            }
        }

        void rdoSanPham_Checked(object sender, RoutedEventArgs e)
        {
            btnAddTaiKhoan.IsEnabled = true;
            if(cmbSanPham.SelectedIndex < 0)
                btnAddTaiKhoan.IsEnabled = false;
            obj.CHIEU_DU_LIEU = "SAN_PHAM";
            radDanhSachLichPhat.Columns[3].IsVisible = true;
            radDanhSachLichPhat.Columns[4].IsVisible = true;
            radDanhSachLichPhat.Columns[5].IsVisible = false;
            radDanhSachLichPhat.Columns[6].IsVisible = false;
            tvwKhuVuc.SelectedItem = tvwKhuVuc.Items[0];
            cmbSanPham.IsEnabled = true;
            cmbLoaiSanPham.IsEnabled = false;
            tvwKhuVuc.IsEnabled = false;
            MaDiaBan = "";
            lstDanhSachSanpham = new List<SAN_PHAM_DBAN_CTIET>();
            radDanhSachLichPhat.ItemsSource = lstDanhSachSanpham;
            radDanhSachLichPhat.Rebind();
        }

        void rdoLoaiSanPham_Checked(object sender, RoutedEventArgs e)
        {
            btnAddTaiKhoan.IsEnabled = true;
            if (cmbLoaiSanPham.SelectedIndex < 0)
                btnAddTaiKhoan.IsEnabled = false;
            obj.CHIEU_DU_LIEU = "LOAI_SAN_PHAM";
            radDanhSachLichPhat.Columns[3].IsVisible = true;
            radDanhSachLichPhat.Columns[4].IsVisible = true;
            radDanhSachLichPhat.Columns[5].IsVisible = false;
            radDanhSachLichPhat.Columns[6].IsVisible = false;
            tvwKhuVuc.SelectedItem = tvwKhuVuc.Items[0];
            cmbLoaiSanPham.IsEnabled = true;
            cmbSanPham.IsEnabled = false;
            tvwKhuVuc.IsEnabled = false;
            MaDiaBan = "";
            lstDanhSachSanpham = new List<SAN_PHAM_DBAN_CTIET>();
            radDanhSachLichPhat.ItemsSource = lstDanhSachSanpham;
            radDanhSachLichPhat.Rebind();
        }

        void rdoDiaBan_Checked(object sender, RoutedEventArgs e)
        {
            obj.CHIEU_DU_LIEU = "DIA_BAN";
            radDanhSachLichPhat.Columns[3].IsVisible = false;
            radDanhSachLichPhat.Columns[4].IsVisible = false;
            radDanhSachLichPhat.Columns[5].IsVisible = true;
            radDanhSachLichPhat.Columns[6].IsVisible = true;
            cmbLoaiSanPham.IsEnabled = false;
            cmbSanPham.IsEnabled = false;
            tvwKhuVuc.IsEnabled = true;
            cmbSanPham.SelectedIndex = -1;
            btnAddTaiKhoan.IsEnabled = false;
            lstDanhSachSanpham = new List<SAN_PHAM_DBAN_CTIET>();
            radDanhSachLichPhat.ItemsSource = lstDanhSachSanpham;
            radDanhSachLichPhat.Rebind();
        }

        void btnAddTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            if (obj.CHIEU_DU_LIEU == "SAN_PHAM")
            {
                if (cmbSanPham.SelectedIndex < 0)
                    return;
                LoadPopUpDiaBan();
            }
            else if (obj.CHIEU_DU_LIEU == "LOAI_SAN_PHAM")
            {
                if (cmbLoaiSanPham.SelectedIndex < 0)
                    return;
                LoadPopUpDiaBan();
            }
            else
            {
                LoadPopUpSanPham();
            }
        }

        void LoadPopUpSanPham()
        {
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.MaDonVi);
            lstPopup = new List<DataRow>();
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation("POPUP_DS_SANPHAM_TD", lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            ucPopup popup = new ucPopup(false, simplePopupResponse, true);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.Content = popup;
            win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ShowDialog();
            if (lstPopup.Count > 0)
            {
                foreach (DataRow dr in lstPopup)
                {
                    if (lstDanhSachSanpham.Where(f => f.MA_SAN_PHAM.Contains(dr["MA_SAN_PHAM"].ToString())).Count() < 1)
                    {
                        SAN_PHAM_DBAN_CTIET objSanPhamCTiet = new SAN_PHAM_DBAN_CTIET();
                        objSanPhamCTiet.NGAY_ADUNG = ClientInformation.NgayLamViecHienTai;
                        objSanPhamCTiet.ID_DBAN = idDiaBan;
                        objSanPhamCTiet.ID_SAN_PHAM = Convert.ToInt32(dr["ID"]);
                        objSanPhamCTiet.LOAI_DBAN = "DM_CUM";
                        objSanPhamCTiet.MA_DBAN = MaDiaBan;
                        objSanPhamCTiet.MA_DVI_QLY = objCum.MA_DVI_QLY;
                        objSanPhamCTiet.MA_DVI_TAO = objCum.MA_DVI_TAO;
                        objSanPhamCTiet.MA_SAN_PHAM = dr["MA_SAN_PHAM"].ToString();
                        objSanPhamCTiet.TEN_SAN_PHAM = dr["TEN_SAN_PHAM"].ToString();
                        objSanPhamCTiet.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                        objSanPhamCTiet.NGUOI_NHAP = ClientInformation.TenDangNhap;
                        objSanPhamCTiet.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                        objSanPhamCTiet.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                        lstDanhSachSanpham.Add(objSanPhamCTiet);
                    }
                    radDanhSachLichPhat.ItemsSource = lstDanhSachSanpham;
                    radDanhSachLichPhat.Rebind();
                }
            }
        }

        void LoadPopUpDiaBan()
        {
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
            lstPopup = new List<DataRow>();
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation("POPUP_DS_CUM", lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            ucPopup popup = new ucPopup(false, simplePopupResponse, true);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.Content = popup;
            win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ShowDialog();
            if (lstPopup.Count > 0)
            {
                AutoCompleteEntry auSanPham = null;
                if(cmbSanPham.SelectedIndex<0)
                auSanPham = lstSourceSanPham.ElementAt(cmbSanPham.SelectedIndex);
                AutoCompleteEntry auLoaiSanPham = null;
                auLoaiSanPham = lstSourceLoaiSanPham.ElementAt(cmbLoaiSanPham.SelectedIndex);
                foreach (DataRow dr in lstPopup)
                {
                    if (lstDanhSachSanpham.Where(f => f.MA_DBAN.Contains(dr["MA_CUM"].ToString())).Count() < 1)
                    {
                        SAN_PHAM_DBAN_CTIET objSanPhamCTiet = new SAN_PHAM_DBAN_CTIET();
                        objSanPhamCTiet.NGAY_ADUNG = ClientInformation.NgayLamViecHienTai;
                        objSanPhamCTiet.ID_DBAN = Convert.ToInt32(dr["ID"]);
                        if (obj.CHIEU_DU_LIEU == "SAN_PHAM")
                            objSanPhamCTiet.ID_SAN_PHAM = Convert.ToInt32(auSanPham.KeywordStrings[1]);
                        else
                            objSanPhamCTiet.ID_SAN_PHAM = 0;
                        objSanPhamCTiet.LOAI_DBAN = "DM_CUM";
                        objSanPhamCTiet.MA_DBAN = dr["MA_CUM"].ToString();
                        objSanPhamCTiet.TEN_DBAN = dr["TEN_CUM"].ToString();
                        objSanPhamCTiet.MA_DVI_QLY = ClientInformation.MaDonVi;
                        objSanPhamCTiet.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                        if (obj.CHIEU_DU_LIEU == "SAN_PHAM")
                            objSanPhamCTiet.MA_SAN_PHAM = auSanPham.KeywordStrings.FirstOrDefault();
                        else
                        {
                            objSanPhamCTiet.MA_SAN_PHAM = auLoaiSanPham.KeywordStrings.FirstOrDefault();
                            obj.LOAI_SAN_PHAM = auLoaiSanPham.KeywordStrings.FirstOrDefault();
                        }
                        objSanPhamCTiet.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                        objSanPhamCTiet.NGUOI_NHAP = ClientInformation.TenDangNhap;
                        objSanPhamCTiet.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                        objSanPhamCTiet.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                        lstDanhSachSanpham.Add(objSanPhamCTiet);
                    }
                }
                radDanhSachLichPhat.ItemsSource = lstDanhSachSanpham;
                radDanhSachLichPhat.Rebind();
            }
        }

        void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnAddTaiKhoan.IsEnabled = true;
            btnCommitTaiKhoan.IsEnabled = true;
            btnCancelTaiKhoan.IsEnabled = true;
            btnDeleteTaiKhoan.IsEnabled = true;
            LoadComboxBoxSanPham();
            LayDuLieu();
        }

        void cmbLoaiSanPham_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnAddTaiKhoan.IsEnabled = true;
            LayDuLieu();
        }

        void radDanhSachLichPhat_Deleted(object sender, GridViewDeletedEventArgs e)
        {
            foreach (SAN_PHAM_DBAN_CTIET objSanPham in e.Items)
            {
                if (obj.CHIEU_DU_LIEU == "LOAI_SAN_PHAM")
                    lstIDXoa.Add(objSanPham.ID_DBAN);
                else
                    lstIDXoa.Add(objSanPham.ID);
            }
            
        }

        void tvwKhuVuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!((RadTreeViewItem)tvwKhuVuc.SelectedItem).Tag.ToString().Substring(0, 3).Equals("CUM"))
            {
                if (rdoDiaBan.IsChecked.GetValueOrDefault())
                    btnAddTaiKhoan.IsEnabled = false;
                return;
            }

            string res = "";
            idDiaBan = ((RadTreeViewItem)tvwKhuVuc.SelectedItem).Tag.ToString().Substring(3).StringToInt32();
            new Presentation.Process.DanhMucProcess().getCumById(idDiaBan, ref objCum, ref res);
            if (!LObject.IsNullOrEmpty(objCum))
            {
                btnAddTaiKhoan.IsEnabled = true;
                MaDiaBan = objCum.MA_CUM;
                idDiaBan = objCum.ID;
                LayDuLieu();
            }
            else
            {
                btnAddTaiKhoan.IsEnabled = false;
            }
        }

        void cmbSanPham_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnAddTaiKhoan.IsEnabled = true;
            LayDuLieu();
        }

        void LoadComboxBoxSanPham()
        {
            List<string> lstDieuKien = new List<string>();
            AutoCompleteEntry au = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex);
            lstDieuKien.Add(au.KeywordStrings.FirstOrDefault());
            lstSourceSanPham.Clear();
            cmbSanPham.Items.Clear();
            new AutoComboBox().GenAutoComboBox(ref lstSourceSanPham, ref cmbSanPham, "COMBOBOX_SAN_PHAM_TD_DBAN", lstDieuKien);
        }

        #endregion
        #region Xu ly nghiep vu
        private void LayDuLieu()
        {
            Cursor = Cursors.Wait;
            try
            {
                lstDanhSachSanpham.Clear();
                if (LObject.IsNullOrEmpty(tvwKhuVuc.SelectedItem)) tvwKhuVuc.SelectedItem = tvwKhuVuc.Items[0];
                string ListKVuc = "";
                if (((RadTreeViewItem)tvwKhuVuc.SelectedItem).Tag.ToString().Substring(0, 3).Equals("CUM"))
                {
                    ListKVuc = MaDiaBan;
                }
                if (ListKVuc.IsNullOrEmptyOrSpace())
                {
                    ListKVuc = "0";
                }
                string MaSanPham = "0";
                if (cmbSanPham.SelectedIndex > -1)
                    MaSanPham = lstSourceSanPham.ElementAt(cmbSanPham.SelectedIndex).KeywordStrings[1];
                string LoaiSanPham = "";
                if (cmbLoaiSanPham.SelectedIndex > -1)
                    LoaiSanPham = lstSourceLoaiSanPham.ElementAt(cmbLoaiSanPham.SelectedIndex).KeywordStrings.FirstOrDefault();
                string maChiNhanh = "";
                if (cmbChiNhanh.SelectedIndex > -1)
                    maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@INP_CHIEU_DLIEU", "string", obj.CHIEU_DU_LIEU);
                LDatatable.AddParameter(ref dt, "@INP_SAN_PHAM", "string", MaSanPham);
                LDatatable.AddParameter(ref dt, "@INP_DBAN", "string", ListKVuc);
                LDatatable.AddParameter(ref dt, "@INP_LOAI_SAN_PHAM", "string", LoaiSanPham);
                LDatatable.AddParameter(ref dt, "@INP_MA_DVI", "string", maChiNhanh);
                DataSet ds = new TinDungProcess().GetThongTinDiaBanSanPham(dt);
                if (!LObject.IsNullOrEmpty(ds) && ds.Tables.Count > 0)
                {
                    dt = ds.Tables["CHI_TIET"];
                    if (!LObject.IsNullOrEmpty(dt) && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            SAN_PHAM_DBAN_CTIET objSanPhamDiaBan = new SAN_PHAM_DBAN_CTIET();
                            objSanPhamDiaBan.ID = Convert.ToInt32(dr["ID"]);
                            objSanPhamDiaBan.ID_DBAN = Convert.ToInt32(dr["ID_DBAN"]);
                            objSanPhamDiaBan.ID_SAN_PHAM = Convert.ToInt32(dr["ID_DBAN"]);
                            objSanPhamDiaBan.LOAI_DBAN = dr["LOAI_DBAN"].ToString();
                            objSanPhamDiaBan.MA_DBAN = dr["MA_DBAN"].ToString();
                            objSanPhamDiaBan.MA_DVI_QLY = dr["MA_DVI_QLY"].ToString();
                            objSanPhamDiaBan.MA_DVI_TAO = dr["MA_DVI_TAO"].ToString();
                            objSanPhamDiaBan.MA_SAN_PHAM = dr["MA_SAN_PHAM"].ToString();
                            objSanPhamDiaBan.NGAY_ADUNG = dr["NGAY_ADUNG"].ToString();
                            objSanPhamDiaBan.NGAY_CNHAT = dr["NGAY_CNHAT"].ToString();
                            objSanPhamDiaBan.NGAY_HHAN = dr["NGAY_HHAN"].ToString();
                            objSanPhamDiaBan.NGAY_NHAP = dr["NGAY_NHAP"].ToString();
                            objSanPhamDiaBan.TEN_SAN_PHAM = dr["TEN_SAN_PHAM"].ToString();
                            objSanPhamDiaBan.TEN_DBAN = dr["TEN_DBAN"].ToString();
                            objSanPhamDiaBan.NGUOI_CNHAT = dr["NGUOI_CNHAT"].ToString();
                            objSanPhamDiaBan.NGUOI_NHAP = dr["NGUOI_NHAP"].ToString();
                            objSanPhamDiaBan.TTHAI_BGHI = dr["TTHAI_BGHI"].ToString();
                            objSanPhamDiaBan.TTHAI_NVU = dr["TTHAI_NVU"].ToString();
                            lstDanhSachSanpham.Add(objSanPhamDiaBan);
                        }
                        radDanhSachLichPhat.ItemsSource = lstDanhSachSanpham;
                        radDanhSachLichPhat.Rebind();
                    }
                    else
                    {
                        lstDanhSachSanpham.Clear();
                        radDanhSachLichPhat.ItemsSource = lstDanhSachSanpham;
                        radDanhSachLichPhat.Rebind();
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }
        private void SetDataControl()
        {
            
        }
        private bool GetDataForm()
        {
            bool kq = true;
            try
            {
                string LoaiSanPham = "";
                if (cmbLoaiSanPham.SelectedIndex > -1)
                    LoaiSanPham = lstSourceLoaiSanPham.ElementAt(cmbLoaiSanPham.SelectedIndex).KeywordStrings.FirstOrDefault();
                obj.LOAI_SAN_PHAM = LoaiSanPham;
                obj.DSACH_SAN_PHAM_DBAN_CTIET = lstDanhSachSanpham.ToArray();
                obj.DSACH_ID_XOA = lstIDXoa.ToArray();
            }
            catch (Exception ex)
            {
                kq = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return kq;
        }
        private bool Validation()
        {
            AutoCompleteEntry au = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex);
            if (au.KeywordStrings.FirstOrDefault() != ClientInformation.MaDonVi)
            {
                LMessage.ShowMessage("M_ResponseMessage_GIAODICH_KhongThuocKiemSoatCuaPhongGD", LMessage.MessageBoxType.Warning);
                return false;
            }
            return true;
        }

        private void OnSave()
        {
            Cursor = Cursors.Wait;
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            try
            {
                if (Validation())
                {
                    if (GetDataForm())
                    {
                        iret = new TinDungProcess().DiaBanSanPham(DatabaseConstant.Action.THEM, ref obj, ref lstResponseDetail);
                        AfterSave(iret, lstResponseDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
            }
            finally
            {
                lstResponseDetail = null;
                Cursor = Cursors.Arrow;
            }
        }

        private void AfterSave(int iret, List<ClientResponseDetail> lstResponseDetail)
        {
            if (iret > 0)
                LMessage.ShowMessage("M.DungChung.LuuDuLieuThanhCong", LMessage.MessageBoxType.Information);
            else
                CommonFunction.ThongBaoKetQua(lstResponseDetail);
            lstIDXoa.Clear();
            LayDuLieu();
        }
        #endregion
    }
}
