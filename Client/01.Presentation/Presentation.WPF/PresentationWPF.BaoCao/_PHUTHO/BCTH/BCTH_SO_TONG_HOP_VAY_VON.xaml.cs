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
namespace PresentationWPF.BaoCao._PHUTHO.BCTH
{
    /// <summary>
    /// Interaction logic for BCTH_SO_TONG_HOP_VAY_VON.xaml
    /// </summary>
    public partial class BCTH_SO_TONG_HOP_VAY_VON : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        ListCheckBoxCombo lstSourceNguonVon = new ListCheckBoxCombo();

        public string machinhanh = "";
        public string maphonggd = "";
        public string NgayDuLieu = "";
        public string NgayDauThang = "";
        public string ngaybaocao = "";
        public string ngaytinhlke = "";
        public string mangonngu = "";
        public string madinhdang = "";
        public List<string> lstID = null;
        List<string> lstNguonVon = new List<string>();
        #endregion

        #region Khoi tao
        public BCTH_SO_TONG_HOP_VAY_VON()
        {
            InitializeComponent();
            LoadCombobox();
            raddtNgayDuLieu.Value = raddtNgayBaoCao.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
        }
        private void LoadCombobox()
        {
            try
            {
                List<string> lstDieuKien = new List<string>();
                AutoComboBox auto = new AutoComboBox();

                lstDieuKien = new List<string>();
                //Tao combobox chi nhanh
                Dispatcher.CurrentDispatcher.DelayInvoke("cmbChiNhanh", () =>
                {
                    auto = new AutoComboBox();
                    auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);
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

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        #endregion

        #region Xu ly nghiep vu

        private void GetValuesOnForm()
        {
            machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
            ngaybaocao = Convert.ToDateTime(raddtNgayBaoCao.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            NgayDuLieu = Convert.ToDateTime(raddtNgayDuLieu.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            NgayDauThang = Convert.ToDateTime(raddtNgayDuLieu.Value).GetFirstDateOfMonth(ApplicationConstant.defaultDateTimeFormat);
            mangonngu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.FirstOrDefault();
            madinhdang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.FirstOrDefault();
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
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayDuLieu", NgayDuLieu, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayDauThang", NgayDauThang, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", mangonngu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", madinhdang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            return listThamSoBaoCao;
        }
        #endregion
    }
}
