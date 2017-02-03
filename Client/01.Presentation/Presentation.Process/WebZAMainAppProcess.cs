using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Collections.ObjectModel;
using System.Configuration;
using Utilities.Common;
using Presentation.Process.ZAMainAppServiceRef;
using Presentation.Process.Common;
using System.ServiceModel.Description;
using System.Web;

namespace Presentation.Process
{
    public class WebZAMainAppProcess
    {
        private static UserInformation userInformation;

        /// <summary>
        /// Biến cục bộ chứa dịch vụ tiện ích của server
        /// </summary>
        private static ZAMainAppServiceClient Client { get; set; }

        /// <summary>
        /// Phương thức khởi tạo
        /// Yêu cầu 
        /// </summary>
        public WebZAMainAppProcess(UserInformation userInfo)
        {
            userInformation = userInfo;
            EndpointAddress endpointAddress = Common.WebUtilities.getEndpointAddress(ApplicationConstant.SystemService.ZAMainAppService.layGiaTri()
                , userInformation.ServerIP, userInformation.ServerPort);
            BasicHttpBinding basicHttpBinding = Common.WebUtilities.getBasicHttpBinding(ApplicationConstant.SystemService.ZAMainAppService.layGiaTri());
            Client = new ZAMainAppServiceClient(basicHttpBinding, endpointAddress);
        }

        public void createSession()
        {
            // Đọc thông tin cấu hình từ Client
            new ClientInitProcess().docThongTinCauHinhClient();
            // Kiểm tra kết nối, server, service trước khi request
            Common.WebUtilities.IsRequestAllow(userInformation.ServerIP, userInformation.ServerPort, ApplicationConstant.SystemService.ZAMainAppService.layGiaTri());

            LLogging.WriteLog("Process createSession after validation: ", LLogging.LogType.SYS, DateTime.Now.ToLongTimeString());
            // Khởi tạo và gán các giá trị cho request
            SessionRequest request = Common.WebUtilities.PrepareRequest(new SessionRequest(),userInformation);
            request.License = ClientInformation.License;
            request.Version = ClientInformation.Version;

            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_LOGIN;
            request.Action = DatabaseConstant.Action.DANG_NHAP;

            // Lấy kết quả trả về
            SessionResponse response = Client.getSession(request);

            // Kiểm tra kết quả trả về
            Common.WebUtilities.ValidResponse(request, response, userInformation);

            // Xử lý dữ liệu trả về
            string sessionId = response.SessionId;
            ClientInformation.SessionId = sessionId;
        }

