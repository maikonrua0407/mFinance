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
using Presentation.Process.DanhMucServiceRef;
using Presentation.Process.KhachHangServiceRef;
using Presentation.Process.UtilitiesServiceRef;
using System.Collections;
using System.Reflection;
using PresentationWPF.DanhMuc.Popup;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls;

namespace PresentationWPF.DanhMuc.Nhom
{
    /// <summary>
    /// Interaction logic for ucNhomCT_01.xaml
    /// </summary>
    public partial class ucNhomCT_01 : UserControl
    {
        #region Khai bao

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

        private string maCum;

        public string MaCum
        {
            get { return maCum; }
            set { maCum = value; }
        }

        private bool chiXem;

        public bool ChiXem
        {
            get { return chiXem; }
            set { chiXem = value; }
        }

        private bool isLoaded = false;
        
        List<DataRow> lstChiTiet;

        public List<DataRow> LstChiTiet
        {
            get { return lstChiTiet; }
            set { lstChiTiet = value; }
        }

        private int ID = 0;

        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        DM_NHOM obj = new DM_NHOM();

        private DatabaseConstant.Module module = DatabaseConstant.Module.DMDC;
        private DatabaseConstant.Function function = DatabaseConstant.Function.DC_DM_NHOM;
        private DatabaseConstant.Table table = DatabaseConstant.Table.DM_NHOM;

        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKhuVuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNhom = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiNhom = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCanBoQuanLy = new List<AutoCompleteEntry>();

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

        public event EventHandler OnSavingCompleted;

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        List<int> lstIDXoa = new List<int>();
        List<KH_KHANG_NHOM> lstKHangNhom = new List<KH_KHANG_NHOM>();
        DataTable dtkhachHang;
        string LoaiNhom = "NHOM_TRA_DAN";
        #endregion

        #region Khoi tao

        public ucNhomCT_01()
        {
            InitializeComponent();

            BindShortkey();

            LoadCombobox();

            InitEventHandler();
            ShowControl();
            ResetForm();

            // Refresh buttons
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_NHOM);
        }

        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.DanhMuc.Nhom.ucNhomCT_01", "");
            foreach (List<string> lst in arr)
            {
                object item = Toolbar.FindName(lst.First());
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
            stpNhomCha.Visibility = Visibility.Collapsed;
            txtMaNhomCha.Visibility = Visibility.Collapsed;
            btnMaNhomCha.Visibility = Visibility.Collapsed;
            lblTenNhomCha.Visibility = Visibility.Collapsed;
            cmbNhomCha.Visibility = Visibility.Collapsed;
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

            cmbLoaiNhom.Items.Clear();
            lstSourceLoaiNhom.Clear();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.LOAI_NHOM.getValue());
            auto.GenAutoComboBox(ref lstSourceLoaiNhom, ref cmbLoaiNhom, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien,"NHOM_TRA_DAN");

            if (lstSourceChiNhanh.Select(i => i.DisplayName).Contains(cmbChiNhanh.Text))
            {
                cmbPhongGD.Items.Clear();
                lstDieuKien.Clear();
                lstSourcePhongGD.Clear();
                lstDieuKien.Add(ClientInformation.MaDonVi);
                auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOMACHA.getValue(), lstDieuKien);

