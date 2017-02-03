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

using Utilities.Common;
using Presentation.Process;
using Presentation.Process.Common;
using PresentationWPF.CustomControl;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.DanhMucServiceRef;
using Microsoft.Windows.Controls.Ribbon;
using PresentationWPF;

namespace PresentationWPF.DanhMuc.DonVi
{
    /// <summary>
    /// Interaction logic for ucDonViCT.xaml
    /// </summary>
    public partial class ucDonViCT : UserControl
    {
        #region Khai bao
        public static DatabaseConstant.Module Module = DatabaseConstant.Module.DMDC;
        public static DatabaseConstant.Function Function = DatabaseConstant.Function.DC_DM_DON_VI;
        public static DatabaseConstant.Table Table = DatabaseConstant.Table.DM_DON_VI;
        public static DatabaseConstant.Action Action;

        public event EventHandler OnSavingCompleted;

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

        private byte[] imageData = null;
        private string imageName = "";

        private string idLoaiDonVi;
        private string maLoaiDonVi;
        private List<AutoCompleteEntry> lstComboBoxLoaiDonVi = new List<AutoCompleteEntry>();

        private string idDonViCha;
        private string maDonViCha;
        private List<AutoCompleteEntry> lstComboBoxDonViCha = new List<AutoCompleteEntry>();

        private string idChiNhanh;
        private string maChiNhanh;
        private List<AutoCompleteEntry> lstComboBoxChiNhanh = new List<AutoCompleteEntry>();

        private string idPhongGD;
        private string maPhongGD;
        private List<AutoCompleteEntry> lstComboBoxPhongGD = new List<AutoCompleteEntry>();

        private string idTinhTP;
        private string maTinhTP;
        private List<AutoCompleteEntry> lstComboBoxTinhTP = new List<AutoCompleteEntry>();

        private string idHachToan;
        private string maHachToan;
        private List<AutoCompleteEntry> lstComboBoxHachToan = new List<AutoCompleteEntry>();

        private string idLichHop;
        private string maLichHop;
        private List<AutoCompleteEntry> lstComboBoxLichHop = new List<AutoCompleteEntry>();

        List<BankAccountInfo> bankAccountList = new List<BankAccountInfo>();
        List<DM_DON_VI_TKHOAN> lstDmDonViTKhoan = new List<DM_DON_VI_TKHOAN>();
        #endregion

        #region Khoi tao

        /// <summary>
        /// Khởi tạo ucDonVi trường hợp thêm mới
        /// </summary>
        public ucDonViCT()
        {
            InitializeComponent();            

            InitEventHandler();            

            BindShortkey();

            LoadCombobox();

            ResetForm();

            // Refresh buttons
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_DON_VI);

            bankAccountList = new List<BankAccountInfo>
            {
            };
            grdTaiKhoan.ItemsSource = bankAccountList;
            cmbChiNhanh.Focus();            
        }

