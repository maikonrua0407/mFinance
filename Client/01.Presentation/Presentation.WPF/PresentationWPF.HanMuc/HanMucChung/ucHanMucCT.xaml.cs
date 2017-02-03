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
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.Common;
using PresentationWPF.CustomControl;
using Presentation.Process.PopupServiceRef;
using Telerik.Windows.Controls;
using Presentation.Process.HanMucServiceRef;

namespace PresentationWPF.HanMuc.HanMucChung
{
    /// <summary>
    /// Interaction logic for ucHanMucCT.xaml
    /// </summary>
    public partial class ucHanMucCT : UserControl
    {
        #region Khai bao
        public event EventHandler OnSavingCompleted;

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

        //Source cac combobox
        List<AutoCompleteEntry> lstLoaiDoiTuong = new List<AutoCompleteEntry>();

        //Khai bao popup
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        //Object thông tin hạn mức tổng
        public List<DC_HAN_MUC> _lstObjHM = new List<DC_HAN_MUC>();

               
        //Thông tin function và trạng thái nghiệp vụ
        private DatabaseConstant.Function _function = DatabaseConstant.Function.DC_HAN_MUC;

        public DatabaseConstant.Action _action = DatabaseConstant.Action.THEM;

        private string tthaiNvu = "";
        #endregion

        #region Khoi tao
        public ucHanMucCT()
        {
            KhoiTaoChung();
            InitEventHandler();
            BeforeAddNew();
            teldtNgayHieuLuc.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            teldtNgayHetHieuLuc.Value = null;
        }
        
        private void KhoiTaoChung()
        {
            InitializeComponent();
            DuyetQuyenTinhNang();
            LoadCombobox();         
        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.HanMuc;component/HanMucChung/ucHanMucCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }
        #endregion

        #region Dang ky hot key, shortcut key
        /// <summary>
        /// Dang ky hot key, shortcut key
        /// </summary>
        
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

        private void InitEventHandler()
        {
            chkHMGiaoDichToiDa.Checked += new RoutedEventHandler(chkHMGiaoDichToiDa_Checked);
            chkHMGiaoDichToiDa.Unchecked += new RoutedEventHandler(chkHMGiaoDichToiDa_Unchecked);
            chkHMPheDuyetToiDa.Checked += new RoutedEventHandler(chkHMPheDuyetToiDa_Checked);
            chkHMPheDuyetToiDa.Unchecked += new RoutedEventHandler(chkHMPheDuyetToiDa_Unchecked);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Hiển thị Form khi thêm mới dữ liệu
            if (_action == DatabaseConstant.Action.THEM)
            {
                BeforeAddNew();
            }

            //Hiển thị Form khi sửa dữ liệu
            else if (_action == DatabaseConstant.Action.SUA)
            {
                BeforeModifyFromList(_action);
            }

            //Hiển thị Form khi xem dữ liệu
            else if (_action == DatabaseConstant.Action.XEM)
            {
                BeforeViewFromList();
            }

            else if (_action == DatabaseConstant.Action.XEM)
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

        private void cmbLoaiDoiTuong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtMaDoiTuong.Text = "";
            lblTenDoiTuong.Content = "";
        }

        private void chkHMGiaoDichToiDa_Checked(object sender, RoutedEventArgs e)
        {
            numHMGiaoDich.Value = 0;
            numHMGiaoDich.IsEnabled = false;
        }

        private void chkHMGiaoDichToiDa_Unchecked(object sender, RoutedEventArgs e)
        {
            numHMGiaoDich.IsEnabled = true;
        }

        private void chkHMPheDuyetToiDa_Checked(object sender, RoutedEventArgs e)
        {
            numHMPheDuyet.Value = 0;
            numHMPheDuyet.IsEnabled = false;
        }

        private void chkHMPheDuyetToiDa_Unchecked(object sender, RoutedEventArgs e)
        {
            numHMPheDuyet.IsEnabled = true;
        }

