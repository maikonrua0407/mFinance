using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ServiceModel;
using Utilities.Common;
using Presentation.Process.TinDungTTServiceRef;
using Presentation.Process.TruyVanServiceRef;
using Presentation.Process.Common;
using System.ServiceModel.Description;

namespace Presentation.Process
{
    public class TinDungTTProcess
    {
        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        static TinDungTTProcess()
        {            
        }
       
        /// <summary>
        /// Khởi tạo service TinDungTThuongService
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        private TinDungTTServiceClient TinDungTTServiceClient(ApplicationConstant.SystemService service)
        {
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(service.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(service.layGiaTri());
            TinDungTTServiceClient Client = new TinDungTTServiceClient(basicHttpBinding, endpointAddress);

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

        #region Dùng chung
        public DataSet getTreeView(string maDonVi, string userName)
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

        #region San pham
        public bool SanPham(DatabaseConstant.Action action, ref TDTT_SAN_PHAM objSanPham, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            TinDungTTServiceClient client = null;
            TinDungTTRequest request = null;
            TinDungTTResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungTTService.layGiaTri());

                client = TinDungTTServiceClient(ApplicationConstant.SystemService.TinDungTTService);
                request = Common.Utilities.PrepareRequest(new TinDungTTServiceRef.TinDungTTRequest());
                response = new TinDungTTServiceRef.TinDungTTResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TD_SAN_PHAMTT;
                request.Action = action;
                request.objSanPham = objSanPham;

                // make a call to service client here
                response = client.TinDungTT(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objSanPham = response.objSanPham;
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

        public bool DanhSachSanPham(DatabaseConstant.Action action, ref List<TDTT_SAN_PHAM> lstSanPham, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            TinDungTTServiceClient client = null;
            TinDungTTRequest request = null;
            TinDungTTResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungTTService.layGiaTri());

                client = TinDungTTServiceClient(ApplicationConstant.SystemService.TinDungTTService);
                request = Common.Utilities.PrepareRequest(new TinDungTTServiceRef.TinDungTTRequest());
                response = new TinDungTTServiceRef.TinDungTTResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TD_SAN_PHAMTT_DS;
                request.Action = action;
                request.lstSanPham = lstSanPham.ToArray();

                // make a call to service client here
                response = client.TinDungTT(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstSanPham = response.lstSanPham.ToList();
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

        public DataSet GetDanhSachSanPham(DataTable dt)
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

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.SAN_PHAMTT";
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

        public DataSet GetTaiKhoanHachToan(string maDoiTuong, string maDonVi)
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

                DataTable dt = null;

                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@MA_PHAN_HE", "STRING", DatabaseConstant.Module.TDTT.getValue());
                LDatatable.AddParameter(ref dt, "@MA_DTUONG", "STRING", maDoiTuong);
                LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", maDonVi);

                request.dtThamSo = dt;
                request.objectName = "INQ.CT.TD_SAN_PHAM";
                request.inquiryName = "TAI_KHOAN_HACH_TOAN";


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

        #region Hợp đồng tín dụng
        public bool HopDongTinDung(DatabaseConstant.Action action, ref TDTT_HOP_DONG_TIN_DUNG objHopDongTinDung, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            TinDungTTServiceClient client = null;
            TinDungTTRequest request = null;
            TinDungTTResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungTTService.layGiaTri());

                client = TinDungTTServiceClient(ApplicationConstant.SystemService.TinDungTTService);
                request = Common.Utilities.PrepareRequest(new TinDungTTServiceRef.TinDungTTRequest());
                response = new TinDungTTServiceRef.TinDungTTResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TD_HDTD;
                request.Action = action;
                request.objHopDongTinDung = objHopDongTinDung;

                // make a call to service client here
                response = client.TinDungTT(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objHopDongTinDung = response.objHopDongTinDung;
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

        public bool DanhSachHopDongTinDung(DatabaseConstant.Action action, ref List<TDTT_HOP_DONG_TIN_DUNG> lstHopDongTinDung, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            TinDungTTServiceClient client = null;
            TinDungTTRequest request = null;
            TinDungTTResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungTTService.layGiaTri());

                client = TinDungTTServiceClient(ApplicationConstant.SystemService.TinDungTTService);
                request = Common.Utilities.PrepareRequest(new TinDungTTServiceRef.TinDungTTRequest());
                response = new TinDungTTServiceRef.TinDungTTResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TD_HDTD_DS;
                request.Action = action;
                request.lstHopDongTinDung = lstHopDongTinDung.ToArray();

                // make a call to service client here
                response = client.TinDungTT(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstHopDongTinDung = response.lstHopDongTinDung.ToList();
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

        public DataSet GetDanhSachHopDongTinDung(DataTable dt)
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

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.HOP_DONG_TIN_DUNG_TT";
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
