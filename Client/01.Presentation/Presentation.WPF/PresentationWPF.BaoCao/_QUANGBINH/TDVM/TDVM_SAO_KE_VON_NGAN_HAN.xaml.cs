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
using PresentationWPF.BaoCao.DungChung;
using Telerik.Windows.Controls;
using Presentation.Process.Common;

namespace PresentationWPF.BaoCao._QUANGBINH.TDVM
{
    /// <summary>
    /// Interaction logic for TDVM_SAO_KE_VON_NGAN_HAN.xaml
    /// </summary>
    public partial class TDVM_SAO_KE_VON_NGAN_HAN : UserControl
    {
        #region Khai bao

        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTieuChiNhom = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceSapXep = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNhomNo = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTSDB = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNguonVonChoVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceMucDichVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        //List<AutoCompleteEntry> lstSourceCum = new List<AutoCompleteEntry>();
        //List<AutoCompleteEntry> lstSourceCum_Select = new List<AutoCompleteEntry>();
        ListCheckBoxCombo lstSourceCum = new ListCheckBoxCombo();
        //ListCheckBoxCombo lstSourceCum_Select = new ListCheckBoxCombo();
        
        // Các tham số báo cáo từ form điều kiện
        public string MaBaoCao;
        public string MaChiNhanh;
        public string TenChiNhanh;
        public string MaPhongGiaoDich;
        public string TenPhongGiaoDich;
        public List<string> MaCum;
        public string NgayBaoCao;
        public string MaNguoiLap;
        public string TenNguoiLap;

        public string NguonVonChoVay;
        public string MucDichVay;
        public string NhomNo;
        public string LoaiTSDB;

        public string NgayChotSoLieu;
        public string TieuChiNhom;
        public string SapXep;
        public string MaLoaiTien;

        public string MaNgonNgu;
        public string MaDinhDang;

        public string TenCum;
        public string TuNgay;
        #endregion