        private void LoadCombobox()
        {
            
            COMBOBOX_DTO combo = null;
            List<string> lstDieuKien = null;
            List<COMBOBOX_DTO> lstCombobox = new List<COMBOBOX_DTO>();
            AutoComboBox auCombo = new AutoComboBox();

            try
            {
                //Loại khách hàng
                lstDieuKien = new List<string>();
                lstDieuKien.Add("LOAI_DTUONG_KTHAC_TNGUYEN");
                combo = new COMBOBOX_DTO();
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                combo.combobox = cmbLoaiDoiTuong;
                combo.lstSource = lstLoaiDoiTuong;
                combo.lstDieuKien = lstDieuKien;
                combo.maChon = "NSD";
                lstCombobox.Add(combo);
                                
                //Gen combobox
                auCombo.GenAutoComboBoxTheoList(ref lstCombobox);
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void btnMaDoiTuong_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbLoaiDoiTuong.SelectedIndex >= 0)
                {
                    AutoCompleteEntry auLoaiDoiTuong = (AutoCompleteEntry)cmbLoaiDoiTuong.SelectedItem;

                    if (auLoaiDoiTuong.KeywordStrings[0].Equals("NSD"))
                    {
                        List<string> lstDieuKien = new List<string>();
                        lstDieuKien.Add(ClientInformation.MaDonVi);
                        lstPopup = new List<DataRow>();
                        PopupProcess popupProcess = new PopupProcess();
                        popupProcess.getPopupInformation("POPUP_HT_NSD", lstDieuKien);                        
                        SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                        ucPopup popup = new ucPopup(true, simplePopupResponse, false);
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
                                txtMaDoiTuong.Tag = Convert.ToInt32(dr["ID"]);
                                txtMaDoiTuong.Text = dr["MA_NSD"].ToString();
                                lblTenDoiTuong.Content = dr["TEN_DAY_DU"].ToString();
                            }
                        }
                    }
                    else if(auLoaiDoiTuong.KeywordStrings[0].Equals("NHNSD"))
                    {
                        List<string> lstDieuKien = new List<string>();
                        lstDieuKien.Add(ClientInformation.MaDonVi);
                        lstPopup = new List<DataRow>();
                        PopupProcess popupProcess = new PopupProcess();
                        popupProcess.getPopupInformation("POPUP_HT_NHNSD", lstDieuKien);
                        SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                        ucPopup popup = new ucPopup(true, simplePopupResponse, false);
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
                                txtMaDoiTuong.Tag = Convert.ToInt32(dr["ID"]);
                                txtMaDoiTuong.Text = dr["MA_NHNSD"].ToString();
                                lblTenDoiTuong.Content = dr["TEN_NHNSD"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void SetEnabledAllControls(bool enable)
        {
            cmbLoaiDoiTuong.IsEnabled = enable;
            txtMaDoiTuong.IsEnabled = enable;
            btnMaDoiTuong.IsEnabled = enable;
            numHMGiaoDich.IsEnabled = enable;
            chkHMGiaoDichToiDa.IsEnabled = enable;
            numHMPheDuyet.IsEnabled = enable;
            chkHMPheDuyetToiDa.IsEnabled = enable;
            teldtNgayHieuLuc.IsEnabled = enable;
            dtpNgayHieuLuc.IsEnabled = enable;
            teldtNgayHetHieuLuc.IsEnabled = enable;
            dtpNgayHetHieuLuc.IsEnabled = enable;
        }

        /// <summary>
        /// Hàm xử lý giải phóng lock dữ liệu cho form
        /// </summary>
        private void Release()
        {
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
           
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
        #endregion

        #region Xử lý nghiệp vụ

        private void SetFormData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                #region Get data từ server
                HanMucProcess process = new HanMucProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = process.HanMucChung(DatabaseConstant.Action.LOAD, ref _lstObjHM, ref listClientResponseDetail);
                #endregion

                if (ret == true)
                {                    
                    tthaiNvu = _lstObjHM.FirstOrDefault().TTHAI_NVU;

                    cmbLoaiDoiTuong.SelectedIndex = lstLoaiDoiTuong.IndexOf(lstLoaiDoiTuong.FirstOrDefault(e => e.KeywordStrings.First().Equals(_lstObjHM.FirstOrDefault().MA_DTUONG_LOAI)));
                    txtMaDoiTuong.Tag = _lstObjHM.FirstOrDefault().ID_DTUONG;
                    txtMaDoiTuong.Text = _lstObjHM.FirstOrDefault().MA_DTUONG;

                    if (_lstObjHM.Where(e => e.MA_TNANG == "ADD").FirstOrDefault().MAX != null)
                    {
                        chkHMGiaoDichToiDa.IsChecked = false;
                        numHMGiaoDich.Value = (double?)_lstObjHM.Where(e => e.MA_TNANG == "ADD").FirstOrDefault().MAX;
                    }
                    else
                    {
                        chkHMGiaoDichToiDa.IsChecked = true;
                    }

                    if (_lstObjHM.Where(e => e.MA_TNANG == "APPROVE").FirstOrDefault().MAX != null)
                    {
                        chkHMPheDuyetToiDa.IsChecked = false;
                        numHMPheDuyet.Value = (double?)_lstObjHM.Where(e => e.MA_TNANG == "APPROVE").FirstOrDefault().MAX;
                    }
                    else
                    {
                        chkHMPheDuyetToiDa.IsChecked = true;
                    }

                    teldtNgayHieuLuc.Value = LDateTime.StringToDate(_lstObjHM.FirstOrDefault().NGAY_ADUNG, "yyyyMMdd");
                    if (_lstObjHM.FirstOrDefault().NGAY_HHAN != null)
                        teldtNgayHetHieuLuc.Value = LDateTime.StringToDate(_lstObjHM.FirstOrDefault().NGAY_HHAN, "yyyyMMdd");
                    else
                        teldtNgayHetHieuLuc.Value = null;

                    lblTrangThai.Content = BusinessConstant.layNgonNguSuDung(_lstObjHM.FirstOrDefault().TTHAI_BGHI);
                    txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(_lstObjHM.FirstOrDefault().TTHAI_BGHI);

                    txtNguoiLap.Text = _lstObjHM.FirstOrDefault().NGUOI_NHAP;
                    teldtNgayNhap.Value = LDateTime.StringToDate(_lstObjHM.FirstOrDefault().NGAY_NHAP, "yyyyMMdd");

                    if (_lstObjHM.FirstOrDefault().NGUOI_CNHAT != null)
                        txtNguoiCapNhat.Text = _lstObjHM.FirstOrDefault().NGUOI_CNHAT;
                    else
                        txtNguoiCapNhat.Text = "";
                    if (_lstObjHM.FirstOrDefault().NGAY_CNHAT != null)
                        teldtNgayCNhat.Value = LDateTime.StringToDate(_lstObjHM.FirstOrDefault().NGAY_CNHAT, "yyyyMMdd");
                    else
                        teldtNgayCNhat.Value = null;
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                CommonFunction.ThongBaoLoi(ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private bool Validation()
        {
            #region Kiểm tra mã khách hàng
            if (txtMaDoiTuong.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblMaDoiTuong.Content.ToString());
                txtMaDoiTuong.Focus();
                return false;
            }
            #endregion

            #region Kiểm tra hạn mức phê duyệt
            if (chkHMGiaoDichToiDa.IsChecked == false && numHMGiaoDich.Value.IsNullOrEmpty())
            {
                CommonFunction.ThongBaoTrong(lblHMGiaoDich.Content.ToString());
                numHMPheDuyet.Focus();
                return false;
            }
            #endregion

            #region Kiểm tra hạn mức phê duyệt
            if (chkHMPheDuyetToiDa.IsChecked == false && numHMPheDuyet.Value.IsNullOrEmpty())
            {
                CommonFunction.ThongBaoTrong(lblHMPheDuyet.Content.ToString());
                numHMPheDuyet.Focus();
                return false;
            }
            #endregion

            #region Kiểm tra ngày phê duyệt
            if (teldtNgayHieuLuc.Value.IsNullOrEmpty())
            {
                CommonFunction.ThongBaoTrong(lblNgayApDung.Content.ToString());
                teldtNgayHieuLuc.Focus();
                return false;
            }
            else
            {
                if (teldtNgayHieuLuc.Value < LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat))
                {
                    CommonFunction.ThongBaoLoi(LLanguage.SearchResourceByKey("M_ResponseMessage_NgayApDung_NhoHonNgayGiaoDich"));
                    teldtNgayHetHieuLuc.Focus();
                    return false;
                }
            }
            #endregion     

            #region Kiểm tra ngày hết hiệu lực
            if (!teldtNgayHetHieuLuc.Value.IsNullOrEmpty())
            {
                if (teldtNgayHetHieuLuc.Value <= teldtNgayHieuLuc.Value)
                {
                    CommonFunction.ThongBaoLoi(LLanguage.SearchResourceByKey("M.DungChung.ThongBao.NgayHetHanNhoNgayHieuLuc"));
                    teldtNgayHetHieuLuc.Focus();
                    return false;
                }
            }
            #endregion
            
            return true;
        }

        private void GetFormData(ref List<DC_HAN_MUC> lstObjHanMuc)
        {
            try
            {
                lstObjHanMuc = new List<DC_HAN_MUC>();

                #region Thông tin hạn mức giao dịch
                DC_HAN_MUC objHMGiaoDich = new DC_HAN_MUC();
                objHMGiaoDich.ID = 0;
                objHMGiaoDich.MA_DTUONG_LOAI = lstLoaiDoiTuong.ElementAt(cmbLoaiDoiTuong.SelectedIndex).KeywordStrings.First();
                objHMGiaoDich.ID_DTUONG = LNumber.StringToInt32(txtMaDoiTuong.Tag.ToString());
                objHMGiaoDich.MA_DTUONG = txtMaDoiTuong.Text;
                objHMGiaoDich.LOAI_HAN_MUC = BusinessConstant.LOAI_HAN_MUC.CHUNG.layGiaTri();
                objHMGiaoDich.ID_CNANG = null;
                objHMGiaoDich.MA_CNANG = null;
                objHMGiaoDich.ID_TNANG = null;
                objHMGiaoDich.MA_TNANG = "ADD";
                objHMGiaoDich.MIN = 0;
                if (chkHMGiaoDichToiDa.IsChecked == false)
                    objHMGiaoDich.MAX = (decimal)numHMGiaoDich.Value;

                objHMGiaoDich.NGAY_ADUNG = LDateTime.DateToString(teldtNgayHieuLuc.Value.Value, "yyyyMMdd");
                if (teldtNgayHetHieuLuc.Value is DateTime)
                    objHMGiaoDich.NGAY_HHAN = LDateTime.DateToString(teldtNgayHetHieuLuc.Value.Value, "yyyyMMdd");

                //Thông tin kiểm soát
                objHMGiaoDich.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                objHMGiaoDich.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                objHMGiaoDich.MA_DVI_QLY = ClientInformation.MaDonVi;
                objHMGiaoDich.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                objHMGiaoDich.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                objHMGiaoDich.NGUOI_NHAP = ClientInformation.TenDangNhap;
                if (_action == DatabaseConstant.Action.SUA)
                {
                    objHMGiaoDich.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    objHMGiaoDich.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }

                lstObjHanMuc.Add(objHMGiaoDich);
                #endregion

                #region Thông tin hạn mức phê duyệt
                DC_HAN_MUC objHMPheDuyet = new DC_HAN_MUC();
                objHMPheDuyet.ID = 0;
                objHMPheDuyet.MA_DTUONG_LOAI = lstLoaiDoiTuong.ElementAt(cmbLoaiDoiTuong.SelectedIndex).KeywordStrings.First();
                objHMPheDuyet.ID_DTUONG = LNumber.StringToInt32(txtMaDoiTuong.Tag.ToString());
                objHMPheDuyet.MA_DTUONG = txtMaDoiTuong.Text;
                objHMPheDuyet.LOAI_HAN_MUC = BusinessConstant.LOAI_HAN_MUC.CHUNG.layGiaTri();
                objHMPheDuyet.ID_CNANG = null;
                objHMPheDuyet.MA_CNANG = null;
                objHMPheDuyet.ID_TNANG = null;
                objHMPheDuyet.MA_TNANG = "APPROVE";
                objHMPheDuyet.MIN = 0;
                if (chkHMPheDuyetToiDa.IsChecked == false)
                    objHMPheDuyet.MAX = (decimal)numHMPheDuyet.Value;

                objHMPheDuyet.NGAY_ADUNG = LDateTime.DateToString(teldtNgayHieuLuc.Value.Value, "yyyyMMdd");
                if (teldtNgayHetHieuLuc.Value is DateTime)
                    objHMPheDuyet.NGAY_HHAN = LDateTime.DateToString(teldtNgayHetHieuLuc.Value.Value, "yyyyMMdd");

                //Thông tin kiểm soát
                objHMPheDuyet.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                objHMPheDuyet.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                objHMPheDuyet.MA_DVI_QLY = ClientInformation.MaDonVi;
                objHMPheDuyet.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                objHMPheDuyet.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                objHMPheDuyet.NGUOI_NHAP = ClientInformation.TenDangNhap;
                if (_action == DatabaseConstant.Action.SUA)
                {
                    objHMPheDuyet.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    objHMPheDuyet.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }

                lstObjHanMuc.Add(objHMPheDuyet);
                #endregion
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void ResetForm()
        {
            _lstObjHM = new List<DC_HAN_MUC>();

            tthaiNvu = "";
            txtMaDoiTuong.Text = "";
            lblTenDoiTuong.Content = "";
            txtMaDoiTuong.Tag = "";
            lblTrangThai.Content = "";
         
            cmbLoaiDoiTuong.SelectedIndex = 0;

            chkHMGiaoDichToiDa.IsChecked = false;
            chkHMPheDuyetToiDa.IsChecked = false;

            numHMGiaoDich.Value = 0;
            numHMPheDuyet.Value = 0;
            teldtNgayHieuLuc.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai,"yyyyMMdd");
            teldtNgayHetHieuLuc.Value = null;

            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
        }

        public void OnHold()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (!Validation()) return;
                tthaiNvu = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
                List<DC_HAN_MUC> lstObjHM = null;
                GetFormData(ref lstObjHM);
                if (_action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(ref lstObjHM);
                }
                else if (_action == DatabaseConstant.Action.SUA)
                {
                    OnModify(ref lstObjHM);
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

        public void OnSave()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (!Validation()) return;
                tthaiNvu = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.TRINH_DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
                List<DC_HAN_MUC> lstObjHM = null;
                GetFormData(ref lstObjHM);
                if (_action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(ref lstObjHM);
                }
                else if (_action == DatabaseConstant.Action.SUA)
                {
                    OnModify(ref lstObjHM);
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

        #region Action View
        //Sau khi luu chuyen sang trang thai xem thi goi toi ham nay
        public void BeforeViewFromDetail()
        {
            SetEnabledAllControls(false);
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, _function);
        }

        public void BeforeViewFromList()
        {
            try
            {
                SetFormData();
                BeforeViewFromDetail();
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
        #endregion

        #region Action AddNew
        public void BeforeAddNew()
        {            
            ResetForm();
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, _function);
        }

        public void OnAddNew(ref List<DC_HAN_MUC> lstObjHM)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HanMucProcess process = new HanMucProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;
                ret = new HanMucProcess().HanMucChung(DatabaseConstant.Action.THEM, ref lstObjHM, ref listClientResponseDetail);
                AfterAddNew(ret, lstObjHM, listClientResponseDetail);
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

        public void AfterAddNew(bool ret, List<DC_HAN_MUC> lstObjHM, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret == true)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);
                    SetEnabledAllControls(false);

                    tthaiNvu = lstObjHM.FirstOrDefault().TTHAI_NVU;
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, _function);
                                       
                    lblTrangThai.Content = BusinessConstant.layNgonNguSuDung(lstObjHM.FirstOrDefault().TTHAI_BGHI);
                    txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(lstObjHM.FirstOrDefault().TTHAI_BGHI);
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

        #region Action Modify
        public void BeforeModifyFromDetail()
        {
            try
            {
                _action = DatabaseConstant.Action.SUA;
                SetEnabledAllControls(true);
                CommonFunction.RefreshButton(Toolbar, _action, tthaiNvu, mnuMain, _function);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void BeforeModifyFromList(DatabaseConstant.Action act)
        {
            try
            {
                SetFormData();
                _action = DatabaseConstant.Action.SUA;
                if (_action != DatabaseConstant.Action.THEM && _action != DatabaseConstant.Action.SUA)
                {
                    SetEnabledAllControls(false);
                }
                else
                {
                    SetEnabledAllControls(true);
                }
                CommonFunction.RefreshButton(Toolbar, _action, tthaiNvu, mnuMain, _function);
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnModify(ref List<DC_HAN_MUC> lstObjHM)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HanMucProcess process = new HanMucProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = new HanMucProcess().HanMucChung(DatabaseConstant.Action.SUA, ref lstObjHM, ref listClientResponseDetail);
                AfterAddNew(ret, lstObjHM, listClientResponseDetail);
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

        public void AfterModify(bool ret, List<DC_HAN_MUC> lstObjHM, List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret == true)
            {
                LMessage.ShowMessage("M.DungChung.SuaThanhCong", LMessage.MessageBoxType.Information);
                SetEnabledAllControls(false);

                tthaiNvu = lstObjHM.FirstOrDefault().TTHAI_NVU;
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, _function);

                lblTrangThai.Content = BusinessConstant.layNgonNguSuDung(lstObjHM.FirstOrDefault().TTHAI_BGHI);
                txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(lstObjHM.FirstOrDefault().TTHAI_BGHI);
                teldtNgayCNhat.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                txtNguoiCapNhat.Text = ClientInformation.TenDangNhap;
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
            }
        }
        #endregion

        #region Action Delete

        public void BeforeDelete()
        {
            try
            {
                // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {

                    _action = DatabaseConstant.Action.XOA;
                    OnDelete();

                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnDelete()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            HanMucProcess process = new HanMucProcess();
            try
            {
                bool ret = false;
                List<DC_HAN_MUC> lstObjHM = new List<DC_HAN_MUC>();

                GetFormData(ref lstObjHM);

                ret = new HanMucProcess().HanMucChung(DatabaseConstant.Action.XOA, ref lstObjHM, ref listClientResponseDetail);
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
                if (ret == true)
                {
                    LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    //LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                }
                                
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
        
        #endregion
    }
}