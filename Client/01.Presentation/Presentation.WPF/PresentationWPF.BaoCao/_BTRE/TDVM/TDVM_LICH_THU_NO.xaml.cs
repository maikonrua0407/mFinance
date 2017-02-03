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
    /// Interaction logic for TDVM_LICH_THU_NO.xaml
    /// </summary>
    public partial class TDVM_LICH_THU_NO : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        ListCheckBoxCombo lstSourceKhuVuc = new ListCheckBoxCombo();
        public string machinhanh = "";
        public string maphonggd = "";
        public string ngaybaocao = "";
        public string thangbaocao = "";
        public string madinhdang = "";
        public string mangonngu = "";
        public string sTenThangBC = "";
        public string sNgayBaoCao = "";
        public List<string> lstIdKhuVuc = new List<string>();
        #endregion

        #region Khoi tao
        public TDVM_LICH_THU_NO()
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
            raddtNgayBaoCao.Value = raddtDenNgay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);

            cmbChiNhanh.SelectionChanged += new SelectionChangedEventHandler(cmbChiNhanh_SelectionChanged);
            cmbPhongGD.SelectionChanged += new SelectionChangedEventHandler(cmbPhongGD_SelectionChanged);
            cmbKhuVuc.DropDownClosed += new EventHandler(cmbKhuVuc_DropDownClosed);

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
            cmbPhongGD.IsEnabled = !maChiNhanh.Equals("%");
        }

        private void LoadComboboxKhuVuc()
        {
            lstSourceKhuVuc = new ListCheckBoxCombo();
            string sMachiNhanh = "";
            string sMaPhongGD = "";
            List<string> lstDieuKien = new List<string>();
            AutoComboBoxListCheckes auto = new AutoComboBoxListCheckes();
            if (cmbChiNhanh.Items.Count == 0) return;
            if (cmbPhongGD.Items.Count == 0) return;
            sMachiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
            sMaPhongGD = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
            lstDieuKien.Add(sMachiNhanh);
            lstDieuKien.Add(sMaPhongGD);
            auto.GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUCLIST.getValue(), lstDieuKien);
        }

        #endregion

        #region Xu ly giao dien
        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxKhuVuc();
        }
        #endregion

        #region Xu ly nghiep vu

        private void GetValuesOnForm()
        {
            lstIdKhuVuc = new List<string>();
            int thang = Convert.ToInt32(Convert.ToDateTime(raddtDenNgay.Value).ToString("MM"));
            machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
            maphonggd = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
            mangonngu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.FirstOrDefault();
            madinhdang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.FirstOrDefault();
            ngaybaocao = Convert.ToDateTime(raddtNgayBaoCao.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            thangbaocao = Convert.ToDateTime(raddtDenNgay.Value).ToString("yyyyMM");
            sTenThangBC = laytenthang(thang);
            sNgayBaoCao = Convert.ToDateTime(raddtNgayBaoCao.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            foreach (AutoCompleteCheckBox lstKV in lstSourceKhuVuc.Where(e => e.CheckedMember == true))
            {
                lstIdKhuVuc.Add(lstKV.ValueMember[1].ToString());
            }
        }

        private string laytenthang(int thang)
        {
            string sthang = "";
            switch (thang)
            {
                case 1:
                    sthang = "JAN";
                    break;
                case 2:
                    sthang = "FEB";
                    break;
                case 3:
                    sthang = "MAR";
                    break;
                case 4:
                    sthang = "APR";
                    break;
                case 5:
                    sthang = "MAY";
                    break;
                case 6:
                    sthang = "JUN";
                    break;
                case 7:
                    sthang = "JUL";
                    break;
                case 8:
                    sthang = "AUG";
                    break;
                case 9:
                    sthang = "SEP";
                    break;
                case 10:
                    sthang = "OCT";
                    break;
                case 11:
                    sthang = "NOV";
                    break;
                case 12:
                    sthang = "DEC";
                    break;
                default:
                    sthang = "JAN";
                    break;
            }
            return sthang;
        }

        public List<ThamSoBaoCao> GetParameters()
        {
            GetValuesOnForm();
            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", machinhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGD", maphonggd, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@ThangBC", thangbaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@ThangTXT", sTenThangBC, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", sNgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", mangonngu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", madinhdang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));
            for (int i = 0; i < lstIdKhuVuc.Count; i++)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@IdKhuVuc", lstIdKhuVuc[i], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            return listThamSoBaoCao;
        }

        private void cmbKhuVuc_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceKhuVuc = cmbKhuVuc.ItemsSource as ListCheckBoxCombo;
        }

        public void OnShowResult(string sFolder)
        {
            if (!sFolder.Trim().Equals(""))
            {
                if (Directory.Exists(sFolder))
                {
                    ProcessStartInfo pfi = new ProcessStartInfo("Explorer.exe", sFolder);
                    System.Diagnostics.Process.Start(pfi);
                }
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        #endregion
    }
}
