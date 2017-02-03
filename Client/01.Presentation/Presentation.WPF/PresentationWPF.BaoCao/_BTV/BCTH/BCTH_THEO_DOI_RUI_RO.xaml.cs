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

namespace PresentationWPF.BaoCao._BTV.BCTH
{
    /// <summary>
    /// Interaction logic for BCTH_THEO_DOI_RUI_RO.xaml
    /// </summary>
    public partial class BCTH_THEO_DOI_RUI_RO : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKhuVuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum = new List<AutoCompleteEntry>();
        #endregion

        #region Khoi tao
        public BCTH_THEO_DOI_RUI_RO()
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

                //Tao combobox khu vuc
                Dispatcher.CurrentDispatcher.DelayInvoke("cmbKhuVuc", () =>
                {
                    LoadComboboxKhuVuc();
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

        private void LoadComboboxKhuVuc()
        {
            try
            {
                lstSourceKhuVuc = new List<AutoCompleteEntry>();
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();
                string machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
                string maphonggd = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
                lstDieuKien.Add(machinhanh);
                lstDieuKien.Add(maphonggd);
                cmbKhuVuc.Items.Clear();
                lstSourceKhuVuc.Add(new AutoCompleteEntry("Tất cả", "%", "%"));
                auto.GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUCLIST.getValue(), lstDieuKien);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void LoadDSCum()
        {
            try
            {
                string machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
                string maphonggd = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
                string idKhuVuc = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings[1];
                DataSet dsCum = new DataSet();
                BaoCaoProcess bcProcess = new BaoCaoProcess();
                dsCum = bcProcess.LayDSCum(idKhuVuc, machinhanh, maphonggd);
                if (dsCum != null && dsCum.Tables.Count > 0)
                {
                    grdKhachHang.DataContext = dsCum.Tables[0].DefaultView;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        #endregion

        #region Xu ly giao dien

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbChiNhanh.SelectedIndex != -1) LoadComboboxPhongGD();
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPhongGD.SelectedIndex != -1) LoadComboboxKhuVuc();
        }

        private void cmbKhuVuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbKhuVuc.SelectedIndex != -1) LoadDSCum();
        }
        #endregion

        #region Xu ly nghiep vu
        private void GetValuesOnForm()
        {
            
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

            return listThamSoBaoCao;
        }
        #endregion
    }
}
