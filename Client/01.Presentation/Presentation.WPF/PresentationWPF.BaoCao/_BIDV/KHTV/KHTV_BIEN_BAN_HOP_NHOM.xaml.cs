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
using Presentation.Process.PopupServiceRef;

namespace PresentationWPF.BaoCao._BIDV.KHTV
{
    /// <summary>
    /// Interaction logic for KHTV_BIEN_BAN_HOP_NHOM.xaml
    /// </summary>
    public partial class KHTV_BIEN_BAN_HOP_NHOM : UserControl
    {
        #region Khai bao bien
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        DataSet dsKhachHang = new DataSet();
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        public string manhom = "";
        public string ngaybaocao = "";

        public string mangonngu = "";
        public string madinhdang = "";

        List<string> lstDXVV = null;
        #endregion

        #region Khoi tao
        public KHTV_BIEN_BAN_HOP_NHOM()
        {
            InitializeComponent();
            KhoiTaoCombobox();
            raddtNgayBaoCao.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
        }
        public void KhoiTaoCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
            auto = new AutoComboBox();
            //auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());
            auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ClientInformation.NgonNgu);

            //khoi tao combobox dinh dang
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
            auto = new AutoComboBox();
            //auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri());
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ReportInformation.DinhDang);
            cmbDinhDang.IsEnabled = false;
        }
        #endregion

        #region Xu ly giao dien
        private void HienDanhMucNhom()
        {
            try
            {
                DanhMucProcess dmProcess = new DanhMucProcess();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DM_CUM", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(true, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup.Count > 0)
                {
                    DataRow dr = lstPopup[0];
                    txtMaNhom.Text = dr["MA_NHOM"].ToString();
                    txtTenNhom.Text = dr["TEN_NHOM"].ToString();
                    if (chkCu.IsChecked == true)
                        LoadDsDXVVCu();
                    else
                        LoadDsDXVVMoi();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void btnNhom_Click(object sender, RoutedEventArgs e)
        {
            HienDanhMucNhom();
        }

        private void txtMaKhachHang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                HienDanhMucNhom();
            }
        }
        #endregion

        #region lay thong tin in bao cao
        private void GetValuesOnForm()
        {
            try
            {
                lstDXVV = new List<string>();
                manhom = txtMaNhom.Text.Trim();
                DataRowView dr = (DataRowView)grdDXVV.SelectedItem;
                ngaybaocao = ClientInformation.NgayLamViecHienTai;
                mangonngu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.FirstOrDefault();
                madinhdang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.FirstOrDefault();

                foreach (DataRowView drv in grdDXVV.SelectedItems)
                {
                    lstDXVV.Add(drv["MA_DXVVVM"].ToString());
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
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
            List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();
            lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@MaPhongGD", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@MaNhom", manhom, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", mangonngu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", madinhdang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            for (int i = 0; i < lstDXVV.Count; i++)
            {
                lstThamSo.Add(new ThamSoBaoCao("@MaDXVV", lstDXVV[i], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            return lstThamSo;
        }
        #endregion

        private void chkCu_Checked(object sender, RoutedEventArgs e)
        {
            if (!txtMaNhom.Text.Trim().Equals("")) LoadDsDXVVCu();
        }

        private void chkCu_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!txtMaNhom.Text.Trim().Equals("")) LoadDsDXVVMoi();
        }

        private void LoadDsDXVVMoi()
        {
            try
            {
                DataSet dsDXVV = new DataSet();
                BaoCaoProcess bcProcess = new BaoCaoProcess();
                dsDXVV = bcProcess.GetDXVVMoiTheoNhom(txtMaNhom.Text);
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
                DataSet dsDXVV = new DataSet();
                BaoCaoProcess bcProcess = new BaoCaoProcess();
                dsDXVV = bcProcess.GetDXVVCuTheoNhom(txtMaNhom.Text);
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
    }
}
