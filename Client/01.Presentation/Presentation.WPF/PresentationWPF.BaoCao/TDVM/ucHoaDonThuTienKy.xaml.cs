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
using PresentationWPF.BaoCao.DungChung;
using Utilities.Common;
using PresentationWPF.CustomControl;
using Presentation.Process.Common;
using Presentation.Process;
using Telerik.Windows.Controls;

namespace PresentationWPF.BaoCao.TDVM
{
    /// <summary>
    /// Interaction logic for ucHoaDonThuTienKy.xaml
    /// </summary>
    public partial class ucHoaDonThuTienKy : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNguyenNhan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKyThu = new List<AutoCompleteEntry>();

        // Các tham số báo cáo từ form điều kiện
        public string MaChiNhanh;
        public string TenChiNhanh;
        public string MaPhongGiaoDich;
        public string TenPhongGiaoDich;
        public string MaCum;
        public string TenCum;
        public string NgayLap;
        public string NgayBaoCao;
        public string MaNguoiLap;
        public string TenNguoiLap;

        public string MaNgonNgu;
        public string MaDinhDang;
        #endregion

        public ucHoaDonThuTienKy()
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
            cmbCum.SelectionChanged += new SelectionChangedEventHandler(cmbCum_SelectionChanged);
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
            auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue(),null);
            LoadComboboxPhongGD();

            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceCum, ref cmbCum, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM_ALL.getValue());
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

            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.PDF.layGiaTri());
            auto.removeEntry(ref lstSourceDinhDang, ref cmbDinhDang, ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri());
            auto.removeEntry(ref lstSourceDinhDang, ref cmbDinhDang, ApplicationConstant.LoaiDinhDangBaoCao.PDF.layGiaTri());
            cmbDinhDang.IsEnabled = false;
        }

        private void LoadComboboxPhongGD()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();

            lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
            string maChiNhanh = null;

            if (lstSourceChiNhanh.Count > 0)
            {
                if (cmbChiNhanh.Items.Count > 0)
                    maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                else
                    maChiNhanh = lstSourceChiNhanh.ElementAt(0).KeywordStrings.First();
                lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();

                // khởi tạo combobox
                auto = new AutoComboBox();
                cmbPhongGD.Items.Clear();
                auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
                cmbPhongGD.SelectedIndex = 0;
                cmbPhongGD.IsEnabled = !maChiNhanh.Equals("%");
            }
            else
            {
                lstSourcePhongGD_Select.Clear();
                cmbPhongGD.Items.Clear();
            }
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();
        }

        private void LoadComboboxCum()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();

            lstSourceCum_Select = new List<AutoCompleteEntry>();
            string maPhongGD = null;

            if (lstSourcePhongGD_Select.Count > 0)
            {
                if (cmbPhongGD.Items.Count > 0)
                    maPhongGD = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
                else
                    maPhongGD = lstSourcePhongGD_Select.ElementAt(0).KeywordStrings.First();

                lstSourceCum_Select = lstSourceCum.Where(e => e.KeywordStrings.ElementAt(1).Equals(maPhongGD)).ToList();

                // khởi tạo combobox
                auto = new AutoComboBox();
                cmbCum.Items.Clear();
                auto.GenAutoComboBox(ref lstSourceCum_Select, ref cmbCum, null);
                cmbCum.SelectedIndex = 0;
                cmbCum.IsEnabled = !maPhongGD.Equals("%");
            }
            else
            {
                lstSourceCum_Select.Clear();
                cmbCum.Items.Clear();
            }
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxCum();
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

            string maCum = string.Empty;
            string tenCum = string.Empty;
            if (lstSourceCum_Select.Count > 0)
            {
                maCum = lstSourceCum_Select.ElementAt(cmbCum.SelectedIndex).KeywordStrings.First();
                tenCum = lstSourceCum_Select.ElementAt(cmbCum.SelectedIndex).DisplayName;
            }

            //DateTime ngayBaoCao = new DateTime();
            //if (raddtNgayBaoCao.Value is DateTime)
            //    ngayBaoCao = (DateTime)raddtNgayBaoCao.Value;
            AutoCompleteEntry auNgayBC = lstSourceKyThu.ElementAt(cmbKyThu.SelectedIndex);
            string ngayBaoCao = "";
            if (!LObject.IsNullOrEmpty(auNgayBC))
                ngayBaoCao = auNgayBC.KeywordStrings.FirstOrDefault();
            string maNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            string maDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();

            // Gán dữ liệu từ form vào các biến truyền cho báo cáo
            MaChiNhanh = maChiNhanh;
            TenChiNhanh = tenChiNhanh;
            MaPhongGiaoDich = maPhongGiaoDich;
            TenPhongGiaoDich = tenPhongGiaoDich;
            NgayLap = Convert.ToDateTime(raddtNgayBaoCao.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            NgayBaoCao = ngayBaoCao;
            MaNgonNgu = maNgonNgu;
            MaDinhDang = maDinhDang;
            MaNguoiLap = Presentation.Process.Common.ClientInformation.TenDangNhap;
            TenNguoiLap = Presentation.Process.Common.ClientInformation.HoTen;
            MaCum = maCum;
            TenCum = tenCum;
        }

        private bool Validation()
        {
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
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenNguoiLap", TenNguoiLap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenChiNhanh", TenChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenPGD", TenPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenCum", TenCum, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayThangNam", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGiaoDich", MaPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));            
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaCum", MaCum, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            return listThamSoBaoCao;
        }

        private void raddtNgayBaoCao_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            cmbKyThu.Items.Clear();
            LayThongTinKyThu(true);
        }

        private bool LayThongTinKyThu(bool isNew = false)
        {
            bool kq = true;
            bool isSelected = false;
            List<DateTime> lstDate = new List<DateTime>();
            this.Cursor = Cursors.Wait;
            try
            {
                AutoCompleteEntry objCum = (AutoCompleteEntry)cmbCum.SelectedItem;
                AutoCompleteEntry objDvi = (AutoCompleteEntry)cmbPhongGD.SelectedItem;
                if (LObject.IsNullOrEmpty(objCum) || LObject.IsNullOrEmpty(objDvi))
                    return false;
                lstDate = new UtilitiesProcess().LayNgayHopCum(objCum.KeywordStrings[0], objDvi.KeywordStrings[0], Convert.ToDateTime(raddtNgayBaoCao.Value).ToString(ApplicationConstant.defaultDateTimeFormat));
                if (lstDate != null && lstDate.Count > 0)
                {
                    cmbKyThu.Items.Clear();
                    lstSourceKyThu.Clear();
                    for (int i = 0; i < lstDate.Count; i++)
                    {
                        lstSourceKyThu.Add(new AutoCompleteEntry("Kỳ thứ " + (i + 1).ToString() + " - Ngày " + lstDate[i].ToString("dd/MM/yyyy"), lstDate[i].DateToString(ApplicationConstant.defaultDateTimeFormat), lstDate[i].DateToString(ApplicationConstant.defaultDateTimeFormat)));
                        if (!isSelected)
                        {
                            if (isNew)
                            {
                                if (lstDate[i] >= raddtNgayBaoCao.Value)
                                {
                                    cmbKyThu.SelectedIndex = i;
                                    isSelected = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    kq = false;
                }
                new AutoComboBox().GenAutoComboBox(ref lstSourceKyThu, ref cmbKyThu, null);
            }
            catch (Exception ex)
            {
                kq = false;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
            return kq;
        }

        private void cmbCum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbKyThu.Items.Clear();
            LayThongTinKyThu(true);
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                cmbKyThu.Items.Clear();
                LayThongTinKyThu(true);
            }
            catch { }
        }
    }
}
