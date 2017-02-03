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
using System.Collections;
using System.Reflection;
using Telerik.Windows.Controls.GridView;
using Presentation.Process.PhiServiceRef;
using Telerik.Windows.Controls;

namespace PresentationWPF.Phi.Phi
{
    /// <summary>
    /// Interaction logic for ucPhiCT.xaml
    /// </summary>
    public partial class ucPhiCT : UserControl
    {
        #region Khai bao
        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        List<AutoCompleteEntry> lstSourceLoai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiPhi = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHinhThucBacThang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiHachToan = new List<AutoCompleteEntry>();

        public static DatabaseConstant.Module Module = DatabaseConstant.Module.DMDC;
        public static DatabaseConstant.Function Function = DatabaseConstant.Function.DC_PHI;
        public static DatabaseConstant.Table Table = DatabaseConstant.Table.DC_BPHI;
        public DatabaseConstant.Action Action;

        public event EventHandler OnSavingCompleted;

        private int id = 0;
        public string formCase = null;
        bool isLoaded = false;
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
        DataTable dtPhiCT = new DataTable();
        DataTable dtPhiGD = new DataTable();
        DataTable dtPhiTK = new DataTable();

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        #endregion

        #region Khoi tao
        public ucPhiCT()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.Phi;component/Phi/ucPhiCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindShortkey();
            LoadCombobox();
            ResetForm();
            InitEventHandler();
            txtTenPhi.Focus();
            HideControl();
            HienThiPhuongPhapTinhPhi();
            HienThiTaiKhoanHachToan();
            isLoaded = false;
            // Refresh buttons
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu);
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
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.H, ModifierKeys.Control);
                        key = new KeyBinding(ucPhiCT.HoldCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(ucPhiCT.ModifyCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.Shift);
                        key = new KeyBinding(ucPhiCT.DeleteCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.A, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucPhiCT.ApproveCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucPhiCT.RefuseCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.C, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucPhiCT.CancelCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(ucPhiCT.HelpCommand, keyg);
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
            e.CanExecute = tlbHold.IsEnabled;
        }
        private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onHold();
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbSubmit.IsEnabled;
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
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                onHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
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
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
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

        #region Xu ly Giao dien

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
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        /// <summary>
        /// Sự kiện load cho form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isLoaded)
            {
                if (Action == DatabaseConstant.Action.XEM)
                    beforeView();
                else if (Action == DatabaseConstant.Action.SUA)
                    beforeModifyFromDetail();
                else
                    beforeAddNew();
            }
        }

        /// <summary>
        /// Sự kiện unload cho form
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
                DatabaseConstant.Function.DC_PHI,
                DatabaseConstant.Table.DC_BPHI,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Khởi tạo các combobox
        /// </summary>
        private void LoadCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            cmbLoaiTien.Items.Clear();
            lstSourceLoai = new List<AutoCompleteEntry>();
            lstSourceLoai.Add(new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DMUC_GTRI.LOAI_BIEU_PHI.PHI"), BusinessConstant.LOAI_PHI.PHI.layGiaTri()));
            lstSourceLoai.Add(new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DMUC_GTRI.LOAI_BIEU_PHI.HOAHONG"), BusinessConstant.LOAI_PHI.HOA_HONG.layGiaTri()));
            auto.GenAutoComboBox(ref lstSourceLoai, ref cmbLoai, null);

            cmbLoaiTien.Items.Clear();
            lstSourceLoaiTien = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TIENTE.getValue(), null, ClientInformation.MaDongNoiTe);

            cmbLoaiPhi.Items.Clear();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.PPHAP_TINH_LSUAT.getValue());
            lstSourceLoaiPhi = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstSourceLoaiPhi, ref cmbLoaiPhi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, BusinessConstant.PPHAP_TINH_LSUAT.DTH.layGiaTri());

            cmbPhiTheo.Items.Clear();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.HTHUC_BTHANG_PHI.getValue());
            lstSourceHinhThucBacThang = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstSourceHinhThucBacThang, ref cmbPhiTheo, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.DON_VI_HACH_TOAN));
            lstLoaiHachToan = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstLoaiHachToan, ref cmbLoaiHachToan, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, DatabaseConstant.PHUONG_PHAP_HACH_TOAN.DOC_LAP.layGiaTri());

            grdTKhoan.Columns[6].IsVisible = true;
            grdTKhoan.Columns[7].IsVisible = true;
            grdTKhoan.Columns[8].IsVisible = false;
            grdTKhoan.Columns[9].IsVisible = false;
        }

        /// <summary>
        /// InitEventHandler
        /// </summary>
        private void InitEventHandler()
        {
            cmbLoai.SelectionChanged += cmbLoai_SelectionChanged;
            cmbLoaiPhi.SelectionChanged += cmbLoaiPhi_SelectionChanged;
            cmbPhiTheo.SelectionChanged += cmbPhiTheo_SelectionChanged;
            cmbLoai.KeyDown += cmbLoai_KeyDown;
            //cmbLoaiTien.KeyDown += cmbLoaiTien_KeyDown;
            cmbLoaiPhi.KeyDown += cmbLoaiPhi_KeyDown;
            cmbPhiTheo.KeyDown += cmbPhiTheo_KeyDown;
            cmbLoaiHachToan.SelectionChanged +=new SelectionChangedEventHandler(cmbLoaiHachToan_SelectionChanged);
            tlbAddLoaiGD.Click +=new RoutedEventHandler(tlbAddLoaiGD_Click);
            tlbDeleteLoaiGD.Click +=new RoutedEventHandler(tlbDeleteLoaiGD_Click);
            //raddgrLSBacThangDS.CellValidating += raddgrLSBacThangDS_CellValidating;
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
                    arr = hethong.SetVisibleControl("PresentationWPF.Phi.Phi.ucPhiCT", formCase);
                    foreach (List<string> lst in arr)
                    {
                        object item = grMain.FindName(lst.First());
                        if (!item.IsNullOrEmpty())
                        {
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
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Đưa form về trạng thái mặc định
        /// </summary>
        private void ResetForm()
        {
            tthaiNvu = "";
            txtMaPhi.Text = string.Empty;
            txtTenPhi.Text = string.Empty;
            
            txtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai,ApplicationConstant.defaultDateTimeFormat);
            raddtNgayHL.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri());
            txtNguoiCapNhat.Text = "";
            raddtNgayHetHL.Value = null;
            lblTrangThai.Content = "";

            //cmbLoaiTien.SelectedIndex = 0;
            cmbPhiTheo.SelectedIndex = 0;
            cmbLoaiPhi.SelectedIndex = 0;

            raddgrLoaiGD.Items.Clear();
            raddgrLSBacThangDS.Items.Clear();

            titemThongTinChung.Focus();
        }

        private void HienThiPhuongPhapTinhPhi()
        {
            string phiTheo = lstSourceHinhThucBacThang.ElementAt(cmbPhiTheo.SelectedIndex).KeywordStrings.First();
            if (phiTheo.Equals(BusinessConstant.HTHUC_BTHANG_PHI.SO_TIEN.layGiaTri()))
            {
                radnumSoTienPhi.IsEnabled = true;
                radnumTyLePhi.IsEnabled = false;
                radnumPhiToiDa.IsEnabled = false;
                radnumPhiToiThieu.IsEnabled = false;
                raddgrLSBacThangDS.Columns[3].IsVisible = true;
                raddgrLSBacThangDS.Columns[4].IsVisible = false;
                raddgrLSBacThangDS.Columns[5].IsVisible = false;
                raddgrLSBacThangDS.Columns[6].IsVisible = false;
            }
            else if (phiTheo.Equals(BusinessConstant.HTHUC_BTHANG_PHI.TY_LE.layGiaTri()))
            {
                radnumSoTienPhi.IsEnabled = false;
                radnumTyLePhi.IsEnabled = true;
                radnumPhiToiDa.IsEnabled = true;
                radnumPhiToiThieu.IsEnabled = true;
                raddgrLSBacThangDS.Columns[3].IsVisible = false;
                raddgrLSBacThangDS.Columns[4].IsVisible = true;
                raddgrLSBacThangDS.Columns[5].IsVisible = true;
                raddgrLSBacThangDS.Columns[6].IsVisible = true;
            }
            else
            {
                radnumSoTienPhi.IsEnabled = true;
                radnumTyLePhi.IsEnabled = true;
            }

            string loaiPhi = lstSourceLoaiPhi.ElementAt(cmbLoaiPhi.SelectedIndex).KeywordStrings.First();
            if (loaiPhi.Equals(BusinessConstant.PPHAP_TINH_LSUAT.BTH.layGiaTri()))
            {
                grChiTietDT.Visibility = Visibility.Collapsed;
                grbPhiDS.Visibility = Visibility.Visible;
            }
            else
            {
                grChiTietDT.Visibility = Visibility.Visible;
                grbPhiDS.Visibility = Visibility.Collapsed;
            }
        }

        private void HienThiTaiKhoanHachToan()
        {
            string sDoiTuong = txtMaPhi.Text;
            if (sDoiTuong == "")
                sDoiTuong = "MACDINH";
            LoadDuLieuTaiKhoanHachToan(sDoiTuong);
        }

        private void cmbLoaiPhi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HienThiPhuongPhapTinhPhi();
        }

        private void cmbPhiTheo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HienThiPhuongPhapTinhPhi();
        }

        //private void cmbLoaiTien_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
        //    {
        //        if (lstSourceLoaiTien.Select(i => i.DisplayName).Contains(cmbLoaiTien.Text) != true)
        //            cmbLoaiTien.SelectedIndex = 0;
        //    }
        //    cmbLoaiTien.IsDropDownOpen = true;
        //}

        private void cmbLoai_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                if (lstSourceLoai.Select(i => i.DisplayName).Contains(cmbLoai.Text) != true)
                    cmbLoai.SelectedIndex = 0;
            }
            cmbLoai.IsDropDownOpen = true;
        }

        private void cmbLoaiPhi_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                if (lstSourceLoaiPhi.Select(i => i.DisplayName).Contains(cmbLoaiPhi.Text) != true)
                    cmbLoaiPhi.SelectedIndex = 0;
            }
            cmbLoaiPhi.IsDropDownOpen = true;
        }

        private void cmbPhiTheo_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                if (lstSourceHinhThucBacThang.Select(i => i.DisplayName).Contains(cmbPhiTheo.Text) != true)
                    cmbPhiTheo.SelectedIndex = 0;
            }
            cmbPhiTheo.IsDropDownOpen = true;
        }

        private void raddgrLSBacThangDS_Loaded(object sender, RoutedEventArgs e)
        {
            raddgrLSBacThangDS.SelectedItems.Clear();
            raddgrLSBacThangDS.Columns[1].Width = new Telerik.Windows.Controls.GridViewLength(50, Telerik.Windows.Controls.GridViewLengthUnitType.Pixel);
            raddgrLSBacThangDS.Columns[2].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Star);
            raddgrLSBacThangDS.Columns[3].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Star);
            raddgrLSBacThangDS.Columns[4].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Star);
            if (lstSourceHinhThucBacThang.ElementAt(cmbPhiTheo.SelectedIndex).KeywordStrings.First().Equals(BusinessConstant.HTHUC_BTHANG_PHI.SO_TIEN.layGiaTri()))
            {
                raddgrLSBacThangDS.Columns[3].IsVisible = true;
                raddgrLSBacThangDS.Columns[4].IsVisible = false;
                raddgrLSBacThangDS.Columns[5].IsVisible = false;
                raddgrLSBacThangDS.Columns[6].IsVisible = false;
            }
            else if (lstSourceHinhThucBacThang.ElementAt(cmbPhiTheo.SelectedIndex).KeywordStrings.First().Equals(BusinessConstant.HTHUC_BTHANG_PHI.TY_LE.layGiaTri()))
            {
                raddgrLSBacThangDS.Columns[3].IsVisible = false;
                raddgrLSBacThangDS.Columns[4].IsVisible = true;
                raddgrLSBacThangDS.Columns[5].IsVisible = true;
                raddgrLSBacThangDS.Columns[6].IsVisible = true;
            }
        }

        private void cmbLoai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HienThiTaiKhoanHachToan();
        }

        private void radGridNumSoTienPhi_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                ThemChiTiet();
                //raddgrLSBacThangDS.CurrentCell.ParentRow = (GridViewRow)raddgrLSBacThangDS.Items[raddgrLSBacThangDS.Items.Count-1];
                //raddgrLSBacThangDS.CurrentCell.Column = raddgrLSBacThangDS.Columns[2];
                //raddgrLSBacThangDS.Items[raddgrLSBacThangDS.Items.Count - 1] .Focus();
            }
        }

        private void radGridNumPhiTda_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                ThemChiTiet();
            }
        }

        private DataRow ShowPopupTaiKhoan(string dauTK)
        {
            {
                try
                {
                    lstPopup = new List<DataRow>();
                    var process = new PopupProcess();
                    List<string> lstDieuKien = new List<string>();
                    lstDieuKien.Add(ClientInformation.MaDonVi.ToString());
                    lstDieuKien.Add(dauTK);
                    process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_TKHOAN_THEO_DAU.getValue(), lstDieuKien);

                    SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                    ucPopup popup = new ucPopup(false, simplePopupResponse);
                    popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                    Window win = new Window();
                    win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                    win.Content = popup;
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    win.ShowDialog();
                    if (lstPopup != null)
                        return lstPopup.First();
                    else return null;
                }
                catch (System.Exception ex)
                {
                    return null;
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private List<DataRow> ShowPopupLoaiGD()
        {
            try
            {
                lstPopup = new List<DataRow>();
                var process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DM_PHE_GDICH.getValue());

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, true);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null)
                    return lstPopup;
                else return null;
            }
            catch (System.Exception ex)
            {
                return null;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
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

        private void PhanLoaiTK_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            GridViewRow grrow = btn.ParentOfType<GridViewRow>();
            PhanLoaiTaiKhoan(grrow);
        }

        private void PhanLoaiTKBSO_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            GridViewRow grrow = btn.ParentOfType<GridViewRow>();
            PhanLoaiTaiKhoanBSo(grrow);
        }

        private void PhanLoaiTaiKhoan(GridViewRow grrow)
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
        #endregion

        #region Xu ly nghiep vu

        /// <summary>
        /// Sự kiện load dữ liệu lên form
        /// </summary>
        private void SetFormData()
        {
            PhiProcess PhiProcess = new PhiProcess();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                dtPhiCT = new DataTable();
                dtPhiCT.Columns.Add("STT", typeof(string));
                dtPhiCT.Columns.Add("ID", typeof(string));
                dtPhiCT.Columns.Add("SO_TIEN", typeof(string));
                dtPhiCT.Columns.Add("SO_TIEN_PHI", typeof(string));
                dtPhiCT.Columns.Add("TY_LE_PHI", typeof(string));
                dtPhiCT.Columns.Add("STIEN_PHI_TTHIEU", typeof(string));
                dtPhiCT.Columns.Add("STIEN_PHI_TDA", typeof(string));

                dtPhiCT.NewRow();
                dtPhiCT.Rows.Add((dtPhiCT.Rows.Count + 1), 0, 0, 0, 0, 0, 0);
                raddgrLSBacThangDS.ItemsSource = dtPhiCT.DefaultView;
                raddgrLSBacThangDS.SelectedItems.Clear();

                dtPhiGD = new DataTable();
                dtPhiGD.Columns.Add("STT", typeof(string));
                dtPhiGD.Columns.Add("MA_LOAI_GDICH", typeof(string));
                dtPhiGD.Columns.Add("TEN_LOAI_GDICH", typeof(string));

                raddgrLoaiGD.ItemsSource = dtPhiGD.DefaultView;
                raddgrLoaiGD.SelectedItems.Clear();
                
                raddtNgayHetHL.Value = null;

                if (id != 0)
                {
                    //Sự kiện load dữ liệu
                    DataSet dsPhi = PhiProcess.GetPhiByID(id);
                    DataTable dtPhi = dsPhi.Tables[0];
                    dtPhiCT = dsPhi.Tables[1];
                    dtPhiTK = dsPhi.Tables[2];
                    dtPhiGD = dsPhi.Tables[3];
                    DataTable dtPhiLS = dsPhi.Tables[4]; //= PhiProcess.GetPhiLichSuByID(id);
                    if (dsPhi != null && dsPhi.Tables.Count > 0 && dtPhi.Rows.Count > 0)
                    {
                        // Thông tin chung
                        // ID,LOAI_BPHI,MA_BPHI,TEN_BPHI,NGAY_ADUNG,NGAY_HHAN,MA_LOAI_TIEN,TCHAT_BPHI,HTHUC_BTHANG,TY_LE_VAT,TTHAI_BGHI,TTHAI_NVU,NGAY_NHAP,NGUOI_NHAP,NGAY_CNHAT,NGUOI_CNHAT
                        // Chi tiết 
                        // ID,ID_BPHI,MA_BPHI,LOAI_BPHI,SO_TIEN,SO_TIEN_PHI,STIEN_PHI_TTHIEU,STIEN_PHI_TDA,TY_LE_PHI,TY_LE_VAT
                        // Tài khoản hạch toán
                        // SO_TAI_KHOAN_01,TEN_TAI_KHOAN_01,SO_TAI_KHOAN_02,TEN_TAI_KHOAN_02,SO_TAI_KHOAN_03,TEN_TAI_KHOAN_03
                        // Loại giao dịch
                        // MA_LOAI_GDICH, TEN_LOAI_GDICH
                        // Chi tiết lịch sử 
                        // STT,NGAY_ADUNG,SO_TIEN,SO_TIEN_PHI,TY_LE_PHI,TY_LE_VAT,STIEN_PHI_TTHIEU,STIEN_PHI_TDA

                        cmbLoaiTien.SelectedIndex = lstSourceLoaiTien.IndexOf(lstSourceLoaiTien.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dtPhi.Rows[0]["MA_LOAI_TIEN"].ToString())));
                        cmbLoaiPhi.SelectedIndex = lstSourceLoaiPhi.IndexOf(lstSourceLoaiPhi.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dtPhi.Rows[0]["TCHAT_BPHI"].ToString())));

                        txtMaPhi.Text = dtPhi.Rows[0]["MA_BPHI"].ToString();
                        txtTenPhi.Text = dtPhi.Rows[0]["TEN_BPHI"].ToString();
                        cmbLoai.SelectedIndex = lstSourceLoai.IndexOf(lstSourceLoai.FirstOrDefault(i => i.KeywordStrings.First().Equals(dtPhi.Rows[0]["LOAI_BPHI"].ToString())));
                        raddtNgayHL.Value = LDateTime.StringToDate(dtPhi.Rows[0]["NGAY_ADUNG"].ToString(), "yyyyMMdd");
                        if (dtPhi.Rows[0]["NGAY_HHAN"].ToString().Length == 8)
                            raddtNgayHetHL.Value = LDateTime.StringToDate(dtPhi.Rows[0]["NGAY_HHAN"].ToString(), "yyyyMMdd");

                        cmbPhiTheo.SelectedIndex = lstSourceHinhThucBacThang.IndexOf(lstSourceHinhThucBacThang.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dtPhi.Rows[0]["HTHUC_BTHANG"].ToString())));
                        if (!dtPhi.Rows[0]["TY_LE_VAT"].ToString().IsNullOrEmptyOrSpace())
                            radnumTyLeVAT.Value = Convert.ToDouble(dtPhi.Rows[0]["TY_LE_VAT"].ToString());
                        if (dtPhi.Rows[0]["TCHAT_BPHI"].ToString().Equals(BusinessConstant.PPHAP_TINH_LSUAT.BTH.layGiaTri()))
                        {
                            if (dtPhiCT.Rows.Count > 0)
                                raddgrLSBacThangDS.ItemsSource = dtPhiCT.DefaultView;
                        }
                        else
                        {
                            if (dtPhiCT.Rows.Count > 0)
                            {
                                if (lstSourceHinhThucBacThang.ElementAt(cmbPhiTheo.SelectedIndex).KeywordStrings.First().Equals(BusinessConstant.HTHUC_BTHANG_PHI.SO_TIEN.layGiaTri()))
                                    radnumSoTienPhi.Value = Convert.ToDouble(dtPhiCT.Rows[0]["SO_TIEN_PHI"].ToString());
                                if (lstSourceHinhThucBacThang.ElementAt(cmbPhiTheo.SelectedIndex).KeywordStrings.First().Equals(BusinessConstant.HTHUC_BTHANG_PHI.TY_LE.layGiaTri()))
                                    radnumTyLePhi.Value = Convert.ToDouble(dtPhiCT.Rows[0]["TY_LE_PHI"].ToString());
                            }
                        }
                        if (dtPhiTK.Rows.Count > 0)
                        {
                            
                        }
                        raddgrLoaiGD.ItemsSource = dtPhiGD;

                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(dtPhi.Rows[0]["TTHAI_BGHI"].ToString());
                        txtNguoiLap.Text = dtPhi.Rows[0]["NGUOI_NHAP"].ToString();
                        txtNguoiCapNhat.Text = dtPhi.Rows[0]["NGUOI_CNHAT"].ToString();
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(dtPhi.Rows[0]["TTHAI_NVU"].ToString());
                        TthaiNvu = dtPhi.Rows[0]["TTHAI_NVU"].ToString();
                        DateTime dtNgayLap = LDateTime.StringToDate(dtPhi.Rows[0]["NGAY_NHAP"].ToString(), "yyyyMMdd");
                        txtNgayLap.Text = dtNgayLap.ToString("dd/MM/yyyy");
                        if (!string.IsNullOrEmpty(dtPhi.Rows[0]["NGAY_CNHAT"].ToString()))
                        {
                            DateTime dtNgayDuyet = LDateTime.StringToDate(dtPhi.Rows[0]["NGAY_CNHAT"].ToString(), "yyyyMMdd");
                            txtNgayCapNhat.Text = dtNgayDuyet.ToString("dd/MM/yyyy");
                        }
                        LoadDuLieuTaiKhoanHachToan(txtMaPhi.Text);
                    }
                }
                else
                {
                    ResetForm();
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DanhMuc.ucPhiCT.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Đưa dữ liệu từ form vào object DC_BPHI
        /// </summary>
        private void GetFormData(ref DC_BPHI obj, ref List<DC_BPHI_CTIET> lstCT, ref List<DC_BPHI_GDICH> lstGD, ref List<KT_BPHI_TKHOAN> lstTK, ref List<KT_PHAN_HE_PLOAI> lstPHPL)
        {
            if (id != 0)
            {
                obj.ID = id;
                obj.MA_BPHI = txtMaPhi.Text;
                obj.NGUOI_NHAP = txtNguoiLap.Text;
                obj.NGAY_NHAP = ((DateTime)txtNgayLap.Value).ToString("yyyyMMdd");
            }
            else
            {
                obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                obj.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
            }

            obj.TEN_BPHI = txtTenPhi.Text;
            obj.LOAI_BPHI = lstSourceLoai.ElementAt(cmbLoai.SelectedIndex).KeywordStrings.First();
            obj.MA_LOAI_TIEN = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.First();
            obj.TCHAT_BPHI = lstSourceLoaiPhi.ElementAt(cmbLoaiPhi.SelectedIndex).KeywordStrings.First();
            obj.HTHUC_BTHANG = lstSourceHinhThucBacThang.ElementAt(cmbPhiTheo.SelectedIndex).KeywordStrings.First();
            obj.TY_LE_VAT = Convert.ToDecimal(radnumTyLeVAT.Value);
            obj.NGAY_ADUNG = ((DateTime)raddtNgayHL.Value).ToString("yyyyMMdd");
            if (raddtNgayHetHL.Value is DateTime)
                obj.NGAY_HHAN = ((DateTime)raddtNgayHetHL.Value).ToString("yyyyMMdd");

            obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
            obj.MA_DVI_TAO = ClientInformation.MaDonVi;
            obj.MA_DVI_QLY = ClientInformation.MaDonVi;
            obj.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();

            obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
            obj.NGAY_CNHAT = LDateTime.GetCurrentDate(ApplicationConstant.defaultDateTimeFormat);

            if (lstSourceLoaiPhi.ElementAt(cmbLoaiPhi.SelectedIndex).KeywordStrings.First().Equals(BusinessConstant.PPHAP_TINH_LSUAT.BTH.layGiaTri()))
            {
                foreach (DataRow row in dtPhiCT.Rows)
                {
                    //SO_TIEN,SO_TIEN_PHI,TY_LE_PHI,STIEN_PHI_TTHIEU,STIEN_PHI_TDA
                    //DataRow row = (DataRow)item;
                    DC_BPHI_CTIET objCT = new DC_BPHI_CTIET();
                    objCT.ID_BPHI = obj.ID;
                    objCT.LOAI_BPHI = obj.LOAI_BPHI;
                    string phiTheo = lstSourceHinhThucBacThang.ElementAt(cmbPhiTheo.SelectedIndex).KeywordStrings.First();
                    if (phiTheo.Equals(BusinessConstant.HTHUC_BTHANG_PHI.SO_TIEN.layGiaTri()))
                    {
                        objCT.SO_TIEN = Convert.ToDecimal(row["SO_TIEN"].ToString());
                        objCT.SO_TIEN_PHI = Convert.ToDecimal(row["SO_TIEN_PHI"].ToString());
                    }
                    if (phiTheo.Equals(BusinessConstant.HTHUC_BTHANG_PHI.TY_LE.layGiaTri()))
                    {
                        objCT.SO_TIEN = Convert.ToDecimal(row["SO_TIEN"].ToString());
                        objCT.TY_LE_PHI = Convert.ToDecimal(row["TY_LE_PHI"].ToString());
                        objCT.STIEN_PHI_TTHIEU = Convert.ToDecimal(row["STIEN_PHI_TTHIEU"].ToString());
                        objCT.STIEN_PHI_TDA = Convert.ToDecimal(row["STIEN_PHI_TDA"].ToString());
                    }
                    objCT.MA_DVI_QLY = obj.MA_DVI_QLY;
                    objCT.MA_DVI_TAO = obj.MA_DVI_TAO;
                    objCT.MA_BPHI = obj.MA_BPHI;
                    objCT.NGAY_CNHAT = obj.NGAY_CNHAT;
                    objCT.NGAY_NHAP = obj.NGAY_CNHAT;
                    objCT.NGUOI_CNHAT = obj.NGUOI_CNHAT;
                    objCT.NGUOI_NHAP = obj.NGUOI_CNHAT;
                    objCT.TTHAI_BGHI = obj.TTHAI_BGHI;
                    objCT.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    lstCT.Add(objCT);
                }
            }
            else if (lstSourceLoaiPhi.ElementAt(cmbLoaiPhi.SelectedIndex).KeywordStrings.First().Equals(BusinessConstant.PPHAP_TINH_LSUAT.DTH.layGiaTri()))
            {
                DC_BPHI_CTIET objCT = new DC_BPHI_CTIET();
                objCT.ID_BPHI = obj.ID;
                objCT.LOAI_BPHI = obj.LOAI_BPHI;
                string phiTheo = lstSourceHinhThucBacThang.ElementAt(cmbPhiTheo.SelectedIndex).KeywordStrings.First();
                if (phiTheo.Equals(BusinessConstant.HTHUC_BTHANG_PHI.SO_TIEN.layGiaTri()))
                {
                    objCT.SO_TIEN_PHI = (decimal)(radnumSoTienPhi.Value);
                    objCT.STIEN_PHI_TTHIEU = (decimal)(radnumSoTienPhi.Value);
                    objCT.STIEN_PHI_TDA = (decimal)(radnumSoTienPhi.Value);
                }
                else if (phiTheo.Equals(BusinessConstant.HTHUC_BTHANG_PHI.TY_LE.layGiaTri()))
                {
                    objCT.TY_LE_PHI = (decimal)(radnumTyLePhi.Value);
                    objCT.STIEN_PHI_TTHIEU = (decimal)(radnumPhiToiThieu.Value);
                    objCT.STIEN_PHI_TDA = (decimal)(radnumPhiToiDa.Value);
                }
                else if (phiTheo.Equals(BusinessConstant.HTHUC_BTHANG_PHI.STIEN_TLE.layGiaTri()))
                {
                    objCT.SO_TIEN_PHI = (decimal)(radnumSoTienPhi.Value);
                    objCT.TY_LE_PHI = (decimal)(radnumTyLePhi.Value);
                    objCT.STIEN_PHI_TTHIEU = (decimal)(radnumPhiToiThieu.Value);
                    objCT.STIEN_PHI_TDA = (decimal)(radnumPhiToiDa.Value);
                }
                objCT.MA_DVI_QLY = obj.MA_DVI_QLY;
                objCT.MA_DVI_TAO = obj.MA_DVI_TAO;
                objCT.MA_BPHI = obj.MA_BPHI;
                objCT.NGAY_CNHAT = obj.NGAY_CNHAT;
                objCT.NGAY_NHAP = obj.NGAY_CNHAT;
                objCT.NGUOI_CNHAT = obj.NGUOI_CNHAT;
                objCT.NGUOI_NHAP = obj.NGUOI_CNHAT;
                objCT.TTHAI_BGHI = obj.TTHAI_BGHI;
                objCT.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                lstCT.Add(objCT);
            }
            foreach (DataRow row in dtPhiGD.Rows)
            {
                //SO_TIEN,SO_TIEN_PHI,TY_LE_PHI,STIEN_PHI_TTHIEU,STIEN_PHI_TDA
                DC_BPHI_GDICH objCT = new DC_BPHI_GDICH();
                objCT.ID_BPHI = obj.ID;
                objCT.MA_LOAI_GDICH = row["MA_LOAI_GDICH"].ToString();
                objCT.MA_DVI_QLY = obj.MA_DVI_QLY;
                objCT.MA_DVI_TAO = obj.MA_DVI_TAO;
                objCT.MA_BPHI = obj.MA_BPHI;
                objCT.NGAY_CNHAT = obj.NGAY_CNHAT;
                objCT.NGAY_NHAP = obj.NGAY_CNHAT;
                objCT.NGUOI_CNHAT = obj.NGUOI_CNHAT;
                objCT.NGUOI_NHAP = obj.NGUOI_CNHAT;
                objCT.TTHAI_BGHI = obj.TTHAI_BGHI;
                objCT.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                lstGD.Add(objCT);
            }


            //SO_TIEN,SO_TIEN_PHI,TY_LE_PHI,STIEN_PHI_TTHIEU,STIEN_PHI_TDA
            KT_BPHI_TKHOAN objTK = new KT_BPHI_TKHOAN();
            objTK.ID_BPHI = obj.ID;
            objTK.MA_BPHI = obj.MA_BPHI;
            if (lstSourceLoai.ElementAt(cmbLoai.SelectedIndex).KeywordStrings.First().Equals(BusinessConstant.LOAI_PHI.PHI.layGiaTri()))
            {
                objTK.ID_TKHOAN_01 = 0;
                objTK.ID_TKHOAN_02 = 0;
            }
            if (lstSourceLoai.ElementAt(cmbLoai.SelectedIndex).KeywordStrings.First().Equals(BusinessConstant.LOAI_PHI.HOA_HONG.layGiaTri()))
            {
                objTK.ID_TKHOAN_01 = 0;
                objTK.ID_TKHOAN_02 = 0;
            }
            objTK.SO_TAI_KHOAN_03 = string.Empty;
            objTK.ID_TKHOAN_03 = 0;
            objTK.MA_DVI_QLY = obj.MA_DVI_QLY;
            objTK.MA_DVI_TAO = obj.MA_DVI_TAO;
            objTK.MA_BPHI = obj.MA_BPHI;
            objTK.NGAY_CNHAT = obj.NGAY_CNHAT;
            objTK.NGAY_NHAP = obj.NGAY_CNHAT;
            objTK.NGUOI_CNHAT = obj.NGUOI_CNHAT;
            objTK.NGUOI_NHAP = obj.NGUOI_CNHAT;
            objTK.TTHAI_BGHI = obj.TTHAI_BGHI;
            objTK.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
            lstTK.Add(objTK);

            DataView dv = (DataView)grdTKhoan.ItemsSource;
            foreach (DataRowView drv in dv)
            {
                KT_PHAN_HE_PLOAI objPhanHePLoai = new KT_PHAN_HE_PLOAI();
                objPhanHePLoai.ID_PHAN_HE = 0;
                objPhanHePLoai.ID = Convert.ToInt32(drv["ID"]);
                objPhanHePLoai.MA_DTUONG = txtMaPhi.Text;
                objPhanHePLoai.MA_PHAN_HE = DatabaseConstant.Module.DMDC.getValue();
                objPhanHePLoai.MA_KY_HIEU = drv["MA_KY_HIEU"].ToString();
                objPhanHePLoai.MA_PLOAI = drv["MA_PLOAI"].ToString();
                objPhanHePLoai.MA_PLOAI_BSO = drv["MA_PLOAI_BSO"].ToString();
                objPhanHePLoai.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                objPhanHePLoai.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                objPhanHePLoai.MA_DVI_QLY = ClientInformation.MaDonVi;
                objPhanHePLoai.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                objPhanHePLoai.NGUOI_NHAP = ClientInformation.TenDangNhap;
                objPhanHePLoai.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                lstPHPL.Add(objPhanHePLoai);
            }
            return;
        }

        /// <summary>
        /// Kiểm tra các thông tin nhập vào trước khi lưu dữ liệu
        /// </summary>
        /// <returns></returns>
        private bool Validation()
        {
            dtPhiCT = ((DataView)raddgrLSBacThangDS.ItemsSource).Table;
            for (int i = dtPhiCT.Rows.Count; i > 0; i--)
            {
                DataRow row = dtPhiCT.Rows[i - 1];
                if ((row["SO_TIEN"].ToString().Equals("0") || LString.IsNullOrEmptyOrSpace(row["SO_TIEN"].ToString())) && (row["SO_TIEN_PHI"].ToString().Equals("0") || LString.IsNullOrEmptyOrSpace(row["SO_TIEN_PHI"].ToString())) && (row["TY_LE_PHI"].ToString().Equals("0") || LString.IsNullOrEmptyOrSpace(row["TY_LE_PHI"].ToString())))
                    dtPhiCT.Rows.RemoveAt(i-1);
            }
            if (LString.IsNullOrEmptyOrSpace(txtTenPhi.Text))
            {
                LMessage.ShowMessage("M.DanhMuc.ucPhiCT.ThieuTenPhi", LMessage.MessageBoxType.Warning);
                txtTenPhi.Focus();
                return false;
            }

            else if (!(raddtNgayHL.Value is DateTime))
            {
                LMessage.ShowMessage("M.DanhMuc.ucPhiCT.NgayHieuLuc", LMessage.MessageBoxType.Warning);
                raddtNgayHL.Focus();
                return false;
            }

            //else if (cmbPhiTheo.IsEnabled == false && (radnumPhi.Value == 0 || LString.IsNullOrEmptyOrSpace(radnumPhi.Text)))
            //{
            //    LMessage.ShowMessage("M.DanhMuc.ucPhiCT.Phi", LMessage.MessageBoxType.Warning);
            //    radnumPhi.Focus();
            //    return false;
            //}
            //else if (cmbPhiTheo.IsEnabled && dtPhiCT.Rows.Count == 0)
            //{
            //    LMessage.ShowMessage("M.DanhMuc.ucPhiCT.PhiCT", LMessage.MessageBoxType.Warning);
            //    raddgrLSBacThangDS.Focus();
            //    return false;
            //}
            else
            {
                if (!ValidatePhiChiTiet())
                    return false;
            }
            return true;
        }

        private bool ValidatePhiChiTiet()
        {
            if (lstSourceLoaiPhi.ElementAt(cmbLoaiPhi.SelectedIndex).KeywordStrings.First().Equals(BusinessConstant.PPHAP_TINH_LSUAT.DTH.layGiaTri()))
            {
                if (lstSourceHinhThucBacThang.ElementAt(cmbPhiTheo.SelectedIndex).KeywordStrings.First().Equals(BusinessConstant.HTHUC_BTHANG_PHI.SO_TIEN.layGiaTri()) && radnumSoTienPhi.Value == 0)
                {
                    LMessage.ShowMessage("Số tiền phí phải lớn hơn 0", LMessage.MessageBoxType.Warning);
                    radnumSoTienPhi.Focus();
                    return false;
                }
                else if (lstSourceHinhThucBacThang.ElementAt(cmbPhiTheo.SelectedIndex).KeywordStrings.First().Equals(BusinessConstant.HTHUC_BTHANG_PHI.TY_LE.layGiaTri()) && radnumTyLePhi.Value == 0)
                {
                    LMessage.ShowMessage("Tỷ lệ phí phải lớn hơn 0", LMessage.MessageBoxType.Warning);
                    radnumTyLePhi.Focus();
                    return false;
                }
                else
                    return true;
            }
            else
            {
                foreach (DataRow row in dtPhiCT.Rows)
                {
                    //SO_TIEN,SO_TIEN_PHI,TY_LE_PHI,STIEN_PHI_TTHIEU,STIEN_PHI_TDA
                    int idxRow = dtPhiCT.Rows.IndexOf(row);
                    // Loại hình bậc thang theo Kỳ hạn
                    if (lstSourceHinhThucBacThang.ElementAt(cmbPhiTheo.SelectedIndex).KeywordStrings.First().Equals(BusinessConstant.HTHUC_BTHANG_PHI.TY_LE.layGiaTri()))
                    {
                        if (idxRow == 0)
                        {
                            if (!row["SO_TIEN"].ToString().Equals("0.00") && !row["SO_TIEN"].ToString().Equals("0"))
                            {
                                LMessage.ShowMessage("Số tiền đầu tiên phải bằng 0", LMessage.MessageBoxType.Warning);
                                raddgrLSBacThangDS.Focus();
                                return false;
                            }
                            else if (!(Convert.ToInt32(row["STIEN_PHI_TDA"].ToString()) >= Convert.ToInt32(dtPhiCT.Rows[idxRow - 1]["STIEN_PHI_TTHIEU"].ToString())))
                            {
                                LMessage.ShowMessage("Phí tối đa thứ " + (idxRow + 1) + " phải lớn hơn hoặc bằng Phí tối thiểu", LMessage.MessageBoxType.Warning);
                                raddgrLSBacThangDS.Focus();
                                return false;
                            }
                        }
                        else
                        {
                            if (!(Convert.ToInt32(row["SO_TIEN"].ToString()) > Convert.ToInt32(dtPhiCT.Rows[idxRow - 1]["SO_TIEN"].ToString())))
                            {
                                LMessage.ShowMessage("Số tiền thứ " + (idxRow + 1) + " phải lớn hơn Số tiền thứ " + idxRow, LMessage.MessageBoxType.Warning);
                                raddgrLSBacThangDS.Focus();
                                return false;
                            }
                            else if (!(Convert.ToInt32(row["STIEN_PHI_TDA"].ToString()) >= Convert.ToInt32(dtPhiCT.Rows[idxRow - 1]["STIEN_PHI_TTHIEU"].ToString())))
                            {
                                LMessage.ShowMessage("Phí tối đa thứ " + (idxRow + 1) + " phải lớn hơn hoặc bằng Phí tối thiểu", LMessage.MessageBoxType.Warning);
                                raddgrLSBacThangDS.Focus();
                                return false;
                            }
                        }
                    }
                    // Loại hình bậc thang theo Số tiền
                    else if (lstSourceHinhThucBacThang.ElementAt(cmbPhiTheo.SelectedIndex).KeywordStrings.First().Equals(BusinessConstant.HTHUC_BTHANG_PHI.SO_TIEN.layGiaTri()))
                    {
                        if (idxRow == 0)
                        {
                            if (!row["SO_TIEN"].ToString().Equals("0.00") && !row["SO_TIEN"].ToString().Equals("0"))
                            {
                                LMessage.ShowMessage("Số tiền đầu tiên phải bằng 0", LMessage.MessageBoxType.Warning);
                                raddgrLSBacThangDS.Focus();
                                return false;
                            }
                        }
                        else
                        {
                            if (!(Convert.ToInt32(row["SO_TIEN"].ToString().Replace(".00", "")) > Convert.ToInt32(dtPhiCT.Rows[idxRow - 1]["SO_TIEN"].ToString().Replace(".00", ""))))
                            {
                                LMessage.ShowMessage("Số tiền thứ " + (idxRow + 1) + " phải lớn hơn Số tiền thứ " + idxRow, LMessage.MessageBoxType.Warning);
                                raddgrLSBacThangDS.Focus();
                                return false;
                            }
                        }
                    }
                    // Loại hình bậc thang theo Kỳ hạn và số tiền
                    //else
                    //{
                    //    if (idxRow == 0)
                    //    {
                    //        if (!row["KYHAN"].ToString().Equals("0"))
                    //        {
                    //            LMessage.ShowMessage("Kỳ hạn đầu tiên phải bằng 0", LMessage.MessageBoxType.Warning);
                    //            raddgrLSBacThangDS.Focus();
                    //            return false;
                    //        }
                    //        else if (!row["SOTIEN"].ToString().Equals("0"))
                    //        {
                    //            LMessage.ShowMessage("Số tiền đầu tiên phải bằng 0", LMessage.MessageBoxType.Warning);
                    //            raddgrLSBacThangDS.Focus();
                    //            return false;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        int hieuKyHan = (Convert.ToInt32(row["KYHAN"].ToString()) - Convert.ToInt32(dtPhiCT.Rows[idxRow - 1]["KYHAN"].ToString()));
                    //        int hieuSoTien = (Convert.ToInt32(row["SOTIEN"].ToString()) - Convert.ToInt32(dtPhiCT.Rows[idxRow - 1]["SOTIEN"].ToString()));
                    //        if (!(hieuKyHan >= 0))
                    //        {
                    //            LMessage.ShowMessage("Kỳ hạn " + (idxRow + 1) + " phải lớn hơn hoặc bằng kỳ hạn " + idxRow, LMessage.MessageBoxType.Warning);
                    //            raddgrLSBacThangDS.Focus();
                    //            return false;
                    //        }
                    //        else if (hieuKyHan == 0 && !(hieuSoTien > 0))
                    //        {
                    //            LMessage.ShowMessage("Số tiền " + (idxRow + 1) + " phải lớn hơn số tiền " + idxRow, LMessage.MessageBoxType.Warning);
                    //            raddgrLSBacThangDS.Focus();
                    //            return false;
                    //        }
                    //    }
                    //}
                }
                return true;
            }
        }

        /// <summary>
        /// Trước khi xem
        /// </summary>
        public void beforeView()
        {
            SetFormData();
            formCase = "XEM";
            HideControl();
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu);
        }

        /// <summary>
        /// Trước khi thêm mới
        /// </summary>
        private void beforeAddNew()
        {
            id = 0;
            SetFormData();
            formCase = "MANAGE";
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu);
        }

        /// <summary>
        /// Trước khi sửa từ danh sách
        /// </summary>
        public void beforeModifyFromList()
        {
            //SetFormData();
            formCase = "MANAGE";
            HideControl();
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu);

            SetFormData();
            // Yêu cầu lock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_PHI,
                DatabaseConstant.Table.DC_BPHI,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Nếu lock thành công >> cho phép sửa
            if (retLockData)
            {
                //SetFormData();
                //SetEnabledAllControls(true);
                //SetEnabledRequiredControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu);
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
                DatabaseConstant.Function.DC_PHI,
                DatabaseConstant.Table.DC_BPHI,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Nếu lock thành công >> cho phép sửa
            if (ret)
            {
                SetFormData();
                formCase = "MANAGE";
                HideControl();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu);
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
                    DatabaseConstant.Function.DC_PHI,
                    DatabaseConstant.Table.DC_BPHI,
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
                    DatabaseConstant.Function.DC_PHI,
                    DatabaseConstant.Table.DC_BPHI,
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
                    DatabaseConstant.Function.DC_PHI,
                    DatabaseConstant.Table.DC_BPHI,
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
                    DatabaseConstant.Function.DC_PHI,
                    DatabaseConstant.Table.DC_BPHI,
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
                PhiProcess PhiProcess = new PhiProcess();
                try
                {
                    // Dữ liệu truyền vào và dữ liệu trả về
                    DC_BPHI obj = new DC_BPHI();
                    List<DC_BPHI_CTIET> lstCT = new List<DC_BPHI_CTIET>();
                    List<DC_BPHI_GDICH> lstGD = new List<DC_BPHI_GDICH>();
                    List<KT_BPHI_TKHOAN> lstTK = new List<KT_BPHI_TKHOAN>();
                    List<KT_PHAN_HE_PLOAI> lstPHPL = new List<KT_PHAN_HE_PLOAI>();

                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (id == 0)
                    {
                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref lstCT, ref lstGD, ref lstTK, ref lstPHPL);
                        obj.TTHAI_NVU = trangThai;

                        // Set các thông tin khác                        
                        obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                        obj.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                        List<DC_BPHI> lstLS = new List<DC_BPHI>();
                        lstLS.Add(obj);
                        List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                        PhiProcess.ProcessPhi(DatabaseConstant.Function.DC_PHI, DatabaseConstant.Action.THEM, ref lstLS, ref lstCT, ref lstTK, ref lstGD, ref lstPHPL, ref listClientResponseDetail);

                        afterAddNew(lstLS.First());
                    }
                    //Nếu là lưu tạm khi sửa
                    //Hoặc lưu tạm khi sửa sau duyệt
                    //Hoặc sửa
                    else
                    {
                        // Lấy thông tin cũ
                        //obj = PhiProcess.GetPhiByID(id);

                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref lstCT, ref lstGD, ref lstTK, ref lstPHPL);
                        obj.TTHAI_NVU = trangThai;
                        List<DC_BPHI> lstLS = new List<DC_BPHI>();
                        lstLS.Add(obj);

                        List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                        PhiProcess.ProcessPhi(DatabaseConstant.Function.DC_PHI, DatabaseConstant.Action.SUA, ref lstLS, ref lstCT, ref lstTK, ref lstGD, ref lstPHPL, ref listClientResponseDetail);
                        afterModify(lstLS.First());
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
                    PhiProcess = null;
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
                PhiProcess PhiProcess = new PhiProcess();
                try
                {
                    // Dữ liệu truyền vào và dữ liệu trả về
                    DC_BPHI obj = new DC_BPHI();
                    List<DC_BPHI_CTIET> lstCT = new List<DC_BPHI_CTIET>();
                    List<DC_BPHI_GDICH> lstGD = new List<DC_BPHI_GDICH>();
                    List<KT_BPHI_TKHOAN> lstTK = new List<KT_BPHI_TKHOAN>();
                    List<KT_PHAN_HE_PLOAI> lstPHPL = new List<KT_PHAN_HE_PLOAI>();
                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (id == 0)
                    {
                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref lstCT, ref lstGD, ref lstTK, ref lstPHPL);
                        obj.TTHAI_NVU = trangThai;

                        // Set các thông tin khác                        
                        obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                        obj.NGAY_NHAP = LDateTime.GetCurrentDate(ApplicationConstant.defaultDateTimeFormat);
                        List<DC_BPHI> lstLS = new List<DC_BPHI>();
                        lstLS.Add(obj);
                        List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                        PhiProcess.ProcessPhi(DatabaseConstant.Function.DC_PHI, DatabaseConstant.Action.THEM, ref lstLS, ref lstCT, ref lstTK, ref lstGD, ref lstPHPL, ref listClientResponseDetail);

                        afterModify(lstLS.First());
                    }
                    // Nếu là lưu tạm khi sửa
                    // Hoặc lưu tạm khi sửa sau duyệt
                    // Hoặc sửa
                    else
                    {
                        // Lấy thông tin cũ
                        obj = PhiProcess.LayThongTinPhi(id);

                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref lstCT, ref lstGD, ref lstTK, ref lstPHPL);
                        obj.TTHAI_NVU = trangThai;
                        List<DC_BPHI> lstLS = new List<DC_BPHI>();
                        lstLS.Add(obj);
                        List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                        PhiProcess.ProcessPhi(DatabaseConstant.Function.DC_PHI, DatabaseConstant.Action.SUA, ref lstLS, ref lstCT, ref lstTK, ref lstGD, ref lstPHPL, ref listClientResponseDetail);
                        afterModify(lstLS.First());
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
                    PhiProcess = null;
                }
            }
        }

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        private void onDelete()
        {
            PhiProcess PhiProcess = new PhiProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = id;
                // Dữ liệu truyền vào và dữ liệu trả về
                DC_BPHI obj = new DC_BPHI();
                obj.ID = id;
                List<DC_BPHI> lstLS = new List<DC_BPHI>();
                lstLS.Add(obj);
                List<DC_BPHI_CTIET> lstCT = new List<DC_BPHI_CTIET>();
                List<DC_BPHI_GDICH> lstGD = new List<DC_BPHI_GDICH>();
                List<KT_BPHI_TKHOAN> lstTK = new List<KT_BPHI_TKHOAN>();
                List<KT_PHAN_HE_PLOAI> lstPHPL = new List<KT_PHAN_HE_PLOAI>();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                bool ret = PhiProcess.ProcessPhi(DatabaseConstant.Function.DC_PHI, DatabaseConstant.Action.XOA, ref lstLS, ref lstCT, ref lstTK, ref lstGD, ref lstPHPL, ref listClientResponseDetail);

                afterDelete(ret);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listId = new List<int>();
                listId.Add(id);
                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_PHI,
                    DatabaseConstant.Table.DC_BPHI,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listId);
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
            PhiProcess PhiProcess = new PhiProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = id;
                // Dữ liệu truyền vào và dữ liệu trả về
                DC_BPHI obj = new DC_BPHI();
                obj.ID = id;
                List<DC_BPHI> lstLS = new List<DC_BPHI>();
                lstLS.Add(obj);
                List<DC_BPHI_CTIET> lstCT = new List<DC_BPHI_CTIET>();
                List<DC_BPHI_GDICH> lstGD = new List<DC_BPHI_GDICH>();
                List<KT_BPHI_TKHOAN> lstTK = new List<KT_BPHI_TKHOAN>();
                List<KT_PHAN_HE_PLOAI> lstPHPL = new List<KT_PHAN_HE_PLOAI>();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                bool ret = PhiProcess.ProcessPhi(DatabaseConstant.Function.DC_PHI, DatabaseConstant.Action.DUYET, ref lstLS, ref lstCT, ref lstTK, ref lstGD, ref lstPHPL, ref listClientResponseDetail);

                afterApprove(ret);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listId = new List<int>();
                listId.Add(id);
                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_PHI,
                    DatabaseConstant.Table.DC_BPHI,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listId);
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
            PhiProcess PhiProcess = new PhiProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = id;
                // Dữ liệu truyền vào và dữ liệu trả về
                DC_BPHI obj = new DC_BPHI();
                obj.ID = id;
                List<DC_BPHI> lstLS = new List<DC_BPHI>();
                lstLS.Add(obj);
                List<DC_BPHI_CTIET> lstCT = new List<DC_BPHI_CTIET>();
                List<DC_BPHI_GDICH> lstGD = new List<DC_BPHI_GDICH>();
                List<KT_BPHI_TKHOAN> lstTK = new List<KT_BPHI_TKHOAN>();
                List<KT_PHAN_HE_PLOAI> lstPHPL = new List<KT_PHAN_HE_PLOAI>();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                bool ret = PhiProcess.ProcessPhi(DatabaseConstant.Function.DC_PHI, DatabaseConstant.Action.THOAI_DUYET, ref lstLS, ref lstCT, ref lstTK, ref lstGD, ref lstPHPL, ref listClientResponseDetail);

                afterCancel(ret);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listId = new List<int>();
                listId.Add(id);
                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_PHI,
                    DatabaseConstant.Table.DC_BPHI,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listId);
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
            PhiProcess PhiProcess = new PhiProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = id;
                // Dữ liệu truyền vào và dữ liệu trả về
                DC_BPHI obj = new DC_BPHI();
                obj.ID = id;
                List<DC_BPHI> lstLS = new List<DC_BPHI>();
                lstLS.Add(obj);
                List<DC_BPHI_CTIET> lstCT = new List<DC_BPHI_CTIET>();
                List<DC_BPHI_GDICH> lstGD = new List<DC_BPHI_GDICH>();
                List<KT_BPHI_TKHOAN> lstTK = new List<KT_BPHI_TKHOAN>();
                List<KT_PHAN_HE_PLOAI> lstPHPL = new List<KT_PHAN_HE_PLOAI>();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                bool ret = PhiProcess.ProcessPhi(DatabaseConstant.Function.DC_PHI, DatabaseConstant.Action.TU_CHOI_DUYET, ref lstLS, ref lstCT, ref lstTK, ref lstGD, ref lstPHPL, ref listClientResponseDetail);

                afterRefuse(ret);
            }
            catch (System.Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listId = new List<int>();
                listId.Add(id);
                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_PHI,
                    DatabaseConstant.Table.DC_BPHI,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listId);
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
        private void afterAddNew(DC_BPHI ret)
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
                    id = ret.ID;
                    TthaiNvu = ret.TTHAI_NVU;
                    txtMaPhi.Text = ret.MA_BPHI;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(ret.TTHAI_NVU);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(ret.TTHAI_BGHI);

                    formCase = "XEM";
                    HideControl();
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu);

                    titemThongTinChung.Focus();
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
        private void afterModify(DC_BPHI ret)
        {
            if (ret != null)
            {
                LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                id = ret.ID;
                TthaiNvu = ret.TTHAI_NVU;
                txtMaPhi.Text = ret.MA_BPHI;
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(ret.TTHAI_NVU);
                txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(ret.TTHAI_BGHI);

                formCase = "XEM";
                HideControl();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu);

                titemThongTinChung.Focus();
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.CapNhatKoThanhCong", LMessage.MessageBoxType.Error);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_PHI,
                DatabaseConstant.Table.DC_BPHI,
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
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);
            List<int> listUnlockId = new List<int>();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_PHI,
                DatabaseConstant.Table.DC_BPHI,
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
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);

                formCase = "XEM";
                HideControl();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu);
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
                DatabaseConstant.Function.DC_PHI,
                DatabaseConstant.Table.DC_BPHI,
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

                formCase = "XEM";
                HideControl();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu);
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
                DatabaseConstant.Function.DC_PHI,
                DatabaseConstant.Table.DC_BPHI,
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

                formCase = "XEM";
                HideControl();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu);
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
                DatabaseConstant.Function.DC_PHI,
                DatabaseConstant.Table.DC_BPHI,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                listLockId);
        }

        private void tlbAddLSBacThang_Click(object sender, RoutedEventArgs e)
        {
            ThemChiTiet();
        }

        private void ThemChiTiet()
        {
            raddgrLSBacThangDS.ItemsSource = null;
            if (dtPhiCT.Rows.Count > 0)
            {
                for (int i = dtPhiCT.Rows.Count; i > 0; i--)
                {
                    DataRow row = dtPhiCT.Rows[i - 1];
                    if ((row["SO_TIEN"].ToString().Replace(".00", "").Equals("0") || LString.IsNullOrEmptyOrSpace(row["SO_TIEN"].ToString())) && (row["SO_TIEN_PHI"].ToString().Replace(".00", "").Equals("0") || LString.IsNullOrEmptyOrSpace(row["SO_TIEN_PHI"].ToString())) && (row["TY_LE_PHI"].ToString().Replace(".00", "").Equals("0") || LString.IsNullOrEmptyOrSpace(row["TY_LE_PHI"].ToString())))
                        dtPhiCT.Rows.RemoveAt(i - 1);
                }
                for (int i = dtPhiCT.Rows.Count; i > 0; i--)
                {
                    dtPhiCT.Rows[i - 1][0] = i.ToString();
                }
            }
            dtPhiCT.NewRow();
            dtPhiCT.Rows.Add((dtPhiCT.Rows.Count + 1), 0, 0, 0, 0, 0, 0);
            raddgrLSBacThangDS.ItemsSource = dtPhiCT.DefaultView;
        }

        private void tlbDeleteLSBacThang_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            try
            {
                if (raddgrLSBacThangDS.SelectedItems.Count > 0)
                {
                    List<DataRow> lstRowDel = new List<DataRow>();
                    foreach (DataRowView item in raddgrLSBacThangDS.SelectedItems)
                    {
                        DataRow r = dtPhiCT.AsEnumerable().FirstOrDefault(d => d.Field<string>("SO_TIEN").Equals(item.Row.Field<string>("SO_TIEN")) && d.Field<string>("SO_TIEN_PHI").Equals(item.Row.Field<string>("SO_TIEN_PHI")) && d.Field<string>("TY_LE_PHI").Equals(item.Row.Field<string>("TY_LE_PHI")));
                        lstRowDel.Add(r);
                    }
                    foreach (DataRow item in lstRowDel)
                    {
                        for (int i = dtPhiCT.Rows.Count; i > 0; i--)
                        {
                            if (dtPhiCT.Rows[i - 1][1].Equals(item[1]) && dtPhiCT.Rows[i - 1][2].Equals(item[2]) && dtPhiCT.Rows[i - 1][3].Equals(item[3]) && dtPhiCT.Rows[i - 1][4].Equals(item[4]) && dtPhiCT.Rows[i - 1][5].Equals(item[5]))
                            {
                                dtPhiCT.Rows.RemoveAt(i - 1);
                                break;
                            }
                        }
                    }
                    for (int i = dtPhiCT.Rows.Count; i > 0; i--)
                    {
                        dtPhiCT.Rows[i - 1][0] = i.ToString();
                    }
                    raddgrLSBacThangDS.ItemsSource = dtPhiCT.DefaultView;
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void tlbAddLoaiGD_Click(object sender, RoutedEventArgs e)
        {
            ThemLoaiGD();
        }

        private void ThemLoaiGD()
        {
            List<DataRow> lstrow = ShowPopupLoaiGD();
            if (lstrow != null)
            {
                if (dtPhiGD.Rows.Count > 0)
                {
                    for (int i = dtPhiGD.Rows.Count; i > 0; i--)
                    {
                        //MA_LOAI_GDICH, TEN_LOAI_GDICH
                        DataRow row1 = dtPhiGD.Rows[i - 1];
                        if (LString.IsNullOrEmptyOrSpace(row1["MA_LOAI_GDICH"].ToString()))
                            dtPhiGD.Rows.RemoveAt(i - 1);
                    }
                    for (int i = dtPhiGD.Rows.Count; i > 0; i--)
                    {
                        dtPhiGD.Rows[i - 1][0] = i.ToString();
                    }
                }
                foreach (DataRow row in lstrow)
                {
                    dtPhiGD.NewRow();
                    dtPhiGD.Rows.Add((dtPhiGD.Rows.Count + 1), row[2].ToString(), row[3].ToString());
                }

            }
            raddgrLoaiGD.ItemsSource = dtPhiGD.DefaultView;
        }

        private void tlbDeleteLoaiGD_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            try
            {
                if (raddgrLoaiGD.SelectedItems.Count > 0)
                {
                    List<DataRow> lstRowDel = new List<DataRow>();
                    foreach (DataRowView item in raddgrLoaiGD.SelectedItems)
                    {
                        DataRow r = dtPhiGD.AsEnumerable().FirstOrDefault(d => d.Field<string>("MA_LOAI_GDICH").Equals(item.Row.Field<string>("MA_LOAI_GDICH")));
                        lstRowDel.Add(r);
                    }
                    foreach (DataRow item in lstRowDel)
                    {
                        for (int i = dtPhiGD.Rows.Count; i > 0; i--)
                        {
                            if (dtPhiGD.Rows[i - 1][1].Equals(item[1]))
                            {
                                dtPhiGD.Rows.RemoveAt(i - 1);
                                break;
                            }
                        }
                    }
                    for (int i = dtPhiGD.Rows.Count; i > 0; i--)
                    {
                        dtPhiGD.Rows[i - 1][0] = i.ToString();
                    }
                    raddgrLoaiGD.ItemsSource = dtPhiGD.DefaultView;
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void LoadDuLieuTaiKhoanHachToan(string sDoiTuong)
        {
            DataTable dt = new PhiProcess().GetTaiKhoanHachToan(sDoiTuong, ClientInformation.MaDonVi).Tables["TAI_KHOAN_HACH_TOAN"];
            grdTKhoan.ItemsSource = dt.DefaultView;
        }
        #endregion

    }
}
