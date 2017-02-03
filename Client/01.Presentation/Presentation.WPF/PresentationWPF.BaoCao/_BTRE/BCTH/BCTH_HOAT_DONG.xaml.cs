using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Data;
using Presentation.Process;
using Presentation.Process.Common;
using System.IO;
using System.Diagnostics;
using Telerik.Windows.Controls;
namespace PresentationWPF.BaoCao._BTRE.BCTH
{
    /// <summary>
    /// Interaction logic for BCTH_HOAT_DONG.xaml
    /// </summary>
    public partial class BCTH_HOAT_DONG : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        ListCheckBoxCombo lstSourceNguonVon = new ListCheckBoxCombo();    

        public string machinhanh = "";
        public string maphonggd = "";
        public string tungay = "";
        public string denngay = "";
        public string ngaybaocao = "";
        public string ngaytinhlke = "";
        public string mangonngu = "";
        public string madinhdang = "";
        public List<string> lstID = null;
        List<string> lstNguonVon = new List<string>();
        #endregion

        #region Khoi tao
        public BCTH_HOAT_DONG()
        {
            InitializeComponent();
            LoadCombobox();
            raddtDenNgay.Value = raddtTuNgay.Value = raddtNgayBaoCao.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
        }
        private void LoadCombobox()
        {
            try
            {
                List<string> lstDieuKien = new List<string>();
                AutoComboBox auto = new AutoComboBox();

                lstDieuKien = new List<string>();
                new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceNguonVon, ref cmbNguonVon, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NGUON_VON.getValue(), lstDieuKien);
                //Tao combobox chi nhanh
                Dispatcher.CurrentDispatcher.DelayInvoke("cmbChiNhanh", () =>
                {
                    auto = new AutoComboBox();
                    auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);
                }, TimeSpan.FromSeconds(0));

                //Tao combobox phonggd
                Dispatcher.CurrentDispatcher.DelayInvoke("cmbPhongGD", () =>
                {
                    auto = new AutoComboBox();
                    auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue());
                    LoadComboboxPhongGD();
                }, TimeSpan.FromSeconds(0));

                //Tao combobox ngon ngu
                Dispatcher.CurrentDispatcher.DelayInvoke("cmbNgonNgu", () =>
                {
                    lstDieuKien = new List<string>();
                    lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
                    // khởi tạo combobox
                    auto = new AutoComboBox();
                    auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());
                }, TimeSpan.FromSeconds(0));

                //Tao combobox dinh dang
                Dispatcher.CurrentDispatcher.DelayInvoke("cmbDinhDang", () =>
                {
                    lstDieuKien = new List<string>();
                    lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
                    // khởi tạo combobox
                    auto = new AutoComboBox();
                    auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri());
                    cmbDinhDang.IsEnabled = false;
                }, TimeSpan.FromSeconds(0));

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void LoadComboboxPhongGD()
        {
            try
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
                //cmbPhongGD.IsEnabled = !maChiNhanh.Equals("%");
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void LoadDSKhuVuc()
        {
            try
            {
                string sLoai = "KHU_VUC";
                string sMaChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
                string sMaPhongGD = "";
                if (cmbPhongGD.Items.Count > 0) sMaPhongGD = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
                else return;
                DataSet dsSource = new DataSet();
                BaoCaoProcess bcProcess = new BaoCaoProcess();
                dsSource = bcProcess.LayDuLieuBCSaoKeTKCKH(sMaChiNhanh, sMaPhongGD, sLoai);
                if (dsSource != null && dsSource.Tables.Count > 0)
                    grdKhachHang.ItemsSource = dsSource.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadDSKhuVuc();
        }
        #endregion

        #region Xu ly nghiep vu
        private void cmbNguonVon_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceNguonVon = cmbNguonVon.ItemsSource as ListCheckBoxCombo;
        }

        private void GetValuesOnForm()
        {
            lstID = new List<string>();
            lstNguonVon = new List<string>();
            machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
            maphonggd = lstSourcePhongGD.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
            ngaybaocao = Convert.ToDateTime(raddtNgayBaoCao.Value).ToString(ApplicationConstant.defaultDateTimeFormat);            
            tungay = Convert.ToDateTime(raddtTuNgay.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            denngay = Convert.ToDateTime(raddtDenNgay.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            mangonngu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.FirstOrDefault();
            madinhdang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.FirstOrDefault();
            foreach (AutoCompleteCheckBox auNguonVon in lstSourceNguonVon)
            {
                if (auNguonVon.CheckedMember)
                    lstNguonVon.Add(auNguonVon.ValueMember[0]);
            }
            foreach (DataRowView drv in grdKhachHang.SelectedItems)
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
            foreach (string nguonvon in lstNguonVon)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaNguonVon", nguonvon, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
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
