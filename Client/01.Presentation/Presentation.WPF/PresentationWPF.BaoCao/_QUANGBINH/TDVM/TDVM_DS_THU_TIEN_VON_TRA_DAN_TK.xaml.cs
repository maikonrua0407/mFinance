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

namespace PresentationWPF.BaoCao._QUANGBINH.TDVM
{
    /// <summary>
    /// Interaction logic for TDVM_DS_THU_TIEN_VON_TRA_DAN_TK.xaml
    /// </summary>
    public partial class TDVM_DS_THU_TIEN_VON_TRA_DAN_TK : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKhuVuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum = new List<AutoCompleteEntry>();
        ListCheckBoxCombo lstSourceLoaiSanPham = new ListCheckBoxCombo();

        public string machinhanh = "";
        public string maphonggd = "";
        public string ngaybaocao = "";
        public string ngaychotdl = "";
        public string madinhdang = "";
        public string mangonngu = "";
        public string makhuvuc = "";
        public string macum = "";
        public string idKhuVuc = "0";
        public string sidCum = "";
        public string sNgayThuTien = "";
        int idCum = 0;
        public List<string> lstMaLoaiSP = new List<string>();
        public List<string> lstIdNhom = new List<string>();

        #endregion

        #region Khoi tao
        public TDVM_DS_THU_TIEN_VON_TRA_DAN_TK()
        {
            InitializeComponent();
            KhoiTaoCombobox();
            radtThangThuTien.Value = raddtNgayBaoCao.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            cmbChiNhanh.SelectionChanged += new SelectionChangedEventHandler(cmbChiNhanh_SelectionChanged);
            cmbPhongGD.SelectionChanged += new SelectionChangedEventHandler(cmbPhongGD_SelectionChanged);
            cmbKhuVuc.SelectionChanged += new SelectionChangedEventHandler(cmbKhuVuc_SelectionChanged);
            cmbCum.SelectionChanged += new SelectionChangedEventHandler(cmbCum_SelectionChanged);
            cmbLoaiSanPham.DropDownClosed += new EventHandler(cmbLoaiSanPham_DropDownClosed);
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
                //auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());
                auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ClientInformation.NgonNgu);
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("cmbDinhDang", () =>
            {
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());

                //Tao combobox dinh dang
                auto = new AutoComboBox();
                //auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri());
                auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ReportInformation.DinhDang);
                cmbDinhDang.IsEnabled = false;
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("LoadComboboxLoaiSP", () =>
            {
                LoadComboboxLoaiSP();
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

        private void LoadComboboxCum()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            string idDonVi = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings[3];
            idKhuVuc = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings[1];
            lstDieuKien.Add(idDonVi);
            lstDieuKien.Add(idKhuVuc);

            lstSourceCum = new List<AutoCompleteEntry>();
            cmbCum.Items.Clear();
            auto.GenAutoComboBox(ref lstSourceCum, ref cmbCum, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM.getValue(), lstDieuKien);
            cmbCum.SelectedIndex = 0;
        }

        private void LoadComboboxLoaiSP()
        {
            lstSourceLoaiSanPham = new ListCheckBoxCombo();
            AutoComboBoxListCheckes auto = new AutoComboBoxListCheckes();

            auto.GenAutoComboBox(ref lstSourceLoaiSanPham, ref cmbLoaiSanPham, "COMBOBOX_LOAI_SAN_PHAM", null);
        }

        private void LoadTTinLichHopCum()
        {
            try
            {
                List<AutoCompleteEntry> lstSourceKyThu = new List<AutoCompleteEntry>();
                this.Cursor = Cursors.Wait;
                AutoComboBox auCB = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();
                sidCum = lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings[1];
                lstDieuKien.Add(Convert.ToDateTime(radtThangThuTien.Value).ToString("yyyyMMdd"));
                lstDieuKien.Add("THU_VON");
                lstDieuKien.Add(sidCum);
                cmbKyThu.Items.Clear();
                auCB.GenAutoComboBox(ref lstSourceKyThu, ref cmbKyThu, "COMBOBOX_DOT_THU_PHAT", lstDieuKien);
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void LoadDSNhom()
        {
            try
            {
                DataSet dsNhom = new DataSet();
                BaoCaoProcess bcProcess = new BaoCaoProcess();
                string idCum = lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings[1];
                dsNhom = bcProcess.LayDSNhom(idCum);
                if (dsNhom != null && dsNhom.Tables.Count > 0)
                {
                    grdKhachHang.DataContext = dsNhom.Tables[0].DefaultView;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        #endregion

        #region Xu ly giao dien

        void cmbKhuVuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbKhuVuc.SelectedIndex != -1) LoadComboboxCum();
        }

        void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPhongGD.SelectedIndex != -1) LoadComboboxKhuVuc();
        }

        void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbChiNhanh.SelectedIndex != -1) LoadComboboxPhongGD();
        }

        void cmbCum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCum.SelectedIndex != -1)
            {
                LoadDSNhom();
                LoadTTinLichHopCum();
            }
        }

        private void cmbLoaiSanPham_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceLoaiSanPham = cmbLoaiSanPham.ItemsSource as ListCheckBoxCombo;
        }

        private void radtThangThuTien_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (cmbCum.SelectedIndex != -1)
            {
                LoadTTinLichHopCum();
            }
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
            ngaybaocao = Convert.ToDateTime(raddtNgayBaoCao.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            idKhuVuc = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings[1];
            sidCum = lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings[1];
            if (cmbKyThu.SelectedItem != null)
            {
                AutoCompleteEntry auKyThu = (AutoCompleteEntry)cmbKyThu.SelectedItem;
                if (auKyThu.KeywordStrings != null && auKyThu.KeywordStrings.Length > 0)
                {
                    sNgayThuTien = LDateTime.StringToDate(auKyThu.KeywordStrings[2], "yyyyMMdd").ToString(ApplicationConstant.defaultDateTimeFormat);
                }
            }
            foreach (DataRowView drv in grdKhachHang.SelectedItems)
            {
                lstIdNhom.Add(drv["ID"].ToString());
            }

            foreach (AutoCompleteCheckBox LoaiSP in lstSourceLoaiSanPham.Where(e => e.CheckedMember == true))
            {
                lstMaLoaiSP.Add(LoaiSP.ValueMember[0]);
            }
        }

        private bool Validation()
        {
            if (grdKhachHang.SelectedItems.Count == 0)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
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
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGD", maphonggd, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@IdKhuVuc", idKhuVuc, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@IdCum", sidCum, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayThuTien", sNgayThuTien, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", mangonngu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", madinhdang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            foreach (string LoaiSP in lstMaLoaiSP)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaLoaiSP", LoaiSP, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            for (int i = 0; i < lstIdNhom.Count; i++)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@IdNhom", lstIdNhom[i], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            return listThamSoBaoCao;
        }

        #endregion

    }
}