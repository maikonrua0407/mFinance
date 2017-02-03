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

namespace PresentationWPF.BaoCao._HVNH.TDVM
{
    /// <summary>
    /// Interaction logic for TDVM_THAM_DINH_TIN_DUNG.xaml
    /// </summary>
    public partial class TDVM_THAM_DINH_TIN_DUNG : UserControl
    {
        #region Khai bao

        ListCheckBoxCombo lstSourceChiNhanh = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourcePhongGD = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourceCanBoTD = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourceXa = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourceAp = new ListCheckBoxCombo();
        List<AutoCompleteEntry> lstSourceNhom = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNhomAll = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();

        // Các tham số báo cáo từ form điều kiện
        public string MaBaoCao;
        public List<string> MaChiNhanh;
        public string TenChiNhanh;
        public List<string> MaPhongGiaoDich;
        public string TenPhongGiaoDich;
        public string NgayBaoCao;
        public string MaNguoiLap;
        public string TenNguoiLap;
        public string MaLoaiTien;
        public List<string> IDCanBo;
        public List<string> IDXa;
        public List<string> IDAp;
        public List<string> IDNhom;
        public List<string> IDKhang;
        public DataTable dt = new DataTable();

        public string TuNgay;
        public string DenNgay;

        public string MaNgonNgu;
        public string MaDinhDang;

        #endregion

