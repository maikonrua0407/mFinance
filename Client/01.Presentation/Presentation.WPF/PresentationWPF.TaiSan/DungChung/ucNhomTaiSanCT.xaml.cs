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
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using PresentationWPF.CustomControl;
using System.Reflection;
using Presentation.Process;
using System.Data;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.TaiSanServiceRef;

namespace PresentationWPF.TaiSan.DungChung
{
    /// <summary>
    /// Interaction logic for ucNhomTaiSanCT.xaml
    /// </summary>
    public partial class ucNhomTaiSanCT : UserControl
    {
        #region Khai bao
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Function function = DatabaseConstant.Function.TS_DM_NHOM_TS_CT;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }
        public event EventHandler OnSavingCompleted;
        private string sTrangThaiNVu = "";
        List<AutoCompleteEntry> lstSourceLoaiTaiSan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNhomTSCha = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhanLoaiTS = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDonViHachToan = new List<AutoCompleteEntry>();
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
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        private TS_DM_NHOM_TSCD _objNhomTS = new TS_DM_NHOM_TSCD();
        DataSet _dsTaiKhoan = null;
        #endregion

        #region Khoi tao
        public ucNhomTaiSanCT()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            KhoiTaoComboBox();

            InitEventHandler();
        }        

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TaiSan;component/DungChung/ucNhomTaiSanCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
        }
        /// <summary>
        /// Khoi tao combobox
        /// </summary>
        private void KhoiTaoComboBox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();

            //Khoi tao combobox loai tai san
            lstSourceLoaiTaiSan.Clear();
            cmbLoaiTaiSan.Items.Clear();
            auto.GenAutoComboBox(ref lstSourceLoaiTaiSan, ref cmbLoaiTaiSan, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TS_LOAI_TS.getValue());
            
            //Khoi tao combobox phan loai tai san
            lstSourcePhanLoaiTS.Clear();
            cmbPhanLoaiTS.Items.Clear();
            auto.GenAutoComboBox(ref lstSourcePhanLoaiTS, ref cmbPhanLoaiTS, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TS_PHAN_LOAI_TS.getValue());

            //Khoi tao combobox nhom cha
            //lstSourceNhomTSCha.Clear();
            //cmbNhomCha.Items.Clear();
            //auto.GenAutoComboBox(ref lstSourceNhomTSCha, ref cmbNhomCha, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TS_NHOM_CHA.getValue());
            //KhoiTaoComBoNhomCha();

            //Khoi tao combobox don vi hach toan bao so
            lstSourceDonViHachToan.Clear();
            cmbDViHachToan.Items.Clear();
            auto.GenAutoComboBox(ref lstSourceDonViHachToan, ref cmbDViHachToan, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DVI_HTOAN.getValue());
        }

        private void KhoiTaoComBoNhomCha()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            string sMaNhomCha = "ABCXYZ";
            string sMaPLoai = lstSourcePhanLoaiTS.ElementAt(cmbPhanLoaiTS.SelectedIndex).KeywordStrings[0];
            if (!LObject.IsNullOrEmpty(_objNhomTS.MA_NHOM))
            {
                sMaNhomCha = _objNhomTS.MA_NHOM;
            }
            lstDieuKien.Add(sMaNhomCha);
            lstDieuKien.Add(sMaPLoai);            
            lstSourceNhomTSCha.Clear();
            cmbNhomCha.Items.Clear();
            auto.GenAutoComboBox(ref lstSourceNhomTSCha, ref cmbNhomCha, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TS_NHOM_CHA.getValue(), lstDieuKien);
        }

        /// <summary>
        /// Lay danh sach tai khoan hach toan
        /// </summary>
        private void LoadDSTaiKhoan(string sMaNhomTS)
        {
            try
            {
                NhomTaiSanProcess process = new NhomTaiSanProcess();
                _dsTaiKhoan = process.GetDSTaiKhoanHachToan(sMaNhomTS);
                if (_dsTaiKhoan != null && _dsTaiKhoan.Tables[0].Rows.Count > 0)
                {
                    grdTaiKhoan.DataContext = _dsTaiKhoan.Tables[0].DefaultView;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(MethodBase.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                CommonFunction.ThongBaoLoi(ex);
            }
        }

        /// <summary>
        /// An hien cot du lieu theo loai don vi hach toan
        /// </summary>
        private void HideColumn()
        {
            if (cmbDViHachToan.SelectedItem != null)
            {
                AutoCompleteEntry auto = (AutoCompleteEntry)cmbDViHachToan.SelectedItem;

                if (auto.KeywordStrings != null && auto.KeywordStrings.Length > 0)
                {
                    if (auto.KeywordStrings[0].Equals("DOC_LAP"))
                    {
                        grdMaPLoai.IsVisible = true;
                        grdMaPLoaiBSo.IsVisible = false;
                        grdTenPLoai.IsVisible = true;
                        grdTenPLoaiBSo.IsVisible = false;
                    }
                    else
                    {
                        grdMaPLoai.IsVisible = false;
                        grdMaPLoaiBSo.IsVisible = true;
                        grdTenPLoai.IsVisible = false;
                        grdTenPLoaiBSo.IsVisible = true;
                    }
                }

            }
        }

        private void cmbDViHachToan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HideColumn();

        }

        private void InitEventHandler()
        {
            cmbDViHachToan.SelectionChanged += new SelectionChangedEventHandler(cmbDViHachToan_SelectionChanged);
            cmbPhanLoaiTS.SelectionChanged+=new SelectionChangedEventHandler(cmbPhanLoaiTS_SelectionChanged);
            cmbNhomCha.SelectionChanged+=new SelectionChangedEventHandler(cmbNhomCha_SelectionChanged);            
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Escape, ModifierKeys.None);
                        key = new KeyBinding(CloseCommand, keyg);
                        key.Gesture = keyg;
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

            bool ret = process.UnlockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_DM_NHOM_TS_CT,
                DatabaseConstant.Table.TS_DM_NHOM_TSCD,
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

        private void btnChon_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            GridViewRow grdRow = btn.ParentOfType<GridViewRow>();
            LayPhanLoaiTaiKhoan(grdRow);
        }

        private void LayPhanLoaiTaiKhoan(GridViewRow grdRow)
        {
            try
            {
                DataRowView drView = grdRow.Item as DataRowView;
                string MaKyhieu = drView["MA_KY_HIEU"].ToString();
                string MaPhanLoai = "%";
                string LoaiHachToan = lstSourceDonViHachToan.ElementAt(cmbDViHachToan.SelectedIndex).KeywordStrings.FirstOrDefault();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(MaKyhieu);
                lstDieuKien.Add(MaPhanLoai);
                lstDieuKien.Add(ClientInformation.MaDonVi);
                var process = new PopupProcess();
                process.getPopupInformation("POPUP_TAI_KHOAN_HACH_TOAN", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                lstPopup.Clear();
                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow row = lstPopup[0];
                    if (LoaiHachToan.Equals(DatabaseConstant.PHUONG_PHAP_HACH_TOAN.DOC_LAP.layGiaTri()))
                    {
                        drView["MA_PLOAI"] = row[2].ToString();
                        drView["TEN_PLOAI"] = row[3].ToString();
                    }
                    else
                    {
                        drView["MA_PLOAI_BSO"] = row[2].ToString();
                        drView["TEN_PLOAI_BSO"] = row[3].ToString();
                    }
                    grdTaiKhoan.CurrentItem = drView;
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void cmbNhomCha_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idNhomCha = 0;
            int iRet = 0;
            TS_DM_NHOM_TSCD objNhomTSCha = new TS_DM_NHOM_TSCD();
            TaiSanProcess process = new TaiSanProcess();
            if (lstSourceNhomTSCha.Count > 0)
            {
                if (!lstSourceNhomTSCha.ElementAt(cmbNhomCha.SelectedIndex).KeywordStrings[0].Equals(""))
                {                    
                    idNhomCha = Convert.ToInt32(lstSourceNhomTSCha.ElementAt(cmbNhomCha.SelectedIndex).KeywordStrings[1]);
                    objNhomTSCha.ID = idNhomCha;
                    iRet = process.GetNhomTSanCha(ref objNhomTSCha);
                    //cmbPhanLoaiTS.IsEnabled = false;
                    cmbLoaiTaiSan.IsEnabled = false;
                    if (iRet != 0)
                    {
                        cmbPhanLoaiTS.SelectedIndex = lstSourcePhanLoaiTS.IndexOf(lstSourcePhanLoaiTS.FirstOrDefault(i => i.KeywordStrings[0].Equals(objNhomTSCha.PHAN_LOAI)));
                        cmbLoaiTaiSan.SelectedIndex = lstSourceLoaiTaiSan.IndexOf(lstSourceLoaiTaiSan.FirstOrDefault(i => i.KeywordStrings[0].Equals(objNhomTSCha.LOAI_TAI_SAN)));
                    }
                }
                else
                {
                    cmbLoaiTaiSan.IsEnabled = true;
                }
            }
            else
            {
                cmbPhanLoaiTS.IsEnabled = true;
            }
        }

        private void cmbPhanLoaiTS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            KhoiTaoComBoNhomCha();
        }


        void ClearForm()
        {
            cmbLoaiTaiSan.SelectedIndex = 0;
            cmbNhomCha.SelectedIndex = 0;
            cmbPhanLoaiTS.SelectedIndex = 0;
            txtMaNhomTaiSan.Text = string.Empty;
            txtTenNhomTaiSan.Text = string.Empty;
            txtThangTu.Value = 0;
            txtThangDen.Value = 0;
            Action = DatabaseConstant.Action.THEM;
            CommonFunction.RefreshButton(Toolbar, Action, sTrangThaiNVu, mnuMain, DatabaseConstant.Function.TS_BAN_GIAO);
        }

        #endregion               

        #region Xử lý nghiệp vụ
        private void GetFormData(ref TS_DM_NHOM_TSCD objNhomTSCD, ref List<KT_PHAN_HE_PLOAI> lstPHePLoai, string sTrangThai)
        {
            try
            {
                #region Lay nhom tai san
                objNhomTSCD = new TS_DM_NHOM_TSCD();
                objNhomTSCD.ID = id;
                if (cmbNhomCha.SelectedIndex != -1)
                {
                    if (Convert.ToInt32(lstSourceNhomTSCha.ElementAt(cmbNhomCha.SelectedIndex).KeywordStrings[1]) != 0)
                    {
                        objNhomTSCD.ID_NHOM_CHA = Convert.ToInt32(lstSourceNhomTSCha.ElementAt(cmbNhomCha.SelectedIndex).KeywordStrings[1]);
                    }
                }
                objNhomTSCD.KHAU_HAO_DEN = Convert.ToInt32(txtThangDen.Value);
                objNhomTSCD.KHAU_HAO_TU = Convert.ToInt32(txtThangTu.Value);
                objNhomTSCD.LOAI_TAI_SAN = lstSourceLoaiTaiSan.ElementAt(cmbLoaiTaiSan.SelectedIndex).KeywordStrings.FirstOrDefault();
                objNhomTSCD.MA_DVI_QLY = ClientInformation.MaDonVi;
                objNhomTSCD.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                objNhomTSCD.MA_NHOM = txtMaNhomTaiSan.Text.Trim();
                objNhomTSCD.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                objNhomTSCD.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                objNhomTSCD.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                objNhomTSCD.NGUOI_NHAP = ClientInformation.TenDangNhap;
                objNhomTSCD.PHAN_LOAI = lstSourcePhanLoaiTS.ElementAt(cmbPhanLoaiTS.SelectedIndex).KeywordStrings.FirstOrDefault();
                objNhomTSCD.TEN_NHOM = txtTenNhomTaiSan.Text.Trim();
                objNhomTSCD.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                objNhomTSCD.TTHAI_NVU = sTrangThai;
                #endregion
                #region Lay danh sach tai khoan hach toan
                DataView dv = (DataView)grdTaiKhoan.ItemsSource;
                KT_PHAN_HE_PLOAI objPLoai = new KT_PHAN_HE_PLOAI();
                foreach (DataRowView drv in dv)
                {
                    objPLoai = new KT_PHAN_HE_PLOAI();
                    objPLoai.ID_PHAN_HE = 0;
                    objPLoai.MA_DTUONG = objNhomTSCD.MA_NHOM;
                    objPLoai.MA_DVI_QLY = ClientInformation.MaDonVi;
                    objPLoai.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    objPLoai.MA_KY_HIEU = drv["MA_KY_HIEU"].ToString();
                    objPLoai.MA_PHAN_HE = DatabaseConstant.Module.QLTS.getValue();
                    objPLoai.MA_PLOAI = drv["MA_PLOAI"].ToString();
                    objPLoai.MA_PLOAI_BSO = drv["MA_PLOAI_BSO"].ToString();
                    objPLoai.NGAY_CNHAT = objPLoai.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    objPLoai.NGUOI_CNHAT = objPLoai.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    objPLoai.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    objPLoai.TTHAI_NVU = sTrangThai;
                    lstPHePLoai.Add(objPLoai);
                }
                #endregion
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                CommonFunction.ThongBaoLoi(ex);
            }
        }

        private void SetFormData()
        {
            TaiSanProcess process = new TaiSanProcess();
            List<ClientResponseDetail> listClientResponse = new List<ClientResponseDetail>();            
            List<KT_PHAN_HE_PLOAI> lstPLoai = null;
            try
            {
                bool bRet = false;                
                _objNhomTS.ID = id;
                bRet = process.NhomTaiSanCT(DatabaseConstant.Action.LOAD, ref _objNhomTS, ref lstPLoai, "", ref listClientResponse);                
                if (bRet)
                {
                    if (_objNhomTS != null)
                    {
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(_objNhomTS.TTHAI_NVU);

                        #region Thong tin nhom tai san                        
                        sTrangThaiNVu = _objNhomTS.TTHAI_NVU;
                        txtMaNhomTaiSan.Text = _objNhomTS.MA_NHOM;
                        txtTenNhomTaiSan.Text = _objNhomTS.TEN_NHOM;
                        txtThangDen.Value = _objNhomTS.KHAU_HAO_DEN;
                        txtThangTu.Value = _objNhomTS.KHAU_HAO_TU;

                        cmbLoaiTaiSan.SelectedIndex = lstSourceLoaiTaiSan.IndexOf(lstSourceLoaiTaiSan.FirstOrDefault(i => i.KeywordStrings[0].Equals(_objNhomTS.LOAI_TAI_SAN)));
                        cmbPhanLoaiTS.SelectedIndex = lstSourcePhanLoaiTS.IndexOf(lstSourcePhanLoaiTS.FirstOrDefault(i => i.KeywordStrings[0].Equals(_objNhomTS.PHAN_LOAI)));
                        if (lstSourceNhomTSCha.Count > 0 && Convert.ToInt32(_objNhomTS.ID_NHOM_CHA)>0)
                        {
                            cmbNhomCha.SelectedIndex = lstSourceNhomTSCha.IndexOf(lstSourceNhomTSCha.FirstOrDefault(i => i.KeywordStrings[1].Equals(_objNhomTS.ID_NHOM_CHA.ToString())));
                        }                        
                        LoadDSTaiKhoan(_objNhomTS.MA_NHOM);
                        #endregion

                        #region Thông tin kiểm soát
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(_objNhomTS.TTHAI_BGHI);
                        raddtNgayLap.Value = LDateTime.StringToDate(_objNhomTS.NGAY_NHAP, "yyyyMMdd");
                        txtNguoiLap.Text = _objNhomTS.NGUOI_NHAP;
                        if (LDateTime.IsDate(_objNhomTS.NGAY_CNHAT, "yyyyMMdd") == true)
                            raddtNgayCapNhat.Value = LDateTime.StringToDate(_objNhomTS.NGAY_CNHAT, "yyyyMMdd");
                        txtNguoiCapNhat.Text = _objNhomTS.NGUOI_CNHAT;
                        #endregion
                    }
                }
                else
                {
                    LMessage.ShowMessage("M_ResponseMessage_HanhDong_LayDuLieuKhongThanhCong", LMessage.MessageBoxType.Warning);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
                listClientResponse = null;                
                lstPLoai = null;
            }
        }

        private void LoadFormData()
        {
            TaiSanProcess process = new TaiSanProcess();                      
            try
            {
                
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                process = null;
            }
        }

        private void ResetForm()
        {
            lblTrangThai.Content = "";
            txtMaNhomTaiSan.Text = "";
            txtTenNhomTaiSan.Text = "";
            txtThangDen.Value = 0;
            txtThangTu.Value = 0;
            cmbDViHachToan.SelectedIndex = 0;
            cmbLoaiTaiSan.SelectedIndex = 0;
            cmbPhanLoaiTS.SelectedIndex = 0;
            id = 0;

            //Lay lai tai khoan mac dinh
            LoadDSTaiKhoan("");

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
            sTrangThaiNVu = "";

            ResetForm();
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
            cmbLoaiTaiSan.Focus();

            cbMultiAdd.IsChecked = false;
        }

        private bool Validation()
        {
            try
            {
                int iThangTu = Convert.ToInt32(txtThangTu.Value);
                int iThangDen = Convert.ToInt32(txtThangDen.Value);
                string sNhomCha = "";
                if (cmbNhomCha.SelectedIndex != 1)
                {
                    sNhomCha = lstSourceNhomTSCha.ElementAt(cmbNhomCha.SelectedIndex).KeywordStrings[0];
                }
                if (txtTenNhomTaiSan.Text.Trim().Equals(""))
                {
                    CommonFunction.ThongBaoTrong(lblTenNhomTaiSan.Content.ToString());
                    txtTenNhomTaiSan.Focus();
                    return false;
                }
                else if (_dsTaiKhoan == null || _dsTaiKhoan.Tables[0].Rows.Count == 0)
                {
                    LMessage.ShowMessage("M.TaiSan.DungChung.ucNhomTaiSanCT.LoiKhongCoTaiKhoanHachToan", LMessage.MessageBoxType.Warning);
                    return false;
                }
                else if (iThangTu == 0)
                {
                    LMessage.ShowMessage("M.TaiSan.DungChung.ucNhomTaiSanCT.LoiKhongNhapTGianKhauHaoTu", LMessage.MessageBoxType.Warning);
                    txtThangTu.Focus();
                    return false;
                }
                else if (iThangDen == 0)
                {
                    LMessage.ShowMessage("M.TaiSan.DungChung.ucNhomTaiSanCT.LoiKhongNhapTGianKhauHaoDen", LMessage.MessageBoxType.Warning);
                    txtThangDen.Focus();
                    return false;
                }
                else if (iThangDen < iThangTu)
                {
                    LMessage.ShowMessage("M.TaiSan.DungChung.ucNhomTaiSanCT.LoiTGianKhauHaoTuLonHonTGianDen", LMessage.MessageBoxType.Warning);
                    txtThangTu.Focus();
                    return false;
                }
                else if (_objNhomTS!=null && _objNhomTS!=null && _objNhomTS.MA_NHOM!="" )
                {
                    if (sNhomCha.Equals(_objNhomTS.MA_NHOM))
                    {
                        LMessage.ShowMessage("M.TaiSan.DungChung.ucNhomTaiSanCT.LoiNhomChaTrungNhomCon", LMessage.MessageBoxType.Warning);
                        return false;
                    }
                    else return true;
                }
                else return true;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                return false;
            }
        }

        private void SetEnabledControls()
        {
            if (action == DatabaseConstant.Action.THEM)
            {
                txtMaNhomTaiSan.IsEnabled = false;
                txtTenNhomTaiSan.IsEnabled = true;
                txtThangDen.IsEnabled = true;
                txtThangTu.IsEnabled = true;
                cmbLoaiTaiSan.IsEnabled = true;
                cmbNhomCha.IsEnabled = true;
                cmbPhanLoaiTS.IsEnabled = true;
                cmbDViHachToan.IsEnabled = true;
                grdTaiKhoan.IsEnabled = true;
            }
            else if (action == DatabaseConstant.Action.SUA)
            {
                txtMaNhomTaiSan.IsEnabled = false;
                txtTenNhomTaiSan.IsEnabled = true;
                txtThangDen.IsEnabled = true;
                txtThangTu.IsEnabled = true;
                cmbLoaiTaiSan.IsEnabled = true;
                cmbNhomCha.IsEnabled = true;
                cmbPhanLoaiTS.IsEnabled = true;
                cmbDViHachToan.IsEnabled = true;
                grdTaiKhoan.IsEnabled = true;
            }
            else
            {
                txtMaNhomTaiSan.IsEnabled = false;
                txtTenNhomTaiSan.IsEnabled = false;
                txtThangDen.IsEnabled = false;
                txtThangTu.IsEnabled = false;
                cmbLoaiTaiSan.IsEnabled = false;
                cmbNhomCha.IsEnabled = false;
                cmbPhanLoaiTS.IsEnabled = false;
                cmbDViHachToan.IsEnabled = false;
                grdTaiKhoan.IsEnabled = false;
            }
        }


        public void OnHold()
        {
            try
            {
                if (!Validation()) return;
                TS_DM_NHOM_TSCD objNhomTS = new TS_DM_NHOM_TSCD();
                List<KT_PHAN_HE_PLOAI> lstPLoai = new List<KT_PHAN_HE_PLOAI>();
                string sMaNhomCha = "";
                if (cmbNhomCha.SelectedIndex != -1)
                {
                    sMaNhomCha = lstSourceNhomTSCha.ElementAt(cmbNhomCha.SelectedIndex).KeywordStrings.FirstOrDefault();
                }
                string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));
                GetFormData(ref objNhomTS, ref lstPLoai, trangThai);
                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(objNhomTS, lstPLoai, sMaNhomCha);
                }
                else
                {
                    OnModify(objNhomTS, lstPLoai);
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
                _objNhomTS = new TS_DM_NHOM_TSCD();
                List<KT_PHAN_HE_PLOAI> lstPLoai = new List<KT_PHAN_HE_PLOAI>();
                string sMaNhomCha = "";
                if (cmbNhomCha.SelectedIndex != -1)
                {
                    if(Convert.ToInt32(lstSourceNhomTSCha.ElementAt(cmbNhomCha.SelectedIndex).KeywordStrings[1])!=0)
                    {
                        sMaNhomCha = lstSourceNhomTSCha.ElementAt(cmbNhomCha.SelectedIndex).KeywordStrings.FirstOrDefault();
                    }
                }
                //string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));
                string trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();                    
                GetFormData(ref _objNhomTS, ref lstPLoai, trangThai);
                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(_objNhomTS, lstPLoai, sMaNhomCha);
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(_objNhomTS, lstPLoai);
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
            LoadFormData();
            action = DatabaseConstant.Action.THEM;
            ResetForm();
            SetEnabledControls();            
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain,function);
        }

        public void OnAddNew(TS_DM_NHOM_TSCD objNhomTS, List<KT_PHAN_HE_PLOAI> lstPloai, string sMaNhomCha)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TaiSanProcess process = new TaiSanProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool bRet = false;
                bRet = process.NhomTaiSanCT(action, ref objNhomTS, ref lstPloai, sMaNhomCha, ref listClientResponseDetail);
                AfterAddNew(bRet, objNhomTS, listClientResponseDetail);
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

        public void AfterAddNew(bool bRet, TS_DM_NHOM_TSCD objNhomTS, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (bRet)
                {
                    if (cbMultiAdd.IsChecked == true)
                    {
                        ResetData();
                    }
                    else
                    {
                        LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);
                        id = objNhomTS.ID;
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(objNhomTS.TTHAI_BGHI);
                        txtMaNhomTaiSan.Text = objNhomTS.MA_NHOM;
                        sTrangThaiNVu = objNhomTS.TTHAI_NVU;
                        BeforeViewFromDetail();
                        //if (cbMultiAdd.IsChecked == true)
                        //    ClearForm();
                        //else
                        //    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, sTrangThaiNVu, mnuMain, DatabaseConstant.Function.TS_BAN_GIAO);
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

                bool ret = process.LockData(DatabaseConstant.Module.QLTS,
                    DatabaseConstant.Function.TS_DM_NHOM_TS_CT,
                    DatabaseConstant.Table.TS_DM_NHOM_TSCD,
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

        public void OnModify(TS_DM_NHOM_TSCD objNhomTS, List<KT_PHAN_HE_PLOAI> lstPLoai)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TaiSanProcess process = new TaiSanProcess();
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                GetFormData(ref objNhomTS, ref lstPLoai, sTrangThaiNVu);
                bool bRet = process.NhomTaiSanCT(action, ref objNhomTS, ref lstPLoai, "", ref listResponseDetail);
                AfterModify(bRet, ref objNhomTS, listResponseDetail);
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

        public void AfterModify(bool bRet, ref TS_DM_NHOM_TSCD objNhomTS, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (bRet)
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);
                    sTrangThaiNVu = objNhomTS.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(objNhomTS.TTHAI_BGHI);
                    raddtNgayCapNhat.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                    txtNguoiCapNhat.Text = ClientInformation.TenDangNhap;
                    BeforeViewFromDetail();
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.QLTS,
                        DatabaseConstant.Function.TS_DM_NHOM_TS_CT,
                        DatabaseConstant.Table.TS_DM_NHOM_TSCD,
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                    DatabaseConstant.Function.TS_DM_NHOM_TS_CT,
                    DatabaseConstant.Table.TS_DM_NHOM_TSCD,
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
            TaiSanProcess process = new TaiSanProcess();
            List<KT_PHAN_HE_PLOAI> lstPLoai = new List<KT_PHAN_HE_PLOAI>();
            try
            {
                bool ret = false;
                _objNhomTS.ID = id;
                ret = process.NhomTaiSanCT(action, ref _objNhomTS, ref lstPLoai, "", ref listClientResponseDetail);
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
                process = null;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                    DatabaseConstant.Function.TS_DM_NHOM_TS_CT,
                    DatabaseConstant.Table.TS_DM_NHOM_TSCD,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.QLTS,
                        DatabaseConstant.Function.TS_DM_NHOM_TS_CT,
                        DatabaseConstant.Table.TS_DM_NHOM_TSCD,
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
            try
            {
                List<KT_PHAN_HE_PLOAI> lstPLoai = new List<KT_PHAN_HE_PLOAI>();
                TaiSanProcess process = new TaiSanProcess();
                sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                GetFormData(ref _objNhomTS, ref lstPLoai, sTrangThaiNVu);
                bool bRet = process.NhomTaiSanCT(action, ref _objNhomTS, ref lstPLoai, "", ref listClientResponseDetail);
                AfterApprove(bRet, listClientResponseDetail);
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                    DatabaseConstant.Function.TS_DM_NHOM_TS_CT,
                    DatabaseConstant.Table.TS_DM_NHOM_TSCD,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.QLTS,
                        DatabaseConstant.Function.TS_DM_NHOM_TS_CT,
                        DatabaseConstant.Table.TS_DM_NHOM_TSCD,
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
            try
            {
                List<KT_PHAN_HE_PLOAI> lstPLoai = new List<KT_PHAN_HE_PLOAI>();
                TaiSanProcess process = new TaiSanProcess();
                sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                GetFormData(ref _objNhomTS, ref lstPLoai, sTrangThaiNVu);
                bool bRet = process.NhomTaiSanCT(action, ref _objNhomTS, ref lstPLoai, "", ref listClientResponseDetail);
                AfterCancel(bRet, listClientResponseDetail);
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                    DatabaseConstant.Function.TS_DM_NHOM_TS_CT,
                    DatabaseConstant.Table.TS_DM_NHOM_TSCD,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.QLTS,
                        DatabaseConstant.Function.TS_DM_NHOM_TS_CT,
                        DatabaseConstant.Table.TS_DM_NHOM_TSCD,
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
            try
            {
                List<KT_PHAN_HE_PLOAI> lstPLoai = new List<KT_PHAN_HE_PLOAI>();
                TaiSanProcess process = new TaiSanProcess();
                sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri();
                GetFormData(ref _objNhomTS, ref lstPLoai, sTrangThaiNVu);
                bool bRet = process.NhomTaiSanCT(action, ref _objNhomTS, ref lstPLoai, "", ref listClientResponseDetail);
                AfterRefuse(bRet, listClientResponseDetail);
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                    DatabaseConstant.Function.TS_DM_NHOM_TS_CT,
                    DatabaseConstant.Table.TS_DM_NHOM_TSCD,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);
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
