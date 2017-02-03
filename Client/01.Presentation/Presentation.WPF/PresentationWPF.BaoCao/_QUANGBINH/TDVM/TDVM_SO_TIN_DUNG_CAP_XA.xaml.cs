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
using System.Windows.Threading;
using PresentationWPF.BaoCao.DungChung;
using Telerik.Windows.Controls;

namespace PresentationWPF.BaoCao._QUANGBINH.TDVM
{
    /// <summary>
    /// Interaction logic for TDVM_SO_TIN_DUNG_CAP_XA.xaml
    /// </summary>
    public partial class TDVM_SO_TIN_DUNG_CAP_XA: UserControl
    {
         #region Khai bao

        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceXa = new List<AutoCompleteEntry>();
        ListCheckBoxCombo lstSourceAp = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourceNguonVon = new ListCheckBoxCombo();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNhomAll = new List<AutoCompleteEntry>();
        List<Tuple<int, string, string>> lstSourceNhom_Select = new List<Tuple<int, string, string>>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();

        List<AutoCompleteEntry> lstSourceVonTraDan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceVonKhac = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceVonBoSung = new List<AutoCompleteEntry>();

        // Các tham số báo cáo từ form điều kiện
        public string MaBaoCao;
        public string MaChiNhanh;
        public string TenChiNhanh;
        public string MaPhongGiaoDich;
        public string TenPhongGiaoDich;
        public string NgayBaoCao;
        public string MaNguoiLap;
        public string TenNguoiLap;
        public string IDXa;
        public List<string> IDAp;
        public List<string> IDNhom;
        public List<string> IDNguonVon;
        public List<string> MaSanPham;

        public string ThangDuLieu;

        public string MaNgonNgu;
        public string MaDinhDang;

        #endregion

        public TDVM_SO_TIN_DUNG_CAP_XA()
        {
            InitializeComponent();
            LoadCombobox();      
            cmbChiNhanh.SelectionChanged += new SelectionChangedEventHandler(cmbChiNhanh_SelectionChanged);
            cmbPhongGD.SelectionChanged += new SelectionChangedEventHandler(cmbPhongGD_SelectionChanged);
            cmbXaPhuong.SelectionChanged += new SelectionChangedEventHandler(cmbXaPhuong_SelectionChanged);
            
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
            lstDieuKien.Add(ClientInformation.TenDangNhap);
            lstDieuKien.Add(ClientInformation.MaDonViQuanLy);
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), lstDieuKien, ClientInformation.MaDonVi);

