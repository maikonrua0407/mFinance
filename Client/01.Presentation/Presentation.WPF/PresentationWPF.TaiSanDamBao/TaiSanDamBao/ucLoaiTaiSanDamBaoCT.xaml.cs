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
using Telerik.Windows.Controls.GridView;
using Presentation.Process.TaiSanDamBaoServiceRef;

namespace PresentationWPF.TaiSanDamBao.TaiSanDamBao
{
    /// <summary>
    /// Interaction logic for ucDmNgheNghiepCT.xaml
    /// </summary>
    public partial class ucLoaiTaiSanDamBaoCT : UserControl
    {
        #region Khai bao
        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private DatabaseConstant.Function function = DatabaseConstant.Function.TD_TSDB_LOAI_CT;

        public event EventHandler OnSavingCompleted;

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private TD_TSAN_LOAI obj;
        public TD_TSAN_LOAI Obj
        {
            get { return obj; }
            set { obj = value; }
        }

        private string sTrangThaiNVu = "";


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


        List<AutoCompleteEntry> lstLoaiHachToan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstKyHieuHachToan = new List<AutoCompleteEntry>();
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        string maDonVi = "";
        string maDonViQLy = "";
        #endregion

        #region Khoi tao
        public ucLoaiTaiSanDamBaoCT()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            InitEventHanler();

            BindShortkey();

            KhoiTaoComboBox();

            txtMaLoaiTSDB.Focus();

        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TaiSanDamBao;component/TaiSanDamBao/ucLoaiTaiSanDamBaoCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        public void InitEventHanler()
        {
            cmbLoaiHachToan.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiHachToan_SelectionChanged);
            cmbKyHieuHToan.SelectionChanged += new SelectionChangedEventHandler(cmbKyHieuHToan_SelectionChanged);
        }

