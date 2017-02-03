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
    public partial class TDVM_BANG_XAC_NHAN_SO_DU_VAY_VON : UserControl
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
        List<string> lstDanhSach = new List<string>();

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
        public string MauBaoCao;
        public string TenMauBaoCao;
        public string NgayChotDL;
        public string NgayBaoCao;

        public string MaNguoiLap;
        public string TenNguoiLap;

        public string MaNgonNgu;
        public string MaDinhDang;

        #endregion

        public TDVM_BANG_XAC_NHAN_SO_DU_VAY_VON()
        {
            InitializeComponent();
            LoadCombobox();

            cmbPhongGD.IsEnabled = false;
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

            raddtNgayPhatVon.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat).GetLastDateOfMonth();

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
            lstSourceChiNhanh.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);

            // khởi tạo combobox
            auto = new AutoComboBox();

            auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue());
            LoadComboboxPhongGD();
            LoadComboboxCum();

            lstSourceMauBaoCao = new List<AutoCompleteEntry>();
            lstSourceMauBaoCao.Add(new AutoCompleteEntry("Báo cáo theo Chi nhánh", "CN", "CN"));
            lstSourceMauBaoCao.Add(new AutoCompleteEntry("Báo cáo theo Phòng giao dịch", "PGD", "PGD"));
            lstSourceMauBaoCao.Add(new AutoCompleteEntry("Báo cáo theo Cụm", "CUM", "CUM"));
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
            string MaDonVi = string.Empty;
            if (cmbPhongGD.SelectedIndex >= 0 && !lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First().IsNullOrEmptyOrSpace())
                MaDonVi = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
            if (MaDonVi.Equals("%"))
                MaDonVi = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            DataSet ds = new BaoCaoProcess().GetCumTheoDonVi(MaDonVi);
            if (ds != null && ds.Tables.Count > 0)
                grCum.ItemsSource = ds.Tables[0];
            else
                grCum.ItemsSource = null;
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();
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
            string loaiBaoCao = lstSourceMauBaoCao.ElementAt(cmbMauBaoCao.SelectedIndex).KeywordStrings.First();

            string maPhongGiaoDich = string.Empty;
            string tenPhongGiaoDich = string.Empty;
            if (lstSourcePhongGD_Select.Count > 0)
            {
                maPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
                tenPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).DisplayName;
            }

            MaDonVi = (MaPhongGiaoDich.IsNullOrEmpty() || maPhongGiaoDich.Equals("%")) ? maChiNhanh : maPhongGiaoDich;

            DateTime ngayPhatVon = new DateTime();
            if (raddtNgayPhatVon.Value is DateTime)
                ngayPhatVon = (DateTime)raddtNgayPhatVon.Value;
            DateTime ngayBaoCao = new DateTime();
            if (raddtNgayBaoCao.Value is DateTime)
                ngayBaoCao = (DateTime)raddtNgayBaoCao.Value;

            lstDanhSach = new List<string>();
            if (loaiBaoCao.Equals("CUM") && grCum.SelectedItems.Count > 0)
            {
                for (int i = 0; i < grCum.SelectedItems.Count; i++)
                {
                    //DataRowView dr = (DataRowView)grCum.SelectedItems[i];
                    DataRow dr = (DataRow)grCum.SelectedItems[i];
                    lstDanhSach.Add(dr["MA_CUM"].ToString());
                }
            }
            else
            {
                for (int i = 0; i < grCum.Items.Count; i++)
                {
                    //DataRowView dr = (DataRowView)grCum.SelectedItems[i];
                    DataRow dr = (DataRow)grCum.Items[i];
                    lstDanhSach.Add(dr["MA_CUM"].ToString());
                }
            }

            string maNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            string maDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();

            // Gán dữ liệu từ form vào các biến truyền cho báo cáo
            MauBaoCao = lstSourceMauBaoCao.ElementAt(cmbMauBaoCao.SelectedIndex).KeywordStrings.First();
            TenMauBaoCao = lstSourceMauBaoCao.ElementAt(cmbMauBaoCao.SelectedIndex).DisplayName;
            MaChiNhanh = maChiNhanh;
            TenChiNhanh = tenChiNhanh;
            MaPhongGiaoDich = maPhongGiaoDich;
            TenPhongGiaoDich = tenPhongGiaoDich;
            NgayChotDL = ngayPhatVon.ToString("yyyyMMdd");
            NgayBaoCao = ngayBaoCao.ToString("yyyyMMdd");
            MaNguoiLap = ClientInformation.TenDangNhap;
            TenNguoiLap = ClientInformation.HoTen;
            MaNgonNgu = maNgonNgu;
            MaDinhDang = maDinhDang;
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
            else if (loaiBaoCao.Equals("CUM") && grCum.SelectedItems.Count < 1)
            {
                msg = "Chưa chọn Cụm cho báo cáo.";
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

            //if (MaCum.Equals("%"))
            //{
            //    if (MaChiNhanh.Equals("%"))
            //    {
            //        foreach (AutoCompleteEntry item in lstSourceCum)
            //        {
            //            string ma = item.KeywordStrings.First();
            //            if (!ma.Equals("%"))
            //                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaCum", ma, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            //        }
            //    }
            //    else
            //    {
            //        if (MaPhongGiaoDich.Equals("%"))
            //        {
            //            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            //            foreach (AutoCompleteEntry item in lstSourceCum.Where(e => e.KeywordStrings.ElementAt(1).Substring(0, maChiNhanh.Length).Equals(maChiNhanh)).ToList())
            //            {
            //                string ma = item.KeywordStrings.First();
            //                if (!ma.Equals("%"))
            //                    listThamSoBaoCao.Add(new ThamSoBaoCao("@MaCum", ma, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            //            }
            //        }
            //        else
            //        {
            //            foreach (AutoCompleteEntry item in lstSourceCum_Select)
            //            {
            //                string ma = item.KeywordStrings.First();
            //                if (!ma.Equals("%"))
            //                    listThamSoBaoCao.Add(new ThamSoBaoCao("@MaCum", ma, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            //            }
            //        }
            //    }
            //}
            //else
            //    listThamSoBaoCao.Add(new ThamSoBaoCao("@MaCum", MaCum, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            foreach (String item in lstDanhSach)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaCum", item, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
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
                if (maBaoCao.Equals("CN"))
                {
                    cmbPhongGD.SelectedIndex = 0;
                    cmbPhongGD.IsEnabled = false;
                    grbCum.Visibility = Visibility.Collapsed;
                }
                else if (maBaoCao.Equals("PGD"))
                {
                    cmbPhongGD.IsEnabled = true;
                    grbCum.Visibility = Visibility.Collapsed;
                }
                else if (maBaoCao.Equals("CUM"))
                {
                    cmbPhongGD.IsEnabled = true;
                    grbCum.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
