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

namespace PresentationWPF.KeToan.TaiKhoan
{
    /// <summary>
    /// Interaction logic for ucPhanLoaiDS.xaml
    /// </summary>
    public partial class ucTaiKhoanNgoaiTeDS : UserControl
    {
        #region Khai bao
        public static RoutedCommand AddCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ViewCommand = new RoutedCommand();
        public static RoutedCommand SearchCommand = new RoutedCommand();
        public static RoutedCommand ReloadCommand = new RoutedCommand();
        public static RoutedCommand ExportCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        private List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();

        private DataSet dtMaPhanLoai = new DataSet();
        #endregion

        #region Khoi tao
        public ucTaiKhoanNgoaiTeDS()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KeToan;component/TaiKhoan/ucTaiKhoanNgoaiTeDS.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += tlbHotKey_Click;
            }
            BindHotkey();
            radPage.PageSize = (int)nudPageSize.Value;
            raddtSoDuNgay.Value = LDateTime.StringToDate(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            InitCombobox();
            TaoTreeView();
            cmbDonVi.SelectionChanged += new SelectionChangedEventHandler(cmbDonVi_SelectionChanged);
        }


        public void InitCombobox()
        {
            AutoComboBox au = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            try
            {
                //List<string> lstDK = new List<string>();
                //lstDK.Add(Presentation.Process.Common.ClientInformation.IdDonViGiaoDich.ToString());
                //au.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CHI_NHANH.getValue(), lstDK);

                lstDieuKien.Add(ClientInformation.TenDangNhap);
                lstDieuKien.Add(ClientInformation.MaDonVi);
                lstDieuKien.Add(DatabaseConstant.ToChucDonVi.PGD.getValue());
                au.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVI_THEOPVI.getValue(), lstDieuKien, ClientInformation.MaDonViGiaoDich);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
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
            e.CanExecute = tlbAdd.IsEnabled;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Them();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbModify.IsEnabled;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Sua();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbDelete.IsEnabled;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Xoa();
        }

