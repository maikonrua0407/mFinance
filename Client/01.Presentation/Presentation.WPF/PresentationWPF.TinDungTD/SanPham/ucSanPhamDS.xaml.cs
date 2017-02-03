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
using Utilities.Common;
using Presentation.Process.Common;
using Microsoft.Windows.Controls.Ribbon;
using System.Data;
using Telerik.Windows.Controls;
using Presentation.Process;
using PresentationWPF.CustomControl;
using Presentation.Process.TinDungTDServiceRef;

namespace PresentationWPF.TinDungTD.SanPham
{
    /// <summary>
    /// Interaction logic for ucSanPhamDS.xaml
    /// </summary>
    public partial class ucSanPhamDS : UserControl
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
        private DatabaseConstant.Function Function = DatabaseConstant.Function.TDTD_SAN_PHAM;

        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceThoiHanVay = new List<AutoCompleteEntry>();

        // Phân trang
        int StartRow = 1;
        int EndRow = ClientInformation.SoLuongBanGhi;
        int CurrentPagging = 1;
        int PaggingSize = ClientInformation.SoLuongBanGhi;

        private delegate void LoadDuLieuCT(bool bSua);
        #endregion

        #region Khoi tao
        public ucSanPhamDS()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDungTD;component/SanPham/ucSanPhamDS.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindHotkey();
            LoadTreeview();
            LoadCombobox();
            dtNgayHieuLucDen.Value = null;
            dtNgayHieuLucTu.Value = null;
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
            Xem();
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
            //XuatExcel();
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
            {
                XuatExcel();
            }  
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            {
                LoadData();
            }            
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
            {
                LoadData(); 
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
            {
                XuatExcel();
            }  
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            {
                LoadData();
            }             
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
            {
                LoadData();
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
            PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(raddgrSanPham, txtTimKiemNhanh.Text);
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
            if (raddgrSanPham != null && raddgrSanPham.ItemsSource != null)
            {
                DataView dt = ((DataView)raddgrSanPham.ItemsSource);
                if (dt != null)
                {
                    radPage.PageSize = (int)nudPageSize.Value;
                    raddgrSanPham.DataContext = dt;
                }
            }
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

        private void LoadCombobox()
        {
            
            try
            {
                COMBOBOX_DTO combo = null;
                List<string> lstDieuKien = new List<string>();
                List<COMBOBOX_DTO> lstCombobox = new List<COMBOBOX_DTO>();
                AutoComboBox auCombo = new AutoComboBox();

                //Add value
                lstSourceLoaiTien.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
                lstSourceThoiHanVay.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));

                //Loại tiền
                combo = new COMBOBOX_DTO();
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_LOAITIEN.getValue();
                combo.combobox = cmbLoaiTien;
                combo.lstSource = lstSourceLoaiTien;
                combo.maChon = ClientInformation.MaDongNoiTe;
                lstCombobox.Add(combo);

                //Loại vay
                lstDieuKien = new List<string>();
                combo = new COMBOBOX_DTO();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.THOI_HAN_VAY_VON.getValue());

                combo.maChon = BusinessConstant.THOI_HAN_CHO_VAY.NGAN_HAN.layGiaTri();
                combo.combobox = cmbLoaiVay;
                combo.lstSource = lstSourceThoiHanVay;
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                combo.lstDieuKien = lstDieuKien;
                lstCombobox.Add(combo);

                //Gen combobox
                auCombo.GenAutoComboBoxTheoList(ref lstCombobox);
                cmbLoaiTien.SelectedIndex = 0;
                cmbLoaiVay.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
        #endregion

        #region Xử lý nghiệp vụ
        private void LoadData()
        {
            Cursor = Cursors.Wait;
            try
            {
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                string loaiTien = "";
                string loaiVay = "";
                string ngayHieuLucTu = "";
                string ngayHieuLucDen = "";
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

                if (cmbLoaiVay.SelectedIndex != -1)
                {
                    loaiVay = lstSourceThoiHanVay.ElementAt(cmbLoaiVay.SelectedIndex).KeywordStrings.FirstOrDefault();
                }

                if (cmbLoaiTien.SelectedIndex != -1)
                {
                    loaiTien = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.FirstOrDefault();
                }

                if (dtNgayHieuLucTu.Value != null)
                {
                    ngayHieuLucTu = dtNgayHieuLucTu.Value.Value.ToString("yyyyMMdd");
                }

                if (dtNgayHieuLucDen.Value != null)
                {
                    ngayHieuLucDen = dtNgayHieuLucDen.Value.Value.ToString("yyyyMMdd");
                }

                // Phân trang
                int StartRow = 1;
                int EndRow = ClientInformation.SoLuongBanGhi;
                int CurrentPagging = 1;
                int PaggingSize = ClientInformation.SoLuongBanGhi;

                TThaiNVu = TThaiNVu.Replace(@"''",@"'");

                LDatatable.AddParameter(ref dt, "@INP_MA_TRANG_THAI_NGHIEP_VU", "String", TThaiNVu);
                LDatatable.AddParameter(ref dt, "@INP_MA_SAN_PHAM", "String", txtMaSanPham.Text.Trim());
                LDatatable.AddParameter(ref dt, "@INP_TEN_SAN_PHAM", "String", txtTenSanPham.Text.Trim());
                LDatatable.AddParameter(ref dt, "@INP_LOAI_TIEN", "String", loaiTien);
                LDatatable.AddParameter(ref dt, "@INP_LOAI_VAY", "String", loaiVay);
                LDatatable.AddParameter(ref dt, "@INP_NGAY_HL_DEN", "String", ngayHieuLucDen);
                LDatatable.AddParameter(ref dt, "@INP_NGAY_HL_TU", "String", ngayHieuLucTu);
                LDatatable.AddParameter(ref dt, "@INP_SO_TIEN_VAY_TU", "Decimal", numSoTienVayTu.Value.Value.ToString());
                LDatatable.AddParameter(ref dt, "@INP_SO_TIEN_VAY_DEN", "Decimal", numSoTienVayDen.Value.Value.ToString());
                LDatatable.AddParameter(ref dt, "@INP_KHUVUC", "String", ListKVuc);
                LDatatable.AddParameter(ref dt, "@INP_USERNAME", "String", ClientInformation.TenDangNhap);
                LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "String", ClientInformation.MaDonViQuanLy);
                LDatatable.AddParameter(ref dt, "@INP_START_ROW", "String", StartRow.ToString());
                LDatatable.AddParameter(ref dt, "@INP_END_ROW", "String", EndRow.ToString());

                DataSet ds = new TinDungTDProcess().GetDSSanPhamTinDungTieuDung(dt);
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr["TRANG_THAI_NVU"] = LLanguage.SearchResourceByKey(dr["TRANG_THAI_NVU"].ToString());
                        dr["LOAI_SAN_PHAM"] = LLanguage.SearchResourceByKey(dr["LOAI_SAN_PHAM"].ToString());
                        dr["LOAI_VAY"] = LLanguage.SearchResourceByKey(dr["LOAI_VAY"].ToString());

                    }
                    DataTable serverDataTable = ds.Tables[0];
                    //int totalRecord = Int32.Parse(ds.Tables[1].Rows[0][0].ToString());
                    DataTable clientDataTable = CommonFunction.BuildClientDataTable(serverDataTable, CurrentPagging, PaggingSize, 1000);

                    raddgrSanPham.ItemsSource = ds.Tables["DANH_SACH"].DefaultView;
                    //raddgrSanPham.ItemsSource = clientDataTable.DefaultView;
                    if (!LObject.IsNullOrEmpty(raddgrSanPham.SelectedItems))
                    {
                        raddgrSanPham.SelectedItems.Clear();
                    }
                    //lblSumKhachHang.Content = totalRecord.ToString();
                }
            }
            catch (Exception ex)
            {

            }
            Cursor = Cursors.Arrow;
        }

        private void LoadDataPhanTrang()
        {
            Cursor = Cursors.Wait;
            try
            {
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                string loaiTien = "";
                string loaiVay = "";
                string ngayHieuLucTu = "";
                string ngayHieuLucDen = "";
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

                if (cmbLoaiVay.SelectedIndex != -1)
                {
                    loaiVay = lstSourceThoiHanVay.ElementAt(cmbLoaiVay.SelectedIndex).KeywordStrings.FirstOrDefault();
                }

                if (cmbLoaiTien.SelectedIndex != -1)
                {
                    loaiTien = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.FirstOrDefault();
                }

                if (dtNgayHieuLucTu.Value != null)
                {
                    ngayHieuLucTu = dtNgayHieuLucTu.Value.Value.ToString("yyyyMMdd");
                }

                if (dtNgayHieuLucDen.Value != null)
                {
                    ngayHieuLucDen = dtNgayHieuLucDen.Value.Value.ToString("yyyyMMdd");
                }

                // Phân trang
                int StartRow = 1;
                int EndRow = ClientInformation.SoLuongBanGhi;
                int CurrentPagging = 1;
                int PaggingSize = ClientInformation.SoLuongBanGhi;

                LDatatable.AddParameter(ref dt, "@INP_MA_TRANG_THAI_NGHIEP_VU", "String", TThaiNVu);
                LDatatable.AddParameter(ref dt, "@INP_MA_SAN_PHAM", "String", txtMaSanPham.Text.Trim());
                LDatatable.AddParameter(ref dt, "@INP_TEN_SAN_PHAM", "String", txtTenSanPham.Text.Trim());
                LDatatable.AddParameter(ref dt, "@INP_LOAI_TIEN", "String", loaiTien);
                LDatatable.AddParameter(ref dt, "@INP_LOAI_VAY", "String", loaiVay);
                LDatatable.AddParameter(ref dt, "@INP_NGAY_HL_DEN", "String", ngayHieuLucDen);
                LDatatable.AddParameter(ref dt, "@INP_NGAY_HL_TU", "String", ngayHieuLucTu);
                LDatatable.AddParameter(ref dt, "@INP_SO_TIEN_VAY_TU", "Decimal", numSoTienVayTu.Value.Value.ToString());
                LDatatable.AddParameter(ref dt, "@INP_SO_TIEN_VAY_DEN", "Decimal", numSoTienVayDen.Value.Value.ToString());
                LDatatable.AddParameter(ref dt, "@INP_KHUVUC", "String", ListKVuc);
                LDatatable.AddParameter(ref dt, "@INP_USERNAME", "String", ClientInformation.TenDangNhap);
                LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "String", ClientInformation.MaDonViQuanLy);
                LDatatable.AddParameter(ref dt, "@INP_START_ROW", "String", StartRow.ToString());
                LDatatable.AddParameter(ref dt, "@INP_END_ROW", "String", EndRow.ToString());

                DataSet ds = new TinDungTDProcess().GetDSSanPhamTinDungTieuDung(dt);

                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr["TRANG_THAI_NVU"] = LLanguage.SearchResourceByKey(dr["TRANG_THAI_NVU"].ToString());
                        dr["LOAI_SAN_PHAM"] = LLanguage.SearchResourceByKey(dr["LOAI_SAN_PHAM"].ToString());
                        dr["LOAI_VAY"] = LLanguage.SearchResourceByKey(dr["LOAI_VAY"].ToString());

                    }

                    DataTable serverDataTable = ds.Tables[0];
                    int totalRecord = Int32.Parse(ds.Tables[1].Rows[0][0].ToString());
                    DataTable clientDataTable = CommonFunction.BuildClientDataTable(serverDataTable, CurrentPagging, PaggingSize, totalRecord);

                    //raddgrSanPham.ItemsSource = ds.Tables["DANH_SACH"].DefaultView;
                    raddgrSanPham.ItemsSource = clientDataTable.DefaultView;
                    if (!LObject.IsNullOrEmpty(raddgrSanPham.SelectedItems))
                        raddgrSanPham.SelectedItems.Clear();
                    lblSumKhachHang.Content = totalRecord.ToString();
                }
            }
            catch (Exception ex)
            {
            }
            Cursor = Cursors.Arrow;
        }

        private void ThemMoi()
        {
            if (!tlbAdd.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            ucSanPhamTD ucSanPham = new ucSanPhamTD();
            ucSanPham.OnSavingComleted += new EventHandler(objHDTDThoaThuan_OnSavingCompleted);
            string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.Function);
            Window window = new Window();
            window.Title = tittle;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Content = ucSanPham;
            window.ShowDialog();
            Cursor = Cursors.Arrow;
        }

        private void Sua()
        {
            if (!tlbModify.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            if (raddgrSanPham.SelectedItems.Count == 1)
            {
                DataRowView dr = (DataRowView)raddgrSanPham.SelectedItems[0];
                ucSanPhamTD ucSanPham = new ucSanPhamTD();
                ucSanPham.IdSanPham = Convert.ToInt32(dr["ID"]);
                ucSanPham.OnSavingComleted += new EventHandler(objHDTDThoaThuan_OnSavingCompleted);
                LoadDuLieuCT loadDuLieu = new LoadDuLieuCT(ucSanPham.LoadDuLieuCT);
                loadDuLieu(true);
                string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.Function);
                Window window = new Window();
                window.Title = tittle;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Content = ucSanPham;
                window.ShowDialog();
            }
            else if (raddgrSanPham.SelectedItems.Count > 1)
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
            Cursor = Cursors.Arrow;
        }

        private void Xem()
        {
            if (!tlbView.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            if (raddgrSanPham.SelectedItems.Count == 1)
            {
                DataRowView dr = (DataRowView)raddgrSanPham.SelectedItems[0];
                ucSanPhamTD ucSanPham = new ucSanPhamTD();
                ucSanPham.IdSanPham = Convert.ToInt32(dr["ID"]);
                ucSanPham.OnSavingComleted += new EventHandler(objHDTDThoaThuan_OnSavingCompleted);
                LoadDuLieuCT loadDuLieu = new LoadDuLieuCT(ucSanPham.LoadDuLieuCT);
                loadDuLieu(false);
                string tittle = DatabaseConstant.layNgonNguTieuDeForm(this.Function);
                Window window = new Window();
                window.Title = tittle;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Content = ucSanPham;
                window.ShowDialog();
            }
            else if (raddgrSanPham.SelectedItems.Count > 1)
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
            Cursor = Cursors.Arrow;
        }

        private void Xoa()
        {
            if (!tlbDelete.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            List<int> lstID = new List<int>();
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = false;
            try
            {
                if (raddgrSanPham.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        TinDungTDProcess tindungProcess = new TinDungTDProcess();
                        List<ClientResponseDetail> ClientResponseDetail = new List<Presentation.Process.Common.ClientResponseDetail>();
                        List<TDTD_SAN_PHAM> lstSanPham = new List<TDTD_SAN_PHAM>();
                        List<KT_PHAN_HE_PLOAI> lstPhanHePloai = new List<KT_PHAN_HE_PLOAI>();

                        foreach (DataRowView dr in raddgrSanPham.SelectedItems)
                        {
                            TDTD_SAN_PHAM objSP = new TDTD_SAN_PHAM();
                            objSP.ID = Convert.ToInt32(dr["ID"]);
                            lstSanPham.Add(objSP);
                            lstID.Add(objSP.ID);
                        }

                        // Yêu cầu lock dữ liệu
                        retLockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                                                        DatabaseConstant.Function.TDTD_SAN_PHAM,
                                                        DatabaseConstant.Table.TDTD_SAN_PHAM,
                                                        DatabaseConstant.Action.XOA,
                                                        lstID);

                        int iret = tindungProcess.SanPhamTinDung(DatabaseConstant.Action.XOA, ref lstSanPham, ref lstPhanHePloai, ref ClientResponseDetail);
                        CommonFunction.ThongBaoKetQua(ClientResponseDetail);
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
                DatabaseConstant.Function.TDTD_DON_XIN_VAY_VON,
                DatabaseConstant.Table.TDTD_HDTD,
                DatabaseConstant.Action.XOA,
                lstID);
                Cursor = Cursors.Arrow;
            }
        }

        private void Duyet()
        {
            if (!tlbApprove.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            List<int> lstID = new List<int>();
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = false;
            try
            {
                if (raddgrSanPham.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        TinDungTDProcess tindungProcess = new TinDungTDProcess();
                        List<ClientResponseDetail> ClientResponseDetail = new List<Presentation.Process.Common.ClientResponseDetail>();
                        List<TDTD_SAN_PHAM> lstSanPham = new List<TDTD_SAN_PHAM>();
                        List<KT_PHAN_HE_PLOAI> lstPhanHePloai = new List<KT_PHAN_HE_PLOAI>();

                        foreach (DataRowView dr in raddgrSanPham.SelectedItems)
                        {
                            TDTD_SAN_PHAM objSP = new TDTD_SAN_PHAM();
                            objSP.ID = Convert.ToInt32(dr["ID"]);
                            lstSanPham.Add(objSP);
                            lstID.Add(objSP.ID);
                        }

                        // Yêu cầu lock dữ liệu
                        retLockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                                                        DatabaseConstant.Function.TDTD_SAN_PHAM,
                                                        DatabaseConstant.Table.TDTD_SAN_PHAM,
                                                        DatabaseConstant.Action.DUYET,
                                                        lstID);

                        int iret = tindungProcess.SanPhamTinDung(DatabaseConstant.Action.DUYET, ref lstSanPham, ref lstPhanHePloai, ref ClientResponseDetail);
                        CommonFunction.ThongBaoKetQua(ClientResponseDetail);
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
                DatabaseConstant.Function.TDTD_DON_XIN_VAY_VON,
                DatabaseConstant.Table.TDTD_HDTD,
                DatabaseConstant.Action.DUYET,
                lstID);
                Cursor = Cursors.Arrow;
            }
        }

        private void ThoaiDuyet()
        {
            if (!tlbCancel.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            List<int> lstID = new List<int>();
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = false;
            try
            {
                if (raddgrSanPham.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        TinDungTDProcess tindungProcess = new TinDungTDProcess();
                        List<ClientResponseDetail> ClientResponseDetail = new List<Presentation.Process.Common.ClientResponseDetail>();
                        List<TDTD_SAN_PHAM> lstSanPham = new List<TDTD_SAN_PHAM>();
                        List<KT_PHAN_HE_PLOAI> lstPhanHePloai = new List<KT_PHAN_HE_PLOAI>();

                        foreach (DataRowView dr in raddgrSanPham.SelectedItems)
                        {
                            TDTD_SAN_PHAM objSP = new TDTD_SAN_PHAM();
                            objSP.ID = Convert.ToInt32(dr["ID"]);
                            lstSanPham.Add(objSP);
                            lstID.Add(objSP.ID);
                        }

                        // Yêu cầu lock dữ liệu
                        retLockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                                                        DatabaseConstant.Function.TDTD_SAN_PHAM,
                                                        DatabaseConstant.Table.TDTD_SAN_PHAM,
                                                        DatabaseConstant.Action.THOAI_DUYET,
                                                        lstID);

                        int iret = tindungProcess.SanPhamTinDung(DatabaseConstant.Action.THOAI_DUYET, ref lstSanPham, ref lstPhanHePloai, ref ClientResponseDetail);
                        CommonFunction.ThongBaoKetQua(ClientResponseDetail);
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
                DatabaseConstant.Function.TDTD_SAN_PHAM,
                DatabaseConstant.Table.TDTD_SAN_PHAM,
                DatabaseConstant.Action.THOAI_DUYET,
                lstID);
                Cursor = Cursors.Arrow;
            }
        }

        private void TuChoiDuyet()
        {
            if (!tlbRefuse.IsEnabled)
                return;
            Cursor = Cursors.Wait;
            List<int> lstID = new List<int>();
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = false;
            try
            {
                if (raddgrSanPham.SelectedItems.Count > 0)
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        TinDungTDProcess tindungProcess = new TinDungTDProcess();
                        List<ClientResponseDetail> ClientResponseDetail = new List<Presentation.Process.Common.ClientResponseDetail>();
                        List<TDTD_SAN_PHAM> lstSanPham = new List<TDTD_SAN_PHAM>();
                        List<KT_PHAN_HE_PLOAI> lstPhanHePloai = new List<KT_PHAN_HE_PLOAI>();

                        foreach (DataRowView dr in raddgrSanPham.SelectedItems)
                        {
                            TDTD_SAN_PHAM objSP = new TDTD_SAN_PHAM();
                            objSP.ID = Convert.ToInt32(dr["ID"]);
                            lstSanPham.Add(objSP);
                            lstID.Add(objSP.ID);
                        }

                        // Yêu cầu lock dữ liệu
                        retLockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                                                        DatabaseConstant.Function.TDTD_SAN_PHAM,
                                                        DatabaseConstant.Table.TDTD_SAN_PHAM,
                                                        DatabaseConstant.Action.TU_CHOI_DUYET,
                                                        lstID);

                        int iret = tindungProcess.SanPhamTinDung(DatabaseConstant.Action.TU_CHOI_DUYET, ref lstSanPham, ref lstPhanHePloai, ref ClientResponseDetail);
                        CommonFunction.ThongBaoKetQua(ClientResponseDetail);
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
                DatabaseConstant.Function.TDTD_SAN_PHAM,
                DatabaseConstant.Table.TDTD_SAN_PHAM,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstID);
                Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(raddgrSanPham);
            //PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel("GET_DS_DONVI");
        }

        private void objHDTDThoaThuan_OnSavingCompleted(object sender, EventArgs e)
        {
            LoadData();
        }
        #endregion
    }
}
