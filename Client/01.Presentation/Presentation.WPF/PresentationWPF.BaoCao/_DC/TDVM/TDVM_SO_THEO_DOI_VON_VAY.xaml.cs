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
using System.Data;
using Presentation.Process;
using Presentation.Process.Common;

namespace PresentationWPF.BaoCao._DC.TDVM
{
    /// <summary>
    /// Interaction logic for TDVM_SO_THEO_DOI_VON_VAY.xaml
    /// </summary>
    public partial class TDVM_SO_THEO_DOI_VON_VAY : UserControl
    {

        #region Khai bao
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        //ListCheckBoxCombo lstSourceChiNhanh = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourcePhongGD = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourceSanPham = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourceXa = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourceAp = new ListCheckBoxCombo();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();

        // Các tham số báo cáo từ form điều kiện
        public string MaBaoCao;
        public string MaChiNhanh;
        //public List<string> MaChiNhanh;
        public string TenChiNhanh;
        public List<string> MaPhongGiaoDich;
        public string TenPhongGiaoDich;
        public string NgayBaoCao;
        public string MaNguoiLap;
        public string TenNguoiLap;
        public string MaLoaiTien;
        public List<string> IDSanPham;
        public List<string> IDXa;
        public List<string> IDAp;

        public string TuNgay;
        public string DenNgay;

        public string MaNgonNgu;
        public string MaDinhDang;

        #endregion

        public TDVM_SO_THEO_DOI_VON_VAY()
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
            cmbPhongGD.DropDownClosed += new EventHandler(cmbPhongGD_DropDownClosed);
            cmbSanPham.DropDownClosed += new EventHandler(cmbSanPham_DropDownClosed);
            cmbXaPhuong.DropDownClosed += new EventHandler(cmbXaPhuong_DropDownClosed);
            cmbThonAp.DropDownClosed += new EventHandler(cmbThonAp_DropDownClosed);

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
            LoadComboboxPhongGD();

            //LoadComboboxSanPham
            LoadComboboxSanPham();

            //LoadComboboxXaPhuong
            LoadComboboxXaPhuong();

            //LoadComboboxThonAp
            LoadComboboxThonAp();


            Dispatcher.CurrentDispatcher.DelayInvoke("cmbLoaiTien", () =>
            {
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.DONG_TIEN_CAC_NUOC.getValue());
                // khởi tạo combobox
                auto = new AutoComboBox();
                auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
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
            new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceSanPham, ref cmbSanPham, DatabaseConstant.DanhSachTruyVan.COMBOBOX_SAN_PHAM_TDLIST.getValue(), lstDieuKien);
        }

        private void LoadComboboxXaPhuong()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            lstSourceXa = new ListCheckBoxCombo();
            string maPGD = "";
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            string idChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings[1];

            lstSourcePhongGD = cmbPhongGD.ItemsSource as ListCheckBoxCombo;
            foreach (AutoCompleteCheckBox lstCN in lstSourcePhongGD)
            {
                if (lstCN.CheckedMember)
                    maPGD += ",'" + lstCN.ValueMember[0].ToString() + "'";
            }
            if (maChiNhanh.Length > 0)
                maPGD = maPGD.Substring(1);
            else
                maPGD = "0";

            List<string> lstDieuKien = new List<string>();

