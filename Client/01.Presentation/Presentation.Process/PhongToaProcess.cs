using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Utilities.Common;
using Presentation.Process.PhongToaServiceRef;

namespace Presentation.Process
{
    public class PhongToaProcess
    {
        /// <summary>
        /// Biến cục bộ chứa dịch vụ tiện ích của server
        /// </summary>
        private static PhongToaServiceClient Client { get; set; }

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        static PhongToaProcess()
        {
            //Client = new PhongToaServiceClient();
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.PhongToaService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.PhongToaService.layGiaTri());
            Client = new PhongToaServiceClient(basicHttpBinding, endpointAddress);
        }
    }
}
