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

namespace PresentationWPF.BaoCao._BIDV.TDVM
{
    /// <summary>
    /// Interaction logic for TDVM_BAO_CAO_DE_XUAT_TIN_DUNG.xaml
    /// </summary>
    public partial class TDVM_BAO_CAO_DE_XUAT_TIN_DUNG : UserControl
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
        DataSet dsDXVV = new DataSet();

        public string _idDXVV = "0";
        public string idKhangHang = "";
        public string maDXVV = "";
        public string maKhachHang = "";
        public string tenDXVV = "";
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
        public string hienThiNgayBC;
        public string makhuvuc;
        public string macum;
        public string manhom;
        List<string> lstMaDXVV = null;
        List<string> lstIdKhangHang = null;

        public string madinhdang;
        public string mangonngu;
        public bool isLoad = false;
        #endregion

        #region Khoi tao
        public TDVM_BAO_CAO_DE_XUAT_TIN_DUNG()
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
                //auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());
                auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ClientInformation.NgonNgu);
            }, TimeSpan.FromSeconds(0));

            //khoi tao combobox dinh dang
            Dispatcher.CurrentDispatcher.DelayInvoke("cmbDinhDang", () =>
            {
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
                auto = new AutoComboBox();
                //auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri());
                auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ReportInformation.DinhDang);
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
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }


        private void LoadDsDXVVMoi()
        {
            try
            {
                machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
                maphonggd = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
                idKhuVuc = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings[1];
                idCum = lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings[1];
                manhom = lstSourceNhom.ElementAt(cmbNhom.SelectedIndex).KeywordStrings[0];
                macum = lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings[0];
                makhuvuc = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings[0];
                DataSet dsDXVV = new DataSet();
                BaoCaoProcess bcProcess = new BaoCaoProcess();
                dsDXVV = bcProcess.GetDXVVMoiTheoNhom(manhom);
                if (dsDXVV != null && dsDXVV.Tables.Count > 0)
                {
                    grdDXVV.DataContext = dsDXVV.Tables[0].DefaultView;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void LoadDsDXVVCu()
        {
            try
            {
                machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
                maphonggd = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
                idKhuVuc = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings[1];
                idCum = lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings[1];
                manhom = lstSourceNhom.ElementAt(cmbNhom.SelectedIndex).KeywordStrings[0];
                macum = lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings[0];
                makhuvuc = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings[0];
                DataSet dsDXVV = new DataSet();
                BaoCaoProcess bcProcess = new BaoCaoProcess();
                dsDXVV = bcProcess.GetDXVVCuTheoNhom(manhom);
                if (dsDXVV != null && dsDXVV.Tables.Count > 0)
                {
                    grdDXVV.DataContext = dsDXVV.Tables[0].DefaultView;
                }
                grdDXVV.SelectAll();
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
            if (cmbNhom.SelectedIndex != -1) LoadDsDXVVMoi();
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
            lstMaDXVV = new List<string>();
            machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
            maphonggd = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
            idKhuVuc = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings[1];
            idCum = lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings[1];
            idNhom = lstSourceNhom.ElementAt(cmbNhom.SelectedIndex).KeywordStrings[1];
            manhom = lstSourceNhom.ElementAt(cmbNhom.SelectedIndex).KeywordStrings[0];
            macum = lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings[0];
            makhuvuc = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings[0];
            mangonngu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.FirstOrDefault();
            madinhdang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.FirstOrDefault();

            ngaybaocao = LDateTime.DateToString(Convert.ToDateTime(raddtNgayBaoCao.Value), ApplicationConstant.defaultDateTimeFormat);
            hienThiNgayBC = Convert.ToDateTime(raddtNgayBaoCao.Value).ToLongDateString();
            foreach (DataRowView drv in grdDXVV.SelectedItems)
            {
                lstMaDXVV.Add(drv["MA_DXVVVM"].ToString());
                maDXVV = drv["MA_DXVVVM"].ToString();
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

                if (grdDXVV.SelectedItems.Count < 1)
                {
                    LMessage.ShowMessage("M.ResponseMessage.BaoCao.ChuaChonDXVV", LMessage.MessageBoxType.Warning);
                    grdDXVV.Focus();
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
            lstThamSo.Add(new ThamSoBaoCao("@MaPhongGiaoDich", maphonggd, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@MaNhom", manhom, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@IdNhom", idNhom, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@MaCum", macum, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@MaKhuVuc", makhuvuc, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", hienThiNgayBC, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", mangonngu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", madinhdang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));
            for (int i = 0; i < lstMaDXVV.Count; i++)
            {
                lstThamSo.Add(new ThamSoBaoCao("@MaDXVV", lstMaDXVV[i], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            return lstThamSo;
        }
        #endregion

        private void chkCu_Checked(object sender, RoutedEventArgs e)
        {
            if (cmbNhom.SelectedIndex != -1) LoadDsDXVVCu();
        }

        private void chkCu_Unchecked(object sender, RoutedEventArgs e)
        {
            if (cmbNhom.SelectedIndex != -1) LoadDsDXVVMoi();
        }

    }
}