        public TDVM_THAM_DINH_TIN_DUNG()
        {
            InitializeComponent();
            LoadCombobox();
            // Nếu người dùng là đơn vị >> disable thông tin chi nhánh
            //if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()) ||
            //    ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()))
            //{
            //    cmbChiNhanh.IsEnabled = false;
            //}
            //else
            //{
            //    cmbChiNhanh.IsEnabled = true;
            //}
            raddtNgayBaoCao.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("MA_KHANG", typeof(string));
            dt.Columns.Add("TEN_KHANG", typeof(string));
            dt.Columns.Add("SO_TIEN", typeof(decimal));

            cmbChiNhanh.DropDownClosed += new EventHandler(cmbChiNhanh_DropDownClosed);
            cmbPhongGD.DropDownClosed += new EventHandler(cmbPhongGD_DropDownClosed);
            cmbXaPhuong.DropDownClosed += new EventHandler(cmbXaPhuong_DropDownClosed);
            cmbThonAp.DropDownClosed += new EventHandler(cmbThonAp_DropDownClosed);
            cmbNhom.SelectionChanged += new SelectionChangedEventHandler(cmbNhom_SelectionChanged);

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
            Dispatcher.CurrentDispatcher.DelayInvoke("cmbChiNhanh", () =>
            {
                auto = new AutoComboBox();
                lstDieuKien.Add(ClientInformation.TenDangNhap);
                lstDieuKien.Add(ClientInformation.MaDonViQuanLy);
                new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANHLIST.getValue(), lstDieuKien);
            }, TimeSpan.FromSeconds(0));

            
            //// khởi tạo combobox
            //auto = new AutoComboBox();
            //auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue());
            //LoadComboboxPhongGD();
            Dispatcher.CurrentDispatcher.DelayInvoke("LoadComboBoxPhongGD", () =>
            {
                LoadComboBoxPhongGD();
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("LoadComboBoxXaPhuong", () =>
            {
                LoadComboBoxXaPhuong();
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("LoadComboBoxThonAp", () =>
            {
                LoadComboBoxThonAp();
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("cmbLoaiTien", () =>
            {
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.DONG_TIEN_CAC_NUOC.getValue());
                // khởi tạo combobox
                auto = new AutoComboBox();
                auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
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


            Dispatcher.CurrentDispatcher.DelayInvoke("cmdNhomAll", () =>
            {
                // khởi tạo dữ liệu nhóm
                auto = new AutoComboBox();
                Telerik.Windows.Controls.RadComboBox cmdNhomAll = new Telerik.Windows.Controls.RadComboBox();
                auto.GenAutoComboBox(ref lstSourceNhomAll, ref cmdNhomAll, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NHOM_ALL.getValue());
                LoadDuLieuNhom();
            }, TimeSpan.FromSeconds(0));

        }

        private void LoadComboBoxPhongGD()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            lstSourcePhongGD = new ListCheckBoxCombo();
            lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
            string maChiNhanh = "";
            lstSourceChiNhanh = cmbChiNhanh.ItemsSource as ListCheckBoxCombo;
            foreach (AutoCompleteCheckBox lstCN in lstSourceChiNhanh)
            {
                if (lstCN.CheckedMember)
                    maChiNhanh += "," + lstCN.ValueMember[1].ToString();
            }
            if (maChiNhanh.Length > 0)
                maChiNhanh = maChiNhanh.Substring(1, maChiNhanh.Length - 1);
            else
                maChiNhanh = "0";
            List<string> lstDieuKien = new List<string>();

            // khởi tạo combobox
            auto = new AutoComboBox();
            lstDieuKien.Add(maChiNhanh);
            lstDieuKien.Add(ClientInformation.TenDangNhap);
            lstDieuKien.Add(ClientInformation.MaDonViQuanLy);
            new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PGDLIST.getValue(), lstDieuKien);
            //lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();
            //lstSourcePhongGD_Select.Insert(0, new AutoCompleteEntry("Tất cả", "%", ""));
            //// khởi tạo combobox
            //auto = new AutoComboBox();
            //cmbPhongGD.Items.Clear();
            //auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
        }

        private void LoadComboBoxXaPhuong()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            lstSourceXa = new ListCheckBoxCombo();
            string maChiNhanh = "";
            string maPGD = "";
            lstSourceChiNhanh = cmbChiNhanh.ItemsSource as ListCheckBoxCombo;
            foreach (AutoCompleteCheckBox lstCN in lstSourceChiNhanh)
            {
                if (lstCN.CheckedMember)
                    maChiNhanh += ",'" + lstCN.ValueMember[0].ToString() + "'";
            }
            if (maChiNhanh.Length > 0)
                maChiNhanh = maChiNhanh.Substring(1);
            else
                maChiNhanh = "0";

            lstSourcePhongGD = cmbPhongGD.ItemsSource as ListCheckBoxCombo;
            foreach (AutoCompleteCheckBox lstCN in lstSourcePhongGD)
            {
                if (lstCN.CheckedMember)
                    maPGD += ",'" + lstCN.ValueMember[0].ToString() + "'";
            }
            if (maChiNhanh.Length > 0)
                maPGD = maPGD.Substring(1);
            else
                maPGD = "0";

            List<string> lstDieuKien = new List<string>();

            // khởi tạo combobox
            auto = new AutoComboBox();
            lstDieuKien.Add(maChiNhanh);
            lstDieuKien.Add(maPGD);
            lstSourceXa = new ListCheckBoxCombo();
            new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceXa, ref cmbXaPhuong, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUCLIST.getValue(), lstDieuKien);
            //lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();
            //lstSourcePhongGD_Select.Insert(0, new AutoCompleteEntry("Tất cả", "%", ""));
            //// khởi tạo combobox
            //auto = new AutoComboBox();
            //cmbPhongGD.Items.Clear();
            //auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
        }

        private void LoadComboBoxThonAp()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            lstSourceXa = new ListCheckBoxCombo();
            string maChiNhanh = "";
            string maPGD = "";
            string maKhuVuc = "";
            lstSourceChiNhanh = cmbChiNhanh.ItemsSource as ListCheckBoxCombo;
            foreach (AutoCompleteCheckBox lstCN in lstSourceChiNhanh)
            {
                if (lstCN.CheckedMember)
                    maChiNhanh += ",'" + lstCN.ValueMember[0].ToString() + "'";
            }
            if (maChiNhanh.Length > 0)
                maChiNhanh = maChiNhanh.Substring(1);
            else
                maChiNhanh = "0";

            lstSourcePhongGD = cmbPhongGD.ItemsSource as ListCheckBoxCombo;
            foreach (AutoCompleteCheckBox lstCN in lstSourcePhongGD)
            {
                if (lstCN.CheckedMember)
                    maPGD += ",'" + lstCN.ValueMember[0].ToString() + "'";
            }
            if (maPGD.Length > 0)
                maPGD = maPGD.Substring(1);
            else
                maPGD = "0";

            lstSourceXa = cmbXaPhuong.ItemsSource as ListCheckBoxCombo;
            foreach (AutoCompleteCheckBox lstCN in lstSourceXa)
            {
                if (lstCN.CheckedMember)
                    maKhuVuc += "," + lstCN.ValueMember[1].ToString();
            }
            if (maKhuVuc.Length > 0)
                maKhuVuc = maKhuVuc.Substring(1);
            else
                maKhuVuc = "0";

            List<string> lstDieuKien = new List<string>();

            // khởi tạo combobox
            auto = new AutoComboBox();
            lstDieuKien.Add(maChiNhanh);
            lstDieuKien.Add(maPGD);
            lstDieuKien.Add(maKhuVuc);
            lstSourceAp = new ListCheckBoxCombo();
            new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceAp, ref cmbThonAp, "COMBOBOX_CUM_KVUC_LIST", lstDieuKien);
            //lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();
            //lstSourcePhongGD_Select.Insert(0, new AutoCompleteEntry("Tất cả", "%", ""));
            //// khởi tạo combobox
            //auto = new AutoComboBox();
            //cmbPhongGD.Items.Clear();
            //auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
        }

