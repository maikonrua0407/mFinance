﻿using System;
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
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.Common;
using PresentationWPF.CustomControl;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.HuyDongVonServiceRef;
using Presentation.Process.LaiSuatServiceRef;
using Presentation.Process.KhachHangServiceRef;
using Telerik.Windows.Controls;
using PresentationWPF.HuyDongVon.Popup;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;

namespace PresentationWPF.HuyDongVon.RutGoc
{
    /// <summary>
    /// Interaction logic for ucRutGocTheoDS.xaml
    /// </summary>
    public partial class ucRutGocTheoDS : UserControl
    {
        #region Khai bao

        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private DatabaseConstant.Function function = DatabaseConstant.Function.HDV_RUT_GOC_THEO_DANH_SACH;
        
        public event EventHandler OnSavingCompleted;

        private DataSet dsRutGoc;

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private string maGiaoDich = "";

        private KIEM_SOAT _objKiemSoat = null;

        private string sTrangThaiNVu = "";

        private HDV_RUT_GOC_THEO_DANH_SACH obj;
        public HDV_RUT_GOC_THEO_DANH_SACH Obj
        {
            get { return obj; }
            set { obj = value; }
        }

        private bool isTinhLai = false;

        private List<THONG_TIN_TIEN_LAI> lstThongTinLai = null;

        List<AutoCompleteEntry> lstSourceGD_HinhThuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCBQL = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand ExportCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        #endregion        

        #region Khoi tao
        public ucRutGocTheoDS()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            LoadCombobox();

            KhoiTaoGridRutGoc();

            radpage.PageSize = (int)nudPageSize.Value;
            
            HideControl();

            btnCalculate.IsEnabled = false;
        }

        public ucRutGocTheoDS(KIEM_SOAT obj)
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            _objKiemSoat = obj;
            sTrangThaiNVu = obj.TTHAI_NVU;

            BindShortkey();

            LoadCombobox();

            KhoiTaoGridRutGoc();

            radpage.PageSize = (int)nudPageSize.Value;

            action = obj.action;

            maGiaoDich = obj.SO_GIAO_DICH;

            this.obj = new HDV_RUT_GOC_THEO_DANH_SACH();