                cmbKhuVuc.Items.Clear();
                lstDieuKien.Clear();
                lstSourceKhuVuc.Clear();
                lstDieuKien.Add(ClientInformation.IdDonVi.ToString());
                auto.GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUC.getValue(), lstDieuKien);

                

                cmbCanBoQuanLy.Items.Clear();
                lstDieuKien.Clear();
                lstSourceCanBoQuanLy.Clear();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                auto.GenAutoComboBox(ref lstSourceCanBoQuanLy, ref cmbCanBoQuanLy, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NSD.getValue(), lstDieuKien);

                if (lstSourcePhongGD.Select(i => i.DisplayName).Contains(cmbPhongGD.Text))
                {
                    string maPhongGD = lstSourcePhongGD.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
                    string idPhongGd = lstSourcePhongGD.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.ElementAt(1);
                    cmbKhuVuc.Items.Clear();
                    lstDieuKien.Clear();
                    lstSourceKhuVuc.Clear();
                    lstDieuKien.Add(idPhongGd);
                    auto.GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUC.getValue(), lstDieuKien);
                    LoadComboboxCum();
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
        }

        /// <summary>
        /// InitEventHandler
        /// </summary>
        private void InitEventHandler()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.DanhMuc;component/Nhom/ucNhomCT_01.xaml", ref Toolbar, ref mnuMain);

            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }

            cmbChiNhanh.SelectionChanged += new SelectionChangedEventHandler(cmbChiNhanh_SelectionChanged);
            cmbPhongGD.SelectionChanged += new SelectionChangedEventHandler(cmbPhongGD_SelectionChanged);
            cmbKhuVuc.SelectionChanged +=new SelectionChangedEventHandler(cmbKhuVuc_SelectionChanged);
            cmbCum.SelectionChanged += new SelectionChangedEventHandler(cmbCum_SelectionChanged);
            cmbLoaiNhom.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiNhom_SelectionChanged);
            cmbChiNhanh.KeyDown += new KeyEventHandler(cmbChiNhanh_KeyDown);
            cmbPhongGD.KeyDown += new KeyEventHandler(cmbPhongGD_KeyDown);            
            btnMaNhomCha.Click += new RoutedEventHandler(btnMaNhomCha_Click);
            tlbAddKHang.Click += new RoutedEventHandler(tlbAddKHang_Click);
            tlbDelKHang.Click += new RoutedEventHandler(tlbDelKHang_Click);
        }

        void btnMaNhomCha_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                process.getPopupInformation("POPUP_DM_NHOM_TRA_DAN", lstDieuKien);

                SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey("U.POPUP.POPUP_DS_NHOM");
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow dr = lstPopup[0];
                    txtMaNhomCha.Text = dr["MA_NHOM"].ToString();
                    txtMaNhomCha.Tag = Convert.ToInt32(dr["ID"]);
                    lblTenNhomCha.Content = dr["TEN_NHOM"].ToString();
                }
            }
            catch (System.Exception ex)
            {
            	
            }
        }

        /// <summary>
        /// Đưa form về trạng thái mặc định
        /// </summary>
        private void ResetForm()
        {
            id = 0;
            tthaiNvu = "";
            maCum = "";
            lstChiTiet = null;
            obj = new DM_NHOM();
            lstIDXoa = new List<int>();


            lblTrangThai.Content = "";

            txtMaNhom.Text = "";
            txtTenNhom.Text = "";
            txtTenTat.Text = "";
            raddtNgayTLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            raddtNgayHetHLuc.Value = null;
            raddtNgayNhap.Text = "";
            txtNguoiNhap.Text = "";
            raddtNgayCapNhat.Text = "";
            txtNguoiCapNhat.Text = "";
            txtTrangThaiBanGhi.Text = "";
            KhoiTaoDataSource();
            cmbLoaiNhom.IsEnabled = true;
        }

        private void KhoiTaoDataSource()
        {
            dtkhachHang = new DataTable();
            dtkhachHang.Columns.Add("ID", typeof(int));
            dtkhachHang.Columns.Add("ID_KHANG", typeof(int));
            dtkhachHang.Columns.Add("MA_KHANG", typeof(string));
            dtkhachHang.Columns.Add("TEN_KHANG", typeof(string));
            dtkhachHang.Columns.Add("DD_GTLQ_SO", typeof(string));
            dtkhachHang.Columns.Add("TRUONG_NHOM", typeof(string));
            dtkhachHang.Columns.Add("MA_NHOM", typeof(string));
            dtkhachHang.Columns.Add("TTHAI_NVU", typeof(string));
            dtkhachHang.Columns.Add("TTHAI_BGHI", typeof(string));
            raddgrKhachHangTVien.ItemsSource = dtkhachHang.DefaultView;
            //raddgrKhachHangTVien.Rebind();
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
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.H, ModifierKeys.Control);
                        key = new KeyBinding(HoldCommand, keyg);
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

        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbAdd.IsEnabled;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeAddNew();
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
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbRefuse.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbCancel.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
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
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                beforeAddNew();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                //Luu();
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
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
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
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                beforeAddNew();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                //Luu();
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
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
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
            cmbChiNhanh.IsEnabled = enable;
            cmbPhongGD.IsEnabled = enable;
            cmbKhuVuc.IsEnabled = enable;
            cmbCum.IsEnabled = enable;
            cmbNhomCha.IsEnabled = enable;
            cmbCanBoQuanLy.IsEnabled = enable;

            txtMaNhom.IsEnabled = enable;
            txtTenNhom.IsEnabled = enable;
            txtTenTat.IsEnabled = enable;
            raddtNgayTLap.IsEnabled = enable;
            dtpNgayTLap.IsEnabled = enable;
            raddtNgayHetHLuc.IsEnabled = enable;
            dtpNgayHetHLuc.IsEnabled = enable;

            txtTrangThaiBanGhi.IsEnabled = enable;
            txtNguoiNhap.IsEnabled = enable;
            raddtNgayNhap.IsEnabled = enable;
            txtNguoiCapNhat.IsEnabled = enable;
            raddtNgayCapNhat.IsEnabled = enable;
            tlbAddKHang.IsEnabled = enable;
            tlbDelKHang.IsEnabled = enable;
            raddgrKhachHangTVien.IsReadOnly = !enable;
        }

        /// <summary>
        /// SetEnabledRequiredControls
        /// </summary>
        /// <param name="enable"></param>
        private void SetEnabledRequiredControls(bool enable)
        {
            cmbChiNhanh.IsEnabled = enable;
            cmbPhongGD.IsEnabled = enable;
            cmbKhuVuc.IsEnabled = enable;
            cmbCum.IsEnabled = enable;
            //cmbLoaiNhom.IsEnabled = enable;
            cmbNhomCha.IsEnabled = enable;

            txtMaNhom.IsEnabled = enable;

            txtTrangThaiBanGhi.IsEnabled = enable;
            txtNguoiNhap.IsEnabled = enable;
            raddtNgayNhap.IsEnabled = enable;
            txtNguoiCapNhat.IsEnabled = enable;
            raddtNgayCapNhat.IsEnabled = enable;
            raddgrKhachHangTVien.IsReadOnly = !enable;            

            // Người dùng đơn vị chỉ được quản lý dữ liệu của đơn vị mình
            if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()) ||
                ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()))
            {
                cmbChiNhanh.IsEnabled = false;
            }
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
            //LoadForm();
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

        private void LoadForm()
        {
            DanhMucProcess danhmucProcess = new DanhMucProcess();
            try
            {
                    if (!isLoaded)
                    {
                        if (LstChiTiet != null)
                        {
                            //0-STT,1-ID,2-MA_NHOM,3-TEN_NHOM,4-TEN_TAT,5-MA_CUM,6-MA_KVUC,7-MA_DON_VI,10-NGAY_TLAP,
                            //11-TTHAI_NVU,12-TTHAI_BGHI,13-NGAY_NHAP,14-NGUOI_NHAP,15-NGAY_CNHAT,16-NGUOI_CNHAT,17-KEY
                            DataRow row = LstChiTiet[0];
                            if (!string.IsNullOrWhiteSpace(row[10].ToString()))
                                lblTrangThai.Content = row[10].ToString();
                            if (!string.IsNullOrWhiteSpace(row[2].ToString()))
                                txtMaNhom.Text = row[2].ToString();
                            if (!string.IsNullOrWhiteSpace(row[3].ToString()))
                                txtTenNhom.Text = row[3].ToString();
                            if (!string.IsNullOrWhiteSpace(row[4].ToString()))
                                txtTenTat.Text = row[4].ToString();
                            if (!string.IsNullOrWhiteSpace(row[1].ToString()))
                                ID = Convert.ToInt32(row[1]);
                            if (!string.IsNullOrWhiteSpace(row[10].ToString()))
                                raddtNgayTLap.Value = LDateTime.StringToDate(row[10].ToString(), "yyyyMMdd");

                            // khởi tạo combobox
                            AutoComboBox auto = new AutoComboBox();
                            // Khởi tạo điều kiện gọi danh mục
                            List<string> lstDieuKien = new List<string>();
                            string MaCum = "Default";
                            if (!string.IsNullOrWhiteSpace(row[5].ToString()))
                                MaCum = row[5].ToString();
                            string MaKhuVuc = "Default";
                            if (!string.IsNullOrWhiteSpace(row[6].ToString()))
                                MaKhuVuc = row[6].ToString();
                            string MaDonVi = "Default";
                            if (!string.IsNullOrWhiteSpace(row[7].ToString()))
                                MaDonVi = row[7].ToString();
                        
                            // khởi tạo combobox
                            auto = new AutoComboBox();
                            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVI.getValue(), null , MaDonVi);
                            MaDonVi = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                            string Id_DonVi = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.ElementAt(1);
                            string maDonVi = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();

                            auto = new AutoComboBox();
                            List<string> lstDonVi = new List<string>();
                            lstDonVi.Add(maDonVi);
                            cmbKhuVuc.Items.Clear();
                            lstSourceKhuVuc = new List<AutoCompleteEntry>();
                            auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue(), lstDonVi);
                            cmbPhongGD.SelectedIndex = 0;
                            string idPhongGD = lstSourcePhongGD.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.ElementAt(1);

                            // Gán giá trị điều kiện
                            lstDieuKien = new List<string>();
                            lstDieuKien.Add(null);
                            lstDieuKien.Add(null);
                            lstDieuKien.Add(idPhongGD);
                            // khởi tạo combobox
                            auto = new AutoComboBox();
                            auto.GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUC.getValue(), lstDieuKien, MaKhuVuc);

                            // khởi tạo combobox
                            auto = new AutoComboBox();
                            lstDieuKien = new List<string>();
                            cmbCum.Items.Clear();
                            lstDieuKien.Clear();
                            lstSourceCum.Clear();
                            lstDieuKien.Add(idPhongGD);
                            string idKhuVuc = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings.ElementAt(1);
                            if (!idKhuVuc.IsNullOrEmptyOrSpace())
                                lstDieuKien.Add(idKhuVuc);
                            else
                                lstDieuKien.Add("NULL");
                            auto.GenAutoComboBox(ref lstSourceCum, ref cmbCum, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM.getValue(), lstDieuKien);
                            MaCum = lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings.First();

                            //11-TTHAI_NVU,12-TTHAI_BGHI,13-NGAY_NHAP,14-NGUOI_NHAP,15-NGAY_CNHAT,16-NGUOI_CNHAT,17-KEY
                            lblTrangThai.Content = row[11].ToString();
                            txtTrangThaiBanGhi.Text = row[12].ToString();
                            raddtNgayNhap.Value = LDateTime.StringToDate(row[13].ToString(), "yyyyMMdd");
                            txtNguoiNhap.Text = row[14].ToString();
                            raddtNgayCapNhat.Value = LDateTime.StringToDate(row[15].ToString(), "yyyyMMdd");
                            txtNguoiCapNhat.Text = row[16].ToString();

                            // khởi tạo sự kiện combobox
                            cmbChiNhanh.KeyDown += cmbChiNhanh_KeyDown;
                            cmbKhuVuc.KeyDown += cmbKhuVuc_KeyDown;
                            cmbCum.KeyDown += cmbCum_KeyDown;
                            cmbChiNhanh.SelectionChanged += cmbChiNhanh_SelectionChanged;
                        }
                        else
                        {
                            tlbApprove.IsEnabled = false;
                            tlbCancel.IsEnabled = false;
                            tlbDelete.IsEnabled = false;
                            tlbModify.IsEnabled = false;
                            tlbRefuse.IsEnabled = false;
                        }
                        tabNhomCT.SelectionChanged += tabNhomCT_SelectionChanged;
                        EnableControl();
                    }
                
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            
        }


        private void tabNhomCT_SelectionChanged(object sender, Telerik.Windows.Controls.RadSelectionChangedEventArgs e)
        {
            if (tabNhomCT.SelectedIndex == 0)
            {
                if (txtMaNhom != null)
                {
                    UpdateLayout();
                    txtMaNhom.Focus();
                }
            }
            else if (tabNhomCT.SelectedIndex == 1)
            {
                if (txtTrangThaiBanGhi != null)
                {
                    UpdateLayout();
                    txtTrangThaiBanGhi.Focus();
                }
            }
        }

        /// <summary>
        /// Sự kiện khi thay đổi giá trị của combobox cmbChiNhanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstSourceChiNhanh.Select(i => i.DisplayName).Contains(cmbChiNhanh.Text))
            {
                string maDonVi = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                string idDonVi = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.ElementAt(1);
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDonVi = new List<string>();
                lstDonVi.Add(maDonVi);
                cmbKhuVuc.Items.Clear();
                lstSourceKhuVuc = new List<AutoCompleteEntry>();
                auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue(), lstDonVi);
                cmbPhongGD.SelectedIndex = 0;
                cmbPhongGD_SelectionChanged(null, null);
            }
            else
                cmbKhuVuc.Text = "";
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

                // khởi tạo combobox
                auto = new AutoComboBox();
                lstDieuKien = new List<string>();
                cmbCum.Items.Clear();
                lstDieuKien.Clear();
                lstSourceCum.Clear();
                lstDieuKien.Add(idPhongGD);
                string idKhuVuc = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings.ElementAt(1);
                if (!idKhuVuc.IsNullOrEmptyOrSpace())
                    lstDieuKien.Add(idKhuVuc);
                else
                    lstDieuKien.Add("NULL");
                auto.GenAutoComboBox(ref lstSourceCum, ref cmbCum, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM.getValue(), lstDieuKien);
                MaCum = lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings.First();

            }
            else
            {
                cmbKhuVuc.Items.Clear();
                lstSourceKhuVuc.Clear();
            }
        }

        /// <summary>
        /// Sự kiện khi thay đổi giá trị của combobox cmbKhuVuc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbKhuVuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxCum();
        }

        void LoadComboboxCum()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            string idPhongGd = lstSourcePhongGD.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.ElementAt(1);
            cmbCum.Items.Clear();
            lstDieuKien.Clear();
            lstSourceCum.Clear();
            lstDieuKien.Add(idPhongGd);
            if (cmbKhuVuc.SelectedIndex >= 0)
            {
                string idKhuVuc = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings.ElementAt(1);
                if (!idKhuVuc.IsNullOrEmptyOrSpace())
                    lstDieuKien.Add(idKhuVuc);
                else
                    lstDieuKien.Add("NULL");
                auto.GenAutoComboBox(ref lstSourceCum, ref cmbCum, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM.getValue(), lstDieuKien);
                MaCum = (lstSourceCum != null && lstSourceCum.Count > 0) ? lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings.First() : "";
            }
            //Set mặc định cán bộ quản lý nhóm bằng cán bộ quản lý cụm
            if (cmbCum.SelectedIndex >= 0)
            {
                DM_CUM objCum = null;
                int idCum = Convert.ToInt32(lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings[1]);
                string responseMessage = "";
                ApplicationConstant.ResponseStatus responseStatus = new DanhMucProcess().getCumById(idCum, ref objCum, ref responseMessage);
                if (objCum != null)
                    cmbCanBoQuanLy.SelectedIndex = lstSourceCanBoQuanLy.IndexOf(lstSourceCanBoQuanLy.FirstOrDefault(i => i.KeywordStrings.First().Equals(objCum.MA_CBO_QLY)));
            }
        }

        /// <summary>
        /// Sự kiện khi thay đổi giá trị của combobox cmbKhuVuc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoComboBox au = new AutoComboBox();
            List<string> lstDK = new List<string>();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                AutoCompleteEntry auPGD = au.getEntryByDisplayName(lstSourcePhongGD, ref cmbPhongGD);
                AutoCompleteEntry auKhuVuc = au.getEntryByDisplayName(lstSourceKhuVuc, ref cmbKhuVuc);
                AutoCompleteEntry auCum = au.getEntryByDisplayName(lstSourceCum, ref cmbCum);
                if (auPGD != null && auCum != null)
                {
                    lstSourceNhom.Clear();
                    cmbNhomCha.Items.Clear();
                    lstDK.Add(auPGD.KeywordStrings[1]);
                    lstDK.Add(auKhuVuc.KeywordStrings[1]);
                    lstDK.Add(auCum.KeywordStrings[1]);
                    au.GenAutoComboBox(ref lstSourceNhom, ref cmbNhomCha, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NHOM.getValue(), lstDK);

                    //Set mặc định cán bộ quản lý nhóm bằng cán bộ quản lý cụm
                    if (cmbCum.SelectedIndex >= 0)
                    {
                        DM_CUM objCum = null;
                        int idCum = Convert.ToInt32(lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings[1]);
                        string responseMessage = "";
                        ApplicationConstant.ResponseStatus responseStatus = new DanhMucProcess().getCumById(idCum, ref objCum, ref responseMessage);
                        if (objCum != null)
                            cmbCanBoQuanLy.SelectedIndex = lstSourceCanBoQuanLy.IndexOf(lstSourceCanBoQuanLy.FirstOrDefault(i => i.KeywordStrings.First().Equals(objCum.MA_CBO_QLY)));
                    }
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                au = null;
            }
        }

        /// <summary>
        /// Sự kiện keydown của ComboBox Đơn vị
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbChiNhanh_KeyDown(object sender, KeyEventArgs e)
        {
            cmbChiNhanh.IsDropDownOpen = true;
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

        /// <summary>
        /// Sự kiện KeyDown của ComboBox Khu vực
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbKhuVuc_KeyDown(object sender, KeyEventArgs e)
        {
            cmbKhuVuc.IsDropDownOpen = true;
        }

        /// <summary>
        /// Sự kiện KeyDown của ComboBox Khu vực
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbCum_KeyDown(object sender, KeyEventArgs e)
        {
            cmbCum.IsDropDownOpen = true;
        }

        private void EnableControl()
        {
            txtMaNhom.IsEnabled = !ChiXem;
            raddtNgayNhap.IsEnabled = !ChiXem;
            raddtNgayCapNhat.IsEnabled = !ChiXem;
            txtNguoiCapNhat.IsEnabled = !ChiXem;
            txtNguoiNhap.IsEnabled = !ChiXem;
            txtTenNhom.IsEnabled = !ChiXem;
            txtTenTat.IsEnabled = !ChiXem;
            txtTrangThaiBanGhi.IsEnabled = !ChiXem;
            cmbCum.IsEnabled = !ChiXem;
            cmbChiNhanh.IsEnabled = !ChiXem;
            cmbKhuVuc.IsEnabled = !ChiXem;
        }

        void tlbDelKHang_Click(object sender, RoutedEventArgs e)
        {
            List<DataRow> lstRow = new List<DataRow>();
            if(LoaiNhom=="NHOM_TRA_DAN")
            {
                foreach (DataRowView dr in raddgrKhachHangTVien.SelectedItems)
                {
                    if (Convert.ToInt32(dr["ID_KHANG"]) > 0)
                        lstIDXoa.Add(Convert.ToInt32(dr["ID_KHANG"]));
                    lstRow.Add(dr.Row);
                }
            }
            else
            {
                foreach (DataRowView dr in raddgrKhachHangTVien.SelectedItems)
                {
                    if (Convert.ToInt32(dr["ID_KHANG"]) > 0)
                        lstIDXoa.Add(Convert.ToInt32(dr["ID_KHANG"]));
                    lstRow.Add(dr.Row);
                }
            }
            foreach (DataRow dr in lstRow)
            {
                dtkhachHang.Rows.Remove(dr);
            }
            raddgrKhachHangTVien.ItemsSource = dtkhachHang.DefaultView;
        }

        void tlbAddKHang_Click(object sender, RoutedEventArgs e)
        {
            if (LoaiNhom=="NHOM_TRA_DAN")
            {
                PopUpKhachHangNhomTraDan();
            } 
            else
            {
                PopUpKhachHangNhomThoiVu();
            }
        }

        /// <summary>
        /// Hàm kiểm tra khách hàng đã được chọn trong Grid hay chưa
        /// </summary>
        /// <param name="maKhachHang">số sổ tiền gửi</param>
        /// <returns>true: chưa được chọn;  false đã được chọn</returns>
        private bool KiemTraKhachHang(string maKhachHang)
        {
            foreach (DataRow dr in dtkhachHang.Rows)
            {
                if (dr["MA_KHANG"].ToString().Equals(maKhachHang))
                {
                    return false;
                }
            }
            return true;
        }

        private void rdoTruongNhom_Checked(object sender, RoutedEventArgs e)
        {
            var rdo = sender as RadioButton;
            GridViewRow grrow = rdo.ParentOfType<GridViewRow>();
            DataRow dr = grrow.Item as DataRow;
            int index = dtkhachHang.Rows.IndexOf(dr);
            foreach (DataRow drow in dtkhachHang.Rows)
            {
                int indexS = dtkhachHang.Rows.IndexOf(drow);
                if (indexS == index)
                    drow["TRUONG_NHOM"] = "true";
                else
                    drow["TRUONG_NHOM"] = "false";
            }
        }

        void cmbLoaiNhom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoaiNhom = lstSourceLoaiNhom.ElementAt(cmbLoaiNhom.SelectedIndex).KeywordStrings.FirstOrDefault();
            if (LoaiNhom == "NHOM_TRA_DAN")
            {
                stpNhomCha.Visibility = Visibility.Collapsed;
                txtMaNhomCha.Visibility = Visibility.Collapsed;
                btnMaNhomCha.Visibility = Visibility.Collapsed;
                lblTenNhomCha.Visibility = Visibility.Collapsed;
                cmbNhomCha.Visibility = Visibility.Collapsed;
            }
            else
            {
                stpNhomCha.Visibility = Visibility.Visible;
                txtMaNhomCha.Visibility = Visibility.Collapsed;
                btnMaNhomCha.Visibility = Visibility.Collapsed;
                lblTenNhomCha.Visibility = Visibility.Collapsed;
                cmbNhomCha.Visibility = Visibility.Visible;

                AutoComboBox au = new AutoComboBox();
                List<string> lstDK = new List<string>();
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    AutoCompleteEntry auPGD = au.getEntryByDisplayName(lstSourcePhongGD, ref cmbPhongGD);
                    AutoCompleteEntry auKhuVuc = au.getEntryByDisplayName(lstSourceKhuVuc, ref cmbKhuVuc);
                    AutoCompleteEntry auCum = au.getEntryByDisplayName(lstSourceCum, ref cmbCum);
                    if (auPGD != null && auCum != null)
                    {
                        lstSourceNhom.Clear();
                        cmbNhomCha.Items.Clear();
                        lstDK.Add(auPGD.KeywordStrings[1]);
                        lstDK.Add(auKhuVuc.KeywordStrings[1]);
                        lstDK.Add(auCum.KeywordStrings[1]);
                        au.GenAutoComboBox(ref lstSourceNhom, ref cmbNhomCha, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NHOM.getValue(), lstDK);
                    }
                }
                catch (System.Exception ex)
                {
                    LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                }
                finally
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                    au = null;
                }
            }
            KhoiTaoDataSource();
        }

        void PopUpKhachHangNhomTraDan()
        {
            try
            {
                Window window = new Window();
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.RenderSize = new Size(1024, 768);
                PopupProcess popupProcess = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                popupProcess.getPopupInformation("POPUP_DS_KHANG_HANG_TRA_DAN", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(false, simplePopupResponse, true);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                window.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                window.Content = popup;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
                if (lstPopup.Count > 0)
                {
                    foreach (DataRow drv in lstPopup)
                    {
                        //DataSet dsSourceKHang = new KhachHangProcess().getThongTinKHTheoID(Convert.ToInt32(drv["ID"]));
                        if (KiemTraKhachHang(drv["MA_KHANG"].ToString()))
                        {
                            DataRow dr = dtkhachHang.NewRow();
                            dr["ID_KHANG"] = drv["ID"];
                            dr["MA_KHANG"] = drv["MA_KHANG"];
                            dr["TEN_KHANG"] = drv["TEN_KHANG"];
                            dr["DD_GTLQ_SO"] = drv["DD_GTLQ_SO"];
                            dr["TRUONG_NHOM"] = "false";
                            dr["TTHAI_NVU"] = drv["TTHAI_NVU"];
                            dr["TTHAI_BGHI"] = "T";
                            dtkhachHang.Rows.Add(dr);
                        }
                    }
                    raddgrKhachHangTVien.ItemsSource = dtkhachHang.DefaultView;
                }
                popup = null;
                
            }
            catch
            {
                LMessage.ShowMessage("M.DungChung.Result.KhongThanhCong", LMessage.MessageBoxType.Error);
            }
        }

        void PopUpKhachHangNhomThoiVu()
        {
            try
            {
                lstPopup.Clear();
                string lstIDKHang = "";
                foreach (DataRow dr in dtkhachHang.Rows)
                {
                    lstIDKHang += "," + dr["ID_KHANG"].ToString();
                }
                if (lstIDKHang.Length > 0)
                    lstIDKHang = lstIDKHang.Substring(1);
                else
                    lstIDKHang = "(0)";

                AutoCompleteEntry aceNhomCha = new AutoComboBox().getEntryByDisplayName(lstSourceNhom, ref cmbNhomCha);
                string maNhomCha = "";
                if (aceNhomCha != null)
                {
                    maNhomCha = aceNhomCha.KeywordStrings.ElementAt(0);
                }
                else
                {
                    maNhomCha = "";
                }

                Window window = new Window();
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.RenderSize = new Size(1024, 768);
                PopupProcess popupProcess = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(lstIDKHang);
                lstDieuKien.Add(maNhomCha);
                popupProcess.getPopupInformation("POPUP_DS_KHANG_HANG_THOI_VU", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(false, simplePopupResponse, true);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                window.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                window.Content = popup;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
                if (lstPopup.Count > 0)
                {
                    foreach (DataRow drv in lstPopup)
                    {
                        //DataSet dsSourceKHang = new KhachHangProcess().getThongTinKHTheoID(Convert.ToInt32(drv["ID"]));
                        if (KiemTraKhachHang(drv["MA_KHANG"].ToString()))
                        {
                            DataRow dr = dtkhachHang.NewRow();
                            dr["ID_KHANG"] = drv["ID"];
                            dr["MA_KHANG"] = drv["MA_KHANG"];
                            dr["TEN_KHANG"] = drv["TEN_KHANG"];
                            dr["DD_GTLQ_SO"] = drv["DD_GTLQ_SO"];
                            dr["TRUONG_NHOM"] = "false";
                            dr["TTHAI_NVU"] = drv["TTHAI_NVU"];
                            dr["TTHAI_BGHI"] = "T";
                            dtkhachHang.Rows.Add(dr);
                        }
                    }
                    raddgrKhachHangTVien.ItemsSource = dtkhachHang.DefaultView;
                }
                popup = null;
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.Result.KhongThanhCong", LMessage.MessageBoxType.Error);
            }
            
        }
        #endregion

        #region Xu ly nghiep vu

        /// <summary>
        /// Xử lý xóa dữ liệu
        /// </summary>
        private void Luu()
        {
            DanhMucProcess danhmucProcess = new DanhMucProcess();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                string trangThai=string.Empty;
                DM_NHOM obj = new DM_NHOM();
                if (ID > 0)
                    obj.ID = ID;
                obj.ID_DVI = Convert.ToInt32(lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.ElementAt(1));
                obj.ID_KVUC = Convert.ToInt32(lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings.ElementAt(1));
                obj.ID_CUM = Convert.ToInt32(lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings.ElementAt(1));
                obj.MA_CUM = lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings.ElementAt(0);
                obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                obj.MA_DVI_TAO = ClientInformation.MaDonVi;
                obj.MA_DVI = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.ElementAt(0);
                obj.MA_KVUC = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings.ElementAt(0);
                obj.NGAY_TLAP = string.IsNullOrEmpty(raddtNgayTLap.Value.ToString().Trim()) ? ((DateTime)raddtNgayTLap.Value).ToString("yyyyMMdd") : (DateTime.Today).ToString("yyyyMMdd");
                obj.NGAY_HET_HLUC = string.IsNullOrEmpty(raddtNgayHetHLuc.Value.ToString().Trim()) ? ((DateTime)raddtNgayHetHLuc.Value).ToString("yyyyMMdd") : (DateTime.Today).ToString("yyyyMMdd");

                obj.NGAY_CNHAT = string.IsNullOrEmpty(raddtNgayCapNhat.Value.ToString().Trim()) ? ((DateTime)raddtNgayCapNhat.Value).ToString("yyyyMMdd") : (DateTime.Today).ToString("yyyyMMdd");
                obj.NGAY_NHAP = string.IsNullOrEmpty(raddtNgayNhap.Value.ToString().Trim()) ? ((DateTime)raddtNgayNhap.Value).ToString("yyyyMMdd") : (DateTime.Today).ToString("yyyyMMdd");
                obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                if (ID == 0)
                    obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                obj.MA_NHOM = txtMaNhom.Text;
                obj.TEN_NHOM = txtTenNhom.Text;
                obj.TEN_TAT = txtTenTat.Text;
                obj.TTHAI_NVU = trangThai;

                AutoCompleteEntry aceCanBoQLy = new AutoComboBox().getEntryByDisplayName(lstSourceCanBoQuanLy, ref cmbCanBoQuanLy);
                if (aceCanBoQLy != null)
                {
                    obj.ID_CBO_QLY = Convert.ToInt32(aceCanBoQLy.KeywordStrings.ElementAt(1));
                    obj.MA_CBO_QLY = aceCanBoQLy.KeywordStrings.ElementAt(0);
                }

                if (ID == 0)
                    danhmucProcess.ThemNhom(obj);
                else
                    danhmucProcess.SuaNhom(obj);
                HoanThanh();
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
        /// Xử lý xóa dữ liệu
        /// </summary>
        private void Xoa()
        {
            if (LMessage.ShowMessage("M.DiaBan.HoiXoa", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
            {
                DanhMucProcess danhmucProcess = new DanhMucProcess();
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    List<int> lstID = new List<int>();
                    lstID.Add((int)LstChiTiet[0][1]);
                    List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();

                    if (danhmucProcess.XoaNhom(lstID.ToArray(), ref listResponseDetail))
                    {
                        LMessage.ShowMessage("M.DanhMuc.ucDonViDS.XoaThanhCong", LMessage.MessageBoxType.Information);
                        HoanThanh();
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
        }

        /// <summary>
        /// Xử lý duyệt
        /// </summary>
        private void Duyet()
        {
            DanhMucProcess danhmucProcess = new DanhMucProcess();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<int> lstID = new List<int>();
                lstID.Add((int)LstChiTiet[0][1]);
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();

                if (danhmucProcess.DuyetNhom(lstID.ToArray(), ref listResponseDetail))
                {
                    CustomControl.CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.DUYET, LstChiTiet[0][1].ToString(), mnuMain, DatabaseConstant.Function.DC_DM_NHOM);
                    LMessage.ShowMessage("M.DiaBan.DuyetThanhCong", LMessage.MessageBoxType.Information);
                    HoanThanh();
                }
                else
                {
                    LMessage.ShowMessage("M.DiaBan.DuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
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
                    new frmThongBaoLoi("M.DiaBan.LoiDuyet", ex).ShowDialog();
                }
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void ThoaiDuyet()
        {
            DanhMucProcess danhmucProcess = new DanhMucProcess();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<int> lstID = new List<int>();
                lstID.Add((int)LstChiTiet[0][1]);
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();

                if (danhmucProcess.ThoaiDuyetNhom(lstID.ToArray(), ref listResponseDetail))
                {
                    CustomControl.CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THOAI_DUYET, LstChiTiet[0][1].ToString(), mnuMain, DatabaseConstant.Function.DC_DM_NHOM);
                    LMessage.ShowMessage("M.DiaBan.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);
                    HoanThanh();
                }
                else
                {
                    LMessage.ShowMessage("M.DiaBan.ThoaiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
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
                    new frmThongBaoLoi("M.DiaBan.LoiThoaiDuyet", ex).ShowDialog();
                }
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void TuChoi()
        {
            DanhMucProcess danhmucProcess = new DanhMucProcess();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<int> lstID = new List<int>();
                lstID.Add((int)LstChiTiet[0][1]);
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();

                if (danhmucProcess.TuChoiNhom(lstID.ToArray(), ref listResponseDetail))
                {
                    CustomControl.CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.TU_CHOI_DUYET, LstChiTiet[0][1].ToString(), mnuMain, DatabaseConstant.Function.DC_DM_NHOM);
                    LMessage.ShowMessage("M.DiaBan.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);
                    HoanThanh();
                }
                else
                {
                    LMessage.ShowMessage("M.DiaBan.TuChoiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
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
                    new frmThongBaoLoi("M.DiaBan.LoiTuChoiDuyet", ex).ShowDialog();
                }
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void HoanThanh()
        {
            if (ID == 0 && cbMultiAdd.IsChecked.Value)
            {
                txtMaNhom.Text = string.Empty;
                txtTenNhom.Text = string.Empty;
                txtTenTat.Text = string.Empty;
                raddtNgayNhap.Value = null;
                raddtNgayCapNhat.Value = null;
                raddtNgayTLap.Text = string.Empty;
                txtNguoiNhap.Text = string.Empty;
                txtTrangThaiBanGhi.Text = string.Empty;
                txtMaNhom.Focus();
            }
            else
                CustomControl.CommonFunction.CloseUserControl(this);
        }

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
                    cmbLoaiNhom.IsEnabled = false;
                    //Sự kiện load dữ liệu
                    //DM_CUM obj = null;
                    ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    string responseMessage = null;

                    ret = danhmucProcess.getNhomById(id, ref obj, ref responseMessage);
                    if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
                    {
                        TthaiNvu = obj.TTHAI_NVU;
                        maCum = obj.MA_CUM;

                        lblTrangThai.Content = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);

                        // khởi tạo combobox
                        AutoComboBox auto = new AutoComboBox();
                        // Khởi tạo điều kiện gọi danh mục
                        List<string> lstDieuKien = new List<string>();
                        // khởi tạo combobox
                        auto = new AutoComboBox();
                        lstDieuKien = new List<string>();
                        lstSourceChiNhanh.Clear();
                        lstDieuKien.Add("'" + DatabaseConstant.ToChucDonVi.HSO.getValue() + "', '" + DatabaseConstant.ToChucDonVi.CNH.getValue() + "'");
                        auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOLOAI.getValue(), lstDieuKien, obj.MA_DVI_QLY);

                        // khởi tạo combobox
                        auto = new AutoComboBox();
                        lstDieuKien = new List<string>();
                        lstSourcePhongGD.Clear();
                        lstDieuKien.Add(obj.MA_DVI_QLY);
                        auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOMACHA.getValue(), lstDieuKien, obj.MA_DVI);

                        // khởi tạo combobox
                        auto = new AutoComboBox();
                        lstDieuKien = new List<string>();
                        lstSourceKhuVuc.Clear();
                        lstDieuKien.Add(obj.ID_DVI.ToString());
                        auto.GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUC.getValue(), lstDieuKien, obj.MA_KVUC);

                        // khởi tạo combobox
                        auto = new AutoComboBox();
                        lstDieuKien = new List<string>();
                        lstSourceCum.Clear();
                        lstDieuKien.Add(obj.ID_DVI.ToString());
                        lstDieuKien.Add(obj.ID_KVUC.ToString());
                        auto.GenAutoComboBox(ref lstSourceCum, ref cmbCum, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM.getValue(), lstDieuKien, obj.MA_CUM);

                        txtTenNhom.Text = obj.TEN_NHOM;
                        txtMaNhom.Text = obj.MA_NHOM;
                        txtTenTat.Text = obj.TEN_TAT;

                        if (!obj.MA_LOAI_NHOM.IsNullOrEmptyOrSpace())
                            cmbLoaiNhom.SelectedIndex = lstSourceLoaiNhom.IndexOf(lstSourceLoaiNhom.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault() == obj.MA_LOAI_NHOM));

                        if (obj.MA_LOAI_NHOM.Equals("NHOM_THOI_VU"))
                        {
                            auto = new AutoComboBox();
                            lstDieuKien = new List<string>();
                            lstDieuKien.Add(obj.ID_DVI.ToString());
                            lstDieuKien.Add(obj.ID_KVUC.ToString());
                            lstDieuKien.Add(obj.ID_CUM.ToString());
                            lstSourceNhom.Clear();
                            auto.GenAutoComboBox(ref lstSourceNhom, ref cmbNhomCha, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NHOM.getValue(), lstDieuKien, obj.MA_NHOM_CHA);                           
                        }
                        
                        
                        if (!string.IsNullOrEmpty(obj.NGAY_TLAP))
                        {
                            raddtNgayTLap.Value = LDateTime.StringToDate(obj.NGAY_TLAP, "yyyyMMdd");
                        }

                        if (!string.IsNullOrEmpty(obj.NGAY_HET_HLUC))
                        {
                            raddtNgayHetHLuc.Value = LDateTime.StringToDate(obj.NGAY_HET_HLUC, "yyyyMMdd");
                        }

                        // khởi tạo combobox
                        auto = new AutoComboBox();
                        lstDieuKien = new List<string>();
                        lstSourceCanBoQuanLy.Clear();
                        lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                        auto.GenAutoComboBox(ref lstSourceCanBoQuanLy, ref cmbCanBoQuanLy, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NSD.getValue(), lstDieuKien, obj.MA_CBO_QLY);

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
                        // Set thông tin khach hang nhom
                        DataSet ds = new DanhMucProcess().getThongTinCTietNhom(id.ToString());
                        if (!LObject.IsNullOrEmpty(ds) && ds.Tables.Count > 0)
                        {
                            dtkhachHang = ds.Tables["KHACH_HANG"].Copy();
                            //raddgrKhachHangTVien.ItemsSource = null;
                            raddgrKhachHangTVien.ItemsSource = dtkhachHang.DefaultView;
                        }
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
        private void GetFormData(ref DM_NHOM obj)
        {
            if (id != 0)
            {
                obj.ID = id;
                obj.MA_NHOM = txtMaNhom.Text;
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
            AutoCompleteEntry aceCum = new AutoComboBox().getEntryByDisplayName(lstSourceCum, ref cmbCum);
            AutoCompleteEntry aceCanBoQLy = new AutoComboBox().getEntryByDisplayName(lstSourceCanBoQuanLy, ref cmbCanBoQuanLy);
            AutoCompleteEntry aceNhomCha = new AutoComboBox().getEntryByDisplayName(lstSourceNhom, ref cmbNhomCha);

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
            if (aceCum != null)
            {
                obj.ID_CUM = Convert.ToInt32(aceCum.KeywordStrings.ElementAt(1));
                obj.MA_CUM = aceCum.KeywordStrings.ElementAt(0);
            }
                        
            if (aceCanBoQLy != null)
            {
                obj.ID_CBO_QLY = Convert.ToInt32(aceCanBoQLy.KeywordStrings.ElementAt(1));
                obj.MA_CBO_QLY = aceCanBoQLy.KeywordStrings.ElementAt(0);
            }

            obj.TEN_NHOM = txtTenNhom.Text;
            obj.TEN_TAT = txtTenTat.Text;
            obj.MA_LOAI_NHOM = lstSourceLoaiNhom.ElementAt(cmbLoaiNhom.SelectedIndex).KeywordStrings.FirstOrDefault();

            if (aceNhomCha != null && obj.MA_LOAI_NHOM.Equals("NHOM_THOI_VU"))
            {
                obj.ID_NHOM_CHA = Convert.ToInt32(aceNhomCha.KeywordStrings.ElementAt(1));
                obj.MA_NHOM_CHA = aceNhomCha.KeywordStrings.ElementAt(0);
            }

            if (raddtNgayTLap.Value is DateTime)
                obj.NGAY_TLAP = ((DateTime)raddtNgayTLap.Value).ToString("yyyyMMdd");
            if (raddtNgayHetHLuc.Value is DateTime)
                obj.NGAY_HET_HLUC = ((DateTime)raddtNgayHetHLuc.Value).ToString("yyyyMMdd");

            obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
            //obj.TTHAI_NVU = TthaiNvu;
            obj.MA_DVI_TAO = acePhongGD.KeywordStrings.ElementAt(0);
            obj.MA_DVI_QLY = aceChiNhanh.KeywordStrings.ElementAt(0); ;

            obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
            obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;

            // Get thông tin lịch họp
            lstKHangNhom = new List<KH_KHANG_NHOM>();
            dtkhachHang = (raddgrKhachHangTVien.ItemsSource as DataView).Table;
            foreach (DataRow dr in dtkhachHang.Rows)
            {
                if (dr["TTHAI_BGHI"].ToString() == "T")
                {
                    KH_KHANG_NHOM objKHNhom = new KH_KHANG_NHOM();
                    objKHNhom.ID_KHANG = Convert.ToInt32(dr["ID_KHANG"]);
                    objKHNhom.ID_NHOM = id;
                    objKHNhom.MA_DVI_QLY = obj.MA_DVI_QLY;
                    objKHNhom.MA_DVI_TAO = obj.MA_DVI_TAO;
                    objKHNhom.MA_KHANG = dr["MA_KHANG"].ToString();
                    objKHNhom.MA_LOAI_NHOM = obj.MA_LOAI_NHOM;
                    objKHNhom.MA_NHOM = obj.MA_NHOM;
                    objKHNhom.NGAY_CNHAT = obj.NGAY_CNHAT;
                    objKHNhom.NGAY_NHAP = obj.NGAY_NHAP;
                    objKHNhom.NGUOI_CNHAT = obj.NGUOI_CNHAT;
                    objKHNhom.NGUOI_NHAP = obj.NGUOI_NHAP;
                    objKHNhom.TTHAI_BGHI = obj.TTHAI_BGHI;
                    objKHNhom.TTHAI_NVU = obj.TTHAI_NVU;
                    lstKHangNhom.Add(objKHNhom);
                }
                if (dr["TRUONG_NHOM"].ToString().ToLower() == "true")
                {
                    obj.ID_NHOM_TRUONG = Convert.ToInt32(dr["ID_KHANG"]);
                    obj.MA_NHOM_TRUONG = dr["MA_KHANG"].ToString();
                }
            }
            return;
        }

        /// <summary>
        /// Kiểm tra các thông tin nhập vào trước khi lưu dữ liệu
        /// </summary>
        /// <returns></returns>
        private bool Validation()
        {
            if (cmbChiNhanh.SelectedIndex < 0)
            {                
                CommonFunction.ThongBaoChuaChon(lblChiNhanh.Content.ToString());
                cmbChiNhanh.Focus();
                return false;
            }

            if (cmbPhongGD.SelectedIndex < 0)
            {
                CommonFunction.ThongBaoChuaChon(lblPhongGiaoDich.Content.ToString());
                cmbPhongGD.Focus();
                return false;
            }

            if (cmbKhuVuc.SelectedIndex < 0)
            {
                CommonFunction.ThongBaoChuaChon(lblKhuVuc.Content.ToString());
                cmbKhuVuc.Focus();
                return false;
            }

            if (cmbCum.SelectedIndex < 0)
            {
                CommonFunction.ThongBaoChuaChon(lblCum.Content.ToString());
                cmbCum.Focus();
                return false;
            }

            if (cmbLoaiNhom.SelectedIndex < 0)
            {
                CommonFunction.ThongBaoChuaChon(lblLoaiNhom.Content.ToString());
                cmbLoaiNhom.Focus();
                return false;
            }

            if (cmbNhomCha.Visibility == System.Windows.Visibility.Visible && cmbNhomCha.SelectedIndex < 0)
            {
                CommonFunction.ThongBaoChuaChon(lblMaNhomCha.Content.ToString());
                cmbNhomCha.Focus();
                return false;
            }

            if (txtTenNhom.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaNhap(lblTenNhom.Content.ToString());
                txtTenNhom.Focus();
                return false;
            }

            if (txtTenTat.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaNhap(lblTenTat.Content.ToString());
                txtTenTat.Focus();
                return false;
            }

            if (raddtNgayTLap.Value > raddtNgayHetHLuc.Value)
            {
                CommonFunction.ThongBaoLoi(lblNgayHetHLuc.Content.ToString());
                raddtNgayHetHLuc.Focus();
                return false;
            }

            if (cmbCanBoQuanLy.SelectedIndex < 0)
            {
                CommonFunction.ThongBaoChuaChon(lblCanBoQuanLy.Content.ToString());
                cmbCanBoQuanLy.Focus();
                return false;
            }

            string maLoaiNhom = lstSourceLoaiNhom.ElementAt(cmbLoaiNhom.SelectedIndex).KeywordStrings.FirstOrDefault();
            int soLuong = raddgrKhachHangTVien.Items.Count;
            if (maLoaiNhom.Equals("NHOM_THOI_VU"))
            {
                string toithieu = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_KHANG_TVIEN_SNGUOI_TTHIEU_NHOM_MVU,ClientInformation.MaDonVi);
                string toiDa = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_KHANG_TVIEN_SNGUOI_TDA_NHOM_MVU, ClientInformation.MaDonVi);
                
                int min = 0;
                int max = 100;
                if (toithieu.IsNumeric())
                    min = Convert.ToInt32(toithieu);

                if (toiDa.IsNumeric())
                    max = Convert.ToInt32(toiDa);

                if (soLuong < min)
                {
                    MessageBoxResult result = LMessage.ShowMessage("M_ResponseMessage_Nhom_SoLuongKhachHangNhoHonQuyDinh", LMessage.MessageBoxType.Question);
                    if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
                    {
                        tlbAddKHang.Focus();
                        return false;
                    }

                }

                if (soLuong > max)
                {
                    MessageBoxResult result = LMessage.ShowMessage("M_ResponseMessage_Nhom_SoLuongKhachHangLonHonQuyDinh", LMessage.MessageBoxType.Question);
                    if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
                    {
                        tlbDelKHang.Focus();
                        return false;
                    }

                }
                
            }
            else if (maLoaiNhom.Equals("NHOM_TRA_DAN"))
            {
                string toithieu = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_KHANG_TVIEN_SNGUOI_TTHIEU_NHOM_TDAN, ClientInformation.MaDonVi);
                string toiDa = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_KHANG_TVIEN_SNGUOI_TDA_NHOM_TDAN, ClientInformation.MaDonVi);

                int min = 0;
                int max = 100;
                if (toithieu.IsNumeric())
                    min = Convert.ToInt32(toithieu);

                if (toiDa.IsNumeric())
                    max = Convert.ToInt32(toiDa);

                if (soLuong < min)
                {
                    MessageBoxResult result = LMessage.ShowMessage("M_ResponseMessage_Nhom_SoLuongKhachHangNhoHonQuyDinh", LMessage.MessageBoxType.Question);
                    if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
                    {
                        tlbAddKHang.Focus();
                        return false;
                    }

                }

                if (soLuong > max)
                {
                    MessageBoxResult result = LMessage.ShowMessage("M_ResponseMessage_Nhom_SoLuongKhachHangLonHonQuyDinh", LMessage.MessageBoxType.Question);
                    if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
                    {
                        tlbDelKHang.Focus();
                        return false;
                    }

                }
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
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_NHOM);
        }

        /// <summary>
        /// Trước khi xem từ danh sách
        /// </summary>
        public void beforeViewFromList()
        {
            SetFormData();
            SetEnabledAllControls(false);
            SetEnabledRequiredControls(false);
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_NHOM);
        }

        /// <summary>
        /// Trước khi thêm mới
        /// </summary>
        private void beforeAddNew()
        {
            ResetForm();
            SetEnabledAllControls(true);
            SetEnabledRequiredControls(false);

            cmbPhongGD.IsEnabled = true;
            cmbKhuVuc.IsEnabled = true;
            cmbCum.IsEnabled = true;

            action = DatabaseConstant.Action.THEM;
            CommonFunction.RefreshButton(Toolbar, action, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_NHOM);
        }

        /// <summary>
        /// Trước khi sửa từ danh sách
        /// </summary>
        public void beforeModifyFromList()
        {
            SetFormData();
            SetEnabledAllControls(true);
            SetEnabledRequiredControls(false);

            cmbPhongGD.IsEnabled = true;
            cmbKhuVuc.IsEnabled = true;
            cmbCum.IsEnabled = true;

            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_NHOM);
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
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_NHOM);
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
                    DatabaseConstant.Function.DC_DM_NHOM,
                    DatabaseConstant.Table.DM_NHOM,
                    DatabaseConstant.Action.SUA,
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
                    //DM_CUM obj = new DM_CUM();
                    //DC_TSUAT_CUM objTanSuatCum = new DC_TSUAT_CUM();
                    string responseMessage = null;
                    List<string> responseMessageData = new List<string>();

                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (id == 0)
                    {
                        // Lấy dữ liệu từ form
                        GetFormData(ref obj);
                        ret = danhmucProcess.ThemNhom(ref obj, ref responseMessage, ref responseMessageData, lstKHangNhom);
                        afterAddNew(ret, obj, responseMessage, responseMessageData);
                    }
                    // Nếu là lưu tạm khi sửa
                    // Hoặc lưu tạm khi sửa sau duyệt
                    // Hoặc sửa
                    else
                    {
                        // Lấy thông tin cũ
                        //obj = danhmucProcess.getCumById(id);

                        // Lấy dữ liệu từ form
                        GetFormData(ref obj);
                        ret = danhmucProcess.SuaNhom(ref obj, ref responseMessage,lstKHangNhom,lstIDXoa);
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
                    DM_NHOM obj = new DM_NHOM();
                    DM_NHOM ret = null;

                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (id == 0)
                    {
                        // Lấy dữ liệu từ form
                        GetFormData(ref obj);
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
                        //obj = danhmucProcess.getCumById(id);

                        // Lấy dữ liệu từ form
                        GetFormData(ref obj);
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
                bool ret = danhmucProcess.XoaNhom(arrayID, ref listClientResponseDetail);

                afterDelete(ret, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu khi xảy ra lỗi
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_NHOM,
                    DatabaseConstant.Table.DM_NHOM,
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
                bool ret = danhmucProcess.DuyetNhom(arrayID, ref listClientResponseDetail);

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
                bool ret = danhmucProcess.ThoaiDuyetNhom(arrayID, ref listClientResponseDetail);

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
                bool ret = danhmucProcess.TuChoiNhom(arrayID, ref listClientResponseDetail);

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
        private void afterAddNew(ApplicationConstant.ResponseStatus ret, DM_NHOM obj, string responseMessage)
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

                    txtMaNhom.Text = obj.MA_NHOM;
                    lblTrangThai.Content = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);

                    SetEnabledAllControls(false);
                    SetEnabledRequiredControls(false);
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_NHOM);

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

        private void afterAddNew(ApplicationConstant.ResponseStatus ret, DM_NHOM obj, string responseMessage, List<string> responseMessageData)
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

                    txtMaNhom.Text = obj.MA_NHOM;
                    lblTrangThai.Content = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);

                    SetEnabledAllControls(false);
                    SetEnabledRequiredControls(false);
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.DC_DM_NHOM);

                    tbiThongTinChung.Focus();
                }
                else
                {
                    onClose();
                }
            }
            else
            {
                LMessage.ShowMessage(responseMessage, responseMessageData.ToArray(), LMessage.MessageBoxType.Error);
            }
        }

        /// <summary>
        /// Sau khi sửa
        /// </summary>
        /// <param name="ret"></param>
        private void afterModify(ApplicationConstant.ResponseStatus ret, DM_NHOM obj, string responseMessage)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                id = obj.ID;
                TthaiNvu = obj.TTHAI_NVU;
                lblTrangThai.Content = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);

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
                DatabaseConstant.Function.DC_DM_NHOM,
                DatabaseConstant.Table.DM_NHOM,
                DatabaseConstant.Action.XOA,
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
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);
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
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);
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
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);
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

    }
}
