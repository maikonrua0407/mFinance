using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows;
using System.Windows.Threading;
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
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.Common;

namespace PresentationWPF.BaoCao._DC.GDKT
{
    /// <summary>
    /// Interaction logic for GDKT_SO_TIEN_GUI_NGAN_HANG.xaml
    /// </summary>
    public partial class GDKT_SO_TIEN_GUI_NGAN_HANG : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNguonVon = new List<AutoCompleteEntry>();

        public string machinhanh;
        public string maphonggd;
        public string tungay;
        public string denngay;
        public string ngaybaocao;
        public string NguonVon;
        public string maloaitien;
        List<string> lstMaPhanLoai = new List<string>();
        public string madinhdang;
        public string mangonngu;
        #endregion

        #region Khoi tao
        public GDKT_SO_TIEN_GUI_NGAN_HANG()
        {
            InitializeComponent();
            LoadCombobox();
            if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()) ||
                ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()))
            {
                cmbChiNhanh.IsEnabled = false;
            }
            else
            {
                cmbChiNhanh.IsEnabled = true;
            }
            raddtTuNgay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat).GetFirstDateOfMonth();
            raddtDenNgay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat).GetLastDateOfMonth();
            raddtNgayBaoCao.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);

            cmbChiNhanh.SelectionChanged += new SelectionChangedEventHandler(cmbChiNhanh_SelectionChanged);
            cmbPhongGD.SelectionChanged += new SelectionChangedEventHandler(cmbPhongGD_SelectionChanged);
        }

        private void LoadCombobox()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            // Khởi tạo điều kiện gọi danh mục
            List<string> lstDieuKien = new List<string>();

            // khởi tạo combobox
            Dispatcher.CurrentDispatcher.DelayInvoke("cmbChiNhanh", () =>
            {
                auto = new AutoComboBox();
                auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);
            }, TimeSpan.FromSeconds(0));


            // khởi tạo combobox
            Dispatcher.CurrentDispatcher.DelayInvoke("cmbPhongGD", () =>
            {
                auto = new AutoComboBox();
                auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue());
                LoadComboboxPhongGD();
            }, TimeSpan.FromSeconds(0));


            Dispatcher.CurrentDispatcher.DelayInvoke("cmbLoaiTien", () =>
            {
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.DONG_TIEN_CAC_NUOC.getValue());
                // khởi tạo combobox
                auto = new AutoComboBox();
                auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ClientInformation.MaDongNoiTe);
                cmbLoaiTien.IsEnabled = false;
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("cmbNgonNgu", () =>
            {
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
                // khởi tạo combobox
                auto = new AutoComboBox();
                auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ClientInformation.NgonNgu);
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("cmbDinhDang", () =>
            {
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
                // khởi tạo combobox
                auto = new AutoComboBox();
                //auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri());
                auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ReportInformation.DinhDang);
                cmbDinhDang.IsEnabled = false;
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("cmbNguonVon", () =>
            {
                lstSourceNguonVon.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
                // khởi tạo combobox
                auto = new AutoComboBox();
                auto.GenAutoComboBox(ref lstSourceNguonVon, ref cmbNguonVon, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NGUON_VON.getValue(), null);
            }, TimeSpan.FromSeconds(0));

        }

        private void LoadComboboxPhongGD()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();

            lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();

            // khởi tạo combobox
            auto = new AutoComboBox();
            cmbPhongGD.Items.Clear();
            auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
            cmbPhongGD.SelectedIndex = 0;
            cmbPhongGD.IsEnabled = !maChiNhanh.Equals("%");
        }

        private void LoadDSTaiKhoan()
        {
            try
            {
                string MaChiNhanh = "";
                string MaPhongGD = "";
                string MaLoaiTien = "";
                if (cmbChiNhanh.Items.Count == 0) return;
                if (cmbLoaiTien.Items.Count == 0) return;
                if (cmbPhongGD.Items.Count == 0 || lstSourcePhongGD_Select.Count == 0) return;
                MaChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
                MaPhongGD = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
                MaLoaiTien = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.FirstOrDefault();

                DataSet dsTaiKhoan = new DataSet();
                BaoCaoProcess bcProcess = new BaoCaoProcess();
                dsTaiKhoan = bcProcess.LayDanhSachTKhoanTGNH(MaChiNhanh, MaLoaiTien);
                if (dsTaiKhoan != null && dsTaiKhoan.Tables.Count > 0)
                {
                    grid.DataContext = dsTaiKhoan.Tables[0].DefaultView;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        #endregion

        #region Xu ly giao dien

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadDSTaiKhoan();
        }

        private void cmbNgonNgu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadDSTaiKhoan();
        }
        #endregion

        #region Xu ly nghiep vu

        private void GetFormData()
        {
            lstMaPhanLoai = new List<string>();
            machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
            maphonggd = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
            maloaitien = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.FirstOrDefault();
            string nguonVon = lstSourceNguonVon.ElementAt(cmbNguonVon.SelectedIndex).KeywordStrings.First();
            tungay = Convert.ToDateTime(raddtTuNgay.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            denngay = Convert.ToDateTime(raddtDenNgay.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            ngaybaocao = Convert.ToDateTime(raddtNgayBaoCao.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            NguonVon = nguonVon;

            foreach (DataRowView drv in grid.SelectedItems)
            {
                lstMaPhanLoai.Add(drv["MA_PLOAI"].ToString());
            }
        }

        private bool Validation()
        {
            if (raddtTuNgay.Value > raddtDenNgay.Value)
            {
                LMessage.ShowMessage("M.ResponseMessage.BaoCao.TuNgayLonHonDenNgay", LMessage.MessageBoxType.Warning);
                raddtTuNgay.Focus();
                return false;
            }

            if (grid.SelectedItems.Count == 0)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                return false;
            }
            return true;
        }

        public List<ThamSoBaoCao> GetParameters()
        {
            if (!Validation())
            {
                return null;
            }

            GetFormData();

            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TuNgay", tungay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DenNgay", denngay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NguonVon", NguonVon, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", machinhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGD", maphonggd, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaLoaiTien", maloaitien, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First(), ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            for (int i = 0; i < lstMaPhanLoai.Count; i++)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhanLoai", lstMaPhanLoai[i], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            return listThamSoBaoCao;
        }
        #endregion

    }
}
