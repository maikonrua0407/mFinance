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

namespace PresentationWPF.BaoCao.BCTH
{
    /// <summary>
    /// Interaction logic for BCTH_BC_VAN_HANH.xaml
    /// </summary>
    public partial class BCTH_BC_VAN_HANH_00 : UserControl
    {
        #region Khai bao

        ListCheckBoxCombo lstSourceChiNhanh = new ListCheckBoxCombo();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceMauBaoCao = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();

        // Các tham số báo cáo từ form điều kiện
        public string MaBaoCao;
        public List<string> MaChiNhanh;
        public string NgayBaoCao;
        public string TenNguoiLap;

        public string NgayChotSoLieu;
        public string MauBaoCao;

        public string MaNgonNgu;
        public string MaDinhDang;

        #endregion

        public BCTH_BC_VAN_HANH_00()
        {
            InitializeComponent();
            LoadCombobox();
            // Nếu người dùng là đơn vị >> disable thông tin chi nhánh
            if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()) ||
                ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()))
            {
                cmbMauBaoCao.SelectedIndex = 1;
                cmbMauBaoCao.IsEnabled = false;
                cmbChiNhanh.IsEnabled = false;
            }
            else
            {
                string mauBaoCao = lstSourceMauBaoCao.ElementAt(cmbMauBaoCao.SelectedIndex).KeywordStrings.First();
                if (mauBaoCao.Equals("DONVI"))
                {
                    cmbChiNhanh.SelectedIndex = 0;
                    cmbChiNhanh.IsEnabled = false;
                }
                else if (mauBaoCao.Equals("CHINHANH"))
                {
                    cmbChiNhanh.IsEnabled = true;
                }
            }
            cmbMauBaoCao.SelectionChanged += cmbMauBaoCao_SelectionChanged;
            cmbChiNhanh.DropDownClosed += new EventHandler(cmbChiNhanh_DropDownClosed);
            raddtNgayChotSoLieu.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat).GetFirstDateOfMonth();
        }

        void cmbChiNhanh_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceChiNhanh = cmbChiNhanh.ItemsSource as ListCheckBoxCombo;
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
            lstDieuKien.Add(ClientInformation.TenDangNhap);
            lstDieuKien.Add(ClientInformation.MaDonViQuanLy);
            new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANHLIST.getValue(), lstDieuKien);

            lstDieuKien = new List<string>();
            //lstDieuKien.Add(DatabaseConstant.DanhMuc.BCAO_TDUNG_TIEU_CHI_NHOM.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            lstSourceMauBaoCao.Insert(0, new AutoCompleteEntry("Tổng hợp toàn M7", new string[2] { "DONVI", "DONVI" }));
            lstSourceMauBaoCao.Insert(1, new AutoCompleteEntry("Tổng hợp chi nhánh", new string[2] { "CHINHANH", "CHINHANH" }));
            auto.GenAutoComboBox(ref lstSourceMauBaoCao, ref cmbMauBaoCao, "", lstDieuKien);

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
            cmbDinhDang.IsEnabled = false;
        }

        private void GetFormData()
        {
            string maNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            DateTime ngayBaoCao = new DateTime();
            if (raddtNgayBaoCao.Value is DateTime)
                ngayBaoCao = (DateTime)raddtNgayBaoCao.Value;
            DateTime ngayChotSoLieu = new DateTime();
            if (raddtNgayChotSoLieu.Value is DateTime)
                ngayChotSoLieu = (DateTime)raddtNgayChotSoLieu.Value;
            string mauBaoCao = lstSourceMauBaoCao.ElementAt(cmbMauBaoCao.SelectedIndex).KeywordStrings.First();
            string maDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();
            List<string> maChiNhanh = new List<string>();
            foreach (AutoCompleteCheckBox item in lstSourceChiNhanh)
            {
                if (item.CheckedMember)
                    maChiNhanh.Add(item.ValueMember.FirstOrDefault());
            }

            //Lấy giá trị
            MaChiNhanh = maChiNhanh;
            NgayBaoCao = ngayBaoCao.ToString("yyyyMMdd");
            MaNgonNgu = maNgonNgu;
            MaDinhDang = maDinhDang;
            TenNguoiLap = Presentation.Process.Common.ClientInformation.HoTen;
            MauBaoCao = mauBaoCao;
            NgayChotSoLieu = ngayChotSoLieu.ToString("yyyyMMdd");
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
            foreach (string maChiNhanh in MaChiNhanh)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", maChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MauBaoCao", MauBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TuNgay", NgayChotSoLieu.StringToDate(ApplicationConstant.defaultDateTimeFormat).GetFirstDateOfMonth(ApplicationConstant.defaultDateTimeFormat), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DenNgay", NgayChotSoLieu, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayCuoiTTruoc", NgayChotSoLieu.StringToDate(ApplicationConstant.defaultDateTimeFormat).AddMonths(-1).GetLastDateOfMonth(ApplicationConstant.defaultDateTimeFormat), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NguoiLap", TenNguoiLap, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            return listThamSoBaoCao;
        }

        private void cmbMauBaoCao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string mauBaoCao = lstSourceMauBaoCao.ElementAt(cmbMauBaoCao.SelectedIndex).KeywordStrings.First();
            if (mauBaoCao.Equals("DONVI"))
            {
                cmbChiNhanh.SelectedIndex = 0;
                cmbChiNhanh.IsEnabled = false;
            }
            else if (mauBaoCao.Equals("CHINHANH"))
            {
                cmbChiNhanh.IsEnabled = true;
            }
        }
    }
}
