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
using System.Data;
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using PresentationWPF.CustomControl;
using Presentation.Process.DanhMucServiceRef;
using System.Collections;
using System.Reflection;

namespace PresentationWPF.DanhMuc.TinhTP
{
    /// <summary>
    /// Interaction logic for ucTinhTPCT.xaml
    /// </summary>
    public partial class ucTinhTPCT01 : UserControl
    {
        #region Khai bao
        public event EventHandler OnSavingCompleted = null;
        public static RoutedCommand AddCommand = new RoutedCommand();
        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        DataRow lstChiTiet = null;

        public DataRow LstChiTiet
        {
            get { return lstChiTiet; }
            set { lstChiTiet = value; }
        }

        List<AutoCompleteEntry> lstSourceLoaiKVuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceVung = new List<AutoCompleteEntry>();
        private string maVung;

        public string MaVung
        {
            get { return maVung; }
            set { maVung = value; }
        }

        List<AutoCompleteEntry> lstSourceMien = new List<AutoCompleteEntry>();
        private string maMien;

        public string MaMien
        {
            get { return maMien; }
            set { maMien = value; }
        }

        private string idMien;

        public string IdMien
        {
            get { return idMien; }
            set { idMien = value; }
        }

        private string idVung;

        public string IdVung
        {
            get { return idVung; }
            set { idVung = value; }
        }

        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private string formCase = string.Empty;

        public string FormCase
        {
            get { return formCase; }
            set { formCase = value; }
        }

        private int id = 0;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string tthaiNvu = "";
        public string TthaiNvu
        {
            get { return tthaiNvu; }
            set { tthaiNvu = value; }
        }
        #endregion

        #region Khoi tao

        public ucTinhTPCT01()
        {
            InitializeComponent();
            LoadCombobox();
            InitEventHandler();
            BindShortkey();
        }

        /// <summary>
        /// Khởi tạo các combobox
        /// </summary>
        private void LoadCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceMien, ref cmbMien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_MIEN.getValue());
            cmbMien.SelectedIndex = 0;

