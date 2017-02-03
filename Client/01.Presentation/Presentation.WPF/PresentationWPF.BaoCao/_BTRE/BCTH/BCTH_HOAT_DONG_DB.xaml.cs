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

namespace PresentationWPF.BaoCao._BTRE.BCTH
{
    /// <summary>
    /// Interaction logic for BCTH_HOAT_DONG_DB.xaml
    /// </summary>
    public partial class BCTH_HOAT_DONG_DB : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();               
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();        
        List<AutoCompleteEntry> lstSourceLoaiBC = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        ListCheckBoxCombo lstSourceKhuVuc = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourceCum = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourcePhongGD = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourceNguonVon = new ListCheckBoxCombo();  

        public string machinhanh = "";
        public string tungay = "";
        public string denngay = "";
        public string ngaybaocao = "";
        public string madinhdang = "";
        public string mangonngu = "";
        public string Loai = "";
        List<string> lstId = null;
        List<string> lstPhongGD = null;
        List<string> lstNguonVon = new List<string>();
        #endregion

        #region Khoi tao
        public BCTH_HOAT_DONG_DB()
        {
            InitializeComponent();
            KhoiTaoCombobox();
            raddtDenNgay.Value = raddtNgayBaoCao.Value = raddtTuNgay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
        }
        public void KhoiTaoCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();

