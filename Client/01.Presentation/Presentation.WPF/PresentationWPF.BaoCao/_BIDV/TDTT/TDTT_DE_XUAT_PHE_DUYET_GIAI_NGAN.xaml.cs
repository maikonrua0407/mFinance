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
namespace PresentationWPF.BaoCao._BIDV.TDTT
{
    /// <summary>
    /// Interaction logic for TDTT_DE_XUAT_PHE_DUYET_GIAI_NGAN.xaml
    /// </summary>
    public partial class TDTT_DE_XUAT_PHE_DUYET_GIAI_NGAN : UserControl
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
        public string SoHDTD = "";
        public string SoKUOC = "";
        public string ngaybaocao = "";        

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
        public TDTT_DE_XUAT_PHE_DUYET_GIAI_NGAN()
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
                string lstIDKH = "";
                if (!txtMaKhachHang.Text.IsNullOrEmptyOrSpace())
                    lstIDKH = "(" + _idKhachHang.ToString() + ")";
                else
                    lstIDKH = "(0)";
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add(lstIDKH);

                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_KHACHHANG_TDTD", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
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
                    dsKhachHang = new KhachHangProcess().getThongTinCoBanKHTheoID(_idKhachHang);
                    if (dsKhachHang != null && dsKhachHang.Tables[0].Rows.Count > 0)
                    {
                        txtMaKhachHang.Text = dsKhachHang.Tables[0].Rows[0]["MA_KHANG"].ToString();
                        txtTenKhachHang.Text = dsKhachHang.Tables[0].Rows[0]["TEN_KHANG"].ToString();
                        txtDiaChi.Text = dsKhachHang.Tables[0].Rows[0]["DIA_CHI"].ToString();
                        LayDSHopDongKheUoc(_idKhachHang);
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void LayDSHopDongKheUoc(int idKhachHang)
        {
            try
            {
                DataSet dsHopDongKheUoc = new DataSet();
                TinDungTDProcess tdProcess = new TinDungTDProcess();
                dsHopDongKheUoc = tdProcess.getDanhSachKUOCTheoKH(idKhachHang);
                if (dsHopDongKheUoc == null) return;                         
                raddgrThongTinDon.ItemsSource = dsHopDongKheUoc.Tables[0].DefaultView;
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
                                        
                }
                makhachhang = txtMaKhachHang.Text.Trim();
                idkhachhang = _idKhachHang.ToString();                
                DataRowView dr = (DataRowView)raddgrThongTinDon.SelectedItem;                
                SoHDTD = dr["SO_HDTD"].ToString();
                SoKUOC = dr["SO_KUOC"].ToString();
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
            lstThamSo.Add(new ThamSoBaoCao("@SoHDTD", SoHDTD, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@SoKUOC", SoKUOC, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));            
            lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", mangonngu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", madinhdang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@MA_DON_VI", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

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
