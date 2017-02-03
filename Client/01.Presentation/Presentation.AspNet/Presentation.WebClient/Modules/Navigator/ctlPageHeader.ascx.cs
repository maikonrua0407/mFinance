using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presentation.WebClient.Business;

namespace Presentation.WebClient.Modules.Navigator
{
    public partial class ctlPageHeader : ControlBase
    {
        #region [User Functions]

        private void ValidateLogin()
        {
            if (null == AppConfig.LoginedUser)
            {
                if (Request.Url.LocalPath.IndexOf("Login.aspx") < 0)
                {
                    Response.Redirect("~/Login.aspx");
                }
            }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ValidateLogin();
            }
        }
    }
}