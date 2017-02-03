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
using Utilities.Common;
using PresentationWPF.BaoCao.DungChung;
using PresentationWPF.CustomControl;
using Presentation.Process.Common;

namespace PresentationWPF.BaoCao.BHTH
{
    /// <summary>
    /// Interaction logic for ucBaoVeVonVay.xaml
    /// </summary>
    public partial class ucDSachBaoVeVonVay : UserControl
    {
        // Các tham số báo cáo từ form điều kiện
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();

        public string MaBaoCao;
        public string MaChiNhanh;
        public string MaPhongGiaoDich;
        public string NgayBaoCao;
        public string MaNguoiLap;
        public string TenNguoiLap;
        public string MaCum;
        public string TuNgay;
        public string DenNgay;
        public string MaLoaiTien;
        public string LoaiTien;

        public string MaNgonNgu;
        public string MaDinhDang;

        public ucDSachBaoVeVonVay()
        {
            InitializeComponent();
            raddtTuNgayChotSoLieu.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            raddtDenNgayChotSoLieu.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            KhoiTaoComboBox();
        }

        private void KhoiTaoComboBox()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            // Khởi tạo điều kiện gọi danh mục
            List<string> lstDieuKien = new List<string>();
            //LoadCombobox(); ???
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
            // khởi tạo combobox
            lstSourceChiNhanh.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);

            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue());
            auto.GenAutoComboBox(ref lstSourceCum, ref cmbCum, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM_ALL.getValue());
            LoadComboboxPhongGD();
            LoadComboboxCum();

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DONG_TIEN_CAC_NUOC.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.PDF.layGiaTri());
        }

        private void LoadComboboxPhongGD()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();

            lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();
            lstSourcePhongGD_Select.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
            // khởi tạo combobox
            auto = new AutoComboBox();
            cmbPhongGD.Items.Clear();
            auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
            cmbPhongGD.SelectedIndex = 0;
            cmbPhongGD.IsEnabled = !maChiNhanh.Equals("%");
        }

        private void LoadComboboxCum()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();

            lstSourceCum_Select = new List<AutoCompleteEntry>();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            string maPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
            if (maPhongGiaoDich == "%")
                maPhongGiaoDich = maChiNhanh;
            lstSourceCum_Select = lstSourceCum.Where(e => e.KeywordStrings.ElementAt(1).Substring(0, maPhongGiaoDich.Length).Equals(maPhongGiaoDich)).ToList();
            lstSourceCum_Select.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
            // khởi tạo combobox
            auto = new AutoComboBox();
            cmbCum.Items.Clear();
            auto.GenAutoComboBox(ref lstSourceCum_Select, ref cmbCum, null);
            cmbCum.SelectedIndex = 0;
            cmbCum.SelectedIndex = 0;
            cmbCum.IsEnabled = !maPhongGiaoDich.Equals("%");
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPhongGD.SelectedIndex >= 0)
                LoadComboboxCum();
        }

        private void GetFormData()
        {
            // Lấy dữ liệu từ form            
            TuNgay = LDateTime.DateToString(raddtTuNgayChotSoLieu.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
            DenNgay = LDateTime.DateToString(raddtDenNgayChotSoLieu.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
            NgayBaoCao = LDateTime.DateToString(raddtNgayBaoCao.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);

            AutoCompleteEntry auChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex);
            if (!LObject.IsNullOrEmpty(auChiNhanh))
                MaChiNhanh = auChiNhanh.KeywordStrings.FirstOrDefault();
            else
                MaChiNhanh = "";

            AutoCompleteEntry auPhongGD = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex);
            if (!LObject.IsNullOrEmpty(auPhongGD))
                MaPhongGiaoDich = auPhongGD.KeywordStrings.FirstOrDefault();
            else
                MaPhongGiaoDich = "";

            AutoCompleteEntry auMaCum = lstSourceCum_Select.ElementAt(cmbCum.SelectedIndex);
            if (!LObject.IsNullOrEmpty(auMaCum))
                MaCum = auMaCum.KeywordStrings.FirstOrDefault();
            else
                MaCum = "";

            AutoCompleteEntry auLoaiTien = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex);
            if (!LObject.IsNullOrEmpty(auLoaiTien))
                LoaiTien = auLoaiTien.KeywordStrings.FirstOrDefault();
            else
                LoaiTien = "";

            AutoCompleteEntry auNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex);
            if (!LObject.IsNullOrEmpty(auNgonNgu))
                MaNgonNgu = auNgonNgu.KeywordStrings.FirstOrDefault();
            else
                MaNgonNgu = "";

            AutoCompleteEntry auDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex);
            if (!LObject.IsNullOrEmpty(auDinhDang))
                MaDinhDang = auNgonNgu.KeywordStrings.FirstOrDefault();
            else
                MaDinhDang = "";
            // Gán dữ liệu từ form vào các biến truyền cho báo cáo
        }

        private bool Validation()
        {
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
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayPhatVon", DenNgay, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@P_MaDonVi", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.GUIPARAM.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGiaoDich", MaPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaCum", MaCum, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaGiaoDich", "%", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TuNgay", TuNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DenNgay", DenNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaLoaiTien", MaLoaiTien, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            return listThamSoBaoCao;
        }
    }
}
