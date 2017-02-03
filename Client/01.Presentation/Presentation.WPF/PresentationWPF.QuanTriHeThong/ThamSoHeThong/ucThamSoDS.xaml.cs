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
using Presentation.Process.TruyVanServiceRef;
using PresentationWPF.CustomControl;
using Presentation.Process.QuanTriHeThongServiceRef;
using Utilities.Common;
using Microsoft.Windows.Controls.Ribbon;
using System.Collections;
using Presentation.Process.Common;
using System.Reflection;
using Telerik.Windows.Controls;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;

namespace PresentationWPF.QuanTriHeThong.ThamSoHeThong
{
    /// <summary>
    /// Interaction logic for ucThamSoDS.xaml
    /// </summary>
    public partial class ucThamSoDS : UserControl
    {
        #region Khai bao

        List<HT_TSO> dsThamSo = new List<HT_TSO>();
        List<HT_TSO_LOAI> dsThamSoLoai = new List<HT_TSO_LOAI>();
        DataTable dt = new DataTable();
        QuanTriHeThongProcess qtht = new QuanTriHeThongProcess();
        HT_TSO_LOAI obj = new HT_TSO_LOAI();
        bool treViewUpdate = false;
        bool isLoaded = false;
        private static int idLoaiCha = 0;
        private static string maLoaiCha = string.Empty;
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        List<AutoCompleteEntry> lstSourceNguon = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();
        private string loaiThamSo = BusinessConstant.LoaiThamSo.TW.layGiaTri();

        private string maNguon;

        public string MaNguon
        {
            get { return maNguon; }
            set { maNguon = value; }
        }

        public static RoutedCommand AddCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ViewCommand = new RoutedCommand();
        public static RoutedCommand SearchCommand = new RoutedCommand();
        public static RoutedCommand ReloadCommand = new RoutedCommand();
        public static RoutedCommand ExportCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        //Lưu danh sách các item được chọn trong treeview
        private List<string> lstChecked = new List<string>();
        #endregion

        #region Khoi tao

