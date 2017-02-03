using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data;
using Utilities.Common;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.Common;
using System.ServiceModel.Description;
using System.Web;

namespace Presentation.Process
{
    public class PopupProcess
    {
        /// <summary>
        /// Biến cục bộ chứa dịch vụ tiện ích của server
        /// </summary>
        private static PopupServiceClient Client { get; set; }

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        static PopupProcess()
        {
            //Client = new PopupServiceClient();
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.PopupService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.PopupService.layGiaTri());
            Client = new PopupServiceClient(basicHttpBinding, endpointAddress);
            
            foreach (var operationDescription in Client.Endpoint.Contract.Operations)
            {
                DataContractSerializerOperationBehavior dcsob =
                    operationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();
                if (dcsob != null)
                {
                    dcsob.MaxItemsInObjectGraph = 2147483646;
                }
            }
        }

        public void getPopupInformation(string maTruyVan, List<string> lstDieuKien = null)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.PopupService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            SimplePopupRequest request = Common.Utilities.PrepareRequest(new SimplePopupRequest());
            request.MaTruyVan = maTruyVan;
            if(lstDieuKien!=null)
            request.ListParamValue = lstDieuKien.ToArray<string>();

            // Lấy kết quả trả về
            SimplePopupResponse response = Client.getPopupInformation(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (ClientInformation.ClientType.Equals(ApplicationConstant.ClientType.DESKTOP.layGiaTri()))
                ClientInformation.SimplePopup = response;
            else
            {
                UserInformation userInfo = HttpContext.Current.Session["UserInformation"] as UserInformation;
                userInfo.SimplePopup = response;
            }
        }

        public void viewPopup()
        {
        }
    }
}
