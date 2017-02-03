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
    /// Interaction logic for TDVM_HOP_DONG_VAY_VON_NHOM_BAO_LANH.xaml
    /// </summary>
    public partial class TDVM_DON_XIN_VAY_VON_NHOM_BAO_LANH : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();

        public string machinhanh = "";
        public string maphonggd = "";
        public string ngaybaocao = "";
        public string madinhdang = "";
        public string mangonngu = "";
        public string sodxvv = "";
        #endregion

        #region Khoi tao
        public TDVM_DON_XIN_VAY_VON_NHOM_BAO_LANH()
        {
            InitializeComponent();
            KhoiTaoCombobox();
            raddtNgayBaoCao.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
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
            //cmbPhongGD.IsEnabled = !maChiNhanh.Equals("%");
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();
        }

        #endregion

        #region Xu ly nghiep vu
        private void GetValuesOnForm()
        {
            string soDonXinVayVon = txtSoDXVV.Text;
            machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
            sodxvv = soDonXinVayVon;
            mangonngu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            madinhdang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();
            maphonggd = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
            ngaybaocao = Convert.ToDateTime(raddtNgayBaoCao.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
        }

        private bool Validation()
        {
            if (string.IsNullOrEmpty(txtSoDXVV.Text))
            {
                LMessage.ShowMessage("Chưa nhập số hợp đồng", LMessage.MessageBoxType.Information);
                return false;
            }
            return true;
        }

        public List<ThamSoBaoCao> GetParameters()
        {
            if (!Validation()) return null;
            GetValuesOnForm();
            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();

            listThamSoBaoCao.Add(new ThamSoBaoCao("@SoDonXinVayVon", sodxvv, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", machinhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", mangonngu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", madinhdang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            return listThamSoBaoCao;
        }
        #endregion
    }
}