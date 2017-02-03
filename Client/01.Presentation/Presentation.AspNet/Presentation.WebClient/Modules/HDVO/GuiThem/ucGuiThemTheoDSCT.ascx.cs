using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presentation.WebClient.Business;
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.HuyDongVonServiceRef;
using Presentation.Process.LaiSuatServiceRef;
using Presentation.Process.KhachHangServiceRef;
using Presentation.Process.BaoCaoServiceRef;
using Presentation.WebClient.Business.CustomControl;
using System.Data;

namespace Presentation.WebClient.Modules.HDVO.GuiThem
{
    public partial class ucGuiThemDanhSachCT : ControlBase
    {
        #region
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private DatabaseConstant.Function function = DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_DANH_SACH;
       
        private DataSet dsGuiThem;

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public DataGrid ChildGrid
        {
            get { return grGuiThemDS; }
        }
        private string maGiaoDich = "";

        private KIEM_SOAT _objKiemSoat = null;

        private string sTrangThaiNVu = "";

        private HDV_GUI_TIEN_THEO_DANH_SACH obj;
        public HDV_GUI_TIEN_THEO_DANH_SACH Obj
        {
            get { return obj; }
            set { obj = value; }
        }

        List<AutoCompleteEntry> lstSourceGD_HinhThuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();

      
        #endregion

       
        protected void Page_Load(object sender, EventArgs e)
        {
            pnThongtinGiaodich.GroupingText = LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS.ThongTinGiaoDich");
            pnDanhsachGuithem.GroupingText = LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.GiaoDichGuiThem.ucGuiThemTienGuiTheoDS.DanhSachGuiThem");
            if (!IsPostBack)
            {
                LoadCombobox();
                KhoiTaoGridGuiThem();
                if (HttpContext.Current.Request.QueryString["ID"] != null && HttpContext.Current.Request.QueryString["t"] != null && HttpContext.Current.Request.QueryString["MAGD"] != null)
                {
                    ID_GIAO_DICH.Value = HttpContext.Current.Request.QueryString["ID"].ToString();
                    inpIDSO.Value = HttpContext.Current.Request.QueryString["ID"].ToString();
                    LOAI_HANH_DONG.Value = HttpContext.Current.Request.QueryString["t"].ToString();
                    MA_GIAO_DICH.Value = HttpContext.Current.Request.QueryString["MAGD"].ToString();
                    
                }
                else
                {
                    inpAction.Value = "THEM";

                    ResetForm();
                }
                SetFormData();
            }
        }

