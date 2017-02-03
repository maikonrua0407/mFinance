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
using Presentation.Process.Common;
using Utilities.Common;
using PresentationWPF.CustomControl;
using Presentation.Process.BaoCaoServiceRef;
using Presentation.Process;
using System.Data;

namespace PresentationWPF.BaoCao.DungChung
{
    /// <summary>
    /// Interaction logic for ucTiLeAnToanVonToiThieu.xaml
    /// </summary>
    public partial class ucNhapLieu : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> listBaoCao = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        public string MaBaoCao = "";
        List<BC_BCTK_DU_LIEU> lstBCTKDuLieu = new List<BC_BCTK_DU_LIEU>();
        #endregion

        #region Khoi tao
        public ucNhapLieu()
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
            auto.GenAutoComboBox(ref listBaoCao, ref cmbLoaiBieu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_BAO_CAO_TKE.getValue(), null, MaBaoCao);

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DONG_TIEN_CAC_NUOC.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ClientInformation.MaDongNoiTe);
        }

        #endregion

        #region Xu ly giao dien
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
        }


        #endregion

        private void cmbLoaiBieu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoCompleteEntry auLoaiBieu = listBaoCao.ElementAt(cmbLoaiBieu.SelectedIndex);
            AutoCompleteEntry auChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex);
            AutoCompleteEntry auPGDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex);
            string NgayDuLieu = LDateTime.DateToString(raddtNgayChotSoLieu.Value.GetValueOrDefault(),ApplicationConstant.defaultDateTimeFormat);
            string NgayDauThang = raddtNgayChotSoLieu.Value.GetValueOrDefault().GetFirstDateOfMonth(ApplicationConstant.defaultDateTimeFormat);
            string NgayCuoiThang = raddtNgayChotSoLieu.Value.GetValueOrDefault().GetLastDateOfMonth(ApplicationConstant.defaultDateTimeFormat);
            if(!LObject.IsNullOrEmpty(auLoaiBieu))
            {
                LoadDuLieu();
            }
        }

        #region Xu ly nghiep vu
        private void LoadDuLieu()
        {
            AutoCompleteEntry auBieuMau = listBaoCao.ElementAt(cmbLoaiBieu.SelectedIndex);
            AutoCompleteEntry auChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex);
            AutoCompleteEntry auPGD = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex);
            string NgayChotDuLieu = LDateTime.DateToString(raddtNgayChotSoLieu.Value.GetValueOrDefault(),ApplicationConstant.defaultDateTimeFormat);
            string NgayDauThang = LDateTime.DateToString(raddtNgayChotSoLieu.Value.GetValueOrDefault().GetFirstDateOfMonth(),ApplicationConstant.defaultDateTimeFormat);
            string NgayCuoiThang = LDateTime.DateToString(raddtNgayChotSoLieu.Value.GetValueOrDefault().GetLastDateOfMonth(),ApplicationConstant.defaultDateTimeFormat);
            DataSet ds = new BaoCaoProcess().GetDuLieuNhapBCTK(auChiNhanh.KeywordStrings[0], auPGD.KeywordStrings[0], auBieuMau.KeywordStrings[0], NgayChotDuLieu, NgayDauThang, NgayCuoiThang);
            if (!LObject.IsNullOrEmpty(ds) && !LObject.IsNullOrEmpty(ds.Tables["BCTK"]))
            {
                raddgrDuLieuBCao.ItemsSource = ds.Tables["BCTK"].DefaultView;
            }
        }

        private void tlbSave_Click(object sender, RoutedEventArgs e)
        {
            ApplicationConstant.ResponseStatus responseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            if(Vadition())
            {
                GetDataForm();
                responseStatus = new BaoCaoProcess().NhapDuLieuDauVaoBCTK(ref lstBCTKDuLieu);
                if (responseStatus.Equals(ApplicationConstant.ResponseStatus.KHONG_THANH_CONG))
                    LMessage.ShowMessage("M.DungChung.LuuDuLieuKhongThanhCong", LMessage.MessageBoxType.Error);
                else
                    LMessage.ShowMessage("M.DungChung.LuuDuLieuThanhCong", LMessage.MessageBoxType.Information);
            }
        }

        private void tlbReset_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GetDataForm()
        {
            
            DataView dv = raddgrDuLieuBCao.ItemsSource as DataView;
            AutoCompleteEntry auBieuMau = listBaoCao.ElementAt(cmbLoaiBieu.SelectedIndex);
            AutoCompleteEntry auChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex);
            try
            {
                foreach (DataRowView drv in dv)
                {
                    BC_BCTK_DU_LIEU objBCTKDuLieu = new BC_BCTK_DU_LIEU();
                    objBCTKDuLieu.ID_CHI_TIEU = Convert.ToInt32(drv["ID_CHI_TIEU"]);
                    objBCTKDuLieu.MA_DVI_QLY = auChiNhanh.KeywordStrings[0];
                    objBCTKDuLieu.MA_MAU_BIEU = auBieuMau.KeywordStrings[0];
                    objBCTKDuLieu.NGAY_DU_LIEU = LDateTime.DateToString(raddtNgayChotSoLieu.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
                    objBCTKDuLieu.NGAY_NHAP = LDateTime.DateToString(raddtNgayBaoCao.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
                    objBCTKDuLieu.SO_TIEN = Convert.ToDecimal(drv["GIA_TRI"]);
                    objBCTKDuLieu.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    objBCTKDuLieu.TTHAI_BGHI = BusinessConstant.TrangThaiSuDung.SU_DUNG.layGiaTri();
                    objBCTKDuLieu.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    lstBCTKDuLieu.Add(objBCTKDuLieu);
                }
            }
            catch (System.Exception ex)
            {
            	
            }
            
        }

        private bool Vadition()
        {
            return true;
        }
        #endregion

        
    }
}
