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
using Presentation.Process.TinDungServiceRef;
using Telerik.Windows.Controls;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.Common;
using System.Reflection;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;

namespace PresentationWPF.TinDung.HDTD
{
    /// <summary>
    /// Interaction logic for ucHDTDThoaThuanCT.xaml
    /// </summary>
    public partial class ucHDTDThoaThuanCT : UserControl
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
        private Presentation.Process.TinDungServiceRef.TDVM_HDTD objHDTDVM = new Presentation.Process.TinDungServiceRef.TDVM_HDTD();
        List<DataRow> lstPopup = new List<DataRow>();
        private DataTable dtKhachHang = null;
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        int idKhachHang = 0;
        string maKhangHang = "";
        string tenKhachHang = "";
        string soGTLQ = "";
        string tenNhom = "";
        string TThaiNVu = "";
        public DatabaseConstant.Action action;
        public event EventHandler OnSavingCompleted;
        List<int> lstId = new List<int>();
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucHDTDThoaThuanCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDung;component/HDTD/ucHDTDThoaThuanCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            InitEventHandler();
            TaoBangDuLieu();
            ResetForm();
        }

        void InitEventHandler()
        {
            tlbDetailAdd.Click += new RoutedEventHandler(tlbDetailAdd_Click);
            tlbDetailDelete.Click += new RoutedEventHandler(tlbDetailDelete_Click);
            uccmbDongSoHuu.EditCellEnd += new EventHandler(uccmbDongSoHuu_EditCellEnd);
            raddgrDSThanhVien.CellEditEnded += new EventHandler<GridViewCellEditEndedEventArgs>(raddgrDSThanhVien_CellEditEnded);
            this.Unloaded += new RoutedEventHandler(ucHDTDThoaThuanCT_Unloaded);
        }

        void ucHDTDThoaThuanCT_Unloaded(object sender, RoutedEventArgs e)
        {
            Release();
        }

        void TaoBangDuLieu()
        {
            dtKhachHang = new DataTable();
            dtKhachHang.Columns.Add("ID");
            dtKhachHang.Columns.Add("MA_HDTDVM");
            dtKhachHang.Columns.Add("ID_KHANG");
            dtKhachHang.Columns.Add("MA_KHANG");
            dtKhachHang.Columns.Add("TEN_KHANG");
            dtKhachHang.Columns.Add("DD_GTLQ_SO");
            dtKhachHang.Columns.Add("TEN_NHOM");
            dtKhachHang.Columns.Add("ID_NGUOI_DTN");
            dtKhachHang.Columns.Add("MA_NGUOI_DTN");
            dtKhachHang.Columns.Add("TTHAI_GIAI_NGAN");
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
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
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
                    throw new System.NullReferenceException("Không tìm thấy control " + sbControl.ToString());
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void tlbDetailDelete_Click(object sender, RoutedEventArgs e)
        {
            if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            foreach (DataRow dr in raddgrDSThanhVien.SelectedItems)
            {
                dtKhachHang.Rows.Remove(dr);
            }
            LoadDataGridView();
        }

        void tlbDetailAdd_Click(object sender, RoutedEventArgs e)
        {
            List<string> lstDieuKien = new List<string>();
            string lstIDKH = "";
            for (int i=0;i<dtKhachHang.Rows.Count;i++)
            {
                lstIDKH += "," + dtKhachHang.Rows[i]["ID_KHANG"].ToString();
            }
            if (lstIDKH.Length > 0)
                lstIDKH = "(" + lstIDKH.Substring(1) + ")";
            else
                lstIDKH = "(0)";
            lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
            lstDieuKien.Add(lstIDKH);
            lstDieuKien.Add("NULL");
            lstDieuKien.Add(DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG.getValue());
            lstPopup = new List<DataRow>();
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation("POPUP_DS_KHACHHANG_TDVM", lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            ucPopup popup = new ucPopup(true, simplePopupResponse, true);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.Content = popup;
            win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ShowDialog();
            if (lstPopup.Count > 0)
            {
                foreach (DataRow dr in lstPopup)
                {
                    getThongTinKhachHang(Convert.ToInt32(dr["ID"]), "");
                    DataRow drv = dtKhachHang.NewRow();
                    drv["ID"] = 0;
                    drv["MA_HDTDVM"] = "";
                    drv["ID_KHANG"] = idKhachHang;
                    drv["MA_KHANG"] = maKhangHang;
                    drv["TEN_KHANG"] = tenKhachHang;
                    drv["DD_GTLQ_SO"] = soGTLQ;
                    drv["TEN_NHOM"] = tenNhom;
                    drv["TTHAI_GIAI_NGAN"] = "GIAI_NGAN";
                    dtKhachHang.Rows.Add(drv);
                }
                LoadDataGridView();
            }
        }

        void raddgrDSThanhVien_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            //Telerik.Windows.Controls.GridView.GridViewCell cellEditCurrent = e.Cell;
            //DataRow drv = (DataRow)e.Cell.ParentRow.Item;
            //int IndexOf = dtKhachHang.Rows.IndexOf(drv);
            //Telerik.Windows.Controls.GridView.GridViewCell cellCurrent = raddgrDSThanhVien.CurrentCell;
            //if (cellCurrent.Column.UniqueName == "MA_KHANG")
            //{
            //    TextBox txtControl = e.EditingElement as TextBox;
            //    string sGiaTri = txtControl.Text;
            //    getThongTinKhachHang(0, sGiaTri);
            //    if (idKhachHang > 0 && !cellCurrent.Value.Equals(maKhangHang))
            //    {
            //        dtKhachHang.Rows[IndexOf]["MA_KHANG"] = maKhangHang;
            //        dtKhachHang.Rows[IndexOf]["ID_KHANG"] = idKhachHang;
            //        dtKhachHang.Rows[IndexOf]["TEN_KHANG"] = tenKhachHang;
            //        dtKhachHang.Rows[IndexOf]["DD_GTLQ_SO"] = soGTLQ;
            //        dtKhachHang.Rows[IndexOf]["TEN_NHOM"] = tenNhom;
            //        dtKhachHang.Rows[IndexOf]["ID_NGUOI_DTN"] = "";
            //    }
            //}
        }

        void uccmbDongSoHuu_EditCellEnd(object sender, EventArgs e)
        {
            Telerik.Windows.Controls.GridView.GridViewCell cellEdit = uccmbDongSoHuu.cellEdit;
            DataRow drv = (DataRow)cellEdit.ParentRow.Item;
            int indexofRows = dtKhachHang.Rows.IndexOf(drv);
            string NameColumn = uccmbDongSoHuu.cellEdit.Column.UniqueName;
            string GiaTri = uccmbDongSoHuu.GiaTri;
            dtKhachHang.Rows[indexofRows][NameColumn] = GiaTri;
            if (NameColumn.Equals("MA_NGUOI_DTN"))
                dtKhachHang.Rows[indexofRows]["ID_NGUOI_DTN"] = uccmbDongSoHuu.LstgiaTri[1];
            //LoadDataGridView();
        }
        #endregion

        /// <summary>
        /// Xu ly nghiep vu
        /// </summary>
        #region Xu ly nghiep vu
        void GetDataForm(BusinessConstant.TrangThaiBanGhi bghi, BusinessConstant.TrangThaiNghiepVu nghiepvu)
        {
            try
            {
                objHDTDVM.SO_GIAO_DICH = txtSoGiaoDich.Text;
                List<TD_HDTDVM> lstHDTDVM = new List<TD_HDTDVM>();
                foreach (DataRow dr in dtKhachHang.Rows)
                {
                    objHDTDVM.HDTD_VM = new TD_HDTDVM();
                    objHDTDVM.HDTD_VM.ID = Convert.ToInt32(dr["ID"]);
                    objHDTDVM.HDTD_VM.ID_KHANG = Convert.ToInt32(dr["ID_KHANG"]);
                    objHDTDVM.HDTD_VM.ID_NGUOI_DTN = null;
                    objHDTDVM.HDTD_VM.SO_GDICH = txtSoGiaoDich.Text;
                    objHDTDVM.HDTD_VM.MA_NGUOI_DTN = dr["MA_NGUOI_DTN"].ToString();
                    if (!dr["ID_NGUOI_DTN"].IsNullOrEmpty() && dr["ID_NGUOI_DTN"].ToString().IsNumeric())
                        objHDTDVM.HDTD_VM.ID_NGUOI_DTN = Convert.ToInt32(dr["ID_NGUOI_DTN"]);
                    objHDTDVM.HDTD_VM.MA_KHANG = dr["MA_KHANG"].ToString();
                    objHDTDVM.HDTD_VM.MA_HDTDVM = dr["MA_HDTDVM"].ToString();
                    objHDTDVM.HDTD_VM.NGAY_HD = teldtNgayLapHD.Value != null ? LDateTime.DateToString((DateTime)teldtNgayLapHD.Value, ApplicationConstant.defaultDateTimeFormat) : "";
                    objHDTDVM.HDTD_VM.LSUAT_QHAN = txtLSQuaHan.Value != null ? (decimal)txtLSQuaHan.Value / 100 : 0;
                    objHDTDVM.HDTD_VM.LSUAT_CCAU = txtLSCoCauLai.Value != null ? (decimal)txtLSCoCauLai.Value / 100 : 0;
                    objHDTDVM.HDTD_VM.TTHAI_NVU = nghiepvu.layGiaTri();
                    objHDTDVM.HDTD_VM.TTHAI_BGHI = bghi.layGiaTri();
                    objHDTDVM.HDTD_VM.SO_GDICH = txtSoGiaoDich.Text;
                    
                    objHDTDVM.HDTD_VM.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    objHDTDVM.HDTD_VM.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    objHDTDVM.HDTD_VM.MA_DVI_QLY = ClientInformation.MaDonVi;
                    objHDTDVM.HDTD_VM.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    if (objHDTDVM.HDTD_VM.SO_GDICH != "")
                    {
                        objHDTDVM.HDTD_VM.NGUOI_NHAP = txtNguoiLap.Text;
                        objHDTDVM.HDTD_VM.NGAY_NHAP = LDateTime.DateToString((DateTime)teldtNgayNhap.Value, ApplicationConstant.defaultDateTimeFormat);
                        objHDTDVM.HDTD_VM.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                        objHDTDVM.HDTD_VM.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    }
                    objHDTDVM.HDTD_VM.TTHAI_GIAI_NGAN = "GIAI_NGAN";
                    lstHDTDVM.Add(objHDTDVM.HDTD_VM);
                }
                if (lstHDTDVM.Count == 0)
                    return;
                objHDTDVM.DSACH_HDTD_VM = lstHDTDVM.ToArray();

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
                ds = new TinDungProcess().getThongTinChiTietHDTDVMBySoGiaoDich(sSoGiaoDich);
                SetTabThongTinChung(ds);
                SetTabThongKiemSoat(ds);
                if (action == DatabaseConstant.Action.SUA)
                    beforeModify();
                else
                    SetEnabledAllControls(false);
                CommonFunction.RefreshButton(Toolbar, action, TThaiNVu);
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
                DataTable dt = ds.Tables["TTIN_CTIET"];
                dtKhachHang.Rows.Clear();
                if (dt.Rows.Count > 0)
                {
                    if (LObject.IsNullOrEmpty(objHDTDVM)) objHDTDVM = new Presentation.Process.TinDungServiceRef.TDVM_HDTD();
                    objHDTDVM.SO_GIAO_DICH = txtSoGiaoDich.Text = dt.Rows[0]["SO_GDICH"].ToString();
                    if (!dt.Rows[0]["NGAY_HD"].ToString().IsNullOrEmpty())
                        teldtNgayLapHD.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_HD"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    txtLSQuaHan.Value = Convert.ToDouble(dt.Rows[0]["LSUAT_QHAN"]) * 100;
                    txtLSCoCauLai.Value = Convert.ToDouble(dt.Rows[0]["LSUAT_CCAU"]) * 100;
                    TThaiNVu = dt.Rows[0]["TTHAI_NVU"].ToString();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
                    foreach (DataRow dr in dt.Rows)
                    {
                        DataRow drn = dtKhachHang.NewRow();
                        for (int i = 0; i < dtKhachHang.Columns.Count; i++)
                        {
                            string ColumnName = dtKhachHang.Columns[i].ColumnName;
                            drn[ColumnName] = dr[ColumnName];
                        }
                        dtKhachHang.Rows.Add(drn);
                    }
                    LoadDataGridView();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void SetTabThongKiemSoat(DataSet ds)
        {
            try
            {
                DataTable dt = ds.Tables["TTIN_CTIET"];
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        bool getThongTinKhachHang(int id, string maKHang)
        {
            bool bResutl = true;
            try
            {
                DataSet ds = new KhachHangProcess().getThongTinCoBanKHTheoMa(id, maKHang, 0);
                if (ds.Tables[0].Rows.Count == 1)
                {
                    idKhachHang = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]);
                    maKhangHang = ds.Tables[0].Rows[0]["MA_KHANG"].ToString();
                    tenKhachHang = ds.Tables[0].Rows[0]["TEN_KHANG"].ToString();
                    soGTLQ = ds.Tables[0].Rows[0]["DD_GTLQ_SO"].ToString();
                    tenNhom = ds.Tables[0].Rows[0]["TEN_CUM"].ToString() + "-" + ds.Tables[0].Rows[0]["TEN_NHOM"].ToString();
                }
            }
            catch (Exception ex)
            {
                bResutl = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
            }
            return bResutl;
        }

        void LoadDataGridView()
        {
            raddgrDSThanhVien.ItemsSource = null;
            raddgrDSThanhVien.ItemsSource = dtKhachHang;
        }

        void ResetForm()
        {
            txtSoGiaoDich.Text = "";
            teldtNgayLapHD.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            txtLSQuaHan.Value = 0;
            txtLSCoCauLai.Value = 0;
            raddgrDSThanhVien.ItemsSource = null;
            lblTrangThai.Content = "";
            txtNguoiLap.Text = "";
            txtNguoiCapNhat.Text = "";
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai,ApplicationConstant.defaultDateTimeFormat);
            teldtNgayCNhat.Value = null;
            txtTrangThai.Text = TThaiNVu = "";
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, TThaiNVu);
            double QHanTDa = Convert.ToDouble(new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_TYLE_LAINO_QUAHAN_TOIDA, ClientInformation.MaDonVi));
            double CoCauTDa = Convert.ToDouble(new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_TYLE_LAICOCAU_KYHAN_TRANO, ClientInformation.MaDonVi));
            txtLSQuaHan.Value = QHanTDa;
            txtLSCoCauLai.Value = CoCauTDa;
        }

        void beforeModify()
        {
            action = DatabaseConstant.Action.SUA;
            SetEnabledAllControls(true);
            OnModify();
        }

        private void SetEnabledAllControls(bool enable)
        {
            grbThongTinChung.IsEnabled = enable;
            tlbDetailAdd.IsEnabled = enable;
            tlbDetailDelete.IsEnabled = enable;
            raddgrDSThanhVien.IsReadOnly = !enable;
        }
        private void Release()
        {
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG,
            DatabaseConstant.Table.TD_HDTDVM,
            DatabaseConstant.Action.SUA,
            lstId);
        }

        void OnModify()
        {
            foreach (DataRow dr in dtKhachHang.Rows)
            {
                lstId.Add(Convert.ToInt32(dr["ID"]));
            }
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG,
            DatabaseConstant.Table.TD_HDTDVM,
            action,
            lstId);
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu);
        }

        bool Validation()
        {
            bool bReturn = true;
            if (teldtNgayLapHD.Value.IsNullOrEmpty())
            {
                CommonFunction.ThongBaoTrong(lblNgayLapHD.Content.ToString());
                teldtNgayLapHD.Focus();
                bReturn = false;
            }
            else if (txtLSQuaHan.Value.IsNullOrEmpty())
            {
                CommonFunction.ThongBaoTrong(lblLSuatQHan.Content.ToString());
                txtLSQuaHan.Focus();
                bReturn = false;
            }
            else if (txtLSCoCauLai.Value.IsNullOrEmpty())
            {
                CommonFunction.ThongBaoTrong(lblLSuatCoCauLai.Content.ToString());
                txtLSCoCauLai.Focus();
                bReturn = false;
            }
            return bReturn;
        }
        void BeforeSave(BusinessConstant.TrangThaiNghiepVu trangthai,BusinessConstant.TrangThaiBanGhi banghi)
        {
            if (!trangthai.Equals(BusinessConstant.TrangThaiNghiepVu.LUU_TAM))
            {
                if(!Validation())
                    return;
            }
            if (dtKhachHang.Rows.Count < 1)
            {
                CommonFunction.ThongBaoTrong(lblDanhSachHDTD.Content.ToString());
                tlbDetailAdd.Focus();
                return;
            }
            GetDataForm(banghi, trangthai);
            OnSave();
        }

        void OnSave()
        {
            
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (txtSoGiaoDich.Text == "")
                iret = new TinDungProcess().ThemMoiHopDongTinDungViMo(ref objHDTDVM, ref lstResponseDetail);
            else
                iret = new TinDungProcess().SuaHopDongTinDungViMo(ref objHDTDVM, ref lstResponseDetail);
                AfterSave(lstResponseDetail,iret);
        }

        void AfterSave(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            if (iret > 0)
            {
                action = DatabaseConstant.Action.XEM;
                SetDataForm(objHDTDVM.SO_GIAO_DICH);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG,
                DatabaseConstant.Table.TD_HDTDVM,
                DatabaseConstant.Action.SUA,
                lstId);
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
                if ((bool)cbMultiAdd.IsChecked)
                    ResetForm();
            }
        }

        void AfterDelete(string sSoGiaoDich, List<ClientResponseDetail> ResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(ResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG,
            DatabaseConstant.Table.TD_HDTDVM,
            DatabaseConstant.Action.XOA,
            lstId);
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            SetDataForm(sSoGiaoDich);
            if (dtKhachHang.Rows.Count < 1)
                CommonFunction.CloseUserControl(this);
        }

        void OnDelete()
        {
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            int bResult = new TinDungProcess().XoaHopDongTinDungViMo(objHDTDVM, ref ResponseDetail);
            AfterDelete(txtSoGiaoDich.Text, ResponseDetail);
        }

        void BeforeDelete()
        {
            if (tlbDelete.IsEnabled == false)
                return;
            try
            {
                if (!txtSoGiaoDich.Text.IsNullOrEmptyOrSpace())
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        List<TD_HDTDVM> lstHDTDVM = new List<TD_HDTDVM>();
                        foreach (DataRow dr in dtKhachHang.Rows)
                        {
                            lstId.Add(Convert.ToInt32(dr["ID"]));
                            objHDTDVM.HDTD_VM = new TD_HDTDVM();
                            objHDTDVM.HDTD_VM.ID = Convert.ToInt32(dr["ID"]);
                            objHDTDVM.HDTD_VM.MA_HDTDVM = dr["MA_HDTDVM"].ToString();
                            objHDTDVM.HDTD_VM.TTHAI_NVU = TThaiNVu;
                            objHDTDVM.HDTD_VM.SO_GDICH = txtSoGiaoDich.Text;
                            lstHDTDVM.Add(objHDTDVM.HDTD_VM);
                        }
                        objHDTDVM.DSACH_HDTD_VM = lstHDTDVM.ToArray();
                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG,
                        DatabaseConstant.Table.TD_HDTDVM,
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
                DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG,
                DatabaseConstant.Table.TD_HDTDVM,
                DatabaseConstant.Action.XOA,
                lstId);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void AfterApprove(string sSoGiaoDich, List<ClientResponseDetail> ResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(ResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG,
            DatabaseConstant.Table.TD_HDTDVM,
            DatabaseConstant.Action.DUYET,
            lstId);
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            SetDataForm(sSoGiaoDich);
        }

        void OnApprove()
        {
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            int bResult = new TinDungProcess().DuyetHopDongTinDungViMo(objHDTDVM, ref ResponseDetail);
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
                        List<TD_HDTDVM> lstHDTDVM = new List<TD_HDTDVM>();
                        foreach (DataRow dr in dtKhachHang.Rows)
                        {
                            lstId.Add(Convert.ToInt32(dr["ID"])); 
                        }
                        objHDTDVM.HDTD_VM = new TD_HDTDVM();
                        objHDTDVM.HDTD_VM.ID = 0;
                        objHDTDVM.HDTD_VM.TTHAI_NVU = TThaiNVu;
                        objHDTDVM.HDTD_VM.SO_GDICH = txtSoGiaoDich.Text;
                        lstHDTDVM.Add(objHDTDVM.HDTD_VM);
                        objHDTDVM.DSACH_HDTD_VM = lstHDTDVM.ToArray();
                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG,
                        DatabaseConstant.Table.TD_HDTDVM,
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
                DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG,
                DatabaseConstant.Table.TD_HDTDVM,
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
            DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG,
            DatabaseConstant.Table.TD_HDTDVM,
            DatabaseConstant.Action.TU_CHOI_DUYET,
            lstId);
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            SetDataForm(sSoGiaoDich);
        }

        void OnRefuse()
        {
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            int bResult = new TinDungProcess().TuChoiDuyetHopDongTinDungViMo(objHDTDVM, ref ResponseDetail);
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
                        List<TD_HDTDVM> lstHDTDVM = new List<TD_HDTDVM>();
                        foreach (DataRow dr in dtKhachHang.Rows)
                        {
                            lstId.Add(Convert.ToInt32(dr["ID"]));
                        }
                        objHDTDVM.HDTD_VM = new TD_HDTDVM();
                        objHDTDVM.HDTD_VM.ID = 0;
                        objHDTDVM.HDTD_VM.TTHAI_NVU = TThaiNVu;
                        objHDTDVM.HDTD_VM.SO_GDICH = txtSoGiaoDich.Text;
                        lstHDTDVM.Add(objHDTDVM.HDTD_VM);
                        objHDTDVM.DSACH_HDTD_VM = lstHDTDVM.ToArray();
                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG,
                        DatabaseConstant.Table.TD_HDTDVM,
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
                DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG,
                DatabaseConstant.Table.TD_HDTDVM,
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
            DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG,
            DatabaseConstant.Table.TD_HDTDVM,
            DatabaseConstant.Action.THOAI_DUYET,
            lstId);
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            SetDataForm(sSoGiaoDich);
        }

        void OnCancel()
        {
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            int bResult = new TinDungProcess().ThoaiDuyetHopDongTinDungViMo(objHDTDVM, ref ResponseDetail);
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
                        List<TD_HDTDVM> lstHDTDVM = new List<TD_HDTDVM>();
                        foreach (DataRow dr in dtKhachHang.Rows)
                        {
                            lstId.Add(Convert.ToInt32(dr["ID"]));
                        }
                        objHDTDVM.HDTD_VM = new TD_HDTDVM();
                        objHDTDVM.HDTD_VM.ID = 0;
                        objHDTDVM.HDTD_VM.TTHAI_NVU = TThaiNVu;
                        objHDTDVM.HDTD_VM.SO_GDICH = txtSoGiaoDich.Text;
                        lstHDTDVM.Add(objHDTDVM.HDTD_VM);
                        objHDTDVM.DSACH_HDTD_VM = lstHDTDVM.ToArray();
                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG,
                        DatabaseConstant.Table.TD_HDTDVM,
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
                DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG,
                DatabaseConstant.Table.TD_HDTDVM,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void OnPrint()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(objHDTDVM.SO_GIAO_DICH))
            {
                LMessage.ShowMessage("Không có thông tin hợp đồng cần xử lý", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                // Cảnh báo phải lựa chọn hợp đồng
                List<string> lstMaHDTD = new List<string>();
                List<DataRow> listDataRow = getListSeletedDataRow();
                if (listDataRow.Count == 0)
                {
                    LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                    return;
                }
                else
                {
                    foreach (DataRow dr in listDataRow)
                    {
                        lstMaHDTD.Add(dr["MA_HDTDVM"].ToString());
                    }
                }

                VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();

                GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.TDVM_HDTD;
                objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG;

                List<Presentation.Process.BaoCaoServiceRef.TDVM_HDTD> listTDVM_HDTD = new List<Presentation.Process.BaoCaoServiceRef.TDVM_HDTD>(); 
                foreach (string maHDTDVM in lstMaHDTD )
                {
                    Presentation.Process.BaoCaoServiceRef.TDVM_HDTD objGDKT_GIAO_DICH = new Presentation.Process.BaoCaoServiceRef.TDVM_HDTD();
                    objGDKT_GIAO_DICH.SoHopDong = maHDTDVM;
                    listTDVM_HDTD.Add(objGDKT_GIAO_DICH);
                }
                

                doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                doiTuongBaoCao.listTDVM_HDTD = listTDVM_HDTD.ToArray();
                xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
            }
        }

        /// <summary>
        /// Lấy danh sách được chọn
        /// </summary>
        /// <returns></returns>
        private List<DataRow> getListSeletedDataRow()
        {
            List<DataRow> listDataRow = new List<DataRow>();
            if (raddgrDSThanhVien.SelectedItems.Count <= 0)
            {
                return listDataRow;
            }
            else
            {
                for (int i = 0; i < raddgrDSThanhVien.SelectedItems.Count; i++)
                {
                    DataRow dr = (DataRow)raddgrDSThanhVien.SelectedItems[i];
                    listDataRow.Add(dr);
                }
                return listDataRow;
            }
        }
        #endregion
    }
}
