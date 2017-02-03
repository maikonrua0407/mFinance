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

namespace PresentationWPF.BaoCao._QUANGBINH.BCTH
{
    /// <summary>
    /// Interaction logic for BCTH_BC_TIEN_DO.xaml
    /// </summary>
    public partial class BCTH_BC_TIEN_DO : UserControl
    {
        #region Khai bao
        ListCheckBoxCombo lstSourceChiNhanh = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourceNguonVon = new ListCheckBoxCombo();        
        List<AutoCompleteEntry> lstSourceNamTaiChinh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        public string TenChiNhanh;
        public string ngaydaunam = "";
        public string ngaycuoinam = "";
        public string ngaybaocao = "";
        public string madinhdang = "";
        public string mangonngu = "";
        public string nam = "";
        public string tungay = "";
        string ngayChotDLieu = "";
        List<string> lstChiNhanh = new List<string>();
        List<string> lstNguonVon = new List<string>();
        #endregion

        #region Khoi tao
        public BCTH_BC_TIEN_DO()
        {
            InitializeComponent();
            LoadCombobox();
            raddtNgayBaoCao.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            raddtThangChot.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMM");
        }
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
            new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANHLIST.getValue(), lstDieuKien);

            lstDieuKien = new List<string>();
            new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceNguonVon, ref cmbNguonVon, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NGUON_VON.getValue(), lstDieuKien);

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());

            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri());
            cmbDinhDang.IsEnabled = false;

            raddtNgayBaoCao.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
        }
        #endregion

        #region Xu ly nghiep vu
        private void cmbChiNhanh_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceChiNhanh = cmbChiNhanh.ItemsSource as ListCheckBoxCombo;
        }

        private void cmbNguonVon_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceNguonVon = cmbNguonVon.ItemsSource as ListCheckBoxCombo;
        }

        private void GetFormValues()
        {
            lstChiNhanh = new List<string>();
            lstNguonVon = new List<string>();
            foreach (AutoCompleteCheckBox auChiNhanh in lstSourceChiNhanh.Where(e => e.CheckedMember == true))
            {
                lstChiNhanh.Add(auChiNhanh.ValueMember[0]);
                TenChiNhanh = auChiNhanh.DislayMember + " - ";
            }
            foreach (AutoCompleteCheckBox auNguonVon in lstSourceNguonVon)
            {
                if (auNguonVon.CheckedMember)
                    lstNguonVon.Add(auNguonVon.ValueMember[0]);
            }
            if (lstChiNhanh.Contains("All"))
                TenChiNhanh = "Toàn hệ thống";

            ngayChotDLieu = LDateTime.DateToString((DateTime)raddtThangChot.Value, ApplicationConstant.defaultDateTimeFormat);
            ngaydaunam = ngayChotDLieu.StringToDate(ApplicationConstant.defaultDateTimeFormat).GetFirstDateOfYear().ToString(ApplicationConstant.defaultDateTimeFormat);
            ngaybaocao = LDateTime.DateToString((DateTime)raddtNgayBaoCao.Value, ApplicationConstant.defaultDateTimeFormat);
            mangonngu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            madinhdang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();
            
        }
        private bool Validation()
        {
            return true;
        }

        public List<ThamSoBaoCao> GetParameters()
        {
            if (!Validation()) return null;
            GetFormValues();
            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TenChiNhanh", TenChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            foreach (string chinhanh in lstChiNhanh)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", chinhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            foreach (string nguonvon in lstNguonVon)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@NguonVon", nguonvon, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayDuLieu", ngayChotDLieu, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", mangonngu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", madinhdang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));
            return listThamSoBaoCao;
        }
        #endregion

    }
}
