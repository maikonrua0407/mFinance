using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;
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
using Presentation.Process;
using Presentation.Process.PopupServiceRef;

namespace PresentationWPF.BaoCao.TDVM
{
    /// <summary>
    /// Interaction logic for TDVM_BKE_CNO_TVIEN_THEO_CN.xaml
    /// </summary>
    public partial class TDVM_BKE_CNO_TVIEN_THEO_CN : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        ListCheckBoxCombo lstSourcePhongGD = new ListCheckBoxCombo();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKhuVuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKhuVuc_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();

        // Các tham số báo cáo từ form điều kiện
        public string MaBaoCao;
        public string MaDonVi;
        public string MaChiNhanh;
        public string TenChiNhanh;
        public string MaPhongGiaoDich;
        List<String> lstMaPhongGiaoDich;
        public string TenPhongGiaoDich;
        public string MaCum;
        public string TenCum;
        public string MaKhuVuc;
        public string TenKhuVuc;
        public string NgayChotSoLieu;
        public string NgayBaoCao;
        public string MaNguoiLap;
        public string TenNguoiLap;
        public string MaNgonNgu;
        public string MaDinhDang;
        public bool IsLoadForm = false;
        #endregion

        public TDVM_BKE_CNO_TVIEN_THEO_CN()
        {
            InitializeComponent();

            Dispatcher.CurrentDispatcher.DelayInvoke("LoadCombobox", () =>
            {
                LoadCombobox();
            }, TimeSpan.FromSeconds(0));
            
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

            raddtNgayChotSoLieu.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            raddtNgayBaoCao.Value = DateTime.Now;
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
            lstDieuKien.Add(ClientInformation.TenDangNhap);
            lstDieuKien.Add(ClientInformation.MaDonViQuanLy);
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), lstDieuKien);

            // khởi tạo combobox
            LoadComboboxPhongGD();

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
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri());
            cmbDinhDang.IsEnabled = false;

            cmbChiNhanh.SelectionChanged += cmbChiNhanh_SelectionChanged;
            cmbPhongGD.DropDownClosed += cmbPhongGD_DropDownClosed;
            IsLoadForm = true;
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoadForm.Equals(true)) LoadComboboxPhongGD();
        }

        private void cmbPhongGD_DropDownClosed(object sender, EventArgs e)
        {
            if (IsLoadForm.Equals(true))
            {
                lstSourcePhongGD = cmbPhongGD.ItemsSource as ListCheckBoxCombo; //cmbPhongGD.ItemsSource as ListCheckBoxCombo;
            }
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

            DateTime ngayChotSoLieu = new DateTime();
            if (raddtNgayChotSoLieu.Value is DateTime)
                ngayChotSoLieu = (DateTime)raddtNgayChotSoLieu.Value;

            DateTime ngayBaoCao = new DateTime();
            if (raddtNgayBaoCao.Value is DateTime)
                ngayBaoCao = (DateTime)raddtNgayBaoCao.Value;

            string maNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            string maDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();

            // Gán dữ liệu từ form vào các biến truyền cho báo cáo
            MaChiNhanh = maChiNhanh;
            TenChiNhanh = tenChiNhanh;
            NgayChotSoLieu = (raddtNgayChotSoLieu.Value is DateTime) ? ngayChotSoLieu.ToString("yyyyMMdd"): "";
            NgayBaoCao = (raddtNgayBaoCao.Value is DateTime) ? ngayBaoCao.ToString("yyyyMMdd") : "";
            MaNguoiLap = ClientInformation.TenDangNhap;
            TenNguoiLap = ClientInformation.HoTen;
            MaNgonNgu = maNgonNgu;
            MaDinhDang = maDinhDang;
        }

        private bool Validation(ref string msgResult)
        {
            bool bResult = true;

            if (lstMaPhongGiaoDich==null || lstMaPhongGiaoDich.Count.Equals(0)) { bResult = false; msgResult = "Chọn phòng giao dịch"; cmbPhongGD.Focus(); return bResult; }
            if (NgayChotSoLieu.Equals("")) { bResult = false; msgResult = "Chọn ngày chốt số liệu"; raddtNgayChotSoLieu.Focus(); return bResult; }
            if (NgayBaoCao.Equals("")) { bResult = false; msgResult = "Chọn ngày báo cáo"; raddtNgayBaoCao.Focus(); return bResult; }
            
            return bResult;
        }

        public DatabaseConstant.Action GetAction() { return DatabaseConstant.Action.IN_CHUNG; }

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
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayChotSoLieu", NgayChotSoLieu, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            return listThamSoBaoCao;
        }
    }

 
}
