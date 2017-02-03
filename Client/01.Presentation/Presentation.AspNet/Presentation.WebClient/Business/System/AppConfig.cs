using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.UI;
using Presentation.WebClient.Business.Entity;
using Utilities.Common;
using System.Web.UI.WebControls;
using System.Data;
using Presentation.Process.Common;

namespace Presentation.WebClient.Business
{
    public class AppConfig
    {
        public static string GetConfigKey(string pv_strKeyName, string pv_strDefaulValue)
        {
            string v_strRet = "";

            try
            {
                v_strRet = ConfigurationManager.AppSettings[pv_strKeyName].ToString().Trim();
                if (null == v_strRet) v_strRet = "";
            }
            catch
            {
                v_strRet = "";
            }

            return v_strRet;
        }

        public static string SQLConnectionString
        {
            get
            {
                return GetConfigKey("SQLConnection", "");
            }
        }

        public static string TabID
        {
            get
            {
                string v_strTabID = HttpContext.Current.Request.QueryString["TabID"];

                if ((null == v_strTabID) || (v_strTabID.Length == 0)) v_strTabID = "1";

                if (HttpContext.Current.Request.Path.ToUpper().IndexOf("Login.aspx".ToUpper()) >= 0)
                {
                    v_strTabID = "0";
                }

                return v_strTabID;
            }
        }


        public static string Language
        {
            get
            {
                string v_strLanguage = "vi";

                if (null != HttpContext.Current.Session && null != HttpContext.Current.Session["Applanguage"])
                {
                    v_strLanguage = HttpContext.Current.Session["Applanguage"].ToString().ToLower();
                }

                return v_strLanguage;
            }

            set
            {
                HttpContext.Current.Session["Applanguage"] = value.ToString().ToLower();
            }
        }

        public static string AppRootPath
        {
            get
            {
                string v_strRootPath = AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug\\", "").Replace("bin\\Release\\", "").Replace("bin\\", "");
                return v_strRootPath;
            }
        }

        public static UserInfo LoginedUser
        {
            get
            {
                UserInfo v_objUser = null;

                if (null != HttpContext.Current.Session["LoginedUser"])
                {
                    v_objUser = (UserInfo)HttpContext.Current.Session["LoginedUser"];
                }
                else if (null != HttpContext.Current.Request.Cookies["LoginedUser"])
                {
                    v_objUser = new UserInfo();
                    v_objUser.SessionID = HttpContext.Current.Request.Cookies["LoginedUser"].Values["SessionID"];
                    v_objUser.UserName = HttpContext.Current.Request.Cookies["LoginedUser"].Values["UserName"];
                    v_objUser.FullName = HttpContext.Current.Request.Cookies["LoginedUser"].Values["FullName"];
                    v_objUser.IDNguoiSuDung = Convert.ToInt32(HttpContext.Current.Request.Cookies["LoginedUser"].Values["IDNguoiSuDung"]);
                    v_objUser.LoaiNguoiSuDung = HttpContext.Current.Request.Cookies["LoginedUser"].Values["LoaiNguoiSuDung"];
                    v_objUser.MaDonVi = HttpContext.Current.Request.Cookies["LoginedUser"].Values["MaDonVi"];
                    v_objUser.TenDonVi = HttpContext.Current.Request.Cookies["LoginedUser"].Values["TenDonVi"];
                    v_objUser.NgayLamViecTruoc = HttpContext.Current.Request.Cookies["LoginedUser"].Values["NgayLamViecTruoc"];
                    v_objUser.NgayLamViecHienTai = HttpContext.Current.Request.Cookies["LoginedUser"].Values["NgayLamViecHienTai"];
                    v_objUser.NgayLamViecSau = HttpContext.Current.Request.Cookies["LoginedUser"].Values["NgayLamViecSau"];
                    v_objUser.MaDongNoiTe = HttpContext.Current.Request.Cookies["LoginedUser"].Values["MaDongNoiTe"];
                    v_objUser.CayMenu = LXMLMessage.ConvertXmlToDataTable(HttpContext.Current.Request.Cookies["LoginedUser"].Values["CayMenu"].Replace("<![CDATA[", "").Replace("]]>", "")); ;

                    //Them doan nay de co the goi cac service khac
                    UserInformation userInfo = new UserInformation();
                    //userInfo.SessionId = HttpContext.Current.Session["LoginedUser"].GetHashCode().ToString();
                    userInfo.SessionId = AppConfig.LoginedUser.SessionID;
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
                }

                return v_objUser;
            }
            set
            {
                if (null == value)
                {
                    HttpContext.Current.Session.Abandon();
                }
                else
                {
                    HttpContext.Current.Session["LoginedUser"] = value;
                }
            }
        }
    }

    public class Utils
    {
        #region [Validate]

        public static bool IsDate(string pv_strVal, string pv_strFormat)
        {
            bool v_blRet = true;
            try
            {
                DateTime.ParseExact(pv_strVal, pv_strFormat, null);
            }
            catch
            {
                v_blRet = false;
            }

            return v_blRet;
        }

        #endregion

        public static void BuildDropdownlist(ref DropDownList pv_objCbo, DataTable pv_dtData,bool pv_blGenDefaultItem=false,string pv_strDefaultVal="")
        {
            try
            {
                pv_objCbo.Items.Clear();

                if (pv_blGenDefaultItem)
                {
                    ListItem v_objRoot = new ListItem();
                    v_objRoot.Text = LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.TatCa");
                    v_objRoot.Value = "ALL";
                    pv_objCbo.Items.Add(v_objRoot);
                }

                if (null != pv_dtData)
                {
                    for (int i = 0; i < pv_dtData.Rows.Count; i++)
                    {
                        ListItem v_objItem = new ListItem();
                        v_objItem.Text = pv_dtData.Rows[i][2].ToString();
                        v_objItem.Value = pv_dtData.Rows[i][1].ToString();
                        pv_objCbo.Items.Add(v_objItem);
                    }

                    if (pv_strDefaultVal.Length > 0)
                    {
                        pv_objCbo.SelectedValue = pv_strDefaultVal;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}