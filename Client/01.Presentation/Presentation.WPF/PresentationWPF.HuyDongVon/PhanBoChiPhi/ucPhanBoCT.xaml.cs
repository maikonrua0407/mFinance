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

namespace PresentationWPF.HuyDongVon.PhanBoChiPhi
{
    /// <summary>
    /// Interaction logic for ucPhanBoCT.xaml
    /// </summary>
    public partial class ucPhanBoCT : UserControl
    {
        #region Khai bao

        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private DatabaseConstant.Function function = DatabaseConstant.Function.HDV_PHAN_BO;

        public event EventHandler OnSavingCompleted;

        private DataSet dsPhanBo;

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private KIEM_SOAT _objKiemSoat = null;

        private string sTrangThaiNVu = "";

        private string maGiaoDich = "";

        private bool isTinhPhanBo = false;

        private HDV_PHAN_BO_CHI_PHI obj;
        public HDV_PHAN_BO_CHI_PHI Obj
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
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        #endregion

        #region Khoi tao
        public ucPhanBoCT()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            LoadCombobox();

            KhoiTaoGridPB();

            radpage.PageSize = (int)nudPageSize.Value;

            btnAdd.Focus();
        }

        public ucPhanBoCT(KIEM_SOAT obj)
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            LoadCombobox();

            KhoiTaoGridPB();

            radpage.PageSize = (int)nudPageSize.Value;

            _objKiemSoat = obj;

            action = _objKiemSoat.action;

            sTrangThaiNVu = _objKiemSoat.TTHAI_NVU;

            isTinhPhanBo = true;

            this.obj = new HDV_PHAN_BO_CHI_PHI();

