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
using Presentation.Process;
using Presentation.Process.UtilitiesServiceRef;
using Presentation.Process.Common;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.TinDungServiceRef;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;
using System.IO;
using Presentation.Process.DanhMucServiceRef;
using Telerik.Windows.Controls.GridView;
using System.Collections;
using System.Reflection;


namespace PresentationWPF.TinDung.HoaDon
{
    /// <summary>
    /// Interaction logic for ucHDThuTienKyCTBinhKhanh.xaml
    /// </summary>
    public partial class ucHDThuTienKyCTBinhKhanh : UserControl
    {
        /// <summary>
        /// Khai bao
        /// </summary>
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
        List<AutoCompleteEntry> lstSourceCum = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceSanPham = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhatVon = new List<AutoCompleteEntry>();
        ListCheckBoxCombo lstSourceKyThu = new ListCheckBoxCombo();

        private string tthaiNvu = "";

        DataTable dtHoaDon = new DataTable();

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        private TDVM_LAP_HOA_DON_TIEN_KY _objHoaDon = null;

        private List<DANH_SACH_KHE_UOC_VONG_VAY> _lstKheUoc = null;

        private KIEM_SOAT _objKiemSoat = null;

        private int idCum = 0;

        private DatabaseConstant.Function _function = DatabaseConstant.Function.TDVM_LAP_HOA_DON_TIEN_KY;

        public event EventHandler OnSavingCompleted;
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucHDThuTienKyCTBinhKhanh()
        {
            KhoiTaoChung();
            
            beforeAddNew();
        }

        public ucHDThuTienKyCTBinhKhanh(KIEM_SOAT obj)
        {
            KhoiTaoChung();
            _objKiemSoat = obj;
            tthaiNvu = _objKiemSoat.TTHAI_NVU;
            GetThongTinGiaoDich(_objKiemSoat.ID);
            beforeModifyFromList(_objKiemSoat.action);
            //LayThongTinKyThu();
        }

        private void KhoiTaoChung()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDung;component/HoaDon/BinhKhanh/ucHDThuTienKyCTBinhKhanh.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            ThemContextMenu();
            BindHotkey();
            ShowControl();
            teldtNgayLap.Value = LDateTime.StringToDate(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            teldtNgayThuTien.Value = LDateTime.StringToDate(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            this.cmbSanPham.SelectionChanged += new SelectionChangedEventHandler(cmbSanPham_SelectionChanged);
            this.cmbKyThu.DropDownClosed += new EventHandler(cmbKyThu_DropDownClosed);
            this.cmbNgayPhatVon.SelectionChanged += new SelectionChangedEventHandler(cmbNgayPhatVon_SelectionChanged);
            KhoiTaoBinding();
            KhoiTaoComboBox();
            this.cmbCum.SelectionChanged += new SelectionChangedEventHandler(cmbCum_SelectionChanged);
            //this.raddgrGocLaiVayDS.KeyboardCommandProvider = new CustomControl.CustomGridViewKeyboardCommand(this.raddgrGocLaiVayDS);
        }

        void cmbNgayPhatVon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboBoxKyThu();
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
                    AggregationExpression = ku => ku.Sum(r => r.KE_HOACH_TKQD + r.KE_HOACH_GOC_VAY + r.KE_HOACH_LAI_TRONG_HAN + r.KE_HOACH_LAI_QUA_HAN + r.KE_HOACH_TKKKH),
                    ResultFormatString = "{0:n0}",
                    Caption = "Tổng: "
                };
                GridViewExpressionColumn columnKH = this.raddgrGocLaiVayDS.Columns["TongKH"] as GridViewExpressionColumn;
                columnKH.AggregateFunctions.Add(tongKH);
                var tongTT = new AggregateFunction<DANH_SACH_KHE_UOC_VONG_VAY, decimal>
                {
                    AggregationExpression = ku => ku.Sum(r => r.THUC_THU_TKQD + r.THUC_THU_GOC_VAY + r.THUC_THU_LAI_TRONG + r.THUC_THU_LAI_QUA_HAN + r.THUC_THU_NOP_VAO_TKKKH + r.PHI_TRA_TRUOC),
                    ResultFormatString = "{0:n0}",
                    Caption = "Tổng: "
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

        private void ThemContextMenu()
        {
            var menuItem = new MenuItem
            {
                Header = btnDetail.Label,
                Name = "ctm" + "Detail",
                Icon = new Image { Source = btnDetail.SmallImageSource, Width = 16, Height = 16 }
            };
            menuItem.Click += new RoutedEventHandler(btnShortcutKey_Click);
            mnuMain.Items.Add(menuItem);
        }

        private void KhoiTaoComboBox()
        {
            AutoComboBox auto = new AutoComboBox();
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceCum, ref cmbCum, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM_ALL.getValue());
            lstSourceCum = lstSourceCum.Where(e => e.KeywordStrings.ElementAt(1).Equals(ClientInformation.MaDonViGiaoDich)).ToList();
            cmbCum.Items.Clear();
            auto.GenAutoComboBox(ref lstSourceCum, ref cmbCum, null);
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.MaDonVi);
            auto.GenAutoComboBox(ref lstSourceSanPham, ref cmbSanPham, "COMBOBOX_SAN_PHAM_TD",lstDieuKien);
        }

        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.TinDung.HoaDon.ucHDThuTienKyCTBinhKhanh", "RibbonButton");
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
                onNew();
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
                onNew();
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
        #endregion

