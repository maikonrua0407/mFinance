using PresentationAspNet.MVC.App_Start;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.UI.WebControls;
using Presentation.Process.Common;
using Utilities.Common;

namespace PresentationAspNet.MVC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static string MaVach = "";
        protected void Application_Start()
        {
            DefaultSettings.BaseDirectory = Server.MapPath("/");
            AreaRegistration.RegisterAllAreas();
            // Tell WebApi to use our custom Ioc (Ninject)
            //IocConfig.RegisterIoc(GlobalConfiguration.Configuration);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());
            AppConfig.LoadModule();

            // set culture
            //System.Threading.Thread.CurrentThread.CurrentCulture =  CultureInfo.GetCultureInfo("vi-VN");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("vi-VN");
        }

        protected void Application_End()
        {
            //  Code that runs on application shutdown
            Session.RemoveAll();
            UserInformation.Session_User = null;
            ClientInformation.Release();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Code that runs when an unhandled error occurs

            Session.RemoveAll();
            UserInformation.Session_User = null;
            ClientInformation.Release();
            Response.Redirect(Server.MapPath("/") + "Login");
        }
    }
}