using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data;
using Presentation.Process.TruyVanServiceRef;
using Utilities.Common;
using Presentation.Process.LaiSuatServiceRef;
using System.Drawing;
using System.IO;
using Presentation.Process.Common;

namespace Presentation.Process
{
    public class LaiSuatProcess
    {
        /// <summary>
        /// Biến cục bộ chứa dịch vụ tiện ích của server
        /// </summary>
        private static LaiSuatServiceClient Client { get; set; }
        private static TruyVanServiceClient ClientTruyVan { get; set; }

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        static LaiSuatProcess()
        {
            EndpointAddress endpointAddressTruyVan = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.TruyVanService.layGiaTri());
            BasicHttpBinding basicHttpBindingTruyVan = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.TruyVanService.layGiaTri());
            ClientTruyVan = new TruyVanServiceClient(basicHttpBindingTruyVan, endpointAddressTruyVan);

            //Client = new LaiSuatServiceClient();
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.LaiSuatService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.LaiSuatService.layGiaTri());
            Client = new LaiSuatServiceClient(basicHttpBinding, endpointAddress);
        }

        public DataSet GetDSLaiSuat(string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "STRING", maDonVi);      
            request.dtThamSo = dt;
            request.objectName = "INQ.DC_LSUAT";
            request.inquiryName = "%";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetLaiSuatByID(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID", "STRING", id.ToString());
            //LDatatable.AddParameter(ref dt, "", "", "");      
            request.dtThamSo = dt;
            request.objectName = "INQ.DC_LSUAT_CT";
            request.inquiryName = "%";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);


            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DC_LSUAT LayThongTinLaiSuat(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            LaiSuatServiceRef.LaiSuatRequest request = Common.Utilities.PrepareRequest(new LaiSuatServiceRef.LaiSuatRequest());
            LaiSuatServiceRef.LaiSuatResponse response = new LaiSuatServiceRef.LaiSuatResponse();
            DC_LSUAT obj = new DC_LSUAT();
            obj.ID = id;
            request.objLsuat = obj;

            // Lấy kết quả trả về
            response = Client.GetThongTinLaiSuat(request);
            

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.objLsuat;
        }

        public DataSet GetLaiSuatCTietByID(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_ID_LS", "STRING", id.ToString());
            //LDatatable.AddParameter(ref dt, "", "", "");      
            request.dtThamSo = dt;
            request.objectName = "INQ.CT.LAI_SUAT_CTIET";
            request.inquiryName = "GET_BY_ID_LS";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public bool LaiSuat(DatabaseConstant.Action Action, ref DC_LSUAT objLaiSuat, ref List<DC_LSUAT_CTIET> lstLsuatCtiet, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.LaiSuatService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            LaiSuatRequest request = Common.Utilities.PrepareRequest(new LaiSuatRequest());
            request.Function = DatabaseConstant.Function.DC_LAI_SUAT_CT;
            request.Action = Action;
            request.objLsuat = objLaiSuat;
            request.lstLsuatCtiet = lstLsuatCtiet.ToArray();

            LaiSuatResponse response = Client.LaiSuat(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                objLaiSuat = response.objLsuat;
                lstLsuatCtiet = response.lstLsuatCtiet.ToList();
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        public bool DanhSachLaiSuat(DatabaseConstant.Action Action, ref List<DC_LSUAT> lstLSuat, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.LaiSuatService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            LaiSuatRequest request = Common.Utilities.PrepareRequest(new LaiSuatRequest());
            request.Function = DatabaseConstant.Function.DC_LAI_SUAT_DS;
            request.Action = Action;
            request.lstLsuat = lstLSuat.ToArray();

            LaiSuatResponse response = Client.LaiSuat(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                lstLSuat = response.lstLsuat.ToList();
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }
    }
}
