using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Utilities.Common;
using Presentation.Process.NganQuyServiceRef;

namespace Presentation.Process
{
    public class NganQuyProcess
    {
        /// <summary>
        /// Biến cục bộ chứa dịch vụ tiện ích của server
        /// </summary>
        private static NganQuyServiceClient Client { get; set; }

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        static NganQuyProcess()
        {
            //Client = new NganQuyServiceClient();
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.NganQuyService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.NganQuyService.layGiaTri());
            Client = new NganQuyServiceClient(basicHttpBinding, endpointAddress);
        }
    }
}
