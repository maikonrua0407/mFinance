using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data;
using Utilities.Common;
using Presentation.Process.TruyVanServiceRef;
using Presentation.Process.Common;
using System.ServiceModel.Description;

namespace Presentation.Process
{
    public class TruyVanProcess
    {
        /// <summary>
        /// Biến cục bộ chứa dịch vụ tiện ích của server
        /// </summary>
        private static TruyVanServiceClient Client { get; set; }

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        static TruyVanProcess()
        {
            //Client = new TruyVanServiceClient();
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.TruyVanService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.TruyVanService.layGiaTri());
            Client = new TruyVanServiceClient(basicHttpBinding, endpointAddress);

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
        /// Lấy dữ liệu theo mã truy vấn và không có tham số
        /// </summary>
        /// <param name="maTruyVan"></param>
        /// <returns></returns>
        public DataSet TruyVan(string maTruyVan)
        {
            DataTable dt = null;
            DataSet ds = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "", "", "");

            ds = Client.TruyVan(maTruyVan, dt);

            return ds;
        }

        public DataSet TruyVan(string maTruyVan, DataTable dt)
        {
            DataSet ds = null;

            ds = Client.TruyVan(maTruyVan, dt);

            return ds;
        }

        public DataSet TruyVanUDTT(string maTruyVan, DataTable dt)
        {
            DataSet ds = null;

            ds = Client.TruyVanUDTT(maTruyVan, dt);

            return ds;
        }

        public DanhSachResponse getDanhSachInformation(string maTruyVan, List<string> lstDieuKien = null)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DanhSachRequest request = Common.Utilities.PrepareRequest(new DanhSachRequest());
            request.MaTruyVan = maTruyVan;
            if(lstDieuKien!=null)
            request.ListParamValue = lstDieuKien.ToArray<string>();            

            // Lấy kết quả trả về
            DanhSachResponse response = Client.getDanhSachInformation(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            return response;
        }

        public DataSet getDanhSachTheoListCSO(List<CSO_TSO> lstCSO)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DanhSachRequest request = Common.Utilities.PrepareRequest(new DanhSachRequest());            
            if (lstCSO != null)
                request.lstCSO = lstCSO.ToArray();

            // Lấy kết quả trả về
            DanhSachResponse response = Client.getDanhSachTheoListCSO(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);
            return response.DataSetSource;
        }

        public DataSet getTreeView(DataTable dt, string inqName)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = inqName;
            request.objectName = inqName;

            // Lấy kết quả trả về
            response = Client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
    }
}
