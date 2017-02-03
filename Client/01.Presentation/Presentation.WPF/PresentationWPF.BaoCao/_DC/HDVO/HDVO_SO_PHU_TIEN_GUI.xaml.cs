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
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.Common;
using System.Data;

namespace PresentationWPF.BaoCao._DC.HDVO
{
    /// <summary>
    /// Interaction logic for HDVO_SO_PHU_TIEN_GUI.xaml
    /// </summary>
    public partial class HDVO_SO_PHU_TIEN_GUI : UserControl
    {
        #region Khai bao

        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiSoPhu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();

        // Các tham số báo cáo từ form điều kiện
        public string SoSoTG;

        public string MaBaoCao;
        public string MaChiNhanh;
        public string TenChiNhanh;
        public string MaPhongGiaoDich;
        public string TenPhongGiaoDich;
        public string MaDonVi;
        public string NgayBaoCao;
        public string MaNguoiLap;
        public string TenNguoiLap;
        public string MaLoaiBaoCao;
        public string MaKhachHang;
        public string TenKhachHang;
        public string DiaChi;
        public string SDT;
        public string SoCMND;
        public string NgayCap;
        public string NoiCap;

        public DateTime TuNgay;
        public DateTime DenNgay;

        public string MaNgonNgu;
        public string MaDinhDang;

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        private int idKhachHang = 0;
        #endregion

