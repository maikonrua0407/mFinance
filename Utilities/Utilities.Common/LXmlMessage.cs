using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Utilities.Common
{
    public static class LXMLAttribute
    {
        public const string FunctionName = "FUNCTIONNAME";
        public const string ResponseCode = "RESPONSECODE";

        public const string Module = "MODULE";
        public const string Function = "FUNCTION";
        public const string Action = "ACTION";

        public const string UserName = "USERNAME";
    }
    public static class LXMLMessage
    {
        public static XmlDocument InitXmlMessageTemplate()
        {
            string v_strRet = "<ObjMessage USERNAME=\"\" MODULE=\"\" FUNCTION=\"\" ACTION=\"\" FUNCTIONNAME=\"\" RESPONSECODE=\"\" >\n\r" +
                                "   <RequestInfo>\n\r" +
                                "   </RequestInfo>\n\r" +
                                "   <ResponseData>\n\r" +
                                "   </ResponseData>\n\r" +
                                "</ObjMessage>";
            XmlDocument v_objDoc = new XmlDocument();
            v_objDoc.LoadXml(v_strRet);
            return v_objDoc;
        }

        public static DataTable InitRequestParamTable()
        {
            DataTable v_dt = new DataTable();
            v_dt.TableName = "REQUESTPARAM";
            v_dt.Columns.Add("PARAMNAME", typeof(string));
            v_dt.Columns.Add("PARAMVALUE", typeof(string));
            return v_dt;
        }

        public static bool SetException(ref XmlDocument pv_objDoc, Exception pv_ex)
        {
            bool v_blRet = true;
            try
            {
                pv_objDoc.Attributes.GetNamedItem(LXMLAttribute.ResponseCode).Value = "-1";
                pv_objDoc.SelectSingleNode("ObjMessage/ResponseData").InnerXml = "<![CDATA[" + pv_ex.Message + " - " + pv_ex.StackTrace + "]]>";
            }
            catch (Exception ex)
            {
                v_blRet = false;
            }

            return v_blRet;
        }

        public static bool SetAttribute(ref XmlDocument pv_objDoc, string pv_attName, string pv_strValue)
        {
            bool v_blRet = true;

            try
            {
                pv_objDoc.DocumentElement.Attributes.GetNamedItem(pv_attName).Value = pv_strValue;
            }
            catch (Exception ex)
            { 
            }

            return v_blRet;
 
        }

        public static string ConvertDocumentToString(XmlDocument pv_objDoc)
        { 
            
            StringWriter v_sw = new StringWriter();
            XmlTextWriter v_stw = new XmlTextWriter(v_sw);

            pv_objDoc.WriteTo(v_stw);

            return v_sw.ToString();
        }

        public static string ConvertDataTableToXml(DataTable pv_dt)
        {
            string v_strRet = "";

            using (TextWriter v_tw = new StringWriter())
            {
                pv_dt.WriteXml(v_tw, XmlWriteMode.WriteSchema);
                v_strRet = v_tw.ToString();
            }

            return v_strRet;
        }

        public static DataTable ConvertXmlToDataTable(string pv_strXmlString)
        {
            
            DataSet v_ds = new DataSet();

            using (TextReader v_tw = new StringReader(pv_strXmlString.Replace("%0d","").Replace("%0a","")))
            {

                v_ds.ReadXml(v_tw,XmlReadMode.ReadSchema);
            }

            return v_ds.Tables[0];
        }
    }
}
