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
using PresentationWPF.NhanSu.Converts;

namespace PresentationWPF.NhanSu.Luong
{
    /// <summary>
    /// Interaction logic for ucLuongCT.xaml
    /// </summary>
    public partial class ucLuongCT : UserControl
    {
        #region Khai bao
        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private DatabaseConstant.Function function = DatabaseConstant.Function.NS_LUONG_CT;

        public event EventHandler OnSavingCompleted;

        private int soBacLuong = 7;

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int idHoSo = 0;
       
        private NS_THONG_TIN_LUONG obj;
        public NS_THONG_TIN_LUONG Obj
        {
            get { return obj; }
            set { obj = value; }
        }

        private DataTable dtLuong = null;
        private DataTable dtPhuCap = null;
        private DataTable dtBacLuong = null;

        private string sTrangThaiNVu = "";
        
        List<AutoCompleteEntry> lstSourcePhongBan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceChucVu = new List<AutoCompleteEntry>();

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
        public ucLuongCT()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            KhoiTaoDataTable();

            LoadCombobox();

            InitEventHandler();

            txtMaNhanVien.Focus();

        }        

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.NhanSu;component/Luong/ucLuongCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void InitEventHandler()
        {
            chkThemNhieuLan.Checked += new RoutedEventHandler(chkThemNhieuLan_Checked);

            btnAddLuong.Click += new RoutedEventHandler(btnAddLuong_Click);
            btnDeleteLuong.Click += new RoutedEventHandler(btnDeleteLuong_Click);
            grLuong.CellEditEnded += new EventHandler<GridViewCellEditEndedEventArgs>(grLuong_CellEditEnded);

            txtMaNhanVien.KeyDown += new KeyEventHandler(txtMaNhanVien_KeyDown);
            btnMaNhanVien.Click += new RoutedEventHandler(btnMaNhanVien_Click);

            numTyLeHuongLuong.LostFocus += new RoutedEventHandler(numTyLeHuongLuong_LostFocus);

            btnAddPhuCap.Click += new RoutedEventHandler(btnAddPhuCap_Click);
            btnDeletePhuCap.Click += new RoutedEventHandler(btnDeletePhuCap_Click);
        }

        private void KhoiTaoDataTable()
        {
            dtLuong = new DataTable();
            dtLuong.Columns.Add("STT", typeof(int));
            dtLuong.Columns.Add("BAC_LUONG", typeof(int));
            dtLuong.Columns.Add("LUONG_CO_BAN", typeof(decimal));
            dtLuong.Columns.Add("LUONG_TINH_BH", typeof(decimal));
            dtLuong.Columns.Add("NGAY_AP_DUNG", typeof(string));
            dtLuong.Columns.Add("BHXH", typeof(bool));
            dtLuong.Columns.Add("BHYT", typeof(bool));
            dtLuong.Columns.Add("BHTN", typeof(bool));
            dtLuong.Columns.Add("KPCD", typeof(bool));

            dtPhuCap = new DataTable();
            dtPhuCap.Columns.Add("STT", typeof(int));
            dtPhuCap.Columns.Add("ID_PHU_CAP", typeof(int));
            dtPhuCap.Columns.Add("TEN_PHU_CAP", typeof(string));
            dtPhuCap.Columns.Add("MUC_PHU_CAP", typeof(decimal));            
            dtPhuCap.Columns.Add("NGAY_AP_DUNG", typeof(string));
            dtPhuCap.Columns.Add("THUE_TNCN", typeof(bool));

            dtBacLuong = new DataTable();
            dtBacLuong.Columns.Add("ID");
            dtBacLuong.Columns.Add("NAME");
            for(int i=1; i<= soBacLuong; i++)
                dtBacLuong.Rows.Add(i, i);                
                            
        }

        private void LoadCombobox()
        {
            COMBOBOX_DTO combo = null;
            List<string> lstDieuKien = null;
            List<COMBOBOX_DTO> lstCombobox = new List<COMBOBOX_DTO>();

            //Phòng ban
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_NS_PHONG_BAN.getValue();
            combo.combobox = cmbPhongBan;
            combo.lstSource = lstSourcePhongBan;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Chức vụ
            combo = new COMBOBOX_DTO();
            lstDieuKien = new List<string>();
            lstDieuKien.Add("%");
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_NS_CHUC_VU.getValue();
            combo.combobox = cmbChucVu;
            combo.lstSource = lstSourceChucVu;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);           

            AutoComboBox auto = new AutoComboBox();
            auto.GenAutoComboBoxTheoList(ref lstCombobox);


