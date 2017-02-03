using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
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
using PresentationWPF.CustomControl;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.HuyDongVonServiceRef;
using Presentation.Process.LaiSuatServiceRef;
using Presentation.Process.KeToanServiceRef;
using Presentation.Process.KhachHangServiceRef;
using PresentationWPF.KhachHang.KhachHang;
using Presentation.Process.BaoCaoServiceRef;
using PresentationWPF.BaoCao.DungChung;
using System.IO;

namespace PresentationWPF.HuyDongVon.MoSo
{
    /// <summary>
    /// Interaction logic for ucTienGuiCoKyHanCT.xaml
    /// </summary>
    public partial class ucTienGuiCoKyHanCT : UserControl
    {
        #region Khai bao

        private DatabaseConstant.Function function;
        public DatabaseConstant.Function Function
        {
            get { return function; }
            set { function = value; }
        }

        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        public event EventHandler OnSavingCompleted;

        BaoCaoProcess process = new BaoCaoProcess();

        /*Cờ đánh dấu trạng thái khi LoadForm: 
         * 0 là khi gọi từ Main chương trình lần đầu
         * 1 là khi thêm từ Form danh sách
         * 2 là khi sửa từ Form danh sách 
         * 3 là khi xem từ Form danh sách
         * -1 là Khi đã load Form tránh trường hợp load nhiều lần
        */
        private int flag = 0;
        public int Flag
        {
            get { return flag; }
            set { flag = value; }
        }

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private string sNhomSP;

        private string maGiaoDich;
        public string MaGiaoDich
        {
            get { return maGiaoDich; }
            set { maGiaoDich = value; }
        }

        private int idKhachHang = -1;
        private int idSanPham = -1;
        private int idLaiSuat = 0;

        private string sTrangThaiNVu = "";

        public static string formCase = null;

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        private DataSet dsDCSH = null;

        List<AutoCompleteEntry> lstSourceTanSuat = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKyHan_DVi = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLaiSuat_DVi = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNguonVon = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNguonVonCT = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceGD_HinhThuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCBQL = new List<AutoCompleteEntry>();

        List<AutoCompleteEntry> lstSourceDHan_ChiThi = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDHan_KyHan_DVi = new List<AutoCompleteEntry>();

        List<AutoCompleteEntry> lstSourceTLai_TanSuat_DVi = new List<AutoCompleteEntry>();

        List<AutoCompleteEntry> lstSourceGGop_TanSuat_DVi = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceGGop_HinhThuc_CNgay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceGGop_HinhThuc = new List<AutoCompleteEntry>();

        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        public static RoutedCommand PreviewXemSo = new RoutedCommand();
        public static RoutedCommand PreviewXemCT = new RoutedCommand();

        private KIEM_SOAT _objKiemSoat = null;
        private string ngayGiaoDich = ClientInformation.NgayLamViecHienTai;
        #endregion

        #region Khoi tao
        public ucTienGuiCoKyHanCT()
        {
            InitializeComponent();

            //Dispatcher.CurrentDispatcher.DelayInvoke("DuyetQuyenTinhNang", () =>
            //{
            //    DuyetQuyenTinhNang();
            //}, TimeSpan.FromSeconds(0));

            //Dispatcher.CurrentDispatcher.DelayInvoke("LoadCombobox", () =>
            //{
            //    LoadCombobox();
            //}, TimeSpan.FromSeconds(0));

            //Dispatcher.CurrentDispatcher.DelayInvoke("KhoiTaoGridDCSH", () =>
            //{
            //    KhoiTaoGridDCSH();
            //}, TimeSpan.FromSeconds(0));

            //Dispatcher.CurrentDispatcher.DelayInvoke("BindShortkey", () =>
            //{
            //    BindShortkey();
            //}, TimeSpan.FromSeconds(0));


            DuyetQuyenTinhNang();
            LoadCombobox();
            KhoiTaoGridDCSH();
            BindShortkey();

            txtMaKhachHang.Focus();

            // In thông tin báo cáo
            if (ClientInformation.Company.Equals("BANTAYVANG") || ClientInformation.Company.Equals("HOCVIENNGANHANG"))
            {
                tlbPreviewXemCT.Visibility = Visibility.Collapsed;
                tlbPreviewGiayGuiTien.Visibility = Visibility.Visible;
            }
            else
            {
                tlbPreviewXemCT.Visibility = Visibility.Visible;
                tlbPreviewGiayGuiTien.Visibility = Visibility.Collapsed;
            }
            ShowControl();
        }

        public ucTienGuiCoKyHanCT(KIEM_SOAT obj)
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            DateTime dt = DateTime.Now;
            LoadCombobox();
            LLogging.WriteLog("LoadCombobox()", LLogging.LogType.BUS, DateTime.Now.Subtract(dt).TotalMilliseconds.ToString());

            KhoiTaoGridDCSH();

            ResetForm();

            BindShortkey();

            _objKiemSoat = obj;

            flag = 2;

            txtMaKhachHang.Focus();

            // In thông tin báo cáo
            if (ClientInformation.Company.Equals("BANTAYVANG"))
            {
                tlbPreviewXemCT.Visibility = Visibility.Collapsed;
                tlbPreviewGiayGuiTien.Visibility = Visibility.Visible;
            }
            else
            {
                tlbPreviewXemCT.Visibility = Visibility.Visible;
                tlbPreviewGiayGuiTien.Visibility = Visibility.Collapsed;
            }
            ShowControl();
        }

        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT", "");
            foreach (List<string> lst in arr)
            {
                object item = Toolbar.FindName(lst.First());
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
        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.HuyDongVon;component/MoSo/ucTienGuiCoKyHanCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void KhoiTaoGridDCSH()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("MA_KHANG", typeof(string));
            dt.Columns.Add("TEN_KHANG", typeof(string));
            dt.Columns.Add("DIA_CHI", typeof(string));
            dsDCSH = new DataSet();
            dsDCSH.Tables.Add(dt);


        }

