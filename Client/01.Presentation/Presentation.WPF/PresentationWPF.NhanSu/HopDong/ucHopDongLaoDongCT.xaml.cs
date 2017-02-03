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
using Presentation.Process.NhanSuServiceRef;
using Presentation.Process.KhachHangServiceRef;
using Presentation.Process.BaoCaoServiceRef;
using PresentationWPF.CustomControl;
using PresentationWPF.NhanSu.Converts;
using PresentationWPF.BaoCao.DungChung;

namespace PresentationWPF.NhanSu.HopDong
{
    /// <summary>
    /// Interaction logic for ucHopDongLaoDongCT.xaml
    /// </summary>
    public partial class ucHopDongLaoDongCT : UserControl
    {
        #region Khai bao
        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private DatabaseConstant.Function function = DatabaseConstant.Function.NS_HOP_DONG_CT;

        public event EventHandler OnSavingCompleted;

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int idHoSo = 0;
        private int idNguoiDaiDien = 0;
       
        private NS_HOP_DONG obj;
        public NS_HOP_DONG Obj
        {
            get { return obj; }
            set { obj = value; }
        }

        DataTable dtPhuCap = null;
        DataTable dtLoaiPhuCap = null;

        private string loaiThoiHan = BusinessConstant.LOAI_THOI_HAN_HDLD.CO_THOI_HAN.layGiaTri();

        private string sTrangThaiNVu = "";

        List<AutoCompleteEntry> lstSourceViTriNguoiDaiDien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceViTriNguoiLaoDong = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiHdld = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceThoiHanHdld = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDonViThoiGian = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHinhThucTraLuong = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiPhuCap = new List<AutoCompleteEntry>();

        List<NS_DM_PHU_CAP> lstDMLoaiPhuCap = new List<NS_DM_PHU_CAP>();

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
        public ucHopDongLaoDongCT()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            InitEventHandler();

            KhoiTaoTable();

            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);

