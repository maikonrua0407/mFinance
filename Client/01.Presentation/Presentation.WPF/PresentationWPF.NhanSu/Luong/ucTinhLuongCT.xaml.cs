using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
using Telerik.Windows.Controls;
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.HuyDongVonServiceRef;
using Presentation.Process.NhanSuServiceRef;
using Presentation.Process.KhachHangServiceRef;
using Presentation.Process.BaoCaoServiceRef;
using PresentationWPF.CustomControl;
using PresentationWPF.NhanSu.PhuCapCongTacVien;
using PresentationWPF.BaoCao.DungChung;

namespace PresentationWPF.NhanSu.Luong
{
    /// <summary>
    /// Interaction logic for ucTinhLuongCT.xaml
    /// </summary>
    public partial class ucTinhLuongCT : UserControl
    {
        #region Khai bao
        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private DatabaseConstant.Function function = DatabaseConstant.Function.NS_TINH_LUONG_CT;

        public event EventHandler OnSavingCompleted;

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private string maGiaoDich;
        public string MaGiaoDich
        {
            get { return maGiaoDich; }
            set { maGiaoDich = value; }
        }

        private bool isTinhLuong;
        public bool IsTinhLuong
        {
            get { return isTinhLuong; }
            set { isTinhLuong = value; }
        }
       
        private NS_THONG_TIN_TINH_LUONG obj;
        public NS_THONG_TIN_TINH_LUONG Obj
        {
            get { return obj; }
            set { obj = value; }
        }

        private DataSet ds = null;

        private string sTrangThaiNVu = "";

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        
        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand AddCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        #endregion

        #region Khoi tao
        public ucTinhLuongCT()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            LoadCombobox();

            KhoiTaoDataTable();

            InitEventHandler();
        }

        public ucTinhLuongCT(KIEM_SOAT objKiemSoat)
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            LoadCombobox();

            KhoiTaoDataTable();

            InitEventHandler();

