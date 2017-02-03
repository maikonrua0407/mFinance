using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Utilities.Common;

namespace Presentation.WebClient.Business
{
    public partial class TemplateBase : UserControl
    {


        #region [Constructors]

        public TemplateBase()
            : base()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us", false);
            this.Init += new EventHandler(Page_Init);
        }

        public void Page_Init(object sender, EventArgs e)
        {
            string v_strPanelCode = "", v_strPageModuleID = "", v_strModuleID = "", v_strContainerCode = "",
                v_strRightCode="",v_strRightList="",v_strModuleTitle="",v_strIconClass="";

            DataRow v_dr = (DataRow)CacheService.Instance().PageConfig[AppConfig.TabID];
            if (null != v_dr)
            {
                //Load default
                DataTable v_dtMD = CacheService.Instance().PageModuleDefault;
                if ((null != v_dtMD) && (v_dtMD.Rows.Count > 0))
                {
                    for (int i = 0; i < v_dtMD.Rows.Count; i++)
                    {
                        v_strModuleID = v_dtMD.Rows[i]["ModuleID"].ToString().Trim();
                        v_strPanelCode = v_dtMD.Rows[i]["PanelCode"].ToString().ToUpper().Trim();
                        v_strContainerCode = v_dtMD.Rows[i]["ContainerCode"].ToString().ToUpper().Trim();
                        v_strModuleTitle = v_dtMD.Rows[i]["ModuleTitle"].ToString().Trim();
                        v_strIconClass = v_dtMD.Rows[i]["ModuleIconClass"].ToString().Trim();

                        LoadModule(v_strModuleID, v_strPanelCode, v_strContainerCode, v_strModuleTitle, v_strIconClass, "","");
                    }
                }

                //Load assigned module
                DataTable v_dtMA = CacheService.Instance().PageModule;
                DataView v_dv = v_dtMA.DefaultView;
                v_dv.RowFilter = "PageID='" + AppConfig.TabID + "'";
                v_dv.Sort = "Orders";

                if (v_dv.Count > 0)
                {
                    for (int i = 0; i < v_dv.Count; i++)
                    {
                        v_strPageModuleID = v_dv[i]["PageModuleID"].ToString().ToUpper().Trim();
                        v_strModuleID = v_dv[i]["ModuleID"].ToString().ToUpper().Trim();
                        v_strPanelCode = v_dv[i]["PanelCode"].ToString().ToUpper().Trim();
                        v_strRightCode = v_dv[i]["RightCode"].ToString().ToUpper().Trim();
                        v_strModuleTitle = v_dv[i]["ModuleTitle"].ToString().ToUpper().Trim();
                        v_strContainerCode = v_dv[i]["ContainerCode"].ToString().ToUpper().Trim();
                        v_strIconClass = v_dv[i]["ModuleIconClass"].ToString().Trim();

                        if (v_strRightCode.Length > 0)
                        {
                            if (null != AppConfig.LoginedUser)
                            {
                                DataView v_dvMenu = AppConfig.LoginedUser.CayMenu.DefaultView;
                                v_dvMenu.RowFilter = "TIEU_DE='" + v_strRightCode.ToUpper().Trim() + "'";
                                if (v_dvMenu.Count <= 0)
                                {
                                    v_strRightList = (v_dvMenu[0]["TINH_NANG"]==DBNull.Value?"": v_dvMenu[0]["TINH_NANG"].ToString());
                                }
                                
                                v_dvMenu.RowFilter = "";
                            }
                        }
                        else
                        {
                            v_strRightList = "FullControl";
                        }
                        if (v_strRightList.Length>0)
                        {
                            LoadModule(v_strModuleID, v_strPanelCode, v_strContainerCode, v_strModuleTitle, v_strIconClass, v_strPageModuleID, v_strRightList);
                        }
                    }
                }
                v_dv.RowFilter = "";
            }

            
        }

        #endregion

        #region [Methods]

