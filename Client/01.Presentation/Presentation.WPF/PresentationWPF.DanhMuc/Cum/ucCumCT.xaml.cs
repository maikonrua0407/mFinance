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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.DanhMucServiceRef;
using PresentationWPF.CustomControl;
using Telerik.Windows.Controls;

namespace PresentationWPF.DanhMuc.Cum
{
    /// <summary>
    /// Interaction logic for ucCumCT.xaml
    /// </summary>
    public partial class ucCumCT : UserControl
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

        Presentation.Process.DanhMucServiceRef.DC_TSUAT tanSuat;

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

        private int idCumTruong;
        public int IdCumTruong
        {
            get { return idCumTruong; }
            set { idCumTruong = value; }
        }

        private string maCumTruong;
        public string MaCumTruong
        {
            get { return maCumTruong; }
            set { maCumTruong = value; }
        }

        private string maCum;
        public string MaCum
        {
            get { return maCum; }
            set { maCum = value; }
        }

        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private bool isLoaded = false;

        List<DataRow> lstChiTiet;

        public List<DataRow> LstChiTiet
        {
            get { return lstChiTiet; }
            set { lstChiTiet = value; }
        }

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        DM_CUM obj = new DM_CUM();
        DC_TSUAT_CUM objTanSuatCum = new DC_TSUAT_CUM();

        private DatabaseConstant.Module module = DatabaseConstant.Module.DMDC;
        private DatabaseConstant.Function function = DatabaseConstant.Function.DC_DM_CUM;
        private DatabaseConstant.Table table = DatabaseConstant.Table.DM_CUM;

        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKhuVuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCanBoQL = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDviTGianHop = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> Items = new List<AutoCompleteEntry>();

        DataTable dtLichHopCum = new DataTable();

        #endregion

        #region Khoi tao

