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
    /// Interaction logic for TDTT_SO_VON_VAY_TIET_KIEM_KHTV.xaml
    /// </summary>
    public partial class TDTT_SO_VON_VAY_TIET_KIEM_KHTV : UserControl
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
        public string mahopdong = "";
        public string ngaybaocao = "";
        public string makheuoc = "";

        public string mangonngu = "";
        public string madinhdang = "";

        List<string> lstMaHD = null;
        List<string> lstMaKuoc = null;
        #endregion

        #region Khoi tao
        public TDTT_SO_VON_VAY_TIET_KIEM_KHTV()
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
                KhachHangProcess khProcess = new KhachHangProcess();
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
                    dsKhachHang = khProcess.getThongTinCoBanKHTheoID(_idKhachHang);
                    if (dsKhachHang != null && dsKhachHang.Tables[0].Rows.Count > 0)
                    {
                        txtMaKhachHang.Text = dsKhachHang.Tables[0].Rows[0]["MA_KHANG"].ToString();
                        txtTenKhachHang.Text = dsKhachHang.Tables[0].Rows[0]["TEN_KHANG"].ToString();
                        LayDanhSachKheUoc(_idKhachHang);
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void LayDanhSachKheUoc(int idKhachHang)
        {
            try
            {
                DataSet dsDonVayVon = new DataSet();
                BaoCaoProcess bcProcess = new BaoCaoProcess();
                dsDonVayVon = bcProcess.LayDSKUocTieuDungBIDV(idKhachHang);
                if (dsDonVayVon == null) return;
                //raddgrThongTinKUoc.DataContext = dsDonVayVon.Tables[0].DefaultView;                
                raddgrThongTinKUoc.ItemsSource = dsDonVayVon.Tables[0].DefaultView;
                raddgrThongTinKUoc.Rebind();
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
                lstMaHD = new List<string>();
                lstMaKuoc = new List<string>();
                makhachhang = txtMaKhachHang.Text.Trim();
                idkhachhang = _idKhachHang.ToString();
                DataRowView dr = (DataRowView)raddgrThongTinKUoc.SelectedItem;
                ngaybaocao = ClientInformation.NgayLamViecHienTai;
                mangonngu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.FirstOrDefault();
                madinhdang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.FirstOrDefault();

                foreach (DataRowView drv in raddgrThongTinKUoc.SelectedItems)
                {
                    lstMaHD.Add(drv["MA_HDTD"].ToString());
                }

                foreach (DataRowView drv in raddgrThongTinKUoc.SelectedItems)
                {
                    lstMaKuoc.Add(drv["MA_KUOC"].ToString());
                }
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
            lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", mangonngu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", madinhdang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));
            for (int i = 0; i < lstMaHD.Count; i++)
            {
                lstThamSo.Add(new ThamSoBaoCao("@MaHopDong", lstMaHD[i], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            for (int i = 0; i < lstMaKuoc.Count; i++)
            {
                lstThamSo.Add(new ThamSoBaoCao("@MaKheUoc", lstMaKuoc[i], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            return lstThamSo;
        }
        #endregion

    }
}
