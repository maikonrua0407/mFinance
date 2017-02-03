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

namespace PresentationWPF.HuyDongVon.LaiNhapGoc
{
    /// <summary>
    /// Interaction logic for ucLaiNhapGocTheoDS.xaml
    /// </summary>
    public partial class ucLaiNhapGocTheoDS : UserControl
    {
        #region Khai bao

        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();    
        
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private DatabaseConstant.Function function = DatabaseConstant.Function.HDV_LAI_NHAP_GOC_THEO_DANH_SACH;

        public event EventHandler OnSavingCompleted;

        private DataSet dsLNG;

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private string maGiaoDich = "";

        private KIEM_SOAT _objKiemSoat = null;

        private string sTrangThaiNVu = "";

        private bool isTinhLai = false;

        private HDV_LAI_NHAP_GOC_THEO_DANH_SACH obj;
        public HDV_LAI_NHAP_GOC_THEO_DANH_SACH Obj
        {
            get { return obj; }
            set { obj = value; }
        }

        private List<THONG_TIN_TIEN_LAI> lstThongTinLai = null;

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
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        #endregion

        #region Khoi tao
        public ucLaiNhapGocTheoDS()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            LoadCombobox();

            KhoiTaoGridLNG();

            radpage.PageSize = (int)nudPageSize.Value;

            btnAdd.Focus();
        }

        public ucLaiNhapGocTheoDS(KIEM_SOAT obj)
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            LoadCombobox();

            KhoiTaoGridLNG();

            radpage.PageSize = (int)nudPageSize.Value;

            _objKiemSoat = obj;

            action = _objKiemSoat.action;

            sTrangThaiNVu = _objKiemSoat.TTHAI_NVU;
            maGiaoDich = _objKiemSoat.SO_GIAO_DICH;
            isTinhLai = true;

            this.obj = new HDV_LAI_NHAP_GOC_THEO_DANH_SACH();

            raddtDenNgay.Focus();