        public TDVM_SAO_KE_VON_NGAN_HAN()
        {
            InitializeComponent();
            LoadCombobox();
            LoadTreeview();
            LoadTreeSanPham();
            //LoadCombobox(); ???
            // Nếu người dùng là đơn vị >> disable thông tin chi nhánh
            if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()) ||
                ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()))
            {
                cmbChiNhanh.IsEnabled = false;
            }
            else
            {
                cmbChiNhanh.IsEnabled = true;
            }
            raddtTuNgay.Value = raddtNgayChotSoLieu.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            raddtNgayBaoCao.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);

        }

        private void LoadTreeview()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            RadComboBox cb = new RadComboBox();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NHOM_NO.getValue());
            lstSourceNhomNo = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstSourceNhomNo, ref cb, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

            RadTreeViewItem rootItem = new RadTreeViewItem();
            rootItem.Header = LLanguage.SearchResourceByKey("Nhóm nợ ") + " (" + lstSourceNhomNo.Count.ToString() + ")";
            rootItem.Tag = string.Empty;
            rootItem.IsExpanded = true;
            rootItem.IsChecked = false;
            tvwTreeNhomNo.Items.Add(rootItem);
            BuildTree(rootItem, lstSourceNhomNo);

            auto = new AutoComboBox();
            lstDieuKien = new List<string>();
            cb = new RadComboBox();
            lstSourceNguonVonChoVay = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstSourceNguonVonChoVay, ref cb, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NGUON_VON_CT.getValue(), lstDieuKien);

            rootItem = new RadTreeViewItem();
            rootItem.Header = LLanguage.SearchResourceByKey("Nguồn vốn vay") + " (" + lstSourceNguonVonChoVay.Count.ToString() + ")";
            rootItem.Tag = string.Empty;
            rootItem.IsExpanded = true;
            rootItem.IsChecked = false;
            tvwTreeNguonVonChoVay.Items.Add(rootItem);
            BuildTree(rootItem, lstSourceNguonVonChoVay);

            auto = new AutoComboBox();
            lstDieuKien = new List<string>();
            cb = new RadComboBox();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.MUC_DICH_VAY_VON.getValue());
            lstSourceMucDichVay = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstSourceMucDichVay, ref cb, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

            rootItem = new RadTreeViewItem();
            rootItem.Header = LLanguage.SearchResourceByKey("Mục đích vay vốn") + " (" + lstSourceMucDichVay.Count.ToString() + ")";
            rootItem.Tag = string.Empty;
            rootItem.IsExpanded = true;
            rootItem.IsChecked = false;
            tvwTreeMucDichVay.Items.Add(rootItem);
            BuildTree(rootItem, lstSourceMucDichVay);
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

        /// <summary>
        /// Khởi tạo các combobox
        /// </summary>
        private void LoadCombobox()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            // Khởi tạo điều kiện gọi danh mục
            List<string> lstDieuKien = new List<string>();

            // khởi tạo combobox
            auto = new AutoComboBox();
            lstSourceChiNhanh.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);

            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue());
            //new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceCum, ref cmbCum, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM_ALL.getValue());
            LoadComboboxPhongGD();
            LoadComboboxCum();

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DONG_TIEN_CAC_NUOC.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);            

            //lstDieuKien = new List<string>();
            //lstDieuKien.Add(DatabaseConstant.DanhMuc.BCAO_TDUNG_TIEU_CHI_NHOM.getValue());
            // khởi tạo combobox
            //auto = new AutoComboBox();
            //auto.GenAutoComboBox(ref lstSourceTieuChiNhom, ref cmbTieuChiLoc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.BCAO_TDUNG_SAP_XEP.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceSapXep, ref cmbSapXepTheo, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

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
            lstSourcePhongGD_Select.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
            // khởi tạo combobox
            auto = new AutoComboBox();
            cmbPhongGD.Items.Clear();
            auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
            cmbPhongGD.SelectedIndex = 0;
            cmbPhongGD.IsEnabled = !maChiNhanh.Equals("%");
        }

        private void LoadComboboxCum()
        {
            if (cmbChiNhanh.SelectedIndex >= 0 && cmbPhongGD.SelectedIndex >= 0)
            {
                // khởi tạo combobox
                AutoComboBoxListCheckes auto = new AutoComboBoxListCheckes();
            
                lstSourceCum = new ListCheckBoxCombo();
                string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                string maPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
                
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(maChiNhanh);
                lstDieuKien.Add(maPhongGiaoDich);                
                auto.GenAutoComboBox(ref lstSourceCum, ref cmbCum, "COMBOBOX_CUM_LIST", lstDieuKien);
                cmbCum.SelectedIndex = 0;
                cmbCum.IsEnabled = !maPhongGiaoDich.Equals("%");
            }
        }

        private void LoadTreeSanPham()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            RadComboBox cb = new RadComboBox();
            MaChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
            lstDieuKien.Add("''" + MaChiNhanh + "''");
            lstDieuKien.Add("%");
            lstDieuKien.Add("1");
            lstDieuKien.Add("0");
            lstSourceLoaiTSDB = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstSourceLoaiTSDB, ref cb, "COMBOBOX_SAN_PHAM_TD", lstDieuKien);

            RadTreeViewItem rootItem = new RadTreeViewItem();
            rootItem.Header = LLanguage.SearchResourceByKey("Sản phẩm tím dụng") + " (" + lstSourceLoaiTSDB.Count.ToString() + ")";
            rootItem.Tag = string.Empty;
            rootItem.IsExpanded = true;
            rootItem.IsChecked = false;
            tvwTreeLoaiTSDB.Items.Clear();
            tvwTreeLoaiTSDB.Items.Add(rootItem);
            BuildTree(rootItem, lstSourceLoaiTSDB);
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();
            LoadTreeSanPham();
        }

        private void GetFormData()
        {
            string maNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            string maDonVi = string.Empty;
            string tenPhongGiaoDich = string.Empty;
            if (lstSourcePhongGD_Select.Count > 0)
            {
                maDonVi = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
                tenPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).DisplayName;
            }
            DateTime ngayBaoCao = new DateTime();
            if (raddtNgayBaoCao.Value is DateTime)
                ngayBaoCao = (DateTime)raddtNgayBaoCao.Value;
            DateTime ngayChotSoLieu = new DateTime();
            if (raddtNgayChotSoLieu.Value is DateTime)
                ngayChotSoLieu = (DateTime)raddtNgayChotSoLieu.Value;
            DateTime tuNgay = new DateTime();
            if (raddtTuNgay.Value is DateTime)
                tuNgay = (DateTime)raddtTuNgay.Value;
            string maLoaiTien = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.First();

            List<string> lstNhomNo = new List<string>();
            string nhomNo = "";
            foreach (RadTreeViewItem item in tvwTreeNhomNo.CheckedItems)
            {
                if (item.Tag != null)
                {
                    string ma = item.Tag.ToString();
                    if (!ma.IsNullOrEmptyOrSpace())
                    {
                        lstNhomNo.Add(ma);
                        nhomNo += ma + "#";
                    }
                }
            }
            if (nhomNo != "")
                nhomNo = nhomNo.Substring(0, nhomNo.Length - 1);

            List<string> lstTSDB = new List<string>();
            string loaiTSDB = "";
            foreach (RadTreeViewItem item in tvwTreeLoaiTSDB.CheckedItems)
            {
                if (item.Tag != null)
                {
                    string ma = item.Tag.ToString();
                    if (!ma.IsNullOrEmptyOrSpace())
                    {
                        lstTSDB.Add(ma);
                        loaiTSDB += ma + "#";
                    }
                }
            }
            if (loaiTSDB != "")
                loaiTSDB = loaiTSDB.Substring(0, loaiTSDB.Length - 1);

            List<string> lstNguonVonChoVay = new List<string>();
            string nguonVon = "";
            foreach (RadTreeViewItem item in tvwTreeNguonVonChoVay.CheckedItems)
            {
                if (item.Tag != null)
                {
                    string ma = item.Tag.ToString();
                    if (!ma.IsNullOrEmptyOrSpace())
                    {
                        lstNguonVonChoVay.Add(ma);
                        nguonVon += ma + "#";
                    }
                }
            }
            if (nguonVon != "")
                nguonVon = nguonVon.Substring(0, nguonVon.Length - 1);

            List<string> lstMucDichVay = new List<string>();
            string mucDichVay = "";
            foreach (RadTreeViewItem item in tvwTreeMucDichVay.CheckedItems)
            {
                if (item.Tag != null)
                {
                    string ma = item.Tag.ToString();
                    if (!ma.IsNullOrEmptyOrSpace())
                    {
                        lstMucDichVay.Add(ma);
                        mucDichVay += ma + "#";
                    }
                }
            }
            if (mucDichVay != "")
                mucDichVay = mucDichVay.Substring(0, mucDichVay.Length - 1);

            string maDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();
            string tieuChiNhom = "";
            string sapXep = lstSourceSapXep.ElementAt(cmbSapXepTheo.SelectedIndex).KeywordStrings.First();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            string tenChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).DisplayName;
            List<string> maCum = new List<string>();
            foreach (AutoCompleteCheckBox cum in lstSourceCum.Where(e => e.CheckedMember == true))
            {
                maCum.Add(cum.ValueMember[0]);
            }
            string tenCum = "";

            // Gán dữ liệu từ form vào các biến truyền cho báo cáo
            MaCum = maCum;
            MaChiNhanh = maChiNhanh;
            TenChiNhanh = tenChiNhanh;
            MaPhongGiaoDich = maDonVi;
            TenPhongGiaoDich = tenPhongGiaoDich;
            TenCum = tenCum;
            NgayBaoCao = ngayBaoCao.ToString("yyyyMMdd");
            MaNgonNgu = maNgonNgu;
            MaDinhDang = maDinhDang;
            MaNguoiLap = Presentation.Process.Common.ClientInformation.TenDangNhap;
            TenNguoiLap = Presentation.Process.Common.ClientInformation.HoTen;
            MaLoaiTien = maLoaiTien;
            TieuChiNhom = tieuChiNhom;
            SapXep = sapXep;
            TuNgay = tuNgay.ToString("yyyyMMdd");
            NgayChotSoLieu = ngayChotSoLieu.ToString("yyyyMMdd");
            NguonVonChoVay = nguonVon;
            MucDichVay = mucDichVay;
            NhomNo = nhomNo;
            LoaiTSDB = loaiTSDB;
        }

        private bool Validation()
        {
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
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenNguoiLap", TenNguoiLap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayThangNam", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenChiNhanh", TenChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenPGD", TenPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenCum", TenCum, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TuNgay", TuNgay, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_DenNgay", NgayChotSoLieu, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_MaLoaiTien", MaLoaiTien, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGiaoDich", MaPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            foreach (string cum in MaCum)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaCum", cum, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            listThamSoBaoCao.Add(new ThamSoBaoCao("@SapXep", SapXep, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TuNgay", TuNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DenNgay", NgayChotSoLieu, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NhomNo", NhomNo, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@LoaiTSDB", LoaiTSDB, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NguonVonChoVay", NguonVonChoVay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MucDichVay", MucDichVay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TieuChiNhom", TieuChiNhom, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaLoaiTien", MaLoaiTien, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBC", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@LoaiBC", "NH", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            return listThamSoBaoCao;
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPhongGD.SelectedIndex >= 0)
                LoadComboboxCum();
        }
    }
}
