using Presentation.WebClient.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using Utilities.Common;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.HuyDongVonServiceRef;
using Presentation.Process.LaiSuatServiceRef;
using Presentation.Process.KhachHangServiceRef;
using Presentation.WebClient.Business.CustomControl;
namespace Presentation.WebClient.Modules.HDVO.GuiThem
{
    public partial class ucGuiThemCT : ControlBase
    {
        #region Khai bao

        private KIEM_SOAT _objKiemSoat = null;
        public KIEM_SOAT objKiemSoat
        {
            get { return _objKiemSoat; }
            set { _objKiemSoat = value; }
        }

        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        public event EventHandler OnSavingCompleted;

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private string maGiaoDich;
        public string MaGiaoDich
        {
            get { return maGiaoDich; }
            set { maGiaoDich = value; }
        }

        private HDV_GUI_TIEN_THEO_SO obj;
        public HDV_GUI_TIEN_THEO_SO Obj
        {
            get { return obj; }
            set { obj = value; }
        }

        private string sTrangThaiNVu = "";

        private string maSanPham = "";

        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceGD_HinhThuc = new List<AutoCompleteEntry>();

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        /*Cờ đánh dấu trạng thái khi LoadForm: 
         * 0 là khi gọi từ Main chương trình lần đầu
         * 1 là khi thêm từ Form danh sách
         * 2 là khi sửa từ Form danh sách 
         * 3 là khi xem từ Form danh sách
         * -1 là Khi đã load Form tránh trường hợp load nhiều lần
        */
        private int flag = 0;
        public int Flag
        {
            get { return flag; }
            set { flag = value; }
        }


        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            
            grbThongtinsotiengui.GroupingText = LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.ThongTinSoTGui");
            grbThongtinKhachang.GroupingText = LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.ThongTinKH");
            tbiKiemSoat.GroupingText = LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.Tab.ThongTinKiemSoat_2");
            pnThongtinGiaodich.GroupingText = LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiCT.ThongTinGD");
            cmbGD_HinhThuc.Attributes.Add("onchange", "changehtgd()");
            inpTrangThai.Value = CommonFuntion.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));
            if (!IsPostBack)
            {
                inpAction.Value = "THEM";
                LoadCombobox();
                ResetForm();
            }
        }
        /// <summary>
        /// Load combobox
        /// </summary>
        private void LoadCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();

            auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TIENTE.getValue());

            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.HINH_THUC_GIAO_DICH));
            auto.GenAutoComboBox(ref lstSourceGD_HinhThuc, ref cmbGD_HinhThuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri());
        }
        private void ResetForm()
        {
            //Thông tin sổ
            lblTrangThai.Text = "";
            txtSoGD.Text = "";
            txtSoTGui.Text = "";
            numSoDu.Text = "0";
           // cmbLoaiTien.SelectedIndex = -1;
            numLaiSuat.Text = "";
            raddtNgayMo.Text = "";
            raddtNgayDH.Text = "";
            raddtNgayCap.Text = "";

            //Thông tin khách hàng
            txtMaKH.Text = "";
            txtTenKH.Text = "";
            txtDiaChi.Text = "";
            txtSoCMT.Text = "";
            raddtNgayCap.Text = "";
            txtNoiCap.Text = "";
            txtSDT.Text = "";

            //Thông tin giao dịch
            cmbGD_HinhThuc.SelectedIndex = 0;// lstSourceGD_HinhThuc.IndexOf(lstSourceGD_HinhThuc.FirstOrDefault(e => e.KeywordStrings.First().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri())));
            numGD_TongTien.Text = "0";
            numGD_SoTienTM.Text = "0";
            numGD_SoTienCK.Text = "0";
            txtGD_TaiKhoanKH.Text = "";
            txtGD_TaiKhoanNB.Text = "";
            txtDienGiai.Text = "";

            //Thông tin kiểm soát
            txtTrangThai.Text = "";
            teldtNgayNhap.Text = LDateTime.StringToDate(AppConfig.LoginedUser.NgayLamViecHienTai, "yyyyMMdd").ToString("dd/MM/yyyy");
            txtNguoiLap.Text = AppConfig.LoginedUser.UserName;
            teldtNgayCNhat.Text = "";
            txtNguoiCapNhat.Text = "";
        }
        private void SetEnabledAllControls(bool enable)
        {
            txtSoTGui.Enabled = enable;
            btnSoTgui.Disabled = !enable;

            #region Giao dịch
            cmbGD_HinhThuc.Enabled = enable;
            numGD_TongTien.Enabled = enable;
            if (enable == true)
            {
                string sHinhThucGD = cmbGD_HinhThuc.SelectedItem.Value;
                if (sHinhThucGD.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri()))
                {
                    numGD_SoTienTM.ReadOnly = false;
                    numGD_SoTienCK.Enabled = false;
                    txtGD_TaiKhoanKH.Enabled = false;
                    btnGD_TaiKhoanKH.Disabled = true;
                    txtGD_TaiKhoanNB.Enabled = false;
                    btnGD_TaiKhoanNB.Disabled = true;
                }
                else if (sHinhThucGD.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN.layGiaTri()))
                {
                    numGD_SoTienTM.Enabled = false;
                    numGD_SoTienCK.Enabled = false;
                    txtGD_TaiKhoanKH.Enabled = true;
                    btnGD_TaiKhoanKH.Disabled = false;
                    txtGD_TaiKhoanNB.Enabled = true;
                    btnGD_TaiKhoanNB.Disabled = false;
                }
                else if (sHinhThucGD.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TMAT_CKHOAN.layGiaTri()))
                {
                    numGD_SoTienTM.Enabled = false;
                    numGD_SoTienCK.Enabled = true;
                    txtGD_TaiKhoanKH.Enabled = true;
                    btnGD_TaiKhoanKH.Disabled = false;
                    txtGD_TaiKhoanNB.Enabled = true;
                    btnGD_TaiKhoanNB.Disabled = false;
                }
            }
            else
            {
                numGD_SoTienTM.Enabled = enable;
                numGD_SoTienCK.Enabled = enable;
                txtGD_TaiKhoanKH.Enabled = enable;
                txtGD_TaiKhoanNB.Enabled = enable;
                btnGD_TaiKhoanKH.Disabled = !enable;
                btnGD_TaiKhoanNB.Disabled = !enable;
            }
            #endregion

            txtDienGiai.Enabled = enable;
        }

        private void GetFormData(ref HDV_GUI_TIEN_THEO_SO obj, string sTrangThaiNVu)
        {
            try
            {
                //Thông tin sổ               
                obj.MA_GDICH = txtSoGD.Text;
                obj.SO_SO_TG = txtSoTGui.Text;
                obj.SO_DU = Convert.ToDecimal(numSoDu.Text.Replace(",",""));
                obj.MA_SAN_PHAM = maSanPham;
                obj.LOAI_TIEN = cmbLoaiTien.SelectedItem.Value;
                obj.LAI_SUAT = Convert.ToDecimal(numLaiSuat.Text.Replace(",", ""));
                obj.NGAY_MO_SO = LDateTime.StringToDate(raddtNgayMo.Text,"dd/MM/yyyy").ToString("yyyyMMdd");
                if (raddtNgayDH.Text != "")
                    obj.NGAY_DAO_HAN = LDateTime.StringToDate(raddtNgayDH.Text, "dd/MM/yyyy").ToString("yyyyMMdd");

                //Thông tin khách hàng
                obj.MA_KHACH_HANG = txtMaKH.Text;
                obj.TEN_KHACH_HANG = txtTenKH.Text;
                obj.DIA_CHI = txtDiaChi.Text;
                obj.SO_CMND = txtSoCMT.Text;
                if (raddtNgayCap.Text != "")
                    obj.NGAY_CAP = LDateTime.StringToDate(raddtNgayCap.Text, "dd/MM/yyyy").ToString("yyyyMMdd");
                obj.NOI_CAP = txtNoiCap.Text;
                obj.SO_DIEN_THOAI = txtSDT.Text;

                //Thông tin giao dịch
                obj.MA_DVI = AppConfig.LoginedUser.MaDonViGiaoDich;
                obj.MA_DVI_QLY = AppConfig.LoginedUser.MaDonVi;
                obj.NGAY_GDICH = AppConfig.LoginedUser.NgayLamViecHienTai;
                obj.HINH_THUC_GIAO_DICH = cmbGD_HinhThuc.SelectedItem.Value;
                obj.TONG_TIEN_GIAO_DICH = Convert.ToDecimal(numGD_TongTien.Text.Replace(",", ""));
                obj.SO_TIEN_MAT = Convert.ToDecimal(numGD_SoTienTM.Text.Replace(",", ""));
                obj.SO_TIEN_CHUYEN_KHOAN = Convert.ToDecimal(numGD_SoTienCK.Text.Replace(",", ""));
                obj.TAI_KHOAN_KHACH_HANG = txtGD_TaiKhoanKH.Text;
                obj.TAI_KHOAN_NOI_BO = txtGD_TaiKhoanNB.Text;
                obj.DIEN_GIAI = txtDienGiai.Text;

                //Thông tin kiểm soát
                obj.TRANG_THAI_NGHIEP_VU = sTrangThaiNVu;
                obj.TRANG_THAI_BAN_GHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.NGAY_LAP = LDateTime.StringToDate(teldtNgayNhap.Text, "dd/MM/yyyy").ToString("yyyyMMdd");
                obj.NGUOI_LAP = txtNguoiLap.Text;
                if (action != DatabaseConstant.Action.THEM)
                {
                    obj.NGAY_CAP_NHAT = AppConfig.LoginedUser.NgayLamViecHienTai;
                    obj.NGUOI_CAP_NHAT = AppConfig.LoginedUser.UserName;
                }
            }
            catch (Exception ex)
            {
               
            }
        }
        public void BeforeViewFromDetail()
        {
            SetEnabledAllControls(false);
            action = DatabaseConstant.Action.XEM;
            inpAction.Value = "XEM";
            //tlbPreview.IsEnabled = true;
        }
    }
}