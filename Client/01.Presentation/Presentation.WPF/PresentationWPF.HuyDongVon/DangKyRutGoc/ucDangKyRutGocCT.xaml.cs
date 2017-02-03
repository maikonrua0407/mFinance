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
    /// Interaction logic for ucDangKyRutGocCT.xaml
    /// </summary>
    public partial class ucDangKyRutGocCT : UserControl
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

        private HDV_THONG_TIN_DKY_RUT_GOC obj;
        public HDV_THONG_TIN_DKY_RUT_GOC Obj
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
        public ucDangKyRutGocCT()
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
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.HuyDongVon;component/DangKyRutGoc/ucDangKyRutGocCT.xaml", ref Toolbar, ref mnuMain);
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
            dtThongTinRutGoc.Columns.Add("MA_NHOM_SP", typeof(string));
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
                popupProcess.getPopupInformation("POPUP_DS_KHANG_HANG_DKY_RUT_TKIEM_QB", lstDieuKien);
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
                        dradd["MA_NHOM_SP"] = dr["MA_NHOM_SP"];
                        dradd["NGAY_MO_SO"] = dr["NGAY_MO_SO"];
                        dradd["SO_TIEN"] = dr["SO_TIEN"];
                        dradd["SO_TIEN_RUT"] = 0;
                        dradd["SO_DU"] = dradd["SO_TIEN"];
                        if (cmbHinhThuc.SelectedIndex == 1)
                        {
                            dradd["SO_TIEN_RUT"] = dr["SO_TIEN"];
                            dradd["SO_DU"] = 0;
                            SO_TIEN_RUT.IsReadOnly = true;
                        }
                        dradd["ID_TIEN_GUI"] = dr["ID_TIEN_GUI"];
                        idTienGui = Convert.ToInt32(dr["ID_TIEN_GUI"]);
                        chuKy = Convert.ToInt32(dr["CHU_KY_VAY"]);
                        dradd["CHU_KY_VAY"] = chuKy;
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

            dtThongTinRutGoc.Rows.Clear();
            grbThongTinRutGoc.DataContext = dtThongTinRutGoc.DefaultView;
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

            dtThongTinRutGoc.Rows.Clear();
            grbThongTinRutGoc.DataContext = dtThongTinRutGoc.DefaultView;
        }

        #endregion

        #region Xử lý nghiệp vụ
        private void GetFormData(ref HDV_THONG_TIN_DKY_RUT_GOC obj, string sTrangThaiNVu)
        {
            try
            {
                obj = new HDV_THONG_TIN_DKY_RUT_GOC();

                #region BL_DKY_RGOC
                BL_DKY_RGOC objDkyRGoc = new BL_DKY_RGOC();
                AutoCompleteEntry auNgayGiaoVon = lstSourceGiaoVon.ElementAt(cmbDotTra.SelectedIndex);
                objDkyRGoc.ID = id;
                objDkyRGoc.MA_DKY = txtMaGiaoDich.Text;
                objDkyRGoc.ID_KVUC = Convert.ToInt32(lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings.ElementAt(1));
                objDkyRGoc.ID_CUM = Convert.ToInt32(lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings.ElementAt(1));
                objDkyRGoc.HTHUC_DKY = lstSourceHinhThucDangKy.ElementAt(cmbHinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                objDkyRGoc.MA_CBQL = lstSourceCBQL.ElementAt(cmbCBQL.SelectedIndex).KeywordStrings.First();
                objDkyRGoc.NGAY_TRA = auNgayGiaoVon.KeywordStrings[2];
                if (teldtThangGiaoVon.Value != null)
                    objDkyRGoc.THANG_TRA = Convert.ToDateTime(teldtThangGiaoVon.Value).ToString("yyyyMMdd");
                objDkyRGoc.NGAY_DKY = Convert.ToDateTime(raddtNgayDangKy.Value).ToString("yyyyMMdd");
                objDkyRGoc.DIEN_GIAI = txtDienGiai.Text;

                //Thông tin kiểm soát
                objDkyRGoc.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                objDkyRGoc.TTHAI_NVU = sTrangThaiNVu;
                objDkyRGoc.MA_DVI_QLY = ClientInformation.MaDonVi;
                objDkyRGoc.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                objDkyRGoc.NGAY_NHAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                objDkyRGoc.NGUOI_NHAP = txtNguoiLap.Text;
                if (action != DatabaseConstant.Action.THEM)
                {
                    objDkyRGoc.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    objDkyRGoc.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }

                obj.OBJ_DKY_RGOC = objDkyRGoc;
                #endregion

                #region BL_DKY_RGOC_CT
                List<BL_DKY_RGOC_CT> lstDkyRGocCT = new List<BL_DKY_RGOC_CT>();
                if (dtThongTinRutGoc.Rows.Count > 0)
                {
                    BL_DKY_RGOC_CT objDkyRGocCT = null;
                    foreach (DataRow dr in dtThongTinRutGoc.Rows)
                    {
                        objDkyRGocCT = new BL_DKY_RGOC_CT();

                        objDkyRGocCT.MA_KHANG = dr["MA_KHANG"].ToString();
                        objDkyRGocCT.TEN_KHANG = dr["TEN_KHANG"].ToString();
                        objDkyRGocCT.NGAY_MO_SO = dr["NGAY_MO_SO"].ToString();
                        objDkyRGocCT.SO_SO_TG = dr["SO_SO_TG"].ToString();
                        objDkyRGocCT.MA_NHOM_SP = dr["MA_NHOM_SP"].ToString();
                        objDkyRGocCT.SO_TIEN_GOC = Convert.ToDecimal(dr["SO_TIEN"]);
                        objDkyRGocCT.SO_TIEN_RUT_GOC = Convert.ToDecimal(dr["SO_TIEN_RUT"]);
                        objDkyRGocCT.SO_DU = Convert.ToDecimal(dr["SO_DU"]);
                        objDkyRGocCT.CHU_KY_VAY = Convert.ToInt32(dr["CHU_KY_VAY"]);
                        objDkyRGocCT.ID_TIEN_GUI = Convert.ToInt32(dr["ID_TIEN_GUI"]);

                        objDkyRGocCT.MA_DKY = objDkyRGoc.MA_DKY;
                        objDkyRGocCT.HTHUC_DKY = objDkyRGoc.HTHUC_DKY;
                        objDkyRGocCT.TTHAI_BGHI = objDkyRGoc.TTHAI_BGHI;
                        objDkyRGocCT.TTHAI_NVU = objDkyRGoc.TTHAI_NVU;
                        objDkyRGocCT.MA_DVI_QLY = objDkyRGoc.MA_DVI_QLY;
                        objDkyRGocCT.MA_DVI_TAO = objDkyRGoc.MA_DVI_TAO;
                        objDkyRGocCT.NGAY_NHAP = objDkyRGoc.NGAY_NHAP;
                        objDkyRGocCT.NGUOI_NHAP = objDkyRGoc.NGUOI_NHAP;
                        objDkyRGocCT.NGAY_CNHAT = objDkyRGoc.NGAY_NHAP;
                        objDkyRGocCT.NGUOI_CNHAT = objDkyRGoc.NGUOI_CNHAT;

                        lstDkyRGocCT.Add(objDkyRGocCT);
                    }
                }

                obj.LST_DKY_RGOC_CT = lstDkyRGocCT.ToArray();
                #endregion

                obj.ID = id;
                obj.MA_DVI = ClientInformation.MaDonVi;
                obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.TTHAI_NVU = sTrangThaiNVu;
                obj.NGAY_NHAP = objDkyRGoc.NGAY_NHAP;
                obj.NGUOI_NHAP = objDkyRGoc.NGUOI_NHAP;
                obj.NGUOI_CNHAT = objDkyRGoc.NGUOI_CNHAT;
                obj.NGAY_CNHAT = objDkyRGoc.NGAY_CNHAT;

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            HuyDongVonProcess processHuyDongVon = new HuyDongVonProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            obj = new HDV_THONG_TIN_DKY_RUT_GOC();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processHuyDongVon.DangKyRutGocQB(DatabaseConstant.Action.LOAD, ref obj, ref listClientResponseDetail);
                if (ret == true)
                {
                    sTrangThaiNVu = obj.OBJ_DKY_RGOC.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    #region Thông tin chung

                    txtMaGiaoDich.Text = obj.OBJ_DKY_RGOC.MA_DKY;
                    cmbKhuVuc.SelectedIndex = lstSourceKhuVuc.IndexOf(lstSourceKhuVuc.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.OBJ_DKY_RGOC.ID_KVUC.ToString())));
                    cmbCum.SelectedIndex = lstSourceCum.IndexOf(lstSourceCum.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.OBJ_DKY_RGOC.ID_CUM.ToString())));
                    cmbHinhThuc.SelectedIndex = lstSourceHinhThucDangKy.IndexOf(lstSourceHinhThucDangKy.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.OBJ_DKY_RGOC.HTHUC_DKY)));
                    cmbCBQL.SelectedIndex = lstSourceCBQL.IndexOf(lstSourceCBQL.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.OBJ_DKY_RGOC.MA_CBQL)));
                    LoadComboBoxDotPhatVon();
                    cmbDotTra.SelectedIndex = lstSourceGiaoVon.IndexOf(lstSourceGiaoVon.FirstOrDefault(f => f.KeywordStrings[2].Equals(obj.OBJ_DKY_RGOC.NGAY_TRA)));
                    txtDienGiai.Text=obj.OBJ_DKY_RGOC.DIEN_GIAI;
                    
                    #endregion

                    #region Thong tin rut goc
                   
                    foreach (var item in obj.LST_DKY_RGOC_CT)
                    {
                        DataRow dr = dtThongTinRutGoc.NewRow();
                        dr["ID_TIEN_GUI"] = item.ID_TIEN_GUI;
                        dr["MA_KHANG"] = item.MA_KHANG;
                        dr["TEN_KHANG"] = item.TEN_KHANG;
                        dr["NGAY_MO_SO"] = item.NGAY_MO_SO;
                        dr["SO_SO_TG"] = item.SO_SO_TG;
                        dr["MA_NHOM_SP"] = item.MA_NHOM_SP;
                        dr["SO_TIEN"] = item.SO_TIEN_GOC;
                        dr["SO_TIEN_RUT"] = item.SO_TIEN_RUT_GOC;
                        dr["SO_DU"] = item.SO_DU;
                        dr["CHU_KY_VAY"] = item.CHU_KY_VAY;
                        
                        dtThongTinRutGoc.Rows.Add(dr);
                    }

                    for (int i = 0; i < dtThongTinRutGoc.Rows.Count; i++)
                    {
                        dtThongTinRutGoc.Rows[i]["STT"] = i + 1;
                    }

                    raddgrThongTin.ItemsSource = dtThongTinRutGoc.DefaultView;
                    #endregion

                    #region Thông tin kiểm soát
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.OBJ_DKY_RGOC.TTHAI_BGHI);
                    raddtNgayLap.Value = LDateTime.StringToDate(obj.OBJ_DKY_RGOC.NGAY_NHAP, "yyyyMMdd");
                    txtNguoiLap.Text = obj.OBJ_DKY_RGOC.NGUOI_NHAP;
                    if (LDateTime.IsDate(obj.OBJ_DKY_RGOC.NGAY_CNHAT, "yyyyMMdd") == true)
                        raddtNgayCapNhat.Value = LDateTime.StringToDate(obj.OBJ_DKY_RGOC.NGAY_CNHAT, "yyyyMMdd");
                    txtNguoiCapNhat.Text = obj.OBJ_DKY_RGOC.NGUOI_CNHAT;
                    #endregion

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
            txtMaGiaoDich.Text = "";
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
                //tlbAdd.IsEnabled = false;
                //tlbModify.IsEnabled = false;
                //tlbDelete.IsEnabled = false;
                //tlbSave.IsEnabled = true;

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
                //tlbAdd.IsEnabled = false;
                //tlbModify.IsEnabled = false;
                //tlbDelete.IsEnabled = true;
                //tlbSave.IsEnabled = true;

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
                //tlbAdd.IsEnabled = true;
                //tlbModify.IsEnabled = true;
                //tlbDelete.IsEnabled = true;
                //tlbSave.IsEnabled = false;
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


        public void OnHold()
        {
            try
            {
                if (!Validation()) return;

                string trangThai = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();

                obj = new HDV_THONG_TIN_DKY_RUT_GOC();

                GetFormData(ref obj, trangThai);

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

        public void OnSave()
        {
            try
            {
                if (!Validation()) return;

                string trangThai = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();

                obj = new HDV_THONG_TIN_DKY_RUT_GOC();

                GetFormData(ref obj, trangThai);

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
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void BeforeViewFromList()
        {
            SetFormData();
            BeforeViewFromDetail();
        }


        public void BeforeAddNew()
        {
            action = DatabaseConstant.Action.THEM;
            ResetForm();
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnAddNew(HDV_THONG_TIN_DKY_RUT_GOC obj)
        {

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HuyDongVonProcess processHuyDongVon = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processHuyDongVon.DangKyRutGocQB(DatabaseConstant.Action.THEM, ref obj, ref listClientResponseDetail);
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

        public void AfterAddNew(bool ret, HDV_THONG_TIN_DKY_RUT_GOC obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                    id = obj.ID;
                    txtMaGiaoDich.Text = obj.OBJ_DKY_RGOC.MA_DKY;

                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);

                    BeforeViewFromDetail();
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
                    DatabaseConstant.Function.HDV_DANG_KY_RUT_GOC_CT,
                    DatabaseConstant.Table.BL_DKY_RGOC,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                if (ret)
                {
                    action = DatabaseConstant.Action.SUA;
                    SetEnabledControls();
                    CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
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
            SetFormData();
            action = DatabaseConstant.Action.SUA;
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnModify(HDV_THONG_TIN_DKY_RUT_GOC obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HuyDongVonProcess processHuyDongVon = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processHuyDongVon.DangKyRutGocQB(DatabaseConstant.Action.SUA, ref obj, ref listClientResponseDetail);
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

        public void AfterModify(bool ret, HDV_THONG_TIN_DKY_RUT_GOC obj, List<ClientResponseDetail> listClientResponseDetail)
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
                    //LMessage.ShowMessage("M.DungChung.CapNhatKoThanhCong", LMessage.MessageBoxType.Error);
                }

                // Yêu cầu Unlock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_DANG_KY_RUT_GOC_CT,
                    DatabaseConstant.Table.BL_DKY_RGOC,
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
                        DatabaseConstant.Function.HDV_DANG_KY_RUT_GOC_CT,
                        DatabaseConstant.Table.BL_DKY_RGOC,
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_DANG_KY_RUT_GOC_CT,
                    DatabaseConstant.Table.BL_DKY_RGOC,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnDelete()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            HuyDongVonProcess processHuyDongVon = new HuyDongVonProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processHuyDongVon.DangKyRutGocQB(action, ref obj, ref listClientResponseDetail);
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
                listClientResponseDetail = null;
                processHuyDongVon = null;
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
                    DatabaseConstant.Function.HDV_DANG_KY_RUT_GOC_CT,
                    DatabaseConstant.Table.BL_DKY_RGOC,
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
