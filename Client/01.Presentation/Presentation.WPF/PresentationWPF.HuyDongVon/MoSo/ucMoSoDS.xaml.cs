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
using Presentation.Process;
using Presentation.Process.Common;
using Utilities.Common;
using Presentation.Process.DanhMucServiceRef;
using Presentation.Process.HuyDongVonServiceRef;
using PresentationWPF.CustomControl;
using Telerik.Windows.Controls;


namespace PresentationWPF.HuyDongVon.MoSo
{
    /// <summary>
    /// Interaction logic for ucMoSoDS.xaml
    /// </summary>
    public partial class ucMoSoDS : UserControl
    {

        #region Khai bao

        private string sSanPham = "";
        private string sIDCum = "";
        private string sIDNhom = "";
        private string sCBQL = "";

        List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKyHan = new List<AutoCompleteEntry>();

        DataTable dtTreeSanPham = new DataTable();
        DataTable dtTreeDonVi = new DataTable();
        DataTable dtTreeCBQL = new DataTable();

        DataTable dtSoTGui = new DataTable();

        private int currentPosition;
        private int currentPage;
        private int currentID;

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
        public ucMoSoDS()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.HuyDongVon;component/MoSo/ucMoSoDS.xaml", ref Toolbar, ref mnuGrid);
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
                        KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.Shift);
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
                    if(key !=null)
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
            //txtTimKiemNhanh.RaiseEvent(new RoutedEventArgs(Button.GotFocusEvent));
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
            raddtNgayDaoHanTu.Value = null;
            raddtNgayDaoHanDen.Value = null;
            raddtNgayMoSoTu.Value = null;
            raddtNgayMoSoDen.Value = null;
            numSoDuTu.Value = null;
            numSoDuDen.Value = null;
            numKyHanTu.Value = null;
            numKyHanDen.Value = null;
            cmbKyHanTu.SelectedIndex = -1;
            cmbKyHanDen.SelectedIndex = -1;
            txtTimKiemNhanh.Focus();
        }

        /// <summary>
        /// Load dữ liệu lên Form
        /// </summary>
        private void LoadDuLieu()
        {
            Dispatcher.CurrentDispatcher.DelayInvoke("LoadCombobox", () =>
            {
                LoadCombobox();
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("LoadTreeview", () =>
            {
                LoadTreeview();
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("LoadGrid", () =>
            {
                LoadGrid();
            }, TimeSpan.FromSeconds(0));
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

                //lstDieuKien.Add("'" + DatabaseConstant.ToChucDonVi.HSO.getValue() + "', '" + DatabaseConstant.ToChucDonVi.CNH.getValue() + "'");
                //auto.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOLOAI.getValue(), lstDieuKien, ClientInformation.MaDonVi);
                //if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()) || ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()))
                //    cmbDonVi.IsEnabled = false;

                lstDieuKien.Add(ClientInformation.TenDangNhap);
                lstDieuKien.Add(ClientInformation.MaDonVi);
                lstDieuKien.Add(DatabaseConstant.ToChucDonVi.PGD.getValue());
                auto.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVI_THEOPVI.getValue(), lstDieuKien, ClientInformation.MaDonViGiaoDich);

                //Load combobox hình thức trả lãi 
                lstDieuKien.Clear();
                while (cmbKyHanTu.Items.Count > 0)
                {
                    cmbKyHanTu.Items.RemoveAt(0);
                }
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.TAN_SUAT));
                auto.GenAutoComboBox(ref lstSourceKyHan, ref cmbKyHanTu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
                cmbKyHanTu.SelectedIndex = lstSourceKyHan.IndexOf(lstSourceKyHan.FirstOrDefault(i => i.KeywordStrings.First().Equals(BusinessConstant.TAN_SUAT.THANG.layGiaTri())));                

                //Load combobox hình thức trả lãi   
                while (cmbKyHanDen.Items.Count > 0)
                {
                    cmbKyHanDen.Items.RemoveAt(0);
                }
                auto.GenAutoComboBox(ref lstSourceKyHan, ref cmbKyHanDen, null);
                cmbKyHanDen.SelectedIndex = lstSourceKyHan.IndexOf(lstSourceKyHan.FirstOrDefault(i => i.KeywordStrings.First().Equals(BusinessConstant.TAN_SUAT.THANG.layGiaTri())));
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
                HuyDongVonProcess huyDongVonProcess = new HuyDongVonProcess();
                string sIdDonVi = "";
                string sMaDonVi = "";
                string sIdDonViQLy = "";
                string sMaDonViQLy = "";
                if (lstSourceDonVi != null)
                {
                    sIdDonVi = lstSourceDonVi.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.ElementAt(1);
                    sMaDonVi = lstSourceDonVi.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.ElementAt(0);
                    DM_DON_VI objDonVi = new DanhMucProcess().getDonViById(Convert.ToInt32(sIdDonVi));

                    if (objDonVi.LOAI_DVI.Equals(DatabaseConstant.ToChucDonVi.HSO.getValue()) || objDonVi.LOAI_DVI.Equals(DatabaseConstant.ToChucDonVi.CNH.getValue()))
                    {
                        sIdDonViQLy = objDonVi.ID.ToString();
                        sMaDonViQLy = objDonVi.MA_DVI;
                    }
                    else
                    {
                        sIdDonViQLy = objDonVi.ID_DVI_CHA.ToString();
                        sMaDonViQLy = objDonVi.MA_DVI_CHA;
                    }

                }

                dtTreeSanPham = huyDongVonProcess.GetTreeSanPham(sMaDonViQLy).Tables[0];
                dtTreeDonVi = huyDongVonProcess.GetTreeDonViNhom(ClientInformation.MaDonViQuanLy, ClientInformation.TenDangNhap, sMaDonViQLy).Tables[0];
                dtTreeCBQL = huyDongVonProcess.GetTreeCBQL(sMaDonViQLy).Tables[0];

                itemSanPham.Items.Clear();
                //Cấu trúc của Tag: GiaTri#Level#LoaiTree ( VD: MaSP001#2#SAN_PHAM)
                itemSanPham.Tag = "0#0#SAN_PHAM";
                itemSanPham.IsExpanded = false;
                BuildTree(itemSanPham, dtTreeSanPham, "SAN_PHAM");

                itemDonVi.Items.Clear();
                //Cấu trúc của Tag: GiaTri#Level#LoaiTree ( VD: MaSP001#2#SAN_PHAM)
                itemDonVi.Tag = "0#0#DON_VI";
                itemDonVi.IsExpanded = false;
                //BuildTree(itemDonVi, dtTreeDonVi, "DON_VI");
                BuildSubTreeKhuVuc(itemDonVi,null,0);

                //Cấu trúc của Tag: GiaTri#Level#LoaiTree ( VD: CB001#1#CBQL)
                itemCBQL.Items.Clear();
                itemCBQL.Tag = "0#0#CBQL";
                itemCBQL.IsExpanded = false;
                BuildTree(itemCBQL, dtTreeCBQL,"CBQL");
                
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

                List<DataRow> lstDataRow = null;
                if (sLoaiTree.Equals("DON_VI"))
                {
                    lstDataRow = dt.Select().OrderBy(row => row[3]).ToList();
                    //lstDataRow = dt.Select("LEVEL<4").OrderBy(row => row[3]).ToList();
                }
                else
                {
                    lstDataRow = dt.Select().ToList();
                }

                foreach (DataRow row in lstDataRow)
                //foreach (DataRow row in dt.Rows)
                {
                    if (iLevel == Convert.ToInt32(row["LEVEL"]) - 1)
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

        void BuildSubTreeKhuVuc(RadTreeViewItem Item, DataRow dr, int iLevel)
        {
            List<DataRow> lstDataRow = null;
            if (dr != null)
                lstDataRow = dtTreeDonVi.Select("NODE_PARENT='" + dr["NODE"] + "' AND LEVEL=" + iLevel).OrderBy(row => row[3]).ToList();
            //lstDataRow = dtTreeDLy.Select("MA_DVI_CHA='" + dr["MA_DVI"] + "' AND LEVEL=" + iLevel).ToList();
            else
                lstDataRow = dtTreeDonVi.Select("NODE_PARENT=0").OrderBy(row => row[3]).ToList();
            //lstDataRow = dtTreeDLy.Select("MA_DVI_CHA=''").ToList();
            foreach (DataRow row in lstDataRow)
            {
                RadTreeViewItem subItem = new RadTreeViewItem();
                subItem.Header = row["NODE_NAME"].ToString();
                //subItem.Tag = row["NODE"].ToString();
                subItem.Tag = row["NODE"].ToString() + "#" + row["LEVEL"].ToString() + "#" + "DON_VI";
                //subItem.IsExpanded = true;
                //subItem.IsChecked = true;
                if (row["LEVEL"].Equals(0))
                    itemDonVi.Items.Add(subItem);
                else
                    Item.Items.Add(subItem);
                BuildSubTreeKhuVuc(subItem, row, Convert.ToInt32(row["LEVEL"]) + 1);
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
                HuyDongVonProcess huyDongVonProcess = new HuyDongVonProcess();

                #region Điều kiện tìm kiếm        
                string sMaDonVi = "";
                //if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()) || ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()))
                //{
                //    sMaDonVi = ClientInformation.MaDonViGiaoDich;
                //}
                //else
                //{
                //    sMaDonVi = lstSourceDonVi.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.ElementAt(0);
                //    if (sMaDonVi.Equals(DatabaseConstant.MA_HSO)) sMaDonVi = "%";
                //}

                sMaDonVi = "(";
                string maChon = lstSourceDonVi.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.ElementAt(0);
                string donVi = "";
                foreach (AutoCompleteEntry item in lstSourceDonVi)
                {
                    donVi = item.KeywordStrings.ElementAt(0);
                    if(donVi.Contains(maChon))
                    {
                        sMaDonVi += "''" + donVi + "'',";
                    }
                }
                sMaDonVi = sMaDonVi.Substring(0, sMaDonVi.Length - 1);
                sMaDonVi += ")";
                
                string sTrangThaiNVu = "%";
                string sSoTGui = "%";
                string sTenSoTGui = "%";
                string sTuNgayMoSo = "%";
                string sDenNgayMoSo = "%";
                string sTuNgayDHan = "%";
                string sDenNgayDHan = "%";
                string soDuTu = "%";
                string soDuDen = "%";
                string iKyHanTu = "%";
                string iKyHanDen = "%";
                string sKyHanDonVi = "%";
                string sMaKH = "%";
                string sTenKH = "%";
                string sSoCMND = "%";
                string sSDT = "%";
                string sEmail = "%";
                string sNgayDC = "%";
                //if (expTimKiemNangCao.IsExpanded == true)
                //{
                    //raddtNgayDaoHanTu.Value = null;
                    if (!ucTrangThaiNVu.GetItemsSelected().Equals("NULL"))
                        sTrangThaiNVu = ucTrangThaiNVu.GetItemsSelected();

                    if (!txtSoSoTG.Text.IsNullOrEmptyOrSpace())
                        sSoTGui = txtSoSoTG.Text;

                    if (!raddtNgayMoSoTu.Value.IsNullOrEmpty())
                        sTuNgayMoSo = Convert.ToDateTime(raddtNgayMoSoTu.Value).ToString("yyyyMMdd");

                    if (!raddtNgayMoSoDen.Value.IsNullOrEmpty())
                        sDenNgayMoSo = Convert.ToDateTime(raddtNgayMoSoDen.Value).ToString("yyyyMMdd");

                    if (!raddtNgayDaoHanTu.Value.IsNullOrEmpty())
                        sTuNgayDHan = Convert.ToDateTime(raddtNgayDaoHanTu.Value).ToString("yyyyMMdd");

                    if (!raddtNgayDaoHanDen.Value.IsNullOrEmpty())
                        sDenNgayDHan = Convert.ToDateTime(raddtNgayDaoHanDen.Value).ToString("yyyyMMdd");

                    if (numSoDuTu.Value != null)
                        soDuTu = numSoDuTu.Value.ToString();

                    if (numSoDuDen.Value != null)
                        soDuDen = numSoDuDen.Value.ToString();

                    if (numKyHanTu.Value != null)
                        iKyHanTu = numKyHanTu.Value.ToString();

                    if (numKyHanDen.Value != null)
                        iKyHanDen = numKyHanDen.Value.ToString();

                    if(cmbKyHanTu.SelectedIndex >= 0)
                        sKyHanDonVi = lstSourceKyHan.ElementAt(cmbKyHanTu.SelectedIndex).KeywordStrings.ElementAt(0);

                    if (!txtKhachHang.Text.IsNullOrEmptyOrSpace())
                        sMaKH = txtKhachHang.Text;

                    if (!txtTenKhachHang.Text.IsNullOrEmptyOrSpace())
                        sTenKH = txtTenKhachHang.Text;                    
                //}

                #endregion
                // Phân trang
                int StartRow = 1;
                int EndRow = ClientInformation.SoLuongBanGhi;
                int CurrentPagging = 1;
                int PaggingSize = ClientInformation.SoLuongBanGhi;

                DataSet ds = huyDongVonProcess.GetDanhSachSoTGuiNhom(sMaDonVi, sSanPham, sIDNhom, sCBQL, sTrangThaiNVu, sSoTGui,
                                         sTenSoTGui, sTuNgayMoSo, sDenNgayMoSo, sTuNgayDHan, sDenNgayDHan,
                                          soDuTu, soDuDen, iKyHanTu, iKyHanDen, sKyHanDonVi,
                                         sMaKH, sTenKH, sSoCMND, sSDT, sEmail, sNgayDC, StartRow.ToString(), EndRow.ToString());
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr["TTHAI_NVU"] = BusinessConstant.layNgonNguNghiepVu(dr["TTHAI_NVU"].ToString());
                        if (!dr["KY_HAN_DVI_TINH"].IsNullOrEmpty() && !dr["KY_HAN_TGIAN"].IsNullOrEmpty() && !dr["KY_HAN"].IsNullOrEmpty())
                        {
                            if (!dr["KY_HAN_TGIAN"].ToString().Equals("0") && !dr["KY_HAN_DVI_TINH"].ToString().Equals(""))
                                dr["KY_HAN"] = dr["KY_HAN_TGIAN"] + " " + BusinessConstant.layNgonNguTuGiaTri(dr["KY_HAN_DVI_TINH"].ToString(), "TAN_SUAT");
                        }
                    }

                    DataTable serverDataTable = ds.Tables[0];
                    int totalRecord = Int32.Parse(ds.Tables[1].Rows[0][0].ToString());
                    DataTable clientDataTable = CommonFunction.BuildClientDataTable(serverDataTable, CurrentPagging, PaggingSize, totalRecord);

                    //dtSoTGui = ds.Tables[0];
                    //grSoTienGuiDS.DataContext = dtSoTGui.DefaultView;
                    dtSoTGui = serverDataTable;
                    grSoTienGuiDS.DataContext = clientDataTable.DefaultView;

                    int soSoTgui = 0;
                    decimal tongSoDu = 0;
                    decimal soDuBinhQuan = 0;
                    decimal tongSoDuGoc = Decimal.Parse(ds.Tables[2].Rows[0][0].ToString());
                    decimal tongSoDuLai = Decimal.Parse(ds.Tables[2].Rows[0][1].ToString());

                    if (totalRecord > 0)
                    {
                        soSoTgui = totalRecord;
                        for (int i = 0; i < dtSoTGui.Rows.Count; i++)
                        {
                            tongSoDu += Convert.ToDecimal(dtSoTGui.Rows[i]["SO_DU"]);
                        }
                        soDuBinhQuan = tongSoDu / soSoTgui;
                    }

                    lblSumSoSo.Content = String.Format("{0:#,#}", soSoTgui);
                    lblSumSoDu.Content = String.Format("{0:#,#}", tongSoDu);
                    lblSoDuBQ.Content = String.Format("{0:#,#}", soDuBinhQuan);
                    lblSumSoDuGoc.Content = String.Format("{0:#,#}", tongSoDuGoc);
                    lblSumSoDuLai.Content = String.Format("{0:#,#}", tongSoDuLai);
                }
                else
                {
                    dtSoTGui.Rows.Clear();
                    grSoTienGuiDS.DataContext = dtSoTGui.DefaultView;
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
            //CommonFunction.GoToPosition(currentID, ref grSoTienGuiDS, radpage, nudPageSize);
            //grSoTienGuiDS.Items.MoveToPage(1);
            //grSoTienGuiDS.Items.MoveCurrentToPosition(1);
        }

        /// <summary>
        /// Sự kiện tìm kiếm nhanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTimKiemNhanh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grSoTienGuiDS, txtTimKiemNhanh.Text);
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
            if (grSoTienGuiDS != null && grSoTienGuiDS.DataContext != null)
            {                
                if (dtSoTGui != null)
                {
                    radpage.PageSize = (int)nudPageSize.Value;
                    grSoTienGuiDS.DataContext = dtSoTGui.DefaultView;
                }
            }
        }

        private void grSoTienGuiDS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BeforeView();
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(grSoTienGuiDS);            
        }

        private void cmbKyHanTu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                cmbKyHanDen.SelectedIndex = cmbKyHanTu.SelectedIndex;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void cmbKyHanDen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                cmbKyHanTu.SelectedIndex = cmbKyHanDen.SelectedIndex;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void cmbDonVi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadTreeview();
            grSoTienGuiDS.DataContext = null;
            lblSumSoSo.Content = 0;
            lblSumSoDu.Content = 0;
            lblSoDuBQ.Content = 0;
            lblSumSoDuGoc.Content = 0;
            lblSumSoDuLai.Content = 0;
        }

        private void radpage_PageIndexChanging(object sender, PageIndexChangingEventArgs e)
        {
            if (e.NewPageIndex < radpage.PageCount)
            {
                CurrentPagging = e.NewPageIndex + 1;
                StartRow = (CurrentPagging - 1) * PaggingSize + 1;
                EndRow = StartRow + PaggingSize - 1;
                //radpage = new RadDataPager();
                TimKiemPhanTrang();
            }
        }

        /// <summary>
        /// Xử lý tìm kiếm dữ liệu
        /// </summary>
        private void TimKiemPhanTrang()
        {
            LayLaiPhanTrang();
        }


        /// <summary>
        /// Lấy lại dữ liệu
        /// </summary>
        private void LayLaiPhanTrang()
        {
            sSanPham = "";
            sIDCum = "";
            sIDNhom = "";
            sCBQL = "";
            foreach (RadTreeViewItem item in tvwTree.CheckedItems)
            {

                ///Cấu trúc của Tag: GiaTri#Level#LoaiTree  ( VD:  MaSP001#2#SAN_PHAM hoặc CUM001#3#DON_VI)
                string sTag = item.Tag.ToString();
                int i1 = sTag.IndexOf("#");
                int i2 = sTag.LastIndexOf("#");

                string sValue = sTag.Substring(0, i1);
                int iLevel = Convert.ToInt32(sTag.Substring(i1 + 1, i2 - i1 - 1));
                string sLoaiTree = sTag.Substring(i2 + 1);

                if (sLoaiTree.Equals("SAN_PHAM"))
                {
                    if (iLevel == 2)
                    {
                        sSanPham = sSanPham + "''" + sValue + "'',";
                    }
                }

                if (sLoaiTree.Equals("DON_VI"))
                {
                    if (iLevel == 4)
                    {
                        sIDNhom = sIDNhom + "''" + sValue + "'',";
                    }
                }

                if (sLoaiTree.Equals("CBQL"))
                {
                    if (iLevel == 1)
                    {
                        sCBQL = sCBQL + "''" + sValue + "'',";
                    }
                }

            }

            if (sSanPham.Length > 0)
            {
                sSanPham = sSanPham.Substring(0, sSanPham.Length - 1);
            }
            if (sIDNhom.Length > 0)
            {
                sIDNhom = sIDNhom.Substring(0, sIDNhom.Length - 1);
            }
            if (sCBQL.Length > 0)
            {
                sCBQL = sCBQL.Substring(0, sCBQL.Length - 1);
            }


            if (itemSanPham.CheckState != System.Windows.Automation.ToggleState.Indeterminate)
            {
                sSanPham = "%";
            }
            if (itemDonVi.CheckState != System.Windows.Automation.ToggleState.Indeterminate)
            {
                sIDNhom = "%";
            }
            if (itemCBQL.CheckState != System.Windows.Automation.ToggleState.Indeterminate)
            {
                sCBQL = "%";
            }

            if (sSanPham.Equals(""))
            {
                sSanPham = "''''";
            }
            if (sIDNhom.Equals(""))
            {
                sIDNhom = "''''";
            }
            if (sCBQL.Equals(""))
            {
                sCBQL = "''''";
            }

            LoadGridPhanTrang();
        }

        /// <summary>
        /// Load dữ liệu lên Grid
        /// </summary>
        private void LoadGridPhanTrang()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HuyDongVonProcess huyDongVonProcess = new HuyDongVonProcess();

                #region Điều kiện tìm kiếm
                string sMaDonVi = "";
                //if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()) || ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()))
                //{
                //    sMaDonVi = ClientInformation.MaDonViGiaoDich;
                //}
                //else
                //{
                //    sMaDonVi = lstSourceDonVi.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.ElementAt(0);
                //    if (sMaDonVi.Equals(DatabaseConstant.MA_HSO)) sMaDonVi = "%";
                //}

                sMaDonVi = "(";
                string maChon = lstSourceDonVi.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.ElementAt(0);
                string donVi = "";
                foreach (AutoCompleteEntry item in lstSourceDonVi)
                {
                    donVi = item.KeywordStrings.ElementAt(0);
                    if (donVi.Contains(maChon))
                    {
                        sMaDonVi += "''" + donVi + "'',";
                    }
                }
                sMaDonVi = sMaDonVi.Substring(0, sMaDonVi.Length - 1);
                sMaDonVi += ")";

                string sTrangThaiNVu = "%";
                string sSoTGui = "%";
                string sTenSoTGui = "%";
                string sTuNgayMoSo = "%";
                string sDenNgayMoSo = "%";
                string sTuNgayDHan = "%";
                string sDenNgayDHan = "%";
                string soDuTu = "%";
                string soDuDen = "%";
                string iKyHanTu = "%";
                string iKyHanDen = "%";
                string sKyHanDonVi = "%";
                string sMaKH = "%";
                string sTenKH = "%";
                string sSoCMND = "%";
                string sSDT = "%";
                string sEmail = "%";
                string sNgayDC = "%";
                //if (expTimKiemNangCao.IsExpanded == true)
                //{
                    //raddtNgayDaoHanTu.Value = null;
                    if (!ucTrangThaiNVu.GetItemsSelected().Equals("NULL"))
                        sTrangThaiNVu = ucTrangThaiNVu.GetItemsSelected();

                    if (!txtSoSoTG.Text.IsNullOrEmptyOrSpace())
                        sSoTGui = txtSoSoTG.Text;
            
                    if (!raddtNgayMoSoTu.Value.IsNullOrEmpty())
                        sTuNgayMoSo = Convert.ToDateTime(raddtNgayMoSoTu.Value).ToString("yyyyMMdd");

                    if (!raddtNgayMoSoDen.Value.IsNullOrEmpty())
                        sDenNgayMoSo = Convert.ToDateTime(raddtNgayMoSoDen.Value).ToString("yyyyMMdd");

                    if (!raddtNgayDaoHanTu.Value.IsNullOrEmpty())
                        sTuNgayDHan = Convert.ToDateTime(raddtNgayDaoHanTu.Value).ToString("yyyyMMdd");

                    if (!raddtNgayDaoHanDen.Value.IsNullOrEmpty())
                        sDenNgayDHan = Convert.ToDateTime(raddtNgayDaoHanDen.Value).ToString("yyyyMMdd");

                    if (numSoDuTu.Value != null)
                        soDuTu = numSoDuTu.Value.ToString();

                    if (numSoDuDen.Value != null)
                        soDuDen = numSoDuDen.Value.ToString();

                    if (numKyHanTu.Value != null)
                        iKyHanTu = numKyHanTu.Value.ToString();

                    if (numKyHanDen.Value != null)
                        iKyHanDen = numKyHanDen.Value.ToString();

                    if (cmbKyHanTu.SelectedIndex >= 0)
                        sKyHanDonVi = lstSourceKyHan.ElementAt(cmbKyHanTu.SelectedIndex).KeywordStrings.ElementAt(0);

                    if (!txtKhachHang.Text.IsNullOrEmptyOrSpace())
                        sMaKH = txtKhachHang.Text;

                    if (!txtTenKhachHang.Text.IsNullOrEmptyOrSpace())
                        sTenKH = txtTenKhachHang.Text;

                //}

                #endregion
                    DataSet ds = huyDongVonProcess.GetDanhSachSoTGuiNhom(sMaDonVi, sSanPham, sIDNhom, sCBQL, sTrangThaiNVu, sSoTGui,
                                         sTenSoTGui, sTuNgayMoSo, sDenNgayMoSo, sTuNgayDHan, sDenNgayDHan,
                                          soDuTu, soDuDen, iKyHanTu, iKyHanDen, sKyHanDonVi,
                                         sMaKH, sTenKH, sSoCMND, sSDT, sEmail, sNgayDC, StartRow.ToString(), EndRow.ToString());
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr["TTHAI_NVU"] = BusinessConstant.layNgonNguNghiepVu(dr["TTHAI_NVU"].ToString());
                        if (!dr["KY_HAN_DVI_TINH"].IsNullOrEmpty() && !dr["KY_HAN_TGIAN"].IsNullOrEmpty() && !dr["KY_HAN"].IsNullOrEmpty())
                        {
                            if (!dr["KY_HAN_TGIAN"].ToString().Equals("0") && !dr["KY_HAN_DVI_TINH"].ToString().Equals(""))
                                dr["KY_HAN"] = dr["KY_HAN_TGIAN"] + " " + BusinessConstant.layNgonNguTuGiaTri(dr["KY_HAN_DVI_TINH"].ToString(), "TAN_SUAT");
                        }
                    }

                    DataTable serverDataTable = ds.Tables[0];
                    int totalRecord = Int32.Parse(ds.Tables[1].Rows[0][0].ToString());
                    DataTable clientDataTable = CommonFunction.BuildClientDataTable(serverDataTable, CurrentPagging, PaggingSize, totalRecord);

                    //dtSoTGui = ds.Tables[0];
                    //grSoTienGuiDS.DataContext = dtSoTGui.DefaultView;
                    dtSoTGui = serverDataTable;
                    grSoTienGuiDS.DataContext = clientDataTable.DefaultView;

                    int soSoTgui = 0;
                    decimal tongSoDu = 0;
                    decimal soDuBinhQuan = 0;
                    if (totalRecord > 0)
                    {
                        soSoTgui = totalRecord;
                        for (int i = 0; i < dtSoTGui.Rows.Count; i++)
                        {
                            tongSoDu += Convert.ToDecimal(dtSoTGui.Rows[i]["SO_DU"]);
                        }
                        soDuBinhQuan = tongSoDu / soSoTgui;
                    }

                    lblSumSoSo.Content = String.Format("{0:#,#}", soSoTgui);
                    lblSumSoDu.Content = String.Format("{0:#,#}", tongSoDu);
                    lblSoDuBQ.Content = String.Format("{0:#,#}", soDuBinhQuan);
                }
                else
                {
                    dtSoTGui.Rows.Clear();
                    grSoTienGuiDS.DataContext = dtSoTGui.DefaultView;
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
                        currentPage = grSoTienGuiDS.Items.PageIndex;
                        currentPosition = grSoTienGuiDS.Items.CurrentPosition;
                        currentID = int.Parse(listDataRow.First()["ID"].ToString());
                        int idSanPham = Convert.ToInt32(listDataRow.First()["ID_SAN_PHAM"]);

                        // Nếu không cho phép sửa sau duyệt
                        if (listDataRow.First()["tthai_nvu"].ToString().Equals(BusinessConstant.layNgonNguNghiepVu(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri())))
                        {
                            LMessage.ShowMessage("M.DungChung.DaDuyetKhongDuocSua", LMessage.MessageBoxType.Warning);
                            return;
                        }

                        OnModify(currentID, idSanPham);
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

                            bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                                DatabaseConstant.Function.HDV_DANH_SACH_SO,
                                DatabaseConstant.Table.BL_TIEN_GUI,
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

                            bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                                DatabaseConstant.Function.HDV_DANH_SACH_SO,
                                DatabaseConstant.Table.BL_TIEN_GUI,
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

                            bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                                DatabaseConstant.Function.HDV_DANH_SACH_SO,
                                DatabaseConstant.Table.BL_TIEN_GUI,
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

                            bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                                DatabaseConstant.Function.HDV_DANH_SACH_SO,
                                DatabaseConstant.Table.BL_TIEN_GUI,
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
                List<DataRowView> listDataRow = getListSeletedDataRow();                
                int idSanPham;

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
                        currentPage = grSoTienGuiDS.Items.PageIndex;
                        currentPosition = grSoTienGuiDS.Items.CurrentPosition;
                        currentID = int.Parse(listDataRow.First()["ID"].ToString());
                        idSanPham = int.Parse(listDataRow.First()["ID_SAN_PHAM"].ToString());

                        OnView(currentID, idSanPham);
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
            ucTienGuiCoKyHanCT userControl  = new ucTienGuiCoKyHanCT();

            userControl.Function = DatabaseConstant.Function.HDV_SO_TKQD;
            userControl.Action = DatabaseConstant.Action.THEM;
            userControl.Flag = 1;
            //uc.OnSavingCompleted += new EventHandler(userControl_OnSavingCompleted);

            frm.Title = DatabaseConstant.layNgonNguTieuDeForm(userControl.Function);
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frm.Content = userControl;
            frm.ShowDialog();
        }

        private void OnModify(int id, int idSanPham)
        {
            try
            {
                ucTienGuiCoKyHanCT userControl = new ucTienGuiCoKyHanCT();                
                #region Tìm chức năng theo sổ: Dựa vào sản phẩm -> Nhóm sản phẩm -> Chức năng tương ứng
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                DataSet ds = processHDV.GetSanPhamByID(idSanPham);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    string sNhomSP = ds.Tables[0].Rows[0]["MA_NHOM_SP"].ToString();
                    if (sNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T01.layGiaTri()))
                    {
                        userControl.Function = DatabaseConstant.Function.HDV_SO_TKQD;
                    }
                    else if (sNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T02.layGiaTri()))
                    {
                        userControl.Function = DatabaseConstant.Function.HDV_SO_TKKKH;
                    }
                    else if (sNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T03.layGiaTri()) || sNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T04.layGiaTri()) || sNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T05.layGiaTri()) || sNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T06.layGiaTri()))
                    {
                        userControl.Function = DatabaseConstant.Function.HDV_SO_TKCKH;
                    }
                    else if (sNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T07.layGiaTri()))
                    {
                        userControl.Function = DatabaseConstant.Function.HDV_SO_TGCKH;
                    }
                    else if (sNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T08.layGiaTri()))
                    {
                        userControl.Function = DatabaseConstant.Function.HDV_SO_TK_TGTT;
                    }

                }
                #endregion

                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);

                bool ret = process.LockData(DatabaseConstant.Module.HDVO,
                    userControl.Function,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.SUA,
                    listLockId);
                if (ret)
                {
                    userControl.OnSavingCompleted += new EventHandler(userControl_OnSavingCompleted);
                    userControl.Action = DatabaseConstant.Action.SUA;
                    userControl.ID = id;
                    userControl.Flag = 1;

                    Window frm = new Window();
                    frm.Title = DatabaseConstant.layNgonNguTieuDeForm(userControl.Function);
                    frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    frm.Content = userControl;
                    frm.ShowDialog();
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
            HuyDongVonProcess processHDV = new HuyDongVonProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = processHDV.DanhSachSoTGui(DatabaseConstant.Action.XOA, listId, ref listClientResponseDetail);

                AfterDelete(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_DANH_SACH_SO,
                    DatabaseConstant.Table.BL_TIEN_GUI,
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
            HuyDongVonProcess processHDV = new HuyDongVonProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = processHDV.DanhSachSoTGui(DatabaseConstant.Action.DUYET, listId, ref listClientResponseDetail);

                AfterApprove(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_DANH_SACH_SO,
                    DatabaseConstant.Table.BL_TIEN_GUI,
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
            HuyDongVonProcess processHDV = new HuyDongVonProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = processHDV.DanhSachSoTGui(DatabaseConstant.Action.THOAI_DUYET, listId, ref listClientResponseDetail);

                AfterCancel(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_DANH_SACH_SO,
                    DatabaseConstant.Table.BL_TIEN_GUI,
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
            HuyDongVonProcess processHDV = new HuyDongVonProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = processHDV.DanhSachSoTGui(DatabaseConstant.Action.TU_CHOI_DUYET, listId, ref listClientResponseDetail);

                AfterRefuse(ret, listId, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_DANH_SACH_SO,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listId);
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void OnView(int id, int idSanPham)
        {
            try
            {
                ucTienGuiCoKyHanCT userControl = new ucTienGuiCoKyHanCT();   
             
                #region Tìm chức năng theo sổ: Dựa vào sản phẩm -> Nhóm sản phẩm -> Chức năng tương ứng
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                DataSet ds = processHDV.GetSanPhamByID(idSanPham);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    string sNhomSP = ds.Tables[0].Rows[0]["MA_NHOM_SP"].ToString();
                    if (sNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T01.layGiaTri()))
                    {
                        userControl.Function = DatabaseConstant.Function.HDV_SO_TKQD;
                    }
                    else if (sNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T02.layGiaTri()))
                    {
                        userControl.Function = DatabaseConstant.Function.HDV_SO_TKKKH;
                    }
                    else if (sNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T03.layGiaTri()) || sNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T04.layGiaTri()) || sNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T05.layGiaTri()) || sNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T06.layGiaTri()))
                    {
                        userControl.Function = DatabaseConstant.Function.HDV_SO_TKCKH;
                    }
                    else if (sNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T07.layGiaTri()))
                    {
                        userControl.Function = DatabaseConstant.Function.HDV_SO_TGCKH;
                    }
                    else if (sNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T08.layGiaTri()))
                    {
                        userControl.Function = DatabaseConstant.Function.HDV_SO_TK_TGTT;                        
                    }

                }
                #endregion
                
                userControl.OnSavingCompleted += new EventHandler(userControl_OnSavingCompleted);
                userControl.Action = DatabaseConstant.Action.XEM;
                userControl.ID = id;
                userControl.Flag = 1;
                
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

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                DatabaseConstant.Function.HDV_DANH_SACH_SO,
                DatabaseConstant.Table.BL_TIEN_GUI,
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

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                DatabaseConstant.Function.HDV_DANH_SACH_SO,
                DatabaseConstant.Table.BL_TIEN_GUI,
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_DANH_SACH_SO,
                    DatabaseConstant.Table.BL_TIEN_GUI,
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_DANH_SACH_SO,
                    DatabaseConstant.Table.BL_TIEN_GUI,
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
            sSanPham = "";
            sIDCum = "";
            sIDNhom = "";
            sCBQL = "";
            foreach (RadTreeViewItem item in tvwTree.CheckedItems)
            {

                ///Cấu trúc của Tag: GiaTri#Level#LoaiTree  ( VD:  MaSP001#2#SAN_PHAM hoặc CUM001#3#DON_VI)
                string sTag = item.Tag.ToString();
                int i1 = sTag.IndexOf("#");
                int i2 = sTag.LastIndexOf("#");

                string sValue = sTag.Substring(0, i1);
                int iLevel = Convert.ToInt32(sTag.Substring(i1 + 1, i2 - i1 - 1));
                string sLoaiTree = sTag.Substring(i2 + 1);

                if (sLoaiTree.Equals("SAN_PHAM"))
                {
                    if (iLevel == 2)
                    {
                        sSanPham = sSanPham + "''" + sValue + "'',";
                    }
                }

                if (sLoaiTree.Equals("DON_VI"))
                {
                    if (iLevel == 4)
                    {
                        sIDNhom = sIDNhom + "''" + sValue + "'',";
                    }
                }

                if (sLoaiTree.Equals("CBQL"))
                {
                    if (iLevel == 1)
                    {
                        sCBQL = sCBQL + "''" + sValue + "'',";
                    }
                }

            }

            if (sSanPham.Length > 0)
            {
                sSanPham = sSanPham.Substring(0, sSanPham.Length - 1);
            }
            if (sIDNhom.Length > 0)
            {
                sIDNhom = sIDNhom.Substring(0, sIDNhom.Length - 1);
            }
            if (sCBQL.Length > 0)
            {
                sCBQL = sCBQL.Substring(0, sCBQL.Length - 1);
            }


            if (itemSanPham.CheckState != System.Windows.Automation.ToggleState.Indeterminate)
            {
                sSanPham = "%";
            }
            if (itemDonVi.CheckState != System.Windows.Automation.ToggleState.Indeterminate)
            {
                sIDNhom = "%";
            }
            if (itemCBQL.CheckState != System.Windows.Automation.ToggleState.Indeterminate)
            {
                sCBQL = "%";
            }

            if (sSanPham.Equals(""))
            {
                sSanPham = "''''";
            }
            if (sIDNhom.Equals(""))
            {
                sIDNhom = "''''";
            }
            if (sCBQL.Equals(""))
            {
                sCBQL = "''''";
            }

            LoadGrid();
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

                if (grSoTienGuiDS.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < grSoTienGuiDS.SelectedItems.Count; i++)
                    {
                        DataRowView dr = (DataRowView)grSoTienGuiDS.SelectedItems[i];
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
