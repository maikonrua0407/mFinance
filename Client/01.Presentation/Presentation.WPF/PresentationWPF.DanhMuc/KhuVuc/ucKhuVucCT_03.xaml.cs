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
using Presentation.Process.DanhMucServiceRef;
using PresentationWPF.CustomControl;

namespace PresentationWPF.DanhMuc.KhuVuc
{
    /// <summary>
    /// Interaction logic for ucKhuVucCT_03.xaml
    /// </summary>
    public partial class ucKhuVucCT_03 : UserControl
    {
        #region Khai bao
        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private DatabaseConstant.Function function = DatabaseConstant.Function.DC_DM_CUM;

        public event EventHandler OnSavingCompleted;

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private DM_KHU_VUC obj;
        public DM_KHU_VUC Obj
        {
            get { return obj; }
            set { obj = value; }
        }

        List<DM_KHU_VUC_NS> lstNhanSu = null;

        int? idHuyen = null;
        int idCBQL = 0;
        string maCBQL = "";

        int idNguoiDD = 0;
        string maNguoiDD = "";
        string loaiHSo = "";
        string loaiQLChinh = "";

        private string sTrangThaiNVu = "";
        
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGiaoDich = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCBQL = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHuyen = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTinhTP = new List<AutoCompleteEntry>();

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        DataTable dtCanBoQLy;
        DataTable dtCanBoCTV;
        
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
        public ucKhuVucCT_03()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();            

            LoadCombobox();

            InitEventHandler();

            KhoiTaoDataTable();
        }        

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.DanhMuc;component/KhuVuc/ucKhuVucCT_03.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void InitEventHandler()
        {
            chkThemNhieuLan.Checked += new RoutedEventHandler(chkThemNhieuLan_Checked);

            cmbChiNhanh.SelectionChanged += new SelectionChangedEventHandler(cmbChiNhanh_SelectionChanged);
            cmbPhongGiaoDich.SelectionChanged += new SelectionChangedEventHandler(cmbPhongGiaoDich_SelectionChanged);
            cmbTinhTP.SelectionChanged += new SelectionChangedEventHandler(cmbTinhTP_SelectionChanged);
            //tlbAddCanBo.Click += new RoutedEventHandler(tlbAddCanBo_Click);
            //tlbAddCongTac.Click += new RoutedEventHandler(tlbAddCongTac_Click);
            //tlbDelCanBo.Click += new RoutedEventHandler(tlbDelCanBo_Click);
            //tlbDelCongTac.Click += new RoutedEventHandler(tlbDelCongTac_Click);
        }       

        private void LoadCombobox()
        {
            
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();

            //Chi nhánh
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);

            if (cmbChiNhanh.SelectedIndex >= 0)
            {                
                string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                string idChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.ElementAt(1);
                cmbPhongGiaoDich.Items.Clear();
                lstDieuKien.Clear();
                lstSourcePhongGiaoDich.Clear();
                lstDieuKien.Add(maChiNhanh);
                auto.GenAutoComboBox(ref lstSourcePhongGiaoDich, ref cmbPhongGiaoDich, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOMACHA.getValue(), lstDieuKien, ClientInformation.MaDonViGiaoDich);
            }

