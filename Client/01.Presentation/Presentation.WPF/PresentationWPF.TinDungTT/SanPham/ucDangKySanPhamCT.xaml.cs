using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Collections;
using System.Reflection;
using System.Data;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using PresentationWPF.CustomControl;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.TinDungTTServiceRef;
using Presentation.Process.QuanTriHeThongServiceRef;


namespace PresentationWPF.TinDungTT.SanPham
{

    public partial class ucDangKySanPhamCT : UserControl
    {

        #region Khai bao
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private DatabaseConstant.Function function = DatabaseConstant.Function.TD_SAN_PHAMTT;

        public event EventHandler OnSavingCompleted;


        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private TDTT_SAN_PHAM obj;
        public TDTT_SAN_PHAM Obj
        {
            get { return obj; }
            set { obj = value; }
        }        

        private DataTable dtTaiKhoanHachHoan = null;

        private string sTrangThaiNVu = "";
        
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhuongThucChoVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiLaiSuat = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTanSuat = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhuongPhapTinhLai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCoSoTinhLai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCanCuXetQuaHan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiHachToan = new List<AutoCompleteEntry>();

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

        //Biến dùng chung
        private int idLaiSuat = -1;        
        #endregion

        #region Khoi tao
        public ucDangKySanPhamCT()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();            

            LoadCombobox();

            ResetForm();

            InitEventHandler();            
        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDungTT;component/SanPham/ucDangKySanPhamCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void InitEventHandler()
        {
            cmbLoaiHachToan.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiHachToan_SelectionChanged);
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

