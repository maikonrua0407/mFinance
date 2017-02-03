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
using PresentationWPF.CustomControl;
using Utilities.Common;
using Presentation.Process.Common;
using Presentation.Process.KhachHangServiceRef;
using Presentation.Process;
using System.Data;
using Presentation.Process.PopupServiceRef;
using PresentationWPF.BaoCao.DungChung;


namespace PresentationWPF.BaoCao._BTV.KHTV
{
    /// <summary>
    /// Interaction logic for KHTV_THAM_DINH.xaml
    /// </summary>
    public partial class KHTV_THAM_DINH : UserControl
    {
        #region Khai bao bien
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        public DataTable dtKhachHang = new DataTable();
        public string machinhanh = "";
        public string maphonggd = "";
        public string tungay = "";
        public string denngay = "";
        public string ngaybaocao = "";
        public string ngonngu = "";
        public string dinhdang = "";
        public string thangbaocao = "";
        int _idKhachHang = 0;
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        DataSet dsKhachHang = new DataSet();

        #endregion

        #region Khoi tao
        public KHTV_THAM_DINH()
        {
            InitializeComponent();
            KhoiTaoCombobox();
            raddtNgayBaoCao.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);

            btnAdd.Click += new RoutedEventHandler(btnAdd_Click);
            btnDelete.Click += new RoutedEventHandler(btnDelete_Click);
        }

        private void KhoiTaoCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();

            //Khoi tao combobox chi nhanh
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);

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
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            HienDanhMucKhachHang();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            LoaiBoKhachHang();
        }

        private void txtMaKhachHang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                HienDanhMucKhachHang();
            }
        }

        private void LoaiBoKhachHang()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                DataRow dr;
                for (int i = 0; i < grKhachHang.SelectedItems.Count; i++)
                {
                    dr = (DataRow)grKhachHang.SelectedItems[i];
                    dtKhachHang.Rows.Remove(dr);
                }

                grKhachHang.ItemsSource = null;
                grKhachHang.ItemsSource = dtKhachHang;

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void HienDanhMucKhachHang()
        {
            try
            {
                List<string> lstDieuKien = new List<string>();
                KhachHangProcess khProcess = new KhachHangProcess();
                string lstIDKH = "";
                lstIDKH = "(0)";
                machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                //lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add(machinhanh);
                lstDieuKien.Add(lstIDKH);
                lstDieuKien.Add("NULL");
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_KHACHHANG_BAOCAO", lstDieuKien);
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
                    DataTable dt = new DataTable();
                    dt = lstPopup.CopyToDataTable();
                    if (!LObject.IsNullOrEmpty(dt) && dt.Rows.Count > 0)
                    {
                        if (dt.Columns.Contains("STT")) dt.Columns.Remove("STT");

                        if (LObject.IsNullOrEmpty(dtKhachHang)) dtKhachHang = new DataTable();
                        else dt.Merge(dtKhachHang);

                        var result = dt.AsEnumerable().Select(x => x).Distinct(System.Data.DataRowComparer.Default).ToList();
                        dtKhachHang = result.CopyToDataTable();

                        grKhachHang.ItemsSource = null;
                        grKhachHang.ItemsSource = dtKhachHang;
                    }

                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void LayDSDonVayVon(int idKhachHang)
        {
            //try
            //{
            //    DataSet dsDonVayVon = new DataSet();
            //    TinDungProcess tdProcess = new TinDungProcess();
            //    dsDonVayVon = tdProcess.LayDSDonVayVon(idKhachHang);
            //    if (dsDonVayVon == null) return;
            //    //raddgrThongTinDon.DataContext = dsDonVayVon.Tables[0].DefaultView;                
            //    raddgrThongTinDon.ItemsSource = dsDonVayVon.Tables[0].DefaultView;
            //    raddgrThongTinDon.Rebind();
            //}
            //catch (Exception ex)
            //{
            //    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            //}
        }

        #endregion

        #region Xu ly giao dien

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        private void GetValuesOnForm()
        {
            machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            ngaybaocao = ClientInformation.NgayLamViecHienTai;
            ngonngu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            dinhdang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();
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

                if (LObject.IsNullOrEmpty(dtKhachHang) || dtKhachHang.Rows.Count <= 0)
                {
                    LMessage.ShowMessage("M.ResponseMessage.BaoCao.ChuaChonMaKhachHang", LMessage.MessageBoxType.Warning);
                    grKhachHang.Focus();
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
            if (!Validation()) return null;
            GetValuesOnForm();
            List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();
            lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", machinhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            foreach(DataRow dr in dtKhachHang.Rows)
            {
                lstThamSo.Add(new ThamSoBaoCao("@MaKhachHang", dr["MA_KHANG"].ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            foreach (DataRow dr in dtKhachHang.Rows)
            {
                lstThamSo.Add(new ThamSoBaoCao("@IdKhachHang", dr["ID"].ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ngonngu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", dinhdang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));
            return lstThamSo;
        }
    }
}
