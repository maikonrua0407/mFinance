using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data;
using System.Drawing;
using System.IO;
using Presentation.Process.TruyVanServiceRef;
using Utilities.Common;
using Presentation.Process.TinDungServiceRef;
using Presentation.Process.Common;
using Presentation.Process.UtilitiesServiceRef;
using System.ServiceModel.Description;

namespace Presentation.Process
{
    public class HoaDonTienKyProcess
    {
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
        /// Khởi tạo service TinDungService
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        private TinDungServiceClient TinDungServiceClient(ApplicationConstant.SystemService service)
        {
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(service.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(service.layGiaTri());
            TinDungServiceClient Client = new TinDungServiceClient(basicHttpBinding, endpointAddress);

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

        #region Lấy thông tin lãi suất
        public DataSet getDSKhachHangBTV(string idCum, string ngayHienTai, string ngayThu, string idKhang)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());
            TruyVanServiceClient ClientTruyVan = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            try
            {
                DataTable dt = null;

                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@ID_CUM", "String", idCum);
                LDatatable.AddParameter(ref dt, "@NGAY_HIEN_TAI", "String", ngayHienTai);
                LDatatable.AddParameter(ref dt, "@NGAY_THU", "String", ngayThu);
                LDatatable.AddParameter(ref dt, "@ID_KHANG", "String", idKhang);
                request.dtThamSo = dt;
                request.objectName = "INQ.POPUP.DS_KHACH_HANG_HDTK_BTV";
                request.inquiryName = "DANH_SACH";

                // Lấy kết quả trả về
                response = ClientTruyVan.TruyVanMessage(request);

                // Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (ClientTruyVan.State == CommunicationState.Faulted)
                {
                    ClientTruyVan.Abort();
                }
                else
                {
                    ClientTruyVan.Close();
                }
            }
            return response.dsResult;
        }
        public DataSet getDSKhachHangBENTRE(string idCum, string ngayHienTai, string ngayThu, string idKhang)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());
            TruyVanServiceClient ClientTruyVan = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            try
            {
                DataTable dt = null;

                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@ID_CUM", "String", idCum);
                LDatatable.AddParameter(ref dt, "@NGAY_HIEN_TAI", "String", ngayHienTai);
                LDatatable.AddParameter(ref dt, "@NGAY_THU", "String", ngayThu);
                LDatatable.AddParameter(ref dt, "@ID_KHANG", "String", idKhang);
                request.dtThamSo = dt;
                request.objectName = "INQ.POPUP.DS_KHACH_HANG_HDTK_BENTRE";
                request.inquiryName = "DANH_SACH";

                // Lấy kết quả trả về
                response = ClientTruyVan.TruyVanMessage(request);

                // Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (ClientTruyVan.State == CommunicationState.Faulted)
                {
                    ClientTruyVan.Abort();
                }
                else
                {
                    ClientTruyVan.Close();
                }
            }
            return response.dsResult;
        }
        public DataSet getDSKhachHangPhuTho(string idCum, string ngayHienTai, string ngayThu, string idKhang)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());
            TruyVanServiceClient ClientTruyVan = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            try
            {
                DataTable dt = null;

                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@ID_CUM", "String", idCum);
                LDatatable.AddParameter(ref dt, "@NGAY_HIEN_TAI", "String", ngayHienTai);
                LDatatable.AddParameter(ref dt, "@NGAY_THU", "String", ngayThu);
                LDatatable.AddParameter(ref dt, "@ID_KHANG", "String", idKhang);
                request.dtThamSo = dt;
                request.objectName = "INQ.POPUP.DS_KHACH_HANG_HDTK_PHUTHO";
                request.inquiryName = "DANH_SACH";

                // Lấy kết quả trả về
                response = ClientTruyVan.TruyVanMessage(request);

                // Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (ClientTruyVan.State == CommunicationState.Faulted)
                {
                    ClientTruyVan.Abort();
                }
                else
                {
                    ClientTruyVan.Close();
                }
            }
            return response.dsResult;
        }

        public DataSet getDSKhachHangQuangBinh(string idCum, string ngayHienTai, string ngayThu, string idKhang)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());
            TruyVanServiceClient ClientTruyVan = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            try
            {
                DataTable dt = null;

                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@ID_CUM", "String", idCum);
                LDatatable.AddParameter(ref dt, "@NGAY_HIEN_TAI", "String", ngayHienTai);
                LDatatable.AddParameter(ref dt, "@NGAY_THU", "String", ngayThu);
                LDatatable.AddParameter(ref dt, "@ID_KHANG", "String", idKhang);
                request.dtThamSo = dt;
                request.objectName = "INQ.POPUP.DS_KHACH_HANG_HDTK_QUANGBINH";
                request.inquiryName = "DANH_SACH";

                // Lấy kết quả trả về
                response = ClientTruyVan.TruyVanMessage(request);

                // Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (ClientTruyVan.State == CommunicationState.Faulted)
                {
                    ClientTruyVan.Abort();
                }
                else
                {
                    ClientTruyVan.Close();
                }
            }
            return response.dsResult;
        }

        public DataSet getDSKhachHangBIDV(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());
            TruyVanServiceClient ClientTruyVan = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            try
            {
                request.typePara = "UDTT";
                request.type = "Multi";
                request.dtThamSo = dt;
                request.objectName = "INQ.POPUP.DS_KHACH_HANG_HDTK_BIDV";
                request.inquiryName = "%";

                // Lấy kết quả trả về
                response = ClientTruyVan.TruyVanMessage(request);

                // Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (ClientTruyVan.State == CommunicationState.Faulted)
                {
                    ClientTruyVan.Abort();
                }
                else
                {
                    ClientTruyVan.Close();
                }
            }
            return response.dsResult;
        }
        #endregion

        #region Lấy thông tin sổ tiết kiệm trong nhóm
        public DataSet getDSSoTKBTV(string maKheUoc,string ngayGDich, string idGDich)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());
            TruyVanServiceClient ClientTruyVan = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            try
            {
                DataTable dt = null;

                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@MA_KUOCVM", "String", maKheUoc);
                LDatatable.AddParameter(ref dt, "@NGAY_GDICH", "String", ngayGDich);
                LDatatable.AddParameter(ref dt, "@ID_GDICH", "String", idGDich);
                request.dtThamSo = dt;
                request.objectName = "INQ.DS_SO_TK_HDTK_BTV";
                request.inquiryName = "POPUP";

                // Lấy kết quả trả về
                response = ClientTruyVan.TruyVanMessage(request);

                // Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (ClientTruyVan.State == CommunicationState.Faulted)
                {
                    ClientTruyVan.Abort();
                }
                else
                {
                    ClientTruyVan.Close();
                }
            }

            return response.dsResult;
        }
        #endregion

        #region Lấy thông tin khe uoc trong nhóm
        public DataSet getThongTinKheUocQB(string maKheUoc)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());
            TruyVanServiceClient ClientTruyVan = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            try
            {
                DataTable dt = null;

                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@MA_KUOCVM", "String", maKheUoc);
                request.dtThamSo = dt;
                request.objectName = "INQ.DS_KHE_UOC_QB";
                request.inquiryName = "POPUP";

                // Lấy kết quả trả về
                response = ClientTruyVan.TruyVanMessage(request);

                // Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (ClientTruyVan.State == CommunicationState.Faulted)
                {
                    ClientTruyVan.Abort();
                }
                else
                {
                    ClientTruyVan.Close();
                }
            }

            return response.dsResult;
        }
        #endregion

        #region Thu gốc lãi vay
        public ApplicationConstant.ResponseStatus ThuGocLaiVay(DatabaseConstant.Function function, DatabaseConstant.Action action, ref TDVM_LAP_HOA_DON_TIEN_KY obj, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungService.layGiaTri());
            TinDungServiceClient ClientTinDung = TinDungServiceClient(ApplicationConstant.SystemService.TinDungService);
            // Khởi tạo và gán các giá trị cho request
            TinDungServiceRef.TinDungViMoRequest request = Common.Utilities.PrepareRequest(new TinDungServiceRef.TinDungViMoRequest());

            TinDungServiceRef.TinDungViMoResponse response = new TinDungServiceRef.TinDungViMoResponse();
            try
            {
                request.Function = function;
                request.Action = action;
                request.objLapHoaDonTienKy = obj;

                // Lấy kết quả trả về
                response = ClientTinDung.TinDungViMo(request);

                // Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                obj = response.objLapHoaDonTienKy;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (ClientTinDung.State == CommunicationState.Faulted)
                {
                    ClientTinDung.Abort();
                }
                else
                {
                    ClientTinDung.Close();
                }
            }

            return response.ResponseStatus;
        }
        #endregion
    }
}