            btnAdd.Focus();

        }

        private void LoadCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();

            auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TIENTE.getValue(), null, ClientInformation.MaDongNoiTe);
        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.HuyDongVon;component/LaiNhapGoc/ucLaiNhapGocTheoDS.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void KhoiTaoGridLNG()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("SO_SO_TG", typeof(string));
            dt.Columns.Add("TEN_KHANG", typeof(string));
            dt.Columns.Add("NGAY_MO_SO", typeof(string));
            dt.Columns.Add("SO_DU", typeof(decimal));
            dt.Columns.Add("LAI_SUAT", typeof(decimal));
            dt.Columns.Add("TINH_LAI_TU", typeof(string));
            dt.Columns.Add("TINH_LAI_DEN", typeof(string));
            dt.Columns.Add("LAI_NHAP_GOC", typeof(decimal));
            dt.Columns.Add("LAI_DU_CHI", typeof(decimal));
            dt.Columns.Add("SO_DU_MOI", typeof(decimal));
            dt.Columns.Add("LAI_SUAT_MOI", typeof(decimal));
            dsLNG = new DataSet();
            dsLNG.Tables.Add(dt);


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

            bool ret = process.UnlockData(DatabaseConstant.Module.HDVO,
                DatabaseConstant.Function.HDV_LAI_NHAP_GOC_THEO_DANH_SACH,
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
            raddtDenNgay.IsEnabled = enable;
            dtpNgayApDung.IsEnabled = enable;
            txtDienGiai.IsEnabled = enable;
            btnAdd.IsEnabled = enable;
            btnDelete.IsEnabled = enable;
            btnCalculate.IsEnabled = enable;
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
                PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grLaiNhapGocDS, txtTimKiemNhanh.Text);
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
            if (grLaiNhapGocDS != null && grLaiNhapGocDS.ItemsSource != null) 
            {
                radpage.PageSize = (int)nudPageSize.Value;
                DataViewManager dataViewManager = new DataViewManager(dsLNG);
                DataView dataView = dataViewManager.CreateDataView(dsLNG.Tables[0]);
                grLaiNhapGocDS.DataContext = dataView;                
            }
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(grLaiNhapGocDS);
        }

        /// <summary>
        /// Hàm kiểm tra sổ đã được chọn trong Grid hay chưa
        /// </summary>
        /// <param name="sSoTGui">số sổ tiền gửi</param>
        /// <returns>true: chưa được chọn;  false đã được chọn</returns>
        private bool KiemTraSo(string sSoTGui)
        {
            foreach (DataRow dr in dsLNG.Tables[0].Rows)
            {
                if (dr["SO_SO_TG"].ToString().Equals(sSoTGui))
                {
                    return false;
                }
            }
            return true;            
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
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
                if (lstPopup != null)
                {
                    isTinhLai = false;

                    HuyDongVonProcess processHDV = new HuyDongVonProcess();
                    foreach (DataRow dr in lstPopup)
                    {
                        DataRow drLNG = dsLNG.Tables[0].NewRow();

                        if (KiemTraSo(dr["SO_SO_TG"].ToString()))
                        {
                            drLNG["ID"] = Convert.ToInt32(dr["ID"]);
                            drLNG["SO_SO_TG"] = dr["SO_SO_TG"];
                            drLNG["TEN_KHANG"] = dr["TEN_KHANG"];
                            drLNG["NGAY_MO_SO"] = dr["NGAY_MO_SO"];
                            drLNG["SO_DU"] = Convert.ToDecimal(dr["SO_DU"]);
                            drLNG["LAI_SUAT"] = Convert.ToDecimal(dr["LAI_SUAT"]);
                            drLNG["TINH_LAI_TU"] = "";
                            drLNG["TINH_LAI_DEN"] = "";
                            drLNG["LAI_NHAP_GOC"] = 0;
                            drLNG["LAI_DU_CHI"] = 0;
                            drLNG["SO_DU_MOI"] = Convert.ToDecimal(dr["SO_DU"]);
                            //drLNG["LAI_SUAT_MOI"] = 0;
                            
                            dsLNG.Tables[0].Rows.Add(drLNG);
                        }
                    }
                }

                for (int i = 0; i < dsLNG.Tables[0].Rows.Count; i++)
                {
                    dsLNG.Tables[0].Rows[i]["STT"] = i + 1;
                }

                DataViewManager dataViewManager = new DataViewManager(dsLNG);
                DataView dataView = dataViewManager.CreateDataView(dsLNG.Tables[0]);
                grLaiNhapGocDS.DataContext = dataView;                

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
                    List<string> lstSoTG = new List<string>();

                    for (int i = 0; i < grLaiNhapGocDS.SelectedItems.Count; i++)
                    {
                        DataRowView dr = (DataRowView)grLaiNhapGocDS.SelectedItems[i];
                        lstSTT.Add(Convert.ToInt32(dr["STT"]));
                        lstSoTG.Add(dr["SO_SO_TG"].ToString());
                    }
                    lstSTT.SortByDesc();
                    foreach (int stt in lstSTT)
                        dsLNG.Tables[0].Rows.RemoveAt(stt - 1);

                    for (int i = 0; i < dsLNG.Tables[0].Rows.Count; i++)
                    {
                        dsLNG.Tables[0].Rows[i]["STT"] = i + 1;
                    }

                    // Xóa thông tin lãi
                    if (!LObject.IsNullOrEmpty(lstThongTinLai) && lstThongTinLai.Count > 0)
                    {
                        foreach (string soTG in lstSoTG)
                        {
                            lstThongTinLai = lstThongTinLai.Where(a => a.DOI_TUONG != soTG).ToList();
                        }
                    }

                    DataViewManager dataViewManager = new DataViewManager(dsLNG);
                    DataView dataView = dataViewManager.CreateDataView(dsLNG.Tables[0]);
                    grLaiNhapGocDS.DataContext = dataView;   

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

        private void cbMultiAdd_Checked(object sender, RoutedEventArgs e)
        {
            //reset biến
            obj = null;
            _objKiemSoat = null;
            id = 0;
            maGiaoDich = "";
            sTrangThaiNVu = "";
            isTinhLai = false;
            lstThongTinLai = null;

            BeforeAddNew();

            cbMultiAdd.IsChecked = false;
            btnAdd.Focus();
        }

        private void grLaiNhapGocDS_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            try
            {
                DataRowView dr = (DataRowView)grLaiNhapGocDS.CurrentCellInfo.Item;
                grLaiNhapGocDS.CurrentItem = dr;

                int i = Convert.ToInt32(dr["STT"]);
                dsLNG.Tables[0].Rows[i - 1]["LAI_SUAT_MOI"] = Convert.ToDecimal(dr["LAI_SUAT_MOI"]);
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void grLaiNhapGocDS_Deleted(object sender, GridViewDeletedEventArgs e)
        {
            for (int i = 0; i < dsLNG.Tables[0].Rows.Count; i++)
            {
                dsLNG.Tables[0].Rows[i]["STT"] = i + 1;
            }

            DataViewManager dataViewManager = new DataViewManager(dsLNG);
            DataView dataView = dataViewManager.CreateDataView(dsLNG.Tables[0]);
            grLaiNhapGocDS.DataContext = dataView;

            TinhTong();
        }


        /// <summary>
        /// Hàm tính toán các thông tin liên quan đến danh sách sổ
        /// Tổng số sổ
        /// Tổng số dư cũ
        /// Tổng lãi nhập gốc
        /// Tổng lãi đã dự chi
        /// Tổng số dư mới
        /// </summary>
        private void TinhTong()
        {
            try
            {
                int tongSoSo = dsLNG.Tables[0].Rows.Count;
                decimal tongSoDu = 0;
                decimal tongLNG = 0;
                decimal tongLaiDaDuChi= 0;
                decimal tongSoDuMoi= 0;

                foreach (DataRow dr in dsLNG.Tables[0].Rows)
                {
                    tongSoDu = tongSoDu + Convert.ToDecimal(dr["SO_DU"]);
                    tongLNG = tongLNG + Convert.ToDecimal(dr["LAI_NHAP_GOC"]);
                    tongLaiDaDuChi = tongLaiDaDuChi + Convert.ToDecimal(dr["LAI_DU_CHI"]);
                    tongSoDuMoi = tongSoDuMoi + Convert.ToDecimal(dr["SO_DU_MOI"]);
                }

                if (tongSoSo != 0)
                {
                    lblTongSo.Content = String.Format("{0:#,#}", tongSoSo);
                }
                else
                {
                    lblTongSo.Content = 0;
                }

                if (tongSoDu != 0)
                {
                    lblTongDuCu.Content = String.Format("{0:#,#}", tongSoDu);
                }
                else
                {
                    lblTongDuCu.Content = 0;
                }

                if (tongLNG != 0)
                {
                    lblTongLaiNhapGoc.Content = String.Format("{0:#,#}", tongLNG);
                }
                else
                {
                    lblTongLaiNhapGoc.Content = 0;
                }

                if (tongLaiDaDuChi != 0)
                {
                    lblTongDaDuChi.Content = String.Format("{0:#,#}", tongLaiDaDuChi);
                }
                else
                {
                    lblTongDaDuChi.Content = 0;
                }

                if (tongSoDuMoi != 0)
                {
                    lblTongSoDuMoi.Content = String.Format("{0:#,#}", tongSoDuMoi);
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
        private void GetFormData(ref HDV_LAI_NHAP_GOC_THEO_DANH_SACH obj, string sTrangThaiNVu)
        {
            try
            {
                if (!maGiaoDich.IsNullOrEmptyOrSpace())
                    obj.MA_GDICH = maGiaoDich;
                obj.MA_DVI = ClientInformation.MaDonViGiaoDich;
                obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                obj.NGAY_GDICH = ClientInformation.NgayLamViecHienTai;
                obj.LAI_NHAP_GOC_DEN_NGAY = Convert.ToDateTime(raddtDenNgay.Value).ToString("yyyyMMdd");
                obj.LOAI_TIEN = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.ElementAt(0);
                obj.DIEN_GIAI = txtDienGiai.Text;

                //Thông tin kiểm soát
                obj.TRANG_THAI_NGHIEP_VU = sTrangThaiNVu;
                obj.TRANG_THAI_BAN_GHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.NGAY_LAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                obj.NGUOI_LAP = txtNguoiLap.Text;
                if (action != DatabaseConstant.Action.THEM)
                {
                    obj.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                }

                List<DANH_SACH_LAI_NHAP_GOC> lstLNG = new List<DANH_SACH_LAI_NHAP_GOC>();
                foreach (DataRow dr in dsLNG.Tables[0].Rows)
                {
                    DANH_SACH_LAI_NHAP_GOC objCT = new DANH_SACH_LAI_NHAP_GOC();
                    objCT.SO_SO_TG = dr["SO_SO_TG"].ToString();
                    objCT.SO_DU = Convert.ToDecimal(dr["SO_DU"]);
                    objCT.NGAY_MO_SO = dr["NGAY_MO_SO"].ToString();
                    objCT.LAI_SUAT = Convert.ToDecimal(dr["LAI_SUAT"]);
                    objCT.TEN_KHACH_HANG = dr["TEN_KHANG"].ToString();
                    objCT.TINH_LAI_TU_NGAY = dr["TINH_LAI_TU"].ToString();
                    objCT.TINH_LAI_DEN_NGAY = dr["TINH_LAI_DEN"].ToString();
                    objCT.LAI_NHAP_GOC = Convert.ToDecimal(dr["LAI_NHAP_GOC"]);
                    objCT.LAI_DU_CHI = Convert.ToDecimal(dr["LAI_DU_CHI"]);
                    objCT.SO_DU_MOI = Convert.ToDecimal(dr["SO_DU_MOI"]);

                    if(LObject.IsNullOrEmpty(dr["LAI_SUAT_MOI"]) || dr["LAI_SUAT_MOI"].ToString().IsNullOrEmptyOrSpace())
                        objCT.LAI_SUAT_MOI = 0;
                    else
                        objCT.LAI_SUAT_MOI = Convert.ToDecimal(dr["LAI_SUAT_MOI"]);

                    lstLNG.Add(objCT);
                }

                obj.DSACH_LAI_NHAP_GOC = lstLNG.ToArray();

                if (lstThongTinLai != null)
                    obj.LIST_THONG_TIN_LAI = lstThongTinLai.ToArray();
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
                DataSet ds = processHDV.GetThongTinLNGDS(ClientInformation.MaDonViGiaoDich, _objKiemSoat.SO_GIAO_DICH);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    raddtDenNgay.Value = LDateTime.StringToDate(dr["NGAY_LNG"].ToString(), "yyyyMMdd");
                    txtDienGiai.Text = dr["DIEN_GIAI"].ToString();

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

                dsLNG = processHDV.GetThongTinLNGDSCTiet(ClientInformation.MaDonViGiaoDich, _objKiemSoat.SO_GIAO_DICH);
                if (dsLNG != null && dsLNG.Tables[0].Rows.Count > 0)
                {
                    DataViewManager dataViewManager = new DataViewManager(dsLNG);
                    DataView dataView = dataViewManager.CreateDataView(dsLNG.Tables[0]);
                    grLaiNhapGocDS.DataContext = dataView;   
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
            raddtDenNgay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtDienGiai.Text = "";

            dsLNG.Tables[0].Rows.Clear();
            DataViewManager dataViewManager = new DataViewManager(dsLNG);
            DataView dataView = dataViewManager.CreateDataView(dsLNG.Tables[0]);
            grLaiNhapGocDS.DataContext = dataView;

            lblTongSo.Content = 0;
            lblTongDuCu.Content = 0;
            lblTongLaiNhapGoc.Content = 0;
            lblTongDaDuChi.Content = 0;
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
                if (dsLNG.Tables[0].Rows.Count == 0)
                {
                    LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                    return false;
                }

                else if (isTinhLai == false)
                {
                    LMessage.ShowMessage("M.DungChung.ChuaThucHienTinhToan", LMessage.MessageBoxType.Warning);
                    btnCalculate.Focus();
                    return false;
                }

                else if (txtDienGiai.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaNhap(lblDienGiai.Content.ToString());
                    txtDienGiai.Focus();
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

        public void OnHold()
        {
            try
            {
                if (!Validation()) return;
                string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));
                obj = new HDV_LAI_NHAP_GOC_THEO_DANH_SACH();
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

                string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));
                obj = new HDV_LAI_NHAP_GOC_THEO_DANH_SACH();
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
            tlbPreview.IsEnabled = true;
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

        public void OnAddNew(HDV_LAI_NHAP_GOC_THEO_DANH_SACH obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                bool ret = false;
                DateTime dt = DateTime.Now;
                ret = processHDV.LaiNhapGocTheoDanhSach(DatabaseConstant.Action.THEM, ref obj, ref listClientResponseDetail);
                LLogging.WriteLog("TrinhDuyet: ", LLogging.LogType.BUS, DateTime.Now.Subtract(dt).TotalMilliseconds.ToString());
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

        public void AfterAddNew(bool ret, HDV_LAI_NHAP_GOC_THEO_DANH_SACH obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);
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
                    //LMessage.ShowMessage("M.DungChung.ThemKhongThanhCong", LMessage.MessageBoxType.Error);
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
                    DatabaseConstant.Function.HDV_LAI_NHAP_GOC_THEO_DANH_SACH,
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

        public void OnModify(HDV_LAI_NHAP_GOC_THEO_DANH_SACH obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processHDV.LaiNhapGocTheoDanhSach(DatabaseConstant.Action.SUA, ref obj, ref listClientResponseDetail);
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

        public void AfterModify(bool ret, HDV_LAI_NHAP_GOC_THEO_DANH_SACH obj, List<ClientResponseDetail> listClientResponseDetail)
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
                    //LMessage.ShowMessage("M.DungChung.CapNhatKoThanhCong", LMessage.MessageBoxType.Error);
                }

                // Yêu cầu Unlock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_LAI_NHAP_GOC_THEO_DANH_SACH,
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
                    List<int> listLockedId = new List<int>();
                    obj = new HDV_LAI_NHAP_GOC_THEO_DANH_SACH();
                    if (!maGiaoDich.IsNullOrEmptyOrSpace())
                        obj.MA_GDICH = maGiaoDich;
                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        DatabaseConstant.Function.HDV_LAI_NHAP_GOC_THEO_DANH_SACH,
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
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_LAI_NHAP_GOC_THEO_DANH_SACH,
                    DatabaseConstant.Table.BL_TIEN_GUI,
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
                obj.MA_GDICH = maGiaoDich;
                ret = processHDV.LaiNhapGocTheoDanhSach(action, ref obj, ref listClientResponseDetail);
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
                    DatabaseConstant.Function.HDV_LAI_NHAP_GOC_THEO_DANH_SACH,
                    DatabaseConstant.Table.BL_TIEN_GUI,
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
                    string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.DUYET, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));
                    obj = new HDV_LAI_NHAP_GOC_THEO_DANH_SACH();
                    GetFormData(ref obj, trangThai);

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        DatabaseConstant.Function.HDV_LAI_NHAP_GOC_THEO_DANH_SACH,
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
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_LAI_NHAP_GOC_THEO_DANH_SACH,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listLockId);

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
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                obj.MA_GDICH = maGiaoDich;
                DateTime dt = DateTime.Now;
                ret = processHDV.LaiNhapGocTheoDanhSach(action, ref obj, ref listClientResponseDetail);
                LLogging.WriteLog("Duyet: ", LLogging.LogType.BUS, DateTime.Now.Subtract(dt).TotalMilliseconds.ToString());
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
                    //LMessage.ShowMessage("M.DungChung.DuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_LAI_NHAP_GOC_THEO_DANH_SACH,
                    DatabaseConstant.Table.BL_TIEN_GUI,
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
                    obj = new HDV_LAI_NHAP_GOC_THEO_DANH_SACH();
                    if (!maGiaoDich.IsNullOrEmptyOrSpace())
                        obj.MA_GDICH = maGiaoDich;
                    obj.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        DatabaseConstant.Function.HDV_LAI_NHAP_GOC_THEO_DANH_SACH,
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
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_LAI_NHAP_GOC_THEO_DANH_SACH,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listLockId);

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
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                obj.MA_GDICH = maGiaoDich;
                ret = processHDV.LaiNhapGocTheoDanhSach(action, ref obj, ref listClientResponseDetail);
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
                    //LMessage.ShowMessage("M.DungChung.ThoaiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_LAI_NHAP_GOC_THEO_DANH_SACH,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listLockId);

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
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
                    obj = new HDV_LAI_NHAP_GOC_THEO_DANH_SACH();
                    if (!maGiaoDich.IsNullOrEmptyOrSpace())
                        obj.MA_GDICH = maGiaoDich;
                    obj.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        DatabaseConstant.Function.HDV_LAI_NHAP_GOC_THEO_DANH_SACH,
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
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_LAI_NHAP_GOC_THEO_DANH_SACH,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);

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
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                obj.MA_GDICH = maGiaoDich;
                ret = processHDV.LaiNhapGocTheoDanhSach(action, ref obj, ref listClientResponseDetail);
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
                    //LMessage.ShowMessage("M.DungChung.TuChoiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_LAI_NHAP_GOC_THEO_DANH_SACH,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        private void OnPreview()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
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

        private void TinhLai()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (dsLNG.Tables[0].Rows.Count == 0)
                {
                    LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                    return;
                }

                obj = new HDV_LAI_NHAP_GOC_THEO_DANH_SACH();
                GetFormData(ref obj, sTrangThaiNVu);

                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                DateTime dt = DateTime.Now;
                isTinhLai = processHDV.LaiNhapGocTheoDanhSach(DatabaseConstant.Action.TINH_TOAN, ref obj, ref listClientResponseDetail);
                LLogging.WriteLog("TinhLai", LLogging.LogType.BUS, DateTime.Now.Subtract(dt).TotalMilliseconds.ToString());
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

        public void AfterTinhLai(bool ret, HDV_LAI_NHAP_GOC_THEO_DANH_SACH obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.TinhToanThanhCong", LMessage.MessageBoxType.Information);

                    foreach (DANH_SACH_LAI_NHAP_GOC objCT in obj.DSACH_LAI_NHAP_GOC)
                    {
                        for (int i = 0; i < dsLNG.Tables[0].Rows.Count; i++)
                        {
                            if (dsLNG.Tables[0].Rows[i]["SO_SO_TG"].ToString().Equals(objCT.SO_SO_TG))
                            {
                                dsLNG.Tables[0].Rows[i]["TINH_LAI_TU"] = LDateTime.StringToDate(objCT.TINH_LAI_TU_NGAY, "yyyyMMdd").ToString("dd/MM/yyyy");
                                dsLNG.Tables[0].Rows[i]["TINH_LAI_DEN"] = LDateTime.StringToDate(objCT.TINH_LAI_DEN_NGAY, "yyyyMMdd").ToString("dd/MM/yyyy");
                                dsLNG.Tables[0].Rows[i]["LAI_NHAP_GOC"] = objCT.LAI_NHAP_GOC;
                                dsLNG.Tables[0].Rows[i]["LAI_DU_CHI"] = objCT.LAI_DU_CHI;
                                dsLNG.Tables[0].Rows[i]["SO_DU_MOI"] = objCT.SO_DU_MOI;
                                dsLNG.Tables[0].Rows[i]["LAI_SUAT_MOI"] = objCT.LAI_SUAT_MOI;
                                break;
                            }
                        }
                    }

                    lstThongTinLai = obj.LIST_THONG_TIN_LAI.ToList();

                    DataViewManager dataViewManager = new DataViewManager(dsLNG);
                    DataView dataView = dataViewManager.CreateDataView(dsLNG.Tables[0]);
                    grLaiNhapGocDS.DataContext = dataView;  

                    TinhTong();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    //if (MessageBox.Show("Xóa khỏi danh sách lãi nhập gốc", "Xóa", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    //{                        
                    //    List<string> lstSoTG = listClientResponseDetail.Select(e => e.Object).ToList();

                    //    foreach (string soSoTGui in lstSoTG)
                    //    {
                    //        foreach (DataRow dr in dsLNG.Tables[0].Rows)
                    //        {
                    //            if (dr["SO_SO_TG"].ToString().Equals(soSoTGui))
                    //            {
                    //                dsLNG.Tables[0].Rows.Remove(dr);
                    //                break;
                    //            }
                    //        }
                    //    }

                    //    for (int i = 0; i < dsLNG.Tables[0].Rows.Count; i++)
                    //    {
                    //        dsLNG.Tables[0].Rows[i]["STT"] = i + 1;
                    //    }

                    //    // Xóa thông tin lãi
                    //    if (!LObject.IsNullOrEmpty(lstThongTinLai) && lstThongTinLai.Count > 0)
                    //    {
                    //        foreach (string soTG in lstSoTG)
                    //        {
                    //            lstThongTinLai = lstThongTinLai.Where(a => a.DOI_TUONG != soTG).ToList();
                    //        }
                    //    }

                    //    DataViewManager dataViewManager = new DataViewManager(dsLNG);
                    //    DataView dataView = dataViewManager.CreateDataView(dsLNG.Tables[0]);
                    //    grLaiNhapGocDS.DataContext = dataView;  

                    //    TinhTong();
                    //}
                }
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
