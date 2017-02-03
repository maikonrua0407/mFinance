using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using System.Reflection;
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using PresentationWPF.CustomControl;
using Telerik.Windows.Controls;
using System.Data;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.KeToanServiceRef;
using Presentation.Process.Common;
using Telerik.Windows.Data;
using Telerik.Windows.Controls.GridView;
using System.ComponentModel;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;

namespace PresentationWPF.KeToan.PhieuKeToan
{
    /// <summary>
    /// Interaction logic for ucPhieuKeToanCT.xaml
    /// </summary>
    public partial class ucPhieuKeToanCT : UserControl
    {
        /// <summary>
        /// Khai bao
        /// </summary>
        #region Khai bao
        //public static RoutedCommand ImportCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand CloneCommand = new RoutedCommand();
        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand CashStmtCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand PreviewCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        private string formCase = Presentation.Process.Common.ClientInformation.FormCase;

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        private DataTable _dtSource = null;

        private string tthaiNvu = "";

        private string maGiaoDich = "";

        public event EventHandler OnSavingCompleted;

        private KIEM_SOAT _objKiemSoat = null;

        private DatabaseConstant.Function _function = DatabaseConstant.Function.KT_PHIEU_KE_TOAN;
        private Presentation.Process.KeToanServiceRef.KT_GIAO_DICH_DTUONG _objDTuong = new Presentation.Process.KeToanServiceRef.KT_GIAO_DICH_DTUONG();
        private Presentation.Process.KeToanServiceRef.KT_GIAO_DICH_DTUONG_DTO _objDTuongDTO = new Presentation.Process.KeToanServiceRef.KT_GIAO_DICH_DTUONG_DTO();
        List<AutoCompleteEntry> lstSourceNguonVon = new List<AutoCompleteEntry>();

        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucPhieuKeToanCT()
        {
            KhoiTaoChung();
            GanFunctionTheoFormCase();

            this.raddgrHachToan.KeyboardCommandProvider = new CustomControl.CustomGridViewKeyboardCommand(this.raddgrHachToan);
            ResetForm();
            beforeAddNew();
        }

        public ucPhieuKeToanCT(KIEM_SOAT obj, string LoaiPhieu)
        {
            _objKiemSoat = obj;
            formCase = LoaiPhieu;
            maGiaoDich = _objKiemSoat.SO_GIAO_DICH;

            KhoiTaoChung();
            GanFunctionTheoFormCase();
            raddgrHachToan.ItemsSource = _dtSource.DefaultView;

            beforeModifyFromList(_objKiemSoat.action);
        }

        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.KeToan.PhieuKeToan.ucPhieuKeToanCT", "");
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
        }

