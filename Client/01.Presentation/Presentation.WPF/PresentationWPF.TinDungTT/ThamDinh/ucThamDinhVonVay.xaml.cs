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
using System.Data;
using Presentation.Process.Common;
using Utilities.Common;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process;
using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process.BaoCaoServiceRef;
using System.IO;

namespace PresentationWPF.TinDungTT.ThamDinh
{
    /// <summary>
    /// Interaction logic for ucThamDinhVonVay.xaml
    /// </summary>
    public partial class ucThamDinhVonVay : UserControl
    {

        #region Khai bao

        public static RoutedCommand ViewCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNhom = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNhom_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();


        BaoCaoProcess process = new BaoCaoProcess();
        HT_BAOCAO htBaoCao = null;
        List<HT_BAOCAO_TSO> lstHtBaoCaoTso = null;

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        // Các tham số báo cáo từ form điều kiện
        public string MaBaoCao;
        public string MaDonVi;
        public string MaChiNhanh;
        public string TenChiNhanh;
        public string MaPhongGiaoDich;
        public string TenPhongGiaoDich;
        public string MaCum;
        public string TenCum;
        public string MaNhom;
        public string NgayPhatVon;
        public string NgayBaoCao;

        public string MaNguoiLap;
        public string TenNguoiLap;

        public string MaNgonNgu;
        public string MaDinhDang;

        #endregion

        public ucThamDinhVonVay()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDungTT;component/ThamDinh/ucThamDinhVonVay.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindHotkey();
            LoadDuLieu();
            LoadCombobox();
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

            raddtNgayChot.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            cmbChiNhanh.SelectionChanged += cmbChiNhanh_SelectionChanged;
            cmbPhongGD.SelectionChanged += cmbPhongGD_SelectionChanged;
            cmbCum.SelectionChanged += cmbCum_SelectionChanged;
            cmbNhom.SelectionChanged += cmbNhom_SelectionChanged;
        }


