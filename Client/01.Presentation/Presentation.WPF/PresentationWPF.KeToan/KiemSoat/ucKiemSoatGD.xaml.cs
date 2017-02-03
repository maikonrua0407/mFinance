using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
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
    public partial class ucKiemSoatGD : UserControl
    {
        #region Khai bao
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand ViewCommand = new RoutedCommand();
        public static RoutedCommand ViewDetailCommand = new RoutedCommand();
        public static RoutedCommand SearchCommand = new RoutedCommand();
        public static RoutedCommand ReloadCommand = new RoutedCommand();
        public static RoutedCommand ExportCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        public string gIdGiaoDich = "";

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
        public ucKiemSoatGD()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KeToan;component/KiemSoat/ucKiemSoatGD.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindHotkey();
            //radPage.PageSize = (int)nudPageSize.Value;
            radPage.PageIndexChanging += new EventHandler<PageIndexChangingEventArgs>(radPage_PageIndexChanging);
            InitCombobox();
            raddtNgayGiaoDichTu.Value = LDateTime.StringToDate(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            raddtNgayGiaoDichDen.Value = LDateTime.StringToDate(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            CreateTreePhanHeGD();

            raddgrGiaoDichDS.MouseDoubleClick += new MouseButtonEventHandler(raddgrGiaoDichDS_MouseDoubleClick);
            raddgrGiaoDichDS.SelectionChanged +=new EventHandler<SelectionChangeEventArgs>(raddgrGiaoDichDS_SelectionChanged);
            exdTimKiemNangCao.Expanded +=new RoutedEventHandler(exdTimKiemNangCao_Expanded);
        }

        public void InitCombobox()
        {
            AutoComboBox au = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            try
            {
                //List<string> lstDK = new List<string>();
                //lstDK.Add(Presentation.Process.Common.ClientInformation.IdDonViGiaoDich.ToString());
                //au.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CHI_NHANH.getValue(), lstDK,ClientInformation.MaDonViGiaoDich);

                lstDieuKien.Add(ClientInformation.TenDangNhap);
                lstDieuKien.Add(ClientInformation.MaDonVi);
                lstDieuKien.Add(DatabaseConstant.ToChucDonVi.PGD.getValue());
                au.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVI_THEOPVI.getValue(), lstDieuKien, ClientInformation.MaDonViGiaoDich);
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                au = null;
                lstDieuKien = null;
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_CHI_TIET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.D, ModifierKeys.Control);
                        key = new KeyBinding(ViewDetailCommand, keyg);
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

        private void ViewDetailCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbViewDetail.IsEnabled;
        }
        private void ViewDetailCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnViewDetail();
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
            //string strTinhNang = "";

            //if (sender is RibbonButton)
            //    strTinhNang = ((RibbonButton)sender).Name.Substring(3, ((RibbonButton)sender).Name.Length - 3);
            //else
            //    strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);

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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_CHI_TIET)))
            {
                OnViewDetail();
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

        /// <summary>
        /// Sự kiện shortcutkey
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_CHI_TIET)))
            {
                OnViewDetail();
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
            for (int i = 0; i < raddgrGiaoDichDS.SelectedItems.Count; i++)
            {
                //DataRow dr = (DataRow)grCum.SelectedItems[i];
                //lstMaCum.Add(dr["MA_CUM"].ToString());

                //DataRowView dr = (DataRowView)raddgrGiaoDichDS.Items[i];
                lstData.Add((DataRowView)raddgrGiaoDichDS.SelectedItems[i]);
            }

            //for (int i = 0; i < raddgrGiaoDichDS.Items.Count; i++)
            //{
            //    DataRowView dr = (DataRowView)raddgrGiaoDichDS.Items[i];
            //    if (Convert.ToBoolean(dr["CHON"]) == true)
            //    {
            //        lstData.Add(dr);
            //    }
            //}
            return lstData;
        }

        private void chkAll_Click(object sender, RoutedEventArgs e)
        {
            //for (int i = 0; i < raddgrGiaoDichDS.Items.Count; i++)
            //{
            //    DataRowView dr = (DataRowView)raddgrGiaoDichDS.Items[i];
            //    dr["CHON"] = chkAll.IsChecked;
            //}
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
            Mouse.OverrideCursor = Cursors.Wait;
            KeToanProcess process = new KeToanProcess();
            AutoComboBox au = new AutoComboBox();
            try
            {
                string maDonVi = "(";
                string maPhanHe = "";
                string trangThai = "";
                string tuNgay = "";
                string denNgay = "";
                string soGD = "";
                string loaiPhanHe = "";
                string soPhieu = "";

                string maChon = lstSourceDonVi.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.ElementAt(0);
                string donVi = "";
                foreach (AutoCompleteEntry item in lstSourceDonVi)
                {
                    donVi = item.KeywordStrings.ElementAt(0);
                    if (donVi.Contains(maChon))
                    {
                        maDonVi += "''" + donVi + "'',";
                    }
                }
                maDonVi = maDonVi.Substring(0, maDonVi.Length - 1) + ")";

                if (trvPhanHeGD.SelectedItem != null)
                {
                    DataRowView dr = (DataRowView)trvPhanHeGD.SelectedItem;
                    maPhanHe = dr["ID_PHAN_HE"].ToString();
                    loaiPhanHe = dr["TEN_BANG"].ToString();
                }

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
                soPhieu = txtSoPhieu.Text;


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
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        dr["TRANG_THAI"] = BusinessConstant.layNgonNguNghiepVu(dr["TRANG_THAI"].ToString());

                    Dispatcher.CurrentDispatcher.DelayInvoke("BuildClientDataTable", () =>
                    {
                        DataTable serverDataTable = ds.Tables[0];
                        int totalRecord = Int32.Parse(ds.Tables[1].Rows[0][0].ToString());
                        DataTable clientDataTable = CommonFunction.BuildClientDataTable(serverDataTable, CurrentPagging, PaggingSize, totalRecord);

                        //raddgrGiaoDichDS.ItemsSource = ds.Tables[0].DefaultView;
                        //lblSumGD.Content = raddgrGiaoDichDS.Items.Count.ToString("###,###,###");

                        raddgrGiaoDichDS.ItemsSource = clientDataTable.DefaultView;
                        lblSumGD.Content = totalRecord;
                    }, TimeSpan.FromSeconds(0));


                    Dispatcher.CurrentDispatcher.DelayInvoke("StatusBar", () =>
                    {
                        decimal tongTienGD = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            tongTienGD += Convert.ToDecimal(dr["TONG_TIEN"]);
                        }
                        lblSumSoTien.Content = tongTienGD.ToString("###,###,###,###");
                    }, TimeSpan.FromSeconds(0));
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
                Mouse.OverrideCursor = Cursors.Arrow;
            }


        }

        private void TimKiemPhanTrang()
        {
            AutoComboBox au = new AutoComboBox();
            string maDonVi = "(";
            string maPhanHe = "";
            string trangThai = "";
            string tuNgay = "";
            string denNgay = "";
            string soGD = "";
            string loaiPhanHe = "";
            string soPhieu = "";
            string maChon = lstSourceDonVi.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.ElementAt(0);
            string donVi = "";
            foreach (AutoCompleteEntry item in lstSourceDonVi)
            {
                donVi = item.KeywordStrings.ElementAt(0);
                if (donVi.Contains(maChon))
                {
                    maDonVi += "''" + donVi + "'',";
                }
            }
            maDonVi = maDonVi.Substring(0, maDonVi.Length - 1) + ")";

            if (trvPhanHeGD.SelectedItem != null)
            {
                DataRowView dr = (DataRowView)trvPhanHeGD.SelectedItem;
                maPhanHe = dr["ID_PHAN_HE"].ToString();
                loaiPhanHe = dr["TEN_BANG"].ToString();
            }

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
            soPhieu = txtSoPhieu.Text;
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
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        dr["TRANG_THAI"] = BusinessConstant.layNgonNguNghiepVu(dr["TRANG_THAI"].ToString());

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

        private void raddgrGiaoDichDS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OnViewDetail();
        }

        private void raddgrGiaoDichDS_Click(object sender, MouseButtonEventArgs e)
        {
            OnViewDetail();
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
                EndRow = StartRow + PaggingSize - 1;
                //radpage = new RadDataPager();
                TimKiemPhanTrang();
            }
        }

        private void raddgrGiaoDichDS_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                string idGiaoDich = "";

                List<DataRowView> listDataRow = getListSeletedDataRow();

                if (listDataRow != null && listDataRow.Count>0)
                {
                    idGiaoDich = listDataRow.First()["ID"].ToString();
                    if (!gIdGiaoDich.Equals(idGiaoDich))
                    {
                        raddgrGiaoDichCT.ItemsSource = null;
                        exdGiaoDichCT.IsExpanded = false;
                    }
                }               
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        #endregion

        #region  Xu ly nghiep vu
        private void BeforeView()
        {
            Mouse.OverrideCursor = Cursors.Wait;
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
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void OnView(DataRowView drGiaoDich)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Utilities.Common.KIEM_SOAT obj = new Utilities.Common.KIEM_SOAT();
                obj.ID = Convert.ToInt32(drGiaoDich["ID"]);
                obj.action = DatabaseConstant.Action.XEM;
                obj.TTHAI_NVU = drGiaoDich["TTHAI_NVU"].ToString();
                obj.MA_LOAI_GD = drGiaoDich["MA_LOAI_GDICH"].ToString();
                obj.MA_PHAN_HE = drGiaoDich["MA_PHAN_HE"].ToString();
                obj.SO_GIAO_DICH = drGiaoDich["SO_GDICH"].ToString();
                obj.MA_TCHIEU = drGiaoDich["MA_TCHIEU"].ToString();

                UserControl p = null;
                bool stretchWindow = false;
                CallForm(ref obj, ref p, ref stretchWindow, drGiaoDich, DatabaseConstant.Action.XEM);

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
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void OnViewDetail()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            KeToanProcess process = new KeToanProcess();
            AutoComboBox au = new AutoComboBox();
            try
            {
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
                        gIdGiaoDich = idGiaoDich;
                    }
                }
                else
                {

                }

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
                process = null;
                au = null;
                Mouse.OverrideCursor = Cursors.Arrow;
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
                    if (listDataRow.First()["TTHAI_NVU"].ToString().Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.TrangThaiNghiepVuKhongPhuHop", LMessage.MessageBoxType.Warning);
                        return;
                    }
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
            obj.MA_TCHIEU = drGiaoDich["MA_TCHIEU"].ToString();

            UserControl p = null;
            bool stretchWindow = false;
            CallForm(ref obj, ref p, ref stretchWindow, drGiaoDich, DatabaseConstant.Action.SUA);

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
                for (int i = 0; i < raddgrGiaoDichDS.SelectedItems.Count; i++)
                {
                    DataRowView dr = (DataRowView)raddgrGiaoDichDS.SelectedItems[i];
                    if (listLockId.Contains((Convert.ToInt32(dr["ID"]))) && dr["TRANG_THAI"].ToString().Equals(BusinessConstant.layNgonNguNghiepVu(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri())))
                    {
                        LMessage.ShowMessage("Giao dịch " + dr["SO_GDICH"].ToString() + " đã duyệt, không được xóa.", LMessage.MessageBoxType.Warning);
                        return;
                    }
                }
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
                        frm.Title = LLanguage.SearchResourceByKey("U.KeToan.KiemSoat.frmLyDo.LyDoDuyet");
                        break;
                    case DatabaseConstant.Action.TU_CHOI_DUYET:
                        frm.Title = LLanguage.SearchResourceByKey("U.KeToan.KiemSoat.frmLyDo.LyDoTuChoiDuyet");
                        break;
                    case DatabaseConstant.Action.THOAI_DUYET:
                        frm.Title = LLanguage.SearchResourceByKey("U.KeToan.KiemSoat.frmLyDo.LyDoThoaiDuyet");
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
                if(action == DatabaseConstant.Action.XOA)
                    LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);

                else if (action == DatabaseConstant.Action.DUYET)
                    LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);

                else if (action == DatabaseConstant.Action.TU_CHOI_DUYET)
                    LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);

                else if (action == DatabaseConstant.Action.THOAI_DUYET)
                    LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);

                else
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);
            }
            else
            {
                CommonFunction.ThongBaoKetQua(lstResponseDetail);
            }
            TimKiem();

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.KT_GIAO_DICH,
                DatabaseConstant.Table.KT_GIAO_DICH,
                action,
                listLockId);
        }

        private DataTable GetDataOnForm()
        {
            //DataTable dtSource = ((DataView)raddgrGiaoDichDS.ItemsSource).ToTable();
            //DataTable dt = new DataTable();
            //dt = dtSource.Clone();
            //foreach (DataRow dr in dtSource.Select("CHON = 1"))
            //{
            //    dt.ImportRow(dr);
            //}

            DataTable dtSource = ((DataView)raddgrGiaoDichDS.ItemsSource).ToTable();
            string filterCondition = "";
            for (int i = 0; i < raddgrGiaoDichDS.SelectedItems.Count; i++)
            {
                DataRowView dr = (DataRowView)raddgrGiaoDichDS.SelectedItems[i];
                filterCondition += (i==0)? (" ID = " + dr["ID"].ToString()) : (" OR ID = " + dr["ID"].ToString());
            }

            DataTable dt = new DataTable();
            dt = dtSource.Clone();
            foreach (DataRow dr in dtSource.Select(filterCondition))
            {
                dr["CHON"] = 1;
                dt.ImportRow(dr);
            }

            return dt;
        }

        private List<int> GetListIdLock(DataTable dt)
        {
            List<int> listLockId = new List<int>();
            //foreach (DataRow dr in dt.Rows)
            //{
                //if (dr["CHON"] != DBNull.Value && Convert.ToBoolean(dr["CHON"]) == true)
                //{
                    //listLockId.Add(Convert.ToInt32(dr["ID"]));
                //}
            //}
            //DataRowView drv = (DataRowView)raddgrGiaoDichDS.SelectedItem;

            for (int i = 0; i < raddgrGiaoDichDS.SelectedItems.Count; i++)
            {
                DataRowView dr = (DataRowView)raddgrGiaoDichDS.SelectedItems[i];
                listLockId.Add(Convert.ToInt32(dr["ID"]));
            }

            return listLockId;
        }

        void CallForm(ref Utilities.Common.KIEM_SOAT obj, ref UserControl p, ref bool stretchWindow, DataRowView drGiaoDich, DatabaseConstant.Action action)
        {
            try
            {
                ApplicationConstant.DonViSuDung donviSuDung = ApplicationConstant.layDonViSuDung(ClientInformation.Company);
                switch (drGiaoDich["TEN_FILE"].ToString())
                {
                    #region Huy động vốn
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
                            //Gửi thêm tiền theo danh sách Excel
                            case "GT03":
                                p = new PresentationWPF.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiExcel(obj);
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
                    #endregion

                    #region Tín dụng

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
                                p = new PresentationWPF.TinDung.XoaNo.ucXoaNoCT01(obj);
                                break;
                            //Hóa đơn thu tiền kỳ
                            case "HD01":
                                switch (donviSuDung)
                                {
                                    case ApplicationConstant.DonViSuDung.M7MFI:
                                        p = new PresentationWPF.TinDung.HoaDon.ucHDThuTienKyCT(obj);
                                        break;
                                    case ApplicationConstant.DonViSuDung.BANTAYVANG:
                                    case ApplicationConstant.DonViSuDung.PHUTHO:
                                    case ApplicationConstant.DonViSuDung.HOCVIENNGANHANG:
                                    case ApplicationConstant.DonViSuDung.BENTRE:
                                    case ApplicationConstant.DonViSuDung.QUANGBINH:
                                        p = new PresentationWPF.HoaDonTienKy.HoaDon.ucThuGocLaiVay(obj);
                                        break;
                                    case ApplicationConstant.DonViSuDung.BIDV:
                                        p = new PresentationWPF.HoaDonTienKy.HoaDon.ucThuGocLaiVay_01(obj);
                                        break;
                                    case ApplicationConstant.DonViSuDung.BIDV_BLF:
                                        p = new PresentationWPF.HoaDonTienKy.HoaDon.ucThuGocLaiVay_01(obj);
                                        break;
                                    default:
                                        p = new PresentationWPF.TinDung.HoaDon.ucHDThuTienKyCT(obj);
                                        break;
                                }
                                stretchWindow = true;
                                break;
                            //Thu gốc lãi trước hạn
                            case "TH01":
                                p = new PresentationWPF.HoaDonTienKy.ThuGocLai.ucThuGocLaiCT(obj);
                                break;
                            //Phân bổ lãi vay
                            case "PL01":
                                p = new PresentationWPF.TinDung.PhanBo.ucPhanBoCT(obj);
                                break;
                            //Tạm ứng giải ngân
                            case "TU01":
                                p = new PresentationWPF.TinDung.TamUng.ucTamUngCT(obj);
                                break;
                            //Hoàn ứng giải ngân
                            case "HU01":
                                p = new PresentationWPF.TinDung.HoanUng.ucHoanUngCT(obj);
                                break;
                            case "PN01":
                                p = new PresentationWPF.TinDung.PhanLoaiNo.ucPhanLoaiNoCT(obj);
                                break;
                        }
                        break;

                    #endregion

                    #region Kế toán

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
                                        switch (donviSuDung)
                                        {
                                            case ApplicationConstant.DonViSuDung.BIDV:
                                                ClientInformation.FormCase = BusinessConstant.LOAI_CHUNG_TU.PHIEU_THU.layGiaTri();
                                                p = new PresentationWPF.KeToan.PhieuKeToan.ucPhieuKeToanNgoaiTeCT(obj, BusinessConstant.LOAI_CHUNG_TU.PHIEU_THU.layGiaTri());
                                                break;
                                            case ApplicationConstant.DonViSuDung.BIDV_BLF:
                                                ClientInformation.FormCase = BusinessConstant.LOAI_CHUNG_TU.PHIEU_THU.layGiaTri();
                                                p = new PresentationWPF.KeToan.PhieuKeToan.ucPhieuKeToanNgoaiTeCT(obj, BusinessConstant.LOAI_CHUNG_TU.PHIEU_THU.layGiaTri());
                                                break;
                                            default:
                                                ClientInformation.FormCase = BusinessConstant.LOAI_CHUNG_TU.PHIEU_THU.layGiaTri();
                                                p = new PresentationWPF.KeToan.PhieuKeToan.ucPhieuKeToanCT(obj, BusinessConstant.LOAI_CHUNG_TU.PHIEU_THU.layGiaTri());
                                                break;
                                        }
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
                                        switch (donviSuDung)
                                        {
                                            case ApplicationConstant.DonViSuDung.BIDV:
                                                ClientInformation.FormCase = BusinessConstant.LOAI_CHUNG_TU.PHIEU_CHI.layGiaTri();
                                                p = new PresentationWPF.KeToan.PhieuKeToan.ucPhieuKeToanNgoaiTeCT(obj, BusinessConstant.LOAI_CHUNG_TU.PHIEU_CHI.layGiaTri());
                                                break;
                                            case ApplicationConstant.DonViSuDung.BIDV_BLF:
                                                ClientInformation.FormCase = BusinessConstant.LOAI_CHUNG_TU.PHIEU_CHI.layGiaTri();
                                                p = new PresentationWPF.KeToan.PhieuKeToan.ucPhieuKeToanNgoaiTeCT(obj, BusinessConstant.LOAI_CHUNG_TU.PHIEU_CHI.layGiaTri());
                                                break;
                                            default:
                                                ClientInformation.FormCase = BusinessConstant.LOAI_CHUNG_TU.PHIEU_CHI.layGiaTri();
                                                p = new PresentationWPF.KeToan.PhieuKeToan.ucPhieuKeToanCT(obj, BusinessConstant.LOAI_CHUNG_TU.PHIEU_CHI.layGiaTri());
                                                break;
                                        }
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
                                    case "KT0033":
                                        p = new PresentationWPF.KeToan.DanhGiaNgoaiTe.ucDanhGiaNgoaiTe(obj);
                                        break;
                                    default:
                                        switch (donviSuDung)
                                        {
                                            case ApplicationConstant.DonViSuDung.BIDV:
                                                ClientInformation.FormCase = BusinessConstant.LOAI_CHUNG_TU.PHIEU_KETOAN.layGiaTri();
                                                p = new PresentationWPF.KeToan.PhieuKeToan.ucPhieuKeToanNgoaiTeCT(obj, BusinessConstant.LOAI_CHUNG_TU.PHIEU_KETOAN.layGiaTri());
                                                break;
                                            case ApplicationConstant.DonViSuDung.BIDV_BLF:
                                                ClientInformation.FormCase = BusinessConstant.LOAI_CHUNG_TU.PHIEU_KETOAN.layGiaTri();
                                                p = new PresentationWPF.KeToan.PhieuKeToan.ucPhieuKeToanNgoaiTeCT(obj, BusinessConstant.LOAI_CHUNG_TU.PHIEU_KETOAN.layGiaTri());
                                                break;
                                            default:
                                                ClientInformation.FormCase = BusinessConstant.LOAI_CHUNG_TU.PHIEU_KETOAN.layGiaTri();
                                                p = new PresentationWPF.KeToan.PhieuKeToan.ucPhieuKeToanCT(obj, BusinessConstant.LOAI_CHUNG_TU.PHIEU_KETOAN.layGiaTri());
                                                break;
                                        }
                                        break;
                                }
                                break;
                            //Bút toán điều chỉnh
                            case "DCHINH":
                                p = new PresentationWPF.KeToan.DieuChinh.ucDieuChinhCT(obj);
                                break;
                            //Kết chuyển
                            case "UNCHI":
                                //p = new PresentationWPF.KeToan.UyNhiemChi.ucPhieuUyNhiemChiCT(obj, BusinessConstant.LOAI_CHUNG_TU.PHIEU_KETOAN.layGiaTri());
                                p = new PresentationWPF.KeToan.UyNhiemChi.ucPhieuUyNhiemChiCT01(obj, BusinessConstant.LOAI_CHUNG_TU.PHIEU_KETOAN.layGiaTri());
                                break;
                            case "KCHUYEN":
                                p = new PresentationWPF.KeToan.KetChuyen.ucKetChuyenCT(obj);
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
                    case "PresentationWPF.NhanSu":
                        switch (drGiaoDich["MA_LOAI_GDICH"].ToString())
                        {
                            //Tính lương nhân viên
                            case "NS01":
                                p = new PresentationWPF.NhanSu.Luong.ucTinhLuongCT(obj);
                                break;
                            case "NS02":
                                //Tính phụ cấp cộng tác viên
                                p = new PresentationWPF.NhanSu.PhuCapCongTacVien.ucTinhPhuCapCT(obj);
                                break;
                        }
                        break;
                    #endregion

                    #region Quản lý tài sản

                    case "PresentationWPF.TaiSan":
                        switch (drGiaoDich["MA_LOAI_GDICH"].ToString())
                        {
                            //Nhập tài sản
                            case "NTS01":
                                p = new PresentationWPF.TaiSan.TaiSan.ucTangCT(obj.MA_TCHIEU, action);
                                break;
                            //Nâng cấp sửa chữa lớn
                            case "NCSC01":
                                p = new PresentationWPF.TaiSan.TaiSan.ucSuaChuaCT(obj.MA_TCHIEU, action);
                                break;
                            //Đánh giá lại
                            case "DGL01":
                                p = new PresentationWPF.TaiSan.TaiSan.ucDanhGiaLaiCT(obj.MA_TCHIEU, action);
                                break;
                            //Khấu hao
                            case "KH01":
                                p = new PresentationWPF.TaiSan.TaiSan.ucKhauHaoCT(obj.MA_TCHIEU, action);
                                break;
                            //Giảm tài sản do thanh lý
                            case "GTS01":
                                p = new PresentationWPF.TaiSan.TaiSan.ucGiamCT(obj.MA_TCHIEU, action);
                                break;
                            //Giảm tài sản do mất
                            case "GTS02":
                                p = new PresentationWPF.TaiSan.TaiSan.ucGiamCT(obj.MA_TCHIEU, action);
                                break;
                        }
                        break;
                    #endregion

                    #region Tài sản đảm bảo
                    case "PresentationWPF.TaiSanDamBao":
                        switch (drGiaoDich["MA_LOAI_GDICH"].ToString())
                        {
                            //Nhập tài sản
                            case "TS01":
                            case "TS02":
                                p = new PresentationWPF.TaiSanDamBao.ucHopDongTheChapNhap(obj);
                                break;
                        }
                        break;
                    #endregion

                    #region Tín dụng tieu dung

                    case "PresentationWPF.TinDungTD":
                        switch (drGiaoDich["MA_LOAI_GDICH"].ToString())
                        {
                            //Giải ngân
                            case "GN03":
                                if (ApplicationConstant.DonViSuDung.BIDV_BLF.Equals(donviSuDung))
                                    p = new PresentationWPF.TinDungTD.GiaiNgan.ucGiaiNganKheUocCT_LMF(obj);
                                else
                                    p = new PresentationWPF.TinDungTD.GiaiNgan.ucGiaiNganKheUocCT(obj);
                                break;
                            //Thu gốc lãi
                            case "HD03":
                                p = new PresentationWPF.TinDungTD.ThuGocLai.ucThuGocLaiCT(obj);
                                break;
                            //Chuyển hoàn nhóm nợ
                            case "CH03":
                                p = new PresentationWPF.TinDungTD.ChuyenHoanNhomNo.ucChuyenHoanNhomNoCT(obj);
                                break;
                            //Chuyển nợ quá hạn
                            case "CN03":
                                p = new PresentationWPF.TinDungTD.ChuyenNoQH.ucChuyenNoQHCT(obj);
                                break;
                            //Dự thu
                            case "DT03":
                                p = new PresentationWPF.TinDungTD.DuThu.ucDuThuCT(obj);
                                break;
                            //Trích lập dự phòng
                            case "DP03":
                                switch (donviSuDung)
                                {
                                    case ApplicationConstant.DonViSuDung.BIDV:
                                        p = new PresentationWPF.TinDungTD.TrichLapDuPhong.ucTLDuPhongCT(obj);
                                        break;
                                    case ApplicationConstant.DonViSuDung.BIDV_BLF:
                                        p = new PresentationWPF.TinDungTD.TrichLapDuPhong.ucTLDuPhongCT_01(obj);
                                        break;
                                    default:
                                        p = new PresentationWPF.TinDungTD.TrichLapDuPhong.ucTLDuPhongCT(obj);
                                        break;
                                }
                                p = new PresentationWPF.TinDungTD.TrichLapDuPhong.ucTLDuPhongCT(obj);
                                break;
                            case "NNB03":
                                p = new PresentationWPF.TinDungTD.TSDB.ucTaiSanDamBaoCT(obj);
                                break;
                            case "XNB03":
                                p = new PresentationWPF.TinDungTD.TSDB.ucTaiSanDamBaoCT(obj);
                                break;
                        }
                        break;

                    #endregion
                }
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
