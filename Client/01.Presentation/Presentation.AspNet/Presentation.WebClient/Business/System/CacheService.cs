using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Data;
using Presentation.WebClient.Business;
using System.IO;
using System.Xml.Linq;
using Utilities.Common;
using Presentation.Process.Common;


namespace Presentation.WebClient.Business
{
    public class CacheService
    {
        #region [Variables]

        private static CacheService mv_objCache = null;

        private PortalInfo mv_objPortal = null;
        private Hashtable mv_htSysvar = new Hashtable();
        private Hashtable mv_htTemplateDef=new Hashtable();
        private Hashtable mv_htContainerDef = new Hashtable();
        private Hashtable mv_htModuleDef = new Hashtable();
        private Hashtable mv_htErrorDef = new Hashtable();
        private Hashtable mv_htPageConfig = new Hashtable();
        private Hashtable mv_htPageConfigByCode = new Hashtable();
        private DataTable mv_dtPageModule = null;
        private DataTable mv_dtPageModuleDefault = null;
        private Hashtable mv_htPageModuleParam = new Hashtable();

        #endregion

        #region [Properties]

        public Hashtable TemplateDef
        {
            get
            {
                return mv_htTemplateDef;
            }
        }

        public Hashtable ContainerDef
        {
            get
            {
                return mv_htContainerDef;
            }
        }

        public Hashtable ModuleDef
        {
            get
            {
                return mv_htModuleDef;
            }
        }

        public Hashtable ErrorDef
        {
            get
            {
                return mv_htErrorDef;
            }
        }

        public Hashtable PageConfig
        {
            get
            {
                return mv_htPageConfig;
            }
        }

        public DataTable PageModule
        {
            get
            {
                return mv_dtPageModule;
            }
        }

        public DataTable PageModuleDefault
        {
            get
            {
                return mv_dtPageModuleDefault;
            }
        }

        public Hashtable PageModuleParam
        {
            get
            {
                return mv_htPageModuleParam;
            }
        }

        public PortalInfo CurrentPortal
        {
            get
            {
                return mv_objPortal;
            }
        }

        #endregion

        #region [Custom methods]

        protected void LoadConfig()
        {
            string v_strConfigPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config\\Config.conf";
            MemoryStream mStream = LSecurity.DESDecryptFile(v_strConfigPath, @"!=Q|A'Z?");
            XElement xml = XElement.Load(mStream);
            DataTable dt = LDatatable.XElementToDataTable(xml);
            if (dt.Rows.Count > 0)
            {
                ClientInformation.DataTableConfig = dt;
                if (dt.Columns.Contains("Company")) ClientInformation.Company = dt.Rows[0]["Company"].ToString();
                else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so Company trong file cau hinh");

                if (dt.Columns.Contains("ClientType")) ClientInformation.ClientType = dt.Rows[0]["ClientType"].ToString();
                else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so ClientType trong file cau hinh");

                if (dt.Columns.Contains("IconName")) ClientInformation.IconName = ClientInformation.ImagesDir + "\\" + dt.Rows[0]["IconName"].ToString() + ".ico";
                else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so IconName trong file cau hinh");

                if (dt.Columns.Contains("ShortName")) ClientInformation.ShortName = dt.Rows[0]["ShortName"].ToString();
                else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so ShortName trong file cau hinh");

                if (dt.Columns.Contains("FullName")) ClientInformation.FullName = dt.Rows[0]["FullName"].ToString();
                else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so FullName trong file cau hinh");

                if (dt.Columns.Contains("Theme")) ClientInformation.Theme = dt.Rows[0]["Theme"].ToString();
                else
                {
                    ClientInformation.Theme = "default";
                    Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so Theme trong file cau hinh");
                }
                if (dt.Columns.Contains("LanguageList")) ClientInformation.LanguageList = dt.Rows[0]["LanguageList"].ToString();
                else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so LanguageList trong file cau hinh");

                if (dt.Columns.Contains("ServerIP")) ClientInformation.ServerIP = dt.Rows[0]["ServerIP"].ToString();
                else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so ServerIP trong file cau hinh");

                if (dt.Columns.Contains("ServerPort")) ClientInformation.ServerPort = dt.Rows[0]["ServerPort"].ToString();
                else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so ServerPort trong file cau hinh");

                ClientInformation.IpAddress = "";
                ClientInformation.MacAddress = "";
            }
        }