        private void ViewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbView.IsEnabled;
        }
        private void ViewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Xem();
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
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        /// <summary>
        /// Sự kiện hot key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            //RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = ""; // tlb.Name.Substring(3, tlb.Name.Length - 3);

            if (sender is RibbonButton)
                strTinhNang = ((RibbonButton)sender).Name.Substring(3, ((RibbonButton)sender).Name.Length - 3);
            else
                strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);

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

            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
                Xem();
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

        /// <summary>
        /// Sự kiện double click trên data grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grKhachHangDS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Xem();
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
            PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(raddgrDanhSachTK, txtTimKiemNhanh.Text);
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
            if (raddgrDanhSachTK != null && raddgrDanhSachTK.DataContext != null)
            {
                DataTable dt = ((DataView)raddgrDanhSachTK.DataContext).Table;
                if (dt != null)
                {
                    radPage.PageSize = (int)nudPageSize.Value;
                    raddgrDanhSachTK.DataContext = dt.DefaultView;
                }
            }
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(raddgrDanhSachTK);
            //PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel("GET_DS_DONVI");
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

        void cmbDonVi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaoTreeView();
            raddgrDanhSachTK.ItemsSource = null;
        }
        #endregion

        private void TaoTreeView()
        {
            Presentation.Process.KeToanProcess process = new Presentation.Process.KeToanProcess();
            AutoComboBox au = new AutoComboBox();
            try
            {
                AutoCompleteEntry auDonVi = au.getEntryByDisplayName(lstSourceDonVi, ref cmbDonVi);
                if (auDonVi != null)
                {
                    dtMaPhanLoai = process.getDanhSachMaPhanLoai(auDonVi.KeywordStrings[0], "(''" + BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri() + "'')", BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri(), "%");
                    if (dtMaPhanLoai != null && dtMaPhanLoai.Tables.Count > 0)
                    {
                        dtMaPhanLoai.Relations.Add("Master2Detail", dtMaPhanLoai.Tables[0].Columns["ID"], dtMaPhanLoai.Tables[0].Columns["ID_PLOAI_CHA"]);
                        DataView dv = dtMaPhanLoai.Tables[0].DefaultView;
                        dv.RowFilter = "ID_PLOAI_CHA IS NULL";
                        trvPhanLoai.ItemsSource = null;
                        trvPhanLoai.Items.Clear();
                        trvPhanLoai.ItemsSource = dv;
                    }
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                au = null;
                process = null;
            }
        }

        #region Xu ly nghiep vu
        /// <summary>
        /// Xử lý sự kiện thêm
        /// </summary>
        private void Them()
        {
            Window window = new Window();
            ucTaiKhoanNgoaiTeCT uc = new ucTaiKhoanNgoaiTeCT();
            uc.TthaiNvu = "";
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.KT_TAI_KHOAN_CT);

            window.Content = uc;
            window.ShowDialog();
            TimKiem();
        }

        /// <summary>
        /// Xử lý sự kiện sửa
        /// </summary>
        private void Sua()
        {
            DataRowView dr = (DataRowView)raddgrDanhSachTK.SelectedItem;
            if (dr == null)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
            }
            else
            {
                Window window = new Window();
                ucTaiKhoanNgoaiTeCT uc = new ucTaiKhoanNgoaiTeCT(Convert.ToInt32(dr["ID"]), dr["TTHAI_NVU"].ToString(), DatabaseConstant.Action.SUA);
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.KT_TAI_KHOAN_CT);
                window.Content = uc;
                window.ShowDialog();
                TimKiem();
            }
        }

        /// <summary>
        /// Xử lý sự kiện sửa
        /// </summary>
        private void Xem()
        {
            DataRowView dr = (DataRowView)raddgrDanhSachTK.SelectedItem;
            if (dr == null)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
            }
            else
            {
                Window window = new Window();
                ucTaiKhoanNgoaiTeCT uc = new ucTaiKhoanNgoaiTeCT(Convert.ToInt32(dr["ID"]), dr["TTHAI_NVU"].ToString(), DatabaseConstant.Action.XEM);
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.KT_TAI_KHOAN_CT);
                window.Content = uc;
                window.ShowDialog();
            }
        }

        /// <summary>
        /// Xử lý xóa dữ liệu
        /// </summary>
        private void Xoa()
        {
            try
            {
                KeToanProcess process = new KeToanProcess();
                List<KT_TKHOAN> lstKtTkhoan = new List<KT_TKHOAN>();
                List<ClientResponseDetail> lstResponse = new List<ClientResponseDetail>();

                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    foreach (DataRowView dr in raddgrDanhSachTK.SelectedItems)
                    {
                        KT_TKHOAN obj = new KT_TKHOAN();
                        obj.ID = Convert.ToInt32(dr["ID"]);
                        obj.SO_TAI_KHOAN = dr["SO_TAI_KHOAN"].ToString();
                        lstKtTkhoan.Add(obj);
                    }

                    if (lstKtTkhoan.Count == 0)
                    {
                        LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                        return;
                    }

                    MessageBoxResult messResult = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);
                    if (messResult == MessageBoxResult.Yes)
                    {
                        ApplicationConstant.ResponseStatus ret = process.TaiKhoanChiTietDS(DatabaseConstant.Action.XOA, lstKtTkhoan, ref lstResponse);
                        CommonFunction.ThongBaoKetQua(lstResponse);
                        TimKiem();
                    }
                }
                catch (System.Exception ex)
                {
                    this.Cursor = Cursors.Arrow;
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Tìm kiếm nâng cao
        /// </summary>
        private void TimKiem()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            KeToanProcess process = new KeToanProcess();
            AutoComboBox au = new AutoComboBox();
            try
            {
                if (cmbDonVi.SelectedIndex < 0)
                    return;

                AutoCompleteEntry auDonVi = au.getEntryByDisplayName(lstSourceDonVi, ref cmbDonVi);

                string maDonVi = "(";
                string ngayDL = "";
                string maPhanLoai = "(";
                string tinhChatTK = "";


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


                foreach (object item in trvPhanLoai.CheckedItems)
                {
                    DataRowView dr = item as DataRowView;
                    if (dr["MA_PLOAI"].ToString().Length != 1)
                    {
                        maPhanLoai += "''" + dr["MA_PLOAI"].ToString() + "'',";
                    }
                }

                if (trvPhanLoai.CheckedItems.Count > 0)
                {
                    maPhanLoai = maPhanLoai.Substring(0, maPhanLoai.Length - 1) + ")";
                }
                else
                {
                    maPhanLoai = "";
                }

                if (raddtSoDuNgay.Value != null)
                {
                    ngayDL = Convert.ToDateTime(raddtSoDuNgay.Value).ToString("yyyyMMdd");
                }

                DataTable dtThamSo = null;
                LDatatable.MakeParameterTable(ref dtThamSo);
                LDatatable.AddParameter(ref dtThamSo, "@MaDonVi", "STRING", maDonVi);
                LDatatable.AddParameter(ref dtThamSo, "@MaPhanLoai", "STRING", maPhanLoai);
                LDatatable.AddParameter(ref dtThamSo, "@MaTK", "STRING", txtMaPhanLoaiTK.Text);
                LDatatable.AddParameter(ref dtThamSo, "@TenTK", "STRING", "");
                LDatatable.AddParameter(ref dtThamSo, "@NgayDL", "STRING", ngayDL);
                LDatatable.AddParameter(ref dtThamSo, "@SoDuTu", "DECIMAL", "0");
                LDatatable.AddParameter(ref dtThamSo, "@SoDuDen", "DECIMAL", "0");
                LDatatable.AddParameter(ref dtThamSo, "@TinhChat", "STRING", tinhChatTK);

                DataSet ds = process.getDanhSachTaiKhoanChiTiet(dtThamSo, "DANH_SACH");
                raddgrDanhSachTK.ItemsSource = null;
                if (ds != null && ds.Tables.Count > 0)
                {
                    raddgrDanhSachTK.ItemsSource = ds.Tables[0].DefaultView;
                    lblSumSoTK.Content = ds.Tables[0].Rows.Count.ToString("###,###,###");

                    decimal tongSoDu = 0;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        tongSoDu += Convert.ToDecimal(dr["SODU"]);
                    }
                    lblSumSoDu.Content = tongSoDu.ToString("###,###,###,###,###,###");
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
                au = null;
            }

            Mouse.OverrideCursor = Cursors.Arrow;
        }
        #endregion

    }
}
