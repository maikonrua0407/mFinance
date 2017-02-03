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
using System.Data;
using Presentation.Process.Common;
using Utilities.Common;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process;

namespace PresentationWPF.BaoCao.TDVM
{
    /// <summary>
    /// Interaction logic for TDVM_PHIEU_THAM_DINH_VON_VAY.xaml
    /// </summary>
    public partial class TDVM_PHIEU_THAM_DINH_VON_VAY : UserControl
    {
                
        #region Khai bao

        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNhom = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNhom_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        // Các tham số báo cáo từ form điều kiện
        public string MaBaoCao;
        public string MaDonVi;
        public string MaChiNhanh;
        public string TenChiNhanh;
        public string MaPhongGiaoDich;
        public string TenPhongGiaoDich;
        public string MaCum;
        public string TenCum;
        public string MaNhom;
        public string NgayPhatVon;
        public string NgayBaoCao;

        public string MaNguoiLap;
        public string TenNguoiLap;

        public string MaNgonNgu;
        public string MaDinhDang;

        #endregion

        public TDVM_PHIEU_THAM_DINH_VON_VAY()
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

            raddtNgayChot.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            cmbChiNhanh.SelectionChanged += cmbChiNhanh_SelectionChanged;
            cmbPhongGD.SelectionChanged += cmbPhongGD_SelectionChanged;
            cmbCum.SelectionChanged += cmbCum_SelectionChanged;
            cmbNhom.SelectionChanged += cmbNhom_SelectionChanged;
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
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceCum, ref cmbCum, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM_ALL.getValue());
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceNhom, ref cmbNhom, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NHOM_ALL.getValue());
            LoadComboboxPhongGD();
            LoadComboboxCum();
            LoadComboboxNhom();

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.LY_DO_VAO_RA.getValue());
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
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.PDF.layGiaTri());
            cmbDinhDang.IsEnabled = false;
        }

        private void LoadComboboxPhongGD()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();

            lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            if (!maChiNhanh.Equals("%"))
                lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();
            
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
            if (!maPhongGiaoDich.Equals("%"))
                lstSourceCum_Select = lstSourceCum.Where(e => e.KeywordStrings.ElementAt(1).Substring(0, maPhongGiaoDich.Length).Equals(maPhongGiaoDich)).ToList();
            // khởi tạo combobox
            auto = new AutoComboBox();
            cmbCum.Items.Clear();
            auto.GenAutoComboBox(ref lstSourceCum_Select, ref cmbCum, null);
            cmbCum.SelectedIndex = 0;
            cmbCum.IsEnabled = !maPhongGiaoDich.Equals("%");
        }

        private void LoadComboboxNhom()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();

            lstSourceNhom_Select = new List<AutoCompleteEntry>();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            string maPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
            if (lstSourceCum_Select.Count > 0)
            {
                string maCum = lstSourceCum_Select.ElementAt(cmbCum.SelectedIndex).KeywordStrings.First();
                if (!maCum.Equals("%"))
                    lstSourceNhom_Select = lstSourceNhom.Where(e => e.KeywordStrings.ElementAt(0).Substring(0, maCum.Length).Equals(maCum)).ToList();
            }
            // khởi tạo combobox
            auto = new AutoComboBox();
            try
            {
                cmbNhom.Items.Clear();
            }
            catch (Exception ex)
            {}
            auto.GenAutoComboBox(ref lstSourceNhom_Select, ref cmbNhom, null);
            cmbNhom.SelectedIndex = 0;

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
            string maChiNhanh = string.Empty;
            string tenChiNhanh = string.Empty;
            maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            tenChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).DisplayName;

            string maPhongGiaoDich = string.Empty;
            string tenPhongGiaoDich = string.Empty;
            if (lstSourcePhongGD_Select.Count > 0)
            {
                maPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
                tenPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).DisplayName;
            }

            MaDonVi = (MaPhongGiaoDich.IsNullOrEmpty() || maPhongGiaoDich.Equals("%")) ? maChiNhanh : maPhongGiaoDich;

            DateTime ngayPhatVon = new DateTime();
            if (raddtNgayChot.Value is DateTime)
                ngayPhatVon = (DateTime)raddtNgayChot.Value;
            DateTime ngayBaoCao = new DateTime();
            if (raddtNgayBaoCao.Value is DateTime)
                ngayBaoCao = (DateTime)raddtNgayBaoCao.Value;

            string tenCum = string.Empty;
            if (lstSourceCum_Select.Count > 0)
                tenCum = lstSourceCum_Select.ElementAt(cmbCum.SelectedIndex).KeywordStrings[1];
            else
                tenCum = string.Empty;
            string maCum = string.Empty;
            if (lstSourceCum_Select.Count > 0)
                maCum = lstSourceCum_Select.ElementAt(cmbCum.SelectedIndex).KeywordStrings.First();
            else
                maCum = string.Empty;
            string maNhom = string.Empty;
            if (lstSourceNhom_Select.Count > 0)
                maNhom = lstSourceNhom_Select.ElementAt(cmbNhom.SelectedIndex).KeywordStrings.First();
            else
                maNhom = string.Empty;

            string maNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            string maDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();
            
            // Gán dữ liệu từ form vào các biến truyền cho báo cáo
            MaChiNhanh = maChiNhanh;
            TenChiNhanh = tenChiNhanh;
            MaPhongGiaoDich = maPhongGiaoDich;
            TenPhongGiaoDich = tenPhongGiaoDich;
            MaCum = maCum;
            TenCum = tenCum;
            MaNhom = maNhom;
            NgayPhatVon = ngayPhatVon.ToString("yyyyMMdd");
            NgayBaoCao = ngayBaoCao.ToString("yyyyMMdd");
            MaNguoiLap = ClientInformation.TenDangNhap;
            TenNguoiLap = ClientInformation.HoTen;
            MaNgonNgu = maNgonNgu;
            MaDinhDang = maDinhDang;
        }

        private bool Validation(ref string msg)
        {
            if (grDanhSachKH.SelectedItems.Count == 0)
            {
                msg = "Chưa chọn khách hàng cho báo cáo.";
                return false;
            }
            return true;
        }

        public DatabaseConstant.Action GetAction() { return DatabaseConstant.Action.IN_CHUNG; }

        public List<ThamSoBaoCao> GetParameters()
        {
            string msg = string.Empty;
            if (!Validation(ref msg))
            {
                if (msg.Equals(""))
                    LMessage.ShowMessage("Thiếu tham số cho báo cáo.", LMessage.MessageBoxType.Information);
                else
                    LMessage.ShowMessage(msg, LMessage.MessageBoxType.Information);
                return null;
            }

            GetFormData();

            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGD", MaPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaCum", MaCum, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaNhom", MaNhom, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            if (grDanhSachKH.SelectedItems.Count > 0)
            {
                for (int i = 0; i < grDanhSachKH.SelectedItems.Count; i++)
                {
                    DataRow dr = (DataRow)grDanhSachKH.SelectedItems[i];
                    listThamSoBaoCao.Add(new ThamSoBaoCao("@DSKhachHang", dr["MA_KHANG"].ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                }
            }
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayChotDL", NgayPhatVon, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));


            return listThamSoBaoCao;
        }

        private void cmbCum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCum.SelectedIndex >= 0)
                LoadComboboxNhom();
        }

        private void cmbNhom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoCompleteEntry auNhom = lstSourceNhom_Select.ElementAt(cmbNhom.SelectedIndex);
            DataSet ds = new BaoCaoProcess().GetKhachHangTheoNhom(auNhom.KeywordStrings[0].ToString());
            if (ds != null && ds.Tables.Count > 0)
                grDanhSachKH.ItemsSource = ds.Tables[0];
            else
                grDanhSachKH.ItemsSource = null;
        }
    }
}
