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
using Presentation.Process.Common;
using System.Data;
using Presentation.Process;
using Presentation.Process.TruyVanServiceRef;
using Presentation.Process.PopupServiceRef;

namespace PresentationWPF.BaoCao.TDVM
{
    /// <summary>
    /// Interaction logic for TDVM_BANG_THOP_PHAT_VON_VAY_THEO_CN.xaml
    /// </summary>
    public partial class TDVM_BANG_THOP_PHAT_VON_VAY_THEO_CN : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        ListCheckBoxCombo lstSourcePhongGD = new ListCheckBoxCombo();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();       
        ListCheckBoxCombo lstSourceSanPhamTinDung = new ListCheckBoxCombo();
        List<AutoCompleteEntry> lstSourceSanPhamTinDung_Select = new List<AutoCompleteEntry>();
        DataTable dtSourceSanPhamTinDung = new DataTable();
        DataTable dtSourceSanPhamTinDung_Select = new DataTable();
        List<AutoCompleteEntry> lstSourceKhuVuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKhuVuc_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum_Select = new List<AutoCompleteEntry>();
        DataTable dtSourceCum = new DataTable();
        DataTable dtSourceCum_Select = new DataTable();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
       


        // Các tham số báo cáo từ form điều kiện
        public string MaBaoCao;
        public string MaDonVi;
        public string MaChiNhanh;
        public List<string> lstMaChiNhanh;
        public string TenChiNhanh;
        public string MaPhongGiaoDich;
        public List<string> lstMaPhongGiaoDich;
        public string TenPhongGiaoDich;
        public string MaCum;
        public List<string> lstMaCum;
        public string TenCum;
        public string MaKhuVuc;
        public List<string> lstMaKhuVuc;
        public string MaSanPhamTinDung;
        public List<string> lstMaSanPhamTinDung;
        public string TenSanPhamTinDung;
        public string TenKhuVuc;
        public string TuNgay;
        public string DenNgay;
        public string SoTienTu;
        public string SoTienDen;
        public string NgayBaoCao;
        public string MaNguoiLap;
        public string TenNguoiLap;
        public string MaNgonNgu;
        public string MaDinhDang;
        public bool IsLoadForm = false;

        public DatabaseConstant.Action GetAction() { return DatabaseConstant.Action.IN_CHUNG; }

        #endregion

        public TDVM_BANG_THOP_PHAT_VON_VAY_THEO_CN()
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

            raddtTuNgay.Value = LDateTime.GetFirstDateOfMonth(LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd"));
            raddtDenNgay.Value = LDateTime.GetLastDateOfMonth(LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd"));
            raddtNgayBaoCao.Value = DateTime.Now;
        }

        /// <summary>
        /// Khởi tạo các combobox
        /// </summary>
        private void LoadCombobox()
        {
            // Khai bao AutoComboBox
            AutoComboBox auto;
            // Khởi tạo điều kiện gọi danh mục
            List<string> lstDieuKien = new List<string>();

            //Tao combobox Chi nhanh,PGD
            auto = new AutoComboBox();
            //lstSourceChiNhanh.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);
            LoadComboboxPhongGD();

            //var process = new TruyVanProcess();
            //lstDieuKien = new List<string>();
            //lstDieuKien.Add("select MA_DVI from DM_DON_VI where TTHAI_NVU='DDU'");
            //DanhSachResponse response = process.getDanhSachInformation(DatabaseConstant.DanhSachTruyVan.COMBOBOX_SAN_PHAM_TD.getValue(), lstDieuKien);
            //if (!LObject.IsNullOrEmpty(response.DataSetSource))
            //{
            //    if (response.DataSetSource.Tables[0].Rows.Count > 0) dtSourceSanPhamTinDung = response.DataSetSource.Tables[0];

            //}

            LoadComboboxSanPhamTD();

            // khởi tạo combobox
            auto = new AutoComboBox();

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());
            cmbNgonNgu.IsEnabled = false;

            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri());
            cmbDinhDang.IsEnabled = false;

