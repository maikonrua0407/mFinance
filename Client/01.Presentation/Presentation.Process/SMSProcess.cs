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
using Presentation.Process.SMSServiceRef;
using Presentation.Process.Common;
using Presentation.Process.UtilitiesServiceRef;
using System.ServiceModel.Description;

namespace Presentation.Process
{
    public class SMSProcess
    {
        private SMSServiceClient SMSServiceClient(ApplicationConstant.SystemService service)
        {
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(service.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(service.layGiaTri());
            SMSServiceClient Client = new SMSServiceClient(basicHttpBinding, endpointAddress);

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

        public int DichVu(DatabaseConstant.Action action, ref DICHVU objDichVu,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            SMSServiceClient client = null;
            SMSRequest request = null;
            SMSResponse response = null;
            int iret = 1;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.SMSService.layGiaTri());

                client = SMSServiceClient(ApplicationConstant.SystemService.SMSService);
                request = Common.Utilities.PrepareRequest(new SMSRequest());
                response = new SMSResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.SMS_DANG_KY_DVU;
                request.Action = action;
                request.objDichVu = objDichVu;

                // make a call to service client here
                response = client.SMS(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response == null)
                {
                    iret = 0;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    objDichVu = response.objDichVu;
                    if (response.ResponseStatus.Equals(ApplicationConstant.ResponseStatus.THANH_CONG))
                        iret = 1;
                    else
                        iret = 0;
                }
            }
            catch (Exception ex)
            {
                iret = 0;
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
            return iret;
        }

        public int LoaiDoiTuong(DatabaseConstant.Action action, ref LOAI_DOI_TUONG objLoaiDoiTuong,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            SMSServiceClient client = null;
            SMSRequest request = null;
            SMSResponse response = null;
            int iret = 1;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.SMSService.layGiaTri());

                client = SMSServiceClient(ApplicationConstant.SystemService.SMSService);
                request = Common.Utilities.PrepareRequest(new SMSRequest());
                response = new SMSResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.SMS_DANG_KY_LOAI_DTUONG;
                request.Action = action;
                request.objLoaiDoiTuong = objLoaiDoiTuong;

                // make a call to service client here
                response = client.SMS(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response == null)
                {
                    iret = 0;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    objLoaiDoiTuong = response.objLoaiDoiTuong;
                    if (response.ResponseStatus.Equals(ApplicationConstant.ResponseStatus.THANH_CONG))
                        iret = 1;
                    else
                        iret = 0;
                }
            }
            catch (Exception ex)
            {
                iret = 0;
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
            return iret;
        }

        public int DoiTuong(DatabaseConstant.Action action, ref DOI_TUONG objDoiTuong,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            SMSServiceClient client = null;
            SMSRequest request = null;
            SMSResponse response = null;
            int iret = 1;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.SMSService.layGiaTri());

                client = SMSServiceClient(ApplicationConstant.SystemService.SMSService);
                request = Common.Utilities.PrepareRequest(new SMSRequest());
                response = new SMSResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.SMS_DANG_KY_DTUONG;
                request.Action = action;
                request.objDoiTuong = objDoiTuong;

                // make a call to service client here
                response = client.SMS(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response == null)
                {
                    iret = 0;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    objDoiTuong = response.objDoiTuong;
                    if (response.ResponseStatus.Equals(ApplicationConstant.ResponseStatus.THANH_CONG))
                        iret = 1;
                    else
                        iret = 0;
                }
            }
            catch (Exception ex)
            {
                iret = 0;
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
            return iret;
        }

        public int TinNhanDen(DatabaseConstant.Action action, ref TIN_NHAN_DEN objTinNhanDen,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            SMSServiceClient client = null;
            SMSRequest request = null;
            SMSResponse response = null;
            int iret = 1;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.SMSService.layGiaTri());

                client = SMSServiceClient(ApplicationConstant.SystemService.SMSService);
                request = Common.Utilities.PrepareRequest(new SMSRequest());
                response = new SMSResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.SMS_DSACH_TNHAN_DEN;
                request.Action = action;
                request.objTinNhanDen = objTinNhanDen;

                // make a call to service client here
                response = client.SMS(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response == null)
                {
                    iret = 0;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    objTinNhanDen = response.objTinNhanDen;
                    if (response.ResponseStatus.Equals(ApplicationConstant.ResponseStatus.THANH_CONG))
                        iret = 1;
                    else
                        iret = 0;
                }
            }
            catch (Exception ex)
            {
                iret = 0;
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
            return iret;
        }

        public int TinNhanDi(DatabaseConstant.Action action, ref TIN_NHAN_DI objTinNhanDi,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            SMSServiceClient client = null;
            SMSRequest request = null;
            SMSResponse response = null;
            int iret = 1;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.SMSService.layGiaTri());

                client = SMSServiceClient(ApplicationConstant.SystemService.SMSService);
                request = Common.Utilities.PrepareRequest(new SMSRequest());
                response = new SMSResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.SMS_DSACH_TNHAN_DI;
                request.Action = action;
                request.objTinNhanDi = objTinNhanDi;

                // make a call to service client here
                response = client.SMS(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response == null)
                {
                    iret = 0;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    objTinNhanDi = response.objTinNhanDi;
                    if (response.ResponseStatus.Equals(ApplicationConstant.ResponseStatus.THANH_CONG))
                        iret = 1;
                    else
                        iret = 0;
                }
            }
            catch (Exception ex)
            {
                iret = 0;
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
            return iret;
        }

        public int Modem(DatabaseConstant.Action action, ref List<MODEM> lstModems,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            SMSServiceClient client = null;
            SMSRequest request = null;
            SMSResponse response = null;
            int iret = 1;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.SMSService.layGiaTri());

                client = SMSServiceClient(ApplicationConstant.SystemService.SMSService);
                request = Common.Utilities.PrepareRequest(new SMSRequest());
                response = new SMSResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.SMS_QUAN_LY_KET_NOI;
                request.Action = action;
                request.lstModems = lstModems.ToArray();

                // make a call to service client here
                response = client.SMS(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response == null)
                {
                    iret = 0;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    lstModems = response.lstModems.ToList();
                    if (response.ResponseStatus.Equals(ApplicationConstant.ResponseStatus.THANH_CONG))
                        iret = 1;
                    else
                        iret = 0;
                }
            }
            catch (Exception ex)
            {
                iret = 0;
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
            return iret;
        }

        public int LayTinNhanDen(DatabaseConstant.Action action, ref List<MODEM> lstModems, ref TIN_NHAN_DEN objTinNhanDen,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            SMSServiceClient client = null;
            SMSRequest request = null;
            SMSResponse response = null;
            int iret = 1;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.SMSService.layGiaTri());

                client = SMSServiceClient(ApplicationConstant.SystemService.SMSService);
                request = Common.Utilities.PrepareRequest(new SMSRequest());
                response = new SMSResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.SMS_DSACH_TNHAN_DEN;
                request.Action = action;
                request.lstModems = lstModems.ToArray();
                request.objTinNhanDen = objTinNhanDen;

                // make a call to service client here
                response = client.SMS(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response == null)
                {
                    iret = 0;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    lstModems = response.lstModems.ToList();
                    objTinNhanDen = response.objTinNhanDen;
                    if (response.ResponseStatus.Equals(ApplicationConstant.ResponseStatus.THANH_CONG))
                        iret = 1;
                    else
                        iret = 0;
                }
            }
            catch (Exception ex)
            {
                iret = 0;
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
            return iret;
        }

    }
}
