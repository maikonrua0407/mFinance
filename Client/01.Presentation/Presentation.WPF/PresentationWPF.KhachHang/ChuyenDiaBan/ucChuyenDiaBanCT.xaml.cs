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
using PresentationWPF.CustomControl;
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using Presentation.Process;
using PresentationWPF.KhachHang.KhachHang;
using System.Data;
using Presentation.Process.Common;
using System.Collections;
using System.Reflection;

namespace PresentationWPF.KhachHang.ChuyenDiaBan
{
    /// <summary>
    /// Interaction logic for ucChuyenDiaBan.xaml
    /// </summary>
    public partial class ucChuyenDiaBanCT : UserControl
    {
        #region Khai bao
        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        // Source combobox
        List<AutoCompleteEntry> lstSourceLoaiKH = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKhuVuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNhom = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLyDo = new List<AutoCompleteEntry>();

        private DataTable dtKhachHang = new DataTable();

        public event EventHandler OnSavingCompleted;

        private string tthaiNvu = "";
        public string TthaiNvu
        {
            get { return tthaiNvu; }
            set { tthaiNvu = value; }
        }

        private int _idChuyenDiaBan = -1;

        #endregion

        #region Khoi tao
        public ucChuyenDiaBanCT()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KhachHang;component/ChuyenDiaBan/ucChuyenDiaBanCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            ShowControl();
            BindShortkey();
            InitCombobox();
            KhoiTaoDataSource();

            cmbLoaiKhachHang.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiKhachHang_SelectionChanged);
            cmbChiNhanh.SelectionChanged += new SelectionChangedEventHandler(cmbChiNhanh_SelectionChanged);
            cmbPGD.SelectionChanged += new SelectionChangedEventHandler(cmbPGD_SelectionChanged);
            cmbCum.SelectionChanged += new SelectionChangedEventHandler(cmbCum_SelectionChanged);
            cmbKhuVuc.SelectionChanged += new SelectionChangedEventHandler(cmbKhuVuc_SelectionChanged);
            raddtNgayChuyen.Value = LDateTime.StringToDate(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai, "yyyyMMdd");

