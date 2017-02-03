using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Xml;
using Utilities.Common;
using Presentation.Process;

namespace Presentation.WebClient.Business
{ 
    public class BaoCaoController
    {
        public static DataTable GetDMBaoCao()
        {
            DataTable v_dt = null;
            string v_strRet = "ERROR|" + ErrorDef.SYSTEM_ERROR.ToString();
            XmlDocument v_objDoc = LXMLMessage.InitXmlMessageTemplate();
            LXMLMessage.SetAttribute(ref v_objDoc, LXMLAttribute.FunctionName, "BusinessServices.BaoCao.BS_BaoCao.ProcessXML");
            DataTable v_dtParam = LXMLMessage.InitRequestParamTable();
            v_dtParam.Rows.Add(new string[] { "FUNCTIONNAME", "GetDMBaoCao" });

            try
            {
                v_objDoc.DocumentElement.SelectSingleNode("RequestInfo").InnerXml = "<![CDATA[" + LXMLMessage.ConvertDataTableToXml(v_dtParam) + "]]>";

                string v_strDocument = LXMLMessage.ConvertDocumentToString(v_objDoc);
                v_strRet = WebProcess.Instance().SendMessage(v_strDocument);
            }
            catch (Exception ex)
            {
                v_strRet = "ERROR|" + ErrorDef.SYSTEM_ERROR.ToString();
            }

            if (v_strRet.StartsWith("OK|"))
            {
                v_dt = LXMLMessage.ConvertXmlToDataTable(v_strRet.Substring(3));
            }

            return v_dt;
        }

        public static DataTable GetDSBaoCao(string pv_strMaDM)
        {
            DataTable v_dt = null;
            string v_strRet = "ERROR|" + ErrorDef.SYSTEM_ERROR.ToString();
            XmlDocument v_objDoc = LXMLMessage.InitXmlMessageTemplate();
            LXMLMessage.SetAttribute(ref v_objDoc, LXMLAttribute.FunctionName, "BusinessServices.BaoCao.BS_BaoCao.ProcessXML");
            DataTable v_dtParam = LXMLMessage.InitRequestParamTable();
            v_dtParam.Rows.Add(new string[] { "FUNCTIONNAME", "GetDSBaoCao" });
            v_dtParam.Rows.Add(new string[] { "MADM", pv_strMaDM });

            try
            {
                v_objDoc.DocumentElement.SelectSingleNode("RequestInfo").InnerXml = "<![CDATA[" + LXMLMessage.ConvertDataTableToXml(v_dtParam) + "]]>";

                string v_strDocument = LXMLMessage.ConvertDocumentToString(v_objDoc);
                v_strRet = WebProcess.Instance().SendMessage(v_strDocument);
            }
            catch (Exception ex)
            {
                v_strRet = "ERROR|" + ErrorDef.SYSTEM_ERROR.ToString();
            }

            if (v_strRet.StartsWith("OK|"))
            {
                v_dt = LXMLMessage.ConvertXmlToDataTable(v_strRet.Substring(3));
            }

            return v_dt;
        }
    }
}