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
using Presentation.Process;
using Utilities.Common;
using Presentation.Process.Common;
using PresentationWPF.BaoCao.DungChung;

namespace PresentationWPF.BaoCao._BIDV.GDKT
{
    /// <summary>
    /// Interaction logic for GDKT_BANG_CAN_DOI_KE_TOAN.xaml
    /// </summary>
    public partial class GDKT_BALANCE_SHEET : UserControl
    {
          #region Khai bao

        ListCheckBoxCombo lstSourceChiNhanh = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourcePhongGD = new ListCheckBoxCombo();  
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();

        // Các tham số báo cáo từ form điều kiện
        public string MaBaoCao;
        public List<string> MaChiNhanh;
        public string TenChiNhanh;
        public List<string> MaPhongGiaoDich;
        public string TenPhongGiaoDich;
        public string NgayDuLieu;
        public string NgayBaoCao;
        public string MaNguoiLap;
        public string TenNguoiLap;
        public string MaLoaiTien;
        public string NguonVon;
        public string HeThongTK;
        public string LoaiBaoCao;

        public string MaNgonNgu;
        public string MaDinhDang;

        #endregion

        public GDKT_BALANCE_SHEET()
        {
            InitializeComponent();
            LoadCombobox();
            // Nếu người dùng là đơn vị >> disable thông tin chi nhánh
            //if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()) ||
            //    ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()))
            //{
            //    cmbChiNhanh.IsEnabled = false;
            //}
            //else
            //{
            //    cmbChiNhanh.IsEnabled = true;
            //}
            raddtNgayDuLieu.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat).GetLastDateOfMonth();
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

            string ThamSoHThongTK = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_HE_THONG_TKHOAN_HTHI, ClientInformation.MaDonVi);
            if (ThamSoHThongTK.IsNullOrEmptyOrSpace())
                ThamSoHThongTK = "CHI_TIET";

            // khởi tạo combobox
            auto = new AutoComboBox();
            lstDieuKien.Add(ClientInformation.TenDangNhap);
            lstDieuKien.Add(ClientInformation.MaDonViQuanLy);
            new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANHLIST.getValue(), lstDieuKien);
            //// khởi tạo combobox
            //auto = new AutoComboBox();
            //auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue());
            //LoadComboboxPhongGD();
            LoadComboboxPhongGD();
            
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ClientInformation.NgonNgu);

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ReportInformation.DinhDang);
            cmbDinhDang.IsEnabled = false;
            //auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri());
            //cmbDinhDang.SelectedIndex = lstSourceDinhDang.FindIndex(e => e.KeywordStrings[0].Equals(ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri()));
        }

        private void LoadComboboxPhongGD()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            string maChiNhanh = "";
            lstSourceChiNhanh = cmbChiNhanh.ItemsSource as ListCheckBoxCombo;
            foreach (AutoCompleteCheckBox lstCN in lstSourceChiNhanh)
            {
                if (lstCN.CheckedMember)
                    maChiNhanh += "," + lstCN.ValueMember[1].ToString();
            }
            if (maChiNhanh.Length > 0)
                maChiNhanh = maChiNhanh.Substring(1, maChiNhanh.Length - 1);
            else
                maChiNhanh = "0";
            List<string> lstDieuKien = new List<string>();
            auto = new AutoComboBox();
            lstDieuKien.Add(maChiNhanh);
            lstDieuKien.Add(ClientInformation.TenDangNhap);
            lstDieuKien.Add(ClientInformation.MaDonViQuanLy);
            new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PGDLIST.getValue(), lstDieuKien);
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();
        }

        private void GetFormData()
        {
            string maNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            string maDonVi = string.Empty;

            DateTime ngayBaoCao = new DateTime();
            if (raddtNgayBaoCao.Value is DateTime)
                ngayBaoCao = (DateTime)raddtNgayBaoCao.Value;
            DateTime ngayDuLieu = new DateTime();
            if (raddtNgayDuLieu.Value is DateTime)
                ngayDuLieu = (DateTime)raddtNgayDuLieu.Value;
            string maDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();

            //Lấy giá trị
            MaChiNhanh = new List<string>();
            foreach (AutoCompleteCheckBox ChiNhanh in lstSourceChiNhanh.Where(e => e.CheckedMember == true))
            {
                MaChiNhanh.Add(ChiNhanh.ValueMember.FirstOrDefault());
                TenChiNhanh = ChiNhanh.DislayMember + " - ";
            }
            MaPhongGiaoDich = new List<string>();
            foreach (AutoCompleteCheckBox PhongGDich in lstSourcePhongGD.Where(e => e.CheckedMember == true))
            {
                MaPhongGiaoDich.Add(PhongGDich.ValueMember.FirstOrDefault());
                TenPhongGiaoDich = PhongGDich.DislayMember + " - ";
            }
            if (MaPhongGiaoDich.Contains("All"))
                TenPhongGiaoDich = "";

            NgayDuLieu = ngayDuLieu.ToString("yyyyMMdd");
            NgayBaoCao = ngayBaoCao.ToString("yyyyMMdd");
            MaNgonNgu = maNgonNgu;
            MaDinhDang = maDinhDang;
            MaNguoiLap = Presentation.Process.Common.ClientInformation.TenDangNhap;
            TenNguoiLap = Presentation.Process.Common.ClientInformation.HoTen;
        }

        private bool Validation()
        {
            if (lstSourceChiNhanh.Where(e => e.CheckedMember == true).ToList().Count < 1)
                return false;
            //if (raddtNgayDuLieu.Value > raddtNgayBaoCao.Value)
            //{
            //    LMessage.ShowMessage("M.ResponseMessage.BaoCao.TuNgayLonHonDenNgay", LMessage.MessageBoxType.Warning);
            //    raddtNgayDuLieu.Focus();
            //    return false;
            //}
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
            //listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenPGD", TenPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayDuLieu", NgayDuLieu, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            //listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayMoSo", TuNgay, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));

            foreach (string ChiNhanh in MaChiNhanh)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", ChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            foreach (string ChiNhanh in MaPhongGiaoDich)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGiaoDich", ChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            listThamSoBaoCao.Add(new ThamSoBaoCao("@TenChiNhanh", TenChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TenPhongGD", TenPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NamChot", NgayDuLieu, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DinhKy", "THANG", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            //listThamSoBaoCao.Add(new ThamSoBaoCao("@MaLoaiTien", MaLoaiTien, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("@P_MaDonVi", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.GUIPARAM.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            return listThamSoBaoCao;
        }

        private void cmbChiNhanh_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceChiNhanh = cmbChiNhanh.ItemsSource as ListCheckBoxCombo;
            LoadComboboxPhongGD();
        }

        private void cmbPhongGD_DropDownClosed(object sender, EventArgs e)
        {
            lstSourcePhongGD = cmbPhongGD.ItemsSource as ListCheckBoxCombo;
        }
    }
}
