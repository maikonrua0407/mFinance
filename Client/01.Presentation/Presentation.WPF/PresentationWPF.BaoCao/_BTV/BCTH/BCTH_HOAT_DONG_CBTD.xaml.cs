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
using Presentation.Process.BaoCaoServiceRef;
using PresentationWPF.CustomControl;
using Utilities.Common;
using Presentation.Process.Common;
using PresentationWPF.BaoCao.DungChung;

namespace PresentationWPF.BaoCao._BTV.BCTH
{
    /// <summary>
    /// Interaction logic for BCTH_HOAT_DONG_CBTD.xaml
    /// </summary>
    public partial class BCTH_HOAT_DONG_CBTD : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNguoiQLy = new List <AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNguoiQLy_Select = new List<AutoCompleteEntry>();
        private bool _Isloaded = false;
        public string mabaocao = "";
        public string machinhanh = "";
        public string maphonggd = "";
        public string tungay = "";
        public string denngay = "";
        public string ngaytinhlke = "";
        public string manguoiqly = "";
        public string dinhdang = "";
        public string ngonngu = "";
        public string tenchinhanh = "";
        public string tennguoiqly = "";        

        public string nguoilap = "";
        public string ngaybaocao = "";
        #endregion

        #region Khoi tao
        public BCTH_HOAT_DONG_CBTD()
        {
            InitializeComponent();
            KhoiTaoCombobox();
        }

        public void KhoiTaoCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();

            //Khoi tao combobox chi nhanh
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);

            //khoi tao combobox phong gd
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue());
            LoadComboboxPhongGD();            
            //khoi tao combobox ngon ngu
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());            
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());
            
            //khoi tao combobox dinh dang
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());            
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri());
            cmbDinhDang.IsEnabled = false;

            //khoi tao combobox nguoi quan ly
            lstDieuKien.Clear();
            lstDieuKien.Add(ClientInformation.MaDonVi);
            lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
            auto.GenAutoComboBox(ref lstSourceNguoiQLy, ref cmbTieuChiLoc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC_CBO_TIN_DUNG.getValue(), lstDieuKien);
            _Isloaded = true;
        }

        #endregion

        #region Xu ly giao dien
        private void LoadComboboxPhongGD()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();

            lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();
            //lstSourcePhongGD_Select.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));

            // khởi tạo combobox
            auto = new AutoComboBox();
            cmbPhongGD.Items.Clear();
            auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
            cmbPhongGD.SelectedIndex = 0;
            cmbPhongGD.IsEnabled = !maChiNhanh.Equals("%");            
        }

        private void LoadComboboxCBoQly()
        {                   
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            lstSourceNguoiQLy = new List<AutoCompleteEntry>();
            string machinhanh = "";
            string maphonggd = "";                        
            if (cmbChiNhanh.Items.Count > 0) machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            if (cmbPhongGD.Items.Count > 0) maphonggd = lstSourcePhongGD.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
            lstDieuKien.Add(machinhanh);
            lstDieuKien.Add(maphonggd);
            //khoi tao combobox
            cmbTieuChiLoc.Items.Clear();
            auto.GenAutoComboBox(ref lstSourceNguoiQLy, ref cmbTieuChiLoc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC_CBO_TIN_DUNG.getValue(), lstDieuKien);
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_Isloaded) LoadComboboxCBoQly();
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string sNgayDauNam = ClientInformation.NgayLamViecHienTai.Substring(0, 4) + "0101";
                raddtTuNgay.Value = raddtDenNgay.Value = raddtNgayBaoCao.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
                raddtNgayTinhLKe.Value = LDateTime.StringToDate(sNgayDauNam, ApplicationConstant.defaultDateTimeFormat);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
        #endregion

        #region Xu ly in bao cao
        private bool Validation()
        {            
            if (cmbChiNhanh.SelectedIndex == -1)
            {
                LMessage.ShowMessage("M.ResponseMessage.BaoCao.ChuaChonChiNhanh", LMessage.MessageBoxType.Warning);
                return false;                    
            }
            if (cmbPhongGD.SelectedIndex == -1)
            {
                LMessage.ShowMessage("M.ResponseMessage.BaoCao.ChuaChonPhongGD", LMessage.MessageBoxType.Warning);
                return false;
            }
            if (cmbTieuChiLoc.SelectedIndex == -1)
            {
                LMessage.ShowMessage("M.ResponseMessage.BaoCao.ChuaChonNguoiQuanLy", LMessage.MessageBoxType.Warning);
                return false;
            }
            if (raddtTuNgay.Value > raddtDenNgay.Value)
            {
                LMessage.ShowMessage("M.ResponseMessage.BaoCao.TuNgayLonHonDenNgay", LMessage.MessageBoxType.Warning);
                raddtTuNgay.Focus();
                return false;
            }
            return true;
        }

        private void GetFormData()
        {
            machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            maphonggd = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
            manguoiqly = lstSourceNguoiQLy.ElementAt(cmbTieuChiLoc.SelectedIndex).KeywordStrings.First();
            tungay = LDateTime.DateToString((DateTime)raddtTuNgay.Value, ApplicationConstant.defaultDateTimeFormat);            
            denngay = LDateTime.DateToString((DateTime)raddtDenNgay.Value, ApplicationConstant.defaultDateTimeFormat);
            ngaytinhlke = LDateTime.DateToString((DateTime)raddtNgayTinhLKe.Value, ApplicationConstant.defaultDateTimeFormat);
            ngaybaocao = ClientInformation.NgayLamViecHienTai;
            ngonngu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            dinhdang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();
            tenchinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).DisplayName;
            tennguoiqly = lstSourceNguoiQLy.ElementAt(cmbTieuChiLoc.SelectedIndex).DisplayName;            
        }

        public DatabaseConstant.Action GetAction() { return DatabaseConstant.Action.IN_CHUNG; }

        public List<ThamSoBaoCao> GetParameters()
        {
            if (!Validation()) return null;
            GetFormData();            
            List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();
            lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", machinhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@MaPhongGD", maphonggd, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@TuNgay", tungay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@DenNgay", denngay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@NgayTinhLKe", ngaytinhlke, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@MaNguoiQLy", manguoiqly, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("TuNgay", tungay, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("DenNgay", denngay, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ngonngu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", dinhdang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));
            return lstThamSo;
        }

        #endregion
    }
}
