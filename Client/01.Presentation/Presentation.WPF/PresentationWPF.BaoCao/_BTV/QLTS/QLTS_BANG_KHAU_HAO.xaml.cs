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
using Presentation.Process.PopupServiceRef;

namespace PresentationWPF.BaoCao._BTV.QLTS
{
    /// <summary>
    /// Interaction logic for QLTS_BANG_KHAU_HAO.xaml
    /// </summary>
    public partial class QLTS_BANG_KHAU_HAO : UserControl
    {
        #region Khai bao

        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        // Các tham số báo cáo từ form điều kiện
        public string MaBaoCao;
        public string MaChiNhanh;
        public string TenChiNhanh;
        public string MaPhongGiaoDich;
        public string TenPhongGiaoDich;
        public string Quy;
        public string Nam;
        public string NgayBaoCao;
        public string MaNguoiLap;
        public string TenNguoiLap;
        public string MaLoaiTien;

        public string MaNgonNgu;
        public string MaDinhDang;

        #endregion

        public QLTS_BANG_KHAU_HAO()
        {
            InitializeComponent();
            LoadCombobox();
            LoadGrid();
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

            auto = new AutoComboBox();
            auto.GenComboBox_Nam(ref cmbNam);
            auto = new AutoComboBox();
            auto.GenComboBox_Quy(ref cmbQuy);

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DONG_TIEN_CAC_NUOC.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri());
        }

        private void LoadComboboxPhongGD()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();

            lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();
            //lstSourcePhongGD_Select.Insert(0, new AutoCompleteEntry("Tất cả", "%", ""));

            // khởi tạo combobox
            auto = new AutoComboBox();
            cmbPhongGD.Items.Clear();
            auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
            cmbPhongGD.SelectedIndex = 0;
            cmbPhongGD.IsEnabled = !maChiNhanh.Equals("%");
        }

        private void LoadGrid()
        {
            TaiSanProcess process = new TaiSanProcess();
            try
            {
                DataSet dsNhomTS = process.LayDanhSachNhomTaiSan();
                if (dsNhomTS.Tables.Count > 0) grNhomTS.DataContext = dsNhomTS.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                CommonFunction.ThongBaoLoi(ex);
            }
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();
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

            Quy = ((AutoCompleteEntry)cmbQuy.SelectedItem).KeywordStrings.First();
            Nam = ((AutoCompleteEntry)cmbNam.SelectedItem).KeywordStrings.First();

            DateTime ngayBaoCao = new DateTime();
            if (raddtNgayBaoCao.Value is DateTime)
                ngayBaoCao = (DateTime)raddtNgayBaoCao.Value;
            string maLoaiTien = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.First();

            string maDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            string tenChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).DisplayName;

            //Lấy giá trị
            MaChiNhanh = maChiNhanh;
            TenChiNhanh = tenChiNhanh;
            MaPhongGiaoDich = maDonVi;
            TenPhongGiaoDich = tenPhongGiaoDich;
            NgayBaoCao = ngayBaoCao.ToString("yyyyMMdd");
            MaLoaiTien = maLoaiTien;
            MaNgonNgu = maNgonNgu;
            MaDinhDang = maDinhDang;
            MaNguoiLap = Presentation.Process.Common.ClientInformation.TenDangNhap;
            TenNguoiLap = Presentation.Process.Common.ClientInformation.HoTen;
        }

        private bool Validation()
        {
            if (grNhomTS.SelectedItems.Count < 1)
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
                foreach (AutoCompleteEntry item in lstSourcePhongGD_Select)
                {
                    string ma = item.KeywordStrings.First();
                    if (!ma.Equals("%"))
                        listThamSoBaoCao.Add(new ThamSoBaoCao("@DSPhongGD", ma, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                }
            }
            else
                listThamSoBaoCao.Add(new ThamSoBaoCao("@DSPhongGD", MaPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            if (grNhomTS.SelectedItems.Count > 0)
            {
                for (int i = 0; i < grNhomTS.SelectedItems.Count; i++)
                {
                    DataRowView dr = (DataRowView)grNhomTS.SelectedItems[i];
                    listThamSoBaoCao.Add(new ThamSoBaoCao("@DSNhom", dr["MA_NHOM"].ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                }
            }
            listThamSoBaoCao.Add(new ThamSoBaoCao("@Quy", Quy, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@Nam", Nam, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            if (Quy.StringToInt32() == 1)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@KyKhauHaoTu", Nam + "01", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@KyKhauHaoDen", Nam + "03", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            else if (Quy.StringToInt32() == 2)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@KyKhauHaoTu", Nam + "04", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@KyKhauHaoDen", Nam + "06", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            else if (Quy.StringToInt32() == 3)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@KyKhauHaoTu", Nam + "07", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@KyKhauHaoDen", Nam + "09", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            else if (Quy.StringToInt32() == 4)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@KyKhauHaoTu", Nam + "10", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@KyKhauHaoDen", Nam + "12", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            listThamSoBaoCao.Add(new ThamSoBaoCao("@LoaiTien", MaLoaiTien, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaBaoCao", MaBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            return listThamSoBaoCao;
        }
    }
}