        void KhoiTaoComboBox()
        {
            List<string> lstDieuKien = new List<string>();
            List<string> lstMaChon = new List<string>();
            string maTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.DON_VI_HACH_TOAN));
            KhoiTaoGiaTriComboBox(ref lstLoaiHachToan, ref cmbLoaiHachToan, maTruyVan, lstDieuKien, DatabaseConstant.PHUONG_PHAP_HACH_TOAN.DOC_LAP.layGiaTri());

            lstDieuKien = new List<string>();
            maTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_KY_HIEU.getValue();
            KhoiTaoGiaTriComboBox(ref lstKyHieuHachToan, ref cmbKyHieuHToan, maTruyVan, lstDieuKien,"TSCCTC");
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

            bool ret = process.UnlockData(DatabaseConstant.Module.TSDB,
                DatabaseConstant.Function.TD_TSDB_LOAI_CT,
                DatabaseConstant.Table.TD_TSAN_LOAI,
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

        private void PhanLoaiTK_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            GridViewRow grrow = btn.ParentOfType<GridViewRow>();
            PhanLoaiTaiKhoan(grrow);
        }

        private void PhanLoaiTK_KeyDown(object sender, KeyEventArgs e)
        {
            var txt = sender as TextBox;
            GridViewRow grrow = txt.ParentOfType<GridViewRow>();
            PhanLoaiTaiKhoan(grrow);
        }

        private void PhanLoaiTK_LostFocus(object sender, RoutedEventArgs e)
        {
            var txt = sender as TextBox;

        }

        private void PhanLoaiTKBSO_LostFocus(object sender, RoutedEventArgs e)
        {
            var txt = sender as TextBox;
        }

        private void PhanLoaiTKBSO_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            GridViewRow grrow = btn.ParentOfType<GridViewRow>();
            PhanLoaiTaiKhoanBSo(grrow);
        }

        private void PhanLoaiTKBSO_KeyDown(object sender, KeyEventArgs e)
        {
            var txt = sender as TextBox;
            GridViewRow grrow = txt.ParentOfType<GridViewRow>();
            PhanLoaiTaiKhoanBSo(grrow);
        } 

        private void PhanLoaiTaiKhoan(GridViewRow grrow)
        {
            try
            {
                AutoCompleteEntry au = lstKyHieuHachToan.ElementAt(cmbKyHieuHToan.SelectedIndex);
                DataRowView drv = grrow.Item as DataRowView;
                string maKyHieu = au.KeywordStrings.FirstOrDefault();
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

        private void LoadDuLieuTaiKhoanHachToan(string sDoiTuong)
        {
            DataTable dt = new TaiSanDamBaoProcess().GetTaiKhoanHachToan(sDoiTuong, ClientInformation.MaDonVi).Tables["TAI_KHOAN_HACH_TOAN"];
            grdTKhoan.ItemsSource = dt.DefaultView;
        }

        private void cmbLoaiHachToan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string PPHachToan = lstLoaiHachToan.ElementAt(cmbLoaiHachToan.SelectedIndex).KeywordStrings.FirstOrDefault();
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

        void KhoiTaoGiaTriComboBox(ref List<AutoCompleteEntry> lstAutoComplete, ref RadComboBox cmbControl, string sMaTruyVan, List<string> lstDKien = null, string Chon = null, List<string> lstMaChon = null)
        {
            AutoComboBox autoComboBox = new AutoComboBox();
            autoComboBox.GenAutoComboBox(ref lstAutoComplete, ref cmbControl, sMaTruyVan, lstDKien, Chon, lstMaChon);
        }

        void cmbKyHieuHToan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        #endregion

        #region Xử lý nghiệp vụ

        private void GetFormData(ref TD_TSAN_LOAI obj, ref List<KT_PHAN_HE_PLOAI> lstPhanHe, string sTrangThaiNVu)
        {
            try
            {
                obj.ID = id;
                obj.MA_LOAI_TSDB = txtMaLoaiTSDB.Text;
                obj.TEN_LOAI_TSDB = txtTenLoaiTSDB.Text;
                obj.HE_SO_RRO = txtHeSoRuiRo.Value != null ? (decimal)txtHeSoRuiRo.Value : 0;
                obj.TY_LE_KHAU_TRU = txtTyLeKhauTru.Value != null ? (decimal)txtTyLeKhauTru.Value : 0;
                obj.TY_LE_VAY_TOI_DA = txtTyLeDamBaoToiDa.Value != null ? (decimal)txtTyLeDamBaoToiDa.Value : 0;

                #region Thông tin hạch toán
                DataView dv = (DataView)grdTKhoan.ItemsSource;
                foreach (DataRowView drv in dv)
                {
                    KT_PHAN_HE_PLOAI objPhanHePLoai = new KT_PHAN_HE_PLOAI();
                    objPhanHePLoai.ID_PHAN_HE = 0;
                    objPhanHePLoai.ID = Convert.ToInt32(drv["ID"]);
                    objPhanHePLoai.MA_DTUONG = txtMaLoaiTSDB.Text;
                    objPhanHePLoai.MA_PHAN_HE = DatabaseConstant.Module.TSDB.getValue();
                    objPhanHePLoai.MA_KY_HIEU = drv["MA_KY_HIEU"].ToString();
                    objPhanHePLoai.MA_PLOAI = drv["MA_PLOAI"].ToString();
                    objPhanHePLoai.MA_PLOAI_BSO = drv["MA_PLOAI_BSO"].ToString();
                    objPhanHePLoai.NGAY_NHAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                    objPhanHePLoai.NGUOI_NHAP = txtNguoiLap.Text;
                    objPhanHePLoai.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    objPhanHePLoai.TTHAI_NVU = sTrangThaiNVu;
                    objPhanHePLoai.MA_DVI_QLY = maDonViQLy;
                    objPhanHePLoai.MA_DVI_TAO = maDonVi;
                    if (action != DatabaseConstant.Action.THEM)
                    {
                        objPhanHePLoai.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                        objPhanHePLoai.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    }
                    lstPhanHe.Add(objPhanHePLoai);
                }
                #endregion

                #region Thông tin kiểm soát
                obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.TTHAI_NVU = sTrangThaiNVu;
                obj.MA_DVI_QLY = maDonViQLy;
                obj.MA_DVI_TAO = maDonVi;
                obj.NGAY_NHAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                obj.NGUOI_NHAP = txtNguoiLap.Text;
                if (action != DatabaseConstant.Action.THEM)
                {
                    obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }
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
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TaiSanDamBaoProcess processTaiSanDamBao = new TaiSanDamBaoProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                List<KT_PHAN_HE_PLOAI> lstPhanHe = new List<KT_PHAN_HE_PLOAI>();
                bool ret = false;

                obj = new TD_TSAN_LOAI();
                obj.ID = id;

                ret = processTaiSanDamBao.LoaiTS(DatabaseConstant.Action.LOAD_DATA, ref obj,ref lstPhanHe, ref listClientResponseDetail);
                if (ret == true)
                {
                    id = obj.ID;
                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    #region Thông tin chung
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(obj.TTHAI_NVU);
                    txtMaLoaiTSDB.Text = obj.MA_LOAI_TSDB;
                    txtTenLoaiTSDB.Text = obj.TEN_LOAI_TSDB;
                    txtHeSoRuiRo.Value = (double)obj.HE_SO_RRO;
                    txtTyLeKhauTru.Value = (double)obj.TY_LE_KHAU_TRU;
                    txtTyLeDamBaoToiDa.Value = (double)obj.TY_LE_VAY_TOI_DA;
                    maDonVi = obj.MA_DVI_TAO;
                    maDonViQLy = obj.MA_DVI_QLY;
                    #endregion

                    #region Thông tin hạch toán
                    LoadDuLieuTaiKhoanHachToan(txtMaLoaiTSDB.Text);
                    #endregion

                    #region Thông tin kiểm soát
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    raddtNgayLap.Value = LDateTime.StringToDate(obj.NGAY_NHAP, "yyyyMMdd");
                    txtNguoiLap.Text = obj.NGUOI_NHAP;
                    if (LDateTime.IsDate(obj.NGAY_CNHAT, "yyyyMMdd") == true)
                        raddtNgayCapNhat.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, "yyyyMMdd");
                    else
                        raddtNgayCapNhat.Value = null;
                    txtNguoiCapNhat.Text = obj.NGUOI_CNHAT;
                    #endregion

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
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void ResetForm()
        {
            //Biến
            obj = null;
            id = 0;

            //Thông tin chung
            lblTrangThai.Content = "";
            txtMaLoaiTSDB.Text = "";
            txtTenLoaiTSDB.Text = "";
            txtTyLeDamBaoToiDa.Value = 0;
            txtTyLeKhauTru.Value = 0;
            txtHeSoRuiRo.Value = 0;
            if (lstKyHieuHachToan.Count > 0)
                cmbKyHieuHToan.SelectedIndex = lstKyHieuHachToan.IndexOf(lstKyHieuHachToan.FirstOrDefault(f=>f.KeywordStrings.FirstOrDefault().Equals("TSCCTC")));
            //Thông tin hạch toán
            LoadDuLieuTaiKhoanHachToan("MACDINH");

            //Thông tin kiểm soát
            txtTrangThaiBanGhi.Text = "";
            raddtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            raddtNgayCapNhat.Value = null;
            txtNguoiCapNhat.Text = "";
            maDonVi = ClientInformation.MaDonViGiaoDich;
            maDonViQLy = ClientInformation.MaDonVi;
        }

        private void ResetData()
        {
            action = DatabaseConstant.Action.THEM;
            obj = null;
            id = 0;
            sTrangThaiNVu = "";

            ResetForm();
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);

            //chkThemNhieuLan.IsChecked = false;
        }

        private void SetEnabledControls()
        {
            #region Thêm
            if (action == DatabaseConstant.Action.THEM)
            {
                txtMaLoaiTSDB.IsEnabled = false;
                txtTenLoaiTSDB.IsEnabled = true;
                txtTyLeDamBaoToiDa.IsEnabled = true;
                txtTyLeKhauTru.IsEnabled = true;
                txtHeSoRuiRo.IsEnabled = true;
                grdTKhoan.IsEnabled = true;
            }
            #endregion

            #region Sửa
            else if (action == DatabaseConstant.Action.SUA)
            {
                txtMaLoaiTSDB.IsEnabled = false;
                txtTenLoaiTSDB.IsEnabled = true;
                txtTyLeDamBaoToiDa.IsEnabled = true;
                txtTyLeKhauTru.IsEnabled = true;
                txtHeSoRuiRo.IsEnabled = true;
                grdTKhoan.IsEnabled = true;
            }
            #endregion

            #region Xem ~ Mặc định
            else
            {
                txtMaLoaiTSDB.IsEnabled = false;
                txtTenLoaiTSDB.IsEnabled = false;
                txtTyLeDamBaoToiDa.IsEnabled = false;
                txtTyLeKhauTru.IsEnabled = false;
                txtHeSoRuiRo.IsEnabled = false;
                grdTKhoan.IsEnabled = false;
            }
            #endregion
        }

        private bool Validation()
        {
            try
            {
                if (txtTenLoaiTSDB.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaNhap(lblTenLoaiTSDB.Content.ToString());
                    txtTenLoaiTSDB.Focus();
                    return false;
                }

                if (txtTyLeDamBaoToiDa.Value>100)
                {
                    CommonFunction.ThongBaoLoi("Tỉ lệ đảm bảo tối đa không được vượt quá 100%");
                    txtTyLeDamBaoToiDa.Focus();
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

                obj = new TD_TSAN_LOAI();

                List<KT_PHAN_HE_PLOAI> lstPhanHe = new List<KT_PHAN_HE_PLOAI>();

                GetFormData(ref obj, ref lstPhanHe, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(obj, lstPhanHe);
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(obj, lstPhanHe);
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

                obj = new TD_TSAN_LOAI();
                List<KT_PHAN_HE_PLOAI> lstPhanHe = new List<KT_PHAN_HE_PLOAI>();

                GetFormData(ref obj, ref lstPhanHe, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(obj, lstPhanHe);
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(obj, lstPhanHe);
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
            txtMaLoaiTSDB.Text = obj.MA_LOAI_TSDB;
            #region Thông tin hạch toán
            LoadDuLieuTaiKhoanHachToan(txtMaLoaiTSDB.Text);
            #endregion
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
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


        public void BeforeAddNew()
        {
            action = DatabaseConstant.Action.THEM;
            ResetForm();
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnAddNew(TD_TSAN_LOAI obj, List<KT_PHAN_HE_PLOAI> lstPhanHe)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TaiSanDamBaoProcess processTaiSanDamBao = new TaiSanDamBaoProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processTaiSanDamBao.LoaiTS(DatabaseConstant.Action.THEM, ref obj,ref lstPhanHe, ref listClientResponseDetail);
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);
                }
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

        public void AfterAddNew(bool ret, TD_TSAN_LOAI obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {

                    id = obj.ID;
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
                    DatabaseConstant.Function.TD_TSDB_LOAI_CT,
                    DatabaseConstant.Table.TD_TSAN_LOAI,
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
            try
            {
                SetFormData();
                action = DatabaseConstant.Action.SUA;
                SetEnabledControls();
                CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnModify(TD_TSAN_LOAI obj, List<KT_PHAN_HE_PLOAI> lstPhanHe)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TaiSanDamBaoProcess processTaiSanDamBao = new TaiSanDamBaoProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processTaiSanDamBao.LoaiTS(DatabaseConstant.Action.SUA, ref obj,ref lstPhanHe, ref listClientResponseDetail);
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

        public void AfterModify(bool ret, TD_TSAN_LOAI obj, List<ClientResponseDetail> listClientResponseDetail)
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
                }

                // Yêu cầu Unlock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TSDB,
                    DatabaseConstant.Function.TD_TSDB_LOAI_CT,
                    DatabaseConstant.Table.TD_TSAN_LOAI,
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
                        DatabaseConstant.Function.TD_TSDB_LOAI_CT,
                        DatabaseConstant.Table.TD_TSAN_LOAI,
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
                    DatabaseConstant.Function.TD_TSDB_LOAI_CT,
                    DatabaseConstant.Table.TD_TSAN_LOAI,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnDelete()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                bool ret = false;
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                TaiSanDamBaoProcess processTaiSanDamBao = new TaiSanDamBaoProcess();
                List<KT_PHAN_HE_PLOAI> lstPhanHe = new List<KT_PHAN_HE_PLOAI>();
                ret = processTaiSanDamBao.LoaiTS(action, ref obj,ref lstPhanHe, ref listClientResponseDetail);
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
                    DatabaseConstant.Function.TD_TSDB_LOAI_CT,
                    DatabaseConstant.Table.TD_TSAN_LOAI,
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
                        DatabaseConstant.Function.TD_TSDB_LOAI_CT,
                        DatabaseConstant.Table.TD_TSAN_LOAI,
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
            List<KT_PHAN_HE_PLOAI> lstPhanHe = new List<KT_PHAN_HE_PLOAI>();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processTaiSanDamBao.LoaiTS(action, ref obj,ref lstPhanHe, ref listClientResponseDetail);
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
                    DatabaseConstant.Function.TD_TSDB_LOAI_CT,
                    DatabaseConstant.Table.TD_TSAN_LOAI,
                    DatabaseConstant.Action.DUYET,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
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
                        DatabaseConstant.Function.TD_TSDB_LOAI_CT,
                        DatabaseConstant.Table.TD_TSAN_LOAI,
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
            List<KT_PHAN_HE_PLOAI> lstPhanHe = new List<KT_PHAN_HE_PLOAI>();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processTaiSanDamBao.LoaiTS(action, ref obj, ref lstPhanHe, ref listClientResponseDetail);
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
                    DatabaseConstant.Function.TD_TSDB_LOAI_CT,
                    DatabaseConstant.Table.TD_TSAN_LOAI,
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
