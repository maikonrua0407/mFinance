using Presentation.WebClient.Business;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Services;
namespace Presentation.WebClient
{
    public partial class Default : System.Web.UI.Page
    {
        #region [Properties]

        private static string mv_strRootPath = AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug\\", "").Replace("bin\\Release\\", "").Replace("bin\\", "");

        #endregion

        #region [Data Source]


        #endregion

        #region [Private Methods]

        protected void LoadParams()
        {
            if (null != CacheService.Instance().CurrentPortal)
            {
                this.Title = CacheService.Instance().CurrentPortal.Title;
                foreach (Control ctl in this.Header.Controls)
                {
                    if (ctl is HtmlMeta)
                    {
                        if (((HtmlMeta)ctl).Name.ToLower() == "keywords")
                        {
                            ((HtmlMeta)ctl).Content = CacheService.Instance().CurrentPortal.Keywords;
                        }
                        else if (((HtmlMeta)ctl).Name.ToLower() == "description")
                        {
                            ((HtmlMeta)ctl).Content = CacheService.Instance().CurrentPortal.Descriptions;
                        }
                    }
                    else if (ctl is HtmlLink)
                    {
                        if (((HtmlLink)ctl).Attributes["rel"] == "shortcut icon")
                        {
                            ((HtmlLink)ctl).Href = CacheService.Instance().CurrentPortal.ICon.Replace("~","");
                        }
                    }
                }

                //Them css,javascript theo theme
                string v_strAttach = "";
                string[] v_arrFile = Directory.GetFiles(Path.GetFullPath(CacheService.Instance().CurrentPortal.TemplateBase.Replace("~", mv_strRootPath)) + "Attach\\");
                if ((null != v_arrFile) && (v_arrFile.GetLength(0) > 0))
                {
                    for (int i = 0; i < v_arrFile.GetLength(0); i++)
                    {
                        switch (Path.GetExtension(v_arrFile[i]).ToUpper().Trim())
                        {
                            case ".CSS":
                                {
                                    v_strAttach += "<link href=\"" + CacheService.Instance().CurrentPortal.TemplateBase.Replace("~/", "/") + "/Attach/" + Path.GetFileName(v_arrFile[i]) + "\" type=\"text/css\" rel=\"Stylesheet\" />\n\r";
                                    break;
                                }
                            case ".JS":
                                {
                                    v_strAttach += "<script language=\"javascript\" type=\"text/javascript\" src=\"" + CacheService.Instance().CurrentPortal.TemplateBase.Replace("~/", "/") + "/Attach/" + Path.GetFileName(v_arrFile[i]) + "\" ></script>\n\r";
                                    break;
                                }
                        }
                    }
                }

                Literal v_el = new Literal();
                v_el.Text = v_strAttach;

                this.Header.Controls.Add(v_el);
            }
        }

        private void InitTemplate()
        {

            string v_strTemplate = "";
            

            DataRow v_dr = ((DataRow)CacheService.Instance().PageConfig[AppConfig.TabID]);

            if (null != v_dr)
            {
                v_strTemplate = v_dr["TemplateSrc"].ToString().Trim().ToUpper();
                if (v_strTemplate.Length <= 0) v_strTemplate = "DEFAULT";

                DataRow v_drTemplate = (DataRow)CacheService.Instance().TemplateDef[v_strTemplate];
                if (null != v_drTemplate)
                {
                    string v_strTemplatePath = v_drTemplate["TemplateSrc"].ToString().Trim();
                    if (v_strTemplatePath.Length > 0)
                    {
                        UserControl control = (UserControl)this.LoadControl(v_strTemplatePath);
                        if (null != control)
                        {
                            this.Form.Controls.Add(control);
                        }
                    }
                }
            }

            ViewState["controlsadded"] = true;
        }

        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);
            if (ViewState["controsladded"] == null)
            {
                LoadParams();
                InitTemplate();
            }
        }

        #endregion

        #region [Event Handles]

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadParams();
                InitTemplate();
            }
        }

        //protected override void OnInit(EventArgs e)
        //{
        //    base.OnInit(e);
        //    LoadParams();
        //    InitTemplate();
        //}

        #endregion
        
    }
  
}