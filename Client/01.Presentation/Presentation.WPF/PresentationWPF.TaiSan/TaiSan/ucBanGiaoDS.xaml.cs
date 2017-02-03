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
using Presentation.Process.DanhMucServiceRef;
using Presentation.Process.HuyDongVonServiceRef;
using PresentationWPF.CustomControl;
using Telerik.Windows.Controls;
using Presentation.Process.TaiSanServiceRef;

namespace PresentationWPF.TaiSan.TaiSan
{
    /// <summary>
    /// Interaction logic for ucBanGiaoDS.xaml
    /// </summary>
    public partial class ucBanGiaoDS : UserControl
    {
        #region Khai bao

        List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKyHan = new List<AutoCompleteEntry>();

        DataTable dtTreeDonVi = new DataTable();

        private int currentPosition;
        private int currentPage;
        private int currentID;
        List<int> lstID = new List<int>();
        BAN_GIAO_DTO obj = new BAN_GIAO_DTO();
        DIEU_KIEN_TIM_KIEM_DTO dieuKien = new DIEU_KIEN_TIM_KIEM_DTO();
        List<DANH_SACH_BAN_GIAO_DTO> danhSach = new List<DANH_SACH_BAN_GIAO_DTO>();

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

        //Lưu danh sách các item được chọn trong treeview
        private List<string> lstChecked = new List<string>();

        // Phân trang
        int StartRow = 1;
        int EndRow = ClientInformation.SoLuongBanGhi;
        int CurrentPagging = 1;
        int PaggingSize = ClientInformation.SoLuongBanGhi;
        #endregion

        #region Khoi tao
        public ucBanGiaoDS()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TaiSan;component/TaiSan/ucBanGiaoDS.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindHotkey();
            //radpage.PageSize = (int)nudPageSize.Value;
            radpage.PageIndexChanging += new EventHandler<PageIndexChangingEventArgs>(radpage_PageIndexChanging);
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
                        KeyGesture keyg = new KeyGesture(Key.D, ModifierKeys.Control);
                        key = new KeyBinding(DeleteCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift);
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
            txtTimKiemNhanh.RaiseEvent(new RoutedEventArgs(Button.GotFocusEvent));
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
            OnHelp();
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
                TimKiem();
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
                OnHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void OnHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
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
            InitEventHandler();
            raddtNgaySDTu.Value = null;
            raddtNgaySDDen.Value = null;
            txtTimKiemNhanh.Focus();
        }

        void InitEventHandler()
        {
            txtTimKiemNhanh.TextChanged += txtTimKiemNhanh_TextChanged;
            txtTimKiemNhanh.GotFocus += txtTimKiemNhanh_GotFocus;
            txtTimKiemNhanh.LostFocus += txtTimKiemNhanh_LostFocus;
            nudPageSize.ValueChanged += nudPageSize_ValueChanged;
        }

        /// <summary>
        /// Load dữ liệu lên Form
        /// </summary>
        private void LoadDuLieu()
        {
            LoadCombobox();
            LoadTreeview();
            LoadGrid();
        }

