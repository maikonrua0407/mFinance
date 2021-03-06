﻿using System;
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
using Presentation.Process;
using Utilities.Common;
using PresentationWPF.CustomControl;
using Telerik.Windows.Controls;
using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process.TruyVanServiceRef;
using Presentation.Process.Common;

namespace PresentationWPF.DanhMuc.DiaBan
{
    /// <summary>
    /// Interaction logic for ucDiaBanDS.xaml
    /// </summary>
    public partial class ucDiaBanDS : UserControl
    {
        #region Khai bao

        public string LoaiDiaBan = "";
        public int IdTinhTP = 0;
        public string MaTinhTP = "";
        public string TenTinhTP = "";
        public int IdQuanHuyen = 0;
        public string MaQuanHuyen = "";
        public string TenQuanHuyen = "";

        public int ID;
        public string Ma;
        public string Ten;
        DataSet dsData = new DataSet();
        DataTable dtMaster = new DataTable();
        DataTable dtDetail = new DataTable();
        DataTable grdData = new DataTable();
        delegate void LoadDuLieuCT();
        List<HeaderDto> lstHeader;
        bool columnsWidthLoad = false;
        List<AutoCompleteEntry> lstSourceLoaiDiaBan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceQuanHuyen = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceQuanHuyen_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhuongXa = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhuongXa_Select = new List<AutoCompleteEntry>();

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
        private List<string> lstChecked = new List<string>();

        private DatabaseConstant.Module Module = DatabaseConstant.Module.DMDC;
        private DatabaseConstant.Function Function = DatabaseConstant.Function.DC_DM_DIA_BAN;
        #endregion

        #region Khoi tao

        public ucDiaBanDS()
        {
            InitializeComponent();
            InitEventHandler();
            
            BindHotkey();
            LoadCombobox();
            LoadDuLieu();
        }

        /// <summary>
        /// InitEventHandler
        /// </summary>
        private void InitEventHandler()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.DanhMuc;component/DiaBan/ucDiaBanDS.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void LoadCombobox()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.LOAI_DIA_BAN.getValue());
            auto.GenAutoComboBox(ref lstSourceLoaiDiaBan, ref cmbLoaiDiaBan, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, DatabaseConstant.DanhSachLoaiDiaBan.HUYEN_QUAN.getValue());

            auto.removeEntry(ref lstSourceLoaiDiaBan, ref cmbLoaiDiaBan, DatabaseConstant.DanhSachLoaiDiaBan.TINH_THANHPHO.getValue());
            auto.removeEntry(ref lstSourceLoaiDiaBan, ref cmbLoaiDiaBan, DatabaseConstant.DanhSachLoaiDiaBan.LANG_TODP.getValue());
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

