using Presentation.WebClient.Business;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace Presentation.WebClient.Modules.Navigator
{
    public partial class ctlLeftMenu : ControlBase
    {
        #region [Custom methods]

        private string LoadSubMenu(int pv_iParentID, int pv_iIndent)
        {
            StringBuilder v_sbSub = new StringBuilder();

            DataView v_dv = AppConfig.LoginedUser.CayMenu.DefaultView;
            v_dv.RowFilter = "";
            v_dv.RowFilter = "ID_CNANG_CHA=" + pv_iParentID.ToString();

            if (v_dv.Count > 0)
            {
                for (int i = 0; i < v_dv.Count; i++)
                {
                    v_sbSub.AppendLine("<div class=\"menulinelv" + pv_iIndent.ToString() + "\" >");
                    if ((v_dv[i]["URL_ICON"] != DBNull.Value) && (v_dv[i]["URL_ICON"].ToString().Trim().Length > 0))
                    {
                        v_sbSub.AppendLine("<span  class=\"" + v_dv[i]["URL_ICON"].ToString() + "\" ></span>");
                    }
                    if ((v_dv[i]["URL"] != DBNull.Value) && (v_dv[i]["URL"].ToString().Trim().Length > 0))
                    {
                        if (v_dv[i]["URL_TYPE"].ToString() == "Popup")
                        {
                            v_sbSub.AppendLine("<a class=\"menulink\" href=\"#\" onclick=\"javascript:showpopup('" + v_dv[i]["URL"].ToString().Replace("~", "") + "', window.outerWidth - 120, window.outerHeight - 100)\" >" + LanguageEngine.Instance().GetContent(LanguageType.TypeUI, v_dv[i]["TIEU_DE"].ToString()) + "</a>");
                        }
                        else
                        {
                            v_sbSub.AppendLine("<a class=\"menulink\" href=\"" + v_dv[i]["URL"].ToString().Replace("~", "") + "\" >" + LanguageEngine.Instance().GetContent(LanguageType.TypeUI, v_dv[i]["TIEU_DE"].ToString()) + "</a>");
                        }
                    }
                    else
                    {
                        v_sbSub.AppendLine("<span  class=\"text bold\" >" + LanguageEngine.Instance().GetContent(LanguageType.TypeUI, v_dv[i]["TIEU_DE"].ToString()) + "</span>");
                    }
                    v_sbSub.AppendLine("</div>");
                    v_sbSub.Append(LoadSubMenu(Convert.ToInt32(v_dv[i]["ID"]), pv_iIndent + 1));
                    v_dv.RowFilter = "ID_CNANG_CHA=" + pv_iParentID.ToString();
                }
            }

            v_dv.RowFilter = "";
            return v_sbSub.ToString();
        }

        private void InitMenu()
        {
            StringBuilder v_sbMenu = new StringBuilder();

            if (null != AppConfig.LoginedUser.CayMenu)
            {
                int v_iRootMenu = 0;
                DataView v_dv = AppConfig.LoginedUser.CayMenu.DefaultView;
                v_dv.RowFilter = "URL LIKE '%TabID=" + AppConfig.TabID + "%'";
                if (v_dv.Count > 0)
                {
                    v_iRootMenu = Convert.ToInt32(v_dv[0]["ID"]);
                    v_dv.RowFilter = "";
                    v_iRootMenu = FindRootMenu(v_iRootMenu);
                }
                v_dv.RowFilter = "";

                v_dv.RowFilter = "ID_CNANG_CHA IS NULL";
                for (int i = 0; i < v_dv.Count; i++)
                {
                    v_dv.RowFilter = "ID_CNANG_CHA IS NULL";

                    v_sbMenu.AppendLine("<div class=\"CBoxHeader\" >");
                    v_sbMenu.AppendLine("    <div class=\"mt5 ml15\" >");
                    if ((v_dv[i]["URL_ICON"] != DBNull.Value) && (v_dv[i]["URL_ICON"].ToString().Trim().Length > 0))
                    {
                        v_sbMenu.AppendLine("       <span class=\"" + v_dv[i]["URL_ICON"].ToString() + "\" ></span>");
                    }
                    if ((v_dv[i]["URL"] != DBNull.Value) && (v_dv[i]["URL"].ToString().Trim().Length > 0))
                    {
                        v_sbMenu.AppendLine("       <a class=\"menulink\" href=\"" + v_dv[i]["URL"].ToString().Replace("~", "") + "\" >" + LanguageEngine.Instance().GetContent(LanguageType.TypeUI, v_dv[i]["TIEU_DE"].ToString()) + "</a>");
                    }
                    else
                    {
                        v_sbMenu.AppendLine("       <span class=\"white bold\">" + LanguageEngine.Instance().GetContent(LanguageType.TypeUI, v_dv[i]["TIEU_DE"].ToString()) + "</span>");
                    }
                    if (v_iRootMenu == Convert.ToInt32(v_dv[i]["ID"]))
                    {
                        v_sbMenu.AppendLine("       <span class=\"icon-arrowup fr\" onclick=\"javascript:togglecontrol(this,'dv" + v_dv[i]["ID"].ToString() + "Content')\" ></span>");
                    }
                    else
                    {
                        v_sbMenu.AppendLine("       <span class=\"icon-arrowdown fr\" onclick=\"javascript:togglecontrol(this,'dv" + v_dv[i]["ID"].ToString() + "Content')\" ></span>");
                    }
                    v_sbMenu.AppendLine("    </div>");
                    v_sbMenu.AppendLine("</div>");
                    if (v_iRootMenu == Convert.ToInt32(v_dv[i]["ID"]))
                    {
                        v_sbMenu.AppendLine("<div id='dv" + v_dv[i]["ID"].ToString() + "Content' class=\"CBoxContent\" >");
                    }
                    else
                    {
                        v_sbMenu.AppendLine("<div id='dv" + v_dv[i]["ID"].ToString() + "Content' class=\"CBoxContent\" style=\"display:none\" >");
                    }
                    v_sbMenu.Append(LoadSubMenu(Convert.ToInt32(v_dv[i]["ID"]), 1));
                    v_sbMenu.AppendLine("</div>");

                    v_dv.RowFilter = "ID_CNANG_CHA IS NULL";
                }
                v_dv.RowFilter = "";
            }

            lstMenu.Text = v_sbMenu.ToString();
        }

        private int FindRootMenu(int MenuID)
        {
            int v_iRootMenu = MenuID;

            DataTable v_dt = AppConfig.LoginedUser.CayMenu;
            DataView v_dv = v_dt.DefaultView;
            v_dv.RowFilter = "ID=" + MenuID.ToString();
            if (v_dv.Count > 0)
            {
                if (DBNull.Value != v_dv[0]["ID_CNANG_CHA"])
                {
                    int v_iParentMenu = Convert.ToInt32(v_dv[0]["ID_CNANG_CHA"]);
                    v_dv.RowFilter = "";
                    v_iRootMenu = FindRootMenu(v_iParentMenu);
                }
                v_dv.RowFilter = "";
            }

            return v_iRootMenu;
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                InitMenu();
            }
        }
    }
}