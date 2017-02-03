using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Utilities.Common;
using Presentation.Process.KhaiThacDuLieuServiceRef;
using System.Data;
using Presentation.Process.TruyVanServiceRef;
using System.ServiceModel.Description;
using Presentation.Process.Common;

namespace Presentation.Process
{
    public class KhaiThacDuLieuProcess
    {        

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        public KhaiThacDuLieuProcess()
        {
            
        }

        private KhaiThacDuLieuServiceClient KhaiThacDuLieuServiceClient(ApplicationConstant.SystemService service)
        {
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.KhaiThacDuLieuService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.KhaiThacDuLieuService.layGiaTri());
            KhaiThacDuLieuServiceClient Client = new KhaiThacDuLieuServiceClient(basicHttpBinding, endpointAddress);

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

        public ApplicationConstant.ResponseStatus DanhSachChiTieu(DatabaseConstant.Action action, string maPhanHe, ref List<HT_BAOCAO> lstBaoCao, ref string responseMessage)
        {
            KhaiThacDuLieuServiceClient client = null;
            KhaiThacDuLieuRequest request = null;
            KhaiThacDuLieuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhaiThacDuLieuService.layGiaTri());

                client = KhaiThacDuLieuServiceClient(ApplicationConstant.SystemService.KhaiThacDuLieuService);
                request = Common.Utilities.PrepareRequest(new KhaiThacDuLieuServiceRef.KhaiThacDuLieuRequest());
                response = new KhaiThacDuLieuServiceRef.KhaiThacDuLieuResponse();

                // Khởi tạo request
                request.Function = DatabaseConstant.Function.KTDL_DM_DS;
                request.Action = action;
                request.maPhanHeBaoCao = maPhanHe;

                // Gửi yêu cầu tới Server
                response = client.KhaiThacDuLieu(request);

                // Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                lstBaoCao = response.lstBaoCao.ToList();
                responseMessage = response.ResponseMessage;
                return response.ResponseStatus;
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

        public ApplicationConstant.ResponseStatus DuLieuChiTieu(DatabaseConstant.Action action, int idBaoCao, ref HT_BAOCAO htBaoCao, ref List<HT_BAOCAO_TSO> lstBaoCaoTso, ref string responseMessage)
        {
            KhaiThacDuLieuServiceClient client = null;
            KhaiThacDuLieuRequest request = null;
            KhaiThacDuLieuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhaiThacDuLieuService.layGiaTri());

                client = KhaiThacDuLieuServiceClient(ApplicationConstant.SystemService.KhaiThacDuLieuService);
                request = Common.Utilities.PrepareRequest(new KhaiThacDuLieuServiceRef.KhaiThacDuLieuRequest());
                response = new KhaiThacDuLieuServiceRef.KhaiThacDuLieuResponse();

                // Khởi tạo request
                request.Function = DatabaseConstant.Function.KTDL_DM_CT;
                request.Action = action;
                request.idBaoCao = idBaoCao;

                // Gửi yêu cầu tới Server
                response = client.KhaiThacDuLieu(request);

                // Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                htBaoCao = response.htBaoCao;
                lstBaoCaoTso = response.lstHtBaoCaoTso != null ? response.lstHtBaoCaoTso.ToList() : null;
                responseMessage = response.ResponseMessage;
                return response.ResponseStatus;
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

        public ApplicationConstant.ResponseStatus DuLieuChiTieu(DatabaseConstant.Action action, int idBaoCao, ref HT_BAOCAO htBaoCao, ref List<HT_BAOCAO_TSO> lstBaoCaoTso,ref DataSet ds, ref string responseMessage)
        {
            KhaiThacDuLieuServiceClient client = null;
            KhaiThacDuLieuRequest request = null;
            KhaiThacDuLieuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhaiThacDuLieuService.layGiaTri());

                client = KhaiThacDuLieuServiceClient(ApplicationConstant.SystemService.KhaiThacDuLieuService);
                request = Common.Utilities.PrepareRequest(new KhaiThacDuLieuServiceRef.KhaiThacDuLieuRequest());
                response = new KhaiThacDuLieuServiceRef.KhaiThacDuLieuResponse();

