using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using PresentationAspNet.MVC.App_Start;
using Utilities.Common;

namespace PresentationAspNet.MVC
{
    public class DefaultSettings
    {
        public static string BaseDirectory;
        public static bool ShowMissingMessage;
    }

    public static class Common
    {

        public static int SoTrangHienThi = 10;

        public static int SoBanGhiTrenTrang()
        {
            var sobanghi = 10;
            return sobanghi > 10 && sobanghi % 10 == 0 ? sobanghi : 10;
        }

        public static int SoBanGhiHienThi()
        {
            return 50;
        }
        public enum ResultJson
        {
            Success,
            Error,
            NoRole,
            NotExistItem,
            Notice
        }
        public static string LayMa(this ResultJson loai)
        {
            switch (loai)
            {
                case ResultJson.Success: return "SUCCESS";
                case ResultJson.Error: return "ERROR";
                case ResultJson.NoRole: return "NOROLE";
                case ResultJson.NotExistItem: return "NOTEXISTITEM";
                case ResultJson.Notice: return "THONGBAO";
                default: return "";
            }
        }

        public static int GetIdDonViGd()
        {
            try
            {
                if (UserInformation.Session_User != null)
                {
                    return UserInformation.Session_User.IdDonViGiaoDich;
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }
        public static void CheckFolder(string folderName)
        {
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/" + folderName + "/")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/" + folderName + "/"));
            }
        }

        public static bool CheckSpecialCharater(string sMessage)
        {
            string[] lines = Regex.Split(sMessage, @"^[,.;?!_-=+#%]$", RegexOptions.IgnoreCase);
            return lines.Length > 1;
        }

        public static string CreateFile(string nameFile)
        {
            CheckFolder("Files");
            var path = @nameFile + ".xlsx";
            var sFileName = HttpContext.Current.Server.MapPath("~/Files/" + path);
            return sFileName;
        }

        public static bool CheckUnicode(string input)
        {
            var asciiBytesCount = Encoding.ASCII.GetByteCount(input);
            var unicodBytesCount = Encoding.UTF8.GetByteCount(input);
            return asciiBytesCount != unicodBytesCount;
        }

        public class SessionAuthorizeAttribute : AuthorizeAttribute
        {
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                return httpContext.Session != null && UserInformation.Session_User != null;
            }

            protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
            {
                filterContext.Result = new RedirectResult("/Home/Login");
            }
        }
        public class SessionCheckAccess : AuthorizeAttribute
        {
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                if (string.Equals(httpContext.Request.RequestContext.RouteData.Values["action"].ToString(), "Index"))
                    return CheckAccess();
                return true;
            }

            protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
            {
                filterContext.Result = new RedirectResult("/Home?id=NOT");
            }
        }

        public static string GetUrl()
        {
            var url = HttpContext.Current.Request.RawUrl;
            return url;
        }


        public static T Map<T>(object entity)
        {
            return BoHelper.TranslateObject<T>(entity);
        }

        public static List<T> Maps<T>(IEnumerable entity)
        {
            return BoHelper.TranslateListObject<T>(entity);
        }
        
        public static bool CheckAccess()
        {
            var bRt = true;
            var listQuyen = UserInformation.Session_User.ListChucNang;
            var cNode = AppConfig.GetCurModule();
            if (cNode != null)
            {
                var quyen = listQuyen.Count(p => string.Equals(p.IDChucNang, cNode.IDChucNang));
                bRt = quyen > 0;
            }
            return bRt;
        }

        #region Lay trang thai ban ghi

        /// <summary>
        /// Lấy trạng thái bản ghi dựa theo action và trạng thái bản ghi hiện tại
        /// </summary>
        /// <returns></returns>
        public static string LayTrangThaiBanGhi(DatabaseConstant.Action action, BusinessConstant.TrangThaiNghiepVu status)
        {
            string trangthai = "";
            switch (action)
            {
                case DatabaseConstant.Action.LUU_TAM:
                    if (status == BusinessConstant.TrangThaiNghiepVu.DA_DUYET)
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri();
                    }
                    else if (status == BusinessConstant.TrangThaiNghiepVu.CHO_DUYET)
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri();
                    }
                    else if (status == BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET)
                    {
                        trangthai = BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET);
                    }
                    else
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri();
                    }
                    break;
                case DatabaseConstant.Action.LUU:
                    if (status == BusinessConstant.TrangThaiNghiepVu.DA_DUYET)
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri();
                    }
                    else if (status == BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET)
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                    }
                    else
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                    }
                    break;
                case DatabaseConstant.Action.TRINH_DUYET:
                    if (status == BusinessConstant.TrangThaiNghiepVu.DA_DUYET)
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri();
                    }
                    else if (status == BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET)
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                    }
                    else
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                    }
                    break;
                case DatabaseConstant.Action.DUYET:
                    trangthai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    break;
                case DatabaseConstant.Action.THOAI_DUYET:
                    trangthai = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                    break;
                case DatabaseConstant.Action.TU_CHOI_DUYET:
                    trangthai = BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri();
                    break;
            }
            return trangthai;
        }

        /// <summary>
        /// Tạo trạng thái bản ghi dựa theo hành động và trạng thái hiện tại của bản ghi
        /// </summary>
        /// <param name="action">Hành động</param>
        /// <param name="trangthaiHienTai">Trạng thái hiện tại của bản ghi</param>
        /// <returns></returns>
        private static string TaoTrangThaiBanGhi(DatabaseConstant.Action action, string trangthaiHienTai)
        {
            if (string.IsNullOrEmpty(trangthaiHienTai)
                        || trangthaiHienTai == BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri()
                        || trangthaiHienTai == BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri()
                        || trangthaiHienTai == BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri()
                )
            {
                if (action == DatabaseConstant.Action.LUU_TAM)
                {
                    return BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri();
                }
                else if (action == DatabaseConstant.Action.LUU)
                {
                    return BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                }
            }
            else
            {
                if (trangthaiHienTai == BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri())
                {
                    if (action == DatabaseConstant.Action.LUU_TAM)
                    {
                        return BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri();
                    }
                    else if (action == DatabaseConstant.Action.LUU)
                    {
                        return BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                    }
                    else if (action == DatabaseConstant.Action.DUYET)
                    {
                        return BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    }
                    else if (action == DatabaseConstant.Action.TU_CHOI_DUYET)
                    {
                        return BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri();
                    }
                }
                else if (trangthaiHienTai == BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri())
                {
                    if (action == DatabaseConstant.Action.LUU_TAM)
                    {
                        // Lưu tạm sửa sau duyệt
                        return "";
                    }
                    else if (action == DatabaseConstant.Action.LUU)
                    {
                        return BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri();
                    }
                    else if (action == DatabaseConstant.Action.THOAI_DUYET)
                    {
                        return BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                    }
                }
                else if (trangthaiHienTai == BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri())
                {
                    if (action == DatabaseConstant.Action.LUU)
                    {
                        return BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri();
                    }
                    else if (action == DatabaseConstant.Action.LUU_TAM)
                    {
                        // Lưu tạm sửa sau duyệt
                        return "";
                    }
                    else if (action == DatabaseConstant.Action.DUYET)
                    {
                        return BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    }
                    else if (action == DatabaseConstant.Action.TU_CHOI_DUYET)
                    {
                        // Từ chối sửa sau duyệt
                        return "";
                    }
                }
                else
                {
                    if (action == DatabaseConstant.Action.LUU)
                    {
                        // Sửa sau duyệt
                        return BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri();
                    }
                    else if (action == DatabaseConstant.Action.LUU_TAM)
                    {
                        //Lưu tạm sửa sau duyệt
                        return "";
                    }
                }
            }
            return "";
        }

        #endregion
    }
}