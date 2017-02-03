using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Utilities.Common;
using Presentation.Process.ZATestAppServiceRef;

namespace Presentation.Process
{
    public class ZATestAppProcess
    {
        /// <summary>
        /// Biến cục bộ chứa dịch vụ tiện ích của server
        /// </summary>
        private static ZATestAppServiceClient Client { get; set; }

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        static ZATestAppProcess()
        {
            //Client = new ZATestAppServiceClient();
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.ZATestAppService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.ZATestAppService.layGiaTri());
            Client = new ZATestAppServiceClient(basicHttpBinding, endpointAddress);
        }

        public void processDataTransfer(byte[] byteArray1, byte[] byteArray2)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.ZATestAppService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DataTransferRequest request = Common.Utilities.PrepareRequest(new DataTransferRequest());
            request.logo1 = byteArray1;
            request.logo2 = byteArray2;

            // Ghi log
            LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "processDataTransfer at Client : ");

            // Lấy kết quả trả về
            DataTransferResponse response = Client.processDataTransfer(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
        }
    }
}
