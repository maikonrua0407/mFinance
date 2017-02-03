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

namespace PresentationWPF.BaoCao.BCTK
{
    /// <summary>
    /// Interaction logic for ucCanDoiKeToanHopNhat.xaml
    /// </summary>
    public partial class BCTK_MONTHLY_REPORT : UserControl
    {
        #region khai bao
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        ListCheckBoxCombo lstSourceChiNhanh = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourcePhongGD = new ListCheckBoxCombo();
        public string TuNgay = "";
        public string DenNgay = "";
        public string NgayBaoCao = "";
        public string MaMauBieu = "";
        public string MaNguoiLap = "";
        public string TenNguoiLap = "";
        public string MaNgonNgu = "";
        public string MaDinhDang = "";
        public List<string> MaChiNhanh;
        public List<string> MaPhongGiaoDich;
        public string TenChiNhanh = "";
        public string TenPhongGiaoDich = "";
        public decimal DonViTinh = 0;

        #endregion
        public BCTK_MONTHLY_REPORT()
        {
            InitializeComponent();
            LoadCombobox();
            raddtNgayDuLieu.Value = ClientInformation.NgayLamViecHienTai.StringToDate(ApplicationConstant.defaultDateTimeFormat);
        }
        /// <summary>
        /// Khoi tao combobox
        /// </summary>
        private void LoadCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            //Tao combobox ngon ngu bao cao
            auto = new AutoComboBox();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
            //auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());
            auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ClientInformation.NgonNgu);

            //Tao combobox dinh dang bao cao
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
            //auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri());
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ReportInformation.DinhDang);
            cmbDinhDang.IsEnabled = false;

            //Tao combobox dinh dang bao cao
            lstDieuKien = new List<string>();
            auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TIENTE.getValue(), lstDieuKien, ClientInformation.MaDongNoiTe);

            lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.TenDangNhap);
            lstDieuKien.Add(ClientInformation.MaDonViQuanLy);
            new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANHLIST.getValue(), lstDieuKien);
            //// khởi tạo combobox
            //auto = new AutoComboBox();
            //auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue());
            //LoadComboboxPhongGD();
            LoadComboboxPhongGD();
        }

        private void LoadComboboxPhongGD()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            lstSourcePhongGD = new ListCheckBoxCombo();
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

            // khởi tạo combobox
            auto = new AutoComboBox();
            lstDieuKien.Add(maChiNhanh);
            lstDieuKien.Add(ClientInformation.TenDangNhap);
            lstDieuKien.Add(ClientInformation.MaDonViQuanLy);
            new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PGDLIST.getValue(), lstDieuKien);
            //lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();
            //lstSourcePhongGD_Select.Insert(0, new AutoCompleteEntry("Tất cả", "%", ""));
            //// khởi tạo combobox
            //auto = new AutoComboBox();
            //cmbPhongGD.Items.Clear();
            //auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
        }

        /// <summary>
        /// Lay cac gia tri tren form 
        /// </summary>
        private void GetFormValues()
        {
            MaChiNhanh = new List<string>();
            MaPhongGiaoDich = new List<string>();
            DateTime dtNgayDL = new DateTime();
            if (raddtNgayDuLieu.Value is DateTime)
            {
                dtNgayDL = (DateTime)raddtNgayDuLieu.Value;
                NgayBaoCao = dtNgayDL.ToString(ApplicationConstant.defaultDateTimeFormat);
                TuNgay = dtNgayDL.GetFirstDateOfMonth(ApplicationConstant.defaultDateTimeFormat);
                DenNgay = dtNgayDL.ToString(ApplicationConstant.defaultDateTimeFormat);
            }
            else
            {
                dtNgayDL = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
                NgayBaoCao = ClientInformation.NgayLamViecHienTai;
                TuNgay = dtNgayDL.GetFirstDateOfMonth(ApplicationConstant.defaultDateTimeFormat);
                DenNgay = dtNgayDL.ToString(ApplicationConstant.defaultDateTimeFormat);
            }

            DonViTinh = Convert.ToDecimal(telnumDonViTinh.Value);
            MaNguoiLap = ClientInformation.TenDangNhap;
            TenNguoiLap = ClientInformation.HoTen;
            MaMauBieu = "MF_KTDL_BCTK_MONTHLY_REPORT";
            TenChiNhanh = ClientInformation.TenDonVi;
            MaNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            MaDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();
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
        }
        public List<ThamSoBaoCao> GetParameters()
        {
            GetFormValues();
            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenChiNhanh", TenChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_DiaChi", "", ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NoiBaoCao", "", ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenNguoiLap", TenNguoiLap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            foreach (string ChiNhanh in MaChiNhanh)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", ChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            foreach (string ChiNhanh in MaPhongGiaoDich)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGiaoDich", ChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaMauBieu", MaMauBieu, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TuNgay", TuNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DenNgay", DenNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDingDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));
            return listThamSoBaoCao;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

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
