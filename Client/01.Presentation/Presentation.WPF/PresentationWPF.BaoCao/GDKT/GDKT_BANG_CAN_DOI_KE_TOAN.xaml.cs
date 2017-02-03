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
using System.Collections;
using System.Reflection;

namespace PresentationWPF.BaoCao.GDKT
{
    /// <summary>
    /// Interaction logic for ucCanDoiKeToanHopNhat.xaml
    /// </summary>
    public partial class GDKT_BANG_CAN_DOI_KE_TOAN : UserControl
    {
        #region khai bao
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();

        public string MaToChuc = "";
        public string MaDonViQuanLy = "";
        public string MaChiNhanh = "";
        public string MaPhongGD = "";
        public string TuNgay = "";
        public string DenNgay = "";
        public string NgayBaoCao = "";
        public string MaMauBieu = "";
        public string MaNguoiLap = "";
        public string TenNguoiLap = "";
        public string TenChiNhanh = "";
        public string MaNgonNgu = "";
        public string MaDinhDang = "";
        public string DinhKyBC = "THANG";
        public string QuyBCao = "1";
        #endregion

        public GDKT_BANG_CAN_DOI_KE_TOAN()
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
            ShowControl();
            raddtNgayBaoCao.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai,ApplicationConstant.defaultDateTimeFormat);
            raddtThangChot.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            cmbDinhKy.SelectionChanged += new SelectionChangedEventHandler(cmbDinhKy_SelectionChanged);
        }

        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.BaoCao.GDKT.GDKT_BANG_CAN_DOI_KE_TOAN", "");
            foreach (List<string> lst in arr)
            {
                object item = grMain.FindName(lst.First());
                string strProperty = lst.ElementAt(1);
                PropertyInfo prty = item.GetType().GetProperty(strProperty);
                if (strProperty.Equals("Visibility"))
                {
                    if (lst.ElementAt(2).Equals("0"))
                        prty.SetValue(item, Visibility.Collapsed, null);
                    else if (lst.ElementAt(2).Equals("1"))
                        prty.SetValue(item, Visibility.Visible, null);
                    else
                        prty.SetValue(item, Visibility.Hidden, null);
                }
                else
                {
                    if (lst.ElementAt(2).Equals("0"))
                        prty.SetValue(item, false, null);
                    else
                        prty.SetValue(item, true, null);
                }
            }
            
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
            if (!(ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()) ||
               ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri())))
                lstSourceChiNhanh.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
            
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);

            if (!(ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()) ||
               ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri())))
                if (lstSourceChiNhanh.Count > 0) cmbChiNhanh.SelectedIndex = 0;

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
        }

        /// <summary>
        /// Lay cac gia tri tren form 
        /// </summary>
        private void GetFormValues()
        {
            MaChiNhanh = "%";
            MaPhongGD = "%";

            //Lay thong tin ma don vi lon nhat
            MaToChuc = ClientInformation.MaToChuc;

            //lay thong tin ma don vi quan ly
            MaDonViQuanLy = ClientInformation.MaDonViQuanLy;

            //Lay thong tin ma chi nhanh
            MaChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();

            //Lay gia tri tungay, denngay
            DateTime dtNgayBC = new DateTime();
            if (raddtNgayBaoCao.Value is DateTime)
            {
                dtNgayBC = (DateTime)raddtNgayBaoCao.Value;
                NgayBaoCao = dtNgayBC.ToString(ApplicationConstant.defaultDateTimeFormat);
            }
            else
            {
                dtNgayBC = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
                NgayBaoCao = ClientInformation.NgayLamViecHienTai;
                TuNgay = dtNgayBC.GetFirstDateOfMonth(ApplicationConstant.defaultDateTimeFormat);
                DenNgay = dtNgayBC.GetLastDateOfMonth(ApplicationConstant.defaultDateTimeFormat);
            }

            DateTime dtDenNgay = new DateTime();
            if (raddtThangChot.Value is DateTime)
            {
                dtDenNgay = (DateTime)raddtThangChot.Value;
                TuNgay = dtDenNgay.GetFirstDateOfMonth(ApplicationConstant.defaultDateTimeFormat);
                DenNgay = dtDenNgay.GetLastDateOfMonth(ApplicationConstant.defaultDateTimeFormat);
            }
            else
            {
                dtDenNgay = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
                TuNgay = dtDenNgay.GetFirstDateOfMonth(ApplicationConstant.defaultDateTimeFormat);
                DenNgay = dtDenNgay.GetLastDateOfMonth(ApplicationConstant.defaultDateTimeFormat);
            }
            string dinhKyBC = cmbDinhKy.SelectedValue.ToString();
            string quyBCao = radQuyBaoCao.Value.ToString();
            //lay thong tin nguoi dang nhap
            MaNguoiLap = ClientInformation.TenDangNhap;

            //lay thong tin ten nguoi dang nhap
            TenNguoiLap = ClientInformation.HoTen;

            //lay thong tin ma mau bieu
            MaMauBieu = "BCTK_BANG_CAN_DOI";

            //Lay thong tin ten don vi
            TenChiNhanh = ClientInformation.TenDonVi;

            //lay thong tin loai ngon ngu
            MaNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();

            //lay thong tin ma dinh dang
            MaDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();
            DinhKyBC = dinhKyBC;
            QuyBCao = quyBCao;
        }

        public List<ThamSoBaoCao> GetParameters()
        {
            GetFormValues();
            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();

            //listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenChiNhanh", TenChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            //listThamSoBaoCao.Add(new ThamSoBaoCao("P_MaPhongGiaoDich", MaPhongGD, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));            
            //listThamSoBaoCao.Add(new ThamSoBaoCao("P_TuNgay", TuNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            //listThamSoBaoCao.Add(new ThamSoBaoCao("P_DenNgay", DenNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            //listThamSoBaoCao.Add(new ThamSoBaoCao("P_DiaChi", "", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            //listThamSoBaoCao.Add(new ThamSoBaoCao("P_NoiBaoCao", "", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@P_SoQuyetDinh", "", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@P_NguoiLap", TenNguoiLap, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@P_NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            if (MaChiNhanh.Equals("%")) //Hợp nhất
            {
                for (int i = 0; i < lstSourceChiNhanh.Count; i++)
                {
                    MaChiNhanh = lstSourceChiNhanh.ElementAt(i).KeywordStrings.First();
                    if (!MaChiNhanh.Equals("%"))
                    {
                        listThamSoBaoCao.Add(new ThamSoBaoCao("@MA_CHI_NHANH", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    }
                }
                listThamSoBaoCao.Add(new ThamSoBaoCao("@P_MaMauBieu", "B02/TCTD-HN", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@P_MaDonVi", MaToChuc, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            else //từng chi nhánh
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@P_MaMauBieu", "B02/TCTD", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@P_MaDonVi", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MA_CHI_NHANH", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            listThamSoBaoCao.Add(new ThamSoBaoCao("@QuyBC", QuyBCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DinhKyBC", DinhKyBC, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            //listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGiaoDich", MaPhongGD, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MA_MAU_BIEU", MaMauBieu, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NGAY_BAO_CAO", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TU_NGAY", TuNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DEN_NGAY", DenNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NamChot", DenNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));
            return listThamSoBaoCao;
        }

        void cmbDinhKy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDinhKy.SelectedValue.Equals("THANG"))
            {
                raddtThangChot.Mask = "MM/yyyy";
                raddtThangChot.Margin = new Thickness(0, 0, 6, 0);
                raddtThangChot.Visibility = System.Windows.Visibility.Visible;
                radQuyBaoCao.Visibility = System.Windows.Visibility.Collapsed;
            }
            else if (cmbDinhKy.SelectedValue.Equals("QUY"))
            {
                raddtThangChot.Mask = "yyyy";
                raddtThangChot.Margin = new Thickness(6, 0, 6, 0);
                raddtThangChot.Visibility = System.Windows.Visibility.Visible;
                radQuyBaoCao.Visibility = System.Windows.Visibility.Visible;
            }
            else if (cmbDinhKy.SelectedValue.Equals("NAM"))
            {
                raddtThangChot.Mask = "yyyy";
                raddtThangChot.Margin = new Thickness(0, 0, 6, 0);
                raddtThangChot.Visibility = System.Windows.Visibility.Visible;
                radQuyBaoCao.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
