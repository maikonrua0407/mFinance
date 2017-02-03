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

namespace PresentationWPF.BaoCao.KHTV
{
    /// <summary>
    /// Interaction logic for ucTangTruongKtKHTV.xaml
    /// </summary>
    public partial class KHTV_TANG_TRUONG_KT_KHTV : UserControl
    {
        #region Khai bao

        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();

        // Các tham số báo cáo từ form điều kiện
        public string MaBaoCao;
        public string MaChiNhanh;
        public string MaPhongGiaoDich;
        public string MaCum;
        public string NgayBaoCao;
        public string MaNguoiLap;
        public string TenNguoiLap;
        public string NgayDuLieu;

        public string MaNgonNgu;
        public string MaDinhDang;

        #endregion

        public KHTV_TANG_TRUONG_KT_KHTV()
        {
            InitializeComponent();
            LoadCombobox();
            raddtNgayDuLieu.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
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
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceCum, ref cmbCum, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM_ALL.getValue());
            LoadComboboxCum();

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
            string maChiNhanh = null;

            if (lstSourceChiNhanh.Count > 0)
            {
                if (cmbChiNhanh.Items.Count > 0)
                    maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                else
                    maChiNhanh = lstSourceChiNhanh.ElementAt(0).KeywordStrings.First();
                lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();

                // khởi tạo combobox
                auto = new AutoComboBox();
                cmbPhongGD.Items.Clear();
                auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
                cmbPhongGD.SelectedIndex = 0;
                cmbPhongGD.IsEnabled = !maChiNhanh.Equals("%");
            }
            else
            {
                lstSourcePhongGD_Select.Clear();
                cmbPhongGD.Items.Clear();
            }
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();
        }

        private void LoadComboboxCum()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();

            lstSourceCum_Select = new List<AutoCompleteEntry>();
            string maPhongGD = null;

            if (lstSourcePhongGD_Select.Count > 0)
            {
                if (cmbPhongGD.Items.Count > 0)
                    maPhongGD = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
                else
                    maPhongGD = lstSourcePhongGD_Select.ElementAt(0).KeywordStrings.First();

                lstSourceCum_Select = lstSourceCum.Where(e => e.KeywordStrings.ElementAt(1).Equals(maPhongGD)).ToList();

                // khởi tạo combobox
                auto = new AutoComboBox();
                cmbCum.Items.Clear();
                auto.GenAutoComboBox(ref lstSourceCum_Select, ref cmbCum, null);
                cmbCum.SelectedIndex = 0;
                cmbCum.IsEnabled = !maPhongGD.Equals("%");
            }
            else
            {
                lstSourceCum_Select.Clear();
                cmbCum.Items.Clear();
            }
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxCum();
        }

        private void GetFormData()
        {
            string maChiNhanh = string.Empty;
            maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();

            string maPhongGiaoDich = string.Empty;
            if (lstSourcePhongGD_Select.Count > 0)
                maPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
            else
                maPhongGiaoDich = string.Empty;

            DateTime ngayBaoCao = new DateTime();
            if (raddtNgayBaoCao.Value is DateTime)
                ngayBaoCao = (DateTime)raddtNgayBaoCao.Value;
            DateTime ngayDuLieu = new DateTime();
            if (raddtNgayDuLieu.Value is DateTime)
                ngayDuLieu = (DateTime)raddtNgayDuLieu.Value;

            string maCum = string.Empty;
            if (lstSourceCum_Select.Count > 0)
                maCum = lstSourceCum_Select.ElementAt(cmbCum.SelectedIndex).KeywordStrings.First();
            else
                maCum = string.Empty;

            string maNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            string maDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();

            //Lấy giá trị
            MaChiNhanh = maChiNhanh;
            MaPhongGiaoDich = maPhongGiaoDich;
            MaCum = maCum;
            NgayBaoCao = ngayBaoCao.ToString("yyyyMMdd");
            NgayDuLieu = ngayDuLieu.ToString("yyyyMMdd");
            MaNgonNgu = maNgonNgu;
            MaDinhDang = maDinhDang;
            MaNguoiLap = Presentation.Process.Common.ClientInformation.TenDangNhap;
            TenNguoiLap = Presentation.Process.Common.ClientInformation.HoTen;                
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
            if (MaChiNhanh.Equals("%"))
            {
                foreach (AutoCompleteEntry item in lstSourceChiNhanh)
                {
                    string ma = item.KeywordStrings.First();
                    if (!ma.Equals("%"))
                        listThamSoBaoCao.Add(new ThamSoBaoCao("@DSChiNhanh", ma, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                }
            }
            else
                listThamSoBaoCao.Add(new ThamSoBaoCao("@DSChiNhanh", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            if (MaPhongGiaoDich.Equals("%"))
            {
                foreach (AutoCompleteEntry item in lstSourcePhongGD_Select)
                {
                    string ma = item.KeywordStrings.First();
                    if (!ma.Equals("%"))
                        listThamSoBaoCao.Add(new ThamSoBaoCao("@DSPhongGD", ma, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                }
            }
            else
                listThamSoBaoCao.Add(new ThamSoBaoCao("@DSPhongGD", MaPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            if (MaCum.Equals("%"))
            {
                foreach (AutoCompleteEntry item in lstSourceCum_Select)
                {
                    string ma = item.KeywordStrings.First();
                    if (!ma.Equals("%"))
                        listThamSoBaoCao.Add(new ThamSoBaoCao("@DSCum", ma, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                }
            }
            else
                listThamSoBaoCao.Add(new ThamSoBaoCao("@DSCum", MaCum, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayInDuLieu", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayChotDuLieu", NgayDuLieu, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            return listThamSoBaoCao;
        }
    }
}
