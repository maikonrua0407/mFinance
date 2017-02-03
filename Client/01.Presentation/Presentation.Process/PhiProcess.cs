using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Utilities.Common;
using Presentation.Process.PhiServiceRef;
using Presentation.Process.Common;
using Presentation.Process.TruyVanServiceRef;
using System.Data;
using System.ServiceModel.Description;

namespace Presentation.Process
{
    public class PhiProcess
    {
        /// <summary>
        /// Biến cục bộ chứa dịch vụ tiện ích của server
        /// </summary>
        private static PhiServiceClient Client { get; set; }
        private static TruyVanServiceClient ClientTruyVan { get; set; }
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

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        static PhiProcess()
        {
            EndpointAddress endpointAddressTruyVan = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.TruyVanService.layGiaTri());
            BasicHttpBinding basicHttpBindingTruyVan = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.TruyVanService.layGiaTri());
            ClientTruyVan = new TruyVanServiceClient(basicHttpBindingTruyVan, endpointAddressTruyVan);

            //Client = new PhiServiceClient();
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.PhiService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.PhiService.layGiaTri());
            Client = new PhiServiceClient(basicHttpBinding, endpointAddress);
        }

        public DataSet GetDSPhi()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "", "", "");      
            request.dtThamSo = dt;
            request.objectName = "INQ.DC_PHI";
            request.inquiryName = "%";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetPhiByID(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_PHI", "STRING", id.ToString());
            //LDatatable.AddParameter(ref dt, "", "", "");      
            request.dtThamSo = dt;
            request.objectName = "INQ.DC_PHI_CT";
            request.inquiryName = "%";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DC_BPHI LayThongTinPhi(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            PhiServiceRef.PhiRequest request = Common.Utilities.PrepareRequest(new PhiServiceRef.PhiRequest());
            PhiServiceRef.PhiResponse response = new PhiServiceRef.PhiResponse();
            DC_BPHI obj = new DC_BPHI();
            obj.ID = id;
            request.objPhi = obj;

            // Lấy kết quả trả về
            response = Client.GetThongTinPhi(request);
            

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.objPhi;
        }

        public DataSet GetPhiCTietByID(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_PHI", "STRING", id.ToString());
            //LDatatable.AddParameter(ref dt, "", "", "");      
            request.dtThamSo = dt;
            request.objectName = "INQ.DC_PHI_CT";
            request.inquiryName = "PHI_CTIET";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public bool ProcessPhi(DatabaseConstant.Function Function, DatabaseConstant.Action Action, ref List<DC_BPHI> lst, ref List<DC_BPHI_CTIET> lstCT,
                                        ref List<KT_BPHI_TKHOAN> lstPhiTK, ref List<DC_BPHI_GDICH> lstPhiGD, ref List<KT_PHAN_HE_PLOAI> lstPhanHePLoai ,ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.PhiService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            PhiRequest request = Common.Utilities.PrepareRequest(new PhiRequest());
            request.Function = Function;
            request.Action = Action;
            request.objPhi = lst.First();
            request.lstPhi = lst.ToArray();
            request.lstPhiCtiet = lstCT.ToArray();
            request.lstPhiTK = lstPhiTK.ToArray();
            request.lstPhiGD = lstPhiGD.ToArray();
            request.lstPhanHePLoai = lstPhanHePLoai.ToArray();

            PhiResponse response = Client.Phi(request);
            listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
            if (response.lstPhi != null && response.lstPhi.Count() > 0)
                lst = response.lstPhi.ToList();

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataSet GetTaiKhoanHachToan(string maDoiTuong, string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            TruyVanServiceClient client = null;
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);

            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_PHAN_HE", "STRING", DatabaseConstant.Module.DMDC.getValue());
            LDatatable.AddParameter(ref dt, "@MA_DTUONG", "STRING", maDoiTuong);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", maDonVi);

            request.dtThamSo = dt;
            request.objectName = "INQ.CT.BIEU_PHI";
            request.inquiryName = "TAI_KHOAN_HACH_TOAN";

            // Lấy kết quả trả về
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
    }
}
