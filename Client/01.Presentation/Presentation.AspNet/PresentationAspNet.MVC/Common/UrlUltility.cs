using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Presentation.Process.ZAMainAppServiceRef;
using Utilities.Common;

namespace PresentationAspNet.MVC
{
    public class UrlUltility
    {
        public static string GetSubDomain()
        {
            var url = HttpContext.Current.Request.Url;
            if (url.HostNameType == UriHostNameType.Dns &&
                !url.Host.Equals("localhost", StringComparison.CurrentCultureIgnoreCase))
            {
                var nodes = Regex.Replace(url.Host, "www.", "", RegexOptions.IgnoreCase).Split('.');
                return nodes.Count() > 2 ? nodes[0] : "stt";
            }
            return "stt";
        }

        public static string GetUrlWithoutQueryString()
        {
            var rtn = "";
            var url = HttpContext.Current.Request.RawUrl;
            var strSpl = url.Split('?');
            if (strSpl.Length > 0)
            {
                rtn = strSpl[0];
            }
            return rtn;
        }

        public static string GetUrlModuleControl()
        {
            var controller = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
            var module = HttpContext.Current.Request.RequestContext.RouteData.Values["module"].ToString();
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var url = urlHelper.Action("Index", controller, new { module = module });
            return url;
        }


        public static ChucNangDto GetSingleModule(string url)
        {
            var modules = UserInformation.Session_User.ListChucNang;
            if (modules == null || !modules.Any() || url == null)
                return null;
            var single =
                modules.FirstOrDefault(
                    m => string.Equals(url.ToLower(), (string.IsNullOrEmpty(m.Url) ? "" : m.Url).ToLower()));
            if (single.IsNullOrEmpty())
            {
                var md = modules.Where(m => url.ToLower().Contains((string.IsNullOrEmpty(m.Url) ? "" : m.Url).ToLower())).OrderByDescending(m => m.Url).ToList();
                single = md.First();
            }

            return single;
        }
    }
}