using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
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

namespace PresentationWPF.BaoCao._BTV.GDKT
{
    /// <summary>
    /// Interaction logic for GDKT_UY_NHIEM_CHI.xaml
    /// </summary>
    public partial class GDKT_UY_NHIEM_CHI : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        // Các tham số báo cáo từ form điều kiện
        public string MaNgonNgu;
        public string MaDinhDang;
        public string NgayBaoCao;
        public string MaChiNhanh;
        public string TenChiNhanh;
        public string MaPhongGiaoDich;
        public string TenPhongGiaoDich;
        public string MaDangNhap;
        public string TenDangNhap;

        public string SoPhieu;
        #endregion

        public GDKT_UY_NHIEM_CHI()
        {
            InitializeComponent();
            LoadCombobox();
        }

        private void LoadCombobox()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            // Khởi tạo điều kiện gọi danh mục
            List<string> lstDieuKien = new List<string>();

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri());
            cmbDinhDang.IsEnabled = false;
        }

        #region Xu ly nghiep vu
        private void GetFormData()
        {
            // Khai báo giá trị
            string maNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            string maDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();
            string ngayBaoCao = string.Empty;
            string maChiNhanh = string.Empty;
            string tenChiNhanh = string.Empty;
            string maPhongGiaoDich = string.Empty;
            string tenPhongGiaoDich = string.Empty;
            string maDangNhap = string.Empty;
            string tenDangNhap = string.Empty;

            string soPhieu = txtSoGiaoDich.Text;

            // Lấy giá trị
            MaNgonNgu = maNgonNgu;
            MaDinhDang = maDinhDang;
            NgayBaoCao = ngayBaoCao;
            MaChiNhanh = maChiNhanh;
            TenChiNhanh = tenChiNhanh;
            MaPhongGiaoDich = maPhongGiaoDich;
            TenPhongGiaoDich = tenPhongGiaoDich;
            MaDangNhap = maDangNhap;
            TenDangNhap = tenDangNhap;

            SoPhieu = soPhieu;
        }

        private bool Validation()
        {
            if (string.IsNullOrEmpty(txtSoGiaoDich.Text))
            {
                LMessage.ShowMessage("Chưa nhập số giao dịch", LMessage.MessageBoxType.Information);
                return false;
            }
            return true;
        }

        public List<ThamSoBaoCao> GetParameters()
        {
            if (!Validation())
            {
                //LMessage.ShowMessage("Không đủ điều kiện vận hành báo cáo", LMessage.MessageBoxType.Information);
                return null;
            }

            GetFormData();

            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();

            listThamSoBaoCao.Add(new ThamSoBaoCao("@SoPhieu", SoPhieu, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            return listThamSoBaoCao;
        }
        #endregion
    }
}