        /// <summary>
        /// Khởi tạo các combobox
        /// </summary>
        private void LoadCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.THUOC_TINH_DON_VI.getValue());
            lstComboBoxLoaiDonVi = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstComboBoxLoaiDonVi, ref cmbLoaiDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
            auto.removeEntry(ref lstComboBoxLoaiDonVi, ref cmbLoaiDonVi, DatabaseConstant.ToChucDonVi.DVI.getValue());
            auto.removeEntry(ref lstComboBoxLoaiDonVi, ref cmbLoaiDonVi, DatabaseConstant.ToChucDonVi.HSO.getValue());
            auto.removeEntry(ref lstComboBoxLoaiDonVi, ref cmbLoaiDonVi, DatabaseConstant.ToChucDonVi.VPGD.getValue());
            auto.removeEntry(ref lstComboBoxLoaiDonVi, ref cmbLoaiDonVi, DatabaseConstant.ToChucDonVi.VDGD.getValue());

            if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()) ||
                ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()))
            {
                auto.removeEntry(ref lstComboBoxLoaiDonVi, ref cmbLoaiDonVi, DatabaseConstant.ToChucDonVi.DVI.getValue());
                auto.removeEntry(ref lstComboBoxLoaiDonVi, ref cmbLoaiDonVi, DatabaseConstant.ToChucDonVi.CNH.getValue());
            }

            cmbTinhTP.Items.Clear();
            lstDieuKien.Clear();
            lstComboBoxTinhTP = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstComboBoxTinhTP, ref cmbTinhTP, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TINHTP.getValue(), lstDieuKien);

            cmbLichHop.Items.Clear();
            lstDieuKien.Clear();
            lstComboBoxLichHop = new List<AutoCompleteEntry>();
            lstComboBoxLichHop.Insert(0, new AutoCompleteEntry("", "", ""));
            auto.GenAutoComboBox(ref lstComboBoxLichHop, ref cmbLichHop, DatabaseConstant.DanhSachTruyVan.COMBOBOX_LICHHOP.getValue(), lstDieuKien);

            cmbHachToan.Items.Clear();
            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DON_VI_HACH_TOAN.getValue());
            lstComboBoxHachToan = new List<AutoCompleteEntry>();
            lstComboBoxHachToan.Insert(0, new AutoCompleteEntry("", "", ""));
            auto.GenAutoComboBox(ref lstComboBoxHachToan, ref cmbHachToan, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
        }
        
        /// <summary>
        /// InitEventHandler
        /// </summary>
        private void InitEventHandler()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.DanhMuc;component/DonVi/ucDonViCT.xaml", ref Toolbar, ref mnuMain);

            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }

            cmbLoaiDonVi.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiDonVi_SelectionChanged);
            cmbLoaiDonVi.KeyDown += new KeyEventHandler(cmbLoaiDonVi_KeyDown);
            cmbLoaiDonVi.LostFocus += new RoutedEventHandler(cmbLoaiDonVi_LostFocus);

            cmbDonViCha.SelectionChanged += new SelectionChangedEventHandler(cmbDonViCha_SelectionChanged);
            cmbDonViCha.KeyDown += new KeyEventHandler(cmbDonViCha_KeyDown);
            cmbDonViCha.LostFocus += new RoutedEventHandler(cmbDonViCha_LostFocus);

            cmbTinhTP.SelectionChanged += new SelectionChangedEventHandler(cmbTinhTP_SelectionChanged);
            cmbTinhTP.KeyDown += new KeyEventHandler(cmbTinhTP_KeyDown);
            cmbTinhTP.LostFocus += new RoutedEventHandler(cmbTinhTP_LostFocus);

            cmbHachToan.SelectionChanged += new SelectionChangedEventHandler(cmbHachToan_SelectionChanged);
            cmbHachToan.KeyDown += new KeyEventHandler(cmbHachToan_KeyDown);
            cmbHachToan.LostFocus += new RoutedEventHandler(cmbHachToan_LostFocus);

            cmbLichHop.SelectionChanged += new SelectionChangedEventHandler(cmbLichHop_SelectionChanged);
            cmbLichHop.KeyDown += new KeyEventHandler(cmbLichHop_KeyDown);
            cmbLichHop.LostFocus += new RoutedEventHandler(cmbLichHop_LostFocus);

            cmbChiNhanh.SelectionChanged += new SelectionChangedEventHandler(cmbChiNhanh_SelectionChanged);
        }

        /// <summary>
        /// Đưa form về trạng thái mặc định
        /// </summary>
        private void ResetForm()
        {
            id = 0;
            tthaiNvu = "";

            txtMaDonVi.Text = "";
            txtTenGiaoDich.Text = "";
            txtTentat.Text = "";
            txtDiaChi.Text = "";

            txtMaNHNN.Text = "";
            txtDienThoai.Text = "";
            txtSoFax.Text = "";
            txtWebsite.Text = "";
            txtEmail.Text = "";
            imgLogo.Tag = "";
            txtSoDKKD.Text = "";
            txtSoTaiKhoan.Text = "";
            txtTenNganHang.Text = "";
            txtMaSoThue.Text = "";

            cmbHachToan.SelectedIndex = 0;
            txtTenGD.Text = "";
            txtPhoneGD.Text = "";
            txtTelGD.Text = "";
            txtFaxGD.Text = "";
            txtEmailGD.Text = "";
            txtTenKeToan.Text = "";
            txtPhoneKeToan.Text = "";
            txtTelKeToan.Text = "";
            txtFaxKeToan.Text = "";
            txtEmailKeToan.Text = "";
            raddtNgayNhap.Text = "";
            raddtNgayCNhat.Text = "";
            txtNguoiCNhat.Text = "";
            txtNguoiNhap.Text = "";
            txtTrangThaiBanGhi.Text = "";
            lblTrangThaiBanGhi.Content = "";
            ResetImage();

            txtMaDonVi.IsEnabled = false;

            lblDonViCha.Visibility = Visibility.Hidden;
            lblRequiredDonViCha.Visibility = Visibility.Hidden;
            cmbDonViCha.Visibility = Visibility.Hidden;

            //tbiThongTinChung.Focus();
            cmbChiNhanh.Focus();

            if (!ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.M7MFI.layGiaTri()))
            {
                lblHachToan.Visibility = Visibility.Collapsed;
                cmbHachToan.Visibility = Visibility.Collapsed;
                lblRequiredHachToan.Visibility = Visibility.Collapsed;

                lblLichHop.Visibility = Visibility.Collapsed;
                cmbLichHop.Visibility = Visibility.Collapsed;
                lblRequiredLichHop.Visibility = Visibility.Collapsed;

                tbiTaiKhoan.Visibility = Visibility.Collapsed;
            }
        }
        
        #endregion

        #region Dang ky hot key, shortcut key

        /// <summary>
        /// Định nghĩa phím tắt
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
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.N, ModifierKeys.Control);
                        key = new KeyBinding(AddCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.H, ModifierKeys.Control);
                        key = new KeyBinding(ucDonViCT.HoldCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(ucDonViCT.ModifyCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.Shift);
                        key = new KeyBinding(ucDonViCT.DeleteCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.A, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucDonViCT.ApproveCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucDonViCT.RefuseCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.C, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucDonViCT.CancelCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(ucDonViCT.HelpCommand, keyg);
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
            ResetForm();
            SetEnabledRequiredControls(false);
            SetEnabledAllControls(true);
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_DON_VI);
        }

        private void HoldCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbHold.IsEnabled;
        }
        private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onHold();
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbSave.IsEnabled;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onSave();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbModify.IsEnabled;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeModifyFromDetail();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbDelete.IsEnabled;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeDelete();
        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbApprove.IsEnabled;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onApprove();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbRefuse.IsEnabled;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onRefuse();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbCancel.IsEnabled;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onCancel();
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
                ResetForm();
                SetEnabledRequiredControls(false);
                SetEnabledAllControls(true);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_DON_VI);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                onHold();
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                beforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                beforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                beforeRefuse();
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
                ResetForm();
                SetEnabledRequiredControls(false);
                SetEnabledAllControls(true);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_DON_VI);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                onHold();
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                beforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                beforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                beforeRefuse();
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

        #region Xu ly giao dien

        /// <summary>
        /// SetEnabledAllControls
        /// </summary>
        /// <param name="enable"></param>
        private void SetEnabledAllControls(bool enable)
        {
            cbMultiAdd.IsEnabled = enable;

            cmbLoaiDonVi.IsEnabled = enable;
            chkLaHoiSoChinh.IsEnabled = enable;
            chkLaVPChiNhanh.IsEnabled = enable;
            chkLaVPPhongGD.IsEnabled = enable;
            cmbDonViCha.IsEnabled = enable;
            cmbTinhTP.IsEnabled = enable;
            cmbHachToan.IsEnabled = enable;
            cmbLichHop.IsEnabled = enable;

            cmbChiNhanh.IsEnabled = enable;
            cmbPhongGD.IsEnabled = enable;

            txtMaDonVi.IsEnabled = enable;
            txtTenGiaoDich.IsEnabled = enable;
            txtTentat.IsEnabled = enable;
            txtDiaChi.IsEnabled = enable;
            txtDienThoai.IsEnabled = enable;
            txtSoFax.IsEnabled = enable;
            txtWebsite.IsEnabled = enable;
            txtDiaChi.IsEnabled = enable;
            imgLogo.IsEnabled = enable;
            txtEmail.IsEnabled = enable;
            txtMaNHNN.IsEnabled = enable;
            raddtNgayHoatDong.IsEnabled = enable;
            dtpNgayHoatDong.IsEnabled = enable;
            txtSoDKKD.IsEnabled = enable;
            txtMaSoThue.IsEnabled = enable;
            txtSoTaiKhoan.IsEnabled = enable;
            txtTenNganHang.IsEnabled = enable;
            txtTenGD.IsEnabled = enable;
            txtTelGD.IsEnabled = enable;
            txtPhoneGD.IsEnabled = enable;
            txtFaxGD.IsEnabled = enable;
            txtEmailGD.IsEnabled = enable;
            txtTenKeToan.IsEnabled = enable;
            txtTelKeToan.IsEnabled = enable;
            txtPhoneKeToan.IsEnabled = enable;
            txtFaxKeToan.IsEnabled = enable;
            txtEmailKeToan.IsEnabled = enable;
            txtTrangThaiBanGhi.IsEnabled = enable;
            txtNguoiNhap.IsEnabled = enable;
            txtNguoiCNhat.IsEnabled = enable;
            lblTrangThaiBanGhi.IsEnabled = enable;
            raddtNgayNhap.IsEnabled = enable;
            raddtNgayCNhat.IsEnabled = enable;
        }

        /// <summary>
        /// SetEnabledRequiredControls
        /// </summary>
        /// <param name="enable"></param>
        private void SetEnabledRequiredControls(bool enable)
        {
            // Không cho phép sửa thông tin ảnh hưởng tới mã và quy tắc sinh mã
            cmbLoaiDonVi.IsEnabled = enable;
            chkLaHoiSoChinh.IsEnabled = enable;
            chkLaVPChiNhanh.IsEnabled = enable;
            chkLaVPPhongGD.IsEnabled = enable;
            cmbDonViCha.IsEnabled = enable;
            txtMaDonVi.IsEnabled = enable;

            cmbChiNhanh.IsEnabled = enable;
            cmbPhongGD.IsEnabled = enable;

            // Không cho phép sửa ngày hoạt động (vì ảnh hưởng tới ngày làm việc)
            raddtNgayHoatDong.IsEnabled = enable;
            dtpNgayHoatDong.IsEnabled = enable;

            // Không cho phép sửa phương thức hạch toán
            cmbHachToan.IsEnabled = enable;

            // Không cho phép sửa lịch họp (vì ảnh hưởng tới tần suất họp cụm)
            cmbLichHop.IsEnabled = enable;
        }
                
        /// <summary>
        /// Sự kiện đóng
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuDong_Click(object sender, RoutedEventArgs e)
        {
            Release();
            if (OnSavingCompleted != null)
            {
                OnSavingCompleted(this, EventArgs.Empty);
            }
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
                Release();
                if (OnSavingCompleted != null)
                {
                    OnSavingCompleted(this, EventArgs.Empty);
                }
                PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            }
            else
            {
                CustomControl.CommonFunction.SelectNextControl(e);
            }
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
                DatabaseConstant.Function.DC_DM_DON_VI,
                DatabaseConstant.Table.DM_DON_VI,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Xử lý sự kiện SelectionChanged cho Loại đơn vị
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbLoaiDonVi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = cmbLoaiDonVi.Text;
            List<string> lstDieuKien = new List<string>();
            AutoComboBox auto = new AutoComboBox();
            AutoCompleteEntry entry = auto.checkDisplayName(lstComboBoxLoaiDonVi, ref cmbLoaiDonVi);

            if (entry == null)
            {
                lblDonViCha.Visibility = Visibility.Collapsed;
                lblRequiredDonViCha.Visibility = Visibility.Collapsed;
                cmbDonViCha.Visibility = Visibility.Collapsed;

                cmbLichHop.Visibility = Visibility.Collapsed;
                lblLichHop.Visibility = Visibility.Collapsed;
                lblRequiredLichHop.Visibility = Visibility.Collapsed;
                cmbHachToan.Visibility = Visibility.Collapsed;
                lblHachToan.Visibility = Visibility.Collapsed;
                lblRequiredHachToan.Visibility = Visibility.Collapsed;

                chkLaHoiSoChinh.Visibility = Visibility.Collapsed;
                chkLaVPChiNhanh.Visibility = Visibility.Collapsed;
                chkLaVPPhongGD.Visibility = Visibility.Collapsed;

                return;
            }
            else
            {
                maLoaiDonVi = entry.KeywordStrings.First();
                idLoaiDonVi = entry.KeywordStrings.ElementAt(1);

                if (maLoaiDonVi.Equals(DatabaseConstant.ToChucDonVi.DVI.getValue()))
                {
                    lblDonViCha.Visibility = Visibility.Collapsed;
                    lblRequiredDonViCha.Visibility = Visibility.Collapsed;
                    cmbDonViCha.Visibility = Visibility.Collapsed;

                    cmbLichHop.Visibility = Visibility.Collapsed;
                    lblLichHop.Visibility = Visibility.Collapsed;
                    lblRequiredLichHop.Visibility = Visibility.Collapsed;
                    cmbHachToan.Visibility = Visibility.Collapsed;
                    lblHachToan.Visibility = Visibility.Collapsed;
                    lblRequiredHachToan.Visibility = Visibility.Collapsed;

                    lblChiNhanh.Visibility = Visibility.Collapsed;
                    lblRequiredChiNhanh.Visibility = Visibility.Collapsed;
                    cmbChiNhanh.Visibility = Visibility.Collapsed;

                    lblPhongGD.Visibility = Visibility.Collapsed;
                    lblRequiredPhongGD.Visibility = Visibility.Collapsed;
                    cmbPhongGD.Visibility = Visibility.Collapsed;

                    chkLaHoiSoChinh.Visibility = Visibility.Collapsed;
                    chkLaVPChiNhanh.Visibility = Visibility.Collapsed;
                    chkLaVPPhongGD.Visibility = Visibility.Collapsed;
                }
                else if (maLoaiDonVi.Equals(DatabaseConstant.ToChucDonVi.HSO.getValue()) || maLoaiDonVi.Equals(DatabaseConstant.ToChucDonVi.CNH.getValue()))
                {
                    lblDonViCha.Visibility = Visibility.Collapsed;
                    lblRequiredDonViCha.Visibility = Visibility.Collapsed;
                    cmbDonViCha.Visibility = Visibility.Collapsed;

                    cmbLichHop.Visibility = Visibility.Visible;
                    lblLichHop.Visibility = Visibility.Visible;
                    lblRequiredLichHop.Visibility = Visibility.Visible;
                    // NgọcDTM yêu cầu
                    cmbHachToan.Visibility = Visibility.Collapsed;
                    lblHachToan.Visibility = Visibility.Collapsed;
                    lblRequiredHachToan.Visibility = Visibility.Collapsed;

                    lblChiNhanh.Visibility = Visibility.Collapsed;
                    lblRequiredChiNhanh.Visibility = Visibility.Collapsed;
                    cmbChiNhanh.Visibility = Visibility.Collapsed;

                    lblPhongGD.Visibility = Visibility.Collapsed;
                    lblRequiredPhongGD.Visibility = Visibility.Collapsed;
                    cmbPhongGD.Visibility = Visibility.Collapsed;

                    chkLaHoiSoChinh.Visibility = Visibility.Visible;
                    chkLaVPChiNhanh.Visibility = Visibility.Collapsed;
                    chkLaVPPhongGD.Visibility = Visibility.Collapsed;

                    //List<string> lstDieuKien = new List<string>();
                    //lstDieuKien.Add("'" + DatabaseConstant.ToChucDonVi.DVI.getValue() + "'");
                    //cmbDonViCha.Items.Clear();
                    //lstComboBoxDonViCha = new List<AutoCompleteEntry>();
                    //auto.GenAutoComboBox(ref lstComboBoxDonViCha, ref cmbDonViCha, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOLOAI.getValue(), lstDieuKien);
                }
                else if (maLoaiDonVi.Equals(DatabaseConstant.ToChucDonVi.PGD.getValue()) || maLoaiDonVi.Equals(DatabaseConstant.ToChucDonVi.VPGD.getValue()))
                {
                    lblDonViCha.Visibility = Visibility.Collapsed;
                    lblRequiredDonViCha.Visibility = Visibility.Collapsed;
                    cmbDonViCha.Visibility = Visibility.Collapsed;

                    cmbLichHop.Visibility = Visibility.Collapsed;
                    lblLichHop.Visibility = Visibility.Collapsed;
                    lblRequiredLichHop.Visibility = Visibility.Collapsed;
                    cmbHachToan.Visibility = Visibility.Visible;
                    lblHachToan.Visibility = Visibility.Visible;
                    lblRequiredHachToan.Visibility = Visibility.Visible;

                    lblChiNhanh.Visibility = Visibility.Visible;
                    lblRequiredChiNhanh.Visibility = Visibility.Visible;
                    cmbChiNhanh.Visibility = Visibility.Visible;

                    lblPhongGD.Visibility = Visibility.Collapsed;
                    lblRequiredPhongGD.Visibility = Visibility.Collapsed;
                    cmbPhongGD.Visibility = Visibility.Collapsed;

                    chkLaHoiSoChinh.Visibility = Visibility.Collapsed;
                    chkLaVPChiNhanh.Visibility = Visibility.Visible;
                    chkLaVPPhongGD.Visibility = Visibility.Collapsed;                    

                    //List<string> lstDieuKien = new List<string>();
                    //lstDieuKien.Add("'" + DatabaseConstant.ToChucDonVi.HSO.getValue() + "', '" + DatabaseConstant.ToChucDonVi.CNH.getValue() + "'");
                    //cmbDonViCha.Items.Clear();
                    //lstComboBoxDonViCha = new List<AutoCompleteEntry>();
                    //auto.GenAutoComboBox(ref lstComboBoxDonViCha, ref cmbDonViCha, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOLOAI.getValue(), lstDieuKien);

                    // khởi tạo combobox
                    auto = new AutoComboBox();
                    cmbChiNhanh.Items.Clear();
                    lstComboBoxChiNhanh = new List<AutoCompleteEntry>();
                    auto.GenAutoComboBox(ref lstComboBoxChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);
                }
                else if (maLoaiDonVi.Equals(DatabaseConstant.ToChucDonVi.DGD.getValue()) || maLoaiDonVi.Equals(DatabaseConstant.ToChucDonVi.VDGD.getValue()))
                {
                    lblDonViCha.Visibility = Visibility.Collapsed;
                    lblRequiredDonViCha.Visibility = Visibility.Collapsed;
                    cmbDonViCha.Visibility = Visibility.Collapsed;

                    cmbLichHop.Visibility = Visibility.Collapsed;
                    lblLichHop.Visibility = Visibility.Collapsed;
                    lblRequiredLichHop.Visibility = Visibility.Collapsed;
                    cmbHachToan.Visibility = Visibility.Collapsed;
                    lblHachToan.Visibility = Visibility.Collapsed;
                    lblRequiredHachToan.Visibility = Visibility.Collapsed;

                    lblChiNhanh.Visibility = Visibility.Visible;
                    lblRequiredChiNhanh.Visibility = Visibility.Visible;
                    cmbChiNhanh.Visibility = Visibility.Visible;

                    lblPhongGD.Visibility = Visibility.Visible;
                    lblRequiredPhongGD.Visibility = Visibility.Visible;
                    cmbPhongGD.Visibility = Visibility.Visible;

                    chkLaHoiSoChinh.Visibility = Visibility.Collapsed;
                    chkLaVPChiNhanh.Visibility = Visibility.Collapsed;
                    chkLaVPPhongGD.Visibility = Visibility.Collapsed;

                    //List<string> lstDieuKien = new List<string>();
                    //lstDieuKien.Add("'" + DatabaseConstant.ToChucDonVi.PGD.getValue() + "', '" + DatabaseConstant.ToChucDonVi.VPGD.getValue() + "'");
                    //cmbDonViCha.Items.Clear();
                    //lstComboBoxDonViCha = new List<AutoCompleteEntry>();
                    //auto.GenAutoComboBox(ref lstComboBoxDonViCha, ref cmbDonViCha, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOLOAI.getValue(), lstDieuKien);

                    // khởi tạo combobox
                    auto = new AutoComboBox();
                    cmbChiNhanh.Items.Clear();
                    lstComboBoxChiNhanh = new List<AutoCompleteEntry>();
                    auto.GenAutoComboBox(ref lstComboBoxChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);

                    cmbPhongGD.Items.Clear();
                    lstComboBoxPhongGD.Clear();
                    lstDieuKien.Clear();
                    lstDieuKien.Add(ClientInformation.MaDonVi);
                    auto.GenAutoComboBox(ref lstComboBoxPhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOMACHA.getValue(), lstDieuKien);
                }
            }

            if (!ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.M7MFI.layGiaTri()))
            {
                lblHachToan.Visibility = Visibility.Collapsed;
                cmbHachToan.Visibility = Visibility.Collapsed;
                lblRequiredHachToan.Visibility = Visibility.Collapsed;

                lblLichHop.Visibility = Visibility.Collapsed;
                cmbLichHop.Visibility = Visibility.Collapsed;
                lblRequiredLichHop.Visibility = Visibility.Collapsed;

                tbiTaiKhoan.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Xử lý sự kiện LostFocus cho Loại đơn vị
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbLoaiDonVi_LostFocus(object sender, RoutedEventArgs e)
        {
            string text = cmbLoaiDonVi.Text;
            List<string> lstDieuKien = new List<string>();
            AutoComboBox auto = new AutoComboBox();
            AutoCompleteEntry entry = auto.checkDisplayName(lstComboBoxLoaiDonVi, ref cmbLoaiDonVi);

            if (entry == null)
            {
                lblDonViCha.Visibility = Visibility.Collapsed;
                lblRequiredDonViCha.Visibility = Visibility.Collapsed;
                cmbDonViCha.Visibility = Visibility.Collapsed;

                cmbLichHop.Visibility = Visibility.Collapsed;
                lblLichHop.Visibility = Visibility.Collapsed;
                lblRequiredLichHop.Visibility = Visibility.Collapsed;
                cmbHachToan.Visibility = Visibility.Collapsed;
                lblHachToan.Visibility = Visibility.Collapsed;
                lblRequiredHachToan.Visibility = Visibility.Collapsed;

                chkLaHoiSoChinh.Visibility = Visibility.Collapsed;
                chkLaVPChiNhanh.Visibility = Visibility.Collapsed;
                chkLaVPPhongGD.Visibility = Visibility.Collapsed;

                return;
            }
            else
            {
                maLoaiDonVi = entry.KeywordStrings.First();
                idLoaiDonVi = entry.KeywordStrings.ElementAt(1);

                if (maLoaiDonVi.Equals(DatabaseConstant.ToChucDonVi.DVI.getValue()))
                {
                    lblDonViCha.Visibility = Visibility.Collapsed;
                    lblRequiredDonViCha.Visibility = Visibility.Collapsed;
                    cmbDonViCha.Visibility = Visibility.Collapsed;

                    cmbLichHop.Visibility = Visibility.Collapsed;
                    lblLichHop.Visibility = Visibility.Collapsed;
                    lblRequiredLichHop.Visibility = Visibility.Collapsed;
                    cmbHachToan.Visibility = Visibility.Collapsed;
                    lblHachToan.Visibility = Visibility.Collapsed;
                    lblRequiredHachToan.Visibility = Visibility.Collapsed;

                    lblChiNhanh.Visibility = Visibility.Collapsed;
                    lblRequiredChiNhanh.Visibility = Visibility.Collapsed;
                    cmbChiNhanh.Visibility = Visibility.Collapsed;

                    lblPhongGD.Visibility = Visibility.Collapsed;
                    lblRequiredPhongGD.Visibility = Visibility.Collapsed;
                    cmbPhongGD.Visibility = Visibility.Collapsed;

                    chkLaHoiSoChinh.Visibility = Visibility.Collapsed;
                    chkLaVPChiNhanh.Visibility = Visibility.Collapsed;
                    chkLaVPPhongGD.Visibility = Visibility.Collapsed;
                }
                else if (maLoaiDonVi.Equals(DatabaseConstant.ToChucDonVi.HSO.getValue()) || maLoaiDonVi.Equals(DatabaseConstant.ToChucDonVi.CNH.getValue()))
                {
                    lblDonViCha.Visibility = Visibility.Collapsed;
                    lblRequiredDonViCha.Visibility = Visibility.Collapsed;
                    cmbDonViCha.Visibility = Visibility.Collapsed;

                    cmbLichHop.Visibility = Visibility.Visible;
                    lblLichHop.Visibility = Visibility.Visible;
                    lblRequiredLichHop.Visibility = Visibility.Visible;
                    // NgọcDTM yêu cầu
                    cmbHachToan.Visibility = Visibility.Collapsed;
                    lblHachToan.Visibility = Visibility.Collapsed;
                    lblRequiredHachToan.Visibility = Visibility.Collapsed;

                    lblChiNhanh.Visibility = Visibility.Collapsed;
                    lblRequiredChiNhanh.Visibility = Visibility.Collapsed;
                    cmbChiNhanh.Visibility = Visibility.Collapsed;

                    lblPhongGD.Visibility = Visibility.Collapsed;
                    lblRequiredPhongGD.Visibility = Visibility.Collapsed;
                    cmbPhongGD.Visibility = Visibility.Collapsed;

                    chkLaHoiSoChinh.Visibility = Visibility.Visible;
                    chkLaVPChiNhanh.Visibility = Visibility.Collapsed;
                    chkLaVPPhongGD.Visibility = Visibility.Collapsed;

                    //List<string> lstDieuKien = new List<string>();
                    //lstDieuKien.Add("'" + DatabaseConstant.ToChucDonVi.DVI.getValue() + "'");
                    //cmbDonViCha.Items.Clear();
                    //lstComboBoxDonViCha = new List<AutoCompleteEntry>();
                    //auto.GenAutoComboBox(ref lstComboBoxDonViCha, ref cmbDonViCha, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOLOAI.getValue(), lstDieuKien);
                }
                else if (maLoaiDonVi.Equals(DatabaseConstant.ToChucDonVi.PGD.getValue()) || maLoaiDonVi.Equals(DatabaseConstant.ToChucDonVi.VPGD.getValue()))
                {
                    lblDonViCha.Visibility = Visibility.Collapsed;
                    lblRequiredDonViCha.Visibility = Visibility.Collapsed;
                    cmbDonViCha.Visibility = Visibility.Collapsed;

                    cmbLichHop.Visibility = Visibility.Collapsed;
                    lblLichHop.Visibility = Visibility.Collapsed;
                    lblRequiredLichHop.Visibility = Visibility.Collapsed;
                    cmbHachToan.Visibility = Visibility.Visible;
                    lblHachToan.Visibility = Visibility.Visible;
                    lblRequiredHachToan.Visibility = Visibility.Visible;

                    lblChiNhanh.Visibility = Visibility.Visible;
                    lblRequiredChiNhanh.Visibility = Visibility.Visible;
                    cmbChiNhanh.Visibility = Visibility.Visible;

                    lblPhongGD.Visibility = Visibility.Collapsed;
                    lblRequiredPhongGD.Visibility = Visibility.Collapsed;
                    cmbPhongGD.Visibility = Visibility.Collapsed;

                    chkLaHoiSoChinh.Visibility = Visibility.Collapsed;
                    chkLaVPChiNhanh.Visibility = Visibility.Visible;
                    chkLaVPPhongGD.Visibility = Visibility.Collapsed;

                    //List<string> lstDieuKien = new List<string>();
                    //lstDieuKien.Add("'" + DatabaseConstant.ToChucDonVi.HSO.getValue() + "', '" + DatabaseConstant.ToChucDonVi.CNH.getValue() + "'");
                    //cmbDonViCha.Items.Clear();
                    //lstComboBoxDonViCha = new List<AutoCompleteEntry>();
                    //auto.GenAutoComboBox(ref lstComboBoxDonViCha, ref cmbDonViCha, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOLOAI.getValue(), lstDieuKien);

                    // khởi tạo combobox
                    auto = new AutoComboBox();
                    cmbChiNhanh.Items.Clear();
                    lstComboBoxChiNhanh = new List<AutoCompleteEntry>();
                    auto.GenAutoComboBox(ref lstComboBoxChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);
                }
                else if (maLoaiDonVi.Equals(DatabaseConstant.ToChucDonVi.DGD.getValue()) || maLoaiDonVi.Equals(DatabaseConstant.ToChucDonVi.VDGD.getValue()))
                {
                    lblDonViCha.Visibility = Visibility.Collapsed;
                    lblRequiredDonViCha.Visibility = Visibility.Collapsed;
                    cmbDonViCha.Visibility = Visibility.Collapsed;

                    cmbLichHop.Visibility = Visibility.Collapsed;
                    lblLichHop.Visibility = Visibility.Collapsed;
                    lblRequiredLichHop.Visibility = Visibility.Collapsed;
                    cmbHachToan.Visibility = Visibility.Collapsed;
                    lblHachToan.Visibility = Visibility.Collapsed;
                    lblRequiredHachToan.Visibility = Visibility.Collapsed;

                    lblChiNhanh.Visibility = Visibility.Visible;
                    lblRequiredChiNhanh.Visibility = Visibility.Visible;
                    cmbChiNhanh.Visibility = Visibility.Visible;

                    lblPhongGD.Visibility = Visibility.Visible;
                    lblRequiredPhongGD.Visibility = Visibility.Visible;
                    cmbPhongGD.Visibility = Visibility.Visible;

                    chkLaHoiSoChinh.Visibility = Visibility.Collapsed;
                    chkLaVPChiNhanh.Visibility = Visibility.Collapsed;
                    chkLaVPPhongGD.Visibility = Visibility.Collapsed;

                    //List<string> lstDieuKien = new List<string>();
                    //lstDieuKien.Add("'" + DatabaseConstant.ToChucDonVi.PGD.getValue() + "', '" + DatabaseConstant.ToChucDonVi.VPGD.getValue() + "'");
                    //cmbDonViCha.Items.Clear();
                    //lstComboBoxDonViCha = new List<AutoCompleteEntry>();
                    //auto.GenAutoComboBox(ref lstComboBoxDonViCha, ref cmbDonViCha, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOLOAI.getValue(), lstDieuKien);

                    // khởi tạo combobox
                    auto = new AutoComboBox();
                    cmbChiNhanh.Items.Clear();
                    lstComboBoxChiNhanh = new List<AutoCompleteEntry>();
                    auto.GenAutoComboBox(ref lstComboBoxChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);

                    cmbPhongGD.Items.Clear();
                    lstComboBoxPhongGD.Clear();
                    lstDieuKien.Clear();
                    lstDieuKien.Add(ClientInformation.MaDonVi);
                    auto.GenAutoComboBox(ref lstComboBoxPhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOMACHA.getValue(), lstDieuKien);
                }
            }

            if (!ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.M7MFI.layGiaTri()))
            {
                lblHachToan.Visibility = Visibility.Collapsed;
                cmbHachToan.Visibility = Visibility.Collapsed;
                lblRequiredHachToan.Visibility = Visibility.Collapsed;

                lblLichHop.Visibility = Visibility.Collapsed;
                cmbLichHop.Visibility = Visibility.Collapsed;
                lblRequiredLichHop.Visibility = Visibility.Collapsed;

                tbiTaiKhoan.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Xử lý sự kiện KeyDown cho Loại đơn vị
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbLoaiDonVi_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                string text = cmbLoaiDonVi.Text;
                List<string> lstDieuKien = new List<string>();
                AutoComboBox auto = new AutoComboBox();
                AutoCompleteEntry entry = auto.checkDisplayName(lstComboBoxLoaiDonVi, ref cmbLoaiDonVi);

                if (entry == null)
                {
                    lblDonViCha.Visibility = Visibility.Collapsed;
                    lblRequiredDonViCha.Visibility = Visibility.Collapsed;
                    cmbDonViCha.Visibility = Visibility.Collapsed;

                    cmbLichHop.Visibility = Visibility.Collapsed;
                    lblLichHop.Visibility = Visibility.Collapsed;
                    lblRequiredLichHop.Visibility = Visibility.Collapsed;
                    cmbHachToan.Visibility = Visibility.Collapsed;
                    lblHachToan.Visibility = Visibility.Collapsed;
                    lblRequiredHachToan.Visibility = Visibility.Collapsed;

                    chkLaHoiSoChinh.Visibility = Visibility.Collapsed;
                    chkLaVPChiNhanh.Visibility = Visibility.Collapsed;
                    chkLaVPPhongGD.Visibility = Visibility.Collapsed;

                    return;
                }
                else
                {
                    maLoaiDonVi = entry.KeywordStrings.First();
                    idLoaiDonVi = entry.KeywordStrings.ElementAt(1);

                    if (maLoaiDonVi.Equals(DatabaseConstant.ToChucDonVi.DVI.getValue()))
                    {
                        lblDonViCha.Visibility = Visibility.Collapsed;
                        lblRequiredDonViCha.Visibility = Visibility.Collapsed;
                        cmbDonViCha.Visibility = Visibility.Collapsed;

                        cmbLichHop.Visibility = Visibility.Collapsed;
                        lblLichHop.Visibility = Visibility.Collapsed;
                        lblRequiredLichHop.Visibility = Visibility.Collapsed;
                        cmbHachToan.Visibility = Visibility.Collapsed;
                        lblHachToan.Visibility = Visibility.Collapsed;
                        lblRequiredHachToan.Visibility = Visibility.Collapsed;

                        lblChiNhanh.Visibility = Visibility.Collapsed;
                        lblRequiredChiNhanh.Visibility = Visibility.Collapsed;
                        cmbChiNhanh.Visibility = Visibility.Collapsed;

                        lblPhongGD.Visibility = Visibility.Collapsed;
                        lblRequiredPhongGD.Visibility = Visibility.Collapsed;
                        cmbPhongGD.Visibility = Visibility.Collapsed;

                        chkLaHoiSoChinh.Visibility = Visibility.Collapsed;
                        chkLaVPChiNhanh.Visibility = Visibility.Collapsed;
                        chkLaVPPhongGD.Visibility = Visibility.Collapsed;
                    }
                    else if (maLoaiDonVi.Equals(DatabaseConstant.ToChucDonVi.HSO.getValue()) || maLoaiDonVi.Equals(DatabaseConstant.ToChucDonVi.CNH.getValue()))
                    {
                        lblDonViCha.Visibility = Visibility.Collapsed;
                        lblRequiredDonViCha.Visibility = Visibility.Collapsed;
                        cmbDonViCha.Visibility = Visibility.Collapsed;

                        cmbLichHop.Visibility = Visibility.Visible;
                        lblLichHop.Visibility = Visibility.Visible;
                        lblRequiredLichHop.Visibility = Visibility.Visible;
                        // NgọcDTM yêu cầu
                        cmbHachToan.Visibility = Visibility.Collapsed;
                        lblHachToan.Visibility = Visibility.Collapsed;
                        lblRequiredHachToan.Visibility = Visibility.Collapsed;

                        lblChiNhanh.Visibility = Visibility.Collapsed;
                        lblRequiredChiNhanh.Visibility = Visibility.Collapsed;
                        cmbChiNhanh.Visibility = Visibility.Collapsed;

                        lblPhongGD.Visibility = Visibility.Collapsed;
                        lblRequiredPhongGD.Visibility = Visibility.Collapsed;
                        cmbPhongGD.Visibility = Visibility.Collapsed;

                        chkLaHoiSoChinh.Visibility = Visibility.Visible;
                        chkLaVPChiNhanh.Visibility = Visibility.Collapsed;
                        chkLaVPPhongGD.Visibility = Visibility.Collapsed;

                        //List<string> lstDieuKien = new List<string>();
                        //lstDieuKien.Add("'" + DatabaseConstant.ToChucDonVi.DVI.getValue() + "'");
                        //cmbDonViCha.Items.Clear();
                        //lstComboBoxDonViCha = new List<AutoCompleteEntry>();
                        //auto.GenAutoComboBox(ref lstComboBoxDonViCha, ref cmbDonViCha, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOLOAI.getValue(), lstDieuKien);
                    }
                    else if (maLoaiDonVi.Equals(DatabaseConstant.ToChucDonVi.PGD.getValue()) || maLoaiDonVi.Equals(DatabaseConstant.ToChucDonVi.VPGD.getValue()))
                    {
                        lblDonViCha.Visibility = Visibility.Collapsed;
                        lblRequiredDonViCha.Visibility = Visibility.Collapsed;
                        cmbDonViCha.Visibility = Visibility.Collapsed;

                        cmbLichHop.Visibility = Visibility.Collapsed;
                        lblLichHop.Visibility = Visibility.Collapsed;
                        lblRequiredLichHop.Visibility = Visibility.Collapsed;
                        cmbHachToan.Visibility = Visibility.Visible;
                        lblHachToan.Visibility = Visibility.Visible;
                        lblRequiredHachToan.Visibility = Visibility.Visible;

                        lblChiNhanh.Visibility = Visibility.Visible;
                        lblRequiredChiNhanh.Visibility = Visibility.Visible;
                        cmbChiNhanh.Visibility = Visibility.Visible;

                        lblPhongGD.Visibility = Visibility.Collapsed;
                        lblRequiredPhongGD.Visibility = Visibility.Collapsed;
                        cmbPhongGD.Visibility = Visibility.Collapsed;

                        chkLaHoiSoChinh.Visibility = Visibility.Collapsed;
                        chkLaVPChiNhanh.Visibility = Visibility.Visible;
                        chkLaVPPhongGD.Visibility = Visibility.Collapsed;                    

                        //List<string> lstDieuKien = new List<string>();
                        //lstDieuKien.Add("'" + DatabaseConstant.ToChucDonVi.HSO.getValue() + "', '" + DatabaseConstant.ToChucDonVi.CNH.getValue() + "'");
                        //cmbDonViCha.Items.Clear();
                        //lstComboBoxDonViCha = new List<AutoCompleteEntry>();
                        //auto.GenAutoComboBox(ref lstComboBoxDonViCha, ref cmbDonViCha, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOLOAI.getValue(), lstDieuKien);

                        // khởi tạo combobox
                        auto = new AutoComboBox();
                        cmbChiNhanh.Items.Clear();
                        lstComboBoxChiNhanh = new List<AutoCompleteEntry>();
                        auto.GenAutoComboBox(ref lstComboBoxChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);
                    }
                    else if (maLoaiDonVi.Equals(DatabaseConstant.ToChucDonVi.DGD.getValue()) || maLoaiDonVi.Equals(DatabaseConstant.ToChucDonVi.VDGD.getValue()))
                    {
                        lblDonViCha.Visibility = Visibility.Collapsed;
                        lblRequiredDonViCha.Visibility = Visibility.Collapsed;
                        cmbDonViCha.Visibility = Visibility.Collapsed;

                        cmbLichHop.Visibility = Visibility.Collapsed;
                        lblLichHop.Visibility = Visibility.Collapsed;
                        lblRequiredLichHop.Visibility = Visibility.Collapsed;
                        cmbHachToan.Visibility = Visibility.Collapsed;
                        lblHachToan.Visibility = Visibility.Collapsed;
                        lblRequiredHachToan.Visibility = Visibility.Collapsed;

                        lblChiNhanh.Visibility = Visibility.Visible;
                        lblRequiredChiNhanh.Visibility = Visibility.Visible;
                        cmbChiNhanh.Visibility = Visibility.Visible;

                        lblPhongGD.Visibility = Visibility.Visible;
                        lblRequiredPhongGD.Visibility = Visibility.Visible;
                        cmbPhongGD.Visibility = Visibility.Visible;

                        chkLaHoiSoChinh.Visibility = Visibility.Collapsed;
                        chkLaVPChiNhanh.Visibility = Visibility.Collapsed;
                        chkLaVPPhongGD.Visibility = Visibility.Collapsed;

                        //List<string> lstDieuKien = new List<string>();
                        //lstDieuKien.Add("'" + DatabaseConstant.ToChucDonVi.PGD.getValue() + "', '" + DatabaseConstant.ToChucDonVi.VPGD.getValue() + "'");
                        //cmbDonViCha.Items.Clear();
                        //lstComboBoxDonViCha = new List<AutoCompleteEntry>();
                        //auto.GenAutoComboBox(ref lstComboBoxDonViCha, ref cmbDonViCha, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOLOAI.getValue(), lstDieuKien);

                        // khởi tạo combobox
                        auto = new AutoComboBox();
                        cmbChiNhanh.Items.Clear();
                        lstComboBoxChiNhanh = new List<AutoCompleteEntry>();
                        auto.GenAutoComboBox(ref lstComboBoxChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);

                        cmbPhongGD.Items.Clear();
                        lstComboBoxPhongGD.Clear();
                        lstDieuKien.Clear();
                        lstDieuKien.Add(ClientInformation.MaDonVi);
                        auto.GenAutoComboBox(ref lstComboBoxPhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOMACHA.getValue(), lstDieuKien);
                    }
                }
            }
            else
            {
                return;
            }

            if (!ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.M7MFI.layGiaTri()))
            {
                lblHachToan.Visibility = Visibility.Collapsed;
                cmbHachToan.Visibility = Visibility.Collapsed;
                lblRequiredHachToan.Visibility = Visibility.Collapsed;

                lblLichHop.Visibility = Visibility.Collapsed;
                cmbLichHop.Visibility = Visibility.Collapsed;
                lblRequiredLichHop.Visibility = Visibility.Collapsed;

                tbiTaiKhoan.Visibility = Visibility.Collapsed;
            }

            cmbLoaiDonVi.IsDropDownOpen = true;
        }

        /// <summary>
        /// Xử lý sự kiện SelectionChanged cho Đơn vị cha
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDonViCha_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = cmbDonViCha.Text;
            AutoComboBox auto = new AutoComboBox();
            AutoCompleteEntry entry = auto.checkDisplayName(lstComboBoxDonViCha, ref cmbDonViCha);

            if (entry == null)
            {
                return;
            }
            else
            {
                maDonViCha = entry.KeywordStrings.First();
                idDonViCha = entry.KeywordStrings.ElementAt(1);
            }
        }

        /// <summary>
        /// Xử lý sự kiện KeyDown cho Đơn vị cha
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDonViCha_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                string text = cmbDonViCha.Text;
                AutoComboBox auto = new AutoComboBox();
                AutoCompleteEntry entry = auto.checkDisplayName(lstComboBoxDonViCha, ref cmbDonViCha);

                if (entry == null)
                {
                    return;
                }
                else
                {
                    maDonViCha = entry.KeywordStrings.First();
                    idDonViCha = entry.KeywordStrings.ElementAt(1);
                }
            }
            else
            {
                return;
            }

            cmbDonViCha.IsDropDownOpen = true;
        }

        /// <summary>
        /// Xử lý sự kiện LostFocus cho Đơn vị cha
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDonViCha_LostFocus(object sender, RoutedEventArgs e)
        {
            string text = cmbDonViCha.Text;
            AutoComboBox auto = new AutoComboBox();
            AutoCompleteEntry entry = auto.checkDisplayName(lstComboBoxDonViCha, ref cmbDonViCha);

            if (entry == null)
            {
                return;
            }
            else
            {
                maDonViCha = entry.KeywordStrings.First();
                idDonViCha = entry.KeywordStrings.ElementAt(1);
            }
        }

        /// <summary>
        /// Xử lý sự kiện SelectionChanged cho Tỉnh TP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTinhTP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = cmbTinhTP.Text;
            AutoComboBox auto = new AutoComboBox();
            AutoCompleteEntry entry = auto.checkDisplayName(lstComboBoxTinhTP, ref cmbTinhTP);

            if (entry == null)
            {
                return;
            }
            else
            {
                maTinhTP = entry.KeywordStrings.First();
                idTinhTP = entry.KeywordStrings.ElementAt(1);
            }
        }

        /// <summary>
        /// Xử lý sự kiện KeyDown cho Tỉnh TP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTinhTP_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                string text = cmbTinhTP.Text;
                AutoComboBox auto = new AutoComboBox();
                AutoCompleteEntry entry = auto.checkDisplayName(lstComboBoxTinhTP, ref cmbTinhTP);

                if (entry == null)
                {
                    return;
                }
                else
                {
                    maTinhTP = entry.KeywordStrings.First();
                    idTinhTP = entry.KeywordStrings.ElementAt(1);
                }
            }
            else
            {
                return;
            }
            cmbTinhTP.IsDropDownOpen = true;
        }

        /// <summary>
        /// Xử lý sự kiện LostFocus cho Tỉnh TP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTinhTP_LostFocus(object sender, RoutedEventArgs e)
        {
            string text = cmbTinhTP.Text;
            AutoComboBox auto = new AutoComboBox();
            AutoCompleteEntry entry = auto.checkDisplayName(lstComboBoxTinhTP, ref cmbTinhTP);

            if (entry == null)
            {
                return;
            }
            else
            {
                maTinhTP = entry.KeywordStrings.First();
                idTinhTP = entry.KeywordStrings.ElementAt(1);
            }
        }

        /// <summary>
        /// Xử lý sự kiện SelectionChanged cho Hạch toán
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbHachToan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = cmbHachToan.Text;
            AutoComboBox auto = new AutoComboBox();
            AutoCompleteEntry entry = auto.checkDisplayName(lstComboBoxHachToan, ref cmbHachToan);

            if (entry == null)
            {
                return;
            }
            else
            {
                maHachToan = entry.KeywordStrings.First();
                idHachToan = entry.KeywordStrings.ElementAt(1);
            }
        }

        /// <summary>
        /// Xử lý sự kiện KeyDown cho Hạch toán
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbHachToan_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                string text = cmbHachToan.Text;
                AutoComboBox auto = new AutoComboBox();
                AutoCompleteEntry entry = auto.checkDisplayName(lstComboBoxHachToan, ref cmbHachToan);

                if (entry == null)
                {
                    return;
                }
                else
                {
                    maHachToan = entry.KeywordStrings.First();
                    idHachToan = entry.KeywordStrings.ElementAt(1);
                }
            }
            else
            {
                return;
            }
            cmbHachToan.IsDropDownOpen = true;
        }

        /// <summary>
        /// Xử lý sự kiện LostFocus cho Hạch toán
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbHachToan_LostFocus(object sender, RoutedEventArgs e)
        {
            string text = cmbHachToan.Text;
            AutoComboBox auto = new AutoComboBox();
            AutoCompleteEntry entry = auto.checkDisplayName(lstComboBoxHachToan, ref cmbHachToan);

            if (entry == null)
            {
                return;
            }
            else
            {
                maHachToan = entry.KeywordStrings.First();
                idHachToan = entry.KeywordStrings.ElementAt(1);
            }
        }

        /// <summary>
        /// Xử lý sự kiện SelectionChanged cho Lịch họp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbLichHop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = cmbLichHop.Text;
            AutoComboBox auto = new AutoComboBox();
            AutoCompleteEntry entry = auto.checkDisplayName(lstComboBoxLichHop, ref cmbLichHop);

            if (entry == null)
            {
                return;
            }
            else
            {
                maLichHop = entry.KeywordStrings.First();
                idLichHop = entry.KeywordStrings.ElementAt(1);
            }
        }

        /// <summary>
        /// Xử lý sự kiện KeyDown cho Lịch họp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbLichHop_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                string text = cmbLichHop.Text;
                AutoComboBox auto = new AutoComboBox();
                AutoCompleteEntry entry = auto.checkDisplayName(lstComboBoxLichHop, ref cmbLichHop);

                if (entry == null)
                {
                    return;
                }
                else
                {
                    maLichHop = entry.KeywordStrings.First();
                    idLichHop = entry.KeywordStrings.ElementAt(1);
                }
            }
            else
            {
                return;
            }
            cmbLichHop.IsDropDownOpen = true;
        }

        /// <summary>
        /// Xử lý sự kiện LostFocus cho Lịch họp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbLichHop_LostFocus(object sender, RoutedEventArgs e)
        {
            string text = cmbLichHop.Text;
            AutoComboBox auto = new AutoComboBox();
            AutoCompleteEntry entry = auto.checkDisplayName(lstComboBoxLichHop, ref cmbLichHop);

            if (entry == null)
            {
                return;
            }
            else
            {
                maLichHop = entry.KeywordStrings.First();
                idTinhTP = entry.KeywordStrings.ElementAt(1);
            }
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstComboBoxChiNhanh.Select(i => i.DisplayName).Contains(cmbChiNhanh.Text))
            {
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();

                string maChiNhanh = lstComboBoxChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                string idChiNhanh = lstComboBoxChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.ElementAt(1);
                cmbPhongGD.Items.Clear();
                lstDieuKien.Clear();
                lstComboBoxPhongGD.Clear();
                lstDieuKien.Add(maChiNhanh);
                auto.GenAutoComboBox(ref lstComboBoxPhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOMACHA.getValue(), lstDieuKien);                
            }
            else
            {
                cmbPhongGD.Items.Clear();
            }
        }
        #endregion

        #region Xu ly nghiep vu

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
                    // Sự kiện load dữ liệu
                    // Thông tin đơn vị
                    DataSet dsDonVi = danhmucProcess.getDonViTheoID(id.ToString());
                    if (dsDonVi != null && dsDonVi.Tables.Count > 0 && dsDonVi.Tables[0].Rows.Count > 0)
                    {
                        // khởi tạo combobox
                        AutoComboBox auto = new AutoComboBox();
                        // Khởi tạo điều kiện gọi danh mục
                        List<string> lstDieuKien = new List<string>();

                        cmbLoaiDonVi.Items.Clear();
                        lstComboBoxLoaiDonVi.Clear();
                        lstDieuKien.Clear();
                        lstDieuKien.Add(DatabaseConstant.DanhMuc.THUOC_TINH_DON_VI.getValue());
                        auto.GenAutoComboBox(ref lstComboBoxLoaiDonVi, ref cmbLoaiDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
                        auto.removeEntry(ref lstComboBoxLoaiDonVi, ref cmbLoaiDonVi, DatabaseConstant.ToChucDonVi.HSO.getValue());
                        auto.removeEntry(ref lstComboBoxLoaiDonVi, ref cmbLoaiDonVi, DatabaseConstant.ToChucDonVi.VPGD.getValue());
                        auto.removeEntry(ref lstComboBoxLoaiDonVi, ref cmbLoaiDonVi, DatabaseConstant.ToChucDonVi.VDGD.getValue());

                        cmbLoaiDonVi.SelectedIndex = lstComboBoxLoaiDonVi.IndexOf(lstComboBoxLoaiDonVi.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dsDonVi.Tables[0].Rows[0]["loai_dvi"].ToString())));
                        cmbDonViCha.SelectedIndex = lstComboBoxDonViCha.IndexOf(lstComboBoxDonViCha.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dsDonVi.Tables[0].Rows[0]["ma_dvi_cha"].ToString())));
                        cmbTinhTP.SelectedIndex = lstComboBoxTinhTP.IndexOf(lstComboBoxTinhTP.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dsDonVi.Tables[0].Rows[0]["ma_tinhtp"].ToString())));
                        

                        string loaiDonVi = dsDonVi.Tables[0].Rows[0]["loai_dvi"].ToString();
                        if (!LObject.IsNullOrEmpty(loaiDonVi))
                        {
                            if (loaiDonVi.Equals(DatabaseConstant.ToChucDonVi.DVI.getValue()))
                            {
                                cmbLoaiDonVi.SelectedIndex = lstComboBoxLoaiDonVi.IndexOf(lstComboBoxLoaiDonVi.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(DatabaseConstant.ToChucDonVi.DVI.getValue())));

                                chkLaHoiSoChinh.Visibility = Visibility.Collapsed;
                                chkLaVPChiNhanh.Visibility = Visibility.Collapsed;
                                chkLaVPPhongGD.Visibility = Visibility.Collapsed;

                                cmbChiNhanh.Visibility = Visibility.Collapsed;
                                cmbPhongGD.Visibility = Visibility.Collapsed;

                                cmbLichHop.Visibility = Visibility.Collapsed;
                                lblLichHop.Visibility = Visibility.Collapsed;
                                lblRequiredLichHop.Visibility = Visibility.Collapsed;
                                cmbHachToan.Visibility = Visibility.Collapsed;
                                lblHachToan.Visibility = Visibility.Collapsed;
                                lblRequiredHachToan.Visibility = Visibility.Collapsed;
                            }
                            else if (loaiDonVi.Equals(DatabaseConstant.ToChucDonVi.HSO.getValue()))
                            {
                                cmbLoaiDonVi.SelectedIndex = lstComboBoxLoaiDonVi.IndexOf(lstComboBoxLoaiDonVi.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(DatabaseConstant.ToChucDonVi.CNH.getValue())));

                                chkLaHoiSoChinh.Visibility = Visibility.Visible;
                                chkLaVPChiNhanh.Visibility = Visibility.Collapsed;
                                chkLaVPPhongGD.Visibility = Visibility.Collapsed;

                                cmbChiNhanh.Visibility = Visibility.Collapsed;
                                cmbPhongGD.Visibility = Visibility.Collapsed;

                                cmbHachToan.Visibility = Visibility.Visible;
                                cmbLichHop.Visibility = Visibility.Visible;

                                chkLaHoiSoChinh.IsChecked = true;

                                cmbLichHop.Visibility = Visibility.Visible;
                                lblLichHop.Visibility = Visibility.Visible;
                                lblRequiredLichHop.Visibility = Visibility.Visible;
                                cmbHachToan.Visibility = Visibility.Visible;
                                lblHachToan.Visibility = Visibility.Visible;
                                lblRequiredHachToan.Visibility = Visibility.Visible;
                            }
                            else if (loaiDonVi.Equals(DatabaseConstant.ToChucDonVi.CNH.getValue()))
                            {
                                cmbLoaiDonVi.SelectedIndex = lstComboBoxLoaiDonVi.IndexOf(lstComboBoxLoaiDonVi.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(DatabaseConstant.ToChucDonVi.CNH.getValue())));

                                chkLaHoiSoChinh.Visibility = Visibility.Visible;
                                chkLaVPChiNhanh.Visibility = Visibility.Collapsed;
                                chkLaVPPhongGD.Visibility = Visibility.Collapsed;

                                cmbChiNhanh.Visibility = Visibility.Collapsed;
                                cmbPhongGD.Visibility = Visibility.Collapsed;

                                cmbHachToan.Visibility = Visibility.Visible;
                                cmbLichHop.Visibility = Visibility.Visible;

                                chkLaHoiSoChinh.IsChecked = false;

                                cmbLichHop.Visibility = Visibility.Visible;
                                lblLichHop.Visibility = Visibility.Visible;
                                lblRequiredLichHop.Visibility = Visibility.Visible;
                                cmbHachToan.Visibility = Visibility.Visible;
                                lblHachToan.Visibility = Visibility.Visible;
                                lblRequiredHachToan.Visibility = Visibility.Visible;
                            }
                            else if (loaiDonVi.Equals(DatabaseConstant.ToChucDonVi.VPGD.getValue()))
                            {
                                cmbLoaiDonVi.SelectedIndex = lstComboBoxLoaiDonVi.IndexOf(lstComboBoxLoaiDonVi.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(DatabaseConstant.ToChucDonVi.PGD.getValue())));

                                chkLaHoiSoChinh.Visibility = Visibility.Collapsed;
                                chkLaVPChiNhanh.Visibility = Visibility.Visible;
                                chkLaVPPhongGD.Visibility = Visibility.Collapsed;

                                cmbChiNhanh.Visibility = Visibility.Visible;
                                cmbPhongGD.Visibility = Visibility.Collapsed;

                                cmbHachToan.Visibility = Visibility.Visible;
                                cmbLichHop.Visibility = Visibility.Visible;

                                chkLaVPChiNhanh.IsChecked = true;

                                cmbLichHop.Visibility = Visibility.Collapsed;
                                lblLichHop.Visibility = Visibility.Collapsed;
                                lblRequiredLichHop.Visibility = Visibility.Collapsed;
                                cmbHachToan.Visibility = Visibility.Visible;
                                lblHachToan.Visibility = Visibility.Visible;
                                lblRequiredHachToan.Visibility = Visibility.Visible;

                                // khởi tạo combobox
                                auto = new AutoComboBox();
                                cmbChiNhanh.Items.Clear();
                                lstComboBoxChiNhanh = new List<AutoCompleteEntry>();
                                auto.GenAutoComboBox(ref lstComboBoxChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, dsDonVi.Tables[0].Rows[0]["ma_dvi_qly"].ToString());
                            }
                            else if (loaiDonVi.Equals(DatabaseConstant.ToChucDonVi.PGD.getValue()))
                            {
                                cmbLoaiDonVi.SelectedIndex = lstComboBoxLoaiDonVi.IndexOf(lstComboBoxLoaiDonVi.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(DatabaseConstant.ToChucDonVi.PGD.getValue())));

                                chkLaHoiSoChinh.Visibility = Visibility.Collapsed;
                                chkLaVPChiNhanh.Visibility = Visibility.Visible;
                                chkLaVPPhongGD.Visibility = Visibility.Collapsed;

                                cmbChiNhanh.Visibility = Visibility.Visible;
                                cmbPhongGD.Visibility = Visibility.Collapsed;

                                cmbHachToan.Visibility = Visibility.Visible;
                                cmbLichHop.Visibility = Visibility.Visible;

                                chkLaVPChiNhanh.IsChecked = false;

                                cmbLichHop.Visibility = Visibility.Collapsed;
                                lblLichHop.Visibility = Visibility.Collapsed;
                                lblRequiredLichHop.Visibility = Visibility.Collapsed;
                                cmbHachToan.Visibility = Visibility.Visible;
                                lblHachToan.Visibility = Visibility.Visible;
                                lblRequiredHachToan.Visibility = Visibility.Visible;

                                // khởi tạo combobox
                                auto = new AutoComboBox();
                                cmbChiNhanh.Items.Clear();
                                lstComboBoxChiNhanh = new List<AutoCompleteEntry>();
                                auto.GenAutoComboBox(ref lstComboBoxChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, dsDonVi.Tables[0].Rows[0]["ma_dvi_qly"].ToString());
                            }
                            else if (loaiDonVi.Equals(DatabaseConstant.ToChucDonVi.VDGD.getValue()))
                            {
                                cmbLoaiDonVi.SelectedIndex = lstComboBoxLoaiDonVi.IndexOf(lstComboBoxLoaiDonVi.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(DatabaseConstant.ToChucDonVi.DGD.getValue())));

                                chkLaHoiSoChinh.Visibility = Visibility.Collapsed;
                                chkLaVPChiNhanh.Visibility = Visibility.Collapsed;
                                chkLaVPPhongGD.Visibility = Visibility.Collapsed;

                                cmbChiNhanh.Visibility = Visibility.Visible;
                                cmbPhongGD.Visibility = Visibility.Visible;

                                cmbHachToan.Visibility = Visibility.Collapsed;
                                cmbLichHop.Visibility = Visibility.Collapsed;

                                chkLaVPPhongGD.IsChecked = true;

                                cmbLichHop.Visibility = Visibility.Collapsed;
                                lblLichHop.Visibility = Visibility.Collapsed;
                                lblRequiredLichHop.Visibility = Visibility.Collapsed;
                                cmbHachToan.Visibility = Visibility.Collapsed;
                                lblHachToan.Visibility = Visibility.Collapsed;
                                lblRequiredHachToan.Visibility = Visibility.Collapsed;

                                // khởi tạo combobox
                                auto = new AutoComboBox();
                                cmbChiNhanh.Items.Clear();
                                lstComboBoxChiNhanh = new List<AutoCompleteEntry>();
                                auto.GenAutoComboBox(ref lstComboBoxChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, dsDonVi.Tables[0].Rows[0]["ma_dvi_qly"].ToString());

                                // khởi tạo combobox
                                auto = new AutoComboBox();
                                cmbPhongGD.Items.Clear();
                                lstComboBoxPhongGD = new List<AutoCompleteEntry>();
                                lstDieuKien = new List<string>();
                                lstDieuKien.Add(dsDonVi.Tables[0].Rows[0]["ma_dvi_qly"].ToString());
                                auto.GenAutoComboBox(ref lstComboBoxPhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOMACHA.getValue(), lstDieuKien, dsDonVi.Tables[0].Rows[0]["ma_dvi_cha"].ToString());
                            }
                            else if (loaiDonVi.Equals(DatabaseConstant.ToChucDonVi.DGD.getValue()))
                            {
                                cmbLoaiDonVi.SelectedIndex = lstComboBoxLoaiDonVi.IndexOf(lstComboBoxLoaiDonVi.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(DatabaseConstant.ToChucDonVi.DGD.getValue())));

                                chkLaHoiSoChinh.Visibility = Visibility.Collapsed;
                                chkLaVPChiNhanh.Visibility = Visibility.Collapsed;
                                chkLaVPPhongGD.Visibility = Visibility.Collapsed;

                                cmbChiNhanh.Visibility = Visibility.Visible;
                                cmbPhongGD.Visibility = Visibility.Visible;

                                cmbHachToan.Visibility = Visibility.Collapsed;
                                cmbLichHop.Visibility = Visibility.Collapsed;

                                chkLaVPPhongGD.IsChecked = false;

                                cmbLichHop.Visibility = Visibility.Collapsed;
                                lblLichHop.Visibility = Visibility.Collapsed;
                                lblRequiredLichHop.Visibility = Visibility.Collapsed;
                                cmbHachToan.Visibility = Visibility.Collapsed;
                                lblHachToan.Visibility = Visibility.Collapsed;
                                lblRequiredHachToan.Visibility = Visibility.Collapsed;

                                // khởi tạo combobox
                                auto = new AutoComboBox();
                                cmbChiNhanh.Items.Clear();
                                lstComboBoxChiNhanh = new List<AutoCompleteEntry>();
                                auto.GenAutoComboBox(ref lstComboBoxChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, dsDonVi.Tables[0].Rows[0]["ma_dvi_qly"].ToString());

                                // khởi tạo combobox
                                auto = new AutoComboBox();
                                cmbPhongGD.Items.Clear();
                                lstComboBoxPhongGD = new List<AutoCompleteEntry>();
                                lstDieuKien = new List<string>();
                                lstDieuKien.Add(dsDonVi.Tables[0].Rows[0]["ma_dvi_qly"].ToString());
                                auto.GenAutoComboBox(ref lstComboBoxPhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOMACHA.getValue(), lstDieuKien, dsDonVi.Tables[0].Rows[0]["ma_dvi_cha"].ToString());
                            }
                            else
                            {

                            }
                        }

                        cmbHachToan.SelectedIndex = lstComboBoxHachToan.IndexOf(lstComboBoxHachToan.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dsDonVi.Tables[0].Rows[0]["ma_hach_toan"].ToString())));
                        cmbLichHop.SelectedIndex = lstComboBoxLichHop.IndexOf(lstComboBoxLichHop.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dsDonVi.Tables[0].Rows[0]["ma_tsuat"].ToString())));

                        txtMaDonVi.Text = dsDonVi.Tables[0].Rows[0]["ma_dvi"].ToString();
                        txtTenGiaoDich.Text = dsDonVi.Tables[0].Rows[0]["ten_gdich"].ToString();
                        txtTentat.Text = dsDonVi.Tables[0].Rows[0]["ten_tat"].ToString();
                        txtDiaChi.Text = dsDonVi.Tables[0].Rows[0]["dia_chi"].ToString();

                        if (!string.IsNullOrEmpty(dsDonVi.Tables[0].Rows[0]["ngay_hdong"].ToString()))
                        {
                            string ngayHoatDong = dsDonVi.Tables[0].Rows[0]["ngay_hdong"].ToString();
                            dtpNgayHoatDong.SelectedDate = LDateTime.StringToDate(ngayHoatDong, "yyyyMMdd");
                            raddtNgayHoatDong.Value = LDateTime.StringToDate(ngayHoatDong, "yyyyMMdd");
                        }

                        txtDienThoai.Text = dsDonVi.Tables[0].Rows[0]["dien_thoai"].ToString();
                        txtSoFax.Text = dsDonVi.Tables[0].Rows[0]["so_fax"].ToString();
                        txtWebsite.Text = dsDonVi.Tables[0].Rows[0]["website"].ToString();
                        txtDiaChi.Text = dsDonVi.Tables[0].Rows[0]["dia_chi"].ToString();
                        imgLogo.Tag = dsDonVi.Tables[0].Rows[0]["logo"].ToString();
                        imageName = imgLogo.Tag.ToString();
                        txtEmail.Text = dsDonVi.Tables[0].Rows[0]["email"].ToString();
                        txtMaNHNN.Text = dsDonVi.Tables[0].Rows[0]["ma_nhnn"].ToString();
                        txtSoDKKD.Text = dsDonVi.Tables[0].Rows[0]["so_dkkd"].ToString();
                        txtMaSoThue.Text = dsDonVi.Tables[0].Rows[0]["ma_so_thue"].ToString();
                        txtSoTaiKhoan.Text = dsDonVi.Tables[0].Rows[0]["so_tai_khoan"].ToString();
                        txtTenNganHang.Text = dsDonVi.Tables[0].Rows[0]["ten_ngan_hang"].ToString();
                        txtTenGD.Text = dsDonVi.Tables[0].Rows[0]["gdoc_ten"].ToString();
                        txtTelGD.Text = dsDonVi.Tables[0].Rows[0]["gdoc_tel"].ToString();
                        txtPhoneGD.Text = dsDonVi.Tables[0].Rows[0]["gdoc_mobile"].ToString();
                        txtFaxGD.Text = dsDonVi.Tables[0].Rows[0]["gdoc_fax"].ToString();
                        txtEmailGD.Text = dsDonVi.Tables[0].Rows[0]["gdoc_email"].ToString();
                        txtTenKeToan.Text = dsDonVi.Tables[0].Rows[0]["kttruong_ten"].ToString();
                        txtTelKeToan.Text = dsDonVi.Tables[0].Rows[0]["kttruong_tel"].ToString();
                        txtPhoneKeToan.Text = dsDonVi.Tables[0].Rows[0]["kttruong_mobile"].ToString();
                        txtFaxKeToan.Text = dsDonVi.Tables[0].Rows[0]["kttruong_fax"].ToString();
                        txtEmailKeToan.Text = dsDonVi.Tables[0].Rows[0]["kttruong_email"].ToString();
                        //txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguNghiepVu(dsDonVi.Tables[0].Rows[0]["tthai_nvu"].ToString());
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(dsDonVi.Tables[0].Rows[0]["tthai_bghi"].ToString());
                        txtNguoiNhap.Text = dsDonVi.Tables[0].Rows[0]["nguoi_nhap"].ToString();
                        txtNguoiCNhat.Text = dsDonVi.Tables[0].Rows[0]["nguoi_cnhat"].ToString();
                        //lblTrangThaiBanGhi.Content = BusinessConstant.layNgonNguNghiepVu(dsDonVi.Tables[0].Rows[0]["tthai_nvu"].ToString());
                        lblTrangThaiBanGhi.Content = BusinessConstant.layNgonNguSuDung(dsDonVi.Tables[0].Rows[0]["tthai_bghi"].ToString());
                        TthaiNvu = dsDonVi.Tables[0].Rows[0]["tthai_nvu"].ToString();
                        if (!string.IsNullOrEmpty(dsDonVi.Tables[0].Rows[0]["ngay_nhap"].ToString()))
                        {
                            string ngayNhap = dsDonVi.Tables[0].Rows[0]["ngay_nhap"].ToString();
                            raddtNgayNhap.Value = LDateTime.StringToDate(ngayNhap, "yyyyMMdd");
                        }
                        if (!string.IsNullOrEmpty(dsDonVi.Tables[0].Rows[0]["ngay_cnhat"].ToString()))
                        {
                            string ngayCNhat = dsDonVi.Tables[0].Rows[0]["ngay_cnhat"].ToString();
                            raddtNgayCNhat.Value = LDateTime.StringToDate(ngayCNhat, "yyyyMMdd");
                        }
                        // Lấy ảnh từ server
                        if (imgLogo.Tag.ToString() != "")
                        {
                            LoadImageFormSever(imgLogo.Tag.ToString());
                        }
                    }

                    // Thông tin tài khoản
                    DataSet dsTaiKhoan = danhmucProcess.getThongTinTaiKhoanDonVi(id.ToString());
                    if (dsTaiKhoan != null && dsTaiKhoan.Tables.Count > 0 && dsTaiKhoan.Tables[0].Rows.Count > 0)
                    {
                        bankAccountList = new List<BankAccountInfo>();
                        DataTable dtTaiKhoan = dsTaiKhoan.Tables["TKHOAN"];
                        foreach (DataRow row in dtTaiKhoan.Rows)
                        {
                            BankAccountInfo account = new BankAccountInfo();
                            account.BankCode = row["NGAN_HANG"].ToString();
                            account.Branch = row["CHI_NHANH"].ToString();
                            account.AccountNo = row["SO_TAI_KHOAN"].ToString();
                            account.AccountName = row["TEN_TAI_KHOAN"].ToString();

                            bankAccountList.Add(account);
                        }
                        grdTaiKhoan.ItemsSource = bankAccountList;
                    }
                    else
                    {
                        grdTaiKhoan.ItemsSource = bankAccountList;
                    }

                    if (!ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.M7MFI.layGiaTri()))
                    {
                        lblHachToan.Visibility = Visibility.Collapsed;
                        cmbHachToan.Visibility = Visibility.Collapsed;
                        lblRequiredHachToan.Visibility = Visibility.Collapsed;

                        lblLichHop.Visibility = Visibility.Collapsed;
                        cmbLichHop.Visibility = Visibility.Collapsed;
                        lblRequiredLichHop.Visibility = Visibility.Collapsed;

                        tbiTaiKhoan.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DanhMuc.ucDonViCT.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Đưa dữ liệu từ form vào object DM_DON_VI
        /// </summary>
        private void GetFormData(ref DM_DON_VI obj)
        {
            if (id != 0)
            {
                obj.ID = id;
                obj.MA_DVI = txtMaDonVi.Text;
            }

            AutoCompleteEntry aceLoaiDonVi = new AutoComboBox().getEntryByDisplayName(lstComboBoxLoaiDonVi, ref cmbLoaiDonVi);
            string loaiDonVi = aceLoaiDonVi.KeywordStrings.ElementAt(0);
            string maChiNhanh = chkLaHoiSoChinh.IsChecked == true ? DatabaseConstant.ToChucDonVi.HSO.getValue() : DatabaseConstant.ToChucDonVi.CNH.getValue();
            string maPhongGD = chkLaVPChiNhanh.IsChecked == true ? DatabaseConstant.ToChucDonVi.VPGD.getValue() : DatabaseConstant.ToChucDonVi.PGD.getValue();
            string maDiemGD = chkLaVPPhongGD.IsChecked == true ? DatabaseConstant.ToChucDonVi.VDGD.getValue() : DatabaseConstant.ToChucDonVi.DGD.getValue();
            if (loaiDonVi.Equals(DatabaseConstant.ToChucDonVi.DVI.getValue()))
            {
                
            }
            else if (loaiDonVi.Equals(DatabaseConstant.ToChucDonVi.CNH.getValue()))
            {
                loaiDonVi = maChiNhanh;
            }
            else if (loaiDonVi.Equals(DatabaseConstant.ToChucDonVi.PGD.getValue()))
            {
                loaiDonVi = maPhongGD;
            }
            else if (loaiDonVi.Equals(DatabaseConstant.ToChucDonVi.DGD.getValue()))
            {
                loaiDonVi = maDiemGD;
            }
            obj.LOAI_DVI = loaiDonVi;

            if (cmbDonViCha.Visibility == Visibility.Visible)
            {
                AutoCompleteEntry aceCha = new AutoComboBox().getEntryByDisplayName(lstComboBoxDonViCha, ref cmbDonViCha);
                obj.MA_DVI_CHA = aceCha.KeywordStrings.ElementAt(0);
                obj.ID_DVI_CHA = Convert.ToInt32(aceCha.KeywordStrings.ElementAt(1));
            }

            string maDonViCha;
            int? idDonViCha;
            AutoCompleteEntry aceDonViCha;
            if (loaiDonVi.Equals(DatabaseConstant.ToChucDonVi.DVI.getValue()))
            {
                maDonViCha = null;
                idDonViCha = null;
            }
            else if (loaiDonVi.Equals(DatabaseConstant.ToChucDonVi.HSO.getValue()) || loaiDonVi.Equals(DatabaseConstant.ToChucDonVi.CNH.getValue()))
            {
                maDonViCha = DatabaseConstant.MA_TOCHUC;
                idDonViCha = DatabaseConstant.ID_TOCHUC;
            }
            else if (loaiDonVi.Equals(DatabaseConstant.ToChucDonVi.VPGD.getValue()) || loaiDonVi.Equals(DatabaseConstant.ToChucDonVi.PGD.getValue()))
            {
                aceDonViCha = new AutoComboBox().getEntryByDisplayName(lstComboBoxChiNhanh, ref cmbChiNhanh);
                maDonViCha = aceDonViCha.KeywordStrings.ElementAt(0);
                idDonViCha = Convert.ToInt32(aceDonViCha.KeywordStrings.ElementAt(1));
            }
            else if (loaiDonVi.Equals(DatabaseConstant.ToChucDonVi.VDGD.getValue()) || loaiDonVi.Equals(DatabaseConstant.ToChucDonVi.DGD.getValue()))
            {
                aceDonViCha = new AutoComboBox().getEntryByDisplayName(lstComboBoxPhongGD, ref cmbPhongGD);
                maDonViCha = aceDonViCha.KeywordStrings.ElementAt(0);
                idDonViCha = Convert.ToInt32(aceDonViCha.KeywordStrings.ElementAt(1));
            }
            else
            {
                maDonViCha = null;
                idDonViCha = null;
            }
            obj.MA_DVI_CHA = maDonViCha;
            obj.ID_DVI_CHA = idDonViCha;

            obj.TEN_GDICH = txtTenGiaoDich.Text.Trim();
            obj.TEN_TAT = txtTentat.Text.Trim();

            AutoCompleteEntry aceTinhTP = new AutoComboBox().getEntryByDisplayName(lstComboBoxTinhTP, ref cmbTinhTP);
            obj.MA_TINHTP = aceTinhTP.KeywordStrings.ElementAt(0);

            if (raddtNgayHoatDong.Value is DateTime)
                obj.NGAY_HDONG = ((DateTime)raddtNgayHoatDong.Value).ToString("yyyyMMdd");

            obj.DIA_CHI = txtDiaChi.Text.Trim();
            obj.MA_NHNN = txtMaNHNN.Text.Trim();
            obj.EMAIL = txtEmail.Text.Trim();
            obj.MA_SO_THUE = txtMaSoThue.Text.Trim();
            obj.SO_DKKD = txtSoDKKD.Text.Trim();
            obj.SO_TAI_KHOAN = txtSoTaiKhoan.Text.Trim();
            obj.TEN_NGAN_HANG = txtTenNganHang.Text.Trim();
            obj.DIEN_THOAI = txtDienThoai.Text.Trim();
            obj.SO_FAX = txtSoFax.Text.Trim();
            obj.WEBSITE = txtWebsite.Text.Trim();

            AutoCompleteEntry aceHachToan = new AutoComboBox().getEntryByDisplayName(lstComboBoxHachToan, ref cmbHachToan);
            obj.MA_HACH_TOAN = aceHachToan != null ? aceHachToan.KeywordStrings.ElementAt(0) : null;
            if (loaiDonVi.Equals(DatabaseConstant.ToChucDonVi.HSO.getValue()) || loaiDonVi.Equals(DatabaseConstant.ToChucDonVi.CNH.getValue()))
            {
                obj.MA_HACH_TOAN = DatabaseConstant.PHUONG_PHAP_HACH_TOAN.DOC_LAP.layGiaTri();
            }

            obj.GDOC_TEN = txtTenGD.Text.Trim();
            obj.GDOC_TEL = txtTelGD.Text.Trim();
            obj.GDOC_MOBILE = txtPhoneGD.Text.Trim();
            obj.GDOC_FAX = txtFaxGD.Text.Trim();
            obj.GDOC_EMAIL = txtEmailGD.Text.Trim();
            obj.KTTRUONG_TEN = txtTenKeToan.Text.Trim();
            obj.KTTRUONG_TEL = txtTelKeToan.Text.Trim();
            obj.KTTRUONG_MOBILE = txtPhoneKeToan.Text.Trim();
            obj.KTTRUONG_FAX = txtFaxKeToan.Text.Trim();
            obj.KTTRUONG_EMAIL = txtEmailKeToan.Text.Trim();

            obj.NGUON_TAO_DL = DatabaseConstant.NguonTaoDuLieu.NSD.layGiaTri();

            AutoCompleteEntry aceLichHop = new AutoComboBox().getEntryByDisplayName(lstComboBoxLichHop, ref cmbLichHop);
            obj.MA_TSUAT = aceLichHop != null ? aceLichHop.KeywordStrings.ElementAt(0) : null;

            obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();

            if (id == 0)
            {
                obj.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                obj.MA_DVI_QLY = ClientInformation.MaDonVi;
            }

            obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
            obj.NGAY_CNHAT = LDateTime.GetCurrentDate(ApplicationConstant.defaultDateTimeFormat);

            obj.LOGO = imageName;

            // Thông tin tài khoản
            bankAccountList = grdTaiKhoan.ItemsSource as List<BankAccountInfo>;
            foreach (BankAccountInfo item in bankAccountList)
            {
                DM_DON_VI_TKHOAN objDmDonViTKhoan = new DM_DON_VI_TKHOAN();
                objDmDonViTKhoan.TEN_TAI_KHOAN = item.AccountName;
                objDmDonViTKhoan.SO_TAI_KHOAN = item.AccountNo;
                objDmDonViTKhoan.NGAN_HANG = item.BankCode;
                objDmDonViTKhoan.CHI_NHANH = item.Branch;

                lstDmDonViTKhoan.Add(objDmDonViTKhoan);
            }

            return;
        }

        /// <summary>
        /// Kiểm tra các thông tin nhập vào trước khi lưu dữ liệu
        /// </summary>
        /// <returns></returns>
        private bool Validation()
        {
            if (string.IsNullOrEmpty(cmbLoaiDonVi.Text))
            {
                LMessage.ShowMessage("M.DanhMuc.ucDonViCT.ThieuLoaiDonVi", LMessage.MessageBoxType.Warning);
                cmbLoaiDonVi.Focus();
                return false;
            }
            else if (cmbChiNhanh.Visibility == Visibility.Visible && LString.IsNullOrEmptyOrSpace(cmbChiNhanh.Text))
            {
                LMessage.ShowMessage("M.DanhMuc.ucDonViCT.ChuaChonChiNhanh", LMessage.MessageBoxType.Warning);
                cmbChiNhanh.Focus();
                return false;
            }
            else if (cmbPhongGD.Visibility == Visibility.Visible && LString.IsNullOrEmptyOrSpace(cmbPhongGD.Text))
            {
                LMessage.ShowMessage("M.DanhMuc.ucDonViCT.ChuaChonPhongGD", LMessage.MessageBoxType.Warning);
                cmbPhongGD.Focus();
                return false;
            }
            

            //else if ((cmbDonViCha.Visibility == Visibility.Visible) && string.IsNullOrEmpty(cmbDonViCha.Text))
            //{
            //    LMessage.ShowMessage("M.DanhMuc.ucDonViCT.ThieuDonViCha", LMessage.MessageBoxType.Warning);
            //    cmbDonViCha.Focus();
            //    return false;
            //}

            else if (LString.IsNullOrEmptyOrSpace(txtTenGiaoDich.Text))
            {
                LMessage.ShowMessage("M.DanhMuc.ucDonViCT.ThieuTenDonVi", LMessage.MessageBoxType.Warning);
                txtTenGiaoDich.Focus();
                return false;
            }
            else if (LString.IsNullOrEmptyOrSpace(txtTentat.Text))
            {
                LMessage.ShowMessage("M.DanhMuc.ucDonViCT.ThieuTenTat", LMessage.MessageBoxType.Warning);
                txtTentat.Focus();
                return false;
            }
            else if (LString.IsNullOrEmptyOrSpace(cmbTinhTP.Text))
            {
                LMessage.ShowMessage("M.DanhMuc.ucDonViCT.ThieuTinhThanh", LMessage.MessageBoxType.Warning);
                cmbTinhTP.Focus();
                return false;
            }
            else if (raddtNgayHoatDong.Value == null)
            {
                LMessage.ShowMessage("M.DanhMuc.ucDonViCT.ThieuNgayHoatDong", LMessage.MessageBoxType.Warning);
                raddtNgayHoatDong.Focus();
                return false;
            }
            else if (cmbHachToan.Visibility == Visibility.Visible && LString.IsNullOrEmptyOrSpace(cmbHachToan.Text))
            {
                LMessage.ShowMessage("M.DanhMuc.ucDonViCT.ChuaChonPhuongThucHachToan", LMessage.MessageBoxType.Warning);
                cmbHachToan.Focus();
                return false;
            }
            else if (cmbLichHop.Visibility == Visibility.Visible && LString.IsNullOrEmptyOrSpace(cmbLichHop.Text))
            {
                LMessage.ShowMessage("M.DanhMuc.ucDonViCT.ChuaChonTanSuatHopCum", LMessage.MessageBoxType.Warning);
                cmbLichHop.Focus();
                return false;
            }
            else if (LString.IsNullOrEmptyOrSpace(txtTenGD.Text))
            {
                LMessage.ShowMessage("M.DanhMuc.ucDonViCT.ThieuTenGiamDoc", LMessage.MessageBoxType.Warning);
                txtTenGD.Focus();
                return false;
            }
            //else if (!LString.IsNullOrEmptyOrSpace(txtEmailGD.Text) && !txtEmailGD.Text.IsEmailAddress())
            //{
            //    LMessage.ShowMessage("M.DanhMuc.ucDonViCT.SaiEmailGiamDoc", LMessage.MessageBoxType.Warning);
            //    txtEmailGD.Focus();
            //    return false;
            //}
            //else if (!LString.IsNullOrEmptyOrSpace(txtEmailKeToan.Text) && !txtEmailKeToan.Text.IsEmailAddress())
            //{
            //    LMessage.ShowMessage("M.DanhMuc.ucDonViCT.SaiEmailKeToan", LMessage.MessageBoxType.Warning);
            //    txtEmailGD.Focus();
            //    return false;
            //}
            return true;
        }

        private bool ValidationTaiKhoan()
        {
            if (string.IsNullOrEmpty(cmbLoaiDonVi.Text))
            {
                LMessage.ShowMessage("M.DanhMuc.ucDonViCT.ThieuLoaiDonVi", LMessage.MessageBoxType.Warning);
                cmbLoaiDonVi.Focus();
                return false;
            }
            else if (LString.IsNullOrEmptyOrSpace(txtTenGD.Text))
            {
                LMessage.ShowMessage("M.DanhMuc.ucDonViCT.ThieuTenGiamDoc", LMessage.MessageBoxType.Warning);
                txtTenGD.Focus();
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
            SetEnabledRequiredControls(false);
            SetEnabledAllControls(false);
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_DON_VI);
        }

        /// <summary>
        /// Trước khi thêm mới
        /// </summary>
        private void beforeAddNew()
        {
            SetEnabledRequiredControls(false);
            SetEnabledAllControls(true);
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_DON_VI);
        }

        /// <summary>
        /// Trước khi sửa từ danh sách
        /// </summary>
        public void beforeModifyFromList()
        {
            //SetFormData();
            SetEnabledAllControls(true);
            SetEnabledRequiredControls(false);
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_DON_VI);

            SetFormData();
            // Yêu cầu lock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_DON_VI,
                DatabaseConstant.Table.DM_DON_VI,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Nếu lock thành công >> cho phép sửa
            if (retLockData)
            {
                //SetFormData();
                SetEnabledAllControls(true);
                SetEnabledRequiredControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_DON_VI);
            }
            else
            {
                LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
            }
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
                DatabaseConstant.Function.DC_DM_DON_VI,
                DatabaseConstant.Table.DM_DON_VI,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Nếu lock thành công >> cho phép sửa
            if (ret)
            {
                SetEnabledAllControls(true);
                SetEnabledRequiredControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_DON_VI);
            }
            else
            {
                LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
            }
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
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_DON_VI,
                    DatabaseConstant.Table.DM_DON_VI,
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
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_DON_VI,
                    DatabaseConstant.Table.DM_DON_VI,
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
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_DON_VI,
                    DatabaseConstant.Table.DM_DON_VI,
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
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_DON_VI,
                    DatabaseConstant.Table.DM_DON_VI,
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
            //string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            string trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
            if (Validation())
            {
                DanhMucProcess danhmucProcess = new DanhMucProcess();
                try
                {
                    // Dữ liệu truyền vào và dữ liệu trả về
                    ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    DM_DON_VI obj = new DM_DON_VI();
                    string responseMessage = null;

                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (id == 0)
                    {
                        // Lấy dữ liệu từ form
                        GetFormData(ref obj);
                        obj.TTHAI_NVU = trangThai;

                        // Set các thông tin khác                        
                        obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                        obj.NGAY_NHAP = LDateTime.GetCurrentDate(ApplicationConstant.defaultDateTimeFormat);
                        ret = danhmucProcess.ThemDonVi(ref obj, ref lstDmDonViTKhoan, imageData, imageName, ref responseMessage);

                        afterAddNew(ret, obj, responseMessage);
                    }
                    // Nếu là lưu tạm khi sửa
                    // Hoặc lưu tạm khi sửa sau duyệt
                    // Hoặc sửa
                    else 
                    {
                        // Lấy thông tin cũ
                        obj = danhmucProcess.getDonViById(id);

                        // Lấy dữ liệu từ form
                        GetFormData(ref obj);
                        obj.TTHAI_NVU = trangThai;

                        ret = danhmucProcess.SuaDonVi(ref obj, ref lstDmDonViTKhoan, imageData, imageName, ref responseMessage);
                        afterModify(ret, obj, responseMessage);
                    }
                }
                catch (System.Exception ex)
                {
                    // Yêu cầu unlock dữ liệu khi xảy ra lỗi
                    // (chỉ trường hợp sửa dữ liệu)
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);

                    bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_DM_DON_VI,
                        DatabaseConstant.Table.DM_DON_VI,
                        DatabaseConstant.Action.XOA,
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
                    ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    DM_DON_VI obj = new DM_DON_VI();
                    string responseMessage = null;

                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (id == 0)
                    {
                        // Lấy dữ liệu từ form
                        GetFormData(ref obj);
                        obj.TTHAI_NVU = trangThai;

                        // Set các thông tin khác                        
                        obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                        obj.NGAY_NHAP = LDateTime.GetCurrentDate(ApplicationConstant.defaultDateTimeFormat);
                        ret = danhmucProcess.ThemDonVi(ref obj, imageData, imageName, ref responseMessage);

                        afterAddNew(ret, obj, responseMessage);
                    }
                    // Nếu là lưu tạm khi sửa
                    // Hoặc lưu tạm khi sửa sau duyệt
                    // Hoặc sửa
                    else
                    {
                        // Lấy thông tin cũ
                        obj = danhmucProcess.getDonViById(id);

                        // Lấy dữ liệu từ form
                        GetFormData(ref obj);
                        obj.TTHAI_NVU = trangThai;

                        ret = danhmucProcess.SuaDonVi(ref obj, imageData, imageName, ref responseMessage);
                        afterModify(ret, obj, responseMessage);
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
                bool ret = danhmucProcess.XoaDonVi(arrayID, ref listClientResponseDetail);

                afterDelete(ret, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu khi xảy ra lỗi
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_DON_VI,
                    DatabaseConstant.Table.DM_DON_VI,
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
                bool ret = danhmucProcess.DuyetDonVi(arrayID, ref listClientResponseDetail);

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
                bool ret = danhmucProcess.ThoaiDuyetDonVi(arrayID, ref listClientResponseDetail);

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
        private void afterAddNew(ApplicationConstant.ResponseStatus ret, DM_DON_VI obj, string responseMessage)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                if (cbMultiAdd.IsChecked == true)
                {
                    ResetForm();
                    SetEnabledRequiredControls(false);
                    SetEnabledAllControls(true);
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_DON_VI);
                }
                else if (!DatabaseConstant.CLOSE_DETAIL_FORM)
                {
                    id = obj.ID;
                    TthaiNvu = obj.TTHAI_NVU;
                    txtMaDonVi.Text = obj.MA_DVI;
                    lblTrangThaiBanGhi.Content = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);

                    SetEnabledAllControls(false);
                    SetEnabledRequiredControls(false);
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_DON_VI);

                    //tbiThongTinChung.Focus();
                    cmbChiNhanh.Focus();
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
        private void afterModify(ApplicationConstant.ResponseStatus ret, DM_DON_VI obj, string responseMessage)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                id = obj.ID;
                TthaiNvu = obj.TTHAI_NVU;
                txtMaDonVi.Text = obj.MA_DVI;
                lblTrangThaiBanGhi.Content = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);

                SetEnabledAllControls(false);
                SetEnabledRequiredControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_DON_VI);

                //tbiThongTinChung.Focus();
                cmbChiNhanh.Focus();
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
                DatabaseConstant.Function.DC_DM_DON_VI,
                DatabaseConstant.Table.DM_DON_VI,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Sau khi xóa
        /// </summary>
        /// <param name="ret"></param>
        private void afterDelete(bool ret,
            List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);

                //SetEnabledAllControls(false);
                //SetEnabledRequiredControls(false);
                //CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_DON_VI);
            }
            else
            {
                //LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
            }
            
            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);
            List<int> listUnlockId = new List<int>();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_DON_VI,
                DatabaseConstant.Table.DM_DON_VI,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Đóng cửa sổ chi tiết sau khi xóa
            onClose();
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
                lblTrangThaiBanGhi.Content = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);
                txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);

                SetEnabledAllControls(false);
                SetEnabledRequiredControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_DON_VI);
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
                DatabaseConstant.Function.DC_DM_DON_VI,
                DatabaseConstant.Table.DM_DON_VI,
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
                lblTrangThaiBanGhi.Content = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);
                txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);

                SetEnabledAllControls(false);
                SetEnabledRequiredControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_DON_VI);
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
                DatabaseConstant.Function.DC_DM_DON_VI,
                DatabaseConstant.Table.DM_DON_VI,
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
                lblTrangThaiBanGhi.Content = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);
                txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);

                SetEnabledAllControls(false);
                SetEnabledRequiredControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_DON_VI);
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
                DatabaseConstant.Function.DC_DM_DON_VI,
                DatabaseConstant.Table.DM_DON_VI,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                listLockId);            
        }        
        
        /// <summary>
        /// Xử lý sự kiện double click vào image để chọn ảnh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgLogo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                try
                {
                    dlg.FileName = "Document";
                    dlg.DefaultExt = ".jpg";
                    dlg.Filter = "Image (.jpg)|*.jpg";

                    // Show open file dialog box
                    Nullable<bool> result = dlg.ShowDialog();

                    if (result == true)
                    {
                        // Lưu đường dẫn
                        imgLogo.Tag = dlg.FileName;
                        // Hiển thị logo
                        LoadImage(dlg.FileName);
                    }
                }
                catch (Exception ex)
                {
                    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                }
            }
        }

        /// <summary>
        /// Xử lý load image theo đường dẫn truyền vào
        /// </summary>
        private void LoadImage(string path)
        {
            // Tạo image source
            BitmapImage myBitmapImage = new BitmapImage();

            // Set image vào image box
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(path);
            myBitmapImage.DecodePixelWidth = (int)brdLogo.ActualWidth;
            myBitmapImage.DecodePixelHeight = (int)brdLogo.ActualHeight;
            myBitmapImage.EndInit();
            imgLogo.Source = myBitmapImage;

            imageData = LImage.GetByteArrayFromImage(path);
            // Lấy tên ảnh
            string[] str = @path.Split('\\');
            imageName = str[str.Length - 1]; ;
        }

        /// <summary>
        /// Lấy ảnh từ server
        /// </summary>
        /// <param name="imageName"></param>
        private void LoadImageFormSever(string imageName)
        {
            Presentation.Process.DanhMucProcess process = new Presentation.Process.DanhMucProcess();
            byte[] source = process.LayAnhTuSever(DatabaseConstant.Table.DM_DON_VI.getValue() + "\\" + imageName);
            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage = LImage.LoadImageFromByteArray(source);
            if (myBitmapImage != null)
            {
                imgLogo.Source = myBitmapImage;
                imgLogo.Tag = "";

                imageData = null;
                this.imageName = "";
            }
        }

        /// <summary>
        /// Xóa ảnh về ảnh mặc định
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mniXoaAnh_Click(object sender, RoutedEventArgs e)
        {
            ResetImage();
        }

        /// <summary>
        /// Reset image về image mặc định
        /// </summary>
        private void ResetImage()
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri("pack://application:,,,/Utilities.Common;component/Images/Other/picture.png");
            logo.EndInit();
            imgLogo.Source = logo;
            imgLogo.Tag = "ResetImage";

            imageData = null;
            imageName = null;
        }

        #endregion

        private void ucNganHang_EditCellEnd(object sender, EventArgs e)
        {
            BankAccountInfo obj = ucNganHang.cellEdit.ParentRow.Item as BankAccountInfo;
            bankAccountList[bankAccountList.IndexOf(obj)].BankCode = ucNganHang.GiaTri;
        }
    }

    public class BankAccountInfo
    {
        public string BankCode { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string Branch { get; set; }
    }
}
