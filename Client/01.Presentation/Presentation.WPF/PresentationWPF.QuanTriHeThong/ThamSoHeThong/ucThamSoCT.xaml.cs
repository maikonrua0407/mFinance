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
using Presentation.Process.QuanTriHeThongServiceRef;
using System.Data;
using Presentation.Process;
using PresentationWPF.CustomControl;
using Utilities.Common;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using Microsoft.Windows.Controls.Ribbon;
using System.Collections;
using System.Reflection;

namespace PresentationWPF.QuanTriHeThong.ThamSoHeThong
{
    /// <summary>
    /// Interaction logic for ucThamSoCT.xaml
    /// </summary>
    public partial class ucThamSoCT : UserControl
    {
        #region Khai bao

        public string formCase = null;
        public HT_TSO obj;
        private static int idLoai = 0;
        DataTable dt = new DataTable();
        QuanTriHeThongProcess qtht = new QuanTriHeThongProcess();
        public DatabaseConstant.Action luuAction = DatabaseConstant.Action.THEM;
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        private bool isLoaded = false;
        List<AutoCompleteEntry> lstSourceKieuDL = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiControl = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhanHe = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhamVi = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiThamSo = new List<AutoCompleteEntry>();

        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        #endregion

        public ucThamSoCT()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.QuanTriHeThong;component/ThamSoHeThong/ucThamSoCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindShortkey();
            LoadDuLieu();
            txtMaLoaiThamSo.Focus();

        }

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
            e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Luu(DatabaseConstant.Action.LUU);
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbModify.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbDelete.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void HelpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onHelp();
        }

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                Luu(DatabaseConstant.Action.LUU);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                formCase = "MANAGE";
                HideControl();
                if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_SA.layGiaTri()) ||
                    ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTTW.layGiaTri()))
                {
                    formCase = "SETVALUE";
                    HideControl();
                }
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                LuuTrangThai(DatabaseConstant.Action.XOA);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                Luu(DatabaseConstant.Action.LUU);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                formCase = "MANAGE";
                HideControl();
                if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_SA.layGiaTri()) ||
                    ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTTW.layGiaTri()))
                {
                    formCase = "SETVALUE";
                    HideControl();
                }
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                LuuTrangThai(DatabaseConstant.Action.XOA);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void onHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }

        #endregion

        #region Xu ly giao dien

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (!isLoaded)
                {
                    if (luuAction == DatabaseConstant.Action.XEM)
                        formCase = "XEM";
                    if (luuAction == DatabaseConstant.Action.SUA || luuAction == DatabaseConstant.Action.THEM)
                        formCase = "MANAGE";
                    if (formCase == null)
                    {
                        formCase = ClientInformation.FormCase;
                    }
                    string strTrangThai = string.Empty;
                    if (obj != null)
                    {
                        txtMaLoaiThamSo.Text = obj.MA_TSO_LOAI;
                        txtMaThamSo.Text = obj.MA_TSO;
                        txtTenThamSo.Text = obj.TEN_TSO;
                        txtMoTaThamSo.Text = obj.MO_TA;
                        txtGiaTriMacDinh.Text = obj.HTHI_GTRI_MDINH;
                        cmbKieuDuLieu.SelectedIndex = lstSourceKieuDL.IndexOf(lstSourceKieuDL.FirstOrDefault(d => d.KeywordStrings.First().Equals(obj.KIEU_DU_LIEU)));
                        cmbHienThiTren.SelectedIndex = lstSourceLoaiControl.IndexOf(lstSourceLoaiControl.FirstOrDefault(d => d.KeywordStrings.First().Equals(obj.HTHI_DIEU_KHIEN)));
                        txtCauLenhHienThi.Text = obj.HTHI_SQL;
                        txtGiaTri.Text = obj.GIA_TRI;
                        cmbThuocPhanHe.SelectedIndex = lstSourcePhanHe.IndexOf(lstSourcePhanHe.FirstOrDefault(d => d.KeywordStrings.First().Equals(obj.MA_PHAN_HE)));
                        cmbPhamViAHuong.SelectedIndex = lstSourcePhamVi.IndexOf(lstSourcePhamVi.FirstOrDefault(d => d.KeywordStrings.First().Equals(obj.PVI_AHUONG)));
                        strTrangThai = obj.TTHAI_NVU;
                        //cmbHanhDong.SelectedIndex = lstSource.IndexOf(lstSourceKieuDL.FirstOrDefault(d => d.KeywordStrings.First().Equals(obj.KIEU_DU_LIEU)));
                    }
                    else
                    {
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri());
                        txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(BusinessConstant.TrangThaiSuDung.SU_DUNG.layGiaTri());
                        raddtNgayNhap.Value = DateTime.Today;
                        txtNguoiLap.Text = ClientInformation.TenDangNhap;
                        strTrangThai = string.Empty;
                    }
                    CommonFunction.RefreshButton(Toolbar, luuAction, strTrangThai, mnuMain, DatabaseConstant.Function.HT_THAM_SO);
                    if (luuAction != DatabaseConstant.Action.THEM)
                        cbMultiAdd.Visibility = Visibility.Collapsed;
                    HideControl();
                    if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_SA.layGiaTri()) ||
                        ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTTW.layGiaTri()))
                    {
                        formCase = "SETVALUE";
                        HideControl();
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void LoadDuLieu()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (formCase == null)
                {
                    formCase = ClientInformation.FormCase;
                }
                AutoComboBox auto = new AutoComboBox();

                // lấy dữ liệu đổ source cho combobox Loại đối tượng
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.KIEU_DU_LIEU.getValue());
                auto.GenAutoComboBox(ref lstSourceKieuDL, ref cmbKieuDuLieu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.LOAI_DIEU_KHIEN.getValue());
                auto.GenAutoComboBox(ref lstSourceLoaiControl, ref cmbHienThiTren, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.DANH_MUC_PHAN_HE.getValue());
                auto.GenAutoComboBox(ref lstSourcePhanHe, ref cmbThuocPhanHe, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.PHAM_VI_ANH_HUONG.getValue());
                auto.GenAutoComboBox(ref lstSourcePhamVi, ref cmbPhamViAHuong, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

                lstDieuKien = new List<string>();
                auto.GenAutoComboBox(ref lstSourceLoaiThamSo, ref cmbMaLoaiThamSo, DatabaseConstant.DanhSachTruyVan.COMBOBOX_LOAITHAMSO.getValue(), lstDieuKien);

                txtMaLoaiThamSo.KeyDown += txtMaLoaiThamSo_KeyDown;
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void HideControl()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (!string.IsNullOrEmpty(formCase))
                {
                    HeThong hethong = new HeThong();
                    ArrayList arr = new ArrayList();
                    arr = hethong.SetVisibleControl("PresentationWPF.QuanTriHeThong.ThamSoHeThong.ucThamSoCT", formCase);
                    foreach (List<string> lst in arr)
                    {
                        object item = grMain.FindName(lst.First());
                        string strProperty = lst.ElementAt(1);
                        PropertyInfo prty = item.GetType().GetProperty(strProperty);
                        if (strProperty.Equals("Visibility"))
                        {
                            if (lst.ElementAt(2).Equals("0"))
                                prty.SetValue(item, Visibility.Collapsed, null);
                            else if (lst.ElementAt(2).Equals("1"))
                                prty.SetValue(item, Visibility.Visible, null);
                            else
                                prty.SetValue(item, Visibility.Hidden, null);
                        }
                        else
                        {
                            if (lst.ElementAt(2).Equals("0"))
                                prty.SetValue(item, false, null);
                            else
                                prty.SetValue(item, true, null);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        #endregion

        #region Xu ly nghiep vu

        public void Luu(DatabaseConstant.Action action)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (Validate())
                {
                    getObject(action);
                    QuanTriHeThongProcess process = new QuanTriHeThongProcess();
                    process.capNhatThamSo(action, obj, null);
                    LMessage.ShowMessage("M.DungChung.LuuDuLieuThanhCong", LMessage.MessageBoxType.Information);
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, obj.TTHAI_NVU, mnuMain, DatabaseConstant.Function.HT_THAM_SO);
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void LuuTrangThai(DatabaseConstant.Action action)
        {
            QuanTriHeThongProcess process = new QuanTriHeThongProcess();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<int> lstID = new List<int>();
                lstID.Add(obj.ID);
                process.capNhatThamSo(action, obj, null);
                LMessage.ShowMessage("M.DungChung.LuuDuLieuThanhCong", LMessage.MessageBoxType.Information);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, obj.TTHAI_NVU, mnuMain, DatabaseConstant.Function.HT_THAM_SO);
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        public void getObject(DatabaseConstant.Action action)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj == null)
                    obj = new HT_TSO();
                obj.MA_TSO_LOAI = txtMaLoaiThamSo.Text;
                //obj.ID_TSO_LOAI = idLoai;
                obj.MA_TSO = txtMaThamSo.Text;
                obj.TEN_TSO = txtTenThamSo.Text;
                obj.PVI_AHUONG = lstSourcePhamVi.ElementAt(cmbPhamViAHuong.SelectedIndex).KeywordStrings.First();
                obj.MO_TA = txtMoTaThamSo.Text;
                obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                obj.MA_DVI_TAO = ClientInformation.MaDonVi;
                obj.HTHI_DIEU_KHIEN = lstSourceLoaiControl.ElementAt(cmbHienThiTren.SelectedIndex).KeywordStrings.First();
                obj.HTHI_GTRI_MDINH = txtGiaTriMacDinh.Text;
                obj.GIA_TRI = txtGiaTri.Text;
                //if (rbtCodeQuery.IsChecked == true)
                //{
                //    obj.HTHI_POPUP = BusinessConstant.CoKhong.CO.layGiaTri();
                //    obj.HTHI_SDUNG_TVAN = BusinessConstant.CoKhong.CO.layGiaTri();
                //}
                //else
                //{
                obj.HTHI_POPUP = BusinessConstant.CoKhong.KHONG.layGiaTri();
                obj.HTHI_SDUNG_TVAN = BusinessConstant.CoKhong.KHONG.layGiaTri();
                //}
                //if (rbtSQLQuery.IsChecked == true)
                //    obj.HTHI_SQL = BusinessConstant.CoKhong.CO.layGiaTri();
                //else
                obj.HTHI_SQL = BusinessConstant.CoKhong.KHONG.layGiaTri();
                //obj.ID_PHAN_HE = Convert.ToInt32(lstSourcePhanHe.ElementAt(cmbThuocPhanHe.SelectedIndex).KeywordStrings.ElementAt(1));
                obj.MA_PHAN_HE = lstSourcePhanHe.ElementAt(cmbThuocPhanHe.SelectedIndex).KeywordStrings.First();
                obj.KIEU_DU_LIEU = lstSourceKieuDL.ElementAt(cmbKieuDuLieu.SelectedIndex).KeywordStrings.First();
                obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.NGAY_CNHAT = string.IsNullOrEmpty(raddtNgayCNhat.Value.ToString().Trim()) ? ((DateTime)raddtNgayCNhat.Value).ToString("yyyyMMdd") : (DateTime.Today).ToString("yyyyMMdd");
                if (obj.ID == 0)
                {
                    obj.NGAY_NHAP = string.IsNullOrEmpty(raddtNgayCNhat.Value.ToString().Trim()) ? ((DateTime)raddtNgayNhap.Value).ToString("yyyyMMdd") : (DateTime.Today).ToString("yyyyMMdd");
                    obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                }
                obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                obj.NGUON_TAO_DL = "NSD";
                string strTTNV = string.Empty;
                if (obj.ID > 0)
                {
                    strTTNV = obj.TTHAI_NVU;
                }
                obj.TTHAI_NVU = CommonFunction.LayTrangThaiBanGhi(action, BusinessConstant.layTrangThaiNghiepVu(strTTNV));
                obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();

            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void btnPopupLoaiThamSo_Click(object sender, RoutedEventArgs e)
        {
            ShowPopupLoaiThamSo();
        }

        private void txtMaLoaiThamSo_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.F3)
            {
                ShowPopupLoaiThamSo();
            }
        }

        private void ShowPopupLoaiThamSo()
        {
            try
            {
                lstPopup = new List<DataRow>();
                var process = new PopupProcess();
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_HT_TSO_LOAI.getValue());

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.Content = popup;
                win.ShowDialog();
                if (lstPopup != null)
                {
                    idLoai = Convert.ToInt32(lstPopup[0][1].ToString());
                    txtMaLoaiThamSo.Text = lstPopup[0][2].ToString();
                    lblLoaiThamSo.Content = lstPopup[0][3].ToString();
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private bool Validate()
        {
            bool valid = true;
            string strAlert = string.Empty;
            try
            {
                if (Utilities.Common.LString.IsNullOrEmptyOrSpace(txtMaLoaiThamSo.Text))
                    strAlert += "\r\nChưa nhập Mã loại tham số.";
                if (Utilities.Common.LString.IsNullOrEmptyOrSpace(txtMaThamSo.Text))
                    strAlert += "\r\nChưa nhập Mã tham số.";
                if (Utilities.Common.LString.IsNullOrEmptyOrSpace(txtTenThamSo.Text))
                    strAlert += "\r\nChưa nhập Tên tham số.";
                if (Utilities.Common.LString.IsNullOrEmptyOrSpace(txtGiaTriMacDinh.Text))
                    strAlert += "\r\nChưa nhập Giá trị mặc định.";
                //if (Utilities.Common.LString.IsNullOrEmptyOrSpace(txtCauLenhHienThi.Text))
                //    strAlert += "\r\nChưa nhập Danh sách giá trị.";
                if (Utilities.Common.LString.IsNullOrEmptyOrSpace(txtGiaTri.Text))
                    strAlert += "\r\nChưa nhập Giá trị.";
                //cmbPQPhamVi.SelectedIndex = obj.PVI_KTHAC;
                strAlert = strAlert.Substring(2, strAlert.Length - 3);
            }
            catch (Exception e)
            {
                strAlert = "Xảy ra lỗi : " + e.InnerException.Message;
            }
            if (strAlert.Length > 0)
            {
                valid = false;
                CommonFunction.ThongBaoLoi(strAlert);
                //ucAlertDetailForm ucAlert = new ucAlertDetailForm();
                //ucAlert.CreateAlert(ref spAlert, strAlert);
            }
            return valid;
        }

        #endregion
    }
}
