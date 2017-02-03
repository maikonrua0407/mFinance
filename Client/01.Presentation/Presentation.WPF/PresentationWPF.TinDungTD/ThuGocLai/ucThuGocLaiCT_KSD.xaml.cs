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
using System.Windows.Threading;
using System.Data;
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using PresentationWPF.CustomControl;
using Presentation.Process.TinDungTDServiceRef;
using Telerik.Windows.Controls;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.Common;
using System.Reflection;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;
using System.Collections;
using System.Reflection;


namespace PresentationWPF.TinDungTD.ThuGocLai
{
    /// <summary>
    /// Interaction logic for ucThuGocLaiCT_KSD.xaml
    /// </summary>
    public partial class ucThuGocLaiCT_KSD : UserControl
    {

        #region Khai bao
        public static RoutedCommand ImportCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand CloneCommand = new RoutedCommand();
        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SubmitCommand = new RoutedCommand();
        public static RoutedCommand CashStmtCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand PreviewCommand = new RoutedCommand();
        public static RoutedCommand ViewCommand = new RoutedCommand();
        public static RoutedCommand ExportCommand = new RoutedCommand();
        public static RoutedCommand SearchCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        private List<AutoCompleteEntry> lstSourceTienTe = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstSourceHinhThucThanhToan = new List<AutoCompleteEntry>();
        public DatabaseConstant.Action action;
        public event EventHandler OnSavingCompleted;
        TDTD_THU_GOC_LAI_VAY obj = new TDTD_THU_GOC_LAI_VAY();
        TDTD_THONG_TIN_THU_NO objTTinThuNo = new TDTD_THONG_TIN_THU_NO();
        List<TDTD_THONG_TIN_THU_NO> lstTTinThuNo = new List<TDTD_THONG_TIN_THU_NO>();
        string TThaiNVu = "";
        List<int> lstId = null;
        int iDGDich = 0;
        #endregion

        #region Khoi tao
        public ucThuGocLaiCT_KSD()
        {
            InitializeComponent();
            KhoiTaoComboBox();
            ResetForm();
            InitEventHanler();
        }

