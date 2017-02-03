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
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using PresentationWPF.CustomControl;
using System.Data;
using Telerik.Windows.Controls;
using System.Collections;
using System.Reflection;

namespace PresentationWPF.DanhMuc.PhanHeGiaoDich
{
    /// <summary>
    /// Interaction logic for ucPhanHeGDCT.xaml
    /// </summary>
    public partial class ucPhanHeGDCT : UserControl
    {
        #region Khai bao

        public event EventHandler OnSavingCompleted;

        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();


        List<AutoCompleteEntry> lstSourcePhanHe = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTrangThai = new List<AutoCompleteEntry>();

        public int idPhanHeGD = 0;
        private string fileNameImage = "";
        public string formCase = null;
        private bool isLoaded = false;

        public string trangThaiNV = string.Empty;

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        #endregion

        #region Khoi tao

        /// <summary>
        /// Khởi tạo ucDonVi trường hợp thêm mới
        /// </summary>
        public ucPhanHeGDCT()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.DanhMuc;component/DonVi/ucPhanHeGDCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
             //Xu ly context menu
             //Bind Shortkey
            BindShortkey();
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            // Khởi tạo điều kiện gọi danh mục
            List<string> lstDieuKien = new List<string>();
            // Gán giá trị điều kiện
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.TRANG_THAI_BAN_GHI));
            auto.GenAutoComboBox(ref lstSourceTrangThai, ref cmbTrangThaiSDung, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

            // khởi tạo combobox Loại danh mục
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourcePhanHe, ref cmbPhanHe, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHANHE.getValue());
            // khởi tạo sự kiện combobox
            cmbTrangThaiSDung.KeyDown += cmb_KeyDown;
            cmbPhanHe.KeyDown += cmb_KeyDown;
            txtMaLoaiGD.Focus();
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
                        key = new KeyBinding(ucPhanHeGDCT.HoldCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(ucPhanHeGDCT.ModifyCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.Shift);
                        key = new KeyBinding(ucPhanHeGDCT.DeleteCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.A, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucPhanHeGDCT.ApproveCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucPhanHeGDCT.RefuseCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.C, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucPhanHeGDCT.CancelCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(ucPhanHeGDCT.HelpCommand, keyg);
                    }
                    if (key != null)
                    {
                        InputBindings.Add(key);
                    }
                }
            }
        }

        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                Luu(BusinessConstant.TrangThaiNghiepVu.LUU_TAM, "");
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                Luu(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET, "");
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                Sua();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                Xoa();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                Luu(BusinessConstant.TrangThaiNghiepVu.DA_DUYET, Presentation.Process.Common.ClientInformation.TenDangNhap);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                Luu(BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET, Presentation.Process.Common.ClientInformation.TenDangNhap);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                Luu(BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET, Presentation.Process.Common.ClientInformation.TenDangNhap);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                Dong();
            }
        }

        #endregion

        #region Dang ky hot key, shortcut key

        private void HoldCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbHold.IsEnabled;
        }
        private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Luu(BusinessConstant.TrangThaiNghiepVu.LUU_TAM, "");
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbSave.IsEnabled;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Luu(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET, "");
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbModify.IsEnabled;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Sua();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbDelete.IsEnabled;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Xoa();
        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbApprove.IsEnabled;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Luu(BusinessConstant.TrangThaiNghiepVu.DA_DUYET, Presentation.Process.Common.ClientInformation.TenDangNhap);
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbRefuse.IsEnabled;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Luu(BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET, Presentation.Process.Common.ClientInformation.TenDangNhap);
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbCancel.IsEnabled;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Luu(BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET, Presentation.Process.Common.ClientInformation.TenDangNhap);
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
            Dong();
        }

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                Luu(BusinessConstant.TrangThaiNghiepVu.LUU_TAM, "");
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                Luu(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET, "");
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                Sua();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                Xoa();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                Luu(BusinessConstant.TrangThaiNghiepVu.DA_DUYET, Presentation.Process.Common.ClientInformation.TenDangNhap);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                Luu(BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET, Presentation.Process.Common.ClientInformation.TenDangNhap);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                Luu(BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET, Presentation.Process.Common.ClientInformation.TenDangNhap);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                Dong();
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
        /// <summary>
        /// Sự kiện load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isLoaded)
            {
                LoadForm();
                HideControl();
                isLoaded = true;
            }
        }

        /// <summary>
        /// Sự kiện đóng
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuDong_Click(object sender, RoutedEventArgs e)
        {
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        /// <summary>
        /// Sự kiện nhấn key trên form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (OnSavingCompleted != null)
                {
                    OnSavingCompleted(this, EventArgs.Empty);
                }
                PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            }
            else
            {
                PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
            }
        }

        /// <summary>
        /// Đóng form
        /// </summary>
        private void Dong()
        {
            if (OnSavingCompleted != null)
            {
                OnSavingCompleted(this, EventArgs.Empty);
            }
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        #endregion

        #region Xu ly nghiep vu

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        private void Xoa()
        {
            DanhMucProcess danhmucProcess = new DanhMucProcess();
            int[] arrayID = new int[0];
            List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = idPhanHeGD;

                if (danhmucProcess.XoaPhanHeGD(arrayID.ToArray(), ref listResponseDetail))
                {
                    LMessage.ShowMessage("M.DanhMuc.ucDonViDS.XoaThanhCong", LMessage.MessageBoxType.Information);
                }
                else
                {
                    LMessage.ShowMessage("M.DanhMuc.ucDonViDS.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                }
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                if (ex.GetType() == typeof(CustomException))
                {
                    new frmThongBaoLoi(ex.Message, ex.InnerException).ShowDialog();
                }
                else if (ex.InnerException.GetType() == typeof(CustomException))
                {
                    new frmThongBaoLoi(ex.InnerException.Message, ex.InnerException).ShowDialog();
                }
                else
                {
                    new frmThongBaoLoi("M.DanhMuc.ucDonViDS.LoiXoaDuLieu", ex).ShowDialog();
                }
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Chuyển sang trạng thái sửa
        /// </summary>
        private void Sua()
        {
            formCase = "MANAGE";
            HideControl();
        }

        /// <summary>
        /// Lưu dữ liệu
        /// </summary>
        private void Luu(BusinessConstant.TrangThaiNghiepVu nghiepvu, string nguoi_cnhat)
        {
            if (Validation())
            {
                DanhMucProcess danhmucProcess = new DanhMucProcess();
                try
                {
                    // Lấy dữ liệu từ giao diện đưa vào object
                    Presentation.Process.DanhMucServiceRef.DM_PHAN_HE_GD obj = new Presentation.Process.DanhMucServiceRef.DM_PHAN_HE_GD();
                    obj = LayDuLieu(nghiepvu, nguoi_cnhat);

                    // Lưu dữ liệu
                    if (idPhanHeGD == 0)
                    {
                        if (!danhmucProcess.checkMaDonViDaSuDung(obj.MA_LOAI_GDICH))
                        {

                            if (danhmucProcess.ThemPhanHeGD(obj) != null)
                            {
                                LMessage.ShowMessage("M.DanhMuc.ucPhanHeGDCT.LuuDuLieuThanhCong", LMessage.MessageBoxType.Information);
                                idPhanHeGD = obj.ID;
                                if (cbMultiAdd.IsChecked == true)
                                {
                                    ResetForm();
                                    idPhanHeGD = 0;
                                }
                                else
                                {
                                    Dong();
                                }
                            }
                            else
                            {
                                LMessage.ShowMessage("M.DanhMuc.ucPhanHeGDCT.LuuDuLieuKoThanhCong", LMessage.MessageBoxType.Error);
                            }
                        }

                        else
                        {
                            LMessage.ShowMessage("M.DanhMuc.ucPhanHeGDCT.MaDonViDaDuocSuDung", LMessage.MessageBoxType.Warning);
                            txtMaLoaiGD.Focus();
                        }
                    }
                    else
                    {
                        if (danhmucProcess.SuaPhanHeGD(obj) != null)
                        {
                            LMessage.ShowMessage("M.DanhMuc.ucPhanHeGDCT.LuuDuLieuThanhCong", LMessage.MessageBoxType.Information);
                        }
                        else
                        {
                            LMessage.ShowMessage("M.DanhMuc.ucPhanHeGDCT.LuuDuLieuKoThanhCong", LMessage.MessageBoxType.Error);
                        }
                        Dong();
                    }

                }
                catch (System.Exception ex)
                {
                    LMessage.ShowMessage("M.DanhMuc.ucPhanHeGDCT.LoiLuuDuLieu", LMessage.MessageBoxType.Error);
                    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                }
                finally
                {
                    danhmucProcess = null;
                }
            }
        }

        /// <summary>
        /// Kiểm tra các thông tin nhập vào trước khi lưu dữ liệu
        /// </summary>
        /// <returns></returns>
        private bool Validation()
        {
            if (LString.IsNullOrEmptyOrSpace(txtMaLoaiGD.Text))
            {
                LMessage.ShowMessage("Mã loại giao dịch không được để trống", LMessage.MessageBoxType.Warning);
                txtMaLoaiGD.Focus();
                return false;
            }
            else if (LString.IsNullOrEmptyOrSpace(txtTenLoaiGD.Text))
            {
                LMessage.ShowMessage("M.DanhMuc.ucPhanHeGDCT.ThieuTenDonVi", LMessage.MessageBoxType.Warning);
                txtTenLoaiGD.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Đưa form về trạng thái mặc định
        /// </summary>
        private void ResetForm()
        {
            txtMaLoaiGD.Text = "";
            txtTenLoaiGD.Text = "";
            txtTentat.Text = "";
            cmbPhanHe.SelectedIndex = 0;
            //cmbTrangThaiSDung.SelectedIndex = 0;
            idPhanHeGD = 0;
        }

        /// <summary>
        /// Sự kiện load dữ liệu lên form
        /// </summary>
        private void LoadForm()
        {
            DanhMucProcess danhmucProcess = new DanhMucProcess();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (idPhanHeGD != 0)
                {
                    //Sự kiện load dữ liệu
                    DataSet dsPhanHeGD=danhmucProcess.getDanhSachPhanHeGDTheoID(idPhanHeGD);
                    if (dsPhanHeGD != null && dsPhanHeGD.Tables.Count > 0)
                    {
                        DataTable dtPhanHeGD = danhmucProcess.getDanhSachPhanHeGDTheoID(idPhanHeGD).Tables[0];
                        if (dtPhanHeGD.Rows.Count > 0)
                        {
                            lblTrangThai.Content = BusinessConstant.layTrangThaiNghiepVu(dtPhanHeGD.Rows[0]["TTHAI_NVU"].ToString());
                            txtMaLoaiGD.Text = dtPhanHeGD.Rows[0]["MA_LOAI_GDICH"].ToString();
                            txtNgayDuyet.Text = dtPhanHeGD.Rows[0]["NGAY_CNHAT"].ToString();
                            txtNgayLap.Text = dtPhanHeGD.Rows[0]["NGAY_NHAP"].ToString();
                            txtTenLoaiGD.Text = dtPhanHeGD.Rows[0]["TEN_LOAI_GDICH"].ToString();
                            txtTentat.Text = dtPhanHeGD.Rows[0]["TEN_TAT"].ToString();
                            txtTrangThaiBanGhi.Text = dtPhanHeGD.Rows[0]["TTHAI_BGHI"].ToString();
                            txtNguoiDuyet.Text = dtPhanHeGD.Rows[0]["NGUOI_CNHAT"].ToString();
                            txtNguoiLap.Text = dtPhanHeGD.Rows[0]["NGUOI_NHAP"].ToString();
                            cmbPhanHe.SelectedIndex = lstSourcePhanHe.IndexOf(lstSourcePhanHe.FirstOrDefault(i => i.KeywordStrings.First().Equals(dtPhanHeGD.Rows[0]["MA_PHAN_HE"].ToString())));
                            cmbTrangThaiSDung.SelectedIndex = lstSourceTrangThai.IndexOf(lstSourceTrangThai.FirstOrDefault(i => i.KeywordStrings.First().Equals(dtPhanHeGD.Rows[0]["TTHAI_BGHI"].ToString())));
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DanhMuc.ucPhanHeGDCT.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void HideControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.DanhMuc.PhanHeGiaoDich.ucPhanHeGDCT", formCase);
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

        private void tabChiTiet_SelectionChanged(object sender, Telerik.Windows.Controls.RadSelectionChangedEventArgs e)
        {
            if (tabChiTiet.SelectedIndex == 0)
            {
                if (txtMaLoaiGD != null)
                {
                    UpdateLayout();
                    txtMaLoaiGD.Focus();
                }
            }
            else if (tabChiTiet.SelectedIndex == 1)
            {
                if (txtTrangThaiBanGhi != null)
                {
                    UpdateLayout();
                    txtTrangThaiBanGhi.Focus();
                }
            }
        }

        private void cmb_KeyDown(object sender, KeyEventArgs e)
        {
            ((RadComboBox)sender).IsDropDownOpen = true;
        }

        /// <summary>
        /// Đưa dữ liệu từ form vào object DM_PHAN_HE_GD
        /// </summary>
        /// <param name="banghi">Trạng thái bản ghi</param>
        /// <param name="nghiepvu">Trạng thái nghiệp vụ</param>
        /// <param name="nguoi_cnhap">Tên người cập nhật</param>
        /// <returns></returns>
        private Presentation.Process.DanhMucServiceRef.DM_PHAN_HE_GD LayDuLieu(BusinessConstant.TrangThaiNghiepVu nghiepvu, string nguoi_cnhat)
        {
            Presentation.Process.DanhMucServiceRef.DM_PHAN_HE_GD obj = new Presentation.Process.DanhMucServiceRef.DM_PHAN_HE_GD();
            if (idPhanHeGD != 0)
            {
                obj.ID = idPhanHeGD;
            }
            obj.MA_LOAI_GDICH = txtMaLoaiGD.Text;
            obj.TEN_LOAI_GDICH = txtTenLoaiGD.Text;
            obj.TEN_TAT = txtTentat.Text;
            obj.MA_PHAN_HE = cmbPhanHe.SelectedValue.ToString();
            obj.ID_PHAN_HE = 0;

            //obj.TTHAI_BGHI = cmbTrangThaiSDung.SelectedValue.ToString();
            obj.TTHAI_NVU = BusinessConstant.layGiaTri(nghiepvu);
            obj.MA_DVI_TAO = Presentation.Process.Common.ClientInformation.MaDonVi;
            obj.MA_DVI_QLY = Presentation.Process.Common.ClientInformation.MaDonVi;
            if (!LString.IsNullOrEmptyOrSpace(nguoi_cnhat))
            {
                obj.NGUOI_CNHAT = nguoi_cnhat;
                obj.NGAY_CNHAT = LDateTime.GetCurrentDate(ApplicationConstant.defaultDateTimeFormat);
            }
            else
            {
                obj.NGUOI_NHAP = Presentation.Process.Common.ClientInformation.TenDangNhap;
                obj.NGAY_NHAP = LDateTime.GetCurrentDate(ApplicationConstant.defaultDateTimeFormat);
            }
            return obj;
        }
        #endregion
    }
}
