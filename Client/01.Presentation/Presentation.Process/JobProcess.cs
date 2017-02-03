using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Presentation.Process.JobServiceRef;
using System.ServiceModel;
using Utilities.Common;
using System.ServiceModel.Description;

namespace Presentation.Process
{
    public class JobProcess
    {
        private static JobServiceClient Client { get; set; }

        static JobProcess()
        {
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.JobService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.JobService.layGiaTri());
            Client = new JobServiceClient(basicHttpBinding, endpointAddress);

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

        public bool SysJobEmail(DatabaseConstant.Action Action, ref SYS_JOB_EMAIL objSysJobEmail, ref string sMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.JobService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            JobRequest request = Common.Utilities.PrepareRequest(new JobRequest());
            request.Function = DatabaseConstant.Function.SYS_JOB_EMAIL;
            request.Action = Action;
            request.objSysJobEmail = objSysJobEmail;
            
            JobResponse response = Client.Job(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objSysJobEmail = response.objSysJobEmail;
                return true;
            }
            else
            {
                sMessage = response.ResponseMessage;
                return false;
            }
        }

        public bool SysJobSubscribe(DatabaseConstant.Action Action, ref string maDoiTuong, ref SYS_JOB_SUBSCRIBE objSysJobSubscribe, ref List<SYS_JOB_SUBSCRIBE> lstSysJobSubscribe, ref string sMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.JobService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            JobRequest request = Common.Utilities.PrepareRequest(new JobRequest());
            request.Function = DatabaseConstant.Function.SYS_JOB_SUBSCRIBE;
            request.Action = Action;
            request.maDoiTuong = maDoiTuong;
            request.objSysJobSubscribe = objSysJobSubscribe;
            request.lstSysJobSubscribe = lstSysJobSubscribe.ToArray();

            JobResponse response = Client.Job(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objSysJobSubscribe = response.objSysJobSubscribe;
                if (lstSysJobSubscribe != null) lstSysJobSubscribe = response.lstSysJobSubscribe.ToList();
                return true;
            }
            else
            {
                sMessage = response.ResponseMessage;
                return false;
            }
        }

        public bool SysJobHis(DatabaseConstant.Action Action, ref string maDoiTuong, ref SYS_JOB_SUBSCRIBE objSysJobSubscribe, ref List<SYS_JOB_HIS> lstSysJobHis, ref string sMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.JobService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            JobRequest request = Common.Utilities.PrepareRequest(new JobRequest());
            request.Function = DatabaseConstant.Function.SYS_JOB_HIS;
            request.Action = Action;
            request.maDoiTuong = maDoiTuong;
            request.objSysJobSubscribe = objSysJobSubscribe;
            request.lstSysJobHis = lstSysJobHis.ToArray();

            JobResponse response = Client.Job(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objSysJobSubscribe = response.objSysJobSubscribe;
                if (lstSysJobHis != null) lstSysJobHis = response.lstSysJobHis.ToList();
                sMessage = response.ResponseMessage;
                return true;
            }
            else
            {
                sMessage = response.ResponseMessage;
                return false;
            }
        }
    }
}
