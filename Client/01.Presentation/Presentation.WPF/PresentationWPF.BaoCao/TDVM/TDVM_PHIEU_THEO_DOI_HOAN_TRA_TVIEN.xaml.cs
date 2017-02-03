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
using System.IO;
using System.Diagnostics;

namespace PresentationWPF.BaoCao.TDVM
{
    /// <summary>
    /// Interaction logic for ucPhieuTheoDoiHoanTraTV.xaml
    /// </summary>
    public partial class TDVM_PHIEU_THEO_DOI_HOAN_TRA_TVIEN : UserControl
    {
        #region Khai bao

        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKheUoc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceSanPham = new List<AutoCompleteEntry>();
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
        public string NgayBaoCao;
        public string SoKheUoc;

        public string MaNguoiLap;
        public string TenNguoiLap;

        public string MaNgonNgu;
        public string MaDinhDang;

        #endregion

        public TDVM_PHIEU_THEO_DOI_HOAN_TRA_TVIEN()
        {
            InitializeComponent();
            LoadCombobox();
            cmbChiNhanh.SelectionChanged +=new SelectionChangedEventHandler(cmbChiNhanh_SelectionChanged);
            cmbPhongGD.SelectionChanged +=new SelectionChangedEventHandler(cmbPhongGD_SelectionChanged);
            cmbSanPham.SelectionChanged += new SelectionChangedEventHandler(cmbSanPham_SelectionChanged);
            cmbCum.SelectionChanged += cmbCum_SelectionChanged;
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

        }

        void cmbSanPham_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            grKheUoc.ItemsSource = null;
            grKheUoc.Rebind();
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
            auto.GenAutoComboBox(ref lstSourceCum, ref cmbCum, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM_ALL.getValue());
            LoadComboboxPhongGD();
            LoadComboboxSanPham();
            LoadComboboxCum();

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

