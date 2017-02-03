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
using Presentation.Process.TaiSanDamBaoServiceRef;
using Presentation.Process.BaoCaoServiceRef;

namespace PresentationWPF.TaiSanDamBao.TaiSanDamBao
{
    /// <summary>
    /// Interaction logic for ucHopDongTheChapCT.xaml
    /// </summary>
    public partial class ucHopDongTheChapCT : UserControl
    {
        #region Khai bao         
        
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
        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }
        private List<AutoCompleteEntry> lstLoaiHDTC = new List<AutoCompleteEntry>();
        private string formCase = Presentation.Process.Common.ClientInformation.FormCase;
        private TDVM_HOP_DONG_TCHAP objHDTC = null;
        List<TDVM_TAI_SAN_DAM_BAO> lstTSDB = null;
        int idKHang = 0;
        int idHDTC = 0;
        int idBLanh = 0;
        string maHDTC = "";
        string soPhuLucTC = "";
        List<int> lstIDXoa = null;
        string tThai_NVu = "";
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        string maDViTao = "";
        string maDviQLy = "";
        #endregion

        #region Khoi tao
        public ucHopDongTheChapCT()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();
            InitEventHanler();
            KhoiTaoComboBox();
            ResetForm();
        }

        public void InitEventHanler()
        {
            btnSoHopDong.Click += new RoutedEventHandler(btnSoHopDong_Click);
            btnBenBL.Click += new RoutedEventHandler(btnBenBL_Click);
            btnMaKH.Click += new RoutedEventHandler(btnMaKH_Click);
            cmbLoaiHD.SelectionChanged +=new SelectionChangedEventHandler(cmbLoaiHD_SelectionChanged);
            tlbDetailAdd.Click += new RoutedEventHandler(tlbDetailAdd_Click);
            tlbDetailDel.Click += new RoutedEventHandler(tlbDetailDel_Click);
            this.KeyDown += new KeyEventHandler(UserControl_KeyDown);
            this.Loaded += new RoutedEventHandler(UserControl_Loaded);
            this.Unloaded += new RoutedEventHandler(UserControl_Unloaded);
        }

        public void KhoiTaoComboBox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            string sMaTruyVan = "";
            sMaTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.HINH_THUC_THE_CHAP.getValue());
            auto.GenAutoComboBox(ref lstLoaiHDTC, ref cmbLoaiHD, sMaTruyVan, lstDieuKien);
        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.HopDongTheChap;component/HopDongTheChap/ucHopDongTheChapCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        public ucHopDongTheChapCT(DatabaseConstant.Action _action, string _formCase, int _idHDTC, string _maHDTC) : this()
        {
            action = _action;
            formCase = _formCase;
            if (!formCase.Equals("PLUC_HD"))
                formCase = "HOP_DONG";
            idHDTC = _idHDTC;
            maHDTC = _maHDTC;

            if (action.Equals(DatabaseConstant.Action.THEM))
            {
                ResetForm();
                SetEnabledControls(true);
            }
            else
            {
                SetFormData();
                if (action.Equals(DatabaseConstant.Action.SUA))
                {
                    SetEnabledControls(true);
                }
                else
                    SetEnabledControls(false);
            }
            CommonFunction.RefreshButton(Toolbar, action, tThai_NVu, mnuMain, DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP);
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(ModifyCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.Shift);
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TINH_TOAN)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.T, ModifierKeys.Control | ModifierKeys.Shift);
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
            //OnHold();
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tThai_NVu = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
            OnSave();
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

        private void CaculateCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CaculateCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //TinhDuChi();
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
                tThai_NVu = BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri();
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                tThai_NVu = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                OnSave();
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
                //OnPreview();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TINH_TOAN)))
            {
                //TinhDuChi();
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
                //OnHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                OnSave();
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TINH_TOAN)))
            {
                //TinhDuChi();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
                //OnPreview();
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
            if (formCase.IsNullOrEmptyOrSpace())
                formCase = ClientInformation.FormCase;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            UnLock();
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
            UnLock();
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

        private void ResetForm()
        {
            AutoCompleteEntry auLoaiHopDong = lstLoaiHDTC.ElementAt(cmbLoaiHD.SelectedIndex);
            if (!auLoaiHopDong.KeywordStrings.FirstOrDefault().Equals("BAO_LANH"))
            {
                lblTTinBenBaoLanh.Visibility = System.Windows.Visibility.Collapsed;
                txtBenBaoLanh.Visibility = System.Windows.Visibility.Collapsed;
                btnBenBL.Visibility = System.Windows.Visibility.Collapsed;
                stpBaoLanh.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                lblTTinBenBaoLanh.Visibility = System.Windows.Visibility.Visible;
                txtBenBaoLanh.Visibility = System.Windows.Visibility.Visible;
                btnBenBL.Visibility = System.Windows.Visibility.Visible;
                stpBaoLanh.Visibility = System.Windows.Visibility.Visible;
            }
            txtSoHopDong.Text = "";
            txtMaKH.Text = "";
            teldtNgayHieuLuc.Value = ClientInformation.NgayLamViecHienTai.StringToDate(ApplicationConstant.defaultDateTimeFormat);
            teldtNgayHetHieuLuc.Value = null;
            txtBenBaoLanh.Text = "";
            lblTTinBenBaoLanh.Content = "";
            lblTTinKHang.Content = "";
            objHDTC = new TDVM_HOP_DONG_TCHAP();
            lstTSDB = new List<TDVM_TAI_SAN_DAM_BAO>();
            raddgrDSachHanMuc.ItemsSource = null;
            raddgrDSachHanMuc.ItemsSource = lstTSDB;
            lstIDXoa = new List<int>();
            maDViTao = "";
            maDviQLy = ClientInformation.MaDonVi;
            if (formCase.Equals("PLUC_HD"))
            {
                txtSoHopDong.IsEnabled = true;
                txtSoHopDong.Margin = new Thickness(0, 0, 35, 0);
                btnSoHopDong.Visibility = Visibility.Visible;
                cmbLoaiHD.IsEnabled = false;
                txtMaKH.IsEnabled = false;
                btnMaKH.IsEnabled = false;
                teldtNgayHieuLuc.IsEnabled = false;
                dtpNgayHieuLuc.IsEnabled = false;
                txtBenBaoLanh.IsEnabled = false;
                btnBenBL.IsEnabled = false;
            }
            else
            {
                txtSoHopDong.IsEnabled = false;
                txtSoHopDong.Margin = new Thickness(0, 0, 0, 0);
                btnSoHopDong.Visibility = Visibility.Collapsed;
            }
            tThai_NVu = "";
            lblTrangThai.Content = "";
            txtNguoiCapNhat.Text = "";
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            teldtNgayCNhat.Value = null;
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            txtTrangThai.Text = "";
            action = DatabaseConstant.Action.THEM;
            CommonFunction.RefreshButton(Toolbar, action, tThai_NVu, mnuMain, DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP);
        }

        void cmbLoaiHD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoCompleteEntry auLoaiHopDong = lstLoaiHDTC.ElementAt(cmbLoaiHD.SelectedIndex);
            if (!auLoaiHopDong.KeywordStrings.FirstOrDefault().Equals("BAO_LANH"))
            {
                lblTTinBenBaoLanh.Visibility = System.Windows.Visibility.Collapsed;
                txtBenBaoLanh.Visibility = System.Windows.Visibility.Collapsed;
                btnBenBL.Visibility = System.Windows.Visibility.Collapsed;
                stpBaoLanh.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                lblTTinBenBaoLanh.Visibility = System.Windows.Visibility.Visible;
                txtBenBaoLanh.Visibility = System.Windows.Visibility.Visible;
                btnBenBL.Visibility = System.Windows.Visibility.Visible;
                stpBaoLanh.Visibility = System.Windows.Visibility.Visible;
            }
        }

        void btnMaKH_Click(object sender, RoutedEventArgs e)
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
            win.ShowDialog();
            if (lstPopup.Count > 0)
            {
                txtMaKH.Text = lstPopup[0][2].ToString();
                idKHang = Convert.ToInt32(lstPopup[0][1]);
                lblTTinKHang.Content = lstPopup[0][3].ToString();
            }
        }

        void btnBenBL_Click(object sender, RoutedEventArgs e)
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
            win.ShowDialog();
            if (lstPopup.Count > 0)
            {
                txtBenBaoLanh.Text = lstPopup[0][2].ToString();
                idBLanh = Convert.ToInt32(lstPopup[0][1]);
                lblTTinBenBaoLanh.Content = lstPopup[0][3].ToString();
            }
        }

        void btnSoHopDong_Click(object sender, RoutedEventArgs e)
        {
            lstPopup.Clear();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
            lstDieuKien.Add("(0)");
            lstDieuKien.Add("NULL");
            lstDieuKien.Add("HOP_DONG");
            lstDieuKien.Add("LAP_PLUC");
            lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation("POPUP_DS_HDTC", lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            ucPopup popup = new ucPopup(true, simplePopupResponse);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
            win.Content = popup;
            win.ShowDialog();
            if (lstPopup.Count > 0)
            {
                txtSoHopDong.Text = lstPopup[0][2].ToString();
                //idHDTC = Convert.ToInt32(lstPopup[0][1]);
                maHDTC = lstPopup[0][2].ToString();
                cmbLoaiHD.SelectedIndex = lstLoaiHDTC.IndexOf(lstLoaiHDTC.FirstOrDefault(f=>f.KeywordStrings.FirstOrDefault().Equals(lstPopup[0]["LOAI_HDTC"].ToString())));
                txtMaKH.Text = lstPopup[0]["MA_KHANG"].ToString();
                idKHang = Convert.ToInt32(lstPopup[0]["ID_KHANG"]);
                lblTTinKHang.Content = lstPopup[0]["TEN_KHANG"].ToString();
                teldtNgayHieuLuc.Value = LDateTime.StringToDate(lstPopup[0]["NGAY_HDTC"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                if (lstPopup[0]["ID_NGUOI_BLANH"] != DBNull.Value)
                {
                    txtBenBaoLanh.Text = lstPopup[0]["MA_NGUOI_BLANH"].ToString();
                    idBLanh = Convert.ToInt32(lstPopup[0]["ID_NGUOI_BLANH"]);
                    lblTTinBenBaoLanh.Content = lstPopup[0]["TEN_NGUOI_BLANH"].ToString();
                }
            }
        }

        void tlbDetailDel_Click(object sender, RoutedEventArgs e)
        {
            if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            List<int> idXoa = new List<int>();
            foreach (TDVM_TAI_SAN_DAM_BAO obj in raddgrDSachHanMuc.SelectedItems)
            {
                idXoa.Add(obj.ID);
            }
            lstTSDB.RemoveAll(f => idXoa.Contains(f.ID));
            raddgrDSachHanMuc.ItemsSource = null;
            raddgrDSachHanMuc.ItemsSource = lstTSDB;
        }

        void tlbDetailAdd_Click(object sender, RoutedEventArgs e)
        {
            lstPopup.Clear();
            List<string> lstDieuKien = new List<string>();
            if (LObject.IsNullOrEmpty(lstTSDB)) lstTSDB = new List<TDVM_TAI_SAN_DAM_BAO>();
            string lstIDTSDB = "";
            foreach (TDVM_TAI_SAN_DAM_BAO objTSDB in lstTSDB)
            {
                lstIDTSDB += "," + objTSDB.ID.ToString();
            }
            if (lstIDTSDB.Length > 0)
                lstIDTSDB = "(" + lstIDTSDB.Substring(1) + ")";
            else
                lstIDTSDB = "(0)";
            lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
            lstDieuKien.Add(lstIDTSDB);
            lstDieuKien.Add("NULL");
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation("POPUP_DS_TSDB_HDTC", lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            ucPopup popup = new ucPopup(true, simplePopupResponse, true);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
            win.Content = popup;
            win.ShowDialog();
            if (lstPopup.Count > 0)
            {
                foreach (DataRow dr in lstPopup)
                {
                    TDVM_TAI_SAN_DAM_BAO objTSDB = new TDVM_TAI_SAN_DAM_BAO();
                    objTSDB.ID = Convert.ToInt32(dr["ID"]);
                    objTSDB.ID_HDTC = idHDTC;
                    objTSDB.ID_KHANG = Convert.ToInt32(dr["ID_KHANG"]);
                    objTSDB.ID_LOAI_TSDB = Convert.ToInt32(dr["ID_LOAI_TSDB"]);
                    objTSDB.MA_CHU_TSAN = dr["MA_CHU_TSAN"].ToString();
                    objTSDB.MA_LOAI_TSDB = dr["MA_LOAI_TSDB"].ToString();
                    objTSDB.MA_TSDB = dr["MA_TSDB"].ToString();
                    objTSDB.TEN_TSDB = dr["TEN_TSDB"].ToString();
                    objTSDB.TEN_CHU_TSAN = dr["TEN_CHU_TSAN"].ToString();
                    objTSDB.GTRI_DBAO_TOI_DA = Convert.ToDecimal(dr["GTRI_DBAO_TOI_DA"]);
                    objTSDB.GTRI_DGIA = Convert.ToDecimal(dr["GTRI_DGIA"]);
                    objTSDB.GTRI_HTOAN = Convert.ToDecimal(dr["GTRI_HTOAN"]);
                    objTSDB.SO_PLUC_HD = soPhuLucTC;
                    objTSDB.MA_DVI_QLY = dr["MA_DVI_QLY"].ToString();
                    lstTSDB.Add(objTSDB);
                }
                raddgrDSachHanMuc.ItemsSource = null;
                raddgrDSachHanMuc.ItemsSource = lstTSDB;
            }
        }
        #endregion               

        #region Xử lý nghiệp vụ
        private bool LockData()
        {
            List<int> lstId = new List<int>();
            lstId.Add(idHDTC);
            UtilitiesProcess process = new UtilitiesProcess();
            return process.LockData(DatabaseConstant.Module.TSDB,
            DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP,
            DatabaseConstant.Table.TD_HDTC,
            action,
            lstId);
        }

        private bool UnLock()
        {
            List<int> lstId = new List<int>();
            lstId.Add(idHDTC);
            UtilitiesProcess process = new UtilitiesProcess();
            return process.UnlockData(DatabaseConstant.Module.TSDB,
            DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP,
            DatabaseConstant.Table.TD_HDTC,
            action,
            lstId);
        }

        private void GetFormData(ref TDVM_HOP_DONG_TCHAP obj, BusinessConstant.TrangThaiNghiepVu tthaiNVu)
        {
            try
            {
                AutoCompleteEntry auLoaiHDTC = lstLoaiHDTC.ElementAt(cmbLoaiHD.SelectedIndex);
                obj = new TDVM_HOP_DONG_TCHAP();
                obj.ID_KHANG = idKHang;
                obj.ID_NGUOI_BLANH = idBLanh;
                obj.ID = idHDTC;
                obj.LOAI_HD = formCase;
                obj.LOAI_HDTC = auLoaiHDTC.KeywordStrings.FirstOrDefault();
                obj.MA_HDTC = txtSoHopDong.Text;
                obj.MA_KHANG = txtMaKH.Text;
                obj.MA_NGUOI_BLANH = txtBenBaoLanh.Text;
                obj.MO_TA = "";
                obj.NGAY_HDTC = teldtNgayHieuLuc.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                obj.NGAY_HET_HLUC = teldtNgayHetHieuLuc.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                lstTSDB = raddgrDSachHanMuc.ItemsSource as List<TDVM_TAI_SAN_DAM_BAO>;
                obj.TONG_GIA_TRI = lstTSDB.Sum(f => f.GTRI_DBAO_TOI_DA);
                obj.SO_PLUC_HD = soPhuLucTC;
                if (idHDTC == 0)
                {
                    obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                    obj.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    obj.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                }
                else
                {
                    obj.MA_DVI_QLY = maDviQLy;
                    obj.MA_DVI_TAO = maDViTao;
                    obj.NGAY_NHAP = teldtNgayNhap.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                    obj.NGUOI_NHAP = txtNguoiLap.Text;
                    obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                }
                obj.TTHAI_NVU = tthaiNVu.layGiaTri();
                obj.DSACH_TAI_SAN_DBAO = lstTSDB.Where(f => f.ID_HDTC == idHDTC).ToArray();
                if (lstIDXoa.IsNullOrEmpty()) lstIDXoa = new List<int>();
                obj.DSACH_ID_XOA = lstIDXoa.ToArray();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            lstTSDB = new List<TDVM_TAI_SAN_DAM_BAO>();
            try
            {
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@INP_IDHDTC", "string", idHDTC.ToString());
                LDatatable.AddParameter(ref dt, "@MA_HDTC", "string", maHDTC);
                LDatatable.AddParameter(ref dt, "@SO_PLUC_HD", "string", soPhuLucTC);
                DataSet ds = new TaiSanDamBaoProcess().GetHopDongTheChap(dt);
                if (!ds.IsNullOrEmpty() && ds.Tables.Count > 1)
                {
                    dt = ds.Tables["TTIN_CTIET"];
                    if (dt.Rows[0]["ID_NGUOI_BLANH"] != DBNull.Value)
                        idBLanh = Convert.ToInt32(dt.Rows[0]["ID_NGUOI_BLANH"]);
                    idHDTC = Convert.ToInt32(dt.Rows[0]["ID"]);
                    idKHang = Convert.ToInt32(dt.Rows[0]["ID_KHANG"]);
                    txtSoHopDong.Text = dt.Rows[0]["MA_HDTC"].ToString();
                    cmbLoaiHD.SelectedIndex = lstLoaiHDTC.IndexOf(lstLoaiHDTC.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dt.Rows[0]["LOAI_HDTC"].ToString())));
                    txtMaKH.Text = dt.Rows[0]["MA_KHANG"].ToString();
                    lblTTinKHang.Content = dt.Rows[0]["TEN_KHANG"].ToString();
                    teldtNgayHieuLuc.Value = dt.Rows[0]["NGAY_HDTC"].ToString().StringToDate(ApplicationConstant.defaultDateTimeFormat);
                    teldtNgayHetHieuLuc.Value = dt.Rows[0]["NGAY_HET_HLUC"].ToString().StringToDate(ApplicationConstant.defaultDateTimeFormat);
                    txtBenBaoLanh.Text = dt.Rows[0]["MA_NGUOI_BLANH"].ToString();
                    lblTTinBenBaoLanh.Content = dt.Rows[0]["TEN_NGUOI_BLANH"].ToString();
                    tThai_NVu = dt.Rows[0]["TTHAI_NVU"].ToString();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tThai_NVu);
                    maDviQLy = dt.Rows[0]["MA_DVI_QLY"].ToString();
                    maDViTao = dt.Rows[0]["MA_DVI_TAO"].ToString();
                    txtNguoiLap.Text = dt.Rows[0]["NGUOI_NHAP"].ToString();
                    if (dt.Rows[0]["NGUOI_CNHAT"] != DBNull.Value)
                        txtNguoiCapNhat.Text = dt.Rows[0]["NGUOI_CNHAT"].ToString();
                    teldtNgayNhap.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_NHAP"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    if (dt.Rows[0]["NGAY_CNHAT"] != DBNull.Value)
                        teldtNgayCNhat.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_CNHAT"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(dt.Rows[0]["TTHAI_BGHI"].ToString());
                    dt = ds.Tables["TSDB"];
                    
                    foreach (DataRow dr in dt.Rows)
                    {
                        TDVM_TAI_SAN_DAM_BAO objTSDB = new TDVM_TAI_SAN_DAM_BAO();
                        objTSDB.ID = Convert.ToInt32(dr["ID_TSDB"]);
                        objTSDB.ID_HDTC = Convert.ToInt32(dr["ID_HDTC"]);
                        objTSDB.ID_HDTC_TSDB = Convert.ToInt32(dr["ID_HDTC_TSDB"]);
                        objTSDB.ID_KHANG = Convert.ToInt32(dr["ID_KHANG"]);
                        objTSDB.ID_LOAI_TSDB = Convert.ToInt32(dr["ID_LOAI_TSDB"]);
                        objTSDB.MA_CHU_TSAN = dr["MA_CHU_TSAN"].ToString();
                        objTSDB.MA_LOAI_TSDB = dr["MA_LOAI_TSDB"].ToString();
                        objTSDB.MA_TSDB = dr["MA_TSDB"].ToString();
                        objTSDB.TEN_TSDB = dr["TEN_TSDB"].ToString();
                        objTSDB.TEN_CHU_TSAN = dr["TEN_CHU_TSAN"].ToString();
                        objTSDB.GTRI_DBAO_TOI_DA = Convert.ToDecimal(dr["GTRI_DBAO_TOI_DA"]);
                        objTSDB.GTRI_DGIA = Convert.ToDecimal(dr["GTRI_DGIA"]);
                        objTSDB.GTRI_HTOAN = Convert.ToDecimal(dr["GTRI_HTOAN"]);
                        objTSDB.SO_PLUC_HD = dr["SO_PLUC_HD"].ToString();
                        objTSDB.MA_DVI_QLY = dr["MA_DVI_QLY"].ToString();
                        lstTSDB.Add(objTSDB);
                    }
                    raddgrDSachHanMuc.ItemsSource = null;
                    raddgrDSachHanMuc.ItemsSource = lstTSDB;
                }
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

        private void SetEnabledControls(bool bBool)
        {
            tlbDetailAdd.IsEnabled = bBool;
            tlbDetailDel.IsEnabled = bBool;
            raddgrDSachHanMuc.IsReadOnly = !bBool;
            grbThongTinHDong.IsEnabled = bBool;
        }

        private bool Validation()
        {
            if (formCase.Equals("PLUC_HD") && maHDTC.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblTDHopDongTC.Content.ToString());
                btnSoHopDong.Focus();
                return false;
            }
            else if (cmbLoaiHD.SelectedIndex < 0)
            {
                CommonFunction.ThongBaoTrong(lblTDLoaiHD.Content.ToString());
                cmbLoaiHD.Focus();
                return false;
            }
            else if (txtMaKH.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblTDKhachHang.Content.ToString());
                btnMaKH.Focus();
                return false;
            }
            else if (teldtNgayHieuLuc.Value.IsNullOrEmpty())
            {
                CommonFunction.ThongBaoTrong(lblTDNgayHieuLuc.Content.ToString());
                teldtNgayHieuLuc.Focus();
                return false;
            }
            else if (teldtNgayHetHieuLuc.Value.IsNullOrEmpty())
            {
                CommonFunction.ThongBaoTrong(lblTDNgayHetHieuLuc.Content.ToString());
                teldtNgayHetHieuLuc.Focus();
                return false;
            }
            else if (teldtNgayHieuLuc.Value >= teldtNgayHetHieuLuc.Value)
            {
                LMessage.ShowMessage("M.PresentationWPF.TaiSanDamBao.ucHopDongTheChapCT.NgayHetHLucLonHonNgayHDTC", LMessage.MessageBoxType.Error);
                teldtNgayHetHieuLuc.Focus();
                return false;
            }
            AutoCompleteEntry auLoaiHD = lstLoaiHDTC.ElementAt(cmbLoaiHD.SelectedIndex);
            if (auLoaiHD.KeywordStrings.FirstOrDefault().Equals("BAO_LANH") && idBLanh == 0)
            {
                CommonFunction.ThongBaoTrong(lblTDBenBLanh.Content.ToString());
                btnBenBL.Focus();
                return false;
            }
            lstTSDB = raddgrDSachHanMuc.ItemsSource as List<TDVM_TAI_SAN_DAM_BAO>;
            if (lstTSDB.IsNullOrEmpty() || lstTSDB.Count < 1)
            {
                CommonFunction.ThongBaoTrong(grbDSPhatVay.Header.ToString() + ":");
                tlbDetailAdd.Focus();
                return false;
            }
            return true;
        }

        public void OnHold()
        {
            try
            { 
               
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnSave()
        {
            if (!Validation())
                return;
            GetFormData(ref objHDTC, BusinessConstant.layTrangThaiNghiepVu(tThai_NVu));
            TaiSanDamBaoProcess processTSDB = new TaiSanDamBaoProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            bool ret = false;
            if (idHDTC == 0)
                ret = processTSDB.HopDongTheChap(DatabaseConstant.Action.THEM, ref objHDTC, ref listClientResponseDetail);
            else
                ret = processTSDB.HopDongTheChap(DatabaseConstant.Action.SUA, ref objHDTC, ref listClientResponseDetail);
            AfterSave(ret, listClientResponseDetail);
        }

        public void AfterSave(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(listClientResponseDetail);
            if (ret)
            {
                if (!cbMultiAdd.IsChecked.Value)
                {
                    idHDTC = objHDTC.ID;
                    maHDTC = objHDTC.MA_HDTC;
                    soPhuLucTC = objHDTC.SO_PLUC_HD;
                    SetEnabledControls(false);
                    SetFormData();
                    action = DatabaseConstant.Action.XEM;
                    CommonFunction.RefreshButton(Toolbar, action, tThai_NVu, mnuMain, DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP);
                }
                else
                {
                    SetEnabledControls(true);
                    ResetForm();
                }
            }
        }

        public void BeforeViewFromDetail()
        {
          
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

        public void OnAddNew(TDVM_HOP_DONG_TCHAP obj)
        {
            
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
                
            }
        }

        public void BeforeModifyFromDetail()
        {
            try
            {
                action = DatabaseConstant.Action.SUA;
                SetEnabledControls(true);
                CommonFunction.RefreshButton(Toolbar, action, tThai_NVu, mnuMain, DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void BeforeModifyFromList()
        {
            try
            {
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }                                         
        }

        public void OnModify(TDVM_HOP_DONG_TCHAP obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                action = DatabaseConstant.Action.SUA;
                LockData();
                SetEnabledControls(true);
                CommonFunction.RefreshButton(Toolbar, action, tThai_NVu, mnuMain, DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP);
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

        public void AfterModify(bool ret, TDVM_HOP_DONG_TCHAP obj, List<ClientResponseDetail> listClientResponseDetail)
        {

        }


        public void BeforeDelete()
        {
            
            OnDelete();
        }

        public void OnDelete()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TaiSanDamBaoProcess processTSDB = new TaiSanDamBaoProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;
                ret = LockData();
                List<int> lst = new List<int>();
                objHDTC = new TDVM_HOP_DONG_TCHAP();
                objHDTC.ID = idHDTC;
                objHDTC.MA_HDTC = maHDTC;
                lst.Add(idHDTC);
                objHDTC.DSACH_ID_XOA = lst.ToArray();
                if (ret)
                    ret = processTSDB.HopDongTheChap(DatabaseConstant.Action.XOA, ref objHDTC, ref listClientResponseDetail);
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
            }
        }

        public void AfterDelete(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(listClientResponseDetail);
            if (ret)
            {
                CommonFunction.CloseUserControl(this);
            }
        }

        public void BeforeApprove()
        {
            OnApprove();
        }

        public void OnApprove()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TaiSanDamBaoProcess processTSDB = new TaiSanDamBaoProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;
                action = DatabaseConstant.Action.DUYET;
                ret = LockData();
                List<int> lst = new List<int>();
                objHDTC = new TDVM_HOP_DONG_TCHAP();
                objHDTC.ID = idHDTC;
                objHDTC.MA_HDTC = maHDTC;
                lst.Add(idHDTC);
                objHDTC.DSACH_ID_XOA = lst.ToArray();
                if (ret)
                    ret = processTSDB.HopDongTheChap(action, ref objHDTC, ref listClientResponseDetail);
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
            }
        }

        public void AfterApprove(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(listClientResponseDetail);
            UnLock();
            if (ret)
            {
                
                SetFormData();
                action = DatabaseConstant.Action.XEM;
                CommonFunction.RefreshButton(Toolbar, action, tThai_NVu, mnuMain, DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP);
            }
        }

        public void BeforeRefuse()
        {
            OnRefuse();
        }

        public void OnRefuse()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TaiSanDamBaoProcess processTSDB = new TaiSanDamBaoProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;
                action = DatabaseConstant.Action.TU_CHOI_DUYET;
                ret = LockData();
                List<int> lst = new List<int>();
                objHDTC = new TDVM_HOP_DONG_TCHAP();
                objHDTC.ID = idHDTC;
                objHDTC.MA_HDTC = maHDTC;
                lst.Add(idHDTC);
                objHDTC.DSACH_ID_XOA = lst.ToArray();
                if (ret)
                    ret = processTSDB.HopDongTheChap(action, ref objHDTC, ref listClientResponseDetail);
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
            }
        }

        public void AfterRefuse(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(listClientResponseDetail);
            UnLock();
            if (ret)
            {
                SetFormData();
                action = DatabaseConstant.Action.XEM;
                CommonFunction.RefreshButton(Toolbar, action, tThai_NVu, mnuMain, DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP);
            }
        }

        public void BeforeCancel()
        {
            OnCancel();
        }

        public void OnCancel()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TaiSanDamBaoProcess processTSDB = new TaiSanDamBaoProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;
                action = DatabaseConstant.Action.THOAI_DUYET;
                ret = LockData();
                List<int> lst = new List<int>();
                objHDTC = new TDVM_HOP_DONG_TCHAP();
                objHDTC.ID = idHDTC;
                objHDTC.MA_HDTC = maHDTC;
                lst.Add(idHDTC);
                objHDTC.DSACH_ID_XOA = lst.ToArray();
                if (ret)
                    ret = processTSDB.HopDongTheChap(action, ref objHDTC, ref listClientResponseDetail);
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
            }
        }

        public void AfterCancel(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(listClientResponseDetail);
            UnLock();
            if (ret)
            {
                SetFormData();
                action = DatabaseConstant.Action.XEM;
                CommonFunction.RefreshButton(Toolbar, action, tThai_NVu, mnuMain, DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP);
            }
        }
        #endregion
    }
}
