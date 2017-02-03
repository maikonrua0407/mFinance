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

namespace PresentationWPF.BaoCao.GDKT
{
    /// <summary>
    /// Interaction logic for ucTKNgoaiBangCanDoiKT.xaml
    /// </summary>
    public partial class GDKT_SO_TIEN_GUI_NGAN_HANG : UserControl
    {
        #region Khai bao

        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();

        // Các tham số báo cáo từ form điều kiện
        public string MaBaoCao;
        public string MaChiNhanh;
        public string TenChiNhanh;
        public string MaPhongGiaoDich;
        public string NgayBaoCao;
        public string MaNguoiLap;
        public string TenNguoiLap;

        public string TuNgay;
        public string DenNgay;

        public string MaNgonNgu;
        public string MaDinhDang;

        #endregion

        public GDKT_SO_TIEN_GUI_NGAN_HANG()
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

            // khởi tạo combobox
            auto = new AutoComboBox();
            cmbPhongGD.Items.Clear();
            auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
            cmbPhongGD.SelectedIndex = 0;
            cmbPhongGD.IsEnabled = !maChiNhanh.Equals("%");
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

            string maDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            string tenChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).DisplayName;

            //Lấy giá trị
            MaChiNhanh = maChiNhanh;
            TenChiNhanh = tenChiNhanh;
            MaPhongGiaoDich = maDonVi;
            TuNgay = tuNgay.ToString("yyyyMMdd");
            DenNgay = denNgay.ToString("yyyyMMdd");
            NgayBaoCao = ngayBaoCao.ToString("yyyyMMdd");
            MaNgonNgu = maNgonNgu;
            MaDinhDang = maDinhDang;
            MaNguoiLap = Presentation.Process.Common.ClientInformation.TenDangNhap;
            TenNguoiLap = Presentation.Process.Common.ClientInformation.HoTen;
        }

        private bool Validation()
        {
            return grTaiKhoanTienGuiNH.SelectedItems.Count > 0;
        }

        public List<ThamSoBaoCao> GetParameters()
        {
            if (!Validation())
            {
                LMessage.ShowMessage("Chưa chọn tài khoản.", LMessage.MessageBoxType.Information);
                return null;
            }

            GetFormData();
	
            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
            //listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenNguoiLap", TenNguoiLap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            if (MaChiNhanh.Equals("%"))
            {
                foreach (AutoCompleteEntry item in lstSourceChiNhanh)
                {
                    string ma = item.KeywordStrings.First();
                    if (!ma.Equals("%"))
                        listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", ma, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                }
            }
            else
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            if (MaPhongGiaoDich.Equals("%"))
            {
                foreach (AutoCompleteEntry item in lstSourcePhongGD_Select)
                {
                    string ma = item.KeywordStrings.First();
                    if (!ma.Equals("%"))
                        listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGD", ma, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                }
            }
            else
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGD", MaPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            if (grTaiKhoanTienGuiNH.SelectedItems.Count > 0)
            {
                for (int i = 0; i < grTaiKhoanTienGuiNH.SelectedItems.Count; i++)
                {
                    DataRow dr = (DataRow)grTaiKhoanTienGuiNH.SelectedItems[i];
                    listThamSoBaoCao.Add(new ThamSoBaoCao("@SoTaiKhoan", dr["SO_TAI_KHOAN"].ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                }
            }
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TuNgay", TuNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DenNgay", DenNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NguoiGhiSo", TenNguoiLap, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            return listThamSoBaoCao;
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetTaiKhoan();
        }

        private void GetTaiKhoan()
        {
            if (cmbPhongGD.SelectedIndex >= 0)
            {
                AutoCompleteEntry auPGD = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex);
                AutoCompleteEntry auChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex);
                string maDonVi = string.Empty;
                if (!auPGD.KeywordStrings.First().Equals("%"))
                    maDonVi = auPGD.KeywordStrings.First();
                else
                    maDonVi = auChiNhanh.KeywordStrings.First();
                DataSet ds = new BaoCaoProcess().GetTaiKhoanTienGuiNganHang(maDonVi);
                if (ds != null && ds.Tables.Count > 0)
                    grTaiKhoanTienGuiNH.ItemsSource = ds.Tables[0];
                else
                    grTaiKhoanTienGuiNH.ItemsSource = null;
            }
        }
    }
}
