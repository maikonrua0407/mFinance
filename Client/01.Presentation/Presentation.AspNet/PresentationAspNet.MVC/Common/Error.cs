using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PresentationAspNet.MVC
{
    public class Error
    {
        public string TitleError { get; set; }
        public string MessageError { get; set; }
        public string MenuParent { get; set; }
        public string MenuChild { get; set; }
    }
    public class ErrorNotHasData
    {
        public static Error CheckHasData(bool checkDiemGd)
        {
            Error info = null;
            if (UserInformation.Session_User == null)
            {
                return new Error
                {
                    TitleError = TitleError.SYSTEM,
                    MessageError = "Tài khoản đã hết phiên làm việc, vui lòng đăng nhập lại...",
                    MenuParent = string.Empty,
                    MenuChild = string.Empty
                };
            }
            return null;
        }
    }
    public class TitleError
    {
        public static string NOT_ROLE = "LỖI PHÂN QUYỀN"; 
        public static string PROCESS = "LỖI QUY TRÌNH";
        public static string SYSTEM = "LỖI HỆ THỐNG"; 
    }
}