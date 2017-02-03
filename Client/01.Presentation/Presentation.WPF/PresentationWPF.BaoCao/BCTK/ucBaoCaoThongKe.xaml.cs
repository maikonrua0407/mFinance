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
using Presentation.Process.BaoCaoServiceRef;
using Presentation.Process;
using System.Data;
namespace PresentationWPF.BaoCao.BCTK
{
    /// <summary>
    /// Interaction logic for ucBaoCaoThongKe.xaml
    /// </summary>
    public partial class ucBaoCaoThongKe : UserControl
    {
        #region Khai báo
        List<AutoCompleteEntry> listBaoCao = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        HT_BAOCAO htBaoCao = null;
        List<HT_BAOCAO_TSO> lstHtBaoCaoTso = null;
        BaoCaoProcess process = new BaoCaoProcess();
        BC_BIEUMAU objBieuMau = null;
        private DatabaseConstant.DanhSachBaoCaoTheoDinhKy dsachBaoCaoDinhKy;

        int idBaoCao;
        private string maBaoCao;

        public string MaBaoCao
        {
            get { return maBaoCao; }
            set { maBaoCao = value;
                  KhoiTaoForm();
                }
        }
        DataSet ds = null;
        #endregion

        #region Khởi tạo
        public ucBaoCaoThongKe()
        {
            InitializeComponent();
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
            lstSourceChiNhanh.Add(new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), "%", "0"));
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);

            // khởi tạo combobox
            auto = new AutoComboBox();
            lstSourcePhongGD.Add(new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), "%", "0"));
            auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue());
            LoadComboboxPhongGD();

            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref listBaoCao, ref cmbLoaiBieu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_BAO_CAO_TKE.getValue(), null);

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DONG_TIEN_CAC_NUOC.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ClientInformation.MaDongNoiTe);

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());
        }

        private void KhoiTaoForm()
        {
            AutoCompleteEntry auMaBaoCao = null;
            try
            {
                if (listBaoCao.Count > 0)
                {
                    cmbLoaiBieu.SelectedIndex = listBaoCao.IndexOf(listBaoCao.FirstOrDefault(e => e.KeywordStrings.First() == maBaoCao));
                    if (cmbLoaiBieu.SelectedIndex >= 0)
                        auMaBaoCao = listBaoCao.ElementAt(cmbLoaiBieu.SelectedIndex);
                    idBaoCao = Convert.ToInt32(auMaBaoCao.KeywordStrings[1]);
                    maBaoCao = auMaBaoCao.KeywordStrings[0];
                    // Lấy thông tin báo cáo và tham số                    
                    process.LayThongTinBaoCao(idBaoCao, maBaoCao, ref htBaoCao, ref lstHtBaoCaoTso);
                }
            }
            catch (System.Exception ex)
            {
            	
            }
        }
        #endregion

        #region Xử lý giao diện
        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();
        }

        private void LoadComboboxPhongGD()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();

            lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            if (!maChiNhanh.Equals("%"))
            {
                lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();
                lstSourcePhongGD_Select.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
            }
            else
                lstSourcePhongGD_Select = lstSourcePhongGD;
            // khởi tạo combobox
            auto = new AutoComboBox();
            cmbPhongGD.Items.Clear();
            auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
            cmbPhongGD.SelectedIndex = 0;
            cmbPhongGD.IsEnabled = !maChiNhanh.Equals("%");
        }
        
        #endregion

        private void tlbTongHop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AutoCompleteEntry auBaoCao = listBaoCao.ElementAt(cmbLoaiBieu.SelectedIndex);
                AutoCompleteEntry auChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex);
                AutoCompleteEntry auPhongGD = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex);
                List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaBaoCao", auBaoCao.KeywordStrings[0], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", LDateTime.DateToString(raddtNgayChotSoLieu.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayCuoiThang", LDateTime.DateToString(raddtNgayChotSoLieu.Value.GetValueOrDefault().GetLastDateOfMonth(), ApplicationConstant.defaultDateTimeFormat), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", auChiNhanh.KeywordStrings[0], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGD", auPhongGD.KeywordStrings[0], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@DinhKy", "1", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@DonViTinh", numDonViTinh.Value.GetValueOrDefault().ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@VonTuCo", numVonTuCo.Value.GetValueOrDefault().ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                // Chuẩn bị điều kiện cho báo cáo
                
                if (listThamSoBaoCao != null && listThamSoBaoCao.Count > 0)
                {
                    if (lstHtBaoCaoTso.Where(t => t.LOAI_TSO == ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri() && t.MA_TSO.Equals("@DT_THAMSO")).Count() > 0)
                    {
                        lstHtBaoCaoTso = new List<HT_BAOCAO_TSO>();
                        foreach (ThamSoBaoCao thamSoBaoCao in listThamSoBaoCao)
                        {
                            HT_BAOCAO_TSO tso = new HT_BAOCAO_TSO();
                            tso.MA_TSO = thamSoBaoCao.MaThamSo;
                            tso.LOAI_TSO = thamSoBaoCao.LoaiThamSo;
                            tso.GTRI_TSO = thamSoBaoCao.GiaTriThamSo;
                            lstHtBaoCaoTso.Add(tso);
                        }
                    }
                    else
                    {
                        foreach (HT_BAOCAO_TSO htBaoCaoTso in lstHtBaoCaoTso)
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
                ds = process.LayDuLieuBaoCaoTKe(idBaoCao, maBaoCao, ref objBieuMau, htBaoCao, lstHtBaoCaoTso);
                BuildData();
            }
            catch (System.Exception ex)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {

            }
        }

        private void BuildData()
        {
            try
            {
                if (!LObject.IsNullOrEmpty(ds) && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    raddgrDuLieuBCao.ItemsSource = null;
                    raddgrDuLieuBCao.ItemsSource = ds.Tables[0].DefaultView;
                    LoadWidthGridView();
                }
                else 
                {
                    raddgrDuLieuBCao.ItemsSource = null;
                    LMessage.ShowMessage("Không có dữ liệu đối với điều kiện tổng hợp tương ứng", LMessage.MessageBoxType.Warning);
                }
            }
            catch (System.Exception ex)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                
            }
            finally
            {
            }
        }

        private void LoadWidthGridView()
        {
            List<string> lstheader = objBieuMau.DS_TTINH_HTHI.Split('#').ToList();
            List<string> lsttype = objBieuMau.DS_DS_KIEU_DL_TTINH.Split('#').ToList();
            int idx = 3;
            raddgrDuLieuBCao.Columns[1].Width = 0;
            raddgrDuLieuBCao.Columns[1].IsVisible = false;
            raddgrDuLieuBCao.Columns[2].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Star);
            raddgrDuLieuBCao.Columns[2].Header = LLanguage.SearchResourceByKey("U.BaoCao.BCTK.ucBaoCaoThongKe.TenChiTieu");
            if(lstheader.Count<=lsttype.Count)
            {
                for (int i = 0; i < lstheader.Count;i++ )
                {
                    raddgrDuLieuBCao.Columns[idx].Header = LLanguage.SearchResourceByKey(lstheader[i]);
                    GridViewDataColumn dtCol = raddgrDuLieuBCao.Columns[idx] as GridViewDataColumn;
                    if (lsttype[i].ToUpper().Equals("NUMBER") || lsttype[i].ToUpper().Equals("INT"))
                        dtCol.DataFormatString = "{0:N0}";
                    else if (lsttype[i].ToUpper().Equals("DECIMAL"))
                        dtCol.DataFormatString = "{0:N2}";
                    idx = idx + 1;
                }
            }
            for (int i = idx; i < raddgrDuLieuBCao.Columns.Count; i++)
            {
                raddgrDuLieuBCao.Columns[i].IsVisible = false;
                raddgrDuLieuBCao.Columns[i].Width = 0;
            }
        }

        private bool Validation()
        {
            if (LObject.IsNullOrEmpty(raddgrDuLieuBCao.ItemsSource))
            {
                LMessage.ShowMessage("Cần tổng hợp dữ liệu trước khi in báo cáo",LMessage.MessageBoxType.Warning);
                return false;
            }
            return true;
        }

        public List<ThamSoBaoCao> GetParameters()
        {
            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
            try
            {
                if (!Validation())
                    return null;
                AutoCompleteEntry auChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex);
                AutoCompleteEntry auBaoCao = listBaoCao.ElementAt(cmbLoaiBieu.SelectedIndex);
                AutoCompleteEntry auPhongGD = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex);
                string maDonViBC = "";
                if (auChiNhanh.KeywordStrings[0].Equals("%"))
                    maDonViBC = ClientInformation.MaToChuc;
                else
                    maDonViBC = auChiNhanh.KeywordStrings[0];
                DataTable table = ((DataView)raddgrDuLieuBCao.ItemsSource).Table;
                listThamSoBaoCao.Add(new ThamSoBaoCao("P_DonViTinh", numDonViTinh.Value.GetValueOrDefault().ToString(), ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri(), new List<string>(), table.DataSet));
                listThamSoBaoCao.Add(new ThamSoBaoCao("P_NguoiLap", ClientInformation.HoTen, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri(), new List<string>(), table.DataSet));
                listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayDuLieu", LDateTime.DateToString(raddtNgayChotSoLieu.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat), ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri(), new List<string>(), table.DataSet));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayInBCao", LDateTime.DateToString(raddtNgayBaoCao.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri(), new List<string>(), table.DataSet));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaBaoCao", auBaoCao.KeywordStrings[0], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", LDateTime.DateToString(raddtNgayChotSoLieu.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", auChiNhanh.KeywordStrings[0], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGD", auPhongGD.KeywordStrings[0], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@DinhKy", "1", ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("@DonViTinh", numDonViTinh.Value.GetValueOrDefault().ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                return listThamSoBaoCao;
            }
            catch (System.Exception ex)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            return listThamSoBaoCao;
        }
    }
}
