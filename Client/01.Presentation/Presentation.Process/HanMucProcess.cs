using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ServiceModel;
using Utilities.Common;
using Presentation.Process.HanMucServiceRef;
using Presentation.Process.TruyVanServiceRef;
using Presentation.Process.Common;
using System.ServiceModel.Description;

namespace Presentation.Process
{
    public class HanMucProcess
    {        
        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        static HanMucProcess()
        {            
        }

        /// <summary>
        /// Khởi tạo service HanMucService
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        private HanMucServiceClient HanMucServiceClient(ApplicationConstant.SystemService service)
        {
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(service.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(service.layGiaTri());
            HanMucServiceClient Client = new HanMucServiceClient(basicHttpBinding, endpointAddress);

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

        #region Hạn mức chi tiết
        public bool HanMucChiTiet(DatabaseConstant.Action action, ref List<DC_HAN_MUC> lstHanMuc, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            HanMucServiceClient client = null;
            HanMucRequest request = null;
            HanMucResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HanMucService.layGiaTri());

                client = HanMucServiceClient(ApplicationConstant.SystemService.HanMucService);
                request = Common.Utilities.PrepareRequest(new HanMucServiceRef.HanMucRequest());
                response = new HanMucServiceRef.HanMucResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.DC_HAN_MUC_CTIET;
                request.Action = action;
                request.lstHanMuc = lstHanMuc.ToArray();

                // make a call to service client here
                response = client.HanMuc(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }

        public DataSet GetHanMucChiTiet(string maLoaiDTuong, string maDTuong, string module, string tinhNang)
        {
            TruyVanServiceClient client = null;
            TruyVanRequest request = null;
            TruyVanResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

                client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
                request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
                response = new TruyVanServiceRef.TruyVanResponse();

                //Khởi tạo request
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@MA_DTUONG_LOAI", "STRING", maLoaiDTuong);
                LDatatable.AddParameter(ref dt, "@MA_DTUONG", "STRING", maDTuong);
                LDatatable.AddParameter(ref dt, "@MODULE", "STRING", module);
                LDatatable.AddParameter(ref dt, "@TINH_NANG", "STRING", tinhNang);      

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.HAN_MUC";
                request.inquiryName = "CHI_TIET";

                // make a call to service client here
                response = client.TruyVanMessage(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                DataSet ds = response.dsResult;
                return ds;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }
        #endregion

        #region Hạn mức Chung
        public bool HanMucChung(DatabaseConstant.Action action, ref List<DC_HAN_MUC> lstHanMuc, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            HanMucServiceClient client = null;
            HanMucRequest request = null;
            HanMucResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HanMucService.layGiaTri());

                client = HanMucServiceClient(ApplicationConstant.SystemService.HanMucService);
                request = Common.Utilities.PrepareRequest(new HanMucServiceRef.HanMucRequest());
                response = new HanMucServiceRef.HanMucResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.DC_HAN_MUC;
                request.Action = action;
                request.lstHanMuc = lstHanMuc.ToArray();

                // make a call to service client here
                response = client.HanMuc(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstHanMuc = response.lstHanMuc.ToList();
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }

        public DataSet GetHanMucChung(string maLoaiDTuong, string maDonVi)
        {
            TruyVanServiceClient client = null;
            TruyVanRequest request = null;
            TruyVanResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

                client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
                request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
                response = new TruyVanServiceRef.TruyVanResponse();

                //Khởi tạo request
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@MA_DTUONG_LOAI", "STRING", maLoaiDTuong);
                LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", maDonVi);                

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.HAN_MUC";
                request.inquiryName = "CHUNG";

                // make a call to service client here
                response = client.TruyVanMessage(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                DataSet ds = response.dsResult;
                return ds;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }

        public DataSet GetDanhSachHanMucChung(string maDonVi)
        {
            TruyVanServiceClient client = null;
            TruyVanRequest request = null;
            TruyVanResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

                client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
                request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
                response = new TruyVanServiceRef.TruyVanResponse();

                //Khởi tạo request
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", maDonVi);

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.HAN_MUC_CHUNG";
                request.inquiryName = "DANH_SACH";

                // make a call to service client here
                response = client.TruyVanMessage(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                DataSet ds = response.dsResult;
                return ds;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }
        #endregion

        #region Hạn mức khách hàng tổng
        public ApplicationConstant.ResponseStatus HanMucTong(DatabaseConstant.Function function, DatabaseConstant.Action action, ref HM_HMUC_TONG obj, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            HanMucServiceClient client = null;
            HanMucRequest request = null;
            HanMucResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HanMucService.layGiaTri());

                client = HanMucServiceClient(ApplicationConstant.SystemService.HanMucService);
                request = Common.Utilities.PrepareRequest(new HanMucServiceRef.HanMucRequest());
                response = new HanMucServiceRef.HanMucResponse();

                //Khởi tạo request
                request.Function = function;
                request.Action = action;
                request.objHanMucTong = obj;

                // make a call to service client here
                response = client.HanMuc(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.objHanMucTong;
                    return ApplicationConstant.ResponseStatus.THANH_CONG;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }

        public DataSet GetDanhSachHanMuc(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            request.dtThamSo = dt;
            request.objectName = "INQ.DS.HM_HAN_MUC_TONG";
            request.inquiryName = "INQ.DS.HM_HAN_MUC_TONG";

            // Lấy kết quả trả về
            TruyVanServiceClient ClientTruyVan = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy tree view hạn mức khách hàng
        /// Cấu trúc: Chi nhánh -> Khu vực
        /// </summary>
        /// <param name="maDonVi"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DataSet getTreeViewHanMucTong(string maDonVi, string userName)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            //LDatatable.AddParameter(ref dt, "@TrangThaiNVU", "STRING", BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri());
            //LDatatable.AddParameter(ref dt, "@TrangThaiSDU", "STRING", BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri());
            LDatatable.AddParameter(ref dt, "@INP_MA_DVI", "string", maDonVi);
            LDatatable.AddParameter(ref dt, "@INP_USER_NAME", "string", userName);

            request.dtThamSo = dt;
            request.objectName = "INQ.TREE.KHU_VUC_TDVM";
            request.inquiryName = "TREE_VIEW";

            // Lấy kết quả trả về
            TruyVanServiceClient ClientTruyVan = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Hạn mức khách hàng chi tiết
        public bool HanMucKhachHangChiTiet(DatabaseConstant.Action action, ref HM_HAN_MUC_KHACH_HANG_CTIET objHanMucKhachHangChiTiet, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            HanMucServiceClient client = null;
            HanMucRequest request = null;
            HanMucResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HanMucService.layGiaTri());

                client = HanMucServiceClient(ApplicationConstant.SystemService.HanMucService);
                request = Common.Utilities.PrepareRequest(new HanMucServiceRef.HanMucRequest());
                response = new HanMucServiceRef.HanMucResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.HM_CTIET_CT;
                request.Action = action;
                request.objHanMucKhachHangChiTiet = objHanMucKhachHangChiTiet;

                // make a call to service client here
                response = client.HanMuc(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objHanMucKhachHangChiTiet = response.objHanMucKhachHangChiTiet;
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }

        public bool DanhSachHanMucKhachHangChiTiet(DatabaseConstant.Action action, ref List<HM_HAN_MUC_KHACH_HANG_CTIET> lstHanMucKhachHangChiTiet, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            HanMucServiceClient client = null;
            HanMucRequest request = null;
            HanMucResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.HanMucService.layGiaTri());

                client = HanMucServiceClient(ApplicationConstant.SystemService.HanMucService);
                request = Common.Utilities.PrepareRequest(new HanMucServiceRef.HanMucRequest());
                response = new HanMucServiceRef.HanMucResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.HM_CTIET_DS;
                request.Action = action;
                request.lstHanMucKhachHangChiTiet = lstHanMucKhachHangChiTiet.ToArray();

                // make a call to service client here
                response = client.HanMuc(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }

        public DataSet GetDanhSachHanMucKhachHangChiTiet(DataTable dt)
        {
            TruyVanServiceClient client = null;
            TruyVanRequest request = null;
            TruyVanResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

                client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
                request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
                response = new TruyVanServiceRef.TruyVanResponse();

                //Khởi tạo request
                //DataTable dt = null;
                //LDatatable.MakeParameterTable(ref dt);
                //LDatatable.AddParameter(ref dt, "@TRANG_THAI_NVU", "STRING", trangThaiNVu);
                //LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", maDonVi); 

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.HMUC_KHACH_HANG_CTIET";
                request.inquiryName = "DANH_SACH";

                // make a call to service client here
                response = client.TruyVanMessage(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                DataSet ds = response.dsResult;
                return ds;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }
        #endregion
    }
}
