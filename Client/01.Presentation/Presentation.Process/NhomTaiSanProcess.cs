using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Presentation.Process.TruyVanServiceRef;
using System.ServiceModel;
using Utilities.Common;
using System.ServiceModel.Description;
using System.Data;
using Presentation.Process.TaiSanServiceRef;

namespace Presentation.Process
{
    public class NhomTaiSanProcess
    {
        /// <summary>
        /// Biến cục bộ chứa dịch vụ tiện ích của server
        /// </summary>
        private static TruyVanServiceClient ClientTruyVan { get; set; }
        private static TaiSanServiceClient Client { get; set; }

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        static NhomTaiSanProcess()
        {
            EndpointAddress endpointAddressTruyVan = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.TruyVanService.layGiaTri());
            BasicHttpBinding basicHttpBindingTruyVan = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.TruyVanService.layGiaTri());
            ClientTruyVan = new TruyVanServiceClient(basicHttpBindingTruyVan, endpointAddressTruyVan);

            foreach (var operationDescription in ClientTruyVan.Endpoint.Contract.Operations)
            {
                DataContractSerializerOperationBehavior dcsob =
                    operationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();
                if (dcsob != null)
                {
                    dcsob.MaxItemsInObjectGraph = 2147483646;
                }
            }

            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.TaiSanService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.TaiSanService.layGiaTri());
            Client = new TaiSanServiceClient(basicHttpBinding, endpointAddress);

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

        /// <summary>
        /// Lay danh sach tai khoan hach toan
        /// </summary>
        /// <param name="MaNhomTS"></param>
        /// <returns></returns>

        public DataSet GetDSTaiKhoanHachToan(string MaNhomTS)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            if (MaNhomTS.Equals(""))
            {
                LDatatable.AddParameter(ref dt, "", "", "");
                request.dtThamSo = dt;
                request.objectName = "INQ.TS.TAI_KHOAN";
                request.inquiryName = "TAI_KHOAN";
            }
            else
            {
                LDatatable.AddParameter(ref dt, "@MA_NHOM_TS", "String", MaNhomTS);
                request.dtThamSo = dt;
                request.objectName = "INQ.TS.TAI_KHOAN";
                request.inquiryName = "MA_NHOM_TS";
            }

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
    }
}