        private void LoadComboboxSanPham()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            lstSourceSanPham = new List<AutoCompleteEntry>();
            lstSourceSanPham.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            lstDieuKien.Add("'" + maChiNhanh + "'");
            cmbSanPham.Items.Clear();
            auto.GenAutoComboBox(ref lstSourceSanPham, ref cmbSanPham, "COMBOBOX_SAN_PHAM_TD", lstDieuKien);
            cmbSanPham.IsEnabled = !maChiNhanh.Equals("%");
            
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

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();
            LoadComboboxSanPham();
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPhongGD.SelectedIndex >= 0)
                LoadComboboxCum();
        }

        private void cmbCum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HienThiDuLieuKhachHang();
        }

        /// <summary>
        /// Sự kiện click button hiển thị popup khách hàng chưa phải là thành viên
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMaKhachHang_Click(object sender, RoutedEventArgs e)
        {
            HienPopUpKhachHang();
        }

        private void HienThiDuLieuKhachHang()
        {
            BaoCaoProcess baocaoProcess = new BaoCaoProcess();
            List<string> lstDK = new List<string>();
            string maCum = "";
            if (cmbCum.SelectedIndex >= 0)
                maCum = lstSourceCum_Select.ElementAt(cmbCum.SelectedIndex).KeywordStrings.ElementAt(0);
            DataSet ds = baocaoProcess.GetKhachHangTheoCum(maCum);
            if (!ds.IsNullOrEmpty() && ds.Tables.Count > 0)
                grKhachHang.ItemsSource = ds.Tables[0];
            else
                grKhachHang.ItemsSource = null;
        }

        /// <summary>
        /// Hiện popup khách hàng
        /// </summary>
        private void HienPopUpKhachHang()
        {
            PopupProcess Popupprocess = new PopupProcess();
            List<string> lstDK = new List<string>();
            string maDV = "";
            if (cmbChiNhanh.SelectedIndex >= 0)
                maDV = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.ElementAt(0);
            string maPGD = "";
            if (cmbChiNhanh.SelectedIndex >= 0)
                maPGD = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.ElementAt(0);
            string maCum = "";
            if (cmbCum.SelectedIndex >= 0)
                maCum = lstSourceCum_Select.ElementAt(cmbCum.SelectedIndex).KeywordStrings.ElementAt(0);
            lstDK.Add(maDV);
            lstDK.Add(maPGD);
            lstDK.Add(maCum);
            Popupprocess.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_KHACHHANG_BAOCAO.getValue(), lstDK);
            SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;
            ucPopup popup = new ucPopup(true, simplePopupResponse, false);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.Title = "Danh sách nhóm khách hàng";
            win.Content = popup;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ShowDialog();
            if (lstPopup != null && lstPopup.Count > 0)
            {
                txtMaKhachHang.Text = lstPopup[0][2].ToString();
            }
            cmbKheUoc.Items.Clear();
            if (!txtMaKhachHang.Text.IsNullOrEmptyOrSpace())
            {
                lstSourceKheUoc = new List<AutoCompleteEntry>();
                AutoComboBox auto = new AutoComboBox();
                // Khởi tạo điều kiện gọi danh mục
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(txtMaKhachHang.Text);
                // khởi tạo combobox
                auto = new AutoComboBox();
                auto.GenAutoComboBox(ref lstSourceKheUoc, ref cmbKheUoc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KUOC_KHANG.getValue(), lstDieuKien);
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

            DateTime ngayBaoCao = new DateTime();
            if (raddtNgayBaoCao.Value is DateTime)
                ngayBaoCao = (DateTime)raddtNgayBaoCao.Value;

            string maCum = string.Empty;
            if (lstSourceCum_Select.Count > 0)
                maCum = lstSourceCum_Select.ElementAt(cmbCum.SelectedIndex).KeywordStrings.First();
            else
                maCum = string.Empty;

            //string soKheUoc = string.Empty;
            //if (lstSourceKheUoc.Count > 0)
            //    soKheUoc = lstSourceKheUoc.ElementAt(cmbKheUoc.SelectedIndex).KeywordStrings.First();
            //else
            //    soKheUoc = string.Empty;

            string maNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            string maDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();

            // Gán dữ liệu từ form vào các biến truyền cho báo cáo
            MaChiNhanh = maChiNhanh;
            TenChiNhanh = tenChiNhanh;
            MaPhongGiaoDich = maPhongGiaoDich;
            TenPhongGiaoDich = tenPhongGiaoDich;
            MaCum = maCum;
            //SoKheUoc = soKheUoc;
            NgayBaoCao = ngayBaoCao.ToString("yyyyMMdd");
            MaNguoiLap = ClientInformation.TenDangNhap;
            TenNguoiLap = ClientInformation.HoTen;
            MaNgonNgu = maNgonNgu;
            MaDinhDang = maDinhDang;
        }

        private bool Validation()
        {
            if (LObject.IsNullOrEmpty(grKhachHang.ItemsSource) || (!LObject.IsNullOrEmpty(grKhachHang) && grKhachHang.SelectedItems.Count == 0))
            {
                LMessage.ShowMessage("Thiếu thông tin khách hàng thực hiện báo cáo", LMessage.MessageBoxType.Warning);
                return false;
            }
            else if (LObject.IsNullOrEmpty(grKheUoc.ItemsSource) || (!LObject.IsNullOrEmpty(grKheUoc) && grKheUoc.SelectedItems.Count == 0))
            {
                LMessage.ShowMessage("Thiếu thông tin khế ước thực hiện báo cáo", LMessage.MessageBoxType.Warning);
                return false;
            }
            return true;
        }

        public DatabaseConstant.Action GetAction() { return DatabaseConstant.Action.IN_CHUNG; }

        public List<ThamSoBaoCao> GetParameters()
        {
            if (!Validation())
            {
                return null;
            }

            GetFormData();

            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGD", MaPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaCum", MaCum, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            for (int i = 0; i < grKhachHang.SelectedItems.Count; i++)
            {
                DataRow dr = (DataRow)grKhachHang.SelectedItems[i];
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaKhachHang", dr["MA_KHANG"].ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            //listThamSoBaoCao.Add(new ThamSoBaoCao("@MaKhachHang", txtMaKhachHang.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            for (int i = 0; i < grKheUoc.SelectedItems.Count; i++)
            {
                DataRow dr = (DataRow)grKheUoc.SelectedItems[i];
                listThamSoBaoCao.Add(new ThamSoBaoCao("@DSSoKheUoc", dr["MA_KUOCVM"].ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            //listThamSoBaoCao.Add(new ThamSoBaoCao("@SoKheUoc", SoKheUoc, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            return listThamSoBaoCao;
        }

        private void grKhachHang_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            BaoCaoProcess baocaoProcess = new BaoCaoProcess();
            string maKhachHang = "";
            string maSanPham = lstSourceSanPham.ElementAt(cmbSanPham.SelectedIndex).KeywordStrings.FirstOrDefault();
            for (int i = 0; i < grKhachHang.SelectedItems.Count; i++)
            {
                DataRow dr = (DataRow)grKhachHang.SelectedItems[i];
                maKhachHang = maKhachHang + ",'" + dr["MA_KHANG"].ToString() + "'";
            }
            if (!maKhachHang.IsNullOrEmptyOrSpace())
                maKhachHang = maKhachHang.Substring(1, maKhachHang.Length - 1);
            DataSet ds = baocaoProcess.GetKheUocKhachHang(maKhachHang,maSanPham);
            if (!ds.IsNullOrEmpty() && ds.Tables.Count > 0)
                grKheUoc.ItemsSource = ds.Tables[0];
            else
                grKheUoc.ItemsSource = null;
        }

        public void OnShowResult(string ketqua)
        {

            //if (!LObject.IsNullOrEmpty(paras) && paras.Length > 0) FileKetQua = (String)paras[0];
            if (!ketqua.Equals(""))
            {
                txtKetQua.Text = ketqua;
            }
            Mouse.OverrideCursor = Cursors.Arrow;
            return;
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            if (!txtKetQua.Text.Trim().Equals(""))
            {
                if (File.Exists(txtKetQua.Text.Trim()))
                {
                    string fileToSelect = txtKetQua.Text.Trim();
                    string args = string.Format("/Select, {0}", fileToSelect);

                    ProcessStartInfo pfi = new ProcessStartInfo("Explorer.exe", args);
                    System.Diagnostics.Process.Start(pfi);
                }
            }
        }

        private void txtKetQua_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnBrowse_Click(sender, e);
            }
        }
    }
}
