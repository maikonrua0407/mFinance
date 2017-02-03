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
using Utilities.Common;
using Telerik.Windows.Controls;
using Presentation.Process.Common;
using Presentation.Process;
using PresentationWPF.CustomControl;
using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process.TinDungTDServiceRef;

namespace PresentationWPF.TinDungTD.KheUoc
{
    /// <summary>
    /// Interaction logic for ucKheUocDS.xaml
    /// </summary>
    public partial class ucKheUocDS : UserControl
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

        private DatabaseConstant.Module Module = DatabaseConstant.Module.TDTD;
        private DatabaseConstant.Function Function = DatabaseConstant.Function.TDTD_KHE_UOC;

        private List<AutoCompleteEntry> lstLoaiGiayTo = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstDonViTinhThoiHan = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstSanPhamTinDung = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstNhomNo = new List<AutoCompleteEntry>();

        // Phân trang
        int StartRow = 1;
        int EndRow = ClientInformation.SoLuongBanGhi;
        int CurrentPagging = 1;
        int PaggingSize = ClientInformation.SoLuongBanGhi;
        #endregion

        #region Khoi tao
        public ucKheUocDS()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDungTD;component/KheUoc/ucKheUocDS.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindHotkey();
            LoadTreeview();
            KhoiTaoComboBox();
            teldtNgayDaoHanHDTu.Value = null;
            teldtNgayDaoHanHDDen.Value = null;
            teldtNgayLapHDTu.Value = null;
            teldtNgayLapHDDen.Value = null;
        }

        private void KhoiTaoComboBox()
        {
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add("''" + ClientInformation.MaDonVi + "''");
            lstDieuKien.Add("0");
            lstDieuKien.Add("0");
            lstDieuKien.Add("0");
            string Dislay = LLanguage.SearchResourceByKey("U.DungChung.TatCa");
            lstSanPhamTinDung.Add(new AutoCompleteEntry(Dislay, "%", "0"));
            KhoiTaoGiaTriComboBox(ref lstSanPhamTinDung, "COMBOBOX_SAN_PHAM_TD", cmbSanPhamTinDung, lstDieuKien);
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

        private void KhoiTaoGiaTriComboBox(ref List<AutoCompleteEntry> lstAutoComplete, string maTruyVan, RadComboBox cmbCommon, List<string> lstDieuKien)
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

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(raddgrHDTDDS);            
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

        private void raddgrHDTDDS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BeforeView();
        }

        private void LoadTreeview()
        {
            try
            {
                DanhMucProcess danhMucProcess = new DanhMucProcess();
                DataTable dtTreeDonVi = new DataTable();
                dtTreeDonVi = danhMucProcess.GetTreeDonVi(ClientInformation.TenDangNhap, ClientInformation.MaDonViQuanLy).Tables[0];

                //Cấu trúc của Tag: GiaTri#Level
                item.Items.Clear();
                foreach (DataRow dr in dtTreeDonVi.Rows)
                {
                    if (Convert.ToInt32(dr["LEVEL"]) == 1)
                    {
                        item.Tag = dr["NODE"].ToString() + "#" + dr["LEVEL"].ToString();
                        item.Header = dr["NODE_NAME"].ToString();
                        item.IsExpanded = true;
                        break;
                    }
                }

                BuildTree(item, dtTreeDonVi);

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        protected void BuildTree(RadTreeViewItem item, DataTable dt)
        {
            try
            {
                //Cấu trúc của Tag: GiaTri#Level  ( VD:  MaSP001#2 hoặc CUM001#3#DON_VI)
                string sTag = item.Tag.ToString();
                int i = sTag.IndexOf("#");

                string sValue = sTag.Substring(0, i);
                int iLevel = Convert.ToInt32(sTag.Substring(i + 1));

                foreach (DataRow row in dt.Rows)
                {
                    if (iLevel < Convert.ToInt32(row["LEVEL"]))
                    {
                        if (row["NODE_PARENT"].ToString() == sValue)
                        {
                            RadTreeViewItem subItem = new RadTreeViewItem();
                            subItem.Header = row["NODE_NAME"].ToString();
                            subItem.Tag = row["NODE"].ToString() + "#" + row["LEVEL"].ToString();
                            subItem.IsExpanded = false;
                            item.Items.Add(subItem);
                            BuildTree(subItem, dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }
        #endregion

        #region Xử lý nghiệp vụ
        void LoadData()
        {
            Cursor = Cursors.Wait;
            try
            {
                string sMaTrangThaiNVu = ucTrangThaiNVu.GetItemsSelected();
                sMaTrangThaiNVu = sMaTrangThaiNVu.Replace(@"''", "'");
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
                if (cmbSanPhamTinDung.SelectedIndex > -1) MaSanPham = lstSanPhamTinDung.ElementAt(cmbSanPhamTinDung.SelectedIndex).KeywordStrings.First();
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

                // Param

                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@INP_MA_TRANG_THAI_NGHIEP_VU", "String", sMaTrangThaiNVu);
                LDatatable.AddParameter(ref dt, "@INP_SO_HDTD", "String", txtSoHDTD.Text.Trim());
                LDatatable.AddParameter(ref dt, "@INP_SO_KUOC", "String", txtSoKheUoc.Text.Trim());
                LDatatable.AddParameter(ref dt, "@INP_NGAY_NNO_TU", "String", NgayNhanNoTu);
                LDatatable.AddParameter(ref dt, "@INP_NGAY_NNO_DEN", "String", NgayNhanNoDen);
                LDatatable.AddParameter(ref dt, "@INP_NGAY_DHAN_TU", "String", NgayDaoHanTu);
                LDatatable.AddParameter(ref dt, "@INP_NGAY_DHAN_DEN", "String", NgayDaoHanDen);
                LDatatable.AddParameter(ref dt, "@INP_SO_TIEN_GN_TU", "String", SoTienGNTu);
                LDatatable.AddParameter(ref dt, "@INP_SO_TIEN_GN_DEN", "String", SoTienGNDen);
                LDatatable.AddParameter(ref dt, "@INP_SO_DU_TU", "String", SoDuTu);
                LDatatable.AddParameter(ref dt, "@INP_SO_DU_DEN", "String", SoDuDen);
                LDatatable.AddParameter(ref dt, "@INP_THOI_HAN_TU", "String", ThoiHanTu);
                LDatatable.AddParameter(ref dt, "@INP_THOI_HAN_DEN", "String", ThoiHanDen);
                LDatatable.AddParameter(ref dt, "@INP_THOI_HAN_DVI_TU", "String", ThoiHanDViTu);
                LDatatable.AddParameter(ref dt, "@INP_THOI_HAN_DVI_DEN", "String", ThoiHanDViDen);
                LDatatable.AddParameter(ref dt, "@INP_LSUAT_TU", "String", LaiSuatTu);
                LDatatable.AddParameter(ref dt, "@INP_LSUAT_DEN", "String", LaiSuatDen);
                LDatatable.AddParameter(ref dt, "@INP_MA_KHANG", "String", txtMaKhachHang.Text.Trim());
                LDatatable.AddParameter(ref dt, "@INP_TEN_KHANG", "String", txtTenKhachHang.Text.Trim());
                LDatatable.AddParameter(ref dt, "@INP_MA_GTO", "String", LoaiGiayTo);
                LDatatable.AddParameter(ref dt, "@INP_SO_GTO", "String", SoGiayTo);
                LDatatable.AddParameter(ref dt, "@INP_DIEN_THOAI", "String", txtDienThoai.Text.Trim());
                LDatatable.AddParameter(ref dt, "@INP_EMAIL", "String", txtEmail.Text.Trim());
                LDatatable.AddParameter(ref dt, "@INP_SANPHAM", "String", MaSanPham);
                LDatatable.AddParameter(ref dt, "@INP_KHUVUC", "string", ListKVuc);
                LDatatable.AddParameter(ref dt, "@INP_USERNAME", "string", ClientInformation.TenDangNhap);
                LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "string", ClientInformation.MaDonViQuanLy);
                LDatatable.AddParameter(ref dt, "@INP_START_ROW", "String", StartRow.ToString());
                LDatatable.AddParameter(ref dt, "@INP_END_ROW", "String", EndRow.ToString());

                DataSet ds = new TinDungTDProcess().getDanhSachKUOC(dt);

                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr["TEN_TTHAINVU"] = BusinessConstant.layNgonNguNghiepVu(dr["TTHAI_NVU"].ToString());
                        dr["TEN_THAI_RUIRO"] = LLanguage.SearchResourceByKey(dr["TEN_THAI_RUIRO"].ToString());
                    }

                    DataTable serverDataTable = ds.Tables[0];
                    int totalRecord = ds.Tables[0].Rows.Count;
                    //decimal totalSum = Decimal.Parse(ds.Tables[2].Rows[0][0].ToString());
                    DataTable clientDataTable = CommonFunction.BuildClientDataTable(serverDataTable, CurrentPagging, PaggingSize, totalRecord);

                    //grdKheUocDS.ItemsSource = ds.Tables["DANH_SACH"].DefaultView;
                    raddgrHDTDDS.ItemsSource = clientDataTable.DefaultView;
                    raddgrHDTDDS.SelectedItems.Clear();
                    lblSumKhachHang.Content = totalRecord.ToString();
                    //lblSumDuNo.Content = totalSum.ToString("N0");
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

        void LoadDataPhanTrang()
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
                if (cmbSanPhamTinDung.SelectedIndex > -1) MaSanPham = lstSanPhamTinDung.ElementAt(cmbSanPhamTinDung.SelectedIndex).KeywordStrings.First();
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
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr["TEN_TTHAINVU"] = BusinessConstant.layNgonNguNghiepVu(dr["TTHAI_NVU"].ToString());
                        dr["TEN_THAI_RUIRO"] = LLanguage.SearchResourceByKey(dr["TEN_THAI_RUIRO"].ToString());
                    }

                    DataTable serverDataTable = ds.Tables[0];
                    int totalRecord = Int32.Parse(ds.Tables[1].Rows[0][0].ToString());
                    decimal totalSum = Decimal.Parse(ds.Tables[2].Rows[0][0].ToString());
                    DataTable clientDataTable = CommonFunction.BuildClientDataTable(serverDataTable, CurrentPagging, PaggingSize, totalRecord);

                    //grdKheUocDS.ItemsSource = ds.Tables["DANH_SACH"].DefaultView;
                    raddgrHDTDDS.ItemsSource = clientDataTable.DefaultView;
                    raddgrHDTDDS.SelectedItems.Clear();
                    lblSumKhachHang.Content = totalRecord.ToString();
                    //lblSumDuNo.Content = totalSum.ToString("N0");
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

        void objHDTDThoaThuan_OnSavingCompleted(object sender, EventArgs e)
        {
            LoadData();
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
            ucKheUocCT userControl = new ucKheUocCT();

            userControl.Action = DatabaseConstant.Action.THEM;
            userControl.OnSavingCompleted += new EventHandler(objHDTDThoaThuan_OnSavingCompleted);

            window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.TDTD_KHE_UOC);
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

                if (raddgrHDTDDS.SelectedItems != null)
                {
                    if (raddgrHDTDDS.SelectedItems.Count == 0)
                    {
                        LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    else if (raddgrHDTDDS.SelectedItems.Count > 1)
                    {
                        LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    else
                    {
                        int currentPage = raddgrHDTDDS.Items.PageIndex;
                        int currentPosition = raddgrHDTDDS.Items.CurrentPosition;
                        int currentID = int.Parse(((DataRowView)raddgrHDTDDS.SelectedItems.First())["ID"].ToString());

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
                ucKheUocCT userControl = new ucKheUocCT();

                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);

                bool ret = process.LockData(DatabaseConstant.Module.TDTT,
                    DatabaseConstant.Function.TD_SAN_PHAMTT,
                    DatabaseConstant.Table.TD_SAN_PHAMTT,
                    DatabaseConstant.Action.SUA,
                    listLockId);
                if (ret)
                {
                    userControl.OnSavingCompleted += new EventHandler(objHDTDThoaThuan_OnSavingCompleted);
                    userControl.Action = DatabaseConstant.Action.SUA;
                    userControl.obj = new TDTD_KHE_UOC();
                    userControl.obj.objKuoc = new THONG_TIN_KHE_UOC();
                    userControl.obj.objKuoc.ID = id;

                    Window window = new Window();
                    window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.TDTD_KHE_UOC);
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.Content = userControl;
                    window.ShowDialog();
                }
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                }
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
                if (raddgrHDTDDS.SelectedItems != null)
                {
                    if (raddgrHDTDDS.SelectedItems.Count == 0)
                    {
                        LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    else if (raddgrHDTDDS.SelectedItems.Count > 1)
                    {
                        LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    else
                    {
                        int currentPage = raddgrHDTDDS.Items.PageIndex;
                        int currentPosition = raddgrHDTDDS.Items.CurrentPosition;
                        int currentID = int.Parse(((DataRowView)raddgrHDTDDS.SelectedItems.First())["ID"].ToString());
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
                ucKheUocCT userControl = new ucKheUocCT();

                userControl.OnSavingCompleted += new EventHandler(objHDTDThoaThuan_OnSavingCompleted);
                userControl.Action = DatabaseConstant.Action.XEM;
                userControl.obj = new TDTD_KHE_UOC();
                userControl.obj.objKuoc = new THONG_TIN_KHE_UOC();
                userControl.obj.objKuoc.ID = id;

                Window window = new Window();
                window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.TDTD_KHE_UOC);
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
                if (raddgrHDTDDS.SelectedItems != null)
                {
                    if (raddgrHDTDDS.SelectedItems.Count == 0)
                    {
                        LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    else
                    {
                        // Lấy danh sách dữ liệu cần xử lý
                        List<int> listId = new List<int>();
                        foreach (DataRowView dr in raddgrHDTDDS.SelectedItems)
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

                            bool retLockData = process.LockData(DatabaseConstant.Module.TDTD,
                                DatabaseConstant.Function.TDTD_KHE_UOC,
                                DatabaseConstant.Table.TDTD_KUOC,
                                DatabaseConstant.Action.XOA,
                                listId);

                            // Nếu lock thành công >> cho phép xử lý
                            if (retLockData)
                            {
                                OnDelete(listId);
                            }
                            else
                            {
                                LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                            }

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
            TinDungTDProcess ProcessTinDungTD = new TinDungTDProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                List<TDTD_KHE_UOC> lstKheUoc = new List<TDTD_KHE_UOC>();

                foreach (int id in listId)
                {
                    TDTD_KHE_UOC obj = new TDTD_KHE_UOC();
                    obj.objKuoc = new THONG_TIN_KHE_UOC();
                    obj.objKuoc.ID = id;
                    lstKheUoc.Add(obj);
                }
                bool ret = ProcessTinDungTD.KheUocTieuDungDanhSach(DatabaseConstant.Action.XOA, ref lstKheUoc, ref listClientResponseDetail);

                AfterDelete(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_KHE_UOC,
                    DatabaseConstant.Table.TDTD_KUOC,
                    DatabaseConstant.Action.XOA,
                    listId);
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

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                DatabaseConstant.Function.TDTD_KHE_UOC,
                DatabaseConstant.Table.TDTD_KUOC,
                DatabaseConstant.Action.XOA,
                listId);
        }

        /// <summary>
        /// Trước khi duyệt
        /// </summary>
        private void BeforeApprove()
        {
            try
            {
                if (raddgrHDTDDS.SelectedItems != null)
                {
                    if (raddgrHDTDDS.SelectedItems.Count == 0)
                    {
                        LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    else
                    {
                        // Lấy danh sách dữ liệu cần xử lý
                        List<int> listId = new List<int>();
                        foreach (DataRowView dr in raddgrHDTDDS.SelectedItems)
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

                            bool retLockData = process.LockData(DatabaseConstant.Module.TDTD,
                                DatabaseConstant.Function.TDTD_KHE_UOC,
                                DatabaseConstant.Table.TDTD_KUOC,
                                DatabaseConstant.Action.DUYET,
                                listId);

                            // Nếu lock thành công >> cho phép xử lý
                            if (retLockData)
                            {
                                OnApprove(listId);
                            }
                            else
                            {
                                LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                            }

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
            TinDungTDProcess ProcessTinDungTD = new TinDungTDProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                List<TDTD_KHE_UOC> lstKheUoc = new List<TDTD_KHE_UOC>();

                foreach (int id in listId)
                {
                    TDTD_KHE_UOC obj = new TDTD_KHE_UOC();
                    obj.objKuoc = new THONG_TIN_KHE_UOC();
                    obj.objKuoc.ID = id;
                    lstKheUoc.Add(obj);
                }
                bool ret = ProcessTinDungTD.KheUocTieuDungDanhSach(DatabaseConstant.Action.DUYET, ref lstKheUoc, ref listClientResponseDetail);

                AfterApprove(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_KHE_UOC,
                    DatabaseConstant.Table.TDTD_KUOC,
                    DatabaseConstant.Action.XOA,
                    listId);
                this.Cursor = Cursors.Arrow;
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

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                DatabaseConstant.Function.TDTD_KHE_UOC,
                DatabaseConstant.Table.TDTD_KUOC,
                DatabaseConstant.Action.DUYET,
                listId);
        }


        /// <summary>
        /// Trước khi thoái duyệt
        /// </summary>
        private void BeforeCancel()
        {
            try
            {
                if (raddgrHDTDDS.SelectedItems != null)
                {
                    if (raddgrHDTDDS.SelectedItems.Count == 0)
                    {
                        LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    else
                    {
                        // Lấy danh sách dữ liệu cần xử lý
                        List<int> listId = new List<int>();
                        foreach (DataRowView dr in raddgrHDTDDS.SelectedItems)
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

                            bool retLockData = process.LockData(DatabaseConstant.Module.TDTD,
                                DatabaseConstant.Function.TDTD_KHE_UOC,
                                DatabaseConstant.Table.TDTD_KUOC,
                                DatabaseConstant.Action.THOAI_DUYET,
                                listId);

                            // Nếu lock thành công >> cho phép xử lý
                            if (retLockData)
                            {
                                OnCancel(listId);
                            }
                            else
                            {
                                LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                            }

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
            TinDungTDProcess ProcessTinDungTD = new TinDungTDProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                List<TDTD_KHE_UOC> lstKheUoc = new List<TDTD_KHE_UOC>();

                foreach (int id in listId)
                {
                    TDTD_KHE_UOC obj = new TDTD_KHE_UOC();
                    obj.objKuoc = new THONG_TIN_KHE_UOC();
                    obj.objKuoc.ID = id;
                    lstKheUoc.Add(obj);
                }
                bool ret = ProcessTinDungTD.KheUocTieuDungDanhSach(DatabaseConstant.Action.THOAI_DUYET, ref lstKheUoc, ref listClientResponseDetail);

                AfterCancel(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTT,
                    DatabaseConstant.Function.TD_SAN_PHAMTT,
                    DatabaseConstant.Table.TD_SAN_PHAMTT,
                    DatabaseConstant.Action.XOA,
                    listId);
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

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_KHE_UOC,
                    DatabaseConstant.Table.TDTD_KUOC,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listId);
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
                if (raddgrHDTDDS.SelectedItems != null)
                {
                    if (raddgrHDTDDS.SelectedItems.Count == 0)
                    {
                        LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    else
                    {
                        // Lấy danh sách dữ liệu cần xử lý
                        List<int> listId = new List<int>();
                        foreach (DataRowView dr in raddgrHDTDDS.SelectedItems)
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

                            bool retLockData = process.LockData(DatabaseConstant.Module.TDTT,
                                DatabaseConstant.Function.TD_SAN_PHAMTT,
                                DatabaseConstant.Table.TD_SAN_PHAMTT,
                                DatabaseConstant.Action.TU_CHOI_DUYET,
                                listId);

                            // Nếu lock thành công >> cho phép xử lý
                            if (retLockData)
                            {
                                OnRefuse(listId);
                            }
                            else
                            {
                                LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                            }

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
            TinDungTDProcess ProcessTinDungTD = new TinDungTDProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                List<TDTD_KHE_UOC> lstKheUoc = new List<TDTD_KHE_UOC>();

                
                foreach (int id in listId)
                {
                    TDTD_KHE_UOC obj = new TDTD_KHE_UOC();
                    obj.objKuoc = new THONG_TIN_KHE_UOC();
                    obj.objKuoc.ID = id;
                    lstKheUoc.Add(obj);
                }
                bool ret = ProcessTinDungTD.KheUocTieuDungDanhSach(DatabaseConstant.Action.TU_CHOI_DUYET, ref lstKheUoc, ref listClientResponseDetail);

                AfterRefuse(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_KHE_UOC,
                    DatabaseConstant.Table.TDTD_KUOC,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listId);
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

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_KHE_UOC,
                    DatabaseConstant.Table.TDTD_KUOC,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listId);
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
        #endregion
    }
}