        protected void LoadPageConfig()
        {
            string v_strXmlPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config\\SystemParam\\";
            DataSet v_ds = new DataSet();
            v_ds.ReadXml(v_strXmlPath + "PageConfig.xml");
            if (v_ds.Tables.Count > 0)
            {
                foreach (DataTable v_dt in v_ds.Tables)
                {
                    switch (v_dt.TableName)
                    {
                        case "Sysvar":
                            {
                                mv_htSysvar.Clear();

                                for (int i = 0; i < v_dt.Rows.Count; i++)
                                {
                                    if (!mv_htSysvar.ContainsKey(v_dt.Rows[i]["ParamName"].ToString().Trim().ToUpper()))
                                    {
                                        mv_htSysvar.Add(v_dt.Rows[i]["ParamName"].ToString().Trim().ToUpper(), v_dt.Rows[i]["ParamValue"].ToString().Trim());
                                    }
                                }
                                break;
                            }
                        case "PortalConfig":
                            {
                                mv_objPortal = new PortalInfo();
                                mv_objPortal.Title = v_dt.Rows[0]["Title"].ToString();
                                mv_objPortal.Keywords = v_dt.Rows[0]["Keywords"].ToString();
                                mv_objPortal.Descriptions = v_dt.Rows[0]["Descriptions"].ToString();
                                mv_objPortal.CopyRight = v_dt.Rows[0]["CopyRight"].ToString();
                                mv_objPortal.Address = v_dt.Rows[0]["Address"].ToString();
                                mv_objPortal.ICon = v_dt.Rows[0]["ICon"].ToString();
                                mv_objPortal.TemplateBase = v_dt.Rows[0]["TemplateBase"].ToString();

                                break;
                            }
                        case "ModuleDef":
                            {
                                mv_htModuleDef.Clear();

                                for (int i = 0; i < v_dt.Rows.Count; i++)
                                {
                                    if (!mv_htModuleDef.ContainsKey(v_dt.Rows[i]["ModuleID"].ToString().Trim().ToUpper()))
                                    {
                                        mv_htModuleDef.Add(v_dt.Rows[i]["ModuleID"].ToString().Trim().ToUpper(), v_dt.Rows[i]);
                                    }
                                }
                                break;
                            }
                        case "ErrorDef":
                            {
                                mv_htErrorDef.Clear();

                                for (int i = 0; i < v_dt.Rows.Count; i++)
                                {
                                    if (!mv_htErrorDef.ContainsKey(v_dt.Rows[i]["ErrorCode"].ToString().Trim().ToUpper()))
                                    {
                                        mv_htErrorDef.Add(v_dt.Rows[i]["ErrorCode"].ToString().Trim().ToUpper(), v_dt.Rows[i]);
                                    }
                                }
                                break;
                            }
                        case "TemplateDef":
                            {
                                mv_htTemplateDef.Clear();

                                for (int i = 0; i < v_dt.Rows.Count; i++)
                                {
                                    if (!mv_htTemplateDef.ContainsKey(v_dt.Rows[i]["TemplateCode"].ToString().Trim().ToUpper()))
                                    {
                                        mv_htTemplateDef.Add(v_dt.Rows[i]["TemplateCode"].ToString().Trim().ToUpper(), v_dt.Rows[i]);
                                    }
                                }
                                break;
                            }
                        case "ContainerDef":
                            {
                                mv_htContainerDef.Clear();

                                for (int i = 0; i < v_dt.Rows.Count; i++)
                                {
                                    if (!mv_htContainerDef.ContainsKey(v_dt.Rows[i]["ContainerCode"].ToString().Trim().ToUpper()))
                                    {
                                        mv_htContainerDef.Add(v_dt.Rows[i]["ContainerCode"].ToString().Trim().ToUpper(), v_dt.Rows[i]);
                                    }
                                }
                                break;
                            }
                        case "PageConfig":
                            {
                                mv_htPageConfig.Clear();
                                mv_htPageConfigByCode.Clear();

                                for (int i = 0; i < v_dt.Rows.Count; i++)
                                {
                                    if (!mv_htPageConfig.ContainsKey(v_dt.Rows[i]["PageID"].ToString().Trim().ToUpper()))
                                    {
                                        mv_htPageConfig.Add(v_dt.Rows[i]["PageID"].ToString().Trim().ToUpper(), v_dt.Rows[i]);
                                    }

                                    if (!mv_htPageConfigByCode.ContainsKey(v_dt.Rows[i]["PageCode"].ToString().Trim().ToUpper()))
                                    {
                                        mv_htPageConfigByCode.Add(v_dt.Rows[i]["PageCode"].ToString().Trim().ToUpper(), v_dt.Rows[i]);
                                    }
                                }
                                break;
                            }
                        case "PageModule":
                            {
                                mv_dtPageModule = v_dt;
                                break;
                            }
                        case "PageModuleDefault":
                            {
                                mv_dtPageModuleDefault = v_dt;
                                break;
                            }
                        case "PageModuleParam":
                            {
                                mv_htPageModuleParam.Clear();
                                DataView v_dv = new DataView(v_dt);
                                DataTable v_dtDistinct = v_dv.ToTable(true, "PageModuleID");

                                if (v_dtDistinct.Rows.Count > 0)
                                {
                                    for (int i = 0; i < v_dtDistinct.Rows.Count; i++)
                                    {
                                        DataView v_dvFilter = v_dt.DefaultView;
                                        v_dvFilter.RowFilter = "PageModuleID='" + v_dtDistinct.Rows[i]["PageModuleID"].ToString() + "'";
                                        if (!mv_htPageModuleParam.ContainsKey(v_dtDistinct.Rows[i]["PageModuleID"].ToString()))
                                        {
                                            mv_htPageModuleParam.Add(v_dtDistinct.Rows[i]["PageModuleID"].ToString(), v_dvFilter.ToTable());
                                        }
                                        v_dvFilter.RowFilter = "";
                                    }
                                }
                                break;
                            }
                        default:
                            {
                                break;
                            }

                    }
                }
            }
        }