        public ucThamSoDS()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.QuanTriHeThong;component/ThamSoHeThong/ucThamSoDS.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindShortkey();

            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            // Khởi tạo điều kiện gọi danh mục
            List<string> lstDieuKien = new List<string>();
            // Gán giá trị điều kiện
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.NGUON_TAO_DU_LIEU));
            auto.GenAutoComboBox(ref lstSourceNguon, ref cmbNguon, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

            // khởi tạo combobox
            lstDieuKien = new List<string>();
            lstDieuKien.Add("'" + DatabaseConstant.ToChucDonVi.HSO.getValue() + "', '" + DatabaseConstant.ToChucDonVi.CNH.getValue() + "'");
            auto.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOLOAI.getValue(), lstDieuKien);

            radPage.PageSize = (int)nudPageSize.Value;
            LoadDuLieu();
            HideControl();
        }

        #endregion

        #region Dang ky hot key

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
                Luu(DatabaseConstant.Action.THEM);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                //Luu(DatabaseConstant.Action.SUA);
                beforeModifyValue();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                LuuTrangThai(DatabaseConstant.Action.XOA);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
                //Luu(DatabaseConstant.Action.XEM);
                beforeViewValue();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            {
                LoadDuLieu();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
                LayLai();
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

        #region Dang ky shortcut key

        private void BindShortkey()
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
                    if(key !=null)
                        InputBindings.Add(key);
                }
            }
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //Luu(DatabaseConstant.Action.SUA);
            beforeModifyValue();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LuuTrangThai(DatabaseConstant.Action.XOA);
        }

        private void ViewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ViewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //Luu(DatabaseConstant.Action.XEM);
            beforeViewValue();
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
            LayLai();
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
            onClose();
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
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (!isLoaded)
                {
                    txtTimKiemNhanh.KeyDown += txtTimKiemNhanh_KeyDown;
                    txtTimKiemNhanh.GotFocus += txtTimKiemNhanh_GotFocus;
                    txtTimKiemNhanh.LostFocus += txtTimKiemNhanh_LostFocus;
                    txtThamSoCha.KeyDown += txtThamSoCha_KeyDown;
                    tvwTree.SelectionChanged += tvwTree_SelectionChanged;
                    tvwTree.MouseDoubleClick += tvwTree_MouseDoubleClick;
                    grDanhSach.MouseDoubleClick += grDanhSach_MouseDoubleClick;
                    cmbDonVi.SelectionChanged += cmbDonVi_SelectionChanged;
                    txtTimKiemNhanh.Focus();
                    isLoaded = true;
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void HideControl()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HeThong hethong = new HeThong();
                ArrayList arr = new ArrayList();
                if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_SA.layGiaTri()) ||
                    ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTTW.layGiaTri()))
                    arr = hethong.SetVisibleControl("PresentationWPF.QuanTriHeThong.ThamSoHeThong.ucThamSoDS", "MANAGE");
                else
                    arr = hethong.SetVisibleControl("PresentationWPF.QuanTriHeThong.ThamSoHeThong.ucThamSoDS", "SETVALUE");
                foreach (List<string> lst in arr)
                {
                    object item = gridMain.FindName(lst.First());
                    string strProperty = lst.ElementAt(1);
                    PropertyInfo prty = item.GetType().GetProperty(strProperty);
                    if (strProperty.Equals("Visibility"))
                    {
                        if (lst.ElementAt(2).Equals("0"))
                            prty.SetValue(item, Visibility.Collapsed, null);
                        else if (lst.ElementAt(2).Equals("1"))
                            prty.SetValue(item, Visibility.Visible, null);
                        else
                            prty.SetValue(item, Visibility.Hidden, null);
                    }
                    else
                    {
                        if (lst.ElementAt(2).Equals("0"))
                            prty.SetValue(item, false, null);
                        else
                            prty.SetValue(item, true, null);
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Sự kiện tìm kiếm nhanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTimKiemNhanh_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                if (txtTimKiemNhanh.Text == LLanguage.SearchResourceByKey("U.DungChung.TimKiemNhanh"))
                {
                    return;
                }
                PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grDanhSach, txtTimKiemNhanh.Text);
                loadWidthColumn();
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
                Luu(DatabaseConstant.Action.THEM);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                //Luu(DatabaseConstant.Action.SUA);
                beforeModifyValue();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                LuuTrangThai(DatabaseConstant.Action.XOA);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                LuuTrangThai(DatabaseConstant.Action.DUYET);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                LuuTrangThai(DatabaseConstant.Action.TU_CHOI_DUYET);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                LuuTrangThai(DatabaseConstant.Action.THOAI_DUYET);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
                //Luu(DatabaseConstant.Action.XEM);
                beforeViewValue();
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
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                onClose();
            }
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
                if (dt.Rows.Count > 0)
                    grDanhSach.ItemsSource = dt;
                lblSumThamSo.Content = dt.Rows.Count;
                loadWidthColumn();
            }
        }

        /// <summary>
        /// Lấy lại dữ liệu
        /// </summary>
        private void LayLai()
        {
            LoadDuLieu();
        }

        /// <summary>
        /// Load dữ liệu lên datagrid
        /// </summary>
        private void LoadDuLieu()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                dsThamSoLoai = new List<HT_TSO_LOAI>();
                dsThamSoLoai = qtht.layLoaiThamSo();

                while (tvwTree.Items.Count > 0)
                    tvwTree.Items.RemoveAt(0);
                UpdateLayout();
                RadTreeViewItem rootItem = new RadTreeViewItem();
                rootItem.Header = LLanguage.SearchResourceByKey("U.QuanTriHeThong.ThamSo.ucThamSoDS.LoaiThamSo");
                rootItem.Tag = null;
                rootItem.IsExpanded = true;
                tvwTree.Items.Add(rootItem);
                bool noneSelect = true;
                foreach (HT_TSO_LOAI item in dsThamSoLoai)
                {
                    RadTreeViewItem node = new RadTreeViewItem();
                    node.Header = item.TEN_TSO_LOAI;
                    node.Tag = item.MA_TSO_LOAI;
                    node.IsExpanded = true;
                    if (noneSelect)
                    {
                        node.IsSelected = true;
                        noneSelect = false;
                    }
                    rootItem.Items.Add(node);
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void BuildGrid()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                string maDonVi = ClientInformation.MaDonVi;

                // Lấy loại tham số
                if (tvwTree.SelectedItem != null)
                    if (((RadTreeViewItem)tvwTree.SelectedItem).Tag != null)
                        loaiThamSo = ((RadTreeViewItem)tvwTree.SelectedItem).Tag.ToString();

                // Ẩn/hiện thông tin đơn vị tùy theo quyền người dùng
                if (loaiThamSo != null)
                {
                    if (loaiThamSo.Equals(BusinessConstant.LoaiThamSo.DV.layGiaTri()))
                    {
                        if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_SA.layGiaTri()) ||
                        ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTTW.layGiaTri()) ||
                        ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVTW.layGiaTri()))
                        {
                            stpDonVi.Visibility = Visibility.Visible;
                            maDonVi = lstSourceDonVi.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.First();                                                        
                        }
                        else
                        {
                            stpDonVi.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        stpDonVi.Visibility = Visibility.Collapsed;
                    }
                }

                // Lấy danh sách tham số theo loại tham số
                dsThamSo = qtht.layThamSoHeThong(loaiThamSo, maDonVi);

                List<HT_TSO> dsThamSoGrid = new List<HT_TSO>();
                if (loaiThamSo != null && loaiThamSo.Length > 1)
                    dsThamSoGrid = dsThamSo.Where(e => e.MA_TSO_LOAI.Equals(loaiThamSo)).ToList();
                else
                    dsThamSoGrid = dsThamSo;

                dt = new DataTable();
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.ThamSo.ucThamSoDS.MaThamSo"), typeof(string));
                dt.Columns.Add("STT", typeof(string));
                dt.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.ThamSo.ucThamSoDS.TenThamSo"), typeof(string));
                dt.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.ThamSo.ucThamSoDS.MoTaThamSo"), typeof(string));
                dt.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.ThamSo.ucThamSoDS.GiaTriThamSo"), typeof(string));
                dt.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.ThamSo.ucThamSoDS.TrangThaiThamSo"), typeof(string));
                dt.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.ThamSo.ucThamSoDS.LoaiThamSo"), typeof(string));
                dt.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.ThamSo.ucThamSoDS.MaDonVi"), typeof(string));
                int stt = 0;
                foreach (var item in dsThamSo)
                {
                    DataRow r = dt.NewRow();
                    stt = stt + 1;
                    r[0] = item.ID;                    
                    r[1] = item.MA_TSO;
                    r[2] = stt;
                    r[3] = item.TEN_TSO;
                    r[4] = item.MO_TA;
                    r[5] = item.GIA_TRI;
                    r[6] = BusinessConstant.layNgonNguSuDung(item.TTHAI_BGHI);
                    r[7] = item.MA_TSO_LOAI;
                    r[8] = item.MA_DVI_QLY;
                    dt.Rows.Add(r);
                }
                // đổ source lên lưới
                grDanhSach.ItemsSource = dt.DefaultView;
                lblSumThamSo.Content = dt.Rows.Count;
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void loadWidthColumn()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                for (int i = 0; i < grDanhSach.Columns.Count; i++)
                {
                    if (i == 0)
                        grDanhSach.Columns[i].IsVisible = false;
                    if (i == 1)
                        grDanhSach.Columns[i].IsVisible = false;
                    else if (i == 2)
                        grDanhSach.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(40, Telerik.Windows.Controls.GridViewLengthUnitType.Pixel);
                    else if (i == 3)
                        grDanhSach.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Star);
                    else if (i == 4)
                        grDanhSach.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Star);
                    else if (i == 5)
                        grDanhSach.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Star);
                    else if (i == 6)
                        grDanhSach.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Auto);
                    else if (i == 7)
                        grDanhSach.Columns[i].IsVisible = false;
                    else if (i == 8)
                        grDanhSach.Columns[i].IsVisible = false;
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void grDanhSach_Loaded(object sender, RoutedEventArgs e)
        {
            loadWidthColumn();
        }

        private void tvwTree_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BuildGrid(); loadWidthColumn();
        }

        private void grDanhSach_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //FrameworkElement originalSender = e.OriginalSource as FrameworkElement;
            //if (originalSender != null)
            //{
            //    Luu(DatabaseConstant.Action.XEM);
            //}
            beforeViewValue();
        }

        private void tvwTree_Loaded(object sender, RoutedEventArgs e)
        {
            BuildGrid(); loadWidthColumn();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool bKetQua = false;
                var process = new QuanTriHeThongProcess();
                if (treViewUpdate)
                {
                    if (obj.ID > 0)
                    {
                        obj.MA_TSO_LOAI = txtMaLoai.Text;
                        obj.TEN_TSO_LOAI = txtTenLoai.Text;
                        obj.NGUON_TAO_DL = lstSourceNguon.ElementAt(cmbNguon.SelectedIndex).KeywordStrings.First();
                        if (process.capNhatLoaiThamSo(DatabaseConstant.Action.LUU, obj, null) == ApplicationConstant.ResponseStatus.THANH_CONG)
                            bKetQua = true;
                        treViewUpdate = false;
                    }
                }
                else
                {
                    obj = new HT_TSO_LOAI();
                    obj.MA_TSO_LOAI = txtMaLoai.Text;
                    obj.TEN_TSO_LOAI = txtTenLoai.Text;
                    obj.NGUON_TAO_DL = lstSourceNguon.ElementAt(cmbNguon.SelectedIndex).KeywordStrings.First();
                    obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                    obj.MA_DVI_TAO = ClientInformation.MaDonVi;
                    obj.NGAY_NHAP = DateTime.Today.ToString("yyyyMMdd");
                    obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    obj.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    if (process.capNhatLoaiThamSo(DatabaseConstant.Action.LUU, obj, null) == ApplicationConstant.ResponseStatus.THANH_CONG)
                        bKetQua = true;
                }
                if (bKetQua)
                {
                    dsThamSoLoai = qtht.layLoaiThamSo();

                    while (tvwTree.Items.Count > 0)
                        tvwTree.Items.RemoveAt(0);
                    UpdateLayout();
                    RadTreeViewItem rootItem = new RadTreeViewItem();
                    rootItem.Header = LLanguage.SearchResourceByKey("U.QuanTriHeThong.ThamSo.ucThamSoDS.LoaiThamSo");
                    rootItem.Tag = null;
                    rootItem.IsExpanded = true;
                    tvwTree.Items.Add(rootItem);
                    bool noneSelect = true;
                    foreach (HT_TSO_LOAI item in dsThamSoLoai)
                    {
                        RadTreeViewItem node = new RadTreeViewItem();
                        node.Header = item.TEN_TSO_LOAI;
                        node.Tag = item.MA_TSO_LOAI;
                        node.IsExpanded = true;
                        if (noneSelect)
                        {
                            node.IsSelected = true;
                            noneSelect = false;
                        }
                        rootItem.Items.Add(node);
                    }
                    txtMaLoai.Text = string.Empty;
                    txtTenLoai.Text = string.Empty;
                    txtThamSoCha.Text = string.Empty;
                    txtBieuTuong.Text = string.Empty;
                }
                LoadDuLieu();
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (obj.ID > 0)
                {
                    var process = new QuanTriHeThongProcess();
                    List<int> id = new List<int>();
                    id.Add(obj.ID);
                    process.capNhatLoaiThamSo(DatabaseConstant.Action.XOA, null, id);
                }
                dsThamSoLoai = qtht.layLoaiThamSo();

                while (tvwTree.Items.Count > 0)
                    tvwTree.Items.RemoveAt(0);
                UpdateLayout();
                RadTreeViewItem rootItem = new RadTreeViewItem();
                rootItem.Header = LLanguage.SearchResourceByKey("U.QuanTriHeThong.ThamSo.ucThamSoDS.LoaiThamSo");
                rootItem.Tag = null;
                rootItem.IsExpanded = true;
                tvwTree.Items.Add(rootItem);
                bool noneSelect = true;
                foreach (HT_TSO_LOAI item in dsThamSoLoai)
                {
                    RadTreeViewItem node = new RadTreeViewItem();
                    node.Header = item.TEN_TSO_LOAI;
                    node.Tag = item.MA_TSO_LOAI;
                    node.IsExpanded = true;
                    if (noneSelect)
                    {
                        node.IsSelected = true;
                        noneSelect = false;
                    }
                    rootItem.Items.Add(node);
                }
                LoadDuLieu();
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void cmbDonVi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BuildGrid();
            loadWidthColumn();
        }

        private void tvwTree_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;
                RadTreeViewItem Loai = (RadTreeViewItem)tvwTree.SelectedItem;
                if (Loai.Tag != null && Loai.Tag.ToString().Length > 1)
                {
                    var process = new QuanTriHeThongProcess();
                    HT_TSO_LOAI loai = new HT_TSO_LOAI();
                    string maLoai = ((RadTreeViewItem)tvwTree.SelectedItem).Tag.ToString();
                    loai = process.layLoaiThamSo().FirstOrDefault(l => l.MA_TSO_LOAI.Equals(maLoai));
                    txtMaLoai.Text = loai.MA_TSO_LOAI;
                    txtTenLoai.Text = loai.TEN_TSO_LOAI;
                    setMaNguonDL(loai.NGUON_TAO_DL);
                    treViewUpdate = true;
                    expLoai.IsExpanded = true;
                    txtMaLoai.Focus();
                }
                this.Cursor = Cursors.Arrow;
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void cmb_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                ((RadComboBox)sender).IsDropDownOpen = true;
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void setMaNguonDL(string maChon)
        {
            try
            {
                cmbNguon.SelectedIndex = lstSourceNguon.IndexOf(lstSourceNguon.FirstOrDefault(i => i.KeywordStrings.First().Equals(maChon)));
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(grDanhSach);
        }

        #endregion

        #region Xu ly nghiep vu

        public void Luu(DatabaseConstant.Action action)
        {
            if (grDanhSach.SelectedItems.Count == 1 || action == DatabaseConstant.Action.THEM || action == DatabaseConstant.Action.SUA)
            {
                try
                {
                    DataRow dr = (DataRow)grDanhSach.SelectedItems.First();

                    ucThamSoCT ct = new ucThamSoCT();
                    ct.luuAction = action;
                    if (action == DatabaseConstant.Action.SUA || action == DatabaseConstant.Action.XEM)
                        ct.obj = dsThamSo.FirstOrDefault(e => e.ID == Convert.ToInt32(dr[0]));
                    Window window = new Window();
                    window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.HT_THAM_SO);
                    window.Content = ct;
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.ShowDialog();
                    BuildGrid();
                    loadWidthColumn();
                }
                catch (System.Exception ex)
                {
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
            }
        }

        private void LuuTrangThai(DatabaseConstant.Action action)
        {
            QuanTriHeThongProcess process = new QuanTriHeThongProcess();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<int> lstID = new List<int>();
                foreach (var row in grDanhSach.SelectedItems)
                {
                    if (row is DataRow)
                    {
                        if ((int)((DataRow)row)[0] > 0)
                            lstID.Add((int)((DataRow)row)[0]);
                    }
                    else
                    {
                        if ((int)((DataRow)row)[0] > 0)
                            lstID.Add((int)((DataRow)row)[0]);
                    }
                }
                string loaiDoiTuong = ((RadTreeViewItem)tvwTree.SelectedItem).Tag.ToString();
                process.capNhatThamSo(action, null, lstID);
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Information);
                BuildGrid();
                loadWidthColumn();
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void btnThamSoCha_Click(object sender, RoutedEventArgs e)
        {
            ShowPopupLoaiThamSo();
        }

        private void txtThamSoCha_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.F3)
            {
                ShowPopupLoaiThamSo();
            }
        }

        private void ShowPopupLoaiThamSo()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                var process = new PopupProcess();
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_HT_TSO_LOAI.getValue());

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null)
                {
                    txtThamSoCha.Text = lstPopup[0][3].ToString();
                    maLoaiCha = lstPopup[0][2].ToString();
                    idLoaiCha = Convert.ToInt32(lstPopup[0][1].ToString());
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        #endregion

        /// <summary>
        /// Trước khi xem
        /// </summary>
        private void beforeView()
        {
            List<DataRowView> listDataRow = getListSeletedDataRow();
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
        /// Trước khi xem giá trị tham số
        /// </summary>
        private void beforeViewValue()
        {
            List<DataRowView> listDataRow = getListSeletedDataRow();
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
                    onViewValue(id);
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
                    int id = int.Parse(listDataRow.First()["id"].ToString());

                    // Yêu cầu lock bản ghi cần xử lý
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);

                    bool ret = true;
                    ret = process.LockData(DatabaseConstant.Module.QTHT,
                        DatabaseConstant.Function.HT_THAM_SO,
                        DatabaseConstant.Table.HT_TSO,
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
        /// Trước khi sửa giá trị tham số
        /// </summary>
        private void beforeModifyValue()
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
                    int id = int.Parse(listDataRow.First()["id"].ToString());
                    string loaiThamSoChon = listDataRow.First()[7].ToString();
                    string maDonViQuanLy = listDataRow.First()[8].ToString();

                    // Nếu người dùng là SA, TW thì không được sửa tham số đơn vị khác
                    if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_SA.layGiaTri()) ||
                        ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTTW.layGiaTri()) ||
                        ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVTW.layGiaTri()))
                    {
                        if (loaiThamSoChon.Equals(BusinessConstant.LoaiThamSo.DV.layGiaTri()))
                        {
                            if (!maDonViQuanLy.Equals(ClientInformation.MaDonVi))
                            {
                                LMessage.ShowMessage("Cannot modify other company's parameter", LMessage.MessageBoxType.Warning);
                                return;
                            }
                        }
                    }
                    // Nếu người dùng là DV thì không được sửa tham số TW
                    else
                    {
                        if (loaiThamSoChon.Equals(BusinessConstant.LoaiThamSo.TW.layGiaTri()) ||
                            loaiThamSoChon.Equals(BusinessConstant.LoaiThamSo.BC.layGiaTri()))
                        {
                            LMessage.ShowMessage("Cannot modify system parameter", LMessage.MessageBoxType.Warning);
                            return;                            
                        }
                        else
                        {
                            if (!maDonViQuanLy.Equals(ClientInformation.MaDonVi))
                            {
                                LMessage.ShowMessage("Cannot modify other company's parameter", LMessage.MessageBoxType.Warning);
                                return;
                            }
                        }
                    }
                    

                    // Yêu cầu lock bản ghi cần xử lý
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);

                    bool ret = true;
                    ret = process.LockData(DatabaseConstant.Module.QTHT,
                        DatabaseConstant.Function.HT_THAM_SO,
                        DatabaseConstant.Table.HT_TSO,
                        DatabaseConstant.Action.SUA,
                        listLockId);

                    // Nếu lock thành công >> cho phép xử lý
                    if (ret)
                    {
                        onModifyValue(id);
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

                        bool retLockData = true;
                        retLockData = process.LockData(DatabaseConstant.Module.QTHT,
                        DatabaseConstant.Function.HT_THAM_SO,
                        DatabaseConstant.Table.HT_TSO,
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
        /// Xem dữ liệu
        /// </summary>
        private void onView(int id)
        {
            ucThamSoCT ct = new ucThamSoCT();
            ct.luuAction = DatabaseConstant.Action.XEM;
            ct.obj = dsThamSo.FirstOrDefault(e => e.ID == id);
            Window window = new Window();
            window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.HT_THAM_SO);
            window.Content = ct;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
            BuildGrid();
            loadWidthColumn();
        }

        /// <summary>
        /// Xem dữ liệu tham số
        /// </summary>
        private void onViewValue(int id)
        {
            ucThamSoValue ct = new ucThamSoValue();
            ct.luuAction = DatabaseConstant.Action.XEM;
            ct.obj = dsThamSo.FirstOrDefault(e => e.ID == id);
            Window window = new Window();
            window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.HT_THAM_SO);
            window.Content = ct;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
            BuildGrid();
            loadWidthColumn();
        }

        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        private void onAddNew()
        {
            ucThamSoCT ct = new ucThamSoCT();
            ct.luuAction = DatabaseConstant.Action.THEM;
            Window window = new Window();
            window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.HT_THAM_SO);
            window.Content = ct;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
            BuildGrid();
            loadWidthColumn();
        }

        /// <summary>
        /// Sửa dữ liệu
        /// </summary>
        private void onModify(int id)
        {
            ucThamSoCT ct = new ucThamSoCT();
            ct.luuAction = DatabaseConstant.Action.XEM;
            ct.obj = dsThamSo.FirstOrDefault(e => e.ID == id);
            Window window = new Window();
            window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.HT_THAM_SO);
            window.Content = ct;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
            BuildGrid();
            loadWidthColumn();
        }

        /// <summary>
        /// Sửa dữ liệu
        /// </summary>
        private void onModifyValue(int id)
        {
            ucThamSoValue ct = new ucThamSoValue();
            ct.luuAction = DatabaseConstant.Action.SUA;
            ct.obj = dsThamSo.FirstOrDefault(e => e.ID == id);
            Window window = new Window();
            window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.HT_THAM_SO);
            window.Content = ct;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
            BuildGrid();
            loadWidthColumn();
        }

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        private void onDelete(List<int> listId)
        {
            QuanTriHeThongProcess process = new QuanTriHeThongProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = true;

                //ret = process.XoaListThamSo(listId.ToArray(), ref listClientResponseDetail);

                afterDelete(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess lockProcess = new UtilitiesProcess();

                bool retUnlockData = true;
                retUnlockData = lockProcess.LockData(DatabaseConstant.Module.QTHT,
                    DatabaseConstant.Function.HT_THAM_SO,
                    DatabaseConstant.Table.HT_TSO,
                    DatabaseConstant.Action.XOA,
                    listId);

                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
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
                BuildGrid();
                loadWidthColumn();
            }
            else
            {
                //LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                BuildGrid();
                loadWidthColumn();
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool retUnlockData = true;
            retUnlockData = process.LockData(DatabaseConstant.Module.QTHT,
                    DatabaseConstant.Function.HT_THAM_SO,
                    DatabaseConstant.Table.HT_TSO,
                    DatabaseConstant.Action.XOA,
                    listId);
        }

        /// <summary>
        /// Lấy danh sách id được chọn
        /// </summary>
        /// <returns></returns>
        private List<DataRowView> getListSeletedDataRow()
        {
            List<DataRowView> listDataRow = new List<DataRowView>();
            if (grDanhSach.SelectedItems.Count <= 0)
            {
                return listDataRow;
            }
            else
            {
                for (int i = 0; i < grDanhSach.SelectedItems.Count; i++)
                {
                    DataRowView dr = (DataRowView)grDanhSach.SelectedItems[i];
                    listDataRow.Add(dr);
                }
                return listDataRow;
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

        /// <summary>
        /// Hàm xử lý giải phóng lock dữ liệu cho form
        /// </summary>
        private void Release()
        {
            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();

            bool ret = process.UnlockDataFromFunctionByUser(DatabaseConstant.Module.QTHT,
                DatabaseConstant.Function.HT_THAM_SO);
        }

        /// <summary>
        /// Sự kiện unlod cho form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Release();

            CustomControl.CommonFunction.CloseUserControl(this);
        }
    }
}
