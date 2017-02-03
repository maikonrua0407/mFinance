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
using Utilities.Common;
using Presentation.Process.Common;
using Presentation.Process.KhachHangServiceRef;
using Presentation.Process;
using System.Data;
using Presentation.Process.PopupServiceRef;
using PresentationWPF.BaoCao.DungChung;
namespace PresentationWPF.BaoCao._BIDV.TDVM
{
    /// <summary>
    /// Interaction logic for TDVM_PHIEU_KHAO_SAT_KHACH_HANG.xaml
    /// </summary>
    public partial class TDVM_PHIEU_KHAO_SAT_KHACH_HANG : UserControl
    {
        #region Khai bao bien
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        DataSet dsKhachHang = new DataSet();
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        int _idKhachHang = 0;

        public string makhachhang = "";
        public string idkhachhang = "0";
        public string sotienvay = "0";
        public string mahopdong = "";
        public string ngaybaocao = "";
        public string masanpham = "";

        public string tenkhachhang = "";
        public string gioitinh = "";
        public string ngaysinh = "";
        public string sohokhau = "";
        public string diachi = "";
        public string socmnd = "";
        public string ngaycap = "";
        public string noicap = "";
        public string nguoithuake = "";
        public string qhnguoithuake = "";
        public string sosohokhau = "";
        public string sTenMucDichVay = "";

        public string mangonngu = "";
        public string madinhdang = "";
        #endregion

        #region Khoi tao
        public TDVM_PHIEU_KHAO_SAT_KHACH_HANG()
        {
            InitializeComponent();
            KhoiTaoCombobox();
            raddtNgayBaoCao.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
        }
        public void KhoiTaoCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
            auto = new AutoComboBox();
            //auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());
            auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ClientInformation.NgonNgu);