        /// <summary>
        /// Load dữ liệu lên Combobox
        /// </summary>
        private void LoadCombobox()
        {
            try
            {
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();
                lstSourceDonVi = new List<AutoCompleteEntry>();
                lstDieuKien.Add(ClientInformation.TenDangNhap);
                lstDieuKien.Add(ClientInformation.MaDonVi);
                lstDieuKien.Add(DatabaseConstant.ToChucDonVi.PGD.getValue());
                auto.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVI_THEOPVI.getValue(), lstDieuKien, ClientInformation.MaDonViGiaoDich);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Load dữ liệu lên Treeview
        /// </summary>
        private void LoadTreeview()
        {
            try
            {
                TaiSanProcess TaiSanProcess = new TaiSanProcess();
                string sIdDonVi = "";
                string sMaDonVi = "";
                string sIdDonViQLy = "";
                string sMaDonViQLy = "";
                if (lstSourceDonVi != null)
                {
                    sIdDonVi = lstSourceDonVi.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.ElementAt(1);
                    sMaDonVi = lstSourceDonVi.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.ElementAt(0);
                    DM_DON_VI objDonVi = new DanhMucProcess().getDonViById(Convert.ToInt32(sIdDonVi));

                    sIdDonViQLy = objDonVi.ID.ToString();
                    sMaDonViQLy = objDonVi.MA_DVI;

                }

                dtTreeDonVi = TaiSanProcess.GetTreeDonVi(sIdDonViQLy).Tables[0];

                itemPhongGD.Items.Clear();
                //Cấu trúc của Tag: GiaTri#Level#LoaiTree ( VD: MaSP001#2#SAN_PHAM)
                itemPhongGD.Tag = "0#0#DON_VI";
                itemPhongGD.IsExpanded = false;
                BuildTree(itemPhongGD, dtTreeDonVi, "DON_VI");

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        protected void BuildTree(RadTreeViewItem item, DataTable dt, string sLoaiTree)
        {
            try
            {
                //Cấu trúc của Tag: GiaTri#Level#LoaiTree  ( VD:  MaSP001#2#SAN_PHAM hoặc CUM001#3#DON_VI)
                string sTag = item.Tag.ToString();
                int i1 = sTag.IndexOf("#");
                int i2 = sTag.LastIndexOf("#");

                string sValue = sTag.Substring(0, i1);
                int iLevel = Convert.ToInt32(sTag.Substring(i1 + 1, i2 - i1 - 1));

                foreach (DataRow row in dt.Rows)
                {
                    if (iLevel < Convert.ToInt32(row["LEVEL"]))
                    {
                        if (row["NODE_PARENT"].ToString() == sValue)
                        {
                            RadTreeViewItem subItem = new RadTreeViewItem();
                            subItem.Header = row["NODE_NAME"].ToString();
                            subItem.Tag = row["NODE"].ToString() + "#" + row["LEVEL"].ToString() + "#" + sLoaiTree;
                            subItem.IsExpanded = false;
                            item.Items.Add(subItem);
                            BuildTree(subItem, dt, sLoaiTree);
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

        /// <summary>
        /// Load dữ liệu lên Grid
        /// </summary>
        private void LoadGrid()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TaiSanProcess process = new TaiSanProcess();
                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();

                #region Điều kiện tìm kiếm
                dieuKien = new DIEU_KIEN_TIM_KIEM_DTO();
                danhSach = new List<DANH_SACH_BAN_GIAO_DTO>();
                List<string> sMaDonVi = new List<string>();

                foreach (RadTreeViewItem item in tvwTree.CheckedItems)
                {

                    ///Cấu trúc của Tag: GiaTri#Level#LoaiTree  ( VD:  MaSP001#2#SAN_PHAM hoặc CUM001#3#DON_VI)
                    string sTag = item.Tag.ToString();
                    int i1 = sTag.IndexOf("#");
                    int i2 = sTag.LastIndexOf("#");

                    string sValue = sTag.Substring(0, i1);
                    int iLevel = Convert.ToInt32(sTag.Substring(i1 + 1, i2 - i1 - 1));
                    string sLoaiTree = sTag.Substring(i2 + 1);

                    if (sLoaiTree.Equals("DON_VI"))
                    {
                        sMaDonVi.Add(sValue);
                    }

                }
                dieuKien.SMaDonVi = sMaDonVi.ToArray();

                if (!ucTrangThaiNVu.GetItemsSelected().Equals("NULL"))
                    dieuKien.STrangThaiNVu = ucTrangThaiNVu.GetItemsSelected();

                if (!txtSoBienBan.Text.IsNullOrEmptyOrSpace())
                    dieuKien.SSoBienBan = txtSoBienBan.Text;

                if (!txtMaTaiSan.Text.IsNullOrEmptyOrSpace())
                    dieuKien.SMaTS = txtMaTaiSan.Text;

                if (!txtTenTS.Text.IsNullOrEmptyOrSpace())
                    dieuKien.STenTS = txtTenTS.Text;

                if (!txtMaDVSD.Text.IsNullOrEmptyOrSpace())
                    dieuKien.SMaDonViSD = txtMaDVSD.Text;

                if (!txtTenDVSD.Text.IsNullOrEmptyOrSpace())
                    dieuKien.STenDonViSD = txtTenDVSD.Text;

                if (!raddtNgaySDTu.Value.IsNullOrEmpty() && !raddtNgaySDTu.Text.IsNullOrEmptyOrSpace())
                    dieuKien.SNgaySDTu = Convert.ToDateTime(raddtNgaySDTu.Value).ToString("yyyyMMdd");

                if (!raddtNgaySDDen.Value.IsNullOrEmpty() && !raddtNgaySDDen.Text.IsNullOrEmptyOrSpace())
                    dieuKien.SNgaySDDen = Convert.ToDateTime(raddtNgaySDDen.Value).ToString("yyyyMMdd");
                danhSach = new List<DANH_SACH_BAN_GIAO_DTO>();
                #endregion
                // Phân trang
                dieuKien.StartRow = 1;
                dieuKien.EndRow = ClientInformation.SoLuongBanGhi;
                int CurrentPagging = 1;
                int PaggingSize = ClientInformation.SoLuongBanGhi;

                process.BanGiaoTaiSan(DatabaseConstant.Function.TS_BAN_GIAO_DS, DatabaseConstant.Action.XEM, lstID, dieuKien, ref obj, ref danhSach, ref lstResponseDetail);
                if (!danhSach.IsNullOrEmpty() && danhSach.Count > 0)
                {
                    foreach (var item in danhSach)
                    {
                        item.TRANG_THAI = BusinessConstant.layNgonNguNghiepVu(item.TTHAI_NVU);
                    }
                    int totalRecord = danhSach.Count;
                }
                if (danhSach != null && danhSach.Count > 0)
                {
                    DataTable serverDataTable = LList.ConvertTo(danhSach);
                    if (serverDataTable.Rows.Count > 0)
                    {
                        int totalRecord = danhSach.Count;
                        lblSumSoSo.Content = String.Format("{0:#,#}", totalRecord);

                        grDanhSach.ItemsSource = serverDataTable.DefaultView;
                        grDanhSach.Rebind();
                    }
                    else
                    {
                        grDanhSach.ItemsSource = null;
                        grDanhSach.Rebind();
                    }
                }
                else
                {
                    grDanhSach.ItemsSource = null;
                    grDanhSach.Rebind();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Load lại dữ liệu khi có thay đổi từ form chi tiết
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void userControl_OnSavingCompleted(object sender, EventArgs e)
        {
            LoadGrid();
            //LoadGridPhanTrang();
            //CommonFunction.GoToPosition(currentID, ref grDanhSach, radpage, nudPageSize);
            //grDanhSach.Items.MoveToPage(1);
            //grDanhSach.Items.MoveCurrentToPosition(1);
        }

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
            PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grDanhSach, txtTimKiemNhanh.Text);
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                BeforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                BeforeRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
                BeforeView();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            {
                txtTimKiemNhanh.RaiseEvent(new RoutedEventArgs(Button.GotFocusEvent));
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
                OnHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }

        /// <summary>
        /// Sự kiện thay đổi số bản ghi trên một page hiển thị
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudPageSize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (grDanhSach != null && grDanhSach.DataContext != null)
            {
                if (danhSach != null)
                {
                    radpage.PageSize = (int)nudPageSize.Value;
                    DataTable dt = LList.ConvertTo(danhSach);
                    grDanhSach.ItemsSource = dt;
                }
            }
        }

        private void grDanhSach_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BeforeView();
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(grDanhSach);
        }

        private void cmbDonVi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadTreeview();
            grDanhSach.DataContext = null;
            lblSumSoSo.Content = 0;
            lblSumSoDu.Content = 0;
            lblSoDuBQ.Content = 0;
        }

        private void radpage_PageIndexChanging(object sender, PageIndexChangingEventArgs e)
        {
            if (e.NewPageIndex < radpage.PageCount)
            {
                CurrentPagging = e.NewPageIndex + 1;
                StartRow = (CurrentPagging - 1) * PaggingSize + 1;
                EndRow = StartRow + PaggingSize;
                //radpage = new RadDataPager();
                LoadGrid();
            }
        }

        #endregion

        #region Xu ly nghiep vu

        /// <summary>
        /// Trước khi thêm mới
        /// </summary>
        private void BeforeAddNew()
        {
            OnAddNew();
        }

        /// <summary>
        /// Trước khi sửa
        /// </summary>
        private void BeforeModify()
        {
            try
            {
                //List<DANH_SACH_BAN_GIAO_DTO> listDataRow = getListSeletedDataRow();
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
                        foreach (DataRowView dr in listDataRow)
                        {
                            if (dr["TTHAI_NVU"].Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                            {
                                LMessage.ShowMessage("Không thể xử lý dữ liệu đã duyệt.", LMessage.MessageBoxType.Warning);
                                return;
                            }
                            else
                            {
                            currentPage = grDanhSach.Items.PageIndex;
                            currentPosition = grDanhSach.Items.CurrentPosition;
                            //currentID = listDataRow.First().ID;
                            currentID = int.Parse(listDataRow.First()["ID"].ToString());

                            OnModify(currentID);
                            }
                        }
                    }
                }
                else
                {
                    LMessage.ShowMessage("Lỗi chọn dữ liệu để xử lý", LMessage.MessageBoxType.Warning);
                    return;
                }
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
                //List<DANH_SACH_BAN_GIAO_DTO> listDataRow = getListSeletedDataRow();
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
                            // Yêu cầu lock bản ghi cần xử lý
                            UtilitiesProcess process = new UtilitiesProcess();
                            List<int> listLockedId = new List<int>();

                            bool retLockData = process.LockData(DatabaseConstant.Module.QLTS,
                                DatabaseConstant.Function.TS_BAN_GIAO_DS,
                                DatabaseConstant.Table.TS_BAN_GIAO,
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
        /// Trước khi duyệt
        /// </summary>
        private void BeforeApprove()
        {
            try
            {
                //List<DANH_SACH_BAN_GIAO_DTO> listDataRow = getListSeletedDataRow();
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
                        MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question);
                        if (ret == MessageBoxResult.Yes)
                        {
                            // Yêu cầu lock bản ghi cần xử lý
                            UtilitiesProcess process = new UtilitiesProcess();
                            List<int> listLockedId = new List<int>();

                            bool retLockData = process.LockData(DatabaseConstant.Module.QLTS,
                                DatabaseConstant.Function.TS_BAN_GIAO_DS,
                                DatabaseConstant.Table.TS_BAN_GIAO,
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
        /// Trước khi thoái duyệt
        /// </summary>
        private void BeforeCancel()
        {
            try
            {
                //List<DANH_SACH_BAN_GIAO_DTO> listDataRow = getListSeletedDataRow();
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
                            if (dr["TTHAI_NVU"].Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                            {
                                LMessage.ShowMessage("Không thể xử lý dữ liệu đã duyệt.", LMessage.MessageBoxType.Warning);
                                return;
                            }
                            else
                            {
                                int id = int.Parse(dr["id"].ToString());
                                listId.Add(id);
                            }
                        }

                        // Cảnh báo người dùng
                        MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question);
                        if (ret == MessageBoxResult.Yes)
                        {
                            // Yêu cầu lock bản ghi cần xử lý
                            UtilitiesProcess process = new UtilitiesProcess();
                            List<int> listLockedId = new List<int>();

                            bool retLockData = process.LockData(DatabaseConstant.Module.QLTS,
                                DatabaseConstant.Function.TS_BAN_GIAO_DS,
                                DatabaseConstant.Table.TS_BAN_GIAO,
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
        /// Trước khi từ chối
        /// </summary>
        private void BeforeRefuse()
        {
            try
            {
                //List<DANH_SACH_BAN_GIAO_DTO> listDataRow = getListSeletedDataRow();
                List<DataRowView> listDataRow = getListSeletedDataRow();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
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
                        MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question);
                        if (ret == MessageBoxResult.Yes)
                        {
                            // Yêu cầu lock bản ghi cần xử lý
                            UtilitiesProcess process = new UtilitiesProcess();
                            List<int> listLockedId = new List<int>();

                            bool retLockData = process.LockData(DatabaseConstant.Module.QLTS,
                                DatabaseConstant.Function.TS_BAN_GIAO_DS,
                                DatabaseConstant.Table.TS_BAN_GIAO,
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
        /// Trước khi xem
        /// </summary>
        private void BeforeView()
        {
            try
            {
                //List<DANH_SACH_BAN_GIAO_DTO> listDataRow = getListSeletedDataRow();
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
                        currentPage = grDanhSach.Items.PageIndex;
                        currentPosition = grDanhSach.Items.CurrentPosition;
                        currentID = int.Parse(listDataRow.First()["id"].ToString());

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

        private void OnAddNew()
        {
            Window frm = new Window();
            ucBanGiaoCT userControl = new ucBanGiaoCT();

            userControl.Function = DatabaseConstant.Function.TS_BAN_GIAO;
            userControl.Action = DatabaseConstant.Action.THEM;
            //uc.OnSavingCompleted += new EventHandler(userControl_OnSavingCompleted);

            frm.Title = DatabaseConstant.layNgonNguTieuDeForm(userControl.Function);
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frm.Content = userControl;
            frm.ShowDialog();
            LoadGrid();
        }

        private void OnModify(int id)
        {
            try
            {
                ucBanGiaoCT userControl = new ucBanGiaoCT();
                userControl.Function = DatabaseConstant.Function.TS_BAN_GIAO;
                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);

                bool ret = process.LockData(DatabaseConstant.Module.QLTS,
                    userControl.Function,
                    DatabaseConstant.Table.TS_BAN_GIAO,
                    DatabaseConstant.Action.SUA,
                    listLockId);
                if (ret)
                {
                    userControl.OnSavingCompleted += new EventHandler(userControl_OnSavingCompleted);
                    userControl.Action = DatabaseConstant.Action.SUA;
                    userControl.idCT = id;

                    Window frm = new Window();
                    frm.Title = DatabaseConstant.layNgonNguTieuDeForm(userControl.Function);
                    frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    frm.Content = userControl;
                    frm.ShowDialog();
                    LoadGrid();
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

        private void OnDelete(List<int> listId)
        {
            TaiSanProcess TaiSanProcess = new TaiSanProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = TaiSanProcess.BanGiaoTaiSan(DatabaseConstant.Function.TS_BAN_GIAO_DS, DatabaseConstant.Action.XOA, listId, dieuKien, ref obj, ref danhSach, ref listClientResponseDetail);

                AfterDelete(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                    DatabaseConstant.Function.TS_BAN_GIAO_DS,
                    DatabaseConstant.Table.TS_BAN_GIAO,
                    DatabaseConstant.Action.XOA,
                    listId);
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void OnApprove(List<int> listId)
        {
            TaiSanProcess TaiSanProcess = new TaiSanProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = TaiSanProcess.BanGiaoTaiSan(DatabaseConstant.Function.TS_BAN_GIAO_DS, DatabaseConstant.Action.DUYET, listId, dieuKien, ref obj, ref danhSach, ref listClientResponseDetail);

                AfterApprove(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                    DatabaseConstant.Function.TS_BAN_GIAO_DS,
                    DatabaseConstant.Table.TS_BAN_GIAO,
                    DatabaseConstant.Action.DUYET,
                    listId);
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void OnCancel(List<int> listId)
        {
            TaiSanProcess TaiSanProcess = new TaiSanProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = TaiSanProcess.BanGiaoTaiSan(DatabaseConstant.Function.TS_BAN_GIAO_DS, DatabaseConstant.Action.THOAI_DUYET, listId, dieuKien, ref obj, ref danhSach, ref listClientResponseDetail);

                AfterCancel(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                    DatabaseConstant.Function.TS_BAN_GIAO_DS,
                    DatabaseConstant.Table.TS_BAN_GIAO,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listId);
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void OnRefuse(List<int> listId)
        {
            TaiSanProcess TaiSanProcess = new TaiSanProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = TaiSanProcess.BanGiaoTaiSan(DatabaseConstant.Function.TS_BAN_GIAO_DS, DatabaseConstant.Action.TU_CHOI_DUYET, listId, dieuKien, ref obj, ref danhSach, ref listClientResponseDetail);

                AfterRefuse(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                    DatabaseConstant.Function.TS_BAN_GIAO_DS,
                    DatabaseConstant.Table.TS_BAN_GIAO,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listId);
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void OnView(int id)
        {
            try
            {
                ucBanGiaoCT userControl = new ucBanGiaoCT();
                userControl.Function = DatabaseConstant.Function.TS_BAN_GIAO;
                userControl.OnSavingCompleted += new EventHandler(userControl_OnSavingCompleted);
                userControl.Action = DatabaseConstant.Action.XEM;
                userControl.idCT = id;

                Window frm = new Window();
                frm.Title = DatabaseConstant.layNgonNguTieuDeForm(userControl.Function);
                frm.Content = userControl;
                frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Sau khi xóa
        /// </summary>
        /// <param name="ret"></param>
        private void AfterDelete(bool ret,
            List<int> listId,
            List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
                LoadGrid();
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                LoadGrid();
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_BAN_GIAO_DS,
                DatabaseConstant.Table.TS_BAN_GIAO,
                DatabaseConstant.Action.XOA,
                listId);
        }

        /// <summary>
        /// Sau khi duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void AfterApprove(bool ret,
            List<int> listId,
            List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);
                LoadGrid();
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                LoadGrid();
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_BAN_GIAO_DS,
                DatabaseConstant.Table.TS_BAN_GIAO,
                DatabaseConstant.Action.DUYET,
                listId);
        }

        /// <summary>
        /// Sau khi thoái duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void AfterCancel(bool ret,
            List<int> listId,
            List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);
                    LoadGrid();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    LoadGrid();
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                    DatabaseConstant.Function.TS_BAN_GIAO_DS,
                    DatabaseConstant.Table.TS_BAN_GIAO,
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
        /// Sau khi từ chối duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void AfterRefuse(bool ret,
            List<int> listId,
            List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);
                    LoadGrid();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    LoadGrid();
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                    DatabaseConstant.Function.TS_BAN_GIAO_DS,
                    DatabaseConstant.Table.TS_BAN_GIAO,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listId);
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Xử lý tìm kiếm dữ liệu
        /// </summary>
        private void TimKiem()
        {
            LayLai();
        }


        /// <summary>
        /// Lấy lại dữ liệu
        /// </summary>
        private void LayLai()
        {
            LoadGrid();
        }

        /// <summary>
        /// Lấy danh sách id được chọn
        /// </summary>
        /// <returns></returns>
        //private List<DANH_SACH_BAN_GIAO_DTO> getListSeletedDataRow()
        //{
        //    try
        //    {
        //        List<DANH_SACH_BAN_GIAO_DTO> listDataRow = new List<DANH_SACH_BAN_GIAO_DTO>();

        //        if (grDanhSach.SelectedItems.Count > 0)
        //        {
        //            for (int i = 0; i < grDanhSach.SelectedItems.Count; i++)
        //            {
        //                DANH_SACH_BAN_GIAO_DTO dr = (DANH_SACH_BAN_GIAO_DTO)grDanhSach.SelectedItems[i];
        //                listDataRow.Add(dr);
        //            }
        //        }
        //        return listDataRow;
        //    }
        //    catch (Exception ex)
        //    {
        //        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
        //        return null;
        //    }

        //}



        /// <summary>
        /// Lấy danh sách id được chọn
        /// </summary>
        /// <returns></returns>
        private List<DataRowView> getListSeletedDataRow()
        {
            try
            {
                List<DataRowView> listDataRow = new List<DataRowView>();

                if (grDanhSach.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < grDanhSach.SelectedItems.Count; i++)
                    {
                        DataRowView dr = (DataRowView)grDanhSach.SelectedItems[i];
                        listDataRow.Add(dr);
                    }
                }
                return listDataRow;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                return null;
            }

        }

        #endregion
    }
}