            bool ret = process.UnlockData(DatabaseConstant.Module.TDTT,
                DatabaseConstant.Function.TD_SAN_PHAMTT,
                DatabaseConstant.Table.TD_SAN_PHAMTT,
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

        #region Load Combobox
        private void LoadCombobox()
        {

            COMBOBOX_DTO combo = null;
            List<string> lstDieuKien = null;
            List<COMBOBOX_DTO> lstCombobox = new List<COMBOBOX_DTO>();
            AutoComboBox auCombo = new AutoComboBox();

            try
            {

                //Loại tiền
                combo = new COMBOBOX_DTO();
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_LOAITIEN.getValue();
                combo.combobox = cmbLoaiTien;
                combo.lstSource = lstSourceLoaiTien;
                combo.maChon = ClientInformation.MaDongNoiTe;
                lstCombobox.Add(combo);

                //Combobox loại vay
                lstDieuKien = new List<string>();
                lstDieuKien.Add("THOI_HAN_VAY_VON");
                combo = new COMBOBOX_DTO();
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                combo.combobox = cmbLoaiVay;
                combo.lstSource = lstSourceLoaiVay;
                combo.lstDieuKien = lstDieuKien;
                combo.maChon = "NGAN_HAN";
                lstCombobox.Add(combo);

                //Combobox phương thức cho vay
                lstDieuKien = new List<string>();
                lstDieuKien.Add("PHUONG_THUC_VAY");
                combo = new COMBOBOX_DTO();
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                combo.combobox = cmbPhuongThucChoVay;
                combo.lstSource = lstSourcePhuongThucChoVay;
                combo.lstDieuKien = lstDieuKien;
                combo.maChon = "TUNG_LAN";
                lstCombobox.Add(combo);

                //Combobox loại lãi suất
                lstDieuKien = new List<string>();
                lstDieuKien.Add("LOAI_LAI_SUAT");
                combo = new COMBOBOX_DTO();
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                combo.combobox = cmbLoaiLaiSuat;
                combo.lstSource = lstSourceLoaiLaiSuat;
                combo.lstDieuKien = lstDieuKien;
                combo.maChon = "CO_DINH";
                lstCombobox.Add(combo);

                //Combobox tần suất đánh giá theo
                lstDieuKien = new List<string>();
                lstDieuKien.Add("TSUAT_DANH_GIA_LSUAT");
                combo = new COMBOBOX_DTO();
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                combo.combobox = cmbTheoLan;
                combo.lstSource = lstSourceTanSuat;
                combo.lstDieuKien = lstDieuKien;
                combo.maChon = "THANG";
                lstCombobox.Add(combo);

                //Combobox phương pháp tính lãi
                lstDieuKien = new List<string>();
                lstDieuKien.Add("PHUONG_PHAP_TINH_LAI");
                combo = new COMBOBOX_DTO();
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                combo.combobox = cmbPhuongPhapTinhLai;
                combo.lstSource = lstSourcePhuongPhapTinhLai;
                combo.lstDieuKien = lstDieuKien;
                combo.maChon = "TICH_SO";
                lstCombobox.Add(combo);

                //Combobox cơ sở tính lãi
                lstDieuKien = new List<string>();
                lstDieuKien.Add("CO_SO_TINH_LAI");
                combo = new COMBOBOX_DTO();
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                combo.combobox = cmbCoSoTinhLai;
                combo.lstSource = lstSourceCoSoTinhLai;
                combo.lstDieuKien = lstDieuKien;
                combo.maChon = "CS360360";
                lstCombobox.Add(combo);

                //Combobox căn cứ xét quá hạn
                lstDieuKien = new List<string>();
                lstDieuKien.Add("CAN_CU_XET_QUA_HAN");
                combo = new COMBOBOX_DTO();
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                combo.combobox = cmbCanCuXetQuaHan;
                combo.lstSource = lstSourceCanCuXetQuaHan;
                combo.lstDieuKien = lstDieuKien;
                combo.maChon = "NGAY_TRA_NO";
                lstCombobox.Add(combo);

                //Combobox tài khoản hạch toán
                lstDieuKien = new List<string>();
                lstDieuKien.Add("DON_VI_HACH_TOAN");
                combo = new COMBOBOX_DTO();
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                combo.combobox = cmbLoaiHachToan;
                combo.lstSource = lstSourceLoaiHachToan;
                combo.lstDieuKien = lstDieuKien;
                combo.maChon = DatabaseConstant.PHUONG_PHAP_HACH_TOAN.DOC_LAP.layGiaTri();
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
        #endregion

        #region Xử lý Popup
        private void btnMaLaiSuat_Click(object sender, RoutedEventArgs e)
        {
            PopupProcess popupProcess = new PopupProcess();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add("TDTT");
            lstDieuKien.Add(ClientInformation.MaDonVi);
            popupProcess.getPopupInformation("POPUP_DS_LAISUAT", lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            ucPopup popup = new ucPopup(false, simplePopupResponse);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.Content = popup;
            win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ShowDialog();
            if (lstPopup.Count > 0)
            {
                idLaiSuat = Convert.ToInt32(lstPopup[0]["ID"]);

                LayThongTinLSuat();
            }
        }

        private void txtMaLaiSuat_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtMaLaiSuat.Text.IsNullOrEmptyOrSpace())
                idLaiSuat = 0;
            else
            {
                DataSet ds = new LaiSuatProcess().GetDSLaiSuat(ClientInformation.MaDonVi);
                List<DataRow> lstdr = ds.Tables["LAI_SUAT_DS"].Select("MA_LSUAT='" + txtMaLaiSuat.Text + "' AND TTHAI_NVU='DDU'").ToList();
                if (!LObject.IsNullOrEmpty(lstdr) && lstdr.Count > 0)
                    idLaiSuat = Convert.ToInt32(lstdr[0]["ID"]);
                else
                    idLaiSuat = 0;
            }
            LayThongTinLSuat();
        }

        private void LayThongTinLSuat()
        {
            TinDungProcess tindungProcess = new TinDungProcess();
            DataSet dsLaiSuat = new DataSet();
            dsLaiSuat = tindungProcess.getLaiSuatByID(idLaiSuat.ToString());
            if (dsLaiSuat != null & dsLaiSuat.Tables[0].Rows.Count > 0)
            {
                txtMaLaiSuat.Text = dsLaiSuat.Tables[0].Rows[0]["MA_LSUAT"].ToString();
                lblTenLaiSuat.Content = dsLaiSuat.Tables[0].Rows[0]["MO_TA"].ToString();
            }
            else
            {
                txtMaLaiSuat.Text = "";
                lblTenLaiSuat.Content = "";
            }
        }
        #endregion

        #region Tab Tài khoản hạch toán
        private void LoadDuLieuTaiKhoanHachToan(string sDoiTuong, string maDonVi)
        {
            DataTable dt = new TinDungTTProcess().GetTaiKhoanHachToan(sDoiTuong, maDonVi).Tables["TAI_KHOAN_HACH_TOAN"];
            grdTKhoan.ItemsSource = dt.DefaultView;
        }

        private void PhanLoaiTK_KeyDown(object sender, KeyEventArgs e)
        {
            var txt = sender as TextBox;
            GridViewRow grrow = txt.ParentOfType<GridViewRow>();
            PhanLoaiTaiKhoan(grrow);
        }

        private void PhanLoaiTKBSO_KeyDown(object sender, KeyEventArgs e)
        {
            var txt = sender as TextBox;
            GridViewRow grrow = txt.ParentOfType<GridViewRow>();
            PhanLoaiTaiKhoanBSo(grrow);
        }        

        private void cmbLoaiHachToan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string PPHachToan = lstSourceLoaiHachToan.ElementAt(cmbLoaiHachToan.SelectedIndex).KeywordStrings.FirstOrDefault();            
            if (PPHachToan.Equals(DatabaseConstant.PHUONG_PHAP_HACH_TOAN.DOC_LAP.layGiaTri()))
            {
                grdTKhoan.Columns[6].IsVisible = true;
                grdTKhoan.Columns[7].IsVisible = true;
                grdTKhoan.Columns[8].IsVisible = false;
                grdTKhoan.Columns[9].IsVisible = false;
            }
            else
            {
                grdTKhoan.Columns[6].IsVisible = false;
                grdTKhoan.Columns[7].IsVisible = false;
                grdTKhoan.Columns[8].IsVisible = true;
                grdTKhoan.Columns[9].IsVisible = true;
            }
            if (ClientInformation.MaDonViGiaoDich.Equals(ClientInformation.MaDonVi))
            {
                grdTKhoan.Columns[6].IsVisible = true;
                grdTKhoan.Columns[7].IsVisible = true;
                grdTKhoan.Columns[8].IsVisible = true;
                grdTKhoan.Columns[9].IsVisible = true;
            }
        }

        private void PhanLoaiTK_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            GridViewRow grrow = btn.ParentOfType<GridViewRow>();
            PhanLoaiTaiKhoan(grrow);
        }

