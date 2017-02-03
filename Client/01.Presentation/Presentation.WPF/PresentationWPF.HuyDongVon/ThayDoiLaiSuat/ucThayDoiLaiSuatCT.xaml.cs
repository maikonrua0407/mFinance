using System;
using System.Collections.Generic;
using System.Linq;
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

namespace PresentationWPF.HuyDongVon.ThayDoiLaiSuat
{
    /// <summary>
    /// Interaction logic for ucThayDoiLaiSuatCT.xaml
    /// </summary>
    public partial class ucThayDoiLaiSuatCT : UserControl
    {
        #region Khai bao

        private DatabaseConstant.Module Module = DatabaseConstant.Module.HDVO;
        private DatabaseConstant.Function function = DatabaseConstant.Function.HDV_DIEU_CHINH_LS;

        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        public event EventHandler OnSavingCompleted;

        private DataSet dsThayDoiLS;

        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string ma;

        public string Ma
        {
            get { return ma; }
            set { ma = value; }
        }

        private KIEM_SOAT _objKiemSoat = new KIEM_SOAT() ;

        private DateTime ngayApDung = ClientInformation.NgayLamViecHienTai.StringToDate("yyyyMMdd");

        private string sTrangThaiNVu = "";

        private HDV_THAY_DOI_LAI_SUAT obj;
        public HDV_THAY_DOI_LAI_SUAT Obj
        {
            get { return obj; }
            set { obj = value; }
        }

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
        public static RoutedCommand ViewCommand = new RoutedCommand();
        public static RoutedCommand SearchCommand = new RoutedCommand();
        public static RoutedCommand ReloadCommand = new RoutedCommand();
        public static RoutedCommand ExportCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        #endregion

        #region Khoi tao
        public ucThayDoiLaiSuatCT()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            KhoiTaoGridThayDoiLS();

            radpage.PageSize = (int)nudPageSize.Value;
        }

        public ucThayDoiLaiSuatCT(KIEM_SOAT obj)
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            KhoiTaoGridThayDoiLS();

            radpage.PageSize = (int)nudPageSize.Value;

            _objKiemSoat = obj;
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

