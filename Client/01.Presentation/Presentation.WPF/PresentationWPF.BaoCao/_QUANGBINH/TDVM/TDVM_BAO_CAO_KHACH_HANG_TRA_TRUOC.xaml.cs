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
using System.Windows.Shapes;
using PresentationWPF.CustomControl;
using System.Data;
using Utilities.Common;
using Presentation.Process.Common;
using Presentation.Process;
using PresentationWPF.BaoCao.DungChung;

namespace PresentationWPF.BaoCao._QUANGBINH.TDVM
{
    /// <summary>
    /// Interaction logic for TDVM_BAO_CAO_KHACH_HANG_TRA_TRUOC.xaml
    /// </summary>
    public partial class TDVM_BAO_CAO_KHACH_HANG_TRA_TRUOC : UserControl
    {
        #region Khai bao

        ListCheckBoxCombo lstSourceChiNhanh = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourcePhongGD = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourceXa = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourceAp = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourceSanPham = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourceLoaiSanPham = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourceNguonVon = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourceNhom = new ListCheckBoxCombo();
        ListCheckBoxCombo lstSourceNhomNo = new ListCheckBoxCombo();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
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
        public List<string> IDXa;
        public List<string> IDAp;
        public List<string> IDNhom;
        public List<string> lstMaSanPham;
        public List<string> lstMaLoaiSP;
        public List<string> lstNguonVon;
        public List<string> lstNhomNo;

        public string NgayChotDL;

        public string MaNgonNgu;
        public string MaDinhDang;

        #endregion

