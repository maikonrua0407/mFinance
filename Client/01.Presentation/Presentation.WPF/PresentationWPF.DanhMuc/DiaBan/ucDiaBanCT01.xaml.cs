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
using PresentationWPF.CustomControl;
using Presentation.Process.PopupServiceRef;
using System.Data;
using Presentation.Process.DanhMucServiceRef;
using System.Collections;
using System.Reflection;


namespace PresentationWPF.DanhMuc.DiaBan
{
    /// <summary>
    /// Interaction logic for ucDiaBanCT.xaml
    /// </summary>
    public partial class ucDiaBanCT01 : UserControl
    {
        #region Khai bao

        private bool chiXem;
        public bool ChiXem
        {
            get { return chiXem; }
            set { chiXem = value; }
        }

        List<DataRow> lstChiTiet;
        public List<DataRow> LstChiTiet
        {
            get { return lstChiTiet; }
            set { lstChiTiet = value; }
        }

        private string trangThaiNV;
        public string TrangThaiNV
        {
            get { return trangThaiNV; }
            set { trangThaiNV = value; }
        }

        private string maTrangThai;
        public string MaTrangThai
        {
            get { return maTrangThai; }
            set { maTrangThai = value; }
        }

        private bool isLoaded = false;

        private string formCase = string.Empty;
        public string FormCase
        {
            get { return formCase; }
            set { formCase = value; }
        }

        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private int id = 0;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        List<AutoCompleteEntry> lstSourceLoaiKVuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiDiaBan = new List<AutoCompleteEntry>();

        List<AutoCompleteEntry> lstSourceQuanHuyen = new List<AutoCompleteEntry>();

        private string maDiaBanQL;

        public string MaDiaBanQL
        {
            get { return maDiaBanQL; }
            set { maDiaBanQL = value; }
        }

        List<AutoCompleteEntry> lstSourceTinhTP = new List<AutoCompleteEntry>();

        private string maDiaBan;

        public string MaDiaBan
        {
            get { return maDiaBan; }
            set { maDiaBan = value; }
        }

        private string tthaiNvu = "";
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
        #endregion

        #region Khoi tao

        public ucDiaBanCT01()
        {
            InitializeComponent();

            LoadCombobox();

            InitEventHandler();

            ResetForm();
        }

        /// <summary>
        /// Khởi tạo các combobox
        /// </summary>
        private void LoadCombobox()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            // Khởi tạo điều kiện gọi danh mục
            List<string> lstDieuKien = new List<string>();

            // khởi tạo combobox
            auto = new AutoComboBox();
            cmbLoaiDB.Items.Clear();
            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.LOAI_DIA_BAN.getValue());
            auto.GenAutoComboBox(ref lstSourceLoaiDiaBan, ref cmbLoaiDB, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
            auto.removeEntry(ref lstSourceLoaiDiaBan, ref cmbLoaiDB, DatabaseConstant.DanhSachLoaiDiaBan.TINH_THANHPHO.getValue());
            auto.removeEntry(ref lstSourceLoaiDiaBan, ref cmbLoaiDB, DatabaseConstant.DanhSachLoaiDiaBan.LANG_TODP.getValue());

            // khởi tạo combobox
            auto = new AutoComboBox();
            cmbTinhTP.Items.Clear();
            lstDieuKien.Clear();
            auto.GenAutoComboBox(ref lstSourceTinhTP, ref cmbTinhTP, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TINHTP.getValue(), lstDieuKien);

            if (lstSourceTinhTP.Select(i => i.DisplayName).Contains(cmbTinhTP.Text))
            {
                string maTinhTP = lstSourceTinhTP.ElementAt(cmbTinhTP.SelectedIndex).KeywordStrings.First();
                string idTinhTP = lstSourceTinhTP.ElementAt(cmbTinhTP.SelectedIndex).KeywordStrings.ElementAt(1);

                // khởi tạo combobox
                auto = new AutoComboBox();
                cmbQuanHuyen.Items.Clear();
                lstDieuKien.Clear();
                lstDieuKien.Add(idTinhTP.ToString());
                lstDieuKien.Add(null);
                lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBan.HUYEN_QUAN.getValue());
                auto.GenAutoComboBox(ref lstSourceQuanHuyen, ref cmbQuanHuyen, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DIABAN.getValue(), lstDieuKien);
            }

