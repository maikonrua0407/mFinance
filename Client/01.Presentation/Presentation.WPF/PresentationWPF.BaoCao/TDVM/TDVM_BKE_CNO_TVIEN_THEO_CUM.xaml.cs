using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
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
using System.Data;
using Presentation.Process;
using Presentation.Process.TruyVanServiceRef;
using Presentation.Process.PopupServiceRef;

namespace PresentationWPF.BaoCao.TDVM
{
    /// <summary>
    /// Interaction logic for TCVM_BKE_CNO_TVIEN_THEO_CUM.xaml
    /// </summary>
    public partial class TDVM_BKE_CNO_TVIEN_THEO_CUM : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKhuVuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKhuVuc_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum_Select = new List<AutoCompleteEntry>();
        DataTable dtSourceCum = new DataTable();
        DataTable dtSourceCum_Select = new DataTable();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();

        // Các tham số báo cáo từ form điều kiện
        public string MaBaoCao;
        public string MaDonVi;
        public string MaChiNhanh;
        public List<string> lstMaChiNhanh;
        public string TenChiNhanh;
        public string MaPhongGiaoDich;
        public List<string> lstMaPhongGiaoDich;
        public string TenPhongGiaoDich;
        public string MaCum;
        public List<string> lstMaCum;
        public string TenCum;
        public string MaKhuVuc;
        public List<string> lstMaKhuVuc;
        public string TenKhuVuc;
        public string NgayChotSoLieu;
        public string NgayBaoCao;
        public string MaNguoiLap;
        public string TenNguoiLap;
        public string MaNgonNgu;
        public string MaDinhDang;
        public bool IsLoadForm = false;

        public DatabaseConstant.Action GetAction() { return DatabaseConstant.Action.IN_CHUNG; }

        #endregion

        public TDVM_BKE_CNO_TVIEN_THEO_CUM()
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
            // Khai bao AutoComboBox
            AutoComboBox auto;
            // Khởi tạo điều kiện gọi danh mục
            List<string> lstDieuKien = new List<string>();

