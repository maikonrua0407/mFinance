using Presentation.Process;
using Presentation.WebClient.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utilities.Common;


namespace Presentation.WebClient.Modules.System
{
    public partial class ctlChangePass : ControlBase
    {
        #region [Protected Methods]

        private void InitForm()
        {
            tblChangePass.Visible = true;
            tblResult.Visible = false;
            txtOldPass.Text = "";
            txtNewPass.Text = "";
            txtConfirmPass.Text = "";
            lblErrorMsg.Text = "";
        }

        private bool Validate()
        {
            bool v_blRet = true;

            //if (txtNewPass.Text.Trim().Length < Convert.ToInt32(CacheService.Instance().GetSysvar("PASSWORDMINLENGTH","6")))
            //{
            //    lblErrorMsg.Text = CacheService.Instance().GetErrorDef(ErrorDef.USER_PASS_LESS_THAN_MIN);
            //    v_blRet = false;
            //} else if (txtNewPass.Text.Trim() != txtConfirmPass.Text.Trim())
            //{
            //    lblErrorMsg.Text = CacheService.Instance().GetErrorDef(ErrorDef.USER_PASS_CONFIRM_NOT_MATCHED);
            //    v_blRet = false;
            //}

            return v_blRet;
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                InitForm();
            }
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                string v_strResult = UserController.ChangePass(AppConfig.LoginedUser.UserName, txtOldPass.Text.Trim(), txtNewPass.Text.Trim());
                if (v_strResult == "OK")
                {
                    HttpContext.Current.Session["LoginedUser"] = null;
                    HttpContext.Current.Session.Abandon();
                    HttpContext.Current.Response.Cookies["LoginedUser"].Expires = DateTime.Now;

                    Response.AddHeader("REFRESH", "5;URL=Login.aspx");

                    tblChangePass.Visible = false;
                    tblResult.Visible = true;
                }
                else
                {
                    lblErrorMsg.Text = CacheService.Instance().GetErrorDef(Convert.ToInt64(v_strResult.Replace("ERROR|", "")));
                }
            }
        }
        
    }
}