            beforeAddNew();
        }

        public ucChuyenDiaBanCT(int id, string tthai, DatabaseConstant.Action action)
        {
            InitializeComponent();
            _idChuyenDiaBan = id;
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KhachHang;component/ChuyenDiaBan/ucChuyenDiaBanCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            ShowControl();
            BindShortkey();
            InitCombobox(); 
            cmbLoaiKhachHang.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiKhachHang_SelectionChanged);
            cmbChiNhanh.SelectionChanged += new SelectionChangedEventHandler(cmbChiNhanh_SelectionChanged);
            cmbPGD.SelectionChanged += new SelectionChangedEventHandler(cmbPGD_SelectionChanged);
            cmbCum.SelectionChanged += new SelectionChangedEventHandler(cmbCum_SelectionChanged);
            cmbKhuVuc.SelectionChanged += new SelectionChangedEventHandler(cmbKhuVuc_SelectionChanged);
            SetFormData();
            beforeModifyFromList(action);
        }

        /// <summary>
        /// Khoi tao combobox
        /// </summary>
        private void InitCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDK = new List<string>();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                //Chi nhanh
                lstDK.Clear();
                lstDK.Add(null);
                lstDK.Add(DatabaseConstant.ToChucDonVi.CNH.getValue());
                lstSourceChiNhanh.Clear();
                auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), lstDK,ClientInformation.MaDonVi);
                cmbChiNhanh_SelectionChanged(null, null);
                cmbPGD_SelectionChanged(null,null);
                

                //Ly do
                lstDK.Clear();
                lstDK.Add(DatabaseConstant.DanhMuc.LY_DO_VAO_RA.getValue());
                lstSourceLyDo.Clear();
                auto.GenAutoComboBox(ref lstSourceLyDo, ref cmbLyDo, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK);

                //Loai KH
                lstDK.Clear();
                lstDK.Add(DatabaseConstant.DanhMuc.LOAI_KHACH_HANG.getValue());
                auto.GenAutoComboBox(ref lstSourceLoaiKH, ref cmbLoaiKhachHang, DatabaseConstant.getValue(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC), lstDK);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void KhoiTaoDataSource()
        {
            try
            {
                KhachHangProcess process = new KhachHangProcess();
                dtKhachHang = process.getThongTinChuyenDiaBan(_idChuyenDiaBan);
                grKhachHangDS.ItemsSource = dtKhachHang.DefaultView;
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.KhachHang.ChuyenDiaBan.ucChuyenDiaBanCT", "");
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
                        key = new KeyBinding(DeleteCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(ModifyCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.Shift);
                        key = new KeyBinding(DeleteCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.A, ModifierKeys.Control | ModifierKeys.Shift);
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
            e.CanExecute = tlbHold.IsEnabled;
        }
        private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onHold();
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbSubmit.IsEnabled;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onSave();
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

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbApprove.IsEnabled;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onApprove();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbRefuse.IsEnabled;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onRefuse();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbCancel.IsEnabled;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onCancel();
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

        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                onHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                onSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                beforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                beforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                beforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                beforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                beforeRefuse();
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

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                onHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                onSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                beforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                beforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                beforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                beforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                beforeRefuse();
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

        #endregion 

        #region Xu ly Giao dien

        private void tlbDeleteThanhVien_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                for (int i = dtKhachHang.Rows.Count - 1; i >= 0; i--)
                {
                    if (Convert.ToBoolean(dtKhachHang.Rows[i]["CHON"]) == true)
                    {
                        dtKhachHang.Rows.RemoveAt(i);
                    }
                }
                for (int i = 0; i < dtKhachHang.Rows.Count; i++)
                {
                    dtKhachHang.Rows[i]["STT"] = i + 1;
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiXoaDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void tlbAddThanhVien_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            try
            {
                string maLoaiKH = lstSourceLoaiKH.ElementAt(cmbLoaiKhachHang.SelectedIndex).KeywordStrings[0];
                Window window = new Window();
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                ucPopupKhachHang uc = new ucPopupKhachHang(true, true, maLoaiKH);
                window.Title = LLanguage.SearchResourceByKey("U.POPUP.POPUP_DS_KHACH_HANG");
                window.Content = uc;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
                //Mouse.OverrideCursor = Cursors.Arrow;
                if (uc.lstData != null && uc.lstData.Count > 0)
                {
                    foreach (DataRowView drKhachHang in uc.lstData)
                    {
                        DataRow drAdd = dtKhachHang.NewRow();
                        dtKhachHang.Rows.Add(drAdd);
                        DataRowView dr = dtKhachHang.DefaultView[dtKhachHang.Rows.Count - 1];
                        dr["CHON"] = false;
                        dr["STT"] = dtKhachHang.Rows.Count;
                        dr["MA_KHANG"] = drKhachHang["MA_KHANG"].ToString();
                        dr["TEN_KHANG"] = drKhachHang["TEN_KHANG"].ToString();
                        dr["TEN_CN_CU"] = drKhachHang["TEN_DVI_CHA"].ToString();
                        dr["TEN_PGD_CU"] = drKhachHang["TEN_DON_VI"].ToString();
                        dr["TEN_CUM"] = drKhachHang["TEN_CUM"].ToString();
                        dr["TEN_NHOM"] = drKhachHang["TEN_NHOM"].ToString();
                        dr["SO_TIEN"] = 0;
                        if (LObject.IsNullOrEmpty(drKhachHang["ID_DVI_CHA"]))
                        {
                            dr["ID_DVI_CN"] = DBNull.Value;
                        }
                        else
                        {
                            dr["ID_DVI_CN"] = Convert.ToInt32(drKhachHang["ID_DVI_CHA"]);
                        }
                        if (LObject.IsNullOrEmpty(drKhachHang["ID_DON_VI"]))
                        {
                            dr["ID_DVI_PGD"] = DBNull.Value;
                        }
                        else
                        {
                            dr["ID_DVI_PGD"] = Convert.ToInt32(drKhachHang["ID_DON_VI"]);
                        }
                        if (LObject.IsNullOrEmpty(drKhachHang["ID_KHU_VUC"]))
                        {
                            dr["ID_KVUC"] = -1;
                        }
                        else
                        {
                            dr["ID_KVUC"] = Convert.ToInt32(drKhachHang["ID_KHU_VUC"]);
                        }
                        if (drKhachHang["ID_CUM"] == null || LString.IsNullOrEmptyOrSpace(drKhachHang["ID_CUM"].ToString()))
                        {
                            dr["ID_CUM"] = DBNull.Value;
                        }
                        else
                        {
                            dr["ID_CUM"] = Convert.ToInt32(drKhachHang["ID_CUM"]);
                        }
                        if (drKhachHang["ID_NHOM"] == null || LString.IsNullOrEmptyOrSpace(drKhachHang["ID_NHOM"].ToString()))
                        {
                            dr["ID_NHOM"] = DBNull.Value;
                        }
                        else
                        {
                            dr["ID_NHOM"] = Convert.ToInt32(drKhachHang["ID_NHOM"]);
                        }
                        dr["MA_DVI_CN"] = drKhachHang["MA_DVI_CHA"].ToString();
                        dr["MA_DVI_PGD"] = drKhachHang["MA_DVI"].ToString();
                        dr["MA_KVUC"] = "";
                        dr["MA_CUM"] = drKhachHang["MA_CUM"].ToString();
                        dr["MA_NHOM"] = drKhachHang["MA_NHOM"].ToString();
                        dr["ID_KHANG_HSO"] = Convert.ToInt32(drKhachHang["ID"]);
                    }

                    #region Tính dư nợ của KH
                    try
                    {
                        string sIdKhang = "(0";
                        foreach (DataRow dr in dtKhachHang.Rows)
                        {
                            sIdKhang = sIdKhang + "," + dr["ID_KHANG_HSO"].ToString();
                        }
                        sIdKhang = sIdKhang + ")";

                        DataSet ds = new TinDungProcess().GetDuNoTheoKH(sIdKhang);
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows) 
                            {
                                foreach (DataRow dr1 in dtKhachHang.Rows)
                                {
                                    if (dr1["ID_KHANG_HSO"].ToString() == dr["ID_KHANG"].ToString())
                                    {
                                        dr1["SO_TIEN"] = dr["SO_DU"];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                    }
                    #endregion
                }
                uc = null;
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
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
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idChuyenDiaBan);

            bool ret = process.UnlockData(DatabaseConstant.Module.KHTV,
                DatabaseConstant.Function.KH_CHUYEN_DIA_BAN,
                DatabaseConstant.Table.KH_CHUYEN_DBAN,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoComboBox au = new AutoComboBox();
            List<string> lstDK = new List<string>();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                AutoCompleteEntry auChiNhanh = au.getEntryByDisplayName(lstSourceChiNhanh, ref cmbChiNhanh);
                if (auChiNhanh != null)
                {
                    lstSourcePGD.Clear();
                    cmbPGD.Items.Clear();
                    lstSourceKhuVuc.Clear();
                    cmbKhuVuc.Items.Clear();
                    lstSourceCum.Clear();
                    cmbCum.Items.Clear();
                    lstSourceNhom.Clear();
                    cmbNhom.Items.Clear();
                    lstDK.Add(auChiNhanh.KeywordStrings[0]);
                    au.GenAutoComboBox(ref lstSourcePGD, ref cmbPGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOMACHA.getValue(), lstDK);
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                au = null;
            }
        }

        private void cmbPGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoComboBox au = new AutoComboBox();
            List<string> lstDK = new List<string>();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                AutoCompleteEntry auPGD = au.getEntryByDisplayName(lstSourcePGD, ref cmbPGD);
                if (auPGD != null)
                {
                    lstSourceKhuVuc.Clear();
                    cmbKhuVuc.Items.Clear();
                    lstSourceCum.Clear();
                    cmbCum.Items.Clear();
                    lstSourceNhom.Clear();
                    cmbNhom.Items.Clear();
                    lstSourceKhuVuc.Add(new AutoCompleteEntry("", new string[2] { "", null }));
                    cmbKhuVuc.Items.Clear();
                    lstDK.Add(auPGD.KeywordStrings[1]);
                    au.GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUC.getValue(), lstDK);
                    if (lstSourceKhuVuc.Count > 1)
                        cmbKhuVuc.IsEnabled = true;
                    else
                        cmbKhuVuc.IsEnabled = false;
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                au = null;
            }
        }

        void cmbKhuVuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoComboBox au = new AutoComboBox();
            List<string> lstDK = new List<string>();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                AutoCompleteEntry auPGD = au.getEntryByDisplayName(lstSourcePGD, ref cmbPGD);
                AutoCompleteEntry auKhuVuc = au.getEntryByDisplayName(lstSourceKhuVuc, ref cmbKhuVuc);
                if (auPGD != null && auKhuVuc != null)
                {
                    lstSourceCum.Clear();
                    cmbCum.Items.Clear();
                    lstSourceNhom.Clear();
                    cmbNhom.Items.Clear();
                    lstSourceCum.Add(new AutoCompleteEntry("", new string[2] { "", null }));
                    cmbCum.Items.Clear();
                    lstDK.Add(auPGD.KeywordStrings[1]);
                    lstDK.Add(auKhuVuc.KeywordStrings[1]);
                    au.GenAutoComboBox(ref lstSourceCum, ref cmbCum, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM.getValue(), lstDK);
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                au = null;
            }
        }

        private void cmbCum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoComboBox au = new AutoComboBox();
            List<string> lstDK = new List<string>();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                AutoCompleteEntry auPGD = au.getEntryByDisplayName(lstSourcePGD, ref cmbPGD);
                AutoCompleteEntry auCum = au.getEntryByDisplayName(lstSourceCum, ref cmbCum);
                if (auPGD != null && auCum != null)
                {
                    lstSourceNhom.Clear();
                    cmbNhom.Items.Clear();
                    lstSourceNhom.Add(new AutoCompleteEntry("", new string[2] { "", null }));
                    cmbNhom.Items.Clear();
                    lstDK.Add(auPGD.KeywordStrings[1]);
                    lstDK.Add(null);
                    lstDK.Add(auCum.KeywordStrings[1]);
                    au.GenAutoComboBox(ref lstSourceNhom, ref cmbNhom, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NHOM.getValue(), lstDK);
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                au = null;
            }
        }

        /// <summary>
        /// Sự kiện chọn loại khác hàng
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbLoaiKhachHang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoComboBox au = new AutoComboBox();
            string maLoaiKH = au.getEntryByDisplayName(lstSourceLoaiKH, ref cmbLoaiKhachHang).KeywordStrings[0];
            //
            while (dtKhachHang.Rows.Count > 0)
                dtKhachHang.Rows.RemoveAt(dtKhachHang.Rows.Count - 1);
            grKhachHangDS.ItemsSource = dtKhachHang.DefaultView;
        }

        private void SetEnabledAllControls(bool enable)
        {
            grbThongTinChung.IsEnabled = enable;
            grbThongTinCT.IsEnabled = enable;
            grbKhachHangDS.IsEnabled = enable;
            tlbAddThanhVien.IsEnabled = enable;
            tlbDeleteThanhVien.IsEnabled = enable;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            raddtNgayChuyen.Focus();
        }

        private void chkKhachHangDS_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < grKhachHangDS.Items.Count; i++)
            {
                DataRowView dr = (DataRowView)grKhachHangDS.Items[i];
                dr["CHON"] = chkKhachHangDS.IsChecked;
            }
        }
        #endregion              
 
        #region Xu ly nghiep vu
        private void beforeView()
        {
            SetFormData();
            SetEnabledAllControls(false);
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu);
        }

        private void beforeAddNew()
        {
            //XuLyGiaoDien("CNHAN");
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu);
            lblTrangThai.Content = "";
            raddtNgayNhap.Value = null;
            raddtNgayCNhat.Value = null;
        }

        private void beforeModifyFromList(DatabaseConstant.Action action)
        {
            if (action == DatabaseConstant.Action.XEM)
            {
                SetEnabledAllControls(true);
            }
            CommonFunction.RefreshButton(Toolbar, action, tthaiNvu);
        }

        private void beforeModifyFromDetail()
        {
            // Yêu cầu lock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idChuyenDiaBan);

            bool ret = process.LockData(DatabaseConstant.Module.KHTV,
                DatabaseConstant.Function.KH_CHUYEN_DIA_BAN,
                DatabaseConstant.Table.KH_CHUYEN_DBAN,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Nếu lock thành công >> cho phép sửa
            if (ret)
            {
                SetEnabledAllControls(true);
                SetFormData();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu);
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
                listLockId.Add(_idChuyenDiaBan);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.KHTV,
                    DatabaseConstant.Function.KH_CHUYEN_DIA_BAN,
                    DatabaseConstant.Table.KH_CHUYEN_DBAN,
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
                listLockId.Add(_idChuyenDiaBan);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.KHTV,
                    DatabaseConstant.Function.KH_CHUYEN_DIA_BAN,
                    DatabaseConstant.Table.KH_CHUYEN_DBAN,
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
                listLockId.Add(_idChuyenDiaBan);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.KHTV,
                    DatabaseConstant.Function.KH_CHUYEN_DIA_BAN,
                    DatabaseConstant.Table.KH_CHUYEN_DBAN,
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
                listLockId.Add(_idChuyenDiaBan);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.KHTV,
                    DatabaseConstant.Function.KH_CHUYEN_DIA_BAN,
                    DatabaseConstant.Table.KH_CHUYEN_DBAN,
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
        /// Lưu dữ liệu (hay trình duyệt)
        /// </summary>
        private void onSave()
        {
            string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.TRINH_DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            if (Validation())
            {
                KhachHangProcess process = new KhachHangProcess();
                try
                {
                    Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN obj = new Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN();
                    List<Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN_CTIET> lstChiTiet = new List<Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN_CTIET>();
                    // Dữ liệu truyền vào và dữ liệu trả về
                    Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN ret = new Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN();
                    Mouse.OverrideCursor = Cursors.Wait;
                    // Nếu là lưu tạm hoặc thêm mới lần đầu
                    if (_idChuyenDiaBan == -1)
                    {
                        List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref lstChiTiet, trangThai);
                        ret = process.ThemGDChuyenDiaBan(obj, lstChiTiet, ref lstResponseDetail);
                        _idChuyenDiaBan = ret.ID;
                        Mouse.OverrideCursor = Cursors.Arrow;
                        afterAddNew(ret, lstResponseDetail);
                    }
                    // Nếu là lưu tạm khi sửa
                    // Hoặc lưu tạm khi sửa sau duyệt
                    // Hoặc sửa
                    else
                    {
                        // Lấy dữ liệu từ form
                        GetFormData(ref obj, ref lstChiTiet, trangThai);
                        List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                        ret = process.SuaGDChuyenDiaBan(obj, lstChiTiet, DatabaseConstant.Action.SUA, ref lstResponseDetail);
                        _idChuyenDiaBan = ret.ID;
                        Mouse.OverrideCursor = Cursors.Arrow;
                        afterModify(ret, lstResponseDetail);
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
                    process = null;
                }
            }
        }

        /// <summary>
        /// Lưu tạm dữ liệu
        /// </summary>
        private void onHold()
        {
            string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            KhachHangProcess process = new KhachHangProcess();
            try
            {
                Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN obj = new Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN();
                List<Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN_CTIET> lstChiTiet = new List<Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN_CTIET>();
                // Dữ liệu truyền vào và dữ liệu trả về
                Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN ret = new Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN();
                Mouse.OverrideCursor = Cursors.Wait;
                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                // Nếu là lưu tạm hoặc thêm mới lần đầu
                if (_idChuyenDiaBan == -1)
                {
                    // Lấy dữ liệu từ form
                    GetFormData(ref obj, ref lstChiTiet, trangThai);
                    ret = process.ThemGDChuyenDiaBan(obj, lstChiTiet, ref lstResponseDetail);
                    _idChuyenDiaBan = ret.ID;
                    Mouse.OverrideCursor = Cursors.Arrow;
                    afterAddNew(ret, lstResponseDetail);
                }
                // Nếu là lưu tạm khi sửa
                // Hoặc lưu tạm khi sửa sau duyệt
                // Hoặc sửa
                else
                {
                    // Lấy dữ liệu từ form
                    GetFormData(ref obj, ref lstChiTiet, trangThai);
                    ret = process.SuaGDChuyenDiaBan(obj, lstChiTiet, DatabaseConstant.Action.SUA, ref lstResponseDetail);
                    _idChuyenDiaBan = ret.ID;
                    Mouse.OverrideCursor = Cursors.Arrow;
                    afterModify(ret, lstResponseDetail);
                }
            }
            catch (System.Exception ex)
            {
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
        /// Xóa dữ liệu
        /// </summary>
        private void onDelete()
        {

            KhachHangProcess process = new KhachHangProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = _idChuyenDiaBan;

                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                ApplicationConstant.ResponseStatus ret = process.XoaGDChuyenDiaBan(arrayID, ref listClientResponseDetail);
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                afterDelete(ret);
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;

        }

        /// <summary>
        /// Duyệt dữ liệu
        /// </summary>
        private void onApprove()
        {

            KhachHangProcess process = new KhachHangProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = _idChuyenDiaBan;

                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                ApplicationConstant.ResponseStatus ret = process.DuyetGDChuyenDiaBan(arrayID, ref listClientResponseDetail);
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                afterApprove(ret);
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Thoái duyệt dữ liệu
        /// </summary>
        private void onCancel()
        {

            KhachHangProcess process = new KhachHangProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = _idChuyenDiaBan;

                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                ApplicationConstant.ResponseStatus ret = process.ThoaiDuyetGDChuyenDiaBan(arrayID, ref listClientResponseDetail);
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                afterCancel(ret);
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;

        }

        /// <summary>
        /// Từ chối duyệt dữ liệu
        /// </summary>
        private void onRefuse()
        {

            KhachHangProcess process = new KhachHangProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = _idChuyenDiaBan;

                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                ApplicationConstant.ResponseStatus ret = process.TuChoiGDChuyenDiaBan(arrayID, ref listClientResponseDetail);
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                afterRefuse(ret, CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.TU_CHOI_DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu)));
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;

        }

        /// <summary>
        /// Sau khi thêm mới
        /// </summary>
        /// <param name="ret"></param>
        private void afterAddNew(Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN ret,List<ClientResponseDetail> lstResponseDetail)
        {
            if (ret != null)
            {

                CommonFunction.ThongBaoKetQua(lstResponseDetail);
                if (ret.ID > 0)
                {
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(ret.TTHAI_NVU);
                    txtNguoiLap.Text = ret.NGUOI_NHAP;
                    raddtNgayNhap.Value = LDateTime.StringToDate(ret.NGAY_NHAP, "yyyyMMdd");
                    tthaiNvu = ret.TTHAI_NVU;
                    txtSoGD.Text = ret.MA_GDICH;
                    SetEnabledAllControls(false);
                    if (cbMultiAdd.IsChecked == true)
                    {
                        SetEnabledAllControls(true);
                        ResetForm();
                    }
                    else if (!DatabaseConstant.CLOSE_DETAIL_FORM)
                    {
                        CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu);
                    }
                    else
                    {
                        onClose();
                    }
                }
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.ThemKhongThanhCong", LMessage.MessageBoxType.Error);
            }
        }

        /// <summary>
        /// Sau khi sửa
        /// </summary>
        /// <param name="ret"></param>
        private void afterModify(Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN ret, List<ClientResponseDetail> lstResponseDetail)
        {
            if (ret != null)
            {
                CommonFunction.ThongBaoKetQua(lstResponseDetail);
                if (ret.ID > 0)
                {
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(ret.TTHAI_NVU);
                    txtNguoiLap.Text = ret.NGUOI_NHAP;
                    raddtNgayNhap.Value = LDateTime.StringToDate(ret.NGAY_NHAP, "yyyyMMdd");
                    tthaiNvu = ret.TTHAI_NVU;
                    txtSoGD.Text = ret.MA_GDICH;
                    txtNguoiCapNhat.Text = ret.NGUOI_CNHAT;
                    raddtNgayCNhat.Value = LDateTime.StringToDate(ret.NGAY_CNHAT, "yyyyMMdd");
                    SetEnabledAllControls(false);
                    if (cbMultiAdd.IsChecked == true)
                    {
                        SetEnabledAllControls(true);
                        ResetForm();
                    }
                    else if (!DatabaseConstant.CLOSE_DETAIL_FORM)
                    {
                        CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu);
                    }
                    else
                    {
                        onClose();
                    }
                }
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.CapNhatKoThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(ret.ID);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                DatabaseConstant.Function.KH_CHUYEN_DIA_BAN,
                DatabaseConstant.Table.KH_CHUYEN_DBAN,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Sau khi xóa
        /// </summary>
        /// <param name="ret"></param>
        private void afterDelete(ApplicationConstant.ResponseStatus ret)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
            }
            else
            {
                LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idChuyenDiaBan);
            List<int> listUnlockId = new List<int>();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                DatabaseConstant.Function.KH_CHUYEN_DIA_BAN,
                DatabaseConstant.Table.KH_CHUYEN_DBAN,
                DatabaseConstant.Action.SUA,
                listLockId);
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                onClose();
            }
            else
            {
                
            }
            // Đóng cửa sổ chi tiết sau khi xóa
            
        }

        /// <summary>
        /// Sau khi duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void afterApprove(ApplicationConstant.ResponseStatus ret)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {


                TthaiNvu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(TthaiNvu);

                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu);
                SetEnabledAllControls(false);
            }
            else
            {
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idChuyenDiaBan);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                DatabaseConstant.Function.KH_CHUYEN_DIA_BAN,
                DatabaseConstant.Table.KH_CHUYEN_DBAN,
                DatabaseConstant.Action.DUYET,
                listLockId);
        }

        /// <summary>
        /// Sau khi thoái duyệt
        /// </summary>
        /// <param name="ret"></param>
        private void afterCancel(ApplicationConstant.ResponseStatus ret)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {

                tthaiNvu = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);

                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu);
                SetEnabledAllControls(false);
            }
            else
            {
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idChuyenDiaBan);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                DatabaseConstant.Function.KH_CHUYEN_DIA_BAN,
                DatabaseConstant.Table.KH_CHUYEN_DBAN,
                DatabaseConstant.Action.THOAI_DUYET,
                listLockId);
        }

        /// <summary>
        /// Sau khi từ chối
        /// </summary>
        /// <param name="ret"></param>
        private void afterRefuse(ApplicationConstant.ResponseStatus ret,string trangThai)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {

                tthaiNvu = trangThai;
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(trangThai);
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(trangThai);

                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu);
                SetEnabledAllControls(false);
            }
            else
            {

            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_idChuyenDiaBan);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                DatabaseConstant.Function.KH_CHUYEN_DIA_BAN,
                DatabaseConstant.Table.KH_CHUYEN_DBAN,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                listLockId);
        }

        /// <summary>
        /// Kiểm tra dữ liệu trước khi lưu
        /// </summary>
        /// <returns></returns>
        private bool Validation()
        {
            AutoComboBox au = new AutoComboBox();
            AutoCompleteEntry auLoaiKH = au.getEntryByDisplayName(lstSourceLoaiKH, ref cmbLoaiKhachHang);
            AutoCompleteEntry auChiNhanh = au.getEntryByDisplayName(lstSourceChiNhanh, ref cmbChiNhanh);
            AutoCompleteEntry auPGD = au.getEntryByDisplayName(lstSourcePGD, ref cmbPGD);
            AutoCompleteEntry auCum = au.getEntryByDisplayName(lstSourceCum, ref cmbCum);
            AutoCompleteEntry auNhom = au.getEntryByDisplayName(lstSourceNhom, ref cmbNhom);
            AutoCompleteEntry auLyDo = au.getEntryByDisplayName(lstSourceLyDo, ref cmbLyDo);
            if (raddtNgayChuyen.Value == null)
            {
                CommonFunction.ThongBaoTrong(lblNgayChuyen.Content.ToString());
                raddtNgayChuyen.Focus();
                return false;
            }
            else if (auChiNhanh.KeywordStrings[0].IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblMaChiNhanh.Content.ToString());
                cmbChiNhanh.Focus();
                return false;
            }
            else if (auPGD.KeywordStrings[0].IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblMaPhongGD.Content.ToString());
                cmbPGD.Focus();
                return false;
            }
            else if (auLoaiKH.KeywordStrings[0].Equals(BusinessConstant.LoaiKhachHang.TVIEN.layGiaTri()) && auCum.KeywordStrings[0].IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblMaCum.Content.ToString());
                cmbCum.Focus();
                return false;
            }
            else if (auLoaiKH.KeywordStrings[0].Equals(BusinessConstant.LoaiKhachHang.TVIEN.layGiaTri()) && cmbNhom.Visibility==Visibility.Visible && auNhom.KeywordStrings[0].IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblMaNhom.Content.ToString());
                cmbNhom.Focus();
                return false;
            }
            else if (auLyDo == null)
            {
                CommonFunction.ThongBaoTrong(lblLyDo.Content.ToString());
                cmbLyDo.Focus();
                return false;
            }
            else if (grKhachHangDS.Items.Count == 0)
            {
                LMessage.ShowMessage("M.KhachHang.ChuyenDiaBan.ucChuyenDiaBanCT.LoiKhongCoKhachHang", LMessage.MessageBoxType.Warning);
                return false;
            }

            foreach (DataRow dr in dtKhachHang.Rows)
            {
                if (Convert.ToDecimal(dr["SO_TIEN"]) > 0)
                {
                    LMessage.ShowMessage("Khách hàng vẫn còn dư nợ", LMessage.MessageBoxType.Warning);
                    ApplicationConstant.DonViSuDung CompanyCode = ApplicationConstant.layDonViSuDung(ClientInformation.Company);
                    switch (CompanyCode)
                    {
                        case ApplicationConstant.DonViSuDung.BINHKHANH:
                            return false;
                        default:
                            MessageBoxResult result = LMessage.ShowMessage("Khách hàng vẫn còn dư nợ. Tiếp tục hay không?", LMessage.MessageBoxType.Question);
                            if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
                            {
                                return false;
                            }
                            else
                                return true;
                    }
                    
                }
            }

            return true;
        }

        private void SetFormData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            KhachHangProcess process = new KhachHangProcess();
            AutoComboBox au = new AutoComboBox();
            try
            {
                dtKhachHang = process.getThongTinChuyenDiaBan(_idChuyenDiaBan);
                if (dtKhachHang != null && dtKhachHang.Rows.Count > 0)
                {
                    txtSoGD.Text = dtKhachHang.Rows[0]["MA_GDICH"].ToString();
                    raddtNgayChuyen.Value = LDateTime.StringToDate(dtKhachHang.Rows[0]["NGAY_CHUYEN"].ToString(), "yyyyMMdd");
                    if (!LString.IsNullOrEmptyOrSpace(dtKhachHang.Rows[0]["MA_CN_MOI"].ToString()))
                    {
                        cmbChiNhanh.SelectedIndex = lstSourceChiNhanh.IndexOf(lstSourceChiNhanh.FirstOrDefault(f => f.KeywordStrings.First().Equals(dtKhachHang.Rows[0]["MA_CN_MOI"].ToString())));
                    }
                    else
                    {
                        cmbChiNhanh.SelectedIndex = -1;
                    }
                    if (!LString.IsNullOrEmptyOrSpace(dtKhachHang.Rows[0]["MA_PGD_MOI"].ToString()))
                    {
                        cmbPGD.SelectedIndex = lstSourcePGD.IndexOf(lstSourcePGD.FirstOrDefault(f => f.KeywordStrings.First().Equals(dtKhachHang.Rows[0]["MA_PGD_MOI"].ToString())));
                    }
                    else
                    {
                        cmbPGD.SelectedIndex = -1;
                    }
                    if (!LString.IsNullOrEmptyOrSpace(dtKhachHang.Rows[0]["MA_KVUC_MOI"].ToString()))
                    {
                        cmbKhuVuc.SelectedIndex = lstSourceKhuVuc.IndexOf(lstSourceKhuVuc.FirstOrDefault(f => f.KeywordStrings.First().Equals(dtKhachHang.Rows[0]["MA_KVUC_MOI"].ToString())));
                    }
                    else
                    {
                        cmbPGD.SelectedIndex = -1;
                    }
                    if (!LString.IsNullOrEmptyOrSpace(dtKhachHang.Rows[0]["MA_CUM_MOI"].ToString()))
                    {
                        cmbCum.SelectedIndex = lstSourceCum.IndexOf(lstSourceCum.FirstOrDefault(f => f.KeywordStrings.First().Equals(dtKhachHang.Rows[0]["MA_CUM_MOI"].ToString())));
                    }
                    else
                    {
                        cmbCum.SelectedIndex = -1;
                    }
                    if (!LString.IsNullOrEmptyOrSpace(dtKhachHang.Rows[0]["MA_NHOM_MOI"].ToString()))
                    {
                        cmbNhom.SelectedIndex = lstSourceNhom.IndexOf(lstSourceNhom.FirstOrDefault(f => f.KeywordStrings.First().Equals(dtKhachHang.Rows[0]["MA_NHOM_MOI"].ToString())));
                    }
                    else
                    {
                        cmbNhom.SelectedIndex = -1;
                    }
                    if (!LString.IsNullOrEmptyOrSpace(dtKhachHang.Rows[0]["LY_DO"].ToString()))
                    {
                        cmbLyDo.SelectedIndex = lstSourceLyDo.IndexOf(lstSourceLyDo.FirstOrDefault(f => f.KeywordStrings.First().Equals(dtKhachHang.Rows[0]["LY_DO"].ToString())));
                    }
                    else
                    {
                        cmbLyDo.SelectedIndex = -1;
                    }

                    tthaiNvu = dtKhachHang.Rows[0]["TTHAI_NVU"].ToString();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(dtKhachHang.Rows[0]["TTHAI_NVU"].ToString());
                    txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(dtKhachHang.Rows[0]["TTHAI_NVU"].ToString());
                    txtNguoiLap.Text = BusinessConstant.layNgonNguNghiepVu(dtKhachHang.Rows[0]["NGUOI_NHAP"].ToString());
                    raddtNgayNhap.Value = LDateTime.StringToDate(dtKhachHang.Rows[0]["NGAY_NHAP"].ToString(), "yyyyMMdd");
                    if (!LString.IsNullOrEmptyOrSpace(dtKhachHang.Rows[0]["NGUOI_CNHAT"].ToString()))
                    {
                        txtNguoiCapNhat.Text = dtKhachHang.Rows[0]["NGUOI_CNHAT"].ToString();
                    }

                    if (!LString.IsNullOrEmptyOrSpace(dtKhachHang.Rows[0]["NGAY_CNHAT"].ToString()))
                    {
                        raddtNgayCNhat.Value = LDateTime.StringToDate(dtKhachHang.Rows[0]["NGAY_CNHAT"].ToString(), "yyyyMMdd");
                    }
                    else
                    {
                        raddtNgayCNhat.Value = null;
                    }
                    grKhachHangDS.ItemsSource = dtKhachHang.DefaultView;
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void GetFormData(ref Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN obj,ref List<Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN_CTIET> lstChiTiet,string tthai)
        {
            AutoComboBox au = new AutoComboBox();
            AutoCompleteEntry auChiNhanh = au.getEntryByDisplayName(lstSourceChiNhanh, ref cmbChiNhanh);
            AutoCompleteEntry auPGD = au.getEntryByDisplayName(lstSourcePGD, ref cmbPGD);
            AutoCompleteEntry auKVuc = au.getEntryByDisplayName(lstSourceKhuVuc, ref cmbKhuVuc);
            AutoCompleteEntry auCum = au.getEntryByDisplayName(lstSourceCum, ref cmbCum);
            AutoCompleteEntry auNhom = au.getEntryByDisplayName(lstSourceNhom, ref cmbNhom);
            AutoCompleteEntry auLyDo = au.getEntryByDisplayName(lstSourceLyDo, ref cmbLyDo);
            // Lấy dữ liệu KH_CHUYEN_DBAN
            if (obj == null)
            {
                obj = new Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN();
            }
            else
            {
                obj.ID = _idChuyenDiaBan;
            }
            obj.ID_DVI_CN = Convert.ToInt32(auChiNhanh.KeywordStrings[1]);
            obj.MA_DVI_CN = auChiNhanh.KeywordStrings[0];
            obj.ID_DVI_PGD = Convert.ToInt32(auPGD.KeywordStrings[1]);
            obj.MA_DVI_PGD = auPGD.KeywordStrings[0];
            if (LObject.IsNullOrEmpty(auKVuc) || LObject.IsNullOrEmpty(auKVuc.KeywordStrings[1]))
            {
                obj.ID_KVUC = -1;
                obj.MA_KVUC = "";
            }
            else
            {
                obj.ID_KVUC = Convert.ToInt32(auKVuc.KeywordStrings[1]);
                obj.MA_KVUC = auKVuc.KeywordStrings[0];
            }
            if (LObject.IsNullOrEmpty(auKVuc) || LObject.IsNullOrEmpty(auKVuc.KeywordStrings[1]))
            {
                obj.ID_CUM = -1;
                obj.MA_CUM = "";
            }
            else
            {
                obj.ID_CUM = Convert.ToInt32(auCum.KeywordStrings[1]);
                obj.MA_CUM = auCum.KeywordStrings[0];
            }
            if (LObject.IsNullOrEmpty(auNhom) || LObject.IsNullOrEmpty(auNhom.KeywordStrings[1]))
            {
                obj.ID_NHOM = -1;
                obj.MA_NHOM = "";
            }
            else
            {
                obj.ID_NHOM = Convert.ToInt32(auNhom.KeywordStrings[1]);
                obj.MA_NHOM = auNhom.KeywordStrings[0];
            }
            
            obj.LY_DO = auLyDo.KeywordStrings[0];
            obj.TTHAI_TTINH = "HT";
            if (raddtNgayChuyen.Value != null)
            {
                obj.NGAY_CHUYEN = LDateTime.DateToString(Convert.ToDateTime(raddtNgayChuyen.Value), "yyyyMMdd");
            }
            else
            {
                obj.NGAY_CHUYEN = "";
            }
            obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
            obj.TTHAI_NVU = tthai;
            
            if (_idChuyenDiaBan < 1)
            {
                obj.MA_DVI_QLY = Presentation.Process.Common.ClientInformation.MaDonVi;
                obj.MA_DVI_TAO = Presentation.Process.Common.ClientInformation.MaDonViGiaoDich;
                obj.NGAY_NHAP = Presentation.Process.Common.ClientInformation.NgayLamViecHienTai;
                obj.NGUOI_NHAP = Presentation.Process.Common.ClientInformation.TenDangNhap;
            }
            else
            {
                obj.NGAY_NHAP = LDateTime.DateToString(Convert.ToDateTime(raddtNgayNhap.Value), "yyyyMMdd");
                obj.NGUOI_NHAP = txtNguoiLap.Text;
                obj.NGAY_CNHAT = Presentation.Process.Common.ClientInformation.NgayLamViecHienTai;
                obj.NGUOI_CNHAT = Presentation.Process.Common.ClientInformation.TenDangNhap;
            }
            // Lấy dữ liệu KH_CHUYEN_DBAN_CTIET
            if (lstChiTiet == null)
            {
                lstChiTiet = new List<Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN_CTIET>();
            }
            foreach (DataRow dr in dtKhachHang.Rows)
            {
                Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN_CTIET objCT = new Presentation.Process.KhachHangServiceRef.KH_CHUYEN_DBAN_CTIET();
                if(dr["ID_CUM"]!=null && !LString.IsNullOrEmptyOrSpace(dr["ID_CUM"].ToString()))
                {
                    objCT.ID_CUM = Convert.ToInt32(dr["ID_CUM"]);
                }
                else
                {
                    objCT.ID_CUM = -1;
                }
                if (dr["ID_DVI_CN"] != null && !LString.IsNullOrEmptyOrSpace(dr["ID_DVI_CN"].ToString()))
                {
                    objCT.ID_DVI_CN = Convert.ToInt32(dr["ID_DVI_CN"]);
                }
                else
                {
                    objCT.ID_DVI_CN = -1;
                }
                if (dr["ID_DVI_PGD"] != null && !LString.IsNullOrEmptyOrSpace(dr["ID_DVI_PGD"].ToString()))
                {
                    objCT.ID_DVI_PGD = Convert.ToInt32(dr["ID_DVI_PGD"]);
                }
                else
                {
                    objCT.ID_DVI_PGD = -1;
                }
                if (dr["ID_KVUC"] != null && !LString.IsNullOrEmptyOrSpace(dr["ID_KVUC"].ToString()))
                {
                    objCT.ID_KVUC = Convert.ToInt32(dr["ID_KVUC"]);
                }
                else
                {
                    objCT.ID_KVUC = -1;
                }
                if (dr["ID_NHOM"] != null && !LString.IsNullOrEmptyOrSpace(dr["ID_NHOM"].ToString()))
                {
                    objCT.ID_NHOM = Convert.ToInt32(dr["ID_NHOM"]);
                }
                else
                {
                    objCT.ID_NHOM = -1;
                }
                
                objCT.ID_KHANG_HSO = Convert.ToInt32(dr["ID_KHANG_HSO"]);
                objCT.LY_DO = auLyDo.KeywordStrings[0];
                objCT.MA_CUM = dr["MA_CUM"].ToString();
                objCT.MA_DVI_CN = dr["MA_DVI_CN"].ToString();
                objCT.MA_DVI_PGD = dr["MA_DVI_PGD"].ToString();
                objCT.MA_KHANG = dr["MA_KHANG"].ToString();
                objCT.MA_KVUC = auKVuc.KeywordStrings[0];
                objCT.MA_NHOM = dr["MA_NHOM"].ToString();
                objCT.NGAY_CHUYEN = Convert.ToDateTime(raddtNgayChuyen.Value).ToString("yyyyMMdd");
                objCT.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                objCT.TTHAI_NVU = tthai;
                objCT.MA_DVI_QLY = Presentation.Process.Common.ClientInformation.MaDonVi;
                objCT.MA_DVI_TAO = Presentation.Process.Common.ClientInformation.MaDonVi;
                objCT.NGAY_NHAP = obj.NGAY_NHAP;
                objCT.NGUOI_NHAP = obj.NGUOI_NHAP;
                objCT.NGAY_CNHAT = obj.NGAY_CNHAT;
                objCT.NGUOI_CNHAT = obj.NGUOI_CNHAT;
                lstChiTiet.Add(objCT);
            }
        }

        private void ResetForm()
        {
            txtSoGD.Text = "";
            raddtNgayChuyen.Value = LDateTime.StringToDate(Presentation.Process.Common.ClientInformation.NgayLamViecHienTai);
            cmbChiNhanh.SelectedIndex = 0;
            cmbPGD.SelectedIndex = 0;
            cmbCum.SelectedIndex = 0;
            cmbNhom.SelectedIndex = 0;
            cmbLyDo.SelectedIndex = 0;
            grKhachHangDS.Items.Clear();
            _idChuyenDiaBan = -1;
            tthaiNvu = "";
            txtNguoiLap.Text = "";
            txtNguoiCapNhat.Text = "";
            raddtNgayNhap.Value = null;
            raddtNgayCNhat.Value = null;
        }
        #endregion

    }
}
