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

namespace PresentationWPF.BaoCao._QUANGBINH.KHTV
{
    /// <summary>
    /// Interaction logic for KHTV_BAN_KHAO_SAT_HOAN_CANH_KTXH.xaml
    /// </summary>
    public partial class KHTV_BAN_KHAO_SAT_HOAN_CANH_KTXH : UserControl
    {
        #region Khai bao bien
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKhuVuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNhom = new List<AutoCompleteEntry>();
        DataSet dsKhachHang = new DataSet();

        int _idKhachHang = 0;

        public string makhachhang = "";
        public string tenkhachhang = "";
        public string gioitinh = "";
        public string ngaysinh = "";
        public string sohokhau = "";
        public string diachi = "";
        public string socmnd = "";
        public string ngaycap = "";
        public string noicap = "";
        public string nguoithuake = "";
        public string qhnguoithuake = "";
        public string xaphuong = "";
        public string thanhphohuyen = "";
        public string sosohokhau = "";
        public string sokhau = "";
        public string sTenMucDichVay = "";

        public string machinhanh;
        public string maphonggd;
        public string idKhuVuc;
        public string idCum;
        public string tungay;
        public string denngay;
        public string idNhom;
        public string ngaybaocao;
        public string makhuvuc;
        public string macum;
        public string manhom;
        List<string> lstIdKhachHang = null;

        public string madinhdang;
        public string mangonngu;
        public bool isLoad = false;
        #endregion

        #region Khoi tao
        public KHTV_BAN_KHAO_SAT_HOAN_CANH_KTXH()
        {
            InitializeComponent();
            KhoiTaoCombobox();
            raddtNgayBaoCao.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            cmbChiNhanh.SelectionChanged += new SelectionChangedEventHandler(cmbChiNhanh_SelectionChanged);
            cmbNhom.SelectionChanged += new SelectionChangedEventHandler(cmbNhom_SelectionChanged);

            isLoad = true;
        }

        private void KhoiTaoCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();

            //Khoi tao combobox chi nhanh
            Dispatcher.CurrentDispatcher.DelayInvoke("cmbChiNhanh", () =>
            {
                auto = new AutoComboBox();
                auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);
            }, TimeSpan.FromSeconds(0));


