using Presentation.Process.ZAMainAppServiceRef;
using Presentation.WebClient.Business;
using Presentation.WebClient.Business.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.Common;

namespace Presentation.WebClient.Modules.System
{
    public partial class ctlLogin : ControlBase
    {
        #region [Custom methods]

        private void InitForm()
        {
            //if (null == AppConfig.LoginedUser)
            //{
                
            //}
            //else
            //{
            //    Response.Redirect("~/Default.aspx");   
            //}

            txtUserName.Text = "";
            txtPass.Text = "";
            chkRemember.Checked = false;
        }

        private bool ValidateForm()
        {
            bool v_blRet = false;

            if (txtUserName.Text.Trim().Length <= 0)
            {
                lblErrorDesc.Text = "Bạn chưa điền tài khoản đăng nhập!";
            }
            else if (txtPass.Text.Trim().Length < 6)
            {
                lblErrorDesc.Text = "Bạn chưa gõ đúng mật khẩu";
            }
            else
            {
                v_blRet = true;
            }

            return v_blRet;
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                InitForm();
            }
        }

        protected void cmdLogin_Click(object sender, EventArgs e)
        {
            lblErrorDesc.Text = "";
            long v_lngErrorCode = ErrorDef.SYSTEM_SUCESS;
            if (ValidateForm())
            {
                string v_strUser = txtUserName.Text.Trim().ToUpper();
                string v_strPass = LSecurity.MD5Encrypt(txtPass.Text.Trim());
                string v_strSessionId = "";

                string v_strResult = UserController.LoginRequest(v_strUser, v_strPass, ref v_strSessionId);
                if (v_strResult.StartsWith("OK"))
                {
                    XmlDocument v_objDoc = new XmlDocument();
                    v_objDoc.LoadXml(v_strResult.Substring(3));
                    UserInfo v_objUser = new UserInfo();
                    v_objUser.SessionID = v_strSessionId;
                    v_objUser.UserName = v_strUser;
                    v_objUser.IDNguoiSuDung = Convert.ToInt32(v_objDoc.DocumentElement.SelectSingleNode("IDNguoiSuDung").InnerText);
                    v_objUser.LoaiNguoiSuDung = v_objDoc.DocumentElement.SelectSingleNode("LoaiNguoiSuDung").InnerText;
                    v_objUser.FullName = v_objDoc.DocumentElement.SelectSingleNode("FullName").InnerText;
                    v_objUser.MaDonVi = v_objDoc.DocumentElement.SelectSingleNode("MaDonVi").InnerText;
                    v_objUser.TenDonVi = v_objDoc.DocumentElement.SelectSingleNode("TenDonVi").InnerText;

                    v_objUser.MaDonViQuanLy = v_objDoc.DocumentElement.SelectSingleNode("MaDonVi").InnerText;
                    v_objUser.TenDonViQuanLy = v_objDoc.DocumentElement.SelectSingleNode("TenDonVi").InnerText;
                    v_objUser.MaDonViGiaoDich = v_objDoc.DocumentElement.SelectSingleNode("MaDonVi").InnerText + "00";
                    v_objUser.TenDonViGiaoDich = "TenDonViGiaoDich";
                    //v_objUser.MaDonViQuanLy = v_objDoc.DocumentElement.SelectSingleNode("MaDonViQuanLy").InnerText;
                    //v_objUser.TenDonViQuanLy = v_objDoc.DocumentElement.SelectSingleNode("TenDonViQuanLy").InnerText;
                    //v_objUser.MaDonViGiaoDich = v_objDoc.DocumentElement.SelectSingleNode("MaDonViGiaoDich").InnerText;
                    //v_objUser.TenDonViGiaoDich = v_objDoc.DocumentElement.SelectSingleNode("TenDonViGiaoDich").InnerText;

                    v_objUser.NgayLamViecTruoc = v_objDoc.DocumentElement.SelectSingleNode("NgayLamViecTruoc").InnerText;
                    v_objUser.NgayLamViecHienTai = v_objDoc.DocumentElement.SelectSingleNode("NgayLamViecHienTai").InnerText;
                    v_objUser.NgayLamViecSau = v_objDoc.DocumentElement.SelectSingleNode("NgayLamViecSau").InnerText;
                    v_objUser.MaDongNoiTe = v_objDoc.DocumentElement.SelectSingleNode("MaDongNoiTe").InnerText;
                    v_objUser.CayMenu = LXMLMessage.ConvertXmlToDataTable(v_objDoc.DocumentElement.SelectSingleNode("CayMenu").InnerXml.Replace("<![CDATA[","").Replace("]]>",""));
                    v_objUser.DSDonVi = LXMLMessage.ConvertXmlToDataTable(v_objDoc.DocumentElement.SelectSingleNode("ListDonVi").InnerXml.Replace("<![CDATA[", "").Replace("]]>", ""));

                    AppConfig.LoginedUser = v_objUser;

                    //Them doan nay de co the goi cac service khac
                    UserInformation userInfo = new UserInformation();
                    //userInfo.SessionId = HttpContext.Current.Session["LoginedUser"].GetHashCode().ToString();
                    userInfo.SessionId = v_strSessionId;                    
                    userInfo.TenDangNhap = AppConfig.LoginedUser.UserName;
                    userInfo.ListChucNang = null;
                    userInfo.IdNguoiSuDung = AppConfig.LoginedUser.IDNguoiSuDung;
                    userInfo.HoTen = AppConfig.LoginedUser.FullName;
                    userInfo.MaDonVi = AppConfig.LoginedUser.MaDonVi;
                    userInfo.TenDonVi = AppConfig.LoginedUser.TenDonVi;
                    userInfo.MaDonViQuanLy = AppConfig.LoginedUser.MaDonViQuanLy;
                    userInfo.TenDonViQuanLy = AppConfig.LoginedUser.TenDonViQuanLy;
                    userInfo.MaDonViGiaoDich = AppConfig.LoginedUser.MaDonViGiaoDich;
                    userInfo.TenDonViGiaoDich = AppConfig.LoginedUser.TenDonViGiaoDich;
                    userInfo.LoaiNguoiSuDung = AppConfig.LoginedUser.LoaiNguoiSuDung;
                    userInfo.NgayLamViecTruoc = AppConfig.LoginedUser.NgayLamViecTruoc;
                    userInfo.NgayLamViecHienTai = AppConfig.LoginedUser.NgayLamViecHienTai;
                    userInfo.NgayLamViecSau = AppConfig.LoginedUser.NgayLamViecSau;
                    userInfo.MaDongNoiTe = AppConfig.LoginedUser.MaDongNoiTe;
                    userInfo.MacAddress = "";
                    HttpContext.Current.Session["UserInformation"] = userInfo;

                    //Add cookie
                    if (chkRemember.Checked)
                    {
                        v_objUser.SaveCookie();
                    }

                    Response.Redirect("~/Default.aspx");
                }
                else
                {
                    v_lngErrorCode = Convert.ToInt64(v_strResult.Replace("ERROR|", ""));

                    lblErrorDesc.Text = CacheService.Instance().GetErrorDef(v_lngErrorCode);
                    txtUserName.Focus();
                }
            }
        }
    }
}