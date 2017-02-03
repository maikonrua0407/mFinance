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

namespace PresentationWPF.BaoCao._BTRE.TDVM
{
    /// <summary>
    /// Interaction logic for TDVM_BC_SO_LIEU_TDUNG_XA.xaml
    /// </summary>
    public partial class TDVM_BC_SO_LIEU_TDUNG_XA : UserControl
    {
        #region Khai bao bien
        ListCheckBoxCombo lstSourceChiNhanh = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourceNguonVon = new ListCheckBoxCombo();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();        
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKhuVuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNhom = new List<AutoCompleteEntry>();

        public string machinhanh = "";
        public string maphonggd = "";
        public string manhom = "";
        public string tungay = "";
        public string denngay = "";
        public string ngaybaocao = "";
        public string ngonngu = "";
        public string dinhdang = "";
        public string thangbaocao = "";
        public string ngaycongnhan = "";
        public bool isLoad = false;

        List<string> lstMaChiNhanh = new List<string>();
        List<string> lstNguonVon = new List<string>();
        #endregion

        #region Khoi tao
        public TDVM_BC_SO_LIEU_TDUNG_XA()
        {
            InitializeComponent();
            KhoiTaoCombobox();
            radtThang.Value = raddtNgayBaoCao.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
        }

        private void KhoiTaoCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();

            //Khoi tao combobox chi nhanh
            Dispatcher.CurrentDispatcher.DelayInvoke("cmbChiNhanh", () =>
            {                
                lstDieuKien.Add(ClientInformation.TenDangNhap);
                lstDieuKien.Add(ClientInformation.MaDonViQuanLy);
                new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANHLIST.getValue(), lstDieuKien);
            }, TimeSpan.FromSeconds(0));

            //Khoi tao combobox chi nhanh
            Dispatcher.CurrentDispatcher.DelayInvoke("cmbNguonVon", () =>
            {
                new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceNguonVon, ref cmbNguonVon, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NGUON_VON.getValue());
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("cmbNgonNgu", () =>
            {
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
                // khởi tạo combobox
                auto = new AutoComboBox();
                auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("cmbDinhDang", () =>
            {
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
                // khởi tạo combobox
                auto = new AutoComboBox();
                auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri());
                cmbDinhDang.SelectedIndex = lstSourceDinhDang.FindIndex(e => e.KeywordStrings[0].Equals(ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri()));
            }, TimeSpan.FromSeconds(0));

        }

        private void loadComboboxNguonVon()
        {
            try
            {
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
        #endregion

        #region Xu ly nghiep vu
        private void cmbNguonVon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loadComboboxNguonVon();
        }

        private void GetValuesOnForm()
        {
            DateTime ngayBaoCao = new DateTime();
            if (raddtNgayBaoCao.Value is DateTime) ngayBaoCao = (DateTime)raddtNgayBaoCao.Value;

            lstMaChiNhanh = new List<string>();
            foreach (AutoCompleteCheckBox ChiNhanh in lstSourceChiNhanh.Where(e => e.CheckedMember == true))
            {
                lstMaChiNhanh.Add(ChiNhanh.ValueMember.FirstOrDefault());                
            }

            lstNguonVon = new List<string>();
            foreach (AutoCompleteCheckBox NguonVon in lstSourceNguonVon.Where(e => e.CheckedMember == true))
            {
                lstNguonVon.Add(NguonVon.ValueMember.FirstOrDefault());
            }
            
            ngaybaocao = (raddtNgayBaoCao.Value is DateTime) ? ngayBaoCao.ToString("yyyyMM") : "";
            thangbaocao = (radtThang.Value is DateTime) ? ngayBaoCao.ToString("yyyyMMdd") : "";
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

                if (lstSourceChiNhanh.Where(e => e.CheckedMember == true).ToList().Count < 1)
                {
                    LMessage.ShowMessage("M.ResponseMessage.BaoCao.ChuaChonChiNhanh", LMessage.MessageBoxType.Warning);
                    return false;
                }
                if (lstSourceNguonVon.Where(e => e.CheckedMember == true).ToList().Count < 1)
                {
                    LMessage.ShowMessage("Chưa chọn nguồn vốn", LMessage.MessageBoxType.Warning);
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

            foreach (string ChiNhanh in lstMaChiNhanh)
            {
                lstThamSo.Add(new ThamSoBaoCao("@ChiNhanh", ChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            foreach (string NguonVon in lstNguonVon)
            {
                lstThamSo.Add(new ThamSoBaoCao("@NguonVon", NguonVon, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@MaPhongGD", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@NguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@Thang", thangbaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            return lstThamSo;
        }
        #endregion
    }
}
