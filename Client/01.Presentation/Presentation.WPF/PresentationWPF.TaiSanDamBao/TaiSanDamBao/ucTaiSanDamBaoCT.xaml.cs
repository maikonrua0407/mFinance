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
using Presentation.Process.TaiSanDamBaoServiceRef;
using Telerik.Windows.Controls.GridView;
using PresentationWPF.CustomControl;
using PresentationWPF.HuyDongVon.Popup;

namespace PresentationWPF.TaiSanDamBao.TaiSanDamBao
{
    /// <summary>
    /// Interaction logic for ucTaiSanDamBaoCT.xaml
    /// </summary>
    public partial class ucTaiSanDamBaoCT : UserControl
    {
        #region Khai bao
        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private DatabaseConstant.Function function = DatabaseConstant.Function.BAO_CAO;

        public event EventHandler OnSavingCompleted;

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int idHMCTietTSDB = 0;
        private int idHMCTiet = 0;
        private int idLoaiTS = 0;
        private int idKhachHang = 0;

        private TD_TAI_SAN_DAM_BAO obj;
        public TD_TAI_SAN_DAM_BAO Obj
        {
            get { return obj; }
            set { obj = value; }
        }

        private DataSet ds = null;

        private DataTable dtHMTSDB = null;
        private DataSet dsKHangLoaiTSDB = new DataSet();
        private DataSet dsLoaiTSDB = new DataSet();

        private string sTrangThaiNVu = "";

        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNguonVon = new List<AutoCompleteEntry>();

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        private List<HM_CTIET_TSDB> lstHMCTietTSDB = new List<HM_CTIET_TSDB>();
        
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
        public ucTaiSanDamBaoCT()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            InitEventHandler();

            LoadCombobox();

            KhoiTaoDataTable();

            txtMaKH.Focus();           
        }        

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TaiSanDamBao;component/HopDong/ucHopDongLaoDongCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void InitEventHandler()
        {
            //chkThemNhieuLan.Checked += new RoutedEventHandler(chkThemNhieuLan_Checked);  
            txtGiaTriDinhGia.LostFocus += new RoutedEventHandler(numTSDBGiaTriDinhGia_LostFocus);
            raddgrDSachHanMuc.CellEditEnded += new EventHandler<GridViewCellEditEndedEventArgs>(grDSachHanMuc_CellEditEnded);
                
        }

        private void KhoiTaoDataTable()
        {
            dtHMTSDB = new DataTable();
            dtHMTSDB.Columns.Add("STT", typeof(int));
            dtHMTSDB.Columns.Add("ID_HMUC_CTIET", typeof(int));
            dtHMTSDB.Columns.Add("MA_HMUC_CTIET", typeof(string));
            dtHMTSDB.Columns.Add("MA_KHANG", typeof(string));
            dtHMTSDB.Columns.Add("TEN_KHANG", typeof(string));
            dtHMTSDB.Columns.Add("TY_LE_DB", typeof(decimal));
            dtHMTSDB.Columns.Add("GTRI_DB", typeof(decimal));

        }

        private void LoadCombobox()
        {
            COMBOBOX_DTO combo = null;
            List<string> lstDieuKien = null;
            List<COMBOBOX_DTO> lstCombobox = new List<COMBOBOX_DTO>();
            AutoComboBox auCombo = new AutoComboBox();

            //Loại Tiền
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_LOAITIEN.getValue();
            combo.combobox = cmbLoaiTien;
            combo.lstSource = lstSourceLoaiTien;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Nguồn vốn
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_NGUON_VON.getValue();
            combo.combobox = cmbNguonVon;
            combo.lstSource = lstSourceNguonVon;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Gen combobox
            auCombo.GenAutoComboBoxTheoList(ref lstCombobox);

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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
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
            OnHold();
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
            BeforeApprove();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeRefuse();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeCancel();
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
                OnHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
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
                OnHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
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
            Release();
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

        private void btn_MaKH_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> lstDieuKien = new List<string>();
                string lstIDKH = "";
                if (!txtMaKH.Text.IsNullOrEmptyOrSpace())
                    lstIDKH = "(" + idKhachHang.ToString() + ")";
                else
                    lstIDKH = "(0)";
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add(lstIDKH);
                lstDieuKien.Add("NULL");
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_KHACHHANG_TDVM", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(true, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup.Count > 0)
                {
                    DataRow dr = lstPopup[0];
                    idKhachHang = Convert.ToInt32(dr["ID"]);
                    txtMaKH.Text = dr["MA_KHANG"].ToString();
                    lblTenKH.Content = dr["TEN_KHANG"].ToString();
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

        private void btn_MaNhomTSDB_Click(object sender, RoutedEventArgs e)
        {
            //list điều kiện theo từng loại sổ
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.MaDonVi);

            var process = new PopupProcess();
            process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_NHOM_TSDB.getValue(), lstDieuKien);

            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

            ucPopup popup = new ucPopup(false, simplePopupResponse, false);
            lstPopup.Clear();
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.Content = popup;
            Mouse.OverrideCursor = Cursors.Arrow;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ShowDialog();
            if (lstPopup != null && lstPopup.Count > 0)
            {
                DataRow row = lstPopup[0];
                idLoaiTS = Convert.ToInt32(row["ID"].ToString());
                txtMaNhomTSDB.Text = row["MA_LOAI_TSDB"].ToString();
                lblTenNhomTSDB.Content = row["TEN_LOAI_TSDB"].ToString();

            }
        }