        protected void LoadModule(string pv_strModuleID, string pv_strPanelCode, string pv_strContainerCode,
                     string pv_strModuleTitle,string pv_strModuleIconClass,string pv_strPageModuleID,string pv_strRightList)
        {
            string v_strContainerSrc = "", v_strModuleSrc="";
            DataTable v_dtModuleParam = null;

            DataRow v_drContainer = (DataRow)CacheService.Instance().ContainerDef[pv_strContainerCode];
            if (null != v_drContainer)
            {
                v_strContainerSrc = v_drContainer["ContainerSrc"].ToString();
            }
            else
            {
                v_strContainerSrc = "DEFAULT";
            }

            DataRow v_drModuleDef = (DataRow)CacheService.Instance().ModuleDef[pv_strModuleID];
            if (null != v_drModuleDef)
            {
                v_strModuleSrc = v_drModuleDef["ModuleSrc"].ToString();
            }

            if (pv_strPageModuleID.Length > 0)
            {
                v_dtModuleParam = (DataTable)CacheService.Instance().PageModuleParam[pv_strPageModuleID];
            }

            if (v_strContainerSrc.Length > 0)
            {

                Control v_ctlPlaceHolder = null;
                if (pv_strPanelCode.Length > 0)
                {
                    v_ctlPlaceHolder = this.FindControl(pv_strPanelCode);
                }
                else
                {
                    v_ctlPlaceHolder = this.FindControl("CONTENT_PANEL");
                }

                if (null != v_ctlPlaceHolder)
                {
                    UserControl v_ctlContainer = (UserControl)this.LoadControl(v_strContainerSrc);
                    if (null != v_ctlContainer)
                    {
                        Control v_ctlModule = v_ctlContainer.LoadControl(v_strModuleSrc);
                        if ((null != v_ctlModule) && (v_ctlModule is ControlBase))
                        {
                            //Set Right
                            if (pv_strRightList.IndexOf("FullControl") >= 0)
                            {
                                ((ControlBase)v_ctlModule).CanAdd = true;
                                ((ControlBase)v_ctlModule).CanUpdate = true;
                                ((ControlBase)v_ctlModule).CanDelete = true;
                            }
                            else
                            {
                                if (pv_strRightList.IndexOf("Add") >= 0)
                                {
                                    ((ControlBase)v_ctlModule).CanAdd = true;
                                }
                                if (pv_strRightList.IndexOf("Modify") >= 0)
                                {
                                    ((ControlBase)v_ctlModule).CanUpdate = true;
                                }
                                if (pv_strRightList.IndexOf("Delete") >= 0)
                                {
                                    ((ControlBase)v_ctlModule).CanDelete = true;
                                }
                            }

                            //Set Title
                            if (pv_strModuleTitle.Length > 0)
                            {
                                Label lbl = (Label)v_ctlContainer.FindControl("lblContainerTitle");
                                if (null != lbl)
                                {
                                    lbl.Text = pv_strModuleTitle;
                                }
                            }

                            if (pv_strModuleIconClass.Trim().Length > 0)
                            {
                                HtmlControl v_objItem = (HtmlControl)v_ctlContainer.FindControl("spContainerIcon");
                                if (null != v_objItem)
                                {
                                    v_objItem.Attributes.Add("class", pv_strModuleIconClass);
                                }
                            }

                            //Set Param
                            if ((null!=v_dtModuleParam)&&(v_dtModuleParam.Rows.Count > 0))
                            {
                                Dictionary<string, string> arrParam = new Dictionary<string, string>();
                                for (int i = 0; i < v_dtModuleParam.Rows.Count; i++)
                                {
                                    arrParam.Add(v_dtModuleParam.Rows[i]["ParamName"].ToString(), v_dtModuleParam.Rows[i]["ParamValue"].ToString());
                                }

                                ((ControlBase)v_ctlModule).ModuleParams = arrParam;
                            }
                            //Add to container
                            Control v_ctlMainContent = v_ctlContainer.FindControl("MAIN_CONTENT");
                            if (null != v_ctlMainContent)
                            {
                                v_ctlMainContent.ID = pv_strModuleID + "_" + v_ctlMainContent.ID;
                                v_ctlMainContent.Controls.Add(v_ctlModule);
                            }
                        }
                        v_ctlPlaceHolder.Controls.Add(v_ctlContainer);
                    }
                }
            }
        }

        #endregion
    }
}