        private void LoadCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.HINH_THUC_GIAO_DICH));
            auto.GenAutoComboBox(ref lstSourceGD_HinhThuc, ref cmbGD_HinhThuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri());

            auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TIENTE.getValue(), null, AppConfig.LoginedUser.MaDongNoiTe);

        }
        private void KhoiTaoGridGuiThem()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("SO_SO_TG", typeof(string));
            dt.Columns.Add("TEN_KHANG", typeof(string));
            dt.Columns.Add("NGAY_MO_SO", typeof(string));
            dt.Columns.Add("NGAY_DEN_HAN", typeof(string));
            dt.Columns.Add("KY_HAN", typeof(string));
            dt.Columns.Add("SO_DU", typeof(decimal));
            dt.Columns.Add("LAI_SUAT", typeof(decimal));
            dt.Columns.Add("SO_TIEN_GUI_THEM", typeof(decimal));
            dt.Columns.Add("SO_DU_MOI", typeof(decimal));
            dt.Columns.Add("TAI_KHOAN_THANH_TOAN", typeof(decimal));
            dsGuiThem = new DataSet();
            dsGuiThem.Tables.Add(dt);

        }

        private void SetFormData()
        {
            string SO_GIAO_DICH = MA_GIAO_DICH.Value;
            try
            {
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                DataSet ds = processHDV.GetThongTinGuiThemTheoDS(AppConfig.LoginedUser.MaDonViGiaoDich, SO_GIAO_DICH);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    lblTrangThai.Text = LanguageEngine.Instance().GetContent(LanguageType.TypeUI, BusinessConstant.layMaNgonNguNghiepVu(dr["TTHAI_NVU"].ToString())); 
                    raddtNgay.Text = LDateTime.StringToDate(dr["NGAY_DL"].ToString(), "yyyyMMdd").ToString("dd/MM/yyyy");
                    txtMaGiaoDich.Text = SO_GIAO_DICH;
                    txtNguoiGiaoDich.Text = dr["TEN_KHANG"].ToString();
                    txtDiaChi.Text = dr["DIA_CHI"].ToString();
                    txtDienGiai.Text = dr["DIEN_GIAI"].ToString();
                    
                    TRANG_THAI_BAN_GHI.Value = dr["TTHAI_BGHI"].ToString();
                    TRANG_THAI_NGHIEP_VU.Value = dr["TTHAI_NVU"].ToString();
                    #region Tab thông tin kiểm soát
                    txtTrangThai.Text = LanguageEngine.Instance().GetContent(LanguageType.TypeUI, BusinessConstant.layMaNgonNguSuDung(dr["TTHAI_BGHI"].ToString())); 
                    teldtNgayNhap.Text = LDateTime.StringToDate(dr["NGAY_NHAP"].ToString(), "yyyyMMdd").ToString("dd/MM/yyyy");
                    txtNguoiLap.Text = dr["NGUOI_NHAP"].ToString();
                    if (LDateTime.IsDate(dr["NGAY_CNHAT"].ToString(), "yyyyMMdd") == true)
                        teldtNgayCNhat.Text = LDateTime.StringToDate(dr["NGAY_CNHAT"].ToString(), "yyyyMMdd").ToString("dd/MM/yyyy");
                    else
                        teldtNgayCNhat.Text = "";
                    txtNguoiCapNhat.Text = dr["NGUOI_CNHAT"].ToString();
                    #endregion
                }

                dsGuiThem = processHDV.GetThongTinGuiThemTheoDSCTiet(AppConfig.LoginedUser.MaDonViGiaoDich, SO_GIAO_DICH);
                if (dsGuiThem != null)
                {
                    //DataViewManager dataViewManager = new DataViewManager(dsGuiThem);
                   // DataView dataView = dataViewManager.CreateDataView(dsGuiThem.Tables[0]);
                    grGuiThemDS.DataSource = dsGuiThem;
                    grGuiThemDS.DataBind();
                }

                TinhTong();
            }
            catch (Exception ex)
            {                
            }
        }
        private void ResetForm()
        {
            txtMaGiaoDich.Text = "";
            txtNguoiGiaoDich.Text = "";
            txtDiaChi.Text = "";
            //numThuTienKy.Value = 0;
            txtDienGiai.Text = "";
            raddtNgay.Text = LDateTime.StringToDate(AppConfig.LoginedUser.NgayLamViecHienTai, "yyyyMMdd").ToString("dd/MM/yyyy");

            dsGuiThem.Tables[0].Rows.Clear();
            
            grGuiThemDS.DataSource = dsGuiThem;
            grGuiThemDS.DataBind();

            lblTongSo.Text = "0";
            lblTongoDuCu.Text = "0";
            lblTongTienGuiThem.Text ="0";
            lblTongSoDuMoi.Text = "0";

            //Thông tin kiểm soát
            txtTrangThai.Text = "";
            teldtNgayNhap.Text = LDateTime.StringToDate(AppConfig.LoginedUser.NgayLamViecHienTai, "yyyyMMdd").ToString("dd/MM/yyyy");
            txtNguoiLap.Text = AppConfig.LoginedUser.UserName;
            teldtNgayCNhat.Text = "";
            txtNguoiCapNhat.Text = "";
        }
        /// <summary>
        /// Hàm tính toán các thông tin liên quan đến danh sách sổ
        /// Tổng số sổ
        /// Tổng số dư cũ
        /// Tổng tiền gửi thêm
        /// Tổng số dư mới
        /// </summary>
        private void TinhTong()
        {
            try
            {
                int tongSoSo = dsGuiThem.Tables[0].Rows.Count;
                decimal tongDuCu = 0;
                decimal tongGuiThem = 0;
                decimal tongDuMoi = 0;
                tongDuCu = decimal.Parse(dsGuiThem.Tables[0].Compute("sum(SO_DU)","").ToString());
                tongGuiThem = decimal.Parse(dsGuiThem.Tables[0].Compute("sum(SO_TIEN_GUI_THEM)", "").ToString());
                tongDuMoi = decimal.Parse(dsGuiThem.Tables[0].Compute("sum(SO_DU_MOI)", "").ToString());
                //foreach (DataRow dr in dsGuiThem.Tables[0].Rows)
                //{
                //    tongDuCu = tongDuCu + Convert.ToDecimal(dr["SO_DU"]);
                //    tongGuiThem = tongGuiThem + Convert.ToDecimal(dr["SO_TIEN_GUI_THEM"]);
                //    tongDuMoi = tongDuMoi + Convert.ToDecimal(dr["SO_DU_MOI"]);
                //}

                if (tongSoSo != 0)
                {
                    lblTongSo.Text = String.Format("{0:#,#}", tongSoSo);
                }
                else
                {
                    lblTongSo.Text = "0";
                }

                if (tongDuCu != 0)
                {
                    lblTongoDuCu.Text = String.Format("{0:#,#}", tongDuCu);
                }
                else
                {
                    lblTongoDuCu.Text = "0";
                }

                if (tongGuiThem != 0)
                {
                    lblTongTienGuiThem.Text = String.Format("{0:#,#}", tongGuiThem);
                }
                else
                {
                    lblTongTienGuiThem.Text = "0";
                }

                if (tongDuMoi != 0)
                {
                    lblTongSoDuMoi.Text = String.Format("{0:#,#}", tongDuMoi);
                }
                else
                {
                    lblTongSoDuMoi.Text = "0";
                }

            }
            catch (Exception ex)
            {
                
            }
        }

        protected void grGuiThemDS_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            e.Item.Attributes.Add("onmouseover", "currColor=this.style.backgroundColor;this.style.backgroundColor='#FDDBA5'");
            e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currColor");
        }
    }
}