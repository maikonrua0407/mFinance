using System;
using System.Data;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilities.Common;
using Presentation.Process;

namespace Presentation.WebClient.Business
{
    public class TruyVanController
    {
        public static DataTable GetTreeDonVi(string pv_strUserName, string pv_strMaDonVi)
        {
            DataTable v_dt = null;
            string v_strRet = "ERROR|" + ErrorDef.SYSTEM_ERROR.ToString();
            XmlDocument v_objDoc = LXMLMessage.InitXmlMessageTemplate();
            LXMLMessage.SetAttribute(ref v_objDoc, LXMLAttribute.FunctionName, "BusinessServices.TruyVan.BS_TruyVan.TruyVanXML");
            DataTable v_dtParam = LXMLMessage.InitRequestParamTable();
            v_dtParam.Rows.Add(new string[] { "OBJECTNAME", "INQ.DS.CUM" });
            v_dtParam.Rows.Add(new string[] { "INQUIRYNAME", "TREE_01" });
            v_dtParam.Rows.Add(new string[] { "USER_NAME", pv_strUserName });
            v_dtParam.Rows.Add(new string[] { "MA_DVI_QLY", pv_strMaDonVi });
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


        public static DataTable GetComboSource(string pv_strMaTruyVan, List<String> pv_lstDieuKien)
        {
            DataTable v_dtRet = null;
            string v_strRet = "ERROR|" + ErrorDef.SYSTEM_ERROR.ToString();
            XmlDocument v_objDoc = LXMLMessage.InitXmlMessageTemplate();
            LXMLMessage.SetAttribute(ref v_objDoc, LXMLAttribute.FunctionName, "BusinessServices.TruyVan.BS_TruyVan.TruyVanXML");
            DataTable v_dtParam = LXMLMessage.InitRequestParamTable();
            v_dtParam.Rows.Add(new string[] { "OBJECTNAME", "INQ.COMBOBOX" });
            v_dtParam.Rows.Add(new string[] { "INQUIRYNAME", pv_strMaTruyVan });

            try
            {
                if (null != pv_lstDieuKien)
                {
                    for (int i = 0; i < pv_lstDieuKien.Count; i++)
                    {
                        v_dtParam.Rows.Add(new string[] { pv_lstDieuKien[i], pv_lstDieuKien[i] });
                    }
                }

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
                v_dtRet = LXMLMessage.ConvertXmlToDataTable(v_strRet.Substring(3));
            }

            return v_dtRet;
        }

        

        public static DataTable GetPopupData(string pv_strMaTruyVan,List<string> pv_lstDieuKien)
        {
            DataTable v_dtRet = null;

            string v_strRet = "ERROR|" + ErrorDef.SYSTEM_ERROR.ToString();
            XmlDocument v_objDoc = LXMLMessage.InitXmlMessageTemplate();
            LXMLMessage.SetAttribute(ref v_objDoc, LXMLAttribute.FunctionName, "BusinessServices.Popup.BS_Popup.ProcessXML");
            DataTable v_dtParam = LXMLMessage.InitRequestParamTable();
            v_dtParam.Rows.Add(new string[] { "FUNCTIONNAME", "Popup" });
            v_dtParam.Rows.Add(new string[] { "MATRUYVAN", pv_strMaTruyVan });

            try
            {
                if (null != pv_lstDieuKien)
                {
                    foreach (string v_strItem in pv_lstDieuKien)
                    {
                        v_dtParam.Rows.Add(new string[] { "DIEUKIEN", v_strItem });
                    }
                }
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
                v_objDoc.LoadXml("<root>" + v_strRet.Substring(3) + "</root>");

                v_dtRet = LXMLMessage.ConvertXmlToDataTable(v_objDoc.DocumentElement.SelectSingleNode("PopupData").InnerXml);
                DataRow v_dr = v_dtRet.NewRow();

                string[] v_arrHeader = v_objDoc.DocumentElement.SelectSingleNode("PopupHeader").InnerXml.Split("#".ToCharArray());
                for (int i = 0; i < v_arrHeader.GetLength(0); i++)
                {
                    string[] v_arrCol = v_arrHeader[i].Split('|');
                    v_dtRet.Columns[i].Caption = v_arrCol[0];
                    v_dtRet.Columns[i].ExtendedProperties.Add("width", v_arrCol[1]);
                }
            }

            return v_dtRet;
        }
    }
}