            //Khoi tao combobox chi nhanh
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);

            //khoi tao combobox phong gd
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
            lstDieuKien = new List<string>();
            new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceNguonVon, ref cmbNguonVon, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NGUON_VON.getValue(), lstDieuKien);
            TaoComboboxLoaiBaoCao();
            TaocomboboxKhuVuc();
            TaocomboboxCum();
        }

        private void LoadComboboxPhongGD()
        {
            if (cmbChiNhanh.SelectedIndex >= 0)
            {
                // khởi tạo combobox
                string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                string idChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings[1];

                // khởi tạo combobox
                lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
                lstSourcePhongGD = new ListCheckBoxCombo();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(idChiNhanh);
                lstDieuKien.Add(ClientInformation.TenDangNhap);
                lstDieuKien.Add(ClientInformation.MaDonViQuanLy);
                new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PGDLIST.getValue(), lstDieuKien);
            }
        }

        private void TaoComboboxLoaiBaoCao()
        {
            try
            {
                AutoComboBox auto = new AutoComboBox();
                lstSourceLoaiBC.Add(new AutoCompleteEntry("Báo cáo theo xã", "KHU_VUC"));
                lstSourceLoaiBC.Add(new AutoCompleteEntry("Báo cáo theo ấp", "CUM"));
                auto.GenAutoComboBox(ref lstSourceLoaiBC, ref cmbLoaiBaoCao, null);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void TaocomboboxKhuVuc()
        {
            try
            {
                // khởi tạo combobox
                AutoComboBox auto = new AutoComboBox();
                lstSourceKhuVuc = new ListCheckBoxCombo();
                string maPGD = "";
                string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                string idChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings[1];

                lstSourcePhongGD = cmbPhongGD.ItemsSource as ListCheckBoxCombo;
                foreach (AutoCompleteCheckBox lstCN in lstSourcePhongGD)
                {
                    if (lstCN.CheckedMember)
                        maPGD += ",'" + lstCN.ValueMember[0].ToString() + "'";
                }
                if (maChiNhanh.Length > 0)
                    maPGD = maPGD.Substring(1);
                else
                    maPGD = "0";

                List<string> lstDieuKien = new List<string>();

                // khởi tạo combobox
                auto = new AutoComboBox();
                lstDieuKien.Add(maChiNhanh);
                lstDieuKien.Add(maPGD);
                new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUCLIST.getValue(), lstDieuKien);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void TaocomboboxCum()
        {
            try
            {
                // khởi tạo combobox
                AutoComboBox auto = new AutoComboBox();
                lstSourceCum = new ListCheckBoxCombo();                
                string maPGD = "";
                string maKhuVuc = "";
                string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                string idChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings[1];

                lstSourcePhongGD = cmbPhongGD.ItemsSource as ListCheckBoxCombo;
                foreach (AutoCompleteCheckBox lstCN in lstSourcePhongGD)
                {
                    if (lstCN.CheckedMember)
                        maPGD += ",'" + lstCN.ValueMember[0].ToString() + "'";
                }
                if (maPGD.Length > 0)
                    maPGD = maPGD.Substring(1);
                else
                    maPGD = "0";

                lstSourceKhuVuc = cmbKhuVuc.ItemsSource as ListCheckBoxCombo;
                foreach (AutoCompleteCheckBox lstCN in lstSourceKhuVuc)
                {
                    if (lstCN.CheckedMember)
                        maKhuVuc += "," + lstCN.ValueMember[1].ToString();
                }
                if (maKhuVuc.Length > 0)
                    maKhuVuc = maKhuVuc.Substring(1);
                else
                    maKhuVuc = "0";

                List<string> lstDieuKien = new List<string>();

                // khởi tạo combobox
                auto = new AutoComboBox();
                lstDieuKien.Add(maChiNhanh);
                lstDieuKien.Add(maPGD);
                lstDieuKien.Add(maKhuVuc);
                new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceCum, ref cmbCum, "COMBOBOX_CUM_KVUC_LIST", lstDieuKien);
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
            TaocomboboxKhuVuc();
        }

        private void cmbKhuVuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaocomboboxCum();
        }

        private void cmbKhuVuc_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceKhuVuc = cmbKhuVuc.ItemsSource as ListCheckBoxCombo;
            TaocomboboxCum();
        }

        private void cmbPhongGD_DropDownClosed(object sender, EventArgs e)
        {
            lstSourcePhongGD = cmbPhongGD.ItemsSource as ListCheckBoxCombo;
            TaocomboboxKhuVuc();
        }

        private void cmbCum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbLoaiBaoCao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string ma = lstSourceLoaiBC.ElementAt(cmbLoaiBaoCao.SelectedIndex).KeywordStrings.FirstOrDefault();
            if (ma.Equals("KHU_VUC"))
            {
                cmbCum.IsEnabled = false;
            }
            else
            {
                cmbCum.IsEnabled = true;
            }
        }

        private void cmbCum_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceCum = cmbCum.ItemsSource as ListCheckBoxCombo;
        }

        private void cmbNguonVon_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceNguonVon = cmbNguonVon.ItemsSource as ListCheckBoxCombo;
        }
        #endregion

        #region Xu ly nghiep vu
        private void GetFormValues()
        {            
            lstId = new List<string>();
            lstPhongGD = new List<string>();
            lstNguonVon = new List<string>();
            machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
            madinhdang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.FirstOrDefault();
            mangonngu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.FirstOrDefault();
            ngaybaocao = Convert.ToDateTime(raddtNgayBaoCao.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            tungay = Convert.ToDateTime(raddtTuNgay.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            denngay = Convert.ToDateTime(raddtDenNgay.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            Loai = lstSourceLoaiBC.ElementAt(cmbLoaiBaoCao.SelectedIndex).KeywordStrings.FirstOrDefault();
            if (Loai.Equals("KHU_VUC"))
            {
                foreach (AutoCompleteCheckBox lstKhuVuc in lstSourceKhuVuc.Where(e => e.CheckedMember = true))
                {
                    lstId.Add(lstKhuVuc.ValueMember[1].ToString());
                }
            }
            else
            {
                foreach (AutoCompleteCheckBox lstCum in lstSourceCum.Where(e => e.CheckedMember = true))
                {
                    lstId.Add(lstCum.ValueMember[1].ToString());
                }
            }

            foreach (AutoCompleteCheckBox lst in lstSourcePhongGD.Where(e => e.CheckedMember = true))
            {
                lstPhongGD.Add(lst.ValueMember[1].ToString());
            }

            foreach (AutoCompleteCheckBox auNguonVon in lstSourceNguonVon)
            {
                if (auNguonVon.CheckedMember)
                    lstNguonVon.Add(auNguonVon.ValueMember[0]);
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
            GetFormValues();
            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", machinhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TuNgay", tungay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DenNgay", denngay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@Loai", Loai, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            foreach (string nguonvon in lstNguonVon)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaNguonVon", nguonvon, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", mangonngu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", madinhdang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            for (int i = 0; i < lstId.Count; i++)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@ID", lstId[i], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            for (int i = 0; i < lstPhongGD.Count; i++)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGD", lstPhongGD[i], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            return listThamSoBaoCao;
        }
        #endregion
    }
}