            //Tao combobox Chi nhanh,PGD, KVUC
            auto = new AutoComboBox();
            //lstSourceChiNhanh.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);
            auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue());
            auto.GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUC_ALL.getValue());
            LoadComboboxPhongGD();
            LoadComboboxKhuVuc();

            //auto.GenAutoComboBox(ref lstSourceCum, ref cmbCum, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM_ALL.getValue());
            //LoadComboboxCum();

            //Tao Gridview Cum
            var process = new TruyVanProcess();
            DanhSachResponse response = process.getDanhSachInformation(DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM_ALL.getValue(), lstDieuKien);
            if (!LObject.IsNullOrEmpty(response.DataSetSource) && response.DataSetSource.Tables[0].Rows.Count > 0)
            {
                dtSourceCum = response.DataSetSource.Tables[0];
                LoadGridViewCum();
            }

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
            cmbPhongGD.SelectionChanged += cmbPhongGD_SelectionChanged;
            cmbKhuVuc.SelectionChanged += cmbKhuVuc_SelectionChanged;
            IsLoadForm = true;
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoadForm.Equals(true)) LoadComboboxPhongGD();
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoadForm.Equals(true)) if (cmbPhongGD.SelectedIndex >= 0) LoadComboboxKhuVuc();
        }

        private void cmbKhuVuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoadForm.Equals(true)) if (cmbKhuVuc.SelectedIndex >= 0) LoadGridViewCum(); //LoadComboboxCum();
        }

        private void LoadComboboxPhongGD()
        {
            if (cmbChiNhanh.SelectedIndex >= 0)
            {
                // khởi tạo combobox
                AutoComboBox auto = new AutoComboBox();

                lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
                string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                //if (!maChiNhanh.Equals("%"))
                lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();
                //lstSourcePhongGD_Select.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));

                // khởi tạo combobox
                auto = new AutoComboBox();
                cmbPhongGD.Items.Clear();
                auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
                cmbPhongGD.SelectedIndex = 0;
                //cmbPhongGD.IsEnabled = !maChiNhanh.Equals("%");
            }
        }

        private void LoadComboboxKhuVuc()
        {
            if (cmbPhongGD.SelectedIndex >= 0)
            {
                AutoComboBox auto = new AutoComboBox();

                lstSourceKhuVuc_Select = new List<AutoCompleteEntry>();
                string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                string maPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();

                string idPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings[3];
                //if (!maPhongGiaoDich.Equals("%"))
                lstSourceKhuVuc_Select = lstSourceKhuVuc.Where(e => e.KeywordStrings.ElementAt(3).Equals(idPhongGiaoDich)).ToList();
                //lstSourceCum_Select = lstSourceCum.Where(e => e.KeywordStrings.ElementAt(3).Substring(0, maPhongGiaoDich.Length).Equals(maPhongGiaoDich)).ToList();

                //lstSourceKhuVuc_Select.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
                // khởi tạo combobox
                auto = new AutoComboBox();
                cmbKhuVuc.Items.Clear();
                auto.GenAutoComboBox(ref lstSourceKhuVuc_Select, ref cmbKhuVuc, null);
                cmbKhuVuc.SelectedIndex = 0;
                //cmbKhuVuc.IsEnabled = !maPhongGiaoDich.Equals("%");
            }
        }

        private void LoadComboboxCum()
        {
            //// khởi tạo combobox
            //AutoComboBox auto = new AutoComboBox();

            //lstSourceCum_Select = new List<AutoCompleteEntry>();
            ////string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            ////string maPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
            //string maKhuVuc = lstSourceKhuVuc_Select.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings.First();

            //if (cmbKhuVuc.SelectedIndex > 0)
            //{
            //    string idKhuVuc = lstSourceKhuVuc_Select.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings[1];
            //    if (!maKhuVuc.Equals("%"))
            //        lstSourceCum_Select = lstSourceCum.Where(e => e.KeywordStrings.ElementAt(3).Equals(idKhuVuc)).ToList();
            //    //lstSourceCum_Select = lstSourceCum.Where(e => e.KeywordStrings.ElementAt(3).Substring(0, maKhuVuc.Length).Equals(maKhuVuc)).ToList();
            //}

            //lstSourceCum_Select.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
            //// khởi tạo combobox
            //auto = new AutoComboBox();
            //cmbCum.Items.Clear();
            //auto.GenAutoComboBox(ref lstSourceCum_Select, ref cmbCum, null);
            //cmbCum.SelectedIndex = 0;
            //cmbCum.IsEnabled = !maKhuVuc.Equals("%");
        }

        private void LoadGridViewCum()
        {
            if (cmbKhuVuc.SelectedIndex >= 0)
            {
                string maKhuVuc = lstSourceKhuVuc_Select.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings.First();
                string idKhuVuc = lstSourceKhuVuc_Select.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings[1];

                dtSourceCum_Select = new DataTable();
                var results = dtSourceCum.AsEnumerable().Select(x => x).Where(x => x.Field<int>("ID_KVUC").Equals(LNumber.StringToInt32(idKhuVuc)));
                if (results.Count() > 0) dtSourceCum_Select = results.CopyToDataTable();
                if (!LObject.IsNullOrEmpty(dtSourceCum_Select) && dtSourceCum_Select.Rows.Count > 0)
                {
                    grCum.ItemsSource = dtSourceCum_Select;
                    grCum.IsEnabled = true;
                }
                else grCum.ItemsSource = null;
            }
            else
            {
                grCum.ItemsSource = null;
                grCum.IsEnabled = false;
            }
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

            DateTime ngayChotSoLieu = new DateTime();
            if (raddtNgayChotSoLieu.Value is DateTime)
                ngayChotSoLieu = (DateTime)raddtNgayChotSoLieu.Value;

            DateTime ngayBaoCao = new DateTime();
            if (raddtNgayBaoCao.Value is DateTime)
                ngayBaoCao = (DateTime)raddtNgayBaoCao.Value;

            string tenKhuVuc = string.Empty;
            if (lstSourceKhuVuc_Select.Count > 0)
                tenKhuVuc = lstSourceKhuVuc_Select.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings[1];
            else
                tenKhuVuc = string.Empty;

            string maKhuVuc = string.Empty;
            if (lstSourceKhuVuc_Select.Count > 0)
                maKhuVuc = lstSourceKhuVuc_Select.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings.First();
            else
                maKhuVuc = string.Empty;

            lstMaCum = new List<string>();
            for (int i = 0; i < grCum.SelectedItems.Count; i++)
            {
                DataRow dr = (DataRow)grCum.SelectedItems[i];
                lstMaCum.Add(dr["MA_CUM"].ToString());
            }

            string maNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            string maDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();

            // Gán dữ liệu từ form vào các biến truyền cho báo cáo
            MaChiNhanh = maChiNhanh;
            TenChiNhanh = tenChiNhanh;
            MaPhongGiaoDich = maPhongGiaoDich;
            TenPhongGiaoDich = tenPhongGiaoDich;
            MaKhuVuc = maKhuVuc;
            TenKhuVuc = tenKhuVuc;
            NgayChotSoLieu = (raddtNgayChotSoLieu.Value is DateTime) ? ngayChotSoLieu.ToString("yyyyMMdd") : "";
            NgayBaoCao = (raddtNgayBaoCao.Value is DateTime) ? ngayBaoCao.ToString("yyyyMMdd") : "";
            MaNguoiLap = ClientInformation.TenDangNhap;
            TenNguoiLap = ClientInformation.HoTen;
            MaNgonNgu = maNgonNgu;
            MaDinhDang = maDinhDang;
        }

        private bool Validation(ref string msgResult)
        {
            bool bResult = true;

            if (MaPhongGiaoDich.Equals("")) { bResult = false; msgResult = "Chọn phòng giao dịch"; cmbPhongGD.Focus(); return bResult; }
            if (MaKhuVuc.Equals("") || MaKhuVuc.Equals("%")) { bResult = false; msgResult = "Chọn khu vực cụ thể"; cmbKhuVuc.Focus(); return bResult; }
            if (LObject.IsNullOrEmpty(lstMaCum) || lstMaCum.Count.Equals(0)) { bResult = false; msgResult = "Chọn cụm cụ thể"; grCum.Focus(); return bResult; }
            if (NgayChotSoLieu.Equals("")) { bResult = false; msgResult = "Chọn ngày chốt số liệu"; raddtNgayChotSoLieu.Focus(); return bResult; }
            if (NgayBaoCao.Equals("")) { bResult = false; msgResult = "Chọn ngày báo cáo"; raddtNgayBaoCao.Focus(); return bResult; }

            return bResult;
        }

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
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGiaoDich", MaPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaKhuVuc", MaKhuVuc, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            foreach (string maCum in lstMaCum)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaCum", maCum, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
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
