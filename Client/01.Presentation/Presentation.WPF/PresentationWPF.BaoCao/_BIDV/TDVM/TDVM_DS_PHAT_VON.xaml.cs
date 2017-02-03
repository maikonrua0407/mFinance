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

namespace PresentationWPF.BaoCao._BIDV.TDVM
{
    /// <summary>
    /// Interaction logic for TDVM_DS_PHAT_VON.xaml
    /// </summary>
    public partial class TDVM_DS_PHAT_VON : UserControl
    {

        #region Khai bao

        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        ListCheckBoxCombo lstSourcePhongGD = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourceCanBoTD = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourceXa = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourceAp = new ListCheckBoxCombo();
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
        public List<string> MaPhongGiaoDich;
        public string TenPhongGiaoDich;
        public string NgayBaoCao;
        public string MaNguoiLap;
        public string TenNguoiLap;
        public string MaLoaiTien;
        public List<string> IDCanBo;
        public List<string> IDXa;
        public List<string> IDAp;
        public List<string> IDNhom;

        public string TuNgay;
        public string DenNgay;

        public string MaNgonNgu;
        public string MaDinhDang;

        #endregion

        public TDVM_DS_PHAT_VON()
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
            raddtNgayBaoCao.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);

            //cmbChiNhanh.DropDownClosed +=new EventHandler(cmbChiNhanh_DropDownClosed);
            cmbChiNhanh.SelectionChanged += new SelectionChangedEventHandler(cmbChiNhanh_SelectionChanged);
            cmbPhongGD.DropDownClosed += new EventHandler(cmbPhongGD_DropDownClosed);
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

            //// khởi tạo combobox
            LoadComboBoxPhongGD();


            //LoadComboBoxXaPhuong
            LoadComboBoxXaPhuong();

            //LoadComboBoxThonAp
            LoadComboBoxThonAp();

            // khởi tạo dữ liệu nhóm
            KhoiTaoDuLieuNhom();

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

        private void KhoiTaoDuLieuNhom()
        {
            lstSourceNhomAll.Clear();
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(Convert.ToDateTime(raddtTuNgay.Value).ToString("yyyyMMdd"));
            Telerik.Windows.Controls.RadComboBox cmdNhomAll = new Telerik.Windows.Controls.RadComboBox();
            auto.GenAutoComboBox(ref lstSourceNhomAll, ref cmdNhomAll, "COMBOBOX_NHOM_KUOCVM_GN", lstDieuKien);
            LoadDuLieuNhom();
        }

        private void LoadComboBoxPhongGD()
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

        private void LoadComboBoxXaPhuong()
        {
            if (cmbChiNhanh.SelectedIndex >= 0 && cmbPhongGD.SelectedIndex >= 0)
            {
                // khởi tạo combobox
                AutoComboBox auto = new AutoComboBox();
                string maChiNhanh = "";
                string maPGD = "";

                maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();

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

                List<string> lstDieuKien = new List<string>();

                // khởi tạo combobox
                auto = new AutoComboBox();
                lstDieuKien.Add(maChiNhanh);
                lstDieuKien.Add(maPGD);

                lstSourceXa = new ListCheckBoxCombo();
                new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceXa, ref cmbXaPhuong, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUCLIST.getValue(), lstDieuKien);
            }
        }

        private void LoadComboBoxThonAp()
        {
            if (cmbChiNhanh.SelectedIndex >= 0 && cmbPhongGD.SelectedIndex >= 0 && cmbXaPhuong.SelectedIndex >= 0)
            {
                // khởi tạo combobox
                AutoComboBox auto = new AutoComboBox();
                lstSourceXa = new ListCheckBoxCombo();
                string maChiNhanh = "";
                string maPGD = "";
                string maKhuVuc = "";
                maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();

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

                lstSourceAp = new ListCheckBoxCombo();
                new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceAp, ref cmbThonAp, "COMBOBOX_CUM_KVUC_LIST", lstDieuKien);
            }
        }

        private void LoadDuLieuNhom()
        {
            List<string> lstCum = lstSourceAp.Where(f => f.CheckedMember.Equals(true)).Select(f => f.ValueMember[1]).ToList();
            Tuple<int, string, string> tuple = null;
            lstSourceNhom_Select.Clear();
            foreach (AutoCompleteEntry auNhom in lstSourceNhomAll.Where(f => lstCum.Contains(f.KeywordStrings[4])).ToList())
            {
                tuple = new Tuple<int, string, string>(Convert.ToInt32(auNhom.KeywordStrings[3]), auNhom.KeywordStrings[0], auNhom.DisplayName);
                lstSourceNhom_Select.Add(tuple);
            }
            if (grSoTienGuiDS.IsNullOrEmpty())
                return;
            grSoTienGuiDS.ItemsSource = lstSourceNhom_Select;
            grSoTienGuiDS.Rebind();
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboBoxPhongGD();
            LoadComboBoxXaPhuong();
            LoadComboBoxThonAp();
            LoadDuLieuNhom();
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
                TenPhongGiaoDich += TenPhongGiaoDich.Equals("") ? PhongGD.DislayMember : (" - " + PhongGD.DislayMember);
            }

            if (MaPhongGiaoDich.Contains("All")) TenPhongGiaoDich = "";

            IDCanBo = new List<string>();
            foreach (AutoCompleteCheckBox CanBoTD in lstSourceCanBoTD.Where(e => e.CheckedMember == true))
            {
                IDCanBo.Add(CanBoTD.ValueMember[1]);
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

            IDNhom = new List<string>();
            foreach (Tuple<int, string, string> tupleNhom in grSoTienGuiDS.SelectedItems)
            {
                IDNhom.Add(tupleNhom.Item1.ToString());
            }

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

            foreach (string CanBo in IDCanBo)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaCanBo", CanBo, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            foreach (string Xa in IDXa)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@IdKhuVuc", Xa, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            foreach (string Ap in IDAp)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@IdCum", Ap, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            foreach (string Nhom in IDNhom)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@IdNhom", Nhom, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
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
            LoadComboBoxPhongGD();
        }

        private void cmbXaPhuong_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceXa = cmbXaPhuong.ItemsSource as ListCheckBoxCombo;
            LoadComboBoxThonAp();
        }

        private void cmbThonAp_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceAp = cmbThonAp.ItemsSource as ListCheckBoxCombo;
            LoadDuLieuNhom();
        }

        private void cmbPhongGD_DropDownClosed(object sender, EventArgs e)
        {
            lstSourcePhongGD = cmbPhongGD.ItemsSource as ListCheckBoxCombo;
            LoadComboBoxXaPhuong();
            LoadComboBoxThonAp();
        }

        private void raddtTuNgay_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            KhoiTaoDuLieuNhom();
            LoadDuLieuNhom();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            raddtTuNgay_ValueChanged(null, null);
        }
    }
}