        /// <summary>
        /// Load combobox
        /// </summary>
        private void LoadCombobox()
        {
            try
            {
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();

                //Load combobox loại tiền - Tab thông tin chung - Group sổ tiền gửi
                //Dispatcher.CurrentDispatcher.DelayInvoke("cmbLoaiTien", () =>
                //{
                    auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TIENTE.getValue(), null, ClientInformation.MaDongNoiTe);
                //}, TimeSpan.FromSeconds(0));

                //Load combobox kỳ hạn - Tab thông tin chung - Group sổ tiền gửi 
                //Dispatcher.CurrentDispatcher.DelayInvoke("cmbKyHan_DVi_Tinh", () =>
                //{
                    lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.TAN_SUAT));
                    auto.GenAutoComboBox(ref lstSourceTanSuat, ref cmbKyHan_DVi_Tinh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, "THANG");
                    lstSourceKyHan_DVi = auto.CopyListEntry(lstSourceTanSuat);
                    auto.removeEntry(ref lstSourceKyHan_DVi, ref cmbKyHan_DVi_Tinh, BusinessConstant.TAN_SUAT.QUY.layGiaTri(), BusinessConstant.TAN_SUAT.THANG.layGiaTri());
                //}, TimeSpan.FromSeconds(0));

                //Load combobox lãi suất - Tab thông tin chung - Group sổ tiền gửi   
                //Dispatcher.CurrentDispatcher.DelayInvoke("cmbLaiSuat_DVi_Tinh", () =>
                //{
                    lstSourceLaiSuat_DVi = auto.CopyListEntry(lstSourceTanSuat);
                    auto.GenAutoComboBoxBySource(ref lstSourceLaiSuat_DVi, ref cmbLaiSuat_DVi_Tinh, BusinessConstant.TAN_SUAT.NAM.layGiaTri());
                    auto.removeEntry(ref lstSourceLaiSuat_DVi, ref cmbLaiSuat_DVi_Tinh, BusinessConstant.TAN_SUAT.QUY.layGiaTri(), BusinessConstant.TAN_SUAT.NAM.layGiaTri());
                //}, TimeSpan.FromSeconds(0));


                //Nguồn vốn
                    auto.GenAutoComboBox(ref lstSourceNguonVon, ref cmbNguonVon, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NGUON_VON.getValue(), null);
                    cmbNguonVon.SelectedIndex = 0;

                    auto.GenAutoComboBox(ref lstSourceNguonVonCT, ref cmbNguonVonCT, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NGUON_VON_CT.getValue(), null);
                    cmbNguonVonCT.SelectedIndex = 0;


                //Load combobox Hình thức giao dịch - Tab thông tin chung - Group thông tin giao dịch
                //Dispatcher.CurrentDispatcher.DelayInvoke("cmbGDHinhThuc", () =>
                //{
                    lstDieuKien.Clear();
                    lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.HINH_THUC_GIAO_DICH));
                    auto.GenAutoComboBox(ref lstSourceGD_HinhThuc, ref cmbGDHinhThuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
                    cmbGDHinhThuc.SelectedIndex = 0;
                //}, TimeSpan.FromSeconds(0));


                //Load combobox cán bộ quản lý - Tab thông tin chung - Group thông tin khác
                //Dispatcher.CurrentDispatcher.DelayInvoke("cmbMaCBQL", () =>
                //{
                    lstDieuKien.Clear();
                    lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                    auto.GenAutoComboBox(ref lstSourceCBQL, ref cmbMaCBQL, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NSD.getValue(), lstDieuKien);
                    cmbMaCBQL.SelectedIndex = lstSourceCBQL.IndexOf(lstSourceCBQL.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TenDangNhap)));
                //}, TimeSpan.FromSeconds(0));

                //Load combobox chỉ thị lựa chọn đáo hạn - Tab chỉ thị đáo hạn - Group chỉ thị đáo hạn
                //Dispatcher.CurrentDispatcher.DelayInvoke("cmbDHan_CThi", () =>
                //{
                    lstDieuKien.Clear();
                    lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.CHI_THI_DAO_HAN));
                    auto.GenAutoComboBox(ref lstSourceDHan_ChiThi, ref cmbDHan_CThi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, BusinessConstant.layGiaTri(BusinessConstant.CHI_THI_DAO_HAN.SPQV_LSM_KHM));
                //}, TimeSpan.FromSeconds(0));

                //Load combobox đơn vị tính kỳ hạn mới - Tab chỉ thị đáo hạn - Group chỉ thị đáo hạn       
                //Dispatcher.CurrentDispatcher.DelayInvoke("cmbDHan_KyHan_DViTinh", () =>
                //{
                    lstSourceDHan_KyHan_DVi = auto.CopyListEntry(lstSourceTanSuat);
                    auto.GenAutoComboBoxBySource(ref lstSourceDHan_KyHan_DVi, ref cmbDHan_KyHan_DViTinh, BusinessConstant.TAN_SUAT.THANG.layGiaTri());
                    auto.removeEntry(ref lstSourceDHan_KyHan_DVi, ref cmbDHan_KyHan_DViTinh, BusinessConstant.TAN_SUAT.QUY.layGiaTri(), BusinessConstant.TAN_SUAT.THANG.layGiaTri());
                //}, TimeSpan.FromSeconds(0));


                //Load combobox đơn vị tính tần suất trả lãi - Tab lập lịch trả lãi- Group lựa chọn lập lịch trả lãi
                //Dispatcher.CurrentDispatcher.DelayInvoke("cmbTLai_TanSuat_DViTinh", () =>
                //{
                    lstSourceTLai_TanSuat_DVi = auto.CopyListEntry(lstSourceTanSuat);
                    auto.GenAutoComboBoxBySource(ref lstSourceTLai_TanSuat_DVi, ref cmbTLai_TanSuat_DViTinh, BusinessConstant.TAN_SUAT.THANG.layGiaTri());
                    auto.removeEntry(ref lstSourceTLai_TanSuat_DVi, ref cmbTLai_TanSuat_DViTinh, BusinessConstant.TAN_SUAT.QUY.layGiaTri(), BusinessConstant.TAN_SUAT.THANG.layGiaTri());
                //}, TimeSpan.FromSeconds(0));

                //Load combobox đơn vị tính tần suất gửi góp - Tab trả góp- Group lựa chọn gửi góp
                //Dispatcher.CurrentDispatcher.DelayInvoke("cmbGGop_TSuat", () =>
                //{
                    lstSourceGGop_TanSuat_DVi = auto.CopyListEntry(lstSourceTanSuat);
                    auto.GenAutoComboBoxBySource(ref lstSourceGGop_TanSuat_DVi, ref cmbGGop_TSuat, BusinessConstant.TAN_SUAT.THANG.layGiaTri());
                    auto.removeEntry(ref lstSourceGGop_TanSuat_DVi, ref cmbGGop_TSuat, BusinessConstant.TAN_SUAT.QUY.layGiaTri(), BusinessConstant.TAN_SUAT.THANG.layGiaTri());
                //}, TimeSpan.FromSeconds(0));

                //Load combobox ngày gửi rơi vào ngày nghỉ - Tab trả góp- Group lựa chọn gửi góp
                //lstDieuKien.Clear();
                //lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.HINH_THUC_CHUYEN_NGAY_GGOP));
                //auto.GenAutoComboBox(ref lstSourceGGop_HinhThuc_CNgay, ref cmbGGop_HinhThuc_CNgay, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
                //cmbGGop_HinhThuc_CNgay.SelectedIndex = 0;

                //Load combobox hình thức đăng ký gửi góp - Tab trả góp- Group lựa chọn gửi góp
                //lstDieuKien.Clear();
                //lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.HINH_THUC_GUI_GOP));
                //auto.GenAutoComboBox(ref lstSourceGGop_HinhThuc, ref cmbGGop_HinhThuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
                //cmbGGop_HinhThuc.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Set giá trị mặc định cho các control
        /// </summary>
        private void ResetForm()
        {
            try
            {
                lblTrangThai.Content = "";

                // Hiện thị thông tin lên Form theo sổ tiết kiệm quy định
                if (function == DatabaseConstant.Function.HDV_SO_TKQD)
                {

                    #region Tab thông tin chung
                    //Group Thông tin khách hàng
                    idKhachHang = -1;
                    txtMaKhachHang.Text = "";
                    lblTenKH.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenKhachHang");
                    txtDiaChi.Text = "";
                    txtCMND.Text = "";
                    raddtNgayCap.Value = null;
                    txtNoiCap.Text = "";
                    txtSDT.Text = "";

                    //Group Thông tin sổ tiền gửi                    
                    txtSoGiaoDich.Text = "";
                    txtSoSoTG.Text = "";
                    idSanPham = -1;
                    txtMaSanPham.Text = "";
                    lblTenSanPham.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenSanPham");
                    numSoTien.Value = 0;
                    numKyHan.Value = null;
                    cmbKyHan_DVi_Tinh.SelectedIndex = -1;
                    numLaiSuat.Value = 0;
                    cmbLaiSuat_DVi_Tinh.SelectedIndex = lstSourceLaiSuat_DVi.IndexOf(lstSourceLaiSuat_DVi.FirstOrDefault(e => e.KeywordStrings.First().Equals(BusinessConstant.TAN_SUAT.NAM.layGiaTri())));
                    cmbLaiSuat_DVi_Tinh.IsEnabled = false;
                    raddtNgayMo.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                    raddtNgayDaoHan.Value = null;
                    cmbNguonVon.SelectedIndex = 0;
                    cmbNguonVonCT.SelectedIndex = 0;

                    //Group giao dịch
                    cmbGDHinhThuc.SelectedIndex = lstSourceGD_HinhThuc.IndexOf(lstSourceGD_HinhThuc.FirstOrDefault(e => e.KeywordStrings.First().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri())));
                    numGDTienMat.Value = 0;
                    numGDTienCKhoan.Value = 0;
                    txtGD_TKhoan_KH.Text = "";
                    lblGD_TKhoan_KH.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenTaiKhoanKH");
                    txtGD_TKhoan_NoiBo.Text = "";
                    lblGD_TKhoan_NoiBo.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenTaiKhoanNoiBo");
                    txtDienGiai.Text = "";

                    //Group thông tin khác
                    cmbMaCBQL.SelectedIndex = lstSourceCBQL.IndexOf(lstSourceCBQL.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TenDangNhap)));
                    txtSoKU.Text = "";
                    numSoDuGoc.Value = null;
                    txtSoTienGuiMoiKy.Value = 0;
                    raddtNgayADKU.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                    #endregion

                    #region Tab đồng chủ sở hữu
                    lblDCSH_ID.Content = "-1";
                    txtDCSH_MaKH.Text = "";
                    lblDCSH_TenKH.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenKhachHangDCSH");
                    txtDCSH_DiaChi.Text = "";
                    txtDCSH_CMND.Text = "";
                    txtDCSH_NoiCap.Text = "";
                    txtDCSH_SDT.Text = "";
                    raddtDCSH_NgayCap.Value = null;
                    dsDCSH.Tables[0].Rows.Clear();
                    DataViewManager dataViewManager = new DataViewManager(dsDCSH);
                    DataView dataView = dataViewManager.CreateDataView(dsDCSH.Tables[0]);
                    grDongSoHuuDS.DataContext = dataView;
                    #endregion

                    tbiSoDuTienLai.Visibility = System.Windows.Visibility.Collapsed;
                    tbiLichSuGD.Visibility = System.Windows.Visibility.Collapsed;
                    tbiLichSuLaiSuat.Visibility = System.Windows.Visibility.Collapsed;

                }

                // Hiện thị thông tin lên Form theo sổ tiết kiệm không kỳ hạn
                else if (function == DatabaseConstant.Function.HDV_SO_TKKKH)
                {
                    #region Tab thông tin chung
                    //Group Thông tin khách hàng
                    idKhachHang = -1;
                    txtMaKhachHang.Text = "";
                    lblTenKH.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenKhachHang");
                    txtDiaChi.Text = "";
                    txtCMND.Text = "";
                    raddtNgayCap.Value = null;
                    txtNoiCap.Text = "";
                    txtSDT.Text = "";

                    //Group Thông tin sổ tiền gửi
                    txtSoGiaoDich.Text = "";
                    txtSoSoTG.Text = "";
                    idSanPham = -1;
                    txtMaSanPham.Text = "";
                    lblTenSanPham.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenSanPham");
                    numSoTien.Value = 0;
                    numKyHan.Value = null;
                    cmbKyHan_DVi_Tinh.SelectedIndex = -1;
                    numLaiSuat.Value = 0;
                    cmbLaiSuat_DVi_Tinh.SelectedIndex = lstSourceLaiSuat_DVi.IndexOf(lstSourceLaiSuat_DVi.FirstOrDefault(e => e.KeywordStrings.First().Equals(BusinessConstant.TAN_SUAT.NAM.layGiaTri())));
                    raddtNgayMo.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                    raddtNgayDaoHan.Value = null;
                    cmbNguonVon.SelectedIndex = 0;
                    cmbNguonVonCT.SelectedIndex = 0;

                    //Group giao dịch
                    cmbGDHinhThuc.SelectedIndex = lstSourceGD_HinhThuc.IndexOf(lstSourceGD_HinhThuc.FirstOrDefault(e => e.KeywordStrings.First().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri())));
                    numGDTienMat.Value = 0;
                    numGDTienCKhoan.Value = 0;
                    txtGD_TKhoan_KH.Text = "";
                    lblGD_TKhoan_KH.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenTaiKhoanKH");
                    txtGD_TKhoan_NoiBo.Text = "";
                    lblGD_TKhoan_NoiBo.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenTaiKhoanNoiBo");
                    txtDienGiai.Text = "";

                    //Group thông tin khác
                    cmbMaCBQL.SelectedIndex = lstSourceCBQL.IndexOf(lstSourceCBQL.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TenDangNhap)));
                    #endregion

                    #region Tab đồng chủ sở hữu
                    lblDCSH_ID.Content = "-1";
                    txtDCSH_MaKH.Text = "";
                    lblDCSH_TenKH.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenKhachHangDCSH");
                    txtDCSH_DiaChi.Text = "";
                    txtDCSH_CMND.Text = "";
                    txtDCSH_NoiCap.Text = "";
                    txtDCSH_SDT.Text = "";
                    raddtDCSH_NgayCap.Value = null;
                    dsDCSH.Tables[0].Rows.Clear();
                    DataViewManager dataViewManager = new DataViewManager(dsDCSH);
                    DataView dataView = dataViewManager.CreateDataView(dsDCSH.Tables[0]);
                    grDongSoHuuDS.DataContext = dataView;
                    #endregion

                    tbiSoDuTienLai.Visibility = System.Windows.Visibility.Collapsed;
                    tbiLichSuGD.Visibility = System.Windows.Visibility.Collapsed;
                    tbiLichSuLaiSuat.Visibility = System.Windows.Visibility.Collapsed;
                }

                // Hiện thị thông tin lên Form theo sổ tiết kiệm có kỳ hạn
                else if (function == DatabaseConstant.Function.HDV_SO_TKCKH)
                {

                    #region Tab thông tin chung
                    //Group Thông tin khách hàng
                    idKhachHang = -1;
                    txtMaKhachHang.Text = "";
                    lblTenKH.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenKhachHang");
                    txtDiaChi.Text = "";
                    txtCMND.Text = "";
                    raddtNgayCap.Value = null;
                    txtNoiCap.Text = "";
                    txtSDT.Text = "";

                    //Group Thông tin sổ tiền gửi
                    txtSoGiaoDich.Text = "";
                    txtSoSoTG.Text = "";
                    idSanPham = -1;
                    txtMaSanPham.Text = "";
                    lblTenSanPham.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenSanPham");
                    numSoTien.Value = 0;
                    numKyHan.Value = 0;
                    cmbKyHan_DVi_Tinh.SelectedIndex = lstSourceKyHan_DVi.IndexOf(lstSourceKyHan_DVi.FirstOrDefault(e => e.KeywordStrings.First().Equals(BusinessConstant.TAN_SUAT.THANG.layGiaTri())));
                    numLaiSuat.Value = 0;
                    cmbLaiSuat_DVi_Tinh.SelectedIndex = lstSourceLaiSuat_DVi.IndexOf(lstSourceLaiSuat_DVi.FirstOrDefault(e => e.KeywordStrings.First().Equals(BusinessConstant.TAN_SUAT.NAM.layGiaTri())));
                    raddtNgayMo.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                    raddtNgayDaoHan.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                    cmbNguonVon.SelectedIndex = 0;
                    cmbNguonVonCT.SelectedIndex = 0;

                    //Group giao dịch
                    cmbGDHinhThuc.SelectedIndex = lstSourceGD_HinhThuc.IndexOf(lstSourceGD_HinhThuc.FirstOrDefault(e => e.KeywordStrings.First().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri())));
                    numGDTienMat.Value = 0;
                    numGDTienCKhoan.Value = 0;
                    txtGD_TKhoan_KH.Text = "";
                    lblGD_TKhoan_KH.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenTaiKhoanKH");
                    txtGD_TKhoan_NoiBo.Text = "";
                    lblGD_TKhoan_NoiBo.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenTaiKhoanNoiBo");
                    txtDienGiai.Text = "";

                    //Group thông tin khác
                    cmbMaCBQL.SelectedIndex = lstSourceCBQL.IndexOf(lstSourceCBQL.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TenDangNhap)));
                    #endregion

                    #region Tab chỉ thị đáo hạn
                    #endregion

                    #region Tab đồng chủ sở hữu
                    lblDCSH_ID.Content = "-1";
                    txtDCSH_MaKH.Text = "";
                    lblDCSH_TenKH.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenKhachHangDCSH");
                    txtDCSH_DiaChi.Text = "";
                    txtDCSH_CMND.Text = "";
                    txtDCSH_NoiCap.Text = "";
                    txtDCSH_SDT.Text = "";
                    raddtDCSH_NgayCap.Value = null;
                    dsDCSH.Tables[0].Rows.Clear();
                    DataViewManager dataViewManager = new DataViewManager(dsDCSH);
                    DataView dataView = dataViewManager.CreateDataView(dsDCSH.Tables[0]);
                    grDongSoHuuDS.DataContext = dataView;
                    #endregion

                    tbiSoDuTienLai.Visibility = System.Windows.Visibility.Collapsed;
                    tbiLichSuGD.Visibility = System.Windows.Visibility.Collapsed;
                    tbiLichSuLaiSuat.Visibility = System.Windows.Visibility.Collapsed;
                }

                // Hiện thị thông tin lên Form theo sổ tài khoản tiền gửi thanh toán
                else if (function == DatabaseConstant.Function.HDV_SO_TK_TGTT)
                {

                    #region Tab thông tin chung
                    //Group Thông tin khách hàng
                    idKhachHang = -1;
                    txtMaKhachHang.Text = "";
                    lblTenKH.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenKhachHang");
                    txtDiaChi.Text = "";
                    txtCMND.Text = "";
                    raddtNgayCap.Value = null;
                    txtNoiCap.Text = "";
                    txtSDT.Text = "";

                    //Group Thông tin sổ tiền gửi
                    txtSoGiaoDich.Text = "";
                    lblSoTGui.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenTaiKhoan");
                    txtSoSoTG.Text = "";
                    txtTenTaiKhoan.Text = "";
                    idSanPham = -1;
                    txtMaSanPham.Text = "";
                    lblTenSanPham.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenSanPham");
                    numSoTien.Value = 0;
                    numKyHan.Value = null;
                    cmbKyHan_DVi_Tinh.SelectedIndex = -1;
                    numLaiSuat.Value = 0;
                    cmbLaiSuat_DVi_Tinh.SelectedIndex = lstSourceLaiSuat_DVi.IndexOf(lstSourceLaiSuat_DVi.FirstOrDefault(e => e.KeywordStrings.First().Equals(BusinessConstant.TAN_SUAT.NAM.layGiaTri())));
                    raddtNgayMo.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                    raddtNgayDaoHan.Value = null;
                    cmbNguonVon.SelectedIndex = 0;
                    cmbNguonVonCT.SelectedIndex = 0;

                    //Group giao dịch
                    cmbGDHinhThuc.SelectedIndex = lstSourceGD_HinhThuc.IndexOf(lstSourceGD_HinhThuc.FirstOrDefault(e => e.KeywordStrings.First().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri())));
                    numGDTienMat.Value = 0;
                    numGDTienCKhoan.Value = 0;
                    txtGD_TKhoan_KH.Text = "";
                    lblGD_TKhoan_KH.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenTaiKhoanKH");
                    txtGD_TKhoan_NoiBo.Text = "";
                    lblGD_TKhoan_NoiBo.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenTaiKhoanNoiBo");
                    txtDienGiai.Text = "";

                    //Group thông tin khác
                    cmbMaCBQL.SelectedIndex = lstSourceCBQL.IndexOf(lstSourceCBQL.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TenDangNhap)));
                    #endregion

                    #region Tab đồng chủ sở hữu
                    lblDCSH_ID.Content = "-1";
                    txtDCSH_MaKH.Text = "";
                    lblDCSH_TenKH.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenKhachHangDCSH");
                    txtDCSH_DiaChi.Text = "";
                    txtDCSH_CMND.Text = "";
                    txtDCSH_NoiCap.Text = "";
                    txtDCSH_SDT.Text = "";
                    raddtDCSH_NgayCap.Value = null;
                    dsDCSH.Tables[0].Rows.Clear();
                    DataViewManager dataViewManager = new DataViewManager(dsDCSH);
                    DataView dataView = dataViewManager.CreateDataView(dsDCSH.Tables[0]);
                    grDongSoHuuDS.DataContext = dataView;
                    #endregion

                    tbiSoDuTienLai.Visibility = System.Windows.Visibility.Collapsed;
                    tbiLichSuGD.Visibility = System.Windows.Visibility.Collapsed;
                    tbiLichSuLaiSuat.Visibility = System.Windows.Visibility.Collapsed;
                }

                // Hiện thị thông tin lên Form theo sổ tiền gửi có kỳ hạn
                else if (function == DatabaseConstant.Function.HDV_SO_TGCKH)
                {

                    #region Tab thông tin chung
                    //Group Thông tin khách hàng
                    idKhachHang = -1;
                    txtMaKhachHang.Text = "";
                    lblTenKH.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenKhachHang");
                    txtDiaChi.Text = "";
                    txtCMND.Text = "";
                    raddtNgayCap.Value = null;
                    txtNoiCap.Text = "";
                    txtSDT.Text = "";

                    //Group Thông tin sổ tiền gửi
                    txtSoGiaoDich.Text = "";
                    txtSoSoTG.Text = "";
                    idSanPham = -1;
                    txtMaSanPham.Text = "";
                    lblTenSanPham.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenSanPham");
                    cmbLoaiTien.SelectedIndex = 0;
                    numSoTien.Value = 0;
                    numKyHan.Value = 0;
                    cmbKyHan_DVi_Tinh.SelectedIndex = lstSourceKyHan_DVi.IndexOf(lstSourceKyHan_DVi.FirstOrDefault(e => e.KeywordStrings.First().Equals(BusinessConstant.TAN_SUAT.THANG.layGiaTri())));
                    numLaiSuat.Value = 0;
                    cmbLaiSuat_DVi_Tinh.SelectedIndex = lstSourceLaiSuat_DVi.IndexOf(lstSourceLaiSuat_DVi.FirstOrDefault(e => e.KeywordStrings.First().Equals(BusinessConstant.TAN_SUAT.THANG.layGiaTri())));
                    raddtNgayMo.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                    raddtNgayDaoHan.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                    cmbNguonVon.SelectedIndex = 0;
                    cmbNguonVonCT.SelectedIndex = 0;

                    //Group giao dịch
                    cmbGDHinhThuc.SelectedIndex = lstSourceGD_HinhThuc.IndexOf(lstSourceGD_HinhThuc.FirstOrDefault(e => e.KeywordStrings.First().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri())));
                    numGDTienMat.Value = 0;
                    numGDTienCKhoan.Value = 0;
                    txtGD_TKhoan_KH.Text = "";
                    lblGD_TKhoan_KH.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenTaiKhoanKH");
                    txtGD_TKhoan_NoiBo.Text = "";
                    lblGD_TKhoan_NoiBo.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenTaiKhoanNoiBo");
                    txtDienGiai.Text = "";

                    //Group thông tin khác
                    cmbMaCBQL.SelectedIndex = lstSourceCBQL.IndexOf(lstSourceCBQL.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TenDangNhap)));
                    #endregion

                    #region Tab chỉ thị đáo hạn
                    #endregion

                    #region Tab đồng chủ sở hữu
                    lblDCSH_ID.Content = "-1";
                    txtDCSH_MaKH.Text = "";
                    lblDCSH_TenKH.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenKhachHangDCSH");
                    txtDCSH_DiaChi.Text = "";
                    txtDCSH_CMND.Text = "";
                    txtDCSH_NoiCap.Text = "";
                    txtDCSH_SDT.Text = "";
                    raddtDCSH_NgayCap.Value = null;
                    dsDCSH.Tables[0].Rows.Clear();
                    DataViewManager dataViewManager = new DataViewManager(dsDCSH);
                    DataView dataView = dataViewManager.CreateDataView(dsDCSH.Tables[0]);
                    grDongSoHuuDS.DataContext = dataView;
                    #endregion

                    tbiSoDuTienLai.Visibility = System.Windows.Visibility.Collapsed;
                    tbiLichSuGD.Visibility = System.Windows.Visibility.Collapsed;
                    tbiLichSuLaiSuat.Visibility = System.Windows.Visibility.Collapsed;
                }

                #region Tab thông tin kiểm soát
                txtTrangThaiBanGhi.Text = "";
                raddtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                txtNguoiLap.Text = ClientInformation.TenDangNhap;
                raddtNgayCapNhat.Value = null;
                txtNguoiCapNhat.Text = "";
                #endregion

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
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

        private void PreviewXemSo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void PreviewXemSo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnPreviewSo();
        }

        private void PreviewXemCT_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void PreviewXemCT_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnPreviewCT();
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                BeforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                BeforeRefuse();
            }
            else if (strTinhNang.Equals("PreviewXemSo"))
            {
                OnPreviewSo();
            }
            else if (strTinhNang.Equals("PreviewXemCT"))
            {
                OnPreviewCT();
            }
            else if (strTinhNang.Equals("PreviewGiayGuiTien"))
            {
                OnPreviewGiayGuiTien();
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

        /// <summary>
        /// SetEnabledAllControls
        /// </summary>
        /// <param name="enable"></param>
        private void SetEnabledAllControls(bool enable)
        {
            try
            {
                #region Tab thông tin chung
                //Group Thông tin khách hàng
                txtMaKhachHang.IsEnabled = enable;
                btnKhachHang.IsEnabled = enable;

                //Group Thông tin sổ tiền gửi                               
                txtTenTaiKhoan.IsEnabled = enable;
                txtMaSanPham.IsEnabled = enable;
                btnSanPham.IsEnabled = enable;
                cmbLoaiTien.IsEnabled = false;
                if (function == DatabaseConstant.Function.HDV_SO_TKCKH || function == DatabaseConstant.Function.HDV_SO_TGCKH)
                {
                    txtSoSoTG.IsEnabled = enable;
                    numSoTien.IsEnabled = enable;
                    numKyHan.IsEnabled = enable;
                    cmbKyHan_DVi_Tinh.IsEnabled = enable;
                    raddtNgayDaoHan.IsEnabled = enable;
                    dtpNgayDHan.IsEnabled = enable;
                }
                else
                {
                    txtSoSoTG.IsEnabled = false;
                    numSoTien.IsEnabled = enable;
                    numKyHan.IsEnabled = false;
                    cmbKyHan_DVi_Tinh.IsEnabled = false;
                    raddtNgayDaoHan.IsEnabled = false;
                    dtpNgayDHan.IsEnabled = false;
                }
                numLaiSuat.IsEnabled = enable;
                raddtNgayMo.IsEnabled = enable;
                dtpNgayMo.IsEnabled = enable;
                cmbNguonVon.IsEnabled = enable;
                cmbNguonVonCT.IsEnabled = enable;


                //Group Thông tin giao dịch
                cmbGDHinhThuc.IsEnabled = enable;
                numGDTienMat.IsEnabled = enable;
                numGDTienCKhoan.IsEnabled = enable;
                txtGD_TKhoan_KH.IsEnabled = enable;
                btnGD_TKhoan_KH.IsEnabled = enable;
                txtGD_TKhoan_NoiBo.IsEnabled = enable;
                btnGD_TKhoan_NoiBo.IsEnabled = enable;
                txtDienGiai.IsEnabled = enable;
                cmbNguonVon.IsEnabled = enable;
                cmbNguonVonCT.IsEnabled = enable;


                #region Giao dịch
                cmbGDHinhThuc.IsEnabled = enable;
                if (enable == true)
                {
                    string sHinhThucGD = lstSourceGD_HinhThuc.ElementAt(cmbGDHinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (sHinhThucGD.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri()))
                    {
                        numGDTienMat.IsEnabled = false;
                        numGDTienCKhoan.IsEnabled = false;
                        txtGD_TKhoan_KH.IsEnabled = false;
                        btnGD_TKhoan_KH.IsEnabled = false;
                        txtGD_TKhoan_NoiBo.IsEnabled = false;
                        btnGD_TKhoan_NoiBo.IsEnabled = false;
                    }
                    else if (sHinhThucGD.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN.layGiaTri()))
                    {
                        numGDTienMat.IsEnabled = false;
                        numGDTienCKhoan.IsEnabled = false;
                        txtGD_TKhoan_KH.IsEnabled = true;
                        btnGD_TKhoan_KH.IsEnabled = true;
                        txtGD_TKhoan_NoiBo.IsEnabled = true;
                        btnGD_TKhoan_NoiBo.IsEnabled = true;
                    }
                    else if (sHinhThucGD.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TMAT_CKHOAN.layGiaTri()))
                    {
                        numGDTienMat.IsEnabled = false;
                        numGDTienCKhoan.IsEnabled = true;
                        txtGD_TKhoan_KH.IsEnabled = true;
                        btnGD_TKhoan_KH.IsEnabled = true;
                        txtGD_TKhoan_NoiBo.IsEnabled = true;
                        btnGD_TKhoan_NoiBo.IsEnabled = true;

                    }
                }
                else
                {
                    numGDTienMat.IsEnabled = enable;
                    numGDTienCKhoan.IsEnabled = enable;
                    txtGD_TKhoan_KH.IsEnabled = enable;
                    btnGD_TKhoan_KH.IsEnabled = enable;
                    txtGD_TKhoan_NoiBo.IsEnabled = enable;
                    btnGD_TKhoan_NoiBo.IsEnabled = enable;
                }
                #endregion

                //Group Thông tin khác
                cmbMaCBQL.IsEnabled = enable;
                txtSoKU.IsEnabled = enable;

                #endregion

                #region Tab chỉ thị đáo hạn
                cmbDHan_CThi.IsEnabled = enable;
                //numDHan_KyHan.IsEnabled = enable;
                //cmbDHan_KyHan_DViTinh.IsEnabled = enable;
                txtDHan_MaLaiSuat.IsEnabled = enable;
                btnDHan_MaLaiSuat.IsEnabled = enable;
                txtDHan_TKhoanTraGoc.IsEnabled = enable;
                btnDHan_TKhoanTraGoc.IsEnabled = enable;
                txtDHan_TKhoanTraLai.IsEnabled = enable;
                btnDHan_TKhoanTraLai.IsEnabled = enable;

                #endregion

                #region Tab đồng chủ sở hữu
                txtDCSH_MaKH.IsEnabled = enable;
                btnDCSH_MaKH.IsEnabled = enable;
                rbtnThemDongSoHuu.IsEnabled = enable;
                rbtnXoaDongSoHuu.IsEnabled = enable;
                #endregion

                #region Tab hạn mức thấu chi
                txtTChi_MaHanMuc.IsEnabled = enable;
                btnTChi_MaHanMuc.IsEnabled = enable;
                txtTChi_TenHanMuc.IsEnabled = enable;
                txtTChi_GiaTri.IsEnabled = enable;
                #endregion
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        /// <summary>
        /// SetEnabledRequiredControls
        /// </summary>
        /// <param name="enable"></param>
        private void SetEnabledRequiredControls(bool enable)
        {
            try
            {
                txtMaKhachHang.IsEnabled = enable;
                btnKhachHang.IsEnabled = enable;

                txtSoSoTG.IsEnabled = enable;
                if (function == DatabaseConstant.Function.HDV_SO_TKQD || function == DatabaseConstant.Function.HDV_SO_TKKKH || function == DatabaseConstant.Function.HDV_SO_TK_TGTT)
                {
                    txtSoSoTG.IsEnabled = false;
                }

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Sự kiện load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            #region Xác định Function của Form
            if (flag == 0) //Nếu gọi Form từ menu chương trình
            {
                formCase = ClientInformation.FormCase;
                if (formCase.Equals("01"))
                    function = DatabaseConstant.Function.HDV_SO_TKQD;
                else if (formCase.Equals("02"))
                    function = DatabaseConstant.Function.HDV_SO_TKKKH;
                else if (formCase.Equals("03"))
                    function = DatabaseConstant.Function.HDV_SO_TKCKH;
                else if (formCase.Equals("04"))
                    function = DatabaseConstant.Function.HDV_SO_TK_TGTT;
                else if (formCase.Equals("05"))
                    function = DatabaseConstant.Function.HDV_SO_TGCKH;
            }
            else if (flag == 2) //Nếu gọi From từ kiểm soát bên kế toán
            {
                Dispatcher.CurrentDispatcher.DelayInvoke("KhoiTaoThongTin", () =>
                {
                    KhoiTaoThongTin();
                }, TimeSpan.FromSeconds(0));
            }
            #endregion

            //Hiển thị Form khi thêm mới dữ liệu
            Dispatcher.CurrentDispatcher.DelayInvoke("ShowData", () =>
            {
                if (action == DatabaseConstant.Action.THEM)
                {
                    DateTime dt = DateTime.Now;
                    BeforeAddNew();
                    LLogging.WriteLog("Them", LLogging.LogType.BUS, DateTime.Now.Subtract(dt).TotalMilliseconds.ToString());
                }

            //Hiển thị Form khi sửa dữ liệu
                else if (action == DatabaseConstant.Action.SUA)
                {
                    DateTime dt = DateTime.Now;
                    BeforeModifyFromList();
                    LLogging.WriteLog("Sua", LLogging.LogType.BUS, DateTime.Now.Subtract(dt).TotalMilliseconds.ToString());
                }

                //Hiển thị Form khi xem dữ liệu
                else if (action == DatabaseConstant.Action.XEM)
                {
                    DateTime dt = DateTime.Now;
                    BeforeViewFromList();
                    LLogging.WriteLog("Xem", LLogging.LogType.BUS, DateTime.Now.Subtract(dt).TotalMilliseconds.ToString());
                }
            }, TimeSpan.FromSeconds(0));


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

        private void cbMultiAdd_Checked(object sender, RoutedEventArgs e)
        {
            action = DatabaseConstant.Action.THEM;
            flag = 0;
            id = -1;
            idKhachHang = -1;
            idSanPham = -1;
            maGiaoDich = "";
            sTrangThaiNVu = "";
            _objKiemSoat = null;

            BeforeAddNew();
            cbMultiAdd.IsChecked = false;
            txtMaKhachHang.Focus();
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
                function,
                DatabaseConstant.Table.BL_TIEN_GUI,
                DatabaseConstant.Action.SUA,
                listLockId);
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
            if (OnSavingCompleted != null)
            {
                OnSavingCompleted(this, EventArgs.Empty);
            }
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        private void HideControl()
        {
            try
            {
                HeThong hethong = new HeThong();
                ArrayList arr = new ArrayList();
                if (function == DatabaseConstant.Function.HDV_SO_TKQD)
                {
                    if (action == DatabaseConstant.Action.THEM)
                        arr = hethong.SetVisibleControl("PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT", "Loai1.1");
                    else if (action == DatabaseConstant.Action.SUA)
                        arr = hethong.SetVisibleControl("PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT", "Loai1.2");
                    else if (action == DatabaseConstant.Action.XEM)
                        arr = hethong.SetVisibleControl("PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT", "Loai1.3");

                }

                else if (function == DatabaseConstant.Function.HDV_SO_TKKKH)
                {
                    if (action == DatabaseConstant.Action.THEM)
                        arr = hethong.SetVisibleControl("PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT", "Loai2.1");
                    else if (action == DatabaseConstant.Action.SUA)
                        arr = hethong.SetVisibleControl("PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT", "Loai2.2");
                    else if (action == DatabaseConstant.Action.XEM)
                        arr = hethong.SetVisibleControl("PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT", "Loai2.3");

                }

                else if (function == DatabaseConstant.Function.HDV_SO_TKCKH)
                {
                    if (action == DatabaseConstant.Action.THEM)
                        arr = hethong.SetVisibleControl("PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT", "Loai3.1");
                    else if (action == DatabaseConstant.Action.SUA)
                        arr = hethong.SetVisibleControl("PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT", "Loai3.2");
                    else if (action == DatabaseConstant.Action.XEM)
                        arr = hethong.SetVisibleControl("PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT", "Loai3.3");

                }

                else if (function == DatabaseConstant.Function.HDV_SO_TK_TGTT)
                {
                    if (action == DatabaseConstant.Action.THEM)
                        arr = hethong.SetVisibleControl("PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT", "Loai4.1");
                    else if (action == DatabaseConstant.Action.SUA)
                        arr = hethong.SetVisibleControl("PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT", "Loai4.2");
                    else if (action == DatabaseConstant.Action.XEM)
                        arr = hethong.SetVisibleControl("PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT", "Loai4.3");

                }

                else if (function == DatabaseConstant.Function.HDV_SO_TGCKH)
                {
                    if (action == DatabaseConstant.Action.THEM)
                        arr = hethong.SetVisibleControl("PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT", "Loai5.1");
                    if (action == DatabaseConstant.Action.SUA)
                        arr = hethong.SetVisibleControl("PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT", "Loai5.2");
                    else if (action == DatabaseConstant.Action.XEM)
                        arr = hethong.SetVisibleControl("PresentationWPF.HuyDongVon.MoSo.ucTienGuiCoKyHanCT", "Loai5.3");

                }

                foreach (List<string> lst in arr)
                {
                    object item = gridMain.FindName(lst.First());
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
        /// Khởi tạo thông tin khi được gọi từ Form kiểm soát
        /// </summary>
        private void KhoiTaoThongTin()
        {
            try
            {
                action = _objKiemSoat.action;
                KeToanProcess processKT = new KeToanProcess();
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                DataSet ds = processKT.getGiaoDich(ClientInformation.MaDonViGiaoDich, _objKiemSoat.SO_GIAO_DICH);
                string soTienGui = ds.Tables[0].Rows[0]["MA_TCHIEU"].ToString();

                DataTable dt = null;

                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@SO_TGUI", "STRING", soTienGui);
                LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", ClientInformation.MaDonViGiaoDich);
                ds = processHDV.GetThongTinSoTGuiTheoMa(dt);

                ID = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]);
                int idSanPham = Convert.ToInt32(ds.Tables[0].Rows[0]["ID_SAN_PHAM"]);

                ds = processHDV.GetSanPhamByID(idSanPham);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    sNhomSP = ds.Tables[0].Rows[0]["MA_NHOM_SP"].ToString();
                    if (sNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T01.layGiaTri()))
                    {
                        Function = DatabaseConstant.Function.HDV_SO_TKQD;
                    }
                    else if (sNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T02.layGiaTri()))
                    {
                        Function = DatabaseConstant.Function.HDV_SO_TKKKH;
                    }
                    else if (sNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T03.layGiaTri()) || sNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T04.layGiaTri()) || sNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T05.layGiaTri()) || sNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T06.layGiaTri()))
                    {
                        Function = DatabaseConstant.Function.HDV_SO_TKCKH;
                    }
                    else if (sNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T07.layGiaTri()))
                    {
                        Function = DatabaseConstant.Function.HDV_SO_TGCKH;
                    }
                    else if (sNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T08.layGiaTri()))
                    {
                        Function = DatabaseConstant.Function.HDV_SO_TK_TGTT;
                    }

                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void btnKhachHang_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Window window = new Window();
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                ucPopupKhachHang uc = new ucPopupKhachHang();
                window.Title = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.DanhSachKhachHang");
                window.Content = uc;
                Mouse.OverrideCursor = Cursors.Arrow;
                window.ShowDialog();
                if (uc.lstData != null && uc.lstData.Count > 0)
                {
                    DataRowView drKhachHang = uc.lstData[0];
                    KhachHangProcess processKH = new KhachHangProcess();
                    int idKhachHang = Convert.ToInt32(drKhachHang["ID"]);
                    Mouse.OverrideCursor = Cursors.Wait;
                    DataRow dr = processKH.getThongTinCoBanKHTheoID(idKhachHang).Tables[0].Rows[0];

                    lblID_KH.Content = dr["ID"].ToString();
                    txtMaKhachHang.Text = dr["MA_KHANG"].ToString();
                    lblTenKH.Content = dr["TEN_KHANG"].ToString();
                    txtDiaChi.Text = dr["DIA_CHI"].ToString();
                    txtSDT.Text = dr["SO_DTHOAI"].ToString();
                    if (dr["DD_GTLQ_LOAI"].ToString().Equals("CHUNG_MINH_ND"))
                    {
                        txtCMND.Text = dr["DD_GTLQ_SO"].ToString();
                        if (LDateTime.IsDate(dr["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd"))
                            raddtNgayCap.Value = LDateTime.StringToDate(dr["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd");
                        else
                            raddtNgayCap.Value = null;
                        txtNoiCap.Text = dr["DD_GTLQ_NOI_CAP"].ToString();
                    }

                    //TinhSoTienGuiMoiKy();

                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void txtMaKhachHang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnKhachHang_Click(null, null);
            }
        }

        private void txtMaKhachHang_LostFocus(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    if (!txtMaKhachHang.Text.IsNullOrEmptyOrSpace())
            //    {
            //        KhachHangProcess processKH = new KhachHangProcess();
            //        DataSet ds = processKH.getThongTinCoBanKHTheoMa(0, txtMaKhachHang.Text, Convert.ToInt32(ClientInformation.IdDonVi));
            //        if (ds != null && ds.Tables[0].Rows.Count > 0)
            //        {
            //            DataRow dr = ds.Tables[0].Rows[0];
            //            lblID_KH.Content = dr["ID"].ToString();
            //            txtMaKhachHang.Text = dr["MA_KHANG"].ToString();
            //            lblTenKH.Content = dr["TEN_KHANG"].ToString();
            //            txtDiaChi.Text = dr["DIA_CHI"].ToString();
            //            txtSDT.Text = dr["SO_DTHOAI"].ToString();
            //            if (dr["DD_GTLQ_LOAI"].ToString().Equals("CHUNG_MINH_ND"))
            //            {
            //                txtCMND.Text = dr["DD_GTLQ_SO"].ToString();
            //                if (LDateTime.IsDate(dr["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd"))
            //                    raddtNgayCap.Value = LDateTime.StringToDate(dr["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd");
            //                else
            //                    raddtNgayCap.Value = null;
            //                txtNoiCap.Text = dr["DD_GTLQ_NOI_CAP"].ToString();
            //            }

            //            TinhSoTienGuiMoiKy();
            //        }
            //        else
            //        {
            //            txtMaKhachHang.Text = "";
            //            lblTenKH.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenKhachHang");
            //            txtDiaChi.Text = "";
            //            txtCMND.Text = "";
            //            raddtNgayCap.Value = null;
            //            txtNoiCap.Text = "";
            //            txtSDT.Text = "";
            //        }
            //    }
            //    else
            //    {
            //        txtMaKhachHang.Text = "";
            //        lblTenKH.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenKhachHang");
            //        txtDiaChi.Text = "";
            //        txtCMND.Text = "";
            //        raddtNgayCap.Value = null;
            //        txtNoiCap.Text = "";
            //        txtSDT.Text = "";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);

            //}
        }

        private void btnSoKU_Click(object sender, RoutedEventArgs e)
        {
            if (txtMaKhachHang.Text.IsNullOrEmptyOrSpace())
                return;

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                //list điều kiện theo từng loại sổ
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add(txtMaKhachHang.Text);                

                var process = new PopupProcess();
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_KHE_UOC_KH.getValue(), lstDieuKien);

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                Mouse.OverrideCursor = Cursors.Arrow;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null)
                {
                    DataRow row = lstPopup[0];

                    txtSoKU.Text = row["MA_KUOCVM"].ToString();
                    numSoDuGoc.Value = Convert.ToDouble(row["SO_TIEN_GIAI_NGAN"]);

                    TinhSoTienMoSo();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void txtSoKU_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnSoKU_Click(null, null);
            }
        }

        private void TinhSoTienMoSo()
        {
            try
            {
                bool ret = false;
                BL_TIEN_GUI objTienGui = new BL_TIEN_GUI();
                List<BL_TIEN_GUI_DCSH> lstDCSH = null;
                HDV_THONG_TIN_SO_TGUI objThongTinSoTG = null;
                List<ClientResponseDetail> lstResponseDetail = null;

                objTienGui.MA_KHANG = txtMaKhachHang.Text;
                objTienGui.MA_KUOC = txtMaSanPham.Text;
                objTienGui.SO_DU_GOC = (decimal)numSoDuGoc.Value;

                if (txtMaKhachHang.Text.IsNullOrEmptyOrSpace() || txtSoKU.Text.IsNullOrEmptyOrSpace()) return;

                HuyDongVonProcess processHDV = new HuyDongVonProcess();

                if (function == DatabaseConstant.Function.HDV_SO_TKQD)
                {
                    ret = processHDV.MoSoTietKiemQuyDinh(DatabaseConstant.Action.TINH_TOAN2, ref objTienGui, ref lstDCSH, ref objThongTinSoTG, ref lstResponseDetail);
                    if (ret)
                    {
                        numSoTien.Value = (double)objTienGui.SO_TIEN_MO_SO;

                        numSoTien_LostFocus(null, null);
                    }
                }
                else return;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtTChi_MaHanMuc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
            }
        }

        /// <summary>
        /// Trả về 0 nếu xử lý không thành công
        /// Trả về 1 nếu xử lý thành công
        /// 
        /// Hàm dựa vào sản phẩm huy động vốn, khách hàng để tìm ra số khế ước, số dư gốc, số tiền gửi mỗi kỳ của khách hàng tương ứng
        /// </summary>
        /// <param name="maSanPham"></param>
        /// <param name="maKhachHang"></param>
        /// <param name="soKheUoc"></param>
        /// <param name="soDuGoc"></param>
        /// <param name="soTienGuiMoiKy"></param>
        /// <returns></returns>
        public int XuLyKheUocTheoSanPhamHDV(string maSanPham, string maKhachHang, ref string soKheUoc, ref decimal soDuGoc, ref decimal soTienGuiMoiKy)
        {
            return 1;
        }

        #region Sản phẩm

        private void btnSanPham_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                //list điều kiện theo từng loại sổ
                List<string> lstDieuKien = new List<string>();
                if (function == DatabaseConstant.Function.HDV_SO_TKQD)
                {
                    lstDieuKien.Add("'" + BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T01) + "'");
                }

                else if (function == DatabaseConstant.Function.HDV_SO_TKKKH)
                {
                    lstDieuKien.Add("'" + BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T02) + "'");
                }

                else if (function == DatabaseConstant.Function.HDV_SO_TKCKH)
                {
                    lstDieuKien.Add("'" + BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T03) + "','" + BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T04) + "', '" + BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T05) + "','" + BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T06) + "'");
                }

                else if (function == DatabaseConstant.Function.HDV_SO_TK_TGTT)
                {
                    lstDieuKien.Add("'" + BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T08) + "'");
                }

                else if (function == DatabaseConstant.Function.HDV_SO_TGCKH)
                {
                    lstDieuKien.Add("'" + BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T07) + "'");
                }
                else
                {
                    return;
                }

                lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
                lstDieuKien.Add(ClientInformation.MaDonVi);

                var process = new PopupProcess();
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_SAN_PHAM_HDV.getValue(), lstDieuKien);

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                Mouse.OverrideCursor = Cursors.Arrow;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null)
                {
                    DataRow row = lstPopup[0];
                    HuyDongVonProcess processHuyDongVon = new HuyDongVonProcess();
                    DataRow drSanPham = processHuyDongVon.GetSanPhamByID(Convert.ToInt32(row[1])).Tables[0].Rows[0];

                    sNhomSP = drSanPham["MA_NHOM_SP"].ToString();
                    lblID_SanPham.Content = drSanPham["ID"].ToString();
                    txtMaSanPham.Text = drSanPham["MA_SAN_PHAM"].ToString();
                    lblTenSanPham.Content = drSanPham["TEN_SAN_PHAM"].ToString();
                    idLaiSuat = (int)drSanPham["ID_LSUAT"];

                    HienTabTheoSP(drSanPham);
                    TinhKyHan();
                    XuLyLaiSuatTheoSPham();
                    //TinhSoTienGuiMoiKy();

                    cmbDHan_CThi_SelectionChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void txtMaSanPham_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.F3)
                {
                    btnSanPham_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void txtMaSanPham_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                HuyDongVonProcess processHuyDongVon = new HuyDongVonProcess();
                DataSet ds = processHuyDongVon.GetSanPhamByMa(txtMaSanPham.Text);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow drSanPham = ds.Tables[0].Rows[0];
                    lblID_SanPham.Content = drSanPham["ID"].ToString();
                    txtMaSanPham.Text = drSanPham["MA_SAN_PHAM"].ToString();
                    lblTenSanPham.Content = drSanPham["TEN_SAN_PHAM"].ToString();

                    HienTabTheoSP(drSanPham);
                    TinhKyHan();
                    XuLyLaiSuatTheoSPham();
                    //TinhSoTienGuiMoiKy();
                }
                else
                {
                    txtMaSanPham.Text = "";
                    lblTenSanPham.Content = "Tên sản phẩm";
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Hiển thị Tab theo sản phẩm
        /// </summary>
        /// <param name="drSanPham"></param>
        private void HienTabTheoSP(DataRow drSanPham)
        {
            try
            {
                if (drSanPham["MA_NHOM_SP"].ToString().Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T03))
                        || (drSanPham["MA_NHOM_SP"].ToString().Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T07)) && drSanPham["TLAI_HTHUC"].ToString().Equals(BusinessConstant.layGiaTri(BusinessConstant.PHUONG_THUC_TRA_LAI.DKY_DAU))))
                {
                    tbiLapLichTraLai.Visibility = System.Windows.Visibility.Visible;
                }
                else if (drSanPham["MA_NHOM_SP"].ToString().Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T06)))
                {
                    tbiTraGop.Visibility = System.Windows.Visibility.Visible;
                }
                else if (drSanPham["MA_NHOM_SP"].ToString().Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T08)) && drSanPham["TT_THAU_CHI"].ToString().Equals("C"))
                {
                    tbiHanMucThauChi.Visibility = System.Windows.Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void TinhKyHan()
        {
            try
            {
                if (function == DatabaseConstant.Function.HDV_SO_TKCKH)
                {
                    bool ret = false;
                    BL_TIEN_GUI objTienGui = null;
                    List<BL_TIEN_GUI_DCSH> lstDCSH = null;
                    HDV_THONG_TIN_SO_TGUI objThongTinSoTG = null;
                    List<ClientResponseDetail> lstResponseDetail = null;
                    HuyDongVonProcess processHDV = new HuyDongVonProcess();

                    objTienGui = new BL_TIEN_GUI();
                    objTienGui.MA_SAN_PHAM = txtMaSanPham.Text;

                    if (txtMaSanPham.Text.IsNullOrEmptyOrSpace()) return;

                    ret = processHDV.MoSoTietKiemCoKyHan(DatabaseConstant.Action.TINH_TOAN_KY_HAN, ref objTienGui, ref lstDCSH, ref objThongTinSoTG, ref lstResponseDetail);
                    if (ret)
                    {
                        numKyHan.Value = (double)objTienGui.KY_HAN;
                        cmbKyHan_DVi_Tinh.SelectedIndex = lstSourceKyHan_DVi.IndexOf(lstSourceKyHan_DVi.FirstOrDefault(i => i.KeywordStrings.First().Equals(objTienGui.KY_HAN_DVI_TINH)));

                        if (raddtNgayDaoHan.IsEnabled)
                        {
                            string sDonViKyHan = lstSourceKyHan_DVi.ElementAt(cmbKyHan_DVi_Tinh.SelectedIndex).KeywordStrings.ElementAt(0);
                            raddtNgayDaoHan.Value = AddDatetime(Convert.ToDateTime(raddtNgayMo.Value), Convert.ToInt32(numKyHan.Value), sDonViKyHan);
                        }
                    }
                }
                else return;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void XuLyLaiSuatTheoSPham()
        {
            try
            {
                bool ret = false;
                BL_TIEN_GUI objTienGui = new BL_TIEN_GUI();
                List<BL_TIEN_GUI_DCSH> lstDCSH = null;
                List<BL_TIEN_GUI_KHOACH> lstBLTienGuiKHoach = null;
                HDV_THONG_TIN_SO_TGUI objThongTinSoTG = null;
                List<ClientResponseDetail> lstResponseDetail = null;
                objTienGui.MA_SAN_PHAM = txtMaSanPham.Text;
                objTienGui.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                if (numSoTien.Value != null)
                    objTienGui.SO_TIEN = (decimal)numSoTien.Value;
                else
                    objTienGui.SO_TIEN = 0;

                if (numKyHan.Value != null)
                    objTienGui.KY_HAN = (int)numKyHan.Value;
                else
                    objTienGui.KY_HAN = 0;
                if (cmbKyHan_DVi_Tinh.SelectedIndex > 0)
                    objTienGui.KY_HAN_DVI_TINH = lstSourceDHan_KyHan_DVi.ElementAt(cmbKyHan_DVi_Tinh.SelectedIndex).KeywordStrings.FirstOrDefault().ToString();

                HuyDongVonProcess processHDV = new HuyDongVonProcess();

                if (function == DatabaseConstant.Function.HDV_SO_TKQD)
                    ret = processHDV.MoSoTietKiemQuyDinh(DatabaseConstant.Action.TINH_TOAN_LAI_SUAT, ref objTienGui, ref lstDCSH, ref objThongTinSoTG, ref lstResponseDetail);

                else if (function == DatabaseConstant.Function.HDV_SO_TKKKH)
                    ret = processHDV.MoSoTietKiemKhongKyHan(DatabaseConstant.Action.TINH_TOAN_LAI_SUAT, ref objTienGui, ref lstDCSH, ref objThongTinSoTG, ref lstResponseDetail);

                else if (function == DatabaseConstant.Function.HDV_SO_TKCKH)
                    ret = processHDV.MoSoTietKiemCoKyHan(DatabaseConstant.Action.TINH_TOAN_LAI_SUAT, ref objTienGui, ref lstDCSH, ref objThongTinSoTG, ref lstResponseDetail);

                else if (function == DatabaseConstant.Function.HDV_SO_TK_TGTT)
                    ret = processHDV.MoTaiKhoanTienGuiThanhToan(DatabaseConstant.Action.TINH_TOAN_LAI_SUAT, ref objTienGui, ref lstDCSH, ref objThongTinSoTG, ref lstResponseDetail);

                else if (function == DatabaseConstant.Function.HDV_SO_TGCKH)
                    ret = processHDV.MoSoTienGuiCoKyHan(DatabaseConstant.Action.TINH_TOAN_LAI_SUAT, ref objTienGui, ref lstDCSH, ref lstBLTienGuiKHoach, ref objThongTinSoTG, ref lstResponseDetail);

                if (ret)
                {
                    numLaiSuat.Value = (double)objTienGui.LAI_SUAT;
                    cmbLaiSuat_DVi_Tinh.SelectedIndex = lstSourceLaiSuat_DVi.IndexOf(lstSourceLaiSuat_DVi.FirstOrDefault(i => i.KeywordStrings.First().Equals(objTienGui.LAI_SUAT_DVI_TINH)));
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void TinhSoTienGuiMoiKy()
        {
            try
            {
                bool ret = false;
                BL_TIEN_GUI objTienGui = new BL_TIEN_GUI();
                List<BL_TIEN_GUI_DCSH> lstDCSH = null;
                HDV_THONG_TIN_SO_TGUI objThongTinSoTG = null;
                List<ClientResponseDetail> lstResponseDetail = null;

                objTienGui.MA_KHANG = txtMaKhachHang.Text;
                objTienGui.MA_SAN_PHAM = txtMaSanPham.Text;

                if (txtMaKhachHang.Text.IsNullOrEmptyOrSpace() || txtMaSanPham.Text.IsNullOrEmptyOrSpace()) return;

                HuyDongVonProcess processHDV = new HuyDongVonProcess();

                if (function == DatabaseConstant.Function.HDV_SO_TKQD)
                {
                    ret = processHDV.MoSoTietKiemQuyDinh(DatabaseConstant.Action.TINH_TOAN_SO_TIEN_VAY, ref objTienGui, ref lstDCSH, ref objThongTinSoTG, ref lstResponseDetail);
                    if (ret)
                    {
                        txtSoKU.Text = objTienGui.MA_KUOC;
                        numSoDuGoc.Value = Convert.ToDouble(objTienGui.SO_DU_GOC);
                        txtSoTienGuiMoiKy.Value = Convert.ToDouble(objTienGui.SO_TIEN_KY);
                    }
                }
                else return;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        #endregion

        #region Giao dịch
        private void btnGD_TKhoan_KH_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                //list điều kiện theo từng loại sổ
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(txtMaKhachHang.Text);
                lstDieuKien.Add(ClientInformation.MaDonVi);

                var process = new PopupProcess();
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_TAI_KHOAN_KHACH_HANG.getValue(), lstDieuKien);

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
                    txtGD_TKhoan_KH.Text = row[3].ToString();
                    lblGD_TKhoan_KH.Content = row[4].ToString();

                    txtGD_TKhoan_NoiBo.Text = "";
                    lblGD_TKhoan_NoiBo.Content = "";
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

        private void btnGD_TKhoan_NoiBo_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                //list điều kiện theo từng loại sổ
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonVi);

                var process = new PopupProcess();
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_TAI_KHOAN_NOI_BO.getValue(), lstDieuKien);

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                Mouse.OverrideCursor = Cursors.Arrow;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow row = lstPopup[0];
                    txtGD_TKhoan_NoiBo.Text = row[2].ToString();
                    lblGD_TKhoan_NoiBo.Content = row[3].ToString();

                    txtGD_TKhoan_KH.Text = "";
                    lblGD_TKhoan_KH.Content = "";
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

        private void txtGD_TKhoan_KH_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnGD_TKhoan_KH_Click(null, null);
            }
        }

        private void txtGD_TKhoan_KH_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtGD_TKhoan_NoiBo_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtGD_TKhoan_NoiBo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnGD_TKhoan_NoiBo_Click(null, null);
            }
        }

        private void cmbGDHinhThuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbGDHinhThuc.SelectedIndex < 0) return;

                string sHinhThucGD = lstSourceGD_HinhThuc.ElementAt(cmbGDHinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                if (sHinhThucGD.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri()))
                {
                    numGDTienMat.IsEnabled = false;
                    numGDTienCKhoan.IsEnabled = false;
                    txtGD_TKhoan_KH.IsEnabled = false;
                    btnGD_TKhoan_KH.IsEnabled = false;
                    txtGD_TKhoan_NoiBo.IsEnabled = false;
                    btnGD_TKhoan_NoiBo.IsEnabled = false;

                    numGDTienMat.Value = numSoTien.Value;
                    numGDTienCKhoan.Value = 0;
                }
                else if (sHinhThucGD.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN.layGiaTri()))
                {
                    numGDTienMat.IsEnabled = false;
                    numGDTienCKhoan.IsEnabled = false;
                    txtGD_TKhoan_KH.IsEnabled = true;
                    btnGD_TKhoan_KH.IsEnabled = true;
                    txtGD_TKhoan_NoiBo.IsEnabled = true;
                    btnGD_TKhoan_NoiBo.IsEnabled = true;

                    numGDTienMat.Value = 0;
                    numGDTienCKhoan.Value = numSoTien.Value;
                }
                else if (sHinhThucGD.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TMAT_CKHOAN.layGiaTri()))
                {
                    numGDTienMat.IsEnabled = false;
                    numGDTienCKhoan.IsEnabled = true;
                    txtGD_TKhoan_KH.IsEnabled = true;
                    btnGD_TKhoan_KH.IsEnabled = true;
                    txtGD_TKhoan_NoiBo.IsEnabled = true;
                    btnGD_TKhoan_NoiBo.IsEnabled = true;

                    numGDTienCKhoan.Value = 0;
                    numGDTienMat.Value = numSoTien.Value - numGDTienCKhoan.Value;
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void numGDTienMat_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                string sHinhThucGD = lstSourceGD_HinhThuc.ElementAt(cmbGDHinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                if (sHinhThucGD.Equals(BusinessConstant.layGiaTri(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN)))
                {
                    numGDTienCKhoan.Value = numGDTienMat.Value;
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void numSoTien_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtMaSanPham.Text.IsNullOrEmptyOrSpace())
                {
                    XuLyLaiSuatTheoSPham();
                }

                string sHinhThucGD = lstSourceGD_HinhThuc.ElementAt(cmbGDHinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                if (sHinhThucGD.Equals(BusinessConstant.layGiaTri(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN)))
                {
                    numGDTienCKhoan.Value = numSoTien.Value;
                    numGDTienMat.Value = 0;
                }
                else
                {
                    numGDTienMat.Value = numSoTien.Value;
                    numGDTienCKhoan.Value = 0;
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void numGDTienCKhoan_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                string sHinhThucGD = lstSourceGD_HinhThuc.ElementAt(cmbGDHinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                if (sHinhThucGD.Equals(BusinessConstant.layGiaTri(BusinessConstant.HINH_THUC_GIAO_DICH.TMAT_CKHOAN)))
                {
                    if (numGDTienCKhoan.Value > numSoTien.Value)
                    {
                        string soTienCK = LLanguage.SearchResourceByKey("U.DungChung.SoTienCK");
                        string soTienTong = LLanguage.SearchResourceByKey("U.DungChung.SoTienTong");
                        LMessage.ShowMessage(soTienCK + " (" + numGDTienCKhoan.Text + ") > " + soTienTong + " (" + numSoTien.Text + ")", LMessage.MessageBoxType.Warning);
                        numGDTienMat.Value = numSoTien.Value;
                        numGDTienCKhoan.Value = 0;
                        numGDTienCKhoan.Focus();
                    }
                    else
                    {
                        numGDTienMat.Value = numSoTien.Value - numGDTienCKhoan.Value;
                    }

                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        #endregion

        #region Tính ngày đáo hạn theo kỳ hạn, ngày mở sổ
        private void numKyHan_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                XuLyLaiSuatTheoSPham();

                if (raddtNgayDaoHan.IsEnabled)
                {
                    string sDonViKyHan = lstSourceKyHan_DVi.ElementAt(cmbKyHan_DVi_Tinh.SelectedIndex).KeywordStrings.ElementAt(0);
                    raddtNgayDaoHan.Value = AddDatetime(Convert.ToDateTime(raddtNgayMo.Value), Convert.ToInt32(numKyHan.Value), sDonViKyHan);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void cmbKyHan_DVi_Tinh_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                XuLyLaiSuatTheoSPham();

                if (raddtNgayDaoHan.IsEnabled)
                {
                    string sDonViKyHan = lstSourceKyHan_DVi.ElementAt(cmbKyHan_DVi_Tinh.SelectedIndex).KeywordStrings.ElementAt(0);
                    raddtNgayDaoHan.Value = AddDatetime(Convert.ToDateTime(raddtNgayMo.Value), Convert.ToInt32(numKyHan.Value), sDonViKyHan);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void raddtNgayMo_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (raddtNgayDaoHan.IsEnabled)
                {
                    string sDonViKyHan = lstSourceKyHan_DVi.ElementAt(cmbKyHan_DVi_Tinh.SelectedIndex).KeywordStrings.ElementAt(0);
                    raddtNgayDaoHan.Value = AddDatetime(Convert.ToDateTime(raddtNgayMo.Value), Convert.ToInt32(numKyHan.Value), sDonViKyHan);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void dtpNgayMo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            raddtNgayMo_LostFocus(null, null);
        }

        private DateTime AddDatetime(DateTime dt, int count, string sDonViTinh)
        {
            DateTime dt1 = new DateTime();
            try
            {

                if (sDonViTinh == BusinessConstant.layGiaTri(BusinessConstant.TAN_SUAT.NAM))
                {
                    dt1 = dt.AddYears(count);
                }
                else if (sDonViTinh == BusinessConstant.layGiaTri(BusinessConstant.TAN_SUAT.QUY))
                {
                    dt1 = dt.AddMonths(count * 3);
                }
                else if (sDonViTinh == BusinessConstant.layGiaTri(BusinessConstant.TAN_SUAT.THANG))
                {
                    dt1 = dt.AddMonths(count);
                }
                else if (sDonViTinh == BusinessConstant.layGiaTri(BusinessConstant.TAN_SUAT.TUAN))
                {
                    dt1 = dt.AddDays(count * 7);
                }
                else if (sDonViTinh == BusinessConstant.layGiaTri(BusinessConstant.TAN_SUAT.NGAY))
                {
                    dt1 = dt.AddDays(count);
                }

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            return dt1;
        }
        #endregion

        #region Tab Đồng chủ sở hữu
        private void btnDCSH_MaKH_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Window window = new Window();
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                ucPopupKhachHang uc = new ucPopupKhachHang();
                window.Title = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.DanhSachKhachHang");
                window.Content = uc;
                Mouse.OverrideCursor = Cursors.Arrow;
                window.ShowDialog();
                if (uc.lstData != null && uc.lstData.Count > 0)
                {
                    DataRowView drKhachHang = uc.lstData[0];
                    KhachHangProcess processKH = new KhachHangProcess();
                    int idKhachHang = Convert.ToInt32(drKhachHang["ID"]);
                    Mouse.OverrideCursor = Cursors.Wait;
                    DataRow dr = processKH.getThongTinCoBanKHTheoID(idKhachHang).Tables[0].Rows[0];

                    lblDCSH_ID.Content = dr["ID"].ToString();
                    txtDCSH_MaKH.Text = dr["MA_KHANG"].ToString();
                    lblDCSH_TenKH.Content = dr["TEN_KHANG"].ToString();
                    txtDCSH_DiaChi.Text = dr["DIA_CHI"].ToString();
                    txtDCSH_SDT.Text = dr["SO_DTHOAI"].ToString();
                    if (dr["DD_GTLQ_LOAI"].ToString().Equals("CHUNG_MINH_ND"))
                    {
                        txtDCSH_CMND.Text = dr["DD_GTLQ_SO"].ToString();
                        if (LDateTime.IsDate(dr["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd"))
                            raddtDCSH_NgayCap.Value = LDateTime.StringToDate(dr["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd");
                        else
                            raddtDCSH_NgayCap.Text = "";
                        txtNoiCap.Text = dr["DD_GTLQ_NOI_CAP"].ToString();
                        txtDCSH_NoiCap.Text = dr["DD_GTLQ_NOI_CAP"].ToString();
                    }
                    Mouse.OverrideCursor = Cursors.Arrow;
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

        private void txtDCSH_MaKH_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnDCSH_MaKH_Click(null, null);
            }
        }

        private void txtDCSH_MaKH_LostFocus(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                KhachHangProcess processKH = new KhachHangProcess();
                DataSet ds = processKH.getThongTinCoBanKHTheoMa(0, txtMaKhachHang.Text, Convert.ToInt32(ClientInformation.IdDonVi));
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    lblDCSH_ID.Content = dr["ID"].ToString();
                    txtDCSH_MaKH.Text = dr["MA_KHANG"].ToString();
                    lblDCSH_TenKH.Content = dr["TEN_KHANG"].ToString();
                    txtDCSH_DiaChi.Text = dr["DIA_CHI"].ToString();
                    txtDCSH_SDT.Text = dr["SO_DTHOAI"].ToString();
                    if (dr["DD_GTLQ_LOAI"].ToString().Equals("CHUNG_MINH_ND"))
                    {
                        txtDCSH_CMND.Text = dr["DD_GTLQ_SO"].ToString();
                        if (LDateTime.IsDate(dr["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd"))
                            raddtDCSH_NgayCap.Value = LDateTime.StringToDate(dr["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd");
                        else
                            raddtDCSH_NgayCap.Text = "";
                        txtDCSH_NoiCap.Text = dr["DD_GTLQ_NOI_CAP"].ToString();
                    }
                }
                else
                {
                    txtDCSH_MaKH.Text = "";
                    lblDCSH_TenKH.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenKhachHangDCSH");
                    txtDCSH_DiaChi.Text = "";
                    txtDCSH_CMND.Text = "";
                    raddtDCSH_NgayCap.Value = null;
                    txtDCSH_NoiCap.Text = "";
                    txtDCSH_SDT.Text = "";
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

        private void rbtnThemDongSoHuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region Validation
                if (txtDCSH_MaKH.Text.IsNullOrEmptyOrSpace())
                {
                    LMessage.ShowMessage("M.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.ChuaChonKhachHang", LMessage.MessageBoxType.Warning);
                    txtDCSH_MaKH.Focus();
                    return;
                }

                if (txtDCSH_MaKH.Text.Equals(txtMaKhachHang.Text))
                {
                    string[] s = new string[1] { txtDCSH_MaKH.Text + " - " + lblDCSH_TenKH.Content.ToString() };
                    LMessage.ShowMessage("M.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.KhachHangDaLaChuSoHuu", s, LMessage.MessageBoxType.Warning);
                    //LMessage.ShowMessage("Khách hàng " + txtDCSH_MaKH.Text + " - " + lblDCSH_TenKH.Content.ToString() + " đã là chủ sở hữu của sổ", LMessage.MessageBoxType.Warning);
                    txtDCSH_MaKH.Focus();
                    return;
                }

                for (int i = 0; i < dsDCSH.Tables[0].Rows.Count; i++)
                {
                    if (dsDCSH.Tables[0].Rows[i]["MA_KHANG"].ToString().Equals(txtDCSH_MaKH.Text))
                    {
                        string[] s = new string[1] { txtDCSH_MaKH.Text + " - " + lblDCSH_TenKH.Content.ToString() };
                        LMessage.ShowMessage("M.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.KhachHangDaDuocChon", s, LMessage.MessageBoxType.Warning);
                        //LMessage.ShowMessage("Khách hàng " + txtDCSH_MaKH.Text + " - " + lblDCSH_TenKH.Content.ToString() + " đã được chọn", LMessage.MessageBoxType.Warning);
                        txtDCSH_MaKH.Focus();
                        return;
                    }
                }
                #endregion

                dsDCSH.Tables[0].Rows.Add((dsDCSH.Tables[0].Rows.Count + 1), Convert.ToInt32(lblDCSH_ID.Content), txtDCSH_MaKH.Text, lblDCSH_TenKH.Content, txtDCSH_DiaChi.Text);
                DataViewManager dataViewManager = new DataViewManager(dsDCSH);
                DataView dataView = dataViewManager.CreateDataView(dsDCSH.Tables[0]);
                grDongSoHuuDS.DataContext = dataView;

                txtDCSH_MaKH.Text = "";
                lblDCSH_TenKH.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenKhachHangDCSH");
                txtDCSH_DiaChi.Text = "";
                txtDCSH_CMND.Text = "";
                txtDCSH_NoiCap.Text = "";
                txtDCSH_SDT.Text = "";
                raddtDCSH_NgayCap.Value = null;

                txtDCSH_MaKH.Focus();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void rbtnXoaDongSoHuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<int> lstSTT = new List<int>();
                for (int i = 0; i < grDongSoHuuDS.SelectedItems.Count; i++)
                {
                    DataRowView dr = (DataRowView)grDongSoHuuDS.SelectedItems[i];
                    lstSTT.Add(Convert.ToInt32(dr["STT"]));
                }
                lstSTT.SortByDesc();
                foreach (int stt in lstSTT)
                    dsDCSH.Tables[0].Rows.RemoveAt(stt - 1);

                for (int i = 0; i < dsDCSH.Tables[0].Rows.Count; i++)
                {
                    dsDCSH.Tables[0].Rows[i]["STT"] = i + 1;
                }

                DataViewManager dataViewManager = new DataViewManager(dsDCSH);
                DataView dataView = dataViewManager.CreateDataView(dsDCSH.Tables[0]);
                grDongSoHuuDS.DataContext = dataView;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
        #endregion

        #region Tab Chỉ thị đáo hạn
        private void cmbDHan_CThi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string sChiThiDHan = lstSourceDHan_ChiThi.ElementAt(cmbDHan_CThi.SelectedIndex).KeywordStrings.ElementAt(0);

                if (sChiThiDHan.Equals(BusinessConstant.layGiaTri(BusinessConstant.CHI_THI_DAO_HAN.LNG_LSQV_KHC)))
                {
                    numDHan_KyHan.IsEnabled = false;
                    cmbDHan_KyHan_DViTinh.IsEnabled = false;
                    txtDHan_MaLaiSuat.IsEnabled = false;
                    btnDHan_MaLaiSuat.IsEnabled = false;
                    txtDHan_TKhoanTraGoc.IsEnabled = false;
                    btnDHan_TKhoanTraGoc.IsEnabled = false;
                    txtDHan_TKhoanTraLai.IsEnabled = false;
                    btnDHan_TKhoanTraLai.IsEnabled = false;
                }

                else if (sChiThiDHan.Equals(BusinessConstant.layGiaTri(BusinessConstant.CHI_THI_DAO_HAN.SPQV_LSM_KHM)))
                {
                    numDHan_KyHan.IsEnabled = true;
                    cmbDHan_KyHan_DViTinh.IsEnabled = true;
                    txtDHan_MaLaiSuat.IsEnabled = true;
                    btnDHan_MaLaiSuat.IsEnabled = true;
                    txtDHan_TKhoanTraGoc.IsEnabled = false;
                    btnDHan_TKhoanTraGoc.IsEnabled = false;
                    txtDHan_TKhoanTraLai.IsEnabled = false;
                    btnDHan_TKhoanTraLai.IsEnabled = false;
                }

                else if (sChiThiDHan.Equals(BusinessConstant.layGiaTri(BusinessConstant.CHI_THI_DAO_HAN.GQV_LSC_TKCT)))
                {
                    numDHan_KyHan.IsEnabled = false;
                    cmbDHan_KyHan_DViTinh.IsEnabled = false;
                    txtDHan_MaLaiSuat.IsEnabled = false;
                    btnDHan_MaLaiSuat.IsEnabled = false;
                    txtDHan_TKhoanTraGoc.IsEnabled = false;
                    btnDHan_TKhoanTraGoc.IsEnabled = false;
                    txtDHan_TKhoanTraLai.IsEnabled = true;
                    btnDHan_TKhoanTraLai.IsEnabled = true;
                }

                else if (sChiThiDHan.Equals(BusinessConstant.layGiaTri(BusinessConstant.CHI_THI_DAO_HAN.GQV_LSM_TKCT)))
                {
                    numDHan_KyHan.IsEnabled = true;
                    cmbDHan_KyHan_DViTinh.IsEnabled = true;
                    txtDHan_MaLaiSuat.IsEnabled = true;
                    btnDHan_MaLaiSuat.IsEnabled = true;
                    txtDHan_TKhoanTraGoc.IsEnabled = false;
                    btnDHan_TKhoanTraGoc.IsEnabled = false;
                    txtDHan_TKhoanTraLai.IsEnabled = true;
                    btnDHan_TKhoanTraLai.IsEnabled = true;
                }

                else if (sChiThiDHan.Equals(BusinessConstant.layGiaTri(BusinessConstant.CHI_THI_DAO_HAN.GOC_LAI_TKCT)))
                {
                    numDHan_KyHan.IsEnabled = false;
                    cmbDHan_KyHan_DViTinh.IsEnabled = false;
                    txtDHan_MaLaiSuat.IsEnabled = false;
                    btnDHan_MaLaiSuat.IsEnabled = false;
                    txtDHan_TKhoanTraGoc.IsEnabled = true;
                    btnDHan_TKhoanTraGoc.IsEnabled = true;
                    txtDHan_TKhoanTraLai.IsEnabled = true;
                    btnDHan_TKhoanTraLai.IsEnabled = true;

                    if(ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BIDV_BLF.layGiaTri()))
                    {
                        //Lãi suất
                        txtDHan_MaLaiSuat.Text = "";
                        lblDHan_MaLaiSuat.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenLaiSuatMoi");

                        //TK chỉ thị trả gốc
                        //TK chỉ thị trả lãi
                    }
                }

                else if (sChiThiDHan.Equals(BusinessConstant.layGiaTri(BusinessConstant.CHI_THI_DAO_HAN.GQV_KHC_LSM_TKCT)))
                {
                    numDHan_KyHan.IsEnabled = false;
                    cmbDHan_KyHan_DViTinh.IsEnabled = false;
                    txtDHan_MaLaiSuat.IsEnabled = false;
                    btnDHan_MaLaiSuat.IsEnabled = false;
                    txtDHan_TKhoanTraGoc.IsEnabled = false;
                    btnDHan_TKhoanTraGoc.IsEnabled = false;
                    txtDHan_TKhoanTraLai.IsEnabled = true;
                    btnDHan_TKhoanTraLai.IsEnabled = true;

                    if (ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BIDV_BLF.layGiaTri()))
                    {
                        //Lãi suất
                        if (idLaiSuat > 0)
                        {
                            DC_LSUAT objLS = new LaiSuatProcess().LayThongTinLaiSuat(idLaiSuat);
                            txtDHan_MaLaiSuat.Text = objLS.MA_LSUAT;
                            lblDHan_MaLaiSuat.Content = objLS.MO_TA;
                        }
                        else
                        {
                            txtDHan_MaLaiSuat.Text = "";
                            lblDHan_MaLaiSuat.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenLaiSuatMoi");
                        }

                        //TK chỉ thị trả gốc
                        txtDHan_TKhoanTraGoc.Text = "";
                        lblDHan_TKhoanTraGoc.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenTaiKhoanChiThiTraGoc");
                        //TK chỉ thị trả lãi
                    }
                }

                else if (sChiThiDHan.Equals(BusinessConstant.layGiaTri(BusinessConstant.CHI_THI_DAO_HAN.LNG_KHC_LSM)))
                {
                    numDHan_KyHan.IsEnabled = false;
                    cmbDHan_KyHan_DViTinh.IsEnabled = false;
                    txtDHan_MaLaiSuat.IsEnabled = false;
                    btnDHan_MaLaiSuat.IsEnabled = false;
                    txtDHan_TKhoanTraGoc.IsEnabled = false;
                    btnDHan_TKhoanTraGoc.IsEnabled = false;
                    txtDHan_TKhoanTraLai.IsEnabled = false;
                    btnDHan_TKhoanTraLai.IsEnabled = false;

                    if (ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BIDV_BLF.layGiaTri()))
                    {
                        //Lãi suất
                        if (idLaiSuat > 0)
                        {
                            DC_LSUAT objLS = new LaiSuatProcess().LayThongTinLaiSuat(idLaiSuat);
                            txtDHan_MaLaiSuat.Text = objLS.MA_LSUAT;
                            lblDHan_MaLaiSuat.Content = objLS.MO_TA;
                        }
                        else
                        {
                            txtDHan_MaLaiSuat.Text = "";
                            lblDHan_MaLaiSuat.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenLaiSuatMoi");
                        }

                        //TK chỉ thị trả gốc
                        txtDHan_TKhoanTraGoc.Text = "";
                        lblDHan_TKhoanTraGoc.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenTaiKhoanChiThiTraGoc");

                        //TK chỉ thị trả lãi
                        txtDHan_TKhoanTraLai.Text = "";
                        lblDHan_TKhoanTraLai.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenTaiKhoanChiThiTraLai");
                    }
                }

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void btnDHan_MaLaiSuat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonVi);

                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DM_LSUAT.getValue(), lstDieuKien);

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null)
                {
                    DataRow row = lstPopup[0];
                    if (!string.IsNullOrWhiteSpace(row[2].ToString()))
                        txtDHan_MaLaiSuat.Text = row[2].ToString();
                    if (!string.IsNullOrWhiteSpace(row[3].ToString()))
                        lblDHan_MaLaiSuat.Content = row[3].ToString();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void btnDHan_TKhoanTraGoc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(txtMaKhachHang.Text);
                lstDieuKien.Add(ClientInformation.MaDonVi);

                var process = new PopupProcess();
                process.getPopupInformation("POPUP_DS_TKHOAN_CA", lstDieuKien);

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
                    if (!string.IsNullOrWhiteSpace(row[2].ToString()))
                        txtDHan_TKhoanTraGoc.Text = row[2].ToString();
                    if (!string.IsNullOrWhiteSpace(row[3].ToString()))
                        lblDHan_TKhoanTraGoc.Content = row[3].ToString();

                    if (cmbDHan_CThi.SelectedIndex >= 0
                        && lstSourceDHan_ChiThi.ElementAt(cmbDHan_CThi.SelectedIndex).KeywordStrings.ElementAt(0).Equals(BusinessConstant.CHI_THI_DAO_HAN.GOC_LAI_TKCT.layGiaTri()))
                    {
                        txtDHan_TKhoanTraLai.Text = txtDHan_TKhoanTraGoc.Text;
                        lblDHan_TKhoanTraLai.Content = lblDHan_TKhoanTraGoc.Content;
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void btnDHan_TKhoanTraLai_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(txtMaKhachHang.Text);
                lstDieuKien.Add(ClientInformation.MaDonVi);

                var process = new PopupProcess();
                process.getPopupInformation("POPUP_DS_TKHOAN_CA", lstDieuKien);

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
                    if (!string.IsNullOrWhiteSpace(row[2].ToString()))
                        txtDHan_TKhoanTraLai.Text = row[2].ToString();
                    if (!string.IsNullOrWhiteSpace(row[3].ToString()))
                        lblDHan_TKhoanTraLai.Content = row[3].ToString();

                    if (cmbDHan_CThi.SelectedIndex >= 0
                       && lstSourceDHan_ChiThi.ElementAt(cmbDHan_CThi.SelectedIndex).KeywordStrings.ElementAt(0).Equals(BusinessConstant.CHI_THI_DAO_HAN.GOC_LAI_TKCT.layGiaTri()))
                    {
                        txtDHan_TKhoanTraGoc.Text = txtDHan_TKhoanTraLai.Text;
                        lblDHan_TKhoanTraGoc.Content = lblDHan_TKhoanTraLai.Content;
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void txtDHan_MaLaiSuat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnDHan_MaLaiSuat_Click(null, null);
            }
        }

        private void txtDHan_TKhoanTraGoc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnDHan_TKhoanTraGoc_Click(null, null);
            }
        }

        private void txtDHan_TKhoanTraLai_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnDHan_TKhoanTraLai_Click(null, null);
            }
        }
        #endregion

        #endregion

        #region Xử lý nghiệp vụ

        private void GetFormData(ref BL_TIEN_GUI objBLTienGui,
                                 ref List<BL_TIEN_GUI_DCSH> lstBLTienGuiDCSH,
                                 ref List<BL_TIEN_GUI_KHOACH> lstBLTienGuiKHoach,
                                 string sTrangThaiNVu)
        {
            try
            {
                UtilitiesProcess processUtilities = new UtilitiesProcess();

                objBLTienGui.ID = ID;

                // Lấy thông tin trên Form theo sổ tiết kiệm quy định
                if (function == DatabaseConstant.Function.HDV_SO_TKQD)
                {
                    #region Tab thông tin chung
                    objBLTienGui.ID_KHANG = Convert.ToInt32(lblID_KH.Content);
                    objBLTienGui.ID_SAN_PHAM = Convert.ToInt32(lblID_SanPham.Content);
                    objBLTienGui.MA_KHANG = txtMaKhachHang.Text;
                    objBLTienGui.MA_GDICH = txtSoGiaoDich.Text;
                    objBLTienGui.SO_SO_TG = txtSoSoTG.Text;
                    objBLTienGui.MA_NHOM_SP = sNhomSP;
                    objBLTienGui.MA_SAN_PHAM = txtMaSanPham.Text;
                    objBLTienGui.MA_LOAI_TIEN = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.ElementAt(0);
                    objBLTienGui.NGAY_MO_SO = Convert.ToDateTime(raddtNgayMo.Value).ToString("yyyyMMdd");
                    objBLTienGui.NGAY_DEN_HAN = "";
                    //objBLTienGui.KY_HAN_DVI_TINH = lstSourceKyHan_DVi.ElementAt(cmbKyHan_DVi_Tinh.SelectedIndex).KeywordStrings.ElementAt(0);
                    objBLTienGui.KY_HAN = 0;
                    objBLTienGui.SO_TIEN_MO_SO = Convert.ToDecimal(numSoTien.Value);
                    objBLTienGui.SO_TIEN = objBLTienGui.SO_TIEN_MO_SO;
                    objBLTienGui.LAI_SUAT = Convert.ToDecimal(numLaiSuat.Value);
                    objBLTienGui.LAI_SUAT_DVI_TINH = lstSourceLaiSuat_DVi.ElementAt(cmbLaiSuat_DVi_Tinh.SelectedIndex).KeywordStrings.ElementAt(0);
                    objBLTienGui.NV_LOAI_NVON = lstSourceNguonVon.ElementAt(cmbNguonVon.SelectedIndex).KeywordStrings.ElementAt(0);

                    if(!ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BENTRE.layGiaTri()))
                        objBLTienGui.NV_LOAI_NVON_CT = lstSourceNguonVonCT.ElementAt(cmbNguonVonCT.SelectedIndex).KeywordStrings.ElementAt(0);

                    objBLTienGui.DIEN_GIAI = txtDienGiai.Text;
                    if (cmbMaCBQL.SelectedIndex >= 0)
                        objBLTienGui.MA_CBQL = lstSourceCBQL.ElementAt(cmbMaCBQL.SelectedIndex).KeywordStrings.ElementAt(0);
                    objBLTienGui.MA_KUOC = txtSoKU.Text;
                    objBLTienGui.SO_DU_GOC = Convert.ToDecimal(numSoDuGoc.Value);
                    objBLTienGui.SO_TIEN_KY = Convert.ToDecimal(txtSoTienGuiMoiKy.Value);
                    objBLTienGui.NGAY_ADUNG = Convert.ToDateTime(raddtNgayADKU.Value).ToString("yyyyMMdd");

                    objBLTienGui.GDICH_HTHUC = lstSourceGD_HinhThuc.ElementAt(cmbGDHinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                    objBLTienGui.GDICH_TIEN_MAT = Convert.ToDecimal(numGDTienMat.Value);
                    objBLTienGui.GDICH_TIEN_CKHOAN = Convert.ToDecimal(numGDTienCKhoan.Value);
                    objBLTienGui.GDICH_TKHOAN_KHANG = txtGD_TKhoan_KH.Text;
                    objBLTienGui.GDICH_TKHOAN_NBO = txtGD_TKhoan_NoiBo.Text;
                    objBLTienGui.NGAY_GDICH = ngayGiaoDich;
                    #endregion

                    #region Tab đồng chủ sở hữu
                    if (dsDCSH != null && dsDCSH.Tables[0].Rows.Count > 0)
                    {
                        lstBLTienGuiDCSH = new List<BL_TIEN_GUI_DCSH>();
                        foreach (DataRow dr in dsDCSH.Tables[0].Rows)
                        {
                            BL_TIEN_GUI_DCSH objCT = new BL_TIEN_GUI_DCSH();
                            objCT.ID_KHANG = Convert.ToInt32(dr["ID"]);
                            objCT.SO_SO_TG = txtSoSoTG.Text;
                            objCT.MA_KHANG = dr["MA_KHANG"].ToString();
                            objCT.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                            objCT.TTHAI_NVU = sTrangThaiNVu;
                            objCT.MA_DVI_QLY = ClientInformation.MaDonVi;
                            objCT.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                            objCT.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                            objCT.NGUOI_NHAP = ClientInformation.TenDangNhap;
                            if (action != DatabaseConstant.Action.THEM)
                            {
                                objCT.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                                objCT.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                            }

                            lstBLTienGuiDCSH.Add(objCT);
                        }
                    }
                    #endregion

                    #region Thông tin khác
                    objBLTienGui.NGAY_TINH_LAI = objBLTienGui.NGAY_MO_SO;
                    objBLTienGui.NGAY_TINH_LAI_DEN = objBLTienGui.NGAY_MO_SO;
                    objBLTienGui.NGAY_KY_MOI = objBLTienGui.NGAY_MO_SO;
                    objBLTienGui.DU_CHI_LAI = 0;
                    objBLTienGui.DU_CHI_DEN_NGAY = objBLTienGui.NGAY_KY_MOI;
                    objBLTienGui.LAI_CHUA_TRA = 0;
                    #endregion

                    #region Thông tin kiểm soát
                    objBLTienGui.LOAI_SODU_TLAI = processUtilities.LayGiaTriThamSo(BusinessConstant.LoaiThamSo.DV, BusinessConstant.MaThamSo.DV_SO_DU_TINH_LAI_KHI_RUT_GOC, ClientInformation.MaDonVi);
                    objBLTienGui.TTHAI_SOTG = BusinessConstant.TRANG_THAI_TAT_TOAN.CHUA_TAT_TOAN.layGiaTri();
                    objBLTienGui.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    objBLTienGui.TTHAI_NVU = sTrangThaiNVu;
                    objBLTienGui.MA_DVI_QLY = ClientInformation.MaDonVi;
                    objBLTienGui.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    objBLTienGui.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    objBLTienGui.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    if (action != DatabaseConstant.Action.THEM)
                    {
                        objBLTienGui.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                        objBLTienGui.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    }
                    #endregion
                }

                // Lấy thông tin trên Form của sổ tiết kiệm không kỳ hạn
                else if (function == DatabaseConstant.Function.HDV_SO_TKKKH)
                {
                    #region Tab thông tin chung
                    objBLTienGui.ID_KHANG = Convert.ToInt32(lblID_KH.Content);
                    objBLTienGui.ID_SAN_PHAM = Convert.ToInt32(lblID_SanPham.Content);
                    objBLTienGui.MA_KHANG = txtMaKhachHang.Text;
                    objBLTienGui.MA_GDICH = txtSoGiaoDich.Text;
                    objBLTienGui.SO_SO_TG = txtSoSoTG.Text;
                    objBLTienGui.MA_NHOM_SP = sNhomSP;
                    objBLTienGui.MA_SAN_PHAM = txtMaSanPham.Text;
                    objBLTienGui.MA_LOAI_TIEN = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.ElementAt(0);
                    objBLTienGui.NGAY_MO_SO = Convert.ToDateTime(raddtNgayMo.Value).ToString("yyyyMMdd");
                    objBLTienGui.NGAY_DEN_HAN = "";
                    objBLTienGui.NGAY_TINH_LAI = "";
                    //objBLTienGui.KY_HAN_DVI_TINH = "";
                    objBLTienGui.KY_HAN = 0;
                    objBLTienGui.SO_TIEN_MO_SO = Convert.ToDecimal(numSoTien.Value);
                    objBLTienGui.SO_TIEN = objBLTienGui.SO_TIEN_MO_SO;
                    objBLTienGui.LAI_SUAT = Convert.ToDecimal(numLaiSuat.Value);
                    objBLTienGui.LAI_SUAT_DVI_TINH = lstSourceLaiSuat_DVi.ElementAt(cmbLaiSuat_DVi_Tinh.SelectedIndex).KeywordStrings.ElementAt(0);
                    objBLTienGui.NV_LOAI_NVON = lstSourceNguonVon.ElementAt(cmbNguonVon.SelectedIndex).KeywordStrings.ElementAt(0);

                    if (!ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BENTRE.layGiaTri()))
                        objBLTienGui.NV_LOAI_NVON_CT = lstSourceNguonVonCT.ElementAt(cmbNguonVonCT.SelectedIndex).KeywordStrings.ElementAt(0);

                    objBLTienGui.DIEN_GIAI = txtDienGiai.Text;
                    if (cmbMaCBQL.SelectedIndex >= 0)
                        objBLTienGui.MA_CBQL = lstSourceCBQL.ElementAt(cmbMaCBQL.SelectedIndex).KeywordStrings.ElementAt(0);

                    objBLTienGui.GDICH_HTHUC = lstSourceGD_HinhThuc.ElementAt(cmbGDHinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                    objBLTienGui.GDICH_TIEN_MAT = Convert.ToDecimal(numGDTienMat.Value);
                    objBLTienGui.GDICH_TIEN_CKHOAN = Convert.ToDecimal(numGDTienCKhoan.Value);
                    objBLTienGui.GDICH_TKHOAN_KHANG = txtGD_TKhoan_KH.Text;
                    objBLTienGui.GDICH_TKHOAN_NBO = txtGD_TKhoan_NoiBo.Text;
                    objBLTienGui.NGAY_GDICH = ngayGiaoDich;
                    #endregion

                    #region Tab đồng chủ sở hữu
                    if (dsDCSH != null && dsDCSH.Tables[0].Rows.Count > 0)
                    {
                        lstBLTienGuiDCSH = new List<BL_TIEN_GUI_DCSH>();
                        foreach (DataRow dr in dsDCSH.Tables[0].Rows)
                        {
                            BL_TIEN_GUI_DCSH objCT = new BL_TIEN_GUI_DCSH();
                            objCT.ID_KHANG = Convert.ToInt32(dr["ID"]);
                            objCT.SO_SO_TG = txtSoSoTG.Text;
                            objCT.MA_KHANG = dr["MA_KHANG"].ToString();
                            objCT.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                            objCT.TTHAI_NVU = sTrangThaiNVu;
                            objCT.MA_DVI_QLY = ClientInformation.MaDonVi;
                            objCT.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                            objCT.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                            objCT.NGUOI_NHAP = ClientInformation.TenDangNhap;
                            if (action != DatabaseConstant.Action.THEM)
                            {
                                objCT.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                                objCT.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                            }

                            lstBLTienGuiDCSH.Add(objCT);
                        }
                    }
                    #endregion

                    #region Thông tin khác
                    objBLTienGui.NGAY_TINH_LAI = objBLTienGui.NGAY_MO_SO;
                    objBLTienGui.NGAY_TINH_LAI_DEN = objBLTienGui.NGAY_MO_SO;
                    objBLTienGui.NGAY_KY_MOI = objBLTienGui.NGAY_MO_SO;
                    objBLTienGui.DU_CHI_DEN_NGAY = objBLTienGui.NGAY_KY_MOI;
                    objBLTienGui.LAI_CHUA_TRA = 0;
                    #endregion

                    #region Thông tin kiểm soát
                    objBLTienGui.LOAI_SODU_TLAI = processUtilities.LayGiaTriThamSo(BusinessConstant.LoaiThamSo.DV, BusinessConstant.MaThamSo.DV_SO_DU_TINH_LAI_KHI_RUT_GOC, ClientInformation.MaDonVi);
                    objBLTienGui.TTHAI_SOTG = BusinessConstant.TRANG_THAI_TAT_TOAN.CHUA_TAT_TOAN.layGiaTri();
                    objBLTienGui.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    objBLTienGui.TTHAI_NVU = sTrangThaiNVu;
                    objBLTienGui.MA_DVI_QLY = ClientInformation.MaDonVi;
                    objBLTienGui.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    objBLTienGui.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    objBLTienGui.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    if (action != DatabaseConstant.Action.THEM)
                    {
                        objBLTienGui.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                        objBLTienGui.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    }
                    #endregion
                }

                // Lấy thông tin trên Form của sổ tiết kiệm có kỳ hạn
                else if (function == DatabaseConstant.Function.HDV_SO_TKCKH)
                {
                    #region Tab thông tin chung
                    objBLTienGui.ID_KHANG = Convert.ToInt32(lblID_KH.Content);
                    objBLTienGui.ID_SAN_PHAM = Convert.ToInt32(lblID_SanPham.Content);
                    objBLTienGui.MA_KHANG = txtMaKhachHang.Text;
                    objBLTienGui.MA_GDICH = txtSoGiaoDich.Text;
                    objBLTienGui.SO_SO_TG = txtSoSoTG.Text;
                    objBLTienGui.MA_NHOM_SP = sNhomSP;
                    objBLTienGui.MA_SAN_PHAM = txtMaSanPham.Text;
                    objBLTienGui.MA_LOAI_TIEN = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.ElementAt(0);
                    objBLTienGui.NGAY_MO_SO = Convert.ToDateTime(raddtNgayMo.Value).ToString("yyyyMMdd");
                    objBLTienGui.NGAY_DEN_HAN = Convert.ToDateTime(raddtNgayDaoHan.Value).ToString("yyyyMMdd");
                    objBLTienGui.NGAY_TINH_LAI = Convert.ToDateTime(raddtNgayMo.Value).ToString("yyyyMMdd");
                    objBLTienGui.KY_HAN_DVI_TINH = lstSourceKyHan_DVi.ElementAt(cmbKyHan_DVi_Tinh.SelectedIndex).KeywordStrings.ElementAt(0);
                    objBLTienGui.KY_HAN = Convert.ToInt32(numKyHan.Value);
                    objBLTienGui.SO_TIEN_MO_SO = Convert.ToDecimal(numSoTien.Value);
                    objBLTienGui.SO_TIEN = objBLTienGui.SO_TIEN_MO_SO;
                    objBLTienGui.LAI_SUAT = Convert.ToDecimal(numLaiSuat.Value);
                    objBLTienGui.LAI_SUAT_DVI_TINH = lstSourceLaiSuat_DVi.ElementAt(cmbLaiSuat_DVi_Tinh.SelectedIndex).KeywordStrings.ElementAt(0);
                    objBLTienGui.NV_LOAI_NVON = lstSourceNguonVon.ElementAt(cmbNguonVon.SelectedIndex).KeywordStrings.ElementAt(0);

                    if (!ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BENTRE.layGiaTri()))
                        objBLTienGui.NV_LOAI_NVON_CT = lstSourceNguonVonCT.ElementAt(cmbNguonVonCT.SelectedIndex).KeywordStrings.ElementAt(0);

                    objBLTienGui.DIEN_GIAI = txtDienGiai.Text;
                    if (cmbMaCBQL.SelectedIndex >= 0)
                        objBLTienGui.MA_CBQL = lstSourceCBQL.ElementAt(cmbMaCBQL.SelectedIndex).KeywordStrings.ElementAt(0);
                    //objBLTienGui.MA_KUOC

                    objBLTienGui.GDICH_HTHUC = lstSourceGD_HinhThuc.ElementAt(cmbGDHinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                    objBLTienGui.GDICH_TIEN_MAT = Convert.ToDecimal(numGDTienMat.Value);
                    objBLTienGui.GDICH_TIEN_CKHOAN = Convert.ToDecimal(numGDTienCKhoan.Value);
                    objBLTienGui.GDICH_TKHOAN_KHANG = txtGD_TKhoan_KH.Text;
                    objBLTienGui.GDICH_TKHOAN_NBO = txtGD_TKhoan_NoiBo.Text;
                    objBLTienGui.NGAY_GDICH = ngayGiaoDich;
                    #endregion

                    #region Tab chỉ thị đáo hạn
                    objBLTienGui.DHAN_CTHI = lstSourceDHan_ChiThi.ElementAt(cmbDHan_CThi.SelectedIndex).KeywordStrings.ElementAt(0);
                    objBLTienGui.DHAN_KY_HAN = Convert.ToInt32(numDHan_KyHan.Value);
                    objBLTienGui.DHAN_KY_HAN_DVI_TINH = lstSourceDHan_KyHan_DVi.ElementAt(cmbDHan_KyHan_DViTinh.SelectedIndex).KeywordStrings.ElementAt(0);
                    objBLTienGui.DHAN_MA_LSUAT = txtDHan_MaLaiSuat.Text;
                    //objBLTienGui.DHAN_MUC_LSUAT Không dùng
                    objBLTienGui.DHAN_TKHOAN_TRGOC = txtDHan_TKhoanTraGoc.Text;
                    objBLTienGui.DHAN_TKHOAN_TRLAI = txtDHan_TKhoanTraLai.Text;
                    #endregion

                    #region Tab đồng chủ sở hữu
                    if (dsDCSH != null && dsDCSH.Tables[0].Rows.Count > 0)
                    {
                        lstBLTienGuiDCSH = new List<BL_TIEN_GUI_DCSH>();
                        foreach (DataRow dr in dsDCSH.Tables[0].Rows)
                        {
                            BL_TIEN_GUI_DCSH objCT = new BL_TIEN_GUI_DCSH();
                            objCT.ID_KHANG = Convert.ToInt32(dr["ID"]);
                            objCT.SO_SO_TG = txtSoSoTG.Text;
                            objCT.MA_KHANG = dr["MA_KHANG"].ToString();
                            objCT.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                            objCT.TTHAI_NVU = sTrangThaiNVu;
                            objCT.MA_DVI_QLY = ClientInformation.MaDonVi;
                            objCT.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                            objCT.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                            objCT.NGUOI_NHAP = ClientInformation.TenDangNhap;
                            if (action != DatabaseConstant.Action.THEM)
                            {
                                objCT.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                                objCT.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                            }

                            lstBLTienGuiDCSH.Add(objCT);
                        }
                    }
                    #endregion

                    #region Thông tin khác
                    objBLTienGui.NGAY_TINH_LAI = objBLTienGui.NGAY_MO_SO;
                    objBLTienGui.NGAY_TINH_LAI_DEN = objBLTienGui.NGAY_MO_SO;
                    objBLTienGui.NGAY_KY_MOI = objBLTienGui.NGAY_MO_SO;
                    objBLTienGui.DU_CHI_LAI = 0;
                    objBLTienGui.DU_CHI_DEN_NGAY = objBLTienGui.NGAY_KY_MOI;
                    objBLTienGui.LAI_CHUA_TRA = 0;
                    #endregion

                    #region Thông tin kiểm soát
                    objBLTienGui.LOAI_SODU_TLAI = processUtilities.LayGiaTriThamSo(BusinessConstant.LoaiThamSo.DV, BusinessConstant.MaThamSo.DV_SO_DU_TINH_LAI_KHI_RUT_GOC, ClientInformation.MaDonVi);
                    objBLTienGui.TTHAI_SOTG = BusinessConstant.TRANG_THAI_TAT_TOAN.CHUA_TAT_TOAN.layGiaTri();
                    objBLTienGui.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    objBLTienGui.TTHAI_NVU = sTrangThaiNVu;
                    objBLTienGui.MA_DVI_QLY = ClientInformation.MaDonVi;
                    objBLTienGui.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    objBLTienGui.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    objBLTienGui.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    if (action != DatabaseConstant.Action.THEM)
                    {
                        objBLTienGui.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                        objBLTienGui.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    }
                    #endregion
                }

                // Lấy thông tin trên Form của tài khoản tiền gửi thanh toán
                else if (function == DatabaseConstant.Function.HDV_SO_TK_TGTT)
                {
                    #region Tab thông tin chung
                    objBLTienGui.ID_KHANG = Convert.ToInt32(lblID_KH.Content);
                    objBLTienGui.ID_SAN_PHAM = Convert.ToInt32(lblID_SanPham.Content);
                    objBLTienGui.MA_KHANG = txtMaKhachHang.Text;
                    objBLTienGui.MA_GDICH = txtSoGiaoDich.Text;
                    objBLTienGui.SO_SO_TG = txtSoSoTG.Text;
                    objBLTienGui.MA_NHOM_SP = sNhomSP;
                    objBLTienGui.MA_SAN_PHAM = txtMaSanPham.Text;
                    objBLTienGui.MA_LOAI_TIEN = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.ElementAt(0);
                    objBLTienGui.NGAY_MO_SO = Convert.ToDateTime(raddtNgayMo.Value).ToString("yyyyMMdd");
                    objBLTienGui.NGAY_DEN_HAN = "";
                    objBLTienGui.NGAY_TINH_LAI = "";
                    objBLTienGui.KY_HAN_DVI_TINH = "";
                    objBLTienGui.KY_HAN = 0;
                    objBLTienGui.SO_TIEN_MO_SO = Convert.ToDecimal(numSoTien.Value);
                    objBLTienGui.SO_TIEN = objBLTienGui.SO_TIEN_MO_SO;
                    objBLTienGui.LAI_SUAT = Convert.ToDecimal(numLaiSuat.Value);
                    objBLTienGui.LAI_SUAT_DVI_TINH = lstSourceLaiSuat_DVi.ElementAt(cmbLaiSuat_DVi_Tinh.SelectedIndex).KeywordStrings.ElementAt(0);
                    objBLTienGui.NV_LOAI_NVON = lstSourceNguonVon.ElementAt(cmbNguonVon.SelectedIndex).KeywordStrings.ElementAt(0);

                    if (!ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BENTRE.layGiaTri()))
                        objBLTienGui.NV_LOAI_NVON_CT = lstSourceNguonVonCT.ElementAt(cmbNguonVonCT.SelectedIndex).KeywordStrings.ElementAt(0);

                    objBLTienGui.DIEN_GIAI = txtDienGiai.Text;
                    if (cmbMaCBQL.SelectedIndex >= 0)
                        objBLTienGui.MA_CBQL = lstSourceCBQL.ElementAt(cmbMaCBQL.SelectedIndex).KeywordStrings.ElementAt(0);

                    objBLTienGui.GDICH_HTHUC = lstSourceGD_HinhThuc.ElementAt(cmbGDHinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                    objBLTienGui.GDICH_TIEN_MAT = Convert.ToDecimal(numGDTienMat.Value);
                    objBLTienGui.GDICH_TIEN_CKHOAN = Convert.ToDecimal(numGDTienCKhoan.Value);
                    objBLTienGui.GDICH_TKHOAN_KHANG = txtGD_TKhoan_KH.Text;
                    objBLTienGui.GDICH_TKHOAN_NBO = txtGD_TKhoan_NoiBo.Text;
                    objBLTienGui.NGAY_GDICH = ngayGiaoDich;
                    #endregion

                    #region Tab đồng chủ sở hữu
                    if (dsDCSH != null && dsDCSH.Tables[0].Rows.Count > 0)
                    {
                        lstBLTienGuiDCSH = new List<BL_TIEN_GUI_DCSH>();
                        foreach (DataRow dr in dsDCSH.Tables[0].Rows)
                        {
                            BL_TIEN_GUI_DCSH objCT = new BL_TIEN_GUI_DCSH();
                            objCT.ID_KHANG = Convert.ToInt32(dr["ID"]);
                            objCT.SO_SO_TG = txtSoSoTG.Text;
                            objCT.MA_KHANG = dr["MA_KHANG"].ToString();
                            objCT.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                            objCT.TTHAI_NVU = sTrangThaiNVu;
                            objCT.MA_DVI_QLY = ClientInformation.MaDonVi;
                            objCT.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                            objCT.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                            objCT.NGUOI_NHAP = ClientInformation.TenDangNhap;
                            if (action != DatabaseConstant.Action.THEM)
                            {
                                objCT.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                                objCT.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                            }

                            lstBLTienGuiDCSH.Add(objCT);
                        }
                    }
                    #endregion

                    #region Tab hạn mức thấu chi
                    #endregion

                    #region Lấy thông tin BL_TIEN_GUI_TKHOAN
                    #endregion

                    #region Thông tin khác
                    objBLTienGui.NGAY_TINH_LAI = objBLTienGui.NGAY_MO_SO;
                    objBLTienGui.NGAY_TINH_LAI_DEN = objBLTienGui.NGAY_MO_SO;
                    objBLTienGui.NGAY_KY_MOI = objBLTienGui.NGAY_MO_SO;
                    objBLTienGui.DU_CHI_LAI = 0;
                    objBLTienGui.DU_CHI_DEN_NGAY = objBLTienGui.NGAY_KY_MOI;
                    objBLTienGui.LAI_CHUA_TRA = 0;
                    #endregion

                    #region Thông tin kiểm soát
                    objBLTienGui.LOAI_SODU_TLAI = processUtilities.LayGiaTriThamSo(BusinessConstant.LoaiThamSo.DV, BusinessConstant.MaThamSo.DV_SO_DU_TINH_LAI_KHI_RUT_GOC, ClientInformation.MaDonVi);
                    objBLTienGui.TTHAI_SOTG = BusinessConstant.TRANG_THAI_TAT_TOAN.CHUA_TAT_TOAN.layGiaTri();
                    objBLTienGui.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    objBLTienGui.TTHAI_NVU = sTrangThaiNVu;
                    objBLTienGui.MA_DVI_QLY = ClientInformation.MaDonVi;
                    objBLTienGui.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    objBLTienGui.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    objBLTienGui.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    if (action != DatabaseConstant.Action.THEM)
                    {
                        objBLTienGui.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                        objBLTienGui.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    }
                    #endregion
                }

                // Lấy thông tin trên Form của sổ tiền gửi có kỳ hạn
                else if (function == DatabaseConstant.Function.HDV_SO_TGCKH)
                {
                    #region Tab thông tin chung
                    objBLTienGui.ID_KHANG = Convert.ToInt32(lblID_KH.Content);
                    objBLTienGui.ID_SAN_PHAM = Convert.ToInt32(lblID_SanPham.Content);
                    objBLTienGui.MA_KHANG = txtMaKhachHang.Text;
                    objBLTienGui.MA_GDICH = txtSoGiaoDich.Text;
                    objBLTienGui.SO_SO_TG = txtSoSoTG.Text;
                    objBLTienGui.MA_NHOM_SP = sNhomSP;
                    objBLTienGui.MA_SAN_PHAM = txtMaSanPham.Text;
                    objBLTienGui.MA_LOAI_TIEN = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.ElementAt(0);
                    objBLTienGui.NGAY_MO_SO = Convert.ToDateTime(raddtNgayMo.Value).ToString("yyyyMMdd");
                    objBLTienGui.NGAY_DEN_HAN = Convert.ToDateTime(raddtNgayDaoHan.Value).ToString("yyyyMMdd");
                    objBLTienGui.NGAY_TINH_LAI = Convert.ToDateTime(raddtNgayMo.Value).ToString("yyyyMMdd");
                    objBLTienGui.KY_HAN_DVI_TINH = lstSourceKyHan_DVi.ElementAt(cmbKyHan_DVi_Tinh.SelectedIndex).KeywordStrings.ElementAt(0);
                    objBLTienGui.KY_HAN = Convert.ToInt32(numKyHan.Value);
                    objBLTienGui.SO_TIEN_MO_SO = Convert.ToDecimal(numSoTien.Value);
                    objBLTienGui.SO_TIEN = objBLTienGui.SO_TIEN_MO_SO;
                    objBLTienGui.LAI_SUAT = Convert.ToDecimal(numLaiSuat.Value);
                    objBLTienGui.LAI_SUAT_DVI_TINH = lstSourceLaiSuat_DVi.ElementAt(cmbLaiSuat_DVi_Tinh.SelectedIndex).KeywordStrings.ElementAt(0);
                    objBLTienGui.NV_LOAI_NVON = lstSourceNguonVon.ElementAt(cmbNguonVon.SelectedIndex).KeywordStrings.ElementAt(0);

                    if (!ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BENTRE.layGiaTri()))
                        objBLTienGui.NV_LOAI_NVON_CT = lstSourceNguonVonCT.ElementAt(cmbNguonVonCT.SelectedIndex).KeywordStrings.ElementAt(0);

                    objBLTienGui.DIEN_GIAI = txtDienGiai.Text;
                    if (cmbMaCBQL.SelectedIndex >= 0)
                        objBLTienGui.MA_CBQL = lstSourceCBQL.ElementAt(cmbMaCBQL.SelectedIndex).KeywordStrings.ElementAt(0);
                    //objBLTienGui.MA_KUOC

                    objBLTienGui.GDICH_HTHUC = lstSourceGD_HinhThuc.ElementAt(cmbGDHinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                    objBLTienGui.GDICH_TIEN_MAT = Convert.ToDecimal(numGDTienMat.Value);
                    objBLTienGui.GDICH_TIEN_CKHOAN = Convert.ToDecimal(numGDTienCKhoan.Value);
                    objBLTienGui.GDICH_TKHOAN_KHANG = txtGD_TKhoan_KH.Text;
                    objBLTienGui.GDICH_TKHOAN_NBO = txtGD_TKhoan_NoiBo.Text;
                    objBLTienGui.NGAY_GDICH = ngayGiaoDich;
                    #endregion

                    #region Tab chỉ thị đáo hạn
                    if (cmbDHan_CThi.SelectedIndex > 0)
                    {
                        objBLTienGui.DHAN_CTHI = lstSourceDHan_ChiThi.ElementAt(cmbDHan_CThi.SelectedIndex).KeywordStrings.ElementAt(0);
                        objBLTienGui.DHAN_KY_HAN = Convert.ToInt32(numDHan_KyHan.Value);
                        objBLTienGui.DHAN_KY_HAN_DVI_TINH = lstSourceDHan_KyHan_DVi.ElementAt(cmbDHan_KyHan_DViTinh.SelectedIndex).KeywordStrings.ElementAt(0);
                        objBLTienGui.DHAN_MA_LSUAT = txtDHan_MaLaiSuat.Text;
                        //objBLTienGui.DHAN_MUC_LSUAT Không dùng
                        objBLTienGui.DHAN_TKHOAN_TRGOC = txtDHan_TKhoanTraGoc.Text;
                        objBLTienGui.DHAN_TKHOAN_TRLAI = txtDHan_TKhoanTraLai.Text;
                    }
                    #endregion

                    #region Tab đồng chủ sở hữu
                    if (dsDCSH != null && dsDCSH.Tables[0].Rows.Count > 0)
                    {
                        lstBLTienGuiDCSH = new List<BL_TIEN_GUI_DCSH>();
                        foreach (DataRow dr in dsDCSH.Tables[0].Rows)
                        {
                            BL_TIEN_GUI_DCSH objCT = new BL_TIEN_GUI_DCSH();
                            objCT.ID_KHANG = Convert.ToInt32(dr["ID"]);
                            objCT.SO_SO_TG = txtSoSoTG.Text;
                            objCT.MA_KHANG = dr["MA_KHANG"].ToString();
                            objCT.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                            objCT.TTHAI_NVU = sTrangThaiNVu;
                            objCT.MA_DVI_QLY = ClientInformation.MaDonVi;
                            objCT.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                            objCT.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                            objCT.NGUOI_NHAP = ClientInformation.TenDangNhap;
                            if (action != DatabaseConstant.Action.THEM)
                            {
                                objCT.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                                objCT.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                            }

                            lstBLTienGuiDCSH.Add(objCT);
                        }
                    }
                    #endregion

                    #region Thông tin khác
                    objBLTienGui.NGAY_TINH_LAI = objBLTienGui.NGAY_MO_SO;
                    objBLTienGui.NGAY_TINH_LAI_DEN = objBLTienGui.NGAY_MO_SO;
                    objBLTienGui.NGAY_KY_MOI = objBLTienGui.NGAY_MO_SO;
                    objBLTienGui.DU_CHI_LAI = 0;
                    objBLTienGui.DU_CHI_DEN_NGAY = objBLTienGui.NGAY_KY_MOI;
                    objBLTienGui.LAI_CHUA_TRA = 0;
                    #endregion

                    #region Thông tin kiểm soát
                    objBLTienGui.LOAI_SODU_TLAI = processUtilities.LayGiaTriThamSo(BusinessConstant.LoaiThamSo.DV, BusinessConstant.MaThamSo.DV_SO_DU_TINH_LAI_KHI_RUT_GOC, ClientInformation.MaDonVi);
                    objBLTienGui.TTHAI_SOTG = BusinessConstant.TRANG_THAI_TAT_TOAN.CHUA_TAT_TOAN.layGiaTri();
                    objBLTienGui.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    objBLTienGui.TTHAI_NVU = sTrangThaiNVu;
                    objBLTienGui.MA_DVI_QLY = ClientInformation.MaDonVi;
                    objBLTienGui.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    objBLTienGui.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    objBLTienGui.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    if (action != DatabaseConstant.Action.THEM)
                    {
                        objBLTienGui.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                        objBLTienGui.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    }
                    #endregion
                }
                else
                {
                    return;
                }
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
                bool ret = false;
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                DateTime dt = DateTime.Now;
                DataSet ds = processHDV.GetThongTinSoTGui(ID);
                LLogging.WriteLog("GetThongTinSoTGui()", LLogging.LogType.BUS, DateTime.Now.Subtract(dt).TotalMilliseconds.ToString());
                if (ds != null)
                {
                    DataTable dtThongTinChung = ds.Tables["TTIN_CTIET"];
                    DataRow drThongTinChung = dtThongTinChung.Rows[0];
                    DataTable dtDCSH = ds.Tables["DONG_CSH"];
                    DataTable dtKHoach = ds.Tables["KHOACH_TGOP"];
                    DataTable dtLSuat = ds.Tables["LSU_LSUAT"];
                    DataTable dtGiaoDich = ds.Tables["LSU_GDICH"];

                    // Hiện thị thông tin lên Form theo sổ tiết kiệm quy định
                    if (function == DatabaseConstant.Function.HDV_SO_TKQD)
                    {
                        MaGiaoDich = drThongTinChung["MA_GDICH"].ToString();
                        sTrangThaiNVu = drThongTinChung["TTHAI_NVU"].ToString();
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        if (drThongTinChung["NGAY_GDICH"] != DBNull.Value)
                            ngayGiaoDich = drThongTinChung["NGAY_GDICH"].ToString();
                        #region Tab thông tin chung
                            //Group Thông tin khách hàng
                        lblID_KH.Content = drThongTinChung["ID_KHANG"].ToString();
                        txtMaKhachHang.Text = drThongTinChung["MA_KHANG"].ToString();
                        lblTenKH.Content = drThongTinChung["TEN_KHANG"].ToString();
                        txtDiaChi.Text = drThongTinChung["DIA_CHI"].ToString();
                        if (drThongTinChung["DD_GTLQ_LOAI"].ToString().Equals("CHUNG_MINH_ND"))
                        {
                            txtCMND.Text = drThongTinChung["DD_GTLQ_SO"].ToString();
                            txtNoiCap.Text = drThongTinChung["DD_GTLQ_NOI_CAP"].ToString();
                            if (LDateTime.IsDate(drThongTinChung["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd") == true)
                                raddtNgayCap.Value = LDateTime.StringToDate(drThongTinChung["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd");
                        }
                        txtSDT.Text = drThongTinChung["SO_DTHOAI"].ToString();

                        //Group Thông tin sổ tiền gửi
                        txtSoGiaoDich.Text = drThongTinChung["MA_GDICH"].ToString();
                        txtSoSoTG.Text = drThongTinChung["SO_SO_TG"].ToString();
                        sNhomSP = drThongTinChung["MA_NHOM_SP"].ToString();
                        lblID_SanPham.Content = drThongTinChung["ID_SAN_PHAM"].ToString();
                        txtMaSanPham.Text = drThongTinChung["MA_SAN_PHAM"].ToString();
                        lblTenSanPham.Content = drThongTinChung["TEN_SAN_PHAM"].ToString();
                        cmbLoaiTien.SelectedIndex = lstSourceLoaiTien.IndexOf(lstSourceLoaiTien.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["MA_LOAI_TIEN"].ToString())));
                        numSoTien.Value = Convert.ToDouble(drThongTinChung["SO_TIEN_MO_SO"]);
                        numKyHan.Value = null;
                        cmbKyHan_DVi_Tinh.Text = "";
                        numLaiSuat.Value = Convert.ToDouble(drThongTinChung["LAI_SUAT"]);
                        cmbLaiSuat_DVi_Tinh.SelectedIndex = lstSourceLaiSuat_DVi.IndexOf(lstSourceLaiSuat_DVi.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["LAI_SUAT_DVI_TINH"].ToString())));
                        if (LDateTime.IsDate(drThongTinChung["NGAY_MO_SO"].ToString(), "yyyyMMdd") == true)
                        {
                            raddtNgayMo.Value = LDateTime.StringToDate(drThongTinChung["NGAY_MO_SO"].ToString(), "yyyyMMdd");
                            dtpNgayMo.SelectedDate = raddtNgayMo.Value;
                        }
                        else
                        {
                            raddtNgayMo.Value = null;
                        }
                        raddtNgayDaoHan.Value = null;
                        cmbNguonVon.SelectedIndex = lstSourceNguonVon.IndexOf(lstSourceNguonVon.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["NV_LOAI_NVON"].ToString())));

                        if (!ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BENTRE.layGiaTri()))
                            cmbNguonVonCT.SelectedIndex = lstSourceNguonVonCT.IndexOf(lstSourceNguonVonCT.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["NV_LOAI_NVON_CT"].ToString())));

                        //Group Thông tin giao dịch
                        cmbGDHinhThuc.SelectedIndex = lstSourceGD_HinhThuc.IndexOf(lstSourceGD_HinhThuc.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["GDICH_HTHUC"].ToString())));
                        if (!drThongTinChung["GDICH_TIEN_MAT"].ToString().IsNullOrEmpty())
                            numGDTienMat.Value = Convert.ToDouble(drThongTinChung["GDICH_TIEN_MAT"]);
                        if (!drThongTinChung["GDICH_TIEN_CKHOAN"].ToString().IsNullOrEmpty())
                            numGDTienCKhoan.Value = Convert.ToDouble(drThongTinChung["GDICH_TIEN_CKHOAN"]);
                        txtGD_TKhoan_KH.Text = drThongTinChung["GDICH_TKHOAN_KHANG"].ToString();
                        lblGD_TKhoan_KH.Content = drThongTinChung["TEN_TKHOAN_KH"].ToString();
                        txtGD_TKhoan_NoiBo.Text = drThongTinChung["GDICH_TKHOAN_NBO"].ToString();
                        lblGD_TKhoan_NoiBo.Content = drThongTinChung["TEN_TKHOAN_NOIBO"].ToString();
                        txtDienGiai.Text = drThongTinChung["DIEN_GIAI"].ToString();

                        //Group Thông tin khác
                        cmbMaCBQL.SelectedIndex = lstSourceCBQL.IndexOf(lstSourceCBQL.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["MA_CBQL"])));
                        txtSoKU.Text = drThongTinChung["MA_KUOC"].ToString();
                        if (!drThongTinChung["SO_DU_GOC"].ToString().IsNullOrEmpty())
                            numSoDuGoc.Value = Convert.ToDouble(drThongTinChung["SO_DU_GOC"]);
                        if(!drThongTinChung["SO_TIEN_KY"].ToString().IsNullOrEmpty())
                            txtSoTienGuiMoiKy.Value = Convert.ToDouble(drThongTinChung["SO_TIEN_KY"]);
                        if(LDateTime.IsDate(drThongTinChung["NGAY_ADUNG"].ToString(),"yyyyMMdd"))
                            raddtNgayADKU.Value = LDateTime.StringToDate(drThongTinChung["NGAY_ADUNG"].ToString(), "yyyyMMdd");
                        #endregion

                        #region Tab thông tin số dư tiền lãi
                        if (action == DatabaseConstant.Action.XEM)
                        {
                            BL_TIEN_GUI objBLTienGui = new BL_TIEN_GUI();
                            List<BL_TIEN_GUI_DCSH> lstBLTienGuiDCSH = new List<BL_TIEN_GUI_DCSH>();
                            List<BL_TIEN_GUI_KHOACH> lstBLTienGuiKHoach = new List<BL_TIEN_GUI_KHOACH>();
                            HDV_THONG_TIN_SO_TGUI objThongTinSoTG = null;
                            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                            objBLTienGui.ID = id;
                            objBLTienGui.SO_SO_TG = txtSoSoTG.Text;
                            DateTime dt2 = DateTime.Now;
                            ret = processHDV.MoSoTietKiemQuyDinh(DatabaseConstant.Action.TINH_TOAN, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);
                            LLogging.WriteLog("TinhLai-SoDu", LLogging.LogType.BUS, DateTime.Now.Subtract(dt2).TotalMilliseconds.ToString());
                            if (ret)
                            {
                                //Số dư
                                numSoDuHienTai.Value = Convert.ToDouble(objThongTinSoTG.SO_DU_HIEN_TAI);
                                numSTPhongToa.Value = Convert.ToDouble(objThongTinSoTG.SO_TIEN_PHONG_TOA);
                                numSoDuKhaDung.Value = Convert.ToDouble(objThongTinSoTG.SO_DU_KHA_DUNG);

                                //Tiền lãi
                                numTinhLaiHT.Value = Convert.ToDouble(objThongTinSoTG.LAI_TINH_DEN_HIEN_TAI);
                                raddtTLai_NgayKyMoi.Value = LDateTime.StringToDate(objThongTinSoTG.NGAY_KY_MOI, "yyyyMMdd");
                            }
                        }
                        #endregion

                        #region Tab đồng chủ sở hữu
                        dsDCSH = new DataSet();
                        dsDCSH.Tables.Add(dtDCSH.Copy());
                        DataViewManager dataViewManager = new DataViewManager(dsDCSH);
                        DataView dataView = dataViewManager.CreateDataView(dsDCSH.Tables[0]);
                        grDongSoHuuDS.DataContext = dataView;
                        #endregion

                        #region Tab lịch sử lãi suất
                        if (action == DatabaseConstant.Action.XEM)
                        {
                            raddgrLaiSuatLS.ItemsSource = null;
                            raddgrLaiSuatLS.ItemsSource = dtLSuat;
                        }
                        #endregion

                        #region Tab lịch sử giao dịch
                        if (action == DatabaseConstant.Action.XEM)
                        {
                            grLichSuGD.ItemsSource = null;
                            grLichSuGD.ItemsSource = dtGiaoDich;
                        }
                        #endregion

                        #region Tab thông tin kiểm soát
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(drThongTinChung["TTHAI_BGHI"].ToString());
                        raddtNgayLap.Value = LDateTime.StringToDate(drThongTinChung["NGAY_NHAP"].ToString(), "yyyyMMdd");
                        txtNguoiLap.Text = drThongTinChung["NGUOI_NHAP"].ToString();
                        if (LDateTime.IsDate(drThongTinChung["NGAY_CNHAT"].ToString(), "yyyyMMdd") == true)
                            raddtNgayCapNhat.Value = LDateTime.StringToDate(drThongTinChung["NGAY_CNHAT"].ToString(), "yyyyMMdd");
                        else
                            raddtNgayCapNhat.Value = null;
                        txtNguoiCapNhat.Text = drThongTinChung["NGUOI_CNHAT"].ToString();
                        #endregion
                    }

                    // Hiện thị thông tin lên Form theo sổ tiết kiệm không kỳ hạn
                    else if (function == DatabaseConstant.Function.HDV_SO_TKKKH)
                    {
                        MaGiaoDich = drThongTinChung["MA_GDICH"].ToString();
                        sTrangThaiNVu = drThongTinChung["TTHAI_NVU"].ToString();
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        if (drThongTinChung["NGAY_GDICH"] != DBNull.Value)
                            ngayGiaoDich = drThongTinChung["NGAY_GDICH"].ToString();
                        #region Tab thông tin chung
                        //Group Thông tin khách hàng
                        lblID_KH.Content = drThongTinChung["ID_KHANG"].ToString();
                        txtMaKhachHang.Text = drThongTinChung["MA_KHANG"].ToString();
                        lblTenKH.Content = drThongTinChung["TEN_KHANG"].ToString();
                        txtDiaChi.Text = drThongTinChung["DIA_CHI"].ToString();
                        if (drThongTinChung["DD_GTLQ_LOAI"].ToString().Equals("CHUNG_MINH_ND"))
                        {
                            txtCMND.Text = drThongTinChung["DD_GTLQ_SO"].ToString();
                            txtNoiCap.Text = drThongTinChung["DD_GTLQ_NOI_CAP"].ToString();
                            if (LDateTime.IsDate(drThongTinChung["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd") == true)
                                raddtNgayCap.Value = LDateTime.StringToDate(drThongTinChung["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd");
                        }
                        txtSDT.Text = drThongTinChung["SO_DTHOAI"].ToString();

                        //Group Thông tin sổ tiền gửi
                        txtSoGiaoDich.Text = drThongTinChung["MA_GDICH"].ToString();
                        txtSoSoTG.Text = drThongTinChung["SO_SO_TG"].ToString();
                        sNhomSP = drThongTinChung["MA_NHOM_SP"].ToString();
                        lblID_SanPham.Content = drThongTinChung["ID_SAN_PHAM"].ToString();
                        txtMaSanPham.Text = drThongTinChung["MA_SAN_PHAM"].ToString();
                        lblTenSanPham.Content = drThongTinChung["TEN_SAN_PHAM"].ToString();
                        cmbLoaiTien.SelectedIndex = lstSourceLoaiTien.IndexOf(lstSourceLoaiTien.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["MA_LOAI_TIEN"].ToString())));
                        numSoTien.Value = Convert.ToDouble(drThongTinChung["SO_TIEN"]);
                        numKyHan.Value = null;
                        cmbKyHan_DVi_Tinh.Text = "";
                        numLaiSuat.Value = Convert.ToDouble(drThongTinChung["LAI_SUAT"]);
                        cmbLaiSuat_DVi_Tinh.SelectedIndex = lstSourceLaiSuat_DVi.IndexOf(lstSourceLaiSuat_DVi.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["LAI_SUAT_DVI_TINH"].ToString())));
                        if (LDateTime.IsDate(drThongTinChung["NGAY_MO_SO"].ToString(), "yyyyMMdd") == true)
                        {
                            raddtNgayMo.Value = LDateTime.StringToDate(drThongTinChung["NGAY_MO_SO"].ToString(), "yyyyMMdd");
                            dtpNgayMo.SelectedDate = raddtNgayMo.Value;
                        }
                        else
                        {
                            raddtNgayMo.Value = null;
                        }
                        raddtNgayDaoHan.Value = null;
                        cmbNguonVon.SelectedIndex = lstSourceNguonVon.IndexOf(lstSourceNguonVon.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["NV_LOAI_NVON"].ToString())));

                        if (!ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BENTRE.layGiaTri()))
                            cmbNguonVonCT.SelectedIndex = lstSourceNguonVonCT.IndexOf(lstSourceNguonVonCT.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["NV_LOAI_NVON_CT"].ToString())));

                        //Group Thông tin giao dịch
                        cmbGDHinhThuc.SelectedIndex = lstSourceGD_HinhThuc.IndexOf(lstSourceGD_HinhThuc.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["GDICH_HTHUC"].ToString())));
                        numGDTienMat.Value = Convert.ToDouble(drThongTinChung["GDICH_TIEN_MAT"]);
                        numGDTienCKhoan.Value = Convert.ToDouble(drThongTinChung["GDICH_TIEN_CKHOAN"]);
                        txtGD_TKhoan_KH.Text = drThongTinChung["GDICH_TKHOAN_KHANG"].ToString();
                        lblGD_TKhoan_KH.Content = drThongTinChung["TEN_TKHOAN_KH"].ToString();
                        txtGD_TKhoan_NoiBo.Text = drThongTinChung["GDICH_TKHOAN_NBO"].ToString();
                        lblGD_TKhoan_NoiBo.Content = drThongTinChung["TEN_TKHOAN_NOIBO"].ToString();
                        txtDienGiai.Text = drThongTinChung["DIEN_GIAI"].ToString();

                        //Group Thông tin khác
                        cmbMaCBQL.SelectedIndex = lstSourceCBQL.IndexOf(lstSourceCBQL.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["MA_CBQL"])));
                        #endregion

                        #region Tab thông tin số dư tiền lãi
                        if (action == DatabaseConstant.Action.XEM)
                        {
                            BL_TIEN_GUI objBLTienGui = new BL_TIEN_GUI();
                            List<BL_TIEN_GUI_DCSH> lstBLTienGuiDCSH = new List<BL_TIEN_GUI_DCSH>();
                            List<BL_TIEN_GUI_KHOACH> lstBLTienGuiKHoach = new List<BL_TIEN_GUI_KHOACH>();
                            HDV_THONG_TIN_SO_TGUI objThongTinSoTG = null;
                            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                            objBLTienGui.ID = id;
                            objBLTienGui.SO_SO_TG = txtSoSoTG.Text;
                            ret = processHDV.MoSoTietKiemKhongKyHan(DatabaseConstant.Action.TINH_TOAN, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);
                            if (ret)
                            {
                                //Số dư
                                numSoDuHienTai.Value = Convert.ToDouble(objThongTinSoTG.SO_DU_HIEN_TAI);
                                numSTPhongToa.Value = Convert.ToDouble(objThongTinSoTG.SO_TIEN_PHONG_TOA);
                                numSoDuKhaDung.Value = Convert.ToDouble(objThongTinSoTG.SO_DU_KHA_DUNG);

                                //Tiền lãi
                                numTinhLaiHT.Value = Convert.ToDouble(objThongTinSoTG.LAI_TINH_DEN_HIEN_TAI);
                                raddtTLai_NgayKyMoi.Value = LDateTime.StringToDate(objThongTinSoTG.NGAY_KY_MOI, "yyyyMMdd");
                            }
                        }
                        #endregion

                        #region Tab đồng chủ sở hữu
                        dsDCSH = new DataSet();
                        dsDCSH.Tables.Add(dtDCSH.Copy());
                        DataViewManager dataViewManager = new DataViewManager(dsDCSH);
                        DataView dataView = dataViewManager.CreateDataView(dsDCSH.Tables[0]);
                        grDongSoHuuDS.DataContext = dataView;
                        #endregion

                        #region Tab lịch sử lãi suất
                        if (action == DatabaseConstant.Action.XEM)
                        {
                            raddgrLaiSuatLS.ItemsSource = null;
                            raddgrLaiSuatLS.ItemsSource = dtLSuat;
                        }
                        #endregion

                        #region Tab lịch sử giao dịch
                        if (action == DatabaseConstant.Action.XEM)
                        {
                            grLichSuGD.ItemsSource = null;
                            grLichSuGD.ItemsSource = dtGiaoDich;
                        }
                        #endregion

                        #region Tab thông tin kiểm soát
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(drThongTinChung["TTHAI_BGHI"].ToString());
                        raddtNgayLap.Value = LDateTime.StringToDate(drThongTinChung["NGAY_NHAP"].ToString(), "yyyyMMdd");
                        txtNguoiLap.Text = drThongTinChung["NGUOI_NHAP"].ToString();
                        if (LDateTime.IsDate(drThongTinChung["NGAY_CNHAT"].ToString(), "yyyyMMdd") == true)
                            raddtNgayCapNhat.Value = LDateTime.StringToDate(drThongTinChung["NGAY_CNHAT"].ToString(), "yyyyMMdd");
                        else
                            raddtNgayCapNhat.Value = null;
                        txtNguoiCapNhat.Text = drThongTinChung["NGUOI_CNHAT"].ToString();
                        #endregion
                    }

                    // Hiện thị thông tin lên Form theo sổ tiết kiệm có kỳ hạn
                    else if (function == DatabaseConstant.Function.HDV_SO_TKCKH)
                    {
                        MaGiaoDich = drThongTinChung["MA_GDICH"].ToString();
                        sTrangThaiNVu = drThongTinChung["TTHAI_NVU"].ToString();
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        if (drThongTinChung["NGAY_GDICH"] != DBNull.Value)
                            ngayGiaoDich = drThongTinChung["NGAY_GDICH"].ToString();
                        #region Tab thông tin chung
                        //Group Thông tin khách hàng
                        lblID_KH.Content = drThongTinChung["ID_KHANG"].ToString();
                        txtMaKhachHang.Text = drThongTinChung["MA_KHANG"].ToString();
                        lblTenKH.Content = drThongTinChung["TEN_KHANG"].ToString();
                        txtDiaChi.Text = drThongTinChung["DIA_CHI"].ToString();
                        if (drThongTinChung["DD_GTLQ_LOAI"].ToString().Equals("CHUNG_MINH_ND"))
                        {
                            txtCMND.Text = drThongTinChung["DD_GTLQ_SO"].ToString();
                            txtNoiCap.Text = drThongTinChung["DD_GTLQ_NOI_CAP"].ToString();
                            if (LDateTime.IsDate(drThongTinChung["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd") == true)
                                raddtNgayCap.Value = LDateTime.StringToDate(drThongTinChung["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd");
                        }
                        txtSDT.Text = drThongTinChung["SO_DTHOAI"].ToString();

                        //Group Thông tin sổ tiền gửi
                        txtSoGiaoDich.Text = drThongTinChung["MA_GDICH"].ToString();
                        txtSoSoTG.Text = drThongTinChung["SO_SO_TG"].ToString();
                        sNhomSP = drThongTinChung["MA_NHOM_SP"].ToString();
                        lblID_SanPham.Content = drThongTinChung["ID_SAN_PHAM"].ToString();
                        txtMaSanPham.Text = drThongTinChung["MA_SAN_PHAM"].ToString();
                        lblTenSanPham.Content = drThongTinChung["TEN_SAN_PHAM"].ToString();
                        cmbLoaiTien.SelectedIndex = lstSourceLoaiTien.IndexOf(lstSourceLoaiTien.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["MA_LOAI_TIEN"].ToString())));
                        numSoTien.Value = Convert.ToDouble(drThongTinChung["SO_TIEN"]);
                        numKyHan.Value = Convert.ToDouble(drThongTinChung["KY_HAN"]);
                        cmbKyHan_DVi_Tinh.SelectedIndex = lstSourceKyHan_DVi.IndexOf(lstSourceKyHan_DVi.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["KY_HAN_DVI_TINH"].ToString())));
                        numLaiSuat.Value = Convert.ToDouble(drThongTinChung["LAI_SUAT"]);
                        cmbLaiSuat_DVi_Tinh.SelectedIndex = lstSourceLaiSuat_DVi.IndexOf(lstSourceLaiSuat_DVi.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["LAI_SUAT_DVI_TINH"].ToString())));                        
                        if (LDateTime.IsDate(drThongTinChung["NGAY_MO_SO"].ToString(), "yyyyMMdd") == true)
                        {
                            raddtNgayMo.Value = LDateTime.StringToDate(drThongTinChung["NGAY_MO_SO"].ToString(), "yyyyMMdd");
                            dtpNgayMo.SelectedDate = raddtNgayMo.Value;
                        }
                        else
                        {
                            raddtNgayMo.Value = null;
                        }
                        if (LDateTime.IsDate(drThongTinChung["NGAY_DEN_HAN"].ToString(), "yyyyMMdd") == true)
                        {
                            raddtNgayDaoHan.Value = LDateTime.StringToDate(drThongTinChung["NGAY_DEN_HAN"].ToString(), "yyyyMMdd");
                            dtpNgayDHan.SelectedDate = raddtNgayDaoHan.Value;
                        }
                        else
                        {
                            raddtNgayDaoHan.Text = "";
                        }
                        cmbNguonVon.SelectedIndex = lstSourceNguonVon.IndexOf(lstSourceNguonVon.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["NV_LOAI_NVON"].ToString())));

                        if (!ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BENTRE.layGiaTri()))
                            cmbNguonVonCT.SelectedIndex = lstSourceNguonVonCT.IndexOf(lstSourceNguonVonCT.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["NV_LOAI_NVON_CT"].ToString())));

                        //Group Thông tin giao dịch
                        cmbGDHinhThuc.SelectedIndex = lstSourceGD_HinhThuc.IndexOf(lstSourceGD_HinhThuc.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["GDICH_HTHUC"].ToString())));
                        numGDTienMat.Value = Convert.ToDouble(drThongTinChung["GDICH_TIEN_MAT"]);
                        numGDTienCKhoan.Value = Convert.ToDouble(drThongTinChung["GDICH_TIEN_CKHOAN"]);
                        txtGD_TKhoan_KH.Text = drThongTinChung["GDICH_TKHOAN_KHANG"].ToString();
                        lblGD_TKhoan_KH.Content = drThongTinChung["TEN_TKHOAN_KH"].ToString();
                        txtGD_TKhoan_NoiBo.Text = drThongTinChung["GDICH_TKHOAN_NBO"].ToString();
                        lblGD_TKhoan_NoiBo.Content = drThongTinChung["TEN_TKHOAN_NOIBO"].ToString();
                        txtDienGiai.Text = drThongTinChung["DIEN_GIAI"].ToString();

                        //Group Thông tin khác
                        cmbMaCBQL.SelectedIndex = lstSourceCBQL.IndexOf(lstSourceCBQL.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["MA_CBQL"])));
                        #endregion

                        #region Tab thông tin số dư tiền lãi
                        if (action == DatabaseConstant.Action.XEM)
                        {
                            BL_TIEN_GUI objBLTienGui = new BL_TIEN_GUI();
                            List<BL_TIEN_GUI_DCSH> lstBLTienGuiDCSH = new List<BL_TIEN_GUI_DCSH>();
                            List<BL_TIEN_GUI_KHOACH> lstBLTienGuiKHoach = new List<BL_TIEN_GUI_KHOACH>();
                            HDV_THONG_TIN_SO_TGUI objThongTinSoTG = null;
                            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                            objBLTienGui.ID = id;
                            objBLTienGui.SO_SO_TG = txtSoSoTG.Text;
                            ret = processHDV.MoSoTietKiemCoKyHan(DatabaseConstant.Action.TINH_TOAN, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);
                            if (ret)
                            {
                                //Số dư
                                numSoDuHienTai.Value = Convert.ToDouble(objThongTinSoTG.SO_DU_HIEN_TAI);
                                numSTPhongToa.Value = Convert.ToDouble(objThongTinSoTG.SO_TIEN_PHONG_TOA);
                                numSoDuKhaDung.Value = Convert.ToDouble(objThongTinSoTG.SO_DU_KHA_DUNG);

                                //Tiền lãi
                                numTinhLaiHT.Value = Convert.ToDouble(objThongTinSoTG.LAI_TINH_DEN_HIEN_TAI);
                                numLaiDaTra.Value = Convert.ToDouble(objThongTinSoTG.LAI_DA_TRA);
                                numDCLuyKe.Value = Convert.ToDouble(objThongTinSoTG.LAI_DU_CHI_LK);

                                //Số tiền phân bổ
                                numSoTienPB.Value = Convert.ToDouble(objThongTinSoTG.SO_TIEN_PB);
                                numSoTienPBLK.Value = Convert.ToDouble(objThongTinSoTG.SO_TIEN_PB_LK);
                                numSoTienConPB.Value = Convert.ToDouble(objThongTinSoTG.SO_TIEN_CON_PB);

                                //Lãi nhập gốc
                                numLNGLK.Value = Convert.ToDouble(objThongTinSoTG.LNG_LUY_KE);
                                numSoVongLNG.Value = Convert.ToDouble(objThongTinSoTG.SO_VONG_LNG);
                                if (LDateTime.IsDate(objThongTinSoTG.NGAY_KY_MOI_LNG, "yyyyMMdd"))
                                    raddtNgayKyMoi.Value = LDateTime.StringToDate(objThongTinSoTG.NGAY_KY_MOI_LNG, "yyyyMMdd");

                            }
                        }
                        #endregion

                        #region Tab chỉ thị đáo hạn
                        if (!drThongTinChung["DHAN_CTHI"].IsNullOrEmpty())
                            cmbDHan_CThi.SelectedIndex = lstSourceDHan_ChiThi.IndexOf(lstSourceDHan_ChiThi.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["DHAN_CTHI"].ToString())));

                        if(!drThongTinChung["DHAN_KY_HAN"].IsNullOrEmpty())
                            numDHan_KyHan.Value = Convert.ToInt32(drThongTinChung["DHAN_KY_HAN"]);

                        if (!drThongTinChung["DHAN_KY_HAN_DVI_TINH"].IsNullOrEmpty())
                            cmbDHan_KyHan_DViTinh.SelectedIndex = lstSourceDHan_KyHan_DVi.IndexOf(lstSourceDHan_KyHan_DVi.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["DHAN_KY_HAN_DVI_TINH"].ToString())));

                        if (!drThongTinChung["DHAN_MA_LSUAT"].IsNullOrEmpty())
                            txtDHan_MaLaiSuat.Text = drThongTinChung["DHAN_MA_LSUAT"].ToString();

                        if (!drThongTinChung["DHAN_TKHOAN_TRGOC"].IsNullOrEmpty())
                            txtDHan_TKhoanTraGoc.Text = drThongTinChung["DHAN_TKHOAN_TRGOC"].ToString();

                        if (!drThongTinChung["DHAN_TKHOAN_TRLAI"].IsNullOrEmpty())
                            txtDHan_TKhoanTraLai.Text = drThongTinChung["DHAN_TKHOAN_TRLAI"].ToString();
                        #endregion

                        #region Tab đồng chủ sở hữu
                        dsDCSH = new DataSet();
                        dsDCSH.Tables.Add(dtDCSH.Copy());
                        DataViewManager dataViewManager = new DataViewManager(dsDCSH);
                        DataView dataView = dataViewManager.CreateDataView(dsDCSH.Tables[0]);
                        grDongSoHuuDS.DataContext = dataView;
                        #endregion

                        #region Tab lịch sử lãi suất
                        if (action == DatabaseConstant.Action.XEM)
                        {
                            raddgrLaiSuatLS.ItemsSource = null;
                            raddgrLaiSuatLS.ItemsSource = dtLSuat;
                        }
                        #endregion

                        #region Tab lịch sử giao dịch
                        if (action == DatabaseConstant.Action.XEM)
                        {
                            grLichSuGD.ItemsSource = null;
                            grLichSuGD.ItemsSource = dtGiaoDich;
                        }
                        #endregion

                        #region Tab thông tin kiểm soát
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(drThongTinChung["TTHAI_BGHI"].ToString());
                        raddtNgayLap.Value = LDateTime.StringToDate(drThongTinChung["NGAY_NHAP"].ToString(), "yyyyMMdd");
                        txtNguoiLap.Text = drThongTinChung["NGUOI_NHAP"].ToString();
                        if (LDateTime.IsDate(drThongTinChung["NGAY_CNHAT"].ToString(), "yyyyMMdd") == true)
                            raddtNgayCapNhat.Value = LDateTime.StringToDate(drThongTinChung["NGAY_CNHAT"].ToString(), "yyyyMMdd");
                        else
                            raddtNgayCapNhat.Value = null;
                        txtNguoiCapNhat.Text = drThongTinChung["NGUOI_CNHAT"].ToString();
                        #endregion
                    }

                    // Hiện thị thông tin lên Form theo sổ tài khoản tiền gửi thanh toán
                    else if (function == DatabaseConstant.Function.HDV_SO_TK_TGTT)
                    {
                        MaGiaoDich = drThongTinChung["MA_GDICH"].ToString();
                        sTrangThaiNVu = drThongTinChung["TTHAI_NVU"].ToString();
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        if (drThongTinChung["NGAY_GDICH"] != DBNull.Value)
                            ngayGiaoDich = drThongTinChung["NGAY_GDICH"].ToString();
                        #region Tab thông tin chung
                        //Group Thông tin khách hàng
                        lblID_KH.Content = drThongTinChung["ID_KHANG"].ToString();
                        txtMaKhachHang.Text = drThongTinChung["MA_KHANG"].ToString();
                        lblTenKH.Content = drThongTinChung["TEN_KHANG"].ToString();
                        txtDiaChi.Text = drThongTinChung["DIA_CHI"].ToString();
                        if (drThongTinChung["DD_GTLQ_LOAI"].ToString().Equals("CHUNG_MINH_ND"))
                        {
                            txtCMND.Text = drThongTinChung["DD_GTLQ_SO"].ToString();
                            txtNoiCap.Text = drThongTinChung["DD_GTLQ_NOI_CAP"].ToString();
                            if (LDateTime.IsDate(drThongTinChung["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd") == true)
                                raddtNgayCap.Value = LDateTime.StringToDate(drThongTinChung["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd");
                        }
                        txtSDT.Text = drThongTinChung["SO_DTHOAI"].ToString();

                        //Group Thông tin sổ tiền gửi
                        txtSoGiaoDich.Text = drThongTinChung["MA_GDICH"].ToString();
                        txtSoSoTG.Text = drThongTinChung["SO_SO_TG"].ToString();
                        sNhomSP = drThongTinChung["MA_NHOM_SP"].ToString();
                        lblID_SanPham.Content = drThongTinChung["ID_SAN_PHAM"].ToString();
                        txtMaSanPham.Text = drThongTinChung["MA_SAN_PHAM"].ToString();
                        lblTenSanPham.Content = drThongTinChung["TEN_SAN_PHAM"].ToString();
                        cmbLoaiTien.SelectedIndex = lstSourceLoaiTien.IndexOf(lstSourceLoaiTien.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["MA_LOAI_TIEN"].ToString())));
                        numSoTien.Value = Convert.ToDouble(drThongTinChung["SO_TIEN"]);
                        numKyHan.Value = null;
                        cmbKyHan_DVi_Tinh.Text = "";
                        numLaiSuat.Value = Convert.ToDouble(drThongTinChung["LAI_SUAT"]);
                        cmbLaiSuat_DVi_Tinh.SelectedIndex = lstSourceLaiSuat_DVi.IndexOf(lstSourceLaiSuat_DVi.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["LAI_SUAT_DVI_TINH"].ToString())));
                        if (LDateTime.IsDate(drThongTinChung["NGAY_MO_SO"].ToString(), "yyyyMMdd") == true)
                        {
                            raddtNgayMo.Value = LDateTime.StringToDate(drThongTinChung["NGAY_MO_SO"].ToString(), "yyyyMMdd");
                            dtpNgayMo.SelectedDate = raddtNgayMo.Value;
                        }
                        else
                        {
                            raddtNgayMo.Value = null;
                        }
                        raddtNgayDaoHan.Value = null;
                        cmbNguonVon.SelectedIndex = lstSourceNguonVon.IndexOf(lstSourceNguonVon.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["NV_LOAI_NVON"].ToString())));

                        if (!ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BENTRE.layGiaTri()))
                            cmbNguonVonCT.SelectedIndex = lstSourceNguonVonCT.IndexOf(lstSourceNguonVonCT.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["NV_LOAI_NVON_CT"].ToString())));

                        //Group Thông tin giao dịch
                        cmbGDHinhThuc.SelectedIndex = lstSourceGD_HinhThuc.IndexOf(lstSourceGD_HinhThuc.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["GDICH_HTHUC"].ToString())));
                        numGDTienMat.Value = Convert.ToDouble(drThongTinChung["GDICH_TIEN_MAT"]);
                        numGDTienCKhoan.Value = Convert.ToDouble(drThongTinChung["GDICH_TIEN_CKHOAN"]);
                        txtGD_TKhoan_KH.Text = drThongTinChung["GDICH_TKHOAN_KHANG"].ToString();
                        lblGD_TKhoan_KH.Content = drThongTinChung["TEN_TKHOAN_KH"].ToString();
                        txtGD_TKhoan_NoiBo.Text = drThongTinChung["GDICH_TKHOAN_NBO"].ToString();
                        lblGD_TKhoan_NoiBo.Content = drThongTinChung["TEN_TKHOAN_NOIBO"].ToString();
                        txtDienGiai.Text = drThongTinChung["DIEN_GIAI"].ToString();

                        //Group Thông tin khác
                        cmbMaCBQL.SelectedIndex = lstSourceCBQL.IndexOf(lstSourceCBQL.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["MA_CBQL"])));
                        txtSoKU.Text = drThongTinChung["MA_KUOC"].ToString();
                        #endregion

                        #region Tab thông tin số dư tiền lãi
                        if (action == DatabaseConstant.Action.XEM)
                        {
                            BL_TIEN_GUI objBLTienGui = new BL_TIEN_GUI();
                            List<BL_TIEN_GUI_DCSH> lstBLTienGuiDCSH = new List<BL_TIEN_GUI_DCSH>();
                            List<BL_TIEN_GUI_KHOACH> lstBLTienGuiKHoach = new List<BL_TIEN_GUI_KHOACH>();
                            HDV_THONG_TIN_SO_TGUI objThongTinSoTG = null;
                            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                            objBLTienGui.ID = id;
                            objBLTienGui.SO_SO_TG = txtSoSoTG.Text;
                            ret = processHDV.MoTaiKhoanTienGuiThanhToan(DatabaseConstant.Action.TINH_TOAN, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);
                            if (ret)
                            {
                                //Số dư
                                numSoDuHienTai.Value = Convert.ToDouble(objThongTinSoTG.SO_DU_HIEN_TAI);
                                numSTPhongToa.Value = Convert.ToDouble(objThongTinSoTG.SO_TIEN_PHONG_TOA);
                                numSoDuKhaDung.Value = Convert.ToDouble(objThongTinSoTG.SO_DU_KHA_DUNG);

                                //Tiền lãi
                                numTinhLaiHT.Value = Convert.ToDouble(objThongTinSoTG.LAI_TINH_DEN_HIEN_TAI);
                                raddtTLai_NgayKyMoi.Value = LDateTime.StringToDate(objThongTinSoTG.NGAY_KY_MOI, "yyyyMMdd");
                            }
                        }
                        #endregion

                        #region Tab đồng chủ sở hữu
                        dsDCSH = new DataSet();
                        dsDCSH.Tables.Add(dtDCSH.Copy());
                        DataViewManager dataViewManager = new DataViewManager(dsDCSH);
                        DataView dataView = dataViewManager.CreateDataView(dsDCSH.Tables[0]);
                        grDongSoHuuDS.DataContext = dataView;
                        #endregion

                        #region Tab lịch sử lãi suất
                        if (action == DatabaseConstant.Action.XEM)
                        {
                            raddgrLaiSuatLS.ItemsSource = null;
                            raddgrLaiSuatLS.ItemsSource = dtLSuat;
                        }
                        #endregion

                        #region Tab lịch sử giao dịch
                        if (action == DatabaseConstant.Action.XEM)
                        {
                            grLichSuGD.ItemsSource = null;
                            grLichSuGD.ItemsSource = dtGiaoDich;
                        }
                        #endregion

                        #region Tab thông tin kiểm soát
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(drThongTinChung["TTHAI_BGHI"].ToString());
                        raddtNgayLap.Value = LDateTime.StringToDate(drThongTinChung["NGAY_NHAP"].ToString(), "yyyyMMdd");
                        txtNguoiLap.Text = drThongTinChung["NGUOI_NHAP"].ToString();
                        if (LDateTime.IsDate(drThongTinChung["NGAY_CNHAT"].ToString(), "yyyyMMdd") == true)
                            raddtNgayCapNhat.Value = LDateTime.StringToDate(drThongTinChung["NGAY_CNHAT"].ToString(), "yyyyMMdd");
                        else
                            raddtNgayCapNhat.Value = null;
                        txtNguoiCapNhat.Text = drThongTinChung["NGUOI_CNHAT"].ToString();
                        #endregion
                    }

                    // Hiện thị thông tin lên Form theo sổ tiền gửi có kỳ hạn
                    else if (function == DatabaseConstant.Function.HDV_SO_TGCKH)
                    {
                        MaGiaoDich = drThongTinChung["MA_GDICH"].ToString();
                        sTrangThaiNVu = drThongTinChung["TTHAI_NVU"].ToString();
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        if (drThongTinChung["NGAY_GDICH"] != DBNull.Value)
                            ngayGiaoDich = drThongTinChung["NGAY_GDICH"].ToString();
                        #region Tab thông tin chung
                        //Group Thông tin khách hàng
                        lblID_KH.Content = drThongTinChung["ID_KHANG"].ToString();
                        txtMaKhachHang.Text = drThongTinChung["MA_KHANG"].ToString();
                        lblTenKH.Content = drThongTinChung["TEN_KHANG"].ToString();
                        txtDiaChi.Text = drThongTinChung["DIA_CHI"].ToString();
                        if (drThongTinChung["DD_GTLQ_LOAI"].ToString().Equals("CHUNG_MINH_ND"))
                        {
                            txtCMND.Text = drThongTinChung["DD_GTLQ_SO"].ToString();
                            txtNoiCap.Text = drThongTinChung["DD_GTLQ_NOI_CAP"].ToString();
                            if (LDateTime.IsDate(drThongTinChung["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd") == true)
                                raddtNgayCap.Value = LDateTime.StringToDate(drThongTinChung["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd");
                        }
                        txtSDT.Text = drThongTinChung["SO_DTHOAI"].ToString();

                        //Group Thông tin sổ tiền gửi
                        txtSoGiaoDich.Text = drThongTinChung["MA_GDICH"].ToString();
                        txtSoSoTG.Text = drThongTinChung["SO_SO_TG"].ToString();
                        sNhomSP = drThongTinChung["MA_NHOM_SP"].ToString();
                        lblID_SanPham.Content = drThongTinChung["ID_SAN_PHAM"].ToString();
                        txtMaSanPham.Text = drThongTinChung["MA_SAN_PHAM"].ToString();
                        lblTenSanPham.Content = drThongTinChung["TEN_SAN_PHAM"].ToString();
                        cmbLoaiTien.SelectedIndex = lstSourceLoaiTien.IndexOf(lstSourceLoaiTien.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["MA_LOAI_TIEN"].ToString())));
                        numSoTien.Value = Convert.ToDouble(drThongTinChung["SO_TIEN"]);
                        numKyHan.Text = "";
                        cmbKyHan_DVi_Tinh.Text = "";
                        numLaiSuat.Value = Convert.ToDouble(drThongTinChung["LAI_SUAT"]);
                        cmbKyHan_DVi_Tinh.SelectedIndex = lstSourceKyHan_DVi.IndexOf(lstSourceKyHan_DVi.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["LAI_SUAT_DVI_TINH"].ToString())));
                        if (LDateTime.IsDate(drThongTinChung["NGAY_MO_SO"].ToString(), "yyyyMMdd") == true)
                        {
                            raddtNgayMo.Value = LDateTime.StringToDate(drThongTinChung["NGAY_MO_SO"].ToString(), "yyyyMMdd");
                            dtpNgayMo.SelectedDate = raddtNgayMo.Value;
                        }
                        else
                        {
                            raddtNgayMo.Value = null;
                        }
                        raddtNgayDaoHan.Text = "";
                        cmbNguonVon.SelectedIndex = lstSourceNguonVon.IndexOf(lstSourceNguonVon.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["NV_LOAI_NVON"].ToString())));

                        if (!ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BENTRE.layGiaTri()))
                            cmbNguonVonCT.SelectedIndex = lstSourceNguonVonCT.IndexOf(lstSourceNguonVonCT.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["NV_LOAI_NVON_CT"].ToString())));

                        //Group Thông tin giao dịch
                        cmbGDHinhThuc.SelectedIndex = lstSourceGD_HinhThuc.IndexOf(lstSourceGD_HinhThuc.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["GDICH_HTHUC"].ToString())));
                        numGDTienMat.Value = Convert.ToDouble(drThongTinChung["GDICH_TIEN_MAT"]);
                        numGDTienCKhoan.Value = Convert.ToDouble(drThongTinChung["GDICH_TIEN_CKHOAN"]);
                        txtGD_TKhoan_KH.Text = drThongTinChung["GDICH_TKHOAN_KHANG"].ToString();
                        lblGD_TKhoan_KH.Content = drThongTinChung["TEN_TKHOAN_KH"].ToString();
                        txtGD_TKhoan_NoiBo.Text = drThongTinChung["GDICH_TKHOAN_NBO"].ToString();
                        lblGD_TKhoan_NoiBo.Content = drThongTinChung["TEN_TKHOAN_NOIBO"].ToString();
                        txtDienGiai.Text = drThongTinChung["DIEN_GIAI"].ToString();

                        //Group Thông tin khác
                        cmbMaCBQL.SelectedIndex = lstSourceCBQL.IndexOf(lstSourceCBQL.FirstOrDefault(i => i.KeywordStrings.First().Equals(drThongTinChung["MA_CBQL"])));
                        txtSoKU.Text = drThongTinChung["MA_KUOC"].ToString();
                        #endregion

                        #region Tab thông tin số dư tiền lãi
                        if (action == DatabaseConstant.Action.XEM)
                        {
                            BL_TIEN_GUI objBLTienGui = new BL_TIEN_GUI();
                            List<BL_TIEN_GUI_DCSH> lstBLTienGuiDCSH = new List<BL_TIEN_GUI_DCSH>();
                            List<BL_TIEN_GUI_KHOACH> lstBLTienGuiKHoach = new List<BL_TIEN_GUI_KHOACH>();
                            HDV_THONG_TIN_SO_TGUI objThongTinSoTG = null;
                            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                            objBLTienGui.ID = id;
                            objBLTienGui.SO_SO_TG = txtSoSoTG.Text;
                            ret = processHDV.MoSoTienGuiCoKyHan(DatabaseConstant.Action.TINH_TOAN, ref objBLTienGui, ref lstBLTienGuiDCSH, ref lstBLTienGuiKHoach, ref objThongTinSoTG, ref listClientResponseDetail);
                            if (ret)
                            {
                                //Số dư
                                numSoDuHienTai.Value = Convert.ToDouble(objThongTinSoTG.SO_DU_HIEN_TAI);
                                numSTPhongToa.Value = Convert.ToDouble(objThongTinSoTG.SO_TIEN_PHONG_TOA);
                                numSoDuKhaDung.Value = Convert.ToDouble(objThongTinSoTG.SO_DU_KHA_DUNG);

                                //Tiền lãi
                                numTinhLaiHT.Value = Convert.ToDouble(objThongTinSoTG.LAI_TINH_DEN_HIEN_TAI);
                                raddtTLai_NgayKyMoi.Value = LDateTime.StringToDate(objThongTinSoTG.NGAY_KY_MOI, "yyyyMMdd");
                            }
                        }
                        #endregion

                        #region Tab đồng chủ sở hữu
                        dsDCSH = new DataSet();
                        dsDCSH.Tables.Add(dtDCSH.Copy());
                        DataViewManager dataViewManager = new DataViewManager(dsDCSH);
                        DataView dataView = dataViewManager.CreateDataView(dsDCSH.Tables[0]);
                        grDongSoHuuDS.DataContext = dataView;
                        #endregion

                        #region Tab lịch sử lãi suất
                        if (action == DatabaseConstant.Action.XEM)
                        {
                            raddgrLaiSuatLS.ItemsSource = null;
                            raddgrLaiSuatLS.ItemsSource = dtLSuat;
                        }
                        #endregion

                        #region Tab lịch sử giao dịch
                        if (action == DatabaseConstant.Action.XEM)
                        {
                            grLichSuGD.ItemsSource = null;
                            grLichSuGD.ItemsSource = dtGiaoDich;
                        }
                        #endregion

                        #region Tab thông tin kiểm soát
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(drThongTinChung["TTHAI_BGHI"].ToString());
                        raddtNgayLap.Value = LDateTime.StringToDate(drThongTinChung["NGAY_NHAP"].ToString(), "yyyyMMdd");
                        txtNguoiLap.Text = drThongTinChung["NGUOI_NHAP"].ToString();
                        if (LDateTime.IsDate(drThongTinChung["NGAY_CNHAT"].ToString(), "yyyyMMdd") == true)
                            raddtNgayCapNhat.Value = LDateTime.StringToDate(drThongTinChung["NGAY_CNHAT"].ToString(), "yyyyMMdd");
                        else
                            raddtNgayCapNhat.Value = null;
                        txtNguoiCapNhat.Text = drThongTinChung["NGUOI_CNHAT"].ToString();
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                CommonFunction.ThongBaoLoi(ex);
            }
        }

        private bool Validation()
        {
            try
            {
                if (txtMaKhachHang.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblMaKH.Content.ToString());
                    txtMaKhachHang.Focus();
                    return false;
                }
                else if (txtSoSoTG.Text.IsNullOrEmptyOrSpace() && function != DatabaseConstant.Function.HDV_SO_TKQD && function != DatabaseConstant.Function.HDV_SO_TKKKH && function != DatabaseConstant.Function.HDV_SO_TK_TGTT
                    && (ClientInformation.Company != ApplicationConstant.DonViSuDung.BIDV.layGiaTri()
                        && ClientInformation.Company != ApplicationConstant.DonViSuDung.BIDV_BLF.layGiaTri()))
                {

                    CommonFunction.ThongBaoTrong(lblSoTGui.Content.ToString());
                    txtSoSoTG.Focus();
                    return false;

                }
                else if (txtMaSanPham.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblMaSP.Content.ToString());
                    txtMaSanPham.Focus();
                    return false;
                }
                else if (cmbLoaiTien.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblLoaiTien.Content.ToString());
                    cmbLoaiTien.Focus();
                    return false;
                }
                else if (numLaiSuat.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblLaiSuat.Content.ToString());
                    numLaiSuat.Focus();
                    return false;
                }
                else if (numSoTien.Value == null || numSoTien.Value <= 0)
                {
                    if (function != DatabaseConstant.Function.HDV_SO_TKQD && function != DatabaseConstant.Function.HDV_SO_TKKKH && function != DatabaseConstant.Function.HDV_SO_TK_TGTT)
                    {
                        CommonFunction.ThongBaoChuaNhap(lblSoTien.Content.ToString());
                        numSoTien.Focus();
                        return false;
                    }
                }
                else if (raddtNgayMo.Text.IsNullOrEmptyOrSpace() || raddtNgayMo.Text.Equals("__/__/____"))
                {
                    CommonFunction.ThongBaoTrong(lblNgayMo.Content.ToString());
                    raddtNgayMo.Focus();
                    return false;
                }
                else if (cmbNguonVon.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblNguonVon.Content.ToString());
                    cmbNguonVon.Focus();
                    return false;
                }
                else if (!ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BENTRE.layGiaTri()) && cmbNguonVonCT.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblNguonVonCT.Content.ToString());
                    cmbNguonVonCT.Focus();
                    return false;
                }
                else if (cmbGDHinhThuc.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblHinhThucGD.Content.ToString());
                    cmbGDHinhThuc.Focus();
                    return false;
                }
                else if (Function == DatabaseConstant.Function.HDV_SO_TKCKH && cmbDHan_CThi.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoTrong(lblDHan_CThi.Content.ToString());
                    cmbDHan_CThi.Focus();
                    return false;
                }
                else if (!cmbDHan_CThi.Text.IsNullOrEmptyOrSpace())
                {
                    if (ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BIDV_BLF.layGiaTri()))
                    {
                        string sChiThiDHan = lstSourceDHan_ChiThi.ElementAt(cmbDHan_CThi.SelectedIndex).KeywordStrings.ElementAt(0);

                        if (sChiThiDHan.Equals(BusinessConstant.layGiaTri(BusinessConstant.CHI_THI_DAO_HAN.GOC_LAI_TKCT)))
                        {
                            if (txtDHan_TKhoanTraGoc.Text.IsNullOrEmptyOrSpace())
                            {
                                CommonFunction.ThongBaoTrong(lblDHan_SoTKhoanTraGoc.Content.ToString());
                                txtDHan_TKhoanTraGoc.Focus();
                                return false;
                            }
                            else if (txtDHan_TKhoanTraLai.Text.IsNullOrEmptyOrSpace())
                            {
                                CommonFunction.ThongBaoTrong(lblDHan_SoTKhoanTraLai.Content.ToString());
                                txtDHan_TKhoanTraLai.Focus();
                                return false;
                            }
                        }
                        else if (sChiThiDHan.Equals(BusinessConstant.layGiaTri(BusinessConstant.CHI_THI_DAO_HAN.GQV_KHC_LSM_TKCT)))
                        {
                            if (txtDHan_TKhoanTraLai.Text.IsNullOrEmptyOrSpace())
                            {
                                CommonFunction.ThongBaoTrong(lblDHan_SoTKhoanTraLai.Content.ToString());
                                txtDHan_TKhoanTraLai.Focus();
                                return false;
                            }
                        }
                    }
                }
                //MA 20130719: Bỏ để tự động tạo sổ khi duyệt khách hàng
                //else if (cmbMaCBQL.Text.IsNullOrEmptyOrSpace())
                //{
                //    CommonFunction.ThongBaoTrong(lblCBQL.Content.ToString());
                //    cmbMaCBQL.Focus();
                //    return false;
                //}

                return true;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                return false;
            }
        }

        private void OnPreviewSo()
        {
            // Khai báo, khởi tạo chung
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (function == DatabaseConstant.Function.HDV_SO_TKCKH)
                {
                    // Cảnh báo khi không có dữ liệu
                    if (LObject.IsNullOrEmpty(id))
                    {
                        LMessage.ShowMessage("M.DungChung.KhongCoThongTin", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    // Thực hiện báo cáo khi có dữ liệu
                    else
                    {
                        if (ClientInformation.Company.Equals("HOCVIENNGANHANG"))
                        {
                            VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();

                            List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                            lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                            lstThamSo.Add(new ThamSoBaoCao("@MaPhongGD", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                            lstThamSo.Add(new ThamSoBaoCao("@SoSoTG", txtSoSoTG.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

                            string maBaoCao = DatabaseConstant.LayMaBaoCaoHVNH(DatabaseConstant.DanhSachBaoCaoHVNH.HDVO_SO_TIET_KIEM);
                            xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                        }
                        if (ClientInformation.Company.Equals("BIDV") || ClientInformation.Company.Equals("BIDV_BLF"))
                        {
                            VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();

                            List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                            lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                            lstThamSo.Add(new ThamSoBaoCao("@MaPhongGD", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                            lstThamSo.Add(new ThamSoBaoCao("@MaGiaoDich", txtSoGiaoDich.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

                            string maBaoCao = DatabaseConstant.LayMaBaoCaoBIDV(DatabaseConstant.DanhSachBaoCaoBIDV.HDVO_SO_TKCKH);
                            xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                        }
                    }
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
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void OnPreviewCT()
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
                    objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.HDV_SO_TGCKH;

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

        private void OnPreviewGiayGuiTien()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                // Cảnh báo khi không có dữ liệu
                if (LObject.IsNullOrEmpty(id) && LObject.IsNullOrEmpty(maGiaoDich))
                {
                    LMessage.ShowMessage("M.TinDung.ChuyenQuaHan.ucChuyenQuaHanCT.KhongCoThongTinSo", LMessage.MessageBoxType.Warning);
                    return;
                }
                else
                {
                    if (ClientInformation.Company.Equals("BANTAYVANG") || ClientInformation.Company.Equals("HOCVIENNGANHANG"))
                    {
                        VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();

                        List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                        lstThamSo.Add(new ThamSoBaoCao("P_NgayLamViec", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("@SoSoTG", txtSoSoTG.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                        lstThamSo.Add(new ThamSoBaoCao("@MaGiaoDich", maGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

                        string maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.HDVO_GIAY_GUI_TIEN);
                        xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                    }
                    else
                    {

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

        private void OnHold()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (!Validation()) return;

                string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));

                BL_TIEN_GUI objBLTienGui = new BL_TIEN_GUI();
                List<BL_TIEN_GUI_DCSH> lstBLTienGuiDCSH = new List<BL_TIEN_GUI_DCSH>();
                List<BL_TIEN_GUI_KHOACH> lstBLTienGuiKHoach = new List<BL_TIEN_GUI_KHOACH>();

                GetFormData(ref objBLTienGui, ref lstBLTienGuiDCSH, ref  lstBLTienGuiKHoach, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(ref objBLTienGui, ref lstBLTienGuiDCSH, ref lstBLTienGuiKHoach);
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(ref objBLTienGui, ref lstBLTienGuiDCSH, ref lstBLTienGuiKHoach);
                }
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                new frmThongBaoLoi("M.ResponseMessage.Common.LoiLuuDuLieu", ex).ShowDialog();
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void OnSave()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (!Validation()) return;

                string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));

                BL_TIEN_GUI objBLTienGui = new BL_TIEN_GUI();
                List<BL_TIEN_GUI_DCSH> lstBLTienGuiDCSH = new List<BL_TIEN_GUI_DCSH>();
                List<BL_TIEN_GUI_KHOACH> lstBLTienGuiKHoach = new List<BL_TIEN_GUI_KHOACH>();

                GetFormData(ref objBLTienGui, ref lstBLTienGuiDCSH, ref  lstBLTienGuiKHoach, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(ref objBLTienGui, ref lstBLTienGuiDCSH, ref lstBLTienGuiKHoach);
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(ref objBLTienGui, ref lstBLTienGuiDCSH, ref lstBLTienGuiKHoach);
                }
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                new frmThongBaoLoi("M.ResponseMessage.Common.LoiLuuDuLieu", ex).ShowDialog();
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }


        private void BeforeViewFromDetail()
        {
            try
            {
                SetEnabledAllControls(false);
                action = DatabaseConstant.Action.XEM;
                CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void BeforeViewFromList()
        {
            SetFormData();
            HideControl();
            BeforeViewFromDetail();
        }


        private void BeforeAddNew()
        {
            try
            {
                HideControl();
                ResetForm();
                SetEnabledAllControls(true);
                SetEnabledRequiredControls(true);
                action = DatabaseConstant.Action.THEM;
                CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void OnAddNew(ref BL_TIEN_GUI objBLTienGui, ref List<BL_TIEN_GUI_DCSH> lstBLTienGuiDCSH, ref List<BL_TIEN_GUI_KHOACH> lstBLTienGuiKHoach)
        {
            try
            {
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                HDV_THONG_TIN_SO_TGUI objThongTinSoTG = null;
                bool ret = false;

                DatabaseConstant.Action action = DatabaseConstant.Action.THEM;

                if (function == DatabaseConstant.Function.HDV_SO_TKQD)
                    ret = processHDV.MoSoTietKiemQuyDinh(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);

                else if (function == DatabaseConstant.Function.HDV_SO_TKKKH)
                    ret = processHDV.MoSoTietKiemKhongKyHan(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);

                else if (function == DatabaseConstant.Function.HDV_SO_TKCKH)
                    ret = processHDV.MoSoTietKiemCoKyHan(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);

                else if (function == DatabaseConstant.Function.HDV_SO_TK_TGTT)
                    ret = processHDV.MoTaiKhoanTienGuiThanhToan(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);

                else if (function == DatabaseConstant.Function.HDV_SO_TGCKH)
                    ret = processHDV.MoSoTienGuiCoKyHan(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref lstBLTienGuiKHoach, ref objThongTinSoTG, ref listClientResponseDetail);

                string s = DatabaseConstant.Action.THEM.layNgonNgu();
                AfterAddNew(ret, objBLTienGui, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void AfterAddNew(bool ret, BL_TIEN_GUI objBLTienGui, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);
                    ID = objBLTienGui.ID;
                    MaGiaoDich = objBLTienGui.MA_GDICH;
                    sTrangThaiNVu = objBLTienGui.TTHAI_NVU;

                    txtSoGiaoDich.Text = objBLTienGui.MA_GDICH;
                    txtSoSoTG.Text = objBLTienGui.SO_SO_TG;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    SetEnabledAllControls(false);
                    SetEnabledRequiredControls(false);
                    action = DatabaseConstant.Action.XEM;
                    CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);

                }
                else
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    //LMessage.ShowMessage("M.DungChung.ThemKhongThanhCong", LMessage.MessageBoxType.Error);
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


        private void BeforeModifyFromDetail()
        {
            try
            {
                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);

                bool ret = process.LockData(DatabaseConstant.Module.HDVO,
                    function,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                if (ret)
                {
                    SetEnabledAllControls(true);
                    SetEnabledRequiredControls(false);
                    cmbGDHinhThuc_SelectionChanged(null, null);
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

        private void BeforeModifyFromList()
        {
            try
            {
                SetFormData();
                HideControl();
                SetEnabledAllControls(true);
                SetEnabledRequiredControls(false);
                action = DatabaseConstant.Action.SUA;
                CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void OnModify(ref BL_TIEN_GUI objBLTienGui, ref List<BL_TIEN_GUI_DCSH> lstBLTienGuiDCSH, ref List<BL_TIEN_GUI_KHOACH> lstBLTienGuiKHoach)
        {
            try
            {
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                HDV_THONG_TIN_SO_TGUI objThongTinSoTG = null;
                bool ret = false;

                DatabaseConstant.Action action = DatabaseConstant.Action.SUA;

                if (function == DatabaseConstant.Function.HDV_SO_TKQD)
                    ret = processHDV.MoSoTietKiemQuyDinh(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);

                else if (function == DatabaseConstant.Function.HDV_SO_TKKKH)
                    ret = processHDV.MoSoTietKiemKhongKyHan(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);

                else if (function == DatabaseConstant.Function.HDV_SO_TKCKH)
                    ret = processHDV.MoSoTietKiemCoKyHan(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);

                else if (function == DatabaseConstant.Function.HDV_SO_TK_TGTT)
                    ret = processHDV.MoTaiKhoanTienGuiThanhToan(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);

                else if (function == DatabaseConstant.Function.HDV_SO_TGCKH)
                    ret = processHDV.MoSoTienGuiCoKyHan(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref lstBLTienGuiKHoach, ref objThongTinSoTG, ref listClientResponseDetail);

                AfterModify(ret, objBLTienGui, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void AfterModify(bool ret, BL_TIEN_GUI objBLTienGui, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = objBLTienGui.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    txtSoSoTG.Text = objBLTienGui.SO_SO_TG;

                    SetEnabledAllControls(false);
                    SetEnabledRequiredControls(false);
                    action = DatabaseConstant.Action.XEM;
                    CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
                }
                else
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    //LMessage.ShowMessage("M.DungChung.CapNhatKoThanhCong", LMessage.MessageBoxType.Error);
                }

                // Yêu cầu Unlock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    function,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.SUA,
                    listLockId);
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


        private void BeforeDelete()
        {
            try
            {
                // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    Mouse.OverrideCursor = Cursors.Wait;

                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        function,
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
                    function,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Wait;
            }
        }

        private void OnDelete()
        {
            try
            {
                bool ret = false;
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                BL_TIEN_GUI objBLTienGui = new BL_TIEN_GUI();
                List<BL_TIEN_GUI_DCSH> lstBLTienGuiDCSH = new List<BL_TIEN_GUI_DCSH>();
                List<BL_TIEN_GUI_KHOACH> lstBLTienGuiKHoach = new List<BL_TIEN_GUI_KHOACH>();
                HDV_THONG_TIN_SO_TGUI objThongTinSoTG = null;

                objBLTienGui.ID = id;

                if (function == DatabaseConstant.Function.HDV_SO_TKQD)
                {
                    ret = processHDV.MoSoTietKiemQuyDinh(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);
                }
                else if (function == DatabaseConstant.Function.HDV_SO_TKKKH)
                {
                    ret = processHDV.MoSoTietKiemKhongKyHan(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);
                }
                else if (function == DatabaseConstant.Function.HDV_SO_TKCKH)
                {
                    ret = processHDV.MoSoTietKiemCoKyHan(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);
                }
                else if (function == DatabaseConstant.Function.HDV_SO_TK_TGTT)
                {
                    ret = processHDV.MoTaiKhoanTienGuiThanhToan(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);
                }
                else if (function == DatabaseConstant.Function.HDV_SO_TGCKH)
                {
                    ret = processHDV.MoSoTienGuiCoKyHan(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref lstBLTienGuiKHoach, ref objThongTinSoTG, ref listClientResponseDetail);
                }

                AfterDelete(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void AfterDelete(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
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
                    function,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                // Đóng cửa sổ chi tiết ret khi xóa
                if (ret) OnClose();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        private void BeforeApprove()
        {
            try
            {
                // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    Mouse.OverrideCursor = Cursors.Wait;

                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        function,
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
                    function,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.DUYET,
                    listLockId);

                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void OnApprove()
        {
            try
            {
                bool ret = false;
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                BL_TIEN_GUI objBLTienGui = new BL_TIEN_GUI();
                List<BL_TIEN_GUI_DCSH> lstBLTienGuiDCSH = new List<BL_TIEN_GUI_DCSH>();
                List<BL_TIEN_GUI_KHOACH> lstBLTienGuiKHoach = new List<BL_TIEN_GUI_KHOACH>();
                HDV_THONG_TIN_SO_TGUI objThongTinSoTG = null;

                objBLTienGui.ID = id;

                if (function == DatabaseConstant.Function.HDV_SO_TKQD)
                {
                    ret = processHDV.MoSoTietKiemQuyDinh(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);
                }
                else if (function == DatabaseConstant.Function.HDV_SO_TKKKH)
                {
                    ret = processHDV.MoSoTietKiemKhongKyHan(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);
                }
                else if (function == DatabaseConstant.Function.HDV_SO_TKCKH)
                {
                    ret = processHDV.MoSoTietKiemCoKyHan(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);
                }
                else if (function == DatabaseConstant.Function.HDV_SO_TK_TGTT)
                {
                    ret = processHDV.MoTaiKhoanTienGuiThanhToan(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);
                }
                else if (function == DatabaseConstant.Function.HDV_SO_TGCKH)
                {
                    ret = processHDV.MoSoTienGuiCoKyHan(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref lstBLTienGuiKHoach, ref objThongTinSoTG, ref listClientResponseDetail);
                }

                AfterApprove(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void AfterApprove(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    SetEnabledAllControls(false);
                    SetEnabledRequiredControls(false);
                    action = DatabaseConstant.Action.XEM;
                    CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
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
                    function,
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


        private void BeforeCancel()
        {
            try
            {
                // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    Mouse.OverrideCursor = Cursors.Wait;

                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        function,
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
                    function,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listLockId);

                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void OnCancel()
        {
            try
            {
                bool ret = false;
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                BL_TIEN_GUI objBLTienGui = new BL_TIEN_GUI();
                List<BL_TIEN_GUI_DCSH> lstBLTienGuiDCSH = new List<BL_TIEN_GUI_DCSH>();
                List<BL_TIEN_GUI_KHOACH> lstBLTienGuiKHoach = new List<BL_TIEN_GUI_KHOACH>();
                HDV_THONG_TIN_SO_TGUI objThongTinSoTG = null;

                objBLTienGui.ID = id;

                if (function == DatabaseConstant.Function.HDV_SO_TKQD)
                {
                    ret = processHDV.MoSoTietKiemQuyDinh(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);
                }
                else if (function == DatabaseConstant.Function.HDV_SO_TKKKH)
                {
                    ret = processHDV.MoSoTietKiemKhongKyHan(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);
                }
                else if (function == DatabaseConstant.Function.HDV_SO_TKCKH)
                {
                    ret = processHDV.MoSoTietKiemCoKyHan(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);
                }
                else if (function == DatabaseConstant.Function.HDV_SO_TK_TGTT)
                {
                    ret = processHDV.MoTaiKhoanTienGuiThanhToan(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);
                }
                else if (function == DatabaseConstant.Function.HDV_SO_TGCKH)
                {
                    ret = processHDV.MoSoTienGuiCoKyHan(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref lstBLTienGuiKHoach, ref objThongTinSoTG, ref listClientResponseDetail);
                }

                AfterCancel(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void AfterCancel(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    SetEnabledAllControls(false);
                    SetEnabledRequiredControls(false);
                    action = DatabaseConstant.Action.XEM;
                    CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
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
                    function,
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


        private void BeforeRefuse()
        {
            try
            {
                // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    Mouse.OverrideCursor = Cursors.Wait;

                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        function,
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
                    function,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);

                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void OnRefuse()
        {
            try
            {
                bool ret = false;
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                BL_TIEN_GUI objBLTienGui = new BL_TIEN_GUI();
                List<BL_TIEN_GUI_DCSH> lstBLTienGuiDCSH = new List<BL_TIEN_GUI_DCSH>();
                List<BL_TIEN_GUI_KHOACH> lstBLTienGuiKHoach = new List<BL_TIEN_GUI_KHOACH>();
                HDV_THONG_TIN_SO_TGUI objThongTinSoTG = null;

                objBLTienGui.ID = id;

                if (function == DatabaseConstant.Function.HDV_SO_TKQD)
                {
                    ret = processHDV.MoSoTietKiemQuyDinh(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);
                }
                else if (function == DatabaseConstant.Function.HDV_SO_TKKKH)
                {
                    ret = processHDV.MoSoTietKiemKhongKyHan(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);
                }
                else if (function == DatabaseConstant.Function.HDV_SO_TKCKH)
                {
                    ret = processHDV.MoSoTietKiemCoKyHan(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);
                }
                else if (function == DatabaseConstant.Function.HDV_SO_TK_TGTT)
                {
                    ret = processHDV.MoTaiKhoanTienGuiThanhToan(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref objThongTinSoTG, ref listClientResponseDetail);
                }
                else if (function == DatabaseConstant.Function.HDV_SO_TGCKH)
                {
                    ret = processHDV.MoSoTienGuiCoKyHan(action, ref objBLTienGui, ref lstBLTienGuiDCSH, ref lstBLTienGuiKHoach, ref objThongTinSoTG, ref listClientResponseDetail);
                }

                AfterRefuse(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void AfterRefuse(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    SetEnabledAllControls(false);
                    SetEnabledRequiredControls(false);
                    action = DatabaseConstant.Action.XEM;
                    CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
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
                    function,
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
