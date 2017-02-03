using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ServiceModel;
using Utilities.Common;
using Presentation.Process.TaiSanDamBaoServiceRef;
using Presentation.Process.TruyVanServiceRef;
using Presentation.Process.Common;
using System.ServiceModel.Description;

namespace Presentation.Process
{
    public class TaiSanDamBaoProcess
    {
        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        /// 
        public TaiSanDamBaoProcess()
        {
        }

        /// <summary>
        /// Khởi tạo service TaiSanDamBaoService
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        private TaiSanDamBaoServiceClient TaiSanDamBaoServiceClient(ApplicationConstant.SystemService service)
        {
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(service.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(service.layGiaTri());
            TaiSanDamBaoServiceClient Client = new TaiSanDamBaoServiceClient(basicHttpBinding, endpointAddress);

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

        public int functionName(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanDamBaoService.layGiaTri());

            TaiSanDamBaoServiceClient client = TaiSanDamBaoServiceClient(ApplicationConstant.SystemService.TaiSanDamBaoService);
            TaiSanDamBaoServiceRef.TaiSanDamBaoRequest request = Common.Utilities.PrepareRequest(new TaiSanDamBaoServiceRef.TaiSanDamBaoRequest());
            TaiSanDamBaoServiceRef.TaiSanDamBaoResponse response = new TaiSanDamBaoServiceRef.TaiSanDamBaoResponse();
            try
            {
                // make a call to service client here
                response = client.functionName(request);
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
            }
            return 0;
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

        public DataSet GetTreeDonVi(string maDangNhap, string maDonVi)
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
                LDatatable.AddParameter(ref dt, "@MA_DANG_NHAP", "STRING", maDangNhap);
                LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "STRING", maDonVi);

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.TREE_PVI";
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
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_PHAN_HE", "STRING", DatabaseConstant.Module.TSDB.getValue());
            LDatatable.AddParameter(ref dt, "@MA_DTUONG", "STRING", maDoiTuong);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", maDonVi);

            request.dtThamSo = dt;
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_CT_TD_SAN_PHAM.getValue();
            request.inquiryName = "TAI_KHOAN_HACH_TOAN";

            // Lấy kết quả trả về
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getThongTinKHLoaiTSTheoIDKhang(string maKH)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA", "INT", maKH.ToString());

            request.dtThamSo = dt;
            request.inquiryName = "INQ.CT.TSDB.KHANG.LOAITS";
            request.objectName = "INQ.CT.TSDB.KHANG.LOAITS";

            // Lấy kết quả trả về
            TruyVanServiceClient ClientTruyVan = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getThongTinLoaiTSDB(string maLoaiTSDB)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA", "INT", maLoaiTSDB.ToString());

            request.dtThamSo = dt;
            request.inquiryName = "INQ.CT.TSDB.LOAITS";
            request.objectName = "INQ.CT.TSDB.LOAITS";

            // Lấy kết quả trả về
            TruyVanServiceClient ClientTruyVan = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Loại tài sản
        public bool LoaiTS(DatabaseConstant.Action action, ref TD_TSAN_LOAI objLoaiTS, ref List<KT_PHAN_HE_PLOAI> lstPhanLoai, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            TaiSanDamBaoServiceClient client = null;
            TaiSanDamBaoRequest request = null;
            TaiSanDamBaoResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanDamBaoService.layGiaTri());

                client = TaiSanDamBaoServiceClient(ApplicationConstant.SystemService.TaiSanDamBaoService);
                request = Common.Utilities.PrepareRequest(new TaiSanDamBaoServiceRef.TaiSanDamBaoRequest());
                response = new TaiSanDamBaoServiceRef.TaiSanDamBaoResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TD_TSDB_LOAI_CT;
                request.Action = action;
                request.objLoaiTS = objLoaiTS;
                request.lstPhanHePLoai = lstPhanLoai.ToArray();

                // make a call to service client here
                response = client.TaiSanDamBao(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objLoaiTS = response.objLoaiTS;
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

        public bool DanhSachLoaiTS(DatabaseConstant.Action action, ref List<TD_TSAN_LOAI> lstLoaiTS, ref List<KT_PHAN_HE_PLOAI> lstPhanLoai, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            TaiSanDamBaoServiceClient client = null;
            TaiSanDamBaoRequest request = null;
            TaiSanDamBaoResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanDamBaoService.layGiaTri());

                client = TaiSanDamBaoServiceClient(ApplicationConstant.SystemService.TaiSanDamBaoService);
                request = Common.Utilities.PrepareRequest(new TaiSanDamBaoServiceRef.TaiSanDamBaoRequest());
                response = new TaiSanDamBaoServiceRef.TaiSanDamBaoResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TD_TSDB_LOAI_DS;
                request.Action = action;
                request.lstLoaiTS = lstLoaiTS.ToArray();
                request.lstPhanHePLoai = lstPhanLoai.ToArray();

                // make a call to service client here
                response = client.TaiSanDamBao(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstLoaiTS = response.lstLoaiTS.ToList();
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

        public DataSet GetDanhSachLoaiTS(DataTable dt)
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

                request.dtThamSo = dt;
                request.objectName = "INQ.TSDB.LOAITS";
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

        #region Tài sản đảm bảo
        public bool TaiSanDamBao(DatabaseConstant.Action action, ref TD_TAI_SAN_DAM_BAO objTSDB, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            TaiSanDamBaoServiceClient client = null;
            TaiSanDamBaoRequest request = null;
            TaiSanDamBaoResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanDamBaoService.layGiaTri());

                client = TaiSanDamBaoServiceClient(ApplicationConstant.SystemService.TaiSanDamBaoService);
                request = Common.Utilities.PrepareRequest(new TaiSanDamBaoServiceRef.TaiSanDamBaoRequest());
                response = new TaiSanDamBaoServiceRef.TaiSanDamBaoResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TD_TSDB_CT;
                request.Action = action;
                request.objTSDB = objTSDB;

                // make a call to service client here
                response = client.TaiSanDamBao(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objTSDB = response.objTSDB;
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

        public bool DanhSachTaiSanDamBao(DatabaseConstant.Action action, ref List<TD_TAI_SAN_DAM_BAO> lstTSDB, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            TaiSanDamBaoServiceClient client = null;
            TaiSanDamBaoRequest request = null;
            TaiSanDamBaoResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanDamBaoService.layGiaTri());

                client = TaiSanDamBaoServiceClient(ApplicationConstant.SystemService.TaiSanDamBaoService);
                request = Common.Utilities.PrepareRequest(new TaiSanDamBaoServiceRef.TaiSanDamBaoRequest());
                response = new TaiSanDamBaoServiceRef.TaiSanDamBaoResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TD_TSDB_DS;
                request.Action = action;
                request.lstTSDB = lstTSDB.ToArray();

                // make a call to service client here
                response = client.TaiSanDamBao(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstTSDB = response.lstTSDB.ToList();
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

        public DataSet GetDanhSachTaiSanDamBao(DataTable dt)
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

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.TSDB.TSDB";
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

        #region Hợp đồng thế chấp
        public bool HopDongTheChap(DatabaseConstant.Action action, ref TDVM_HOP_DONG_TCHAP objHDTC, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            TaiSanDamBaoServiceClient client = null;
            TaiSanDamBaoRequest request = null;
            TaiSanDamBaoResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanDamBaoService.layGiaTri());

                client = TaiSanDamBaoServiceClient(ApplicationConstant.SystemService.TaiSanDamBaoService);
                request = Common.Utilities.PrepareRequest(new TaiSanDamBaoServiceRef.TaiSanDamBaoRequest());
                response = new TaiSanDamBaoServiceRef.TaiSanDamBaoResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP;
                request.Action = action;
                request.objHDTC = objHDTC;

                // make a call to service client here
                response = client.TaiSanDamBao(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objHDTC = response.objHDTC;
                    return true;
                }
                else
                {
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

        public bool DanhSachHopDongTheChap(DatabaseConstant.Action action, ref List<int> lstID, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            TaiSanDamBaoServiceClient client = null;
            TaiSanDamBaoRequest request = null;
            TaiSanDamBaoResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanDamBaoService.layGiaTri());

                client = TaiSanDamBaoServiceClient(ApplicationConstant.SystemService.TaiSanDamBaoService);
                request = Common.Utilities.PrepareRequest(new TaiSanDamBaoServiceRef.TaiSanDamBaoRequest());
                response = new TaiSanDamBaoServiceRef.TaiSanDamBaoResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TD_DANH_SACH_HOP_DONG_THE_CHAP;
                request.Action = action;
                request.lstIdHopDongTheChap = lstID.ToArray();

                // make a call to service client here
                response = client.TaiSanDamBao(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    return true;
                }
                else
                {
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

        public DataSet GetHopDongTheChap(DataTable dt)
        {
            DataSet ds;
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
                request.dtThamSo = dt;
                request.inquiryName = "%";
                request.objectName = "INQ.CT.TD_HDTC";

                // make a call to service client here
                response = client.TruyVanMessage(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                ds = response.dsResult;

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
            return ds;
        }

        public DataSet GetDanhSachHopDongTheChap(DataTable dt)
        {
            DataSet ds;
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
                request.dtThamSo = dt;
                request.inquiryName = "%";
                request.objectName = "INQ.DS.TD_HDTC";

                // make a call to service client here
                response = client.TruyVanMessage(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                ds = response.dsResult;

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
            return ds;
        }

        public DataSet GetTreeDanhSachHopDongTheChap(DataTable dt)
        {
            DataSet ds;
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
                request.dtThamSo = dt;
                request.inquiryName = "TREE_DVI_PGD_CUM";
                request.objectName = "INQ.DS.TD_HDTC";

                // make a call to service client here
                response = client.TruyVanMessage(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                ds = response.dsResult;

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
            return ds;
        }
        #endregion

        #region Nhập xuất ngoại bảng
        public bool NhapNgoaiBang(DatabaseConstant.Action action, ref NHAP_XUAT_TSDB obj, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            TaiSanDamBaoServiceClient client = null;
            TaiSanDamBaoRequest request = null;
            TaiSanDamBaoResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanDamBaoService.layGiaTri());

                client = TaiSanDamBaoServiceClient(ApplicationConstant.SystemService.TaiSanDamBaoService);
                request = Common.Utilities.PrepareRequest(new TaiSanDamBaoServiceRef.TaiSanDamBaoRequest());
                response = new TaiSanDamBaoServiceRef.TaiSanDamBaoResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TSDB_HOP_DONG_THE_CHAP_NHAP;
                request.Action = action;
                request.objNhapXuat = obj;

                // make a call to service client here
                response = client.TaiSanDamBao(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.objNhapXuat;
                    return true;
                }
                else
                {
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

        public DataSet GetNhapNgoaiBang(DataTable dt)
        {
            DataSet ds;
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
                request.dtThamSo = dt;
                request.inquiryName = "%";
                request.objectName = "INQ.CT.NHAP_NGOAI_BANG_TSDB";

                // make a call to service client here
                response = client.TruyVanMessage(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                ds = response.dsResult;

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
            return ds;
        }

        public DataSet GetDanhSachTSNhapNgoaiBang(DataTable dt)
        {
            DataSet ds;
            TruyVanServiceClient client = null;
            TruyVanRequest request = null;
            TruyVanResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

                client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);

                //Khởi tạo request
                

                // make a call to service client here
                ds = client.TruyVanUDTT("_DS_SP_LAY_DANH_SACH_TAI_SAN_THE_CHAP_NHAP_XUAT", dt);

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
            return ds;
        }
        #endregion


        #region Tài sản đảm bảo TDTD
        public bool TaiSanDamBaoTDTD(DatabaseConstant.Action action, ref TDTD_TAI_SAN_DAM_BAO objTSDB, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            TaiSanDamBaoServiceClient client = null;
            TaiSanDamBaoRequest request = null;
            TaiSanDamBaoResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanDamBaoService.layGiaTri());

                client = TaiSanDamBaoServiceClient(ApplicationConstant.SystemService.TaiSanDamBaoService);
                request = Common.Utilities.PrepareRequest(new TaiSanDamBaoServiceRef.TaiSanDamBaoRequest());
                response = new TaiSanDamBaoServiceRef.TaiSanDamBaoResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDTD_TSDB_CT;
                request.Action = action;
                request.objTDTD_TSDB = objTSDB;

                // make a call to service client here
                response = client.TaiSanDamBao(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objTSDB = response.objTDTD_TSDB;
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

        public bool DanhSachTaiSanDamBaoTDTD(DatabaseConstant.Action action, ref List<TDTD_TAI_SAN_DAM_BAO> lstTSDB, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            TaiSanDamBaoServiceClient client = null;
            TaiSanDamBaoRequest request = null;
            TaiSanDamBaoResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanDamBaoService.layGiaTri());

                client = TaiSanDamBaoServiceClient(ApplicationConstant.SystemService.TaiSanDamBaoService);
                request = Common.Utilities.PrepareRequest(new TaiSanDamBaoServiceRef.TaiSanDamBaoRequest());
                response = new TaiSanDamBaoServiceRef.TaiSanDamBaoResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDTD_TSDB_DS;
                request.Action = action;
                request.lstTDTD_TSDB = lstTSDB.ToArray();

                // make a call to service client here
                response = client.TaiSanDamBao(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstTSDB = response.lstTDTD_TSDB.ToList();
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

        public DataSet GetDanhSachTaiSanDamBaoTDTD(DataTable dt)
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

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.TDTD_TSDB";
                request.inquiryName = "DANH_SACH";
                request.typePara = "UDTT";
                request.type = "Multi";

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

        #region Hợp đồng thế chấp TDTD
        public bool HopDongTheChapTDTD(DatabaseConstant.Action action, ref TDTD_HOP_DONG_TCHAP objHDTC, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            TaiSanDamBaoServiceClient client = null;
            TaiSanDamBaoRequest request = null;
            TaiSanDamBaoResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanDamBaoService.layGiaTri());

                client = TaiSanDamBaoServiceClient(ApplicationConstant.SystemService.TaiSanDamBaoService);
                request = Common.Utilities.PrepareRequest(new TaiSanDamBaoServiceRef.TaiSanDamBaoRequest());
                response = new TaiSanDamBaoServiceRef.TaiSanDamBaoResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDTD_HDTC_CT;
                request.Action = action;
                request.objTDTD_HDTC = objHDTC;

                // make a call to service client here
                response = client.TaiSanDamBao(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objHDTC = response.objTDTD_HDTC;
                    return true;
                }
                else
                {
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

        public bool DanhSachHopDongTheChapTDTD(DatabaseConstant.Action action, ref List<int> lstID, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            TaiSanDamBaoServiceClient client = null;
            TaiSanDamBaoRequest request = null;
            TaiSanDamBaoResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanDamBaoService.layGiaTri());

                client = TaiSanDamBaoServiceClient(ApplicationConstant.SystemService.TaiSanDamBaoService);
                request = Common.Utilities.PrepareRequest(new TaiSanDamBaoServiceRef.TaiSanDamBaoRequest());
                response = new TaiSanDamBaoServiceRef.TaiSanDamBaoResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TD_DANH_SACH_HOP_DONG_THE_CHAP;
                request.Action = action;
                request.lstIdHopDongTheChap = lstID.ToArray();

                // make a call to service client here
                response = client.TaiSanDamBao(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    return true;
                }
                else
                {
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

        public DataSet GetHopDongTheChapTDTD(DataTable dt)
        {
            DataSet ds;
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
                request.dtThamSo = dt;
                request.inquiryName = "TTIN_CHI_TIET";
                request.objectName = "INQ.CT.TDTD_HDTC";
                request.typePara = "UDTT";
                request.type = "Multi";

                // make a call to service client here
                response = client.TruyVanMessage(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                ds = response.dsResult;

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
            return ds;
        }

        public DataSet GetDanhSachHopDongTheChapTDTD(DataTable dt)
        {
            DataSet ds;
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
                request.dtThamSo = dt;
                request.inquiryName = "DANH_SACH";
                request.objectName = "INQ.DS.TDTD_HDTC";
                request.typePara = "UDTT";
                request.type = "Multi";

                // make a call to service client here
                response = client.TruyVanMessage(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                ds = response.dsResult;

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
            return ds;
        }

        public DataSet GetTreeDanhSachHopDongTheChapTDTD(DataTable dt)
        {
            DataSet ds;
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
                request.dtThamSo = dt;
                request.inquiryName = "TREE_DVI_PGD_CUM";
                request.objectName = "INQ.DS.TD_HDTC";

                // make a call to service client here
                response = client.TruyVanMessage(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                ds = response.dsResult;

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
            return ds;
        }
        #endregion
    }
}
