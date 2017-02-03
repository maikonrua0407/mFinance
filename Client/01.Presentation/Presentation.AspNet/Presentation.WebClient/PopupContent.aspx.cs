using Presentation.WebClient.Business;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Services;

using Utilities.Common;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.HuyDongVonServiceRef;
using Presentation.Process.LaiSuatServiceRef;
using Presentation.Process.KhachHangServiceRef;
using Presentation.WebClient.Business.CustomControl;

namespace Presentation.WebClient
{
    public partial class PopupContent : System.Web.UI.Page
    {
        #region [Properties]

        private static string mv_strRootPath = AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug\\", "").Replace("bin\\Release\\", "").Replace("bin\\", "");

        #endregion

        #region [Data Source]


        #endregion

        #region [Private Methods]

        protected void LoadParams()
        {
            if (null != CacheService.Instance().CurrentPortal)
            {
                this.Title = CacheService.Instance().CurrentPortal.Title;
                foreach (Control ctl in this.Header.Controls)
                {
                    if (ctl is HtmlMeta)
                    {
                        if (((HtmlMeta)ctl).Name.ToLower() == "keywords")
                        {
                            ((HtmlMeta)ctl).Content = CacheService.Instance().CurrentPortal.Keywords;
                        }
                        else if (((HtmlMeta)ctl).Name.ToLower() == "description")
                        {
                            ((HtmlMeta)ctl).Content = CacheService.Instance().CurrentPortal.Descriptions;
                        }
                    }
                    else if (ctl is HtmlLink)
                    {
                        if (((HtmlLink)ctl).Attributes["rel"] == "shortcut icon")
                        {
                            ((HtmlLink)ctl).Href = CacheService.Instance().CurrentPortal.ICon.Replace("~", "");
                        }
                    }
                }

                //Them css,javascript theo theme
                string v_strAttach = "";
                string[] v_arrFile = Directory.GetFiles(Path.GetFullPath(CacheService.Instance().CurrentPortal.TemplateBase.Replace("~", mv_strRootPath)) + "Attach\\");
                if ((null != v_arrFile) && (v_arrFile.GetLength(0) > 0))
                {
                    for (int i = 0; i < v_arrFile.GetLength(0); i++)
                    {
                        switch (Path.GetExtension(v_arrFile[i]).ToUpper().Trim())
                        {
                            case ".CSS":
                                {
                                    v_strAttach += "<link href=\"" + CacheService.Instance().CurrentPortal.TemplateBase.Replace("~/", "/") + "/Attach/" + Path.GetFileName(v_arrFile[i]) + "\" type=\"text/css\" rel=\"Stylesheet\" />\n\r";
                                    break;
                                }
                            case ".JS":
                                {
                                    v_strAttach += "<script language=\"javascript\" type=\"text/javascript\" src=\"" + CacheService.Instance().CurrentPortal.TemplateBase.Replace("~/", "/") + "/Attach/" + Path.GetFileName(v_arrFile[i]) + "\" ></script>\n\r";
                                    break;
                                }
                        }
                    }
                }

                Literal v_el = new Literal();
                v_el.Text = v_strAttach;

                this.Header.Controls.Add(v_el);
            }
        }

        private void InitTemplate()
        {
            string v_strTemplate = "";


            DataRow v_dr = ((DataRow)CacheService.Instance().PageConfig[AppConfig.TabID]);

            if (null != v_dr)
            {
                v_strTemplate = "POPUP";// v_dr["TemplateSrc"].ToString().Trim().ToUpper();
                // if (v_strTemplate.Length <= 0) v_strTemplate = "DEFAULT";

                DataRow v_drTemplate = (DataRow)CacheService.Instance().TemplateDef[v_strTemplate];
                if (null != v_drTemplate)
                {
                    string v_strTemplatePath = v_drTemplate["TemplateSrc"].ToString().Trim();
                    if (v_strTemplatePath.Length > 0)
                    {
                        UserControl control = (UserControl)this.LoadControl(v_strTemplatePath);
                        if (null != control)
                        {
                            this.Form.Controls.Add(control);
                        }
                    }
                }
            }
        }

        #endregion

        #region [Event Handles]

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            LoadParams();
            InitTemplate();
        }

        #endregion

        #region public webmethod

