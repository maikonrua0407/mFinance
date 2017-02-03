using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Utilities.Common;
using Presentation.Process.BaoHiemServiceRef;

namespace Presentation.Process
{
    public class BaoHiemProcess
    {
        /// <summary>
        /// Biến cục bộ chứa dịch vụ tiện ích của server
        /// </summary>
        private static BaoHiemServiceClient Client { get; set; }

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        static BaoHiemProcess()
        {
            //Client = new BaoHiemServiceClient();
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.BaoHiemService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.BaoHiemService.layGiaTri());
            Client = new BaoHiemServiceClient(basicHttpBinding, endpointAddress);
        }
    }
}
