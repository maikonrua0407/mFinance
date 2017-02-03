using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
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

namespace PresentationWPF.BaoCao.GDKT
{
    /// <summary>
    /// Interaction logic for ucCanDoiKeToanHopNhat.xaml
    /// </summary>
    public partial class GDKT_KET_XUAT_BCDKT_THEO_CAU_TRUC : UserControl
    {
        #region khai bao
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();

        public string MaToChuc = "";
        public string MaDonViQuanLy = "";
        public string MaChiNhanh = "";
        public string TuNgay = "";
        public string DenNgay = "";
        public string NgayBaoCao = "";
        public int CapTaiKhoan = 4;
        public int LanTaoThu = 1;
        public string MaNgonNgu = "";
        public string MaDinhDang = "";
        #endregion

        public GDKT_KET_XUAT_BCDKT_THEO_CAU_TRUC()
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

            cmbDinhDang.IsEnabled = false;

            raddtNgayBaoCao.Value = DateTime.Now;
            raddtThangBaoCao.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");

            //cmbChiNhanh.IsEnabled = false;
        }

        /// <summary>
        /// Khoi tao combobox
        /// </summary>
        private void LoadCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();

            //Tạo combox chi nhanh
            auto = new AutoComboBox();
            lstSourceChiNhanh.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);
            //if (lstSourceChiNhanh.Count > 0) cmbChiNhanh.SelectedIndex = 0;

            //Tao combobox ngon ngu bao cao
            auto = new AutoComboBox();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
            auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());

            //Tao combobox dinh dang bao cao
            auto = new AutoComboBox();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.TEXT.layGiaTri());
        }

        /// <summary>
        /// Lay cac gia tri tren form 
        /// </summary>
        private void GetFormValues()
        {
            MaChiNhanh = "%";

            //Lay thong tin ma don vi lon nhat
            MaToChuc = ClientInformation.MaToChuc;

            //lay thong tin ma don vi quan ly
            MaDonViQuanLy = ClientInformation.MaDonViQuanLy;

            //Lay thong tin ma chi nhanh
            MaChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();

            if (raddtThangBaoCao.Value == null)
            {
                LMessage.ShowMessage("Chọn tháng báo cáo!", LMessage.MessageBoxType.Error);
                raddtThangBaoCao.Focus();
                return;
            }

            if (raddtNgayBaoCao.Value == null)
            {
                LMessage.ShowMessage("Chọn ngày báo cáo!", LMessage.MessageBoxType.Error);
                raddtNgayBaoCao.Focus();
                return;
            }

            //Lay gia tri tungay, denngay
            DateTime dtNgayBC = new DateTime();
            if (raddtThangBaoCao.Value is DateTime)
            {
                dtNgayBC = raddtThangBaoCao.Value != null ? (DateTime)raddtThangBaoCao.Value : DateTime.Now;
                NgayBaoCao = raddtNgayBaoCao.Value != null ? ((DateTime)raddtNgayBaoCao.Value).ToString("yyyyMMdd") : DateTime.Now.ToString("yyyyMMdd");
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

            //lay thong tin nguoi dang nhap
            //MaNguoiLap = ClientInformation.TenDangNhap;

            //lay thong tin ten nguoi dang nhap
            //TenNguoiLap = ClientInformation.HoTen;

            //lay thong tin ma mau bieu
            //MaMauBieu = "BCTK_BANG_CAN_DOI";

            //Lay thong tin ten don vi
            //TenChiNhanh = ClientInformation.TenDonVi;

            CapTaiKhoan = LNumber.StringToInt32(nudCap.Value.ToString());
            LanTaoThu = LNumber.StringToInt32(nudLanTaoThu.Value.ToString());

            //lay thong tin loai ngon ngu
            MaNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();

            //lay thong tin ma dinh dang
            MaDinhDang = ApplicationConstant.LoaiDinhDangBaoCao.TEXT.layGiaTri(); //lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();
        }

        public List<ThamSoBaoCao> GetParameters()
        {
            GetFormValues();
            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();

            if (MaChiNhanh.Equals("%")) //Toàn hệ thống
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaDonVi", "%", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaNHNN", MaToChuc, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            else //từng chi nhánh
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaDonVi", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaNHNN", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TuNgay", TuNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DenNgay", DenNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@CapTaiKhoan", CapTaiKhoan.ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@LanTaoThu", LanTaoThu.ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDingDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));
            return listThamSoBaoCao;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void OnShowResult(string ketqua)
        {

            //if (!LObject.IsNullOrEmpty(paras) && paras.Length > 0) FileKetQua = (String)paras[0];
            if (!ketqua.Equals(""))
            {
                txtKetQua.Text = ketqua;
            }
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


    }
}