                // Khởi tạo request
                request.Function = DatabaseConstant.Function.KTDL_DM_DL_CT;
                request.Action = action;
                request.idBaoCao = idBaoCao;
                request.htBaoCao = htBaoCao;
                request.lstHtBaoCaoTso = lstBaoCaoTso.ToArray();
                // Gửi yêu cầu tới Server
                response = client.KhaiThacDuLieu(request);

                // Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                htBaoCao = response.htBaoCao;
                lstBaoCaoTso = response.lstHtBaoCaoTso != null ? response.lstHtBaoCaoTso.ToList() : null;
                ds = response.dsDuLieuBaoCao;
                responseMessage = response.ResponseMessage;
                return response.ResponseStatus;
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

        /// <summary>
        /// Lấy thông tin giao dịch khách hàng, hợp đồng tín dụng, tiền gửi tiết kiệm
        /// </summary>
        /// <param name="id">ID khách hàng</param>
        /// <returns></returns>
        public DataSet GetThongTinCoBanKhachHang(int id, string maKHang)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_KHANG", "INT", id.ToString());
            LDatatable.AddParameter(ref dt, "@MA_KHANG", "INT", maKHang);

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.KTDL.KHTV_THONG_TIN";

            // Lấy kết quả trả về
            TruyVanServiceClient client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin giao dịch khách hàng, hợp đồng tín dụng, tiền gửi tiết kiệm
        /// </summary>
        /// <param name="id">ID khách hàng</param>
        /// <returns></returns>
        public DataSet GetThongTinDanhSachKUOC(int id, string maHDTD)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_HDTDVM", "INT", id.ToString());
            LDatatable.AddParameter(ref dt, "@MA_HDTDVM", "INT", maHDTD);

            request.dtThamSo = dt;
            request.inquiryName = "DANHSACH.KUOCVM";
            request.objectName = "INQ.KTDL.KHTV_GDICH_TDUNG";

            // Lấy kết quả trả về
            TruyVanServiceClient client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin giao dịch khách hàng, hợp đồng tín dụng, tiền gửi tiết kiệm
        /// </summary>
        /// <param name="id">ID khách hàng</param>
        /// <returns></returns>
        public DataSet GetThongTinGiaoDichKUOC(int id, string maKUOVM)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_KUOCVM", "INT", id.ToString());
            LDatatable.AddParameter(ref dt, "@MA_KUOCVM", "INT", maKUOVM);

            request.dtThamSo = dt;
            request.inquiryName = "DANHSACH.GIAODICH";
            request.objectName = "INQ.KTDL.KHTV_GDICH_TDUNG";

            // Lấy kết quả trả về
            TruyVanServiceClient client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin giao dịch khách hàng, hợp đồng tín dụng, tiền gửi tiết kiệm
        /// </summary>
        /// <param name="id">ID khách hàng</param>
        /// <returns></returns>
        public DataSet GetThongTinGiaoDichTietKiem(int id, string maSoTG)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_TIEN_GUI", "INT", id.ToString());
            LDatatable.AddParameter(ref dt, "@MA_TIEN_GUI", "INT", maSoTG);

            request.dtThamSo = dt;
            request.inquiryName = "DANHSACH.GIAODICH";
            request.objectName = "INQ.KTDL.KHTV_GDICH_TIETKIEM";

            // Lấy kết quả trả về
            TruyVanServiceClient client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        #region Loại đối tượng
        public bool LoaiTK(DatabaseConstant.Action action, ref BC_LOAITK objLoaiTK, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            KhaiThacDuLieuServiceClient client = null;
            KhaiThacDuLieuRequest request = null;
            KhaiThacDuLieuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhaiThacDuLieuService.layGiaTri());

                client = KhaiThacDuLieuServiceClient(ApplicationConstant.SystemService.KhaiThacDuLieuService);
                request = Common.Utilities.PrepareRequest(new KhaiThacDuLieuServiceRef.KhaiThacDuLieuRequest());
                response = new KhaiThacDuLieuServiceRef.KhaiThacDuLieuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.KTDL_MAPPING_LOAITK_CT;
                request.Action = action;
                request.objLoaiTK = objLoaiTK;

                // make a call to service client here
                response = client.KhaiThacDuLieu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objLoaiTK = response.objLoaiTK;
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