        private void LoadDuLieuNhom()
        {
            List<string> lstCum = lstSourceAp.Where(f => f.CheckedMember.Equals(true)).Select(f => f.ValueMember[1]).ToList();
            lstSourceNhom = new List<AutoCompleteEntry>();
            foreach (AutoCompleteEntry auNhom in lstSourceNhomAll.Where(f => lstCum.Contains(f.KeywordStrings[4])).ToList())
            {
                lstSourceNhom.Add(new AutoCompleteEntry(auNhom.DisplayName, auNhom.KeywordStrings));
            }
            new AutoComboBox().GenAutoComboBox(ref lstSourceNhom, ref cmbNhom, null);
        }

        private void LoadDuLieuKHang()
        {
            AutoCompleteEntry au = lstSourceNhom.ElementAt(cmbNhom.SelectedIndex);
            DataSet ds = new BaoCaoProcess().GetKhachHangTheoNhom(au.KeywordStrings[0]);
            if (!LObject.IsNullOrEmpty(ds) && ds.Tables.Count > 0)
            {
                foreach (DataRow drRoot in ds.Tables[0].Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = drRoot["ID"];
                    dr["MA_KHANG"] = drRoot["MA_KHANG"];
                    dr["TEN_KHANG"] = drRoot["TEN_KHANG"];
                    dr["SO_TIEN"] = 5000;
                    dt.Rows.Add(dr);
                }
                grSoTienGuiDS.ItemsSource = dt.DefaultView;
            }
            else
                grSoTienGuiDS.ItemsSource = null;
        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboBoxPhongGD();
        }

        private void GetFormData()
        {
            string maNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            string maDonVi = string.Empty;

            DateTime ngayBaoCao = new DateTime();
            if (raddtNgayBaoCao.Value is DateTime)
                ngayBaoCao = (DateTime)raddtNgayBaoCao.Value;
            string maLoaiTien = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.First();
            string maDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();

            //Lấy giá trị
            MaChiNhanh = new List<string>();
            foreach (AutoCompleteCheckBox ChiNhanh in lstSourceChiNhanh.Where(e => e.CheckedMember == true))
            {
                MaChiNhanh.Add(ChiNhanh.ValueMember.FirstOrDefault());
                TenChiNhanh = ChiNhanh.DislayMember + " - ";
            }
            MaPhongGiaoDich = new List<string>();
            foreach (AutoCompleteCheckBox PhongGD in lstSourcePhongGD.Where(e => e.CheckedMember == true))
            {
                MaPhongGiaoDich.Add(PhongGD.ValueMember.FirstOrDefault());
                TenPhongGiaoDich = PhongGD.DislayMember + " - ";
            }

            if (MaChiNhanh.Contains("All"))
                TenChiNhanh = "Toàn hệ thống";
            if (MaPhongGiaoDich.Contains("All"))
                TenPhongGiaoDich = "";

            IDCanBo = new List<string>();
            foreach (AutoCompleteCheckBox CanBoTD in lstSourceCanBoTD.Where(e => e.CheckedMember == true))
            {
                IDCanBo.Add(CanBoTD.ValueMember[1]);
            }

            IDXa = new List<string>();
            foreach (AutoCompleteCheckBox Xa in lstSourceXa.Where(e => e.CheckedMember == true))
            {
                IDXa.Add(Xa.ValueMember[1]);
            }

            IDAp = new List<string>();
            foreach (AutoCompleteCheckBox Ap in lstSourceAp.Where(e => e.CheckedMember == true))
            {
                IDAp.Add(Ap.ValueMember[1]);
            }

            IDNhom = new List<string>();
            AutoCompleteEntry auNhom = lstSourceNhom.ElementAt(cmbNhom.SelectedIndex);
            IDNhom.Add(auNhom.KeywordStrings[3]);

            IDKhang = new List<string>();
            foreach (DataRowView drv in grSoTienGuiDS.SelectedItems)
            {
                IDKhang.Add(drv["ID"].ToString());
            }
            NgayBaoCao = ngayBaoCao.ToString("yyyyMMdd");
            MaLoaiTien = maLoaiTien;
            MaNgonNgu = maNgonNgu;
            MaDinhDang = maDinhDang;
            MaNguoiLap = Presentation.Process.Common.ClientInformation.TenDangNhap;
            TenNguoiLap = Presentation.Process.Common.ClientInformation.HoTen;
        }

