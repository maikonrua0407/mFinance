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
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using PresentationWPF.CustomControl;
using Presentation.Process;
using Telerik.Windows.Controls;
using Presentation.Process.Common;
using Presentation.Process.TinDungTDServiceRef;

namespace PresentationWPF.TinDungTD.HDTD
{
    /// <summary>
    /// Interaction logic for ucHopDongTinDungCaNhanDS.xaml
    /// </summary>
    public partial class ucHopDongTinDungCaNhanDS : UserControl
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
        private List<string> lstCheckedKhuVuc = new List<string>();
        private List<string> lstCheckedSanPham = new List<string>();
        private List<AutoCompleteEntry> lstLoaiGiayTo = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstSourceLoaiSPham = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstSourceSanPham = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstSourceMucDichVay = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstSourceDViTinh = new List<AutoCompleteEntry>();
        private DatabaseConstant.Module Module = DatabaseConstant.Module.TDTD;
        private DatabaseConstant.Function Function = DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN;
        private TD_HDTD_TD objHDTDTD = new TD_HDTD_TD();
        DataTable dtTreeDLy;

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
        public ucHopDongTinDungCaNhanDS()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDungTD;component/HopDong/ucHopDongTinDungCaNhanDS.xaml", ref Toolbar, ref mnuGrid);
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
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.LOAI_GIAY_TO));
            string Dislay = LLanguage.SearchResourceByKey("U.DungChung.TatCa");
            lstLoaiGiayTo.Add(new AutoCompleteEntry(Dislay, "%", "0"));
            KhoiTaoGiaTriComboBox(ref lstLoaiGiayTo, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), cmbLoaiGiayTo, lstDieuKien);
            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.DVI_TINH_KY_HAN));
            lstSourceDViTinh.Add(new AutoCompleteEntry(Dislay, "%", "0"));
            KhoiTaoGiaTriComboBox(ref lstSourceDViTinh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), cmbThoiHanVayTu, lstDieuKien);
            KhoiTaoGiaTriComboBox(ref lstSourceDViTinh, null, cmbThoiHanVayDen, lstDieuKien);
            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.MUC_DICH_VAY_VON));
            lstSourceMucDichVay.Add(new AutoCompleteEntry(Dislay, "%", "0"));
            KhoiTaoGiaTriComboBox(ref lstSourceMucDichVay, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), cmbMucDichVay, lstDieuKien);


            LoadComboboxSanPham();
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
            ThemMoi();
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
            Duyet();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TuChoiDuyet();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ThoaiDuyet();
        }

        private void ViewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ViewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Xoa();
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
                ThemMoi();
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
        /// Sự kiện shortcutkey
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ThemMoi();
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
            PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(raddgrHDTDDS, txtTimKiemNhanh.Text);
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
            if (raddgrHDTDDS != null && raddgrHDTDDS.ItemsSource != null)
            {
                DataView dt = ((DataView)raddgrHDTDDS.ItemsSource);
                if (dt != null)
                {
                    radPage.PageSize = (int)nudPageSize.Value;
                    raddgrHDTDDS.DataContext = dt;
                }
            }
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(raddgrHDTDDS);
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

        private void raddgrHDTDDS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Xem();
        }

        private void ClearForm()
        {
            teldtNgayLapHDDen.Value = null;
            teldtNgayLapHDTu.Value = null;
            txtSoTienDuyetVayDen.Value = null;
            txtSoTienDuyetVayTu.Value = null;
            txtSoTienXinVayDen.Value = null;
            txtSoTienXinVayTu.Value = null;
            txtThoiHanVayTu.Value = null;
            txtThoiHanVayDen.Value = null;
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

        private void LoadComboboxSanPham()
        {
            RadTreeViewItem treeItemRoot = (RadTreeViewItem)tvwKhuVuc.SelectedItem;
            string strLstDonVi = "";

            if (treeItemRoot.IsNullOrEmpty())
                strLstDonVi = "''" + ClientInformation.MaDonVi + "''";
            else
            {
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
            }
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(strLstDonVi);
            lstDieuKien.Add("0");

            string Dislay = LLanguage.SearchResourceByKey("U.DungChung.TatCa");

            lstSourceSanPham.Clear();
            lstSourceSanPham = new List<AutoCompleteEntry>();

            cmbSanPham.Items.Clear();
            lstSourceSanPham.Add(new AutoCompleteEntry(Dislay, "%", "0"));
            KhoiTaoGiaTriComboBox(ref lstSourceSanPham, "COMBOBOX_SAN_PHAM_TDTD", cmbSanPham, lstDieuKien);
        }
        #endregion

        #region Xử lý nghiệp vụ
        void LoadData()
        {
            Cursor = Cursors.Wait;
            AutoCompleteEntry auSanPham = lstSourceSanPham.ElementAt(cmbSanPham.SelectedIndex);
            AutoCompleteEntry auMucDichVay = lstSourceMucDichVay.ElementAt(cmbMucDichVay.SelectedIndex);
            AutoCompleteEntry auTGianDVTTu = lstSourceDViTinh.ElementAt(cmbThoiHanVayTu.SelectedIndex);
            AutoCompleteEntry auTGianDVTDen = lstSourceDViTinh.ElementAt(cmbThoiHanVayDen.SelectedIndex);
            string TThaiNVu = ucTrangThaiNVu.GetItemsSelected();
            string SoHDTD = txtSoHDTD.Text;
            string NgayHopDongTu = teldtNgayLapHDTu.Value != null ? LDateTime.DateToString((DateTime)teldtNgayLapHDTu.Value, ApplicationConstant.defaultDateTimeFormat) : "";
            string NgayHopDongDen = teldtNgayLapHDDen.Value != null ? LDateTime.DateToString((DateTime)teldtNgayLapHDDen.Value, ApplicationConstant.defaultDateTimeFormat) : "";
            string ThoiGianVayTu = txtThoiHanVayTu.Value.GetValueOrDefault().ToString();
            string ThoiGianVayDen = txtThoiHanVayDen.Value.GetValueOrDefault().ToString();
            string SoTienXinVayTu = txtSoTienXinVayTu.Value.GetValueOrDefault().ToString();
            string SoTienXinVayDen = txtSoTienXinVayDen.Value.GetValueOrDefault().ToString();
            string SoTienDuyetVayTu = txtSoTienDuyetVayTu.Value.GetValueOrDefault().ToString();
            string SoTienDuyetVayDen = txtSoTienDuyetVayDen.Value.GetValueOrDefault().ToString();
            string MaKhachHang = txtMaKhachHang.Text;
            string TenKhachHang = txtTenKhachHang.Text;
            string LoaiGiayTo = lstLoaiGiayTo.ElementAt(cmbLoaiGiayTo.SelectedIndex).KeywordStrings.First();
            string SoGiayTo = txtSoGiayTo.Text;
            string DienThoai = txtDienThoai.Text;
            string Email = txtEmail.Text;
            string MaSanPham = auSanPham.KeywordStrings.FirstOrDefault();
            string MucDichVay = auMucDichVay.KeywordStrings.FirstOrDefault();
            string DonViTinhTu = auTGianDVTTu.KeywordStrings.FirstOrDefault();
            string DonViTinhDen = auTGianDVTDen.KeywordStrings.FirstOrDefault();
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
            LDatatable.AddParameter(ref dt, "@INP_SO_HDTD", "string", SoHDTD);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_HD_TU", "string", NgayHopDongTu);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_HD_DEN", "string", NgayHopDongDen);
            LDatatable.AddParameter(ref dt, "@INP_SPHAM", "string", MaSanPham);
            LDatatable.AddParameter(ref dt, "@INP_MUC_DICH_VAY", "string", MucDichVay);
            LDatatable.AddParameter(ref dt, "@INP_TGIAN_VAY_TU", "string", ThoiGianVayTu);
            LDatatable.AddParameter(ref dt, "@INP_TGIAN_VAY_DEN", "string", ThoiGianVayDen);
            LDatatable.AddParameter(ref dt, "@INP_TGIAN_DVTINH_TU", "string", DonViTinhTu);
            LDatatable.AddParameter(ref dt, "@INP_TGIAN_DVTINH_DEN", "string", DonViTinhDen);
            LDatatable.AddParameter(ref dt, "@INP_MUC_XIN_VAY_TU", "string", SoTienXinVayTu);
            LDatatable.AddParameter(ref dt, "@INP_MUC_XIN_VAY_DEN", "string", SoTienXinVayDen);
            LDatatable.AddParameter(ref dt, "@INP_MUC_DUYET_VAY_TU", "string", SoTienDuyetVayTu);
            LDatatable.AddParameter(ref dt, "@INP_MUC_DUYET_VAY_DEN", "string", SoTienDuyetVayDen);

            LDatatable.AddParameter(ref dt, "@INP_MA_KHANG", "string", MaKhachHang);
            LDatatable.AddParameter(ref dt, "@INP_TEN_KHANG", "string", TenKhachHang);
            LDatatable.AddParameter(ref dt, "@INP_MA_GTO", "string", LoaiGiayTo);
            LDatatable.AddParameter(ref dt, "@INP_SO_GTO", "string", SoGiayTo);
            LDatatable.AddParameter(ref dt, "@INP_DIEN_THOAI", "string", DienThoai);
            LDatatable.AddParameter(ref dt, "@INP_EMAIL", "string", Email);
            LDatatable.AddParameter(ref dt, "@INP_KHUVUC", "string", ListKVuc);
            LDatatable.AddParameter(ref dt, "@INP_USERNAME", "string", ClientInformation.TenDangNhap);
            LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "string", ClientInformation.MaDonViQuanLy);
            LDatatable.AddParameter(ref dt, "@START", "String", StartRow.ToString());
            LDatatable.AddParameter(ref dt, "@END", "String", EndRow.ToString());
            DataSet ds = new TinDungTDProcess().HopDongTinDungCaNhanDanhSach(dt);
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                    dr["TEN_TTHAI_NVU"] = BusinessConstant.layNgonNguNghiepVu(dr["TTHAI_NVU"].ToString());

                DataTable serverDataTable = ds.Tables[0];
                int totalRecord = Int32.Parse(ds.Tables[1].Rows[0][0].ToString());
                DataTable clientDataTable = CommonFunction.BuildClientDataTable(serverDataTable, CurrentPagging, PaggingSize, totalRecord);

                //raddgrHDTDDS.ItemsSource = ds.Tables["DANH_SACH"].DefaultView;
                raddgrHDTDDS.ItemsSource = clientDataTable.DefaultView;
                if (!LObject.IsNullOrEmpty(raddgrHDTDDS.SelectedItems))
                    raddgrHDTDDS.SelectedItems.Clear();
                lblSumKhachHang.Content = totalRecord.ToString();
            }
            Cursor = Cursors.Arrow;
        }

        void LoadDataPhanTrang()
        {
            Cursor = Cursors.Wait;
            AutoCompleteEntry auSanPham = lstSourceSanPham.ElementAt(cmbSanPham.SelectedIndex);
            AutoCompleteEntry auMucDichVay = lstSourceMucDichVay.ElementAt(cmbMucDichVay.SelectedIndex);
            AutoCompleteEntry auTGianDVTTu = lstSourceDViTinh.ElementAt(cmbThoiHanVayTu.SelectedIndex);
            AutoCompleteEntry auTGianDVTDen = lstSourceDViTinh.ElementAt(cmbThoiHanVayDen.SelectedIndex);
            string TThaiNVu = ucTrangThaiNVu.GetItemsSelected();
            string SoHDTD = txtSoHDTD.Text;
            string NgayHopDongTu = teldtNgayLapHDTu.Value != null ? LDateTime.DateToString((DateTime)teldtNgayLapHDTu.Value, ApplicationConstant.defaultDateTimeFormat) : "";
            string NgayHopDongDen = teldtNgayLapHDDen.Value != null ? LDateTime.DateToString((DateTime)teldtNgayLapHDDen.Value, ApplicationConstant.defaultDateTimeFormat) : "";
            string ThoiGianVayTu = txtThoiHanVayTu.Value.GetValueOrDefault().ToString();
            string ThoiGianVayDen = txtThoiHanVayDen.Value.GetValueOrDefault().ToString();
            string SoTienXinVayTu = txtSoTienXinVayTu.Value.GetValueOrDefault().ToString();
            string SoTienXinVayDen = txtSoTienXinVayDen.Value.GetValueOrDefault().ToString();
            string SoTienDuyetVayTu = txtSoTienDuyetVayTu.Value.GetValueOrDefault().ToString();
            string SoTienDuyetVayDen = txtSoTienDuyetVayDen.Value.GetValueOrDefault().ToString();
            string MaKhachHang = txtMaKhachHang.Text;
            string TenKhachHang = txtTenKhachHang.Text;
            string LoaiGiayTo = lstLoaiGiayTo.ElementAt(cmbLoaiGiayTo.SelectedIndex).KeywordStrings.First();
            string SoGiayTo = txtSoGiayTo.Text;
            string DienThoai = txtDienThoai.Text;
            string Email = txtEmail.Text;
            string MaSanPham = auSanPham.KeywordStrings.FirstOrDefault();
            string MucDichVay = auMucDichVay.KeywordStrings.FirstOrDefault();
            string DonViTinhTu = auTGianDVTTu.KeywordStrings.FirstOrDefault();
            string DonViTinhDen = auTGianDVTDen.KeywordStrings.FirstOrDefault();
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
            LDatatable.AddParameter(ref dt, "@INP_SO_HDTD", "string", SoHDTD);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_HD_TU", "string", NgayHopDongTu);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_HD_DEN", "string", NgayHopDongDen);
            LDatatable.AddParameter(ref dt, "@INP_SPHAM", "string", MaSanPham);
            LDatatable.AddParameter(ref dt, "@INP_MUC_DICH_VAY", "string", MucDichVay);
            LDatatable.AddParameter(ref dt, "@INP_TGIAN_VAY_TU", "string", ThoiGianVayTu);
            LDatatable.AddParameter(ref dt, "@INP_TGIAN_VAY_DEN", "string", ThoiGianVayDen);
            LDatatable.AddParameter(ref dt, "@INP_TGIAN_DVTINH_TU", "string", DonViTinhTu);
            LDatatable.AddParameter(ref dt, "@INP_TGIAN_DVTINH_DEN", "string", DonViTinhDen);
            LDatatable.AddParameter(ref dt, "@INP_MUC_XIN_VAY_TU", "string", SoTienXinVayTu);
            LDatatable.AddParameter(ref dt, "@INP_MUC_XIN_VAY_DEN", "string", SoTienXinVayDen);
            LDatatable.AddParameter(ref dt, "@INP_MUC_DUYET_VAY_TU", "string", SoTienDuyetVayTu);
            LDatatable.AddParameter(ref dt, "@INP_MUC_DUYET_VAY_DEN", "string", SoTienDuyetVayDen);

            LDatatable.AddParameter(ref dt, "@INP_MA_KHANG", "string", MaKhachHang);
            LDatatable.AddParameter(ref dt, "@INP_TEN_KHANG", "string", TenKhachHang);
            LDatatable.AddParameter(ref dt, "@INP_MA_GTO", "string", LoaiGiayTo);
            LDatatable.AddParameter(ref dt, "@INP_SO_GTO", "string", SoGiayTo);
            LDatatable.AddParameter(ref dt, "@INP_DIEN_THOAI", "string", DienThoai);
            LDatatable.AddParameter(ref dt, "@INP_EMAIL", "string", Email);
            LDatatable.AddParameter(ref dt, "@INP_KHUVUC", "string", ListKVuc);
            LDatatable.AddParameter(ref dt, "@INP_USERNAME", "string", ClientInformation.TenDangNhap);
            LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "string", ClientInformation.MaDonViQuanLy);
            LDatatable.AddParameter(ref dt, "@START", "String", StartRow.ToString());
            LDatatable.AddParameter(ref dt, "@END", "String", EndRow.ToString());
            DataSet ds = new TinDungTDProcess().HopDongTinDungCaNhanDanhSach(dt);

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                    dr["TEN_TTHAI_NVU"] = BusinessConstant.layNgonNguNghiepVu(dr["TTHAI_NVU"].ToString());

                DataTable serverDataTable = ds.Tables[0];
                int totalRecord = Int32.Parse(ds.Tables[1].Rows[0][0].ToString());
                DataTable clientDataTable = CommonFunction.BuildClientDataTable(serverDataTable, CurrentPagging, PaggingSize, totalRecord);

                //raddgrHDTDDS.ItemsSource = ds.Tables["DANH_SACH"].DefaultView;
                raddgrHDTDDS.ItemsSource = clientDataTable.DefaultView;
                if (!LObject.IsNullOrEmpty(raddgrHDTDDS.SelectedItems))
                    raddgrHDTDDS.SelectedItems.Clear();
                lblSumKhachHang.Content = totalRecord.ToString();
            }
            Cursor = Cursors.Arrow;
        }
        
        void ThemMoi()
        {
            if (!tlbAdd.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            ucHopDongTinDungCaNhanCT objHDTDThoaThuan = new ucHopDongTinDungCaNhanCT();
            //objHDTDThoaThuan.OnSavingCompleted += new EventHandler(objHDTDThoaThuan_OnSavingCompleted);
            string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.Function);
            Window window = new Window();
            window.Title = tittle;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Content = objHDTDThoaThuan;
            window.ShowDialog();
            Cursor = Cursors.Arrow;
        }
        void Sua()
        {
            if(!tlbModify.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            if (raddgrHDTDDS.SelectedItems.Count == 1)
            {
                DataRowView dr = (DataRowView)raddgrHDTDDS.SelectedItems[0];
                ucHopDongTinDungCaNhanCT objHDTDThoaThuan = new ucHopDongTinDungCaNhanCT();
                //objHDTDThoaThuan.OnSavingCompleted += new EventHandler(objHDTDThoaThuan_OnSavingCompleted);
                objHDTDThoaThuan.action = DatabaseConstant.Action.SUA;
                objHDTDThoaThuan.SetDataForm(dr["MA_HDTD"].ToString());
                string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.Function);
                Window window = new Window();
                window.Title = tittle;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Content = objHDTDThoaThuan;
                window.ShowDialog();
            }
            else if (raddgrHDTDDS.SelectedItems.Count > 1)
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
            Cursor = Cursors.Arrow;
        }

        void Xem()
        {
            if (!tlbView.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            if (raddgrHDTDDS.SelectedItems.Count == 1)
            {
                DataRowView dr = (DataRowView)raddgrHDTDDS.SelectedItems[0];
                ucHopDongTinDungCaNhanCT objHDTDThoaThuan = new ucHopDongTinDungCaNhanCT();
                //objHDTDThoaThuan.OnSavingCompleted += new EventHandler(objHDTDThoaThuan_OnSavingCompleted);
                objHDTDThoaThuan.action = DatabaseConstant.Action.XEM;
                objHDTDThoaThuan.SetDataForm(dr["MA_HDTD"].ToString());
                string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.Function);
                Window window = new Window();
                window.Title = tittle;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Content = objHDTDThoaThuan;
                window.ShowDialog();
            }
            else if (raddgrHDTDDS.SelectedItems.Count > 1)
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
            Cursor = Cursors.Arrow;
        }

        void Xoa()
        {
            if (!tlbDelete.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            List<int> lstID = new List<int>();
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = true;
            try
            {
                if (raddgrHDTDDS.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        List<TDTD_HDTD> lstHDTDTD = new List<TDTD_HDTD>();

                        foreach (DataRowView dr in raddgrHDTDDS.SelectedItems)
                        {
                            lstID.Add(Convert.ToInt32(dr["ID"]));
                            objHDTDTD.HDTD_TD = new TDTD_HDTD();
                            objHDTDTD.HDTD_TD.ID = Convert.ToInt32(dr["ID"]);
                            objHDTDTD.HDTD_TD.MA_HDTD = dr["MA_HDTD"].ToString();
                            objHDTDTD.HDTD_TD.TTHAI_NVU = dr["TTHAI_NVU"].ToString();
                            objHDTDTD.HDTD_TD.SO_GDICH = dr["SO_GDICH"].ToString();
                            lstHDTDTD.Add(objHDTDTD.HDTD_TD);
                        }
                        objHDTDTD.DSACH_HDTD_TD = lstHDTDTD.ToArray();
                        // Yêu cầu lock dữ liệu

                        retLockData = process.LockData(DatabaseConstant.Module.TDTD,
                        DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN,
                        DatabaseConstant.Table.TDTD_HDTD,
                        DatabaseConstant.Action.XOA,
                        lstID);
                        List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
                        int bResult = new TinDungTDProcess().HopDongTinDungCaNhan(DatabaseConstant.Action.XOA, ref objHDTDTD, ref ResponseDetail);
                        if (bResult == 1)
                            LoadData();
                        CommonFunction.ThongBaoKetQua(ResponseDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                // Yêu cầu Unlock dữ liệu
                retLockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN,
                DatabaseConstant.Table.TDTD_HDTD,
                DatabaseConstant.Action.XOA,
                lstID);
            }
            Cursor = Cursors.Arrow;
        }

        void Duyet()
        {
            if (!tlbApprove.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            List<int> lstID = new List<int>();
            UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData=false;
            try
            {
                if (raddgrHDTDDS.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        List<TDTD_HDTD> lstHDTDTD = new List<TDTD_HDTD>();
                        foreach (DataRowView dr in raddgrHDTDDS.SelectedItems)
                        {
                            lstID.Add(Convert.ToInt32(dr["ID"]));
                            objHDTDTD.HDTD_TD = new TDTD_HDTD();
                            objHDTDTD.HDTD_TD.ID = Convert.ToInt32(dr["ID"]);
                            objHDTDTD.HDTD_TD.MA_HDTD = dr["MA_HDTD"].ToString();
                            objHDTDTD.HDTD_TD.TTHAI_NVU = dr["TTHAI_NVU"].ToString();
                            objHDTDTD.HDTD_TD.SO_GDICH = dr["SO_GDICH"].ToString();
                            lstHDTDTD.Add(objHDTDTD.HDTD_TD);
                        }
                        objHDTDTD.DSACH_HDTD_TD = lstHDTDTD.ToArray();
                        // Yêu cầu lock dữ liệu
                        retLockData = process.LockData(DatabaseConstant.Module.TDTD,
                        DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN,
                        DatabaseConstant.Table.TDTD_HDTD,
                        DatabaseConstant.Action.DUYET,
                        lstID);
                        List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
                        int bResult = new TinDungTDProcess().HopDongTinDungCaNhan(DatabaseConstant.Action.DUYET, ref objHDTDTD, ref ResponseDetail);
                        if (bResult == 1)
                            LoadData();
                        CommonFunction.ThongBaoKetQua(ResponseDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                // Yêu cầu Unlock dữ liệu
                retLockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN,
                DatabaseConstant.Table.TDTD_HDTD,
                DatabaseConstant.Action.DUYET,
                lstID);
                Cursor = Cursors.Arrow;
            }
        }

        void ThoaiDuyet()
        {
            if (!tlbCancel.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            List<int> lstID = new List<int>();
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData=false;
            try
            {
                if (raddgrHDTDDS.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        List<TDTD_HDTD> lstHDTDTD = new List<TDTD_HDTD>();
                        foreach (DataRowView dr in raddgrHDTDDS.SelectedItems)
                        {
                            lstID.Add(Convert.ToInt32(dr["ID"]));
                            objHDTDTD.HDTD_TD = new TDTD_HDTD();
                            objHDTDTD.HDTD_TD.ID = Convert.ToInt32(dr["ID"]);
                            objHDTDTD.HDTD_TD.MA_HDTD = dr["MA_HDTD"].ToString();
                            objHDTDTD.HDTD_TD.TTHAI_NVU = dr["TTHAI_NVU"].ToString();
                            objHDTDTD.HDTD_TD.SO_GDICH = dr["SO_GDICH"].ToString();
                            lstHDTDTD.Add(objHDTDTD.HDTD_TD);
                        }
                        objHDTDTD.DSACH_HDTD_TD = lstHDTDTD.ToArray();
                        // Yêu cầu lock dữ liệu
                        retLockData = process.LockData(DatabaseConstant.Module.TDTD,
                        DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN,
                        DatabaseConstant.Table.TDTD_HDTD,
                        DatabaseConstant.Action.THOAI_DUYET,
                        lstID);
                        List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
                        int bResult = new TinDungTDProcess().HopDongTinDungCaNhan(DatabaseConstant.Action.THOAI_DUYET, ref objHDTDTD, ref ResponseDetail);
                        if (bResult == 1)
                            LoadData();
                        CommonFunction.ThongBaoKetQua(ResponseDetail);

                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                // Yêu cầu Unlock dữ liệu
                retLockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN,
                DatabaseConstant.Table.TDTD_HDTD,
                DatabaseConstant.Action.THOAI_DUYET,
                lstID);
                Cursor = Cursors.Arrow;
            }
        }

        void TuChoiDuyet()
        {
            if (!tlbRefuse.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            List<int> lstID = new List<int>();
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = false;
            try
            {
                if (raddgrHDTDDS.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        List<TDTD_HDTD> lstHDTDTD = new List<TDTD_HDTD>();
                        foreach (DataRowView dr in raddgrHDTDDS.SelectedItems)
                        {
                            lstID.Add(Convert.ToInt32(dr["ID"]));
                            objHDTDTD.HDTD_TD = new TDTD_HDTD();
                            objHDTDTD.HDTD_TD.ID = Convert.ToInt32(dr["ID"]);
                            objHDTDTD.HDTD_TD.MA_HDTD = dr["MA_HDTD"].ToString();
                            objHDTDTD.HDTD_TD.TTHAI_NVU = dr["TTHAI_NVU"].ToString();
                            objHDTDTD.HDTD_TD.SO_GDICH = dr["SO_GDICH"].ToString();
                            lstHDTDTD.Add(objHDTDTD.HDTD_TD);
                        }
                        objHDTDTD.DSACH_HDTD_TD = lstHDTDTD.ToArray();
                        // Yêu cầu lock dữ liệu
                        retLockData = process.LockData(DatabaseConstant.Module.TDTD,
                        DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN,
                        DatabaseConstant.Table.TDTD_HDTD,
                        DatabaseConstant.Action.TU_CHOI_DUYET,
                        lstID);
                        List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
                        int bResult = new TinDungTDProcess().HopDongTinDungCaNhan(DatabaseConstant.Action.TU_CHOI_DUYET, ref objHDTDTD, ref ResponseDetail);
                        if (bResult == 1)
                            LoadData();
                        CommonFunction.ThongBaoKetQua(ResponseDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                // Yêu cầu Unlock dữ liệu
                retLockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN,
                DatabaseConstant.Table.TDTD_HDTD,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstID);
                Cursor = Cursors.Arrow;
            }
        }

        void objHDTDThoaThuan_OnSavingCompleted(object sender, EventArgs e)
        {
            LoadData();
        }
        #endregion
    }
}