        public bool DanhSachLoaiTK(DatabaseConstant.Action action, ref List<BC_LOAITK> lstLoaiTK, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            KhaiThacDuLieuServiceClient client = null;
            KhaiThacDuLieuRequest request = null;
            KhaiThacDuLieuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhaiThacDuLieuService.layGiaTri());

                client = KhaiThacDuLieuServiceClient(ApplicationConstant.SystemService.KhaiThacDuLieuService);
                request = Common.Utilities.PrepareRequest(new KhaiThacDuLieuServiceRef.KhaiThacDuLieuRequest());
                response = new KhaiThacDuLieuServiceRef.KhaiThacDuLieuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.KTDL_MAPPING_LOAITK_DS;
                request.Action = action;
                request.lstLoaiTK = lstLoaiTK.ToArray();

                // make a call to service client here
                response = client.KhaiThacDuLieu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstLoaiTK = response.lstLoaiTK.ToList();
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


        public DataSet GetDanhSachLoaiTK(DataTable dt)
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
                request.objectName = "INQ.DS.MAPPING_LOAITK";
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


        #region Thống kê chỉ tiêu
        public bool MaTK(DatabaseConstant.Action action, ref BC_MATK objMaTK, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            KhaiThacDuLieuServiceClient client = null;
            KhaiThacDuLieuRequest request = null;
            KhaiThacDuLieuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhaiThacDuLieuService.layGiaTri());

                client = KhaiThacDuLieuServiceClient(ApplicationConstant.SystemService.KhaiThacDuLieuService);
                request = Common.Utilities.PrepareRequest(new KhaiThacDuLieuServiceRef.KhaiThacDuLieuRequest());
                response = new KhaiThacDuLieuServiceRef.KhaiThacDuLieuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.KTDL_MAPPING_MATK_CT;
                request.Action = action;
                request.objMaTK = objMaTK;

                // make a call to service client here
                response = client.KhaiThacDuLieu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objMaTK = response.objMaTK;
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

        public bool DanhSachMaTK(DatabaseConstant.Action action, ref List<BC_MATK> lstMaTK, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            KhaiThacDuLieuServiceClient client = null;
            KhaiThacDuLieuRequest request = null;
            KhaiThacDuLieuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhaiThacDuLieuService.layGiaTri());

                client = KhaiThacDuLieuServiceClient(ApplicationConstant.SystemService.KhaiThacDuLieuService);
                request = Common.Utilities.PrepareRequest(new KhaiThacDuLieuServiceRef.KhaiThacDuLieuRequest());
                response = new KhaiThacDuLieuServiceRef.KhaiThacDuLieuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.KTDL_MAPPING_MATK_DS;
                request.Action = action;
                request.lstMaTK = lstMaTK.ToArray();

                // make a call to service client here
                response = client.KhaiThacDuLieu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstMaTK = response.lstMaTK.ToList();
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


        public DataSet GetDanhSachMaTK(DataTable dt)
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
                request.objectName = "INQ.DS.MAPPING_MATK";
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


        #region Mapping

        public DataSet GetDanhSachDKien(string dkien)
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

                request.dtThamSo = null;
                request.objectName = dkien.Trim();
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


        public DataSet GetDanhSachMapping(DataTable dt)
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
                request.objectName = "INQ.DS.MAPPING_DS_MAPPING";
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

        public bool Mapping(DatabaseConstant.Action action, ref List<BC_MATK_MAPPING> lstMapping, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            KhaiThacDuLieuServiceClient client = null;
            KhaiThacDuLieuRequest request = null;
            KhaiThacDuLieuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhaiThacDuLieuService.layGiaTri());

                client = KhaiThacDuLieuServiceClient(ApplicationConstant.SystemService.KhaiThacDuLieuService);
                request = Common.Utilities.PrepareRequest(new KhaiThacDuLieuServiceRef.KhaiThacDuLieuRequest());
                response = new KhaiThacDuLieuServiceRef.KhaiThacDuLieuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.KTDL_MAPPING_MAPPING;
                request.Action = action;
                request.lstMaTKMapping = lstMapping.ToArray();

                // make a call to service client here
                response = client.KhaiThacDuLieu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstMapping = response.lstMaTKMapping.ToList();
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
        #endregion


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
    }
}
