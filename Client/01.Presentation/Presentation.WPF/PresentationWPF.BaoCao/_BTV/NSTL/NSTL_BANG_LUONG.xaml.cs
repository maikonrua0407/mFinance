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

namespace PresentationWPF.BaoCao._BTV.NSTL
{
    /// <summary>
    /// Interaction logic for NSTL_BANG_LUONG.xaml
    /// </summary>
    public partial class NSTL_BANG_LUONG : UserControl
    {
        #region Khai bao bien
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        public string machinhanh = "";
        public string maphonggd = "";
        public string tungay = "";
        public string denngay = "";
        public string ngaybaocao = "";
        public string ngonngu = "";
        public string dinhdang = "";
        public string thangbaocao = "";
        #endregion

        #region Khoi tao
        public NSTL_BANG_LUONG()
        {
            InitializeComponent();
            KhoiTaoCombobox();
            raddtThangBaoCao.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai,ApplicationConstant.defaultDateTimeFormat);
        }

        private void KhoiTaoCombobox()
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
            lstSourcePhongGD_Select.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));

            // khởi tạo combobox
            auto = new AutoComboBox();
            cmbPhongGD.Items.Clear();
            auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
            cmbPhongGD.SelectedIndex = 0;
            cmbPhongGD.IsEnabled = !maChiNhanh.Equals("%");
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        #endregion

        private void GetValuesOnForm()
        {
            DateTime dtNgayDauThang = new DateTime();
            DateTime dtNgayCuoiThang = new DateTime();
            int iMonth = Convert.ToDateTime(raddtThangBaoCao.Value).Month;
            int iYear = Convert.ToDateTime(raddtThangBaoCao.Value).Year;
            dtNgayDauThang = LDateTime.GetFirstDateOfMonth(iYear, iMonth);
            dtNgayCuoiThang = LDateTime.GetLastDateOfMonth(iYear, iMonth);
            machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            maphonggd = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
            tungay = dtNgayDauThang.ToString(ApplicationConstant.defaultDateTimeFormat);
            denngay = dtNgayCuoiThang.ToString(ApplicationConstant.defaultDateTimeFormat);
            ngaybaocao = ClientInformation.NgayLamViecHienTai;
            ngonngu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            dinhdang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();
            thangbaocao = Convert.ToDateTime(raddtThangBaoCao.Value).ToString("yyyyMM");
        }

        private bool Validation()
        {
            try
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
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return false;
            }
            return true;
        }

        public List<ThamSoBaoCao> GetParameters()
        {
            if (!Validation()) return null;
            GetValuesOnForm();
            List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();
            lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", machinhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@MaPhongGD", maphonggd, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@TuNgay", tungay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@DenNgay", denngay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ngonngu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", dinhdang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            lstThamSo.Add(new ThamSoBaoCao("@ThangBaoCao", thangbaocao, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            return lstThamSo;
        }
    }
}