            //khoi tao combobox dinh dang
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
            auto = new AutoComboBox();
            //auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri());
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ReportInformation.DinhDang);
            cmbDinhDang.IsEnabled = false;
        }
        #endregion

        #region Xu ly giao dien
        private void HienDanhMucKhachHang()
        {
            try
            {
                List<string> lstDieuKien = new List<string>();
                KhachHangProcess khProcess = new KhachHangProcess();
                string lstIDKH = "";
                if (!txtMaKhachHang.Text.IsNullOrEmptyOrSpace())
                    lstIDKH = "(" + _idKhachHang.ToString() + ")";
                else
                    lstIDKH = "(0)";
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add("NULL");
                lstDieuKien.Add("NULL");
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_KHACHHANG_BAOCAO", lstDieuKien);
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
                    _idKhachHang = Convert.ToInt32(dr["ID"]);
                    dsKhachHang = khProcess.getThongTinCoBanKHTheoID(_idKhachHang);
                    if (dsKhachHang != null && dsKhachHang.Tables[0].Rows.Count > 0)
                    {
                        txtMaKhachHang.Text = dsKhachHang.Tables[0].Rows[0]["MA_KHANG"].ToString();
                        txtTenKhachHang.Text = dsKhachHang.Tables[0].Rows[0]["TEN_KHANG"].ToString();
                        txtDiaChi.Text = dsKhachHang.Tables[0].Rows[0]["DIA_CHI"].ToString();
                        LayDSDonVayVon(_idKhachHang);
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void LayDSDonVayVon(int idKhachHang)
        {
            try
            {
                DataSet dsDonVayVon = new DataSet();
                TinDungProcess tdProcess = new TinDungProcess();
                dsDonVayVon = tdProcess.LayDSDonVayVonBIDV(idKhachHang);
                if (dsDonVayVon == null) return;
                //raddgrThongTinDon.DataContext = dsDonVayVon.Tables[0].DefaultView;                
                raddgrThongTinDon.ItemsSource = dsDonVayVon.Tables[0].DefaultView;
                raddgrThongTinDon.Rebind();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void btnKhachHang_Click(object sender, RoutedEventArgs e)
        {
            HienDanhMucKhachHang();
        }

        private void txtMaKhachHang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                HienDanhMucKhachHang();
            }
        }
        #endregion

        #region lay thong tin in bao cao
        private void GetValuesOnForm()
        {
            try
            {

                KhachHangProcess khProcess = new KhachHangProcess();
                dsKhachHang = khProcess.getThongTinKHTheoID(_idKhachHang);
                DataTable dtKhangHSo = null;
                if (chkInMauMoi.IsChecked == false)
                {
                    if (dsKhachHang != null && dsKhachHang.Tables.Count > 0)
                    {
                        dtKhangHSo = dsKhachHang.Tables[0];
                        tenkhachhang = dtKhangHSo.Rows[0]["ten_khang"].ToString();
                        diachi = dtKhangHSo.Rows[0]["dia_chi"].ToString();
                        gioitinh = dtKhangHSo.Rows[0]["DD_GIOI_TINH"].ToString();
                        ngaysinh = dtKhangHSo.Rows[0]["DD_NGAY_SINH"].ToString();
                        socmnd = dtKhangHSo.Rows[0]["DD_GTLQ_SO"].ToString();
                        ngaycap = dtKhangHSo.Rows[0]["DD_GTLQ_NGAY_CAP"].ToString();
                        noicap = dtKhangHSo.Rows[0]["DD_GTLQ_NOI_CAP"].ToString();
                        sosohokhau = dtKhangHSo.Rows[0]["SO_HO_KHAU"].ToString();
                        DataRowView drv = (DataRowView)raddgrThongTinDon.SelectedItem;
                        sTenMucDichVay = drv["TEN_DMUC"].ToString();
                    }
                    makhachhang = txtMaKhachHang.Text.Trim();
                    idkhachhang = _idKhachHang.ToString();
                    DataRowView dr = (DataRowView)raddgrThongTinDon.SelectedItem;
                    sotienvay = LNumber.ToDecimal(dr["SO_TIEN_VAY"]).ToString();
                    mahopdong = dr["MA_DXVVVM"].ToString();
                    masanpham = dr["MA_SAN_PHAM"].ToString();
                }
                else
                {
                    makhachhang = "";
                    idkhachhang = _idKhachHang.ToString();
                    sotienvay = "0";
                    mahopdong = "";
                    masanpham = "";
                    tenkhachhang = "";
                    diachi = "";
                    gioitinh = "";
                    ngaysinh = "";
                    socmnd = "";
                    ngaycap = "";
                    noicap = "";
                    sosohokhau = "";
                    sTenMucDichVay = "";
                }
                ngaybaocao = ClientInformation.NgayLamViecHienTai;
                mangonngu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.FirstOrDefault();
                madinhdang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.FirstOrDefault();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private bool Validation()
        {
            if (chkInMauMoi.IsChecked == false)
            {
                if (txtMaKhachHang.Text.Trim() == "") return false;
                if (raddgrThongTinDon.Items.Count == 0) return false;
                if (raddgrThongTinDon.SelectedItems.Count > 1) return false;
            }
            return true;
        }

        public List<ThamSoBaoCao> GetParameters()
        {
            if (!Validation()) return null;
            GetValuesOnForm();
            List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();
            lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@MaPhongGD", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@MaKhachHang", makhachhang, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@IdKhachHang", idkhachhang, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@SoTienVay", sotienvay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@MaHopDongTD", mahopdong, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@MaSanPhamTD", masanpham, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", mangonngu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", madinhdang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@MA_DON_V", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            lstThamSo.Add(new ThamSoBaoCao("P_TEN_KHACH_HANG", tenkhachhang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("P_TEN_THUONG_GOI", tenkhachhang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("P_GIOI_TINH", gioitinh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("P_NGAY_SINH", ngaysinh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("P_DIA_CHI", diachi, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("P_SO_CMND", socmnd, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("P_NGAY_CAP", ngaycap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("P_NOI_CAP", noicap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("P_SO_SO_HO_KHAU", sosohokhau, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("P_MUC_DICH_VAY_VON", sTenMucDichVay, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            return lstThamSo;
        }
        #endregion

    }
}
