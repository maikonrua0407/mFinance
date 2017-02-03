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

namespace PresentationWPF.BaoCao._BTRE.TDVM
{
    /// <summary>
    /// Interaction logic for TDVM_PHIEU_KIEM_TRA_SU_DUNG_VON_VAY.xaml
    /// </summary>
    public partial class TDVM_PHIEU_KIEM_TRA_SU_DUNG_VON_VAY : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKhuVuc = new List<AutoCompleteEntry>();
        ListCheckBoxCombo lstSourceCum = new ListCheckBoxCombo();

        public string machinhanh = "";
        public string maphonggd = "";
        public string tungay = "";
        public string denngay = "";
        public string madinhdang = "";
        public string mangonngu = "";
        public string idKhuVuc = "0";
        public List<string> lstIdCum = new List<string>();
        #endregion

        #region Khoi tao
        public TDVM_PHIEU_KIEM_TRA_SU_DUNG_VON_VAY()
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
            raddtTuNgay.Value = raddtDenNgay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);

            cmbChiNhanh.SelectionChanged += new SelectionChangedEventHandler(cmbChiNhanh_SelectionChanged);
            cmbPhongGD.SelectionChanged += new SelectionChangedEventHandler(cmbPhongGD_SelectionChanged);
            cmbKhuVuc.SelectionChanged += new SelectionChangedEventHandler(cmbKhuVuc_SelectionChanged);
            cmbCum.DropDownClosed += new EventHandler(cmbCum_DropDownClosed);
        }

        private void KhoiTaoCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();

            //Tao combobox chi nhanh
            Dispatcher.CurrentDispatcher.DelayInvoke("cmbChiNhanh", () =>
            {
                auto = new AutoComboBox();
                auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
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
                auto = new AutoComboBox();
                auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());
            }, TimeSpan.FromSeconds(0));

            //Tao combobox dinh dang
            Dispatcher.CurrentDispatcher.DelayInvoke("cmbDinhDang", () =>
            {
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
                auto = new AutoComboBox();
                auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri());
                cmbDinhDang.IsEnabled = false;
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
        }


        private void LoadComboboxKhuVuc()
        {
            lstSourceKhuVuc = new List<AutoCompleteEntry>();
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            string IdDonVi = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings[3];
            lstDieuKien.Add(IdDonVi);

            cmbKhuVuc.Items.Clear();
            auto.GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUC.getValue(), lstDieuKien, null);
            cmbKhuVuc.SelectedIndex = 0;
        }

        private void LoadComboboxCum()
        {
            lstSourceCum = new ListCheckBoxCombo();
            AutoComboBoxListCheckes auto = new AutoComboBoxListCheckes();
            List<string> lstDieuKien = new List<string>();
            string idDonVi = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings[3];
            idKhuVuc = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings[1];
            lstDieuKien.Add(idDonVi);
            lstDieuKien.Add(idKhuVuc);

            auto.GenAutoComboBox(ref lstSourceCum, ref cmbCum, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM.getValue(), lstDieuKien);

        }
        #endregion

        #region Xu ly giao dien
        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbChiNhanh.SelectedIndex == -1) return;
            LoadComboboxPhongGD();
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPhongGD.SelectedIndex == -1) return;
            LoadComboboxKhuVuc();
        }

        private void cmbKhuVuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbKhuVuc.SelectedIndex == -1) return;
            LoadComboboxCum();
        }

        private void cmbCum_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceCum = cmbCum.ItemsSource as ListCheckBoxCombo;
        }

        #endregion

        #region Xu ly nghiep vu
        private void GetValuesOnForm()
        {
            lstIdCum = new List<string>();
            machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
            maphonggd = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
            madinhdang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.FirstOrDefault();
            mangonngu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.FirstOrDefault();
            denngay = Convert.ToDateTime(raddtDenNgay.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            tungay = Convert.ToDateTime(raddtTuNgay.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            idKhuVuc = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings[1];

            foreach (AutoCompleteCheckBox lstCum in lstSourceCum.Where(e => e.CheckedMember == true))
            {
                lstIdCum.Add(lstCum.ValueMember[1].ToString());
            }
        }

        private bool Validation()
        {
            return true;
        }

        public List<ThamSoBaoCao> GetParameters()
        {
            if (!Validation()) return null;
            GetValuesOnForm();

            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", machinhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGD", maphonggd, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TuNgay", tungay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DenNgay", denngay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@IdKhuVuc", idKhuVuc, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", mangonngu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", madinhdang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            for (int i = 0; i < lstIdCum.Count; i++)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@IdCum", lstIdCum[i], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            return listThamSoBaoCao;
        }
        #endregion
    }
}

