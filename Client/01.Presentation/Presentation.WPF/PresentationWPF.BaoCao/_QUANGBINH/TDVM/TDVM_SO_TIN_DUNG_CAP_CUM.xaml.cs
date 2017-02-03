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

namespace PresentationWPF.BaoCao._QUANGBINH.TDVM
{
    /// <summary>
    /// Interaction logic for TDVM_SO_TIN_DUNG.xaml
    /// </summary>
    public partial class TDVM_SO_TIN_DUNG_CAP_CUM : UserControl
    {
         #region Khai bao

        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceXa = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceAp = new List<AutoCompleteEntry>();
        ListCheckBoxCombo lstSourceNguonVon = new ListCheckBoxCombo();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNhomAll = new List<AutoCompleteEntry>();
        List<Tuple<int, string, string>> lstSourceNhom_Select = new List<Tuple<int, string, string>>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();

        // Các tham số báo cáo từ form điều kiện
        public string MaBaoCao;
        public string MaChiNhanh;
        public string TenChiNhanh;
        public string MaPhongGiaoDich;
        public string TenPhongGiaoDich;
        public string NgayBaoCao;
        public string MaNguoiLap;
        public string TenNguoiLap;
        public string IDXa;
        public string IDAp;
        public List<string> IDNhom;
        public List<string> IDNguonVon;

        public string ThangDuLieu;

        public string MaNgonNgu;
        public string MaDinhDang;

        #endregion

        public TDVM_SO_TIN_DUNG_CAP_CUM()
        {
            InitializeComponent();
            LoadCombobox();
                        
            cmbChiNhanh.SelectionChanged += new SelectionChangedEventHandler(cmbChiNhanh_SelectionChanged);
            cmbPhongGD.SelectionChanged += new SelectionChangedEventHandler(cmbPhongGD_SelectionChanged);
            cmbXaPhuong.SelectionChanged += new SelectionChangedEventHandler(cmbXaPhuong_SelectionChanged);
            cmbThonAp.SelectionChanged += new SelectionChangedEventHandler(cmbThonAp_SelectionChanged);

            raddtThangDuLieu.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMM");
            raddtNgayBaoCao.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
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

            //// khởi tạo combobox
            //Tao combobox phonggd
            Dispatcher.CurrentDispatcher.DelayInvoke("cmbPhongGD", () =>
            {
                auto = new AutoComboBox();
                auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue());
                LoadComboBoxPhongGD();
            }, TimeSpan.FromSeconds(0));


            //LoadComboboxNguonVon
            LoadComboboxNguonVon();

            // khởi tạo dữ liệu nhóm
            auto = new AutoComboBox();
            Telerik.Windows.Controls.RadComboBox cmdNhomAll = new Telerik.Windows.Controls.RadComboBox();
            auto.GenAutoComboBox(ref lstSourceNhomAll, ref cmdNhomAll, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NHOM_ALL.getValue());
            

            Dispatcher.CurrentDispatcher.DelayInvoke("cmbNgonNgu", () =>
            {
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
                // khởi tạo combobox
                auto = new AutoComboBox();
                auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("cmbDinhDang", () =>
            {
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
                // khởi tạo combobox
                auto = new AutoComboBox();
                auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri());
                cmbDinhDang.SelectedIndex = lstSourceDinhDang.FindIndex(e => e.KeywordStrings[0].Equals(ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri()));
            }, TimeSpan.FromSeconds(0));
            
        }

