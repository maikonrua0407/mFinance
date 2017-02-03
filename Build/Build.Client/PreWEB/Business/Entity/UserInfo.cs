using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Utilities.Common;

namespace Presentation.WebClient.Business.Entity
{
    public class UserInfo
    {
        #region [Variables]

        private string mv_strUserName;

        public string UserName
        {
            get { return mv_strUserName; }
            set { mv_strUserName = value; }
        }

        private string mv_strFullName;

        public string FullName
        {
            get { return mv_strFullName; }
            set { mv_strFullName = value; }
        }

        private int mv_iIDNguoiSuDung;

        public int IDNguoiSuDung
        {
            get { return mv_iIDNguoiSuDung; }
            set { mv_iIDNguoiSuDung = value; }
        }
        private string mv_strLoaiNguoiSuDung;

        public string LoaiNguoiSuDung
        {
            get { return mv_strLoaiNguoiSuDung; }
            set { mv_strLoaiNguoiSuDung = value; }
        }
        private string mv_strMaDonVi;

        public string MaDonVi
        {
            get { return mv_strMaDonVi; }
            set { mv_strMaDonVi = value; }
        }

        private string mv_strTenDonVi;

        public string TenDonVi
        {
            get { return mv_strTenDonVi; }
            set { mv_strTenDonVi = value; }
        }

        private string mv_strMaDonViQuanLy;

        public string MaDonViQuanLy
        {
            get { return mv_strMaDonViQuanLy; }
            set { mv_strMaDonViQuanLy = value; }
        }

        private string mv_strTenDonViQuanLy;

        public string TenDonViQuanLy
        {
            get { return mv_strTenDonViQuanLy; }
            set { mv_strTenDonViQuanLy = value; }
        }

        private string mv_strMaDonViGiaoDich;

        public string MaDonViGiaoDich
        {
            get { return mv_strMaDonViGiaoDich; }
            set { mv_strMaDonViGiaoDich = value; }
        }

        private string mv_strTenDonViGiaoDich;

        public string TenDonViGiaoDich
        {
            get { return mv_strTenDonViGiaoDich; }
            set { mv_strTenDonViGiaoDich = value; }
        }

        private string mv_strNgayLamViecTruoc;

        public string NgayLamViecTruoc
        {
            get { return mv_strNgayLamViecTruoc; }
            set { mv_strNgayLamViecTruoc = value; }
        }
        private string mv_strNgayLamViecHienTai;

        public string NgayLamViecHienTai
        {
            get { return mv_strNgayLamViecHienTai; }
            set { mv_strNgayLamViecHienTai = value; }
        }
        private string mv_strNgayLamViecSau;

        public string NgayLamViecSau
        {
            get { return mv_strNgayLamViecSau; }
            set { mv_strNgayLamViecSau = value; }
        }
        private string mv_strMaDongNoiTe;

        public string MaDongNoiTe
        {
            get { return mv_strMaDongNoiTe; }
            set { mv_strMaDongNoiTe = value; }
        }


        private DataTable mv_dtCayMenu = null;

        public DataTable CayMenu
        {
            get { return mv_dtCayMenu; }
            set { mv_dtCayMenu = value; }
        }

        #endregion

        #region [Properties]
        #endregion

        #region [Methods]

        public bool SaveCookie()
        {
            bool v_blRet = true;

            try
            {
                HttpContext.Current.Response.Cookies["LoginedUser"].Values["UserName"] = UserName;
                HttpContext.Current.Response.Cookies["LoginedUser"].Values["FullName"] = FullName;
                HttpContext.Current.Response.Cookies["LoginedUser"].Values["IDNguoiSuDung"] = IDNguoiSuDung.ToString();
                HttpContext.Current.Response.Cookies["LoginedUser"].Values["LoaiNguoiSuDung"] = LoaiNguoiSuDung;
                HttpContext.Current.Response.Cookies["LoginedUser"].Values["MaDonVi"] = MaDonVi;
                HttpContext.Current.Response.Cookies["LoginedUser"].Values["TenDonVi"] = TenDonVi;
                HttpContext.Current.Response.Cookies["LoginedUser"].Values["NgayLamViecTruoc"] = NgayLamViecTruoc;
                HttpContext.Current.Response.Cookies["LoginedUser"].Values["NgayLamViecHienTai"] = NgayLamViecHienTai;
                HttpContext.Current.Response.Cookies["LoginedUser"].Values["NgayLamViecSau"] = NgayLamViecSau;
                HttpContext.Current.Response.Cookies["LoginedUser"].Values["MaDongNoiTe"] = MaDongNoiTe;
                HttpContext.Current.Response.Cookies["LoginedUser"].Values["CayMenu"] = "<![CDATA[" + LXMLMessage.ConvertDataTableToXml(CayMenu) + "]]>";
                HttpContext.Current.Response.Cookies["LoginedUser"].Expires = DateTime.Now.AddDays(7);
            }
            catch (Exception ex)
            {
                v_blRet = false;
            }

            return v_blRet;
        }

        public bool Logout()
        {
            bool v_blRet = true;
            try
            {
                HttpContext.Current.Session["LoginedUser"] = null;
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Response.Cookies["LoginedUser"].Expires = DateTime.Now;
            }
            catch (Exception ex)
            {
                v_blRet = false;
            }
            return v_blRet;
        }

        #endregion
    }
}