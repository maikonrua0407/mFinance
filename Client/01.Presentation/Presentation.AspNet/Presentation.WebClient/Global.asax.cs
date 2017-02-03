using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Presentation.WebClient.Business;
using Utilities.Common;

namespace Presentation.WebClient
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            CacheService.Instance();

            Presentation.Process.Common.ClientInitProcess objRead = new Presentation.Process.Common.ClientInitProcess();
            objRead.docThongTinCauHinhClient(0);
            LLanguage.ApplyLanguage(AppDomain.CurrentDomain.BaseDirectory + "languages", AppConfig.Language);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}