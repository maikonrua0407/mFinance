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
    /// Interaction logic for TDVM_TO_KE_THU.xaml
    /// </summary>
    public partial class TDVM_TO_KE_THU : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKhuVuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCBQL = new List<AutoCompleteEntry>();

        public string machinhanh = "";
        public string maphonggd = "";
        public string ngaybaocao = "";
        public string ngaychotdl = "";
        public string madinhdang = "";
        public string mangonngu = "";
        public string makhuvuc = "";
        public string macum = "";
        public string idKhuVuc = "0";
        public string sidCBQL = "";
        public string tuNgay = "";
        public string denNgay = "";
        int idCum = 0;
        public List<string> lstIdNhom = new List<string>();
        #endregion

        #region Khoi tao
        public TDVM_TO_KE_THU()
        {
            InitializeComponent();
            KhoiTaoCombobox();
            raddtTuNgay.Value = LDateTime.GetFirstDateOfMonth(LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat));
            raddtDenNgay.Value = LDateTime.GetLastDateOfMonth(LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat));
            cmbChiNhanh.SelectionChanged += new SelectionChangedEventHandler(cmbChiNhanh_SelectionChanged);
            cmbPhongGD.SelectionChanged += new SelectionChangedEventHandler(cmbPhongGD_SelectionChanged);            
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

            Dispatcher.CurrentDispatcher.DelayInvoke("cmbDinhDang", () =>
            {
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());

                //Tao combobox dinh dang
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
            //cmbPhongGD.IsEnabled = !maChiNhanh.Equals("%");
        }

        private void LoadComboboxKhuVuc()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            string IdDonVi = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings[3];
            lstDieuKien.Add(IdDonVi);

            lstSourceKhuVuc = new List<AutoCompleteEntry>();
            cmbKhuVuc.Items.Clear();
            auto.GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUC.getValue(), lstDieuKien, null);
            cmbKhuVuc.SelectedIndex = 0;
        }

        private void LoadComboboxCBQL()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            lstSourceCBQL = new List<AutoCompleteEntry>();
            string maPhongGD = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();            

            cmbCBQL.Items.Clear();
            lstDieuKien.Clear();
            lstSourceCBQL.Clear();
            lstDieuKien.Add(maPhongGD);
            auto.GenAutoComboBox(ref lstSourceCBQL, ref cmbCBQL, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NSD.getValue(), lstDieuKien);
        }


        #endregion

        #region Xu ly giao dien     

        void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPhongGD.SelectedIndex != -1)
            {
                LoadComboboxKhuVuc();
                LoadComboboxCBQL();
            }
        }

        void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbChiNhanh.SelectedIndex != -1) LoadComboboxPhongGD();
        }

        #endregion

        #region Xu ly nghiep vu
        private void GetValuesOnForm()
        {
            lstIdNhom = new List<string>();
            machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
            maphonggd = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
            madinhdang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.FirstOrDefault();
            mangonngu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.FirstOrDefault();
            tuNgay = Convert.ToDateTime(raddtTuNgay.Value).ToString("yyyyMMdd");
            denNgay = Convert.ToDateTime(raddtDenNgay.Value).ToString("yyyyMMdd");
            ngaybaocao = Convert.ToDateTime(raddtNgayBaoCao.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            idKhuVuc = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings[1];
            sidCBQL = lstSourceCBQL.ElementAt(cmbCBQL.SelectedIndex).KeywordStrings[1];
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
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@IdKhuVuc", idKhuVuc, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@IdCBQL", sidCBQL, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TuNgay", tuNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DenNgay", denNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", mangonngu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", madinhdang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            for (int i = 0; i < lstIdNhom.Count; i++)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@IdNhom", lstIdNhom[i], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            return listThamSoBaoCao;
        }

        #endregion

    }
}