            //Combobox cột bậc lương trong grid
            ((GridViewComboBoxColumn)grLuong.Columns["BAC_LUONG"]).ItemsSource = dtBacLuong.DefaultView;

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
                        KeyGesture keyg = new KeyGesture(Key.H, ModifierKeys.Control);
                        key = new KeyBinding(HoldCommand, keyg);
                        key.Gesture = keyg;
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
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift);
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

        private void HoldCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnHold();
        }

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
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
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
                DatabaseConstant.Function.NS_LUONG_CT,
                DatabaseConstant.Table.NS_LUONG,
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
            //txtTenNhanVien.Focus();

            //chkThemNhieuLan.IsChecked = false;
        }        

        private void btnMaNhanVien_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lstPopup = null;
                var process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_LUONG_HSO.getValue(), lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow row = lstPopup[0];
                    if (!string.IsNullOrWhiteSpace(row[1].ToString()))
                        idHoSo = Convert.ToInt32(row[1].ToString());
                    if (!string.IsNullOrWhiteSpace(row[2].ToString()))
                        txtMaNhanVien.Text = row[2].ToString();
                    if (!string.IsNullOrWhiteSpace(row[3].ToString()))
                        txtTenNhanVien.Text = row[3].ToString();
                    if (!string.IsNullOrWhiteSpace(row[4].ToString()))
                        cmbChucVu.SelectedIndex = lstSourceChucVu.IndexOf(lstSourceChucVu.FirstOrDefault(i => i.KeywordStrings[1].Equals(row[4].ToString())));
                    if (!string.IsNullOrWhiteSpace(row[5].ToString()))
                        cmbPhongBan.SelectedIndex = lstSourcePhongBan.IndexOf(lstSourcePhongBan.FirstOrDefault(i => i.KeywordStrings[1].Equals(row[5].ToString())));

                    if (!string.IsNullOrWhiteSpace(row[6].ToString()))
                    {
                        if (row[6].ToString().Equals(BusinessConstant.LOAI_HO_SO.CHINH_THUC.layGiaTri()))
                        {
                            numTyLeHuongLuong.Value = 100;
                        }
                        else if (row[6].ToString().Equals(BusinessConstant.LOAI_HO_SO.HOC_VIEC.layGiaTri()))
                        {
                            numTyLeHuongLuong.Value = 85;
                        }
                        else if (row[6].ToString().Equals(BusinessConstant.LOAI_HO_SO.THU_VIEC.layGiaTri()))
                        {
                            numTyLeHuongLuong.Value = 85;
                        }
                        else
                        {
                            numTyLeHuongLuong.Value = 0;
                        }
                    }

                    dtLuong.Rows.Clear();
                    grLuong.DataContext = dtLuong.DefaultView;

                    dtPhuCap.Rows.Clear();
                    grPhuCap.DataContext = dtPhuCap.DefaultView;  
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                CommonFunction.ThongBaoLoi(ex);
            }
        }

        private void txtMaNhanVien_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnMaNhanVien_Click(null, null);
            }
        }

        private void btnAddLuong_Click(object sender, RoutedEventArgs e)
        {
            DataRow dr = dtLuong.NewRow();            
            //dr["BAC_LUONG"] = null;
            dr["LUONG_CO_BAN"] = 0;
            dr["LUONG_TINH_BH"] = 0;
            dr["NGAY_AP_DUNG"] = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai,"yyyyMMdd").ToString("dd/MM/yyyy");
            dr["BHXH"] = true;
            dr["BHYT"] = true;
            dr["BHTN"] = true;
            dr["KPCD"] = true;
            dtLuong.Rows.Add(dr);

            for (int i = 0; i < dtLuong.Rows.Count; i++)
            {
                dtLuong.Rows[i]["STT"] = i + 1;
            }

            grLuong.DataContext = dtLuong.DefaultView;
        }

        private void btnDeleteLuong_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<int> lstSTT = new List<int>();
                for (int i = 0; i < grLuong.SelectedItems.Count; i++)
                {
                    DataRowView dr = (DataRowView)grLuong.SelectedItems[i];
                    lstSTT.Add(Convert.ToInt32(dr["STT"]));
                }
                lstSTT.SortByDesc();
                foreach (int stt in lstSTT)
                    dtLuong.Rows.RemoveAt(stt - 1);

                for (int i = 0; i < dtLuong.Rows.Count; i++)
                {
                    dtLuong.Rows[i]["STT"] = i + 1;
                }

                grLuong.DataContext = dtLuong.DefaultView;
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void grLuong_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            string column = e.Cell.Column.Name;

            if (column.Equals("BAC_LUONG"))
            {
                DataRowView dr = (DataRowView)grLuong.CurrentCellInfo.Item;
                
                int bacLuong = Convert.ToInt32(dr["BAC_LUONG"]);

                decimal luongCoBan = 0;
                bool ret = OnTinhToan(bacLuong, ref luongCoBan);
                if(ret == true)
                {
                    dr["LUONG_CO_BAN"] = luongCoBan;
                    dr["LUONG_TINH_BH"] = luongCoBan;
                    grLuong.CurrentItem = dr;
                }
                else
                {
                    dr["LUONG_CO_BAN"] = 0;
                    dr["LUONG_TINH_BH"] = 0;
                    grLuong.CurrentItem = dr;
                }
            }

        }

        private void btnAddPhuCap_Click(object sender, RoutedEventArgs e)
        {
            lstPopup = null;
            var process = new PopupProcess();            
            process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_PHU_CAP.getValue());
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

            ucPopup popup = new ucPopup(false, simplePopupResponse, false);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.Content = popup;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Title = "Danh sách phụ cấp";
            win.ShowDialog();

            if (lstPopup != null)
            {
                foreach (DataRow dr in lstPopup)
                {
                    DataRow drPhuCap = dtPhuCap.NewRow();

                    if (KiemTraPhuCap(dr["ID"].ToString()))
                    {
                        drPhuCap["ID_PHU_CAP"] = Convert.ToInt32(dr["ID"]);
                        drPhuCap["TEN_PHU_CAP"] = dr["TEN"];
                        drPhuCap["MUC_PHU_CAP"] = 0;
                        drPhuCap["NGAY_AP_DUNG"] = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai,"yyyyMMdd").ToString("dd/MM/yyyy");
                        drPhuCap["THUE_TNCN"] = true;

                        dtPhuCap.Rows.Add(drPhuCap);
                    }
                }

                for (int i = 0; i < dtPhuCap.Rows.Count; i++)
                {
                    dtPhuCap.Rows[i]["STT"] = i + 1;
                }

                grPhuCap.DataContext = dtPhuCap.DefaultView;
            }            
        }

        private void btnDeletePhuCap_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<int> lstSTT = new List<int>();
                for (int i = 0; i < grPhuCap.SelectedItems.Count; i++)
                {
                    DataRowView dr = (DataRowView)grPhuCap.SelectedItems[i];
                    lstSTT.Add(Convert.ToInt32(dr["STT"]));
                }
                lstSTT.SortByDesc();
                foreach (int stt in lstSTT)
                    dtPhuCap.Rows.RemoveAt(stt - 1);

                for (int i = 0; i < dtPhuCap.Rows.Count; i++)
                {
                    dtPhuCap.Rows[i]["STT"] = i + 1;
                }

                grPhuCap.DataContext = dtPhuCap.DefaultView;
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Hàm kiểm tra phụ cấp đã được chọn trong Grid hay chưa
        /// </summary>
        /// <param name="id">id phụ cấp</param>
        /// <returns>true: chưa được chọn;  false đã được chọn</returns>
        private bool KiemTraPhuCap(string id)
        {
            foreach (DataRow dr in dtPhuCap.Rows)
            {
                if (dr["ID_PHU_CAP"].ToString().Equals(id))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion               

        #region Xử lý nghiệp vụ
        private void GetFormData(ref NS_THONG_TIN_LUONG obj, string sTrangThaiNVu)
        {
            try
            {
                obj = new NS_THONG_TIN_LUONG();                

                #region NS_LUONG
                NS_LUONG objLuong = new NS_LUONG();
                objLuong.ID = id;
                objLuong.ID_HO_SO = idHoSo;
                objLuong.SO_NGUOI_PHU_THUOC = Convert.ToInt32(numSoNguoiPhuThuoc.Value);
                objLuong.MA_SO_THUE = txtMaSoThue.Text;
                objLuong.TY_LE_HUONG_LUONG = Convert.ToInt32(numTyLeHuongLuong.Value);
                objLuong.SO_TAI_KHOAN = txtSoTaiKhoan.Text;
                objLuong.NGAN_HANG = txtTaiNganHang.Text;

                int count1 = dtLuong.Rows.Count - 1;
                objLuong.BAC_LUONG = Convert.ToInt32(dtLuong.Rows[count1]["BAC_LUONG"]);
                objLuong.LUONG_CO_BAN = Convert.ToDecimal(dtLuong.Rows[count1]["LUONG_CO_BAN"]);
                objLuong.LUONG_TINH_BH = Convert.ToDecimal(dtLuong.Rows[count1]["LUONG_TINH_BH"]);
                objLuong.BHXH = (Convert.ToBoolean(dtLuong.Rows[count1]["BHXH"]) == true) ? "CO" : "KHONG";
                objLuong.BHYT = (Convert.ToBoolean(dtLuong.Rows[count1]["BHYT"]) == true) ? "CO" : "KHONG";
                objLuong.BHTN = (Convert.ToBoolean(dtLuong.Rows[count1]["BHTN"]) == true) ? "CO" : "KHONG";
                objLuong.KPCD = (Convert.ToBoolean(dtLuong.Rows[count1]["KPCD"]) == true) ? "CO" : "KHONG";

                //Thông tin kiểm soát
                objLuong.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                objLuong.TTHAI_NVU = sTrangThaiNVu;
                objLuong.MA_DVI_QLY = ClientInformation.MaDonVi;
                objLuong.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                objLuong.NGAY_NHAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                objLuong.NGUOI_NHAP = txtNguoiLap.Text;
                if (action != DatabaseConstant.Action.THEM)
                {
                    objLuong.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    objLuong.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }

                obj.OBJ_LUONG = objLuong;
                #endregion

                #region NS_LUONG_LSU
                List<NS_LUONG_LSU> lstLSu = new List<NS_LUONG_LSU>();
                NS_LUONG_LSU objLSu = null;
                foreach (DataRow dr in dtLuong.Rows)
                {
                    objLSu = new NS_LUONG_LSU();
                    
                    objLSu.NGAY_AP_DUNG = LDateTime.StringToDate(dr["NGAY_AP_DUNG"].ToString(),"dd/MM/yyyy").ToString("yyyyMMdd");
                    objLSu.ID_LUONG = objLuong.ID;
                    objLSu.ID_HO_SO = objLuong.ID_HO_SO;
                    objLSu.BAC_LUONG = Convert.ToInt32(dtLuong.Rows[count1]["BAC_LUONG"]);
                    objLSu.LUONG_CO_BAN = Convert.ToDecimal(dr["LUONG_CO_BAN"]);
                    objLSu.LUONG_TINH_BH = Convert.ToDecimal(dr["LUONG_TINH_BH"]);
                    objLSu.BHXH = (Convert.ToBoolean(dr["BHXH"]) == true) ? "CO" : "KHONG";
                    objLSu.BHYT = (Convert.ToBoolean(dr["BHYT"]) == true) ? "CO" : "KHONG";
                    objLSu.BHTN = (Convert.ToBoolean(dr["BHTN"]) == true) ? "CO" : "KHONG";
                    objLSu.KPCD = (Convert.ToBoolean(dr["KPCD"]) == true) ? "CO" : "KHONG";
                    objLSu.TTHAI_BGHI = objLuong.TTHAI_BGHI;
                    objLSu.TTHAI_NVU = objLuong.TTHAI_NVU;
                    objLSu.MA_DVI_QLY = objLuong.MA_DVI_QLY;
                    objLSu.MA_DVI_TAO = objLuong.MA_DVI_TAO;
                    objLSu.NGAY_NHAP = objLuong.NGAY_NHAP;
                    objLSu.NGUOI_NHAP = objLuong.NGUOI_NHAP;
                    objLSu.NGAY_CNHAT = objLuong.NGAY_NHAP;
                    objLSu.NGUOI_CNHAT = objLuong.NGUOI_CNHAT;

                    lstLSu.Add(objLSu);
                }

                obj.LST_LUONG_LSU = lstLSu.ToArray();
                #endregion

                #region NS_LUONG_PHU_CAP
                List<NS_LUONG_PHU_CAP> lstPhuCap = new List<NS_LUONG_PHU_CAP>();
                if (dtPhuCap.Rows.Count > 0)
                {
                    NS_LUONG_PHU_CAP objPhuCap = null;
                    foreach (DataRow dr in dtPhuCap.Rows)
                    {
                        objPhuCap = new NS_LUONG_PHU_CAP();
                        
                        objPhuCap.ID_LUONG = objLuong.ID;
                        objPhuCap.ID_HO_SO = objLuong.ID_HO_SO;
                        objPhuCap.ID_PHU_CAP = Convert.ToInt32(dr["ID_PHU_CAP"]);
                        objPhuCap.MUC_PHU_CAP = Convert.ToDecimal(dr["MUC_PHU_CAP"]);
                        objPhuCap.CHIU_THUE_TNCN = (Convert.ToBoolean(dr["THUE_TNCN"]) == true) ? "CO" : "KHONG";
                        objPhuCap.NGAY_APDUNG = LDateTime.StringToDate(dr["NGAY_AP_DUNG"].ToString(), "dd/MM/yyyy").ToString("yyyyMMdd");
                        objPhuCap.TTHAI_BGHI = objLuong.TTHAI_BGHI;
                        objPhuCap.TTHAI_NVU = objLuong.TTHAI_NVU;
                        objPhuCap.MA_DVI_QLY = objLuong.MA_DVI_QLY;
                        objPhuCap.MA_DVI_TAO = objLuong.MA_DVI_TAO;
                        objPhuCap.NGAY_NHAP = objLuong.NGAY_NHAP;
                        objPhuCap.NGUOI_NHAP = objLuong.NGUOI_NHAP;
                        objPhuCap.NGAY_CNHAT = objLuong.NGAY_NHAP;
                        objPhuCap.NGUOI_CNHAT = objLuong.NGUOI_CNHAT;

                        lstPhuCap.Add(objPhuCap);
                    }
                }

                obj.LST_LUONG_PHU_CAP = lstPhuCap.ToArray();
                #endregion

                obj.ID = id;
                obj.ID_NHAN_VIEN = idHoSo;
                obj.MA_NHAN_VIEN = txtMaNhanVien.Text;
                obj.HO_TEN = txtTenNhanVien.Text;
                obj.ID_PHONG_BAN = Convert.ToInt32(lstSourcePhongBan[cmbPhongBan.SelectedIndex].KeywordStrings[1]);
                obj.ID_CHUC_VU = Convert.ToInt32(lstSourceChucVu[cmbChucVu.SelectedIndex].KeywordStrings[1]);
                obj.MA_DVI = ClientInformation.MaDonVi;
                obj.MA_DVI_GDICH = ClientInformation.MaDonViGiaoDich;
                obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.TTHAI_NVU = sTrangThaiNVu;
                obj.NGAY_NHAP = objLuong.NGAY_NHAP;
                obj.NGUOI_NHAP = objLuong.NGUOI_NHAP;
                obj.NGUOI_CNHAT = objLuong.NGUOI_CNHAT;
                obj.NGAY_CNHAT = objLuong.NGAY_CNHAT;
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
            obj = new NS_THONG_TIN_LUONG();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processNhanSu.Luong(DatabaseConstant.Action.LOAD, ref obj, ref listClientResponseDetail);
                if (ret == true)
                {
                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    #region Thông tin chung
                    idHoSo = obj.OBJ_LUONG.ID_HO_SO;
                    txtMaNhanVien.Text = obj.MA_NHAN_VIEN;
                    txtTenNhanVien.Text = obj.HO_TEN;
                    cmbChucVu.SelectedIndex = lstSourceChucVu.IndexOf(lstSourceChucVu.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.ID_CHUC_VU.ToString())));
                    cmbPhongBan.SelectedIndex = lstSourcePhongBan.IndexOf(lstSourcePhongBan.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.ID_PHONG_BAN.ToString())));
                    numSoNguoiPhuThuoc.Value = Convert.ToDouble(obj.OBJ_LUONG.SO_NGUOI_PHU_THUOC);
                    txtMaSoThue.Text = obj.OBJ_LUONG.MA_SO_THUE;
                    numTyLeHuongLuong.Value = Convert.ToDouble(obj.OBJ_LUONG.TY_LE_HUONG_LUONG);
                    txtSoTaiKhoan.Text = obj.OBJ_LUONG.SO_TAI_KHOAN;
                    txtTaiNganHang.Text = obj.OBJ_LUONG.NGAN_HANG;
                    #endregion

                    #region Lương
                    foreach (var item in obj.LST_LUONG_LSU)
                    {
                        DataRow dr = dtLuong.NewRow();
                        dr["BAC_LUONG"] = item.BAC_LUONG;
                        dr["LUONG_CO_BAN"] = item.LUONG_CO_BAN;
                        dr["LUONG_TINH_BH"] = item.LUONG_TINH_BH;
                        dr["NGAY_AP_DUNG"] = LDateTime.StringToDate(item.NGAY_AP_DUNG, "yyyyMMdd").ToString("dd/MM/yyyy");
                        dr["BHXH"] = (item.BHXH.Equals(BusinessConstant.CoKhong.CO.layGiaTri())) ? true : false;
                        dr["BHYT"] = (item.BHYT.Equals(BusinessConstant.CoKhong.CO.layGiaTri())) ? true : false;
                        dr["BHTN"] = (item.BHTN.Equals(BusinessConstant.CoKhong.CO.layGiaTri())) ? true : false;
                        dr["KPCD"] = (item.KPCD.Equals(BusinessConstant.CoKhong.CO.layGiaTri())) ? true : false;

                        dtLuong.Rows.Add(dr);
                    }

                    for (int i = 0; i < dtLuong.Rows.Count; i++)
                    {
                        dtLuong.Rows[i]["STT"] = i + 1;
                    }

                    grLuong.DataContext = dtLuong.DefaultView;
                    #endregion

                    #region Thông tin phụ cấp
                    foreach (var item in obj.LST_LUONG_PHU_CAP)
                    {
                        DataRow dr = dtPhuCap.NewRow();
                        dr["ID_PHU_CAP"] = item.ID_PHU_CAP;
                        dr["TEN_PHU_CAP"] = obj.LST_DM_PHU_CAP.Where(e => e.ID == item.ID_PHU_CAP).FirstOrDefault().TEN;
                        dr["MUC_PHU_CAP"] = item.MUC_PHU_CAP;
                        dr["NGAY_AP_DUNG"] = LDateTime.StringToDate(item.NGAY_APDUNG, "yyyyMMdd").ToString("dd/MM/yyyy");
                        dr["THUE_TNCN"] = (item.CHIU_THUE_TNCN.Equals(BusinessConstant.CoKhong.CO.layGiaTri())) ? true : false;

                        dtPhuCap.Rows.Add(dr);
                    }

                    for (int i = 0; i < dtPhuCap.Rows.Count; i++)
                    {
                        dtPhuCap.Rows[i]["STT"] = i + 1;
                    }

                    grPhuCap.DataContext = dtPhuCap.DefaultView;
                    #endregion

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

            #region Thông tin chung
            id = 0;
            idHoSo = 0;
            txtMaNhanVien.Text = "";
            txtTenNhanVien.Text = "";
            cmbPhongBan.Text = "";
            cmbChucVu.Text = "";
            numSoNguoiPhuThuoc.Value = 0;
            txtMaSoThue.Text = "";
            numTyLeHuongLuong.Value = 100;
            txtSoTaiKhoan.Text = "";
            txtTaiNganHang.Text = "";

            dtLuong.Rows.Clear();
            grLuong.DataContext = dtLuong.DefaultView;

            dtPhuCap.Rows.Clear();
            grPhuCap.DataContext = dtPhuCap.DefaultView;            

            #endregion

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

        private bool Validation()
        {
            try
            {
                if (txtMaNhanVien.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaChon(lblMaNhanVien.Content.ToString());
                    txtMaNhanVien.Focus();
                    return false;
                }

                if (numTyLeHuongLuong.Value == null || numTyLeHuongLuong.Value <= 0 || numTyLeHuongLuong.Value > 100)
                {
                    CommonFunction.ThongBaoChuaNhap(lblTyLeHuongLuong.Content.ToString());
                    numTyLeHuongLuong.Focus();
                    return false;
                }

                if (dtLuong.Rows.Count<=0)
                {
                    CommonFunction.ThongBaoChuaNhap("thông tin lương:");
                    txtMaNhanVien.Focus();
                    return false;
                }

                string message = "";
                if (KiemTraGridLuong(ref message) == false)
                {
                    LMessage.ShowMessage(message, LMessage.MessageBoxType.Warning);
                    return false;
                }

                if (KiemTraGridPhuCap(ref message) == false)
                {
                    LMessage.ShowMessage(message, LMessage.MessageBoxType.Warning);
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

        private bool KiemTraGridLuong(ref string message)
        {
            bool kq = true;


            message = "Thông tin lương không hợp lệ các dòng:";
            foreach (DataRow dr in dtLuong.Rows)
            {
                if (!dr["BAC_LUONG"].ToString().IsNumeric())
                {
                    message = message + " " + dr["STT"].ToString();
                    kq = false;
                    continue;
                }
                if (!dr["LUONG_CO_BAN"].ToString().IsNumeric())
                {
                    message = message + " " + dr["STT"].ToString();
                    kq = false;
                    continue;
                }
                if (!dr["LUONG_TINH_BH"].ToString().IsNumeric())
                {
                    message = message + " " + dr["STT"].ToString();
                    kq = false;
                    continue;
                }

                if (!LDateTime.IsDate(dr["NGAY_AP_DUNG"].ToString(), "dd/MM/yyyy"))
                {
                    message = message + " " + dr["STT"].ToString();
                    kq = false;
                    continue;
                }

                int bacLuong = Convert.ToInt32(dr["BAC_LUONG"]);
                if (bacLuong <= 0 || bacLuong > soBacLuong)
                {
                    message = message + " " + dr["STT"].ToString();
                    kq = false;
                    continue;
                }

                double luongCoBan = Convert.ToDouble(dr["LUONG_CO_BAN"]);
                double luongTinhBH = Convert.ToDouble(dr["LUONG_TINH_BH"]);
                if (luongCoBan < luongTinhBH)
                {
                    message = message + " " + dr["STT"].ToString();
                    kq = false;
                    continue;
                }

            }

            return kq;
        }

        private bool KiemTraGridPhuCap(ref string message)
        {
            bool kq = true;


            message = "Thông tin phụ cấp không hợp lệ các dòng:";
            foreach (DataRow dr in dtPhuCap.Rows)
            {
                if (!dr["MUC_PHU_CAP"].ToString().IsNumeric())
                {
                    message = message + " " + dr["STT"].ToString();
                    kq = false;
                    continue;
                }
                if (!LDateTime.IsDate(dr["NGAY_AP_DUNG"].ToString(),"dd/MM/yyyy"))
                {
                    message = message + " " + dr["STT"].ToString();
                    kq = false;
                    continue;
                }
            }

            return kq;
        }

        private void SetEnabledControls()
        {
            #region Thêm
            if (action == DatabaseConstant.Action.THEM)
            {
                txtMaNhanVien.IsEnabled = true;
                btnMaNhanVien.IsEnabled = true;
                txtTenNhanVien.IsEnabled = false;                
                cmbPhongBan.IsEnabled = false;
                cmbChucVu.IsEnabled = false;
                numSoNguoiPhuThuoc.IsEnabled = true;
                txtMaSoThue.IsEnabled = true;
                numTyLeHuongLuong.IsEnabled = true;
                txtSoTaiKhoan.IsEnabled = true;
                txtTaiNganHang.IsEnabled = true;

                btnAddLuong.IsEnabled = true;
                btnDeleteLuong.IsEnabled = true;
                grLuong.IsEnabled = true;

                btnAddPhuCap.IsEnabled = true;
                btnDeletePhuCap.IsEnabled = true;
                grPhuCap.IsEnabled = true;
            }
            #endregion

            #region Sửa
            else if (action == DatabaseConstant.Action.SUA)
            {
                txtMaNhanVien.IsEnabled = true;
                btnMaNhanVien.IsEnabled = true;
                txtTenNhanVien.IsEnabled = false;
                cmbPhongBan.IsEnabled = false;
                cmbChucVu.IsEnabled = false;
                numSoNguoiPhuThuoc.IsEnabled = true;
                txtMaSoThue.IsEnabled = true;
                numTyLeHuongLuong.IsEnabled = true;
                txtSoTaiKhoan.IsEnabled = true;
                txtTaiNganHang.IsEnabled = true;

                btnAddLuong.IsEnabled = true;
                btnDeleteLuong.IsEnabled = true;
                grLuong.IsEnabled = true;

                btnAddPhuCap.IsEnabled = true;
                btnDeletePhuCap.IsEnabled = true;
                grPhuCap.IsEnabled = true;

                if (sTrangThaiNVu == BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri())
                {
                    btnAddLuong.IsEnabled = false;
                    btnDeleteLuong.IsEnabled = false;
                    grLuong.IsEnabled = false;

                    btnAddPhuCap.IsEnabled = false;
                    btnDeletePhuCap.IsEnabled = false;
                    grPhuCap.IsEnabled = false;
                }
               
            }
            #endregion

            #region Xem ~ Mặc định
            else
            {
                txtMaNhanVien.IsEnabled = false;
                btnMaNhanVien.IsEnabled = false;
                txtTenNhanVien.IsEnabled = false;
                cmbPhongBan.IsEnabled = false;
                cmbChucVu.IsEnabled = false;
                numSoNguoiPhuThuoc.IsEnabled = false;
                txtMaSoThue.IsEnabled = false;
                numTyLeHuongLuong.IsEnabled = false;
                txtSoTaiKhoan.IsEnabled = false;
                txtTaiNganHang.IsEnabled = false;

                btnAddLuong.IsEnabled = false;
                btnDeleteLuong.IsEnabled = false;
                grLuong.IsEnabled = false;

                btnAddPhuCap.IsEnabled = false;
                btnDeletePhuCap.IsEnabled = false;
                grPhuCap.IsEnabled = false;
            }
            #endregion
        }


        public void OnHold()
        {
            try
            {
                if (!Validation()) return;

                string trangThai = "";
                if (sTrangThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                else
                    trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));

                obj = new NS_THONG_TIN_LUONG();

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
                if (sTrangThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                else
                    trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));

                obj = new NS_THONG_TIN_LUONG();

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

        public void OnAddNew(NS_THONG_TIN_LUONG obj)
        {
            
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                NhanSuProcess processNhanSu = new NhanSuProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processNhanSu.Luong(DatabaseConstant.Action.THEM, ref obj, ref listClientResponseDetail);
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

        public void AfterAddNew(bool ret, NS_THONG_TIN_LUONG obj, List<ClientResponseDetail> listClientResponseDetail)
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
                    DatabaseConstant.Function.NS_LUONG_CT,
                    DatabaseConstant.Table.NS_LUONG,
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

        public void OnModify(NS_THONG_TIN_LUONG obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                NhanSuProcess processNhanSu = new NhanSuProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processNhanSu.Luong(DatabaseConstant.Action.SUA,ref obj, ref listClientResponseDetail);
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

        public void AfterModify(bool ret, NS_THONG_TIN_LUONG obj, List<ClientResponseDetail> listClientResponseDetail)
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
                    DatabaseConstant.Function.NS_LUONG_CT,
                    DatabaseConstant.Table.NS_LUONG,
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
                        DatabaseConstant.Function.NS_LUONG_CT,
                        DatabaseConstant.Table.NS_LUONG,
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
                    DatabaseConstant.Function.NS_LUONG_CT,
                    DatabaseConstant.Table.NS_LUONG,
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
                ret = processNhanSu.Luong(action, ref obj, ref listClientResponseDetail);
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
                    DatabaseConstant.Function.NS_LUONG_CT,
                    DatabaseConstant.Table.NS_LUONG,
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
                        DatabaseConstant.Function.NS_LUONG_CT,
                        DatabaseConstant.Table.NS_LUONG,
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
                ret = processNhanSu.Luong(action, ref obj, ref listClientResponseDetail);
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
                    DatabaseConstant.Function.NS_LUONG_CT,
                    DatabaseConstant.Table.NS_LUONG,
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
                        DatabaseConstant.Function.NS_LUONG_CT,
                        DatabaseConstant.Table.NS_LUONG,
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
                ret = processNhanSu.Luong(action, ref obj, ref listClientResponseDetail);
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
                    DatabaseConstant.Function.NS_LUONG_CT,
                    DatabaseConstant.Table.NS_LUONG,
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
                        DatabaseConstant.Function.NS_LUONG_CT,
                        DatabaseConstant.Table.NS_LUONG,
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
                ret = processNhanSu.Luong(action, ref obj, ref listClientResponseDetail);
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
                    DatabaseConstant.Function.NS_LUONG_CT,
                    DatabaseConstant.Table.NS_LUONG,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        public bool OnTinhToan(int bacLuong, ref decimal luongCoBan)
        {
            bool ret = false;
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            NhanSuProcess processNhanSu = new NhanSuProcess();
            try
            {                
                obj = new NS_THONG_TIN_LUONG();
                obj.ID_CHUC_VU = Convert.ToInt32(lstSourceChucVu[cmbChucVu.SelectedIndex].KeywordStrings[1]);
                obj.OBJ_LUONG = new NS_LUONG();
                obj.OBJ_LUONG.BAC_LUONG = bacLuong;
                ret = processNhanSu.Luong(DatabaseConstant.Action.TINH_TOAN, ref obj, ref listClientResponseDetail);
                luongCoBan = obj.OBJ_LUONG.LUONG_CO_BAN;

                return ret;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                return ret;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                listClientResponseDetail = null;
                processNhanSu = null;
            }
        }
        
        #endregion       

        private void numTyLeHuongLuong_LostFocus(object sender, RoutedEventArgs e)
        {
            if (numTyLeHuongLuong.Value <= 0 || numTyLeHuongLuong.Value > 100)
            {
                LMessage.ShowMessage("Tỷ lệ hưởng lương không hợp lệ", LMessage.MessageBoxType.Warning);
                return;
            }
        }

    }
}