        private void PhanLoaiTK_LostFocus(object sender, RoutedEventArgs e)
        {
            var txt = sender as TextBox;

        }
       
        private void PhanLoaiTKBSO_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            GridViewRow grrow = btn.ParentOfType<GridViewRow>();
            PhanLoaiTaiKhoanBSo(grrow);
        }

        private void PhanLoaiTKBSO_LostFocus(object sender, RoutedEventArgs e)
        {
            var txt = sender as TextBox;
        }

        private void PhanLoaiTaiKhoan(GridViewRow grrow)
        {
            if (action == DatabaseConstant.Action.XEM)
                return;

            try
            {
                DataRowView drv = grrow.Item as DataRowView;
                string maKyHieu = drv["MA_KY_HIEU"].ToString();
                string maPhanLoai = "%";
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(maKyHieu);
                lstDieuKien.Add(maPhanLoai);
                lstDieuKien.Add(ClientInformation.MaDonVi);
                var process = new PopupProcess();
                lstPopup.Clear();
                process.getPopupInformation("POPUP_TAI_KHOAN_HACH_TOAN", lstDieuKien);

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

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
                    drv["MA_PLOAI"] = row[2].ToString();
                    drv["TEN_PLOAI"] = row[3].ToString();
                    drv["MA_PLOAI_BSO"] = row[2].ToString();
                    drv["TEN_PLOAI"] = row[3].ToString();
                    grdTKhoan.CurrentItem = drv;
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void PhanLoaiTaiKhoanBSo(GridViewRow grrow)
        {
            if (action == DatabaseConstant.Action.XEM)
                return;

            try
            {
                DataRowView drv = grrow.Item as DataRowView;
                string maKyHieu = drv["MA_KY_HIEU"].ToString();
                string maPhanLoai = "%";
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(maKyHieu);
                lstDieuKien.Add(maPhanLoai);
                lstDieuKien.Add(ClientInformation.MaDonVi);
                var process = new PopupProcess();
                lstPopup.Clear();
                process.getPopupInformation("POPUP_TAI_KHOAN_HACH_TOAN", lstDieuKien);

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

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
                    drv["MA_PLOAI_BSO"] = row[2].ToString();
                    drv["TEN_PLOAI_BSO"] = row[3].ToString();
                    grdTKhoan.CurrentItem = drv;
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
        #endregion
        #endregion

        #region Xử lý nghiệp vụ
        private void GetFormData(ref TDTT_SAN_PHAM obj, string sTrangThaiNVu)
        {
            try
            {
                obj = new TDTT_SAN_PHAM();
                obj.OBJ_TD_SAN_PHAMTT = new TD_SAN_PHAMTT();
                List<KT_PHAN_HE_PLOAI> lstPhanHePLoai = new List<KT_PHAN_HE_PLOAI>();

                obj.ID = id;
                obj.MA_SAN_PHAM = txtMaSanPham.Text.Trim();
                obj.MA_DVI = ClientInformation.MaDonVi;
                obj.MA_DVI_GDICH = ClientInformation.MaDonViGiaoDich;
                obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.TTHAI_NVU = sTrangThaiNVu;

                #region TD_SAN_PHAMTT
                obj.OBJ_TD_SAN_PHAMTT.ID = id;
                obj.OBJ_TD_SAN_PHAMTT.MA_SAN_PHAM = txtMaSanPham.Text.Trim();
                obj.OBJ_TD_SAN_PHAMTT.TEN_SAN_PHAM = txtTenSanPham.Text.Trim();
                obj.OBJ_TD_SAN_PHAMTT.MA_LOAI_VAY = lstSourceLoaiVay[cmbLoaiVay.SelectedIndex].KeywordStrings[0];
                obj.OBJ_TD_SAN_PHAMTT.MA_PHUONG_THUC_CHO_VAY = lstSourcePhuongThucChoVay[cmbPhuongThucChoVay.SelectedIndex].KeywordStrings[0];
                obj.OBJ_TD_SAN_PHAMTT.MA_LOAI_LSUAT = lstSourceLoaiLaiSuat[cmbLoaiLaiSuat.SelectedIndex].KeywordStrings[0];
                obj.OBJ_TD_SAN_PHAMTT.ID_LSUAT = idLaiSuat;
                obj.OBJ_TD_SAN_PHAMTT.MA_LSUAT = txtMaLaiSuat.Text.Trim();
                obj.OBJ_TD_SAN_PHAMTT.BIEN_DO = Convert.ToDecimal(numBienDo.Value);
                obj.OBJ_TD_SAN_PHAMTT.TAN_SUAT_DANH_GIA = Convert.ToInt32(numTanSuatDanhGia.Value);
                obj.OBJ_TD_SAN_PHAMTT.TAN_SUAT_DANH_GIA_THEO = lstSourceTanSuat[cmbTheoLan.SelectedIndex].KeywordStrings[0];
                obj.OBJ_TD_SAN_PHAMTT.MA_PPHAP_TINH_LAI = lstSourcePhuongPhapTinhLai[cmbPhuongPhapTinhLai.SelectedIndex].KeywordStrings[0];
                obj.OBJ_TD_SAN_PHAMTT.MA_CSO_TINH_LAI = lstSourceCoSoTinhLai[cmbCoSoTinhLai.SelectedIndex].KeywordStrings[0];
                obj.OBJ_TD_SAN_PHAMTT.MA_CAN_CU_XET_QUA_HAN = lstSourceCanCuXetQuaHan[cmbCanCuXetQuaHan.SelectedIndex].KeywordStrings[0];
                obj.OBJ_TD_SAN_PHAMTT.TY_LE_LAI_QUA_HAN = Convert.ToDecimal(numTyLeLaiQuaHan.Value);
                if (teldtNgayHieuLuc.Value != null)
                {
                    obj.OBJ_TD_SAN_PHAMTT.NGAY_HIEU_LUC = teldtNgayHieuLuc.Value.Value.ToString("yyyyMMdd");
                }

                if (teldtNgayHetHieuLuc.Value != null)
                {
                    obj.OBJ_TD_SAN_PHAMTT.NGAY_HET_HIEU_LUC = teldtNgayHetHieuLuc.Value.Value.ToString("yyyyMMdd");
                }

                obj.OBJ_TD_SAN_PHAMTT.LOAI_TIEN = lstSourceLoaiTien[cmbLoaiTien.SelectedIndex].KeywordStrings[0];
                obj.OBJ_TD_SAN_PHAMTT.TTHAI_BGHI = obj.TTHAI_BGHI;
                obj.OBJ_TD_SAN_PHAMTT.TTHAI_NVU = obj.TTHAI_NVU;
                obj.OBJ_TD_SAN_PHAMTT.MA_DVI_QLY = ClientInformation.MaDonVi;
                obj.OBJ_TD_SAN_PHAMTT.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                obj.OBJ_TD_SAN_PHAMTT.NGUOI_NHAP = txtNguoiLap.Text;
                obj.OBJ_TD_SAN_PHAMTT.NGAY_NHAP = Convert.ToDateTime(teldtNgayNhap.Value).ToString("yyyyMMdd");
                if (action != DatabaseConstant.Action.THEM)
                {
                    obj.OBJ_TD_SAN_PHAMTT.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    obj.OBJ_TD_SAN_PHAMTT.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }
                
                #endregion

                #region LIST KT_PHAN_HE_PLOAI
                DataView dv = (DataView)grdTKhoan.ItemsSource;
                foreach (DataRowView drv in dv)
                {
                    KT_PHAN_HE_PLOAI objPhanHePLoai = new KT_PHAN_HE_PLOAI();
                    objPhanHePLoai.ID_PHAN_HE = 0;
                    objPhanHePLoai.ID = Convert.ToInt32(drv["ID"]);
                    objPhanHePLoai.MA_DTUONG = txtMaSanPham.Text;
                    objPhanHePLoai.MA_PHAN_HE = DatabaseConstant.Module.TDTT.getValue();
                    objPhanHePLoai.MA_KY_HIEU = drv["MA_KY_HIEU"].ToString();
                    objPhanHePLoai.MA_PLOAI = drv["MA_PLOAI"].ToString();
                    objPhanHePLoai.MA_PLOAI_BSO = drv["MA_PLOAI_BSO"].ToString();
                    objPhanHePLoai.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    objPhanHePLoai.TTHAI_NVU = sTrangThaiNVu;
                    objPhanHePLoai.MA_DVI_QLY = obj.OBJ_TD_SAN_PHAMTT.MA_DVI_QLY;
                    objPhanHePLoai.MA_DVI_TAO = obj.OBJ_TD_SAN_PHAMTT.MA_DVI_TAO;
                    objPhanHePLoai.NGUOI_NHAP = obj.OBJ_TD_SAN_PHAMTT.NGUOI_NHAP;
                    objPhanHePLoai.NGAY_NHAP = obj.OBJ_TD_SAN_PHAMTT.NGAY_NHAP;
                    objPhanHePLoai.NGUOI_CNHAT = obj.OBJ_TD_SAN_PHAMTT.NGUOI_CNHAT;
                    objPhanHePLoai.NGAY_CNHAT = obj.OBJ_TD_SAN_PHAMTT.NGAY_CNHAT;
                    lstPhanHePLoai.Add(objPhanHePLoai);
                }
                obj.LIST_KT_PHAN_HE_PLOAI = lstPhanHePLoai.ToArray();
                #endregion
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            TinDungTTProcess processTinDungTT = new TinDungTTProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            obj = new TDTT_SAN_PHAM();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processTinDungTT.SanPham(DatabaseConstant.Action.LOAD, ref obj, ref listClientResponseDetail);
                if (ret == true)
                {
                    if (obj != null && obj.OBJ_TD_SAN_PHAMTT != null)
                    {
                        sTrangThaiNVu = obj.TTHAI_NVU;
                        idLaiSuat = (int)obj.OBJ_TD_SAN_PHAMTT.ID_LSUAT;

                        #region tab Thông tin chung
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        txtMaSanPham.Text = obj.OBJ_TD_SAN_PHAMTT.MA_SAN_PHAM;
                        txtTenSanPham.Text = obj.OBJ_TD_SAN_PHAMTT.TEN_SAN_PHAM;

                        cmbLoaiTien.SelectedIndex = lstSourceLoaiTien.IndexOf(lstSourceLoaiTien.FirstOrDefault(i => i.KeywordStrings[0].Equals(obj.OBJ_TD_SAN_PHAMTT.LOAI_TIEN)));
                        cmbLoaiVay.SelectedIndex = lstSourceLoaiVay.IndexOf(lstSourceLoaiVay.FirstOrDefault(i => i.KeywordStrings[0].Equals(obj.OBJ_TD_SAN_PHAMTT.MA_LOAI_VAY)));
                        cmbPhuongThucChoVay.SelectedIndex = lstSourcePhuongThucChoVay.IndexOf(lstSourcePhuongThucChoVay.FirstOrDefault(i => i.KeywordStrings[0].Equals(obj.OBJ_TD_SAN_PHAMTT.MA_PHUONG_THUC_CHO_VAY)));

                        if (obj.OBJ_TD_SAN_PHAMTT.MA_PPHAP_TINH_LAI != null)
                        {
                            cmbPhuongPhapTinhLai.SelectedIndex = lstSourcePhuongPhapTinhLai.IndexOf(lstSourcePhuongPhapTinhLai.FirstOrDefault(i => i.KeywordStrings[0].Equals(obj.OBJ_TD_SAN_PHAMTT.MA_PPHAP_TINH_LAI)));
                        }

                        cmbCoSoTinhLai.SelectedIndex = lstSourceCoSoTinhLai.IndexOf(lstSourceCoSoTinhLai.FirstOrDefault(i => i.KeywordStrings[0].Equals(obj.OBJ_TD_SAN_PHAMTT.MA_CSO_TINH_LAI)));

                        if (obj.OBJ_TD_SAN_PHAMTT.MA_CAN_CU_XET_QUA_HAN != null)
                        {
                            cmbCanCuXetQuaHan.SelectedIndex = lstSourceCanCuXetQuaHan.IndexOf(lstSourceCanCuXetQuaHan.FirstOrDefault(i => i.KeywordStrings[0].Equals(obj.OBJ_TD_SAN_PHAMTT.MA_CAN_CU_XET_QUA_HAN)));
                        }

                        if (obj.OBJ_TD_SAN_PHAMTT.NGAY_HIEU_LUC != null)
                            teldtNgayHieuLuc.Value = LDateTime.StringToDate(obj.OBJ_TD_SAN_PHAMTT.NGAY_HIEU_LUC, "yyyyMMdd");
                        else
                            teldtNgayHieuLuc.Value = null;

                        if (obj.OBJ_TD_SAN_PHAMTT.NGAY_HET_HIEU_LUC!= null)
                            teldtNgayHetHieuLuc.Value = LDateTime.StringToDate(obj.OBJ_TD_SAN_PHAMTT.NGAY_HET_HIEU_LUC, "yyyyMMdd");
                        else
                            teldtNgayHetHieuLuc.Value = null;
                        

                        if (obj.OBJ_TD_SAN_PHAMTT.MA_LOAI_LSUAT != null)
                        {
                            cmbLoaiLaiSuat.SelectedIndex = lstSourceLoaiLaiSuat.IndexOf(lstSourceLoaiLaiSuat.FirstOrDefault(i => i.KeywordStrings[0].Equals(obj.OBJ_TD_SAN_PHAMTT.MA_LOAI_LSUAT)));
                        }

                        cmbTheoLan.SelectedIndex = lstSourceTanSuat.IndexOf(lstSourceTanSuat.FirstOrDefault(i => i.KeywordStrings[0].Equals(obj.OBJ_TD_SAN_PHAMTT.TAN_SUAT_DANH_GIA_THEO)));

                        txtMaLaiSuat.Text = obj.OBJ_TD_SAN_PHAMTT.MA_LSUAT;
                        lblTenLaiSuat.Content = obj.TEN_LAI_SUAT;

                        if (obj.OBJ_TD_SAN_PHAMTT.TY_LE_LAI_QUA_HAN != null)
                        {
                            numTyLeLaiQuaHan.Value = Convert.ToDouble(obj.OBJ_TD_SAN_PHAMTT.TY_LE_LAI_QUA_HAN);
                        }

                        if (obj.OBJ_TD_SAN_PHAMTT.BIEN_DO != null)
                        {
                            numBienDo.Value = Convert.ToDouble(obj.OBJ_TD_SAN_PHAMTT.BIEN_DO);
                        }

                        if (obj.OBJ_TD_SAN_PHAMTT.TAN_SUAT_DANH_GIA != null)
                        {
                            numTanSuatDanhGia.Value = Convert.ToDouble(obj.OBJ_TD_SAN_PHAMTT.TAN_SUAT_DANH_GIA);
                        }
                        #endregion

                        #region tab Tài khoản hạch toán
                        LoadDuLieuTaiKhoanHachToan(txtMaSanPham.Text.Trim(), obj.OBJ_TD_SAN_PHAMTT.MA_DVI_QLY);
                        #endregion

                        #region tab Thông tin kiểm soát
                        txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        teldtNgayNhap.Value = LDateTime.StringToDate(obj.NGAY_NHAP, "yyyyMMdd");
                        txtNguoiLap.Text = obj.NGUOI_NHAP;
                        if (!obj.NGAY_CNHAT.IsNullOrEmptyOrSpace())
                        {
                            teldtNgayCNhat.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, "yyyyMMdd");
                        }

                        if (!obj.NGUOI_CNHAT.IsNullOrEmptyOrSpace())
                        {
                            txtNguoiCapNhat.Text = obj.NGUOI_CNHAT;
                        }
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
            obj = null;
            id = 0;

            #region Thông tin chung
            txtMaSanPham.Text = "";
            txtTenSanPham.Text = "";
            cmbLoaiTien.SelectedIndex = lstSourceLoaiTien.IndexOf(lstSourceLoaiTien.FirstOrDefault(i => i.KeywordStrings[0].Equals(ClientInformation.MaDongNoiTe)));
            cmbLoaiVay.SelectedIndex = lstSourceLoaiVay.IndexOf(lstSourceLoaiVay.FirstOrDefault(i => i.KeywordStrings[0].Equals("NGAN_HAN")));
            cmbPhuongThucChoVay.SelectedIndex = lstSourcePhuongThucChoVay.IndexOf(lstSourcePhuongThucChoVay.FirstOrDefault(i => i.KeywordStrings[0].Equals("TUNG_LAN"))); ;
            cmbPhuongPhapTinhLai.SelectedIndex = lstSourcePhuongPhapTinhLai.IndexOf(lstSourcePhuongPhapTinhLai.FirstOrDefault(i => i.KeywordStrings[0].Equals("TICH_SO"))); ;
            cmbCoSoTinhLai.SelectedIndex = lstSourceCoSoTinhLai.IndexOf(lstSourceCoSoTinhLai.FirstOrDefault(i => i.KeywordStrings[0].Equals("CS360360"))); ;
            cmbCanCuXetQuaHan.SelectedIndex = lstSourceCanCuXetQuaHan.IndexOf(lstSourceCanCuXetQuaHan.FirstOrDefault(i => i.KeywordStrings[0].Equals("NGAY_TRA_NO"))); ;

            numTyLeLaiQuaHan.Value = lstSourceLoaiVay.IndexOf(lstSourceLoaiVay.FirstOrDefault(i => i.KeywordStrings[0].Equals("NGAN_HAN"))); ;
            teldtNgayHieuLuc.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            teldtNgayHetHieuLuc.Value = null;

            cmbLoaiLaiSuat.SelectedIndex = lstSourceLoaiLaiSuat.IndexOf(lstSourceLoaiLaiSuat.FirstOrDefault(i => i.KeywordStrings[0].Equals("CO_DINH"))); ;
            txtMaLaiSuat.Text = "";
            lblTenLaiSuat.Content = "Tên lãi suất";
            numBienDo.Value = 0;
            numTanSuatDanhGia.Value = 0;
            cmbTheoLan.SelectedIndex = 0;
            
            #endregion

            #region Thông tin kiểm soát
            txtTrangThai.Text = "";
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            teldtNgayCNhat.Value = null;
            txtNguoiCapNhat.Text = "";
            #endregion

            #region Tab tài khoản hạch toán
            cmbLoaiHachToan.SelectedIndex = lstSourceLoaiHachToan.IndexOf(lstSourceLoaiHachToan.FirstOrDefault(i => i.KeywordStrings[0].Equals(DatabaseConstant.PHUONG_PHAP_HACH_TOAN.DOC_LAP.layGiaTri())));
            LoadDuLieuTaiKhoanHachToan("MACDINH", ClientInformation.MaDonVi);
            #endregion

        }

        private bool Validation()
        {
            try
            {
                if (txtTenSanPham.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaChon(lblTenSPham.Content.ToString().Trim());
                    txtTenSanPham.Focus();
                    return false;
                }
                else if (txtMaLaiSuat.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaChon(lblMaLaiSuat.Content.ToString().Trim());
                    txtMaLaiSuat.Focus();
                    return false;
                }
                else
                {
                    DataView dv = (DataView)grdTKhoan.ItemsSource;
                    foreach (DataRowView drv in dv)
                    {
                        if (drv["MA_KY_HIEU"] == null || drv["MA_KY_HIEU"].ToString().IsNullOrEmptyOrSpace())
                        {
                            LMessage.ShowMessage("Chưa nhập mã ký hiệu hạch toán", LMessage.MessageBoxType.Warning);
                            return false;
                        }
                        else if (drv["MA_PLOAI"] == null || drv["MA_PLOAI"].ToString().IsNullOrEmptyOrSpace())
                        {
                            LMessage.ShowMessage("Chưa nhập mã phân loại hạch toán", LMessage.MessageBoxType.Warning);
                            return false;
                        }
                        else if (drv["MA_PLOAI_BSO"] == null || drv["MA_PLOAI_BSO"].ToString().IsNullOrEmptyOrSpace())
                        {
                            LMessage.ShowMessage("Chưa nhập mã phân loại hạch toán báo sổ", LMessage.MessageBoxType.Warning);
                            return false;
                        }
                    }
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
                grpThongTinChung.IsEnabled = true;
                grpLaiSuat.IsEnabled = true;
                cmbLoaiHachToan.IsEnabled = true;                                
                grdTKhoan.IsReadOnly = false;
            }
            #endregion

            #region Sửa
            else if (action == DatabaseConstant.Action.SUA)
            {
                grpThongTinChung.IsEnabled = true;
                grpLaiSuat.IsEnabled = true;                
                cmbLoaiHachToan.IsEnabled = true;
                grdTKhoan.IsReadOnly = false;
            }
            #endregion

            #region Xem ~ Mặc định
            else
            {
                grpThongTinChung.IsEnabled = false;
                grpLaiSuat.IsEnabled = false;                
                cmbLoaiHachToan.IsEnabled = false;
                grdTKhoan.IsReadOnly = true;
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

                obj = new TDTT_SAN_PHAM();

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

                obj = new TDTT_SAN_PHAM();

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

        public void OnAddNew(TDTT_SAN_PHAM obj)
        {

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TinDungTTProcess processTinDungTT = new TinDungTTProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processTinDungTT.SanPham(DatabaseConstant.Action.THEM, ref obj, ref listClientResponseDetail);
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

        public void AfterAddNew(bool ret, TDTT_SAN_PHAM obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                    if (cbMultiAdd.IsChecked == true)
                    {
                        ResetForm();
                    }
                    else
                    {
                        id = obj.ID;
                        txtMaSanPham.Text = obj.MA_SAN_PHAM;
                        sTrangThaiNVu = obj.TTHAI_NVU;

                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);                        

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

                bool ret = process.LockData(DatabaseConstant.Module.TDTT,
                    DatabaseConstant.Function.TD_SAN_PHAMTT,
                    DatabaseConstant.Table.TD_SAN_PHAMTT,
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

        public void OnModify(TDTT_SAN_PHAM obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TinDungTTProcess processTinDungTT = new TinDungTTProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processTinDungTT.SanPham(DatabaseConstant.Action.SUA, ref obj, ref listClientResponseDetail);
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

        public void AfterModify(bool ret, TDTT_SAN_PHAM obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);                                        

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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTT,
                    DatabaseConstant.Function.TD_SAN_PHAMTT,
                    DatabaseConstant.Table.TD_SAN_PHAMTT,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.TDTT,
                        DatabaseConstant.Function.TD_SAN_PHAMTT,
                        DatabaseConstant.Table.TD_SAN_PHAMTT,
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTT,
                    DatabaseConstant.Function.TD_SAN_PHAMTT,
                    DatabaseConstant.Table.TD_SAN_PHAMTT,
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
            TinDungTTProcess processTinDungTT = new TinDungTTProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processTinDungTT.SanPham(action, ref obj, ref listClientResponseDetail);
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
                processTinDungTT = null;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTT,
                    DatabaseConstant.Function.TD_SAN_PHAMTT,
                    DatabaseConstant.Table.TD_SAN_PHAMTT,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.TDTT,
                        DatabaseConstant.Function.TD_SAN_PHAMTT,
                        DatabaseConstant.Table.TD_SAN_PHAMTT,
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
            TinDungTTProcess processTinDungTT = new TinDungTTProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processTinDungTT.SanPham(action, ref obj, ref listClientResponseDetail);
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
                processTinDungTT = null;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTT,
                    DatabaseConstant.Function.TD_SAN_PHAMTT,
                    DatabaseConstant.Table.TD_SAN_PHAMTT,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.TDTT,
                        DatabaseConstant.Function.TD_SAN_PHAMTT,
                        DatabaseConstant.Table.TD_SAN_PHAMTT,
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
            TinDungTTProcess processTinDungTT = new TinDungTTProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processTinDungTT.SanPham(action, ref obj, ref listClientResponseDetail);
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
                processTinDungTT = null;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTT,
                    DatabaseConstant.Function.TD_SAN_PHAMTT,
                    DatabaseConstant.Table.TD_SAN_PHAMTT,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.TDTT,
                        DatabaseConstant.Function.TD_SAN_PHAMTT,
                        DatabaseConstant.Table.TD_SAN_PHAMTT,
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
            TinDungTTProcess processTinDungTT = new TinDungTTProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processTinDungTT.SanPham(action, ref obj, ref listClientResponseDetail);
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
                processTinDungTT = null;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTT,
                    DatabaseConstant.Function.TD_SAN_PHAMTT,
                    DatabaseConstant.Table.TD_SAN_PHAMTT,
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
