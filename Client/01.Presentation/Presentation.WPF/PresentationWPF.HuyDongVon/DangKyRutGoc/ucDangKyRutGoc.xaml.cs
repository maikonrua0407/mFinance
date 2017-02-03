using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
using Telerik.Windows.Controls;
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.HuyDongVonServiceRef;
using PresentationWPF.CustomControl;
using PresentationWPF.HuyDongVon.Popup;

namespace PresentationWPF.HuyDongVon.DangKyRutGoc
{
    /// <summary>
    /// Interaction logic for ucDangKyRutGoc.xaml
    /// </summary>
    public partial class ucDangKyRutGoc : UserControl
    {
        #region Khai bao
        bool isLoad = false;
        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private DatabaseConstant.Function function = DatabaseConstant.Function.HDV_DANG_KY_RUT_GOC;

        public event EventHandler OnSavingCompleted;

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        int idDonVi;
        int idKhuVuc = 0;
        int idCum = 0;
        int idTienGui = 0;
        int chuKy = 0;

        private HDV_THONG_TIN_DK_RUT_GOC obj;
        public HDV_THONG_TIN_DK_RUT_GOC Obj
        {
            get { return obj; }
            set { obj = value; }
        }

        private string sTrangThaiNVu = "";

        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGiaoDich = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKhuVuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceGiaoVon = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCBQL = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHinhThucDangKy = new List<AutoCompleteEntry>();

        DataTable dtThongTinRutGoc;
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand AddCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        #endregion

        #region Khoi tao
        public ucDangKyRutGoc()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            LoadCombobox();

            KhoiTaoDataTable();

            InitEventHandler();

            ClearForm();

        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.HuyDongVon;component/DangKyRutGoc/ucDangKyRutGoc.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void InitEventHandler()
        {

            cmbKhuVuc.SelectionChanged += new SelectionChangedEventHandler(cmbKhuVuc_SelectionChanged);
            cmbCum.SelectionChanged += new SelectionChangedEventHandler(cmbCum_SelectionChanged);
            cmbHinhThuc.SelectionChanged += new SelectionChangedEventHandler(cmbHinhThuc_SelectionChanged);
            tlbDetailAdd.Click += new RoutedEventHandler(tlbDetailAdd_Click);
            tlbDetailDelete.Click += new RoutedEventHandler(tlbDetailDelete_Click);
            raddgrThongTin.CellEditEnded += new EventHandler<GridViewCellEditEndedEventArgs>(raddgrThongTin_CellEditEnded);
            dtpThangGiaoVon.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(dtpThangGiaoVon_SelectedDateChanged);

        }

        private void LoadCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            idDonVi = ClientInformation.IdDonViGiaoDich;
            lstDieuKien.Clear();

            LoadComboBoxKhuVuc();
            LoadComboBoxCum();
            LoadComboBoxCBQL();
            LoadComboBoxHinhThucDangKy();
        }

        private void KhoiTaoDataTable()
        {
            dtThongTinRutGoc = new DataTable();
            dtThongTinRutGoc.Columns.Add("STT", typeof(int));
            dtThongTinRutGoc.Columns.Add("SO_SO_TG", typeof(string));
            dtThongTinRutGoc.Columns.Add("ID_TIEN_GUI", typeof(int));
            dtThongTinRutGoc.Columns.Add("CHU_KY_VAY", typeof(int));
            dtThongTinRutGoc.Columns.Add("MA_KHANG", typeof(string));
            dtThongTinRutGoc.Columns.Add("TEN_KHANG", typeof(string));
            dtThongTinRutGoc.Columns.Add("NGAY_MO_SO", typeof(string));
            dtThongTinRutGoc.Columns.Add("SO_TIEN", typeof(decimal));
            dtThongTinRutGoc.Columns.Add("SO_TIEN_RUT", typeof(decimal));
            dtThongTinRutGoc.Columns.Add("SO_DU", typeof(decimal));

        }
        #endregion

