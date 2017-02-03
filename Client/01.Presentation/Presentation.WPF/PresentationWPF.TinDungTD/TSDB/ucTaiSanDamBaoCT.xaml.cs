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
using Presentation.Process.TaiSanDamBaoServiceRef;
using Telerik.Windows.Controls.GridView;
using PresentationWPF.CustomControl;

namespace PresentationWPF.TinDungTD.TSDB
{
    /// <summary>
    /// Interaction logic for ucTaiSanDamBaoCT.xaml
    /// </summary>
    public partial class ucTaiSanDamBaoCT : UserControl
    {
        #region Khai bao
        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private DatabaseConstant.Function function = DatabaseConstant.Function.TDTD_TSDB_CT;

        public event EventHandler OnSavingCompleted;

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
              
        private int idKhachHang = 0;

        private TDTD_TAI_SAN_DAM_BAO obj;
        public TDTD_TAI_SAN_DAM_BAO Obj
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
        public ucTaiSanDamBaoCT()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            InitEventHandler();

            LoadCombobox();    
        }

        public ucTaiSanDamBaoCT(KIEM_SOAT obj)
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            InitEventHandler();

            LoadCombobox();

            action = obj.action;
            id = LNumber.StringToInt32(obj.MA_TCHIEU);
        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDungTD;component/TSDB/ucTaiSanDamBaoCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void InitEventHandler()
        {
            chkMoTaiLMF.Checked += new RoutedEventHandler(chkMoTaiLMF_Checked);
            chkMoTaiLMF.Unchecked += new RoutedEventHandler(chkMoTaiLMF_Unchecked);
            cmbLoaiTSDB.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiTSDB_SelectionChanged);
        }                

        private void LoadCombobox()
        {
            COMBOBOX_DTO combo = null;
            List<string> lstDieuKien = null;
            List<COMBOBOX_DTO> lstCombobox = new List<COMBOBOX_DTO>();
            AutoComboBox auCombo = new AutoComboBox();

            ////Loại Tiền
            //combo = new COMBOBOX_DTO();
            //combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_LOAITIEN.getValue();
            //combo.combobox = cmbLoaiTien;
            //combo.lstSource = lstSourceLoaiTien;
            //combo.lstDieuKien = lstDieuKien;
            //lstCombobox.Add(combo);

            //Gen combobox
            auCombo.GenAutoComboBoxTheoList(ref lstCombobox);

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

            bool ret = process.UnlockData(DatabaseConstant.Module.TSDB,
                DatabaseConstant.Function.TDTD_TSDB_CT,
                DatabaseConstant.Table.TDTD_TSDB,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        private void ResetData()
        {
            action = DatabaseConstant.Action.THEM;
            obj = null;
            id = 0;
            idKhachHang = 0;
            sTrangThaiNVu = "";

            ResetForm();
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
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
            //id = 0;
            //idHoSo = 0;
            //idNguoiDaiDien = 0;
            //obj = null;
            //loaiThoiHan = BusinessConstant.LOAI_THOI_HAN_HDLD.CO_THOI_HAN.layGiaTri();
            //sTrangThaiNVu = "";

            //ResetForm();
            //SetEnabledControls();
            //CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
            //txtTenNhanVien.Focus();

            //chkThemNhieuLan.IsChecked = false;
        }

        private void btn_MaKH_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                lstPopup.Clear();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add("NULL");
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_KHACHHANG", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(true, simplePopupResponse);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.Content = popup;
                Mouse.OverrideCursor = Cursors.Arrow;
                win.ShowDialog();                
                if (lstPopup.Count > 0)
                {
                    txtMaKH.Text = lstPopup[0][2].ToString();
                    //txtTenKH.Text = lstPopup[0][3].ToString();
                    txtTenChuTaiSan.Text = lstPopup[0][3].ToString();
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void btnSoSoTK_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {                
                lstPopup.Clear();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_SO_TGUI_TSDB_TDTD", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.Content = popup;
                Mouse.OverrideCursor = Cursors.Arrow;
                win.ShowDialog();
                if (lstPopup.Count > 0)
                {
                    txtSoSoTK.Text = lstPopup[0][2].ToString();
                    txtDiaChiTSDB.Text = lstPopup[0][4].ToString();
                    numGiaTriTS.Value = LNumber.ToDouble(lstPopup[0][8].ToString());

                    DataSet dsTTKH = new KhachHangProcess().getThongTinCoBanKHTheoID(LNumber.StringToInt32(lstPopup[0][5].ToString()));
                    if (dsTTKH != null && dsTTKH.Tables[0].Rows.Count > 0)
                    {
                        txtMaKH.Text = dsTTKH.Tables[0].Rows[0]["MA_KHANG"].ToString();
                        //txtTenKH.Text = dsTTKH.Tables[0].Rows[0]["TEN_KHANG"].ToString();
                        //txtTenChuTaiSan.Text = txtTenKH.Text;
                        txtTenChuTaiSan.Text = dsTTKH.Tables[0].Rows[0]["TEN_KHANG"].ToString();
                        txtDiaChiChuTS.Text = dsTTKH.Tables[0].Rows[0]["DIA_CHI"].ToString();
                    }

                    TinhGiaTriDamBao();
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void chkMoTaiLMF_Checked(object sender, RoutedEventArgs e)
        {
            btnSoSoTK.IsEnabled = true;
        }

        private void chkMoTaiLMF_Unchecked(object sender, RoutedEventArgs e)
        {
            btnSoSoTK.IsEnabled = false;
        }

        private void numGiaTriTS_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                TinhGiaTriDamBao();
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
        private void numGiaTriTS_ValueChanged(object sender, RoutedEventArgs e)
        {
            try
            {

                if (numGiaTriTS.Value < 0)
                    numGiaTriTS.Value = - numGiaTriTS.Value;

                TinhGiaTriDamBao();
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void numTiLeDamBao_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                TinhGiaTriDamBao();
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
        private void numTiLeDamBao_ValueChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbLoaiTSDB.SelectedIndex == 0)
                {    
                    if(numTiLeDamBao.Value > 95)               
                        numTiLeDamBao.Value = 95;
                    if(numTiLeDamBao.Value < 0)
                        numTiLeDamBao.Value = 0;
                }
                else if (cmbLoaiTSDB.SelectedIndex == 1)
                {
                    if (numTiLeDamBao.Value > 80)
                        numTiLeDamBao.Value = 80;
                    if (numTiLeDamBao.Value < 0)
                        numTiLeDamBao.Value = 0;
                }
                else if (cmbLoaiTSDB.SelectedIndex == 2)
                {
                    if (numTiLeDamBao.Value > 70)
                        numTiLeDamBao.Value = 70;
                    if (numTiLeDamBao.Value < 0)
                        numTiLeDamBao.Value = 0;
                }
                TinhGiaTriDamBao();
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
        private void cmbLoaiTSDB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if(cmbLoaiTSDB.SelectedIndex == 0)
                {
                    lblSoSoTK.Visibility = Visibility.Visible;
                    txtSoSoTK.Visibility = Visibility.Visible;
                    chkMoTaiLMF.Visibility = Visibility.Visible;
                    btnSoSoTK.Visibility = Visibility.Visible;
                    numTiLeDamBao.Value = 95;
                }
                else if (cmbLoaiTSDB.SelectedIndex == 1)
                {
                    lblSoSoTK.Visibility = Visibility.Collapsed;
                    txtSoSoTK.Visibility = Visibility.Collapsed;
                    chkMoTaiLMF.Visibility = Visibility.Collapsed;
                    btnSoSoTK.Visibility = Visibility.Collapsed;
                    numTiLeDamBao.Value = 80;
                }
                else if (cmbLoaiTSDB.SelectedIndex == 2)
                {
                    lblSoSoTK.Visibility = Visibility.Collapsed;
                    txtSoSoTK.Visibility = Visibility.Collapsed;
                    chkMoTaiLMF.Visibility = Visibility.Collapsed;
                    btnSoSoTK.Visibility = Visibility.Collapsed;
                    numTiLeDamBao.Value = 70;
                }
                TinhGiaTriDamBao();
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
        #endregion

        #region Xử lý nghiệp vụ
        private void GetFormData(ref TDTD_TAI_SAN_DAM_BAO obj, string sTrangThaiNVu)
        {
            try
            {
                obj = new TDTD_TAI_SAN_DAM_BAO();

                #region TDTD_TSDB
                TDTD_TSDB objTSDB = new TDTD_TSDB();
                objTSDB.ID = id;
                objTSDB.MA_TSDB = txtMaTSDB.Text;
                objTSDB.TEN_TSDB = txtTenTSDB.Text;
                objTSDB.DIA_CHI_TSDB = txtDiaChiTSDB.Text;
                objTSDB.TEN_CHU_TSAN = txtTenChuTaiSan.Text;
                objTSDB.DIA_CHI_CHU_TSAN = txtDiaChiChuTS.Text;
                objTSDB.MA_KHANG = txtMaKH.Text;

                objTSDB.MA_LOAI_TSDB = cmbLoaiTSDB.SelectedValue.ToString();
                if (objTSDB.MA_LOAI_TSDB.ToUpper().Equals("SO_TIET_KIEM"))
                {
                    objTSDB.SO_SO_TK = txtSoSoTK.Text;
                    objTSDB.MO_TAI_LMF = (bool)chkMoTaiLMF.IsChecked ? "CO" : "KHONG";
                }
                else
                {
                    objTSDB.SO_SO_TK = "";
                    objTSDB.MO_TAI_LMF = "";
                }

                objTSDB.GTRI_TAI_SAN = LNumber.ToDecimal(numGiaTriTS.Value);
                objTSDB.TI_LE_DAM_BAO = LNumber.ToDecimal(numTiLeDamBao.Value);
                objTSDB.GTRI_DAM_BAO = LNumber.ToDecimal(numGiaTriDamBao.Value);

                objTSDB.DIEN_GIAI = txtDienGiai.Text;

                //Thông tin kiểm soát
                objTSDB.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                objTSDB.TTHAI_NVU = sTrangThaiNVu;
                objTSDB.MA_DVI_QLY = ClientInformation.MaDonVi;
                objTSDB.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                objTSDB.NGAY_NHAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                objTSDB.NGUOI_NHAP = txtNguoiLap.Text;
                if (action != DatabaseConstant.Action.THEM)
                {
                    objTSDB.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    objTSDB.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }
                objTSDB.TTHAI_TAI_SAN = BusinessConstant.TRANG_THAI_TAT_TOAN.CHUA_TAT_TOAN.layGiaTri();
                #endregion

                obj.ID = id;
                obj.OBJ_TDTD_TSDB = objTSDB;
                //obj.MA_TSDB = objTSDB.MA_TSDB;
                //obj.TEN_TSDB = objTSDB.TEN_TSDB;
                //obj.DIA_CHI_TSDB = objTSDB.DIA_CHI_TSDB;
                //obj.TEN_CHU_TSDB = objTSDB.TEN_CHU_TSAN;
                //obj.DIA_CHI_CHU_TSDB = objTSDB.DIA_CHI_CHU_TSAN;
                //obj.MA_LOAI_TSDB = objTSDB.MA_LOAI_TSDB;
                //obj.GIA_TRI_TAI_SAN = (decimal)objTSDB.GTRI_TAI_SAN;
                //obj.TI_LE_DAM_BAO = (decimal)objTSDB.TI_LE_DAM_BAO;
                //obj.GIA_TRI_DAM_BAO = (decimal)objTSDB.GTRI_DAM_BAO;
                //obj.DIEN_GIAI = objTSDB.DIEN_GIAI;
                obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                obj.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.TTHAI_NVU = sTrangThaiNVu;
                obj.NGAY_NHAP = objTSDB.NGAY_NHAP;
                obj.NGUOI_NHAP = objTSDB.NGUOI_NHAP;
                obj.NGUOI_CNHAT = objTSDB.NGUOI_CNHAT;
                obj.NGAY_CNHAT = objTSDB.NGAY_CNHAT;
                obj.TTHAI_TAI_SAN = BusinessConstant.TRANG_THAI_TAT_TOAN.CHUA_TAT_TOAN.layGiaTri();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            TaiSanDamBaoProcess processTaiSanDamBao = new TaiSanDamBaoProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            obj = new TDTD_TAI_SAN_DAM_BAO();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processTaiSanDamBao.TaiSanDamBaoTDTD(DatabaseConstant.Action.LOAD_DATA, ref obj, ref listClientResponseDetail);
                if (ret == true)
                {
                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    #region Thông tin chung
                    txtMaTSDB.Text = obj.OBJ_TDTD_TSDB.MA_TSDB;
                    txtTenTSDB.Text = obj.OBJ_TDTD_TSDB.TEN_TSDB;
                    txtDiaChiTSDB.Text = obj.OBJ_TDTD_TSDB.DIA_CHI_TSDB;
                    txtSoSoTK.Text = obj.OBJ_TDTD_TSDB.SO_SO_TK;
                    chkMoTaiLMF.IsChecked = obj.OBJ_TDTD_TSDB.MO_TAI_LMF.ToUpper().Equals("CO") ? true : false;
                    txtMaKH.Text = obj.OBJ_TDTD_TSDB.MA_KHANG;
                    txtTenChuTaiSan.Text = obj.OBJ_TDTD_TSDB.TEN_CHU_TSAN;
                    txtDiaChiChuTS.Text = obj.OBJ_TDTD_TSDB.DIA_CHI_CHU_TSAN;
                    
                    numGiaTriTS.Value = Convert.ToDouble(obj.OBJ_TDTD_TSDB.GTRI_TAI_SAN);
                    numTiLeDamBao.Value = Convert.ToDouble(obj.OBJ_TDTD_TSDB.TI_LE_DAM_BAO);
                    numGiaTriDamBao.Value = Convert.ToDouble(obj.OBJ_TDTD_TSDB.GTRI_DAM_BAO);

                    if (obj.OBJ_TDTD_TSDB.MA_LOAI_TSDB.ToUpper().Equals("SO_TIET_KIEM"))
                        cmbLoaiTSDB.SelectedIndex = 0;
                    else if (obj.OBJ_TDTD_TSDB.MA_LOAI_TSDB.ToUpper().Equals("BAT_DONG_SAN"))
                        cmbLoaiTSDB.SelectedIndex = 1;
                    else if (obj.OBJ_TDTD_TSDB.MA_LOAI_TSDB.ToUpper().Equals("DONG_SAN_VA_TAI_SAN_KHAC"))
                        cmbLoaiTSDB.SelectedIndex = 2;

                    string _mA_KH;
                    _mA_KH = obj.OBJ_TDTD_TSDB.MA_KHANG;
                    //DataSet dsKHang = new KhachHangProcess().getThongTinCoBanKHTheoMaKH(_mA_KH);
                    //if (dsKHang != null && dsKHang.Tables.Count > 0)
                    //{
                    //    DataRow dr = dsKHang.Tables[0].Rows[0];
                    //    if (dr["ID"] != null && !LString.IsNullOrEmptyOrSpace(dr["ID"].ToString()))
                    //    {
                    //        txtTenKH.Text = dr["TEN_KHANG"].ToString();
                    //    }
                    //}

                    txtDienGiai.Text = obj.OBJ_TDTD_TSDB.DIEN_GIAI;
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

        private void LoadFormData()
        {
            
        }

        private void ResetForm()
        {
            sTrangThaiNVu = "";
            lblTrangThai.Content = "";

            #region Thông tin chung
            cmbLoaiTSDB.SelectedIndex = 0;
            txtMaTSDB.Text = "";
            txtSoSoTK.Text = "";
            txtTenTSDB.Text = "";
            txtDiaChiTSDB.Text = "";
            chkMoTaiLMF.IsChecked = true;
            txtTenTSDB.Text = "";
            txtMaKH.Text = "";
            //txtTenKH.Text = "";
            txtTenChuTaiSan.Text = "";
            txtDiaChiChuTS.Text = "";
            numGiaTriTS.Value = 0;
            numTiLeDamBao.Value = 95;
            numGiaTriDamBao.Value = 0;
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
            if (txtTenTSDB.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaChon(lbTenTSDB.Content.ToString());
                txtTenTSDB.Focus();
                return false;
            }
            if (txtDiaChiTSDB.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaChon(lbDiaChiTSDB.Content.ToString());
                txtDiaChiTSDB.Focus();
                return false;
            }
            //if (txtMaKH.Text.IsNullOrEmptyOrSpace())
            //{
            //    CommonFunction.ThongBaoChuaChon(lblMaKH.Content.ToString());
            //    txtMaKH.Focus();
            //    return false;
            //}
            if (txtTenChuTaiSan.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaChon(lblTenChuTaiSan.Content.ToString());
                txtTenChuTaiSan.Focus();
                return false;
            }
            if (txtDiaChiChuTS.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaChon(lbDiaChiChuTS.Content.ToString());
                txtDiaChiChuTS.Focus();
                return false;
            }
            if (txtSoSoTK.IsVisible && txtSoSoTK.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaChon(lblSoSoTK.Content.ToString());
                txtSoSoTK.Focus();
                return false;
            }
            return true;
        }

        private void SetEnabledControls()
        {
            #region Thêm
            if (action == DatabaseConstant.Action.THEM)
            {
                cmbLoaiTSDB.IsEnabled = true;
                txtSoSoTK.IsEnabled = true;
                btnSoSoTK.IsEnabled = true;
                chkMoTaiLMF.IsEnabled = true;
                txtMaTSDB.IsEnabled = false;
                txtTenTSDB.IsEnabled = true;
                txtDiaChiTSDB.IsEnabled = true;
                txtMaKH.IsEnabled = true;
                btnMaKH.IsEnabled = true;
                //txtTenKH.IsEnabled = false;
                txtTenChuTaiSan.IsEnabled = true;
                txtDiaChiChuTS.IsEnabled = true;
                numGiaTriTS.IsEnabled = true;
                numTiLeDamBao.IsEnabled = true;
                numGiaTriDamBao.IsEnabled = false;
                txtDienGiai.IsEnabled = true;
            }
            #endregion

            #region Sửa
            else if (action == DatabaseConstant.Action.SUA)
            {
                cmbLoaiTSDB.IsEnabled = true;
                txtSoSoTK.IsEnabled = true;
                btnSoSoTK.IsEnabled = true;
                chkMoTaiLMF.IsEnabled = true;
                txtMaTSDB.IsEnabled = false;
                txtTenTSDB.IsEnabled = true;
                txtDiaChiTSDB.IsEnabled = true;
                txtMaKH.IsEnabled = true;
                btnMaKH.IsEnabled = true;
                //txtTenKH.IsEnabled = false;
                txtTenChuTaiSan.IsEnabled = true;
                txtDiaChiChuTS.IsEnabled = true;
                numGiaTriTS.IsEnabled = true;
                numTiLeDamBao.IsEnabled = true;
                numGiaTriDamBao.IsEnabled = false;
                txtDienGiai.IsEnabled = true;
            }
            #endregion

            #region Xem ~ Mặc định
            else
            {
                cmbLoaiTSDB.IsEnabled = false;
                txtSoSoTK.IsEnabled = false;
                btnSoSoTK.IsEnabled = false;
                chkMoTaiLMF.IsEnabled = false;
                txtMaTSDB.IsEnabled = false;
                txtTenTSDB.IsEnabled = false;
                txtDiaChiTSDB.IsEnabled = false;
                txtMaKH.IsEnabled = false;
                btnMaKH.IsEnabled = false;
                //txtTenKH.IsEnabled = false;
                txtTenChuTaiSan.IsEnabled = false;
                txtDiaChiChuTS.IsEnabled = false;
                numGiaTriTS.IsEnabled = false;
                numTiLeDamBao.IsEnabled = false;
                numGiaTriDamBao.IsEnabled = false;
                txtDienGiai.IsEnabled = false;
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

                obj = new TDTD_TAI_SAN_DAM_BAO();

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

                obj = new TDTD_TAI_SAN_DAM_BAO();

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
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
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
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnAddNew(TDTD_TAI_SAN_DAM_BAO obj)
        {

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TaiSanDamBaoProcess processTaiSanDamBao = new TaiSanDamBaoProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processTaiSanDamBao.TaiSanDamBaoTDTD(DatabaseConstant.Action.THEM, ref obj, ref listClientResponseDetail);
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

        public void AfterAddNew(bool ret, TDTD_TAI_SAN_DAM_BAO obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                    id = obj.ID;

                    txtMaTSDB.Text = obj.MA_TSDB;
                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);

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


        public void BeforeModifyFromDetail()
        {
            try
            {
                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);

                bool ret = process.LockData(DatabaseConstant.Module.TSDB,
                    DatabaseConstant.Function.TDTD_TSDB_CT,
                    DatabaseConstant.Table.TDTD_TSDB,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                if (ret)
                {
                    action = DatabaseConstant.Action.SUA;
                    SetEnabledControls();
                    CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
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

        public void OnModify(TDTD_TAI_SAN_DAM_BAO obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TaiSanDamBaoProcess processTaiSanDamBao = new TaiSanDamBaoProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processTaiSanDamBao.TaiSanDamBaoTDTD(DatabaseConstant.Action.SUA, ref obj, ref listClientResponseDetail);
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

        public void AfterModify(bool ret, TDTD_TAI_SAN_DAM_BAO obj, List<ClientResponseDetail> listClientResponseDetail)
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TSDB,
                    DatabaseConstant.Function.TDTD_TSDB_CT,
                    DatabaseConstant.Table.TDTD_TSDB,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.TSDB,
                        DatabaseConstant.Function.TDTD_TSDB_CT,
                        DatabaseConstant.Table.TDTD_TSDB,
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TSDB,
                    DatabaseConstant.Function.TDTD_TSDB_CT,
                    DatabaseConstant.Table.TDTD_TSDB,
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
            TaiSanDamBaoProcess processTaiSanDamBao = new TaiSanDamBaoProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processTaiSanDamBao.TaiSanDamBaoTDTD(action, ref obj, ref listClientResponseDetail);
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
                processTaiSanDamBao = null;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TSDB,
                    DatabaseConstant.Function.TDTD_TSDB_CT,
                    DatabaseConstant.Table.TDTD_TSDB,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.TSDB,
                        DatabaseConstant.Function.TDTD_TSDB_CT,
                        DatabaseConstant.Table.TDTD_TSDB,
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
            TaiSanDamBaoProcess processTaiSanDamBao = new TaiSanDamBaoProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processTaiSanDamBao.TaiSanDamBaoTDTD(action, ref obj, ref listClientResponseDetail);
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
                processTaiSanDamBao = null;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TSDB,
                    DatabaseConstant.Function.TDTD_TSDB_CT,
                    DatabaseConstant.Table.TDTD_TSDB,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.TSDB,
                        DatabaseConstant.Function.TDTD_TSDB_CT,
                        DatabaseConstant.Table.TDTD_TSDB,
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
            TaiSanDamBaoProcess processTaiSanDamBao = new TaiSanDamBaoProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processTaiSanDamBao.TaiSanDamBaoTDTD(action, ref obj, ref listClientResponseDetail);
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
                processTaiSanDamBao = null;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TSDB,
                    DatabaseConstant.Function.TDTD_TSDB_CT,
                    DatabaseConstant.Table.TDTD_TSDB,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.TSDB,
                        DatabaseConstant.Function.TDTD_TSDB_CT,
                        DatabaseConstant.Table.TDTD_TSDB,
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
            TaiSanDamBaoProcess processTaiSanDamBao = new TaiSanDamBaoProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processTaiSanDamBao.TaiSanDamBaoTDTD(action, ref obj, ref listClientResponseDetail);
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
                processTaiSanDamBao = null;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TSDB,
                    DatabaseConstant.Function.TDTD_TSDB_CT,
                    DatabaseConstant.Table.TDTD_TSDB,
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

        private void TinhGiaTriDamBao()
        {
            numGiaTriDamBao.Value = numGiaTriTS.Value * numTiLeDamBao.Value / 100;
        }

        private void numTiLeDamBao_ValueChanged_1(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

        }
    }
}
