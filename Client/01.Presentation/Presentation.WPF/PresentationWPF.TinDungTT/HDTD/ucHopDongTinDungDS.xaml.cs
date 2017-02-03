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
using Telerik.Windows.Controls;
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.TinDungTTServiceRef;
using PresentationWPF.CustomControl;
using Presentation.Process.Common;

namespace PresentationWPF.TinDungTT.HDTD
{
    /// <summary>
    /// Interaction logic for ucHopDongTinDungDS.xaml
    /// </summary>
    public partial class ucHopDongTinDungDS : UserControl
    {
        #region Khai bao
        private int flag = 0;
        private int currentPosition;
        private int currentPage;
        private int currentID;

        DataTable dtTreeDonVi = new DataTable();

        List<AutoCompleteEntry> lstSourceChucVu = new List<AutoCompleteEntry>();

        private DataTable dtSourceTree = new DataTable();

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

        List<AutoCompleteEntry> lstLoaiHDTD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSanPham = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceAThoiHanVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceAThoiHanVayDen = new List<AutoCompleteEntry>();

        //Lưu danh sách các item được chọn trong treeview
        private List<string> lstChecked = new List<string>();

        // Phân trang
        int StartRow = 1;
        int EndRow = ClientInformation.SoLuongBanGhi;
        int CurrentPagging = 1;
        int PaggingSize = ClientInformation.SoLuongBanGhi;
        #endregion

        #region Khoi tao
        public ucHopDongTinDungDS()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDungTT;component/HDTD/ucHopDongTinDungDS.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindHotkey();
            radPage.PageSize = (int)nudPageSize.Value;
            LoadCombobox();
            LoadTreeview();
            txtTimKiemNhanh.Focus();
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
            TimKiem();
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
            cmbThoiHanVay.SelectedIndex = -1;
            cmbThoiHanVayDen.SelectedIndex = -1;
            txtTimKiemNhanh.Focus();
        }

