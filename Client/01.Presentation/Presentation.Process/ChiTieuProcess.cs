using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Utilities.Common;
using Presentation.Process.ChiTieuServiceRef;

namespace Presentation.Process
{
    public class ChiTieuProcess
    {
        /// <summary>
        /// Biến cục bộ chứa dịch vụ tiện ích của server
        /// </summary>
        private static ChiTieuServiceClient Client { get; set; }

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        static ChiTieuProcess()
        {
            //Client = new ChiTieuServiceClient();
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.ChiTieuService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.ChiTieuService.layGiaTri());
            Client = new ChiTieuServiceClient(basicHttpBinding, endpointAddress);
        }
    }
}
