using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BusinessServices.QuanTriHeThong;
using BusinessServices.Utilities.DTO;
using DataModel.EntityFramework;
using CommunicationServices.QuanTriHeThong.Messages;
using Utilities.Common;
using CommunicationContracts.Base.ContractBases;
using CommunicationMessages.Base.MessageBases;
using System.Collections;
using System.ServiceModel.Activation;
using BusinessServices.QuanTriHeThong.DTO;
using BusinessServices.QuanTriHeThong.Function;
using BusinessServices.Utilities;
using System.Reflection;

namespace CommunicationServices.QuanTriHeThong
{
    [ServiceBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class QuanTriHeThongService : ServiceBase, IQuanTriHeThongService
    {
        public string returnHello()
        {
            return "Hello";
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maChucNang"></param>
        /// <returns></returns>
        public HT_CNANG LayChucNangTheoMa(string maChucNang)
        {
            BS_QuanTriHeThong bsService = new BS_QuanTriHeThong();
            HT_CNANG result = bsService.LayChucNangTheoMa(maChucNang);
            return result;
        }

        public List<HT_CNANG> LayChucNangTheoMaCha(string maChucNangCha)
        {
            BS_QuanTriHeThong bsService = new BS_QuanTriHeThong();
            List<HT_CNANG> result = bsService.LayChucNangTheoMaCha(maChucNangCha);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maDangNhap"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public List<HT_CNANG> LayChucNangTheoQuyen(string maDangNhap, string passWord)
        {
            BS_QuanTriHeThong bsService = new BS_QuanTriHeThong();
            List<HT_CNANG> result = bsService.LayChucNangTheoQuyen(maDangNhap, passWord);
            return result;
        }

        /// <summary>
        /// Lấy danh sách ngày làm việc trong tháng
        /// </summary>
        /// <param name="year">Năm cần kiểm tra</param>
        /// <param name="month">Tháng cần kiểm tra</param>
        /// <returns>Trả lại danh sách theo từng ngày trong tháng chứa giá trị true là ngày làm việc, false là ngày nghỉ</returns>
        public LayDSNgayLamViecResponse LayDSNgayLamViec(LayDSNgayLamViecRequest request)
        {
            LayDSNgayLamViecResponse response = new LayDSNgayLamViecResponse();
            response.ResponseId = request.RequestId;

            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                List<bool> result = (new BS_LichLamViec()).LayDSNgayLamViec(request.Year, request.Month);
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;
                response.ListNgayLamViec = result;
                int i = 0;
                int a = 2 / i;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                response.ResponseMessage = ApplicationConstant.LichLamViecResponseMessage.M_Response_LichLamViec_LayDSNgayLamViec.layGiaTri();
                response.ExceptionObject = new CustomException(ex).Serialize();
            }
            return response;
        }

        /// <summary>
        /// Cập nhật toàn bộ bảng HT_LICH
        /// </summary>
        /// <param name="value">Danh sách các object dữ liệu lịch</param>
        public LuuLichResponse LuuLich(LuuLichRequest request)
        {
            LuuLichResponse response = new LuuLichResponse();
            response.ResponseId = request.RequestId;

            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                (new BS_LichLamViec()).LuuLich(request.ListHT_LICH);
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;
                
                return response;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                response.ResponseMessage = ApplicationConstant.LichLamViecResponseMessage.M_Response_LichLamViec_LuuLich.layGiaTri();
                response.ExceptionObject = new CustomException(ex).Serialize();
            }
            return response;
        }

        public PhanQuyenResponse layCNangTNang(PhanQuyenRequest request)
        {
            PhanQuyenResponse response = new PhanQuyenResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.ListChucNangTinhNang = new BS_PhanQuyen().layCNangTNang();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public PhanQuyenResponse layCNangTNangTheoListIdChucNang(PhanQuyenRequest request)
        {
            PhanQuyenResponse response = new PhanQuyenResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.ListChucNangTinhNang = new BS_PhanQuyen().layCNangTNangTheoListIdChucNang(request.lstIdChucNang);
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public PhanQuyenResponse layTNangDuocPhanQuyen(PhanQuyenRequest request)
        {
            PhanQuyenResponse response = new PhanQuyenResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.ListTinhNang = new BS_PhanQuyen().layTNangDuocPhanQuyen();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public PhanQuyenResponse layCNang(PhanQuyenRequest request)
        {
            PhanQuyenResponse response = new PhanQuyenResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.ListChucNang = new BS_PhanQuyen().layCNang();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public PhanQuyenResponse layCNangTheoPhanHe(PhanQuyenRequest request)
        {
            PhanQuyenResponse response = new PhanQuyenResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.ListChucNang = new BS_PhanQuyen().layCNangTheoPhanHe(request.maPhanHe);
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public PhanQuyenResponse layCNangThietLapAPTheoPhanHe(PhanQuyenRequest request)
        {
            PhanQuyenResponse response = new PhanQuyenResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.ListChucNang = new BS_PhanQuyen().layCNangThietLapAPTheoPhanHe(request.maPhanHe);
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public PhanQuyenResponse layCNangPQuyen(PhanQuyenRequest request)
        {
            PhanQuyenResponse response = new PhanQuyenResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.ListChucNangPhanQuyen = new BS_PhanQuyen().layCNangPQuyen();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public PhanQuyenResponse layCNangPQuyenTheoDoiTuong(PhanQuyenRequest request)
        {
            PhanQuyenResponse response = new PhanQuyenResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.ListChucNangPhanQuyen = new BS_PhanQuyen().layCNangPQuyenTheoDoiTuong(request.maDoiTuong, request.loaiDoiTuong);
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public PhanQuyenResponse layCNangPQuyenTheoDoiTuongChucNang(PhanQuyenRequest request)
        {
            PhanQuyenResponse response = new PhanQuyenResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.ListChucNangPhanQuyen = new BS_PhanQuyen().layCNangPQuyenTheoDoiTuongChucNang(request.maDoiTuong, request.loaiDoiTuong, request.lstIdChucNang);
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public PhanQuyenResponse layTNang(PhanQuyenRequest request)
        {
            PhanQuyenResponse response = new PhanQuyenResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.ListTinhNang = new BS_PhanQuyen().layTNang();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public PhanQuyenResponse layTNangTheoListIdTinhNang(PhanQuyenRequest request)
        {
            PhanQuyenResponse response = new PhanQuyenResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.ListTinhNang = new BS_PhanQuyen().layTNangTheoListIdTinhNang(request.lstIdTinhNang);
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public PhanQuyenResponse layTNguyen(PhanQuyenRequest request)
        {
            PhanQuyenResponse response = new PhanQuyenResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.ListTaiNguyen = new BS_PhanQuyen().layTNguyen(request.maTaiNguyen, request.loaiTaiNguyen);
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public PhanQuyenResponse layDSTNguyenKThac(PhanQuyenRequest request)
        {
            PhanQuyenResponse response = new PhanQuyenResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.ListTaiNguyenKhaiThac = new BS_PhanQuyen().layDSTNguyenKThac(request.maDoiTuong, request.loaiDoiTuong, request.loaiTaiNguyen);
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public PhanQuyenResponse luuPhanQuyen(PhanQuyenRequest request)
        {
            PhanQuyenResponse response = new PhanQuyenResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                ArrayList lstPhanQuyen = new ArrayList();
                foreach (var item in request.lstPhanQuyen)
                {
                    List<string> lst = item.lstChucNang;
                    lstPhanQuyen.Add(lst);
                }
                response.bKetQua = new BS_PhanQuyen().luuPhanQuyen(request.maDoiTuong, request.loaiDoiTuong, lstPhanQuyen, request.nguoiCapNhat);
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public PhanQuyenResponse luuPhanQuyenChucNang(PhanQuyenRequest request)
        {
            PhanQuyenResponse response = new PhanQuyenResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                ApplicationConstant.QuanTriHeThongResponseMessage responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                ApplicationConstant.ResponseStatus responseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                BS_ResponseDetail bsResponseDetail = new BS_ResponseDetail();
                bool KetQua = true;

                responseStatus = new BS_PhanQuyen().luuPhanQuyenChucNang(
                    request.loaiDoiTuong,
                    request.idDoiTuong,
                    request.maDoiTuong,
                    request.maDonVi,
                    request.UserName,
                    DateTime.Today.ToString("yyyyMMdd"),
                    request.dsCNangPQuyenXoa,
                    request.dsCNangTNangThem,
                    ref responseMessage, 
                    ref bsResponseDetail);

                KetQua = responseStatus == ApplicationConstant.ResponseStatus.THANH_CONG ? true : false;
                response.bKetQua = KetQua;
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;
                response.ResponseMessage = responseMessage.ToString();

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public NguoiDungResponse layDanhSachDonVi(NguoiDungRequest request)
        {
            NguoiDungResponse response = new NguoiDungResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.ListDonVi = new BS_QuanTriHeThong().layDanhSachDonVi();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            finally
            {
            }
            return response;
        }

        public NguoiDungResponse layThongTinCaNhan(NguoiDungRequest request)
        {
            NguoiDungResponse response = new NguoiDungResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.NSD = new BS_QuanTriHeThong().layThongTinCaNhan(request.UserId, request.UserName, request.DeptCode);
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            finally
            {
            }
            return response;
        }

        public NguoiDungResponse layNSD(NguoiDungRequest request)
        {
            NguoiDungResponse response = new NguoiDungResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.ListNSD = new BS_QuanTriHeThong().layNSD(request.UserType, request.DeptCode);
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            finally
            {
            }
            return response;
        }

        public NguoiDungResponse layNhomNSD(NguoiDungRequest request)
        {
            NguoiDungResponse response = new NguoiDungResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.ListNHNSD = new BS_QuanTriHeThong().layNhomNSD(request.UserType, request.DeptCode);
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            finally
            {
            }
            return response;
        }

        public NguoiDungResponse layNSDTheoNhom(NguoiDungRequest request)
        {
            NguoiDungResponse response = new NguoiDungResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.ListNSD = new BS_QuanTriHeThong().layNSDTheoNhom(request.objNHNSD.ID);
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public NguoiDungResponse layNhomTheoNSD(NguoiDungRequest request)
        {
            NguoiDungResponse response = new NguoiDungResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.ListNHNSD = new BS_QuanTriHeThong().layNhomTheoNSD(request.objNSD.ID);
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public NguoiDungResponse layTruyCapTheoNSD(NguoiDungRequest request)
        {
            NguoiDungResponse response = new NguoiDungResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.lstTruyCap = new BS_QuanTriHeThong().LayTruyCapTheoNSD(request.objNSD.ID);
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public NguoiDungResponse layTruyCapTheoNhomNSD(NguoiDungRequest request)
        {
            NguoiDungResponse response = new NguoiDungResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.lstTruyCap = new BS_QuanTriHeThong().LayTruyCapTheoNhomNSD(request.objNHNSD.ID);
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public NguoiDungResponse layPhamViPhongGDTheoNSDVaMaLoaiPhamVi(NguoiDungRequest request)
        {
            NguoiDungResponse response = new NguoiDungResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                List<BS_PhamVi> listBsPhamVi = new List<BS_PhamVi>();
                listBsPhamVi = new BS_QuanTriHeThong().layPhamViTheoIdNSDVaMaLoaiPhamVi(request.objNSD.ID, request.maLoaiPhamVi);

                List<PhamViDto> listPhamVi = new List<PhamViDto>();
                foreach (BS_PhamVi item in listBsPhamVi)
                {
                    PhamViDto bsPhamVi = new PhamViDto();
                    bsPhamVi.IdLoaiPvi = item.IdLoaiPvi;
                    bsPhamVi.MaLoaiPvi = item.MaLoaiPvi;
                    bsPhamVi.IdPvi = item.IdPvi;
                    bsPhamVi.MaPvi = item.MaPvi;
                    bsPhamVi.TenPvi = item.TenPvi;
                    listPhamVi.Add(bsPhamVi);
                }

                response.listPhamVi = listPhamVi;
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public NguoiDungResponse ThemNSD(NguoiDungRequest request)
        {
            NguoiDungResponse response = new NguoiDungResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                ApplicationConstant.QuanTriHeThongResponseMessage responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                ApplicationConstant.ResponseStatus responseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                BS_ResponseDetail bsResponseDetail = new BS_ResponseDetail();
                HT_NSD htNsd = request.objNSD;
                bool KetQua = true;

                // Thông tin phạm vi
                List<BS_PhamVi> listPhamVi = new List<BS_PhamVi>();
                foreach (PhamViDto item in request.listPhamVi)
                {
                    BS_PhamVi bsPhamVi = new BS_PhamVi();
                    bsPhamVi.IdLoaiPvi = item.IdLoaiPvi;
                    bsPhamVi.MaLoaiPvi = item.MaLoaiPvi;
                    bsPhamVi.IdPvi = item.IdPvi;
                    bsPhamVi.MaPvi = item.MaPvi;
                    bsPhamVi.TenPvi = item.TenPvi;
                    listPhamVi.Add(bsPhamVi);
                }

                responseStatus = new BS_QuanTriHeThong().ThemNSD(ref htNsd, request.lstIdNHNSD, listPhamVi, request.lstTruyCap, ref responseMessage, ref bsResponseDetail);
                if (responseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    KetQua = true;
                }
                else
                {
                    KetQua = false;
                }

                response.ResponseStatus = responseStatus;
                response.ResponseMessage = responseMessage.layGiaTri();
                response.NSD = htNsd;
                response.bKetQua = KetQua;

                List<BS_ResponseDetail> lstBsResponseDetail = new List<BS_ResponseDetail>();
                lstBsResponseDetail.Add(bsResponseDetail);
                List<ResponseDetail> responseDetail = convertToResponseDetail(lstBsResponseDetail);
                response.ResponseDetail = responseDetail;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            finally
            { 
            }
            return response;
        }

        public NguoiDungResponse SuaNSD(NguoiDungRequest request)
        {
            NguoiDungResponse response = new NguoiDungResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                ApplicationConstant.QuanTriHeThongResponseMessage responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                ApplicationConstant.ResponseStatus responseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                HT_NSD htNsd = request.objNSD;
                BS_ResponseDetail bsResponseDetail = new BS_ResponseDetail();
                bool KetQua = true;

                // Thông tin phạm vi
                List<BS_PhamVi> listPhamVi = new List<BS_PhamVi>();
                foreach (PhamViDto item in request.listPhamVi)
                {
                    BS_PhamVi bsPhamVi = new BS_PhamVi();
                    bsPhamVi.IdLoaiPvi = item.IdLoaiPvi;
                    bsPhamVi.MaLoaiPvi = item.MaLoaiPvi;
                    bsPhamVi.IdPvi = item.IdPvi;
                    bsPhamVi.MaPvi = item.MaPvi;
                    bsPhamVi.TenPvi = item.TenPvi;
                    listPhamVi.Add(bsPhamVi);
                }

                responseStatus = new BS_QuanTriHeThong().SuaNSD(ref htNsd, request.lstIdNHNSD, listPhamVi, request.lstTruyCap, ref responseMessage, ref bsResponseDetail);
                if (responseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    KetQua = true;
                }
                else
                {
                    KetQua = false;
                }

                response.ResponseStatus = responseStatus;
                response.ResponseMessage = responseMessage.layGiaTri();
                response.NSD = htNsd;
                response.bKetQua = KetQua;

                List<BS_ResponseDetail> lstBsResponseDetail = new List<BS_ResponseDetail>();
                lstBsResponseDetail.Add(bsResponseDetail);
                List<ResponseDetail> responseDetail = convertToResponseDetail(lstBsResponseDetail);
                response.ResponseDetail = responseDetail;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            finally
            {
            }
            return response;
        }

        public NguoiDungResponse XoaListNSDTheoID(NguoiDungRequest request)
        {
            NguoiDungResponse response = new NguoiDungResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                ApplicationConstant.QuanTriHeThongResponseMessage responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                ApplicationConstant.ResponseStatus responseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<BS_ResponseDetail> bsResponseDetail = new List<BS_ResponseDetail>();
                bool KetQua = true;

                responseStatus = new BS_QuanTriHeThong().XoaListNSDTheoId(request.listID, ref responseMessage, ref bsResponseDetail);
                if (responseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    KetQua = true;
                }
                else
                {
                    KetQua = false;
                }

                response.ResponseStatus = responseStatus;
                response.ResponseMessage = responseMessage.layGiaTri();
                response.bKetQua = KetQua;

                List<ResponseDetail> responseDetail = convertToResponseDetail(bsResponseDetail);
                response.ResponseDetail = responseDetail;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            finally
            {
            }
            return response;
        }

        public NguoiDungResponse ThemNHNSD(NguoiDungRequest request)
        {
            NguoiDungResponse response = new NguoiDungResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                ApplicationConstant.QuanTriHeThongResponseMessage responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                ApplicationConstant.ResponseStatus responseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                BS_ResponseDetail bsResponseDetail = new BS_ResponseDetail();
                HT_NHNSD htNhNsd = request.objNHNSD;
                bool KetQua = true;

                responseStatus = new BS_QuanTriHeThong().ThemNHNSD(ref htNhNsd, request.lstIdNSD, request.lstTruyCap, ref responseMessage, ref bsResponseDetail);
                if (responseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    KetQua = true;
                }
                else
                {
                    KetQua = false;
                }

                response.ResponseStatus = responseStatus;
                response.ResponseMessage = responseMessage.layGiaTri();
                response.NHNSD = htNhNsd;
                response.bKetQua = KetQua;

                List<BS_ResponseDetail> lstBsResponseDetail = new List<BS_ResponseDetail>();
                lstBsResponseDetail.Add(bsResponseDetail);
                List<ResponseDetail> responseDetail = convertToResponseDetail(lstBsResponseDetail);
                response.ResponseDetail = responseDetail;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            finally
            {
            }
            return response;
        }

        public NguoiDungResponse SuaNHNSD(NguoiDungRequest request)
        {
            NguoiDungResponse response = new NguoiDungResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                ApplicationConstant.QuanTriHeThongResponseMessage responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                ApplicationConstant.ResponseStatus responseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                BS_ResponseDetail bsResponseDetail = new BS_ResponseDetail();
                HT_NHNSD htNhNsd = request.objNHNSD;
                bool KetQua = true;

                responseStatus = new BS_QuanTriHeThong().SuaNHNSD(ref htNhNsd, request.lstIdNSD, request.lstTruyCap, ref responseMessage, ref bsResponseDetail);
                if (responseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    KetQua = true;
                }
                else
                {
                    KetQua = false;
                }

                response.ResponseStatus = responseStatus;
                response.ResponseMessage = responseMessage.layGiaTri();
                response.NHNSD = htNhNsd;
                response.bKetQua = KetQua;                

                List<BS_ResponseDetail> lstBsResponseDetail = new List<BS_ResponseDetail>();
                lstBsResponseDetail.Add(bsResponseDetail);
                List<ResponseDetail> responseDetail = convertToResponseDetail(lstBsResponseDetail);
                response.ResponseDetail = responseDetail;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            finally
            {
            }
            return response;
        }

        public NguoiDungResponse XoaListNHNSDTheoID(NguoiDungRequest request)
        {
            NguoiDungResponse response = new NguoiDungResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                ApplicationConstant.QuanTriHeThongResponseMessage responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                ApplicationConstant.ResponseStatus responseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<BS_ResponseDetail> bsResponseDetail = new List<BS_ResponseDetail>();
                bool KetQua = true;

                responseStatus = new BS_QuanTriHeThong().XoaListNHNSDTheoId(request.listID, ref responseMessage, ref bsResponseDetail);
                if (responseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    KetQua = true;
                }
                else
                {
                    KetQua = false;
                }

                response.ResponseStatus = responseStatus;
                response.ResponseMessage = responseMessage.layGiaTri();
                response.bKetQua = KetQua;

                List<ResponseDetail> responseDetail = convertToResponseDetail(bsResponseDetail);
                response.ResponseDetail = responseDetail;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            finally
            {
            }
            return response;
        }

        public ThamSoResponse layThamSo(ThamSoRequest request)
        {
            ThamSoResponse response = new ThamSoResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.ListThamSo = new BS_QuanTriHeThong().layThamSo();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public ThamSoResponse layThamSoHeThong(ThamSoRequest request)
        {
            ThamSoResponse response = new ThamSoResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }
                ApplicationConstant.ResponseStatus responseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<HT_TSO> lstHtTso = null;

                responseStatus = new BS_QuanTriHeThong().layThamSoHeThong(request.maDonVi, request.loaiThamSo, ref lstHtTso);

                if (responseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;
                    response.ListThamSo = lstHtTso;
                }
                else
                {
                    response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                }
                
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public ThamSoResponse layLoaiThamSo(ThamSoRequest request)
        {
            ThamSoResponse response = new ThamSoResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.ListLoaiThamSo = new BS_QuanTriHeThong().layLoaiThamSo();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public ThamSoResponse capNhatThamSo(ThamSoRequest request)
        {
            ThamSoResponse response = new ThamSoResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.ResponseStatus = new BS_QuanTriHeThong().capNhatThamSo(request.Action, request.objThamSo,
                    request.listID, ref response.bKetQua, ref response.iKetQua, ref response.responseMessage);
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public ThamSoResponse capNhatGiaTriThamSo(ThamSoRequest request)
        {
            ThamSoResponse response = new ThamSoResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                ApplicationConstant.QuanTriHeThongResponseMessage responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                ApplicationConstant.ResponseStatus responseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                BS_ResponseDetail bsResponseDetail = new BS_ResponseDetail();
                HT_TSO htTso = request.objThamSo;
                bool KetQua = true;

                responseStatus = new BS_QuanTriHeThong().capNhatGiaTriThamSo(ref htTso, ref responseMessage, ref bsResponseDetail);

                if (responseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    KetQua = true;
                }
                else
                {
                    KetQua = false;
                }

                response.ResponseStatus = responseStatus;
                response.ResponseMessage = responseMessage.layGiaTri();
                response.objThamSo = htTso;
                response.bKetQua = KetQua;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public ThamSoResponse capNhatLoaiThamSo(ThamSoRequest request)
        {
            ThamSoResponse response = new ThamSoResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.ResponseStatus = new BS_QuanTriHeThong().capNhatLoaiThamSo(request.Action, request.objLoaiThamSo,
                    request.listID, ref response.bKetQua, ref response.iKetQua, ref response.responseMessage);
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public NguoiDungResponse DoiMatKhauNguoiDungLucDangNhap(NguoiDungRequest request)
        {
            NguoiDungResponse response = new NguoiDungResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.SecurityKey))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }
                ApplicationConstant.QuanTriHeThongResponseMessage responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                ApplicationConstant.ResponseStatus responseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                bool KetQua = true;

                responseStatus = new BS_QuanTriHeThong().DoiMatKhauNguoiDung(request.maNguoiDung,
                    request.oldPassword,
                    request.newPassword,
                    ref responseMessage);

                if (responseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    KetQua = true;
                }
                else
                {
                    KetQua = false;
                }

                response.ResponseStatus = responseStatus;
                response.ResponseMessage = responseMessage.layGiaTri();
                response.bKetQua = KetQua;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public NguoiDungResponse DoiMatKhauNguoiDung(NguoiDungRequest request)
        {
            NguoiDungResponse response = new NguoiDungResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }
                ApplicationConstant.QuanTriHeThongResponseMessage responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                ApplicationConstant.ResponseStatus responseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                bool KetQua = true;

                responseStatus = new BS_QuanTriHeThong().DoiMatKhauNguoiDung(request.maNguoiDung,
                    request.oldPassword,
                    request.newPassword,
                    ref responseMessage);

                if (responseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    KetQua = true;
                }
                else
                {
                    KetQua = false;
                }

                response.ResponseStatus = responseStatus;
                response.ResponseMessage = responseMessage.layGiaTri();
                response.bKetQua = KetQua;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public NguoiDungResponse ThietLapMatKhauNguoiDung(NguoiDungRequest request)
        {
            NguoiDungResponse response = new NguoiDungResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                ApplicationConstant.QuanTriHeThongResponseMessage responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                ApplicationConstant.ResponseStatus responseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                bool KetQua = true;

                responseStatus = new BS_QuanTriHeThong().ThietLapMatKhauNguoiDung(request.maNguoiDung,
                    request.newPassword,
                    ref responseMessage);

                if (responseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    KetQua = true;
                }
                else
                {
                    KetQua = false;
                }

                response.ResponseStatus = responseStatus;
                response.ResponseMessage = responseMessage.layGiaTri();
                response.bKetQua = KetQua;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public PhienBanResponse CheckClientVersion(PhienBanRequest request)
        {
            PhienBanResponse response = new PhienBanResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                ApplicationConstant.QuanTriHeThongResponseMessage responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                ApplicationConstant.ResponseStatus responseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                string serverVersion = "";
                string lastestClientVersion = "";
                HT_PBAN htPban = new HT_PBAN();
                List<HT_PBAN_CTIET> listHtPbanCtiet = new List<HT_PBAN_CTIET>();
                List<HT_PBAN_FILE> listHtPbanFile = new List<HT_PBAN_FILE>();

                responseStatus = new BS_PhienBan().CheckClientVersion(request.ClientVersion, ref serverVersion, ref lastestClientVersion, 
                    ref htPban, ref listHtPbanCtiet, ref listHtPbanFile, ref responseMessage);

                response.ResponseStatus = responseStatus;
                response.ServerVersion = serverVersion;
                response.LastestClientVersion = lastestClientVersion;
                response.HtPban = htPban;
                response.ListHtPbanCtiet = listHtPbanCtiet;
                response.ListHtPbanFile = listHtPbanFile;
                response.ResponseMessage = responseMessage.layGiaTri();

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public PhienBanResponse DownloadClientVersion(PhienBanRequest request)
        {
            PhienBanResponse response = new PhienBanResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                ApplicationConstant.QuanTriHeThongResponseMessage responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                ApplicationConstant.ResponseStatus responseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                string serverVersion = "";
                string lastestClientVersion = "";
                BS_PhienBanDTO bsPhienBan = new BS_PhienBanDTO();

                responseStatus = new BS_PhienBan().DownloadClientVersion(request.ClientVersion, lastestClientVersion, request.HtPban, ref bsPhienBan, ref responseMessage);

                response.ResponseStatus = responseStatus;
                response.ServerVersion = serverVersion;
                response.LastestClientVersion = lastestClientVersion;
                response.ResponseMessage = responseMessage.layGiaTri();

                PhienBanDTO phienBanDTO = new PhienBanDTO();
                List<PhienBanItemDTO> listPhienBanItemDTO = new List<PhienBanItemDTO>();
                foreach (BS_PhienBanItemDTO item in bsPhienBan.ListPhienBanItem)
                {
                    PhienBanItemDTO phienBanItemDTO = new PhienBanItemDTO();
                    phienBanItemDTO.HtPbanCtiet = item.HtPbanCtiet;
                    phienBanItemDTO.HtPbanFile = item.HtPbanFile;

                    FileBase fileDuLieuPhienBan = new FileBase();
                    fileDuLieuPhienBan.FileName = item.HtPbanData.FileName;
                    fileDuLieuPhienBan.FileFormat = item.HtPbanData.FileFormat;
                    fileDuLieuPhienBan.FileData = item.HtPbanData.FileData;
                    phienBanItemDTO.HtPbanData = fileDuLieuPhienBan;

                    listPhienBanItemDTO.Add(phienBanItemDTO);
                }
                phienBanDTO.HtPban = bsPhienBan.HtPban;
                phienBanDTO.ListPhienBanItemDTO = listPhienBanItemDTO;

                response.PhienBanDTO = phienBanDTO;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public PhienBanResponse DownloadClientVersionItem(PhienBanRequest request)
        {
            PhienBanResponse response = new PhienBanResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                ApplicationConstant.QuanTriHeThongResponseMessage responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                ApplicationConstant.ResponseStatus responseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                string serverVersion = "";
                string lastestClientVersion = "";
                BS_PhienBanItemDTO bsPhienBanItem = new BS_PhienBanItemDTO();

                responseStatus = new BS_PhienBan().DownloadClientVersionItem(request.ClientVersion, lastestClientVersion, request.HtPban, request.HtPbanCtiet, ref bsPhienBanItem, ref responseMessage);

                response.ResponseStatus = responseStatus;
                response.ServerVersion = serverVersion;
                response.LastestClientVersion = lastestClientVersion;
                response.ResponseMessage = responseMessage.layGiaTri();

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        /*
        public NguoiDungResponse LayDanhSachPhongGD(NguoiDungRequest request)
        {
            NguoiDungResponse response = new NguoiDungResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                ApplicationConstant.QuanTriHeThongResponseMessage responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                ApplicationConstant.ResponseStatus responseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;
                
                // Lấy danh sách các phòng giao dịch mà người dùng được phân quyền
                string maLoaiPhamVi = BusinessConstant.LoaiPhamViDuLieu.PHONG_GIAO_DICH.layMaPhamVi();
                List<BS_PhamVi> ListPhamVi = new BS_QuanTriHeThong().layPhamViTheoIdNSDVaMaLoaiPhamVi(request.UserId, maLoaiPhamVi);
                List<DM_DON_VI> listPhongGD = new List<DM_DON_VI>();
                foreach (BS_PhamVi item in ListPhamVi)
                {
                    DM_DON_VI dmPhongGD = new DM_DON_VI();
                    dmPhongGD.ID = item.IdPvi;
                    dmPhongGD.MA_DVI = item.MaPvi;
                    dmPhongGD.TEN_GDICH = item.TenPvi;
                    dmPhongGD.MA_HACH_TOAN = item.PhuongPhapHachToan;
                    listPhongGD.Add(dmPhongGD);
                }

                response.ResponseStatus = responseStatus;
                response.ListPhongGD = listPhongGD;
                response.ResponseMessage = responseMessage.layGiaTri();

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }
        */

        public NguoiDungResponse LayDanhSachPhongGD(NguoiDungRequest request)
        {
            NguoiDungResponse response = new NguoiDungResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                ApplicationConstant.QuanTriHeThongResponseMessage responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
                ApplicationConstant.ResponseStatus responseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;

                // Lấy danh sách các phòng giao dịch mà người dùng được phân quyền
                string vpgd = DatabaseConstant.ToChucDonVi.VPGD.getValue();
                string pgd = DatabaseConstant.ToChucDonVi.PGD.getValue();
                List<DM_DON_VI> ListDonVi = new BS_CoNguoiSuDung().LayPhamViDuLieuDonVi(request.UserName, request.MngDeptCode);
                List<DM_DON_VI> listPhongGD = new List<DM_DON_VI>();
                if (ListDonVi != null)
                {
                    foreach (DM_DON_VI item in ListDonVi)
                    {
                        if (item.LOAI_DVI.Equals(vpgd) || item.LOAI_DVI.Equals(pgd))
                        {
                            DM_DON_VI dmPhongGD = new DM_DON_VI();
                            dmPhongGD.ID = item.ID;
                            dmPhongGD.MA_DVI = item.MA_DVI;
                            dmPhongGD.TEN_GDICH = item.TEN_GDICH;
                            dmPhongGD.MA_DVI_CHA = item.MA_DVI_CHA;
                            dmPhongGD.MA_HACH_TOAN = item.MA_HACH_TOAN;
                            listPhongGD.Add(dmPhongGD);
                        }
                    }
                }

                response.ResponseStatus = responseStatus;
                response.ListPhongGD = listPhongGD;
                response.ResponseMessage = responseMessage.layGiaTri();

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public PhamViResponse PhanQuyenPhamVi(PhamViRequest request)
        {
            PhamViResponse response = new PhamViResponse();
            THONG_TIN_CHUNG objThongTinChung = new THONG_TIN_CHUNG();
            ApplicationConstant.QuanTriHeThongResponseMessage responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
            ApplicationConstant.ResponseStatus responseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;
            List<BS_ResponseDetail> lstResponseDetail = new List<BS_ResponseDetail>();
            string ngayLamViec = "";

            #region Tạo thông tin chung
            objThongTinChung.MA_DON_VI = request.DeptCode;
            objThongTinChung.MA_DON_VI_GD = request.OprDeptCode;
            objThongTinChung.TEN_DANG_NHAP = request.UserName;
            objThongTinChung.MA_CHUC_NANG = request.Function.getValue();
            int iret = new BS_CoNgayLamViec().LayNgayLamViec(objThongTinChung.MA_DON_VI, out ngayLamViec);
            if (iret == 1)
            {
                objThongTinChung.NGAY_LAM_VIEC = ngayLamViec;
            }
            else
            {
                responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
            }
            #endregion
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                // Lấy danh sách các phòng giao dịch mà người dùng được phân quyền
                string maLoaiPhamVi = BusinessConstant.LoaiPhamViDuLieu.PHONG_GIAO_DICH.layMaPhamVi();
                responseStatus = new BS_QuanTriHeThong_Function().PhanQuyenPhamVi(request.Action, ref objThongTinChung, ref lstResponseDetail, ref request.objPhamVi, ref responseMessage);

                response.ResponseStatus = responseStatus;
                response.objPhamVi = request.objPhamVi;
                response.ResponseMessage = responseMessage.layGiaTri();

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public ChucNangResponse ChucNang(ChucNangRequest request)
        {
            ChucNangResponse response = new ChucNangResponse();
            THONG_TIN_CHUNG objThongTinChung = new THONG_TIN_CHUNG();
            ApplicationConstant.QuanTriHeThongResponseMessage responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
            ApplicationConstant.ResponseStatus responseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;
            List<BS_ResponseDetail> lstResponseDetail = new List<BS_ResponseDetail>();
            string ngayLamViec = "";

            #region Tạo thông tin chung
            objThongTinChung.MA_DON_VI = request.DeptCode;
            objThongTinChung.MA_DON_VI_GD = request.OprDeptCode;
            objThongTinChung.TEN_DANG_NHAP = request.UserName;
            objThongTinChung.MA_CHUC_NANG = request.Function.getValue();
            int iret = new BS_CoNgayLamViec().LayNgayLamViec(objThongTinChung.MA_DON_VI, out ngayLamViec);
            if (iret == 1)
            {
                objThongTinChung.NGAY_LAM_VIEC = ngayLamViec;
            }
            else
            {
                responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_KhongThanhCong;
            }
            #endregion
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                // Lấy danh sách các phòng giao dịch mà người dùng được phân quyền
                HT_CNANG htCnang = request.htCNang;
                responseStatus = new BS_QuanTriHeThong_Function().MaTranPheDuyet(request.Action, ref objThongTinChung, ref lstResponseDetail, ref htCnang, ref responseMessage);

                response.ResponseStatus = responseStatus;
                response.htCNang = htCnang;
                response.ResponseMessage = responseMessage.layGiaTri();

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }


        public PhanQuyenResponse layCNangTNangTheoListIdMenu(PhanQuyenRequest request)
        {
            PhanQuyenResponse response = new PhanQuyenResponse();
            try
            {
                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailRequest(request));

                // Kiểm tra xác thực message từ client
                if (!ValidRequest(request, response, ApplicationConstant.ValidationLevel.All))
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                response.ListChucNangTinhNang = new BS_PhanQuyen().layCNangTNangTheoListIdMenu(request.lstIdMenu);
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }
    }
}
