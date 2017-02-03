using Presentation.Process.Common;
using Presentation.Process.ZAMainAppServiceRef;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Utilities.Common;
using Presentation.Process.WebProcessServiceRef;
using System.Xml;

namespace Presentation.Process
{
    public class WebProcess
    {

        #region [Variables]
        private static WebProcess mv_objProcess = null;
        private WebProcessServiceClient mv_objWebProcessSrv = null;
        
        #endregion

        #region [Properties]

        #endregion

        #region [Constructor]

        private void InitServiceProcess()
        {
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.WebProcessService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.WebProcessService.layGiaTri());
            mv_objWebProcessSrv = new WebProcessServiceClient(basicHttpBinding, endpointAddress);
            foreach (var operationDescription in mv_objWebProcessSrv.Endpoint.Contract.Operations)
            {
                DataContractSerializerOperationBehavior dcsob =
                    operationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();
                if (dcsob != null)
                {
                    dcsob.MaxItemsInObjectGraph = 2147483646;
                }
            }
        }

        private WebProcess()
        {//Prepare Client Information

            InitServiceProcess();
        }

        public static WebProcess Instance()
        {
            if (null == mv_objProcess)
            {
                mv_objProcess = new WebProcess();
            }

            return mv_objProcess;
        }

        #endregion

        #region [Methods]

        public string SendMessage(string pv_strMsg)
        {
            string v_strRet = pv_strMsg;

            long v_lngErrorCode = mv_objWebProcessSrv.MessageProcess(ref v_strRet);
            if (v_lngErrorCode == ErrorDef.SYSTEM_SUCESS)
            {
                XmlDocument v_objDoc = new XmlDocument();
                v_objDoc.LoadXml(v_strRet);
                v_strRet = "OK|" + v_objDoc.SelectSingleNode("ObjMessage/ResponseData").InnerXml;
            }
            else
            {
                v_strRet = "ERROR|" + v_lngErrorCode.ToString();
            }
            return v_strRet;
        }

        public string SendRequest(string pv_xmlMsg, ref string SessionId)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.WebProcessService.layGiaTri());


            // Khởi tạo và gán các giá trị cho request
            WebProcessServiceRef.WebRequest request = Common.Utilities.PrepareRequest(new WebProcessServiceRef.WebRequest());
            request.xmlMessageRequest = pv_xmlMsg;

            WebProcessServiceRef.WebResponse response = new WebProcessServiceRef.WebResponse();

            // Lấy kết quả trả về
            string v_xmlRet = pv_xmlMsg;
            response = mv_objWebProcessSrv.RequestProcess(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            v_xmlRet = response.xmlMessageResponse;
            long v_lngErrorCode = response.lngMessageResponse;

            if (v_lngErrorCode == ErrorDef.SYSTEM_SUCESS)
            {
                XmlDocument v_objDoc = new XmlDocument();
                v_objDoc.LoadXml(v_xmlRet);
                v_xmlRet = "OK|" + v_objDoc.SelectSingleNode("ObjMessage/ResponseData").InnerXml;
                SessionId = response.SessionId;
            }
            else
            {
                v_xmlRet = "ERROR|" + v_lngErrorCode.ToString();
            }
            return v_xmlRet;            
        }

        #endregion

    }
}