        public TDVM_BAO_CAO_KHACH_HANG_TRA_TRUOC()
        {
            InitializeComponent();
            LoadCombobox();
          
            cmbChiNhanh.DropDownClosed += new EventHandler(cmbChiNhanh_DropDownClosed);
            cmbPhongGD.DropDownClosed += new EventHandler(cmbPhongGD_DropDownClosed);
            cmbXaPhuong.DropDownClosed += new EventHandler(cmbXaPhuong_DropDownClosed);
            cmbThonAp.DropDownClosed += new EventHandler(cmbThonAp_DropDownClosed);
            cmbSanPham.DropDownClosed += new EventHandler(cmbSanPham_DropDownClosed);
            cmbLoaiSP.DropDownClosed += new EventHandler(cmbLoaiSP_DropDownClosed);

            raddtNgayChotSoLieu.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            raddtNgayBaoCao.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
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

            Dispatcher.CurrentDispatcher.DelayInvoke("LoadDuLieuNhom", () =>
            {
                LoadDuLieuNhom();
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("LoadComboboxLoaiSP", () =>
            {
                LoadComboboxLoaiSP();
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("LoadComboboxNguonVon", () =>
            {
                LoadComboboxNguonVon();
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("LoadComboboxNhomNo", () =>
            {
                LoadComboboxNhomNo();
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("LoadComboboxSanPham", () =>
            {
                LoadComboboxSanPham();
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("cmbNgonNgu", () =>
            {
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
                // khởi tạo combobox
                auto = new AutoComboBox();
                //auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());
                auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ClientInformation.NgonNgu);
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("cmbDinhDang", () =>
            {
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
                // khởi tạo combobox
                auto = new AutoComboBox();
                //auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri());
                //cmbDinhDang.SelectedIndex = lstSourceDinhDang.FindIndex(e => e.KeywordStrings[0].Equals(ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri()));
                auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ReportInformation.DinhDang);
                cmbDinhDang.IsEnabled = false;
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
            
        }

        private void LoadComboboxLoaiSP()
        {
            lstSourceLoaiSanPham = new ListCheckBoxCombo();
            AutoComboBoxListCheckes auto = new AutoComboBoxListCheckes();

            auto.GenAutoComboBox(ref lstSourceLoaiSanPham, ref cmbLoaiSP, "COMBOBOX_LOAI_SAN_PHAM", null);
        }

        private void LoadComboboxNguonVon()
        {
            lstSourceNguonVon = new ListCheckBoxCombo();
            AutoComboBoxListCheckes auto = new AutoComboBoxListCheckes();

            auto.GenAutoComboBox(ref lstSourceNguonVon, ref cmbNguonVon, "COMBOBOX_NGUON_VON_CT", null);
        }

        private void LoadComboboxNhomNo()
        {
            lstSourceNhomNo = new ListCheckBoxCombo();
            AutoComboBoxListCheckes auto = new AutoComboBoxListCheckes();

            auto.GenAutoComboBox(ref lstSourceNhomNo, ref cmbNhomNo, "COMBOBOX_NHOM_NO", null);
        }
        
        private void LoadComboboxSanPham()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            lstSourceSanPham = new ListCheckBoxCombo();
            string maChiNhanh = "";
            string maPGD = "";
            string maLoaiSP = "";
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

            lstSourceLoaiSanPham = cmbLoaiSP.ItemsSource as ListCheckBoxCombo;
            foreach (AutoCompleteCheckBox lstLoaiSP in lstSourceLoaiSanPham)
            {
                if (lstLoaiSP.CheckedMember)
                    maLoaiSP += ",'" + lstLoaiSP.ValueMember[0].ToString() + "'";
            }
            if (maLoaiSP.Length > 0)
                maLoaiSP = maLoaiSP.Substring(1);
            else
                maLoaiSP = "0";

            List<string> lstDieuKien = new List<string>();

            // khởi tạo combobox
            auto = new AutoComboBox();
            //lstDieuKien.Add(maChiNhanh);
            lstDieuKien.Add(maPGD);
            lstDieuKien.Add(maLoaiSP);
            lstSourceSanPham = new ListCheckBoxCombo();
            new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceSanPham, ref cmbSanPham, "COMBOBOX_SAN_PHAM_TDLIST_01", lstDieuKien);
        }

        private void LoadDuLieuNhom()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            lstSourceNhom = new ListCheckBoxCombo();
            string maPGD = "";
            string maKhuVuc = "";
            string maCum = "";
            
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
                {
                    if(lstCN.ValueMember[0].ToString().Equals("All"))
                        maKhuVuc += ",'" + lstCN.ValueMember[0].ToString() + "'";
                    else
                        maKhuVuc += ",'" + lstCN.ValueMember[3].ToString() + "'";
                }
            }
            if (maKhuVuc.Length > 0)
                maKhuVuc = maKhuVuc.Substring(1);
            else
                maKhuVuc = "0";

            lstSourceAp = cmbThonAp.ItemsSource as ListCheckBoxCombo;
            foreach (AutoCompleteCheckBox lstCN in lstSourceAp)
            {
                if (lstCN.CheckedMember)
                    maCum += ",'" + lstCN.ValueMember[0].ToString() + "'";
            }
            if (maCum.Length > 0)
                maCum = maCum.Substring(1);
            else
                maCum = "0";

            List<string> lstDieuKien = new List<string>();

            // khởi tạo combobox
            auto = new AutoComboBox();
            lstDieuKien.Add(maPGD);
            lstDieuKien.Add(maKhuVuc);
            lstDieuKien.Add(maCum);
            lstSourceNhom = new ListCheckBoxCombo();
            new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceNhom, ref cmbNhom, "COMBOBOX_NHOM_02", lstDieuKien);
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
            DateTime ngayChotDL = new DateTime();
            if (raddtNgayChotSoLieu.Value is DateTime)
                ngayChotDL = (DateTime)raddtNgayChotSoLieu.Value;

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
            foreach (AutoCompleteCheckBox Nhom in lstSourceNhom.Where(e => e.CheckedMember == true))
            {
                IDNhom.Add(Nhom.ValueMember[1]);
            }


            lstMaLoaiSP = new List<string>();
            foreach (AutoCompleteCheckBox LoaiSP in lstSourceLoaiSanPham.Where(e => e.CheckedMember == true))
            {
                lstMaLoaiSP.Add(LoaiSP.ValueMember[0]);
            }

            lstMaLoaiSP = new List<string>();
            foreach (AutoCompleteCheckBox LoaiSP in lstSourceLoaiSanPham.Where(e => e.CheckedMember == true))
            {
                lstMaLoaiSP.Add(LoaiSP.ValueMember[0]);
            }

            lstMaSanPham = new List<string>();
            foreach (AutoCompleteCheckBox SP in lstSourceSanPham.Where(e => e.CheckedMember == true))
            {
                lstMaSanPham.Add(SP.ValueMember[0]);
            }

            lstNguonVon = new List<string>();
            foreach (AutoCompleteCheckBox NguonVon in lstSourceNguonVon.Where(e => e.CheckedMember == true))
            {
                lstNguonVon.Add(NguonVon.ValueMember[0]);
            }

            lstNhomNo = new List<string>();
            foreach (AutoCompleteCheckBox NhomNo in lstSourceNhomNo.Where(e => e.CheckedMember == true))
            {
                lstNhomNo.Add(NhomNo.ValueMember[0]);
            }

            NgayBaoCao = ngayBaoCao.ToString("yyyyMMdd");
            NgayChotDL = ngayChotDL.ToString("yyyyMMdd");
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
            if (lstSourceAp.Where(e => e.CheckedMember == true).ToList().Count < 1)
                return false;
            if (lstSourceXa.Where(e => e.CheckedMember == true).ToList().Count < 1)
                return false;
            if (lstSourceNhom.Where(e => e.CheckedMember == true).ToList().Count < 1)
                return false;
            if (lstSourceNguonVon.Where(e => e.CheckedMember == true).ToList().Count < 1)
                return false;
            if (lstSourceNhomNo.Where(e => e.CheckedMember == true).ToList().Count < 1)
                return false;
            if (lstSourceSanPham.Where(e => e.CheckedMember == true).ToList().Count < 1)
                return false;
            if (lstSourceLoaiSanPham.Where(e => e.CheckedMember == true).ToList().Count < 1)
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
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayChotDL", NgayChotDL, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
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

            foreach (string Xa in IDXa)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@IDKhuVuc", Xa, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            foreach (string Ap in IDAp)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@IDCum", Ap, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            foreach (string Nhom in IDNhom)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@IDNhom", Nhom, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            foreach (string NguonVon in lstNguonVon)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaNguonVon", NguonVon, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            foreach (string LoaiSP in lstMaLoaiSP)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaLoaiSP", LoaiSP, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            for (int i = 0; i < lstMaSanPham.Count; i++)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaSanPham", lstMaSanPham[i], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            foreach (string NhomNo in lstNhomNo)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaNhomNo", NhomNo, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            listThamSoBaoCao.Add(new ThamSoBaoCao("@TenChiNhanh", TenChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TenPhongGiaoDich", TenPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayChotDL", NgayChotDL, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("@P_MaDonVi", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.GUIPARAM.layGiaTri()));
            IDNhom = new List<string>();
              


            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            return listThamSoBaoCao;
        }

        private void cmbChiNhanh_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceChiNhanh = cmbChiNhanh.ItemsSource as ListCheckBoxCombo;
            LoadComboBoxPhongGD();
            cmbPhongGD_DropDownClosed(sender, e);

        }

        private void cmbXaPhuong_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceXa = cmbXaPhuong.ItemsSource as ListCheckBoxCombo;
            LoadComboBoxThonAp();
            cmbThonAp_DropDownClosed(sender, e);
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
            cmbXaPhuong_DropDownClosed(sender, e);
        }

        private void cmbLoaiSP_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceLoaiSanPham = cmbLoaiSP.ItemsSource as ListCheckBoxCombo;
            LoadComboboxSanPham();
        }

        private void cmbSanPham_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceSanPham = cmbSanPham.ItemsSource as ListCheckBoxCombo;
        }

        private void cmbNhom_DropDownClosed(object sender, EventArgs e)
        {

        }
    }
}