            lstSourceLoaiKVuc = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstSourceLoaiKVuc, ref cmbLoaiKVuc, "COMBOBOX_LOAI_KHU_VUC", null);
            cmbLoaiKVuc.SelectedIndex = 0;
        }

        /// <summary>
        /// InitEventHandler
        /// </summary>
        private void InitEventHandler()
        {
            tabDiaBanCT.SelectionChanged -= tabDiaBanCT_SelectionChanged;
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.DanhMuc;component/DiaBan/ucDiaBanCT01.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }

            BindShortkey();
            cmbQuanHuyen.SelectionChanged += cmbQuanHuyen_SelectionChanged; 
            cmbQuanHuyen.KeyDown += cmbQuanHuyen_KeyDown;
            cmbTinhTP.SelectionChanged += cmbTinhTP_SelectionChanged;
            cmbTinhTP.KeyDown += cmbTinhTP_KeyDown;
            cmbLoaiDB.SelectionChanged += cmbLoaiDB_SelectionChanged;
            cmbLoaiDB.KeyDown += cmbLoaiDB_KeyDown;            

        }

        /// <summary>
        /// Đưa form về trạng thái mặc định
        /// </summary>
        private void ResetForm()
        {
            id = 0;
            tthaiNvu = "";

            txtMa.Text = "";
            txtTen.Text = "";
            txtTenTat.Text = "";
            raddtNgayNhap.Value = LDateTime.GetCurrentDate();
            raddtNgayCNhat.Value = null;
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            txtNguoiCapNhat.Text = "";
            txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(BusinessConstant.TrangThaiBanGhi.SU_DUNG.ToString());
            lblTrangThai.Content = "";

            spnQuanHuyen.Visibility = Visibility.Hidden;
            cmbQuanHuyen.Visibility = Visibility.Hidden;

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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(ucDiaBanCT.SaveCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.H, ModifierKeys.Control);
                        key = new KeyBinding(ucDiaBanCT.HoldCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(ucDiaBanCT.ModifyCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.Shift);
                        key = new KeyBinding(ucDiaBanCT.DeleteCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.A, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucDiaBanCT.ApproveCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucDiaBanCT.RefuseCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.C, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucDiaBanCT.CancelCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(ucDiaBanCT.HelpCommand, keyg);
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
            beforeModify();
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
            //e.CanExecute = tlbApprove.IsEnabled;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onApprove();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute = tlbRefuse.IsEnabled;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onRefuse();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute = tlbCancel.IsEnabled;
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
                beforeAddNew();
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
                beforeModify();
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
                beforeAddNew();
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
                beforeModify();
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

        #region Xu ly Giao dien

        /// <summary>
        /// SetEnabledAllControls
        /// </summary>
        /// <param name="enable"></param>
        private void SetEnabledAllControls(bool enable)
        {
            cmbTinhTP.IsEnabled = enable;
            cmbLoaiDB.IsEnabled = enable;
            cmbQuanHuyen.IsEnabled = enable;

            txtMa.IsEnabled = enable;
            txtTen.IsEnabled = enable;
            txtTenTat.IsEnabled = enable;
            
            txtTrangThai.IsEnabled = enable;
            txtNguoiLap.IsEnabled = enable;
            raddtNgayNhap.IsEnabled = enable;
            txtNguoiCapNhat.IsEnabled = enable;
            raddtNgayCNhat.IsEnabled = enable;
        }

        /// <summary>
        /// SetEnabledRequiredControls
        /// </summary>
        /// <param name="enable"></param>
        private void SetEnabledRequiredControls(bool enable)
        {
            cmbTinhTP.IsEnabled = enable;
            cmbLoaiDB.IsEnabled = enable;
            cmbQuanHuyen.IsEnabled = enable;

            txtMa.IsEnabled = enable;

            txtTrangThai.IsEnabled = enable;
            txtNguoiLap.IsEnabled = enable;
            raddtNgayNhap.IsEnabled = enable;
            txtNguoiCapNhat.IsEnabled = enable;
            raddtNgayCNhat.IsEnabled = enable;
        }

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
                    arr = hethong.SetVisibleControl("PresentationWPF.DanhMuc.DiaBan.ucDiaBanCT", formCase);
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //if (!isLoaded)
            //{
            //    SetFormData();
            //    HideControl();
            //    isLoaded = true;
            //}
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
        /// Sự kiện nhấn key trên form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        private void tabDiaBanCT_SelectionChanged(object sender, Telerik.Windows.Controls.RadSelectionChangedEventArgs e)
        {
            if (tabDiaBanCT.SelectedIndex == 0)
            {
                if (cmbQuanHuyen != null)
                {
                    UpdateLayout();
                    cmbQuanHuyen.Focus();
                }
            }
            else if (tabDiaBanCT.SelectedIndex == 1)
            {
                if (txtTrangThai != null)
                {
                    UpdateLayout();
                    txtTrangThai.Focus();
                }
            }
        }

        public void setMaDiaBan(string maChon)
        {
            cmbQuanHuyen.SelectedIndex = lstSourceQuanHuyen.IndexOf(lstSourceQuanHuyen.FirstOrDefault(i => i.KeywordStrings.First().Equals(maChon)));
        }

        private void cmbQuanHuyen_KeyDown(object sender, KeyEventArgs e)
        {
            cmbQuanHuyen.IsDropDownOpen = true;
        }

        private void cmbQuanHuyen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbTinhTP_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                if (lstSourceTinhTP.Select(i => i.DisplayName).Contains(cmbTinhTP.Text))
                {
                    string maTinhTP = lstSourceTinhTP.ElementAt(cmbTinhTP.SelectedIndex).KeywordStrings.First();
                    string idTinhTP = lstSourceTinhTP.ElementAt(cmbTinhTP.SelectedIndex).KeywordStrings.ElementAt(1);

                    // khởi tạo combobox
                    AutoComboBox auto = new AutoComboBox();
                    List<String> lstDieuKien = new List<String>();
                    lstSourceQuanHuyen = new List<AutoCompleteEntry>();
                    cmbQuanHuyen.Items.Clear();
                    lstDieuKien.Clear();
                    lstDieuKien.Add(idTinhTP.ToString());
                    lstDieuKien.Add(null);
                    lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBan.HUYEN_QUAN.getValue());
                    auto.GenAutoComboBox(ref lstSourceQuanHuyen, ref cmbQuanHuyen, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DIABAN.getValue(), lstDieuKien);
                }
                else
                {

                }
            }
            else
            {

            }
        }

        private void cmbTinhTP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTinhTP.SelectedIndex >= 0)
            {
                if (lstSourceTinhTP.Select(i => i.DisplayName).Contains(cmbTinhTP.Text))
                {
                    string maTinhTP = lstSourceTinhTP.ElementAt(cmbTinhTP.SelectedIndex).KeywordStrings.First();
                    string idTinhTP = lstSourceTinhTP.ElementAt(cmbTinhTP.SelectedIndex).KeywordStrings.ElementAt(1);

                    // khởi tạo combobox
                    AutoComboBox auto = new AutoComboBox();
                    List<String> lstDieuKien = new List<String>();
                    lstSourceQuanHuyen = new List<AutoCompleteEntry>();
                    cmbQuanHuyen.Items.Clear();
                    lstDieuKien.Clear();
                    lstDieuKien.Add(idTinhTP.ToString());
                    lstDieuKien.Add(null);
                    lstDieuKien.Add(DatabaseConstant.DanhSachLoaiDiaBan.HUYEN_QUAN.getValue());
                    auto.GenAutoComboBox(ref lstSourceQuanHuyen, ref cmbQuanHuyen, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DIABAN.getValue(), lstDieuKien);
                }
                else
                {

                }
            }
            else
            {

            }
        }

        private void cmbLoaiDB_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                if (lstSourceLoaiDiaBan.Select(i => i.DisplayName).Contains(cmbLoaiDB.Text))
                {
                    string loaiDiaBan = lstSourceLoaiDiaBan.ElementAt(cmbLoaiDB.SelectedIndex).KeywordStrings.First();

                    if (loaiDiaBan.Equals(DatabaseConstant.DanhSachLoaiDiaBan.HUYEN_QUAN.getValue()))
                    {
                        spnQuanHuyen.Visibility = Visibility.Hidden;
                        cmbQuanHuyen.Visibility = Visibility.Hidden;
                    }
                    else if (loaiDiaBan.Equals(DatabaseConstant.DanhSachLoaiDiaBan.XA_PHUONG.getValue()))
                    {
                        spnQuanHuyen.Visibility = Visibility.Visible;
                        cmbQuanHuyen.Visibility = Visibility.Visible;
                    }
                }
                else
                {

                }
            }
            else
            {

            }
        }

        private void cmbLoaiDB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbLoaiDB.SelectedIndex >= 0)
            {
                if (lstSourceLoaiDiaBan.Select(i => i.DisplayName).Contains(cmbLoaiDB.Text))
                {
                    string loaiDiaBan = lstSourceLoaiDiaBan.ElementAt(cmbLoaiDB.SelectedIndex).KeywordStrings.First();

                    if (loaiDiaBan.Equals(DatabaseConstant.DanhSachLoaiDiaBan.HUYEN_QUAN.getValue()))
                    {
                        spnQuanHuyen.Visibility = Visibility.Hidden;
                        cmbQuanHuyen.Visibility = Visibility.Hidden;
                    }
                    else if (loaiDiaBan.Equals(DatabaseConstant.DanhSachLoaiDiaBan.XA_PHUONG.getValue()))
                    {
                        spnQuanHuyen.Visibility = Visibility.Visible;
                        cmbQuanHuyen.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    
                }
            }
            else
            {

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
                DatabaseConstant.Function.DC_DM_DIA_BAN,
                DatabaseConstant.Table.DM_DIA_BAN,
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
            CustomControl.CommonFunction.CloseUserControl(this);
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
                if (Id != 0)
                {
                    DanhMucProcess proccess = new DanhMucProcess();
                    DMDiaBanResponse response = proccess.getDiaBanById01(id);
                    DM_DIA_BAN obj = response.obj;
                    VDM_DBAN_TTIN_KHAC objTTKhac = response.objTTKhac;
                    txtMa.Text = obj.MA_DBAN;
                    txtTen.Text = obj.TEN_DBAN;
                    txtTenTat.Text = obj.TEN_TAT;
                    cmbTinhTP.SelectedIndex = lstSourceTinhTP.IndexOf(lstSourceTinhTP.FirstOrDefault(e => e.KeywordStrings.ElementAt(1).Equals(obj.ID_TINHTP.ToString())));
                    if (!obj.ID_DBAN_CHA.IsNullOrEmpty())
                        cmbQuanHuyen.SelectedIndex = lstSourceQuanHuyen.IndexOf(lstSourceQuanHuyen.FirstOrDefault(e => e.KeywordStrings.ElementAt(1).Equals(obj.ID_DBAN_CHA.ToString())));
                    if (!obj.LOAI_DBAN.IsNullOrEmptyOrSpace())
                        cmbLoaiDB.SelectedIndex = lstSourceLoaiDiaBan.IndexOf(lstSourceLoaiDiaBan.FirstOrDefault(e => e.KeywordStrings.First().Equals(obj.LOAI_DBAN)));
                    lblTrangThai.Content = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    raddtNgayNhap.Value = LDateTime.StringToDate(obj.NGAY_NHAP, "yyyyMMdd");
                    if (!obj.NGAY_CNHAT.IsNullOrEmptyOrSpace())
                        raddtNgayCNhat.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, "yyyyMMdd");
                    txtNguoiLap.Text = obj.NGUOI_NHAP;
                    txtNguoiCapNhat.Text = obj.NGUOI_CNHAT;
                    //Sua(bBool);
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(lblTrangThai.Content.ToString()); 
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
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuGrid, DatabaseConstant.Function.DC_DM_DIA_BAN);
                if (formCase.Equals("MANAGE"))
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu, mnuGrid, DatabaseConstant.Function.DC_DM_DIA_BAN);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DanhMuc.ucDiaBanCT.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Đưa dữ liệu từ form vào object DM_DIA_BAN
        /// </summary>
        private void GetFormData(ref DM_DIA_BAN obj, ref VDM_DBAN_TTIN_KHAC objTTKhac)
        {
            obj.MA_DBAN = txtMa.Text;
            obj.TEN_DBAN = txtTen.Text;
            obj.TEN_TAT = txtTenTat.Text;
            obj.ID_DBAN_CHA = Convert.ToInt32(lstSourceQuanHuyen.ElementAt(cmbQuanHuyen.SelectedIndex).KeywordStrings.ElementAt(1));
            obj.ID_TINHTP = Convert.ToInt32(lstSourceTinhTP.ElementAt(cmbTinhTP.SelectedIndex).KeywordStrings.ElementAt(1));
            obj.LOAI_DBAN = lstSourceLoaiDiaBan.ElementAt(cmbLoaiDB.SelectedIndex).KeywordStrings.First();
            obj.NGAY_NHAP = LDateTime.GetCurrentDate(ApplicationConstant.defaultDateTimeFormat);
            obj.MA_DVI_QLY = ClientInformation.MaDonVi;
            obj.MA_DVI_TAO = ClientInformation.MaDonVi;
            obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
            obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
            obj.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();

            objTTKhac.ID_DBAN = obj.ID;
            objTTKhac.DAN_SO = telnumDanSo.Value.ToString();
            objTTKhac.DIEN_TICH = telnumDienTich.Value.ToString();
            objTTKhac.LOAI = "DIA_BAN";
            objTTKhac.LOAI_KVUC = lstSourceLoaiKVuc.ElementAt(cmbLoaiKVuc.SelectedIndex).KeywordStrings[0];
            objTTKhac.MA_DBAN = obj.MA_DBAN;
            objTTKhac.MA_DVI_TAO = obj.MA_DVI_QLY;
            objTTKhac.NGAY_CNHAT = obj.NGAY_CNHAT;
            objTTKhac.NGAY_DL = obj.NGAY_NHAP;
            objTTKhac.NGAY_NHAP = obj.NGAY_NHAP;
            objTTKhac.NGUOI_CNHAT = obj.NGUOI_CNHAT;
            objTTKhac.NGUOI_NHAP = obj.NGUOI_NHAP;
            objTTKhac.SO_HO_CAN_NGHEO = telnumSoHoCanNgheo.Value.ToString();
            objTTKhac.SO_HO_NGHEO = telnumSoHoNgheo.Value.ToString();
            objTTKhac.TEN_BANG = "DM_DIA_BAN";
            objTTKhac.THUOC_DVI = obj.ID_TINHTP.ToString();
            objTTKhac.THUOC_VUNG = "";
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
            bool result = true;
            try
            {
                if (!lstSourceQuanHuyen.Select(i => i.DisplayName).Contains(cmbQuanHuyen.Text))
                {
                    result = false;
                }
                else
                {
                    MaDiaBan = lstSourceQuanHuyen.ElementAt(cmbQuanHuyen.SelectedIndex).KeywordStrings.First();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return result;
        }

        /// <summary>
        /// Trước khi xem
        /// </summary>
        public void beforeView()
        {
            SetFormData();
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuGrid, DatabaseConstant.Function.DC_DM_DIA_BAN);
        }

        /// <summary>
        /// Trước khi xem từ trang list
        /// </summary>
        public void beforeViewFromList()
        {
            SetFormData();
            formCase = "XEM";
            HideControl();
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuGrid, DatabaseConstant.Function.DC_DM_DIA_BAN);
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
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuGrid, DatabaseConstant.Function.DC_DM_DIA_BAN);
            cmbTinhTP.Focus();
        
        }

        /// <summary>
        /// Trước khi sửa từ chi tiết
        /// </summary>
        private void beforeModify()
        {
            // Yêu cầu lock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(Id);

            bool ret = process.LockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_DIA_BAN,
                DatabaseConstant.Table.DM_DIA_BAN,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Nếu lock thành công >> cho phép sửa
            if (ret)
            {
                formCase = "MANAGE";
                HideControl();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu, mnuGrid, DatabaseConstant.Function.DC_DM_DIA_BAN);
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
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu, mnuGrid, DatabaseConstant.Function.DC_DM_DIA_BAN);
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
                listLockId.Add(Id);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_DIA_BAN,
                    DatabaseConstant.Table.DM_DIA_BAN,
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
                listLockId.Add(Id);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_DIA_BAN,
                    DatabaseConstant.Table.DM_DIA_BAN,
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
                listLockId.Add(Id);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_DIA_BAN,
                    DatabaseConstant.Table.DM_DIA_BAN,
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
                listLockId.Add(Id);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_DIA_BAN,
                    DatabaseConstant.Table.DM_DIA_BAN,
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
            string trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
            if (Validation())
            {
                DanhMucProcess danhmucProcess = new DanhMucProcess();
                try
                {
                    // Dữ liệu truyền vào và dữ liệu trả về
                    DM_DIA_BAN obj = new DM_DIA_BAN();
                    VDM_DBAN_TTIN_KHAC objTTKhac = new VDM_DBAN_TTIN_KHAC();
                    DM_DIA_BAN ret = null;


                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (Id == 0)
                    {
                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref objTTKhac);
                        obj.TTHAI_NVU = trangThai;

                        // Set các thông tin khác                        
                        obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                        obj.NGAY_NHAP = LDateTime.GetCurrentDate(ApplicationConstant.defaultDateTimeFormat);
                        ret = danhmucProcess.ThemDiaBan01(obj, objTTKhac);

                        afterAddNew(ret);
                    }
                    // Nếu là lưu tạm khi sửa
                    // Hoặc lưu tạm khi sửa sau duyệt
                    // Hoặc sửa
                    else
                    {
                        // Lấy thông tin cũ
                        DMDiaBanResponse response = danhmucProcess.getDiaBanById01(Id);
                        obj = response.obj;
                        objTTKhac = response.objTTKhac;

                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref objTTKhac);
                        obj.TTHAI_NVU = trangThai;

                        ret = danhmucProcess.SuaDiaBan01(obj, objTTKhac);
                        afterModify(ret);
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
                    DM_DIA_BAN obj = new DM_DIA_BAN();
                    VDM_DBAN_TTIN_KHAC objTTKhac = new VDM_DBAN_TTIN_KHAC();
                    DM_DIA_BAN ret = null;

                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (Id == 0)
                    {
                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref objTTKhac);
                        obj.TTHAI_NVU = trangThai;

                        // Set các thông tin khác                        
                        obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                        obj.NGAY_NHAP = LDateTime.GetCurrentDate(ApplicationConstant.defaultDateTimeFormat);
                        ret = danhmucProcess.ThemDiaBan01(obj, objTTKhac);

                        afterAddNew(ret);
                    }
                    // Nếu là lưu tạm khi sửa
                    // Hoặc lưu tạm khi sửa sau duyệt
                    // Hoặc sửa
                    else
                    {
                        // Lấy thông tin cũ
                        DMDiaBanResponse response = danhmucProcess.getDiaBanById01(Id);
                        obj = response.obj;
                        objTTKhac = response.objTTKhac;

                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref objTTKhac);
                        obj.TTHAI_NVU = trangThai;

                        ret = danhmucProcess.SuaDiaBan01(obj, objTTKhac);
                        afterModify(ret);
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
                arrayID[arrayID.Length - 1] = Id;

                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = danhmucProcess.XoaDiaBan01(arrayID, ref listClientResponseDetail);

                afterDelete(ret);
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
                arrayID[arrayID.Length - 1] = Id;

                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = danhmucProcess.DuyetDiaBan(arrayID, ref listClientResponseDetail);

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
                arrayID[arrayID.Length - 1] = Id;

                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = danhmucProcess.ThoaiDuyetDiaBan(arrayID, ref listClientResponseDetail);

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
                arrayID[arrayID.Length - 1] = Id;

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
        private void afterAddNew(DM_DIA_BAN ret)
        {
            if (ret != null)
            {
                LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                if (cbMultiAdd.IsChecked == true)
                {
                    beforeAddNew();
                }
                else if (!DatabaseConstant.CLOSE_DETAIL_FORM)
                {
                    Id = ret.ID;
                    tthaiNvu = ret.TTHAI_NVU;
                    txtMa.Text = ret.MA_DBAN;
                    lblTrangThai.Content = BusinessConstant.layNgonNguSuDung(ret.TTHAI_BGHI);
                    txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(ret.TTHAI_NVU);

                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuGrid, DatabaseConstant.Function.DC_DM_DIA_BAN);

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
                LMessage.ShowMessage("M.DungChung.ThemKhongThanhCong", LMessage.MessageBoxType.Error);
            }
        }

        /// <summary>
        /// Sau khi sửa
        /// </summary>
        /// <param name="ret"></param>
        private void afterModify(DM_DIA_BAN ret)
        {
            if (ret != null)
            {
                LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                Id = ret.ID;
                tthaiNvu = ret.TTHAI_NVU;
                txtMa.Text = ret.MA_DBAN;
                lblTrangThai.Content = BusinessConstant.layNgonNguSuDung(ret.TTHAI_BGHI);
                lblTrangThai.Content = BusinessConstant.layNgonNguSuDung(ret.TTHAI_BGHI);

                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuGrid, DatabaseConstant.Function.DC_DM_DIA_BAN);

                formCase = "XEM";
                HideControl();
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.CapNhatKoThanhCong", LMessage.MessageBoxType.Error);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(Id);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_DIA_BAN,
                DatabaseConstant.Table.DM_DIA_BAN,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Sau khi xóa
        /// </summary>
        /// <param name="ret"></param>
        private void afterDelete(bool ret)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);

                //SetEnabledAllControls(false);
                //SetEnabledRequiredControls(false);
                //CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuGrid, DatabaseConstant.Function.DC_DM_DIA_BAN);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(Id);
            List<int> listUnlockId = new List<int>();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_DIA_BAN,
                DatabaseConstant.Table.DM_DIA_BAN,
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

                tthaiNvu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);

                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuGrid, DatabaseConstant.Function.DC_DM_DIA_BAN);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.DuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(Id);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_DIA_BAN,
                DatabaseConstant.Table.DM_DIA_BAN,
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

                tthaiNvu = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);

                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuGrid, DatabaseConstant.Function.DC_DM_DIA_BAN);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.ThoaiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(Id);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_DIA_BAN,
                DatabaseConstant.Table.DM_DIA_BAN,
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

                tthaiNvu = BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);

                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuGrid, DatabaseConstant.Function.DC_DM_DIA_BAN);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.TuChoiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(Id);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_DIA_BAN,
                DatabaseConstant.Table.DM_DIA_BAN,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                listLockId);
        }        

        #endregion
    }
}
