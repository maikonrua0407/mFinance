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
using Telerik.Windows.Controls;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.Common;

namespace PresentationWPF.BaoCao._BIDV.TDTT
{
    /// <summary>
    /// Interaction logic for TDTT_SAO_KE_TIN_DUNG_TH.xaml
    /// </summary>
    public partial class TDTT_SAO_KE_TIN_DUNG_TH : UserControl
    {
        #region Khai bao

        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTieuChiNhom = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNhomNo = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNguonVonChoVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceMucDichVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();

        // Các tham số báo cáo từ form điều kiện
        public string MaBaoCao;
        public string MaChiNhanh;
        public string TenChiNhanh;
        public string MaPhongGiaoDich;
        public string TenPhongGiaoDich;
        public string NgayBaoCao;
        public string MaNguoiLap;
        public string TenNguoiLap;

        public string NguonVonChoVay;
        public string MucDichVay;
        public string NhomNo;

        public string NgayChotSoLieu;
        public string TieuChi;

        public string MaNgonNgu;
        public string MaDinhDang;
        List<string> lstNhomNo = null;
        List<string> lstMucDichVay = null;
        List<string> lstSanPham = null;

        #endregion

        #region Khoi tao
        public TDTT_SAO_KE_TIN_DUNG_TH()
        {
            InitializeComponent();
            LoadCombobox();
            LoadTreeview();
            raddtNgayBaoCao.Value = raddtNgayChotSoLieu.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
        }

        private void LoadCombobox()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            // Khởi tạo điều kiện gọi danh mục
            List<string> lstDieuKien = new List<string>();

            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);

            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue());
            LoadComboboxPhongGD();

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.BCAO_TDUNG_TIEU_CHI_NHOM.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceTieuChiNhom, ref cmbTieuChiLoc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            //auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());
            auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ClientInformation.NgonNgu);

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            //auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri());
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ReportInformation.DinhDang);
            cmbDinhDang.IsEnabled = false;
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

        private void LoadTreeview()
        {
            string machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            RadComboBox cb = new RadComboBox();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NHOM_NO.getValue());
            lstSourceNhomNo = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstSourceNhomNo, ref cb, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

            RadTreeViewItem rootItem = new RadTreeViewItem();
            rootItem.Header = LLanguage.SearchResourceByKey("U.DMUC_LOAI.NHOM_NO") + " (" + lstSourceNhomNo.Count.ToString() + ")";
            rootItem.Tag = string.Empty;
            rootItem.IsExpanded = true;
            rootItem.IsChecked = false;
            tvwTreeNhomNo.Items.Add(rootItem);
            BuildTree(rootItem, lstSourceNhomNo);

            auto = new AutoComboBox();
            lstDieuKien = new List<string>();
            cb = new RadComboBox();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.MUC_DICH_VAY_VON.getValue());
            lstSourceMucDichVay = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstSourceMucDichVay, ref cb, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

            rootItem = new RadTreeViewItem();
            rootItem.Header = LLanguage.SearchResourceByKey("U.TinDung.ucDonVayVonCT_01.GrvMucDichSuDungVon") + " (" + lstSourceMucDichVay.Count.ToString() + ")";
            rootItem.Tag = string.Empty;
            rootItem.IsExpanded = true;
            rootItem.IsChecked = false;
            tvwTreeMucDichVay.Items.Add(rootItem);
            BuildTree(rootItem, lstSourceMucDichVay);
        }

        private void LoadTreeSanPham()
        {
            string machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            RadComboBox cb = new RadComboBox();
            auto = new AutoComboBox();
            lstDieuKien = new List<string>();
            RadTreeViewItem rootItem = new RadTreeViewItem();
            cb = new RadComboBox();
            lstDieuKien.Add(machinhanh);
            lstSourceNguonVonChoVay = new List<AutoCompleteEntry>();
            tvwTreeNguonVonChoVay.Items.Clear();
            auto.GenAutoComboBox(ref lstSourceNguonVonChoVay, ref cb, DatabaseConstant.DanhSachTruyVan.COMBOBOX_SAN_PHAM_TDLIST.getValue(), lstDieuKien);

            rootItem = new RadTreeViewItem();
            rootItem.Header = LLanguage.SearchResourceByKey("U.DMUC_GTRI.BCAO_TDUNG_TIEU_CHI_NHOM.NGUON_VON_VAY") + " (" + lstSourceNguonVonChoVay.Count.ToString() + ")";
            rootItem.Tag = string.Empty;
            rootItem.IsExpanded = true;
            rootItem.IsChecked = false;
            tvwTreeNguonVonChoVay.Items.Add(rootItem);
            BuildTree(rootItem, lstSourceNguonVonChoVay);
        }

        /// <summary>
        /// Build tree
        /// </summary>
        /// <param name="item"></param>
        protected void BuildTree(RadTreeViewItem item, List<AutoCompleteEntry> lstSource)
        {
            foreach (AutoCompleteEntry entry in lstSource)
            {
                RadTreeViewItem subItem = new RadTreeViewItem();
                subItem.Header = entry.DisplayName;
                subItem.Tag = entry.KeywordStrings.First();
                subItem.Uid = entry.KeywordStrings.ElementAt(1);
                //subItem.Tag = row["id"].ToString();
                subItem.IsExpanded = true;
                subItem.IsChecked = false;
                item.Items.Add(subItem);
            }
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();
            LoadTreeSanPham();
        }
        #endregion

        #region Xu ly nghiep vu
        private void GetValuesOnForm()
        {
            MaChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
            MaPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
            NgayBaoCao = Convert.ToDateTime(raddtNgayBaoCao.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            NgayChotSoLieu = Convert.ToDateTime(raddtNgayChotSoLieu.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            TieuChi = lstSourceTieuChiNhom.ElementAt(cmbTieuChiLoc.SelectedIndex).KeywordStrings.FirstOrDefault();
            MaNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.FirstOrDefault();
            MaDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.FirstOrDefault();

            #region Lay nhom no
            lstNhomNo = new List<string>();
            foreach (RadTreeViewItem item in tvwTreeNhomNo.CheckedItems)
            {
                if (item.Tag != null)
                {
                    lstNhomNo.Add(item.Tag.ToString());
                }
            }
            #endregion

            #region Lay muc dich vay
            lstMucDichVay = new List<string>();
            foreach (RadTreeViewItem item in tvwTreeMucDichVay.CheckedItems)
            {
                if (item.Tag != null)
                {
                    lstMucDichVay.Add(item.Tag.ToString());
                }
            }
            #endregion

            #region Lay san pham
            lstSanPham = new List<string>();
            foreach (RadTreeViewItem item in tvwTreeNguonVonChoVay.CheckedItems)
            {
                if (item.Tag != null)
                {
                    lstSanPham.Add(item.Tag.ToString());
                }
            }
            #endregion
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
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGD", MaPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayChotDL", NgayChotSoLieu, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TieuChi", TieuChi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            for (int i = 0; i < lstMucDichVay.Count; i++)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MucDichVay", lstMucDichVay[i], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            for (int i = 0; i < lstNhomNo.Count; i++)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@NhomNoHienTai", lstNhomNo[i], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            for (int i = 0; i < lstSanPham.Count; i++)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaSanPham", lstSanPham[i], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            return listThamSoBaoCao;
        }
        #endregion

    }
}
