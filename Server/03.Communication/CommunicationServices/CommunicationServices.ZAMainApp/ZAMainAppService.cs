using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessServices.ZAMainApp;
using DataModel.EntityFramework;
using Hosts.Startup;
using Utilities.Common;
using CommunicationMessages.Base.MessageBases;
using CommunicationServices.ZAMainApp.Messages;
using BusinessServices.QuanTriHeThong;
using BusinessServices.DanhMuc;
using BusinessServices.Utilities;
using System.ServiceModel;
using CommunicationContracts.Base.ContractBases;
using System.ServiceModel.Activation;
using BusinessServices.Utilities.DTO;

namespace CommunicationServices.ZAMainApp
{
    [ServiceBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ZAMainAppService : ServiceBase, IZAMainAppService
    {
        // Session state variables
        // public string SessionId;
        // public string UserName;
        private bool CheckVersion = true;

        public string returnHello()
        {
            return "Hello";
        }

        public SessionResponse getSession(SessionRequest request)
        {
            SessionResponse response = new SessionResponse();
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

                // 1. Kiểm tra invalid license >> quan niệm là không thành công
                // Cài đặt cụ thể hàm check license
                string inputLicense = request.License;
                bool isValidLicense = (inputLicense != null && inputLicense.Equals("123456")) ? true : false;
                if (!isValidLicense)
                {
                    response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_InvalidLicense.layGiaTri();
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    
                    return response;
                }

                // 2. Kiểm tra invalid version >> quan niệm là không thành công
                // Cài đặt cụ thể hàm check version
                string inputVersion = request.Version;
                List<HT_PBAN_MAPPING> listHtPbanMapping = new List<HT_PBAN_MAPPING>();
                new BS_CoHeThong().LayDanhSachPhienBanClientPhuHop(HostInformation.Version, ref listHtPbanMapping);
                List<string> listClientVersion = listHtPbanMapping.Select(e => e.MA_PBAN_CLIENT).ToList();
                bool isValidVersion = (inputVersion != null && listClientVersion != null && listClientVersion.Contains(inputVersion)) ? true : false;
                if (!isValidVersion)
                {
                    response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_InvalidVersion.layGiaTri();
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    
                    return response;
                }
                                
                // Sinh sessionid
                string sessionId = Guid.NewGuid().ToString();
                response.SessionId = sessionId;
                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_ThanhCong.layGiaTri();
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));

                return response;
            }
            catch (Exception ex)
            {
                /// Ghi log lỗi
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                response.ExceptionObject = LCollection.Serialize(ex);
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_KhongThanhCong.layGiaTri();
                response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            return response;
        }

