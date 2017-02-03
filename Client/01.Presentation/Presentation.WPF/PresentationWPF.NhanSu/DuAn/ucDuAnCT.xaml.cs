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
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls;
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.Common;
using PresentationWPF.CustomControl;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.NhanSuServiceRef;

namespace PresentationWPF.NhanSu.DuAn
{
    /// <summary>
    /// Interaction logic for ucDuAnCT.xaml
    /// </summary>
    public partial class ucDuAnCT : UserControl
    {
        #region Khai bao
        public event EventHandler OnSavingCompleted;

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private string trangThaiNVu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();

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

        List<AutoCompleteEntry> lstSourceDuAn = new List<AutoCompleteEntry>();

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        private DataTable dt = null;

        #endregion

        #region Khoi tao
        public ucDuAnCT()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            KhoiTaoDataTable();

            LoadCombobox();

            cmbDuAn.Focus();
        }

        private void LoadCombobox()
        {
            COMBOBOX_DTO combo = null;
            List<string> lstDieuKien = null;
            List<COMBOBOX_DTO> lstCombobox = new List<COMBOBOX_DTO>();

            //Loại phụ cấp
            lstDieuKien = new List<string>();
            lstDieuKien.Add("NS_DM_DU_AN");
            lstDieuKien.Add(ClientInformation.MaDonVi);
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC_NHAN_SU_THEO_BANG.getValue();
            combo.combobox = cmbDuAn;
            combo.lstSource = lstSourceDuAn;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            AutoComboBox auto = new AutoComboBox();
            auto.GenAutoComboBoxTheoList(ref lstCombobox);

            cmbDuAn.SelectedIndex = 0;



            DataSet dsNQL = new NhanSuProcess().GetDanhSachHoSo(ClientInformation.MaDonViGiaoDich, BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), BusinessConstant.LOAI_HO_SO.CHINH_THUC.layGiaTri());
            if (dsNQL != null && dsNQL.Tables[0].Rows.Count > 0)
            {
                ((GridViewComboBoxColumn)grid.Columns["NQL"]).ItemsSource = dsNQL.Tables[0].DefaultView;
            }
            else
            {
                ((GridViewComboBoxColumn)grid.Columns["NQL"]).ItemsSource = null;
            }                        
        }

        private void KhoiTaoDataTable()
        {
            dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("ID_HSO", typeof(int));
            dt.Columns.Add("MA_HSO", typeof(string));
            dt.Columns.Add("TEN_HSO", typeof(string));
            dt.Columns.Add("NGAY_THAM_GIA", typeof(string));
            dt.Columns.Add("CHUC_VU", typeof(int));
            dt.Columns.Add("NQL", typeof(int));
        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.NhanSu;component/DuAn/ucDuAnCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
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

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnSave();
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

            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                OnSave();
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

            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                OnSave();
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

        /// <summary>
        /// Hàm xử lý giải phóng lock dữ liệu cho form
        /// </summary>
        private void Release()
        {
            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool ret = process.UnlockData(DatabaseConstant.Module.NSTL, 
                DatabaseConstant.Function.NS_QLY_DU_AN,                 
                DatabaseConstant.Table.NS_QLY_DU_AN,
                DatabaseConstant.Action.SUA,
                listLockId);
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

        private bool KiemTraTonTaiGrid(string id)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ID_HSO"].ToString().Equals(id))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region Xử lý nghiệp vụ
        private void GetFormData(ref NS_QLY_DU_AN_DTO obj)
        {
            try
            {
                obj = new NS_QLY_DU_AN_DTO();

                obj.ID_DU_AN = Convert.ToInt32(lstSourceDuAn.ElementAt(cmbDuAn.SelectedIndex).KeywordStrings.ElementAt(1));

                List<NS_QLY_DU_AN> lst = new List<NS_QLY_DU_AN>();
                NS_QLY_DU_AN objQLyDuAn = null;
                foreach (DataRow dr in dt.Rows)
                {
                    objQLyDuAn = new NS_QLY_DU_AN();

                    objQLyDuAn.ID_DU_AN = obj.ID_DU_AN;
                    objQLyDuAn.ID_HO_SO = Convert.ToInt32(dr["ID_HSO"]);
                    objQLyDuAn.ID_CHUC_VU_DU_AN = Convert.ToInt32(dr["CHUC_VU"]);
                    objQLyDuAn.ID_NGUOI_QLY = Convert.ToInt32(dr["NQL"]);
                    if (LDateTime.IsDate(dr["NGAY_THAM_GIA"].ToString(), "dd/MM/yyyy"))
                        objQLyDuAn.NGAY_THAM_GIA = LDateTime.StringToDate(dr["NGAY_THAM_GIA"].ToString(), "dd/MM/yyyy").ToString("yyyyMMdd");
                    objQLyDuAn.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    objQLyDuAn.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    objQLyDuAn.MA_DVI_QLY = ClientInformation.MaDonVi;
                    objQLyDuAn.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    objQLyDuAn.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    objQLyDuAn.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    objQLyDuAn.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    objQLyDuAn.NGUOI_CNHAT = ClientInformation.TenDangNhap;

                    lst.Add(objQLyDuAn);
                }

                obj.LST_QLY_DU_AN = lst.ToArray();     
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void LoadDuLieu()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                NhanSuProcess processNhanSu = new NhanSuProcess();
                int idDuAn = Convert.ToInt32(lstSourceDuAn[cmbDuAn.SelectedIndex].KeywordStrings[1]);
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", ClientInformation.MaDonViQuanLy);
                LDatatable.AddParameter(ref dt, "@ID_DU_AN", "STRING", idDuAn.ToString());
                DataSet ds = processNhanSu.GetDanhSachQuanLyDuAn(dt);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    grid.DataContext = dt.DefaultView;
                }
                else
                {
                    dt.Rows.Clear();
                    grid.DataContext = null;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);                
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private bool Validation()
        {
            try
            {

                return true;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                return false;
            }
        }

        public void OnSave()
        {
            try
            {
                if (!Validation()) return;

                NS_QLY_DU_AN_DTO obj = null;
                GetFormData(ref obj);

                NhanSuProcess processNhanSu = new NhanSuProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                Mouse.OverrideCursor = Cursors.Wait;
                ret = processNhanSu.QuanLyDuAn(DatabaseConstant.Action.LUU, ref obj, ref listClientResponseDetail);
                Mouse.OverrideCursor = Cursors.Arrow;

                AfterSave(ret, listClientResponseDetail);

            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void AfterSave(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);
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

        #endregion        

        private void cmbDuAn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            #region Load combobox chức vụ dự án
            try
            {
                string maDuAn = lstSourceDuAn[cmbDuAn.SelectedIndex].KeywordStrings[0];
                DataSet dsChucVu = new NhanSuProcess().GetDanhSachChucVuDuAn(maDuAn, ClientInformation.MaDonVi);
                if (dsChucVu != null && dsChucVu.Tables[0].Rows.Count > 0)
                {
                    ((GridViewComboBoxColumn)grid.Columns["CHUC_VU"]).ItemsSource = dsChucVu.Tables[0].DefaultView;
                }
                else
                {
                    ((GridViewComboBoxColumn)grid.Columns["CHUC_VU"]).ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                CommonFunction.ThongBaoLoi(ex);
            }
            #endregion

            LoadDuLieu();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lstPopup = null;
                var process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_HOSO_NVIEN.getValue(), lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null)
                {
                    foreach (DataRow dr in lstPopup)
                    {
                        if (KiemTraTonTaiGrid(dr["ID"].ToString()))
                        {

                            DataRow drHSo = dt.NewRow();

                            drHSo["ID_HSO"] = Convert.ToInt32(dr["ID"]);
                            drHSo["MA_HSO"] = dr["MA_HSO"].ToString();
                            drHSo["TEN_HSO"] = dr["TEN_HSO"].ToString();
                            drHSo["CHUC_VU"] = 0;
                            drHSo["NGAY_THAM_GIA"] = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd").ToString("dd/MM/yyyy");
                            drHSo["NQL"] = 0;

                            dt.Rows.Add(drHSo);
                        }
                    }
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["STT"] = i + 1;
                }

                grid.DataContext = dt.DefaultView;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                CommonFunction.ThongBaoLoi(ex);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<int> lstSTT = new List<int>();
                for (int i = 0; i < grid.SelectedItems.Count; i++)
                {
                    DataRowView dr = (DataRowView)grid.SelectedItems[i];
                    lstSTT.Add(Convert.ToInt32(dr["STT"]));
                }
                lstSTT.SortByDesc();
                foreach (int stt in lstSTT)
                    dt.Rows.RemoveAt(stt - 1);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["STT"] = i + 1;
                }

                grid.DataContext = dt.DefaultView;
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
    }
}
