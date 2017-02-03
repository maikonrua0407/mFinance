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
using PresentationWPF.CustomControl;
using Presentation.Process.UtilitiesServiceRef;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.Common;
using Presentation.Process.TinDungServiceRef;
using Telerik.Windows.Controls;
using Presentation.Process;
using System.Reflection;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;
using Telerik.Windows.Data;

namespace PresentationWPF.TinDung.DuThu
{
    /// <summary>
    /// Interaction logic for ucDuThuCT.xaml
    /// </summary>
    public partial class ucDuThuCT : UserControl
    {
         /// <summary>
        /// Khai bao
        /// </summary>
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
        TDVM_DU_THU TDVMDUTHU = null;
        int iDGiaoDich = 0;
        DANH_SACH_KHE_UOC_DU_THU objDuThu = null;
        List<DANH_SACH_KHE_UOC_DU_THU> lstDuThu = null;
        List<DANH_SACH_KHE_UOC_DU_THU> lstDuThuNgoaiBang = null;
        string TThaiNV = "";
        DatabaseConstant.Action action;
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        List<int> lstPopupKU = new List<int>();
        public void LayDuLieuTuPopup(List<int> lst)
        {
            lstPopupKU = lst;
        }

        private KIEM_SOAT _objKiemSoat = null;
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucDuThuCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDung;component/DuThu/ucDuThuCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            ClearForm();
            KhoiTaoBinding();
            tlbThemKUOC.Click += new RoutedEventHandler(tlbThemKUOC_Click);
            tlbXoaKUOC.Click +=new RoutedEventHandler(tlbXoaKUOC_Click);
            tlbCalculate.Click += new RoutedEventHandler(tlbCalculate_Click);
            tlbThemKUOCNgoaiBang.Click += new RoutedEventHandler(tlbThemKUOCNgoaiBang_Click);
            tlbXoaKUOCNgoaiBang.Click += new RoutedEventHandler(tlbXoaKUOCNgoaiBang_Click);
            tlbCalculateNgoaiBang.Click += new RoutedEventHandler(tlbCalculateNgoaiBang_Click);
            chkDuThuNgoaiBang.Click += new RoutedEventHandler(chkDuThuNgoaiBang_Click);
        }

        public ucDuThuCT(KIEM_SOAT objKiemSoat) : this()
        {
            _objKiemSoat = objKiemSoat;
            TDVMDUTHU.MA_GIAO_DICH = _objKiemSoat.SO_GIAO_DICH;
            TThaiNV = _objKiemSoat.TTHAI_NVU;
            action = _objKiemSoat.action;
            LoadDataForm();
            CommonFunction.RefreshButton(Toolbar, action, TThaiNV,mnuMain);
        }

        private void KhoiTaoBinding()
        {
            try
            {
                if (lstDuThu == null)
                {
                    lstDuThu = new List<DANH_SACH_KHE_UOC_DU_THU>();
                }
                raddgrDuThuDS.ItemsSource = lstDuThu;

                //MA 20130322
                var tongKH = new AggregateFunction<DANH_SACH_KHE_UOC_DU_THU, decimal>
                {
                    AggregationExpression = ku => ku.Sum(r => r.LAI_DU_THU + r.LUY_KE_KY_TRUOC),
                    ResultFormatString = "{0:n0}",
                    Caption = "Tổng: "
                };
                GridViewExpressionColumn columnKH = this.raddgrDuThuDS.Columns["TongTT"] as GridViewExpressionColumn;
                columnKH.AggregateFunctions.Add(tongKH);
                var tongTT = new AggregateFunction<DANH_SACH_KHE_UOC_DU_THU, decimal>
                {
                    AggregationExpression = ku => ku.Sum(r => r.LAI_DU_THU + r.LUY_KE_KY_TRUOC),
                    ResultFormatString = "{0:n0}",
                    Caption = "Tổng: "
                };
                GridViewExpressionColumn columnTT = this.raddgrDuThuDSNgoaiBang.Columns["TongTT"] as GridViewExpressionColumn;
                columnTT.AggregateFunctions.Add(tongTT);
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
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
                        KeyGesture keyg = new KeyGesture(Key.V, ModifierKeys.Control|ModifierKeys.Shift);
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
                        KeyGesture keyg = new KeyGesture(Key.W,ModifierKeys.Control | ModifierKeys.Shift);
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
                ClearForm();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                action = DatabaseConstant.Action.SUA;
                OnModify();
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
                BeforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                BeforeRefuse();
            }
            else if (strTinhNang.Equals("PreviewChungTu"))
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
                ClearForm();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                action = DatabaseConstant.Action.SUA;
                OnModify();
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
                BeforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                BeforeRefuse();
            }
            else if (strTinhNang.Equals("PreviewChungTu"))
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

