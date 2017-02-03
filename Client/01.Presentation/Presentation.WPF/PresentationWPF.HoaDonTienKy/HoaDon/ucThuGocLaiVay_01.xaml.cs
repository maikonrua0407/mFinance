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
using Presentation.Process.PopupServiceRef;
using PresentationWPF.CustomControl;
using Utilities.Common;
using System.Data;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using Presentation.Process;
using Presentation.Process.TinDungServiceRef;
using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process.Common;
using Presentation.Process.BaoCaoServiceRef;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.DanhMucServiceRef;
using Aspose.Cells;
using System.IO;
using Telerik.Windows.Controls.GridView;
using System.Collections;
using System.Reflection;

namespace PresentationWPF.HoaDonTienKy.HoaDon
{
    /// <summary>
    /// Interaction logic for ucThuGocLaiVay_01.xaml
    /// </summary>
    public partial class ucThuGocLaiVay_01 : UserControl
    {
        #region Khai bao
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
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        private DANH_SACH_KHE_UOC_VONG_VAY objKheUoc = null;
        public void LayDuLieuTuPopup(DANH_SACH_KHE_UOC_VONG_VAY _obj)
        {
            objKheUoc = _obj;
        }

        private ListCheckBoxCombo lstSourceKyThu = new ListCheckBoxCombo();
        private List<AutoCompleteEntry> lstSourceCanBoThuTien = new List<AutoCompleteEntry>();

        private int idCum = -1;
        private int idNhom = -1;

        private TDVM_LAP_HOA_DON_TIEN_KY _objHoaDon = null;
        private List<DANH_SACH_KHE_UOC_VONG_VAY> _lstKheUoc = null;
        private KIEM_SOAT _objKiemSoat = null;

        private DatabaseConstant.Function _function = DatabaseConstant.Function.TDVM_LAP_HOA_DON_TIEN_KY;
        private string tthaiNvu = "";
        public event EventHandler OnSavingCompleted;
        List<string> lstNgayThuTien = null;
        #endregion

        #region Khoi tao
        public ucThuGocLaiVay_01()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.HoaDonTienKy;component/HoaDon/ucThuGocLaiVay_01.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            KhoiTaoBinding();
            LoadComboboxCanBo();
            ShowControl();
            ClearForm();
            btnCum.Focus();
            
            if (ClientInformation.Company.Equals("BANTAYVANG"))
            {
                tlbPreviewKHoach.Visibility = Visibility.Collapsed;
            }
            else 
            { }
        }

        public ucThuGocLaiVay_01(KIEM_SOAT objKiemSoat)
            : this()
        {
            _objKiemSoat = objKiemSoat;
            tthaiNvu = _objKiemSoat.TTHAI_NVU;
            GetThongTinGiaoDich(_objKiemSoat.ID);
            LayThongTinDotThu();
            beforeModifyFromList(_objKiemSoat.action);

            if (ClientInformation.Company.Equals("BANTAYVANG"))
            {
                tlbPreviewKHoach.Visibility = Visibility.Collapsed;
            }
            else
            { }
        }