        private bool Validation()
        {
            if (lstSourceChiNhanh.Where(e => e.CheckedMember == true).ToList().Count < 1)
                return false;
            if (lstSourcePhongGD.Where(e => e.CheckedMember == true).ToList().Count < 1)
                return false;
            if (LObject.IsNullOrEmpty(grSoTienGuiDS.ItemsSource))
                return false;
            if (grSoTienGuiDS.SelectedItems.Count < 1)
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
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_TuNgay", TuNgay, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_DenNgay", DenNgay, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            //listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayMoSo", TuNgay, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));

            foreach (string ChiNhanh in MaChiNhanh)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", ChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }
            foreach (string PhongGD in MaPhongGiaoDich)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGiaoDich", PhongGD, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            foreach (string CanBo in IDCanBo)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaCanBo", CanBo, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            foreach (string Xa in IDXa)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaKhuVuc", Xa, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            foreach (string Ap in IDAp)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaCum", Ap, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            foreach (string Nhom in IDNhom)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaNhom", Nhom, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            //foreach (string KHang in IDKhang)
            //{
            //    listThamSoBaoCao.Add(new ThamSoBaoCao("@MaKHang", KHang, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            //}
            foreach (DataRowView drv in grSoTienGuiDS.SelectedItems)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaKhachHang", drv["MA_KHANG"].ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@TenKhachHang", drv["TEN_KHANG"].ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            listThamSoBaoCao.Add(new ThamSoBaoCao("@TenChiNhanh", TenChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TenPhongGiaoDich", TenPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayPhatVon", TuNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DenNgay", DenNgay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            //listThamSoBaoCao.Add(new ThamSoBaoCao("@MaLoaiTien", MaLoaiTien, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@LoaiTK", "TKTH", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("@P_MaDonVi", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.GUIPARAM.layGiaTri()));
            IDNhom = new List<string>();
            AutoCompleteEntry auNhom = lstSourceNhom.ElementAt(cmbNhom.SelectedIndex);
            IDNhom.Add(auNhom.KeywordStrings[3]);


            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            return listThamSoBaoCao;
        }

        private void cmbChiNhanh_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceChiNhanh = cmbChiNhanh.ItemsSource as ListCheckBoxCombo;
            LoadComboBoxPhongGD();
        }

        private void cmbXaPhuong_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceXa = cmbXaPhuong.ItemsSource as ListCheckBoxCombo;
            LoadComboBoxThonAp();
        }

        private void cmbThonAp_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceAp = cmbThonAp.ItemsSource as ListCheckBoxCombo;
            LoadDuLieuNhom();
        }

        private void cmbPhongGD_DropDownClosed(object sender, EventArgs e)
        {
            lstSourcePhongGD = cmbPhongGD.ItemsSource as ListCheckBoxCombo;
            LoadComboBoxXaPhuong();
            LoadComboBoxThonAp();
        }

        private void cmbNhom_DropDownClosed(object sender, EventArgs e)
        {

        }

        private void cmbNhom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadDuLieuKHang();
        }
    }
}
