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

namespace PresentationWPF.BaoCao._BTV.BCTH
{
    /// <summary>
    /// Interaction logic for BCTH_HOAT_DONG_KHTC_DB.xaml
    /// </summary>
    public partial class BCTH_HOAT_DONG_KHTC_DB : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiBaoCao = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();

        public string machinhanh = "";
        public string maphonggd = "";
        public string tungay = "";
        public string denngay = "";
        public string ngaybaocao = "";
        public string ngaytinhlke = "";
        public string mangonngu = "";
        public string madinhdang = "";
        public List<string> lstID = null;
        #endregion

        #region Khoi tao
        public BCTH_HOAT_DONG_KHTC_DB()
        {
            InitializeComponent();
            KhoiTaoCombobox();
            raddtDenNgay.Value = raddtNgayBaoCao.Value = raddtNgayTinhLKe.Value = raddtTuNgay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
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
            auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());

            //khoi tao combobox dinh dang
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri());
            cmbDinhDang.IsEnabled = false;                    
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
            if (cmbPhongGD.SelectedIndex == -1) cmbPhongGD.SelectedIndex = 0;
        }

        private void HienDuLieuLenGrid()
        {
            try
            {
                string sLoai = "KHU_VUC";
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
        #endregion

        #region Xu ly giao dien
        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaoComboboxPGD();
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

        #region Xu ly nghiep vu
        private void GetValuesOnForm()
        {
            lstID = new List<string>();
            machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
            maphonggd = lstSourcePhongGD.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
            ngaybaocao = Convert.ToDateTime(raddtNgayBaoCao.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            ngaytinhlke = Convert.ToDateTime(raddtNgayTinhLKe.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            tungay = Convert.ToDateTime(raddtTuNgay.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            denngay = Convert.ToDateTime(raddtDenNgay.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            mangonngu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.FirstOrDefault();
            madinhdang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.FirstOrDefault();

            foreach (DataRowView drv in radgrdKVSP.SelectedItems)
            {
                lstID.Add(drv["ID"].ToString());
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
            return true;
        }

        public List<ThamSoBaoCao> GetParameters()
        {
            if (!Validation()) return null;
            GetValuesOnForm();
            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", machinhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGiaoDich", maphonggd, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TuNgay", tungay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DenNgay", denngay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayTinhLKe", ngaytinhlke, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", mangonngu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", madinhdang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            for (int i = 0; i < lstID.Count; i++)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@ID_XA", lstID[i], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            return listThamSoBaoCao;
        }
        #endregion
    }
}