        private void LoadComboBoxPhongGD()
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
                auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
                cmbPhongGD.SelectedIndex = 0;
            }
        }

        private void LoadComboboxNguonVon()
        {
            lstSourceNguonVon = new ListCheckBoxCombo();
            AutoComboBoxListCheckes auto = new AutoComboBoxListCheckes();

            auto.GenAutoComboBox(ref lstSourceNguonVon, ref cmbNguonVon, "COMBOBOX_NGUON_VON_CT", null);
        }

        private void LoadComboBoxXaPhuong()
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

        private void LoadComboBoxThonAp()
        {
            lstSourceAp = new List<AutoCompleteEntry>();
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            string idDonVi = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings[3];
            string idKhuVuc = lstSourceXa.ElementAt(cmbXaPhuong.SelectedIndex).KeywordStrings[1];
            lstDieuKien.Add(idDonVi);
            lstDieuKien.Add(idKhuVuc);

            cmbThonAp.Items.Clear();
            auto.GenAutoComboBox(ref lstSourceAp, ref cmbThonAp, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM.getValue(), lstDieuKien);
            cmbThonAp.SelectedIndex = 0;
        }

        private void LoadDuLieuNhom()
        {
            List<string> lstCum = new List<string>();
            lstCum.Add(lstSourceAp.ElementAt(cmbThonAp.SelectedIndex).KeywordStrings[1]);
            Tuple<int, string, string> tuple = null;
            lstSourceNhom_Select.Clear();
            foreach (AutoCompleteEntry auNhom in lstSourceNhomAll.Where(f => lstCum.Contains(f.KeywordStrings[4])).ToList())
            {
                tuple = new Tuple<int, string, string>(Convert.ToInt32(auNhom.KeywordStrings[3]), auNhom.KeywordStrings[0], auNhom.DisplayName);
                lstSourceNhom_Select.Add(tuple);
            }
            grNhom.ItemsSource = lstSourceNhom_Select;
            grNhom.Rebind();
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbChiNhanh.SelectedIndex == -1) return;
            LoadComboBoxPhongGD();
        }

        private void GetFormData()
        {
            string maNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            string maDonVi = string.Empty;

            DateTime ngayBaoCao = new DateTime();
            if (raddtNgayBaoCao.Value is DateTime)
                ngayBaoCao = (DateTime)raddtNgayBaoCao.Value;
            DateTime thangDuLieu = new DateTime();
            if (raddtThangDuLieu.Value is DateTime)
                thangDuLieu = (DateTime)raddtThangDuLieu.Value;
            string maDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();

            //Lấy giá trị
            MaChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            TenChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).DisplayName;

            MaPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
            TenPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).DisplayName;


            IDNguonVon = new List<string>();
            foreach (AutoCompleteCheckBox NguonVon in lstSourceNguonVon.Where(e => e.CheckedMember == true))
            {
                IDNguonVon.Add(NguonVon.ValueMember[1]);
            }

            IDXa = lstSourceXa.ElementAt(cmbXaPhuong.SelectedIndex).KeywordStrings[1];

            IDAp = lstSourceAp.ElementAt(cmbThonAp.SelectedIndex).KeywordStrings[1];

            IDNhom = new List<string>();
            foreach (Tuple<int, string, string> tupleNhom in grNhom.SelectedItems)
            {
                IDNhom.Add(tupleNhom.Item1.ToString());
            }

            ThangDuLieu = thangDuLieu.GetLastDateOfMonth().ToString("yyyyMMdd");
            NgayBaoCao = ngayBaoCao.ToString("yyyyMMdd");
            MaNgonNgu = maNgonNgu;
            MaDinhDang = maDinhDang;
            MaNguoiLap = Presentation.Process.Common.ClientInformation.TenDangNhap;
            TenNguoiLap = Presentation.Process.Common.ClientInformation.HoTen;
        }

        private bool Validation()
        {
            if (cmbPhongGD.SelectedIndex == -1)
                return false;
            if (cmbXaPhuong.SelectedIndex == -1)
                return false;
            if (cmbThonAp.SelectedIndex == -1)
                return false;
            if (grNhom.Items.Count <= 0)
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
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_ThangDuLieu", ThangDuLieu, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            //listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayMoSo", TuNgay, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGD", MaPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            foreach (string NguonVon in IDNguonVon)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@IdNguonVon", NguonVon, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
                
            listThamSoBaoCao.Add(new ThamSoBaoCao("@IdKhuVuc", IDXa, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("@IdCum", IDAp, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            foreach (string Nhom in IDNhom)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@IdNhom", Nhom, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            listThamSoBaoCao.Add(new ThamSoBaoCao("@TenChiNhanh", TenChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TenPhongGiaoDich", TenPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@ThangDuLieu", ThangDuLieu, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("@P_MaDonVi", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.GUIPARAM.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            return listThamSoBaoCao;
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPhongGD.SelectedIndex == -1) return;
            LoadComboBoxXaPhuong();
        }

        private void cmbXaPhuong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbXaPhuong.SelectedIndex == -1) return;
            LoadComboBoxThonAp();
        }

        private void cmbThonAp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbThonAp.SelectedIndex == -1) return;
            LoadDuLieuNhom();
        }

        private void cmbNguonVon_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceNguonVon = cmbNguonVon.ItemsSource as ListCheckBoxCombo;
        }
    }
}