            btnAdd.Focus();
        }

        private void LoadCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>(); lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DONG_TIEN_CAC_NUOC.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, "VND"); 

        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.HuyDongVon;component/DuChi/ucDuChiCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void KhoiTaoGridPB()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("SO_SO_TG", typeof(string));
            dt.Columns.Add("MA_KHANG", typeof(string));
            dt.Columns.Add("TEN_KHANG", typeof(string));
            dt.Columns.Add("TEN_SAN_PHAM", typeof(string));
            dt.Columns.Add("MA_SAN_PHAM", typeof(string));
            dt.Columns.Add("NGAY_MO_SO", typeof(string));
            dt.Columns.Add("NGAY_DEN_HAN", typeof(string));
            dt.Columns.Add("KY_HAN", typeof(string));
            dt.Columns.Add("SO_DU", typeof(decimal));
            dt.Columns.Add("LAI_SUAT", typeof(decimal));
            dt.Columns.Add("SO_TIEN_PB", typeof(decimal));
            dt.Columns.Add("DA_PB_DEN_NGAY", typeof(string));
            dt.Columns.Add("SO_NGAY_TINH_PB", typeof(int));
            dt.Columns.Add("SO_TIEN_PB_KY_NAY", typeof(decimal));
            dt.Columns.Add("SO_TIEN_PB_LK", typeof(decimal));
            dt.Columns.Add("SO_TIEN_CON_PB", typeof(decimal));
            dsPhanBo = new DataSet();
            dsPhanBo.Tables.Add(dt);


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
            e.CanExecute = true;
        }
        private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //OnHold();
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
                //OnHold();
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
                //OnHold();
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
                DatabaseConstant.Function.HDV_PHAN_BO,
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
                PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grPhanBoDS, txtTimKiemNhanh.Text);
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
            if (grPhanBoDS != null && grPhanBoDS.ItemsSource != null)
            {
                radpage.PageSize = (int)nudPageSize.Value;
                grPhanBoDS.ItemsSource = dsPhanBo.Tables[0]; ;
            }
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(grPhanBoDS);
        }

        /// <summary>
        /// Hàm kiểm tra sổ đã được chọn trong Grid hay chưa
        /// </summary>
        /// <param name="sSoTGui">số sổ tiền gửi</param>
        /// <returns>true: chưa được chọn;  false đã được chọn</returns>
        private bool KiemTraSo(string sSoTGui)
        {
            foreach (DataRow dr in dsPhanBo.Tables[0].Rows)
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
                window.Title = LLanguage.SearchResourceByKey("U.HuyDongVon.PhanBoChiPhi.ucPhanBoCT.DSSoTienGuiPB");
                window.Content = uc;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
                if (lstPopup != null)
                {
                    isTinhPhanBo = false;

                    foreach (DataRow dr in lstPopup)
                    {
                        DataRow drPhanBo = dsPhanBo.Tables[0].NewRow();

                        if (KiemTraSo(dr["SO_SO_TG"].ToString()))
                        {
                            drPhanBo["ID"] = Convert.ToInt32(dr["ID"]);
                            drPhanBo["SO_SO_TG"] = dr["SO_SO_TG"];
                            drPhanBo["MA_KHANG"] = dr["MA_KHANG"];
                            drPhanBo["TEN_KHANG"] = dr["TEN_KHANG"];
                            drPhanBo["MA_SAN_PHAM"] = dr["MA_SAN_PHAM"];
                            drPhanBo["TEN_SAN_PHAM"] = dr["TEN_SAN_PHAM"];
                            drPhanBo["NGAY_MO_SO"] = dr["NGAY_MO_SO"];
                            drPhanBo["NGAY_DEN_HAN"] = dr["NGAY_DEN_HAN"];
                            drPhanBo["KY_HAN"] = dr["KY_HAN"];
                            drPhanBo["SO_DU"] = Convert.ToDecimal(dr["SO_DU"]);
                            drPhanBo["LAI_SUAT"] = Convert.ToDecimal(dr["LAI_SUAT"]);
                            drPhanBo["SO_TIEN_PB"] = 0;
                            drPhanBo["DA_PB_DEN_NGAY"] = "";
                            drPhanBo["SO_NGAY_TINH_PB"] =0;
                            drPhanBo["SO_TIEN_PB_KY_NAY"] = 0;
                            drPhanBo["SO_TIEN_PB_LK"] = 0;
                            drPhanBo["SO_TIEN_CON_PB"] = 0;

                            dsPhanBo.Tables[0].Rows.Add(drPhanBo);
                        }
                    }
                }

                for (int i = 0; i < dsPhanBo.Tables[0].Rows.Count; i++)
                {
                    dsPhanBo.Tables[0].Rows[i]["STT"] = i + 1;
                }

                DataViewManager dataViewManager = new DataViewManager(dsPhanBo);
                DataView dataView = dataViewManager.CreateDataView(dsPhanBo.Tables[0]);
                grPhanBoDS.DataContext = dataView;

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
                    for (int i = 0; i < grPhanBoDS.SelectedItems.Count; i++)
                    {
                        DataRow dr = (DataRow)grPhanBoDS.SelectedItems[i];
                        lstSTT.Add(Convert.ToInt32(dr["STT"]));
                    }
                    lstSTT.SortByDesc();
                    foreach (int stt in lstSTT)
                        dsPhanBo.Tables[0].Rows.RemoveAt(stt - 1);

                    for (int i = 0; i < dsPhanBo.Tables[0].Rows.Count; i++)
                    {
                        dsPhanBo.Tables[0].Rows[i]["STT"] = i + 1;
                    }

                    DataViewManager dataViewManager = new DataViewManager(dsPhanBo);
                    DataView dataView = dataViewManager.CreateDataView(dsPhanBo.Tables[0]);
                    grPhanBoDS.DataContext = dataView;

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
            TinhPhanBo();
            //HuyDongVonProcess processHDV = new HuyDongVonProcess();
            //DateTime denNgay = (DateTime)raddtDenNgay.Value;
            //int count = 0;
            //foreach (DataRow r in dsPhanBo.Tables[0].Rows)
            //{
            //    string sSoTGui = r["SO_SO_TG"].ToString();
            //    DataSet dsTinhPhanBo = processHDV.GetTTinPhanBo(sSoTGui);
            //    if (dsTinhPhanBo != null && dsTinhPhanBo.Tables.Count > 0)
            //    {
            //        DataRow thongtin = dsTinhPhanBo.Tables[0].Rows[0];
            //        //TLAI_TU_NGAY, TLAI_DEN_NGAY, PBO_SO_TIEN, PBO_DEN_NGAY, SO_TIEN_PBO_KY_LKE
            //        decimal soTienPB = Convert.ToDecimal(thongtin["PBO_SO_TIEN"].ToString());
            //        DateTime tinhLaiTuNgay = thongtin["TLAI_TU_NGAY"].ToString().StringToDate("yyyyMMdd");
            //        DateTime tinhLaiDenNgay = thongtin["TLAI_DEN_NGAY"].ToString().StringToDate("yyyyMMdd");
            //        DateTime tuNgay = new DateTime();
            //        if (thongtin["PBO_DEN_NGAY"].ToString().IsNullOrEmptyOrSpace())
            //            tuNgay = thongtin["TLAI_TU_NGAY"].ToString().StringToDate("yyyyMMdd");
            //        else
            //            tuNgay = thongtin["PBO_DEN_NGAY"].ToString().StringToDate("yyyyMMdd");
            //        int soNgayPB = 0;
            //        if (denNgay.CompareTo(tinhLaiDenNgay) <= 0)
            //            soNgayPB = LDateTime.CountDayBetweenDates(denNgay, tuNgay);
            //        else
            //            soNgayPB = LDateTime.CountDayBetweenDates(tinhLaiDenNgay, tuNgay);
            //        decimal soTienPBKyNay = 0;
            //        soTienPBKyNay = (soTienPB * soNgayPB) / (LDateTime.CountDayBetweenDates(tinhLaiDenNgay, tinhLaiTuNgay));
            //        decimal soTienPBLuyKe = Convert.ToDecimal(thongtin["SO_TIEN_PBO_KY_LKE"].ToString()) + soTienPBKyNay;
            //        r["SO_TIEN_PB"] = soTienPB;
            //        r["DA_PB_DEN_NGAY"] = tuNgay.DateToString("dd/MM/yyyy");
            //        r["SO_NGAY_TINH_PB"] = soNgayPB;
            //        r["SO_TIEN_PB_KY_NAY"] = soTienPBKyNay;
            //        r["SO_TIEN_PB_LK"] = soTienPBLuyKe;
            //        r["SO_TIEN_CON_PB"] = soTienPB - soTienPBLuyKe;
            //        count = count + 1;
            //    }
            //}
            //if (count > 0)
            //{
            //    grPhanBoDS.ItemsSource = null;
            //    grPhanBoDS.ItemsSource = dsPhanBo.Tables[0];

            //    TinhTong();
            //}
        }

        /// <summary>
        /// Hàm tính toán các thông tin liên quan đến danh sách sổ
        /// Tổng số sổ
        /// Tổng tiền Phân bổ
        /// Tổng tiền phần bổ trong kỳ
        /// Tổng tiền phân bổ lũy kế
        /// Tổng tiền còn phân bổ
        /// </summary>
        private void TinhTong()
        {
            try
            {
                int tongSoSo = dsPhanBo.Tables[0].Rows.Count;
                decimal tongTienPB = 0;
                decimal tongTienPBTrongKy= 0;
                decimal tongTienPBLK= 0;
                decimal tongTienConPB = 0;

                foreach (DataRow dr in dsPhanBo.Tables[0].Rows)
                {
                    tongTienPB = tongTienPB + Convert.ToDecimal(dr["SO_TIEN_PB"]);
                    tongTienPBTrongKy = tongTienPBTrongKy + Convert.ToDecimal(dr["SO_TIEN_PB_KY_NAY"]);
                    tongTienPBLK = tongTienPBLK + Convert.ToDecimal(dr["SO_TIEN_PB_LK"]);
                    tongTienConPB = tongTienConPB + Convert.ToDecimal(dr["SO_TIEN_CON_PB"]);
                }

                if (tongSoSo != 0)
                {
                    lblSumSoSo.Content = String.Format("{0:#,#}", tongSoSo);
                }
                else
                {
                    lblSumSoSo.Content = 0;
                }

                if (tongTienPB != 0)
                {
                    lblTongTienPB.Content = String.Format("{0:#,#}", tongTienPB);
                }
                else
                {
                    lblTongTienPB.Content = 0;
                }

                if (tongTienPBTrongKy != 0)
                {
                    lblTongTienPBTrongKy.Content = String.Format("{0:#,#}", tongTienPBTrongKy);
                }
                else
                {
                    lblTongTienPBTrongKy.Content = 0;
                }

                if (tongTienPBLK != 0)
                {
                    lblTongTienPBLK.Content = String.Format("{0:#,#}", tongTienPBLK);
                }
                else
                {
                    lblTongTienPBLK.Content = 0;
                }

                if (tongTienConPB != 0)
                {
                    lblTongTienConPB.Content = String.Format("{0:#,#}", tongTienConPB);
                }
                else
                {
                    lblTongTienConPB.Content = 0;
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
        private void GetFormData(ref HDV_PHAN_BO_CHI_PHI obj, string sTrangThaiNVu)
        {
            try
            {
                if (!maGiaoDich.IsNullOrEmptyOrSpace())
                    obj.MA_GDICH = maGiaoDich;
                obj.NGAY_GDICH = ClientInformation.NgayLamViecHienTai;
                //obj.Den = Convert.ToDateTime(raddtDenNgay.Value).ToString("yyyyMMdd");
                obj.LOAI_TIEN = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.ElementAt(0);
                obj.MA_DVI = ClientInformation.MaDonViGiaoDich;
                obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                obj.NGAY_GDICH = ClientInformation.NgayLamViecHienTai;
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

                List<DANH_SACH_PHAN_BO_CHI_PHI> lstLNG = new List<DANH_SACH_PHAN_BO_CHI_PHI>();
                foreach (DataRow dr in dsPhanBo.Tables[0].Rows)
                {
                    DANH_SACH_PHAN_BO_CHI_PHI objCT = new DANH_SACH_PHAN_BO_CHI_PHI();
                    objCT.SO_SO_TG = dr["SO_SO_TG"].ToString();
                    objCT.NGAY_MO_SO = dr["NGAY_MO_SO"].ToString().StringToDate("dd/MM/yyyy").DateToString("yyyyMMdd");
                    objCT.NGAY_DAO_HAN = dr["NGAY_DEN_HAN"].ToString().StringToDate("dd/MM/yyyy").DateToString("yyyyMMdd");
                    objCT.MA_SAN_PHAM = dr["MA_SAN_PHAM"].ToString();
                    objCT.MA_KHACH_HANG = dr["MA_KHANG"].ToString();
                    objCT.SO_DU = Convert.ToDecimal(dr["SO_DU"]);
                    objCT.LAI_SUAT = Convert.ToDecimal(dr["LAI_SUAT"]);
                    objCT.TEN_KHACH_HANG = dr["TEN_KHANG"].ToString();
                    objCT.DA_PHAN_BO_DEN_NGAY = dr["DA_PB_DEN_NGAY"].ToString().StringToDate("dd/MM/yyyy").DateToString("yyyyMMdd");
                    objCT.SO_NGAY_TINH_PHAN_BO = Convert.ToInt32(dr["SO_NGAY_TINH_PB"]);
                    objCT.SO_TIEN_CON_PHAN_BO = Convert.ToDecimal(dr["SO_TIEN_CON_PB"]);
                    objCT.SO_TIEN_PHAN_BO = Convert.ToDecimal(dr["SO_TIEN_PB"]);
                    objCT.SO_TIEN_PHAN_BO_KY_NAY = Convert.ToDecimal(dr["SO_TIEN_PB_KY_NAY"]);
                    objCT.SO_TIEN_PHAN_BO_LUY_KE = Convert.ToDecimal(dr["SO_TIEN_PB_LK"]);

                    lstLNG.Add(objCT);
                }

                obj.DSACH_PHAN_BO_CHI_PHI = lstLNG.ToArray();
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
                if (_objKiemSoat != null && !_objKiemSoat.SO_GIAO_DICH.IsNullOrEmptyOrSpace())
                {
                    maGiaoDich = _objKiemSoat.SO_GIAO_DICH;
                    HuyDongVonProcess processHDV = new HuyDongVonProcess();
                    DataSet ds = processHDV.GetTTinChiTietPhanBo(_objKiemSoat.SO_GIAO_DICH, ClientInformation.MaDonViGiaoDich);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drGiaoDich = ds.Tables[1].Rows[0];
                        DataTable dtChiTiet = ds.Tables[0];

                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                        #region Hiển thị thông tin giao dịch

                        raddtDenNgay.Value = LDateTime.StringToDate(drGiaoDich["NGAY_GDICH"].ToString(), "yyyyMMdd");
                        txtDienGiai.Text = drGiaoDich["DIEN_GIAI"].ToString();
                        #endregion

                        #region Tab thông tin kiểm soát
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(drGiaoDich["TTHAI_BGHI"].ToString());
                        raddtNgayLap.Value = LDateTime.StringToDate(drGiaoDich["NGAY_NHAP"].ToString(), "yyyyMMdd");
                        txtNguoiLap.Text = drGiaoDich["NGUOI_NHAP"].ToString();
                        if (LDateTime.IsDate(drGiaoDich["NGAY_CNHAT"].ToString(), "yyyyMMdd") == true)
                            raddtNgayCapNhat.Value = LDateTime.StringToDate(drGiaoDich["NGAY_CNHAT"].ToString(), "yyyyMMdd");
                        else
                            raddtNgayCapNhat.Value = null;
                        txtNguoiCapNhat.Text = drGiaoDich["NGUOI_CNHAT"].ToString();
                        #endregion

                        #region Hiển thị thông tin chi tiết Phân bổ

                        foreach (DataRow dr in dtChiTiet.Rows)
                        {
                            DataRow drPhanBo = dsPhanBo.Tables[0].NewRow();

                            if (KiemTraSo(dr["SO_SO_TG"].ToString()))
                            {
                                drPhanBo["ID"] = Convert.ToInt32(dr["ID"]);
                                drPhanBo["SO_SO_TG"] = dr["SO_SO_TG"];
                                drPhanBo["MA_KHANG"] = dr["MA_KHANG"];
                                drPhanBo["TEN_KHANG"] = dr["TEN_KHANG"];
                                drPhanBo["MA_SAN_PHAM"] = dr["MA_SAN_PHAM"];
                                drPhanBo["TEN_SAN_PHAM"] = dr["TEN_SAN_PHAM"];
                                drPhanBo["NGAY_MO_SO"] = dr["NGAY_MO_SO"];
                                drPhanBo["NGAY_DEN_HAN"] = dr["NGAY_DEN_HAN"];
                                drPhanBo["KY_HAN"] = dr["KY_HAN"];
                                drPhanBo["SO_DU"] = Convert.ToDecimal(dr["SO_DU"]);
                                drPhanBo["LAI_SUAT"] = Convert.ToDecimal(dr["LAI_SUAT"]);
                                drPhanBo["SO_TIEN_PB"] = Convert.ToDecimal(dr["SO_TIEN_PB"]);
                                drPhanBo["DA_PB_DEN_NGAY"] = dr["PBO_DEN_NGAY"].ToString();
                                drPhanBo["SO_NGAY_TINH_PB"] = Convert.ToInt32(dr["SO_NGAY_PBO"]);
                                drPhanBo["SO_TIEN_PB_KY_NAY"] = Convert.ToDecimal(dr["SO_TIEN_PBO_KY_NAY"]);
                                drPhanBo["SO_TIEN_PB_LK"] = Convert.ToDecimal(dr["SO_TIEN_PBO_KY_LKE"]);
                                drPhanBo["SO_TIEN_CON_PB"] = Convert.ToDecimal(dr["SO_TIEN_CON_PBO"]);

                                dsPhanBo.Tables[0].Rows.Add(drPhanBo);
                            }
                        }

                        for (int i = 0; i < dsPhanBo.Tables[0].Rows.Count; i++)
                        {
                            dsPhanBo.Tables[0].Rows[i]["STT"] = i + 1;
                        }

                        DataViewManager dataViewManager = new DataViewManager(dsPhanBo);
                        DataView dataView = dataViewManager.CreateDataView(dsPhanBo.Tables[0]);
                        grPhanBoDS.DataContext = dataView;

                        TinhTong();

                        #endregion
                    }
                }
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

            dsPhanBo.Tables[0].Rows.Clear();
            DataViewManager dataViewManager = new DataViewManager(dsPhanBo);
            DataView dataView = dataViewManager.CreateDataView(dsPhanBo.Tables[0]);
            grPhanBoDS.DataContext = dataView;

            lblSumSoSo.Content = 0;
            lblTongTienPB.Content = 0;
            lblTongTienPBTrongKy.Content = 0;
            lblTongTienPBLK.Content = 0;
            lblTongTienConPB.Content = 0;

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
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                foreach (DataRow dr in dsPhanBo.Tables[0].Rows)
                {
                    if (Convert.ToInt32(dr["SO_NGAY_TINH_PB"]) == 0 || Convert.ToDecimal(dr["SO_TIEN_PB_KY_NAY"]) == 0)
                    {
                        ClientResponseDetail responseDetail = new ClientResponseDetail();
                        responseDetail.Id = 0;
                        responseDetail.Object = dr["SO_SO_TG"].ToString();
                        responseDetail.Operation = DatabaseConstant.Action.THEM.layNgonNgu();
                        responseDetail.Result = ApplicationConstant.OperationStatus.Failed.layGiaTri();
                        responseDetail.Detail = "Số tiền phân bổ không hợp lệ";
                        listClientResponseDetail.Add(responseDetail);
                    }
                }
                if (listClientResponseDetail.Count > 0)
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
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


        public void OnSave()
        {
            try
            {
                if (!Validation()) return;

                string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));
                obj = new HDV_PHAN_BO_CHI_PHI();
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
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain,function);
            tlbPreview.IsEnabled = true;
        }

        public void BeforeViewFromList()
        {
            SetFormData();
            BeforeViewFromDetail();
        }


        public void BeforeAddNew()
        {
            //Thiết lập các thông tin mặc định
            raddtDenNgay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");

            txtTrangThaiBanGhi.Text = "";
            raddtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            raddtNgayCapNhat.Value = null;
            txtNguoiCapNhat.Text = "";

            SetEnabledAllControls(true);
            action = DatabaseConstant.Action.THEM;
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain,function);
        }

        public void OnAddNew(HDV_PHAN_BO_CHI_PHI obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processHDV.PhanBo(DatabaseConstant.Action.THEM, ref obj, ref listClientResponseDetail);
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

        public void AfterAddNew(bool ret, HDV_PHAN_BO_CHI_PHI obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);
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
                    DatabaseConstant.Function.HDV_PHAN_BO,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                if (ret)
                {
                    SetEnabledAllControls(true);
                    action = DatabaseConstant.Action.SUA;
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
            if (sTrangThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                SetEnabledAllControls(false);
            else
                SetEnabledAllControls(true);
            action = DatabaseConstant.Action.SUA;
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnModify(HDV_PHAN_BO_CHI_PHI obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processHDV.PhanBo(DatabaseConstant.Action.SUA, ref obj, ref listClientResponseDetail);
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

        public void AfterModify(bool ret, HDV_PHAN_BO_CHI_PHI obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);
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
                    DatabaseConstant.Function.HDV_PHAN_BO,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        DatabaseConstant.Function.HDV_PHAN_BO,
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
                    DatabaseConstant.Function.HDV_PHAN_BO,
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
                obj = new HDV_PHAN_BO_CHI_PHI();
                GetFormData(ref obj, sTrangThaiNVu);
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                ret = processHDV.PhanBo(action, ref obj, ref listClientResponseDetail);
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
                    DatabaseConstant.Function.HDV_PHAN_BO,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        DatabaseConstant.Function.HDV_PHAN_BO,
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
                    DatabaseConstant.Function.HDV_PHAN_BO,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.DUYET,
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
                obj = new HDV_PHAN_BO_CHI_PHI();
                GetFormData(ref obj, sTrangThaiNVu);
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                ret = processHDV.PhanBo(action, ref obj, ref listClientResponseDetail);
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
                    DatabaseConstant.Function.HDV_PHAN_BO,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        DatabaseConstant.Function.HDV_PHAN_BO,
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
                    DatabaseConstant.Function.HDV_PHAN_BO,
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
                obj = new HDV_PHAN_BO_CHI_PHI();
                GetFormData(ref obj, sTrangThaiNVu);
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                ret = processHDV.PhanBo(action, ref obj, ref listClientResponseDetail);
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
                    DatabaseConstant.Function.HDV_PHAN_BO,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        DatabaseConstant.Function.HDV_PHAN_BO,
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
                    DatabaseConstant.Function.HDV_PHAN_BO,
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
                obj = new HDV_PHAN_BO_CHI_PHI();
                GetFormData(ref obj, sTrangThaiNVu);
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                ret = processHDV.PhanBo(action, ref obj, ref listClientResponseDetail);
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
                    DatabaseConstant.Function.HDV_PHAN_BO,
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

        private void TinhPhanBo()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {

                if (dsPhanBo.Tables[0].Rows.Count == 0)
                {
                    LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                    return;
                }

                obj = new HDV_PHAN_BO_CHI_PHI();
                GetFormData(ref obj, sTrangThaiNVu);

                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                isTinhPhanBo = processHDV.PhanBo(DatabaseConstant.Action.TINH_TOAN, ref obj, ref listClientResponseDetail);

                AfterTinhPhanBo(isTinhPhanBo, obj, listClientResponseDetail);
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

        public void AfterTinhPhanBo(bool ret, HDV_PHAN_BO_CHI_PHI obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.TinhToanThanhCong", LMessage.MessageBoxType.Information);

                    foreach (DANH_SACH_PHAN_BO_CHI_PHI objCT in obj.DSACH_PHAN_BO_CHI_PHI)
                    {
                        for (int i = 0; i < dsPhanBo.Tables[0].Rows.Count; i++)
                        {
                            if (dsPhanBo.Tables[0].Rows[i]["SO_SO_TG"].ToString().Equals(objCT.SO_SO_TG))
                            {
                                //dsDuChi.Tables[0].Rows[i]["SO_NGAY_TINH_LAI"] = objCT.SO_NGAY_TINH_LAI;
                                //dsDuChi.Tables[0].Rows[i]["LAI_DU_CHI"] = objCT.LAI_DU_CHI;
                                //dsDuChi.Tables[0].Rows[i]["LAI_DU_CHI_LK"] = objCT.LAI_DU_CHI_LUY_KE;
                                //break;
                            }
                        }
                    }                    

                    DataViewManager dataViewManager = new DataViewManager(dsPhanBo);
                    DataView dataView = dataViewManager.CreateDataView(dsPhanBo.Tables[0]);
                    grPhanBoDS.DataContext = dataView;

                    TinhTong();
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

        #endregion    

        private void cbMultiAdd_Checked(object sender, RoutedEventArgs e)
        {
            //reset biến
            obj = null;
            _objKiemSoat = null;
            id = 0;
            maGiaoDich = "";
            sTrangThaiNVu = "";
            isTinhPhanBo = false;
            
            BeforeAddNew();

            cbMultiAdd.IsChecked = false;
            btnAdd.Focus();
        }
    }
}