            MaMien = lstSourceMien.ElementAt(cmbMien.SelectedIndex).KeywordStrings.First();
            IdMien = lstSourceMien.ElementAt(cmbMien.SelectedIndex).KeywordStrings.ElementAt(1);
            auto = new AutoComboBox();
            List<string> lstMien = new List<string>();
            lstMien.Add(idMien);
            cmbVung.Items.Clear();
            lstSourceVung = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstSourceVung, ref cmbVung, DatabaseConstant.DanhSachTruyVan.COMBOBOX_VUNG.getValue(), lstMien);
            cmbVung.SelectedIndex = 0;

            lstSourceLoaiKVuc = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstSourceLoaiKVuc, ref cmbLoaiKVuc, "COMBOBOX_LOAI_KHU_VUC", null);
            cmbLoaiKVuc.SelectedIndex = 0;

        }

        /// <summary>
        /// InitEventHandler
        /// </summary>
        private void InitEventHandler()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.DanhMuc;component/TinhTP/ucTinhTPCT01.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }

            BindShortkey();

            cmbMien.KeyDown += new KeyEventHandler(cmbMien_KeyDown);
            cmbMien.LostFocus += new RoutedEventHandler(cmbMien_LostFocus);
            cmbMien.SelectionChanged += new SelectionChangedEventHandler(cmbMien_SelectionChanged);
            cmbVung.KeyDown += new KeyEventHandler(cmbVung_KeyDown);
            txtMaTinh.Focus();
        }

        /// <summary>
        /// Đưa form về trạng thái mặc định
        /// </summary>
        private void ResetForm()
        {
            //Reset biến
            id = 0;
            tthaiNvu = "";

            txtMaTinh.Text = "";
            txtTenDayDu.Text = "";
            txtTenTat.Text = "";
            txtNgayLap.Value = LDateTime.GetCurrentDate();
            txtNgayDuyet.Value = null;
            cmbVung.SelectedIndex = 0;
            cmbMien.SelectedIndex = 0;
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            txtNguoiDuyet.Text = "";
            txtTrangThaiBanGhi.Text = "";
            lblTrangThai.Content = "";

            telnumDienTich.Value = 0;
            telnumDanSo.Value = 0;
            telnumSoHoNgheo.Value = 0;
            telnumSoHoCanNgheo.Value = 0;
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
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.N, ModifierKeys.Control);
                        key = new KeyBinding(AddCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
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

        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbAdd.IsEnabled;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeAddNew();
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onSave();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeModifyFromDetail();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeDelete();
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
            onClose();
        }

        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                beforeAddNew();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                onSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                beforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                beforeDelete();
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
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                beforeAddNew();
            }
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                onSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                beforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                beforeDelete();
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

        #region Xu ly Giao dien

        /// <summary>
        /// thiết lập hiển thị cho các control
        /// </summary>
        private void HideControl()
        {
            this.Cursor = Cursors.Wait;
            try
            {
                if (formCase == null)
                {
                    formCase = ClientInformation.FormCase;
                }
                if (!string.IsNullOrEmpty(formCase))
                {
                    HeThong hethong = new HeThong();
                    ArrayList arr = new ArrayList();
                    arr = hethong.SetVisibleControl("PresentationWPF.DanhMuc.TinhTP.ucTinhTPCT01", formCase);
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

        /// <summary>
        /// Sự kiện ấn key trên form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        /// <summary>
        /// Sự kiện lostfocus của ComboBox miền 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbMien_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstSourceMien.Select(i => i.DisplayName).Contains(cmbMien.Text))
            {
                MaMien = lstSourceMien.ElementAt(cmbMien.SelectedIndex).KeywordStrings.First();
                IdMien = lstSourceMien.ElementAt(cmbMien.SelectedIndex).KeywordStrings.ElementAt(1);
                AutoComboBox auto = new AutoComboBox();
                List<string> lstMien = new List<string>();
                lstMien.Add(idMien);
                cmbVung.Items.Clear();
                lstSourceVung = new List<AutoCompleteEntry>();
                auto.GenAutoComboBox(ref lstSourceVung, ref cmbVung, "COMBOBOX_VUNG", lstMien);
                cmbVung.Text = "";
            }
            else
                cmbMien.Text = "";
        }

        /// <summary>
        /// Su kien LostFocus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbMien_LostFocus(object sender, RoutedEventArgs e)
        {
            if (idMien == null || idMien == "")
            {
                if (lstSourceMien.Select(i => i.DisplayName).Contains(cmbMien.Text))
                {
                    MaMien = lstSourceMien.ElementAt(cmbMien.SelectedIndex).KeywordStrings.First();
                    IdMien = lstSourceMien.ElementAt(cmbMien.SelectedIndex).KeywordStrings.ElementAt(1);
                    AutoComboBox auto = new AutoComboBox();
                    List<string> lstMien = new List<string>();
                    lstMien.Add(idMien);
                    cmbVung.Items.Clear();
                    lstSourceVung = new List<AutoCompleteEntry>();
                    auto.GenAutoComboBox(ref lstSourceVung, ref cmbVung, "COMBOBOX_VUNG", lstMien);
                }
                else
                    cmbMien.Text = "";
            }
        }

        /// <summary>
        /// Sự kiện keydown của ComboBox miền
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbMien_KeyDown(object sender, KeyEventArgs e)
        {
            cmbMien.IsDropDownOpen = true;
        }

        /// <summary>
        /// Sự kiện KeyDown của ComboBox vùng
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbVung_KeyDown(object sender, KeyEventArgs e)
        {
            cmbVung.IsDropDownOpen = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //SetFormData();
            //HideControl();
            //Hiển thị Form khi thêm mới dữ liệu
            if (action == DatabaseConstant.Action.THEM)
            {
                beforeAddNew();
            }

            //Hiển thị Form khi sửa dữ liệu
            else if (action == DatabaseConstant.Action.SUA)
            {
                beforeModifyFromList();
            }

            //Hiển thị Form khi xem dữ liệu
            else if (action == DatabaseConstant.Action.XEM)
            {
                beforeViewFromList();
            }

            else
            {
                beforeAddNew();
            }
        }

        /// <summary>
        /// Sự kiện unlod cho form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
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
            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool ret = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_TINH_THANH,
                DatabaseConstant.Table.DM_TINH_TP,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void onHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
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

        #endregion

        #region Xu ly Nghiep Vu
        
        /// <summary>
        /// Sự kiện load dữ liệu lên form
        /// </summary>
        private void SetFormData()
        {
            DanhMucProcess danhmucProcess = new DanhMucProcess();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (id != 0)
                {
                    DanhMucProcess proccess = new DanhMucProcess();
                    DMTinhTPResponse response = proccess.getTinhTPById01(id);
                    DM_TINH_TP obj = response.obj;
                    VDM_DBAN_TTIN_KHAC objTTKhac = response.objTTKhac;
                    txtMaTinh.Text = obj.MA_TINHTP;
                    txtTenDayDu.Text = obj.TEN_TINHTP;
                    txtTenTat.Text = obj.TEN_TAT;

                    IdMien = proccess.getVungMienById(obj.ID_VUNG_MIEN.Value).ID_CHA.ToString();
                    cmbMien.SelectedIndex = lstSourceMien.IndexOf(lstSourceMien.FirstOrDefault(i => i.KeywordStrings.ElementAt(1).Equals(IdMien)));

                    AutoComboBox auto = new AutoComboBox();
                    List<string> lstMien = new List<string>();
                    lstMien.Add(IdMien);
                    cmbVung.Items.Clear();
                    lstSourceVung = new List<AutoCompleteEntry>();
                    auto.GenAutoComboBox(ref lstSourceVung, ref cmbVung, DatabaseConstant.DanhSachTruyVan.COMBOBOX_VUNG.getValue(), lstMien);
                    cmbVung.SelectedIndex = lstSourceVung.IndexOf(lstSourceVung.FirstOrDefault(i => i.KeywordStrings.ElementAt(1).Equals(obj.ID_VUNG_MIEN.ToString())));
                    MaVung = lstSourceVung.ElementAt(cmbVung.SelectedIndex).KeywordStrings.First();
                    IdVung = lstSourceVung.ElementAt(cmbVung.SelectedIndex).KeywordStrings.ElementAt(1);
                    lblTrangThai.Content = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    txtNgayLap.Value = LDateTime.StringToDate(obj.NGAY_NHAP, "yyyyMMdd");
                    if (!obj.NGAY_CNHAT.IsNullOrEmptyOrSpace())
                        txtNgayDuyet.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, "yyyyMMdd");
                    txtNguoiLap.Text = obj.NGUOI_NHAP;
                    txtNguoiDuyet.Text = obj.NGUOI_CNHAT;
                    tthaiNvu = obj.TTHAI_NVU;

                    if (objTTKhac != null)
                    {
                        telnumDanSo.Value = Convert.ToInt32(objTTKhac.DAN_SO);
                        telnumDienTich.Value = Convert.ToInt32(objTTKhac.DIEN_TICH);
                        telnumSoHoNgheo.Value = Convert.ToInt32(objTTKhac.SO_HO_NGHEO);
                        telnumSoHoCanNgheo.Value = Convert.ToInt32(objTTKhac.SO_HO_CAN_NGHEO);
                        cmbLoaiKVuc.SelectedIndex = lstSourceLoaiKVuc.IndexOf(lstSourceLoaiKVuc.FirstOrDefault(i => i.KeywordStrings.ElementAt(0).Equals(objTTKhac.LOAI_KVUC.ToString())));
                    }
                }
                else
                {
                    ResetForm();
                }
                if (formCase.Equals("XEM"))
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_TINH_THANH);
                if (formCase.Equals("MANAGE"))
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_TINH_THANH);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DanhMuc.ucTinhTPCT.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Đưa dữ liệu từ form vào object DM_TINH_TP
        /// </summary>
        private void GetFormData(ref DM_TINH_TP obj, ref VDM_DBAN_TTIN_KHAC objTTKhac)
        {
            if (id != 0)
            {
                obj.ID = id;
                obj.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
            }
            else
            {
                obj.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();

                obj.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
            }

            obj.MA_TINHTP = txtMaTinh.Text;
            obj.TEN_TINHTP = txtTenDayDu.Text;
            obj.TEN_TAT = txtTenTat.Text;
            obj.ID_VUNG_MIEN = int.Parse(idVung);
            obj.NGAY_NHAP = LDateTime.GetCurrentDate(ApplicationConstant.defaultDateTimeFormat);
            obj.MA_DVI_QLY = ClientInformation.MaDonVi;
            obj.MA_DVI_TAO = ClientInformation.MaDonVi;
            obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
            obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
            obj.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();

            objTTKhac.ID_DBAN = obj.ID;
            objTTKhac.DAN_SO = telnumDanSo.Value.ToString();
            objTTKhac.DIEN_TICH = telnumDienTich.Value.ToString();
            objTTKhac.LOAI = "TINH_TP";
            objTTKhac.LOAI_KVUC = lstSourceLoaiKVuc.ElementAt(cmbLoaiKVuc.SelectedIndex).KeywordStrings[0];
            objTTKhac.MA_DBAN = obj.MA_TINHTP;
            objTTKhac.MA_DVI_TAO = obj.MA_DVI_QLY;
            objTTKhac.NGAY_CNHAT = obj.NGAY_CNHAT;
            objTTKhac.NGAY_DL = obj.NGAY_NHAP;
            objTTKhac.NGAY_NHAP = obj.NGAY_NHAP;
            objTTKhac.NGUOI_CNHAT = obj.NGUOI_CNHAT;
            objTTKhac.NGUOI_NHAP = obj.NGUOI_NHAP;
            objTTKhac.SO_HO_CAN_NGHEO = telnumSoHoCanNgheo.Value.ToString();
            objTTKhac.SO_HO_NGHEO = telnumSoHoNgheo.Value.ToString();
            objTTKhac.TEN_BANG = "DM_TINH_TP";
            objTTKhac.THUOC_DVI = null;
            objTTKhac.THUOC_VUNG = obj.ID_VUNG_MIEN.ToString();
            objTTKhac.TTHAI_BGHI = obj.TTHAI_BGHI;
            objTTKhac.TTHAI_NVU = obj.TTHAI_NVU;
            objTTKhac.MA_DOI_TUONG = "VDM_DBAN_TTIN_KHAC";
            return;
        }

        /// <summary>
        /// Kiểm tra các thông tin nhập vào trước khi lưu dữ liệu
        /// </summary>
        /// <returns></returns>
        private bool Validation()
        {
            if (lstSourceVung.Select(i => i.DisplayName).Contains(cmbVung.Text))
                IdVung = lstSourceVung.ElementAt(cmbVung.SelectedIndex).KeywordStrings.ElementAt(1);
            else
                cmbVung.Text = "";
            if (txtMaTinh.Text.IsNullOrEmpty())
            {
                LMessage.ShowMessage("M.DanhMuc.ucTinhTPCT.ThieuMaTinhThanhPho", LMessage.MessageBoxType.Warning);
                txtMaTinh.Focus();
                return false;
            }
            else if (txtTenDayDu.Text.IsNullOrEmpty())
            {
                LMessage.ShowMessage("M.DanhMuc.ucTinhTPCT.ThieuTenDayDuThanhPho", LMessage.MessageBoxType.Warning);
                txtTenDayDu.Focus();
                return false;
            }
            else if (txtTenTat.Text.IsNullOrEmpty())
            {
                LMessage.ShowMessage("M.DanhMuc.ucTinhTPCT.ThieuTenTatThanhPho", LMessage.MessageBoxType.Warning);
                txtTenTat.Focus();
                return false;
            }
            else if (cmbMien.Text.IsNullOrEmpty())
            {
                LMessage.ShowMessage("M.DanhMuc.ucTinhTPCT.ThieuMien", LMessage.MessageBoxType.Warning);
                cmbMien.Focus();
                return false;
            }
            else if (cmbVung.Text.IsNullOrEmpty())
            {
                LMessage.ShowMessage("M.DanhMuc.ucTinhTPCT.ThieuVung", LMessage.MessageBoxType.Warning);
                cmbVung.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Trước khi xem
        /// </summary>
        public void beforeView()
        {
            SetFormData();
            formCase = "XEM";
            HideControl();
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_TINH_THANH);
        }

        /// <summary>
        /// Trước khi xem từ trang list
        /// </summary>
        public void beforeViewFromList()
        {
            SetFormData();
            formCase = "XEM";
            HideControl();
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_TINH_THANH);
        }

        /// <summary>
        /// Trước khi thêm mới
        /// </summary>
        private void beforeAddNew()
        {

            id = 0;
            SetFormData();
            formCase = "MANAGE";
            HideControl();                       
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_TINH_THANH);
            txtMaTinh.Focus();
        }

        /// <summary>
        /// Trước khi sửa từ chi tiết
        /// </summary>
        private void beforeModifyFromDetail()
        {
            // Yêu cầu lock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool ret = process.LockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_TINH_THANH,
                DatabaseConstant.Table.DM_TINH_TP,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Nếu lock thành công >> cho phép sửa
            if (ret)
            {
                formCase = "MANAGE";
                HideControl();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_TINH_THANH);
            }
            else
            {
                LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
            }
        }

        /// <summary>
        /// Trước khi sửa từ danh sách
        /// </summary>
        public void beforeModifyFromList()
        {
            SetFormData();
            formCase = "MANAGE";
            HideControl();
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_TINH_THANH);
        }

        /// <summary>
        /// Trước khi xóa
        /// </summary>
        private void beforeDelete()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listLockedidTinhTP = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_TINH_THANH,
                    DatabaseConstant.Table.DM_TINH_TP,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    // Gọi tới hàm xóa dữ liệu
                    onDelete();
                    return;
                }
                // Nếu lock không thành công >> cảnh báo
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                    return;
                }
            }
            else
            {
                // Unlock dữ liệu
                // (Hiện tại không phải Unlock dữ liệu, vì chỉ được xóa khi chọn sửa - sẽ Unlock sau)
                return;
            }
        }

        /// <summary>
        /// Trước khi duyệt
        /// </summary>
        private void beforeApprove()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listLockedidTinhTP = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_TINH_THANH,
                    DatabaseConstant.Table.DM_TINH_TP,
                    DatabaseConstant.Action.DUYET,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    // Gọi tới hàm duyệt dữ liệu
                    onApprove();
                    return;
                }
                // Nếu lock không thành công >> cảnh báo
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                    return;
                }
            }
            else
            {
                // Unlock dữ liệu
                // (Hiện tại không phải Unlock dữ liệu, vì chỉ được xóa khi chọn sửa - sẽ Unlock sau)
                return;
            }
        }

        /// <summary>
        /// Trước khi thoái duyệt
        /// </summary>
        private void beforeCancel()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listLockedidTinhTP = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_TINH_THANH,
                    DatabaseConstant.Table.DM_TINH_TP,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    // Gọi tới hàm thoái duyệt dữ liệu
                    onCancel();
                    return;
                }
                // Nếu lock không thành công >> cảnh báo
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                    return;
                }
            }
            else
            {
                // Unlock dữ liệu
                // (Hiện tại không phải Unlock dữ liệu, vì chỉ được xóa khi chọn sửa - sẽ Unlock sau)
                return;
            }
        }

        /// <summary>
        /// Trước khi từ chối
        /// </summary>
        private void beforeRefuse()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listLockedidTinhTP = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_TINH_THANH,
                    DatabaseConstant.Table.DM_TINH_TP,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    // Gọi tới hàm xóa dữ liệu
                    onRefuse();
                    return;
                }
                // Nếu lock không thành công >> cảnh báo
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                    return;
                }
            }
            else
            {
                // Unlock dữ liệu
                // (Hiện tại không phải Unlock dữ liệu, vì chỉ được xóa khi chọn sửa - sẽ Unlock sau)
                return;
            }
        }

        /// <summary>
        /// Lưu dữ liệu (hay trình duyệt)
        /// </summary>
        private void onSave()
        {
            string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            if (Validation())
            {
                DanhMucProcess danhmucProcess = new DanhMucProcess();                
                try
                {
                    // Dữ liệu truyền vào và dữ liệu trả về
                    DM_TINH_TP obj = new DM_TINH_TP();
                    VDM_DBAN_TTIN_KHAC objTTKhac = new VDM_DBAN_TTIN_KHAC();

                    ApplicationConstant.ResponseStatus retStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    string responseMessage = null;

                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (id == 0)
                    {
                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref objTTKhac);
                        retStatus = danhmucProcess.ThemTinhTP01(ref obj, ref objTTKhac, ref responseMessage);

                        afterAddNew(retStatus, obj, responseMessage);
                    }
                    // Nếu là lưu tạm khi sửa
                    // Hoặc lưu tạm khi sửa sau duyệt
                    // Hoặc sửa
                    else
                    {
                        // Lấy thông tin cũ
                        DMTinhTPResponse response = danhmucProcess.getTinhTPById01(id);
                        obj = response.obj;
                        objTTKhac = response.objTTKhac;

                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref objTTKhac);
                        retStatus = danhmucProcess.SuaTinhTP01(ref obj, ref objTTKhac, ref responseMessage);
                        afterModify(retStatus, obj, responseMessage);
                    }
                }
                catch (System.Exception ex)
                {
                    // Yêu cầu unlock dữ liệu khi xảy ra lỗi
                    // (chỉ dùng trong trường hợp sửa)

                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);
                    List<int> listUnlockidTinhTP = new List<int>();

                    bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_DM_TINH_THANH,
                        DatabaseConstant.Table.DM_TINH_TP,
                        DatabaseConstant.Action.SUA,
                        listLockId);

                    this.Cursor = Cursors.Arrow;
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                finally
                {
                    danhmucProcess = null;
                }
            }
        }

        /// <summary>
        /// Lưu tạm dữ liệu
        /// </summary>
        private void onHold()
        {
            string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));

            if (Validation())
            {
                DanhMucProcess danhmucProcess = new DanhMucProcess();
                try
                {
                    // Dữ liệu truyền vào và dữ liệu trả về
                    DM_TINH_TP obj = new DM_TINH_TP();
                    VDM_DBAN_TTIN_KHAC objTTKhac = new VDM_DBAN_TTIN_KHAC();
                    DM_TINH_TP ret = null;

                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (id == 0)
                    {
                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref objTTKhac);
                        obj.TTHAI_NVU = trangThai;

                        // Set các thông tin khác                        
                        obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                        obj.NGAY_NHAP = LDateTime.GetCurrentDate(ApplicationConstant.defaultDateTimeFormat);
                        //ret = danhmucProcess.ThemTinhTP(obj);

                        //afterAddNew(ret);
                    }
                    // Nếu là lưu tạm khi sửa
                    // Hoặc lưu tạm khi sửa sau duyệt
                    // Hoặc sửa
                    else
                    {
                        // Lấy thông tin cũ
                        DMTinhTPResponse response = danhmucProcess.getTinhTPById01(id);
                        obj = response.obj;
                        objTTKhac = response.objTTKhac;

                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref objTTKhac);
                        obj.TTHAI_NVU = trangThai;

                        //ret = danhmucProcess.SuaTinhTP(obj);
                        //afterModify(ret);
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
                    danhmucProcess = null;
                }
            }
        }

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        private void onDelete()
        {
            DanhMucProcess danhmucProcess = new DanhMucProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = id;

                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = danhmucProcess.XoaTinhTP01(arrayID, ref listClientResponseDetail);

                afterDelete(ret, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu khi xảy ra lỗi
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockidTinhTP = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_TINH_THANH,
                    DatabaseConstant.Table.DM_TINH_TP,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Duyệt dữ liệu
        /// </summary>
        private void onApprove()
        {
            DanhMucProcess danhmucProcess = new DanhMucProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = id;

                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = danhmucProcess.DuyetTinhTP(arrayID, ref listClientResponseDetail);

                afterApprove(ret);
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Thoái duyệt dữ liệu
        /// </summary>
        private void onCancel()
        {
            DanhMucProcess danhmucProcess = new DanhMucProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = id;

                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = danhmucProcess.ThoaiDuyetTinhTP(arrayID, ref listClientResponseDetail);

                afterApprove(ret);
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Từ chối duyệt dữ liệu
        /// </summary>
        private void onRefuse()
        {
            DanhMucProcess danhmucProcess = new DanhMucProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = id;

                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = danhmucProcess.TuChoiCum(arrayID, ref listClientResponseDetail);

                afterRefuse(ret);
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Sau khi thêm mới
        /// </summary>
        /// <param name="ret"></param>
        private void afterAddNew(ApplicationConstant.ResponseStatus ret, DM_TINH_TP obj, string responseMessage)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                if (cbMultiAdd.IsChecked == true)
                {                    
                    beforeAddNew();
                }
                else if (!DatabaseConstant.CLOSE_DETAIL_FORM)
                {
                    id = obj.ID;
                    TthaiNvu = obj.TTHAI_NVU;
                    txtMaTinh.Text = obj.MA_TINHTP;
                    lblTrangThai.Content = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);

                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_TINH_THANH);

                    formCase = "XEM";
                    HideControl();
                }
                else
                {
                    onClose();
                }
            }
            else
            {
                LMessage.ShowMessage(responseMessage, LMessage.MessageBoxType.Error);
            }
        }

        /// <summary>
        /// Sau khi sửa
        /// </summary>
        /// <param name="ret"></param>
        private void afterModify(ApplicationConstant.ResponseStatus ret, DM_TINH_TP obj, string responseMessage)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                id = obj.ID;
                TthaiNvu = obj.TTHAI_NVU;
                txtMaTinh.Text = obj.MA_TINHTP;
                lblTrangThai.Content = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);

                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_TINH_THANH);

                formCase = "XEM";
                HideControl();
            }
            else
            {
                LMessage.ShowMessage(responseMessage, LMessage.MessageBoxType.Error);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_TINH_THANH,
                DatabaseConstant.Table.DM_TINH_TP,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Sau khi xóa
        /// </summary>
        /// <param name="ret"></param>
        private void afterDelete(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);
            List<int> listUnlockidTinhTP = new List<int>();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_TINH_THANH,
                DatabaseConstant.Table.DM_TINH_TP,
                DatabaseConstant.Action.XOA,
                listLockId);

            // Đóng cửa sổ chi tiết sau khi xóa thành công
            if (ret)
            {
                onClose();
            }
        }

        /// <summary>
        /// Sau khi duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void afterApprove(bool ret)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);

                TthaiNvu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);
                txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);

                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_TINH_THANH);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.DuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_TINH_THANH,
                DatabaseConstant.Table.DM_TINH_TP,
                DatabaseConstant.Action.DUYET,
                listLockId);
        }

        /// <summary>
        /// Sau khi thoái duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void afterCancel(bool ret)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);

                TthaiNvu = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);
                txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);

                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_TINH_THANH);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.ThoaiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_TINH_THANH,
                DatabaseConstant.Table.DM_TINH_TP,
                DatabaseConstant.Action.THOAI_DUYET,
                listLockId);
        }

        /// <summary>
        /// Sau khi từ chối
        /// </summary>
        /// <param name="ret"></param>
        private void afterRefuse(bool ret)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);

                TthaiNvu = BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);
                txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);

                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_TINH_THANH);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.TuChoiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_TINH_THANH,
                DatabaseConstant.Table.DM_TINH_TP,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                listLockId);
        }

        #endregion

    }
}
