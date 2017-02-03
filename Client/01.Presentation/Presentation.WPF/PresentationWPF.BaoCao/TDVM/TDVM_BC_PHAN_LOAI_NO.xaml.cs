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
using System.Data;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;

namespace PresentationWPF.BaoCao.TDVM
{
    /// <summary>
    /// Interaction logic for ucDSKhachHangNhanVon.xaml
    /// </summary>
    public partial class TDVM_BC_PHAN_LOAI_NO : UserControl
    {
                
        #region Khai bao

        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceMauBaoCao = new List<AutoCompleteEntry>();
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
        public string MauBaoCao;
        public string TenMauBaoCao;
        public string NgayChotDL;
        public string NgayBaoCao;

        public string MaNguoiLap;
        public string TenNguoiLap;

        public string MaNgonNgu;
        public string MaDinhDang;

        #endregion

        public TDVM_BC_PHAN_LOAI_NO()
        {
            InitializeComponent();
            LoadCombobox();
            GetSanPhamTD();
            cmbPhongGD.IsEnabled = false;
            cmbChiNhanh.IsEnabled = false;

            raddtNgayPhatVon.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");

            cmbMauBaoCao.SelectionChanged += cmbMauBaoCao_SelectionChanged;
            cmbChiNhanh.SelectionChanged += cmbChiNhanh_SelectionChanged;
            cmbPhongGD.SelectionChanged += cmbPhongGD_SelectionChanged;
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
            if (!(ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()) ||
                ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri())))
            {
                lstSourceChiNhanh.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
            }
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);

            // khởi tạo combobox
            auto = new AutoComboBox();

            auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue());
            auto.GenAutoComboBox(ref lstSourceCum, ref cmbCum, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM_ALL.getValue());
            LoadComboboxPhongGD();
            LoadComboboxCum();

            lstSourceMauBaoCao = new List<AutoCompleteEntry>();
            if (!(ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()) ||
                ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri())))
            {
                lstSourceMauBaoCao.Add(new AutoCompleteEntry("Báo cáo toàn M7", "M7", "M7"));
            }
            lstSourceMauBaoCao.Add(new AutoCompleteEntry("Báo cáo theo Chi nhánh", "CN", "CN"));
            lstSourceMauBaoCao.Add(new AutoCompleteEntry("Báo cáo theo Phòng giao dịch", "PGD", "PGD"));
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceMauBaoCao, ref cmbMauBaoCao, null);
            cmbMauBaoCao.SelectedIndex = 0;

            // khởi tạo combobox
            auto = new AutoComboBox();         

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());

            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.PDF.layGiaTri());

            cmbChiNhanh_SelectionChanged(null, null);
        }

        private void LoadComboboxPhongGD()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();

            lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            if (!maChiNhanh.Equals("%"))
                lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();
            lstSourcePhongGD_Select.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));

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
            lstSourceCum_Select.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
            // khởi tạo combobox
            auto = new AutoComboBox();
            cmbCum.Items.Clear();
            auto.GenAutoComboBox(ref lstSourceCum_Select, ref cmbCum, null);
            cmbCum.SelectedIndex = 0;
            cmbCum.IsEnabled = !maPhongGiaoDich.Equals("%");
        }

        private void GetSanPhamTD()
        {
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            DateTime ngayPhatVon = new DateTime();
            if (raddtNgayPhatVon.Value is DateTime)
                ngayPhatVon = (DateTime)raddtNgayPhatVon.Value;
            string ngayDL = ngayPhatVon.ToString("yyyyMMdd");
            DataSet ds = new BaoCaoProcess().GetSPTinDungTheoDonVi(maChiNhanh, ngayDL);
            if (!ds.IsNullOrEmpty() && ds.Tables.Count > 0)
            {
                grSanPham.ItemsSource = ds.Tables[0];
                grSanPham.Rebind();
                grSanPham.SelectAll();
            }
            else
                grSanPham.ItemsSource = null;
            cmbMauBaoCao_SelectionChanged(null, null);
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();
            GetSanPhamTD();
            cmbMauBaoCao_SelectionChanged(sender, e);
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPhongGD.SelectedIndex >= 0)
                LoadComboboxCum();
            cmbMauBaoCao_SelectionChanged(sender, e);
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

            DateTime ngayPhatVon = new DateTime();
            if (raddtNgayPhatVon.Value is DateTime)
                ngayPhatVon = (DateTime)raddtNgayPhatVon.Value;
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

            string maNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            string maDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();

            // Gán dữ liệu từ form vào các biến truyền cho báo cáo
            MauBaoCao = lstSourceMauBaoCao.ElementAt(cmbMauBaoCao.SelectedIndex).KeywordStrings.First();
            TenMauBaoCao = lstSourceMauBaoCao.ElementAt(cmbMauBaoCao.SelectedIndex).DisplayName;
            MaChiNhanh = maChiNhanh;
            TenChiNhanh = tenChiNhanh;
            MaPhongGiaoDich = maPhongGiaoDich;
            TenPhongGiaoDich = tenPhongGiaoDich;
            MaCum = maCum;
            TenCum = tenCum;
            NgayChotDL = ngayPhatVon.ToString("yyyyMMdd");
            NgayBaoCao = ngayBaoCao.ToString("yyyyMMdd");
            MaNguoiLap = ClientInformation.TenDangNhap;
            TenNguoiLap = ClientInformation.HoTen;
            MaNgonNgu = maNgonNgu;
            MaDinhDang = maDinhDang;

            if (MauBaoCao.Equals("M7"))
            {
                MaDonVi = DatabaseConstant.MA_TOCHUC;
            }
            else if (MauBaoCao.Equals("CN"))
            {
                MaDonVi = MaChiNhanh;
            }
            else if (MauBaoCao.Equals("PGD"))
            {
                MaDonVi = MaChiNhanh;
            }
        }

        private bool Validation(ref string msg)
        {
            string loaiBaoCao = lstSourceMauBaoCao.ElementAt(cmbMauBaoCao.SelectedIndex).KeywordStrings[0];
            string maCN = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            string maPGD = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
            if (loaiBaoCao.Equals("CN") && maCN.Equals("%"))
            {
                msg = "Chưa chọn Chi nhánh cho báo cáo.";
                return false;
            }
            else if (loaiBaoCao.Equals("PGD") && maPGD.Equals("%"))
            {
                msg = "Chưa chọn Phòng giao dịch cho báo cáo.";
                return false;
            }
            return true;
        }

        public List<ThamSoBaoCao> GetParameters()
        {
            string msg = "";
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

            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaDonVi", MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            if (MaChiNhanh.Equals("%"))
            {
                foreach (AutoCompleteEntry item in lstSourceChiNhanh)
                {
                    string ma = item.KeywordStrings.First();
                    if (!ma.Equals("%"))
                        listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", ma, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                }
            }
            else
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            if (MaPhongGiaoDich.Equals("%"))
            {
                if (MaChiNhanh.Equals("%"))
                {
                    foreach (AutoCompleteEntry item in lstSourcePhongGD)
                    {
                        string ma = item.KeywordStrings.First();
                        if (!ma.Equals("%"))
                            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGD", ma, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    }
                }
                else
                {
                    foreach (AutoCompleteEntry item in lstSourcePhongGD_Select)
                    {
                        string ma = item.KeywordStrings.First();
                        if (!ma.Equals("%"))
                            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGD", ma, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    }
                }
            }
            else
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGD", MaPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            if (MaCum.Equals("%"))
            {
                if (MaChiNhanh.Equals("%"))
                {
                    foreach (AutoCompleteEntry item in lstSourceCum)
                    {
                        string ma = item.KeywordStrings.First();
                        if (!ma.Equals("%"))
                            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaCum", ma, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    }
                }
                else
                {
                    if (MaPhongGiaoDich.Equals("%"))
                    {
                        string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                        foreach (AutoCompleteEntry item in lstSourceCum.Where(e => e.KeywordStrings.ElementAt(1).Substring(0, maChiNhanh.Length).Equals(maChiNhanh)).ToList())
                        {
                            string ma = item.KeywordStrings.First();
                            if (!ma.Equals("%"))
                                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaCum", ma, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                        }
                    }
                    else
                    {
                        foreach (AutoCompleteEntry item in lstSourceCum_Select)
                        {
                            string ma = item.KeywordStrings.First();
                            if (!ma.Equals("%"))
                                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaCum", ma, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                        }
                    }
                }
            }
            else
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaCum", MaCum, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            for (int i = 0; i < grSanPham.SelectedItems.Count; i++)
            {
                DataRow dr = (DataRow)grSanPham.SelectedItems[i];
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaSanPham", dr["MA_SAN_PHAM"].ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MauBaoCao", MauBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TenBaoCao", TenMauBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayChotDL", NgayChotDL, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));


            return listThamSoBaoCao;
        }

        private void cmbMauBaoCao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!lstSourceMauBaoCao.IsNullOrEmpty() && lstSourceMauBaoCao.Count > 0)
            {
                string maBaoCao = lstSourceMauBaoCao.ElementAt(cmbMauBaoCao.SelectedIndex).KeywordStrings.First();
                if (maBaoCao.Equals("M7"))
                {
                    cmbChiNhanh.SelectedIndex = 0;
                    cmbChiNhanh.IsEnabled = false;
                    cmbPhongGD.SelectedIndex = 0;
                    cmbPhongGD.IsEnabled = false;
                }
                else if (maBaoCao.Equals("CN"))
                {
                    if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()) ||
                        ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()))
                    {
                        cmbChiNhanh.IsEnabled = false;
                    }
                    else
                    {
                        cmbChiNhanh.IsEnabled = true;
                    }
                    cmbPhongGD.SelectedIndex = 0;
                    cmbPhongGD.IsEnabled = false;
                }
                else if (maBaoCao.Equals("PGD"))
                {
                    if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()) ||
                        ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()))
                    {
                        cmbChiNhanh.IsEnabled = false;
                    }
                    else
                    {
                        cmbChiNhanh.IsEnabled = true;
                    }
                    cmbPhongGD.IsEnabled = true;
                }
            }
        }

        private void grSanPham_Loaded(object sender, RoutedEventArgs e)
        {
            grSanPham.SelectAll();
        }
    }
}