        public LoginResponse doLogin(LoginRequest request)
        {
            LoginResponse response = new LoginResponse();
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

                // Kiểm tra thông tin đăng nhập trong CSDL
                BS_ZAMainApp loginService = new BS_ZAMainApp();
                HT_NSD htNsd = loginService.doLogin(request.UserName, request.PassWord);

                // Nếu không tìm được người dùng trong CSDL
                if (htNsd == null)
                {
                    response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_SaiThongTinDangNhap.layGiaTri();
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                // Nếu tìm được người dùng trong CSDL
                else
                {
                    // 1. Yêu cầu đổi mật khẩu lần đầu đăng nhập >> quan niệm là không thành công
                    if (htNsd.TDOI_MKHAU == null || htNsd.TDOI_MKHAU.Equals(BusinessConstant.YeuCauDoiMatKhau.CHUA_THAY_DOI.layGiaTri()))
                    {
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_YeuCauDoiMatKhauLanDau.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }

                    // 2. Người dùng phải đổi mật khẩu (đến hạn) >> quan niệm là không thành công
                    string ngayHienTai = LDateTime.GetCurrentDate("yyyyMMdd");
                    string ngayDoiMatKhau = htNsd.NGAY_DOI_MKHAU;
                    int tgianDoiMkhau = (int)htNsd.TGIAN_DOI_MKHAU;
                    string tgianDoiMkhauDviTinh = htNsd.TGIAN_DOI_DVI_TINH;

                    string ngayPhaiDoiMatKhau = tgianDoiMkhauDviTinh != null ? 
                        !tgianDoiMkhauDviTinh.Equals(BusinessConstant.TAN_SUAT.NA.layGiaTri()) ?
                        LDateTime.DateToString(LDateTime.PlusDaysComposite(LDateTime.StringToDate(ngayDoiMatKhau, "yyyyMMdd"), tgianDoiMkhau, tgianDoiMkhauDviTinh), "yyyyMMdd") : 
                        "" :
                        "" ;

                    if (!(ngayPhaiDoiMatKhau.Equals("")) && ngayHienTai.CompareTo(ngayPhaiDoiMatKhau) >= 0)
                    {
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_YeuCauDoiMatKhau.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }

                    // 3. Tài khoản người dùng chưa được duyệt >> quan niệm là không thành công
                    if (htNsd.TTHAI_NVU == null  || !htNsd.TTHAI_NVU.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    {
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_TaiKhoanChuaDuocDuyet.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }

                    // 4. Tài khoản người dùng chưa tới ngày hiệu lực >> quan niệm là không thành công
                    string ngayHieuLuc = htNsd.NGAY_HIEU_LUC;
                    if (ngayHieuLuc == null || ngayHienTai.CompareTo(ngayHieuLuc) < 0)
                    {
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_ChuaToiNgayHieuLuc.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }

                    // 5. Tài khoản người dùng không được sử dụng >> quan niệm là không thành công
                    if (htNsd.TTHAI_BGHI == null || !htNsd.TTHAI_BGHI.Equals(BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri()))
                    {
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_TaiKhoanKhongDuocSuDung.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }

                    // 6. Tài khoản người dùng hết hạn >> quan niệm là không thành công                    
                    string ngayHetHan = htNsd.NGAY_HET_HAN;
                    if (ngayHienTai == null || ngayHienTai.CompareTo(ngayHetHan) >= 0)
                    {
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_TaiKhoanHetHan.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }

                    // 7. Người dùng bị khóa >> quan niệm là không thành công
                    // CSDL chưa có thông tin cho chức năng khóa/mở khóa người dùng
                    if (htNsd.TTHAI_BGHI == "")
                    {
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_TaiKhoanBiKhoa.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }

                    //8. Kiểm tra địa chỉ MAC và IP
                    string MAC = response.ClientMAC;
                    string IP = response.ClientIP;


                    // Lấy danh sách chức năng của người dùng
                    // Và chuyển thành kiểu ListChucNang
                    // Kiểm tra timing
                    BS_QuanTriHeThong quanTriHeThongService = new BS_QuanTriHeThong();
                    DataSet dsTaiNguyen = quanTriHeThongService.layDanhSachChucNangTheoUser(request.UserName, htNsd.MA_DVI_QLY);
                    List<ChucNangDto> lstChucNang = new List<ChucNangDto>();
                    if (dsTaiNguyen != null && dsTaiNguyen.Tables[0].Rows.Count > 0)
                    {
                        Entities entites = ContextFactory.GetInstance();
                        foreach (DataRow r in dsTaiNguyen.Tables[0].Rows)
                        {
                            ChucNangDto chucNangDto = new ChucNangDto();
                            chucNangDto.IDChucNang = r["ID"].ToString().StringToInt32();
                            if (!string.IsNullOrEmpty(r["ID_CNANG_CHA"].ToString()))
                                chucNangDto.IDChucNangCha = r["ID_CNANG_CHA"].ToString().StringToInt32();
                            chucNangDto.STT = Convert.ToInt32(r["SO_TTU"]);
                            chucNangDto.TieuDe = r["TIEU_DE"].ToString();
                            chucNangDto.ThuocTinh = r["THUOC_TINH"].ToString();
                            chucNangDto.BieuTuong = r["BTUONG_TEN"].ToString();
                            chucNangDto.ChucNang = r["CHUC_NANG"].ToString();
                            chucNangDto.PhuongThuc = r["PHUONG_THUC"].ToString();
                            chucNangDto.Quyen = r["QUYEN"].ToString().StringToInt32();
                            chucNangDto.FormCase = r["FORMCASE"].ToString();
                            chucNangDto.MenuHelp = r["MENU_HELP"].ToString();
                            chucNangDto.MenuType = r["MENU_TYPE"].ToString();
                            chucNangDto.UrlGroup = r["URL_GROUP"].ToString();
                            chucNangDto.Url = r["URL"].ToString();
                            chucNangDto.UrlType = r["URL_TYPE"].ToString();
                            chucNangDto.UrlICon = r["URL_ICON"].ToString();
                            chucNangDto.UrlHelp = r["URL_HELP"].ToString();
                            chucNangDto.UrlCat = r["URL_CAT"].ToString();

                            //HT_MENU_TSO cnangThamSo = entites.HT_MENU_TSO.FirstOrDefault(e => e.MA_MENU.Equals(chucNangDto.TieuDe.Replace("MENU.","")) && e.ID_TTINH == 5);
                            //if(cnangThamSo!=null)
                            //    chucNangDto.FormCase = cnangThamSo.GIA_TRI_TT;

                            List<TinhNangDto> lstTinhNang = new List<TinhNangDto>();
                            string[] MaTinhNangs = r["TINH_NANG"].ToString().Split('#');
                            if (MaTinhNangs.Length > 0)
                            {
                                foreach (string maTinhNang in MaTinhNangs)
                                {
                                    if (!maTinhNang.IsNullOrEmptyOrSpace())
                                    {
                                        TinhNangDto tinhNangDto = new TinhNangDto();
                                        tinhNangDto.MaTinhNang = maTinhNang;
                                        lstTinhNang.Add(tinhNangDto);
                                        if (maTinhNang.Equals(DatabaseConstant.Action.TOAN_QUYEN.getValue()))
                                        {
                                            lstTinhNang = new List<TinhNangDto>();
                                            lstTinhNang.Add(tinhNangDto);
                                            break;
                                        }
                                    }
                                }
                            }
                            chucNangDto.lstTinhNang = lstTinhNang;
                            lstChucNang.Add(chucNangDto);
                        }
                        response.ListChucNang = lstChucNang;
                    }
                    else 
                    {
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_LoadUserRolesFailed.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }
                    

                    // Lấy thông tin đơn vị
                    BS_DM_DON_VI bsDmDonVi = new BS_DM_DON_VI();
                    DM_DON_VI dmDonVi = bsDmDonVi.getDonViByMaDonVi(htNsd.MA_DVI_QLY);
                    if (dmDonVi == null)
                    {
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_LoadUserBranchesFailed.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }

                    // Lấy thông tin đơn vị root
                    DM_DON_VI dmDonViRoot = bsDmDonVi.getDonViRootByMaDonVi(htNsd.MA_DVI_QLY);
                    if (dmDonViRoot == null)
                    {
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_LoadUserBranchesFailed.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }

                    // Lấy thông tin ngày làm việc
                    HT_NGAY_LVIEC htNgayLamViec = quanTriHeThongService.getNgayLamViecTheoDonVi(htNsd.MA_DVI_QLY);
                    if (htNgayLamViec == null)
                    {
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_LoadBusinessDateFailed.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }

                    // Lấy thông tin tham số hệ thống cần thiết
                    // Tiền tạm
                    string maDongNoiTe = new BS_CoThamSo().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_MA_NOITE);

                    // Gán các thông tin response nếu thành công
                    response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;
                    response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_ThanhCong.layGiaTri();
                    response.NguoiSuDung = htNsd;
                    response.DonVi = dmDonVi;
                    response.DonViRoot = dmDonViRoot;
                    response.NgayLamViecTruoc = htNgayLamViec != null ? htNgayLamViec.NGAY_TRUOC : null;
                    response.NgayLamViecHienTai = htNgayLamViec != null ? htNgayLamViec.NGAY_LVIEC : null;
                    response.NgayLamViecSau = htNgayLamViec != null ? htNgayLamViec.NGAY_TTHEO : null;
                    response.MaDongNoiTe = maDongNoiTe;
                    response.Company = ApplicationConstant.layDonViSuDung(HostInformation.Company);
                    // Kiểm tra thông tin session phù hợp
                    // Người dùng đã được đăng nhập tại một Client khác trước đó >> quan niệm là thành công, gửi kèm message thông báo
                    if (DuplicateUserSession(request.UserName))
                    {
                        // Xóa thông tin phiên làm việc trước đó của người dùng
                        if (DeleteUserSession(request.UserName))
                        {
                            response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_OverSession.layGiaTri();
                        }
                        else
                        {
                            response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                            response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_DefineSessionFailed.layGiaTri();
                            LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                            return response;
                        }
                    }

                    // Tạo session cho người dùng
                    if (CreateUserSession(htNsd, request))
                    {
                        
                    }
                    else
                    {
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_DefineSessionFailed.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }

                    // Lock các dữ liệu về phiên làm việc của người dùng
                    // HT_NSD, DM_DON_VI
                    DataInformation nguoiDung = new DataInformation();
                    nguoiDung.TenDangNhap = htNsd.MA_DANG_NHAP;
                    nguoiDung.MaDonVi = dmDonVi.MA_DVI;
                    nguoiDung.Module = DatabaseConstant.Module.QTHT;
                    nguoiDung.Function = DatabaseConstant.Function.HT_DANG_NHAP;
                    nguoiDung.Table = DatabaseConstant.Table.HT_NSD;
                    nguoiDung.Action = DatabaseConstant.Action.XU_LY;
                    nguoiDung.Id = htNsd.ID;
                    nguoiDung.lockDataLevel = ApplicationConstant.LockDataLevel.Delete;
                    nguoiDung.lockDataType = ApplicationConstant.LockDataType.Application;

                    DataInformation donVi = new DataInformation();
                    donVi.TenDangNhap = htNsd.MA_DANG_NHAP;
                    donVi.MaDonVi = dmDonVi.MA_DVI;
                    donVi.Module = DatabaseConstant.Module.QTHT;
                    donVi.Function = DatabaseConstant.Function.HT_DANG_NHAP;
                    donVi.Table = DatabaseConstant.Table.DM_DON_VI;
                    donVi.Action = DatabaseConstant.Action.XU_LY;
                    donVi.Id = dmDonVi.ID;
                    donVi.lockDataLevel = ApplicationConstant.LockDataLevel.Delete;
                    donVi.lockDataType = ApplicationConstant.LockDataType.Application;

                    new BS_Utilities().LockData(nguoiDung);
                    new BS_Utilities().LockData(donVi);
                    
                    // Ghi log thành công
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }
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

        public LoginResponse doLoginWithSession(LoginRequest request)
        {
            LoginResponse response = new LoginResponse();
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

                // 0. Kiểm tra CompanyCode >> quan niệm là không thành công
                string clientCompany = ApplicationConstant.layGiaTri(request.Company);
                string serverCompany = HostInformation.Company;
                bool isValidCompany = (clientCompany != null && clientCompany.Equals(serverCompany)) ? true : false;
                if (!isValidCompany)
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "!isValidCompany");
                    response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_InvalidCompany.layGiaTri();
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));

