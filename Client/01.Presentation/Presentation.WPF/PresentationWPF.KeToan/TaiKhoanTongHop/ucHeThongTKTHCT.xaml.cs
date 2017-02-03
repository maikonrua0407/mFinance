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
using PresentationWPF.CustomControl;
using System.Data;
using Utilities.Common;
using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process.Common;
using Presentation.Process;
using Presentation.Process.KeToanServiceRef;

namespace PresentationWPF.KeToan.TaiKhoanTongHop
{
    /// <summary>
    /// Interaction logic for ucTongHopLoai.xaml
    /// </summary>
    public partial class ucHeThongTKTHCT : UserControl
    {
        #region Khai bao
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand MakeCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        // Source combobox
        List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();

        public event EventHandler OnSavingCompleted;

        private DataRow drPhanLoai = null;

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        private string tthaiNvu = "";
        public string TthaiNvu
        {
            get { return tthaiNvu; }
            set { tthaiNvu = value; }
        }

        private int _idTaiKhoan = -1;

        private string dauPhanCachTK = ".";

        private bool daPhatSinhGD = false;
        private string tthaiDongMoTK = "MO";
        private HE_THONG_TKTH objHeThong = new HE_THONG_TKTH();
        int id = 0;
        DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        #endregion

        #region Khoi tao
        public ucHeThongTKTHCT()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KeToan;component/TaiKhoanTongHop/ucHeThongTKTHCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindShortkey();
            InitCombobox();
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, "", mnuMain, DatabaseConstant.Function.KT_HE_THONG_TKTH);
        }

        public ucHeThongTKTHCT(int _id, DatabaseConstant.Action _action) : this()
        {
            id = _id;
            onLoad();
            LoadDuLieu();
            if (_action.Equals(DatabaseConstant.Action.SUA))
            {
                onModify();
            }
            else
            {
                action = _action;
                CommonFunction.RefreshButton(Toolbar, action, objHeThong.HTTKTHCT.TTHAI_NVU, mnuMain, DatabaseConstant.Function.KT_HE_THONG_TKTH);
                SetEnableControl(false);
            }
        }
        /// <summary>
        /// Khoi tao combobox
        /// </summary>
        private void InitCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDK = new List<string>();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
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
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(DeleteCommand, keyg);
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

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onSave();
        }

        private void MakeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute = tlbSyn.IsEnabled;
        }
        private void MakeCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //onMake();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //beforeModifyFromDetail();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onDelete();
        }

        private void HelpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //onHelp();
        }

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //onClose();
        }

        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);

            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                onSave();
            }
            else if (strTinhNang.Equals("Syn"))
            {
                //TongHopDuLieu();
                //beforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                onModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                onDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TAO_DU_LIEU)))
            {
                //onMake();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                onClose();
            }
        }

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                onSave();
            }
            else if (strTinhNang.Equals("Syn"))
            {
                //pDuLieu();
                //beforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TAO_DU_LIEU)))
            {
                //onMake();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                onModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                onDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                onClose();
            }
        }

        #endregion

        /// <summary>
        /// Sự kiện nhấn key trên form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        #region Xy ly nghiep vu
        

        private bool Validation()
        {
            if (txtMaHeThongTaiKhoan.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblMaHTTKTH.Content.ToString());
                return false;
            }
            if (txtTenHeThongTaiKhoan.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblTenHTTKTH.Content.ToString());
                return false;
            }
            if (!dtNgayHetHieuLuc.Value.IsNullOrEmpty())
            {
                if(dtNgayHetHieuLuc.Value<dtNgayHieuLuc.Value)
                {
                    LMessage.ShowMessage("M.KeToan.TaiKhoanTongHop.NgayHetHanLonNgayHieuLuc", LMessage.MessageBoxType.Warning);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void onHelp()
        {
        }

        /// <summary>
        /// Đóng form
        /// </summary>
        private void onClose()
        {
            Release();
            if (OnSavingCompleted != null)
            {
                OnSavingCompleted(this, EventArgs.Empty);
            }
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        /// <summary>
        /// Hàm xử lý giải phóng lock dữ liệu cho form
        /// </summary>
        private void Release()
        {

        }

        private int GetDataForm()
        {
            int iret = 1;
            try
            {
                if (objHeThong.IsNullOrEmpty())
                    objHeThong = new HE_THONG_TKTH();
                if (objHeThong.HTTKTHCT.IsNullOrEmpty())
                    objHeThong.HTTKTHCT = new HT_TKTH_CTIET();
                objHeThong.HTTKTHCT.ID = id;
                objHeThong.HTTKTHCT.MA_DVI_QLY = ClientInformation.MaDonVi;
                objHeThong.HTTKTHCT.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                objHeThong.HTTKTHCT.MA_HT_TKTH = txtMaHeThongTaiKhoan.Text;
                objHeThong.HTTKTHCT.NGAY_ADUNG = dtNgayHieuLuc.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                if (!dtNgayHetHieuLuc.Value.IsNullOrEmpty())
                    objHeThong.HTTKTHCT.NGAY_HET_HLUC = dtNgayHetHieuLuc.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                objHeThong.HTTKTHCT.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                objHeThong.HTTKTHCT.NGUOI_NHAP = ClientInformation.TenDangNhap;
                objHeThong.HTTKTHCT.NGUON_TAO_DL = "HTH";
                objHeThong.HTTKTHCT.TEN_HT_TKTH = txtTenHeThongTaiKhoan.Text;
                objHeThong.HTTKTHCT.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                objHeThong.HTTKTHCT.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                List<int> lstID = new List<int>();
                lstID.Add(id);
                objHeThong.DSACHID = lstID.ToArray();
                if(id>0)
                {
                    objHeThong.HTTKTHCT.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    objHeThong.HTTKTHCT.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }
            }
            catch (Exception ex)
            {
                iret = 0;
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return iret;
        }

        private void onSave()
        {
            string trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
            if (Validation())
            {
                KeToanProcess process = new KeToanProcess();
                try
                {
                    List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                    ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    // Dữ liệu truyền vào và dữ liệu trả về

                    Mouse.OverrideCursor = Cursors.Wait;
                    if (GetDataForm()==1)
                    {
                        if(process.HeThongTaiKhoanChiTiet(DatabaseConstant.Action.THEM, ref objHeThong, ref listClientResponseDetail) == ApplicationConstant.ResponseStatus.THANH_CONG)
                        {
                            CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                            LoadDuLieu();
                            SetEnableControl(false);
                            action = DatabaseConstant.Action.XEM;
                            CommonFunction.RefreshButton(Toolbar, action, objHeThong.HTTKTHCT.TTHAI_NVU, mnuMain, DatabaseConstant.Function.KT_HE_THONG_TKTH);
                        }
                        else
                        {
                            CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                        }
                    }
                    
                }
                catch (System.Exception ex)
                {
                    this.Cursor = Cursors.Arrow;
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                finally
                {
                    process = null;
                }
            }
        }

        private void LoadDuLieu()
        {

            lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(objHeThong.HTTKTHCT.TTHAI_NVU);
            txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(objHeThong.HTTKTHCT.TTHAI_BGHI);
            txtMaHeThongTaiKhoan.Text = objHeThong.HTTKTHCT.MA_HT_TKTH;
            txtTenHeThongTaiKhoan.Text = objHeThong.HTTKTHCT.TEN_HT_TKTH;
            dtNgayHieuLuc.Value = LDateTime.StringToDate(objHeThong.HTTKTHCT.NGAY_ADUNG, ApplicationConstant.defaultDateTimeFormat);
            if (!objHeThong.HTTKTHCT.NGAY_HET_HLUC.IsNullOrEmptyOrSpace())
                dtNgayHetHieuLuc.Value = LDateTime.StringToDate(objHeThong.HTTKTHCT.NGAY_HET_HLUC, ApplicationConstant.defaultDateTimeFormat);
            txtNguoiLap.Text = objHeThong.HTTKTHCT.NGUOI_NHAP;
            txtNguoiCapNhat.Text = objHeThong.HTTKTHCT.NGUOI_CNHAT;
            raddtNgayNhap.Value = LDateTime.StringToDate(objHeThong.HTTKTHCT.NGAY_NHAP, ApplicationConstant.defaultDateTimeFormat);
            if (!objHeThong.HTTKTHCT.NGAY_CNHAT.IsNullOrEmptyOrSpace())
                raddtNgayCNhat.Value = LDateTime.StringToDate(objHeThong.HTTKTHCT.NGAY_CNHAT, ApplicationConstant.defaultDateTimeFormat);
        }

        private void onDelete()
        {
            string trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
            if (Validation())
            {
                KeToanProcess process = new KeToanProcess();
                try
                {
                    List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                    ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    // Dữ liệu truyền vào và dữ liệu trả về

                    Mouse.OverrideCursor = Cursors.Wait;
                    if (GetDataForm() == 1)
                    {
                        if (process.HeThongTaiKhoanChiTiet(DatabaseConstant.Action.THEM, ref objHeThong, ref listClientResponseDetail) == ApplicationConstant.ResponseStatus.THANH_CONG)
                        {
                            CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                            this.onClose();
                        }
                        else
                        {
                            CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                        }
                    }

                }
                catch (System.Exception ex)
                {
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                finally
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                    process = null;
                }
            }
        }

        private void onLoad()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            KeToanProcess process = new KeToanProcess();
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                objHeThong = new HE_THONG_TKTH();
                objHeThong.HTTKTHCT = new HT_TKTH_CTIET();
                objHeThong.HTTKTHCT.ID = id;
                if (process.HeThongTaiKhoanChiTiet(DatabaseConstant.Action.LOAD_DATA, ref objHeThong, ref listClientResponseDetail) == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    LoadDuLieu();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                process = null;
            }
        }

        private void onModify()
        {
            action = DatabaseConstant.Action.SUA;
            SetEnableControl(true);
            CommonFunction.RefreshButton(Toolbar, action, objHeThong.HTTKTHCT.TTHAI_NVU, mnuMain, DatabaseConstant.Function.KT_HE_THONG_TKTH);
        }

        private void SetEnableControl(bool bBool)
        {
            txtMaHeThongTaiKhoan.IsEnabled = false;
            txtTenHeThongTaiKhoan.IsEnabled = bBool;
            dtNgayHieuLuc.IsEnabled = bBool;
            dtNgayHetHieuLuc.IsEnabled = bBool;
            dtpNgayHieuLuc.IsEnabled = bBool;
            dtpNgayHetHieuLuc.IsEnabled = bBool;
        }
        #endregion
    }
}
