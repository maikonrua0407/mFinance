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
using System.Data;
using Presentation.Process;
using Presentation.Process.Common;

namespace PresentationWPF.BaoCao._DC.GDKT
{
    /// <summary>
    /// Interaction logic for GDKT_SO_NHAT_KY_CHI.xaml
    /// </summary>
    public partial class GDKT_SO_NHAT_KY_CHI_TELLER : UserControl
    {
        #region Khai bao

        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNguonVon = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTeller = new List<AutoCompleteEntry>();

        // Các tham số báo cáo từ form điều kiện
        public string MaBaoCao;
        public string MaChiNhanh;
        public string TenChiNhanh;
        public string MaPhongGiaoDich;
        public string TenPhongGiaoDich;
        public string NgayBaoCao;
        public string MaNguoiLap;
        public string TenNguoiLap;
        public string MaLoaiTien;

        public string SoTaiKhoan;
        public string MaNgonNgu;
        public string MaDinhDang;

        public string TuNgay;
        public string DenNgay;
        public string NguonVon;
        public string MaTeller;
        #endregion

        #region Khoi tao
        public GDKT_SO_NHAT_KY_CHI_TELLER()
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
            raddtTuNgay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat).GetFirstDateOfMonth();
            raddtDenNgay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat).GetLastDateOfMonth();
            cmbChiNhanh.SelectionChanged += new SelectionChangedEventHandler(cmbChiNhanh_SelectionChanged);
            cmbPhongGD.SelectionChanged += new SelectionChangedEventHandler(cmbPhongGD_SelectionChanged);
            cmbNguonVon.SelectionChanged += new SelectionChangedEventHandler(cmbNguonVon_SelectionChanged);
            GetTaiKhoan();
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

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DONG_TIEN_CAC_NUOC.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ClientInformation.MaDongNoiTe);

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ClientInformation.NgonNgu);

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            //auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri());
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ReportInformation.DinhDang);
            cmbDinhDang.IsEnabled = false;

            lstSourceNguonVon.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceNguonVon, ref cmbNguonVon, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NGUON_VON.getValue(), null);
        }

        private void LoadComboboxPhongGD()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();

            lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();
            lstSourcePhongGD_Select.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), "%", ""));

            // khởi tạo combobox
            auto = new AutoComboBox();
            cmbPhongGD.Items.Clear();
            auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
            cmbPhongGD.SelectedIndex = 0;
            cmbPhongGD.IsEnabled = !maChiNhanh.Equals("%");
            LoadComboboxNhanSu();
        }

        private void LoadComboboxNhanSu()
        {
            string maDonVi = string.Empty;
            if (cmbPhongGD.SelectedIndex >= 0 && !lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First().IsNullOrEmptyOrSpace())
                maDonVi = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
            else
                maDonVi = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            if (maDonVi.Equals("%"))
                maDonVi = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            string maDonViQLy = string.Empty;
            maDonViQLy = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            DateTime tuNgay = new DateTime();
            if (raddtTuNgay.Value is DateTime)
                tuNgay = (DateTime)raddtTuNgay.Value;
            DateTime denNgay = new DateTime();
            if (raddtDenNgay.Value is DateTime)
                denNgay = (DateTime)raddtDenNgay.Value;
            AutoComboBox au = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(maDonVi);
            lstDieuKien.Add(tuNgay.ToString("yyyyMMdd"));
            lstDieuKien.Add(denNgay.ToString("yyyyMMdd"));
            lstSourceTeller.Clear();
            cmbTeller.Items.Clear();
            au.GenAutoComboBox(ref lstSourceTeller, ref cmbTeller, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DS_NSD.getValue(), lstDieuKien);
        }
        #endregion

        #region Xu ly
        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();
            GetTaiKhoan();
        }

        private void GetTaiKhoan()
        {
            string maDonVi = string.Empty;
            if (cmbPhongGD.SelectedIndex >= 0 && !lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First().IsNullOrEmptyOrSpace())
                maDonVi = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
            else
                maDonVi = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            string maDonViQLy = string.Empty;
            maDonViQLy = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            string nguonVon = string.Empty;
            nguonVon = lstSourceNguonVon.ElementAt(cmbNguonVon.SelectedIndex).KeywordStrings.First();
            DataSet ds = new BaoCaoProcess().GetTaiKhoanTienMatTheoDonVi(maDonViQLy, maDonVi, nguonVon);
            if (ds != null && ds.Tables.Count > 0)
                grSoTienGuiDS.ItemsSource = ds.Tables["TKHOAN_TIENMAT_THEO_NGUON"];
            else
                grSoTienGuiDS.ItemsSource = null;
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxNhanSu();
            GetTaiKhoan();
        }

        private void GetFormData()
        {
            string maNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            string maDonVi = string.Empty;
            string tenPhongGiaoDich = string.Empty;
            if (lstSourcePhongGD_Select.Count > 0)
            {
                maDonVi = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
                tenPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).DisplayName;
            }
            else
                maDonVi = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            DateTime ngayBaoCao = new DateTime();
            if (raddtNgayBaoCao.Value is DateTime)
                ngayBaoCao = (DateTime)raddtNgayBaoCao.Value;
            DateTime tuNgay = new DateTime();
            if (raddtTuNgay.Value is DateTime)
                tuNgay = (DateTime)raddtTuNgay.Value;
            DateTime denNgay = new DateTime();
            if (raddtDenNgay.Value is DateTime)
                denNgay = (DateTime)raddtDenNgay.Value;
            string maLoaiTien = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.First();

            List<string> lstDanhSach = new List<string>();
            string soTaiKhoan = "";
            if (grSoTienGuiDS.SelectedItems.Count > 0)
            {
                for (int i = 0; i < grSoTienGuiDS.SelectedItems.Count; i++)
                {
                    DataRow dr = (DataRow)grSoTienGuiDS.SelectedItems[i];
                    lstDanhSach.Add(dr["SO_TAI_KHOAN"].ToString());
                    soTaiKhoan = soTaiKhoan + dr["SO_TAI_KHOAN"].ToString() + "#";
                }
            }
            if (soTaiKhoan != "")
            {
                soTaiKhoan = soTaiKhoan.Substring(0, soTaiKhoan.Length - 1);
            }
            string maDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            string tenChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).DisplayName;
            string nguonVon = lstSourceNguonVon.ElementAt(cmbNguonVon.SelectedIndex).KeywordStrings.First();
            string maTeller = lstSourceTeller.ElementAt(cmbTeller.SelectedIndex).KeywordStrings.First();
            //Lấy giá trị
            MaChiNhanh = maChiNhanh;
            TenChiNhanh = tenChiNhanh;
            MaPhongGiaoDich = maDonVi;
            TenPhongGiaoDich = tenPhongGiaoDich;
            TuNgay = tuNgay.ToString("yyyyMMdd");
            DenNgay = denNgay.ToString("yyyyMMdd");
            NgayBaoCao = ngayBaoCao.ToString("yyyyMMdd");
            MaLoaiTien = maLoaiTien;
            MaNgonNgu = maNgonNgu;
            MaDinhDang = maDinhDang;
            MaNguoiLap = Presentation.Process.Common.ClientInformation.TenDangNhap;
            TenNguoiLap = Presentation.Process.Common.ClientInformation.HoTen;
            SoTaiKhoan = soTaiKhoan;
            NguonVon = nguonVon;
            MaTeller = maTeller;
        }

        private bool Validation()
        {
            if (grSoTienGuiDS.SelectedItems.Count < 1)
                return false;
            return true;
        }

        public List<ThamSoBaoCao> GetParameters()
        {
            if (!Validation())
            {
                LMessage.ShowMessage("Thiếu tham số cho báo cáo.", LMessage.MessageBoxType.Information);
                return null;
            }

            GetFormData();

            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
            //listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenNguoiLap", TenNguoiLap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenChiNhanh", TenChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenPGD", TenPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TuNgay", TuNgay, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_DenNgay", DenNgay, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayMoSo", TuNgay, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NguoiGhiSo", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_SoTaiKhoan", SoTaiKhoan, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGiaoDich", MaPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TuNgay", TuNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DenNgay", DenNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@Teller", MaTeller, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            //listThamSoBaoCao.Add(new ThamSoBaoCao("@MaLoaiTien", MaLoaiTien, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@LoaiTK", "TKTH", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@SoTaiKhoan", SoTaiKhoan, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NguonVon", NguonVon, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@P_MaDonVi", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.GUIPARAM.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            return listThamSoBaoCao;
        }

        private void cmbNguonVon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetTaiKhoan();
        }
        #endregion
    }
}
