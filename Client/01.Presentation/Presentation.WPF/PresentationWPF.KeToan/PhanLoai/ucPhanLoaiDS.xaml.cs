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

namespace PresentationWPF.KeToan.PhanLoai
{
    /// <summary>
    /// Interaction logic for ucPhanLoaiDS.xaml
    /// </summary>
    public partial class ucPhanLoaiDS : UserControl
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

        private List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();

        private DataSet dtMaPhanLoai = new DataSet();

        List<TTHAI_LY_DO> lstLyDo = new List<TTHAI_LY_DO>();
        public void LayDuLieuLyDo(List<TTHAI_LY_DO> lst)
        {
            lstLyDo = lst;
        }

        #endregion

        #region Khoi tao
        public ucPhanLoaiDS()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KeToan;component/PhanLoai/ucPhanLoaiDS.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += tlbHotKey_Click;
            }
            BindHotkey();
            radPage.PageSize = (int)nudPageSize.Value;
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
                lstDieuKien.Add(DatabaseConstant.ToChucDonVi.CNH.getValue());
                au.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVI_THEOPVI.getValue(), lstDieuKien,ClientInformation.MaDonVi);
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

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbApprove.IsEnabled;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Duyet();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbRefuse.IsEnabled;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TuChoi();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbCancel.IsEnabled;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ThoaiDuyet();
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
            txtTimKiemNhanh.RaiseEvent(new RoutedEventArgs(Button.GotFocusEvent));
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                Duyet();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                TuChoi();
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
            PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(raddgrPhanLoaiTK, txtTimKiemNhanh.Text);
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
            if (raddgrPhanLoaiTK != null && raddgrPhanLoaiTK.ItemsSource != null)
            {
                DataTable dt = ((DataView)raddgrPhanLoaiTK.ItemsSource).Table;
                if (dt != null)
                {
                    radPage.PageSize = (int)nudPageSize.Value;
                    raddgrPhanLoaiTK.ItemsSource = dt.DefaultView;
                }
            }
        }

        /// <summary>
        /// Sự kiện double click trên data grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void raddgrPhanLoaiTK_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Xem();
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(raddgrPhanLoaiTK);
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
                    //trvPhanLoai.Items.Clear();
                    dtMaPhanLoai = process.getDanhSachMaPhanLoai(auDonVi.KeywordStrings[0], "(''" + BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri() + "'')", BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri(),"%");
                    if (dtMaPhanLoai != null && dtMaPhanLoai.Tables.Count > 0)
                    {
                        dtMaPhanLoai.Relations.Add("Master2Detail", dtMaPhanLoai.Tables[0].Columns["ID"], dtMaPhanLoai.Tables[0].Columns["ID_PLOAI_CHA"]);
                        DataView dv = dtMaPhanLoai.Tables[0].DefaultView;
                        dv.RowFilter = "ID_PLOAI_CHA IS NULL";
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
            ucPhanLoaiCT uc = new ucPhanLoaiCT();
            uc.TthaiNvu = "";
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.KT_PHAN_LOAI_CT);

            window.Content = uc;
            window.ShowDialog();
            TimKiem();
        }

        /// <summary>
        /// Xử lý sự kiện sửa
        /// </summary>
        private void Sua()
        {
            DataRowView dr = (DataRowView)raddgrPhanLoaiTK.SelectedItem;
            if (dr == null)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
            }
            else
            {
                Window window = new Window();
                ucPhanLoaiCT uc = new ucPhanLoaiCT(Convert.ToInt32(dr["ID"]), dr["TTHAI_NVU"].ToString(), DatabaseConstant.Action.SUA);
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.KT_PHAN_LOAI_CT);
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
            DataRowView dr = (DataRowView)raddgrPhanLoaiTK.SelectedItem;
            if (dr == null)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
            }
            else
            {
                Window window = new Window();
                ucPhanLoaiCT uc = new ucPhanLoaiCT(Convert.ToInt32(dr["ID"]), dr["TTHAI_NVU"].ToString(), DatabaseConstant.Action.XEM);
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.KT_PHAN_LOAI_CT);
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
                List<KT_PLOAI> lstKtPloai = new List<KT_PLOAI>();
                List<ClientResponseDetail> lstResponse = new List<ClientResponseDetail>();

                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    for (int i = 0; i < raddgrPhanLoaiTK.Items.Count; i++)
                    {
                        DataRowView dr = (DataRowView)raddgrPhanLoaiTK.Items[i];
                        if (Convert.ToBoolean(dr["CHON"]) == true)
                        {
                            KT_PLOAI obj = new KT_PLOAI();
                            obj.ID = Convert.ToInt32(dr["ID"]);
                            obj.MA_PLOAI = dr["MA_PLOAI"].ToString();
                            lstKtPloai.Add(obj);
                        }
                    }

                    if (lstKtPloai.Count == 0)
                    {
                        LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                        return;
                    }

                    MessageBoxResult messResult = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);
                    if (messResult == MessageBoxResult.Yes)
                    {
                        bool ret = process.XuLyPhanLoaiTaiKhoanDS(lstKtPloai.ToArray(), ref lstResponse, DatabaseConstant.Action.XOA);
                        if (!ret)
                        {
                            LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                        }
                        else
                        {
                            CommonFunction.ThongBaoKetQua(lstResponse);
                            TimKiem();
                        }
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

        private void Duyet()
        {
            try
            {
                KeToanProcess process = new KeToanProcess();
                List<KT_PLOAI> lstKtPloai = new List<KT_PLOAI>();
                List<ClientResponseDetail> lstResponse = new List<ClientResponseDetail>();
                lstLyDo = new List<TTHAI_LY_DO>();
                this.Cursor = Cursors.Wait;
                try
                {
                    for (int i = 0; i < raddgrPhanLoaiTK.Items.Count; i++)
                    {
                        DataRowView dr = (DataRowView)raddgrPhanLoaiTK.Items[i];
                        if (Convert.ToBoolean(dr["CHON"]) == true)
                        {
                            KT_PLOAI obj = new KT_PLOAI();
                            obj.ID = Convert.ToInt32(dr["ID"]);
                            obj.MA_PLOAI = dr["MA_PLOAI"].ToString();
                            lstKtPloai.Add(obj);
                            TTHAI_LY_DO objTThai = new TTHAI_LY_DO();
                            objTThai.ID = Convert.ToInt32(dr["ID"]);
                            objTThai.MA = dr["MA_PLOAI"].ToString();
                            objTThai.TEN = dr["TEN_PLOAI"].ToString();
                            lstLyDo.Add(objTThai);
                        }
                    }

                    if (lstKtPloai.Count == 0)
                    {
                        LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                        return;
                    }

                    MessageBoxResult messResult = LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question);
                    if (messResult == MessageBoxResult.Yes)
                    {
                        ucLyDo lydo = new ucLyDo(lstLyDo);
                        lydo.DuLieuTraVe = new ucLyDo.LayDuLieu(LayDuLieuLyDo);
                        Window win = new Window();
                        //win.Title = "Danh sách mã phân loại tài khoản";
                        win.Content = lydo;
                        win.Title = LLanguage.SearchResourceByKey("U.KeToan.KiemSoat.frmLyDo.LyDo") + "-" + LLanguage.SearchResourceByKey("U.DungChung.Button.Duyet");
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.ShowDialog();
                        lstKtPloai.ForEach(f => f.TTHAI_LY_DO = lstLyDo.FirstOrDefault(g => g.ID == f.ID).LY_DO);
                        bool ret = process.XuLyPhanLoaiTaiKhoanDS(lstKtPloai.ToArray(), ref lstResponse, DatabaseConstant.Action.DUYET);
                        if (!ret)
                        {
                            LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                        }
                        else
                        {
                            CommonFunction.ThongBaoKetQua(lstResponse);
                            TimKiem();
                        }
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

        private void TuChoi()
        {
            try
            {
                KeToanProcess process = new KeToanProcess();
                List<KT_PLOAI> lstKtPloai = new List<KT_PLOAI>();
                List<ClientResponseDetail> lstResponse = new List<ClientResponseDetail>();
                lstLyDo = new List<TTHAI_LY_DO>();
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    for (int i = 0; i < raddgrPhanLoaiTK.Items.Count; i++)
                    {
                        DataRowView dr = (DataRowView)raddgrPhanLoaiTK.Items[i];
                        if (Convert.ToBoolean(dr["CHON"]) == true)
                        {
                            KT_PLOAI obj = new KT_PLOAI();
                            obj.ID = Convert.ToInt32(dr["ID"]);
                            obj.MA_PLOAI = dr["MA_PLOAI"].ToString();
                            lstKtPloai.Add(obj);
                            TTHAI_LY_DO objTThai = new TTHAI_LY_DO();
                            objTThai.ID = Convert.ToInt32(dr["ID"]);
                            objTThai.MA = dr["MA_PLOAI"].ToString();
                            objTThai.TEN = dr["TEN_PLOAI"].ToString();
                            lstLyDo.Add(objTThai);
                        }
                    }

                    if (lstKtPloai.Count == 0)
                    {
                        LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                        return;
                    }

                    MessageBoxResult messResult = LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question);
                    if (messResult == MessageBoxResult.Yes)
                    {
                        ucLyDo lydo = new ucLyDo(lstLyDo);
                        lydo.DuLieuTraVe = new ucLyDo.LayDuLieu(LayDuLieuLyDo);
                        Window win = new Window();
                        //win.Title = "Danh sách mã phân loại tài khoản";
                        win.Content = lydo;
                        win.Title = LLanguage.SearchResourceByKey("U.KeToan.KiemSoat.frmLyDo.LyDo") + "-" + LLanguage.SearchResourceByKey("U.DungChung.Button.TuChoi");
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.ShowDialog();
                        lstKtPloai.ForEach(f => f.TTHAI_LY_DO = lstLyDo.FirstOrDefault(g => g.ID == f.ID).LY_DO);
                        bool ret = process.XuLyPhanLoaiTaiKhoanDS(lstKtPloai.ToArray(), ref lstResponse, DatabaseConstant.Action.TU_CHOI_DUYET);
                        if (!ret)
                        {
                            LMessage.ShowMessage("M.DungChung.TuChoiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
                        }
                        else
                        {
                            CommonFunction.ThongBaoKetQua(lstResponse);
                            TimKiem();
                        }
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

        private void ThoaiDuyet()
        {
            try
            {
                KeToanProcess process = new KeToanProcess();
                List<KT_PLOAI> lstKtPloai = new List<KT_PLOAI>();
                List<ClientResponseDetail> lstResponse = new List<ClientResponseDetail>();
                lstLyDo = new List<TTHAI_LY_DO>();
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    for (int i = 0; i < raddgrPhanLoaiTK.Items.Count; i++)
                    {
                        DataRowView dr = (DataRowView)raddgrPhanLoaiTK.Items[i];
                        if (Convert.ToBoolean(dr["CHON"]) == true)
                        {
                            KT_PLOAI obj = new KT_PLOAI();
                            obj.ID = Convert.ToInt32(dr["ID"]);
                            obj.MA_PLOAI = dr["MA_PLOAI"].ToString();
                            lstKtPloai.Add(obj);
                            TTHAI_LY_DO objTThai = new TTHAI_LY_DO();
                            objTThai.ID = Convert.ToInt32(dr["ID"]);
                            objTThai.MA = dr["MA_PLOAI"].ToString();
                            objTThai.TEN = dr["TEN_PLOAI"].ToString();
                            lstLyDo.Add(objTThai);
                        }
                    }
                    if (lstKtPloai.Count == 0)
                    {
                        LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                        return;
                    }

                    MessageBoxResult messResult = LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question);
                    if (messResult == MessageBoxResult.Yes)
                    {
                        ucLyDo lydo = new ucLyDo(lstLyDo);
                        lydo.DuLieuTraVe = new ucLyDo.LayDuLieu(LayDuLieuLyDo);
                        Window win = new Window();
                        //win.Title = "Danh sách mã phân loại tài khoản";
                        win.Content = lydo;
                        win.Title = LLanguage.SearchResourceByKey("U.KeToan.KiemSoat.frmLyDo.LyDo") + "-" + LLanguage.SearchResourceByKey("U.DungChung.Button.ThoaiDuyet");
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.ShowDialog();
                        lstKtPloai.ForEach(f => f.TTHAI_LY_DO = lstLyDo.FirstOrDefault(g => g.ID == f.ID).LY_DO);
                        bool ret = process.XuLyPhanLoaiTaiKhoanDS(lstKtPloai.ToArray(), ref lstResponse, DatabaseConstant.Action.THOAI_DUYET);
                        if (!ret)
                        {
                            LMessage.ShowMessage("M.DungChung.ThoaiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
                        }
                        else
                        {
                            CommonFunction.ThongBaoKetQua(lstResponse);
                            TimKiem();
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    this.Cursor = Cursors.Arrow;
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
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

        /// <summary>
        /// Tìm kiếm nâng cao
        /// </summary>
        private void TimKiem()
        {
            string maPhanLoai = "(";
            string tthaiNvu = "";

            foreach (object item in trvPhanLoai.CheckedItems)
            {
                DataRowView dr = (DataRowView)item;
                maPhanLoai += dr["MA_PLOAI"] + ",";
            }

            if (trvPhanLoai.CheckedItems.Count > 0)
            {
                maPhanLoai = maPhanLoai.Substring(0, maPhanLoai.Length - 1) + ")";
            }
            else
            {
                maPhanLoai = "";
            }

            if (ucTrangThaiNVu.GetItemsSelected() != "NULL")
            {
                tthaiNvu = ucTrangThaiNVu.GetItemsSelected();
            }
            else
                tthaiNvu = "";

            KeToanProcess process = new KeToanProcess();
            AutoComboBox au = new AutoComboBox();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                AutoCompleteEntry auDonVi = au.getEntryByDisplayName(lstSourceDonVi, ref cmbDonVi);
                DataSet ds = process.getDanhSachMaPhanLoai(auDonVi.KeywordStrings[0], tthaiNvu, BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri(), maPhanLoai);
                raddgrPhanLoaiTK.ItemsSource = null;
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr["TRANG_THAI"] = BusinessConstant.layNgonNguNghiepVu(dr["TTHAI_NVU"].ToString());
                        dr["TCHAT_CNO"] = BusinessConstant.layNgonNguTuGiaTri(dr["MA_TCHAT_CNO"].ToString(), "CO_KHONG");
                    }

                    raddgrPhanLoaiTK.ItemsSource = ds.Tables[0].DefaultView;
                    lblSum.Content = ds.Tables[0].Rows.Count.ToString("#");
                }
                else
                {
                    raddgrPhanLoaiTK.ItemsSource = null;
                    lblSum.Content = 0;
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

        //private void chkAll_Click(object sender, RoutedEventArgs e)
        //{
        //    for (int i = 0; i < raddgrPhanLoaiTK.Items.Count; i++)
        //    {
        //        DataRowView dr = (DataRowView)raddgrPhanLoaiTK.Items[i];
        //        dr["CHON"] = chkAll.IsChecked;
        //    }
        //}
    }
}