        #region MoSo
        [WebMethod]
        public static string GetInfoCus(int idKhachHang)
        {
            string result = "";            
            KhachHangProcess processKH = new KhachHangProcess();
            DataRow dr = processKH.getThongTinCoBanKHTheoID(idKhachHang).Tables[0].Rows[0];

            result += dr["ID"].ToString()+"#";
            result += dr["MA_KHANG"].ToString()+"#";
            result += dr["TEN_KHANG"].ToString()+"#";
            result += dr["DIA_CHI"].ToString()+"#";
            result += dr["SO_DTHOAI"].ToString()+"#";
            if (dr["DD_GTLQ_LOAI"].ToString().Equals("CHUNG_MINH_ND"))
            {
               result +=  dr["DD_GTLQ_SO"].ToString()+"#";
               if (LDateTime.IsDate(dr["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd"))
                   result += LDateTime.StringToDate(dr["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd").ToString("dd/MM/yyyy") + "#";
               else
                   result += "#";
                result += dr["DD_GTLQ_NOI_CAP"].ToString();
            }
            return result;
        }

        #endregion
        #region guithemtien chi tiet
        [WebMethod]
        public static string ThongtinSotiengui(int idSoTGui)
        {
            string Result = "";
            HuyDongVonProcess processHDV = new HuyDongVonProcess();
            DataSet ds = processHDV.GetThongTinQTrongSoTGui(idSoTGui);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                #region Hiển thị thông tin sổ
                Result+= dr["ID"].ToString()+"#";
                Result += dr["SO_SO_TG"].ToString() + "#";
                Result += Convert.ToDouble(dr["SO_TIEN"]).ToString("#,##0") + "#";
                Result += dr["MA_LOAI_TIEN"].ToString() + "#";
                Result += Convert.ToDouble(dr["LAI_SUAT"]).ToString() + "#";
                //maSanPham = dr["MA_SAN_PHAM"].ToString();
                if (LDateTime.IsDate(dr["NGAY_MO_SO"].ToString(), "yyyyMMdd"))
                    Result += LDateTime.StringToDate(dr["NGAY_MO_SO"].ToString(), "yyyyMMdd").ToString("dd/MM/yyyy") + "#";
                else
                    Result += "" + "#";

                if (LDateTime.IsDate(dr["NGAY_DEN_HAN"].ToString(), "yyyyMMdd"))
                    Result += LDateTime.StringToDate(dr["NGAY_DEN_HAN"].ToString(), "yyyyMMdd").ToString("dd/MM/yyyy") + "#";
                else
                    Result += "" + "#";
                #endregion

                #region Hiển thị thông tin khách hàng
                Result += dr["ID_KHANG"].ToString() + "#";
                Result += dr["MA_KHANG"].ToString() + "#";
                Result += dr["TEN_KHANG"].ToString() + "#";
                Result += dr["DIA_CHI"].ToString() + "#";
                Result += dr["SO_DTHOAI"].ToString() + "#";

                if (dr["DD_GTLQ_LOAI"].ToString().Equals("CHUNG_MINH_ND"))
                {
                    Result += dr["DD_GTLQ_SO"].ToString() + "#";
                    if (LDateTime.IsDate(dr["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd"))
                        Result += LDateTime.StringToDate(dr["DD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd").ToString("dd/MM/yyyy") + "#";
                    else
                        Result += "" + "#";
                    Result += dr["DD_GTLQ_NOI_CAP"].ToString() + "#";
                }
                #endregion
            }
            return Result;
        }
        [WebMethod]
        public static string GuiThemCT_THEM(string data,string straction,string IDVALUE)
        {
            string result = "";
            string stype = "0";
            string display = "";
            string val = "";
            if (data == "" || data.Split('#').Length <= 1)
                return "";
            int IDGIAODICH = 0;

            string [] strdata = data.Split('#');
            List<string> kq = new List<string>();
            if (straction == "THEM")
            {
                try
                {

                    HDV_GUI_TIEN_THEO_SO obj = new HDV_GUI_TIEN_THEO_SO();
                    obj = getdata(data);
                    HuyDongVonProcess processHDV = new HuyDongVonProcess();
                    List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                    bool ret = false;

                    ret = processHDV.GuiThemTheoTungSo(DatabaseConstant.Action.THEM, ref obj, ref listClientResponseDetail);
                    if (ret)
                    {
                        display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.ThemThanhCong");

                        val = obj.MA_GDICH + "@@";

                        val += LanguageEngine.Instance().GetContent(LanguageType.TypeUI, BusinessConstant.layMaNgonNguNghiepVu(obj.TRANG_THAI_NGHIEP_VU)) + "@@";
                        val += LanguageEngine.Instance().GetContent(LanguageType.TypeUI, BusinessConstant.layMaNgonNguSuDung(obj.TRANG_THAI_BAN_GHI)) + "@@";
                        val += LDateTime.StringToDate(obj.NGAY_LAP, "yyyyMMdd").ToString("dd/MM/yyyy") + "@@";
                        val += obj.NGUOI_LAP + "@@";
                        val += obj.ID.ToString();
                        stype = "1";
                    }
                    else
                    {
                        foreach (ClientResponseDetail cl in listClientResponseDetail)
                        {
                            display += LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]) + "@@";
                        }

                    }
                    // AfterAddNew(ret, obj, listClientResponseDetail);//
                }
                catch (Exception ex)
                {
                    display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.ThemKhongThanhCong");
                    //
                }
            }else
                if (straction == "SUA")
                {
                    try
                    {
                        
                        HuyDongVonProcess processHDV = new HuyDongVonProcess();
                        List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                        bool ret = false;
                        HDV_GUI_TIEN_THEO_SO obj = new HDV_GUI_TIEN_THEO_SO();
                        obj = getdata(data);
                        ret = processHDV.GuiThemTheoTungSo(DatabaseConstant.Action.SUA, ref obj, ref listClientResponseDetail);
                        KeToanProcess processHDVGET = new KeToanProcess();
                        DataSet ds = processHDVGET.getGiaoDich(obj.MA_DVI, obj.MA_GDICH);
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = ds.Tables[0].Rows[0];
                            IDGIAODICH = int.Parse(dr["ID"].ToString());
                            try
                            {
                                if (ret)
                                {
                                    display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.CapNhatThanhCong");
                                    val = obj.MA_GDICH + "@@";
                                    val += BusinessConstant.layMaNgonNguNghiepVu(obj.TRANG_THAI_NGHIEP_VU) + "@@";
                                    val += BusinessConstant.layMaNgonNguSuDung(obj.TRANG_THAI_BAN_GHI) + "@@";
                                    val += LDateTime.StringToDate(obj.NGAY_LAP, "yyyyMMdd").ToString("dd/MM/yyyy") + "@@";
                                    val += obj.NGUOI_LAP + "@@";
                                    val += obj.ID.ToString() + "@@";
                                    val += AppConfig.LoginedUser.NgayLamViecHienTai + "@@";
                                    val += AppConfig.LoginedUser.UserName + "@@";
                                    stype = "1";

                                }
                                else
                                {
                                    foreach (ClientResponseDetail cl in listClientResponseDetail)
                                    {
                                        display += LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]) + "@@";
                                    }
                                }

                                // Yêu cầu Unlock bản ghi cần sửa
                                UtilitiesProcess process = new UtilitiesProcess();
                                List<int> listLockId = new List<int>();
                                listLockId.Add(IDGIAODICH);

                                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                                    DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
                                    DatabaseConstant.Table.BL_TIEN_GUI,
                                    DatabaseConstant.Action.SUA,
                                    listLockId);
                            }
                            catch (Exception ex)
                            {
                                display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.LoiChung");
                            }
                        }
                        else
                        {
                            display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.LoiChung");
                        }
                    }
                    catch (Exception ex)
                    {
                        display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.LoiChung");
                    }
                }

            return stype+"#"+display+"#"+val;
        }
        [WebMethod]
        public static string GuiThemCT_SUA(string data, string straction,string IDVALUE)
        {
            
            string stype = "0";
            string display = "";
            string val = "";
            try
            {
                int IDGIAODICH = int.Parse(data);
                //KeToanProcess processHDVGET = new KeToanProcess();
                // DataSet ds = processHDVGET.getGiaoDich(AppConfig.LoginedUser.MaDonViGiaoDich, IDVALUE);
                // if (ds != null && ds.Tables[0].Rows.Count > 0)
                // {
                //     DataRow dr = ds.Tables[0].Rows[0];
                //     IDGIAODICH = int.Parse(dr["ID"].ToString());
                // }
                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(IDGIAODICH);

                bool ret = process.LockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                if (!ret)
                {
                    display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid");
                }
                else
                {
                    
                    stype = "1";
                    
                }
            }
            catch (Exception ex)
            {
                display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid");
            }
            return stype + "#" + display + "#" + val;
        }
        [WebMethod]
        public static string GuiThemCT_XOA(string data, string straction, string IDVALUE)
        {
            string stype = "0";
            string display = "";
            string val = "";
            try
            {
                int IDGIAODICH = 0;
                KeToanProcess processHDVGET = new KeToanProcess();
                DataSet ds = processHDVGET.getGiaoDich(AppConfig.LoginedUser.MaDonViGiaoDich, IDVALUE);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    IDGIAODICH = int.Parse(dr["ID"].ToString());
                }
                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(IDGIAODICH);

                bool ret = process.LockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                if (!ret)
                {
                    display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid");
                }
                else
                {
                    ;
                    HuyDongVonProcess processHDV = new HuyDongVonProcess();
                    List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                    HDV_GUI_TIEN_THEO_SO obj = new HDV_GUI_TIEN_THEO_SO();
                    obj = getdata(data);
                    obj.MA_GDICH = IDVALUE;
                    ret = processHDV.GuiThemTheoTungSo(DatabaseConstant.Action.XOA, ref obj, ref listClientResponseDetail);
                    if (ret)
                    {
                        display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.XoaThanhCong");
                        stype = "1";
                    }
                    else
                    {
                       
                        foreach (ClientResponseDetail cl in listClientResponseDetail)
                        {
                            display += LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]) + "@@";
                        }
                        
                    }
                    // Yêu cầu unlock dữ liệu
                    try
                    {
                        bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                            DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
                            DatabaseConstant.Table.BL_TIEN_GUI,
                            DatabaseConstant.Action.XOA,
                            listLockId);
                    }
                    catch { }
                    // Đóng cửa sổ chi tiết sau khi xóa
                }
            }
            catch (Exception ex)
            {
                display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid");
            }
            return stype + "#" + display + "#" + val;
        }
        [WebMethod]
        public static string GuiThemCT_DUYET(string data, string straction, string IDVALUE)
        {
            string stype = "0";
            string display = "";
            string val = "";
            try
            {
                int IDGIAODICH = 0;
                KeToanProcess processHDVGET = new KeToanProcess();
                DataSet ds = processHDVGET.getGiaoDich(AppConfig.LoginedUser.MaDonViGiaoDich, IDVALUE);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    IDGIAODICH = int.Parse(dr["ID"].ToString());
                }
                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(IDGIAODICH);

                bool ret = process.LockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.DUYET,
                    listLockId);

                if (!ret)
                {
                    display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid");
                }
                else
                {
                   
                    HuyDongVonProcess processHDV = new HuyDongVonProcess();
                    List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                    HDV_GUI_TIEN_THEO_SO obj = new HDV_GUI_TIEN_THEO_SO();
                    obj = getdata(data);
                    obj.MA_GDICH = IDVALUE;
                    ret = processHDV.GuiThemTheoTungSo(DatabaseConstant.Action.DUYET, ref obj, ref listClientResponseDetail);
                    if (ret)
                    {
                        display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.DuyetThanhCong");
                        val = obj.MA_GDICH + "@@";
                        string sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                        val += Convert.ToDouble(obj.SO_DU).ToString("#,##0") + "@@";
                        val += LanguageEngine.Instance().GetContent(LanguageType.TypeUI, BusinessConstant.layMaNgonNguNghiepVu(sTrangThaiNVu)) + "@@";
                        val += LanguageEngine.Instance().GetContent(LanguageType.TypeUI,BusinessConstant.layMaNgonNguSuDung(obj.TRANG_THAI_BAN_GHI)) + "@@";
                        val += LDateTime.StringToDate(obj.NGAY_LAP, "yyyyMMdd").ToString("dd/MM/yyyy") + "@@";
                        val += obj.NGUOI_LAP + "@@";
                        val += obj.ID.ToString() + "@@";
                        val += AppConfig.LoginedUser.NgayLamViecHienTai + "@@";
                        val += AppConfig.LoginedUser.UserName + "@@";
                        stype = "1";
                    }
                    else
                    {

                        foreach (ClientResponseDetail cl in listClientResponseDetail)
                        {
                            display += LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]) + "@@";
                        }

                    }
                    // Yêu cầu unlock dữ liệu
                    try
                    {
                        bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                            DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
                            DatabaseConstant.Table.BL_TIEN_GUI,
                            DatabaseConstant.Action.DUYET,
                            listLockId);
                    }
                    catch { }
                    // Đóng cửa sổ chi tiết sau khi xóa
                }
            }
            catch (Exception ex)
            {
                display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid");
            }
            return stype + "#" + display + "#" + val;
        }
        [WebMethod]
        public static string GuiThemCT_THOAIDUYET(string data, string straction, string IDVALUE)
        {
            string stype = "0";
            string display = "";
            string val = "";
            try
            {
                int IDGIAODICH = 0;
                KeToanProcess processHDVGET = new KeToanProcess();
                DataSet ds = processHDVGET.getGiaoDich(AppConfig.LoginedUser.MaDonViGiaoDich, IDVALUE);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    IDGIAODICH = int.Parse(dr["ID"].ToString());
                }
                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(IDGIAODICH);

                bool ret = process.LockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listLockId);

                if (!ret)
                {
                    display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid");
                }
                else
                {
                    ;
                    HuyDongVonProcess processHDV = new HuyDongVonProcess();
                    List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                    HDV_GUI_TIEN_THEO_SO obj = new HDV_GUI_TIEN_THEO_SO();
                    obj = getdata(data);
                    obj.MA_GDICH = IDVALUE;
                    ret = processHDV.GuiThemTheoTungSo(DatabaseConstant.Action.THOAI_DUYET, ref obj, ref listClientResponseDetail);
                    if (ret)
                    {
                        display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.ThoaiDuyetThanhCong");
                        val = obj.MA_GDICH + "@@";
                        string sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();

                        val += Convert.ToDouble(obj.SO_DU).ToString("#,##0") + "@@";
                        val += LanguageEngine.Instance().GetContent(LanguageType.TypeMessage,BusinessConstant.layMaNgonNguNghiepVu(sTrangThaiNVu)) + "@@";
                        val += LanguageEngine.Instance().GetContent(LanguageType.TypeMessage,BusinessConstant.layMaNgonNguSuDung(obj.TRANG_THAI_BAN_GHI)) + "@@";
                        val += LDateTime.StringToDate(obj.NGAY_LAP, "yyyyMMdd").ToString("dd/MM/yyyy") + "@@";
                        val += obj.NGUOI_LAP + "@@";
                        val += obj.ID.ToString() + "@@";
                        val += AppConfig.LoginedUser.NgayLamViecHienTai + "@@";
                        val += AppConfig.LoginedUser.UserName + "@@";
                        stype = "1";
                    }
                    else
                    {

                        foreach (ClientResponseDetail cl in listClientResponseDetail)
                        {
                            display += LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]) + "@@";
                        }
                    }
                    // Yêu cầu unlock dữ liệu
                    try
                    {
                        bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                            DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
                            DatabaseConstant.Table.BL_TIEN_GUI,
                            DatabaseConstant.Action.THOAI_DUYET,
                            listLockId);
                    }
                    catch { }
                    // Đóng cửa sổ chi tiết sau khi xóa
                }
            }
            catch (Exception ex)
            {
                display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid");
            }
            return stype + "#" + display + "#" + val;
        }
        [WebMethod]
        public static string GuiThemCT_TUCHOIDUYET(string data, string straction, string IDVALUE)
        {
            string stype = "0";
            string display = "";
            string val = "";
            try
            {
                int IDGIAODICH = 0;
                KeToanProcess processHDVGET = new KeToanProcess();
                DataSet ds = processHDVGET.getGiaoDich(AppConfig.LoginedUser.MaDonViGiaoDich, IDVALUE);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    IDGIAODICH = int.Parse(dr["ID"].ToString());
                }
                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(IDGIAODICH);

                bool ret = process.LockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
                    DatabaseConstant.Table.BL_TIEN_GUI,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);

                if (!ret)
                {
                    display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid");
                }
                else
                {
                    ;
                    HuyDongVonProcess processHDV = new HuyDongVonProcess();
                    List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                    HDV_GUI_TIEN_THEO_SO obj = new HDV_GUI_TIEN_THEO_SO();
                    obj = getdata(data);
                    obj.MA_GDICH = IDVALUE;
                    ret = processHDV.GuiThemTheoTungSo(DatabaseConstant.Action.TU_CHOI_DUYET, ref obj, ref listClientResponseDetail);
                    if (ret)
                    {
                        display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.TuChoiDuyetThanhCong");
                        val = obj.MA_GDICH + "@@";
                        val += BusinessConstant.layMaNgonNguNghiepVu(BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri()) + "@@";
                        val += BusinessConstant.layMaNgonNguSuDung(obj.TRANG_THAI_BAN_GHI) + "@@";
                        val += LDateTime.StringToDate(obj.NGAY_LAP, "yyyyMMdd").ToString("dd/MM/yyyy") + "@@";
                        val += obj.NGUOI_LAP + "@@";
                        val += obj.ID.ToString() + "@@";
                        val += AppConfig.LoginedUser.NgayLamViecHienTai + "@@";
                        val += AppConfig.LoginedUser.UserName + "@@";
                        stype = "1";
                    }
                    else
                    {

                        foreach (ClientResponseDetail cl in listClientResponseDetail)
                        {
                            display += LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]) + "@@";
                        }

                    }
                    // Yêu cầu unlock dữ liệu
                    try
                    {
                        bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                            DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
                            DatabaseConstant.Table.BL_TIEN_GUI,
                            DatabaseConstant.Action.TU_CHOI_DUYET,
                            listLockId);
                    }
                    catch { }
                    // Đóng cửa sổ chi tiết sau khi xóa
                }
            }
            catch (Exception ex)
            {
                display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid");
            }
            return stype + "#" + display + "#" + val;
        }
        public static HDV_GUI_TIEN_THEO_SO getdata(string sdata)
        {
            string[] strdata = sdata.Split('#');
            HDV_GUI_TIEN_THEO_SO obj = new HDV_GUI_TIEN_THEO_SO();
                // getdata
                obj.MA_GDICH = strdata[0];
                obj.SO_SO_TG = strdata[1];
                obj.SO_DU = Convert.ToDecimal(strdata[2]);
                obj.MA_SAN_PHAM = "";
                obj.LOAI_TIEN = strdata[4];
                obj.LAI_SUAT = Convert.ToDecimal(strdata[5]);
                obj.NGAY_MO_SO = LDateTime.StringToDate(strdata[6], "dd/MM/yyyy").ToString("yyyyMMdd");
                if (strdata[7] != "")
                    obj.NGAY_DAO_HAN = LDateTime.StringToDate(strdata[7], "dd/MM/yyyy").ToString("yyyyMMdd");

                //Thông tin khách hàng
                obj.MA_KHACH_HANG = strdata[8];
                obj.TEN_KHACH_HANG = strdata[9];
                obj.DIA_CHI = strdata[10];
                obj.SO_CMND = strdata[11];
                if (strdata[12] != "")
                    obj.NGAY_CAP = LDateTime.StringToDate(strdata[12], "dd/MM/yyyy").ToString("yyyyMMdd");
                obj.NOI_CAP = strdata[13];
                obj.SO_DIEN_THOAI = strdata[14];

                //Thông tin giao dịch
                obj.MA_DVI = strdata[15]; 
                obj.MA_DVI_QLY = strdata[16]; 
                obj.NGAY_GDICH = strdata[17]; 
                obj.HINH_THUC_GIAO_DICH = strdata[18];
                obj.TONG_TIEN_GIAO_DICH = Convert.ToDecimal(strdata[19].Replace(",", ""));
                obj.SO_TIEN_MAT = Convert.ToDecimal(strdata[20].Replace(",", ""));
                obj.SO_TIEN_CHUYEN_KHOAN = Convert.ToDecimal(strdata[21].Replace(",", ""));
                obj.TAI_KHOAN_KHACH_HANG = strdata[22];
                obj.TAI_KHOAN_NOI_BO = strdata[23];
                obj.DIEN_GIAI = strdata[24];

                //Thông tin kiểm soát
                obj.TRANG_THAI_NGHIEP_VU = strdata[25];
                obj.TRANG_THAI_BAN_GHI = strdata[26];
                obj.NGAY_LAP = LDateTime.StringToDate(strdata[27], "dd/MM/yyyy").ToString("yyyyMMdd");
                obj.NGUOI_LAP = strdata[28];
                if (strdata[29] != "THEM")
                {
                    obj.NGAY_CAP_NHAT = strdata[30];
                    obj.NGUOI_CAP_NHAT = strdata[31];
                }
                return obj;
        }
        #endregion
        #region guithemtien ds
        [WebMethod]
        public static string GuiThemDS_SUA(string data, string straction, string IDVALUE)
        {

            string stype = "0";
            string display = "";
            string val = "";
            try
            {
                int IDGIAODICH = int.Parse(data);               
                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(IDGIAODICH);

                bool ret = process.LockData(DatabaseConstant.Module.HDVO,
                DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_DANH_SACH,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                listLockId);

                if (!ret)
                {
                    display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid");
                }
                else
                {

                    stype = "1";

                }
            }
            catch (Exception ex)
            {
                display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid");
            }
            return stype + "#" + display + "#" + val;
        }
        [WebMethod]
        public static string GuiThemDS_THEM(string data, string straction,string IDVALUE)
        {
            string result = "";
            string stype = "0";
            string display = "";
            string val = "";
            if (data == "" || data.Split('#').Length <= 1)
                return "";
            int IDGIAODICH = 0;

            string[] strdata = data.Split('#');
            List<string> kq = new List<string>();
            if (straction == "THEM")
            {
                try
                {

                    HDV_GUI_TIEN_THEO_DANH_SACH obj = new HDV_GUI_TIEN_THEO_DANH_SACH();
                    obj = GetFormData(data, straction);

                    HuyDongVonProcess processHDV = new HuyDongVonProcess();
                    List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                    bool ret = false;

                    ret = processHDV.GuiThemTheoDanhSach(DatabaseConstant.Action.THEM, ref obj, ref listClientResponseDetail);                    
                    if (ret)
                    {
                        display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.ThemThanhCong");
                        val = obj.ID + "@@";
                        val += obj.MA_GDICH + "@@";
                        val += obj.TRANG_THAI_NGHIEP_VU + "@@";
                        val += LanguageEngine.Instance().GetContent(LanguageType.TypeUI, BusinessConstant.layMaNgonNguNghiepVu(obj.TRANG_THAI_NGHIEP_VU)) + "@@";
                        val += LanguageEngine.Instance().GetContent(LanguageType.TypeUI, BusinessConstant.layMaNgonNguSuDung(obj.TRANG_THAI_BAN_GHI)) + "@@";
                        val += LDateTime.StringToDate(obj.NGAY_LAP, "yyyyMMdd").ToString("dd/MM/yyyy") + "@@";
                        val += obj.NGUOI_LAP;

                        stype = "1";
                    }
                    else
                    {
                        foreach (ClientResponseDetail cl in listClientResponseDetail)
                        {
                            display += LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]) + "@@";
                        }

                    }
                    // AfterAddNew(ret, obj, listClientResponseDetail);//
                }
                catch (Exception ex)
                {
                    display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.ThemKhongThanhCong");
                    
                }
            }
            else
                if (straction == "SUA")
                {
                    try
                    {

                        HuyDongVonProcess processHDV = new HuyDongVonProcess();
                        List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                        bool ret = false;
                        HDV_GUI_TIEN_THEO_SO obj = new HDV_GUI_TIEN_THEO_SO();
                        obj = getdata(data);
                        ret = processHDV.GuiThemTheoTungSo(DatabaseConstant.Action.SUA, ref obj, ref listClientResponseDetail);
                        KeToanProcess processHDVGET = new KeToanProcess();
                        DataSet ds = processHDVGET.getGiaoDich(obj.MA_DVI, obj.MA_GDICH);
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = ds.Tables[0].Rows[0];
                            IDGIAODICH = int.Parse(dr["ID"].ToString());
                            try
                            {
                                if (ret)
                                {
                                    display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.CapNhatThanhCong");
                                    val = obj.MA_GDICH + "@@";
                                    val += BusinessConstant.layMaNgonNguNghiepVu(obj.TRANG_THAI_NGHIEP_VU) + "@@";
                                    val += BusinessConstant.layMaNgonNguSuDung(obj.TRANG_THAI_BAN_GHI) + "@@";
                                    val += LDateTime.StringToDate(obj.NGAY_LAP, "yyyyMMdd").ToString("dd/MM/yyyy") + "@@";
                                    val += obj.NGUOI_LAP + "@@";
                                    val += obj.ID.ToString() + "@@";
                                    val += AppConfig.LoginedUser.NgayLamViecHienTai + "@@";
                                    val += AppConfig.LoginedUser.UserName + "@@";
                                    stype = "1";

                                }
                                else
                                {
                                    foreach (ClientResponseDetail cl in listClientResponseDetail)
                                    {
                                        display += LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]) + "@@";
                                    }
                                }

                                // Yêu cầu Unlock bản ghi cần sửa
                                UtilitiesProcess process = new UtilitiesProcess();
                                List<int> listLockId = new List<int>();
                                listLockId.Add(IDGIAODICH);

                                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                                    DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO,
                                    DatabaseConstant.Table.BL_TIEN_GUI,
                                    DatabaseConstant.Action.SUA,
                                    listLockId);
                            }
                            catch (Exception ex)
                            {
                                display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.LoiChung");
                            }
                        }
                        else
                        {
                            display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.LoiChung");
                        }
                    }
                    catch (Exception ex)
                    {
                        display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.LoiChung");
                    }
                }

            return stype + "#" + display + "#" + val;
        }

        private static HDV_GUI_TIEN_THEO_DANH_SACH GetFormData(string sdata, string straction)
        {
            string[] vspt = new string[1] { "@@@" };
            string[] rootdata = sdata.Split(vspt, StringSplitOptions.None);
            string[] strdata = rootdata[0].Split('#');
            HDV_GUI_TIEN_THEO_DANH_SACH obj = new HDV_GUI_TIEN_THEO_DANH_SACH();
            try
            {
              
                obj.MA_GDICH = strdata[0];
                obj.NGAY_GDICH = AppConfig.LoginedUser.NgayLamViecHienTai;
                obj.HINH_THUC_GIAO_DICH = strdata[1];
                obj.LOAI_TIEN = strdata[2];
                obj.KY_THU_TIEN = Convert.ToInt32(strdata[3]);
                obj.NGUOI_GDICH = strdata[4];
                obj.DIA_CHI = strdata[5];
                obj.DIEN_GIAI = strdata[6];
                obj.MA_DVI = AppConfig.LoginedUser.MaDonViGiaoDich;
                obj.MA_DVI_QLY = AppConfig.LoginedUser.MaDonVi;
                //obj.THANG = Convert.ToInt32(numThang.Value);
                //obj.NAM= Convert.ToInt32(numNam.Value);

                //Thông tin kiểm soát
                obj.TRANG_THAI_NGHIEP_VU = strdata[7];
                if (straction == "THEM") obj.TRANG_THAI_NGHIEP_VU = "CDU";
                obj.TRANG_THAI_BAN_GHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.NGAY_LAP = LDateTime.StringToDate(strdata[9], "dd/MM/yyyy").ToString("yyyyMMdd");
                obj.NGUOI_LAP = strdata[10];
                if (straction != "THEM")
                {
                    obj.NGAY_CAP_NHAT = AppConfig.LoginedUser.NgayLamViecHienTai;
                    obj.NGUOI_CAP_NHAT = AppConfig.LoginedUser.UserName;
                }
                string[] strdsthem = rootdata[1].Split('#');
                List<DANH_SACH_SO> lstGuiThem = new List<DANH_SACH_SO>();
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                for (int j = 0; j < strdsthem.Length;j++ )
                {
                    if (strdsthem[j] != "")
                    {
                        //  '#TIEN_MAT#VND#0#ngd#dia chi#ass##SDU#05/10/2016#THAIHT#@@@5832@10000@600#5862@90000@800#','straction':'THEM','IDVALUE':'0'
                        string[] dsdetail = strdsthem[j].Split('@');
                        processHDV = new HuyDongVonProcess();
                        DataSet ds = processHDV.GetThongTinQTrongSoTGui(int.Parse(dsdetail[0]));
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {

                            DANH_SACH_SO objCT = new DANH_SACH_SO();
                            objCT.SO_SO_TG = ds.Tables[0].Rows[0]["SO_SO_TG"].ToString();
                            objCT.SO_DU = Convert.ToDecimal(dsdetail[2]);
                            objCT.LAI_SUAT = Convert.ToDecimal(ds.Tables[0].Rows[0]["LAI_SUAT"]);
                            objCT.TEN_KHACH_HANG = ds.Tables[0].Rows[0]["TEN_KHANG"].ToString();
                            objCT.SO_TIEN_GUI_THEM = Convert.ToDecimal(dsdetail[1]);
                            objCT.SO_DU_MOI = Convert.ToDecimal(dsdetail[2]) + Convert.ToDecimal(dsdetail[1]);
                            objCT.TAI_KHOAN_THANH_TOAN = "";
                            lstGuiThem.Add(objCT);
                        }
                    }
                }

                obj.DSACH_SO = lstGuiThem.ToArray();
            }
            catch (Exception ex)
            {
                
            }
            return obj;
        }

        [WebMethod]
        public static string GuiThemDS_DUYET(string data, string straction, string IDVALUE)
        {
            string stype = "0";
            string display = "";
            string val = "";
            try
            {
                int IDGIAODICH = 0;
                string StrangthaiNghiepvu = "";
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                
                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(int.Parse(IDVALUE));

                bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_DANH_SACH,
                        DatabaseConstant.Table.KT_GIAO_DICH,
                        DatabaseConstant.Action.DUYET,
                        listLockId);

                if (!retLockData)
                {
                    display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid");
                }
                else
                {
                    bool ret = false;
                    
                    List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                    HDV_GUI_TIEN_THEO_DANH_SACH obj = new HDV_GUI_TIEN_THEO_DANH_SACH();
                    obj = GetFormData(data, "DUYET");

                    //obj.TRANG_THAI_NGHIEP_VU = "CDU";

                    //obj.MA_GDICH = data.Split('#')[0];
                    processHDV = new HuyDongVonProcess();
                    ret = processHDV.GuiThemTheoDanhSach(DatabaseConstant.Action.DUYET, ref obj, ref listClientResponseDetail);

                    
                    if (ret)
                    {
                        display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.DuyetThanhCong");
                        val = obj.MA_GDICH + "@@";
                        string sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();

                        val += LanguageEngine.Instance().GetContent(LanguageType.TypeUI, BusinessConstant.layMaNgonNguNghiepVu(sTrangThaiNVu));
                        stype = "1";
                    }
                    else
                    {

                        foreach (ClientResponseDetail cl in listClientResponseDetail)
                        {
                            display += LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]) + "@@";
                        }

                    }
                    // Yêu cầu unlock dữ liệu
                    try
                    {
                        bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                            DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_DANH_SACH,
                            DatabaseConstant.Table.KT_GIAO_DICH,
                            DatabaseConstant.Action.DUYET,
                            listLockId);
                      
                    }
                    catch { }
                    // Đóng cửa sổ chi tiết sau khi xóa
                }
            }
            catch (Exception ex)
            {
                display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid");
            }
            return stype + "#" + display + "#" + val;
        }
        [WebMethod]
        public static string GuiThemDS_THOAIDUYET(string data, string straction, string IDVALUE)
        {
            string stype = "0";
            string display = "";
            string val = "";
            try
            {
                int IDGIAODICH = 0;
                string StrangthaiNghiepvu = "";
                HuyDongVonProcess processHDV = new HuyDongVonProcess();

                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(int.Parse(IDVALUE));

                bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_DANH_SACH,
                        DatabaseConstant.Table.KT_GIAO_DICH,
                        DatabaseConstant.Action.THOAI_DUYET,
                        listLockId);

                if (!retLockData)
                {
                    display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid");
                }
                else
                {
                    bool ret = false;

                    List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                    HDV_GUI_TIEN_THEO_DANH_SACH obj = new HDV_GUI_TIEN_THEO_DANH_SACH();
                    obj = GetFormData(data, "THOAIDUYET");

                    //obj.TRANG_THAI_NGHIEP_VU = "CDU";

                    //obj.MA_GDICH = data.Split('#')[0];
                    processHDV = new HuyDongVonProcess();
                    ret = processHDV.GuiThemTheoDanhSach(DatabaseConstant.Action.THOAI_DUYET, ref obj, ref listClientResponseDetail);


                    if (ret)
                    {
                        display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.ThoaiDuyetThanhCong");
                        val = obj.MA_GDICH + "@@";
                        string sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                        val += sTrangThaiNVu + "@@";
                        val += LanguageEngine.Instance().GetContent(LanguageType.TypeUI, BusinessConstant.layMaNgonNguNghiepVu(sTrangThaiNVu));
                        stype = "1";
                    }
                    else
                    {

                        foreach (ClientResponseDetail cl in listClientResponseDetail)
                        {
                            display += LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]) + "@@";
                        }

                    }
                    // Yêu cầu unlock dữ liệu
                    try
                    {
                        bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                            DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_DANH_SACH,
                            DatabaseConstant.Table.KT_GIAO_DICH,
                            DatabaseConstant.Action.THOAI_DUYET,
                            listLockId);

                    }
                    catch { }
                    // Đóng cửa sổ chi tiết sau khi xóa
                }
            }
            catch (Exception ex)
            {
                display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid");
            }
            return stype + "#" + display + "#" + val;
        }
        [WebMethod(EnableSession = true)]
        public static string GuiThemDS_DSGUITHEM(string idSoTGui)
        {
            string Result = "";
            try
            {
                DataSet dsGuiThem = new DataSet();
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
                int tongSoSo = 0;
                decimal tongDuCu = 0;
                decimal tongGuiThem = 0;
                decimal tongDuMoi = 0;
                if (idSoTGui != "")
                {
                    string[] stabId = idSoTGui.Split('#');
                    HuyDongVonProcess processHDV = new HuyDongVonProcess();
                    for (int i = 0; i < stabId.Length; i++)
                    {
                        int id = int.Parse(stabId[i].Split('@')[0]);
                        decimal sotien = 0;
                        decimal sodu = 0;
                        try
                        {
                            sotien = decimal.Parse(stabId[i].Split('@')[1]);
                            sodu = decimal.Parse(stabId[i].Split('@')[2]);
                        }
                        catch { }



                        DataRow drGuiThem = dsGuiThem.Tables[0].NewRow();
                        processHDV = new HuyDongVonProcess();
                        DataSet ds = processHDV.GetThongTinQTrongSoTGui(id);
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = ds.Tables[0].Select()[0];
                            if (KiemTraSo(dr["SO_SO_TG"].ToString(), dsGuiThem.Tables[0].Select()))
                            {
                                drGuiThem["STT"] = i + 1;
                                drGuiThem["ID"] = id;
                                decimal so_du = 0;
                                try
                                {
                                    so_du = Convert.ToDecimal(dr["SO_TIEN"]);
                                }
                                catch { }

                                drGuiThem["SO_SO_TG"] = dr["SO_SO_TG"];
                                drGuiThem["TEN_KHANG"] = dr["TEN_KHANG"];
                                drGuiThem["NGAY_MO_SO"] = dr["NGAY_MO_SO"];
                                drGuiThem["NGAY_DEN_HAN"] = dr["NGAY_DEN_HAN"];
                                drGuiThem["KY_HAN"] = dr["KY_HAN"];
                                if (sodu > 0)
                                    drGuiThem["SO_DU"] = sodu;
                                else
                                {
                                    drGuiThem["SO_DU"] = so_du;
                                }
                                drGuiThem["LAI_SUAT"] = Convert.ToDecimal(dr["LAI_SUAT"]);
                                drGuiThem["SO_TIEN_GUI_THEM"] = sotien;
                                drGuiThem["SO_DU_MOI"] = so_du + sotien;
                                dsGuiThem.Tables[0].Rows.Add(drGuiThem);
                            }
                        }

                    }
                    tongSoSo = dsGuiThem.Tables[0].Rows.Count;
                    tongDuCu = decimal.Parse(dsGuiThem.Tables[0].Compute("sum(SO_DU)", "").ToString());
                    tongGuiThem = decimal.Parse(dsGuiThem.Tables[0].Compute("sum(SO_TIEN_GUI_THEM)", "").ToString());
                    tongDuMoi = decimal.Parse(dsGuiThem.Tables[0].Compute("sum(SO_DU_MOI)", "").ToString());
                }
              
                
                Page pg = new Page();
                string path = @"\Modules\HDVO\GuiThem\ucGuiThemTheoDSCT.ascx";

                UserControl control = (UserControl)pg.LoadControl(path);
                pg.Controls.Add(control);
                DataGrid grGuiThemDS = (DataGrid)FindControlRecursive(control, "grGuiThemDS");// control.FindControl("grGuiThemDS");

                grGuiThemDS.DataSource = dsGuiThem;
                grGuiThemDS.DataBind();
                System.IO.StringWriter sw = new System.IO.StringWriter();
                grGuiThemDS.RenderControl(new HtmlTextWriter(sw));
                return sw.ToString() + "@@@" + tongSoSo.ToString("#,##0") + "@@@" + tongDuCu.ToString("#,##0") + "@@@" + tongGuiThem.ToString("#,##0")
                    + "@@@" + tongDuMoi.ToString("#,##0");
            }
            catch { }
            return "";
        }

        [WebMethod]
        public static string GuiThemDS_XOA(string data, string straction, string IDVALUE)
        {
            string stype = "0";
            string display = "";
            string val = "";
            try
            {
                
              
                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(int.Parse(IDVALUE));

                bool ret = process.LockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_DANH_SACH,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                if (!ret)
                {
                    display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid");
                }
                else
                {
                    ;
                    HuyDongVonProcess processHDV = new HuyDongVonProcess();
                    List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                    HDV_GUI_TIEN_THEO_DANH_SACH obj = new HDV_GUI_TIEN_THEO_DANH_SACH();
                    obj = GetFormData(data,"XOA");
                    //obj.MA_GDICH = IDVALUE;
                    ret = processHDV.GuiThemTheoDanhSach(DatabaseConstant.Action.XOA, ref obj, ref listClientResponseDetail);
                    if (ret)
                    {
                        display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.XoaThanhCong");
                        stype = "1";
                    }
                    else
                    {

                        foreach (ClientResponseDetail cl in listClientResponseDetail)
                        {
                            display += LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]) + "@@";
                        }

                    }
                    // Yêu cầu unlock dữ liệu
                    try
                    {
                        bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                             DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_DANH_SACH,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.XOA,
                    listLockId);
                    }
                    catch { }
                    // Đóng cửa sổ chi tiết sau khi xóa
                }
            }
            catch (Exception ex)
            {
                display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid");
            }
            return stype + "#" + display + "#" + val;
        }
        [WebMethod]
        public static string GuiThemDS_TUCHOIDUYET(string data, string straction, string IDVALUE)
        {
            string stype = "0";
            string display = "";
            string val = "";
            try
            {
                int IDGIAODICH = 0;
              
                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(int.Parse(IDVALUE));

                bool ret = process.LockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_DANH_SACH,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);

                if (!ret)
                {
                    display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid");
                }
                else
                {
                    ;
                    HuyDongVonProcess processHDV = new HuyDongVonProcess();
                    List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                    HDV_GUI_TIEN_THEO_DANH_SACH obj = new HDV_GUI_TIEN_THEO_DANH_SACH();
                    obj = GetFormData(data,"TUCHOIDUYET");
                    //obj.MA_GDICH = IDVALUE;
                    ret = processHDV.GuiThemTheoDanhSach(DatabaseConstant.Action.TU_CHOI_DUYET, ref obj, ref listClientResponseDetail);
                    if (ret)
                    {
                        display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.TuChoiDuyetThanhCong");
                        val = obj.MA_GDICH + "@@";
                        string sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                        val += sTrangThaiNVu + "@@";
                        val += LanguageEngine.Instance().GetContent(LanguageType.TypeUI, BusinessConstant.layMaNgonNguNghiepVu(sTrangThaiNVu));
                        stype = "1";
                    }
                    else
                    {

                        foreach (ClientResponseDetail cl in listClientResponseDetail)
                        {
                            display += LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]) + "@@";
                        }

                    }
                    // Yêu cầu unlock dữ liệu
                    try
                    {
                        bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                             DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_DANH_SACH,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);
                    }
                    catch { }
                    // Đóng cửa sổ chi tiết sau khi xóa
                }
            }
            catch (Exception ex)
            {
                display = LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid");
            }
            return stype + "#" + display + "#" + val;
        }
        public static Control FindControlRecursive(Control control, string id)
        {
            if (control == null) return null;
            //try to find the control at the current level
            Control ctrl = control.FindControl(id);

            if (ctrl == null)
            {
                //search the children
                foreach (Control child in control.Controls)
                {
                    ctrl = FindControlRecursive(child, id);

                    if (ctrl != null) break;
                }
            }
            return ctrl;
        }
        private static bool KiemTraSo(string sSoTGui, DataRow [] dsr)
        {
            foreach (DataRow dr in dsr)
            {
                if (dr["SO_SO_TG"].ToString().Equals(sSoTGui))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
        #endregion
    }
}