            HideControl();
        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.HuyDongVon;component/RutGoc/ucRutGocTheoDS.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void LoadCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();            
            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.HINH_THUC_GIAO_DICH));
            auto.GenAutoComboBox(ref lstSourceGD_HinhThuc, ref cmbGD_HinhThuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri());

            lstDieuKien.Clear();
            lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
            auto.GenAutoComboBox(ref lstSourceCBQL, ref cmbMaCBQL, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NSD.getValue(), lstDieuKien);
            cmbMaCBQL.SelectedIndex = lstSourceCBQL.IndexOf(lstSourceCBQL.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TenDangNhap)));

            auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TIENTE.getValue(), null, ClientInformation.MaDongNoiTe);

        }

        private void KhoiTaoGridRutGoc()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("SO_SO_TG", typeof(string));
            dt.Columns.Add("ID_CUM", typeof(string));
            dt.Columns.Add("TEN_CUM", typeof(string));
            dt.Columns.Add("MA_KHANG", typeof(string));
            dt.Columns.Add("TEN_KHANG", typeof(string));            
            dt.Columns.Add("NGAY_MO_SO", typeof(string));
            dt.Columns.Add("NGAY_DEN_HAN", typeof(string));
            dt.Columns.Add("KY_HAN", typeof(string));
            dt.Columns.Add("SO_DU", typeof(decimal));
            dt.Columns.Add("LAI_SUAT", typeof(decimal));            
            dt.Columns.Add("SO_TIEN_RUT_GOC", typeof(decimal));
            dt.Columns.Add("SO_DU_MOI", typeof(decimal));
            dt.Columns.Add("TAI_KHOAN_THANH_TOAN", typeof(decimal));
            dt.Columns.Add("TIEN_LAI_TINH_DUOC", typeof(decimal));
            dt.Columns.Add("SO_TIEN_LAI", typeof(decimal));
            dt.Columns.Add("LAI_DU_CHI", typeof(decimal));
            dt.Columns.Add("RUT_LAI", typeof(string));
            dsRutGoc = new DataSet();
            dsRutGoc.Tables.Add(dt);


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
                        KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.Shift);
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

            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                OnHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                OnSave();
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
                OnPreview();
            }
            else if (strTinhNang.Equals("PreviewChungTu"))
            {
                OnPreview();
            }
            else if (strTinhNang.Equals("PreviewDanhSach"))
            {
                OnPreviewDanhSach();
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

            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                OnHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                OnSave();
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
                OnPreview();
            }
            else if (strTinhNang.Equals("PreviewChungTu"))
            {
                OnPreview();
            }
            else if (strTinhNang.Equals("PreviewDanhSach"))
            {
                OnPreviewDanhSach();
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

        private void HideControl()
        {
            try
            {
                HeThong hethong = new HeThong();
                ArrayList arr = new ArrayList();
                arr = hethong.SetVisibleControl("PresentationWPF.HuyDongVon.RutGoc.ucRutGocTheoDS", "");
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
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
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

            bool ret = process.UnlockData(DatabaseConstant.Module.HDVO,
                DatabaseConstant.Function.HDV_RUT_GOC_THEO_DANH_SACH,
                DatabaseConstant.Table.BL_TIEN_GUI,
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

        private void SetEnabledAllControls(bool enable)
        {
            cmbMaCBQL.IsEnabled = enable;
            raddtNgay.IsEnabled = enable;
            txtNguoiGiaoDich.IsEnabled = enable;
            txtDiaChi.IsEnabled = enable;
            txtDienGiai.IsEnabled = enable;
            btnAdd.IsEnabled = enable;
            btnDelete.IsEnabled = enable;
            grRutGocDS.IsReadOnly = !enable;
        }

        /// <summary>
        /// Sự kiện tìm kiếm nhanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTimKiemNhanh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grRutGocDS, txtTimKiemNhanh.Text);
            }
        }

        /// <summary>
        /// Sự kiện focus vào textbox tìm kiếm nhanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTimKiemNhanh_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtTimKiemNhanh.Text == LLanguage.SearchResourceByKey("U.DungChung.TimKiemNhanh"))
            {
                txtTimKiemNhanh.Text = "";
            }
        }

        /// <summary>
        /// Sự kiện rời focus khỏi textbox tìm kiếm nhanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTimKiemNhanh_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTimKiemNhanh.Text))
            {
                txtTimKiemNhanh.Text = LLanguage.SearchResourceByKey("U.DungChung.TimKiemNhanh");
            }
        }

        /// <summary>
        /// Sự kiện thay đổi số bản ghi trên một page hiển thị
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudPageSize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (grRutGocDS != null && grRutGocDS.DataContext != null)
            {
                radpage.PageSize = (int)nudPageSize.Value;
                DataViewManager dataViewManager = new DataViewManager(dsRutGoc);
                DataView dataView = dataViewManager.CreateDataView(dsRutGoc.Tables[0]);
                grRutGocDS.DataContext = dataView;
            }
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(grRutGocDS);
        }

        /// <summary>
        /// Hàm kiểm tra sổ đã được chọn trong Grid hay chưa
        /// </summary>
        /// <param name="sSoTGui">số sổ tiền gửi</param>
        /// <returns>true: chưa được chọn;  false đã được chọn</returns>
        private bool KiemTraSo(string sSoTGui)
        {
            foreach (DataRow dr in dsRutGoc.Tables[0].Rows)
            {
                if (dr["SO_SO_TG"].ToString().Equals(sSoTGui))
                {
                    return false;
                }
            }
            return true;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            btnCalculate.IsEnabled = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            btnCalculate.IsEnabled = false;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ClientInformation.Company.Equals("QUANGBINH"))
                {
                    lstPopup.Clear();
                    Window window = new Window();
                    ucPopupSoTGui uc = new ucPopupSoTGui();
                    uc.Function = function;
                    uc.DuLieuTraVe = new ucPopupSoTGui.LayDuLieu(LayDuLieuTuPopup);
                    window.Title = LLanguage.SearchResourceByKey("MENU.5215_MO_SO_DS");
                    window.Content = uc;
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.ShowDialog();
                }
                else
                {
                    lstPopup.Clear();
                    Window window = new Window();
                    ucPopupSoTGuiNhom uc = new ucPopupSoTGuiNhom();
                    uc.Function = function;
                    uc.DuLieuTraVe = new ucPopupSoTGuiNhom.LayDuLieu(LayDuLieuTuPopup);
                    window.Title = LLanguage.SearchResourceByKey("MENU.5215_MO_SO_DS");
                    window.Content = uc;
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.ShowDialog();
                }
                if (lstPopup != null)
                {
                    isTinhLai = false;
                    foreach (DataRow dr in lstPopup)
                    {
                        DataRow drRutGoc = dsRutGoc.Tables[0].NewRow();

                        if (KiemTraSo(dr["SO_SO_TG"].ToString()))
                        {
                            drRutGoc["ID"] = Convert.ToInt32(dr["ID"]);
                            drRutGoc["SO_SO_TG"] = dr["SO_SO_TG"];
                            drRutGoc["ID_CUM"] = dr["ID_CUM"];
                            drRutGoc["TEN_CUM"] = dr["TEN_CUM"];
                            drRutGoc["MA_KHANG"] = dr["MA_KHANG"];
                            drRutGoc["TEN_KHANG"] = dr["TEN_KHANG"];
                            drRutGoc["NGAY_MO_SO"] = dr["NGAY_MO_SO"];
                            drRutGoc["NGAY_DEN_HAN"] = dr["NGAY_DEN_HAN"];
                            drRutGoc["KY_HAN"] = dr["KY_HAN"];
                            drRutGoc["SO_DU"] = Convert.ToDecimal(dr["SO_DU"]);
                            drRutGoc["LAI_SUAT"] = Convert.ToDecimal(dr["LAI_SUAT"]);
                            drRutGoc["SO_TIEN_RUT_GOC"] = 0;
                            drRutGoc["SO_DU_MOI"] = Convert.ToDecimal(dr["SO_DU"]);
                            drRutGoc["TIEN_LAI_TINH_DUOC"] = 0;
                            drRutGoc["SO_TIEN_LAI"] = 0;
                            drRutGoc["LAI_DU_CHI"] = 0;

                            dsRutGoc.Tables[0].Rows.Add(drRutGoc);
                        }
                    }
                }

                for (int i = 0; i < dsRutGoc.Tables[0].Rows.Count; i++)
                {
                    dsRutGoc.Tables[0].Rows[i]["STT"] = i + 1;
                }

                DataViewManager dataViewManager = new DataViewManager(dsRutGoc);
                DataView dataView = dataViewManager.CreateDataView(dsRutGoc.Tables[0]);
                grRutGocDS.DataContext = dataView;

                TinhTong();
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    List<int> lstSTT = new List<int>();
                    for (int i = 0; i < grRutGocDS.SelectedItems.Count; i++)
                    {
                        DataRowView dr = (DataRowView)grRutGocDS.SelectedItems[i];
                        lstSTT.Add(Convert.ToInt32(dr["STT"]));
                    }
                    lstSTT.SortByDesc();
                    foreach (int stt in lstSTT)
                        dsRutGoc.Tables[0].Rows.RemoveAt(stt - 1);

                    for (int i = 0; i < dsRutGoc.Tables[0].Rows.Count; i++)
                    {
                        dsRutGoc.Tables[0].Rows[i]["STT"] = i + 1;
                    }

                    DataViewManager dataViewManager = new DataViewManager(dsRutGoc);
                    DataView dataView = dataViewManager.CreateDataView(dsRutGoc.Tables[0]);
                    grRutGocDS.DataContext = dataView;

                    TinhTong();
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            TinhLai();
        }

        private void grRutGocDS_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Convert.ToInt32(e.Text);
            }
            catch (Exception ex)
            {
                e.Handled = true;

            }
        }

        private void grRutGocDS_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            try
            {
                DataRowView dr = (DataRowView)grRutGocDS.CurrentCellInfo.Item;
                decimal soDu = Convert.ToDecimal(dr["SO_DU"]);
                decimal soTienRutGoc = Convert.ToDecimal(dr["SO_TIEN_RUT_GOC"]);
                if (soDu < soTienRutGoc)
                {
                    string soTien = LLanguage.SearchResourceByKey("U.DungChung.SoTienRG");
                    string soTienTong = LLanguage.SearchResourceByKey("U.DungChung.SoDu");
                    LMessage.ShowMessage(soTien + " (" + soTienRutGoc + ") > " + soTienTong + " (" + soDu + ")", LMessage.MessageBoxType.Warning);
                    return;
                }
                dr["SO_DU_MOI"] = Convert.ToDecimal(dr["SO_DU"]) - Convert.ToDecimal(dr["SO_TIEN_RUT_GOC"]);
                grRutGocDS.CurrentItem = dr;

                int i = Convert.ToInt32(dr["STT"]);
                dsRutGoc.Tables[0].Rows[i - 1]["SO_TIEN_RUT_GOC"] = soTienRutGoc;
                dsRutGoc.Tables[0].Rows[i - 1]["SO_DU_MOI"] = Convert.ToDecimal(dr["SO_DU_MOI"]);

                TinhTong();

            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }            
        }

        private void cbMultiAdd_Checked(object sender, RoutedEventArgs e)
        {
            //reset biến
            obj = null;
            _objKiemSoat = null;
            id = 0;
            maGiaoDich = "";
            sTrangThaiNVu = "";
            isTinhLai = false;

            BeforeAddNew();

            cbMultiAdd.IsChecked = false;
            btnAdd.Focus();
        }

        private void grRutGocDS_Deleted(object sender, GridViewDeletedEventArgs e)
        {
            for (int i = 0; i < dsRutGoc.Tables[0].Rows.Count; i++)
            {
                dsRutGoc.Tables[0].Rows[i]["STT"] = i + 1;
            }

            DataViewManager dataViewManager = new DataViewManager(dsRutGoc);
            DataView dataView = dataViewManager.CreateDataView(dsRutGoc.Tables[0]);
            grRutGocDS.DataContext = dataView;

            TinhTong();
        }

        /// <summary>
        /// Hàm tính toán các thông tin liên quan đến danh sách sổ
        /// Tổng số sổ
        /// Tổng số dư cũ
        /// Tổng tiền gửi thêm
        /// Tổng số dư mới
        /// </summary>
        private void TinhTong()
        {
            try
            {
                int tongSoSo = dsRutGoc.Tables[0].Rows.Count;
                decimal tongDuCu = 0;
                decimal tongRutGoc = 0;
                decimal tongDuMoi = 0;

                foreach (DataRow dr in dsRutGoc.Tables[0].Rows)
                {
                    tongDuCu = tongDuCu + Convert.ToDecimal(dr["SO_DU"]);
                    tongRutGoc = tongRutGoc + Convert.ToDecimal(dr["SO_TIEN_RUT_GOC"]);
                    tongDuMoi = tongDuMoi + Convert.ToDecimal(dr["SO_DU_MOI"]);
                }

                if (tongSoSo != 0)
                {
                    lblTongSo.Content = String.Format("{0:#,#}", tongSoSo);
                }
                else
                {
                    lblTongSo.Content = 0;
                }

                if (tongDuCu != 0)
                {
                    lblTongoDuCu.Content = String.Format("{0:#,#}", tongDuCu);
                }
                else
                {
                    lblTongoDuCu.Content = 0;
                }

                if (tongRutGoc != 0)
                {
                    lblTongTienRutGoc.Content = String.Format("{0:#,#}", tongRutGoc);
                }
                else
                {
                    lblTongTienRutGoc.Content = 0;
                }

                if (tongDuMoi != 0)
                {
                    lblTongSoDuMoi.Content = String.Format("{0:#,#}", tongDuMoi);
                }
                else
                {
                    lblTongSoDuMoi.Content = 0;
                }

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }
        #endregion

        #region Xử lý nghiệp vụ

        private void GetFormData(ref HDV_RUT_GOC_THEO_DANH_SACH obj, string sTrangThaiNVu)
        {
            try
            {
                if (!maGiaoDich.IsNullOrEmptyOrSpace())
                    obj.MA_GDICH = maGiaoDich;
                obj.NGAY_GDICH = ClientInformation.NgayLamViecHienTai;
                obj.HINH_THUC_GIAO_DICH = lstSourceGD_HinhThuc.ElementAt(cmbGD_HinhThuc.SelectedIndex).KeywordStrings.First();
                if(cmbMaCBQL.SelectedIndex>=0)
                    obj.MA_CBQL = lstSourceCBQL.ElementAt(cmbMaCBQL.SelectedIndex).KeywordStrings.ElementAt(0);
                obj.LOAI_TIEN = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.First();
                obj.NGUOI_GDICH = txtNguoiGiaoDich.Text;
                obj.DIA_CHI = txtDiaChi.Text;
                obj.DIEN_GIAI = txtDienGiai.Text;
                if (chkRutLai.IsChecked == true)
                    obj.RUT_LAI = BusinessConstant.CoKhong.CO.layGiaTri();
                else
                    obj.RUT_LAI = BusinessConstant.CoKhong.KHONG.layGiaTri();

                if (lstThongTinLai != null)
                    obj.LIST_THONG_TIN_LAI = lstThongTinLai.ToArray();

                //Thông tin kiểm soát
                obj.MA_DVI = ClientInformation.MaDonViGiaoDich;
                obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                obj.TRANG_THAI_NGHIEP_VU = sTrangThaiNVu;
                obj.TRANG_THAI_BAN_GHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.NGAY_LAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                obj.NGUOI_LAP = txtNguoiLap.Text;
                if (action != DatabaseConstant.Action.THEM)
                {
                    obj.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                }

                List<DANH_SACH_SO_RUT_GOC> lstRutGoc = new List<DANH_SACH_SO_RUT_GOC>();
                foreach (DataRow dr in dsRutGoc.Tables[0].Rows)
                {
                    DANH_SACH_SO_RUT_GOC objCT = new DANH_SACH_SO_RUT_GOC();
                    objCT.SO_SO_TG = dr["SO_SO_TG"].ToString();
                    objCT.SO_DU = Convert.ToDecimal(dr["SO_DU"]);
                    objCT.LAI_SUAT = Convert.ToDecimal(dr["LAI_SUAT"]);
                    objCT.ID_CUM = Convert.ToInt32(dr["ID_CUM"]);
                    objCT.TEN_CUM = dr["TEN_CUM"].ToString();
                    objCT.MA_KHACH_HANG = dr["MA_KHANG"].ToString();
                    objCT.TEN_KHACH_HANG = dr["TEN_KHANG"].ToString();
                    objCT.SO_TIEN_RUT_GOC = Convert.ToDecimal(dr["SO_TIEN_RUT_GOC"]);
                    objCT.SO_DU_MOI = Convert.ToDecimal(dr["SO_DU_MOI"]);
                    objCT.TAI_KHOAN_THANH_TOAN = dr["TAI_KHOAN_THANH_TOAN"].ToString();
                    objCT.TIEN_LAI_TINH_DUOC = Convert.ToDecimal(dr["TIEN_LAI_TINH_DUOC"]);
                    objCT.SO_TIEN_LAI = Convert.ToDecimal(dr["SO_TIEN_LAI"]);                    
                    objCT.LAI_DU_CHI = Convert.ToDecimal(dr["LAI_DU_CHI"]);                   

                    lstRutGoc.Add(objCT);
                }

                obj.DANH_SACH_RUT_GOC = lstRutGoc.ToArray();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            try
            {
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                DataSet ds = processHDV.GetThongTinRutGocTheoDS(ClientInformation.MaDonViGiaoDich, _objKiemSoat.SO_GIAO_DICH);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    txtMaGiaoDich.Text = _objKiemSoat.SO_GIAO_DICH;
                    raddtNgay.Value = LDateTime.StringToDate(dr["NGAY_DL"].ToString(), "yyyyMMdd");
                    cmbMaCBQL.SelectedIndex = lstSourceCBQL.IndexOf(lstSourceCBQL.FirstOrDefault(i => i.KeywordStrings.First().Equals(dr["MA_CBQL"])));
                    txtNguoiGiaoDich.Text = dr["TEN_KHANG"].ToString();
                    txtDiaChi.Text = dr["DIA_CHI"].ToString();
                    txtDienGiai.Text = dr["DIEN_GIAI"].ToString();
                    if (dr["RUT_LAI"].ToString().Equals(BusinessConstant.CoKhong.CO.layGiaTri()))
                    {
                        chkRutLai.IsChecked = true;
                        isTinhLai = true;
                    }
                    else
                    {
                        chkRutLai.IsChecked = false;
                        isTinhLai = false;
                    }

                    #region Tab thông tin kiểm soát
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(dr["TTHAI_BGHI"].ToString());
                    raddtNgayLap.Value = LDateTime.StringToDate(dr["NGAY_NHAP"].ToString(), "yyyyMMdd");
                    txtNguoiLap.Text = dr["NGUOI_NHAP"].ToString();
                    if (LDateTime.IsDate(dr["NGAY_CNHAT"].ToString(), "yyyyMMdd") == true)
                        raddtNgayCapNhat.Value = LDateTime.StringToDate(dr["NGAY_CNHAT"].ToString(), "yyyyMMdd");
                    else
                        raddtNgayCapNhat.Value = null;
                    txtNguoiCapNhat.Text = dr["NGUOI_CNHAT"].ToString();
                    #endregion
                }

                dsRutGoc = processHDV.GetThongTinRutGocTheoDSCTiet(ClientInformation.MaDonViGiaoDich, _objKiemSoat.SO_GIAO_DICH);
                if (dsRutGoc != null && dsRutGoc.Tables[0].Rows.Count > 0)
                {
                    DataViewManager dataViewManager = new DataViewManager(dsRutGoc);
                    DataView dataView = dataViewManager.CreateDataView(dsRutGoc.Tables[0]);
                    grRutGocDS.DataContext = dataView;
                }

                TinhTong();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void ResetForm()
        {
            cmbMaCBQL.SelectedIndex = -1;
            txtMaGiaoDich.Text = "";
            txtNguoiGiaoDich.Text = "";
            txtDiaChi.Text = "";
            txtDienGiai.Text = "";
            raddtNgay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");

            dsRutGoc.Tables[0].Rows.Clear();
            DataViewManager dataViewManager = new DataViewManager(dsRutGoc);
            DataView dataView = dataViewManager.CreateDataView(dsRutGoc.Tables[0]);
            grRutGocDS.DataContext = dataView;

            lblTongSo.Content = 0;
            lblTongoDuCu.Content = 0;
            lblTongTienRutGoc.Content = 0;
            lblTongSoDuMoi.Content = 0;

            //Thông tin kiểm soát
            txtTrangThaiBanGhi.Text = "";
            raddtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            raddtNgayCapNhat.Value = null;
            txtNguoiCapNhat.Text = "";
        }

        private bool Validation()
        {
            try
            {
                if(dsRutGoc.Tables[0].Rows.Count==0)
                {
                    LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                    return false;
                }

                else if (txtDienGiai.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaNhap(lblDienGiai.Content.ToString());
                    txtDienGiai.Focus();
                    return false;
                }

                else if (chkRutLai.IsChecked == true && isTinhLai == false)
                {
                    LMessage.ShowMessage("M.DungChung.ChuaThucHienTinhToan", LMessage.MessageBoxType.Warning);
                    btnCalculate.Focus();
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

        private bool KiemTraHopLeSoDuKhiRutGoc()
        {
            HuyDongVonProcess processHDV = new HuyDongVonProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            bool ret = false;

            obj = new HDV_RUT_GOC_THEO_DANH_SACH();
            GetFormData(ref obj, sTrangThaiNVu);
            ret = processHDV.RutGocTheoDanhSach(DatabaseConstant.Action.KIEM_TRA, ref obj, ref listClientResponseDetail);

            if (ret == false)
            {
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);

                if (LMessage.ShowMessage("M.DungChung.TiepTucThucHien", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    return true;
                else
                    return false;
            }
            else
            {
                return true;
            }
        }


        public void OnHold()
        {
            try
            {
                if (!Validation()) return;

                if (!KiemTraHopLeSoDuKhiRutGoc()) return;

                string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));
                obj = new HDV_RUT_GOC_THEO_DANH_SACH();
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

                if (!KiemTraHopLeSoDuKhiRutGoc()) return;

                string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));
                obj = new HDV_RUT_GOC_THEO_DANH_SACH();
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
            SetEnabledAllControls(false);
            action = DatabaseConstant.Action.XEM;
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);            
            
        }

        public void BeforeViewFromList()
        {
            SetFormData();
            BeforeViewFromDetail();
        }


        public void BeforeAddNew()
        {
            ResetForm();
            SetEnabledAllControls(true);
            action = DatabaseConstant.Action.THEM;
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnAddNew(HDV_RUT_GOC_THEO_DANH_SACH obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processHDV.RutGocTheoDanhSach(DatabaseConstant.Action.THEM, ref obj, ref listClientResponseDetail);
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

        public void AfterAddNew(bool ret, HDV_RUT_GOC_THEO_DANH_SACH obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                    id = obj.ID;
                    maGiaoDich = obj.MA_GDICH;
                    sTrangThaiNVu = obj.TRANG_THAI_NGHIEP_VU;
                    txtMaGiaoDich.Text = maGiaoDich;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TRANG_THAI_BAN_GHI);
                    raddtNgayLap.Value = LDateTime.StringToDate(obj.NGAY_LAP, "yyyyMMdd");
                    txtNguoiLap.Text = obj.NGUOI_LAP;
                    raddtNgayCapNhat.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                    txtNguoiCapNhat.Text = ClientInformation.TenDangNhap;

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
                    DatabaseConstant.Function.HDV_RUT_GOC_THEO_DANH_SACH,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                if (ret)
                {
                    SetEnabledAllControls(true);
                    action = DatabaseConstant.Action.SUA;
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
            if (sTrangThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                SetEnabledAllControls(false);
            else
                SetEnabledAllControls(true);
            action = DatabaseConstant.Action.SUA;
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnModify(HDV_RUT_GOC_THEO_DANH_SACH obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processHDV.RutGocTheoDanhSach(DatabaseConstant.Action.SUA, ref obj, ref listClientResponseDetail);
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

        public void AfterModify(bool ret, HDV_RUT_GOC_THEO_DANH_SACH obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                    id = obj.ID;
                    maGiaoDich = obj.MA_GDICH;
                    sTrangThaiNVu = obj.TRANG_THAI_NGHIEP_VU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TRANG_THAI_BAN_GHI);
                    raddtNgayLap.Value = LDateTime.StringToDate(obj.NGAY_LAP, "yyyyMMdd");
                    txtNguoiLap.Text = obj.NGUOI_LAP;
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
                    DatabaseConstant.Function.HDV_RUT_GOC_THEO_DANH_SACH,
                    DatabaseConstant.Table.BL_TIEN_GUI,
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
                    obj.MA_GDICH = maGiaoDich;
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        DatabaseConstant.Function.HDV_RUT_GOC_THEO_DANH_SACH,
                        DatabaseConstant.Table.BL_TIEN_GUI,
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
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                obj.MA_GDICH = maGiaoDich;
                ret = processHDV.RutGocTheoDanhSach(action, ref obj, ref listClientResponseDetail);
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
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_RUT_GOC_THEO_DANH_SACH,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                // Đóng cửa sổ chi tiết sau khi xóa
                OnClose();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
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
                    obj.MA_GDICH = maGiaoDich;
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        DatabaseConstant.Function.HDV_RUT_GOC_THEO_DANH_SACH,
                        DatabaseConstant.Table.BL_TIEN_GUI,
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
            try
            {
                bool ret = false;
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                obj.MA_GDICH = maGiaoDich;
                ret = processHDV.RutGocTheoDanhSach(action, ref obj, ref listClientResponseDetail);
                if (ret)
                {
                    AfterApprove(ret, listClientResponseDetail);
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

        public void AfterApprove(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);

                    id = obj.ID;
                    maGiaoDich = obj.MA_GDICH;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_RUT_GOC_THEO_DANH_SACH,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.DUYET,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
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
                    obj.MA_GDICH = maGiaoDich;
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        DatabaseConstant.Function.HDV_RUT_GOC_THEO_DANH_SACH,
                        DatabaseConstant.Table.BL_TIEN_GUI,
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
            try
            {
                bool ret = false;
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                obj.MA_GDICH = maGiaoDich;
                ret = processHDV.RutGocTheoDanhSach(action, ref obj, ref listClientResponseDetail);
                if (ret)
                {
                    AfterCancel(ret, listClientResponseDetail);
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

        public void AfterCancel(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);

                    id = obj.ID;
                    maGiaoDich = obj.MA_GDICH;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_RUT_GOC_THEO_DANH_SACH,
                    DatabaseConstant.Table.BL_TIEN_GUI,
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
                    obj.MA_GDICH = maGiaoDich;
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        DatabaseConstant.Function.HDV_RUT_GOC_THEO_DANH_SACH,
                        DatabaseConstant.Table.BL_TIEN_GUI,
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
            try
            {
                bool ret = false;
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                obj.MA_GDICH = maGiaoDich;
                ret = processHDV.RutGocTheoDanhSach(action, ref obj, ref listClientResponseDetail);
                if (ret)
                {
                    AfterRefuse(ret, listClientResponseDetail);
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

        public void AfterRefuse(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);

                    id = obj.ID;
                    maGiaoDich = obj.MA_GDICH;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_RUT_GOC_THEO_DANH_SACH,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }


        private void TinhLai()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (dsRutGoc.Tables[0].Rows.Count == 0)
                {
                    LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                    return;
                }

                obj = new HDV_RUT_GOC_THEO_DANH_SACH();
                GetFormData(ref obj, sTrangThaiNVu);

                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                
                isTinhLai = processHDV.RutGocTheoDanhSach(DatabaseConstant.Action.TINH_TOAN, ref obj, ref listClientResponseDetail);                
                AfterTinhLai(isTinhLai, obj, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                CommonFunction.ThongBaoLoi(ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }

        }

        public void AfterTinhLai(bool ret, HDV_RUT_GOC_THEO_DANH_SACH obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage(LLanguage.SearchResourceByKey("M_ResponseMessage_HDV_RutGocTheoDS_TinhLaiThanhCong"), LMessage.MessageBoxType.Information);

                    foreach (DANH_SACH_SO_RUT_GOC objCT in obj.DANH_SACH_RUT_GOC)
                    {
                        for (int i = 0; i < dsRutGoc.Tables[0].Rows.Count; i++)
                        {
                            if (dsRutGoc.Tables[0].Rows[i]["SO_SO_TG"].ToString().Equals(objCT.SO_SO_TG))
                            {
                                dsRutGoc.Tables[0].Rows[i]["TIEN_LAI_TINH_DUOC"] = objCT.TIEN_LAI_TINH_DUOC;
                                dsRutGoc.Tables[0].Rows[i]["SO_TIEN_LAI"] = objCT.SO_TIEN_LAI;
                                dsRutGoc.Tables[0].Rows[i]["LAI_DU_CHI"] = objCT.LAI_DU_CHI;
                                
                                break;
                            }
                        }
                    }

                    lstThongTinLai = obj.LIST_THONG_TIN_LAI.ToList();

                    DataViewManager dataViewManager = new DataViewManager(dsRutGoc);
                    DataView dataView = dataViewManager.CreateDataView(dsRutGoc.Tables[0]);
                    grRutGocDS.DataContext = dataView;                    

                    TinhTong();
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


        private void OnPreview()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(id) && LObject.IsNullOrEmpty(maGiaoDich))
            {
                LMessage.ShowMessage("M.DungChung.KhongCoThongTin", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();

                GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.GDKT_IN_GIAO_DICH;
                objGIAO_DICH_BASE.ChucNang = function;

                GDKT_GIAO_DICH objGDKT_GIAO_DICH = new GDKT_GIAO_DICH();
                objGDKT_GIAO_DICH.MaGiaoDich = maGiaoDich;

                doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                doiTuongBaoCao.objGDKT_GIAO_DICH = objGDKT_GIAO_DICH;
                xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
            }
        }

        private void OnPreviewDanhSach()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(maGiaoDich))
            {
                LMessage.ShowMessage("M.DungChung.KhongCoThongTin", LMessage.MessageBoxType.Warning);

            }
            else
            {
                if (ClientInformation.Company.Equals("BANTAYVANG"))
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();

                    List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();
                    lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaGiaoDich", maGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.HDVO_DANH_SACH_HOAN_TK);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
                else
                {

                }
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }
        #endregion

    }
}
