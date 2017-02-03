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
using Presentation.Process.Common;
using System.Windows.Threading;
using PresentationWPF.BaoCao.DungChung;

namespace PresentationWPF.BaoCao._QUANGBINH.HDVO
{
    /// <summary>
    /// Interaction logic for HDVO_DS_HUY_DONG_VON_CAP_XA.xaml
    /// </summary>
    public partial class HDVO_DS_HUY_DONG_VON_CAP_XA : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        //ListCheckBoxCombo lstSourceChiNhanh = new ListCheckBoxCombo();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        ListCheckBoxCombo lstSourceSanPham = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourceNguonVon = new ListCheckBoxCombo();
        List<AutoCompleteEntry> lstSourceXa = new List<AutoCompleteEntry>();
        ListCheckBoxCombo lstSourceAp = new ListCheckBoxCombo();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();

        // Các tham số báo cáo từ form điều kiện
        public string MaBaoCao;
        public string MaChiNhanh;
        //public List<string> MaChiNhanh;
        public string TenChiNhanh;
        public string MaPhongGiaoDich;
        public string TenPhongGiaoDich;
        public string NgayBaoCao;
        public string MaNguoiLap;
        public string TenNguoiLap;
        public string MaLoaiTien;
        public List<string> IDSanPham;
        public string IDXa;
        public List<string> IDAp;
        public List<string> lstNguonVon;

        public string TuNgay;
        public string DenNgay;

        public string MaNgonNgu;
        public string MaDinhDang;

        #endregion