                    return response;
                }

                // 1. Kiểm tra invalid license >> quan niệm là không thành công
                // Cài đặt cụ thể hàm check license
                string inputLicense = request.License;
                bool isValidLicense = (inputLicense != null && inputLicense.Equals("123456")) ? true : false;
                if (!isValidLicense)
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "!isValidLicense");
                    response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_InvalidLicense.layGiaTri();
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));

                    return response;
                }

                // 2. Kiểm tra invalid version >> quan niệm là không thành công
                // Cài đặt cụ thể hàm check version
                string inputVersion = request.Version;
                List<string> listClientVersion = new List<string>();
                if (request.ClientType.Equals(ApplicationConstant.ClientType.DESKTOP))
                {                    
                    // Client version init
                    List<HT_PBAN_MAPPING> listHtPbanMapping = new List<HT_PBAN_MAPPING>();
                    new BS_CoHeThong().LayDanhSachPhienBanClientPhuHop(HostInformation.Version, ref listHtPbanMapping);
                    listClientVersion = listHtPbanMapping.Select(e => e.MA_PBAN_CLIENT).ToList();
                    bool isValidVersion = (inputVersion != null && listClientVersion != null && listClientVersion.Contains(inputVersion)) ? true : false;
                    if (!isValidVersion && CheckVersion)
                    {
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "!isValidVersion");
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_InvalidVersion.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));

                        return response;
                    }
                }

                // Kiểm tra thông tin đăng nhập trong CSDL
                BS_ZAMainApp loginService = new BS_ZAMainApp();
                HT_NSD htNsd = loginService.doLogin(request.UserName, request.PassWord);

                // Nếu không tìm được người dùng trong CSDL
                if (htNsd == null)
                {
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "!htNsd");
                    response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_SaiThongTinDangNhap.layGiaTri();
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }

                // Nếu tìm được người dùng trong CSDL
                else
                {
                    BS_QuanTriHeThong quanTriHeThongService = new BS_QuanTriHeThong();
                    // Lấy thông tin ngày làm việc
                    HT_NGAY_LVIEC htNgayLamViec = quanTriHeThongService.getNgayLamViecTheoDonVi(htNsd.MA_DVI_QLY);
                    if (htNgayLamViec == null)
                    {
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "!htNgayLamViec");
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_LoadBusinessDateFailed.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }

                    //string ngayHienTai = LDateTime.GetCurrentDate("yyyyMMdd");
                    string ngayHienTai = htNgayLamViec.NGAY_LVIEC;

                    // 3. Tài khoản người dùng chưa được duyệt >> quan niệm là không thành công
                    if (htNsd.TTHAI_NVU == null || !htNsd.TTHAI_NVU.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    {
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "!htNsd.TTHAI_NVU");
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_TaiKhoanChuaDuocDuyet.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }

                    // 4. Tài khoản người dùng chưa tới ngày hiệu lực >> quan niệm là không thành công
                    string ngayHieuLuc = htNsd.NGAY_HIEU_LUC;
                    if (ngayHieuLuc == null || ngayHienTai.CompareTo(ngayHieuLuc) < 0)
                    {
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "!ngayHieuLuc");
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_ChuaToiNgayHieuLuc.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }

                    // 5. Tài khoản người dùng không xác định >> quan niệm là không thành công
                    if (htNsd.TINH_TRANG == null || htNsd.TINH_TRANG == "")
                    {
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "!htNsd.TINH_TRANG.NULL");
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_TaiKhoanKhongDuocSuDung.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }

                    // 5. Tài khoản người dùng không được sử dụng >> quan niệm là không thành công
                    if (htNsd.TINH_TRANG.Equals(BusinessConstant.TrangThaiNguoiDung.KHONG_SU_DUNG.layGiaTri()))
                    {
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "!htNsd.TINH_TRANG.KHONG_SU_DUNG");
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_TaiKhoanKhongDuocSuDung.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }

                    // 6. Tài khoản người dùng hết hạn >> quan niệm là không thành công                    
                    string ngayHetHan = htNsd.NGAY_HET_HAN;
                    if (ngayHetHan != null && ngayHienTai.CompareTo(ngayHetHan) >= 0)
                    {
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "!ngayHetHan");
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_TaiKhoanHetHan.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }

                    // 7. Người dùng bị khóa >> quan niệm là không thành công
                    if (htNsd.TINH_TRANG.Equals(BusinessConstant.TrangThaiNguoiDung.TAM_KHOA.layGiaTri()))
                    {
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "!htNsd.TINH_TRANG.TAM_KHOA");
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_TaiKhoanBiKhoa.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }

                    //8. Kiểm tra địa chỉ MAC và IP
                    bool hopLe = new BS_ZAMainApp().KiemTraTruyCap(htNsd, request.ClientMAC, request.ClientIP);
                    if (hopLe == false)
                    {
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "!hopLe");             
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_DiaChiKhongHopLe.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }

                    // 1. Yêu cầu đổi mật khẩu lần đầu đăng nhập >> quan niệm là không thành công
                    if (htNsd.TDOI_MKHAU == null || htNsd.TDOI_MKHAU.Equals(BusinessConstant.YeuCauDoiMatKhau.CHUA_THAY_DOI.layGiaTri()))
                    {
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "!htNsd.TDOI_MKHAU");             
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_YeuCauDoiMatKhauLanDau.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }

                    // 2. Người dùng phải đổi mật khẩu (đến hạn) >> quan niệm là không thành công                    
                    string ngayDoiMatKhau = htNsd.NGAY_DOI_MKHAU;
                    int tgianDoiMkhau = (int)htNsd.TGIAN_DOI_MKHAU;
                    string tgianDoiMkhauDviTinh = htNsd.TGIAN_DOI_DVI_TINH;

                    string ngayPhaiDoiMatKhau = tgianDoiMkhauDviTinh != null ?
                        !tgianDoiMkhauDviTinh.Equals(BusinessConstant.TAN_SUAT.NA.layGiaTri()) ?
                        LDateTime.DateToString(LDateTime.PlusDaysComposite(LDateTime.StringToDate(ngayDoiMatKhau, "yyyyMMdd"), tgianDoiMkhau, tgianDoiMkhauDviTinh), "yyyyMMdd") :
                        "" :
                        "";
                    if (!(ngayPhaiDoiMatKhau.Equals("")) && ngayHienTai.CompareTo(ngayPhaiDoiMatKhau) >= 0)
                    {
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "!ngayPhaiDoiMatKhau");             
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_YeuCauDoiMatKhau.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }                    

                    // Sinh sessionid
                    string sessionId = Guid.NewGuid().ToString();
                    response.SessionId = sessionId;
                    request.SessionId = sessionId;


                    //Lấy ngôn ngữ dưới server                    
                    NGON_NGU_DTO objNgonNguDTO = null;
                    
                    int ret = new BS_CoHeThong().LayNgonNgu(clientCompany, ref objNgonNguDTO, request.PhienBanResource, request.PhienBanMessage);
                    if (ret != 1)
                    {
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "!objNgonNguDTO");
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_LayNgonNguKhongThanhCong.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }      
                                  

                    //Lay show config
                    DSACH_SHOW_CONFIG objDSachShow = null;
                    
                    ret = new BS_CoHeThong().LayDanhSachShowConfig(out objDSachShow);
                    if (ret != 1)
                    {
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "!objDSachShow");
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_LayShowConfigKhongThanhCong.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }
                      

                    // Lấy danh sách chức năng của người dùng
                    // Và chuyển thành kiểu ListChucNang
                    // Kiểm tra timing
                    //BS_QuanTriHeThong quanTriHeThongService = new BS_QuanTriHeThong();
                    DataSet dsTaiNguyen = quanTriHeThongService.layDanhSachChucNangTheoUser(request.UserName, htNsd.MA_DVI_QLY);
                    List<ChucNangDto> lstChucNang = new List<ChucNangDto>();
                    if (dsTaiNguyen != null && dsTaiNguyen.Tables[0].Rows.Count > 0)
                    {
                        Entities entites = ContextFactory.GetInstance();
                        foreach (DataRow r in dsTaiNguyen.Tables[0].Rows)
                        {
                            ChucNangDto chucNangDto = new ChucNangDto();
                            chucNangDto.IDChucNang = r["ID"].ToString().StringToInt32();
                            if (!string.IsNullOrEmpty(r["ID_CNANG_CHA"].ToString()))
                                chucNangDto.IDChucNangCha = r["ID_CNANG_CHA"].ToString().StringToInt32();
                            chucNangDto.STT = Convert.ToInt32(r["SO_TTU"]);
                            chucNangDto.TieuDe = r["TIEU_DE"].ToString();
                            chucNangDto.ThuocTinh = r["THUOC_TINH"].ToString();
                            chucNangDto.BieuTuong = r["BTUONG_TEN"].ToString();
                            chucNangDto.ChucNang = r["CHUC_NANG"].ToString();
                            chucNangDto.PhuongThuc = r["PHUONG_THUC"].ToString();
                            chucNangDto.Quyen = r["QUYEN"].ToString().StringToInt32();
                            chucNangDto.FormCase = r["FORMCASE"].ToString();
                            chucNangDto.MenuHelp = r["MENU_HELP"].ToString();
                            chucNangDto.MenuType = r["MENU_TYPE"].ToString();
                            chucNangDto.UrlGroup = r["URL_GROUP"].ToString();
                            chucNangDto.Url = r["URL"].ToString();
                            chucNangDto.UrlType = r["URL_TYPE"].ToString();
                            chucNangDto.UrlICon = r["URL_ICON"].ToString();
                            chucNangDto.UrlHelp = r["URL_HELP"].ToString();
                            chucNangDto.UrlCat = r["URL_CAT"].ToString();
                            //HT_MENU_TSO cnangThamSo = entites.HT_MENU_TSO.FirstOrDefault(e => e.MA_MENU.Equals(chucNangDto.TieuDe.Replace("MENU.","")) && e.ID_TTINH == 5);
                            //if(cnangThamSo!=null)
                            //    chucNangDto.FormCase = cnangThamSo.GIA_TRI_TT;

                            List<TinhNangDto> lstTinhNang = new List<TinhNangDto>();
                            string[] MaTinhNangs = r["TINH_NANG"].ToString().Split('#');
                            if (MaTinhNangs.Length > 0)
                            {
                                foreach (string maTinhNang in MaTinhNangs)
                                {
                                    if (!maTinhNang.IsNullOrEmptyOrSpace())
                                    {
                                        TinhNangDto tinhNangDto = new TinhNangDto();
                                        tinhNangDto.MaTinhNang = maTinhNang;
                                        lstTinhNang.Add(tinhNangDto);
                                        if (maTinhNang.Equals(DatabaseConstant.Action.TOAN_QUYEN.getValue()))
                                        {
                                            lstTinhNang = new List<TinhNangDto>();
                                            lstTinhNang.Add(tinhNangDto);
                                            break;
                                        }
                                    }
                                }
                            }
                            chucNangDto.lstTinhNang = lstTinhNang;
                            lstChucNang.Add(chucNangDto);
                        }
                        response.ListChucNang = lstChucNang;
                    }
                    else
                    {
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "!dsTaiNguyen");
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_LoadUserRolesFailed.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }


                    // Lấy thông tin đơn vị
                    BS_DM_DON_VI bsDmDonVi = new BS_DM_DON_VI();
                    DM_DON_VI dmDonVi = bsDmDonVi.getDonViByMaDonVi(htNsd.MA_DVI_QLY);
                    if (dmDonVi == null)
                    {
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "!dmDonVi");
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_LoadUserBranchesFailed.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }

                    // Lấy thông tin đơn vị root
                    DM_DON_VI dmDonViRoot = bsDmDonVi.getDonViRootByMaDonVi(htNsd.MA_DVI_QLY);
                    if (dmDonViRoot == null)
                    {
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "!dmDonViRoot");
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_LoadUserBranchesFailed.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }

                    // Lấy thông tin phòng giao dịch
                    DM_DON_VI dmDonViGiaoDich = new DM_DON_VI();
                    //dmDonViGiaoDich.MA_DVI = dmDonVi.MA_DVI + "01";

                    // Lấy danh sách các phòng giao dịch mà người dùng được phân quyền
                    //string maLoaiPhamVi = BusinessConstant.LoaiPhamViDuLieu.PHONG_GIAO_DICH.layMaPhamVi();
                    //List<BS_PhamVi> ListPhamVi = new BS_QuanTriHeThong().layPhamViTheoIdNSDVaMaLoaiPhamVi(htNsd.ID, maLoaiPhamVi);
                    //List<DM_DON_VI> listPhongGD = new List<DM_DON_VI>();
                    //foreach (BS_PhamVi item in ListPhamVi)
                    //{
                    //    DM_DON_VI dmPhongGD = new DM_DON_VI();
                    //    dmPhongGD.ID = item.IdPvi;
                    //    dmPhongGD.MA_DVI = item.MaPvi;
                    //    dmPhongGD.TEN_GDICH = item.TenPvi;
                    //    dmPhongGD.MA_HACH_TOAN = item.PhuongPhapHachToan;
                    //    listPhongGD.Add(dmPhongGD);
                    //}

                    string vpgd = DatabaseConstant.ToChucDonVi.VPGD.getValue();
                    string pgd = DatabaseConstant.ToChucDonVi.PGD.getValue();
                    List<DM_DON_VI> ListDonVi = new BS_CoNguoiSuDung().LayPhamViDuLieuDonVi(htNsd.MA_DANG_NHAP, htNsd.MA_DVI_QLY);
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
                                dmPhongGD.MA_TINHTP = item.MA_TINHTP;
                                listPhongGD.Add(dmPhongGD);
                            }
                        }
                    }


                    //// Lấy thông tin ngày làm việc
                    //HT_NGAY_LVIEC htNgayLamViec = quanTriHeThongService.getNgayLamViecTheoDonVi(htNsd.MA_DVI_QLY);
                    //if (htNgayLamViec == null)
                    //{
                    //    response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    //    response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_LoadBusinessDateFailed.layGiaTri();
                    //    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    //    return response;
                    //}

                    // Lấy thông tin tham số hệ thống cần thiết
                    // Tiền nội tệ
                    string maDongNoiTe = new BS_CoThamSo().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_MA_NOITE);
                    // Quốc gia bản địa
                    string maQuocGiaBanDia = new BS_CoThamSo().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_MA_QUOC_GIA);

                    // Gán các thông tin response nếu thành công
                    response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;
                    response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_ThanhCong.layGiaTri();
                    response.NguoiSuDung = htNsd;
                    response.DonViGiaoDich = dmDonViGiaoDich != null ? dmDonViGiaoDich : null;
                    response.DonVi = dmDonVi;
                    response.DonViQuanLy = dmDonVi;
                    response.DonViRoot = dmDonViRoot;
                    response.ListPhongGD = listPhongGD != null ? listPhongGD : null;
                    response.NgayLamViecTruoc = htNgayLamViec != null ? htNgayLamViec.NGAY_TRUOC : null;
                    response.NgayLamViecHienTai = htNgayLamViec != null ? htNgayLamViec.NGAY_LVIEC : null;
                    response.NgayLamViecSau = htNgayLamViec != null ? htNgayLamViec.NGAY_TTHEO : null;
                    response.MaDongNoiTe = maDongNoiTe;
                    response.MaQuocGiaBanDia = maQuocGiaBanDia;
                    response.Company = ApplicationConstant.layDonViSuDung(HostInformation.Company);
                    response.NgonNguDTO = objNgonNguDTO;
                    response.ShowConfigDTO = objDSachShow;

                    // Kiểm tra thông tin session phù hợp
                    // Người dùng đã được đăng nhập tại một Client khác trước đó >> quan niệm là thành công, gửi kèm message thông báo
                    if (DuplicateUserSession(request.UserName))
                    {
                        // Xóa thông tin phiên làm việc trước đó của người dùng
                        if (DeleteUserSession(request.UserName))
                        {
                            response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_OverSession.layGiaTri();
                        }
                        else
                        {
                            LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "!DuplicateUserSession");
                            response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                            response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_DefineSessionFailed.layGiaTri();
                            LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                            return response;
                        }
                    }

                    // Tạo session cho người dùng
                    if (CreateUserSession(htNsd, request))
                    {

                    }
                    else
                    {
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "!CreateUserSession");
                        response.ResponseStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        response.ResponseMessage = ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_DefineSessionFailed.layGiaTri();
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                        return response;
                    }

                    // Lock các dữ liệu về phiên làm việc của người dùng
                    // HT_NSD, DM_DON_VI
                    DataInformation nguoiDung = new DataInformation();
                    nguoiDung.TenDangNhap = htNsd.MA_DANG_NHAP;
                    nguoiDung.MaDonVi = dmDonVi.MA_DVI;
                    nguoiDung.Module = DatabaseConstant.Module.QTHT;
                    nguoiDung.Function = DatabaseConstant.Function.HT_DANG_NHAP;
                    nguoiDung.Table = DatabaseConstant.Table.HT_NSD;
                    nguoiDung.Action = DatabaseConstant.Action.XU_LY;
                    nguoiDung.Id = htNsd.ID;
                    nguoiDung.lockDataLevel = ApplicationConstant.LockDataLevel.Delete;
                    nguoiDung.lockDataType = ApplicationConstant.LockDataType.Application;

                    DataInformation donVi = new DataInformation();
                    donVi.TenDangNhap = htNsd.MA_DANG_NHAP;
                    donVi.MaDonVi = dmDonVi.MA_DVI;
                    donVi.Module = DatabaseConstant.Module.QTHT;
                    donVi.Function = DatabaseConstant.Function.HT_DANG_NHAP;
                    donVi.Table = DatabaseConstant.Table.DM_DON_VI;
                    donVi.Action = DatabaseConstant.Action.XU_LY;
                    donVi.Id = dmDonVi.ID;
                    donVi.lockDataLevel = ApplicationConstant.LockDataLevel.Delete;
                    donVi.lockDataType = ApplicationConstant.LockDataType.Application;

                    new BS_Utilities().LockData(nguoiDung);
                    new BS_Utilities().LockData(donVi);

                    // Cảnh báo cập nhật version
                    if (response.ClientType.Equals(ApplicationConstant.ClientType.DESKTOP))
                    {
                        string latestClientVersion = listClientVersion.LastOrDefault();
                        if (!inputVersion.Equals(latestClientVersion) && CheckVersion)
                        {
                            response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;
                            response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_NotLatestVersion.layGiaTri();
                        }
                    }

                    // Truongnx: 20150608 - Cap nhat du lieu sai (BinhKhanh)
                    //if (request.Company.Equals(ApplicationConstant.DonViSuDung.BINHKHANH))
                    //{
                    //    int numInvalidData = new BS_ZAMainApp().CapNhatKheUocDaTatToan(request.Company);
                    //    int numInvalidDataSoTK = new BS_ZAMainApp().CapNhatSoTietKiem(request.Company);
                    //}

                    // Ghi log thành công
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "response.ResponseStatus: " + response.ResponseStatus.ToString());
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, detailResponse(response));
                    return response;
                }
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

        public LogoutResponse doLogout(LogoutRequest request)
        {
            LogoutResponse response = new LogoutResponse();
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

                // Xóa session của người dùng
                DeleteUserSession(request.UserName);

                // Unlock các dữ liệu của người dùng
                new BS_Utilities().UnlockDataFromUser(request.UserName, request.DeptCode);

                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_ThanhCong.layGiaTri();

                // Ghi log thành công
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

        public SessionResponse doCheckSession(SessionRequest request)
        {
            SessionResponse response = new SessionResponse();
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

                response.ResponseStatus = ApplicationConstant.ResponseStatus.THANH_CONG;
                response.ResponseMessage = ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_ThanhCong.layGiaTri();

                // Ghi log thành công
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

