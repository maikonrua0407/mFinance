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
using System.Windows.Threading;
using PresentationWPF.CustomControl;
using Utilities.Common;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.Common;

namespace PresentationWPF.BaoCao._QUANGBINH.BCTC
{
    /// <summary>
    /// Interaction logic for BCTC_KET_QUA_HOAT_DONG.xaml
    /// </summary>
    public partial class BCTC_KET_QUA_HOAT_DONG : UserControl
    {
        #region khai bao
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNamTaiChinh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNguonVon = new List<AutoCompleteEntry>();        

        string maChiNhanh = "";
        string maPhongGD = "";
        string tenPhongGiaoDich = "";
        string ngayChotDLieu = "";
        string ngayBaoCao = "";
        string dinhdang = "";
        string ngonngu = "";
        string tenchinhanh = "";
        string dinhKyBC = "THANG";
        string quyBCao = "1";
        string loaiBaoCao = "";
        string nguonVon = "";
        string kieuDuLieu = "";
        
        #endregion

        #region Khoi tao
        public BCTC_KET_QUA_HOAT_DONG()
        {
            InitializeComponent();
            LoadCombobox();
            cmbDinhKy.SelectionChanged += new SelectionChangedEventHandler(cmbDinhKy_SelectionChanged);
            cmbChiNhanh.SelectionChanged += new SelectionChangedEventHandler(cmbChiNhanh_SelectionChanged);
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
            Dispatcher.CurrentDispatcher.DelayInvoke("cmbChiNhanh", () =>
            {
                auto = new AutoComboBox();
                lstSourceChiNhanh.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
                auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);
                LoadComboboxPhongGD();
            }, TimeSpan.FromSeconds(0));

            //khoi tao combobox nguon von
            Dispatcher.CurrentDispatcher.DelayInvoke("cmbNguonVon", () =>
            {
                auto = new AutoComboBox();
                lstDieuKien = new List<string>();
                lstSourceNguonVon.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
                auto.GenAutoComboBox(ref lstSourceNguonVon, ref cmbNguonVon, "COMBOBOX_NGUON_VON_CT", lstDieuKien);
            }, TimeSpan.FromSeconds(0));

            //khoi tao combobox ngon ngu
            Dispatcher.CurrentDispatcher.DelayInvoke("cmbNgonNgu", () =>
            {
                auto = new AutoComboBox();
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
                auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());
            }, TimeSpan.FromSeconds(0));
            
            
            //khoi tao combox dinh dang
            Dispatcher.CurrentDispatcher.DelayInvoke("cmbDinhDang", () =>
            {
                auto = new AutoComboBox();
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
                auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.PDF.layGiaTri());
            }, TimeSpan.FromSeconds(0));
            
        }

        private void LoadComboboxPhongGD()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();

            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            if (maChiNhanh == "%")
            {
                cmbPhongGD.Items.Clear();
                lstSourcePhongGD = new List<AutoCompleteEntry>();
                lstSourcePhongGD.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
                cmbPhongGD.SelectedIndex = 0;
                cmbPhongGD.IsEnabled = false;
            }
            else
            {
                cmbPhongGD.IsEnabled = true;
                cmbPhongGD.Items.Clear();
                lstDieuKien.Add(maChiNhanh);
                lstSourcePhongGD = new List<AutoCompleteEntry>();
                lstSourcePhongGD.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
                auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOMACHA.getValue(), lstDieuKien);
                cmbPhongGD.SelectedIndex = 0;
            }

        }

        void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();
        }
        #endregion

        #region Xu ly in bao cao
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
        private bool Validation()
        {
            if (cmbChiNhanh.SelectedIndex == -1)
            {
                LMessage.ShowMessage("M.ResponseMessage.BaoCao.ChuaChonChiNhanh", LMessage.MessageBoxType.Warning);
                return false;
            }
            return true;
        }

        private void GetFormData()
        {
            maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            maPhongGD = lstSourcePhongGD.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
            nguonVon = lstSourceNguonVon.ElementAt(cmbNguonVon.SelectedIndex).KeywordStrings.First();
            ngayChotDLieu = LDateTime.DateToString((DateTime)raddtThangChot.Value, ApplicationConstant.defaultDateTimeFormat);
            ngayBaoCao = LDateTime.DateToString((DateTime)raddtNgayBaoCao.Value, ApplicationConstant.defaultDateTimeFormat);
            ngonngu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            dinhdang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();
            tenchinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).DisplayName;
            dinhKyBC = cmbDinhKy.SelectedValue.ToString();
            kieuDuLieu = cmbKieuDuLieu.SelectedValue.ToString();
            quyBCao = radQuyBaoCao.Value.ToString();
            loaiBaoCao = cmbLoaiBaoCao.SelectedValue.ToString();
        }

        public DatabaseConstant.Action GetAction() { return DatabaseConstant.Action.IN_CHUNG; }

        public List<ThamSoBaoCao> GetParameters()
        {
            if (!Validation()) return null;
            GetFormData();
            List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();
            lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", maChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@MaPhongGD", maPhongGD, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            //lstThamSo.Add(new ThamSoBaoCao("@TuNgay", tungay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@NamChot", ngayChotDLieu, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@QuyBC", quyBCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@DinhKyBC", dinhKyBC, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ngayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@NguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@LoaiBC", loaiBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@NguonVon", nguonVon, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@KieuCDoi", kieuDuLieu, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("P_DINH_KY", dinhKyBC, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("P_QUY_BC", quyBCao, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("P_NGAY_CHOT_DL", ngayChotDLieu, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ngonngu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", dinhdang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));
            return lstThamSo;
        }

        #endregion
    }
}