        public ucCumCT()
        {
            InitializeComponent();

            InitEventHandler();

            BindShortkey();

            LoadCombobox();

            ResetForm();

            // Refresh buttons
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_CUM);
        }

        /// <summary>
        /// Khởi tạo các combobox
        /// </summary>
        private void LoadCombobox()
        {
            AutoComboBox auto = new AutoComboBox();

            List<string> lstDieuKien = new List<string>();
            
            cmbChiNhanh.Items.Clear();
            lstSourceChiNhanh.Clear();
            lstDieuKien.Add("'" + DatabaseConstant.ToChucDonVi.HSO.getValue() + "', '" + DatabaseConstant.ToChucDonVi.CNH.getValue() + "'");
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOLOAI.getValue(), lstDieuKien, ClientInformation.MaDonVi);

            if (lstSourceChiNhanh.Select(i => i.DisplayName).Contains(cmbChiNhanh.Text))
            {
                cmbPhongGD.Items.Clear();
                lstDieuKien.Clear();
                lstSourcePhongGD.Clear();
                lstDieuKien.Add(ClientInformation.MaDonVi);
                auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOMACHA.getValue(), lstDieuKien,ClientInformation.MaDonViGiaoDich);

                cmbKhuVuc.Items.Clear();
                lstDieuKien.Clear();
                lstSourceKhuVuc.Clear();
                lstDieuKien.Add(ClientInformation.IdDonVi.ToString());
                auto.GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUC.getValue(), lstDieuKien);
                cmbCanBoQLy.Items.Clear();
                lstDieuKien.Clear();
                lstSourceCanBoQL.Clear();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                auto.GenAutoComboBox(ref lstSourceCanBoQL, ref cmbCanBoQLy, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NSD.getValue(), lstDieuKien);

                if (lstSourcePhongGD.Select(i => i.DisplayName).Contains(cmbPhongGD.Text))
                {
                    string maPhongGD = lstSourcePhongGD.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
                    string idPhongGd = lstSourcePhongGD.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.ElementAt(1);
                    cmbKhuVuc.Items.Clear();
                    lstDieuKien.Clear();
                    lstSourceKhuVuc.Clear();
                    lstDieuKien.Add(idPhongGd);
                    auto.GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUC.getValue(), lstDieuKien);
                }
                else
                {
                    cmbKhuVuc.Items.Clear();
                    lstSourceKhuVuc.Clear();
                }                
            }
            else
            {
                cmbPhongGD.Items.Clear();
                lstSourcePhongGD.Clear();
                cmbKhuVuc.Items.Clear();
                lstSourceKhuVuc.Clear();
            } 
            
            cmbDviTGianHop.Items.Clear();
            lstDieuKien.Clear();
            lstSourceDviTGianHop.Clear();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.LHOP_DVI_TINH_TOAN.getValue());
            auto.GenAutoComboBox(ref lstSourceDviTGianHop, ref cmbDviTGianHop, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
            if (lstSourceKhuVuc.Count < 2)
                cmbKhuVuc.IsEnabled = false;
        }

        /// <summary>
        /// InitEventHandler
        /// </summary>
        private void InitEventHandler()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.DanhMuc;component/Cum/ucCumCT.xaml", ref Toolbar, ref mnuMain);

            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }

            cmbChiNhanh.SelectionChanged += new SelectionChangedEventHandler(cmbChiNhanh_SelectionChanged);
            cmbPhongGD.SelectionChanged += new SelectionChangedEventHandler(cmbPhongGD_SelectionChanged);
            cmbChiNhanh.KeyDown +=new KeyEventHandler(cmbChiNhanh_KeyDown);
            cmbPhongGD.KeyDown += new KeyEventHandler(cmbPhongGD_KeyDown);
            cmbDviTGianHop.SelectionChanged += cmbDviTGianHop_SelectionChanged;
        }

        /// <summary>
        /// Đưa form về trạng thái mặc định
        /// </summary>
        private void ResetForm()
        {
            id = 0;
            tthaiNvu = "";

            lblTrangThaiBanGhi.Content = "";

            txtMaCum.Text = "";
            txtTenCum.Text = "";
            txtTenTat.Text = "";
            raddtNgayTLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            raddtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiNhap.Text = ClientInformation.TenDangNhap;
            raddtNgayCapNhat.Value = null;
            txtNguoiCapNhat.Text = "";
            txtTrangThaiBanGhi.Text = "";
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
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
            onSave();
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
            //tlbModify.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            beforeModifyFromDetail();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //tlbDelete.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            beforeDelete();
        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //tlbApprove.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            onApprove();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //tlbRefuse.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            onRefuse();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //tlbCancel.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
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
            //CustomControl.CommonFunction.CloseUserControl(this);
            onClose();
        }

        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                onSave();
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                onRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                onCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                //CustomControl.CommonFunction.CloseUserControl(this);
                onClose();
            }
        }

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                onSave();
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                onRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                onCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                //CustomControl.CommonFunction.CloseUserControl(this);
                onClose();
            }
        }

        #endregion 

        #region Xu ly Giao dien

        /// <summary>
        /// SetEnabledAllControls
        /// </summary>
        /// <param name="enable"></param>
        private void SetEnabledAllControls(bool enable)
        {
            //cmbChiNhanh.IsEnabled = enable;
            //cmbPhongGD.IsEnabled = enable;
            if (lstSourceKhuVuc.Count < 2)
                cmbKhuVuc.IsEnabled = false;
            else
                cmbKhuVuc.IsEnabled = enable;

            txtMaCum.IsEnabled = enable;
            txtTenCum.IsEnabled = enable;
            txtTenTat.IsEnabled = enable;
            raddtNgayTLap.IsEnabled = enable;
            dtpNgayTLap.IsEnabled = enable;
            cmbCanBoQLy.IsEnabled = enable;
            txtCumTruong.IsEnabled = enable;
            btnPopupKhachHang.IsEnabled = enable;


            txtTrangThaiBanGhi.IsEnabled = enable;
            txtNguoiNhap.IsEnabled = enable;
            raddtNgayNhap.IsEnabled = enable;
            txtNguoiCapNhat.IsEnabled = enable;
            raddtNgayCapNhat.IsEnabled = enable;
        }

        /// <summary>
        /// SetEnabledRequiredControls
        /// </summary>
        /// <param name="enable"></param>
        private void SetEnabledRequiredControls(bool enable)
        {
            //cmbChiNhanh.IsEnabled = enable;
            //cmbPhongGD.IsEnabled = enable;
            //if (lstSourceKhuVuc.Count < 2)
            //    cmbKhuVuc.IsEnabled = false;
            //else
            //    cmbKhuVuc.IsEnabled = enable;

            txtMaCum.IsEnabled = enable;

            txtTrangThaiBanGhi.IsEnabled = enable;
            txtNguoiNhap.IsEnabled = enable;
            raddtNgayNhap.IsEnabled = enable;
            txtNguoiCapNhat.IsEnabled = enable;
            raddtNgayCapNhat.IsEnabled = enable;

            // Người dùng đơn vị chỉ được quản lý dữ liệu của đơn vị mình
            //if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()) ||
            //    ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()))
            //{
            //    cmbChiNhanh.IsEnabled = false;
            //}
        }

        /// <summary>
        /// Sự kiện ấn key trên form
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

            bool ret = process.UnlockData(module, function, table,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
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

        private void tabCumCT_SelectionChanged(object sender, Telerik.Windows.Controls.RadSelectionChangedEventArgs e)
        {
            if (tabCumCT.SelectedIndex == 0)
            {
                if (txtMaCum != null)
                {
                    UpdateLayout();
                    txtMaCum.Focus();
                }
            }
            else if (tabCumCT.SelectedIndex == 1)
            {
                if (txtTrangThaiBanGhi != null)
                {
                    UpdateLayout();
                    txtTrangThaiBanGhi.Focus();
                }
            }
        }

        /// <summary>
        /// Sự kiện khi thay đổi giá trị của combobox cmbDonVi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstSourceChiNhanh.Select(i => i.DisplayName).Contains(cmbChiNhanh.Text))
            {
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();

                string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                string idChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.ElementAt(1);
                cmbPhongGD.Items.Clear();
                lstDieuKien.Clear();
                lstSourcePhongGD.Clear();
                lstDieuKien.Add(maChiNhanh);
                auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOMACHA.getValue(), lstDieuKien);

                cmbKhuVuc.Items.Clear();
                lstDieuKien.Clear();
                lstSourceKhuVuc.Clear();
                lstDieuKien.Add(idChiNhanh.ToString());
                auto.GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUC.getValue(), lstDieuKien);

                cmbCanBoQLy.Items.Clear();
                lstDieuKien.Clear();
                lstSourceCanBoQL.Clear();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                auto.GenAutoComboBox(ref lstSourceCanBoQL, ref cmbCanBoQLy, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NSD.getValue(), lstDieuKien);

                if (lstSourcePhongGD.Select(i => i.DisplayName).Contains(cmbPhongGD.Text))
                {
                    string maPhongGD = lstSourcePhongGD.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
                    string idPhongGD = lstSourcePhongGD.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.ElementAt(1);
                    cmbKhuVuc.Items.Clear();
                    lstDieuKien.Clear();
                    lstSourceKhuVuc.Clear();
                    lstDieuKien.Add(idPhongGD);
                    auto.GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUC.getValue(), lstDieuKien);
                }
                else
                {
                    cmbKhuVuc.Items.Clear();
                    lstSourceKhuVuc.Clear();
                } 
                SetTanSuat();                
            }
            else
            {
                cmbPhongGD.Items.Clear();
                lstSourcePhongGD.Clear();

                cmbKhuVuc.Items.Clear();
                lstSourceKhuVuc.Clear();
            }
        }

        // <summary>
        /// Sự kiện khi thay đổi giá trị của combobox cmbPhongGD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstSourcePhongGD.Select(i => i.DisplayName).Contains(cmbPhongGD.Text))
            {
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();

                string maPhongGD = lstSourcePhongGD.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
                string idPhongGD = lstSourcePhongGD.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.ElementAt(1);
                cmbKhuVuc.Items.Clear();
                lstDieuKien.Clear();
                lstSourceKhuVuc.Clear();
                lstDieuKien.Add(idPhongGD);
                auto.GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUC.getValue(), lstDieuKien);
            }
            else
            {
                cmbKhuVuc.Items.Clear();
                lstSourceKhuVuc.Clear();
            }
        }

        /// <summary>
        /// Sự kiện keydown của ComboBox Đơn vị
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbChiNhanh_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                if (lstSourceChiNhanh.Select(i => i.DisplayName).Contains(cmbChiNhanh.Text))
                {
                    AutoComboBox auto = new AutoComboBox();
                    List<string> lstDieuKien = new List<string>();

                    string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                    string idChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.ElementAt(1);
                    cmbPhongGD.Items.Clear();
                    lstDieuKien.Clear();
                    lstSourcePhongGD.Clear();
                    lstDieuKien.Add(maChiNhanh);
                    auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOMACHA.getValue(), lstDieuKien);


                    cmbKhuVuc.Items.Clear();
                    lstDieuKien.Clear();
                    lstSourceKhuVuc.Clear();
                    lstDieuKien.Add(idChiNhanh.ToString());
                    auto.GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUC.getValue(), lstDieuKien);

                    cmbCanBoQLy.Items.Clear();
                    lstDieuKien.Clear();
                    lstSourceCanBoQL.Clear();
                    lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                    auto.GenAutoComboBox(ref lstSourceCanBoQL, ref cmbCanBoQLy, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NSD.getValue(), lstDieuKien);

                    if (lstSourcePhongGD.Select(i => i.DisplayName).Contains(cmbPhongGD.Text))
                    {
                        string maPhongGD = lstSourcePhongGD.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
                        string idPhongGD = lstSourcePhongGD.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.ElementAt(1);
                        cmbKhuVuc.Items.Clear();
                        lstDieuKien.Clear();
                        lstSourceKhuVuc.Clear();
                        lstDieuKien.Add(idPhongGD);
                        auto.GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUC.getValue(), lstDieuKien);
                    }
                    else
                    {
                        cmbKhuVuc.Items.Clear();
                        lstSourceKhuVuc.Clear();
                    }
                    SetTanSuat();
                }
                else
                {
                    cmbPhongGD.Items.Clear();
                    lstSourcePhongGD.Clear();

                    cmbKhuVuc.Items.Clear();
                    lstSourceKhuVuc.Clear();
                }
            }
        }

        /// <summary>
        /// Sự kiện KeyDown của ComboBox cmbPhongGD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbPhongGD_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                if (lstSourcePhongGD.Select(i => i.DisplayName).Contains(cmbPhongGD.Text))
                {
                    AutoComboBox auto = new AutoComboBox();
                    List<string> lstDieuKien = new List<string>();

                    string maPhongGD = lstSourcePhongGD.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
                    string idPhongGD = lstSourcePhongGD.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.ElementAt(1);
                    cmbKhuVuc.Items.Clear();
                    lstDieuKien.Clear();
                    lstSourceKhuVuc.Clear();
                    lstDieuKien.Add(idPhongGD);
                    auto.GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUC.getValue(), lstDieuKien);
                }
                else
                {
                    cmbKhuVuc.Items.Clear();
                    lstSourceKhuVuc.Clear();
                }
            }
        }        

        private void btnPopupKhachHang_Click(object sender, RoutedEventArgs e)
        {
            ShowPopupKhachHang();
        }

        private void ShowPopupKhachHang()
        {
            if (Convert.ToInt32(lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.ElementAt(1)) > 0)
            {                
                if (maCum != null)
                {
                    List<string> lstDieuKien = new List<string>();
                    lstDieuKien.Add(lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.ElementAt(0));
                    lstDieuKien.Add(maCum);
                    var process = new PopupProcess();
                    process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_KHACHHANG.getValue(), lstDieuKien);

                    SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                    ucPopup popup = new ucPopup(true, simplePopupResponse, true);
                    popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                    Window win = new Window();
                    win.Content = popup;
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    win.ShowDialog();
                    if (lstPopup != null && lstPopup.Count > 0)
                    {
                        DataRow row = lstPopup[0];
                        IdCumTruong = Convert.ToInt32(row[1].ToString());
                        MaCumTruong = row[2].ToString();
                        txtCumTruong.Text = row[3].ToString();
                    }
                }
                else
                {
                    LMessage.ShowMessage("Chưa có thông tin khách hàng thành viên", LMessage.MessageBoxType.Warning);
                }
                
            }
            else
                LMessage.ShowMessage("Chưa chọn đơn vị", LMessage.MessageBoxType.Warning);
        }

        private void txtCumTruong_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                ShowPopupKhachHang();
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
                    //Sự kiện load dữ liệu
                    //DM_CUM obj = null;
                    ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    string responseMessage = null;

                    ret = danhmucProcess.getCumById(id, ref obj, ref responseMessage);
                    if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
                    {
                        TthaiNvu = obj.TTHAI_NVU;
                        maCum = obj.MA_CUM;

                        lblTrangThaiBanGhi.Content = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);

                        // khởi tạo combobox
                        AutoComboBox auto = new AutoComboBox();
                        // Khởi tạo điều kiện gọi danh mục
                        List<string> lstDieuKien = new List<string>();
                        // khởi tạo combobox
                        auto = new AutoComboBox();
                        lstSourceChiNhanh.Clear();
                        cmbChiNhanh.Items.Clear();
                        lstDieuKien = new List<string>();
                        lstDieuKien.Add("'" + DatabaseConstant.ToChucDonVi.HSO.getValue() + "', '" + DatabaseConstant.ToChucDonVi.CNH.getValue() + "'");
                        auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOLOAI.getValue(), lstDieuKien, obj.MA_DVI_QLY);

                        // khởi tạo combobox
                        auto = new AutoComboBox();
                        lstSourcePhongGD.Clear();
                        cmbPhongGD.Items.Clear();
                        lstDieuKien = new List<string>();
                        lstDieuKien.Add(obj.MA_DVI_QLY);
                        auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOMACHA.getValue(), lstDieuKien, obj.MA_DVI);

                        // khởi tạo combobox
                        auto = new AutoComboBox();
                        lstDieuKien = new List<string>();
                        lstDieuKien.Add(obj.ID_DVI.ToString());
                        cmbKhuVuc.Items.Clear();
                        lstSourceKhuVuc.Clear();
                        auto.GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUC.getValue(), lstDieuKien, obj.MA_KVUC);

                        // khởi tạo combobox
                        auto = new AutoComboBox();
                        lstDieuKien = new List<string>();
                        lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                        auto.GenAutoComboBox(ref lstSourceCanBoQL, ref cmbCanBoQLy, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NSD.getValue(), lstDieuKien, obj.MA_CBO_QLY);

                        txtTenCum.Text = obj.TEN_CUM;
                        txtMaCum.Text = obj.MA_CUM;
                        txtTenTat.Text = obj.TEN_TAT;

                        if (!string.IsNullOrEmpty(obj.NGAY_TLAP))
                        {
                            raddtNgayTLap.Value = LDateTime.StringToDate(obj.NGAY_TLAP, "yyyyMMdd");
                        }

                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                        txtNguoiNhap.Text = obj.NGUOI_NHAP;
                        if (!string.IsNullOrEmpty(obj.NGAY_NHAP))
                        {
                            raddtNgayNhap.Value = LDateTime.StringToDate(obj.NGAY_NHAP, "yyyyMMdd");
                        }
                        txtNguoiCapNhat.Text = obj.NGUOI_CNHAT;
                        if (!string.IsNullOrEmpty(obj.NGAY_CNHAT))
                        {
                            raddtNgayCapNhat.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, "yyyyMMdd");
                        }

                        Presentation.Process.DanhMucServiceRef.DC_TSUAT_CUM tanSuatCum = danhmucProcess.GetTanSuatCumbyIdCum(obj.ID);

                        if (!tanSuatCum.IsNullOrEmpty())
                        {
                            // khởi tạo combobox
                            auto = new AutoComboBox();
                            lstDieuKien = new List<string>();
                            lstDieuKien.Add(DatabaseConstant.DanhMuc.LHOP_DVI_TINH_TOAN.getValue());
                            cmbDviTGianHop.Items.Clear();
                            lstSourceDviTGianHop.Clear();
                            auto.GenAutoComboBox(ref lstSourceDviTGianHop, ref cmbDviTGianHop, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, tanSuatCum.LHOP_DVI_TINH_TOAN);
                            for (int i = 0; i < dtLichHopCum.Rows.Count; i++)
                            {
                                //THU,NGAY,TUAN,THANG
                                if (lstSourceDviTGianHop.ElementAt(cmbDviTGianHop.SelectedIndex).KeywordStrings.First().Equals("NGAY"))
                                {
                                    string itemGIA_TRI_DDANG = tanSuatCum.GIA_TRI_DDANG.SplitByDelimiter("#")[i];
                                    dtLichHopCum.Rows[i]["STT"] = i + 1;
                                    dtLichHopCum.Rows[i]["THU"] = DBNull.Value;
                                    dtLichHopCum.Rows[i]["NGAY"] = itemGIA_TRI_DDANG.SplitByDelimiter(".")[0];
                                    dtLichHopCum.Rows[i]["TUAN"] = DBNull.Value;
                                    dtLichHopCum.Rows[i]["THANG"] = itemGIA_TRI_DDANG.SplitByDelimiter(".")[1];
                                }
                                else if (lstSourceDviTGianHop.ElementAt(cmbDviTGianHop.SelectedIndex).KeywordStrings.First().Equals("THU"))
                                {
                                    string itemGIA_TRI_DDANG = tanSuatCum.GIA_TRI_DDANG.SplitByDelimiter("#")[i];
                                    dtLichHopCum.Rows[i]["STT"] = i + 1;
                                    dtLichHopCum.Rows[i]["THU"] = itemGIA_TRI_DDANG.SplitByDelimiter(".")[0];
                                    dtLichHopCum.Rows[i]["NGAY"] = DBNull.Value;
                                    dtLichHopCum.Rows[i]["TUAN"] = itemGIA_TRI_DDANG.SplitByDelimiter(".")[1];
                                    dtLichHopCum.Rows[i]["THANG"] = DBNull.Value;
                                }
                            }
                            raddgrLichHopThu.ItemsSource = null;
                            raddgrLichHopThu.ItemsSource = dtLichHopCum.DefaultView;
                            if (lstSourceDviTGianHop.ElementAt(cmbDviTGianHop.SelectedIndex).KeywordStrings.First().Equals("NGAY"))
                            {
                                raddgrLichHopThu.Columns[1].IsVisible = false;
                                raddgrLichHopThu.Columns[2].IsVisible = true;
                                raddgrLichHopThu.Columns[3].IsVisible = false;
                                raddgrLichHopThu.Columns[4].IsVisible = true;
                            }
                            else if (lstSourceDviTGianHop.ElementAt(cmbDviTGianHop.SelectedIndex).KeywordStrings.First().Equals("THU"))
                            {
                                raddgrLichHopThu.Columns[1].IsVisible = true;
                                raddgrLichHopThu.Columns[2].IsVisible = false;
                                raddgrLichHopThu.Columns[3].IsVisible = true;
                                raddgrLichHopThu.Columns[4].IsVisible = false;
                            }

                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DanhMuc.ucCumCT.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void SetTanSuat()
        {
            dtLichHopCum = new DataTable();
            dtLichHopCum.Columns.Add("STT", typeof(int));
            dtLichHopCum.Columns.Add("THU", typeof(string));
            dtLichHopCum.Columns.Add("NGAY", typeof(int));
            dtLichHopCum.Columns.Add("TUAN", typeof(int));
            dtLichHopCum.Columns.Add("THANG", typeof(int));
            DanhMucProcess danhmucProcess = new DanhMucProcess();
            tanSuat = danhmucProcess.GetTanSuatbyMaDonVi(lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First());
            if (tanSuat != null)
            {
                lblTenTanSuat.Content = tanSuat.TEN_TSUAT;
                string dvTG = string.Empty;
                if(cmbDviTGianHop.SelectedIndex>=0)
                    dvTG = lstSourceDviTGianHop.ElementAt(cmbDviTGianHop.SelectedIndex).KeywordStrings.First();
                if (dvTG.IsNullOrEmptyOrSpace())
                    dvTG = tanSuat.LHOP_DVI_TINH_TOAN;
                for (int i = 0; i < tanSuat.TSUAT_LAN; i++)
                {
                    //THU,NGAY,TUAN,THANG
                    if (dvTG.Equals("NGAY"))
                    {
                        dtLichHopCum.NewRow();
                        dtLichHopCum.Rows.Add(i + 1, "", 0, 0, 0);
                    }
                    else if (dvTG.Equals("THU"))
                    {
                        dtLichHopCum.NewRow();
                        dtLichHopCum.Rows.Add(i + 1, "MON", 0, 0, 0);
                    }
                }
                raddgrLichHopThu.ItemsSource = dtLichHopCum.DefaultView;
                if (dvTG.Equals("NGAY"))
                {
                    raddgrLichHopThu.Columns[1].IsVisible = false;
                    raddgrLichHopThu.Columns[2].IsVisible = true;
                    raddgrLichHopThu.Columns[3].IsVisible = false;
                    raddgrLichHopThu.Columns[4].IsVisible = true;
                }
                else if (dvTG.Equals("THU"))
                {
                    raddgrLichHopThu.Columns[1].IsVisible = true;
                    raddgrLichHopThu.Columns[2].IsVisible = false;
                    raddgrLichHopThu.Columns[3].IsVisible = true;
                    raddgrLichHopThu.Columns[4].IsVisible = false;
                }
            }
            else
            {

            }
        }

        void uccmbThuTrongTuan_EditCellEnd(object sender, EventArgs e)
        {
            Telerik.Windows.Controls.GridView.GridViewCell cellEdit = uccmbThuTrongTuan.cellEdit;
            DataRowView drv = (DataRowView)cellEdit.ParentRow.Item;
            int indexofRows = dtLichHopCum.Rows.IndexOf(drv.Row);
            string NameColumn = uccmbThuTrongTuan.cellEdit.Column.UniqueName;
            string GiaTri = uccmbThuTrongTuan.GiaTri;
            dtLichHopCum.Rows[indexofRows][NameColumn] = GiaTri;
            //LoadDataGridView();
        }

        /// <summary>
        /// Đưa dữ liệu từ form vào object DM_DON_VI
        /// </summary>
        private void GetFormData(ref DM_CUM obj, ref DC_TSUAT_CUM objTanSuatCum)
        {
            if (id != 0)
            {
                obj.ID = id;
                obj.MA_CUM = txtMaCum.Text;
                obj.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
            }
            else
            {
                obj.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();

                obj.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
            }
            AutoCompleteEntry aceChiNhanh = new AutoComboBox().getEntryByDisplayName(lstSourceChiNhanh, ref cmbChiNhanh);
            AutoCompleteEntry acePhongGD = new AutoComboBox().getEntryByDisplayName(lstSourcePhongGD, ref cmbPhongGD);
            AutoCompleteEntry aceKhuVuc = new AutoComboBox().getEntryByDisplayName(lstSourceKhuVuc, ref cmbKhuVuc);
            AutoCompleteEntry aceCanBoQLy = new AutoComboBox().getEntryByDisplayName(lstSourceCanBoQL, ref cmbCanBoQLy);

            if (aceKhuVuc != null)
            {
                obj.ID_KVUC = Convert.ToInt32(aceKhuVuc.KeywordStrings.ElementAt(1));
                obj.MA_KVUC = aceKhuVuc.KeywordStrings.ElementAt(0);
            }

            if (acePhongGD != null)
            {
                obj.ID_DVI = Convert.ToInt32(acePhongGD.KeywordStrings.ElementAt(1));
                obj.MA_DVI = acePhongGD.KeywordStrings.ElementAt(0);
            }
            else if (aceChiNhanh != null)
            {
                obj.ID_DVI = Convert.ToInt32(aceChiNhanh.KeywordStrings.ElementAt(1));
                obj.MA_DVI = aceChiNhanh.KeywordStrings.ElementAt(0);
            }

            if (aceCanBoQLy != null)
            {
                obj.ID_CBO_QLY = Convert.ToInt32(aceCanBoQLy.KeywordStrings.ElementAt(1));
                obj.MA_CBO_QLY = aceCanBoQLy.KeywordStrings.ElementAt(0);
            }

            obj.ID_CUM_TRUONG = idCumTruong;
            obj.MA_CUM_TRUONG = MaCumTruong;

            obj.TEN_CUM = txtTenCum.Text;
            obj.TEN_TAT = txtTenTat.Text;

            if (raddtNgayTLap.Value is DateTime)
                obj.NGAY_TLAP = ((DateTime)raddtNgayTLap.Value).ToString("yyyyMMdd");

            obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
            obj.TTHAI_NVU = TthaiNvu;
            obj.MA_DVI_TAO = acePhongGD.KeywordStrings.ElementAt(0);
            obj.MA_DVI_QLY = aceChiNhanh.KeywordStrings.ElementAt(0); ;

            obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
            obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;

            // Get thông tin lịch họp

            DataView dt = (DataView)raddgrLichHopThu.ItemsSource;
            objTanSuatCum.ID_TSUAT = tanSuat.ID; // Em Tài fix để không lỗi dữ liệu khi tính kỳ họp cụm do không lấy được ID tuần suất từ bảng DC_TSUAT.
            objTanSuatCum.LHOP_DVI_TINH_TOAN = lstSourceDviTGianHop.ElementAt(cmbDviTGianHop.SelectedIndex).KeywordStrings.First();
            objTanSuatCum.ID_DVI = Convert.ToInt32(lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.ElementAt(1));
            objTanSuatCum.ID_KVUC = Convert.ToInt32(lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings.ElementAt(1));
            objTanSuatCum.MA_DVI_QLY = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            objTanSuatCum.MA_DVI_TAO = acePhongGD.KeywordStrings.ElementAt(0);
            string itemGIA_TRI_DDANG = "";
            if (objTanSuatCum.LHOP_DVI_TINH_TOAN.Equals("NGAY"))
            {
                foreach (DataRow r in dtLichHopCum.Rows)
                {
                    itemGIA_TRI_DDANG += "#" + r["NGAY"].ToString() + "." + r["THANG"].ToString();
                }
            }
            else if (objTanSuatCum.LHOP_DVI_TINH_TOAN.Equals("THU"))
            {
                foreach (DataRow r in dtLichHopCum.Rows)
                {
                    itemGIA_TRI_DDANG += "#" + r["THU"].ToString() + "." + r["TUAN"].ToString();
                }
            }
            objTanSuatCum.GIA_TRI_DDANG = itemGIA_TRI_DDANG.Substring(1, itemGIA_TRI_DDANG.Length - 1);

            objTanSuatCum.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
            objTanSuatCum.TTHAI_NVU = TthaiNvu;
            objTanSuatCum.NGUOI_NHAP = obj.NGUOI_NHAP;
            objTanSuatCum.NGAY_NHAP = obj.NGAY_NHAP;
            objTanSuatCum.NGUOI_CNHAT = ClientInformation.TenDangNhap;
            objTanSuatCum.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
            return;
        }

        /// <summary>
        /// Kiểm tra các thông tin nhập vào trước khi lưu dữ liệu
        /// </summary>
        /// <returns></returns>
        private bool Validation()
        {
            if (string.IsNullOrEmpty(cmbChiNhanh.Text))
            {
                LMessage.ShowMessage("M.DanhMuc.ucCumCT.ThieuChiNhanh", LMessage.MessageBoxType.Warning);
                cmbChiNhanh.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtTenCum.Text))
            {
                LMessage.ShowMessage("M.DanhMuc.ucCumCT.ThieuTenCum", LMessage.MessageBoxType.Warning);
                txtTenCum.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtTenTat.Text))
            {
                LMessage.ShowMessage("M.DanhMuc.ucCumCT.ThieuTenTat", LMessage.MessageBoxType.Warning);
                txtTenTat.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(raddtNgayTLap.Text))
            {
                LMessage.ShowMessage("M.DanhMuc.ucCumCT.ThieuNgayTLap", LMessage.MessageBoxType.Warning);
                raddtNgayTLap.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(cmbCanBoQLy.Text))
            {
                LMessage.ShowMessage("M.DanhMuc.ucCumCT.ThieuCanBoQLy", LMessage.MessageBoxType.Warning);
                cmbCanBoQLy.Focus();
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
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_CUM);
        }

        /// <summary>
        /// Trước khi xem từ danh sách
        /// </summary>
        public void beforeViewFromList()
        {
            SetFormData();
            SetEnabledAllControls(false);
            SetEnabledRequiredControls(false);
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_CUM);
        }

        /// <summary>
        /// Trước khi thêm mới
        /// </summary>
        private void beforeAddNew()
        {
            ResetForm();            
            SetEnabledAllControls(true);
            SetEnabledRequiredControls(false);

            //// Nếu người dùng là quản trị tại đơn vị hoặc đang ở trạng thái xem >> disable thông tin đơn vị
            //if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()) ||
            //    ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()))
            //{
            //    cmbChiNhanh.IsEnabled = false;
            //}
            //else
            //{
            //    cmbChiNhanh.IsEnabled = true;
            //}

            //cmbPhongGD.IsEnabled = true;
            //cmbKhuVuc.IsEnabled = true;

            action = DatabaseConstant.Action.THEM;
            CommonFunction.RefreshButton(Toolbar, action, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_CUM);
        }

        /// <summary>
        /// Trước khi sửa từ danh sách
        /// </summary>
        public void beforeModifyFromList()
        {
            SetFormData();
            SetEnabledAllControls(true);
            SetEnabledRequiredControls(false);

            //cmbPhongGD.IsEnabled = true;
            //cmbKhuVuc.IsEnabled = true;

            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_CUM);
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

            bool ret = process.LockData(module, function, table,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Nếu lock thành công >> cho phép sửa
            if (ret)
            {
                SetEnabledAllControls(true);
                SetEnabledRequiredControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_CUM);
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
            string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            if (Validation())
            {
                DanhMucProcess danhmucProcess = new DanhMucProcess();
                try
                {
                    // Dữ liệu truyền vào và dữ liệu trả về
                    ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    DM_CUM obj = new DM_CUM();
                    DC_TSUAT_CUM objTanSuatCum = new DC_TSUAT_CUM();
                    string responseMessage = null;

                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (id == 0)
                    {
                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref objTanSuatCum);
                        ret = danhmucProcess.ThemCum(ref obj, ref objTanSuatCum, ref responseMessage);
                        afterAddNew(ret, obj, responseMessage);
                    }
                    // Nếu là lưu tạm khi sửa
                    // Hoặc lưu tạm khi sửa sau duyệt
                    // Hoặc sửa
                    else
                    {
                        // Lấy thông tin cũ
                        danhmucProcess.getCumById(id, ref obj, ref responseMessage);

                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref objTanSuatCum);
                        ret = danhmucProcess.SuaCum(ref obj, ref objTanSuatCum, ref responseMessage);
                        afterModify(ret, obj, responseMessage);
                    }
                }
                catch (System.Exception ex)
                {
                    // Yêu cầu unlock dữ liệu khi xảy ra lỗi
                    // (chỉ trường hợp xóa dữ liệu)
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);

                    bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_DM_CUM,
                        DatabaseConstant.Table.DM_CUM,
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
                    DM_CUM obj = new DM_CUM();
                    DC_TSUAT_CUM objTanSuatCum = new DC_TSUAT_CUM();
                    DM_CUM ret = null;
                    string responseMessage = null;

                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (id == 0)
                    {
                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref objTanSuatCum);
                        obj.TTHAI_NVU = trangThai;

                        // Set các thông tin khác                        
                        obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                        obj.NGAY_NHAP = LDateTime.GetCurrentDate(ApplicationConstant.defaultDateTimeFormat);
                    }
                    // Nếu là lưu tạm khi sửa
                    // Hoặc lưu tạm khi sửa sau duyệt
                    // Hoặc sửa
                    else
                    {
                        // Lấy thông tin cũ
                        danhmucProcess.getCumById(id, ref obj, ref responseMessage);

                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref objTanSuatCum);
                        obj.TTHAI_NVU = trangThai;
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
                bool ret = danhmucProcess.XoaCum(arrayID, ref listClientResponseDetail);

                afterDelete(ret, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu khi xảy ra lỗi
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_CUM,
                    DatabaseConstant.Table.DM_CUM,
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
                bool ret = danhmucProcess.DuyetCum(arrayID, ref listClientResponseDetail);

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
                bool ret = danhmucProcess.ThoaiDuyetCum(arrayID, ref listClientResponseDetail);

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
        private void afterAddNew(ApplicationConstant.ResponseStatus ret, DM_CUM obj, string responseMessage)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                if (cbMultiAdd.IsChecked == true)
                {
                    ResetForm();
                }
                else if (!DatabaseConstant.CLOSE_DETAIL_FORM)
                {
                    id = obj.ID;
                    TthaiNvu = obj.TTHAI_NVU;

                    txtMaCum.Text = obj.MA_CUM;
                    lblTrangThaiBanGhi.Content = BusinessConstant.layNgonNguNghiepVu(obj.TTHAI_NVU);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguNghiepVu(obj.TTHAI_NVU);

                    SetEnabledAllControls(false);
                    SetEnabledRequiredControls(false);
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_CUM);

                    tbiThongTinChung.Focus();
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
        private void afterModify(ApplicationConstant.ResponseStatus ret, DM_CUM obj, string responseMessage)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                id = obj.ID;
                TthaiNvu = obj.TTHAI_NVU;
                lblTrangThaiBanGhi.Content = BusinessConstant.layNgonNguSuDung(obj.TTHAI_NVU);
                txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_NVU);

                SetEnabledAllControls(false);
                SetEnabledRequiredControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_CUM);

                tbiThongTinChung.Focus();
            }
            else
            {
                LMessage.ShowMessage(responseMessage, LMessage.MessageBoxType.Error);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool retUnlockData = process.UnlockData(module, function, table,
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
            List<int> listUnlockId = new List<int>();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_CUM,
                DatabaseConstant.Table.DM_CUM,
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
                lblTrangThaiBanGhi.Content = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);
                txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);

                SetEnabledAllControls(false);
                SetEnabledRequiredControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_CUM);
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
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_CUM);
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
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_CUM);
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

        #endregion

        private void cmbDviTGianHop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetTanSuat();
        }
    }
}
