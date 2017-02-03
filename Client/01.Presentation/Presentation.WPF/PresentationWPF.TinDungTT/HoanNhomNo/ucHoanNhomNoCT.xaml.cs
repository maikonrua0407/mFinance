using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Data;
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
using PresentationWPF.CustomControl;
using Telerik.Windows.Controls;
using Presentation.Process.Common;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.TinDungServiceRef;
using System.Reflection;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;

namespace PresentationWPF.TinDungTT.HoanNhomNo
{
    /// <summary>
    /// Interaction logic for ucHoanNhomNoCT.xaml
    /// </summary>
    public partial class ucHoanNhomNoCT : UserControl
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
        DataTable dtKUocHoan = new DataTable();
        List<DataRow> lstPopupKU = new List<DataRow>();
        TDVM_CHUYEN_HOAN_NHOM_NO TDVMCHNO = null;
        List<DANH_SACH_KHE_UOC_CHUYEN_HOAN_NHOM_NO> lstChuyenHoan = null;
        void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopupKU = lst;
        }
        string TThaiNVu = "";
        List<AutoCompleteEntry> lstTienTe = new List<AutoCompleteEntry>();
        int iDGiaoDich;
        DatabaseConstant.Action action;
        string mAGiaoDich;
        public EventHandler OnSavingCompleted;
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucHoanNhomNoCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDungTT;component/HoanNhomNo/ucHoanNhomNoCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            InitEventHanler();
            ClearForm();
        }

        public ucHoanNhomNoCT(KIEM_SOAT objKiemSoat) : this()
        {
            action = objKiemSoat.action;
            iDGiaoDich = objKiemSoat.ID;
            mAGiaoDich = objKiemSoat.SO_GIAO_DICH;
            SetDataForm();
        }

        void KhoiTaoComboBox(Telerik.Windows.Controls.GridView.GridViewRowDetailsEventArgs e)
        {
            try
            {
                AutoComboBox auto = new AutoComboBox();
                RadComboBox combobox = new RadComboBox();
                combobox = e.DetailsElement.FindName("cmbTienTeDaTrich") as RadComboBox;
                if (LObject.IsNullOrEmpty(lstTienTe) || lstTienTe.Count == 0)
                    auto.GenAutoComboBox(ref lstTienTe, ref combobox, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TIENTE.getValue(), null, ClientInformation.MaDongNoiTe);
                else
                    auto.GenAutoComboBox(ref lstTienTe, ref combobox, null, null, ClientInformation.MaDongNoiTe);
                combobox = e.DetailsElement.FindName("cmbTienTePhaiTrich") as RadComboBox;
                auto.GenAutoComboBox(ref lstTienTe, ref combobox, null, null, ClientInformation.MaDongNoiTe);
                combobox = e.DetailsElement.FindName("cmbTienTeChenhLech") as RadComboBox;
                auto.GenAutoComboBox(ref lstTienTe, ref combobox, null, null, ClientInformation.MaDongNoiTe);
                combobox = e.DetailsElement.FindName("cmbTienTeDuThu") as RadComboBox;
                auto.GenAutoComboBox(ref lstTienTe, ref combobox, null, null, ClientInformation.MaDongNoiTe);
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            
        }

        void InitEventHanler()
        {
            btnCal.Click += new RoutedEventHandler(btnCal_Click);
            ucNhomNoMoi.EditCellEnd += new EventHandler(ucNhomNoMoi_EditCellEnd);
            grdKheUoc.CellEditEnded += new EventHandler<GridViewCellEditEndedEventArgs>(grdKheUoc_CellEditEnded);
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
                        KeyGesture keyg = new KeyGesture(Key.I, ModifierKeys.Control);
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
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control|ModifierKeys.Shift);
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
                        KeyGesture keyg = new KeyGesture(Key.F, ModifierKeys.Control | ModifierKeys.Shift);
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
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAP_DU_LIEU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ClearForm();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                Modify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            { }
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
            { }
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
                    throw new System.NullReferenceException(LLanguage.SearchResourceByKey("M.TinDungTT.HoanNhomNo.ucHoanNhomNoCT.KhongTimThayControl") + sbControl.ToString());
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            try
            {
                string sidKheUoc = "";
                if (LObject.IsNullOrEmpty(lstChuyenHoan)) lstChuyenHoan = new List<DANH_SACH_KHE_UOC_CHUYEN_HOAN_NHOM_NO>();
                foreach (DANH_SACH_KHE_UOC_CHUYEN_HOAN_NHOM_NO dr in lstChuyenHoan)
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
                lstDieuKien.Add("KUOCCHUYENHOAN");
                lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
                lstDieuKien.Add("%");
                lstPopupKU.Clear();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_KHEUOC", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopupKheUocViMo popup = new ucPopupKheUocViMo(true, simplePopupResponse, true);
                popup.LayGiaTriListDataRow = new ucPopupKheUocViMo.LayListDataRow(LayDuLieuTuPopup);
                Window win = new Window();
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.Content = popup;
                win.ShowDialog();
                if (lstPopupKU.Count > 0)
                {
                    foreach (DataRow drv in lstPopupKU)
                    {
                        DANH_SACH_KHE_UOC_CHUYEN_HOAN_NHOM_NO objDSKUOC = new DANH_SACH_KHE_UOC_CHUYEN_HOAN_NHOM_NO();
                        foreach (DataColumn dcl in drv.Table.Columns)
                        {
                            PropertyInfo proper = objDSKUOC.GetType().GetProperty(dcl.ColumnName);
                            if (!LObject.IsNullOrEmpty(proper))
                            {
                                if (proper.PropertyType.Equals(typeof(int)))
                                    proper.SetValue(objDSKUOC, Convert.ToInt32(drv[dcl.ColumnName]),null);
                                else if (proper.PropertyType.Equals(typeof(decimal)))
                                    proper.SetValue(objDSKUOC, Convert.ToDecimal(drv[dcl.ColumnName]), null);
                                else
                                    proper.SetValue(objDSKUOC, drv[dcl.ColumnName].ToString(), null);
                            }
                        }
                        lstChuyenHoan.Add(objDSKUOC);
                    }
                    LoadDataGridKheUoc();
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
            
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            foreach (DANH_SACH_KHE_UOC_CHUYEN_HOAN_NHOM_NO drv in grdKheUoc.SelectedItems)
            {
                lstChuyenHoan.Remove(drv);
            }
            LoadDataGridKheUoc();
        }

        private void btnCal_Click(object sender, RoutedEventArgs e)
        {
            int iret = 0;
            try
            {
                if (!LObject.IsNullOrEmpty(grdKheUoc.RowInEditMode))
                    grdKheUoc.CommitEdit();
                TDVMCHNO.NGAY_GIAO_DICH = TDVMCHNO.NGAY_LAP = ClientInformation.NgayLamViecHienTai;
                lstChuyenHoan = grdKheUoc.ItemsSource as List<DANH_SACH_KHE_UOC_CHUYEN_HOAN_NHOM_NO>;
                TDVMCHNO.DSACH_KHE_UOC = lstChuyenHoan.ToArray();
                List<ClientResponseDetail> lstResponseDatail = new List<ClientResponseDetail>();
                iret = new TinDungProcess().TinhToanTrichLapDuPhong(ref TDVMCHNO, ref lstResponseDatail);
                CommonFunction.ThongBaoKetQua(lstResponseDatail);
                if (iret > 0)
                {
                    lstChuyenHoan = TDVMCHNO.DSACH_KHE_UOC.ToList();
                    LoadDataGridKheUoc();
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void ClearForm()
        {
            TDVMCHNO = new TDVM_CHUYEN_HOAN_NHOM_NO();
            lstChuyenHoan = new List<DANH_SACH_KHE_UOC_CHUYEN_HOAN_NHOM_NO>();
            txtSoPhieu.Text = "";
            teldtNgayCapPhatVon.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            txtDienGiai.Text = "";
            lblTrangThai.Content = "";
            txtNguoiLap.Text = "";
            txtNguoiCapNhat.Text = "";
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            teldtNgayCNhat.Value = null;
            txtTrangThai.Text = TThaiNVu = "";
            LoadDataGridKheUoc();
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, TThaiNVu);
        }

        void ucNhomNoMoi_EditCellEnd(object sender, EventArgs e)
        {
            DANH_SACH_KHE_UOC_CHUYEN_HOAN_NHOM_NO objKUOC = ucNhomNoMoi.cellEdit.ParentRow.Item as DANH_SACH_KHE_UOC_CHUYEN_HOAN_NHOM_NO;
            lstChuyenHoan[lstChuyenHoan.IndexOf(objKUOC)].NHOM_NO_MOI = ucNhomNoMoi.GiaTri;
        }

        private void grdKheUoc_LoadingRowDetails(object sender, Telerik.Windows.Controls.GridView.GridViewRowDetailsEventArgs e)
        {
            KhoiTaoComboBox(e);
        }
        void grdKheUoc_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            DANH_SACH_KHE_UOC_CHUYEN_HOAN_NHOM_NO objKUOCCHoan = e.Cell.ParentRow.Item as DANH_SACH_KHE_UOC_CHUYEN_HOAN_NHOM_NO;
            lstChuyenHoan.ElementAt(lstChuyenHoan.IndexOf(objKUOCCHoan)).DU_PHONG_CU_THE_TRICH_LAP = objKUOCCHoan.DU_PHONG_CU_THE_TRICH_LAP;
            lstChuyenHoan.ElementAt(lstChuyenHoan.IndexOf(objKUOCCHoan)).DU_THU_HOAN_LAI = objKUOCCHoan.DU_THU_HOAN_LAI;
        }
        #endregion

        /// <summary>
        /// Xu ly nghiep vu
        /// </summary>
        #region Xu ly nghiep vu
        void LoadDataGridKheUoc()
        {
            grdKheUoc.ItemsSource = null;
            grdKheUoc.ItemsSource = lstChuyenHoan;
        }

        bool Validation()
        {
            bool bReturn = true;
            try
            {
                if (txtDienGiai.Text.IsNullOrEmpty())
                {
                    CommonFunction.ThongBaoTrong(lblDienGiai.Content.ToString());
                    txtDienGiai.Focus();
                    bReturn = false;
                }
            }
            catch (System.Exception ex)
            {
                bReturn = false;
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return bReturn;
        }

        void Modify()
        {
            List<int> lstId = new List<int>();
            lstId.Add(iDGiaoDich);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_CHUYEN_HOAN_NHOM_NO,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.SUA,
            lstId);
            action = DatabaseConstant.Action.SUA;
            OnModify();
            //SetEnabledAllControl(true);
        }
        void GetDataForm(BusinessConstant.TrangThaiBanGhi bghi, BusinessConstant.TrangThaiNghiepVu nghiepvu)
        {

            try
            {
                TDVMCHNO.NGAY_GIAO_DICH = LDateTime.DateToString(teldtNgayCapPhatVon.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
                TDVMCHNO.ID_GIAO_DICH = iDGiaoDich;
                TDVMCHNO.MA_GIAO_DICH = mAGiaoDich;
                TDVMCHNO.LOAI_TIEN = ClientInformation.MaDongNoiTe;
                TDVMCHNO.DIEN_GIAI = TDVMCHNO.LY_DO = txtDienGiai.Text;
                TDVMCHNO.MA_DVI = ClientInformation.MaDonViGiaoDich;
                TDVMCHNO.TRANG_THAI_BAN_GHI = bghi.layGiaTri();
                TDVMCHNO.TRANG_THAI_NGHIEP_VU = nghiepvu.layGiaTri();
                TDVMCHNO.NGUOI_LAP = ClientInformation.TenDangNhap;
                TDVMCHNO.NGAY_LAP = ClientInformation.NgayLamViecHienTai;
                TDVMCHNO.MA_GIAO_DICH = mAGiaoDich;
                if (iDGiaoDich > 0)
                {
                    TDVMCHNO.NGUOI_LAP = txtNguoiLap.Text;
                    TDVMCHNO.NGAY_LAP = LDateTime.DateToString((DateTime)teldtNgayNhap.Value, ApplicationConstant.defaultDateTimeFormat);
                    TDVMCHNO.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                    TDVMCHNO.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                }
                lstChuyenHoan = grdKheUoc.ItemsSource as List<DANH_SACH_KHE_UOC_CHUYEN_HOAN_NHOM_NO>;
                TDVMCHNO.DSACH_KHE_UOC = lstChuyenHoan.ToArray();
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }

        }
        void SetDataForm()
        {
            Cursor = Cursors.Wait;
            try
            {
                DataSet ds = new TinDungProcess().GetThongTinChuyenHoanNhomNo(mAGiaoDich);
                if (LObject.IsNullOrEmpty(ds) || ds.Tables.Count < 1)
                    return;
                SetTabThongTinChung(ds);
                SetTabThongTinKiemSoat(ds);
                if (action.Equals(DatabaseConstant.Action.SUA))
                    Modify();
                else
                    OnModify();
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }
        void SetTabThongTinChung(DataSet ds)
        {
            try
            {
                DataTable dt = ds.Tables["TTIN_CTIET"];
                if (LObject.IsNullOrEmpty(TDVMCHNO)) TDVMCHNO = new TDVM_CHUYEN_HOAN_NHOM_NO();
                if (LObject.IsNullOrEmpty(dt) || dt.Rows.Count < 1)
                    return;
                TDVMCHNO.DIEN_GIAI = dt.Rows[0]["DIEN_GIAI"].ToString();
                iDGiaoDich = TDVMCHNO.ID_GIAO_DICH = Convert.ToInt32(dt.Rows[0]["ID_GIAO_DICH"]);
                TDVMCHNO.LOAI_TIEN = dt.Rows[0]["LOAI_TIEN"].ToString();
                TDVMCHNO.LOAI_TIEN = dt.Rows[0]["LOAI_TIEN"].ToString();
                TDVMCHNO.LY_DO = dt.Rows[0]["LY_DO"].ToString();
                TDVMCHNO.MA_DVI = dt.Rows[0]["MA_DVI"].ToString();
                TDVMCHNO.MA_DVI = dt.Rows[0]["MA_DVI"].ToString();
                TDVMCHNO.MA_GIAO_DICH = dt.Rows[0]["MA_GIAO_DICH"].ToString();
                TDVMCHNO.NGAY_GIAO_DICH = dt.Rows[0]["NGAY_GIAO_DICH"].ToString();
                TDVMCHNO.NGAY_LAP = dt.Rows[0]["NGAY_LAP"].ToString();
                TDVMCHNO.NGUOI_LAP = dt.Rows[0]["NGUOI_LAP"].ToString();
                TDVMCHNO.TRANG_THAI_BAN_GHI = dt.Rows[0]["TRANG_THAI_BAN_GHI"].ToString();
                TDVMCHNO.TRANG_THAI_NGHIEP_VU = dt.Rows[0]["TRANG_THAI_NGHIEP_VU"].ToString();
                TDVMCHNO.NGUOI_CAP_NHAT = dt.Rows[0]["NGUOI_CAP_NHAT"] != null ? dt.Rows[0]["NGUOI_CAP_NHAT"].ToString() : "";
                TDVMCHNO.NGAY_CAP_NHAT = dt.Rows[0]["NGAY_CAP_NHAT"] != null ? dt.Rows[0]["NGAY_CAP_NHAT"].ToString() : "";
                txtDienGiai.Text = TDVMCHNO.DIEN_GIAI;
                mAGiaoDich = txtSoPhieu.Text = TDVMCHNO.MA_GIAO_DICH;
                TThaiNVu = TDVMCHNO.TRANG_THAI_NGHIEP_VU;
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TDVMCHNO.TRANG_THAI_NGHIEP_VU);
                teldtNgayCapPhatVon.Value = LDateTime.StringToDate(TDVMCHNO.NGAY_GIAO_DICH, ApplicationConstant.defaultDateTimeFormat);
                dt = ds.Tables["KHE_UOC"];
                lstChuyenHoan = new List<DANH_SACH_KHE_UOC_CHUYEN_HOAN_NHOM_NO>();
                foreach (DataRow dr in dt.Rows)
                {
                    DANH_SACH_KHE_UOC_CHUYEN_HOAN_NHOM_NO objKUOC = new DANH_SACH_KHE_UOC_CHUYEN_HOAN_NHOM_NO();
                    foreach (DataColumn dcl in dt.Columns)
                    {
                        PropertyInfo proper = objKUOC.GetType().GetProperty(dcl.ColumnName);
                        if (!LObject.IsNullOrEmpty(proper))
                        {
                            if (proper.PropertyType.Equals(typeof(int)))
                                proper.SetValue(objKUOC, Convert.ToInt32(dr[dcl.ColumnName]), null);
                            else if (proper.PropertyType.Equals(typeof(decimal)))
                                proper.SetValue(objKUOC, Convert.ToDecimal(dr[dcl.ColumnName]), null);
                            else
                                proper.SetValue(objKUOC, dr[dcl.ColumnName].ToString(), null);
                        }
                    }
                    lstChuyenHoan.Add(objKUOC);
                }
                grdKheUoc.ItemsSource = null;
                grdKheUoc.ItemsSource = lstChuyenHoan;
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
            }

        }
        void SetTabThongTinKiemSoat(DataSet ds)
        {
            try
            {
                DataTable dt = ds.Tables["TTIN_CTIET"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    txtNguoiLap.Text = dt.Rows[0]["NGUOI_LAP"].ToString();
                    txtNguoiCapNhat.Text = dt.Rows[0]["NGUOI_CAP_NHAT"].ToString();
                    teldtNgayNhap.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_LAP"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    if (!dt.Rows[0]["NGAY_CAP_NHAT"].ToString().IsNullOrEmptyOrSpace())
                        teldtNgayCNhat.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_CAP_NHAT"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(dt.Rows[0]["TRANG_THAI_BAN_GHI"].ToString());
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
            }
        }
        void BeforeSave(BusinessConstant.TrangThaiNghiepVu nghiepvu, BusinessConstant.TrangThaiBanGhi bghi)
        {
            Cursor = Cursors.Wait;
            try
            {
                txtDienGiai.Focus();
                if (!nghiepvu.Equals(BusinessConstant.TrangThaiNghiepVu.LUU_TAM))
                {
                    if (!Validation())
                    {
                        Cursor = Cursors.Arrow;
                        return;
                    }
                }
                GetDataForm(bghi, nghiepvu);
                OnSave();
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }

        }
        void OnSave()
        {
            try
            {
                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                int iret = 0;
                if (iDGiaoDich == 0)
                    iret = new TinDungProcess().ThemMoiGiaoDichChuyenHoanNo(ref TDVMCHNO, ref lstResponseDetail);
                else
                    iret = new TinDungProcess().SuaGiaoDichChuyenHoanNo(ref TDVMCHNO, ref lstResponseDetail);
                iDGiaoDich = TDVMCHNO.ID_GIAO_DICH;
                mAGiaoDich = TDVMCHNO.MA_GIAO_DICH;
                AfterSave(lstResponseDetail, iret);
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
        void AfterSave(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            try
            {
                CommonFunction.ThongBaoKetQua(lstResponseDetail);
                // Yêu cầu Unlock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHUYEN_HOAN_NHOM_NO,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.SUA,
                lstId);
                action = DatabaseConstant.Action.XEM;
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
                if (iret > 0)
                    SetDataForm();
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void OnPreview()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(iDGiaoDich) && LObject.IsNullOrEmpty(mAGiaoDich))
            {
                LMessage.ShowMessage("M.TinDungTT.HoanNhomNo.ucHoanNhomNoCT.KhongCoThongTinSo", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();

                GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.GDKT_IN_GIAO_DICH;
                objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDVM_CHUYEN_HOAN_NHOM_NO;

                GDKT_GIAO_DICH objGDKT_GIAO_DICH = new GDKT_GIAO_DICH();
                objGDKT_GIAO_DICH.MaGiaoDich = mAGiaoDich;

                doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                doiTuongBaoCao.objGDKT_GIAO_DICH = objGDKT_GIAO_DICH;
                xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
            }
        }

        void BeforeDelete()
        {
            Cursor = Cursors.Wait;
            try
            {
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHUYEN_HOAN_NHOM_NO,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.XOA,
                lstId);
                OnDelete();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHUYEN_HOAN_NHOM_NO,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.XOA,
                lstId);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally { Cursor = Cursors.Arrow; }
        }
        void OnDelete()
        {
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (iDGiaoDich != 0)
            {
                TDVMCHNO.ID_GIAO_DICH = iDGiaoDich;
                TDVMCHNO.MA_GIAO_DICH = mAGiaoDich;
                TDVMCHNO.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                TDVMCHNO.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                iret = new TinDungProcess().XoaGiaoDichChuyenHoanNo(ref TDVMCHNO, ref lstResponseDetail);
            }
            AfterDelete(lstResponseDetail, iret);
        }
        void AfterDelete(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(iDGiaoDich);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_CHUYEN_HOAN_NHOM_NO,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.XOA,
            lstId);
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            Cursor = Cursors.Arrow;
            if (iret == 0)
                SetDataForm();
            else
                CommonFunction.CloseUserControl(this);
        }

        void BeforeApprove()
        {
            try
            {
                Cursor = Cursors.Wait;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHUYEN_HOAN_NHOM_NO,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.DUYET,
                lstId);
                OnApprove();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHUYEN_HOAN_NHOM_NO,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.DUYET,
                lstId);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally { Cursor = Cursors.Arrow; }
        }
        void OnApprove()
        {
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (iDGiaoDich != 0)
            {
                TDVMCHNO.ID_GIAO_DICH = iDGiaoDich;
                TDVMCHNO.MA_GIAO_DICH = mAGiaoDich;
                TDVMCHNO.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                TDVMCHNO.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                iret = new TinDungProcess().DuyetGiaoDichChuyenHoanNo(ref TDVMCHNO, ref lstResponseDetail);
            }
            AfterApprove(lstResponseDetail, iret);
        }
        void AfterApprove(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(iDGiaoDich);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_CHUYEN_HOAN_NHOM_NO,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.DUYET,
            lstId);
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            SetDataForm();
            Cursor = Cursors.Arrow;
        }

        void BeforeRefuse()
        {
            try
            {
                Cursor = Cursors.Wait;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHUYEN_HOAN_NHOM_NO,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);
                OnRefuse();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHUYEN_HOAN_NHOM_NO,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally { Cursor = Cursors.Arrow; }
        }
        void OnRefuse()
        {
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (iDGiaoDich != 0)
            {
                TDVMCHNO.ID_GIAO_DICH = iDGiaoDich;
                TDVMCHNO.MA_GIAO_DICH = mAGiaoDich;
                TDVMCHNO.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                TDVMCHNO.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                iret = new TinDungProcess().TuChoiDuyetGiaoDichChuyenHoanNo(ref TDVMCHNO, ref lstResponseDetail);
            }
            AfterRefuse(lstResponseDetail, iret);
        }
        void AfterRefuse(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(iDGiaoDich);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_CHUYEN_HOAN_NHOM_NO,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.TU_CHOI_DUYET,
            lstId);
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            Cursor = Cursors.Arrow;
            SetDataForm();
        }

        void BeforeCancel()
        {
            try
            {
                Cursor = Cursors.Wait;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHUYEN_HOAN_NHOM_NO,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                OnCancel();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(iDGiaoDich);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHUYEN_HOAN_NHOM_NO,
                DatabaseConstant.Table.TD_KUOCVM,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);

            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }
        void OnCancel()
        {
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (iDGiaoDich != 0)
            {
                TDVMCHNO.ID_GIAO_DICH = iDGiaoDich;
                TDVMCHNO.MA_GIAO_DICH = mAGiaoDich;
                TDVMCHNO.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                TDVMCHNO.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                iret = new TinDungProcess().ThoaiDuyetGiaoDichChuyenHoanNo(ref TDVMCHNO, ref lstResponseDetail);
            }
            AfterCancel(lstResponseDetail, iret);
        }
        void AfterCancel(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(iDGiaoDich);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_CHUYEN_HOAN_NHOM_NO,
            DatabaseConstant.Table.TD_KUOCVM,
            DatabaseConstant.Action.THOAI_DUYET,
            lstId);
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            Cursor = Cursors.Arrow;
            SetDataForm();
        }

        void OnModify()
        {
            if (action.Equals(DatabaseConstant.Action.SUA))
            {
                btnAdd.IsEnabled = true;
                btnCal.IsEnabled = true;
                btnDelete.IsEnabled = true;
                grdKheUoc.IsReadOnly = false;
                txtDienGiai.IsEnabled = true;
            }
            else
            {
                btnAdd.IsEnabled = false;
                btnCal.IsEnabled = false;
                btnDelete.IsEnabled = false;
                grdKheUoc.IsReadOnly = true;
                txtDienGiai.IsEnabled = false;
            }
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_CHUYEN_HOAN_NHOM_NO);
        }
        #endregion
        
    }
}
