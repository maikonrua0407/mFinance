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
using Presentation.Process.DanhMucServiceRef;
using Presentation.Process.BaoCaoServiceRef;

namespace PresentationWPF.KeToan.DoiTuong
{
    /// <summary>
    /// Interaction logic for ucDoiTuongCT.xaml
    /// </summary>
    public partial class ucDoiTuongCT : UserControl
    {
        #region Khai bao
        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }
        
        public event EventHandler OnSavingCompleted;

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private DM_DTUONG obj;
        public DM_DTUONG Obj
        {
            get { return obj; }
            set { obj = value; }
        }

        private string Ma_Dvi_Qly_Htai;
        public string MA_DVI_QLY_HTAI
        {
            get { return Ma_Dvi_Qly_Htai; }
            set { Ma_Dvi_Qly_Htai = value; }
        }

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        private string trangThaiNVu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
        private string TTHAI_BGHI;
        private string TTHAI_NVU;
        private string MA_DVI_QLY;
        private string MA_DVI_TAO;
        private string NGAY_NHAP;
        private string NGUOI_NHAP;

        List<AutoCompleteEntry> lstSourceDoiTuongLoai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();

        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        #endregion

        #region Khoi tao
        public ucDoiTuongCT()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();
            MA_DVI_QLY_HTAI = ClientInformation.MaDonVi;
            LoadCombobox();
            txtMaDoiTuong.Focus();
        }
        public ucDoiTuongCT(string maDonVi) : this()
        {
            MA_DVI_QLY_HTAI = maDonVi;
            LoadCombobox();
        }
        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KeToan;component/DoiTuong/ucDoiTuongCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void LoadCombobox()
        {
            
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(MA_DVI_QLY_HTAI);
            lstSourceDoiTuongLoai.Clear();
            lstSourceDonVi.Clear();
            cmbLoaiDoiTuong.Items.Clear();
            cmbMaDonVi.Items.Clear();
            auto.GenAutoComboBox(ref lstSourceDoiTuongLoai, ref cmbLoaiDoiTuong, "COMBOBOX_LOAI_DOI_TUONG_THEO_BANG", lstDieuKien);
            auto.GenAutoComboBox(ref lstSourceDonVi, ref cmbMaDonVi, "COMBOBOX_MA_DON_VI_THEO_BANG", new List<string>());
            cmbMaDonVi.SelectedIndex = lstSourceDonVi.IndexOf(lstSourceDonVi.FirstOrDefault(i => i.KeywordStrings[0].ToString().Equals(MA_DVI_QLY_HTAI)));
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                BeforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                BeforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
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
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(this);
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
                DatabaseConstant.Function.DC_DM_DTUONG,
                DatabaseConstant.Table.DM_DTUONG,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void OnHelp()
        {
            PresentationWPF.CustomControl.CommonFunction.ShowHelp(this);
        }

        /// <summary>
        /// Đóng form
        /// </summary>
        private void OnClose()
        {
            Release();
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(this);
        }

        private void btnPopupMaThamChieu_Click(object sender, RoutedEventArgs e)
        {
            ShowPopupMaThamChieu();
        }

        private void ShowPopupMaThamChieu()
        {
            //ClientInformation.MaDonVi
            //ClientInformation.MaDonViGiaoDich
            //ClientInformation.TenDangNhap
            //ClientInformation.NgayLamViecHienTai
            //lay loai tham chieu
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            DanhMucProcess DanhMucProcess = new DanhMucProcess();
            DM_LOAI_DTUONG dmDoiTuongLoai = new DM_LOAI_DTUONG();
            int idDoiTuongLoai;
            if (cmbLoaiDoiTuong.SelectedIndex>=0&&int.TryParse(lstSourceDoiTuongLoai.ElementAt(cmbLoaiDoiTuong.SelectedIndex).KeywordStrings.ElementAt(1).ToString().Trim(), out idDoiTuongLoai))
            {
                dmDoiTuongLoai.ID = idDoiTuongLoai;
                DanhMucProcess.getDoiTuongLoaiById(dmDoiTuongLoai.ID, ref dmDoiTuongLoai, ref listClientResponseDetail);
            }
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.MaDonVi);
            lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
            lstDieuKien.Add(ClientInformation.TenDangNhap);
            lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
            var process = new PopupProcess();
            process.getPopupInformation(dmDoiTuongLoai.NGUON_TAO_DL, lstDieuKien);

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
                txtMaThamChieu.Text = row[1].ToString();
                txtTenDoiTuong.Text = row[2].ToString();
            }

        }

        private void txtMaThamChieu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                ShowPopupMaThamChieu();
            }
        }

        #endregion

        #region Xử lý nghiệp vụ
        private void GetFormData(ref DM_DTUONG obj)
        {
            try
            {
                obj.ID = id;

                obj.ID_LOAI_DTUONG = int.Parse(lstSourceDoiTuongLoai.ElementAt(cmbLoaiDoiTuong.SelectedIndex).KeywordStrings.ElementAt(1).ToString().Trim());

                obj.MA_DTUONG = txtMaDoiTuong.Text;
                obj.MA_DVI = lstSourceDonVi.ElementAt(cmbMaDonVi.SelectedIndex).KeywordStrings.ElementAt(0).ToString().Trim();
                obj.MA_LOAI_TCHIEU = txtMaLoaiThamChieu.Text;
                obj.MA_TCHIEU = txtMaThamChieu.Text;
                obj.MO_TA = txtMota.Text;
                obj.TEN_DTUONG = txtTenDoiTuong.Text;


                #region Thông tin kiểm soát

                if (action != DatabaseConstant.Action.THEM)
                {
                    obj.TTHAI_BGHI = TTHAI_BGHI;
                    obj.TTHAI_NVU = TTHAI_NVU;
                    obj.MA_DVI_QLY = MA_DVI_QLY;
                    obj.MA_DVI_TAO = MA_DVI_TAO;
                    obj.NGAY_NHAP = NGAY_NHAP;
                    obj.NGUOI_NHAP = NGUOI_NHAP;
                    obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }
                else
                {
                    obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    obj.TTHAI_NVU = trangThaiNVu;
                    obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                    obj.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    obj.NGAY_NHAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                    obj.NGUOI_NHAP = txtNguoiLap.Text;
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
                DanhMucProcess processDanhMuc = new DanhMucProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                obj = new DM_DTUONG();
                obj.ID = id;

                ret = processDanhMuc.getDoiTuongById(id, ref obj, ref listClientResponseDetail);
                if (ret == true)
                {
                    id = obj.ID;

                    #region Thông tin chung
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(obj.TTHAI_NVU);

                    cmbLoaiDoiTuong.SelectedIndex = lstSourceDoiTuongLoai.IndexOf(lstSourceDoiTuongLoai.FirstOrDefault(i => i.KeywordStrings[1].ToString().Equals(obj.ID_LOAI_DTUONG.ToString())));

                    txtMaDoiTuong.Text = obj.MA_DTUONG;
                    cmbMaDonVi.SelectedIndex = lstSourceDonVi.IndexOf(lstSourceDonVi.FirstOrDefault(i => i.KeywordStrings[0].ToString().Equals(obj.MA_DVI.ToString())));
                    txtMaLoaiThamChieu.Text = obj.MA_LOAI_TCHIEU;
                    txtMaThamChieu.Text = obj.MA_TCHIEU;
                    txtMota.Text = obj.MO_TA;
                    txtTenDoiTuong.Text = obj.TEN_DTUONG;
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

                    #region Lưu thông tin chung
                    TTHAI_BGHI = obj.TTHAI_BGHI;
                    TTHAI_NVU = obj.TTHAI_NVU;
                    MA_DVI_QLY = obj.MA_DVI_QLY;
                    MA_DVI_TAO = obj.MA_DVI_TAO;
                    NGAY_NHAP = obj.NGAY_NHAP;
                    NGUOI_NHAP = obj.NGUOI_NHAP;
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
            //txtLoaiDoiTuong.Text = "";
            txtMaDoiTuong.Text = "";
            //txtMaDonVi.Text = "";
            txtMaLoaiThamChieu.Text = "";
            txtMaThamChieu.Text = "";
            txtMota.Text = "";
            txtTenDoiTuong.Text = "";

            //Thông tin kiểm soát
            txtTrangThaiBanGhi.Text = "";
            raddtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            raddtNgayCapNhat.Value = null;
            txtNguoiCapNhat.Text = "";
        }

        private void SetEnabledControls()
        {
            #region Thêm
            if (action == DatabaseConstant.Action.THEM)
            {
                tlbSubmit.IsEnabled = true;
                tlbModify.IsEnabled = false;
                tlbDelete.IsEnabled = false;

                cmbLoaiDoiTuong.IsEnabled = true;
                txtMaDoiTuong.IsEnabled = true;
                cmbMaDonVi.IsEnabled = false;
                txtMaLoaiThamChieu.IsEnabled = true;
                txtMaThamChieu.IsEnabled = true;
                btnPopupMaThamChieu.IsEnabled = true;
                txtMota.IsEnabled = true;
                txtTenDoiTuong.IsEnabled = true;
            }
            #endregion

            #region Sửa
            else if (action == DatabaseConstant.Action.SUA)
            {
                tlbSubmit.IsEnabled = true;
                tlbModify.IsEnabled = false;
                tlbDelete.IsEnabled = true;

                cmbLoaiDoiTuong.IsEnabled = false;
                txtMaDoiTuong.IsEnabled = false;
                cmbMaDonVi.IsEnabled = false;
                txtMaLoaiThamChieu.IsEnabled = true;
                txtMaThamChieu.IsEnabled = true;
                btnPopupMaThamChieu.IsEnabled = true;
                txtMota.IsEnabled = true;
                txtTenDoiTuong.IsEnabled = true;
            }
            #endregion

            #region Xem ~ Mặc định
            else
            {
                tlbSubmit.IsEnabled = false;
                tlbModify.IsEnabled = true;
                tlbDelete.IsEnabled = true;

                cmbLoaiDoiTuong.IsEnabled = false;
                txtMaDoiTuong.IsEnabled = false;
                cmbMaDonVi.IsEnabled = false;
                txtMaLoaiThamChieu.IsEnabled = false;
                txtMaThamChieu.IsEnabled = false;
                btnPopupMaThamChieu.IsEnabled = false;
                txtMota.IsEnabled = false;
                txtTenDoiTuong.IsEnabled = false;
            }
            #endregion
        }

        private bool Validation()
        {
            try
            {
                if (cmbMaDonVi.SelectedIndex == -1 || cmbMaDonVi.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaChon(lblMaDonVi.Content.ToString());
                    lblMaDonVi.Focus();
                    return false;
                }
                else if (txtMaDoiTuong.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaNhap(lblMaDoiTuong.Content.ToString());
                    txtMaDoiTuong.Focus();
                    return false;
                }
                else if (cmbLoaiDoiTuong.SelectedIndex == -1 || cmbLoaiDoiTuong.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaChon(lblLoaiDoiTuong.Content.ToString());
                    lblLoaiDoiTuong.Focus();
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

                obj = new DM_DTUONG();
                GetFormData(ref obj);

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
            ResetForm();
            action = DatabaseConstant.Action.THEM;
            SetEnabledControls();
            txtMaDoiTuong.Focus();
        }

        public void OnAddNew(DM_DTUONG obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                DanhMucProcess processDanhMuc = new DanhMucProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processDanhMuc.ThemDoiTuong(ref obj, ref listClientResponseDetail);
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

        public void AfterAddNew(bool ret, DM_DTUONG obj, List<ClientResponseDetail> listClientResponseDetail)
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
                        trangThaiNVu = obj.TTHAI_NVU;
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(trangThaiNVu);
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
                DatabaseConstant.Function.DC_DM_DTUONG,
                DatabaseConstant.Table.DM_DTUONG,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                if (ret)
                {
                    action = DatabaseConstant.Action.SUA;
                    SetEnabledControls();
                    txtTenDoiTuong.Focus();
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
                SetEnabledControls();
                txtTenDoiTuong.Focus();
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnModify(DM_DTUONG obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                DanhMucProcess processDanhMuc = new DanhMucProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processDanhMuc.SuaDoiTuong(ref obj, ref listClientResponseDetail);
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

        public void AfterModify(bool ret, DM_DTUONG obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                    trangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(trangThaiNVu);
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_DTUONG,
                DatabaseConstant.Table.DM_DTUONG,
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
                DatabaseConstant.Function.DC_DM_DTUONG,
                DatabaseConstant.Table.DM_DTUONG,
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
                DatabaseConstant.Function.DC_DM_DTUONG,
                DatabaseConstant.Table.DM_DTUONG,
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
                DanhMucProcess processDanhMuc = new DanhMucProcess();
                ret = processDanhMuc.XoaDoiTuong(new int[]{obj.ID}, ref listClientResponseDetail);
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
               
                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_DTUONG,
                DatabaseConstant.Table.DM_DTUONG,
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