                    if (key != null)
                    {
                        InputBindings.Add(key);
                    }
                }
            }
        }

        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbAdd.IsEnabled;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeAddNew();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbModify.IsEnabled;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeModify();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbDelete.IsEnabled;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeDelete();
        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute = tlbApprove.IsEnabled;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeApprove();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute = tlbRefuse.IsEnabled;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeRefuse();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute = tlbCancel.IsEnabled;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeCancel();
        }

        private void ViewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbView.IsEnabled;
        }
        private void ViewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeView();
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
            Reload();
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
                beforeAddNew();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                beforeModify();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                beforeDelete();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                beforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                beforeRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                beforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
                beforeView();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
                txtTimKiemNhanh.RaiseEvent(new RoutedEventArgs(Button.GotFocusEvent));
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
                Reload();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
                XuatExcel();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                onClose();
            }
        }

        #endregion

        #region Xu ly giao dien

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
                beforeAddNew();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                beforeModify();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                beforeDelete();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                beforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                beforeRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                beforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
                beforeView();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
                txtTimKiemNhanh.RaiseEvent(new RoutedEventArgs(Button.GotFocusEvent));
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
                Reload();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
                XuatExcel();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                onClose();
            }
        }

        /// <summary>
        /// Lấy lại dữ liệu
        /// </summary>
        private void Reload()
        {
            LoadDuLieu();
        }  

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtTimKiemNhanh.KeyDown += txtTimKiemNhanh_KeyDown;
            txtTimKiemNhanh.GotFocus += txtTimKiemNhanh_GotFocus;
            txtTimKiemNhanh.LostFocus += txtTimKiemNhanh_LostFocus;
            txtTimKiemNhanh.Focus();
        }

        /// <summary>
        /// Sự kiện tìm kiếm nhanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTimKiemNhanh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txtTimKiemNhanh.Text == LLanguage.SearchResourceByKey("U.DungChung.TimKiemNhanh"))
                {
                    return;
                }
                PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grDanhSach, txtTimKiemNhanh.Text);

                #region LoadWidthColumns
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
                        grDanhSach.Columns[idx].Width = new Telerik.Windows.Controls.GridViewLength(width, unit);
                        grDanhSach.Columns[idx].IsReadOnly = true;
                        grDanhSach.Columns[idx].Header = LLanguage.SearchResourceByKey(item.LanguageKey);
                    }
                    else
                        grDanhSach.Columns[idx].IsVisible = false;
                    idx = idx + 1;
                }
                columnsWidthLoad = true;
                #endregion
            }
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
        /// Sự kiện thay đổi số bản ghi trên một page hiển thị
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudPageSize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (grDanhSach != null)
            {
                radPage.PageSize = (int)nudPageSize.Value;
                if (grdData.Rows.Count > 0)
                    grDanhSach.ItemsSource = grdData;
                lblTong.Content = grdData.Rows.Count;
                loadWidthColumn();
            }
        }

        private void LayLai()
        { }

        private void getAllData()
        {
            var process = new TruyVanProcess();
            List<string> lstDkLoc = new List<string>();
            lstDkLoc.Add(ucTrangThaiNVu.GetItemsSelected());
            lstDkLoc.Add(ucTrangThaiSDung.GetItemsSelected());
            lstDkLoc.Add(IdTinhTP.ToString());
            DanhSachResponse DanhSachResponse = process.getDanhSachInformation(DatabaseConstant.DanhSachTruyVan.DM_DIABAN.getValue(), lstDkLoc);

            if (DanhSachResponse.DataSetSource != null)
            {
                lstHeader = DanhSachResponse.ListHeader.ToList();
                dsData = DanhSachResponse.DataSetSource;
                dtMaster = DanhSachResponse.DataSetSource.Tables[0];
                dtDetail = DanhSachResponse.DataSetSource.Tables[1];
            }
        }

        private void getGridData()
        {
            var process = new TruyVanProcess();
            List<string> lstDkLoc = new List<string>();
            lstDkLoc.Add(ucTrangThaiNVu.GetItemsSelected());
            lstDkLoc.Add(ucTrangThaiSDung.GetItemsSelected());
            lstDkLoc.Add(IdTinhTP.ToString());
            DanhSachResponse DanhSachResponse = process.getDanhSachInformation(DatabaseConstant.DanhSachTruyVan.DM_DIABAN.getValue(), lstDkLoc);

            if (DanhSachResponse.DataSetSource != null)
            {
                lstHeader = DanhSachResponse.ListHeader.ToList();
                //dsData = DanhSachResponse.DataSetSource;
                //dtMaster = DanhSachResponse.DataSetSource.Tables[0];
                dtDetail = DanhSachResponse.DataSetSource.Tables[1];
            }
        }

        public void LoadDuLieu()
        {
            getAllData();
            if (dsData != null &&
                dtMaster != null &&
                dtDetail != null)
            {                
                while (tvwTree.Items.Count > 0)
                    tvwTree.Items.RemoveAt(0);
                UpdateLayout();

                DataRow drRoot = dtMaster.Rows[0];
                RadTreeViewItem rootItem = new RadTreeViewItem();
                rootItem.Header = drRoot["NODE_NAME"].ToString();
                rootItem.Tag = drRoot["NODE"].ToString();
                rootItem.Uid = drRoot["NODE_INFO"].ToString();
                rootItem.IsExpanded = true;
                tvwTree.Items.Add(rootItem);
                BuildTree(rootItem);
            }
        }

        protected void BuildTree(RadTreeViewItem item)
        {
            foreach (DataRow row in dtMaster.Rows)
            {
                if (row["NODE_PARENT"].ToString() == item.Tag.ToString())
                {
                    RadTreeViewItem subItem = new RadTreeViewItem();
                    subItem.Header = row["NODE_NAME"].ToString();
                    subItem.Tag = row["NODE"].ToString();
                    subItem.Uid = row["NODE_INFO"].ToString();
                    subItem.IsExpanded = true;
                    item.Items.Add(subItem);
                    BuildTree(subItem);
                }
            }
        }

        private void tvwTree_Checked(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            BuildGrid();
            loadWidthColumn();
        }

        private void BuildGrid()
        {
            if (IdTinhTP != 0)
            {
                // Nếu loại địa bàn là quận huyện
                string maQuanHuyen = "";
                string idQuanHuyen = "";
                if (LoaiDiaBan.Equals(DatabaseConstant.DanhSachLoaiDiaBan.HUYEN_QUAN.getValue()))
                {
                    if (cmbQuanHuyen.SelectedIndex >= 0)
                    {
                        maQuanHuyen = lstSourceQuanHuyen.ElementAt(cmbQuanHuyen.SelectedIndex).KeywordStrings.ElementAt(0);
                        idQuanHuyen = lstSourceQuanHuyen.ElementAt(cmbQuanHuyen.SelectedIndex).KeywordStrings.ElementAt(2);
                    }

                    if (!string.IsNullOrEmpty(maQuanHuyen) && maQuanHuyen.Equals("") != true)
                    {
                        grdData = new DataTable();
                        foreach (DataColumn col in dtDetail.Columns)
                        {
                            grdData.Columns.Add(col.ColumnName, typeof(string));
                        }

                        int stt = 0;
                        // Nếu là tất cả quận huyện
                        if (maQuanHuyen.Equals("NULL"))
                        {
                            foreach (DataRow row in dtDetail.Rows)
                            {
                                if (row["LOAI_DBAN"].ToString().Equals(DatabaseConstant.DanhSachLoaiDiaBan.HUYEN_QUAN.getValue()))
                                {
                                    stt = stt + 1;
                                    row[0] = stt.ToString();
                                    grdData.ImportRow(row);
                                }
                            }
                        }
                        // Nếu là một quận huyện cụ thể
                        else
                        {
                            foreach (DataRow row in dtDetail.Rows)
                            {
                                if (maQuanHuyen.Equals(row["MA_DBAN"].ToString()) &&
                                    row["LOAI_DBAN"].ToString().Equals(DatabaseConstant.DanhSachLoaiDiaBan.HUYEN_QUAN.getValue()))
                                {
                                    stt = stt + 1;
                                    row[0] = stt.ToString();
                                    grdData.ImportRow(row);
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (LoaiDiaBan.Equals(DatabaseConstant.DanhSachLoaiDiaBan.XA_PHUONG.getValue()))
                {
                    if (cmbQuanHuyen.SelectedIndex >= 0)
                    {
                        maQuanHuyen = lstSourceQuanHuyen.ElementAt(cmbQuanHuyen.SelectedIndex).KeywordStrings.ElementAt(0);
                        idQuanHuyen = lstSourceQuanHuyen.ElementAt(cmbQuanHuyen.SelectedIndex).KeywordStrings.ElementAt(2);
                    }

                    if (!string.IsNullOrEmpty(maQuanHuyen) && maQuanHuyen.Equals("") != true)
                    {
                        grdData = new DataTable();
                        foreach (DataColumn col in dtDetail.Columns)
                        {
                            grdData.Columns.Add(col.ColumnName, typeof(string));
                        }

                        int stt = 0;
                        // Nếu là tất cả quận huyện
                        if (maQuanHuyen.Equals("NULL"))
                        {
                            foreach (DataRow row in dtDetail.Rows)
                            {
                                if (row["LOAI_DBAN"].ToString().Equals(DatabaseConstant.DanhSachLoaiDiaBan.XA_PHUONG.getValue()))
                                {
                                    stt = stt + 1;
                                    row[0] = stt.ToString();
                                    grdData.ImportRow(row);
                                }
                            }
                        }
                        // Nếu là một quận huyện cụ thể
                        else
                        {
                            foreach (DataRow row in dtDetail.Rows)
                            {
                                if (idQuanHuyen.Equals(row["ID_DBAN_CHA"].ToString()) &&
                                    row["LOAI_DBAN"].ToString().Equals(DatabaseConstant.DanhSachLoaiDiaBan.XA_PHUONG.getValue()))
                                {
                                    stt = stt + 1;
                                    row[0] = stt.ToString();
                                    grdData.ImportRow(row);
                                }
                            }
                        }
                    }
                }

                /*
                if (!string.IsNullOrEmpty(maQuanHuyen) && maQuanHuyen.Equals("") != true)
                {
                    grdData = new DataTable();
                    foreach (DataColumn col in dtDetail.Columns)
                    {
                        grdData.Columns.Add(col.ColumnName, typeof(string));
                    }
                    int stt = 0;
                    foreach (DataRow row in dtDetail.Rows)
                    {
                        if (maQuanHuyen.Equals(row["KEY"].ToString()) && row["LOAI_DBAN"].ToString().Equals(DatabaseConstant.DanhSachLoaiDiaBan.XA_PHUONG.getValue()))
                        {
                            stt = stt + 1;
                            row[0] = stt.ToString();
                            grdData.ImportRow(row);
                        }
                    }
                    if (stt == 0)
                    {
                        foreach (DataRow row in dtDetail.Rows)
                        {
                            maQuanHuyen = lstSourceQuanHuyen.ElementAt(cmbQuanHuyen.SelectedIndex).KeywordStrings.ElementAt(1);
                            if (maQuanHuyen.Equals(row["KEY"].ToString()))
                            {
                                stt = stt + 1;
                                row[0] = stt.ToString();
                                grdData.ImportRow(row);
                            }
                        }
                    }
                }
                */
                
                if (grdData.Rows.Count > 0)
                    grDanhSach.ItemsSource = grdData;
                else
                    grDanhSach.ItemsSource = null;
                lblTong.Content = grdData.Rows.Count;
            }
            else
            {

            }
        }

        private void BuildGridPhuongXa()
        {
        }

        private void tvwTree_Unchecked(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            BuildGrid();
            loadWidthColumn();
        }

        private void grDanhSach_Loaded(object sender, RoutedEventArgs e)
        {
            loadWidthColumn();
        }

        /// <summary>
        /// Load lai du lieu khi co thay doi tu form chi tiet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void userControl_OnSavingCompleted(object sender, EventArgs e)
        {
            //LoadDuLieu();
            getAllData();
            getGridData();
            BuildGrid();
            loadWidthColumn();
        }

        private void loadWidthColumn()
        {
            if (grDanhSach.Items.Count > 0)
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
                        grDanhSach.Columns[idx].Width = new Telerik.Windows.Controls.GridViewLength(width, unit);
                        grDanhSach.Columns[idx].IsReadOnly = true;
                        grDanhSach.Columns[idx].Header = LLanguage.SearchResourceByKey(item.LanguageKey);
                    }
                    else
                        grDanhSach.Columns[idx].IsVisible = false;
                    idx = idx + 1;
                }
                columnsWidthLoad = true;
            }
        }

        private void cmbQuanHuyen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BuildGrid();
            loadWidthColumn();
        }

        private void cmbLoaiDiaBan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoaiDiaBan = lstSourceLoaiDiaBan.ElementAt(cmbLoaiDiaBan.SelectedIndex).KeywordStrings.First();
            BuildGrid();
            loadWidthColumn();
        }

        private void tvwTree_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            RadTreeViewItem item = (RadTreeViewItem)tvwTree.SelectedItems.First();
            string nodeCode = item.Tag.ToString();
            string[] nodeInfo = item.Uid.ToString().Split('#');
            string nodeType = nodeInfo[0];
            string nodeName = item.Header.ToString();
            int nodeId = int.Parse(nodeInfo[1]);

            if (nodeType.Equals(DatabaseConstant.DanhSachLoaiDiaBan.TINH_THANHPHO.getValue()))            
            {
                IdTinhTP = nodeId;
                MaTinhTP = nodeCode;
                TenTinhTP = nodeName;
                LoaiDiaBan = DatabaseConstant.DanhSachLoaiDiaBan.HUYEN_QUAN.getValue();
                cmbLoaiDiaBan.SelectedIndex = lstSourceLoaiDiaBan.IndexOf(lstSourceLoaiDiaBan.FirstOrDefault(entry => entry.KeywordStrings.First().Equals(DatabaseConstant.DanhSachLoaiDiaBan.HUYEN_QUAN.getValue())));

                getGridData();

                // khởi tạo combobox
                AutoComboBox auto = new AutoComboBox();
                // Khởi tạo điều kiện gọi danh mục
                lstSourceQuanHuyen = null;
                lstSourceQuanHuyen = new List<AutoCompleteEntry>();
                lstSourceQuanHuyen_Select = new List<AutoCompleteEntry>();
                lstSourcePhuongXa = null;
                lstSourcePhuongXa = new List<AutoCompleteEntry>();
                lstSourcePhuongXa_Select = new List<AutoCompleteEntry>();

                foreach (DataRow row in dtDetail.Rows)
                {
                    if (MaTinhTP.Equals(row["KEY"].ToString()) &&
                        DatabaseConstant.DanhSachLoaiDiaBan.HUYEN_QUAN.getValue().Equals(row["LOAI_DBAN"].ToString()))
                    {
                        lstSourceQuanHuyen_Select.Add(new AutoCompleteEntry(row[4].ToString(), row[3].ToString(), row[2].ToString(), row[1].ToString(), row["KEY"].ToString()));
                    }

                    else if (MaTinhTP.Equals(row["KEY"].ToString()) &&
                        DatabaseConstant.DanhSachLoaiDiaBan.XA_PHUONG.getValue().Equals(row["LOAI_DBAN"].ToString()))
                    {
                        lstSourcePhuongXa_Select.Add(new AutoCompleteEntry(row[4].ToString(), row[3].ToString(), row[2].ToString(), row[1].ToString(), row["KEY"].ToString()));
                    }
                }

                if (lstSourceQuanHuyen_Select.Count > 0)
                {
                    lstSourceQuanHuyen.Add(new AutoCompleteEntry("[ALL]", "NULL", "NULL", "NULL", MaTinhTP));
                    foreach (AutoCompleteEntry row in lstSourceQuanHuyen_Select)
                    {
                        lstSourceQuanHuyen.Add(row);
                    }

                    cmbQuanHuyen.Items.Clear();
                    auto.GenAutoComboBox(ref lstSourceQuanHuyen, ref cmbQuanHuyen, null);
                    cmbQuanHuyen.SelectedIndex = lstSourceQuanHuyen.IndexOf(lstSourceQuanHuyen.FirstOrDefault(entry => entry.KeywordStrings.First().Equals("NULL")));
                }
                if (lstSourcePhuongXa_Select.Count > 0)
                {
                    lstSourcePhuongXa.Add(new AutoCompleteEntry("[ALL]", "NULL", "NULL", "NULL", MaTinhTP));
                    foreach (AutoCompleteEntry row in lstSourcePhuongXa_Select)
                    {
                        lstSourcePhuongXa.Add(row);
                    }

                    //cmbPhuongXa.Items.Clear();
                    //auto.GenAutoComboBox(ref lstSourcePhuongXa, ref cmbPhuongXa, null);
                }


                // Tên tỉnh thành
                lblTenTinhTP.Content = TenTinhTP; 

                UpdateLayout();
                BuildGrid();
                loadWidthColumn();
            }
        }

        /// <summary>
        /// Click tree item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvwTree_ItemClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            RadTreeViewItem item = (RadTreeViewItem)tvwTree.SelectedItems.First();
            string nodeCode = item.Tag.ToString();
            string[] nodeInfo = item.Uid.ToString().Split('#');
            string nodeType = nodeInfo[0];
            string nodeName = item.Header.ToString();
            int nodeId = int.Parse(nodeInfo[1]);

            if (nodeType.Equals(DatabaseConstant.DanhSachLoaiDiaBan.TINH_THANHPHO.getValue()))
            {
                IdTinhTP = nodeId;
                MaTinhTP = nodeCode;
                TenTinhTP = nodeName;
                LoaiDiaBan = DatabaseConstant.DanhSachLoaiDiaBan.HUYEN_QUAN.getValue();
                cmbLoaiDiaBan.SelectedIndex = lstSourceLoaiDiaBan.IndexOf(lstSourceLoaiDiaBan.FirstOrDefault(entry => entry.KeywordStrings.First().Equals(DatabaseConstant.DanhSachLoaiDiaBan.HUYEN_QUAN.getValue())));

                getGridData();

                // khởi tạo combobox
                AutoComboBox auto = new AutoComboBox();
                // Khởi tạo điều kiện gọi danh mục
                lstSourceQuanHuyen = null;
                lstSourceQuanHuyen = new List<AutoCompleteEntry>();
                List<AutoCompleteEntry> lstSourceQuanHuyen_Select = new List<AutoCompleteEntry>();
                lstSourcePhuongXa = null;
                lstSourcePhuongXa = new List<AutoCompleteEntry>();
                List<AutoCompleteEntry> lstSourcePhuongXa_Select = new List<AutoCompleteEntry>();

                foreach (DataRow row in dtDetail.Rows)
                {
                    if (MaTinhTP.Equals(row["KEY"].ToString()) &&
                        DatabaseConstant.DanhSachLoaiDiaBan.HUYEN_QUAN.getValue().Equals(row["LOAI_DBAN"].ToString()))
                    {
                        lstSourceQuanHuyen_Select.Add(new AutoCompleteEntry(row[4].ToString(), row[3].ToString(), row[2].ToString(), row[1].ToString(), row["KEY"].ToString()));
                    }

                    else if (MaTinhTP.Equals(row["KEY"].ToString()) &&
                        DatabaseConstant.DanhSachLoaiDiaBan.XA_PHUONG.getValue().Equals(row["LOAI_DBAN"].ToString()))
                    {
                        lstSourcePhuongXa_Select.Add(new AutoCompleteEntry(row[4].ToString(), row[3].ToString(), row[2].ToString(), row[1].ToString(), row["KEY"].ToString()));
                    }
                }

                if (lstSourceQuanHuyen_Select.Count > 0)
                {
                    lstSourceQuanHuyen.Add(new AutoCompleteEntry("[ALL]", "NULL", "NULL", "NULL", MaTinhTP));
                    foreach (AutoCompleteEntry row in lstSourceQuanHuyen_Select)
                    {
                        lstSourceQuanHuyen.Add(row);
                    }

                    cmbQuanHuyen.Items.Clear();
                    auto.GenAutoComboBox(ref lstSourceQuanHuyen, ref cmbQuanHuyen, null);
                    cmbQuanHuyen.SelectedIndex = lstSourceQuanHuyen.IndexOf(lstSourceQuanHuyen.FirstOrDefault(entry => entry.KeywordStrings.First().Equals("NULL")));
                }
                if (lstSourcePhuongXa_Select.Count > 0)
                {
                    lstSourcePhuongXa.Add(new AutoCompleteEntry("[ALL]", "NULL", "NULL", "NULL", MaTinhTP));
                    foreach (AutoCompleteEntry row in lstSourcePhuongXa_Select)
                    {
                        lstSourcePhuongXa.Add(row);
                    }

                    //cmbPhuongXa.Items.Clear();
                    //auto.GenAutoComboBox(ref lstSourcePhuongXa, ref cmbPhuongXa, null);
                }

                // Tên tỉnh thành
                lblTenTinhTP.Content = TenTinhTP; 

                UpdateLayout();
                BuildGrid();
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
                RadTreeViewItem item = (RadTreeViewItem)tvwTree.SelectedItems.First();
                string nodeCode = item.Tag.ToString();
                string[] nodeInfo = item.Uid.ToString().Split('#');
                string nodeType = nodeInfo[0];
                string nodeName = item.Header.ToString();
                int nodeId = int.Parse(nodeInfo[1]);

                if (nodeType.Equals(DatabaseConstant.DanhSachLoaiDiaBan.TINH_THANHPHO.getValue()))
                {
                    IdTinhTP = nodeId;
                    MaTinhTP = nodeCode;
                    TenTinhTP = nodeName;
                    LoaiDiaBan = DatabaseConstant.DanhSachLoaiDiaBan.HUYEN_QUAN.getValue();
                    cmbLoaiDiaBan.SelectedIndex = lstSourceLoaiDiaBan.IndexOf(lstSourceLoaiDiaBan.FirstOrDefault(entry => entry.KeywordStrings.First().Equals(DatabaseConstant.DanhSachLoaiDiaBan.HUYEN_QUAN.getValue())));

                    getGridData();

                    // khởi tạo combobox
                    AutoComboBox auto = new AutoComboBox();
                    // Khởi tạo điều kiện gọi danh mục
                    lstSourceQuanHuyen = null;
                    lstSourceQuanHuyen = new List<AutoCompleteEntry>();
                    List<AutoCompleteEntry> lstSourceQuanHuyen_Select = new List<AutoCompleteEntry>();
                    lstSourcePhuongXa = null;
                    lstSourcePhuongXa = new List<AutoCompleteEntry>();
                    List<AutoCompleteEntry> lstSourcePhuongXa_Select = new List<AutoCompleteEntry>();

                    foreach (DataRow row in dtDetail.Rows)
                    {
                        if (MaTinhTP.Equals(row["KEY"].ToString()) &&
                            DatabaseConstant.DanhSachLoaiDiaBan.HUYEN_QUAN.getValue().Equals(row["LOAI_DBAN"].ToString()))
                        {
                            lstSourceQuanHuyen_Select.Add(new AutoCompleteEntry(row[4].ToString(), row[3].ToString(), row[2].ToString(), row[1].ToString(), row["KEY"].ToString()));
                        }

                        else if (MaTinhTP.Equals(row["KEY"].ToString()) &&
                            DatabaseConstant.DanhSachLoaiDiaBan.XA_PHUONG.getValue().Equals(row["LOAI_DBAN"].ToString()))
                        {
                            lstSourcePhuongXa_Select.Add(new AutoCompleteEntry(row[4].ToString(), row[3].ToString(), row[2].ToString(), row[1].ToString(), row["KEY"].ToString()));
                        }
                    }

                    if (lstSourceQuanHuyen_Select.Count > 0)
                    {
                        lstSourceQuanHuyen.Add(new AutoCompleteEntry("[ALL]", "NULL", "NULL", "NULL", MaTinhTP));
                        foreach (AutoCompleteEntry row in lstSourceQuanHuyen_Select)
                        {
                            lstSourceQuanHuyen.Add(row);
                        }

                        cmbQuanHuyen.Items.Clear();
                        auto.GenAutoComboBox(ref lstSourceQuanHuyen, ref cmbQuanHuyen, null);
                        cmbQuanHuyen.SelectedIndex = lstSourceQuanHuyen.IndexOf(lstSourceQuanHuyen.FirstOrDefault(entry => entry.KeywordStrings.First().Equals("NULL")));
                    }
                    if (lstSourcePhuongXa_Select.Count > 0)
                    {
                        lstSourcePhuongXa.Add(new AutoCompleteEntry("[ALL]", "NULL", "NULL", "NULL", MaTinhTP));
                        foreach (AutoCompleteEntry row in lstSourcePhuongXa_Select)
                        {
                            lstSourcePhuongXa.Add(row);
                        }

                        //cmbPhuongXa.Items.Clear();
                        //auto.GenAutoComboBox(ref lstSourcePhuongXa, ref cmbPhuongXa, null);
                    }

                    // Tên tỉnh thành
                    lblTenTinhTP.Content = TenTinhTP; 

                    UpdateLayout();
                    BuildGrid();
                    loadWidthColumn();
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
        /// Trợ giúp
        /// </summary>
        private void onHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }

        /// <summary>
        /// Đóng form
        /// </summary>
        private void onClose()
        {
            Release();
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        /// <summary>
        /// Sự kiện unload cho form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Release();
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        /// <summary>
        /// Hàm xử lý giải phóng lock dữ liệu cho form
        /// </summary>
        private void Release()
        {
            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool ret = process.UnlockDataFromFunctionByUser(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_DIA_BAN);
        }

        #endregion

        #region Xu ly nghiep vu

        /// <summary>
        /// Sự kiện double click để sửa dữ liệu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grDanhSach_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            beforeView();
        }

        /// <summary>
        /// Trước khi xem
        /// </summary>
        private void beforeView()
        {
            List<DataRow> listDataRow = getListSeletedDataRow();
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
            List<DataRow> listDataRow = getListSeletedDataRow();

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
                    int id = int.Parse(listDataRow.First()["id"].ToString());
                    // Kiểm tra hợp lệ thao tác trên client (nếu có)
                    // Nếu không cho phép sửa sau duyệt
                    if (listDataRow.First()["tthai_nvu"].ToString().Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    {
                        LMessage.ShowMessage("M.DungChung.DaDuyetKhongDuocSua", LMessage.MessageBoxType.Warning);
                        return;
                    }

                    // Yêu cầu lock bản ghi cần xử lý
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);

                    bool ret = process.LockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_DM_DIA_BAN,
                        DatabaseConstant.Table.DM_DIA_BAN,
                        DatabaseConstant.Action.SUA,
                        listLockId);

                    // Nếu lock thành công >> cho phép xử lý
                    if (ret)
                    {
                        onModify(id);
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
            List<DataRow> listDataRow = getListSeletedDataRow();

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
                    foreach (DataRow dr in listDataRow)
                    {
                        int id = int.Parse(dr["id"].ToString());
                        listId.Add(id);
                    }

                    // Cảnh báo người dùng
                    MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);
                    if (ret == MessageBoxResult.Yes)
                    {

                        // Yêu cầu lock bản ghi cần xử lý
                        UtilitiesProcess process = new UtilitiesProcess();
                        List<int> listLockedId = new List<int>();

                        bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                            DatabaseConstant.Function.DC_DM_DIA_BAN,
                            DatabaseConstant.Table.DM_DIA_BAN,
                            DatabaseConstant.Action.XOA,
                            listId);

                        // Nếu lock thành công >> cho phép xử lý
                        if (retLockData)
                        {
                            onDelete(listId);
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
        /// Trước khi duyệt
        /// </summary>
        private void beforeApprove()
        {
            List<DataRow> listDataRow = getListSeletedDataRow();

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
                    foreach (DataRow dr in listDataRow)
                    {
                        int id = int.Parse(dr["id"].ToString());
                        listId.Add(id);
                    }

                    // Cảnh báo người dùng
                    MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question);
                    if (ret == MessageBoxResult.Yes)
                    {

                        // Yêu cầu lock bản ghi cần xử lý
                        UtilitiesProcess process = new UtilitiesProcess();
                        List<int> listLockedId = new List<int>();

                        bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                            DatabaseConstant.Function.DC_DM_DIA_BAN,
                            DatabaseConstant.Table.DM_DIA_BAN,
                            DatabaseConstant.Action.DUYET,
                            listId);

                        // Nếu lock thành công >> cho phép xử lý
                        if (retLockData)
                        {
                            onApprove(listId);
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
        /// Trước khi thoái duyệt
        /// </summary>
        private void beforeCancel()
        {
            List<DataRow> listDataRow = getListSeletedDataRow();

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
                    foreach (DataRow dr in listDataRow)
                    {
                        int id = int.Parse(dr["id"].ToString());
                        listId.Add(id);
                    }

                    // Cảnh báo người dùng
                    MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question);
                    if (ret == MessageBoxResult.Yes)
                    {

                        // Yêu cầu lock bản ghi cần xử lý
                        UtilitiesProcess process = new UtilitiesProcess();
                        List<int> listLockedId = new List<int>();

                        bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                            DatabaseConstant.Function.DC_DM_DIA_BAN,
                            DatabaseConstant.Table.DM_DIA_BAN,
                            DatabaseConstant.Action.THOAI_DUYET,
                            listId);

                        // Nếu lock thành công >> cho phép xử lý
                        if (retLockData)
                        {
                            onCancel(listId);
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
        /// Trước khi từ chối
        /// </summary>
        private void beforeRefuse()
        {
            List<DataRow> listDataRow = getListSeletedDataRow();

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
                    foreach (DataRow dr in listDataRow)
                    {
                        int id = int.Parse(dr["id"].ToString());
                        listId.Add(id);
                    }

                    // Cảnh báo người dùng
                    MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question);
                    if (ret == MessageBoxResult.Yes)
                    {

                        // Yêu cầu lock bản ghi cần xử lý
                        UtilitiesProcess process = new UtilitiesProcess();
                        List<int> listLockedId = new List<int>();

                        bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                            DatabaseConstant.Function.DC_DM_DIA_BAN,
                            DatabaseConstant.Table.DM_DIA_BAN,
                            DatabaseConstant.Action.TU_CHOI_DUYET,
                            listId);

                        // Nếu lock thành công >> cho phép xử lý
                        if (retLockData)
                        {
                            onRefuse(listId);
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
        private void onView(int id)
        {
            ucDiaBanCT userControl = new ucDiaBanCT();
            userControl.OnSavingCompleted += new EventHandler(userControl_OnSavingCompleted);
            userControl.Id = id;
            userControl.FormCase = "XEM";
            userControl.Action = DatabaseConstant.Action.XEM;
            LoadDuLieuCT dlgLoadDuLieuCT = new LoadDuLieuCT(userControl.beforeModifyFromList);
            dlgLoadDuLieuCT();

            string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.Function);
            Window frm = new Window();
            frm.Title = tittle;
            frm.Content = userControl;
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frm.ShowDialog();
        }

        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        private void onAddNew()
        {
            string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.Function);
            Window frm = new Window();
            frm.Title = tittle;
            ucDiaBanCT uc = new ucDiaBanCT();
            uc.FormCase = "MANAGE";
            frm.Content = uc;
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frm.ShowDialog();
        }

        /// <summary>
        /// Sửa dữ liệu
        /// </summary>
        private void onModify(int id)
        {
            ucDiaBanCT userControl = new ucDiaBanCT();
            userControl.OnSavingCompleted += new EventHandler(userControl_OnSavingCompleted);
            userControl.Id = id;
            userControl.FormCase = "MANAGE";
            userControl.Action = DatabaseConstant.Action.SUA;
            LoadDuLieuCT dlgLoadDuLieuCT = new LoadDuLieuCT(userControl.beforeModifyFromList);
            dlgLoadDuLieuCT();

            string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.Function);
            Window frm = new Window();
            frm.Title = tittle;
            frm.Content = userControl;
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frm.ShowDialog();
        }

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        private void onDelete(List<int> listId)
        {
            DanhMucProcess danhmucProcess = new DanhMucProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = danhmucProcess.XoaDiaBan(listId.ToArray(), ref listClientResponseDetail);

                afterDelete(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_DIA_BAN,
                    DatabaseConstant.Table.DM_DIA_BAN,
                    DatabaseConstant.Action.XOA,
                    listId);
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Duyệt dữ liệu
        /// </summary>
        private void onApprove(List<int> listId)
        {
            DanhMucProcess danhmucProcess = new DanhMucProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = danhmucProcess.DuyetDiaBan(listId.ToArray(), ref listClientResponseDetail);

                afterApprove(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_DIA_BAN,
                    DatabaseConstant.Table.DM_DIA_BAN,
                    DatabaseConstant.Action.DUYET,
                    listId);

                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Thoái duyệt dữ liệu
        /// </summary>
        private void onCancel(List<int> listId)
        {
            DanhMucProcess danhmucProcess = new DanhMucProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = danhmucProcess.ThoaiDuyetDiaBan(listId.ToArray(), ref listClientResponseDetail);

                afterCancel(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_DIA_BAN,
                    DatabaseConstant.Table.DM_DIA_BAN,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listId);

                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Từ chối duyệt dữ liệu
        /// </summary>
        private void onRefuse(List<int> listId)
        {
            DanhMucProcess danhmucProcess = new DanhMucProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = danhmucProcess.TuChoiCum(listId.ToArray(), ref listClientResponseDetail);

                afterRefuse(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_DIA_BAN,
                    DatabaseConstant.Table.DM_DIA_BAN,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listId);

                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Sau khi xem
        /// </summary>
        /// <param name="ret"></param>
        private void afterView()
        {
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
            List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
                LoadDuLieu();
            }
            else
            {
                //LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                LoadDuLieu();
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_DIA_BAN,
                DatabaseConstant.Table.DM_DIA_BAN,
                DatabaseConstant.Action.XOA,
                listId);
        }

        /// <summary>
        /// Sau khi duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void afterApprove(bool ret,
            List<int> listId,
            List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);
                LoadDuLieu();
            }
            else
            {
                //LMessage.ShowMessage("M.DungChung.DuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                LoadDuLieu();
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_DIA_BAN,
                DatabaseConstant.Table.DM_DIA_BAN,
                DatabaseConstant.Action.DUYET,
                listId);
        }

        /// <summary>
        /// Sau khi thoái duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void afterCancel(bool ret,
            List<int> listId,
            List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);
                LoadDuLieu();
            }
            else
            {
                //LMessage.ShowMessage("M.DungChung.ThoaiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                LoadDuLieu();
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_DIA_BAN,
                DatabaseConstant.Table.DM_DIA_BAN,
                DatabaseConstant.Action.THOAI_DUYET,
                listId);
        }

        /// <summary>
        /// Sau khi từ chối
        /// </summary>
        /// <param name="ret"></param>
        private void afterRefuse(bool ret,
            List<int> listId,
            List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);
                LoadDuLieu();
            }
            else
            {
                //LMessage.ShowMessage("M.DungChung.TuChoiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                LoadDuLieu();
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_DIA_BAN,
                DatabaseConstant.Table.DM_DIA_BAN,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                listId);
        }

        /// <summary>
        /// Lấy danh sách id được chọn
        /// </summary>
        /// <returns></returns>
        private List<DataRow> getListSeletedDataRow()
        {
            List<DataRow> listDataRow = new List<DataRow>();
            if (grDanhSach.SelectedItems.Count <= 0)
            {
                return listDataRow;
            }
            else
            {
                for (int i = 0; i < grDanhSach.SelectedItems.Count; i++)
                {
                    DataRow dr = (DataRow)grDanhSach.SelectedItems[i];
                    listDataRow.Add(dr);
                }
                return listDataRow;
            }
        }

        #endregion
    }
}
