using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Utilities.Common;
using Presentation.Process;
using System.Data;

namespace Presentation.WebClient.Business
{
    public class UserController
    {
        public static string Login(string pv_strUserName, string pv_strPass)
        {
            string v_strRet = "ERROR|" + ErrorDef.SYSTEM_ERROR.ToString();
            try
            {
                XmlDocument v_objDoc = LXMLMessage.InitXmlMessageTemplate();
                LXMLMessage.SetAttribute(ref v_objDoc, LXMLAttribute.FunctionName, "BusinessServices.System.SystemController.LoginUser");
                DataTable v_dtParam = LXMLMessage.InitRequestParamTable();
                v_dtParam.Rows.Add(new string[] { "UserName", pv_strUserName });
                v_dtParam.Rows.Add(new string[] { "UserPass", pv_strPass });

                v_objDoc.DocumentElement.SelectSingleNode("RequestInfo").InnerXml = "<![CDATA[" + LXMLMessage.ConvertDataTableToXml(v_dtParam) + "]]>";

                string v_strDocument = LXMLMessage.ConvertDocumentToString(v_objDoc);


                v_strRet = WebProcess.Instance().SendMessage(v_strDocument);
            }
            catch (Exception ex)
            {
                v_strRet = "ERROR|" + ErrorDef.SYSTEM_ERROR.ToString();
            }

            return v_strRet;
        }

        public static string LoginRequest(string pv_strUserName, string pv_strPass, ref string SessionId)
        {
            string v_strRet = "ERROR|" + ErrorDef.SYSTEM_ERROR.ToString();
            try
            {
                XmlDocument v_objDoc = LXMLMessage.InitXmlMessageTemplate();
                LXMLMessage.SetAttribute(ref v_objDoc, LXMLAttribute.FunctionName, "BusinessServices.System.SystemController.LoginUser");
                LXMLMessage.SetAttribute(ref v_objDoc, LXMLAttribute.Module, DatabaseConstant.Module.QTHT.getValue());
                LXMLMessage.SetAttribute(ref v_objDoc, LXMLAttribute.Function, DatabaseConstant.Function.HT_LOGIN.getValue());
                LXMLMessage.SetAttribute(ref v_objDoc, LXMLAttribute.Action, DatabaseConstant.Action.DANG_NHAP.getValue());
                LXMLMessage.SetAttribute(ref v_objDoc, LXMLAttribute.UserName, pv_strUserName);
                DataTable v_dtParam = LXMLMessage.InitRequestParamTable();
                v_dtParam.Rows.Add(new string[] { "UserName", pv_strUserName });
                v_dtParam.Rows.Add(new string[] { "UserPass", pv_strPass });

                v_objDoc.DocumentElement.SelectSingleNode("RequestInfo").InnerXml = "<![CDATA[" + LXMLMessage.ConvertDataTableToXml(v_dtParam) + "]]>";

                string v_strDocument = LXMLMessage.ConvertDocumentToString(v_objDoc);


                v_strRet = WebProcess.Instance().SendRequest(v_strDocument, ref SessionId);
            }
            catch (Exception ex)
            {
                v_strRet = "ERROR|" + ErrorDef.SYSTEM_ERROR.ToString();
            }

            return v_strRet;
        }

        public static string ChangePass(string pv_strUserName, string pv_strPass, string pv_strNewPass)
        {
            string v_strRet = "ERROR|" + ErrorDef.SYSTEM_ERROR.ToString();
            try
            {
                XmlDocument v_objDoc = LXMLMessage.InitXmlMessageTemplate();
                LXMLMessage.SetAttribute(ref v_objDoc, LXMLAttribute.FunctionName, "BusinessServices.System.SystemController.ChangeUserPass");
                DataTable v_dtParam = LXMLMessage.InitRequestParamTable();
                v_dtParam.Rows.Add(new string[] { "UserName", pv_strUserName });
                v_dtParam.Rows.Add(new string[] { "UserPass", LSecurity.MD5Encrypt(pv_strPass) });
                v_dtParam.Rows.Add(new string[] { "NewPass", LSecurity.MD5Encrypt(pv_strNewPass) });

                v_objDoc.DocumentElement.SelectSingleNode("RequestInfo").InnerXml = "<![CDATA[" + LXMLMessage.ConvertDataTableToXml(v_dtParam) + "]]>";

                string v_strDocument = LXMLMessage.ConvertDocumentToString(v_objDoc);

                v_strRet = WebProcess.Instance().SendMessage(v_strDocument);
            }
            catch (Exception ex)
            {
                v_strRet = "ERROR|" + ErrorDef.SYSTEM_ERROR.ToString();
            }

            return v_strRet;
        }
    }
}