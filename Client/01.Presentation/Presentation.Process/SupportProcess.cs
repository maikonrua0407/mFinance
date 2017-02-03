using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Common;
using Presentation.Process.TruyVanServiceRef;
using System.ServiceModel.Description;
using System.ServiceModel;
using Presentation.Process.SupportServiceRef;
using Presentation.Process.Common;

namespace Presentation.Process
{
    public class SupportProcess
    {
        public SupportProcess()
        {

        }

        private SupportServiceClient SupportServiceClient(ApplicationConstant.SystemService service)
        {
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.SupportService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.SupportService.layGiaTri());
            SupportServiceClient Client = new SupportServiceClient(basicHttpBinding, endpointAddress);

            return Client;
        }

        /// <summary>
        /// Khởi tạo service TruyVanService
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        private TruyVanServiceClient TruyVanServiceClient(ApplicationConstant.SystemService service)
        {
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(service.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(service.layGiaTri());
            TruyVanServiceClient Client = new TruyVanServiceClient(basicHttpBinding, endpointAddress);

            foreach (var operationDescription in Client.Endpoint.Contract.Operations)
            {
                DataContractSerializerOperationBehavior dcsob =
                    operationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();
                if (dcsob != null)
                {
                    dcsob.MaxItemsInObjectGraph = 2147483646;
                }
            }

            return Client;
        }


        //Chức năng
        public bool GetMenu(DatabaseConstant.Action action, ref HT_MENU objMenu, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            SupportServiceClient client = null;
            SupportServiceRequest request = null;
            SupportServiceResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhaiThacDuLieuService.layGiaTri());

                client = SupportServiceClient(ApplicationConstant.SystemService.KhaiThacDuLieuService);
                request = Common.Utilities.PrepareRequest(new SupportServiceRef.SupportServiceRequest());
                response = new SupportServiceRef.SupportServiceResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.DEFAULT;
                request.Action = action;
                request.objMenu = objMenu;

                // make a call to service client here
                response = client.Support(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objMenu = response.objMenu;
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }
    }
}
