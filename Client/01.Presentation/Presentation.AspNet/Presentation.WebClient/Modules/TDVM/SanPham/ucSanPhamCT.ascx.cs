using Presentation.WebClient.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using Utilities.Common;
using Presentation.WebClient.Business.CustomControl;
using Presentation.Process;
using Presentation.Process.TinDungServiceRef;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.QuanTriHeThongServiceRef;
using System.Collections;
using System.Reflection;
using System.Globalization;

namespace Presentation.WebClient.Modules.TDVM.SanPham
{
    public partial class ucSanPhamCT : ControlBase
    {
        #region Khai bao
        List<AutoCompleteEntry> lstPhuongThucVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstMucDichSuDung = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiSanPham = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstNguonVonVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstNhomVongVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstPhuongThucTinhLai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstCoSoTinhLai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiLSuat = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstTanSuat = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiHachToan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiTienKyQuy = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiTienLai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiTienGoc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiTienGop = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstPThucTinhKQuy = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstThoiHanVay = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstHinhThucTra = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstHinhThucTraGoc = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstHinhThucTraLai = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstHanMucGocVay = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstHanMucKyHan = new List<AutoCompleteEntry>();
        DataSet dsLaiSuat;
        public int idSanPham = 0;
        int idLaiSuat = 0;
        int idCoSoTinhLai = 0;
        int idVongVay = 0;
        string maLaiSuat="";
        string maHinhThucChoVay="";
        string maMucDichSuDung="";
        string maLoaiSanPham="";
        string maNguonVon="";
        string maVongVay="";
        string maCoSoTLai="";
        string maLoaiLSuat="";
        string maTanSuatDanhGia="";
        string maPhuongThucTLai="";
        decimal dLaiSuat;
        string sTrangThai="";
        string sBaremTinhLai = "";
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
       
       // public event EventHandler OnSavingComleted = null;
        List<VONG_VAY_CTIET> lstVongVay = null;
        SAN_PHAM_TIN_DUNG obj = new SAN_PHAM_TIN_DUNG();
        string Round = "";
        string tyLeHoanTraGoc = "";
        #endregion

        #region Khoi tao
        protected void Page_Load(object sender, EventArgs e)
        {
           
            rbTuongdoi.Attributes.Add("onchange", "changeTypeKQ()");
            rbTuyetdoi.Attributes.Add("onchange", "changeTypeKQ()");
            cmbLoaiHachToan.Attributes.Add("onChange", "changecmbLoaiHachToan('"+cmbLoaiHachToan.ClientID+"','"+AppConfig.LoginedUser.MaDonViGiaoDich+"','"+AppConfig.LoginedUser.MaDonVi+"','"+DatabaseConstant.PHUONG_PHAP_HACH_TOAN.DOC_LAP.layGiaTri()+"');");
            txtBienDo.Attributes.Add("onchange","changebiendo(this,"+dLaiSuat.ToString()+")");
            if (!IsPostBack)
            {
                if (HttpContext.Current.Request.QueryString["ID"].ToString() != "")
                {
                    try
                    {
                        idSanPham = int.Parse(HttpContext.Current.Request.QueryString["ID"].ToString());
                        inpID.Value = idSanPham.ToString();
                    }
                    catch
                    {
                        Response.Write("<script>window.close();</script>");
                    }
                }
                KhoiTaoComboBox();
                // Khoi tao gia tri cho Griview Bang ke goc lai
                KhoiTaoGiaTriBangGocLaiVayDS();
                //ShowControl();
                Round = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_MUC_LAM_TRON_TD_TINH_LAI, ClientInformation.MaDonVi);
                tyLeHoanTraGoc = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_TY_LE_HOAN_TRA_GOC, ClientInformation.MaDonVi);