            cmbChiNhanh.SelectionChanged += new SelectionChangedEventHandler(cmbChiNhanh_SelectionChanged);
            //cmbPhongGD.SelectionChanged += new SelectionChangedEventHandler(cmbPhongGD_SelectionChanged);
            IsLoadForm = true;
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoadForm.Equals(true))
            {
                LoadComboboxPhongGD();
                LoadComboboxSanPhamTD();
            }
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (IsLoadForm.Equals(true)) LoadComboboxSanPhamTD();
        }

        private void LoadComboboxPhongGD()
        {
            if (cmbChiNhanh.SelectedIndex >= 0)
            {
                // khởi tạo combobox
                string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                string idChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings[1];

                // khởi tạo combobox
                lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
                lstSourcePhongGD = new ListCheckBoxCombo();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(idChiNhanh);
                lstDieuKien.Add(ClientInformation.TenDangNhap);
                lstDieuKien.Add(ClientInformation.MaDonViQuanLy);
                new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PGDLIST.getValue(), lstDieuKien);
            }
        }

        private void LoadComboboxSanPhamTD()
        {
            if (cmbChiNhanh.SelectedIndex >= 0)
            {
                ListCheckBoxCombo lstSourceSanPham = new ListCheckBoxCombo();
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();

                lstSourceSanPham = new ListCheckBoxCombo();
                string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                lstDieuKien.Add(""+maChiNhanh+"");
                lstDieuKien.Add("%");
                lstDieuKien.Add("1");
                lstDieuKien.Add("0");
                //cmbSanPham.Items.Clear();
                new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceSanPham, ref cmbSanPham, "COMBOBOX_SAN_PHAM_TD", lstDieuKien);
                //cmbSanPham.IsEnabled = !maChiNhanh.Equals("%");
            }
        }

        private void GetFormData()
        {
            string maChiNhanh = string.Empty;
            string tenChiNhanh = string.Empty;
            maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            tenChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).DisplayName;

            lstMaPhongGiaoDich = new List<string>();
            foreach (AutoCompleteCheckBox PhongGD in lstSourcePhongGD.Where(e => e.CheckedMember == true))
            {
                lstMaPhongGiaoDich.Add(PhongGD.ValueMember.FirstOrDefault());
            }

            lstMaSanPhamTinDung = new List<string>();
            lstSourceSanPhamTinDung = cmbSanPham.ItemsSource as ListCheckBoxCombo;
            foreach (AutoCompleteCheckBox SanPhamTD in lstSourceSanPhamTinDung.Where(e => e.CheckedMember == true))
            {
                lstMaSanPhamTinDung.Add(SanPhamTD.ValueMember.FirstOrDefault());
            }

            DateTime tuNgay = new DateTime();
            if (raddtTuNgay.Value is DateTime) tuNgay = (DateTime)raddtTuNgay.Value;

            DateTime denNgay = new DateTime();
            if (raddtDenNgay.Value is DateTime) denNgay = (DateTime)raddtDenNgay.Value;

            DateTime ngayBaoCao = new DateTime();
            if (raddtNgayBaoCao.Value is DateTime) ngayBaoCao = (DateTime)raddtNgayBaoCao.Value;

            string maNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            string maDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();

            // Gán dữ liệu từ form vào các biến truyền cho báo cáo
            MaChiNhanh = maChiNhanh;
            TenChiNhanh = tenChiNhanh;
            TuNgay = (raddtTuNgay.Value is DateTime) ? tuNgay.ToString("yyyyMM") + "01" : "";
            DenNgay = (raddtDenNgay.Value is DateTime) ? denNgay.ToString("yyyyMM") + LDateTime.GetLastDateOfMonth(denNgay).ToString("dd") : "";
            SoTienTu = (!LObject.IsNullOrEmpty(numSoTienTu.Value)) ? LNumber.StringToInt32(numSoTienTu.Value.ToString()).ToString() : "";
            SoTienDen = (!LObject.IsNullOrEmpty(numSoTienDen.Value)) ? LNumber.StringToInt32(numSoTienDen.Value.ToString()).ToString() : "";
            NgayBaoCao = (raddtNgayBaoCao.Value is DateTime) ? ngayBaoCao.ToString("yyyyMMdd") : "";
            MaNguoiLap = ClientInformation.TenDangNhap;
            TenNguoiLap = ClientInformation.HoTen;
            MaNgonNgu = maNgonNgu;
            MaDinhDang = maDinhDang;
        }

        private bool Validation(ref string msgResult)
        {
            bool bResult = true;

            if (MaChiNhanh.Equals("")) { bResult = false; msgResult = "Chọn chi nhánh"; cmbChiNhanh.Focus(); return bResult; }
            if (TuNgay.Equals("")) { bResult = false; msgResult = "Nhập từ ngày"; raddtTuNgay.Focus(); return bResult; }
            if (DenNgay.Equals("")) { bResult = false; msgResult = "Nhập đến ngày"; raddtDenNgay.Focus(); return bResult; }
            if (SoTienTu.Equals("")) { bResult = false; msgResult = "Nhập số tiền"; numSoTienTu.Focus(); return bResult; }
            if (SoTienDen.Equals("")) { bResult = false; msgResult = "Nhập số tiền"; numSoTienDen.Focus(); return bResult; }
            if (NgayBaoCao.Equals("")) { bResult = false; msgResult = "Nhập ngày báo cáo"; raddtNgayBaoCao.Focus(); return bResult; }
            if (LObject.IsNullOrEmpty(lstMaPhongGiaoDich) || lstMaPhongGiaoDich.Count<=0) { bResult = false; msgResult = "Chọn phòng giao dịch"; raddtNgayBaoCao.Focus(); return bResult; }
            if (LObject.IsNullOrEmpty(lstMaSanPhamTinDung) || lstMaSanPhamTinDung.Count <= 0) { bResult = false; msgResult = "Chọn sản phẩm tín dụng"; raddtNgayBaoCao.Focus(); return bResult; }
                         

            return bResult;
        }

        public List<ThamSoBaoCao> GetParameters()
        {
            string msgResult = "";

            GetFormData();

            if (!Validation(ref msgResult))
            {
                LMessage.ShowMessage(msgResult, LMessage.MessageBoxType.Information);
                return null;
            }

            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            foreach (string PhongGD in lstMaPhongGiaoDich)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGiaoDich", PhongGD, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            foreach (string SanPhamTD in lstMaSanPhamTinDung)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaSanPhamTinDung", SanPhamTD, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TuNgay", TuNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DenNgay", DenNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@SoTienTu", SoTienTu, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@SoTienDen", SoTienDen, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            return listThamSoBaoCao;
        }

    }
}
