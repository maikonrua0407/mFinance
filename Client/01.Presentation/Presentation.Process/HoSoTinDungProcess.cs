using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Utilities.Common;
using Presentation.Process.HoSoTinDungServiceRef;

namespace Presentation.Process
{
    public class HoSoTinDungProcess
    {
        /// <summary>
        /// Biến cục bộ chứa dịch vụ tiện ích của server
        /// </summary>
        private static HoSoTinDungServiceClient Client { get; set; }

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        static HoSoTinDungProcess()
        {
            //Client = new HoSoTinDungServiceClient();
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.HoSoTinDungService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.HoSoTinDungService.layGiaTri());
            Client = new HoSoTinDungServiceClient(basicHttpBinding, endpointAddress);
        }
    }
}