        /// <summary>
        /// Dang ky hot key, shortcut key
        /// </summary>
        #region Dang ky hot key, shortcut key
        private void BindHotkey()
        {
            foreach (var child in Toolbar.Children)
            {
                if (child.GetType() == typeof(RibbonButton))
                {
                    RibbonButton tlb = (RibbonButton)child;
                    KeyBinding key = new KeyBinding();
                    string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.W, ModifierKeys.Control);
                        key = new KeyBinding(ViewCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(HelpCommand, keyg);
                    }

                    if (key != null)
                    {
                        InputBindings.Add(key);
                    }
                }
            }
        }

        private void ViewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ViewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onView();
        }

        private void HelpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onHelp();
        }

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        /// <summary>
        /// Sự kiện hot key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
                onView();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }
        /// <summary>
        /// Xử lý sự kiện keydown trên form
        /// Bao gồm:
        /// Nhấn Escape để thoát form
        /// Nhấn Enter/Tab để focus vào control tiếp theo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            // Kiểm tra escape thoát form
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);

            // Nhấn enter để chuyển focus tới control tiếp theo
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        /// <summary>
        /// Sự kiện shortcutkey
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
                onView();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void onHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }

        #endregion

        private void LoadDuLieu()
        {
            int idBaoCao = DatabaseConstant.DanhSachBaoCaoTheoDinhKy.TDVM_PHIEU_THAM_DINH_VON_VAY.layIdBaoCao();
            string maBaoCao = DatabaseConstant.DanhSachBaoCaoTheoDinhKy.TDVM_PHIEU_THAM_DINH_VON_VAY.layMaBaoCao();
            process.LayThongTinBaoCao(idBaoCao, maBaoCao, ref htBaoCao, ref lstHtBaoCaoTso);
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
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);

            // khởi tạo combobox
            auto = new AutoComboBox();

            auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue());
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceCum, ref cmbCum, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM_ALL.getValue());
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceNhom, ref cmbNhom, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NHOM_ALL.getValue());
            LoadComboboxPhongGD();
            LoadComboboxCum();
            LoadComboboxNhom();

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.LY_DO_VAO_RA.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();         

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());
            cmbNgonNgu.IsEnabled = false;

            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.PDF.layGiaTri());
            cmbDinhDang.IsEnabled = false;
        }

        private void LoadComboboxPhongGD()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();

            lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            if (!maChiNhanh.Equals("%"))
                lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();
            
            // khởi tạo combobox
            auto = new AutoComboBox();
            cmbPhongGD.Items.Clear();
            auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
            cmbPhongGD.SelectedIndex = 0;
        }

        private void LoadComboboxCum()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();

            lstSourceCum_Select = new List<AutoCompleteEntry>();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            string maPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
            if (!maPhongGiaoDich.Equals("%"))
                lstSourceCum_Select = lstSourceCum.Where(e => e.KeywordStrings.ElementAt(1).Substring(0, maPhongGiaoDich.Length).Equals(maPhongGiaoDich)).ToList();
            // khởi tạo combobox
            auto = new AutoComboBox();
            cmbCum.Items.Clear();
            auto.GenAutoComboBox(ref lstSourceCum_Select, ref cmbCum, null);
            cmbCum.SelectedIndex = 0;
        }

        private void LoadComboboxNhom()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();

            lstSourceNhom_Select = new List<AutoCompleteEntry>();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            string maPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
            if (lstSourceCum_Select.Count > 0)
            {
                string maCum = lstSourceCum_Select.ElementAt(cmbCum.SelectedIndex).KeywordStrings.First();
                if (!maCum.Equals("%"))
                    lstSourceNhom_Select = lstSourceNhom.Where(e => e.KeywordStrings.ElementAt(0).Substring(0, maCum.Length).Equals(maCum)).ToList();
            }
            // khởi tạo combobox
            auto = new AutoComboBox();
            try
            {
                cmbNhom.Items.Clear();
            }
            catch (Exception ex)
            {}
            auto.GenAutoComboBox(ref lstSourceNhom_Select, ref cmbNhom, null);
            cmbNhom.SelectedIndex = 0;

        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPhongGD.SelectedIndex >= 0)
                LoadComboboxCum();
        }

        private void GetFormData()
        {
            string maChiNhanh = string.Empty;
            string tenChiNhanh = string.Empty;
            maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            tenChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).DisplayName;

            string maPhongGiaoDich = string.Empty;
            string tenPhongGiaoDich = string.Empty;
            if (lstSourcePhongGD_Select.Count > 0)
            {
                maPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
                tenPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).DisplayName;
            }

            MaDonVi = (MaPhongGiaoDich.IsNullOrEmpty() || maPhongGiaoDich.Equals("%")) ? maChiNhanh : maPhongGiaoDich;

            DateTime ngayPhatVon = new DateTime();
            if (raddtNgayChot.Value is DateTime)
                ngayPhatVon = (DateTime)raddtNgayChot.Value;
            DateTime ngayBaoCao = new DateTime();
            if (raddtNgayBaoCao.Value is DateTime)
                ngayBaoCao = (DateTime)raddtNgayBaoCao.Value;

            string tenCum = string.Empty;
            if (lstSourceCum_Select.Count > 0)
                tenCum = lstSourceCum_Select.ElementAt(cmbCum.SelectedIndex).KeywordStrings[1];
            else
                tenCum = string.Empty;
            string maCum = string.Empty;
            if (lstSourceCum_Select.Count > 0)
                maCum = lstSourceCum_Select.ElementAt(cmbCum.SelectedIndex).KeywordStrings.First();
            else
                maCum = string.Empty;
            string maNhom = string.Empty;
            if (lstSourceNhom_Select.Count > 0)
                maNhom = lstSourceNhom_Select.ElementAt(cmbNhom.SelectedIndex).KeywordStrings.First();
            else
                maNhom = string.Empty;

            string maNgonNgu = lstSourceNgonNgu.ElementAt(cmbNgonNgu.SelectedIndex).KeywordStrings.First();
            string maDinhDang = lstSourceDinhDang.ElementAt(cmbDinhDang.SelectedIndex).KeywordStrings.First();
            
            // Gán dữ liệu từ form vào các biến truyền cho báo cáo
            MaChiNhanh = maChiNhanh;
            TenChiNhanh = tenChiNhanh;
            MaPhongGiaoDich = maPhongGiaoDich;
            TenPhongGiaoDich = tenPhongGiaoDich;
            MaCum = maCum;
            TenCum = tenCum;
            MaNhom = maNhom;
            NgayPhatVon = ngayPhatVon.ToString("yyyyMMdd");
            NgayBaoCao = ngayBaoCao.ToString("yyyyMMdd");
            MaNguoiLap = ClientInformation.TenDangNhap;
            TenNguoiLap = ClientInformation.HoTen;
            MaNgonNgu = maNgonNgu;
            MaDinhDang = maDinhDang;
        }

        private bool Validation(ref string msg)
        {
            if (grDanhSachKH.SelectedItems.Count == 0)
            {
                msg = "Chưa chọn khách hàng cho báo cáo.";
                return false;
            }
            return true;
        }

        public DatabaseConstant.Action GetAction() { return DatabaseConstant.Action.IN_CHUNG; }

        public List<ThamSoBaoCao> GetParameters()
        {
            string msg = string.Empty;
            if (!Validation(ref msg))
            {
                if (msg.Equals(""))
                    LMessage.ShowMessage("Thiếu tham số cho báo cáo.", LMessage.MessageBoxType.Information);
                else
                    LMessage.ShowMessage(msg, LMessage.MessageBoxType.Information);
                return null;
            }

            GetFormData();

            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", MaChiNhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGD", MaPhongGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaCum", MaCum, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaNhom", MaNhom, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            if (grDanhSachKH.SelectedItems.Count > 0)
            {
                for (int i = 0; i < grDanhSachKH.SelectedItems.Count; i++)
                {
                    DataRow dr = (DataRow)grDanhSachKH.SelectedItems[i];
                    listThamSoBaoCao.Add(new ThamSoBaoCao("@DSKhachHang", dr["MA_KHANG"].ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                }
            }
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayChotDL", NgayPhatVon, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", NgayBaoCao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", MaNgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", MaDinhDang, ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));


            return listThamSoBaoCao;
        }

        private void cmbCum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCum.SelectedIndex >= 0)
                LoadComboboxNhom();
        }

        private void cmbNhom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoCompleteEntry auNhom = lstSourceNhom_Select.ElementAt(cmbNhom.SelectedIndex);
            DataSet ds = new BaoCaoProcess().GetKhachHangTheoNhom(auNhom.KeywordStrings[0].ToString());
            if (ds != null && ds.Tables.Count > 0)
                grDanhSachKH.ItemsSource = ds.Tables[0];
            else
                grDanhSachKH.ItemsSource = null;
        }

        private void onView()
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                // Lấy dữ liệu từ form điều kiện                
                List<HT_BAOCAO_TSO> listHtBaoCaoTso = lstHtBaoCaoTso;
                List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
                DatabaseConstant.Action action = DatabaseConstant.Action.IN;
                listThamSoBaoCao = GetParameters();
                DataSet ds = new DataSet();
                // Chuẩn bị điều kiện cho báo cáo
                if (listThamSoBaoCao != null && listThamSoBaoCao.Count > 0)
                {
                    if (listHtBaoCaoTso.Where(t => t.LOAI_TSO == ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri() && t.MA_TSO.Equals("@DT_THAMSO")).Count() > 0)
                    {
                        listHtBaoCaoTso = new List<HT_BAOCAO_TSO>();
                        foreach (ThamSoBaoCao thamSoBaoCao in listThamSoBaoCao)
                        {
                            HT_BAOCAO_TSO tso = new HT_BAOCAO_TSO();
                            tso.MA_TSO = thamSoBaoCao.MaThamSo;
                            tso.LOAI_TSO = thamSoBaoCao.LoaiThamSo;
                            tso.GTRI_TSO = thamSoBaoCao.GiaTriThamSo;
                            listHtBaoCaoTso.Add(tso);
                        }
                    }
                    else
                    {
                        foreach (HT_BAOCAO_TSO htBaoCaoTso in listHtBaoCaoTso)
                        {
                            foreach (ThamSoBaoCao thamSoBaoCao in listThamSoBaoCao)
                            {
                                if (htBaoCaoTso.MA_TSO.Equals(thamSoBaoCao.MaThamSo) &&
                                    htBaoCaoTso.LOAI_TSO.Equals(thamSoBaoCao.LoaiThamSo))
                                {
                                    htBaoCaoTso.GTRI_TSO = thamSoBaoCao.GiaTriThamSo;
                                    break;
                                }
                                if (!LObject.IsNullOrEmpty(thamSoBaoCao.DsThamSo))
                                    ds = thamSoBaoCao.DsThamSo;
                            }
                        }
                    }
                }

                ApplicationConstant.ResponseStatus retStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                FileBase fileResponse = new FileBase();
                List<FileBase> lstFileResponse = new List<FileBase>();
                string responseMessage = null;

                if (htBaoCao.MA_BAOCAO.Equals(DatabaseConstant.DanhSachBaoCaoTheoDinhKy.TDVM_PHIEU_THEO_DOI_HOAN_TRA_TVIEN.layMaBaoCao()))
                    retStatus = process.LayDuLieu(htBaoCao, listHtBaoCaoTso, ref lstFileResponse, ref responseMessage, ds, action);
                else
                    retStatus = process.LayDuLieu(htBaoCao, listHtBaoCaoTso, ref fileResponse, ref responseMessage, ds, action);

                if (retStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    string fileReport = "";
                    string folderReport = ClientInformation.TempDir;
                    if (!lstFileResponse.IsNullOrEmpty() && lstFileResponse.Count > 0)
                    {
                        folderReport = ClientInformation.TempDir + "\\Hoan_Tra_TV";
                        if (Directory.Exists(folderReport))
                        {
                            LFile.DeleteFolder(folderReport);
                            Directory.CreateDirectory(folderReport);
                        }
                        else
                        {
                            Directory.CreateDirectory(folderReport);
                        }
                        foreach (FileBase item in lstFileResponse)
                            ShowFile(ref folderReport, ref fileReport, false, item, listThamSoBaoCao);
                    }
                    else
                        ShowFile(ref folderReport, ref fileReport, true, fileResponse, listThamSoBaoCao);
                    if (htBaoCao.MA_BAOCAO.Equals(DatabaseConstant.DanhSachBaoCaoTheoDinhKy.GDKT_KET_XUAT_BCDKT_THEO_CAU_TRUC.layMaBaoCao()))
                    {
                        folderReport = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    }
                }
                else
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                    LMessage.ShowMessage(responseMessage, LMessage.MessageBoxType.Error);
                    return;
                }

                /*
                FileBase fileResponse = process.LayDuLieuBaoCao();
                string fileReport = ClientInformation.TempDir + "\\" + fileResponse.FileName + ".pdf";
                LFile.WriteFileFromByteArray(fileResponse.FileData, fileReport);

                // show file
                Stream stream = LFile.ConvertByteArrayToStream(fileResponse.FileData);

                System.Diagnostics.Process.Start(fileReport);
                 */
            }
            catch (System.Exception ex)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }

        }

        private void ShowFile(ref string folderReport, ref string fileReport, bool isOpenFile, FileBase fileResponse, List<ThamSoBaoCao> listThamSoBaoCao)
        {
            try
            {
                if (fileResponse.FileFormat.Equals(ApplicationConstant.LoaiDinhDangBaoCao.TEXT.layGiaTri()))
                    fileReport = folderReport + "\\" + fileResponse.FileName;
                else
                    fileReport = folderReport + "\\" + fileResponse.FileName + "." + fileResponse.FileFormat;

                LFile.WriteFileFromByteArray(fileResponse.FileData, fileReport);
                if (isOpenFile)
                {
                    // show file 
                    if (fileResponse.FileFormat == "rar")
                    {
                        LZip.UnZipFiles(fileReport, folderReport, "ng-mFina", false);
                        string format = "";
                        string loaiThamSo = ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri();
                        format = listThamSoBaoCao.Where(item => item.LoaiThamSo.Equals(loaiThamSo)).FirstOrDefault().GiaTriThamSo;
                        string originalFormat = "";
                        if (format.Equals(ApplicationConstant.LoaiDinhDangBaoCao.PDF.layGiaTri()))
                        {
                            originalFormat = "pdf";
                        }
                        else if (format.Equals(ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri()))
                        {
                            originalFormat = "xls";
                        }
                        else if (format.Equals(ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri()))
                        {
                            originalFormat = "doc";
                        }
                        else if (format.Equals(ApplicationConstant.LoaiDinhDangBaoCao.TEXT.layGiaTri()))
                        {
                            originalFormat = "txt";
                        }
                        else
                        {
                            originalFormat = "pdf";
                        }
                        string originalFileReport = ClientInformation.TempDir + "\\" + fileResponse.FileName + "." + originalFormat;
                        //Stream stream = LFile.ConvertByteArrayToStream(fileResponse.FileData);
                        Mouse.OverrideCursor = Cursors.Arrow;
                        System.Diagnostics.Process.Start(originalFileReport);
                    }
                    else
                    {
                        //Stream stream = LFile.ConvertByteArrayToStream(fileResponse.FileData);
                        Mouse.OverrideCursor = Cursors.Arrow;

                        System.Diagnostics.Process.Start(fileReport);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
    }
}