            // khởi tạo combobox
            auto = new AutoComboBox();
            lstDieuKien.Add(maChiNhanh);
            lstDieuKien.Add(maPGD);
            new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceXa, ref cmbXaPhuong, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUCLIST.getValue(), lstDieuKien);
        }

        private void LoadComboboxThonAp()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            lstSourceXa = new ListCheckBoxCombo();
            lstSourceAp = new ListCheckBoxCombo();
            string maPGD = "";
            string maKhuVuc = "";
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            string idChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings[1];

            lstSourcePhongGD = cmbPhongGD.ItemsSource as ListCheckBoxCombo;
            foreach (AutoCompleteCheckBox lstCN in lstSourcePhongGD)
            {
                if (lstCN.CheckedMember)
                    maPGD += ",'" + lstCN.ValueMember[0].ToString() + "'";
            }
            if (maPGD.Length > 0)
                maPGD = maPGD.Substring(1);
            else
                maPGD = "0";

            lstSourceXa = cmbXaPhuong.ItemsSource as ListCheckBoxCombo;
            foreach (AutoCompleteCheckBox lstCN in lstSourceXa)
            {
                if (lstCN.CheckedMember)
                    maKhuVuc += "," + lstCN.ValueMember[1].ToString();
            }
            if (maKhuVuc.Length > 0)
                maKhuVuc = maKhuVuc.Substring(1);
            else
                maKhuVuc = "0";

            List<string> lstDieuKien = new List<string>();

            // khởi tạo combobox
            auto = new AutoComboBox();
            lstDieuKien.Add(maChiNhanh);
            lstDieuKien.Add(maPGD);
            lstDieuKien.Add(maKhuVuc);
            new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceAp, ref cmbThonAp, "COMBOBOX_CUM_KVUC_LIST", lstDieuKien);
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbChiNhanh.SelectedIndex > 0)
            {
                LoadComboboxPhongGD();
                LoadComboboxSanPham();
                LoadComboboxXaPhuong();
                LoadComboboxThonAp();
            }
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
            string maLoaiTien = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.First();
            string maDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();

            //Lấy giá trị
            MaChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            TenChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).DisplayName;


            MaPhongGiaoDich = new List<string>();
            TenPhongGiaoDich = "";
            foreach (AutoCompleteCheckBox PhongGD in lstSourcePhongGD.Where(e => e.CheckedMember == true))
            {
                MaPhongGiaoDich.Add(PhongGD.ValueMember.FirstOrDefault());
                TenPhongGiaoDich = TenPhongGiaoDich.Equals("") ? PhongGD.DislayMember : (" - " + PhongGD.DislayMember);
            }

            IDSanPham = new List<string>();
            foreach (AutoCompleteCheckBox SanPham in lstSourceSanPham.Where(e => e.CheckedMember == true))
            {
                IDSanPham.Add(SanPham.ValueMember[1]);
            }

            IDXa = new List<string>();
            foreach (AutoCompleteCheckBox Xa in lstSourceXa.Where(e => e.CheckedMember == true))
            {
                IDXa.Add(Xa.ValueMember[1]);
            }

            IDAp = new List<string>();
            foreach (AutoCompleteCheckBox Ap in lstSourceAp.Where(e => e.CheckedMember == true))
            {
                IDAp.Add(Ap.ValueMember[1]);
            }

            if (MaPhongGiaoDich.Contains("All")) TenPhongGiaoDich = "";

            TuNgay = tuNgay.ToString("yyyyMMdd");
            DenNgay = denNgay.ToString("yyyyMMdd");
            NgayBaoCao = ngayBaoCao.ToString("yyyyMMdd");
            MaLoaiTien = maLoaiTien;
            MaNgonNgu = maNgonNgu;
            MaDinhDang = maDinhDang;
            MaNguoiLap = Presentation.Process.Common.ClientInformation.TenDangNhap;
            TenNguoiLap = Presentation.Process.Common.ClientInformation.HoTen;
        }

        private bool Validation()
        {
            //if (lstSourceChiNhanh.Where(e => e.CheckedMember == true).ToList().Count < 1)
            //    return false;

            if (raddtTuNgay.Value > raddtDenNgay.Value)
            {
                LMessage.ShowMessage("M.ResponseMessage.BaoCao.TuNgayLonHonDenNgay", LMessage.MessageBoxType.Warning);
                raddtTuNgay.Focus();
                return false;
            }
            if (lstSourcePhongGD.Where(e => e.CheckedMember == true).ToList().Count < 1)
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

            foreach (string PhongGD in MaPhongGiaoDich)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGiaoDich", PhongGD, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            foreach (string SanPham in IDSanPham)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaSanPham", SanPham, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            foreach (string Xa in IDXa)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaKhuVuc", Xa, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            foreach (string Ap in IDAp)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaCum", Ap, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            listThamSoBaoCao.Add(new ThamSoBaoCao("@TenChiNhanh", TenChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TenPhongGiaoDich", TenPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayPhatVon", TuNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
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

        private void cmbChiNhanh_DropDownClosed(object sender, EventArgs e)
        {
            //lstSourceChiNhanh = cmbChiNhanh.ItemsSource as ListCheckBoxCombo;
            LoadComboboxPhongGD();
            LoadComboboxSanPham();
        }

        private void cmbXaPhuong_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceXa = cmbXaPhuong.ItemsSource as ListCheckBoxCombo;
            LoadComboboxThonAp();
        }

        private void cmbThonAp_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceAp = cmbThonAp.ItemsSource as ListCheckBoxCombo;
        }

        private void cmbPhongGD_DropDownClosed(object sender, EventArgs e)
        {
            lstSourcePhongGD = cmbPhongGD.ItemsSource as ListCheckBoxCombo;
            LoadComboboxXaPhuong();
            LoadComboboxThonAp();
        }

        private void cmbSanPham_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceSanPham = cmbSanPham.ItemsSource as ListCheckBoxCombo;
        }
    }
}