        private void LoadCombobox()
        {
            try
            {
                string Dislay = LLanguage.SearchResourceByKey("U.DungChung.TatCa");
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();
                string maTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();

                // Gán giá trị điều kiện
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.LOAI_HOP_DONG_VAY));
                lstLoaiHDTD.Add(new AutoCompleteEntry(Dislay, "%", "%"));
                KhoiTaoGiaTriComboBox(ref lstLoaiHDTD, ref cmbLoaiHDTD, maTruyVan, lstDieuKien);

                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.LOAI_SAN_PHAM_TDUNG));
                lstSanPham.Add(new AutoCompleteEntry(Dislay, "%", "%"));
                KhoiTaoGiaTriComboBox(ref lstSanPham, ref cmbSanPham, maTruyVan, lstDieuKien);

                //Load combobox hình thức trả lãi 
                lstDieuKien.Clear();
                while (cmbThoiHanVay.Items.Count > 0)
                {
                    cmbThoiHanVay.Items.RemoveAt(0);
                }
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.TAN_SUAT));
                KhoiTaoGiaTriComboBox(ref lstSourceAThoiHanVay, ref cmbThoiHanVay, maTruyVan, lstDieuKien);
                cmbThoiHanVay.SelectedIndex = lstSourceAThoiHanVay.IndexOf(lstSourceAThoiHanVay.FirstOrDefault(i => i.KeywordStrings.First().Equals(BusinessConstant.TAN_SUAT.THANG.layGiaTri())));

                //Load combobox hình thức trả lãi   
                while (cmbThoiHanVayDen.Items.Count > 0)
                {
                    cmbThoiHanVayDen.Items.RemoveAt(0);
                }
                KhoiTaoGiaTriComboBox(ref lstSourceAThoiHanVayDen, ref cmbThoiHanVayDen, maTruyVan, lstDieuKien);
                cmbThoiHanVayDen.SelectedIndex = lstSourceAThoiHanVayDen.IndexOf(lstSourceAThoiHanVayDen.FirstOrDefault(i => i.KeywordStrings.First().Equals(BusinessConstant.TAN_SUAT.THANG.layGiaTri())));

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }


        }

        void KhoiTaoGiaTriComboBox(ref List<AutoCompleteEntry> lstAutoComplete, ref RadComboBox cmbControl, string sMaTruyVan, List<string> lstDKien = null)
        {
            AutoComboBox autoComboBox = new AutoComboBox();
            autoComboBox.GenAutoComboBox(ref lstAutoComplete, ref cmbControl, sMaTruyVan, lstDKien);
        }

        private void cmbThoiHanVay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                cmbThoiHanVayDen.SelectedIndex = cmbThoiHanVay.SelectedIndex;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void cmbThoiHanVayDen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                cmbThoiHanVay.SelectedIndex = cmbThoiHanVayDen.SelectedIndex;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        void LoadTreeview()
        {
            dtSourceTree = new TinDungProcess().getDanhSachDonVi(ClientInformation.MaDonViQuanLy, ClientInformation.TenDangNhap).Tables[0];
            try
            {
                RadTreeViewItem Item = new RadTreeViewItem();
                BuildSubTreeKhuVuc(Item, null, 0);
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void BuildSubTreeKhuVuc(RadTreeViewItem Item, DataRow dr, int iLevel)
        {

            List<DataRow> lstDataRow = null;
            if (dr != null)
                lstDataRow = dtSourceTree.Select("MA_DVI_CHA='" + dr["MA_DVI"] + "' AND LEVEL=" + iLevel).OrderBy(row => row[4]).ToList();
            //lstDataRow = dtTreeDLy.Select("MA_DVI_CHA='" + dr["MA_DVI"] + "' AND LEVEL=" + iLevel).ToList();
            else
                lstDataRow = dtSourceTree.Select("MA_DVI_CHA=''").OrderBy(row => row[4]).ToList();
            //lstDataRow = dtTreeDLy.Select("MA_DVI_CHA=''").ToList();
            foreach (DataRow row in lstDataRow)
            {
                RadTreeViewItem subItem = new RadTreeViewItem();
                subItem.Header = row["TEN_GDICH"].ToString();
                subItem.Tag = row["MA_DVI"].ToString();
                //subItem.IsExpanded = true;
                subItem.IsChecked = true;
                if (row["LEVEL"].Equals(0))
                    tvwKhachHangDS.Items.Add(subItem);
                else
                    Item.Items.Add(subItem);
                BuildSubTreeKhuVuc(subItem, row, Convert.ToInt32(row["LEVEL"]) + 1);
            }
        }


        private void LoadgrdHDTDDS()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);

                string sMaTrangThaiNVu = ucTrangThaiNVu.GetItemsSelected();
                //string maLoaiVay = lstLoaiVay.ElementAt(cmbLoaiVay.SelectedIndex).KeywordStrings.FirstOrDefault();
                //string maPhuongThucChoVay = lstPhuongThucChoVay.ElementAt(cmbPhuongThucChoVay.SelectedIndex).KeywordStrings.FirstOrDefault();
                if (tvwKhachHangDS.SelectedItem == null)
                    tvwKhachHangDS.SelectedItem = tvwKhachHangDS.Items[0];
                string ListKVuc = "";
                if (((RadTreeViewItem)tvwKhachHangDS.SelectedItem).Tag.ToString().Substring(0, 3).Equals("DVI"))
                {
                    RadTreeViewItem itemDVI = (RadTreeViewItem)tvwKhachHangDS.SelectedItem;
                    foreach (RadTreeViewItem item in itemDVI.Items)
                    {
                        if (item.Tag.ToString().Substring(0, 3).Equals("CNH"))
                            ListKVuc += ",''" + item.Tag.ToString() + "''";
                    }
                    ListKVuc = ListKVuc.Substring(3);
                    ListKVuc = ListKVuc.Substring(0, ListKVuc.Length - 2);
                }
                else
                    ListKVuc = ((RadTreeViewItem)tvwKhachHangDS.SelectedItem).Tag.ToString();

                // Load dữ liệu lên grid
                if (ucTrangThaiNVu.GetItemsSelected() == "NULL")
                {
                    LDatatable.AddParameter(ref dt, "@TRANG_THAI_NVU", "STRING", "");
                }
                else
                {
                    LDatatable.AddParameter(ref dt, "@TRANG_THAI_NVU", "STRING", ucTrangThaiNVu.GetItemsSelected());
                }

                LDatatable.AddParameter(ref dt, "@MA_SAN_PHAM", "STRING", txtSoHDTD.Text.Trim());
                //LDatatable.AddParameter(ref dt, "@TEN_SAN_PHAM", "STRING", txtTenSanPham.Text.Trim());
                //LDatatable.AddParameter(ref dt, "@MA_LOAI_VAY", "STRING", maLoaiVay);
                //LDatatable.AddParameter(ref dt, "@MA_PHUONG_THUC_CHO_VAY", "STRING", maPhuongThucChoVay);
                if (raddtNgayHopDong.Value != null)
                {
                    LDatatable.AddParameter(ref dt, "@INP_NGAY_HIEU_LUC_TU", "STRING", Convert.ToDateTime(raddtNgayHopDong.Value).ToString("yyyyMMdd"));
                }
                else
                {
                    LDatatable.AddParameter(ref dt, "@INP_NGAY_HIEU_LUC_TU", "STRING", "");
                }
                if (raddtNgayHopDongDen.Value != null)
                {
                    LDatatable.AddParameter(ref dt, "@INP_NGAY_HIEU_LUC_DEN", "STRING", Convert.ToDateTime(raddtNgayHopDongDen.Value).ToString("yyyyMMdd"));
                }
                else
                {
                    LDatatable.AddParameter(ref dt, "@INP_NGAY_HIEU_LUC_DEN", "STRING", "");
                }
                if (raddtNgayDaoHan.Value != null)
                {
                    LDatatable.AddParameter(ref dt, "@INP_NGAY_HET_HIEU_LUC_TU", "STRING", Convert.ToDateTime(raddtNgayDaoHan.Value).ToString("yyyyMMdd"));
                }
                else
                {
                    LDatatable.AddParameter(ref dt, "@INP_NGAY_HET_HIEU_LUC_TU", "STRING", "");
                }
                if (raddtNgayDaoHanDen.Value != null)
                {
                    LDatatable.AddParameter(ref dt, "@INP_NGAY_HET_HIEU_LUC_DEN", "STRING", Convert.ToDateTime(raddtNgayDaoHanDen.Value).ToString("yyyyMMdd"));
                }
                else
                {
                    LDatatable.AddParameter(ref dt, "@INP_NGAY_HET_HIEU_LUC_DEN", "STRING", "");
                }
                LDatatable.AddParameter(ref dt, "@INP_DON_VI", "string", ListKVuc);
                LDatatable.AddParameter(ref dt, "@INP_USER", "string", ClientInformation.TenDangNhap);
                LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "string", ClientInformation.MaDonViQuanLy);
                LDatatable.AddParameter(ref dt, "@StartRow", "INT", StartRow.ToString());
                LDatatable.AddParameter(ref dt, "@EndRow", "INT", EndRow.ToString());

                Presentation.Process.TinDungTTProcess process = new Presentation.Process.TinDungTTProcess();
                DataSet ds = process.GetDanhSachSanPham(dt);
                if (ds != null && ds.Tables.Count > 0)
                {
                    grdHDTDDS.ItemsSource = ds.Tables[0].DefaultView;
                }
                else
                {
                    grdHDTDDS.ItemsSource = null;
                }


            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Load dữ liệu lên Form
        /// </summary>
        private void LoadDuLieu(object sender, EventArgs e)
        {
            try
            {
                LoadCombobox();
                LoadTreeview();

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Load lại dữ liệu khi có thay đổi từ form chi tiết
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void userControl_OnSavingCompleted(object sender, EventArgs e)
        {
            LoadgrdHDTDDS();
            CommonFunction.GoToPosition(currentID, ref grdHDTDDS, radPage, nudPageSize);

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
            PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grdHDTDDS, txtTimKiemNhanh.Text);
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

        private void grdHDTDDS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                BeforeView();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Sự kiện thay đổi số bản ghi trên một page hiển thị
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudPageSize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (grdHDTDDS != null && grdHDTDDS.DataContext != null)
            {
                DataTable dt = ((DataView)grdHDTDDS.DataContext).Table;
                if (dt != null)
                {
                    radPage.PageSize = (int)nudPageSize.Value;
                    grdHDTDDS.DataContext = dt.DefaultView;
                }
            }
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(grdHDTDDS);
        }

        private void TimKiemPhanTrang()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            DataTable dt = null;
            LDatatable.MakeParameterTable(ref dt);


            Mouse.OverrideCursor = Cursors.Arrow;
        }
        #endregion

        #region Xử lý nghiệp vụ
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
            ucHopDongTinDungCT userControl = new ucHopDongTinDungCT();

            userControl.Action = DatabaseConstant.Action.THEM;
            userControl.OnSavingCompleted += new EventHandler(userControl_OnSavingCompleted);

            window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.TD_SAN_PHAMTT);
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
                        currentPage = grdHDTDDS.Items.PageIndex;
                        currentPosition = grdHDTDDS.Items.CurrentPosition;
                        currentID = int.Parse(listDataRow.First()["ID"].ToString());

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
                ucHopDongTinDungCT userControl = new ucHopDongTinDungCT();

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
                    userControl.OnSavingCompleted += new EventHandler(userControl_OnSavingCompleted);
                    userControl.Action = DatabaseConstant.Action.SUA;
                    userControl.ID = id;

                    Window window = new Window();
                    window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.TD_SAN_PHAMTT);
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
                        currentPage = grdHDTDDS.Items.PageIndex;
                        currentPosition = grdHDTDDS.Items.CurrentPosition;
                        currentID = int.Parse(listDataRow.First()["ID"].ToString());
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
                ucHopDongTinDungCT userControl = new ucHopDongTinDungCT();

                userControl.OnSavingCompleted += new EventHandler(userControl_OnSavingCompleted);
                userControl.Action = DatabaseConstant.Action.XEM;
                userControl.ID = id;

                Window window = new Window();
                window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.TD_SAN_PHAMTT);
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

                            bool retLockData = process.LockData(DatabaseConstant.Module.TDTT,
                                DatabaseConstant.Function.TD_SAN_PHAMTT,
                                DatabaseConstant.Table.TD_SAN_PHAMTT,
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
            TinDungTTProcess ProcessTinDungTT = new TinDungTTProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                List<TDTT_SAN_PHAM> lstSanPham = new List<TDTT_SAN_PHAM>();

                TDTT_SAN_PHAM obj = null;
                foreach (int id in listId)
                {
                    obj = new TDTT_SAN_PHAM();
                    obj.ID = id;
                    lstSanPham.Add(obj);
                }
                bool ret = ProcessTinDungTT.DanhSachSanPham(DatabaseConstant.Action.XOA, ref lstSanPham, ref listClientResponseDetail);

                AfterDelete(ret, listId, listClientResponseDetail);
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
        /// Sau khi xóa
        /// </summary>
        /// <param name="ret"></param>
        private void AfterDelete(bool ret, List<int> listId, List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
                LoadgrdHDTDDS();
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                LoadgrdHDTDDS();
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTT,
                DatabaseConstant.Function.TD_SAN_PHAMTT,
                DatabaseConstant.Table.TD_SAN_PHAMTT,
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

                            bool retLockData = process.LockData(DatabaseConstant.Module.TDTT,
                                DatabaseConstant.Function.TD_SAN_PHAMTT,
                                DatabaseConstant.Table.TD_SAN_PHAMTT,
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
            TinDungTTProcess ProcessTinDungTT = new TinDungTTProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                List<TDTT_SAN_PHAM> lstSanPham = new List<TDTT_SAN_PHAM>();

                TDTT_SAN_PHAM obj = null;
                foreach (int id in listId)
                {
                    obj = new TDTT_SAN_PHAM();
                    obj.ID = id;
                    lstSanPham.Add(obj);
                }
                bool ret = ProcessTinDungTT.DanhSachSanPham(DatabaseConstant.Action.DUYET, ref lstSanPham, ref listClientResponseDetail);

                AfterApprove(ret, listId, listClientResponseDetail);
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
        /// Sau khi duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void AfterApprove(bool ret, List<int> listId, List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);
                LoadgrdHDTDDS();
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                LoadgrdHDTDDS();
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTT,
                DatabaseConstant.Function.TD_SAN_PHAMTT,
                DatabaseConstant.Table.TD_SAN_PHAMTT,
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
                        MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question);
                        if (ret == MessageBoxResult.Yes)
                        {
                            // Yêu cầu lock bản ghi cần xử lý
                            UtilitiesProcess process = new UtilitiesProcess();
                            List<int> listLockedId = new List<int>();

                            bool retLockData = process.LockData(DatabaseConstant.Module.TDTT,
                                DatabaseConstant.Function.TD_SAN_PHAMTT,
                                DatabaseConstant.Table.TD_SAN_PHAMTT,
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
            TinDungTTProcess ProcessTinDungTT = new TinDungTTProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                List<TDTT_SAN_PHAM> lstSanPham = new List<TDTT_SAN_PHAM>();

                TDTT_SAN_PHAM obj = null;
                foreach (int id in listId)
                {
                    obj = new TDTT_SAN_PHAM();
                    obj.ID = id;
                    lstSanPham.Add(obj);
                }
                bool ret = ProcessTinDungTT.DanhSachSanPham(DatabaseConstant.Action.THOAI_DUYET, ref lstSanPham, ref listClientResponseDetail);

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
                    LoadgrdHDTDDS();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    LoadgrdHDTDDS();
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTT,
                    DatabaseConstant.Function.TD_SAN_PHAMTT,
                    DatabaseConstant.Table.TD_SAN_PHAMTT,
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
            TinDungTTProcess ProcessTinDungTT = new TinDungTTProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                List<TDTT_SAN_PHAM> lstSanPham = new List<TDTT_SAN_PHAM>();

                TDTT_SAN_PHAM obj = null;
                foreach (int id in listId)
                {
                    obj = new TDTT_SAN_PHAM();
                    obj.ID = id;
                    lstSanPham.Add(obj);
                }
                bool ret = ProcessTinDungTT.DanhSachSanPham(DatabaseConstant.Action.TU_CHOI_DUYET, ref lstSanPham, ref listClientResponseDetail);

                AfterRefuse(ret, listId, listClientResponseDetail);
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
                    LoadgrdHDTDDS();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    LoadgrdHDTDDS();
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTT,
                    DatabaseConstant.Function.TD_SAN_PHAMTT,
                    DatabaseConstant.Table.TD_SAN_PHAMTT,
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
        /// Tìm kiếm nâng cao
        /// </summary>
        private void TimKiem()
        {
            try
            {
                LoadgrdHDTDDS();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Lấy lại dữ liệu
        /// </summary>
        private void LayLai()
        {
            LoadgrdHDTDDS();
        }

        /// <summary>
        /// Lấy danh sách id được chọn
        /// </summary>
        /// <returns></returns>
        private List<DataRowView> getListSeletedDataRow()
        {
            try
            {
                List<DataRowView> listDataRow = new List<DataRowView>();

                if (grdHDTDDS.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < grdHDTDDS.SelectedItems.Count; i++)
                    {
                        DataRowView dr = (DataRowView)grdHDTDDS.SelectedItems[i];
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