            //// khởi tạo combobox
            //Tao combobox phonggd
            Dispatcher.CurrentDispatcher.DelayInvoke("cmbPhongGD", () =>
            {
                auto = new AutoComboBox();
                auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue());
                LoadComboBoxPhongGD();
            }, TimeSpan.FromSeconds(0));


            //LoadComboboxNguonVon
            LoadComboboxNguonVon();
            
            // khởi tạo dữ liệu nhóm
            auto = new AutoComboBox();
            Telerik.Windows.Controls.RadComboBox cmdNhomAll = new Telerik.Windows.Controls.RadComboBox();
            auto.GenAutoComboBox(ref lstSourceNhomAll, ref cmdNhomAll, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NHOM_ALL.getValue());
            

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

        private void LoadComboBoxPhongGD()
        {
            if (cmbChiNhanh.SelectedIndex >= 0)
            {
                AutoComboBox auto = new AutoComboBox();

                lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
                string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();

                // khởi tạo combobox
                auto = new AutoComboBox();
                cmbPhongGD.Items.Clear();
                auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
                cmbPhongGD.SelectedIndex = 0;
            }
        }

        private void LoadComboboxNguonVon()
        {
            lstSourceNguonVon = new ListCheckBoxCombo();
            AutoComboBoxListCheckes auto = new AutoComboBoxListCheckes();

            auto.GenAutoComboBox(ref lstSourceNguonVon, ref cmbNguonVon, "COMBOBOX_NGUON_VON_CT", null);
        }

        private void LoadComboBoxXaPhuong()
        {
            lstSourceXa = new List<AutoCompleteEntry>();
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            string IdDonVi = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings[3];
            lstDieuKien.Add(IdDonVi);

            cmbXaPhuong.Items.Clear();
            auto.GenAutoComboBox(ref lstSourceXa, ref cmbXaPhuong, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUC.getValue(), lstDieuKien, null);
            cmbXaPhuong.SelectedIndex = 0;
        }

        private void LoadComboBoxThonAp()
        {
            lstSourceAp = new ListCheckBoxCombo();
            AutoComboBoxListCheckes auto = new AutoComboBoxListCheckes();
            List<string> lstDieuKien = new List<string>();
            string idDonVi = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings[3];
            string idKhuVuc = lstSourceXa.ElementAt(cmbXaPhuong.SelectedIndex).KeywordStrings[1];
            lstDieuKien.Add(idDonVi);
            lstDieuKien.Add(idKhuVuc);

          
            auto.GenAutoComboBox(ref lstSourceAp, ref cmbThonAp, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM.getValue(), lstDieuKien);
            cmbThonAp.SelectedIndex = 0;
        }

        private void LoadTreeview()
        {
            string maPGD = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();

            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            RadComboBox cb = new RadComboBox();
            lstDieuKien.Add(maPGD);
            lstDieuKien.Add("VON_TRA_DAN");
            lstSourceVonTraDan = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstSourceVonTraDan, ref cb, "COMBOBOX_SAN_PHAM_TDLIST_02", lstDieuKien);

            RadTreeViewItem rootItem = new RadTreeViewItem();
            rootItem.Header = LLanguage.SearchResourceByKey("Vốn trả dần ") + " (" + lstSourceVonTraDan.Count.ToString() + ")";
            rootItem.Tag = string.Empty;
            rootItem.IsExpanded = true;
            rootItem.IsChecked = false;
            tvwVonTraDan.Items.Clear();
            tvwVonTraDan.Items.Add(rootItem);
            BuildTree(rootItem, lstSourceVonTraDan);

            auto = new AutoComboBox();
            lstDieuKien = new List<string>();
            cb = new RadComboBox();
            lstDieuKien.Add(maPGD);
            lstDieuKien.Add("TIN_CHAP");
            lstSourceVonKhac = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstSourceVonKhac, ref cb, "COMBOBOX_SAN_PHAM_TDLIST_02", lstDieuKien);

            rootItem = new RadTreeViewItem();
            rootItem.Header = LLanguage.SearchResourceByKey("Vốn khác ") + " (" + lstSourceVonKhac.Count.ToString() + ")";
            rootItem.Tag = string.Empty;
            rootItem.IsExpanded = true;
            rootItem.IsChecked = false;
            tvwVonKhac.Items.Clear();
            tvwVonKhac.Items.Add(rootItem);
            BuildTree(rootItem, lstSourceVonKhac);


            auto = new AutoComboBox();
            lstDieuKien = new List<string>();
            cb = new RadComboBox();
            lstDieuKien.Add(maPGD);
            lstDieuKien.Add("VON_THOI_VU");
            lstSourceVonBoSung = new List<AutoCompleteEntry>();
            auto.GenAutoComboBox(ref lstSourceVonBoSung, ref cb, "COMBOBOX_SAN_PHAM_TDLIST_02", lstDieuKien);

            rootItem = new RadTreeViewItem();
            rootItem.Header = LLanguage.SearchResourceByKey("Vốn bổ sung ") + " (" + lstSourceVonBoSung.Count.ToString() + ")";
            rootItem.Tag = string.Empty;
            rootItem.IsExpanded = true;
            rootItem.IsChecked = false;
            tvwVonBoSung.Items.Clear();
            tvwVonBoSung.Items.Add(rootItem);
            BuildTree(rootItem, lstSourceVonBoSung);

            
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
            if (cmbChiNhanh.SelectedIndex == -1) return;
            LoadComboBoxPhongGD();
        }

        private void GetFormData()
        {
            string maNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            string maDonVi = string.Empty;

            DateTime ngayBaoCao = new DateTime();
            if (raddtNgayBaoCao.Value is DateTime)
                ngayBaoCao = (DateTime)raddtNgayBaoCao.Value;
            DateTime thangDuLieu = new DateTime();
            if (raddtThangDuLieu.Value is DateTime)
                thangDuLieu = (DateTime)raddtThangDuLieu.Value;
            string maDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();

            //Lấy giá trị
            MaChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            TenChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).DisplayName;

            MaPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
            TenPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).DisplayName;


            IDNguonVon = new List<string>();
            foreach (AutoCompleteCheckBox NguonVon in lstSourceNguonVon.Where(e => e.CheckedMember == true))
            {
                IDNguonVon.Add(NguonVon.ValueMember[1]);
            }

            IDXa = lstSourceXa.ElementAt(cmbXaPhuong.SelectedIndex).KeywordStrings[1];

            IDAp = new List<string>();
            foreach (AutoCompleteCheckBox Ap in lstSourceAp.Where(e => e.CheckedMember == true))
            {
                IDAp.Add(Ap.ValueMember[1]);
            }

            List<string> lstSanPham = new List<string>();
            foreach (RadTreeViewItem item in tvwVonTraDan.CheckedItems)
            {
                if (item.Tag != null)
                {
                    string ma = item.Tag.ToString();
                    if (!ma.IsNullOrEmptyOrSpace())
                    {
                        lstSanPham.Add(ma);
                    }
                }
            }
            foreach (RadTreeViewItem item in tvwVonKhac.CheckedItems)
            {
                if (item.Tag != null)
                {
                    string ma = item.Tag.ToString();
                    if (!ma.IsNullOrEmptyOrSpace())
                    {
                        lstSanPham.Add(ma);
                    }
                }
            }
            foreach (RadTreeViewItem item in tvwVonBoSung.CheckedItems)
            {
                if (item.Tag != null)
                {
                    string ma = item.Tag.ToString();
                    if (!ma.IsNullOrEmptyOrSpace())
                    {
                        lstSanPham.Add(ma);
                    }
                }
            }

            ThangDuLieu = thangDuLieu.GetLastDateOfMonth().ToString("yyyyMMdd");
            NgayBaoCao = ngayBaoCao.ToString("yyyyMMdd");
            MaNgonNgu = maNgonNgu;
            MaDinhDang = maDinhDang;
            MaNguoiLap = Presentation.Process.Common.ClientInformation.TenDangNhap;
            TenNguoiLap = Presentation.Process.Common.ClientInformation.HoTen;
            MaSanPham = lstSanPham;
        }

        private bool Validation()
        {
            if (cmbPhongGD.SelectedIndex == -1)
                return false;
            if (cmbXaPhuong.SelectedIndex == -1)
                return false;
            if (cmbThonAp.SelectedIndex == -1)
                return false;
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
            //listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenNguoiLap", TenNguoiLap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenChiNhanh", TenChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            //listThamSoBaoCao.Add(new ThamSoBaoCao("P_TenPGD", TenPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_ThangDuLieu", ThangDuLieu, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            //listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayMoSo", TuNgay, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGD", MaPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            foreach (string NguonVon in IDNguonVon)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@IdNguonVon", NguonVon, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
                
            listThamSoBaoCao.Add(new ThamSoBaoCao("@IdKhuVuc", IDXa, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            foreach (string Ap in IDAp)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@IdCum", Ap, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            
            foreach (string SP in MaSanPham)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaSanPham", SP, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            listThamSoBaoCao.Add(new ThamSoBaoCao("@TenChiNhanh", TenChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TenPhongGiaoDich", TenPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@ThangDuLieu", ThangDuLieu, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("@P_MaDonVi", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.GUIPARAM.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            return listThamSoBaoCao;
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPhongGD.SelectedIndex == -1) return;
            LoadTreeview();
            LoadComboBoxXaPhuong();            
        }

        private void cmbXaPhuong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbXaPhuong.SelectedIndex == -1) return;
            LoadComboBoxThonAp();
        }

       
        private void cmbNguonVon_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceNguonVon = cmbNguonVon.ItemsSource as ListCheckBoxCombo;
        }
    }
}