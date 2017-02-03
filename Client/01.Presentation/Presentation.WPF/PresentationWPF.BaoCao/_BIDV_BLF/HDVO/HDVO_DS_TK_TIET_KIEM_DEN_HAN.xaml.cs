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

namespace PresentationWPF.BaoCao._BIDV_BLF.HDVO
{
    /// <summary>
    /// Interaction logic for HDVO_DS_TK_TIET_KIEM_DEN_HAN.xaml
    /// </summary>
    public partial class HDVO_DS_TK_TIET_KIEM_DEN_HAN : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        //List<AutoCompleteEntry> lstSourceTeller = new List<AutoCompleteEntry>();

        bool _isLoaded = false;
        public List<string> lstIDSanPham = new List<string>();
        public List<string> lstIDKyHan = new List<string>();
        public string machinhanh = "";
        public string maphonggd = "";
        public string tungay = "";
        public string denngay = "";
        public string ngaybaocao = "";
        public string madinhdang = "";
        public string mangonngu = "";
        //public string maTeller;
        #endregion

        #region Khoi tao
        public HDVO_DS_TK_TIET_KIEM_DEN_HAN()
        {
            InitializeComponent();
            KhoiTaoCombobox();
            if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()) ||
                ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()))
            {
                cmbChiNhanh.IsEnabled = false;
            }
            else
            {
                cmbChiNhanh.IsEnabled = true;
            }
            raddtDenNgay.Value = raddtTuNgay.Value = raddtNgayBaoCao.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
        }
        private void KhoiTaoCombobox()
        {
            List<string> lstDieuKien = new List<string>();
            AutoComboBox auto = new AutoComboBox();

            //Tao combobox chi nhanh
            lstDieuKien.Clear();
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue());
            TaoComboboxPGD();

            // khoi tao combobox ngon ngu
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
            auto = new AutoComboBox();
            //auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());
            auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ClientInformation.NgonNgu);
            cmbNgonNgu.IsEnabled = false;

            //khoi tao combobox dinh dang
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
            auto = new AutoComboBox();
            //auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri());
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ReportInformation.DinhDang);
            cmbDinhDang.IsEnabled = false;

            _isLoaded = true;
            LayDSSanPham();
            LayDSKyHan();
        }

        private void TaoComboboxPGD()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();

            lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();
            lstSourcePhongGD_Select.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), "%", ""));

            // khởi tạo combobox
            auto = new AutoComboBox();
            cmbPhongGD.Items.Clear();
            auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
            cmbPhongGD.SelectedIndex = 0;
            cmbPhongGD.IsEnabled = !maChiNhanh.Equals("%");
        }

        
        #endregion

        #region xu ly giao dien

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isLoaded) TaoComboboxPGD();
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isLoaded)
            {
                LayDSSanPham();
                LayDSKyHan();
            }
        }

        private void LayDSSanPham()
        {
            try
            {
                string sLoai = "SAN_PHAM";
                string sMaChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
                string sMaPhongGD = "";
                if (cmbPhongGD.Items.Count > 0)
                {
                    sMaPhongGD = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
                }
                else
                    sMaPhongGD = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                DataSet dsSource = new DataSet();
                BaoCaoProcess bcProcess = new BaoCaoProcess();
                dsSource = bcProcess.LayDuLieuBCSaoKeTKCKH(sMaChiNhanh, sMaChiNhanh, sLoai);
                if (dsSource != null && dsSource.Tables.Count > 0)
                    radgrdSP.ItemsSource = dsSource.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void LayDSKyHan()
        {
            try
            {
                string sLoai = "";
                string sMaChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
                string sMaPhongGD = "";
                if (cmbPhongGD.Items.Count > 0) sMaPhongGD = lstSourcePhongGD.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
                else return;
                DataSet dsSource = new DataSet();
                BaoCaoProcess bcProcess = new BaoCaoProcess();
                dsSource = bcProcess.LayDSKyHanHDV();
                if (dsSource != null && dsSource.Tables.Count > 0)
                    radgrdKV.ItemsSource = dsSource.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
        #endregion

        #region Lay du lieu in bao cao

        private bool validation()
        {
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
            lstIDKyHan = new List<string>();
            lstIDSanPham = new List<string>();
            machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
            if (lstSourcePhongGD_Select.Count > 0)
            {
                maphonggd = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
            }
            else
                maphonggd = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            //maphonggd = lstSourcePhongGD.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
           
            madinhdang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.FirstOrDefault();
            mangonngu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.FirstOrDefault();
            tungay = Convert.ToDateTime(raddtTuNgay.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            denngay = Convert.ToDateTime(raddtDenNgay.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            ngaybaocao = Convert.ToDateTime(raddtNgayBaoCao.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            foreach (DataRowView drv in radgrdSP.SelectedItems)
            {
                lstIDSanPham.Add(drv["ID"].ToString());
            }
            foreach (DataRowView drv in radgrdKV.SelectedItems)
            {
                lstIDKyHan.Add(drv["ID"].ToString());
            }
        }
        public List<ThamSoBaoCao> GetParameters()
        {
            if (!validation()) return null;
            GetFormData();
            List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();
            lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", machinhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@MaPhongGD", maphonggd, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@TuNgay", tungay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@DenNgay", denngay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@NguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
           
            lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", mangonngu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", madinhdang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            for (int i = 0; i < lstIDKyHan.Count; i++)
            {
                lstThamSo.Add(new ThamSoBaoCao("@IDKyHan", lstIDKyHan[i], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            for (int i = 0; i < lstIDSanPham.Count; i++)
            {
                lstThamSo.Add(new ThamSoBaoCao("@IDSanPham", lstIDSanPham[i], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            return lstThamSo;
        }
        #endregion
    }
}