        public HDVO_SO_PHU_TIEN_GUI()
        {
            InitializeComponent();
            LoadCombobox();
            // Nếu người dùng là đơn vị >> disable thông tin chi nhánh
            if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()) ||
                ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()))
            {
                cmbChiNhanh.IsEnabled = false;
            }
            else
            {
                cmbChiNhanh.IsEnabled = true;
            }

            raddtTuNgay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            raddtDenNgay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            cmbDinhDang.IsEnabled = false;
        }

        /// <summary>
        /// Khởi tạo các combobox
        /// </summary>
        private void LoadCombobox()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            // Khởi tạo điều kiện gọi danh mục
            List<string> lstDieuKien = new List<string>();

            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);

            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue());
            LoadComboboxPhongGD();

            // khởi tạo combobox
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.LOAI_SO_PHU.getValue());
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceLoaiSoPhu, ref cmbLoaiBaoCao, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
            lstSourceLoaiSoPhu.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
            cmbLoaiBaoCao.Items.Clear();
            auto.GenAutoComboBox(ref lstSourceLoaiSoPhu, ref cmbLoaiBaoCao, null);
            cmbLoaiBaoCao.SelectedIndex = 0;

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri());
        }

        private void LoadComboboxPhongGD()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();

            lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();

            // khởi tạo combobox
            auto = new AutoComboBox();
            cmbPhongGD.Items.Clear();
            auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
            cmbPhongGD.SelectedIndex = 0;
            cmbPhongGD.IsEnabled = !maChiNhanh.Equals("%");
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();
            ResetThongTinKhachHang();
        }

        private void btnKhachHang_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                var process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First());
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_KHACHHANG_PGD.getValue(), lstDieuKien);

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                Mouse.OverrideCursor = Cursors.Arrow;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Title = LLanguage.SearchResourceByKey("U.POPUP.POPUP_DS_KHACHHANG");
                win.ShowDialog();
                if (lstPopup != null)
                {
                    DataRow row = lstPopup[0];
                    KhachHangProcess processKH = new KhachHangProcess();
                    DataRow dr = processKH.getThongTinCoBanKHTheoID(Convert.ToInt32(row[1])).Tables[0].Rows[0];

                    AfterGetKhachHang(dr);
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

        private void txtMaKhachHang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnKhachHang_Click(null, null);
            }
        }

        private void txtMaKhachHang_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtMaKhachHang.Text.IsNullOrEmptyOrSpace())
                {
                    KhachHangProcess processKH = new KhachHangProcess();
                    DataTable dt = processKH.getThongTinCoBanKHTheoMa(0, txtMaKhachHang.Text, Convert.ToInt32(ClientInformation.IdDonVi)).Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        //lblDCSH_ID.Content = dr["ID"].ToString();
                        AfterGetKhachHang(dr);
                    }
                }
                else
                {
                    ResetThongTinKhachHang();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);

            }
        }

        private void AfterGetKhachHang(DataRow row)
        {
            try
            {
                if (row != null)
                {
                    idKhachHang = Convert.ToInt32(row["ID"]);
                    txtMaKhachHang.Text = row["MA_KHANG"].ToString();
                    lblTenKH.Content = row["TEN_KHANG"].ToString();
                    txtDiaChi.Text = row["DIA_CHI"].ToString();
                    txtSDT.Text = row["SO_DTHOAI"].ToString();
                    if (row["DD_GTLQ_LOAI"].ToString().Equals("CHUNG_MINH_ND"))
                    {
                        txtCMND.Text = row["DD_GTLQ_SO"].ToString();
                        if (LDateTime.IsDate(row["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd"))
                            raddtNgayCap.Value = LDateTime.StringToDate(row["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd");
                        else
                            raddtNgayCap.Value = null;
                        txtNoiCap.Text = row["DD_GTLQ_NOI_CAP"].ToString();
                    }
                    GetSoPhu();
                }
                else
                {
                    ResetThongTinKhachHang();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void ResetThongTinKhachHang()
        {
            idKhachHang = 0;
            txtMaKhachHang.Text = "";
            lblTenKH.Content = LLanguage.SearchResourceByKey("U.BaoCao._BTV.HDVO.HDVO_SO_PHU_TIEN_GUI.TenKH");
            txtDiaChi.Text = "";
            txtCMND.Text = "";
            raddtNgayCap.Value = null;
            txtNoiCap.Text = "";
            txtSDT.Text = "";
            grSoTienGuiDS.ItemsSource = null;
        }

        private void GetSoPhu()
        {
            try
            {
                if (!txtMaKhachHang.Text.IsNullOrEmptyOrSpace())
                {
                    string loaiKyHan = string.Empty;
                    if (lstSourceLoaiSoPhu.ElementAt(cmbLoaiBaoCao.SelectedIndex).KeywordStrings.First().Equals(BusinessConstant.LOAI_SO_PHU.SO_PHU_TGUI_CO_KY_HAN.layGiaTri()))
                    {
                        loaiKyHan = "CO_KY_HAN";
                    }
                    else if (lstSourceLoaiSoPhu.ElementAt(cmbLoaiBaoCao.SelectedIndex).KeywordStrings.First().Equals(BusinessConstant.LOAI_SO_PHU.SO_PHU_TGUI_KHONG_KY_HAN.layGiaTri()))
                    {
                        loaiKyHan = "KHONG_KY_HAN";
                    }
                    DataSet ds = new BaoCaoProcess().GetSoPhuTienGui(idKhachHang, loaiKyHan);
                    if (ds != null && ds.Tables.Count > 0)
                        grSoTienGuiDS.ItemsSource = ds.Tables["SO_PHU_TGUI"];
                    else
                        grSoTienGuiDS.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void cmbLoaiBaoCao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetSoPhu();
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetThongTinKhachHang();
        }

        private void GetFormData()
        {
            List<string> lstDanhSach = new List<string>();
            if (grSoTienGuiDS.SelectedItems.Count > 0)
            {
                for (int i = 0; i < grSoTienGuiDS.SelectedItems.Count; i++)
                {
                    DataRow dr = (DataRow)grSoTienGuiDS.SelectedItems[i];
                    lstDanhSach.Add(dr["SO_SO_TG"].ToString());
                }
            }

            if (lstDanhSach != null)
            {
                SoSoTG = lstDanhSach[0];
                MaNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
                MaDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();
                TenNguoiLap = ClientInformation.HoTen;
                MaChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                TenChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).DisplayName;
                if (lstSourcePhongGD_Select.Count > 0)
                {
                    MaPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
                    TenPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).DisplayName;
                }
                MaDonVi = (MaPhongGiaoDich.IsNullOrEmpty() || MaPhongGiaoDich.Equals("%")) ? MaChiNhanh : MaPhongGiaoDich;
                TuNgay = Convert.ToDateTime(raddtTuNgay.Value);
                DenNgay = Convert.ToDateTime(raddtDenNgay.Value);
                MaKhachHang = txtMaKhachHang.Text;
                TenKhachHang = lblTenKH.Content.ToString();
                DiaChi = txtDiaChi.Text;
                SoCMND = txtCMND.Text;
                SDT = txtSDT.Text;
                if (raddtNgayCap.Value != null)
                    NgayCap = Convert.ToDateTime(raddtNgayCap.Value).ToString("dd/MM/yyyy");
                NoiCap = txtNoiCap.Text;
            }
        }

        private bool Validation()
        {
            if (raddtTuNgay.Value == null)
            {
                LMessage.ShowMessage("M.ResponseMessage.BaoCao.TuNgayKhongDuocTrong", LMessage.MessageBoxType.Warning);
                raddtTuNgay.Focus();
                return false;
            }
            if (raddtDenNgay.Value == null)
            {
                LMessage.ShowMessage("M.ResponseMessage.BaoCao.DenNgayKhongDuocTrong", LMessage.MessageBoxType.Warning);
                raddtDenNgay.Focus();
                return false;
            }
            if (raddtTuNgay.Value > raddtDenNgay.Value)
            {
                LMessage.ShowMessage("M.ResponseMessage.BaoCao.TuNgayLonHonDenNgay", LMessage.MessageBoxType.Warning);
                raddtTuNgay.Focus();
                return false;
            }
            if (grSoTienGuiDS.SelectedItems.Count == 0)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                return false;
            }
            else if (grSoTienGuiDS.SelectedItems.Count > 1)
            {
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
                return false;
            }
            return true;
        }

        public List<ThamSoBaoCao> GetParameters()
        {
            if (!Validation())
            {
                return null;
            }

            GetFormData();

            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TEN_CHI_NHANH", TenChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NGAY_BAO_CAO", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenNguoiLap", TenNguoiLap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenChiNhanh", TenChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenPGD", TenPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TuNgay", TuNgay.ToString("dd/MM/yyyy"), ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_DenNgay", DenNgay.ToString("dd/MM/yyyy"), ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_SoSoTG", SoSoTG, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_MaKhachHang", MaKhachHang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenKhachHang", TenKhachHang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_DiaChiKH", DiaChi, ApplicationConstant.LoaiThamSoBaoCao.GUISQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_Tel", SDT, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_SoCMND", SoCMND, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayCap", Convert.ToDateTime(raddtNgayCap.Value).ToString("dd/MM/yyyy"), ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NoiCap", NoiCap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@P_SoSo", SoSoTG, ApplicationConstant.LoaiThamSoBaoCao.GUIPARAM.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@SoSoTG", SoSoTG, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TuNgay", TuNgay.ToString("yyyyMMdd"), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DenNgay", DenNgay.ToString("yyyyMMdd"), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            return listThamSoBaoCao;
        }
    }
}