            //khoi tao combobox ngon ngu
            Dispatcher.CurrentDispatcher.DelayInvoke("cmbNgonNgu", () =>
            {
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
                auto = new AutoComboBox();
                auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());
            }, TimeSpan.FromSeconds(0));

            //khoi tao combobox dinh dang
            Dispatcher.CurrentDispatcher.DelayInvoke("cmbDinhDang", () =>
            {
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
                auto = new AutoComboBox();
                auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri());
                cmbDinhDang.IsEnabled = false;
            }, TimeSpan.FromSeconds(0));

            //Tao combobox phonggd
            Dispatcher.CurrentDispatcher.DelayInvoke("cmbPhongGD", () =>
            {
                auto = new AutoComboBox();
                auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue());
                LoadComboboxPhongGD();
            }, TimeSpan.FromSeconds(0));

            LoadComboboxKhuVuc();
            loadComboboxCum();
            LoadComboboxNhom();
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

        private void LoadComboboxKhuVuc()
        {
            try
            {
                lstSourceKhuVuc = new List<AutoCompleteEntry>();
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();
                machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
                maphonggd = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
                lstDieuKien.Add(machinhanh);
                lstDieuKien.Add(maphonggd);
                cmbKhuVuc.Items.Clear();
                auto.GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUCLIST.getValue(), lstDieuKien);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void loadComboboxCum()
        {
            try
            {
                lstSourceCum = new List<AutoCompleteEntry>();
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();
                machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
                maphonggd = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
                makhuvuc = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings[1];
                lstDieuKien.Add(machinhanh);
                lstDieuKien.Add(maphonggd);
                lstDieuKien.Add(makhuvuc);
                cmbCum.Items.Clear();
                auto.GenAutoComboBox(ref lstSourceCum, ref cmbCum, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM_KVUC_LIST.getValue(), lstDieuKien);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void LoadComboboxNhom()
        {
            try
            {
                lstSourceNhom = new List<AutoCompleteEntry>();
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();
                machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings[1];
                maphonggd = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings[3];
                makhuvuc = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings[1];
                macum = lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings[1];
                lstDieuKien.Add(maphonggd);
                lstDieuKien.Add(makhuvuc);
                lstDieuKien.Add(macum);
                cmbNhom.Items.Clear();
                auto.GenAutoComboBox(ref lstSourceNhom, ref cmbNhom, "COMBOBOX_NHOM_NQL", lstDieuKien);
                isLoad = true;
                LoadThongTinBoSung();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void LoadThongTinBoSung()
        {
            try
            {

                if (isLoad && !LObject.IsNullOrEmpty(lstSourceNhom) && lstSourceNhom.Count > 0)
                {
                    txtCanBoQuanLy.Text = lstSourceNhom.ElementAt(cmbNhom.SelectedIndex).KeywordStrings[6];
                }
                else txtCanBoQuanLy.Text = "";
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void LoadDsThanhVien()
        {
            try
            {
                machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
                maphonggd = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
                idKhuVuc = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings[1];
                idCum = lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings[1];
                manhom = lstSourceNhom.ElementAt(cmbNhom.SelectedIndex).KeywordStrings[0];
                DataSet dsNhanVien = new DataSet();
                BaoCaoProcess bcProcess = new BaoCaoProcess();
                dsNhanVien = bcProcess.GetKhachHangTheoNhom(manhom);
                if (dsNhanVien != null && dsNhanVien.Tables.Count > 0)
                {
                    grdKhachHang.DataContext = dsNhanVien.Tables[0].DefaultView;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        #endregion

        #region Xu ly giao dien

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();
        }

        private void cmbNhom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadThongTinBoSung();
            if (cmbNhom.SelectedIndex != -1) LoadDsThanhVien();
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxKhuVuc();
        }

        private void cmbKhuVuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loadComboboxCum();
        }

        private void cmbCum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxNhom();
        }
        #endregion

        #region Xu ly nghiep vu
        private void GetValuesOnForm()
        {
            lstIdKhachHang = new List<string>();
            machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
            maphonggd = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
            idKhuVuc = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings[1];
            idCum = lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings[1];
            idNhom = lstSourceNhom.ElementAt(cmbNhom.SelectedIndex).KeywordStrings[1];
            manhom = lstSourceNhom.ElementAt(cmbNhom.SelectedIndex).KeywordStrings[0];
            mangonngu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.FirstOrDefault();
            madinhdang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.FirstOrDefault();

            ngaybaocao = LDateTime.DateToString(Convert.ToDateTime(raddtNgayBaoCao.Value), ApplicationConstant.defaultDateTimeFormat);

            foreach (DataRowView drv in grdKhachHang.SelectedItems)
            {
                lstIdKhachHang.Add(drv["ID"].ToString());
                _idKhachHang = Convert.ToInt32(drv["ID"]);
            }
            if (lstIdKhachHang.Count > 0)
            {
                KhachHangProcess khProcess = new KhachHangProcess();
                dsKhachHang = khProcess.getThongTinKHTheoID(_idKhachHang);
                DataTable dtKhangHSo = null;
                if (dsKhachHang != null && dsKhachHang.Tables.Count > 0)
                {
                    dtKhangHSo = dsKhachHang.Tables[0];
                    tenkhachhang = dtKhangHSo.Rows[0]["TEN_KHANG"].ToString();
                    makhachhang = dtKhangHSo.Rows[0]["MA_KHANG"].ToString();
                    diachi = dtKhangHSo.Rows[0]["DIA_CHI"].ToString();
                    gioitinh = dtKhangHSo.Rows[0]["DD_GIOI_TINH"].ToString();
                    ngaysinh = dtKhangHSo.Rows[0]["DD_NGAY_SINH"].ToString();
                    socmnd = dtKhangHSo.Rows[0]["DD_GTLQ_SO"].ToString();
                    ngaycap = dtKhangHSo.Rows[0]["DD_GTLQ_NGAY_CAP"].ToString();
                    noicap = dtKhangHSo.Rows[0]["DD_GTLQ_NOI_CAP"].ToString();
                    sosohokhau = dtKhangHSo.Rows[0]["SO_HO_KHAU"].ToString();

                }
            }
        }

        private bool Validation()
        {
            try
            {
                if (cmbChiNhanh.SelectedIndex == -1)
                {
                    LMessage.ShowMessage("M.ResponseMessage.BaoCao.ChuaChonChiNhanh", LMessage.MessageBoxType.Warning);
                    cmbChiNhanh.Focus();
                    return false;
                }

                if (cmbNhom.SelectedIndex == -1)
                {
                    LMessage.ShowMessage("M.ResponseMessage.BaoCao.ChuaChonNhom", LMessage.MessageBoxType.Warning);
                    cmbNhom.Focus();
                    return false;
                }

                if (LObject.IsNullOrEmpty(ngaybaocao) || ngaybaocao.Equals(""))
                {
                    LMessage.ShowMessage("M.ResponseMessage.BaoCao.ChuaChonNgayBaoCao", LMessage.MessageBoxType.Warning);
                    raddtNgayBaoCao.Focus();
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return false;
            }
            return true;
        }

        public List<ThamSoBaoCao> GetParameters()
        {
            GetValuesOnForm();

            if (!Validation()) return null;
            
            List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();
            lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", machinhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@MaNhom", manhom, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@TenCanBoQuanLy", txtCanBoQuanLy.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", mangonngu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", madinhdang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            for (int i = 0; i < lstIdKhachHang.Count; i++)
            {
                lstThamSo.Add(new ThamSoBaoCao("@IdKhachHang", lstIdKhachHang[i], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            return lstThamSo;
        }
        #endregion

    }
}