        private void btn_ChuTaiSan_Click(object sender, RoutedEventArgs e)
        {
            lstPopup.Clear();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
            lstDieuKien.Add("NULL");
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation("POPUP_DS_KHACHHANG", lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            ucPopup popup = new ucPopup(true, simplePopupResponse);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
            win.Content = popup;
            win.ShowDialog();
            if (lstPopup.Count > 0)
            {
                txtChuTaiSan.Text = lstPopup[0][2].ToString();
                txtTenChuTaiSan.Text = lstPopup[0][3].ToString();
            }
        }

        private void btn_SoTKThamChieu_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                lstPopup.Clear();
                Window window = new Window();
                ucPopupSoTGui uc = new ucPopupSoTGui();
                uc.Function = DatabaseConstant.Function.HDV_TAT_TOAN;
                uc.DuLieuTraVe = new ucPopupSoTGui.LayDuLieu(LayDuLieuTuPopup);
                window.Title = "Danh sách sổ";
                window.Content = uc;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    HuyDongVonProcess processHDV = new HuyDongVonProcess();
                    DataRow row = lstPopup[0];
                    int idSoTGui = Convert.ToInt32(row[1]);
                    DataSet ds = processHDV.GetThongTinQTrongSoTGui(idSoTGui);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];

                        #region Hiển thị thông tin sổ
                        txtSoTKThamChieu.Text = dr["SO_SO_TG"].ToString();
                        txtSoDu.Value = Convert.ToDouble(dr["SO_TIEN"]);

                        if (LDateTime.IsDate(dr["NGAY_DEN_HAN"].ToString(), "yyyyMMdd"))
                            raddtNgayDH.Value = LDateTime.StringToDate(dr["NGAY_DEN_HAN"].ToString(), "yyyyMMdd");
                        else
                            raddtNgayDH.Value = null;
                        #endregion
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

