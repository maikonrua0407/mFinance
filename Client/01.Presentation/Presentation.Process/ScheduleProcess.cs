using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ServiceModel;
using Utilities.Common;
using Presentation.Process.TruyVanServiceRef;
using Presentation.Process.Common;
using System.ServiceModel.Description;

namespace Presentation.Process
{
    public class ScheduleProcess
    {
        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        public ScheduleProcess()
        {
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


        public DataSet GetListCobInfomation(DataTable dt)
        {
            TruyVanServiceClient client = null;
            TruyVanRequest request = null;
            TruyVanResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

                client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
                request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
                response = new TruyVanServiceRef.TruyVanResponse();

                //Khởi tạo request
                
                request.dtThamSo = dt;
                request.objectName = "INQ.DS.COB";
                request.inquiryName = "RESULT";

                // make a call to service client here
                response = client.TruyVanMessage(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                DataSet ds = response.dsResult;
                return ds;
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

        public DataSet GetResultCT(DataTable dt)
        {
            TruyVanServiceClient client = null;
            TruyVanRequest request = null;
            TruyVanResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

                client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
                request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
                response = new TruyVanServiceRef.TruyVanResponse();

                //Khởi tạo request

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.COB";
                request.inquiryName = "RESULT_CT";

                // make a call to service client here
                response = client.TruyVanMessage(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                DataSet ds = response.dsResult;
                return ds;
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

        public DataSet GetResultMessage(DataTable dt)
        {
            TruyVanServiceClient client = null;
            TruyVanRequest request = null;
            TruyVanResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

                client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
                request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
                response = new TruyVanServiceRef.TruyVanResponse();

                //Khởi tạo request

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.COB";
                request.inquiryName = "RESULT_MESSAGE";

                // make a call to service client here
                response = client.TruyVanMessage(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                DataSet ds = response.dsResult;
                return ds;
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
