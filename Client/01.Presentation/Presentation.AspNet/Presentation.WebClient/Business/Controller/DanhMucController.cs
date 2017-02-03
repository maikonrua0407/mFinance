using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Xml;
using Utilities.Common;
using Presentation.Process;


namespace Presentation.WebClient.Business
{
    public class DanhMucController
    {

        public static string GetCumByID(int pv_ICumID)
        {
            string v_strRet = "ERROR|" + ErrorDef.SYSTEM_ERROR.ToString();
            XmlDocument v_objDoc = LXMLMessage.InitXmlMessageTemplate();
            LXMLMessage.SetAttribute(ref v_objDoc, LXMLAttribute.FunctionName, "BusinessServices.DanhMuc.BS_DM_CUM.ProcessXML");
            DataTable v_dtParam = LXMLMessage.InitRequestParamTable();
            v_dtParam.Rows.Add(new string[] { "FUNCTIONNAME", "getByID" });
            v_dtParam.Rows.Add(new string[] { "ID", pv_ICumID.ToString() });

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
                v_strRet = v_strRet.Substring(3);
            }

            return v_strRet;
        }

        public static DataTable GetDSCum(string pv_strUserName, string pv_strMaDonViQL, string pv_strMaDV, string pv_strLoai)
        {
            DataTable v_dt = null;
            string v_strRet = "ERROR|" + ErrorDef.SYSTEM_ERROR.ToString();
            XmlDocument v_objDoc = LXMLMessage.InitXmlMessageTemplate();
            LXMLMessage.SetAttribute(ref v_objDoc, LXMLAttribute.FunctionName, "BusinessServices.TruyVan.BS_TruyVan.TruyVanXML");
            DataTable v_dtParam = LXMLMessage.InitRequestParamTable();
            v_dtParam.Rows.Add(new string[] { "OBJECTNAME", "INQ.DS.CUM" });
            v_dtParam.Rows.Add(new string[] { "INQUIRYNAME", "DANH_SACH_01" });
            v_dtParam.Rows.Add(new string[] { "USER_NAME", pv_strUserName });
            v_dtParam.Rows.Add(new string[] { "MA_DVI_QLY", pv_strMaDonViQL });
            v_dtParam.Rows.Add(new string[] { "MA", pv_strMaDV });
            v_dtParam.Rows.Add(new string[] { "LOAI", pv_strLoai });

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

        public static long LuuTTCum(string[] pv_arrTTCum)
        {
            long v_lngError = ErrorDef.SYSTEM_SUCESS;
            string v_strRet = "";

            XmlDocument v_objDoc = LXMLMessage.InitXmlMessageTemplate();
            LXMLMessage.SetAttribute(ref v_objDoc, LXMLAttribute.FunctionName, "BusinessServices.DanhMuc.BS_DM_CUM.ProcessXML");
            DataTable v_dtParam = LXMLMessage.InitRequestParamTable();
            v_dtParam.Rows.Add(new string[] { "FUNCTIONNAME", "LuuTTCum" });

            try
            {
                if (pv_arrTTCum.GetLength(0) > 0)
                {
                    for (int i = 0; i < pv_arrTTCum.GetLength(0); i++)
                    {
                        DataRow v_dr = v_dtParam.NewRow();
                    }
                }
                v_objDoc.DocumentElement.SelectSingleNode("RequestInfo").InnerXml = "<![CDATA[" + LXMLMessage.ConvertDataTableToXml(v_dtParam) + "]]>";

                string v_strDocument = LXMLMessage.ConvertDocumentToString(v_objDoc);

                v_strRet = WebProcess.Instance().SendMessage(v_strDocument);
            }
            catch (Exception ex)
            {
                v_lngError =  ErrorDef.SYSTEM_ERROR;
            }

            return v_lngError;
        }
    }
}