        private void KhoiTaoBinding()
        {
            try
            {
                if (_lstKheUoc == null)
                {
                    _lstKheUoc = new List<DANH_SACH_KHE_UOC_VONG_VAY>();
                }
                raddgrGocLaiVayDS.ItemsSource = _lstKheUoc;

                //MA 20130322
                var tongKH = new AggregateFunction<DANH_SACH_KHE_UOC_VONG_VAY, decimal>
                {
                    AggregationExpression = ku => ku.Sum(r => r.KE_HOACH_TKQD + r.KE_HOACH_GOC_VAY + r.KE_HOACH_LAI_TRONG_HAN + r.KE_HOACH_LAI_QUA_HAN),
                    ResultFormatString = "{0:n0}",
                };
                GridViewExpressionColumn columnKH = this.raddgrGocLaiVayDS.Columns["TongKH"] as GridViewExpressionColumn;
                columnKH.AggregateFunctions.Add(tongKH);
                var tongTT = new AggregateFunction<DANH_SACH_KHE_UOC_VONG_VAY, decimal>
                {
                    AggregationExpression = ku => ku.Sum(r => r.THUC_THU_TKQD + r.THUC_THU_GOC_VAY + r.THUC_THU_LAI_TRONG + r.THUC_THU_LAI_QUA_HAN + r.THUC_THU_NOP_VAO_TKKKH + r.PHI_TRA_TRUOC + r.THUC_THU_QUY_TT),
                    ResultFormatString = "{0:n0}",
                };
                GridViewExpressionColumn columnTT = this.raddgrGocLaiVayDS.Columns["TongTT"] as GridViewExpressionColumn;
                columnTT.AggregateFunctions.Add(tongTT);
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.HoaDonTienKy.HoaDon.ucThuGocLaiVay_01", "");
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
        #endregion

        /// <summary>
        /// Dang ky hot key, shortcut key
        /// </summary>
        #region Dang ky hot key, shortcut key
        private void BindHotkey()
        {
            try
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
                        else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
                        {
                            KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                            key = new KeyBinding(SubmitCommand, keyg);
                            key.Gesture = keyg;
                        }
                        else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
                        {
                            KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift);
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
            catch (Exception ex)
            {

            }
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
            //e.CanExecute = tlbHold.IsEnabled;
        }
        private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onHold();
        }
        private void SubmitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbSubmit.IsEnabled;
        }
        private void SubmitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
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

            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                beforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ClearForm();
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAP_DU_LIEU)))
            {
                onImport();
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
            else if (strTinhNang.Equals("PreviewKHoach"))
            {
                OnPreviewKeHoach();
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
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                beforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ClearForm();
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
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                onClose();
            }
            else if (strTinhNang.Equals("Detail"))
            {
                btnDetail_Click(null, null);
            }
        }

        private void ClearForm()
        {
            teldtNgayThuTien.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            teldtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            tthaiNvu = "";
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.TDVM_LAP_HOA_DON_TIEN_KY);
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

        private void btnCum_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                lstPopup.Clear();

                this.Cursor = Cursors.Wait;

                lstDieuKien.Add(Presentation.Process.Common.ClientInformation.MaDonViGiaoDich);
                if (ClientInformation.Company.Equals("PHUTHO"))
                {
                    process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DM_CUM.getValue(), lstDieuKien);
                    SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;

                    ucPopup popup = new ucPopup(true, simplePopupResponse, false);
                    popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                    Window win = new Window();
                    win.Title = LLanguage.SearchResourceByKey("M.TinDung.HoaDon.ucHDThuTienKyCT_01.DanhSachCum");
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    win.Content = popup;
                    win.ShowDialog();
                    if (lstPopup != null && lstPopup.Count > 0)
                    {
                        DataRow dr = lstPopup[0];
                        if (idCum != Convert.ToInt32(dr[1]))
                        {
                            idNhom = Convert.ToInt32(dr[1]);
                            idCum = Convert.ToInt32(dr[5]);
                            txtCum.Tag = dr[4].ToString(); // ma don vi
                            txtCum.Text = dr[2].ToString(); // ma cum
                            lblTenCum.Content = dr[3].ToString(); // ten cum
                            cmbCanBoThuTien.SelectedIndex = lstSourceCanBoThuTien.IndexOf(lstSourceCanBoThuTien.FirstOrDefault(f => f.KeywordStrings[5].Equals(dr[6].ToString())));
                            if (LayThongTinDotThu())
                            {
                                if (_lstKheUoc != null)
                                {
                                    _lstKheUoc.Clear();
                                }
                                raddgrGocLaiVayDS.Rebind();
                            }
                        }
                    }
                }
                else
                {
                    process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_CUM.getValue(), lstDieuKien);
                    SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;

                    ucPopup popup = new ucPopup(true, simplePopupResponse, false);
                    popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                    Window win = new Window();
                    win.Title = LLanguage.SearchResourceByKey("M.TinDung.HoaDon.ucHDThuTienKyCT_01.DanhSachCum");
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    win.Content = popup;
                    win.ShowDialog();
                    if (lstPopup != null && lstPopup.Count > 0)
                    {
                        DataRow dr = lstPopup[0];
                        if (idCum != Convert.ToInt32(dr[1]))
                        {
                            idCum = Convert.ToInt32(dr[1]);
                            txtCum.Tag = dr[4].ToString(); // ma don vi
                            txtCum.Text = dr[2].ToString(); // ma cum
                            lblTenCum.Content = dr[3].ToString(); // ten cum
                            cmbCanBoThuTien.SelectedIndex = lstSourceCanBoThuTien.IndexOf(lstSourceCanBoThuTien.FirstOrDefault(f => f.KeywordStrings[1].Equals(dr[6].ToString())));
                            if (LayThongTinDotThu())
                            {
                                if (_lstKheUoc != null)
                                {
                                    _lstKheUoc.Clear();
                                }
                                raddgrGocLaiVayDS.Rebind();
                            }
                        }
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
                this.Cursor = Cursors.Arrow;
            }
        }

        private void teldtNgayThuTien_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (idCum != 0)
            {
                if (LayThongTinDotThu())
                {
                    if (!LObject.IsNullOrEmpty(_lstKheUoc))
                        _lstKheUoc.Clear();
                    else
                        _lstKheUoc = new List<DANH_SACH_KHE_UOC_VONG_VAY>();
                    raddgrGocLaiVayDS.Rebind();
                }
            }
        }

        private bool LayThongTinDotThu()
        {
            bool kq = true;
            try
            {
                this.Cursor = Cursors.Wait;
                AutoComboBox auCB = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(Convert.ToDateTime(teldtNgayThuTien.Value).ToString("yyyyMMdd"));
                lstDieuKien.Add("THU_VON");
                lstDieuKien.Add(idCum.ToString());
                lstSourceKyThu = new ListCheckBoxCombo();
                new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceKyThu, ref cmbKyThu, "COMBOBOX_DOT_THU_PHAT", lstDieuKien);
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
            return kq;
        }

        private void raddgrGocLaiVayDS_CellValidated(object sender, GridViewCellValidatedEventArgs e)
        {
            DANH_SACH_KHE_UOC_VONG_VAY obj = (DANH_SACH_KHE_UOC_VONG_VAY)this.raddgrGocLaiVayDS.CurrentCellInfo.Item;
            decimal tongTienThu = obj.THUC_THU_GOC_VAY + obj.THUC_THU_LAI_QUA_HAN + obj.THUC_THU_LAI_TRONG + obj.THUC_THU_NOP_VAO_TKKKH;

            if (obj.NOP_TIEN_TU_TKKKH == "CO")
            {
                obj.THUC_THU_TKKKH = tongTienThu + obj.THUC_THU_TKQD;
            }
            else
            {
                obj.THUC_THU_TIEN_MAT = tongTienThu + obj.THUC_THU_TKQD;
            }
        }

        private void SetEnabledAllControls(bool enable)
        {
            raddgrGocLaiVayDS.IsReadOnly = !enable;
            grbThongTinChung.IsEnabled = enable;
            btnAdd.IsEnabled = enable;
            btnDelete.IsEnabled = enable;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (LString.IsNullOrEmptyOrSpace(txtCum.Text))
            {
                LMessage.ShowMessage("M.TinDung.HoaDon.ucHDThuTienKyCT_01.ChuaChonCum", LMessage.MessageBoxType.Warning);
                txtCum.Focus();
                return;
            }

            
            string ngayThuTien = Convert.ToDateTime(teldtNgayThuTien.Value).ToString("yyyyMMdd");

            List<string> lstDieuKien = new List<string>();
            //@MA_DVI#@LST_IDKHANG#@MA_CUM#@NGAY_THU_TIEN#@NGAY_HOP_CUM
            if (ClientInformation.Company.Equals("PHUTHO"))
            {
                lstDieuKien.Add(idNhom.ToString());
            }
            else
                lstDieuKien.Add(idCum.ToString());
            lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);

            //Lay thong tin ky thu
            ngayThuTien = "";
            foreach (AutoCompleteCheckBox autoNgayThu in lstSourceKyThu)
            {
                if (autoNgayThu.CheckedMember && !autoNgayThu.ValueMember[0].Equals("All"))
                {
                    ngayThuTien += autoNgayThu.ValueMember[2] + ",";
                }
            }
            if (ngayThuTien.Length <= 1)
            {
                LMessage.ShowMessage("M.TinDung.HoaDon.ucHDThuTienKyCT_01.ChuaChonKyThu", LMessage.MessageBoxType.Warning);
                cmbKyThu.Focus();
                return;
            }

            lstDieuKien.Add("(" + ngayThuTien.Substring(0, ngayThuTien.Length - 1) + ")");

            if (raddgrGocLaiVayDS.Items.Count > 0)
            {
                if (lstDieuKien.Count == 3)
                {
                    lstDieuKien.Add("");
                }
                lstDieuKien[3] = "(";
                foreach (DANH_SACH_KHE_UOC_VONG_VAY drv in raddgrGocLaiVayDS.Items)
                {
                    lstDieuKien[3] += drv.ID_KHACH_HANG.ToString() + ",";
                }
                lstDieuKien[3] = lstDieuKien[3].Substring(0, lstDieuKien[3].Length - 1) + ")";
            }
            else
            {
                lstDieuKien.Add("(0)");
            }
            

            lstPopup.Clear();

            if (grbThongTinChung.IsExpanded == true)
            {
                grbThongTinChung.IsExpanded = false;
            }

            try
            {
                //PopupProcess popupProcess = new PopupProcess();
                //popupProcess.getPopupInformation("POPUP_DS_KH_HOA_DON_TK_BTV", lstDieuKien);
                //SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                //ucPopup popup = new ucPopup(true, simplePopupResponse, true);
                //popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                //Window win = new Window();
                //win.Content = popup;
                //win.Title = LLanguage.SearchResourceByKey("M.TinDung.HoaDon.ucHDThuTienKyCT_01.DanhSachKhachHang");
                //win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                //win.ShowDialog();

                List<DANH_SACH_KHE_UOC_VONG_VAY> lstKheUoc = null;

                HoaDonTienKyProcess process = new HoaDonTienKyProcess();
                DataSet ds = null;
                ApplicationConstant.DonViSuDung CompanyCode = ApplicationConstant.layDonViSuDung(ClientInformation.Company);
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@ID_CUM", "String", lstDieuKien[0]);
                LDatatable.AddParameter(ref dt, "@NGAY_HIEN_TAI", "String", lstDieuKien[1]);
                LDatatable.AddParameter(ref dt, "@NGAY_THU", "String", lstDieuKien[2]);
                LDatatable.AddParameter(ref dt, "@ID_KHANG", "String", lstDieuKien[3]);
                switch (CompanyCode)
                {
                    case ApplicationConstant.DonViSuDung.BANTAYVANG:
                        ds = process.getDSKhachHangBTV(lstDieuKien[0], lstDieuKien[1], lstDieuKien[2], lstDieuKien[3]);
                        break;
                    case ApplicationConstant.DonViSuDung.BENTRE:
                        ds = process.getDSKhachHangBENTRE(lstDieuKien[0], lstDieuKien[1], lstDieuKien[2], lstDieuKien[3]);
                        break;
                    case ApplicationConstant.DonViSuDung.PHUTHO:
                        ds = process.getDSKhachHangPhuTho(lstDieuKien[0], lstDieuKien[1], lstDieuKien[2], lstDieuKien[3]);
                        break;
                    case ApplicationConstant.DonViSuDung.QUANGBINH:
                        ds = process.getDSKhachHangBENTRE(lstDieuKien[0], lstDieuKien[1], lstDieuKien[2], lstDieuKien[3]);
                        break;
                    case ApplicationConstant.DonViSuDung.BIDV:
                        ds = process.getDSKhachHangBIDV(dt);
                        break;
                    case ApplicationConstant.DonViSuDung.BIDV_BLF:
                        ds = process.getDSKhachHangBIDV(dt);
                        break;
                    default:
                        ds = process.getDSKhachHangBTV(lstDieuKien[0], lstDieuKien[1], lstDieuKien[2], lstDieuKien[3]);
                        break;
                }
                if (ds != null && ds.Tables.Count > 0)
                {
                    lstKheUoc = new List<DANH_SACH_KHE_UOC_VONG_VAY>();
                    foreach (DataRow drv in ds.Tables[0].Rows)
                    {
                        DANH_SACH_KHE_UOC_VONG_VAY obj = new DANH_SACH_KHE_UOC_VONG_VAY();
                        obj.MA_KHACH_HANG = drv["MA_KHACH_HANG"].ToString();
                        obj.ID_KHACH_HANG = Convert.ToInt32(drv["ID_KHACH_HANG"]);
                        obj.TEN_KHACH_HANG = drv["TEN_KHACH_HANG"].ToString();
                        //foreach (DataRow dr in lstPopup)
                        //{
                        //    if (obj.ID_KHACH_HANG == Convert.ToInt32(dr[1]))
                        //    {
                        //        obj.TEN_KHACH_HANG = dr[3].ToString();
                        //    }
                        //}

                        if (drv["ID_KHE_UOC"] != DBNull.Value)
                        {
                            obj.ID_KHE_UOC = Convert.ToInt32(drv["ID_KHE_UOC"]);
                        }

                        if (lstKheUoc.IsNullOrEmpty())
                        {
                            lstKheUoc = new List<DANH_SACH_KHE_UOC_VONG_VAY>();
                        }
                        lstKheUoc.Add(obj);
                    }
                    ConvertDataSetToObject(ref lstKheUoc, ds);
                    if (lstKheUoc != null && lstKheUoc.Count > 0)
                    {
                        List<ClientResponseDetail> lstResponse = new List<ClientResponseDetail>();
                        if (_objHoaDon == null)
                        {
                            _objHoaDon = new TDVM_LAP_HOA_DON_TIEN_KY();
                        }
                        _objHoaDon.NGAY_THU_TIEN_KY = ngayThuTien;
                        _objHoaDon.DSACH_KHE_UOC = lstKheUoc.ToArray();

                        if (_lstKheUoc == null)
                        {
                            _lstKheUoc = new List<DANH_SACH_KHE_UOC_VONG_VAY>();
                        }
                        _lstKheUoc.AddRange(_objHoaDon.DSACH_KHE_UOC.ToList());
                        _objHoaDon.DSACH_KHE_UOC = null;
                        raddgrGocLaiVayDS.ItemsSource = _lstKheUoc;
                        raddgrGocLaiVayDS.Rebind();
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
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            foreach (DANH_SACH_KHE_UOC_VONG_VAY obj in raddgrGocLaiVayDS.SelectedItems)
            {
                _lstKheUoc.Remove(obj);
            }
            raddgrGocLaiVayDS.ItemsSource = _lstKheUoc;
            raddgrGocLaiVayDS.Rebind();
        }

        private void btnDetail_Click(object sender, RoutedEventArgs e)
        {
            ChiTietThucThu();
        }

        private void LoadComboboxCanBo()
        {
            try
            {
                this.Cursor = Cursors.Wait;
                AutoComboBox auCB = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                auCB.GenAutoComboBox(ref lstSourceCanBoThuTien, ref cmbCanBoThuTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NHAN_SU.getValue(), lstDieuKien);
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void chkMoRongNhom_Checked(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            raddgrGocLaiVayDS.ExpandAllGroups();
            chkMoRongNhom.IsChecked = false;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void chkThuHepNhom_Checked(object sender, RoutedEventArgs e)
        {
            raddgrGocLaiVayDS.CollapseAllGroups();
            chkThuHepNhom.IsChecked = false;
        }

        private void chkLocKeHoach_Click(object sender, RoutedEventArgs e)
        {
            bool check = Convert.ToBoolean(chkLocKeHoach.IsChecked);
            foreach (var obj in raddgrGocLaiVayDS.Columns)
            {
                if (obj.ColumnGroupName == "KeHoach")
                {
                    obj.IsFilterable = check;
                    if (check == false)
                    {
                        obj.ClearFilters();
                    }
                }
            }
        }

        private void chkLocThucThu_Click(object sender, RoutedEventArgs e)
        {
            bool check = Convert.ToBoolean(chkLocThucThu.IsChecked);
            foreach (var obj in raddgrGocLaiVayDS.Columns)
            {
                if (obj.ColumnGroupName == "ThucTe")
                {
                    obj.IsFilterable = check;
                    if (check == false)
                    {
                        obj.ClearFilters();
                    }
                }
            }
        }

        private void cmbKyThu_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceKyThu = cmbKyThu.ItemsSource as ListCheckBoxCombo;
        }

        private void raddgrGocLaiVayDS_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            DANH_SACH_KHE_UOC_VONG_VAY objKUOCVM = e.Cell.ParentRow.Item as DANH_SACH_KHE_UOC_VONG_VAY;
            int index = _lstKheUoc.IndexOf(objKUOCVM);
            if (!LObject.IsNullOrEmpty(objKUOCVM.DSACH_SO_RUT_TIEN))
                objKUOCVM.DSACH_SO_RUT_TIEN.ToList().ForEach(f => f.SO_TIEN_RUT_RA = 0);
            if (!LObject.IsNullOrEmpty(e.Cell.Column.UniqueName))
            {
                if(e.Cell.Column.UniqueName.Equals("THUC_THU_NOP_VAO_TKKKH"))
                {
                    if (!LObject.IsNullOrEmpty(objKUOCVM.DSACH_SO_NOP_TIEN))
                    {
                        objKUOCVM.DSACH_SO_NOP_TIEN.ToList().FirstOrDefault(f=>f.LOAI_SAN_PHAM_HDV!="T01").SO_TIEN_NOP_VAO = Convert.ToDecimal(e.NewData);
                    }
                }
                if (e.Cell.Column.UniqueName.Equals("THUC_THU_TKQD"))
                {
                    if (!LObject.IsNullOrEmpty(objKUOCVM.DSACH_SO_NOP_TIEN))
                    {
                        objKUOCVM.DSACH_SO_NOP_TIEN.ToList().FirstOrDefault(f => f.LOAI_SAN_PHAM_HDV == "T01").SO_TIEN_NOP_VAO = Convert.ToDecimal(e.NewData);
                    }
                }
                else if (e.Cell.Column.UniqueName.Equals("THUC_THU_GOC_VAY"))
                {
                    decimal soTienGoc = Convert.ToDecimal(e.NewData);
                    foreach(THONG_TIN_THU_NO objThuNo in objKUOCVM.DSACH_THONG_TIN_THU_NO)
                    {
                        decimal soTienThu = Math.Min(soTienGoc, objThuNo.GOC_KH);
                        soTienGoc -= soTienThu;
                        objThuNo.GOC_TT = soTienThu;
                        if (soTienGoc == 0)
                            break;
                    }
                }
                else if (e.Cell.Column.UniqueName.Equals("THUC_THU_LAI_TRONG"))
                {
                    decimal soTienGoc = Convert.ToDecimal(e.NewData);
                    foreach (THONG_TIN_THU_NO objThuNo in objKUOCVM.DSACH_THONG_TIN_THU_NO)
                    {
                        decimal soTienThu = Math.Min(soTienGoc, objThuNo.LAI_KH);
                        soTienGoc -= soTienThu;
                        objThuNo.LAI_TT = soTienThu;
                        if (!objThuNo.NHOM_NO.Equals("part0"))
                            objKUOCVM.THUC_THU_LAI_QUA_HAN += soTienThu;
                        if (soTienGoc == 0)
                            break;
                    }
                }
            }
        }

        private void GetThongTinGiaoDich(int idGiaoDich)
        {
            TinDungProcess process = new TinDungProcess();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                txtSoGiaoDich.Text = _objKiemSoat.SO_GIAO_DICH;

                DataSet ds = process.getThongTinHDTKTheoIDGiaoDich(idGiaoDich);
                if (ds != null)
                {
                    // VKT_TDVM_HDTK_CHUNG
                    if (ds.Tables["VKT_TDVM_HDTK_CHUNG"] != null && ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows.Count > 0)
                    {
                        idCum = Convert.ToInt32(ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["ID_CUM"]);
                        txtCum.Text = ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["MA_CUM"].ToString();
                        txtCum.Tag = ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["MA_DVI"].ToString();
                        lblTenCum.Content = ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["TEN_CUM"].ToString();
                        teldtNgayLap.Value = LDateTime.StringToDate(ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["NGAY_LAP"].ToString(), "yyyyMMdd");
                        teldtNgayThuTien.Value = LDateTime.StringToDate(ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["NGAY_THU_TIEN"].ToString().Substring(0,6)+"01", "yyyyMMdd");
                        lstNgayThuTien = ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["NGAY_THU_TIEN"].ToString().Split('#').ToList();
                        txtDienGiai.Text = ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["DIEN_GIAI"].ToString();
                        txtNguoiLap.Text = ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["NGUOI_NHAP"].ToString();
                        txtNguoiCapNhat.Text = ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["NGUOI_CNHAT"].ToString();
                        cmbCanBoThuTien.Text = ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["TEN_KHANG"].ToString();
                        txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(_objKiemSoat.TTHAI_NVU);
                        //teldtNgayNhap.Value = LDateTime.StringToDate(ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["NGAY_NHAP"].ToString(), "yyyyMMdd");
                        //lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(_objKiemSoat.TTHAI_NVU);
                        //txtTrangThai.Text = lblTrangThai.Content.ToString();
                        if (!LString.IsNullOrEmptyOrSpace(ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["NGAY_CNHAT"].ToString()))
                        {
                            teldtNgayCNhat.Value = LDateTime.StringToDate(ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["NGAY_CNHAT"].ToString(), "yyyyMMdd");
                        }
                    }
                    LayThongTinGiaoDich(ds);
                    raddgrGocLaiVayDS.ItemsSource = _lstKheUoc;
                    raddgrGocLaiVayDS.Rebind();
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
                process = null;
            }
        }

        private void LayThongTinGiaoDich(DataSet ds)
        {
            if (_lstKheUoc == null)
            {
                _lstKheUoc = new List<DANH_SACH_KHE_UOC_VONG_VAY>();
            }

            if (ds.Tables["VKT_TDVM_HDTK_KUOC"] != null)
            {
                foreach (DataRow dr in ds.Tables["VKT_TDVM_HDTK_KUOC"].Rows)
                {
                    Presentation.Process.TinDungServiceRef.DANH_SACH_KHE_UOC_VONG_VAY obj = new Presentation.Process.TinDungServiceRef.DANH_SACH_KHE_UOC_VONG_VAY();
                    List<DANH_SACH_SO> lstDanhSachNop = new List<DANH_SACH_SO>();
                    List<DANH_SACH_SO> lstDanhSachRut = new List<DANH_SACH_SO>();
                    List<THONG_TIN_THU_NO> lstTTinThuNo = new List<THONG_TIN_THU_NO>();
                    obj.TEN_KHACH_HANG = dr["TEN_KHACH_HANG"].ToString();
                    obj.ID_KHACH_HANG = Convert.ToInt32(dr["ID_KHACH_HANG"]);

                    if (dr["ID_KHE_UOC"] != DBNull.Value)
                    {
                        obj.ID_KHE_UOC = Convert.ToInt32(dr["ID_KHE_UOC"]);
                    }
                    obj.MA_KHE_UOC = dr["MA_KHE_UOC"].ToString();
                    obj.SO_SO_TG = dr["SO_SO_TG"].ToString();
                    obj.SO_SO_TG_KKH = dr["SO_SO_TG_KKH"].ToString();
                    obj.MA_KHACH_HANG = dr["MA_KHACH_HANG"].ToString();

                    if (dr["TEN_NHOM"] != DBNull.Value)
                    {
                        obj.TEN_NHOM = dr["TEN_NHOM"].ToString();
                    }

                    if (dr["NHOM_NO_HIEN_TAI"] != DBNull.Value)
                    {
                        obj.NHOM_NO_HIEN_TAI = dr["NHOM_NO_HIEN_TAI"].ToString();
                    }

                    if (dr["LAI_PHAN_BO_KY_NAY"] != DBNull.Value)
                    {
                        obj.LAI_PHAN_BO_KY_NAY = Convert.ToDecimal(dr["LAI_PHAN_BO_KY_NAY"]);
                    }

                    if (dr["KE_HOACH_TKQD"] != DBNull.Value)
                    {
                        obj.KE_HOACH_TKQD = Convert.ToDecimal(dr["KE_HOACH_TKQD"]);
                    }

                    if (dr["KE_HOACH_GOC_VAY"] != DBNull.Value)
                    {
                        obj.KE_HOACH_GOC_VAY = Convert.ToDecimal(dr["KE_HOACH_GOC_VAY"]);
                    }
                    if (dr["KE_HOACH_LAI_TRONG_HAN"] != DBNull.Value)
                    {
                        obj.KE_HOACH_LAI_TRONG_HAN = Convert.ToDecimal(dr["KE_HOACH_LAI_TRONG_HAN"]);
                    }
                    if (dr["KE_HOACH_LAI_QUA_HAN"] != DBNull.Value)
                    {
                        obj.KE_HOACH_LAI_QUA_HAN = Convert.ToDecimal(dr["KE_HOACH_LAI_QUA_HAN"]);
                    }
                    obj.KE_HOACH_TONG = Convert.ToDecimal(dr["KE_HOACH_TONG"]);

                    if (dr["THUC_THU_TKQD"] != DBNull.Value)
                    {
                        obj.THUC_THU_TKQD = Convert.ToDecimal(dr["THUC_THU_TKQD"]);
                    }
                    if (dr["THUC_THU_GOC_VAY"] != DBNull.Value)
                    {
                        obj.THUC_THU_GOC_VAY = Convert.ToDecimal(dr["THUC_THU_GOC_VAY"]);
                    }
                    if (dr["THUC_THU_LAI_TRONG"] != DBNull.Value)
                    {
                        obj.THUC_THU_LAI_TRONG = Convert.ToDecimal(dr["THUC_THU_LAI_TRONG"]);
                    }
                    if (dr["THUC_THU_LAI_QUA_HAN"] != DBNull.Value)
                    {
                        obj.THUC_THU_LAI_QUA_HAN = Convert.ToDecimal(dr["THUC_THU_LAI_QUA_HAN"]);
                    }
                    if (dr["PHI_TRA_TRUOC"] != DBNull.Value)
                    {
                        obj.PHI_TRA_TRUOC = Convert.ToDecimal(dr["PHI_TRA_TRUOC"]);
                    }
                    if (dr["TONG_TIEN_TRA_TRUOC"] != DBNull.Value)
                    {
                        obj.TONG_TIEN_TRA_TRUOC = Convert.ToDecimal(dr["TONG_TIEN_TRA_TRUOC"]);
                    }
                    obj.THUC_THU_TONG = Convert.ToDecimal(dr["THUC_THU_TONG"]);
                    obj.TONG_SO_TIEN = Convert.ToDecimal(dr["TONG_SO_TIEN"]);
                    obj.THUC_THU_TIEN_MAT = Convert.ToDecimal(dr["THUC_THU_TIEN_MAT"]);


                    if (dr["TAI_KHOAN_TKQD"] != DBNull.Value)
                    {
                        obj.TAI_KHOAN_TKQD = dr["TAI_KHOAN_TKQD"].ToString();
                    }
                    if (dr["TAI_KHOAN_TD"] != DBNull.Value)
                    {
                        obj.TAI_KHOAN_TD = dr["TAI_KHOAN_TD"].ToString();
                    }
                    if (dr["MA_SAN_PHAM_TKQD"] != DBNull.Value)
                    {
                        obj.MA_SAN_PHAM_TKQD = dr["MA_SAN_PHAM_TKQD"].ToString();
                    }
                    if (dr["MA_SAN_PHAM"] != DBNull.Value)
                    {
                        obj.MA_SAN_PHAM_TD = dr["MA_SAN_PHAM"].ToString();
                    }

                    if (dr["GHI_CHU"] != DBNull.Value)
                    {
                        obj.GHI_CHU = dr["GHI_CHU"].ToString();
                    }

                    if (dr["NV_LOAI_NVON"] != DBNull.Value)
                    {
                        obj.NV_LOAI_NVON = dr["NV_LOAI_NVON"].ToString();
                    }

                    if (dr["CHU_KY"] != DBNull.Value)
                    {
                        obj.CHU_KY = Convert.ToInt32(dr["CHU_KY"]);
                    }

                    if (dr["PLOAI_NO"] != DBNull.Value)
                    {
                        obj.PLOAI_NO = dr["PLOAI_NO"].ToString();
                    }

                    if (dr["SO_TIEN_DU_THU"] != DBNull.Value)
                    {
                        obj.SO_TIEN_DU_THU = Convert.ToDecimal(dr["SO_TIEN_DU_THU"]);
                    }

                    if (dr["THUC_THU_QUY_TT"] != DBNull.Value)
                    {
                        obj.THUC_THU_QUY_TT = Convert.ToDecimal(dr["THUC_THU_QUY_TT"]);
                    }

                    if (dr["TAT_TOAN"] != DBNull.Value)
                    {
                        obj.TAT_TOAN = Convert.ToString(dr["TAT_TOAN"]);
                    }

                    if (dr["MA_PHI_TRA_TRUOC"] != DBNull.Value)
                    {
                        obj.MA_PHI_TRA_TRUOC = Convert.ToString(dr["MA_PHI_TRA_TRUOC"]);
                    }

                    if (dr.Table.Columns.Contains("LOAI_TIEN") && dr["LOAI_TIEN"] != DBNull.Value)
                    {
                        obj.LOAI_TIEN = Convert.ToString(dr["LOAI_TIEN"]);
                    }
                    if (dr.Table.Columns.Contains("ID_NHOM") && dr["ID_NHOM"] != DBNull.Value)
                    {
                        obj.ID_NHOM = Convert.ToInt32(dr["ID_NHOM"]);
                    }
                    if (dr.Table.Columns.Contains("DU_NO") && dr["DU_NO"] != DBNull.Value)
                    {
                        obj.DU_NO = Convert.ToInt32(dr["DU_NO"]);
                    }

                    obj.NOP_TIEN_TU_TKKKH = dr["NOP_TIEN_TU_TKKKH"].ToString();
                    obj.NOP_TIEN_VAO_TKKKH = dr["NOP_TIEN_VAO_TKKKH"].ToString();
                    obj.GHI_NHAN_VAO_TKNB = dr["GHI_NHAN_VAO_TKNB"].ToString();
                    obj.TRA_GOC_LAI_TRUOC_HAN = dr["TRA_GOC_LAI_TRUOC_HAN"].ToString();
                    obj.MA_DON_VI = Presentation.Process.Common.ClientInformation.MaDonViGiaoDich;

                    foreach (DataRow drNop in ds.Tables["VKT_TDVM_HDTK_SOTK"].Rows)
                    {
                        if (LString.IsNullOrEmptyOrSpace(obj.MA_KHE_UOC))
                        {
                            if (drNop["MA_KHANG"].ToString().Equals(obj.MA_KHACH_HANG))
                            {
                                if (drNop["LOAI"].ToString() == "NOP")
                                {
                                    DANH_SACH_SO objNop = new DANH_SACH_SO();
                                    objNop.SO_DU = decimal.Parse(drNop["SO_DU"].ToString());
                                    objNop.SO_SO = drNop["SO_SO_TK"].ToString();
                                    objNop.LOAI_SAN_PHAM_HDV = drNop["MA_NHOM_SP"].ToString();
                                    objNop.SO_TAI_KHOAN = "";
                                    objNop.SO_TIEN_NOP_VAO = Convert.ToDecimal(drNop["SO_TIEN_NOP_VAO"].ToString());
                                    objNop.SO_TIEN_RUT_RA = 0;
                                    lstDanhSachNop.Add(objNop);
                                }
                            }
                        }
                        else
                        {
                            if (drNop["MA_KHANG"].ToString().Equals(obj.MA_KHACH_HANG) && drNop["MA_KHE_UOC"].ToString().Equals(obj.MA_KHE_UOC))
                            {
                                if (drNop["LOAI"].ToString() == "NOP")
                                {
                                    DANH_SACH_SO objNop = new DANH_SACH_SO();
                                    objNop.SO_DU = decimal.Parse(drNop["SO_DU"].ToString());
                                    objNop.SO_SO = drNop["SO_SO_TK"].ToString();
                                    objNop.LOAI_SAN_PHAM_HDV = drNop["MA_NHOM_SP"].ToString();
                                    objNop.SO_TAI_KHOAN = "";
                                    objNop.SO_TIEN_NOP_VAO = Convert.ToDecimal(drNop["SO_TIEN_NOP_VAO"].ToString());
                                    objNop.SO_TIEN_RUT_RA = 0;
                                    lstDanhSachNop.Add(objNop);
                                }
                            }
                        }
                    }

                    foreach (DataRow drNop in ds.Tables["VKT_TDVM_HDTK_SOTK"].Rows)
                    {
                        if (LString.IsNullOrEmptyOrSpace(obj.MA_KHE_UOC))
                        {
                            if (drNop["MA_KHANG"].ToString().Equals(obj.MA_KHACH_HANG))
                            {
                                if (drNop["LOAI"].ToString() == "RUT")
                                {
                                    DANH_SACH_SO objNop = new DANH_SACH_SO();
                                    objNop.SO_DU = decimal.Parse(drNop["SO_DU"].ToString());
                                    objNop.SO_SO = drNop["SO_SO_TK"].ToString();
                                    objNop.LOAI_SAN_PHAM_HDV = drNop["MA_NHOM_SP"].ToString();
                                    objNop.SO_TAI_KHOAN = "";
                                    objNop.SO_TIEN_RUT_RA = Convert.ToDecimal(drNop["SO_TIEN_RUT_RA"].ToString());
                                    lstDanhSachRut.Add(objNop);
                                }
                            }
                        }
                        else
                        {
                            if (drNop["MA_KHE_UOC"].ToString().Equals(obj.MA_KHE_UOC))
                            {
                                if (drNop["LOAI"].ToString() == "RUT")
                                {
                                    DANH_SACH_SO objNop = new DANH_SACH_SO();
                                    objNop.SO_DU = decimal.Parse(drNop["SO_DU"].ToString());
                                    objNop.SO_SO = drNop["SO_SO_TK"].ToString();
                                    objNop.LOAI_SAN_PHAM_HDV = drNop["MA_NHOM_SP"].ToString();
                                    objNop.SO_TAI_KHOAN = "";
                                    objNop.SO_TIEN_RUT_RA = Convert.ToDecimal(drNop["SO_TIEN_RUT_RA"].ToString());
                                    lstDanhSachRut.Add(objNop);
                                }
                            }
                        }
                    }
                    if (!ds.Tables["VKT_TDVM_HDTK_KHOACH"].IsNullOrEmpty())
                    {
                        DataRow[] arrdr = ds.Tables["VKT_TDVM_HDTK_KHOACH"].Select("MA_KHE_UOC='" + obj.MA_KHE_UOC.ToString() + "'");
                        THONG_TIN_THU_NO objTTinThuNo = null;
                        decimal thucThuGoc = obj.THUC_THU_GOC_VAY;
                        decimal thucThuLai = obj.THUC_THU_LAI_TRONG;
                        foreach (DataRow dtr in arrdr)
                        {
                            objTTinThuNo = new THONG_TIN_THU_NO();
                            objTTinThuNo.ID_KUOC = obj.ID_KHE_UOC;
                            objTTinThuNo.MA_KUOC = Convert.ToString(dtr["MA_KHE_UOC"]);
                            objTTinThuNo.SO_NGAY_QH = Convert.ToInt32(dtr["SO_NGAY_QH"]);
                            objTTinThuNo.NGAY_KH = Convert.ToString(dtr["KH_NGAY_TRA"]);
                            objTTinThuNo.NHOM_NO = Convert.ToString(dtr["NHOM_NO"]);
                            objTTinThuNo.GOC_KH = Convert.ToDecimal(dtr["KH_TRA_GOC"]);
                            objTTinThuNo.LAI_KH = Convert.ToDecimal(dtr["KH_TRA_LAI"]);
                            objTTinThuNo.LAI_PHAT_KH = Convert.ToDecimal(dtr["KH_TRA_PHI"]);
                            objTTinThuNo.GOC_TT = Convert.ToDecimal(dtr["TT_TRA_GOC"]);
                            objTTinThuNo.LAI_TT = Convert.ToDecimal(dtr["TT_TRA_LAI"]);
                            objTTinThuNo.LAI_PHAT_TT = Convert.ToDecimal(dtr["TT_TRA_PHI"]);
                            lstTTinThuNo.Add(objTTinThuNo);
                        }
                        obj.DSACH_THONG_TIN_THU_NO = lstTTinThuNo.ToArray();
                    }
                    DataTable dt = ds.Tables["VKT_BIEU_PHI"];
                    List<BIEU_PHI_CTIET_DTO> lstBieuPhi = null;
                    if (dt.Rows.Count > 0)
                    {
                        DataRow[] arr = dt.Select("MA_BPHI='" + obj.MA_PHI_TRA_TRUOC + "'");
                        if(arr.Count()>0)
                        {
                            DataRow drd = arr[0];
                            obj.BIEU_PHI = new BIEU_PHI_DTO();
                            obj.BIEU_PHI.HTHUC_BTHANG = drd["HTHUC_BTHANG"].ToString();
                            obj.BIEU_PHI.ID_BPHI = Convert.ToInt32(drd["ID"]);
                            obj.BIEU_PHI.LOAI_BPHI = drd["LOAI_BPHI"].ToString();
                            obj.BIEU_PHI.LOAI_TIEN = drd["MA_LOAI_TIEN"].ToString();
                            obj.BIEU_PHI.MA_BPHI = drd["MA_BPHI"].ToString();
                            obj.BIEU_PHI.NGAY_ADUNG = drd["NGAY_ADUNG"].ToString();
                            if (drd["NGAY_HHAN"] != DBNull.Value)
                                obj.BIEU_PHI.NGAY_HHAN = drd["NGAY_HHAN"].ToString();
                            obj.BIEU_PHI.TCHAT_BPHI = drd["TCHAT_BPHI"].ToString();
                            obj.BIEU_PHI.TEN_BPHI = drd["TEN_BPHI"].ToString();
                            obj.BIEU_PHI.TY_LE_VAT = Convert.ToDecimal(drd["TY_LE_VAT"]);
                            obj.MA_PHI_TRA_TRUOC = drd["MA_BPHI"].ToString();
                        }
                        else
                            obj.BIEU_PHI = new BIEU_PHI_DTO();
                        dt = ds.Tables["VKT_BIEU_PHI_CT"];
                        lstBieuPhi = new List<BIEU_PHI_CTIET_DTO>();
                        if (dt.Rows.Count > 0)
                        {
                            arr = dt.Select("ID_BPHI='" + obj.BIEU_PHI.ID_BPHI.ToString() + "'");
                            foreach (DataRow drd in arr)
                            {
                                BIEU_PHI_CTIET_DTO objBieuPhiCT = new BIEU_PHI_CTIET_DTO();
                                objBieuPhiCT.ID_BPHI = Convert.ToInt32(drd["ID_BPHI"]);
                                objBieuPhiCT.LOAI_BPHI = drd["LOAI_BPHI"].ToString();
                                objBieuPhiCT.MA_BPHI = drd["MA_BPHI"].ToString();
                                objBieuPhiCT.SO_TIEN_TINH_PHI = Convert.ToDecimal(drd["SO_TIEN"]);
                                objBieuPhiCT.SO_TIEN_PHI = Convert.ToDecimal(drd["SO_TIEN_PHI"]);
                                objBieuPhiCT.SO_TIEN_TDA = Convert.ToDecimal(drd["SO_TIEN_TDA"]);
                                objBieuPhiCT.SO_TIEN_TTHIEU = Convert.ToDecimal(drd["SO_TIEN_TTHIEU"]);
                                objBieuPhiCT.TY_LE_PHI = Convert.ToDecimal(drd["TY_LE_PHI"]);
                                objBieuPhiCT.TY_LE_VAT = Convert.ToDecimal(drd["TY_LE_VAT"]);
                                lstBieuPhi.Add(objBieuPhiCT);
                            }

                        }
                        else
                        {
                            lstBieuPhi = new List<BIEU_PHI_CTIET_DTO>();
                        }
                        
                    }
                    else
                    {
                        obj.BIEU_PHI = new BIEU_PHI_DTO();
                    }
                    obj.BIEU_PHI.DSACH_BPHI_CT = lstBieuPhi.ToArray();

                    if (!lstDanhSachNop.Where(f => f.LOAI_SAN_PHAM_HDV != "T01").IsNullOrEmpty())
                        obj.THUC_THU_NOP_VAO_TKKKH = lstDanhSachNop.Where(f => f.LOAI_SAN_PHAM_HDV != "T01").Sum(e => e.SO_TIEN_NOP_VAO);
                    if (!lstDanhSachNop.Where(f => f.LOAI_SAN_PHAM_HDV == "T01").IsNullOrEmpty())
                        obj.THUC_THU_TKQD = lstDanhSachNop.Where(f => f.LOAI_SAN_PHAM_HDV == "T01").Sum(e => e.SO_TIEN_NOP_VAO);
                    obj.THUC_THU_TKKKH = lstDanhSachRut.Sum(e => e.SO_TIEN_RUT_RA);

                    decimal tongTienThu = obj.THUC_THU_GOC_VAY + obj.THUC_THU_LAI_QUA_HAN + obj.THUC_THU_LAI_TRONG + obj.THUC_THU_NOP_VAO_TKKKH + obj.PHI_TRA_TRUOC;

                    if (obj.THUC_THU_NOP_VAO_TKKKH > 0)
                    {
                        obj.NOP_TIEN_VAO_TKKKH = "CO";
                    }

                    if (obj.THUC_THU_TKKKH > 0)
                    {
                        obj.NOP_TIEN_TU_TKKKH = "CO";
                    }
                    else
                    {
                        obj.NOP_TIEN_TU_TKKKH = "KHONG";
                    }

                    //if (obj.NOP_TIEN_TU_TKKKH == "CO")
                    //{
                    //    obj.THUC_THU_TKKKH = tongTienThu + obj.THUC_THU_TKQD;
                    //}
                    //else
                    //{
                    //    obj.THUC_THU_TIEN_MAT = tongTienThu + obj.THUC_THU_TKQD;
                    //}

                    obj.DSACH_SO_NOP_TIEN = lstDanhSachNop.ToArray();
                    obj.DSACH_SO_RUT_TIEN = lstDanhSachRut.ToArray();
                    _lstKheUoc.Add(obj);
                }
            }
        }

        private void chkTatToan_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            DANH_SACH_KHE_UOC_VONG_VAY item = (DANH_SACH_KHE_UOC_VONG_VAY)raddgrGocLaiVayDS.SelectedItem;
            if (chk.IsChecked.GetValueOrDefault())
            {
                item.THUC_THU_GOC_VAY = item.DU_NO;
                item.DSACH_THONG_TIN_THU_NO.ToList().ForEach(f => f.GOC_TT = f.GOC_KH);
                item.DSACH_SO_NOP_TIEN.ToList().ForEach(f => f.SO_TIEN_NOP_VAO = 0);
                item.DSACH_SO_RUT_TIEN.ToList().ForEach(f => f.SO_TIEN_RUT_RA = 0);
                ApplicationConstant.DonViSuDung donViSuDung = ApplicationConstant.layDonViSuDung(ClientInformation.Company);
                switch (donViSuDung)
                {
                    case ApplicationConstant.DonViSuDung.BIDV:
                        TinhPhiTraTruocBIDV(ref item);
                        break;
                    case ApplicationConstant.DonViSuDung.BIDV_BLF:
                        TinhPhiTraTruocBIDV(ref item);
                        break;
                    default:
                        TinhPhiTraTruoc(ref objKheUoc);
                        break;
                }
            }
            else
            {
                item.THUC_THU_GOC_VAY = item.KE_HOACH_GOC_VAY;
                decimal soTienGoc = item.KE_HOACH_GOC_VAY;
                foreach (THONG_TIN_THU_NO objThuNo in item.DSACH_THONG_TIN_THU_NO)
                {
                    decimal soTienThu = Math.Min(soTienGoc, objThuNo.GOC_KH);
                    soTienGoc -= soTienThu;
                    objThuNo.GOC_TT = soTienThu;
                    if (soTienGoc == 0)
                        objThuNo.GOC_TT = soTienGoc;
                }
            }
            raddgrGocLaiVayDS.CurrentItem = item;
            raddgrGocLaiVayDS.Rebind();
        }

        private void raddgrGocLaiVayDS_BeginningEdit(object sender, GridViewBeginningEditRoutedEventArgs e)
        {
            DANH_SACH_KHE_UOC_VONG_VAY objKUOCVM = e.Cell.ParentRow.Item as DANH_SACH_KHE_UOC_VONG_VAY;
            if (!LObject.IsNullOrEmpty(e.Cell.Column.UniqueName))
            {
                if (e.Cell.Column.UniqueName.Equals("THUC_THU_NOP_VAO_TKKKH"))
                {
                    if (LObject.IsNullOrEmpty(objKUOCVM.DSACH_SO_NOP_TIEN.Where(f=>f.LOAI_SAN_PHAM_HDV!="T01")))
                    {
                        e.Cancel = true;
                    }
                }
                if (e.Cell.Column.UniqueName.Equals("THUC_THU_TKQD"))
                {
                    if (LObject.IsNullOrEmpty(objKUOCVM.DSACH_SO_NOP_TIEN.Where(f => f.LOAI_SAN_PHAM_HDV == "T01")))
                    {
                        e.Cancel = true;
                    }
                }
                else if (e.Cell.Column.UniqueName.Equals("THUC_THU_GOC_VAY"))
                {
                    if (LObject.IsNullOrEmpty(objKUOCVM.DSACH_THONG_TIN_THU_NO) || objKUOCVM.DSACH_THONG_TIN_THU_NO.Count()==0)
                    {
                        e.Cancel = true;
                    }
                }
                else if (e.Cell.Column.UniqueName.Equals("THUC_THU_LAI_TRONG"))
                {
                    if (LObject.IsNullOrEmpty(objKUOCVM.DSACH_THONG_TIN_THU_NO) || objKUOCVM.DSACH_THONG_TIN_THU_NO.Count() == 0)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Xu ly nghiep vu
        /// </summary>
        #region Xu ly nghiep vu

        private void beforeView()
        {
            //SetFormData();
            SetEnabledAllControls(false);
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, _function);
            tlbImport.IsEnabled = true;
        }

        private void beforeAddNew()
        {
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, _function);
            tlbImport.IsEnabled = true;
            //lblTrangThai.Content = "";
            //teldtNgayNhap.Value = null;
            teldtNgayCNhat.Value = null;
        }

        private void beforeModifyFromList(DatabaseConstant.Action action)
        {
            //SetFormData();
            if (action == DatabaseConstant.Action.XEM)
            {
                SetEnabledAllControls(false);
            }
            CommonFunction.RefreshButton(Toolbar, action, tthaiNvu, mnuMain, _function);
            tlbImport.IsEnabled = true;

        }

        private void beforeModifyFromDetail()
        {
            // Yêu cầu lock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_objKiemSoat.ID);

            bool ret = process.LockData(DatabaseConstant.Module.TDVM,
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
                tlbImport.IsEnabled = true;
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

                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
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

                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
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

                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
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

                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
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
            HoaDonTienKyProcess process = new HoaDonTienKyProcess();
            try
            {
                Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY obj = new Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                // Dữ liệu truyền vào và dữ liệu trả về

                Mouse.OverrideCursor = Cursors.Wait;
                obj = GetFormData(trangThai);
                // Nếu là lưu tạm hoặc thêm mới lần đầu
                if (_objKiemSoat == null)
                {
                    // Lấy dữ liệu từ form
                    ret = process.ThuGocLaiVay(_function, DatabaseConstant.Action.THEM, ref obj, ref listResponseDetail);

                    Mouse.OverrideCursor = Cursors.Arrow;
                    afterAddNew(ret, obj, listResponseDetail);
                }
                // Nếu là lưu tạm khi sửa
                // Hoặc lưu tạm khi sửa sau duyệt
                // Hoặc sửa
                else
                {
                    // Lấy dữ liệu từ form
                    ret = process.ThuGocLaiVay(_function, DatabaseConstant.Action.SUA, ref obj, ref listResponseDetail);

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
            if (Validation())
            {
                HoaDonTienKyProcess process = new HoaDonTienKyProcess();
                try
                {
                    Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY obj = new Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY();
                    ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                    // Dữ liệu truyền vào và dữ liệu trả về

                    Mouse.OverrideCursor = Cursors.Wait;
                    obj = GetFormData(trangThai);
                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (_objKiemSoat == null)
                    {
                        // Lấy dữ liệu từ form
                        ret = process.ThuGocLaiVay(_function, DatabaseConstant.Action.THEM, ref obj, ref listResponseDetail);

                        Mouse.OverrideCursor = Cursors.Arrow;
                        afterAddNew(ret, obj, listResponseDetail);
                    }
                    // Nếu là lưu tạm khi sửa
                    // Hoặc lưu tạm khi sửa sau duyệt
                    // Hoặc sửa
                    else
                    {
                        // Lấy dữ liệu từ form
                        ret = process.ThuGocLaiVay(_function, DatabaseConstant.Action.SUA, ref obj, ref listResponseDetail);

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
            HoaDonTienKyProcess process = new HoaDonTienKyProcess();
            try
            {
                Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY obj = new Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                // Dữ liệu truyền vào và dữ liệu trả về

                Mouse.OverrideCursor = Cursors.Wait;
                obj = GetFormData(tthaiNvu);
                ret = process.ThuGocLaiVay(_function, DatabaseConstant.Action.XOA, ref obj, ref listResponseDetail);

                Mouse.OverrideCursor = Cursors.Arrow;
                afterDelete(ret, listResponseDetail);

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
            HoaDonTienKyProcess process = new HoaDonTienKyProcess();
            try
            {
                Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY obj = new Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                // Dữ liệu truyền vào và dữ liệu trả về

                Mouse.OverrideCursor = Cursors.Wait;
                obj = GetFormData(tthaiNvu);
                ret = process.ThuGocLaiVay(_function, DatabaseConstant.Action.DUYET, ref obj, ref listResponseDetail);

                Mouse.OverrideCursor = Cursors.Arrow;
                afterApprove(ret, obj, listResponseDetail);
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
            HoaDonTienKyProcess process = new HoaDonTienKyProcess();
            try
            {
                Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY obj = new Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                // Dữ liệu truyền vào và dữ liệu trả về

                Mouse.OverrideCursor = Cursors.Wait;
                obj = GetFormData(tthaiNvu);
                ret = process.ThuGocLaiVay(_function, DatabaseConstant.Action.THOAI_DUYET, ref obj, ref listResponseDetail);

                Mouse.OverrideCursor = Cursors.Arrow;
                afterCancel(ret, obj, listResponseDetail);

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
            HoaDonTienKyProcess process = new HoaDonTienKyProcess();
            try
            {
                Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY obj = new Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                // Dữ liệu truyền vào và dữ liệu trả về

                Mouse.OverrideCursor = Cursors.Wait;
                obj = GetFormData(tthaiNvu);
                ret = process.ThuGocLaiVay(_function, DatabaseConstant.Action.TU_CHOI_DUYET, ref obj, ref listResponseDetail);

                Mouse.OverrideCursor = Cursors.Arrow;
                afterRefuse(ret, obj, listResponseDetail);

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
        /// Nhập dữ liệu từ Excel
        /// </summary>
        private void onImport()
        {
            if (LString.IsNullOrEmptyOrSpace(txtCum.Text))
            {
                LMessage.ShowMessage("M.TinDung.HoaDon.ucHDThuTienKyCT_01.ChuaChonCum", LMessage.MessageBoxType.Warning);
                txtCum.Focus();
                return;
            }

            try
            {
                //Create a License object
                License license = new License();
                //Set the license of Aspose.Cells to avoid the evaluation
                //limitations
                MemoryStream aspLic = new MemoryStream();
                StreamWriter sw = new StreamWriter(aspLic, Encoding.UTF8);
                sw.WriteLine(BusinessConstant.asposeExcelLic);
                sw.Flush();
                aspLic.Position = 0;
                license.SetLicense(aspLic);

                //Open file
                DM_DON_VI phongGD = new DanhMucProcess().getDonViByMaCum(txtCum.Text);
                string maPhong = LObject.IsNullOrEmpty(phongGD) ? "" : phongGD.MA_DVI;
                AutoCompleteEntry auKyThu = (AutoCompleteEntry)cmbKyThu.SelectedItem;
                // Configure open file dialog box
                string xlsFile = "HDTTK_" + maPhong + "_" + txtCum.Text + "_" + "Kỳ " + auKyThu.DisplayName.Substring(7, 1) + "_" + auKyThu.KeywordStrings[0].Substring(4, 2) + "_" + auKyThu.KeywordStrings[0].Substring(0, 4);
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.InitialDirectory = ClientInformation.TempDir;
                //dlg.FileName = xlsFile; // Default file name
                dlg.DefaultExt = ".xls"; // Default file extension
                dlg.Filter = LLanguage.SearchResourceByKey("M.TinDung.HoaDon.ucHDThuTienKyCT_01.FileNameExport") + xlsFile + ".xls"; // Filter files by extension 

                // Show open file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process open file dialog box results 
                if (result == true)
                {
                    this.Cursor = Cursors.Wait;
                    // Open document 
                    Workbook workbook = new Workbook(dlg.FileName);
                    Worksheet worksheet = workbook.Worksheets[0];
                    Cell cell;
                    object cell1, cell2;
                    decimal value1, value2;
                    decimal goc1, lai1, goc2, lai2;
                    int soKhachHang = 0;
                    bool isMatched = false;

                    //Import
                    FindOptions opts = new FindOptions();
                    opts.LookInType = LookInType.Values;
                    opts.LookAtType = LookAtType.EntireContent;
                    opts.SearchNext = true;

                    raddgrGocLaiVayDS.SelectedItems.Clear();
                    foreach (DANH_SACH_KHE_UOC_VONG_VAY drv in raddgrGocLaiVayDS.Items)
                    {
                        isMatched = false;
                        cell = worksheet.Cells.Find(drv.MA_KHACH_HANG, null, opts);

                        if (!cell.IsNullOrEmpty())
                        {
                            if (drv.KE_HOACH_TKQD > 0)
                            {   //TK
                                cell1 = worksheet.Cells[cell.Row, 13].Value;
                                if (!cell1.IsNullOrEmpty() && cell1.ToString().IsNumeric())
                                    value1 = cell1.ToString().StringToDecimal();
                                else
                                    value1 = 0;
                                drv.THUC_THU_TKQD = value1;
                                cell1 = worksheet.Cells[cell.Row, 14].Value;
                                if (!cell1.IsNullOrEmpty() && cell1.ToString().IsNumeric())
                                    value1 = cell1.ToString().StringToDecimal();
                                else
                                    value1 = 0;
                                drv.THUC_THU_NOP_VAO_TKKKH = value1;
                                isMatched = true;
                            }

                            goc1 = 0;
                            lai1 = 0;
                            goc2 = 0;
                            lai2 = 0;
                            while (!cell.IsNullOrEmpty())
                            {   //NH
                                cell1 = worksheet.Cells[cell.Row, 6].Value;
                                if (!cell1.IsNullOrEmpty() && cell1.ToString().IsNumeric())
                                    value1 = cell1.ToString().StringToDecimal();
                                else
                                    value1 = 0;
                                goc1 = value1;
                                cell1 = worksheet.Cells[cell.Row, 7].Value;
                                if (!cell1.IsNullOrEmpty() && cell1.ToString().IsNumeric())
                                    value1 = cell1.ToString().StringToDecimal();
                                else
                                    value1 = 0;
                                lai1 = value1;

                                if (drv.KE_HOACH_GOC_VAY == goc1 && drv.KE_HOACH_LAI_TRONG_HAN == lai1 && goc1 + lai1 > 0)
                                    break;
                                else
                                {   //TH
                                    cell2 = worksheet.Cells[cell.Row, 10].Value;
                                    if (!cell2.IsNullOrEmpty() && cell2.ToString().IsNumeric())
                                        value2 = cell2.ToString().StringToDecimal();
                                    else
                                        value2 = 0;
                                    goc2 = value2;
                                    cell2 = worksheet.Cells[cell.Row, 11].Value;
                                    if (!cell2.IsNullOrEmpty() && cell2.ToString().IsNumeric())
                                        value2 = cell2.ToString().StringToDecimal();
                                    else
                                        value2 = 0;
                                    lai2 = value2;
                                    if (drv.KE_HOACH_GOC_VAY == goc2 && drv.KE_HOACH_LAI_TRONG_HAN == lai2)
                                        break;
                                    else
                                    {
                                        cell = worksheet.Cells[cell.Row + 1, cell.Column];
                                        if (!cell.StringValue.IsNullOrEmptyOrSpace()) cell = null;
                                    }
                                }
                            }

                            if (!cell.IsNullOrEmpty())
                            {
                                if (drv.KE_HOACH_GOC_VAY + drv.KE_HOACH_LAI_TRONG_HAN == 0)
                                {   //
                                    drv.THUC_THU_GOC_VAY = 0;
                                    drv.THUC_THU_LAI_TRONG = 0;
                                }
                                else if (drv.KE_HOACH_GOC_VAY + drv.KE_HOACH_LAI_TRONG_HAN == goc1 + lai1)
                                {   //NH
                                    cell1 = worksheet.Cells[cell.Row, 15].Value;
                                    if (!cell1.IsNullOrEmpty() && cell1.ToString().IsNumeric())
                                        value1 = cell1.ToString().StringToDecimal();
                                    else
                                        value1 = 0;
                                    drv.THUC_THU_GOC_VAY = value1;
                                    cell1 = worksheet.Cells[cell.Row, 16].Value;
                                    if (!cell1.IsNullOrEmpty() && cell1.ToString().IsNumeric())
                                        value1 = cell1.ToString().StringToDecimal();
                                    else
                                        value1 = 0;
                                    drv.THUC_THU_LAI_TRONG = value1;
                                }
                                else
                                {   //TH
                                    cell1 = worksheet.Cells[cell.Row, 17].Value;
                                    if (!cell1.IsNullOrEmpty() && cell1.ToString().IsNumeric())
                                        value2 = cell1.ToString().StringToDecimal();
                                    else
                                        value2 = 0;
                                    drv.THUC_THU_GOC_VAY = value2;
                                    cell1 = worksheet.Cells[cell.Row, 18].Value;
                                    if (!cell1.IsNullOrEmpty() && cell1.ToString().IsNumeric())
                                        value2 = cell1.ToString().StringToDecimal();
                                    else
                                        value2 = 0;
                                    drv.THUC_THU_LAI_TRONG = value2;
                                }

                                isMatched = true;
                            }

                            if (isMatched)
                            {
                                raddgrGocLaiVayDS.SelectedItems.Add(drv);
                                soKhachHang++;
                            }
                        }
                        else
                        {
                            drv.THUC_THU_TKQD = 0;
                            drv.THUC_THU_NOP_VAO_TKKKH = 0;
                            drv.THUC_THU_GOC_VAY = 0;
                            drv.THUC_THU_LAI_TRONG = 0;
                        }



                        //while (!cell.IsNullOrEmpty())
                        //{   //Matched
                        //    cell1 = worksheet.Cells[cell.Row, 6].Value;
                        //    if (!cell1.IsNullOrEmpty() && cell1.ToString().IsNumeric())
                        //        value1 = cell1.ToString().StringToDecimal();
                        //    else
                        //        value1 = 0;
                        //    cell2 = worksheet.Cells[cell.Row, 10].Value;
                        //    if (!cell2.IsNullOrEmpty() && cell2.ToString().IsNumeric())
                        //        value2 = cell2.ToString().StringToDecimal();
                        //    else
                        //        value2 = 0;
                        //    goc = value1 + value2;
                        //    cell1 = worksheet.Cells[cell.Row, 7].Value;
                        //    if (!cell1.IsNullOrEmpty() && cell1.ToString().IsNumeric())
                        //        value1 = cell1.ToString().StringToDecimal();
                        //    else
                        //        value1 = 0;
                        //    cell2 = worksheet.Cells[cell.Row, 11].Value;
                        //    if (!cell2.IsNullOrEmpty() && cell2.ToString().IsNumeric())
                        //        value2 = cell2.ToString().StringToDecimal();
                        //    else
                        //        value2 = 0;
                        //    lai = value1 + value2;
                        //    if (drv.KE_HOACH_GOC_VAY == goc && drv.KE_HOACH_LAI_TRONG_HAN == lai)
                        //        break;
                        //    else
                        //        cell = worksheet.Cells.Find(drv.MA_KHACH_HANG, cell, opts);
                        //}
                        ////Import
                        //if (!cell.IsNullOrEmpty())
                        //{
                        //    soKhachHang++;
                        //    cell1 = worksheet.Cells[cell.Row, 13].Value;
                        //    if (!cell1.IsNullOrEmpty() && cell1.ToString().IsNumeric())
                        //        value1 = cell1.ToString().StringToDecimal();
                        //    else
                        //        value1 = 0;
                        //    drv.THUC_THU_TKQD = value1;
                        //    cell1 = worksheet.Cells[cell.Row, 14].Value;
                        //    if (!cell1.IsNullOrEmpty() && cell1.ToString().IsNumeric())
                        //        value1 = cell1.ToString().StringToDecimal();
                        //    else
                        //        value1 = 0;
                        //    drv.THUC_THU_NOP_VAO_TKKKH = value1;
                        //    cell1 = worksheet.Cells[cell.Row, 15].Value;
                        //    if (!cell1.IsNullOrEmpty() && cell1.ToString().IsNumeric())
                        //        value1 = cell1.ToString().StringToDecimal();
                        //    else
                        //        value1 = 0;
                        //    cell2 = worksheet.Cells[cell.Row, 17].Value;
                        //    if (!cell2.IsNullOrEmpty() && cell2.ToString().IsNumeric())
                        //        value2 = cell2.ToString().StringToDecimal();
                        //    else
                        //        value2 = 0;
                        //    drv.THUC_THU_GOC_VAY = value1 + value2;
                        //    cell1 = worksheet.Cells[cell.Row, 16].Value;
                        //    if (!cell1.IsNullOrEmpty() && cell1.ToString().IsNumeric())
                        //        value1 = cell1.ToString().StringToDecimal();
                        //    else
                        //        value1 = 0;
                        //    cell2 = worksheet.Cells[cell.Row, 18].Value;
                        //    if (!cell2.IsNullOrEmpty() && cell2.ToString().IsNumeric())
                        //        value2 = cell2.ToString().StringToDecimal();
                        //    else
                        //        value2 = 0;
                        //    drv.THUC_THU_LAI_TRONG = value1 + value2;
                        //    raddgrGocLaiVayDS.SelectedItems.Add(drv);
                        //}
                        //else
                        //{
                        //    drv.THUC_THU_TKQD = 0;
                        //    drv.THUC_THU_NOP_VAO_TKKKH = 0;
                        //    drv.THUC_THU_GOC_VAY = 0;
                        //    drv.THUC_THU_LAI_TRONG = 0;
                        //}
                    }
                    raddgrGocLaiVayDS.CalculateAggregates();
                    LMessage.ShowMessage(LLanguage.SearchResourceByKey("M.TinDung.HoaDon.ucHDThuTienKyCT_01.DaCapNhat") + soKhachHang + LLanguage.SearchResourceByKey("M.TinDung.HoaDon.ucHDThuTienKyCT_01.DongDuLieu"), LMessage.MessageBoxType.Information);
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
            }
        }

        /// <summary>
        /// Sau khi thêm mới
        /// </summary>
        /// <param name="ret"></param>
        private void afterAddNew(ApplicationConstant.ResponseStatus ret, Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY obj, List<ClientResponseDetail> listResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(listResponseDetail);
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                SetEnabledAllControls(false);
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(obj.TRANG_THAI_NGHIEP_VU);
                //txtTrangThai.Text = lblTrangThai.Content.ToString();
                txtNguoiLap.Text = obj.NGUOI_LAP;
                //teldtNgayNhap.Value = LDateTime.StringToDate(obj.NGAY_LAP, "yyyyMMdd");
                txtSoGiaoDich.Text = obj.MA_GIAO_DICH;
                tthaiNvu = obj.TRANG_THAI_NGHIEP_VU;
                _objKiemSoat = new KIEM_SOAT();
                _objKiemSoat.ID = obj.ID_GIAO_DICH;
                _objKiemSoat.SO_GIAO_DICH = obj.MA_GIAO_DICH;


                if (cbMultiAdd.IsChecked == true)
                {
                    SetEnabledAllControls(true);
                    ResetForm();
                }
                else if (!DatabaseConstant.CLOSE_DETAIL_FORM)
                {
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, _function);
                    tlbImport.IsEnabled = true;
                }
                else
                {
                    onClose();
                }
            }
            else
            {

            }
        }

        /// <summary>
        /// Sau khi sửa
        /// </summary>
        /// <param name="ret"></param>
        private void afterModify(ApplicationConstant.ResponseStatus ret, Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY obj, List<ClientResponseDetail> listResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(listResponseDetail);
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                tthaiNvu = obj.TRANG_THAI_NGHIEP_VU;
                SetEnabledAllControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, _function);
                tlbImport.IsEnabled = true;
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(obj.TRANG_THAI_NGHIEP_VU);
                //txtTrangThai.Text = lblTrangThai.Content.ToString();
                txtNguoiCapNhat.Text = obj.NGUOI_CAP_NHAT;
                teldtNgayCNhat.Value = LDateTime.StringToDate(obj.NGAY_CAP_NHAT, "yyyyMMdd");
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, _function);
                tlbImport.IsEnabled = true;
            }
            else
            {

            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(obj.ID_GIAO_DICH);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                _function,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Sau khi xóa
        /// </summary>
        /// <param name="ret"></param>
        private void afterDelete(ApplicationConstant.ResponseStatus ret, List<ClientResponseDetail> listResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(listResponseDetail);
            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_objKiemSoat.ID);
            List<int> listUnlockId = new List<int>();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                _function,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Đóng cửa sổ chi tiết sau khi xóa
            onClose();
        }

        /// <summary>
        /// Sau khi duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void afterApprove(ApplicationConstant.ResponseStatus ret, Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY obj, List<ClientResponseDetail> listResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(listResponseDetail);
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {

                tthaiNvu = obj.TRANG_THAI_NGHIEP_VU;
                SetEnabledAllControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, _function);
                tlbImport.IsEnabled = true;
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                //txtTrangThai.Text = lblTrangThai.Content.ToString();
                txtNguoiCapNhat.Text = obj.NGUOI_CAP_NHAT;
                teldtNgayCNhat.Value = LDateTime.StringToDate(obj.NGAY_CAP_NHAT, "yyyyMMdd");
            }
            else
            {

            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(obj.ID_GIAO_DICH);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                _function,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Sau khi thoái duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void afterCancel(ApplicationConstant.ResponseStatus ret, Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY obj, List<ClientResponseDetail> listResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(listResponseDetail);
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                tthaiNvu = obj.TRANG_THAI_NGHIEP_VU;
                SetEnabledAllControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, _function);
                tlbImport.IsEnabled = true;
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                //txtTrangThai.Text = lblTrangThai.Content.ToString();
                txtNguoiCapNhat.Text = obj.NGUOI_CAP_NHAT;
                teldtNgayCNhat.Value = LDateTime.StringToDate(obj.NGAY_CAP_NHAT, "yyyyMMdd");
            }
            else
            {

            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(obj.ID_GIAO_DICH);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                _function,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Sau khi từ chối
        /// </summary>
        /// <param name="ret"></param>
        private void afterRefuse(ApplicationConstant.ResponseStatus ret, Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY obj, List<ClientResponseDetail> listResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(listResponseDetail);
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {

                tthaiNvu = obj.TRANG_THAI_NGHIEP_VU;
                SetEnabledAllControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, _function);
                tlbImport.IsEnabled = true;
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                //txtTrangThai.Text = lblTrangThai.Content.ToString();
                txtNguoiCapNhat.Text = obj.NGUOI_CAP_NHAT;
                teldtNgayCNhat.Value = LDateTime.StringToDate(obj.NGAY_CAP_NHAT, "yyyyMMdd");
            }
            else
            {

            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(obj.ID_GIAO_DICH);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                _function,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        private void OnPreview()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                // Cảnh báo khi không có dữ liệu
                //if (LObject.IsNullOrEmpty(_objKiemSoat.ID) && LObject.IsNullOrEmpty(_objKiemSoat.SO_GIAO_DICH))
                //{
                //    LMessage.ShowMessage("M.TinDung.HoaDon.ucHDThuTienKyCT_01.KhongCoThongTinSo", LMessage.MessageBoxType.Warning);
                //}
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
                    objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.KT_PHIEU_KE_TOAN;

                    GDKT_GIAO_DICH objGDKT_GIAO_DICH = new GDKT_GIAO_DICH();
                    objGDKT_GIAO_DICH.MaGiaoDich = _objKiemSoat.SO_GIAO_DICH;

                    doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                    doiTuongBaoCao.objGDKT_GIAO_DICH = objGDKT_GIAO_DICH;
                    xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void OnPreviewKeHoach()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                // Cảnh báo khi không có dữ liệu
                if (!_objKiemSoat.IsNullOrEmpty() && LObject.IsNullOrEmpty(_objKiemSoat.ID) && LObject.IsNullOrEmpty(_objKiemSoat.SO_GIAO_DICH))
                {
                    LMessage.ShowMessage("M.TinDung.HoaDon.ucHDThuTienKyCT_01.KhongCoThongTinSo", LMessage.MessageBoxType.Warning);
                }
                else
                {
                    BaoCaoProcess process = new BaoCaoProcess();
                    DanhMucProcess dmProcess = new DanhMucProcess();

                    HT_BAOCAO htBaoCao = new HT_BAOCAO();
                    List<HT_BAOCAO_TSO> lstHtBaoCaoTso = new List<HT_BAOCAO_TSO>();
                    int idBaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.TDVM_HOA_DON_THU_TIEN_KY.layIdBaoCao();
                    string maBaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.TDVM_HOA_DON_THU_TIEN_KY.layMaBaoCao();

                    DM_DON_VI phongGD = dmProcess.getDonViByMaCum(txtCum.Text);
                    string maPhong = LObject.IsNullOrEmpty(phongGD) ? "" : phongGD.MA_DVI;
                    string tenPhong = LObject.IsNullOrEmpty(phongGD) ? "" : phongGD.TEN_GDICH;

                    List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
                    listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenNguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenChiNhanh", ClientInformation.TenDonVi, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri())); // fake
                    listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenPGD", tenPhong, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenCum", lblTenCum.Content.ToString(), ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayThangNam", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri())); // fake
                    listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGiaoDich", maPhong, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("@MaCum", txtCum.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", Convert.ToDateTime(teldtNgayThuTien.Value).ToString("yyyyMMdd"), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

                    listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                    // Lấy thông tin báo cáo và tham số                    
                    process.LayThongTinBaoCao(idBaoCao, maBaoCao, ref htBaoCao, ref lstHtBaoCaoTso);
                    // Chuẩn bị điều kiện cho báo cáo
                    if (listThamSoBaoCao != null && listThamSoBaoCao.Count > 0)
                    {
                        foreach (HT_BAOCAO_TSO htBaoCaoTso in lstHtBaoCaoTso)
                        {
                            foreach (ThamSoBaoCao thamSoBaoCao in listThamSoBaoCao)
                            {
                                if (htBaoCaoTso.MA_TSO.Equals(thamSoBaoCao.MaThamSo) &&
                                    htBaoCaoTso.LOAI_TSO.Equals(thamSoBaoCao.LoaiThamSo))
                                {
                                    htBaoCaoTso.GTRI_TSO = thamSoBaoCao.GiaTriThamSo;
                                    break;
                                }
                            }
                        }
                    }

                    ApplicationConstant.ResponseStatus retStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    FileBase fileResponse = new FileBase();
                    string responseMessage = null;
                    retStatus = process.LayDuLieu(htBaoCao, lstHtBaoCaoTso, ref fileResponse, ref responseMessage);

                    if (retStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                    {
                        string fileReport = ClientInformation.TempDir + "\\" + fileResponse.FileName + "." + fileResponse.FileFormat; ;
                        LFile.WriteFileFromByteArray(fileResponse.FileData, fileReport);

                        // show file
                        Stream stream = LFile.ConvertByteArrayToStream(fileResponse.FileData);
                        System.Diagnostics.Process.Start(fileReport);
                    }
                    else
                    {
                        LMessage.ShowMessage(responseMessage, LMessage.MessageBoxType.Error);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
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

                bool ret = process.UnlockData(DatabaseConstant.Module.TDVM,
                    _function,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.SUA,
                    listLockId);
            }
        }

        /// <summary>
        /// Kiểm tra dữ liệu trước khi lưu
        /// </summary>
        /// <returns></returns>
        private bool Validation()
        {
            try
            {
                if (teldtNgayLap.Value == null)
                {
                    LMessage.ShowMessage("M.TinDung.HoaDon.ucHDThuTienKyCT_01.NgayLapKhongDeTrong", LMessage.MessageBoxType.Warning);
                    if (grbThongTinChung.IsExpanded == false)
                    {
                        grbThongTinChung.IsExpanded = true;
                    }
                    teldtNgayLap.Focus();
                    return false;
                }
                else if (teldtNgayThuTien.Value == null)
                {
                    LMessage.ShowMessage("M.TinDung.HoaDon.ucHDThuTienKyCT_01.NgayThuTienKhongDeTrong", LMessage.MessageBoxType.Warning);
                    if (grbThongTinChung.IsExpanded == false)
                    {
                        grbThongTinChung.IsExpanded = true;
                    }
                    teldtNgayThuTien.Focus();
                    return false;
                }
                else if (LString.IsNullOrEmptyOrSpace(txtDienGiai.Text))
                {
                    LMessage.ShowMessage("M.TinDung.HoaDon.ucHDThuTienKyCT_01.DienGiaiKhongDeTrong", LMessage.MessageBoxType.Warning);
                    if (grbThongTinChung.IsExpanded == false)
                    {
                        grbThongTinChung.IsExpanded = true;
                    }
                    txtDienGiai.Focus();
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return true;
        }

        private void SetFormData(DataTable dt)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            HoaDonTienKyProcess process = new HoaDonTienKyProcess();
            AutoComboBox au = new AutoComboBox();
            try
            {
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void ResetForm()
        {
            //lblTrangThai.Content = "";
            txtTrangThai.Text = "";
            txtNguoiLap.Text = "";
            txtNguoiCapNhat.Text = "";
        }

        private Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY GetFormData(string tthai)
        {
            Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY obj = new Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY();
            if (_objKiemSoat != null)
            {
                obj.ID_GIAO_DICH = _objKiemSoat.ID;
                obj.MA_GIAO_DICH = _objKiemSoat.SO_GIAO_DICH;
                obj.NGUOI_LAP = txtNguoiLap.Text;
                obj.NGAY_LAP = Convert.ToDateTime(teldtNgayLap.Value).ToString("yyyyMMdd");
                obj.NGUOI_CAP_NHAT = Presentation.Process.Common.ClientInformation.TenDangNhap;
                obj.NGAY_CAP_NHAT = Presentation.Process.Common.ClientInformation.NgayLamViecHienTai;
            }
            else
            {
                obj.NGUOI_LAP = Presentation.Process.Common.ClientInformation.TenDangNhap;
                obj.NGAY_LAP = Presentation.Process.Common.ClientInformation.NgayLamViecHienTai;
            }

            string ngayThuTien = "";
            foreach (AutoCompleteCheckBox autoNgayThu in lstSourceKyThu)
            {
                if (autoNgayThu.CheckedMember && !autoNgayThu.ValueMember[0].Equals("All"))
                {
                    ngayThuTien += autoNgayThu.ValueMember[2] + "#";
                }
            }

            obj.LOAI_TIEN = ClientInformation.MaDongNoiTe;
            obj.DIEN_GIAI = txtDienGiai.Text;
            obj.LY_DO = "";
            obj.MA_DVI = Presentation.Process.Common.ClientInformation.MaDonViGiaoDich;
            obj.NGAY_LAP = Convert.ToDateTime(teldtNgayLap.Value).ToString("yyyyMMdd");
            obj.NGAY_GIAO_DICH = ClientInformation.NgayLamViecHienTai;
            obj.NGAY_LAP_HOA_DON = ClientInformation.NgayLamViecHienTai;
            obj.NGAY_THU_TIEN_KY = ngayThuTien.Substring(0, ngayThuTien.Length - 1);
            obj.SO_CUM = txtCum.Text;
            obj.TEN_CUM = lblTenCum.Content.ToString();
            obj.TRANG_THAI_BAN_GHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
            obj.TRANG_THAI_NGHIEP_VU = tthai;
            obj.NGUOI_GD = cmbCanBoThuTien.Text;
            obj.DIA_CHI = "";
            TinhTongThucThu();
            obj.DSACH_KHE_UOC = _lstKheUoc.ToArray();
            return obj;
        }

        private void TinhTongThucThu()
        {
            foreach (DANH_SACH_KHE_UOC_VONG_VAY obj in _lstKheUoc)
            {
                obj.TONG_SO_TIEN =
                obj.THUC_THU_TKQD + obj.THUC_THU_GOC_VAY + obj.THUC_THU_LAI_QUA_HAN + obj.THUC_THU_LAI_TRONG + obj.THUC_THU_NOP_VAO_TKKKH + obj.LAI_PHAN_BO_KY_NAY;
                obj.THUC_THU_TONG = obj.THUC_THU_TKQD + obj.THUC_THU_GOC_VAY + obj.THUC_THU_LAI_QUA_HAN + obj.THUC_THU_LAI_TRONG + obj.THUC_THU_NOP_VAO_TKKKH + obj.THUC_THU_QUY_TT + obj.PHI_TRA_TRUOC;
                
                if (obj.THUC_THU_NOP_VAO_TKKKH > 0)
                {
                    obj.NOP_TIEN_VAO_TKKKH = "CO";
                }

                //if (obj.THUC_THU_TKKKH == 0)
                //{
                //    if (obj.DSACH_SO_NOP_TIEN != null)
                //    {
                //        obj.DSACH_SO_NOP_TIEN = null;
                //    }
                //}

                //if (obj.NOP_TIEN_TU_TKKKH == "KHONG")
                //{
                //    if (obj.DSACH_SO_RUT_TIEN != null)
                //    {
                //        obj.DSACH_SO_RUT_TIEN = null;
                //    }
                //}
                if (!obj.DSACH_SO_RUT_TIEN.IsNullOrEmpty())
                    obj.THUC_NOP_TU_TKKKH = obj.DSACH_SO_RUT_TIEN.Sum(f => f.SO_TIEN_RUT_RA);
                obj.THUC_THU_TIEN_MAT = obj.THUC_THU_TONG - obj.THUC_NOP_TU_TKKKH;
            }
        }

        private void TinhPhiTraTruoc(ref DANH_SACH_KHE_UOC_VONG_VAY objKheUoc)
        {
            HoaDonTienKyProcess process = new HoaDonTienKyProcess();
            try
            {
                Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY obj = new Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                // Dữ liệu truyền vào và dữ liệu trả về

                obj.NGAY_GIAO_DICH = ClientInformation.NgayLamViecHienTai;
                obj.NGAY_LAP_HOA_DON = ClientInformation.NgayLamViecHienTai;
                obj.NGAY_THU_TIEN_KY = Convert.ToDateTime(teldtNgayThuTien.Value).ToString("yyyyMMdd");
                obj.DSACH_KHE_UOC = new DANH_SACH_KHE_UOC_VONG_VAY[1] { objKheUoc };
                Mouse.OverrideCursor = Cursors.Wait;
                // Lấy dữ liệu từ form
                ret = process.ThuGocLaiVay(_function, DatabaseConstant.Action.TINH_TOAN, ref obj, ref listResponseDetail);
                if (ret > 0)
                    objKheUoc = obj.DSACH_KHE_UOC[0];
                else
                {
                    objKheUoc.THUC_THU_GOC_VAY = objKheUoc.KE_HOACH_GOC_VAY;
                    objKheUoc.TAT_TOAN = "false";
                    objKheUoc.PHI_TRA_TRUOC = 0;
                }
                Mouse.OverrideCursor = Cursors.Arrow;
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

        private void TinhPhiTraTruocBIDV(ref DANH_SACH_KHE_UOC_VONG_VAY objKheUoc)
        {
            try
            {
                string ngayThuTien = "";
                foreach (AutoCompleteCheckBox autoNgayThu in lstSourceKyThu)
                {
                    if (autoNgayThu.CheckedMember && !autoNgayThu.ValueMember[0].Equals("All"))
                    {
                        ngayThuTien += autoNgayThu.ValueMember[2] + "#";
                    }
                }
                ngayThuTien = ngayThuTien.Substring(0, ngayThuTien.Length - 1);
                ngayThuTien = ngayThuTien.Split('#').LastOrDefault();
                string ngayDaoHan = objKheUoc.DSACH_THONG_TIN_THU_NO.LastOrDefault(f => f.GOC_KH > 0).NGAY_KH;
                decimal soDu = objKheUoc.DU_NO;
                decimal soTienPhi = 0;
                int soNgayTraTruoc = LDateTime.StringToDate(ngayDaoHan, ApplicationConstant.defaultDateTimeFormat).CountDayBetweenDates(LDateTime.StringToDate(ngayThuTien, ApplicationConstant.defaultDateTimeFormat));
                decimal tyLe = 0;
                decimal soTien = 0;
                decimal soTienTThieu = 0;
                decimal soTienTDa = 0;
                if (objKheUoc.BIEU_PHI.IsNullOrEmpty())
                    return;
                List<BIEU_PHI_CTIET_DTO> lstBieuPhi = objKheUoc.BIEU_PHI.DSACH_BPHI_CT.ToList();
                if (!lstBieuPhi.IsNullOrEmpty())
                {
                    soTienTThieu = lstBieuPhi.FirstOrDefault().SO_TIEN_TTHIEU;
                    soTienTDa = lstBieuPhi.FirstOrDefault().SO_TIEN_TDA;
                }
                if (objKheUoc.BIEU_PHI.TCHAT_BPHI.Equals(BusinessConstant.TCHAT_BPHI.DTH.layGiaTri()))
                {
                    if (objKheUoc.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.TY_LE.layGiaTri()))
                    {
                        tyLe = lstBieuPhi.FirstOrDefault().TY_LE_PHI;
                    }
                    else if (objKheUoc.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.SO_TIEN.layGiaTri()))
                    {
                        soTien = lstBieuPhi.FirstOrDefault().SO_TIEN_PHI;
                    }
                    else if (objKheUoc.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.STIEN_TLE.layGiaTri()))
                    {
                        tyLe = lstBieuPhi.FirstOrDefault().TY_LE_PHI;
                        soTien = lstBieuPhi.FirstOrDefault().SO_TIEN_PHI;
                    }
                }
                else
                {
                }
                if (objKheUoc.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.TY_LE.layGiaTri()))
                {
                    soTienPhi = objKheUoc.DU_NO * soNgayTraTruoc * (tyLe / 360 / 100);
                }
                else if (objKheUoc.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.SO_TIEN.layGiaTri()))
                {
                    soTienPhi = soTien;
                }
                else if (objKheUoc.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.STIEN_TLE.layGiaTri()))
                {

                }
                if (soTienPhi < soTienTThieu)
                    soTienPhi = soTienTThieu;
                if (soTienPhi > soTienTDa)
                    soTienPhi = soTienTDa;
                soTienPhi = soTienPhi.Rounding(0);
                objKheUoc.PHI_TRA_TRUOC = soTienPhi;
                objKheUoc.MA_PHI_TRA_TRUOC = objKheUoc.BIEU_PHI.MA_BPHI;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiKhongXacDinh", LMessage.MessageBoxType.Error);
            }
        }
        private void ConvertDataSetToObject(ref List<DANH_SACH_KHE_UOC_VONG_VAY> lst, DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                try
                {
                    
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["MA_KHACH_HANG"] != DBNull.Value && !dr["MA_KHACH_HANG"].ToString().IsNullOrEmptyOrSpace())
                        {
                            DANH_SACH_KHE_UOC_VONG_VAY obj = new DANH_SACH_KHE_UOC_VONG_VAY();
                            List<THONG_TIN_THU_NO> lstTTinThuNo = new List<THONG_TIN_THU_NO>();
                            List<DANH_SACH_SO> lstDSSoGuiTien = new List<DANH_SACH_SO>();
                            List<DANH_SACH_SO> lstDSSoRutTien = new List<DANH_SACH_SO>();
                            if (dr["ID_KHE_UOC"] != DBNull.Value)
                            {
                                obj = lst.First(e => e.ID_KHACH_HANG == Convert.ToInt32(dr["ID_KHACH_HANG"]) && e.ID_KHE_UOC == Convert.ToInt32(dr["ID_KHE_UOC"]));
                            }
                            else
                            {
                                obj = lst.First(e => e.ID_KHACH_HANG == Convert.ToInt32(dr["ID_KHACH_HANG"]));
                            }
                            if (obj != null)
                            {
                                if (dr["ID_KHE_UOC"] != DBNull.Value)
                                {
                                    obj.ID_KHE_UOC = Convert.ToInt32(dr["ID_KHE_UOC"]);
                                }

                                if (dr["MA_KHE_UOC"] != DBNull.Value)
                                {
                                    obj.MA_KHE_UOC = dr["MA_KHE_UOC"].ToString();
                                }

                                if (dr["KY_THU"] != DBNull.Value)
                                {
                                    obj.KY_THU = Convert.ToInt32(dr["KY_THU"]);
                                }

                                if (dr["SO_SO_TG"] != DBNull.Value)
                                {
                                    obj.SO_SO_TG = dr["SO_SO_TG"].ToString();
                                }

                                if (dr["TEN_NHOM"] != DBNull.Value)
                                {
                                    obj.TEN_NHOM = dr["TEN_NHOM"].ToString();
                                }

                                if (dr["NHOM_NO_HIEN_TAI"] != DBNull.Value)
                                {
                                    obj.NHOM_NO_HIEN_TAI = dr["NHOM_NO_HIEN_TAI"].ToString();
                                }

                                if (dr["LAI_SUAT"] != DBNull.Value)
                                {
                                    obj.LAI_SUAT = Convert.ToDecimal(dr["LAI_SUAT"]);
                                }
                                if (dr["KE_HOACH_TKQD"] != DBNull.Value)
                                {
                                    obj.KE_HOACH_TKQD = Convert.ToDecimal(dr["KE_HOACH_TKQD"]);
                                }

                                if (dr["LAI_PHAT"] != DBNull.Value)
                                {
                                    obj.LAI_PHAT = Convert.ToDecimal(dr["LAI_PHAT"]);
                                }

                                if (dr["KE_HOACH_GOC_VAY"] != DBNull.Value)
                                {
                                    obj.KE_HOACH_GOC_VAY = Convert.ToDecimal(dr["KE_HOACH_GOC_VAY"]);
                                }
                                if (dr["KE_HOACH_LAI_TRONG_HAN"] != DBNull.Value)
                                {
                                    obj.KE_HOACH_LAI_TRONG_HAN = Convert.ToDecimal(dr["KE_HOACH_LAI_TRONG_HAN"]);
                                }
                                if (dr["KE_HOACH_LAI_QUA_HAN"] != DBNull.Value)
                                {
                                    obj.KE_HOACH_LAI_QUA_HAN = Convert.ToDecimal(dr["KE_HOACH_LAI_QUA_HAN"]);
                                }

                                if (dr["THUC_THU_TKQD"] != DBNull.Value)
                                {
                                    obj.THUC_THU_TKQD = Convert.ToDecimal(dr["THUC_THU_TKQD"]);
                                }
                                if (dr["THUC_THU_GOC_VAY"] != DBNull.Value)
                                {
                                    obj.THUC_THU_GOC_VAY = Convert.ToDecimal(dr["THUC_THU_GOC_VAY"]);
                                }
                                if (dr["THUC_THU_LAI_TRONG"] != DBNull.Value)
                                {
                                    obj.THUC_THU_LAI_TRONG = Convert.ToDecimal(dr["THUC_THU_LAI_TRONG"]);
                                }
                                if (dr["THUC_THU_LAI_QUA_HAN"] != DBNull.Value)
                                {
                                    obj.THUC_THU_LAI_QUA_HAN = Convert.ToDecimal(dr["THUC_THU_LAI_QUA_HAN"]);
                                }

                                if (dr["TAI_KHOAN_TKQD"] != DBNull.Value)
                                {
                                    obj.TAI_KHOAN_TKQD = dr["TAI_KHOAN_TKQD"].ToString();
                                }
                                if (dr["TAI_KHOAN_TD"] != DBNull.Value)
                                {
                                    obj.TAI_KHOAN_TD = dr["TAI_KHOAN_TD"].ToString();
                                }
                                if (dr["MA_SAN_PHAM_TKQD"] != DBNull.Value)
                                {
                                    obj.MA_SAN_PHAM_TKQD = dr["MA_SAN_PHAM_TKQD"].ToString();
                                }
                                if (dr["MA_SAN_PHAM_TD"] != DBNull.Value)
                                {
                                    obj.MA_SAN_PHAM_TD = dr["MA_SAN_PHAM_TD"].ToString();
                                }
                                if (dr["SO_SO_TG_KKH"] != DBNull.Value)
                                {
                                    obj.SO_SO_TG_KKH = dr["SO_SO_TG_KKH"].ToString();
                                }

                                if (dr["NV_LOAI_NVON"] != DBNull.Value)
                                {
                                    obj.NV_LOAI_NVON = dr["NV_LOAI_NVON"].ToString();
                                }

                                if (dr["CHU_KY"] != DBNull.Value)
                                {
                                    obj.CHU_KY = Convert.ToInt32(dr["CHU_KY"]);
                                }

                                if (dr["SO_TIEN_DU_THU"] != DBNull.Value)
                                {
                                    obj.SO_TIEN_DU_THU = Convert.ToDecimal(dr["SO_TIEN_DU_THU"]);
                                }

                                if (dr.Table.Columns.Contains("THUC_THU_QUY_TT") && dr["THUC_THU_QUY_TT"] != DBNull.Value)
                                {
                                    obj.THUC_THU_QUY_TT = Convert.ToDecimal(dr["THUC_THU_QUY_TT"]);
                                }
                                if (dr.Table.Columns.Contains("LOAI_TIEN") && dr["LOAI_TIEN"] != DBNull.Value)
                                {
                                    obj.LOAI_TIEN = Convert.ToString(dr["LOAI_TIEN"]);
                                }
                                if (dr.Table.Columns.Contains("ID_NHOM") && dr["ID_NHOM"] != DBNull.Value)
                                {
                                    obj.ID_NHOM = Convert.ToInt32(dr["ID_NHOM"]);
                                }
                                if (dr.Table.Columns.Contains("DU_NO") && dr["DU_NO"] != DBNull.Value)
                                {
                                    obj.DU_NO = Convert.ToInt32(dr["DU_NO"]);
                                }
                                obj.NGAY_GD = ClientInformation.NgayLamViecHienTai;
                                obj.NOP_TIEN_TU_TKKKH = "KHONG";
                                obj.NOP_TIEN_VAO_TKKKH = "KHONG";
                                obj.GHI_NHAN_VAO_TKNB = "KHONG";
                                obj.TRA_GOC_LAI_TRUOC_HAN = "KHONG";
                                obj.TAT_TOAN = "KHONG";
                                obj.MA_DON_VI = Presentation.Process.Common.ClientInformation.MaDonViGiaoDich;
                            }
                            if(!ds.Tables["KHOACH_VM"].IsNullOrEmpty())
                            {
                                DataRow[] arrdr = ds.Tables["KHOACH_VM"].Select("ID_KUOCVM=" + obj.ID_KHE_UOC.ToString());
                                THONG_TIN_THU_NO objTTinThuNo = null;
                                decimal thucThuGoc = obj.THUC_THU_GOC_VAY;
                                decimal thucThuLai = obj.THUC_THU_LAI_TRONG;
                                foreach (DataRow dtr in arrdr)
                                {
                                    objTTinThuNo = new THONG_TIN_THU_NO();
                                    objTTinThuNo.ID_KUOC = Convert.ToInt32(dtr["ID_KUOCVM"]);
                                    objTTinThuNo.MA_KUOC = Convert.ToString(dtr["MA_KUOCVM"]);
                                    objTTinThuNo.SO_NGAY_QH = Convert.ToInt32(dtr["SO_NGAY_QH"]);
                                    objTTinThuNo.NGAY_KH = Convert.ToString(dtr["NGAY_KH"]);
                                    objTTinThuNo.SO_NGAY_QH = Convert.ToInt32(dtr["SO_NGAY_QH"]);
                                    objTTinThuNo.NHOM_NO = Convert.ToString(dtr["NHOM_NO"]);
                                    objTTinThuNo.GOC_KH = Convert.ToDecimal(dtr["KH_TRA_GOC"]);
                                    objTTinThuNo.LAI_KH = Convert.ToDecimal(dtr["KH_TRA_LAI"]);
                                    objTTinThuNo.LAI_PHAT_KH = Convert.ToDecimal(dtr["KH_TRA_LAI_PHAT"]);
                                    objTTinThuNo.GOC_TT = Math.Min(thucThuGoc,objTTinThuNo.GOC_KH);
                                    thucThuGoc -= objTTinThuNo.GOC_TT;
                                    objTTinThuNo.LAI_TT = Math.Min(thucThuLai, objTTinThuNo.LAI_KH);
                                    thucThuLai -= objTTinThuNo.LAI_TT;
                                    objTTinThuNo.LAI_PHAT_TT = 0;
                                    lstTTinThuNo.Add(objTTinThuNo);
                                }
                                obj.DSACH_THONG_TIN_THU_NO = lstTTinThuNo.ToArray();
                            }
                            if (!ds.Tables["TGUI_KHANG"].IsNullOrEmpty())
                            {
                                DataRow[] arrdr = ds.Tables["TGUI_KHANG"].Select("ID_NHOM='" + obj.ID_NHOM.ToString() + "'");
                                DANH_SACH_SO objDSachSo = null;
                                foreach (DataRow dtr in arrdr)
                                {
                                    if(Convert.ToInt32(dtr["ID_KHANG"])==obj.ID_KHACH_HANG)
                                    {
                                        objDSachSo = new DANH_SACH_SO();
                                        objDSachSo.ID_NHOM = Convert.ToInt32(dtr["ID_NHOM"]);
                                        objDSachSo.LOAI_SAN_PHAM_HDV = Convert.ToString(dtr["LOAI_SAN_PHAM_HDV"]);
                                        objDSachSo.MA_KHANG = Convert.ToString(dtr["MA_KHANG"]);
                                        objDSachSo.SO_DU = Convert.ToDecimal(dtr["SO_DU"]);
                                        objDSachSo.SO_SO = Convert.ToString(dtr["SO_SO"]);
                                        objDSachSo.SO_TAI_KHOAN = Convert.ToString(dtr["SO_TAI_KHOAN"]);
                                        objDSachSo.SO_TIEN_NOP_VAO = Convert.ToDecimal(dtr["SO_TIEN_NOP_VAO"]);
                                        objDSachSo.SO_TIEN_RUT_RA = Convert.ToDecimal(dtr["SO_TIEN_RUT_RA"]);
                                        objDSachSo.TEN_KHANG = Convert.ToString(dtr["TEN_KHANG"]);
                                        lstDSSoGuiTien.Add(objDSachSo);
                                    }
                                    objDSachSo = new DANH_SACH_SO();
                                    objDSachSo.ID_NHOM = Convert.ToInt32(dtr["ID_NHOM"]);
                                    objDSachSo.LOAI_SAN_PHAM_HDV = Convert.ToString(dtr["LOAI_SAN_PHAM_HDV"]);
                                    objDSachSo.MA_KHANG = Convert.ToString(dtr["MA_KHANG"]);
                                    objDSachSo.SO_DU = Convert.ToDecimal(dtr["SO_DU"]);
                                    objDSachSo.SO_SO = Convert.ToString(dtr["SO_SO"]);
                                    objDSachSo.SO_TAI_KHOAN = Convert.ToString(dtr["SO_TAI_KHOAN"]);
                                    objDSachSo.SO_TIEN_NOP_VAO = Convert.ToDecimal(dtr["SO_TIEN_NOP_VAO"]);
                                    objDSachSo.SO_TIEN_RUT_RA = Convert.ToDecimal(dtr["SO_TIEN_RUT_RA"]);
                                    objDSachSo.TEN_KHANG = Convert.ToString(dtr["TEN_KHANG"]);
                                    lstDSSoRutTien.Add(objDSachSo);
                                }
                                obj.DSACH_THONG_TIN_THU_NO = lstTTinThuNo.ToArray();
                                obj.DSACH_SO_NOP_TIEN = lstDSSoGuiTien.ToArray();
                                obj.DSACH_SO_RUT_TIEN = lstDSSoRutTien.ToArray();
                            }
                            DataTable dt = ds.Tables["BIEU_PHI"];
                            if (dt.Rows.Count > 0)
                            {
                                DataRow drd = dt.Rows[0];
                                obj.BIEU_PHI = new BIEU_PHI_DTO();
                                obj.BIEU_PHI.HTHUC_BTHANG = drd["HTHUC_BTHANG"].ToString();
                                obj.BIEU_PHI.ID_BPHI = Convert.ToInt32(drd["ID"]);
                                obj.BIEU_PHI.LOAI_BPHI = drd["LOAI_BPHI"].ToString();
                                obj.BIEU_PHI.LOAI_TIEN = drd["MA_LOAI_TIEN"].ToString();
                                obj.BIEU_PHI.MA_BPHI = drd["MA_BPHI"].ToString();
                                obj.BIEU_PHI.NGAY_ADUNG = drd["NGAY_ADUNG"].ToString();
                                if (drd["NGAY_HHAN"] != DBNull.Value)
                                    obj.BIEU_PHI.NGAY_HHAN = drd["NGAY_HHAN"].ToString();
                                obj.BIEU_PHI.TCHAT_BPHI = drd["TCHAT_BPHI"].ToString();
                                obj.BIEU_PHI.TEN_BPHI = drd["TEN_BPHI"].ToString();
                                obj.BIEU_PHI.TY_LE_VAT = Convert.ToDecimal(drd["TY_LE_VAT"]);
                                obj.MA_PHI_TRA_TRUOC = drd["MA_BPHI"].ToString();
                            }
                            else
                                obj.BIEU_PHI = new BIEU_PHI_DTO();
                            dt = ds.Tables["BIEU_PHI_CT"];
                            List<BIEU_PHI_CTIET_DTO> lstBieuPhi = new List<BIEU_PHI_CTIET_DTO>();
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow drd in dt.Rows)
                                {
                                    BIEU_PHI_CTIET_DTO objBieuPhiCT = new BIEU_PHI_CTIET_DTO();
                                    objBieuPhiCT.ID_BPHI = Convert.ToInt32(drd["ID_BPHI"]);
                                    objBieuPhiCT.LOAI_BPHI = drd["LOAI_BPHI"].ToString();
                                    objBieuPhiCT.MA_BPHI = drd["MA_BPHI"].ToString();
                                    objBieuPhiCT.SO_TIEN_TINH_PHI = Convert.ToDecimal(drd["SO_TIEN"]);
                                    objBieuPhiCT.SO_TIEN_PHI = Convert.ToDecimal(drd["SO_TIEN_PHI"]);
                                    objBieuPhiCT.SO_TIEN_TDA = Convert.ToDecimal(drd["SO_TIEN_TDA"]);
                                    objBieuPhiCT.SO_TIEN_TTHIEU = Convert.ToDecimal(drd["SO_TIEN_TTHIEU"]);
                                    objBieuPhiCT.TY_LE_PHI = Convert.ToDecimal(drd["TY_LE_PHI"]);
                                    objBieuPhiCT.TY_LE_VAT = Convert.ToDecimal(drd["TY_LE_VAT"]);
                                    lstBieuPhi.Add(objBieuPhiCT);
                                }

                            }
                            else
                            {
                                lstBieuPhi = new List<BIEU_PHI_CTIET_DTO>();
                            }
                            obj.BIEU_PHI.DSACH_BPHI_CT = lstBieuPhi.ToArray();
                        }
                        else
                        {
                            DANH_SACH_KHE_UOC_VONG_VAY obj = lst.First(e => e.ID_KHACH_HANG == Convert.ToInt32(dr["ID_KHACH_HANG"]));
                            lst.Remove(obj);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Arrow;
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
            }
        }

        private void ChiTietThucThu()
        {
            if (raddgrGocLaiVayDS.SelectedItem != null)
            {
                Window window = new Window();
                try
                {
                    DANH_SACH_KHE_UOC_VONG_VAY item = (DANH_SACH_KHE_UOC_VONG_VAY)raddgrGocLaiVayDS.SelectedItem;
                    int index = -1;
                    index = _lstKheUoc.IndexOf(item);
                    item.TONG_SO_TIEN =
                item.THUC_THU_TKQD + item.THUC_THU_GOC_VAY + item.THUC_THU_LAI_QUA_HAN + item.THUC_THU_LAI_TRONG + item.THUC_THU_NOP_VAO_TKKKH + item.LAI_PHAN_BO_KY_NAY;
                    item.THUC_THU_TONG = item.THUC_THU_TKQD + item.THUC_THU_GOC_VAY + item.THUC_THU_LAI_QUA_HAN + item.THUC_THU_LAI_TRONG + item.THUC_THU_NOP_VAO_TKKKH + item.THUC_THU_QUY_TT + item.PHI_TRA_TRUOC;
                    item.THUC_THU_TIEN_MAT = item.THUC_THU_TONG - item.THUC_NOP_TU_TKKKH;
                    item.KE_HOACH_TONG = item.KE_HOACH_GOC_VAY + item.KE_HOACH_LAI_QUA_HAN + item.KE_HOACH_LAI_TRONG_HAN + item.KE_HOACH_TKKKH + item.KE_HOACH_TKQD;
                    string ngayThuKy = lstSourceKyThu.LastOrDefault().ValueMember[2];
                    ApplicationConstant.DonViSuDung donViSuDung = ApplicationConstant.layDonViSuDung(ClientInformation.Company);
                    
                    switch (donViSuDung)
                    {
                        case ApplicationConstant.DonViSuDung.QUANGBINH:
                            PopupNghiepVu.ucPopupThucThuQuangBinh uc1 = null;
                            if (_objKiemSoat.IsNullOrEmpty())
                                uc1 = new PopupNghiepVu.ucPopupThucThuQuangBinh(item, ngayThuKy, "", 0);
                            else
                                uc1 = new PopupNghiepVu.ucPopupThucThuQuangBinh(item, ngayThuKy, _objKiemSoat.SO_GIAO_DICH, _objKiemSoat.ID);
                            window.Content = uc1;
                            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            window.Title = LLanguage.SearchResourceByKey("U.TinDung.HoaDon.ucHDThuTienKyCT_01_01.ThongTinChiTietKhachHang") + item.TEN_KHACH_HANG;
                            //window.Width = 1024;
                            //window.Height = 700;
                            window.WindowState = WindowState.Maximized;
                            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            window.ShowDialog();
                            _lstKheUoc[index] = uc1.objKheUocViMo;
                            
                            break;
                        case ApplicationConstant.DonViSuDung.BIDV:
                            PopupNghiepVu.ucPopupThuGocLaiCT ucPopupCT = new PopupNghiepVu.ucPopupThuGocLaiCT(item, ngayThuKy);
                            ucPopupCT.DuLieuTraVe = new PopupNghiepVu.ucPopupThuGocLaiCT.LayDuLieu(LayDuLieuTuPopup);
                            Window win = new Window();
                            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            win.WindowState = WindowState.Maximized;
                            string[] tenKhang = new string[1];
                            tenKhang[0] = item.TEN_KHACH_HANG;
                            win.Title = LLanguage.SearchResourceByKey("U.TinDung.HoaDon.ucHDThuTienKyCT_01.ThongTinChiTietKhachHang", tenKhang);
                            win.Content = ucPopupCT;
                            win.ShowDialog();
                            if (!objKheUoc.IsNullOrEmpty())
                                _lstKheUoc[index] = objKheUoc;
                            break;
                        case ApplicationConstant.DonViSuDung.BIDV_BLF:
                            PopupNghiepVu.ucPopupThuGocLaiCT ucPopupCT01 = new PopupNghiepVu.ucPopupThuGocLaiCT(item, ngayThuKy);
                            ucPopupCT01.DuLieuTraVe = new PopupNghiepVu.ucPopupThuGocLaiCT.LayDuLieu(LayDuLieuTuPopup);
                            Window win01 = new Window();
                            win01.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            win01.WindowState = WindowState.Maximized;
                            string[] tenKhang01 = new string[1];
                            tenKhang01[0] = item.TEN_KHACH_HANG;
                            win01.Title = LLanguage.SearchResourceByKey("U.TinDung.HoaDon.ucHDThuTienKyCT_01.ThongTinChiTietKhachHang", tenKhang01);
                            win01.Content = ucPopupCT01;
                            win01.ShowDialog();
                            if (!objKheUoc.IsNullOrEmpty())
                                _lstKheUoc[index] = objKheUoc;
                            break;
                        default:
                            PopupNghiepVu.ucPopupThucThu uc = null;
                            if (_objKiemSoat.IsNullOrEmpty())
                                uc = new PopupNghiepVu.ucPopupThucThu(item, ngayThuKy, "", 0);
                            else
                                uc = new PopupNghiepVu.ucPopupThucThu(item, ngayThuKy, _objKiemSoat.SO_GIAO_DICH, _objKiemSoat.ID);
                            window.Content = uc;
                            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            window.Title = LLanguage.SearchResourceByKey("U.TinDung.HoaDon.ucHDThuTienKyCT_01.ThongTinChiTietKhachHang") + item.TEN_KHACH_HANG;
                            //window.Width = 1024;
                            //window.Height = 700;
                            window.WindowState = WindowState.Maximized;
                            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            window.ShowDialog();
                            _lstKheUoc[index] = uc.objKheUocViMo;
                            break;
                    }
                    raddgrGocLaiVayDS.ItemsSource = _lstKheUoc;
                    raddgrGocLaiVayDS.Rebind();
                }
                catch (System.Exception ex)
                {
                    this.Cursor = Cursors.Arrow;
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                finally
                {
                    window = null;
                }
            }
        }
        #endregion

    }
}
