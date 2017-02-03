using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Utilities.Common;
using Presentation.Process.ThanhVienServiceRef;

namespace Presentation.Process
{
    public class ThanhVienProcess
    {
        /// <summary>
        /// Biến cục bộ chứa dịch vụ tiện ích của server
        /// </summary>
        private static ThanhVienServiceClient Client { get; set; }

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        static ThanhVienProcess()
        {
            //Client = new ThanhVienServiceClient();
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.ThanhVienService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.ThanhVienService.layGiaTri());
            Client = new ThanhVienServiceClient(basicHttpBinding, endpointAddress);
        }
    }
}
