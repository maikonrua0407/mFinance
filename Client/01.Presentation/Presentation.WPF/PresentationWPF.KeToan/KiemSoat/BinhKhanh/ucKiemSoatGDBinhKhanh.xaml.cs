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
using Presentation.Process.KeToanServiceRef;
using Presentation.Process.Common;
using System.Reflection;

namespace PresentationWPF.KeToan.KiemSoat
{
    /// <summary>
    /// Interaction logic for ucPhanLoaiDS.xaml
    /// </summary>
    public partial class ucKiemSoatGDBinhKhanh : UserControl
    {
        #region Khai bao
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

        private List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();

        private DataSet dtMaPhanLoai = new DataSet();
        
        private List<int> listLockId = new List<int>();

        // Phân trang
        int StartRow = 1;
        int EndRow = ClientInformation.SoLuongBanGhi;
        int CurrentPagging = 1;
        int PaggingSize = ClientInformation.SoLuongBanGhi;
        #endregion

        #region Khoi tao
        public ucKiemSoatGDBinhKhanh()
        {
            InitializeComponent();
            //HeThong hethong = new HeThong();
            //hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KeToan;component/PhanLoai/ucPhanLoaiDS.xaml", ref Toolbar, ref mnuGrid);
            //foreach (var item in mnuGrid.Items)
            //{
            //    if (item is MenuItem)
            //        ((MenuItem)item).Click += btnShortcutKey_Click;
            //}
            BindHotkey();
            //radPage.PageSize = (int)nudPageSize.Value;
            radPage.PageIndexChanging += new EventHandler<PageIndexChangingEventArgs>(radPage_PageIndexChanging);
            InitCombobox();
            raddtNgayGiaoDichTu.Value = LDateTime.StringToDate(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            raddtNgayGiaoDichDen.Value = LDateTime.StringToDate(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            CreateTreePhanHeGD();
            exdTimKiemNangCao.Expanded += exdTimKiemNangCao_Expanded;
        }

        public void InitCombobox()
        {
            AutoComboBox au = new AutoComboBox();
            try
            {
                List<string> lstDK = new List<string>();
                lstDK.Add(Presentation.Process.Common.ClientInformation.IdDonViGiaoDich.ToString());
                au.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CHI_NHANH.getValue(), lstDK,ClientInformation.MaDonViGiaoDich);
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                au = null;
            }
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
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
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
                    if (key != null)
                        InputBindings.Add(key);
                }
            }
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbModify.IsEnabled;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeModify();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbDelete.IsEnabled;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeDelete();
        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbApprove.IsEnabled;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeApprove();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbRefuse.IsEnabled;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeRefuse();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbCancel.IsEnabled;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeCancel();
        }

