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

namespace PresentationWPF.BaoCao._DC.HDVO
{
    /// <summary>
    /// Interaction logic for HDVO_SAO_KE_TKCKH.xaml
    /// </summary>
    public partial class HDVO_SAO_KE_TKCKH : UserControl
    {
        #region Khai bao 
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiBaoCao = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();

        bool _isLoaded = false;
        public string machinhanh = "";
        public string makhuvuc = "";
        public string maphonggd = "";
        public string masanpham = "";
        public string mangonngu = "";
        public string madinhdang = "";
        public string tungay = "";
        public string denngay = "";
        public string ngaybaocao = "";
        public string mauinbc = "";
        public List<string> lstID = new List<string>();
        int id = 0;
        string ma = "";
        string sLoaiIN = "";
        #endregion

        #region Khoi tao
        public HDVO_SAO_KE_TKCKH()
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

            // khoi tao combobox ngon ngu
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());            
            auto = new AutoComboBox();
            //auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());
            auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ClientInformation.NgonNgu);

            //khoi tao combobox dinh dang
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());            
            auto = new AutoComboBox();
            //auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri());
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ReportInformation.DinhDang);
            cmbDinhDang.IsEnabled = false;

            TaoComBoBoxLoaiBaoCao();
            _isLoaded = true;
            TaoComboboxPGD();            
        }

        private void TaoComboboxPGD()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            lstSourcePhongGD = new List<AutoCompleteEntry>();
            cmbPhongGD.Items.Clear();
            string smaCN = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
            lstDieuKien.Add(smaCN);
            auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue(), lstDieuKien, ClientInformation.MaDonViGiaoDich);
        }

        private void TaoComBoBoxLoaiBaoCao()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            lstSourceLoaiBaoCao.Add(new AutoCompleteEntry("Bảng kê chi tiết theo khu vực", "B01", "KHU_VUC","CHI_TIET"));
            lstSourceLoaiBaoCao.Add(new AutoCompleteEntry("Bảng kê tổng hợp theo khu vực", "B02", "KHU_VUC","TONG_HOP_KV"));
            lstSourceLoaiBaoCao.Add(new AutoCompleteEntry("Bảng kê chi tiết theo sản phẩm", "B03", "SAN_PHAM_CKH","CHI_TIET"));
            lstSourceLoaiBaoCao.Add(new AutoCompleteEntry("Bảng kê tổng hợp theo sản phẩm", "B04", "SAN_PHAM_CKH","TONG_HOP_SP"));
            auto.GenAutoComboBox(ref lstSourceLoaiBaoCao, ref cmbLoaiBaoCao, null);
        }
        #endregion

        #region Xu ly giao dien
        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isLoaded) TaoComboboxPGD();
        }

        private void HienDuLieuLenGrid()
        {
            try
            {
                string sLoai = lstSourceLoaiBaoCao.ElementAt(cmbLoaiBaoCao.SelectedIndex).KeywordStrings[1];
                string sMaChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
                string sMaPhongGD = "";
                if (cmbPhongGD.Items.Count > 0) sMaPhongGD = lstSourcePhongGD.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
                else return;
                DataSet dsSource = new DataSet();
                BaoCaoProcess bcProcess = new BaoCaoProcess();
                dsSource = bcProcess.LayDuLieuBCSaoKeTKCKH(sMaChiNhanh, sMaPhongGD, sLoai);
                if (dsSource != null && dsSource.Tables.Count > 0)
                    radgrdKVSP.ItemsSource = dsSource.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HienDuLieuLenGrid();
        }

        private void cmbLoaiBaoCao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HienDuLieuLenGrid();
        }

        #endregion

        #region Lay du lieu in bao cao
        private bool Validation()
        {
            if (raddtTuNgay.Value > raddtDenNgay.Value)
            {
                LMessage.ShowMessage("M.ResponseMessage.BaoCao.TuNgayLonHonDenNgay", LMessage.MessageBoxType.Warning);
                raddtTuNgay.Focus();
                return false;
            }
            if (radgrdKVSP.SelectedItems == null)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                return false;
            }
            return true;
        }
        private void GetvaluesOnForm()
        {
            lstID = new List<string>();
            machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
            maphonggd = lstSourcePhongGD.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
            mauinbc = lstSourceLoaiBaoCao.ElementAt(cmbLoaiBaoCao.SelectedIndex).KeywordStrings.FirstOrDefault();
            tungay = Convert.ToDateTime(raddtTuNgay.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            denngay = Convert.ToDateTime(raddtDenNgay.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            ngaybaocao = Convert.ToDateTime(raddtNgayBaoCao.Value).ToString(ApplicationConstant.defaultDateTimeFormat);                                    
            sLoaiIN = lstSourceLoaiBaoCao.ElementAt(cmbLoaiBaoCao.SelectedIndex).KeywordStrings[2];

            foreach (DataRowView drv in radgrdKVSP.SelectedItems)
            {
                lstID.Add(drv["id"].ToString());
            }
        }

        public List<ThamSoBaoCao> GetParameters()
        {            
            if (!Validation()) return null;
            GetvaluesOnForm();
            List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();
            lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", machinhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@MaPhongGiaoDich", maphonggd, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@TuNgay", tungay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@DenNgay", denngay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));                        
            lstThamSo.Add(new ThamSoBaoCao("@LoaiInBC", mauinbc, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("P_LOAI_IN_BC", sLoaiIN, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", mangonngu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", madinhdang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            for (int i = 0; i < lstID.Count; i++)
            {
                lstThamSo.Add(new ThamSoBaoCao("@ID", lstID[i], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            return lstThamSo;
        }
        #endregion
    }
}