        private void GanFunctionTheoFormCase()
        {

            if (!LString.IsNullOrEmptyOrSpace(formCase))
            {
                formCase = Presentation.Process.Common.ClientInformation.FormCase;

                if (formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_THU.layGiaTri()))
                {
                    _function = DatabaseConstant.Function.KT_PHIEU_THU;
                }
                else if (formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_CHI.layGiaTri()))
                {
                    _function = DatabaseConstant.Function.KT_PHIEU_CHI;
                }
                else if (formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_KETOAN.layGiaTri()))
                {
                    _function = DatabaseConstant.Function.KT_PHIEU_KE_TOAN;
                }
            }
        }

        /// <summary>
        /// Khởi tạo cấu trúc datasource
        /// </summary>
        private void KhoiTaoDataSource()
        {
            Presentation.Process.KeToanProcess process = new Presentation.Process.KeToanProcess();
            try
            {
                
                _dtSource = new DataTable();
                _dtSource.Columns.Add("STT", typeof(int));
                _dtSource.Columns.Add("ID_MA_PLOAI", typeof(int));
                _dtSource.Columns.Add("MA_PLOAI", typeof(string));
                _dtSource.Columns.Add("SO_TAI_KHOAN", typeof(string));
                _dtSource.Columns.Add("TEN_TAI_KHOAN", typeof(string));
                _dtSource.Columns.Add("GHI_NO", typeof(decimal));
                _dtSource.Columns.Add("GHI_CO", typeof(decimal));
                DataColumn dtc = new DataColumn("NHOM_DKHOAN", typeof(string));
                dtc.DefaultValue = "1";
                _dtSource.Columns.Add(dtc);
                _dtSource.Columns.Add("LOAI_DTUONG", typeof(string));
                _dtSource.Columns.Add("MA_DTUONG", typeof(string));
                _dtSource.Columns.Add("MA_TCHAT_GOC", typeof(string));
                _dtSource.Columns.Add("MA_KY_HIEU", typeof(string));

                if (_objKiemSoat != null)
                {
                    DataSet dt = process.getThongTinMaPhieuKeToanTheoMa(_objKiemSoat.SO_GIAO_DICH, ClientInformation.MaDonViGiaoDich);
                    if (dt != null && dt.Tables.Count > 0)
                    {
                        SetFormData(dt);
                    }
                }

            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }
        }

        private void KhoiTaoChung()
        {
            InitializeComponent();

            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KeToan;component/PhieuKeToan/ucPhieuKeToanCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }

            BindHotkey();
            LoadComboBoxNguonVon(ClientInformation.MaDonViGiaoDich);
            KhoiTaoDataSource();
            raddgrHachToan.BeginningEdit += raddgrHachToan_BeginningEdit;
            cmbNguonVon.SelectionChanged += new SelectionChangedEventHandler(cmbNguonVon_SelectionChanged);
            ShowControl();
        }

        private void GetsTaiKhoanHachToan()
        {
            KeToanProcess process = new KeToanProcess();
            ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
            try
            {
                _dtSource.Rows.Clear();
                if (formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_THU.layGiaTri()) || formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_CHI.layGiaTri()))
                {
                    AutoCompleteEntry auNguonVon = lstSourceNguonVon.ElementAt(cmbNguonVon.SelectedIndex);
                    Presentation.Process.KeToanServiceRef.PHIEU_KE_TOAN obj = new Presentation.Process.KeToanServiceRef.PHIEU_KE_TOAN();
                    obj.MA_DVI = ClientInformation.MaDonViGiaoDich;
                    obj.NV_LOAI_NVON = auNguonVon.KeywordStrings.FirstOrDefault();
                    obj.MA_LOAI_TIEN_GCO = ClientInformation.MaDongNoiTe;
                    obj.MA_LOAI_TIEN_GNO = ClientInformation.MaDongNoiTe;
                    obj.MA_LOAI_TIEN_GCO_QD = ClientInformation.MaDongNoiTe;
                    obj.MA_LOAI_TIEN_GNO_QD = ClientInformation.MaDongNoiTe;
                    ret = process.PhieuKeToan(_function, DatabaseConstant.Action.LAY_LAI, ref obj, ref listResponseDetail);
                    if (ret.Equals(ApplicationConstant.ResponseStatus.THANH_CONG))
                    {
                        DataRow dr = _dtSource.NewRow();
                        dr["STT"] = _dtSource.Rows.Count + 1;
                        dr["ID_MA_PLOAI"] = obj.DSACH_TKHOAN[0].ID_PLOAI;
                        dr["MA_PLOAI"] = obj.DSACH_TKHOAN[0].MA_PLOAI;
                        dr["SO_TAI_KHOAN"] = obj.DSACH_TKHOAN[0].SO_TAI_KHOAN;
                        dr["TEN_TAI_KHOAN"] = obj.DSACH_TKHOAN[0].TEN_TAI_KHOAN;
                        dr["MA_KY_HIEU"] = obj.DSACH_TKHOAN[0].KY_HIEU;
                        dr["NHOM_DKHOAN"] = "1";
                        _dtSource.Rows.Add(dr);
                        dr = _dtSource.NewRow();
                        dr["STT"] = _dtSource.Rows.Count + 1;
                        dr["NHOM_DKHOAN"] = "1";
                        _dtSource.Rows.Add(dr);
                        raddgrHachToan.ItemsSource = _dtSource.DefaultView;
                    }
                }
                else
                {
                    DataRow dr = _dtSource.NewRow();
                    dr["STT"] = _dtSource.Rows.Count + 1;
                    _dtSource.Rows.Add(dr);
                    dr = _dtSource.NewRow();
                    dr["STT"] = _dtSource.Rows.Count + 1;
                    _dtSource.Rows.Add(dr);
                    raddgrHachToan.ItemsSource = _dtSource.DefaultView;
                }
            }
            catch (Exception ex)
            {

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
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
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
                        key = new KeyBinding(SaveCommand, keyg);
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
        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbAdd.IsEnabled;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ResetForm();
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
            e.CanExecute = tlbApprove.IsEnabled;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeApprove();
        }
        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbCancel.IsEnabled;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeCancel();
        }
        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbRefuse.IsEnabled;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeRefuse();
        }
        private void PreviewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void PreviewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Xem trước dữ liệu");
        }
        private void HelpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Trợ giúp");
        }
        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onClose();
        }

        /// <summary>
        /// Sự kiện hot key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetForm();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                beforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                beforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                onHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                onSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                beforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                beforeRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                beforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
                OnPreview();
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

        /// <summary>
        /// Sự kiện shortcutkey
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetForm();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                beforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                beforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                onHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                onSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                beforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                beforeRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                beforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
                OnPreview();
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

        /// <summary>
        /// Xu ly giao dien
        /// </summary>
        #region Xu ly giao dien
        void LoadComboBoxNguonVon(string maDonVi)
        {
            AutoComboBox auto = new AutoComboBox();
            // Combobox nguon von
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(maDonVi);
            if (!lstSourceNguonVon.IsNullOrEmpty())
                lstSourceNguonVon.Clear();
            if (!cmbNguonVon.Items.IsNullOrEmpty())
                cmbNguonVon.Items.Clear();
            auto.GenAutoComboBox(ref lstSourceNguonVon, ref cmbNguonVon, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NGUON_VON_DVI.getValue(), lstDieuKien);
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
                onClose();
            }
            else if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
            }
        }

        private void btnMaKhachHang_Click(object sender, RoutedEventArgs e)
        {
            //Window window = new Window();
            //window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //window.RenderSize = new Size(1024, 768);
            //PresentationWPF.KhachHang.KhachHang.ucPopupKhachHang uc = new PresentationWPF.KhachHang.KhachHang.ucPopupKhachHang(false);
            //window.Title = "Danh sách khách hàng";
            //window.Content = uc;
            //window.ShowDialog();
            //if (uc.lstData != null && uc.lstData.Count > 0)
            //{
            //    DataRowView dr = uc.lstData[0];
            //    txtMaKhachHang.Text = dr["MA_KHANG"].ToString();
            //    txtTenKhachHang.Text = dr["TEN_KHANG"].ToString();
            //    txtDiaChi.Text = dr["DIA_CHI"].ToString();
            //    txtCmtMst.Text = dr["DD_GTLQ_SO"].ToString();
            //    if (dr["DD_GTLQ_NGAY_CAP"] != null && !LString.IsNullOrEmptyOrSpace(dr["DD_GTLQ_NGAY_CAP"].ToString()))
            //    {
            //        raddtNgayCap.Value = LDateTime.StringToDate(dr["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd");
            //    }
            //    txtNoiCap.Text = dr["DD_GTLQ_NOI_CAP"].ToString();
            //    txtSoDienThoai.Text = dr["DD_SO_DTHOAI"].ToString();
            //}
            //uc = null;
            string sMaDoiTuong = "";
            string MaDonViTao = ClientInformation.MaDonViGiaoDich;
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(MaDonViTao);
            var process = new PopupProcess();
            process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_DOI_TUONG_GIAO_DICH.getValue(), lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            lstPopup.Clear();
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
                sMaDoiTuong = row["MA_DTUONG"].ToString();
                LayDoiTuongTheoMaDoiTuong(sMaDoiTuong);
            }
        }

        private void LayDoiTuongTheoMaDoiTuong(string sMaDoiTuong)
        {
            try
            {
                int iRet = 0;
                KeToanProcess process = new KeToanProcess();
                _objDTuong = new Presentation.Process.KeToanServiceRef.KT_GIAO_DICH_DTUONG();
                _objDTuong.MA_DTUONG = sMaDoiTuong;
                _objDTuong.LOAI_DTUONG = "THEO_MA_DTUONG";
                if (!string.IsNullOrEmpty(sMaDoiTuong))
                {
                    iRet = process.LayDoiTuongGiaoDichTheoMa(ref _objDTuong);
                    if (iRet != 0)
                    {
                        if (_objDTuong != null)
                        {
                            txtMaKhachHang.Text = _objDTuong.MA_DTUONG;
                            txtTenKhachHang.Text = _objDTuong.TEN_DTUONG;
                            txtTenGoiNho.Text = _objDTuong.MA_GOI_NHO;
                            txtSoDienThoai.Text = _objDTuong.SO_DTHOAI;
                            if (!LObject.IsNullOrEmpty(_objDTuong.NGAY_CAP))
                            {
                                raddtNgayCap.Value = LDateTime.StringToDate(_objDTuong.NGAY_CAP, "yyyyMMdd");
                            }
                            txtNoiCap.Text = _objDTuong.NOI_CAP;
                            txtCmtMst.Text = _objDTuong.SO_CMND;
                            txtDiaChi.Text = _objDTuong.DIA_CHI;
                        }
                        else
                        {
                            txtMaKhachHang.Text = "";
                            //txtTenKhachHang.Text = "";
                            //txtTenGoiNho.Text = "";
                            //txtSoDienThoai.Text = "";
                            //txtNoiCap.Text = "";
                            //txtCmtMst.Text = "";
                            //txtDiaChi.Text = "";
                        }
                    }
                    else
                    {
                        txtMaKhachHang.Text = "";
                        //txtTenKhachHang.Text = "";
                        //txtTenGoiNho.Text = "";
                        //txtSoDienThoai.Text = "";
                        //txtNoiCap.Text = "";
                        //txtCmtMst.Text = "";
                        //txtDiaChi.Text = "";
                    }
                }
                else
                {
                    txtMaKhachHang.Text = "";
                    //txtTenKhachHang.Text = "";
                    //txtTenGoiNho.Text = "";
                    //txtSoDienThoai.Text = "";
                    //txtNoiCap.Text = "";
                    //txtCmtMst.Text = "";
                    //txtDiaChi.Text = "";
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void LayDoiTuongTheoMaGoiNho(string sMaDoiTuong)
        {
            try
            {
                int iRet = 0;
                KeToanProcess process = new KeToanProcess();
                _objDTuong = new Presentation.Process.KeToanServiceRef.KT_GIAO_DICH_DTUONG();
                _objDTuong.MA_GOI_NHO = sMaDoiTuong;
                _objDTuong.LOAI_DTUONG = "THEO_MA_GOI_NHO";

                if (!string.IsNullOrEmpty(sMaDoiTuong))
                {
                    iRet = process.LayDTuongGDichTheoMaGoiNho(ref _objDTuong);
                    if (iRet != 0)
                    {
                        if (_objDTuong != null)
                        {
                            txtMaKhachHang.Text = _objDTuong.MA_DTUONG;
                            txtTenKhachHang.Text = _objDTuong.TEN_DTUONG;
                            txtTenGoiNho.Text = _objDTuong.MA_GOI_NHO;
                            txtSoDienThoai.Text = _objDTuong.SO_DTHOAI;
                            if (!LObject.IsNullOrEmpty(_objDTuong.NGAY_CAP))
                            {
                                raddtNgayCap.Value = LDateTime.StringToDate(_objDTuong.NGAY_CAP, "yyyyMMdd");
                            }
                            txtNoiCap.Text = _objDTuong.NOI_CAP;
                            txtCmtMst.Text = _objDTuong.SO_CMND;
                            txtDiaChi.Text = _objDTuong.DIA_CHI;
                        }
                        else
                        {
                            txtMaKhachHang.Text = "";
                            //txtTenKhachHang.Text = "";
                            //txtTenGoiNho.Text = "";
                            //txtSoDienThoai.Text = "";
                            //txtNoiCap.Text = "";
                            //txtCmtMst.Text = "";
                            //txtDiaChi.Text = "";
                        }
                    }
                    else
                    {
                        txtMaKhachHang.Text = "";
                        //txtTenKhachHang.Text = "";
                        //txtTenGoiNho.Text = "";
                        //txtSoDienThoai.Text = "";
                        //txtNoiCap.Text = "";
                        //txtCmtMst.Text = "";
                        //txtDiaChi.Text = "";
                    }
                }
                else
                {
                    txtMaKhachHang.Text = "";
                    //txtTenKhachHang.Text = "";
                    //txtTenGoiNho.Text = "";
                    //txtSoDienThoai.Text = "";
                    //txtNoiCap.Text = "";
                    //txtCmtMst.Text = "";
                    //txtDiaChi.Text = "";
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void txtMaKhachHang_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (LString.IsNullOrEmptyOrSpace(txtMaKhachHang.Text)) return;

            //Presentation.Process.KhachHangProcess process = new Presentation.Process.KhachHangProcess();
            //try
            //{
            //    DataSet ds = process.getThongTinCoBanKHTheoMa(0, txtMaKhachHang.Text, Convert.ToInt32(ClientInformation.IdDonVi));
            //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //    {
            //        DataRow dr = ds.Tables[0].Rows[0];
            //        txtMaKhachHang.Tag = dr["ID"].ToString();
            //        txtMaKhachHang.Text = dr["MA_KHANG"].ToString();
            //        txtTenKhachHang.Text = dr["TEN_KHANG"].ToString();
            //        txtDiaChi.Text = dr["DIA_CHI"].ToString();
            //        txtSoDienThoai.Text = dr["SO_DTHOAI"].ToString();
            //        txtCmtMst.Text = dr["DD_GTLQ_SO"].ToString();
            //        if (!LString.IsNullOrEmptyOrSpace(dr["DD_GTLQ_NGAY_CAP"].ToString()))
            //        {
            //            raddtNgayCap.Value = LDateTime.StringToDate(dr["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd");
            //        }

            //        txtNoiCap.Text = dr["DIA_CHI"].ToString();
            //    }
            //    else
            //    {
            //        LMessage.ShowMessage("Không tồn tại khách hàng này", LMessage.MessageBoxType.Warning);
            //        txtMaKhachHang.Text = "";
            //        txtMaKhachHang.Tag = "";
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    CommonFunction.ThongBaoLoi(ex);
            //    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            //}
            //finally
            //{
            //    process = null;
            //}
            LayDoiTuongTheoMaDoiTuong(txtMaKhachHang.Text.Trim());
        }

        private void btnMaGD_Click(object sender, RoutedEventArgs e)
        {
            Presentation.Process.KeToanProcess ketoanProcess = new Presentation.Process.KeToanProcess();
            try
            {
                //Bat popup
                PopupProcess process = new PopupProcess();
                List<string> lstDK = new List<string>();
                lstDK.Add(BusinessConstant.layMaPhanHeTheoLoaiChungTu(formCase));
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_LOAIGD_KE_TOAN.getValue(), lstDK);

                SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = "Danh sách loại giao dịch";
                win.Content = popup;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow dr = lstPopup[0];
                    txtTenGiaoDich.Text = dr[3].ToString();
                    txtMaGD.Text = dr[2].ToString();
                    txtMaGD.Tag = dr[1].ToString();
                    txtDienGiai.Text = txtTenGiaoDich.Text;

                    LayDanhSachPloaiTheoLoaiGD(txtMaGD.Text);
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                ketoanProcess = null;
            }
        }

        private void txtMaGD_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LString.IsNullOrEmptyOrSpace(txtMaGD.Text))
            {
                txtMaGD.Text = "";
                txtMaGD.Tag = "";
                txtTenGiaoDich.Text = "";
                _dtSource.Rows.Clear();
                DataRow drNew = _dtSource.NewRow();
                drNew["STT"] = _dtSource.Rows.Count + 1;
                _dtSource.Rows.Add(drNew);
            }
            else
            {
                Presentation.Process.DanhMucProcess process = new Presentation.Process.DanhMucProcess();
                try
                {
                    DataSet ds = process.getDanhSachPhanHeGDTheoMa(txtMaGD.Text);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["MA_PHAN_HE"].ToString().Equals(BusinessConstant.layMaPhanHeTheoLoaiChungTu(formCase)))
                        {
                            txtMaGD.Tag = ds.Tables[0].Rows[0]["ID"];
                            txtTenGiaoDich.Text = ds.Tables[0].Rows[0]["TEN_LOAI_GDICH"].ToString();
                            if (LString.IsNullOrEmptyOrSpace(txtDienGiai.Text))
                            {
                                txtDienGiai.Text = txtTenGiaoDich.Text;
                            }

                            LayDanhSachPloaiTheoLoaiGD(txtMaGD.Text);
                        }
                        else
                        {
                            LMessage.ShowMessage("Mã loại giao dịch này không phù hợp", LMessage.MessageBoxType.Warning);
                            txtMaGD.Text = "";
                            txtMaGD.Tag = "";
                            txtTenGiaoDich.Text = "";
                            if (LString.IsNullOrEmptyOrSpace(txtDienGiai.Text))
                            {
                                txtDienGiai.Text = "";
                            }
                            _dtSource.Rows.Clear();
                            DataRow drNew = _dtSource.NewRow();
                            drNew["STT"] = _dtSource.Rows.Count + 1;
                            _dtSource.Rows.Add(drNew);
                        }
                    }
                    else
                    {
                        LMessage.ShowMessage("Không tồn tại mã loại giao dịch này", LMessage.MessageBoxType.Warning);
                        txtMaGD.Text = "";
                        txtMaGD.Tag = "";
                        txtTenGiaoDich.Text = "";
                        if (LString.IsNullOrEmptyOrSpace(txtDienGiai.Text))
                        {
                            txtDienGiai.Text = "";
                        }
                        _dtSource.Rows.Clear();
                        DataRow drNew = _dtSource.NewRow();
                        drNew["STT"] = _dtSource.Rows.Count + 1;
                        _dtSource.Rows.Add(drNew);
                    }
                }
                catch (System.Exception ex)
                {
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                finally
                {
                    process = null;
                }
            }
        }

        private void raddgrHachToan_KeyDown(object sender, KeyEventArgs e)
        {
            List<string> lstDieuKien = new List<string>();
            DataRowView dr = (DataRowView)raddgrHachToan.CurrentItem;
            if (e.Key == Key.F3)
            {
                switch (raddgrHachToan.CurrentCell.Column.UniqueName)
                {
                    case "SO_TAI_KHOAN":
                    case "MA_PLOAI":
                        //if (formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_THU.layGiaTri()) || formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_CHI.layGiaTri()))
                        //{
                        //    DataRowView drv = raddgrHachToan.CurrentCell.ParentRow.Item as DataRowView;
                        //    if (drv["MA_KY_HIEU"].Equals("TIENMAT"))
                        //    {
                        //        return;
                        //    }
                        //}
                        AutoCompleteEntry auNguonVon = lstSourceNguonVon.ElementAt(cmbNguonVon.SelectedIndex);
                        if (LString.IsNullOrEmptyOrSpace(dr["MA_KY_HIEU"].ToString()))
                        {
                            lstDieuKien.Add("%");
                        }
                        else
                        {
                            lstDieuKien.Add(dr["MA_KY_HIEU"].ToString());
                        }
                        lstDieuKien.Add("NOI_BANG");
                        lstDieuKien.Add(Presentation.Process.Common.ClientInformation.MaDonViGiaoDich);
                        lstDieuKien.Add(auNguonVon.KeywordStrings.FirstOrDefault());
                        HienThiPopup(DatabaseConstant.DanhSachTruyVan.POPUP_TKHOAN_CTIET, lstDieuKien);
                        break;
                    case "MA_DTUONG":
                        if (dr["LOAI_DTUONG"].ToString().IsNullOrEmptyOrSpace())
                            break;
                        lstDieuKien.Add(Presentation.Process.Common.ClientInformation.MaDonVi);
                        lstDieuKien.Add(dr["LOAI_DTUONG"].ToString());
                        HienThiPopup(DatabaseConstant.DanhSachTruyVan.POPUP_DS_DOI_TUONG, lstDieuKien);
                        break;
                }
            }

        }

        private void HienThiPopup(DatabaseConstant.DanhSachTruyVan tenPopUp, List<string> lstDieuKien)
        {
            Presentation.Process.KeToanProcess ketoanProcess = new Presentation.Process.KeToanProcess();
            try
            {
                //Bat popup
                var process = new PopupProcess();

                process.getPopupInformation(tenPopUp.getValue(), lstDieuKien);
                SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;

                lstPopup.Clear();
                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.Title = simplePopupResponse.PopupTitle;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow dr = lstPopup[0];
                    DataRowView drCurrent = (DataRowView)raddgrHachToan.SelectedItem;
                    if (LString.IsNullOrEmptyOrSpace(drCurrent["SO_TAI_KHOAN"].ToString()) && LString.IsNullOrEmptyOrSpace(drCurrent["MA_PLOAI"].ToString()) && LString.IsNullOrEmptyOrSpace(drCurrent["MA_DTUONG"].ToString()))
                    {
                        // Them dong moi
                        DataRow drNew = _dtSource.NewRow();
                        drNew["STT"] = _dtSource.Rows.Count + 1;
                        if (formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_THU.layGiaTri()) || formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_CHI.layGiaTri()))
                            drNew["NHOM_DKHOAN"] = "1";
                        _dtSource.Rows.Add(drNew);
                    }

                    switch (tenPopUp)
                    {
                        case DatabaseConstant.DanhSachTruyVan.POPUP_TKHOAN_CTIET:
                            drCurrent["ID_MA_PLOAI"] = dr["ID_PLOAI"];
                            drCurrent["MA_PLOAI"] = dr["MA_PLOAI"];
                            drCurrent["SO_TAI_KHOAN"] = dr[2];
                            drCurrent["TEN_TAI_KHOAN"] = dr[3];
                            drCurrent["MA_TCHAT_GOC"] = dr["MA_TCHAT_GOC"].ToString();
                            drCurrent["LOAI_DTUONG"] = dr["LOAI_DTUONG"].ToString();
                            drCurrent["MA_KY_HIEU"] = dr["MA_KY_HIEU"].ToString();
                            break;
                        case DatabaseConstant.DanhSachTruyVan.POPUP_DS_DOI_TUONG:
                            drCurrent["MA_DTUONG"] = dr[2];
                            break;
                    }
                    raddgrHachToan.CommitEdit();
                    lstPopup.Clear();
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                ketoanProcess = null;
            }
        }

        private void LayDanhSachPloaiTheoLoaiGD(string maLoaiGD)
        {
            KeToanProcess process = new KeToanProcess();
            try
            {
                DataTable dt = null;
                AutoCompleteEntry auNguonVon = lstSourceNguonVon.ElementAt(cmbNguonVon.SelectedIndex);
                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@MaLoaiGD", "STRING", maLoaiGD);
                LDatatable.AddParameter(ref dt, "@MaPhanHe", "STRING", BusinessConstant.layMaPhanHeTheoLoaiChungTu(formCase));
                LDatatable.AddParameter(ref dt, "@MaDonVi", "STRING", ClientInformation.MaDonViGiaoDich);
                LDatatable.AddParameter(ref dt, "@MaNguonVon", "STRING", auNguonVon.KeywordStrings.FirstOrDefault());
                dt = process.getDanhSachBToanTheoLoaiGD(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    _dtSource.Rows.Clear();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = _dtSource.NewRow();
                        dr["STT"] = Convert.ToInt32(dt.Rows[i]["STT"]);
                        dr["MA_PLOAI"] = dt.Rows[i]["MA_PLOAI"].ToString();
                        dr["SO_TAI_KHOAN"] = dt.Rows[i]["SO_TAI_KHOAN"].ToString();
                        dr["TEN_TAI_KHOAN"] = dt.Rows[i]["TEN_TAI_KHOAN"].ToString();
                        dr["GHI_NO"] = 0;
                        dr["GHI_CO"] = 0;
                        dr["NHOM_DKHOAN"] = 1;
                        dr["ID_MA_PLOAI"] = Convert.ToInt32(dt.Rows[i]["ID_PLOAI"]);
                        _dtSource.Rows.Add(dr);
                    }

                    DataRow drNew = _dtSource.NewRow();
                    drNew["STT"] = _dtSource.Rows.Count + 1;
                    _dtSource.Rows.Add(drNew);
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
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
            if (_objKiemSoat != null)
            {
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_objKiemSoat.ID);

                bool ret = process.UnlockData(DatabaseConstant.Module.GDKT,
                    _function,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.SUA,
                    listLockId);
            }
        }

        private void SetEnabledAllControls(bool enable)
        {
            grbThongTinGD.IsEnabled = enable;
            grbThongTinHachToan.IsEnabled = enable;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (LString.IsNullOrEmptyOrSpace(formCase))
            {
                formCase = Presentation.Process.Common.ClientInformation.FormCase;

                if (formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_THU.layGiaTri()))
                {
                    _function = DatabaseConstant.Function.KT_PHIEU_THU;
                }
                else if (formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_CHI.layGiaTri()))
                {
                    _function = DatabaseConstant.Function.KT_PHIEU_CHI;
                }
                else if (formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_KETOAN.layGiaTri()))
                {
                    _function = DatabaseConstant.Function.KT_PHIEU_KE_TOAN;
                }
            }
            txtTenKhachHang.Focus();
        }

        private void raddgrHachToan_Deleting(object sender, GridViewDeletingEventArgs e)
        {
            DataRowView drv = (DataRowView)raddgrHachToan.CurrentItem;
            if (formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_THU.layGiaTri()) || formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_CHI.layGiaTri()))
            {
                if (drv["MA_KY_HIEU"].Equals("TIENMAT"))
                {
                    e.Cancel = true;
                }
            }
            if (raddgrHachToan.Items.Count <= 2)
            {
                e.Cancel = true;
            }
        }

        private void raddgrHachToan_BeginningEdit(object sender, GridViewBeginningEditRoutedEventArgs e)
        {
            if (formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_THU.layGiaTri()) || formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_CHI.layGiaTri()))
            {
                DataRowView drv = e.Cell.ParentRow.Item as DataRowView;
                if (formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_THU.layGiaTri()) && e.Cell.Column.UniqueName.Equals("GHI_CO") && drv["MA_KY_HIEU"].Equals("TIENMAT"))
                    e.Cancel = true;
                else if (formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_CHI.layGiaTri()) && e.Cell.Column.UniqueName.Equals("GHI_NO") && drv["MA_KY_HIEU"].Equals("TIENMAT"))
                    e.Cancel = true;
                else if (formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_THU.layGiaTri()) && e.Cell.Column.UniqueName.Equals("GHI_NO") && !drv["MA_KY_HIEU"].Equals("TIENMAT"))
                    e.Cancel = true;
                else if (formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_CHI.layGiaTri()) && e.Cell.Column.UniqueName.Equals("GHI_CO") && !drv["MA_KY_HIEU"].Equals("TIENMAT"))
                    e.Cancel = true;
            }
        }

        void cmbNguonVon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetsTaiKhoanHachToan();
            LayDanhSachPloaiTheoLoaiGD(txtMaGD.Text);
        }
        #endregion

        #region Xu ly nghiep vu
        private void beforeView()
        {
            //SetFormData();
            SetEnabledAllControls(false);
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, _function);
        }

        private void beforeAddNew()
        {
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, _function);
            lblTrangThai.Content = "";
            raddtNgayNhap.Value = null;
            raddtNgayCNhat.Value = null;
        }

        private void beforeModifyFromList(DatabaseConstant.Action action)
        {
            //SetFormData();
            if (action == DatabaseConstant.Action.XEM)
            {
                SetEnabledAllControls(false);
            }
            CommonFunction.RefreshButton(Toolbar, action, tthaiNvu, mnuMain, _function);
        }

        private void beforeModifyFromDetail()
        {
            // Yêu cầu lock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_objKiemSoat.ID);

            bool ret = process.LockData(DatabaseConstant.Module.GDKT,
                _function,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Nếu lock thành công >> cho phép sửa
            if (ret)
            {
                SetEnabledAllControls(true);
                //SetFormData();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu, mnuMain, _function);
            }
            else
            {
                LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
            }
        }

        private void beforeDelete()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_objKiemSoat.ID);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.GDKT,
                    _function,
                    DatabaseConstant.Table.KT_GIAO_DICH,
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

        private void beforeApprove()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_objKiemSoat.ID);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.GDKT,
                    _function,
                    DatabaseConstant.Table.KT_GIAO_DICH,
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

        private void beforeCancel()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_objKiemSoat.ID);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.GDKT,
                    _function,
                    DatabaseConstant.Table.KT_GIAO_DICH,
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
                listLockId.Add(_objKiemSoat.ID);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.GDKT,
                    _function,
                    DatabaseConstant.Table.KT_GIAO_DICH,
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
        /// Lưu tạm dữ liệu
        /// </summary>
        private void onHold()
        {
            string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            raddgrHachToan.CommitEdit();
            KeToanProcess process = new KeToanProcess();
            try
            {
                Presentation.Process.KeToanServiceRef.PHIEU_KE_TOAN obj = new Presentation.Process.KeToanServiceRef.PHIEU_KE_TOAN();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                // Dữ liệu truyền vào và dữ liệu trả về

                Mouse.OverrideCursor = Cursors.Wait;
                obj = GetPhieuKeToan(trangThai);
                // Nếu là lưu tạm hoặc thêm mới lần đầu
                if (_objKiemSoat == null)
                {
                    // Lấy dữ liệu từ form
                    ret = process.PhieuKeToan(_function, DatabaseConstant.Action.THEM, ref obj, ref listResponseDetail);

                    Mouse.OverrideCursor = Cursors.Arrow;
                    afterAddNew(ret, obj, listResponseDetail);
                }
                // Nếu là lưu tạm khi sửa
                // Hoặc lưu tạm khi sửa sau duyệt
                // Hoặc sửa
                else
                {
                    // Lấy dữ liệu từ form
                    ret = process.PhieuKeToan(_function, DatabaseConstant.Action.SUA, ref obj, ref listResponseDetail);

                    Mouse.OverrideCursor = Cursors.Arrow;
                    afterModify(ret, obj, listResponseDetail);
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
                this.Cursor = Cursors.Arrow;
                process = null;
            }
        }

        /// <summary>
        /// Lưu dữ liệu (hay trình duyệt)
        /// </summary>
        private void onSave()
        {
            string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.TRINH_DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            raddgrHachToan.CommitEdit();
            if (Validation())
            {
                KeToanProcess process = new KeToanProcess();
                try
                {
                    Presentation.Process.KeToanServiceRef.PHIEU_KE_TOAN obj = new Presentation.Process.KeToanServiceRef.PHIEU_KE_TOAN();
                    ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                    // Dữ liệu truyền vào và dữ liệu trả về

                    Mouse.OverrideCursor = Cursors.Wait;
                    obj = GetPhieuKeToan(trangThai);
                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (_objKiemSoat == null)
                    {
                        // Lấy dữ liệu từ form
                        ret = process.PhieuKeToan(_function, DatabaseConstant.Action.THEM, ref obj, ref listResponseDetail);

                        Mouse.OverrideCursor = Cursors.Arrow;
                        afterAddNew(ret, obj, listResponseDetail);
                    }
                    // Nếu là lưu tạm khi sửa
                    // Hoặc lưu tạm khi sửa sau duyệt
                    // Hoặc sửa
                    else
                    {
                        // Lấy dữ liệu từ form
                        ret = process.PhieuKeToan(_function, DatabaseConstant.Action.SUA, ref obj, ref listResponseDetail);

                        Mouse.OverrideCursor = Cursors.Arrow;
                        afterModify(ret, obj, listResponseDetail);
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
                    this.Cursor = Cursors.Arrow;
                    process = null;
                }
            }
        }

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        private void onDelete()
        {
            string trangThai = tthaiNvu;

            KeToanProcess process = new KeToanProcess();
            try
            {
                Presentation.Process.KeToanServiceRef.PHIEU_KE_TOAN obj = new Presentation.Process.KeToanServiceRef.PHIEU_KE_TOAN();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                // Dữ liệu truyền vào và dữ liệu trả về

                Mouse.OverrideCursor = Cursors.Wait;
                obj = GetPhieuKeToan(trangThai);
                ret = process.PhieuKeToan(_function, DatabaseConstant.Action.XOA, ref obj, ref listResponseDetail);

                Mouse.OverrideCursor = Cursors.Arrow;
                afterDelete(ret);

            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }
        }

        /// <summary>
        /// Duyệt dữ liệu
        /// </summary>
        private void onApprove()
        {
            //string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            string trangThai = tthaiNvu;
            KeToanProcess process = new KeToanProcess();
            try
            {
                Presentation.Process.KeToanServiceRef.PHIEU_KE_TOAN obj = new Presentation.Process.KeToanServiceRef.PHIEU_KE_TOAN();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                // Dữ liệu truyền vào và dữ liệu trả về

                Mouse.OverrideCursor = Cursors.Wait;
                obj = GetPhieuKeToan(trangThai);
                ret = process.PhieuKeToan(_function, DatabaseConstant.Action.DUYET, ref obj, ref listResponseDetail);

                Mouse.OverrideCursor = Cursors.Arrow;
                afterApprove(ret, obj);
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }
        }

        /// <summary>
        /// Thoái duyệt dữ liệu
        /// </summary>
        private void onCancel()
        {
            //string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.THOAI_DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            string trangThai = tthaiNvu;
            KeToanProcess process = new KeToanProcess();
            try
            {
                Presentation.Process.KeToanServiceRef.PHIEU_KE_TOAN obj = new Presentation.Process.KeToanServiceRef.PHIEU_KE_TOAN();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                // Dữ liệu truyền vào và dữ liệu trả về

                Mouse.OverrideCursor = Cursors.Wait;
                obj = GetPhieuKeToan(trangThai);
                ret = process.PhieuKeToan(_function, DatabaseConstant.Action.THOAI_DUYET, ref obj, ref listResponseDetail);

                Mouse.OverrideCursor = Cursors.Arrow;
                afterCancel(ret, obj);

            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }
        }

        /// <summary>
        /// Từ chối duyệt dữ liệu
        /// </summary>
        private void onRefuse()
        {
            //string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.TU_CHOI_DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            string trangThai = tthaiNvu;
            KeToanProcess process = new KeToanProcess();
            try
            {
                Presentation.Process.KeToanServiceRef.PHIEU_KE_TOAN obj = new Presentation.Process.KeToanServiceRef.PHIEU_KE_TOAN();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                // Dữ liệu truyền vào và dữ liệu trả về

                Mouse.OverrideCursor = Cursors.Wait;
                obj = GetPhieuKeToan(trangThai);
                ret = process.PhieuKeToan(_function, DatabaseConstant.Action.TU_CHOI_DUYET, ref obj, ref listResponseDetail);

                Mouse.OverrideCursor = Cursors.Arrow;
                afterRefuse(ret, obj);

            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }
        }

        /// <summary>
        /// Sau khi thêm mới
        /// </summary>
        /// <param name="ret"></param>
        private void afterAddNew(ApplicationConstant.ResponseStatus ret, Presentation.Process.KeToanServiceRef.PHIEU_KE_TOAN obj, List<ClientResponseDetail> listResponseDetail)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);
                SetEnabledAllControls(false);
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(obj.TTHAI_NVU);
                txtTrangThai.Text = lblTrangThai.Content.ToString();
                txtNguoiLap.Text = obj.NGUOI_NHAP;
                raddtNgayNhap.Value = LDateTime.StringToDate(obj.NGAY_GDICH, "yyyyMMdd");
                tthaiNvu = obj.TTHAI_NVU;
                txtSoGD.Text = obj.MA_GDICH;
                maGiaoDich = obj.MA_GDICH;
                _objKiemSoat = new KIEM_SOAT();
                _objKiemSoat.ID = obj.ID;
                txtDienGiai.Text = obj.DIEN_GIAI;


                if (cbMultiAdd.IsChecked == true)
                {
                    SetEnabledAllControls(true);
                    ResetForm();
                }
                else if (!DatabaseConstant.CLOSE_DETAIL_FORM)
                {
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, _function);
                }
                else
                {
                    onClose();
                }
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listResponseDetail);
            }
        }

        /// <summary>
        /// Sau khi sửa
        /// </summary>
        /// <param name="ret"></param>
        private void afterModify(ApplicationConstant.ResponseStatus ret, Presentation.Process.KeToanServiceRef.PHIEU_KE_TOAN obj, List<ClientResponseDetail> listResponseDetail)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);
                tthaiNvu = obj.TTHAI_NVU;
                SetEnabledAllControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, _function);
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(obj.TTHAI_NVU);
                txtTrangThai.Text = lblTrangThai.Content.ToString();
                txtNguoiCapNhat.Text = obj.NGUOI_CNHAT;
                raddtNgayNhap.Value = LDateTime.StringToDate(obj.NGAY_GDICH, "yyyyMMdd");
                txtDienGiai.Text = obj.DIEN_GIAI;
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listResponseDetail);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(obj.ID);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
                _function,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Sau khi xóa
        /// </summary>
        /// <param name="ret"></param>
        private void afterDelete(ApplicationConstant.ResponseStatus ret)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_objKiemSoat.ID);
            List<int> listUnlockId = new List<int>();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
                _function,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                listLockId);

            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                // Đóng cửa sổ chi tiết sau khi xóa
                onClose();
            }
        }

        /// <summary>
        /// Sau khi duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void afterApprove(ApplicationConstant.ResponseStatus ret, Presentation.Process.KeToanServiceRef.PHIEU_KE_TOAN obj)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);
                tthaiNvu = obj.TTHAI_NVU;
                SetEnabledAllControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, _function);
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                txtTrangThai.Text = lblTrangThai.Content.ToString();
                txtNguoiCapNhat.Text = obj.NGUOI_CNHAT;
                raddtNgayCNhat.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, "yyyyMMdd");
                txtDienGiai.Text = obj.DIEN_GIAI;
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.DuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(obj.ID);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
                _function,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Sau khi thoái duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void afterCancel(ApplicationConstant.ResponseStatus ret, Presentation.Process.KeToanServiceRef.PHIEU_KE_TOAN obj)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);
                tthaiNvu = obj.TTHAI_NVU;
                SetEnabledAllControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, _function);
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                txtTrangThai.Text = lblTrangThai.Content.ToString();
                txtNguoiCapNhat.Text = obj.NGUOI_CNHAT;
                raddtNgayCNhat.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, "yyyyMMdd");
                txtDienGiai.Text = obj.DIEN_GIAI;
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.ThoaiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(obj.ID);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
                _function,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Sau khi từ chối
        /// </summary>
        /// <param name="ret"></param>
        private void afterRefuse(ApplicationConstant.ResponseStatus ret, Presentation.Process.KeToanServiceRef.PHIEU_KE_TOAN obj)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);
                tthaiNvu = obj.TTHAI_NVU;
                SetEnabledAllControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, _function);
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                txtTrangThai.Text = lblTrangThai.Content.ToString();
                txtNguoiCapNhat.Text = obj.NGUOI_CNHAT;
                raddtNgayCNhat.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, "yyyyMMdd");
                txtDienGiai.Text = obj.DIEN_GIAI;
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.TuChoiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(obj.ID);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.GDKT,
                _function,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Xem báo cáo
        /// </summary>
        private void OnPreview()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(maGiaoDich))
            {
                LMessage.ShowMessage("Không có thông tin dữ liệu cần hiển thị", LMessage.MessageBoxType.Warning);

            }
            else
            {
                //if (ClientInformation.Company.Equals("BANTAYVANG"))
                //{
                //    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                //    DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();

                //    GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                //    objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.GDKT_IN_GIAO_DICH;
                //    objGIAO_DICH_BASE.ChucNang = _function;

                //    GDKT_GIAO_DICH objGDKT_GIAO_DICH = new GDKT_GIAO_DICH();
                //    objGDKT_GIAO_DICH.MaGiaoDich = maGiaoDich;

                //    doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                //    doiTuongBaoCao.objGDKT_GIAO_DICH = objGDKT_GIAO_DICH;                    

                //    List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
                //    listThamSoBaoCao.Add(new ThamSoBaoCao("@SoPhieu", maGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                //    listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                //    listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                //    string maBaoCao = "GDKT_GIAO_DICH";
                //    xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
                //    xemBaoCao.LayDuLieu(maBaoCao, listThamSoBaoCao);
                //}
                //else
                //{
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                    DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();

                    GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                    objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.GDKT_IN_GIAO_DICH;
                    objGIAO_DICH_BASE.ChucNang = _function;

                    GDKT_GIAO_DICH objGDKT_GIAO_DICH = new GDKT_GIAO_DICH();
                    objGDKT_GIAO_DICH.MaGiaoDich = maGiaoDich;

                    doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                    doiTuongBaoCao.objGDKT_GIAO_DICH = objGDKT_GIAO_DICH;
                    xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
                //}
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Kiểm tra dữ liệu trước khi lưu
        /// </summary>
        /// <returns></returns>
        private bool Validation()
        {
            try
            {
                if (raddgrHachToan.Items.Count > 0)
                {
                    DataView dataview = raddgrHachToan.ItemsSource as DataView;
                    foreach (DataRowView drv in dataview)
                    {
                        if (drv["MA_PLOAI"].ToString().IsNullOrEmptyOrSpace() && drv["SO_TAI_KHOAN"].ToString().IsNullOrEmptyOrSpace() && drv["GHI_NO"].ToString().IsNullOrEmptyOrSpace() && drv["GHI_CO"].ToString().IsNullOrEmptyOrSpace())
                            _dtSource.Rows.Remove(drv.Row);
                    }
                    raddgrHachToan.Rebind();
                }
                if (LString.IsNullOrEmptyOrSpace(txtTenKhachHang.Text))
                {
                    CommonFunction.ThongBaoTrong(lblTenKhachHang.Content.ToString());
                    txtTenKhachHang.Focus();
                    return false;
                }
                else if (raddgrHachToan.ItemsSource == null || raddgrHachToan.Items.Count == 0)
                {
                    LMessage.ShowMessage("Chưa có các bút toán", LMessage.MessageBoxType.Warning);
                    return false;
                }
                else
                {
                    // Kiểm tra thông tin trên grid
                    for (int i = 0; i < _dtSource.Rows.Count; i++)
                    {
                        // Kiểm tra tài khoản trống
                        if (LString.IsNullOrEmptyOrSpace(_dtSource.Rows[i]["SO_TAI_KHOAN"].ToString()))
                        {
                            LMessage.ShowMessage("Dòng thứ " + (i + 1) + ": chưa có số tài khoản", LMessage.MessageBoxType.Warning);
                            return false;
                        }
                        else if (LString.IsNullOrEmptyOrSpace(_dtSource.Rows[i]["NHOM_DKHOAN"].ToString()))
                        {
                            LMessage.ShowMessage("Dòng thứ " + (i + 1) + ": nhóm định khoản không được để trống", LMessage.MessageBoxType.Warning);
                            return false;
                        }
                        else if (LString.IsNullOrEmptyOrSpace(_dtSource.Rows[i]["GHI_NO"].ToString()) && LString.IsNullOrEmptyOrSpace(_dtSource.Rows[i]["GHI_CO"].ToString()))
                        {
                            LMessage.ShowMessage("Dòng thứ " + (i + 1) + ": chưa có số tiền ghi nợ hoặc ghi có", LMessage.MessageBoxType.Warning);
                            return false;
                        }
                        else
                        {
                            if (!LString.IsNullOrEmptyOrSpace(_dtSource.Rows[i]["GHI_NO"].ToString()) && !LString.IsNullOrEmptyOrSpace(_dtSource.Rows[i]["GHI_CO"].ToString()))
                            {
                                decimal ghiNo = Convert.ToDecimal(_dtSource.Rows[i]["GHI_NO"]);
                                decimal ghiCo = Convert.ToDecimal(_dtSource.Rows[i]["GHI_CO"]);
                                if (ghiNo == 0 && ghiCo == 0)
                                {
                                    LMessage.ShowMessage("Dòng thứ " + (i + 1) + ": chưa có số tiền ghi nợ hoặc ghi có", LMessage.MessageBoxType.Warning);
                                    return false;
                                }
                                if (ghiNo != 0 && ghiCo != 0)
                                {
                                    LMessage.ShowMessage("Dòng thứ " + (i + 1) + ": một tài khoản chỉ được phép ghi nợ hoặc ghi có số tiền", LMessage.MessageBoxType.Warning);
                                    return false;
                                }
                            }
                        }
                        if (!LString.IsNullOrEmptyOrSpace(_dtSource.Rows[i]["LOAI_DTUONG"].ToString()) && LString.IsNullOrEmptyOrSpace(_dtSource.Rows[i]["MA_DTUONG"].ToString()))
                        {
                            LMessage.ShowMessage("Dòng thứ " + (i + 1) + ": chưa chọn đối tượng cho tài khoản hạch toán", LMessage.MessageBoxType.Warning);
                            return false;
                        }
                    }


                    // Kiem tra tong no = tong co tren cac nhom dinh khoan
                    if (this.raddgrHachToan.GroupDescriptors.Count == 0)
                    {
                        this.raddgrHachToan.GroupDescriptors.Add(new ColumnGroupDescriptor()
                            {
                                Column = this.raddgrHachToan.Columns["NHOM_DKHOAN"],
                                SortDirection = ListSortDirection.Ascending
                            });
                    }

                    if (this.raddgrHachToan.Items.Groups != null)
                    {
                        foreach (IGroup group in this.raddgrHachToan.Items.Groups)
                        {
                            decimal tongNo = 0;
                            decimal tongCo = 0;
                            int soButToanNo = 0;
                            int soButToanCo = 0;
                            List<DataRowView> items = group.Items.Cast<DataRowView>().ToList();
                            foreach (DataRowView dr in items)
                            {
                                if (!LString.IsNullOrEmptyOrSpace(dr["GHI_NO"].ToString()))
                                {
                                    tongNo += Convert.ToDecimal(dr["GHI_NO"]);
                                }

                                if (!LString.IsNullOrEmptyOrSpace(dr["GHI_CO"].ToString()))
                                {
                                    tongCo += Convert.ToDecimal(dr["GHI_CO"]);
                                }

                                if (!LString.IsNullOrEmptyOrSpace(dr["GHI_NO"].ToString()) && Convert.ToDecimal(dr["GHI_NO"].ToString()) != 0)
                                {
                                    soButToanNo++;
                                }

                                if (!LString.IsNullOrEmptyOrSpace(dr["GHI_CO"].ToString()) && Convert.ToDecimal(dr["GHI_CO"].ToString()) != 0)
                                {
                                    soButToanCo++;
                                }
                            }

                            if (tongNo != tongCo)
                            {
                                LMessage.ShowMessage("Nhóm định khoản " + group.Key + " có tổng phát sinh nợ không bằng tổng phát sinh có", LMessage.MessageBoxType.Warning);
                                return false;
                            }
                            else
                            {
                                if (soButToanNo >= 2 && soButToanCo >= 2)
                                {
                                    LMessage.ShowMessage("Nhóm định khoản " + group.Key + " không được ghi nhiều nợ nhiều có", LMessage.MessageBoxType.Warning);
                                    return false;
                                }
                                else if (soButToanNo > 0 && soButToanCo == 0)
                                {
                                    LMessage.ShowMessage("Nhóm định khoản " + group.Key + " chưa có vế ghi có", LMessage.MessageBoxType.Warning);
                                    return false;
                                }
                                else if (soButToanNo == 0 && soButToanCo > 0)
                                {
                                    LMessage.ShowMessage("Nhóm định khoản " + group.Key + " chưa có vế ghi nợ", LMessage.MessageBoxType.Warning);
                                    return false;
                                }
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
            finally
            {
                if (this.raddgrHachToan.GroupDescriptors.Count > 0)
                {
                    this.raddgrHachToan.GroupDescriptors.Clear();
                }

                DataRow dr = _dtSource.NewRow();
                dr["STT"] = _dtSource.Rows.Count + 1;
                _dtSource.Rows.Add(dr);
                raddgrHachToan.Rebind();
            }
            return true;
        }

        private void SetFormData(DataSet ds)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            KeToanProcess process = new KeToanProcess();
            AutoComboBox au = new AutoComboBox();
            try
            {
                DataTable dt = ds.Tables["BUT_TOAN"];
                if (dt.Rows.Count < 1)
                    return;
                // Thông tin chi tiết
                string sMaDoiTuong = dt.Rows[0]["MA_KHANG"].ToString();
                txtSoGD.Text = dt.Rows[0]["MA_GDICH"].ToString();
                txtMaGD.Text = dt.Rows[0]["MA_LOAI_GDICH"].ToString();
                txtMaGD.Tag = dt.Rows[0]["ID_LOAI_GDICH"].ToString();
                txtTenGiaoDich.Text = dt.Rows[0]["TEN_LOAI_GDICH"].ToString();
                if (!LObject.IsNullOrEmpty(sMaDoiTuong)) LayDoiTuongTheoMaDoiTuong(sMaDoiTuong);
                else
                {
                    txtMaKhachHang.Text = dt.Rows[0]["MA_KHANG"].ToString();
                    txtTenKhachHang.Text = dt.Rows[0]["TEN_KHANG"].ToString();
                    txtDiaChi.Text = dt.Rows[0]["DIA_CHI"].ToString();
                    txtCmtMst.Text = dt.Rows[0]["SO_CMND"].ToString();
                    if (!LString.IsNullOrEmptyOrSpace(dt.Rows[0]["NGAY_CAP"].ToString()))
                    {
                        raddtNgayCap.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_CAP"].ToString(), "yyyyMMdd");
                    }
                    else
                    {
                        raddtNgayCap.Value = null;
                    }

                    txtNoiCap.Text = dt.Rows[0]["NOI_CAP"].ToString();
                    txtSoDienThoai.Text = "";
                }
                raddtNgayChungTu.Value = dt.Rows[0]["NGAY_CTU_KTHEO"] != DBNull.Value ? LDateTime.StringToDate(dt.Rows[0]["NGAY_CTU_KTHEO"].ToString(), "yyyyMMdd") : LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                txtChungTuKemTheo.Text = dt.Rows[0]["SO_CTU_KTHEO"].ToString();
                txtDienGiai.Text = dt.Rows[0]["DIEN_GIAI"].ToString();

                txtNguoiLap.Text = dt.Rows[0]["NGUOI_NHAP"].ToString();
                raddtNgayNhap.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_GDICH"].ToString(), "yyyyMMdd");
                txtNguoiCapNhat.Text = dt.Rows[0]["NGUOI_CNHAT"].ToString();
                if (!LString.IsNullOrEmptyOrSpace(dt.Rows[0]["NGAY_CNHAT"].ToString()))
                {
                    raddtNgayCNhat.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_CNHAT"].ToString(), "yyyyMMdd");
                }
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(dt.Rows[0]["TTHAI_NVU"].ToString());
                lblTrangThai.Content = txtTrangThai.Text;
                tthaiNvu = dt.Rows[0]["TTHAI_NVU"].ToString();
                cmbNguonVon.SelectionChanged -= cmbNguonVon_SelectionChanged;
                LoadComboBoxNguonVon(dt.Rows[0]["MA_DVI"].ToString());
                if (dt.Rows[0]["NV_LOAI_NVON"] != DBNull.Value)
                    cmbNguonVon.SelectedIndex = lstSourceNguonVon.IndexOf(lstSourceNguonVon.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dt.Rows[0]["NV_LOAI_NVON"].ToString())));
                cmbNguonVon.SelectionChanged += cmbNguonVon_SelectionChanged;
                // Grid hạch toán
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow drSource = _dtSource.NewRow();
                    drSource["STT"] = Convert.ToInt32(dr["STT"]);
                    drSource["ID_MA_PLOAI"] = Convert.ToInt32(dr["ID_PLOAI"]);
                    drSource["MA_PLOAI"] = dr["MA_PLOAI"].ToString();
                    drSource["SO_TAI_KHOAN"] = dr["SO_TAI_KHOAN"].ToString();
                    drSource["TEN_TAI_KHOAN"] = dr["TEN_TAI_KHOAN"].ToString();
                    drSource["GHI_NO"] = Convert.ToDecimal(dr["GHI_NO"]);
                    drSource["GHI_CO"] = Convert.ToDecimal(dr["GHI_CO"]);
                    drSource["NHOM_DKHOAN"] = dr["NHOM_DKHOAN"].ToString();
                    drSource["MA_DTUONG"] = dr["MA_DTUONG"].ToString();
                    drSource["MA_TCHAT_GOC"] = dr["MA_TCHAT_GOC"].ToString();
                    drSource["LOAI_DTUONG"] = dr["LOAI_DTUONG"].ToString();
                    drSource["MA_KY_HIEU"] = dr["MA_KY_HIEU"].ToString();
                    _dtSource.Rows.Add(drSource);
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private Presentation.Process.KeToanServiceRef.PHIEU_KE_TOAN GetPhieuKeToan(string tthai)
        {
            Presentation.Process.KeToanServiceRef.PHIEU_KE_TOAN obj = new Presentation.Process.KeToanServiceRef.PHIEU_KE_TOAN();
            AutoCompleteEntry auNguonVon = lstSourceNguonVon.ElementAt(cmbNguonVon.SelectedIndex);
            if (_objKiemSoat != null)
            {
                obj.ID = _objKiemSoat.ID;
                obj.NGUOI_CNHAT = Presentation.Process.Common.ClientInformation.TenDangNhap;
                obj.NGAY_CNHAT = Presentation.Process.Common.ClientInformation.NgayLamViecHienTai;
                obj.NGUOI_NHAP = txtNguoiLap.Text;
                obj.NGAY_NHAP = Convert.ToDateTime(raddtNgayNhap.Value).ToString("yyyyMMdd");
            }
            else
            {
                obj.NGUOI_NHAP = Presentation.Process.Common.ClientInformation.TenDangNhap;
                obj.NGAY_NHAP = Presentation.Process.Common.ClientInformation.NgayLamViecHienTai;
            }

            if (LString.IsNullOrEmptyOrSpace(txtMaGD.Text))
            {
                txtMaGD.Text = BusinessConstant.layLoaiChungTu(formCase).layMaLoaiChungTuMacDinh();
                txtMaGD_LostFocus(null, null);
            }

            obj.ID_LOAI_GDICH = Convert.ToInt32(txtMaGD.Tag);
            obj.MA_LOAI_GDICH = txtMaGD.Text;
            obj.ID_DVI = Presentation.Process.Common.ClientInformation.IdDonViGiaoDich;
            obj.MA_DVI = Presentation.Process.Common.ClientInformation.MaDonViGiaoDich;
            obj.MA_GDICH = txtSoGD.Text;
            obj.MA_KHANG = txtMaKhachHang.Text;
            obj.TEN_KHANG = txtTenKhachHang.Text;
            obj.DIA_CHI = txtDiaChi.Text;
            obj.SO_CMND = txtCmtMst.Text;
            obj.NV_LOAI_NVON = auNguonVon.KeywordStrings.FirstOrDefault();

            if (raddtNgayCap.Value != null)
            {
                obj.NGAY_CAP = Convert.ToDateTime(raddtNgayCap.Value).ToString("yyyyMMdd");
            }
            else
            {
                obj.NGAY_CAP = "";
            }

            obj.NOI_CAP = txtNoiCap.Text;
            obj.DIEN_GIAI = txtDienGiai.Text;

            if (raddtNgayNhap.Value != null)
            {
                obj.NGAY_GDICH = Convert.ToDateTime(raddtNgayNhap.Value).ToString("yyyyMMdd");
            }
            else
            {
                obj.NGAY_GDICH = Presentation.Process.Common.ClientInformation.NgayLamViecHienTai;
            }
            if (raddtNgayChungTu.Value.HasValue)
            {
                obj.NGAY_CTU_KTHEO = Convert.ToDateTime(raddtNgayChungTu.Value).ToString("yyyyMMdd");
            }
            obj.NGAY_LAP = Convert.ToDateTime(raddtNgayChungTu.Value).ToString("yyyyMMdd");
            obj.SO_CTU_KTHEO = txtChungTuKemTheo.Text;

            obj.LOAI_CHUNG_TU = BusinessConstant.layLoaiChungTu(formCase).layLoaiChungTu();
            obj.LY_DO = "";
            obj.MA_DTUONG = "";
            obj.MA_LOAI_TIEN_GCO = ClientInformation.MaDongNoiTe;
            obj.MA_LOAI_TIEN_GNO = ClientInformation.MaDongNoiTe;
            obj.MA_LOAI_TIEN_GCO_QD = ClientInformation.MaDongNoiTe;
            obj.MA_LOAI_TIEN_GNO_QD = ClientInformation.MaDongNoiTe;
            obj.MA_PHAN_HE = BusinessConstant.layMaPhanHeTheoLoaiChungTu(formCase);
            obj.MA_TCHIEU = "";
            //obj.NGAY_CTU_KTHEO = "";
            obj.PHUONG_PHAP = "";
            obj.TY_GIA = 1;
            obj.HACH_TOAN = "BANG_TAY";
            obj.DSACH_TKHOAN = GetThongTinGD().ToArray();
            obj.TTHAI_NVU = tthai;

            if (chkLuuNguoiGDich.IsChecked == true)
            {
                LayThongTinDoiTuongGDich(ref _objDTuongDTO, tthaiNvu);
                obj.KT_GIAO_DICH_DTUONG_DTO = _objDTuongDTO;
            }

            return obj;
        }

        private void LayThongTinDoiTuongGDich(ref Presentation.Process.KeToanServiceRef.KT_GIAO_DICH_DTUONG_DTO objDTuong, string sTrangThai)
        {
            objDTuong = new Presentation.Process.KeToanServiceRef.KT_GIAO_DICH_DTUONG_DTO();
            //objDTuong.ID = _objDTuong.ID;
            objDTuong.DIA_CHI = txtDiaChi.Text.Trim();
            objDTuong.MA_DTUONG = txtMaKhachHang.Text;
            objDTuong.MA_DVI_QLY = ClientInformation.MaDonVi;
            objDTuong.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
            objDTuong.MA_GOI_NHO = txtTenGoiNho.Text;
            if (raddtNgayCap.Value != null) objDTuong.NGAY_CAP = Convert.ToDateTime(raddtNgayCap.Value).ToString("yyyyMMdd");
            objDTuong.NGAY_CNHAT = objDTuong.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
            objDTuong.NGUOI_CNHAT = objDTuong.NGUOI_NHAP = ClientInformation.TenDangNhap;
            objDTuong.NOI_CAP = txtNoiCap.Text.Trim();
            objDTuong.SO_CMND = txtCmtMst.Text.Trim();
            objDTuong.SO_DTHOAI = txtSoDienThoai.Text.Trim();
            objDTuong.TEN_CHI_NHANH = ClientInformation.TenDonViGiaoDich;
            objDTuong.TEN_DTUONG = txtTenKhachHang.Text.Trim();
            objDTuong.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
            objDTuong.TTHAI_NVU = sTrangThai;
        }

        private List<Presentation.Process.KeToanServiceRef.TAIKHOAN> GetThongTinGD()
        {
            List<Presentation.Process.KeToanServiceRef.TAIKHOAN> lstCTruc = new List<Presentation.Process.KeToanServiceRef.TAIKHOAN>();
            for (int i = 0; i < _dtSource.Rows.Count - 1; i++)
            {
                Presentation.Process.KeToanServiceRef.TAIKHOAN obj = new Presentation.Process.KeToanServiceRef.TAIKHOAN();
                obj.ID_PLOAI = Convert.ToInt32(_dtSource.Rows[i]["ID_MA_PLOAI"]);
                obj.MA_DOI_TUONG = _dtSource.Rows[i]["MA_DTUONG"].ToString();
                obj.MA_PLOAI = _dtSource.Rows[i]["MA_PLOAI"].ToString();
                obj.NHOM_DINH_KHOAN = _dtSource.Rows[i]["NHOM_DKHOAN"].ToString();
                obj.SO_TAI_KHOAN = _dtSource.Rows[i]["SO_TAI_KHOAN"].ToString();
                
                if (!LString.IsNullOrEmptyOrSpace(_dtSource.Rows[i]["GHI_CO"].ToString()))
                {
                    obj.STIEN_CO = Convert.ToDecimal(_dtSource.Rows[i]["GHI_CO"].ToString());
                }
                else
                {
                    obj.STIEN_CO = 0;
                }

                if (!LString.IsNullOrEmptyOrSpace(_dtSource.Rows[i]["GHI_NO"].ToString()))
                {
                    obj.STIEN_NO = Convert.ToDecimal(_dtSource.Rows[i]["GHI_NO"].ToString());
                }
                else
                {
                    obj.STIEN_NO = 0;
                }
                lstCTruc.Add(obj);
            }

            return lstCTruc;
        }

        private void ResetForm()
        {
            _objKiemSoat = null;
            txtMaGD.Text = "";
            txtMaGD.Tag = "";
            txtSoGD.Text = "";
            txtTenGiaoDich.Text = "";
            txtMaKhachHang.Text = "";
            txtMaKhachHang.Tag = "";
            txtTenKhachHang.Text = "";
            txtDiaChi.Text = "";
            txtCmtMst.Text = "";
            txtNoiCap.Text = "";
            txtSoDienThoai.Text = "";
            txtChungTuKemTheo.Text = "";
            txtDienGiai.Text = "";
            raddtNgayCap.Value = null;
            raddtNgayChungTu.Value = LDateTime.StringToDate(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            lblTrangThai.Content = "";
            tthaiNvu = "";
            txtTrangThai.Text = "";
            txtNguoiLap.Text = "";
            txtNguoiCapNhat.Text = "";
            raddtNgayNhap.Value = null;
            raddtNgayCNhat.Value = null;
            _dtSource.Rows.Clear();
            GetsTaiKhoanHachToan();
        }

        #endregion

        private void raddgrHachToan_CellValidating(object sender, GridViewCellValidatingEventArgs e)
        {
            var cell = e.Cell;
            if (cell != null)
            {
                if (e.EditingElement is TextBox)
                {
                    if (LString.IsNullOrEmptyOrSpace(((TextBox)e.EditingElement).Text))
                    {
                        if (e.Cell.Column.UniqueName == "GHI_NO" || e.Cell.Column.UniqueName == "GHI_CO")
                        {
                            ((TextBox)e.EditingElement).Text = "0";
                        }
                        else if (e.Cell.Column.UniqueName == "NHOM_DKHOAN")
                        {
                            ((TextBox)e.EditingElement).Text = "1";
                        }
                    }
                }
            }
        }

        private void txtTenGoiNho_LostFocus(object sender, RoutedEventArgs e)
        {
            LayDoiTuongTheoMaGoiNho(txtTenGoiNho.Text.Trim());
        }

        private void raddgrHachToan_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            DataRowView drv = e.Cell.ParentRow.Item as DataRowView;
            try
            {
                if (formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_THU.layGiaTri()) || formCase.Equals(BusinessConstant.LOAI_CHUNG_TU.PHIEU_CHI.layGiaTri()))
                {
                    drv["NHOM_DKHOAN"] = "1";
                }
                raddgrHachToan.CurrentItem = drv;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                drv = null;
            }
        }
    }
}