        public HDVO_DS_HUY_DONG_VON_CAP_XA()
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
            raddtTuNgay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            raddtDenNgay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            raddtNgayBaoCao.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);

            cmbChiNhanh.SelectionChanged += new SelectionChangedEventHandler(cmbChiNhanh_SelectionChanged);
            //cmbChiNhanh.DropDownClosed += new EventHandler(cmbChiNhanh_DropDownClosed);
            cmbPhongGD.SelectionChanged += new SelectionChangedEventHandler(cmbPhongGD_SelectionChanged);
            cmbSanPham.DropDownClosed += new EventHandler(cmbSanPham_DropDownClosed);
            cmbXaPhuong.SelectionChanged += new SelectionChangedEventHandler(cmbXaPhuong_SelectionChanged);
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
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), lstDieuKien, ClientInformation.MaDonVi);


            // khởi tạo combobox
            Dispatcher.CurrentDispatcher.DelayInvoke("cmbPhongGD", () =>
            {
                auto = new AutoComboBox();
                auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue());
                LoadComboboxPhongGD();
            }, TimeSpan.FromSeconds(0));

            //LoadComboboxSanPham
            Dispatcher.CurrentDispatcher.DelayInvoke("cmbSanPham", () =>
            {
                if (cmbChiNhanh.SelectedIndex == -1) return;
                LoadComboboxSanPham();
            }, TimeSpan.FromSeconds(0));

            //LoadComboboxXaPhuong
            Dispatcher.CurrentDispatcher.DelayInvoke("cmbXaPhuong", () =>
            {
                if (cmbPhongGD.SelectedIndex == -1) return;
                LoadComboboxXaPhuong();
            }, TimeSpan.FromSeconds(0));

            //LoadComboboxThonAp
            Dispatcher.CurrentDispatcher.DelayInvoke("cmbThonAp", () =>
            {
                if (cmbXaPhuong.SelectedIndex == -1) return;
                LoadComboboxThonAp();
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("LoadComboboxNguonVon", () =>
            {
                LoadComboboxNguonVon();
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("cmbNgonNgu", () =>
            {
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
                // khởi tạo combobox
                auto = new AutoComboBox();
                //auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());
                auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ClientInformation.NgonNgu);
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("cmbDinhDang", () =>
            {
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
                // khởi tạo combobox
                auto = new AutoComboBox();
                //auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri());
                //cmbDinhDang.SelectedIndex = lstSourceDinhDang.FindIndex(e => e.KeywordStrings[0].Equals(ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri()));
                auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ReportInformation.DinhDang);
                cmbDinhDang.IsEnabled = false;
            }, TimeSpan.FromSeconds(0));

        }

        private void LoadComboboxPhongGD()
        {
            if (cmbChiNhanh.SelectedIndex >= 0)
            {
                AutoComboBox auto = new AutoComboBox();

                lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
                string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();

                // khởi tạo combobox
                auto = new AutoComboBox();
                cmbPhongGD.Items.Clear();
                cmbXaPhuong.Items.Clear();
                auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
                cmbPhongGD.SelectedIndex = 0;
            }
        }

        private void LoadComboboxSanPham()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            lstSourceSanPham = new ListCheckBoxCombo();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            string idChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings[1];

            List<string> lstDieuKien = new List<string>();

            // khởi tạo combobox
            auto = new AutoComboBox();
            lstDieuKien.Add(maChiNhanh);
            new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceSanPham, ref cmbSanPham, "COMBOBOX_SAN_PHAM_TK_01", lstDieuKien);
        }

        private void LoadComboboxXaPhuong()
        {
            lstSourceXa = new List<AutoCompleteEntry>();
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            string IdDonVi = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings[3];
            lstDieuKien.Add(IdDonVi);

            cmbXaPhuong.Items.Clear();
            auto.GenAutoComboBox(ref lstSourceXa, ref cmbXaPhuong, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUC.getValue(), lstDieuKien, null);
            cmbXaPhuong.SelectedIndex = 0;
        }

        private void LoadComboboxThonAp()
        {
            lstSourceAp = new ListCheckBoxCombo();
            AutoComboBoxListCheckes auto = new AutoComboBoxListCheckes();
            List<string> lstDieuKien = new List<string>();
            string idDonVi = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings[3];
            string idKhuVuc = lstSourceXa.ElementAt(cmbXaPhuong.SelectedIndex).KeywordStrings[1];
            lstDieuKien.Add(idDonVi);
            lstDieuKien.Add(idKhuVuc);

            auto.GenAutoComboBox(ref lstSourceAp, ref cmbThonAp, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM.getValue(), lstDieuKien);
        }

        private void LoadComboboxNguonVon()
        {
            lstSourceNguonVon = new ListCheckBoxCombo();
            AutoComboBoxListCheckes auto = new AutoComboBoxListCheckes();

            auto.GenAutoComboBox(ref lstSourceNguonVon, ref cmbNguonVon, "COMBOBOX_NGUON_VON_CT", null);
        }

        private void GetFormData()
        {
            string maNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            string maDonVi = string.Empty;

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

            //Lấy giá trị
            MaChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            TenChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).DisplayName;


            MaPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
            TenPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).DisplayName;

            IDSanPham = new List<string>();
            foreach (AutoCompleteCheckBox SanPham in lstSourceSanPham.Where(e => e.CheckedMember == true))
            {
                IDSanPham.Add(SanPham.ValueMember[1]);
            }

            IDXa = lstSourceXa.ElementAt(cmbXaPhuong.SelectedIndex).KeywordStrings[1];

            IDAp = new List<string>();
            foreach (AutoCompleteCheckBox Ap in lstSourceAp.Where(e => e.CheckedMember == true))
            {
                IDAp.Add(Ap.ValueMember[1]);
            }

            lstNguonVon = new List<string>();
            foreach (AutoCompleteCheckBox NguonVon in lstSourceNguonVon.Where(e => e.CheckedMember == true))
            {
                lstNguonVon.Add(NguonVon.ValueMember[0]);
            }

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
            if (raddtTuNgay.Value > raddtDenNgay.Value)
            {
                LMessage.ShowMessage("M.ResponseMessage.BaoCao.TuNgayLonHonDenNgay", LMessage.MessageBoxType.Warning);
                raddtTuNgay.Focus();
                return false;
            }
            if (cmbPhongGD.SelectedIndex == -1)
                return false;
            if (cmbXaPhuong.SelectedIndex == -1)
                return false;
            if (cmbThonAp.SelectedIndex == -1)
                return false;
            if (lstSourceSanPham.Where(e => e.CheckedMember == true).ToList().Count < 1)
                return false;
            if (lstSourceNguonVon.Where(e => e.CheckedMember == true).ToList().Count < 1)
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
            //listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenPGD", TenPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TuNgay", TuNgay, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_DenNgay", DenNgay, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            //listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayMoSo", TuNgay, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));


            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGiaoDich", MaPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));


            foreach (string SanPham in IDSanPham)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@IdSanPham", SanPham, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            listThamSoBaoCao.Add(new ThamSoBaoCao("@IdKhuVuc", IDXa, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            foreach (string Ap in IDAp)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@IdCum", Ap, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            foreach (string NguonVon in lstNguonVon)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaNguonVon", NguonVon, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            listThamSoBaoCao.Add(new ThamSoBaoCao("@TenChiNhanh", TenChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TenPhongGiaoDich", TenPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TuNgay", TuNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DenNgay", DenNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            //listThamSoBaoCao.Add(new ThamSoBaoCao("@MaLoaiTien", MaLoaiTien, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@LoaiTK", "TKTH", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("@P_MaDonVi", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.GUIPARAM.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            return listThamSoBaoCao;
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbChiNhanh.SelectedIndex == -1) return;
            LoadComboboxPhongGD();
            LoadComboboxSanPham();
        }

        private void cmbXaPhuong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbXaPhuong.SelectedIndex == -1) return;
            LoadComboboxThonAp();
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPhongGD.SelectedIndex == -1) return;
            LoadComboboxXaPhuong();
        }

        private void cmbSanPham_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceSanPham = cmbSanPham.ItemsSource as ListCheckBoxCombo;
        }
    }
}
