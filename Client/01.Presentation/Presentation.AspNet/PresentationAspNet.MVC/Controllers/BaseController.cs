using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities.Common;

namespace PresentationAspNet.MVC.Controllers
{
    [Common.SessionAuthorize]
    [Common.SessionCheckAccess]
    public class BaseController : Controller
    {
        public BaseController()
        {
            //var tem = UserInformation.Session_User;
            //if (Session != null)
            //{
            //    Session.Clear();
            //}
            //UserInformation.Session_User = tem;
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            //Log Exception e
            LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.ERR, filterContext.Exception);
        }
    }
}