        public bool doLogin(string userName, string passWord, ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.WebUtilities.IsRequestAllow(userInformation.ServerIP, userInformation.ServerPort, ApplicationConstant.SystemService.ZAMainAppService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            LoginRequest request = Common.WebUtilities.PrepareRequest(new LoginRequest(), userInformation);
            request.UserName = userName.ToUpper();
            request.PassWord = passWord;

            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_LOGIN;
            request.Action = DatabaseConstant.Action.DANG_NHAP;

            // Ghi log
            LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "Login at Client : " + userName);

            // Lấy kết quả trả về
            LoginResponse response = Client.doLogin(request);

            // Kiểm tra kết quả trả về
            Common.WebUtilities.ValidResponse(request, response, userInformation);

            // Xử lý dữ liệu trả về
            if (response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
            {
                responseMessage = response.ResponseMessage;
                return false;
            }
            else
            {                
                ClientInformation.TenDangNhap = response.NguoiSuDung.MA_DANG_NHAP;
                ClientInformation.ListChucNang = response.ListChucNang;
                if (response.NguoiSuDung != null)
                {
                    ClientInformation.IdNguoiSuDung = response.NguoiSuDung.ID;
                    ClientInformation.HoTen = response.NguoiSuDung.TEN_DAY_DU;
                    ClientInformation.MaDonVi = response.NguoiSuDung.MA_DVI_QLY;
                    ClientInformation.LoaiNguoiSuDung = response.NguoiSuDung.PHAN_LOAI_NSD;
                }
                if (response.DonVi != null)
                {
                    ClientInformation.TenDonVi = response.DonVi.TEN_GDICH;
                    ClientInformation.IdDonVi = response.DonVi.ID;
                }
                if (response.DonViRoot != null)
                {
                    ClientInformation.TenToChuc = response.DonViRoot.TEN_GDICH;
                    ClientInformation.MaToChuc = response.DonViRoot.TEN_GDICH;
                    ClientInformation.IdToChu = response.DonViRoot.ID;
                }
                ClientInformation.NgayLamViecTruoc = response.NgayLamViecTruoc;
                ClientInformation.NgayLamViecHienTai = response.NgayLamViecHienTai;
                ClientInformation.NgayLamViecSau = response.NgayLamViecSau;
                ClientInformation.MaDongNoiTe = response.MaDongNoiTe;
                ClientInformation.Company = ApplicationConstant.layGiaTri(response.Company);

                return true;
            }
        }

        public bool doLoginWithSession(string userName, string passWord, ref NGON_NGU_DTO ngonNguDTO, ref string responseMessage, UserInformation userInfomation)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.WebUtilities.IsRequestAllow(userInformation.ServerIP, userInformation.ServerPort, ApplicationConstant.SystemService.ZAMainAppService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            LoginRequest request = Common.WebUtilities.PrepareRequest(new LoginRequest(), userInformation);
            request.License = userInformation.License;
            request.Version = userInformation.Version;
            request.UserName = userName.ToUpper();
            request.PassWord = passWord;

            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_LOGIN;
            request.Action = DatabaseConstant.Action.DANG_NHAP;
            request.Company = ApplicationConstant.layDonViSuDung(userInformation.Company);

            // Ghi log
            LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "Login at Client : " + userName);

            // Lấy kết quả trả về            
            LoginResponse response = Client.doLoginWithSession(request);            

            // Kiểm tra kết quả trả về
            Common.WebUtilities.ValidResponse(request, response, userInformation);

            // Xử lý dữ liệu trả về
            if (response.ResponseStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
            {
                responseMessage = response.ResponseMessage;
                return false;
            }
            else
            {
                responseMessage = response.ResponseMessage;
                string sessionId = response.SessionId;
                userInformation.SessionId = sessionId;

                userInformation.TenDangNhap = response.NguoiSuDung.MA_DANG_NHAP;
                userInformation.ListChucNang = response.ListChucNang;
                if (response.NguoiSuDung != null)
                {
                    userInformation.IdNguoiSuDung = response.NguoiSuDung.ID;
                    userInformation.HoTen = response.NguoiSuDung.TEN_DAY_DU;
                    userInformation.MaDonViQuanLy = response.NguoiSuDung.MA_DVI_QLY;
                    userInformation.LoaiNguoiSuDung = response.NguoiSuDung.PHAN_LOAI_NSD;
                }
                if (response.DonViGiaoDich != null)
                {
                    userInformation.IdDonViGiaoDich = response.DonViGiaoDich.ID;                    
                    userInformation.MaDonViGiaoDich = response.DonViGiaoDich.MA_DVI;
                    userInformation.TenDonViGiaoDich = response.DonViGiaoDich.TEN_GDICH;
                    userInformation.PhuongPhapHachToan = response.DonViGiaoDich.MA_HACH_TOAN;
                }
                if (response.DonVi != null)
                {   
                    userInformation.IdDonVi = response.DonVi.ID;
                    userInformation.MaDonVi = response.DonVi.MA_DVI;
                    userInformation.TenDonVi = response.DonVi.TEN_GDICH;
                }
                if (response.DonViQuanLy != null)
                {
                    userInformation.IdDonViQuanLy = response.DonViQuanLy.ID;
                    userInformation.MaDonViQuanLy = response.DonViQuanLy.MA_DVI;
                    userInformation.TenDonViQuanLy = response.DonViQuanLy.TEN_GDICH;
                }
                if (response.DonViRoot != null)
                {   
                    userInformation.IdToChu = response.DonViRoot.ID;
                    userInformation.MaToChuc = response.DonViRoot.MA_DVI;
                    userInformation.TenToChuc = response.DonViRoot.TEN_GDICH;
                }                
                if (response.ListPhongGD != null)
                {
                    userInformation.ListPhongGD = response.ListPhongGD.ToList();
                }
                if (response.NgonNguDTO != null)
                {
                    ngonNguDTO = response.NgonNguDTO;
                }
                userInformation.NgayLamViecTruoc = response.NgayLamViecTruoc;
                userInformation.NgayLamViecHienTai = response.NgayLamViecHienTai;
                userInformation.NgayLamViecSau = response.NgayLamViecSau;
                userInformation.MaDongNoiTe = response.MaDongNoiTe;
                userInformation.MaQuocGiaBanDia = response.MaQuocGiaBanDia;
                userInformation.Company = ApplicationConstant.layGiaTri(response.Company);
                return true;
            }
        }


        /// <summary>
        /// Đăng xuất khỏi hệ thống
        /// </summary>
        public void doLogout()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.WebUtilities.IsRequestAllow(userInformation.ServerIP, userInformation.ServerPort, ApplicationConstant.SystemService.ZAMainAppService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            LogoutRequest request = Common.WebUtilities.PrepareRequest(new LogoutRequest(), userInformation);

            request.Module = DatabaseConstant.Module.QTHT;
            request.Function = DatabaseConstant.Function.HT_LOGOUT;
            request.Action = DatabaseConstant.Action.DANG_XUAT;

            // Ghi log
            LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "Logout at Client : " + request.UserName);

            // Lấy kết quả trả về
            LogoutResponse response = Client.doLogout(request);

            // Kiểm tra kết quả trả về
            Common.WebUtilities.ValidResponse(request, response, userInformation);
        }
    }
}
