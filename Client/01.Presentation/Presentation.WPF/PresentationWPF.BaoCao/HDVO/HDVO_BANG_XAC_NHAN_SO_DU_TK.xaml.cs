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
using System.Data;
using Presentation.Process;
using Presentation.Process.Common;

namespace PresentationWPF.BaoCao.HDVO
{
    /// <summary>
    /// Interaction logic for ucXacNhanSoDuTietKiem.xaml
    /// </summary>
    public partial class HDVO_BANG_XAC_NHAN_SO_DU_TK : UserControl
    {
        #region Khai bao

        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiBaoCao = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        List<string> lstDanhSach = new List<string>();

        // Các tham số báo cáo từ form điều kiện
        public string MaBaoCao;
        public string MaChiNhanh;
        public string TenChiNhanh;
        public string MaPhongGiaoDich;
        public string TenPhongGiaoDich;
        public string NgayBaoCao;
        public string MaNguoiLap;
        public string TenNguoiLap;
        public string LoaiBaoCao;

        public string MaLoaiTien;
        public string TuNgay;
        public string DenNgay;

        public string MaNgonNgu;
        public string MaDinhDang;

        #endregion

        public HDVO_BANG_XAC_NHAN_SO_DU_TK()
        {
            InitializeComponent();
            LoadCombobox();
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
            raddtDenNgay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat).GetLastDateOfMonth();
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
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);

            // khởi tạo combobox
            auto = new AutoComboBox();

            auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue());
            LoadComboboxPhongGD();
            GetCum();

            lstSourceLoaiBaoCao = new List<AutoCompleteEntry>();
            lstSourceLoaiBaoCao.Add(new AutoCompleteEntry("Bảng xác nhận số dư tiền gửi tiết kiệm", "B01", "B01"));
            lstSourceLoaiBaoCao.Add(new AutoCompleteEntry("Bảng tổng hợp số dư tiền gửi tiết kiệm theo cụm", "B02", "B02"));
            lstSourceLoaiBaoCao.Add(new AutoCompleteEntry("Bảng tổng hợp số dư tiền gửi tiết kiệm theo Văn phòng CN/PGD", "B03", "B03"));
            lstSourceLoaiBaoCao.Add(new AutoCompleteEntry("Bảng xác nhận số dư tiền gửi tiết kiệm (TKBB và TKKKH)", "B04", "B04"));
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceLoaiBaoCao, ref cmbLoaiBaoCao, null);
            cmbLoaiBaoCao.SelectedIndex = 0;

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.PDF.layGiaTri());
        }

        private void LoadComboboxPhongGD()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();

            lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();
            lstSourcePhongGD_Select.Insert(0, new AutoCompleteEntry("Tất cả", "%", ""));

            // khởi tạo combobox
            auto = new AutoComboBox();
            cmbPhongGD.Items.Clear();
            auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
            cmbPhongGD.SelectedIndex = 0;
            cmbPhongGD.IsEnabled = !maChiNhanh.Equals("%");
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();
            GetCum();
        }

        private void GetCum()
        {
            string MaDonVi = string.Empty;
            if (cmbPhongGD.SelectedIndex >= 0 && !lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First().IsNullOrEmptyOrSpace())
                MaDonVi = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
            if(MaDonVi.Equals("%"))
                MaDonVi = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            DataSet ds = new BaoCaoProcess().GetCumTheoDonVi(MaDonVi);
            if (ds != null && ds.Tables.Count > 0)
                grCum.ItemsSource = ds.Tables[0];
            else
                grCum.ItemsSource = null;
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetCum();
        }

        private void GetFormData()
        {
            string maNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            string maDonVi = string.Empty;
            string tenPhongGiaoDich = string.Empty;
            if (lstSourcePhongGD_Select.Count > 0)
            {
                maDonVi = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
                tenPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).DisplayName;
            }
            else
                maDonVi = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();


            DateTime ngayBaoCao = new DateTime();
            if (raddtNgayBaoCao.Value is DateTime)
                ngayBaoCao = (DateTime)raddtNgayBaoCao.Value;
            DateTime tuNgay = new DateTime();
            DateTime denNgay = new DateTime();
            if (raddtDenNgay.Value is DateTime)
                denNgay = (DateTime)raddtDenNgay.Value;

            string maDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            string loaiBaoCao = lstSourceLoaiBaoCao.ElementAt(cmbLoaiBaoCao.SelectedIndex).KeywordStrings.First();
            string tenChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).DisplayName;

            lstDanhSach = new List<string>();
            if (loaiBaoCao.Equals("B01") && grCum.SelectedItems.Count > 0)
            {
                for (int i = 0; i < grCum.SelectedItems.Count; i++)
                {
                    //DataRowView dr = (DataRowView)grCum.SelectedItems[i];
                    DataRow dr = (DataRow)grCum.SelectedItems[i];
                    lstDanhSach.Add(dr["MA_CUM"].ToString());
                }
            }
            else if (loaiBaoCao.Equals("B02") || loaiBaoCao.Equals("B03"))
            {
                for (int i = 0; i < grCum.Items.Count; i++)
                {
                    //DataRowView dr = (DataRowView)grCum.SelectedItems[i];
                    DataRow dr = (DataRow)grCum.Items[i];
                    lstDanhSach.Add(dr["MA_CUM"].ToString());
                }
            }
            else if (loaiBaoCao.Equals("B04") && grCum.SelectedItems.Count > 0)
            {
                for (int i = 0; i < grCum.SelectedItems.Count; i++)
                {
                    //DataRowView dr = (DataRowView)grCum.SelectedItems[i];
                    DataRow dr = (DataRow)grCum.SelectedItems[i];
                    lstDanhSach.Add(dr["MA_CUM"].ToString());
                }
            }
            //Lấy giá trị
            MaChiNhanh = maChiNhanh;
            MaBaoCao = loaiBaoCao;
            TenChiNhanh = tenChiNhanh;
            MaPhongGiaoDich = maDonVi;
            TenPhongGiaoDich = tenPhongGiaoDich;
            TuNgay = tuNgay.ToString("yyyyMMdd");
            DenNgay = denNgay.ToString("yyyyMMdd");
            NgayBaoCao = ngayBaoCao.ToString("yyyyMMdd");
            MaNgonNgu = maNgonNgu;
            MaDinhDang = maDinhDang;
            MaNguoiLap = Presentation.Process.Common.ClientInformation.TenDangNhap;
            TenNguoiLap = Presentation.Process.Common.ClientInformation.HoTen;
        }

        private bool Validation(ref string msg)
        {
            string loaiBaoCao = lstSourceLoaiBaoCao.ElementAt(cmbLoaiBaoCao.SelectedIndex).KeywordStrings[0];
            string maPGD = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
            if ((loaiBaoCao.Equals("B01") || loaiBaoCao.Equals("B04")) && maPGD.Equals("%"))
            {
                msg = "Chưa chọn Phòng giao dịch cho báo cáo.";
                return false;
            }
            else if ((loaiBaoCao.Equals("B01") || loaiBaoCao.Equals("B04")) && grCum.SelectedItems.Count < 1)
            {
                msg = "Chưa chọn cụm cho báo cáo.";
                return false;
            }
            else if (loaiBaoCao.Equals("B02") && maPGD.Equals("%"))
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
                if(msg.Equals(""))
                    LMessage.ShowMessage("Thiếu tham số cho báo cáo.", LMessage.MessageBoxType.Information);
                else
                    LMessage.ShowMessage(msg, LMessage.MessageBoxType.Information);
                return null;
            }

            GetFormData();

            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();

            if (MaChiNhanh.Equals("%"))
            {
                foreach (AutoCompleteEntry item in lstSourceChiNhanh)
                {
                    string ma = item.KeywordStrings.First();
                    if (!ma.Equals("%"))
                        listThamSoBaoCao.Add(new ThamSoBaoCao("@DSChiNhanh", ma, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                }
            }
            else
                listThamSoBaoCao.Add(new ThamSoBaoCao("@DSChiNhanh", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            if (MaPhongGiaoDich.Equals("%"))
            {
                foreach (AutoCompleteEntry item in lstSourcePhongGD_Select)
                {
                    string ma = item.KeywordStrings.First();
                    if (!ma.Equals("%"))
                        listThamSoBaoCao.Add(new ThamSoBaoCao("@DSPhongGD", ma, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                }
            }
            else
                listThamSoBaoCao.Add(new ThamSoBaoCao("@DSPhongGD", MaPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            foreach (String item in lstDanhSach)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@DSCum", item, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DenNgay", DenNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@LoaiTien", "VND", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DonViTinh", numDonViTinh.Value.ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@LoaiBaoCao", MaBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            return listThamSoBaoCao;
        }

        private void cmbLoaiBaoCao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string loaiBaoCao = lstSourceLoaiBaoCao.ElementAt(cmbLoaiBaoCao.SelectedIndex).KeywordStrings[0];
            if (loaiBaoCao.Equals("B01"))
            {
                grbCum.Visibility = Visibility.Visible;
                cmbPhongGD.IsEnabled = true;
            }
            else if (loaiBaoCao.Equals("B02"))
            {
                grbCum.Visibility = Visibility.Collapsed;
                cmbPhongGD.IsEnabled = true;
            }
            else if (loaiBaoCao.Equals("B03"))
            {
                grbCum.Visibility = Visibility.Collapsed;
                cmbPhongGD.SelectedIndex = 0;
                cmbPhongGD.IsEnabled = false;
            }
            else if (loaiBaoCao.Equals("B04"))
            {
                grbCum.Visibility = Visibility.Visible;
                cmbPhongGD.IsEnabled = true;
            }
        }
    }
}
