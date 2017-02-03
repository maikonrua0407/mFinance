using Presentation.WebClient.Business;
using Presentation.WebClient.Business.CustomControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utilities.Common;

namespace Presentation.WebClient.Modules.Navigator
{
    public partial class ctlHorizonalNavigator : ControlBase 
    {
        protected string NavigatorString = "";

        #region [User Methods]

        private void BuildComboPGD()
        {
            cboChiNhanh.Items.Clear();
            DataTable v_dt = AppConfig.LoginedUser.DSDonVi;
            if (null != v_dt)
            {
                for (int i = 0; i < v_dt.Rows.Count; i++)
                {
                    ListItem v_objItem = new ListItem();
                    v_objItem.Text = v_dt.Rows[i]["TEN_GDICH"].ToString();
                    v_objItem.Value = v_dt.Rows[i]["MA_DVI"].ToString();
                    cboChiNhanh.Items.Add(v_objItem);
                }
            }
        }

        private void InitForm()
        {
            Control v_ctlr = FindControlRecursive(Page, "LEFT_PANEL");
            if (null != v_ctlr)
            {
                chkToogle.Attributes.Add("onclick", "javascript:togglepanel('" + v_ctlr.ClientID + "')");
            }
            BuildNavigator();

            
        }

        private void BuildNavigator()
        {
            if (null != AppConfig.LoginedUser)
            {
                DataTable v_dt = AppConfig.LoginedUser.CayMenu;
                DataView v_dv = v_dt.DefaultView;
                v_dv.RowFilter = "URL LIKE '%TabID=" + AppConfig.TabID + "%'";
                if (v_dv.Count > 0)
                {
                    int v_iMenu = Convert.ToInt32(v_dv[0]["ID"]);
                    v_dv.RowFilter = "";

                    NavigatorString = GetParent(v_iMenu);
                }

                cboChiNhanh.Visible = true;
                BuildComboPGD();
            }
            else
            {
                cboChiNhanh.Visible = false;
            }

        }

        private string GetParent(int pv_iMenuID)
        {
            string v_strContent = "";

            DataTable v_dt = AppConfig.LoginedUser.CayMenu;
            DataView v_dv = v_dt.DefaultView;
            v_dv.RowFilter = "ID=" + pv_iMenuID.ToString();
            if (v_dv.Count > 0)
            {
                v_strContent += "<span class=\"divider\">&gt;</span>";
                if (DBNull.Value != v_dv[0]["URL_ICON"])
                {
                    v_strContent += "<span class=\"" + v_dv[0]["URL_ICON"].ToString() + "\"></span>";
                }
                if (DBNull.Value != v_dv[0]["URL"])
                {
                    v_strContent += "<a href=\"" + v_dv[0]["URL"].ToString().Replace("~", "") + "\">" + LanguageEngine.Instance().GetContent(LanguageType.TypeUI, v_dv[0]["TIEU_DE"].ToString()) + "</a>";
                }
                else
                {
                    v_strContent += "<span  class=\"text bold white\" >" + LanguageEngine.Instance().GetContent(LanguageType.TypeUI, v_dv[0]["TIEU_DE"].ToString()) + "</span>";
                }

                //Build cha
                if (DBNull.Value != v_dv[0]["ID_CNANG_CHA"])
                {
                    int v_iMenucha = Convert.ToInt32(v_dv[0]["ID_CNANG_CHA"]);
                    v_dv.RowFilter = "";
                    v_strContent = GetParent(v_iMenucha) + v_strContent;
                }
            }

            return v_strContent;
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            InitForm();
        }

        protected void cboChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (null != cboChiNhanh.SelectedValue)
            {
                AppConfig.LoginedUser.MaDonVi = cboChiNhanh.SelectedValue;
                AppConfig.LoginedUser.TenDonVi = cboChiNhanh.SelectedItem.Text;
                Response.Redirect("~/Default.aspx");
            }
        }
    }
}