        public ucThuGocLaiCT_KSD(KIEM_SOAT _objKiemSoat)
            : this()
        {
            obj = new TDTD_THU_GOC_LAI_VAY();
            obj.ID_GDICH = _objKiemSoat.ID;
            obj.MA_GDICH = _objKiemSoat.SO_GIAO_DICH;
            obj.MA_KUOC = _objKiemSoat.MA_TCHIEU;
            action = _objKiemSoat.action;
            SetDataForm(obj.MA_GDICH);
        }
        private void KhoiTaoComboBox()
        {
            COMBOBOX_DTO combo = null;
            List<string> lstDieuKien = new List<string>();
            List<COMBOBOX_DTO> lstCombobox = new List<COMBOBOX_DTO>();
            AutoComboBox auCombo = new AutoComboBox();

            try
            {

                //Loại tiền
                combo = new COMBOBOX_DTO();
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_LOAITIEN.getValue();
                combo.combobox = txtLoaiTien;
                combo.lstSource = lstSourceTienTe;
                combo.maChon = ClientInformation.MaDongNoiTe;
                lstCombobox.Add(combo);

                combo = new COMBOBOX_DTO();
                combo.maCSo = null;
                combo.combobox = cbmTienTe;
                combo.lstSource = lstSourceTienTe;
                combo.maChon = ClientInformation.MaDongNoiTe;
                lstCombobox.Add(combo);

                //Thời hạn vay
                combo = null;
                combo = new COMBOBOX_DTO();
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.HINH_THUC_GIAO_DICH.getValue());
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                combo.lstDieuKien = lstDieuKien;
                combo.lstSource = lstSourceHinhThucThanhToan;
                combo.combobox = cmbHinhThucTToan;
                combo.maChon = BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri();
                lstCombobox.Add(combo);

                //Thời hạn vay
                combo = null;
                combo = new COMBOBOX_DTO();
                lstDieuKien = new List<string>();
                combo.maCSo = null;
                combo.lstDieuKien = lstDieuKien;
                combo.lstSource = lstSourceHinhThucThanhToan;
                combo.combobox = cmbHinhThucTToanTra;
                combo.maChon = BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri();
                lstCombobox.Add(combo);
                //Gen combobox
                auCombo.GenAutoComboBoxTheoList(ref lstCombobox);
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void InitEventHanler()
        {
            btnSoKheUoc.Click += new RoutedEventHandler(btnSoKheUoc_Click);
            btnSoTaiKhoan.Click += new RoutedEventHandler(btnSoTaiKhoan_Click);
            btnSoTaiKhoanTraKH.Click += new RoutedEventHandler(btnSoTaiKhoanTraKH_Click);
            cmbHinhThucTToan.SelectionChanged += new SelectionChangedEventHandler(cmbHinhThucTToan_SelectionChanged);
            cmbHinhThucTToanTra.SelectionChanged += new SelectionChangedEventHandler(cmbHinhThucTToanTra_SelectionChanged);
            grdThuGocLai.BeginningEdit += new EventHandler<Telerik.Windows.Controls.GridViewBeginningEditRoutedEventArgs>(grdThuGocLai_BeginningEdit);
            grdThuGocLai.CellValidating += new EventHandler<Telerik.Windows.Controls.GridViewCellValidatingEventArgs>(grdThuGocLai_CellValidating);
            grdThuGocLai.DataLoaded += new EventHandler<EventArgs>(grdThuGocLai_DataLoaded);
            chkTatToanTruocHan.Checked += new RoutedEventHandler(chkTatToanTruocHan_Checked);
            chkTatToanTruocHan.Unchecked += new RoutedEventHandler(chkTatToanTruocHan_Unchecked);
            grdThuGocLai.CellEditEnded += new EventHandler<Telerik.Windows.Controls.GridViewCellEditEndedEventArgs>(grdThuGocLai_CellEditEnded);

        }

        
        #endregion

        /// <summary>
        /// Dang ky hot key, shortcut key
        /// </summary>
        #region Dang ky hot key, shortcut key
        /// <summary>
        /// Binding HotKey
        /// </summary>
        private void BindHotkey()
        {
            foreach (var child in Toolbar.Children)
            {
                if (child.GetType() == typeof(RibbonButton))
                {
                    RibbonButton tlb = (RibbonButton)child;
                    KeyBinding key = new KeyBinding();
                    string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAP_DU_LIEU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.I, ModifierKeys.Shift);
                        key = new KeyBinding(ImportCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(ModifyCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.Shift);
                        key = new KeyBinding(DeleteCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.V, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(CloneCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.H, ModifierKeys.Control);
                        key = new KeyBinding(HoldCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(SubmitCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Shift);
                        key = new KeyBinding(CashStmtCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.A, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ApproveCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(RefuseCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.C, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(CancelCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.W, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(PreviewCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.W, ModifierKeys.Control);
                        key = new KeyBinding(ViewCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.E, ModifierKeys.Shift);
                        key = new KeyBinding(ExportCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(SearchCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(HelpCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Escape, ModifierKeys.None);
                        key = new KeyBinding(CloseCommand, keyg);
                        key.Gesture = keyg;
                    }

                    InputBindings.Add(key);
                }
            }
        }
        private void ImportCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ImportCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Nhập dữ liệu");
        }
        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Sửa dữ liệu");
        }
        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Xóa dữ liệu");
        }
        private void CloneCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CloneCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Nhân bản dữ liệu");
        }
        private void HoldCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Lưu tạm dữ liệu");
        }
        private void SubmitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SubmitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Lưu dữ liệu");
        }
        private void CashStmtCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CashStmtCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Bảng kê tiền mặt");
        }
        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Duyệt dữ liệu");
        }
        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Hủy duyệt dữ liệu");
        }
        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Từ chối dữ liệu");
        }
        private void PreviewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void PreviewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Xem trước dữ liệu");
        }
        private void ViewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ViewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Xem dữ liệu");
        }
        private void ExportCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ExportCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Xuất dữ liệu");
        }
        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Tìm kiếm dữ liệu");
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

        /// <summary>
        /// Sự kiện hot key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            txtSoGiaoDich.Focus();

            // Truongnx
            string strTinhNang = "";
            if (sender is RibbonButton)
            {
                RibbonButton tlb = (RibbonButton)sender;
                strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            }
            else if (sender is RibbonMenuItem)
            {
                RibbonMenuItem tlb = (RibbonMenuItem)sender;
                strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            }

            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAP_DU_LIEU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetForm();
                SetEnabledAllControls(true);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                beforeModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                BeforeSave(BusinessConstant.TrangThaiNghiepVu.LUU_TAM, BusinessConstant.TrangThaiBanGhi.SU_DUNG);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                BeforeSave(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET, BusinessConstant.TrangThaiBanGhi.SU_DUNG);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                BeforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                BeforeRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                BeforeCancel();
            }
            else if (strTinhNang.Equals("Print"))
            {
                OnPrint();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
                OnPreview();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            { }
            else if (strTinhNang.Equals("PreviewDonVayVon"))
            {
                OnPreviewDonVayVon();
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
        /// Sự kiện shortcutkey
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAP_DU_LIEU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetForm();
                SetEnabledAllControls(true);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
                OnPreview();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            { }
            else if (strTinhNang.Equals("PreviewDonVayVon"))
            {
                OnPreviewDonVayVon();
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

        /// <summary>
        /// Xu ly giao dien
        /// </summary>
        #region Xu ly giao dien

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

        private void ResetForm()
        {
            txtSoGiaoDich.Text = "";
            teldtNgayGiaoDich.Value = ClientInformation.NgayLamViecHienTai.StringToDate(ApplicationConstant.defaultDateTimeFormat);
            txtDienGiai.Text = "";
            txtSoKheUoc.Text = "";
            txtSoKheUoc.Tag = 0;
            txtSoHDTD.Text = "";
            txtMaKHang.Text = "";
            txtTenKHang.Text = "";
            txtSoDu.Value = null;
            chkHoanDuThu.IsChecked = false;
            chkTraNoTruocHan.IsChecked = false;
            chkTatToanTruocHan.IsChecked = false;
            obj = new TDTD_THU_GOC_LAI_VAY();
            objTTinThuNo = new TDTD_THONG_TIN_THU_NO();
            telnumTongTienGDich.Value = null;
            telnumTongTienTrongHan.Value = null;
            telnumGocTrongHan.Value = null;
            telnumLaiTrongHan.Value = null;
            telnumTongTienQuaHan.Value = null;
            telnumGocQuaHan.Value = null;
            telnumLaiQuaHan.Value = null;
            telnumLaiPhat.Value = null;
            telnumLaiDuThuTH.Value = null;
            telnumLaiDuThuQH.Value = null;
            telnumHoanDPhong.Value = null;
            telnumDoanhThuPB.Value = null;
            teldtNgayBDPB.Value = null;
            telnumSoTienQuyDoi.Value = null;
            telnumSoTienMat.Value = null;
            telnumSoChuyenKhoan.Value = null;
            txtSoTaiKhoan.Text = "";
            lblTenTaiKhoan.Content = LLanguage.SearchResourceByKey("U.TinDung.ucThuGocLaiCT_KSD.TenTaiKhoanHToan");
            telnumTraKHang.Value = null;
            telnumSoTienMatTraKH.Value = null;
            telnumSoChuyenKhoanTraKH.Value = null;
            txtSoTaiKhoanTraKH.Text = "";
            lblTenTaiKhoanTraKH.Content = LLanguage.SearchResourceByKey("U.TinDung.ucThuGocLaiCT_KSD.TenTaiKhoanHToan");
            grMain.DataContext = obj;
            obj.HOAN_DU_THU = BusinessConstant.CoKhong.KHONG.layGiaTri();
            obj.TRA_NO_THAN = BusinessConstant.CoKhong.KHONG.layGiaTri();
            obj.TAT_TOAN_THAN = BusinessConstant.CoKhong.KHONG.layGiaTri();
            action = DatabaseConstant.Action.THEM;
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDTD_THU_GOC_LAI);
        }

        void cmbHinhThucTToanTra_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoCompleteEntry auHinhThuc = lstSourceHinhThucThanhToan.ElementAt(cmbHinhThucTToanTra.SelectedIndex);
            if (auHinhThuc.KeywordStrings.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri()))
            {
                telnumSoTienMatTraKH.Value = telnumTraKHang.Value;
                telnumSoChuyenKhoanTraKH.Value = 0;
                telnumSoTienMatTraKH.IsEnabled = false;
                txtSoTaiKhoanTraKH.IsEnabled = false;
            }
            else if (auHinhThuc.KeywordStrings.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TMAT_CKHOAN.layGiaTri()))
            {
                telnumSoTienMatTraKH.Value = telnumTraKHang.Value;
                telnumSoChuyenKhoanTraKH.Value = 0;
                telnumSoTienMatTraKH.IsEnabled = true;
                txtSoTaiKhoanTraKH.IsEnabled = true;
            }
            else
            {
                telnumSoTienMatTraKH.Value = 0;
                telnumSoChuyenKhoanTraKH.Value = telnumTraKHang.Value;
                telnumSoTienMatTraKH.IsEnabled = false;
                txtSoTaiKhoanTraKH.IsEnabled = true;
            }
        }

        void cmbHinhThucTToan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoCompleteEntry auHinhThuc = lstSourceHinhThucThanhToan.ElementAt(cmbHinhThucTToan.SelectedIndex);
            if (auHinhThuc.KeywordStrings.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri()))
            {
                telnumSoTienMat.Value = telnumTongTienGDich.Value;
                telnumSoChuyenKhoan.Value = 0;
                telnumSoTienMat.IsEnabled = false;
                txtSoTaiKhoan.IsEnabled = false;
            }
            else if (auHinhThuc.KeywordStrings.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TMAT_CKHOAN.layGiaTri()))
            {
                telnumSoTienMat.Value = telnumTongTienGDich.Value;
                telnumSoChuyenKhoan.Value = 0;
                telnumSoTienMat.IsEnabled = true;
                txtSoTaiKhoan.IsEnabled = true;
            }
            else
            {
                telnumSoTienMat.Value = 0;
                telnumSoChuyenKhoan.Value = telnumTongTienGDich.Value;
                telnumSoTienMat.IsEnabled = false;
                txtSoTaiKhoan.IsEnabled = true;
            }
        }

        void btnSoTaiKhoanTraKH_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void btnSoTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void btnSoKheUoc_Click(object sender, RoutedEventArgs e)
        {
            lstPopup.Clear();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
            lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation("POPUP_DS_TDTD_KHE_UOC_THU_GOC_LAI", lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            ucPopup popup = new ucPopup(false, simplePopupResponse);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
            win.Content = popup;
            win.ShowDialog();
            if (lstPopup.Count > 0)
            {
                DataRow dr = lstPopup.FirstOrDefault();
                obj.ID_HDTD = Convert.ToInt32(dr["ID_HDTD"]);
                obj.ID_KHANG = Convert.ToInt32(dr["ID_KHANG"]);
                obj.ID_KUOC = Convert.ToInt32(dr["ID_KUOC"]);
                obj.LOAI_TIEN = Convert.ToString(dr["LOAI_TIEN"]);
                obj.LOAI_TIEN_QDOI = Convert.ToString(dr["LOAI_TIEN"]);
                obj.MA_KHANG = Convert.ToString(dr["MA_KHANG"]);
                obj.MA_KUOC = Convert.ToString(dr["MA_KUOC"]);
                obj.MA_SAN_PHAM = Convert.ToString(dr["MA_SAN_PHAM"]);
                if (dr["PHAN_BO_DEN_NGAY"] != DBNull.Value)
                    obj.NGAY_BD_PBO = Convert.ToString(dr["PHAN_BO_DEN_NGAY"]);
                obj.NGAY_GD = ClientInformation.NgayLamViecHienTai;
                obj.NGAY_GDICH = ClientInformation.NgayLamViecHienTai;
                obj.NGUOI_GD = ClientInformation.HoTen;
                obj.NHOM_NO = Convert.ToString(dr["NHOM_NO"]);
                obj.NOI_CAP = Convert.ToString(dr["DD_GTLQ_NOI_CAP"]);
                obj.NV_LOAI_NVON = Convert.ToString(dr["NV_LOAI_NVON"]);
                obj.SO_CMND = Convert.ToString(dr["DD_GTLQ_SO"]);
                obj.DIA_CHI = Convert.ToString(dr["DIA_CHI"]);
                obj.HOAN_DU_PHONG = Convert.ToDecimal(dr["DPHONG_DA_TRICH"]);
                if (dr["LAI_DA_XUAT_NB"] != DBNull.Value)
                    obj.LAI_DU_THU_QH = Convert.ToDecimal(dr["LAI_DA_XUAT_NB"]);
                if (dr["LAI_DU_THU"] != DBNull.Value)
                    obj.LAI_DU_THU_TH = Convert.ToDecimal(dr["LAI_DU_THU"]);
                obj.TEN_KHANG = Convert.ToString(dr["TEN_KHANG"]);
                obj.SO_DU = Convert.ToDecimal(dr["SO_DU"]);
                obj.SO_HDTD = Convert.ToString(dr["SO_HDTD"]);
                obj.SO_KUOC = Convert.ToString(dr["SO_KUOC"]);
                //txtSoKheUoc.Text = obj.SO_KUOC;
                //txtSoHDTD.Text = obj.SO_HDTD;
                //txtMaKHang.Text = obj.MA_KHANG;
                //txtTenKHang.Text = obj.TEN_KHANG;
                //txtSoDu.Value = Convert.ToDouble(obj.SO_DU);
                txtLoaiTien.SelectedIndex = lstSourceTienTe.IndexOf(lstSourceTienTe.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(obj.LOAI_TIEN)));
                GetKeHoachThuGocLai();
            }
        }

        void GetKeHoachThuGocLai()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dtPar = null;
                LDatatable.MakeParameterTable(ref dtPar);
                LDatatable.AddParameter(ref dtPar, "@ID_KUOC", "String", obj.ID_KUOC.ToString());
                LDatatable.AddParameter(ref dtPar, "@MA_KUOC", "String", obj.MA_KUOC.ToString());
                LDatatable.AddParameter(ref dtPar, "@NGAY_GDICH", "String", ClientInformation.NgayLamViecHienTai);
                ds = new TinDungTDProcess().GetThongTinKeHoachThuGocLai(dtPar);
                lstTTinThuNo = new List<TDTD_THONG_TIN_THU_NO>();
                if (!ds.IsNullOrEmpty() && !ds.Tables.IsNullOrEmpty() && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        objTTinThuNo = new TDTD_THONG_TIN_THU_NO();
                        objTTinThuNo.GOC_KH = Convert.ToDecimal(dr["TRA_GOC_KH"]);
                        objTTinThuNo.LAI_KH = Convert.ToDecimal(dr["TRA_LAI_KH"]);
                        objTTinThuNo.LAI_PHAT_KH = Convert.ToDecimal(dr["LAI_PHAT_KH"]);
                        objTTinThuNo.GOC_TT = Convert.ToDecimal(dr["TRA_GOC_TT"]);
                        objTTinThuNo.LAI_TT = Convert.ToDecimal(dr["TRA_LAI_TT"]);
                        objTTinThuNo.LAI_PHAT_TT = Convert.ToDecimal(dr["LAI_PHAT_TT"]);
                        objTTinThuNo.NGAY_KH = Convert.ToString(dr["NGAY_KH"]);
                        objTTinThuNo.ID_KUOC = Convert.ToInt32(dr["ID_KUOC"]);
                        objTTinThuNo.MA_KUOC = Convert.ToString(dr["MA_KUOC"]);
                        objTTinThuNo.NHOM_NO = Convert.ToString(dr["TTHAI_NO"]);
                        objTTinThuNo.SO_NGAY_QH = Convert.ToInt32(dr["SO_NGAY_QH"]);
                        lstTTinThuNo.Add(objTTinThuNo);
                    }
                    grdThuGocLai.ItemsSource = null;
                    grdThuGocLai.ItemsSource = lstTTinThuNo;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void grdThuGocLai_CellValidating(object sender, Telerik.Windows.Controls.GridViewCellValidatingEventArgs e)
        {
            TDTD_THONG_TIN_THU_NO objTTin = e.Cell.ParentRow.Item as TDTD_THONG_TIN_THU_NO;
            if (!e.Cell.Column.UniqueName.IsNullOrEmptyOrSpace() && e.Cell.Column.UniqueName.Equals("ThucTeGoc") && objTTin.GOC_KH < Convert.ToDecimal(e.NewValue))
            {
                e.IsValid = false;
                e.ErrorMessage = LLanguage.SearchResourceByKey("M_ResponseMessage_SoTien_KhongHopLe");
            }
            else if (!e.Cell.Column.UniqueName.IsNullOrEmptyOrSpace() && e.Cell.Column.UniqueName.Equals("ThucTeLai") && objTTin.LAI_KH < Convert.ToDecimal(e.NewValue))
            {
                e.IsValid = false;
                e.ErrorMessage = LLanguage.SearchResourceByKey("M_ResponseMessage_SoTien_KhongHopLe");
            }
        }

        void grdThuGocLai_BeginningEdit(object sender, Telerik.Windows.Controls.GridViewBeginningEditRoutedEventArgs e)
        {
            TDTD_THONG_TIN_THU_NO objTTin = e.Cell.ParentRow.Item as TDTD_THONG_TIN_THU_NO;
            int Index = lstTTinThuNo.IndexOf(objTTin);
            Index--;
            TDTD_THONG_TIN_THU_NO objTTinTrc = null;
            if (Index > -1)
            {
                objTTinTrc = lstTTinThuNo.ElementAt(Index);
                if (objTTinTrc.GOC_KH + objTTinTrc.LAI_KH > objTTinTrc.GOC_TT + objTTinTrc.LAI_TT)
                    e.Cancel = true;
            }
            if(obj.TRA_NO_THAN.Equals(BusinessConstant.CoKhong.KHONG.layGiaTri()))
            {
                if (objTTin.NGAY_KH.CompareTo(ClientInformation.NgayLamViecHienTai) > 0)
                    e.Cancel = true;
            }
            else if (obj.TAT_TOAN_THAN.Equals(BusinessConstant.CoKhong.CO.layGiaTri()))
            {
                if (e.Cell.Column.UniqueName.IsNullOrEmptyOrSpace() || !e.Cell.Column.UniqueName.Equals("ThucTeLai"))
                    e.Cancel = true;
            }
            
        }

        void grdThuGocLai_DataLoaded(object sender, EventArgs e)
        {
            AutoCompleteEntry auHinhThuc = lstSourceHinhThucThanhToan.ElementAt(cmbHinhThucTToan.SelectedIndex);
            obj.GOC_QUA_HAN = lstTTinThuNo.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_TT);
            obj.GOC_TRONG_HAN = lstTTinThuNo.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_TT);
            obj.LAI_QUA_HAN = lstTTinThuNo.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT);
            obj.LAI_TRONG_HAN = lstTTinThuNo.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT);
            obj.LAI_PHAT = lstTTinThuNo.Sum(f => f.LAI_TT);
            obj.NO_QUA_HAN = obj.GOC_QUA_HAN + obj.LAI_QUA_HAN + obj.LAI_PHAT;
            obj.NO_TRONG_HAN = obj.GOC_TRONG_HAN + obj.LAI_TRONG_HAN;
            obj.SO_TIEN_GDICH = obj.NO_TRONG_HAN + obj.NO_QUA_HAN;
            obj.SO_TIEN_QDOI = obj.SO_TIEN_GDICH;
            if (!auHinhThuc.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN.layGiaTri()))
            {
                obj.SO_TIEN_MAT = obj.SO_TIEN_QDOI;
                obj.SO_TIEN_CK = 0;
            }
            else
            {
                obj.SO_TIEN_CK = obj.SO_TIEN_QDOI;
                obj.SO_TIEN_MAT = 0;
            }

        }

        void chkTatToanTruocHan_Unchecked(object sender, RoutedEventArgs e)
        {
            lstTTinThuNo.ForEach(f => { f.GOC_TT = 0; f.LAI_TT = 0; });
            grdThuGocLai.Rebind();
        }

        void chkTatToanTruocHan_Checked(object sender, RoutedEventArgs e)
        {
            lstTTinThuNo.ForEach(f => f.GOC_TT = f.GOC_KH);
            grdThuGocLai.Rebind();
        }

        void grdThuGocLai_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            AutoCompleteEntry auHinhThuc = lstSourceHinhThucThanhToan.ElementAt(cmbHinhThucTToan.SelectedIndex);
            obj.GOC_QUA_HAN = lstTTinThuNo.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_TT);
            obj.GOC_TRONG_HAN = lstTTinThuNo.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_TT);
            obj.LAI_QUA_HAN = lstTTinThuNo.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT);
            obj.LAI_TRONG_HAN = lstTTinThuNo.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT);
            obj.LAI_PHAT = lstTTinThuNo.Sum(f => f.LAI_PHAT_TT);
            obj.NO_QUA_HAN = obj.GOC_QUA_HAN + obj.LAI_QUA_HAN + obj.LAI_PHAT;
            obj.NO_TRONG_HAN = obj.GOC_TRONG_HAN + obj.LAI_TRONG_HAN;
            obj.SO_TIEN_GDICH = obj.NO_TRONG_HAN + obj.NO_QUA_HAN;
            obj.SO_TIEN_QDOI = obj.SO_TIEN_GDICH;
            if (!auHinhThuc.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN.layGiaTri()))
            {
                obj.SO_TIEN_MAT = obj.SO_TIEN_QDOI;
                obj.SO_TIEN_CK = 0;
            }
            else
            {
                obj.SO_TIEN_CK = obj.SO_TIEN_QDOI;
                obj.SO_TIEN_MAT = 0;
            }
        }

        #endregion

        #region Xu ly nghiep vu
        void GetDataForm(BusinessConstant.TrangThaiBanGhi bghi, BusinessConstant.TrangThaiNghiepVu nghiepvu)
        {
            try
            {
                AutoCompleteEntry auLoaiTien = lstSourceTienTe.ElementAt(txtLoaiTien.SelectedIndex);
                AutoCompleteEntry auLoaiTienQDoi = lstSourceTienTe.ElementAt(cbmTienTe.SelectedIndex);
                AutoCompleteEntry auHinhThucTToan = lstSourceHinhThucThanhToan.ElementAt(cmbHinhThucTToan.SelectedIndex);
                AutoCompleteEntry auHinhThucTToanKH = lstSourceHinhThucThanhToan.ElementAt(cmbHinhThucTToanTra.SelectedIndex);
                obj.LOAI_TIEN = auLoaiTien.KeywordStrings.FirstOrDefault();
                obj.LOAI_TIEN_QDOI = auLoaiTienQDoi.KeywordStrings.FirstOrDefault();
                obj.HINH_THUC_TT = auHinhThucTToan.KeywordStrings.FirstOrDefault();
                obj.HINH_THUC_TRA_KH = auHinhThucTToanKH.KeywordStrings.FirstOrDefault();
                
                obj.DSACH_THU_NO = lstTTinThuNo.ToArray();
                if (obj.ID_GDICH > 0)
                {
                    obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                }
                else
                {
                    obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    obj.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    obj.MA_DVI_GDICH = ClientInformation.MaDonViGiaoDich;
                    obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                    obj.TTHAI_BGHI = bghi.layGiaTri();
                    obj.TTHAI_NVU = nghiepvu.layGiaTri();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public void SetDataForm(string sSoGiaoDich)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dtPar = null;
                LDatatable.MakeParameterTable(ref dtPar);
                LDatatable.AddParameter(ref dtPar, "@SoGiaoDich", "String", obj.MA_GDICH);
                LDatatable.AddParameter(ref dtPar, "@IdGiaoDich", "String", obj.ID_GDICH.ToString());
                ds = new TinDungTDProcess().GetThongTinThuGocLaiVay(dtPar);
                Dispatcher.CurrentDispatcher.DelayInvoke("SetDataForm", () =>
                {
                    SetTabThongTinChung(ds);

                    Dispatcher.CurrentDispatcher.DelayInvoke("SetTabThongKiemSoat", () =>
                    {
                        SetTabThongKiemSoat(ds);
                    }, TimeSpan.FromSeconds(0));

                }, TimeSpan.FromSeconds(0));
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void SetTabThongTinChung(DataSet ds)
        {
            try
            {
                DataTable dt = ds.Tables["TTIN_CHUNG"];
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    obj = new TDTD_THU_GOC_LAI_VAY();
                    obj.ID_GDICH = Convert.ToInt32(dr["ID_GDICH"]);
                    obj.MA_GDICH = Convert.ToString(dr["MA_GDICH"]);
                    obj.NGAY_GDICH = dr["NGAY_GDICH"].ToString();
                    obj.NGAY_GD = dr["NGAY_GD"].ToString();
                    obj.TTHAI_NVU = dr["TTHAI_NVU"].ToString();
                    obj.DIEN_GIAI = dr["DIEN_GIAI"].ToString();
                    obj.DIA_CHI = dr["DIA_CHI"].ToString();
                    obj.DOANH_THU_PBO = Convert.ToDecimal(dr["DOANH_THU_PBO"]);
                    obj.GOC_QUA_HAN = Convert.ToDecimal(dr["GOC_QUA_HAN"]);
                    obj.GOC_TRONG_HAN = Convert.ToDecimal(dr["GOC_TRONG_HAN"]);
                    obj.HINH_THUC_TRA_KH = Convert.ToString(dr["HINH_THUC_TRA_KH"]);
                    obj.HINH_THUC_TT = Convert.ToString(dr["HINH_THUC_TT"]);
                    obj.HOAN_DU_PHONG = Convert.ToDecimal(dr["HOAN_DU_PHONG"]);
                    obj.HOAN_DU_THU = Convert.ToString(dr["HOAN_DU_THU"]);
                    obj.ID_HDTD = Convert.ToInt32(dr["ID_HDTD"]);
                    obj.ID_KUOC = Convert.ToInt32(dr["ID_KUOC"]);
                    obj.LAI_DU_THU_QH = Convert.ToDecimal(dr["LAI_DU_THU_QH"]);
                    obj.LAI_DU_THU_TH = Convert.ToDecimal(dr["LAI_DU_THU_TH"]);
                    obj.LAI_PHAT = Convert.ToDecimal(dr["LAI_PHAT"]);
                    obj.LAI_QUA_HAN = Convert.ToDecimal(dr["LAI_QUA_HAN"]);
                    obj.LAI_TRA_TRUOC = Convert.ToDecimal(dr["LAI_TRA_TRUOC"]);
                    obj.LAI_TRONG_HAN = Convert.ToDecimal(dr["LAI_TRONG_HAN"]);
                    obj.LOAI_TIEN = Convert.ToString(dr["LOAI_TIEN"]);
                    obj.LOAI_TIEN_QDOI = Convert.ToString(dr["LOAI_TIEN_QDOI"]);
                    obj.MA_DVI_GDICH = Convert.ToString(dr["MA_DVI"]);
                    obj.MA_DVI_QLY = Convert.ToString(dr["MA_DVI_QLY"]);
                    obj.MA_HDTD = Convert.ToString(dr["MA_HDTD"]);
                    obj.MA_KHANG = Convert.ToString(dr["MA_KHANG"]);
                    obj.MA_KUOC = Convert.ToString(dr["MA_KUOC"]);
                    obj.MA_SAN_PHAM = Convert.ToString(dr["MA_SAN_PHAM"]);
                    if (dr["NGAY_BD_PBO"] != DBNull.Value)
                        obj.NGAY_BD_PBO = Convert.ToString(dr["NGAY_BD_PBO"]);
                    if (dr["NGAY_CAP"] != DBNull.Value)
                        obj.NGAY_CAP = Convert.ToString(dr["NGAY_CAP"]);
                    if (dr["NGAY_CNHAT"] != DBNull.Value)
                        obj.NGAY_CNHAT = Convert.ToString(dr["NGAY_CNHAT"]);
                    obj.NGAY_NHAP = Convert.ToString(dr["NGAY_NHAP"]);
                    if (dr["NGUOI_CNHAT"] != DBNull.Value)
                        obj.NGUOI_CNHAT = Convert.ToString(dr["NGUOI_CNHAT"]);
                    if (dr["NGUOI_NHAP"] != DBNull.Value)
                        obj.NGUOI_NHAP = Convert.ToString(dr["NGUOI_NHAP"]);
                    if (dr["NHOM_NO"] != DBNull.Value)
                        obj.NHOM_NO = Convert.ToString(dr["NHOM_NO"]);
                    obj.NO_QUA_HAN = Convert.ToDecimal(dr["NO_QUA_HAN"]);
                    obj.NO_TRONG_HAN = Convert.ToDecimal(dr["NO_TRONG_HAN"]);
                    obj.NOI_CAP = Convert.ToString(dr["NOI_CAP"]);
                    obj.NV_LOAI_NVON = Convert.ToString(dr["NV_LOAI_NVON"]);
                    obj.SO_CMND = Convert.ToString(dr["SO_CMND"]);
                    obj.SO_DU = Convert.ToDecimal(dr["SO_DU"]);
                    obj.SO_HDTD = Convert.ToString(dr["SO_HDTD"]);
                    obj.SO_KUOC = Convert.ToString(dr["SO_KUOC"]);
                    obj.SO_TAI_KHOAN = Convert.ToString(dr["SO_TAI_KHOAN"]);
                    obj.SO_TIEN_CK = Convert.ToDecimal(dr["SO_TIEN_CK"]);
                    obj.SO_TIEN_CK_TRA_KH = Convert.ToDecimal(dr["SO_TIEN_CK_TRA_KH"]);
                    obj.SO_TIEN_GDICH = Convert.ToDecimal(dr["SO_TIEN_GDICH"]);
                    obj.SO_TIEN_MAT = Convert.ToDecimal(dr["SO_TIEN_MAT"]);
                    obj.SO_TIEN_MAT_TRA_KH = Convert.ToDecimal(dr["SO_TIEN_MAT_TRA_KH"]);
                    obj.SO_TIEN_QDOI = Convert.ToDecimal(dr["SO_TIEN_QDOI"]);
                    obj.SO_TIEN_TRA_KH = Convert.ToDecimal(dr["SO_TIEN_TRA_KH"]);
                    obj.SO_TKHOAN_TRA_KH = Convert.ToString(dr["SO_TKHOAN_TRA_KH"]);
                    obj.TAT_TOAN_THAN = Convert.ToString(dr["TAT_TOAN_THAN"]);
                    obj.TEN_KHANG = Convert.ToString(dr["TEN_KHANG"]);
                    obj.TRA_NO_THAN = Convert.ToString(dr["TRA_NO_THAN"]);
                    obj.TTHAI_BGHI = Convert.ToString(dr["TTHAI_BGHI"]);
                    if (dr["TEN_TKHOAN"] != DBNull.Value && !dr["TEN_TKHOAN"].ToString().IsNullOrEmptyOrSpace())
                        lblTenTaiKhoan.Content = LLanguage.SearchResourceByKey(dr["TEN_TKHOAN"].ToString());
                    if (dr["TEN_TKHOAN_KH"] != DBNull.Value && !dr["TEN_TKHOAN_KH"].ToString().IsNullOrEmptyOrSpace())
                        lblTenTaiKhoanTraKH.Content = LLanguage.SearchResourceByKey(dr["TEN_TKHOAN_KH"].ToString());
                    grMain.DataContext = obj;
                    TThaiNVu = dr["TTHAI_NVU"].ToString();
                    lblLabelTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
                    CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDTD_THU_GOC_LAI);
                    if (action.Equals(DatabaseConstant.Action.SUA))
                        SetEnabledAllControls(true);
                    else
                        SetEnabledAllControls(false);
                }
                lstTTinThuNo = new List<TDTD_THONG_TIN_THU_NO>();
                dt = ds.Tables["TTIN_KHOACH"];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        objTTinThuNo = new TDTD_THONG_TIN_THU_NO();
                        objTTinThuNo.GOC_KH = Convert.ToDecimal(dr["GOC_KH"]);
                        objTTinThuNo.GOC_TT = Convert.ToDecimal(dr["GOC_TT"]);
                        objTTinThuNo.ID_KUOC = Convert.ToInt32(dr["ID_KUOC"]);
                        objTTinThuNo.LAI_KH = Convert.ToDecimal(dr["LAI_KH"]);
                        objTTinThuNo.LAI_PHAT_KH = Convert.ToDecimal(dr["LAI_PHAT_KH"]);
                        objTTinThuNo.LAI_PHAT_TT = Convert.ToDecimal(dr["LAI_PHAT_TT"]);
                        objTTinThuNo.LAI_TT = Convert.ToDecimal(dr["LAI_TT"]);
                        objTTinThuNo.MA_KUOC = Convert.ToString(dr["MA_KUOC"]);
                        objTTinThuNo.NGAY_KH = Convert.ToString(dr["NGAY_KH"]);
                        objTTinThuNo.NHOM_NO = Convert.ToString(dr["NHOM_NO"]);
                        objTTinThuNo.SO_NGAY_QH = Convert.ToInt32(dr["SO_NGAY_QH"]);
                        lstTTinThuNo.Add(objTTinThuNo);
                    }
                }
                grdThuGocLai.ItemsSource = null;
                grdThuGocLai.ItemsSource = lstTTinThuNo;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void SetTabThongKiemSoat(DataSet ds)
        {
            try
            {
                if (!LObject.IsNullOrEmpty(ds) && ds.Tables.Count > 1)
                {
                    DataTable dt = ds.Tables["TTIN_CHUNG"];
                    if (dt.Rows.Count > 0)
                    {
                        txtNguoiLap.Text = dt.Rows[0]["NGUOI_NHAP"].ToString();
                        teldtNgayNhap.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_NHAP"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                        txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(dt.Rows[0]["TTHAI_BGHI"].ToString());
                        txtNguoiCapNhat.Text = dt.Rows[0]["NGUOI_CNHAT"].ToString();
                        if (!dt.Rows[0]["NGAY_CNHAT"].ToString().IsNullOrEmptyOrSpace())
                            teldtNgayCNhat.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_CNHAT"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void beforeModify()
        {
            action = DatabaseConstant.Action.SUA;
            SetEnabledAllControls(true);
            OnModify();
        }

        private void SetEnabledAllControls(bool enable)
        {
            AutoCompleteEntry auHinhThuc = lstSourceHinhThucThanhToan.ElementAt(cmbHinhThucTToan.SelectedIndex);
            AutoCompleteEntry auHinhThucTra = lstSourceHinhThucThanhToan.ElementAt(cmbHinhThucTToanTra.SelectedIndex);
            txtDienGiai.IsEnabled = enable;
            txtSoKheUoc.IsEnabled = enable;
            chkHoanDuThu.IsEnabled = enable;
            chkTraNoTruocHan.IsEnabled = enable;
            chkTatToanTruocHan.IsEnabled = enable;
            grdThuGocLai.IsReadOnly = !enable;
            cmbHinhThucTToan.IsEnabled = enable;
            if (auHinhThuc.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TMAT_CKHOAN.layGiaTri()))
                telnumSoTienMat.IsEnabled = enable;
            else
                telnumSoTienMat.IsEnabled = false;
            telnumTraKHang.IsEnabled = enable;
            if (auHinhThucTra.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TMAT_CKHOAN.layGiaTri()))
                telnumSoTienMatTraKH.IsEnabled = enable;
            else
                telnumSoTienMatTraKH.IsEnabled = false;

        }

        private void Release()
        {
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDTD_THU_GOC_LAI,
            DatabaseConstant.Table.KT_GIAO_DICH,
            DatabaseConstant.Action.SUA,
            lstId);
        }

        void OnModify()
        {
            UtilitiesProcess process = new UtilitiesProcess();
            lstId = new List<int>();
            lstId.Add(iDGDich);
            bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDTD_THU_GOC_LAI,
            DatabaseConstant.Table.KT_GIAO_DICH,
            action,
            lstId);
            SetEnabledAllControls(true);
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDTD_THU_GOC_LAI);
        }

        bool Validation()
        {
            bool bReturn = true;
            if (obj.IsNullOrEmpty() || obj.ID_KUOC <= 0)
            {
                CommonFunction.ThongBaoTrong(lblSoKheUoc.Content.ToString());
                txtSoKheUoc.Focus();
                return false;
            }
            if (txtDienGiai.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblDienGiai.Content.ToString());
                txtDienGiai.Focus();
                return false;
            }
            return bReturn;
        }
        void BeforeSave(BusinessConstant.TrangThaiNghiepVu trangthai, BusinessConstant.TrangThaiBanGhi banghi)
        {
            try
            {
                Cursor = Cursors.Wait;
                if (!trangthai.Equals(BusinessConstant.TrangThaiNghiepVu.LUU_TAM))
                {
                    if (!Validation())
                        return;
                }
                GetDataForm(banghi, trangthai);
                OnSave();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        void OnSave()
        {

            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (txtSoGiaoDich.Text == "")
                iret = new TinDungTDProcess().ThuGocLaiVay(DatabaseConstant.Action.THEM, ref obj, ref lstResponseDetail);
            else
                iret = new TinDungTDProcess().ThuGocLaiVay(DatabaseConstant.Action.SUA, ref obj, ref lstResponseDetail);
            AfterSave(lstResponseDetail, iret);
        }

        void AfterSave(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            if (iret > 0)
            {
                action = DatabaseConstant.Action.XEM;
                SetInfomation();
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDTD_THU_GOC_LAI,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                lstId);
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
                if ((bool)cbMultiAdd.IsChecked)
                    ResetForm();
            }
            
        }

        void AfterDelete(string sSoGiaoDich, List<ClientResponseDetail> ResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(ResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDTD_THU_GOC_LAI,
            DatabaseConstant.Table.KT_GIAO_DICH,
            DatabaseConstant.Action.XOA,
            lstId);
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            if (iret < 1)
                SetInfomation();
            else
                ResetForm();
        }

        void OnDelete()
        {
            int iret = 0;
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            iret = new TinDungTDProcess().ThuGocLaiVay(DatabaseConstant.Action.XOA, ref obj, ref ResponseDetail);
            AfterDelete(txtSoGiaoDich.Text, ResponseDetail, iret);
        }

        void BeforeDelete()
        {
            if (tlbDelete.IsEnabled == false)
                return;
            try
            {
                Cursor = Cursors.Wait;
                if (!txtSoGiaoDich.Text.IsNullOrEmptyOrSpace())
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {

                        lstId = new List<int>();
                        lstId.Add(iDGDich);

                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDTD_THU_GOC_LAI,
                        DatabaseConstant.Table.KT_GIAO_DICH,
                        DatabaseConstant.Action.XOA,
                        lstId);
                        OnDelete();
                    }
                }
            }
            catch (Exception ex)
            {
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDTD_THU_GOC_LAI,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.XOA,
                lstId);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        void AfterApprove(string sSoGiaoDich, List<ClientResponseDetail> ResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(ResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDTD_THU_GOC_LAI,
            DatabaseConstant.Table.KT_GIAO_DICH,
            DatabaseConstant.Action.DUYET,
            lstId);
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            SetDataForm(sSoGiaoDich);
        }

        void OnApprove()
        {
            int iret = 0;
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            iret = new TinDungTDProcess().ThuGocLaiVay(DatabaseConstant.Action.DUYET, ref obj, ref ResponseDetail);
            AfterApprove(txtSoGiaoDich.Text, ResponseDetail);
        }

        void BeforeApprove()
        {
            if (!tlbApprove.IsEnabled)
                return;
            try
            {
                if (!txtSoGiaoDich.Text.IsNullOrEmptyOrSpace())
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        lstId = new List<int>();
                        lstId.Add(iDGDich);

                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDTD_THU_GOC_LAI,
                        DatabaseConstant.Table.KT_GIAO_DICH,
                        DatabaseConstant.Action.DUYET,
                        lstId);
                        OnApprove();
                    }
                }
            }
            catch (Exception ex)
            {
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDTD_THU_GOC_LAI,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.DUYET,
                lstId);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void AfterRefuse(string sSoGiaoDich, List<ClientResponseDetail> ResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(ResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDTD_THU_GOC_LAI,
            DatabaseConstant.Table.KT_GIAO_DICH,
            DatabaseConstant.Action.TU_CHOI_DUYET,
            lstId);
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            SetDataForm(sSoGiaoDich);
        }

        void OnRefuse()
        {
            int iret = 0;
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            iret = new TinDungTDProcess().ThuGocLaiVay(DatabaseConstant.Action.TU_CHOI_DUYET, ref obj, ref ResponseDetail);
            AfterRefuse(txtSoGiaoDich.Text, ResponseDetail);
        }

        void BeforeRefuse()
        {
            if (!tlbRefuse.IsEnabled)
                return;
            try
            {
                if (!txtSoGiaoDich.Text.IsNullOrEmptyOrSpace())
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {

                        lstId = new List<int>();
                        lstId.Add(iDGDich);

                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDTD_THU_GOC_LAI,
                        DatabaseConstant.Table.KT_GIAO_DICH,
                        DatabaseConstant.Action.TU_CHOI_DUYET,
                        lstId);
                        OnRefuse();
                    }
                }
            }
            catch (Exception ex)
            {
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDTD_THU_GOC_LAI,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }


        void AfterCancel(string sSoGiaoDich, List<ClientResponseDetail> ResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(ResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDTD_THU_GOC_LAI,
            DatabaseConstant.Table.KT_GIAO_DICH,
            DatabaseConstant.Action.THOAI_DUYET,
            lstId);
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            SetDataForm(sSoGiaoDich);
        }

        void OnCancel()
        {
            int iret = 0;
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            iret = new TinDungTDProcess().ThuGocLaiVay(DatabaseConstant.Action.THOAI_DUYET, ref obj, ref ResponseDetail);
            AfterCancel(txtSoGiaoDich.Text, ResponseDetail);
        }

        void BeforeCancel()
        {
            if (!tlbCancel.IsEnabled)
                return;
            try
            {
                if (!txtSoGiaoDich.Text.IsNullOrEmptyOrSpace())
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {

                        lstId = new List<int>();
                        lstId.Add(iDGDich);

                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDTD_THU_GOC_LAI,
                        DatabaseConstant.Table.KT_GIAO_DICH,
                        DatabaseConstant.Action.THOAI_DUYET,
                        lstId);
                        OnCancel();
                    }
                }
            }
            catch (Exception ex)
            {
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDTD_THU_GOC_LAI,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void OnPrint()
        {

        }

        private void OnPreviewDonVayVon()
        {

        }

        /// <summary>
        /// Xem báo cáo
        /// </summary>
        private void OnPreview()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(txtSoGiaoDich.Text))
            {
                LMessage.ShowMessage("Không có thông tin dữ liệu cần hiển thị", LMessage.MessageBoxType.Warning);

            }
            else
            {

                VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();

                GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.GDKT_IN_GIAO_DICH;
                objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDTD_THU_GOC_LAI;

                GDKT_GIAO_DICH objGDKT_GIAO_DICH = new GDKT_GIAO_DICH();
                objGDKT_GIAO_DICH.MaGiaoDich = txtSoGiaoDich.Text;

                doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                doiTuongBaoCao.objGDKT_GIAO_DICH = objGDKT_GIAO_DICH;

                List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
                listThamSoBaoCao.Add(new ThamSoBaoCao("@SoPhieu", txtSoGiaoDich.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                string maBaoCao = "GDKT_GIAO_DICH";
                xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
                xemBaoCao.LayDuLieu(maBaoCao, listThamSoBaoCao);

            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Lấy danh sách được chọn
        /// </summary>
        /// <returns></returns>
        private List<DataRow> getListSeletedDataRow()
        {
            List<DataRow> listDataRow = new List<DataRow>();
            return listDataRow;
        }

        private void SetInfomation()
        {
            try
            {
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(obj.TTHAI_NVU);
                action = DatabaseConstant.Action.XEM;
                TThaiNVu = obj.TTHAI_NVU;
                txtSoGiaoDich.Text = obj.MA_GDICH;
                CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDTD_THU_GOC_LAI);
                SetEnabledAllControls(false);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
        #endregion
    }
}