        /// <summary>
        /// Xu ly giao dien
        /// </summary>
        #region Xu ly giao dien

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
                        cmbCum.SelectedIndex = lstSourceCum.IndexOf(lstSourceCum.FirstOrDefault(e=>e.KeywordStrings.First() == ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["MA_CUM"].ToString()));
                        cmbCum.Tag = ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["MA_DVI"].ToString();
                        cmbSanPham.SelectedIndex = lstSourceSanPham.IndexOf(lstSourceSanPham.FirstOrDefault(e => e.KeywordStrings.First() == ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["MA_SAN_PHAM"].ToString()));
                        teldtNgayLap.Value = LDateTime.StringToDate(ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["NGAY_LAP"].ToString(), "yyyyMMdd");
                        teldtNgayThuTien.Value = LDateTime.StringToDate(ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["NGAY_LAP"].ToString(), "yyyyMMdd").GetFirstDateOfMonth();
                        txtDienGiai.Text = ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["DIEN_GIAI"].ToString();
                        txtNguoiLap.Text = ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["NGUOI_NHAP"].ToString();
                        txtNguoiCapNhat.Text = ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["NGUOI_CNHAT"].ToString();
                        if (LayThongTinNgayGiaiNgan())
                        {
                            if(ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["NGAY_PHAT_VON"] != DBNull.Value && cmbNgayPhatVon.Items.Count > 0)
                                cmbNgayPhatVon.SelectedIndex = lstSourcePhatVon.IndexOf(lstSourcePhatVon.FirstOrDefault(f=>f.KeywordStrings.FirstOrDefault().Equals(ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["NGAY_PHAT_VON"].ToString())));
                        }
                        //teldtNgayNhap.Value = LDateTime.StringToDate(ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["NGAY_NHAP"].ToString(), "yyyyMMdd");
                        //lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(_objKiemSoat.TTHAI_NVU);
                        //txtTrangThai.Text = lblTrangThai.Content.ToString();
                        if (!LString.IsNullOrEmptyOrSpace(ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["NGAY_CNHAT"].ToString()))
                        {
                            teldtNgayCNhat.Value = LDateTime.StringToDate(ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["NGAY_CNHAT"].ToString(), "yyyyMMdd");
                        }
                        int iCount=0;
                        lstSourceKyThu = new ListCheckBoxCombo();
                        foreach(string str in ds.Tables["VKT_TDVM_HDTK_CHUNG"].Rows[0]["NGAY_THU_TIEN"].ToString().Split('#'))
                        {
                            if (str.Equals("All"))
                                lstSourceKyThu.Add(new AutoCompleteCheckBox("Tất cả", new string[2] { "%", "" }, true, iCount));
                            else
                                lstSourceKyThu.Add(new AutoCompleteCheckBox("Kỳ thu ngày: " + LDateTime.StringToDate(str, ApplicationConstant.defaultDateTimeFormat).ToString("dd/MM/yyyy"), new string[2] { str, "" }, true, iCount));
                            iCount++;
                        }
                        new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceKyThu, ref cmbKyThu, null);
                    }
                    LayThongTinGiaoDich(ds);
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

                    if (dr["KE_HOACH_TKQD"] != DBNull.Value)
                    {
                        obj.KE_HOACH_TKQD = Convert.ToDecimal(dr["KE_HOACH_TKQD"]);
                    }

                    if (dr["KE_HOACH_TKKKH"] != DBNull.Value)
                    {
                        obj.KE_HOACH_TKKKH = Convert.ToDecimal(dr["KE_HOACH_TKKKH"]);
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
                    if (dr["SO_SO_TG_KKH"] != DBNull.Value)
                    {
                        obj.SO_SO_TG_KKH = dr["SO_SO_TG_KKH"].ToString();
                    }
                    if (dr["NV_LOAI_NVON"] != DBNull.Value)
                    {
                        obj.NV_LOAI_NVON = dr["NV_LOAI_NVON"].ToString();
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
                                    objNop.SO_DU = 0;
                                    objNop.SO_SO = drNop["SO_SO_TK"].ToString();
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
                                    objNop.SO_DU = 0;
                                    objNop.SO_SO = drNop["SO_SO_TK"].ToString();
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
                                    objNop.SO_DU = 0;
                                    objNop.SO_SO = drNop["SO_SO_TK"].ToString();
                                    objNop.SO_TAI_KHOAN = "";
                                    objNop.SO_TIEN_RUT_RA = Convert.ToDecimal(drNop["SO_TIEN_RUT_RA"].ToString());
                                    lstDanhSachRut.Add(objNop);
                                }
                            }
                        }
                        else
                        {
                            if (drNop["MA_KHANG"].ToString().Equals(obj.MA_KHACH_HANG) && drNop["MA_KHE_UOC"].ToString().Equals(obj.MA_KHE_UOC))
                            {
                                if (drNop["LOAI"].ToString() == "RUT")
                                {
                                    DANH_SACH_SO objNop = new DANH_SACH_SO();
                                    objNop.SO_DU = 0;
                                    objNop.SO_SO = drNop["SO_SO_TK"].ToString();
                                    objNop.SO_TAI_KHOAN = "";
                                    objNop.SO_TIEN_RUT_RA = Convert.ToDecimal(drNop["SO_TIEN_RUT_RA"].ToString());
                                    lstDanhSachRut.Add(objNop);
                                }
                            }
                        }
                    }

                    obj.THUC_THU_NOP_VAO_TKKKH = lstDanhSachNop.Sum(e => e.SO_TIEN_NOP_VAO);
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

                    if (obj.NOP_TIEN_TU_TKKKH == "CO")
                    {
                        obj.THUC_THU_TKKKH = tongTienThu + obj.THUC_THU_TKQD;
                    }
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
                    throw new System.NullReferenceException(LLanguage.SearchResourceByKey("M.TinDung.HoaDon.BinhKhanh.ucHDThuTienKyCTBinhKhanh.KhongTimThayControl") + sbControl.ToString());
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void ClearForm()
        {
            txtSoGiaoDich.Text = "";
            teldtNgayThuTien.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            teldtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            //lblTrangThai.Content = "";
            txtNguoiLap.Text = "";
            txtNguoiCapNhat.Text = "";
            //teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            teldtNgayCNhat.Value = null;
            txtTrangThai.Text = tthaiNvu = "";
            raddgrGocLaiVayDS.ItemsSource = null;
            txtDienGiai.Text = "";
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (LString.IsNullOrEmptyOrSpace(cmbCum.Text))
            {
                LMessage.ShowMessage("M.TinDung.HoaDon.BinhKhanh.ucHDThuTienKyCTBinhKhanh.ChuaChonCum", LMessage.MessageBoxType.Warning);
                cmbCum.Focus();
                return;
            }

            if (cmbNgayPhatVon.SelectedItem == null)
            {
                LMessage.ShowMessage("Chưa chọn ngày phát vốn", LMessage.MessageBoxType.Warning);
                cmbNgayPhatVon.Focus();
                return;
            }

            if (cmbKyThu.SelectedItem == null)
            {
                LMessage.ShowMessage("M.TinDung.HoaDon.BinhKhanh.ucHDThuTienKyCTBinhKhanh.ChuaChonKyThu", LMessage.MessageBoxType.Warning);
                cmbKyThu.Focus();
                return;
            }            

            string ngayThuTien = "";
            foreach (AutoCompleteCheckBox autoNgayThu in lstSourceKyThu)
            {
                if (autoNgayThu.CheckedMember && !autoNgayThu.ValueMember[0].Equals("All"))
                    ngayThuTien += autoNgayThu.ValueMember[0] + "#";
            }

            if (ngayThuTien.IsNullOrEmptyOrSpace())
            {
                LMessage.ShowMessage("M.TinDung.HoaDon.BinhKhanh.ucHDThuTienKyCTBinhKhanh.ChuaChonKyThu", LMessage.MessageBoxType.Warning);
                cmbKyThu.Focus();
                return;
            }

            List<string> lstDieuKien = new List<string>();
            //@MA_DVI#@LST_IDKHANG#@MA_CUM#@NGAY_THU_TIEN#@NGAY_HOP_CUM
            AutoCompleteEntry auCum = lstSourceCum.ElementAt(cmbCum.SelectedIndex);
            AutoCompleteEntry auSanPham = lstSourceSanPham.ElementAt(cmbSanPham.SelectedIndex);
            AutoCompleteEntry auNgayPhatVon = lstSourcePhatVon.ElementAt(cmbNgayPhatVon.SelectedIndex);
            lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
            if (raddgrGocLaiVayDS.Items.Count > 0)
            {
                if (lstDieuKien.Count == 1)
                {
                    lstDieuKien.Add("");
                }
                lstDieuKien[1] = "(";
                foreach (DANH_SACH_KHE_UOC_VONG_VAY drv in raddgrGocLaiVayDS.Items)
                {
                    lstDieuKien[1] += drv.ID_KHACH_HANG.ToString() + ",";
                }
                lstDieuKien[1] = lstDieuKien[1].Substring(0, lstDieuKien[1].Length - 1) + ")";
            }
            else
            {
                lstDieuKien.Add("(0)");
            }
            lstDieuKien.Add(auCum.KeywordStrings[0]);
            lstDieuKien.Add(ngayThuTien.Substring(0, ngayThuTien.Length - 1));
            lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
            lstDieuKien.Add(auSanPham.KeywordStrings[0]);
            lstDieuKien.Add(auNgayPhatVon.KeywordStrings[0]);

            lstPopup.Clear();
            
            if (grbThongTinChung.IsExpanded == true)
            {
                grbThongTinChung.IsExpanded = false;
            }

            try
            {
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_KH_HOA_DON_TK_00.getValue(), lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(true, simplePopupResponse, true);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.Title = LLanguage.SearchResourceByKey("M.TinDung.HoaDon.BinhKhanh.ucHDThuTienKyCTBinhKhanh.DanhSachKhachHang");
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup.Count > 0)
                {
                    string listIDKhachHang = "";
                    List<DANH_SACH_KHE_UOC_VONG_VAY> lstKheUoc = new List<DANH_SACH_KHE_UOC_VONG_VAY>();
                    foreach (DataRow drv in lstPopup)
                    {
                        listIDKhachHang += drv["ID"] + ",";
                    }
                    listIDKhachHang = listIDKhachHang.Substring(0, listIDKhachHang.Length - 1);
                    TinDungProcess process = new TinDungProcess();
                    Mouse.OverrideCursor = Cursors.Wait;
                    
                    DataSet ds = process.getChiTietHoaDonThuTienTheoKhang(listIDKhachHang, ngayThuTien, auSanPham.KeywordStrings[0]);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        foreach (DataRow drv in ds.Tables[0].Rows)
                        {
                            DANH_SACH_KHE_UOC_VONG_VAY obj = new DANH_SACH_KHE_UOC_VONG_VAY();
                            obj.MA_KHACH_HANG = drv["MA_KHACH_HANG"].ToString();
                            obj.ID_KHACH_HANG = Convert.ToInt32(drv["ID_KHACH_HANG"]);
                            foreach (DataRow dr in lstPopup)
                            {
                                if (obj.ID_KHACH_HANG == Convert.ToInt32(dr[1]))
                                {
                                    obj.TEN_KHACH_HANG = dr[3].ToString();
                                }
                            }

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
                            _objHoaDon.DSACH_KHE_UOC = lstKheUoc.ToArray();
                            _objHoaDon.NGAY_THU_TIEN_KY = ngayThuTien;
                            _objHoaDon.LAY_KY_TRUOC = chkLayKyTruoc.IsChecked.GetValueOrDefault().ToString();
                            new TinDungProcess().HoaDonThuTienKy(_function, DatabaseConstant.Action.SINH_DU_LIEU, ref _objHoaDon, ref lstResponse);
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
            if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;

            foreach (DANH_SACH_KHE_UOC_VONG_VAY drv in raddgrGocLaiVayDS.SelectedItems)
            {
                if (drv.KE_HOACH_TKQD > 0)
                {
                    if (_lstKheUoc.Count(f => f.MA_KHACH_HANG.Equals(drv.MA_KHACH_HANG)) > 1)
                    {
                        _lstKheUoc.First(g => g.MA_KHACH_HANG.Equals(drv.MA_KHACH_HANG) && !g.MA_KHE_UOC.Equals(drv.MA_KHE_UOC)).KE_HOACH_TKQD = drv.KE_HOACH_TKQD;
                    }
                }
                _lstKheUoc.Remove(drv);
            }
            raddgrGocLaiVayDS.Rebind();
        }

        private void LoadDataGridKheUoc()
        {
            raddgrGocLaiVayDS.ItemsSource = null;
            raddgrGocLaiVayDS.ItemsSource = dtHoaDon;
        }

        private void btnCum_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                lstPopup.Clear();
                lstDieuKien.Add(Presentation.Process.Common.ClientInformation.MaDonViGiaoDich);
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_CUM.getValue(), lstDieuKien);

                SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(true, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey("M.TinDung.HoaDon.BinhKhanh.ucHDThuTienKyCTBinhKhanh.DanhSachCum");
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Content = popup;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow dr = lstPopup[0];
                    if (idCum != Convert.ToInt32(dr[1]))
                    {
                        idCum = Convert.ToInt32(dr[1]);
                        cmbCum.Tag = dr[4].ToString(); // ma don vi
                        cmbCum.Text = dr[2].ToString(); // ma cum

                        if (LayThongTinNgayGiaiNgan())
                        {
                            if (LayThongTinKyThu(true))
                            {
                                _lstKheUoc.Clear();
                                raddgrGocLaiVayDS.Rebind();
                            }
                        }
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

        private void btnDetail_Click(object sender, RoutedEventArgs e)
        {
            ViewPopup();
        }

        private void ConvertDataSetToObject(ref List<DANH_SACH_KHE_UOC_VONG_VAY> lst, DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                try
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["MA_KHACH_HANG"] != null && !String.IsNullOrEmpty(dr["MA_KHACH_HANG"].ToString()))
                        {
                            DANH_SACH_KHE_UOC_VONG_VAY obj = new DANH_SACH_KHE_UOC_VONG_VAY();
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

                                obj.NOP_TIEN_TU_TKKKH = "KHONG";
                                obj.NOP_TIEN_VAO_TKKKH = "KHONG";
                                obj.GHI_NHAN_VAO_TKNB = "KHONG";
                                obj.TRA_GOC_LAI_TRUOC_HAN = "KHONG";
                                obj.MA_DON_VI = Presentation.Process.Common.ClientInformation.MaDonViGiaoDich;
                            }
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

        private void SetEnabledAllControls(bool enable)
        {
            raddgrGocLaiVayDS.IsReadOnly = !enable;
            //grbThongTinChung.IsEnabled = enable;
            btnAdd.IsEnabled = enable;
            btnDelete.IsEnabled = enable;
            cmbCum.IsEnabled = enable;
            cmbKyThu.IsEnabled = enable;
            cmbSanPham.IsEnabled = enable;
            teldtNgayThuTien.IsEnabled = enable;
            dtpNgayThuTien.IsEnabled = enable;
            txtDienGiai.IsEnabled = enable;
            cmbNgayPhatVon.IsEnabled = enable;
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

        private bool LayThongTinKyThu(bool isNew = false)
        {
            bool kq = true;
            bool isSelected = false;
            List<DateTime> lstDate = new List<DateTime>();
            this.Cursor = Cursors.Wait;
            try
            {
                lstDate = new UtilitiesProcess().LayNgayHopCum(idCum, Convert.ToDateTime(teldtNgayThuTien.Value).ToString("yyyyMMdd"));
                if (lstDate != null && lstDate.Count > 0)
                {
                    cmbKyThu.Items.Clear();
                    for (int i = 0; i < lstDate.Count; i++)
                    {
                        cmbKyThu.Items.Add(new AutoCompleteEntry(LLanguage.SearchResourceByKey("M.TinDung.HoaDon.BinhKhanh.ucHDThuTienKyCTBinhKhanh.KyThu") + (i + 1).ToString() + LLanguage.SearchResourceByKey("M.TinDung.HoaDon.BinhKhanh.ucHDThuTienKyCTBinhKhanh.Ngay") + lstDate[i].ToString("dd/MM/yyyy"), lstDate[i].ToString("yyyyMMdd")));
                        if (!isSelected)
                        {
                            if (isNew)
                            {
                                if (lstDate[i] >= teldtNgayLap.Value)
                                {
                                    cmbKyThu.SelectedIndex = i;
                                    isSelected = true;
                                }
                            }
                            else
                            {
                                if (lstDate[i] == teldtNgayThuTien.Value)
                                {
                                    cmbKyThu.SelectedIndex = i;
                                    isSelected = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    kq = false;
                }
            }
            catch (Exception ex)
            {
                kq = false;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
            return kq;
        }

        private bool LayThongTinNgayGiaiNgan()
        {
            bool kq = true;
            try
            {
                lstSourcePhatVon = new List<AutoCompleteEntry>();
                cmbNgayPhatVon.Items.Clear();
                AutoCompleteEntry auCum = lstSourceCum.ElementAt(cmbCum.SelectedIndex);
                AutoCompleteEntry auSanPham = lstSourceSanPham.ElementAt(cmbSanPham.SelectedIndex);
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(auCum.KeywordStrings.FirstOrDefault());
                lstDieuKien.Add(auSanPham.KeywordStrings.FirstOrDefault());

                if (tthaiNvu.Equals("CDU") || tthaiNvu.Equals(""))
                    new AutoComboBox().GenAutoComboBox(ref lstSourcePhatVon, ref cmbNgayPhatVon, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NGAY_PHATVON_CUM.getValue(), lstDieuKien);
                else if (tthaiNvu.Equals("DDU"))
                    new AutoComboBox().GenAutoComboBox(ref lstSourcePhatVon, ref cmbNgayPhatVon, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NGAY_PHATVON_CUM_ALL.getValue(), lstDieuKien);
                else
                    new AutoComboBox().GenAutoComboBox(ref lstSourcePhatVon, ref cmbNgayPhatVon, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NGAY_PHATVON_CUM.getValue(), lstDieuKien);               
            }
            catch (Exception ex)
            {
                kq = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
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

            //obj.THUC_THU_LAI_QUA_HAN = Math.Min(obj.KE_HOACH_LAI_QUA_HAN, tongTienThu);
            //tongTienThu = Math.Max(0, tongTienThu - obj.KE_HOACH_LAI_QUA_HAN);

            //obj.THUC_THU_LAI_TRONG = Math.Min(obj.KE_HOACH_LAI_TRONG_HAN, tongTienThu);
            //tongTienThu = Math.Max(0, tongTienThu - obj.KE_HOACH_LAI_TRONG_HAN);

            //obj.THUC_THU_GOC_VAY = Math.Min(tongTienThu, obj.KE_HOACH_GOC_VAY);
            //tongTienThu = Math.Max(0, tongTienThu - obj.KE_HOACH_GOC_VAY);

            //if (tongTienThu > 0)
            //{
            //    obj.THUC_THU_NOP_VAO_TKKKH = tongTienThu;
            //    obj.NOP_TIEN_VAO_TKKKH = "CO";
            //}
            //else
            //{
            //    obj.THUC_THU_NOP_VAO_TKKKH = 0;
            //    obj.NOP_TIEN_VAO_TKKKH = "KHONG";
            //}
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

        private void chkThuHepNhom_Checked(object sender, RoutedEventArgs e)
        {
            raddgrGocLaiVayDS.CollapseAllGroups();
            chkThuHepNhom.IsChecked = false;
        }

        private void chkMoRongNhom_Checked(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            raddgrGocLaiVayDS.ExpandAllGroups();
            chkMoRongNhom.IsChecked = false;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void raddgrGocLaiVayDS_GotFocus(object sender, RoutedEventArgs e)
        {
            if (grbThongTinChung.IsExpanded == true)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                grbThongTinChung.IsExpanded = false;
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void raddgrGocLaiVayDS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewPopup();
        }

        private void ViewPopup()
        {
            if (raddgrGocLaiVayDS.SelectedItem != null)
            {
                Window window = new Window();
                try
                {
                    string ngayThuTien = "";
                    foreach (AutoCompleteCheckBox autoNgayThu in lstSourceKyThu)
                    {
                        if (autoNgayThu.CheckedMember && !autoNgayThu.ValueMember[0].Equals("All"))
                            ngayThuTien += autoNgayThu.ValueMember[0] + "#";
                    }
                    ngayThuTien = ngayThuTien.Substring(0, ngayThuTien.Length - 1);
                    DANH_SACH_KHE_UOC_VONG_VAY item = (DANH_SACH_KHE_UOC_VONG_VAY)raddgrGocLaiVayDS.SelectedItem;
                    PopupNghiepVu.ucPopupChiTietThucThuBinhKhanh uc = new PopupNghiepVu.ucPopupChiTietThucThuBinhKhanh(item, ngayThuTien);
                    window.Content = uc;
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.Title = LLanguage.SearchResourceByKey("M.TinDung.HoaDon.BinhKhanh.ucHDThuTienKyCTBinhKhanh.ThongTinChiTietKhachHang") + item.TEN_KHACH_HANG;
                    window.Width = 1024;
                    window.Height = 700;
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.ShowDialog();

                    int index = _lstKheUoc.IndexOf(item);
                    _lstKheUoc[index] = uc.objKheUocViMo;
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
        private void DefaultUserControl()
        {
            #region Reset biến
            tthaiNvu = "";
            dtHoaDon = null;
            dtHoaDon = new DataTable();
            lstPopup = null;
            lstPopup = new List<DataRow>();
            _objHoaDon = null;
            _lstKheUoc = null;
            _objKiemSoat = null;
            idCum = 0;
            #endregion

            #region Control
            txtSoGiaoDich.Text = "";
            teldtNgayLap.Value = LDateTime.StringToDate(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            teldtNgayThuTien.Value = LDateTime.StringToDate(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            cmbCum.Text = "";
            txtDienGiai.Text = "";
            txtTrangThai.Text = "";
            txtNguoiLap.Text = "";
            teldtNgayCNhat.Value = null;
            txtNguoiCapNhat.Text = "";
            raddgrGocLaiVayDS.ItemsSource = null;
            raddgrGocLaiVayDS.IsReadOnly = false;
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu);
            btnAdd.IsEnabled = false;
            btnDelete.IsEnabled = false;
            #endregion
        }

        private void cmbKyThu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbKyThu.SelectedItem != null)
            {
                AutoCompleteEntry auKyThu = (AutoCompleteEntry)cmbKyThu.SelectedItem;
                if (auKyThu.KeywordStrings != null && auKyThu.KeywordStrings.Length > 0)
                {
                    teldtNgayThuTien.Value = LDateTime.StringToDate(auKyThu.KeywordStrings[0], "yyyyMMdd");
                }
            }
        }

        private void teldtNgayThuTien_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            LoadComboBoxKyThu();
        }

        private void chkTatToan_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            GridViewRow grrow = chk.ParentOfType<GridViewRow>();
            DANH_SACH_KHE_UOC_VONG_VAY objKheUoc = grrow.Item as DANH_SACH_KHE_UOC_VONG_VAY;
            objKheUoc.TAT_TOAN = chk.IsChecked.GetValueOrDefault().ToString();
            raddgrGocLaiVayDS.CurrentItem = objKheUoc;
        }

        private void cmbKyThu_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceKyThu = cmbKyThu.ItemsSource as ListCheckBoxCombo;
        }

        private void LoadComboBoxKyThu()
        {
            if (LObject.IsNullOrEmpty(lstSourceCum) || LObject.IsNullOrEmpty(lstSourceSanPham) || lstSourceCum.Count == 0 || lstSourceSanPham.Count == 0 || LObject.IsNullOrEmpty(lstSourcePhatVon) || lstSourcePhatVon.Count == 0)
            {
                lstSourceKyThu = new ListCheckBoxCombo();
                new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceKyThu, ref cmbKyThu, null);
                return;
            }
            List<string> lstDieuKien = new List<string>();
            AutoCompleteEntry auCum = lstSourceCum.ElementAt(cmbCum.SelectedIndex);
            AutoCompleteEntry auPhatVon = lstSourcePhatVon.ElementAt(cmbNgayPhatVon.SelectedIndex);
            lstDieuKien.Add(auCum.KeywordStrings[0]);
            AutoCompleteEntry auSanPham = lstSourceSanPham.ElementAt(cmbSanPham.SelectedIndex);
            lstDieuKien.Add(auSanPham.KeywordStrings[0]);
            lstDieuKien.Add(teldtNgayThuTien.Value.GetValueOrDefault().ToString("yyyyMM")+"01");
            lstDieuKien.Add(auPhatVon.KeywordStrings[0]);
            if (LObject.IsNullOrEmpty(auCum) || LObject.IsNullOrEmpty(auSanPham))
                return;
            lstSourceKyThu = new ListCheckBoxCombo();
            new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceKyThu, ref cmbKyThu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KY_THU.getValue(), lstDieuKien);
            _lstKheUoc.Clear();
            raddgrGocLaiVayDS.Rebind();
        }

        void cmbSanPham_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LayThongTinNgayGiaiNgan();
        }

        void cmbCum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoCompleteEntry auCum = lstSourceCum.ElementAt(cmbCum.SelectedIndex);
            LayThongTinNgayGiaiNgan();
        }

        private void raddgrGocLaiVayDS_Loaded(object sender, RoutedEventArgs e)
        {
            if (raddgrGocLaiVayDS.Items.Count > 1)
                chkLayKyTruoc.IsEnabled = false;
            else
                chkLayKyTruoc.IsEnabled = true;
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
        }

        private void beforeAddNew()
        {
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, _function);
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
            TinDungProcess process = new TinDungProcess();
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
                    ret = process.HoaDonThuTienKy(_function, DatabaseConstant.Action.THEM, ref obj, ref listResponseDetail);

                    Mouse.OverrideCursor = Cursors.Arrow;
                    afterAddNew(ret, obj, listResponseDetail);
                }
                // Nếu là lưu tạm khi sửa
                // Hoặc lưu tạm khi sửa sau duyệt
                // Hoặc sửa
                else
                {
                    // Lấy dữ liệu từ form
                    ret = process.HoaDonThuTienKy(_function, DatabaseConstant.Action.SUA, ref obj, ref listResponseDetail);

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
                TinDungProcess process = new TinDungProcess();
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
                        ret = process.HoaDonThuTienKy(_function, DatabaseConstant.Action.THEM, ref obj, ref listResponseDetail);

                        Mouse.OverrideCursor = Cursors.Arrow;
                        afterAddNew(ret, obj, listResponseDetail);
                    }
                    // Nếu là lưu tạm khi sửa
                    // Hoặc lưu tạm khi sửa sau duyệt
                    // Hoặc sửa
                    else
                    {
                        // Lấy dữ liệu từ form
                        ret = process.HoaDonThuTienKy(_function, DatabaseConstant.Action.SUA, ref obj, ref listResponseDetail);

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
            TinDungProcess process = new TinDungProcess();
            try
            {
                Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY obj = new Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                // Dữ liệu truyền vào và dữ liệu trả về

                Mouse.OverrideCursor = Cursors.Wait;
                obj = GetFormData(tthaiNvu);
                ret = process.HoaDonThuTienKy(_function, DatabaseConstant.Action.XOA, ref obj, ref listResponseDetail);

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
            TinDungProcess process = new TinDungProcess();
            try
            {
                Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY obj = new Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                // Dữ liệu truyền vào và dữ liệu trả về

                Mouse.OverrideCursor = Cursors.Wait;
                obj = GetFormData(tthaiNvu);
                ret = process.HoaDonThuTienKy(_function, DatabaseConstant.Action.DUYET, ref obj, ref listResponseDetail);

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
            TinDungProcess process = new TinDungProcess();
            try
            {
                Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY obj = new Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                // Dữ liệu truyền vào và dữ liệu trả về

                Mouse.OverrideCursor = Cursors.Wait;
                obj = GetFormData(tthaiNvu);
                ret = process.HoaDonThuTienKy(_function, DatabaseConstant.Action.THOAI_DUYET, ref obj, ref listResponseDetail);

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
            TinDungProcess process = new TinDungProcess();
            try
            {
                Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY obj = new Presentation.Process.TinDungServiceRef.TDVM_LAP_HOA_DON_TIEN_KY();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                // Dữ liệu truyền vào và dữ liệu trả về

                Mouse.OverrideCursor = Cursors.Wait;
                obj = GetFormData(tthaiNvu);
                ret = process.HoaDonThuTienKy(_function, DatabaseConstant.Action.TU_CHOI_DUYET, ref obj, ref listResponseDetail);

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

        private void onNew()
        {
            cmbCum.SelectedIndex = 0;
            cmbSanPham.SelectedIndex = 0;
            ClearForm();
            SetEnabledAllControls(true);
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
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(obj.TRANG_THAI_NGHIEP_VU);
                //txtTrangThai.Text = lblTrangThai.Content.ToString();
                txtNguoiCapNhat.Text = obj.NGUOI_CAP_NHAT;
                teldtNgayCNhat.Value = LDateTime.StringToDate(obj.NGAY_CAP_NHAT, "yyyyMMdd");
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, _function);
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
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(_objKiemSoat.ID) && LObject.IsNullOrEmpty(_objKiemSoat.SO_GIAO_DICH))
            {
                LMessage.ShowMessage("M.TinDung.HoaDon.BinhKhanh.ucHDThuTienKyCTBinhKhanh.KhongCoThongTinSo", LMessage.MessageBoxType.Warning);
            }
            else
            {
                VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();

                GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.GDKT_IN_GIAO_DICH;
                objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDVM_LAP_HOA_DON_TIEN_KY;

                GDKT_GIAO_DICH objGDKT_GIAO_DICH = new GDKT_GIAO_DICH();
                objGDKT_GIAO_DICH.MaGiaoDich = _objKiemSoat.SO_GIAO_DICH;

                doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                doiTuongBaoCao.objGDKT_GIAO_DICH = objGDKT_GIAO_DICH;
                xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
            }
            Mouse.OverrideCursor = Cursors.Wait;
        }

        private void OnPreviewKeHoach()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(_objKiemSoat.ID) && LObject.IsNullOrEmpty(_objKiemSoat.SO_GIAO_DICH))
            {
                LMessage.ShowMessage("M.TinDung.HoaDon.BinhKhanh.ucHDThuTienKyCTBinhKhanh.KhongCoThongTinSo", LMessage.MessageBoxType.Warning);
            }
            else
            {
                BaoCaoProcess process = new BaoCaoProcess();
                DanhMucProcess dmProcess = new DanhMucProcess();

                HT_BAOCAO htBaoCao = new HT_BAOCAO();
                List<HT_BAOCAO_TSO> lstHtBaoCaoTso = new List<HT_BAOCAO_TSO>();
                int idBaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.TDVM_HOA_DON_THU_TIEN_KY.layIdBaoCao();
                string maBaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.TDVM_HOA_DON_THU_TIEN_KY.layMaBaoCao();

                DM_DON_VI phongGD = dmProcess.getDonViByMaCum(cmbCum.Text);
                string maPhong = LObject.IsNullOrEmpty(phongGD) ? "" : phongGD.MA_DVI;
                string tenPhong = LObject.IsNullOrEmpty(phongGD) ? "" : phongGD.TEN_GDICH;

                List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
                listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenNguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenChiNhanh", ClientInformation.TenDonVi, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri())); // fake
                listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenPGD", tenPhong, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenCum", cmbCum.Text.ToString(), ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayThangNam", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri())); // fake
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGiaoDich", maPhong, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaCum", cmbCum.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", Convert.ToDateTime(teldtNgayThuTien.Value).ToString("yyyyMMdd"), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

                listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.PDF.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

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
                if (teldtNgayLap.Value == null)
                {
                    LMessage.ShowMessage("M.TinDung.HoaDon.BinhKhanh.ucHDThuTienKyCTBinhKhanh.NgayLapKhongDeTrong", LMessage.MessageBoxType.Warning);
                    if (grbThongTinChung.IsExpanded == false)
                    {
                        grbThongTinChung.IsExpanded = true;
                    }
                    teldtNgayLap.Focus();
                    return false;
                }
                else if (teldtNgayThuTien.Value == null)
                {
                    LMessage.ShowMessage("M.TinDung.HoaDon.BinhKhanh.ucHDThuTienKyCTBinhKhanh.NgayThuTienKhongDeTrong", LMessage.MessageBoxType.Warning);
                    if (grbThongTinChung.IsExpanded == false)
                    {
                        grbThongTinChung.IsExpanded = true;
                    }
                    teldtNgayThuTien.Focus();
                    return false;
                }
                else if (LString.IsNullOrEmptyOrSpace(txtDienGiai.Text))
                {
                    LMessage.ShowMessage("M.TinDung.HoaDon.BinhKhanh.ucHDThuTienKyCTBinhKhanh.DienGiaiKhongDeTrong", LMessage.MessageBoxType.Warning);
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
            TinDungProcess process = new TinDungProcess();
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
            AutoCompleteEntry au = lstSourcePhatVon.ElementAt(cmbNgayPhatVon.SelectedIndex);
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
            obj.LOAI_TIEN = ClientInformation.MaDongNoiTe;
            obj.DIEN_GIAI = txtDienGiai.Text;
            obj.LY_DO = "";
            obj.MA_DVI = Presentation.Process.Common.ClientInformation.MaDonViGiaoDich;
            obj.NGAY_LAP = Convert.ToDateTime(teldtNgayLap.Value).ToString("yyyyMMdd");
            obj.NGAY_GIAO_DICH = ClientInformation.NgayLamViecHienTai;
            obj.NGAY_LAP_HOA_DON = ClientInformation.NgayLamViecHienTai;
            string ngayThuTien = "";
            foreach (AutoCompleteCheckBox autoNgayThu in lstSourceKyThu)
            {
                if (autoNgayThu.CheckedMember && !autoNgayThu.ValueMember[0].Equals("All"))
                    ngayThuTien += autoNgayThu.ValueMember[0] + "#";
            }
            obj.NGAY_THU_TIEN_KY = ngayThuTien.Substring(0,ngayThuTien.Length-1);
            AutoCompleteEntry auCum = lstSourceCum.ElementAt(cmbCum.SelectedIndex);
            obj.SO_CUM = auCum.KeywordStrings[0];
            obj.TEN_CUM = cmbCum.Text;
            AutoCompleteEntry auSanPham = lstSourceSanPham.ElementAt(cmbSanPham.SelectedIndex);
            obj.MA_SAN_PHAM = auSanPham.KeywordStrings[0];
            obj.TEN_SAN_PHAM = cmbSanPham.Text;
            obj.TRANG_THAI_BAN_GHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
            obj.TRANG_THAI_NGHIEP_VU = tthai;
            obj.NGAY_PHAT_VON = au.KeywordStrings.FirstOrDefault();
            TinhTongThucThu();
            obj.DSACH_KHE_UOC = _lstKheUoc.ToArray();
            return obj;
        }

        private void TinhTongThucThu()
        {
            foreach (DANH_SACH_KHE_UOC_VONG_VAY obj in _lstKheUoc)
            {
                obj.THUC_THU_TONG = obj.THUC_THU_TIEN_MAT + obj.THUC_THU_TKKKH;
                obj.TONG_SO_TIEN = obj.THUC_THU_TIEN_MAT + obj.THUC_THU_TKKKH;
                obj.THUC_THU_TIEN_MAT = obj.THUC_THU_TONG - obj.THUC_THU_TKKKH;
                if (obj.THUC_THU_NOP_VAO_TKKKH > 0)
                {
                    obj.NOP_TIEN_VAO_TKKKH = "CO";
                }

                if (obj.THUC_THU_TKKKH == 0)
                {
                    if (obj.DSACH_SO_NOP_TIEN != null)
                    {
                        obj.DSACH_SO_NOP_TIEN = null;
                    }
                }

                if (obj.NOP_TIEN_TU_TKKKH == "KHONG")
                {
                    if (obj.DSACH_SO_RUT_TIEN != null)
                    {
                        obj.DSACH_SO_RUT_TIEN = null;
                    }
                }
            }
        }

        #endregion

    }
}