        private void ViewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbView.IsEnabled;
        }
        private void ViewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeView();
        }

        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbSearch.IsEnabled;
        }
        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TimKiem();
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
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
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
                TimKiem();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
            {
                TimKiem();
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
                onClose();
            }
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
            PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(raddgrGiaoDichDS, txtTimKiemNhanh.Text);
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
            if (raddgrGiaoDichDS != null && raddgrGiaoDichDS.DataContext != null)
            {
                DataTable dt = ((DataView)raddgrGiaoDichDS.DataContext).Table;
                if (dt != null)
                {
                    radPage.PageSize = (int)nudPageSize.Value;
                    raddgrGiaoDichDS.DataContext = dt.DefaultView;
                }
            }
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(raddgrGiaoDichDS);
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

        private List<DataRowView> getListSeletedDataRow()
        {
            List<DataRowView> lstData = new List<DataRowView>();
            for (int i = 0; i < raddgrGiaoDichDS.Items.Count; i++)
            {
                DataRowView dr = (DataRowView)raddgrGiaoDichDS.Items[i];
                if (Convert.ToBoolean(dr["CHON"]) == true)
                {
                    lstData.Add(dr);
                }
            }
            return lstData;
        }

        private void chkAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < raddgrGiaoDichDS.Items.Count; i++)
            {
                DataRowView dr = (DataRowView)raddgrGiaoDichDS.Items[i];
                dr["CHON"] = chkAll.IsChecked;
            }
        }

        private void CreateTreePhanHeGD()
        {
            KeToanProcess process = new KeToanProcess();
            try
            {
                trvPhanHeGD.Items.Clear();
                DataSet ds = process.getTreePhanHeGD(ClientInformation.TenDangNhap,ClientInformation.MaDonViQuanLy);
                if (ds != null && ds.Tables.Count > 0)
                {
                    ds.Relations.Add("Master2Detail", ds.Tables[0].Columns["ID_PHAN_HE"], ds.Tables[0].Columns["ID_PHAN_HE_CHA"]);
                    DataView dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = "ID_PHAN_HE_CHA IS NULL";
                    trvPhanHeGD.ItemsSource = dv;

                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }
        }

        private void TimKiem()
        {
            AutoComboBox au = new AutoComboBox();
            string maDonVi = "";
            string maPhanHe = "";
            string trangThai = "";
            string tuNgay = "";
            string denNgay = "";
            string soGD = "";
            string loaiPhanHe = "";
            string soPhieu = "";

            if (trvPhanHeGD.SelectedItem != null)
            {
                DataRowView dr = (DataRowView)trvPhanHeGD.SelectedItem;
                maPhanHe = dr["ID_PHAN_HE"].ToString();
                loaiPhanHe = dr["TEN_BANG"].ToString();
            }

            maDonVi = au.getEntryByDisplayName(lstSourceDonVi, ref cmbDonVi).KeywordStrings[0];

            if (ucTrangThaiNVu.GetItemsSelected() != "NULL")
            {
                trangThai = ucTrangThaiNVu.GetItemsSelected();
            }

            if (raddtNgayGiaoDichTu.Value != null)
            {
                tuNgay = Convert.ToDateTime(raddtNgayGiaoDichTu.Value).ToString("yyyyMMdd");
            }

            if (raddtNgayGiaoDichDen.Value != null)
            {
                denNgay = Convert.ToDateTime(raddtNgayGiaoDichDen.Value).ToString("yyyyMMdd");
            }

            soGD = txtSoGD.Text;

            Mouse.OverrideCursor = Cursors.Wait;
            KeToanProcess process = new KeToanProcess();
            try
            {
                // Phân trang
                int StartRow = 1;
                int EndRow = ClientInformation.SoLuongBanGhi;
                int CurrentPagging = 1;
                int PaggingSize = ClientInformation.SoLuongBanGhi;
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@MaDonVi", "STRING", maDonVi);
                LDatatable.AddParameter(ref dt, "@MaPhanHe", "STRING", maPhanHe);
                LDatatable.AddParameter(ref dt, "@LoaiPhanHe", "STRING", loaiPhanHe);
                LDatatable.AddParameter(ref dt, "@TrangThai", "STRING", trangThai);
                LDatatable.AddParameter(ref dt, "@TuNgay", "STRING", tuNgay);
                LDatatable.AddParameter(ref dt, "@DenNgay", "STRING", denNgay);
                LDatatable.AddParameter(ref dt, "@SoGD", "STRING", soGD);
                LDatatable.AddParameter(ref dt, "@SoPhieu", "STRING", soPhieu);
                LDatatable.AddParameter(ref dt, "@MaKhachHang", "STRING", txtMaKhachHang.Text);
                LDatatable.AddParameter(ref dt, "@TenKhachHang", "STRING", txtTenKhachHang.Text);
                LDatatable.AddParameter(ref dt, "@SoKheUoc", "STRING", txtMaKheUoc.Text);
                LDatatable.AddParameter(ref dt, "@SoSoTK", "STRING", txtSoSoTK.Text);
                LDatatable.AddParameter(ref dt, "@StartRow", "INT", StartRow.ToString());
                LDatatable.AddParameter(ref dt, "@EndRow", "INT", EndRow.ToString());
                LDatatable.AddParameter(ref dt, "@UserName", "STRING", ClientInformation.TenDangNhap);
                LDatatable.AddParameter(ref dt, "@MaDViQLy", "STRING", ClientInformation.MaDonViQuanLy);
                DataSet ds = process.getDanhSachGiaoDich(dt);
                raddgrGiaoDichDS.ItemsSource = null;
                lblSumGD.Content = "0";
                lblSumSoTien.Content = "0";
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable serverDataTable = ds.Tables[0];
                    int totalRecord = Int32.Parse(ds.Tables[1].Rows[0][0].ToString());
                    DataTable clientDataTable = CommonFunction.BuildClientDataTable(serverDataTable, CurrentPagging, PaggingSize, totalRecord);

                    //raddgrGiaoDichDS.ItemsSource = ds.Tables[0].DefaultView;
                    //lblSumGD.Content = raddgrGiaoDichDS.Items.Count.ToString("###,###,###");

                    raddgrGiaoDichDS.ItemsSource = clientDataTable.DefaultView;
                    lblSumGD.Content = totalRecord;

                    decimal tongTienGD = 0;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        tongTienGD += Convert.ToDecimal(dr["TONG_TIEN"]);
                    }
                    lblSumSoTien.Content = tongTienGD.ToString("###,###,###,###");
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
                au = null;
            }

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void TimKiemPhanTrang()
        {
            AutoComboBox au = new AutoComboBox();
            string maDonVi = "";
            string maPhanHe = "";
            string trangThai = "";
            string tuNgay = "";
            string denNgay = "";
            string soGD = "";
            string loaiPhanHe = "";
            string soPhieu = "";

            if (trvPhanHeGD.SelectedItem != null)
            {
                DataRowView dr = (DataRowView)trvPhanHeGD.SelectedItem;
                maPhanHe = dr["ID_PHAN_HE"].ToString();
                loaiPhanHe = dr["TEN_BANG"].ToString();
            }

            maDonVi = au.getEntryByDisplayName(lstSourceDonVi, ref cmbDonVi).KeywordStrings[0];

            if (ucTrangThaiNVu.GetItemsSelected() != "NULL")
            {
                trangThai = ucTrangThaiNVu.GetItemsSelected();
            }

            if (raddtNgayGiaoDichTu.Value != null)
            {
                tuNgay = Convert.ToDateTime(raddtNgayGiaoDichTu.Value).ToString("yyyyMMdd");
            }

            if (raddtNgayGiaoDichDen.Value != null)
            {
                denNgay = Convert.ToDateTime(raddtNgayGiaoDichDen.Value).ToString("yyyyMMdd");
            }

            soGD = txtSoGD.Text;
            
            Mouse.OverrideCursor = Cursors.Wait;
            KeToanProcess process = new KeToanProcess();
            try
            {
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@MaDonVi", "STRING", maDonVi);
                LDatatable.AddParameter(ref dt, "@MaPhanHe", "STRING", maPhanHe);
                LDatatable.AddParameter(ref dt, "@LoaiPhanHe", "STRING", loaiPhanHe);
                LDatatable.AddParameter(ref dt, "@TrangThai", "STRING", trangThai);
                LDatatable.AddParameter(ref dt, "@TuNgay", "STRING", tuNgay);
                LDatatable.AddParameter(ref dt, "@DenNgay", "STRING", denNgay);
                LDatatable.AddParameter(ref dt, "@SoGD", "STRING", soGD);
                LDatatable.AddParameter(ref dt, "@SoPhieu", "STRING", soPhieu);
                LDatatable.AddParameter(ref dt, "@MaKhachHang", "STRING", txtMaKhachHang.Text);
                LDatatable.AddParameter(ref dt, "@TenKhachHang", "STRING", txtTenKhachHang.Text);
                LDatatable.AddParameter(ref dt, "@SoKheUoc", "STRING", txtMaKheUoc.Text);
                LDatatable.AddParameter(ref dt, "@SoSoTK", "STRING", txtSoSoTK.Text);
                LDatatable.AddParameter(ref dt, "@StartRow", "INT", StartRow.ToString());
                LDatatable.AddParameter(ref dt, "@EndRow", "INT", EndRow.ToString());
                LDatatable.AddParameter(ref dt, "@UserName", "STRING", ClientInformation.TenDangNhap);
                LDatatable.AddParameter(ref dt, "@MaDViQLy", "STRING", ClientInformation.MaDonViQuanLy);
                DataSet ds = process.getDanhSachGiaoDich(dt);
                raddgrGiaoDichDS.ItemsSource = null;
                lblSumGD.Content = "0";
                lblSumSoTien.Content = "0";
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable serverDataTable = ds.Tables[0];
                    int totalRecord = Int32.Parse(ds.Tables[1].Rows[0][0].ToString());
                    DataTable clientDataTable = CommonFunction.BuildClientDataTable(serverDataTable, CurrentPagging, PaggingSize, totalRecord);

                    //raddgrGiaoDichDS.ItemsSource = ds.Tables[0].DefaultView;
                    //lblSumGD.Content = raddgrGiaoDichDS.Items.Count.ToString("###,###,###");

                    raddgrGiaoDichDS.ItemsSource = clientDataTable.DefaultView;
                    lblSumGD.Content = totalRecord;

                    decimal tongTienGD = 0;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        tongTienGD += Convert.ToDecimal(dr["TONG_TIEN"]);
                    }
                    lblSumSoTien.Content = tongTienGD.ToString("###,###,###,###");
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
                au = null;
            }

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void btnLayDuLieu_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            KeToanProcess process = new KeToanProcess();
            AutoComboBox au = new AutoComboBox();

            string maDonVi = au.getEntryByDisplayName(lstSourceDonVi, ref cmbDonVi).KeywordStrings[0];
            string idGiaoDich = "";

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
                    idGiaoDich = listDataRow.First()["ID"].ToString();
                }
            }
            else
            {

            }

            try
            {
                DataSet ds = process.getDanhSachGiaoDichCT(maDonVi, idGiaoDich);
                raddgrGiaoDichCT.ItemsSource = null;
                lblSumNo.Content = "0";
                lblSumCo.Content = "0";
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    raddgrGiaoDichCT.ItemsSource = ds.Tables[0].DefaultView;
                    decimal tongNo = 0;
                    decimal tongCo = 0;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        tongNo += Convert.ToDecimal(dr["PSN"]);
                        tongCo += Convert.ToDecimal(dr["PSC"]);
                    }
                    lblSumNo.Content = tongNo.ToString("###,###,###,###");
                    lblSumCo.Content = tongCo.ToString("###,###,###,###");
                }
                exdGiaoDichCT.IsExpanded = true;
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                process = null;
                au = null;
            }
        }

        private void exdTimKiemNangCao_Expanded(object sender, RoutedEventArgs e)
        {
            exdGiaoDichCT.IsExpanded = false;
        }

        /// <summary>
        /// Hàm xử lý giải phóng lock dữ liệu cho form
        /// </summary>
        public void Release(DatabaseConstant.Action action = DatabaseConstant.Action.SUA)
        {
            if (listLockId.Count > 0)
            {
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
                    DatabaseConstant.Function.KT_GIAO_DICH,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    action,
                    listLockId);
            }
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

        private void radPage_PageIndexChanging(object sender, PageIndexChangingEventArgs e)
        {
            if (e.NewPageIndex < radPage.PageCount)
            {
                CurrentPagging = e.NewPageIndex + 1;
                StartRow = (CurrentPagging - 1) * PaggingSize + 1;
                EndRow = StartRow + PaggingSize;
                //radpage = new RadDataPager();
                TimKiemPhanTrang();
            }
        }

        #endregion

        #region  Xu ly nghiep vu
        private void BeforeView()
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
                    OnView(listDataRow.First());
                }
            }
            else
            {

            }
        }

        private void OnView(DataRowView drGiaoDich)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Utilities.Common.KIEM_SOAT obj = new Utilities.Common.KIEM_SOAT();
            obj.ID = Convert.ToInt32(drGiaoDich["ID"]);
            obj.action = DatabaseConstant.Action.XEM;
            obj.TTHAI_NVU = drGiaoDich["TTHAI_NVU"].ToString();
            obj.MA_LOAI_GD = drGiaoDich["MA_LOAI_GDICH"].ToString();
            obj.MA_PHAN_HE = drGiaoDich["MA_PHAN_HE"].ToString();
            obj.SO_GIAO_DICH = drGiaoDich["SO_GDICH"].ToString();

            UserControl p = null;
            bool stretchWindow = false;
            switch (drGiaoDich["TEN_FILE"].ToString())
            {
                case "PresentationWPF.HuyDongVon":
                    switch (drGiaoDich["MA_LOAI_GDICH"].ToString())
                    {
                        //Mở sổ tiết kiệm quy định
                        case "MS01":
                            p = new PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT(obj);
                            break;
                        //Mở sổ tiết kiệm không kỳ hạn
                        case "MS02":
                            p = new PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT(obj);
                            break;
                        //Mở sổ tiết kiệm có kỳ hạn
                        case "MS03":
                            p = new PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT(obj);
                            break;
                        //Mở sổ tiền gửi có kỳ hạn
                        case "MS04":
                            p = new PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT(obj);
                            break;
                        //Mở tài khoản thanh toán
                        case "MS05":
                            p = new PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT(obj);
                            break;
                        //Gửi thêm tiền theo từng sổ
                        case "GT01":
                            p = new PresentationWPF.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT(obj);
                            break;
                        //Gửi thêm tiền theo danh sách
                        case "GT02":
                            p = new PresentationWPF.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS(obj);
                            break;
                        //Rút gốc một phần
                        case "RG01":
                            p = new PresentationWPF.HuyDongVon.RutGoc.ucRutGocCT(obj);
                            break;
                        //Rút gốc một phần theo danh sách
                        case "RG02":
                            p = new PresentationWPF.HuyDongVon.RutGoc.ucRutGocTheoDS(obj);
                            break;
                        //Tất toán sổ tiền gửi
                        case "TT01":
                            p = new PresentationWPF.HuyDongVon.TatToan.ucTatToanCT(obj);
                            break;
                        //Tất toán sổ tiền gửi theo danh sách
                        case "TT02":
                            p = new PresentationWPF.HuyDongVon.TatToan.ucTatToanTheoDS(obj);
                            break;
                        //Phong tỏa tài khoản
                        case "PK01":
                            p = new PresentationWPF.HuyDongVon.PhongToaTK.ucPhongToaTKCT(obj);
                            break;
                        //Giải tỏa tài khoản
                        case "GK01":
                            p = new PresentationWPF.HuyDongVon.PhongToaTK.ucPhongToaTKCT(obj);
                            break;
                        //Trả lãi tiền gửi
                        case "TL01":
                            p = new PresentationWPF.HuyDongVon.TraLaiTienGui.ucTraLaiCT(obj);
                            break;
                        //Trả lãi tiền gửi theo danh sách
                        case "TL02":
                            p = new PresentationWPF.HuyDongVon.TraLaiTienGui.ucTraLaiTheoDS(obj);
                            break;
                        //Dự chi
                        case "DC01":
                            p = new PresentationWPF.HuyDongVon.DuChi.ucDuChiCT(obj);
                            break;
                        //Phân bổ chi phí
                        case "PC01":
                            p = new PresentationWPF.HuyDongVon.PhanBoChiPhi.ucPhanBoCT(obj);
                            break;
                        //Lãi nhập gốc theo từng sổ
                        case "LG01":
                            p = new PresentationWPF.HuyDongVon.LaiNhapGoc.ucLaiNhapGocCT(obj);
                            break;
                        //Lãi nhập gốc theo danh sách
                        case "LG02":
                            p = new PresentationWPF.HuyDongVon.LaiNhapGoc.ucLaiNhapGocTheoDS(obj);
                            break;
                        //Đóng tài khoản
                        case "DK01":
                            p = new PresentationWPF.HuyDongVon.DongTK.ucTKBiDongCT(obj);
                            break;
                        //Mở tài khoản
                        case "MK01":
                            p = new PresentationWPF.HuyDongVon.DongTK.ucTKBiDongCT(obj);
                            break;
                    }
                    break;
                case "PresentationWPF.TinDung":
                    switch (drGiaoDich["MA_LOAI_GDICH"].ToString())
                    {
                        //Giải ngân
                        case "GN01":
                            p = new PresentationWPF.TinDung.GiaiNgan.ucGiaiNganCT(obj);
                            break;
                        //Dự thu
                        case "DT01":
                            p = new PresentationWPF.TinDung.DuThu.ucDuThuCT(obj);
                            break;
                        //Trích lập dự phòng
                        case "DP01":
                            p = new PresentationWPF.TinDung.TrichLapDuPhong.ucTLDuPhongCT(obj);
                            break;
                        //Gia hạn nợ
                        case "GH01":
                            break;
                        //Chuyển hoàn nhóm nợ
                        case "CH01":
                            p = new PresentationWPF.TinDung.HoanNhomNo.ucHoanNhomNoCT(obj);
                            break;
                        //Chuyển nợ quá hạn
                        case "CN01":
                            p = new PresentationWPF.TinDung.ChuyenQuaHan.ucChuyenQuaHanCT(obj);
                            break;
                        case "XL01":
                            break;
                        //Hóa đơn thu tiền kỳ
                        case "HD01":
                            p = new PresentationWPF.TinDung.HoaDon.ucHDThuTienKyCTBinhKhanh(obj);
                            stretchWindow = true;
                            break;
                        //Thu gốc lãi trước hạn
                        case "TH01":
                            break;
                        //Phân bổ lãi vay
                        case "PL01":
                            break;
                    }
                    break;
                case "PresentationWPF.KeToan":
                    switch (drGiaoDich["MA_PHAN_HE"].ToString())
                    {
                        //Phiếu thu
                        case "PTHU":
                            switch (drGiaoDich["MA_LOAI_GDICH"].ToString())
                            {
                                case "PT0002":
                                    p = new PresentationWPF.KeToan.TiepQuy.ucHoanQuyCuoiNgay(obj, BusinessConstant.LOAI_CHUNG_TU.PHIEU_THU.layGiaTri());
                                    break;
                                default:
                                    p = new PresentationWPF.KeToan.PhieuKeToan.ucPhieuKeToanCT(obj, BusinessConstant.LOAI_CHUNG_TU.PHIEU_THU.layGiaTri());
                                    break;
                            }
                            break;
                        //Phiếu chi
                        case "PCHI":
                            switch (drGiaoDich["MA_LOAI_GDICH"].ToString())
                            {
                                case "PC0038":
                                    p = new PresentationWPF.KeToan.TiepQuy.ucTiepQuyDauNgay(obj, BusinessConstant.LOAI_CHUNG_TU.PHIEU_CHI.layGiaTri());
                                    break;
                                default:
                                    p = new PresentationWPF.KeToan.PhieuKeToan.ucPhieuKeToanCT(obj, BusinessConstant.LOAI_CHUNG_TU.PHIEU_CHI.layGiaTri());
                                    break;
                            }
                            break;
                        //Nhập xuất ngoại bảng
                        case "NBANG":
                            p = new PresentationWPF.KeToan.NgoaiBang.ucNgoaiBangCT(obj);
                            break;
                        //Phiếu kế toán
                        case "PKET":
                            switch (drGiaoDich["MA_LOAI_GDICH"].ToString())
                            {
                                case "KT0032":
                                    p = new PresentationWPF.KeToan.TiepQuy.ucTiepQuyDauNgay(obj, BusinessConstant.LOAI_CHUNG_TU.PHIEU_KETOAN.layGiaTri());
                                    break;
                                default:
                                    p = new PresentationWPF.KeToan.PhieuKeToan.ucPhieuKeToanCT(obj, BusinessConstant.LOAI_CHUNG_TU.PHIEU_KETOAN.layGiaTri());
                                    break;
                            }
                            break;
                        //Bút toán điều chỉnh
                        case "DCHINH":
                            p = new PresentationWPF.KeToan.DieuChinh.ucDieuChinhCT(obj);
                            break;
                        //Kết chuyển
                        case "KT06":
                            p = new PresentationWPF.KeToan.KetChuyen.ucKetChuyenCT();
                            break;
                    }
                    break;
                case "PresentationWPF.BaoHiem":
                    switch (drGiaoDich["MA_LOAI_GDICH"].ToString())
                    {
                        //Thu hộ phí xác lập thành viên
                        case "BH01":
                            break;
                        //Chi hộ phí xác lập thành viên
                        case "BH02":
                            break;
                        //Thu hộ phí bảo hiểm
                        case "BH03":
                            break;
                        //Chi hộ phí bảo hiểm
                        case "BH04":
                            break;
                    }
                    break;
            }

            if (p != null)
            {
                Window window = new Window
                {
                    Title = LLanguage.SearchResourceByKey(drGiaoDich["MA_NNGU"].ToString()),
                    WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                    //Icon = (rmb.ImageSource == null ? this.Icon : rmb.ImageSource),
                    Width = (Double.IsNaN(p.Width) ? 1024 : p.Width),
                    Height = (Double.IsNaN(p.Height) ? 700 : p.Height),
                    Content = p,
                };
                if (stretchWindow == true)
                {
                    window.WindowState = WindowState.Maximized;
                }
                p.Width = Double.NaN;
                p.Height = Double.NaN;

                Mouse.OverrideCursor = Cursors.Arrow;
                window.ShowDialog();
            }
            else
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                LMessage.ShowMessage("M.KeToan.KiemSoat.ucKiemSoatGD.KhongTimThayChucNang", LMessage.MessageBoxType.Warning);
            }
        }

        private void BeforeModify()
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
                    // Yêu cầu lock bản ghi cần xử lý
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(Convert.ToInt32(listDataRow.First()["ID"]));

                    bool ret = process.LockData(DatabaseConstant.Module.GDKT,
                        DatabaseConstant.Function.KT_GIAO_DICH,
                        DatabaseConstant.Table.KT_GIAO_DICH,
                        DatabaseConstant.Action.SUA,
                        listLockId);

                    // Nếu lock thành công >> cho phép xử lý
                    if (ret)
                    {
                        OnModify(listDataRow.First(), listLockId);
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

            }
        }

        private void OnModify(DataRowView drGiaoDich, List<int> listLockId)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Utilities.Common.KIEM_SOAT obj = new Utilities.Common.KIEM_SOAT();
            obj.ID = Convert.ToInt32(drGiaoDich["ID"]);
            obj.action = DatabaseConstant.Action.SUA;
            obj.TTHAI_NVU = drGiaoDich["TTHAI_NVU"].ToString();
            obj.MA_LOAI_GD = drGiaoDich["MA_LOAI_GDICH"].ToString();
            obj.MA_PHAN_HE = drGiaoDich["MA_PHAN_HE"].ToString();
            obj.SO_GIAO_DICH = drGiaoDich["SO_GDICH"].ToString();

            UserControl p = null;
            bool stretchWindow = false;
            switch (drGiaoDich["TEN_FILE"].ToString())
            {
                case "PresentationWPF.HuyDongVon":
                    switch (drGiaoDich["MA_LOAI_GDICH"].ToString())
                    {
                        //Mở sổ tiết kiệm quy định
                        case "MS01":
                            p = new PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT(obj);
                            break;
                        //Mở sổ tiết kiệm không kỳ hạn
                        case "MS02":
                            p = new PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT(obj);
                            break;
                        //Mở sổ tiết kiệm có kỳ hạn
                        case "MS03": 
                            p = new PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT(obj);
                            break;
                        //Mở sổ tiền gửi có kỳ hạn
                        case "MS04":
                            p = new PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT(obj);
                            break;
                        //Mở tài khoản thanh toán
                        case "MS05":
                            p = new PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT(obj);
                            break;
                        //Gửi thêm tiền theo từng sổ
                        case "GT01":
                            p = new PresentationWPF.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT(obj);
                            break;
                        //Gửi thêm tiền theo danh sách
                        case "GT02":
                            p = new PresentationWPF.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS(obj);
                            break;
                        //Rút gốc một phần
                        case "RG01":
                            p = new PresentationWPF.HuyDongVon.RutGoc.ucRutGocCT(obj);
                            break;
                        //Rút gốc một phần theo danh sách
                        case "RG02":
                            p = new PresentationWPF.HuyDongVon.RutGoc.ucRutGocTheoDS(obj);
                            break;
                        //Tất toán sổ tiền gửi
                        case "TT01":
                            p = new PresentationWPF.HuyDongVon.TatToan.ucTatToanCT(obj);
                            break;
                        //Tất toán sổ tiền gửi theo danh sách
                        case "TT02":
                            p = new PresentationWPF.HuyDongVon.TatToan.ucTatToanTheoDS(obj);
                            break;
                        //Phong tỏa tài khoản
                        case "PK01":
                            p = new PresentationWPF.HuyDongVon.PhongToaTK.ucPhongToaTKCT(obj);
                            break;
                        //Giải tỏa tài khoản
                        case "GK01":
                            p = new PresentationWPF.HuyDongVon.PhongToaTK.ucPhongToaTKCT(obj);
                            break;
                        //Trả lãi tiền gửi
                        case "TL01":
                            p = new PresentationWPF.HuyDongVon.TraLaiTienGui.ucTraLaiCT(obj);
                            break;
                        //Trả lãi tiền gửi theo danh sách
                        case "TL02":
                            p = new PresentationWPF.HuyDongVon.TraLaiTienGui.ucTraLaiTheoDS(obj);
                            break;
                        //Dự chi
                        case "DC01":
                            p = new PresentationWPF.HuyDongVon.DuChi.ucDuChiCT(obj);
                            break;
                        //Phân bổ chi phí
                        case "PC01":
                            p = new PresentationWPF.HuyDongVon.PhanBoChiPhi.ucPhanBoCT(obj);
                            break;
                        //Lãi nhập gốc theo từng sổ
                        case "LG01":
                            p = new PresentationWPF.HuyDongVon.LaiNhapGoc.ucLaiNhapGocCT(obj);
                            break;
                        //Lãi nhập gốc theo danh sách
                        case "LG02":
                            p = new PresentationWPF.HuyDongVon.LaiNhapGoc.ucLaiNhapGocTheoDS(obj);
                            break;
                        //Đóng tài khoản
                        case "DK01":
                            p = new PresentationWPF.HuyDongVon.DongTK.ucTKBiDongCT(obj);
                            break;
                        //Mở tài khoản
                        case "MK01":
                            p = new PresentationWPF.HuyDongVon.DongTK.ucTKBiDongCT(obj);
                            break;
                    }
                    break;
                case "PresentationWPF.TinDung":
                    switch (drGiaoDich["MA_LOAI_GDICH"].ToString())
                    {
                        //Giải ngân
                        case "GN01":
                            p = new PresentationWPF.TinDung.GiaiNgan.ucGiaiNganCT(obj);
                            break;
                        //Dự thu
                        case "DT01":
                            p = new PresentationWPF.TinDung.DuThu.ucDuThuCT(obj);
                            break;
                        //Trích lập dự phòng
                        case "DP01":
                            p = new PresentationWPF.TinDung.TrichLapDuPhong.ucTLDuPhongCT(obj);
                            break;
                        //Gia hạn nợ
                        case "GH01":
                            break;
                        //Chuyển hoàn nhóm nợ
                        case "CH01":
                            p = new PresentationWPF.TinDung.HoanNhomNo.ucHoanNhomNoCT(obj);
                            break;
                        //Chuyển nợ quá hạn
                        case "CN01":
                            p = new PresentationWPF.TinDung.ChuyenQuaHan.ucChuyenQuaHanCT(obj);
                            break;
                        //Xử lý nợ
                        case "XL01":
                            break;
                        //Hóa đơn thu tiền kỳ
                        case "HD01":
                            p = new PresentationWPF.TinDung.HoaDon.ucHDThuTienKyCTBinhKhanh(obj);
                            stretchWindow = true;
                            break;
                        //Thu gốc lãi trước hạn
                        case "TH01":
                            break;
                        //Phân bổ lãi vay
                        case "PL01":
                            break;
                    }
                    break;
                case "PresentationWPF.KeToan":
                    switch (drGiaoDich["MA_PHAN_HE"].ToString())
                    {
                        //Phiếu thu
                        case "PTHU":
                            switch (drGiaoDich["MA_LOAI_GDICH"].ToString())
                            {
                                case "PT0002" :
                                p = new PresentationWPF.KeToan.TiepQuy.ucHoanQuyCuoiNgay(obj, BusinessConstant.LOAI_CHUNG_TU.PHIEU_THU.layGiaTri());
                                break;
                                default:
                                p = new PresentationWPF.KeToan.PhieuKeToan.ucPhieuKeToanCT(obj, BusinessConstant.LOAI_CHUNG_TU.PHIEU_THU.layGiaTri());
                                break;
                            }
                            break;    
                        //Phiếu chi
                        case "PCHI":
                            switch (drGiaoDich["MA_LOAI_GDICH"].ToString())
                            {
                                case "PC0038":
                                    p = new PresentationWPF.KeToan.TiepQuy.ucTiepQuyDauNgay(obj, BusinessConstant.LOAI_CHUNG_TU.PHIEU_CHI.layGiaTri());
                                    break;
                                default:
                                p = new PresentationWPF.KeToan.PhieuKeToan.ucPhieuKeToanCT(obj, BusinessConstant.LOAI_CHUNG_TU.PHIEU_CHI.layGiaTri());
                                break;
                            }
                            break;
                        //Nhập xuất ngoại bảng
                        case "NBANG":
                            p = new PresentationWPF.KeToan.NgoaiBang.ucNgoaiBangCT(obj);
                            break;
                        //Phiếu kế toán
                        case "PKET":
                            switch (drGiaoDich["MA_LOAI_GDICH"].ToString())
                            {
                                case "KT0032":
                                    p = new PresentationWPF.KeToan.TiepQuy.ucTiepQuyDauNgay(obj, BusinessConstant.LOAI_CHUNG_TU.PHIEU_KETOAN.layGiaTri());
                                    break;
                                default:
                                p = new PresentationWPF.KeToan.PhieuKeToan.ucPhieuKeToanCT(obj, BusinessConstant.LOAI_CHUNG_TU.PHIEU_KETOAN.layGiaTri());
                                break;
                            }
                            break;
                        //Bút toán điều chỉnh
                        case "DCHINH":
                            p = new PresentationWPF.KeToan.DieuChinh.ucDieuChinhCT(obj);
                            break;
                        //Kết chuyển
                        case "KT06":
                            p = new PresentationWPF.KeToan.KetChuyen.ucKetChuyenCT();
                            break;
                    }
                    break;
                case "PresentationWPF.BaoHiem":
                    switch (drGiaoDich["MA_LOAI_GDICH"].ToString())
                    {
                        //Thu hộ phí xác lập thành viên
                        case "BH01":
                            break;
                        //Chi hộ phí xác lập thành viên
                        case "BH02":
                            break;
                        //Thu hộ phí bảo hiểm
                        case "BH03":
                            break;
                        //Chi hộ phí bảo hiểm
                        case "BH04":
                            break;
                    }
                    break;
            }

            if (p != null)
            {
                Window window = new Window
                {
                    Title = LLanguage.SearchResourceByKey(drGiaoDich["MA_NNGU"].ToString()),
                    WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                    //Icon = (rmb.ImageSource == null ? this.Icon : rmb.ImageSource),
                    Width = (Double.IsNaN(p.Width) ? 1024 : p.Width),
                    Height = (Double.IsNaN(p.Height) ? 700 : p.Height),
                    Content = p,
                };
                if (stretchWindow == true)
                {
                    window.WindowState = WindowState.Maximized;
                }
                p.Width = Double.NaN;
                p.Height = Double.NaN;
                Mouse.OverrideCursor = Cursors.Arrow;
                window.ShowDialog();
            }
            else
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                LMessage.ShowMessage("M.KeToan.KiemSoat.ucKiemSoatGD.KhongTimThayChucNang", LMessage.MessageBoxType.Warning);
            }
            AfterModify(listLockId);
        }

        private void AfterModify(List<int> listLockId)
        {
            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_GIAO_DICH,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        private void BeforeDelete()
        {
            listLockId.Clear();
            listLockId = GetListIdLock(((DataView)raddgrGiaoDichDS.ItemsSource).ToTable());
            if (listLockId == null || listLockId.Count == 0)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                MessageBoxResult result = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);
                if (result == MessageBoxResult.Yes)
                {
                    // Yêu cầu lock bản ghi cần xử lý
                    UtilitiesProcess process = new UtilitiesProcess();

                    bool ret = process.LockData(DatabaseConstant.Module.GDKT,
                        DatabaseConstant.Function.KT_GIAO_DICH,
                        DatabaseConstant.Table.KT_GIAO_DICH,
                        DatabaseConstant.Action.XOA,
                        listLockId);

                    // Nếu lock thành công >> cho phép xử lý
                    if (ret)
                    {
                        OnAction(DatabaseConstant.Action.XOA);
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                        return;
                    }
                }
            }
        }

        private void BeforeApprove()
        {
            listLockId.Clear();
            listLockId = GetListIdLock(((DataView)raddgrGiaoDichDS.ItemsSource).ToTable());
            if (listLockId == null || listLockId.Count == 0)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                MessageBoxResult result = LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question);
                if (result == MessageBoxResult.Yes)
                {
                    // Yêu cầu lock bản ghi cần xử lý
                    UtilitiesProcess process = new UtilitiesProcess();

                    bool ret = process.LockData(DatabaseConstant.Module.GDKT,
                        DatabaseConstant.Function.KT_GIAO_DICH,
                        DatabaseConstant.Table.KT_GIAO_DICH,
                        DatabaseConstant.Action.DUYET,
                        listLockId);

                    // Nếu lock thành công >> cho phép xử lý
                    if (ret)
                    {
                        OnAction(DatabaseConstant.Action.DUYET);
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                        return;
                    }
                }
            }
        }

        private void BeforeRefuse()
        {
            listLockId.Clear();
            listLockId = GetListIdLock(((DataView)raddgrGiaoDichDS.ItemsSource).ToTable());
            if (listLockId == null || listLockId.Count == 0)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                MessageBoxResult result = LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question);
                if (result == MessageBoxResult.Yes)
                {
                    // Yêu cầu lock bản ghi cần xử lý
                    UtilitiesProcess process = new UtilitiesProcess();

                    bool ret = process.LockData(DatabaseConstant.Module.GDKT,
                        DatabaseConstant.Function.KT_GIAO_DICH,
                        DatabaseConstant.Table.KT_GIAO_DICH,
                        DatabaseConstant.Action.TU_CHOI_DUYET,
                        listLockId);

                    // Nếu lock thành công >> cho phép xử lý
                    if (ret)
                    {
                        OnAction(DatabaseConstant.Action.TU_CHOI_DUYET);
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                        return;
                    }
                }
            }
        }

        private void BeforeCancel()
        {
            listLockId.Clear();
            listLockId = GetListIdLock(((DataView)raddgrGiaoDichDS.ItemsSource).ToTable());
            if (listLockId == null || listLockId.Count == 0)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                MessageBoxResult result = LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question);
                if (result == MessageBoxResult.Yes)
                {
                    // Yêu cầu lock bản ghi cần xử lý
                    UtilitiesProcess process = new UtilitiesProcess();

                    bool ret = process.LockData(DatabaseConstant.Module.GDKT,
                        DatabaseConstant.Function.KT_GIAO_DICH,
                        DatabaseConstant.Table.KT_GIAO_DICH,
                        DatabaseConstant.Action.THOAI_DUYET,
                        listLockId);

                    // Nếu lock thành công >> cho phép xử lý
                    if (ret)
                    {
                        OnAction(DatabaseConstant.Action.THOAI_DUYET);
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                        return;
                    }
                }
            }
        }

        private void OnAction(DatabaseConstant.Action action)
        {
            Presentation.Process.KeToanProcess process = new Presentation.Process.KeToanProcess();
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                frmLyDo frm = new frmLyDo();
                frm.dtGiaoDich = GetDataOnForm();
                frm.action = action;
                frm.GetData = new frmLyDo.AfterProcess(AfterAction);
                frm.ProcessUnlockData = new frmLyDo.AfterProcessException(Release);
                switch (action)
                {
                    case DatabaseConstant.Action.DUYET:
                        frm.Title = "Lý do duyệt";
                        break;
                    case DatabaseConstant.Action.TU_CHOI_DUYET:
                        frm.Title = "Lý do từ chối duyệt";
                        break;
                    case DatabaseConstant.Action.THOAI_DUYET:
                        frm.Title = "Lý do thoái duyệt";
                        break;
                }
                if (action == DatabaseConstant.Action.XOA)
                {
                    frm.Close();
                }
                else
                {
                    frm.ShowDialog();
                }
            }
            catch (System.Exception ex)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }
        }

        public void AfterAction(DatabaseConstant.Action action, ApplicationConstant.ResponseStatus responseStatus, List<ClientResponseDetail> lstResponseDetail)
        {
            if (responseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);
            }
            else
            {
                CommonFunction.ThongBaoKetQua(lstResponseDetail);
            }
            TimKiem();

            // Yêu cầu unlock dữ liệu
            //UtilitiesProcess process = new UtilitiesProcess();

            //bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
            //    DatabaseConstant.Function.KT_GIAO_DICH,
            //    DatabaseConstant.Table.KT_GIAO_DICH,
            //    action,
            //    listLockId);
        }

        private DataTable GetDataOnForm()
        {
            DataTable dtSource = ((DataView)raddgrGiaoDichDS.ItemsSource).ToTable();
            DataTable dt = new DataTable();
            dt = dtSource.Clone();
            foreach (DataRow dr in dtSource.Select("CHON = 1"))
            {
                dt.ImportRow(dr);
            }
            return dt;
        }

        private List<int> GetListIdLock(DataTable dt)
        {
            List<int> listLockId = new List<int>();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["CHON"]!= DBNull.Value && Convert.ToBoolean(dr["CHON"]) == true)
                {
                    listLockId.Add(Convert.ToInt32(dr["ID"]));
                }
            }
            return listLockId;
        }
        #endregion
    }
}