            maGiaoDich = objKiemSoat.SO_GIAO_DICH;
            action = objKiemSoat.action;
        }        

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.NhanSu;component/Luong/ucTinhLuongCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void InitEventHandler()
        {
            chkThemNhieuLan.Checked += new RoutedEventHandler(chkThemNhieuLan_Checked);

            btnAdd.Click += new RoutedEventHandler(btnAdd_Click);
            btnDelete.Click += new RoutedEventHandler(btnDelete_Click);
            btnCalculate.Click += new RoutedEventHandler(btnCalculate_Click);
            btnPBo.Click += new RoutedEventHandler(btnPBo_Click);
            grid.CellEditEnded += new EventHandler<GridViewCellEditEndedEventArgs>(grid_CellEditEnded);
        }

        private void KhoiTaoDataTable()
        {
            ds = new DataSet();

            DataTable dtTinhLuong = new DataTable();
            dtTinhLuong.Columns.Add("STT", typeof(int));
            dtTinhLuong.Columns.Add("ID_HSO", typeof(int));
            dtTinhLuong.Columns.Add("MA_HSO", typeof(string));
            dtTinhLuong.Columns.Add("TEN_HSO", typeof(string));
            dtTinhLuong.Columns.Add("ID_CHUC_VU", typeof(int));
            dtTinhLuong.Columns.Add("CHUC_VU", typeof(string));
            dtTinhLuong.Columns.Add("TY_LE_HOAN_THANH", typeof(string));
            dtTinhLuong.Columns.Add("LUONG_CO_BAN", typeof(decimal));
            dtTinhLuong.Columns.Add("NLD_TU_NOP", typeof(decimal));
            dtTinhLuong.Columns.Add("NSDLD_NOP", typeof(decimal));
            dtTinhLuong.Columns.Add("TONG_PHU_CAP", typeof(decimal));
            dtTinhLuong.Columns.Add("TONG_PHU_CAP_BAN_DAU", typeof(decimal));
            dtTinhLuong.Columns.Add("TRU", typeof(decimal));
            dtTinhLuong.Columns.Add("THUONG", typeof(decimal));
            dtTinhLuong.Columns.Add("THUE_TNCN", typeof(decimal));
            dtTinhLuong.Columns.Add("TONG_THUC_NHAN", typeof(decimal));
            dtTinhLuong.Columns.Add("TONG_CHI_PHI", typeof(decimal));
            dtTinhLuong.Columns.Add("GHI_CHU", typeof(string));
            ds.Tables.Add(dtTinhLuong);

            DataTable dtPBo = new DataTable();
            dtPBo.Columns.Add("ID_HSO", typeof(int));
            dtPBo.Columns.Add("ID_DU_AN", typeof(int));
            dtPBo.Columns.Add("TEN_DU_AN", typeof(string));
            dtPBo.Columns.Add("SO_TIEN", typeof(decimal));
            ds.Tables.Add(dtPBo);

            DataTable dtGTri = new DataTable();
            dtGTri.Columns.Add("ID_HSO", typeof(int));
            dtGTri.Columns.Add("FN01", typeof(string));
            dtGTri.Columns.Add("FN02", typeof(string));
            dtGTri.Columns.Add("FN03", typeof(string));
            dtGTri.Columns.Add("FN04", typeof(string));
            dtGTri.Columns.Add("FN05", typeof(string));
            dtGTri.Columns.Add("FN06", typeof(string));
            dtGTri.Columns.Add("FN07", typeof(string));
            dtGTri.Columns.Add("FN08", typeof(string));
            dtGTri.Columns.Add("FN09", typeof(string));
            dtGTri.Columns.Add("FN10", typeof(string));
            ds.Tables.Add(dtGTri);
            
        }

        private void LoadCombobox()
        {            

        }
        #endregion

        #region Dang ky hot key, shortcut key

        private void BindShortkey()
        {
            foreach (var child in Toolbar.Children)
            {
                if (child.GetType() == typeof(RibbonButton))
                {
                    RibbonButton tlb = (RibbonButton)child;
                    KeyBinding key = new KeyBinding();
                    string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
                    {
                        //KeyGesture keyg = new KeyGesture(Key.H, ModifierKeys.Control);
                        //key = new KeyBinding(HoldCommand, keyg);
                        //key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(SaveCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.N, ModifierKeys.Control);
                        key = new KeyBinding(AddCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(ModifyCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.D, ModifierKeys.Control);
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(HelpCommand, keyg);
                    }
                    if (key != null)
                    {
                        InputBindings.Add(key);
                    }
                }
            }
        }

        //private void HoldCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        //{
        //    e.CanExecute = true;
        //}
        //private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        //{
        //    OnHold();
        //}

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnSave();
        }

        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ResetData();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeModifyFromDetail();
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
            OnClose();
        }

        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = "";
            if (sender is RibbonButton)
            {
                RibbonButton tlb = (RibbonButton)sender;
                strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            }
            else if (sender is RibbonMenuItem)
            {
                RibbonMenuItem tlb = (RibbonMenuItem)sender;
                strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            }

            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                OnHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetData();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                BeforeModifyFromDetail();
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
                
            }
            else if (strTinhNang.Equals("PreviewChungTu"))
            {
                OnPreviewChungTu();
            }
            else if (strTinhNang.Equals("PreviewBangLuong"))
            {
                OnPreviewBangLuong();
            }
            else if (strTinhNang.Equals("PreviewTheLuong"))
            {
                OnPreviewTheLuong();
            }
            else if (strTinhNang.Equals("PreviewChuyenKhoan"))
            {
                OnPreviewChuyenKhoan();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                OnHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                OnClose();
            }
        }

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                OnHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetData();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                BeforeModifyFromDetail();
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {                
            }
            else if (strTinhNang.Equals("PreviewChungTu"))
            {
                OnPreviewChungTu();
            }
            else if (strTinhNang.Equals("PreviewBangLuong"))
            {
                OnPreviewBangLuong();
            }
            else if (strTinhNang.Equals("PreviewTheLuong"))
            {
                OnPreviewTheLuong();
            }
            else if (strTinhNang.Equals("PreviewChuyenKhoan"))
            {
                OnPreviewChuyenKhoan();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                OnHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                OnClose();
            }
        }

        #endregion 

        #region Xu ly Giao dien
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Hiển thị Form khi thêm mới dữ liệu
            if (action == DatabaseConstant.Action.THEM)
            {
                BeforeAddNew();
            }

            //Hiển thị Form khi sửa dữ liệu
            else if (action == DatabaseConstant.Action.SUA)
            {
                BeforeModifyFromList();
            }

            //Hiển thị Form khi xem dữ liệu
            else if (action == DatabaseConstant.Action.XEM)
            {
                BeforeViewFromList();
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Release();
            if (OnSavingCompleted != null)
            {
                OnSavingCompleted(this, EventArgs.Empty);
            }
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        /// <summary>
        /// Hàm xử lý giải phóng lock dữ liệu cho form
        /// </summary>
        private void Release()
        {
            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool ret = process.UnlockData(DatabaseConstant.Module.NSTL,
                DatabaseConstant.Function.NS_TINH_LUONG_CT,
                DatabaseConstant.Table.NS_TINH_LUONG,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void OnHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }

        /// <summary>
        /// Đóng form
        /// </summary>
        private void OnClose()
        {
            Release();
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        private void chkThemNhieuLan_Checked(object sender, RoutedEventArgs e)
        {
            //action = DatabaseConstant.Action.THEM;
            //ResetForm();
            //SetEnabledControls();
            //CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
            ////txtTenNhanVien.Focus();

            //chkThemNhieuLan.IsChecked = false;
        }                

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            lstPopup = null;
            var process = new PopupProcess();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
            string thangTinhLuong = Convert.ToDateTime(raddtThang.Value).ToString("yyyyMM");
            lstDieuKien.Add(thangTinhLuong);
            process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_TINHLUONG_HSO.getValue(), lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

            ucPopup popup = new ucPopup(false, simplePopupResponse, false);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.Content = popup;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Title = "Danh sách nhân viên";
            win.ShowDialog();

            if (lstPopup != null)
            {
                isTinhLuong = false;

                foreach (DataRow dr in lstPopup)
                {                    
                    if (KiemTraTonTaiGrid(dr["ID"].ToString()))
                    {

                        DataRow drTinhLuong = ds.Tables[0].NewRow();

                        drTinhLuong["ID_HSO"] = Convert.ToInt32(dr["ID"]);
                        drTinhLuong["MA_HSO"] = dr["MA_HSO"].ToString();
                        drTinhLuong["TEN_HSO"] = dr["TEN_HSO"].ToString();
                        drTinhLuong["ID_CHUC_VU"] = Convert.ToInt32(dr["ID_CHUC_VU"]);
                        drTinhLuong["CHUC_VU"] = "";
                        drTinhLuong["TY_LE_HOAN_THANH"] = 100;
                        drTinhLuong["NLD_TU_NOP"] = 0;
                        drTinhLuong["NSDLD_NOP"] = 0;
                        drTinhLuong["TONG_PHU_CAP"] = 0;
                        drTinhLuong["TRU"] = 0;
                        drTinhLuong["THUONG"] = 0;
                        drTinhLuong["THUE_TNCN"] = 0;
                        drTinhLuong["TONG_THUC_NHAN"] = 0;
                        drTinhLuong["TONG_CHI_PHI"] = 0;
                        drTinhLuong["GHI_CHU"] = "";

                        ds.Tables[0].Rows.Add(drTinhLuong);
                    }
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ds.Tables[0].Rows[i]["STT"] = i + 1;
                }

                grid.DataContext = ds.Tables[0].DefaultView;
            }            
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<int> lstSTT = new List<int>();
                List<int> lstIdHoSo = new List<int>();

                for (int i = 0; i < grid.SelectedItems.Count; i++)
                {
                    DataRowView dr = (DataRowView)grid.SelectedItems[i];
                    lstSTT.Add(Convert.ToInt32(dr["STT"]));
                }
                lstSTT.SortByDesc();
                foreach (int stt in lstSTT)
                    ds.Tables[0].Rows.RemoveAt(stt - 1);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ds.Tables[0].Rows[i]["STT"] = i + 1;
                }                

                grid.DataContext = ds.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            if (ds.Tables[0].Rows.Count <= 0)
            {
                LMessage.ShowMessage("Chưa có dữ liệu tính lương", LMessage.MessageBoxType.Warning);
                return;
            }
            else
                OnTinhToan();
        }

        private void btnPBo_Click(object sender, RoutedEventArgs e)
        {
            if (isTinhLuong == false)
            {
                LMessage.ShowMessage("Chưa thực hiện tính lương", LMessage.MessageBoxType.Warning);
                btnCalculate.Focus();
                return ;
            }

            Mouse.OverrideCursor = Cursors.Wait;
            Window window = new Window();
            ucPhanBoLuong uc = new ucPhanBoLuong(ds);
            uc.DuLieuTraVe = new ucPhanBoLuong.LayDuLieu(LayDuLieuPopup);
            window.Content = uc;
            window.Title = LLanguage.SearchResourceByKey("Phân bổ tiền lương theo dự án");
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Mouse.OverrideCursor = Cursors.Arrow;
            window.ShowDialog();
        }

        private void grid_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Name.Equals("TY_LE_HOAN_THANH"))
                {
                    DataRowView dr = (DataRowView)grid.CurrentItem;
                    decimal tyLeHoanThanh = Convert.ToDecimal(dr["TY_LE_HOAN_THANH"]);
                    if(tyLeHoanThanh > 100)
                        tyLeHoanThanh = 100;
                    else if(tyLeHoanThanh < 0)
                        tyLeHoanThanh = 0;

                    dr["TONG_PHU_CAP"] = Convert.ToDecimal(dr["TONG_PHU_CAP_BAN_DAU"]) * tyLeHoanThanh / 100;
                    grid.CurrentItem = dr;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        public void LayDuLieuPopup(DataSet ds)
        {
            this.ds = ds;
        }

        /// <summary>
        /// Hàm kiểm tra phụ cấp đã được chọn trong Grid hay chưa
        /// </summary>
        /// <param name="id">id phụ cấp</param>
        /// <returns>true: chưa được chọn;  false đã được chọn</returns>
        private bool KiemTraTonTaiGrid(string id)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["ID_HSO"].ToString().Equals(id))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion               

        #region Xử lý nghiệp vụ
        private void GetFormData(ref NS_THONG_TIN_TINH_LUONG obj, string sTrangThaiNVu)
        {
            try
            {
                obj = new NS_THONG_TIN_TINH_LUONG();
                obj.ID = id;

                #region NS_TINH_LUONG
                NS_TINH_LUONG objTinhLuong = new NS_TINH_LUONG();
                objTinhLuong.ID = id;
                objTinhLuong.MA_GDICH = txtSoGiaoDich.Text;
                objTinhLuong.THANG_TINH_LUONG = Convert.ToDateTime(raddtThang.Value).ToString("yyyyMM");
                objTinhLuong.DIEN_GIAI = txtDienGiai.Text;

                //Thông tin kiểm soát
                objTinhLuong.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                objTinhLuong.TTHAI_NVU = sTrangThaiNVu;
                objTinhLuong.MA_DVI_QLY = ClientInformation.MaDonVi;
                objTinhLuong.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                objTinhLuong.NGAY_NHAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                objTinhLuong.NGUOI_NHAP = txtNguoiLap.Text;
                if (action != DatabaseConstant.Action.THEM)
                {
                    objTinhLuong.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    objTinhLuong.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }

                obj.OBJ_TINH_LUONG = objTinhLuong;
                #endregion

                obj.ID = id;
                obj.MA_GDICH = maGiaoDich;
                obj.DATA_SET = ds;
                obj.MA_DVI = ClientInformation.MaDonVi;
                obj.MA_DVI_GDICH = ClientInformation.MaDonViGiaoDich;
                obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.TTHAI_NVU = sTrangThaiNVu;
                obj.NGAY_NHAP = objTinhLuong.NGAY_NHAP;
                obj.NGUOI_NHAP = objTinhLuong.NGUOI_NHAP;
                obj.NGUOI_CNHAT = objTinhLuong.NGUOI_CNHAT;
                obj.NGAY_CNHAT = objTinhLuong.NGAY_CNHAT;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            NhanSuProcess processNhanSu = new NhanSuProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            obj = new NS_THONG_TIN_TINH_LUONG();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                bool ret = false;
                obj.ID = id;
                obj.MA_GDICH = maGiaoDich;
                ret = processNhanSu.TinhLuong(DatabaseConstant.Action.LOAD, ref obj, ref listClientResponseDetail);
                if (ret == true)
                {
                    sTrangThaiNVu = obj.TTHAI_NVU;
                    isTinhLuong = true;
                    id = obj.OBJ_TINH_LUONG.ID;
                    maGiaoDich = obj.OBJ_TINH_LUONG.MA_GDICH;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    txtSoGiaoDich.Text = obj.OBJ_TINH_LUONG.MA_GDICH;
                    raddtThang.Value = LDateTime.StringToDate(obj.OBJ_TINH_LUONG.THANG_TINH_LUONG + "01", "yyyyMMdd");                    
                    txtDienGiai.Text = obj.OBJ_TINH_LUONG.DIEN_GIAI;

                    ds = obj.DATA_SET;
                    grid.DataContext = ds.Tables[0].DefaultView;

                    #region Thông tin kiểm soát
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    raddtNgayLap.Value = LDateTime.StringToDate(obj.NGAY_NHAP, "yyyyMMdd");
                    txtNguoiLap.Text = obj.NGUOI_NHAP;
                    if (LDateTime.IsDate(obj.NGAY_CNHAT, "yyyyMMdd") == true)
                        raddtNgayCapNhat.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, "yyyyMMdd");
                    txtNguoiCapNhat.Text = obj.NGUOI_CNHAT;
                    #endregion

                }
                else
                {
                    LMessage.ShowMessage("M_ResponseMessage_HanhDong_LayDuLieuKhongThanhCong", LMessage.MessageBoxType.Warning);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M_ResponseMessage_HanhDong_LayDuLieuKhongThanhCong", LMessage.MessageBoxType.Warning);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void ResetForm()
        {
            sTrangThaiNVu = "";
            lblTrangThai.Content = "";
            isTinhLuong = false;
            
            ds.Tables[0].Rows.Clear();
            ds.Tables[1].Rows.Clear();
            grid.DataContext = ds.Tables[0].DefaultView;

            txtSoGiaoDich.Text = "";
            raddtThang.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtDienGiai.Text = "";

            #region Thông tin kiểm soát
            txtTrangThaiBanGhi.Text = "";
            raddtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            raddtNgayCapNhat.Value = null;
            txtNguoiCapNhat.Text = "";
            #endregion

        }

        private void ResetData()
        {
            action = DatabaseConstant.Action.THEM;
            id = 0;
            obj = null;
            sTrangThaiNVu = "";

            ResetForm();
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);

            chkThemNhieuLan.IsChecked = false;
        }

        private void OnPreviewChungTu()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(maGiaoDich))
            {
                LMessage.ShowMessage("Không có thông tin dữ liệu cần hiển thị", LMessage.MessageBoxType.Warning);

            }
            else
            {
                VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();
                DatabaseConstant.Function _function = DatabaseConstant.Function.KT_PHIEU_KE_TOAN;

                GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.GDKT_IN_GIAO_DICH;
                objGIAO_DICH_BASE.ChucNang = _function;

                GDKT_GIAO_DICH objGDKT_GIAO_DICH = new GDKT_GIAO_DICH();
                objGDKT_GIAO_DICH.MaGiaoDich = maGiaoDich;

                doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                doiTuongBaoCao.objGDKT_GIAO_DICH = objGDKT_GIAO_DICH;
                xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void OnPreviewBangLuong()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(maGiaoDich))
            {
                LMessage.ShowMessage("Không có thông tin dữ liệu cần hiển thị", LMessage.MessageBoxType.Warning);

            }
            else
            {
                if (ClientInformation.Company.Equals("M7MFI") || ClientInformation.Company.Equals("BINHKHANH"))
                {
                    
                }
                else if (ClientInformation.Company.Equals("BANTAYVANG"))
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();

                    List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                    DateTime dtNgayDauThang = new DateTime();
                    DateTime dtNgayCuoiThang = new DateTime();
                    int iMonth = Convert.ToDateTime(raddtThang.Value).Month;
                    int iYear = Convert.ToDateTime(raddtThang.Value).Year;
                    dtNgayDauThang = LDateTime.GetFirstDateOfMonth(iYear, iMonth);
                    dtNgayCuoiThang = LDateTime.GetLastDateOfMonth(iYear, iMonth);
                    string tungay = dtNgayDauThang.ToString(ApplicationConstant.defaultDateTimeFormat);
                    string denngay = dtNgayCuoiThang.ToString(ApplicationConstant.defaultDateTimeFormat);

                    lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaPhongGD", "%", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@TuNgay", tungay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@DenNgay", denngay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                    lstThamSo.Add(new ThamSoBaoCao("@ThangBaoCao", Convert.ToDateTime(raddtThang.Value).ToString("yyyyMM"), ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.NSTL_BANG_LUONG);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
                else
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();

                    List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                    DateTime dtNgayDauThang = new DateTime();
                    DateTime dtNgayCuoiThang = new DateTime();
                    int iMonth = Convert.ToDateTime(raddtThang.Value).Month;
                    int iYear = Convert.ToDateTime(raddtThang.Value).Year;
                    dtNgayDauThang = LDateTime.GetFirstDateOfMonth(iYear, iMonth);
                    dtNgayCuoiThang = LDateTime.GetLastDateOfMonth(iYear, iMonth);
                    string tungay = dtNgayDauThang.ToString(ApplicationConstant.defaultDateTimeFormat);
                    string denngay = dtNgayCuoiThang.ToString(ApplicationConstant.defaultDateTimeFormat);

                    lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaPhongGD", "%", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@TuNgay", tungay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@DenNgay", denngay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                    lstThamSo.Add(new ThamSoBaoCao("@ThangBaoCao", Convert.ToDateTime(raddtThang.Value).ToString("yyyyMM"), ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.NSTL_BANG_LUONG);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void OnPreviewTheLuong()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(maGiaoDich))
            {
                LMessage.ShowMessage("Không có thông tin dữ liệu cần hiển thị", LMessage.MessageBoxType.Warning);

            }
            else
            {
                if (ClientInformation.Company.Equals("M7MFI") || ClientInformation.Company.Equals("BINHKHANH"))
                {

                }
                else if (ClientInformation.Company.Equals("BANTAYVANG"))
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();

                    List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                    lstThamSo.Add(new ThamSoBaoCao("@MaGiaoDich", maGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                    List<string> lstID = new List<string>();
                    foreach (DataRowView drv in grid.SelectedItems)
                    {
                        lstID.Add(drv["ID_HSO"].ToString());
                    }

                    for (int i = 0; i < lstID.Count; i++)
                    {
                        lstThamSo.Add(new ThamSoBaoCao("@IdNhanVien", lstID[i], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    }

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.NSTL_THE_LUONG);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
                else
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();

                    List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                    lstThamSo.Add(new ThamSoBaoCao("@MaGiaoDich", maGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                    List<string>  lstID = new List<string>();
                    foreach (DataRowView drv in grid.SelectedItems)
                    {
                        lstID.Add(drv["ID_HSO"].ToString());
                    }

                    for (int i = 0; i < lstID.Count; i++)
                    {
                        lstThamSo.Add(new ThamSoBaoCao("@IdNhanVien", lstID[i], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    }

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.NSTL_THE_LUONG);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void OnPreviewChuyenKhoan()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(maGiaoDich))
            {
                LMessage.ShowMessage("Không có thông tin dữ liệu cần hiển thị", LMessage.MessageBoxType.Warning);

            }
            else
            {
                if (ClientInformation.Company.Equals("M7MFI") || ClientInformation.Company.Equals("BINHKHANH"))
                {

                }
                else if (ClientInformation.Company.Equals("BANTAYVANG"))
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();

                    List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                    string TenNguoiLap = "";
                    string TenChiNhanh = "";
                    string TenPhongGiaoDich = "";

                    lstThamSo.Add(new ThamSoBaoCao("P_TenNguoiLap", TenNguoiLap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_TenChiNhanh", TenChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_TenPGD", TenPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_Thang", raddtThang.Text, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NgayBaoCao", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaPhongGD", "%", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@ThangTinhLuong", Convert.ToDateTime(raddtThang.Value).ToString("yyyyMM"), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.NSTL_DANH_SACH_CHUYEN_KHOAN);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
                else
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();

                    List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                    string TenNguoiLap = "";
                    string TenChiNhanh = "";
                    string TenPhongGiaoDich = "";

                    lstThamSo.Add(new ThamSoBaoCao("P_TenNguoiLap", TenNguoiLap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_TenChiNhanh", TenChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_TenPGD", TenPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_Thang", raddtThang.Text, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NgayBaoCao", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaPhongGD", "%", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@ThangTinhLuong", Convert.ToDateTime(raddtThang.Value).ToString("yyyyMM"), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    
                    string maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.NSTL_DANH_SACH_CHUYEN_KHOAN);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private bool Validation()
        {
            try
            {
                if (raddtThang.Value == null)
                {
                    CommonFunction.ThongBaoChuaNhap(lblTinhLuongThang.Content.ToString());
                    raddtThang.Focus();
                    return false;
                }

                else if (txtDienGiai.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaNhap(lblDienGiai.Content.ToString());
                    txtDienGiai.Focus();
                    return false;
                }

                else if (isTinhLuong == false)
                {
                    LMessage.ShowMessage("Chưa thực hiện tính lương", LMessage.MessageBoxType.Warning);
                    btnCalculate.Focus();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                return false;
            }
        }

        private void SetEnabledControls()
        {
            #region Thêm
            if (action == DatabaseConstant.Action.THEM)
            {
                txtSoGiaoDich.IsEnabled = false;
                raddtThang.IsEnabled = true;
                txtDienGiai.IsEnabled = true;

                btnAdd.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnCalculate.IsEnabled = true;
                btnPBo.IsEnabled = true;
                grid.IsReadOnly = false;
            }
            #endregion

            #region Sửa
            else if (action == DatabaseConstant.Action.SUA)
            {
                txtSoGiaoDich.IsEnabled = false;
                raddtThang.IsEnabled = false;
                txtDienGiai.IsEnabled = true;

                btnAdd.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnCalculate.IsEnabled = true;
                btnPBo.IsEnabled = true;
                grid.IsReadOnly = false;
            }
            #endregion

            #region Xem ~ Mặc định
            else
            {
                txtSoGiaoDich.IsEnabled = false;
                raddtThang.IsEnabled = false;
                txtDienGiai.IsEnabled = false;

                btnAdd.IsEnabled = false;
                btnDelete.IsEnabled = false;
                btnCalculate.IsEnabled = false;
                btnPBo.IsEnabled = false;
                grid.IsReadOnly = false;
            }
            #endregion
        }


        public void OnHold()
        {
            try
            {
                if (!Validation()) return;

                string trangThai = "";
                trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));

                obj = new NS_THONG_TIN_TINH_LUONG();

                GetFormData(ref obj, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(obj);
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(obj);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnSave()
        {                    
            try
            {
                if (!Validation()) return;

                string trangThai = "";
                trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));

                obj = new NS_THONG_TIN_TINH_LUONG();

                GetFormData(ref obj, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(obj);
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(obj);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }

        }

        public void BeforeViewFromDetail()
        {
            action = DatabaseConstant.Action.XEM;
            SetEnabledControls();            
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain,function);
        }

        public void BeforeViewFromList()
        {
            SetFormData();
            BeforeViewFromDetail();
        }


        public void BeforeAddNew()
        {            
            action = DatabaseConstant.Action.THEM;
            ResetForm();
            SetEnabledControls();            
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain,function);
        }

        public void OnAddNew(NS_THONG_TIN_TINH_LUONG obj)
        {
            
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                NhanSuProcess processNhanSu = new NhanSuProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processNhanSu.TinhLuong(DatabaseConstant.Action.THEM, ref obj, ref listClientResponseDetail);
                AfterAddNew(ret, obj, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public void AfterAddNew(bool ret, NS_THONG_TIN_TINH_LUONG obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                    if (chkThemNhieuLan.IsChecked == true)
                    {
                        ResetData();
                    }
                    else
                    {
                        id = obj.ID;
                        sTrangThaiNVu = obj.TTHAI_NVU;

                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                        txtSoGiaoDich.Text = obj.OBJ_TINH_LUONG.MA_GDICH;
                        maGiaoDich = obj.OBJ_TINH_LUONG.MA_GDICH;

                        BeforeViewFromDetail();
                    }
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);                    
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        public void BeforeModifyFromDetail()
        {
            try
            {
                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);

                bool ret = process.LockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_TINH_LUONG_CT,
                    DatabaseConstant.Table.NS_TINH_LUONG,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                if (ret)
                {
                    action = DatabaseConstant.Action.SUA;
                    SetEnabledControls();                    
                    CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain,function);
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

        public void BeforeModifyFromList()
        {
            SetFormData();
            action = DatabaseConstant.Action.SUA;
            SetEnabledControls();            
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnModify(NS_THONG_TIN_TINH_LUONG obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                NhanSuProcess processNhanSu = new NhanSuProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processNhanSu.TinhLuong(DatabaseConstant.Action.SUA,ref obj, ref listClientResponseDetail);
                AfterModify(ret, obj, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public void AfterModify(bool ret, NS_THONG_TIN_TINH_LUONG obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    raddtNgayCapNhat.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                    txtNguoiCapNhat.Text = ClientInformation.TenDangNhap;                   

                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    //LMessage.ShowMessage("M.DungChung.CapNhatKoThanhCong", LMessage.MessageBoxType.Error);
                }

                // Yêu cầu Unlock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);                

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_TINH_LUONG_CT,
                    DatabaseConstant.Table.NS_TINH_LUONG,
                    DatabaseConstant.Action.SUA,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        public void BeforeDelete()
        {
            try
            {
                // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.NSTL,
                        DatabaseConstant.Function.NS_TINH_LUONG_CT,
                        DatabaseConstant.Table.NS_TINH_LUONG,
                        DatabaseConstant.Action.XOA,
                        listLockId);

                    if (retLockData)
                    {
                        action = DatabaseConstant.Action.XOA;
                        OnDelete();
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_TINH_LUONG_CT,
                    DatabaseConstant.Table.NS_TINH_LUONG,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnDelete()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            NhanSuProcess processNhanSu = new NhanSuProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                obj.MA_GDICH = maGiaoDich;
                ret = processNhanSu.TinhLuong(action, ref obj, ref listClientResponseDetail);
                AfterDelete(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                listClientResponseDetail = null;
                processNhanSu = null;
            }
        }

        public void AfterDelete(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    //LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_TINH_LUONG_CT,
                    DatabaseConstant.Table.NS_TINH_LUONG,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                // Đóng cửa sổ chi tiết sau khi xóa
                OnClose();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        public void BeforeApprove()
        {
            try
            {
                // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.NSTL,
                        DatabaseConstant.Function.NS_TINH_LUONG_CT,
                        DatabaseConstant.Table.NS_TINH_LUONG,
                        DatabaseConstant.Action.DUYET,
                        listLockId);

                    if (retLockData)
                    {
                        action = DatabaseConstant.Action.DUYET;
                        OnApprove();
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnApprove()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            NhanSuProcess processNhanSu = new NhanSuProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                obj.MA_GDICH = maGiaoDich;
                ret = processNhanSu.TinhLuong(action, ref obj, ref listClientResponseDetail);
                AfterApprove(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                listClientResponseDetail = null;
                processNhanSu = null;
            }
        }

        public void AfterApprove(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_TINH_LUONG_CT,
                    DatabaseConstant.Table.NS_TINH_LUONG,
                    DatabaseConstant.Action.DUYET,
                    listLockId);
            }
            catch (Exception ex)
            {                
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        public void BeforeCancel()
        {
            try
            {
                // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.NSTL,
                        DatabaseConstant.Function.NS_TINH_LUONG_CT,
                        DatabaseConstant.Table.NS_TINH_LUONG,
                        DatabaseConstant.Action.THOAI_DUYET,
                        listLockId);

                    if (retLockData)
                    {
                        action = DatabaseConstant.Action.THOAI_DUYET;
                        OnCancel();
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnCancel()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            NhanSuProcess processNhanSu = new NhanSuProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                obj.MA_GDICH = maGiaoDich;
                ret = processNhanSu.TinhLuong(action, ref obj, ref listClientResponseDetail);
                AfterCancel(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                listClientResponseDetail = null;
                processNhanSu = null;
            }
        }

        public void AfterCancel(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_TINH_LUONG_CT,
                    DatabaseConstant.Table.NS_TINH_LUONG,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listLockId);

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }


        public void BeforeRefuse()
        {
            try
            {
                // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.NSTL,
                        DatabaseConstant.Function.NS_TINH_LUONG_CT,
                        DatabaseConstant.Table.NS_TINH_LUONG,
                        DatabaseConstant.Action.TU_CHOI_DUYET,
                        listLockId);

                    if (retLockData)
                    {
                        action = DatabaseConstant.Action.TU_CHOI_DUYET;
                        OnRefuse();
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnRefuse()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            NhanSuProcess processNhanSu = new NhanSuProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                obj.MA_GDICH = maGiaoDich;
                ret = processNhanSu.TinhLuong(action, ref obj, ref listClientResponseDetail);
                AfterRefuse(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                listClientResponseDetail = null;
                processNhanSu = null;
            }
        }

        public void AfterRefuse(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_TINH_LUONG_CT,
                    DatabaseConstant.Table.NS_TINH_LUONG,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        public void OnTinhToan()
        {            
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            NhanSuProcess processNhanSu = new NhanSuProcess();
            try
            {
                GetFormData(ref obj, sTrangThaiNVu);

                isTinhLuong = processNhanSu.TinhLuong(DatabaseConstant.Action.TINH_TOAN, ref obj, ref listClientResponseDetail);

                AfterTinhToan(isTinhLuong, obj, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);                
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                listClientResponseDetail = null;
                processNhanSu = null;
            }
        }

        public void AfterTinhToan(bool ret, NS_THONG_TIN_TINH_LUONG obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("Tính lương thành công", LMessage.MessageBoxType.Information);

                    raddtThang.IsEnabled = false;
                    ds = obj.DATA_SET;
                    grid.DataContext = ds.Tables[0].DefaultView;

                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);                    
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        #endregion       
    }
}