        #region Dang ky hot key, shortcut key

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
                        key = new KeyBinding(HoldCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(SaveCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.N, ModifierKeys.Control);
                        key = new KeyBinding(AddCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(ModifyCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.D, ModifierKeys.Control);
                        key = new KeyBinding(DeleteCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ApproveCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(RefuseCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.C, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(CancelCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(HelpCommand, keyg);
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
            e.CanExecute = true;
        }
        private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnSave();
        }

        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeAddNew();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeModifyFromDetail();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeDelete();
        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void HelpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnHelp();
        }

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnClose();
        }

        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                BeforeAddNew();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                BeforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
            }
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                OnHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                OnClose();
            }
        }

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                BeforeAddNew();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                BeforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
            }
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                OnHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                OnClose();
            }
        }

        #endregion

        #region Xu ly Giao dien
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Hiển thị Form khi thêm mới dữ liệu
            if (action == DatabaseConstant.Action.THEM)
            {
                BeforeAddNew();
            }

            //Hiển thị Form khi sửa dữ liệu
            else if (action == DatabaseConstant.Action.SUA)
            {
                BeforeModifyFromList();
            }

            //Hiển thị Form khi xem dữ liệu
            else if (action == DatabaseConstant.Action.XEM)
            {
                BeforeViewFromList();
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (OnSavingCompleted != null)
            {
                OnSavingCompleted(this, EventArgs.Empty);
            }
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void OnHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }

        /// <summary>
        /// Đóng form
        /// </summary>
        private void OnClose()
        {
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        /// <summary>
        /// Sự kiện khi thay đổi giá trị của combobox cmbDonVi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        void tlbDetailDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<int> lstSTT = new List<int>();
                for (int i = 0; i < raddgrThongTin.SelectedItems.Count; i++)
                {
                    DataRowView dr = (DataRowView)raddgrThongTin.SelectedItems[i];
                    lstSTT.Add(Convert.ToInt32(dr["STT"]));
                }
                lstSTT.SortByDesc();
                foreach (int stt in lstSTT)
                    dtThongTinRutGoc.Rows.RemoveAt(stt - 1);

                for (int i = 0; i < dtThongTinRutGoc.Rows.Count; i++)
                {
                    dtThongTinRutGoc.Rows[i]["STT"] = i + 1;
                }

                raddgrThongTin.DataContext = dtThongTinRutGoc.DefaultView;
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        void tlbDetailAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PopupProcess popupProcess = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonVi);
                lstDieuKien.Add(lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings.ElementAt(1));
                lstDieuKien.Add(lstSourceGiaoVon.ElementAt(cmbDotTra.SelectedIndex).KeywordStrings.ElementAt(2));
                popupProcess.getPopupInformation("POPUP_DS_KHANG_HANG_DKY_RUT_TKIEM", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(false, simplePopupResponse, true);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.Content = popup;
                win.ShowDialog();
                if (lstPopup != null)
                {
                    foreach (DataRow dr in lstPopup)
                    {
                        DataRow dradd = dtThongTinRutGoc.NewRow();
                        dradd["MA_KHANG"] = dr["MA_KHANG"];
                        dradd["TEN_KHANG"] = dr["TEN_KHANG"];
                        dradd["SO_SO_TG"] = dr["SO_SO_TG"];
                        dradd["NGAY_MO_SO"] = dr["NGAY_MO_SO"];
                        dradd["SO_TIEN"] = dr["SO_TIEN"];
                        if (cmbHinhThuc.SelectedIndex == 1)
                        {
                            dradd["SO_TIEN_RUT"] = dr["SO_TIEN"];
                            dradd["SO_DU"] = 0;
                            SO_TIEN_RUT.IsReadOnly = true;
                        }
                        dradd["ID_TIEN_GUI"] = dr["ID_TIEN_GUI"];
                        idTienGui = Convert.ToInt32(dr["ID_TIEN_GUI"]);
                        chuKy = Convert.ToInt32(dr["CHU_KY_VAY"]);
                        dtThongTinRutGoc.Rows.Add(dradd);

                        for (int i = 0; i < dtThongTinRutGoc.Rows.Count; i++)
                        {
                            dtThongTinRutGoc.Rows[i]["STT"] = i + 1;
                        }
                    }
                    raddgrThongTin.ItemsSource = dtThongTinRutGoc.DefaultView;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void raddgrThongTin_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        { 
            string column = e.Cell.Column.Name;

            if (column.Equals("SO_TIEN_RUT"))
            {
                DataRowView dr = (DataRowView)raddgrThongTin.CurrentCellInfo.Item;
                decimal soTien = Convert.ToDecimal(dr["SO_TIEN"]);
                decimal soTienRut = Convert.ToDecimal(dr["SO_TIEN_RUT"]);
                dr["SO_DU"] = soTien - soTienRut;
                raddgrThongTin.CurrentItem = dr;
            }
        }

        void cmbCum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboBoxDotPhatVon();
        }

        void cmbKhuVuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboBoxCum();
        }

        void cmbHinhThuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbHinhThuc.SelectedIndex == 0)
            {
                SO_TIEN_RUT.IsReadOnly = false;
                foreach (DataRow dr in dtThongTinRutGoc.Rows)
                {
                    decimal soTien = Convert.ToDecimal(dr["SO_TIEN"]);
                    dr["SO_TIEN_RUT"] = 0;
                    dr["SO_DU"] = 0;
                    raddgrThongTin.CurrentItem = dr;
                }
            }
            else if (cmbHinhThuc.SelectedIndex == 1)
            {
                SO_TIEN_RUT.IsReadOnly = true;
                foreach (DataRow dr in dtThongTinRutGoc.Rows)
                {
                    decimal soTien = Convert.ToDecimal(dr["SO_TIEN"]);
                    dr["SO_TIEN_RUT"] = soTien;
                    dr["SO_DU"] = 0;
                    raddgrThongTin.CurrentItem = dr;
                }
            }
        }

        void LoadComboBoxKhuVuc()
        {
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(idDonVi.ToString());
            lstSourceKhuVuc.Clear();
            cmbKhuVuc.Items.Clear();
            new AutoComboBox().GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, "COMBOBOX_KHUVUC", lstDieuKien, idKhuVuc.ToString());
        }

        void LoadComboBoxCum()
        {
            lstSourceCum.Clear();
            cmbCum.Items.Clear();
            if (cmbKhuVuc.Items.Count <= 0 || cmbKhuVuc.SelectedIndex < 0)
                return;
            AutoCompleteEntry auKhuVuc = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex);
            idKhuVuc = auKhuVuc.KeywordStrings[1].StringToInt32();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(idDonVi.ToString());
            lstDieuKien.Add(auKhuVuc.KeywordStrings[1]);
            new AutoComboBox().GenAutoComboBox(ref lstSourceCum, ref cmbCum, "COMBOBOX_CUM", lstDieuKien, idCum.ToString());
        }

        void LoadComboBoxCBQL()
        {
            List<string> lstDieuKien = new List<string>();
            cmbCBQL.Items.Clear();
            lstDieuKien.Clear();
            lstSourceCBQL.Clear();
            lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
            new AutoComboBox().GenAutoComboBox(ref lstSourceCBQL, ref cmbCBQL, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NSD.getValue(), lstDieuKien);
        }

        void LoadComboBoxHinhThucDangKy()
        {
            List<string> lstDieuKien = new List<string>();
            COMBOBOX_DTO combo = null;
            List<COMBOBOX_DTO> lstCombobox = new List<COMBOBOX_DTO>();
            AutoComboBox auto = new AutoComboBox();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.HINH_THUC_RUT_GOC.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbHinhThuc;
            combo.lstSource = lstSourceHinhThucDangKy;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);
            auto.GenAutoComboBoxTheoList(ref lstCombobox);
        }

        void LoadComboBoxDotPhatVon()
        {
            if (isLoad)
                return;
            lstSourceGiaoVon.Clear();
            cmbDotTra.Items.Clear();
            if (cmbCum.Items.Count <= 0 || cmbCum.SelectedIndex < 0)
                return;
            AutoCompleteEntry auCum = lstSourceCum.ElementAt(cmbCum.SelectedIndex);
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(LDateTime.DateToString(teldtThangGiaoVon.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat));
            lstDieuKien.Add("PHAT_VON");
            lstDieuKien.Add(auCum.KeywordStrings[1]);
            new AutoComboBox().GenAutoComboBox(ref lstSourceGiaoVon, ref cmbDotTra, "COMBOBOX_DOT_THU_PHAT", lstDieuKien);
        }

        void dtpThangGiaoVon_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboBoxDotPhatVon();
        }

        void ClearForm()
        {
            raddtNgayDangKy.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            teldtThangGiaoVon.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            LoadComboBoxDotPhatVon();
        }

        #endregion

        #region Xử lý nghiệp vụ
        private void GetFormData(ref HDV_THONG_TIN_DK_RUT_GOC obj)
        {
            try
            {
                List<VKT_HDV_DKY_RGOC> lstDKy = new List<VKT_HDV_DKY_RGOC>();
                VKT_HDV_DKY_RGOC objDK = null;
                foreach (DataRow dr in dtThongTinRutGoc.Rows)
                {
                    objDK = new VKT_HDV_DKY_RGOC();
                    AutoCompleteEntry auNgayGiaoVon = lstSourceGiaoVon.ElementAt(cmbDotTra.SelectedIndex);
                    objDK.ID_TIEN_GUI = Convert.ToInt32(dr["ID_TIEN_GUI"]);
                    objDK.TEN_BANG = DatabaseConstant.Table.BL_TIEN_GUI.getValue();
                    objDK.MA_DOI_TUONG = "VKT_HDV_DKY_RGOC";
                    objDK.ID_KVUC = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings.ElementAt(1);
                    objDK.MA_KVUC = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings.First();
                    objDK.TEN_KVUC = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings.ElementAt(2);
                    objDK.TEN_CUM = lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings.ElementAt(2);
                    objDK.ID_CUM = lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings.ElementAt(1);
                    objDK.MA_CUM = lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings.First();
                    objDK.HTHUC_DKY = lstSourceHinhThucDangKy.ElementAt(cmbHinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                    objDK.DIEN_GIAI = txtDienGiai.Text;
                    {
                        obj.NGAY_DL = Convert.ToDateTime(raddtNgayDangKy.Value).ToString("yyyyMMdd");
                        objDK.NGAY_DL = Convert.ToDateTime(raddtNgayDangKy.Value).ToString("yyyyMMdd");
                    }
                    objDK.MA_CBQL = lstSourceCBQL.ElementAt(cmbCBQL.SelectedIndex).KeywordStrings.First();
                    objDK.NGAY_TRA = auNgayGiaoVon.KeywordStrings[2];
                    if (teldtThangGiaoVon.Value != null)
                        objDK.THANG_TRA = Convert.ToDateTime(teldtThangGiaoVon.Value).ToString("yyyyMMdd");
                    //Thông tin kiểm soát
                    objDK.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    objDK.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    objDK.MA_DVI_TAO = ClientInformation.MaDonVi;
                    objDK.NGAY_NHAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                    objDK.NGUOI_NHAP = txtNguoiLap.Text;
                    if (action != DatabaseConstant.Action.THEM)
                    {
                        objDK.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                        objDK.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    }
                    objDK.MA_KHANG = dr["MA_KHANG"].ToString();
                    objDK.TEN_KHANG = dr["TEN_KHANG"].ToString();
                    objDK.NGAY_MO_SO = dr["NGAY_MO_SO"].ToString();
                    objDK.SO_SO_TG = dr["SO_SO_TG"].ToString();
                    objDK.SO_TIEN_GOC = dr["SO_TIEN"].ToString();
                    objDK.SO_TIEN_RUT_GOC = dr["SO_TIEN_RUT"].ToString();
                    objDK.SO_DU = dr["SO_DU"].ToString();
                    objDK.CHU_KY_VAY = dr["CHU_KY_VAY"].ToString();

                    lstDKy.Add(objDK);
                }

                obj.LST_HDV_DKY_RGOC = lstDKy.ToArray();

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            HuyDongVonProcess processHDV = new HuyDongVonProcess();

            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            obj = new HDV_THONG_TIN_DK_RUT_GOC();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                bool ret = false;
                obj.ID = id;
                if (ret == true)
                {
                    //sTrangThaiNVu = obj.TTHAI_NVU;
                    //lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    //#region Thông tin chung
                    //cmbKhuVuc.SelectedIndex = lstSourceKhuVuc.IndexOf(lstSourceKhuVuc.FirstOrDefault(e => e.KeywordStrings.First().Equals(obj.MA_KVUC)));
                    //cmbCum.SelectedIndex = lstSourceCum.IndexOf(lstSourceKhuVuc.FirstOrDefault(e => e.KeywordStrings.First().Equals(obj.MA_CUM)));
                    //if (LDateTime.IsDate(obj.NGAY_DL, "yyyyMMdd"))
                    //    raddtNgayDangKy.Value = LDateTime.StringToDate(obj.NGAY_DL, "yyyyMMdd");
                    //cmbCBQL.SelectedIndex = lstSourceCBQL.IndexOf(lstSourceCBQL.FirstOrDefault(e => e.KeywordStrings.First().Equals(obj.MA_CBQL)));
                    //cmbHinhThuc.SelectedIndex = lstSourceHinhThucDangKy.IndexOf(lstSourceHinhThucDangKy.FirstOrDefault(e => e.KeywordStrings.First().Equals(obj.HTHUC_DKY)));
                    //txtDienGiai.Text = obj.DIEN_GIAI;
                    //if (LDateTime.IsDate(obj.THANG_TRA, "yyyyMMdd"))
                    //    teldtThangGiaoVon.Value = LDateTime.StringToDate(obj.THANG_TRA, "yyyyMMdd");
                    //cmbDotTra.SelectedIndex = lstSourceGiaoVon.IndexOf(lstSourceGiaoVon.FirstOrDefault(f => f.KeywordStrings[2].Equals(obj.NGAY_TRA)));

                    //#endregion

                    //#region Thông tin kiểm soát
                    //txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    //raddtNgayLap.Value = LDateTime.StringToDate(obj.NGAY_NHAP, "yyyyMMdd");
                    //txtNguoiLap.Text = obj.NGUOI_NHAP;
                    //if (LDateTime.IsDate(obj.NGAY_CNHAT, "yyyyMMdd") == true)
                    //    raddtNgayCapNhat.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, "yyyyMMdd");
                    //txtNguoiCapNhat.Text = obj.NGUOI_CNHAT;
                    //#endregion

                }
                else
                {
                    LMessage.ShowMessage("M_ResponseMessage_HanhDong_LayDuLieuKhongThanhCong", LMessage.MessageBoxType.Warning);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M_ResponseMessage_HanhDong_LayDuLieuKhongThanhCong", LMessage.MessageBoxType.Warning);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void ResetForm()
        {
            sTrangThaiNVu = "";
            lblTrangThai.Content = "";
            id = 0;
            obj = null;

            #region Thông tin chung
            raddtNgayDangKy.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            cmbCBQL.SelectedIndex = 0;
            cmbKhuVuc.SelectedIndex = -1;
            cmbCum.SelectedIndex = -1;
            cmbHinhThuc.SelectedIndex = -1;
            txtDienGiai.Text = "";
            cmbDotTra.SelectedIndex = -1;

            dtThongTinRutGoc.Rows.Clear();
            raddgrThongTin.DataContext = dtThongTinRutGoc.DefaultView;
            #endregion

            #region Thông tin kiểm soát
            txtTrangThaiBanGhi.Text = "";
            raddtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            raddtNgayCapNhat.Value = null;
            txtNguoiCapNhat.Text = "";
            #endregion
            //CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, sTrangThaiNVu, mnuMain, DatabaseConstant.Function.HDV_DANG_KY_RUT_GOC);

        }

        private bool Validation()
        {
            try
            {
                if (cmbKhuVuc.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblKhuVuc.Content.ToString());
                    cmbKhuVuc.Focus();
                    return false;
                }

                else if (cmbCum.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblCum.Content.ToString());
                    cmbCum.Focus();
                    return false;
                }

                else if (cmbHinhThuc.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblHinhThuc.Content.ToString());
                    cmbHinhThuc.Focus();
                    return false;
                }

                else if (cmbDotTra.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblDotTra.Content.ToString());
                    cmbDotTra.Focus();
                    return false;
                }

                else if (dtpThangGiaoVon.Text.IsNullOrEmptyOrSpace() || teldtThangGiaoVon.Text.Equals("__/__/____"))
                {
                    CommonFunction.ThongBaoTrong(lblThangTra.Content.ToString());
                    dtpThangGiaoVon.Focus();
                    return false;
                }

                else if (dtpNgayDangKy.Text.IsNullOrEmptyOrSpace() || raddtNgayDangKy.Text.Equals("__/__/____"))
                {
                    CommonFunction.ThongBaoTrong(lblThangTra.Content.ToString());
                    dtpNgayDangKy.Focus();
                    return false;
                }

                else if (cmbCBQL.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblCBQL.Content.ToString());
                    cmbCBQL.Focus();
                    return false;
                }

                if (dtThongTinRutGoc.Rows.Count <= 0)
                {
                    CommonFunction.ThongBaoChuaNhap(grbThongTinRutGoc.Header.ToString());
                    tlbDetailAdd.Focus();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                return false;
            }
        }

        private void SetEnabledControls()
        {
            #region Thêm
            if (action == DatabaseConstant.Action.THEM)
            {
                tlbAdd.IsEnabled = false;
                tlbModify.IsEnabled = false;
                tlbDelete.IsEnabled = false;
                tlbSave.IsEnabled = true;

                cmbKhuVuc.IsEnabled = true;
                cmbCum.IsEnabled = true;
                cmbHinhThuc.IsEnabled = true;
                cmbDotTra.IsEnabled = true;
                dtpNgayDangKy.IsEnabled = true;
                dtpThangGiaoVon.IsEnabled = true;
                teldtThangGiaoVon.IsEnabled = true;
                raddtNgayDangKy.IsEnabled = true;
                cmbCBQL.IsEnabled = true;
                grbThongTinRutGoc.IsEnabled = true;
                txtDienGiai.IsEnabled = true;
            }
            #endregion

            #region Sửa
            else if (action == DatabaseConstant.Action.SUA)
            {
                tlbAdd.IsEnabled = false;
                tlbModify.IsEnabled = false;
                tlbDelete.IsEnabled = true;
                tlbSave.IsEnabled = true;

                cmbKhuVuc.IsEnabled = true;
                cmbCum.IsEnabled = true;
                cmbHinhThuc.IsEnabled = true;
                cmbDotTra.IsEnabled = true;
                dtpNgayDangKy.IsEnabled = true;
                dtpThangGiaoVon.IsEnabled = true;
                teldtThangGiaoVon.IsEnabled = true;
                raddtNgayDangKy.IsEnabled = true;
                cmbCBQL.IsEnabled = true;
                grbThongTinRutGoc.IsEnabled = true;
                txtDienGiai.IsEnabled = true;
            }
            #endregion

            #region Xem ~ Mặc định
            else
            {
                tlbAdd.IsEnabled = true;
                tlbModify.IsEnabled = true;
                tlbDelete.IsEnabled = true;
                tlbSave.IsEnabled = false;
                cmbKhuVuc.IsEnabled = false;
                cmbCum.IsEnabled = false;
                cmbHinhThuc.IsEnabled = false;
                cmbDotTra.IsEnabled = false;
                dtpNgayDangKy.IsEnabled = false;
                dtpThangGiaoVon.IsEnabled = false;
                teldtThangGiaoVon.IsEnabled = false;
                raddtNgayDangKy.IsEnabled = false;
                cmbCBQL.IsEnabled = false;
                grbThongTinRutGoc.IsEnabled = false;
                txtDienGiai.IsEnabled = false;
            }
            #endregion
        }


        public void OnSave()
        {
            try
            {
                if (!Validation()) return;

                obj = new HDV_THONG_TIN_DK_RUT_GOC();
                GetFormData(ref obj);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(obj);
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(obj);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void BeforeViewFromDetail()
        {
            action = DatabaseConstant.Action.XEM;
            SetEnabledControls();
            //CommonFunction.RefreshButton(Toolbar, action, BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), mnuMain, DatabaseConstant.Function.HDV_THONG_TIN_DK_RUT_GOC_PL);
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


        public void BeforeAddNew()
        {
            ResetForm();
            action = DatabaseConstant.Action.THEM;
            SetEnabledControls();
            cmbKhuVuc.Focus();
        }

        public void OnAddNew(HDV_THONG_TIN_DK_RUT_GOC objTT)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processHDV.DangKyRutGoc(DatabaseConstant.Action.THEM, ref objTT, ref listClientResponseDetail);
                obj = objTT;
                AfterAddNew(ret, obj, listClientResponseDetail);
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

        public void AfterAddNew(bool ret, HDV_THONG_TIN_DK_RUT_GOC obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);

                    if (chkThemNhieuLan.IsChecked == true)
                    {
                        BeforeAddNew();
                    }
                    else
                    {
                        id = obj.ID;
                        sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);

                        BeforeViewFromDetail();
                    }
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        public void BeforeModifyFromDetail()
        {
            try
            {
                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);

                bool ret = process.LockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_DANG_KY_RUT_GOC,
                    DatabaseConstant.Table.VKT_HDV_DKY_RGOC,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                if (ret)
                {
                    action = DatabaseConstant.Action.SUA;
                    SetEnabledControls();
                }
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                }
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
                SetFormData();
                SetEnabledControls();
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnModify(HDV_THONG_TIN_DK_RUT_GOC objTT)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processHDV.DangKyRutGoc(DatabaseConstant.Action.SUA, ref objTT, ref listClientResponseDetail);
                obj = objTT;
                AfterModify(ret, obj, listClientResponseDetail);
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

        public void AfterModify(bool ret, HDV_THONG_TIN_DK_RUT_GOC obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    raddtNgayCapNhat.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                    txtNguoiCapNhat.Text = ClientInformation.TenDangNhap;

                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }

                // Yêu cầu Unlock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_DANG_KY_RUT_GOC,
                    DatabaseConstant.Table.VKT_HDV_DKY_RGOC,
                    DatabaseConstant.Action.SUA,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        public void BeforeDelete()
        {
            try
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        DatabaseConstant.Function.HDV_DANG_KY_RUT_GOC,
                        DatabaseConstant.Table.VKT_HDV_DKY_RGOC,
                        DatabaseConstant.Action.XOA,
                        listLockId);

                    if (retLockData)
                    {
                        action = DatabaseConstant.Action.XOA;
                        OnDelete();
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.HDV_DANG_KY_RUT_GOC,
                    DatabaseConstant.Table.VKT_HDV_DKY_RGOC,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnDelete()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                bool ret = false;
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                ret = processHDV.DangKyRutGoc(action, ref obj, ref listClientResponseDetail);
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
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    //LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_DANG_KY_RUT_GOC,
                    DatabaseConstant.Table.VKT_HDV_DKY_RGOC,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                // Đóng cửa sổ chi tiết sau khi xóa
                OnClose();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        #endregion


    }
}
