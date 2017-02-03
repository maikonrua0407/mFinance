using Presentation.WebClient.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentation.WebClient.Modules.Navigator
{
    public partial class ctlUserInfoWidget : ControlBase
    {

        #region [User Functions]

        private void LoadUserInfo()
        {
            if (null != AppConfig.LoginedUser)
            {
                spDonVi.InnerText = AppConfig.LoginedUser.TenDonVi;
                dvFullName.InnerText = AppConfig.LoginedUser.FullName;
                dvNgayLamViec.InnerText = DateTime.ParseExact(AppConfig.LoginedUser.NgayLamViecHienTai, "yyyyMMdd", null).ToString("dd/MM/yyyy");
            }
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadUserInfo();
            }
        }

        protected void cmdLogout_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["LoginedUser"] = null;
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Response.Cookies["LoginedUser"].Expires = DateTime.Now;

            Response.Redirect("~/Login.aspx");
        }

        protected void cmdChangePass_Click(object sender, EventArgs e)
        {
            Response.Redirect(CacheService.Instance().NavigatePage("CHANGEPASS", ""),true);
        }
    }
}