            txtNguoiDaiDien.Focus();
        }        

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.NhanSu;component/HopDong/ucHopDongLaoDongCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void InitEventHandler()
        {
            chkThemNhieuLan.Checked += new RoutedEventHandler(chkThemNhieuLan_Checked);

            btnNguoiDaiDien.Click += new RoutedEventHandler(btnNguoiDaiDien_Click);
            txtNguoiDaiDien.KeyDown += new KeyEventHandler(txtNguoiDaiDien_KeyDown);
            txtNhanVien.KeyDown += new KeyEventHandler(txtNhanVien_KeyDown);
            btnNhanVien.Click += new RoutedEventHandler(btnNhanVien_Click);

            cmbLoaiThoiHan.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiThoiHan_SelectionChanged);

            numLuongCoBan.LostFocus += new RoutedEventHandler(numLuongCoBan_LostFocus);            
            numHeSoLuong.LostFocus += new RoutedEventHandler(numHeSoLuong_LostFocus);

            btnAdd.Click += new RoutedEventHandler(btnAdd_Click);
            btnDelete.Click += new RoutedEventHandler(btnDelete_Click);
                
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
            ResetData();
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
                ResetData();
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
                OnPrint();
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
                ResetData();
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
                OnPrint();
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
                DatabaseConstant.Function.NS_HOP_DONG_CT,
                DatabaseConstant.Table.NS_HOP_DONG,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        private void ResetData()
        {
            action = DatabaseConstant.Action.THEM;
            id = 0;
            obj = null;
            sTrangThaiNVu = "";

            ResetForm();
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
            txtTenNhanVien.Focus();

            chkThemNhieuLan.IsChecked = false;
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

        private void btnNguoiDaiDien_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lstPopup = null;
                var process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonVi);
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_HOSO_NVIEN_DVI.getValue(), lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow row = lstPopup[0];
                    if (!string.IsNullOrWhiteSpace(row[1].ToString()))
                        idNguoiDaiDien = Convert.ToInt32(row[1].ToString());
                    if (!string.IsNullOrWhiteSpace(row[2].ToString()))
                        txtNguoiDaiDien.Text = row[2].ToString();
                    if (!string.IsNullOrWhiteSpace(row[3].ToString()))
                        txtTenNguoiDaiDien.Text = row[3].ToString();
                    if (!string.IsNullOrWhiteSpace(row[4].ToString()))
                        cmbChucVu.SelectedIndex = lstSourceViTriNguoiDaiDien.IndexOf(lstSourceViTriNguoiDaiDien.FirstOrDefault(i => i.KeywordStrings[1].Equals(row[4].ToString())));
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                CommonFunction.ThongBaoLoi(ex);
            }
        }

        private void btnNhanVien_Click(object sender, RoutedEventArgs e)
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
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow row = lstPopup[0];
                    if (!string.IsNullOrWhiteSpace(row[1].ToString()))
                        idHoSo = Convert.ToInt32(row[1].ToString());
                    if (!string.IsNullOrWhiteSpace(row[2].ToString()))
                        txtNhanVien.Text = row[2].ToString();
                    if (!string.IsNullOrWhiteSpace(row[3].ToString()))
                        txtTenNhanVien.Text = row[3].ToString();
                    if (!string.IsNullOrWhiteSpace(row[4].ToString()))
                        cmbChucVuNhanVien.SelectedIndex = lstSourceViTriNguoiLaoDong.IndexOf(lstSourceViTriNguoiLaoDong.FirstOrDefault(i => i.KeywordStrings[1].Equals(row[4].ToString())));
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                CommonFunction.ThongBaoLoi(ex);
            }
        }

        private void txtNguoiDaiDien_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnNguoiDaiDien_Click(null, null);
            }
        }

        private void txtNhanVien_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnNhanVien_Click(null, null);
            }
        }

        private void cmbLoaiThoiHan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loaiThoiHan = lstSourceThoiHanHdld[cmbLoaiThoiHan.SelectedIndex].KeywordStrings[0].ToString();

            if (loaiThoiHan.Equals(BusinessConstant.LOAI_THOI_HAN_HDLD.CO_THOI_HAN.layGiaTri()))
            {
                numThoiHan.IsEnabled = true;
                cmbThoiHan.IsEnabled = true;
            }
            else
            {
                numThoiHan.IsEnabled = false;
                cmbThoiHan.IsEnabled = false;
            }
        }

        private void numLuongCoBan_LostFocus(object sender, RoutedEventArgs e)
        {
            numThanhTien.Value = numLuongCoBan.Value * numHeSoLuong.Value;
        }

        private void numHeSoLuong_LostFocus(object sender, RoutedEventArgs e)
        {
            numThanhTien.Value = numLuongCoBan.Value * numHeSoLuong.Value;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            dtPhuCap.Rows.Add(dtPhuCap.Rows.Count + 1, null, null);
            grdPhuCap.DataContext = dtPhuCap.DefaultView;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            List<DataRowView> lstSelected = new List<DataRowView>();
            for (int i = 0; i < grdPhuCap.SelectedItems.Count; i++)
            {
                DataRowView dr = (DataRowView)grdPhuCap.SelectedItems[i];
                if (!dr["STT"].ToString().IsNullOrEmptyOrSpace())
                    lstSelected.Add(dr);
            }

            if (lstSelected.Count == 0)
            {
                //LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                foreach (DataRowView dr in lstSelected)
                {
                    grdPhuCap.Items.Remove(dr);
                }

                for (int i = 0; i < grdPhuCap.Items.Count; i++)
                {

                    DataRowView dr = (DataRowView)grdPhuCap.Items[i];
                    if (!dr["STT"].ToString().IsNullOrEmptyOrSpace() && Convert.ToInt32(dr["STT"]) > 0)
                    {
                        if (!dr[2].ToString().IsNullOrEmptyOrSpace())
                        {
                            dr["STT"] = i + 1;
                            grdPhuCap.SelectedItem = grdPhuCap.Items[i];
                            grdPhuCap.CurrentItem = dr;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }        

        private void KhoiTaoTable()
        {
            dtPhuCap = new DataTable();
            dtPhuCap.Columns.Add("STT", typeof(int));
            dtPhuCap.Columns.Add("PHU_CAP", typeof(string));
            dtPhuCap.Columns.Add("SO_TIEN", typeof(decimal));

            dtLoaiPhuCap = new DataTable();
            dtLoaiPhuCap.Columns.Add("ID", typeof(int));
            dtLoaiPhuCap.Columns.Add("TEN", typeof(string));
        }
      
        #endregion               

        #region Xử lý nghiệp vụ
        private void GetFormData(ref NS_HOP_DONG obj, ref List<NS_PHU_CAP> lstPhuCap, string sTrangThaiNVu)
        {
            try
            {
                obj = new NS_HOP_DONG();
                lstPhuCap = new List<NS_PHU_CAP>();                

                #region NS_HOP_DONG
                obj.ID = id;
                obj.ID_HSO = idHoSo;
                obj.MA_HOP_DONG = txtSoHopDong.Text;
                obj.NGAY_LAP = Convert.ToDateTime(raddtNgayLapHopDong.Value).ToString("yyyyMMdd");
                obj.NGAY_HLUC = Convert.ToDateTime(raddtNgayHieuLuc.Value).ToString("yyyyMMdd");
                obj.MA_LOAI_HDLD = lstSourceLoaiHdld[cmbLoaiHopDong.SelectedIndex].KeywordStrings[0];
                obj.ID_THAN_HDLD = Convert.ToInt32(lstSourceThoiHanHdld[cmbLoaiThoiHan.SelectedIndex].KeywordStrings[1]);
                if (cmbThoiHan.SelectedIndex >= 0)
                {
                    obj.THOI_HAN = Convert.ToInt32(numThoiHan.Value);
                    obj.THOI_HAN_ID_DVTTG = Convert.ToInt32(lstSourceDonViThoiGian[cmbThoiHan.SelectedIndex].KeywordStrings[1]);
                }
                if(raddtNgayHetHieuLuc.Value != null)
                    obj.NGAY_HET_HLUC = Convert.ToDateTime(raddtNgayHetHieuLuc.Value).ToString("yyyyMMdd"); ;

                obj.ID_NGUOI_DDIEN = idNguoiDaiDien;
                obj.MA_GIAY_UY_QUYEN = txtSoUyQuyen.Text;
                if(numSoNgayLam1Tuan.Value != null)
                    obj.LVIEC_MOT_TUAN = Convert.ToInt32(numSoNgayLam1Tuan.Value);
                if(numSoGioLam1Ngay.Value != null)
                    obj.LVIEC_MOT_NGAY = Convert.ToInt32(numSoGioLam1Ngay.Value); ;
                obj.LUONG_CO_BAN = Convert.ToDecimal(numLuongCoBan.Value);
                obj.HE_SO_LUONG = Convert.ToDecimal(numHeSoLuong.Value);
                obj.THANH_TIEN = Convert.ToDecimal(numThanhTien.Value);
                obj.ID_HTHUC_TLUONG = Convert.ToInt32(lstSourceHinhThucTraLuong[cmbHinhThucTraLuong.SelectedIndex].KeywordStrings[1]);
                obj.NGAY_TRA_LUONG = Convert.ToInt32(numNgayTraLuong.Value).ToString();
                obj.SO_TKHOAN = txtSoTaiKhoan.Text;
                obj.SO_TKHOAN_NHANG = null;
                obj.GHI_CHU = txtDienGiai.Text;
    
                //Thông tin kiểm soát
                obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.TTHAI_NVU = sTrangThaiNVu;                
                obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                obj.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                obj.NGAY_NHAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                obj.NGUOI_NHAP = txtNguoiLap.Text;
                if (action != DatabaseConstant.Action.THEM)
                {
                    obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }

                #endregion 

                #region NS_PHU_CAP
                foreach (DataRow row in dtPhuCap.Rows)
                {
                    NS_PHU_CAP objPhuCap = new NS_PHU_CAP();
                    objPhuCap.ID_HOP_DONG = id;
                    objPhuCap.ID_HSO = idHoSo;                    
                    objPhuCap.NGAY_DLIEU = obj.NGAY_LAP;
                    objPhuCap.NGAY_HLUC = obj.NGAY_HLUC;
                    objPhuCap.NGAY_HET_HLUC = obj.NGAY_HET_HLUC;
                    objPhuCap.ID_LOAI_TNHAP = lstDMLoaiPhuCap.FirstOrDefault(e => e.TEN.Equals(row["PHU_CAP"])).ID;
                    objPhuCap.SO_TIEN = Convert.ToDecimal(row["SO_TIEN"]);
                    objPhuCap.TTHAI_BGHI = obj.TTHAI_BGHI;
                    objPhuCap.TTHAI_NVU = obj.TTHAI_NVU;
                    objPhuCap.MA_DVI_QLY = obj.MA_DVI_QLY;
                    objPhuCap.MA_DVI_TAO = obj.MA_DVI_TAO;
                    objPhuCap.NGAY_NHAP = obj.NGAY_NHAP;
                    objPhuCap.NGUOI_NHAP = obj.NGUOI_NHAP;
                    objPhuCap.NGAY_CNHAT = obj.NGAY_CNHAT;
                    objPhuCap.NGUOI_CNHAT = obj.NGUOI_CNHAT;
                    lstPhuCap.Add(objPhuCap);
                }
                #endregion
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            NhanSuProcess processNhanSu = new NhanSuProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            obj = new NS_HOP_DONG();
            NS_TEMP_HOP_DONG objTempHopDong = null;
            List<NS_PHU_CAP> lstPhuCap = null;
            List<NS_DM_CHUC_VU> lstDMChucVu = null;
            List<NS_DM_LOAI_HDLD> lstDMLoaiHdld = null;
            List<NS_DM_THAN_HDLD> lstDMThoiHanHdld = null;
            List<NS_DM_DVI_TGIAN> lstDMDonViThoiGian = null;
            List<NS_DM_HTHUC_TLUONG> lstDMHinhThucTraLuong = null;
            lstDMLoaiPhuCap = new List<NS_DM_PHU_CAP>();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processNhanSu.HopDong(DatabaseConstant.Action.LOAD, ref obj, ref objTempHopDong, ref lstPhuCap, ref lstDMChucVu, ref lstDMLoaiHdld, ref lstDMThoiHanHdld, ref lstDMDonViThoiGian, ref lstDMHinhThucTraLuong, ref lstDMLoaiPhuCap, ref listClientResponseDetail);
                if (ret == true)
                {
                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    #region Dữ liệu lên combobox
                    lstSourceViTriNguoiDaiDien = ConvertNhanSu.ToAutoCompleteEntry(lstDMChucVu);
                    lstSourceViTriNguoiLaoDong = ConvertNhanSu.ToAutoCompleteEntry(lstDMChucVu);
                    lstSourceLoaiHdld = ConvertNhanSu.ToAutoCompleteEntry(lstDMLoaiHdld);
                    lstSourceThoiHanHdld = ConvertNhanSu.ToAutoCompleteEntry(lstDMThoiHanHdld);
                    lstSourceDonViThoiGian = ConvertNhanSu.ToAutoCompleteEntry(lstDMDonViThoiGian);
                    lstSourceHinhThucTraLuong = ConvertNhanSu.ToAutoCompleteEntry(lstDMHinhThucTraLuong);
                    lstSourceLoaiPhuCap = ConvertNhanSu.ToAutoCompleteEntry(lstDMLoaiPhuCap);

                    AutoComboBox auto = new AutoComboBox();
                    auto.GenAutoComboBoxBySource(ref lstSourceViTriNguoiDaiDien, ref cmbChucVu);
                    auto.GenAutoComboBoxBySource(ref lstSourceViTriNguoiLaoDong, ref cmbChucVuNhanVien);
                    auto.GenAutoComboBoxBySource(ref lstSourceLoaiHdld, ref cmbLoaiHopDong);
                    auto.GenAutoComboBoxBySource(ref lstSourceThoiHanHdld, ref cmbLoaiThoiHan);
                    auto.GenAutoComboBoxBySource(ref lstSourceDonViThoiGian, ref cmbThoiHan);
                    auto.GenAutoComboBoxBySource(ref lstSourceHinhThucTraLuong, ref cmbHinhThucTraLuong);
                    #endregion

                    #region Người sử dụng lao động
                    idNguoiDaiDien = objTempHopDong.ID_NGUOI_DDIEN;
                    txtNguoiDaiDien.Text = objTempHopDong.MA_NGUOI_DDIEN;
                    txtTenNguoiDaiDien.Text = objTempHopDong.TEN_NGUOI_DDIEN;
                    cmbChucVu.SelectedIndex = lstSourceViTriNguoiDaiDien.IndexOf(lstSourceViTriNguoiDaiDien.FirstOrDefault(i => i.KeywordStrings[1].Equals(objTempHopDong.ID_CHUC_VU_NGUOI_DDIEN.ToString())));
                    txtSoUyQuyen.Text = obj.MA_GIAY_UY_QUYEN;
                    #endregion

                    #region Người lao động
                    idHoSo = objTempHopDong.ID_NGUOI_LDONG;
                    txtNhanVien.Text = objTempHopDong.MA_NGUOI_LDONG;
                    txtTenNhanVien.Text = objTempHopDong.TEN_NGUOI_LDONG;
                    cmbChucVuNhanVien.SelectedIndex = lstSourceViTriNguoiLaoDong.IndexOf(lstSourceViTriNguoiLaoDong.FirstOrDefault(i => i.KeywordStrings[1].Equals(objTempHopDong.ID_CHUC_VU_NGUOI_LDONG.ToString())));
                    #endregion

                    #region Thông tin hợp đồng
                    txtSoHopDong.Text = obj.MA_HOP_DONG;
                    cmbLoaiHopDong.SelectedIndex = lstSourceLoaiHdld.IndexOf(lstSourceLoaiHdld.FirstOrDefault(i => i.KeywordStrings[0].Equals(obj.MA_LOAI_HDLD.ToString())));
                    raddtNgayLapHopDong.Value = LDateTime.StringToDate(obj.NGAY_LAP, "yyyyMMdd");
                    cmbLoaiThoiHan.SelectedIndex = lstSourceThoiHanHdld.IndexOf(lstSourceThoiHanHdld.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.ID_THAN_HDLD.ToString())));
                    numThoiHan.Value = obj.THOI_HAN;
                    cmbThoiHan.SelectedIndex = lstSourceDonViThoiGian.IndexOf(lstSourceDonViThoiGian.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.THOI_HAN_ID_DVTTG.ToString())));
                    raddtNgayHieuLuc.Value = LDateTime.StringToDate(obj.NGAY_HLUC, "yyyyMMdd");
                    if(LDateTime.IsDate(obj.NGAY_HET_HLUC,"yyyyMMdd"))
                        raddtNgayHetHieuLuc.Value = LDateTime.StringToDate(obj.NGAY_HET_HLUC, "yyyyMMdd");
                    numSoNgayLam1Tuan.Value = Convert.ToDouble(obj.LVIEC_MOT_TUAN);
                    numSoGioLam1Ngay.Value = Convert.ToDouble(obj.LVIEC_MOT_NGAY);
                    #endregion

                    #region Lương
                    numLuongCoBan.Value = Convert.ToDouble(obj.LUONG_CO_BAN);
                    numHeSoLuong.Value = Convert.ToDouble(obj.HE_SO_LUONG);
                    numThanhTien.Value = Convert.ToDouble(obj.THANH_TIEN);
                    numNgayTraLuong.Value = Convert.ToInt32(obj.NGAY_TRA_LUONG);
                    cmbHinhThucTraLuong.SelectedIndex = lstSourceHinhThucTraLuong.IndexOf(lstSourceHinhThucTraLuong.FirstOrDefault(i => i.KeywordStrings[1].Equals(obj.ID_HTHUC_TLUONG.ToString())));
                    txtSoTaiKhoan.Text = obj.SO_TKHOAN;
                    #endregion

                    #region Thông tin phụ cấp
                    foreach (var item in lstDMLoaiPhuCap)
                    {
                        dtLoaiPhuCap.Rows.Add(item.ID, item.TEN);
                    }

                    dtPhuCap.Rows.Clear();
                    for(int i=0; i<lstPhuCap.Count; i++)
                    {
                        string tenPhuCap = lstDMLoaiPhuCap.FirstOrDefault(e => e.ID == lstPhuCap[i].ID_LOAI_TNHAP).TEN;
                        dtPhuCap.Rows.Add(i + 1, tenPhuCap, lstPhuCap[i].SO_TIEN);
                    }
                    grdPhuCap.DataContext = dtPhuCap.DefaultView;
                    ((GridViewComboBoxColumn)grdPhuCap.Columns["PHU_CAP"]).ItemsSource = dtLoaiPhuCap.DefaultView;
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
                processNhanSu = null;
                listClientResponseDetail = null;
                objTempHopDong = null;
                lstPhuCap = null;
                lstDMChucVu = null;
                lstDMLoaiHdld = null;
                lstDMThoiHanHdld = null;
                lstDMDonViThoiGian = null;
                lstDMHinhThucTraLuong = null;
            }
        }

        private void LoadFormData()
        {
            NhanSuProcess processNhanSu = new NhanSuProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            obj = new NS_HOP_DONG();
            NS_TEMP_HOP_DONG objTempHopDong = null;
            List<NS_PHU_CAP> lstPhuCap = null;
            List<NS_DM_CHUC_VU> lstDMChucVu = null;
            List<NS_DM_LOAI_HDLD> lstDMLoaiHdld = null;
            List<NS_DM_THAN_HDLD> lstDMThoiHanHdld = null;
            List<NS_DM_DVI_TGIAN> lstDMDonViThoiGian = null;
            List<NS_DM_HTHUC_TLUONG> lstDMHinhThucTraLuong = null;
            lstDMLoaiPhuCap = new List<NS_DM_PHU_CAP>();
            try
            {
                bool ret = false;
                ret = processNhanSu.HopDong(DatabaseConstant.Action.LOAD, ref obj, ref objTempHopDong, ref lstPhuCap, ref lstDMChucVu, ref lstDMLoaiHdld, ref lstDMThoiHanHdld, ref lstDMDonViThoiGian, ref lstDMHinhThucTraLuong, ref lstDMLoaiPhuCap, ref listClientResponseDetail);
                if (ret == true)
                {
                    lstSourceViTriNguoiDaiDien = ConvertNhanSu.ToAutoCompleteEntry(lstDMChucVu);
                    lstSourceViTriNguoiLaoDong = ConvertNhanSu.ToAutoCompleteEntry(lstDMChucVu);
                    lstSourceLoaiHdld = ConvertNhanSu.ToAutoCompleteEntry(lstDMLoaiHdld);
                    lstSourceThoiHanHdld = ConvertNhanSu.ToAutoCompleteEntry(lstDMThoiHanHdld);
                    lstSourceDonViThoiGian = ConvertNhanSu.ToAutoCompleteEntry(lstDMDonViThoiGian);
                    lstSourceHinhThucTraLuong = ConvertNhanSu.ToAutoCompleteEntry(lstDMHinhThucTraLuong);
                    lstSourceLoaiPhuCap = ConvertNhanSu.ToAutoCompleteEntry(lstDMLoaiPhuCap);

                    AutoComboBox auto = new AutoComboBox();
                    auto.GenAutoComboBoxBySource(ref lstSourceViTriNguoiDaiDien, ref cmbChucVu);
                    auto.GenAutoComboBoxBySource(ref lstSourceViTriNguoiLaoDong, ref cmbChucVuNhanVien);
                    auto.GenAutoComboBoxBySource(ref lstSourceLoaiHdld, ref cmbLoaiHopDong);
                    auto.GenAutoComboBoxBySource(ref lstSourceThoiHanHdld, ref cmbLoaiThoiHan);
                    auto.GenAutoComboBoxBySource(ref lstSourceDonViThoiGian, ref cmbThoiHan);
                    auto.GenAutoComboBoxBySource(ref lstSourceHinhThucTraLuong, ref cmbHinhThucTraLuong);


                    //Phụ cấp
                    foreach (var item in lstDMLoaiPhuCap)
                    {
                        dtLoaiPhuCap.Rows.Add(item.ID, item.TEN);
                    }
                    grdPhuCap.DataContext = dtPhuCap.DefaultView;
                    ((GridViewComboBoxColumn)grdPhuCap.Columns["PHU_CAP"]).ItemsSource = dtLoaiPhuCap.DefaultView;
                }
                else
                {
                    LMessage.ShowMessage("M_ResponseMessage_HanhDong_LayDuLieuKhongThanhCong", LMessage.MessageBoxType.Warning);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                processNhanSu = null;
                listClientResponseDetail = null;            
                objTempHopDong = null;
                lstPhuCap = null;
                lstDMChucVu = null;
                lstDMLoaiHdld = null;
                lstDMThoiHanHdld = null;
                lstDMDonViThoiGian = null;
                lstDMHinhThucTraLuong = null;                
            }
        }

        private void ResetForm()
        {

            #region Người sử dụng lao động
            txtNguoiDaiDien.Text = "";
            txtTenNguoiDaiDien.Text = "";
            cmbChucVu.SelectedIndex = -1;
            txtSoUyQuyen.Text = "";
            #endregion

            #region Người lao động
            txtNhanVien.Text = "";
            txtTenNhanVien.Text = "";
            cmbChucVuNhanVien.SelectedIndex = -1;
            #endregion

            #region Thông tin hợp đồng
            txtSoHopDong.Text = "";
            cmbLoaiHopDong.SelectedIndex = lstSourceLoaiHdld.IndexOf(lstSourceLoaiHdld.FirstOrDefault(i => i.KeywordStrings[0].Equals(BusinessConstant.LOAI_HDLD.CHINH_THUC.layGiaTri())));
            raddtNgayLapHopDong.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            cmbLoaiThoiHan.SelectedIndex = lstSourceThoiHanHdld.IndexOf(lstSourceThoiHanHdld.FirstOrDefault(i => i.KeywordStrings[0].Equals(BusinessConstant.LOAI_THOI_HAN_HDLD.CO_THOI_HAN.layGiaTri())));
            numThoiHan.Value = null;
            cmbThoiHan.SelectedIndex = -1;
            raddtNgayHieuLuc.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            raddtNgayHetHieuLuc.Value = null;
            numSoNgayLam1Tuan.Value = null;
            numSoGioLam1Ngay.Value = null;
            #endregion

            #region Lương
            numLuongCoBan.Value = null;
            numHeSoLuong.Value = null;
            numThanhTien.Value = null;
            numNgayTraLuong.Value = null;
            cmbHinhThucTraLuong.SelectedIndex = 0;
            txtSoTaiKhoan.Text = "";
            #endregion

            #region Thông tin phụ cấp
            dtPhuCap.Rows.Clear();
            grdPhuCap.DataContext = dtPhuCap.DefaultView;
            #endregion

            #region Thông tin kiểm soát
            txtTrangThaiBanGhi.Text = "";
            raddtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            raddtNgayCapNhat.Value = null;
            txtNguoiCapNhat.Text = "";
            #endregion

            #region
            txtNguoiDaiDien.Focus();
            #endregion

        }

        private bool Validation()
        {
            try
            {
                if (txtNguoiDaiDien.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaChon(lblNguoiDaiDien.Content.ToString());
                    txtNguoiDaiDien.Focus();
                    return false;
                }
                if (txtNhanVien.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaChon(lblNhanVien.Content.ToString());
                    txtNhanVien.Focus();
                    return false;
                }
                if (cmbLoaiHopDong.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblLoaiHopDong.Content.ToString());
                    cmbLoaiHopDong.Focus();
                    return false;
                }
                if (raddtNgayLapHopDong.Value == null || raddtNgayLapHopDong.Text.Equals("__/__/____"))
                {
                    CommonFunction.ThongBaoChuaNhap(lblNgayHopDong.Content.ToString());
                    raddtNgayLapHopDong.Focus();
                    return false;
                }

                if (cmbLoaiThoiHan.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblLoaiThoiHan.Content.ToString());
                    cmbLoaiThoiHan.Focus();
                    return false;
                }
                else
                {                    
                    if(loaiThoiHan.Equals(BusinessConstant.LOAI_THOI_HAN_HDLD.CO_THOI_HAN.layGiaTri()))
                    {
                        if (numThoiHan.Value == null)
                        {
                            CommonFunction.ThongBaoChuaNhap(lblThoiHan.Content.ToString());
                            numThoiHan.Focus();
                            return false;
                        }
                        if (cmbThoiHan.SelectedIndex < 0)
                        {
                            CommonFunction.ThongBaoChuaChon(lblThoiHan.Content.ToString());
                            cmbThoiHan.Focus();
                            return false;
                        }
                    }
                }

                if (raddtNgayHieuLuc.Value == null || raddtNgayHieuLuc.Text.Equals("__/__/____"))
                {
                    CommonFunction.ThongBaoChuaNhap(lblNgayHieuLuc.Content.ToString());
                    raddtNgayHieuLuc.Focus();
                    return false;
                }
                if (numLuongCoBan.Value == null)
                {
                    CommonFunction.ThongBaoChuaNhap(lblLuongCoBan.Content.ToString());
                    numLuongCoBan.Focus();
                    return false;
                }
                if (numHeSoLuong.Value == null)
                {
                    CommonFunction.ThongBaoChuaNhap(lblLuongCoBan.Content.ToString());
                    numLuongCoBan.Focus();
                    return false;
                }
                if (numNgayTraLuong.Value == null)
                {
                    CommonFunction.ThongBaoChuaNhap(lblNgayTraLuong.Content.ToString());
                    numNgayTraLuong.Focus();
                    return false;
                }
                if (cmbHinhThucTraLuong.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblHinhThucTraLuong.Content.ToString());
                    cmbHinhThucTraLuong.Focus();
                    return false;
                }
                foreach (DataRow row in dtPhuCap.Rows)
                {
                    if (row["PHU_CAP"].ToString().IsNullOrEmptyOrSpace())
                    {                        
                        CommonFunction.ThongBaoChuaNhap(LLanguage.SearchResourceByKey("U.NhanSu.HopDong.ucHopDongLaoDongCT.PhuCap") + ":");
                        tbiPhuCap.IsSelected = true;
                        return false;
                    }

                    if (row["SO_TIEN"].ToString().IsNullOrEmptyOrSpace() || Convert.ToDecimal(row["SO_TIEN"]) <= 0)
                    {                        
                        CommonFunction.ThongBaoChuaNhap(LLanguage.SearchResourceByKey("U.NhanSu.HopDong.ucHopDongLaoDongCT.SoTien") + ":");
                        tbiPhuCap.IsSelected = true;
                        return false;
                    }
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
                #region Người sử dụng lao động
                txtNguoiDaiDien.IsEnabled = true;
                btnNguoiDaiDien.IsEnabled = true;
                txtTenNguoiDaiDien.IsEnabled = false;
                cmbChucVu.IsEnabled = false;
                txtSoUyQuyen.IsEnabled = true;
                #endregion

                #region Người lao động
                txtNhanVien.IsEnabled = true;
                btnNhanVien.IsEnabled = true;
                txtTenNhanVien.IsEnabled = false;
                cmbChucVuNhanVien.IsEnabled = false;
                #endregion

                #region Thông tin hợp đồng
                txtSoHopDong.IsEnabled = false;
                cmbLoaiHopDong.IsEnabled = true;
                raddtNgayLapHopDong.IsEnabled = true;
                dtpNgayLapHopDong.IsEnabled = true;
                cmbLoaiThoiHan.IsEnabled = true;
                numThoiHan.IsEnabled = true;
                cmbThoiHan.IsEnabled = true;
                raddtNgayHieuLuc.IsEnabled = true;
                dtpNgayHieuLuc.IsEnabled = true;
                raddtNgayHetHieuLuc.IsEnabled = true;
                numSoNgayLam1Tuan.IsEnabled = true;
                numSoGioLam1Ngay.IsEnabled = true;
                txtDienGiai.IsEnabled = true;
                #endregion

                #region Lương
                numLuongCoBan.IsEnabled = true;
                numHeSoLuong.IsEnabled = true;
                numThanhTien.IsEnabled = false;
                numNgayTraLuong.IsEnabled = true;
                cmbHinhThucTraLuong.IsEnabled = true;
                txtSoTaiKhoan.IsEnabled = true;
                #endregion

                #region Thông tin phụ cấp
                btnAdd.IsEnabled = true;
                btnDelete.IsEnabled = true;
                #endregion
            }
            #endregion

            #region Sửa
            else if (action == DatabaseConstant.Action.SUA)
            {
                #region Người sử dụng lao động
                txtNguoiDaiDien.IsEnabled = true;
                btnNguoiDaiDien.IsEnabled = true;
                txtTenNguoiDaiDien.IsEnabled = false;
                cmbChucVu.IsEnabled = false;
                txtSoUyQuyen.IsEnabled = true;
                #endregion

                #region Người lao động
                txtNhanVien.IsEnabled = true;
                btnNhanVien.IsEnabled = true;
                txtTenNhanVien.IsEnabled = false;
                cmbChucVuNhanVien.IsEnabled = false;
                #endregion

                #region Thông tin hợp đồng
                txtSoHopDong.IsEnabled = false;
                cmbLoaiHopDong.IsEnabled = true;
                raddtNgayLapHopDong.IsEnabled = true;
                dtpNgayLapHopDong.IsEnabled = true;
                cmbLoaiThoiHan.IsEnabled = true;
                numThoiHan.IsEnabled = true;
                cmbThoiHan.IsEnabled = true;
                raddtNgayHieuLuc.IsEnabled = true;
                dtpNgayHieuLuc.IsEnabled = true;
                raddtNgayHetHieuLuc.IsEnabled = true;
                numSoNgayLam1Tuan.IsEnabled = true;
                numSoGioLam1Ngay.IsEnabled = true;
                txtDienGiai.IsEnabled = true;
                #endregion

                #region Lương
                numLuongCoBan.IsEnabled = true;
                numHeSoLuong.IsEnabled = true;
                numThanhTien.IsEnabled = false;
                numNgayTraLuong.IsEnabled = true;
                cmbHinhThucTraLuong.IsEnabled = true;
                txtSoTaiKhoan.IsEnabled = true;
                #endregion

                #region Thông tin phụ cấp
                btnAdd.IsEnabled = true;
                btnDelete.IsEnabled = true;
                #endregion

                #region Ngoại lệ

                #endregion

            }
            #endregion

            #region Xem ~ Mặc định
            else
            {
                #region Người sử dụng lao động
                txtNguoiDaiDien.IsEnabled = false;
                btnNguoiDaiDien.IsEnabled = false;
                txtTenNguoiDaiDien.IsEnabled = false;
                cmbChucVu.IsEnabled = false;
                txtSoUyQuyen.IsEnabled = false;
                #endregion

                #region Người lao động
                txtNhanVien.IsEnabled = false;
                btnNhanVien.IsEnabled = false;
                txtTenNhanVien.IsEnabled = false;
                cmbChucVuNhanVien.IsEnabled = false;
                #endregion

                #region Thông tin hợp đồng
                txtSoHopDong.IsEnabled = false;
                cmbLoaiHopDong.IsEnabled = false;
                raddtNgayLapHopDong.IsEnabled = false;
                dtpNgayLapHopDong.IsEnabled = false;
                cmbLoaiThoiHan.IsEnabled = false;
                numThoiHan.IsEnabled = false;
                cmbThoiHan.IsEnabled = false;
                raddtNgayHieuLuc.IsEnabled = false;
                dtpNgayHieuLuc.IsEnabled = false;
                raddtNgayHetHieuLuc.IsEnabled = false;
                numSoNgayLam1Tuan.IsEnabled = false;
                numSoGioLam1Ngay.IsEnabled = false;
                txtDienGiai.IsEnabled = false;
                #endregion

                #region Lương
                numLuongCoBan.IsEnabled = false;
                numHeSoLuong.IsEnabled = false;
                numThanhTien.IsEnabled = false;
                numNgayTraLuong.IsEnabled = false;
                cmbHinhThucTraLuong.IsEnabled = false;
                txtSoTaiKhoan.IsEnabled = false;
                #endregion

                #region Thông tin phụ cấp
                btnAdd.IsEnabled = false;
                btnDelete.IsEnabled = false;
                #endregion

                #region Ngoại lệ
                
                #endregion

            }
            #endregion
        }


        public void OnHold()
        {
            List<NS_PHU_CAP> lstPhuCap = null;
            try
            {
                if (!Validation()) return;

                string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));
                obj = new NS_HOP_DONG();
                //GetFormData(ref obj, ref lstPhuCap, ref lstTrinhDoHocVan, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    //OnAddNew(obj, lstPhuCap, lstTrinhDoHocVan);
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    //OnModify(obj, lstPhuCap, lstTrinhDoHocVan);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                lstPhuCap = null;                
            }
        }

        public void OnSave()
        {
            List<NS_PHU_CAP> lstPhuCap = null;            
            try
            {
                if (!Validation()) return;

                string trangThai = "";
                trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));

                obj = new NS_HOP_DONG();

                GetFormData(ref obj, ref lstPhuCap, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(obj, lstPhuCap);
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(obj, lstPhuCap);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                lstPhuCap = null;                
            }
        }

        public void BeforeViewFromDetail()
        {
            action = DatabaseConstant.Action.XEM;
            SetEnabledControls();            
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain,function);
        }

        public void BeforeViewFromList()
        {
            SetFormData();
            BeforeViewFromDetail();
        }


        public void BeforeAddNew()
        {
            LoadFormData();
            action = DatabaseConstant.Action.THEM;
            ResetForm();
            SetEnabledControls();            
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain,function);
        }

        public void OnAddNew(NS_HOP_DONG obj, List<NS_PHU_CAP> lstPhuCap)
        {
            
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                NhanSuProcess processNhanSu = new NhanSuProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processNhanSu.HopDong(DatabaseConstant.Action.THEM, ref obj, lstPhuCap, ref listClientResponseDetail);
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

        public void AfterAddNew(bool ret, NS_HOP_DONG obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                    if (chkThemNhieuLan.IsChecked == true)
                    {
                        ResetData();
                    }
                    else
                    {
                        id = obj.ID;
                        txtSoHopDong.Text = obj.MA_HOP_DONG;
                        sTrangThaiNVu = obj.TTHAI_NVU;
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

                bool ret = process.LockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_HOP_DONG_CT,
                    DatabaseConstant.Table.NS_HOP_DONG,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                if (ret)
                {
                    action = DatabaseConstant.Action.SUA;
                    SetEnabledControls();                    
                    CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain,function);
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

        public void OnModify(NS_HOP_DONG obj, List<NS_PHU_CAP> lstPhuCap)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                NhanSuProcess processNhanSu = new NhanSuProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processNhanSu.HopDong(DatabaseConstant.Action.SUA,ref obj, lstPhuCap, ref listClientResponseDetail);
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

        public void AfterModify(bool ret, NS_HOP_DONG obj, List<ClientResponseDetail> listClientResponseDetail)
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_HOP_DONG_CT,
                    DatabaseConstant.Table.NS_HOP_DONG,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.NSTL,
                        DatabaseConstant.Function.NS_HOP_DONG_CT,
                        DatabaseConstant.Table.NS_HOP_DONG,
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
                    DatabaseConstant.Function.NS_HOP_DONG_CT,
                    DatabaseConstant.Table.NS_HOP_DONG,
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
            NhanSuProcess processNhanSu = new NhanSuProcess();
            try
            {
                bool ret = false;                
                obj.ID = id;
                ret = processNhanSu.HopDong(action, ref obj, null, ref listClientResponseDetail);
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
                processNhanSu = null;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_HOP_DONG_CT,
                    DatabaseConstant.Table.NS_HOP_DONG,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.NSTL,
                        DatabaseConstant.Function.NS_HOP_DONG_CT,
                        DatabaseConstant.Table.NS_HOP_DONG,
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
            NhanSuProcess processNhanSu = new NhanSuProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processNhanSu.HopDong(action, ref obj, null, ref listClientResponseDetail);
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
                processNhanSu = null;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_HOP_DONG_CT,
                    DatabaseConstant.Table.NS_HOP_DONG,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.NSTL,
                        DatabaseConstant.Function.NS_HOP_DONG_CT,
                        DatabaseConstant.Table.NS_HOP_DONG,
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
            NhanSuProcess processNhanSu = new NhanSuProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processNhanSu.HopDong(action, ref obj, null, ref listClientResponseDetail);
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
                processNhanSu = null;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_HOP_DONG_CT,
                    DatabaseConstant.Table.NS_HOP_DONG,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.NSTL,
                        DatabaseConstant.Function.NS_HOP_DONG_CT,
                        DatabaseConstant.Table.NS_HOP_DONG,
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
            NhanSuProcess processNhanSu = new NhanSuProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processNhanSu.HopDong(action, ref obj, null, ref listClientResponseDetail);
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
                processNhanSu = null;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.NSTL,
                    DatabaseConstant.Function.NS_HOP_DONG_CT,
                    DatabaseConstant.Table.NS_HOP_DONG,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void OnPrint()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(obj) ||
                (!LObject.IsNullOrEmpty(obj) && LObject.IsNullOrEmpty(obj.ID)) ||
                (!LObject.IsNullOrEmpty(obj) && !LObject.IsNullOrEmpty(obj.MA_LOAI_HDLD) && obj.MA_LOAI_HDLD.Equals("CONG_TAC_VIEN")))
            {
                LMessage.ShowMessage("Không có thông tin hợp đồng cần xử lý", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                if (ClientInformation.Company.Equals("M7MFI") || ClientInformation.Company.Equals("BINHKHANH"))
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                    DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();

                    GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                    if (obj.MA_LOAI_HDLD.Equals("HOC_VIEC"))
                        objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.NSTL_HOP_DONG_HOC_VIEC;
                    else if (obj.MA_LOAI_HDLD.Equals("THU_VIEC"))
                        objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.NSTL_HOP_DONG_THU_VIEC;
                    else if (obj.MA_LOAI_HDLD.Equals("CHINH_THUC"))
                        objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.NSTL_HOP_DONG_LAO_DONG;
                    objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.NS_HOP_DONG_CT;
                    NSTL_HOPDONG objNS_HOP_DONG = new NSTL_HOPDONG();
                    objNS_HOP_DONG.HD_ID = obj.ID;
                    objNS_HOP_DONG.HD_SO = obj.MA_HOP_DONG;
                    objNS_HOP_DONG.HD_LOAI = obj.MA_LOAI_HDLD;
                    objNS_HOP_DONG.HD_NGAY_LAP = obj.NGAY_LAP;
                    doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                    doiTuongBaoCao.objNSTL_HOPDONG = objNS_HOP_DONG;
                    xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
                }
                else if (ClientInformation.Company.Equals("BANTAYVANG"))
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();

                    List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
                    listThamSoBaoCao.Add(new ThamSoBaoCao("@HD_ID", obj.ID.ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("@HD_MA", obj.MA_HOP_DONG, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("@TONG_GIAM_DOC", "", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("@GIAM_DOC", "", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.NSTL_HDLD_CHINH_THUC);
                    if (obj.MA_LOAI_HDLD.Equals("HOC_VIEC"))
                        maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.NSTL_HDLD_HOC_VIEC);
                    else if (obj.MA_LOAI_HDLD.Equals("THU_VIEC"))
                        maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.NSTL_HDLD_THU_VIEC);
                    else if (obj.MA_LOAI_HDLD.Equals("CHINH_THUC"))
                        maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.NSTL_HDLD_CHINH_THUC);                  
                    
                    xemBaoCao.LayDuLieu(maBaoCao, listThamSoBaoCao);
                }
                else
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();

                    List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
                    listThamSoBaoCao.Add(new ThamSoBaoCao("@HD_ID", obj.ID.ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("@HD_MA", obj.MA_HOP_DONG, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("@TONG_GIAM_DOC", "", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("@GIAM_DOC", "", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.NSTL_HDLD_CHINH_THUC);
                    if (obj.MA_LOAI_HDLD.Equals("HOC_VIEC"))
                        maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.NSTL_HDLD_HOC_VIEC);
                    else if (obj.MA_LOAI_HDLD.Equals("THU_VIEC"))
                        maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.NSTL_HDLD_THU_VIEC);
                    else if (obj.MA_LOAI_HDLD.Equals("CHINH_THUC"))
                        maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.NSTL_HDLD_CHINH_THUC);

                    xemBaoCao.LayDuLieu(maBaoCao, listThamSoBaoCao);
                }
            }
        }
        #endregion       
        
    }
}