        private void KhoiTaoGridThayDoiLS()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("ID_TIEN_GUI", typeof(int));
            dt.Columns.Add("SO_SO_TG", typeof(string));
            dt.Columns.Add("TEN_KHANG", typeof(string));
            dt.Columns.Add("NGAY_MO_SO", typeof(string));
            dt.Columns.Add("NGAY_DEN_HAN", typeof(string));
            dt.Columns.Add("KY_HAN", typeof(string));
            dt.Columns.Add("SO_DU", typeof(decimal));            
            dt.Columns.Add("LAI_SUAT_HIEN_TAI", typeof(decimal));
            dt.Columns.Add("LAI_SUAT_MOI", typeof(decimal));
            dt.Columns.Add("NGAY_ADUNG", typeof(string));                 
            dsThayDoiLS = new DataSet();
            dsThayDoiLS.Tables.Add(dt);
        }

        #endregion

        #region Dang ky hot key, shortcut key

        /// <summary>
        /// Định nghĩa phím tắt
        /// </summary>
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
                        key = new KeyBinding(ucThayDoiLaiSuatCT.HoldCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(ucThayDoiLaiSuatCT.ModifyCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.Shift);
                        key = new KeyBinding(ucThayDoiLaiSuatCT.DeleteCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucThayDoiLaiSuatCT.ApproveCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucThayDoiLaiSuatCT.RefuseCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.C, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucThayDoiLaiSuatCT.CancelCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(ucThayDoiLaiSuatCT.HelpCommand, keyg);
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
            OnHold();
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbSave.IsEnabled;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnSave();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbModify.IsEnabled;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeModifyFromDetail();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbDelete.IsEnabled;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeDelete();
        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbApprove.IsEnabled;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnApprove();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbRefuse.IsEnabled;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnRefuse();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbCancel.IsEnabled;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnCancel();
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                BeforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                BeforeRefuse();
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                BeforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                BeforeRefuse();
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
            if (action == DatabaseConstant.Action.XEM)
                BeforeViewFromList();
            else if (action == DatabaseConstant.Action.SUA)
                BeforeModifyFromList();
            else
                BeforeAddNew();
            
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

        private void cbMultiAdd_Checked(object sender, RoutedEventArgs e)
        {
            //reset biến
            obj = null;
            _objKiemSoat = null;
            id = 0;
            ma = "";
            sTrangThaiNVu = "";

            BeforeAddNew();

            cbMultiAdd.IsChecked = false;
            btnAdd.Focus();
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
                DatabaseConstant.Function.HDV_DIEU_CHINH_LS,
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
            raddtNgayApDung.IsEnabled = enable;
            raddtNgayGD.IsEnabled = enable;
            dtpNgayApDung.IsEnabled = enable;
            numLaiSuatMoi.IsEnabled = enable;
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
                PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grThayDoiLS, txtTimKiemNhanh.Text);
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
            if (grThayDoiLS != null && grThayDoiLS.DataContext != null) 
            {
                radpage.PageSize = (int)nudPageSize.Value;
                grThayDoiLS.DataContext = dsThayDoiLS.Tables[0];
            }
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(grThayDoiLS);
        }

        /// <summary>
        /// Hàm kiểm tra sổ đã được chọn trong Grid hay chưa
        /// </summary>
        /// <param name="sSoTGui">số sổ tiền gửi</param>
        /// <returns>true: chưa được chọn;  false đã được chọn</returns>
        private bool KiemTraSo(string sSoTGui)
        {
            foreach (DataRow dr in dsThayDoiLS.Tables[0].Rows)
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
                uc.Function = DatabaseConstant.Function.HDV_DIEU_CHINH_LS;
                uc.DuLieuTraVe = new ucPopupSoTGui.LayDuLieu(LayDuLieuTuPopup);
                window.Title = "Danh sách sổ";
                window.Content = uc;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
                if (lstPopup != null)
                {
                    foreach (DataRow dr in lstPopup)
                    {
                        DataRow drThayDoiLS = dsThayDoiLS.Tables[0].NewRow();

                        if (KiemTraSo(dr["SO_SO_TG"].ToString()))
                        {
                            drThayDoiLS["ID_TIEN_GUI"] = Convert.ToInt32(dr["ID"]);
                            drThayDoiLS["SO_SO_TG"] = dr["SO_SO_TG"];
                            drThayDoiLS["TEN_KHANG"] = dr["TEN_KHANG"];
                            drThayDoiLS["NGAY_MO_SO"] = dr["NGAY_MO_SO"];
                            drThayDoiLS["NGAY_DEN_HAN"] = dr["NGAY_DEN_HAN"];
                            drThayDoiLS["KY_HAN"] = dr["KY_HAN"];
                            drThayDoiLS["SO_DU"] = Convert.ToDecimal(dr["SO_DU"]);
                            drThayDoiLS["LAI_SUAT_HIEN_TAI"] = dr["LAI_SUAT"];
                            drThayDoiLS["LAI_SUAT_MOI"] = Convert.ToDecimal(numLaiSuatMoi.Value);
                            drThayDoiLS["NGAY_ADUNG"] = raddtNgayApDung.Text;                            

                            dsThayDoiLS.Tables[0].Rows.Add(drThayDoiLS);
                        }
                    }
                }

                for (int i = 0; i < dsThayDoiLS.Tables[0].Rows.Count; i++)
                {
                    dsThayDoiLS.Tables[0].Rows[i]["STT"] = i + 1;
                }
                grThayDoiLS.DataContext = null;
                grThayDoiLS.DataContext = dsThayDoiLS.Tables[0].DefaultView;

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
                    for (int i = 0; i < grThayDoiLS.SelectedItems.Count; i++)
                    {
                        DataRowView dr = (DataRowView)grThayDoiLS.SelectedItems[i];
                        lstSTT.Add(Convert.ToInt32(dr["STT"]));
                    }
                    lstSTT.SortByDesc();
                    foreach (int stt in lstSTT)
                        dsThayDoiLS.Tables[0].Rows.RemoveAt(stt - 1);

                    for (int i = 0; i < dsThayDoiLS.Tables[0].Rows.Count; i++)
                    {
                        dsThayDoiLS.Tables[0].Rows[i]["STT"] = i + 1;
                    }

                    grThayDoiLS.DataContext = null;
                    grThayDoiLS.DataContext = dsThayDoiLS.Tables[0].DefaultView;

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
            TinhLaiLSuat();
        }

        private void TinhLaiLSuat()
        {
            try
            {
                if (((DateTime)raddtNgayApDung.Value).CompareTo(ClientInformation.NgayLamViecHienTai.StringToDate("yyyyMMdd")) >= 0)
                {
                    grThayDoiLS.DataContext = null;
                    if (dsThayDoiLS != null && dsThayDoiLS.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsThayDoiLS.Tables[0].Rows)
                        {
                            dr["LAI_SUAT_MOI"] = Convert.ToDecimal(numLaiSuatMoi.Value);
                            dr["NGAY_ADUNG"] = raddtNgayApDung.Text;
                        }
                    }
                    grThayDoiLS.DataContext = null;
                    grThayDoiLS.DataContext = dsThayDoiLS.Tables[0].DefaultView;

                    TinhTong();
                }
                else
                    raddtNgayApDung.Value = ngayApDung;
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Hàm tính toán các thông tin liên quan đến danh sách sổ
        /// Tổng số sổ        
        /// </summary>
        private void TinhTong()
        {
            try
            {
                int tongSoSo = dsThayDoiLS.Tables[0].Rows.Count;
                decimal tongSoDu = 0;

                foreach (DataRow dr in dsThayDoiLS.Tables[0].Rows)
                {
                    tongSoDu = tongSoDu + Convert.ToDecimal(dr["SO_DU"]);                 
                }

                if (tongSoSo != 0)
                {
                    lblTongSoSo.Content = String.Format("{0:#,#}", tongSoSo);
                }
                else
                {
                    lblTongSoSo.Content = 0;
                }

                if (tongSoDu != 0)
                {
                    lblTongSoDu.Content = String.Format("{0:#,#}", tongSoDu);
                }
                else
                {
                    lblTongSoDu.Content = 0;
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
        private void GetFormData(ref HDV_THAY_DOI_LAI_SUAT obj, string sTrangThaiNVu)
        {
            try
            {
                obj.ID_TDOI_LSUAT = id;
                obj.MA_TDOI_LSUAT = ma;
                obj.MA_DVI = ClientInformation.MaDonViGiaoDich;
                obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                obj.NGAY_GDICH = ClientInformation.NgayLamViecHienTai;
                obj.NGAY_AP_DUNG = Convert.ToDateTime(raddtNgayApDung.Value).ToString("yyyyMMdd");
                obj.LAI_SUAT_MOI = Convert.ToDecimal(numLaiSuatMoi.Value);

                //Thông tin kiểm soát
                obj.TRANG_THAI_NGHIEP_VU = sTrangThaiNVu;
                obj.TRANG_THAI_BAN_GHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.NGAY_LAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                obj.NGUOI_LAP = txtNguoiLap.Text;
                obj.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                obj.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                //TinhLaiLSuat();
                List<DANH_SACH_THAY_DOI_LAI_SUAT> lstThayDoiLS = new List<DANH_SACH_THAY_DOI_LAI_SUAT>();
                foreach (DataRow dr in dsThayDoiLS.Tables[0].Rows)
                {
                    DANH_SACH_THAY_DOI_LAI_SUAT objCT = new DANH_SACH_THAY_DOI_LAI_SUAT();

                    //objCT.ID_TDOI_LSUAT_CT = 
                    //objCT.MA_TDOI_LSUAT_CT
                    objCT.ID_SO_SO_TG = Convert.ToInt32(dr["ID_TIEN_GUI"]);
                    objCT.SO_SO_TG = dr["SO_SO_TG"].ToString();
                    //objCT.MA_KHACH_HANG = 
                    objCT.TEN_KHACH_HANG = dr["TEN_KHANG"].ToString();
                    //objCT.MA_SAN_PHAM
                    //objCT.TEN_SAN_PHAM
                    objCT.NGAY_MO_SO = dr["NGAY_MO_SO"].ToString();
                    objCT.NGAY_DAO_HAN = dr["NGAY_DEN_HAN"].ToString();
                    //objCT.KY_HAN = Convert.ToInt32(dr["KY_HAN"].ToString()); 
                    //objCT.KY_HAN_DVI_TINH
                    objCT.SO_DU = Convert.ToInt32(dr["SO_DU"]);
                    //objCT.LOAI_TIEN
                    objCT.LAI_SUAT = Convert.ToDecimal(dr["LAI_SUAT_HIEN_TAI"]);
                    objCT.LAI_SUAT_MOI = Convert.ToDecimal(dr["LAI_SUAT_MOI"]);
                    objCT.TRANG_THAI_NGHIEP_VU = obj.TRANG_THAI_NGHIEP_VU;
                    objCT.TRANG_THAI_BAN_GHI = obj.TRANG_THAI_BAN_GHI;
                    objCT.NGAY_LAP = obj.NGAY_LAP;
                    objCT.NGUOI_LAP = obj.NGUOI_LAP;
                    objCT.NGAY_CAP_NHAT = obj.NGAY_CAP_NHAT;
                    objCT.NGUOI_CAP_NHAT = obj.NGUOI_CAP_NHAT;

                    lstThayDoiLS.Add(objCT);
                        
                }

                obj.DSACH_THAY_DOI_LAI_SUAT = lstThayDoiLS.ToArray();
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
                if (id != 0)
                {
                    HuyDongVonProcess huyDongVonProcess = new HuyDongVonProcess();
                    DataSet ds = huyDongVonProcess.GetThongTinThayDoiLaiSuat(id);
                    DataTable dtThongTinChung = ds.Tables["THAY_DOI"];
                    DataTable dtThongTinCT = ds.Tables["THAY_DOI_CT"];
                    if (dtThongTinChung != null && dtThongTinChung.Rows.Count > 0)
                    {
                        DataRow record = dtThongTinChung.Rows[0];
                        raddtNgayGD.Value = LDateTime.StringToDate(record["NGAY_TDOI"].ToString(), "yyyyMMdd");
                        raddtNgayApDung.Value = LDateTime.StringToDate(record["NGAY_ADUNG"].ToString(), "yyyyMMdd");
                        ngayApDung = LDateTime.StringToDate(record["NGAY_ADUNG"].ToString(), "yyyyMMdd");
                        numLaiSuatMoi.Value = Convert.ToDouble(record["LAI_SUAT_MOI"].ToString());
                        sTrangThaiNVu = record["TTHAI_NVU"].ToString();
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(record["TTHAI_NVU"].ToString());
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(record["TTHAI_BGHI"].ToString());
                        raddtNgayLap.Value = LDateTime.StringToDate(record["NGAY_NHAP"].ToString(), "yyyyMMdd");
                        if (!record["NGAY_CNHAT"].ToString().IsNullOrEmptyOrSpace())
                            raddtNgayCapNhat.Value = LDateTime.StringToDate(record["NGAY_CNHAT"].ToString(), "yyyyMMdd");
                        txtNguoiLap.Text = record["NGUOI_NHAP"].ToString();
                        txtNguoiCapNhat.Text = record["NGUOI_CNHAT"].ToString();
                    }
                    if (dtThongTinCT != null && dtThongTinCT.Rows.Count > 0)
                    {
                        //foreach (DataRow r in dtThongTinCT.Rows)
                        //{
                        //    r["LAI_SUAT_MOI"] = r["LAI_SUAT_MOI"].ToString().Replace(".0000","");
                        //}
                        dsThayDoiLS = new DataSet();
                        dsThayDoiLS.Tables.Add(dtThongTinCT.Copy());
                        grThayDoiLS.DataContext = dsThayDoiLS.Tables[0].DefaultView;

                        TinhTong();
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void ResetForm()
        {
            raddtNgayApDung.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            raddtNgayGD.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");

            dsThayDoiLS.Tables[0].Rows.Clear();
            grThayDoiLS.DataContext = null;

            lblTongSoSo.Content = 0;
            lblTongSoDu.Content = 0;            

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
                DataTable dt = dsThayDoiLS.Tables[0];

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

                string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));
                if (LObject.IsNullOrEmpty(obj))
                    obj = new HDV_THAY_DOI_LAI_SUAT();
                GetFormData(ref obj, trangThai);

                if (id==0)
                {
                    OnAddNew(ref obj);
                }
                else
                {
                    OnModify(ref obj);
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
                if(LObject.IsNullOrEmpty(obj))
                    obj = new HDV_THAY_DOI_LAI_SUAT();
                GetFormData(ref obj, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(ref obj);
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(ref obj);
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
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain);
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
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain);
        }

        public void OnAddNew(ref HDV_THAY_DOI_LAI_SUAT obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;
                List<HDV_THAY_DOI_LAI_SUAT> lst = new List<HDV_THAY_DOI_LAI_SUAT>();
                ret = processHDV.ThayDoiLaiSuat(function, DatabaseConstant.Action.THEM, ref lst, ref obj, ref listClientResponseDetail);
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

        public void AfterAddNew(bool ret, HDV_THAY_DOI_LAI_SUAT obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);
                    id = obj.ID_TDOI_LSUAT;
                    ma = obj.MA_TDOI_LSUAT;
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
                    //LMessage.ShowMessage("M.DungChung.ThemKhongThanhCong", LMessage.MessageBoxType.Error);
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
                    DatabaseConstant.Function.HDV_DIEU_CHINH_LS,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                if (ret)
                {
                    SetEnabledAllControls(true);
                    action = DatabaseConstant.Action.SUA;
                    CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain);
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
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnModify(ref HDV_THAY_DOI_LAI_SUAT obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                List<HDV_THAY_DOI_LAI_SUAT> lst = new List<HDV_THAY_DOI_LAI_SUAT>();
                ret = processHDV.ThayDoiLaiSuat(function, DatabaseConstant.Action.SUA, ref lst, ref obj, ref listClientResponseDetail);
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

        public void AfterModify(bool ret, HDV_THAY_DOI_LAI_SUAT obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);
                    id = obj.ID_TDOI_LSUAT;
                    ma = obj.MA_TDOI_LSUAT;
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
                    // LMessage.ShowMessage("M.DungChung.CapNhatKoThanhCong", LMessage.MessageBoxType.Error);
                }

                // Yêu cầu Unlock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_DIEU_CHINH_LS,
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
                    if (obj.IsNullOrEmpty())
                        obj = new HDV_THAY_DOI_LAI_SUAT();
                    obj.ID_TDOI_LSUAT = id;
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        DatabaseConstant.Function.HDV_DIEU_CHINH_LS,
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
                    DatabaseConstant.Function.HDV_DIEU_CHINH_LS,
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
                List<HDV_THAY_DOI_LAI_SUAT> lst = new List<HDV_THAY_DOI_LAI_SUAT>();
                ret = processHDV.ThayDoiLaiSuat(function, DatabaseConstant.Action.XOA, ref lst, ref obj, ref listClientResponseDetail);
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
                    DatabaseConstant.Function.HDV_DIEU_CHINH_LS,
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
                    if (obj.IsNullOrEmpty())
                        obj = new HDV_THAY_DOI_LAI_SUAT();
                    obj.ID_TDOI_LSUAT = id;
                    obj.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                    obj.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        DatabaseConstant.Function.HDV_DIEU_CHINH_LS,
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
                    DatabaseConstant.Function.HDV_DIEU_CHINH_LS,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.DUYET,
                    listLockId);

                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnApprove()
        {
            try
            {
                if (!Validation()) return;

                string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.DUYET, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));
                if (LObject.IsNullOrEmpty(obj))
                    obj = new HDV_THAY_DOI_LAI_SUAT();
                GetFormData(ref obj, trangThai);
                bool ret = false;
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<HDV_THAY_DOI_LAI_SUAT> lst = new List<HDV_THAY_DOI_LAI_SUAT>();
                ret = processHDV.ThayDoiLaiSuat(function, DatabaseConstant.Action.DUYET, ref lst, ref obj, ref listClientResponseDetail);
                AfterApprove(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public void AfterApprove(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);
                    id = obj.ID_TDOI_LSUAT;
                    ma = obj.MA_TDOI_LSUAT;
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
                    DatabaseConstant.Function.HDV_DIEU_CHINH_LS,
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
                    if (obj.IsNullOrEmpty())
                        obj = new HDV_THAY_DOI_LAI_SUAT();
                    obj.ID_TDOI_LSUAT = id;
                    obj.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                    obj.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        DatabaseConstant.Function.HDV_DIEU_CHINH_LS,
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
                    DatabaseConstant.Function.HDV_DIEU_CHINH_LS,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listLockId);

                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnCancel()
        {
            try
            {
                bool ret = false;
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<HDV_THAY_DOI_LAI_SUAT> lst = new List<HDV_THAY_DOI_LAI_SUAT>();
                ret = processHDV.ThayDoiLaiSuat(function, DatabaseConstant.Action.THOAI_DUYET, ref lst, ref obj, ref listClientResponseDetail);
                AfterCancel(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public void AfterCancel(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);
                    id = obj.ID_TDOI_LSUAT;
                    ma = obj.MA_TDOI_LSUAT;
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
                    DatabaseConstant.Function.HDV_DIEU_CHINH_LS,
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
                    if (obj.IsNullOrEmpty())
                        obj = new HDV_THAY_DOI_LAI_SUAT();
                    obj.ID_TDOI_LSUAT = id;
                    obj.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                    obj.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        DatabaseConstant.Function.HDV_DIEU_CHINH_LS,
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
                    DatabaseConstant.Function.HDV_DIEU_CHINH_LS,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);

                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnRefuse()
        {
            try
            {
                bool ret = false;
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<HDV_THAY_DOI_LAI_SUAT> lst = new List<HDV_THAY_DOI_LAI_SUAT>();
                ret = processHDV.ThayDoiLaiSuat(function, DatabaseConstant.Action.TU_CHOI_DUYET, ref lst, ref obj, ref listClientResponseDetail);
                AfterRefuse(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public void AfterRefuse(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);
                    id = obj.ID_TDOI_LSUAT;
                    ma = obj.MA_TDOI_LSUAT;
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
                    DatabaseConstant.Function.HDV_DIEU_CHINH_LS,
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
        #endregion        

    }
}