        /// <summary>
        /// Sự kiện chọn ngày của DatetimePicker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DatePicker dtpControl = (DatePicker)sender;
                StringBuilder sbControl = new StringBuilder();
                sbControl.Append("teldt");
                sbControl.Append(dtpControl.Name.Substring(3));
                RadMaskedDateTimeInput telControl = (RadMaskedDateTimeInput)grMain.FindName(sbControl.ToString());
                if (telControl != null)
                    telControl.Value = dtpControl.SelectedDate;
                else
                    throw new System.NullReferenceException(LLanguage.SearchResourceByKey("M.TinDung.DuThu.ucDuThuCT.KhongTimThayControl") + sbControl.ToString());
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void ClearForm()
        {
            txtSoGD.Text = "";
            txtDienGiai.Text = "";
            lstDuThu = new List<DANH_SACH_KHE_UOC_DU_THU>();
            lstDuThuNgoaiBang = new List<DANH_SACH_KHE_UOC_DU_THU>();
            iDGiaoDich = 0;
            TDVMDUTHU = new TDVM_DU_THU();
            teldtDuThuDenNgay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            string ThamSo = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.DV, BusinessConstant.MaThamSo.DV_DU_THU_DEN_NGAY, ClientInformation.MaDonVi);
            if (ThamSo.Equals("NGAY_GIAO_DICH"))
                teldtNgayGD.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            else
                teldtNgayGD.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecSau, ApplicationConstant.defaultDateTimeFormat);
            numTyLe.Value = 100;
            TThaiNV = "";
            txtNguoiCapNhat.Text = "";
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            txtTrangThai.Text = "";
            lblTrangThai.Content = "";
            teldtNgayCNhat.Value = null;
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            raddgrDuThuDS.ItemsSource = lstDuThu;
            action = DatabaseConstant.Action.THEM;
            CommonFunction.RefreshButton(Toolbar, action, TThaiNV,mnuMain);
        }

        private void tlbXoaKUOC_Click(object sender, RoutedEventArgs e)
        {
            if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            List<int> idXoa = new List<int>();
            foreach (DANH_SACH_KHE_UOC_DU_THU obj in raddgrDuThuDS.SelectedItems)
            {
                idXoa.Add(obj.ID_KHE_UOC);
            }
            lstDuThu.RemoveAll(f => idXoa.Contains(f.ID_KHE_UOC));
            raddgrDuThuDS.ItemsSource = null;
            raddgrDuThuDS.ItemsSource = lstDuThu;
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            List<ClientResponseDetail> lstClientResponseDetail = new List<ClientResponseDetail>();
            lstDuThu = (List<DANH_SACH_KHE_UOC_DU_THU>)raddgrDuThuDS.ItemsSource;
            TDVMDUTHU.DSACH_KHE_UOC = lstDuThu.ToArray();
            TDVMDUTHU.TY_LE = (decimal)numTyLe.Value.GetValueOrDefault(0);
            TDVMDUTHU.DU_THU_DEN_NGAY = teldtDuThuDenNgay.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
            int iret = new TinDungProcess().TinhToanDuThuTinDungTrongHan(ref TDVMDUTHU, ref lstClientResponseDetail);
            lstDuThu = TDVMDUTHU.DSACH_KHE_UOC.ToList();
            CommonFunction.ThongBaoKetQua(lstClientResponseDetail);
            raddgrDuThuDS.ItemsSource = null;
            raddgrDuThuDS.ItemsSource = lstDuThu;
        }

        void chkDuThuNgoaiBang_Click(object sender, RoutedEventArgs e)
        {
            if (chkDuThuNgoaiBang.IsChecked.GetValueOrDefault())
                grbNhomNoCao.Visibility = System.Windows.Visibility.Visible;
            else
                grbNhomNoCao.Visibility = System.Windows.Visibility.Collapsed;
        }

        void tlbCalculateNgoaiBang_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> lstClientResponseDetail = new List<ClientResponseDetail>();
                lstDuThuNgoaiBang = (List<DANH_SACH_KHE_UOC_DU_THU>)raddgrDuThuDSNgoaiBang.ItemsSource;
                if (lstDuThuNgoaiBang.IsNullOrEmpty())
                {
                    CommonFunction.ThongBaoChuaNhap(grbNhomNoCao.Header.ToString() + ":");
                    return;
                }
                TDVMDUTHU.DSACH_KHE_UOC_NGOAI_BANG = lstDuThuNgoaiBang.ToArray();
                TDVMDUTHU.TY_LE = (decimal)numTyLe.Value.GetValueOrDefault(0);
                TDVMDUTHU.DU_THU_DEN_NGAY = teldtDuThuDenNgay.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                int iret = new TinDungProcess().TinhToanDuThuTinDungQuaHan(ref TDVMDUTHU, ref lstClientResponseDetail);
                lstDuThuNgoaiBang = TDVMDUTHU.DSACH_KHE_UOC_NGOAI_BANG.ToList();
                CommonFunction.ThongBaoKetQua(lstClientResponseDetail);
                raddgrDuThuDSNgoaiBang.ItemsSource = null;
                raddgrDuThuDSNgoaiBang.ItemsSource = lstDuThuNgoaiBang;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        void tlbXoaKUOCNgoaiBang_Click(object sender, RoutedEventArgs e)
        {
            if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            List<int> idXoa = new List<int>();
            foreach (DANH_SACH_KHE_UOC_DU_THU obj in raddgrDuThuDSNgoaiBang.SelectedItems)
            {
                idXoa.Add(obj.ID_KHE_UOC);
            }
            lstDuThuNgoaiBang.RemoveAll(f => idXoa.Contains(f.ID_KHE_UOC));
            raddgrDuThuDSNgoaiBang.ItemsSource = null;
            raddgrDuThuDSNgoaiBang.ItemsSource = lstDuThuNgoaiBang;
        }

        void tlbThemKUOCNgoaiBang_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            try
            {
                string sidKheUoc = "";
                foreach (DANH_SACH_KHE_UOC_DU_THU dr in lstDuThuNgoaiBang)
                {
                    sidKheUoc += "," + dr.ID_KHE_UOC.ToString();
                }
                if (sidKheUoc.Length > 0)
                    sidKheUoc = sidKheUoc.Substring(1);
                else
                    sidKheUoc = "0";
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(sidKheUoc);
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add("KUOCDTHUNGOAIBANG");
                lstDieuKien.Add(LDateTime.DateToString(teldtDuThuDenNgay.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat));
                lstDieuKien.Add("%");
                lstDieuKien.Add("''NHOM3'',''NHOM4'',''NHOM5''");
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                ApplicationConstant.DonViSuDung donViSuDung = ApplicationConstant.layDonViSuDung(ClientInformation.Company);
                switch (donViSuDung)
                {
                    case ApplicationConstant.DonViSuDung.BIDV:
                        popupProcess.getPopupInformation("POPUP_DS_KHEUOC_DU_THU", lstDieuKien);
                        break;
                    case ApplicationConstant.DonViSuDung.BIDV_BLF:
                        popupProcess.getPopupInformation("POPUP_DS_KHEUOC_DU_THU", lstDieuKien);
                        break;
                    default:
                        popupProcess.getPopupInformation("POPUP_DS_KHEUOC", lstDieuKien);
                        break;
                }
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopupKheUocViMo popup = new ucPopupKheUocViMo(true, simplePopupResponse, true);
                popup.LayGiaTriListDataRow = new ucPopupKheUocViMo.LayListDataRow(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Content = popup;
                win.ShowDialog();
                if (lstPopup.Count > 0)
                {
                    if (lstDuThuNgoaiBang.IsNullOrEmpty())
                        lstDuThuNgoaiBang = new List<DANH_SACH_KHE_UOC_DU_THU>();
                    foreach (DataRow drv in lstPopup)
                    {
                        objDuThu = new DANH_SACH_KHE_UOC_DU_THU();
                        objDuThu.ID_KHE_UOC = Convert.ToInt32(drv["ID_KHE_UOC"]);
                        objDuThu.MA_KHACH_HANG = drv["MA_KHACH_HANG"].ToString();
                        objDuThu.MA_KHE_UOC = drv["MA_KHE_UOC"].ToString();
                        objDuThu.NGAY_DU_THU_TRUOC = drv["NGAY_DU_THU_TRUOC"].ToString();
                        objDuThu.TEN_KHACH_HANG = drv["TEN_KHACH_HANG"].ToString();
                        objDuThu.SO_DU = Convert.ToDecimal(drv["SO_DU"]);
                        objDuThu.LAI_SUAT = Convert.ToDecimal(drv["LAI_SUAT"]);
                        objDuThu.NGAY_VAY = drv["NGAY_VAY"].ToString();
                        objDuThu.SO_NGAY = Convert.ToInt32(drv["SO_NGAY"]);
                        objDuThu.LUY_KE_KY_TRUOC = Convert.ToDecimal(drv["DU_THU_LKE"]);
                        objDuThu.PTHUC_TLAI = drv["PTHUC_TLAI"].ToString();
                        objDuThu.MA_SAN_PHAM = drv["MA_SAN_PHAM"].ToString();
                        objDuThu.NGUON_VON = drv["NV_LOAI_NVON"].ToString();
                        lstDuThuNgoaiBang.Add(objDuThu);
                    }
                    raddgrDuThuDSNgoaiBang.ItemsSource = null;
                    raddgrDuThuDSNgoaiBang.ItemsSource = lstDuThuNgoaiBang;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Cursor = Cursors.Arrow;
        }

        void tlbCalculate_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            try
            {
                List<ClientResponseDetail> lstClientResponseDetail = new List<ClientResponseDetail>();
                lstDuThu = (List<DANH_SACH_KHE_UOC_DU_THU>)raddgrDuThuDS.ItemsSource;
                if (lstDuThu.IsNullOrEmpty())
                {
                    CommonFunction.ThongBaoChuaNhap(grbDSachDuThu.Header.ToString() + ":");
                    return;
                }
                TDVMDUTHU.DSACH_KHE_UOC = lstDuThu.ToArray();
                TDVMDUTHU.TY_LE = (decimal)numTyLe.Value.GetValueOrDefault(0);
                TDVMDUTHU.DU_THU_DEN_NGAY = teldtDuThuDenNgay.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                int iret = new TinDungProcess().TinhToanDuThuTinDungTrongHan(ref TDVMDUTHU, ref lstClientResponseDetail);
                lstDuThu = TDVMDUTHU.DSACH_KHE_UOC.ToList();
                CommonFunction.ThongBaoKetQua(lstClientResponseDetail);
                raddgrDuThuDS.ItemsSource = null;
                raddgrDuThuDS.ItemsSource = lstDuThu;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        void tlbThemKUOC_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            try
            {
                string sidKheUoc = "";
                foreach (DANH_SACH_KHE_UOC_DU_THU dr in lstDuThu)
                {
                    sidKheUoc += "," + dr.ID_KHE_UOC.ToString();
                }
                if (sidKheUoc.Length > 0)
                    sidKheUoc = sidKheUoc.Substring(1);
                else
                    sidKheUoc = "0";
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(sidKheUoc);
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add("KUOCDTHUNBANG");
                lstDieuKien.Add(LDateTime.DateToString(teldtDuThuDenNgay.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat));
                lstDieuKien.Add("%");
                lstDieuKien.Add("''NHOM1'',''NHOM2''");
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                ApplicationConstant.DonViSuDung donViSuDung = ApplicationConstant.layDonViSuDung(ClientInformation.Company);
                switch (donViSuDung)
                {
                    case ApplicationConstant.DonViSuDung.BIDV:
                        popupProcess.getPopupInformation("POPUP_DS_KHEUOC_DU_THU", lstDieuKien);
                        break;
                    case ApplicationConstant.DonViSuDung.BIDV_BLF:
                        popupProcess.getPopupInformation("POPUP_DS_KHEUOC_DU_THU", lstDieuKien);
                        break;
                    default:
                        popupProcess.getPopupInformation("POPUP_DS_KHEUOC", lstDieuKien);
                        break;
                }
                
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopupKheUocViMo popup = new ucPopupKheUocViMo(true, simplePopupResponse, true);
                popup.LayGiaTriListDataRow = new ucPopupKheUocViMo.LayListDataRow(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Content = popup;
                win.ShowDialog();
                if (lstPopup.Count > 0)
                {
                    if (lstDuThu.IsNullOrEmpty())
                        lstDuThu = new List<DANH_SACH_KHE_UOC_DU_THU>();
                    foreach (DataRow drv in lstPopup)
                    {
                        objDuThu = new DANH_SACH_KHE_UOC_DU_THU();
                        objDuThu.ID_KHE_UOC = Convert.ToInt32(drv["ID_KHE_UOC"]);
                        objDuThu.MA_KHACH_HANG = drv["MA_KHACH_HANG"].ToString();
                        objDuThu.MA_KHE_UOC = drv["MA_KHE_UOC"].ToString();
                        objDuThu.NGAY_DU_THU_TRUOC = drv["NGAY_DU_THU_TRUOC"].ToString();
                        objDuThu.TEN_KHACH_HANG = drv["TEN_KHACH_HANG"].ToString();
                        objDuThu.SO_DU = Convert.ToDecimal(drv["SO_DU"]);
                        objDuThu.LAI_SUAT = Convert.ToDecimal(drv["LAI_SUAT"]);
                        objDuThu.NGAY_VAY = drv["NGAY_VAY"].ToString();
                        objDuThu.SO_NGAY = Convert.ToInt32(drv["SO_NGAY"]);
                        objDuThu.LUY_KE_KY_TRUOC = Convert.ToDecimal(drv["DU_THU_LKE"]);
                        objDuThu.PTHUC_TLAI = drv["PTHUC_TLAI"].ToString();
                        objDuThu.MA_SAN_PHAM = drv["MA_SAN_PHAM"].ToString();
                        objDuThu.NGUON_VON = drv["NV_LOAI_NVON"].ToString();
                        objDuThu.LAI_DU_THU = 0;
                        lstDuThu.Add(objDuThu);
                    }
                    raddgrDuThuDS.ItemsSource = null;
                    raddgrDuThuDS.ItemsSource = lstDuThu;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
            
        }
        #endregion

        /// <summary>
        /// Xu ly nghiep vu
        /// </summary>
        #region Xu ly nghiep vu
        void LoadGridKheUoc()
        {
            raddgrDuThuDS.ItemsSource = null;
            raddgrDuThuDS.ItemsSource = lstDuThu;
        }

        void BeforeSave(BusinessConstant.TrangThaiNghiepVu nghiepvu, BusinessConstant.TrangThaiBanGhi bghi)
        {
            if(!nghiepvu.Equals(BusinessConstant.TrangThaiNghiepVu.LUU_TAM))
            {
                if(!VaditionData())
                    return;
            }
            List<int> lstId = new List<int>();
            lstId.Add(iDGiaoDich);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_DU_THU,
            DatabaseConstant.Table.KT_GIAO_DICH,
            DatabaseConstant.Action.SUA,
            lstId);
            GetFormData(nghiepvu,bghi);
            OnSave();
        }

        void OnSave()
        {
            List<ClientResponseDetail> lstResponse = new List<ClientResponseDetail>();
            int iret = 0;
            if (iDGiaoDich == 0)
                iret = new TinDungProcess().ThemMoiGiaoDichDuThu(ref TDVMDUTHU, ref lstResponse);
            else
                iret = new TinDungProcess().SuaGiaoDichDuThu(ref TDVMDUTHU, ref lstResponse);
            AfterSave(lstResponse, iret);
        }

        void AfterSave(List<ClientResponseDetail> lstResponse, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponse);
            if (iret > 0)
            {
                action = DatabaseConstant.Action.XEM;
                List<int> lstId = new List<int>();
                lstId.Add(TDVMDUTHU.ID_GIAO_DICH);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DU_THU,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                lstId);
                LoadDataForm();
                CommonFunction.RefreshButton(Toolbar, action, TThaiNV,mnuMain);
                if ((bool)cbMultiAdd.IsChecked)
                    ClearForm();
            }
        }

        void BeforeDelete()
        {
            if (iDGiaoDich > 0)
            {
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DU_THU,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.XOA,
                lstId);
                OnDelete();
            }
        }

        void OnDelete()
        {
            if (iDGiaoDich > 0)
            {
                TDVMDUTHU.ID_GIAO_DICH = iDGiaoDich;
                TDVMDUTHU.MA_GIAO_DICH = txtSoGD.Text;
                TDVMDUTHU.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                TDVMDUTHU.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                List<ClientResponseDetail> lstClientDetail = new List<ClientResponseDetail>();
                int iret = new TinDungProcess().XoaGiaoDichDuThu(ref TDVMDUTHU, ref lstClientDetail);
                AfterDelete(lstClientDetail, iret);
            }
        }

        void AfterDelete(List<ClientResponseDetail> lstResponse, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponse);
            if (iret > 0)
            {
                List<int> lstId = new List<int>();
                lstId.Add(TDVMDUTHU.ID_GIAO_DICH);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DU_THU,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.XOA,
                lstId);

                if (iret > 0)
                {
                    CommonFunction.CloseUserControl(this);
                    return;
                }
                if ((bool)cbMultiAdd.IsChecked)
                    ClearForm();
            }
        }

        void BeforeApprove()
        {
            if (iDGiaoDich > 0)
            {
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DU_THU,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.DUYET,
                lstId);
                OnApprove();
            }
        }

        void OnApprove()
        {
            if (iDGiaoDich > 0)
            {
                TDVMDUTHU.ID_GIAO_DICH = iDGiaoDich;
                TDVMDUTHU.MA_GIAO_DICH = txtSoGD.Text;
                TDVMDUTHU.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                TDVMDUTHU.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                List<ClientResponseDetail> lstClientDetail = new List<ClientResponseDetail>();
                int iret = new TinDungProcess().DuyetGiaoDichDuThu(ref TDVMDUTHU, ref lstClientDetail);
                AfterApprove(lstClientDetail, iret);
            }
        }

        void AfterApprove(List<ClientResponseDetail> lstResponse, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponse);
            if (iret > 0)
            {
                action = DatabaseConstant.Action.XEM;
                List<int> lstId = new List<int>();
                lstId.Add(TDVMDUTHU.ID_GIAO_DICH);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DU_THU,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.DUYET,
                lstId);
                LoadDataForm();
                CommonFunction.RefreshButton(Toolbar, action, TThaiNV,mnuMain);

                if ((bool)cbMultiAdd.IsChecked)
                    ClearForm();
            }
        }

        void BeforeRefuse()
        {
            if (iDGiaoDich > 0)
            {
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DU_THU,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);
                OnRefuse();
            }
        }

        void OnRefuse()
        {
            if (iDGiaoDich > 0)
            {
                TDVMDUTHU.ID_GIAO_DICH = iDGiaoDich;
                TDVMDUTHU.MA_GIAO_DICH = txtSoGD.Text;
                TDVMDUTHU.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                TDVMDUTHU.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                List<ClientResponseDetail> lstClientDetail = new List<ClientResponseDetail>();
                int iret = new TinDungProcess().TuChoiGiaoDichDuThu(ref TDVMDUTHU, ref lstClientDetail);
                AfterRefuse(lstClientDetail, iret);
            }
        }

        void AfterRefuse(List<ClientResponseDetail> lstResponse, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponse);
            if (iret > 0)
            {
                action = DatabaseConstant.Action.XEM;
                List<int> lstId = new List<int>();
                lstId.Add(TDVMDUTHU.ID_GIAO_DICH);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DU_THU,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);
                LoadDataForm();
                CommonFunction.RefreshButton(Toolbar, action, TThaiNV,mnuMain);

                if ((bool)cbMultiAdd.IsChecked)
                    ClearForm();
            }
        }

        void BeforeCancel()
        {
            if (iDGiaoDich > 0)
            {
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DU_THU,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                OnCancel();
            }
        }

        void OnCancel()
        {
            if (iDGiaoDich > 0)
            {
                TDVMDUTHU.ID_GIAO_DICH = iDGiaoDich;
                TDVMDUTHU.MA_GIAO_DICH = txtSoGD.Text;
                TDVMDUTHU.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                TDVMDUTHU.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                List<ClientResponseDetail> lstClientDetail = new List<ClientResponseDetail>();
                int iret = new TinDungProcess().ThoaiDuyetGiaoDichDuThu(ref TDVMDUTHU, ref lstClientDetail);
                AfterCancel(lstClientDetail, iret);
            }
        }

        void AfterCancel(List<ClientResponseDetail> lstResponse, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponse);
            if (iret > 0)
            {
                action = DatabaseConstant.Action.XEM;
                List<int> lstId = new List<int>();
                lstId.Add(TDVMDUTHU.ID_GIAO_DICH);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DU_THU,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                LoadDataForm();
                CommonFunction.RefreshButton(Toolbar, action, TThaiNV,mnuMain);

                if ((bool)cbMultiAdd.IsChecked)
                    ClearForm();
            }
        }
        void GetFormData(BusinessConstant.TrangThaiNghiepVu nghiepvu, BusinessConstant.TrangThaiBanGhi bghi)
        {
            TDVMDUTHU = new TDVM_DU_THU();
            
            if (lstDuThu.IsNullOrEmpty())
                lstDuThu = new List<DANH_SACH_KHE_UOC_DU_THU>();
            if (lstDuThuNgoaiBang.IsNullOrEmpty())
                lstDuThuNgoaiBang = new List<DANH_SACH_KHE_UOC_DU_THU>();
            if(!chkDuThuNgoaiBang.IsChecked.GetValueOrDefault())
                lstDuThuNgoaiBang = new List<DANH_SACH_KHE_UOC_DU_THU>();
            TDVMDUTHU.DSACH_KHE_UOC = lstDuThu.ToArray();
            TDVMDUTHU.DSACH_KHE_UOC_NGOAI_BANG = lstDuThuNgoaiBang.ToArray();
            TDVMDUTHU.MA_GIAO_DICH = txtSoGD.Text;
            TDVMDUTHU.ID_GIAO_DICH = iDGiaoDich;
            TDVMDUTHU.DIEN_GIAI = txtDienGiai.Text;
            TDVMDUTHU.DU_THU_DEN_NGAY = LDateTime.DateToString(teldtDuThuDenNgay.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
            TDVMDUTHU.LOAI_TIEN = ClientInformation.MaDongNoiTe;
            TDVMDUTHU.MA_DVI = ClientInformation.MaDonViGiaoDich;
            TDVMDUTHU.NGAY_GIAO_DICH = LDateTime.DateToString(teldtNgayGD.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
            TDVMDUTHU.TRANG_THAI_BAN_GHI = bghi.layGiaTri();
            TDVMDUTHU.TRANG_THAI_NGHIEP_VU = nghiepvu.layGiaTri();
            TDVMDUTHU.TY_LE = (decimal)numTyLe.Value.GetValueOrDefault(100);
            TDVMDUTHU.NGAY_LAP = ClientInformation.NgayLamViecHienTai;
            TDVMDUTHU.NGUOI_LAP = ClientInformation.TenDangNhap;
            if (chkDuThuNgoaiBang.IsChecked.GetValueOrDefault())
                TDVMDUTHU.DU_THU_NGOAI_BANG = BusinessConstant.CoKhong.CO.layGiaTri();
            else
                TDVMDUTHU.DU_THU_NGOAI_BANG = BusinessConstant.CoKhong.KHONG.layGiaTri();
            if (iDGiaoDich>0)
            {
                TDVMDUTHU.NGAY_LAP = LDateTime.DateToString(teldtNgayNhap.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat); ;
                TDVMDUTHU.NGUOI_LAP = txtNguoiLap.Text;
                TDVMDUTHU.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                TDVMDUTHU.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
            }
        }
        void LoadDataForm()
        {
            DataSet ds = new TinDungProcess().GetThongTinChiTietGDichDuThu(TDVMDUTHU.MA_GIAO_DICH);
            if(ds!=null)
            {
                LoadTabThongTinChung(ds);
                LoadTabKiemSoat(ds);
                OnModify();
            }
        }
        void LoadTabThongTinChung(DataSet ds)
        {
            DataTable dt = ds.Tables["TTIN_CTIET"];
            if(dt!=null)
            {
                txtSoGD.Text = TDVMDUTHU.MA_GIAO_DICH;
                teldtNgayGD.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_GIAO_DICH"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                teldtDuThuDenNgay.Value = LDateTime.StringToDate(dt.Rows[0]["DTHU_DEN_NGAY"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                numTyLe.Value = Convert.ToDouble(dt.Rows[0]["TY_LE"]);
                txtDienGiai.Text = dt.Rows[0]["DIEN_GIAI"].ToString();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(dt.Rows[0]["TTHAI_NVU"].ToString());
                iDGiaoDich = Convert.ToInt32(dt.Rows[0]["ID"]);
                chkDuThuNgoaiBang.IsChecked = Convert.ToBoolean(dt.Rows[0]["DU_THU_NGOAI_BANG"]);
                if (chkDuThuNgoaiBang.IsChecked.GetValueOrDefault())
                    grbNhomNoCao.Visibility = System.Windows.Visibility.Visible;
                else
                    grbNhomNoCao.Visibility = System.Windows.Visibility.Collapsed;
            }
            dt = ds.Tables["KHE_UOC"];
            if (dt != null && dt.Rows.Count > 0)
                {
                    lstDuThu = new List<DANH_SACH_KHE_UOC_DU_THU>();
                    foreach(DataRow dr in dt.Rows)
                    {
                        DANH_SACH_KHE_UOC_DU_THU objDSKUoc = new DANH_SACH_KHE_UOC_DU_THU();
                        foreach(DataColumn dc in dt.Columns)
                        {
                            PropertyInfo proper = objDSKUoc.GetType().GetProperty(dc.ColumnName);
                            if(proper!=null)
                            {
                                if(proper.PropertyType==typeof(decimal))
                                    proper.SetValue(objDSKUoc, Convert.ToDecimal(dr[dc.ColumnName]), null);
                                else if(proper.PropertyType==typeof(int))
                                    proper.SetValue(objDSKUoc, Convert.ToInt32(dr[dc.ColumnName]), null);
                                else if(proper.PropertyType==typeof(string))
                                    proper.SetValue(objDSKUoc, Convert.ToString(dr[dc.ColumnName]), null);
                            }
                                
                        }
                        lstDuThu.Add(objDSKUoc);
                    }
                    LoadGridKheUoc();
                }
            dt = ds.Tables["KHE_UOC_NGOAI_BANG"];
            if (dt != null && dt.Rows.Count > 0)
            {
                lstDuThuNgoaiBang = new List<DANH_SACH_KHE_UOC_DU_THU>();
                foreach (DataRow dr in dt.Rows)
                {
                    DANH_SACH_KHE_UOC_DU_THU objDSKUoc = new DANH_SACH_KHE_UOC_DU_THU();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        PropertyInfo proper = objDSKUoc.GetType().GetProperty(dc.ColumnName);
                        if (proper != null)
                        {
                            if (proper.PropertyType == typeof(decimal))
                                proper.SetValue(objDSKUoc, Convert.ToDecimal(dr[dc.ColumnName]), null);
                            else if (proper.PropertyType == typeof(int))
                                proper.SetValue(objDSKUoc, Convert.ToInt32(dr[dc.ColumnName]), null);
                            else if (proper.PropertyType == typeof(string))
                                proper.SetValue(objDSKUoc, Convert.ToString(dr[dc.ColumnName]), null);
                        }

                    }
                    lstDuThuNgoaiBang.Add(objDSKUoc);
                }
                raddgrDuThuDSNgoaiBang.ItemsSource = null;
                raddgrDuThuDSNgoaiBang.ItemsSource = lstDuThuNgoaiBang;
            }
        }
        void LoadTabKiemSoat(DataSet ds)
        {
            DataTable dt = ds.Tables["TTIN_CTIET"];
            if (dt != null && dt.Rows.Count > 0)
            {
                txtNguoiLap.Text = dt.Rows[0]["NGUOI_NHAP"].ToString();
                txtNguoiCapNhat.Text = dt.Rows[0]["NGUOI_CNHAT"].ToString();
                teldtNgayNhap.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_NHAP"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                if (!dt.Rows[0]["NGAY_CNHAT"].ToString().IsNullOrEmptyOrSpace())
                    teldtNgayCNhat.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_CNHAT"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(dt.Rows[0]["TTHAI_BGHI"].ToString());
                TThaiNV = dt.Rows[0]["TTHAI_NVU"].ToString();
            }
        }
        bool VaditionData()
        {
            lstDuThu = (List<DANH_SACH_KHE_UOC_DU_THU>)raddgrDuThuDS.ItemsSource;
            lstDuThuNgoaiBang = (List<DANH_SACH_KHE_UOC_DU_THU>)raddgrDuThuDSNgoaiBang.ItemsSource;
            if (lstDuThu.IsNullOrEmpty() && lstDuThuNgoaiBang.IsNullOrEmpty())
            {
                CommonFunction.ThongBaoTrong(grbDSachDuThu.Content.ToString());
                return false;
            }
            return true;
        }
        void OnModify()
        {
            if (action.Equals(DatabaseConstant.Action.SUA))
                EnableAllControl(true);
            else
                EnableAllControl(false);
            CommonFunction.RefreshButton(Toolbar, action, TThaiNV,mnuMain);
        }

        void EnableAllControl(bool bBool)
        {
            grbThongTinChung.IsEnabled = bBool;
            tlbThemKUOC.IsEnabled = bBool;
            tlbXoaKUOC.IsEnabled = bBool;
            tlbCalculate.IsEnabled = bBool;
            tlbThemKUOCNgoaiBang.IsEnabled = bBool;
            tlbXoaKUOCNgoaiBang.IsEnabled = bBool;
            tlbCalculateNgoaiBang.IsEnabled = bBool;
            raddgrDuThuDS.IsReadOnly = !bBool;
        }

        void ReleaseForm()
        {
            if (iDGiaoDich > 0)
            {
                List<int> lstId = new List<int>();
                lstId.Add(TDVMDUTHU.ID_GIAO_DICH);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_DU_THU,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                lstId);
            }
        }

        private void OnPreview()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(txtSoGD.Text))
            {
                LMessage.ShowMessage("Không có thông tin dữ liệu cần hiển thị", LMessage.MessageBoxType.Warning);

            }
            else
            {
                VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();

                GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.GDKT_IN_GIAO_DICH;
                DatabaseConstant.Function _function = DatabaseConstant.Function.KT_PHIEU_KE_TOAN;

                objGIAO_DICH_BASE.ChucNang = _function;

                GDKT_GIAO_DICH objGDKT_GIAO_DICH = new GDKT_GIAO_DICH();
                objGDKT_GIAO_DICH.MaGiaoDich = txtSoGD.Text;

                doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                doiTuongBaoCao.objGDKT_GIAO_DICH = objGDKT_GIAO_DICH;
                xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }


        #endregion 
    }
}
