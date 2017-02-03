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
    public partial class BCTK_BANG_CAN_DOI : UserControl
    {
        #region khai bao
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        ListCheckBoxCombo lstSourceChiNhanh = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourcePhongGD = new ListCheckBoxCombo();        
        public string MaChiNhanh = "";
        public string MaPhongGD = "";
        public string TuNgay = "";
        public string DenNgay = "";
        public string NgayBaoCao = "";
        public string MaMauBieu = "";
        public string MaNguoiLap = "";
        public string TenNguoiLap="";
        public string TenChiNhanh = "";
        public string MaNgonNgu = "";
        public string MaDinhDang = "";
        #endregion
        public BCTK_BANG_CAN_DOI()
        {
            InitializeComponent();
            LoadCombobox();
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
            auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());

            //Tao combobox dinh dang bao cao
            auto = new AutoComboBox();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.PDF.layGiaTri());

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
            MaChiNhanh = "%";
            MaPhongGD = "%";
            DateTime dtNgayBC = new DateTime();
            if (raddtNgayBaoCao.Value is DateTime)
            {
                dtNgayBC = (DateTime)raddtNgayBaoCao.Value;
                NgayBaoCao = dtNgayBC.ToString(ApplicationConstant.defaultDateTimeFormat);
                TuNgay = dtNgayBC.GetFirstDateOfMonth(ApplicationConstant.defaultDateTimeFormat);
                DenNgay = dtNgayBC.GetLastDateOfMonth(ApplicationConstant.defaultDateTimeFormat);
            }
            else
            {
                dtNgayBC = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
                NgayBaoCao = ClientInformation.NgayLamViecHienTai;
                TuNgay = dtNgayBC.GetFirstDateOfMonth(ApplicationConstant.defaultDateTimeFormat);
                DenNgay = dtNgayBC.GetLastDateOfMonth(ApplicationConstant.defaultDateTimeFormat);
            }
            MaNguoiLap = ClientInformation.TenDangNhap;
            TenNguoiLap = ClientInformation.HoTen;
            MaMauBieu = "MF_KTDL_BCTK_BANG_CAN_DOI";
            TenChiNhanh = ClientInformation.TenDonVi;
            MaNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            MaDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();
        }
        public List<ThamSoBaoCao> GetParameters()
        {
            GetFormValues();
            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenChiNhanh", TenChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_DiaChi", "", ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NoiBaoCao", "", ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));       
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenNguoiLap",TenNguoiLap,ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));            
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayBaoCao",NgayBaoCao,ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));                 
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh",MaChiNhanh,ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGiaoDich", MaPhongGD, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaMauBieu",MaMauBieu,ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TuNgay",TuNgay,ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DenNgay",DenNgay,ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu",MaNgonNgu,ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDingDang",MaDinhDang,ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));
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
