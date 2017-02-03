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
using Utilities.Common;
using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process.TaiSanDamBaoServiceRef;
using PresentationWPF.CustomControl;
using Presentation.Process.Common;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using System.Data;

namespace PresentationWPF.TaiSanDamBao
{
    /// <summary>
    /// Interaction logic for ucHopDongTheChapNhap.xaml
    /// </summary>
    public partial class ucHopDongTheChapNhap : UserControl
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
        private NHAP_XUAT_TSDB objNhapXuat = null;
        private List<THONG_TIN_TSDB> lstNhapXuat = null;
        private KIEM_SOAT _objKiemSoat = null;
        string maDViTao = "";
        string maDviQLy = ClientInformation.MaDonVi;
        public KIEM_SOAT ObjKiemSoat
        {
            get { return _objKiemSoat; }
            set { _objKiemSoat = value; }
        }

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        List<AutoCompleteEntry> lstSourceCanBo = new List<AutoCompleteEntry>();
        int _id_HDTC = 0;
        string _mA_HDTC = "";
        int _id_GDICH = 0;
        string _mA_GDICH = "";
        DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        KIEM_SOAT objKiemSoat = null;
        string tThai_NVu = "";
        #endregion

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
            OnModify(objNhapXuat);
        }
        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeDelete();
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
            tThai_NVu = BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri();
            OnSave();
        }
        private void SubmitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SubmitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tThai_NVu = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
            OnSave();
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
            BeforeApprove();
        }
        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeCancel();
        }
        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeRefuse();
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                OnModify(objNhapXuat);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                tThai_NVu = BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri();
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                tThai_NVu = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                OnSave();
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                OnModify(objNhapXuat);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                tThai_NVu = BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri();
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                tThai_NVu = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                OnSave();
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

        #region Khoi tao
        public ucHopDongTheChapNhap()
        {
            InitializeComponent();
            LoadComboBox();
            InitEventHanler();
            ResetForm();
        }

        public ucHopDongTheChapNhap(KIEM_SOAT objKiemSoat)
            : this()
        {
            _objKiemSoat = objKiemSoat;
            action = objKiemSoat.action;
            _id_GDICH = objKiemSoat.ID;
            _mA_GDICH = objKiemSoat.SO_GIAO_DICH;
            tThai_NVu = objKiemSoat.TTHAI_NVU;
            SetFormData();
            CommonFunction.RefreshButton(Toolbar, action, tThai_NVu, mnuMain, DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP_NHAP);
        }

        private void InitEventHanler()
        {
            btnSoHopDong.Click += new RoutedEventHandler(btnSoHopDong_Click);
            cmbLoaiPhieu.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiPhieu_SelectionChanged);
            tlbThemDetail.Click += new RoutedEventHandler(tlbThemDetail_Click);
            tlbXoaDetail.Click += new RoutedEventHandler(tlbXoaDetail_Click);
            cmbNguoiGiao.SelectionChanged += new SelectionChangedEventHandler(cmbNguoiGiao_SelectionChanged);
            cmbNguoiNhan.SelectionChanged += new SelectionChangedEventHandler(cmbNguoiNhan_SelectionChanged);
        }

        private void LoadComboBox()
        {
            //LoadComboboxCanBo();
        }

        private void LoadComboboxCanBo()
        {
            try
            {
                this.Cursor = Cursors.Wait;
                AutoComboBox auCB = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(maDviQLy);
                lstSourceCanBo.Clear();
                cmbNguoiGiao.Items.Clear();
                cmbNguoiNhan.Items.Clear();
                auCB.GenAutoComboBox(ref lstSourceCanBo, ref cmbNguoiGiao, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NHAN_SU.getValue(), lstDieuKien);
                auCB.GenAutoComboBox(ref lstSourceCanBo, ref cmbNguoiNhan, null,null);
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

        #endregion

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

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        void btnSoHopDong_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lstPopup.Clear();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add("(0)");
                lstDieuKien.Add("NULL");
                lstDieuKien.Add("HOP_DONG");
                lstDieuKien.Add(cmbLoaiPhieu.SelectedValue.ToString());
                lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_HDTC", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(true, simplePopupResponse);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.Content = popup;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    if (objNhapXuat.IsNullOrEmpty()) objNhapXuat = new NHAP_XUAT_TSDB();
                    objNhapXuat.MA_HDTC = _mA_HDTC = lstPopup[0]["MA_HDTC"].ToString();
                    objNhapXuat.ID_HDTC = _id_HDTC = Convert.ToInt32(lstPopup[0]["ID_HDTC"]);
                    txtSoHopDong.Text = lstPopup[0]["MA_HDTC"].ToString();
                    txtMaKhachHang.Text = lstPopup[0]["MA_KHANG"].ToString();
                    txtTenKhachHang.Text = lstPopup[0]["TEN_KHANG"].ToString();
                    txtSoCMND.Text = lstPopup[0]["GTQL_SO"].ToString();
                    if (lstPopup[0]["GTLQ_NGAY_CAP"] != DBNull.Value)
                        txtNgayCap.Value = LDateTime.StringToDate(lstPopup[0]["GTLQ_NGAY_CAP"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    DataTable dt = null;
                    DataSet ds;
                    LDatatable.MakeParameterTable(ref dt);
                    LDatatable.AddParameter(ref dt, "@ID_HDTC", "string", _id_HDTC.ToString());
                    LDatatable.AddParameter(ref dt, "@MA_HDTC", "string", _mA_HDTC);
                    LDatatable.AddParameter(ref dt, "@LOAI_CTU", "string", cmbLoaiPhieu.SelectedValue.ToString());
                    ds = new TaiSanDamBaoProcess().GetDanhSachTSNhapNgoaiBang(dt);
                    lstNhapXuat = new List<THONG_TIN_TSDB>();
                    if (!ds.IsNullOrEmpty() && ds.Tables.Count > 0)
                    {
                        
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            THONG_TIN_TSDB objTSDB = new THONG_TIN_TSDB();
                            objTSDB.ID_HDTC = _id_HDTC;
                            objTSDB.MA_HDTC = _mA_HDTC;
                            objTSDB.MA_KHANG = dr["MA_KHANG"].ToString();
                            objTSDB.MA_LOAI_TIEN = dr["MA_LOAI_TIEN"].ToString();
                            objTSDB.ID_TSDB = Convert.ToInt32(dr["ID_TSDB"]);
                            objTSDB.MA_TSDB = dr["MA_TSDB"].ToString();
                            objTSDB.TEN_LOAI_TSDB = dr["MA_LOAI_TSDB"].ToString();
                            objTSDB.TEN_TSDB = dr["TEN_TSDB"].ToString();
                            objTSDB.GTRI_HTOAN = Convert.ToDecimal(dr["GTRI_HTOAN"]);
                            objTSDB.GTRI_DGIA = Convert.ToDecimal(dr["GTRI_DGIA"]);
                            objTSDB.MA_DVI_QLY = dr["MA_DVI_QLY"].ToString();
                            lstNhapXuat.Add(objTSDB);
                        }
                        telTongTienGiaoDich.Value = (double)lstNhapXuat.Sum(f => f.GTRI_HTOAN);
                    }
                    grdKheUoc.ItemsSource = null;
                    grdKheUoc.ItemsSource = lstNhapXuat;
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

        void tlbXoaDetail_Click(object sender, RoutedEventArgs e)
        {
            if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            List<int> idXoa = new List<int>();
            foreach (THONG_TIN_TSDB obj in grdKheUoc.SelectedItems)
            {
                idXoa.Add(obj.ID_TSDB);
            }
            lstNhapXuat.RemoveAll(f => idXoa.Contains(f.ID_TSDB));
            grdKheUoc.ItemsSource = null;
            grdKheUoc.ItemsSource = lstNhapXuat;
            telTongTienGiaoDich.Value = (double)lstNhapXuat.Sum(f => f.GTRI_HTOAN);
        }

        void tlbThemDetail_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = null;
            DataSet ds;
            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_HDTC", "string", _id_HDTC.ToString());
            LDatatable.AddParameter(ref dt, "@MA_HDTC", "string", _mA_HDTC);
            LDatatable.AddParameter(ref dt, "@LOAI_CTU", "string", cmbLoaiPhieu.SelectedValue.ToString());
            ds = new TruyVanProcess().TruyVanUDTT("_DS_SP_LAY_DANH_SACH_TAI_SAN_THE_CHAP_NHAP_XUAT", dt);
            if (!ds.IsNullOrEmpty() && ds.Tables.Count > 0)
            {
                if (lstNhapXuat.IsNullOrEmpty()) lstNhapXuat = new List<THONG_TIN_TSDB>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    THONG_TIN_TSDB objTSDB = lstNhapXuat.FirstOrDefault(f=>f.ID_TSDB.Equals(Convert.ToInt32(dr["ID_TSDB"])));
                    if (objTSDB.IsNullOrEmpty())
                    {
                        objTSDB = new THONG_TIN_TSDB();
                        objTSDB.ID_HDTC = _id_HDTC;
                        objTSDB.MA_HDTC = _mA_HDTC;
                        objTSDB.MA_KHANG = dr["MA_KHANG"].ToString();
                        objTSDB.MA_LOAI_TIEN = dr["MA_LOAI_TIEN"].ToString();
                        objTSDB.ID_TSDB = Convert.ToInt32(dr["ID_TSDB"]);
                        objTSDB.MA_TSDB = dr["MA_TSDB"].ToString();
                        objTSDB.TEN_LOAI_TSDB = dr["MA_LOAI_TSDB"].ToString();
                        objTSDB.TEN_TSDB = dr["TEN_TSDB"].ToString();
                        objTSDB.GTRI_HTOAN = Convert.ToDecimal(dr["GTRI_HTOAN"]);
                        objTSDB.GTRI_DGIA = Convert.ToDecimal(dr["GTRI_DGIA"]);
                        objTSDB.MA_DVI_QLY = dr["MA_DVI_QLY"].ToString();
                        lstNhapXuat.Add(objTSDB);
                    }
                }
                grdKheUoc.ItemsSource = null;
                grdKheUoc.ItemsSource = lstNhapXuat;
                telTongTienGiaoDich.Value = (double)lstNhapXuat.Sum(f => f.GTRI_HTOAN);
            }
        }

        void cmbLoaiPhieu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            objNhapXuat = new NHAP_XUAT_TSDB();
            lstNhapXuat = new List<THONG_TIN_TSDB>();
            grdKheUoc.ItemsSource = null;
            grdKheUoc.ItemsSource = lstNhapXuat;
            txtSoHopDong.Text = "";
            _id_HDTC = 0;
            _mA_HDTC = "";
            txtMaKhachHang.Text = "";
            txtTenKhachHang.Text = "";
            txtSoCMND.Text = "";
            txtNgayCap.Value = null;
            if (cmbLoaiPhieu.SelectedValue.ToString().Equals("NHAP_NB"))
            {
                cmbNguoiNhan.Visibility = System.Windows.Visibility.Visible;
                lblNguoiNhan.Visibility = System.Windows.Visibility.Visible;
                stpMaNguoiNhan.Visibility = System.Windows.Visibility.Visible;
                cmbNguoiGiao.Visibility = System.Windows.Visibility.Collapsed;
                lblNguoiGiao.Visibility = System.Windows.Visibility.Collapsed;
                stpMaNguoiGiao.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                cmbNguoiGiao.Visibility = System.Windows.Visibility.Visible;
                lblNguoiGiao.Visibility = System.Windows.Visibility.Visible;
                stpMaNguoiGiao.Visibility = System.Windows.Visibility.Visible;
                cmbNguoiNhan.Visibility = System.Windows.Visibility.Collapsed;
                lblNguoiNhan.Visibility = System.Windows.Visibility.Collapsed;
                stpMaNguoiNhan.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        void cmbNguoiNhan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbNguoiNhan.SelectedIndex > -1)
            {
                AutoCompleteEntry au = lstSourceCanBo.ElementAt(cmbNguoiNhan.SelectedIndex);
                lblNguoiNhan.Content = LLanguage.SearchResourceByKey("U.PresentationWPF.TaiSanDamBao.TaiSanDamBao.ucHopDongTheChapNhap.ChucVu") + au.KeywordStrings[5];
            }
        }

        void cmbNguoiGiao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbNguoiGiao.SelectedIndex > -1)
            {
                AutoCompleteEntry au = lstSourceCanBo.ElementAt(cmbNguoiGiao.SelectedIndex);
                lblNguoiGiao.Content = LLanguage.SearchResourceByKey("U.PresentationWPF.TaiSanDamBao.TaiSanDamBao.ucHopDongTheChapNhap.ChucVu") + au.KeywordStrings[5];
            }
        }


        void ResetForm()
        {
            _objKiemSoat = null;
            txtSoGiaoDich.Text = "";
            _id_GDICH = 0;
            _mA_GDICH = "";
            raddtNgayGiaoDich.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            txtSoHopDong.Text = "";
            _id_HDTC = 0;
            _mA_HDTC = "";
            txtMaKhachHang.Text = "";
            txtTenKhachHang.Text = "";
            txtSoCMND.Text = "";
            txtNgayCap.Value = null;
            telTongTienGiaoDich.Value = 0;
            txtDienGiai.Text = "";
            lstNhapXuat = new List<THONG_TIN_TSDB>();
            grdKheUoc.ItemsSource = null;
            grdKheUoc.ItemsSource = lstNhapXuat;
            lblTrangThai.Content = "";
            txtNguoiCapNhat.Text = "";
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            teldtNgayCNhat.Value = null;
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            txtTrangThai.Text = "";
            tThai_NVu = "";
            LoadComboboxCanBo();
            action = DatabaseConstant.Action.THEM;
            CommonFunction.RefreshButton(Toolbar, action, tThai_NVu, mnuMain, DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP_NHAP);
        }
        #endregion

        #region Xy ly nghiep vu
        private bool LockData()
        {
            List<int> lstId = new List<int>();
            lstId.Add(_id_HDTC);
            UtilitiesProcess process = new UtilitiesProcess();
            return process.LockData(DatabaseConstant.Module.TSDB,
            DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP_NHAP,
            DatabaseConstant.Table.KT_GIAO_DICH,
            action,
            lstId);
        }

        private bool UnLock()
        {
            List<int> lstId = new List<int>();
            lstId.Add(_id_GDICH);
            UtilitiesProcess process = new UtilitiesProcess();
            return process.UnlockData(DatabaseConstant.Module.TSDB,
            DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP_NHAP,
            DatabaseConstant.Table.KT_GIAO_DICH,
            action,
            lstId);
        }

        private void GetFormData(ref NHAP_XUAT_TSDB obj, BusinessConstant.TrangThaiNghiepVu tthaiNVu)
        {
            try
            {
                if (obj.IsNullOrEmpty()) obj = new NHAP_XUAT_TSDB();
                obj.ID_GDICH = _id_GDICH;
                obj.ID_HDTC = _id_HDTC;
                obj.LOAI_CTU = cmbLoaiPhieu.SelectedValue.ToString();
                obj.MA_GDICH = _mA_GDICH;
                obj.MA_KHANG = txtMaKhachHang.Text;
                obj.TEN_KHANG = txtTenKhachHang.Text;
                obj.TONG_SO_TIEN = (decimal)telTongTienGiaoDich.Value.GetValueOrDefault();
                obj.NGAY_GDICH = LDateTime.DateToString(raddtNgayGiaoDich.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
                AutoCompleteEntry auNguoiNhan = lstSourceCanBo.ElementAt(cmbNguoiNhan.SelectedIndex);
                if (!obj.LOAI_CTU.Equals("NHAP_NB"))
                    auNguoiNhan = lstSourceCanBo.ElementAt(cmbNguoiGiao.SelectedIndex);
                obj.MA_CHUC_VU = auNguoiNhan.KeywordStrings[4];
                obj.TEN_CHUC_VU = auNguoiNhan.KeywordStrings[5];
                obj.TEN_NGUOI_GIAO_NHAN = auNguoiNhan.DisplayName;
                obj.MA_NGUOI_GIAO_NHAN = auNguoiNhan.KeywordStrings[0];
                obj.MA_HDTC = _mA_HDTC;
                if (!txtNgayCap.Value.IsNullOrEmpty())
                    obj.NGAY_CAP = LDateTime.DateToString(txtNgayCap.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
                obj.SO_CMND = txtSoCMND.Text;
                obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.TTHAI_NVU = tthaiNVu.layGiaTri();
                obj.DIEN_GIAI = txtDienGiai.Text;
                obj.DDSACH_TTIN_TSDB = lstNhapXuat.ToArray();
                if (_id_GDICH == 0)
                {
                    obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                    obj.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    obj.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    obj.NGAY_GDICH = ClientInformation.NgayLamViecHienTai;
                }
                else
                {
                    obj.MA_DVI_QLY = maDviQLy;
                    obj.MA_DVI_TAO = maDViTao;
                    obj.NGAY_NHAP = teldtNgayNhap.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                    obj.NGUOI_NHAP = txtNguoiLap.Text;
                    obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            lstNhapXuat = new List<THONG_TIN_TSDB>();
            try
            {
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@INP_ID_GDICH", "string", _id_GDICH.ToString());
                LDatatable.AddParameter(ref dt, "@INP_MA_GDICH", "string", _mA_GDICH);
                DataSet ds = new TaiSanDamBaoProcess().GetNhapNgoaiBang(dt);
                if (!ds.IsNullOrEmpty() && ds.Tables.Count > 1)
                {
                    dt = ds.Tables["TTIN_CHUNG"];
                    _id_HDTC = Convert.ToInt32(dt.Rows[0]["ID_HDTC"]);
                    _mA_HDTC = dt.Rows[0]["MA_HDTC"].ToString();
                    txtSoGiaoDich.Text = _mA_GDICH;
                    raddtNgayGiaoDich.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_GDICH"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    if (dt.Rows[0]["LOAI_CTU"].ToString().Equals("NHAP_NB"))
                        cmbLoaiPhieu.SelectedIndex = 0;
                    else
                        cmbLoaiPhieu.SelectedIndex = 1;
                    txtSoHopDong.Text = _mA_HDTC;
                    txtMaKhachHang.Text = dt.Rows[0]["MA_KHANG"].ToString();
                    txtTenKhachHang.Text = dt.Rows[0]["TEN_KHANG"].ToString();
                    txtSoCMND.Text = dt.Rows[0]["SO_CMND"].ToString();
                    if (dt.Rows[0]["NGAY_CAP"] != DBNull.Value)
                        txtNgayCap.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_CAP"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    maDviQLy = dt.Rows[0]["MA_DVI_QLY"].ToString();
                    maDViTao = dt.Rows[0]["MA_DVI_TAO"].ToString();
                    LoadComboboxCanBo();
                    cmbNguoiGiao.SelectedIndex = lstSourceCanBo.IndexOf(lstSourceCanBo.FirstOrDefault(f=>f.KeywordStrings.FirstOrDefault().Equals(dt.Rows[0]["MA_NGUOI_GIAO_NHAN"].ToString())));
                    cmbNguoiNhan.SelectedIndex = lstSourceCanBo.IndexOf(lstSourceCanBo.FirstOrDefault(f=>f.KeywordStrings.FirstOrDefault().Equals(dt.Rows[0]["MA_NGUOI_GIAO_NHAN"].ToString())));
                    telTongTienGiaoDich.Value = Convert.ToDouble(dt.Rows[0]["TONG_SO_TIEN"]);
                    txtDienGiai.Text = dt.Rows[0]["DIEN_GIAI"].ToString();
                    #region Thông tin kiểm soát
                    tThai_NVu = dt.Rows[0]["TTHAI_NVU"].ToString();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tThai_NVu);
                    txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(dt.Rows[0]["TTHAI_BGHI"].ToString());
                    teldtNgayNhap.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_NHAP"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    txtNguoiLap.Text = dt.Rows[0]["NGUOI_NHAP"].ToString();
                    if (LDateTime.IsDate(dt.Rows[0]["NGAY_CNHAT"].ToString(), ApplicationConstant.defaultDateTimeFormat) == true)
                        teldtNgayCNhat.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_CNHAT"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    else
                        teldtNgayCNhat.Value = null;
                    txtNguoiCapNhat.Text = dt.Rows[0]["NGUOI_CNHAT"].ToString();
                    #endregion
                    dt = ds.Tables["TTIN_TSDB"];
                    lstNhapXuat = new List<THONG_TIN_TSDB>();
                    THONG_TIN_TSDB objTSDB = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        objTSDB = new THONG_TIN_TSDB();
                        objTSDB.GTRI_HTOAN = Convert.ToDecimal(dr["GTRI_HTOAN"]);
                        objTSDB.ID = Convert.ToInt32(dr["ID"]);
                        objTSDB.ID_GDICH = _id_GDICH;
                        objTSDB.ID_HDTC = _id_HDTC;
                        objTSDB.ID_TSDB = Convert.ToInt32(dr["ID_TSDB"]);
                        objTSDB.MA_GDICH = _mA_GDICH;
                        objTSDB.MA_HDTC = _mA_HDTC;
                        objTSDB.MA_KHANG = dr["MA_KHANG"].ToString();
                        objTSDB.MA_LOAI_TIEN = dr["MA_LOAI_TIEN"].ToString();
                        objTSDB.MA_TSDB = dr["MA_TSDB"].ToString();
                        objTSDB.TEN_LOAI_TSDB = dr["TEN_LOAI_TSDB"].ToString();
                        objTSDB.TEN_TSDB = dr["TEN_TSDB"].ToString();
                        objTSDB.GTRI_DGIA = Convert.ToDecimal(dr["GTRI_DGIA"]);
                        objTSDB.MA_DVI_QLY = dr["MA_DVI_QLY"].ToString();
                        objTSDB.MA_DVI_TAO = dr["MA_DVI_TAO"].ToString();
                        
                        lstNhapXuat.Add(objTSDB);
                    }
                    grdKheUoc.ItemsSource = null;
                    grdKheUoc.ItemsSource = lstNhapXuat;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void SetEnabledControls(bool bBool)
        {
            tlbThemDetail.IsEnabled = bBool;
            tlbXoaDetail.IsEnabled = bBool;
            cmbLoaiPhieu.IsEnabled = bBool;
            txtSoHopDong.IsEnabled = bBool;
            btnSoHopDong.IsEnabled = bBool;
            cmbNguoiNhan.IsEnabled = bBool;
            txtDienGiai.IsEnabled = bBool;
            grdKheUoc.IsReadOnly = !bBool;
            grbTTinGiaoDich.IsEnabled = bBool;

        }

        private bool Validation()
        {
            if (txtSoHopDong.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblSoHopDong.Content.ToString());
                btnSoHopDong.Focus();
                return false;
            }
            else if (txtDienGiai.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblDienGiai.Content.ToString());
                txtDienGiai.Focus();
                return false;
            }
            lstNhapXuat = grdKheUoc.ItemsSource as List<THONG_TIN_TSDB>;
            if (lstNhapXuat.IsNullOrEmpty() || lstNhapXuat.Count == 0)
            {
                CommonFunction.ThongBaoTrong(grbThongTinTSDB.Header.ToString()+":");
                tlbThemDetail.Focus();
                return false;
            }
            return true;
        }

        public void OnHold()
        {
            try
            {

            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnSave()
        {
            if (!Validation())
                return;
            GetFormData(ref objNhapXuat, BusinessConstant.layTrangThaiNghiepVu(tThai_NVu));
            TaiSanDamBaoProcess processTSDB = new TaiSanDamBaoProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            bool ret = false;
            if (_id_GDICH == 0)
                ret = processTSDB.NhapNgoaiBang(DatabaseConstant.Action.THEM, ref objNhapXuat, ref listClientResponseDetail);
            else
                ret = processTSDB.NhapNgoaiBang(DatabaseConstant.Action.SUA, ref objNhapXuat, ref listClientResponseDetail);
            AfterSave(ret, listClientResponseDetail);
        }

        public void AfterSave(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(listClientResponseDetail);
            if (ret)
            {
                if (!cbMultiAdd.IsChecked.Value)
                {
                    _id_GDICH = objNhapXuat.ID_GDICH;
                    _mA_GDICH = objNhapXuat.MA_GDICH;
                    SetEnabledControls(false);
                    SetFormData();
                    action = DatabaseConstant.Action.XEM;
                    CommonFunction.RefreshButton(Toolbar, action, tThai_NVu, mnuMain, DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP_NHAP);
                }
                else
                {
                    SetEnabledControls(true);
                    ResetForm();
                }
            }
        }

        public void BeforeViewFromDetail()
        {

        }

        public void BeforeViewFromList()
        {
            try
            {
                SetFormData();
                BeforeViewFromDetail();
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnAddNew(NHAP_XUAT_TSDB obj)
        {

            try
            {

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {

            }
        }

        public void BeforeModifyFromDetail()
        {
            try
            {
                action = DatabaseConstant.Action.SUA;
                SetEnabledControls(true);
                CommonFunction.RefreshButton(Toolbar, action, tThai_NVu, mnuMain, DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP_NHAP);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void BeforeModifyFromList()
        {
            try
            {
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnModify(NHAP_XUAT_TSDB obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                action = DatabaseConstant.Action.SUA;
                LockData();
                SetEnabledControls(true);
                CommonFunction.RefreshButton(Toolbar, action, tThai_NVu, mnuMain, DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP_NHAP);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public void AfterModify(bool ret, NHAP_XUAT_TSDB obj, List<ClientResponseDetail> listClientResponseDetail)
        {

        }


        public void BeforeDelete()
        {

            OnDelete();
        }

        public void OnDelete()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TaiSanDamBaoProcess processTSDB = new TaiSanDamBaoProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;
                ret = LockData();
                List<int> lst = new List<int>();
                objNhapXuat = new NHAP_XUAT_TSDB();
                objNhapXuat.ID_GDICH = _id_GDICH;
                objNhapXuat.MA_GDICH = _mA_GDICH;
                objNhapXuat.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                lst.Add(_id_GDICH);
                if (ret)
                    ret = processTSDB.NhapNgoaiBang(DatabaseConstant.Action.XOA, ref objNhapXuat, ref listClientResponseDetail);
                AfterDelete(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public void AfterDelete(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(listClientResponseDetail);
            if (ret)
            {
                CommonFunction.CloseUserControl(this);
            }
        }

        public void BeforeApprove()
        {
            OnApprove();
        }

        public void OnApprove()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TaiSanDamBaoProcess processTSDB = new TaiSanDamBaoProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;
                action = DatabaseConstant.Action.DUYET;
                ret = LockData();
                List<int> lst = new List<int>();
                objNhapXuat = new NHAP_XUAT_TSDB();
                objNhapXuat.ID_GDICH = _id_GDICH;
                objNhapXuat.MA_GDICH = _mA_GDICH;
                objNhapXuat.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                lst.Add(_id_GDICH);
                if (ret)
                    ret = processTSDB.NhapNgoaiBang(action, ref objNhapXuat, ref listClientResponseDetail);
                AfterApprove(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public void AfterApprove(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(listClientResponseDetail);
            UnLock();
            if (ret)
            {

                SetFormData();
                action = DatabaseConstant.Action.XEM;
                CommonFunction.RefreshButton(Toolbar, action, tThai_NVu, mnuMain, DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP_NHAP);
            }
        }

        public void BeforeRefuse()
        {
            OnRefuse();
        }

        public void OnRefuse()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TaiSanDamBaoProcess processTSDB = new TaiSanDamBaoProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;
                action = DatabaseConstant.Action.TU_CHOI_DUYET;
                ret = LockData();
                List<int> lst = new List<int>();
                objNhapXuat = new NHAP_XUAT_TSDB();
                objNhapXuat.ID_GDICH = _id_GDICH;
                objNhapXuat.MA_GDICH = _mA_GDICH;
                objNhapXuat.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                lst.Add(_id_GDICH);
                if (ret)
                    ret = processTSDB.NhapNgoaiBang(action, ref objNhapXuat, ref listClientResponseDetail);
                AfterRefuse(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public void AfterRefuse(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(listClientResponseDetail);
            UnLock();
            if (ret)
            {
                SetFormData();
                action = DatabaseConstant.Action.XEM;
                CommonFunction.RefreshButton(Toolbar, action, tThai_NVu, mnuMain, DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP_NHAP);
            }
        }

        public void BeforeCancel()
        {
            OnCancel();
        }

        public void OnCancel()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TaiSanDamBaoProcess processTSDB = new TaiSanDamBaoProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;
                action = DatabaseConstant.Action.THOAI_DUYET;
                ret = LockData();
                List<int> lst = new List<int>();
                objNhapXuat = new NHAP_XUAT_TSDB();
                objNhapXuat.ID_GDICH = _id_GDICH;
                objNhapXuat.MA_GDICH = _mA_GDICH;
                objNhapXuat.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                lst.Add(_id_GDICH);
                if (ret)
                    ret = processTSDB.NhapNgoaiBang(action, ref objNhapXuat, ref listClientResponseDetail);
                AfterCancel(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public void AfterCancel(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(listClientResponseDetail);
            UnLock();
            if (ret)
            {
                SetFormData();
                action = DatabaseConstant.Action.XEM;
                CommonFunction.RefreshButton(Toolbar, action, tThai_NVu, mnuMain, DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP_NHAP);
            }
        }
        #endregion
    }
}
