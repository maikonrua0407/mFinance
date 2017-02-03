using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
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
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.Common;

namespace PresentationWPF.BaoCao._BTV.NSTL
{
    /// <summary>
    /// Interaction logic for NSTL_THE_LUONG.xaml
    /// </summary>
    public partial class NSTL_THE_LUONG : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();

        public string magiaodich = "";
        public List<string> lstID = new List<string>();

        public string madinhdang;
        public string mangonngu;

        #endregion

        #region Khoi tao
        public NSTL_THE_LUONG()
        {
            InitializeComponent();
            LoadCombobox();
        }
        private void LoadCombobox()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            // Khởi tạo điều kiện gọi danh mục
            List<string> lstDieuKien = new List<string>();

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri());
            cmbDinhDang.IsEnabled = false;
        }
        #endregion

        #region Xu ly giao dien
        private void btnLayGT_Click(object sender, RoutedEventArgs e)
        {
            if (txtMaGD.Text.Trim() == "") return;
            LoadNhanVien();
        }
        private void LoadNhanVien()
        {
            string magiaodich = txtMaGD.Text.Trim();
            BaoCaoProcess bcProcess = new BaoCaoProcess();
            DataSet dsNhanVien = new DataSet();
            dsNhanVien = bcProcess.LayDanhSachNhanVienTinhLuong(magiaodich);
            if (dsNhanVien != null && dsNhanVien.Tables.Count > 0)
            {
                grid.DataContext = dsNhanVien.Tables[0].DefaultView;
            }
        }
        #endregion

        #region Xu ly nghiep vu
        private void GetFormData()
        {
            lstID = new List<string>();
            magiaodich = txtMaGD.Text.Trim();
            madinhdang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.FirstOrDefault();
            mangonngu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.FirstOrDefault();
            foreach (DataRowView drv in grid.SelectedItems)
            {
                lstID.Add(drv["id"].ToString());
            }
        }

        private bool Validation()
        {
            if (grid.SelectedItems.Count == 0)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                return false;
            }
            if (txtMaGD.Text.Trim() == "") return false;
            return true;
        }

        public List<ThamSoBaoCao> GetParameters()
        {
            if (!Validation())
            {
                LMessage.ShowMessage("Thiếu tham số cho báo cáo.", LMessage.MessageBoxType.Information);
                return null;
            }

            GetFormData();
            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaGiaoDich", magiaodich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", mangonngu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", madinhdang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));
            for (int i = 0; i < lstID.Count; i++)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@IdNhanVien", lstID[i], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            return listThamSoBaoCao;
        }
        #endregion
    }
}