                if (idSanPham != 0)
                {
                    LoadDuLieuCT(true);

                }
                else
                    ClearForm();
            }
            idSanPham = int.Parse(inpID.Value);
            if (cfaction.Value == "edit")
            {
                Sua();
                cfaction.Value = "0";
            }
            else if (cfaction.Value == "delete")
            {
                List<string> kq = Xoa();
                if (kq != null && kq.Count > 0)
                {
                    string strkq = "";
                    for (int j = 0; j < kq.Count; j++)
                    {
                        strkq += kq[j].Split('#')[0] + " --    " + kq[j].Split('#')[1] + "\n";
                    }
                    inpshowresult.Value = "1";
                    lblErr.Text = strkq;
                }               
                cfaction.Value = "0";
            }
            else if (cfaction.Value == "submit")
            {
                List<string> kq=LuuDuLieu(BusinessConstant.TrangThaiBanGhi.SU_DUNG, BusinessConstant.TrangThaiNghiepVu.CHO_DUYET, DatabaseConstant.Action.TRINH_DUYET);
                
                if (kq != null && kq.Count > 0)
                {
                    string strkq = "";
                    for (int j = 0; j < kq.Count; j++)
                    {
                        strkq += kq[j].Split('#')[0] + " --    " + kq[j].Split('#')[1] + "\n\r";
                    }
                    inpshowresult.Value = "1";
                    lblErr.Text = strkq;
                }
                cfaction.Value = "0";
            }
            else if (cfaction.Value == "approve")
            {
                List<string> kq = Duyet();
                if (kq != null && kq.Count > 0)
                {
                    string strkq = "";
                    for (int j = 0; j < kq.Count; j++)
                    {
                        strkq += kq[j].Split('#')[0] + " --    " + kq[j].Split('#')[1] + "\n";
                    }
                    inpshowresult.Value = "1";
                    lblErr.Text = strkq;
                }
                cfaction.Value = "0";
            }
            else if (cfaction.Value == "reject")
            {
                List<string> kq = TuChoiDuyet();
                if (kq != null && kq.Count > 0)
                {
                    string strkq = "";
                    for (int j = 0; j < kq.Count; j++)
                    {
                        strkq += kq[j].Split('#')[0] + " --    " + kq[j].Split('#')[1] + "\n";
                    }
                    inpshowresult.Value = "1";
                    lblErr.Text = strkq;
                }
                cfaction.Value = "0";
            }
            else if (cfaction.Value == "refuse")
            {
                List<string> kq = ThoaiDuyet();
                if (kq != null && kq.Count > 0)
                {
                    string strkq = "";
                    for (int j = 0; j < kq.Count; j++)
                    {
                        strkq += kq[j].Split('#')[0] + " --    " + kq[j].Split('#')[1] + "\n";
                    }
                    inpshowresult.Value = "1";
                    lblErr.Text = strkq;
                }
                cfaction.Value = "0";
            }
        }
      protected  void KhoiTaoComboBox()
        {
            List<string> lstDieuKien = new List<string>();
            List<string> lstMaChon = new List<string>();
            string maTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            // Gán giá trị điều kiện
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.PHUONG_THUC_VAY));
            KhoiTaoGiaTriComboBox(ref lstPhuongThucVay, ref cmbHinhThucVay, maTruyVan, lstDieuKien, "TIN_CHAP");
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.MUC_DICH_VAY_VON));
            KhoiTaoGiaTriComboBox(ref lstMucDichSuDung, ref cmbMucDichVayVon, maTruyVan, lstDieuKien, "01");
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.LOAI_SAN_PHAM_TDUNG));
            KhoiTaoGiaTriComboBox(ref lstLoaiSanPham, ref cmbLoaiSanPham, maTruyVan, lstDieuKien, "VON_TRA_DAN");
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.PHUONG_THUC_TINH_LAI));
            KhoiTaoGiaTriComboBox(ref lstPhuongThucTinhLai, ref cmbPhuongThucTinh, maTruyVan, lstDieuKien, "DNO_BDAU");
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.DVI_TINH_KY_HAN));
            KhoiTaoGiaTriComboBox(ref lstThoiHanVay, ref cmbThoiHanVay, maTruyVan, lstDieuKien, BusinessConstant.DON_VI_TINH_THOI_GIAN.THANG.layGiaTri());
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.HINH_THUC_NOP_KQUY));
            KhoiTaoGiaTriComboBox(ref lstHinhThucTra, ref cmbHinhThucNop, maTruyVan, lstDieuKien);
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.PP_TINH_NOP_KQUY));
            KhoiTaoGiaTriComboBox(ref lstPThucTinhKQuy, ref cmbPPTinh, maTruyVan, lstDieuKien);
       
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.DON_VI_HACH_TOAN));
            KhoiTaoGiaTriComboBox(ref lstLoaiHachToan, ref cmbLoaiHachToan, maTruyVan, lstDieuKien, DatabaseConstant.PHUONG_PHAP_HACH_TOAN.DOC_LAP.layGiaTri());
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.HINH_THUC_TRA_GOC.getValue());
            lstMaChon.Add(BusinessConstant.HINH_THUC_TRA_GOC.DINH_KY.layGiaTri());
            lstMaChon.Add(BusinessConstant.HINH_THUC_TRA_GOC.TRA_SAU.layGiaTri());
            //--KhoiTaoGiaTriComboBox(ref lstHinhThucTraGoc, ref cmbDinhKyTraGoc, maTruyVan, lstDieuKien, BusinessConstant.HINH_THUC_TRA_GOC.DINH_KY.layGiaTri(), lstMaChon);
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.HINH_THUC_TRA_LAI.getValue());
           //-- KhoiTaoGiaTriComboBox(ref lstHinhThucTraLai, ref cmbDinhKyTraLai, maTruyVan, lstDieuKien, BusinessConstant.HINH_THUC_TRA_GOC.DINH_KY.layGiaTri());
            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.TINH_CHAT_VONG_VAY.getValue());
            KhoiTaoGiaTriComboBox(ref lstHanMucGocVay, ref cmbHanMucGocVay, maTruyVan, lstDieuKien, BusinessConstant.TINH_CHAT_VONG_VAY.CO_DINH.layGiaTri());
            KhoiTaoGiaTriComboBox(ref lstHanMucKyHan, ref cmbHanMucKHan, maTruyVan, lstDieuKien, BusinessConstant.TINH_CHAT_VONG_VAY.CO_DINH.layGiaTri());
            maTruyVan = "COMBOBOX_NHOMVONGVAY";
            lstNhomVongVay.Insert(0, new AutoCompleteEntry("",  "0"));
            KhoiTaoGiaTriComboBox3C(ref lstNhomVongVay, ref cmbNhomVongVay, maTruyVan);
            maTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_LOAI_LSUAT.getValue();
            KhoiTaoGiaTriComboBox3C(ref lstCoSoTinhLai, ref cmbCSTinhLai, maTruyVan);
            cmbCSTinhLai.SelectedIndex = 0;
            maTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_LOAITIEN.getValue();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.TINH_CHAT_VONG_VAY));
           
          // Gen ComboBox bằng việc gọi hàm

            //KhoiTaoGiaTriComboBox(ref lstLoaiTienGoc, ref cmbLoaiTienGoc, maTruyVan, lstDieuKien, AppConfig.LoginedUser.MaDongNoiTe,null);
            //KhoiTaoGiaTriComboBox(ref lstLoaiTienGop, ref cmbLoaiTienGop, maTruyVan, lstDieuKien, AppConfig.LoginedUser.MaDongNoiTe, null);
            KhoiTaoGiaTriComboBox(ref lstLoaiTienKyQuy, ref cmbLoaiTienKyQuy, maTruyVan, lstDieuKien, AppConfig.LoginedUser.MaDongNoiTe, null);
            //KhoiTaoGiaTriComboBox(ref lstLoaiTienLai, ref cmbLoaiTienLai, maTruyVan, lstDieuKien, AppConfig.LoginedUser.MaDongNoiTe, null);
            //KhoiTaoGiaTriComboBox(ref lstLoaiTienGoc, ref cmbLoaiTienGoc, maTruyVan, lstDieuKien, AppConfig.LoginedUser.MaDongNoiTe, null);
        }
        #endregion
        #region Xu ly giao dien
          
      

     
      /// <summary>
      /// Khởi tạo lấy Items cho ComboBox 
      /// </summary>
      /// <param name="lstAutoComplete"></param>
      /// <param name="cmbControl"></param>
      /// <param name="sMaTruyVan"></param>
      /// <param name="lstDKien"></param>
      /// <param name="bSelectChanged"></param>
        void KhoiTaoGiaTriComboBox(ref List<AutoCompleteEntry> lstAutoComplete, ref DropDownList cmbControl, string sMaTruyVan, List<string> lstDKien = null, string Chon = null, List<string> lstMaChon = null)
      {
          AutoComboBox autoComboBox = new AutoComboBox();
          autoComboBox.GenAutoComboBox(ref lstAutoComplete, ref cmbControl, sMaTruyVan, lstDKien, Chon, lstMaChon);
      }
        void KhoiTaoGiaTriComboBox3C(ref List<AutoCompleteEntry> lstAutoComplete, ref DropDownList cmbControl, string sMaTruyVan, List<string> lstDKien = null, string Chon = null, List<string> lstMaChon = null)
        {
            AutoComboBox autoComboBox = new AutoComboBox();
            autoComboBox.GenAutoComboBox3C(ref lstAutoComplete, ref cmbControl, sMaTruyVan, lstDKien, Chon, lstMaChon);
        }
      /// <summary>
      /// Clear form dữ liệu
      /// </summary>
      void ClearForm()
      {
          cmbHinhThucVay.Focus();
          txtMaLaiSuat.Text = "";
          txtMaSanPham.Text = "";
          idSanPham = 0;
          NumberFormatInfo provider = new NumberFormatInfo();
          provider.NumberDecimalSeparator = ".";
          provider.NumberGroupSeparator = ",";
          provider.NumberGroupSizes = new int[] { 3 };
          try
          {
              numTyLeHoanTraGoc.Text = Convert.ToDouble(tyLeHoanTraGoc, provider).ToString();
          }
          catch { numTyLeHoanTraGoc.Text = "100"; }
         
             
          txtTenSanPham.Text = "";
          txtMaLaiSuat.Text = "";
          txtBienDo.Text = "";
          txtLaiSuat.Text = "";

          KhoiTaoGiaTriBangGocLaiVayDS();
          lblTrangThai.Text = "";
          txtNguoiLap.Text = "";
          txtNguoiCapNhat.Text = "";
          teldtNgayHieuLuc.Text = LDateTime.StringToDate(AppConfig.LoginedUser.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat).ToString("dd/MM/yyyy");
          teldtNgayHetHieuLuc.Text = "";
          teldtNgayNhap.Text = LDateTime.StringToDate(AppConfig.LoginedUser.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat).ToString("dd/MM/yyyy");
          teldtNgayCNhat.Text = "";
          txtTrangThai.Text = sTrangThai = "";
          //CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, sTrangThai, mnuMain, DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM);

          LoadDuLieuTaiKhoanHachToan("MACDINH");
          grbGocLaiVay.Enabled = true;
          grbThongTinChung.Enabled = true;
          pntrangthai.Enabled = true;
          grbLaiSuat.Enabled = true;
      }

     
      
      void KhoiTaoGiaTriBangGocLaiVayDS()
      {
          lstVongVay = new List<VONG_VAY_CTIET>();
          raddgrGocLaiVayDS.DataSource = lstVongVay;
          raddgrGocLaiVayDS.DataBind();

      }
      /// <summary>
      /// Load dữ liệu chi tiết cho các control
      /// </summary>
      /// <param name="bSua"></param>
      public void LoadDuLieuCT(bool bSua)
      {
          try
          {
              DataSet dsChiTiet = new TinDungProcess().getSanPhamTDByID(idSanPham.ToString());
              if (dsChiTiet.Tables[0].Rows.Count > 0)
              {
                  LoadDuLieuThongTinChung(dsChiTiet);
                  LoadDuLieuKiemSoat(dsChiTiet);
                  TinhToanBangKeGocLai();
                  LoadDuLieuTaiKhoanHachToan(txtMaSanPham.Text);
                  TinhSoTienGopHangKy();
              }
              List<Control> lstConTrol = new List<Control>();
              lstConTrol.Add((Control)grbGocLaiVay);
              lstConTrol.Add((Control)grbThongTinChung);
              lstConTrol.Add((Control)grbLaiSuat);
              
              lstConTrol.Add((Control)grbKyQuy);
             // lstConTrol.Add((Control)grbHinhThuc);
              if (!bSua)
              {
                  LockControl(lstConTrol, false);
                 // CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, dsChiTiet.Tables[0].Rows[0]["TTHAI_NVU"].ToString(), mnuMain, DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM);
              }
              else
              {
                  LockControl(lstConTrol, true);
                  //CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, dsChiTiet.Tables[0].Rows[0]["TTHAI_NVU"].ToString(), mnuMain, DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM);
              }
          }
          catch (Exception ex)
          {
            //  LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
              LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
          }
      }
      void LockControl(List<Control> lstControl, bool bBool)
      {
          foreach (Control control in lstControl)
          {
              if (control is Panel)
              {
                 
                  ((Panel)(control)).Enabled = bBool;
              }
             
          }
          grdTKhoan.Enabled = !bBool;
          raddgrGocLaiVayDS.Enabled = !bBool;
      }
      /// <summary>
      /// Đưa dữ liệu vào các điều khiển tab thông tin chung
      /// </summary>
      /// <param name="dsThongTin"></param>
      private void LoadDuLieuThongTinChung(DataSet dsThongTin)
      {
          if (LObject.IsNullOrEmpty(obj.SAN_PHAM_TD)) obj.SAN_PHAM_TD = new TD_SAN_PHAM();
          lblTrangThai.Text = dsThongTin.Tables[0].Rows[0]["TEN_TTHAI_NVU"].ToString();
          sTrangThai = dsThongTin.Tables[0].Rows[0]["TTHAI_NVU"].ToString();
          obj.SAN_PHAM_TD.ID = idSanPham;
          txtMaSanPham.Text = obj.SAN_PHAM_TD.MA_SAN_PHAM = dsThongTin.Tables[0].Rows[0]["MA_SAN_PHAM"].ToString();
          cmbHinhThucVay.SelectedIndex = lstPhuongThucVay.IndexOf(lstPhuongThucVay.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dsThongTin.Tables[0].Rows[0]["PTHUC_VAY"])));
          txtTenSanPham.Text = dsThongTin.Tables[0].Rows[0]["TEN_SAN_PHAM"].ToString();
          cmbMucDichVayVon.SelectedIndex = lstMucDichSuDung.IndexOf(lstMucDichSuDung.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dsThongTin.Tables[0].Rows[0]["MUC_DICH_VAY"])));
          cmbLoaiSanPham.SelectedIndex = lstLoaiSanPham.IndexOf(lstLoaiSanPham.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dsThongTin.Tables[0].Rows[0]["LOAI_SAN_PHAM"])));
          cmbNhomVongVay.SelectedIndex = lstNhomVongVay.IndexOf(lstNhomVongVay.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dsThongTin.Tables[0].Rows[0]["MA_VONG_VAY"])));
         
          idVongVay = int.Parse(cmbNhomVongVay.SelectedValue.Split('|')[1]);// Convert.ToInt32(lstNhomVongVay.ElementAt(cmbNhomVongVay.SelectedIndex).KeywordStrings.ElementAt(1));
          maVongVay = cmbNhomVongVay.SelectedValue.Split('|')[0];////)

          if (dsThongTin.Tables[0].Rows[0]["TLE_HTRA_GOC"] != DBNull.Value)
              numTyLeHoanTraGoc.Text = Convert.ToDouble(dsThongTin.Tables[0].Rows[0]["TLE_HTRA_GOC"]).ToString();
          teldtNgayHieuLuc.Text = LDateTime.StringToDate(dsThongTin.Tables[0].Rows[0]["NGAY_ADUNG"].ToString(), "yyyyMMdd").ToString("dd/MM/yyyy");
          if (!dsThongTin.Tables[0].Rows[0]["NGAY_HHAN"].ToString().IsNullOrEmptyOrSpace())
              teldtNgayHetHieuLuc.Text = LDateTime.StringToDate(dsThongTin.Tables[0].Rows[0]["NGAY_HHAN"].ToString(), "yyyyMMdd").ToString("dd/MM/yyyy");
          cmbPhuongThucTinh.SelectedIndex = lstPhuongThucTinhLai.IndexOf(lstPhuongThucTinhLai.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dsThongTin.Tables[0].Rows[0]["TLAI_PTHUC"])));
          cmbCSTinhLai.SelectedIndex = lstCoSoTinhLai.IndexOf(lstCoSoTinhLai.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dsThongTin.Tables[0].Rows[0]["MA_CSO_TLAI"])));
          string LoaiKyQuy = dsThongTin.Tables[0].Rows[0]["LOAI_KQUY"].ToString();
          if (LoaiKyQuy.Equals(BusinessConstant.PPHAP_TINH_RGOC.TUYETDOI.layGiaTri()))
          {
              rbTuyetdoi.Checked= true;
              if (dsThongTin.Tables[0].Rows[0]["SO_TIEN_KQUY"] != DBNull.Value)
                  numSoTienKyQuy.Text = Convert.ToDouble(dsThongTin.Tables[0].Rows[0]["SO_TIEN_KQUY"]).ToString("#,##0");
          }
          else
          {
              rbTuongdoi.Checked = true;
              if (dsThongTin.Tables[0].Rows[0]["SO_TIEN_KQUY"] != DBNull.Value)
                  numTyLeKyQuy.Text = Convert.ToDouble(dsThongTin.Tables[0].Rows[0]["SO_TIEN_KQUY"]).ToString();
          }
          cmbHinhThucNop.SelectedIndex = lstHinhThucTra.IndexOf(lstHinhThucTra.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dsThongTin.Tables[0].Rows[0]["HINH_THUC_NOP_KQUY"])));
          cmbPPTinh.SelectedIndex = lstPThucTinhKQuy.IndexOf(lstPThucTinhKQuy.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dsThongTin.Tables[0].Rows[0]["PP_TINH_KQUY"])));
          if (dsThongTin.Tables[0].Rows[0]["SO_TIEN_GOP"] != DBNull.Value)
              numSoTienGop.Text = Convert.ToDouble(dsThongTin.Tables[0].Rows[0]["SO_TIEN_GOP"]).ToString();
          idSanPham = Convert.ToInt32(dsThongTin.Tables[0].Rows[0]["ID"]);
          if (dsThongTin.Tables[0].Rows[0]["ID_LSUAT"] != DBNull.Value)
              idLaiSuat = Convert.ToInt32(dsThongTin.Tables[0].Rows[0]["ID_LSUAT"]);
          if (dsThongTin.Tables[0].Rows[0]["ID_CS_TLAI"] != DBNull.Value)
              idCoSoTinhLai = Convert.ToInt32(dsThongTin.Tables[0].Rows[0]["ID_CS_TLAI"]);
          obj.SAN_PHAM_TD.MA_DVI_QLY = dsThongTin.Tables[0].Rows[0]["MA_DVI_QLY"].ToString();
          obj.SAN_PHAM_TD.MA_DVI_TAO = dsThongTin.Tables[0].Rows[0]["MA_DVI_TAO"].ToString();
          txtBienDo.Text = dsThongTin.Tables[0].Rows[0]["LSUAT_BIEN_DO"].ToString();
          LayThongTinLSuat();
      }

      /// <summary>
      /// Đưa dữ liệu vào các điều khiển tab kiểm soát
      /// </summary>
      /// <param name="dsThongTin"></param>
      private void LoadDuLieuKiemSoat(DataSet dsThongTin)
      {
          txtTrangThai.Text = dsThongTin.Tables[0].Rows[0]["TEN_TTHAI_BGHI"].ToString();
          teldtNgayNhap.Text = LDateTime.StringToDate(dsThongTin.Tables[0].Rows[0]["NGAY_NHAP"].ToString(), "yyyyMMdd").ToString("dd/MM/yyyy");
          txtNguoiLap.Text = dsThongTin.Tables[0].Rows[0]["NGUOI_NHAP"].ToString();
          if (dsThongTin.Tables[0].Rows[0]["NGAY_CNHAT"].ToString().Length >= 8)
              teldtNgayCNhat.Text = LDateTime.StringToDate(dsThongTin.Tables[0].Rows[0]["NGAY_CNHAT"].ToString(), "yyyyMMdd").ToString("dd/MM/yyyy");
          else
              teldtNgayCNhat.Text = "";
          txtNguoiCapNhat.Text = dsThongTin.Tables[0].Rows[0]["NGUOI_CNHAT"].ToString();
      }

      
      #endregion

       #region Nghiepvu
      private void TinhToanBangKeGocLai()
      {
          try
          {
              lstVongVay = new List<VONG_VAY_CTIET>();
              DataSet dsVongVay = new TinDungProcess().getVongVonVayByID(idVongVay.ToString());
              bool bCheck = false;
              if (!LObject.IsNullOrEmpty(dsVongVay) && dsVongVay.Tables.Count > 0)
              {
                  foreach (DataRow dr in dsVongVay.Tables[0].Rows)
                  {
                      VONG_VAY_CTIET objVongVay = new VONG_VAY_CTIET();
                      objVongVay.ID = Convert.ToInt32(dr["ID"]);
                      objVongVay.ID_VONG_VAY = Convert.ToInt32(dr["ID_VONG_VAY"]);
                      objVongVay.KY_HAN = Convert.ToInt32(dr["KY_HAN"]);
                      telThoiGianVay.Text = Convert.ToDouble(dr["KY_HAN"]).ToString();
                      objVongVay.KY_HAN_DVI_TINH = dr["KY_HAN_DVI_TINH"].ToString();
                      objVongVay.MA_VONG_VAY = dr["MA_VONG_VAY"].ToString();
                      objVongVay.SO_THU_TU = dr["SO_THU_TU"].ToString();
                      objVongVay.SO_TIEN = Convert.ToDecimal(dr["SO_TIEN"]);
                      objVongVay.SO_TIEN_GOC = Convert.ToDecimal(dr["SO_TIEN_GOC"]);
                      objVongVay.SO_TIEN_GOC_MOI_KY = Convert.ToDecimal(dr["SO_TIEN_GOC_MOI_KY"]);
                      objVongVay.SO_TIEN_LAI = Convert.ToDecimal(dr["SO_TIEN_LAI"]);
                      objVongVay.SO_TIEN_LAI_MOI_KY = Convert.ToDecimal(dr["SO_TIEN_LAI_MOI_KY"]);
                      objVongVay.SO_TIEN_MOI_KY = Convert.ToDecimal(dr["SO_TIEN_MOI_KY"]);
                      objVongVay.SO_TIEN_TKBB = Convert.ToDecimal(dr["SO_TIEN_TKBB"]);
                      objVongVay.TOAN_TU = dr["TOAN_TU"].ToString();
                      if (!bCheck)
                      {
                          cmbHanMucGocVay.SelectedIndex = lstHanMucGocVay.IndexOf(lstHanMucGocVay.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["TCHAT_GOC_VAY"].ToString())));
                          cmbHanMucKHan.SelectedIndex = lstHanMucGocVay.IndexOf(lstHanMucGocVay.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["TCHAT_KY_HAN"].ToString())));
                          bCheck = true;
                      }
                      lstVongVay.Add(objVongVay);
                  }
              }
              raddgrGocLaiVayDS.DataSource = lstVongVay;
              raddgrGocLaiVayDS.DataBind();
          }
          catch
          {

          }
      }
      private void LoadDuLieuTaiKhoanHachToan(string sDoiTuong)
      {
          DataTable dt = new TinDungProcess().GetTaiKhoanHachToan(sDoiTuong, AppConfig.LoginedUser.MaDonVi).Tables["TAI_KHOAN_HACH_TOAN"];
          grdTKhoan.DataSource = dt.DefaultView;
          grdTKhoan.DataBind();
      }
     // <summary>
        /// Lấy thông tin lãi suất
        /// </summary>
      private void LayThongTinLSuat()
      {
          TinDungProcess tindungProcess = new TinDungProcess();
          dsLaiSuat = tindungProcess.getLaiSuatByID(idLaiSuat.ToString());
          if (dsLaiSuat != null & dsLaiSuat.Tables[0].Rows.Count > 0)
          {

              maLaiSuat = txtMaLaiSuat.Text = dsLaiSuat.Tables[0].Rows[0]["MA_LSUAT"].ToString();
              lblTenLSuat.Text = dsLaiSuat.Tables[0].Rows[0]["MO_TA"].ToString();
              dLaiSuat = Convert.ToDecimal(dsLaiSuat.Tables[0].Rows[0]["LAI_SUAT"]);
              double biendo=0;
              if (txtBienDo.Text!="")
              try{
                  biendo=double.Parse(txtBienDo.Text);
              }catch{}
              if (dsLaiSuat.Tables[0].Rows[0]["PPHAP_TINH_LSUAT"].Equals("DTH"))
                  txtLaiSuat.Text = (Convert.ToDouble(dLaiSuat) + biendo).ToString();
              else
                  txtLaiSuat.Text = "";
              TinhToanBangKeGocLai();
              txtBienDo.Attributes.Add("onchange", "changebiendo(this," + dLaiSuat.ToString() + ")");
          }
          else
          {
              txtLaiSuat.Text = "";
              lblTenLSuat.Text = "";
              dLaiSuat = 0;
              txtLaiSuat.Text = "";
              //titemsGocLaiVay.Visibility = Visibility.Collapsed;
          }
      }

      /// <summary>
      /// Xóa dữ liệu
      /// </summary>
      private List<string> Xoa()
      {
          //if (tlbDelete.IsEnabled == false)
          //    return;
          List<string> listResult = new List<string>();
          if (idSanPham > 0)
          {
              try
              {                  
                      TinDungProcess tindungProcess = new TinDungProcess();
                      List<ClientResponseDetail> ClientResponseDetail = new List<Presentation.Process.Common.ClientResponseDetail>();
                      // Yêu cầu lock dữ liệu
                      List<int> lstId = new List<int>();
                      lstId.Add(idSanPham);
                      UtilitiesProcess process = new UtilitiesProcess();
                      if (process.LockData(DatabaseConstant.Module.TDVM,
                      DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                      DatabaseConstant.Table.TD_SAN_PHAM,
                      DatabaseConstant.Action.XOA,
                      lstId))
                      {
                          int iResult = 0;
                          List<KT_PHAN_HE_PLOAI> lstPhanHe = new List<KT_PHAN_HE_PLOAI>();
                          obj.DSACH_SAN_PHAM_TD = new TD_SAN_PHAM[1] { obj.SAN_PHAM_TD };
                          iResult = new TinDungProcess().SanPhamTinDung(DatabaseConstant.Action.XOA, ref obj, ref lstPhanHe, ref ClientResponseDetail);
                          if (iResult > 0)
                          {                              
                              foreach (ClientResponseDetail cl in ClientResponseDetail)
                              {
                                  listResult.Add(LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Result) + "#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]));
                              } 
                              // Yeu cau Unlook du lieu
                              process.UnlockData(DatabaseConstant.Module.TDVM,
                          DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                          DatabaseConstant.Table.TD_SAN_PHAM,
                          DatabaseConstant.Action.XOA,
                          lstId);

                          }
                          else
                              listResult.Add("M.DungChung.XoaKhongThanhCong#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.XoaKhongThanhCong"));
                      }
                      else
                          listResult.Add("M.ResponseMessage.Common.LockDataInvalid#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid"));
              }
              catch (Exception ex)
              {
                  listResult.Add("M.DungChung.LoiChung#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.LoiChung"));
                 
                  LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
              }
          }
          return listResult;
      }
      private void TinhSoTienGopHangKy()
      {
          decimal ThoiGianVay = decimal.Parse(telThoiGianVay.Text);
          decimal SoTienGop = decimal.Parse(numSoTienGop.Text);
          decimal SoTienGoc = 0;
          decimal SoTienLai = 0;
          int iret = new TinDungProcess().BaremTinhLaiTienVay(SoTienGop, ThoiGianVay, out SoTienGoc, out SoTienLai, ClientInformation.MaDonVi);
          if (iret > 0)
          {
              numSoTienGoc.Text = Convert.ToDouble(SoTienGoc).ToString();
              numSoTienLai.Text = Convert.ToDouble(SoTienLai).ToString();
          }
          else
          {
              numSoTienGoc.Text = "0";
              numSoTienLai.Text = "0";
          }
      }
      /// <summary>
      /// Duyệt chi tiết
      /// </summary>
      private List<string> Duyet()
      {
          List<string> listResult = new List<string>();
          try
          {
              if (idSanPham > 0 & sTrangThai.Equals(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri()))
              {
                  List<ClientResponseDetail> ClientResponseDetail = new List<ClientResponseDetail>();
                  
                      List<int> lstID = new List<int>();
                      lstID.Add(idSanPham);
                      UtilitiesProcess process = new UtilitiesProcess();
                      if (process.LockData(DatabaseConstant.Module.TDVM,
                      DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                      DatabaseConstant.Table.TD_SAN_PHAM,
                      DatabaseConstant.Action.DUYET,
                      lstID))
                      {
                          int iResult = 0;
                          List<KT_PHAN_HE_PLOAI> lstPhanHe = new List<KT_PHAN_HE_PLOAI>();
                          obj.DSACH_SAN_PHAM_TD = new TD_SAN_PHAM[1] { obj.SAN_PHAM_TD };
                          iResult = new TinDungProcess().SanPhamTinDung(DatabaseConstant.Action.DUYET, ref obj, ref lstPhanHe, ref ClientResponseDetail);
                          if (iResult > 0)
                          {
                              foreach (ClientResponseDetail cl in ClientResponseDetail)
                              {
                                  listResult.Add(LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Result) + "#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]));
                              }
                              // Yeu cau Unlook du lieu
                         process.UnlockData(DatabaseConstant.Module.TDVM,
                          DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                          DatabaseConstant.Table.TD_SAN_PHAM,
                          DatabaseConstant.Action.DUYET,
                          lstID);

                              LoadDuLieuCT(false);
                          }
                          else
                          {
                              listResult.Add("M.DungChung.DuyetKhongThanhCong#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.DuyetKhongThanhCong"));
                          }
                      }
                      else
                      {
                          listResult.Add("M.ResponseMessage.Common.LockDataInvalid#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid"));
                      }
                  
              }
          }
          catch (Exception ex)
          {
              listResult.Add("M.DungChung.LoiChung#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.LoiChung"));              
              LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
          }
          return listResult;
      }

      /// <summary>
      /// Thoái duyệt
      /// </summary>
      private List<string> ThoaiDuyet()
      {
          List<string> listResult = new List<string>();
          try
          {
              if (idSanPham > 0 & sTrangThai.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
              {
                  
                      List<ClientResponseDetail> ClientResponseDetail = new List<ClientResponseDetail>();
                      List<int> lstID = new List<int>();
                      lstID.Add(idSanPham);
                      UtilitiesProcess process = new UtilitiesProcess();
                      if (process.LockData(DatabaseConstant.Module.TDVM,
                      DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                      DatabaseConstant.Table.TD_SAN_PHAM,
                      DatabaseConstant.Action.THOAI_DUYET,
                      lstID))
                      {
                          int iResult = 0;
                          List<KT_PHAN_HE_PLOAI> lstPhanHe = new List<KT_PHAN_HE_PLOAI>();
                          obj.DSACH_SAN_PHAM_TD = new TD_SAN_PHAM[1] { obj.SAN_PHAM_TD };
                          iResult = new TinDungProcess().SanPhamTinDung(DatabaseConstant.Action.THOAI_DUYET, ref obj, ref lstPhanHe, ref ClientResponseDetail);
                          if (iResult > 0)
                          {
                              foreach (ClientResponseDetail cl in ClientResponseDetail)
                              {
                                  listResult.Add(LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Result) + "#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]));
                              }
                              // Yeu cau Unlook du lieu
                              process.UnlockData(DatabaseConstant.Module.TDVM,
                          DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                          DatabaseConstant.Table.TD_SAN_PHAM,
                          DatabaseConstant.Action.THOAI_DUYET,
                          lstID);

                              LoadDuLieuCT(false);
                          }
                          else
                          {
                              listResult.Add("M.DungChung.ThoaiDuyetKhongThanhCong#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.ThoaiDuyetKhongThanhCong"));

                          }
                      }
                      else
                      {
                          listResult.Add("M.ResponseMessage.Common.LockDataInvalid#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid"));
                      }
              }
          }
          catch (Exception ex)
          {
              listResult.Add("M.DungChung.LoiChung#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.LoiChung")); 
              LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
          }
          return listResult;
      }

      /// <summary>
      /// Từ chối duyệt
      /// </summary>
      private List<string> TuChoiDuyet()
      {
          List<string> listResult = new List<string>();
          try
          {
              if (idSanPham > 0 & sTrangThai.Equals(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri()))
              {
                      List<ClientResponseDetail> ClientResponseDetail = new List<ClientResponseDetail>();
                      List<int> lstID = new List<int>();
                      lstID.Add(idSanPham);
                      UtilitiesProcess process = new UtilitiesProcess();
                      if (process.LockData(DatabaseConstant.Module.TDVM,
                      DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                      DatabaseConstant.Table.TD_SAN_PHAM,
                      DatabaseConstant.Action.TU_CHOI_DUYET,
                      lstID))
                      {
                          int iResult = 0;
                          List<KT_PHAN_HE_PLOAI> lstPhanHe = new List<KT_PHAN_HE_PLOAI>();
                          obj.DSACH_SAN_PHAM_TD = new TD_SAN_PHAM[1] { obj.SAN_PHAM_TD };
                          iResult = new TinDungProcess().SanPhamTinDung(DatabaseConstant.Action.TU_CHOI_DUYET, ref obj, ref lstPhanHe, ref ClientResponseDetail);
                          if (iResult > 0)
                          {
                              foreach (ClientResponseDetail cl in ClientResponseDetail)
                              {
                                  listResult.Add(LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Result) + "#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]));
                              }
                              // Yeu cau Unlook du lieu
                              process.UnlockData(DatabaseConstant.Module.TDVM,
                          DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                          DatabaseConstant.Table.TD_SAN_PHAM,
                          DatabaseConstant.Action.TU_CHOI_DUYET,
                          lstID);

                              LoadDuLieuCT(false);
                          }
                          else
                          {
                              listResult.Add("M.DungChung.TuChoiDuyetKhongThanhCong#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.TuChoiDuyetKhongThanhCong"));
                          }
                      }
                      else
                      {
                          listResult.Add("M.ResponseMessage.Common.LockDataInvalid#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid"));
                      }
              }
          }
          catch (Exception ex)
          {
              listResult.Add("M.DungChung.LoiChung#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.LoiChung")); 
              LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
          }
          return listResult;
      }

      private List<string> LuuDuLieu(BusinessConstant.TrangThaiBanGhi banghi, BusinessConstant.TrangThaiNghiepVu nghiepvu, DatabaseConstant.Action action)
      {
          List<string> listResult = new List<string>();
          if (action != DatabaseConstant.Action.LUU_TAM)
          {
              int iResult = 0;
              TinDungProcess tindungProcess = new TinDungProcess();
              List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
              try
              {
                  // Yêu cầu lock dữ liệu
                  List<int> lstId = new List<int>();
                  lstId.Add(idSanPham);
                  UtilitiesProcess process = new UtilitiesProcess();
                  if (process.LockData(DatabaseConstant.Module.TDVM,
                  DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                  DatabaseConstant.Table.TD_SAN_PHAM,
                  DatabaseConstant.Action.SUA,
                  lstId))
                  {
                      List<KT_PHAN_HE_PLOAI> lstPhanHe = new List<KT_PHAN_HE_PLOAI>();
                      LayDuLieu(ref lstPhanHe, banghi, nghiepvu);
                      if (idSanPham == 0)
                          iResult = tindungProcess.SanPhamTinDung(DatabaseConstant.Action.THEM, ref obj, ref lstPhanHe, ref lstResponseDetail);
                      else
                          iResult = tindungProcess.SanPhamTinDung(DatabaseConstant.Action.SUA, ref obj, ref lstPhanHe, ref lstResponseDetail);
                      if (iResult > 0)
                      {

                          listResult.Add("M.DungChung.LuuDuLieuThanhCong#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.LuuDuLieuThanhCong"));
                          // Yêu cầu Unlock dữ liệu
                          process.UnlockData(DatabaseConstant.Module.TDVM,
                          DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                          DatabaseConstant.Table.TD_SAN_PHAM,
                          DatabaseConstant.Action.SUA,
                          lstId);
                          idSanPham = iResult;
                          SetDataForm();
                          if (cbThemnhieulan.Checked == true)
                          {
                              ClearForm();
                              idSanPham = 0;
                          }
                      }
                      else
                      {
                          foreach (ClientResponseDetail cl in lstResponseDetail)
                          {
                              listResult.Add(LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]) + "#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]));
                          }
                      }
                  }
                  else
                  {
                      listResult.Add("M.ResponseMessage.Common.LockDataInvalid#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid"));
                  }
              }
              catch (Exception ex)
              {
                  listResult.Add("M.DungChung.LoiLuuDuLieu#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.LoiLuuDuLieu")); 
                  
                  LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
              }
          }
          return listResult;
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="objSP"></param>
      /// <param name="bangghi"></param>
      /// <param name="nghiepvu"></param>
      void LayDuLieu(ref List<KT_PHAN_HE_PLOAI> lstPhanHe, BusinessConstant.TrangThaiBanGhi bangghi, BusinessConstant.TrangThaiNghiepVu nghiepvu)
      {
          string sLoaiKyQuy = BusinessConstant.PPHAP_TINH_RGOC.TUYETDOI.layGiaTri();
          decimal SoTienKyQuy = 0;
          string sHinhThucNop = "NOP_1LAN";
          string sPPhapTinh = "TONG_DNO_KUOC";
          string sKyHanTinh = "THANG";
          string sTChatGocVay = "CO_DINH";
          string sTChatKyHan = "CO_DINH";
          if (LObject.IsNullOrEmpty(obj.SAN_PHAM_TD)) obj.SAN_PHAM_TD = new TD_SAN_PHAM();
          if (rbTuyetdoi.Checked)
          {
              sLoaiKyQuy = BusinessConstant.PPHAP_TINH_RGOC.TUYETDOI.layGiaTri();
              SoTienKyQuy = decimal.Parse(numSoTienKyQuy.Text);
          }
          if (rbTuongdoi.Checked)
          {
              sLoaiKyQuy = BusinessConstant.PPHAP_TINH_RGOC.TUONGDOI.layGiaTri();
              SoTienKyQuy = decimal.Parse(numTyLeKyQuy.Text);
          }
          if (cmbHinhThucNop.SelectedIndex > -1)
          {
              //AutoCompleteEntry auHinhThucNop = lstHinhThucTra.ElementAt(cmbHinhThucNop.SelectedIndex);
              //if (!LObject.IsNullOrEmpty(auHinhThucNop))
              sHinhThucNop = cmbHinhThucNop.SelectedItem.Value;// auHinhThucNop.KeywordStrings[0];
          }
          if (cmbPPTinh.SelectedIndex > -1)
          {
              //AutoCompleteEntry auPPhapTinh = lstPThucTinhKQuy.ElementAt(cmbPPTinh.SelectedIndex);
              //if (!LObject.IsNullOrEmpty(auPPhapTinh))
              sPPhapTinh = cmbPPTinh.SelectedItem.Value;
          }
          if (cmbThoiHanVay.SelectedIndex > -1)
          {
              //AutoCompleteEntry auThoiHanVay = lstThoiHanVay.ElementAt(cmbThoiHanVay.SelectedIndex);
              //if (!LObject.IsNullOrEmpty(auThoiHanVay))
              sKyHanTinh = cmbThoiHanVay.SelectedItem.Value;
          }
          if (cmbHanMucGocVay.SelectedIndex > -1)
          {
              //AutoCompleteEntry auHanMucGocVay = lstHanMucGocVay.ElementAt(cmbHanMucGocVay.SelectedIndex);
              //if (!LObject.IsNullOrEmpty(auHanMucGocVay))
                  sTChatGocVay = cmbHanMucGocVay.SelectedItem.Value;
          }
          if (cmbHanMucKHan.SelectedIndex > -1)
          {
              //AutoCompleteEntry auHanMucKHan = lstHanMucGocVay.ElementAt(cmbHanMucKHan.SelectedIndex);
              //if (!LObject.IsNullOrEmpty(auHanMucKHan))
                  sTChatKyHan = cmbHanMucKHan.SelectedItem.Value;
          }
          obj.SAN_PHAM_TD.ID_CS_TLAI = int.Parse(cmbCSTinhLai.SelectedItem.Value.Split('|')[1]);// idCoSoTinhLai;
          obj.SAN_PHAM_TD.ID_LSUAT = 12;// idLaiSuat; fix tam cho popup search
          obj.SAN_PHAM_TD.MA_NHOM_SP = "";
          obj.SAN_PHAM_TD.NGAY_ADUNG = teldtNgayHieuLuc.Text != "" ? LDateTime.DateToString(LDateTime.StringToDate(teldtNgayHieuLuc.Text, "dd/MM/yyyy"), ApplicationConstant.defaultDateTimeFormat) : "";
          obj.SAN_PHAM_TD.LOAI_SAN_PHAM = cmbLoaiSanPham.SelectedItem.Value;// maLoaiSanPham;
          obj.SAN_PHAM_TD.NGAY_HHAN = teldtNgayHetHieuLuc.Text != "" ? LDateTime.DateToString(LDateTime.StringToDate(teldtNgayHetHieuLuc.Text,"dd/MM/yyyy"), ApplicationConstant.defaultDateTimeFormat) : "";
          obj.SAN_PHAM_TD.TEN_SAN_PHAM = txtTenSanPham.Text;
          obj.SAN_PHAM_TD.MA_VONG_VAY = cmbNhomVongVay.SelectedItem.Value.Split('|')[0];// maVongVay;
          obj.SAN_PHAM_TD.MA_LOAI_TIEN = "";
          obj.SAN_PHAM_TD.PTHUC_VAY = cmbHinhThucVay.SelectedItem.Value;// maHinhThucChoVay;
          obj.SAN_PHAM_TD.LOAI_THAN_VAY = "";
          obj.SAN_PHAM_TD.NGUON_VAY =  maNguonVon;
          obj.SAN_PHAM_TD.MUC_DICH_VAY = cmbMucDichVayVon.SelectedItem.Value;// maMucDichSuDung;
          obj.SAN_PHAM_TD.MA_LSUAT = "LS0012";//--maLaiSuat; fix tam cho popup search
          obj.SAN_PHAM_TD.MA_CSO_TLAI = cmbCSTinhLai.SelectedItem.Value.Split('|')[0];// maCoSoTLai;
          obj.SAN_PHAM_TD.LSUAT_BIEN_DO = txtBienDo.Text != "" ? Convert.ToDecimal(double.Parse(txtBienDo.Text)) : 0;
          obj.SAN_PHAM_TD.TLAI_DVI_TINH = "";
          obj.SAN_PHAM_TD.TLAI_PTHUC = cmbPhuongThucTinh.SelectedItem.Value;// maPhuongThucTLai;
          try
          {
              maTanSuatDanhGia = cmbTanSuatDanhGia.SelectedItem.Value;
          }
          catch { }
          obj.SAN_PHAM_TD.DKY_TDOI_LSUAT =  maTanSuatDanhGia;//cmbTanSuatDanhGia.SelectedItem.Value;//
          obj.SAN_PHAM_TD.LOAI_LSUAT = maLoaiLSuat;
          obj.SAN_PHAM_TD.DGIA_SO_DKY = null;
          obj.SAN_PHAM_TD.DGIA_DVI_TINH = null;
          obj.SAN_PHAM_TD.TLE_LPHAT = null;
          obj.SAN_PHAM_TD.CTHI_QHAN = null;
          obj.SAN_PHAM_TD.CTHI_DTHU = null;
          obj.SAN_PHAM_TD.HAN_MUC = null;
          obj.SAN_PHAM_TD.MA_HMUC = null;
          obj.SAN_PHAM_TD.NTAC_LTRON = null;
          obj.SAN_PHAM_TD.LOAI_KQUY = sLoaiKyQuy;
          obj.SAN_PHAM_TD.SO_TIEN_KQUY = SoTienKyQuy;
          obj.SAN_PHAM_TD.HINH_THUC_NOP_KQUY = sHinhThucNop;
          obj.SAN_PHAM_TD.PP_TINH_KQUY = sPPhapTinh;
          obj.SAN_PHAM_TD.SO_TIEN_GOP = numSoTienGop.Text != "" ? Convert.ToDecimal(double.Parse(numSoTienGop.Text)) : 0;
          obj.SAN_PHAM_TD.TTHAI_BGHI = BusinessConstant.layGiaTri(bangghi);
          obj.SAN_PHAM_TD.TLE_HTRA_GOC = numTyLeHoanTraGoc.Text != "" ? Convert.ToDecimal(double.Parse(numTyLeHoanTraGoc.Text)) : 0;
          if (!sTrangThai.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
              obj.SAN_PHAM_TD.TTHAI_NVU = BusinessConstant.layGiaTri(nghiepvu);
          else
              obj.SAN_PHAM_TD.TTHAI_NVU = sTrangThai;
          obj.SAN_PHAM_TD.NGUOI_NHAP = AppConfig.LoginedUser.UserName;
          obj.SAN_PHAM_TD.NGAY_NHAP = AppConfig.LoginedUser.NgayLamViecHienTai;
          obj.SAN_PHAM_TD.MA_DVI_QLY = AppConfig.LoginedUser.MaDonVi;
          obj.SAN_PHAM_TD.MA_DVI_TAO = AppConfig.LoginedUser.MaDonViGiaoDich;
          if (idSanPham > 0)
          {
              obj.SAN_PHAM_TD.MA_SAN_PHAM = txtMaSanPham.Text;
              obj.SAN_PHAM_TD.ID = idSanPham;
              obj.SAN_PHAM_TD.NGUOI_NHAP = txtNguoiLap.Text;
              obj.SAN_PHAM_TD.NGAY_NHAP = LDateTime.DateToString(LDateTime.StringToDate(teldtNgayNhap.Text,"dd/MM/yyyy"), ApplicationConstant.defaultDateTimeFormat);
              obj.SAN_PHAM_TD.NGUOI_CNHAT = AppConfig.LoginedUser.UserName;
              obj.SAN_PHAM_TD.NGAY_CNHAT = AppConfig.LoginedUser.NgayLamViecHienTai;
          }
          if (LObject.IsNullOrEmpty(obj.VONG_VAY)) obj.VONG_VAY = new VONG_VAY();
          obj.VONG_VAY.ID = int.Parse(cmbNhomVongVay.SelectedItem.Value.Split('|')[1]);
          obj.VONG_VAY.KY_HAN_DVI_TINH = sKyHanTinh;
          obj.VONG_VAY.MA_VONG_VAY = cmbNhomVongVay.SelectedItem.Value.Split('|')[0];// maVongVay;
          obj.VONG_VAY.TCHAT_GOC_VAY = sTChatGocVay;
          obj.VONG_VAY.TCHAT_KY_HAN = sTChatKyHan;
          if (cmbNhomVongVay.SelectedItem.Text=="")
              obj.VONG_VAY.TEN_VONG_VAY = "Vòng vay tín dụng " + txtTenSanPham.Text;
          obj.VONG_VAY.TOAN_TU = ">=";
          obj.VONG_VAY.NGAY_HIEU_LUC = teldtNgayHieuLuc.Text != "" ? LDateTime.DateToString(LDateTime.StringToDate(teldtNgayHieuLuc.Text, "dd/MM/yyyy"), ApplicationConstant.defaultDateTimeFormat) : "";
          obj.VONG_VAY.NGAY_HET_HLUC = teldtNgayHetHieuLuc.Text != "" ? LDateTime.DateToString(LDateTime.StringToDate(teldtNgayHetHieuLuc.Text, "dd/MM/yyyy"), ApplicationConstant.defaultDateTimeFormat) : "";
         
          // fix tam trong thoi gian cho edit gtrid
          lstVongVay = new List<VONG_VAY_CTIET>();
          DataSet dsVongVay = new TinDungProcess().getVongVonVayByID(cmbNhomVongVay.SelectedItem.Value.Split('|')[1]);
         
          if (!LObject.IsNullOrEmpty(dsVongVay) && dsVongVay.Tables.Count > 0)
          {
              foreach (DataRow dr in dsVongVay.Tables[0].Rows)
              {
                  VONG_VAY_CTIET objVongVay = new VONG_VAY_CTIET();
                  objVongVay.ID = Convert.ToInt32(dr["ID"]);
                  objVongVay.ID_VONG_VAY = Convert.ToInt32(dr["ID_VONG_VAY"]);
                  objVongVay.KY_HAN = Convert.ToInt32(dr["KY_HAN"]);
                  telThoiGianVay.Text = Convert.ToDouble(dr["KY_HAN"]).ToString();
                  objVongVay.KY_HAN_DVI_TINH = dr["KY_HAN_DVI_TINH"].ToString();
                  objVongVay.MA_VONG_VAY = dr["MA_VONG_VAY"].ToString();
                  objVongVay.SO_THU_TU = dr["SO_THU_TU"].ToString();
                  objVongVay.SO_TIEN = Convert.ToDecimal(dr["SO_TIEN"]);
                  objVongVay.SO_TIEN_GOC = Convert.ToDecimal(dr["SO_TIEN_GOC"]);
                  objVongVay.SO_TIEN_GOC_MOI_KY = Convert.ToDecimal(dr["SO_TIEN_GOC_MOI_KY"]);
                  objVongVay.SO_TIEN_LAI = Convert.ToDecimal(dr["SO_TIEN_LAI"]);
                  objVongVay.SO_TIEN_LAI_MOI_KY = Convert.ToDecimal(dr["SO_TIEN_LAI_MOI_KY"]);
                  objVongVay.SO_TIEN_MOI_KY = Convert.ToDecimal(dr["SO_TIEN_MOI_KY"]);
                  objVongVay.SO_TIEN_TKBB = Convert.ToDecimal(dr["SO_TIEN_TKBB"]);
                  objVongVay.TOAN_TU = dr["TOAN_TU"].ToString();                
                  lstVongVay.Add(objVongVay);
              }
          }
          ///
          obj.VONG_VAY.DSACH_VONG_VAY = lstVongVay.ToArray();
          //DataView dv = (DataView)grdTKhoan.DataSource;
          DataView dv= new TinDungProcess().GetTaiKhoanHachToan("MACDINH", AppConfig.LoginedUser.MaDonVi).Tables["TAI_KHOAN_HACH_TOAN"].DefaultView;
          foreach (DataRowView drv in dv)
          {
              KT_PHAN_HE_PLOAI objPhanHePLoai = new KT_PHAN_HE_PLOAI();
              objPhanHePLoai.ID_PHAN_HE = 0;
              objPhanHePLoai.ID = Convert.ToInt32(drv["ID"]);
              objPhanHePLoai.MA_DTUONG = txtMaSanPham.Text;
              objPhanHePLoai.MA_PHAN_HE = DatabaseConstant.Module.TDVM.getValue();
              objPhanHePLoai.MA_KY_HIEU = drv["MA_KY_HIEU"].ToString();
              objPhanHePLoai.MA_PLOAI = drv["MA_PLOAI"].ToString();
              objPhanHePLoai.MA_PLOAI_BSO = drv["MA_PLOAI_BSO"].ToString();
              objPhanHePLoai.TTHAI_BGHI = BusinessConstant.layGiaTri(bangghi);
              objPhanHePLoai.TTHAI_NVU = BusinessConstant.layGiaTri(nghiepvu);
              objPhanHePLoai.MA_DVI_QLY = AppConfig.LoginedUser.MaDonVi;
              objPhanHePLoai.MA_DVI_TAO = AppConfig.LoginedUser.MaDonViGiaoDich;
              objPhanHePLoai.NGUOI_NHAP = AppConfig.LoginedUser.UserName;
              objPhanHePLoai.NGAY_NHAP = AppConfig.LoginedUser.NgayLamViecHienTai;
              lstPhanHe.Add(objPhanHePLoai);
          }
      }

      /// <summary>
      /// Sửa
      /// </summary>
      private void Sua()
      {
          List<string> listResult = new List<string>();
          if (sTrangThai.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
          {
              grdTKhoan.Enabled = false;
              grbThongTinChung.Enabled = true;
              foreach (Control ct in grbThongTinChung.Controls)
              {
                  if (ct.ID == "txtTenSanPham")
                      ct.Visible = true;
                  else
                      ct.Visible = false;
              }             
          }
          else
          {
              List<Control> lstConTrol = new List<Control>();
              lstConTrol.Add((Control)grbGocLaiVay);
              lstConTrol.Add((Control)grbThongTinChung);
              lstConTrol.Add((Control)grbLaiSuat);
              lstConTrol.Add((Control)grbKyQuy);
              //lstConTrol.Add((Control)grbHinhThuc);
              LockControl(lstConTrol, true);
          }
         // CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, sTrangThai, mnuMain, DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM);
      }

      private void SetDataForm()
      {
          sTrangThai = obj.SAN_PHAM_TD.TTHAI_NVU;
          txtMaSanPham.Text = obj.SAN_PHAM_TD.MA_SAN_PHAM;
          idSanPham = obj.SAN_PHAM_TD.ID;
          idVongVay = obj.VONG_VAY.ID;
          maVongVay = obj.SAN_PHAM_TD.MA_VONG_VAY;
          lblTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(sTrangThai);
          txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(obj.SAN_PHAM_TD.TTHAI_BGHI);
          teldtNgayNhap.Text = LDateTime.StringToDate(obj.SAN_PHAM_TD.NGAY_NHAP, "yyyyMMdd").ToString("dd/MM/yyyy");
          if (!obj.SAN_PHAM_TD.TLE_HTRA_GOC.IsNullOrEmpty())
              numTyLeHoanTraGoc.Text = LNumber.ToDouble(obj.SAN_PHAM_TD.TLE_HTRA_GOC.Value).ToString("#,##0");
          txtNguoiLap.Text = obj.SAN_PHAM_TD.NGUOI_NHAP;
          if (!LObject.IsNullOrEmpty(obj.SAN_PHAM_TD.NGAY_CNHAT) && obj.SAN_PHAM_TD.NGAY_CNHAT.Length >= 8)
              teldtNgayCNhat.Text = (LDateTime.StringToDate(obj.SAN_PHAM_TD.NGAY_CNHAT, "yyyyMMdd")).ToString("dd/MM/yyyy");
          else
              teldtNgayCNhat.Text = "";
          txtNguoiCapNhat.Text = obj.SAN_PHAM_TD.NGUOI_CNHAT;
          if (obj.SAN_PHAM_TD.TLE_HTRA_GOC.IsNullOrEmpty())
              numTyLeHoanTraGoc.Text = ((double)obj.SAN_PHAM_TD.TLE_HTRA_GOC).ToString("#,##0.00");
          if (obj.SAN_PHAM_TD.SO_TIEN_GOP.IsNullOrEmpty())
              radNumSoTienTuongTro.Text = ((double)obj.SAN_PHAM_TD.SO_TIEN_GOP).ToString("#,##0.00");
          List<Control> lstConTrol = new List<Control>();
          lstConTrol.Add((Control)grbGocLaiVay);
          lstConTrol.Add((Control)grbThongTinChung);
          lstConTrol.Add((Control)grbLaiSuat);
          lstConTrol.Add((Control)grbKyQuy);
          //lstConTrol.Add((Control)grbHinhThuc);
          LockControl(lstConTrol, false);
          lstNhomVongVay.Clear();
          cmbNhomVongVay.Items.Clear();
          lstNhomVongVay.Insert(0, new AutoCompleteEntry("", "", "0"));
          KhoiTaoGiaTriComboBox(ref lstNhomVongVay, ref cmbNhomVongVay, "COMBOBOX_NHOMVONGVAY", null, maVongVay);
          LoadDuLieuTaiKhoanHachToan(txtMaSanPham.Text);
          //CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, sTrangThai, mnuMain, DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM);
      }
      //bool VadidateData()
      //{
      //    if (cmbHinhThucVay.SelectedIndex < 0)
      //    {
      //        CommonFunction.ThongBaoTrong(lblHinhThucChoVay.Content.ToString());
      //        cmbHinhThucVay.Focus();
      //        return false;
      //    }
      //    else if (txtTenSanPham.Text.IsNullOrEmptyOrSpace())
      //    {
      //        CommonFunction.ThongBaoTrong(lblTenSPham.Content.ToString());
      //        txtTenSanPham.Focus();
      //        return false;
      //    }
      //    else if (cmbLoaiSanPham.SelectedIndex < 0)
      //    {
      //        CommonFunction.ThongBaoTrong(lblLoaiSanPham.Content.ToString());
      //        cmbLoaiSanPham.Focus();
      //        return false;
      //    }
      //    else if (teldtNgayHieuLuc.Value.IsNullOrEmpty())
      //    {
      //        CommonFunction.ThongBaoTrong(lblNgayHieuLuc.Content.ToString());
      //        teldtNgayHieuLuc.Focus();
      //        return false;
      //    }
      //    else if (teldtNgayHetHieuLuc.Value < teldtNgayHieuLuc.Value)
      //    {
      //        CommonFunction.ThongBaoLoi(label10.Content.ToString() + " Không hợp lệ");
      //        teldtNgayHetHieuLuc.Focus();
      //        return false;
      //    }
      //    else if (cmbCSTinhLai.Text.IsNullOrEmptyOrSpace())
      //    {
      //        CommonFunction.ThongBaoTrong(lblTenSPham.Content.ToString());
      //        cmbCSTinhLai.Focus();
      //        return false;
      //    }
      //    else if (numSoTienGop.IsVisible && numSoTienGop.Value.IsNullOrEmpty())
      //    {
      //        CommonFunction.ThongBaoTrong(lblSoTienGop.Content.ToString());
      //        numSoTienGop.Focus();
      //        return false;
      //    }
      //    else if (numSoTienGoc.IsVisible && numSoTienGoc.Value.GetValueOrDefault(0) < 1)
      //    {
      //        CommonFunction.ThongBaoTrong(lblSoTienGoc.Content.ToString());
      //        numSoTienGop.Focus();
      //        return false;
      //    }
      //    else
      //        return true;
      //}
       #endregion

      #region event
      protected void cmbNhomVongVay_SelectedIndexChanged(object sender, EventArgs e)
      {
          try
          {
              if (cmbNhomVongVay.SelectedIndex > 0)
              {
                  idVongVay = int.Parse(cmbNhomVongVay.SelectedValue.Split('|')[1]);// Convert.ToInt32(lstNhomVongVay.ElementAt(cmbNhomVongVay.SelectedIndex).KeywordStrings.ElementAt(1));
                  maVongVay = cmbNhomVongVay.SelectedValue.Split('|')[0];////); //lstNhomVongVay.ElementAt(cmbNhomVongVay.SelectedIndex).KeywordStrings.First();
                  TinhToanBangKeGocLai();
              }
              else
              {
                  //titemsGocLaiVay.Visibility = System.Windows.Visibility.Collapsed;
              }
          }
          catch (Exception ex)
          {
              //LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
              LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
          }
      }

               

      protected void btnMaLSuat_Click(object sender, EventArgs e)
      {
          //PopupProcess popupProcess = new PopupProcess();
          //List<string> lstDieuKien = new List<string>();
          //lstDieuKien.Add("TDVM");
          //lstDieuKien.Add(ClientInformation.MaDonVi);
          //lstDieuKien.Add(LDateTime.DateToString(teldtNgayHieuLuc.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat));
          //popupProcess.getPopupInformation("POPUP_DS_LAISUAT", lstDieuKien);
          //SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
          //ucPopup popup = new ucPopup(false, simplePopupResponse);
          //popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
          //Window win = new Window();
          //win.Content = popup;
          //win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
          //win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
          //win.ShowDialog();
          //if (lstPopup.Count > 0)
          {
              idLaiSuat = 12;
              LayThongTinLSuat();
          }
          cmbNhomVongVay_SelectedIndexChanged(sender, e);
      }

      protected void raddgrGocLaiVayDS_ItemDataBound(object sender, DataGridItemEventArgs e)
      {
          e.Item.Attributes.Add("onmouseover", "currColor=this.style.backgroundColor;this.style.backgroundColor='#FDDBA5'");
          e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currColor");
          try
          {
              string tableRowId = raddgrGocLaiVayDS.ClientID + "_" +
                                  ((VONG_VAY_CTIET)e.Item.DataItem).ID;
              e.Item.Attributes.Add("id", tableRowId);
          }
          catch { }
      }

      protected void grdTKhoan_ItemDataBound(object sender, DataGridItemEventArgs e)
      {
          e.Item.Attributes.Add("onmouseover", "currColor1=this.style.backgroundColor;this.style.backgroundColor='#FDDBA5'");
          e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currColor1");
          try
          {
              string tableRowId = grdTKhoan.ClientID + "_" +
                                   ((DataRowView)e.Item.DataItem)["MA_KY_HIEU"].ToString();
              e.Item.Attributes.Add("id", tableRowId);
          }
          catch { }
      }
      #endregion
    }
}