        private void txt_MaKH_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btn_MaKH_Click(null, null);
            }
        }

        private void txt_MaNhomTSDB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btn_MaNhomTSDB_Click(null, null);
            }
        }

        private void txt_ChuTaiSan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btn_ChuTaiSan_Click(null, null);
            }
        }

        private void txt_SoTKThamChieu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btn_SoTKThamChieu_Click(null, null);
            }
        }

        private void btnThemHM_Click(object sender, RoutedEventArgs e)
        {
            DataRow dr = dtHMTSDB.NewRow();

            //dr["MA_HMUC"] = null;
            //dr["MA_KHANG"] = null;
            //dr["TENKHANG"] = null;
            dr["TY_LE_DB"] = 0;
            dr["GTRI_DB"] = 0;
            dtHMTSDB.Rows.Add(dr);

            for (int i = 0; i < dtHMTSDB.Rows.Count; i++)
            {
                dtHMTSDB.Rows[i]["STT"] = i + 1;
            }

            raddgrDSachHanMuc.DataContext = dtHMTSDB.DefaultView;
        }

        private void btnXoaHM_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<int> lstSTT = new List<int>();
                for (int i = 0; i < raddgrDSachHanMuc.SelectedItems.Count; i++)
                {
                    DataRowView dr = (DataRowView)raddgrDSachHanMuc.SelectedItems[i];
                    lstSTT.Add(Convert.ToInt32(dr["STT"]));
                }
                lstSTT.SortByDesc();
                foreach (int stt in lstSTT)
                    dtHMTSDB.Rows.RemoveAt(stt - 1);

                for (int i = 0; i < dtHMTSDB.Rows.Count; i++)
                {
                    dtHMTSDB.Rows[i]["STT"] = i + 1;
                }

                raddgrDSachHanMuc.DataContext = dtHMTSDB.DefaultView;
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void btn_MaHanMuc_Click(object sender, RoutedEventArgs e)
        {
            if (txtMaNhomTSDB.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaChon(lblMaNhomTSDB.Content.ToString());
                txtMaNhomTSDB.Focus();
                return;
            }
            //list điều kiện
            DataRowView drv = (DataRowView)raddgrDSachHanMuc.CurrentItem;

            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(txtMaNhomTSDB.Text);

            var process = new PopupProcess();
            process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_HM_NHOM_TSDB.getValue(), lstDieuKien);

            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

            ucPopup popup = new ucPopup(false, simplePopupResponse, false);
            lstPopup.Clear();
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.Content = popup;
            Mouse.OverrideCursor = Cursors.Arrow;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ShowDialog();
            if (lstPopup != null && lstPopup.Count > 0)
            {
                DataRow row = lstPopup[0];

                drv["ID_HMUC_CTIET"] = row["ID_HMUC_CTIET"].ToString();
                drv["MA_HMUC_CTIET"] = row["MA_HMUC_CTIET"].ToString();
                drv["MA_KHANG"] = row["MA_KHANG"].ToString();
                drv["TEN_KHANG"] = row["TEN_KHANG"].ToString();
            }
        }

        private void txt_MaHanMuc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btn_MaHanMuc_Click(null, null);
            }
        }

        private void numTSDBGiaTriDinhGia_LostFocus(object sender, RoutedEventArgs e)
        {
            TaiSanDamBaoProcess processTaiSanDamBao = new TaiSanDamBaoProcess();
            string _mA_Loai_TS = txtMaNhomTSDB.Text;
            decimal tyleDBTD = 0;
            if (_mA_Loai_TS.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaChon(lblMaNhomTSDB.Content.ToString());
                txtMaNhomTSDB.Focus();
            }
            else
            {
                dsLoaiTSDB = processTaiSanDamBao.getThongTinLoaiTSDB(_mA_Loai_TS);
                if (dsLoaiTSDB != null && dsLoaiTSDB.Tables.Count > 0)
                {
                    DataRow dr = dsLoaiTSDB.Tables[0].Rows[0];
                    if (dr["ID"] != null && !LString.IsNullOrEmptyOrSpace(dr["ID"].ToString()))
                    {
                        tyleDBTD = Convert.ToDecimal(dr["TY_LE_VAY_TOI_DA"]);
                    }
                }

                txtGiaTriDBToiDa.Value = txtGiaTriDinhGia.Value * Convert.ToDouble(tyleDBTD) / 100;
                txtGiaTriHachToan.Value = txtGiaTriDinhGia.Value;
            }
        }

        private void grDSachHanMuc_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            string column = e.Cell.Column.Name;

            if (column.Equals("TY_LE_DB"))
            {
                DataRowView dr = (DataRowView)raddgrDSachHanMuc.CurrentCellInfo.Item;

                int tyleDB = Convert.ToInt32(dr["TY_LE_DB"]);

                dr["GTRI_DB"] = tyleDB * txtGiaTriDBToiDa.Value / 100;
                raddgrDSachHanMuc.CurrentItem = dr;
            }

        }

        /// <summary>
        /// Hàm xử lý giải phóng lock dữ liệu cho form
        /// </summary>
        private void Release()
        {
            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool ret = process.UnlockData(DatabaseConstant.Module.TSDB,
                DatabaseConstant.Function.TD_TSDB_CT,
                DatabaseConstant.Table.TD_TSDB,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        private void ResetData()
        {
            action = DatabaseConstant.Action.THEM;
            obj = null;
            id = 0;
            idKhachHang = 0;
            sTrangThaiNVu = "";

            ResetForm();
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
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
            Release();
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        private void chkThemNhieuLan_Checked(object sender, RoutedEventArgs e)
        {
            //action = DatabaseConstant.Action.THEM;
            //id = 0;
            //idHoSo = 0;
            //idNguoiDaiDien = 0;
            //obj = null;
            //loaiThoiHan = BusinessConstant.LOAI_THOI_HAN_HDLD.CO_THOI_HAN.layGiaTri();
            //sTrangThaiNVu = "";

            //ResetForm();
            //SetEnabledControls();
            //CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
            //txtTenNhanVien.Focus();

            //chkThemNhieuLan.IsChecked = false;
        }
      
        #endregion               

        #region Xử lý nghiệp vụ
        private void GetFormData(ref TD_TAI_SAN_DAM_BAO obj, string sTrangThaiNVu)
        {
            try
            {
                obj = new TD_TAI_SAN_DAM_BAO();

                #region TD_TSDB
                TD_TSDB objTSDB = new TD_TSDB();
                objTSDB.ID = id;
                objTSDB.SO_HDTC = txtSoHDTC.Text;
                objTSDB.MA_TSDB = txtMaTSDB.Text;
                objTSDB.ID_KHANG = idKhachHang;
                objTSDB.MA_KHANG = txtMaKH.Text;
                objTSDB.ID_LOAI_TSDB = idLoaiTS;
                objTSDB.MA_LOAI_TSDB = txtMaNhomTSDB.Text;
                objTSDB.TEN_TSDB = txtTenTSDB.Text;
                objTSDB.MO_TA = txtMoTa.Text;
                objTSDB.MA_CHU_TSAN = txtChuTaiSan.Text;
                objTSDB.TEN_CHU_TSAN = txtTenChuTaiSan.Text;
                objTSDB.SO_TKIEM_TC = txtSoTKThamChieu.Text;
                objTSDB.SO_TKIEM_TC_SODU = txtSoDu.Value != null ? (decimal)txtSoDu.Value : 0;
                objTSDB.MA_LOAI_TIEN = lstSourceLoaiTien[cmbLoaiTien.SelectedIndex].KeywordStrings[0];
                objTSDB.NV_LOAI_NVON = lstSourceNguonVon[cmbNguonVon.SelectedIndex].KeywordStrings[0];
                objTSDB.GTRI_DGIA = txtGiaTriDinhGia.Value != null ? (decimal)txtGiaTriDinhGia.Value : 0;
                //objTSDB.GTRI_DBAO_TOI_DA = txtGiaTriDBToiDa.Value != null ? (decimal)txtGiaTriDBToiDa.Value : 0;
                objTSDB.GTRI_DBAO_TOI_DA = Convert.ToDecimal(txtGiaTriDinhGia.Value);
                objTSDB.GTRI_HTOAN = txtGiaTriHachToan.Value != null ? (decimal)txtGiaTriHachToan.Value : 0;

                if (teldtNgayHieuLuc.Value != null)
                    objTSDB.NGAY_HLUC = Convert.ToDateTime(teldtNgayHieuLuc.Value).ToString("yyyyMMdd");
                if (teldtNgayHetHieuLuc.Value != null)
                    objTSDB.NGAY_HET_HLUC = Convert.ToDateTime(teldtNgayHetHieuLuc.Value).ToString("yyyyMMdd");

                //Thông tin kiểm soát
                objTSDB.TTHAI_TSAN = BusinessConstant.TTHAI_TSAN.CHUA_HTOAN.layGiaTri();
                objTSDB.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                objTSDB.TTHAI_NVU = sTrangThaiNVu;
                objTSDB.MA_DVI_QLY = ClientInformation.MaDonVi;
                objTSDB.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                objTSDB.NGAY_NHAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                objTSDB.NGUOI_NHAP = txtNguoiLap.Text;
                if (action != DatabaseConstant.Action.THEM)
                {
                    objTSDB.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    objTSDB.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }

                obj.OBJ_TD_TSDB = objTSDB;
                #endregion

                #region HM_CTIET_TSDB
                List<HM_CTIET_TSDB> lstHMCTietTSDB = new List<HM_CTIET_TSDB>();
                HM_CTIET_TSDB objHMCTietTSDB = null;
                foreach (DataRow dr in dtHMTSDB.Rows)
                {
                    objHMCTietTSDB = new HM_CTIET_TSDB();

                    objHMCTietTSDB.ID_TSDB = objTSDB.ID;
                    objHMCTietTSDB.ID_LOAI_TSDB = objTSDB.ID_LOAI_TSDB;
                    objHMCTietTSDB.MA_LOAI_TSDB = objTSDB.MA_LOAI_TSDB;
                    objHMCTietTSDB.MA_TSDB = objTSDB.MA_TSDB;
                    objHMCTietTSDB.TTHAI_BGHI = objTSDB.TTHAI_BGHI;
                    objHMCTietTSDB.TTHAI_NVU = objTSDB.TTHAI_NVU;
                    objHMCTietTSDB.MA_DVI_QLY = objTSDB.MA_DVI_QLY;
                    objHMCTietTSDB.MA_DVI_TAO = objTSDB.MA_DVI_TAO;
                    objHMCTietTSDB.NGAY_NHAP = objTSDB.NGAY_NHAP;
                    objHMCTietTSDB.NGUOI_NHAP = objTSDB.NGUOI_NHAP;
                    objHMCTietTSDB.NGAY_CNHAT = objTSDB.NGAY_NHAP;
                    objHMCTietTSDB.NGUOI_CNHAT = objTSDB.NGUOI_CNHAT;
                    objHMCTietTSDB.ID_HMUC_CTIET = Convert.ToInt32(dr["ID_HMUC_CTIET"]);
                    objHMCTietTSDB.TY_LE_DB = Convert.ToDecimal(dr["TY_LE_DB"]);
                    objHMCTietTSDB.GTRI_DB = Convert.ToDecimal(dr["GTRI_DB"]);
                    objHMCTietTSDB.MA_HMUC_CTIET = Convert.ToString(dr["MA_HMUC_CTIET"]);

                    lstHMCTietTSDB.Add(objHMCTietTSDB);
                }

                obj.LST_HM_CTIET_TSDB = lstHMCTietTSDB.ToArray();
                #endregion

                obj.ID = id;
                obj.ID_LOAI_TSDB = idLoaiTS;
                obj.ID_KHACH_HANG = idKhachHang;
                obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                obj.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.TTHAI_NVU = sTrangThaiNVu;
                obj.NGAY_NHAP = objTSDB.NGAY_NHAP;
                obj.NGUOI_NHAP = objTSDB.NGUOI_NHAP;
                obj.NGUOI_CNHAT = objTSDB.NGUOI_CNHAT;
                obj.NGAY_CNHAT = objTSDB.NGAY_CNHAT;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            TaiSanDamBaoProcess processTaiSanDamBao = new TaiSanDamBaoProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            obj = new TD_TAI_SAN_DAM_BAO();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processTaiSanDamBao.TaiSanDamBao(DatabaseConstant.Action.LOAD_DATA, ref obj, ref listClientResponseDetail);
                if (ret == true)
                {
                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    #region Thông tin chung
                    txtMaTSDB.Text = obj.OBJ_TD_TSDB.MA_TSDB;
                    txtSoHDTC.Text = obj.OBJ_TD_TSDB.SO_HDTC;
                    txtMaKH.Text = obj.OBJ_TD_TSDB.MA_KHANG;
                    txtMaNhomTSDB.Text = obj.OBJ_TD_TSDB.MA_LOAI_TSDB;
                    txtTenTSDB.Text = obj.OBJ_TD_TSDB.TEN_TSDB;
                    txtMoTa.Text = obj.OBJ_TD_TSDB.MO_TA;
                    txtChuTaiSan.Text = obj.OBJ_TD_TSDB.MA_CHU_TSAN;
                    txtTenChuTaiSan.Text = obj.OBJ_TD_TSDB.TEN_CHU_TSAN;
                    txtSoTKThamChieu.Text = obj.OBJ_TD_TSDB.SO_TKIEM_TC;
                    txtSoDu.Text = obj.OBJ_TD_TSDB.SO_TKIEM_TC_SODU.ToString();
                    txtGiaTriDinhGia.Value = Convert.ToDouble(obj.OBJ_TD_TSDB.GTRI_DGIA);
                    txtGiaTriDBToiDa.Value = Convert.ToDouble(obj.OBJ_TD_TSDB.GTRI_DBAO_TOI_DA);
                    txtGiaTriHachToan.Value = Convert.ToDouble(obj.OBJ_TD_TSDB.GTRI_HTOAN);
                    if (obj.OBJ_TD_TSDB.NGAY_HET_HLUC != null)
                        raddtNgayDH.Value = LDateTime.StringToDate(obj.OBJ_TD_TSDB.NGAY_HET_HLUC, "yyyyMMdd");
                    else
                        raddtNgayDH.Value = null;
                    if (obj.OBJ_TD_TSDB.NGAY_HLUC != null)
                        teldtNgayHieuLuc.Value = LDateTime.StringToDate(obj.OBJ_TD_TSDB.NGAY_HLUC, "yyyyMMdd");
                    else
                        teldtNgayHieuLuc.Value = null;
                    if (obj.OBJ_TD_TSDB.NGAY_HET_HLUC != null)
                        teldtNgayHetHieuLuc.Value = LDateTime.StringToDate(obj.OBJ_TD_TSDB.NGAY_HET_HLUC, "yyyyMMdd");
                    else
                        teldtNgayHetHieuLuc.Value = null;
                    cmbLoaiTien.SelectedIndex = lstSourceLoaiTien.IndexOf(lstSourceLoaiTien.FirstOrDefault(e => e.KeywordStrings[0].Equals(obj.OBJ_TD_TSDB.MA_LOAI_TIEN)));
                    cmbNguonVon.SelectedIndex = lstSourceNguonVon.IndexOf(lstSourceNguonVon.FirstOrDefault(e => e.KeywordStrings[0].Equals(obj.OBJ_TD_TSDB.NV_LOAI_NVON)));

                    string _mA_KH;
                    _mA_KH = obj.OBJ_TD_TSDB.MA_KHANG;
                    dsKHangLoaiTSDB = processTaiSanDamBao.getThongTinKHLoaiTSTheoIDKhang(_mA_KH);
                    if (dsKHangLoaiTSDB != null && dsKHangLoaiTSDB.Tables.Count > 0)
                    {
                        DataRow dr = dsKHangLoaiTSDB.Tables[0].Rows[0];
                        if (dr["ID"] != null && !LString.IsNullOrEmptyOrSpace(dr["ID"].ToString()))
                        {
                            lblTenKH.Content = dr["TEN_KHANG"].ToString();
                            lblTenNhomTSDB.Content = dr["TEN_LOAI_TSDB"].ToString();
                        }
                    }
                    #endregion

                    #region Thông tin hạn mức

                    ds = obj.DATA_SET;
                    grbDSHanMuc.DataContext = ds.Tables[0].DefaultView;
                    #endregion

                    #region Thông tin kiểm soát
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    raddtNgayLap.Value = LDateTime.StringToDate(obj.NGAY_NHAP, "yyyyMMdd");
                    txtNguoiLap.Text = obj.NGUOI_NHAP;
                    if (LDateTime.IsDate(obj.NGAY_CNHAT, "yyyyMMdd") == true)
                        raddtNgayCapNhat.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, "yyyyMMdd");
                    txtNguoiCapNhat.Text = obj.NGUOI_CNHAT;
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

        private void LoadFormData()
        {
            
        }

        private void ResetForm()
        {
            sTrangThaiNVu = "";
            lblTrangThai.Content = "";

            #region Thông tin chung
            txtMaKH.Text = "";
            lblTenKH.Content = "";
            txtSoTKThamChieu.Text = "";
            txtMaNhomTSDB.Text = "";
            txtTenTSDB.Text = "";
            txtSoDu.Value = 0;
            txtGiaTriDinhGia.Value = 0;
            txtGiaTriDBToiDa.Value = 0;
            txtGiaTriHachToan.Value = 0;
            raddtNgayDH.Value = null;
            teldtNgayHieuLuc.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            teldtNgayHetHieuLuc.Value = null;
            txtMoTa.Text = "";

            cmbLoaiTien.SelectedIndex = lstSourceLoaiTien.IndexOf(lstSourceLoaiTien.FirstOrDefault(e => e.KeywordStrings[0].Equals(ClientInformation.MaDongNoiTe)));

            #endregion

            #region Thông tin kiểm soát
            txtTrangThaiBanGhi.Text = "";
            raddtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            raddtNgayCapNhat.Value = null;
            txtNguoiCapNhat.Text = "";
            #endregion
         
        }

        private bool Validation()
        {
            if (txtMaKH.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaChon(lblMaKH.Content.ToString());
                txtMaKH.Focus();
                return false;
            }

            if (txtTenTSDB.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaChon(lbTenTSDB.Content.ToString());
                txtTenTSDB.Focus();
                return false;
            }

            if (txtGiaTriDinhGia.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaChon(lbGiaTriDinhGia.Content.ToString());
                txtGiaTriDinhGia.Focus();
                return false;
            }

            if (txtGiaTriDBToiDa.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaChon(lbGiaTriDBToiDa.Content.ToString());
                txtGiaTriDBToiDa.Focus();
                return false;
            }

            if (cmbNguonVon.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaChon(lbNguonVon.Content.ToString());
                cmbNguonVon.Focus();
                return false;
            }

            if (txtGiaTriDBToiDa.Value > txtGiaTriDinhGia.Value)
            {
                CommonFunction.ThongBaoLoi("Giá trị đảm bảo tối đa không được lớn hơn giá trị định giá");
                txtGiaTriDBToiDa.Focus();
                return false;
            }

            if (txtGiaTriHachToan.Value > txtGiaTriDinhGia.Value)
            {
                CommonFunction.ThongBaoLoi("Giá trị hạch toán không được lớn hơn giá trị định giá");
                txtGiaTriHachToan.Focus();
                return false;
            }

            if (dtHMTSDB.Rows.Count == 0)
            {
                CommonFunction.ThongBaoLoi("Chưa nhập thông tin hạn mức");
                grbDSHanMuc.Focus();
                return false;
            }

            if (teldtNgayHieuLuc.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaChon(lblNgayHieuLuc.Content.ToString());
                teldtNgayHieuLuc.Focus();
                return false;
            }

            if (teldtNgayHetHieuLuc.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaChon(lblNgayHetHieuLuc.Content.ToString());
                teldtNgayHetHieuLuc.Focus();
                return false;
            }

            return true;
        }

        private void SetEnabledControls()
        {
            #region Thêm
            if (action == DatabaseConstant.Action.THEM)
            {
                txtMaKH.IsEnabled = true;
                btnMaKH.IsEnabled = true;
                txtMaTSDB.IsEnabled = false;
                txtMaNhomTSDB.IsEnabled = true;
                txtTenTSDB.IsEnabled = true;
                cmbLoaiTien.IsEnabled = true;
                cmbNguonVon.IsEnabled = true;
                txtSoTKThamChieu.IsEnabled = false;
                txtSoDu.IsEnabled = false;
                txtSoHDTC.IsEnabled = true;
                txtChuTaiSan.IsEnabled = true;
                txtGiaTriDBToiDa.IsEnabled = true;
                txtGiaTriDinhGia.IsEnabled = true;
                txtGiaTriHachToan.IsEnabled = true;
                teldtNgayHieuLuc.IsEnabled = true;
                dtpNgayHieuLuc.IsEnabled = true;
                teldtNgayHetHieuLuc.IsEnabled = true;
                dtpNgayHetHieuLuc.IsEnabled = true;
                txtMoTa.IsEnabled = true;
                btnAddHM.IsEnabled = true;
                btnDeleteHM.IsEnabled = true;
            }
            #endregion

            #region Sửa
            else if (action == DatabaseConstant.Action.SUA)
            {
                txtMaKH.IsEnabled = true;
                btnMaKH.IsEnabled = true;
                txtMaTSDB.IsEnabled = false;
                txtMaNhomTSDB.IsEnabled = true;
                txtTenTSDB.IsEnabled = true;
                cmbLoaiTien.IsEnabled = true;
                cmbNguonVon.IsEnabled = true;
                txtSoTKThamChieu.IsEnabled = false;
                txtSoDu.IsEnabled = false;
                txtSoHDTC.IsEnabled = true;
                txtChuTaiSan.IsEnabled = true;
                txtGiaTriDBToiDa.IsEnabled = true;
                txtGiaTriDinhGia.IsEnabled = true;
                txtGiaTriHachToan.IsEnabled = true;
                teldtNgayHieuLuc.IsEnabled = true;
                dtpNgayHieuLuc.IsEnabled = true;
                teldtNgayHetHieuLuc.IsEnabled = true;
                dtpNgayHetHieuLuc.IsEnabled = true;
                txtMoTa.IsEnabled = true;
                btnAddHM.IsEnabled = true;
                btnDeleteHM.IsEnabled = true;
                raddgrDSachHanMuc.IsEnabled = true;
            }
            #endregion

            #region Xem ~ Mặc định
            else
            {
                txtMaKH.IsEnabled = false;
                btnMaKH.IsEnabled = false;
                btnMaNhomTSDB.IsEnabled = false;
                btnChuTaiSan.IsEnabled = false;
                btnSoTKThamChieu.IsEnabled = false;
                txtMaTSDB.IsEnabled = false;
                txtTenChuTaiSan.IsEnabled = false;
                txtMaNhomTSDB.IsEnabled = false;
                txtTenTSDB.IsEnabled = false;
                cmbLoaiTien.IsEnabled = false;
                cmbNguonVon.IsEnabled = false;
                txtSoTKThamChieu.IsEnabled = false;
                txtSoDu.IsEnabled = false;
                txtSoHDTC.IsEnabled = false;
                txtChuTaiSan.IsEnabled = false;
                txtGiaTriDBToiDa.IsEnabled = false;
                txtGiaTriDinhGia.IsEnabled = false;
                txtGiaTriHachToan.IsEnabled = false;
                teldtNgayHieuLuc.IsEnabled = false;
                dtpNgayHieuLuc.IsEnabled = false;
                teldtNgayHetHieuLuc.IsEnabled = false;
                dtpNgayHetHieuLuc.IsEnabled = false;
                txtMoTa.IsEnabled = false;
                btnAddHM.IsEnabled = false;
                btnDeleteHM.IsEnabled = false;
                raddgrDSachHanMuc.IsEnabled = false;
            }
            #endregion
        }


        public void OnHold()
        {
            try
            {
                if (!Validation()) return;

                string trangThai = "";
                if (sTrangThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                else
                    trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));

                obj = new TD_TAI_SAN_DAM_BAO();

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

                string trangThai = "";
                if (sTrangThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                else
                    trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));

                obj = new TD_TAI_SAN_DAM_BAO();

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

        public void OnAddNew(TD_TAI_SAN_DAM_BAO obj)
        {

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TaiSanDamBaoProcess processTaiSanDamBao = new TaiSanDamBaoProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processTaiSanDamBao.TaiSanDamBao(DatabaseConstant.Action.THEM, ref obj, ref listClientResponseDetail);
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

        public void AfterAddNew(bool ret, TD_TAI_SAN_DAM_BAO obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                    id = obj.ID;

                    txtMaTSDB.Text = obj.MA_TSDB;
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

                bool ret = process.LockData(DatabaseConstant.Module.TSDB,
                    DatabaseConstant.Function.TD_TSDB_CT,
                    DatabaseConstant.Table.TD_TSDB,
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

        public void OnModify(TD_TAI_SAN_DAM_BAO obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TaiSanDamBaoProcess processTaiSanDamBao = new TaiSanDamBaoProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processTaiSanDamBao.TaiSanDamBao(DatabaseConstant.Action.SUA, ref obj, ref listClientResponseDetail);
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

        public void AfterModify(bool ret, TD_TAI_SAN_DAM_BAO obj, List<ClientResponseDetail> listClientResponseDetail)
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TSDB,
                    DatabaseConstant.Function.TD_TSDB_CT,
                    DatabaseConstant.Table.TD_TSDB,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.TSDB,
                        DatabaseConstant.Function.TD_TSDB_CT,
                        DatabaseConstant.Table.TD_TSDB,
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TSDB,
                    DatabaseConstant.Function.TD_TSDB_CT,
                    DatabaseConstant.Table.TD_TSDB,
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
            TaiSanDamBaoProcess processTaiSanDamBao = new TaiSanDamBaoProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processTaiSanDamBao.TaiSanDamBao(action, ref obj, ref listClientResponseDetail);
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
                processTaiSanDamBao = null;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TSDB,
                    DatabaseConstant.Function.TD_TSDB_CT,
                    DatabaseConstant.Table.TD_TSDB,
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


        public void BeforeApprove()
        {
            try
            {
                // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.TSDB,
                        DatabaseConstant.Function.TD_TSDB_CT,
                        DatabaseConstant.Table.TD_TSDB,
                        DatabaseConstant.Action.DUYET,
                        listLockId);

                    if (retLockData)
                    {
                        action = DatabaseConstant.Action.DUYET;
                        OnApprove();
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
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnApprove()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            TaiSanDamBaoProcess processTaiSanDamBao = new TaiSanDamBaoProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processTaiSanDamBao.TaiSanDamBao(action, ref obj, ref listClientResponseDetail);
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
                listClientResponseDetail = null;
                processTaiSanDamBao = null;
            }
        }

        public void AfterApprove(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TSDB,
                    DatabaseConstant.Function.TD_TSDB_CT,
                    DatabaseConstant.Table.TD_TSDB,
                    DatabaseConstant.Action.DUYET,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        public void BeforeCancel()
        {
            try
            {
                // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.TSDB,
                        DatabaseConstant.Function.TD_TSDB_CT,
                        DatabaseConstant.Table.TD_TSDB,
                        DatabaseConstant.Action.THOAI_DUYET,
                        listLockId);

                    if (retLockData)
                    {
                        action = DatabaseConstant.Action.THOAI_DUYET;
                        OnCancel();
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
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnCancel()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            TaiSanDamBaoProcess processTaiSanDamBao = new TaiSanDamBaoProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processTaiSanDamBao.TaiSanDamBao(action, ref obj, ref listClientResponseDetail);
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
                listClientResponseDetail = null;
                processTaiSanDamBao = null;
            }
        }

        public void AfterCancel(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TSDB,
                    DatabaseConstant.Function.TD_TSDB_CT,
                    DatabaseConstant.Table.TD_TSDB,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listLockId);

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }


        public void BeforeRefuse()
        {
            try
            {
                // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.TSDB,
                        DatabaseConstant.Function.TD_TSDB_CT,
                        DatabaseConstant.Table.TD_TSDB,
                        DatabaseConstant.Action.TU_CHOI_DUYET,
                        listLockId);

                    if (retLockData)
                    {
                        action = DatabaseConstant.Action.TU_CHOI_DUYET;
                        OnRefuse();
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
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnRefuse()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            TaiSanDamBaoProcess processTaiSanDamBao = new TaiSanDamBaoProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processTaiSanDamBao.TaiSanDamBao(action, ref obj, ref listClientResponseDetail);
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
                listClientResponseDetail = null;
                processTaiSanDamBao = null;
            }
        }

        public void AfterRefuse(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TSDB,
                    DatabaseConstant.Function.TD_TSDB_CT,
                    DatabaseConstant.Table.TD_TSDB,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);
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