            auto = new AutoComboBox();
            cmbTinhTP.Items.Clear();
            lstDieuKien.Clear();
            auto.GenAutoComboBox(ref lstSourceTinhTP, ref cmbTinhTP, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TINHTP.getValue(), lstDieuKien);
        }

        private void KhoiTaoDataTable()
        {
            dtCanBoQLy = new DataTable();
            dtCanBoCTV = new DataTable();

            dtCanBoQLy.Columns.Add("ID_NS_HO_SO", typeof(int));
            dtCanBoQLy.Columns.Add("MA_NS_HO_SO", typeof(string));
            dtCanBoQLy.Columns.Add("TEN_HO_SO", typeof(string));
            dtCanBoQLy.Columns.Add("CHUC_VU", typeof(string));
            dtCanBoQLy.Columns.Add("MA_LOAI_HSO", typeof(string));
            dtCanBoQLy.Columns.Add("LOAI_QLY_CHINH", typeof(string));

            dtCanBoCTV.Columns.Add("ID_NS_HO_SO", typeof(int));
            dtCanBoCTV.Columns.Add("MA_NS_HO_SO", typeof(string));
            dtCanBoCTV.Columns.Add("TEN_HO_SO", typeof(string));
            dtCanBoCTV.Columns.Add("CHUC_VU", typeof(string));
            dtCanBoCTV.Columns.Add("MA_LOAI_HSO", typeof(string));
            dtCanBoCTV.Columns.Add("LOAI_QLY_CHINH", typeof(string));

            //raddgrCanBoQLy.ItemsSource = dtCanBoQLy;
            //raddgrCongTacVien.ItemsSource = dtCanBoCTV;
            //raddgrCanBoQLy.Rebind();
            //raddgrCongTacVien.Rebind();
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
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
            BeforeAddNew();
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
            
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
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
                
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                BeforeAddNew();
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
                
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                
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
                
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                BeforeAddNew();
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
                
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
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

            bool ret = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_CUM,
                DatabaseConstant.Table.DM_CUM,
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

        /// <summary>
        /// Sự kiện khi thay đổi giá trị của combobox cmbDonVi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbChiNhanh.SelectedIndex>=0)
            {
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();

                string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                string idChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.ElementAt(1);
                cmbPhongGiaoDich.Items.Clear();
                lstDieuKien.Clear();
                lstSourcePhongGiaoDich.Clear();
                lstDieuKien.Add(maChiNhanh);
                auto.GenAutoComboBox(ref lstSourcePhongGiaoDich, ref cmbPhongGiaoDich, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOMACHA.getValue(), lstDieuKien);        
            }
            else
            {
                cmbPhongGiaoDich.Items.Clear();
                lstSourcePhongGiaoDich.Clear();

                lstSourceCBQL.Clear();
            }
        }

        private void cmbPhongGiaoDich_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void cmbTinhTP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtHuyen.Text = "";
            idHuyen = null;
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

        private void chkNguoiDDLaKHTV_Checked(object sender, RoutedEventArgs e)
        {
            //raddgrCongTacVien.Items.Clear();
            //raddgrCongTacVien.Rebind();
            txtNguoiDD.Text = "";
        }

        private void chkNguoiDDLaKHTV_UnChecked(object sender, RoutedEventArgs e)
        {
            //raddgrCongTacVien.Items.Clear();
            //raddgrCongTacVien.Rebind();
            txtNguoiDD.Text = "";
        }

        //private void tlbDelCongTac_Click(object sender, RoutedEventArgs e)
        //{
        //    foreach (DataRow drv in raddgrCongTacVien.SelectedItems)
        //    {
        //        dtCanBoCTV.Rows.Remove(drv);
        //    }
        //    raddgrCongTacVien.ItemsSource = dtCanBoCTV;
        //    raddgrCongTacVien.Rebind();
        //}

        //private void tlbDelCanBo_Click(object sender, RoutedEventArgs e)
        //{
        //    foreach (DataRow drv in raddgrCanBoQLy.SelectedItems)
        //    {
        //        dtCanBoQLy.Rows.Remove(drv);
        //    }
        //    raddgrCanBoQLy.ItemsSource = dtCanBoQLy;
        //    raddgrCanBoQLy.Rebind();
        //}

        private void btnPopupNguoiDD_Click(object sender, RoutedEventArgs e)
        {
            ShowPopupNguoiDD();
        }

        private void ShowPopupNguoiDD()
        {
            if (chkNguoiDDLaKHTV.IsChecked == false)
            {
                List<string> lstDieuKien = new List<string>();
                string lstIDKH = "";
                for (int i = 0; i < dtCanBoCTV.Rows.Count; i++)
                {
                    lstIDKH += "," + dtCanBoCTV.Rows[i]["ID_NS_HO_SO"].ToString();
                }
                if (lstIDKH.Length > 0)
                    lstIDKH = "(" + lstIDKH.Substring(1) + ")";
                else
                    lstIDKH = "(0)";
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add(BusinessConstant.LOAI_HO_SO.CONG_TAC_VIEN.layGiaTri());
                lstDieuKien.Add(lstIDKH);
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_HOSO_NHAN_SU_CTV.getValue(), lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(false, simplePopupResponse, true);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup.Count > 0)
                {
                    foreach (DataRow dr in lstPopup)
                    {
                        DataRow row = lstPopup[0];
                        idNguoiDD = Convert.ToInt32(row[1]);
                        maNguoiDD = row[2].ToString();
                        loaiHSo = row[5].ToString(); ;
                        loaiQLChinh = row[6].ToString(); ;
                        txtNguoiDD.Text = row[3].ToString();
                    }                    
                }
            }
            else
            {
                List<string> lstDieuKien = new List<string>();

                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_KHTV_NGUOIDD", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(false, simplePopupResponse, true);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup.Count > 0)
                {
                    foreach (DataRow dr in lstPopup)
                    {
                        DataRow row = lstPopup[0];
                        idNguoiDD = Convert.ToInt32(row[1]);
                        maNguoiDD = row[2].ToString();
                        loaiHSo = "KHTV";
                        loaiQLChinh = "";
                        txtNguoiDD.Text = row[3].ToString();
                    }
                }
            }
        }

        private void btnPopupCBQL_Click(object sender, RoutedEventArgs e)
        {
            ShowPopupCBQL();
        }

        private void ShowPopupCBQL()
        {
            List<string> lstDieuKien = new List<string>();
            string lstIDKH = "";
            for (int i = 0; i < dtCanBoQLy.Rows.Count; i++)
            {
                lstIDKH += "," + dtCanBoQLy.Rows[i]["ID_NS_HO_SO"].ToString();
            }
            if (lstIDKH.Length > 0)
                lstIDKH = "(" + lstIDKH.Substring(1) + ")";
            else
                lstIDKH = "(0)";
            lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
            lstDieuKien.Add(BusinessConstant.LOAI_HO_SO.CHINH_THUC.layGiaTri());
            lstDieuKien.Add(lstIDKH);
            lstPopup = new List<DataRow>();
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_HOSO_NHAN_SU_QLY.getValue(), lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            ucPopup popup = new ucPopup(false, simplePopupResponse, true);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.Content = popup;
            win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ShowDialog();
            if (lstPopup != null && lstPopup.Count > 0)
            {
                foreach (DataRow dr in lstPopup)
                {
                    DataRow row = lstPopup[0];
                    idCBQL = Convert.ToInt32(row[1]);
                    maCBQL = row[2].ToString();
                    txtCBQL.Text = row[3].ToString();
                }
            }
        }

        private void btnPopupHuyen_Click(object sender, RoutedEventArgs e)
        {
            ShowPopupHuyen();
        }

        private void ShowPopupHuyen()
        {
            if (Convert.ToInt32(lstSourceTinhTP.ElementAt(cmbTinhTP.SelectedIndex).KeywordStrings.ElementAt(1)) > 0)
            {
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(lstSourceTinhTP.ElementAt(cmbTinhTP.SelectedIndex).KeywordStrings.ElementAt(1));
                var process = new PopupProcess();
                process.getPopupInformation("POPUP_DM_DIABAN_QUAN_HUYEN", lstDieuKien);

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
                    idHuyen = Convert.ToInt32(row[1]);
                    txtHuyen.Text = row[3].ToString();
                }

            }
            else
                LMessage.ShowMessage("Chưa chọn đơn vị", LMessage.MessageBoxType.Warning);
        }
        #endregion               

        #region Xử lý nghiệp vụ
        private void GetFormData(ref DM_KHU_VUC obj, ref List<DM_KHU_VUC_NS> lstNS, string sTrangThaiNVu)
        {
            try
            {
                obj = new DM_KHU_VUC();

                obj.ID = id;
                obj.ID_DVI = Convert.ToInt32(lstSourcePhongGiaoDich.ElementAt(cmbPhongGiaoDich.SelectedIndex).KeywordStrings.ElementAt(1));
                obj.MA_KVUC = txtMaKhuVuc.Text;
                obj.MA_DVI = lstSourcePhongGiaoDich.ElementAt(cmbPhongGiaoDich.SelectedIndex).KeywordStrings.First();
                obj.TEN_KVUC = txtTenKhuVuc.Text;
                obj.TEN_TAT = txtTenTat.Text;
                obj.NGAY_HOP = Convert.ToDateTime(raddtNgayHop.Value).ToString("yyyyMMdd");
                obj.ID_TINH = Convert.ToInt32(lstSourceTinhTP.ElementAt(cmbTinhTP.SelectedIndex).KeywordStrings.ElementAt(1));
                obj.ID_HUYEN = idHuyen;
                obj.ID_CBO_QLY = idCBQL;
                obj.MA_CBO_QLY = maCBQL;

                //Thông tin kiểm soát
                obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.TTHAI_NVU = sTrangThaiNVu;
                obj.MA_DVI_QLY = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                obj.MA_DVI_TAO = obj.MA_DVI;
                obj.NGAY_NHAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                obj.NGUOI_NHAP = txtNguoiLap.Text;
                if (action != DatabaseConstant.Action.THEM)
                {
                    obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }

                lstNhanSu = new List<DM_KHU_VUC_NS>();

                DM_KHU_VUC_NS objQLy = new DM_KHU_VUC_NS();
                objQLy.ID_KHU_VUC = id;
                objQLy.MA_KHU_VUC = txtMaKhuVuc.Text;
                objQLy.MA_LOAI_HSO = loaiHSo.ToString();
                objQLy.ID_NS_HO_SO = idNguoiDD.ToString().StringToInt32();
                objQLy.MA_NS_HO_SO = maNguoiDD.ToString();
                objQLy.LOAI_QLY_CHINH = loaiQLChinh.ToString();
                objQLy.MA_DVI_QLY = obj.MA_DVI_QLY;
                objQLy.MA_DVI_TAO = obj.MA_DVI_TAO;
                objQLy.NGAY_CNHAT = obj.NGAY_CNHAT;
                objQLy.NGAY_NHAP = obj.NGAY_NHAP;
                objQLy.NGUOI_CNHAT = obj.NGUOI_CNHAT;
                objQLy.NGUOI_NHAP = obj.NGUOI_NHAP;
                objQLy.TTHAI_BGHI = obj.TTHAI_BGHI;
                objQLy.TTHAI_NVU = obj.TTHAI_NVU;
                lstNhanSu.Add(objQLy);

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            DanhMucProcess processDanhMuc = new DanhMucProcess();
            NhanSuProcess processNhanSu = new NhanSuProcess();
            KhachHangProcess processKhachHang = new KhachHangProcess();  

            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            obj = new DM_KHU_VUC();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processDanhMuc.KhuVuc02(DatabaseConstant.Action.LOAD, ref obj, ref listClientResponseDetail);
                DataSet ds = new DanhMucProcess().getThongTinNguoiDaiDien(obj.ID.ToString());                

                if (ret == true)
                {
                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    #region Thông tin chung
                    cmbChiNhanh.SelectedIndex = lstSourceChiNhanh.IndexOf(lstSourceChiNhanh.FirstOrDefault(e => e.KeywordStrings.First().Equals(obj.MA_DVI_QLY)));
                    cmbPhongGiaoDich.SelectedIndex = lstSourcePhongGiaoDich.IndexOf(lstSourcePhongGiaoDich.FirstOrDefault(e => e.KeywordStrings.First().Equals(obj.MA_DVI)));
                    txtMaKhuVuc.Text = obj.MA_KVUC;
                    txtTenKhuVuc.Text = obj.TEN_KVUC;
                    txtTenTat.Text = obj.TEN_TAT;

                    if(!obj.ID_TINH.IsNullOrEmpty() && obj.ID_TINH != 0)
                        cmbTinhTP.SelectedIndex = lstSourceTinhTP.IndexOf(lstSourceTinhTP.FirstOrDefault(e => e.KeywordStrings[1] == obj.ID_TINH.ToString()));

                    if (!obj.ID_HUYEN.IsNullOrEmpty() && obj.ID_HUYEN != 0)
                    {
                        txtHuyen.Text = processDanhMuc.getDiaBanById(obj.ID_HUYEN.Value).TEN_DBAN;
                        idHuyen = obj.ID_HUYEN;
                    }
                    if (!obj.ID_CBO_QLY.IsNullOrEmpty() && obj.ID_CBO_QLY != 0)
                    {
                        txtCBQL.Text = processNhanSu.getThongTinNhanSuTheoID(obj.ID_CBO_QLY.Value).Tables[0].Rows[0]["TEN_HSO"].ToString();
                        idCBQL = obj.ID_CBO_QLY.Value;
                        maCBQL = obj.MA_CBO_QLY;
                    }
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        chkNguoiDDLaKHTV.IsChecked = ds.Tables[0].Rows[0]["MA_LOAI_HSO"].ToString().Equals("KHTV");
                        if (chkNguoiDDLaKHTV.IsChecked == true)
                        {
                            txtNguoiDD.Text = processKhachHang.getThongTinKHTheoID(
                                Convert.ToInt32(ds.Tables[0].Rows[0]["ID_NS_HO_SO"])).Tables[0].Rows[0]["TEN_KHANG"].ToString();                            
                        }
                        else
                        {
                            txtNguoiDD.Text = processNhanSu.getThongTinNhanSuTheoID(
                                obj.ID_CBO_QLY.Value).Tables[0].Rows[0]["TEN_HSO"].ToString();
                        }
                        idNguoiDD = Convert.ToInt32(ds.Tables[0].Rows[0]["ID_NS_HO_SO"]);
                        maNguoiDD = ds.Tables[0].Rows[0]["MA_NS_HO_SO"].ToString();
                        loaiHSo = ds.Tables[0].Rows[0]["MA_LOAI_HSO"].ToString();
                        loaiQLChinh = ds.Tables[0].Rows[0]["LOAI_QLY_CHINH"].ToString();
                    }

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
            id = 0;
            obj = null;

            #region Thông tin chung            
            txtMaKhuVuc.Text = "";
            txtTenKhuVuc.Text = "";
            txtTenTat.Text = "";
            cmbTinhTP.SelectedIndex = 0;
            txtHuyen.Text = "";
            txtCBQL.Text = "";
            txtNguoiDD.Text = "";
            raddtNgayHop.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            #endregion

            #region Thông tin kiểm soát
            txtTrangThaiBanGhi.Text = "";
            raddtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            raddtNgayCapNhat.Value = null;
            txtNguoiCapNhat.Text = "";
            #endregion
            
        }

        private bool Validation()
        {
            try
            {
                if (cmbTinhTP.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblTinhTP.Content.ToString());
                    cmbTinhTP.Focus();
                    return false;
                }

                if (cmbChiNhanh.SelectedIndex<0)
                {
                    CommonFunction.ThongBaoChuaChon(lblChiNhanh.Content.ToString());
                    cmbChiNhanh.Focus();
                    return false;
                }

                if (cmbPhongGiaoDich.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblPhongGiaoDich.Content.ToString());
                    cmbPhongGiaoDich.Focus();
                    return false;
                }

                if (txtHuyen.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaChon(lblHuyen.Content.ToString());
                    txtHuyen.Focus();
                    return false;
                }

                if (txtTenKhuVuc.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaChon(lblTenKhuVuc.Content.ToString());
                    txtTenKhuVuc.Focus();
                    return false;
                }

                if (txtTenTat.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaChon(lblTenTat.Content.ToString());
                    txtTenTat.Focus();
                    return false;
                }

                if (txtCBQL.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaChon(lblCBQL.Content.ToString());
                    txtTenTat.Focus();
                    return false;
                }

                if (txtNguoiDD.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaChon(lblNguoiDD.Content.ToString());
                    txtTenTat.Focus();
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
                cmbChiNhanh.IsEnabled = false;
                cmbPhongGiaoDich.IsEnabled = false;
                txtMaKhuVuc.IsEnabled = false;
                txtTenKhuVuc.IsEnabled = true;
                txtTenTat.IsEnabled = true;
                cmbTinhTP.IsEnabled = true;
                txtHuyen.IsEnabled = true;
                btnPopupHuyen.IsEnabled = true;
                txtCBQL.IsEnabled = true;
                btnPopupCBQL.IsEnabled = true;
                txtNguoiDD.IsEnabled = true;
                btnPopupNguoiDD.IsEnabled = true;
                raddtNgayHop.IsEnabled = true;
                chkNguoiDDLaKHTV.IsEnabled = true;
                dpNgayHop.IsEnabled = true;
            }
            #endregion

            #region Sửa
            else if (action == DatabaseConstant.Action.SUA)
            {
                cmbChiNhanh.IsEnabled = false;
                cmbPhongGiaoDich.IsEnabled = false;
                txtMaKhuVuc.IsEnabled = false;
                txtTenKhuVuc.IsEnabled = true;
                txtTenTat.IsEnabled = true;
                cmbTinhTP.IsEnabled = true;
                txtHuyen.IsEnabled = true;
                btnPopupHuyen.IsEnabled = true;
                txtCBQL.IsEnabled = true;
                btnPopupCBQL.IsEnabled = true;
                txtNguoiDD.IsEnabled = true;
                btnPopupNguoiDD.IsEnabled = true;
                raddtNgayHop.IsEnabled = true;
                chkNguoiDDLaKHTV.IsEnabled = true;
                dpNgayHop.IsEnabled = true;
            }
            #endregion

            #region Xem ~ Mặc định
            else
            {
                cmbChiNhanh.IsEnabled = false;
                cmbPhongGiaoDich.IsEnabled = false;
                txtMaKhuVuc.IsEnabled = false;
                txtTenKhuVuc.IsEnabled = false;
                txtTenTat.IsEnabled = false;
                cmbTinhTP.IsEnabled = false;
                txtHuyen.IsEnabled = false;
                btnPopupHuyen.IsEnabled = false;
                txtCBQL.IsEnabled = false;
                btnPopupCBQL.IsEnabled = false;
                txtNguoiDD.IsEnabled = false;
                btnPopupNguoiDD.IsEnabled = false;
                raddtNgayHop.IsEnabled = false;
                chkNguoiDDLaKHTV.IsEnabled = false;
                dpNgayHop.IsEnabled = false;
            }
            #endregion
        }


        public void OnSave()
        {                    
            try
            {
                if (!Validation()) return;

                string trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();

                obj = new DM_KHU_VUC();

                GetFormData(ref obj, ref lstNhanSu, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(obj, lstNhanSu);
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(obj, lstNhanSu);
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
            txtTenKhuVuc.Focus();
        }

        public void OnAddNew(DM_KHU_VUC obj, List<DM_KHU_VUC_NS> lstNS)
        {
            
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                DanhMucProcess processDanhMuc = new DanhMucProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                obj = processDanhMuc.ThemKhuVuc(obj, ref listClientResponseDetail, lstNhanSu);
                if (obj.ID > 0)
                    ret = true;
                    //ret = processDanhMuc.KhuVuc02(DatabaseConstant.Action.THEM, ref obj, ref listClientResponseDetail);
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

        public void AfterAddNew(bool ret, DM_KHU_VUC obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                    if (chkThemNhieuLan.IsChecked == true)
                    {
                        BeforeAddNew();
                    }
                    else
                    {
                        id = obj.ID;
                        txtMaKhuVuc.Text = obj.MA_KVUC;

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

                bool ret = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_CUM,
                    DatabaseConstant.Table.DM_CUM,
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

        public void OnModify(DM_KHU_VUC obj, List<DM_KHU_VUC_NS> lstNS)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                DanhMucProcess processDanhMuc = new DanhMucProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;
                obj = new DanhMucProcess().SuaKhuVuc(obj, ref listClientResponseDetail, lstNhanSu);
                if (obj.ID > 0)
                    ret = true;
                //ret = processDanhMuc.KhuVuc02(DatabaseConstant.Action.SUA, ref obj, ref listClientResponseDetail);
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

        public void AfterModify(bool ret, DM_KHU_VUC obj, List<ClientResponseDetail> listClientResponseDetail)
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_CUM,
                    DatabaseConstant.Table.DM_CUM,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_DM_CUM,
                        DatabaseConstant.Table.DM_CUM,
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_CUM,
                    DatabaseConstant.Table.DM_CUM,
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
            DanhMucProcess processDanhMuc = new DanhMucProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret =  new DanhMucProcess().XoaKhuVuc(new int[] { obj.ID }, ref listClientResponseDetail);
                //ret = processDanhMuc.KhuVuc02(action, ref obj, ref listClientResponseDetail);
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
                processDanhMuc = null;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_CUM,
                    DatabaseConstant.Table.DM_CUM,
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
        
        #endregion              
    }
}