        public string NavigatePage(string pv_strPageCode,string pv_strQuery)
        {
            string v_strUrl = "";

            if (mv_htPageConfigByCode.ContainsKey(pv_strPageCode.ToUpper().Trim()))
            {
                DataRow v_dr = (DataRow)mv_htPageConfigByCode[pv_strPageCode.ToUpper().Trim()];
                v_strUrl = "~/Default.aspx?TabID=" + v_dr["PageID"].ToString();
            }

            if (pv_strQuery.Length > 0)
            {
                v_strUrl = v_strUrl + "&" + pv_strQuery;
            }

            return v_strUrl;
        }

        #endregion

        #region [Constructor]

        private CacheService()
        {
            //Load config file
            //LoadConfig();
            LoadPageConfig();
        }

        public static CacheService Instance()
        {
            if (null == mv_objCache)
            {
                mv_objCache = new CacheService();
            }

            return mv_objCache;
        }

        #endregion

        #region [Methods]

        public string GetSysvar(string pv_strVarName, string pv_strDefVal)
        {
            string v_strRet = pv_strDefVal;

            if (mv_htSysvar.ContainsKey(pv_strVarName.Trim().ToUpper()))
            {
                v_strRet = mv_htSysvar[pv_strVarName.Trim().ToUpper()].ToString();
            }

            return v_strRet;
        }

        public string GetErrorDef(long pv_iErrorCode)
        {
            string v_strRet = "Unknown error";

            if (mv_htErrorDef.ContainsKey(pv_iErrorCode.ToString()))
            {
                v_strRet = ((DataRow)mv_htErrorDef[pv_iErrorCode.ToString()])[AppConfig.Language.ToUpper() + "Desc"].ToString();
            }

            return v_strRet;
        }

        public string GetErrorDefList(string pv_strErrorCode)
        {
            string v_strRet = "Unknown error";
            string[] v_arrError = pv_strErrorCode.Split(',');
            if (v_arrError.GetLength(0) > 0)
            {
                for (int i = 0; i < v_arrError.GetLength(0); i++)
                {
                    if (mv_htErrorDef.ContainsKey(v_arrError[i]))
                    {
                        v_strRet = ((DataRow)mv_htErrorDef[v_arrError[i]])[AppConfig.Language.ToUpper() + "Desc"].ToString() + "\r\n";
                    }
                }
            }

            if (v_strRet.EndsWith("\r\n")) v_strRet = v_strRet.Substring(0, v_strRet.Length - 4);

            return v_strRet;
        }

        #endregion
    }
}