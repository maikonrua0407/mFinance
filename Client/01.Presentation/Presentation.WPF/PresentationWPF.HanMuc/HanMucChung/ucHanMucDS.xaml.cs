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
using Utilities.Common;
using PresentationWPF.CustomControl;
using Telerik.Windows.Controls;
using Presentation.Process.TruyVanServiceRef;
using Presentation.Process.Common;
using Presentation.Process.DanhMucServiceRef;
using System.Reflection;
using Presentation.Process.HanMucServiceRef;

namespace PresentationWPF.HanMuc.HanMucChung
{
    /// <summary>
    /// Interaction logic for ucHanMucDS.xaml
    /// </summary>
    public partial class ucHanMucDS : UserControl
    {
        #region Khai bao        
        private int currentPosition;
        private int currentPage;
        private int currentID;

        DataTable dtTreeDonVi = new DataTable();

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
        #endregion

        #region Khoi tao

        public ucHanMucDS()
        {
            InitializeComponent();
            InitEventHandler();            
            BindHotkey();
            radPage.PageSize = (int)nudPageSize.Value;
            LoadDuLieu(null, null);
            txtTimKiemNhanh.Focus();
            TimKiem();
        }

        /// <summary>
        /// InitEventHandler
        /// </summary>
        private void InitEventHandler()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.DanhMuc;component/HanMucChung/ucHanMucDS.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }

            txtTimKiemNhanh.GotFocus += txtTimKiemNhanh_GotFocus;
            txtTimKiemNhanh.LostFocus += txtTimKiemNhanh_LostFocus;
            txtTimKiemNhanh.TextChanged += txtTimKiemNhanh_TextChanged;
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
            //BeforeApprove();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //BeforeRefuse();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //BeforeCancel();
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
                //BeforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                //BeforeRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                //BeforeCancel();
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
                //BeforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                //BeforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                //BeforeRefuse();
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
            txtTimKiemNhanh.Focus();
        }

        private void LoadTreeview()
        {
            try
            {
                DanhMucProcess DanhMucProcess = new DanhMucProcess();
                dtTreeDonVi = DanhMucProcess.GetTreeDonVi(ClientInformation.TenDangNhap, ClientInformation.MaDonViQuanLy).Tables[0];

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

                List<DataRow> lstDataRow = null;
                lstDataRow = dt.Select().OrderBy(row => row[2]).ToList();

                foreach (DataRow row in lstDataRow)
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

        private void LoadGrid()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                #region Lấy điều kiện truyền vào
                string maDonVi = ClientInformation.MaDonVi;

                //foreach (RadTreeViewItem item in tvwTree.CheckedItems)
                //{
                //    int i = item.Tag.ToString().IndexOf("#");
                //    string sValue = item.Tag.ToString().Substring(0, i);
                //    int iLevel = Convert.ToInt32(item.Tag.ToString().Substring(i + 1));

                //    if (iLevel == 3)
                //    {
                //        maDonVi = maDonVi + "''" + sValue + "'',";
                //    }
                //}
                //if (maDonVi.Length > 0)
                //{
                //    maDonVi = maDonVi.Substring(0, maDonVi.Length - 1);
                //    maDonVi = "(" + maDonVi + ")";
                //}
                //else
                //{
                //    maDonVi = "('''')";
                //}

                #endregion

                DataSet ds = new HanMucProcess().GetDanhSachHanMucChung(maDonVi);                
                if (ds != null)
                {
                    grHanMucDS.DataContext = ds.Tables[0].DefaultView;
                }
                else
                {
                    grHanMucDS.Items.Clear();
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
        /// Load dữ liệu lên Form
        /// </summary>
        private void LoadDuLieu(object sender, EventArgs e)
        {
            try
            {                
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
            LoadGrid();
            CommonFunction.GoToPosition(currentID, ref grHanMucDS, radPage, nudPageSize);

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
            PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grHanMucDS, txtTimKiemNhanh.Text);
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

        private void grHanMucDS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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
            if (grHanMucDS != null && grHanMucDS.DataContext != null)
            {
                DataTable dt = ((DataView)grHanMucDS.DataContext).Table;
                if (dt != null)
                {
                    radPage.PageSize = (int)nudPageSize.Value;
                    grHanMucDS.DataContext = dt.DefaultView;
                }
            }
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(grHanMucDS);
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
            ucHanMucCT userControl = new ucHanMucCT();            

            userControl._action = DatabaseConstant.Action.THEM;
            userControl.OnSavingCompleted += new EventHandler(userControl_OnSavingCompleted);

            window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.DC_HAN_MUC);
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
                        currentPage = grHanMucDS.Items.PageIndex;
                        currentPosition = grHanMucDS.Items.CurrentPosition;
                        //currentID = int.Parse(listDataRow.First()["ID"].ToString());

                        OnModify(listDataRow.First()["LOAI_DOI_TUONG"].ToString(), listDataRow.First()["MA_DOI_TUONG"].ToString());
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
        private void OnModify(string maLoaiDT, string maDT)
        {
            try
            {
                ucHanMucCT userControl = new ucHanMucCT();
                userControl.OnSavingCompleted += new EventHandler(userControl_OnSavingCompleted);
                userControl._action = DatabaseConstant.Action.SUA;

                List<DC_HAN_MUC> lstObjHM = new List<DC_HAN_MUC>();
                DC_HAN_MUC objHM = new DC_HAN_MUC();
                objHM.MA_DTUONG_LOAI = maLoaiDT;
                objHM.MA_DTUONG = maDT;

                lstObjHM.Add(objHM);

                userControl._lstObjHM = lstObjHM;

                Window window = new Window();
                window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.DC_HAN_MUC);
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
                        currentPage = grHanMucDS.Items.PageIndex;
                        currentPosition = grHanMucDS.Items.CurrentPosition;
                        //currentID = int.Parse(listDataRow.First()["ID"].ToString());
                        OnView(listDataRow.First()["LOAI_DOI_TUONG"].ToString(), listDataRow.First()["MA_DOI_TUONG"].ToString());
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
        private void OnView(string maLoaiDT, string maDT)
        {
            try
            {
                ucHanMucCT userControl = new ucHanMucCT();

                userControl.OnSavingCompleted += new EventHandler(userControl_OnSavingCompleted);
                userControl._action = DatabaseConstant.Action.XEM;

                List<DC_HAN_MUC> lstObjHM = new List<DC_HAN_MUC>();
                DC_HAN_MUC objHM = new DC_HAN_MUC();
                objHM.MA_DTUONG_LOAI = maLoaiDT;
                objHM.MA_DTUONG = maDT;

                lstObjHM.Add(objHM);

                userControl._lstObjHM = lstObjHM;

                Window window = new Window();
                window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.DC_HAN_MUC);
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
                        List<DC_HAN_MUC> lstObjHM = new List<DC_HAN_MUC>();
                        foreach (DataRowView dr in listDataRow)
                        {
                            DC_HAN_MUC objHM = new DC_HAN_MUC();
                            objHM.MA_DTUONG_LOAI = dr["LOAI_DOI_TUONG"].ToString();
                            objHM.MA_DTUONG = dr["MA_DOI_TUONG"].ToString();

                            lstObjHM.Add(objHM);
                        }

                        // Cảnh báo người dùng
                        MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);
                        if (ret == MessageBoxResult.Yes)
                        {                            
                            OnDelete(lstObjHM);                            
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
        private void OnDelete(List<DC_HAN_MUC> lstObjHM)
        {
            DanhMucProcess processDanhMuc = new DanhMucProcess();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();


                bool ret = new HanMucProcess().HanMucChung(DatabaseConstant.Action.XOA, ref lstObjHM, ref listClientResponseDetail);

                AfterDelete(ret, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                             
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
        private void AfterDelete(bool ret, List<ClientResponseDetail> listClientResponseDetail)
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
            
        }
              

        /// <summary>
        /// Tìm kiếm nâng cao
        /// </summary>
        private void TimKiem()
        {
            try
            {
                LoadGrid();
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

                if (grHanMucDS.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < grHanMucDS.SelectedItems.Count; i++)
                    {
                        DataRowView dr = (DataRowView)grHanMucDS.SelectedItems[i];
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
